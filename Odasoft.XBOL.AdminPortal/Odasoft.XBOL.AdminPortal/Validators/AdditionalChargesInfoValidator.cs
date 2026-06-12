using Odasoft.XBOL.AdminPortal.Resources;
using Odasoft.XBOL.AdminPortal.ViewModels.Event;
using Odasoft.XBOL.Business.ModelValidators;

namespace Odasoft.XBOL.AdminPortal.Validators
{
    public class AdditionalChargesInfoValidator<T> : MudServerValidatorBase<AdditionalChargesInfoModel>
    {
        public AdditionalChargesInfoValidator(AppLocalizer<T> L)
        {
            RuleForEach(x => x.AdditionalCharges).SetValidator(new AdditionalChargeInfoValidator<T>(L));
        }
    }
}
