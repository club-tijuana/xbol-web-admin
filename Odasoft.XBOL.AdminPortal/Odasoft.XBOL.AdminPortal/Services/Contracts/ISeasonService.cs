using Odasoft.XBOL.AdminPortal.ViewModels.Shared;

namespace Odasoft.XBOL.AdminPortal.Services.Contracts
{
    public interface ISeasonService
    {
        public Task<List<Business.SeasonSelectorItem>> GetSeasonSelectorItemsAsync();

        public Task<ImageHeroBannerViewModel> GetSeasonBannerByEventAsync(long eventId);
    }
}
