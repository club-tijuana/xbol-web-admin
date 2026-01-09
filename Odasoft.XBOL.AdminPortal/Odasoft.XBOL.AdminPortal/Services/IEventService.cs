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
        EventFilterParameters? filters = null
    );

    Task<List<string>> GetVenuesAsync();
    Task<List<string>> GetEventTypesAsync();
    Task<bool> CancelEventAsync(Guid eventId);
}
