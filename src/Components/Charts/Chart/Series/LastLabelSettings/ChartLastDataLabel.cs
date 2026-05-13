using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Charts.Internal;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Defines the options used to display and customize the label for the last visible value in a <see cref="ChartSeries"/>.
    /// </summary>
    /// <remarks>
    /// Use this component to highlight the most recent data point in a series with a label and an optional horizontal indicator line.
    /// Appearance can be customized through properties such as <see cref="Background"/>, <see cref="LineColor"/>, <see cref="DashArray"/>,
    /// <see cref="LineWidth"/>, and corner radii <see cref="Rx"/> / <see cref="Ry"/>. To style the label's text and outline, use
    /// the nested <see cref="ChartLastDataLabelFont"/> and <see cref="ChartLastDataLabelBorder"/> subcomponents.
    /// </remarks>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <SfChart>
    ///   <ChartSeriesCollection>
    ///     <ChartSeries XName="X" YName="Y" DataSource="@StockData" Type="ChartSeriesType.Line">
    ///       <ChartLastDataLabel ShowLabel="true"
    ///                            Background="rgba(0,0,0,0.5)"
    ///                            LineColor="black"
    ///                            LineWidth="1"
    ///                            DashArray="4,2"
    ///                            Rx="6"
    ///                            Ry="6">
    ///         <ChartLastDataLabelBorder Color="white" Width="1" />
    ///         <ChartLastDataLabelFont Size="12px" FontWeight="600" Color="white" />
    ///       </ChartLastDataLabel>
    ///     </ChartSeries>
    ///   </ChartSeriesCollection>
    /// </SfChart>
    /// ]]>
    /// </code>
    /// </example>
    public class ChartLastDataLabel : ChartSubComponent, ISubcomponentTracker, IChartElement
    {
        #region Fields
        bool _showLabel;
        string _dashArray = string.Empty;
        string _background = string.Empty;
        double _lineWidth = Constants.DefaultBorderWidth;
        string _lineColor = string.Empty;
        double _cornerRadiusX = Constants.DefaultCornerRadius;
        double _cornerRadiusY = Constants.DefaultCornerRadius;

        LastDataLabelRenderer? _renderer;
        Type _rendererType = null!;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the label and indicator for the last data point in the series are visible.
        /// </summary>
        /// <value>
        /// <c>true</c> to display the last value label and the corresponding grid line indicator; otherwise, <c>false</c>.
        /// The default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// Toggle this to highlight the end point of a trend, improving readability for monitoring scenarios and dashboards.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartLastDataLabel ShowLabel="true" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool ShowLabel
        {
            get => _showLabel;
            set
            {
                if (_showLabel != value)
                {
                    _showLabel = value;
                    Renderer?.ToggleVisibility();
                }
            }
        }

        /// <summary>
        /// Gets or sets the background color of the last value label.
        /// </summary>
        /// <value>
        /// A <c>string</c> representing the background color (e.g., <c>#ffffff</c>, <c>rgba(0,0,0,0.25)</c>, or <c>transparent</c>).
        /// The default value is <c>transparent</c> (theme-dependent).
        /// </value>
        /// <remarks>
        /// Use a contrasting color to ensure the label remains legible over colored or complex chart backgrounds.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartLastDataLabel Background="red" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string Background
        {
            get => _background;
            set
            {
                if (_background != value)
                {
                    _background = value;
                    Renderer?.LastlabelValueChanged();
                }
            }
        }

        /// <summary>
        /// Gets the owning <see cref="ChartSeries"/> via cascading parameters.
        /// </summary>
        /// <value>The parent <see cref="ChartSeries"/> if available; otherwise, <see langword="null"/>.</value>
        [CascadingParameter]
        internal ChartSeries? Series { get; set; }

        /// <summary>
        /// Holds the border configuration of the last data label.
        /// </summary>
        /// <value>An instance of <see cref="ChartLastDataLabelBorder"/> that describes border styling.</value>
        internal ChartLastDataLabelBorder Border { get; set; } = new ChartLastDataLabelBorder();

        /// <summary>
        /// Holds the font configuration of the last data label.
        /// </summary>
        /// <value>An instance of <see cref="ChartLastDataLabelFont"/> that describes font styling.</value>
        internal ChartLastDataLabelFont Font { get; set; } = new ChartLastDataLabelFont();

        /// <summary>
        /// Gets or sets the dash pattern for the grid line behind the last value label.
        /// </summary>
        /// <value>
        /// A <c>string</c> defining the dash array (e.g., <c>"4,2"</c> for dashed lines).
        /// An empty string renders a solid line.
        /// </value>
        /// <remarks>
        /// Use dash patterns to visually distinguish the indicator from other grid or series lines.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartLastDataLabel DashArray="2,5" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string DashArray
        {
            get => _dashArray;
            set
            {
                if (_dashArray != value)
                {
                    _dashArray = value;
                    Renderer?.LastlabelValueChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the width of the indicator line displayed behind the last value label.
        /// </summary>
        /// <value>
        /// A <c>double</c> specifying the thickness of the indicator line. The default value is <c>1</c>.
        /// </value>
        /// <remarks>
        /// Thicker lines can improve visibility, especially for dense charts or when multiple series are present.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartLastDataLabel LineWidth="0.5" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double LineWidth
        {
            get => _lineWidth;
            set
            {
                if (_lineWidth != value)
                {
                    _lineWidth = value;
                    Renderer?.LastlabelValueChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the color of the indicator line displayed behind the last value label.
        /// </summary>
        /// <value>
        /// A <c>string</c> specifying the line color. If empty, the series color is used.
        /// </value>
        /// <remarks>
        /// Provides a clear horizontal reference to the last value to improve readability.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartLastDataLabel LineColor="black" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string LineColor
        {
            get => _lineColor;
            set
            {
                if (_lineColor != value)
                {
                    _lineColor = value;
                    Renderer?.LastlabelValueChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the horizontal radius of the rounded corners of the last data label's background.
        /// </summary>
        /// <value>A <see cref="double"/> representing the horizontal radius. The default is <b>5</b>.</value>
        /// <remarks>
        /// This applies when <see cref="ChartLastDataLabelBorder"/> is enabled.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartLastDataLabel Rx="10">
        ///   <ChartLastDataLabelBorder Color="Green" Width="1" />
        /// </ChartLastDataLabel>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double Rx
        {
            get => _cornerRadiusX;
            set
            {
                if (_cornerRadiusX != value)
                {
                    _cornerRadiusX = value;
                    Renderer?.LastlabelValueChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the vertical radius of the rounded corners of the last data label's background.
        /// </summary>
        /// <value>A <see cref="double"/> representing the vertical radius. The default is <b>5</b>.</value>
        /// <remarks>
        /// This applies when <see cref="ChartLastDataLabelBorder"/> is enabled.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartLastDataLabel Ry="10">
        ///   <ChartLastDataLabelBorder Color="Green" Width="1" />
        /// </ChartLastDataLabel>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double Ry
        {
            get => _cornerRadiusY;
            set
            {
                if (_cornerRadiusY != value)
                {
                    _cornerRadiusY = value;
                    Renderer?.LastlabelValueChanged();
                }
            }
        }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public Type RendererType
        {
            get => _rendererType;
            set => _rendererType = value;
        }

        /// <summary>
        /// Internal renderer instance used by the charting infrastructure.
        /// Invokes <c>OnParentParameterSet</c> when changed.
        /// </summary>
        /// <value>The current <see cref="LastDataLabelRenderer"/>; otherwise <see langword="null"/>.</value>
        internal LastDataLabelRenderer Renderer
        {
            get => _renderer ?? null!;
            set
            {
                if (_renderer != value)
                {
                    _renderer = value;
                    _renderer?.OnParentParameterSet();
                }
            }
        }

        /// <inheritdoc />
        string IChartElement.RendererKey { get; set; } = null!;

        #endregion

        #region Lifecycle Methods

        /// <exclude />
        /// <summary>
        /// Initializes the component and wires it to the parent <see cref="ChartSeries"/>.
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

            RendererType = typeof(LastDataLabelRenderer);
            if (Series is not null)
            {
                Series.UpdateSeriesProperties("LastDataLabel", this);
            }
        }

        /// <exclude />
        /// <summary>
        /// Applies parameter updates and propagates changes to the owning <see cref="ChartSeries"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            Series?.UpdateSeriesProperties("LastDataLabel", this);
        }

        /// <summary>
        /// Releases component resources and disconnects from parent references.
        /// </summary>
        protected override ValueTask DisposeAsyncCore()
        {
            ComponentDispose();
            return base.DisposeAsyncCore();
        }

        #endregion

        #region Internal Methods
        /// <summary>
        /// Releases component resources and disconnects from parent references.
        /// </summary>
        internal void ComponentDispose()
        {
            Series = null;
            ChildContent = null!;
            Border?.ComponentDispose();
            Font?.ComponentDispose();
        }
        /// <summary>
        /// Updates internal last-label related properties when a child subcomponent changes.
        /// </summary>
        /// <param name="key">A property key; expected values are <c>"Border"</c> or <c>"Font"</c>.</param>
        /// <param name="keyValue">The new subcomponent instance.</param>
        internal void UpdateLastlabelProperties(string key, object keyValue)
        {
            switch (key)
            {
                case nameof(Border):
                    Border = (ChartLastDataLabelBorder)keyValue;
                    break;
                case nameof(Font):
                    Font = (ChartLastDataLabelFont)keyValue;
                    break;
            }
        }

        #endregion
    }
}