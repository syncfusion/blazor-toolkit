using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Text;
using System.Dynamic;
using System.Reflection;

namespace Syncfusion.Blazor.Toolkit.Data
{
    internal class AsyncHandler
    {
    }

    /// <summary>
    /// Used to convert the enum integer values into a string
    /// Also, ignores the string conversion of number enum.
    /// </summary>
    internal class NonFlagStringEnumConverter : JsonConverter<NonFlagStringEnumConverter>
    {
        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>true if this instance can convert the specified object type; otherwise, false.</returns>
        public override bool CanConvert(Type objectType)
        {
            if (!base.CanConvert(objectType))
            {
                return false;
            }

            Type underlyingType = Nullable.GetUnderlyingType(objectType) ?? objectType;
            object[] attributes = underlyingType.GetCustomAttributes(typeof(FlagsAttribute), false);
            return attributes.Length == 0;
        }

        public override NonFlagStringEnumConverter Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, NonFlagStringEnumConverter value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Used to Parse string Date Time values into a DateTime Format
    /// </summary>
    public class DateTimeZoneHandlingConverter : JsonConverter<DateTime>
    {
        private readonly DateTime _epoch = new(1970, 1, 1, 0, 0, 0);
        private readonly Regex _regex = new("^/Date\\(([+-]*\\d+)\\)/$", RegexOptions.CultureInvariant);
        /// <summary>
        /// Reads a JSON value and converts it to a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="reader"> The <see cref="Utf8JsonReader"/> positioned at the JSON token representing the date value.</param>
        /// <param name="typeToConvert">The target type to convert to. This implementation asserts that it is <see cref="DateTime"/>.</param>
        /// <param name="options"> Serialization options supplied by the caller (not used by this implementation). </param>
        /// <returns>
        /// A <see cref="DateTime"/> parsed from the input. If the input matches <c>s_regex</c> and the captured number can be parsed as a Unix timestamp in milliseconds
        /// </returns>

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            System.Diagnostics.Debug.Assert(typeToConvert == typeof(DateTime));
            string formatted = reader.GetString()!;
            Match match = _regex.Match(formatted);

            return !match.Success || !long.TryParse(match.Groups[1].Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out long unixTime)
                ? DateTime.Parse(formatted, CultureInfo.InvariantCulture)
                : _epoch.AddMilliseconds(unixTime);
        }

        /// <summary>
        /// Writes a <see cref="DateTime"/> value as a JSON string in Unix timestamp format.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> used to write the JSON output.</param>
        /// <param name="value">The <see cref="DateTime"/> value to write.</param>
        /// <param name="options">Serialization options (not used).</param>

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            ArgumentNullException.ThrowIfNull(writer);
            long unixTime = Convert.ToInt64((value - _epoch).TotalMilliseconds);

            string formatted = FormattableString.Invariant($"/Date({unixTime})/");

