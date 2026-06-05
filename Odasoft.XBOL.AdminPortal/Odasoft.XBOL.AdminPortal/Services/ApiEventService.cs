using MudBlazor;
using Odasoft.XBOL.AdminPortal.ViewModels;
using Odasoft.XBOL.Business;

namespace Odasoft.XBOL.AdminPortal.Services;

// TODO: Move to Business project

public class ApiEventService(IAdminClient adminClient, AdminMediaUrlResolver mediaUrlResolver) : IEventService
{
    public async Task<GridData<EventViewModel>> GetEventCatalogAsync(
        int page,
        int pageSize,
        string? sortColumn,
        bool sortDescending,
        string? search = null,
        EventFilterParameters? filters = null,
        EventCatalogItemType? itemType = null,
        BundleType? bundleType = null,
        bool? upcoming = null)
    {
        var response = await adminClient.GetEventCatalogItemsAsync(
            searchTerm: search,
            itemType: itemType,
            bundleType: bundleType,
            status: null,
            venue: filters?.Venues.FirstOrDefault(),
            startDate: ToOffset(filters?.DateFrom),
            endDate: ToEndOffset(filters?.DateTo),
            upcoming: upcoming,
            sortBy: sortColumn,
            descending: sortDescending,
            page: page + 1,
            pageSize: pageSize
        );

        var items = response.Items?.Select(e => new EventViewModel(
            e.Id ?? 0,
            e.ScheduledStartDate?.DateTime ?? default,
            e.Name ?? "",
            FormatCategories(e.Categories),
            e.VenueName ?? "",
            e.AvailableSeats ?? 0,
            e.TotalSeats ?? 0,
            e.Status,
            e.ExternalEventKey,
            mediaUrlResolver.Resolve(e.PosterImageUrl),
            mediaUrlResolver.Resolve(e.BannerImageUrl),
            e.IsSeason,
            e.ItemType,
            e.BundleType,
            e.Code,
            e.EventScheduleId,
            e.VenueMapId
        )).ToArray() ?? [];

        return new GridData<EventViewModel>
        {
            TotalItems = response.TotalCount,
            Items = items
        };
    }

    public async Task<GridData<BundleScheduleViewModel>> GetBundleScheduleItemsAsync(
        long bundleId,
        int page,
        int pageSize,
        string? sortColumn,
        bool sortDescending,
        string? search = null,
        EventFilterParameters? filters = null)
    {
        var response = await adminClient.GetBundleScheduleItemsAsync(
            bundleId: bundleId,
            searchTerm: search,
            venue: filters?.Venues.FirstOrDefault(),
            venueMapId: null,
            startDate: ToOffset(filters?.DateFrom),
            endDate: ToEndOffset(filters?.DateTo),
            upcoming: null,
            sortBy: sortColumn,
            descending: sortDescending,
            page: page + 1,
            pageSize: pageSize
        );

        var items = response.Items?.Select(e => new BundleScheduleViewModel(
            e.EventId ?? 0,
            e.EventScheduleId ?? 0,
            e.ScheduledStartDate?.DateTime ?? default,
            e.Name ?? "",
            FormatCategories(e.Categories),
            e.VenueName ?? "",
            e.AvailableSeats ?? 0,
            e.TotalSeats ?? 0,
            e.Status,
            e.ExternalEventKey
        )).ToArray() ?? [];

        return new GridData<BundleScheduleViewModel>
        {
            TotalItems = response.TotalCount,
            Items = items
        };
    }

    public async Task<GridData<EventViewModel>> GetEventScheduleItemsAsync(
        int page,
        int pageSize,
        string? sortColumn,
        bool sortDescending,
        string? search = null,
        EventFilterParameters? filters = null,
        long? venueMapId = null,
        bool? upcoming = null)
    {
        var response = await adminClient.GetEventScheduleItemsAsync(
            searchTerm: search,
            venue: filters?.Venues.FirstOrDefault(),
            venueMapId: venueMapId,
            startDate: ToOffset(filters?.DateFrom),
            endDate: ToEndOffset(filters?.DateTo),
            upcoming: upcoming,
            sortBy: sortColumn,
            descending: sortDescending,
            page: page + 1,
            pageSize: pageSize
        );

        var items = response.Items?.Select(e => new EventViewModel(
            e.EventId ?? 0,
            e.ScheduledStartDate?.DateTime ?? default,
            e.Name ?? "",
            FormatCategories(e.Categories),
            e.VenueName ?? "",
            e.AvailableSeats ?? 0,
            e.TotalSeats ?? 0,
            null,
            e.ExternalEventKey,
            ItemType: EventCatalogItemType.Event,
            EventScheduleId: e.EventScheduleId,
            VenueMapId: e.VenueMapId
        )).ToArray() ?? [];

        return new GridData<EventViewModel>
        {
            TotalItems = response.TotalCount,
            Items = items
        };
    }

