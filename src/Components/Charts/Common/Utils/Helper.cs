using System.Dynamic;
using System.Globalization;
using System.Text.Json;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Helper utilities used across DataVizCommon components.
    /// </summary>
    public class DataVizCommonHelper
    {
        #region Constants
        private const string SPACE = " ";
        #endregion

        #region Internal Methods

        /// <summary>
        /// Determines a simple identifier for supported dynamic-like data types.
        /// </summary>
        /// <param name="dataType">The <see cref="Type"/> to inspect.</param>
        /// <returns>
        /// <c>"JsonElement"</c>, <c>"ExpandoObject"</c>, <c>"DynamicObject"</c> when matched; otherwise an empty string.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataType"/> is <c>null</c>.</exception>
        internal static string FindDataType(Type dataType)
        {
            if (dataType.Equals(typeof(JsonElement)))
            {
                return "JsonElement";
            }
            else if (dataType.Equals(typeof(ExpandoObject)))
            {
                return "ExpandoObject";
            }
            else if (dataType.BaseType is not null && dataType.BaseType.Equals(typeof(DynamicObject)))
            {
                return "DynamicObject";
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Invokes event handler function of the corresponding event name with parameters.
        /// </summary>
        /// <param name="eventFn">Action to invoke the event handler method.</param>
        /// <param name="eventArgs">Arguments of the event handler method.</param>
        internal static void InvokeEvent<T>(object eventFn, T eventArgs)
        {
            if (eventFn is not null)
            {
                Action<T> eventHandler = (Action<T>)eventFn;
                eventHandler.Invoke(eventArgs);
            }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Converts a CSS-style size string (for example "50%", "20px", "auto", or "10") into a numeric pixel value.
        /// </summary>
        /// <param name="size">The size string to parse.</param>
        /// <param name="containerSize">The container size used to resolve percentages.</param>
        /// <returns>
        /// The resolved numeric value in pixels when parsing succeeds; otherwise <see cref="double.NaN"/> for "auto", invalid, or empty input.
        /// </returns>
        public static double StringToNumber(string size, double containerSize)
        {
            return !string.IsNullOrEmpty(size) && size != "auto"
                ? size.Contains('%', StringComparison.InvariantCulture) ? containerSize / 100 * (int)double.Parse(size.Replace("%", SPACE, StringComparison.InvariantCulture), null) : double.Parse(size.ToUpper(CultureInfo.InvariantCulture).Replace("PX", string.Empty, StringComparison.InvariantCulture), CultureInfo.InvariantCulture)
                : double.NaN;
        }
        #endregion
    }
}