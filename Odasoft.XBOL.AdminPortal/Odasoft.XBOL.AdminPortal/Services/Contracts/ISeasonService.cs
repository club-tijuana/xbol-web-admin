using Odasoft.XBOL.AdminPortal.ViewModels.Shared;
using Odasoft.XBOL.Business;

namespace Odasoft.XBOL.AdminPortal.Services.Contracts
{
    public interface ISeasonService
    {
        Task<SeasonListItemPagedResponse> GetSeasonsAsync(SeasonStatus? status, SeasonPeriod? period, string? sortBy, bool? descending);
        Task<SeasonResult> GetSeasonByIdAsync(long id);
        Task<ImageHeroBannerViewModel> GetSeasonBannerAsync(long seasonId);
        Task<SeasonResult> CreateSeasonAsync(CreateSeasonRequest request);
        Task UpdateSeasonAsync(long id, UpdateSeasonRequest request);
        Task DeleteSeasonAsync(long id);
    }
}
