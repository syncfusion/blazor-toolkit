using Syncfusion.Blazor.Toolkit.Buttons;
using Syncfusion.Blazor.Toolkit.Charts;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Syncfusion.Blazor.Toolkit.Popups;

namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>
    /// Specifies the global animation behavior applied to Blazor Toolkit components.
    /// </summary>
    public enum GlobalAnimationMode
    {
        /// <summary>
        /// Use the default animation behavior (no override). The toolkit or application-level setting will determine
        /// whether animations are enabled.
        /// </summary>
        Default,

        /// <summary>
        /// Force animations to be enabled for components that support animation effects.
        /// </summary>
        Enable,

        /// <summary>
        /// Force animations to be disabled for components that support animation effects.
        /// </summary>
        Disable
    }


    /// <summary> 
    /// Specifies text overflow options when the text overflowing the container. 
    /// </summary> 
    public enum LabelOverflow
    {
        /// <summary> 
        /// Specifies an ellipsis (�...�) to the clipped text. 
        /// </summary>     
        Ellipse,

        /// <summary> 
        /// Specifies the text is clipped and not accessible. 
        /// </summary> 
        Clip
    }

    /// <summary> 
    /// Specifies text wrap options when the text overflowing the container. 
    /// </summary> 
    public enum TextWrap
    {
        /// <summary> 
        /// Specifies to break words only at allowed break points. 
        /// </summary> 
        Normal,

        /// <summary> 
        /// Specifies to break a word once it is too long to fit on a line by itself. 
        /// </summary>     
        Wrap,

        /// <summary> 
        /// Specifies to break a word at any point if there are no otherwise-acceptable break points in the line. 
        /// </summary> 
        AnyWhere
    }

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
    /// Specifies the unit of strip line size.
    /// </summary>
    public enum SizeType
    {
        /// <summary>
        /// Defines auto type.
        /// </summary>
        Auto,

        /// <summary>
        /// Defines pixel type.
        /// </summary>
        Pixel,

        /// <summary>
        /// Defines years type.
        /// </summary>
        Years,

        /// <summary>
        /// Defines months type.
        /// </summary>
        Months,

        /// <summary>
        /// Defines days type.
        /// </summary>
        Days,

        /// <summary>
        /// Defines hours type.
        /// </summary>
        Hours,

        /// <summary>
        /// Defines minutes type.
        /// </summary>
        Minutes,

        /// <summary>
        /// Defines seconds type.
        /// </summary>
        Seconds
    }

    /// <summary>
    /// Specifies the order of the strip line.
    /// </summary>
    public enum ZIndex
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
    /// Specifies the area type of chart.
    /// </summary>
    public enum ChartAreaType
    {
        /// <summary>
        /// Defines the cartesian axes area type.
        /// </summary>
        CartesianAxes
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
    /// Specifies the orientation of chart axis.
    /// </summary>
    public enum Orientation
    {
        /// <summary>
        /// Defines the null orientation.
        /// </summary>
        Null,

        /// <summary>
        /// Defines the horizontal orientation.
        /// </summary>
        Horizontal,

        /// <summary>
        /// Defines the vertical orientation.
        /// </summary>
        Vertical
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
    /// Specifies the type of animation.
    /// </summary>
    public enum AnimationType
    {
        /// <summary>
        /// Defines the progressive animation type.
        /// </summary>
        Progressive,

        /// <summary>
        /// Defines the linear animation type.
        /// </summary>
        Linear,

        /// <summary>
        /// Defines the rect animation type.
        /// </summary>
        Rect,

        /// <summary>
        /// Defines the marker animation type.
        /// </summary>
        Marker
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
    /// Defines the visual style and color scheme of the chart.
    /// </summary>
    /// <remarks>
    /// The theme affects the chart's background color, text color, grid lines, axis labels, series colors, and legend appearance.
    /// </remarks>
    /// <example>
    /// The following example demonstrates how to apply a theme to the chart.
    /// <code>
    /// <![CDATA[
    /// <SfChart Title="Sales Data" Theme="Theme.Fluent">
    ///     <ChartSeriesCollection>
    ///         <ChartSeries DataSource="@SalesData" XName="Month" YName="Revenue" Type="ChartSeriesType.Column" />
    ///     </ChartSeriesCollection>
    /// </SfChart>
    /// ]]>
    /// </code>
    /// </example>
    public enum Theme
    {
        /// <summary>
        /// Applies the Fluent light theme to the chart, rendering with a light background, dark text, and neutral accent colors.
        /// </summary>
        /// <remarks>
        /// The Fluent light theme is ideal for applications that follow a light mode interface.
        /// </remarks>
        Fluent,
        /// <summary>
        /// Applies the Fluent dark theme to the chart, rendering with a dark background, light text, and adjusted accent colors for visibility.
        /// </summary>
        /// <remarks>
        /// The Fluent dark theme is designed for dark mode interfaces.
        /// </remarks>
        FluentDark
    }

    /// <summary> 
    /// Specifies the calendar system to be used in calendar components. 
    /// </summary> 
    /// <remarks> 
    /// The <see cref="CalendarType"/> enum allows selecting between different calendar systems such as Gregorian and Islamic (Hijri).  
    /// It determines how dates are calculated, displayed, and handled in components such as <c>SfCalendar</c>, <c>SfDatePicker</c>, and <c>SfDateTimePicker</c>.
    /// The calendar type affects date calculations, month names, and cultural formatting.
    /// </remarks>
    /// <example>
    /// Setting the calendar type to Islamic:
    /// <code><![CDATA[
    /// <SfCalendar TValue="DateTime" CalendarMode="CalendarType.Islamic"></SfCalendar>
    /// ]]></code>
    /// </example>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CalendarType
    {
        /// <summary>
        /// Represents the Gregorian calendar system, which is the internationally accepted civil calendar.
        /// </summary>
        /// <remarks>
        /// This is the default calendar type used worldwide. The Gregorian calendar has 12 months with varying day counts,
        /// and includes leap years every four years (with some exceptions). It starts from January 1st as the new year.
        /// </remarks>
        [EnumMember(Value = "Gregorian")]
        Gregorian,

        /// <summary>
        /// Represents the Islamic (Hijri) calendar system used in Islamic cultures.
        /// </summary>
        /// <remarks>
        /// The Islamic calendar is a lunar calendar consisting of 12 months with approximately 354 or 355 days in a year.
        /// It starts from the year of Prophet Muhammad's migration to Medina (622 CE in the Gregorian calendar).
        /// Each month begins with the sighting of the new moon.
        /// </remarks>
        [EnumMember(Value = "Islamic")]
        Islamic,
    }

    /// <summary>
    /// Specifies the display format for day names in the calendar header.
    /// </summary>
    /// <remarks>
    /// The <see cref="DayHeaderFormats"/> enum controls how day names are displayed in the header row of calendar components.
    /// This affects the visual appearance and space utilization of the calendar, allowing customization based on available space and user preferences.
    /// Different formats provide varying levels of detail for day identification.
    /// </remarks>
    /// <example>
    /// Setting the day header format:
    /// <code><![CDATA[
    /// <SfCalendar TValue="DateTime" DayHeaderFormat="DayHeaderFormats.Abbreviated"></SfCalendar>
    /// ]]></code>
    /// </example>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DayHeaderFormats
    {
        /// <summary>
        /// Displays day names in short format, typically showing two characters (e.g., "Su", "Mo", "Tu").
        /// </summary>
        /// <remarks>
        /// This format provides a compact representation while still being easily recognizable.
        /// It's suitable for calendars where space is moderately constrained but readability remains important.
        /// </remarks>
        [EnumMember(Value = "Short")]
        Short,

        /// <summary>
        /// Displays day names as single characters (e.g., "S", "M", "T").
        /// </summary>
        /// <remarks>
        /// This is the most compact format, using only the first letter of each day name.
        /// It's ideal for mobile interfaces or very small calendar displays where space is at a premium.
        /// Note that some days may share the same initial letter in certain locales.
        /// </remarks>
        [EnumMember(Value = "Narrow")]
        Narrow,

        /// <summary>
        /// Displays day names in abbreviated format, typically showing three characters (e.g., "Sun", "Mon", "Tue").
        /// </summary>
        /// <remarks>
        /// This format provides a good balance between space efficiency and clarity.
        /// It's commonly used in many calendar applications as it's easily readable while not taking up excessive space.
        /// </remarks>
        [EnumMember(Value = "Abbreviated")]
        Abbreviated,

        /// <summary>
        /// Displays day names in full format, showing complete day names (e.g., "Sunday", "Monday", "Tuesday").
        /// </summary>
        /// <remarks>
        /// This format provides the clearest representation of day names but requires the most space.
        /// It's suitable for large calendar displays where readability is prioritized over space conservation.
        /// </remarks>
        [EnumMember(Value = "Wide")]
        Wide,
    }

    /// <summary>
    /// Defines the types of target elements for positioning popup components.
    /// </summary>
    /// <remarks>
    /// The <see cref="TargetType"/> enumeration specifies how the popup element should be positioned relative to its target element.
    /// This affects the positioning behavior and reference point used by the popup component.
    /// </remarks>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TargetType
    {
        /// <summary>
        /// Specifies that the popup should be positioned relative to the target element.
        /// </summary>
        /// <remarks>
        /// When set to Relative, the popup position is calculated based on the target element's position and dimensions.
        /// This is useful when you want the popup to appear in relation to a specific UI element.
        /// </remarks>
        Relative,

        /// <summary>
        /// Specifies that the popup should be positioned relative to the container element.
        /// </summary>
        /// <remarks>
        /// When set to Container, the popup position is calculated based on the container's boundaries.
        /// This is useful for creating popups that stay within specific container boundaries.
        /// </remarks>
        Container
    }

    /// <summary>
    /// Defines the types of collision handling behavior for popup elements when they exceed viewport boundaries.
    /// </summary>
    /// <remarks>
    /// The <see cref="CollisionType"/> enumeration specifies how the popup should behave when it would be positioned outside the visible area.
    /// This helps ensure popups remain visible and accessible to users regardless of the target element's position.
    /// </remarks>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CollisionType
    {
        /// <summary>
        /// Specifies that no collision handling should be applied to the popup.
        /// </summary>
        /// <remarks>
        /// When set to None, the popup will be positioned at its calculated position regardless of viewport boundaries.
        /// This may result in the popup being partially or completely hidden outside the visible area.
        /// </remarks>
        [EnumMember(Value = "none")]
        None,

        /// <summary>
        /// Specifies that the popup should flip to the opposite side when collision is detected.
        /// </summary>
        /// <remarks>
        /// When set to Flip, the popup will automatically reposition to the opposite side of the target element
        /// if the original position would cause the popup to exceed viewport boundaries.
        /// </remarks>
        [EnumMember(Value = "flip")]
        Flip,

        /// <summary>
        /// Specifies that the popup should be adjusted to fit within the viewport boundaries.
        /// </summary>
        /// <remarks>
        /// When set to Fit, the popup position will be adjusted to ensure it remains completely visible within the viewport.
        /// The popup may be moved or resized to accommodate the available space.
        /// </remarks>
        [EnumMember(Value = "fit")]
        Fit
    }

    /// <summary>
    /// Specifies the animation effects that can be applied to the Tooltip component during show and hide transitions.
    /// </summary>
    /// <remarks>
    /// The <see cref="Effect"/> enumeration provides various animation options to enhance the visual experience when displaying or hiding tooltips.
    /// Different effects can be configured for open and close actions to create custom transition behaviors.
    /// Animation effects improve user experience by providing smooth visual feedback during tooltip interactions.
    /// </remarks>
    /// <example>
    /// The following example demonstrates how to set animation effects for a tooltip:
    /// <code><![CDATA[
    /// <SfTooltip Content="Sample Tooltip" OpenEffect="Effect.FadeIn" CloseEffect="Effect.FadeOut">
    ///     <div>Hover over me</div>
    /// </SfTooltip>
    /// ]]></code>
    /// </example>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Effect
    {
        /// <summary>
        /// Applies a fade-in animation effect where the tooltip gradually appears with increasing opacity.
        /// </summary>
        /// <remarks>
        /// The FadeIn effect provides a smooth transition by gradually increasing the tooltip's opacity from 0% to 100%.
        /// This is commonly used for tooltip opening animations to create a gentle appearance effect.
        /// </remarks>
        [EnumMember(Value = "FadeIn")]
        FadeIn,

        /// <summary>
        /// Applies a fade-out animation effect where the tooltip gradually disappears with decreasing opacity.
        /// </summary>
        /// <remarks>
        /// The FadeOut effect provides a smooth transition by gradually decreasing the tooltip's opacity from 100% to 0%.
        /// This is commonly used for tooltip closing animations to create a gentle disappearance effect.
        /// </remarks>
        [EnumMember(Value = "FadeOut")]
        FadeOut,

        /// <summary>
        /// Applies a combined fade and zoom-in animation effect where the tooltip appears with both opacity and scale transitions.
        /// </summary>
        /// <remarks>
        /// The FadeZoomIn effect combines fading and scaling animations, making the tooltip appear to grow from a smaller size while fading in.
        /// This creates a more dynamic and attention-grabbing entrance animation compared to simple fade effects.
        /// </remarks>
        [EnumMember(Value = "FadeZoomIn")]
        FadeZoomIn,

        /// <summary>
        /// Applies a combined fade and zoom-out animation effect where the tooltip disappears with both opacity and scale transitions.
        /// </summary>
        /// <remarks>
        /// The FadeZoomOut effect combines fading and scaling animations, making the tooltip appear to shrink to a smaller size while fading out.
        /// This creates a more dynamic and visually appealing exit animation compared to simple fade effects.
        /// </remarks>
        [EnumMember(Value = "FadeZoomOut")]
        FadeZoomOut,

        /// <summary>
        /// Applies a 3D flip animation effect where the tooltip rotates along the X-axis from the bottom upward during appearance.
        /// </summary>
        /// <remarks>
        /// The FlipXDownIn effect creates a 3D rotation animation where the tooltip flips from the bottom edge upward along the X-axis.
        /// This provides a sophisticated 3D visual effect that makes the tooltip appear to rotate into view from below.
        /// </remarks>
        [EnumMember(Value = "FlipXDownIn")]
        FlipXDownIn,

        /// <summary>
        /// Applies a 3D flip animation effect where the tooltip rotates along the X-axis from the top downward during disappearance.
        /// </summary>
        /// <remarks>
        /// The FlipXDownOut effect creates a 3D rotation animation where the tooltip flips from the top edge downward along the X-axis.
        /// This provides a sophisticated 3D visual effect that makes the tooltip appear to rotate out of view downward.
        /// </remarks>
        [EnumMember(Value = "FlipXDownOut")]
        FlipXDownOut,

        /// <summary>
        /// Applies a 3D flip animation effect where the tooltip rotates along the X-axis from the top upward during appearance.
        /// </summary>
        /// <remarks>
        /// The FlipXUpIn effect creates a 3D rotation animation where the tooltip flips from the top edge upward along the X-axis.
        /// This provides a sophisticated 3D visual effect that makes the tooltip appear to rotate into view from above.
        /// </remarks>
        [EnumMember(Value = "FlipXUpIn")]
        FlipXUpIn,

        /// <summary>
        /// Applies a 3D flip animation effect where the tooltip rotates along the X-axis from the bottom upward during disappearance.
        /// </summary>
        /// <remarks>
        /// The FlipXUpOut effect creates a 3D rotation animation where the tooltip flips from the bottom edge upward along the X-axis.
        /// This provides a sophisticated 3D visual effect that makes the tooltip appear to rotate out of view upward.
        /// </remarks>
        [EnumMember(Value = "FlipXUpOut")]
        FlipXUpOut,

        /// <summary>
        /// Applies a 3D flip animation effect where the tooltip rotates along the Y-axis from the right toward the left during appearance.
        /// </summary>
        /// <remarks>
        /// The FlipYLeftIn effect creates a 3D rotation animation where the tooltip flips from the right edge toward the left along the Y-axis.
        /// This provides a sophisticated 3D visual effect that makes the tooltip appear to rotate into view from the right side.
        /// </remarks>
        [EnumMember(Value = "FlipYLeftIn")]
        FlipYLeftIn,

        /// <summary>
        /// Applies a 3D flip animation effect where the tooltip rotates along the Y-axis from the left toward the right during disappearance.
        /// </summary>
        /// <remarks>
        /// The FlipYLeftOut effect creates a 3D rotation animation where the tooltip flips from the left edge toward the right along the Y-axis.
        /// This provides a sophisticated 3D visual effect that makes the tooltip appear to rotate out of view toward the right side.
        /// </remarks>
        [EnumMember(Value = "FlipYLeftOut")]
        FlipYLeftOut,

        /// <summary>
        /// Applies a 3D flip animation effect where the tooltip rotates along the Y-axis from the left toward the right during appearance.
        /// </summary>
        /// <remarks>
        /// The FlipYRightIn effect creates a 3D rotation animation where the tooltip flips from the left edge toward the right along the Y-axis.
        /// This provides a sophisticated 3D visual effect that makes the tooltip appear to rotate into view from the left side.
        /// </remarks>
        [EnumMember(Value = "FlipYRightIn")]
        FlipYRightIn,

        /// <summary>
        /// Applies a 3D flip animation effect where the tooltip rotates along the Y-axis from the right toward the left during disappearance.
        /// </summary>
        /// <remarks>
        /// The FlipYRightOut effect creates a 3D rotation animation where the tooltip flips from the right edge toward the left along the Y-axis.
        /// This provides a sophisticated 3D visual effect that makes the tooltip appear to rotate out of view toward the left side.
        /// </remarks>
        [EnumMember(Value = "FlipYRightOut")]
        FlipYRightOut,

        /// <summary>
        /// Applies a zoom-in animation effect where the tooltip appears by scaling from a smaller size to its full size.
        /// </summary>
        /// <remarks>
        /// The ZoomIn effect creates a scaling animation where the tooltip grows from a smaller scale to its normal size during appearance.
        /// This provides a dynamic visual effect that draws attention to the tooltip as it expands into view.
        /// </remarks>
        [EnumMember(Value = "ZoomIn")]
        ZoomIn,

        /// <summary>
        /// Applies a zoom-out animation effect where the tooltip disappears by scaling from its full size to a smaller size.
        /// </summary>
        /// <remarks>
        /// The ZoomOut effect creates a scaling animation where the tooltip shrinks from its normal size to a smaller scale during disappearance.
        /// This provides a dynamic visual effect that makes the tooltip appear to compress as it exits view.
        /// </remarks>
        [EnumMember(Value = "ZoomOut")]
        ZoomOut,

        /// <summary>
        /// Specifies that no animation effect should be applied to the tooltip during show or hide transitions.
        /// </summary>
        /// <remarks>
        /// The None option disables all animation effects, causing the tooltip to appear or disappear instantly without any transition.
        /// This is useful for performance-critical scenarios or when immediate tooltip display is preferred over visual effects.
        /// </remarks>
        [EnumMember(Value = "None")]
        None
    }

    /// <summary>
    /// Specifies the different trigger modes that determine how the Tooltip component is opened and displayed to users.
    /// </summary>
    /// <remarks>
    /// The <see cref="OpenMode"/> enumeration defines various interaction patterns for triggering tooltip display.
    /// Different modes provide flexibility to accommodate different user interface patterns and device types.
    /// The behavior may vary between desktop and mobile devices to ensure optimal user experience across platforms.
    /// </remarks>
    /// <example>
    /// The following example demonstrates how to set different open modes for a tooltip:
    /// <code><![CDATA[
    /// <SfTooltip Content="Click to see tooltip" OpenMode="OpenMode.Click">
    ///     <button>Click me</button>
    /// </SfTooltip>
    /// ]]></code>
    /// </example>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OpenMode
    {
        /// <summary>
        /// Automatically determines the best trigger method based on the target element type and device capabilities.
        /// </summary>
        /// <remarks>
        /// In Auto mode, the tooltip intelligently selects the appropriate trigger mechanism:
        /// - On desktop: appears on hover for most elements, or focus for interactive elements like buttons and inputs
        /// - On touch devices: opens on tap and hold gesture to accommodate touch interaction patterns
        /// This mode provides the most intuitive user experience by adapting to the context and platform.
        /// </remarks>
        [EnumMember(Value = "Auto")]
        Auto,

        /// <summary>
        /// Triggers the tooltip when the user hovers the mouse pointer over the target element.
        /// </summary>
        /// <remarks>
        /// Hover mode is optimized for mouse-based interactions on desktop devices:
        /// - On desktop: tooltip appears immediately when the mouse enters the target element area
        /// - On touch devices: opens on tap and hold gesture since hover is not directly supported
        /// This mode is ideal for providing contextual information without requiring explicit user actions.
        /// </remarks>
        [EnumMember(Value = "Hover")]
        Hover,

        /// <summary>
        /// Triggers the tooltip when the user clicks or taps on the target element.
        /// </summary>
        /// <remarks>
        /// Click mode requires explicit user action to display the tooltip:
        /// - On desktop: tooltip appears when the target element is clicked with the mouse
        /// - On touch devices: tooltip appears with a single tap on the target element
        /// This mode is useful when you want to show tooltips only on demand or for important information that requires user acknowledgment.
        /// </remarks>
        [EnumMember(Value = "Click")]
        Click,

        /// <summary>
        /// Triggers the tooltip when the target element receives keyboard focus or is programmatically focused.
        /// </summary>
        /// <remarks>
        /// Focus mode is particularly useful for accessibility and keyboard navigation:
        /// - On desktop: tooltip appears when the element receives focus via keyboard navigation or programmatic focus
        /// - On touch devices: tooltip appears with a single tap since touch typically triggers focus
        /// This mode ensures tooltips are accessible to users relying on keyboard navigation and assistive technologies.
        /// </remarks>
        [EnumMember(Value = "Focus")]
        Focus,

        /// <summary>
        /// Disables all default trigger behaviors, requiring manual control through programmatic methods.
        /// </summary>
        /// <remarks>
        /// Custom mode provides complete control over tooltip display timing:
        /// - No automatic triggers are active, preventing default show/hide behavior
        /// - Tooltips must be controlled using the Open() and Close() public methods
        /// - Both desktop and mobile devices require explicit method calls for tooltip management
        /// This mode is ideal for complex scenarios where tooltip display depends on custom business logic or specific application states.
        /// </remarks>
        [EnumMember(Value = "Custom")]
        Custom
    }

    /// <summary>
    /// Specifies the positioning options that determine where the Tooltip should be displayed relative to its target element.
    /// </summary>
    /// <remarks>
    /// The <see cref="Position"/> enumeration provides precise control over tooltip placement around target elements.
    /// Each position combines a primary direction (top, bottom, left, right) with an alignment (center, left, right, top, bottom).
    /// The tooltip positioning system automatically handles collision detection and may adjust the position if the preferred location would place the tooltip outside the viewport.
    /// </remarks>
    /// <example>
    /// The following example demonstrates how to set tooltip position:
    /// <code><![CDATA[
    /// <SfTooltip Content="Positioned at top-center" Position="Position.TopCenter">
    ///     <div>Target element</div>
    /// </SfTooltip>
    /// ]]></code>
    /// </example>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Position
    {
        /// <summary>
        /// Positions the tooltip above the target element, horizontally centered with respect to the target's width.
        /// </summary>
        /// <remarks>
        /// TopCenter positioning places the tooltip directly above the target element with the tooltip's horizontal center aligned with the target's horizontal center.
        /// This is one of the most commonly used positions as it provides clear visibility without obscuring the target element.
        /// The tooltip's bottom edge will be positioned near the target's top edge.
        /// </remarks>
        [EnumMember(Value = "TopCenter")]
        TopCenter,

        /// <summary>
        /// Positions the tooltip above the target element, aligned with the target's left edge.
        /// </summary>
        /// <remarks>
        /// TopLeft positioning places the tooltip above the target element with the tooltip's left edge aligned with the target's left edge.
        /// This position is useful when you want to maintain left alignment between the tooltip and target.
        /// The tooltip appears above the target without extending beyond the target's left boundary.
        /// </remarks>
        [EnumMember(Value = "TopLeft")]
        TopLeft,

        /// <summary>
        /// Positions the tooltip above the target element, aligned with the target's right edge.
        /// </summary>
        /// <remarks>
        /// TopRight positioning places the tooltip above the target element with the tooltip's right edge aligned with the target's right edge.
        /// This position is useful when you want to maintain right alignment between the tooltip and target.
        /// The tooltip appears above the target without extending beyond the target's right boundary.
        /// </remarks>
        [EnumMember(Value = "TopRight")]
        TopRight,

        /// <summary>
        /// Positions the tooltip below the target element, aligned with the target's left edge.
        /// </summary>
        /// <remarks>
        /// BottomLeft positioning places the tooltip below the target element with the tooltip's left edge aligned with the target's left edge.
        /// This position maintains consistent left alignment and is useful when screen space above the target is limited.
        /// The tooltip's top edge will be positioned near the target's bottom edge.
        /// </remarks>
        [EnumMember(Value = "BottomLeft")]
        BottomLeft,

        /// <summary>
        /// Positions the tooltip below the target element, horizontally centered with respect to the target's width.
        /// </summary>
        /// <remarks>
        /// BottomCenter positioning places the tooltip directly below the target element with the tooltip's horizontal center aligned with the target's horizontal center.
        /// This is another commonly used position that provides excellent visibility when there's insufficient space above the target.
        /// The tooltip's top edge will be positioned near the target's bottom edge.
        /// </remarks>
        [EnumMember(Value = "BottomCenter")]
        BottomCenter,

        /// <summary>
        /// Positions the tooltip below the target element, aligned with the target's right edge.
        /// </summary>
        /// <remarks>
        /// BottomRight positioning places the tooltip below the target element with the tooltip's right edge aligned with the target's right edge.
        /// This position maintains consistent right alignment and is useful for right-aligned UI layouts.
        /// The tooltip appears below the target without extending beyond the target's right boundary.
        /// </remarks>
        [EnumMember(Value = "BottomRight")]
        BottomRight,

        /// <summary>
        /// Positions the tooltip to the left of the target element, aligned with the target's top edge.
        /// </summary>
        /// <remarks>
        /// LeftTop positioning places the tooltip to the left side of the target element with the tooltip's top edge aligned with the target's top edge.
        /// This position is useful for wide layouts where horizontal space is available on the left side.
        /// The tooltip's right edge will be positioned near the target's left edge.
        /// </remarks>
        [EnumMember(Value = "LeftTop")]
        LeftTop,

        /// <summary>
        /// Positions the tooltip to the left of the target element, vertically centered with respect to the target's height.
        /// </summary>
        /// <remarks>
        /// LeftCenter positioning places the tooltip to the left side of the target element with the tooltip's vertical center aligned with the target's vertical center.
        /// This position provides optimal balance and is commonly used when there's adequate space to the left of the target.
        /// The tooltip's right edge will be positioned near the target's left edge.
        /// </remarks>
        [EnumMember(Value = "LeftCenter")]
        LeftCenter,

        /// <summary>
        /// Positions the tooltip to the left of the target element, aligned with the target's bottom edge.
        /// </summary>
        /// <remarks>
        /// LeftBottom positioning places the tooltip to the left side of the target element with the tooltip's bottom edge aligned with the target's bottom edge.
        /// This position maintains bottom alignment and is useful when you want the tooltip to align with the target's lower portion.
        /// The tooltip's right edge will be positioned near the target's left edge.
        /// </remarks>
        [EnumMember(Value = "LeftBottom")]
        LeftBottom,

        /// <summary>
        /// Positions the tooltip to the right of the target element, aligned with the target's top edge.
        /// </summary>
        /// <remarks>
        /// RightTop positioning places the tooltip to the right side of the target element with the tooltip's top edge aligned with the target's top edge.
        /// This position is useful for layouts where horizontal space is available on the right side.
        /// The tooltip's left edge will be positioned near the target's right edge.
        /// </remarks>
        [EnumMember(Value = "RightTop")]
        RightTop,

        /// <summary>
        /// Positions the tooltip to the right of the target element, vertically centered with respect to the target's height.
        /// </summary>
        /// <remarks>
        /// RightCenter positioning places the tooltip to the right side of the target element with the tooltip's vertical center aligned with the target's vertical center.
        /// This position provides optimal balance and is commonly used when there's adequate space to the right of the target.
        /// The tooltip's left edge will be positioned near the target's right edge.
        /// </remarks>
        [EnumMember(Value = "RightCenter")]
        RightCenter,

        /// <summary>
        /// Positions the tooltip to the right of the target element, aligned with the target's bottom edge.
        /// </summary>
        /// <remarks>
        /// RightBottom positioning places the tooltip to the right side of the target element with the tooltip's bottom edge aligned with the target's bottom edge.
        /// This position maintains bottom alignment and is useful when you want the tooltip to align with the target's lower portion.
        /// The tooltip's left edge will be positioned near the target's right edge.
        /// </remarks>
        [EnumMember(Value = "RightBottom")]
        RightBottom
    }

    /// <summary>
    /// Specifies the positioning options for the tip pointer (arrow) that connects the Tooltip to its target element.
    /// </summary>
    /// <remarks>
    /// The <see cref="TipPointerPosition"/> enumeration controls the placement of the visual pointer that indicates the relationship between the tooltip and its target.
    /// The tip pointer provides a clear visual connection, making it obvious which element the tooltip refers to.
    /// The actual position depends on the tooltip's overall position relative to the target element (top, bottom, left, or right).
    /// Different pointer positions can improve visual balance and alignment in various UI layouts.
    /// </remarks>
    /// <example>
    /// The following example demonstrates how to set tip pointer position:
    /// <code><![CDATA[
    /// <SfTooltip Content="Tooltip with custom pointer" TipPointerPosition="TipPointerPosition.Start">
    ///     <div>Target element</div>
    /// </SfTooltip>
    /// ]]></code>
    /// </example>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TipPointerPosition
    {
        /// <summary>
        /// Automatically determines the optimal tip pointer position based on the tooltip's position and available space.
        /// </summary>
        /// <remarks>
        /// Auto positioning allows the tooltip component to intelligently select the best pointer placement:
        /// - Considers the tooltip's position relative to the target element
        /// - Takes into account available viewport space and potential collisions
        /// - Ensures optimal visual connection between tooltip and target
        /// This mode provides the most user-friendly experience by adapting to different scenarios automatically.
        /// </remarks>
        [EnumMember(Value = "Auto")]
        Auto,

        /// <summary>
        /// Positions the tip pointer at the beginning edge of the tooltip element.
        /// </summary>
        /// <remarks>
        /// Start positioning places the pointer at the beginning of the tooltip's edge that faces the target:
        /// - For top/bottom positioned tooltips: pointer appears at the left edge of the tooltip
        /// - For left/right positioned tooltips: pointer appears at the top edge of the tooltip
        /// This position is useful for creating left-aligned or top-aligned pointer placement that matches UI design requirements.
        /// </remarks>
        [EnumMember(Value = "Start")]
        Start,

        /// <summary>
        /// Positions the tip pointer at the center of the tooltip element's edge facing the target.
        /// </summary>
        /// <remarks>
        /// Middle positioning places the pointer at the center point of the tooltip's edge that faces the target:
        /// - For top/bottom positioned tooltips: pointer appears at the horizontal center of the tooltip
        /// - For left/right positioned tooltips: pointer appears at the vertical center of the tooltip
        /// This position provides balanced visual alignment and is commonly used for centered tooltip designs.
        /// </remarks>
        [EnumMember(Value = "Middle")]
        Middle,

        /// <summary>
        /// Positions the tip pointer at the ending edge of the tooltip element.
        /// </summary>
        /// <remarks>
        /// End positioning places the pointer at the end of the tooltip's edge that faces the target:
        /// - For top/bottom positioned tooltips: pointer appears at the right edge of the tooltip
        /// - For left/right positioned tooltips: pointer appears at the bottom edge of the tooltip
        /// This position is useful for creating right-aligned or bottom-aligned pointer placement that matches specific UI design requirements.
        /// </remarks>
        [EnumMember(Value = "End")]
        End
    }

    /// <summary>
    /// Specifies the type of drop effect to be applied during drag-and-drop operations in input components.
    /// </summary>
    /// <remarks>
    /// The <see cref="DropEffect"/> enumeration defines the visual feedback and behavior that occurs when an item is dropped during a drag-and-drop operation.
    /// This enum is commonly used in file upload components and other input controls that support drag-and-drop functionality.
    /// The drop effect determines how the dragged content will be handled when dropped onto the target area.
    /// </remarks>
    /// <example>
    /// Setting a drop effect for a file upload component:
    /// <code><![CDATA[
    /// <SfUploader DropEffect="DropEffect.Copy" ValueChange="OnChange">
    /// </SfUploader>
    /// ]]></code>
    /// </example>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DropEffect
    {
        /// <summary>
        /// Creates a copy of the dragged item at the drop location.
        /// </summary>
        /// <remarks>
        /// When <c>Copy</c> is specified, the original item remains in its source location while a duplicate is created at the drop target.
        /// This is the most common drop effect for file uploads where the original file remains in its source directory.
        /// </remarks>
        [EnumMember(Value = "Copy")]
        Copy,

        /// <summary>
        /// Moves the dragged item from its source to the drop location.
        /// </summary>
        /// <remarks>
        /// When <c>Move</c> is specified, the item is relocated from its original position to the drop target location.
        /// The original item is removed from the source location once the drop operation is completed successfully.
        /// </remarks>
        [EnumMember(Value = "Move")]
        Move,

        /// <summary>
        /// Creates a link or reference to the dragged item at the drop location.
        /// </summary>
        /// <remarks>
        /// When <c>Link</c> is specified, a reference or shortcut to the original item is created at the drop target.
        /// The original item remains unchanged in its source location, and the link provides access to the original content.
        /// This is useful for creating shortcuts or references without duplicating the actual data.
        /// </remarks>
        [EnumMember(Value = "Link")]
        Link,

        /// <summary>
        /// Indicates that no drop effect should be applied, effectively disabling the drop operation.
        /// </summary>
        /// <remarks>
        /// When <c>None</c> is specified, the drop target will not accept the dragged item, and no operation will be performed.
        /// This effectively disables drag-and-drop functionality for the target area and provides visual feedback that dropping is not allowed.
        /// </remarks>
        [EnumMember(Value = "None")]
        None,

        /// <summary>
        /// Uses the default drop effect behavior as determined by the browser or component.
        /// </summary>
        /// <remarks>
        /// When <c>Default</c> is specified, the component will use the standard drop effect behavior provided by the browser.
        /// This typically defaults to the <c>Copy</c> operation for most file upload scenarios, but may vary based on the browser implementation and context.
        /// Using the default option allows the component to automatically determine the most appropriate drop effect.
        /// </remarks>
        [EnumMember(Value = "Default")]
        Default,
    }

    /// <summary>
    /// Specifies the direction from which the <see cref="SfDialog"/> can be resized.
    /// </summary>
    /// <remarks>
    /// This enumeration controls which edges and corners of the dialog are active for resizing. To enable resizing, set <see cref="SfDialog.EnableResize"/> to <c>true</c>.
    /// </remarks>
    public enum ResizeDirection
    {
        /// <summary>
        /// Specifies that the dialog can be resized by dragging the bottom-right corner.
        /// </summary>
        [EnumMember(Value = "SouthEast")]
        SouthEast,

        /// <summary>
        /// Specifies that the dialog can be resized by dragging the bottom edge.
        /// </summary>
        [EnumMember(Value = "South")]
        South,

        /// <summary>
        /// Specifies that the dialog can be resized by dragging the top edge.
        /// </summary>
        [EnumMember(Value = "North")]
        North,

        /// <summary>
        /// Specifies that the dialog can be resized by dragging the right edge.
        /// </summary>
        [EnumMember(Value = "East")]
        East,

        /// <summary>
        /// Specifies that the dialog can be resized by dragging the left edge.
        /// </summary>
        [EnumMember(Value = "West")]
        West,

        /// <summary>
        /// Specifies that the dialog can be resized by dragging the top-right corner.
        /// </summary>
        [EnumMember(Value = "NorthEast")]
        NorthEast,

        /// <summary>
        /// Specifies that the dialog can be resized by dragging the top-left corner.
        /// </summary>
        [EnumMember(Value = "NorthWest")]
        NorthWest,

        /// <summary>
        /// Specifies that the dialog can be resized by dragging the bottom-left corner.
        /// </summary>
        [EnumMember(Value = "SouthWest")]
        SouthWest,

        /// <summary>
        /// Specifies that the dialog can be resized from all edges and corners.
        /// </summary>
        [EnumMember(Value = "All")]
        All
    }

    /// <summary>
    /// Specifies the built-in animation effect to apply when the <see cref="SfDialog"/> is shown or hidden.
    /// </summary>
    /// <remarks>
    /// These effects provide visual transitions to enhance user experience. The animation is determined by the <see cref="DialogAnimationSettings.Effect"/> property.
    /// </remarks>
    public enum DialogEffect
    {
        /// <summary>
        /// The dialog fades in when opening and fades out when closing.
        /// </summary>
        [EnumMember(Value = "Fade")]
        Fade,

        /// <summary>
        /// The dialog fades and zooms in when opening, and fades and zooms out when closing.
        /// </summary>
        [EnumMember(Value = "FadeZoom")]
        FadeZoom,

        /// <summary>
        /// The dialog flips in from the left and downwards when opening.
        /// </summary>
        [EnumMember(Value = "FlipLeftDown")]
        FlipLeftDown,

        /// <summary>
        /// The dialog flips in from the left and upwards when opening.
        /// </summary>
        [EnumMember(Value = "FlipLeftUp")]
        FlipLeftUp,

        /// <summary>
        /// The dialog flips in from the right and downwards when opening.
        /// </summary>
        [EnumMember(Value = "FlipRightDown")]
        FlipRightDown,

        /// <summary>
        /// The dialog flips in from the right and upwards when opening.
        /// </summary>
        [EnumMember(Value = "FlipRightUp")]
        FlipRightUp,

        /// <summary>
        /// The dialog flips downwards on its X-axis when opening.
        /// </summary>
        [EnumMember(Value = "FlipXDown")]
        FlipXDown,

        /// <summary>
        /// The dialog flips upwards on its X-axis when opening.
        /// </summary>
        [EnumMember(Value = "FlipXUp")]
        FlipXUp,

        /// <summary>
        /// The dialog flips to the left on its Y-axis when opening.
        /// </summary>
        [EnumMember(Value = "FlipYLeft")]
        FlipYLeft,

        /// <summary>
        /// The dialog flips to the right on its Y-axis when opening.
        /// </summary>
        [EnumMember(Value = "FlipYRight")]
        FlipYRight,

        /// <summary>
        /// The dialog slides in from the bottom when opening.
        /// </summary>
        [EnumMember(Value = "SlideBottom")]
        SlideBottom,

        /// <summary>
        /// The dialog slides in from the left when opening.
        /// </summary>
        [EnumMember(Value = "SlideLeft")]
        SlideLeft,

        /// <summary>
        /// The dialog slides in from the right when opening.
        /// </summary>
        [EnumMember(Value = "SlideRight")]
        SlideRight,

        /// <summary>
        /// The dialog slides in from the top when opening.
        /// </summary>
        [EnumMember(Value = "SlideTop")]
        SlideTop,

        /// <summary>
        /// The dialog zooms in when opening and zooms out when closing.
        /// </summary>
        [EnumMember(Value = "Zoom")]
        Zoom,

        /// <summary>
        /// No animation is applied. The dialog appears and disappears instantly.
        /// </summary>
        [EnumMember(Value = "None")]
        None
    }

    /// <summary>
    /// Defines the floating label behavior for input components, controlling how and when the label transitions from placeholder to floating position.
    /// </summary>
    /// <value>
    /// An enumeration that specifies the floating label behavior mode for input components.
    /// </value>
    /// <remarks>
    /// <para>The floating label provides enhanced user experience by transforming the placeholder text into a label that appears above the input field.
    /// This behavior helps maintain context while the user interacts with the input, ensuring they always know what information is expected.
    /// The different modes offer flexibility for various design patterns and user interaction preferences.</para>
    /// <list type="bullet">
    /// <item><description><see cref="Never"/> - The label remains as placeholder text and never transitions to a floating position.</description></item>
    /// <item><description><see cref="Always"/> - The label is permanently positioned above the input field.</description></item>
    /// <item><description><see cref="Auto"/> - The label automatically floats when the field receives focus or contains a value.</description></item>
    /// </list>
    /// </remarks>
    /// <example>
    /// Setting floating label behavior:
    /// <code><![CDATA[
    /// <SfTextBox FloatLabelType="FloatLabelType.Auto" Placeholder="Enter your name"></SfTextBox>
    /// ]]></code>
    /// </example>
    public enum FloatLabelType
    {
        /// <summary>
        /// The label remains static as a placeholder and never transitions to a floating position above the input.
        /// </summary>
        /// <value>
        /// Represents the "Never" floating label mode where the label stays as placeholder text.
        /// </value>
        /// <remarks>
        /// When this mode is selected, the label text remains in the input field as placeholder text and does not move to a floating position above the input, even when the field is focused or contains a value.
        /// </remarks>
        [EnumMember(Value = "Never")]
        Never,

        /// <summary>
        /// The label is permanently positioned above the input field, regardless of focus state or content.
        /// </summary>
        /// <value>
        /// Represents the "Always" floating label mode where the label is permanently positioned above the input.
        /// </value>
        /// <remarks>
        /// In this mode, the label is always displayed above the input field, providing constant visual context for the expected input regardless of whether the field is focused or contains a value.
        /// </remarks>
        [EnumMember(Value = "Always")]
        Always,

        /// <summary>
        /// The label automatically transitions to a floating position above the input when the field receives focus or contains a value.
        /// </summary>
        /// <value>
        /// Represents the "Auto" floating label mode where the label automatically transitions based on input state.
        /// </value>
        /// <remarks>
        /// This is the most dynamic mode where the label starts as placeholder text and smoothly transitions to a floating position above the input when the user focuses on the field or when the field contains a value. This provides the best balance of space efficiency and user experience.
        /// </remarks>
        [EnumMember(Value = "Auto")]
        Auto,
    }

    /// <summary>
    /// Defines whether the browser is allowed to automatically enter or select values for input fields using stored user data.
    /// </summary>
    /// <value>
    /// An enumeration that controls browser autocomplete behavior for input fields.
    /// </value>
    /// <remarks>
    /// <para>The autocomplete feature leverages browser-stored user data such as previously entered values, saved passwords,
    /// or address information to provide suggestions and automatic filling capabilities. This enhances user experience
    /// by reducing repetitive data entry, but can be disabled for sensitive fields or when custom autocomplete
    /// functionality is preferred.</para>
    /// <list type="bullet">
    /// <item><description><see cref="On"/> - Enables browser autocomplete suggestions and automatic filling.</description></item>
    /// <item><description><see cref="Off"/> - Disables browser autocomplete for the input field.</description></item>
    /// </list>
    /// </remarks>
    /// <example>
    /// Controlling autocomplete behavior:
    /// <code><![CDATA[
    /// <SfTextBox AutoComplete="AutoComplete.Off" Placeholder="Sensitive information"></SfTextBox>
    /// ]]></code>
    /// </example>
    public enum AutoComplete
    {
        /// <summary>
        /// Enables browser autocomplete functionality, allowing automatic suggestions and filling based on user's previous inputs and saved data.
        /// </summary>
        /// <value>
        /// Represents the "on" state for browser autocomplete functionality.
        /// </value>
        /// <remarks>
        /// When enabled, the browser will provide autocomplete suggestions based on previously entered values, saved form data, and other stored user information. This improves user experience by reducing the need for repetitive typing.
        /// </remarks>
        [EnumMember(Value = "on")]
        On,

        /// <summary>
        /// Disables browser autocomplete functionality, preventing automatic suggestions and requiring manual input for all values.
        /// </summary>
        /// <value>
        /// Represents the "off" state for browser autocomplete functionality.
        /// </value>
        /// <remarks>
        /// When disabled, the browser will not provide autocomplete suggestions or automatically fill the input field. This is typically used for sensitive information such as passwords, credit card numbers, or when implementing custom autocomplete functionality.
        /// </remarks>
        [EnumMember(Value = "off")]
        Off
    }

    /// <summary>
    /// Specifies the input type for TextBox components, determining the data format, validation behavior, and browser-specific features.
    /// </summary>
    /// <value>
    /// An enumeration that defines the input type and associated behavior for TextBox components.
    /// </value>
    /// <remarks>
    /// Different input types provide specialized functionality including:
    /// <list type="bullet">
    /// <item><description>Format validation (email, URL patterns)</description></item>
    /// <item><description>Virtual keyboard optimization on mobile devices</description></item>
    /// <item><description>Browser-specific UI enhancements (password masking, number steppers)</description></item>
    /// <item><description>Accessibility improvements for screen readers</description></item>
    /// <item><description>Built-in validation messages and constraints</description></item>
    /// </list>
    /// Choose the appropriate input type to ensure optimal user experience and data integrity.
    /// </remarks>
    /// <example>
    /// Setting different input types:
    /// <code><![CDATA[
    /// <SfTextBox Type="InputType.Email" Placeholder="Enter email address"></SfTextBox>
    /// <SfTextBox Type="InputType.Password" Placeholder="Enter password"></SfTextBox>
    /// ]]></code>
    /// </example>
    public enum InputType
    {
        /// <summary>
        /// Standard single-line text input accepting any alphanumeric characters and symbols without format restrictions.
        /// </summary>
        /// <value>
        /// Represents the "text" input type for general text entry.
        /// </value>
        /// <remarks>
        /// This is the default input type that allows users to enter any combination of letters, numbers, and special characters. No specific format validation or input restrictions are applied.
        /// </remarks>
        [EnumMember(Value = "text")]
        Text,

        /// <summary>
        /// Email address input with built-in validation for proper email format and optimized virtual keyboard on mobile devices.
        /// </summary>
        /// <value>
        /// Represents the "email" input type for email address entry.
        /// </value>
        /// <remarks>
        /// This input type provides built-in email format validation and displays an optimized virtual keyboard on mobile devices with easy access to the @ symbol and common email domains.
        /// </remarks>
        [EnumMember(Value = "email")]
        Email,

        /// <summary>
        /// Password input where characters are visually masked for security, preventing shoulder surfing and maintaining privacy.
        /// </summary>
        /// <value>
        /// Represents the "password" input type for secure text entry.
        /// </value>
        /// <remarks>
        /// Characters entered in password fields are masked with dots or asterisks to prevent visual eavesdropping. This input type also typically disables browser autocomplete and copy functionality for enhanced security.
        /// </remarks>
        [EnumMember(Value = "password")]
        Password,

        /// <summary>
        /// Numeric input with built-in number validation and spinner controls for incrementing/decrementing values.
        /// </summary>
        /// <value>
        /// Represents the "number" input type for numeric entry.
        /// </value>
        /// <remarks>
        /// This input type restricts input to numeric values and may provide spinner controls (up/down arrows) for incrementing and decrementing values. Mobile devices will display a numeric keypad for easier number entry.
        /// </remarks>
        [EnumMember(Value = "number")]
        Number,

        /// <summary>
        /// Search input optimized for search queries with enhanced styling and potential search-specific browser features.
        /// </summary>
        /// <value>
        /// Represents the "search" input type for search functionality.
        /// </value>
        /// <remarks>
        /// Search inputs may have special styling such as rounded corners and a search icon. Some browsers provide additional features like a clear button (X) to quickly empty the search field.
        /// </remarks>
        [EnumMember(Value = "search")]
        Search,

        /// <summary>
        /// Telephone number input with specialized virtual keyboard layout and potential format validation for phone numbers.
        /// </summary>
        /// <value>
        /// Represents the "tel" input type for telephone number entry.
        /// </value>
        /// <remarks>
        /// This input type optimizes the virtual keyboard on mobile devices for phone number entry, typically displaying a numeric keypad with additional characters like + and * that are commonly used in phone numbers.
        /// </remarks>
        [EnumMember(Value = "tel")]
        Tel,

        /// <summary>
        /// URL input with validation for proper web address format and optimized virtual keyboard for URL entry.
        /// </summary>
        /// <value>
        /// Represents the "url" input type for web address entry.
        /// </value>
        /// <remarks>
        /// URL inputs provide format validation for web addresses and display an optimized virtual keyboard on mobile devices with easy access to common URL characters like forward slashes, dots, and the .com key.
        /// </remarks>
        [EnumMember(Value = "url")]
        URL
    }

    /// <summary>
    /// Defines the resize behavior and directional constraints for TextArea components, controlling how users can dynamically adjust the input area dimensions.
    /// </summary>
    /// <value>
    /// An enumeration that specifies the resize capabilities and directional constraints for TextArea components.
    /// </value>
    /// <remarks>
    /// The resize functionality allows users to adjust the TextArea dimensions to accommodate varying content lengths and personal preferences.
    /// Different resize modes provide flexibility while maintaining layout integrity:
    /// <list type="bullet">
    /// <item><description><strong>None:</strong> Maintains fixed dimensions for consistent layouts</description></item>
    /// <item><description><strong>Vertical:</strong> Allows height adjustment for accommodating more text lines</description></item>
    /// <item><description><strong>Horizontal:</strong> Allows width adjustment for longer text lines</description></item>
    /// <item><description><strong>Both:</strong> Provides maximum flexibility for user customization</description></item>
    /// </list>
    /// Consider the layout requirements and user needs when selecting the appropriate resize mode.
    /// </remarks>
    /// <example>
    /// Configuring TextArea resize behavior:
    /// <code><![CDATA[
    /// <SfTextArea ResizeMode="Resize.Vertical" Placeholder="Enter your message"></SfTextArea>
    /// ]]></code>
    /// </example>
    public enum Resize
    {
        /// <summary>
        /// The TextArea component maintains fixed dimensions and cannot be resized by the user in any direction.
        /// </summary>
        /// <value>
        /// Represents the "None" resize mode where the TextArea has fixed dimensions.
        /// </value>
        /// <remarks>
        /// When this mode is selected, the TextArea maintains its initial width and height settings and users cannot resize it. This ensures consistent layout appearance and prevents users from disrupting the page layout.
        /// </remarks>
        [EnumMember(Value = "None")]
        None,

        /// <summary>
        /// The TextArea component can be resized vertically to adjust height while maintaining a fixed width.
        /// </summary>
        /// <value>
        /// Represents the "Vertical" resize mode allowing height adjustment only.
        /// </value>
        /// <remarks>
        /// This mode allows users to adjust only the height of the TextArea by dragging the resize handle vertically. The width remains fixed, making it ideal for accommodating varying amounts of text content while maintaining consistent column layouts.
        /// </remarks>
        [EnumMember(Value = "Vertical")]
        Vertical,

        /// <summary>
        /// The TextArea component can be resized horizontally to adjust width while maintaining a fixed height.
        /// </summary>
        /// <value>
        /// Represents the "Horizontal" resize mode allowing width adjustment only.
        /// </value>
        /// <remarks>
        /// This mode allows users to adjust only the width of the TextArea by dragging the resize handle horizontally. The height remains fixed, which is useful for accommodating longer text lines while maintaining consistent row heights in the layout.
        /// </remarks>
        [EnumMember(Value = "Horizontal")]
        Horizontal,

        /// <summary>
        /// The TextArea component can be resized in both vertical and horizontal directions, providing complete dimensional flexibility.
        /// </summary>
        /// <value>
        /// Represents the "Both" resize mode allowing full dimensional adjustment.
        /// </value>
        /// <remarks>
        /// This mode provides maximum flexibility by allowing users to resize the TextArea in both width and height dimensions. Users can drag the resize handle in any direction to adjust the component size according to their content and preference needs.
        /// </remarks>
        [EnumMember(Value = "Both")]
        Both,

    }

    /// <summary>
    /// Specifies the possible label positions for components supporting label alignment.
    /// </summary>
    /// <remarks>
    /// The <see cref="LabelPosition"/> enumeration allows you to choose whether the label appears before or after
    /// the associated component, such as a button or an input. This enables customization of the UI layout and accessibility support.
    /// </remarks>
    /// <example>
    /// The following example demonstrates how to set the label position for a button:
    /// <code><![CDATA[
    /// <SfButton Label="Save" LabelPosition="LabelPosition.After" />
    /// ]]></code>
    /// </example>
    public enum LabelPosition
    {
        /// <summary>
        /// Positions the label after the component (for example: text will be rendered to the right of a button).
        /// </summary>
        /// <value>
        /// Represents the label placed after the associated component.
        /// </value>
        /// <remarks>
        /// Use <see cref="After"/> to display the label following the component.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfButton Label="Click Me" LabelPosition="LabelPosition.After" />
        /// ]]></code>
        /// </example>
        After,

        /// <summary>
        /// Positions the label before the component (for example: text will be rendered to the left of a button).
        /// </summary>
        /// <value>
        /// Represents the label placed before the associated component.
        /// </value>
        /// <remarks>
        /// Use <see cref="Before"/> to display the label preceding the component.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfButton Label="Click Me" LabelPosition="LabelPosition.Before" />
        /// ]]></code>
        /// </example>
        Before,
    }


    /// <summary>
    /// Internal representation of checkbox state transitions for managing visual and logical states.
    /// </summary>
    public enum CheckboxState
    {
        /// <summary>The checkbox is in the checked state.</summary>
        Checked,

        /// <summary>The checkbox is in the unchecked state.</summary>
        Unchecked,

        /// <summary>The checkbox is in the indeterminate (mixed) state.</summary>
        Indeterminate
    }

    /// <summary>
    /// Specifies the selection behavior of the <see cref="SfButtonGroup"/>.
    /// </summary>
    public enum SelectionMode
    {
        /// <summary>
        /// No items can be selected. Selection is disabled.
        /// </summary>
        None,

        /// <summary>
        /// Only one item can be selected at a time. Selecting a new item automatically deselects the previously selected item.
        /// </summary>
        Single,

        /// <summary>
        /// Multiple items can be selected simultaneously. Users can select and deselect items independently.
        /// </summary>
        Multiple,
    }

    /// <summary>
    /// Specifies the layout position of an icon inside a <see cref="SfButton"/>.
    /// </summary>
    /// <remarks>
    /// This enumeration determines where the icon is placed relative to the button content: left, right, above, or below.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// <SfButton IconCss="e-icons e-search" IconPosition="IconPosition.Top" Content="Search" />
    /// ]]></code>
    /// </example>
    public enum IconPosition
    {
        /// <summary>
        /// Positions the icon to the left of the button content.
        /// </summary>
        Left,

        /// <summary>
        /// Positions the icon to the right of the button content.
        /// </summary>
        Right,

        /// <summary>
        /// Positions the icon above the button content.
        /// </summary>
        Top,

        /// <summary>
        /// Positions the icon below the button content.
        /// </summary>
        Bottom,
    }


    /// <summary>
    /// Specifies the marker shape options for SVG rendering.
    /// </summary>
    public enum ShapeName
    {
        /// <summary>
        /// Defines a path shape element.
        /// </summary>
        path,

        /// <summary>
        /// Defines an ellipse shape element.
        /// </summary>
        ellipse,

        /// <summary>
        /// Defines an image shape element.
        /// </summary>
        image
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

    /// <summary>
    /// Specifies the horizontal alignment of chart elements.
    /// </summary>
    /// <remarks>
    /// Used to position titles, legends, labels, and other UI elements within their containers.
    /// </remarks>
    public enum Alignment
    {
        /// <summary>
        /// Elements are aligned toward the start (left for LTR, right for RTL).
        /// </summary>
        Near,

        /// <summary>
        /// Elements are centered within their container.
        /// </summary>
        Center,

        /// <summary>
        /// Elements are aligned toward the end (right for LTR, left for RTL).
        /// </summary>
        Far
    }

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
    /// Supported marker shapes used inside tooltips.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TooltipShape
    {
        // Circle marker.
        [EnumMember(Value = "Circle")]
        Circle,
        // Rectangle marker.
        [EnumMember(Value = "Rectangle")]
        Rectangle,
        // Triangle marker.
        [EnumMember(Value = "Triangle")]
        Triangle,
        // Diamond marker.
        [EnumMember(Value = "Diamond")]
        Diamond,
        // Cross marker.
        [EnumMember(Value = "Cross")]
        Cross,
        // Horizontal line marker.
        [EnumMember(Value = "HorizontalLine")]
        HorizontalLine,
        // Vertical line marker.
        [EnumMember(Value = "VerticalLine")]
        VerticalLine,
        // Pentagon marker.
        [EnumMember(Value = "Pentagon")]
        Pentagon,
        // Inverted triangle marker.
        [EnumMember(Value = "InvertedTriangle")]
        InvertedTriangle,
        // Image marker.
        [EnumMember(Value = "Image")]
        Image,
        // No marker.
        [EnumMember(Value = "None")]
        None
    }

    /// <summary>
    /// Specifies the different view levels available for calendar components.
    /// </summary>
    /// <remarks>
    /// The <see cref="CalendarView"/> enum defines the hierarchical view levels that can be displayed in calendar components such as <c>SfCalendar</c>, <c>SfDatePicker</c>, and <c>SfDateTimePicker</c>.
    /// Users can navigate between these views to select dates at different levels of granularity.
    /// </remarks>
    /// <example>
    /// Setting the calendar view:
    /// <code><![CDATA[
    /// <SfCalendar TValue="DateTime" View="CalendarView.Year"></SfCalendar>
    /// ]]></code>
    /// </example>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CalendarView
    {
        /// <summary>
        /// Displays the calendar in month view, showing individual days of a specific month.
        /// </summary>
        /// <remarks>
        /// This is the default and most detailed view level, allowing users to select specific dates within a month.
        /// The month view displays all days in a grid format with day names as headers.
        /// </remarks>
        [EnumMember(Value = "Month")]
        Month,

        /// <summary>
        /// Displays the calendar in year view, showing all months of a specific year.
        /// </summary>
        /// <remarks>
        /// In year view, users can select entire months rather than individual dates.
        /// This view is useful when month-level selection is required or when navigating to different months quickly.
        /// </remarks>
        [EnumMember(Value = "Year")]
        Year,

        /// <summary>
        /// Displays the calendar in decade view, showing a range of years within a decade.
        /// </summary>
        /// <remarks>
        /// The decade view provides the highest level of navigation, allowing users to select entire years.
        /// This view is typically used for quick navigation across multiple years or when year-level selection is needed.
        /// </remarks>
        [EnumMember(Value = "Decade")]
        Decade,
    }

    /// <summary>
    /// Defines the button types for HTML button element behavior.
    /// </summary>
    /// <remarks>
    /// This enumeration allows you to specify how the button interacts with forms.
    /// Use these types to control form submission, reset, or default button behavior.
    /// </remarks>
    /// <example>
    /// The following example demonstrates how to use button types:
    /// <code><![CDATA[
    /// <SfButton Type="ButtonType.Submit" Content="Submit Form" />
    /// ]]></code>
    /// </example>
    public enum ButtonType
    {
        /// <summary>
        /// Specifies that the button is a standard button that does not interact with forms.
        /// </summary>
        /// <remarks>
        /// This is the default button type and won't trigger form submission or reset.
        /// </remarks>
        [EnumMember(Value = "Button")]
        Button,

        /// <summary>
        /// Specifies that the button submits the form when clicked.
        /// </summary>
        /// <remarks>
        /// Use this type to trigger form validation and submission.
        /// </remarks>
        [EnumMember(Value = "Submit")]
        Submit,

        /// <summary>
        /// Specifies that the button resets all form fields to their initial values when clicked.
        /// </summary>
        /// <remarks>
        /// Use this type to clear form fields back to their default state.
        /// </remarks>
        [EnumMember(Value = "Reset")]
        Reset
    }
}
