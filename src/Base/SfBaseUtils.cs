using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;
using System.Text.RegularExpressions;
namespace Syncfusion.Blazor.Toolkit.Internal
{
    /// <summary>
    /// Common utility methods shared across all Syncfusion Blazor Toolkit components,
    /// covering dictionary updates, equality comparison, CSS formatting, event invocation,
    /// type conversion, class manipulation, unique ID generation, and two-way binding helpers.
    /// </summary>
    internal class SfBaseUtils
    {
        /// <summary>
        /// Hash set containing names of multidimensional array types used to identify multidimensional arrays during serialization comparison.
        /// </summary>
        /// <exclude />
        private static readonly HashSet<string> _multidimensionalArrayTypes =
        [
            typeof(int[,]).Name, typeof(double[,]).Name, typeof(int?[,]).Name, typeof(decimal[,]).Name
        ];

        /// <summary>
        /// JSON serializer options configured to ignore cycles and read-only properties.
        /// </summary>
        /// <exclude />
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            IgnoreReadOnlyProperties = true
        };

        /// <summary>
        /// Adds or updates a value in the specified dictionary based on the provided key.
        /// If the dictionary is null, a new instance will be created.
        /// </summary>
        /// <param name="key">The key whose value should be added or updated.</param>
        /// <param name="data">The value to assign to the specified key.</param>
        /// <param name="dictionary">The dictionary to modify. If null, a new dictionary is created.</param>
        /// <returns>The updated dictionary containing the new or modified key-value pair.</returns>
        internal static Dictionary<string, object> UpdateDictionary(string key, object data, Dictionary<string, object>? dictionary)
        {
            dictionary ??= [];
            dictionary[key] = data;
            return dictionary;
        }

        /// <summary>
        /// Compares two values for equality, using JSON serialization for collection and array comparison.
        /// </summary>
        /// <typeparam name="T">The type of values being compared.</typeparam>
        /// <param name="oldValue">The original value.</param>
        /// <param name="newValue">The new value.</param>
        /// <returns>
        /// <see langword="true"/> if the values are equal; otherwise <see langword="false"/>.
        /// </returns>
        /// <remarks>
        /// Array and collection types are serialized to JSON and compared as strings. Primitive types
        /// use <see cref="EqualityComparer{T}.Default"/>.
        /// </remarks>
        internal static bool Equals<T>(T oldValue, T newValue)
        {
            Type? valueType = oldValue?.GetType();
            bool isValueCollection = valueType != null && valueType.IsArray;
            // Compare collection values using Json Serializer
            if (isValueCollection)
            {
                if (!_multidimensionalArrayTypes.Contains(valueType?.Name ?? string.Empty))
                {
                    string oldString = JsonSerializer.Serialize(oldValue, _jsonOptions);
                    string newString = JsonSerializer.Serialize(newValue, _jsonOptions);
                    return string.Equals(oldString, newString, StringComparison.Ordinal);
                }
            }
            return EqualityComparer<T>.Default.Equals(oldValue, newValue);
        }

        /// <summary>
        /// Normalizes a CSS dimension string by appending <c>px</c> when no valid unit is present.
        /// </summary>
        /// <param name="propertyValue">A raw dimension string such as <c>"100"</c>, <c>"100px"</c>, or <c>"50%"</c>.</param>
        /// <returns>The input value if it already contains a recognized CSS unit; otherwise the value with <c>px</c> appended.</returns>
        /// <remarks>
        /// Recognized units include: <c>auto</c>, <c>%</c>, <c>px</c>, <c>vh</c>, <c>em</c>, <c>vw</c>, <c>rem</c>, <c>in</c>, <c>cm</c>, <c>fr</c>, <c>mm</c>, <c>pt</c>, <c>pc</c>, <c>ch</c>, <c>ex</c>, <c>ms</c>, <c>deg</c>, <c>s</c>, <c>rad</c>, <c>grad</c>, <c>vmin</c>, and <c>vmax</c>.
        /// </remarks>
        internal static string FormatUnit(string propertyValue)
        {
            if (string.IsNullOrWhiteSpace(propertyValue))
            {
                return propertyValue;
            }

            string regexPattern = "auto|%|px|vh|em|vw|rem|in|cm|fr|mm|pt|pc|ch|ex|ms|deg|s|rad|grad|vmin|vmax";

            return Regex.IsMatch(propertyValue, regexPattern) ? propertyValue : propertyValue + "px";
        }

