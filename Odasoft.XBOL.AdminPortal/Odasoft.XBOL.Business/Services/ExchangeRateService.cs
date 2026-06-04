namespace Odasoft.XBOL.Business.Services
{
    public class ExchangeRateService
    {
        private IAdminClient _adminClient;

        public ExchangeRateService(IAdminClient adminClient)
        {
            _adminClient = adminClient;
        }

        public async Task<ExchangeRateResponse> GetExchangeRateByEventAsync(long eventId)
        {
            var response = await _adminClient.GetExchangeRateByEventAsync(eventId);
            return response;
        }

        public async Task<ExchangeRateResponse> GetExchangeRateBySeasonAsync(long seasonId)
        {
            var response = await _adminClient.GetExchangeRateBySeasonAsync(seasonId);
            return response;
        }
    }
}
