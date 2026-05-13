using System.ComponentModel;
using System.Reflection;
using System.Globalization;
using System.Collections;
using Hashtable = System.Collections.Generic.Dictionary<object, object>;

namespace Syncfusion.Blazor.Toolkit.Data
{
    /// <summary>
    /// A framework independent utility class for the new Nullable type in .NET Framework 2.0.
    /// </summary>
    /// <exclude/>
    public static class NullableHelperInternal
    {
        /// <summary>
        /// Indicates whether the specified PropertyDescriptor has nested properties.
        /// </summary>
        /// <param name="pd">The PropertyDescriptor to be checked.</param>
        /// <returns>True if nested properties are found; False otherwise.</returns>
        internal static bool IsComplexType(PropertyInfo pd)
        {
            Type t = pd.PropertyType;
            return IsComplexType(t);
        }

        /// <summary>
        /// Indicates whether the specified Type has nested properties.
        /// </summary>
        /// <param name="t">The Type to be checked.</param>
        /// <returns>True if nested properties are found; False otherwise.</returns>
        public static bool IsComplexType(Type t)
        {
            Type underlyingType = GetUnderlyingType(t);
            if (underlyingType != null)
            {
                t = underlyingType;
            }
            return t != typeof(object)
                && t != typeof(decimal)
                && t != typeof(DateTime)
                && t != typeof(Type)
                && t != typeof(string)
                && t != typeof(Guid)
                && t.GetTypeInfo().BaseType != typeof(Enum)
                && !t.GetTypeInfo().IsPrimitive;
        }

        /// <summary>
        /// Determines whether the given property represents an enumerable (collection)
        /// type that should be treated as complex for binding/serialization purposes.
        /// </summary>
        /// <param name="pd">The <see cref="PropertyInfo"/> describing the property to inspect.</param>
        /// <returns>
        /// <c>true</c> if the property's type is a non-primitive enumerable complex type; otherwise <c>false</c>.
        /// </returns>
        public static bool IsIEnumerableType(PropertyInfo pd)
        {
            return pd != null && IsComplexType(pd.PropertyType)
            && !typeof(byte[]).IsAssignableFrom(pd.PropertyType)
            && pd.PropertyType != typeof(string)
            && typeof(IEnumerable).IsAssignableFrom(pd.PropertyType)
            && !(pd.PropertyType.IsArray && pd.PropertyType.GetElementType()?.GetTypeInfo().IsPrimitive == true);
        }

        /// <summary>
        /// Use this method instead of Convert.ChangeType. Makes Convert.ChangeType work with Nullable types.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ChangeType(object value, Type type)
        {
            Type? nullableUnderlyingType = Nullable.GetUnderlyingType(type);
            if (nullableUnderlyingType != null)
            {
                if (value is string v && nullableUnderlyingType != typeof(string))
                {
                    if (ValueConvert.IsEmpty(v))
                    {
                        return null!;
                    }
                }

                value = ChangeType(value, nullableUnderlyingType, CultureInfo.InvariantCulture);
                return value is DBNull ? null! : value;
            }
            return !type.GetTypeInfo().IsInterface ? TypeConverterHelper.ChangeType(value, type, CultureInfo.InvariantCulture) : value;
        }

