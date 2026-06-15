using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Odasoft.XBOL.Auth;
using Odasoft.XBOL.Common.Options;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Yarp.ReverseProxy.Forwarder;

namespace Odasoft.XBOL.AdminPortal.Extensions;

public static class ApiDocsProxyEndpointConfiguration
{
    public static IEndpointRouteBuilder MapApiDocsProxyEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/docs/{service}")
            .RequireAuthorization(PermissionPolicy.Build(AdminPermissions.Docs.Read));

        group.MapMethods("", ApiDocsProxy.Methods, RedirectToIndexAsync);
        group.MapMethods("/", ApiDocsProxy.Methods, RedirectToIndexAsync);
        group.MapMethods("/{**path}", ApiDocsProxy.Methods, ProxyPathAsync);

        return endpoints;
    }

    private static Task RedirectToIndexAsync(HttpContext context, string service)
    {
        if (!ApiDocsProxy.IsKnownService(service))
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            return Task.CompletedTask;
        }

        context.Response.Redirect($"{context.Request.PathBase}/docs/{service}/index.html");
        return Task.CompletedTask;
    }

    private static async Task ProxyPathAsync(
        HttpContext context,
        IHttpForwarder forwarder,
        IOptions<ApiDocsProxyOptions> apiDocsOptions,
        IOptions<AdminApiClientOptions> adminApiOptions,
        string service,
        string? path,
        CancellationToken cancellationToken)
    {
        ApiDocsProxyTarget target;
        try
        {
            ApiDocsProxy.ValidatePath(path);
            target = ApiDocsProxy.ResolveTarget(service, apiDocsOptions.Value, adminApiOptions.Value.BaseAddress);
        }
        catch (BadHttpRequestException ex)
        {
            context.Response.StatusCode = ex.StatusCode;
            return;
        }

        if (string.IsNullOrWhiteSpace(target.BaseAddress))
        {
            context.Response.StatusCode = StatusCodes.Status502BadGateway;
            return;
        }

        context.Items[ApiDocsProxy.ServiceItemKey] = service;
        context.Items[ApiDocsProxy.UpstreamPathItemKey] = ApiDocsProxy.BuildUpstreamPath(path);

        var destinationPrefix = ApiDocsProxy.BuildDestinationPrefix(target.BaseAddress);
        await ApiDocsProxy.ForwardAsync(context, forwarder, destinationPrefix, cancellationToken);
    }
}

public readonly record struct ApiDocsProxyTarget(
    string? BaseAddress,
    string DocumentName);

public static class ApiDocsProxy
{
    public const string ServiceItemKey = "ApiDocsProxy.Service";
    public const string UpstreamPathItemKey = "ApiDocsProxy.UpstreamPath";

    public static readonly string[] Methods =
    [
        HttpMethods.Get,
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

    private static readonly ApiDocsProxyTransformer Transformer = new();

    private static readonly Regex SwaggerRootRelativeUrlPattern = new(
        @"(?<prefix>^|[""'\(=\s>])(?<path>/swagger)(?=$|[/?#""'<\)\s;])",
        RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

    public static bool IsKnownService(string service)
    {
        return service is "admin-api" or "ticketing-api" or "client-api";
    }

    public static ApiDocsProxyTarget ResolveTarget(
        string service,
        ApiDocsProxyOptions options,
        string adminApiBaseAddress)
    {
        return service switch
        {
            "admin-api" => new ApiDocsProxyTarget(
                string.IsNullOrWhiteSpace(options.AdminApiBaseAddress)
                    ? adminApiBaseAddress
                    : options.AdminApiBaseAddress,
                "admin-api"),
            "ticketing-api" => new ApiDocsProxyTarget(options.TicketingApiBaseAddress, "ticketing-api"),
            "client-api" => new ApiDocsProxyTarget(options.ClientApiBaseAddress, "client-api"),
            _ => throw new BadHttpRequestException(
                "Unknown API docs service.",
                StatusCodes.Status404NotFound)
        };
    }

    public static string BuildUpstreamPath(string? publicPath)
    {
        if (string.IsNullOrWhiteSpace(publicPath))
        {
            return "/swagger/index.html";
        }

        var trimmed = publicPath.TrimStart('/');
        if (trimmed.Equals("index.html", StringComparison.OrdinalIgnoreCase)
            || trimmed.Equals("swagger", StringComparison.OrdinalIgnoreCase))
        {
            return "/swagger/index.html";
        }

        if (trimmed.StartsWith("swagger/", StringComparison.OrdinalIgnoreCase))
        {
            trimmed = trimmed["swagger/".Length..];
        }

        return $"/swagger/{trimmed}";
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
                    "API docs paths cannot contain dot segments.",
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

    public static string RewriteResponseBodyForDocs(string body, PathString pathBase, string service)
    {
        return SwaggerRootRelativeUrlPattern.Replace(
            body,
            match => $"{match.Groups["prefix"].Value}{BuildPublicDocsBase(pathBase, service)}");
    }

    public static string RewriteLocationHeader(string location, PathString pathBase, string service)
    {
        if (!IsSwaggerAbsolutePath(location))
        {
            return location;
        }

        var suffix = location["/swagger".Length..];
        return $"{BuildPublicDocsBase(pathBase, service)}{suffix}";
    }

    public static async Task ForwardAsync(
        HttpContext context,
        IHttpForwarder forwarder,
        string destinationPrefix,
        CancellationToken cancellationToken)
    {
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

    private static string BuildPublicDocsBase(PathString pathBase, string service)
    {
        return pathBase.HasValue
            ? $"{pathBase}/docs/{service}"
            : $"/docs/{service}";
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
        var service = GetRequiredString(context, ServiceItemKey);
        var encoding = GetResponseEncoding(context.Response);
        var reader = new StreamReader(
            bufferedBody,
            encoding,
            detectEncodingFromByteOrderMarks: true,
            leaveOpen: true);
        var body = await reader.ReadToEndAsync(cancellationToken);
        var bytes = encoding.GetBytes(RewriteResponseBodyForDocs(body, context.Request.PathBase, service));

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

    private static string GetRequiredString(HttpContext context, string itemKey)
    {
        return context.Items.TryGetValue(itemKey, out var value) && value is string text
            ? text
            : throw new InvalidOperationException($"{itemKey} is missing.");
    }

    private static bool IsSwaggerAbsolutePath(string location)
    {
        return location.Equals("/swagger", StringComparison.OrdinalIgnoreCase)
            || location.StartsWith("/swagger/", StringComparison.OrdinalIgnoreCase)
            || location.StartsWith("/swagger?", StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsDotSegment(string segment)
    {
        return segment is "." or "..";
    }
}

public sealed class ApiDocsProxyTransformer : HttpTransformer
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
            GetRequiredItem(httpContext, ApiDocsProxy.UpstreamPathItemKey),
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
            var service = GetRequiredItem(httpContext, ApiDocsProxy.ServiceItemKey);
            httpContext.Response.Headers[HeaderNames.Location] = new StringValues(
                values.Select(value =>
                    ApiDocsProxy.RewriteLocationHeader(
                        value ?? string.Empty,
                        httpContext.Request.PathBase,
                        service)).ToArray());
        }

        return shouldCopyBody;
    }

    private static string GetRequiredItem(HttpContext context, string itemKey)
    {
        return context.Items.TryGetValue(itemKey, out var value) && value is string text
            ? text
            : throw new InvalidOperationException($"{itemKey} is missing.");
    }
}
