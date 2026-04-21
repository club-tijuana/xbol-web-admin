using Microsoft.Extensions.DependencyInjection;
using Odasoft.XBOL.Business.Services;
using Odasoft.XBOL.Business.Services.Contracts;

namespace Odasoft.XBOL.Business.Extensions
{
    public static class ServiceConfiguration
    {
        // Add your service configurations here in the future
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<CatalogsService>();
            services.AddScoped<ClientCreditsService>();
            services.AddScoped<ClientsService>();
            services.AddScoped<CreditTransactionsService>();
            services.AddScoped<OrdersService>();
            services.AddScoped<SuiteAgreementsService>();
            services.AddScoped<SuiteLevelsService>();
            services.AddScoped<ISuitesService, SuitesService>();
            services.AddScoped<SupportService>();
            services.AddScoped<VenueMapsService>();
            services.AddScoped<VenuesService>();

            return services;
        }
    }
}
