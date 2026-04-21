using Microsoft.Extensions.Localization;

namespace Odasoft.XBOL.AdminPortal.Resources
{
    public class AppLocalizer<T>(
        IStringLocalizer<T> localizer,
        IStringLocalizer<SharedResource> sharedLocalizer)
    {
        public LocalizedString this[string name]
        {
            get
            {
                var localizedString = localizer[name];

                if (localizedString.ResourceNotFound)
                {
                    return sharedLocalizer[name];
                }

                return localizedString;
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var localizedString = localizer[name, arguments];

                if (localizedString.ResourceNotFound)
                {
                    return sharedLocalizer[name, arguments];
                }

                return localizedString;
            }
        }
    }
}
