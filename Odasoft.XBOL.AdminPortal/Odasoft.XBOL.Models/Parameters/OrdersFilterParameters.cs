namespace Odasoft.XBOL.Models.Parameters
{
    public record OrdersFilterParameters : BaseFilterParameters
    {
        public long? ClientId { get; set; }
        public List<string> Events { get; set; } = [];
        public DateTimeOffset? StartDate;
        public DateTimeOffset? EndDate;
    }
}
