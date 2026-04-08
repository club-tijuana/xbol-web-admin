using Odasoft.XBOL.Models.DTO;

namespace Odasoft.XBOL.Business.Services
{
    public class CatalogsService
    {
        private readonly IAdminClient _adminClient;

        public CatalogsService(IAdminClient adminClient)
        {
            _adminClient = adminClient;
        }

        public async Task<List<ListItem>> GetVenuesAsync()
        {
            var venues = await _adminClient.GetVenueCatalogAsync();
            return venues.OrderBy(x => x.Name).ToList();
        }

        public async Task<List<ListItem>> GetVenueMapsByVenueId(long venueId)
        {
            var venueMaps = await _adminClient.GetVenueMapCatalogByVenueIdAsync(venueId);

            return venueMaps.OrderBy(x => x.Name).ToList();
        }

        public async Task<List<string>> GetVenueCitiesAsync()
        {
            var venueCities = await _adminClient.GetVenueCitiesCatalogAsync();
            return venueCities.Order().ToList();
        }

        public async Task<List<ListItem>> GetSuiteLevelsAsync()
        {
            var suiteLevels = await _adminClient.GetSuiteLevelCatalogAsync();
            return suiteLevels.OrderBy(x => x.Name).ToList();
        }

        public async Task<List<ListItem>> GetSuitesBySuiteLevelIdAsync(long suiteLevelId)
        {
            var agreementTypes = await _adminClient.GetSuiteCatalogBySuiteLevelIdAsync(suiteLevelId);
            return agreementTypes.OrderBy(x => x.Name).ToList();
        }

        public async Task<List<ListItem>> GetEventsAsync()
        {
            var events = await _adminClient.GetEventCatalogAsync();
            return events.OrderBy(x => x.Name).ToList();
        }

        public async Task<List<PhoneRegionCodeResponse>> GetPhoneRegionCodesAsync()
        {
            var regionCodes = await _adminClient.GetPhoneRegionCodesAsync();
            return regionCodes.OrderBy(x => x.RegionCode).ToList();
        }

        public async Task<List<Amenity>> GetAmenitiesAsync()
        {
            var amenities = await _adminClient.GetAmenitiesCatalogAsync();
            return amenities
                    .Select(x => new Amenity
                    {
                        IconIdentifier = x.IconIdentifier,
                        Id = x.Id,
                        Name = x.Name
                    })
                    .OrderBy(x => x.Name)
                    .ToList();
        }
    }
}
