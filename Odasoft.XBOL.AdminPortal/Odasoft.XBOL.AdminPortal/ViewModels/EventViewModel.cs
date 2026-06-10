using Odasoft.XBOL.Business;

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
    AdminEventStatus? Status,
    string? ExternalEventKey,
    string? PosterImageUrl = null,
    string? BannerImageUrl = null,
    bool? IsSeason = null,
    EventCatalogItemType? ItemType = null,
    BundleType? BundleType = null,
    string? Code = null,
    long? EventScheduleId = null,
    long? VenueMapId = null
)
{
    public string Availability => $"{Available:D3}/{Total:D3}";
    public bool IsBundle => ItemType == EventCatalogItemType.Bundle;
}
