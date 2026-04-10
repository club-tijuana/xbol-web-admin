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

        services.AddOptions<AuthenticationOptions>()
            .BindConfiguration("Authentication");

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
}
