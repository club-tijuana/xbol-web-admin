using MudBlazor.Services;
using Odasoft.XBOL.AdminPortal.Resources;
using Odasoft.XBOL.Common.Options;
using System.Globalization;

namespace Odasoft.XBOL.AdminPortal.Extensions;

public static class LocalizationConfiguration
{
    public static IServiceCollection ConfigureLocalization(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLocalization(options => options.ResourcesPath = "Resources");

        var localizationOptions = configuration.GetSection("Localization").Get<LocalizationOptions>() ?? new();

        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.SetDefaultCulture(localizationOptions.DefaultCulture);
            options.AddSupportedCultures(localizationOptions.SupportedCultures);
            options.AddSupportedUICultures(localizationOptions.SupportedCultures);
        });

        var defaultCulture = new CultureInfo(localizationOptions.DefaultCulture);
        CultureInfo.DefaultThreadCurrentCulture = defaultCulture;
        CultureInfo.DefaultThreadCurrentUICulture = defaultCulture;

        services.AddTransient(typeof(AppLocalizer<>));
        services.AddLocalizationInterceptor<CustomLocalizationInterceptor>();

        return services;
    }
}
