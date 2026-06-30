using Odasoft.XBOL.AdminPortal.Helpers;
using Odasoft.XBOL.Common.Options;
using Xunit;

namespace Odasoft.XBOL.AdminPortal.Tests;

public sealed class PaymentLinkDateTimeTests
{
    [Fact]
    public void CreateDefaultExpiration_uses_configured_hours_and_timezone()
    {
        var options = new PaymentLinkOptions
        {
            DefaultExpirationHours = 48
        };
        var localizationOptions = new LocalizationOptions
        {
            TimeZoneId = "America/Tijuana"
        };
        var now = new DateTimeOffset(2026, 6, 25, 20, 0, 0, TimeSpan.Zero);

        var expiration = PaymentLinkDateTime.CreateDefaultExpiration(options, localizationOptions, now);

        Assert.Equal(new DateTimeOffset(2026, 6, 27, 13, 0, 0, TimeSpan.FromHours(-7)), expiration);
    }

    [Fact]
    public void CreateDefaultExpiration_adds_hours_to_elapsed_instant_across_dst()
    {
        var options = new PaymentLinkOptions
        {
            DefaultExpirationHours = 48
        };
        var localizationOptions = new LocalizationOptions
        {
            TimeZoneId = "America/Tijuana"
        };
        var now = new DateTimeOffset(2026, 3, 7, 20, 0, 0, TimeSpan.Zero);

        var expiration = PaymentLinkDateTime.CreateDefaultExpiration(options, localizationOptions, now);

        Assert.Equal(new DateTimeOffset(2026, 3, 9, 13, 0, 0, TimeSpan.FromHours(-7)), expiration);
    }

    [Fact]
    public void ToConfiguredOffset_preserves_admin_selected_local_time()
    {
        var options = new LocalizationOptions
        {
            TimeZoneId = "America/Tijuana"
        };
        var selected = new DateTime(2026, 12, 15, 9, 30, 0);

        var expiration = PaymentLinkDateTime.ToConfiguredOffset(selected, options);

        Assert.Equal(new DateTimeOffset(2026, 12, 15, 9, 30, 0, TimeSpan.FromHours(-8)), expiration);
    }

    [Fact]
    public void ToDisplayDateTime_returns_configured_timezone_local_time()
    {
        var options = new LocalizationOptions
        {
            TimeZoneId = "America/Tijuana"
        };
        var storedUtc = new DateTimeOffset(2026, 6, 27, 20, 0, 0, TimeSpan.Zero);

        var display = PaymentLinkDateTime.ToDisplayDateTime(storedUtc, options);

        Assert.Equal(new DateTime(2026, 6, 27, 13, 0, 0), display);
    }
}
