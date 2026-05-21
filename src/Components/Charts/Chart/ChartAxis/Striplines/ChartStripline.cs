using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Charts.Internal;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides options to customize the striplines of the axis.
    /// </summary>
    /// <remarks>
    /// This class provides various properties to define the appearance and behavior of striplines in a chart axis, such as color, size, and text alignment.
    /// </remarks>
    public class ChartStripline : ChartSubComponent, IChartElement
    {
        #region Fields
        private bool _isPropertyChanged;
        private bool _isUpdateDirection;
        private string _color = "#808080";
        private object? _start;
        private object? _end;
        private string? _dashArray;
        private double _opacity = 1;
        private double _size;
        private string _text = string.Empty;
        private bool _visible = true;
        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the cascading parent striplines collection.
        /// </summary>
        [CascadingParameter]
        internal ChartStriplines? Parent { get; set; }

        /// <summary>
        /// Gets or sets the tooltip associated with the stripline.
        /// </summary>
        internal ChartStriplineTooltip? StriplineTooltip { get; set; }

        /// <summary>
        /// Gets or sets the renderer responsible for rendering the stripline.
        /// </summary>
        internal ChartStriplineRenderer? Renderer { get; set; }
        #endregion

        #region Properties

        /// <summary> 
        /// Gets or sets an instance of <see cref="ChartStriplineBorder"/> that defines the border of the strip line. 
        /// </summary> 
        /// <value> 
        /// The default value is a instance of <see cref="ChartStriplineBorder"/>.
        /// </value> 
        /// <remarks> 
        /// This property can be used to customize the color and width of the stripline border. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to apply a custom border style to a stripline on the Y-axis.
        /// <SfChart>
        ///     <ChartPrimaryYAxis>
        ///         <ChartStriplines>
        ///             <ChartStripline Start="20" End="25" Color="red">
        ///                 <ChartStriplineBorder Width="2" Color="blue"></ChartStriplineBorder>
        ///             </ChartStripline>
        ///         </ChartStriplines>
        ///     </ChartPrimaryYAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ChartStriplineBorder Border { get; set; } = new ChartStriplineBorder();

        /// <summary> 
        /// Gets or sets the color of the strip line. 
        /// </summary> 
        /// <value> 
        /// A string representing the color of the strip line. The default value is <c>"#808080"</c>. 
        /// </value> 
        /// <remarks> 
        /// Use valid hex or rgba CSS color strings for the color value. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set a color for a stripline on the Y-axis.
        /// <SfChart>
        ///     <ChartPrimaryYAxis>
        ///         <ChartStriplines>
        ///             <ChartStripline Start="20" End="25" Color="red">
        ///             </ChartStripline>
        ///         </ChartStriplines>
        ///     </ChartPrimaryYAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example> 
        [Parameter]
        public string Color { get; set; } = "#808080";

        /// <summary> 
        /// Gets or sets the dash array of the strip line. 
        /// </summary> 
        /// <value> 
        /// A string representing the dash array of the strip line. The default value is <c>null</c>. 
        /// </value>
        /// <remarks> 
        /// The default value is <c>null</c>, indicating no specific dash array is set by default. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to apply a dashed pattern to a stripline using the DashArray property.
        /// <SfChart>
        ///     <ChartPrimaryYAxis>
        ///         <ChartStriplines>
        ///             <ChartStripline Start="20" End="25" DashArray="10,1">
        ///             </ChartStripline>
        ///         </ChartStriplines>
        ///     </ChartPrimaryYAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string? DashArray { get; set; }

        /// <summary> 
        /// Gets or sets the end value of the strip line. 
        /// </summary> 
        /// <value> 
        /// An object representing the end value of the strip line. The default value is <c>null</c>. 
        /// </value> 
        /// <remarks> 
        /// This property type is an object; based on the axis type, the value will be assigned. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the end position of a stripline using the End property.
        /// <SfChart>
        ///     <ChartPrimaryYAxis>
        ///         <ChartStriplines>
        ///             <ChartStripline Start="20" End="25">
        ///             </ChartStripline>
        ///         </ChartStriplines>
        ///     </ChartPrimaryYAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public object? End { get; set; }

        /// <summary> 
        /// Defines the position of the strip line text horizontally. 
        /// </summary> 
        /// <value> 
        /// One of the <see cref="Anchor"/> enumerations that specifies the strip line text position. 
        /// The options include: 
        /// - <c>Start</c>: Places the text at the start of the strip line. 
        /// - <c>Middle</c>: Places the text at the middle of the strip line. 
        /// - <c>End</c>: Places the text at the end of the strip line.
        /// <br/>
        /// The default value is <b>Anchor.Middle</b>. 
        /// </value> 
        /// <remarks> 
        /// This property determines where the strip line text will be horizontally positioned relative to the strip line. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to align the stripline text at the start of the Y-axis using the HorizontalAlignment property.
        /// <SfChart>
        ///     <ChartPrimaryYAxis>
        ///         <ChartStriplines>
        ///             <ChartStripline Start="20" End="25" HorizontalAlignment="Anchor.Start" Text="Yaxis">
        ///             </ChartStripline>
        ///         </ChartStriplines>
        ///     </ChartPrimaryYAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public Anchor HorizontalAlignment { get; set; } = Anchor.Middle;

        /// <summary> 
        /// Gets or sets a value that specifies whether the strip line should be repeated. 
        /// </summary> 
        /// <value> 
        /// <b>true</b> if the strip line should be repeated; otherwise, <b>false</b>. The default value is <b>false</b>.
        /// </value>
        /// <remarks>
        /// When set to <b>true</b>, the strip line will be drawn repeatedly across the axis.
        /// If set to <b>false</b>, only a single strip line will be displayed.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example shows how to repeat the stripline along the Y-axis using the IsRepeat property.
        /// <SfChart>
        ///     <ChartPrimaryYAxis>
        ///         <ChartStriplines>
        ///             <ChartStripline Start="20" End="25" IsRepeat="true" >
        ///             </ChartStripline>
        ///         </ChartStriplines>
        ///     </ChartPrimaryYAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool IsRepeat { get; set; }

        /// <summary> 
        /// Gets or sets a value indicating whether the strip line is segmented. 
        /// </summary> 
        /// <value> 
        /// <b>true</b> if the strip line is segmented; otherwise, <b>false</b>. The default value is <b>false</b>. 
        /// </value> 
        /// <remarks> 
        /// If set to true, stripline can be created in a specific region as a segment. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to create a segmented stripline along the X-axis.
        /// <SfChart>
        ///     <ChartPrimaryXAxis IntervalType="IntervalType.Minutes" ValueType="Syncfusion.Blazor.Toolkit.ValueType.DateTime">
        ///         <ChartStriplines>
        ///             <ChartStripline Start="new DateTime(2016, 06, 13, 08, 00, 00)" End="new DateTime(2016, 06, 13, 08, 05, 00)" Color="#E0E0E0" IsSegmented="true" SegmentStart="1.8" SegmentAxisName="PrimaryYAxis" SegmentEnd="2.2" StartFromAxis="false" />
        ///         </ChartStriplines>
        ///     </ChartPrimaryXAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@StepLineData" XName="X" YName="Y" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool IsSegmented { get; set; }

        /// <summary> 
        /// Gets or sets the opacity of the strip line. 
        /// </summary> 
        /// <value> 
        /// The double representing the opacity of the strip line. The default value is <b>1</b>.
        /// </value>
        /// <remarks> It accepts values from 0 to 1. </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the opacity of the stripline along the Y-axis.
        /// <SfChart>
        ///     <ChartPrimaryYAxis>
        ///         <ChartStriplines>
        ///             <ChartStripline Start="20" End="25" Opacity="0.6">
        ///             </ChartStripline>
        ///         </ChartStriplines>
        ///     </ChartPrimaryYAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double Opacity { get; set; } = 1;

        /// <summary> 
        /// Gets or sets a value that specifies the interval based on the axis value for repeating stripline. 
        /// </summary> 
        /// <value> 
        /// An object representing the interval for repeating the stripline based on the axis value.  The default value is <b>null</b>.
        /// </value> 
        /// <remarks> 
        /// Only applicable when <see cref="IsRepeat"/> is true and <see cref="SizeType"/> is not set to SizeType.Pixel. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to render a chart with a primary X-axis that includes a stripline with a specified size type and repeat interval.
        /// <SfChart>
        ///     <ChartPrimaryXAxis IntervalType="IntervalType.Minutes" ValueType="Syncfusion.Blazor.Toolkit.ValueType.DateTime">
        ///         <ChartStriplines>
        ///             <ChartStripline Start="new DateTime(2016, 06, 13, 08, 00, 00)" End="new DateTime(2016, 06, 13, 08, 05, 00)" Color="#E0E0E0" IsSegmented="true" SegmentStart="1.8" SegmentAxisName="PrimaryYAxis" SegmentEnd="2.2" StartFromAxis="false" SizeType="SizeType.Minutes" RepeatEvery="10" />
        ///         </ChartStriplines>
        ///     </ChartPrimaryXAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@StepLineData" XName="X" YName="Y" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public object RepeatEvery { get; set; } = null!;

        /// <summary> 
        /// Gets or sets a value that specifies the limit based on the axis value for repeating stripline. 
        /// </summary> 
        /// <value> 
        /// An object representing the limit for repeating the stripline based on the axis value. The default value is <b>null</b>.
        /// </value> 
        /// <remarks> 
        /// Only applicable when <see cref="IsRepeat"/> is true, <see cref="SizeType"/> is not set to SizeType.Pixel, and <see cref="RepeatEvery"/> is not null. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to render a chart with a primary X-axis that includes a stripline with repeat settings and a repeat until limit.
        /// <SfChart>
        ///     <ChartPrimaryXAxis IntervalType="IntervalType.Minutes" ValueType="Syncfusion.Blazor.Toolkit.ValueType.DateTime">
        ///         <ChartStriplines>
        ///             <ChartStripline Start="new DateTime(2016, 06, 13, 08, 00, 00)" End="new DateTime(2016, 06, 13, 08, 05, 00)" Color="#E0E0E0" IsSegmented="true" SegmentStart="1.8" SegmentAxisName="PrimaryYAxis" SegmentEnd="2.2" StartFromAxis="false" SizeType="SizeType.Minutes" RepeatEvery="10" IsRepeat="true" RepeatUntil="100" />
        ///         </ChartStriplines>
        ///     </ChartPrimaryXAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@StepLineData" XName="X" YName="Y" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public object RepeatUntil { get; set; } = null!;

        /// <summary> 
        /// Gets or sets the angle to which the strip line text gets rotated. 
        /// </summary> 
        /// <value> 
        /// The rotation angle for the strip line text. 
        /// The default value is <c>double.NaN</c>. 
        /// </value> 
        /// <remarks>
        /// This property allows customization of the text orientation on a strip line by specifying the rotation angle in degrees.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set a stripline on the Y-axis with a rotation of 45 degrees and a custom text "StripLine".
        /// <SfChart>
        ///     <ChartPrimaryYAxis>
        ///         <ChartStriplines>
        ///             <ChartStripline Start="20" End="25" Text="StripLine" Rotation="45">
        ///             </ChartStripline>
        ///         </ChartStriplines>
        ///     </ChartPrimaryYAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double Rotation { get; set; } = double.NaN;

        /// <summary> 
        /// Gets or sets the name of the axis associated with the segment of the strip line. 
        /// </summary> 
        /// <value> 
        /// A string representing the name of the axis which the strip line segment is associated. The default value is <c>null</c>.
        /// </value> 
        /// <remarks> 
        /// This property ensures proper alignment and positioning of the strip line with respect to the specified axis in the chart. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // Demonstrates the use of the SegmentAxisName property to render a stripline within a specified axis segment.
        /// <SfChart>
        ///     <ChartPrimaryXAxis LabelFormat="yyyy-MM-dd HH:mm:ss tt"
        ///                        Interval="5"
        ///                        IntervalType="IntervalType.Minutes"
        ///                        ValueType="Syncfusion.Blazor.Toolkit.ValueType.DateTime">
        ///         <ChartStriplines>
        ///             <ChartStripline Start="new DateTime(2016, 06, 13, 08, 00, 00)"
        ///                             End="new DateTime(2016, 06, 13, 08, 05, 00)"
        ///                             SegmentAxisName="PrimaryYAxis"
        ///                             SegmentStart="1.8"
        ///                             SegmentEnd="2.2" />
        ///         </ChartStriplines>
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis Interval="0.5" Minimum="0" Maximum="3.5" />
        ///     <ChartSeriesCollection>
        ///         <ChartSeries Fill="blue"
        ///                      DataSource="@StepLineData"
        ///                      Width="2"
        ///                      XName="X"
        ///                      YName="Y"
        ///                      Type="ChartSeriesType.StepLine" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string SegmentAxisName { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets the end value of the strip line segment. 
        /// </summary> 
        /// <value> 
        /// The end value of the strip line segment. The default value is <b>null</b>.
        /// </value> 
        /// <remarks> 
        /// This property determines where the segment concludes, providing control over the length and extent of the strip line. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to define a stripline with specific segment boundaries using SegmentStart and SegmentEnd.
        /// <SfChart>
        ///     <ChartPrimaryXAxis LabelFormat="yyyy-MM-dd HH:mm:ss tt"
        ///                        Interval="5"
        ///                        IntervalType="IntervalType.Minutes"
        ///                        ValueType="Syncfusion.Blazor.Toolkit.ValueType.DateTime">
        ///         <ChartStriplines>
        ///             <ChartStripline Start="new DateTime(2016, 06, 13, 08, 00, 00)"
        ///                             End="new DateTime(2016, 06, 13, 08, 05, 00)"
        ///                             Color="#E0E0E0"
        ///                             SegmentStart="1.8"
        ///                             SegmentEnd="2.2" />
        ///         </ChartStriplines>
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis Interval="0.5" Minimum="0" Maximum="3.5"></ChartPrimaryYAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries Fill="blue"
        ///                      DataSource="@StepLineData"
        ///                      Width="2"
        ///                      XName="X"
        ///                      YName="Y"
        ///                      Type="ChartSeriesType.StepLine" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public object SegmentEnd { get; set; } = null!;

        /// <summary> 
        /// Gets or sets the start value of the strip line segment. 
        /// </summary> 
        /// <value> 
        /// The start value of the strip line segment. The default value is <b>null</b>.
        /// </value> 
        /// <remarks> 
        /// This property determines where the segment begins, providing control over the length and extent of the strip line. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to define a stripline with specific segment boundaries using SegmentStart and SegmentEnd.
        /// // The stripline spans from 08:00 to 08:05 on the X-axis and between 1.8 and 2.2 on the Y-axis, creating a rectangular band.
        /// <SfChart>
        ///     <ChartPrimaryXAxis LabelFormat="yyyy-MM-dd HH:mm:ss tt" Interval="5" IntervalType="IntervalType.Minutes" ValueType="Syncfusion.Blazor.Toolkit.ValueType.DateTime">
        ///         <ChartStriplines>
        ///             <ChartStripline Start="new DateTime(2016, 06, 13, 08, 00, 00)" End="new DateTime(2016, 06, 13, 08, 05, 00)" SegmentStart="1.8" SegmentEnd="2.2" />
        ///         </ChartStriplines>
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis Interval="0.5" Minimum="0" Maximum="3.5"></ChartPrimaryYAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries Fill="blue" DataSource="@StepLineData" Width="2" XName="X" YName="Y" Type="ChartSeriesType.StepLine">
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public object SegmentStart { get; set; } = null!;

        /// <summary> 
        /// Gets or sets the size of the strip line based on the <see cref="Start"/> value and the axis value type. 
        /// </summary> 
        /// <value> 
        /// The size of the strip line. This property determines the extent or length of the strip line on the chart. The default value is <b>0</b>.
        /// </value> 
        /// <remarks> 
        /// For example, if <see cref="StartFromAxis"/> is set to true, and the Size is 4 on a Category axis, the strip line will be rendered over 4 category values from the origin. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to use the StartFromAxis and Size properties of a stripline to span a fixed size from the axis.
        /// <SfChart>
        ///     <ChartPrimaryYAxis>
        ///         <ChartStriplines>
        ///             <ChartStripline StartFromAxis="true" Size="30">
        ///             </ChartStripline>
        ///         </ChartStriplines>
        ///     </ChartPrimaryYAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double Size { get; set; }

        /// <summary> 
        /// Gets or sets the size type of the strip line. 
        /// </summary> 
        /// <value> 
        /// One of the <see cref="SizeType"/> enumerations that specifies the unit of strip line size. 
        /// The options include: 
        /// - <c>Auto</c> - Automatically determines the appropriate unit based on the data.
        /// - <c>Pixel</c> - Measures the size in pixels.
        /// - <c>Years</c> - Measures the size in years for date-time axis.
        /// - <c>Months</c> - Measures the size in months for date-time axis.
        /// - <c>Days</c> - Measures the size in days for date-time axis.
        /// - <c>Hours</c> - Measures the size in hours for date-time axis.
        /// - <c>Minutes</c> - Measures the size in minutes for date-time axis.
        /// - <c>Seconds</c> - Measures the size in seconds for date-time axis.
        /// <br/>
        /// The default value is <b>SizeType.Auto</b>. 
        /// </value> 
        /// <remarks>
        /// This property allows for flexible sizing of strip lines in both time-based and pixel-based units.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to render a chart with a primary X-axis that includes a stripline with a specified size type.
        /// <SfChart>
        ///     <ChartPrimaryXAxis IntervalType="IntervalType.Minutes" ValueType="Syncfusion.Blazor.Toolkit.ValueType.DateTime">
        ///         <ChartStriplines>
        ///             <ChartStripline Start="new DateTime(2016, 06, 13, 08, 00, 00)" End="new DateTime(2016, 06, 13, 08, 05, 00)" Color="#E0E0E0" IsSegmented="true" SegmentStart="1.8" SegmentAxisName="PrimaryYAxis" SegmentEnd="2.2" StartFromAxis="false" SizeType="SizeType.Minutes" />
        ///         </ChartStriplines>
        ///     </ChartPrimaryXAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@StepLineData" XName="X" YName="Y" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public SizeType SizeType { get; set; }

        /// <summary> 
        /// Gets or sets the start value of the strip line. 
        /// </summary> 
        /// <value> 
        /// An object representing the start value of the strip line. The default value is <b>null</b>.
        /// </value> 
        /// <remarks>
        /// This property determines where the strip line begins along the axis. Based on the axis type, the value will be assigned. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set a start value for a stripline on the primary Y-axis.
        /// <SfChart>
        ///     <ChartPrimaryYAxis>
        ///         <ChartStriplines>
        ///             <ChartStripline Start="20" End="25" />
        ///         </ChartStriplines>
        ///     </ChartPrimaryYAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public object? Start { get; set; }

        /// <summary> 
        /// Gets or sets a value indicating whether the strip line should be rendered from the axis origin. 
        /// </summary> 
        /// <value> 
        /// <b>true</b> if the strip line should be rendered from the axis origin; otherwise, <b>false</b>. 
        /// The default value is <b>false</b>. 
        /// </value> 
        /// <remarks> 
        /// This property allows you to control whether the strip line starts from the axis origin or from a specified value. 
        /// If set to true, the stripline will be rendered from the axis origin, ignoring the <see cref="Start"/> value if provided. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to render a stripline on the Y-axis starting directly from the axis line.
        /// <SfChart>
        ///     <ChartPrimaryYAxis>
        ///         <ChartStriplines>
        ///             <ChartStripline Start="18" End="25" StartFromAxis="true">
        ///             </ChartStripline>
        ///         </ChartStriplines>
        ///     </ChartPrimaryYAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool StartFromAxis { get; set; }

        /// <summary> 
        /// Gets or sets the text for the strip line. 
        /// </summary> 
        /// <value> 
        /// A string representing the strip line text. The default value is an empty string. 
        /// </value> 
        /// <remarks>
        /// This property is used to assign a label or descriptive text to the strip line, which can enhance the interpretability of charts by providing contextual information directly on the visual elements.
        /// If the text is not set, the strip line will appear without a label, maintaining a cleaner visual appearance but potentially lacking descriptive context.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to display a stripline with a text label on the Y-axis.
        /// <SfChart>
        ///     <ChartPrimaryYAxis>
        ///         <ChartStriplines>
        ///             <ChartStripline Start="18" End="25" Text="Strip-line">
        ///             </ChartStripline>
        ///         </ChartStriplines>
        ///     </ChartPrimaryYAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string Text { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets an instance of <see cref="ChartStriplineTextStyle"/> which defines the stripline text style. 
        /// </summary> 
        /// <value> 
        /// The default value is an instance of <see cref="ChartStriplineTextStyle"/>. 
        /// </value> 
        /// <remarks> 
        /// Use this property to define specific styling attributes such as color and font-properties for the text displayed within the strip line. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to customize the text style of a stripline label.
        /// <SfChart>
        ///     <ChartPrimaryYAxis>
        ///         <ChartStriplines>
        ///             <ChartStripline Start="18" End="25" Text="Strip-Line">
        ///                 <ChartStriplineTextStyle Size="16px"></ChartStriplineTextStyle>
        ///             </ChartStripline>
        ///         </ChartStriplines>
        ///     </ChartPrimaryYAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ChartStriplineTextStyle TextStyle { get; set; } = new ChartStriplineTextStyle();

        /// <summary> 
        /// Gets or sets a value that specifies the position of the strip line text vertically. 
        /// </summary> 
        /// <value> 
        /// One of the <see cref="Anchor"/> enumerations that specifies the position of the strip line text vertically. 
        /// The options include: 
        ///  - <c>Start</c>: Places the text at the start of the strip line. 
        ///  - <c>Middle</c>: Places the text at the middle of the strip line. 
        ///  - <c>End</c>: Places the text at the end of the strip line. 
        /// <br/>
        /// The default value is <b>Anchor.Middle</b> 
        /// </value> 
        /// <remarks> 
        /// This property determines where the strip line text will be vertically positioned relative to the strip line. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to align the stripline text at the start of the Y-axis using the VerticalAlignment property.
        /// <SfChart>
        ///     <ChartPrimaryYAxis>
        ///         <ChartStriplines>
        ///             <ChartStripline Start="20" End="25" VerticalAlignment="Anchor.Start" Text="Yaxis">
        ///             </ChartStripline>
        ///         </ChartStriplines>
        ///     </ChartPrimaryYAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public Anchor VerticalAlignment { get; set; } = Anchor.Middle;

        /// <summary> 
        /// Gets or sets a value indicating whether the strip line for the axis should be visible. 
        /// </summary> 
        /// <value> 
        /// <b>true</b> if the strip line for the axis should be visible; otherwise, <b>false</b>. 
        /// The default value is <b>true</b>. 
        /// </value> 
        /// <remarks>
        /// Use this property to toggle the strip line's visibility based on the specific design and functionality requirements of your application.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to hide a stripline on the Y-axis by setting the Visible property to 'false'.
        /// <SfChart>
        ///     <ChartPrimaryYAxis>
        ///         <ChartStriplines>
        ///             <ChartStripline Start="20" End="25" Visible="false">
        ///             </ChartStripline>
        ///         </ChartStriplines>
        ///     </ChartPrimaryYAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool Visible { get; set; } = true;

        /// <summary> 
        /// Gets or sets the order of the strip line in relation to series elements. 
        /// </summary> 
        /// <value> 
        /// One of the <see cref="ZIndex"/> enumerations that specify the order of the strip line. 
        /// The options include: 
        /// - <c>Behind</c>: Places the strip line behind the series elements. 
        /// - <c>Over</c>: Places the strip line over the series elements. 
        /// <br/>
        /// The default value is <b>ZIndexPosition.Behind</b>. 
        /// </value> 
        /// <remarks>
        /// Adjusting the <see cref="ZIndex"/> is useful when you need to control the visual layering of the strip line relative to other elements in a chart, thereby ensuring the intended elements are visible.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to render a stripline above the series elements by setting the ZIndex property to 'Over'.
        /// <SfChart>
        ///     <ChartPrimaryYAxis>
        ///         <ChartStriplines>
        ///             <ChartStripline Start="18" End="25" ZIndex="ZIndexPosition.Over">
        ///             </ChartStripline>
        ///         </ChartStriplines>
        ///     </ChartPrimaryYAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ZIndexPosition ZIndex { get; set; } = ZIndexPosition.Behind;

        /// <summary>
        /// Gets or sets the renderer key for internal use.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public string RendererKey { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the renderer type for this component.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public Type RendererType { get; set; } = null!;

        #endregion

        #region Lifecycle methods

        /// <summary>
        /// Performs component initialization when the component is first created.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Tracker is ChartStriplines chartStriplines)
            {
                Parent = chartStriplines;
            }
            Parent?.Axis?.Container?.AddStripline(this, ZIndex);
            RendererType = GetRendererType();
        }

        /// <summary>
        /// Handles parameter changes and processes property updates.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            // Color
            if (_color != Color)
            {
                _color = Color;
                Renderer?.UpdateCustomization(nameof(Color));
                _isPropertyChanged = Parent is not null;
            }

            // DashArray
            if (_dashArray != DashArray)
            {
                _dashArray = DashArray;
                Renderer?.UpdateCustomization(nameof(DashArray));
                _isPropertyChanged = Parent is not null;
            }

            // Opacity
            if (_opacity != Opacity)
            {
                _opacity = Opacity;
                Renderer?.UpdateCustomization(nameof(Opacity));
                _isPropertyChanged = Parent is not null;
            }

            // Text
            if (_text != Text)
            {
                _text = Text;
                Renderer?.UpdateCustomization(nameof(Text));
                _isPropertyChanged = Parent is not null;
            }

            // Start
            if (!Equals(_start, Start))
            {
                _start = Start;
                _isUpdateDirection = _isPropertyChanged = Parent is not null;
            }

            // End
            if (!Equals(_end, End))
            {
                _end = End;
                _isUpdateDirection = _isPropertyChanged = Parent is not null;
            }

            // Size
            if (_size != Size)
            {
                _size = Size;
                _isUpdateDirection = _isPropertyChanged = Parent is not null;
            }

            // Visible (special case)
            if (_visible != Visible)
            {
                _visible = Visible;

                if (Renderer is not null)
                {
                    if (ZIndex == ZIndexPosition.Behind)
                    {
                        Parent?.Axis?.Container?._striplineBehindContainer?.UpdateStriplineCollection();
                    }
                    else
                    {
                        Parent?.Axis?.Container?._striplineOverContainer?.UpdateStriplineCollection();
                    }

                    _isPropertyChanged = Parent is not null;
                }
            }
            if (_isPropertyChanged)
            {
                _isPropertyChanged = false;
                if (_isUpdateDirection)
                {
                    _isUpdateDirection = false;
                    Renderer?.UpdateDirection();
                }

                Renderer?.ProcessRenderQueue();
            }
        }

        /// <summary>
        /// Handles component disposal and cleanup.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override ValueTask DisposeAsyncCore()
        {
            Parent?.Axis?.Container?.RemoveStripline(this, ZIndex);
            return base.DisposeAsyncCore();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Determines the appropriate renderer type based on the <see cref="ZIndex"/> property.
        /// </summary>
        /// <returns>The renderer type for the stripline, either Behind or Over.</returns>
        private Type GetRendererType()
        {
            switch (ZIndex)
            {
                case ZIndexPosition.Behind:
                    return typeof(ChartStriplineBehindRenderer);
                case ZIndexPosition.Over:
                    return typeof(ChartStriplineOverRenderer);
                default:
                    break;
            }

            return null!;
        }
        #endregion

        #region Internal methods

        /// <summary>
        /// Sets the border styling for the stripline.
        /// </summary>
        /// <param name="border">The <see cref="ChartStriplineBorder"/> instance defining the border style.</param>
        internal void SetBorderValues(ChartStriplineBorder border)
        {
            Border = border;
        }

        /// <summary>
        /// Sets the text styling for the stripline label.
        /// </summary>
        /// <param name="textStyle">The <see cref="ChartStriplineTextStyle"/> instance defining the text style.</param>
        internal void SetTextStyleValue(ChartStriplineTextStyle textStyle)
        {
            TextStyle = textStyle;
        }

        /// <summary>
        /// Sets the tooltip settings for the stripline.
        /// </summary>
        /// <param name="settings">The <see cref="ChartStriplineTooltip"/> instance defining the tooltip behavior.</param>
        internal void SetTooltipSettings(ChartStriplineTooltip settings)
        {
            StriplineTooltip = settings;
        }
        #endregion
    }
}
