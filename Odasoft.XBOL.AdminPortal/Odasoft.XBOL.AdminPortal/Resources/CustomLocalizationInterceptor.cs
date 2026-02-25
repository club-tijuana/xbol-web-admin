using Microsoft.Extensions.Localization;
using MudBlazor;
using MudBlazor.Translations;
using System.Globalization;

namespace Odasoft.XBOL.AdminPortal.Resources
{
    public class CustomLocalizationInterceptor(IStringLocalizer<SharedResource> localizer) : ILocalizationInterceptor
    {
        public LocalizedString Handle(string key, params object[] arguments)
        {
            LocalizedString? customTranslation;
            if (arguments.Length > 0)
            {
                customTranslation = localizer[key, arguments];
            }
            else
            {
                customTranslation = localizer[key];
            }

            if (!customTranslation.ResourceNotFound)
            {
                return customTranslation;
            }

            bool notFound = false;
            string? translation = LanguageResource.ResourceManager.GetString(key, CultureInfo.CurrentUICulture);

            if (
                !Equals(CultureInfo.CurrentUICulture, CultureInfo.InvariantCulture)
                && string.IsNullOrWhiteSpace(translation)
            )
            {
                translation = LanguageResource.ResourceManager.GetString(key, CultureInfo.InvariantCulture);
                notFound = true;
            }

            if (translation is not null && arguments.Length > 0)
            {
                translation = string.Format(translation, arguments);
            }

            if (translation is null)
            {
                translation = key;
                notFound = true;
            }

            return new LocalizedString(key, translation, notFound);
        }
    }
}
