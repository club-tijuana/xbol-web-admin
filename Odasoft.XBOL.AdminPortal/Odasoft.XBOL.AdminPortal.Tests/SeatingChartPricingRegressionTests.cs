using Xunit;

namespace Odasoft.XBOL.AdminPortal.Tests;

public sealed class SeatingChartPricingRegressionTests
{
    [Fact]
    public void Seating_chart_matches_numeric_price_categories_to_seatsio_string_keys()
    {
        var source = ReadAppSource("Components/SeatingChart.razor.js");

        Assert.Contains(
            "String(p.category) === String(obj.category?.key)",
            source,
            StringComparison.Ordinal);
    }

    private static string ReadAppSource(string relativePath)
    {
        var path = Path.Combine(GetAppSourceRoot(), relativePath);

        return File.ReadAllText(path);
    }

    private static string GetAppSourceRoot()
    {
        return Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory,
            "../../../..",
            "Odasoft.XBOL.AdminPortal"));
    }
}
