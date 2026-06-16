using Odasoft.XBOL.AdminPortal.Services;
using Xunit;

namespace Odasoft.XBOL.AdminPortal.Tests;

public sealed class SeasonPassPurchaseWindowTests
{
    private static readonly DateTimeOffset Now = new(2026, 6, 16, 12, 0, 0, TimeSpan.Zero);

    [Fact]
    public void CanBuy_requires_complete_sale_window()
    {
        Assert.False(SeasonPassPurchaseWindow.CanBuy(null, Now.AddDays(1), false, null, true, Now));
        Assert.False(SeasonPassPurchaseWindow.CanBuy(Now.AddDays(-1), null, false, null, true, Now));
        Assert.True(SeasonPassPurchaseWindow.CanBuy(Now.AddDays(-1), Now.AddDays(1), false, null, true, Now));
    }

    [Fact]
    public void CanBuy_requires_bookable_bundle()
    {
        Assert.False(SeasonPassPurchaseWindow.CanBuy(
            Now.AddDays(-1),
            Now.AddDays(1),
            false,
            null,
            false,
            Now));
        Assert.True(SeasonPassPurchaseWindow.CanBuy(
            Now.AddDays(-1),
            Now.AddDays(1),
            false,
            null,
            true,
            Now));
    }

    [Fact]
    public void CanBuy_renewal_bundle_waits_until_renewal_end()
    {
        Assert.False(SeasonPassPurchaseWindow.CanBuy(Now.AddDays(-1), Now.AddDays(10), true, Now.AddDays(1), true, Now));
        Assert.True(SeasonPassPurchaseWindow.CanBuy(Now.AddDays(-10), Now.AddDays(10), true, Now.AddDays(-1), true, Now));
    }

    [Fact]
    public void IsRenewalOpen_requires_complete_sale_and_renewal_windows()
    {
        Assert.False(SeasonPassPurchaseWindow.IsRenewalOpen(null, Now.AddDays(10), Now.AddDays(-1), Now.AddDays(1), true, Now));
        Assert.False(SeasonPassPurchaseWindow.IsRenewalOpen(Now.AddDays(-10), Now.AddDays(10), null, Now.AddDays(1), true, Now));
        Assert.False(SeasonPassPurchaseWindow.IsRenewalOpen(Now.AddDays(-10), Now.AddDays(10), Now.AddDays(-1), null, true, Now));
        Assert.True(SeasonPassPurchaseWindow.IsRenewalOpen(Now.AddDays(-10), Now.AddDays(10), Now.AddDays(-1), Now.AddDays(1), true, Now));
    }

    [Fact]
    public void IsRenewalOpen_requires_bookable_bundle()
    {
        Assert.False(SeasonPassPurchaseWindow.IsRenewalOpen(
            Now.AddDays(-10),
            Now.AddDays(10),
            Now.AddDays(-1),
            Now.AddDays(1),
            false,
            Now));
        Assert.True(SeasonPassPurchaseWindow.IsRenewalOpen(
            Now.AddDays(-10),
            Now.AddDays(10),
            Now.AddDays(-1),
            Now.AddDays(1),
            true,
            Now));
    }

    [Fact]
    public void CanRenew_remains_available_after_renewal_end_until_sale_closes()
    {
        Assert.False(SeasonPassPurchaseWindow.CanRenew(
            Now.AddDays(-10),
            Now.AddDays(10),
            Now.AddDays(1),
            Now.AddDays(5),
            true,
            Now));
        Assert.True(SeasonPassPurchaseWindow.CanRenew(
            Now.AddDays(-10),
            Now.AddDays(10),
            Now.AddDays(-5),
            Now.AddDays(1),
            true,
            Now));
        Assert.True(SeasonPassPurchaseWindow.CanRenew(
            Now.AddDays(-10),
            Now.AddDays(10),
            Now.AddDays(-5),
            Now.AddDays(-1),
            true,
            Now));
        Assert.False(SeasonPassPurchaseWindow.CanRenew(
            Now.AddDays(-10),
            Now.AddDays(-1),
            Now.AddDays(-5),
            Now.AddDays(-2),
            true,
            Now));
    }

    [Fact]
    public void RequiresSeatSelectionForRenewal_starts_at_renewal_end()
    {
        Assert.False(SeasonPassPurchaseWindow.RequiresSeatSelectionForRenewal(Now.AddTicks(1), Now));
        Assert.True(SeasonPassPurchaseWindow.RequiresSeatSelectionForRenewal(Now, Now));
        Assert.True(SeasonPassPurchaseWindow.RequiresSeatSelectionForRenewal(Now.AddTicks(-1), Now));
    }
}
