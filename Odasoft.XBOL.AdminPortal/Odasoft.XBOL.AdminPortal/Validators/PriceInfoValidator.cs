using FluentValidation;
using Odasoft.XBOL.AdminPortal.Resources;
using Odasoft.XBOL.AdminPortal.ViewModels.Event;
using Odasoft.XBOL.Business.ModelValidators;

namespace Odasoft.XBOL.AdminPortal.Validators
{
    public class PriceInfoValidator<T> : MudServerValidatorBase<PriceInfoModel>
    {
        public PriceInfoValidator(AppLocalizer<T> L)
        {
            RuleForEach(x => x.PriceTypes)
            .Must((model, priceKvp) =>
            {
                var key = priceKvp.Key;

                var isBasePrice = priceKvp.Value;

                if (!isBasePrice)
                {
                    return true;
                }

                model.Prices.TryGetValue(key, out var currentPrice);

                return currentPrice > 0;
            })
            .WithMessage((model, priceKvp) => $"The price for {priceKvp.Key} must be greater than 0.");
        }
    }
}
