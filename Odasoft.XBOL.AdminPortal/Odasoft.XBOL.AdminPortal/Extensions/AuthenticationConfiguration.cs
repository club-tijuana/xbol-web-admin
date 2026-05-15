using FirebaseAdmin.Auth;
using FirebaseAdmin.Auth.Multitenancy;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Odasoft.XBOL.AdminPortal.Services;
using Odasoft.XBOL.AdminPortal.Services.Contracts;

namespace Odasoft.XBOL.AdminPortal.Extensions;

public static class AuthenticationConfiguration
{
    public static IServiceCollection ConfigureAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(FirebaseSessionAuthenticationHandler.SchemeName)
            .AddScheme<AuthenticationSchemeOptions, FirebaseSessionAuthenticationHandler>(
                FirebaseSessionAuthenticationHandler.SchemeName,
                options => { });

        services.AddAuthorization();
        services.AddAuthorizationCore();
        services.AddCascadingAuthenticationState();

        services.AddScoped<FirebaseAuthJsInterop>();
        services.AddScoped<BrowserFormPostJsInterop>();
        services.AddScoped<IAuthService, AuthService>();

        services.AddSingleton(provider =>
        {
            var options = provider.GetRequiredService<IOptions<GcipAuthOptions>>().Value;
            return GcipAuthConfiguration.InitializeFirebaseApp(options);
        });
        services.AddSingleton(provider => FirebaseAuth.GetAuth(provider.GetRequiredService<FirebaseAdmin.FirebaseApp>()));
        services.AddSingleton(provider =>
        {
            var options = provider.GetRequiredService<IOptions<GcipAuthOptions>>().Value;
            return provider.GetRequiredService<FirebaseAuth>().TenantManager.AuthForTenant(options.TenantId);
        });

        return services;
    }
}
