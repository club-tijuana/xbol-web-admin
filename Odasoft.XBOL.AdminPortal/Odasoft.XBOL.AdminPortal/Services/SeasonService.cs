using Odasoft.XBOL.AdminPortal.Services.Contracts;
using Odasoft.XBOL.AdminPortal.ViewModels.Shared;
using Odasoft.XBOL.Business;

namespace Odasoft.XBOL.AdminPortal.Services
{
    public class SeasonService(IAdminClient apiClient) : ISeasonService
    {
        public async Task<SeasonListItemPagedResponse> GetSeasonsAsync(SeasonStatus? status, SeasonPeriod? period, string? sortBy, bool? descending)
        {
            return await apiClient.GetSeasonsAsync(
                startDate: null,
                endDate: null,
                status: status,
                // TODO: Do a proper fix
                period: [period.Value],
                searchTerm: null,
                sortBy: sortBy,
                descending: descending,
                page: null,
                pageSize: null);
        }

        public async Task<SeasonResult> GetSeasonByIdAsync(long id)
        {
            return await apiClient.GetSeasonByIdAsync(id);
        }

        public async Task<SeasonResult> CreateSeasonAsync(CreateSeasonRequest request)
            => await apiClient.CreateSeasonAsync(request);

        public async Task UpdateSeasonAsync(long id, UpdateSeasonRequest request)
            => await apiClient.UpdateSeasonAsync(id, request);

        public async Task DeleteSeasonAsync(long id)
            => await apiClient.DeleteSeasonAsync(id);

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
                ExternalKey = season.ExternalSeasonKey,
            };
        }
    }
}
