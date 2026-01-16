using MudBlazor;
using Odasoft.XBOL.AdminPortal.ViewModels;

namespace Odasoft.XBOL.AdminPortal.Services;

public class ApiEventService(IAdminApiClient apiClient) : IEventService
{
    public async Task<GridData<EventViewModel>> GetEventsAsync(
        int page,
        int pageSize,
        string? sortColumn,
        bool sortDescending,
        string? search = null,
        EventFilterParameters? filters = null)
    {
        var response = await apiClient.GetEventsAsync(
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
            e.TotalSeats
        )).ToArray() ?? [];

        return new GridData<EventViewModel>
        {
            TotalItems = response.TotalCount,
            Items = items
        };
    }

    public async Task<List<VenueListItem>> GetVenuesAsync()
    {
        var result = await apiClient.GetVenuesAsync();
        return result.ToList();
    }

    public async Task<List<string>> GetCategoriesAsync()
    {
        var result = await apiClient.GetCategoriesAsync();
        return result.ToList();
    }
}
