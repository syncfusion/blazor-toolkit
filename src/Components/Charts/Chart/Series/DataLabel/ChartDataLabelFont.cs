using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Charts.Internal;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provide options to customize the text style of datalabel.
    /// </summary>
    public class ChartDataLabelFont : ChartDefaultFont
    {
        #region Fields

        private ChartDataLabel? _dataLabel;
        private string? _size;
        private string? _color;
        private string? _fontFamily;
        private string? _fontWeight;
        private string? _fontStyle;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the size of the data label text in the <see cref="SfChart"/>.
        /// </summary>
        /// <value>
        /// A string representing the font size of the datalabel text. The default size is determined by the <see cref="SfChart">Chart's</see> theme. By default, the theme is set to Fluent with a datalabel text size of <b>12px</b>.
        /// </value>
        /// <remarks>
        /// Use the <see cref="Size"/> property to change the font size of data labels in the <see cref="SfChart">Chart</see> series.
        /// The size value accepts standard CSS font size specifications such as 'px', 'em', etc.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following example demonstrates how to set a custom font size for data label text:
        /// <SfChart>
        ///     <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y">
        ///         <ChartMarker>
        ///             <ChartDataLabel Visible="true">
        ///                 <ChartDataLabelFont Size="20px"></ChartDataLabelFont>
        ///             </ChartDataLabel>
        ///         </ChartMarker>
        ///     </ChartSeries>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override string Size { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the color of the data label text in the <see cref="SfChart"/>.
        /// </summary>
        /// <value>
        /// Accepts valid CSS color string values, including hexadecimal, rgba, and color names. The default color is determined by the <see cref="SfChart">Chart's</see> theme. By default, the theme is set to Fluent with a datalabel text color of <b>black</b>.
        /// </value>
        /// <remarks>
        /// Use the <see cref="Color"/> property to change the font color of data labels in the <see cref="SfChart">Chart</see> series.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following example demonstrates how to set a custom color for data label text:
        /// <SfChart>
        ///     <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y">
        ///         <ChartMarker>
        ///              <ChartDataLabel Visible="true">
        ///                 <ChartDataLabelFont Color="#c0faf4"></ChartDataLabelFont>
        ///             </ChartDataLabel>
        ///         </ChartMarker>
        ///     </ChartSeries>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override string Color { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the font family of the datalabel text in the <see cref="SfChart">Chart</see>.
        /// </summary>
        /// <value>
        /// A string representing the font family of the datalabel text. The default font family is determined by the <see cref="SfChart">Chart's</see> theme. By default, the theme is set to Fluent with datalabel font family <b>Roboto</b>.
        /// </value>
        /// <remarks>
        /// The font family accepts typical CSS font family syntax, allowing customization to match the desired text style.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following example demonstrates how to set a custom font family for data label text:
        /// <SfChart>
        ///     <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y">
        ///         <ChartMarker>
        ///             <ChartDataLabel Visible="true">
        ///                 <ChartDataLabelFont FontFamily="Arial"></ChartDataLabelFont>
        ///             </ChartDataLabel>
        ///         </ChartMarker>
        ///     </ChartSeries>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override string FontFamily { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the font weight of the datalabel text in the <see cref="SfChart">Chart</see>.
        /// </summary>
        /// <value>
        /// A string representing the font weight of the datalabel text. The default font weight is determined by the <see cref="SfChart">Chart's</see> theme. By default, the theme is set to Fluent with datalabel font weight of <b>400</b>.
        /// </value>
        /// <remarks>
        /// The font weight can be a number (100 to 900), or a keyword such as 'normal', 'bold', 'bolder', or 'lighter'.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following example demonstrates how to set a custom font weight for data label text:
        /// <SfChart>
        ///     <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y">
        ///         <ChartMarker>
        ///             <ChartDataLabel Visible="true">
        ///                 <ChartDataLabelFont FontWeight="bold"></ChartDataLabelFont>
        ///             </ChartDataLabel>
        ///         </ChartMarker>
        ///     </ChartSeries>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override string FontWeight { get; set; } = string.Empty;

        #endregion

        #region Lifecycle Methods

        /// <exclude />
        /// <summary>
        /// Initializes the font configuration and registers it with the parent <see cref="ChartDataLabel"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Tracker is ChartDataLabel chartDataLabel)
            {
                _dataLabel = chartDataLabel;
            }

            _dataLabel?.UpdateDatalabelProperties(nameof(ChartDataLabel.Font), this);
        }

        /// <exclude />
        /// <summary>
        /// Applies parameter updates and triggers label refresh when font values change.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            _dataLabel?.UpdateDatalabelProperties(nameof(ChartDataLabel.Font), this);

            if (_size != Size || _color != Color || _fontFamily != FontFamily ||
                _fontWeight != FontWeight || _fontStyle != FontStyle)
            {
                _size = Size;
                _color = Color;
                _fontFamily = FontFamily;
                _fontWeight = FontWeight;
                _fontStyle = FontStyle;

                _dataLabel?.Renderer?.DatalabelValueChanged();
            }
        }

        /// <summary>
        /// Disposes the font component and clears references.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override ValueTask DisposeAsyncCore()
        {
            ComponentDispose();
            return base.DisposeAsyncCore();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Disposes the font component and clears references.
        /// </summary>
        internal void ComponentDispose()
        {
            _dataLabel = null;
            ChildContent = null!;
        }
        /// <summary>
        /// Produces a resolved <see cref="ChartDefaultFont"/> using theme fallbacks.
        /// </summary>
        /// <param name="chartThemeStyle">The theme style to resolve defaults from.</param>
        /// <returns>Resolved <see cref="ChartDefaultFont"/> values.</returns>
        internal ChartDefaultFont GetChartDefaultFont(ChartThemeStyle chartThemeStyle)
        {
            return new ChartDefaultFont
            {
                Color = Color,
                FontFamily = GetFontFamily(chartThemeStyle),
                FontStyle = FontStyle,
                FontWeight = GetFontWeight(chartThemeStyle),
                Opacity = Opacity,
                Size = GetFontSize(chartThemeStyle),
                TextAlignment = TextAlignment,
                TextOverflow = TextOverflow
            };
        }

        /// <summary>
        /// Resolves the font size using the theme when not provided.
        /// </summary>
        /// <param name="chartThemeStyle">Theme style.</param>
        /// <returns>Resolved size string.</returns>
        internal string GetFontSize(ChartThemeStyle chartThemeStyle)
        {
            return !string.IsNullOrEmpty(Size) ? Size : chartThemeStyle?.DataLabelSize ?? string.Empty;
        }

        /// <summary>
        /// Resolves the font weight using the theme when not provided.
        /// </summary>
        /// <param name="chartThemeStyle">Theme style.</param>
        /// <returns>Resolved weight string.</returns>
        internal string GetFontWeight(ChartThemeStyle chartThemeStyle)
        {
            return !string.IsNullOrEmpty(FontWeight) ? FontWeight : chartThemeStyle?.DataLabelFontWeight ?? string.Empty;
        }

        /// <summary>
        /// Resolves the font family using the theme when not provided.
        /// </summary>
        /// <param name="chartThemeStyle">Theme style.</param>
        /// <returns>Resolved family string.</returns>
        internal string GetFontFamily(ChartThemeStyle chartThemeStyle)
        {
            return !string.IsNullOrEmpty(FontFamily) ? FontFamily : chartThemeStyle?.DataLabelFontFamily ?? string.Empty;
        }

        /// <summary>
        /// Creates simplified font options using theme fallbacks.
        /// </summary>
        /// <param name="chartThemeStyle">Theme style.</param>
        /// <returns><see cref="ChartFontOptions"/> with resolved values.</returns>
        internal ChartFontOptions GetFontOptions(ChartThemeStyle chartThemeStyle)
        {
            return new ChartFontOptions
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
