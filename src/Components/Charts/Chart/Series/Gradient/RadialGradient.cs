using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Charts.Internal;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Represents the options for configuring a radial gradient used to paint chart elements in the chart component.
    /// </summary>
    /// <remarks>
    /// Use <see cref="RadialGradient"/> to create a center-out color blend for paintable chart elements (for example, series fill, markers,
    /// legends, and tooltip markers). A radial gradient is defined by its center (<c>Cx</c>, <c>Cy</c>), optional focal point (<c>Fx</c>, <c>Fy</c>),
    /// radius (<c>R</c>), and one or more color stops.
    ///
    /// Coordinate values are typically normalized within the gradient box in the range 0 to 1, where <c>(0.5, 0.5)</c> represents the center.
    /// After defining the gradient, reference it from a paintable property (for example, series <c>Fill</c>) using <c>url(#gradientId)</c>.
    /// </remarks>
    public class RadialGradient : ChartSubComponent, IChartElement
    {
        #region Fields

        private double _centerX;
        private double _centerY;
        private double _focalX;
        private double _focalY;
        private double _radius;

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
        /// Gets or sets the x-coordinate of the center of the radial gradient.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> representing the normalized center x-coordinate (range: 0 to 1). The default value is <c>0.5</c>.
        /// </value>
        /// <remarks>
        /// Specifies the horizontal origin of the gradient’s circle within the gradient box; <c>0</c> aligns to the left edge and <c>1</c> to the right edge.
        /// </remarks>
        [Parameter]
        public double Cx { get; set; }

        /// <summary>
        /// Gets or sets the y-coordinate of the center of the radial gradient.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> representing the normalized center y-coordinate (range: 0 to 1). The default value is <c>0.5</c>.
        /// </value>
        /// <remarks>
        /// Specifies the vertical origin of the gradient’s circle within the gradient box; <c>0</c> aligns to the top edge and <c>1</c> to the bottom edge.
        /// </remarks>
        [Parameter]
        public double Cy { get; set; }

        /// <summary>
        /// Gets or sets the x-coordinate of the focal point of the radial gradient.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> representing the normalized focal x-coordinate (range: 0 to 1). The default value is <c>0.5</c>.
        /// </value>
        /// <remarks>
        /// Controls the apparent focus from which the gradient emanates horizontally. Values offset from <see cref="Cx"/> skew the gradient highlight toward that direction.
        /// </remarks>
        [Parameter]
        public double Fx { get; set; }

        /// <summary>
        /// Gets or sets the y-coordinate of the focal point of the radial gradient.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> representing the normalized focal y-coordinate (range: 0 to 1). The default value is <c>0.5</c>.
        /// </value>
        /// <remarks>
        /// Controls the apparent focus from which the gradient emanates vertically. Values offset from <see cref="Cy"/> skew the gradient highlight toward that direction.
        /// </remarks>
        [Parameter]
        public double Fy { get; set; }

        /// <summary>
        /// Gets or sets the radius of the radial gradient circle.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> representing the normalized radius (range: 0 to 1). The default value is <c>0.5</c>.
        /// </value>
        /// <remarks>
        /// Determines the size of the gradient coverage area relative to the gradient box; <c>0</c> is a point and <c>1</c> spans the full box extent.
        /// </remarks>
        [Parameter]
        public double R { get; set; }

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
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            bool changed = false;

            if (_centerX != Cx)
            {
                _centerX = Cx;
                changed = true;
            }

            if (_centerY != Cy)
            {
                _centerY = Cy;
                changed = true;
            }

            if (_focalX != Fx)
            {
                _focalX = Fx;
                changed = true;
            }

            if (_focalY != Fy)
            {
                _focalY = Fy;
                changed = true;
            }

            if (_radius != R)
            {
                _radius = R;
                changed = true;
            }

            if (changed)
            {
                Renderer?.GradientValueChanged();
            }
            if (_renderer != Renderer)
            {
                _renderer = Renderer;
                _renderer?.OnParentParameterSet();
            }
        }

        #endregion

        #region Internal Methods

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
