using System.Text;

namespace Syncfusion.Blazor.Toolkit.Internal
{
    /// <summary>
    /// Merges component-computed CSS classes and inline styles with the user supplied
    /// <c>HtmlAttributes</c> of the Card family, so a user <c>class</c> or <c>style</c> is appended
    /// rather than dropped.
    /// </summary>
    /// <exclude />
    internal static class CardAttributes
    {
        private const string ClassKey = "class";
        private const string StyleKey = "style";
        private const char Space = ' ';

        /// <summary>
        /// Builds the final class string: the component class, the <c>CssClass</c> parameter, then any user <c>class</c> attribute.
        /// </summary>
        internal static string MergeClass(string baseClass, string? cssClass, IReadOnlyDictionary<string, object>? attributes)
        {
            StringBuilder builder = new(baseClass);
            Append(builder, cssClass);
            Append(builder, GetText(attributes, ClassKey));
            return builder.ToString();
        }

        /// <summary>
        /// Appends the user supplied <c>style</c> attribute to the computed inline style.
        /// </summary>
        /// <returns>The merged style, or <see langword="null"/> when there is nothing to render.</returns>
        internal static string? MergeStyle(string? computedStyle, IReadOnlyDictionary<string, object>? attributes)
        {
            string? userStyle = GetText(attributes, StyleKey);
            bool hasUser = !string.IsNullOrWhiteSpace(userStyle);
            bool hasComputed = !string.IsNullOrEmpty(computedStyle);

            string? merged = hasComputed ? computedStyle : null;
            if (hasUser)
            {
                merged = hasComputed ? computedStyle!.TrimEnd(';') + ";" + userStyle : userStyle;
            }

            return merged;
        }

        /// <summary>
        /// Returns the attributes with the reserved <c>class</c> and <c>style</c> keys removed, because those two are rendered through the merged values.
        /// </summary>
        internal static Dictionary<string, object> Filter(IReadOnlyDictionary<string, object>? attributes)
        {
            Dictionary<string, object> filtered = [];
            if (attributes is null)
            {
                return filtered;
            }

            foreach (KeyValuePair<string, object> pair in attributes)
            {
                if (string.Equals(pair.Key, ClassKey, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(pair.Key, StyleKey, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                filtered[pair.Key] = pair.Value;
            }

            return filtered;
        }

        private static string? GetText(IReadOnlyDictionary<string, object>? attributes, string key)
        {
            return attributes is not null && attributes.TryGetValue(key, out object? value) ? value?.ToString() : null;
        }

        private static void Append(StringBuilder builder, string? value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                _ = builder.Append(Space);
                _ = builder.Append(value.Trim());
            }
        }
    }
}
