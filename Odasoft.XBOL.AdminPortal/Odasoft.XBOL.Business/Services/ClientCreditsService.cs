namespace Odasoft.XBOL.Business.Services
{
    public class ClientCreditsService(IAdminClient _adminClient)
    {
        public async Task<CreditAccountResult> GetCreditAccountAsync(long clientId)
        {
            var creditAccount = await _adminClient.GetCreditAccountByClientIdAsync(clientId);
            return creditAccount;
        }
    }
}
