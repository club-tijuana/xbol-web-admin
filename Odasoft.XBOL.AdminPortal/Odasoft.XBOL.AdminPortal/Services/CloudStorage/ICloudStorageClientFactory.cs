using Google.Cloud.Storage.V1;

namespace Odasoft.XBOL.AdminPortal.Services.CloudStorage;

public interface ICloudStorageClientFactory
{
    StorageClient CreateClient();
}
