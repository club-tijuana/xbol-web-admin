namespace Odasoft.XBOL.Business.Services
{
    public class PriceService(IAdminClient _adminClient)
    {
        public async Task<PricesResponse> CreatePricesAsync(PricesRequest request)
        {
            return await _adminClient.CreatePricesAsync(request);
        }

        public async Task UpdatePricesAsync(long priceId, PricesRequest request)
        {
            await _adminClient.UpdatePricesAsync(priceId, request);
        }

        public async Task<PricesResponse> GetPricesByScheduleAndVenueAsync(long scheduleId, long venueId)
        {
            return await _adminClient.GetPricesByScheduleAndVenueAsync(scheduleId, venueId);
        }
    }
}
