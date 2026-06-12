using Odasoft.XBOL.AdminPortal.Resources;
using Odasoft.XBOL.AdminPortal.ViewModels.Event;
using Odasoft.XBOL.Business.ModelValidators;

namespace Odasoft.XBOL.AdminPortal.Validators
{
    public class PricesInfoValidator<T> : MudServerValidatorBase<PricesInfoModel>
    {
        public PricesInfoValidator(AppLocalizer<T> L)
        {
            RuleForEach(x => x.Prices).SetValidator(new PriceInfoValidator<T>(L));
        }
    }
}
