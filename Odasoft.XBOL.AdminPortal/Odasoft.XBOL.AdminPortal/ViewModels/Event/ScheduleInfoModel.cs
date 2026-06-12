using Odasoft.XBOL.Business;

namespace Odasoft.XBOL.AdminPortal.ViewModels.Event
{
    public record ScheduleInfoModel()
    {
        public DateTime? StartDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public DateTime? EndDate { get; set; }
        public TimeSpan? EndTime { get; set; }
        public DateTime? PublishedDate { get; set; }
        public TimeSpan? PublishedTime { get; set; }
        public DateTime? PreSaleDate { get; set; }
        public TimeSpan? PreSaleTime { get; set; }
        public DateTime? PreSaleEndDate { get; set; }
        public TimeSpan? PreSaleEndTime { get; set; }
        public DateTime? OnSaleDate { get; set; }
        public TimeSpan? OnSaleTime { get; set; }
        public DateTime? OffSaleDate { get; set; }
        public TimeSpan? OffSaleTime { get; set; }
        public DateTime? GateOpenDate { get; set; }
        public TimeSpan? GateOpenTime { get; set; }
        public AgeRestriction? AgeRestriction { get; set; }
        public string? ExternalEventKey { get; set; }
        public string? SecurityPolicies { get; set; }
        public string? AdditionalComments { get; set; }
        public int? HoldExpirationInMinutes { get; set; }
    }
}
