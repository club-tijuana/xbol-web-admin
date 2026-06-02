using Odasoft.XBOL.Models.DTO;

namespace Odasoft.XBOL.Business.Services.Contracts
{
    public interface ISuitesService
    {
        Task<ICollection<SuiteResponse>> GetSuitesAsync(long venueId);

        Task<SuiteResponse> GetSuiteByIdAsync(long id);

        Task<long> CreateSuiteAsync(SuiteRequest request);

        Task UpdateSuiteAsync(long suiteId, SuiteRequest request);

        Task DeleteSuiteAsync(long id);

        Task<List<AmenityDTO>> GetAmenitiesBySuiteAsync(long suiteId);

        Task SaveSuiteAmenitiesAsync(long suiteId, List<long> amenityIds);

        Task<long> UploadSuiteImageAsync(long suiteId, ImageType imageType, int order, FileParameter imageFile);

        Task DeleteSuiteImageAsync(long suiteImageId);

        Task UpdateSuiteImageAsync(long suiteImageId, ImageType imageType, int order, FileParameter file);

        Task<ICollection<SuiteImageResponse>> GetSuiteImageByTypeAsync(long suiteId, ImageType imageType);
    }
}
