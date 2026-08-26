using System;
using System.Collections.Generic;
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

        #region Nested Validator

        /// <summary>
        /// Validates a string value against the WAI-ARIA 1.2 abstract role list.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The Chart component family exposes an <c>AccessibilityRole</c> string parameter
        /// on six different types (<see cref="SfChart"/>,
        /// <see cref="ChartAnnotations"/>,
        /// <see cref="ChartTitleStyle"/>,
        /// <see cref="ChartSubTitleStyle"/>,
        /// <see cref="ChartLegendSettings"/>,
        /// <see cref="ChartTrendline"/>,
        /// <see cref="ChartSeries"/>). The role is
        /// forwarded verbatim into the DOM <c>role</c> attribute, so an unknown
        /// value (for example <c>"count"</c>) becomes an invalid ARIA role at
        /// runtime. This validator runs at the parameter setter and rejects
        /// unknown non-empty values with <see cref="ArgumentException"/> so the
        /// bug is caught at component initialization rather than at audit time.
        /// </para>
        /// <para>
        /// Empty / null values are always considered valid — the component's
        /// own renderers fall back to <c>"region"</c> when the role is unset,
        /// and existing valid values (region, status, group, img, heading,
        /// button, link, list, listitem, navigation, presentation, none, etc.)
        /// are unchanged.
        /// </para>
        /// </remarks>
        internal static class AriaRoleValidator
        {
            // WAI-ARIA 1.2 abstract roles, compared case-insensitively.
            // Sourced from
            // https://www.w3.org/TR/wai-aria-1.2/#role_definitions — the same
            // set used by the audit's Full Assessment spec.
            private static readonly HashSet<string> ValidRoles = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "alert", "alertdialog", "application", "article", "banner",
                "button", "cell", "checkbox", "columnheader", "combobox",
                "complementary", "contentinfo", "definition", "dialog",
                "directory", "document", "feed", "figure", "form", "grid",
                "gridcell", "group", "heading", "img", "input", "link", "list",
                "listbox", "listitem", "log", "main", "marquee", "math", "menu",
                "menubar", "menuitem", "menuitemcheckbox", "menuitemradio",
                "navigation", "none", "note", "option", "presentation",
                "progressbar", "radio", "radiogroup", "region", "row", "rowgroup",
                "rowheader", "scrollbar", "search", "searchbox", "separator",
                "slider", "spinbutton", "status", "switch", "tab", "table",
                "tablist", "tabpanel", "term", "textbox", "timer", "toolbar",
                "tooltip", "tree", "treegrid", "treeitem"
            };

            /// <summary>
            /// <para>
            /// Ensures the supplied role string is a valid WAI-ARIA abstract role.
            /// </para>
            /// </summary>
            /// <param name="value">The role string supplied by the consumer. May be null or empty.</param>
            /// <param name="paramName">The parameter name to surface in any thrown exception.</param>
            /// <exception cref="ArgumentException">
            /// Thrown when <paramref name="value"/> is non-empty and is not a recognized
            /// WAI-ARIA role. Empty / null values are accepted unchanged.
            /// </exception>
            internal static void EnsureValidRole(string? value, string paramName)
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                if (!ValidRoles.Contains(value))
                {
                    throw new ArgumentException(
                        $"'{value}' is not a valid WAI-ARIA role for {paramName}. " +
                        "Use one of the roles listed at " +
                        "https://www.w3.org/TR/wai-aria-1.2/#role_definitions " +
                        "(for example: region, status, group, img, heading, button, link, " +
                        "list, listitem, navigation, presentation, none). " +
                        "Leave the property empty to use the component default.",
                        paramName);
                }
            }
        }
        #endregion
    }
}
