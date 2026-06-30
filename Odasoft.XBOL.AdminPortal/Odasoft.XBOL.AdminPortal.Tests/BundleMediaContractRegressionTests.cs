using System.Text.Json;
using Xunit;

namespace Odasoft.XBOL.AdminPortal.Tests;

public sealed class BundleMediaContractRegressionTests
{
    [Fact]
    public void Admin_api_spec_maps_bundle_media_to_media_set_response()
    {
        using var document = JsonDocument.Parse(ReadBusinessSource("OpenAPIs/admin-api.json"));

        var media = document.RootElement
            .GetProperty("components")
            .GetProperty("schemas")
            .GetProperty("BundleDTO")
            .GetProperty("properties")
            .GetProperty("media");

        Assert.Equal("MediaSetResponse", media.GetProperty("allOf")[0].GetProperty("$ref").GetString()?.Split('/').Last());
        Assert.False(media.TryGetProperty("type", out var type) && type.GetString() == "array");
    }

    [Fact]
    public void Admin_web_uses_media_set_response_for_event_and_bundle_media()
    {
        var mediaService = ReadBusinessSource("Services/MediaService.cs");
        var gallery = ReadAppSource("Components/Pages/EventDetails/Gallery.razor");
        var bundleCreate = ReadAppSource("Components/Pages/BundleCreate.razor");
        var bundleDetail = ReadAppSource("Components/Pages/BundleDetail.razor");
        var combinedSource = string.Join(
            Environment.NewLine,
            mediaService,
            gallery,
            bundleCreate,
            bundleDetail);

        Assert.DoesNotContain("EventMediaSetResponse", combinedSource, StringComparison.Ordinal);
        Assert.DoesNotContain("bundleResult.Media?.FirstOrDefault", bundleCreate, StringComparison.Ordinal);
        Assert.DoesNotContain("bundleResult.Media?.Where", bundleCreate, StringComparison.Ordinal);
        Assert.DoesNotContain("_bundle?.Media?.FirstOrDefault", bundleDetail, StringComparison.Ordinal);
        Assert.DoesNotContain("_bundle?.Media?.Where", bundleDetail, StringComparison.Ordinal);

        Assert.Contains("Task<MediaSetResponse> GetEventMediaAsync", mediaService, StringComparison.Ordinal);
        Assert.Contains("Task<MediaSetResponse> ReconcileEventMediaAsync", mediaService, StringComparison.Ordinal);
        Assert.Contains("Task<MediaSetResponse> ReconcileBundleMediaAsync", mediaService, StringComparison.Ordinal);
        Assert.Contains("public MediaSetResponse Media { get; set; }", gallery, StringComparison.Ordinal);
        Assert.Contains("var banner = bundleResult.Media?.Banner", bundleCreate, StringComparison.Ordinal);
        Assert.Contains("var gallery = bundleResult.Media?.Gallery?.ToList()", bundleCreate, StringComparison.Ordinal);
        Assert.Contains("private MediaSetResponse _media = new();", bundleDetail, StringComparison.Ordinal);
        Assert.Contains("Banner = _bundle?.Media?.Banner ?? new MediaResponse { Url = _bundle?.BannerImageUrl }", bundleDetail, StringComparison.Ordinal);
        Assert.Contains("Sponsors = _bundle?.Media?.Sponsors?.ToList() ?? []", bundleDetail, StringComparison.Ordinal);
        Assert.Contains("Gallery = _bundle?.Media?.Gallery?.ToList() ?? []", bundleDetail, StringComparison.Ordinal);
    }

    private static string ReadAppSource(string relativePath)
    {
        var path = Path.Combine(GetAppSourceRoot(), relativePath);

        return File.ReadAllText(path);
    }

    private static string ReadBusinessSource(string relativePath)
    {
        var path = Path.Combine(GetBusinessSourceRoot(), relativePath);

        return File.ReadAllText(path);
    }

    private static string GetAppSourceRoot()
    {
        return Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory,
            "../../../..",
            "Odasoft.XBOL.AdminPortal"));
    }

    private static string GetBusinessSourceRoot()
    {
        return Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory,
            "../../../..",
            "Odasoft.XBOL.Business"));
    }
}
