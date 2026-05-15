using Microsoft.Extensions.Options;
using Odasoft.XBOL.AdminPortal.Services;
using Odasoft.XBOL.Business;
using Odasoft.XBOL.Common.Options;

namespace Odasoft.XBOL.AdminPortal.Extensions;

public static class HttpClientConfiguration
{
    public static IServiceCollection ConfigureHttpClients(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddTransient<AdminSessionCookieHandler>();

        services.AddHttpClient("AdminApiSession", (provider, client) =>
        {
            ConfigureAdminApiClient(provider, client);
        });

        services.AddScoped<HttpClient>(provider =>
        {
            var sessionCookieHandler = provider.GetRequiredService<AdminSessionCookieHandler>();
            sessionCookieHandler.InnerHandler = new HttpClientHandler();

            var client = new HttpClient(sessionCookieHandler, disposeHandler: true);
            ConfigureAdminApiClient(provider, client);

            return client;
        });

        services.AddScoped<IAdminClient>(provider =>
            new AdminClient(provider.GetRequiredService<HttpClient>()));

        return services;
    }

    private static void ConfigureAdminApiClient(IServiceProvider provider, HttpClient client)
    {
        var config = provider.GetRequiredService<IOptions<AdminApiClientOptions>>().Value;
        client.BaseAddress = new Uri(config.BaseAddress);
        client.DefaultRequestHeaders.Add("Accept-Language", "es");
    }
}
