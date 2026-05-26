using System.Net;
using System.Xml;
using System.Xml.Linq;
using Google;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.DataProtection.Repositories;

namespace Odasoft.XBOL.AdminPortal.Services.DataProtection;

public sealed class CloudStorageXmlRepository(
    StorageClient storageClient,
    string bucketName,
    string objectName) : IXmlRepository
{
    private static readonly RetryPolicy LoadRetryPolicy = new(
        maxAttempts: 5,
        initialDelay: TimeSpan.FromMilliseconds(200),
        backoffMultiplier: 1.5,
        maxDelay: TimeSpan.FromSeconds(1));

    private static readonly RetryPolicy StoreRetryPolicy = new(
        maxAttempts: 5,
        initialDelay: TimeSpan.FromSeconds(1),
        backoffMultiplier: 2,
        maxDelay: TimeSpan.FromSeconds(3));

    private readonly object _syncRoot = new();
    private LoadedKeyRing? _latestKeyRing;

    public IReadOnlyCollection<XElement> GetAllElements()
    {
        var keyRing = LoadLatest();
        return keyRing.Document?.Root?.Elements().Select(element => new XElement(element)).ToArray()
            ?? [];
    }

    public void StoreElement(XElement element, string friendlyName)
    {
        StoreRetryPolicy.Execute(() =>
        {
            var latest = LoadLatest();
            var document = latest.Document is null
                ? new XDocument(new XElement("root"))
                : new XDocument(latest.Document);

            document.Root!.Add(new XElement(element));

            long generation;
            using (var stream = new MemoryStream())
            {
                document.Save(stream, SaveOptions.DisableFormatting);
                stream.Position = 0;

                var uploadedObject = storageClient.UploadObject(
                    bucketName,
                    objectName,
                    "application/xml",
                    stream,
                    new UploadObjectOptions { IfGenerationMatch = latest.Generation });

                generation = uploadedObject.Generation
                    ?? throw new InvalidOperationException(
                        $"CloudStorage DataProtection key ring upload for gs://{bucketName}/{objectName} did not return an object generation.");
            }

            Cache(new LoadedKeyRing(generation, document));
        });
    }

    private LoadedKeyRing LoadLatest()
    {
        var previous = ReadCached();
        var current = LoadRetryPolicy.Execute(() => LoadLatestOnce(previous));
        Cache(current);
        return current;
    }

    private LoadedKeyRing LoadLatestOnce(LoadedKeyRing? previous)
    {
        long generation;
        try
        {
            var metadata = storageClient.GetObject(
                bucketName,
                objectName,
                previous is null ? null : new GetObjectOptions { IfGenerationNotMatch = previous.Generation });

            generation = metadata.Generation
                ?? throw new InvalidOperationException(
                    $"CloudStorage DataProtection key ring metadata for gs://{bucketName}/{objectName} did not include an object generation.");
        }
        catch (GoogleApiException ex) when (ex.HttpStatusCode == HttpStatusCode.NotFound)
        {
            return LoadedKeyRing.Missing;
        }
        catch (GoogleApiException ex) when (ex.HttpStatusCode == HttpStatusCode.NotModified && previous is not null)
        {
            return previous;
        }

        using var stream = new MemoryStream();
        storageClient.DownloadObject(
            bucketName,
            objectName,
            stream,
            new DownloadObjectOptions { IfGenerationMatch = generation });

        stream.Position = 0;
        try
        {
            return new LoadedKeyRing(generation, XDocument.Load(stream));
        }
        catch (XmlException ex)
        {
            throw new InvalidOperationException(
                $"CloudStorage DataProtection key ring object gs://{bucketName}/{objectName} is not valid XML.",
                ex);
        }
    }

    private LoadedKeyRing? ReadCached()
    {
        lock (_syncRoot)
        {
            return _latestKeyRing;
        }
    }

    private void Cache(LoadedKeyRing keyRing)
    {
        lock (_syncRoot)
        {
            _latestKeyRing = keyRing;
        }
    }

    private sealed record LoadedKeyRing(long Generation, XDocument? Document)
    {
        public static LoadedKeyRing Missing { get; } = new(0, null);
    }

    private sealed class RetryPolicy(
        int maxAttempts,
        TimeSpan initialDelay,
        double backoffMultiplier,
        TimeSpan maxDelay)
    {
        public T Execute<T>(Func<T> action)
        {
            var attempts = 0;
            var delay = initialDelay;

            while (true)
            {
                try
                {
                    return action();
                }
                catch (GoogleApiException) when (++attempts < maxAttempts)
                {
                    Thread.Sleep(delay);
                    delay = TimeSpan.FromMilliseconds(
                        Math.Min(maxDelay.TotalMilliseconds, delay.TotalMilliseconds * backoffMultiplier));
                }
            }
        }

        public void Execute(Action action)
        {
            Execute(
                () =>
                {
                    action();
                    return true;
                });
        }
    }
}
