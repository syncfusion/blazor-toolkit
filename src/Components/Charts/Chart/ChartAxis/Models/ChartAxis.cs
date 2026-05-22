using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Charts.Internal;
using Syncfusion.Blazor.Toolkit.Internal;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Represents an axis within a chart, providing customization options for the axis's appearance and behavior.
    /// </summary>
    public class ChartAxis : ChartSubComponent, IChartElement
    {
        #region Private Fields

        private bool _needLayoutUpdate;
        private bool _updateLabels;
        private bool _needAxisRefresh;
        private bool _needSeriesRefresh;
        private bool _updateRange;
        private bool _isIndexed;
        private bool _isInversed;
        private bool _enableTrim;
        private bool _opposedPosition;
        private bool _visible = true;
        private bool _placeNextToAxisLine = true;
        private double _labelRotation;
        private double _interval = double.NaN;
        private double _zoomFactor = 1;
        private double _desiredIntervals = double.NaN;
        private double _zoomPosition;
        private double _startAngle;
        private double _maximumLabelWidth = 34;
        private string _name = string.Empty;
        private string _title = string.Empty;
        private string _labelFormat = string.Empty;
        private string _format = string.Empty;
        private object _minimum = null!;
        private object _maximum = null!;
        private object _crossesAt = null!;

        private ValueType _valueType;
        private IntervalType _intervalType = IntervalType.Auto;
        private LabelPlacement _labelPlacement = LabelPlacement.BetweenTicks;
        private EdgeLabelPlacement _edgeLabelPlacement = EdgeLabelPlacement.Shift;
        private LabelIntersectAction _labelIntersectAction = LabelIntersectAction.Trim;
        private ChartRangePadding _rangePadding = ChartRangePadding.Auto;
        private AxisPosition _labelPosition = AxisPosition.Outside;
        private AxisPosition _tickPosition = AxisPosition.Outside;
        private ChartAxisMajorGridLines PrevMajorGridLines { get; set; } = new ChartAxisMajorGridLines();
        private ChartAxisLabelStyle PrevLabelStyle { get; set; } = new ChartAxisLabelStyle();
        private ChartAxisTitleStyle PrevTitleStyle { get; set; } = new ChartAxisTitleStyle();
        private ChartAxisScrollbarSettings PrevScrollbarSettings { get; set; } = new ChartAxisScrollbarSettings();
        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the parent chart container.
        /// </summary>
        /// <value>A cascading parameter reference to the parent <see cref="SfChart"/> component.</value>
        [CascadingParameter]
        internal SfChart? Container { get; set; }

        /// <summary>
        /// Gets or sets the string representation of the axis value type.
        /// </summary>
        /// <value>The value type as a string.</value>
        internal string? AxisValueType { get; set; }

        /// <summary>
        /// Gets or sets the actual interval type in use.
        /// </summary>
        /// <value>The interval type as a string. Default: "Auto".</value>
        internal string AxisActualIntervalType { get; set; } = "Auto";

        /// <summary>
        /// Gets or sets the string representation of the configured interval type.
        /// </summary>
        /// <value>The interval type as a string.</value>
        internal string? AxisIntervalType { get; set; }

        /// <summary>
        /// Gets or sets the internal visibility state of the axis.
        /// </summary>
        /// <value><see langword="true"/> if visible; otherwise <see langword="false"/>. Default: <see langword="true"/>.</value>
        internal bool InternalVisiblity { get; set; } = true;

        /// <summary>
        /// Gets or sets the renderer responsible for drawing the axis.
        /// </summary>
        /// <value>A <see cref="ChartAxisRenderer"/> instance.</value>
        internal ChartAxisRenderer? Renderer { get; set; }

        /// <summary>
        /// Gets or sets the height of the scrollbar in pixels.
        /// </summary>
        /// <value>The scrollbar height value.</value>
        internal double ScrollBarHeight { get; set; }

        /// <summary>
        /// Gets or sets whether the axis is in inverse rendering mode.
        /// </summary>
        /// <value><see langword="true"/> if inverse; otherwise <see langword="false"/>.</value>
        internal bool IsAxisInverse { get; set; }

        /// <summary>
        /// Gets or sets whether axis label trimming is active.
        /// </summary>
        /// <value><see langword="true"/> if trimming is enabled; otherwise <see langword="false"/>.</value>
        internal bool IsAxisLabelTrim { get; set; }

        /// <summary>
        /// Gets or sets whether the axis is in opposed position mode.
        /// </summary>
        /// <value><see langword="true"/> if opposed; otherwise <see langword="false"/>.</value>
        internal bool IsAxisOpposedPosition { get; set; }

        #endregion

        #region Properties

        /// <summary> 
        /// Gets or sets a value indicating whether the category axis should be rendered using the data source index values. 
        /// </summary> 
        /// <value> 
        /// <b>true</b> if the category axis should be rendered based on the data source index values; otherwise, <b>false</b>. The default value is <b>false</b>. 
        /// </value> 
        /// <remarks> 
        /// If set to <b>true</b>, multiple series will be rendered based on their data source index values. Each series will be aligned based on the index of data points rather than matching the category names.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to enable indexed category axis:
        /// <SfChart>
        ///     <ChartPrimaryXAxis IsIndexed="true" ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
        ///     <ChartSeries DataSource="@WeatherReports1" XName="X" YName="Y" />
        ///     <ChartSeries DataSource="@WeatherReports2" XName="X" YName="Y" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool IsIndexed
        {
            get => _isIndexed;

            set
            {
                if (_isIndexed != value)
                {
                    _isIndexed = value;
                    if (Renderer is not null)
                    {
                        Renderer.Labels.Clear();
                        foreach (ChartSeriesRenderer seriesRenderer in Renderer.SeriesRenderer)
                        {
                            seriesRenderer.InitSeriesRendererFields();
                            seriesRenderer.ProcessData();
                        }

                        _updateRange = !_needLayoutUpdate;
                    }
                }
            }
        }

        /// <summary> 
        /// Gets or sets a value indicating whether the axis should be rendered in an inversed manner. 
        /// </summary> 
        /// <value> 
        /// <b>true</b> if the axis should be rendered in an inversed manner; otherwise, <b>false</b>. 
        /// The default value is <b>false</b>. 
        /// </value> 
        /// <remarks> 
        /// If set to <b>true</b>, the axis will be rendered with the greatest value on the axis moving closer to the origin, and vice versa. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates how to reverse the axis labels for both the x and y axes:
        /// <SfChart>
        ///     <ChartPrimaryXAxis IsInversed="true" />
        ///     <ChartPrimaryYAxis IsInversed="true" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool IsInversed
        {
            get => _isInversed;

            set
            {
                if (_isInversed != value)
                {
                    _isInversed = value;
                    if (Renderer is not null)
                    {
                        Renderer.SetInverseAndOpposedPosition();
                        _needSeriesRefresh = !_needLayoutUpdate;
                    }

                }
            }
        }

        /// <summary> 
        /// Gets or sets the unique identifier name of an axis. 
        /// </summary> 
        /// <value> 
        /// A string representing the unique identifier name of the axis. The default value is an empty string. 
        /// </value> 
        /// <remarks> 
        /// To associate an axis with the series, set this name to the xAxisName/yAxisName properties of the series. 
        /// </remarks>  
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the name of the axis:
        /// <SfChart>
        ///     <ChartAxis OpposedPosition="true" RowIndex="1" Name="YAxis" />
        ///     <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y"/>
        ///     <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y1" YAxisName="YAxis" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string Name
        {
            get => _name;

            set
            {
                if (_name != value)
                {
                    if (Renderer is not null && Container?._axisContainer is not null && Container._axisContainer.Axes.ContainsKey(_name))
                    {
                        Container._axisContainer.Axes.Remove(_name);
                        Container._axisContainer.Axes.TryAdd(value, this);
                        _name = value;
                    }
                    else
                    {
                        _name = value;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to place an axis in the opposite position of its original position.
        /// </summary>
        /// <value>
        /// <b>true</b> if the axis should render at the opposite position; otherwise, <b>false</b>.
        /// The default value is <b>false</b>.
        /// </value>
        /// <remarks>
        /// For a horizontal axis, setting <see cref="OpposedPosition"/> to <b>true</b> will position the axis at the top of the chart.
        /// For a vertical axis, setting <see cref="OpposedPosition"/> to <b>true</b> will position the axis at the right side of the chart.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates how to oppose-position the axis labels for both the x and y axes:
        /// <SfChart>
        ///     <ChartPrimaryXAxis OpposedPosition="true" />
        ///     <ChartPrimaryYAxis OpposedPosition="true" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool OpposedPosition
        {
            get => _opposedPosition;

            set
            {
                if (_opposedPosition != value)
                {
                    _opposedPosition = value;
                    if (Renderer is not null)
                    {
                        Renderer.SetInverseAndOpposedPosition();
                        _needLayoutUpdate = true;
                    }
                }
            }
        }

        /// <summary>  
        /// Gets or sets the title of an axis.  
        /// </summary>  
        /// <value>  
        /// A string representing the title of the axis. The default value is an empty string.  
        /// </value>  
        /// <remarks>  
        /// The axis title provides a quick information to the user about the data plotted in the axis.
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates how to set the title to the axis labels for both the x and y axes:
        /// <SfChart>
        ///     <ChartPrimaryXAxis Title="XAxis" />
        ///     <ChartPrimaryYAxis Title="YAxis" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    if (Renderer is not null && !_needLayoutUpdate)
                    {
                        _needLayoutUpdate = true;
                    }
                }
            }
        }

        /// <summary>  
        /// Gets or sets the type of data the axis is handling.  
        /// </summary> 
        /// <value>  
        /// One of the <see cref="ValueType"/> enumerations that specifies the value type of the axis.  
        /// Options include:  
        /// - <c>Double</c>: Renders a numeric axis.  
        /// - <c>DateTime</c>: Renders a date time axis.  
        /// - <c>Category</c>: Renders a category axis.  
        /// - <c>Logarithmic</c>: Renders a logarithmic axis.
        /// - <c>DateTimeCategory</c> : Renders a date time category axis.
        /// <br/>
        /// The default value is <b>ValueType.Double</b>.  
        /// </value> 
        /// <remarks>  
        /// This property determines the type of data the axis is handling, and it affects the way axis values are interpreted and displayed on the chart.  
        /// It is essential to set this property correctly based on the nature of the data being plotted.  
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates how to set the value type to the axis:
        /// <SfChart>
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"/>
        ///     <ChartSeries DataSource="@MedalDetails" XName="Country" YName="Gold"/>
        /// </SfChart>
        /// @code{
        ///     public class ChartData
        ///     {
        ///         public string Country { get; set;}
        ///         public double Gold {get; set;}
        ///     }
        ///     public List<ChartData> MedalDetails = new List<ChartData>
        ///     {
        ///         new ChartData{ Country = "USA", Gold = 50 },
        ///         new ChartData{ Country = "China", Gold = 40 },
        ///         new ChartData{ Country = "Japan", Gold = 70 }
        ///     };
        /// }
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ValueType ValueType
        {
            get => _valueType;
            set
            {
                if (_valueType != value)
                {
                    if (Container is null)
                    {
                        _valueType = value;
                    }

                    if (Container is not null && Container._axisContainer is not null)
                    {
                        ChartAxisRenderer renderer = Container._axisContainer.Renderers.Find(renderer => renderer.GetType().Equals(ChartAxisRenderer.GetRendererType(_valueType)) && (renderer as ChartAxisRenderer)?.Axis?.Name == this.Name) as ChartAxisRenderer ?? null!;
                        Container._axisContainer.RemoveRenderer(renderer);
                        _valueType = value;
                        RendererType = ChartAxisRenderer.GetRendererType(_valueType);
                        Container._axisContainer.RendererShouldRender = true;
                        Container._axisContainer.Prerender();
                    }
                }
            }
        }

        /// <summary>  
        /// Gets or sets a value indicating whether the axis elements such as axis labels, tick lines, grid lines, and axis title should be visible.  
        /// </summary>  
        /// <value>  
        /// <b>true</b> if the axis and its associated elements are visible; otherwise, <b>false</b>.  
        /// The default value is <b>true</b>.  
        /// </value>  
        /// <remarks>  
        /// Set this property to control the visibility of various axis elements. When set to <b>false</b>, the axis and its associated elements will not be rendered on the chart.  
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates how to set the visibility of the axis:
        /// <SfChart>
        ///     <ChartPrimaryXAxis Visible="true" />
        ///     <ChartPrimaryYAxis Visible="false" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool Visible
        {
            get => _visible;

            set
            {
                if (_visible != value)
                {
                    _visible = InternalVisiblity = value;
                    if (Renderer is not null && !_needLayoutUpdate)
                    {
                        _needLayoutUpdate = true;
                    }
                }
            }
        }

        /// <summary> 
        /// Gets or sets the radius for polar and radar series charts. 
        /// </summary> 
        /// <value> 
        /// The double value represents the radius for polar and radar series charts. The default value is <b>100</b>. 
        /// </value> 
        /// <remarks> 
        /// Applicable only for Polar and Radar series type charts.  
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates how to set the radius for polar and radar series charts:
        /// <SfChart>
        ///     <ChartPrimaryXAxis Coefficient="60" />
        ///     <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y" Type="ChartSeriesType.Polar" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double Coefficient { get; set; } = 100;

        /// <summary> 
        /// Gets or sets the index of the column with which the axis is associated. 
        /// </summary> 
        /// <value> 
        /// The index of the column associated with the axis. The default value is <b>0</b>.
        /// </value> 
        /// <remarks> 
        /// This property is applicable only when the chart area is divided into multiple plot areas using the <see cref="ChartColumns"/>.
        /// To bind a horizonatal axis to a specific column, set the axis’s ColumnIndex property to that column’s index. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a divided chart area with multiple columns and binding a horizontal axis to a specific column:
        /// <SfChart>
        ///     <ChartColumns>
        ///         <ChartColumn Width="50%" />
        ///         <ChartColumn Width="50%" />
        ///     </ChartColumns>
        ///     <ChartAxis ColumnIndex="1" Name="XAxis" />
        ///     <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y" />
        ///     <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y1" XAxisName="XAxis" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double ColumnIndex { get; set; }

        /// <summary> 
        /// Gets or sets the value (numeric, datetime, or logarithmic) at which the horizontal axis line intersects with the vertical axis or vice versa. 
        /// </summary> 
        /// <value> 
        /// The value at which the horizontal axis line intersects with the vertical axis or vice versa. The default value is <b>null</b>.
        /// </value> 
        /// <remarks> 
        /// If the <see cref="ValueType"/> of the horizontal axis is <b>ValueType.Category</b>, the <see cref="CrossesAt"/> value for the vertical axis is accepted as a numeric value, which will be considered as the index of horizontal axis labels. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to create a chart with a secondary Y-axis that crosses at zero:
        /// <SfChart>
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
        ///     <ChartAxis Minimum="-10" Maximum="10" Interval="1" OpposedPosition="true" RowIndex="1" Name="YAxis" CrossesAt="0" />
        ///     <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y" Type="ChartSeriesType.Column" />
        ///     <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y1" YAxisName="YAxis" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public object CrossesAt
        {
            get => _crossesAt;

            set
            {
                if (_crossesAt is null || !_crossesAt.Equals(value))
                {
                    _crossesAt = value;
                    if (Renderer is not null && Renderer.CrossInAxis is not null && !_needLayoutUpdate)
                    {
                        Renderer.UpdateCrossValue();
                        if (Renderer.CrossInAxis.Renderer is not null && Renderer.IsInside(Renderer.CrossInAxis.Renderer.VisibleRange, Renderer.CrossAt))
                        {
                            _needAxisRefresh = true;
                        }
                        else
                        {
                            _needLayoutUpdate = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of the axis with which the axis line is to be crossed.
        /// </summary>
        /// <value>
        /// The name of the axis with which the axis line is crossed.
        /// </value>
        /// <remarks>
        /// Applicable only when <see cref="CrossesAt"/> is provided with a value.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to create a chart with a secondary Y-axis named "YAxis" that crosses the primary X-axis at zero:
        /// <SfChart>
        ///     <ChartAxis Minimum="-10" Maximum="10" Interval="1" OpposedPosition="true" RowIndex="1" Name="YAxis" CrossesAt="0" />
        ///     <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y" Type="ChartSeriesType.Column" />
        ///     <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y1" YAxisName="YAxis" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string CrossesInAxis { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets the description about the axis and its elements for accessibility purposes. 
        /// </summary> 
        /// <value> 
        /// A string representing the description about the axis and its elements for accessibility. The default value is an empty string.  
        /// </value>
        /// <remarks> 
        /// Accepts the values in string.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the description for the primary X-axis in a chart:
        /// <SfChart>
        ///     <ChartPrimaryXAxis Description="Primary X-Axis" />
        ///     <ChartPrimaryYAxis Description="Primary Y-Axis" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string Description { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets a value that indicates the approximate interval count for axis interval calculation. 
        /// </summary> 
        /// <value> 
        /// The desired interval count for axis interval calculation. The default value is <see cref="double.NaN"/>. 
        /// </value> 
        /// <remarks> 
        /// Not applicable when <see cref="ValueType"/> of the axis is <b>ValueType.Category</b>. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates how to set the desired number of intervals on the X-axis:
        /// <SfChart>
        ///     <ChartPrimaryXAxis DesiredIntervals="1" />
        ///     <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double DesiredIntervals
        {
            get => _desiredIntervals;

            set
            {
                if (_desiredIntervals != value && !(double.IsNaN(_desiredIntervals) && double.IsNaN(value)))
                {
                    _desiredIntervals = value;
                    if (Renderer is not null && !_needLayoutUpdate && !double.IsNaN(Interval))
                    {
                        Renderer.VisibleInterval = Renderer.ActualInterval = Renderer.CalculateActualInterval(Renderer.ActualRange);
                        _updateLabels = true;
                    }
                }
            }
        }

        /// <summary> 
        /// Gets or sets the placement of labels at the edge of the axis. 
        /// </summary> 
        /// <value> 
        /// One of the <see cref="EdgeLabelPlacement"/> enumerations that specify the edge label placement for the axis. 
        /// The options include: 
        /// - <c>None</c>: No action will be performed, and the label will be rendered as it is. 
        /// - <c>Hide</c>: Hides the edge label if it exceeds the chart area. 
        /// - <c>Shift</c>: Shifts the edge labels within the chart area. 
        /// <br/> 
        /// The default mode is <c>EdgeLabelPlacement.None</c>. 
        /// </value> 
        /// <remarks> 
        /// The longer text labels at the axis edges may only be partially visible in the chart. Utilize this property to place the edge labels efficiently for a better user experience. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to hide edge labels on the primary axis in a chart:
        /// <SfChart>
        ///     <ChartPrimaryXAxis EdgeLabelPlacement="EdgeLabelPlacement.Hide" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public EdgeLabelPlacement EdgeLabelPlacement
        {
            get => _edgeLabelPlacement;

            set
            {
                if (_edgeLabelPlacement != value)
                {
                    _edgeLabelPlacement = value;
                    if (Renderer is not null && !_needLayoutUpdate)
                    {
                        _needAxisRefresh = true;
                    }
                }
            }
        }

        /// <summary> 
        /// Gets or sets a value indicating whether the axis interval should be calculated automatically with respect to the zoomed range. 
        /// </summary> 
        /// <value> 
        /// <b>true</b> if automatic interval calculation is enabled during zooming; otherwise, <b>false</b>. 
        /// The default value is <b>true</b>. 
        /// </value> 
        /// <remarks> 
        /// This property affects axis intervals only when the chart is zoomed. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the EnableAutoIntervalOnZooming for the axis:
        /// <SfChart>
        ///     <ChartPrimaryXAxis EnableAutoIntervalOnZooming="true/>
        ///     <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y"/>
        ///     <ChartZoomSettings EnableSelectionZooming="true"></ChartZoomSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool EnableAutoIntervalOnZooming { get; set; } = true;

        /// <summary> 
        /// Gets or sets a value indicating whether the scrollbar for zooming is enabled. 
        /// </summary> 
        /// <value> 
        /// <b>true</b> if the scrollbar for zooming is enabled; otherwise, <b>false</b>. 
        /// The default value is <b>true</b>. 
        /// </value> 
        /// <remarks> 
        /// If set to <b>true</b>, the axis will be rendered with a scrollbar when the chart is zoomed. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the EnableScrollbarOnZooming for the axis:
        /// <SfChart>
        ///     <ChartPrimaryXAxis EnableScrollbarOnZooming="true" />
        ///     <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y"/>
        ///     <ChartZoomSettings EnableSelectionZooming="true"></ChartZoomSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool EnableScrollbarOnZooming { get; set; } = true;

        /// <summary> 
        /// Gets or sets a value indicating whether axis labels should be trimmed when they exceed the <see cref="MaximumLabelWidth"/>. 
        /// </summary> 
        /// <value> 
        /// <b>true</b> if axis labels should be trimmed; otherwise, <b>false</b>. The default value is <b>false</b>. 
        /// </value> 
        /// <remarks> 
        /// If set to <b>true</b>, axis labels which exceed the <see cref="MaximumLabelWidth"/> will be trimmed regardless of the <see cref="LabelIntersectAction"/> applied. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to trim long labels on the axis and restrict them to a maximum width in a chart:
        /// <SfChart>
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" MaximumLabelWidth="20" EnableTrim="true" />
        ///     <ChartSeries DataSource="@MedalDetails" XName="X" YName="YValue" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool EnableTrim
        {
            get => _enableTrim;

            set
            {
                if (_enableTrim != value)
                {
                    _enableTrim = value;
                    if (Renderer is not null && !_needLayoutUpdate)
                    {
                        _updateLabels = true;
                    }
                }
            }
        }

        /// <summary> 
        /// Gets or sets the interval type for the DateTime-based axis. 
        /// </summary> 
        /// <value> 
        /// One of the <see cref="IntervalType"/> enumerations that specifies the interval type for the DateTime-based axis. 
        /// The options include: 
        /// - <c>Auto</c>: Defines the interval of the axis based on data. 
        /// - <c>Years</c>: Defines the interval of the axis in years. 
        /// - <c>Months</c>: Defines the interval of the axis in months. 
        /// - <c>Days</c>: Defines the interval of the axis in days. 
        /// - <c>Hours</c>: Defines the interval of the axis in hours. 
        /// - <c>Minutes</c>: Defines the interval of the axis in minutes. 
        /// <br/>
        /// The default value is <b>IntervalType.Auto</b>. 
        /// </value> 
        /// <remarks>
        /// Choosing an appropriate interval type is essential for displaying DateTime data effectively.
        /// For instance, using a larger interval such as <c>Years</c> or <c>Months</c> may be more suitable
        /// for data spanning extended periods, while shorter intervals like <c>Hours</c> or <c>Minutes</c> are better
        /// suited for data requiring more detailed temporal granularity. Adjusting the interval type helps in achieving
        /// clarity and precision in your chart’s time representation.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to configure the axis to display DateTime values and to show labels at monthly intervals in a Syncfusion Blazor chart:
        /// <SfChart>
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.Charts.ValueType.DateTime" IntervalType="IntervalType.Months" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public IntervalType IntervalType
        {
            get => _intervalType;

            set
            {
                if (_intervalType != value)
                {
                    _intervalType = value;
                    if (Renderer is not null)
                    {
                        _updateRange = true;
                    }
                }
            }
        }

        /// <summary> 
        /// Gets or sets the interval for the axis. 
        /// </summary> 
        /// <value> 
        /// The numeric value representing the interval for the axis. The default value is <see cref="double.NaN"/>. 
        /// </value> 
        /// <remarks> 
        /// The interval can be customized using <see cref="IntervalType"/> for DateTime-based axis and <see cref="LogBase"/> for logarithmic-based axis. 
        /// For example, if the interval is set to 2 and the <see cref="IntervalType"/> is set to Years, it considers 2 years to be the interval. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set a custom interval of 2 for the axis in a chart:
        /// <SfChart>
        ///     <ChartPrimaryXAxis Interval="2" />
        ///     <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double Interval
        {
            get => _interval;

            set
            {
                if (_interval != value && !(double.IsNaN(_interval) && double.IsNaN(value)))
                {
                    _interval = value;
                    if (Renderer is not null)
                    {
                        Renderer.VisibleInterval = value;
                        _updateLabels = true;
                    }
                }
            }
        }

        /// <summary> 
        /// Gets or sets the minimum range of an axis. 
        /// </summary> 
        /// <value> 
        /// The value representing the minimum range of an axis. The default value is derived from the values of corresponding data source points. 
        /// </value> 
        /// <remarks> 
        /// This property specifies the minimum range for the axis. It determines the lower limit of the visible range on the axis.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set a custom minimum value for the axis in a Syncfusion Blazor chart:
        /// <SfChart>
        ///     <ChartPrimaryXAxis Minimum="-10" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public object Minimum
        {
            get => _minimum;

            set
            {
                if (_minimum is null || !_minimum.Equals(value))
                {
                    _minimum = value;
                    if (Renderer is not null && value is not null)
                    {
                        double start = (Renderer.Axis?.ValueType == ValueType.DateTime) ? (Renderer as DateTimeAxisRenderer ?? null!).GetTime((DateTime)value) : Convert.ToDouble(value, null);
                        if (Renderer.NeedAxisLayoutChange(start, Renderer.VisibleRange.End))
                        {
                            _needLayoutUpdate = true;
                        }
                        else
                        {
                            Renderer.VisibleRange = new DoubleRange(start, Renderer.VisibleRange.End);
                            _updateLabels = _updateRange = true;
                        }
                    }
                }
            }
        }

        /// <summary> 
        /// Gets or sets a value that specifies the placement of axis labels with respect to the axis tick lines. 
        /// </summary> 
        /// <value> 
        /// One of the <see cref="LabelPlacement"/> enumerations that specifies the placement for the axis labels. 
        /// The options include: 
        /// - <c>BetweenTicks</c>: Renders the label between the ticks. 
        /// - <c>OnTicks</c>: Renders the label on the ticks.
        /// <br/>
        /// The default value is <b>LabelPlacement.BetweenTicks</b>. 
        /// </value> 
        /// <remarks>
        /// The placement of axis labels can have a significant impact on the readability and accuracy of the chart.
        /// When labels are placed <c>BetweenTicks</c>, they are centered between tick marks, often used to represent
        /// categories or ranges accurately. Alternatively, when labels are placed <c>OnTicks</c>, they align directly
        /// with the tick marks, which can be beneficial for pinpointing specific values or time points.
        /// Choosing the appropriate label placement contributes to better data comprehension and presentation.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to position the axis labels on the tick:
        /// <SfChart>
        ///     <ChartPrimaryXAxis LabelPlacement="LabelPlacement.OnTicks" ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
        ///     <ChartSeries DataSource="@MedalDetails" XName="X" YName="Y" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public LabelPlacement LabelPlacement
        {
            get => _labelPlacement;

            set
            {
                if (_labelPlacement != value && !_needLayoutUpdate)
                {
                    _labelPlacement = value;
                    if (Renderer is not null)
                    {
                        _updateRange = true;
                    }
                }
            }
        }

        /// <summary> 
        /// Gets or sets the maximum range of an axis. 
        /// </summary> 
        /// <value> 
        /// The value representing the maximum range of an axis. The default value is derived from the values of corresponding data source points. 
        /// </value> 
        /// <remarks> 
        /// This property specifies the maximum range for the axis. It determines the upper limit of the visible range on the axis.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set a maximum value for the axis in chart:
        /// <SfChart>
        ///     <ChartPrimaryXAxis Maximum="20" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public object Maximum
        {
            get => _maximum;

            set
            {
                if (_maximum is null || !_maximum.Equals(value))
                {
                    _maximum = value;
                    if (Renderer is not null && value is not null)
                    {
                        double end = (Renderer.Axis?.ValueType == ValueType.DateTime) ? (Renderer as DateTimeAxisRenderer ?? null!).GetTime((DateTime)value) : Convert.ToDouble(value, null);
                        if (Renderer.NeedAxisLayoutChange(Renderer.VisibleRange.Start, end))
                        {
                            _needLayoutUpdate = true;
                        }
                        else
                        {
                            Renderer.VisibleRange = new DoubleRange(Renderer.VisibleRange.Start, end);
                            _updateLabels = _updateRange = true;
                        }
                    }
                }
            }
        }

        /// <summary>  
        /// Gets or sets the scaling factor for the axis during zoom operations.  
        /// </summary>  
        /// <value>  
        /// The scaling factor for the axis, which ranges from 0 to 1.
        /// The default value is <b>1</b>.  
        /// </value>  
        /// <remarks>  
        /// When <see cref="ZoomFactor"/> is set to 0.5, the chart is scaled by 200% along this axis.  
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the ZoomFactor of the axis to zoom into the chart:
        /// <SfChart>
        ///     <ChartPrimaryXAxis ZoomFactor="0.5" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double ZoomFactor
        {
            get => _zoomFactor;

            set
            {
                if (_zoomFactor != value)
                {
                    bool isMouseDown = false;
                    bool isRefresh = false;

                    if (Container is not null)
                    {
                        isMouseDown = false;
                        isRefresh = false;
                    }

                    if (!isMouseDown || isRefresh)
                    {
                        _zoomFactor = value;
                        if (Renderer is not null && Container is not null && Container._isChartFirstRender)
                        {
                            Renderer.VisibleRange = Renderer.CalculateVisibleRange(Renderer.ActualRange);
                            _updateLabels = true;
                            if (Container._zoomSettings is not null && Container._zoomSettings.EnablePan)
                            {
                                _ = Container.DelayLayoutChangeAsync(true);
                            }
                        }
                    }
                    if (Container is not null)
                    {
                        Container._selectionModule?.ClearDraggedRects();
                        Container._selectionModule?.OnPropertyChanged();
                        Container._parentRect?.ClearElements();
                        if (Renderer is not null)
                        {
                            _ = Container.ProcessOnLayoutChangeAsync();
                        }
                    }
                }
            }
        }

        /// <summary>  
        /// Gets or sets the position of the zoomed axis during zoom operations.  
        /// </summary>  
        /// <value>  
        /// The position of the zoomed axis, which ranges from 0 to 1.
        /// </value>  
        /// <remarks>  
        /// The ZoomPosition property determines the relative position of the zoomed axis during zoom operations. 
        /// A value of 0 represents the start of the axis, and a value of 1 represents the end of the axis. 
        /// This property is used in conjunction with the <see cref="ZoomFactor"/> property to control the zoom behavior along the axis. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to configure the ZoomFactor and ZoomPosition for the X-axis:
        /// <SfChart>
        ///     <ChartPrimaryXAxis ZoomFactor="0.5" ZoomPosition="0.6" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double ZoomPosition
        {
            get => _zoomPosition;

            set
            {
                if (_zoomPosition != value)
                {
                    bool isMouseDown = false;
                    bool isRefresh = false;

                    if (Container is not null)
                    {
                        isMouseDown = false;
                        isRefresh = false;
                    }

                    if (!isMouseDown || isRefresh)
                    {
                        _zoomPosition = value;
                        if (Renderer is not null && Container is not null && Container._isChartFirstRender)
                        {
                            Renderer.VisibleRange = Renderer.CalculateVisibleRange(Renderer.ActualRange);
                            _updateLabels = true;
                            if (Container._zoomSettings is not null && Container._zoomSettings.EnablePan)
                            {
                                _ = Container.DelayLayoutChangeAsync(true);
                            }
                        }
                    }
                }
            }
        }

        /// <summary> 
        /// Gets or sets a value that is used to format the numeric, logarithmic, and datetime labels to all globalize formats. 
        /// Axis also supports custom label format using placeholders such as {value}K, where the value represents the axis label. 
        /// </summary> 
        /// <value> 
        /// A string representing the format for processing numeric, logarithmic, and datetime labels. 
        /// </value> 
        /// <remarks> 
        /// The <see cref="LabelFormat"/> supports standard and custom formatting strings.
        /// For numeric values, formats like "C", "N", "P", etc., can be applied.
        /// For datetime values, formats like "d", "D", "t", etc., are supported.
        /// The property is not applicable to category axis labels.
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to apply a currency format to the axis labels:
        /// <SfChart>
        ///     <ChartPrimaryXAxis LabelFormat="c" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string LabelFormat
        {
            get => _labelFormat;

            set
            {
                if (_labelFormat != value)
                {
                    _labelFormat = value;
                    if (Renderer is not null && !_needLayoutUpdate)
                    {
                        if (Renderer.Orientation == Orientation.Vertical)
                        {
                            _needLayoutUpdate = true;
                        }
                        else
                        {
                            _updateLabels = true;
                        }
                    }
                }
            }
        }

        /// <summary> 
        /// Gets or sets a value that specifies how to arrange axis labels intelligently when they intersect with each other. 
        /// </summary> 
        /// <value> 
        /// One of the <see cref="LabelIntersectAction"/> enumerations that specifies the alignment of axis labels when they intersect. 
        /// The options include: 
        /// - <c>None</c>: Show all labels, regardless of intersections. 
        /// - <c>Hide</c>: Hide labels that intersect with each other. 
        /// - <c>Trim</c>: Trim labels to fit within the available space when they intersect. 
        /// - <c>Wrap</c>: Wrap labels to multiple lines when they intersect. 
        /// - <c>MultipleRows</c>: Display labels in multiple rows when they intersect. 
        /// - <c>Rotate45</c>: Rotate labels by 45 degrees when they intersect. 
        /// - <c>Rotate90</c>: Rotate labels by 90 degrees when they intersect. 
        /// <br/>
        /// The default LabelIntersectAction is <b>LabelIntersectAction.Trim</b>.  
        /// </value> 
        /// <remarks> 
        /// Note: For the vertical axis, the <see cref="LabelIntersectAction.Hide"/> option is the only applicable choice. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to apply a rotate 45 degrees to the axis labels:
        /// <SfChart>
        ///     <ChartPrimaryXAxis LabelIntersectAction="LabelIntersectAction.Rotate45" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public LabelIntersectAction LabelIntersectAction
        {
            get => _labelIntersectAction;

            set
            {
                if (_labelIntersectAction != value)
                {
                    _labelIntersectAction = value;
                    if (Renderer is not null && !_needLayoutUpdate)
                    {
                        _needLayoutUpdate = true;
                    }
                }
            }
        }

        /// <summary> 
        /// Gets or sets the padding for axis labels from the axis line in pixels. 
        /// </summary> 
        /// <value> 
        /// The numeric value representing the label padding from the axis line in pixels. The default value is <b>5</b>. 
        /// </value> 
        /// <remarks>
        /// Increasing the padding can prevent labels from overlapping with the axis line, ensuring clear visibility.
        /// Conversely, reducing the padding can create a more compact layout, useful for fitting charts into
        /// limited spaces. It is important to consider the overall design and label size when setting this value to maintain
        /// a clean and readable chart display.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to add padding to the axis labels in the X axis.
        /// // LabelPadding property is used to move the labels away from the axis line.
        /// <SfChart>
        ///     <ChartPrimaryXAxis LabelPadding="20"></ChartPrimaryXAxis>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double LabelPadding { get; set; } = 5;

        /// <summary> 
        /// Gets or sets the position of axis labels relative to the axis line. 
        /// </summary> 
        /// <value> 
        /// One of the <see cref="AxisPosition"/> enumerations that specifies the position of labels relative to the axis line. 
        /// The options include: 
        /// - <c>Inside</c>: Renders the labels inside the axis line. 
        /// - <c>Outside</c>: Renders the labels outside the axis line. 
        /// <br/>
        /// The default value is <b>AxisPosition.Outside</b>. 
        /// </value> 
        /// <remarks>
        /// Placing labels inside the axis line (<c>Inside</c>) might be beneficial when you have limited space or when you want a minimal chart appearance.
        /// Conversely, placing labels outside the axis line (<c>Outside</c>) can enhance visibility and separation from the chart area, particularly in situations where overlapping with data points or elements might occur.
        /// It is important to consider the overall chart layout and data presentation needs when setting this property.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to position the axis labels inside the chart area.
        /// <SfChart>
        ///     <ChartPrimaryXAxis LabelPosition="AxisPosition.Inside" />
        ///     <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public AxisPosition LabelPosition
        {
            get => _labelPosition;

            set
            {
                if (_labelPosition != value && !_needLayoutUpdate)
                {
                    _labelPosition = value;
                    _needLayoutUpdate = Renderer is not null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the angle at which the axis labels are rotated.
        /// </summary>
        /// <value>
        /// A double value representing the rotation angle of the axis labels. The default value is <b>0</b>.
        /// </value>
        /// <remarks>
        /// This property specifies the rotation angle applied to axis labels, which can enhance their orientation and improve readability.
        /// It accepts numerical values ranging from 0 to 360 degrees. Negative values indicate angles that are calculated in a opposite direction, starting from 360 degrees.
        /// Setting this property allows for customizing how the labels are presented along the axis, accommodating various layout needs.
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to rotate the axis labels on the axis.
        /// <SfChart>
        ///     <ChartPrimaryXAxis LabelRotation="45" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double LabelRotation
        {
            get => _labelRotation;

            set
            {
                if (_labelRotation != value)
                {
                    _labelRotation = value;
                    if (Renderer is not null && !_needLayoutUpdate)
                    {
                        _needLayoutUpdate = true;
                    }
                }
            }
        }

        /// <summary> 
        /// Gets or sets an instance of <see cref="ChartAxisLabelStyle"/> that specifies the text style of axis labels. 
        /// </summary> 
        /// <value> 
        /// An instance of <see cref="ChartAxisLabelStyle"/>. 
        /// </value> 
        /// <remarks> 
        /// This property can be used to customize the color and font properties such as size, font-family, font-weight and font-style of axis labels. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to customize the axis label style for the primary X-axis.
        /// <SfChart>
        ///     <ChartPrimaryXAxis>
        ///         <ChartAxisLabelStyle 
        ///             Color="blue" 
        ///             FontFamily="italic" 
        ///             FontWeight="400" 
        ///             FontStyle="Normal" 
        ///             Size="12px" />
        ///     </ChartPrimaryXAxis>
        ///     <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ChartAxisLabelStyle LabelStyle
        {
            get => PrevLabelStyle;

            set
            {
                if (PrevLabelStyle != value)
                {
                    PrevLabelStyle = value;
                    if (PrevLabelStyle._isPropertyChanged)
                    {
                        PrevLabelStyle._isPropertyChanged = false;
                        _ = Container?.ProcessOnLayoutChangeAsync();
                        return;
                    }
                    Renderer?.CustomizeGridRenderingOptions(nameof(LabelStyle));
                    Renderer?.ProcessRenderQueue();
                }
            }
        }

        /// <summary> 
        /// Gets or sets an instance of <see cref="ChartAxisLineStyle"/> that controls the customization of axis line. 
        /// </summary> 
        /// <value> 
        /// An instance of <see cref="ChartAxisLineStyle"/>. 
        /// </value> 
        /// <remarks> 
        /// This property can be used to customize the color, width, and dash array of the axis line. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to customize the axis line style for the primary axis in a chart.
        /// <SfChart>
        ///     <ChartPrimaryXAxis>
        ///         <ChartAxisLineStyle 
        ///             Width="2" 
        ///             Color="red" 
        ///             DashArray="5,1" />
        ///     </ChartPrimaryXAxis>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ChartAxisLineStyle LineStyle { get; set; } = new ChartAxisLineStyle();

        /// <summary> 
        /// Gets or sets the base value for the logarithmic axis. 
        /// </summary> 
        /// <value> 
        /// A numeric value representing the base value for the logarithmic axis. The default value is <b>10</b>.
        /// </value> 
        /// <remarks> 
        /// This property is applicable only when the <see cref="ValueType"/> of the axis is set to <see cref="ValueType.Logarithmic"/>. 
        /// For example, if the logarithmic base is 10 and the <see cref="Interval"/> is 2, the axis labels will be placed at intervals of 10^2.
        /// Adjusting this value is crucial for ensuring appropriate scale representation in logarithmic value type axis.
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to configure a logarithmic axis for the primary axis in a chart.
        /// // The axis uses a logarithmic scale with a base of 20 to display data spanning multiple magnitudes.
        /// <SfChart>
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.Charts.ValueType.Logarithmic" LogBase="20" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example> 
        [Parameter]
        public double LogBase { get; set; } = 10;

        /// <summary> 
        /// Gets or sets an instance of <see cref="ChartAxisMajorGridLines"/> that controls the customization of major grid lines. 
        /// </summary> 
        /// <value> 
        /// An instance of <see cref="ChartAxisMajorGridLines"/>. 
        /// </value> 
        /// <remarks> 
        /// This property can be used to customize the color, width, and dash array of the major grid lines. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a customize for the axis major gridlines.
        /// <SfChart>
        ///     <ChartPrimaryXAxis>
        ///         <ChartAxisMajorGridLines DashArray="5,1" />
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis>
        ///         <ChartAxisMajorGridLines DashArray="5,1" />
        ///     </ChartPrimaryYAxis>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ChartAxisMajorGridLines MajorGridLines
        {
            get => PrevMajorGridLines;

            set
            {
                if (PrevMajorGridLines != value)
                {
                    PrevMajorGridLines = value;
                    Renderer?.CustomizeGridRenderingOptions(nameof(MajorGridLines));
                    Renderer?.ProcessRenderQueue();
                }
            }
        }

        /// <summary> 
        /// Gets or sets an instance of <see cref="ChartAxisMajorTickLines"/> that controls the customization of major tick lines. 
        /// </summary> 
        /// <value> 
        /// An instance of <see cref="ChartAxisMajorTickLines"/>. 
        /// </value> 
        /// <remarks> 
        /// This property can be used to customize the color, width, and height of the major tick lines. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a customized for the axis major tick lines.
        /// <SfChart>
        ///     <ChartPrimaryXAxis>
        ///         <ChartAxisMajorTickLines Color="red" />
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis>
        ///         <ChartAxisMajorTickLines Color="blue" />
        ///     </ChartPrimaryYAxis>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ChartAxisMajorTickLines MajorTickLines { get; set; } = new ChartAxisMajorTickLines();

        /// <summary> 
        /// Gets or sets the maximum width for an axis label. 
        /// </summary> 
        /// <value> 
        /// A numeric value representing the maximum allowable width for an axis label, measured in pixels.
        /// The default value is <b>34</b>. 
        /// </value> 
        /// <remarks> 
        /// When <see cref="EnableTrim"/> is set to <b>true</b>, axis labels that exceed the specified <see cref="MaximumLabelWidth"/> will be trimmed to fit within this limit.
        /// </remarks>  
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to trim long labels on the axis and restrict them to a maximum width in a chart.
        /// <SfChart>
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" MaximumLabelWidth="20" EnableTrim="true" />
        ///     <ChartSeries DataSource="@MedalDetails" XName="X" YName="YValue" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        [DefaultValue(34)]
        public double MaximumLabelWidth
        {
            get => _maximumLabelWidth;

            set
            {
                if (_maximumLabelWidth != value)
                {
                    _maximumLabelWidth = value;
                    if (Renderer is not null && !_needLayoutUpdate)
                    {
                        _updateLabels = true;
                    }
                }
            }
        }

        /// <summary> 
        /// Gets or sets the maximum number of labels per 100 pixels relative to the axis length.
        /// </summary> 
        /// <value> 
        /// A numeric value representing the maximum label count. The default value is <b>3</b>.
        /// </value> 
        /// <remarks>
        /// Adjusting this value can help optimize the readability and clutter of axis labels.
        /// </remarks>  
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to apply a maximum label count to the primary axis in a chart.
        /// <SfChart>
        ///     <ChartPrimaryXAxis MaximumLabels="5" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double MaximumLabels { get; set; } = 3;

        /// <summary> 
        /// Gets or sets an instance of <see cref="ChartAxisMinorGridLines"/> that controls the customization of minor grid lines. 
        /// </summary> 
        /// <value> 
        /// An instance of <see cref="ChartAxisMinorGridLines"/>. 
        /// </value> 
        /// <remarks> 
        /// This property can be used to customize the color, width, and dash array of the minor grid lines.
        /// <br/>
        /// Note: Minor grid lines will be rendered only when <see cref="MinorTicksPerInterval"/> is provided with a value greater than zero.
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a customized style for the axis minor grid lines.
        /// <SfChart>
        ///     <ChartPrimaryXAxis MinorTicksPerInterval="1">
        ///         <ChartAxisMinorGridLines Color="red" />
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis MinorTicksPerInterval="1">
        ///         <ChartAxisMinorGridLines Color="blue" />
        ///     </ChartPrimaryYAxis>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ChartAxisMinorGridLines MinorGridLines { get; set; } = new ChartAxisMinorGridLines();

        /// <summary> 
        /// Gets or sets an instance of <see cref="ChartAxisMinorTickLines"/> that controls the customization of minor tick lines. 
        /// </summary> 
        /// <value> 
        /// An instance of <see cref="ChartAxisMinorTickLines"/>. 
        /// </value> 
        /// <remarks> 
        /// This property can be used to customize the color, width, and dash array of the minor tick lines.
        /// <br/>
        /// Note: Minor tick lines will be rendered only when <see cref="MinorTicksPerInterval"/> is provided with a value greater than zero. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a custom style for the axis minor tick lines.
        /// <SfChart>
        ///     <ChartPrimaryXAxis MinorTicksPerInterval="1">
        ///         <ChartAxisMinorTickLines Color="red" />
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis MinorTicksPerInterval="1">
        ///         <ChartAxisMinorTickLines Color="blue" />
        ///     </ChartPrimaryYAxis>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ChartAxisMinorTickLines MinorTickLines { get; set; } = new ChartAxisMinorTickLines();

        /// <summary> 
        /// Gets or sets the number of minor ticks to render per interval.
        /// </summary> 
        /// <value> 
        /// A numeric value representing the number of minor ticks per interval.
        /// </value> 
        /// <remarks> 
        /// Note: Minor grid lines and minor tick lines are rendered only when this property is set to a value greater than zero.
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a custom style for the axis minor tick lines and grid line.
        /// <SfChart>
        ///     <ChartPrimaryXAxis MinorTicksPerInterval="1" />
        ///     <ChartPrimaryYAxis MinorTicksPerInterval="1" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double MinorTicksPerInterval { get; set; }

        /// <summary> 
        /// Gets or sets a value indicating whether axis elements, such as axis labels and axis titles, should be placed next to the axis line when an axis crossing occurs.
        /// </summary> 
        /// <value> 
        /// <b>true</b> if axis elements should be placed next to the axis line; otherwise, <b>false</b>.
        /// The default value is <b>true</b>. 
        /// </value>
        /// <remarks>
        /// This property applies only when an axis crossing occurs.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to create a chart with a secondary Y-axis that crosses at zero and is placed next to the axis line.
        /// <SfChart>
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
        ///     <ChartAxis Minimum="-10" Maximum="10" Interval="1" OpposedPosition="true" RowIndex="1" Name="YAxis" CrossesAt="0" PlaceNextToAxisLine="true" />
        ///     <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y" Type="ChartSeriesType.Column" />
        ///     <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y1" YAxisName="YAxis" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool PlaceNextToAxisLine
        {
            get => _placeNextToAxisLine;

            set
            {
                if (_placeNextToAxisLine != value)
                {
                    _placeNextToAxisLine = value;
                    if (Renderer is not null && !_needLayoutUpdate)
                    {
                        _needLayoutUpdate = true;
                    }
                }
            }
        }

        /// <summary> 
        /// Gets or sets the padding for the plot area in pixels. 
        /// </summary> 
        /// <value> 
        /// A numeric value representing the padding for the plot area.
        /// </value> 
        /// <remarks> 
        /// This property specifies the top and bottom padding of the plot area for the vertical axis,
        /// and the left and right padding for the horizontal axis.
        /// Adjusting the <see cref="PlotOffset"/> property allows for customization of the spacing around the plot area.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to apply plot offset to both the primary X and Y axes.
        /// // PlotOffset creates spacing between the axis line and the plot area to enhance readability.
        /// <SfChart>
        ///     <ChartPrimaryXAxis PlotOffset="20" />
        ///     <ChartPrimaryYAxis PlotOffset="20" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double PlotOffset { get; set; }

        /// <summary> 
        /// Gets or sets the bottom padding for the plot area in pixels. 
        /// </summary> 
        /// <value> 
        /// A numeric value representing the bottom padding for the plot area. The default value is <see cref="double.NaN"/>.
        /// </value> 
        /// <remarks> 
        /// This property is applicable only for the vertical axis.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to apply bottom plot offset for both primary X and Y axes.
        /// // PlotOffsetBottom adds extra padding below the axis to improve spacing or visual alignment.
        /// <SfChart>
        ///     <ChartPrimaryXAxis PlotOffsetBottom="20" />
        ///     <ChartPrimaryYAxis PlotOffsetBottom="20" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double PlotOffsetBottom { get; set; } = double.NaN;

        /// <summary> 
        /// Gets or sets the left padding for the plot area in pixels. 
        /// </summary> 
        /// <value> 
        /// A numeric value representing the left padding for the plot area. The default value is <see cref="double.NaN"/>.
        /// </value>
        /// <remarks> 
        /// This property is applicable only for the horizontal axis.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to apply left plot offset for both primary X and Y axes.
        /// // PlotOffsetLeft adds spacing on the left side of the axis area to control layout and visual alignment.
        /// <SfChart>
        ///     <ChartPrimaryXAxis PlotOffsetLeft="20" />
        ///     <ChartPrimaryYAxis PlotOffsetLeft="20" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double PlotOffsetLeft { get; set; } = double.NaN;

        /// <summary> 
        /// Gets or sets the right padding for the plot area in pixels. 
        /// </summary> 
        /// <value> 
        /// A numeric value representing the right padding for the plot area. The default value is <see cref="double.NaN"/>.
        /// </value> 
        /// <remarks> 
        /// This property is applicable only for the horizontal axis.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to apply right plot offset for both primary X and Y axes.
        /// // PlotOffsetLeft adds spacing on the right side of the axis area to control layout and visual alignment.
        /// <SfChart>
        ///     <ChartPrimaryXAxis PlotOffsetRight="20" />
        ///     <ChartPrimaryYAxis PlotOffsetRight="20" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double PlotOffsetRight { get; set; } = double.NaN;

        /// <summary> 
        /// Gets or sets the top padding for the plot area in pixels. 
        /// </summary> 
        /// <value> 
        /// A numeric value representing the top padding for the plot area. The default value is <see cref="double.NaN"/>.
        /// </value> 
        /// <remarks> 
        /// This property is applicable only for the vertical axis.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to apply top plot offset for both primary X and Y axes.
        /// // PlotOffsetLeft adds spacing on the top side of the axis area to control layout and visual alignment.
        /// <SfChart>
        ///     <ChartPrimaryXAxis PlotOffsetTop="20" />
        ///     <ChartPrimaryYAxis PlotOffsetTop="20" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double PlotOffsetTop { get; set; } = double.NaN;

        /// <summary> 
        /// Gets or sets a value that specifies the padding for the axis range in terms of interval. 
        /// </summary> 
        /// <value> 
        /// One of the <see cref="ChartRangePadding"/> enumerations that specifies the range padding of the axis. 
        /// The options include: 
        /// - <c>None</c>: Padding cannot be applied to the axis. 
        /// - <c>Normal</c>: Padding is applied to the axis based on the range calculation. 
        /// - <c>Additional</c>: Interval of the axis is added as padding to the minimum and maximum values of the range. 
        /// - <c>Round</c>: Axis range is rounded to the nearest possible value divisible by the interval. 
        /// <br/>
        /// The default value is <b>ChartRangePadding.Auto</b>. 
        /// </value> 
        /// <remarks> 
        /// This setting ensures clarity and better axis range handling by allowing extra space or precise boundary control as necessary.
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the range padding of the primary Y-axis to None.
        /// // Setting RangePadding to 'None' disables the automatic padding, allowing the axis range
        /// // to match the actual minimum and maximum of the data values.
        /// <SfChart>
        ///     <ChartPrimaryYAxis RangePadding="ChartRangePadding.None" />
        ///     <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ChartRangePadding RangePadding
        {
            get => _rangePadding;
            set
            {
                if (_rangePadding != value)
                {
                    _rangePadding = value;
                    if (Renderer is not null)
                    {
                        _updateRange = true;
                    }
                }
            }
        }

        /// <summary>  
        /// Gets or sets an instance of <see cref="ChartAxisTitleStyle"/> that specifies the style of the axis title.  
        /// </summary>  
        /// <value>  
        /// An instance of <see cref="ChartAxisTitleStyle"/>.  
        /// </value>  
        /// <remarks>  
        /// This property allows customization of the visual style, including color and font properties enhancing the visual appeal of the axis title.  
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a customized style for the axis title.
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
        public ChartAxisTitleStyle TitleStyle
        {
            get => PrevTitleStyle;
            set
            {
                if (PrevTitleStyle != value)
                {
                    PrevTitleStyle = value;
                    Renderer?.CustomizeGridRenderingOptions(nameof(TitleStyle));
                    Renderer?.ProcessRenderQueue();
                }
            }
        }

        /// <summary> 
        /// Gets or sets a value that specifies the index of the row with which the axis is associated. 
        /// </summary> 
        /// <value> 
        /// A numeric value representing the index of the row associated with the axis. The default value is <b>0</b>.
        /// </value> 
        /// <remarks> 
        /// This property is applicable only when the chart area is divided into multiple plot areas using `Rows`. 
        /// To bind a vertical axis to a specific row, set the axis’s RowIndex property to that row’s index. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a custom row in a chart.
        /// <SfChart>
        ///     <ChartRows>
        ///         <ChartRow Height="50%" />
        ///         <ChartRow Height="50%" />
        ///     </ChartRows>
        ///     <ChartAxis RowIndex="1" Name="YAxis" />
        ///     <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y" />
        ///     <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y1" YAxisName="YAxis" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double RowIndex { get; set; }

        /// <summary> 
        /// Gets or sets the DateTime format in which the labels of the elements such as axis label, data label, and tooltip in the chart component will be processed. 
        /// </summary>  
        /// <value> 
        /// A string representing the DateTime format for processing label values. 
        /// The default value is an empty string. 
        /// </value> 
        /// <remarks> 
        /// This property is applicable only for DateTime-based axis types. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the format for the primary X-axis in a chart.
        /// // The 'Format' property is set to "d MMM yyyy", which displays the date in a custom format,
        /// // such as "1 Jan 2025". The ValueType is set to DateTimeCategory to properly handle the date data.
        /// <SfChart>
        ///     <ChartPrimaryXAxis Format="d MMM yyyy" ValueType="Syncfusion.Blazor.Toolkit.Charts.ValueType.DateTimeCategory" />
        ///     <ChartSeries DataSource="@WeatherReports" XName="XValue" YName="YValue" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string Format
        {
            get => _format;

            set
            {
                if (_format != value)
                {
                    _format = value;
                    if (Renderer is not null && !_needLayoutUpdate)
                    {
                        _updateLabels = true;
                    }
                }
            }
        }

        /// <summary> 
        /// Specifies the number of columns or rows an axis must span horizontally or vertically. 
        /// </summary> 
        /// <value> 
        /// An integer representing the number of columns or rows the axis spans. The default value is <b>1</b>. 
        /// </value> 
        /// <remarks> 
        /// This property is applicable only when the chart area is divided into multiple panes using “Rows” or “Columns”. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the span for the primary y-axis in a chart.
        /// <SfChart>
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
        ///     <ChartPrimaryYAxis Span="2" Minimum="0" Maximum="90" Interval="20" />
        ///     <ChartRows>
        ///         <ChartRow Height="50%" />
        ///         <ChartRow Height="50%" />
        ///     </ChartRows>
        ///     <ChartAxis Minimum="24" Maximum="36" Interval="2" OpposedPosition="true" RowIndex="1" Name="YAxis" LabelFormat="{value}°C" />
        ///     <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y" Type="ChartSeriesType.Column" />
        ///     <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y1" YAxisName="YAxis" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public int Span { get; set; } = 1;

        /// <summary> 
        /// Gets or sets the start angle for the series. 
        /// </summary> 
        /// <value> 
        /// The numeric value representing the start angle for the series. 
        /// </value> 
        /// <remarks> 
        /// This property is applicable only for customizing series in Polar and Radar series type charts. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set a custom start angle for the primary X-axis in a polar chart.
        /// // The 'StartAngle' is set to 270 degrees, which rotates the chart so the first point starts at the bottom.
        /// <SfChart>
        ///     <ChartPrimaryXAxis StartAngle="270" ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
        ///     <ChartSeries DataSource="@SalesReports" XName="X" YName="Y"
        ///                  Type="ChartSeriesType.Polar" DrawType="ChartDrawType.Line" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double StartAngle
        {
            get => _startAngle;

            set
            {
                if (_startAngle != value)
                {
                    _startAngle = value;
                    if (Renderer is not null && !_needLayoutUpdate)
                    {
                        if (Container?._axisContainer is not null)
                        {
                            foreach (ChartAxisRenderer renderer in Container._axisContainer.Renderers)
                            {
                                renderer.ClearAxisInfo();
                                renderer.UpdateAxisRendering();
                            }
                        }

                        foreach (ChartSeriesRenderer seriesRenderer in Renderer.SeriesRenderer)
                        {
                            seriesRenderer.UpdateDirection();
                            seriesRenderer.ProcessRenderQueue();
                        }
                    }
                }
            }
        }

        /// <summary> 
        /// Gets or sets a value indicating whether the numeric axis should start from zero. 
        /// </summary> 
        /// <value> 
        /// <b>true</b> if the numeric axis starts from zero; otherwise, <b>false</b>. The default value is <b>true</b>. 
        /// </value> 
        /// <remarks> 
        /// This property is only applicable for numeric axes. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to disable the 'StartFromZero' property for the primary X-axis.
        /// // When 'StartFromZero' is set to 'false', the axis will not automatically start from zero.
        /// <SfChart>
        ///     <ChartPrimaryXAxis StartFromZero="false"></ChartPrimaryXAxis>
        ///     <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool StartFromZero { get; set; } = true;

        /// <summary>  
        /// Gets or sets the tabindex value for the axis for accessibility purposes.  
        /// </summary>  
        /// <value>  
        /// A numeric value representing the tabindex for the axis. The default value is 2.  
        /// </value>  
        /// <remarks>
        /// The property allows users to include the axis in the natural tab order of the page or modify its order in the sequence.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the TabIndex for the primary X-axis.
        /// // The TabIndex property determines the tab order of the chart elements for keyboard navigation.
        /// <SfChart>
        ///     <ChartPrimaryXAxis TabIndex="1"></ChartPrimaryXAxis>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double TabIndex { get; set; } = 2;

        /// <summary>  
        /// Gets or sets the placement of ticks on the axis line.  
        /// </summary>  
        /// <value>  
        /// One of the <see cref="AxisPosition"/> enumeration that specifies the placement of ticks with respect to the axis line.  
        /// Options include:  
        /// - <c>Inside</c>: Renders the ticks inside the axis line.  
        /// - <c>Outside</c>: Renders the ticks outside the axis line.  
        /// The default mode is <b>AxisPosition.Outside</b>. 
        /// </value>
        /// <remarks>
        /// Adjusting the placement of ticks can have an impact on the readability and aesthetics of the chart.
        /// Placing ticks inside the axis line (<c>Inside</c>) may result in a cleaner look when space is constrained.
        /// In contrast, placing ticks outside (<c>Outside</c>) can improve clarity and separation from the chart content, especially if data points are dense or close to the axis.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to position the tick lines inside the chart area
        /// // for both the primary X-axis and Y-axis using the TickPosition property.
        /// <SfChart>
        ///     <ChartPrimaryXAxis TickPosition="AxisPosition.Inside"></ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis TickPosition="AxisPosition.Inside"></ChartPrimaryYAxis>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public AxisPosition TickPosition
        {
            get => _tickPosition;

            set
            {
                if (_tickPosition != value && !_needLayoutUpdate)
                {
                    _tickPosition = value;
                    _needLayoutUpdate = Renderer is not null;
                }
            }
        }

        /// <summary> 
        /// Gets or sets a collection of strip lines for the axis. 
        /// </summary> 
        /// <value> 
        /// A list containing instances of <see cref="ChartStripline"/> representing the strip lines for the axis. 
        /// </value> 
        /// <remarks> 
        /// This property allows the addition of strip lines to the axis. The strip lines in the list will be rendered on the respective axis. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to use strip lines in a chart.
        /// // Strip lines are used to highlight specific ranges in the chart axis.
        /// // In this example, red and blue strip lines are added on the Y-axis.
        /// <SfChart>
        ///     <ChartPrimaryYAxis>
        ///         <ChartStriplines>
        ///             <ChartStripline Start="20" End="25" Color="red" />
        ///             <ChartStripline Start="32" End="35" Color="blue" />
        ///         </ChartStriplines>
        ///     </ChartPrimaryYAxis>
        ///     <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example> 
        [Parameter]
        public List<ChartStripline> StripLines { get; set; } = new List<ChartStripline>();

        /// <summary> 
        /// Gets or sets a collection of <see cref="ChartMultiLevelLabel"/> representing the multilevel labels for the axis. 
        /// </summary> 
        /// <value> 
        /// A list of <see cref="ChartMultiLevelLabel"/>. 
        /// </value> 
        /// <remarks> 
        /// This property allows adding any number of layers of labels to the axis. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to use multi-level labels in the primary X-axis of a chart.
        /// // Multi-level labels allow grouping of axis labels into hierarchical levels for better readability.
        /// // Two categories are created for "Half yearly 1" and "Half yearly 2" with a maximum text width of 50.
        /// <SfChart>
        ///     <ChartPrimaryXAxis>
        ///         <ChartMultiLevelLabels>
        ///             <ChartMultiLevelLabel>
        ///                 <ChartCategories>
        ///                     <ChartCategory Start="10" End="40" Text="Half yearly 1" MaximumTextWidth=50 />
        ///                     <ChartCategory Start="40" End="70" Text="Half yearly 2" MaximumTextWidth=50 />
        ///                 </ChartCategories>
        ///             </ChartMultiLevelLabel>
        ///         </ChartMultiLevelLabels>
        ///     </ChartPrimaryXAxis>
        ///     <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public List<ChartMultiLevelLabel> MultiLevelLabels { get; set; } = new List<ChartMultiLevelLabel>();

        /// <summary> 
        /// Gets or sets an instance of <see cref="ChartAxisLabelBorder"/> that specifies the border for the axis labels. 
        /// </summary> 
        /// <value> 
        /// An instance of <see cref="ChartAxisLabelBorder"/>. 
        /// </value> 
        /// <remarks> 
        /// This property can be used to customize the border for axis labels. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a customized style for the axis label.
        /// <SfChart>
        ///     <ChartPrimaryXAxis>
        ///         <ChartAxisLabelBorder Color="blue" Width="2" />
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis>
        ///         <ChartAxisLabelBorder Color="blue" Width="2" />
        ///     </ChartPrimaryYAxis>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ChartAxisLabelBorder Border { get; set; } = new ChartAxisLabelBorder();

        /// <summary> 
        /// Gets or sets an instance of <see cref="ChartAxisScrollbarSettings"/> that controls the customization of the axis scrollbar. 
        /// </summary> 
        /// <value> 
        /// An instance of <see cref="ChartAxisScrollbarSettings"/>. 
        /// </value> 
        /// <remarks> 
        /// This property can be used to customize the visibility, pointslength and range for the axis scrollbar. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the range for the scrollbar on the primary X-axis of a chart.
        /// <SfChart>
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.Charts.ValueType.DateTime">
        ///         <ChartAxisScrollbarSettings Enable="true" PointsLength="30" />
        ///     </ChartPrimaryXAxis>
        ///     <ChartSeries DataSource="@dataSource" XName="x" YName="y" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ChartAxisScrollbarSettings ScrollbarSettings
        {
            get => PrevScrollbarSettings;

            set
            {
                if (PrevScrollbarSettings != value)
                {
                    PrevScrollbarSettings = value;
                }
            }
        }

        /// <summary> 
        /// Gets or sets an instance of <see cref="ChartAxisCrosshairTooltip"/> that controls the customization of the axis crosshair tooltip. 
        /// </summary> 
        /// <value> 
        /// An instance of <see cref="ChartAxisCrosshairTooltip"/>. 
        /// </value> 
        /// <remarks> 
        /// This property can be used to customize the fill and text style of the axis crosshair tooltip. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to enable crosshair tooltip for the primary X-axis in a chart.
        /// // The ChartCrosshairSettings component enables the crosshair globally,
        /// // and ChartAxisCrosshairTooltip enables the tooltip specifically for the X-axis.
        /// <SfChart>
        ///     <ChartPrimaryXAxis>
        ///         <ChartAxisCrosshairTooltip Enable="true" Fill="red" />
        ///     </ChartPrimaryXAxis>
        ///     <ChartCrosshairSettings Enable="true" />
        ///     <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ChartAxisCrosshairTooltip CrosshairTooltip { get; set; } = new ChartAxisCrosshairTooltip();

        /// <summary> 
        /// Gets or sets the template used to render axis labels of the <see cref="SfChart"/>. 
        /// </summary> 
        /// <value> 
        /// A <see cref="RenderFragment{ChartAxisLabelInfo}"/> representing the custom template for axis labels. 
        /// </value> 
        /// <example> 
        /// <code> 
        /// <![CDATA[ 
        /// <SfChart> 
        ///     <ChartPrimaryXAxis> 
        ///         <LabelTemplate> 
        ///             @{ 
        ///                 var data = context as ChartAxisLabelInfo; 
        ///             } 
        ///             <div>@data.Text</div> 
        ///         </LabelTemplate> 
        ///     </ChartPrimaryXAxis> 
        /// </SfChart> 
        /// ]]> 
        /// </code> 
        /// </example> 
        /// <remarks> 
        /// This property allows for the display of customized labels, images, or other UI elements in the axis labels. 
        /// </remarks> 
        [Parameter]
        public RenderFragment<ChartAxisLabelInfo> LabelTemplate { get; set; } = null!;

        /// <summary>
        /// Gets or sets the renderer key for internal use.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public string RendererKey { get; set; } = SfBaseUtils.GenerateID("chartaxis");

        /// <summary>
        /// Gets or sets the renderer type for this component.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public Type RendererType { get; set; } = null!;

        #endregion

        #region Private Methods

        /// <summary>
        /// Updates series rendering when axis properties change.
        /// </summary>
        private void UpdateSeries()
        {
            Container?._seriesContainer?._dataLabelCollection.Clear();
            if (Renderer is not null)
            {
                foreach (ChartSeriesRenderer seriesRenderer in Renderer.SeriesRenderer)
                {
                    if (seriesRenderer.Series is not null)
                    {
                        seriesRenderer.UpdateDirection();
                        seriesRenderer.ProcessRenderQueue();
                    }
                }
            }
        }

        /// <summary>
        /// Processes pending axis state changes and triggers appropriate updates.
        /// </summary>
        private void HandlePendingUpdates()
        {
            if (_needLayoutUpdate)
            {
                _needLayoutUpdate = false;
                Renderer?.Chart?.OnLayoutChange();
                return;
            }

            if (_updateLabels || _updateRange)
            {
                Renderer?.ChangeAxisRange(_updateRange);
                _updateLabels = _updateRange = false;
                UpdateSeries();
                return;
            }

            if (_needSeriesRefresh || _needAxisRefresh)
            {
                Renderer?.ClearAxisInfo();
                Renderer?.UpdateAxisRendering();
                if (_needSeriesRefresh)
                {
                    UpdateSeries();
                }

                _needAxisRefresh = _needSeriesRefresh = false;
            }
        }

        #endregion

        #region Lifecycle methods

        /// <summary>
        /// Performs component initialization by determining the appropriate renderer type.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            RendererType = ChartAxisRenderer.GetRendererType(ValueType);
        }

        /// <summary>
        /// Handles parameter changes and registers the axis with the parent chart container.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            AxisValueType = ValueType.ToString();
            AxisIntervalType = IntervalType.ToString();

            Container?.AddAxis(this);

            HandlePendingUpdates();
        }

        /// <summary>
        /// Cleans up resources when the component is disposed.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override ValueTask DisposeAsyncCore()
        {
            Container?.RemoveAxis(this);
            ChildContent = null!;
            PrevScrollbarSettings?.ComponentDispose();
            return base.DisposeAsyncCore();
        }

        #endregion

        #region Internal methods

        /// <summary>
        /// Sets the inverse state for the axis.
        /// </summary>
        /// <param name="isInversed">A boolean indicating whether the axis should be inverted.</param>
        internal void SetIsInversed(bool isInversed)
        {
            this._isInversed = isInversed;
        }

        /// <summary>
        /// Updates the zoom factor and position for the axis.
        /// </summary>
        /// <param name="axisZoomFactor">The new zoom factor (between 0 and 1).</param>
        /// <param name="axisZoomPosition">The new zoom position (between 0 and 1).</param>
        internal void UpdateZoomValues(double axisZoomFactor, double axisZoomPosition)
        {
            _zoomFactor = axisZoomFactor;
            _zoomPosition = axisZoomPosition;

            if (Container is not null)
            {
                Container._selectionModule?.ClearDraggedRects();
                Container._selectionModule?.OnPropertyChanged();
                Container._parentRect?.ClearElements();
            }
        }

        /// <summary>
        /// Sets the visibility of the axis.
        /// </summary>
        /// <param name="visibility">A boolean indicating whether the axis should be visible.</param>
        internal void SetAxisVisibility(bool visibility)
        {
            _visible = visibility;
        }

        /// <summary>Updates a specific axis property by name with the provided value.</summary>
        /// <param name="key">The property name to update.</param>
        /// <param name="keyValue">The new value for the property.</param>
        /// <remarks>
        /// This method handles dynamic property updates for nested objects like styles, grid lines, and settings.
        /// It reinitializes target objects to ensure clean state before assignment.
        /// </remarks>
        internal void UpdateAxisProperties(string key, object keyValue)
        {
            switch (key)
            {
                case nameof(LabelStyle):
                    PrevLabelStyle = new ChartAxisLabelStyle();
                    LabelStyle = (ChartAxisLabelStyle)keyValue;
                    break;
                case nameof(LineStyle):
                    LineStyle = (ChartAxisLineStyle)keyValue;
                    break;
                case nameof(MajorGridLines):
                    PrevMajorGridLines = new ChartAxisMajorGridLines();
                    MajorGridLines = (ChartAxisMajorGridLines)keyValue;
                    break;
                case nameof(MajorTickLines):
                    MajorTickLines = (ChartAxisMajorTickLines)keyValue;
                    break;
                case nameof(MinorGridLines):
                    MinorGridLines = (ChartAxisMinorGridLines)keyValue;
                    break;
                case nameof(MinorTickLines):
                    MinorTickLines = (ChartAxisMinorTickLines)keyValue;
                    break;
                case nameof(MultiLevelLabels):
                    MultiLevelLabels = (List<ChartMultiLevelLabel>)keyValue;
                    break;
                case nameof(Border):
                    Border = (ChartAxisLabelBorder)keyValue;
                    break;
                case nameof(ScrollbarSettings):
                    ScrollbarSettings = (ChartAxisScrollbarSettings)keyValue;
                    if (Container is not null && Renderer is not null)
                    {
                        Container.UpdateRenderers();
                        if (Container._zoomingModule is not null && ((Container._zoomingModule.IsAxisZoomed(Container._axisContainer?.Renderers ?? null!) && !(Container._zoomSettings.ToolbarDisplayMode == ToolbarMode.None)) || Container._zoomSettings.ToolbarDisplayMode == ToolbarMode.Always))
                        {
                            Container._zoomingToolkitContent?.ShowZoomingKit();
                        }
                    }
                    break;
                case nameof(CrosshairTooltip):
                    CrosshairTooltip = (ChartAxisCrosshairTooltip)keyValue;
                    break;
                case nameof(TitleStyle):
                    PrevTitleStyle = new ChartAxisTitleStyle();
                    TitleStyle = (ChartAxisTitleStyle)keyValue;
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Public Methods

        /// <summary> 
        /// Gets the name of the axis. 
        /// </summary> 
        /// <returns> 
        /// A string representing the name of the axis. 
        /// </returns> 
        /// <example> 
        /// This example demonstrates how to get axis name using button click in a Blazor component. 
        /// <code> 
        /// <![CDATA[ 
        /// <SfButton OnClick="GetAxisName">Get AxisName</SfButton> 
        /// <SfChart> 
        /// <ChartPrimaryXAxis @ref="@chartaxis" ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"/> 
        /// …   
        /// </SfChart> 
        /// ]]> 
        /// @code {   
        ///  public ChartAxis chartaxis; 
        ///  public void GetAxisName() 
        ///  { 
        ///    string axisName = chartaxis.GetName(); 
        ///  } 
        /// }   
        /// </code>   
        /// </example> 
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public virtual string GetName()
        {
            return Name;
        }

        #endregion
    }
}