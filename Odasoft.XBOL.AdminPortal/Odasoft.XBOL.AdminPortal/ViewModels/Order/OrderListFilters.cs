namespace Odasoft.XBOL.AdminPortal.ViewModels.Order
{
    public class OrderListFilters
    {
        public long SeasonId { get; set; }
        public bool RenovationMode { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? TextFilter { get; set; }
    }
}
