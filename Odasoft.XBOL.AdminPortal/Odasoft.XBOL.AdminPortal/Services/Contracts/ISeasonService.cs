using Odasoft.XBOL.AdminPortal.ViewModels.Shared;
using Odasoft.XBOL.Business;

namespace Odasoft.XBOL.AdminPortal.Services.Contracts
{
    public interface ISeasonService
    {
        Task<SeasonListItemPagedResponse> GetSeasonsAsync(SeasonStatus? status, string? sortBy, bool? descending);
        Task<SeasonResult> GetSeasonByIdAsync(long id);
        Task<ImageHeroBannerViewModel> GetSeasonBannerAsync(long seasonId);
    }
}
