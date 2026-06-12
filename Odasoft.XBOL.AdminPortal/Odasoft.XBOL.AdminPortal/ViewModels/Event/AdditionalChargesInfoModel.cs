namespace Odasoft.XBOL.AdminPortal.ViewModels.Event
{
    public record AdditionalChargesInfoModel()
    {
        public List<AdditionalChargeInfoModel> AdditionalCharges { get; set; } = [];
    }
}
