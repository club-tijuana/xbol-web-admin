using Odasoft.XBOL.Business;

namespace Odasoft.XBOL.AdminPortal.ViewModels
{
    public class CreditTransactionModel
    {
        public decimal AmountPaid { get; set; }
        public DateTime? PaymentDate { get; set; }
        public PaymentType? PaymentMethod { get; set; }
    }
}