        /// <summary>
        /// Use this method instead of Convert.ChangeType. Makes Convert.ChangeType work with Nullable types.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static object ChangeType(object value, Type type, IFormatProvider provider)
        {
            Type? nullableUnderlyingType = Nullable.GetUnderlyingType(type);
            if (nullableUnderlyingType != null)
            {
                if (value is string v && nullableUnderlyingType != typeof(string))
                {
                    if (ValueConvert.IsEmpty(v))
                    {
                        return null!;
                    }
                }

                value = ChangeType(value, nullableUnderlyingType, provider);
                return value is DBNull ? null! : value;
            }

            return TypeConverterHelper.ChangeType(value, type, provider);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="nullableType"/> is a
        /// nullable value type of the form <c>Nullable&lt;T&gt;</c>.
        /// </summary>
        /// <param name="nullableType">The <see cref="Type"/> to inspect. Cannot be <c>null</c>.</param>
        /// <returns><c>true</c> if <paramref name="nullableType"/> is <c>Nullable&lt;T&gt;</c>; otherwise <c>false</c>.</returns>
        public static bool IsNullableType(Type nullableType)
        {
            ArgumentNullException.ThrowIfNull(nullableType);

            bool result = false;
            if (nullableType.GetTypeInfo().IsGenericType && !nullableType.GetTypeInfo().IsGenericTypeDefinition && (nullableType.GetGenericTypeDefinition() == typeof(Nullable<>)))
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Returns a <see cref="Nullable{T}"/> type for the supplied <paramref name="type"/>
        /// when <paramref name="type"/> is a value type. If <paramref name="type"/> is already
        /// a nullable type or a reference type, the original type is returned.
        /// </summary>
        /// <param name="type">The input <see cref="Type"/>. May be <c>null</c> (preserves existing behavior).</param>
        /// <returns>
        /// A <see cref="Type"/> representing <c>Nullable&lt;T&gt;</c> for value types, or the original
        /// <paramref name="type"/> if it is already nullable or a reference type.
        /// </returns>
        public static Type GetNullableType(Type type)
        {
            if (type == null)
            {
                return null!;
            }

            if (IsNullableType(type))
            {
                return type;
            }

            Type? underlyingType = Nullable.GetUnderlyingType(type);
            underlyingType ??= type;
            return underlyingType.GetTypeInfo().IsValueType ? typeof(Nullable<>).MakeGenericType(type) : type;
        }

        /// <summary>
        /// Returns null if value is DBNull and specified type is a Nullable type. Otherwise the value is returned unchanged.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object FixDbNUllasNull(object value, Type type)
        {
            if (type == null)
            {
                return value;
            }

            Type? nullableUnderlyingType = Nullable.GetUnderlyingType(type);
            if (nullableUnderlyingType != null)
            {
                if (value is DBNull)
                {
                    return null!;
                }
            }

            /*
             * Do not return DBNull for strong typed properties of an object. For example, if Parsing a string failed
             * (e.g. if an empty string was passed in as argument) we need to check if it as object and in that
             * case return null. Only if it is a ValueType type (that is not nullable) then we should return DBNull
             * so that it also works with DataRowView.
             * */
            if (!type.GetTypeInfo().IsValueType)
            {
                if (value is DBNull)
                {
                    return null!;
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the underlying type of a Nullable type. For .NET 1.0 and 1.1 this method will always return null.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetUnderlyingType(Type type)
        {
            return type == null! ? null! : Nullable.GetUnderlyingType(type)!;
        }

        /// <exclude/>
        private class TypeConverterHelper
        {
            public static object ChangeType(object value, Type type)
            {
                return ChangeType(value, type, null!);
            }

            public static object ChangeType(object value, Type type, IFormatProvider provider)
            {
                // Fix for defects: 13036, 13024, 12601  & 12716
                if (value == null)
                {
                    return null!;
                }

                TypeConverter typeConverter = TypeDescriptor.GetConverter(value.GetType());
                return typeConverter != null && typeConverter.CanConvertTo(type)
                    ? typeConverter.ConvertTo(value, type)!
                    : value is DBNull ? DBNull.Value : Convert.ChangeType(value, type, provider);
            }
        }
    }

    /// <summary>
    /// <see cref="ValueConvert"/> provides conversion routines for values
    /// to convert them to another type and routines for formatting values.
    /// </summary>
    public static class ValueConvert
    {
        /// <overload>
        /// Converts value from one type to another using an optional <see cref="IFormatProvider"/>.
        /// </overload>
        /// <summary>
        /// Converts value from one type to another using an optional <see cref="IFormatProvider"/>.
        /// </summary>
        /// <param name="value">The original value.</param>
        /// <param name="type">The target type.</param>
        /// <param name="provider">A <see cref="IFormatProvider"/> used to format or parse the value.</param>
        /// <returns>The new value in the target type.</returns>
        public static object ChangeType(object value, Type type, IFormatProvider provider)
        {
            return ChangeType(value, type, provider, false);
        }

        /// <summary>
        /// Converts value from one type to another using an optional <see cref="IFormatProvider"/>.
        /// </summary>
        /// <param name="value">The original value.</param>
        /// <param name="type">The target type.</param>
        /// <param name="provider">A <see cref="IFormatProvider"/> used to format or parse the value.</param>
        /// <param name="returnDbNUllIfNotValid">Indicates whether exceptions should be avoided or catched and return value should be DBNull if
        /// it cannot be converted to the target type.</param>
        /// <returns>The new value in the target type.</returns>
        public static object ChangeType(object value, Type type, IFormatProvider provider, bool returnDbNUllIfNotValid)
        {
            return ChangeType(value, type, provider, string.Empty, returnDbNUllIfNotValid);
        }

        /// <summary>
        /// Converts value from one type to another using an optional <see cref="IFormatProvider"/>.
        /// </summary>
        /// <param name="value">The original value.</param>
        /// <param name="type">The target type.</param>
        /// <param name="provider">A <see cref="IFormatProvider"/> used to format or parse the value.</param>
        /// <param name="format">Format string.</param>
        /// <param name="returnDbNUllIfNotValid">Indicates whether exceptions should be avoided or catched and return value should be DBNull if
        /// it cannot be converted to the target type.</param>
        /// <returns>The new value in the target type.</returns>
        public static object ChangeType(object value, Type type, IFormatProvider provider, string format,
                                        bool returnDbNUllIfNotValid)
        {
            Type? nullableUnderlyingType = Nullable.GetUnderlyingType(type);
            if (nullableUnderlyingType != null)
            {
                value = ChangeType(value, nullableUnderlyingType, provider, true);
                return NullableHelperInternal.FixDbNUllasNull(value, type);
            }

            if (value != null && type != null && !type.IsAssignableFrom(value.GetType()))
            {
                try
                {
                    if (value is string v)
                    {
                        value = format != null && format.Length > 0
                            ? Parse(v, type, provider, format, returnDbNUllIfNotValid)
                            : Parse(v, type, provider, string.Empty, returnDbNUllIfNotValid);
                    }
                    else if (value is DBNull)
                    {
                        // value = null; changed after 4.1.0.50: do not set it to null - this causes then issues
                        // if you have a DataTable and the key is used for lookups, e.g.
                        // see sample in http://www.syncfusion.com/support/forums/message.aspx?MessageID=40207
                        // For NullableTypes the above call to NullableHelper.FixDbNUllasNull will
                        // take care of converting DbNull to null for nullable types only.
                    }
                    else if (type.GetTypeInfo().IsEnum)
                    {
                        value = Convert.ChangeType(value, typeof(int), provider);
                        value = Enum.ToObject(type, (int)value);
                    }
                    else
                    {
                        value = type == typeof(string) && value is not IConvertible
                            ? value.ToString()!
                            : type == typeof(DateOnly) && value is DateTime time
                            ? ChangeType(DateOnly.FromDateTime(time), type, provider)
                            : type == typeof(TimeOnly) && value is DateTime dateTime
                            ? ChangeType(TimeOnly.FromDateTime(dateTime), type, provider)
                            : NullableHelperInternal.ChangeType(value, type, provider);
                    }
                }
                catch
                {
                    if (returnDbNUllIfNotValid)
                    {
                        return DBNull.Value;
                    }

                    throw;
                }
            }

            return (value == null || value is DBNull) && type == typeof(string) ? null! : value!;
        }

        private static Hashtable _cachedDefaultValues = [];

        /// <summary>
        /// Parses the given text using the resultTypes "Parse" method or using a type converter.
        /// </summary>
        /// <param name="s">The text to parse.</param>
        /// <param name="resultType">The requested result type.</param>
        /// <param name="provider">A <see cref="IFormatProvider"/> used to format or parse the value. Can be NULL.</param>
        /// <param name="format">A format string used in a <see cref="object.ToString"/> call. Right now
        /// format is only interpreted to enable roundtripping for formatted dates.
        /// </param>
        /// <returns>The new value in the target type.</returns>
        public static object Parse(string s, Type resultType, IFormatProvider provider, string format)
        {
            return Parse(s, resultType, provider, format, false);
        }

        /// <summary>
        /// Parse the given text using the resultTypes "Parse" method or using a type converter.
        /// </summary>
        /// <param name="s">The text to parse.</param>
        /// <param name="resultType">The requested result type.</param>
        /// <param name="provider">A <see cref="IFormatProvider"/> used to format or parse the value. Can be NULL.</param>
        /// <param name="format">A format string used in a <see cref="object.ToString"/> call. Right now
        /// format is only interpreted to enable roundtripping for formatted dates.
        /// </param>
        /// <param name="returnDbNUllIfNotValid">Indicates whether DbNull should be returned if value cannot be parsed. Otherwise an exception is thrown.</param>
        /// <returns>The new value in the target type.</returns>
        public static object Parse(string s, Type resultType, IFormatProvider provider, string format,
                                   bool returnDbNUllIfNotValid)
        {
            object value = ParseText(s, resultType, provider, format, returnDbNUllIfNotValid);
            return NullableHelperInternal.FixDbNUllasNull(value, resultType);
        }

        /// <summary>
        /// Parse the given text using the resultTypes "Parse" method or using a type converter.
        /// </summary>
        /// <param name="s">The text to parse.</param>
        /// <param name="resultType">The requested result type.</param>
        /// <param name="provider">A <see cref="IFormatProvider"/> used to format or parse the value. Can be NULL.</param>
        /// <param name="formats">A string array holding permissible formats used in a <see cref="object.ToString"/> call. Right now
        /// formats is only interpreted to enable roundtripping for formatted dates.
        /// </param>
        /// <param name="returnDbNUllIfNotValid">Indicates whether DbNull should be returned if value cannot be parsed. Otherwise an exception is thrown.</param>
        /// <returns>The new value in the target type.</returns>
        public static object Parse(string s, Type resultType, IFormatProvider provider, string[] formats,
                                   bool returnDbNUllIfNotValid)
        {
            object value = ParseText(s ?? string.Empty, resultType, provider, "", formats, returnDbNUllIfNotValid);
            return NullableHelperInternal.FixDbNUllasNull(value, resultType);
        }

        private static object ParseText(string s, Type resultType, IFormatProvider provider, string format,
                                     bool returnDbNUllIfNotValid)
        {
            return ParseText(s, resultType, provider, format, null!, returnDbNUllIfNotValid);
        }

        private static object ParseDouble(string s, Type resultType, IFormatProvider provider, bool returnDbNUllIfNotValid)
        {
            object? result = null;
            if (IsEmpty(s))
            {
                return DBNull.Value;
            }

            if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double d))
            {
                result = Convert.ChangeType(d, resultType, provider);
                return result;
            }

            if (returnDbNUllIfNotValid)
            {
                if (resultType == typeof(double) || resultType == typeof(float))
                {
                    return DBNull.Value;
                }
            }
            return result!;
        }

        private static object ParseDecimal(string s, Type resultType, IFormatProvider provider)
        {
            object? result = null;
            if (IsEmpty(s))
            {
                return DBNull.Value;
            }

            if (decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d))
            {
                result = Convert.ChangeType(d, resultType, provider);
                return result;
            }
            return result!;
        }

        private static object ParseDateTime(string s, Type resultType, IFormatProvider provider, string format,
                                     string[] formats)
        {
            _ = resultType;
            if (IsEmpty(s))
            {
                return DBNull.Value;
            }

            if (formats == null || (formats.GetLength(0) == 0 && format.Length > 0))
            {
                formats = [format, "G", "g", "f", "F", "d", "D"];
            }

            if (formats.GetLength(0) > 0)
            {
                if (DateTime.TryParseExact(s, formats, provider,
                                           DateTimeStyles.AllowInnerWhite | DateTimeStyles.AllowLeadingWhite |
                                           DateTimeStyles.AllowTrailingWhite | DateTimeStyles.AllowWhiteSpaces,
                                           out DateTime dtresult))
                {
                    return dtresult;
                }
            }

            _ = DateTime.TryParse(s, provider,
                              DateTimeStyles.AllowInnerWhite | DateTimeStyles.AllowLeadingWhite |
                              DateTimeStyles.AllowTrailingWhite |
                              DateTimeStyles.AllowWhiteSpaces, out DateTime validDateTime);
            return validDateTime;
        }

        private static object ParseDateOnly(string dateInputValue, IFormatProvider provider,
                            string[] formats, bool returnDbNUllIfNotValid)
        {
            if (IsEmpty(dateInputValue))
            {
                return DBNull.Value;
            }

            if (formats == null || formats.Length == 0)
            {
                formats = ["d", "D", "M", "Y", "yyyy-MM-dd", "dd/MM/yyyy", "MM/dd/yyyy", "dddd, dd MMMM yyyy"];
            }

            return DateOnly.TryParseExact(dateInputValue, formats, provider as CultureInfo,
                                    DateTimeStyles.AllowInnerWhite | DateTimeStyles.AllowLeadingWhite |
                                    DateTimeStyles.AllowTrailingWhite | DateTimeStyles.AllowWhiteSpaces,
                                    out DateOnly dateOnlyResult)
                ? dateOnlyResult
                : DateOnly.TryParse(dateInputValue, provider as CultureInfo, DateTimeStyles.AllowWhiteSpaces, out DateOnly validDateOnly)
                ? validDateOnly
                : returnDbNUllIfNotValid ? DBNull.Value : null!;
        }

        private static object ParseTimeOnly(string timeInputValue, bool returnDbNULLIfNotValid)
        {
            return string.IsNullOrWhiteSpace(timeInputValue)
                ? DBNull.Value
                : TimeOnly.TryParse(timeInputValue, out TimeOnly timeonly) ? timeonly : returnDbNULLIfNotValid ? DBNull.Value : null!;
        }

        private static object ParseTimeSpan(string s)
        {
            if (IsEmpty(s))
            {
                return DBNull.Value;
            }

            bool isValid = false;
            if (TimeSpan.TryParse(s, out TimeSpan timespan))
            {
                isValid = true;
            }

            return isValid ? timespan : timespan;
        }

        private static object ParseBool(string s)
        {
            if (IsEmpty(s))
            {
                return DBNull.Value;
            }

            if (s == "1" || s.Equals(bool.TrueString, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            else if (s == "0" || s.Equals(bool.FalseString, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            return false;
        }

        private static object ParseLong(string s, Type resultType, IFormatProvider provider, bool returnDbNUllIfNotValid)
        {
            object? result = null;
            if (IsEmpty(s))
            {
                return DBNull.Value;
            }

            if (long.TryParse(s, NumberStyles.Any, provider, out long d))
            {
                result = Convert.ChangeType(d, resultType, provider);
                return result;
            }

            if (returnDbNUllIfNotValid)
            {
                if (resultType.GetTypeInfo().IsPrimitive && !resultType.GetTypeInfo().IsEnum)
                {
                    return DBNull.Value;
                }
            }
            return result!;
        }

        private static object ParseULong(string s, Type resultType, IFormatProvider provider, bool returnDbNUllIfNotValid)
        {
            object? result = null;
            if (IsEmpty(s))
            {
                return DBNull.Value;
            }

            if (ulong.TryParse(s, NumberStyles.Any, provider, out ulong d))
            {
                result = Convert.ChangeType(d, resultType, provider);
                return result;
            }

            if (returnDbNUllIfNotValid)
            {
                if (resultType.GetTypeInfo().IsPrimitive && !resultType.GetTypeInfo().IsEnum)
                {
                    return DBNull.Value;
                }
            }
            return result!;
        }

        private static object ParseNumber(string s, Type resultType, IFormatProvider provider, bool returnDbNUllIfNotValid)
        {
            object? result = null;
            if (IsEmpty(s))
            {
                return DBNull.Value;
            }

            if (double.TryParse(s, NumberStyles.Any, provider, out double d))
            {
                result = Convert.ChangeType(d, resultType, provider);
                return result;
            }

            if (returnDbNUllIfNotValid)
            {
                if (resultType.GetTypeInfo().IsPrimitive && !resultType.GetTypeInfo().IsEnum)
                {
                    return DBNull.Value;
                }
            }
            return result!;
        }

        private static object ParseText(string s, Type resultType, IFormatProvider provider, string format,
                                     string[] formats, bool returnDbNUllIfNotValid)
        {
            if (resultType == null) 
            {
                return s;
            }

            object result;

            try
            {
                if (typeof(double).IsAssignableFrom(resultType))
                {
                    return ParseDouble(s, resultType, provider, returnDbNUllIfNotValid);
                }
                else if (typeof(decimal).IsAssignableFrom(resultType))
                {
                    return ParseDecimal(s, resultType, provider);
                }
                else if (typeof(DateTime).IsAssignableFrom(resultType))
                {
                    return ParseDateTime(s, resultType, provider, format, formats);
                }
                else if (typeof(DateOnly).IsAssignableFrom(resultType))
                {
                    return ParseDateOnly(s, provider, formats, returnDbNUllIfNotValid);
                }
                else if (typeof(TimeOnly).IsAssignableFrom(resultType))
                {
                    return ParseTimeOnly(s, returnDbNUllIfNotValid);
                }
                else if (typeof(TimeSpan).IsAssignableFrom(resultType))
                {
                    return ParseTimeSpan(s);
                }
                else if (typeof(bool).IsAssignableFrom(resultType))
                {
                    return ParseBool(s);
                }
                else if (typeof(long).IsAssignableFrom(resultType))
                {
                    return ParseLong(s, resultType, provider, returnDbNUllIfNotValid);
                }
                else if (typeof(ulong).IsAssignableFrom(resultType))
                {
                    return ParseULong(s, resultType, provider, returnDbNUllIfNotValid);
                }
                else if (typeof(int).IsAssignableFrom(resultType)
                         || typeof(short).IsAssignableFrom(resultType)
                         || typeof(float).IsAssignableFrom(resultType)
                         || typeof(uint).IsAssignableFrom(resultType)
                         || typeof(ushort).IsAssignableFrom(resultType)
                         || typeof(byte).IsAssignableFrom(resultType))
                {
                    return ParseNumber(s, resultType, provider, returnDbNUllIfNotValid);
                }
                else if (resultType == typeof(Type))
                {
                    result = Type.GetType(s)!;
                    return result;
                }

                TypeConverter typeConverter = TypeDescriptor.GetConverter(resultType);

                if (typeConverter is NullableConverter)
                {
                    Type nullableUnderlyingType = NullableHelperInternal.GetUnderlyingType(resultType);
                    if (nullableUnderlyingType != null)
                    {
                        return ParseText(s, nullableUnderlyingType, provider, format, formats, returnDbNUllIfNotValid);
                    }
                }

                if (typeConverter != null &&
                    typeConverter.CanConvertFrom(typeof(string)) &&
                    s != null && s.Length > 0)
                {
                    result = provider is CultureInfo info ? typeConverter.ConvertFrom(null, info, s)! : typeConverter.ConvertFrom(s)!;

                    return result;
                }
            }
            catch
            {
                if (returnDbNUllIfNotValid)
                {
                    return DBNull.Value;
                }

                throw;
            }

            return DBNull.Value;
        }

        /// <summary>
        /// Generates display text using the specified format, culture info and number format.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <param name="valueType">The value type on which formatting is based. The original value will first be converted to this type.</param>
        /// <param name="format">The format like in ToString(string format).</param>
        /// <param name="ci">The <see cref="CultureInfo"/> for formatting the value.</param>
        /// <param name="nfi">The <see cref="NumberFormatInfo"/> for formatting the value.</param>
        /// <returns>The string with the formatted text for the value.</returns>
        public static string FormatValue(object value, Type valueType, string format, CultureInfo ci,
                                         NumberFormatInfo nfi)
        {
            string strResult;
            object obj;
            try
            {
                if (value is string v)
                {
                    return v;
                }
                else if (value is byte[]) // Picture
                {
                    return string.Empty;
                }
                else if (value == null || valueType == null || value.GetType() == valueType)
                {
                    obj = value!;
                }
                else
                {
                    try
                    {
                        obj = ChangeType(value, valueType, ci, true);
                    }
                    catch (Exception ex)
                    {
                        obj = value;
                        if (!(ex is FormatException || ex.InnerException is FormatException))
                        {
                            throw;
                        }
                    }
                }

                if (obj is null or DBNull)
                {
                    strResult = string.Empty; // or "NullString"
                }
                else
                {
                    if (obj is IFormattable formattableValue)
                    {
                        IFormatProvider? provider = null;
                        if (nfi != null && obj is not DateTime)
                        {
                            provider = nfi;
                        }
                        else if (ci != null)
                        {
                            provider = obj is DateTime
                                           ? ci.DateTimeFormat
                                           : ci.NumberFormat;
                        }

                        strResult = (format != null && format.Length > 0) || nfi != null
                            ? formattableValue.ToString(format, provider)
                            : formattableValue.ToString()!;
                    }
                    else
                    {
                        TypeConverter tc = TypeDescriptor.GetConverter(obj.GetType());
                        strResult = tc.CanConvertTo(typeof(string))
                            ? (string)tc.ConvertTo(null, ci, obj, typeof(string))!
                            : obj is IConvertible ? Convert.ToString(obj, ci)! : obj.ToString()!;
                    }
                }
            }
            catch
            {
                throw; // TODO: should I throw a more specific instead?
            }

            strResult ??= string.Empty;

            if (AllowFormatValueTrimEnd)
            {
                strResult = strResult.TrimEnd();
            }

            return strResult;
        }

        /// <summary>
        /// Indicates whether <see cref="FormatValue"/> should trim whitespace characters from
        /// the end of the formatted text.
        /// </summary>
        public static bool AllowFormatValueTrimEnd { get; set; }

        /// <summary>
        /// Returns a representative value for any given type.
        /// </summary>
        /// <param name="type">The <see cref="Type"/>.</param>
        /// <returns>A value with the specified type.</returns>
        public static object GetDefaultValue(Type type)
        {
            object value;

            if (type == null)
            {
                return "0";
            }

            lock (_cachedDefaultValues)
            {
                if (_cachedDefaultValues.ContainsKey(type))
                {
                    value = _cachedDefaultValues[type];
                }
                else
                {
                    value = type.FullName switch
                    {
                        "System.Double" or "System.Single" or "System.Decimal" => 123.4567,
                        "System.Boolean" => true,
                        "System.String" => string.Empty,
                        "System.DateTime" => DateTime.Now,
                        "System.Int32" or "System.Int16" or "System.Int64" or "System.SByte" or "System.Byte" or "System.UInt16" or "System.UInt32" or "System.UInt64" => 123,
                        "System.Char" => 'A',
                        "System.DBNull" => DBNull.Value,
                        _ => string.Empty,
                    };
                    _cachedDefaultValues[type] = value;
                }

                return value;
            }
        }

        /// <summary>
        /// Overloaded. Parses the given string including type information. String can be in format %lt;type&gt; 'value'.
        /// </summary>
        /// <param name="valueAsString"></param>
        /// <param name="retVal"></param>
        /// <returns></returns>
        private static bool ParseValueWithTypeInformation(string valueAsString, out object retVal)
        {
            return ParseValueWithTypeInformation(valueAsString, out retVal);
        }

        /// <summary>
        /// Parses the given string including type information. String can be in format %lt;type&gt; 'value'.
        /// </summary>
        /// <param name="valueAsString"></param>
        /// <param name="retVal"></param>
        /// <param name="allowConvertFromBase64">Indicates whether TypeConverter should be checked whether the type to be
        /// parsed supports conversion to/from byte array (e.g. an Image).</param>
        /// <returns></returns>
        public static bool ParseValueWithTypeInformation(string valueAsString, object retVal,
                                                         bool allowConvertFromBase64)
        {
            if (string.IsNullOrEmpty(valueAsString)) { valueAsString = string.Empty; }
            if (valueAsString.Length > 1 && valueAsString[0] == '\'' && valueAsString[^1] == '\'')
            {
                retVal = valueAsString[1..^1];
                return true;
            }
            else if (valueAsString.Length > 0 && valueAsString[0] == '<')
            {
                int closeBracket = valueAsString.IndexOf('>', StringComparison.InvariantCulture);
                if (closeBracket > 1)
                {
                    string typeName = valueAsString[1..closeBracket];
                    if (typeName == "null")
                    {
                        return true;
                    }
                    else if (typeName == "System.DBNull")
                    {
                        retVal = DBNull.Value;
                    }
                    else
                    {
                        valueAsString = valueAsString[(closeBracket + 1)..].Trim();
                        if (valueAsString.Length > 1 && valueAsString[0] == '\'' && valueAsString[^1] == '\'')
                        {
                            valueAsString = valueAsString[1..^1];
                            Type type = GetType(typeName);
                            if (type != null)
                            {
                                bool handled = false;

                                if (allowConvertFromBase64)
                                {
                                    handled = TryConvertFromBase64String(type, valueAsString, out retVal);
                                }

                                if (!handled)
                                {
                                    retVal = Parse(valueAsString, type,
                                                                CultureInfo.InvariantCulture, string.Empty);
                                }

                                return true;
                            }
                        }
                    }
                }
            }

            retVal = valueAsString;
            return false;
        }

        /// <summary>
        /// Indicates whether the TypeConverter associated with the type supports conversion to/from a byte array (e.g. an Image).
        /// If that is the case the string is converted to a byte array from a base64 string.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="valueAsString"></param>
        /// <param name="retVal"></param>
        /// <returns></returns>
        public static bool TryConvertFromBase64String(Type type, string valueAsString, out object retVal)
        {
            bool handled = false;
            retVal = null!;
            TypeConverter tc = TypeDescriptor.GetConverter(type);
            if (tc != null)
            {
                if (tc.CanConvertFrom(typeof(byte[])))
                {
                    byte[] byteArray = Convert.FromBase64String(valueAsString);
                    retVal = tc.ConvertFrom(byteArray)!;
                    handled = true;
                }
                else if (tc.CanConvertFrom(typeof(MemoryStream)))
                {
                    using MemoryStream ms = new(Convert.FromBase64String(valueAsString));
                    retVal = tc.ConvertFrom(ms)!;
                    handled = true;
                }
            }

            return handled;
        }

        /// <summary>
        /// Formats the given value as string including type information. String will be in format %lt;type&gt; 'value'.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FormatValueWithTypeInformation(object value)
        {
            if (value is string v)
            {
                return "'" + v + "'";
            }
            else if (value is DBNull)
            {
                return "<System.DBNull>";
            }
            else if (value == null)
            {
                return "<null>";
            }
            else
            {
                string? valueAsString = FormatValue(value, typeof(string), "",
                                             CultureInfo.InvariantCulture, null!);

                return "<" + GetTypeName(value.GetType()) + "> '" + valueAsString + "'";
            }
        }

        /// <summary>
        /// Returns the type name. If type is not in mscorlib, the assembly name is appended.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetTypeName(Type type)
        {
            return type?.FullName ?? string.Empty;
        }

        /// <summary>
        /// Returns the type from the specified name. If an assembly name is appended the list of currently loaded
        /// assemblies in the current AppDomain are checked.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static Type GetType(string typeName)
        {
            return Type.GetType(typeName)!;
        }

        /// <summary>
        /// Indicates whether string is null or empty.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsEmpty(string str)
        {
            return str == null || str.Length == 0;
        }
    }
}