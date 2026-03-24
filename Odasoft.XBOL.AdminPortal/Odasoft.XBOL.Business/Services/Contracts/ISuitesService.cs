using Odasoft.XBOL.Models.Parameters;

namespace Odasoft.XBOL.Business.Services.Contracts
{
    public interface ISuitesService
    {
        Task<SuiteResultPagedResponse> GetSuitesAsync(SuitesFilterParameters parameters);
        Task<SuiteResult> GetSuiteByIdAsync(long id);
        Task CreateSuiteAsync(CreateSuiteRequest request);
        Task UpdateSuiteAsync(long suiteId, UpdateSuiteRequest request);
        Task DeleteSuiteAsync(long id);
    }
}
