using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Charts.Internal;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides options to customize the text style of the axis title in a chart.
    /// </summary>
    public class ChartAxisTitleStyle : ChartDefaultFont
    {
        #region Fields
        private string _size = string.Empty;
        private string _color = string.Empty;
        private string _fontFamily = string.Empty;
        private string _fontWeight = string.Empty;
        private bool _isPropertyChanged;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the parent <see cref="ChartAxis"/> component that owns this axis title style configuration.
        /// </summary>
        /// <value>
        /// A reference to the parent <see cref="ChartAxis"/> component, or <see langword="null"/> if not set.
        /// </value>
        [CascadingParameter]
        private ChartAxis? Axis { get; set; }

        /// <summary> 
        /// Gets or sets the font size of the axis title.
        /// </summary> 
        /// <value> 
        /// A string representing the font size of the axis title. The default font size for the axis title is <b>14px</b>.
        /// </value>
        /// <remarks>
        /// Use this property to specify the font size for the axis title text.
        /// This can be set using a valid CSS font size string (e.g., "14px", "1em").
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a customized size for the axis title.
        /// <SfChart>
        ///     <ChartPrimaryXAxis Title="XAxis">
        ///         <ChartAxisTitleStyle Size="15px" />
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis Title="YAxis">
        ///         <ChartAxisTitleStyle Size="15px" />
        ///     </ChartPrimaryYAxis>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override string Size { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets the font family for the axis title. 
        /// </summary> 
        /// <value> 
        /// A string representing the font family for the axis title. The default font family is determined by the chart's theme. By default, the theme is set to Fluent with the font family <b>Roboto</b>.
        /// </value>
        /// <remarks>
        /// This property allows you to define a family of fonts to be used for the axis title.
        /// It can be set to any valid CSS font-family value, such as "Arial", "Times New Roman", or a generic font family like "serif" or "sans-serif".
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a customized font family for the axis title.
        /// <SfChart>
        ///     <ChartPrimaryXAxis Title="XAxis">
        ///         <ChartAxisTitleStyle FontFamily="italic" />
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis Title="YAxis">
        ///         <ChartAxisTitleStyle FontFamily="italic" />
        ///     </ChartPrimaryYAxis>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override string FontFamily { get; set; } = string.Empty;

        /// <summary>  
        /// Gets or sets the font weight for the axis title.  
        /// </summary>  
        /// <value>  
        /// A string representing the font weight for the axis title. The default font weight is determined by the chart's theme. By default, the theme is set to Fluent with a font weight of <b>600</b>.
        /// </value>
        /// <remarks>
        /// This property allows you to specify the thickness or boldness of the font used for rendering the axis title.
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a customized font weight for the axis title.
        /// <SfChart>
        ///     <ChartPrimaryXAxis Title="XAxis">
        ///         <ChartAxisTitleStyle FontWeight="600" />
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis Title="YAxis">
        ///         <ChartAxisTitleStyle FontWeight="600" />
        ///     </ChartPrimaryYAxis>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override string FontWeight { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets the font color for the axis title. 
        /// </summary> 
        /// <value> 
        /// A string representing the font color for the axis title. The default font color is determined by the chart's theme. By default, the theme is set to Fluent with a font color of <b>rgba(0, 0, 0, 1)</b>. 
        /// </value>
        /// <remarks> 
        /// Use valid hex or rgba CSS color strings for the color value. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a customized color for the axis title.
        /// <SfChart>
        ///     <ChartPrimaryXAxis Title="XAxis">
        ///         <ChartAxisTitleStyle Color="blue" />
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis Title="YAxis">
        ///         <ChartAxisTitleStyle Color="blue" />
        ///     </ChartPrimaryYAxis>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override string Color { get; set; } = string.Empty;

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Performs component initialization by registering with the parent <see cref="ChartAxis"/> instance.
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
            Axis?.UpdateAxisProperties("TitleStyle", this);
            _size = Size;
            _color = Color;
            _fontFamily = FontFamily;
            _fontWeight = FontWeight;
        }

        /// <summary>
        /// Handles parameter changes and notifies the parent axis of style updates.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(false);
            _isPropertyChanged = _size != Size || _color != Color || _fontFamily != FontFamily || _fontWeight != FontWeight;
            if (_isPropertyChanged)
            {
                _isPropertyChanged = false;
                _size = Size;
                _color = Color;
                _fontFamily = FontFamily;
                _fontWeight = FontWeight;
                Axis?.UpdateAxisProperties("TitleStyle", this);
            }
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Gets the font options for rendering the axis title based on the current style and theme.
        /// </summary>
        /// <param name="chartThemeStyle">
        /// The chart theme style containing default font values for fallback when properties are not explicitly set.
        /// </param>
        /// <returns>
        /// A <see cref="ChartFontOptions"/> object containing the resolved font properties (size, color, family, weight, style, overflow).
        /// </returns>
        internal ChartFontOptions GetChartFontOptions(ChartThemeStyle chartThemeStyle)
        {
            return new ChartFontOptions { Color = Color, Size = GetFontSize(chartThemeStyle), FontFamily = GetFontFamily(chartThemeStyle), FontWeight = GetFontWeight(chartThemeStyle), FontStyle = FontStyle, TextOverflow = TextOverflow };
        }

        /// <summary>
        /// Gets the resolved font size, preferring the explicitly set value over the theme default.
        /// </summary>
        /// <param name="chartThemeStyle">
        /// The chart theme style containing the default font size for fallback.
        /// </param>
        /// <returns>
        /// The font size string; either the explicitly set value or the theme's axis title font size.
        /// </returns>
        internal string GetFontSize(ChartThemeStyle chartThemeStyle)
        {
            return !string.IsNullOrEmpty(Size) ? Size : chartThemeStyle?.AxisTitleFontSize ?? string.Empty;
        }

        /// <summary>
        /// Gets the resolved font weight, preferring the explicitly set value over the theme default.
        /// </summary>
        /// <param name="chartThemeStyle">
        /// The chart theme style containing the default font weight for fallback.
        /// </param>
        /// <returns>
        /// The font weight string; either the explicitly set value or the theme's axis title font weight.
        /// </returns>
        internal string GetFontWeight(ChartThemeStyle chartThemeStyle)
        {
            return !string.IsNullOrEmpty(FontWeight) ? FontWeight : chartThemeStyle?.AxisTitleFontWeight ?? string.Empty;
        }

        /// <summary>
        /// Gets the resolved font family, preferring the explicitly set value over the theme default.
        /// </summary>
        /// <param name="chartThemeStyle">
        /// The chart theme style containing the default font family for fallback.
        /// </param>
        /// <returns>
        /// The font family string; either the explicitly set value or the theme's axis title font family.
        /// </returns>
        internal string GetFontFamily(ChartThemeStyle chartThemeStyle)
        {
            return !string.IsNullOrEmpty(FontFamily) ? FontFamily : chartThemeStyle?.AxisTitleFontFamily ?? string.Empty;
        }
        #endregion
    }
}
