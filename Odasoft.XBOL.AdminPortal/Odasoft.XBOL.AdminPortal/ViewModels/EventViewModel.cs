namespace Odasoft.XBOL.AdminPortal.ViewModels;

// TODO: Move to Model project

public record EventViewModel(
    long Id,
    DateTime DateTime,
    string EventName,
    string Categories,
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
