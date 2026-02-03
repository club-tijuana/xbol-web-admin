using Odasoft.XBOL.AdminPortal.Helpers;

namespace Odasoft.XBOL.AdminPortal.ViewModels.Shared
{
    public class ImageHeroBannerViewModel
    {
        public string? ImageUrl { get; set; }
        public string? Title { get; set; }
        public string? Subtitle { get; set; }
        public string? Venue { get; set; } 
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public string? DateRangeText => EventDisplayDateHelper.GetDateRange(StartDateTime, EndDateTime);
        public string? ShortDate => EventDisplayDateHelper.GetShortDate(StartDateTime);
        public string? LongDate => EventDisplayDateHelper.GetLongDate(StartDateTime);
        public string? TimeRange => EventDisplayDateHelper.GetDateTimeRange(StartDateTime, EndDateTime);
    }
}
