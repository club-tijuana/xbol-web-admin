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
            e.Category ?? "",
            e.VenueName ?? "",
            e.AvailableSeats,
            e.TotalSeats,
            e.ExternalEventKey,
            e.PosterImageUrl ?? ""
        )).ToArray() ?? [];

        return new GridData<EventViewModel>
        {
            TotalItems = response.TotalCount,
            Items = items
        };
    }

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
            e.Category ?? "",
            e.VenueName ?? "",
            e.AvailableSeats,
            e.TotalSeats,
            e.ExternalEventKey,
            e.PosterImageUrl ?? "",
            e.IsSeason
        )).ToArray() ?? [];

        return new GridData<EventViewModel>
        {
            TotalItems = response.TotalCount,
            Items = items
        };
    }

    public async Task<List<string>> GetCategoriesAsync()
    {
        var result = await adminClient.GetCategoriesNamesAsync();
        return result.ToList();
    }

    public async Task<EventResult> CreateEventAsync(CreateEventRequest request) => await adminClient.CreateEventAsync(request);

    public async Task UpdateEventAsync(long id, UpdateEventRequest request) => await adminClient.UpdateEventAsync(id, request);

    public async Task DeleteEventAsync(long id)
            => await adminClient.DeleteEventAsync(id);
}
