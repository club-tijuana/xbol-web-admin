using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Odasoft.XBOL.Common.Options;

namespace Odasoft.XBOL.AdminPortal.Services;

public sealed class AdminSessionCookieHandler(
    IHttpContextAccessor httpContextAccessor,
    IOptions<AdminSessionCookieOptions> options) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var cookieName = options.Value.CookieName;
        var context = httpContextAccessor.HttpContext;

        if (context?.Request.Cookies.TryGetValue(cookieName, out var sessionCookie) == true
            && !string.IsNullOrWhiteSpace(sessionCookie))
        {
            request.Headers.Add(HeaderNames.Cookie.ToString(), $"{cookieName}={sessionCookie}");
        }

        return base.SendAsync(request, cancellationToken);
    }
}
