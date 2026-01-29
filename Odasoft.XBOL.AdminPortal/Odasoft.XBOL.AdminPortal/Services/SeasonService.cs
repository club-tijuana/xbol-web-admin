using Odasoft.XBOL.AdminPortal.Services.Contracts;
using Odasoft.XBOL.AdminPortal.ViewModels.Shared;
using Odasoft.XBOL.Business;

namespace Odasoft.XBOL.AdminPortal.Services
{
    public class SeasonService(IAdminClient apiClient) : ISeasonService
    {
        public async Task<ImageHeroBannerViewModel> GetSeasonBannerByEventAsync(long eventId)
        {
            var result = await apiClient.GetSeasonBannerAsync(eventId);
            if (result != null)
            {
                ImageHeroBannerViewModel banner = new()
                {
                    ImageUrl = result.ImageUrl,
                    Subtitle = result.Subtitle,
                    Title = result.Title,
                    Venue = result.Venue,
                    EndDateTime = result.EndDateTime?.UtcDateTime,
                    StartDateTime = result.StartDateTime?.UtcDateTime,
                };
                return banner;
            }
            return new ImageHeroBannerViewModel();
        }

        public async Task<List<SeasonSelectorItem>> GetSeasonSelectorItemsAsync()
        {
            var result = await apiClient.GetSeasonSelectorItemsAsync();
            return result.ToList();
        }

        public async Task<long?> GetLatestEventIdBySeasonAsync(long seasonId)
        {
            var result = await apiClient.GetLatestEventIdBySeasonAsync(seasonId);
            return result;
        }

        public async Task<string?> GetSeasonKeyAsync(long seasonId)
        {
            var result = await apiClient.GetSeasonKeyAsync(seasonId);
            return result.Value;
        }
    }
}
