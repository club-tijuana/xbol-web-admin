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

        public async Task<FileResponse?> DownloadPricesCsvAsync(long? scheduleId, long venueMapId)
        {
            try
            {
                PricesCsvRequest request = new()
                {
                    ScheduleId = scheduleId,
                    VenueMapId = venueMapId
                };
                return await _adminClient.DownloadPricesCsvAsync(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error downloading prices template: {ex.Message}");
                return null;
            }
        }

        public async Task<ICollection<PriceResponse>> ValidatePricesCsvAsync(long venueMapId, FileParameter file)
        {
            return await _adminClient.ValidatePricesCsvAsync(venueMapId, file);
        }
    }
}
