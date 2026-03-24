using Odasoft.XBOL.Business.Services.Contracts;
using Odasoft.XBOL.Models.Parameters;

namespace Odasoft.XBOL.Business.Services
{
    public class SuitesService(IAdminClient apiClient) : ISuitesService
    {
        public async Task<SuiteResultPagedResponse> GetSuitesAsync(SuitesFilterParameters parameters)
        {
            var levels = parameters.Levels is not null
                    ? string.Join(",", parameters.Levels)
                    : string.Empty;

            return await apiClient.GetSuitesAsync(
                levels,
                parameters.SearchTerm,
                parameters.SortBy,
                parameters.Descending,
                parameters.Page,
                parameters.PageSize);
        }

        public async Task<SuiteResult> GetSuiteByIdAsync(long id)
            => await apiClient.GetSuiteByIdAsync(id);

        public async Task CreateSuiteAsync(CreateSuiteRequest request)
            => await apiClient.CreateSuiteAsync(request);

        public async Task UpdateSuiteAsync(long suiteId, UpdateSuiteRequest request)
            => await apiClient.UpdateSuiteAsync(suiteId, request);

        public async Task DeleteSuiteAsync(long id)
            => await apiClient.DeleteSuiteAsync(id);
    }
}
