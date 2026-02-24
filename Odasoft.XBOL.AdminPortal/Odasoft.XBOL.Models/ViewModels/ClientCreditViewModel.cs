namespace Odasoft.XBOL.Models.ViewModels
{
    public class ClientCreditViewModel
    {
        public long Id { get; set; }
        private long Clientid { get; set; }
        public string Credit { get; set; } = "";
        public string StartDate { get; set; } = "";
        public string PaymentTerm { get; set; } = "";
        public string AppliesTaxes { get; set; } = "";
        public string AmountPaid { get; set; } = "";
        public string PendingAmount { get; set; } = "";
        public string Status { get; set; } = "";
        public string Term { get; set; } = "";
        public string NumberOfInstallments { get; set; } = "";
    }
}
