using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Charts.Internal;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Represents the options for configuring a linear gradient used to paint chart elements in the chart component.
    /// </summary>
    /// <remarks>
    /// Use <see cref="LinearGradient"/> to define a directional color blend that can be applied to chart visuals such as series, markers and legend symbols.
    ///
    /// A linear gradient typically consists of a direction (for example, from top to bottom or left to right) and one or more gradient stops
    /// that specify colors at given offsets. After defining the gradient, reference it from a paintable property (for example, <c>Fill</c>)
    /// using the syntax <c>url(#gradientId)</c>.
    /// </remarks>
    public class LinearGradient : ChartSubComponent, IChartElement
    {
        #region Fields

        private double _gradientStartX;
        private double _gradientStartY;
        private double _gradientEndX;
        private double _gradientEndY;

        private List<ChartGradientColorStop> _gradientColorStops = [];

        private ChartGradientRenderer? _renderer;

        #endregion

        #region Properties

        /// <inheritdoc />
        string IChartElement.RendererKey { get; set; } = null!;

        /// <summary>
        /// Gets the unique identifier of the gradient definition.
        /// </summary>
        /// <value>A unique string assigned by the chart runtime, if any.</value>
        internal string? ID { get; set; }

        /// <summary>
        /// Gets or sets the renderer that paints this gradient.
        /// </summary>
        /// <value>The active <see cref="ChartGradientRenderer"/> instance.</value>
        internal ChartGradientRenderer? Renderer { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public Type RendererType { get; set; } = null!;

        /// <summary>
        /// Gets or sets the x-coordinate of the starting point of the linear gradient.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> value representing the start x-coordinate. The default value is <c>0</c>.
        /// </value>
        /// <remarks>
        /// Defines the origin on the x-axis for the gradient direction. Values are commonly specified in the normalized range <c>0</c> to <c>1</c>.
        /// </remarks>
        [Parameter]
        public double X1 { get; set; }

        /// <summary>
        /// Gets or sets the y-coordinate of the starting point of the linear gradient.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> value representing the start y-coordinate. The default value is <c>0</c>.
        /// </value>
        /// <remarks>
        /// Defines the origin on the y-axis for the gradient direction. Values are commonly specified in the normalized range <c>0</c> to <c>1</c>.
        /// </remarks>
        [Parameter]
        public double Y1 { get; set; }

        /// <summary>
        /// Gets or sets the x-coordinate of the endpoint for the linear gradient.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> value representing the end x-coordinate. The default value is <c>0</c>.
        /// </value>
        /// <remarks>
        /// Determines the end position on the x-axis to control the gradient direction. Values are commonly specified in the normalized range <c>0</c> to <c>1</c>.
        /// </remarks>
        [Parameter]
        public double X2 { get; set; }

        /// <summary>
        /// Gets or sets the y-coordinate of the endpoint for the linear gradient.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> value representing the end y-coordinate. The default value is <c>0</c>.
        /// </value>
        /// <remarks>
        /// Determines the end position on the y-axis to control the gradient direction. Values are commonly specified in the normalized range <c>0</c> to <c>1</c>.
        /// </remarks>
        [Parameter]
        public double Y2 { get; set; }

        /// <summary>
        /// Represents the collection of color stops for the gradient fill of a chart <see cref="ChartSeries"/>.
        /// These stops determine how the colors blend across the gradient.
        /// </summary>
        /// <remarks>
        /// Use <see cref="ChartGradientColorStops"/> as a child of <see cref="ChartLinearGradient"/> or <see cref="ChartRadialGradient"/> to
        /// define the ordered set of <see cref="ChartGradientColorStop"/> entries that form the gradient. At least two stops (for example,
        /// offsets 0 and 100) are recommended to produce a visible transition.
        ///
        /// Stops are applied in ascending order of <see cref="ChartGradientColorStop.Offset"/>. For consistent results, ensure offsets are within
        /// the 0–100 range and colors/opacity are set as required by your design.
        /// </remarks>
        internal List<ChartGradientColorStop> GradientColorStops
        {
            get => _gradientColorStops;
            set
            {
                if (_gradientColorStops != value)
                {
                    _gradientColorStops = value;
                }
            }
        }

        #endregion

        #region Lifecycle Methods

        /// <exclude />
        /// <summary>
        /// Initializes the gradient by setting its renderer type.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();
            RendererType = typeof(ChartGradientRenderer);
        }

        /// <summary>
        /// Detect changes to parameter values and notify the renderer when they change.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            bool changed = false;

            if (_gradientStartX != X1)
            {
                _gradientStartX = X1;
                changed = true;
            }

            if (_gradientStartY != Y1)
            {
                _gradientStartY = Y1;
                changed = true;
            }

            if (_gradientEndX != X2)
            {
                _gradientEndX = X2;
                changed = true;
            }

            if (_gradientEndY != Y2)
            {
                _gradientEndY = Y2;
                changed = true;
            }
            if (_renderer != Renderer)
            {
                _renderer = Renderer;
                _renderer?.OnParentParameterSet();
            }

            if (changed)
            {
                Renderer?.GradientValueChanged();
            }
        }

        #endregion

        #region Helper Methods 

        /// <summary>
        /// Updates the gradient properties from a child subcomponent.
        /// </summary>
        /// <param name="key">The property name being updated.</param>
        /// <param name="keyValue">The property value.</param>
        internal void UpdateGradientProperties(string key, object keyValue)
        {
            switch (key)
            {
                case nameof(GradientColorStops):
                    GradientColorStops = _gradientColorStops = (List<ChartGradientColorStop>)keyValue;
                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}
