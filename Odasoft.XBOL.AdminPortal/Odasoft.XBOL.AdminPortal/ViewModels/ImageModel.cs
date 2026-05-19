using Microsoft.AspNetCore.Components.Forms;
using Odasoft.XBOL.Business;

namespace Odasoft.XBOL.AdminPortal.ViewModels
{
    public class ImageModel
    {
        public long? Id { get; set; }
        public string Name { get; set; } = "";
        public IBrowserFile? TempFile { get; set; }
        public byte[]? FileBytes { get; set; }
        public string ContentType { get; set; } = "";
        public string Src { get; set; } = string.Empty;
        public int Order { get; set; } = 0;
        public ImageType ImageType { get; set; }
        public MediaType MediaType { get; set; }
    }
}