        /// <summary>
        /// Invokes the provided <see cref="EventCallback{T}"/> delegate if it has a subscriber.
        /// </summary>
        /// <typeparam name="T">The type of event arguments passed to the callback.</typeparam>
        /// <param name="eventFn">The event callback as an <see cref="object"/>, expected to be an <see cref="EventCallback{T}"/>.</param>
        /// <param name="eventArgs">The arguments to pass to the event handler.</param>
        /// <returns>
        /// A task that completes when the event handler has finished executing, or immediately if
        /// no handler is assigned.
        /// </returns>
        /// <remarks>
        /// This helper is used internally when the exact generic type of an <see cref="EventCallback"/>
        /// is not known at compile time (for example, when invoking callbacks stored in a dictionary).
        /// </remarks>
        internal static async Task InvokeEventAsync<T>(object eventFn, T eventArgs)
        {
            if (eventFn != null)
            {
                EventCallback<T> eventHandler = (EventCallback<T>)eventFn;
                if (eventHandler.HasDelegate)
                {
                    await eventHandler.InvokeAsync(eventArgs).ConfigureAwait(true);
                }
            }
        }

        /// <summary>
        /// Converts the specified value to the given target type.
        /// </summary>
        /// <param name="dataValue">The value to convert.</param>
        /// <param name="conversionType">The target <see cref="Type"/> to convert the value to.</param>
        /// <param name="isParseValue">
        /// If set to <c>true</c>, indicates that the conversion is part of a parse operation and
        /// any parse-specific handling should be applied; otherwise, normal conversion behavior is used.
        /// </param>
        /// <returns>The converted value as an <see cref="object"/>, or <c>null</c> if conversion is not possible.</returns>
        internal static object? ChangeType(object? dataValue, Type conversionType, bool isParseValue = false)
        {
            // Returns null value
            if (dataValue is null || conversionType is null)
            {
                return null;
            }

            Type valueType = dataValue.GetType();
            bool isValueCollection = valueType != null && valueType.Namespace != null && (valueType.Namespace.Contains("Collections", StringComparison.Ordinal) || valueType.IsArray ||
                (valueType.BaseType != null && valueType.BaseType.Namespace != null && (valueType.BaseType.Namespace.Contains("Collections", StringComparison.Ordinal) || valueType.BaseType.IsArray)));

            // Returns the nullable type values
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                conversionType = Nullable.GetUnderlyingType(conversionType)!;
            }

            // Returns the basic Convert.ChangeType value when both source and destination has the same Type name.
            if (dataValue.GetType().Name == conversionType.Name)
            {
                return Convert.ChangeType(dataValue, conversionType, CultureInfo.CurrentCulture);
            }

            // Converts the value to string for array and collection values
            if ((conversionType.IsPrimitive && !conversionType.IsArray && !(conversionType.Namespace?.Contains("Collections", StringComparison.Ordinal) == true)) || conversionType == typeof(decimal) || conversionType == typeof(string) || (conversionType == typeof(object) && !isValueCollection) || conversionType == typeof(DateTime))
            {
                dataValue = Convert.ToString(dataValue, CultureInfo.InvariantCulture)!;
            }
            else if (conversionType == typeof(Guid))
            {
                dataValue = new Guid(dataValue.ToString()!);
            }
            else if (conversionType.Name == "DateTimeOffset")
            {
                dataValue = DateTimeOffset.Parse(dataValue.ToString()!, CultureInfo.InvariantCulture);
            }
            else if (conversionType.Name == "TimeOnly")
            {
                dataValue = TimeOnly.Parse(dataValue.ToString()!, CultureInfo.InvariantCulture);
            }
            else if (conversionType.Name == "DateOnly")
            {
                dataValue = DateOnly.Parse(dataValue.ToString()!, CultureInfo.InvariantCulture);
            }
            else if (conversionType.Name == "TimeSpan")
            {
                string tempValue = JsonSerializer.Serialize(dataValue);
                dataValue = JsonSerializer.Deserialize(tempValue, conversionType)!;
            }

