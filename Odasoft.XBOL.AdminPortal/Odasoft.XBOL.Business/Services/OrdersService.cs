using Odasoft.XBOL.Models.Parameters;

namespace Odasoft.XBOL.Business.Services
{
    public class OrdersService
    {
        private IAdminClient _adminClient;

        public OrdersService(IAdminClient adminClient)
        {
            _adminClient = adminClient;
        }

        public async Task<OrderResultPagedResponse> GetCreditOrdersAsync(OrdersFilterParameters parameters)
        {
            var response = await _adminClient.GetCreditOrdersAsync(
                parameters.ClientId,
                parameters.Events,
                parameters.StartDate,
                parameters.EndDate,
                parameters.SearchTerm,
                parameters.SortBy,
                parameters.Descending,
                parameters.Page,
                parameters.PageSize);

            return response;
        }

        public async Task<OrderRenewalInfoResponse> FindOrder(string orderReference)
        {
            return await _adminClient.GetOrderRenawalInfoByReferenceAsync(orderReference);
        }

        public async Task<CanRenewOrderResponse> CanOrderBeRenewedAsync(string orderReference)
        {
            return await _adminClient.CanOrderBeRenewedAsync(orderReference);
        }
    }
}
