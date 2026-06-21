using Odasoft.XBOL.Business;

namespace Odasoft.XBOL.AdminPortal.ViewModels.Event
{
    public record AdditionalChargeInfoModel()
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public FeeType FeeType { get; set; }
        public ChargeCategory ChargeCategory { get; set; } = ChargeCategory.Fee;
        public decimal? Value { get; set; }
        public Guid UiId { get; set; } = new Guid();

        public virtual bool Equals(AdditionalChargeInfoModel? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return UiId == other.UiId;
        }

        public override int GetHashCode()
        {
            return UiId.GetHashCode();
        }
    }
}