            CultureInfo currentCulture = isParseValue ? CultureInfo.InvariantCulture : CultureInfo.CurrentCulture;
            return Convert.ChangeType(dataValue, conversionType, currentCulture);
        }

        /// <summary>
        /// Adds a CSS class to a space-separated class string if not already present.
        /// </summary>
        /// <param name="prevClass">Existing class string (can be null or empty).</param>
        /// <param name="className">Class name to add.</param>
        /// <returns>Updated class string with duplicates removed and spaces normalized.</returns>
        internal static string AddClass(string? prevClass, string? className)
        {
            if (string.IsNullOrWhiteSpace(className))
            {
                return prevClass?.Trim() ?? string.Empty;
            }
            if (string.IsNullOrWhiteSpace(prevClass))
            {
                return className.Trim();
            }
            HashSet<string> classes = new(
                prevClass.Split(' ', StringSplitOptions.RemoveEmptyEntries),
                StringComparer.Ordinal);
            _ = classes.Add(className.Trim());
            return string.Join(" ", classes);
        }

        /// <summary>
        /// Removes a CSS class from a space-separated class string.
        /// </summary>
        /// <param name="prevClass">Existing class string (can be null or empty).</param>
        /// <param name="className">Class name to remove.</param>
        /// <returns>Updated class string with the specified class removed and spaces normalized.</returns>
        internal static string RemoveClass(string? prevClass, string? className)
        {
            if (string.IsNullOrWhiteSpace(prevClass))
            {
                return string.Empty;
            }
            if (string.IsNullOrWhiteSpace(className))
            {
                return prevClass.Trim();
            }
            HashSet<string> classes = new(
                prevClass.Split(' ', StringSplitOptions.RemoveEmptyEntries),
                StringComparer.Ordinal);
            foreach (string cls in className.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                _ = classes.Remove(cls.Trim());
            }
            return string.Join(" ", classes);
        }

        /// <summary>
        /// Generates a unique identifier using a GUID, optionally prefixed with a custom name.
        /// </summary>
        /// <param name="name">Optional prefix to prepend to the GUID. If provided, a hyphen separator is added.</param>
        /// <returns>
        /// A unique string identifier in the format "name-guid" if <paramref name="name"/> is provided,
        /// or just "guid" if not. The GUID is in standard hyphenated format.
        /// </returns>
        internal static string GenerateID(string? name = null)
        {
            return string.IsNullOrWhiteSpace(name)
                ? Guid.NewGuid().ToString()
                : $"{name}-{Guid.NewGuid()}";
        }

        /// <summary>
        /// Invokes the EventCallback to notify the parent component when a property value changes in two-way binding.
        /// </summary>
        /// <typeparam name="T">The type of the property value being updated.</typeparam>
        /// <param name="publicValue">The new property value from the component.</param>
        /// <param name="privateValue">The current property value used to detect changes.</param>
        /// <param name="eventCallback">The EventCallback delegate that notifies the parent component of value changes.</param>
        /// <param name="editContext">Optional. The EditContext instance for form validation notification.</param>
        /// <param name="expression">Optional. The property expression for EditContext field identification.</param>
        /// <returns>Returns the public value to be assigned to the component's internal field.</returns>
        /// <remarks>
        /// The callback is only invoked if the value has changed. When editContext and expression are provided, form validation is triggered.
        /// </remarks>
        internal static async Task<T> UpdatePropertyAsync<T>(T publicValue, T privateValue, EventCallback<T> eventCallback, EditContext? editContext = null, Expression<Func<T>>? expression = null)
        {
            T finalValue = publicValue;
            // Checking eventcallback for two-way notification
            if (eventCallback.HasDelegate && !Equals(publicValue, privateValue))
            {
                await eventCallback.InvokeAsync(finalValue).ConfigureAwait(true);
                if (editContext != null)
                {
                    ValidateExpression(editContext, expression);
                }
            }
            return finalValue;
        }

        /// <summary>
        /// Notifies the EditContext that a field value has changed for the specified expression,
        /// triggering validation and UI updates.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the field value being tracked.
        /// </typeparam>
        /// <param name="editContext">
        /// The EditContext associated with the form. If null, no action is taken.
        /// </param>
        /// <param name="expression">
        /// A lambda expression identifying the field that changed (e.g., () => model.PropertyName). 
        /// If null, no action is taken.
        /// </param>
        /// <remarks>
        /// This utility method is used in custom Blazor form components to manually notify 
        /// the EditContext when a bound field value changes, which triggers field validation 
        /// and updates validation messages in the UI.
        /// </remarks>
        internal static void ValidateExpression<T>(EditContext editContext, Expression<Func<T>>? expression)
        {
            // Notify form fields about the value changes
            if (expression != null)
            {
                editContext?.NotifyFieldChanged(FieldIdentifier.Create(expression));
            }
        }

        /// <summary>
        /// Retrieves the <see cref="EnumMemberAttribute.Value"/> associated with an enum member, if one is defined.
        /// </summary>
        /// <typeparam name="T">The enumeration type, constrained to value types that implement <see cref="IConvertible"/>.</typeparam>
        /// <param name="enumValue">The enumeration member whose attribute value is to be retrieved.</param>
        /// <returns>
        /// The string value defined in <see cref="EnumMemberAttribute.Value"/> for the specified enum member,
        /// or <c>null</c> if no such attribute is defined.
        /// </returns>
        internal static string? GetEnumValue<T>(T enumValue)
            where T : struct, IConvertible
        {
            return typeof(T).GetTypeInfo().DeclaredMembers.SingleOrDefault(x => x.Name == enumValue.ToString())?.GetCustomAttribute<EnumMemberAttribute>(false)?.Value;
        }
    }
}
