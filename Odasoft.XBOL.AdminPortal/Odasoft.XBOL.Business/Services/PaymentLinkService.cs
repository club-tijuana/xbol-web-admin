namespace Odasoft.XBOL.Business.Services
{
    public class PaymentLinkService
    {
        private readonly IAdminClient _adminClient;

        public PaymentLinkService(IAdminClient adminClient)
        {
            _adminClient = adminClient;
        }

        public async Task Cancel(string code)
        {
            try
            {
                await _adminClient.CancelPaymentLinkAsync(code);
            }
            catch (ApiException ex)
            {
                throw;
            }
        }

        public async Task<PaymentLinkResponse> Regenerate(long orderId, PaymentLinkRequest request)
        {
            try
            {
                var response = await _adminClient.RegeneratePaymentLinkCodeAsync(orderId, request);
                return response;
            }
            catch (ApiException ex)
            {
                throw;
            }
        }
    }
}
