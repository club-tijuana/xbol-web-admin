using MudBlazor;
using Odasoft.XBOL.Business;

namespace Odasoft.XBOL.AdminPortal.Services.Contracts;

public interface ISeasonPassService
{
    public Task<GridData<OrderListItem>> GetSeasonPasssAsync(OrderListFilters filters);

    public Task<ClientSeasonEvent> GetClientSeasonEventInfo(ClientFilter filter);

    public Task<BookingResult> BookSeasonAsync(SeasonBookingRequest request);

    public Task<ClientSeasonEvent> GetClientSeasonEventByOrderReference(string orderReference);

    public Task<List<EnumItemDto>> GetSeasonPassStatusListAsync();

    public Task<List<EnumItemDto>> GetSeasonPassSuspendedReasonListAsync();

    public Task<List<EnumItemDto>> GetSeasonPassRenewalTypeList();

    public Task UpdateSeasonPassStatusAsync(long id, SeasonPassStatusRequest request);
}
