namespace Odasoft.XBOL.AdminPortal.ViewModels;

// TODO: Move to Model project

public record EventViewModel(
    long Id,
    DateTime DateTime,
    string EventName,
    string Category,
    string Venue,
    int Available,
    int Total,
    string? ExternalEventKey,
    string? PosterImageUrl = null,
    bool? IsSeason = null
)
{
    public string Availability => $"{Available:D3}/{Total:D3}";
}
