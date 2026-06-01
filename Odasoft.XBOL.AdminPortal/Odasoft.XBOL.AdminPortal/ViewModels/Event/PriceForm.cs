namespace Odasoft.XBOL.AdminPortal.ViewModels.Event
{
    public class PriceForm
    {
        public long? Id { get; set; }
        public long? BaseZoneId { get; set; }
        public long? BaseSectionId { get; set; }
        public long? BaseRowId { get; set; }
        public long? BaseSeatId { get; set; }
        public Dictionary<string, bool> PriceTypes { get; set; } = [];
        public Dictionary<string, decimal> Prices { get; set; } = [];

        public bool IsZonePrice => BaseSectionId is null && BaseRowId is null && BaseSeatId is null;
    }
}
