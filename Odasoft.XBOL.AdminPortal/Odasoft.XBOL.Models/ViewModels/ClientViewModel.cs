namespace Odasoft.XBOL.Models.ViewModels
{
    public class ClientViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; } = "";

        public string CompanyName { get; set; } = "";

        public string CompanyTaxId { get; set; } = "";

        public string Address { get; set; } = "";

        public string ContactEmail { get; set; } = "";
        public string Phone { get; set; } = "";
    }
}
