namespace Odasoft.XBOL.AdminPortal.Helpers;

public static class DateTimeHelpers
{
    public static DateTimeOffset ToDateTimeOffset(DateTime? date) =>
        date.HasValue ? new DateTimeOffset(date.Value, TimeSpan.Zero) : default;

    public static DateTime? ToNullableDateTime(DateTimeOffset? dto) =>
        dto.HasValue && dto.Value != default ? dto.Value.DateTime : null;
}
