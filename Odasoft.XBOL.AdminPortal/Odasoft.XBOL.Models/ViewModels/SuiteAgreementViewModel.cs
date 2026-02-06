namespace Odasoft.XBOL.Models.ViewModels
{
    public class SuiteAgreementViewModel
    {
        public required long Id { get; set; }
        public required long SuiteId { get; set; }
        public string SuiteName { get; set; } = "";
        public string SuiteLevel { get; set; } = "";
        public string OwnerName { get; set; } = "";
        public string OwnerEmail { get; set; } = "";
        public string OwnerPhone { get; set; } = "";
        public string StartDate { get; set; } = "";
        public string EndDate { get; set; } = "";
        public string FileName { get; set; } = "";
    }
}
