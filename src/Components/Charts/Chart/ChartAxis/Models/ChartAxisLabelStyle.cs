using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Charts.Internal;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides options to customize the text style of axis labels in a chart.
    /// </summary>
    /// <remarks>
    /// This class inherits from <see cref="ChartDefaultFont"/> and allows you to set font properties 
    /// such as size, color, family, weight, and style for axis labels. Changes are automatically 
    /// propagated to the parent chart axis when the component is rendered.
    /// </remarks>
    public class ChartAxisLabelStyle : ChartDefaultFont
    {
        #region Fields
        private string _size = null!;
        private string _color = null!;
        private string _fontFamily = string.Empty;
        private string _fontWeight = string.Empty;
        private string _fontStyle = "Normal";

        internal bool _isPropertyChanged;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the parent <see cref="ChartAxis"/> component that owns this style configuration.
        /// </summary>
        /// <value>
        /// A reference to the parent <see cref="ChartAxis"/> component, or <see langword="null"/> if not set.
        /// </value>
        [CascadingParameter]
        private ChartAxis? Axis { get; set; }

        /// <summary>  
        /// Gets or sets the font size for axis labels.  
        /// </summary>  
        /// <value>  
        /// A string representing the font size for axis labels. The default font size for the axis labels is <b>12px</b>.
        /// </value>  
        /// <remarks>  
        /// This property allows you to specify the font size for rendering axis labels.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a custom font size for the axis.
        /// <SfChart>
        ///     <ChartPrimaryXAxis>
        ///         <ChartAxisLabelStyle Size="20px" />
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis>
        ///         <ChartAxisLabelStyle Size="20px" />
        ///     </ChartPrimaryYAxis>
        ///     <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override string Size { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets the font color for axis labels. 
        /// </summary> 
        /// <value> 
        /// A string representing the font color for axis labels. The default font color is determined by the chart's theme. By default, the theme is set to Fluent with a font color of <b>rgba(97, 97, 97, 1)</b>. 
        /// </value>
        /// <remarks> 
        /// Use valid hex or rgba CSS color strings for the color value. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a custom font color for the axis.
        /// <SfChart>
        ///     <ChartPrimaryXAxis>
        ///         <ChartAxisLabelStyle Color="red" />
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis>
        ///         <ChartAxisLabelStyle Color="red" />
        ///     </ChartPrimaryYAxis>
        ///     <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override string Color { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets the font family for axis labels. 
        /// </summary> 
        /// <value> 
        /// A string representing the font family for axis labels. The default font family is determined by the chart's theme. By default, the theme is set to Fluent with the font family <b>Roboto</b>.
        /// </value>
        /// <remarks>
        /// This property is used to specify the desired font family for axis labels, allowing for a consistent styling of text across your application. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a custom font family for the axis.
        /// <SfChart>
        ///     <ChartPrimaryXAxis>
        ///         <ChartAxisLabelStyle FontFamily="italic" />
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis>
        ///         <ChartAxisLabelStyle FontFamily="italic" />
        ///     </ChartPrimaryYAxis>
        ///     <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override string FontFamily { get; set; } = string.Empty;

        /// <summary>  
        /// Gets or sets the font weight for axis labels.  
        /// </summary>  
        /// <value>  
        /// A string representing the font weight for axis labels. The default font weight is determined by the chart's theme. By default, the theme is set to Fluent with a font weight of <b>400</b>.
        /// </value>  
        /// <remarks>  
        /// This property allows you to specify the thickness or boldness of the font used for rendering axis labels.  
        /// Common values include "Normal", "Bold", "Bolder", "Lighter", or numeric values such as 100, 200, ..., 900. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a custom font weight for the axis.
        /// <SfChart>
        ///     <ChartPrimaryXAxis>
        ///         <ChartAxisLabelStyle FontWeight="500" />
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis>
        ///         <ChartAxisLabelStyle FontWeight="500" />
        ///     </ChartPrimaryYAxis>
        ///     <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override string FontWeight { get; set; } = string.Empty;

        #endregion

        #region LifeCycle Methods

        /// <summary>
        /// Initializes the component and registers it with the parent axis.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Tracker is ChartAxis chartAxis)
            {
                Axis = chartAxis;
            }
            Axis?.UpdateAxisProperties("LabelStyle", this);
        }

        /// <summary>
        /// Handles parameter changes and updates the axis if properties have changed.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            _isPropertyChanged = _fontFamily != FontFamily || _fontWeight != FontWeight || _fontStyle != FontStyle || _size != Size;

            if (Axis?.Container is not null && Axis.Container.IsRendered && (_isPropertyChanged || _color != Color))
            {
                _size = Size;
                _color = Color;
                _fontFamily = FontFamily;
                _fontWeight = FontWeight;
                _fontStyle = FontStyle;
                Axis?.UpdateAxisProperties("LabelStyle", this);
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Retrieves the effective font size, using the specified size or falling back to the theme default.
        /// </summary>
        /// <param name="chartThemeStyle">The chart theme style containing default font settings.</param>
        /// <returns>The font size string (e.g., "12px"), or an empty string if unavailable.</returns>
        internal string GetFontSize(ChartThemeStyle chartThemeStyle)
        {
            return !string.IsNullOrEmpty(Size) ? Size : chartThemeStyle?.AxisLabelFontSize ?? string.Empty;
        }

        /// <summary>
        /// Retrieves the effective font weight, using the specified weight or falling back to the theme default.
        /// </summary>
        /// <param name="chartThemeStyle">The chart theme style containing default font settings.</param>
        /// <returns>The font weight string (e.g., "400", "bold"), or an empty string if unavailable.</returns>
        internal string GetFontWeight(ChartThemeStyle chartThemeStyle)
        {
            return !string.IsNullOrEmpty(FontWeight) ? FontWeight : chartThemeStyle?.AxisLabelFontWeight ?? string.Empty;
        }

        /// <summary>
        /// Retrieves the effective font family, using the specified family or falling back to the theme default.
        /// </summary>
        /// <param name="chartThemeStyle">The chart theme style containing default font settings.</param>
        /// <returns>The font family string (e.g., "Roboto", "Arial"), or an empty string if unavailable.</returns>
        internal string GetFontFamily(ChartThemeStyle chartThemeStyle)
        {
            return !string.IsNullOrEmpty(FontFamily) ? FontFamily : chartThemeStyle?.AxisLabelFontFamily ?? string.Empty;
        }

        /// <summary>
        /// Creates a <see cref="ChartFontOptions"/> object with the current label style settings.
        /// </summary>
        /// <param name="chartThemeStyle">The chart theme style containing default font settings.</param>
        /// <returns>A <see cref="ChartFontOptions"/> object populated with resolved font properties.</returns>
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
        #endregion
    }
}
