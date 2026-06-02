using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Options;
using Odasoft.XBOL.Common.Options;

namespace Odasoft.XBOL.AdminPortal.Services.CloudStorage;

public sealed class GoogleCloudStorageClientFactory(
    IOptions<CloudStorageOptions> options) : ICloudStorageClientFactory
{
    public StorageClient CreateClient()
    {
        var cloudStorageOptions = options.Value;
        var credential = GoogleServiceAccountCredentialFactory.Create(
            cloudStorageOptions.ServiceAccountJson,
            cloudStorageOptions.ServiceAccountJsonPath);

        return StorageClient.Create(credential);
    }
}
