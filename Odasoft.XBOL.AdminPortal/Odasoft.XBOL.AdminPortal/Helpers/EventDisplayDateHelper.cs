using Odasoft.XBOL.Common.Constants;
using System.Globalization;

namespace Odasoft.XBOL.AdminPortal.Helpers
{
    public static class EventDisplayDateHelper
    {
        private static readonly CultureInfo Culture = new("es-MX");

        //Date range (15 Mar 2026 - 17 Mar 2026)
        public static string? GetDateRange(DateTime? start, DateTime? end)
        {
            if (!start.HasValue)
            {
                return null;
            }

            if (!end.HasValue || start.Value.Date == end.Value.Date)
            {
                return start.Value.ToString(FormatConstants.DATE_TIME, Culture);
            }

            return $"{start:dd MMM yyyy} - {end:dd MMM yyyy}";
        }

        // Short date (15 Mar)
        public static string? GetShortDate(DateTime? date)
        {
            return date?.ToString("dd MMM", Culture);
        }

        // Long date (15 de marzo de 2026)
        public static string? GetLongDate(DateTime? date)
        {
            return date?.ToString("dd 'de' MMMM 'de' yyyy", Culture);
        }

        // Time (20:30)
        public static string? GetTime(DateTime? date)
        {
            return date?.ToString("HH:mm", Culture);
        }

        // Datetime range (15 Mar 20:00 - 23:00)
        public static string? GetDateTimeRange(DateTime? start, DateTime? end)
        {
            if (!start.HasValue)
            {
                return null;
            }

            if (!end.HasValue)
            {
                return $"{GetLongDate(start)} {GetTime(start)}";
            }

            return $"{GetLongDate(start)} {GetTime(start)} - {GetTime(end)}";
        }

        // Event duration
        public static TimeSpan? GetDuration(DateTime? start, DateTime? end)
        {
            if (!start.HasValue || !end.HasValue)
            {
                return null;
            }

            return end.Value - start.Value;
        }

        // Duration (3 days / 2 hours)
        public static string? GetReadableDuration(DateTime? start, DateTime? end)
        {
            var duration = GetDuration(start, end);
            if (duration is null)
            {
                return null;
            }

            if (duration.Value.TotalDays >= 1)
            {
                return $"{Math.Ceiling(duration.Value.TotalDays)} días";
            }

            if (duration.Value.TotalHours >= 1)
            {
                return $"{Math.Ceiling(duration.Value.TotalHours)} horas";
            }

            return $"{Math.Ceiling(duration.Value.TotalMinutes)} minutos";
        }

        // Valid date range
        public static bool IsValidRange(DateTime? start, DateTime? end)
        {
            if (!start.HasValue)
            {
                return false;
            }

            if (!end.HasValue)
            {
                return true;
            }

            return start <= end;
        }

        // Days until datetime
        public static int? DaysUntil(DateTime? start)
        {
            if (!start.HasValue)
            {
                return null;
            }

            return (start.Value.Date - DateTime.Today).Days;
        }
    }
}
