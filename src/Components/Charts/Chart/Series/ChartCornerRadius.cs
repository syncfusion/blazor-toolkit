using Microsoft.AspNetCore.Components;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Specifies the configuration of the corner radius for the rectangle-type series.
    /// </summary>
    /// <remarks>
    /// Use this component within a <see cref="ChartSeries"/> to set per-corner roundness for rectangular shapes
    /// (e.g., <c>Column</c>, <c>Bar</c>, <c>RangeColumn</c>).
    /// When any value changes at runtime, the series renderer updates direction and re-queues rendering.
    /// </remarks>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// // This example demonstrates how to render a column chart with rounded corners.
    /// <SfChart>
    ///     <ChartSeriesCollection>
    ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" Type="ChartSeriesType.Column">
    ///             <ChartCornerRadius TopLeft="10" TopRight="10" BottomLeft="4" BottomRight="4" />
    ///         </ChartSeries>
    ///     </ChartSeriesCollection>
    /// </SfChart>
    /// ]]>
    /// </code>
    /// </example>
    public class ChartCornerRadius : ChartSubComponent
    {
        #region Fields

        private double _bottomLeft;
        private double _bottomRight;
        private double _topLeft;
        private double _topRight;
        private bool _isPropertyChanged;

        #endregion

        #region Properties

        [CascadingParameter]
        private ChartSeries? Series { get; set; }

        /// <summary>
        /// Gets or sets the bottom-left corner radius for the chart series.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> representing the bottom-left corner radius of the series. The default is <b>0</b>.
        /// </value>
        /// <remarks>
        /// When this property value changes after initial render, the chart series updates its rendering.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // Rounded bottom-left corner only.
        /// <ChartCornerRadius BottomLeft="20" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double BottomLeft { get; set; }

        /// <summary>
        /// Gets or sets the bottom-right corner radius for the chart series.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> representing the bottom-right corner radius of the series. The default is <b>0</b>.
        /// </value>
        /// <remarks>
        /// When this property value changes after initial render, the chart series updates its rendering.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // Rounded bottom-right corner only.
        /// <ChartCornerRadius BottomRight="20" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double BottomRight { get; set; }

        /// <summary>
        /// Gets or sets the top-left corner radius for the chart series.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> representing the top-left corner radius of the series. The default is <b>0</b>.
        /// </value>
        /// <remarks>
        /// When this property value changes after initial render, the chart series updates its rendering.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // Rounded top-left corner only.
        /// <ChartCornerRadius TopLeft="20" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double TopLeft { get; set; }

        /// <summary>
        /// Gets or sets the top-right corner radius for the chart series.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> representing the top-right corner radius of the series. The default is <b>0</b>.
        /// </value>
        /// <remarks>
        /// When this property value changes after initial render, the chart series updates its rendering.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // Rounded top-right corner only.
        /// <ChartCornerRadius TopRight="20" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double TopRight { get; set; }

        #endregion

        #region Lifecycle Methods

        /// <exclude />
        /// <summary>
        /// Initializes the component and registers with the parent <see cref="ChartSeries"/>.
        /// </summary>      
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Tracker is ChartSeries chartSeries)
            {
                Series = chartSeries;
            }

            Series?.UpdateSeriesProperties("CornerRadius", this);
        }

        /// <exclude />
        /// <summary>
        /// Applies pending property changes by updating the series renderer and re-queuing render.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (_bottomLeft != BottomLeft)
            {
                _bottomLeft = BottomLeft;
                if (Series is not null) { _isPropertyChanged = true; }
            }

            if (_bottomRight != BottomRight)
            {
                _bottomRight = BottomRight;
                if (Series is not null) { _isPropertyChanged = true; }
            }

            if (_topLeft != TopLeft)
            {
                _topLeft = TopLeft;
                if (Series is not null) { _isPropertyChanged = true; }
            }

            if (_topRight != TopRight)
            {
                _topRight = TopRight;
                if (Series is not null) { _isPropertyChanged = true; }
            }

            if (_isPropertyChanged)
            {
                _isPropertyChanged = false;
                Series?.Renderer?.UpdateDirection();
                Series?.Renderer?.ProcessRenderQueue();
            }
        }

        /// <summary>
        /// Disposes component resources and breaks references to the parent series.
        /// </summary>
        /// <inheritdoc />
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override ValueTask DisposeAsyncCore()
        {
            Series = null;
            ChildContent = null!;
            return base.DisposeAsyncCore();
        }

        #endregion
    }
}
