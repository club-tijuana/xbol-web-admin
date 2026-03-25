using Odasoft.XBOL.Business.Services.Contracts;
using Odasoft.XBOL.Models.Parameters;

namespace Odasoft.XBOL.Business.Services
{
    public class SuitesService : ISuitesService
    {
        private IAdminClient _adminClient;

        public SuitesService(IAdminClient adminClient)
        {
            _adminClient = adminClient;
        }

        public async Task<SuiteResponsePagedResponse> GetSuitesAsync(SuitesFilterParameters parameters)
        {
            var levels = parameters.Levels is not null
                    ? string.Join(",", parameters.Levels)
                    : string.Empty;

            var suites = await _adminClient.GetSuitesAsync(
                levels,
                parameters.SearchTerm,
                parameters.SortBy,
                parameters.Descending,
                parameters.Page,
                parameters.PageSize);

            return suites;
        }

        public async Task<SuiteResponse> GetSuiteByIdAsync(long suiteId)
        {
            return await _adminClient.GetSuiteByIdAsync(suiteId);
        }

        public async Task<long> CreateSuiteAsync(SuiteRequest request)
        {
            return await _adminClient.CreateSuiteAsync(request);
        }

        public async Task UpdateSuiteAsync(long suiteId, SuiteRequest request)
        {
            await _adminClient.UpdateSuiteAsync(suiteId, request);
        }

        public async Task DeleteSuiteAsync(long suiteId)
        {
            await _adminClient.DeleteSuiteAsync(suiteId);
        }
    }
}
