using Odasoft.XBOL.Common.Options;

namespace Odasoft.XBOL.AdminPortal.Helpers;

public static class PaymentLinkDateTime
{
    public static DateTimeOffset CreateDefaultExpiration(
        PaymentLinkOptions paymentLinkOptions,
        LocalizationOptions localizationOptions,
        DateTimeOffset? now = null)
    {
        var timeZone = ResolveTimeZone(localizationOptions.TimeZoneId);
        var expirationInstant = (now ?? DateTimeOffset.UtcNow)
            .ToUniversalTime()
            .AddHours(paymentLinkOptions.DefaultExpirationHours);

        return TimeZoneInfo.ConvertTime(expirationInstant, timeZone);
    }

    public static DateTimeOffset ToConfiguredOffset(DateTime localDateTime, LocalizationOptions options)
    {
        var timeZone = ResolveTimeZone(options.TimeZoneId);
        var unspecifiedLocal = DateTime.SpecifyKind(localDateTime, DateTimeKind.Unspecified);
        if (timeZone.IsInvalidTime(unspecifiedLocal))
        {
            throw new ArgumentException("The selected expiration date and time does not exist in the configured timezone.", nameof(localDateTime));
        }

        if (timeZone.IsAmbiguousTime(unspecifiedLocal))
        {
            var ambiguousOffset = timeZone.GetAmbiguousTimeOffsets(unspecifiedLocal).Min();
            return new DateTimeOffset(unspecifiedLocal, ambiguousOffset);
        }

        return new DateTimeOffset(unspecifiedLocal, timeZone.GetUtcOffset(unspecifiedLocal));
    }

    public static DateTime ToDisplayDateTime(DateTimeOffset value, LocalizationOptions options)
    {
        var timeZone = ResolveTimeZone(options.TimeZoneId);

        return TimeZoneInfo.ConvertTime(value, timeZone).DateTime;
    }

    public static TimeZoneInfo ResolveTimeZone(string timeZoneId)
    {
        return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
    }
}
