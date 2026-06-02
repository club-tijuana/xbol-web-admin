using Xunit;

namespace Odasoft.XBOL.AdminPortal.Tests;

public sealed class BundleRouteRegressionTests
{
    [Fact]
    public void Bundle_create_uses_kind_before_create_in_route_and_menu_links()
    {
        var bundleCreateSource = ReadAppSource("Components/Pages/BundleCreate.razor");
        var eventsSource = ReadAppSource("Components/Pages/Events.razor");

        Assert.Contains("@page \"/events/bundles/{BundleKind}/create\"", bundleCreateSource, StringComparison.Ordinal);
        Assert.Contains("Navigation.NavigateTo(\"/events/bundles/basic/create\")", eventsSource, StringComparison.Ordinal);
        Assert.Contains("Navigation.NavigateTo(\"/events/bundles/season-pass/create\")", eventsSource, StringComparison.Ordinal);

        Assert.DoesNotContain("@page \"/events/bundles/create/{BundleKind}\"", bundleCreateSource, StringComparison.Ordinal);
        Assert.DoesNotContain("Navigation.NavigateTo(\"/events/bundles/create/basic\")", eventsSource, StringComparison.Ordinal);
        Assert.DoesNotContain("Navigation.NavigateTo(\"/events/bundles/create/season-pass\")", eventsSource, StringComparison.Ordinal);
    }

    private static string ReadAppSource(string relativePath)
    {
        var path = Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory,
            "../../../..",
            "Odasoft.XBOL.AdminPortal",
            relativePath));

        return File.ReadAllText(path);
    }
}
