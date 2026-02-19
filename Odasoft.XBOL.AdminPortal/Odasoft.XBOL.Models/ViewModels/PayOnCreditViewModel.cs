namespace Odasoft.XBOL.Models.ViewModels
{
    public class PayOnCreditViewModel
    {
        public decimal AmmountPaid { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string PaymentMethod { get; set; } = "";
    }
}
