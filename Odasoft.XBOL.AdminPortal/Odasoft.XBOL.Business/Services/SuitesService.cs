using Odasoft.XBOL.Business.Services.Contracts;
using Odasoft.XBOL.Models.DTO;

namespace Odasoft.XBOL.Business.Services
{
    public class SuitesService : ISuitesService
    {
        private IAdminClient _adminClient;

        public SuitesService(IAdminClient adminClient)
        {
            _adminClient = adminClient;
        }

        public async Task<ICollection<SuiteResponse>> GetSuitesAsync(long venueId)
        {
            ICollection<SuiteResponse> suites = await _adminClient.GetSuitesAsync(venueId);

            return suites;
        }

        public async Task<SuiteResponse> GetSuiteByIdAsync(long suiteId)
        {
            return await _adminClient.GetSuiteByIdAsync(suiteId);
        }

        public async Task<long> CreateSuiteAsync(SuiteRequest request)
        {
            return await _adminClient.CreateSuiteAsync(request);
        }

        public async Task UpdateSuiteAsync(long suiteId, SuiteRequest request)
        {
            await _adminClient.UpdateSuiteAsync(suiteId, request);
        }

        public async Task DeleteSuiteAsync(long suiteId)
        {
            await _adminClient.DeleteSuiteAsync(suiteId);
        }

        public async Task<List<AmenityDTO>> GetAmenitiesBySuiteAsync(long suiteId)
        {
            ICollection<SuiteAmenityResponse> suiteAmenities = await _adminClient.GetAmenitiesBySuiteAsync(suiteId);

            return suiteAmenities
                    .Select(a => new AmenityDTO
                    {
                        Id = a.Id,
                        Name = a.Name,
                        IconIdentifier = a.IconIdentifier
                    }).ToList();
        }

        public async Task SaveSuiteAmenitiesAsync(long suiteId, List<long> amenityIds)
        {
            await _adminClient.SaveSuiteAmenitiesAsync(suiteId, amenityIds);
        }

        public async Task<long> UploadSuiteImageAsync(long suiteId, ImageType imageType, int order, FileParameter imageFile)
        {
            return await _adminClient.UploadSuiteImageAsync(suiteId, imageType, order, imageFile);
        }

        public async Task DeleteSuiteImageAsync(long suiteImageId)
        {
            await _adminClient.DeleteSuiteImageByIdAsync(suiteImageId);
        }

        public async Task UpdateSuiteImageAsync(long suiteImageId, ImageType imageType, int order, FileParameter file)
        {
            await _adminClient.UpdateSuiteImageByIdAsync(suiteImageId, imageType, order, file);
        }

        public async Task<ICollection<SuiteImageResponse>> GetSuiteImageByTypeAsync(long suiteId, ImageType imageType)
        {
            return await _adminClient.GetSuiteImagesByImageTypeAsync(suiteId, imageType);
        }
    }
}
