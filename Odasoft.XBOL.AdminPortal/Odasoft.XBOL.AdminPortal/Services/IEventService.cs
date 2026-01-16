using MudBlazor;
using Odasoft.XBOL.AdminPortal.ViewModels;

namespace Odasoft.XBOL.AdminPortal.Services;

public interface IEventService
{
    Task<GridData<EventViewModel>> GetEventsAsync(
        int page,
        int pageSize,
        string? sortColumn,
        bool sortDescending,
        string? search = null,
        EventFilterParameters? filters = null
    );

    Task<List<VenueListItem>> GetVenuesAsync();
    Task<List<string>> GetCategoriesAsync();
}
