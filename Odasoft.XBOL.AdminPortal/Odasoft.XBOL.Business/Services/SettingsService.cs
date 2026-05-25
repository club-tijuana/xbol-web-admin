namespace Odasoft.XBOL.Business.Services
{
    public class SettingsService
    {
        private readonly IAdminClient _adminClient;

        public SettingsService(IAdminClient adminClient)
        {
            _adminClient = adminClient;
        }

        public async Task<ClientCreditSettings> GetClientCreditSettingsAsync()
        {
            ClientCreditSettings settings = await _adminClient.GetClientCreditSettingsAsync();
            return settings;
        }
    }
}
