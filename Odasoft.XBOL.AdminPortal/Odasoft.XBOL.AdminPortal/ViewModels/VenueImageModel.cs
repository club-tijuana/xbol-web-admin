using Microsoft.AspNetCore.Components.Forms;

namespace Odasoft.XBOL.AdminPortal.ViewModels
{
    public class VenueImageModel
    {
        public long? Id { get; set; }
        public string Name { get; set; } = "";
        public IBrowserFile? TempFile { get; set; }
    }
}
