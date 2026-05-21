using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Charts.Internal;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Configures data labels for a series in <see cref="SfChart"/>.
    /// </summary>
    /// <remarks>
    /// Place this component under a series' <c>ChartMarker</c> to configure the label's visibility, text or template,
    /// layout (position, alignment, margins, corner radius), style (fill, opacity, border, font), rotation, and
    /// overlap behavior. Property changes at runtime notify the renderer to refresh only the label layer.
    /// </remarks>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// @using Syncfusion.Blazor.Toolkit.Charts
    ///
    /// <SfChart>
    ///   <ChartSeriesCollection>
    ///     <ChartSeries DataSource="@WeatherReports"
    ///                  XName="X"
    ///                  YName="Y"
    ///                  Type="ChartSeriesType.Column">
    ///       <ChartMarker>
    ///         <ChartDataLabel Visible="true"
    ///                          Format="{value}°C"
    ///                          Position="ChartLabelPosition.Outer"
    ///                          Fill="#1b49cc"
    ///                          Opacity="0.85"
    ///                          LabelIntersectAction="Trim">
    ///           <ChartDataLabelBorder Width="1" Color="white" />
    ///           <ChartDataLabelMargin Top="6" Bottom="6" Left="8" Right="8" />
    ///           <ChartDataLabelFont Size="12px" FontFamily="Segoe UI" Color="white" FontWeight="600" />
    ///         </ChartDataLabel>
    ///       </ChartMarker>
    ///     </ChartSeries>
    ///   </ChartSeriesCollection>
    /// </SfChart>
    ///
    /// @code {
    ///   public class Weather
    ///   {
    ///       public string X { get; set; } = string.Empty;
    ///       public double Y { get; set; }
    ///   }
    ///
    ///   public List<Weather> WeatherReports { get; set; } = new()
    ///   {
    ///       new Weather { X = "Mon", Y = 22 },
    ///       new Weather { X = "Tue", Y = 24 },
    ///       new Weather { X = "Wed", Y = 19 },
    ///       new Weather { X = "Thu", Y = 27 },
    ///       new Weather { X = "Fri", Y = 23 },
    ///   };
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public class ChartDataLabel : ChartSubComponent, IChartElement, ISubcomponentTracker
    {

        #region Fields

        private int _pendingParametersSetCount;
        private ChartMarker? _marker;
        private ChartDataLabelRenderer? _renderer;

        // Backing fields
        private bool _visible;
        private string _name = null!;
        private string _fill = Constants.Transparent;
        private string _format = null!;
        private string _labelIntersectAction = "Hide";
        private double _opacity = Constants.DefaultOpacity;
        private double _angle;
        private bool _enableRotation;
        private ChartLabelPosition _position = ChartLabelPosition.Auto;
        private double _horizontalRadius = 5;
        private double _verticalRadius = 5;
        private Alignment _alignment = Alignment.Center;
        private ChartDataLabelBorder? _border = new();
        private ChartDataLabelMargin? _margin = new();
        private ChartDataLabelFont? _font = new();

        #endregion

        #region Properties

        /// <inheritdoc />
        string IChartElement.RendererKey { get; set; } = null!;

        /// <summary>
        /// Gets or sets the associated renderer instance for this data label.
        /// </summary>
        /// <value>
        /// The <see cref="ChartDataLabelRenderer"/> that renders data labels.
        /// </value>
        /// <remarks>
        /// Assigned by the chart infrastructure. When replaced, the renderer is notified to pull fresh parameters.
        /// </remarks>
        internal ChartDataLabelRenderer? Renderer { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public Type RendererType { get; set; } = null!;

        /// <summary>
        /// Gets or sets whether data labels are visible.
        /// </summary>
        /// <value>
        /// <c>true</c> to show labels; otherwise <c>false</c>. Default is <c>false</c>.
        /// </value>
        /// <remarks>
        /// Toggling this calls the renderer to show or hide the labels without re-rendering the entire chart.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartDataLabel Visible="true" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool Visible { get; set; }

        /// <summary>
        /// Gets or sets the field name from the data source used as the label text.
        /// </summary>
        /// <value>
        /// Mapping field name. Default is <c>string.Empty</c>.
        /// </value>
        /// <remarks>
        /// When specified, the chart uses the field's value from each data item as the label text for that point.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartDataLabel Visible="true" Name="DisplayText" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the format string for label values.
        /// </summary>
        /// <value>
        /// String with <c>{value}</c> placeholder and/or .NET format specifiers (e.g., <c>"{value}°C"</c>, <c>"{value:N1}"</c>).
        /// </value>
        /// <remarks>
        /// Applied when a <see cref="Template"/> is not used for the specific point.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartDataLabel Visible="true" Format="{value}%" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string Format { get; set; } = null!;

        /// <summary>
        /// Gets or sets the fill color of the datalabel.
        /// </summary>
        /// <value>
        /// Accepts valid CSS color string values such as hexadecimal, rgba, and color names. The default value is <c>transparent</c>.
        /// </value>
        /// <remarks>
        /// The fill color is used to set the background color of the data label.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartDataLabel Visible="true" Fill="#2E77D0" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string Fill { get; set; } = Constants.Transparent;

        /// <summary>
        /// Gets or sets the background opacity of the label.
        /// </summary>
        /// <value>
        /// A value in [0, 1]. Default is <b>1</b>.
        /// </value>
        /// <remarks>
        /// Values are clamped to the valid range to ensure consistent rendering.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartDataLabel Visible="true" Opacity="0.65" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double Opacity { get; set; } = Constants.DefaultOpacity;

        /// <summary>
        /// Gets or sets the rotation angle (in degrees) for the label text.
        /// </summary>
        /// <value>
        /// A value in [-360, 360]. Default is <b>0</b>.
        /// </value>
        /// <remarks>
        /// Takes effect only when <see cref="EnableRotation"/> is <c>true</c>.
        /// Out-of-range values are clamped.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartDataLabel Visible="true" EnableRotation="true" Angle="90" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double Angle { get; set; }

        /// <summary>
        /// Gets or sets whether rotation is enabled for labels.
        /// </summary>
        /// <value>
        /// <c>true</c> to rotate labels using <see cref="Angle"/>; otherwise <c>false</c>.
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartDataLabel Visible="true" EnableRotation="true" Angle="45" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool EnableRotation { get; set; }

        /// <summary>
        /// Gets or sets the label position relative to the data point.
        /// </summary>
        /// <value>
        /// One of <see cref="ChartLabelPosition"/> values. Default is <see cref="ChartLabelPosition.Auto"/>.
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartDataLabel Visible="true" Position="ChartLabelPosition.Outer" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ChartLabelPosition Position { get; set; } = ChartLabelPosition.Auto;

        /// <summary>
        /// Gets or sets the horizontal corner radius for the label background.
        /// </summary>
        /// <value>
        /// Non-negative double. Default is <b>5</b>.
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartDataLabel Visible="true" Rx="8" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double Rx { get; set; } = 5;

        /// <summary>
        /// Gets or sets the vertical corner radius for the label background.
        /// </summary>
        /// <value>
        /// Non-negative double. Default is <b>5</b>.
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartDataLabel Visible="true" Ry="8" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double Ry { get; set; } = 5;

        /// <summary>
        /// Gets or sets the alignment for the label relative to the data point.
        /// </summary>
        /// <value>
        /// One of <see cref="Alignment"/> values. Default is <see cref="Alignment.Center"/>.
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartDataLabel Visible="true" Alignment="Alignment.Near" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public Alignment Alignment { get; set; } = Alignment.Center;

        /// <summary>
        /// Gets or sets the border configuration for labels.
        /// </summary>
        /// <value>
        /// An instance of <see cref="ChartDataLabelBorder"/>. Default is a new instance.
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartDataLabel Visible="true">
        ///   <ChartDataLabelBorder Width="2" Color="black" />
        /// </ChartDataLabel>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ChartDataLabelBorder? Border { get; set; } = new();

        /// <summary>
        /// Gets or sets the margin around the label background.
        /// </summary>
        /// <value>
        /// An instance of <see cref="ChartDataLabelMargin"/>. Defaults to 5 on all sides.
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartDataLabel Visible="true">
        ///   <ChartDataLabelMargin Top="10" Bottom="10" Left="12" Right="12" />
        /// </ChartDataLabel>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ChartDataLabelMargin? Margin { get; set; } = new();

        /// <summary>
        /// Gets or sets the font configuration for labels.
        /// </summary>
        /// <value>
        /// An instance of <see cref="ChartDataLabelFont"/>. Default is a new instance.
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartDataLabel Visible="true">
        ///   <ChartDataLabelFont Size="14px" FontFamily="Arial" FontWeight="600" Color="#333" />
        /// </ChartDataLabel>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ChartDataLabelFont? Font { get; set; } = new();

        /// <summary>
        /// Gets or sets the template to render data labels.
        /// </summary>
        /// <value>
        /// A <see cref="RenderFragment{ChartDataPointInfo}"/> that renders rich content for each label.
        /// </value>
        /// <remarks>
        /// The template receives <see cref="ChartDataPointInfo"/> as the context. When a template is used,
        /// <see cref="Format"/> is not applied for that point.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartDataLabel Visible="true">
        ///   <Template Context="pt">
        ///     <div style="padding:4px 6px;background:#303F9F;color:#fff;border-radius:4px;">
        ///       @pt.X : @pt.Y
        ///     </div>
        ///   </Template>
        /// </ChartDataLabel>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public RenderFragment<ChartDataPointInfo> Template { get; set; } = null!;

        /// <summary>
        /// Gets or sets the action for data label intersection.
        /// </summary>
        /// <value>
        /// A string representing the action to be taken when data labels intersect. The options include:
        ///   - <c>Hide</c>: Data labels that intersect will not be displayed.
        ///   - <c>Trim</c>: Data labels will be truncated to fit without intersection.
        ///   - <c>None</c>: No action is taken, labels will overlap if necessary.
        /// <br/>
        /// The default value is <b>Hide</b>.
        /// </value>
        /// <remarks>
        /// This property determines how overlapping data labels are managed on the chart.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartDataLabel Visible="true" LabelIntersectAction="Trim" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string LabelIntersectAction { get; set; } = "Hide";

        #endregion

        #region Lifecycle Methods

        /// <exclude />
        /// <summary>
        /// Initializes the data label and registers it with the parent <c>ChartMarker</c>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Tracker is ChartMarker chartMarker)
            {
                _marker = chartMarker;
            }

            RendererType = typeof(ChartDataLabelRenderer);
            NotifyMarkerOfPropertyUpdate();
        }

        /// <exclude />
        /// <summary>
        /// Applies updated parameters from markup to the parent marker.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (_renderer != Renderer)
            {
                _renderer = Renderer;
                Renderer?.OnParentParameterSet();
            }

            if (_visible != Visible)
            {
                _visible = Visible;
                Renderer?.ToggleVisibility();
            }

            if (_name != Name)
            {
                _name = Name ?? string.Empty;
            }

            if (_format != Format)
            {
                _format = Format;
                Renderer?.DatalabelValueChanged();
            }

            if (_fill != Fill)
            {
                _fill = Fill ?? Constants.Transparent;
                Renderer?.DatalabelValueChanged();
            }

            if (_opacity != Opacity)
            {
                _opacity = Opacity;
                Renderer?.DatalabelValueChanged();
            }

            if (_angle != Angle)
            {
                _angle = Angle;
                Renderer?.DatalabelValueChanged();
            }

            if (_enableRotation != EnableRotation)
            {
                _enableRotation = EnableRotation;
                Renderer?.DatalabelValueChanged();
            }

            if (_position != Position)
            {
                _position = Position;
                Renderer?.DatalabelValueChanged();
            }

            if (_horizontalRadius != Rx)
            {
                _horizontalRadius = Rx;
                Renderer?.DatalabelValueChanged();
            }

            if (_verticalRadius != Ry)
            {
                _verticalRadius = Ry;
                Renderer?.DatalabelValueChanged();
            }

            if (_alignment != Alignment)
            {
                _alignment = Alignment;
                Renderer?.DatalabelValueChanged();
            }

            if (!Equals(_border, Border))
            {
                _border = Border;
                if (_border is not null && _border._isPropertyChanged)
                {
                    _border._isPropertyChanged = false;
                    Renderer?.DatalabelValueChanged();
                }
            }

            if (!Equals(_margin, Margin))
            {
                _margin = Margin;
                if (_margin is not null && _margin._isPropertyChanged)
                {
                    _margin._isPropertyChanged = false;
                    Renderer?.DatalabelValueChanged();
                }
            }

            if (!Equals(_font, Font))
            {
                _font = Font;
                if (_font is not null && _font._isPropertyChanged)
                {
                    _font._isPropertyChanged = false;
                    Renderer?.DatalabelValueChanged();
                }
            }

            if (_labelIntersectAction != LabelIntersectAction)
            {
                _labelIntersectAction = LabelIntersectAction;
                Renderer?.DatalabelValueChanged();
            }

            NotifyMarkerOfPropertyUpdate();
        }

        /// <summary>
        /// Disposes this component and its nested subcomponents.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override ValueTask DisposeAsyncCore()
        {
            ComponentDispose();
            return base.DisposeAsyncCore();
        }

        internal void ComponentDispose()
        {
            _marker = null;
            _renderer = null;
            ChildContent = null;
            _margin?.ComponentDispose();
            _margin = null;
            _border?.ComponentDispose();
            _border = null;
            _font?.ComponentDispose();
            _font = null;
            _name = null!;
            _format = null!;
            _labelIntersectAction = "Hide";
            _opacity = 1;
            _angle = 0;
            _enableRotation = false;
            _position = ChartLabelPosition.Auto;
            _horizontalRadius = 5;
            _verticalRadius = 5;
            _alignment = Alignment.Center;
            _visible = false;
            _pendingParametersSetCount = 0;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates nested subcomponent references (Margin, Border, Font).
        /// </summary>
        /// <param name="key">Property name to update: <c>Margin</c>, <c>Border</c>, or <c>Font</c>.</param>
        /// <param name="keyValue">Instance of the subcomponent.</param>
        internal void UpdateDatalabelProperties(string key, object keyValue)
        {
            switch (key)
            {
                case nameof(Margin):
                    Margin = (ChartDataLabelMargin)keyValue;
                    break;
                case nameof(Border):
                    Border = (ChartDataLabelBorder)keyValue;
                    break;
                case nameof(Font):
                    Font = (ChartDataLabelFont)keyValue;
                    break;
                default:
                    break;
            }
        }

        /// <inheritdoc />
        void ISubcomponentTracker.PushSubcomponent()
        {
            _pendingParametersSetCount++;
        }

        /// <inheritdoc />
        void ISubcomponentTracker.PopSubcomponent()
        {
            _pendingParametersSetCount--;
            if (_pendingParametersSetCount == 0)
            {
                _marker?.Series?.Container?._seriesContainer?.Prerender();
            }
        }

        /// <summary>
        /// Copies values from the specified <paramref name="dataLabel"/> into this instance.
        /// </summary>
        /// <param name="dataLabel">Source instance.</param>
        internal void SetDataLableValues(ChartDataLabel dataLabel)
        {
            Visible = dataLabel.Visible;
            Name = dataLabel.Name;
            Fill = dataLabel.Fill;
            Angle = dataLabel.Angle;
            EnableRotation = dataLabel.EnableRotation;
            Position = dataLabel.Position;
            Rx = dataLabel.Rx;
            Ry = dataLabel.Ry;
            Alignment = dataLabel.Alignment;
            Border = dataLabel.Border;
            Margin = dataLabel.Margin;
            Font = dataLabel.Font;
            Template = dataLabel.Template;
            LabelIntersectAction = dataLabel.LabelIntersectAction;
        }

        #endregion

        #region Helper Methods

        private void NotifyMarkerOfPropertyUpdate()
        {
            _marker?.UpdateMarkerProperties("DataLabel", this);
        }

        #endregion
    }
}
