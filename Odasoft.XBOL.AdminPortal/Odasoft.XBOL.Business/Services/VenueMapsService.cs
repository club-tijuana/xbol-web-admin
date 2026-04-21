namespace Odasoft.XBOL.Business.Services
{
    public class VenueMapsService
    {
        private IAdminClient _adminClient;

        public VenueMapsService(IAdminClient adminClient)
        {
            _adminClient = adminClient;
        }

        public async Task<VenueMapResponse> GetVenueMapByIdAsync(long venueMapId)
        {
            return await _adminClient.GetVenueMapsByIdAsync(venueMapId);
        }

        public async Task<ICollection<VenueMapResponse>> GetVenueMapsByVenueAsync(long venueId)
        {
            return await _adminClient.GetVenueMapsByVenueAsync(venueId);
        }

        public async Task<Chart> GetMapChart(string mapKey)
        {
            return await _adminClient.GetVenueMapChartByKeyAsync(mapKey);
        }

        public async Task SaveVenueMapAsync(VenueMapRequest request)
        {
            await _adminClient.CreateVenueMapAsync(request);
        }

        public async Task UpdateVenueMapAsync(long venueMapId, VenueMapRequest request)
        {
            await _adminClient.UpdateVenueMapAsync(venueMapId, request);
        }

        public async Task DeleteVenueMapAsync(long venueMapId)
        {
            await _adminClient.DeleteVenueMapAsync(venueMapId);
        }
    }
}
