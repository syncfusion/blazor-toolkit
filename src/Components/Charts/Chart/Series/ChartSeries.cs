using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Charts.Internal;
using Syncfusion.Blazor.Toolkit.Data;
using Syncfusion.Blazor.Toolkit.Internal;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Defines the options for a series in a chart, allowing customization of its appearance and behavior.
    /// </summary>
    /// <remarks>
    /// The <see cref="ChartSeries"/> component participates in data-binding and cooperates with the owning <see cref="SfChart"/>.
    /// It exposes a rich set of parameters for appearance, interactivity, accessibility and data mapping.
    /// </remarks>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <SfChart>
    ///     <ChartSeriesCollection>
    ///         <ChartSeries DataSource="@Data" XName="X" YName="Y" Type="ChartSeriesType.Line" />
    ///     </ChartSeriesCollection>
    /// </SfChart>
    /// ]]>
    /// </code>
    /// </example>
    public class ChartSeries : ChartDataBoundComponent, IChartElement
    {
        #region Constants

        /// <summary>
        /// Refresh throttle (in ms) used when a datasource mutates frequently to avoid excessive UI churn.
        /// </summary>

        private const int UPDATE_THRESHOLD = 200;

        #endregion

        #region Fields

        private bool _shouldWire = true;
        private bool _needLayoutUpdate;
        private bool _refreshRange;
        private bool _refreshSeries;
        private bool _shouldProcess = true;
        private int _labelCurrentCount;
        private int _labelPreviousCount;

        internal bool _isSeriesChanged;
        internal bool _isLegendClicked;
        internal ChartDataEditSettings _chartDataEditSettings;
        #endregion

        #region Properties

       /// <summary>
       /// Gets or sets the custom template used to render the content of the legend item for the current <see cref="ChartSeries"/>.
       /// </summary>
       /// <value>Specifies any valid HTML or Blazor content that defines the legend item's appearance for this series.</value>
       /// <remarks>
       /// When set, this property displays the provided template in place of the default legend item text and shape for the current series.
       /// If both <see cref="LegendItemTemplate"/> and the <see cref="Name"/> are unset, no legend item will be displayed.
       /// </remarks>
       /// <example>  
       /// <code>  
       /// <![CDATA[  
       /// <SfChart> 
       ///     <ChartSeriesCollection> 
       ///         <ChartSeries> 
       ///             <LegendItemTemplate> 
       ///                 <div>Profit</div> 
       ///             </LegendItemTemplate> 
       ///         </ChartSeries> 
       ///     </ChartSeriesCollection> 
       ///     <ChartLegendSettings Visible="true"/> 
       /// </SfChart> 
       /// ]]> 
       /// </code> 
       /// </example>
       [Parameter]
        public RenderFragment LegendItemTemplate { get; set; } = null!;

        /// <summary>
        /// Specifies the type of series, such as Line, Column, Area, and others.
        /// </summary>
        /// <value>An enumeration of type <see cref="ChartSeriesType"/> representing the series type.</value>
        /// <remarks>
        /// The type of the series affects how the data is rendered. Assign <see cref="DataSource"/>, <see cref="XName"/>, and <see cref="YName"/>.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to render a line chart type in a chart.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" Type="ChartSeriesType.Line" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        private ChartSeriesType _type = ChartSeriesType.Line;
        [Parameter]
        public ChartSeriesType Type
        {
            get => _type;
            set
            {
                if (_type == value)
                {
                    return;
                }

                if (Container is null)
                {
                    _type = value;
                    return;
                }

                if (Container._seriesContainer is not null)
                {
                    if (value.ToString().Contains("Bar", StringComparison.InvariantCulture) || (SeriesType is not null && SeriesType.Contains("Bar", StringComparison.InvariantCulture)))
                    {
                        _type = value;
                        RendererType = ChartSeriesRenderer.GetRendererType(_type);
                        NeedRendererRemove = true;
                        Container._seriesContainer.RendererShouldRender = true;
                        Container._seriesContainer.Prerender();
                    }
                    else
                    {
                        _type = value;
                        RendererType = ChartSeriesRenderer.GetRendererType(_type);
                        NeedRendererUpdate = true;
                        NeedRendererRemove = true;
                        _isSeriesChanged = true;
                        Container._seriesContainer.RendererShouldRender = true;
                        Container._seriesContainer.Prerender();

                        if (Container._legendRenderer is not null)
                        {
                            Container._legendRenderer.RendererShouldRender = Visible;
                            Container._legendRenderer.UpdateLegendShape(Renderer);
                            Container._legendRenderer.ProcessRenderQueue();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Specifies the visibility of the series.
        /// </summary>
        /// <value>A boolean value indicating whether the series is visible or not. The default is <c>true</c>.</value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to hide a chart series using the 'Visible' property.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" Visible="false" />
        ///     </ChartSeriesCollection>
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
                    if (Renderer is not null)
                    {
                        if (FindLayoutChange())
                        {
                            _needLayoutUpdate = true;
                        }
                        else
                        {
                            _refreshRange = true;
                        }

                        if (Renderer.IsCategoryAxis())
                        {
                            _labelPreviousCount = Renderer.XAxisRenderer.Labels.Count;

                            if (Container is not null && Container._dataEditingModule is not null && !Container._dataEditingModule._isPointDragged)
                            {
                                Renderer.UpdateCategoryData();
                            }

                            _labelCurrentCount = Renderer.XAxisRenderer.Labels.Count;
                            Renderer.XAxisRenderer.ChangeAxisRange(true);
                            Container?._striplineBehindContainer?.UpdateStriplineCollection();
                        }

                        if (Container?._legendRenderer is not null && Container._legendRenderer.LegendSettings is not null && Container._legendRenderer.LegendSettings.Visible)
                        {
                            Container._legendRenderer.RendererShouldRender = true;
                            Container._legendRenderer.UpdateLegendFill(Renderer);
                            Container._legendRenderer.ProcessRenderQueue();
                        }

                        Container?._seriesContainer?.UpdateStackingValues();
                    }
                }
            }
        }

        /// <summary>
        /// Defines the stroke width for the series (e.g., line-type series and indicator signal lines).
        /// </summary>
        /// <value>A double value specifying the width in pixels.</value>
        /// <remarks>
        /// This property is applicable for line-type series.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the width of a chart series.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" Width="20" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        private double _widthProperty = 1;
        [Parameter]
        public double Width
        {
            get => _widthProperty;
            set
            {
                if (Math.Abs(_widthProperty - value) > double.Epsilon)
                {
                    _widthProperty = value;
                    Renderer?.UpdateCustomization(nameof(Width));
                    Renderer?.Container?.AddToRenderQueue(Renderer);
                    Renderer?.ProcessRenderQueue();

                    if (Container?._legendRenderer?.LegendSettings is not null && Container._legendRenderer.LegendSettings.Visible)
                    {
                        Container._legendRenderer.RendererShouldRender = true;
                        Container._legendRenderer.UpdateLegendWidth(Renderer ?? null!, _widthProperty);
                        Container._legendRenderer.ProcessRenderQueue();
                    }
                }
            }
        }

        /// <summary>
        /// Specifies the name of the horizontal axis associated with the series.
        /// </summary>
        /// <value>A string representing the name of the associated horizontal axis. Default is <b>"PrimaryXAxis"</b>.</value>
        /// <remarks>
        /// This property is used when associating the series with an axis.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to assign secondary x axis to the chart series.
        /// <SfChart>
        ///     <ChartAxes>
        ///         <ChartAxis Name="XAxis" />
        ///     </ChartAxes>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y" />
        ///         <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y1" XAxisName="XAxis" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string XAxisName { get; set; } = Constants.PrimaryXAxis;

        /// <summary>
        /// Gets or sets the accessibility description for the <see cref="ChartSeries"/>.
        /// </summary>
        /// <value>Accepts a string that defines the accessibility description for the <see cref="ChartSeries"/> root element.</value>
        /// <remarks>
        /// Use this property to provide an accessibility description for the <see cref="ChartSeries"/> root element, which is the parent element of the series points.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the accessibility description for a chart series.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" AccessibilityDescription="This is a sample series" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string AccessibilityDescription { get; set; } = null!;

        /// <summary>
        /// Gets or sets the format string used for the accessibility description of the data points in the <see cref="ChartSeries"/>.
        /// The format string can include the following placeholders:
        /// <list type="bullet">
        /// <item><description>${series.name}: Displays the name of the series.</description></item>
        /// <item><description>${point.x}: Displays the x-value of the point.</description></item>
        /// <item><description>${point.y}: Displays the y-value of the point.</description></item>
        /// <item><description>${point.high}: Displays the high value of the point. This is applicable only for financial and range series.</description></item>
        /// <item><description>${point.low}: Displays the low value of the point. This is applicable only for financial and range series.</description></item>
        /// <item><description>${point.open}: Displays the open value of the point. This is applicable only for financial series.</description></item>
        /// <item><description>${point.close}: Displays the close value of the point. This is applicable only for financial series.</description></item>
        /// </list>
        /// </summary>
        /// <value>A string that specifies the format for the accessibility description of points in the series.</value>
        /// <remarks>
        /// Use this property to specify the format for the points accessibility description of the <see cref="ChartSeries"/>. The <see cref="AccessibilityDescription"/> property provides accessibility information for the series root element.
        /// However, this property allows you to provide dynamic information based on the <see cref="ChartSeries"/> data.
        /// For example, the format "${series.name} : ${point.x}" displays the series name and x-value of the point in the accessibility description.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the accessibility description format for a chart series.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" AccessibilityDescriptionFormat="${series.name} : ${point.x}" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string AccessibilityDescriptionFormat { get; set; } = null!;

        /// <summary>
        /// Gets or sets the accessibility role for the <see cref="ChartSeries"/>.
        /// </summary>
        /// <value>A string that defines the accessibility role for the <see cref="ChartSeries"/>.</value>
        /// <remarks>
        /// Use this property to specify the accessibility role for the <see cref="ChartSeries"/> root element.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the accessibility role for a chart series.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" AccessibilityRole="region" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string AccessibilityRole { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the series root can receive keyboard focus.
        /// </summary>
        /// <value><c>true</c> to enable keyboard focus on the root; otherwise <c>false</c>. Default is <c>true</c>.</value>
        /// <remarks>
        /// Use this property to toggle keyboard navigation focus for the <see cref="ChartSeries"/>.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to enable focusable behavior on a chart series.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" Focusable="true" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool Focusable { get; set; } = true;

        /// <summary>
        /// Gets or sets the field name in the data source that represents the x-axis value.
        /// </summary>
        /// <value>A string specifying the data source field for the x-axis value.</value>
        /// <remarks>
        /// This property is applicable to both series and technical indicators, determining how data points are bound to the x-axis in the chart.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to bind a field from for data source to X axis.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string XName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the vertical axis associated with the series.
        /// </summary>
        /// <value>A string representing the name of the associated vertical axis. Default is <b>"PrimaryYAxis"</b>.</value>
        /// <remarks>
        /// Use this property to link the series to a specific vertical axis, allowing for targeted data plotting on the chart.
        /// It is applicable for series and technical indicators. It requires <c>Axes</c> of the chart.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to assign secondary y axis to the chart series.
        /// <SfChart>
        ///     <ChartAxes>
        ///         <ChartAxis Name="YAxis" />
        ///     </ChartAxes>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y" />
        ///         <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y1" YAxisName="YAxis" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string YAxisName { get; set; } = Constants.PrimaryYAxis;

        /// <summary>
        /// Gets or sets the field name in the data source that represents the y-axis value.
        /// </summary>
        /// <value>A string specifying the data source field for the y-axis value.</value>
        /// <remarks>
        /// This property is used to map data points to the y-axis.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to bind a field from for data source to y axis.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string YName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the field name in the data source that represents the high value for financial series.
        /// </summary>
        /// <value>A string specifying the data source field for the high value.</value>
        /// <remarks>
        /// This property is used in financial charts to determine the high points of data points, like in stock market data.
        /// Note: Need to assign the High, Low, Open, Close property.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to render a HiloOpenClose series in a chart using the High property.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@StockDetails" 
        ///                      XName="X" 
        ///                      High="High" 
        ///                      Low="Low" 
        ///                      Open="Open" 
        ///                      Close="Close" 
        ///                      Type="ChartSeriesType.HiloOpenClose">
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string High { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the field name in the data source that represents the low value for financial series.
        /// </summary>
        /// <value>A string specifying the data source field for the low value.</value>
        /// <remarks>
        /// This property is utilized in financial charts to identify the low points of data entries, such as those in stock market data.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to render a HiloOpenClose series in a chart using the Low property.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@StockDetails" 
        ///                      XName="X" 
        ///                      High="High" 
        ///                      Low="Low" 
        ///                      Open="Open" 
        ///                      Close="Close" 
        ///                      Type="ChartSeriesType.HiloOpenClose">
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string Low { get; set; } = string.Empty;

        /// <summary>
        /// Defines the group name to associate chart series.
        /// </summary>
        /// <value>A string indicating the group name for series stacking.</value>
        /// <remarks>
        /// The axis in the same group shares the same baseline and location on the corresponding axis.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to use grouped column series in a Chart.
        /// <SfChart>
        ///     <ChartPrimaryXAxis ValueType="@Syncfusion.Blazor.Toolkit.ValueType.Category" Interval="1">
        ///     </ChartPrimaryXAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@ChartPoints" XName="Year" YName="USA_Total" GroupName="USA" Type="ChartSeriesType.Column" />
        ///         <ChartSeries DataSource="@ChartPoints" XName="Year" YName="USA_Gold" GroupName="USA" Type="ChartSeriesType.Column" />
        ///         <ChartSeries DataSource="@ChartPoints" XName="Year" YName="UK_Total" GroupName="UK" Type="ChartSeriesType.Column" />
        ///         <ChartSeries DataSource="@ChartPoints" XName="Year" YName="UK_Gold" GroupName="UK" Type="ChartSeriesType.Column" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string GroupName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the field name in the data source that represents the opening value for financial series.
        /// </summary>
        /// <value>A string specifying the data source field for the opening value.</value>
        /// <remarks>
        /// This property is used in financial charts to denote the opening price of financial data points, such as stocks at the start of a trading session.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to render a HiloOpenClose series in a chart using the Open property.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@StockDetails" 
        ///                      XName="X" 
        ///                      High="High" 
        ///                      Low="Low" 
        ///                      Open="Open" 
        ///                      Close="Close" 
        ///                      Type="ChartSeriesType.HiloOpenClose">
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string Open { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the field name in the data source that represents the closing value for financial series.
        /// </summary>
        /// <value>A string specifying the data source field for the closing value.</value>
        /// <remarks>
        /// This property is used in financial charts to denote the closing price of financial data points, such as stocks at the end of a trading session.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to render a HiloOpenClose series in a chart using the Close property.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@StockDetails" 
        ///                      XName="X" 
        ///                      High="High" 
        ///                      Low="Low" 
        ///                      Open="Open" 
        ///                      Close="Close" 
        ///                      Type="ChartSeriesType.HiloOpenClose">
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string Close { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the field name in the data source that represents the volume value for financial series.
        /// </summary>
        /// <value>A string specifying the data source field for the volume value.</value>
        /// <remarks>
        /// This property is used in financial charts to denote the volume of trades or transactions for financial data points, such as stock trading volumes.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to render a candlestick chart with an Accumulation/Distribution indicator.
        /// <SfChart>    
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.DateTime" 
        ///                         Title="months" 
        ///                         IntervalType="IntervalType.Months">
        ///     </ChartPrimaryXAxis>
        ///     
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@StockDetails" 
        ///                      Name="Apple Inc" 
        ///                      XName="X" 
        ///                      Low="Low" 
        ///                      High="High" 
        ///                      Close="Close" 
        ///                      Volume="Volume" 
        ///                      Open="Open" 
        ///                      Width="2" 
        ///                      Type="ChartSeriesType.Candle">
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string Volume { get; set; } = string.Empty;

        /// <summary>
        /// Specifies the name of the series.
        /// </summary>
        /// <value>A string representing the name for identification and display purposes.</value>
        /// <remarks>
        /// This property is used for identifying and labeling the series in the chart.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to assign a name to a chart series.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" 
        ///                      XName="XValue" 
        ///                      YName="YValue" 
        ///                      Name="chartData" />
        ///     </ChartSeriesCollection>
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
                if (_name != value)
                {
                    _name = value;

                if (Container is not null && Container._isChartFirstRender)
                {
                    Renderer?.UpdateSeriesDataAsync();
                    if (Container._legendRenderer is not null && !Container._isLayoutChange)

                    {
                        Container._legendRenderer.RendererShouldRender = Visible;
                        Container._legendRenderer.UpdateLegendShape(Renderer);
                        Container._legendRenderer.ProcessRenderQueue();
                    }
                }

                }
            }
        }

        /// <summary>
        /// Specifies the z-order of the series.
        /// </summary>
        /// <value>An integer representing the stacking order of the series.</value>
        /// <remarks>
        /// This determines the drawing order of series in the chart, with higher values drawn above lower values.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to use zorder in chart series.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" Type="ChartSeriesType.Column" ZOrder="1">
        ///         </ChartSeries>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="Y1Value" Type="ChartSeriesType.Column">
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public int ZOrder { get; set; }

        /// <summary>
        /// Gets or sets the fill color for the chart series.
        /// </summary>
        /// <value> 
        /// It accepts a value in hex or rgba as a valid CSS color string.
        /// It also represents the color of signal lines in technical indicators.
        /// For technical indicators, the default value is <b>'blue'</b>; for series, it is <b>null</b>.
        /// </value>
        /// <remarks>
        /// This property determines the fill color for the series elements, affecting both visual appearance and legend representation.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the fill color of a chart series.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" 
        ///                      XName="XValue" 
        ///                      YName="YValue" 
        ///                      Fill="blue" Type="ChartSeriesType.Column"/>
        ///     </ChartSeriesCollection>
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
                if (_fill != value)
                {
                    _fill = value;
                    if (Renderer is not null)
                    {
                        Renderer.Interior = !string.IsNullOrEmpty(_fill)
                            ? _fill
                            : Container?._seriesContainer?._palette?[Renderer.Index % Container._seriesContainer._palette.Length] ?? null!;
                        Renderer.UpdateCustomization(nameof(Fill));
                        Renderer.Container?.AddToRenderQueue(Renderer);
                        Renderer.ProcessRenderQueue();

                        if (Container?._legendRenderer is not null)
                        {
                            Container._legendRenderer.RendererShouldRender = true;
                            Container._legendRenderer.UpdateLegendFill(Renderer, Renderer.Interior);
                            Container._legendRenderer.ProcessRenderQueue();
                        }

                        Renderer.Series?.Marker?.Renderer?.UpdateCustomization("Color");

                        if (IsUpdateDirectionOnFill())
                        {
                            _refreshSeries = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the data source for the chart.
        /// </summary>
        /// <value>An <see cref="IEnumerable{T}"/> of items or a DataManager.</value>
        /// <remarks>
        /// This property determines the data set displayed by the chart series.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to bind the datasource in chart series.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// @code {
        ///     public class ChartData
        ///     {
        ///         public double XValue { get; set; }
        ///         public double YValue { get; set; }
        ///     }
        /// 
        ///     public List<ChartData> Data = new List<ChartData>
        ///     {
        ///         new ChartData { XValue = 10, YValue = 21 },
        ///         new ChartData { XValue = 20, YValue = 24 },
        ///         new ChartData { XValue = 30, YValue = 36 }
        ///     };
        /// }
        /// ]]>
        /// </code>
        /// </example>
        private IEnumerable<object> _dataSource = null!;
        [Parameter]
        public IEnumerable<object> DataSource
        {
            get => _dataSource;
            set
            {
                if (value is not null && _shouldProcess)
                {
                    bool isDataChanged = _refreshRange;
                    object[] dataArray = [.. value];

                    if (Renderer is null)
                    {
                        _dataSource = IsINotifyImplements(value) ? value : dataArray;
                        _ = SetDataManager<object>(_dataSource);

                        if (_dataSource is not null && _shouldWire && IsINotifyImplements(_dataSource))
                        {
                            _shouldWire = false;
                            ((INotifyCollectionChanged)_dataSource).CollectionChanged += DataCollectionChanged;

                            if (_dataSource.Any() && _dataSource.First() is INotifyPropertyChanged)
                            {
                                foreach (INotifyPropertyChanged item in (_dataSource).Cast<INotifyPropertyChanged>())
                                {
                                    item.PropertyChanged += PropertyChanged;
                                }
                            }
                        }

                        _shouldProcess = false;
                    }
                    else if (Container is not null && Container._isChartFirstRender &&
                             ((_dataSource?.Count()) != dataArray.Length || CheckDatasourceChanged(_dataSource, value, out isDataChanged)))
                    {
                        isDataChanged = !isDataChanged ? _dataSource?.Count() != dataArray.Length : isDataChanged;
                        _dataSource = IsINotifyImplements(value) ? value : dataArray;
                        CurrentViewData = IsINotifyImplements(value) ? value : [.. dataArray];
                        _ = SetDataManager<object>(_dataSource);
                        UpdateDataSource = true;
                        Container._isOnceRendered = !isDataChanged;
                        _shouldProcess = false;
                        Container._selectionModule?.ClearDraggedRects();
                        Container._selectionModule?.OnPropertyChanged();
                        Container._parentRect?.ClearElements();
                    }
                    else if (Container is not null && !Container._isChartFirstRender)
                    {
                        _dataSource = IsINotifyImplements(value) ? value : dataArray;
                        CurrentViewData = IsINotifyImplements(value) ? value : dataArray;
                        _shouldProcess = false;
                    }
                }
            }
        }

        /// <summary>
        /// Specifies the query to select data from <c>DataSource</c>.
        /// </summary>
        /// <value>A <see cref="Query"/> describing the selection/filtering.</value>
        /// <remarks>
        /// This property is applicable when the DataSource is SfDataManager.
        /// </remarks>
        private Query _query = null!;
        [Parameter]
        public Query Query
        {
            get => _query;
            set
            {
                if (_query != value)
                {
                    _query = value;
                    _ = Renderer?.UpdateSeriesDataAsync();
                }
            }
        }

        /// <summary>
        /// Improves chart performance through data mapping.
        /// </summary>
        /// <value><c>true</c> to enable complex property binding; otherwise <c>false</c>. Default is <c>false</c>.</value>
        /// <remarks>
        /// Use this property to enhance chart rendering performance with complex data binding.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to bind complex object properties as Y-values in a column chart.
        /// <SfChart>
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@medalData" XName="Country" YName="Silver.Count" Type="ChartSeriesType.Column" EnableComplexProperty="true" />
        ///         <ChartSeries DataSource="@medalData" XName="Country" YName="Gold.Count" EnableComplexProperty="true" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        private bool _enableComplexProperty;
        [Parameter]
        public bool EnableComplexProperty
        {
            get => _enableComplexProperty;
            set
            {
                if (_enableComplexProperty != value)
                {
                    _enableComplexProperty = value;
                    Renderer?.UpdateDirection();
                }
            }
        }

        /// <summary>
        /// If set to <c>true</c>, the tooltip for the series will be visible.
        /// </summary>
        /// <value><c>true</c> to enable series tooltips; otherwise <c>false</c>. Default is <c>true</c>.</value>
        /// <remarks>
        /// Use this property to display additional information about series points when hovering over them with cursor.
        /// Note: This property is applicable only when the <c>ChartTooltipSettings.Enable</c> property is set to <c>true</c>.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to disable the tooltip for a chart series.
        /// <SfChart>
        ///     <ChartTooltipSettings Enable="true"></ChartTooltipSettings>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" EnableTooltip="false" />
        ///     </ChartSeriesCollection>
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
                if (_enableTooltip != value)
                {
                    _enableTooltip = value;
                    if (Renderer is not null && Renderer.Series is not null && Renderer.Series.Visible)
                    {
                        Renderer.UpdateDirection();
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets a value that determines whether tooltips are displayed for the nearest data point to the cursor for this series.
        /// </summary>
        /// <value><c>true</c> to display the nearest tooltip for the series; otherwise <c>false</c>. Default is <c>true</c>.</value>
        /// <remarks>
        /// This property controls the display of the tooltip based on the proximity of the nearest data point to the cursor for a specific chart series.
        /// The <b>ChartTooltipSettings.ShowNearestTooltip</b> property must be set to <b>true</b> for this property to function.
        /// When the <b>ChartSeries.ShowNearestTooltip</b> property is set to <b>false</b>, the tooltip for the nearest data point will not be displayed for the current series, but it may still be shown for other series.
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to enable and show the nearest tooltip on both the chart and series.
        /// <SfChart>
        ///     <ChartTooltipSettings Enable="true" ShowNearestTooltip="true"></ChartTooltipSettings>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" ShowNearestTooltip="true" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        private bool _showNearestTooltip = true;
        [Parameter]
        public bool ShowNearestTooltip
        {
            get => _showNearestTooltip;
            set
            {
                if (_showNearestTooltip != value)
                {
                    _showNearestTooltip = value;
                    Renderer?.UpdateDirection();
                }
            }
        }

        /// <summary>
        /// Defines the tooltip format for the series.
        /// </summary>
        /// <value>A composite string with placeholders for series/point fields.</value>
        /// <remarks>
        /// This property allows customization of the content displayed in tooltips, using placeholders for dynamic data such as series and data point values.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to enable tooltip with custom format.
        /// <SfChart>
        ///     <ChartTooltipSettings Enable="true"></ChartTooltipSettings>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" TooltipFormat="${point.x}" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        private string _tooltipFormat = null!;
        [Parameter]
        public string TooltipFormat
        {
            get => _tooltipFormat;
            set
            {
                if (_tooltipFormat != value)
                {
                    _tooltipFormat = value;
                    Renderer?.UpdateDirection();
                }
            }
        }

        /// <summary>
        /// Gets or sets the data source field name that contains the color value of points.
        /// </summary>
        /// <value>A string specifying the field in the data source for point color mapping.</value>
        /// <remarks>
        /// This property specifies the data source field that provides color values for individual data points in the series.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to use PointColorMapping in a column chart to apply different colors to each data point.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" Type="ChartSeriesType.Column" PointColorMapping="Color"/>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// @code {
        ///     public class ChartData
        ///     {
        ///         public double XValue { get; set; }
        ///         public double YValue { get; set; }
        ///         public string Color { get; set; }
        ///     }
        /// 
        ///     public List<ChartData> Data = new List<ChartData>
        ///     {
        ///         new ChartData { XValue = 10, YValue = 21 , Color = "red"},
        ///         new ChartData { XValue = 20, YValue = 24 , Color = "green"},
        ///         new ChartData { XValue = 30, YValue = 36 , Color = "blue"}
        ///     };
        /// }
        /// ]]>
        /// </code>
        /// </example>
        private string _pointColorMapping = string.Empty;
        [Parameter]
        public string PointColorMapping
        {
            get => _pointColorMapping;
            set
            {
                if (_pointColorMapping != value)
                {
                    _pointColorMapping = value;
                    Renderer?.InitSeriesRendererFields();
                    Renderer?.ProcessData();
                    Renderer?.UpdateDirection();
                }
            }
        }

        /// <summary>
        /// The data source field that contains the size value for the bubble series.
        /// </summary>
        /// <value>A string that specifies the field name used for bubble size.</value>
        /// <remarks>
        /// This property specifies the data source field that provides size values, used to render bubbles in a bubble series.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to render a bubble chart using Size property.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" Type="ChartSeriesType.Bubble" Size="BubbleSize"/>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// @code {
        ///     public class ChartData
        ///     {
        ///         public double XValue { get; set; }
        ///         public double YValue { get; set; }
        ///         public double BubbleSize { get; set; }
        ///     }
        /// 
        ///     public List<ChartData> Data = new List<ChartData>
        ///     {
        ///         new ChartData { XValue = 10, YValue = 21 , BubbleSize = 1.241},
        ///         new ChartData { XValue = 20, YValue = 24 , BubbleSize = 3.234},
        ///         new ChartData { XValue = 30, YValue = 36 , BubbleSize = 5.634}
        ///     };
        /// }
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string Size { get; set; } = string.Empty;

        /// <summary>
        /// Defines the pattern of dashes and gaps to stroke the lines.
        /// </summary>
        /// <value>A string pattern for dash arrays used when drawing lines.</value>
        /// <remarks>
        /// This property specifies the pattern of dashes and gaps applied to lines in the series, allowing customization of line styles.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to apply a dashed line style to a chart series in Chart.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" DashArray="5,3" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        private string _dashArrayProperty = "0";
        [Parameter]
        public string DashArray
        {
            get => _dashArrayProperty;
            set
            {
                if (_dashArrayProperty != value)
                {
                    _dashArrayProperty = value;
                    Renderer?.UpdateCustomization(nameof(DashArray));
                    Renderer?.Container?.AddToRenderQueue(Renderer);
                    Renderer?.ProcessRenderQueue();
                }
            }
        }

        /// <summary>
        /// Defines the opacity of the series fill.
        /// </summary>
        /// <value>A double from 0 to 1. Default is <b>1</b>.</value>
        /// <remarks>
        /// This property specifies the transparency level of the series fill, allowing for various visual presentation effects.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the opacity of a chart series in Chart.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" Type="ChartSeriesType.Column" Fill="red" Opacity="0.5" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        private double _opacityProperty = 1;
        [Parameter]
        public double Opacity
        {
            get => _opacityProperty;
            set
            {
                if (Math.Abs(_opacityProperty - value) > double.Epsilon)
                {
                    _opacityProperty = value;
                    Renderer?.UpdateCustomization(nameof(Opacity));
                    Renderer?.Container?.AddToRenderQueue(Renderer);
                    Renderer?.ProcessRenderQueue();
                }
            }
        }

        /// <summary>
        /// Defines the border of the rectangle-shaped series.
        /// </summary>
        /// <value>A <see cref="ChartSeriesBorder"/> object for configuring series borders.</value>
        /// <remarks>
        /// Use this property to specify the border attributes, such as color and width, for series that can be represented as rectangular shapes.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to render a column chart with a custom border.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" Type="ChartSeriesType.Column">
        ///             <ChartSeriesBorder Width="10" Color="blue"></ChartSeriesBorder>
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ChartSeriesBorder Border { get; set; } = new();

        /// <summary>
        /// Specifies the legend shape of the series.
        /// </summary>
        /// <value>A <see cref="LegendShape"/> representing the legend symbol.</value>
        /// <remarks>
        /// This property defines the shape used to represent the series in the chart legend, enhancing visual clarity for viewers.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to render a chart with a custom legend shape.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" LegendShape="LegendShape.Rectangle" Name="chart">
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
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
                if (_legendShape != value)
                {
                    _legendShape = value;

                    if (Container?._legendRenderer is not null)
                    {
                        Container._legendRenderer.RendererShouldRender = Visible;
                        Container._legendRenderer.UpdateLegendShape(Renderer);
                        Container._legendRenderer.ProcessRenderQueue();
                    }
                }
            }
        }

        /// <summary>
        /// Specifies the customization of the marker of the series.
        /// </summary>
        /// <value>A <see cref="ChartMarker"/> object controlling marker visibility and appearance.</value>
        /// <remarks>
        /// Use this property to configure the appearance of markers, which are used to identify individual data points within the series.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to render a chart with visible data point markers.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue">
        ///             <ChartMarker Visible="true"></ChartMarker>
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ChartMarker Marker { get; set; } = new();

        /// <summary>
        /// Provides options to configure the last data label for the series.
        /// </summary>
        internal ChartLastDataLabel LastDataLabel
        {
            get; set;
        } = new();

        /// <summary>
        /// Provides options to configure a linear gradient for a chart element owned by this series.
        /// </summary>
        /// <remarks>
        /// <see cref="ChartLinearGradient"/> is a child component that inherits from <see cref="LinearGradient"/>. Use
        /// <see cref="LinearGradient.X1"/>, <see cref="LinearGradient.Y1"/>, <see cref="LinearGradient.X2"/>, and <see cref="LinearGradient.Y2"/>
        /// to set the gradient direction, and define color transitions using <see cref="ChartGradientColorStops"/> with one or more
        /// <see cref="ChartGradientColorStop"/> elements.
        /// 
        /// Coordinate values are typically normalized to the range 0..1 relative to the paint box, where <c>(0,0)</c> is the top-left and
        /// <c>(1,1)</c> is the bottom-right. Gradient stop <c>Offset</c> commonly supports normalized 0..1 or percentage 0..100 values.
        /// 
        /// When nested under <see cref="ChartSeries"/>, <c>ChartIndicator</c>, or <c>ChartTrendline</c>, the gradient is applied
        /// automatically to the owning element. 
        /// </remarks>
  
        internal ChartLinearGradient? LinearGradient
        {
            get; set;
        } = new();
     

        /// <summary>
        /// Provides options to configure a radial gradient for a chart element owned by this series.
        /// </summary>
        /// <remarks>
        /// 
        /// <see cref="ChartRadialGradient"/> is a child component that inherits from <see cref="RadialGradient"/>. Set the center with
        /// <see cref="RadialGradient.Cx"/> and <see cref="RadialGradient.Cy"/>, the focal point with <see cref="RadialGradient.Fx"/> and
        /// <see cref="RadialGradient.Fy"/>, and the radius with <see cref="RadialGradient.R"/>. Define color transitions using
        /// <see cref="ChartGradientColorStops"/> containing one or more <see cref="ChartGradientColorStop"/> elements.
        /// 
        /// Coordinates are typically normalized to 0..1 relative to the gradient box; <c>(0.5, 0.5)</c> centers the gradient, and <c>R="0.5"</c>
        /// covers roughly half the box. Stop <c>Offset</c> may be specified as 0..1 or 0..100 (percent). 
        /// 
        /// When placed under <see cref="ChartSeries"/>, <c>ChartIndicator</c>, or <see cref="ChartTrendline"/>, the gradient is applied
        /// automatically. 
        /// </remarks>
       
        internal ChartRadialGradient? RadialGradient
        {
            get; set;
        } = new();
       

        /// <summary>
        /// Specifies the customization of the empty point settings for the series.
        /// </summary>
        /// <value>A <see cref="ChartEmptyPointSettings"/> instance for handling empty points.</value>
        /// <remarks>
        /// This property provides settings to handle and style data points that do not have values, such as display mode or custom markers.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to handle empty points in a column chart by averaging them,
        /// // and setting a custom fill color for the empty points.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" Type="ChartSeriesType.Column">
        ///             <ChartEmptyPointSettings Mode="EmptyPointMode.Average" Fill="red"></ChartEmptyPointSettings>
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ChartEmptyPointSettings EmptyPointSettings { get; set; } = new();

        /// <summary>
        /// Defines the space between adjacent series for rectangle-shaped series.
        /// </summary>
        /// <value>A double representing the spacing between series columns.</value>
        /// <remarks>
        /// This property determines the amount of space between columns in a column or bar series, allowing for better separation and visual clarity.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to render a column chart with custom column spacing.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" 
        ///                       Type="ChartSeriesType.Column" ColumnSpacing="10" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        private double _columnSpacing;
        [Parameter]
        public double ColumnSpacing
        {
            get => _columnSpacing;
            set
            {
                if (Math.Abs(_columnSpacing - value) > double.Epsilon)
                {
                    _columnSpacing = value;

                    if (Renderer is not null && Renderer.Series is not null && Renderer.Series.Visible)
                    {
                        Renderer.UpdateDirection();
                    }

                    Renderer?.Container?.AddToRenderQueue(Renderer);
                }
            }
        }

        /// <summary>
        /// Specifies the corner radius of the rectangle-shaped series.
        /// </summary>
        /// <value>A <see cref="ChartCornerRadius"/> configuring the series corner radius.</value>
        /// <remarks>
        /// Use this property to round the corners of rectangular series representations, enhancing their visual aesthetics by applying a uniform radius to corners.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to render a column chart with rounded corners.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" Type="ChartSeriesType.Column">
        ///             <ChartCornerRadius BottomLeft="20" BottomRight="20" TopLeft="20" TopRight="20">
        ///             </ChartCornerRadius>
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ChartCornerRadius CornerRadius { get; set; } = new();

        /// <summary>
        /// Specifies the column width of the rectangle-shaped series.
        /// </summary>
        /// <value>A double representing the relative width of columns.</value>
        /// <remarks>
        /// This property sets the relative width of columns in a column or bar series, affecting the appearance and spacing of the series columns.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to render a column chart with a custom column width.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue"
        ///                       Type="ChartSeriesType.Column" ColumnWidth="10" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        private double _columnWidth = double.NaN;
        [Parameter]
        public double ColumnWidth
        {
            get => _columnWidth;
            set
            {
                if (_columnWidth != value)
                {
                    _columnWidth = value;

                    if (Renderer is not null && Renderer.Series is not null && Renderer.Series.Visible)
                    {
                        Renderer.UpdateDirection();
                    }

                    Renderer?.Container?.AddToRenderQueue(Renderer);
                }
            }
        }

        /// <summary>
        /// Gets or sets the width of the columns, in pixels, for the <see cref="ChartSeries"/> points.
        /// </summary>
        /// <value>A double that defines the absolute width in pixels. Default is <see cref="double.NaN"/>.</value>
        /// <remarks>
        /// Use this property to customize the column width in the <see cref="ChartSeries"/>.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to render a column chart with a fixed column width in pixels.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue"
        ///                       Type="ChartSeriesType.Column" ColumnWidthInPixel="30" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        private double _columnWidthInPixel = double.NaN;
        [Parameter]
        public double ColumnWidthInPixel
        {
            get => _columnWidthInPixel;
            set
            {
                if (Math.Abs(_columnWidthInPixel - value) > double.Epsilon)
                {
                    _columnWidthInPixel = value;

                    if (Renderer is not null && Renderer.Series is not null && Renderer.Series.Visible)
                    {
                        Renderer?.UpdateDirection();
                    }

                    Renderer?.Container?.AddToRenderQueue(Renderer);
                }
            }
        }

        /// <summary>
        /// Specifies the group name used to stack series together.
        /// </summary>
        /// <value>A string representing the group name for stacking series.</value>
        /// <remarks>
        /// This property groups series together for stacking purposes, allowing multiple series to share the same stacking baseline within the chart.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to render a stacking bar chart with multiple groupings.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" 
        ///                       Type="ChartSeriesType.StackingBar" StackingGroup="Group1" />
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="Y1Value" 
        ///                       Type="ChartSeriesType.StackingBar" StackingGroup="Group2" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string StackingGroup { get; set; } = string.Empty;

        /// <summary>
        /// Specifies the axis used for segmentation.
        /// </summary>
        /// <value>A <see cref="Segment"/> indicating the segmentation axis.</value>
        /// <remarks>
        /// This property determines which axis will be used for segmenting the series, which can affect how series data is divided and displayed.
        /// </remarks>
        [Parameter]
        public Segment SegmentAxis { get; set; } = Segment.X;

        /// <summary>
        /// Specifies the tension for the Cardinal Spline in spline series.
        /// </summary>
        /// <value>A double value representing the tension. Default is <b>0.5</b>.</value>
        /// <remarks>
        /// This property adjusts the stiffness of the Cardinal Spline curve in spline series, affecting its smoothness and shape.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to render a SplineArea chart with cardinal spline tension.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue"
        ///                       Type="ChartSeriesType.SplineArea" CardinalSplineTension="1" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        private double _cardinalSplineTension = 0.5;
        [Parameter]
        public double CardinalSplineTension
        {
            get => _cardinalSplineTension;
            set
            {
                if (Math.Abs(_cardinalSplineTension - value) > double.Epsilon)
                {
                    _cardinalSplineTension = value;
                    _ = Renderer?.UpdateSeriesDataAsync();
                }
            }
        }

        /// <summary>
        /// Specifies the type of spline to be drawn in spline series.
        /// </summary>
        /// <value>A <see cref="SplineType"/> representing the method used to compute the spline curves.</value>
        /// <remarks>
        /// This property determines the method used to compute and render the spline curves in the chart, affecting their continuity and appearance.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to render a SplineArea chart using the Cardinal spline type.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue"
        ///                       Type="ChartSeriesType.SplineArea" SplineType="SplineType.Cardinal" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        private SplineType _splineType = SplineType.Natural;
        [Parameter]
        public SplineType SplineType
        {
            get => _splineType;
            set
            {
                if (_splineType != value)
                {
                    _splineType = value;
                    _ = Renderer?.UpdateSeriesDataAsync();
                }
            }
        }

        /// <summary>
        /// Defines the position of steps for step series.
        /// </summary>
        /// <value>A <see cref="StepPosition"/> describing the step alignment. Default is <b>Left</b>.</value>
        /// <remarks>
        /// This property specifies the alignment of the steps in a step chart, which can be set to the left, right, or center of the data points.
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to render a StepLine chart with the step position set to 'Right'.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue"
        ///                       Type="ChartSeriesType.StepLine" StepPosition="StepPosition.Right" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        private StepPosition _stepPosition = StepPosition.Left;
        [Parameter]
        public StepPosition StepPosition
        {
            get => _stepPosition;
            set
            {
                if (_stepPosition != value)
                {
                    _stepPosition = value;
                    Renderer?.UpdateDirection();
                }
            }
        }

        /// <summary>
        /// Specifies the maximum radius for bubbles in a bubble series.
        /// </summary>
        /// <value>A double value representing the maximum bubble size.</value>
        /// <remarks>
        /// This property sets the upper limit for the size of bubbles in a bubble chart, influencing how data values are visually represented as bubble sizes.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to render a bubble chart with a maximum radius of 30.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue"
        ///                      Type="ChartSeriesType.Bubble" MaxRadius="30" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double MaxRadius { get; set; } = 3;

        /// <summary>
        /// Specifies the minimum radius for bubbles in a bubble series.
        /// </summary>
        /// <value>A double representing the minimum bubble size.</value>
        /// <remarks>
        /// This property controls the smallest size a bubble can have in a bubble chart, affecting the visual representation of data points.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to render a bubble chart with a minimum radius of 30.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue"
        ///                      Type="ChartSeriesType.Bubble" MinRadius="30" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double MinRadius { get; set; } = 1;

        /// <summary>
        /// Specifies the field name in the data source used for mapping tooltips in waterfall series.
        /// </summary>
        /// <value>A string representing the data source field for tooltips.</value>
        /// <remarks>
        /// This property allows customization of tooltip content by using a specific field from the data source in a waterfall series.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to render a scatter chart with tooltip mapping for additional data.
        /// <SfChart DataSource="@Data">
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"></ChartPrimaryXAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries TooltipMappingName="Additional" XName="BS" YName="Rank" Type="ChartSeriesType.Scatter">
        ///             <ChartMarker Width="10" Height="10"></ChartMarker>
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        ///     <ChartTooltipSettings Enable=true Format="BS : ${point.x} <br> Rank : ${point.y} <br> Additional : ${point.tooltip}"></ChartTooltipSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string TooltipMappingName { get; set; } = string.Empty;

        /// <summary>
        /// Specifies the trendlines for the series.
        /// </summary>
        /// <value>A list of <see cref="ChartTrendline"/> objects representing trendlines.</value>
        /// <remarks>
        /// Trendlines predict and highlight trends in the data by displaying them over the series. They can be configured for different mathematical models such as linear or exponential.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to add a trendline in chart
        /// // and enable markers for it in a chart.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue">
        ///             <ChartTrendlines>
        ///                 <ChartTrendline Period="5" Type="TrendlineTypes.MovingAverage">
        ///                     <ChartTrendlineMarker Visible="true" />
        ///                 </ChartTrendline>
        ///             </ChartTrendlines>
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public IList<ChartTrendline> Trendlines { get; set; } = new List<ChartTrendline>();

        /// <summary>
        /// Specifies the segments of the multicolor series.
        /// </summary>
        /// <value>A list of <see cref="ChartSegment"/> objects that define multicolored segments within a series.</value>
        /// <remarks>
        /// Use this property to create multicolored segments for visual differentiation of parts within the same series, enhancing the interpretation of changes within segmented data.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the segments for a multicolored series.
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
        [Parameter]
        public IList<ChartSegment> Segments { get; set; } = [];

        /// <summary>
        /// Specifies the animation settings for the series.
        /// </summary>
        /// <value>A <see cref="ChartSeriesAnimation"/> object that defines animation behavior.</value>
        /// <remarks>
        /// This property allows customization of animation effects such as duration and delay to enhance the visual experience of series data as it transitions onto the chart.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to enable animation on a chart series with a specific duration and delay.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue">
        ///             <ChartSeriesAnimation Enable="true" Duration="3000" Delay="1000" />
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ChartSeriesAnimation Animation { get; set; } = new();

        /// <summary>
        /// Specifies the styling class used when the series is selected.
        /// </summary>
        /// <value>A string representing the CSS class applied to selected state.</value>
        /// <remarks>
        /// This property applies a specific CSS class to style the series when it is selected by the user.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to apply a custom style to selected chart points.
        /// <style>
        /// .selection {
        ///     fill: yellow;
        /// }
        /// </style>
        /// <SfChart SelectionMode="SelectionMode.Point">
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" 
        ///                      Type="ChartSeriesType.Column" SelectionStyle="selection" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string? SelectionStyle { get; set; }

        /// <summary>
        /// Specifies the styling class used when the series is deselected.
        /// </summary>
        /// <value>A string representing the CSS class applied to unselected state.</value>
        /// <remarks>
        /// This property applies a specific CSS class to style the series when it is not selected.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to apply a custom style to unselected chart points.
        /// <style>
        /// .selection {
        ///     fill: yellow;
        /// }
        /// .unselection {
        ///     fill: blue;
        /// }
        /// </style>
        /// <SfChart SelectionMode="SelectionMode.Point">
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" 
        ///                      Type="ChartSeriesType.Column" SelectionStyle="selection" UnSelectedStyle="unselection" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string UnSelectedStyle { get; set; } = null!;

        /// <summary>
        /// Specifies the styling class used when the series is not highlighted.
        /// </summary>
        /// <value>A string representing the CSS class applied to non-highlighted state.</value>
        /// <remarks>
        /// This property applies a specific CSS class to style the series when it is not highlighted by the user.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to apply a custom style to unhighlighted chart points.
        /// <style>
        /// .selection {
        ///     fill: yellow;
        /// }
        /// .unselection {
        ///     fill: blue;
        /// }
        /// </style>
        /// <SfChart SelectionMode="SelectionMode.Point">
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" 
        ///                      Type="ChartSeriesType.Column" HighlightStyle="selection" NonHighlightStyle="unselection" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string NonHighlightStyle { get; set; } = null!;

        /// <summary>
        /// Specifies the styling class used when the series is highlighted.
        /// </summary>
        /// <value>A string representing the CSS class applied to highlighted state.</value>
        /// <remarks>
        /// This property applies a specific CSS class to style the series when it is highlighted by the user.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to apply a custom style to highlight chart points.
        /// <style>
        /// .selection {
        ///     fill: yellow;
        /// }
        /// </style>
        /// <SfChart SelectionMode="SelectionMode.Point">
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" 
        ///                      Type="ChartSeriesType.Column" HighlightStyle="selection" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string HighlightStyle { get; set; } = null!;

        /// <summary>
        /// Configures drag settings for the series.
        /// </summary>
        /// <value>A <see cref="ChartDataEditSettings"/> describing interactive editing behavior.</value>
        /// <remarks>
        /// This property allows users to modify series data points directly on the chart by dragging, enhancing interactivity and data exploration.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to enable data editing in a column chart
        /// // and restrict the minimum editable Y-value to 30 using the MinY property.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" Name="England" XName="XValue" YName="YValue" Type="ChartSeriesType.Column">
        ///             <ChartDataEditSettings Enable="true" MinY="30"></ChartDataEditSettings>
        ///             <ChartMarker Visible="true" Width="10" Height="10" />
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ChartDataEditSettings ChartDataEditSettings
        {
            get => _chartDataEditSettings;
            set
            {
                if (_chartDataEditSettings != value)
                {
                    _chartDataEditSettings = value;
                    _chartDataEditSettings._isPropertyChanged = false;
                }
            }
        }

        /// <summary>
        /// Gets the owning chart (cascading).
        /// </summary>
        /// <value>The parent <see cref="SfChart"/> instance.</value>
        [CascadingParameter]
        internal SfChart? Container { get; set; }

        internal bool NeedRendererUpdate { get; set; }
        internal bool NeedRendererRemove { get; set; }
        internal bool UpdateDataSource { get; set; }
        internal string? SeriesType { get; set; }
        internal Size? LegendTemplateSize { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public string RendererKey { get; set; } = SfBaseUtils.GenerateID("chartseries");

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public Type RendererType { get; set; } = null!;

        internal IEnumerable<object> CurrentViewData { get; set; } = [];


        private ChartSeriesRenderer? _rendererProperty;
        internal ChartSeriesRenderer? Renderer
        {
            get => _rendererProperty;
            set
            {
                if (_rendererProperty != value)
                {
                    _rendererProperty = value;
                    _rendererProperty?.OnParentParameterSet();
                }
            }
        }

        #endregion

        #region Lifecycle Methods

        /// <exclude />
        /// <summary>
        /// Initializes the component, registers the series with the parent chart, resolves the renderer type,
        /// and primes the initial data query.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);
            Container?.AddSeries(this);
            UpdateSeriesKey();
            RendererType = ChartSeriesRenderer.GetRendererType(Type);
            _ = UpdateSeriesDataAsync().ConfigureAwait(false);
        }

        /// <exclude />
        /// <summary>
        /// Applies parameter updates and orchestrates minimal re-rendering based on what changed
        /// (layout, axis range, or series visuals).
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override async Task OnParametersSetAsync()
        {
            _shouldProcess = true;
            await base.OnParametersSetAsync().ConfigureAwait(false);

            SeriesType = Type.ToString();

            HandleContainerRefreshFlags();
            SubscribeToDataSourceIfNeeded();

            if (UpdateDataSource && NeedRendererUpdate)
            {
                NeedRendererUpdate = UpdateDataSource = false;
                await (Container?.ProcessOnLayoutChangeAsync()).ConfigureAwait(false);
            }

            else if (UpdateDataSource && !NeedRendererRemove && Container is not null && !Container._isOnceRendered)
            {
                await ProcessUpdateWithoutLayoutChangeAsync().ConfigureAwait(false);
            }

            if (_needLayoutUpdate)
            {
                _needLayoutUpdate = false;
                await Task.Delay(10).ConfigureAwait(false);
                await (Renderer?.Container?.Owner?.DelayLayoutChangeAsync()).ConfigureAwait(false);
            }

            else if (_refreshRange && Renderer?.XAxisRenderer is not null && Renderer.YAxisRenderer is not null)
            {
                if (Renderer.IsCategoryAxis())
                {
                    _refreshRange = false;
                    Container?.UpdateRenderers();
                    return;
                }

                Renderer.XAxisRenderer.ChangeAxisRange(_refreshRange);
                Renderer.YAxisRenderer.ChangeAxisRange(_refreshRange);
                _refreshRange = false;
                UpdateSeriesCollection();
            }

            else if (_refreshSeries && Container is not null && !Container._isOnceRendered)
            {
                _refreshSeries = false;
                if (Renderer is not null)
                {
                    UpdateSeries(Renderer);
                }
            }
        }

        /// <exclude />
        /// <summary>
        /// Invoked after the component has rendered.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            if (!firstRender)
            {
                _shouldProcess = false;
                if (Container != null)
                {
                    Container._isOnceRendered = false;
                }
            }
        }

        /// <summary>
        /// Disposes nested child components and unregisters event handlers.
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

        #region Private Methods

        /// <summary>
        /// Updates a single series renderer by either re-directing visuals or marking for render,
        /// and then processing its render queue.
        /// </summary>
        /// <param name="seriesRenderer">The renderer to update.</param>
        private static void UpdateSeries(ChartSeriesRenderer seriesRenderer)
        {
            if (seriesRenderer.Series is not null && seriesRenderer.Series.Visible)
            {
                seriesRenderer.UpdateDirection();
            }
            else
            {
                seriesRenderer.RendererShouldRender = true;

                if (seriesRenderer.Series?.Marker?.Renderer is not null)
                {
                    seriesRenderer.Series.Marker.Renderer.RendererShouldRender =
                        seriesRenderer.Series.Marker.Visible && seriesRenderer.Series.Container is not null && seriesRenderer.Series.Container._shouldRenderMarker;
                }

                if (seriesRenderer.Series?.Marker?.DataLabel?.Renderer is not null)
                {
                    seriesRenderer.Series.Marker.DataLabel.Renderer.RendererShouldRender =
                        seriesRenderer.Series.Marker.DataLabel.Visible && seriesRenderer.Series.Container is not null && seriesRenderer.Series.Container._shouldRenderDataLabel;
                }
            }

            seriesRenderer.ProcessRenderQueue();
        }

        /// <summary>
        /// Indicates whether the component is in a state where re-binding the data source should occur (initial render path).
        /// </summary>
        /// <returns><c>true</c> if data source update should occur; otherwise <c>false</c>.</returns>
        private bool IsUpdateDataSource()
        {
            return Container is not null && Container._isChartFirstRender && Renderer is not null && Renderer.IsSeriesRender && Renderer.XAxisRenderer is not null && Renderer.YAxisRenderer is not null;
        }

        /// <summary>
        /// Determines whether a change in <see cref="Fill"/> should also update marker or tooltip dependent visuals.
        /// </summary>
        /// <returns><c>true</c> if the direction should be updated; otherwise <c>false</c>.</returns>
        private bool IsUpdateDirectionOnFill()
        {
            return (Container?._tooltip?.Enable == true)
                       || (Marker?.Visible == true)
                       || (Type is ChartSeriesType.Scatter or ChartSeriesType.Bubble);
        }

        /// <summary>
        /// Ensures that a <see cref="DataManager"/> is available. It prefers the local <see cref="DataSource"/>,
        /// falling back to the parent chart's data source when necessary.
        /// </summary>
        /// <returns>An awaitable task.</returns>
        private Task EnsureDataManagerAsync()
        {
            if (DataSource is not null && DataManager is null)
            {
                _ = SetDataManager<object>(DataSource);
            }
            else if (Container?.DataSource is not null)
            {
                _ = SetDataManager<object>(Container.DataSource);
            }

            return Task.CompletedTask;
        }

        private async Task<object> GenerateAndExecuteQueryAsync(Query query)
        {
            return await DataManager.ExecuteQuery<object>(query).ConfigureAwait(false);
        }

        /// <summary>
        /// Normalizes container refresh flags to avoid unintended full refresh on subsequent parameter sets.
        /// </summary>
        private void HandleContainerRefreshFlags()
        {
            if (Container is not null && (Container._isRefreshed || Container._seriesChanged))
            {
                Container._isRefreshed = false;
                Container._seriesChanged = false;
            }
        }

        /// <summary>
        /// Subscribes to <see cref="INotifyCollectionChanged"/> data sources when appropriate to react to live updates.
        /// </summary>
        private void SubscribeToDataSourceIfNeeded()
        {
            if (Container is not null && DataSource is not null && !UpdateDataSource && IsINotifyImplements(DataSource))
            {
                ((INotifyCollectionChanged)DataSource).CollectionChanged += DataCollectionChanged;
            }
        }

        /// <summary>
        /// Processes data source changes that do not require removing the renderer, updating series data
        /// and possibly the legend or data label templates.
        /// </summary>
        /// <returns>An awaitable task.</returns>
        private async Task ProcessUpdateWithoutLayoutChangeAsync()
        {
            _ = Renderer?.UpdateSeriesDataAsync();
            Container!._isOnceRendered = true;

            IChartElementRenderer lastValidRenderer =
                Container._seriesContainer?.Renderers
                    .LastOrDefault(renderer => (renderer as ChartSeriesRenderer)?.Series?.DataSource is not null) ?? null!;

            if (lastValidRenderer is not null && (lastValidRenderer as ChartSeriesRenderer) == Renderer)
            {
                Container.UpdateRenderers();

                if (Renderer?.Series?.Marker?.DataLabel?.Template is not null)
                {
                    await Task.Delay(100).ConfigureAwait(false);
                    await Container.UpdateDatalabelTemplateAsync().ConfigureAwait(true);
                }
                if (Container?._selectionModule is not null)
                {
                    await Container._selectionModule.RemoveSelectedElementsAsync().ConfigureAwait(false);
                }
            }

            UpdateDataSource = false;
        }

        /// <summary>
        /// Determines whether label digit counts across series on the same axis differ enough to require a layout change.
        /// </summary>
        /// <returns><c>true</c> if layout change is required; otherwise <c>false</c>.</returns>
        private bool FindLayoutChange()
        {
            ChartSeriesRenderer? renderer = Renderer;
            ChartAxisRenderer? xAxisRenderer = renderer?.XAxisRenderer;
            ChartAxisRenderer? yAxisRenderer = renderer?.YAxisRenderer;

            if (renderer is null || xAxisRenderer is null || yAxisRenderer is null)
            {
                return false;
            }

            int xDigits = Math.Max(renderer.XMax.ToString(CultureInfo.InvariantCulture).Length, renderer.XMin.ToString(CultureInfo.InvariantCulture).Length);
            int yDigits = Math.Max(renderer.YMax.ToString(CultureInfo.InvariantCulture).Length, renderer.YMin.ToString(CultureInfo.InvariantCulture).Length);

            bool needLayoutUpdate = false;
            List<ChartSeriesRenderer> seriesCollection =
                ChartSeriesRendererContainer.FindAxisToSeriesCollection(xAxisRenderer, yAxisRenderer);

            foreach (ChartSeriesRenderer seriesRenderer in seriesCollection)
            {
                if (seriesRenderer != renderer)
                {

                    int xOtherDigits = Math.Max(seriesRenderer.XMax.ToString(CultureInfo.InvariantCulture).Length, seriesRenderer.XMin.ToString(CultureInfo.InvariantCulture).Length);
                    int yOtherDigits = Math.Max(seriesRenderer.YMax.ToString(CultureInfo.InvariantCulture).Length, seriesRenderer.YMin.ToString(CultureInfo.InvariantCulture).Length);

                    needLayoutUpdate = xAxisRenderer.Orientation == Orientation.Vertical
                        ? xDigits != xOtherDigits || needLayoutUpdate
                        : yDigits != yOtherDigits || needLayoutUpdate;
                }
            }

            return needLayoutUpdate;
        }

        /// <summary>
        /// Refreshes the series when datasource or item properties change, with throttling.
        /// </summary>
        /// <returns>A task that completes when refresh is processed or deferred.</returns>
        private async Task RefreshSeriesAsync()
        {
            if (Container is not null && Container._isRefreshed) { return; }

            if (IsUpdateDataSource())
            {
                Container?._svgRenderer?.RefreshElementList();

                Renderer.IsSeriesRender = false;
                _ = SetDataManager<object>((DataSource is not null) ? DataSource : Container?.DataSource);
                Container?._seriesContainer?.AddToRenderQueue(Renderer);
                UpdateDataSource = true;

                await Renderer.UpdateSeriesDataAsync().ConfigureAwait(false);
                if (!IsDisposed && Container is not null && Container._seriesContainer is not null &&
                    (Container._seriesContainer._previousRequestTime == DateTime.MinValue
                     || (DateTime.Now - Container._seriesContainer._previousRequestTime).TotalMilliseconds > UPDATE_THRESHOLD))
                {
                    Container._seriesContainer._previousRequestTime = DateTime.Now;
                    await Task.Delay(UPDATE_THRESHOLD).ConfigureAwait(false);
                    UpdateDataSource = false;

                    if (Renderer?.Owner?._tooltip is not null && Renderer.Owner._tooltip.Enable)
                    {
                        Renderer.Owner.GetChartPoints();
                    }
                }
            }
        }

        /// <summary>
        /// Checks whether an enumerable implements <see cref="INotifyCollectionChanged"/> to enable live updates.
        /// </summary>
        /// <param name="data">The source enumerable.</param>
        /// <returns><c>true</c> if the source supports collection change notifications; otherwise <c>false</c>.</returns>
        private static bool IsINotifyImplements(IEnumerable<object> data)
        {
            return data is INotifyCollectionChanged;
        }

        /// <summary>
        /// Prepares the SVG and selection state prior to toggling visibility via legend interaction.
        /// Clears cached SVG elements and resets axis rendering flags for an accurate redraw.
        /// </summary>
        private void PrepareForLegendToggle()
        {
            Container?._svgRenderer?.PathElementList?.Clear();
            Container?._svgRenderer?.EllipseElementList?.Clear();
            Container?._svgRenderer?.TextElementList?.Clear();
            Container?._selectionModule?.ClearDraggedRects();
            Container?._selectionModule?.OnPropertyChanged();
            Container?._parentRect?.ClearElements();

            if (Container != null)
            {
                Container._needAxisRendering = true;
            }

            CartesianAxisLayout._previousAxisEnd = 0;
            CartesianAxisLayout._previousAxis = null;
        }

        /// <summary>
        /// Resets container refresh flags used during re-render orchestration to avoid unintended full refresh.
        /// </summary>
        private void ResetContainerRefreshFlags()
        {
            if (Container is not null && (Container._isRefreshed || Container._seriesChanged))
            {
                Container._isRefreshed = false;
                Container._seriesChanged = false;
            }
        }

        /// <summary>
        /// Applies axis range changes after legend interaction and updates series/scrollbar accordingly.
        /// Handles both cartesian and polar/radar axis layouts.
        /// </summary>
        private void HandleLegendRangeChange()
        {
            if (Renderer.IsCategoryAxis() &&
               ((_labelPreviousCount > 0 && _labelCurrentCount == 0) || (_labelPreviousCount == 0 && _labelCurrentCount > 0)))
            {
                _refreshRange = false;
                Container?.UpdateRenderers();
                Container?.UpdateClientSideScrollbar();
                return;
            }

            Renderer.XAxisRenderer.ChangeAxisRange(_refreshRange);
            Renderer.YAxisRenderer.ChangeAxisRange(_refreshRange);
            _refreshRange = false;

            Container?.UpdateRenderers();
            Container?.UpdateClientSideScrollbar();
        }

        /// <summary>
        /// Checks whether two enumerable data sources contain different items or ordering.
        /// </summary>
        /// <param name="previousdata">The previous data sequence.</param>
        /// <param name="currentData">The current data sequence.</param>
        /// <param name="dataChanged">Outputs <c>true</c> when difference is detected.</param>
        /// <returns><c>true</c> when data changed; otherwise <c>false</c>.</returns>
        private static bool CheckDatasourceChanged(IEnumerable<object> previousdata, IEnumerable<object> currentData, out bool dataChanged)
        {
            List<object> previous = [.. previousdata];
            List<object> current = [.. currentData];

            dataChanged = previous.Except(current).Any() || !previous.SequenceEqual(current);
            return dataChanged;
        }

        #endregion

        #region Internal Methods
        /// <summary>
        /// Disposes nested child components and unregisters event handlers.
        /// </summary>
        internal void ComponentDispose()
        {
            if (DataSource is INotifyCollectionChanged changed)
            {
                changed.CollectionChanged -= DataCollectionChanged;
            }

            Container?._seriesContainer?.RemoveElement(this);

            Animation?.ComponentDispose();
            EmptyPointSettings?.ComponentDispose();
            Marker?.ComponentDispose();
            ChartDataEditSettings?.ComponentDispose();
            LastDataLabel?.ComponentDispose();
            LinearGradient?.ComponentDispose();
            RadialGradient?.ComponentDispose();

            LastDataLabel = null!;
            LegendItemTemplate = null!;
            LinearGradient = null!;
            RadialGradient = null!;
            LinearGradient = null;
            RadialGradient = null;
        }
        /// <summary>
        /// Handles collection change events from an <see cref="INotifyCollectionChanged"/> data source and triggers a throttled refresh.
        /// </summary>
        /// <param name="source">The notifying collection.</param>
        /// <param name="e">Change details.</param>
        internal void DataCollectionChanged(object? source, NotifyCollectionChangedEventArgs e)
        {
            if (Container != null)
            {
                Container._isLiveChart = Renderer.Series is null || !Renderer.Series._isSeriesChanged;
            }

            _ = RefreshSeriesAsync();
        }

        /// <summary>
        /// Handles item-level property changes (when data items implement <see cref="INotifyPropertyChanged"/>),
        /// triggering a throttled refresh to reflect the change on the chart.
        /// </summary>
        /// <param name="sender">The changed data item.</param>
        /// <param name="e">Change details.</param>
        internal void PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            _ = RefreshSeriesAsync();
        }

        /// <summary>
        /// Handles legend click toggling for this series.
        /// </summary>
        /// <param name="value">A boolean indicating desired visibility.</param>
        internal void OnLegendClick(bool value)
        {
            PrepareForLegendToggle();
            Visible = value;

            Container?._legendRenderer?.RefreshSeriesPosition();
            ResetContainerRefreshFlags();

            if (_needLayoutUpdate)
            {
                _needLayoutUpdate = false;
                Container?.UpdateRenderers();
                Container?.UpdateClientSideScrollbar();
            }
            else if (_refreshRange)
            {
                HandleLegendRangeChange();
            }

            Container?._annotationContainer?.UpdateRenderers();
        }

        /// <summary>
        /// Ensures the series key is registered in the container for index-based color/legend resolution.
        /// </summary>
        internal void UpdateSeriesKey()
        {
            if (Container?._seriesContainer is not null && !Container._seriesContainer._seriesIndexes.ContainsKey(GenerateSeriesKey()))
            {
                Container._seriesContainer._seriesIndexes[GenerateSeriesKey()] = Container._seriesContainer._seriesIndexes.Count;
            }
        }

        /// <summary>
        /// Generates a key used for series indexing in the container based on data identity and key visuals.
        /// </summary>
        /// <returns>A stable string key.</returns>
        internal string GenerateSeriesKey()
        {
            string key = string.Empty;
            if (DataSource is not null)
            {
                key += DataSource.GetHashCode();
            }
            key += Name + Fill;
            return key;
        }

        /// <summary>
        /// Updates nested sub-component instances when they are set from markup.
        /// </summary>
        /// <param name="key">The property name being updated.</param>
        /// <param name="keyValue">The property value to apply.</param>
        internal void UpdateSeriesProperties(string key, object keyValue)
        {
            switch (key)
            {
                case nameof(Marker):
                    Marker = (ChartMarker)keyValue;
                    break;
                case nameof(Trendlines):
                    Trendlines = (IList<ChartTrendline>)keyValue;
                    break;
                case nameof(Segments):
                    Segments = (IList<ChartSegment>)keyValue;
                    break;
                case nameof(Border):
                    Border = (ChartSeriesBorder)keyValue;
                    break;
                case nameof(CornerRadius):
                    CornerRadius = (ChartCornerRadius)keyValue;
                    break;
                case nameof(Animation):
                    Animation = (ChartSeriesAnimation)keyValue;
                    break;
                case nameof(ChartDataEditSettings):
                    ChartDataEditSettings = (ChartDataEditSettings)keyValue;
                    break;
                case nameof(EmptyPointSettings):
                    EmptyPointSettings = (ChartEmptyPointSettings)keyValue;
                    break;
                case nameof(LastDataLabel):
                    LastDataLabel = (ChartLastDataLabel)keyValue;
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
        /// Ensures the component has a data manager (from DataSource or parent) and retrieves current page of data.
        /// </summary>
        /// <returns>The current view data set.</returns>
        internal async Task<IEnumerable<object>> UpdateSeriesDataAsync()
        {
            await EnsureDataManagerAsync().ConfigureAwait(false);

            if (DataManager is null)
            {
                CurrentViewData = [];
                if (Renderer != null)
                {
                    Renderer.CurrentViewData = [];
                }
                return CurrentViewData;
            }

            DataManager.DataAdaptor.SetRunSyncOnce(true);

            Query query = Query ?? new Query();

            object data = await GenerateAndExecuteQueryAsync(query).ConfigureAwait(false);
            if (data is not null)
            {
                CurrentViewData = (IEnumerable<object>)data;
            }

            return CurrentViewData;
        }

        /// <summary>
        /// Updates all series sharing the same axes as this series.
        /// </summary>
        internal void UpdateSeriesCollection()
        {
            ChartSeriesRenderer? renderer = Renderer;
            ChartAxisRenderer? xAxisRenderer = renderer?.XAxisRenderer;
            ChartAxisRenderer? yAxisRenderer = renderer?.YAxisRenderer;

            if (renderer is null || xAxisRenderer is null || yAxisRenderer is null)
            {
                return;
            }

            List<ChartSeriesRenderer> seriesCollection =
                ChartSeriesRendererContainer.FindAxisToSeriesCollection(xAxisRenderer, yAxisRenderer);

            Container?._seriesContainer?._dataLabelCollection.Clear();

            foreach (ChartSeriesRenderer seriesRenderer in seriesCollection)
            {
                if (seriesRenderer.ClipRect is not null)
                {
                    UpdateSeries(seriesRenderer);
                }
            }
        }

        /// <summary>
        /// Applies a set of common trendline properties to this series.
        /// </summary>
        /// <param name="name">Name of the series.</param>
        /// <param name="xname">X member.</param>
        /// <param name="yname">Y member.</param>
        /// <param name="dashArray">Dash array for stroke.</param>
        /// <param name="width">Stroke width.</param>
        /// <param name="fill">Stroke/fill color.</param>
        /// <param name="legendShape">Legend shape.</param>
        /// <param name="tooltip">Enable tooltip.</param>
        /// <param name="border">Border settings.</param>
        /// <param name="accessibilityDescription">Accessibility description for the series.</param>
        /// <param name="accessibilityDescriptionFormat">Accessibility format for points.</param>
        /// <param name="accessibilityRole">ARIA role.</param>
        /// <param name="focusable">Focusable flag.</param>
        internal void SetTrendlineValues(
            string name, string xname, string yname, string dashArray, double width, string fill, LegendShape legendShape,
            bool tooltip, ChartSeriesBorder border,
            string accessibilityDescription, string accessibilityDescriptionFormat, string accessibilityRole, bool focusable)
        {
            SetName(name);
            XName = xname;
            YName = yname;
            Fill = fill;
            Width = width;
            DashArray = dashArray;
            EnableTooltip = tooltip;
            LegendShape = legendShape;
            Border = border;
            AccessibilityDescription = accessibilityDescription;
            AccessibilityDescriptionFormat = accessibilityDescriptionFormat;
            AccessibilityRole = accessibilityRole;
            Focusable = focusable;
        }

        /// <summary>
        /// Sets the display name of the series.
        /// </summary>
        /// <param name="name">The series name to use.</param>
        internal void SetName(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Sets the series <see cref="ChartSeriesType"/> used for rendering.
        /// </summary>
        /// <param name="type">The trendline series type to apply.</param>
        internal void SetTrendlineType(ChartSeriesType type)
        {
            Type = type;
        }

        /// <summary>
        /// Sets the <see cref="LegendShape"/> used by this series in the legend.
        /// </summary>
        /// <param name="shape">Desired legend shape.</param>
        internal void SetLegendShape(LegendShape shape)
        {
            LegendShape = shape;
        }

        #endregion
    }


}
