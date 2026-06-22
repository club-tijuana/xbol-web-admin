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

    Task<GridData<EventViewModel>> GetEventCatalogAsync(
        int page,
        int pageSize,
        string? sortColumn,
        bool sortDescending,
        string? search = null,
        EventFilterParameters? filters = null,
        EventCatalogItemType? itemType = null,
        BundleType? bundleType = null,
        bool? upcoming = null,
        bool? buyableOnly = null,
        AdminEventStatus? status = null
    );

    Task<GridData<BundleScheduleViewModel>> GetBundleScheduleItemsAsync(
        long bundleId,
        int page,
        int pageSize,
        string? sortColumn,
        bool sortDescending,
        string? search = null,
        EventFilterParameters? filters = null
    );

    Task<GridData<EventViewModel>> GetEventScheduleItemsAsync(
        int page,
        int pageSize,
        string? sortColumn,
        bool sortDescending,
        string? search = null,
        EventFilterParameters? filters = null,
        long? venueMapId = null,
        bool? upcoming = null
    );

    Task<List<AdminEventCategoryResult>> GetCategoriesAsync();
    Task<EventResult> CreateEventAsync(EventRequest request);
    Task<BundleDTO> CreateBundleAsync(BundleCreateRequest request);
    Task UpdateEventAsync(long id, EventRequest request);

    Task DeleteEventAsync(long id);

    Task<EventInfoDTO> GetEventByIdAsync(long id);

    Task ApproveEventAsync(long id);

    Task RejectEventAsync(long id);

    Task ResubmitEventAsync(long id);

    Task PublishEventAsync(long id);
    Task<BundleDTO> GetBundleByIdAsync(long id);
    Task DeleteBundleAsync(long id);
    Task UpdateBundleAsync(long id, BundleUpdateRequest request);
    Task AddBundleEventSchedulesAsync(long bundleId, IReadOnlyList<long> eventScheduleIds);
    Task RemoveBundleEventSchedulesAsync(long bundleId, IReadOnlyList<long> eventScheduleIds);
    Task SubmitBundleAsync(long id);
    Task ResubmitBundleAsync(long id);
    Task ApproveBundleAsync(long id);
    Task RejectBundleAsync(long id);
    Task PublishBundleAsync(long id);
}
