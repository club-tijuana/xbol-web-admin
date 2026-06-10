using Odasoft.XBOL.AdminPortal.Services;
using Odasoft.XBOL.AdminPortal.Services.Contracts;
using Odasoft.XBOL.AdminPortal.States;
using Odasoft.XBOL.Business.Services;

namespace Odasoft.XBOL.AdminPortal.Extensions;

public static class ApplicationServicesConfiguration
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<AdminMediaUrlResolver>();
        services.AddScoped<IEventService, ApiEventService>();
        services.AddScoped<ISeasonPassService, SeasonPassService>();
        services.AddScoped<ISeasonService, SeasonService>();
        services.AddScoped<ClientsService>();
        services.AddScoped<EventScheduleService>();

        services.AddScoped<CartState>();
        services.AddScoped<LoadingState>();
        services.AddScoped<PersistentDialogState>();

        return services;
    }
}
