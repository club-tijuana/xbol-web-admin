using Odasoft.XBOL.Models.DTO;

namespace Odasoft.XBOL.Business.Services
{
    // TODO: Separate venue's map and amenities logic in separate services.
    public class VenuesService
    {
        private IAdminClient _adminClient;

        public VenuesService(IAdminClient adminClient)
        {
            _adminClient = adminClient;
        }

        // TODO: Since the enum VenueCategory is from the admin client API I can`t create the VenueFilterParams
        // to reduce the number of parameters for this method since it will create a cycle reference.
        // To do that I will need to reorganice part of the code and right know we are tight on time.
        public async Task<VenueResponsePagedResponse> GetVenuesAsync(
                List<string> cities,
                List<VenueCategory> categories,
                string searchTerm,
                string sortBy,
                bool descending,
                int page,
                int pageSize)
        {
            return await _adminClient.GetVenuesAsync(
                cities,
                categories,
                searchTerm,
                sortBy,
                descending,
                page,
                pageSize);
        }

        public async Task<VenueResponse> GetVenueByIdAsync(long venueId)
        {
            return await _adminClient.GetVenueByIdAsync(venueId);
        }

        public async Task UpdateVenueAsync(long venueId, VenueRequest request)
        {
            await _adminClient.UpdateVenueAsync(venueId, request);
        }

        public async Task<long> CreateVenueAsync(VenueRequest request)
        {
            return await _adminClient.CreateVenueAsync(request);
        }

        public async Task DeleteVenueAsync(long venueId)
        {
            await _adminClient.DeleteVenuAsync(venueId);
        }

        public async Task<long> UploadVenueImageAsync(long venueId, ImageType imageType, int order, FileParameter imageFile)
        {
            return await _adminClient.UploadVenueImageAsync(venueId, imageType, order, imageFile);
        }

        public async Task DeleteVenueImageAsync(long venueImageId)
        {
            await _adminClient.DeleteVenueImageByIdAsync(venueImageId);
        }

        public async Task UpdateVenueImageAsync(long venueImageId, ImageType imageType, int order, FileParameter file)
        {
            await _adminClient.UpdateVenueImageByIdAsync(venueImageId, imageType, order, file);
        }

        public async Task<ICollection<VenueImageResponse>> GetVenueImageByTypeAsync(long venueId, ImageType imageType)
        {
            return await _adminClient.GetVenueImagesByImageTypeAsync(venueId, imageType);
        }

        public async Task UpdateVenueStatusAsync(long venueId, VenueStatus status)
        {
            await _adminClient.UpdateVenueStatusAsync(venueId, status);
        }

        public async Task<List<AmenityDTO>> GetAmenitiesByVenueAsync(long venueId)
        {
            ICollection<VenueAmenityResponse> venueAmenities = await _adminClient.GetAmenitiesByVenueAsync(venueId);

            return venueAmenities
                    .Select(x => new AmenityDTO
                    {
                        Id = x.Id,
                        Name = x.Name,
                        IconIdentifier = x.IconIdentifier
                    }).ToList();
        }

        public async Task SaveVenueAmenitiesAsync(long venueId, List<long> amenityIds)
        {
            await _adminClient.SaveVenueAmenitiesAsync(venueId, amenityIds);
        }
    }
}
