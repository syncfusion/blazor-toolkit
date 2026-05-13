using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Theme styles for the scrollbar components.
    /// </summary>
    public class ScrollbarThemeStyle
    {
        /// <summary>
        /// Gets or sets the back rectangle style.
        /// </summary>
        [JsonPropertyName("backRect")]
        public string BackRect { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the thumb style.
        /// </summary>
        [JsonPropertyName("thumb")]
        public string Thumb { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the circle style.
        /// </summary>
        [JsonPropertyName("circle")]
        public string Circle { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the circle hover style.
        /// </summary>
        [JsonPropertyName("circleHover")]
        public string CircleHover { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the arrow style.
        /// </summary>
        [JsonPropertyName("arrow")]
        public string Arrow { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the grip style.
        /// </summary>
        [JsonPropertyName("grip")]
        public string Grip { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the arrow hover style.
        /// </summary>
        [JsonPropertyName("arrowHover")]
        public string ArrowHover { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the back rectangle border style.
        /// </summary>
        [JsonPropertyName("backRectBorder")]
        public string BackRectBorder { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents touch point information for multi-touch interactions.
    /// </summary>
    public class Touches
    {
        /// <summary>
        /// Gets or sets the X coordinate of the touch point on the page.
        /// </summary>
        public double PageX { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate of the touch point on the page.
        /// </summary>
        public double PageY { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the touch pointer.
        /// </summary>
        public double PointerId { get; set; }
    }

    /// <summary>
    /// Represents the zoom range for a chart axis.
    /// </summary>
    public class ZoomAxisRange
    {
        internal double ActualMin { get; set; }

        internal double ActualDelta { get; set; }

        internal double Min { get; set; }

        internal double Delta { get; set; }
    }

    /// <summary>
    /// Represents the thickness of padding or spacing on all four sides.
    /// </summary>
    public class Thickness : DomRect
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Thickness"/> class with specified values.
        /// </summary>
        /// <param name="left">The left thickness value.</param>
        /// <param name="right">The right thickness value.</param>
        /// <param name="top">The top thickness value.</param>
        /// <param name="bottom">The bottom thickness value.</param>
        internal Thickness(double left, double right, double top, double bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }
    }

    /// <summary>
    /// Represents connection points for line or spline series.
    /// </summary>
    public class ConnectPoints
    {
        /// <summary>
        /// Gets or sets the first connection point.
        /// </summary>
        internal Point? First { get; set; }

        /// <summary>
        /// Gets or sets the last connection point.
        /// </summary>
        internal Point? Last { get; set; }
    }

    /// <summary>
    /// Represents a range with start and end values.
    /// </summary>
    public class FromTo
    {
        /// <summary>
        /// Gets or sets the start value of the range.
        /// </summary>
        internal double From { get; set; }

        /// <summary>
        /// Gets or sets the end value of the range.
        /// </summary>
        internal double To { get; set; }
    }

    /// <summary>
    /// Represents the position and count of rectangles in a layout.
    /// </summary>
    public class RectPosition
    {
        /// <summary>
        /// Gets or sets the position of the rectangle.
        /// </summary>
        internal double Position { get; set; }

        /// <summary>
        /// Gets or sets the count of rectangles.
        /// </summary>
        internal double RectCount { get; set; }
    }

    /// <summary>
    /// Represents the location of a symbol. 
    /// </summary>
    public class SymbolLocation
    {
        /// <summary>
        /// Gets or sets the location's x value.
        /// </summary>
        /// <value>
        /// Accepts the double value. The default value is 0.
        /// </value>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the location's y value.
        /// </summary>
        /// <value>
        /// Accepts the double value. The default value is 0.
        /// </value>
        public double Y { get; set; }
    }

    /// <summary>
    /// Represents the point attribute X, Y values. 
    /// </summary>
    public class AtrPoints
    {
        /// <summary>
        /// Gets or sets the x value.
        /// </summary>
        /// <value>
        /// Accepts the value as object. The default value is null.
        /// </value>
        public object X { get; set; } = null!;

        /// <summary>
        /// Gets or sets the y value.
        /// </summary>
        /// <value>
        /// Accepts the value as object. The default value is null.
        /// </value>
        public double Y { get; set; }
    }

    /// <summary>
    /// Represents a DOM rectangle with position and size information.
    /// </summary>
    public class DomRect : Size
    {
        /// <summary>
        /// Gets or sets the left position of the rectangle.
        /// </summary>
        public double Left { get; set; }

        /// <summary>
        /// Gets or sets the top position of the rectangle.
        /// </summary>
        public double Top { get; set; }

        /// <summary>
        /// Gets or sets the right position of the rectangle.
        /// </summary>
        public double Right { get; set; }

        /// <summary>
        /// Gets or sets the bottom position of the rectangle.
        /// </summary>
        public double Bottom { get; set; }
    }

    /// <summary>
    /// Represents browser-specific information and capabilities.
    /// </summary>
    public class Browser
    {
        /// <summary>
        /// Gets or sets the name of the browser.
        /// </summary>
        public string BrowserName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the browser supports pointer events.
        /// </summary>
        public bool IsPointer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the browser is running on a device.
        /// </summary>
        public bool IsDevice { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the browser supports touch events.
        /// </summary>
        public bool IsTouch { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the browser is running on iOS.
        /// </summary>
        public bool IsIos { get; set; }
    }

    /// <summary>
    /// Represents a financial data point with OHLC (Open, High, Low, Close) values.
    /// </summary>
    public class FinancialPoint : Point
    {
        /// <summary>
        /// Gets or sets the high value of the financial data point.
        /// </summary>
        public object High { get; set; } = null!;

        /// <summary>
        /// Gets or sets the low value of the financial data point.
        /// </summary>
        public object Low { get; set; } = null!;

        /// <summary>
        /// Gets or sets the open value of the financial data point.
        /// </summary>
        public object Open { get; set; } = null!;

        /// <summary>
        /// Gets or sets the close value of the financial data point.
        /// </summary>
        public object Close { get; set; } = null!;

        /// <summary>
        /// Gets or sets the volume value of the financial data point.
        /// </summary>
        public object Volume { get; set; } = null!;
    }

    /// <summary>
    /// Represents a box plot data point with statistical values.
    /// </summary>
    public class BoxPoint : Point
    {

        /// <summary>
        /// Gets or sets the upper quartile value.
        /// </summary>
        public double UpperQuartile { get; set; }

        /// <summary>
        /// Gets or sets the lower quartile value.
        /// </summary>
        public double LowerQuartile { get; set; }

        /// <summary>
        /// Gets or sets the median value.
        /// </summary>
        public double Median { get; set; }

        /// <summary>
        /// Gets or sets the array of outlier values.
        /// </summary>
        public double[] Outliers { get; set; } = null!;
        /// <summary>
        /// Gets or sets the collection of Y values.
        /// </summary>
        public double[] YValueCollection { get; set; } = null!;

        /// <summary>
        /// Gets or sets the average value.
        /// </summary>
        public double Average { get; set; }
    }

    /// <summary>
    /// Represents a bubble chart data point with size information.
    /// </summary>
    public class BubblePoint : Point
    {
        /// <summary>
        /// Gets or sets the size value of the bubble.
        /// </summary>
        public object Size { get; set; } = null!;
    }

    /// <summary>
    /// Represents a visible axis label with formatting and layout information.
    /// </summary>
    public class VisibleLabels
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VisibleLabels"/> class with specified values.
        /// </summary>
        /// <param name="text">The label text to display.</param>
        /// <param name="value">The numeric value of the label.</param>
        /// <param name="labelStyle">The style settings for the label.</param>
        /// <param name="originalText">The original text before formatting.</param>
        /// <param name="size">The size of the label. Default is 0x0.</param>
        /// <param name="breakLabelSize">The size of the label when broken into multiple lines. Default is 0x0.</param>
        /// <param name="index">The index of the label. Default is 1.</param>
        /// <param name="dateTimeValue">The date/time value associated with the label. Default is null.</param>
        internal VisibleLabels(string text, double value, ChartAxisLabelStyle labelStyle, string originalText, Size size = null!, Size breakLabelSize = null!, double index = 1, DateTime? dateTimeValue = null)
        {
            Text = text;
            TextArr = [text];
            OriginalText = originalText;
            Value = value;
            LabelStyle = labelStyle;
            Size = size is not null ? size : Size;
            BreakLabelSize = breakLabelSize is not null ? breakLabelSize : BreakLabelSize;
            Index = index;
            DateTimeValue = dateTimeValue ?? DateTime.MinValue;
        }

        /// <summary>
        /// Gets or sets the text of the label.
        /// </summary>
        internal string Text { get; set; }

        /// <summary>
        /// Gets or sets the text array representation of the label.
        /// </summary>
        internal string[] TextArr { get; set; }

        /// <summary>
        /// Gets or sets the numeric value of the label.
        /// </summary>
        internal double Value { get; set; }

        /// <summary>
        /// Gets or sets the style settings for the label.
        /// </summary>
        internal ChartAxisLabelStyle LabelStyle { get; set; }

        /// <summary>
        /// Gets or sets the size of the label.
        /// </summary>
        internal Size Size { get; set; } = new Size(0, 0);

        /// <summary>
        /// Gets or sets the size of the label when broken into multiple lines.
        /// </summary>
        internal Size BreakLabelSize { get; set; } = new Size(0, 0);

        /// <summary>
        /// Gets or sets the index of the label.
        /// </summary>
        internal double Index { get; set; }

        /// <summary>
        /// Gets or sets the original text before formatting.
        /// </summary>
        internal string OriginalText { get; set; }

        /// <summary>
        /// Gets or sets the template identifier for the label.
        /// </summary>
        internal string? TemplateID { get; set; }

        /// <summary>
        /// Gets or sets the size of the label template.
        /// </summary>
        internal Size TemplateSize { get; set; } = new Size(0, 0);

        /// <summary>
        /// Gets or sets the date/time value associated with the label.
        /// </summary>
        internal DateTime DateTimeValue { get; set; }
    }

    /// <summary>
    /// Represents the theme style settings for various chart elements.
    /// </summary>
    public class ChartThemeStyle
    {
        /// <summary>
        /// Gets or sets the axis label theme style.
        /// </summary>
        internal string? AxisLabel { get; set; }

        /// <summary>
        /// Gets or sets the axis title theme style.
        /// </summary>
        internal string? AxisTitle { get; set; }

        /// <summary>
        /// Gets or sets the axis line theme style.
        /// </summary>
        internal string? AxisLine { get; set; }

        /// <summary>
        /// Gets or sets the major grid line theme style.
        /// </summary>
        internal string? MajorGridLine { get; set; }

        /// <summary>
        /// Gets or sets the minor grid line theme style.
        /// </summary>
        internal string? MinorGridLine { get; set; }

        /// <summary>
        /// Gets or sets the major tick line theme style.
        /// </summary>
        internal string? MajorTickLine { get; set; }

        /// <summary>
        /// Gets or sets the minor tick line theme style.
        /// </summary>
        internal string? MinorTickLine { get; set; }

        /// <summary>
        /// Gets or sets the chart title theme style.
        /// </summary>
        internal string? ChartTitle { get; set; }

        /// <summary>
        /// Gets or sets the legend label theme style.
        /// </summary>
        internal string? LegendLabel { get; set; }

        /// <summary>
        /// Gets or sets the background theme style.
        /// </summary>
        internal string? Background { get; set; }

        /// <summary>
        /// Gets or sets the area border theme style.
        /// </summary>
        internal string? AreaBorder { get; set; }

        /// <summary>
        /// Gets or sets the error bar theme style.
        /// </summary>
        internal string? ErrorBar { get; set; }

        /// <summary>
        /// Gets or sets the crosshair line theme style.
        /// </summary>
        internal string? CrosshairLine { get; set; }

        /// <summary>
        /// Gets or sets the crosshair background theme style.
        /// </summary>
        internal string? CrosshairBackground { get; set; }

        /// <summary>
        /// Gets or sets the crosshair fill theme style.
        /// </summary>
        internal string? CrosshairFill { get; set; }

        /// <summary>
        /// Gets or sets the crosshair label theme style.
        /// </summary>
        internal string? CrosshairLabel { get; set; }

        /// <summary>
        /// Gets or sets the tooltip fill theme style.
        /// </summary>
        internal string? TooltipFill { get; set; }

        /// <summary>
        /// Gets or sets the tooltip bold label theme style.
        /// </summary>
        internal string? TooltipBoldLabel { get; set; }

        /// <summary>
        /// Gets or sets the tooltip light label theme style.
        /// </summary>
        internal string? TooltipLightLabel { get; set; }

        /// <summary>
        /// Gets or sets the tooltip header line theme style.
        /// </summary>
        internal string? TooltipHeaderLine { get; set; }

        /// <summary>
        /// Gets or sets the marker shadow theme style.
        /// </summary>
        internal string? MarkerShadow { get; set; }

        /// <summary>
        /// Gets or sets the selection rectangle fill theme style.
        /// </summary>
        internal string? SelectionRectFill { get; set; }

        /// <summary>
        /// Gets or sets the selection rectangle stroke theme style.
        /// </summary>
        internal string? SelectionRectStroke { get; set; }

        /// <summary>
        /// Gets or sets the selection circle stroke theme style.
        /// </summary>
        internal string? SelectionCircleStroke { get; set; }

        /// <summary>
        /// Gets or sets the tab color theme style.
        /// </summary>
        internal string? TabColor { get; set; }

        /// <summary>
        /// Gets or sets the ND line color theme style.
        /// </summary>
        internal string? NDLineColor { get; set; }

        /// <summary>
        /// Gets or sets the bear fill color theme style.
        /// </summary>
        internal string? BearFillColor { get; set; }

        /// <summary>
        /// Gets or sets the bull fill color theme style.
        /// </summary>
        internal string? BullFillColor { get; set; }

        /// <summary>
        /// Gets or sets the chart title size theme style.
        /// </summary>
        internal string? ChartTitleSize { get; set; }

        /// <summary>
        /// Gets or sets the chart title font weight theme style.
        /// </summary>
        internal string? ChartTitleFontWeight { get; set; }

        /// <summary>
        /// Gets or sets the chart title font family theme style.
        /// </summary>
        internal string? ChartTitleFontFamily { get; set; }

        /// <summary>
        /// Gets or sets the axis label font size theme style.
        /// </summary>
        internal string? AxisLabelFontSize { get; set; }

        /// <summary>
        /// Gets or sets the axis label font family theme style.
        /// </summary>
        internal string? AxisLabelFontFamily { get; set; }

        /// <summary>
        /// Gets or sets the axis label font weight theme style.
        /// </summary>
        internal string? AxisLabelFontWeight { get; set; }

        /// <summary>
        /// Gets or sets the tooltip text size theme style.
        /// </summary>
        internal string? TooltipTextSize { get; set; }

        /// <summary>
        /// Gets or sets the tooltip font family theme style.
        /// </summary>
        internal string? TooltipFontFamily { get; set; }

        /// <summary>
        /// Gets or sets the tooltip font weight theme style.
        /// </summary>
        internal string? ToolTipFontWeight { get; set; }

        /// <summary>
        /// Gets or sets the legend text size theme style.
        /// </summary>
        internal string? LegendTextSize { get; set; }

        /// <summary>
        /// Gets or sets the legend font family theme style.
        /// </summary>
        internal string? LegendFontFamily { get; set; }

        /// <summary>
        /// Gets or sets the legend font weight theme style.
        /// </summary>
        internal string? LegendFontWeight { get; set; }

        /// <summary>
        /// Gets or sets the crosshair text size theme style.
        /// </summary>
        internal string? CrosshairTextSize { get; set; }

        /// <summary>
        /// Gets or sets the crosshair font family theme style.
        /// </summary>
        internal string? CrosshairFontFamily { get; set; }

        /// <summary>
        /// Gets or sets the crosshair font weight theme style.
        /// </summary>
        internal string? CrosshairFontWeight { get; set; }

        /// <summary>
        /// Gets or sets the data label size theme style.
        /// </summary>
        internal string? DataLabelSize { get; set; }

        /// <summary>
        /// Gets or sets the data label font family theme style.
        /// </summary>
        internal string? DataLabelFontFamily { get; set; }

        /// <summary>
        /// Gets or sets the data label font weight theme style.
        /// </summary>
        internal string? DataLabelFontWeight { get; set; }

        /// <summary>
        /// Gets or sets the axis title font size theme style.
        /// </summary>
        internal string? AxisTitleFontSize { get; set; }

        /// <summary>
        /// Gets or sets the axis title font family theme style.
        /// </summary>
        internal string? AxisTitleFontFamily { get; set; }

        /// <summary>
        /// Gets or sets the axis title font weight theme style.
        /// </summary>
        internal string? AxisTitleFontWeight { get; set; }

        /// <summary>
        /// Gets or sets the chart subtitle theme style.
        /// </summary>
        internal string? ChartSubTitle { get; set; }

        /// <summary>
        /// Gets or sets the chart subtitle size theme style.
        /// </summary>
        internal string? ChartSubTitleSize { get; set; }

        /// <summary>
        /// Gets or sets the chart subtitle font weight theme style.
        /// </summary>
        internal string? ChartSubTitleFontWeight { get; set; }

        /// <summary>
        /// Gets or sets the chart subtitle font family theme style.
        /// </summary>
        internal string? ChartSubTitleFontFamily { get; set; }

        /// <summary>
        /// Gets or sets the stripline text color theme style.
        /// </summary>
        internal string? StriplineTextColor { get; set; }

        /// <summary>
        /// Gets or sets the stripline font size theme style.
        /// </summary>
        internal string? StriplineFontSize { get; set; }

        /// <summary>
        /// Gets or sets the stripline font family theme style.
        /// </summary>
        internal string? StriplineFontFamily { get; set; }

        /// <summary>
        /// Gets or sets the stripline font weight theme style.
        /// </summary>
        internal string? StriplineFontWeight { get; set; }

        /// <summary>
        /// Gets or sets the center label font size theme style.
        /// </summary>
        internal string? CenterLabelFontSize { get; set; }

        /// <summary>
        /// Gets or sets the center label font family theme style.
        /// </summary>
        internal string? CenterLabelFontFamily { get; set; }

        /// <summary>
        /// Gets or sets the center label font weight theme style.
        /// </summary>
        internal string? CenterLabelFontWeight { get; set; }
    }

    /// <summary>
    /// Represents point data with series information and lier index.
    /// </summary>
    public class PointData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PointData"/> class with specified values.
        /// </summary>
        /// <param name="point">The chart point.</param>
        /// <param name="series">The chart series containing the point.</param>
        /// <param name="lierIndex">The outlier index. Default is 0.</param>
        internal PointData(Point point, ChartSeries series, double lierIndex = 0)
        {
            Point = point;
            Series = series;
            LierIndex = lierIndex;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointData"/> class with no arguments.
        /// </summary>
        internal PointData()
        {
            // To create empty instance for further calculation not initial.
        }

        /// <summary>
        /// Gets or sets the chart point.
        /// </summary>
        public Point Point { get; set; } = null!;

        /// <summary>
        /// Gets or sets the chart series containing the point.
        /// </summary>
        public ChartSeries Series { get; set; } = null!;

        /// <summary>
        /// Gets or sets the outlier index.
        /// </summary>
        public double LierIndex { get; set; }
    }

    /// <summary>
    /// Represents client X and Y coordinates.
    /// </summary>
    public class ClientXY
    {
        /// <summary>
        /// Gets or sets the client X coordinate.
        /// </summary>
        public double ClientX { get; set; }

        /// <summary>
        /// Gets or sets the client Y coordinate.
        /// </summary>
        public double ClientY { get; set; }
    }

    /// <summary>
    /// Represents internal mouse event arguments for chart interactions.
    /// </summary>
    public class ChartInternalMouseEventArgs : ClientXY
    {
        /// <summary>
        /// Gets or sets a value indicating whether to prevent default behavior.
        /// </summary>
        public bool PreventDefault { get; set; }

        /// <summary>
        /// Gets or sets the collection of touch points.
        /// </summary>
        public List<Touches> Touches { get; set; } = null!;

        /// <summary>
        /// Gets or sets the type of event.
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the pointer identifier.
        /// </summary>
        public double PointerId { get; set; }

        /// <summary>
        /// Gets or sets the mouse X coordinate.
        /// </summary>
        public double MouseX { get; set; }

        /// <summary>
        /// Gets or sets the mouse Y coordinate.
        /// </summary>
        public double MouseY { get; set; }

        /// <summary>
        /// Gets or sets the pointer type.
        /// </summary>
        public string PointerType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the target element.
        /// </summary>
        public string Target { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the changed touch points.
        /// </summary>
        public ClientXY ChangedTouches { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public string ID { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents mouse wheel event arguments for chart zoom operations.
    /// </summary>
    public class ChartMouseWheelArgs : ClientXY
    {
        /// <summary>
        /// Gets or sets the detail value for the mouse wheel event.
        /// </summary>
        public double Detail { get; set; }

        /// <summary>
        /// Gets or sets the wheel delta value.
        /// </summary>
        public double WheelDelta { get; set; }

        /// <summary>
        /// Gets or sets the mouse X coordinate.
        /// </summary>
        public double MouseX { get; set; }

        /// <summary>
        /// Gets or sets the mouse Y coordinate.
        /// </summary>
        public double MouseY { get; set; }

        /// <summary>
        /// Gets or sets the name of the browser.
        /// </summary>
        public string BrowserName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the target element.
        /// </summary>
        public string Target { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the browser supports pointer events.
        /// </summary>
        public bool IsPointer { get; set; }
    }

    /// <summary>
    /// Represents stacked series start and end values.
    /// </summary>
    public class StackValues
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StackValues"/> class.
        /// </summary>
        /// <param name="startValue">The collection of start values.</param>
        /// <param name="endValue">The collection of end values.</param>
        internal StackValues(List<double> startValue, List<double> endValue)
        {
            StartValues = startValue;
            EndValues = endValue;
        }

        /// <summary>
        /// Gets or sets the collection of start values.
        /// </summary>
        internal List<double> StartValues { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of end values.
        /// </summary>
        internal List<double> EndValues { get; set; } = [];
    }

    /// <summary>
    /// Represents control points for spline curve calculations.
    /// </summary>
    public class ControlPoints
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ControlPoints"/> class.
        /// </summary>
        /// <param name="controlPoint1">The first control point.</param>
        /// <param name="controlPoint2">The second control point.</param>
        internal ControlPoints(ChartEventLocation controlPoint1, ChartEventLocation controlPoint2)
        {
            ControlPoint1 = controlPoint1;
            ControlPoint2 = controlPoint2;
        }

        /// <summary>
        /// Gets or sets the first control point.
        /// </summary>
        internal ChartEventLocation ControlPoint1 { get; set; }

        /// <summary>
        /// Gets or sets the second control point.
        /// </summary>
        internal ChartEventLocation ControlPoint2 { get; set; }
    }

    /// <summary>
    /// Represents slope and intercept values for linear calculations.
    /// </summary>
    public class SlopeIntercept
    {
        /// <summary>
        /// Gets or sets the slope value.
        /// </summary>
        internal double Slope { get; set; }

        /// <summary>
        /// Gets or sets the intercept value.
        /// </summary>
        internal double Intercept { get; set; }
    }

    /// <summary>
    /// Represents options for chart element animations.
    /// </summary>
    public class AnimationOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationOptions"/> class.
        /// </summary>
        /// <param name="id">The element identifier to animate.</param>
        /// <param name="type">The type of animation to apply.</param>
        internal AnimationOptions(string id, AnimationType type)
        {
            Id = id;
            Type = type;
        }

        /// <summary>
        /// Gets or sets the element identifier to animate.
        /// </summary>
        /// <remarks>
        /// This member needs to be passed to JavaScript, so it is declared as public.
        /// </remarks>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the type of animation to apply.
        /// </summary>
        internal AnimationType Type { get; set; }
    }

    /// <summary>
    /// Represents options for dynamic path animations.
    /// </summary>
    /// <remarks>
    /// NOTE: Need to pass all these members to JS so they are declared as public
    /// </remarks>
    public class DynamicPathAnimationOptions
    {
        /// <summary>
        /// Gets or sets the parent element identifier.
        /// </summary>
        public string ParentId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the element identifier.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the previous path direction.
        /// </summary>
        public string PreviousDir { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the current path direction.
        /// </summary>
        public string CurrentDir { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents options for dynamic rectangle animations.
    /// </summary>
    /// <remarks>
    /// NOTE: Need to pass all these members to JS so they are declared as public
    /// </remarks>
    public class DynamicRectAnimationOptions
    {
        /// <summary>
        /// Gets or sets the element identifier.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the previous rectangle.
        /// </summary>
        public Rect PreviousRect { get; set; } = null!;

        /// <summary>
        /// Gets or sets the current rectangle.
        /// </summary>
        public Rect CurrentRect { get; set; } = null!;
    }

    /// <summary>
    /// Represents options for dynamic last label animations.
    /// </summary>
    public class DynamicLastLabelOptions
    {
        /// <summary>
        /// Gets or sets the element identifier.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the previous transform value.
        /// </summary>
        public string PreviousTransform { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the current X coordinate.
        /// </summary>
        public double CurrentX { get; set; }

        /// <summary>
        /// Gets or sets the current Y coordinate.
        /// </summary>
        public double CurrentY { get; set; }

        /// <summary>
        /// Gets or sets the animation duration in milliseconds.
        /// </summary>
        public double Duration { get; set; }
    }

    /// <summary>
    /// Represents options for dynamic text position animations.
    /// </summary>
    /// <remarks>
    /// NOTE: Need to pass all these members to JS so they are declared as public
    /// </remarks>
    public class DynamicTextAnimationOptions
    {
        /// <summary>
        /// Gets or sets the element identifier.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the previous X location.
        /// </summary>
        public virtual double PreLocationX { get; set; }

        /// <summary>
        /// Gets or sets the previous Y location.
        /// </summary>
        public virtual double PreLocationY { get; set; }

        /// <summary>
        /// Gets or sets the current X location.
        /// </summary>
        public double CurLocationX { get; set; }

        /// <summary>
        /// Gets or sets the current Y location.
        /// </summary>
        public double CurLocationY { get; set; }

        /// <summary>
        /// Gets or sets the X attribute name.
        /// </summary>
        public string X { get; set; } = "x";

        /// <summary>
        /// Gets or sets the Y attribute name.
        /// </summary>
        public string Y { get; set; } = "y";
    }

    /// <summary>
    /// Represents options for dynamic accumulated text animations.
    /// </summary>
    /// <remarks>
    /// NOTE: Need to pass all these members to JS so they are declared as public
    /// </remarks>
    public class DynamicAccTextAnimationOptions : DynamicTextAnimationOptions
    {
        /// <summary>
        /// Gets or sets the previous X location as NaN by default.
        /// </summary>
        public override double PreLocationX { get; set; } = double.NaN;

        /// <summary>
        /// Gets or sets the previous Y location as NaN by default.
        /// </summary>
        public override double PreLocationY { get; set; } = double.NaN;
    }

    /// <summary>
    /// Represents mean and standard deviation values for error bar calculations.
    /// </summary>
    public class Mean
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Mean"/> class.
        /// </summary>
        /// <param name="verticalStandardMean">The vertical standard mean value.</param>
        /// <param name="verticalSquareRoot">The vertical square root value.</param>
        /// <param name="horizontalStandardMean">The horizontal standard mean value.</param>
        /// <param name="horizontalSquareRoot">The horizontal square root value.</param>
        /// <param name="verticalMean">The vertical mean value.</param>
        /// <param name="horizontalMean">The horizontal mean value.</param>
        internal Mean(double verticalStandardMean, double verticalSquareRoot, double horizontalStandardMean, double horizontalSquareRoot, double verticalMean, double horizontalMean)
        {
            VerticalStandardMean = verticalStandardMean;
            HorizontalStandardMean = horizontalStandardMean;
            VerticalSquareRoot = verticalSquareRoot;
            HorizontalSquareRoot = horizontalSquareRoot;
            VerticalMean = verticalMean;
            HorizontalMean = horizontalMean;
        }

        /// <summary>
        /// Gets or sets the vertical standard mean value.
        /// </summary>
        internal double VerticalStandardMean { get; set; }

        /// <summary>
        /// Gets or sets the horizontal standard mean value.
        /// </summary>
        internal double HorizontalStandardMean { get; set; }

        /// <summary>
        /// Gets or sets the vertical square root value.
        /// </summary>
        internal double VerticalSquareRoot { get; set; }

        /// <summary>
        /// Gets or sets the horizontal square root value.
        /// </summary>
        internal double HorizontalSquareRoot { get; set; }

        /// <summary>
        /// Gets or sets the vertical mean value.
        /// </summary>
        internal double VerticalMean { get; set; }

        /// <summary>
        /// Gets or sets the horizontal mean value.
        /// </summary>
        internal double HorizontalMean { get; set; }
    }

    /// <summary>
    /// Represents Bollinger Band indicator values.
    /// </summary>
    public class BollingerPoints
    {
        /// <summary>
        /// Gets or sets the upper bound value.
        /// </summary>
        internal double UpperBound { get; set; }

        /// <summary>
        /// Gets or sets the lower bound value.
        /// </summary>
        internal double LowerBound { get; set; }

        /// <summary>
        /// Gets or sets the middle bound value.
        /// </summary>
        internal double MiddleBound { get; set; }
    }

    /// <summary>
    /// Represents legend item options and metadata.
    /// </summary>
    public class LegendOption
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LegendOption"/> class with default values.
        /// </summary>
        internal LegendOption()
        {
            // To create empty instance for further calculation not initial.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LegendOption"/> class with specified values.
        /// </summary>
        /// <param name="id">The template identifier.</param>
        /// <param name="templateSize">The size of the legend template.</param>
        /// <param name="textSize">The size of the legend text.</param>
        /// <param name="text">The legend text to display.</param>
        /// <param name="fill">The fill color for the legend shape.</param>
        /// <param name="shape">The legend shape type.</param>
        /// <param name="visible">Whether the legend item is visible.</param>
        /// <param name="type">The legend item type.</param>
        /// <param name="accType">The accumulation chart type (optional).</param>
        /// <param name="markerShape">The marker shape type (optional).</param>
        /// <param name="markerVisibility">Whether the marker is visible (optional).</param>
        /// <param name="pointIndex">The point index (optional).</param>
        /// <param name="seriesIndex">The series index (optional).</param>
        /// <param name="seriesBorderColor">The series border color (optional).</param>
        /// <param name="seriesBorderWidth">The series border width (optional).</param>
        /// <param name="seriesWidth">The series line width (optional).</param>
        /// <param name="textStyle">The text style settings (optional).</param>
        /// <param name="dashArray">The stroke dash array pattern (optional).</param>
        /// <param name="legendTemplate">The legend template render fragment (optional).</param>
        /// <param name="legendTemplateText">The legend template text content (optional).</param>
        /// <param name="locatedPageIndex">The page index where the legend is located (optional).</param>
        internal LegendOption(string id, Size templateSize, Size textSize, string text, string fill, LegendShape shape, bool visible, string type, [Optional] string accType, [Optional] ChartShape markerShape, [Optional] bool markerVisibility, [Optional] double pointIndex, [Optional] double seriesIndex, [Optional] string seriesBorderColor, [Optional] double seriesBorderWidth, [Optional] double seriesWidth, [Optional] ChartLegendTextStyle textStyle, [Optional] string dashArray, [Optional] RenderFragment legendTemplate, [Optional] string legendTemplateText, [Optional] int locatedPageIndex)
        {
            TemplateID = id;
            TemplateSize = templateSize;
            TextSize = textSize;
            Text = text;
            Fill = fill;
            Shape = shape;
            Visible = visible;
            Type = type;
            AccType = accType;
            MarkerVisibility = markerVisibility;
            MarkerShape = markerShape;
            PointIndex = pointIndex;
            SeriesIndex = seriesIndex;
            SeriesBorderColor = seriesBorderColor;
            SeriesBorderWidth = seriesBorderWidth;
            SeriesWidth = seriesWidth;
            TextStyle = textStyle;
            StrokeDashArray = dashArray;
            LegendTemplate = legendTemplate;
            LegendTemplateText = legendTemplateText;
            LocatedPageIndex = locatedPageIndex;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the legend item should be rendered.
        /// </summary>
        public bool Render { get; set; }

        /// <summary>
        /// Gets or sets the template identifier.
        /// </summary>
        internal string? TemplateID { get; set; }

        /// <summary>
        /// Gets or sets the size of the legend template.
        /// </summary>
        internal Size? TemplateSize { get; set; }

        /// <summary>
        /// Gets or sets the text to display in the legend.
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the text collection for the legend.
        /// </summary>
        internal List<string>? TextCollection { get; set; }

        /// <summary>
        /// Gets or sets the fill color for the legend shape.
        /// </summary>
        public string Fill { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the shape type of the legend.
        /// </summary>
        public LegendShape Shape { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the legend item is visible.
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// Gets or sets the type of the legend item.
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the accumulation chart type.
        /// </summary>
        public string AccType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the size of the legend text.
        /// </summary>
        public Size TextSize { get; set; } = null!;

        /// <summary>
        /// Gets or sets the location of the legend item.
        /// </summary>
        public ChartEventLocation Location { get; set; } = new ChartEventLocation(0, 0);

        /// <summary>
        /// Gets or sets the point index for the legend item.
        /// </summary>
        public double PointIndex { get; set; }

        /// <summary>
        /// Gets or sets the series index for the legend item.
        /// </summary>
        public double SeriesIndex { get; set; }

        /// <summary>
        /// Gets or sets the marker shape type.
        /// </summary>
        public ChartShape MarkerShape { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the marker is visible.
        /// </summary>
        public bool MarkerVisibility { get; set; }

        /// <summary>
        /// Gets or sets the border color of the series in the legend.
        /// </summary>
        public string SeriesBorderColor { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the border width of the series in the legend.
        /// </summary>
        public double SeriesBorderWidth { get; set; }

        /// <summary>
        /// Gets or sets the line width of the series in the legend.
        /// </summary>
        public double SeriesWidth { get; set; }

        /// <summary>
        /// Gets or sets the text style settings for the legend.
        /// </summary>
        public ChartLegendTextStyle TextStyle { get; set; } = null!;

        /// <summary>
        /// Gets or sets the row index of the legend item.
        /// </summary>
        internal int RowIndex { get; set; }

        /// <summary>
        /// Gets or sets the stroke dash array pattern for the legend shape.
        /// </summary>
        public string StrokeDashArray { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the legend template render fragment.
        /// </summary>
        internal RenderFragment? LegendTemplate { get; set; }

        /// <summary>
        /// Gets or sets the legend template text content.
        /// </summary>
        internal string LegendTemplateText { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the page index where the legend is located.
        /// </summary>
        internal int LocatedPageIndex { get; set; }
    }

    /// <summary>
    /// Represents options for marker animation information.
    /// </summary>
    /// <remarks>
    /// NOTE: Need to pass all these members to JS so they are declared as public
    /// </remarks>
    public class MarkerAnimationInfo
    {
        /// <summary>
        /// Gets or sets the marker element identifier.
        /// </summary>
        public string MarkerElementId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the marker clip path identifier.
        /// </summary>
        public string MarkerClipPathId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection of point indices.
        /// </summary>
        public List<double> PointIndex { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of low point indices.
        /// </summary>
        public List<double> LowPointIndex { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of point X coordinates.
        /// </summary>
        public List<double> PointX { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of point Y coordinates.
        /// </summary>
        public List<double> PointY { get; set; } = new List<double>();

        /// <summary>
        /// Gets or sets the collection of low point X coordinates.
        /// </summary>
        public List<double> LowPointX { get; set; } = new List<double>();

        /// <summary>
        /// Gets or sets the collection of low point Y coordinates.
        /// </summary>
        public List<double> LowPointY { get; set; } = new List<double>();
    }

    /// <summary>
    /// Represents options for error bar animation information.
    /// </summary>
    /// <remarks>
    /// NOTE: Need to pass all these members to JS so they are declared as public
    /// </remarks>
    public class ErrorBarAnimationInfo
    {
        /// <summary>
        /// Gets or sets the error bar element identifier.
        /// </summary>
        public string ErrorBarElementId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the error bar clip path identifier.
        /// </summary>
        public string ErrorBarClipPathId { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents options for data label animation information.
    /// </summary>
    /// <remarks>
    /// NOTE: Need to pass all these members to JS so they are declared as public
    /// </remarks>
    public class DataLabelAnimatioInfo
    {
        /// <summary>
        /// Gets or sets the shape group element identifier.
        /// </summary>
        public string ShapeGroupId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the text group element identifier.
        /// </summary>
        public string TextGroupId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection of template identifiers.
        /// </summary>
        public List<string> TemplateId { get; set; } = new List<string>();
    }

    /// <summary>
    /// Represents options for initial animation information.
    /// </summary>
    /// <remarks>
    /// NOTE: Need to pass all these members to JS so they are declared as public
    /// </remarks>
    public class InitialAnimationInfo
    {
        /// <summary>
        /// Gets or sets the type of animation.
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the element identifier.
        /// </summary>
        public string ElementId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the clip path identifier.
        /// </summary>
        public string ClipPathId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection of point indices.
        /// </summary>
        public List<double> PointIndex { get; set; } = new List<double>();

        /// <summary>
        /// Gets or sets the collection of point X coordinates.
        /// </summary>
        public List<double> PointX { get; set; } = new List<double>();

        /// <summary>
        /// Gets or sets the collection of point Y coordinates.
        /// </summary>
        public List<double> PointY { get; set; } = new List<double>();

        /// <summary>
        /// Gets or sets the collection of point widths.
        /// </summary>
        public List<double> PointWidth { get; set; } = new List<double>();

        /// <summary>
        /// Gets or sets the collection of point heights.
        /// </summary>
        public List<double> PointHeight { get; set; } = new List<double>();

        /// <summary>
        /// Gets or sets the animation duration in milliseconds.
        /// </summary>
        public double Duration { get; set; }

        /// <summary>
        /// Gets or sets the animation delay in milliseconds.
        /// </summary>
        public double Delay { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the axis is inverted.
        /// </summary>
        public bool IsInvertedAxis { get; set; }

        /// <summary>
        /// Gets or sets the stroke dash array pattern.
        /// </summary>
        public string StrokeDashArray { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the marker animation information.
        /// </summary>
        public MarkerAnimationInfo MarkerInfo { get; set; } = null!;

        /// <summary>
        /// Gets or sets the error bar animation information.
        /// </summary>
        public ErrorBarAnimationInfo ErrorBarInfo { get; set; } = null!;

        /// <summary>
        /// Gets or sets the data label animation information.
        /// </summary>
        public DataLabelAnimatioInfo DataLabelInfo { get; set; } = null!;
    }

    /// <summary>
    /// Represents options for chart font options.
    /// </summary>
    public class ChartFontOptions : FontOptions
    {
        /// <summary>
        /// Gets or sets the text overflow behavior.
        /// </summary>
        internal TextOverflow TextOverflow { get; set; }

        /// <summary>
        /// Gets or sets the text alignment.
        /// </summary>
        internal Alignment TextAlignment { get; set; }
    }


    /// <summary>
    /// Represents options for label template options.
    /// </summary>
    public class LabelTemplateOptions
    {
        /// <summary>
        /// Gets or sets the X position of the label.
        /// </summary>
        public string X { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Y position of the label.
        /// </summary>
        public string Y { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the identifier for the label.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the size of the label template.
        /// </summary>
        public Size TemplateSize { get; set; } = new Size(0, 0);

        /// <summary>
        /// Gets or sets the axis label information.
        /// </summary>
        public ChartAxisLabelInfo AxisInfo { get; set; } = null!;

        /// <summary>
        /// Gets or sets the width of the label.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the label.
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Gets or sets the transform value for the label.
        /// </summary>
        public string Transform { get; set; } = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="LabelTemplateOptions"/> class with no arguments.
        /// </summary>
        public LabelTemplateOptions()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LabelTemplateOptions"/> class with specified values.
        /// </summary>
        /// <param name="x">The X position of the label.</param>
        /// <param name="y">The Y position of the label.</param>
        /// <param name="id">The identifier for the label.</param>
        /// <param name="axisInfo">The axis label information.</param>
        /// <param name="width">The width of the label.</param>
        /// <param name="height">The height of the label.</param>
        public LabelTemplateOptions(string x, string y, string id, ChartAxisLabelInfo axisInfo, double width, double height)
        {
            X = x;
            Y = y;
            Id = id;
            AxisInfo = axisInfo;
            Width = width;
            Height = height;
        }
    }

    /// <summary>
    /// Represents options for axis render information.
    /// </summary>
    internal class AxisRenderInfo
    {
        internal Dictionary<string, List<PathOptions>> AxisGridOptions { get; set; } = [];

        internal List<TextOptions> AxisLabelOptions { get; set; } = [];

        internal List<LabelTemplateOptions> LabelTemplateOptions { get; set; } = [];

        internal List<CircleOptions> MajorGridCircleOptions { get; set; } = [];

        internal TextOptions? AxisTitleOption { get; set; }

        internal PathOptions? AxisLine { get; set; }

        internal PathOptions? AxisBorder { get; set; }

        internal PathOptions? RowBorder { get; set; }

        internal PathOptions? ColumnBorder { get; set; }

        internal List<KeyValuePair<string, Size>> AxisLabelTemplateSizeList { get; set; } = [];
    }

    /// <summary>
    /// Represents options for selection highlight.
    /// </summary>
    internal class SelectionHighlightOptions
    {
        [JsonPropertyName("pinchZoomingEnable")]
        public bool PinchZoomingEnable { get; set; }

        [JsonPropertyName("toggleVisibility")]
        public bool ToggleVisibility { get; set; }

        [JsonPropertyName("enableHighlight")]
        public bool EnableHighlight { get; set; }
        [JsonPropertyName("selectionMode")]
        public string SelectionMode { get; set; } = string.Empty;

        [JsonPropertyName("highlightMode")]
        public string HighlightMode { get; set; } = string.Empty;

        [JsonPropertyName("seriesTypes")]
        public string[] SeriesTypes { get; set; } = null!;

        [JsonPropertyName("selectedDataIndexes")]
        public ChartSelectedDataIndex[] SelectedDataIndexes { get; set; } = null!;

        [JsonPropertyName("highlightColor")]
        public string HighlightColor { get; set; } = string.Empty;

        [JsonPropertyName("highlightPattern")]
        public string HighlightPattern { get; set; } = string.Empty;

        [JsonPropertyName("selectionPattern")]
        public string SelectionPattern { get; set; } = string.Empty;

        [JsonPropertyName("allowMultiSelection")]
        public bool AllowMultiSelection { get; set; }
    }

    /// <summary>
    /// Represents options for tooltip.
    /// </summary>
    internal class TooltipOptions
    {
        /// <summary>
        /// Gets or sets the available size for the tooltip.
        /// </summary>
        [JsonPropertyName("availableSize")]
        public Size AvailableSize { get; set; } = null!;

        /// <summary>
        /// Gets or sets the border width of the tooltip.
        /// </summary>
        [JsonPropertyName("borderWidth")]
        public double BorderWidth { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to disable track tooltip.
        /// </summary>
        [JsonPropertyName("disableTrackTooltip")]
        public bool DisableTrackTooltip { get; set; }

        /// <summary>
        /// Gets or sets the axis clip rectangle for the tooltip.
        /// </summary>
        [JsonPropertyName("axisClipRect")]
        public Rect AxisClipRect { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether a point is being dragged.
        /// </summary>
        [JsonPropertyName("isPointDragging")]
        public bool IsPointDragging { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether mouse button is down on a point.
        /// </summary>
        [JsonPropertyName("isPointMouseDown")]
        public bool IsPointMouseDown { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the axis is inverted.
        /// </summary>
        [JsonPropertyName("isInverted")]
        public bool IsInverted { get; set; }

        /// <summary>
        /// Gets or sets the chart area type.
        /// </summary>
        [JsonPropertyName("chartAreaType")]
        public string ChartAreaType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the tooltip format string.
        /// </summary>
        [JsonPropertyName("tooltipFormat")]
        public string TooltipFormat { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether markers are enabled.
        /// </summary>
        [JsonPropertyName("isMarkerEnable")]
        public bool MarkerEnable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether RTL is enabled.
        /// </summary>
        [JsonPropertyName("enableRTL")]
        public bool EnableRTL { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether highlighting is enabled.
        /// </summary>
        [JsonPropertyName("enableHighlight")]
        public bool EnableHighlight { get; set; }

        /// <summary>
        /// Gets or sets the crosshair settings.
        /// </summary>
        [JsonPropertyName("crosshair")]
        public ChartCrosshairSettings Crosshair { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether marker explode is enabled.
        /// </summary>
        [JsonPropertyName("markerExplode")]
        public bool MarkerExplode { get; set; }

        /// <summary>
        /// Gets or sets the chart radius.
        /// </summary>
        [JsonPropertyName("chartRadius")]
        public double ChartRadius { get; set; }

        /// <summary>
        /// Gets or sets the theme name for the tooltip.
        /// </summary>
        [JsonPropertyName("theme")]
        public string Theme { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the crosshair line theme style.
        /// </summary>
        [JsonPropertyName("themeStyleCrosshairLine")]
        public string ThemeStyleCrosshairLine { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the crosshair background theme style.
        /// </summary>
        [JsonPropertyName("themeStyleCrosshairBackground")]
        public string ThemeStyleCrosshairBackground { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the crosshair fill theme style.
        /// </summary>
        [JsonPropertyName("themeStyleCrosshairFill")]
        public string ThemeStyleCrosshairFill { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the crosshair label theme style.
        /// </summary>
        [JsonPropertyName("themeStyleCrosshairLabel")]
        public string ThemeStyleCrosshairLabel { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the crosshair text size theme style.
        /// </summary>
        [JsonPropertyName("themeStyleCrosshairTextSize")]
        public string ThemeStyleCrosshairTextSize { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the crosshair font family theme style.
        /// </summary>
        [JsonPropertyName("themeStyleCrosshairFontFamily")]
        public string ThemeStyleCrosshairFontFamily { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the crosshair font weight theme style.
        /// </summary>
        [JsonPropertyName("themeStyleCrosshairFontWeight")]
        public string ThemeStyleCrosshairFontWeight { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the template string for the tooltip.
        /// </summary>
        [JsonPropertyName("templateString")]
        public string TemplateString { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the initial rectangle for the tooltip.
        /// </summary>
        [JsonPropertyName("initialRect")]
        public Rect InitialRect { get; set; } = null!;

        /// <summary>
        /// Gets or sets the secondary element offset.
        /// </summary>
        [JsonPropertyName("secondaryElementOffset")]
        public DomRect SecondaryElementOffset { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether tooltip event was called.
        /// </summary>
        [JsonPropertyName("tooltipEventCalled")]
        public bool TooltipEventCalled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether shared tooltip event was called.
        /// </summary>
        [JsonPropertyName("sharedTooltipEventCalled")]
        public bool SharedTooltipEventCalled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether crosshair mouse move event was called.
        /// </summary>
        [JsonPropertyName("crosshairMouseMoveEventCalled")]
        public bool CrosshairMouseMoveEventCalled { get; set; }

        /// <summary>
        /// Gets or sets the series tooltip top position.
        /// </summary>
        [JsonPropertyName("seriesTooltipTop")]
        public double SeriesTooltipTop { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether grouping is used for tooltips.
        /// </summary>
        [JsonPropertyName("useGrouping")]
        public bool UseGrouping { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the tooltip is focusable.
        /// </summary>
        [JsonPropertyName("focusable")]
        public bool Focusable { get; set; }
    }

    /// <summary>
    /// Represents options for chart point.
    /// </summary>
    public class IChartPoint
    {
        /// <summary>
        /// Gets or sets the symbol locations for the point.
        /// </summary>
        [JsonPropertyName("s")]
        public List<IChartInternalLocation> SymbolLocations { get; set; } = new List<IChartInternalLocation>();

        /// <summary>
        /// Gets or sets the regions for the point.
        /// </summary>
        [JsonPropertyName("r")]
        public List<IRect> Regions { get; set; } = new List<IRect>();

        /// <summary>
        /// Gets or sets the X value of the point.
        /// </summary>
        [JsonPropertyName("xV")]
        public double XValue { get; set; }

        /// <summary>
        /// Gets or sets the Y value of the point.
        /// </summary>
        [JsonPropertyName("yV")]
        public double YValue { get; set; }

        /// <summary>
        /// Gets or sets the X coordinate of the point.
        /// </summary>
        [JsonPropertyName("x")]
        public object X { get; set; } = null!;

        /// <summary>
        /// Gets or sets the Y coordinate of the point.
        /// </summary>
        [JsonPropertyName("y")]
        public object Y { get; set; } = null!;

        /// <summary>
        /// Gets or sets the text for the point.
        /// </summary>
        [JsonPropertyName("t")]
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the tooltip for the point.
        /// </summary>
        [JsonPropertyName("tT")]
        public string Tooltip { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the interior color for the point.
        /// </summary>
        [JsonPropertyName("i")]
        public string Interior { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the region data for the point.
        /// </summary>
        [JsonPropertyName("rD")]
        public PolarArc RegionData { get; set; } = null!;

        /// <summary>
        /// Gets or sets the index of the point.
        /// </summary>
        [JsonPropertyName("iX")]
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the percentage value of the point.
        /// </summary>
        [JsonPropertyName("p")]
        public double Percentage { get; set; }

        /// <summary>
        /// Gets or sets the marker setting for the point.
        /// </summary>
        [JsonPropertyName("mK")]
        public IMarkerSettingModel Marker { get; set; } = null!;

        /// <summary>
        /// Gets or sets the sum of same index value.
        /// </summary>
        public double SumOfSameIndex { get; set; }
    }

    /// <summary>
    /// Represents options for financial point.
    /// </summary>
    public class IFinancialPoint : IChartPoint
    {
        /// <summary>
        /// Gets or sets the high value of the financial point.
        /// </summary>
        [JsonPropertyName("h")]
        public object High { get; set; } = null!;

        /// <summary>
        /// Gets or sets the low value of the financial point.
        /// </summary>
        [JsonPropertyName("l")]
        public object Low { get; set; } = null!;

        /// <summary>
        /// Gets or sets the open value of the financial point.
        /// </summary>
        [JsonPropertyName("o")]
        public object Open { get; set; } = null!;

        /// <summary>
        /// Gets or sets the close value of the financial point.
        /// </summary>
        [JsonPropertyName("c")]
        public object Close { get; set; } = null!;

        /// <summary>
        /// Gets or sets the volume value of the financial point.
        /// </summary>
        [JsonPropertyName("v")]
        public object Volume { get; set; } = null!;
    }

    /// <summary>
    /// Represents options for bubble point.
    /// </summary>
    public class IBubblePoint : IChartPoint
    {
        /// <summary>
        /// Gets or sets the size value of the bubble point.
        /// </summary>
        [JsonPropertyName("sI")]
        public object Size { get; set; } = null!;
    }

    /// <summary>
    /// Represents options for Rect.
    /// </summary>
    public class IRect
    {
        /// <summary>
        /// Gets or sets the X coordinate of the rectangle.
        /// </summary>
        [JsonPropertyName("x")]
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate of the rectangle.
        /// </summary>
        [JsonPropertyName("y")]
        public double Y { get; set; }

        /// <summary>
        /// Gets or sets the width of the rectangle.
        /// </summary>
        [JsonPropertyName("w")]
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the rectangle.
        /// </summary>
        [JsonPropertyName("h")]
        public double Height { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IRect"/> class with no arguments.
        /// </summary>
        public IRect() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="IRect"/> class with specified values.
        /// </summary>
        /// <param name="x">The X coordinate of the rectangle.</param>
        /// <param name="y">The Y coordinate of the rectangle.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        public IRect(double x, double y, double width, double height)
        {
            X = !double.IsNaN(x) ? x : 0;
            Y = !double.IsNaN(y) ? y : 0;
            Width = !double.IsNaN(width) ? width : 0;
            Height = !double.IsNaN(height) ? height : 0;
        }
    }

    /// <summary>
    /// Represents options for chart internal location.
    /// </summary>
    public class IChartInternalLocation
    {
        /// <summary>
        /// Gets or sets the X coordinate of the location.
        /// </summary>
        public double x { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate of the location.
        /// </summary>
        public double y { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IChartInternalLocation"/> class with specified coordinates.
        /// </summary>
        /// <param name="locationX">The X coordinate of the location.</param>
        /// <param name="locationY">The Y coordinate of the location.</param>
        public IChartInternalLocation(double locationX, double locationY)
        {
            x = locationX;
            y = locationY;
        }
    }

    /// <summary>
    /// Represents options for chart event border.
    /// </summary>
    public class IChartEventBorder
    {
        /// <summary>
        /// Gets or sets the border color.
        /// </summary>
        [JsonPropertyName("cL")]
        public string Color { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the border width.
        /// </summary>
        [JsonPropertyName("wT")]
        public double Width { get; set; }
    }

    /// <summary>
    /// Represents options for marker setting model.
    /// </summary>
    public class IMarkerSettingModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether the marker is visible.
        /// </summary>
        [JsonPropertyName("vS")]
        public bool Visible { get; set; }

        /// <summary>
        /// Gets or sets the marker border settings.
        /// </summary>
        [JsonPropertyName("b")]
        public IChartEventBorder Border { get; set; } = new IChartEventBorder() { Width = 2 };

        /// <summary>
        /// Gets or sets the fill color of the marker.
        /// </summary>
        [JsonPropertyName("f")]
        public string Fill { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the width of the marker.
        /// </summary>
        [JsonPropertyName("mW")]
        public double Width { get; set; } = Constants.MarkerSize;

        /// <summary>
        /// Gets or sets the height of the marker.
        /// </summary>
        [JsonPropertyName("mH")]
        public double Height { get; set; } = Constants.MarkerSize;

        /// <summary>
        /// Gets or sets the shape of the marker.
        /// </summary>
        [JsonPropertyName("sH")]
        public string Shape { get; set; } = "Auto";

        /// <summary>
        /// Gets or sets the image URL for the marker.
        /// </summary>
        [JsonPropertyName("iU")]
        public string ImageUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the opacity of the marker.
        /// </summary>
        [JsonPropertyName("oP")]
        public double Opacity { get; set; } = Constants.DefaultOpacity;

        /// <summary>
        /// Gets or sets a value indicating whether highlighting is allowed.
        /// </summary>
        [JsonPropertyName("aH")]
        public bool AllowHighlight { get; set; }
    }

    /// <summary>
    /// Represents options for axis.
    /// </summary>
    public class IAxis
    {
        /// <summary>
        /// Gets or sets the name of the axis.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether to place the axis next to the axis line.
        /// </summary>
        [JsonPropertyName("placeNextToAxisLine")]
        public bool PlaceNextToAxisLine { get; set; }

        /// <summary>
        /// Gets or sets the rectangle bounds of the axis.
        /// </summary>
        [JsonPropertyName("rect")]
        public IRect Rect { get; set; } = null!;

        /// <summary>
        /// Gets or sets the updated rectangle bounds of the axis.
        /// </summary>
        [JsonPropertyName("updatedRect")]
        public IRect UpdatedRect { get; set; } = null!;

        /// <summary>
        /// Gets or sets the value type of the axis.
        /// </summary>
        [JsonPropertyName("valueType")]
        public string ValueType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the label placement for the axis.
        /// </summary>
        [JsonPropertyName("labelPlacement")]
        public string LabelPlacement { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the axis is positioned opposite.
        /// </summary>
        [JsonPropertyName("isAxisOppositePosition")]
        public bool IsAxisOppositePosition { get; set; }

        /// <summary>
        /// Gets or sets the orientation of the axis.
        /// </summary>
        [JsonPropertyName("orientation")]
        public string Orientation { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the plot offset for the axis.
        /// </summary>
        [JsonPropertyName("plotOffset")]
        public double PlotOffset { get; set; }

        /// <summary>
        /// Gets or sets the visible range of the axis.
        /// </summary>
        [JsonPropertyName("visibleRange")]
        public DoubleRange VisibleRange { get; set; }

        /// <summary>
        /// Gets or sets the actual range of the axis.
        /// </summary>
        [JsonPropertyName("actualRange")]
        public DoubleRange ActualRange { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the axis is inverted.
        /// </summary>
        [JsonPropertyName("isAxisInverse")]
        public bool IsAxisInverse { get; set; }

        /// <summary>
        /// Gets or sets the date format for the axis.
        /// </summary>
        [JsonPropertyName("dateFormat")]
        public string DateFormat { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the label format for the axis.
        /// </summary>
        [JsonPropertyName("labelFormat")]
        public string LabelFormat { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the label position for the axis.
        /// </summary>
        [JsonPropertyName("labelPosition")]
        public string LabelPosition { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection of label texts for the axis.
        /// </summary>
        [JsonPropertyName("labels")]
        public List<string> Labels { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether scrollbar settings are enabled.
        /// </summary>
        [JsonPropertyName("scrollbarSettingsEnable")]
        public bool ScrollbarSettingsEnable { get; set; }

        /// <summary>
        /// Gets or sets the height of the scrollbar.
        /// </summary>
        [JsonPropertyName("scrollBarHeight")]
        public double ScrollBarHeight { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the axis is stacked at 100%.
        /// </summary>
        [JsonPropertyName("isStack100")]
        public bool IsStack100 { get; set; }

        /// <summary>
        /// Gets or sets the actual interval type for the axis.
        /// </summary>
        [JsonPropertyName("actualIntervalType")]
        public string ActualIntervalType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the range interval type for the axis.
        /// </summary>
        [JsonPropertyName("rangeIntervalType")]
        public string RangeIntervalType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the format string for the axis.
        /// </summary>
        [JsonPropertyName("format")]
        public string Format { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the logarithmic base for the axis.
        /// </summary>
        [JsonPropertyName("logBase")]
        public double LogBase { get; set; }

        /// <summary>
        /// Gets or sets the crosshair tooltip settings for the axis.
        /// </summary>
        [JsonPropertyName("crosshairTooltip")]
        public ChartAxisCrosshairTooltip CrosshairTooltip { get; set; } = null!;

        /// <summary>
        /// Gets or sets the start angle for the axis.
        /// </summary>
        [JsonPropertyName("startAngle")]
        public double StartAngle { get; set; }

        /// <summary>
        /// Gets or sets the date/time interval for the axis.
        /// </summary>
        [JsonPropertyName("dataTimeInterval")]
        public double DataTimeInterval { get; set; }

        /// <summary>
        /// Gets or sets the visible interval for the axis.
        /// </summary>
        [JsonPropertyName("visibleInterval")]
        public double VisibleInterval { get; set; }

        /// <summary>
        /// Gets or sets the visible label count for the axis.
        /// </summary>
        [JsonPropertyName("visibleLabelCount")]
        public double VisibleLabelCount { get; set; }

        /// <summary>
        /// Gets or sets the zoom factor for the axis.
        /// </summary>
        [JsonPropertyName("zoomFactor")]
        public double ZoomFactor { get; set; }

        /// <summary>
        /// Gets or sets the zoom position for the axis.
        /// </summary>
        [JsonPropertyName("zoomPosition")]
        public double ZoomPosition { get; set; }

        /// <summary>
        /// Gets or sets the scrollbar settings for the axis.
        /// </summary>
        [JsonPropertyName("scrollbarSettings")]
        public ChartCommonScrollbarSettings ScrollbarSettings { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether the axis is visible.
        /// </summary>
        [JsonPropertyName("visible")]
        public bool Visible { get; set; }

        /// <summary>
        /// Gets or sets the maximum point length for the axis.
        /// </summary>
        [JsonPropertyName("maxPointLength")]
        public double MaxPointLength { get; set; }

        /// <summary>
        /// Gets or sets the axis line style width.
        /// </summary>
        [JsonPropertyName("axisLineStyleWidth")]
        public double AxisLineStyleWidth { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether universal date/time is used.
        /// </summary>
        [JsonPropertyName("isUniversalDateTime")]
        public bool IsUniversalDateTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether strip line tooltip is enabled.
        /// </summary>
        [JsonPropertyName("isStripLineTooltip")]
        public bool IsStripLineTooltip { get; set; }
    }

    /// <summary>
    /// Represents options for tooltip rendering args.
    /// </summary>
    public class ITooltipRenderEventArgs
    {
        /// <summary>
        /// Defines the point informations.
        /// </summary>
        [JsonPropertyName("data")]
        public IPointInfo Data { get; set; } = null!;

        /// <summary>
        /// Defines the header text for the tooltip.
        /// </summary>
        [JsonPropertyName("headerText")]
        public string HeaderText { get; set; } = string.Empty;

        /// <summary>
        /// Defines current tooltip point.
        /// </summary>
        [JsonPropertyName("point")]
        public object Point { get; set; } = null!;

        /// <summary>
        /// Defines current tooltip series.
        /// </summary>
        [JsonPropertyName("series")]
        public object Series { get; set; } = null!;

        /// <summary>
        /// Defines tooltip text collections.
        /// </summary>
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents options for point information.
    /// </summary>
    public class IPointInfo
    {
        /// <summary>
        /// Defines the point index.
        /// </summary>
        [JsonPropertyName("pointIndex")]
        public double PointIndex { get; set; }

        /// <summary>
        /// Gets or sets the point text.
        /// </summary>
        [JsonPropertyName("pointText")]
        public string PointText { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the X value of the point.
        /// </summary>
        [JsonPropertyName("pointX")]
        public string PointX { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Y value of the point.
        /// </summary>
        [JsonPropertyName("pointY")]
        public string PointY { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the chart series index.
        /// </summary>
        [JsonPropertyName("seriesIndex")]
        public double SeriesIndex { get; set; }

        /// <summary>
        /// Gets or sets the chart series name.
        /// </summary>
        [JsonPropertyName("seriesName")]
        public string SeriesName { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents options for shared tooltip rendering args.
    /// </summary>
    public class ISharedTooltipRenderEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Defines the tooltip text collection.
        /// </summary>
        [JsonPropertyName("text")]
        public List<string> Text { get; set; } = null!;

        /// <summary>
        /// Defines the text of the header.
        /// </summary>
        [JsonPropertyName("headerText")]
        public string HeaderText { get; set; } = string.Empty;

        /// <summary>
        /// Defines the information of the points.
        /// </summary>
        [JsonPropertyName("data")]
        public List<IPointInfo> Data { get; set; } = null!;
    }

    /// <summary>
    /// Represents options for chart tooltip information.
    /// </summary>
    public class IChartTooltipInfo : IPointInfo
    {
        /// <summary>
        /// Gets or sets the close value for the tooltip.
        /// </summary>
        [JsonPropertyName("close")]
        public string Close { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the high value for the tooltip.
        /// </summary>
        [JsonPropertyName("high")]
        public string High { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the low value for the tooltip.
        /// </summary>
        [JsonPropertyName("low")]
        public string Low { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the open value for the tooltip.
        /// </summary>
        [JsonPropertyName("open")]
        public string Open { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the text for the tooltip.
        /// </summary>
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the volume value for the tooltip.
        /// </summary>
        [JsonPropertyName("volume")]
        public string Volume { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the X value for the tooltip.
        /// </summary>
        [JsonPropertyName("x")]
        public string X { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Y value for the tooltip.
        /// </summary>
        [JsonPropertyName("y")]
        public string Y { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents options for zoom settings.
    /// </summary>
    public class IChartZoomSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether deferred zooming is enabled.
        /// </summary>
        [JsonPropertyName("enableDeferredZooming")]
        public bool EnableDeferredZooming { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether mouse wheel zooming is enabled.
        /// </summary>
        [JsonPropertyName("enableMouseWheelZooming")]
        public bool EnableMouseWheelZooming { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether panning is enabled.
        /// </summary>
        [JsonPropertyName("enablePan")]
        public bool EnablePan { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether pinch zooming is enabled.
        /// </summary>
        [JsonPropertyName("enablePinchZooming")]
        public bool EnablePinchZooming { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether selection zooming is enabled.
        /// </summary>
        [JsonPropertyName("enableSelectionZooming")]
        public bool EnableSelectionZooming { get; set; }

        /// <summary>
        /// Gets or sets the zoom mode.
        /// </summary>
        [JsonPropertyName("mode")]
        public string Mode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the toolbar display mode for zoom.
        /// </summary>
        [JsonPropertyName("toolbarDisplayMode")]
        public string ToolbarDisplayMode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the selection rectangle fill theme style.
        /// </summary>
        [JsonPropertyName("themeStyleSelectionRectFill")]
        public string ThemeStyleSelectionRectFill { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the selection rectangle stroke theme style.
        /// </summary>
        [JsonPropertyName("themeStyleSelectionRectStroke")]
        public string ThemeStyleSelectionRectStroke { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether zoom start event was called.
        /// </summary>
        [JsonPropertyName("isOnZoomStartCalled")]
        public bool IsOnZoomStartCalled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether zoom end event was called.
        /// </summary>
        [JsonPropertyName("isOnZoomEndCalled")]
        public bool IsOnZoomEndCalled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether panning is currently active.
        /// </summary>
        [JsonPropertyName("isPanning")]
        public bool IsPanning { get; set; }
    }

    /// <summary>
    /// Represents options for zooming states.
    /// </summary>
    public class IZoomingStates
    {
        /// <summary>
        /// Gets or sets a value indicating whether panning is currently active.
        /// </summary>
        [JsonPropertyName("isPanning")]
        public bool IsPanning { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the chart is currently zoomed.
        /// </summary>
        [JsonPropertyName("isZoomed")]
        public bool IsZoomed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether wheel zoom is active.
        /// </summary>
        [JsonPropertyName("isWheelZoom")]
        public bool IsWheelZoom { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether UI performed an action.
        /// </summary>
        [JsonPropertyName("performedUI")]
        public bool PerformedUI { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether redraw should be delayed.
        /// </summary>
        [JsonPropertyName("delayRedraw")]
        public bool DelayRedraw { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a double tap event occurred.
        /// </summary>
        [JsonPropertyName("isDoubleTap")]
        public bool IsDoubleTap { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether chart drag is active.
        /// </summary>
        [JsonPropertyName("isChartDrag")]
        public bool IsChartDrag { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether chart panning is active.
        /// </summary>
        [JsonPropertyName("isChartPanning")]
        public bool IsChartPanning { get; set; }
    }

    /// <summary>
    /// Represents options for scrollbar options.
    /// </summary>
    internal class ScrollbarOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether scrollbar exists.
        /// </summary>
        [JsonPropertyName("isScrollExist")]
        public bool IsScrollExist { get; set; }

        /// <summary>
        /// Gets or sets the chart area type.
        /// </summary>
        [JsonPropertyName("chartAreaType")]
        public string ChartAreaType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection of axes.
        /// </summary>
        [JsonPropertyName("axes")]
        public List<IAxis> Axes { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether resize is in progress.
        /// </summary>
        [JsonPropertyName("isResize")]
        public bool IsResize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether lazy load is enabled.
        /// </summary>
        [JsonPropertyName("isLazyLoad")]
        public bool IsLazyLoad { get; set; }

        /// <summary>
        /// Gets or sets the height of the scrollbar.
        /// </summary>
        [JsonPropertyName("height")]
        public double Height { get; set; }

        /// <summary>
        /// Gets or sets the width of the scrollbar.
        /// </summary>
        [JsonPropertyName("width")]
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets the scrollbar theme style.
        /// </summary>
        [JsonPropertyName("scrollbarThemeStyle")]
        public ScrollbarThemeStyle ScrollbarThemeStyle { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether scroll event was called.
        /// </summary>
        [JsonPropertyName("isScrollEventCalled")]
        public bool isScrollEventCalled { get; set; }

        /// <summary>
        /// Gets or sets the chart title position.
        /// </summary>
        [JsonPropertyName("chartTitlePosition")]
        public string ChartTitlePosition { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the chart margin.
        /// </summary>
        [JsonPropertyName("margin")]
        public ChartMargin Margin { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether the legend is visible.
        /// </summary>
        [JsonPropertyName("isLegendVisible")]
        public bool IsLegendVisible { get; set; }

        /// <summary>
        /// Gets or sets the marker height.
        /// </summary>
        [JsonPropertyName("markerHeight")]
        public double MarkerHeight { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether padding is enabled.
        /// </summary>
        [JsonPropertyName("enablePadding")]
        public bool EnablePadding { get; set; }

        /// <summary>
        /// Gets or sets the chart title height.
        /// </summary>
        [JsonPropertyName("chartTitleHeight")]
        public double ChartTitleHeight { get; set; }

        /// <summary>
        /// Gets or sets the chart subtitle height.
        /// </summary>
        [JsonPropertyName("chartSubTitleHeight")]
        public double ChartSubTitleHeight { get; set; }
    }

    /// <summary>
    /// Represents options for scroll event args.
    /// </summary>
    public class IScrollEventsArgs
    {
        /// <summary>
        /// Gets or sets the name of the scroll event.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the axis being scrolled.
        /// </summary>
        [JsonPropertyName("axisName")]
        public string AxisName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the zoom position.
        /// </summary>
        [JsonPropertyName("zoomPosition")]
        public double ZoomPosition { get; set; }

        /// <summary>
        /// Gets or sets the zoom factor.
        /// </summary>
        [JsonPropertyName("zoomFactor")]
        public double ZoomFactor { get; set; }

        /// <summary>
        /// Gets or sets the current range minimum value.
        /// </summary>
        [JsonPropertyName("currentRangeMin")]
        public string CurrentRangeMin { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the current range maximum value.
        /// </summary>
        [JsonPropertyName("currentRangeMax")]
        public string CurrentRangeMax { get; set; } = string.Empty;
    }
}