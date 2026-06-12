using FluentValidation;
using Odasoft.XBOL.AdminPortal.Resources;
using Odasoft.XBOL.AdminPortal.ViewModels.Event;
using Odasoft.XBOL.Business.ModelValidators;

namespace Odasoft.XBOL.AdminPortal.Validators
{
    public class EventInfoValidator<T> : MudServerValidatorBase<EventInfoModel>
    {
        public EventInfoValidator(AppLocalizer<T> L)
        {
            ApiToClientMap["VenueMapId"] = "SelectedVenueMap";
            ApiToClientMap["CategoryIds"] = "SelectedCategories";

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithName(L["Name"]);
            RuleFor(x => x.SelectedVenue)
                .NotEmpty()
                .WithName(L["Venue"]);
            RuleFor(x => x.SelectedVenueMap)
                .NotEmpty()
                .WithName(L["Map"]);
            RuleFor(x => x.SelectedCategories)
                .NotEmpty()
                .WithName(L["EventType"]);
        }
    }
}
