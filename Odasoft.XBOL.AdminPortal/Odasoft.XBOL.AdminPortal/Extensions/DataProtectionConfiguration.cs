using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Options;
using Odasoft.XBOL.AdminPortal.Services.CloudStorage;
using Odasoft.XBOL.AdminPortal.Services.DataProtection;
using Odasoft.XBOL.Common.Options;

namespace Odasoft.XBOL.AdminPortal.Extensions;

public static class DataProtectionConfiguration
{
    public static IServiceCollection ConfigureDataProtection(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        var dataProtectionOptions = new DataProtectionKeyRingOptions();
        configuration.GetSection("DataProtection").Bind(dataProtectionOptions);

        services.AddDataProtection()
            .SetApplicationName(dataProtectionOptions.ApplicationName);

        if (environment.IsDevelopment())
        {
            return services;
        }

        services.AddSingleton<ICloudStorageClientFactory, GoogleCloudStorageClientFactory>();
        services.AddSingleton(provider => provider.GetRequiredService<ICloudStorageClientFactory>().CreateClient());
        services.AddSingleton(provider =>
        {
            var cloudStorage = provider.GetRequiredService<IOptions<CloudStorageOptions>>().Value;
            var dataProtection = provider.GetRequiredService<IOptions<DataProtectionKeyRingOptions>>().Value;
            var storageClient = provider.GetRequiredService<StorageClient>();

            return new CloudStorageXmlRepository(
                storageClient,
                cloudStorage.BucketName,
                dataProtection.KeyRingObjectName);
        });

        services.AddSingleton<IConfigureOptions<KeyManagementOptions>>(provider =>
            new ConfigureOptions<KeyManagementOptions>(options =>
            {
                options.XmlRepository = provider.GetRequiredService<CloudStorageXmlRepository>();
            }));

        return services;
    }
}
