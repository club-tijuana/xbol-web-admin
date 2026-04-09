using MudBlazor.Services;
using Odasoft.XBOL.AdminPortal.Resources;
using System.Globalization;

namespace Odasoft.XBOL.AdminPortal.Extensions;

public static class LocalizationConfiguration
{
    public static IServiceCollection ConfigureLocalization(this IServiceCollection services)
    {
        services.AddLocalization(options => options.ResourcesPath = "Resources");

        services.Configure<RequestLocalizationOptions>(options =>
        {
            string[] supportedCultures = ["es-MX"];
            options.SetDefaultCulture("es-MX");
            options.AddSupportedCultures(supportedCultures);
            options.AddSupportedUICultures(supportedCultures);
        });

        var mexicoCulture = new CultureInfo("es-MX");
        CultureInfo.DefaultThreadCurrentCulture = mexicoCulture;
        CultureInfo.DefaultThreadCurrentUICulture = mexicoCulture;

        services.AddTransient(typeof(AppLocalizer<>));
        services.AddLocalizationInterceptor<CustomLocalizationInterceptor>();

        return services;
    }
}
