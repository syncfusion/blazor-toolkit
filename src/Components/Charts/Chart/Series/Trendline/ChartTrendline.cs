using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Internal;
using Syncfusion.Blazor.Toolkit.Charts.Internal;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Represents a single trendline configuration used to predict or visualize trends over a series.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A <see cref="ChartTrendline"/> defines an analytical model (linear, exponential, polynomial, power, or moving average)
    /// drawn on top of a <see cref="ChartSeries"/> to indicate the direction of data values.
    /// </para>
    /// <para>
    /// Trendlines support visual customization, accessibility metadata, markers, gradients, and animation.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <SfChart>
    ///     <ChartSeries DataSource="@Data" XName="X" YName="Y">
    ///         <ChartTrendlines>
    ///             <ChartTrendline Name="Linear" Type="TrendlineTypes.Linear" Width="2" DashArray="5,1">
    ///                 <ChartTrendlineMarker Visible="true" />
    ///                 <ChartTrendlineAnimation Enable="true" Duration="800" />
    ///             </ChartTrendline>
    ///         </ChartTrendlines>
    ///     </ChartSeries>
    /// </SfChart>
    /// ]]>
    /// </code>
    /// </example>
    public class ChartTrendline : ChartSubComponent, IChartElement
    {
        #region Fields

        private SfChart? _chart;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of the trendline.
        /// </summary>
        /// <value>A string representing the name of the trendline. Default is an empty string.</value>
        /// <remarks>The name is shown in the legend and used for accessibility metadata.</remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to add a name property to a trendline.
        /// <SfChart>
        ///     <ChartSeries DataSource="@Data" XName="XValue" YName="YValue">
        ///         <ChartTrendlines>
        ///             <ChartTrendline Name="Linear" />
        ///         </ChartTrendlines>
        ///     </ChartSeries>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        private string _name = string.Empty;
        [Parameter]
        public string Name
        {
            get => _name;
            set
            {
                if (_name == value)
                {
                    return;
                }

                _name = value;
                if (Renderer is not null)
                {
                    Renderer.Series?.SetName(_name);
                    _chart?.OnLayoutChange();
                }
            }
        }

        /// <summary>
        /// Gets or sets the dash array pattern for the trendline stroke.
        /// </summary>
        /// <value>A string that specifies the dash pattern (e.g., <c>"5,1"</c>). Default is <c>"0"</c>.</value>
        /// <remarks>Use comma-separated values to define dashed or dotted styles.</remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to add a dashed trendline to a chart.
        /// <SfChart>
        ///     <ChartSeries DataSource="@Data" XName="XValue" YName="YValue">
        ///         <ChartTrendlines>
        ///             <ChartTrendline DashArray="5,1" />
        ///         </ChartTrendlines>
        ///     </ChartSeries>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        private string _dashArray = "0";
        [Parameter]
        public string DashArray
        {
            get => _dashArray;
            set
            {
                if (_dashArray == value)
                {
                    return;
                }

                _dashArray = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the trendline is visible.
        /// </summary>
        /// <value><see langword="true"/> to show; otherwise <see langword="false"/>. Default is <see langword="true"/>.</value>
        /// <remarks>Updates both trendline rendering and legend visibility.</remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to add a visible trendline to a chart.
        /// <SfChart>
        ///     <ChartSeries DataSource="@Data" XName="XValue" YName="YValue">
        ///         <ChartTrendlines>
        ///             <ChartTrendline Visible="true" />
        ///         </ChartTrendlines>
        ///     </ChartSeries>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        private bool _visible = true;
        [Parameter]
        public bool Visible
        {
            get => _visible;
            set
            {
                if (_visible != value)
                {
                    _visible = value;

                    if (TargetSeries?.Renderer is not null)
                    {

                        TargetSeries.Renderer.RendererShouldRender = true;
                        TargetSeries.Renderer.TrendLineLegendVisibility = _visible;
                        TargetSeries.Renderer.ProcessRenderQueue();

                        if (_chart?._legendRenderer is not null)
                        {
                            _chart._legendRenderer.RendererShouldRender = true;
                            _chart._legendRenderer.UpdateLegendFill(TargetSeries.Renderer);
                            _chart._legendRenderer.ProcessRenderQueue();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the type of the trendline.
        /// </summary>
        /// <value>A <see cref="TrendlineTypes"/> value specifying the trendline model.</value>
        /// <remarks>Choose a model that best fits your data characteristics.</remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to add a power trendline to a chart.
        /// <SfChart>
        ///     <ChartSeries DataSource="@Data" XName="XValue" YName="YValue">
        ///         <ChartTrendlines>
        ///             <ChartTrendline Type="TrendlineTypes.Power" />
        ///         </ChartTrendlines>
        ///     </ChartSeries>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        private TrendlineTypes _type;
        [Parameter]
        public TrendlineTypes Type
        {
            get => _type;
            set
            {
                if (_type != value)
                {

                    _type = value;
                    if (_chart is not null)
                    {

                        _chart._trendlineContainer?.RemoveRenderer(TargetSeries?.Renderer ?? null!);
                        TrendlineInitiator?.InitSeriesCollection();

                        if (_chart._trendlineContainer is not null)
                        {
                            _chart._trendlineContainer.RendererShouldRender = true;
                            _chart._trendlineContainer.Prerender();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the accessibility description for the <see cref="ChartTrendline"/>.
        /// </summary>
        /// <value>A string used by assistive technologies. Default is empty.</value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to add a power trendline with an accessibility description to a chart.
        /// <SfChart>
        ///     <ChartSeries DataSource="@Data" XName="XValue" YName="YValue">
        ///         <ChartTrendlines>
        ///             <ChartTrendline AccessibilityDescription="Power trendline" />
        ///         </ChartTrendlines>
        ///     </ChartSeries>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string AccessibilityDescription { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the format string used to generate dynamic accessibility descriptions.
        /// </summary>
        /// <value>A format string (e.g., <c>"${series.name} : ${point.x}"</c>). Default is empty.</value>
        /// <remarks>Applied when accessibility descriptions should be computed at runtime.</remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to add a power trendline with an accessibility description format to a chart.
        /// <SfChart>
        ///     <ChartSeries DataSource="@Data" XName="XValue" YName="YValue">
        ///         <ChartTrendlines>
        ///             <ChartTrendline AccessibilityDescriptionFormat="Power trendline" />
        ///         </ChartTrendlines>
        ///     </ChartSeries>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string AccessibilityDescriptionFormat { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the ARIA role for the trendline.
        /// </summary>
        /// <value>A string representing the ARIA role. Default is empty.</value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to add a power trendline with an accessibility role to a chart.
        /// <SfChart>
        ///     <ChartSeries DataSource="@Data" XName="XValue" YName="YValue">
        ///         <ChartTrendlines>
        ///             <ChartTrendline AccessibilityRole="Trendline" />
        ///         </ChartTrendlines>
        ///     </ChartSeries>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string AccessibilityRole { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets whether the trendline is focusable via keyboard navigation.
        /// </summary>
        /// <value><see langword="true"/> to enable focus; otherwise <see langword="false"/>. Default is <see langword="true"/>.</value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to make a trendline focusable in a chart.
        /// <SfChart>
        ///     <ChartSeries DataSource="@Data" XName="XValue" YName="YValue">
        ///         <ChartTrendlines>
        ///             <ChartTrendline Focusable="true" />
        ///         </ChartTrendlines>
        ///     </ChartSeries>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool Focusable { get; set; } = true;

        /// <summary>
        /// Gets or sets the moving average period for the trendline.
        /// </summary>
        /// <value>A <see cref="double"/> representing the period for moving average.</value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to add a Moving Average trendline with a specified period
        /// // and enable markers for it in a chart.
        /// <SfChart>
        ///     <ChartSeries DataSource="@Data" XName="XValue" YName="YValue">
        ///         <ChartTrendlines>
        ///             <ChartTrendline Period="5" Type="TrendlineTypes.MovingAverage">
        ///                 <ChartTrendlineMarker Visible="true" />
        ///             </ChartTrendline>
        ///         </ChartTrendlines>
        ///     </ChartSeries>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        private double _period = Constants.TrendlinePeriod;
        [Parameter]
        public double Period
        {
            get => _period;
            set
            {
                if (Math.Abs(_period - value) < double.Epsilon)
                {
                    return;
                }

                _period = value;
            }
        }

        /// <summary>
        /// Gets or sets the order for polynomial trendline fitting.
        /// </summary>
        /// <value>A <see cref="double"/> representing the polynomial order. Default is <b>2</b>.</value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to add a polynomial trendline with order 1 to a chart.
        /// <SfChart>
        ///     <ChartSeries DataSource="@Data" XName="XValue" YName="YValue">
        ///         <ChartTrendlines>
        ///             <ChartTrendline PolynomialOrder="1" Type="TrendlineTypes.Polynomial" />
        ///         </ChartTrendlines>
        ///     </ChartSeries>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        private double _polynomialOrder = Constants.TrendlinePolynomialOrder;
        [Parameter]
        public double PolynomialOrder
        {
            get => _polynomialOrder;
            set
            {
                if (Math.Abs(_polynomialOrder - value) < double.Epsilon)
                {
                    return;
                }

                _polynomialOrder = value;
            }
        }

        /// <summary>
        /// Gets or sets the backward forecast period for the trendline.
        /// </summary>
        /// <value>A <see cref="double"/> that extends the trendline backward.</value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to apply a trendline with a backward forecast of 5 to a chart.
        /// <SfChart>
        ///     <ChartSeries DataSource="@Data" XName="XValue" YName="YValue">
        ///         <ChartTrendlines>
        ///             <ChartTrendline BackwardForecast="5" />
        ///         </ChartTrendlines>
        ///     </ChartSeries>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        private double _backwardForecast;
        [Parameter]
        public double BackwardForecast
        {
            get => _backwardForecast;
            set
            {
                if (Math.Abs(_backwardForecast - value) < double.Epsilon)
                {
                    return;
                }

                _backwardForecast = value;
            }
        }

        /// <summary>
        /// Gets or sets the forward forecast period for the trendline.
        /// </summary>
        /// <value>A <see cref="double"/> that extends the trendline forward.</value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to apply a trendline with a forward forecast of 10 to a chart.
        /// <SfChart>
        ///     <ChartSeries DataSource="@Data" XName="XValue" YName="YValue">
        ///         <ChartTrendlines>
        ///             <ChartTrendline ForwardForecast="10" />
        ///         </ChartTrendlines>
        ///     </ChartSeries>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        private double _forwardForecast;
        [Parameter]
        public double ForwardForecast
        {
            get => _forwardForecast;
            set
            {
                if (Math.Abs(_forwardForecast - value) < double.Epsilon)
                {
                    return;
                }

                _forwardForecast = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether tooltips are enabled for the trendline.
        /// </summary>
        /// <value><see langword="true"/> to enable; otherwise <see langword="false"/>. Default is <see langword="true"/>.</value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to enable tooltips for trendlines and display trendline markers.
        /// <SfChart>
        ///     <ChartTooltipSettings Enable="true"></ChartTooltipSettings>
        ///     <ChartSeries DataSource="@Data" XName="XValue" YName="YValue">
        ///         <ChartTrendlines>
        ///             <ChartTrendline EnableTooltip="true">
        ///                <ChartTrendlineMarker Visible="true" />
        ///             </ChartTrendline>
        ///         </ChartTrendlines>
        ///     </ChartSeries>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        private bool _enableTooltip = true;
        [Parameter]
        public bool EnableTooltip
        {
            get => _enableTooltip;
            set
            {
                if (_enableTooltip == value)
                {
                    return;
                }

                _enableTooltip = value;
            }
        }

        /// <summary>
        /// Gets or sets the Y-intercept of the trendline.
        /// </summary>
        /// <value>A <see cref="double"/> representing the intercept. Default is <see cref="double.NaN"/>.</value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to apply a trendline with an intercept of 30 to a chart.
        /// <SfChart>
        ///     <ChartSeries DataSource="@Data" XName="XValue" YName="YValue">
        ///         <ChartTrendlines>
        ///             <ChartTrendline Intercept="30" />
        ///         </ChartTrendlines>
        ///     </ChartSeries>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        private double _intercept = double.NaN;
        [Parameter]
        public double Intercept
        {
            get => _intercept;
            set
            {
                if (Math.Abs(_intercept - value) < double.Epsilon)
                {
                    return;
                }

                _intercept = value;
            }
        }

        /// <summary>
        /// Gets or sets the stroke color of the trendline.
        /// </summary>
        /// <value>A CSS color (hex, rgb/rgba, or named). Default is empty.</value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the fill color of a trendline to green.
        /// <SfChart>
        ///     <ChartSeries DataSource="@Data" XName="XValue" YName="YValue">
        ///         <ChartTrendlines>
        ///             <ChartTrendline Fill="green" />
        ///         </ChartTrendlines>
        ///     </ChartSeries>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        private string _fill = string.Empty;
        [Parameter]
        public string Fill
        {
            get => _fill;
            set
            {
                if (_fill == value)
                {
                    return;
                }

                _fill = value;
            }
        }

        /// <summary>
        /// Gets or sets the stroke width of the trendline.
        /// </summary>
        /// <value>A <see cref="double"/> representing the width. Default is <b>1</b>.</value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the width of a trendline to 20.
        /// <SfChart>
        ///     <ChartSeries DataSource="@Data" XName="XValue" YName="YValue">
        ///         <ChartTrendlines>
        ///             <ChartTrendline Width="20" />
        ///         </ChartTrendlines>
        ///     </ChartSeries>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        private double _widthProperty = Constants.TrendlineWidth;
        [Parameter]
        public double Width
        {
            get => _widthProperty;
            set
            {
                if (Math.Abs(_widthProperty - value) < double.Epsilon)
                {
                    return;
                }

                _widthProperty = value;
            }
        }

        /// <summary>
        /// Gets or sets the legend marker shape associated with the trendline.
        /// </summary>
        /// <value>A <see cref="LegendShape"/> describing the legend marker.</value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to customize the legend shape of a trendline and enable markers in a chart.
        /// <SfChart>
        ///     <ChartSeries DataSource="@Data" XName="XValue" YName="YValue">
        ///         <ChartTrendlines>
        ///             <ChartTrendline LegendShape="LegendShape.Rectangle" Name="Triangle">
        ///                 <ChartTrendlineMarker Visible="true" />
        ///             </ChartTrendline>
        ///         </ChartTrendlines>
        ///     </ChartSeries>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        private LegendShape _legendShape = LegendShape.SeriesType;
        [Parameter]
        public LegendShape LegendShape
        {
            get => _legendShape;
            set
            {
                if (_legendShape == value)
                {
                    return;
                }

                _legendShape = value;
            }
        }

        /// <summary>
        /// Gets or sets the marker settings for the trendline.
        /// </summary>
        /// <value>A <see cref="ChartTrendlineMarker"/> with marker properties.</value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to display trendline markers on a chart.
        /// <SfChart>
        ///     <ChartSeries DataSource="@Data" XName="XValue" YName="YValue">
        ///         <ChartTrendlines>
        ///             <ChartTrendline>
        ///                 <ChartTrendlineMarker Visible="true" />
        ///             </ChartTrendline>
        ///         </ChartTrendlines>
        ///     </ChartSeries>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ChartTrendlineMarker Marker { get; set; } = new ChartTrendlineMarker();

        /// <summary>
        /// Provides options to configure a linear gradient for a chart <see cref="ChartSeries"/>, technical <c>ChartIndicator</c>,
        /// or <see cref="ChartTrendline"/> within the <see cref="SfChart"/> component.
        /// </summary>
        /// <remarks>
        /// 
        /// <see cref="ChartLinearGradient"/> is a child component that inherits from <see cref="LinearGradient"/>. Use
        /// <see cref="LinearGradient.X1"/>, <see cref="LinearGradient.Y1"/>, <see cref="LinearGradient.X2"/>, and <see cref="LinearGradient.Y2"/>
        /// to set the gradient direction, and define color transitions using <see cref="ChartGradientColorStops"/> with one or more
        /// <see cref="ChartGradientColorStop"/> elements.
        /// 
        /// 
        /// Coordinate values are typically normalized to the range 0..1 relative to the paint box, where <c>(0,0)</c> is the top-left and
        /// <c>(1,1)</c> is the bottom-right. Gradient stop <c>Offset</c> commonly supports normalized 0..1 or percentage 0..100 values.
        /// 
        /// 
        /// When nested under <see cref="ChartSeries"/>, <c>ChartIndicator</c>, or <see cref="ChartTrendline"/>, the gradient is applied
        /// automatically to the owning element.
        /// 
        /// </remarks>
        internal ChartLinearGradient? LinearGradient { get; set; }

        /// <summary>
        /// Represents options to configure a radial gradient for a chart <see cref="ChartSeries"/>, technical <c>ChartIndicator</c>,
        /// or <see cref="ChartTrendline"/> within the <see cref="SfChart"/> component.
        /// </summary>
        /// <remarks>
        ///
        /// <see cref="ChartRadialGradient"/> is a child component that inherits from <see cref="RadialGradient"/>. Set the center with
        /// <see cref="RadialGradient.Cx"/> and <see cref="RadialGradient.Cy"/>, the focal point with <see cref="RadialGradient.Fx"/> and
        /// <see cref="RadialGradient.Fy"/>, and the radius with <see cref="RadialGradient.R"/>. Define color transitions using
        /// <see cref="ChartGradientColorStops"/> containing one or more <see cref="ChartGradientColorStop"/> elements.
        ///
        ///
        /// Coordinates are typically normalized to 0..1 relative to the gradient box; <c>(0.5, 0.5)</c> centers the gradient, and <c>R="0.5"</c>
        /// covers roughly half the box. Stop <c>Offset</c> may be specified as 0..1 or 0..100 (percent).
        ///
        ///
        /// When placed under <see cref="ChartSeries"/>, <c>ChartIndicator</c>, or <see cref="ChartTrendline"/>, the gradient is applied
        /// automatically.
        ///
        /// </remarks>
        internal ChartRadialGradient? RadialGradient { get; set; }

        /// <summary>
        /// Gets or sets the animation settings for the trendline.
        /// </summary>
        /// <value>A <see cref="ChartTrendlineAnimation"/> instance specifying animation behavior.</value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to enable animation for a trendline with a duration of 2000ms.
        /// <SfChart>
        ///     <ChartSeries DataSource="@Data" XName="XValue" YName="YValue">
        ///         <ChartTrendlines>
        ///             <ChartTrendline>
        ///                 <ChartTrendlineAnimation Enable="true" Duration="2000" />
        ///             </ChartTrendline>
        ///         </ChartTrendlines>
        ///     </ChartSeries>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ChartTrendlineAnimation Animation { get; set; } = new ChartTrendlineAnimation();

        [CascadingParameter]
        internal ChartTrendlines? Parent { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public Type RendererType { get; set; } = null!;

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public string RendererKey { get; set; } = SfBaseUtils.GenerateID("charttrendline");

        /// <summary>
        /// Gets or sets the owning series for this trendline.
        /// </summary>
        internal ChartSeries? TargetSeries { get; set; }

        /// <summary>
        /// Gets the internal trendline initiator orchestrating renderer creation and updates.
        /// </summary>
        internal TrendlineBase? TrendlineInitiator { get; set; }

        /// <summary>
        /// Gets the series renderer associated with this trendline (if any).
        /// </summary>
        internal ChartSeriesRenderer? Renderer { get; set; }

        #endregion

        #region Lifecycle Methods

        /// <exclude />
        /// <summary>
        /// Initializes the trendline, associates it with the parent collection, and registers with the chart container.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Tracker is ChartTrendlines chartTrendlines)
            {
                Parent = chartTrendlines;
            }

            _chart = Parent?.Series?.Container;
            Parent?.Trendlines.Add(this);
            InitTrendline();
        }

        /// <summary>
        /// Disposes the trendline and unregisters it from the chart container and parent collection.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override ValueTask DisposeAsyncCore()
        {
            _chart?._trendlineContainer?.RemoveElement(this);
            TrendlineInitiator?.Dispose();
            _ = Parent?.Trendlines?.Remove(this);
            return base.DisposeAsyncCore();
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Initializes internal trendline infrastructure and registers the element with the chart container.
        /// </summary>
        /// <remarks>
        /// Creates the <see cref="TrendlineBase"/> initiator, binds this instance, initializes series collection,
        /// and invokes <c>AddTrendline</c> on the owning chart container.
        /// </remarks>
        internal void InitTrendline()
        {
            TrendlineInitiator = new TrendlineBase
            {
                Trendline = this
            };

            TrendlineInitiator.InitSeriesCollection();
            Parent?.Series?.Container?.AddTrendline(this);
        }

        /// <summary>
        /// Updates a trendline child property based on an identifier key. Used by child components to propagate changes.
        /// </summary>
        /// <param name="key">The property key (use <c>nameof(ChartTrendline.Property)</c>).</param>
        /// <param name="keyValue">The property value to assign.</param>
        /// <remarks>
        /// Accepts keys for <see cref="Animation"/>, <see cref="Marker"/>, <see cref="LinearGradient"/>, and <see cref="RadialGradient"/>.
        /// </remarks>
        internal void UpdateTrendlineProperty(string key, object keyValue)
        {
            switch (key)
            {
                case nameof(Animation):
                    Animation = (ChartTrendlineAnimation)keyValue;
                    break;

                case nameof(Marker):
                    Marker = (ChartTrendlineMarker)keyValue;
                    break;

                case nameof(LinearGradient):
                    LinearGradient = (ChartLinearGradient)keyValue;
                    break;

                case nameof(RadialGradient):
                    RadialGradient = (ChartRadialGradient)keyValue;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Sets the polynomial order; provided for internal call sites to avoid direct parameter mutation patterns.
        /// </summary>
        /// <param name="value">The polynomial order value.</param>
        internal void PolynomialOrderValue(double value)
        {
            PolynomialOrder = value;
        }

        /// <summary>
        /// Sets the visibility flag; provided for internal call sites to avoid direct parameter mutation patterns.
        /// </summary>
        /// <param name="value">A value indicating whether the trendline should be visible.</param>
        internal void SetVisibility(bool value)
        {
            Visible = value;
        }

        #endregion
    }
}
