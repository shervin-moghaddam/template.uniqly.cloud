using robotmanden.Resources;
using Microsoft.Extensions.Localization;
using System.Reflection;

namespace robotmanden.Services
{
    public class GlobalLocalizationService
    {
        private readonly IStringLocalizer localizer;
        public GlobalLocalizationService(IStringLocalizerFactory factory)
        {
            var assemblyName = new AssemblyName(typeof(CommonResource).GetTypeInfo().Assembly.FullName);
            localizer = factory.Create(nameof(CommonResource), assemblyName.Name);
        }

        public string Get(string key)
        {
            return localizer[key];
        }
    }
}
