using MudBlazor;
using Odasoft.XBOL.AdminPortal.Services.Contracts;
using Odasoft.XBOL.Business;

namespace Odasoft.XBOL.AdminPortal.Services
{
    public class SeasonPassService(IAdminClient apiClient) : ISeasonPassService
    {
        public async Task<List<EnumItemDto>> GetSeasonPassRenewalTypeList()
        {
            var result = await apiClient.GetSeasonPassRenewalTypeListAsync();
            return result.ToList();
        }

        public async Task<GridData<OrderListItem>> GetSeasonPasssAsync(OrderListFilters filters)
        {
            var response = await apiClient.GetOrderListAsync(filters);

            return new GridData<OrderListItem>
            {
                TotalItems = response.TotalItems,
                Items = response.Items ?? [],
            };
        }

        public async Task<List<EnumItemDto>> GetSeasonPassStatusListAsync()
        {
            var result = await apiClient.GetSeasonPassStatusListAsync();
            return result.ToList();
        }

        public async Task<List<EnumItemDto>> GetSeasonPassSuspendedReasonListAsync()
        {
            var result = await apiClient.GetSeasonPassSuspendedReasonListAsync();
            return result.ToList();
        }

        public async Task UpdateSeasonPassStatusAsync(long id, SeasonPassStatusRequest request)
        {
            await apiClient.UpdateSeasonPassStatusAsync(id, request);
        }
    }
}
