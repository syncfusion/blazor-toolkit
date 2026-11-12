using Microsoft.Extensions.Localization;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Internal
{
    /// <summary>Looks up Chip resource strings without embedding English in component code.</summary>
    internal static class ChipResources
    {
        internal const string RemoveKey = "Chip_Remove";
        internal const string ShowMoreKey = "Chip_ShowMore";
        internal const string ShowLessKey = "Chip_ShowLess";

        internal static string Format(IStringLocalizer? localizer, string key, string fallback, params object?[] args)
        {
            string template = fallback;
            if (localizer is not null)
            {
                LocalizedString localized = localizer[key];
                if (!localized.ResourceNotFound && !string.IsNullOrEmpty(localized.Value))
                {
                    template = localized.Value;
                }
            }

            return args.Length == 0 ? template : string.Format(CultureInfo.CurrentCulture, template, args);
        }
    }
}
