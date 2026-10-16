using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>
    /// Specifies the chart component highlight mode type.
    /// </summary>
    public enum HighlightMode
    {
        /// <summary>
        /// No highlight mode is applied to the chart component.
        /// </summary>
        None,

        /// <summary>
        /// Highlights the entire series in the chart component.
        /// </summary>
        Series,

        /// <summary>
        /// Highlights individual data points in the chart component.
        /// </summary>
        Point,

        /// <summary>
        /// Highlights a group of data points in the chart component.
        /// </summary>
        Cluster
    }

    /// <summary>
    /// Specifies the highlighting or selecting patterns.
    /// </summary>
    public enum SelectionPattern
    {
        /// <summary>
        /// No pattern is applied for highlighting or selecting.
        /// </summary>
        None,

        /// <summary>
        /// Applies a chessboard pattern for highlighting or selecting.
        /// </summary>
        Chessboard,

        /// <summary>
        /// Applies a dot pattern for highlighting or selecting.
        /// </summary>
        Dots,

        /// <summary>
        /// Applies a diagonal forward line pattern for highlighting or selecting.
        /// </summary>
        DiagonalForward,

        /// <summary>
        /// Applies a crosshatch pattern for highlighting or selecting.
        /// </summary>
        Crosshatch,

        /// <summary>
        /// Applies a pacman pattern for highlighting or selecting.
        /// </summary>
        Pacman,

        /// <summary>
        /// Applies a diagonal backward line pattern for highlighting or selecting.
        /// </summary>
        DiagonalBackward,

        /// <summary>
        /// Applies a grid pattern for highlighting or selecting.
        /// </summary>
        Grid,

        /// <summary>
        /// Applies a turquoise pattern for highlighting or selecting.
        /// </summary>
        Turquoise,

        /// <summary>
        /// Applies a star pattern for highlighting or selecting.
        /// </summary>
        Star,

        /// <summary>
        /// Applies a triangle pattern for highlighting or selecting.
        /// </summary>
        Triangle,

        /// <summary>
        /// Applies a circle pattern for highlighting or selecting.
        /// </summary>
        Circle,

        /// <summary>
        /// Applies a tile pattern for highlighting or selecting.
        /// </summary>
        Tile,

        /// <summary>
        /// Applies a horizontal dash pattern for highlighting or selecting.
        /// </summary>
        HorizontalDash,

        /// <summary>
        /// Applies a vertical dash pattern for highlighting or selecting.
        /// </summary>
        VerticalDash,

        /// <summary>
        /// Applies a rectangle pattern for highlighting or selecting.
        /// </summary>
        Rectangle,

        /// <summary>
        /// Applies a box pattern for highlighting or selecting.
        /// </summary>
        Box,

        /// <summary>
        /// Applies a vertical stripe pattern for highlighting or selecting.
        /// </summary>
        VerticalStripe,

        /// <summary>
        /// Applies a horizontal stripe pattern for highlighting or selecting.
        /// </summary>
        HorizontalStripe,

        /// <summary>
        /// Applies a bubble pattern for highlighting or selecting.
        /// </summary>
        Bubble
    }

    /// <summary>
    /// Specifies the selection mode.
    /// </summary>
    public enum ChartSelectionMode
    {
        /// <summary>
        /// Disables the selection functionality.
        /// </summary>
        None,

        /// <summary>
        /// Enables selection of series in the chart.
        /// </summary>
        Series,

        /// <summary>
        /// Enables selection of individual points in the chart.
        /// </summary>
        Point,

        /// <summary>
        /// Enables selection of clusters of points in the chart.
        /// </summary>
        Cluster,

        /// <summary>
        /// Enables selection of points by dragging with respect to both axes.
        /// </summary>
        DragXY,

        /// <summary>
        /// Enables selection of points by dragging with respect to the horizontal axis.
        /// </summary>
        DragY,

        /// <summary>
        /// Enables selection of points by dragging with respect to the vertical axis.
        /// </summary>
        DragX,

        /// <summary>
        /// Enables free-form selection of points by lasso.
        /// </summary>
        Lasso
    }

    /// <summary>
    /// Specifies the segment axis.
    /// </summary>
    public enum Segment
    {
        /// <summary>
        /// Segments rendered based on the horizontal axis.
        /// </summary>
        X,

        /// <summary>
        /// Segments rendered based on the vertical axis.
        /// </summary>
        Y
    }

    /// <summary>
    /// Specifies the type of spline.
    /// </summary>
    public enum SplineType
    {
        /// <summary>
        /// Renders a natural spline.
        /// </summary>
        Natural,

        /// <summary>
        /// Renders a monotonic spline.
        /// </summary>
        Monotonic,

        /// <summary>
        /// Renders a cardinal spline.
        /// </summary>
        Cardinal,

        /// <summary>
        /// Renders a clamped spline.
        /// </summary>
        Clamped
    }

    /// <summary> 
    /// Specifies the position for the steps in the stepline, steparea, and steprange area chart types. 
    /// </summary> 

    public enum StepPosition
    {
        /// <summary> 
        /// Steps start from the left side of the second point. 
        /// </summary> 
        /// <value>Left</value> 
        Left = 0,

        /// <summary> 
        /// Steps start from the right side of the first point. 
        /// </summary> 
        /// <value>Right</value> 
        Right = 1,

        /// <summary> 
        /// Steps start between the data points. 
        /// </summary> 
        /// <value>Center</value> 
        Center = 2,
    }

    /// <summary>
    /// Specifies the type series in chart.
    /// </summary>
    public enum ChartSeriesType
    {
        /// <summary>
        /// Renders a line series.
        /// </summary>
        Line,

        /// <summary>
        /// Renders a column series.
        /// </summary>
        Column,

        /// <summary>
        /// Renders an area series.
        /// </summary>
        Area,

        /// <summary>
        /// Renders a bar series.
        /// </summary>
        Bar,

        /// <summary>
        /// Renders a stacking column series.
        /// </summary>
        StackingColumn,

        /// <summary>
        /// Renders a stacking area series.
        /// </summary>
        StackingArea,

        /// <summary>
        /// Renders a stacking line series.
        /// </summary>
        StackingLine,

        /// <summary>
        /// Renders a stacking bar series.
        /// </summary>
        StackingBar,

        /// <summary>
        /// Renders a stacking step area series.
        /// </summary>
        StackingStepArea,

        /// <summary>
        /// Renders a step line series.
        /// </summary>
        StepLine,

        /// <summary>
        /// Renders a step area series.
        /// </summary>
        StepArea,

        /// <summary>
        /// Renders a spline area series.
        /// </summary>
        SplineArea,

        /// <summary>
        /// Renders a scatter series.
        /// </summary>
        Scatter,

        /// <summary>
        /// Renders a spline series.
        /// </summary>
        Spline,

        /// <summary>
        /// Renders a stacking column 100 percent series.
        /// </summary>
        StackingColumn100,

        /// <summary>
        /// Renders a stacking bar 100 percent series.
        /// </summary>
        StackingBar100,

        /// <summary>
        /// Renders a stacking line 100 percent series.
        /// </summary>
        StackingLine100,

        /// <summary>
        /// Renders a stacking area 100 percent series.
        /// </summary>
        StackingArea100,

        /// <summary>
        /// Renders a bubble series.
        /// </summary>
        Bubble,

        /// <summary>
        /// Renders a multicolored line series.
        /// </summary>
        MultiColoredLine,

        /// <summary>
        /// Renders a multicolored area series.
        /// </summary>
        MultiColoredArea
    }

    /// <summary>
    /// Specifies the type of trendlines.
    /// </summary>
    public enum TrendlineTypes
    {
        /// <summary>
        /// Renders a linear trendline.
        /// </summary>
        Linear,

        /// <summary>
        /// Renders an exponential trendline.
        /// </summary>
        Exponential,

        /// <summary>
        /// Renders a polynomial trendline.
        /// </summary>
        Polynomial,

        /// <summary>
        /// Renders a power trendline.
        /// </summary>
        Power,

        /// <summary>
        /// Renders a logarithmic trendline.
        /// </summary>
        Logarithmic,

        /// <summary>
        /// Renders a moving average trendline.
        /// </summary>
        MovingAverage
    }

    /// <summary>
    /// Specifies the shape of marker.
    /// </summary>
    public enum ChartShape
    {
        /// <summary>
        /// Specifies the shape of the marker as a circle symbol.
        /// </summary>
        /// <value>Circle</value>
        Circle = 0,

        /// <summary>
        /// Specifies the shape of the marker as a triangle symbol.
        /// </summary>
        /// <value>Triangle</value>
        Triangle = 1,

        /// <summary>
        /// Specifies the shape of the marker as a diamond symbol.
        /// </summary>
        /// <value>Diamond</value>
        Diamond = 2,

        /// <summary>
        /// Specifies the shape of the marker as a rectangle symbol.
        /// </summary>
        /// <value>Rectangle</value>
        Rectangle = 3,

        /// <summary>
        /// Specifies the shape of the marker as a pentagon symbol.
        /// </summary>
        /// <value>Pentagon</value>
        Pentagon = 4,

        /// <summary>
        /// Specifies the shape of the marker as an inverted triangle symbol.
        /// </summary>
        /// <value>InvertedTriangle</value>
        InvertedTriangle = 5,

        /// <summary>
        /// Specifies the shape of the marker as a vertical line symbol.
        /// </summary>
        /// <value>VerticalLine</value>
        VerticalLine = 6,

        /// <summary>
        /// Specifies the shape of the marker as a cross symbol.
        /// </summary>
        /// <value>Cross</value>
        Cross = 7,

        /// <summary> 
        /// Specifies the shape of the marker as a plus symbol. 
        /// </summary> 
        /// <value>Plus</value>
        Plus = 8,

        /// <summary>
        /// Specifies the shape of the marker as a horizontal line symbol.
        /// </summary>
        /// <value>HorizontalLine</value>
        HorizontalLine = 9,

        /// <summary>
        /// Specifies the shape of the marker as an image.
        /// </summary>
        /// <value>Image</value>
        Image = 10,

        /// <summary>
        /// Specifies the shape of the marker as auto.
        /// </summary>
        /// <value>Auto</value>
        Auto = 11
    }

    /// <summary>
    /// Specifies the label position.
    /// </summary>
    public enum ChartLabelPosition
    {
        /// <summary>
        /// Label is positioned on the outside of the data point.
        /// </summary>
        Outer,

        /// <summary>
        /// Label is positioned on top of the data point.
        /// </summary>
        Top,

        /// <summary>
        /// Label is positioned at the bottom of the data point.
        /// </summary>
        Bottom,

        /// <summary>
        /// Label is positioned in the middle of the data point.
        /// </summary>
        Middle,

        /// <summary>
        /// Label position is automatically based on the series.
        /// </summary>
        Auto
    }

    /// <summary>
    /// Specifies the possible positions for a scrollbar in a chart.
    /// 
    /// Available options:
    /// <see cref="PlaceNextToAxisLine"/> – Default. Positions the scrollbar next to the axis line.
    /// <see cref="Top"/> – Positions the scrollbar at the top of the chart (horizontal only).
    /// <see cref="Bottom"/> – Positions the scrollbar at the bottom of the chart (horizontal only).
    /// <see cref="Left"/> – Positions the scrollbar on the left side of the chart (vertical only).
    /// <see cref="Right"/> – Positions the scrollbar on the right side of the chart (vertical only).
    /// </summary>
    public enum ScrollbarPosition
    {
        /// <summary>
        /// Positions the scrollbar next to the axis line. This is the default setting.
        /// </summary>
        PlaceNextToAxisLine,

        /// <summary>
        /// Positions the scrollbar at the top of the chart. Applicable only to horizontal scrollbars.
        /// </summary>
        Top,

        /// <summary>
        /// Positions the scrollbar at the bottom of the chart. Applicable only to horizontal scrollbars.
        /// </summary>
        Bottom,

        /// <summary>
        /// Positions the scrollbar on the left side of the chart. Applicable only to vertical scrollbars.
        /// </summary>
        Left,

        /// <summary>
        /// Positions the scrollbar on the right side of the chart. Applicable only to vertical scrollbars.
        /// </summary>
        Right
    }

    /// <summary>
    /// Specifies the edge Label Placement for an axis.
    /// </summary>
    public enum EdgeLabelPlacement
    {
        /// <summary>
        /// No action will be performed on the edge labels.
        /// </summary>
        None,

        /// <summary>
        /// Edge labels will be hidden.
        /// </summary>
        Hide,

        /// <summary>
        /// Shifts the edge labels.
        /// </summary>
        Shift
    }

    /// <summary>
    /// Specifies the interval type of datetime axis.
    /// </summary>
    public enum IntervalType
    {
        /// <summary>
        /// Interval of the axis is determined based on data.
        /// </summary>
        Auto,

        /// <summary>
        /// Interval of the axis is in years.
        /// </summary>
        Years,

        /// <summary>
        /// Interval of the axis is in months.
        /// </summary>
        Months,

        /// <summary>
        /// Interval of the axis is in days.
        /// </summary>
        Days,

        /// <summary>
        /// Interval of the axis is in hours.
        /// </summary>
        Hours,

        /// <summary>
        /// Interval of the axis is in minutes.
        /// </summary>
        Minutes,

        /// <summary>
        /// Interval of the axis is in seconds.
        /// </summary>
        Seconds
    }

    /// <summary>
    /// Specifies the alignment.
    /// </summary>
    public enum LabelIntersectAction
    {
        /// <summary>
        /// Shows all the labels without any action.
        /// </summary>
        None,

        /// <summary>
        /// Hides the label when it intersects.
        /// </summary>
        Hide,

        /// <summary>
        /// Trims the label when it intersects.
        /// </summary>
        Trim,

        /// <summary>
        /// Wraps the label when it intersects.
        /// </summary>
        Wrap,

        /// <summary>
        /// Sets the label in multiple rows when it intersects.
        /// </summary>
        MultipleRows,

        /// <summary>
        /// Rotates the label at a 45-degree angle when it intersects.
        /// </summary>
        Rotate45,

        /// <summary>
        /// Rotates the label at a 90-degree angle when it intersects.
        /// </summary>
        Rotate90
    }

    /// <summary>
    /// Specifies the label placement for category axis.
    /// </summary>
    public enum LabelPlacement
    {
        /// <summary>
        /// Render the label between the ticks.
        /// </summary>
        BetweenTicks,

        /// <summary>
        /// Render the label on the ticks.
        /// </summary>
        OnTicks
    }

    /// <summary>
    /// Specifies the position.
    /// </summary>
    public enum AxisPosition
    {
        /// <summary>
        /// Ticks or labels are inside the axis line.
        /// </summary>
        Inside,

        /// <summary>
        /// Ticks or labels are outside the axis line.
        /// </summary>
        Outside
    }

    /// <summary>
    /// Specifies the range padding of axis.
    /// </summary>
    public enum ChartRangePadding
    {
        /// <summary>
        /// Automatic padding is applied to the axis.
        /// </summary>
        Auto,

        /// <summary>
        /// Padding is not applied to the axis.
        /// </summary>
        None,

        /// <summary>
        /// Padding is applied to the axis based on the range calculation.
        /// </summary>
        Normal,

        /// <summary>
        /// Interval of the axis is added as padding to the min and max values of the range.
        /// </summary>
        Additional,

        /// <summary>
        /// Axis range is rounded to the nearest possible value divided by the interval.
        /// </summary>
        Round
    }

    /// <summary>
    /// Specifies the type of axis.
    /// </summary>
    public enum ValueType
    {
        /// <summary>
        /// Renders a numeric axis.
        /// </summary>
        Double,

        /// <summary>
        /// Renders a datetime axis.
        /// </summary>
        DateTime,

        /// <summary>
        /// Renders a category axis.
        /// </summary>
        Category,

        /// <summary>
        /// Renders a logarithmic axis.
        /// </summary>
        Logarithmic,

        /// <summary>
        /// Renders a datetime category axis.
        /// </summary>
        DateTimeCategory
    }

    /// <summary>
    /// Specifies the strip line text position.
    /// </summary>
    public enum Anchor
    {
        /// <summary>
        /// Strip line text is at the start.
        /// </summary>
        Start,

        /// <summary>
        /// Strip line text is in the middle.
        /// </summary>
        Middle,

        /// <summary>
        /// Strip line text is at the end.
        /// </summary>
        End
    }

    /// <summary>
    /// Specifies the order of the strip line.
    /// </summary>
    public enum ZIndexPosition
    {
        /// <summary>
        /// Defines the strip line over the series elements.
        /// </summary>
        Over,

        /// <summary>
        /// Defines the strip line behind the series elements.
        /// </summary>
        Behind
    }

    /// <summary>
    /// Specifies border type for multi-level labels.
    /// </summary>
    public enum BorderType
    {
        /// <summary>
        /// Rectangle border type.
        /// </summary>
        Rectangle,

        /// <summary>
        /// Brace border type.
        /// </summary>
        Brace,

        /// <summary>
        /// No border type.
        /// </summary>
        WithoutBorder,

        /// <summary>
        /// No top border type.
        /// </summary>
        WithoutTopBorder,

        /// <summary>
        /// No top and bottom border type.
        /// </summary>
        WithoutTopandBottomBorder,

        /// <summary>
        /// Curly brace border type.
        /// </summary>
        CurlyBrace,

        /// <summary>
        /// Auto border type.
        /// </summary>
        Auto
    }

    /// <summary>
    /// Specifies coordinate units of an annotation.
    /// </summary>
    public enum Units
    {
        /// <summary>
        /// Defines pixel units.
        /// </summary>
        Pixel,

        /// <summary>
        /// Defines point units.
        /// </summary>
        Point
    }

    /// <summary>
    /// Specifies regions of an annotation.
    /// </summary>
    public enum Regions
    {
        /// <summary>
        /// Defines chart region.
        /// </summary>
        Chart,

        /// <summary>
        /// Defines series region.
        /// </summary>
        Series
    }

    /// <summary>
    /// Specifies the mode of line in crosshair.
    /// </summary>
    public enum LineType
    {
        /// <summary>
        /// Hides both vertical and horizontal crosshair lines.
        /// </summary>
        None,

        /// <summary>
        /// Shows both vertical and horizontal crosshair lines.
        /// </summary>
        Both,

        /// <summary>
        /// Shows the vertical line only.
        /// </summary>
        Vertical,

        /// <summary>
        /// Shows the horizontal line only.
        /// </summary>
        Horizontal
    }

    /// <summary>
    /// Specifies the zooming mode.
    /// </summary>
    public enum ZoomMode
    {
        /// <summary>
        /// Zooms regarding both vertical and horizontal axes.
        /// </summary>
        XY,

        /// <summary>
        /// Zooms with respect to the horizontal axis.
        /// </summary>
        X,

        /// <summary>
        /// Zooms with respect to the vertical axis.
        /// </summary>
        Y
    }

    /// <summary>
    /// Specifies the interval type of datetime axis.
    /// </summary>
    public enum RangeIntervalType
    {
        /// <summary>
        /// Defines the interval of the axis automatically based on data.
        /// </summary>
        Auto,

        /// <summary>
        /// Defines the interval of the axis in years.
        /// </summary>
        Years,

        /// <summary>
        /// Defines the interval of the axis based on quarters.
        /// </summary>
        Quarter,

        /// <summary>
        /// Defines the interval of the axis in months.
        /// </summary>
        Months,

        /// <summary>
        /// Defines the interval of the axis in weeks.
        /// </summary>
        Weeks,

        /// <summary>
        /// Defines the interval of the axis in days.
        /// </summary>
        Days,

        /// <summary>
        /// Defines the interval of the axis in hours.
        /// </summary>
        Hours,

        /// <summary>
        /// Defines the interval of the axis in minutes.
        /// </summary>
        Minutes,

        /// <summary>
        /// Defines the interval of the axis in seconds.
        /// </summary>
        Seconds
    }

    /// <summary>
    /// Specifies the empty point mode of the chart.
    /// </summary>
    public enum EmptyPointMode
    {
        /// <summary>
        /// Displays empty points as a gap.
        /// </summary>
        Gap,

        /// <summary>
        /// Displays empty points as zero.
        /// </summary>
        Zero,

        /// <summary>
        /// Ignores empty points while rendering.
        /// </summary>
        Drop,

        /// <summary>
        /// Displays empty points as an average of previous and next points.
        /// </summary>
        Average
    }

    /// <summary>
    /// Specifies the series type of chart.
    /// </summary>
    public enum SeriesValueType
    {
        /// <summary>
        /// Defines the xy series type of chart.
        /// </summary>
        XY,

        /// <summary>
        /// Defines the high low series type of chart.
        /// </summary>
        HighLow,

        /// <summary>
        /// Defines the high low open close series type of chart.
        /// </summary>
        HighLowOpenClose,

        /// <summary>
        /// Defines the box plot series type of chart.
        /// </summary>
        BoxPlot
    }

    /// <summary>
    /// Specifies the zooming toolkit types.
    /// </summary>
    public enum ToolbarItems
    {
        /// <summary>
        /// Defines the zoom button.
        /// </summary>
        Zoom,

        /// <summary>
        /// Defines the zoom in button.
        /// </summary>
        ZoomIn,

        /// <summary>
        /// Defines the zoom out button.
        /// </summary>
        ZoomOut,

        /// <summary>
        /// Defines the pan button.
        /// </summary>
        Pan,

        /// <summary>
        /// Defines the reset button.
        /// </summary>
        Reset
    }

    /// <summary>
    /// Specifies the series categories type.
    /// </summary>
    public enum SeriesCategories
    {
        /// <summary>
        /// Defines the trend line type for series categories.
        /// </summary>
        TrendLine,

        /// <summary>
        /// Defines the indicator type for series categories.
        /// </summary>
        Indicator,

        /// <summary>
        /// Defines the series type for series categories.
        /// </summary>
        Series,

        /// <summary>
        /// Defines the pareto type for series categories.
        /// </summary>
        Pareto
    }

    /// <summary>
    /// Specifies the visibility mode for zooming toolbar items.
    /// </summary>
    public enum ToolbarMode
    {
        /// <summary>
        /// Zooming toolbar items are visible only while the chart is zoomed.
        /// </summary>
        /// <value>OnDemand</value>
        OnDemand = 0,

        /// <summary>
        /// Zooming toolbar items are always visible.
        /// </summary>
        /// <value>Always</value> 
        Always = 1,

        /// <summary>
        /// Zooming toolbar items are not visible even when chart is zoomed.
        /// </summary>
        /// <value>None</value> 
        None = 2
    }

    /// <summary>
    /// Specifies the chart width category for adaptive layout rendering.
    /// </summary>
    internal enum ChartWidthCategory
    {
        Small = 0,
        Medium = 1,
        Large = 2,
        Normal = 3
    }

    /// <summary>
    /// Specifies the chart height category for adaptive layout rendering.
    /// </summary>
    internal enum ChartHeightCategory
    {
        Small = 0,
        Medium = 1,
        Large = 2,
        Normal = 3
    }

    /// <summary>
    /// Specifies the position of the title for the <see cref="SfChart">Chart</see>.
    /// </summary>
    public enum ChartTitlePosition
    {
        /// <summary> 
        /// Displays the title and subtitle at the top of the chart. 
        /// </summary> 
        Top,

        /// <summary>
        /// Displays the title and subtitle at the right of the chart. 
        /// </summary>
        Right,

        /// <summary>
        /// Displays the title and subtitle at the bottom of the chart.
        /// </summary>
        Bottom,

        /// <summary>
        /// Displays the title and subtitle at the left of the chart. 
        /// </summary>
        Left,

        /// <summary>
        /// Displays the title and subtitle based on the specified X and Y coordinates.
        /// </summary>
        Custom
    }

    /// <summary>
    /// Specifies the options for horizontal alignment of the toolbar.
    /// </summary>
    public enum HorizontalAlign
    {
        /// <summary>
        /// Aligns the toolbar to the left side of the chart.
        /// </summary>
        Left,

        /// <summary>
        /// Centers the toolbar horizontally within the chart.
        /// </summary>
        Center,

        /// <summary>
        /// Aligns the toolbar to the right side of the chart.
        /// </summary>
        Right
    }

    /// <summary>
    /// Specifies the vertical position options for the toolbar.
    /// </summary>
    public enum VerticalAlign
    {
        /// <summary>
        /// Positions the toolbar at the top of the chart.
        /// </summary>
        Top,

        /// <summary>
        /// Vertically centers the toolbar within the chart.
        /// </summary>
        Middle,

        /// <summary>
        /// Positions the toolbar at the bottom of the chart.
        /// </summary>
        Bottom
    }

    /// <summary>
    /// Defines the position of the legend in the chart.
    /// </summary>
    /// <remarks>
    /// The legend can be positioned automatically based on component dimensions or placed at a specific location.
    /// </remarks>
    public enum LegendPosition
    {
        /// <summary>
        /// The legend position is automatically determined based on the chart's width and height.
        /// </summary>
        Auto,

        /// <summary>
        /// The legend is positioned below the chart content area.
        /// </summary>
        Bottom,

        /// <summary>
        /// The legend is positioned above the chart content area.
        /// </summary>
        Top,

        /// <summary>
        /// The legend is positioned to the left of the chart content area.
        /// </summary>
        Left,

        /// <summary>
        /// The legend is positioned to the right of the chart content area.
        /// </summary>
        Right,

        /// <summary>
        /// The legend is positioned at custom X and Y coordinates specified in the legend settings.
        /// </summary>
        Custom
    }

    /// <summary>
    /// Defines how text content overflows its container bounds in chart titles and labels.
    /// </summary>
    /// <remarks>
    /// Controls the behavior when text exceeds the available space in titles, axis labels, or legend items.
    /// </remarks>
    public enum TextOverflow
    {
        /// <summary>
        /// Text is displayed as-is without any overflow handling.
        /// </summary>
        None,

        /// <summary>
        /// Text is truncated and an ellipsis ("...") is appended if it exceeds the container margins.
        /// </summary>
        Trim,

        /// <summary>
        /// Text is wrapped to multiple lines if it exceeds the container margins.
        /// </summary>
        Wrap
    }

    // /// <summary> 
    // /// Specifies text overflow options when text overflows its container. 
    // /// </summary> 
    // public enum LabelOverflow
    // {
    //     /// <summary> 
    //     /// Appends an ellipsis (...) to clipped text.
    //     /// </summary>     
    //     Ellipse,

    //     /// <summary> 
    //     /// Clips the text without appending any indicator.
    //     /// </summary> 
    //     Clip
    // }

    /// <summary>
    /// Defines the visual shape used to represent legend items in the chart.
    /// </summary>
    /// <remarks>
    /// The shape appears next to the series name in the legend, aiding visual identification of data series.
    /// For line-based series, use line shapes; for area/bar series, use rectangular shapes.
    /// </remarks>
    public enum LegendShape
    {
        /// <summary>
        /// Legend item is displayed as a filled circle.
        /// </summary>
        Circle,

        /// <summary>
        /// Legend item is displayed as a filled rectangle.
        /// </summary>
        Rectangle,

        /// <summary>
        /// Legend item is displayed as a filled upright triangle.
        /// </summary>
        Triangle,

        /// <summary>
        /// Legend item is displayed as a filled diamond shape.
        /// </summary>
        Diamond,

        /// <summary>
        /// Legend item is displayed as a cross (+) symbol.
        /// </summary>
        Cross,

        /// <summary>
        /// Legend item is displayed as a multiply (×) symbol.
        /// </summary>
        Multiply,

        /// <summary>
        /// Legend item is displayed as a rectangle matching the series actual dimensions.
        /// </summary>
        ActualRect,

        /// <summary>
        /// Legend item is displayed as a target-style rectangle.
        /// </summary>
        TargetRect,

        /// <summary>
        /// Legend item is displayed as a horizontal line.
        /// </summary>
        HorizontalLine,

        /// <summary>
        /// Legend item is displayed as a vertical line.
        /// </summary>
        VerticalLine,

        /// <summary>
        /// Legend item is displayed as a filled pentagon.
        /// </summary>
        Pentagon,

        /// <summary>
        /// Legend item is displayed as a filled inverted (downward-pointing) triangle.
        /// </summary>
        InvertedTriangle,

        /// <summary>
        /// Legend item shape is automatically determined by the series type.
        /// </summary>
        SeriesType
    }


    /// <summary>
    /// Specifies marker shapes supported inside tooltip visual elements.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TooltipShape
    {
        /// <summary>
        /// Renders a circular marker.
        /// </summary>
        [EnumMember(Value = "Circle")]
        Circle,
        /// <summary>
        /// Renders a rectangular marker.
        /// </summary>
        [EnumMember(Value = "Rectangle")]
        Rectangle,
        /// <summary>
        /// Renders a triangular marker.
        /// </summary>
        [EnumMember(Value = "Triangle")]
        Triangle,
        /// <summary>
        /// Renders a diamond-shaped marker.
        /// </summary>
        [EnumMember(Value = "Diamond")]
        Diamond,
        /// <summary>
        /// Renders a cross-shaped marker.
        /// </summary>
        [EnumMember(Value = "Cross")]
        Cross,
        /// <summary>
        /// Renders a horizontal line marker.
        /// </summary>
        [EnumMember(Value = "HorizontalLine")]
        HorizontalLine,
        /// <summary>
        /// Renders a vertical line marker.
        /// </summary>
        [EnumMember(Value = "VerticalLine")]
        VerticalLine,
        /// <summary>
        /// Renders a pentagonal marker.
        /// </summary>
        [EnumMember(Value = "Pentagon")]
        Pentagon,
        /// <summary>
        /// Renders an inverted triangular marker.
        /// </summary>
        [EnumMember(Value = "InvertedTriangle")]
        InvertedTriangle,
        /// <summary>
        /// Renders an image-based marker.
        /// </summary>
        [EnumMember(Value = "Image")]
        Image,
        /// <summary>
        /// Disables the marker.
        /// </summary>
        [EnumMember(Value = "None")]
        None
    }
}
