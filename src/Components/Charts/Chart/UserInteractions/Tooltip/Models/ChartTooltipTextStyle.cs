using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Charts.Internal;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides options to customize the text style of the tooltip.
    /// </summary>
    /// <remarks>
    /// Set font size, family, weight, color, and style for tooltip text. Values default to theme settings if empty.
    /// </remarks>
    public class ChartTooltipTextStyle : ChartDefaultFont
    {
        #region Properties

        /// <summary>
        /// Gets or sets the parent tooltip settings to which this text style belongs.
        /// </summary>
        /// <value>The parent <see cref="ChartTooltipSettings"/> if available.</value>
        [CascadingParameter]
        private ChartTooltipSettings? Parent { get; set; }

        /// <summary>
        /// Gets or sets the font size for the tooltip text.
        /// </summary>
        /// <value>
        /// A string representing the font size. Defaults to <b>12px</b> if not set (theme fallback).
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartTooltipTextStyle Size="15px" />
        /// ]]>
        /// </code>
        /// </example>
        public override string Size { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the font family of the tooltip text.
        /// </summary>
        /// <value>
        /// The font family string. Defaults to the theme's font family (e.g., <b>Roboto</b> in Fluent).
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartTooltipTextStyle FontFamily="Inter, Roboto, Arial" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override string FontFamily { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the font weight of the tooltip text.
        /// </summary>
        /// <value>
        /// The font weight (e.g., <c>bold</c>, <c>400</c>). Defaults to theme value.
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartTooltipTextStyle FontWeight="600" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override string FontWeight { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the font color of the tooltip text.
        /// </summary>
        /// <value>
        /// CSS color string (hex/rgba). Defaults to theme value (e.g., <b>rgba(249, 250, 251, 1)</b>).
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartTooltipTextStyle Color="#d1d5db" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override string Color { get; set; } = string.Empty;

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Initializes the component and registers the text style with the parent tooltip.
        /// </summary>
        /// <remarks>
        /// Called by the framework when the component is initialized.
        /// </remarks>
        /// <inheritdoc cref="ComponentBase.OnInitialized" />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Tracker is ChartTooltipSettings chartTooltipSettings)
            {
                Parent = chartTooltipSettings;
            }

            Parent?.UpdateTooltipProperties("TextStyle", this);
        }

        /// <summary>
        /// Releases references and child content.
        /// </summary>
        protected override ValueTask DisposeAsyncCore()
        {
            Parent = null;
            ChildContent = null!;
            return base.DisposeAsyncCore();
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Gets the effective font size (explicit value or theme fallback).
        /// </summary>
        /// <param name="chartThemeStyle">The chart theme style.</param>
        /// <returns>The resolved font size.</returns>
        internal string GetFontSize(ChartThemeStyle chartThemeStyle)
        {
            return !string.IsNullOrEmpty(Size) ? Size : chartThemeStyle?.TooltipTextSize ?? string.Empty;
        }

        /// <summary>
        /// Gets the effective font weight (explicit value or theme fallback).
        /// </summary>
        /// <param name="chartThemeStyle">The chart theme style.</param>
        /// <returns>The resolved font weight.</returns>
        internal string GetFontWeight(ChartThemeStyle chartThemeStyle)
        {
            return !string.IsNullOrEmpty(FontWeight) ? FontWeight : chartThemeStyle?.ToolTipFontWeight ?? string.Empty;
        }

        /// <summary>
        /// Gets the effective font family (explicit value or theme fallback).
        /// </summary>
        /// <param name="chartThemeStyle">The chart theme style.</param>
        /// <returns>The resolved font family.</returns>
        internal string GetFontFamily(ChartThemeStyle chartThemeStyle)
        {
            return !string.IsNullOrEmpty(FontFamily) ? FontFamily : chartThemeStyle?.TooltipFontFamily ?? string.Empty;
        }

        #endregion
    }
}
