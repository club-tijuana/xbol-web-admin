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
        EventFilterParameters? filters = null,
        int? seasonId = null,
        bool? upcoming = null
    );

    Task<GridData<EventViewModel>> GetEventsOnSaleAsync(
        int page,
        int pageSize,
        string? sortColumn,
        bool sortDescending,
        string? search = null,
        EventFilterParameters? filters = null
    );

    Task<List<EventCategoryResult>> GetCategoriesAsync();
    Task<EventResult> CreateEventAsync(EventRequest request);
    Task UpdateEventAsync(long id, EventRequest request);
    Task DeleteEventAsync(long id);
    Task<EventInfoDTO> GetEventByIdAsync(long id);
    Task ApproveEventAsync(long id);
    Task RejectEventAsync(long id);
    Task PublishEventAsync(long id);
}
