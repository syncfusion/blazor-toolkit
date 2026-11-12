using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Syncfusion.Blazor.Toolkit.Internal
{
    /// <summary>Resolved chip data used by both standalone and projected chips.</summary>
    internal sealed class ChipModel
    {
        internal object Key { get; init; } = string.Empty;

        internal object? Value { get; init; }

        internal string Text { get; init; } = string.Empty;

        internal bool Disabled { get; init; }

        internal bool Removable { get; init; }

        internal string? IconCss { get; init; }

        internal string? ImageUrl { get; init; }

        internal string? ImageAlt { get; init; }

        internal string? CssClass { get; init; }

        internal string? Href { get; init; }

        internal bool DuplicateKey { get; init; }
    }

    /// <summary>Resolves item fields with selector, then ChipFields, then convention.</summary>
    internal static class ChipFieldResolver
    {
        internal static IReadOnlyList<ChipModel> Resolve<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] TItem>(
            IEnumerable<TItem>? source,
            Func<TItem, object>? itemKey,
            Func<TItem, string>? textSelector,
            Func<TItem, object?>? valueSelector,
            Func<TItem, bool>? disabledSelector,
            Func<TItem, bool>? removableSelector,
            Func<TItem, string?>? iconCssSelector,
            Func<TItem, string?>? imageUrlSelector,
            Func<TItem, string?>? imageAltSelector,
            Func<TItem, string?>? hrefSelector,
            Func<TItem, string?>? cssClassSelector,
            ChipFields? fields,
            bool defaultRemovable)
        {
            if (source is null)
            {
                return [];
            }

            List<ChipModel> models = [];
            HashSet<object> seen = new(ChipKeyComparer.Instance);
            int index = 0;
            foreach (TItem item in source)
            {
                object key = itemKey is not null ? itemKey(item) : IsPrimitive(item!) ? item! : ConventionKey(item, index);
                bool duplicate = !seen.Add(key);
                string text = textSelector is not null
                    ? textSelector(item)
                    : ReadString(item, fields?.Text, "Text") ?? item?.ToString() ?? string.Empty;
                object? value = valueSelector is not null
                    ? valueSelector(item)
                    : ReadObject(item, fields?.Value, "Value") ?? key;
                models.Add(new ChipModel
                {
                    Key = key,
                    Value = value,
                    Text = text,
                    Disabled = disabledSelector?.Invoke(item) ?? ReadBool(item, fields?.Disabled, "Disabled"),
                    Removable = removableSelector?.Invoke(item) ?? ReadBool(item, fields?.Removable, "Removable", defaultRemovable),
                    IconCss = iconCssSelector?.Invoke(item) ?? ReadString(item, fields?.IconCss, "IconCss"),
                    ImageUrl = imageUrlSelector?.Invoke(item) ?? ReadString(item, fields?.ImageUrl, "ImageUrl"),
                    ImageAlt = imageAltSelector is not null
                        ? imageAltSelector(item)
                        : ReadString(item, fields?.ImageAlt, "ImageAlt"),
                    CssClass = cssClassSelector?.Invoke(item) ?? ReadString(item, fields?.CssClass, "CssClass"),
                    Href = hrefSelector?.Invoke(item) ?? ReadString(item, fields?.Href, "Href"),
                    DuplicateKey = duplicate,
                });
                index++;
            }

            return models;
        }

        private static object ConventionKey<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] TItem>(TItem item, int index)
        {
            if (item is null)
            {
                return index;
            }

            if (IsPrimitive(item))
            {
                return item;
            }

            object? value = ReadObject(item, null, "Id") ?? ReadObject(item, null, "Key");
            return value ?? index;
        }

        private static bool IsPrimitive(object item)
        {
            return item is string or ValueType;
        }

        private static string? ReadString<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] TItem>(TItem item, string? fieldName, string convention)
        {
            object? value = ReadObject(item, fieldName, convention);
            return value?.ToString();
        }

        private static bool ReadBool<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] TItem>(TItem item, string? fieldName, string convention, bool fallback = false)
        {
            object? value = ReadObject(item, fieldName, convention);
            return value is bool flag ? flag : fallback;
        }

        [UnconditionalSuppressMessage("Trimming", "IL2075", Justification = "ChipFields property names are author-supplied. Public properties are preserved by DynamicallyAccessedMembers on the item type when the caller annotates it; primitive items never reflect.")]
        private static object? ReadObject<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] TItem>(
            TItem item,
            string? fieldName,
            string convention)
        {
            if (item is null || IsPrimitive(item))
            {
                return null;
            }

            string? name = string.IsNullOrEmpty(fieldName) ? convention : fieldName;
            PropertyInfo? property = typeof(TItem).GetProperty(name, BindingFlags.Instance | BindingFlags.Public);
            return property?.GetValue(item);
        }
    }

    /// <summary>Equality for chip keys that may be strings or value types.</summary>
    /// <remarks>
    /// Implements <see cref="IEqualityComparer{T}"/> over <see cref="object"/> so it can be used with
    /// both non-nullable key collections and nullable value collections.
    /// </remarks>
    internal sealed class ChipKeyComparer : IEqualityComparer<object?>
    {
        internal static ChipKeyComparer Instance { get; } = new();

        public new bool Equals(object? x, object? y)
        {
            return x is null || y is null ? x is null && y is null : x.Equals(y);
        }

        public int GetHashCode(object obj)
        {
            return obj.GetHashCode();
        }
    }
}
