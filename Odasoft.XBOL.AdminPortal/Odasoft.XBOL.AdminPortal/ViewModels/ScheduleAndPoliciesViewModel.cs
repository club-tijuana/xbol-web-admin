using Odasoft.XBOL.Business;

namespace Odasoft.XBOL.AdminPortal.ViewModels
{
    public class ScheduleAndPoliciesViewModel
    {
        public DateTimeOffset? StartDateTime { get; set; }
        public DateTimeOffset? EndDateTime { get; set; }
        public DateTimeOffset? PublishedDate { get; set; }
        public DateTimeOffset? PreSaleStartDate { get; set; }
        public DateTimeOffset? PreSaleEndDate { get; set; }
        public DateTimeOffset? OnSaleDate { get; set; }
        public DateTimeOffset? OffSaleDate { get; set; }
        public DateTimeOffset? GateOpenDate { get; set; }

        public AgeRestriction? AgeRestriction { get; set; }
        public string? SecurityPolicies { get; set; }
        public string? AdditionalComments { get; set; }
    }
}
