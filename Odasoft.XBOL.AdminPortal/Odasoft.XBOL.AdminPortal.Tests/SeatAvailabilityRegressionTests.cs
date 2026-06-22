using Xunit;

namespace Odasoft.XBOL.AdminPortal.Tests;

public sealed class SeatAvailabilityRegressionTests
{
    [Fact]
    public void Availability_sell_matches_catalog_bundle_sellability()
    {
        var source = ReadAppSource("Components/Pages/SeatAvailability.razor");

        Assert.Contains("CanSellSelectedSeats", source, StringComparison.Ordinal);
        Assert.Contains("Disabled=\"@(!CanSellSelectedSeats)\"", source, StringComparison.Ordinal);
        Assert.Contains("GetBundleByIdAsync(Id)", source, StringComparison.Ordinal);
        Assert.Contains("GetEventByIdAsync(Id)", source, StringComparison.Ordinal);
        Assert.Contains("schedule.Status == ScheduleStatus.OnSale", source, StringComparison.Ordinal);
        Assert.Contains("IsSaleOpen(schedule.OnSaleDate, schedule.OffSaleDate", source, StringComparison.Ordinal);
        Assert.Contains("SeasonPassPurchaseWindow.CanBuy", source, StringComparison.Ordinal);
        Assert.Contains("SaleType = _availabilitySaleType", source, StringComparison.Ordinal);
        Assert.Contains("SaleId = _availabilitySaleId", source, StringComparison.Ordinal);
        Assert.DoesNotContain("detail.Type == BookableUnitType.Bundle", source, StringComparison.Ordinal);
        Assert.Contains("bundle.BundleType != BundleType.SeasonPass", source, StringComparison.Ordinal);
        Assert.Contains("if (!CanSellSelectedSeats) return;", source, StringComparison.Ordinal);
    }

    private static string ReadAppSource(string relativePath)
    {
        var fullPath = Path.Combine(GetAppSourceRoot(), relativePath);
        return File.ReadAllText(fullPath);
    }

    private static string GetAppSourceRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            var candidate = Path.Combine(
                directory.FullName,
                "Odasoft.XBOL.AdminPortal",
                "Odasoft.XBOL.AdminPortal.csproj");

            if (File.Exists(candidate))
            {
                return Path.GetDirectoryName(candidate)!;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Could not locate Odasoft.XBOL.AdminPortal project root.");
    }
}