    public async Task<GridData<EventViewModel>> GetEventsAsync(
        int page,
        int pageSize,
        string? sortColumn,
        bool sortDescending,
        string? search = null,
        EventFilterParameters? filters = null,
        int? seasonId = null,
        bool? upcoming = null)
    {
        var response = await adminClient.GetEventsAsync(
            venues: filters?.Venues != null && filters.Venues.Count > 0 ? string.Join(",", filters.Venues) : null,
            categories: filters?.Categories != null && filters.Categories.Count > 0 ? string.Join(",", filters.Categories) : null,
            startDate: filters?.DateFrom,
            endDate: filters?.DateTo,
            search: search,
            sortBy: sortColumn,
            descending: sortDescending,
            page: page + 1,
            pageSize: pageSize,
            seasonId: seasonId,
            status: null,
            upcoming: upcoming
        );

        var items = response.Items?.Select(e => new EventViewModel(
            e.Id,
            e.ScheduledStartDate.DateTime,
            e.Name ?? "",
            FormatCategories(e.Categories),
            e.VenueName ?? "",
            e.AvailableSeats,
            e.TotalSeats,
            e.Status,
            e.ExternalEventKey,
            mediaUrlResolver.Resolve(e.PosterImageUrl),
            mediaUrlResolver.Resolve(e.BannerImageUrl)
        )).ToArray() ?? [];

        return new GridData<EventViewModel>
        {
            TotalItems = response.TotalCount,
            Items = items
        };
    }

    public async Task<EventInfoDTO> GetEventByIdAsync(long id) => await adminClient.GetEventByIdAsync(id);

    public async Task<BundleDTO> GetBundleByIdAsync(long id) => await adminClient.GetBundleByIdAsync(id);

    public async Task<GridData<EventViewModel>> GetEventsOnSaleAsync(
        int page,
        int pageSize,
        string? sortColumn,
        bool sortDescending,
        string? search = null,
        EventFilterParameters? filters = null)
    {
        var response = await adminClient.GetEventsOnSaleAsync(
            venues: filters?.Venues != null && filters.Venues.Count > 0 ? string.Join(",", filters.Venues) : null,
            categories: filters?.Categories != null && filters.Categories.Count > 0 ? string.Join(",", filters.Categories) : null,
            startDate: filters?.DateFrom,
            endDate: filters?.DateTo,
            search: search,
            sortBy: sortColumn,
            descending: sortDescending,
            page: page + 1,
            pageSize: pageSize
        );

        var items = response.Items?.Select(e => new EventViewModel(
            e.Id,
            e.ScheduledStartDate.DateTime,
            e.Name ?? "",
            FormatCategories(e.Categories),
            e.VenueName ?? "",
            e.AvailableSeats,
            e.TotalSeats,
            e.Status,
            e.ExternalEventKey,
            mediaUrlResolver.Resolve(e.PosterImageUrl),
            mediaUrlResolver.Resolve(e.BannerImageUrl),
            e.IsSeason
        )).ToArray() ?? [];

        return new GridData<EventViewModel>
        {
            TotalItems = response.TotalCount,
            Items = items
        };
    }

    public async Task<List<AdminEventCategoryResult>> GetCategoriesAsync()
    {
        var result = await adminClient.GetCategoriesAsync();
        return result.ToList();
    }

    public async Task<EventResult> CreateEventAsync(EventRequest request) => await adminClient.CreateEventAsync(request);

    public async Task<BundleDTO> CreateBundleAsync(BundleCreateRequest request) => await adminClient.CreateBundleAsync(request);

    public async Task UpdateEventAsync(long id, EventRequest request) => await adminClient.UpdateEventAsync(id, request);
    public async Task UpdateBundleAsync(long id, BundleUpdateRequest request) => await adminClient.UpdateBundleAsync(id, request);

    public async Task DeleteEventAsync(long id)
            => await adminClient.DeleteEventAsync(id);

    public async Task DeleteBundleAsync(long id)
            => await adminClient.DeleteBundleAsync(id);

    private static string FormatCategories(ICollection<AdminEventCategoryResult>? categories)
        => categories is { Count: > 0 }
            ? string.Join(", ", categories.Select(c => c.DisplayName))
            : "";

    private static DateTimeOffset? ToOffset(DateTime? value)
        => value is null ? null : new DateTimeOffset(DateTime.SpecifyKind(value.Value, DateTimeKind.Local));

    private static DateTimeOffset? ToEndOffset(DateTime? value)
    {
        if (value is null)
        {
            return null;
        }

        var end = value.Value.TimeOfDay == TimeSpan.Zero
            ? value.Value.Date.AddDays(1).AddTicks(-1)
            : value.Value;

        return new DateTimeOffset(DateTime.SpecifyKind(end, DateTimeKind.Local));
    }

    public async Task ApproveEventAsync(long id) => await adminClient.ApproveEventAsync(id);

    public async Task RejectEventAsync(long id) => await adminClient.RejectEventAsync(id);

    public async Task PublishEventAsync(long id) => await adminClient.PublishEventAsync(id);
}
