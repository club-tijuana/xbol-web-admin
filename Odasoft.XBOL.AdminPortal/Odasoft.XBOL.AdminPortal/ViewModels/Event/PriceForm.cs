namespace Odasoft.XBOL.AdminPortal.ViewModels.Event
{
    public class PriceForm
    {
        public long? Id { get; set; }
        public long? BaseZoneId { get; set; }
        public long? BaseSectionId { get; set; }
        public long? BaseRowId { get; set; }
        public long? BaseSeatId { get; set; }
        public decimal? ItemPrice { get; set; }
        public Dictionary<string, decimal?> PriceTypes { get; set; } = new();

        public bool IsZonePrice => BaseSectionId is null && BaseRowId is null && BaseSeatId is null;
    }
}
