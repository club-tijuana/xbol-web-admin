using Odasoft.XBOL.Models.Parameters;

namespace Odasoft.XBOL.Business.Services.Contracts
{
    public interface ISuitesService
    {
        Task<SuiteResponsePagedResponse> GetSuitesAsync(SuitesFilterParameters parameters);

        Task<SuiteResponse> GetSuiteByIdAsync(long id);

        Task<long> CreateSuiteAsync(SuiteRequest request);

        Task UpdateSuiteAsync(long suiteId, SuiteRequest request);

        Task DeleteSuiteAsync(long id);
    }
}
