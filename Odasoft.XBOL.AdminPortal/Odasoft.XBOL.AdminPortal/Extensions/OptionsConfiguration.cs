using Odasoft.XBOL.AdminPortal.Services;
using Odasoft.XBOL.Common.Options;

namespace Odasoft.XBOL.AdminPortal.Extensions;

public static class OptionsConfiguration
{
    public static IServiceCollection ConfigureOptions(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<AdminApiClientOptions>()
            .BindConfiguration("AdminApiClient")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<FirebaseAuthOptions>()
            .BindConfiguration("FirebaseAuth")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<GcipAuthOptions>()
            .BindConfiguration("GcipAuth")
            .ValidateDataAnnotations()
            .Validate(ValidateGcipAuthOptions, "GcipAuth requires ServiceAccountJson or ServiceAccountJsonPath.")
            .ValidateOnStart();

        services.AddOptions<AdminSessionCookieOptions>()
            .BindConfiguration("AdminSession")
            .ValidateDataAnnotations()
            .Validate(ValidateAdminSessionOptions, "AdminSession has invalid cookie settings.")
            .ValidateOnStart();

        services.AddOptions<SeatsIoOptions>()
            .BindConfiguration("SeatsIo")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<HostingOptions>()
            .BindConfiguration("Hosting")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.PostConfigure<HostingOptions>(o =>
        {
            if (string.IsNullOrWhiteSpace(o.PathBase))
            {
                o.PathBase = null;
                return;
            }

            var trimmed = o.PathBase.Trim().TrimEnd('/');
            o.PathBase = string.IsNullOrEmpty(trimmed) ? null : trimmed;
        });

        return services;
    }

    private static bool ValidateGcipAuthOptions(GcipAuthOptions options)
    {
        return !string.IsNullOrWhiteSpace(options.ServiceAccountJson)
            || !string.IsNullOrWhiteSpace(options.ServiceAccountJsonPath);
    }

    private static bool ValidateAdminSessionOptions(AdminSessionCookieOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.CookieName))
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(options.Path) || !options.Path.StartsWith('/'))
        {
            return false;
        }

        options.Path = options.Path.TrimEnd('/');
        if (string.IsNullOrEmpty(options.Path))
        {
            options.Path = "/";
        }

        if (options.Lifetime < AdminSessionCookieOptions.MinLifetime
            || options.Lifetime > AdminSessionCookieOptions.MaxLifetime)
        {
            return false;
        }

        if (options.RecentSignInWindow <= TimeSpan.Zero)
        {
            return false;
        }

        if (!Enum.TryParse<SameSiteMode>(options.SameSite, ignoreCase: true, out var sameSite))
        {
            return false;
        }

        if (sameSite == SameSiteMode.None && !options.Secure)
        {
            return false;
        }

        if (options.CookieName.StartsWith("__Secure-", StringComparison.Ordinal) && !options.Secure)
        {
            return false;
        }

        if (!options.CookieName.StartsWith("__Host-", StringComparison.Ordinal))
        {
            return true;
        }

        return options.Secure
            && options.Path == "/"
            && string.IsNullOrWhiteSpace(options.Domain);
    }
}
