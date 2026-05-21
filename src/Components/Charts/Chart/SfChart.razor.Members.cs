using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Internal;
using System.ComponentModel;
using Syncfusion.Blazor.Toolkit.Charts.Internal;
using System.Collections.Specialized;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Represents the SfChart component which is used to render various types of charts in a Blazor application.
    /// </summary>
    public partial class SfChart
    {
        #region Constants
        private const string DefaultHeight = "100%";
        private const string DefaultWidth = "100%";
        #endregion

        #region Fields

        private string _highlightColor = null!;
        private string _background = string.Empty;
        private string _title = string.Empty;
        private string _subTitle = string.Empty;
        private string _height = DefaultHeight;
        private string _width = DefaultWidth;
        private string[] _palettes = [];
        private bool _isMultiSelect;
        private bool _isTransposed;
        private bool _enableSideBySidePlacement = true;
        private bool _updateLayout;
        private HighlightMode _highlightMode;
        private ChartSelectionMode _selectionMode;
        private SelectionPattern _selectionPattern;
        private SelectionPattern _highlightPattern;
        private IEnumerable<object> _dataSource = null!;
        private Theme _theme = Theme.Fluent;

        #endregion

        internal ZoomContent? _zoomingContent;
        internal ZoomToolkit? _zoomingToolkitContent;

        #region Members

        /// <summary> 
        /// Gets or sets the height of the chart as a string. 
        /// </summary> 
        /// <value> 
        /// The height of the chart as a string value. The default value is <b>”100%”</b>. 
        /// </value> 
        /// <remarks> 
        /// Accepts input as either pixel or percentage. 
        /// If specified as '100%', the chart renders to the full height of its parent element. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the height of a Syncfusion Blazor Chart.
        /// <SfChart Height="400px">
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string Height { get; set; } = null!;

        /// <summary> 
        /// Gets or sets the width of the chart as a string. 
        /// </summary> 
        /// <value> 
        /// The width of chart as string value. 
        /// </value> 
        /// <remarks> 
        /// Accepts input as either pixel or percentage. 
        /// If specified as '100%', the chart renders to the full width of its parent element. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the Width of a Syncfusion Blazor Chart.
        /// <SfChart Width="400px">
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string Width { get; set; } = DefaultWidth;

        /// <summary> 
        /// Gets or sets the title of the chart component. 
        /// </summary> 
        /// <value> 
        /// A string representing the title of the chart. The default value is an empty string. 
        /// </value> 
        /// <remarks>
        /// This property is used to provide a title for the chart component, which will be displayed above the chart by default.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the title of a Syncfusion Blazor Chart.
        /// <SfChart Title="Chart Title">
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string Title { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets the subtitle of the chart component. 
        /// </summary> 
        /// <value> 
        /// A string representing the subtitle of the chart. The default value is an empty string. 
        /// </value> 
        /// <remarks> 
        /// Applicable only when <see cref="Title"/> is provided. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the subtitle of a Syncfusion Blazor Chart.
        /// <SfChart Title="Chart Title" SubTitle="Chart SubTitle">
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string SubTitle { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the accessibility description for the <see cref="SfChart">Chart</see> component.
        /// </summary>
        /// <value>
        /// Accepts a string that defines the accessibility description for the <see cref="SfChart">Chart</see> component. The default value is an empty string.
        /// </value>
        /// <remarks>
        /// Use this property to provide an accessibility description for the <see cref="SfChart">Chart</see> component.
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the accessibility description for a Syncfusion Blazor Chart.
        /// <SfChart AccessibilityDescription="Chart Description">
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string AccessibilityDescription { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the accessibility role for the <see cref="SfChart">Chart</see> component.
        /// </summary>
        /// <value>
        /// Accepts a string that defines the accessibility role for the <see cref="SfChart">Chart</see> component. The default value is <b>null</b>.
        /// </value>
        /// <remarks>
        /// Use this property to provide an accessibility role for the <see cref="SfChart">Chart</see> component.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the accessibility role for a Syncfusion Blazor Chart.
        /// <SfChart AccessibilityRole="Chart Description">
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string AccessibilityRole { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the accessibility keyboard navigation focus option for the <see cref="SfChart">Chart</see> component.
        /// </summary>
        /// <value>
        /// Accepts the boolean value to enable or disable the keyboard navigation for the <see cref="SfChart">Chart</see> component. The default value is <b>true</b>.
        /// </value>
        /// <remarks>
        /// Use this property to toggle the keyboard navigation focus for the <see cref="SfChart">Chart</see> component.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the accessibility keyboard navigation focus for a Syncfusion Blazor Chart.
        /// <SfChart Focusable="true">
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool Focusable { get; set; } = true;

        /// <summary>
        /// Gets or sets the focus border color for the <see cref="SfChart">Chart</see> component.
        /// </summary>
        /// <value>
        /// Accepts a string that defines the focus border color for the <see cref="SfChart">Chart</see> component. The default value is <b>null</b>.
        /// </value>
        /// <remarks>
        /// Use this property to customize the focus border color for the <see cref="SfChart">Chart</see> component. By default, the focus border color is set based on the theme.
        /// This <see cref="FocusBorderColor"/> property is only applicable when the <see cref="Focusable"/> property is set to true.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to enable focus and customize the focus border color in a Syncfusion Blazor Chart.
        /// <SfChart Focusable="true" FocusBorderWidth="2" FocusBorderColor="red" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string FocusBorderColor { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the focus border width for the <see cref="SfChart">Chart</see> component.
        /// </summary> 
        /// <value>
        /// Accepts a double value in pixels that defines the focus border width for the <see cref="SfChart">Chart</see> component. The default value is <b>1.5</b>.
        /// </value>
        /// <remarks>
        /// Use this property to customize the focus border width for the <see cref="SfChart">Chart</see> component.
        /// This <see cref="FocusBorderWidth"/> property is only applicable when the <see cref="Focusable"/> property is set to true.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to enable focus and customize the focus border width in a Syncfusion Blazor Chart.
        /// <SfChart Focusable="true" FocusBorderWidth="2" FocusBorderColor="red" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double FocusBorderWidth { get; set; } = 1.5;


        /// <summary>
        /// Gets or sets the focus border margin for the <see cref="SfChart">Chart</see> component.
        /// </summary>
        /// <value>
        /// Accepts a double value in pixels that defines the focus border margin for the <see cref="SfChart">Chart</see> component. The default value is <b>0</b>.
        /// </value>
        /// <remarks>
        /// Use this property to customize the focus border margin for the <see cref="SfChart">Chart</see> component.
        /// This <see cref="FocusBorderMargin"/> property is only applicable when the <see cref="Focusable"/> property is set to true.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set focus border margin in a Syncfusion Blazor Chart.
        /// <SfChart Focusable="true" FocusBorderMargin="5" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double FocusBorderMargin { get; set; } = 0;

        /// <summary> 
        /// Gets or sets the ID of the chart component. 
        /// </summary> 
        /// <value> 
        /// A string representing the ID of the chart component. 
        /// </value> 
        /// <remarks>
        /// This property is used to uniquely identify the chart component in the DOM.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set an ID for a Syncfusion Blazor Chart.
        /// <SfChart ID="Chart" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string ID { get; set; } = SfBaseUtils.GenerateID("chart");

        /// <summary>
        /// Gets or sets whether the chart should be rendered in a transposed manner. 
        /// </summary> 
        /// <value> 
        /// <b>true</b> if transposing can be enabled; otherwise, <b>false</b>. The default value is <b>false</b>. 
        /// </value> 
        /// <remarks> 
        /// If set to <c>true</c>, the chart will be rendered in a transposed manner with the horizontal axis placed as the vertical axis and vice versa. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to render a transposed column chart.
        /// <SfChart IsTransposed="true">
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" Type="ChartSeriesType.Column" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool IsTransposed { get; set; }

        /// <summary> 
        /// Gets or sets the theme for the chart. 
        /// </summary>
        /// <remarks> 
        /// Chart element's color and text get modified, such as fill, font size, font family, and font style, which enhances the overall chart appearance based on the predefined theme applied. 
        /// </remarks> 
        [Parameter]
        public Theme Theme { get; set; }

        /// <summary> 
        /// Gets or sets the palette for the chart series. 
        /// </summary> 
        /// <value> 
        /// Accepts a string array that specifies the palette for chart series. The default value is an empty string array. 
        /// </value> 
        /// <remarks> 
        /// Multiple series will be applied with fill color based on the order of values in the palette array. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to apply a custom color palette to a Syncfusion Blazor Chart.
        /// <SfChart Palettes='new string[]{"red", "blue"}'>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y" Type="ChartSeriesType.Column" />
        ///         <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y1" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string[] Palettes { get; set; } = [];

        /// <summary> 
        /// Gets or sets the data source for the chart. 
        /// </summary> 
        /// <value>
        /// <![CDATA[An IEnumerable<object> collection representing the data source for the chart.]]>
        /// </value> 
        /// <remarks>  
        /// Accepts a collection of objects such as JSON objects, ExpandoObject, DynamicDictionary, ObservableCollection, or an instance of DataManager.  
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to bind a data source directly to the SfChart component in a Syncfusion Blazor Chart.
        /// <SfChart DataSource="@WeatherReports">
        ///     <ChartSeriesCollection>
        ///         <ChartSeries XName="X" YName="Y" Type="ChartSeriesType.Column" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public IEnumerable<object> DataSource { get; set; } = null!;

        /// <summary> 
        /// Gets or sets the background color of the chart.  
        /// </summary> 
        /// <value> 
        /// A string value specifying the background color of the chart. The default background color is determined by the chart's theme. By default, the theme is set to Fluent with a background color of <b>#FFFFFF</b>. 
        /// </value> 
        /// <remarks> 
        /// The value can be specified in hex or rgba format, following valid CSS color string conventions. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the background color of the chart.
        /// <SfChart Background="red"></SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string Background { get; set; } = string.Empty;

        /// <summary> 
        /// Gets a value indicating whether the right-to-left (RTL) direction is enabled from global options.
        /// </summary>
        /// <value>
        /// <b>true</b> if the right-to-left direction is enabled in `SyncfusionService` options; otherwise, <b>false</b>.
        /// </value>
        internal bool EnableRtl => SyncfusionService?._options?.EnableRtl ?? false;

        /// <summary> 
        /// Gets or sets a value indicating whether to enable side-by-side placement of series. 
        /// </summary> 
        /// <value> 
        /// <b>true</b> if side-by-side placement can be enabled; otherwise, <b>false</b>. The default value is <b>true</b>. 
        /// </value> 
        /// <remarks> 
        /// This property is applicable only for the below mentioned chart series types: 
        /// - <b>Column</b> 
        /// - <b>Range Column</b> 
        /// - <b>Bar</b> 
        /// - <b>Box and Whisker</b> 
        /// - <b>Waterfall</b> 
        /// - <b>Histogram</b> 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to disable side-by-side placement for column series in a chart.
        /// <SfChart EnableSideBySidePlacement="false">
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" Type="ChartSeriesType.Column" />
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="Y1Value" Type="ChartSeriesType.Column" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example> 
        [Parameter]
        public bool EnableSideBySidePlacement { get; set; } = true;

        /// <summary> 
        /// Gets or sets whether both axis intervals should be calculated automatically with respect to the zoomed range. 
        /// </summary> 
        /// <value> 
        /// <b>true</b> if the intervals for both axes should be calculated automatically based on the zoom range; otherwise, <b>false</b>. 
        /// The default value is <b>false</b>. 
        /// </value> 
        /// <remarks> 
        /// This property affects axis intervals only when the chart is zoomed. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to enable automatic interval adjustment on both axes.
        /// <SfChart EnableAutoIntervalOnBothAxis="true">
        ///     <ChartZoomSettings ToolbarDisplayMode="ToolbarMode.Always"></ChartZoomSettings>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y" Type="ChartSeriesType.Column" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool EnableAutoIntervalOnBothAxis { get; set; }

        /// <summary> 
        /// Gets or sets the background image for the chart. 
        /// </summary> 
        /// <value> 
        /// A string representing the URL or path to the background image. The default value is an empty string. 
        /// </value> 
        /// <remarks>
        /// This property is used to set a background image for the chart to enhance the visual representation of the data.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set a background image in a Syncfusion Blazor Chart.
        /// <SfChart BackgroundImage="https://example.com/image.png">
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string BackgroundImage { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets the highlight color for the chart. 
        /// </summary> 
        /// <value> 
        /// A string representing the highlight color. Accepts valid CSS color string values. 
        /// </value> 
        /// <remarks> 
        /// Applicable only when <see cref="HighlightMode"/> is applied. 
        /// Chart points, series or a cluster of points (based on the <see cref="HighlightMode"/> applied and chart series type) will be displayed in the provided HighlightColor when the user hovers over them. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to enable series highlight color in a Syncfusion Blazor Chart.
        /// <SfChart HighlightColor="blue" HighlightMode="HighlightMode.Series">
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y" Type="ChartSeriesType.Column" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string HighlightColor { get; set; } = null!;

        /// <summary> 
        /// Gets or sets the selection mode of the chart component. 
        /// </summary> 
        /// <value> 
        /// One of the <see cref="SelectionMode" /> enumerations that specifies the selection mode of the chart component. 
        /// The options include: 
        ///  - <c>None</c>: No selection will occur. 
        ///  - <c>Series</c>: Selects a series. 
        ///  - <c>Point</c>: Selects a single point. 
        ///  - <c>Cluster</c>: Selects the points in all series that correspond to the same index. 
        /// <br/>
        /// The default value is <b>ChartSelectionMode.None</b>. 
        /// </value> 
        /// <remarks>
        /// This property determines how the user can select elements within the chart, which can enhance user interaction and data analysis.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to enable drag-to-select functionality in a Syncfusion Blazor Chart.
        /// <SfChart SelectionMode="ChartSelectionMode.DragXY">
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y" Type="ChartSeriesType.Column" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ChartSelectionMode SelectionMode { get; set; }

        /// <summary> 
        /// Gets or sets the highlight mode of the chart component. 
        /// </summary> 
        /// <value> 
        /// One of the <see cref="HighlightMode"/> enumeration that specifies the highlight mode of the chart component. 
        /// The options include: 
        ///  - <c>None</c>: No highlighting will occur. 
        ///  - <c>Series</c>: Series will be highlighted when hovered. 
        ///  - <c>Point</c>: Point will be highlighted when hovered. 
        ///  - <c>Cluster</c>: A cluster of points in all series that correspond to the same index will be highlighted when hovered.
        /// <br/>
        /// The default value is <b>HighlightMode.None</b>. 
        /// </value>
        /// <remarks>
        /// This property determines how the chart elements will be highlighted based on user interaction, enhancing the visual feedback during data exploration.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to enable series highlighting in a Syncfusion Blazor Chart.
        /// <SfChart HighlightColor="true" HighlightMode="HighlightMode.Series">
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y" Type="ChartSeriesType.Column" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public HighlightMode HighlightMode { get; set; }

        private async Task CallJSInteropForSelectionHighlightOptionAsync(bool is_selectionModule = false)
        {
            if (_isScriptLoaded)
            {
                await InvokeVoidAsync(_chartJsModule!, _chartJsInProcessModule!, Constants.SetHighlightSelectionOptions, [_dataId, GetSelectionHighlightOptions()]).ConfigureAwait(false);
                if (is_selectionModule)
                {
                    await InvokeVoidAsync(_chartJsModule!, _chartJsInProcessModule!, Constants.SelectDataIndex, [_dataId, _selectedDataIndexes?.ToArray() ?? []]).ConfigureAwait(false);
                }
            }
        }

        /// <summary> 
        /// Gets or sets the selection pattern of the chart component. 
        /// </summary> 
        /// <value> 
        /// One of the <see cref="SelectionPattern" /> enumerations that specifies the selecting patterns. 
        /// The default value is <b>SelectionPattern.None</b>. 
        /// </value> 
        /// <remarks> 
        /// The provided pattern will be displayed on points, series, or a cluster of points (based on the <see cref="ChartSelectionMode"/> applied). 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to apply a triangle selection pattern to a series in a Syncfusion Blazor Chart.
        /// <SfChart SelectionMode="ChartSelectionMode.Series" SelectionPattern="SelectionPattern.Triangle">
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y" Type="ChartSeriesType.Column" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public SelectionPattern SelectionPattern { get; set; }

        /// <summary> 
        /// Gets or sets the highlight pattern of the chart component. 
        /// </summary> 
        /// <value> 
        /// One of the <see cref="SelectionPattern"/> enumeration that specifies the highlighting patterns. 
        /// The default value is <b>SelectionPattern.None</b>. 
        /// </value> 
        /// <remarks> 
        /// The provided pattern will be displayed on points, series, or a cluster of points (based on the <see cref="HighlightMode"/> applied). 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to highlight a series with a triangle pattern in a Syncfusion Blazor Chart.
        /// <SfChart HighlightMode="HighlightMode.Series" HighlightPattern="SelectionPattern.Triangle">
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y" Type="ChartSeriesType.Column" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public SelectionPattern HighlightPattern { get; set; }

        /// <summary> 
        /// Gets or sets a value indicating whether multiple selection is enabled in the chart. 
        /// </summary> 
        /// <value> 
        /// <b>true</b> if multiple selection can be enabled; otherwise, <b>false</b>. 
        /// The default value is <b>false</b>. 
        /// </value> 
        /// <remarks> 
        /// This property is applicable only when <see cref="ChartSelectionMode"/> is applied. Enabling this property allows the selection of multiple points or series (based on the <see cref="ChartSelectionMode"/> applied). 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to enable multiple series selection with a triangle pattern in a Syncfusion Blazor Chart.
        /// <SfChart SelectionMode="ChartSelectionMode.Series" SelectionPattern="SelectionPattern.Triangle" AllowMultiSelection="true">
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y" Type="ChartSeriesType.Column" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool AllowMultiSelection { get; set; }

        /// <summary> 
        /// Gets or sets the option to enable the grouping separator for the numeric axis labels. 
        /// </summary> 
        /// <value> 
        /// <b>true</b> if the grouping separator can be enabled; otherwise, <b>false</b>. The default value is <b>false</b>. 
        /// </value> 
        /// <remarks> 
        /// If set to <b>true</b>, numeric axis labels will be rendered with a grouping separator. 
        /// For example, 2000 will be rendered as 2,000. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to use grouping separators for numeric values in a Syncfusion Blazor Chart.
        /// <SfChart UseGroupingSeparator="true">
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool UseGroupingSeparator { get; set; }

        /// <summary> 
        /// Gets or sets the tabindex of the chart title for accessibility purposes. 
        /// </summary> 
        /// <value> 
        /// A double value representing the tab index of the chart title. The default value is <b>0</b>. 
        /// </value> 
        /// <remarks>
        /// This property determines the order in which the chart title receives focus when navigating through the chart elements.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the tab index of a Syncfusion Blazor Chart component.
        /// <SfChart TabIndex="1">
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y" Type="ChartSeriesType.Column" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double TabIndex { get; set; } = 0;

        /// <summary> 
        /// Gets or sets the value that indicates whether animation is enabled for the chart. 
        /// </summary> 
        /// <value> 
        /// <b>true</b> if animation is enabled; otherwise, <b>false</b>. The default value is <b>true</b>. 
        /// </value> 
        /// <remarks> 
        /// If set to <b>true</b>, chart elements such as series, axis, axis labels, major and minor gridlines, major and minor ticklines will be animated. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to enable animation in a Syncfusion Blazor Chart.
        /// <SfChart EnableAnimation="true">
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y" Type="ChartSeriesType.Column" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool EnableAnimation { get; set; } = true;

        /// <summary> 
        /// Gets or sets the custom class for the chart. 
        /// </summary> 
        /// <value> 
        /// A string representing the custom class. The default value is an empty string. 
        /// </value> 
        /// <remarks> 
        /// The provided custom class will be appended to the chart element, allowing customization of the element style. 
        /// </remarks> 
        /// <example> 
        /// The following example demonstrates how to customize the chart element style using a custom class: 
        /// <code> 
        /// <![CDATA[   
        /// <SfChart CustomClass="@customClass"> 
        ///     <!-- Other chart configurations -->  
        /// </SfChart> 
        /// <style> 
        ///    .chartcustomclass { 
        ///        width: 100%; 
        ///    } 
        /// </style> 
        /// @code {  
        ///     string customClass = "chartcustomclass"; 
        /// }  
        /// ]]> 
        /// </code> 
        /// </example>  
        [Parameter]
        public string CustomClass { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether adaptive rendering is enabled for the <see cref="SfChart">Chart</see> component.
        /// </summary>
        /// <value>
        /// A boolean value that enables or disables adaptive rendering for the <see cref="SfChart">Chart</see> component. The default value is false.
        /// </value>
        /// <remarks>
        /// When adaptive rendering is enabled, the chart will render with optimized elements based on the device resolution. This setting may override certain chart element properties such as titles, axis labels, axis titles, data labels, and legends. For example, when the screen size is 300x300, axis labels may move inside, axis titles may be hidden, and the legend position may adjust to fit the screen size, among other changes.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to enable adaptive rendering in a Syncfusion Blazor Chart.
        /// <SfChart EnableAdaptiveRendering="true" Height="300" Width="300" Title="chart">
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y" Type="ChartSeriesType.Column" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool EnableAdaptiveRendering { get; set; }

        /// <summary>
        /// Specifies the template to be displayed when the chart has no data.
        /// </summary>
        /// <value>
        /// Accepts a <see cref="RenderFragment"/> that allows rendering custom content when the chart has no data.
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <SfChart>
        ///     <NoDataTemplate>
        ///         <div>No data available to display.</div>
        ///     </NoDataTemplate>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        /// <remarks>
        /// This template enables users to display customized messages, images, or other UI elements in place of an empty chart. 
        /// It provides a better user experience by offering context when no data points are available.
        /// </remarks>
        [Parameter]
        public RenderFragment NoDataTemplate { get; set; } = null!;

        /// <summary>
        /// Gets or sets a function that returns the top position of the tooltip.
        /// </summary>
        /// <value>
        /// A function delegate that returns a <see cref="double"/> value representing the tooltip top position in pixels.
        /// </value>
        /// <remarks>
        /// This property is used internally to position the tooltip during chart interactions.
        /// </remarks>
        /// <inheritdoc />
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        [Parameter]
        public Func<double> GetTooltipTop { get; set; } = null!;

        #endregion

        #region Private Methods

        private void OnThemeChanged()
        {
            _chartBorderRenderer?.OnThemeChanged();
            _chartAreaRenderer?.OnThemeChanged();
            _chartTitleRenderer?.OnThemeChanged();
            _axisContainer?.OnThemeChanged();
            _seriesContainer?.OnThemeChanged();
            _legendRenderer?.OnThemeChanged();
            _stackLabelRenderer?.OnThemeChanged();
            _ = SetTooltipDataAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Recalculates chart dimensions, initializes axes and updates layout as needed.
        /// </summary>
        private async Task OnDimensionChangedAsync()
        {
            CalculateAvailableSize();
            await SetSvgDimensionAsync(Constants.SetSvgDimensions).ConfigureAwait(true);
            if (_seriesContainer is not null && !_seriesContainer.ContainerPrerender)
            {
                foreach (IChartElementRenderer renderer in _seriesContainer.Renderers.ToArray())
                {
                    if (renderer is ChartSeriesRenderer seriesRenderer)
                    {
                        if (seriesRenderer.XAxisRenderer is null || seriesRenderer.YAxisRenderer is null)
                        {
                            return;
                        }
                    }
                }
                InitiAxis();
                OnLayoutChange();
                if ((_tooltip.Enable || _crosshair.Enable || _markerExplode is not null) && _isScriptCalled)
                {
                    _seriesContainer.SetGlobalizationValues();
                    await InvokeVoidAsync(_chartJsModule!, _chartJsInProcessModule!, Constants.SetTooltipOptions, [_dataId, _tooltip.GetTooltipForScript(), GetTooltipOptions(), _seriesClipRects.ToArray(), _seriesMarkers.ToArray(), _seriesBorders.ToArray(), _axes.ToArray(), _seriesContainer._dateValuePairs, _seriesContainer._numberValuePairs]).ConfigureAwait(true);
                }
            }
        }

        /// <summary>
        /// Marks the chart title renderer for re-render and requests renderer updates.
        /// </summary>
        private void TitleChanged()
        {
            if (IsRendered && _chartTitleRenderer != null)
            {
                _chartTitleRenderer.RendererShouldRender = true;
                UpdateRenderers();
            }
        }

        /// <summary>
        /// Propagates a collection change notification to all series renderers.
        /// </summary>
        /// <param name="source">The collection source that raised the change event.</param>
        /// <param name="e">Details about the collection change.</param>
        private void DataCollectionChanged(object? source, NotifyCollectionChangedEventArgs e)
        {
            if (_seriesContainer is not null)
            {
                foreach (IChartElementRenderer renderer in _seriesContainer.Renderers.ToArray())
                {
                    if (renderer is ChartSeriesRenderer seriesRenderer)
                    {
                        seriesRenderer.Series?.DataCollectionChanged(source ?? null!, e);
                    }
                }
            }
        }

        /// <summary>
        /// Propagates a property change notification to all series renderers.
        /// </summary>
        /// <param name="source">The object that raised the property change event.</param>
        /// <param name="e">Details about the property that changed.</param>
        private void PropertyChanged(object? source, PropertyChangedEventArgs e)
        {
            if (_seriesContainer is not null)
            {
                foreach (IChartElementRenderer renderer in _seriesContainer.Renderers.ToArray())
                {
                    if (renderer is ChartSeriesRenderer seriesRenderer)
                    {
                        seriesRenderer.Series?.PropertyChanged(source ?? null!, e);
                    }
                }
            }
        }

        #endregion
    }
}
