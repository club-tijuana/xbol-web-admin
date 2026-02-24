namespace Odasoft.XBOL.Models.ViewModels
{
    public class EnterpriseViewModel
    {
        public required long Id { get; set; }
        public string Name { get; set; } = "";
        public string LegalRepresentative { get; set; } = "";
        public string Credit { get; set; } = "";
        public string PendingCredit { get; set; } = "";
        public string AmountPaid { get; set; } = "";
        public string CreditStatus { get; set; } = "";
        public int? CreditStatusValue { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is EnterpriseViewModel other)
            {
                return Id == other.Id;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
