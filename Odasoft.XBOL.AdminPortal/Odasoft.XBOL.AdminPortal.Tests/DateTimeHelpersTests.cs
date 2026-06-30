using Odasoft.XBOL.AdminPortal.Helpers;
using Odasoft.XBOL.Common.Options;
using Xunit;

namespace Odasoft.XBOL.AdminPortal.Tests;

public sealed class DateTimeHelpersTests
{
    [Fact]
    public void ToDateTimeOffset_uses_configured_timezone_for_selected_local_time()
    {
        var options = new LocalizationOptions
        {
            TimeZoneId = "America/Tijuana"
        };

        var result = DateTimeHelpers.ToDateTimeOffset(
            new DateTime(2026, 6, 27),
            new TimeSpan(10, 0, 0),
            options);

        Assert.Equal(new DateTimeOffset(2026, 6, 27, 10, 0, 0, TimeSpan.FromHours(-7)), result);
        Assert.Equal(new DateTimeOffset(2026, 6, 27, 17, 0, 0, TimeSpan.Zero), result!.Value.ToUniversalTime());
    }

    [Fact]
    public void ToNullableDateTime_returns_configured_timezone_display_time()
    {
        var options = new LocalizationOptions
        {
            TimeZoneId = "America/Tijuana"
        };
        var storedUtc = new DateTimeOffset(2026, 6, 27, 17, 0, 0, TimeSpan.Zero);

        var result = DateTimeHelpers.ToNullableDateTime(storedUtc, options);

        Assert.Equal(new DateTime(2026, 6, 27, 10, 0, 0), result);
    }
}
