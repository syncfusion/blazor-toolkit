using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Charts.Internal;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Represents the text style properties for legend text in a chart.
    /// </summary>
    public class ChartLegendTextStyle : ChartDefaultFont
    {
        #region Fields
        private string _size = string.Empty;
        private string _color = string.Empty;
        private string _fontFamily = string.Empty;
        private string _fontWeight = string.Empty;
        private string _fontStyle = "Normal";
        private double _opacity = 1;
        private TextOverflow _textOverflow = TextOverflow.Trim;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the chart component that owns this text style.
        /// </summary>
        /// <value>
        /// The parent <see cref="SfChart"/> component. This is automatically set via cascading parameters.
        /// </value>
        [CascadingParameter]
        internal SfChart? Owner { get; set; }

        /// <summary>
        /// Gets or sets the legend settings that contain this text style.
        /// </summary>
        /// <value>
        /// The <see cref="ChartLegendSettings"/> that manages this text style configuration.
        /// </value>
        internal ChartLegendSettings? ChartLegend { get; set; }

        /// <summary> 
        /// Gets or sets the font size for the legend text. 
        /// </summary> 
        /// <value> 
        /// A string representing the font size for the legend text. The default font size for the legend text is <b>14px</b>.
        /// </value>
        /// <remarks>
        /// This property allows customization of how large or small the legend's text should appear.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to customize the legend text size in a chart.
        /// <SfChart>
        ///     <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" />
        ///     <ChartLegendSettings Visible="true">
        ///         <ChartLegendTextStyle Size="14px" />
        ///     </ChartLegendSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override string Size { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets the font family for the legend text. 
        /// </summary> 
        /// <value> 
        /// A string representing the font family for the legend text. The default font family is determined by the chart's theme. By default, the theme is set to Fluent with the font family <b>Roboto</b>.
        /// </value>
        /// <remarks>
        /// This property enables setting a specific font style for the legend text, 
        /// allowing for better visual harmony with other theme elements.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to customize the legend text font family in a chart.
        /// <SfChart>
        ///     <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" />
        ///     <ChartLegendSettings Visible="true">
        ///         <ChartLegendTextStyle FontFamily="Roboto"  />
        ///     </ChartLegendSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override string FontFamily { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets the font weight for the legend text. 
        /// </summary> 
        /// <value> 
        /// A string representing the font weight for the legend text. The default font weight is determined by the chart's theme. By default, the theme is set to Fluent with a font weight of <b>400</b>.
        /// </value>  
        /// <remarks>
        /// Use this property to control the boldness or thinness of the legend text, 
        /// which can affect readability and emphasis.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to customize the legend text font weight in a chart.
        /// <SfChart>
        ///     <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" />
        ///     <ChartLegendSettings Visible="true">
        ///         <ChartLegendTextStyle FontWeight="400"  />
        ///     </ChartLegendSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override string FontWeight { get; set; } = string.Empty;

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Executes during component initialization.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();
            ChartLegend = (ChartLegendSettings)Tracker!;
            ChartLegend!.UpdateLegendProperties("TextStyle", this);
        }

        /// <summary>
        /// Executes when parameters are set or updated.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            Owner?._legendRenderer?.ProcessRenderQueue();
            ChartLegend?.UpdateLegendProperties("TextStyle", this);
            bool isFontUpdated = _fontFamily != FontFamily || _fontWeight != FontWeight || _fontStyle != FontStyle;

            if (Owner?.IsRendered == true && (_size != Size || _color != Color || isFontUpdated || _opacity != Opacity || TextAlignment != TextAlignment || _textOverflow != TextOverflow))
            {
                _size = Size;
                _color = Color;
                _fontFamily = FontFamily;
                _fontWeight = FontWeight;
                _fontStyle = FontStyle;
                _opacity = Opacity;
                _textOverflow = TextOverflow;
                ChartLegend?.LegendPropertyChanged(isFontUpdated);
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Gets the font size from the current style or theme default.
        /// </summary>
        /// <param name="chartThemeStyle">The chart theme style providing default values.</param>
        /// <returns>The font size string to apply.</returns>
        internal string GetFontSize(ChartThemeStyle chartThemeStyle)
        {
            return !string.IsNullOrEmpty(Size) ? Size : chartThemeStyle?.LegendTextSize ?? string.Empty;
        }

        /// <summary>
        /// Gets the font weight from the current style or theme default.
        /// </summary>
        /// <param name="chartThemeStyle">The chart theme style providing default values.</param>
        /// <returns>The font weight string to apply.</returns>
        internal string GetFontWeight(ChartThemeStyle chartThemeStyle)
        {
            return !string.IsNullOrEmpty(FontWeight) ? FontWeight : chartThemeStyle?.LegendFontWeight ?? string.Empty;
        }

        /// <summary>
        /// Gets the font family from the current style or theme default.
        /// </summary>
        /// <param name="chartThemeStyle">The chart theme style providing default values.</param>
        /// <returns>The font family string to apply.</returns>
        internal string GetFontFamily(ChartThemeStyle chartThemeStyle)
        {
            return !string.IsNullOrEmpty(FontFamily) ? FontFamily : chartThemeStyle?.LegendFontFamily ?? string.Empty;
        }

        /// <summary>
        /// Compiles the current text style properties into a <see cref="ChartFontOptions"/> object.
        /// </summary>
        /// <param name="chartThemeStyle">The chart theme style providing default values.</param>
        /// <returns>A <see cref="ChartFontOptions"/> containing the computed font properties.</returns>
        internal ChartFontOptions GetChartFontOptions(ChartThemeStyle chartThemeStyle)
        {
            return new ChartFontOptions { Color = Color, Size = GetFontSize(chartThemeStyle), FontFamily = GetFontFamily(chartThemeStyle), FontWeight = GetFontWeight(chartThemeStyle), FontStyle = FontStyle, TextOverflow = TextOverflow };
        }

        #endregion
    }
}
