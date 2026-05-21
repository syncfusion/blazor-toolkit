using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Defines a region (segment) that helps differentiate a line-type series, typically used by multi-colored series.
    /// </summary>
    public class ChartSegment : ChartSubComponent
    {
        #region Fields

        private bool _isPropertyChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the associated series via cascading parameters.
        /// </summary>
        /// <value>The owning <see cref="ChartSeries"/> instance.</value>
        [CascadingParameter]
        private ChartSeries? Series { get; set; }

        /// <summary>
        /// Gets or sets the parent segment collection via cascading parameters.
        /// </summary>
        /// <value>The parent <see cref="ChartSegments"/> collection.</value>
        [CascadingParameter]
        private ChartSegments? Parent { get; set; }

        /// <summary>
        /// Gets or sets the value of the segment series.
        /// </summary>
        /// <value>
        /// An object representing the value associated with the segment within the chart series.
        /// </value>
        /// <remarks>
        /// This property allows you to assign or retrieve data values specific to this segment, which can affect
        /// the visual representation and data interpretation of the series.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to render a MultiColoredArea chart with a custom segment value pattern.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" Type="ChartSeriesType.MultiColoredArea">
        ///             <ChartSegments>
        ///                 <ChartSegment Value="30" Color="blue" />
        ///             </ChartSegments>
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        private object _value = null!;
        [Parameter]
        public object Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    _isPropertyChanged = Series is not null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the color of the segment series.
        /// </summary>
        /// <value>
        /// A string representing the color of the segment, used to visually distinguish it from other segments in the chart.
        /// </value>
        /// <remarks>
        /// By adjusting this property, you can enhance the visual presentation of the chart by applying specific color codes
        /// to different segments.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to render a MultiColoredArea chart with a custom segment color.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" Type="ChartSeriesType.MultiColoredArea">
        ///             <ChartSegments>
        ///                 <ChartSegment Value="30" Color="blue" DashArray="4,3" />
        ///             </ChartSegments>
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        private string _color = null!;
        [Parameter]
        public string Color
        {
            get => _color;
            set
            {
                if (_color != value)
                {
                    _color = value;
                    _isPropertyChanged = Series is not null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the dash array of the segment series.
        /// </summary>
        /// <value>
        /// A string that defines the pattern of dashes and gaps used to outline the segment in the series.
        /// </value>
        /// <remarks>
        /// This property provides control over the line style of the chart segment, enabling customization of patterns
        /// such as dotted or dashed lines for stylistic variations.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to render a MultiColoredArea chart with a custom segment dash pattern.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" Type="ChartSeriesType.MultiColoredArea">
        ///             <ChartSegments>
        ///                 <ChartSegment Value="30" Color="blue" DashArray="4,3" />
        ///             </ChartSegments>
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        private string _dashArray = null!;
        [Parameter]
        public string DashArray
        {
            get => _dashArray;
            set
            {
                if (_dashArray != value)
                {
                    _dashArray = value;
                    _isPropertyChanged = Series is not null;
                }
            }
        }

        #endregion

        #region Lifecycle Methods

        /// <exclude />
        /// <summary>
        /// Called by the framework to initialize the component. Registers this segment with its parent collection.
        /// </summary>      
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Tracker is ChartSegments segments)
            {
                Parent = segments;
            }

            Parent?._segments?.Add(this);
        }

        /// <exclude />
        /// <summary>
        /// Called by the framework when component parameters are set. If any segment property changed,
        /// schedules a renderer update to refresh the visual path and render queue.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (_isPropertyChanged)
            {
                _isPropertyChanged = false;
                Series?.Renderer?.UpdateDirection();
                Series?.Renderer?.ProcessRenderQueue();
            }
        }

        /// <summary>
        /// Disposes the component and clears references to parent and series.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override ValueTask DisposeAsyncCore()
        {
            Series = null;
            Parent = null;
            return base.DisposeAsyncCore();
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Sets the segment value and color.
        /// </summary>
        /// <param name="segmentValue">The threshold or segment value that determines the segment region.</param>
        /// <param name="color">The color to apply to the segment.</param>
        internal void SetSegmentValue(object segmentValue, string color)
        {
            Value = segmentValue;
            Color = color;
        }

        #endregion
    }
}
