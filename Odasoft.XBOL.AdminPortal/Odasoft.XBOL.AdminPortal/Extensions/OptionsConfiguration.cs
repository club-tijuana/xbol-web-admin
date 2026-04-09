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

        return services;
    }
}
