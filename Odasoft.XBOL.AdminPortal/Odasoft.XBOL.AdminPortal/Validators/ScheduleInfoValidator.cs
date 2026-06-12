using FluentValidation;
using Odasoft.XBOL.AdminPortal.Resources;
using Odasoft.XBOL.AdminPortal.ViewModels.Event;
using Odasoft.XBOL.Business.ModelValidators;

namespace Odasoft.XBOL.AdminPortal.Validators
{
    public class ScheduleInfoValidator<T> : MudServerValidatorBase<ScheduleInfoModel>
    {
        public ScheduleInfoValidator(AppLocalizer<T> L)
        {
            RuleFor(x => x.StartDate)
                .NotEmpty()
                .WithName(L["StartDate"]);
            RuleFor(x => x.StartTime)
                .NotEmpty()
                .WithName(L["Time"]);
            RuleFor(x => x.EndDate)
                .NotEmpty()
                .WithName(L["EndDate"]);
            RuleFor(x => x.EndTime)
                .NotEmpty()
                .WithName(L["Time"]);
            RuleFor(x => x.OnSaleDate)
                .NotEmpty()
                .WithName(L["StartDate"]);
            RuleFor(x => x.OnSaleTime)
                .NotEmpty()
                .WithName(L["Time"]);
            RuleFor(x => x.OffSaleDate)
                .NotEmpty()
                .WithName(L["EndDate"]);
            RuleFor(x => x.OffSaleTime)
                .NotEmpty()
                .WithName(L["Time"]);
        }
    }
}
