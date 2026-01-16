namespace Odasoft.XBOL.AdminPortal.ViewModels;

public record EventViewModel(
    long Id,
    DateTime DateTime,
    string EventName,
    string Category,
    string Venue,
    int Available,
    int Total
)
{
    public string Availability => $"{Available:D3}/{Total:D3}";
}
