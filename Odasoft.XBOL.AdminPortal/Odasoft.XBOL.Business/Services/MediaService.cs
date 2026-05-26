namespace Odasoft.XBOL.Business.Services
{
    public class MediaService(IAdminClient adminClient)
    {
        public async Task<MediaResponse> UploadMediaAsync(long referenceId, AdminSaleType referenceType, MediaType mediaType, int order, FileParameter file)
        {
            return await adminClient.UploadMediaAsync(referenceId, referenceType, mediaType, order, file);
        }

        public async Task DeleteMediaAsync(long mediaId)
        {
            await adminClient.DeleteMediaByIdAsync(mediaId);
        }

        public async Task UpdateMediaAsync(long mediaId, FileParameter file, MediaType? mediaType = null, int? order = null)
        {
            await adminClient.UpdateMediaByIdAsync(mediaId, mediaType, order, file);
        }

        public async Task<ICollection<MediaResponse>> GetReferenceMediaByMediaTypeAsync(long referenceId, AdminSaleType referenceType, MediaType mediaType)
        {
            return await adminClient.GetReferenceMediaByMediaTypeAsync(referenceId, referenceType, mediaType);
        }

        public async Task<ICollection<MediaResponse>> GetReferenceMediaAsync(long referenceId, AdminSaleType referenceType)
        {
            return await adminClient.GetProductMediaAsync(referenceId, referenceType);
        }

        public async Task<EventMediaSetResponse> GetEventMediaAsync(long eventId)
        {
            return await adminClient.GetEventMediaAsync(eventId);
        }

        public async Task<EventMediaSetResponse> ReconcileEventMediaAsync(
            long eventId,
            IEnumerable<EventMediaDesiredItem> items,
            IEnumerable<FileParameter>? files = null)
        {
            var serializedItems = Newtonsoft.Json.JsonConvert.SerializeObject(items);
            return await adminClient.ReconcileEventMediaAsync(eventId, serializedItems, files ?? []);
        }
    }
}
