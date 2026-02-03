using MudBlazor;
using Odasoft.XBOL.AdminPortal.ViewModels;
using Odasoft.XBOL.Business;

namespace Odasoft.XBOL.AdminPortal.Services;

// TODO: Move to Business project

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
