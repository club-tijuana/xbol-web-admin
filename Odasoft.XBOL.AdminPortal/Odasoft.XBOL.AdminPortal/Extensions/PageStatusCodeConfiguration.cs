using System.IO;

namespace Odasoft.XBOL.AdminPortal.Extensions;

public static class PageStatusCodeConfiguration
{
    public static IApplicationBuilder UseAnonymousPageAuthRedirects(this IApplicationBuilder app)
    {
        return app.Use(async (context, next) =>
        {
            await next();

            if (context.Response.HasStarted
                || !IsPageRequest(context)
                || !IsAnonymousAuthStatus(context))
            {
                return;
            }

            RedirectToLogin(context);
        });
    }

    private static bool IsAnonymousAuthStatus(HttpContext context)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            return false;
        }

        return context.Response.StatusCode is StatusCodes.Status401Unauthorized
            or StatusCodes.Status403Forbidden;
    }

    private static bool IsPageRequest(HttpContext context)
    {
        if (!HttpMethods.IsGet(context.Request.Method)
            && !HttpMethods.IsHead(context.Request.Method))
        {
            return false;
        }

        var path = context.Request.Path;
        if (path.StartsWithSegments("/_blazor")
            || path.StartsWithSegments("/auth")
            || path.StartsWithSegments("/healthz"))
        {
            return false;
        }

        return string.IsNullOrEmpty(Path.GetExtension(path.Value));
    }

    private static void RedirectToLogin(HttpContext context)
    {
        var returnUrl = $"{context.Request.PathBase}{context.Request.Path}{context.Request.QueryString}";
        if (string.IsNullOrWhiteSpace(returnUrl))
        {
            returnUrl = $"{context.Request.PathBase}/";
        }

        context.Response.Clear();
        context.Response.Redirect(
            $"{context.Request.PathBase}/login?returnUrl={Uri.EscapeDataString(returnUrl)}");
    }
}
