using MudBlazor;
using Odasoft.XBOL.AdminPortal.ViewModels;

namespace Odasoft.XBOL.AdminPortal.Services;

// TODO: Implement actual API
public class HttpEventService(HttpClient httpClient) : IEventService
{
    public async Task<GridData<EventViewModel>> GetEventsAsync(
        int page,
        int pageSize,
        string? sortColumn,
        bool sortDescending,
        EventFilterParameters? filters = null
    )
    {
        var query = $"?page={page + 1}&size={pageSize}";

        if (!string.IsNullOrEmpty(sortColumn))
        {
            query += $"&sort={sortColumn}&desc={sortDescending}";
        }

        // TODO: Update query parameter names when API is ready
        if (filters?.Venues.Count > 0)
        {
            foreach (var venue in filters.Venues)
            {
                query += $"&venue={Uri.EscapeDataString(venue)}";
            }
        }

        // TODO: Update query parameter names when API is ready
        if (filters?.Types.Count > 0)
        {
            foreach (var type in filters.Types)
            {
                query += $"&type={Uri.EscapeDataString(type)}";
            }
        }

        // TODO: Update query parameter names and date format when API is ready
        if (filters?.DateFrom.HasValue == true)
        {
            query += $"&dateFrom={filters.DateFrom.Value:yyyy-MM-dd}";
        }

        // TODO: Update query parameter names and date format when API is ready
        if (filters?.DateTo.HasValue == true)
        {
            query += $"&dateTo={filters.DateTo.Value:yyyy-MM-dd}";
        }

        var response = await httpClient.GetFromJsonAsync<PagedApiResponse<EventViewModel>>(
            $"api/events{query}"
        );

        if (response == null)
        {
            return new GridData<EventViewModel> { TotalItems = 0, Items = [] };
        }

        return new GridData<EventViewModel>
        {
            TotalItems = response.TotalCount,
            Items = response.Items,
        };
    }

    // TODO: Update endpoint when API is ready
    public async Task<List<string>> GetVenuesAsync()
    {
        var response = await httpClient.GetFromJsonAsync<List<string>>("api/venues");
        return response ?? new List<string>();
    }

    // TODO: Update endpoint when API is ready
    public async Task<List<string>> GetEventTypesAsync()
    {
        var response = await httpClient.GetFromJsonAsync<List<string>>("api/event-types");
        return response ?? new List<string>();
    }

    // TODO: Update endpoint when API is ready
    public async Task<bool> CancelEventAsync(Guid eventId)
    {
        var response = await httpClient.DeleteAsync($"api/events/{eventId}");
        return response.IsSuccessStatusCode;
    }

    private class PagedApiResponse<T>
    {
        public IEnumerable<T> Items { get; set; } = [];
        public int TotalCount { get; set; }
    }
}
