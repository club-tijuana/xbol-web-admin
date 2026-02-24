namespace Odasoft.XBOL.Models.ViewModels
{
    public class LegalRepresentativeViewModel
    {
        public long Id { get; set; }
        public long ClientId { get; set; }
        public string Name { get; set; } = "";
        public string DOB { get; set; } = "";
        public string TaxId { get; set; } = "";

        // TODO: Review if its better to name the field CURP
        public string SSN { get; set; } = "";
    }
}
