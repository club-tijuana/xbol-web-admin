using Odasoft.XBOL.AdminPortal.Resources;
using Odasoft.XBOL.AdminPortal.ViewModels.Event;
using Odasoft.XBOL.Business.ModelValidators;

namespace Odasoft.XBOL.AdminPortal.Validators
{
    public class EventFormValidator<T> : MudServerValidatorBase<EventFormModel>
    {
        public EventFormValidator(AppLocalizer<T> L)
        {
            RuleFor(x => x.EventInfo).SetValidator(new EventInfoValidator<T>(L));
            RuleFor(x => x.PricesInfo).SetValidator(new PricesInfoValidator<T>(L));
            RuleFor(x => x.ScheduleInfo).SetValidator(new ScheduleInfoValidator<T>(L));
            RuleFor(x => x.ImagesInfo).SetValidator(new ImagesInfoValidator<T>(L));
        }
    }
}
