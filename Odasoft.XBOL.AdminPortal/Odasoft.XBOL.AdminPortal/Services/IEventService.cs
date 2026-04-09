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
    Task<EventResult> CreateEventAsync(CreateEventRequest request);
    Task UpdateEventAsync(long id, UpdateEventRequest request);
    Task DeleteEventAsync(long id);
    Task<EventInfoDTO> GetEventByIdAsync(long id);
    Task<long> UploadEventImageAsync(long eventId, ImageType imageType, int order, FileParameter imageFile);
    Task DeleteEventImageAsync(long eventImageId);
    Task UpdateEventImageAsync(long eventImageId, ImageType imageType, int order, FileParameter file);
    Task<ICollection<EventImageResponse>> GetEventImageByTypeAsync(long eventId, ImageType imageType);
}
