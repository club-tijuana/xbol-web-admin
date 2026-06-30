namespace Odasoft.XBOL.Business.Services
{
    public class PriceReferenceService
    {
        public readonly IAdminClient _adminClient;

        public PriceReferenceService(IAdminClient adminClient)
        {
            _adminClient = adminClient;
        }

        public async Task<PriceReferenceResponse> GetPriceReferenceAsync(AdminSaleType saleType, long referenceId, long venueMapId)
        {
            return await _adminClient.GetPriceReferenceAsync(saleType, referenceId, venueMapId);
        }

        public async Task<PriceReferenceResponse> SavePriceReferenceAsync(PriceReferenceRequest request)
        {
            return await _adminClient.SavePriceReferenceAsync(request);
        }

        public async Task<GeneratePriceListResponse> GeneratePriceListAsync(AdminSaleType saleType, long referenceId)
        {
            return await _adminClient.GeneratePriceListAsync(saleType, referenceId);
        }
    }
}
