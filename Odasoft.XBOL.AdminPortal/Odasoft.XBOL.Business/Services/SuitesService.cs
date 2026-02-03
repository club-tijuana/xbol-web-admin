using Odasoft.XBOL.Models.Parameters;

namespace Odasoft.XBOL.Business.Services
{
    public class SuitesService
    {
        private IAdminClient _adminClient;

        public SuitesService(IAdminClient adminClient)
        {
            _adminClient = adminClient;
        }

        public async Task<SuiteResultPagedResponse> GetSuitesAsync(SuitesFilterParameters parameters)
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

        public async Task<SuiteResult> GetSuiteByIdAsync(long suiteId)
        {
            return await _adminClient.GetSuiteByIdAsync(suiteId);
        }

        public async Task<bool> CreateSuiteAsync(CreateSuiteRequest request)
        {
            return await _adminClient.CreateSuiteAsync(request);
        }

        public async Task<bool> UpdateSuiteAsync(UpdateSuiteRequest request)
        {
            return await _adminClient.UpdateSuiteAsync(request);
        }

        public async Task<bool> DeleteSuiteAsync(long suiteId)
        {
            return await _adminClient.DeleteSuiteAsync(suiteId);
        }
    }
}
