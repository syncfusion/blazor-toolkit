using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Handles the events related to chart rendering and customization.
    /// </summary>
    public partial class SfChart
    {
        #region Properties

        /// <summary>
        /// An event that is raised before each axis label is rendered. 
        /// </summary>       
        /// <remarks>
        /// You can customize the axis label through <see cref="AxisLabelRenderEventArgs"/> event argument.
        /// This event is raised to customize the axis labels.
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to customize axis labels using the OnAxisLabelRender event.
        /// <SfChart OnAxisLabelRender="AxisLabelEvent">
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Sales" XName="Month" YName="SalesValue" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        ///
        /// @code {
        ///     public void AxisLabelEvent(AxisLabelRenderEventArgs args)
        ///     {
        ///         // Customize axis label text
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>       
        [Parameter]
        public Action<AxisLabelRenderEventArgs> OnAxisLabelRender { get; set; } = null!;

        /// <summary> 
        /// An event that is raised before each axis range is rendered. 
        /// </summary> 
        /// <remarks> 
        /// The <see cref="Action{T}"/> should accept a <see cref="AxisRangeCalculatedEventArgs"/> parameter, 
        /// providing information about the axis range calculation event. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to handle the AxisActualRangeCalculated event in the chart.
        /// <SfChart OnAxisActualRangeCalculated="AxisActualRangeEvent">
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"></ChartPrimaryXAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Sales" XName="Month" YName="SalesValue" Type="ChartSeriesType.Column">
        ///             <ChartMarker Visible="true">
        ///             </ChartMarker>
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        ///
        /// @code {
        ///     public void AxisActualRangeEvent(AxisRangeCalculatedEventArgs args)
        ///     {
        ///         // You can modify the actual axis range here.
        ///         // For example: args.Maximum = 100;
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public Action<AxisRangeCalculatedEventArgs> OnAxisActualRangeCalculated { get; set; } = null!;

        /// <summary> 
        /// An event that is raised before each data point for the series is rendered. 
        /// </summary> 
        /// <remarks> 
        /// The <see cref="Action{T}"/> should accept a <see cref="PointRenderEventArgs"/> parameter, 
        /// providing information about the rendering event for a data point in the series. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to use the PointRender event in a Syncfusion Blazor Chart.
        /// <SfChart OnPointRender="PointRenderEvent">
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"></ChartPrimaryXAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Sales" XName="Month" YName="SalesValue" Type="ChartSeriesType.Column">
        ///             <ChartMarker Visible="true">
        ///             </ChartMarker>
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        ///
        /// @code {
        ///     public void PointRenderEvent(PointRenderEventArgs args)
        ///     {
        ///         // Customize the appearance of each point.
        ///         // For example: args.Fill = "red";
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public Action<PointRenderEventArgs> OnPointRender { get; set; } = null!;

        /// <summary> 
        /// An event that is raised before the data label for a series is rendered. 
        /// </summary> 
        /// <remarks> 
        /// The <see cref="Action{T}"/> should accept a <see cref="TextRenderEventArgs"/> parameter, 
        /// providing information about the rendering event for a data label. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to use the DataLabelRender event in a Syncfusion Blazor Chart.
        /// <SfChart OnDataLabelRender="DataLabelEvent">
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"></ChartPrimaryXAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Sales" XName="Month" YName="SalesValue" Type="ChartSeriesType.Column">
        ///             <ChartMarker>
        ///                 <ChartDataLabel Visible="true"></ChartDataLabel>
        ///             </ChartMarker>
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        ///
        /// @code {
        ///     public void DataLabelEvent(TextRenderEventArgs args)
        ///     {
        ///         // Customize the data label text or style.
        ///         // Example: args.Text = "$" + args.Text;
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public Action<TextRenderEventArgs> OnDataLabelRender { get; set; } = null!;

        /// <summary> 
        /// An event that is raised when a mouse click event occurs on the chart. 
        /// </summary> 
        /// <remarks> 
        /// The <see cref="Action{T}"/> should accept a <see cref="ChartMouseEventArgs"/> parameter, 
        /// providing information about the mouse click event on the chart. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to use the ChartMouseClick event in a Syncfusion Blazor Chart.
        /// <SfChart ChartMouseClick="OnMouseEvent">
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"></ChartPrimaryXAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Sales" XName="Month" YName="SalesValue" Type="ChartSeriesType.Column">
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        ///     <ChartZoomSettings EnableSelectionZooming="true"></ChartZoomSettings>
        /// </SfChart>
        ///
        /// @code {
        ///     public void OnMouseEvent(ChartMouseEventArgs args)
        ///     {
        ///         // Handle chart mouse click event here.
        ///         // Example: Console.WriteLine($"Clicked at X: {args.X}, Y: {args.Y}");
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public EventCallback<ChartMouseEventArgs> ChartMouseClick { get; set; }

        /// <summary> 
        /// An event that is raised when a mouse down event occurs on the chart. 
        /// </summary> 
        /// <remarks> 
        /// The <see cref="Action{T}"/> should accept a <see cref="ChartMouseEventArgs"/> parameter, 
        /// providing information about the mouse down event on the chart. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to use the ChartMouseDown event in a Syncfusion Blazor Chart.
        /// <SfChart ChartMouseDown="OnMouseEvent">
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"></ChartPrimaryXAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Sales" XName="Month" YName="SalesValue" Type="ChartSeriesType.Column">
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        ///     <ChartZoomSettings EnableSelectionZooming="true"></ChartZoomSettings>
        /// </SfChart>
        ///
        /// @code {
        ///     public void OnMouseEvent(ChartMouseEventArgs args)
        ///     {
        ///         // Handle chart mouse down event here.
        ///         // Example: Console.WriteLine($"Mouse down at: X = {args.X}, Y = {args.Y}");
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public Action<ChartMouseEventArgs> ChartMouseDown { get; set; } = null!;

        /// <summary> 
        /// An event that is raised when a mouse up event occurs on the chart. 
        /// </summary> 
        /// <remarks> 
        /// The <see cref="Action{T}"/> should accept a <see cref="ChartMouseEventArgs"/> parameter, 
        /// providing information about the mouse up event on the chart. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to use the ChartMouseUp event in a Syncfusion Blazor Chart.
        /// <SfChart ChartMouseUp="OnMouseEvent">
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"></ChartPrimaryXAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Sales" XName="Month" YName="SalesValue" Type="ChartSeriesType.Column">
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        ///     <ChartZoomSettings EnableSelectionZooming="true"></ChartZoomSettings>
        /// </SfChart>
        ///
        /// @code {
        ///     public void OnMouseEvent(ChartMouseEventArgs args)
        ///     {
        ///         // Handle chart mouse up event here.
        ///         // Example: Console.WriteLine($"Mouse up at: X = {args.X}, Y = {args.Y}");
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public Action<ChartMouseEventArgs> ChartMouseUp { get; set; } = null!;

        /// <summary> 
        /// An event that is raised when a mouse move event occurs on the chart. 
        /// </summary> 
        /// <remarks> 
        /// The <see cref="Action{T}"/> should accept a <see cref="ChartMouseEventArgs"/> parameter, 
        /// providing information about the mouse move event on the chart. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to use the ChartMouseMove event in a Syncfusion Blazor Chart.
        /// <SfChart ChartMouseMove="OnMouseEvent">
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"></ChartPrimaryXAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Sales" XName="Month" YName="SalesValue" Type="ChartSeriesType.Column">
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        ///     <ChartZoomSettings EnableSelectionZooming="true"></ChartZoomSettings>
        /// </SfChart>
        ///
        /// @code {
        ///     public void OnMouseEvent(ChartMouseEventArgs args)
        ///     {
        ///         // Handle chart mouse move event here.
        ///         // Example: Console.WriteLine($"Mouse moved at: X = {args.X}, Y = {args.Y}");
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public Action<ChartMouseEventArgs> ChartMouseMove { get; set; } = null!;

        /// <summary> 
        /// An event that is raised before each series is rendered. 
        /// </summary> 
        /// <remarks> 
        /// The <see cref="Action{T}"/> should accept a <see cref="SeriesRenderEventArgs"/> parameter, 
        /// providing information about the rendering event for a series. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to use the OnSeriesRender event in a Syncfusion Blazor Chart.
        /// <SfChart OnSeriesRender="OnSeriesRenderEvent">
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"></ChartPrimaryXAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Sales" XName="Month" YName="SalesValue" Type="ChartSeriesType.Column">
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        ///     <ChartZoomSettings EnableSelectionZooming="true"></ChartZoomSettings>
        /// </SfChart>
        ///
        /// @code {
        ///     public void OnSeriesRenderEvent(SeriesRenderEventArgs args)
        ///     {
        ///         // Customize series appearance or behavior.
        ///         // Example: args.Series.Fill = "orange";
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public Action<SeriesRenderEventArgs> OnSeriesRender { get; set; } = null!;

        /// <summary> 
        /// An event that is raised before each legend item is rendered. 
        /// </summary> 
        /// <remarks> 
        /// The <see cref="Action{T}"/> should accept a <see cref="LegendRenderEventArgs"/> parameter, 
        /// providing information about the rendering event for a legend item. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to use the OnLegendItemRender event in a Syncfusion Blazor Chart.
        /// <SfChart OnLegendItemRender="LegendEvent">
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"></ChartPrimaryXAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Sales" Name="Column" XName="Month" YName="SalesValue" Type="ChartSeriesType.Column">
        ///         </ChartSeries>
        ///         <ChartSeries DataSource="@Sales" Name="Line" XName="Month" YName="SalesValue" Type="ChartSeriesType.Line">
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        ///
        /// @code {
        ///     public void LegendEvent(LegendRenderEventArgs args)
        ///     {
        ///         // Customize legend item appearance.
        ///         // Example: args.Text = "Modified " + args.Text;
        ///         // Example: args.Shape = LegendShape.Diamond;
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public Action<LegendRenderEventArgs> OnLegendItemRender { get; set; } = null!;

        /// <summary> 
        /// An event that is raised while rendering multi-level labels on the axis. 
        /// </summary> 
        /// <remarks> 
        /// The <see cref="Action{T}"/> should accept an <see cref="AxisMultiLabelRenderEventArgs"/> parameter, 
        /// providing information about the rendering event for multi-level labels on the axis. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to use the OnAxisMultiLevelLabelRender event in a Syncfusion Blazor Chart.
        /// <SfChart OnAxisMultiLevelLabelRender="AxisMultiLevelLabelEvent">
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category">
        ///         <ChartMultiLevelLabels>
        ///             <ChartMultiLevelLabel>
        ///                 <ChartCategories>
        ///                     <ChartCategory Start="0" End="3" Text="First_Half"></ChartCategory>
        ///                     <ChartCategory Start="3" End="6" Text="Second_Half"></ChartCategory>
        ///                 </ChartCategories>
        ///             </ChartMultiLevelLabel>
        ///         </ChartMultiLevelLabels>
        ///     </ChartPrimaryXAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Sales" XName="Month" YName="SalesValue" Type="ChartSeriesType.Column">
        ///             <ChartMarker Visible="true">
        ///             </ChartMarker>
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        ///
        /// @code {
        ///     public void AxisMultiLevelLabelEvent(AxisMultiLabelRenderEventArgs args)
        ///     {
        ///         // Example: Change label text
        ///         // args.Text = args.Text.Replace("_", " ");
        ///
        ///         // Example: Apply custom style
        ///         // args.TextStyle.Color = "blue";
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public Action<AxisMultiLabelRenderEventArgs> OnAxisMultiLevelLabelRender { get; set; } = null!;

        /// <summary> 
        /// An event that is raised when the scroll changes. 
        /// </summary> 
        /// <remarks> 
        /// The <see cref="Action{T}"/> should accept a <see cref="ScrollEventArgs"/> parameter, 
        /// providing information about the event when the scroll is changed. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to handle the OnScrollChanged event in a Syncfusion Blazor Chart.
        /// <SfChart OnScrollChanged="ScrollChangeEvent">
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" ZoomFactor="0.5" ZoomPosition="0.2">
        ///         <ChartAxisScrollbarSettings Enable="true"></ChartAxisScrollbarSettings>
        ///     </ChartPrimaryXAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Sales" XName="Month" YName="SalesValue" Type="ChartSeriesType.Column">
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        ///
        /// @code {
        ///     public void ScrollChangeEvent(ScrollEventArgs args)
        ///     {
        ///         // Example: Log the new zoom factor and position
        ///         // Console.WriteLine($"ZoomFactor: {args.CurrentZoomFactor}, ZoomPosition: {args.CurrentZoomPosition}");
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public Action<ScrollEventArgs> OnScrollChanged { get; set; } = null!;

        /// <summary> 
        /// An event that is raised during the zooming operation. 
        /// </summary> 
        /// <remarks> 
        /// The <see cref="Action{T}"/> should accept a <see cref="ZoomingEventArgs"/> parameter, 
        /// providing information about the event that occurs during the zooming operation. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to handle the OnZoomStart event in a Syncfusion Blazor Chart.
        /// <SfChart OnZoomStart="OnZoomingEvent">
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"></ChartPrimaryXAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Sales" XName="Month" YName="SalesValue" Type="ChartSeriesType.Column">
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        ///     <ChartZoomSettings EnableSelectionZooming="true"></ChartZoomSettings>
        /// </SfChart>
        ///
        /// @code {
        ///     public void OnZoomingEvent(ZoomingEventArgs args)
        ///     {
        ///         // Example: Cancel zooming if necessary
        ///         // args.Cancel = true;
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public Action<ZoomingEventArgs> OnZooming { get; set; } = null!;

        /// <summary> 
        /// An event that is raised at the start of the zooming operation. 
        /// </summary> 
        /// <remarks> 
        /// The <see cref="Action{T}"/> should accept a <see cref="ZoomingEventArgs"/> parameter, 
        /// providing information about the event at the beginning of the zooming operation. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to handle the OnZoomStart event in a Syncfusion Blazor Chart.
        /// <SfChart OnZoomStart="OnZoomingEvent">
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"></ChartPrimaryXAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Sales" XName="Month" YName="SalesValue" Type="ChartSeriesType.Column">
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        ///     <ChartZoomSettings EnableSelectionZooming="true"></ChartZoomSettings>
        /// </SfChart>
        /// 
        /// @code {
        ///     public void OnZoomingEvent(ZoomingEventArgs args)
        ///     {
        ///         // Example: Cancel zooming if necessary
        ///         // args.Cancel = true;
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public Action<ZoomingEventArgs> OnZoomStart { get; set; } = null!;

        /// <summary> 
        /// An event that is raised after the zooming operation is completed. 
        /// </summary> 
        /// <remarks> 
        /// The <see cref="Action{T}"/> should accept a <see cref="ZoomingEventArgs"/> parameter, 
        /// providing information about the event when the zooming operation is completed. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to handle the OnZoomEnd event in a Syncfusion Blazor Chart.
        /// <SfChart OnZoomEnd="OnZoomingEvent">
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"></ChartPrimaryXAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Sales" XName="Month" YName="SalesValue" Type="ChartSeriesType.Column">
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        ///     <ChartZoomSettings EnableSelectionZooming="true"></ChartZoomSettings>
        /// </SfChart>
        /// 
        /// @code {
        ///     public void OnZoomingEvent(ZoomingEventArgs args)
        ///     {
        ///         // Example: Implement custom logic when zooming ends
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public Action<ZoomingEventArgs> OnZoomEnd { get; set; } = null!;

        /// <summary> 
        /// An event that is raised when a legend item is clicked. 
        /// </summary> 
        /// <remarks> 
        /// The <see cref="Action{T}"/> should accept a <see cref="LegendClickEventArgs"/> parameter, 
        /// providing information about the click event on the legend item. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to handle the OnLegendClick event in a Syncfusion Blazor Chart.
        /// <SfChart OnLegendClick="LegendClickEvent">
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"></ChartPrimaryXAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Sales" Name="Column" XName="Month" YName="SalesValue" Type="ChartSeriesType.Column">
        ///         </ChartSeries>
        ///         <ChartSeries DataSource="@Sales" Name="Line" XName="Month" YName="SalesValue" Type="ChartSeriesType.Line">
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// 
        /// @code {
        ///     public void LegendClickEvent(LegendClickEventArgs args)
        ///     {
        ///         // Example: Implement custom logic when a legend item is clicked
        ///         // args.Series is the clicked legend's associated chart series.
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public EventCallback<LegendClickEventArgs> OnLegendClick { get; set; }

        /// <summary> 
        /// An event that is raised when the crosshair is moved. 
        /// </summary> 
        /// <remarks> 
        /// The <see cref="Action{T}"/> should accept a <see cref="CrosshairMoveEventArgs"/> parameter, 
        /// providing information about the event when the crosshair moves along the axis. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to handle the OnCrosshairMove event in a Syncfusion Blazor Chart.
        /// <SfChart OnCrosshairMove="OnCrosshairMove">
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.DateTime">
        ///         <ChartAxisCrosshairTooltip Enable="true" Fill="red">
        ///             <ChartCrosshairTextStyle Size="14px" Color="white"> </ChartCrosshairTextStyle>
        ///         </ChartAxisCrosshairTooltip>
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis>
        ///         <ChartAxisCrosshairTooltip Enable="true" Fill="red">
        ///             <ChartCrosshairTextStyle Size="14px" Color="white"> </ChartCrosshairTextStyle>
        ///         </ChartAxisCrosshairTooltip>
        ///     </ChartPrimaryYAxis>
        ///     <ChartCrosshairSettings Enable="true">
        ///         <ChartCrosshairLine Width="2" Color="green"></ChartCrosshairLine>
        ///     </ChartCrosshairSettings>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@SalesDetails" XName="X" YName="Y" Type="ChartSeriesType.Line">
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// 
        /// @code {
        ///     public void OnCrosshairMove(CrosshairMoveEventArgs args)
        ///     {
        ///         // Example: Implement custom logic when the crosshair moves.
        ///         // args is the event argument containing information about the crosshair's position.
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example> 
        [Parameter]
        public Action<CrosshairMoveEventArgs> OnCrosshairMove { get; set; } = null!;

        /// <summary> 
        /// An event that is raised when the point drag ends during data editing. 
        /// </summary> 
        /// <remarks> 
        /// The <see cref="Action{T}"/> should accept a <see cref="DataEditingEventArgs"/> parameter, 
        /// providing information about the event when the user completes dragging a data point. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to handle the OnDataEditCompleted event in a Syncfusion Blazor Chart.
        /// <SfChart OnDataEditCompleted="OnDataEditCompleted">
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.DateTime" />
        ///     <ChartPrimaryYAxis LabelFormat="{value}%" RangePadding="ChartRangePadding.None" Minimum="0" Maximum="100" Interval="20" />
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@ConsumerDetails" XName="XValue" Width="2"
        ///                      Opacity="1" YName="YValue" Type="ChartSeriesType.Column">
        ///             <ChartMarker Visible="true" Width="10" Height="10">
        ///             </ChartMarker>
        ///             <ChartDataEditSettings Enable="true"></ChartDataEditSettings>
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// 
        /// @code {
        ///     public void OnDataEditCompleted(DataEditingEventArgs args)
        ///     {
        ///         // Example: Implement custom logic when data editing is completed.
        ///         // args contains information about the edited data.
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public Action<DataEditingEventArgs> OnDataEditCompleted { get; set; } = null!;

        /// <summary> 
        /// An event that is raised when the point drag starts during data editing. 
        /// </summary> 
        /// <remarks> 
        /// The <see cref="Action{T}"/> should accept a <see cref="DataEditingEventArgs"/> parameter, 
        /// providing information about the event when the user starts dragging a data point. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to handle the OnDataEditCompleted event in a Syncfusion Blazor Chart.
        /// <SfChart OnDataEdit="OnDataEdit">
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.DateTime" />
        ///     <ChartPrimaryYAxis LabelFormat="{value}%" RangePadding="ChartRangePadding.None" Minimum="0" Maximum="100" Interval="20" />
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@ConsumerDetails" XName="XValue" Width="2"
        ///                      Opacity="1" YName="YValue" Type="ChartSeriesType.Column">
        ///             <ChartMarker Visible="true" Width="10" Height="10">
        ///             </ChartMarker>
        ///             <ChartDataEditSettings Enable="true"></ChartDataEditSettings>
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// 
        /// @code {
        ///     public void OnDataEdit(DataEditingEventArgs args)
        ///     {
        ///         // Example: Implement custom logic when data editing is completed.
        ///         // args contains information about the edited data.
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public Action<DataEditingEventArgs> OnDataEdit { get; set; } = null!;

        /// <summary> 
        /// An event that is raised after the selection is completed. 
        /// </summary> 
        /// <remarks> 
        /// The <see cref="Action{T}"/> should accept a <see cref="SelectionCompleteEventArgs"/> parameter, 
        /// providing information about the event when the selection operation is completed. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to handle the OnSelectionChanged event in a Syncfusion Blazor Chart.
        /// <SfChart Title="Olympic Medals" SelectionMode="SelectionMode.Point" OnSelectionChanged="OnSelectionChanged">
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category">
        ///     </ChartPrimaryXAxis>
        ///     <ChartLegendSettings Visible="true" ToggleVisibility="true">
        ///     </ChartLegendSettings>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@MedalDetails" XName="Country" YName="Gold"
        ///                      Type="ChartSeriesType.Column" SelectionStyle="chartSelection1" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// 
        /// <style>
        ///     .chartSelection1 {
        ///         fill: red;
        ///     }
        /// </style>
        /// 
        /// @code {
        ///     public void OnSelectionChanged(SelectionCompleteEventArgs args)
        ///     {
        ///         // Example: Implement custom logic when a selection changes.
        ///         // args contains information about the selected data points.
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public Action<SelectionCompleteEventArgs> OnSelectionChanged { get; set; } = null!;

        /// <summary> 
        /// An event that is raised after the chart has been resized. 
        /// </summary> 
        /// <remarks> 
        /// The <see cref="Action{T}"/> should accept a <see cref="ResizeEventArgs"/> parameter, 
        /// providing information about the event that occurs after the chart is resized. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to handle the SizeChanged event in a Syncfusion Blazor Chart.
        /// <SfChart SizeChanged="@SizeChangedEvent">
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"></ChartPrimaryXAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Sales" XName="Month" YName="SalesValue" Type="ChartSeriesType.Column">
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        ///
        /// @code {
        ///     public void SizeChangedEvent(ResizeEventArgs args)
        ///     {
        ///         // Here, you can customize your code.
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public Action<ResizeEventArgs> SizeChanged { get; set; } = null!;

        /// <summary> 
        /// An event that is raised before the tooltip for a series is rendered. 
        /// </summary> 
        /// <remarks> 
        /// The <see cref="Action{T}"/> should accept a <see cref="TooltipRenderEventArgs"/> parameter, 
        /// providing information about the rendering event for the tooltip of a series. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to handle the TooltipRender event in a Syncfusion Blazor Chart.
        /// <SfChart TooltipRender="TooltipEvent">
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Sales" XName="Month" YName="SalesValue" Type="ChartSeriesType.Column">
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        ///     <ChartTooltipSettings Enable="true"></ChartTooltipSettings>
        /// </SfChart>
        ///
        /// @code {
        ///     public void TooltipEvent(TooltipRenderEventArgs args){ args.TextStyle.Color = "red";}
        /// }
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public Action<TooltipRenderEventArgs> TooltipRender { get; set; } = null!;

        /// <summary> 
        /// An event that is raised before the tooltip for a series is rendered. 
        /// </summary> 
        /// <remarks> 
        /// The <see cref="Action{T}"/> should accept a <see cref="SharedTooltipRenderEventArgs"/> parameter, 
        /// providing information about the rendering event for the shared tooltip of a series. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to handle the SharedTooltipRender event in a Syncfusion Blazor Chart.
        /// <SfChart SharedTooltipRender="SharedTooltipEvent">
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"></ChartPrimaryXAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Sales" XName="Month" YName="SalesValue" Type="ChartSeriesType.Column">
        ///         </ChartSeries>
        ///         <ChartSeries DataSource="@Sales" XName="Month" YName="SalesValue" Type="ChartSeriesType.Column">
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        ///     <ChartTooltipSettings Enable="true" Shared="true"></ChartTooltipSettings>
        /// </SfChart>
        ///
        /// @code {
        ///     public void SharedTooltipEvent(SharedTooltipRenderEventArgs args)
        ///     {
        ///         // Here, you can customize your tooltip.
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public Action<SharedTooltipRenderEventArgs> SharedTooltipRender { get; set; } = null!;

        /// <summary> 
        /// An event that is raised on a point click event. 
        /// </summary> 
        /// <remarks> 
        /// The <see cref="Action{T}"/> should accept a <see cref="PointEventArgs"/> parameter, 
        /// providing information about the click event on a data point. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to handle the OnPointClick event in a Syncfusion Blazor Chart.
        /// <SfChart OnPointClick="PointClickEvent">
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"></ChartPrimaryXAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Sales" XName="Month" YName="SalesValue" Type="ChartSeriesType.Column">
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        ///
        /// @code {
        ///     public void PointClickEvent(PointEventArgs args)
        ///     {
        ///         // Here, you can customize your response to point click.
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public EventCallback<PointEventArgs> OnPointClick { get; set; }

        /// <summary> 
        /// An event that is raised after a click on a multi-level label. 
        /// </summary>
        /// <remarks> 
        /// The <see cref="Action{T}"/> should accept a <see cref="MultiLevelLabelClickEventArgs"/> parameter, 
        /// providing information about the click event on a multi-level label. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to handle the OnMultiLevelLabelClick event in a Syncfusion Blazor Chart.
        /// <SfChart OnMultiLevelLabelClick="MultiLabelClickEvent">
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category">
        ///         <ChartMultiLevelLabels>
        ///             <ChartMultiLevelLabel>
        ///                 <ChartCategories>
        ///                     <ChartCategory Start="0" End="3" Text="First_Half"></ChartCategory>
        ///                     <ChartCategory Start="3" End="6" Text="Second_Half"></ChartCategory>
        ///                 </ChartCategories>
        ///             </ChartMultiLevelLabel>
        ///         </ChartMultiLevelLabels>
        ///     </ChartPrimaryXAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Sales" XName="Month" YName="SalesValue" Type="ChartSeriesType.Column">
        ///             <ChartMarker Visible="true"></ChartMarker>
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        ///
        /// @code {
        ///     public void MultiLabelClickEvent(MultiLevelLabelClickEventArgs args)
        ///     {
        ///         // Example: Get the clicked label text
        ///         // var labelText = args.Text;
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public Action<MultiLevelLabelClickEventArgs> OnMultiLevelLabelClick { get; set; } = null!;

        /// <summary> 
        /// An event that is raised when the chart rendering is completed. 
        /// </summary> 
        /// <remarks> 
        /// The <see cref="Action{T}"/> should accept a <see cref="LoadedEventArgs"/> parameter, 
        /// providing information about the completion of the chart rendering. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to handle the Loaded event in a Syncfusion Blazor Chart.
        /// <SfChart Loaded="LoadedEvent">
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"></ChartPrimaryXAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Sales" XName="Month" YName="SalesValue" Type="ChartSeriesType.Column">
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        ///
        /// @code {
        ///     public void LoadedEvent(LoadedEventArgs args)
        ///     {
        ///         // Example: Perform an action after the chart is fully loaded
        ///         // Console.WriteLine("Chart loaded.");
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public Action<LoadedEventArgs> Loaded { get; set; } = null!;

        /// <summary> 
        /// An event that is raised when any chart axis label is clicked. 
        /// </summary> 
        /// <remarks> 
        /// The <see cref="Action{T}"/> should accept an <see cref="AxisLabelClickEventArgs"/> parameter, 
        /// providing information about the click event on a chart axis label. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to handle the AxisLabelClick event in a Syncfusion Blazor Chart.
        /// <SfChart OnAxisLabelClick="AxisLabelClickEvent">
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"></ChartPrimaryXAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Sales" XName="Month" YName="SalesValue" Type="ChartSeriesType.Column">
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        ///
        /// @code {
        ///     public void AxisLabelClickEvent(AxisLabelClickEventArgs args)
        ///     {
        ///         // Example: Perform an action when an axis label is clicked
        ///         // Console.WriteLine($"Axis label clicked: {args.AxisLabel}");
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public Action<AxisLabelClickEventArgs> OnAxisLabelClick { get; set; } = null!;

        #endregion
    }
}
