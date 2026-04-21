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

        public async Task<List<VenueResponse>> GetVenuesAsync()
        {
            var venues = await _adminClient.GetVenueCatalogAsync();
            return venues.OrderBy(x => x.Name).ToList();
        }

        public async Task<List<VenueMapResponse>> GetVenueMapsByVenueId(long venueId)
        {
            var venueMaps = await _adminClient.GetVenueMapCatalogByVenueIdAsync(venueId);

            return venueMaps.OrderBy(x => x.Name).ToList();
        }

        public async Task<List<string>> GetVenueCitiesAsync()
        {
            var venueCities = await _adminClient.GetVenueCitiesCatalogAsync();
            return venueCities.Order().ToList();
        }

        public async Task<List<ListItem>> GetSuiteLevelsAsync(long? venueId)
        {
            if (venueId.HasValue)
            {
                var venueSuiteLevels = await _adminClient.GetSuiteLevelCatalogByVenueAsync(venueId.Value);

                return venueSuiteLevels.OrderBy(x => x.Name).ToList();
            }

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

        public async Task<List<Amenity>> GetVenueAmenitiesAsync()
        {
            var amenities = await _adminClient.GetVenueAmenitiesCatalogAsync();

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

        public async Task<List<Amenity>> GetSuiteAmenitiesAsync()
        {
            var amenities = await _adminClient.GetSuiteAmenitiesCatalogAsync();

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
