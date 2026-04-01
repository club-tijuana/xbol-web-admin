using Odasoft.XBOL.Common.Constants;

namespace Odasoft.XBOL.Business.Services
{
    public class SupportService
    {
        private readonly IAdminClient _adminClient;

        public SupportService(IAdminClient adminClient)
        {
            _adminClient = adminClient;
        }

        public async Task<OrderInfoResponse?> SearchOrderByReferenceAsync(string reference)
        {
            try
            {
                return await _adminClient.GetOrderInfoByReferenceAsync(reference);
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == HttpStatusCodes.NOT_FOUND
                    || ex.StatusCode == HttpStatusCodes.NO_CONTENT)
                {
                    return null;
                }

                throw;
            }
        }

        public async Task<ICollection<OrderActionResponse>> GetOrderActionLogAsync(long orderId)
        {
            return await _adminClient.GetOrderActionsAsync(orderId);
        }

        public async Task<bool> PerformOrderActionAsync(long orderId, OrderActionRequest request)
        {
            long logId = await _adminClient.PerformOrderActionAsync(orderId, request);

            return logId > 0;
        }
    }
}
