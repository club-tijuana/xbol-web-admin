using Odasoft.XBOL.Business;

namespace Odasoft.XBOL.AdminPortal.ViewModels;

// TODO: Move to Model project

public record EventViewModel(
    long Id,
    DateTime StartDateTime,
    DateTime? EndDateTime,
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
    long? VenueMapId = null,
    DateTimeOffset? OnSaleDate = null,
    DateTimeOffset? OffSaleDate = null,
    DateTimeOffset? RenewalStartDate = null,
    DateTimeOffset? RenewalEndDate = null,
    bool IsBookable = false,
    ScheduleStatus? ScheduleStatus = null
)
{
    public string Availability => $"{Available:D3}/{Total:D3}";
    public bool IsBundle => ItemType == EventCatalogItemType.Bundle;
    public bool HasRenewalWindow => RenewalStartDate.HasValue || RenewalEndDate.HasValue;
}
