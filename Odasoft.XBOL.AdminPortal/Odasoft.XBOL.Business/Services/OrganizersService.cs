namespace Odasoft.XBOL.Business.Services
{
    public class OrganizersService(IAdminClient _adminClient)
    {
        public async Task<IEnumerable<ListItem>> GetOrganizersCatalogAsync() => await _adminClient.GetOrganizerCatalogAsync();
    }
}
