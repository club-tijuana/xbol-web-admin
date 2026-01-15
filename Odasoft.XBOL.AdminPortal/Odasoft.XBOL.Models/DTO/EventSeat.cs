namespace Odasoft.XBOL.Models.DTO
{
    public class SeatPriceDTO
    {
        public long SeatId { get; set; }
        public long BaseSeatId { get; set; }
        public decimal PriceOverride { get; set; }
        public string ExternalSeatObjectKey { get; set; } = "";
    }
}
