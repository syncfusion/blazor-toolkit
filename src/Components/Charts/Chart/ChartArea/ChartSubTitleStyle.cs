using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Charts.Internal;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides options to customize the subtitle style of the chart.
    /// </summary>
    public class ChartSubTitleStyle : ChartDefaultFont
    {
        #region Properties

        /// <summary>
        /// Gets the parent chart component that owns this subtitle style.
        /// </summary>
        /// <value>The parent <see cref="SfChart"/> component, or <see langword="null"/> if not cascaded.</value>
        [CascadingParameter]
        internal SfChart? Owner { get; set; }

        /// <summary>
        /// Gets or sets the font size of the chart subtitle.
        /// </summary>
        /// <value>
        /// A string representing the font size of the chart subtitle. he default size is determined by the <see cref="SfChart">Chart's</see> theme. By default, the theme is set to Fluent with a subtitle text size of <b>14px</b>.
        /// </value>
        /// <remarks>
        /// Use the <see cref="Size"/> property to change the font size of subtitle text in the <see cref="SfChart">Chart</see>.
        /// This property accepts standard CSS font-size specifications, such as values in 'px'.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a custom font size for the subtitle text:
        /// <SfChart Title="Sales Data" SubTitle="2023 Overview">
        ///     <ChartSubTitleStyle Size="16px" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override string Size { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets the font weight of the chart subtitle. 
        /// </summary> 
        /// <value> 
        /// A string representing the font weight of the subtitle text. The default font weight is determined by the <see cref="SfChart">Chart's</see> theme. By default, the theme is set to Fluent with subtitle font weight of <b>400</b>.
        /// </value> 
        /// <remarks>
        /// The font weight can be a number (100 to 900), or a keyword such as 'normal', 'bold', 'bolder', or 'lighter'.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a custom font weight for the subtitle text:
        /// <SfChart Title="Sales Data" SubTitle="2023 Overview">
        ///     <ChartSubTitleStyle FontWeight="600" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override string FontWeight { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets the font family of the chart subtitle. 
        /// </summary> 
        /// <value>
        /// A string representing the font family of the subtitle text. The default font family is determined by the <see cref="SfChart">Chart's</see> theme. By default, the theme is set to Fluent with a subtitle font family of <b>Roboto</b>.
        /// </value>
        /// <remarks>
        /// The font family accepts typical CSS font family syntax, allowing customization to match the desired text style.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a custom font family for the subtitle text:
        /// <SfChart Title="Sales Data" SubTitle="2023 Overview">
        ///     <ChartSubTitleStyle FontFamily="italic" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override string FontFamily { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the accessibility description for the chart subtitle.
        /// </summary>
        /// <value>
        /// Accepts a string that defines the accessibility description for the chart subtitle. The default value is an empty string.
        /// </value>
        /// <remarks>
        /// Use this property to provide an accessibility description for screen readers and other assistive technologies.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting an accessibility description for the subtitle text:
        /// <SfChart Title="Sales Data" SubTitle="2023 Overview">
        ///     <ChartSubTitleStyle AccessibilityDescription="This is the subtitle of the chart" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string AccessibilityDescription { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the accessibility role for the chart subtitle.
        /// </summary>
        /// <value>
        /// Accepts a string that defines the accessibility role for the chart subtitle. The default value is <b>null</b>.
        /// </value>
        /// <remarks>
        /// Use this property to provide an accessibility role for the chart subtitle.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting an accessibility role for the subtitle text:
        /// <SfChart Title="Sales Data" SubTitle="2023 Overview">
        ///     <ChartSubTitleStyle AccessibilityRole="heading" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string AccessibilityRole { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the accessibility keyboard navigation focus option for the chart subtitle.
        /// </summary>
        /// <value>
        /// Accepts the boolean value to enable or disable the keyboard navigation for the chart subtitle. The default value is <b>true</b>.
        /// </value>
        /// <remarks>
        /// Use this property to toggle the keyboard navigation focus for the chart subtitle.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting the keyboard navigation focus for the chart subtitle:
        /// <SfChart Title="Sales Data" SubTitle="2023 Overview">
        ///     <ChartSubTitleStyle Focusable="false" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool Focusable { get; set; } = true;

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Initializes the component and sets up the subtitle style on the parent chart renderer.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Owner?._chartTitleRenderer is not null)
            {
                Owner._chartTitleRenderer.SubTitleStyle = this;
            }
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Retrieves the effective font size for the subtitle, falling back to the theme default if not set.
        /// </summary>
        /// <param name="chartThemeStyle">The chart's theme style settings.</param>
        /// <returns>The computed font size string.</returns>
        internal string GetFontSize(ChartThemeStyle chartThemeStyle)
        {
            return !string.IsNullOrEmpty(Size) ? Size : chartThemeStyle?.ChartSubTitleSize ?? string.Empty;
        }

        /// <summary>
        /// Retrieves the effective font weight for the subtitle, falling back to the theme default if not set.
        /// </summary>
        /// <param name="chartThemeStyle">The chart's theme style settings.</param>
        /// <returns>The computed font weight string.</returns>
        internal string GetFontWeight(ChartThemeStyle chartThemeStyle)
        {
            return !string.IsNullOrEmpty(FontWeight) ? FontWeight : chartThemeStyle?.ChartSubTitleFontWeight ?? string.Empty;
        }

        /// <summary>
        /// Retrieves the effective font family for the subtitle, falling back to the theme default if not set.
        /// </summary>
        /// <param name="chartThemeStyle">The chart's theme style settings.</param>
        /// <returns>The computed font family string.</returns>
        internal string GetFontFamily(ChartThemeStyle chartThemeStyle)
        {
            return !string.IsNullOrEmpty(FontFamily) ? FontFamily : chartThemeStyle?.ChartSubTitleFontFamily ?? string.Empty;
        }

        /// <summary>
        /// Compiles all font styling properties into a single <see cref="ChartFontOptions"/> object.
        /// </summary>
        /// <param name="chartThemeStyle">The chart's theme style settings.</param>
        /// <returns>A <see cref="ChartFontOptions"/> instance with computed values.</returns>
        internal ChartFontOptions GetFontOptions(ChartThemeStyle chartThemeStyle)
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
        #endregion
    }
}
