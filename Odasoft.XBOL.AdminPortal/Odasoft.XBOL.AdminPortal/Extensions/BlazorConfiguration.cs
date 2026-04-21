using Microsoft.AspNetCore.Components.Server;
using MudBlazor.Services;
using Radzen;

namespace Odasoft.XBOL.AdminPortal.Extensions;

public static class BlazorConfiguration
{
    public static IServiceCollection ConfigureBlazor(this IServiceCollection services)
    {
        services.AddRazorComponents().AddInteractiveServerComponents();

        services.AddHttpContextAccessor();

        services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.PositionClass = MudBlazor.Defaults.Classes.Position.TopCenter;
            config.SnackbarConfiguration.SnackbarVariant = MudBlazor.Variant.Text;
            config.SnackbarConfiguration.MaxDisplayedSnackbars = 5;
        });

        services.AddRadzenComponents();

        services.Configure<CircuitOptions>(options =>
        {
            options.DetailedErrors = true;
        });

        return services;
    }
}
