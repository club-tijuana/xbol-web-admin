using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Odasoft.XBOL.Common.Options;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Yarp.ReverseProxy.Forwarder;

namespace Odasoft.XBOL.AdminPortal.Extensions;

public static class WorkerDashboardProxyEndpointConfiguration
{
    public static IEndpointRouteBuilder MapWorkerDashboardProxyEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/worker").RequireAuthorization();

        group.MapMethods("", WorkerDashboardProxy.Methods, ProxyRootAsync);
        group.MapMethods("/{**path}", WorkerDashboardProxy.Methods, ProxyPathAsync);

        return endpoints;
    }

    private static Task ProxyRootAsync(
        HttpContext context,
        IHttpForwarder forwarder,
        IOptions<AdminApiClientOptions> adminApiOptions,
        CancellationToken cancellationToken)
    {
        return ProxyAsync(context, forwarder, adminApiOptions, path: null, cancellationToken);
    }

    private static Task ProxyPathAsync(
        HttpContext context,
        IHttpForwarder forwarder,
        IOptions<AdminApiClientOptions> adminApiOptions,
        string? path,
        CancellationToken cancellationToken)
    {
        return ProxyAsync(context, forwarder, adminApiOptions, path, cancellationToken);
    }

    private static async Task ProxyAsync(
        HttpContext context,
        IHttpForwarder forwarder,
        IOptions<AdminApiClientOptions> adminApiOptions,
        string? path,
        CancellationToken cancellationToken)
    {
        try
        {
            WorkerDashboardProxy.ValidatePath(path);
        }
        catch (BadHttpRequestException ex)
        {
            context.Response.StatusCode = ex.StatusCode;
            return;
        }

        var destinationPrefix = WorkerDashboardProxy.BuildDestinationPrefix(adminApiOptions.Value.BaseAddress);
        await WorkerDashboardProxy.ForwardAsync(context, forwarder, destinationPrefix, cancellationToken);
    }
}

public static class WorkerDashboardProxy
{
    public static readonly string[] Methods =
    [
        HttpMethods.Get,
        HttpMethods.Post,
        HttpMethods.Head
    ];

    private static readonly HttpMessageInvoker ForwarderHttpClient = new(new SocketsHttpHandler
    {
        UseProxy = false,
        AllowAutoRedirect = false,
        AutomaticDecompression = DecompressionMethods.None,
        UseCookies = false,
        EnableMultipleHttp2Connections = true,
        ActivityHeadersPropagator = new ReverseProxyPropagator(DistributedContextPropagator.Current),
        ConnectTimeout = TimeSpan.FromSeconds(15)
    });

    private static readonly ForwarderRequestConfig RequestConfig = new()
    {
        ActivityTimeout = TimeSpan.FromSeconds(100)
    };

    private static readonly WorkerDashboardProxyTransformer Transformer = new();

    private static readonly Regex WorkerRootRelativeUrlPattern = new(
        @"(?<prefix>^|[""'\(=\s>])(?<path>/worker)(?=$|[/?#""'<\)\s;])",
        RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

    public static async Task ForwardAsync(
        HttpContext context,
        IHttpForwarder forwarder,
        string destinationPrefix,
        CancellationToken cancellationToken)
    {
        if (!context.Request.PathBase.HasValue)
        {
            await ForwardCoreAsync(context, forwarder, destinationPrefix, cancellationToken);
            return;
        }

        var originalBody = context.Response.Body;
        await using var bufferedBody = new MemoryStream();
        context.Response.Body = bufferedBody;

        try
        {
            await ForwardCoreAsync(context, forwarder, destinationPrefix, cancellationToken);

            context.Response.Body = originalBody;
            bufferedBody.Position = 0;

            if (!HttpMethods.IsHead(context.Request.Method)
                && ShouldRewriteResponseBody(context.Response))
            {
                await CopyRewrittenResponseBodyAsync(context, bufferedBody, cancellationToken);
            }
            else
            {
                await bufferedBody.CopyToAsync(originalBody, cancellationToken);
            }
        }
        finally
        {
            context.Response.Body = originalBody;
        }
    }

    public static void ValidatePath(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return;
        }

        foreach (var segment in path.TrimStart('/').Split('/', StringSplitOptions.RemoveEmptyEntries))
        {
            if (IsDotSegment(segment) || IsDotSegment(Uri.UnescapeDataString(segment)))
            {
                throw new BadHttpRequestException(
                    "Worker dashboard paths cannot contain dot segments.",
                    StatusCodes.Status400BadRequest);
            }
        }
    }

    public static string BuildDestinationPrefix(string baseAddress)
    {
        return baseAddress.EndsWith("/", StringComparison.Ordinal)
            ? baseAddress
            : $"{baseAddress}/";
    }

    public static string RewriteResponseBodyForPathBase(string body, PathString pathBase)
    {
        if (!pathBase.HasValue)
        {
            return body;
        }

        return WorkerRootRelativeUrlPattern.Replace(
            body,
            match => $"{match.Groups["prefix"].Value}{pathBase.Value}{match.Groups["path"].Value}");
    }

