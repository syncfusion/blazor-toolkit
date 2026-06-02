using System.Globalization;
using System.Text;

namespace Syncfusion.Blazor.Toolkit.Internal
{
    /// <summary>
    /// Provides culture-aware formatting utilities for dates, numbers, currency symbols,
    /// and calendar operations used across Syncfusion Blazor Toolkit components.
    /// </summary>
    /// <exclude />
    internal static class Intl
    {
        /// <summary>
        /// Retrieves a map of ISO currency codes to their display symbols for all available cultures.
        /// The result is cached in <see cref="CurrencyData"/> for subsequent calls.
        /// </summary>
        /// <returns>A dictionary mapping ISO currency codes (for example "USD") to the culture-specific currency symbol (for example "$").</returns>
        /// <remarks>Neutral cultures are excluded and exceptions thrown while obtaining region info are propagated as <see cref="ArgumentException"/>.</remarks>
        /// <exclude />
        internal static Dictionary<string, string> GetCurrencyData()
        {
            CurrencyData ??= CultureInfo.GetCultures(CultureTypes.AllCultures)
                                .Where(c => !c.IsNeutralCulture && !c.Equals(CultureInfo.InvariantCulture))
                                .Select(culture =>
                                {
                                    try
                                    {
                                        return new RegionInfo(culture.Name);
                                    }
                                    catch (Exception e)
                                    {
                                        throw new ArgumentException(e.Message);
                                    }
                                })
                                .Where(ri => ri != null)
                                .GroupBy(ri => ri.ISOCurrencySymbol)
                                .ToDictionary(x => x.Key, x => x.First().CurrencySymbol);
            return CurrencyData;
        }

        /// <summary>
        /// Gets or sets the cached dictionary mapping ISO currency codes to culture-specific display symbols.
        /// </summary>
        /// <value>
        /// A dictionary initialized by <see cref="GetCurrencyData"/> and reused for subsequent calls.
        /// </value>
        /// <exclude />
        internal static Dictionary<string, string> CurrencyData { get; set; } = default!;

