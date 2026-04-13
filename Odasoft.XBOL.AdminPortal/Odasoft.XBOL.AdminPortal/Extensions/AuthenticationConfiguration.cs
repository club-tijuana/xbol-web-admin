using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Odasoft.XBOL.AdminPortal.Services;
using Odasoft.XBOL.AdminPortal.Services.Contracts;
using Odasoft.XBOL.AdminPortal.States;

namespace Odasoft.XBOL.AdminPortal.Extensions;

public static class AuthenticationConfiguration
{
    public static IServiceCollection ConfigureAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/login";
                options.LogoutPath = "/logout";
                options.AccessDeniedPath = "/login";
            });

        services.AddAuthorization();
        services.AddAuthorizationCore();

        services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
        services.AddScoped<AuthStateProvider>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
