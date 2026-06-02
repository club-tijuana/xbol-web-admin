namespace Odasoft.XBOL.AdminPortal.Extensions;

public static class PageStatusCodeConfiguration
{
    public static IApplicationBuilder UseAnonymousPageAuthRedirects(this IApplicationBuilder app)
    {
        return app.Use(async (context, next) =>
        {
            await next();

            if (context.Response.HasStarted || !IsPageRequest(context))
            {
                return;
            }

            var statusCode = context.Response.StatusCode;

            if (statusCode is StatusCodes.Status401Unauthorized or StatusCodes.Status403Forbidden)
            {
                if (context.User.Identity?.IsAuthenticated != true)
                {
                    RedirectToLogin(context);
                }
                else
                {
                    RedirectToAccessDenied(context);
                }
            }
        });
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

    private static void RedirectToAccessDenied(HttpContext context)
    {
        context.Response.Clear();
        context.Response.Redirect($"{context.Request.PathBase}/access-denied");
    }
}
