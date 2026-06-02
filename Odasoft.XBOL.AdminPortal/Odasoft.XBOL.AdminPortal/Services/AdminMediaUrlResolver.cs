using Microsoft.Extensions.Options;
using Odasoft.XBOL.Common.Options;

namespace Odasoft.XBOL.AdminPortal.Services;

public sealed class AdminMediaUrlResolver(IOptions<AdminApiClientOptions> options)
{
    private readonly Uri adminApiBaseAddress = new(options.Value.BaseAddress, UriKind.Absolute);

    public string? Resolve(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return null;
        }

        var trimmedUrl = url.Trim();
        if (Uri.TryCreate(trimmedUrl, UriKind.Absolute, out var absoluteUri) && !absoluteUri.IsFile)
        {
            return trimmedUrl;
        }

        return new Uri(adminApiBaseAddress, trimmedUrl).ToString();
    }
}
