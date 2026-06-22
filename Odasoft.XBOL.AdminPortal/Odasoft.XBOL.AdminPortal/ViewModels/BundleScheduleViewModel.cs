using Odasoft.XBOL.Business;

namespace Odasoft.XBOL.AdminPortal.ViewModels;

public record BundleScheduleViewModel(
    long EventId,
    long EventScheduleId,
    DateTime StartDateTime,
    DateTime? EndDateTime,
    string EventName,
    string Categories,
    string Venue,
    int Available,
    int Total,
    ScheduleStatus? Status,
    string? ExternalEventKey
)
{
    public string Availability => $"{Available:D3}/{Total:D3}";
}
