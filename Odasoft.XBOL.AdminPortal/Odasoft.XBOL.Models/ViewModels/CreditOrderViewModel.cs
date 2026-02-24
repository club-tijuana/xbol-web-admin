namespace Odasoft.XBOL.Models.ViewModels
{
    public class CreditOrderViewModel
    {
        public long Id { get; set; }
        public string Date { get; set; } = "";
        public string Event { get; set; } = "";
        public int Tickets { get; set; }
        public string Amount { get; set; } = "";
    }
}
