using robotmanden.Resources;
using Microsoft.Extensions.Localization;

namespace robotmanden.Services
{
    /// <summary>
    /// Message localizer as a service will be primarily used to translate Data Annotation and Model Validation messages.
    /// </summary>
    public class GlobalTranslationService
    {
        private readonly IStringLocalizer<CommonResource> _commonLocalizer;
        public GlobalTranslationService(IStringLocalizer<CommonResource> commonLocalizer)
        {
            _commonLocalizer = commonLocalizer;
        }

        public string GetText(string Key, string LocalizationIdentifier = "common")
        {
            return LocalizationIdentifier.ToLower() switch
            {
                "common" => _commonLocalizer[Key],
                _ => _commonLocalizer[Key]
            };
        }
    }
}