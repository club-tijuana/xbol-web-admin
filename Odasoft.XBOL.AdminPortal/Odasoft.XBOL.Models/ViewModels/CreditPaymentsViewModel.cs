namespace Odasoft.XBOL.Models.ViewModels
{
    public class CreditPaymentsViewModel
    {
        public long Id { get; set; }
        public string Date { get; set; } = "";
        public string PaymentId { get; set; } = "";
        public string Amount { get; set; } = "";
        public string PaymentMethod { get; set; } = "";
        public string ReceivedBy { get; set; } = "";
    }
}