            writer.WriteStringValue(formatted);
            //writer.WriteStringValue(value.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ"));
        }
    }

    /// <summary>
    /// Used to Parse string Date Time values into a DateTime Offset Format
    /// </summary>
    public class DateTimeOffsetCustomConverter : JsonConverter<DateTimeOffset>
    {

        /// <summary>
        /// Reads a JSON string and converts it to a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="reader">The <see cref="Utf8JsonReader"/> positioned at the date value.</param>
        /// <param name="typeToConvert">The type to convert, expected to be <see cref="DateTime"/>.</param>
        /// <param name="options">Serialization options (not used).</param>
        /// <returns>A <see cref="DateTime"/> parsed from the JSON value.</returns>

        public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTimeOffset.ParseExact(reader.GetString()!, "MM/dd/yyyy", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Writes a <see cref="DateTime"/> value as a JSON string in Unix timestamp format.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> used to write the JSON output.</param>
        /// <param name="value">The <see cref="DateTime"/> value to write.</param>
        /// <param name="options">Serialization options (not used).</param>

        public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
        {
            ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
        }
    }

    /// <summary>
    /// Custom type converter for ExpandoObject properties. Converts an <see cref="ExpandoObject"/> to JSON.
    /// Also, used to parse the data into their original primitive type data.
    /// </summary>
    public class ExpandoObjectConverter : JsonConverter<object>
    {

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The System.Text.Json.Utf8JsonWriter to write UTF-8 encoded JSON text.</param>
        /// <param name="value">The value that get it from dynamic ExpandoObject.</param>
        /// <param name="options">Provides options to be used with JsonSerializer.</param>

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The System.Text.Json.Utf8JsonReader to read value from UTF-8 encoded JSON.</param>
        /// <param name="typeToConvert">Type of the object.</param>
        /// <param name="options">Provides options to be used with JsonSerializer.</param>

        public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Null:
                    return null;
                case JsonTokenType.False:
                    return false;
                case JsonTokenType.True:
                    return true;
                case JsonTokenType.String:
                    {
                        return DateOnly.TryParseExact(reader.GetString(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly result)
                            ? result
                            : reader.TryGetDateTime(out DateTime date_time) ? date_time : reader.GetString()!;
                    }
                case JsonTokenType.Number:
                    {
                        if (reader.TryGetInt32(out int int_val))
                        {
                            return int_val;
                        }

                        if (reader.TryGetInt64(out long long_val))
                        {
                            return long_val;
                        }

                        if (reader.TryGetDecimal(out decimal decimal_val))
                        {
                            return decimal_val;
                        }
                        else if (reader.TryGetDouble(out double double_val))
                        {
                            return double_val;
                        }

                        throw new JsonException();
                    }
                case JsonTokenType.StartArray:
                    {
                        List<object> list = [];
                        while (reader.Read())
                        {
                            switch (reader.TokenType)
                            {
                                default:
                                    list.Add(Read(ref reader, typeof(object), options)!);
                                    break;
                                case JsonTokenType.EndArray:
                                    return list;
                                case JsonTokenType.None:
                                    break;
                                case JsonTokenType.StartObject:
                                    break;
                                case JsonTokenType.EndObject:
                                    break;
                                case JsonTokenType.StartArray:
                                    break;
                                case JsonTokenType.PropertyName:
                                    break;
                                case JsonTokenType.Comment:
                                    break;
                                case JsonTokenType.String:
                                    break;
                                case JsonTokenType.Number:
                                    break;
                                case JsonTokenType.True:
                                    break;
                                case JsonTokenType.False:
                                    break;
                                case JsonTokenType.Null:
                                    break;
                            }
                        }
                        throw new JsonException();
                    }
                default:
                    using (JsonDocument document = JsonDocument.ParseValue(ref reader))
                    {
                        return document.RootElement.Clone();
                    }
            }
        }
    }

    /// <summary>
    /// Ignorable properties converter is used to remove the unedited fields while generating patch request.
    /// </summary>
    public class IgnorablePropertiesConverter<T> : JsonConverter<T>
    {
        private readonly List<IgnorePropertiesByType> _ignorePropertiesByTypes;

        internal IgnorablePropertiesConverter(IEnumerable<IgnorePropertiesByType> ignorePropertiesByTypes)
        {
            _ignorePropertiesByTypes = [.. ignorePropertiesByTypes];
        }

        /// <summary>
        /// Reads a JSON value and converts it to an instance of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="reader">The <see cref="Utf8JsonReader"/> positioned at the JSON token.</param>
        /// <param name="typeToConvert">The type to convert, expected to be <typeparamref name="T"/>.</param>
        /// <param name="options">Serialization options (not used).</param>
        /// <returns>An instance of <typeparamref name="T"/> parsed from the JSON value.</returns>

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException("Deserialization is not implemented.");
        }

        /// <summary>
        /// Writes an instance of type <typeparamref name="T"/> as a JSON object.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> used to write the JSON output.</param>
        /// <param name="value">The <typeparamref name="T"/> value to write.</param>
        /// <param name="options">Serialization options (not used).</param>

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStartObject();
            WriteProperties(writer, value!, options);
            writer.WriteEndObject();
        }

        private void WriteProperties(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            WritePropertiesRecursive(writer, value, options, []);
        }

        private void WritePropertiesRecursive(Utf8JsonWriter writer, object value, JsonSerializerOptions options, List<string> contextPath)
        {
            Type type = value.GetType();
            PropertyInfo[] properties = type.GetProperties();
            HashSet<string> propertiesToIgnore = GetPropertiesToIgnoreByType(type);

            foreach (PropertyInfo property in properties)
            {
                string currentPath = string.Join(".", contextPath.Append(property.Name));

                if (!propertiesToIgnore.Contains(property.Name))
                {
                    object? propertyValue = property.GetValue(value);
                    if (propertyValue == null)
                    {
                        writer.WritePropertyName(property.Name);
                        JsonSerializer.Serialize(writer, propertyValue, property.PropertyType, options);
                    }
                    else
                    {
                        if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                        {
                            try
                            {
                                using MemoryStream memoryStream = new();
                                using (Utf8JsonWriter tempWriter = new(memoryStream))
                                {
                                    tempWriter.WriteStartObject();
                                    WritePropertiesRecursive(tempWriter, propertyValue, options, [.. contextPath, property.Name]);
                                    tempWriter.WriteEndObject();
                                }

                                string jsonString = Encoding.UTF8.GetString(memoryStream.ToArray());
                                if (jsonString != "{}")
                                {
                                    writer.WritePropertyName(property.Name);
                                    writer.WriteRawValue(jsonString);
                                }
                            }
                            finally
                            {
                                MemoryStream memoryStream = new();
                                Utf8JsonWriter tempWriter = new(memoryStream);
                                tempWriter.Dispose();
                                memoryStream.Dispose();
                            }
                        }
                        else
                        {
                            writer.WritePropertyName(property.Name);
                            JsonSerializer.Serialize(writer, propertyValue, property.PropertyType, options);
                        }
                    }
                }
            }
        }

        private HashSet<string> GetPropertiesToIgnoreByType(Type type)
        {
            IgnorePropertiesByType? ignorePropertiesForType = _ignorePropertiesByTypes.FirstOrDefault(x => x.Type == type);
            return ignorePropertiesForType?.Properties ?? [];
        }
    }

    /// <summary>
    /// Used to get the package name for specific component script loading.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="PackageNameAttribute"/> class.
    /// </remarks>
    /// <param name="packageName">package name.</param>
    [AttributeUsage(AttributeTargets.Field)]
    internal sealed class PackageNameAttribute(string packageName) : Attribute
    {
        /// <summary>
        /// Gets the package name.
        /// </summary>
        public string PackageName { get; } = packageName;
    }

    internal class IgnorePropertiesByType(Type type, IEnumerable<string> properties)
    {
        public Type Type { get; set; } = type;
        public HashSet<string> Properties { get; set; } = new HashSet<string>(properties, StringComparer.OrdinalIgnoreCase);
    }
}