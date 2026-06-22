using Xunit;

namespace Odasoft.XBOL.AdminPortal.Tests;

public sealed class ReportsRegressionTests
{
    [Fact]
    public void Reports_event_selector_uses_unified_catalog_with_composite_keys()
    {
        var source = File.ReadAllText(Path.Combine(GetAppSourceRoot(), "Components/Pages/Reports.razor"));

        Assert.Contains("GetEventCatalogItemsAsync(", source, StringComparison.Ordinal);
        Assert.Contains("EventSelectKey", source, StringComparison.Ordinal);
        Assert.Contains("CatalogItemType", source, StringComparison.Ordinal);
        Assert.Contains("EventCatalogItemType.Bundle", source, StringComparison.Ordinal);
        Assert.Contains("EventCatalogItemType.Event", source, StringComparison.Ordinal);
        Assert.Contains("GetEventLabel", source, StringComparison.Ordinal);
        Assert.Contains("GetCashiersAsync(_selectedEventKey.Id, _selectedEventKey.ItemType)", source, StringComparison.Ordinal);
        Assert.DoesNotContain("_adminClient.GetEventsAsync(", source, StringComparison.Ordinal);
        Assert.DoesNotContain("private long? _selectedEventId;", source, StringComparison.Ordinal);
    }

    private static string GetAppSourceRoot()
        => Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory,
            "../../../..",
            "Odasoft.XBOL.AdminPortal"));
}
