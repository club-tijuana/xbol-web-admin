using MudBlazor;
using Odasoft.XBOL.AdminPortal.ViewModels;

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
        bool? onSale = null
    );

    Task<GridData<EventViewModel>> GetEventsOnSaleAsync(
        int page,
        int pageSize,
        string? sortColumn,
        bool sortDescending,
        string? search = null,
        EventFilterParameters? filters = null
    );

    Task<List<string>> GetCategoriesAsync();
}