    public static string RewriteLocationHeader(string location, PathString pathBase)
    {
        if (!IsWorkerAbsolutePath(location) || !pathBase.HasValue)
        {
            return location;
        }

        return $"{pathBase}{location}";
    }

    private static async Task ForwardCoreAsync(
        HttpContext context,
        IHttpForwarder forwarder,
        string destinationPrefix,
        CancellationToken cancellationToken)
    {
        var error = await forwarder.SendAsync(
            context,
            destinationPrefix,
            ForwarderHttpClient,
            RequestConfig,
            Transformer,
            cancellationToken);

        if (error != ForwarderError.None && !context.Response.HasStarted)
        {
            context.Response.StatusCode = StatusCodes.Status502BadGateway;
        }
    }

    private static async Task CopyRewrittenResponseBodyAsync(
        HttpContext context,
        MemoryStream bufferedBody,
        CancellationToken cancellationToken)
    {
        var encoding = GetResponseEncoding(context.Response);
        var reader = new StreamReader(
            bufferedBody,
            encoding,
            detectEncodingFromByteOrderMarks: true,
            leaveOpen: true);
        var body = await reader.ReadToEndAsync(cancellationToken);
        var bytes = encoding.GetBytes(RewriteResponseBodyForPathBase(body, context.Request.PathBase));

        context.Response.ContentLength = bytes.Length;
        await context.Response.Body.WriteAsync(bytes, cancellationToken);
    }

    private static bool ShouldRewriteResponseBody(HttpResponse response)
    {
        return !response.Headers.ContainsKey(HeaderNames.ContentEncoding)
            && IsTextResponse(response);
    }

    private static bool IsTextResponse(HttpResponse response)
    {
        if (string.IsNullOrWhiteSpace(response.ContentType)
            || !MediaTypeHeaderValue.TryParse(response.ContentType, out var contentType))
        {
            return false;
        }

        var mediaType = contentType.MediaType.Value;
        if (string.IsNullOrEmpty(mediaType))
        {
            return false;
        }

        return mediaType.StartsWith("text/", StringComparison.OrdinalIgnoreCase)
            || mediaType.EndsWith("+xml", StringComparison.OrdinalIgnoreCase)
            || mediaType.Equals("application/javascript", StringComparison.OrdinalIgnoreCase)
            || mediaType.Equals("application/json", StringComparison.OrdinalIgnoreCase)
            || mediaType.Equals("application/xml", StringComparison.OrdinalIgnoreCase)
            || mediaType.Equals("image/svg+xml", StringComparison.OrdinalIgnoreCase);
    }

    private static Encoding GetResponseEncoding(HttpResponse response)
    {
        if (string.IsNullOrWhiteSpace(response.ContentType)
            || !MediaTypeHeaderValue.TryParse(response.ContentType, out var contentType)
            || !contentType.Charset.HasValue)
        {
            return Encoding.UTF8;
        }

        return Encoding.GetEncoding(contentType.Charset.Value.Trim('"'));
    }

    private static bool IsDotSegment(string segment)
    {
        return segment is "." or "..";
    }

    private static bool IsWorkerAbsolutePath(string location)
    {
        return location.Equals("/worker", StringComparison.OrdinalIgnoreCase)
            || location.StartsWith("/worker/", StringComparison.OrdinalIgnoreCase)
            || location.StartsWith("/worker?", StringComparison.OrdinalIgnoreCase);
    }
}

public sealed class WorkerDashboardProxyTransformer : HttpTransformer
{
    public override async ValueTask TransformRequestAsync(
        HttpContext httpContext,
        HttpRequestMessage proxyRequest,
        string destinationPrefix,
        CancellationToken cancellationToken)
    {
        await base.TransformRequestAsync(httpContext, proxyRequest, destinationPrefix, cancellationToken);

        proxyRequest.RequestUri = RequestUtilities.MakeDestinationAddress(
            destinationPrefix,
            httpContext.Request.Path,
            httpContext.Request.QueryString);
        proxyRequest.Headers.Remove(HeaderNames.AcceptEncoding);
        proxyRequest.Headers.Host = null;
    }

    public override async ValueTask<bool> TransformResponseAsync(
        HttpContext httpContext,
        HttpResponseMessage? proxyResponse,
        CancellationToken cancellationToken)
    {
        var shouldCopyBody = await base.TransformResponseAsync(httpContext, proxyResponse, cancellationToken);

        if (httpContext.Response.Headers.TryGetValue(HeaderNames.Location, out var values))
        {
            httpContext.Response.Headers[HeaderNames.Location] = new StringValues(
                values.Select(value =>
                    WorkerDashboardProxy.RewriteLocationHeader(
                        value ?? string.Empty,
                        httpContext.Request.PathBase)).ToArray());
        }

        return shouldCopyBody;
    }
}
