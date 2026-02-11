using Odasoft.XBOL.AdminPortal.Services.Contracts;
using Odasoft.XBOL.AdminPortal.ViewModels.Shared;
using Odasoft.XBOL.Business;

namespace Odasoft.XBOL.AdminPortal.Services
{
    public class SeasonService(IAdminClient apiClient) : ISeasonService
    {
        public async Task<SeasonListItemPagedResponse> GetSeasonsAsync(SeasonStatus? status, string? sortBy, bool? descending)
        {
            return await apiClient.GetSeasonsAsync(null, null, status, null, sortBy, descending, null, null);
        }

        public async Task<SeasonResult> GetSeasonByIdAsync(long id)
        {
            return await apiClient.GetSeasonByIdAsync(id);
        }

        public async Task<ImageHeroBannerViewModel> GetSeasonBannerAsync(long seasonId)
        {
            var season = await apiClient.GetSeasonByIdAsync(seasonId);

            return new ImageHeroBannerViewModel
            {
                ImageUrl = season.BannerImageUrl,
                Title = season.Name,
                Subtitle = season.Description,
                Venue = season.Venue,
                StartDateTime = season.StartDate?.UtcDateTime,
                EndDateTime = season.EndDate?.UtcDateTime,
            };
        }
    }
}
