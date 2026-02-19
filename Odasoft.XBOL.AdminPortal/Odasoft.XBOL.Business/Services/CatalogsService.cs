namespace Odasoft.XBOL.Business.Services
{
    public class CatalogsService
    {
        private readonly IAdminClient _adminClient;

        public CatalogsService(IAdminClient adminClient)
        {
            _adminClient = adminClient;
        }

        public async Task<ICollection<ListItem>> GetVenuesAsync()
        {
            var venues = await _adminClient.GetVenueCatalogAsync();
            return venues.OrderBy(x => x.Name).ToList();
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
    }
}
