using Odasoft.XBOL.AdminPortal.Resources;
using Odasoft.XBOL.AdminPortal.ViewModels.Event;
using Odasoft.XBOL.Business.ModelValidators;

namespace Odasoft.XBOL.AdminPortal.Validators
{
    public class ImagesInfoValidator<T> : MudServerValidatorBase<ImagesInfoModel>
    {
        public ImagesInfoValidator(AppLocalizer<T> L)
        {

        }
    }
}
