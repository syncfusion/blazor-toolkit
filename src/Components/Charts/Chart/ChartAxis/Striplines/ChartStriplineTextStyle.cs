using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Charts.Internal;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Specifies the configuration of text style for the stripline text.
    /// </summary>
    public class ChartStriplineTextStyle : ChartDefaultFont
    {
        #region Fields
        [CascadingParameter]
        private ChartStripline? Parent { get; set; }
        #endregion

        #region Properties

        /// <summary> 
        /// Gets or sets the font size for the stripline text. 
        /// </summary> 
        /// <value> 
        /// A string representing the font size for the stripline text. The default font size for the stripline text is <b>12px</b>.
        /// </value> 
        /// <remarks>
        /// The font size can be adjusted to ensure that the strip line text is readable and fits well within the visual design of the chart.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to customize the text size of a stripline label on the primary Y-axis of a Chart.
        /// <SfChart>
        ///     <ChartPrimaryYAxis>
        ///         <ChartStriplines>
        ///             <ChartStripline Start="18" End="25" Text="Strip-Line">
        ///                 <ChartStriplineTextStyle Size="16px"></ChartStriplineTextStyle>
        ///             </ChartStripline>
        ///         </ChartStriplines>
        ///     </ChartPrimaryYAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override string Size { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets the font color for the stripline text. 
        /// </summary> 
        /// <value> 
        /// A string representing the font color for the stripline text. The default font color is determined by the chart's theme. By default, the theme is set to Fluent with a font color of <b>rgba(158, 158, 158, 1)</b>.
        /// </value> 
        /// <remarks>  
        /// Use valid hex or rgba CSS color strings for the color value.  
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to customize the text color of a stripline label on the primary Y-axis of a Chart.
        /// <SfChart>
        ///     <ChartPrimaryYAxis>
        ///         <ChartStriplines>
        ///             <ChartStripline Start="18" End="25" Text="Strip-Line">
        ///                 <ChartStriplineTextStyle Color="blue" />
        ///             </ChartStripline>
        ///         </ChartStriplines>
        ///     </ChartPrimaryYAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override string Color { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets the font family for the stripline text. 
        /// </summary> 
        /// <value> 
        /// A string representing the font family for the stripline text. The default font family is determined by the chart's theme. By default, the theme is set to Fluent with a font family of <b>Roboto</b>.
        /// </value>
        /// <remarks>
        /// Changing the font family can help in achieving a specific visual style or maintain consistency with other interface elements.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to customize the font family of a stripline label on the primary Y-axis of a Chart.
        /// <SfChart>
        ///     <ChartPrimaryYAxis>
        ///         <ChartStriplines>
        ///             <ChartStripline Start="18" End="25" Text="Strip-Line">
        ///                 <ChartStriplineTextStyle FontFamily="italic" />
        ///             </ChartStripline>
        ///         </ChartStriplines>
        ///     </ChartPrimaryYAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override string FontFamily { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets the font weight for the stripline text. 
        /// </summary> 
        /// <value> 
        /// A string representing the font weight for the stripline text. The default font weight is determined by the chart's theme. By default, the theme is set to Fluent with a font weight of <b>400</b>.
        /// </value>
        /// <remarks>
        /// Adjusting the font weight can help emphasize or de-emphasize the strip line text by making it bolder or lighter.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to customize the font weight of a stripline label on the primary Y-axis of a Chart.
        /// <SfChart>
        ///     <ChartPrimaryYAxis>
        ///         <ChartStriplines>
        ///             <ChartStripline Start="18" End="25" Text="Strip-Line">
        ///                 <ChartStriplineTextStyle FontWeight="700" />
        ///             </ChartStripline>
        ///         </ChartStriplines>
        ///     </ChartPrimaryYAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override string FontWeight { get; set; } = string.Empty;

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Initializes the stripline text style component and registers it with the parent stripline.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Tracker is ChartStripline chartStripline)
            {
                Parent = chartStripline;
            }
            Parent?.SetTextStyleValue(this);
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Gets the effective font size for the stripline text, falling back to theme default if not specified.
        /// </summary>
        /// <param name="chartThemeStyle">The chart theme style providing default values.</param>
        /// <returns>The font size as a string, or empty if not available.</returns>
        internal string GetFontSize(ChartThemeStyle chartThemeStyle)
        {
            return !string.IsNullOrEmpty(Size) ? Size : chartThemeStyle?.StriplineFontSize ?? string.Empty;
        }

        /// <summary>
        /// Gets the effective font weight for the stripline text, falling back to theme default if not specified.
        /// </summary>
        /// <param name="chartThemeStyle">The chart theme style providing default values.</param>
        /// <returns>The font weight as a string, or empty if not available.</returns>
        internal string GetFontWeight(ChartThemeStyle chartThemeStyle)
        {
            return !string.IsNullOrEmpty(FontWeight) ? FontWeight : chartThemeStyle?.StriplineFontWeight ?? string.Empty;
        }

        /// <summary>
        /// Gets the effective font family for the stripline text, falling back to theme default if not specified.
        /// </summary>
        /// <param name="chartThemeStyle">The chart theme style providing default values.</param>
        /// <returns>The font family as a string, or empty if not available.</returns>
        internal string GetFontFamily(ChartThemeStyle chartThemeStyle)
        {
            return !string.IsNullOrEmpty(FontFamily) ? FontFamily : chartThemeStyle?.StriplineFontFamily ?? string.Empty;
        }

        /// <summary>
        /// Builds a <see cref="ChartFontOptions"/> object from the current text style and theme defaults.
        /// </summary>
        /// <param name="chartThemeStyle">The chart theme style providing default values.</param>
        /// <returns>A <see cref="ChartFontOptions"/> object containing resolved font properties.</returns>
        internal ChartFontOptions GetFontOptions(ChartThemeStyle chartThemeStyle)
        {
            return new ChartFontOptions { Color = Color, Size = GetFontSize(chartThemeStyle), FontFamily = GetFontFamily(chartThemeStyle), FontWeight = GetFontWeight(chartThemeStyle), FontStyle = FontStyle };
        }
        #endregion
    }
}
