namespace Odasoft.XBOL.AdminPortal.ViewModels.Event
{
    public record PricesInfoModel()
    {
        public List<PriceInfoModel> Prices { get; set; } = [];
    }
}
