using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Options to customize the multilevel label text in a chart axis.
    /// </summary>
    /// <remarks>
    /// This component offers properties to style the text of multi-level labels, ensuring control over their visual presentation in chart axes.
    /// </remarks>
    public class ChartAxisMultiLevelLabelTextStyle : ChartDefaultFont
    {
        #region Fields

        private string _size = null!;
        private string _color = null!;
        private string _fontFamily = string.Empty;
        private string _fontStyle = string.Empty;
        private string _fontWeight = string.Empty;
        private double _opacity = 1;

        private Alignment _textAlignment = Alignment.Center;
        private TextOverflow _textOverflow = TextOverflow.Trim;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the cascading parent multi-level label component.
        /// </summary>
        [CascadingParameter]
        private ChartMultiLevelLabel? Parent { get; set; }

        /// <summary>
        /// Gets the owner chart component.
        /// </summary>
        [CascadingParameter]
        internal SfChart? Owner { get; set; }

        /// <summary> 
        /// Gets or sets the color for the multi-level label text. 
        /// </summary> 
        /// <value> 
        /// A string representing the color of the multi-level label text. The default multi-level label text color is determined by the chart's theme. By default, the theme is set to Fluent with a font color of <b>rgba(97, 97, 97, 1)</b>. 
        /// </value> 
        /// <remarks> 
        /// Use valid hex or rgba CSS color strings for the color value. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to customize the text color of a multi-level label in the primary X-axis of a Chart.
        /// <SfChart>
        ///     <ChartPrimaryXAxis>
        ///         <ChartMultiLevelLabels>
        ///             <ChartMultiLevelLabel>
        ///                 <ChartAxisMultiLevelLabelTextStyle Color="red" />
        ///                 <ChartCategories>
        ///                     <ChartCategory Start="10" End="40" Text="Half yearly 1" />
        ///                 </ChartCategories>
        ///             </ChartMultiLevelLabel>
        ///         </ChartMultiLevelLabels>
        ///     </ChartPrimaryXAxis>
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
        /// Gets or sets the font size for the multi-level label text. 
        /// </summary> 
        /// <value> 
        /// A string representing the font size of the multi-level label text. The default value is <b>"12px"</b>. 
        /// </value>
        /// <remarks>
        /// This property allows customization of the font size, promoting consistency with the general design of the chart.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to customize the text size of a multi-level label in the primary X-axis of a Chart.
        /// <SfChart>
        ///     <ChartPrimaryXAxis>
        ///         <ChartMultiLevelLabels>
        ///             <ChartMultiLevelLabel>
        ///                 <ChartAxisMultiLevelLabelTextStyle Size="15px" />
        ///                 <ChartCategories>
        ///                     <ChartCategory Start="10" End="40" Text="Half yearly 1" />
        ///                 </ChartCategories>
        ///             </ChartMultiLevelLabel>
        ///         </ChartMultiLevelLabels>
        ///     </ChartPrimaryXAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override string Size { get; set; } = "12px";

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Performs initialization when the component is first rendered.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Tracker is ChartMultiLevelLabel multiLevelLabel)
            {
                Parent = multiLevelLabel;
            }
            Parent?.UpdateMultiLevelLabelProperties("TextStyle", this);
        }

        /// <summary>
        /// Handles parameter changes and triggers a chart refresh if necessary.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (Color != _color || Size != _size || FontFamily != _fontFamily || FontStyle != _fontStyle || FontWeight != _fontWeight || Opacity != _opacity || TextAlignment != _textAlignment || TextOverflow != _textOverflow)
            {
                _color = Color;
                _size = Size;
                _fontFamily = FontFamily;
                _fontStyle = FontStyle;
                _fontWeight = FontWeight;
                _opacity = Opacity;
                _textAlignment = TextAlignment;
                _textOverflow = TextOverflow;
                if (Owner != null && Owner._isChartFirstRender)
                {
                    _ = Owner.RefreshChartAsync();
                }
            }
        }

        /// <summary>
        /// Releases resources associated with this component.
        /// </summary>
        protected override ValueTask DisposeAsyncCore()
        {
            Parent = null;
            ChildContent = null!;
            return base.DisposeAsyncCore();
        }

        #endregion
    }
}
