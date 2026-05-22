using FirebaseAdmin.Auth;
using FirebaseAdmin.Auth.Multitenancy;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using Odasoft.XBOL.Common.Options;
using System.Net;
using System.Net.Http.Headers;

namespace Odasoft.XBOL.AdminPortal.Extensions;

public static class AdminSessionEndpointConfiguration
{
    public static IEndpointRouteBuilder MapAdminSessionEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/auth/login", CreateSessionAsync)
            .AllowAnonymous()
            .WithMetadata(new RequireAntiforgeryTokenAttribute());

        endpoints.MapPost("/auth/logout", DeleteSession)
            .AllowAnonymous()
            .WithMetadata(new RequireAntiforgeryTokenAttribute());

        return endpoints;
    }

    private static async Task<IResult> CreateSessionAsync(
        FirebaseAuth firebaseAuth,
        TenantAwareFirebaseAuth tenantAuth,
        IHttpClientFactory httpClientFactory,
        ILoggerFactory loggerFactory,
        IOptions<AdminSessionCookieOptions> sessionOptions,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var form = await context.Request.ReadFormAsync(cancellationToken);
        var idToken = form["idToken"].ToString();
        var returnUrl = GetLocalRedirectUrl(context, form["returnUrl"].ToString());

        if (string.IsNullOrWhiteSpace(idToken))
        {
            return LoginRedirect(context, "session");
        }

        FirebaseToken token;
        try
        {
            token = await tenantAuth.VerifyIdTokenAsync(idToken, cancellationToken);
        }
        catch (FirebaseAuthException)
        {
            return LoginRedirect(context, "session");
        }

        var options = sessionOptions.Value;
        var now = DateTimeOffset.UtcNow;
        if (!TryGetAuthTime(token.Claims, out var authTime)
            || authTime > now
            || now - authTime > options.RecentSignInWindow)
        {
            return LoginRedirect(context, "session");
        }

        var logger = loggerFactory.CreateLogger("Odasoft.XBOL.AdminPortal.AdminSession");
        var profileValidationResult = await ValidateAdminProfileAsync(httpClientFactory, logger, idToken, cancellationToken);
        if (profileValidationResult is not AdminProfileValidationResult.Valid)
        {
            var error = profileValidationResult switch
            {
                AdminProfileValidationResult.ProfileRejected => "admin_profile",
                AdminProfileValidationResult.ServiceError => "admin_api_error",
                _ => "admin_api_unavailable"
            };
            return LoginRedirect(context, error);
        }

        var sessionCookie = await firebaseAuth.CreateSessionCookieAsync(
            idToken,
            new SessionCookieOptions { ExpiresIn = options.Lifetime },
            cancellationToken);

        context.Response.Cookies.Append(
            options.CookieName,
            sessionCookie,
            BuildSessionCookieOptions(options, DateTimeOffset.UtcNow.Add(options.Lifetime)));

        return Results.LocalRedirect(returnUrl);
    }

    private static async Task<AdminProfileValidationResult> ValidateAdminProfileAsync(
        IHttpClientFactory httpClientFactory,
        ILogger logger,
        string idToken,
        CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient("AdminApiSession");
        using var profileRequest = new HttpRequestMessage(HttpMethod.Get, "auth/me");
        profileRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", idToken);

        try
        {
            using var profileResponse = await client.SendAsync(profileRequest, cancellationToken);
            if (profileResponse.IsSuccessStatusCode)
            {
                return AdminProfileValidationResult.Valid;
            }

            var responseBody = profileResponse.Content is null
                ? null
                : await profileResponse.Content.ReadAsStringAsync(cancellationToken);
            logger.LogWarning(
                "Admin profile check failed with status {StatusCode}. Response body: {ResponseBody}",
                (int)profileResponse.StatusCode,
                TruncateForLog(responseBody));

            return profileResponse.StatusCode switch
            {
                HttpStatusCode.BadRequest
                    or HttpStatusCode.Unauthorized
                    or HttpStatusCode.Forbidden
                    or HttpStatusCode.NotFound
                    or HttpStatusCode.Conflict => AdminProfileValidationResult.ProfileRejected,
                _ => AdminProfileValidationResult.ServiceError
            };
        }
        catch (HttpRequestException ex)
        {
            logger.LogWarning(ex, "Admin profile check failed while contacting Admin API.");
        }
        catch (OperationCanceledException ex) when (!cancellationToken.IsCancellationRequested)
        {
            logger.LogWarning(ex, "Admin profile check timed out while contacting Admin API.");
        }

        return AdminProfileValidationResult.Unavailable;
    }

    private enum AdminProfileValidationResult
    {
        Valid,
        ProfileRejected,
        ServiceError,
        Unavailable
    }

    private static string? TruncateForLog(string? value)
    {
        const int maxLength = 1024;
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return value.Length <= maxLength
            ? value
            : value[..maxLength];
    }

    private static IResult DeleteSession(IOptions<AdminSessionCookieOptions> sessionOptions, HttpContext context)
    {
        var options = sessionOptions.Value;
        context.Response.Cookies.Delete(options.CookieName, BuildSessionCookieOptions(options, expires: null));
        return Results.LocalRedirect(BuildAppPath(context, "/login"));
    }

    private static CookieOptions BuildSessionCookieOptions(
        AdminSessionCookieOptions options,
        DateTimeOffset? expires)
    {
        return new CookieOptions
        {
            HttpOnly = true,
            Secure = options.Secure,
            SameSite = Enum.Parse<Microsoft.AspNetCore.Http.SameSiteMode>(options.SameSite, ignoreCase: true),
            Domain = string.IsNullOrWhiteSpace(options.Domain) ? null : options.Domain,
            Path = options.Path,
            Expires = expires
        };
    }

    private static IResult LoginRedirect(HttpContext context, string error)
    {
        return Results.LocalRedirect(BuildAppPath(context, $"/login?error={Uri.EscapeDataString(error)}"));
    }

    private static string GetLocalRedirectUrl(HttpContext context, string? returnUrl)
    {
        if (string.IsNullOrWhiteSpace(returnUrl))
        {
            return BuildAppPath(context, "/");
        }

        return RedirectHttpResult.IsLocalUrl(returnUrl)
            ? returnUrl
            : BuildAppPath(context, "/");
    }

    private static string BuildAppPath(HttpContext context, string path)
    {
        return $"{context.Request.PathBase}{path}";
    }

    private static bool TryGetAuthTime(
        IReadOnlyDictionary<string, object> claims,
        out DateTimeOffset authTime)
    {
        authTime = default;
        if (!claims.TryGetValue("auth_time", out var value))
        {
            return false;
        }

        long seconds;
        switch (value)
        {
            case long longValue:
                seconds = longValue;
                break;
            case int intValue:
                seconds = intValue;
                break;
            default:
                return false;
        }

        authTime = DateTimeOffset.FromUnixTimeSeconds(seconds);
        return true;
    }
}
