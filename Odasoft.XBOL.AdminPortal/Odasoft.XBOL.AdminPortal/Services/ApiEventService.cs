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
        bool? onSale = null)
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
            onSale: onSale
        );

        var items = response.Items?.Select(e => new EventViewModel(
            e.Id,
            e.ScheduledStartDate.DateTime,
            e.Name ?? "",
            e.Category ?? "",
            e.VenueName ?? "",
            e.AvailableSeats,
            e.TotalSeats,
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
}
