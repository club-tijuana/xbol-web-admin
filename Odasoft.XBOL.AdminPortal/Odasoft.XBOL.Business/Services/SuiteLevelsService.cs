namespace Odasoft.XBOL.Business.Services
{
    public class SuiteLevelsService
    {
        private IAdminClient _adminClient;

        public SuiteLevelsService(IAdminClient adminClient)
        {
            _adminClient = adminClient;
        }

        public async Task<long> CreateSuiteLevelAsync(CreateSuiteLevelRequest request)
        {
            long suiteLevelId = await _adminClient.CreateSuiteLevelAsync(request);

            return suiteLevelId;
        }
    }
}
