using Odasoft.XBOL.Common.Options;

namespace Odasoft.XBOL.AdminPortal.Helpers;

public static class DateTimeHelpers
{
    public static DateTimeOffset? ToDateTimeOffset(DateTime? date) =>
        date.HasValue ? new DateTimeOffset(date.Value, TimeSpan.Zero) : null;

    public static DateTimeOffset? ToDateTimeOffset(DateTime? date, TimeSpan? timeOfDay)
    {
        if (!date.HasValue)
        {
            return null;
        }
        DateTime combinedDateTime = date.Value.Date + (timeOfDay ?? TimeSpan.Zero);
        return new DateTimeOffset(combinedDateTime, TimeSpan.Zero);
    }

    public static DateTimeOffset? ToDateTimeOffset(
        DateTime? date,
        TimeSpan? timeOfDay,
        LocalizationOptions options)
    {
        if (!date.HasValue)
        {
            return null;
        }

        DateTime combinedDateTime = date.Value.Date + (timeOfDay ?? TimeSpan.Zero);
        return PaymentLinkDateTime.ToConfiguredOffset(combinedDateTime, options);
    }

    public static DateTime? ToNullableDateTime(DateTimeOffset? dto) =>
        dto.HasValue && dto.Value != default ? dto.Value.DateTime : null;

    public static DateTime? ToNullableDateTime(DateTimeOffset? dto, LocalizationOptions options) =>
        dto.HasValue && dto.Value != default
            ? PaymentLinkDateTime.ToDisplayDateTime(dto.Value, options)
            : null;

    public static TimeSpan? ToNullableTimeSpan(DateTimeOffset? dto) =>
        dto.HasValue && dto.Value != default ? dto.Value.TimeOfDay : null;

    public static TimeSpan? ToNullableTimeSpan(DateTimeOffset? dto, LocalizationOptions options) =>
        dto.HasValue && dto.Value != default
            ? PaymentLinkDateTime.ToDisplayDateTime(dto.Value, options).TimeOfDay
            : null;
}
