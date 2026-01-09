namespace Odasoft.XBOL.AdminPortal.ViewModels;

public record EventViewModel(
    Guid Id,
    DateTime DateTime,
    string TicketId,
    string EventName,
    string Type,
    string Venue,
    int Available,
    int Total
)
{
    public string Availability => $"{Available:D3}/{Total:D3}";
}
