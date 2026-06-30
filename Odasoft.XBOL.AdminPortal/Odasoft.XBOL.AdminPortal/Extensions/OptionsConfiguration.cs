using Odasoft.XBOL.AdminPortal.Services;
using Odasoft.XBOL.Common.Options;

namespace Odasoft.XBOL.AdminPortal.Extensions;

public static class OptionsConfiguration
{
    public static IServiceCollection ConfigureOptions(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        services.AddOptions<AdminApiClientOptions>()
            .BindConfiguration("AdminApiClient")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<ApiDocsProxyOptions>()
            .BindConfiguration(ApiDocsProxyOptions.SectionName);

        services.AddOptions<FirebaseAuthOptions>()
            .BindConfiguration("FirebaseAuth")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<GcipAuthOptions>()
            .BindConfiguration("GcipAuth")
            .ValidateDataAnnotations()
            .Validate(ValidateGcipAuthOptions, "GcipAuth requires ServiceAccountJson or ServiceAccountJsonPath.")
            .Validate(ValidateGcipAuthCredentialSource, "GcipAuth service account credentials are invalid or unavailable.")
            .ValidateOnStart();

        if (!environment.IsDevelopment())
        {
            services.AddOptions<CloudStorageOptions>()
                .BindConfiguration("CloudStorage")
                .ValidateDataAnnotations()
                .Validate(
                    ValidateCloudStorageOptions,
                    "CloudStorage:ServiceAccountJson or CloudStorage:ServiceAccountJsonPath is required.")
                .Validate(
                    ValidateCloudStorageCredentialSource,
                    "CloudStorage service account credentials are invalid or unavailable.")
                .ValidateOnStart();

            services.AddOptions<DataProtectionKeyRingOptions>()
                .BindConfiguration("DataProtection")
                .ValidateDataAnnotations()
                .Validate(ValidateDataProtectionKeyRingOptions, "DataProtection has invalid key-ring settings.")
                .ValidateOnStart();
        }

        services.AddOptions<AdminSessionCookieOptions>()
            .BindConfiguration("AdminSession")
            .ValidateDataAnnotations()
            .Validate(ValidateAdminSessionOptions, "AdminSession has invalid cookie settings.")
            .ValidateOnStart();

        services.AddOptions<SeatsIoOptions>()
            .BindConfiguration("SeatsIo")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<PaymentLinkOptions>()
            .BindConfiguration("PaymentLink")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<LocalizationOptions>()
            .BindConfiguration("Localization")
            .ValidateDataAnnotations()
            .Validate(
                options => IsValidTimeZone(options.TimeZoneId),
                "Localization:TimeZoneId must be a valid timezone ID.")
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

    private static bool IsValidTimeZone(string? timeZoneId)
    {
        if (string.IsNullOrWhiteSpace(timeZoneId))
        {
            return false;
        }

        try
        {
            TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return true;
        }
        catch (TimeZoneNotFoundException)
        {
            return false;
        }
        catch (InvalidTimeZoneException)
        {
            return false;
        }
    }

    private static bool ValidateGcipAuthOptions(GcipAuthOptions options)
    {
        return !string.IsNullOrWhiteSpace(options.ServiceAccountJson)
            || !string.IsNullOrWhiteSpace(options.ServiceAccountJsonPath);
    }

    private static bool ValidateGcipAuthCredentialSource(GcipAuthOptions options)
    {
        return ValidateServiceAccountCredentialSource(options.ServiceAccountJson, options.ServiceAccountJsonPath);
    }

    private static bool ValidateCloudStorageOptions(CloudStorageOptions options)
    {
        return !string.IsNullOrWhiteSpace(options.ServiceAccountJson)
            || !string.IsNullOrWhiteSpace(options.ServiceAccountJsonPath);
    }

    private static bool ValidateCloudStorageCredentialSource(CloudStorageOptions options)
    {
        return ValidateServiceAccountCredentialSource(options.ServiceAccountJson, options.ServiceAccountJsonPath);
    }

    private static bool ValidateServiceAccountCredentialSource(string? serviceAccountJson, string? serviceAccountJsonPath)
    {
        return GoogleServiceAccountCredentialFactory.Validate(serviceAccountJson, serviceAccountJsonPath);
    }

    private static bool ValidateDataProtectionKeyRingOptions(DataProtectionKeyRingOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.ApplicationName))
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(options.KeyRingObjectName))
        {
            return false;
        }

        if (options.KeyRingObjectName.StartsWith("/", StringComparison.Ordinal)
            || options.KeyRingObjectName.EndsWith("/", StringComparison.Ordinal)
            || options.KeyRingObjectName.Contains("//", StringComparison.Ordinal)
            || options.KeyRingObjectName.Contains("..", StringComparison.Ordinal))
        {
            return false;
        }

        return !options.KeyRingObjectName.Any(char.IsControl);
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
