namespace Odasoft.XBOL.AdminPortal.ViewModels.Event
{
    public record ImagesInfoModel()
    {
        public ImageModel? BannerImage { get; set; }
        public ImageModel?[] SponsorImages { get; set; } = new ImageModel?[2];
        public List<ImageModel> GalleryImages { get; set; } = [];
    }
}
