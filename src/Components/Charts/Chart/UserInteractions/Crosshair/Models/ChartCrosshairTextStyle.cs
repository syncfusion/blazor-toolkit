using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Charts.Internal;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides options to customize the tooltip text style of the crosshair tooltip.
    /// </summary>
    public class ChartCrosshairTextStyle : ChartDefaultFont
    {
        #region Properties

        [CascadingParameter]
        private ChartAxisCrosshairTooltip? Parent { get; set; }

        /// <summary> 
        /// Gets or sets the color for the crosshair tooltip text. 
        /// </summary> 
        /// <value> 
        /// A CSS color string. The default is theme-dependent. For Fluent, the default is <b>rgba(249, 250, 251, 1)</b>.
        /// </value> 
        /// <remarks> 
        /// Accepts any valid CSS color (e.g., <c>#fff</c>, <c>rgba(255,255,255,0.9)</c>).
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartPrimaryXAxis>
        ///     <ChartAxisCrosshairTooltip Enable="true">
        ///         <ChartCrosshairTextStyle Color="blue" />
        ///     </ChartAxisCrosshairTooltip>
        /// </ChartPrimaryXAxis>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override string Color { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets the font size for the crosshair tooltip text. 
        /// </summary> 
        /// <value> 
        /// A string representing the font size (e.g., <c>"12px"</c>). The default is <b>12px</b>.
        /// </value> 
        /// <remarks>
        /// Adjust to ensure readability on different display densities.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartPrimaryXAxis>
        ///     <ChartAxisCrosshairTooltip Enable="true">
        ///         <ChartCrosshairTextStyle Size="15px" />
        ///     </ChartAxisCrosshairTooltip>
        /// </ChartPrimaryXAxis>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override string Size { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets the font family for the crosshair tooltip text. 
        /// </summary> 
        /// <value> 
        /// A string representing the font family. The default is theme-dependent (e.g., <b>Roboto</b> for Fluent).
        /// </value> 
        /// <remarks>
        /// Use this to align with organizational typography guidelines.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartPrimaryXAxis>
        ///     <ChartAxisCrosshairTooltip Enable="true">
        ///         <ChartCrosshairTextStyle FontFamily="Arial" />
        ///     </ChartAxisCrosshairTooltip>
        /// </ChartPrimaryXAxis>
        /// ]]>
        /// </code>
        /// </example>
        public override string FontFamily { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets the font weight for the crosshair tooltip text. 
        /// </summary> 
        /// <value> 
        /// A string representing the font weight (e.g., <c>"bold"</c> or <c>"400"</c>). The default is theme-dependent (typically <b>400</b>).
        /// </value> 
        /// <remarks>
        /// Heavier weights draw greater attention to the tooltip content.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartPrimaryXAxis>
        ///     <ChartAxisCrosshairTooltip Enable="true">
        ///         <ChartCrosshairTextStyle FontWeight="bold" />
        ///     </ChartAxisCrosshairTooltip>
        /// </ChartPrimaryXAxis>
        /// ]]>
        /// </code>
        /// </example>
        public override string FontWeight { get; set; } = string.Empty;

        #endregion

        #region Lifecycle Methods

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Tracker is ChartAxisCrosshairTooltip crosshairTooltip)
            {
                Parent = crosshairTooltip;
            }

            Parent?.UpdateChildProperties("TextStyle", this);
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Gets the effective font size, using theme defaults when not set.
        /// </summary>
        /// <param name="chartThemeStyle">The chart theme style.</param>
        /// <returns>The resolved font size.</returns>
        internal string GetFontSize(ChartThemeStyle chartThemeStyle)
        {
            return !string.IsNullOrEmpty(Size) ? Size : chartThemeStyle?.CrosshairTextSize ?? string.Empty;
        }

        /// <summary>
        /// Gets the effective font weight, using theme defaults when not set.
        /// </summary>
        /// <param name="chartThemeStyle">The chart theme style.</param>
        /// <returns>The resolved font weight.</returns>
        internal string GetFontWeight(ChartThemeStyle chartThemeStyle)
        {
            return !string.IsNullOrEmpty(FontWeight) ? FontWeight : chartThemeStyle?.CrosshairFontWeight ?? string.Empty;
        }

        /// <summary>
        /// Gets the effective font family, using theme defaults when not set.
        /// </summary>
        /// <param name="chartThemeStyle">The chart theme style.</param>
        /// <returns>The resolved font family.</returns>
        internal string GetFontFamily(ChartThemeStyle chartThemeStyle)
        {
            return !string.IsNullOrEmpty(FontFamily) ? FontFamily : chartThemeStyle?.CrosshairFontFamily ?? string.Empty;
        }

        /// <summary>
        /// Builds <see cref="ChartFontOptions"/> based on current settings and theme.
        /// </summary>
        /// <param name="chartThemeStyle">The chart theme style.</param>
        /// <returns>A populated <see cref="ChartFontOptions"/> instance.</returns>
        internal ChartFontOptions GetChartFontOptions(ChartThemeStyle chartThemeStyle)
        {
            return new ChartFontOptions
            {
                Color = Color,
                Size = GetFontSize(chartThemeStyle),
                FontFamily = GetFontFamily(chartThemeStyle),
                FontWeight = GetFontWeight(chartThemeStyle),
                FontStyle = FontStyle,
                TextOverflow = TextOverflow
            };
        }

        /// <summary>
        /// Builds <see cref="FontOptions"/> based on current settings and theme.
        /// </summary>
        /// <param name="chartThemeStyle">The chart theme style.</param>
        /// <returns>A populated <see cref="FontOptions"/> instance.</returns>
        internal FontOptions GetFontOptions(ChartThemeStyle chartThemeStyle)
        {
            return new FontOptions
            {
                Color = Color,
                Size = GetFontSize(chartThemeStyle),
                FontFamily = GetFontFamily(chartThemeStyle),
                FontWeight = GetFontWeight(chartThemeStyle),
                FontStyle = FontStyle
            };
        }

        #endregion
    }
}