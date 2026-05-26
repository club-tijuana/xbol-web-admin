using MudBlazor;
using Odasoft.XBOL.AdminPortal.ViewModels;
using Odasoft.XBOL.Business;

namespace Odasoft.XBOL.AdminPortal.Services;

// TODO: Move to Business project

public class ApiEventService(IAdminClient adminClient) : IEventService
{
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
            e.PosterImageUrl ?? "",
            e.BannerImageUrl ?? ""
        )).ToArray() ?? [];

        return new GridData<EventViewModel>
        {
            TotalItems = response.TotalCount,
            Items = items
        };
    }

    public async Task<EventInfoDTO> GetEventByIdAsync(long id) => await adminClient.GetEventByIdAsync(id);

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
            e.PosterImageUrl ?? "",
            e.BannerImageUrl ?? "",
            e.IsSeason
        )).ToArray() ?? [];

        return new GridData<EventViewModel>
        {
            TotalItems = response.TotalCount,
            Items = items
        };
    }

    public async Task<List<EventCategoryResult>> GetCategoriesAsync()
    {
        var result = await adminClient.GetCategoriesAsync();
        return result.ToList();
    }

    public async Task<EventResult> CreateEventAsync(EventRequest request) => await adminClient.CreateEventAsync(request);

    public async Task UpdateEventAsync(long id, EventRequest request) => await adminClient.UpdateEventAsync(id, request);

    public async Task DeleteEventAsync(long id)
            => await adminClient.DeleteEventAsync(id);

    private static string FormatCategories(ICollection<EventCategoryResult>? categories)
        => categories is { Count: > 0 }
            ? string.Join(", ", categories.Select(c => c.DisplayName))
            : "";

    public async Task ApproveEventAsync(long id) => await adminClient.ApproveEventAsync(id);

    public async Task RejectEventAsync(long id) => await adminClient.RejectEventAsync(id);

    public async Task PublishEventAsync(long id) => await adminClient.PublishEventAsync(id);
}
