using Microsoft.Extensions.Options;
using Odasoft.XBOL.Business;
using Odasoft.XBOL.Common.Options;

namespace Odasoft.XBOL.AdminPortal.Extensions;

public static class HttpClientConfiguration
{
    public static IServiceCollection ConfigureHttpClients(this IServiceCollection services)
    {
        services.AddHttpClient<IAdminClient, AdminClient>(
            (provider, client) =>
            {
                var config = provider.GetRequiredService<IOptions<AdminApiClientOptions>>().Value;
                client.BaseAddress = new Uri(config.BaseAddress);
                client.DefaultRequestHeaders.Add("Accept-Language", "es");
            });

        return services;
    }
}
