using FluentValidation;
using Odasoft.XBOL.AdminPortal.Resources;
using Odasoft.XBOL.AdminPortal.ViewModels.Event;
using Odasoft.XBOL.Business;
using Odasoft.XBOL.Business.ModelValidators;

namespace Odasoft.XBOL.AdminPortal.Validators
{
    public class AdditionalChargeInfoValidator<T> : MudServerValidatorBase<AdditionalChargeInfoModel>
    {
        public AdditionalChargeInfoValidator(AppLocalizer<T> L)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithName(L["FeeName"]);
            RuleFor(x => x.FeeType)
                .IsInEnum()
                .Must(type => type is FeeType.Fixed or FeeType.Percentage)
                .WithName(L["Type"]);
            RuleFor(x => x.Value)
                .NotEmpty()
                .GreaterThan(0)
                .WithName(L["FeeAmount"]);
        }
    }
}