        /// <summary>
        /// Formats the provided date value according to the specified format string and culture.
        /// </summary>
        /// <typeparam name="T">A type implementing <see cref="IFormattable"/> representing a date/time value.</typeparam>
        /// <param name="date">The value to format.</param>
        /// <param name="format">A .NET format string. When not specified, the culture's default is used.</param>
        /// <returns>A culture-aware formatted date string.</returns>
        /// <remarks>
        /// If the current culture uses a date separator other than <c>/</c>, the separator character
        /// in <paramref name="format"/> is escaped so it is treated as a literal rather than a
        /// format specifier. ASCII digits are also replaced with the culture's native digits when available.
        /// </remarks>
        /// <exception cref="InvalidOperationException">Thrown when the formatting operation fails.</exception>
        internal static string GetDateFormat<T>(T date, string? format = null)
        {
            try
            {
                CultureInfo currentCulture = GetCulture();
                IFormattable? dateValue = date as IFormattable;
                // Culture-specific slash handling
                if (!string.IsNullOrEmpty(format))
                {
                    if (currentCulture.DateTimeFormat.DateSeparator != "/")
                    {
                        format = format.Replace("/", "'/'", StringComparison.Ordinal);
                    }
                }
                string? dateCulture = dateValue?.ToString(format, currentCulture);
                if (string.IsNullOrEmpty(dateCulture))
                {
                    return string.Empty;
                }
                dateCulture = GetNativeDigits(dateCulture, currentCulture.NumberFormat.NativeDigits);
                return dateCulture;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }

        /// <summary>
        /// Formats a numeric value according to the provided format string and culture, optionally using a specified currency symbol.
        /// </summary>
        /// <typeparam name="T">A type implementing <see cref="IFormattable"/> representing a numeric value.</typeparam>
        /// <param name="numberValue">The numeric value to format.</param>
        /// <param name="format">A .NET numeric format string.</param>
        /// <param name="currencyCode">Optional ISO currency code to use its symbol during formatting.</param>
        /// <returns>A string with the formatted numeric value according to the resolved culture and format.</returns>
        /// <remarks>
        /// When <paramref name="currencyCode"/> is provided, the culture's <see cref="NumberFormatInfo.CurrencySymbol"/>
        /// is temporarily replaced with the resolved symbol and restored after formatting to avoid side effects.
        /// </remarks>
        /// <exception cref="InvalidOperationException">Thrown when the formatting operation fails.</exception>
        internal static string GetNumericFormat<T>(T numberValue, string? format = null, string? currencyCode = null)
        {
            try
            {
                if (GetCulture().Clone() is not CultureInfo currentCulture)
                {
                    return string.Empty;
                }
                string cacheCurrency = currentCulture.NumberFormat.CurrencySymbol;
                string? currencySymbol = string.Empty;
                if (currencyCode != null && GetCurrencyData().TryGetValue(currencyCode, out currencySymbol) && currencySymbol != null)
                {
                    currentCulture.NumberFormat.CurrencySymbol = currencySymbol;
                }
                IFormattable? numericValue = numberValue as IFormattable;
                string? numericCulture = numericValue?.ToString(format, currentCulture);
                if (string.IsNullOrEmpty(numericCulture))
                {
                    if (currencyCode != null)
                    {
                        currentCulture.NumberFormat.CurrencySymbol = cacheCurrency;
                    }
                    return string.Empty;
                }
                numericCulture = GetNativeDigits(numericCulture, currentCulture.NumberFormat.NativeDigits);
                if (currencyCode != null)
                {
                    currentCulture.NumberFormat.CurrencySymbol = cacheCurrency;
                }
                return numericCulture;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }

        /// <summary>
        /// Calculates the week-of-year for the specified date according to the current culture and week rule.
        /// </summary>
        /// <param name="dateValue">The date for which to determine the calendar week number.</param>
        /// <param name="isLastDayOfWeek">If <c>true</c> treat the date as the last day of the week for calculation; otherwise use the culture's first day of week.</param>
        /// <param name="weekRule">The <see cref="CalendarWeekRule"/> to apply when computing the week number.</param>
        /// <returns>The computed week-of-year as an integer.</returns>
        /// <exclude />
        internal static int GetWeekOfYear(DateTime dateValue, bool isLastDayOfWeek = false, CalendarWeekRule weekRule = CalendarWeekRule.FirstDay)
        {
            CultureInfo currentCulture = GetCulture();
            DayOfWeek dayOfWeek = isLastDayOfWeek ? dateValue.DayOfWeek : currentCulture.DateTimeFormat.FirstDayOfWeek;
            dateValue = isLastDayOfWeek ? dateValue.AddDays(6) : dateValue;
            int weekNumber = currentCulture.Calendar.GetWeekOfYear(dateValue, weekRule, dayOfWeek);
            return weekNumber;
        }

        /// <summary>
        /// Returns an ordered list of narrow day names (single-character abbreviations) for the specified culture.
        /// </summary>
        /// <returns>A list of single-character strings representing narrow day names in culture order.</returns>
        /// <exclude />
        internal static List<string> GetNarrowDayNames()
        {
            List<string> narrowDays = [];
            CultureInfo currentCulture = GetCulture();
            string[] shortDays = currentCulture.DateTimeFormat.ShortestDayNames;
            foreach (string dayName in shortDays)
            {
                narrowDays.Add(dayName[0].ToString());
            }
            return narrowDays;
        }

        /// <summary>
        /// Returns the current <see cref="CultureInfo"/> for the executing environment.
        /// If <see cref="CultureInfo.CurrentCulture"/> is <c>null</c> for any reason, a fallback
        /// <see cref="CultureInfo"/> for <c>en-US</c> is returned.
        /// </summary>
        /// <exclude />
        /// <returns>The resolved <see cref="CultureInfo"/> (the current culture or the <c>en-US</c> fallback).</returns>
        internal static CultureInfo GetCulture()
        {
            return CultureInfo.CurrentCulture ?? new CultureInfo("en-US");
        }

        /// <summary>
        /// Replaces ASCII digit characters in the provided formatted string with culture-specific native digits.
        /// </summary>
        /// <param name="formatValue">A formatted numeric or date string containing ASCII digits 0-9.</param>
        /// <param name="nativeDigits">An array of 10 strings representing the culture's native digits for 0-9. If the array does not contain exactly 10 elements, the original string is returned unchanged.</param>
        /// <returns>The input string with ASCII digits replaced by the corresponding native digits, or the original string if replacement is not possible.</returns>
        /// <exclude />
        internal static string GetNativeDigits(string formatValue, string[] nativeDigits)
        {
            if (string.IsNullOrEmpty(formatValue) || nativeDigits == null || nativeDigits.Length != 10)
            {
                return formatValue;
            }

            // Use StringBuilder and append native digit strings for ASCII digits '0'..'9'.
            StringBuilder sb = new(formatValue.Length * (nativeDigits[0]?.Length ?? 1));
            foreach (char c in formatValue)
            {
                _ = c is >= '0' and <= '9' ? sb.Append(nativeDigits[c - '0']) : sb.Append(c);
            }
            return sb.ToString();
        }
    }
}
