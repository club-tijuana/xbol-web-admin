namespace Odasoft.XBOL.Business.Services
{
    public class AdditionalChargesService(IAdminClient adminClient)
    {
        public async Task<ICollection<AdditionalChargeResponse>> GetChargesByPriceReferenceIdAsync(long priceReferenceId) => await adminClient.GetAdditionalChargesByPriceReferenceIdAsync(priceReferenceId);

        public async Task SyncChargesAsync(long priceReferenceId, IEnumerable<AdditionalChargeRequest> charges) => await adminClient.SyncChargesAsync(priceReferenceId, charges);
    }
}
