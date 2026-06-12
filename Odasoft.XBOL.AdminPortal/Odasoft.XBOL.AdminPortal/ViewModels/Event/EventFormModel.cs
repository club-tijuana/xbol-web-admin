using static Odasoft.XBOL.AdminPortal.Components.Pages.EventEdit;

namespace Odasoft.XBOL.AdminPortal.ViewModels.Event
{
    public class EventFormModel()
    {
        public EventInfoModel EventInfo { get; set; } = new();
        public ScheduleInfoModel ScheduleInfo { get; set; } = new();
        public PricesInfoModel PricesInfo { get; set; } = new();
        public AdditionalChargesInfoModel AdditionalChargesInfo { get; set; } = new();
        public ImagesInfoModel ImagesInfo { get; set; } = new();
    }
}
