using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Charts.Internal;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Toolkit.Charts
{

    /// <summary>
    /// Represents the tooltip information for a chart data point.
    /// </summary>
    public class ChartTooltipInfo : ChartDataPointInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChartTooltipInfo"/> class.
        /// </summary>
        /// <remarks>
        /// The constructor initializes the tooltip information with default values.
        /// </remarks>
        public ChartTooltipInfo()
        {
        }
    }

    /// <summary>
    /// Represents the point data information for a chart.
    /// </summary>
    public class ChartDataPointInfo : PointInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChartDataPointInfo"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor initializes the chart data point information with default values.
        /// </remarks>
        public ChartDataPointInfo()
        {
        }

        /// <summary>
        /// Gets or sets the close value for the point.
        /// </summary>
        /// <value>
        /// Accepts the value as an object. The default value is <b>null</b>.
        /// </value>
        /// <remarks>
        /// This property is important for financial charts where a single point may represent a period.
        /// </remarks>
        public object Close { get; set; } = null!;

        /// <summary>
        /// Gets or sets the high value for the point.
        /// </summary>
        /// <value>
        /// Accepts the value as an object. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// Used to specify the highest value of a series of data points in a chart.
        /// </remarks>
        public object High { get; set; } = null!;

        /// <summary>
        /// Gets or sets the low value for the point.
        /// </summary>
        /// <value>
        /// Accepts the value as an object. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// Represents the lowest value of a data point series in a chart.
        /// </remarks>
        public object Low { get; set; } = null!;

        /// <summary>
        /// Gets or sets the open value for the point.
        /// </summary>
        /// <value>
        /// Accepts the value as an object. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This is used in open-high-low-close (OHLC) financial charts, indicative of the opening value.
        /// </remarks>
        public object Open { get; set; } = null!;

        /// <summary>
        /// Gets or sets the text for the point.
        /// </summary>
        /// <value>
        /// Accepts the value as a string. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// The text serves to annotate a data point, providing additional context or labels.
        /// </remarks>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the volume value for the point.
        /// </summary>
        /// <value>
        /// Accepts the value as an object. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// Important for charts displaying traded volumes in stock market data.
        /// </remarks>
        public object Volume { get; set; } = null!;

        /// <summary>
        /// Gets or sets the x value for the point.
        /// </summary>
        /// <value>
        /// Accepts the value as an object. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// Establishes the horizontal position of the data point on a chart.
        /// </remarks>
        public object X { get; set; } = null!;

        /// <summary>
        /// Gets or sets the y value for the point.
        /// </summary>
        /// <value>
        /// Accepts the value as an object. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// Determines the vertical position of the data point in a Cartesian chart.
        /// </remarks>
        public object Y { get; set; } = null!;
    }

    /// <summary>
    /// Describes the border properties for chart-related events.
    /// </summary>
    public class ChartEventBorder
    {
        /// <summary>
        /// Gets or sets the color of the border.
        /// </summary>
        /// <value>
        /// Accepts a string value representing the color, which can be a hex or rgba CSS color string.
        /// </value>
        /// <remarks>
        /// Use this property to set the visual color of the border in chart components.
        /// </remarks>
        public string Color { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the width of the border in pixels.
        /// </summary>
        /// <value>
        /// A double value representing the width of the border. The default value is <c>0</c>.
        /// </value>
        /// <remarks>
        /// This property allows customization of the border's thickness, enhancing visibility or design emphasis on chart elements.
        /// </remarks>
        public double Width { get; set; }
    }

    /// <summary>
    /// Represents the data template for chart points.
    /// </summary>
    public class TemplateData
    {
        /// <summary>
        /// Gets or sets the x value for the chart point.
        /// </summary>
        /// <value>
        /// Accepts the value as an object. The default value is <b>null</b>.
        /// </value>
        /// <remarks>
        /// This property defines the horizontal position of the chart point within the chart.
        /// </remarks>
        public object X { get; set; } = null!;

        /// <summary>
        /// Gets or sets the y value for the chart point.
        /// </summary>
        /// <value>
        /// Accepts the value as an object. The default value is <b>null</b>.
        /// </value>
        /// <remarks>
        /// This property defines the vertical position of the chart point within the chart.
        /// </remarks>
        public object Y { get; set; } = null!;

        /// <summary>
        /// Gets or sets the text associated with the chart point.
        /// </summary>
        /// <value>
        /// Accepts the value as a string. The default value is <b>null</b>.
        /// </value>
        /// <remarks>
        /// This property is used to display additional information for the chart point.
        /// </remarks>
        public string Text { get; set; } = string.Empty;
    }

    /// <summary> 
    /// Gets or sets the corner radius of the data point. 
    /// </summary> 
    public class CornerRadius
    {
        /// <summary> 
        /// Gets or sets the corner radius of the top-left corner of the data point. 
        /// </summary> 
        /// <value> 
        /// A double value that represents the corner radius. The default value is the value set to the <c>TopLeft</c> property of <see cref="ChartCornerRadius"/>. 
        /// </value> 
        /// <remarks> 
        /// This property modifies the radius of the top-left corner of the data point. 
        /// </remarks> 
        public double TopLeft { get; set; }

        /// <summary> 
        /// Gets or sets the corner radius of the top-right corner of the data point. 
        /// </summary> 
        /// <value> 
        /// A double value that represents the corner radius. The default value is the value set to the <c>TopRight</c> property of <see cref="ChartCornerRadius"/>. 
        /// </value> 
        /// <remarks> 
        /// This property modifies the radius of the top-right corner of the data point. 
        /// </remarks> 
        public double TopRight { get; set; }

        /// <summary> 
        /// Gets or sets the corner radius of the bottom-left corner of the data point. 
        /// </summary> 
        /// <value> 
        /// A double value that represents the corner radius. The default value is the value set to the <c>BottomLeft</c> property of <see cref="ChartCornerRadius"/>. 
        /// </value> 
        /// <remarks> 
        /// This property modifies the radius of the bottom-left corner of the data point. 
        /// </remarks> 
        public double BottomLeft { get; set; }

        /// <summary> 
        /// Gets or sets the corner radius of the bottom-left corner of the data point. 
        /// </summary> 
        /// <value> 
        /// A double value that represents the corner radius. The default value is the value set to the <c>BottomRight</c> property of <see cref="ChartCornerRadius"/>. 
        /// </value> 
        /// <remarks> 
        /// This property modifies the radius of the bottom-left corner of the data point. 
        /// </remarks> 
        public double BottomRight { get; set; }
    }

    /// <summary>
    /// Represents the data point of a chart, including various properties for customization and data representation.
    /// </summary>
    public class Point : TemplateData
    {
        /// <summary>
        /// Gets or sets the error value for the chart point.
        /// </summary>
        /// <value>
        /// Accepts the value as a double. The default value is <b>0</b>.
        /// </value>
        /// <remarks>
        /// This property is used to represent the error value of the point within a chart.
        /// </remarks>
        public double Error { get; set; }

        /// <summary>
        /// Gets or sets the visibility state of the chart point.
        /// </summary>
        /// <value>
        /// <c>true</c> if the point is visible; otherwise, <c>false</c>. The default value is <b>true</b>.
        /// </value>
        /// <remarks>
        /// This property determines whether the chart point is displayed or hidden in the chart, allowing dynamic visibility control.
        /// </remarks>
        public bool Visible { get; set; } = true;

        /// <summary>
        /// Gets or sets the tooltip format string for the chart point's tooltip display.
        /// </summary>
        /// <value>
        /// Accepts the value as a string. The default value is <b>null</b>.
        /// </value>
        /// <remarks>
        /// This property allows you to define a specific tooltip format for the chart point, enhancing data presentation when hovering over the point.
        /// </remarks>
        public string Tooltip { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the location coordinates of the chart point.
        /// </summary>
        /// <value>
        /// A list of <see cref="ChartEventLocation"/> representing the symbol location values for the point.
        /// </value>
        /// <remarks>
        /// Use this property to specify and retrieve the coordinates where the chart point's symbols are rendered on the chart.
        /// </remarks>
        public List<ChartEventLocation> SymbolLocations { get; set; } = new List<ChartEventLocation>();

        /// <summary>
        /// Gets or sets the visual regions associated with the chart point.
        /// </summary>
        /// <value>
        /// A list of <see cref="Rect"/> representing the chart point's regions.
        /// </value>
        /// <remarks>
        /// Regions define the visual area representing the chart point, often used for hit testing and rendering purposes.
        /// </remarks>
        public List<Rect> Regions { get; set; } = new List<Rect>();

        /// <summary>
        /// Gets or sets the X-Value for the chart point in the chart's coordinate system.
        /// </summary>
        /// <value>
        /// Accepts the value as a double. The default value is <b>0</b>.
        /// </value>
        /// <remarks>
        /// This property is used to define the horizontal position of the chart point, crucial for data plotting in charts.
        /// </remarks>
        public double XValue { get; set; }

        /// <summary>
        /// Gets or sets the Y-Value for the chart point in the chart's coordinate system.
        /// </summary>
        /// <value>
        /// Accepts the value as a double. The default value is <b>0</b>.
        /// </value>
        /// <remarks>
        /// This property is used to define the vertical position of the chart point, important for accurate data representation in charts.
        /// </remarks>
        public double YValue { get; set; }

        /// <summary>
        /// Gets or sets the index of the chart point within its series or dataset.
        /// </summary>
        /// <value>
        /// Accepts the value as an integer. The default value is <b>0</b>.
        /// </value>
        /// <remarks>
        /// The point index is useful for identifying and manipulating the chart point's position within a data series or dataset.
        /// </remarks>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the percentage value associated with the chart point for visualizations.
        /// </summary>
        /// <value>
        /// Accepts the value as a double. The default value is <b>0</b>.
        /// </value>
        /// <remarks>
        /// This property is used to compute and display percentage values in data visualizations involving the chart point.
        /// </remarks>
        public double Percentage { get; set; }

        /// <summary>
        /// Gets or sets the region data for polar and radar chart points, defining their coverage area.
        /// </summary>
        /// <value>
        /// The <see cref="PolarArc"/> object representing the data for polar and radar chart points.
        /// </value>
        /// <remarks>
        /// Region data is used for defining the area covered by a chart point in polar and radar chart types.
        /// </remarks>
        public PolarArc RegionData { get; set; } = null!;

        /// <summary>
        /// Specifies whether the chart point is treated as empty in the visualization.
        /// </summary>
        /// <value>
        /// <c>true</c> if the point is empty; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// This property can be used to identify or exclude empty chart points from rendering or calculations in the chart.
        /// </remarks>
        public bool IsEmpty { get; set; }

        /// <summary>
        /// Gets or sets the minimum value for the chart point, often used in range definitions.
        /// </summary>
        /// <value>
        /// Accepts the value as a double. The default value is <b>0</b>.
        /// </value>
        /// <remarks>
        /// The minimum value is often utilized in statistical visuals where defining a range is necessary.
        /// </remarks>
        public double Minimum { get; set; }

        /// <summary>
        /// Gets or sets the maximum value for the chart point, often used in range definitions.
        /// </summary>
        /// <value>
        /// Accepts the value as a double. The default value is <b>0</b>.
        /// </value>
        /// <remarks>
        /// The maximum value is often utilized in statistical visuals where defining a range is necessary.
        /// </remarks>
        public double Maximum { get; set; }

        /// <summary>
        /// Gets or sets the interior color for the chart point within the chart.
        /// </summary>
        /// <value>
        /// Accepts the string value. The default value is <b>null</b>.
        /// </value>
        /// <remarks>
        /// This property sets the fill color of the chart point, aiding in the visual differentiation of data points.
        /// </remarks>
        public string Interior { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the selection state of the chart point.
        /// </summary>
        /// <value>
        /// <c>true</c> if the point is selected; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// Use this property to perform additional actions based on the selection state of the chart point.
        /// </remarks>
        public bool IsSelected { get; set; }

        /// <summary>
        /// Gets or sets the marker data for the chart point's visualization.
        /// </summary>
        /// <value>
        /// The <see cref="MarkerSettingModel"/> object containing marker data.
        /// </value>
        /// <remarks>
        /// Marker settings are used to customize the appearance of the markers representing chart points.
        /// </remarks>
        public MarkerSettingModel Marker { get; set; } = null!;

        internal List<string> TemplateID { get; set; } = new List<string>();

        internal List<Size> TemplateSize { get; set; } = new List<Size>();

        // To hold the sum of sorting key values in the same point index.
        internal double SumOfSameIndex { get; set; }
    }

    /// <summary>
    /// Represents the polar and radar chart's points data.
    /// </summary>
    /// <remarks>
    /// This class contains the properties defining the angles and radii necessary for plotting points
    /// in polar and radar charts.
    /// </remarks>
    public class PolarArc
    {
        internal PolarArc(double startAngle = 0, double endAngle = 0, double innerRadius = 0, double radius = 0, double currentXPosition = 0)
        {
            StartAngle = startAngle;
            EndAngle = endAngle;
            InnerRadius = innerRadius;
            Radius = radius;
            CurrentXPosition = currentXPosition;
        }

        /// <summary>
        /// Gets or sets the start angle of the arc segment for the point.
        /// </summary>
        /// <value>
        /// A double representing the start angle of the arc in degrees. The default value is <b>0</b>.
        /// </value>
        /// <remarks>
        /// The start angle defines the initial angle from which the arc begins in polar or radar charts.
        /// </remarks>
        [JsonPropertyName("sA")]
        public double StartAngle { get; set; }

        /// <summary>
        /// Gets or sets the end angle of the arc segment for the point.
        /// </summary>
        /// <value>
        /// A double representing the end angle of the arc in degrees. The default value is <b>0</b>.
        /// </value>
        /// <remarks>
        /// The end angle specifies the terminal angle of the arc in polar or radar charts.
        /// </remarks>
        [JsonPropertyName("eA")]
        public double EndAngle { get; set; }

        /// <summary>
        /// Gets or sets the inner radius of the arc segment for the point.
        /// </summary>
        /// <value>
        /// A double representing the inner radius of the arc. The default value is <b>0</b>.
        /// </value>
        /// <remarks>
        /// The inner radius helps define the hollow or inner space within the arc area.
        /// </remarks>
        [JsonPropertyName("iR")]
        public double InnerRadius { get; set; }

        /// <summary>
        /// Gets or sets the radius of the arc segment for the point.
        /// </summary>
        /// <value>
        /// A double representing the radius of the arc segment. The default value is <b>0</b>.
        /// </value>
        /// <remarks>
        /// The radius describes the overall reach from the center to the arc's perimeter in a polar or radar chart.
        /// </remarks>
        [JsonPropertyName("r")]
        public double Radius { get; set; }

        /// <summary>
        /// Gets or sets the current X-Position of the point.
        /// </summary>
        /// <value>
        /// A double representing the current X-Position of the point. The default value is <b>0</b>.
        /// </value>
        /// <remarks>
        /// This property determines the horizontal position of the data point within the polar or radar chart.
        /// </remarks>
        [JsonPropertyName("cP")]
        public double CurrentXPosition { get; set; }
    }

    /// <summary>
    /// Represents the location data for a chart component, used to specify coordinates of elements within the chart.
    /// </summary>
    /// <remarks>
    /// This class provides a flexible way to manage location data.
    /// </remarks>
    public class ChartEventLocation : SymbolLocation
    {
        public ChartEventLocation(double locationX, double locationY)
        {
            X = locationX;
            Y = locationY;
        }
    }

    /// <summary>
    /// Represents the chart marker settings, utilized for customizing the marker appearance in chart components.
    /// </summary>
    public class MarkerSettingModel
    {
        /// <summary>
        /// Gets or sets the visibility of the marker in the chart.
        /// </summary>
        /// <value>
        /// <c>true</c> if the marker is visible; otherwise, <c>false</c>. The default value is <b>true</b>.
        /// </value>
        /// <remarks>
        /// This property determines whether the marker is displayed on the chart, allowing for dynamic visibility control based on chart requirements.
        /// </remarks>
        public bool Visible { get; set; }

        /// <summary>
        /// Gets or sets the shape of the marker.
        /// </summary>
        /// <value>
        /// One of the <see cref="ChartShape"/> enumeration values that specifies the shape of the marker. The default shape is <b>ChartShape.Auto</b>.
        /// </value>
        /// <remarks>
        /// The marker shape provides a visual representation of data points on the chart and can be customized to fit the design requirements.
        /// </remarks>
        public ChartShape Shape { get; set; } = ChartShape.Auto;

        /// <summary>
        /// Gets or sets the height of the marker.
        /// </summary>
        /// <value>
        /// Accepts a double value representing the height of the marker. The default value is <b>5</b>.
        /// </value>
        /// <remarks>
        /// The height of the marker controls the vertical dimension of the marker, contributing to its overall size and appearance.
        /// </remarks>
        public double Height { get; set; } = 5;

        /// <summary>
        /// Gets or sets the width of the marker.
        /// </summary>
        /// <value>
        /// Accepts a double value representing the width of the marker. The default value is <b>5</b>.
        /// </value>
        /// <remarks>
        /// The width of the marker affects its horizontal dimension, contributing to its overall size and appearance.
        /// </remarks>
        public double Width { get; set; } = 5;

        /// <summary>
        /// Gets or sets the border properties for the marker.
        /// </summary>
        /// <value>
        /// A <see cref="ChartEventBorder"/> object representing the border values for the marker.
        /// </value>
        /// <remarks>
        /// The border includes color and width details, enhancing the marker's visual outline and prominence on the chart.
        /// </remarks>
        public ChartEventBorder Border { get; set; } = new ChartEventBorder() { Width = 2 };

        /// <summary>
        /// Gets or sets the fill color of the marker.
        /// </summary>
        /// <value>
        /// Accepts a string value representing the fill color of the marker. The default value is <b>null</b>.
        /// </value>
        /// <remarks>
        /// The fill color provides a distinctive interior color to the marker, enhancing differentiation and visibility on the chart.
        /// </remarks>
        public string Fill { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents the dimension of an element, consisting of width and height properties.
    /// </summary>
    public class Size
    {
        public Size(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public Size()
        {
        }

        /// <summary>
        /// Gets or sets the width of an element.
        /// </summary>
        /// <value>
        /// Accepts a double value. The default value is <b>0</b>.
        /// </value>
        /// <remarks>
        /// This property defines the horizontal span of an element, contributing to its overall shape and layout.
        /// </remarks>
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets the height of an element.
        /// </summary>
        /// <value>
        /// Accepts a double value. The default value is <b>0</b>.
        /// </value>
        /// <remarks>
        /// This property indicates the vertical reach of an element, contributing to its overall shape and layout.
        /// </remarks>
        public double Height { get; set; }

        /// <summary>
        /// Determines whether two <see cref="Size"/> objects have the same dimensions.
        /// </summary>
        /// <param name="a">The first <see cref="Size"/> object to compare.</param>
        /// <param name="b">The second <see cref="Size"/> object to compare.</param>
        /// <returns>
        /// <c>true</c> if both the <see cref="Width"/> and <see cref="Height"/> properties of 
        /// the two <see cref="Size"/> objects are equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator ==(Size a, Size b)
        {
            return a?.Width == b?.Width && a?.Height == b?.Height;
        }

        /// <summary>
        /// Determines whether two <see cref="Size"/> objects have different dimensions.
        /// </summary>
        /// <param name="a">The first <see cref="Size"/> object to compare.</param>
        /// <param name="b">The second <see cref="Size"/> object to compare.</param>
        /// <returns>
        /// <c>true</c> if either the <see cref="Width"/> or <see cref="Height"/> properties of 
        /// the two <see cref="Size"/> objects are not equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator !=(Size a, Size b)
        {
            return a?.Width != b?.Width || a?.Height != b?.Height;
        }

        /// <summary>
        /// Subtracts the dimensions of one <see cref="Size"/> object from another.
        /// </summary>
        /// <param name="a">The first <see cref="Size"/> object to operate on.</param>
        /// <param name="b">The second <see cref="Size"/> object to subtract from the first.</param>
        /// <returns>
        /// A <see cref="Size"/> object representing the difference in dimensions between
        /// the two <see cref="Size"/> objects.
        /// </returns>
        public static Size operator -(Size a, Size b)
        {
            if (a is not null && b is not null)
            {
                return new Size() { Width = a.Width - b.Width, Height = a.Height - b.Height };
            }

            return new Size();
        }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            return Equals(obj);
        }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return GetHashCode();
        }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static Size Subtract(Size left, Size right)
        {
            if (left is not null && right is not null)
            {
                return new Size() { Width = left.Width - right.Width, Height = left.Height - right.Height };
            }

            return new Size();
        }
    }

    /// <summary>
    /// Represents the label location for chart elements, indicating where labels should be positioned within the chart.
    /// </summary>
    /// <remarks>
    /// This class provides a flexible way to manage label location data.
    /// </remarks>
    public class LabelLocation : SymbolLocation
    {
        internal LabelLocation(double locationX, double locationY)
        {
            X = locationX;
            Y = locationY;
        }
    }

    /// <summary>
    /// Represents the selected point X, Y, SeriesIndex, and PointIndex values.
    /// </summary>
    /// <remarks>
    /// This class provides a flexible way to manage selected point data.
    /// </remarks>
    public class PointXY
    {
        /// <summary>
        /// Gets or sets the X value of the point.
        /// </summary>
        /// <value>
        /// An object representing the X coordinate value of the point. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property specifies the horizontal position of a data point within the chart's coordinate system.
        /// </remarks>
        public object X { get; set; } = null!;

        /// <summary>
        /// Gets or sets the Y value of the point.
        /// </summary>
        /// <value>
        /// A double representing the Y coordinate value of the point. The default value is <c>0</c>.
        /// </value>
        /// <remarks>
        /// This property determines the vertical position of a data point within the chart's coordinate system.
        /// </remarks>
        public double Y { get; set; }

        /// <summary>
        /// Gets or sets the reference to the series index.
        /// </summary>
        /// <value>
        /// Accepts the value as an integer. The default value is <b>0</b>.
        /// </value>
        /// <remarks>
        /// The series index helps to identify which series a specific data point belongs to.
        /// </remarks>
        public int? SeriesIndex { get; set; }

        /// <summary>
        /// Gets or sets the reference to the point index.
        /// </summary>
        /// <value>
        /// Accepts the value as an integer. The default value is <b>0</b>.
        /// </value>
        /// <remarks>
        /// The point index specifies the position of the data point within its series.
        /// </remarks>
        public int? PointIndex { get; set; }
    }

    /// <summary>
    /// TODO: Use this class from accumulation chart.
    /// </summary>
    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class BaseEventArgs
    {
        /// <summary>
        /// Defines whether the event should be canceled.
        /// </summary>
        /// <value>
        /// <c>true</c> if the event is to be canceled; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// Use the Cancel property to control whether an event should proceed or not.
        /// </remarks>
        [JsonPropertyName("cancel")]
        public bool Cancel { get; set; }

        /// <summary>
        /// Defines the name of the event.
        /// </summary>
        /// <value>
        /// Accepts the value as a string.
        /// </value>
        /// <remarks>
        /// This property is used to identify the specific event being triggered or handled.
        /// </remarks>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    /// <summary>
    /// Specifies the event arguments for mouse-related events in the chart.
    /// </summary>
    /// <remarks>
    /// This class provides comprehensive mouse interaction data including coordinates,
    /// chart identification, and axis value mappings for the pointer location.
    /// </remarks>
    public class ChartMouseEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChartMouseEventArgs"/> class.
        /// </summary>
        /// <param name="name">The name of the event.</param>
        /// <param name="mouseX">The X coordinate of the mouse pointer.</param>
        /// <param name="mouseY">The Y coordinate of the mouse pointer.</param>
        /// <param name="id">The ID of the chart component.</param>
        internal ChartMouseEventArgs(string name, double mouseX, double mouseY, string id)
        {
            Name = name;
            MouseX = mouseX;
            MouseY = mouseY;
            ID = id;
        }

        /// <summary>
        /// Gets or sets the X coordinate of the mouse pointer.
        /// </summary>
        /// <value>
        /// Accepts the double value.
        /// </value>
        /// <remarks>
        /// Use this property to retrieve the horizontal position of the mouse pointer relative to the chart.
        /// </remarks>
        public double MouseX { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate of the mouse pointer.
        /// </summary>
        /// <value>
        /// Accepts the double value.
        /// </value>
        /// <remarks>
        /// Use this property to retrieve the vertical position of the mouse pointer relative to the chart.
        /// </remarks>
        public double MouseY { get; set; }

        /// <summary>
        /// Gets the ID of the chart component when mouse events are triggered.
        /// </summary>
        /// <value>
        /// The value of the chart component’s id. The default value is chart component’s auto generated id.
        /// </value>
        /// <remarks>
        /// It is a read only value.
        /// </remarks>
        public string ID { get; internal set; }

        /// <summary>
        /// Gets the axis values of the chart component when mouse events are triggered.
        /// </summary>
        /// <value>
        /// The collection of axes data where the key is the axis name, and the value is the axis values for the location. The default value is <b>null</b>.
        /// </value>
        /// <remarks>
        /// This value is read-only and provides information about the axis values for chart interactions.
        /// </remarks>
        /// <example>
        /// <![CDATA[
        /// <SfChart ChartMouseClick = "MouseClick">
        ///    < !--Chart Configuration -->
        /// </SfChart>
        /// @code {
        ///     void MouseClick(ChartMouseEventArgs Args)
        ///     {
        ///         if (Args.AxisData.TryGetValue("PrimaryXAxis", out object xValue) && Args.AxisData.TryGetValue("PrimaryYAxis", out object yValue))
        ///         {
        ///             AddToDataSource(xValue, yValue);
        ///         }
        ///     }
        ///
        ///     void AddToDataSource(object xValue, object yValue)
        ///     {
        ///         // Add the X and Y values to the data source here.
        ///     }
        /// }
        /// ]]>
        /// </example>
        public Dictionary<string, object> AxisData { get; internal set; } = null!;
    }

    /// <summary>
    /// Specifies the event arguments available on chart point render.
    /// </summary>
    /// <remarks>
    /// This class provides customization options for individual data point rendering, including shape, size, color, and border properties.
    /// </remarks>
    public class PointRenderEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PointRenderEventArgs"/> class.
        /// </summary>
        /// <param name="name">The name of the event.</param>
        /// <param name="cancel">Indicates whether the event should be canceled.</param>
        /// <param name="point">The data point being rendered.</param>
        /// <param name="series">The chart series containing the point.</param>
        /// <param name="fill">The fill color for the point.</param>
        /// <param name="border">The border properties for the point.</param>
        /// <param name="width">The width of the point. Default is 0.</param>
        /// <param name="height">The height of the point. Default is 0.</param>
        /// <param name="shape">The marker shape for the point. Default is <see cref="ChartShape.Auto"/>.</param>
        internal PointRenderEventArgs(string name, bool cancel, Point point, ChartSeries series, string fill, ChartEventBorder border, double width = 0, double height = 0, ChartShape shape = ChartShape.Auto)
        {
            Name = name;
            Cancel = cancel;
            Point = point;
            Series = series;
            Shape = shape;
            Fill = fill;
            Border = border;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Gets or sets the current point border.
        /// </summary>
        /// <value>
        /// A <see cref="ChartEventBorder"/> object containing the border color and width.
        /// </value>
        /// <remarks>
        /// This property allows customization of the visual boundary of the data point.
        /// </remarks>
        public ChartEventBorder Border { get; set; }

        /// <summary>
        /// Gets or sets the current point fill color.
        /// </summary>
        /// <value>
        /// Accepts a string value representing a color, such as hex or rgba CSS color strings.
        /// </value>
        /// <remarks>
        /// The fill color enhances the visibility and differentiation of the point.
        /// </remarks>
        public string Fill { get; set; }

        /// <summary>
        /// Gets or sets the current point height.
        /// </summary>
        /// <value>
        /// Accepts the double value.
        /// </value>
        /// <remarks>
        /// Adjusting the height controls the vertical dimension of the rendered point's visual representation.
        /// </remarks>
        public double Height { get; set; }

        /// <summary>
        /// Defines the current point.
        /// </summary>
        /// <value>
        /// A <see cref="Syncfusion.Blazor.Toolkit.Charts.Point"/> object that represents the current point.
        /// </value>
        /// <remarks>
        /// This read-only property provides detailed information about the specific data point involved in rendering.
        /// </remarks>
        public Point Point { get; private set; }

        /// <summary>
        /// Defines the current series of the point.
        /// </summary>
        /// <value>
        /// A <see cref="ChartSeries"/> object that represents the current chart series of point.
        /// </value>
        /// <remarks>
        /// Provides information about the chart series to which the rendering point belongs.
        /// </remarks>
        public ChartSeries Series { get; private set; }

        /// <summary>
        /// Gets or sets the current point marker shape.
        /// </summary>
        /// <value>
        /// A <see cref="ChartShape"/> enumeration value that specifies the marker shape.
        /// </value>
        /// <remarks>
        /// The marker shape determines the visual appearance of the point on the chart.
        /// </remarks>
        public ChartShape Shape { get; set; }

        /// <summary>
        /// Gets or sets the current point width.
        /// </summary>
        /// <value>
        /// Accepts the double value.
        /// </value>
        /// <remarks>
        /// Adjusting the width controls the horizontal dimension of the rendered point's visual representation.
        /// </remarks>
        public double Width { get; set; }

        /// <summary> 
        /// Gets or sets the corner radius of the data point. 
        /// </summary> 
        /// <value>
        /// A <see cref="CornerRadius"/> object that represents the corner radius of the data point.
        /// </value>
        /// <remarks> 
        /// This property is used to modify the appearance of a data point by rounding its corners.
        /// </remarks> 
        public CornerRadius CornerRadius { get; set; } = new CornerRadius();
    }

    /// <summary>
    /// Specifies the event arguments available on chart series render.
    /// </summary>
    /// <remarks>
    /// This class provides customization options for series rendering, including fill color and data modifications.
    /// </remarks>
    public class SeriesRenderEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeriesRenderEventArgs"/> class.
        /// </summary>
        /// <param name="name">The name of the event.</param>
        /// <param name="cancel">Indicates whether the event should be canceled.</param>
        /// <param name="fill">The fill color for the series.</param>
        /// <param name="data">The data collection for the series.</param>
        /// <param name="chartSeries">The chart series being rendered.</param>
        internal SeriesRenderEventArgs(string name, bool cancel, string fill, IEnumerable<object> data, ChartSeries chartSeries)
        {
            Name = name;
            Cancel = cancel;
            Fill = fill;
            Data = data;
            Series = chartSeries;
        }

        /// <summary>
        /// Gets or sets the current series fill color.
        /// </summary>
        /// <value>
        /// Accepts the string value.
        /// </value>
        /// <remarks>
        /// The fill color is used to visually distinguish this series from others.
        /// </remarks>
        public string Fill { get; set; }

        /// <summary>
        /// Gets or sets the current series data.
        /// </summary>
        /// <value>
        /// A collection of objects that represent the series data.
        /// </value>
        /// <remarks>
        /// This property provides access to the data utilized during series rendering.
        /// </remarks>
        public IEnumerable<object> Data { get; set; }

        /// <summary>
        /// Defines the current series.
        /// </summary>
        /// <value>
        /// The <see cref="ChartSeries"/> object representing the current series.
        /// </value>
        /// <remarks>
        /// Provides context about the chart series automatically engaging in rendering.
        /// </remarks>
        public ChartSeries Series { get; private set; }
    }

    /// <summary>
    /// Specifies the event arguments available for the resize event of the chart component.
    /// </summary>
    /// <remarks>
    /// This class provides information about the chart's size before and after a resize operation.
    /// </remarks>
    public class ResizeEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResizeEventArgs"/> class.
        /// </summary>
        /// <param name="name">The name of the event.</param>
        /// <param name="cancel">Indicates whether the event should be canceled.</param>
        /// <param name="current">The current size of the chart after resizing.</param>
        /// <param name="previous">The previous size of the chart before resizing.</param>
        internal ResizeEventArgs(string name, bool cancel, Size current, Size previous)
        {
            Name = name;
            Cancel = cancel;
            CurrentSize = current;
            PreviousSize = previous;
        }

        /// <summary>
        /// Defines the current size of the chart.
        /// </summary>
        /// <value>
        /// A <see cref="Syncfusion.Blazor.Toolkit.Charts.Size"/> object representing the current size of the chart.
        /// </value>
        /// <remarks>
        /// This read-only property provides the updated size of the chart following a resize event.
        /// </remarks>
        public Size CurrentSize { get; private set; }

        /// <summary>
        /// Defines the previous size of the chart.
        /// </summary>
        /// <value>
        /// A <see cref="Syncfusion.Blazor.Toolkit.Charts.Size"/> object representing the previous size of the chart.
        /// </value>
        /// <remarks>
        /// This read-only property provides the chart's size prior to the occurrence of a resize event.
        /// </remarks>
        public Size PreviousSize { get; private set; }
    }

    /// <summary>
    /// Specifies the event arguments available to click on point.
    /// </summary>
    public class PointEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PointEventArgs"/> class.
        /// </summary>
        /// <param name="name">The name of the event.</param>
        /// <param name="cancel">Indicates whether the event should be canceled.</param>
        /// <param name="pageX">The X coordinate on the page.</param>
        /// <param name="pageY">The Y coordinate on the page.</param>
        /// <param name="point">The clicked data point.</param>
        /// <param name="pointIndex">The index of the clicked point.</param>
        /// <param name="series">The series containing the clicked point.</param>
        /// <param name="seriesIndex">The index of the series.</param>
        /// <param name="x">The X coordinate of the click location.</param>
        /// <param name="y">The Y coordinate of the click location.</param>
        /// <param name="isRightClick">Indicates whether the point was clicked with the right mouse button.</param>
        internal PointEventArgs(string name, bool cancel, double pageX, double pageY, Point point, double pointIndex, ChartSeries series, double seriesIndex, double x, double y, bool isRightClick)
        {
            Name = name;
            Cancel = cancel;
            PageX = pageX;
            PageY = pageY;
            Point = point;
            PointIndex = pointIndex;
            Series = series;
            SeriesIndex = seriesIndex;
            X = x;
            Y = y;
            IsRightClick = isRightClick;
        }

        /// <summary>
        /// Gets the X-coordinate of the point on the page.
        /// </summary>
        /// <value>
        /// Returns a double value.
        /// </value>
        /// <remarks>
        /// This read-only property provides the horizontal position of the click event relative to the entire page.
        /// </remarks>
        public double PageX { get; private set; }

        /// <summary>
        /// Gets the Y-coordinate of the point on the page.
        /// </summary>
        /// <value>
        /// Returns a double value.
        /// </value>
        /// <remarks>
        /// This read-only property provides the vertical position of the click event relative to the entire page.
        /// </remarks>
        public double PageY { get; private set; }

        /// <summary>
        /// Gets the point that was clicked.
        /// </summary>
        /// <value>
        /// A <see cref="Syncfusion.Blazor.Toolkit.Charts.Point"/> object representing the clicked data point.
        /// </value>
        /// <remarks>
        /// This read-only property identifies the specific chart data point that was subjected to a click event.
        /// </remarks>
        public Point Point { get; private set; }

        /// <summary>
        /// Defines the point index.
        /// </summary>
        /// <value>
        /// Returns a double value.
        /// </value>
        /// <remarks>
        /// This read-only property provides the position of the clicked point within its series.
        /// </remarks>
        public double PointIndex { get; private set; }

        /// <summary>
        /// Defines the current series.
        /// </summary>
        /// <value>
        /// The <see cref="ChartSeries"/> containing the point.
        /// </value>
        /// <remarks>
        /// This read-only property provides context about the series from which the clicked point originates.
        /// </remarks>
        public ChartSeries Series { get; private set; }

        /// <summary>
        /// Defines the series index.
        /// </summary>
        /// <value>
        /// Returns a double value.
        /// </value>
        /// <remarks>
        /// This read-only property identifies the series associated with the clicked point.
        /// </remarks>
        public double SeriesIndex { get; private set; }

        /// <summary>
        /// Gets the X-coordinate of the clicked location.
        /// </summary>
        /// <value>
        /// Returns a double value.
        /// </value>
        /// <remarks>
        /// This property reflects the horizontal position of the mouse pointer when the click occurred.
        /// </remarks>
        public double X { get; private set; }

        /// <summary>
        /// Gets the Y-coordinate of the clicked location.
        /// </summary>
        /// <value>
        /// Returns a double value.
        /// </value>
        /// <remarks>
        /// This property reflects the vertical position of the mouse pointer when the click occurred.
        /// </remarks>
        public double Y { get; private set; }

        /// <summary>
        /// Defines whether the point is clicked by right click or not.
        /// </summary>
        /// <value>
        /// <c>true</c>, if the point is clicked by right click; Otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// Use this property to check if the click action was triggered by the right mouse button.
        /// </remarks>
        public bool IsRightClick { get; private set; }
    }

    /// <summary>
    /// Defines the information of a chart point.
    /// </summary>
    /// <remarks>
    /// This class encapsulates details about a data point, including its index, coordinates, and associated series information.
    /// </remarks>
    public class PointInfo
    {
        /// <summary>
        /// Gets or sets the point index.
        /// </summary>
        /// <value>
        /// Accepts the double value.
        /// </value>
        /// <remarks>
        /// Use this property to access or specify the position of the point within its dataset.
        /// </remarks>
        public double PointIndex { get; set; }

        /// <summary>
        /// Gets or sets the point text.
        /// </summary>
        /// <value>
        /// Accepts the string value.
        /// </value>
        /// <remarks>
        /// The text can be used for labeling the point in a tooltip or legend.
        /// </remarks>
        public string PointText { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the x-coordinate value of the point.
        /// </summary>
        /// <value> 
        /// Accepts the object value.
        /// </value>
        /// <remarks>
        /// This property is crucial for positioning the point on the chart's X-axis.
        /// </remarks>
        public object PointX { get; set; } = null!;

        /// <summary>
        /// Gets or sets the y-coordinate value of the point.
        /// </summary>
        /// <value>
        /// Accepts the object value.
        /// </value>
        /// <remarks>
        /// This property is crucial for positioning the point on the chart's Y-axis.
        /// </remarks>
        public object PointY { get; set; } = null!;

        /// <summary>
        /// Gets or sets the chart series index.
        /// </summary>
        /// <value>
        /// Accepts the double value.
        /// </value>
        /// <remarks>
        /// The series index helps to identify within a set of data series on the chart.
        /// </remarks>
        public double SeriesIndex { get; set; }

        /// <summary>
        /// Gets or sets the chart series name.
        /// </summary>
        /// <value>
        /// Accepts the string value.
        /// </value>
        /// <remarks>
        /// The series name is used to denote the identity of the data series visually represented by the point.
        /// </remarks>
        public string SeriesName { get; set; } = string.Empty;

        /// <summary>
        /// Gets the total y value of a data point in the stacking series types chart. This property allows for the inclusion of the cumulative sum of the data points in the chart. It can be used to customize the <see cref="Syncfusion.Blazor.Toolkit.Charts.TooltipRender"/> event to display the total value alongside other data points information.
        /// </summary>
        /// <value>
        /// The sum of the same point indexed y values for the stacking series type chart. The default value is <b>null</b>.
        /// </value>
        /// <remarks>
        /// This is a read-only property and is applicable only for stacking series types.
        /// </remarks>
        public double StackedTotalValue { get; internal set; }
    }

    /// <summary>
    /// Specifies the event arguments available for the tooltip render events in the chart component.
    /// </summary>
    /// <remarks>
    /// This class provides customization options for tooltip content, styling, and data display.
    /// </remarks>
    public class TooltipRenderEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TooltipRenderEventArgs"/> class.
        /// </summary>
        /// <param name="name">The name of the event.</param>
        /// <param name="cancel">Indicates whether the event should be canceled.</param>
        /// <param name="data">The point information for the tooltip.</param>
        /// <param name="headerText">The header text for the tooltip.</param>
        /// <param name="point">The current tooltip point.</param>
        /// <param name="series">The current tooltip series.</param>
        /// <param name="text">The text content for the tooltip.</param>
        /// <param name="textStyle">The text style for the tooltip.</param>
        internal TooltipRenderEventArgs(string name, bool cancel, PointInfo data, string headerText, object point, object series, string text, ChartDefaultFont textStyle)
        {
            Name = name;
            Cancel = cancel;
            Data = data;
            HeaderText = headerText;
            Point = point;
            Series = series;
            Text = text;
            TextStyle = textStyle;
        }

        /// <summary>
        /// Defines the point information.
        /// </summary>
        /// <value>
        /// The <see cref="PointInfo"/> object representing the point information.
        /// </value>
        /// <remarks>
        /// This read-only property gives information on the data point for which the tooltip is being rendered.
        /// </remarks>
        public PointInfo Data { get; private set; }

        /// <summary>
        /// Gets or sets the header text for the tooltip.
        /// </summary>
        /// <value>
        /// Accepts the string value.
        /// </value>
        /// <remarks>
        /// Use this property to display additional context or the name of the series in the tooltip.
        /// </remarks>
        public string HeaderText { get; set; }

        /// <summary>
        /// Defines current tooltip point.
        /// </summary>
        /// <value>
        /// An object representing the current tooltip point.
        /// </value>
        /// <remarks>
        /// This read-only property provides the data point the tooltip is currently being shown for.
        /// </remarks>
        public object Point { get; private set; }

        /// <summary>
        /// Defines current tooltip series.
        /// </summary>
        /// <value>
        /// An object representing the current tooltip series.
        /// </value>
        /// <remarks>
        /// This read-only property provides the chart series the tooltip is associated with.
        /// </remarks>
        public object Series { get; private set; }

        /// <summary>
        /// Gets or sets the tooltip text collections.
        /// </summary>
        /// <value>
        /// Accepts the string value.
        /// </value>
        /// <remarks>
        /// Customize or access this property to alter the text displayed in the tooltip.
        /// </remarks>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the tooltip text style.
        /// </summary>
        /// <value>
        /// The <see cref="ChartDefaultFont"/> object describing the text style.
        /// </value>
        /// <remarks>
        /// Use this property to modify the font family, size, and other attributes of tooltip text.
        /// </remarks>
        public ChartDefaultFont TextStyle { get; set; }
    }

    /// <summary>
    /// Specifies the event arguments available for the legend item click events in the chart component.
    /// </summary>
    public class LegendClickEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LegendClickEventArgs"/> class.
        /// </summary>
        /// <param name="name">The name of the event.</param>
        /// <param name="cancel">Indicates whether the event should be canceled.</param>
        /// <param name="chart">The chart instance.</param>
        /// <param name="legendShape">The shape of the legend item.</param>
        /// <param name="series">The series associated with the legend item.</param>
        /// <param name="legendText">The text of the legend item.</param>
        internal LegendClickEventArgs(string name, bool cancel, SfChart chart, LegendShape legendShape, ChartSeries series, string legendText)
        {
            Name = name;
            Cancel = cancel;
            Chart = chart;
            LegendShape = legendShape;
            Series = series;
            LegendText = legendText;
        }

        /// <summary>
        /// Defines the instance of the chart.
        /// </summary>
        /// <value>
        /// A <see cref="SfChart"/> object that represents the instance of the chart.
        /// </value>
        /// <remarks>
        /// This read-only property provides access to the chart component that generated the legend click event.
        /// </remarks>
        public SfChart Chart { get; private set; }

        /// <summary>
        /// Defines the Legend shape.
        /// </summary>
        /// <value>
        /// A <see cref="LegendShape"/> value that defines the shape of the legend.
        /// </value>
        /// <remarks>
        /// Provides useful visual information regarding the legend item's appearance that was clicked.
        /// </remarks>
        public LegendShape LegendShape { get; set; }

        /// <summary>
        /// Defines the current series.
        /// </summary>
        /// <value>
        /// A <see cref="ChartSeries"/> object that represents the current series.
        /// </value>
        /// <remarks>
        /// This read-only property can be used to retrieve the series associated with the legend item click.
        /// </remarks>
        public ChartSeries Series { get; private set; }

        /// <summary>
        /// Defines the current legend text.
        /// </summary>
        /// <value>
        /// Accepts the string value.
        /// </value>
        /// <remarks>
        /// Allows users to programmatically access the text label displayed on the legend item click.
        /// </remarks>
        public string LegendText { get; private set; }
    }

    /// <summary>
    /// Specifies the event arguments available for the legend render events in the chart component.
    /// </summary>
    public class LegendRenderEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Gets or sets the legend text.
        /// </summary>
        /// <value>
        /// Accepts the string value.
        /// </value>
        /// <remarks>
        /// This property allows customization of the label that appears in the chart legend.
        /// </remarks>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the legend color.
        /// </summary>
        /// <value>
        /// Accepts the string value.
        /// </value>
        /// <remarks>
        /// This property determines the color used in the legend to represent chart elements.
        /// </remarks>
        public string Fill { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the legend shape.
        /// </summary>
        /// <value>
        /// A <see cref="LegendShape"/> value that defines the shape of the legend.
        /// </value>
        /// <remarks>
        /// Use this property to specify the graphical representation form used in legend items.
        /// </remarks>
        public LegendShape Shape { get; set; }

        /// <summary>
        /// Gets or sets the marker text.
        /// </summary>
        /// <value>
        /// A <see cref="ChartShape"/> value that defines the shape of the marker.
        /// </value>
        /// <remarks>
        /// This property allows customization of the marker's appearance within the legend.
        /// </remarks>
        public ChartShape MarkerShape { get; set; }
    }

    /// <summary>
    /// Specifies the event arguments available for on axis label render events in the chart component.
    /// </summary>
    public class AxisLabelRenderEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AxisLabelRenderEventArgs"/> class.
        /// </summary>
        /// <param name="name">The name of the event.</param>
        /// <param name="cancel">A value indicating whether the event operation can be canceled.</param>
        /// <param name="axis">The axis associated with the label.</param>
        /// <param name="text">The label text.</param>
        /// <param name="value">The label value.</param>
        /// <param name="labelStyle">The font style of the label.</param>
        internal AxisLabelRenderEventArgs(string name, bool cancel, ChartAxis axis, string text, double value, ChartDefaultFont labelStyle)
        {
            Name = name;
            Cancel = cancel;
            Axis = axis;
            Text = text;
            Value = value;
            LabelStyle = labelStyle;
        }

        /// <summary>
        /// Defines the current axis.
        /// </summary>
        /// <value>
        /// A <see cref="ChartAxis"/> object that represents the current axis.
        /// </value>
        /// <remarks>
        /// This read-only property provides details about the axis where the label is applied.
        /// </remarks>
        public ChartAxis Axis { get; private set; }

        /// <summary>
        /// Gets or sets the axis current label text.
        /// </summary>
        /// <value>
        /// Accepts the string value.
        /// </value>
        /// <remarks>
        /// Modify this property to tailor the text content of the axis label on rendering.
        /// </remarks>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets axis current label value.
        /// </summary>
        /// <value>
        /// Returns a double value.
        /// </value>
        /// <remarks>
        /// This property reflects the numerical position where the label appears on the axis.
        /// </remarks>
        public double Value { get; set; }

        /// <summary>
        /// Gets or sets the axis current label font style.
        /// </summary>
        /// <value>
        /// A <see cref="ChartDefaultFont"/> that defines the font style of the current axis labels.
        /// </value>
        /// <remarks>
        /// Use this property to modify the visual appearance of labels in terms of font size, type, etc.
        /// </remarks>
        public ChartDefaultFont LabelStyle { get; set; }
    }

    /// <summary>
    /// Specifies the event arguments available for on range calculated events in the chart component.
    /// </summary>
    public class AxisRangeCalculatedEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AxisRangeCalculatedEventArgs"/> class.
        /// </summary>
        /// <param name="name">The name of the event.</param>
        /// <param name="cancel">A value indicating whether the event operation can be canceled.</param>
        /// <param name="minimum">The minimum range value.</param>
        /// <param name="maximum">The maximum range value.</param>
        /// <param name="interval">The interval between range values.</param>
        /// <param name="bounds">The bounding rectangle of the axis.</param>
        /// <param name="axisName">The name of the axis.</param>
        internal AxisRangeCalculatedEventArgs(string name, bool cancel, double minimum, double maximum, double interval, Rect bounds, string axisName)
        {
            Name = name;
            Cancel = cancel;
            Minimum = minimum;
            Maximum = maximum;
            Interval = interval;
            Bounds = bounds;
            AxisName = axisName;
        }

        /// <summary>
        /// Defines the minimum range of the axis.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> value representing the minimum range of the axis.
        /// </value>
        /// <remarks>
        /// This property provides the minimum value of the axis range calculated during the event.
        /// </remarks>
        public double Minimum { get; set; }

        /// <summary>
        /// Defines the maximum range of the axis.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> value representing the maximum range of the axis.
        /// </value>
        /// <remarks>
        /// This property provides the maximum value of the axis range calculated during the event.
        /// </remarks>
        public double Maximum { get; set; }

        /// <summary>
        /// Defines the interval of the axis range.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> value representing the interval of the axis range.
        /// </value>
        /// <remarks>
        /// This property indicates the interval between values in the axis range calculated during the event.
        /// </remarks>
        public double Interval { get; set; }

        /// <summary>
        /// Define current axis bounds.
        /// </summary>
        /// <value>
        /// A <see cref="Rect"/> object that represents the current axis bounds.
        /// </value>
        /// <remarks>
        /// This property is a read-only value that provides the bounding rectangle for the axis based on the calculated range.
        /// </remarks>
        public Rect Bounds { get; private set; }

        /// <summary>
        /// Defines current axis name.
        /// </summary>
        /// <value>
        /// Accepts the string value.
        /// </value>
        /// <remarks>
        /// This property is a read-only value that provides the name of the axis for which the range has been calculated.
        /// </remarks>
        public string AxisName { get; private set; }
    }

    /// <summary>
    /// Specifies the event arguments available for the text render events in the chart component.
    /// </summary>
    public class TextRenderEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextRenderEventArgs"/> class.
        /// </summary>
        /// <param name="name">The name of the event.</param>
        /// <param name="cancel">A value indicating whether the event operation can be canceled.</param>
        /// <param name="series">The chart series being rendered.</param>
        /// <param name="point">The data point associated with the text.</param>
        /// <param name="location">The location of the text in the chart.</param>
        /// <param name="text">The text content.</param>
        /// <param name="color">The text color.</param>
        /// <param name="border">The border properties.</param>
        /// <param name="template">The custom render template.</param>
        /// <param name="font">The font style.</param>
        /// <param name="seriesIndex">The index of the series.</param>
        internal TextRenderEventArgs(string name, bool cancel, ChartSeries series, Point point, LabelLocation location, string text, string color, BorderModel border, RenderFragment<ChartDataPointInfo> template, ChartDefaultFont font, int seriesIndex)
        {
            Name = name;
            Cancel = cancel;
            Series = series;
            Point = point;
            Location = location;
            Text = text;
            Color = color;
            Border = border;
            Template = template;
            Font = font;
            SeriesIndex = seriesIndex;
        }

        /// <summary>
        /// Defines the current series.
        /// </summary>
        /// <value>
        /// A <see cref="ChartSeries"/> object that represents the current series.
        /// </value>
        /// <remarks>
        /// This property provides the chart series associated with the text being rendered.
        /// </remarks> 
        public ChartSeries Series { get; private set; }

        /// <summary>
        /// Defines the text point.
        /// </summary>
        /// <value>
        /// A <see cref="Point"/> object that represents the location of the text.
        /// </value>
        /// <remarks>
        /// This property provides the data point associated with the text being rendered.
        /// </remarks>
        public Point Point { get; private set; }

        /// <summary>
        /// Defines the text location.
        /// </summary>
        /// <value>
        /// A <see cref="LabelLocation"/> object that specifies the text's location on the chart.
        /// </value>
        /// <remarks>
        /// This property provides the coordinates for displaying the text within the chart area.
        /// </remarks>
        public LabelLocation Location { get; private set; }

        /// <summary>
        /// Gets or sets the text to be rendered on the chart.
        /// </summary>
        /// <value>
        /// Accepts the string value.
        /// </value>
        /// <remarks>
        /// This property contains the text content to be displayed during the rendering event.
        /// </remarks>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        /// <value>
        /// Accepts string value.
        /// </value>
        /// <remarks>
        /// This property defines the visual appearance of the text color.
        /// </remarks>
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets the information of the border.
        /// </summary>
        /// <value>
        /// A <see cref="BorderModel"/> object that defines the information of the border.
        /// </value>
        /// <remarks>
        /// Use this property to modify or retrieve border characteristics, such as color, width, and style, as defined in the <see cref="BorderModel"/>.
        /// </remarks>
        public BorderModel Border { get; set; }

        /// <summary>
        /// Gets or sets the template used for the custom rendering of data points.
        /// </summary>
        /// <value>
        /// A <see cref="RenderFragment{ChartDataPointInfo}"/> representing the custom template.
        /// </value>
        /// <remarks>
        /// This property allows the user to define a custom rendering template for data points.
        /// </remarks>
        public RenderFragment<ChartDataPointInfo> Template { get; set; }

        /// <summary>
        /// Gets or sets the information of the font.
        /// </summary>
        /// <value>
        /// A <see cref="ChartDefaultFont"/> object representing the font style of the text.
        /// </value>
        /// <remarks>
        /// This property determines the font style used for rendering the text on the chart.
        /// </remarks>
        public ChartDefaultFont Font { get; set; }

        /// <summary>
        /// Defines the current series index.
        /// </summary>
        /// <value>
        /// Accepts an integer value.
        /// </value>
        /// <remarks>
        /// This property provides the index of the series associated with the text rendering.
        /// </remarks>
        public int SeriesIndex { get; private set; }
    }

    /// <summary>
    /// Specifies the event arguments available for the multi level label render events in the chart component.
    /// </summary>
    public class AxisMultiLabelRenderEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AxisMultiLabelRenderEventArgs"/> class.
        /// </summary>
        /// <param name="name">The name of the event.</param>
        /// <param name="cancel">A value indicating whether the event operation can be canceled.</param>
        /// <param name="axis">The axis containing the multi-level labels.</param>
        /// <param name="customAttributes">Custom attributes associated with the labels.</param>
        /// <param name="text">The label text.</param>
        /// <param name="textStyle">The font style for the text.</param>
        /// <param name="alignment">The text alignment.</param>
        internal AxisMultiLabelRenderEventArgs(string name, bool cancel, ChartAxis axis, object customAttributes, string text, ChartDefaultFont textStyle, Alignment alignment)
        {
            Name = name;
            Cancel = cancel;
            Axis = axis;
            CustomAttributes = customAttributes;
            Text = text;
            TextStyle = textStyle;
            Alignment = alignment;
        }

        /// <summary>
        /// Gets or sets the text alignment for multi labels.
        /// </summary>
        /// <value>
        /// An <see cref="Alignment"/> value that defines the text alignment for multi-labels.
        /// </value>
        /// <remarks>
        /// This property allows you to specify how the text of multi-level labels should be aligned.
        /// </remarks>
        public Alignment Alignment { get; set; }

        /// <summary>
        /// Defines the current axis.
        /// </summary>
        /// <value>
        /// A <see cref="ChartAxis"/> object that represents the current axis.
        /// </value>
        /// <remarks>
        /// This property provides the axis that contains the multi-level labels being rendered.
        /// </remarks>
        public ChartAxis Axis { get; private set; }

        /// <summary>
        /// Defines custom objects for multi labels.
        /// </summary>
        /// <value>
        /// An object that represents the custom objects for multi-labels.
        /// </value>
        /// <remarks>
        /// This property allows you to retrieve any additional custom attributes specified for the multi-level labels.
        /// </remarks>
        public object CustomAttributes { get; private set; }

        /// <summary>
        /// Gets or sets the current axis label text.
        /// </summary>
        /// <value>
        /// Accepts the string value.
        /// </value>
        /// <remarks>
        /// This property allows you to modify or access the text content of the multi-level label during rendering.
        /// </remarks>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the font style for multi labels.
        /// </summary>
        /// <value>
        /// A <see cref="ChartDefaultFont"/> object that specifies the font style for the multi-level labels.
        /// </value>
        /// <remarks>
        /// This property provides control over the font styling of the multi-level labels.
        /// </remarks>
        public ChartDefaultFont TextStyle { get; set; }
    }

    /// <summary>
    /// Specifies the event arguments available for on scroll changed events in the chart component.
    /// </summary>
    public class ScrollEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollEventArgs"/> class.
        /// </summary>
        /// <param name="name">The name of the event.</param>
        /// <param name="axis">The axis being scrolled.</param>
        /// <param name="currentRange">The current scroll range.</param>
        /// <param name="previousRange">The previous scroll range.</param>
        /// <param name="previousZoomFactor">The previous zoom factor.</param>
        /// <param name="previousZoomPosition">The previous zoom position.</param>
        /// <param name="range">The current visible range.</param>
        /// <param name="zoomFactor">The current zoom factor.</param>
        /// <param name="zoomPosition">The current zoom position.</param>
        internal ScrollEventArgs(string name, ChartAxis axis, ChartAxisScrollbarSettingsRange currentRange, VisibleRangeModel previousRange, double previousZoomFactor, double previousZoomPosition, VisibleRangeModel range, double zoomFactor, double zoomPosition)
        {
            Name = name;
            Axis = axis;
            CurrentRange = currentRange;
            PreviousRange = previousRange;
            PreviousZoomFactor = previousZoomFactor;
            PreviousZoomPosition = previousZoomPosition;
            Range = range;
            ZoomFactor = zoomFactor;
            ZoomPosition = zoomPosition;
        }

        /// <summary>
        /// Defines the current scroll axis.
        /// </summary>
        /// <value>
        /// The <see cref="ChartAxis"/> object representing the current scroll axis.
        /// </value>
        /// <remarks>
        /// This property provides the scroll axis that is affected during the scrolling event.
        /// </remarks>
        public ChartAxis Axis { get; private set; }

        /// <summary>
        /// Gets or sets the current range of the axis.
        /// </summary>
        /// <value>
        /// An instance of the <see cref="ChartAxisScrollbarSettingsRange"/> class representing the current range.
        /// </value>
        /// <remarks>
        /// This property allows access to the current range settings as the axis is being scrolled.
        /// </remarks>
        public ChartAxisScrollbarSettingsRange CurrentRange { get; set; }

        /// <summary>
        /// Gets or sets the previous range of the axis.
        /// </summary>
        /// <value>
        /// An instance of the <see cref="ChartAxisScrollbarSettingsRange"/> class representing the previous range.
        /// </value>
        /// <remarks>
        /// This property provides details about the range settings of the axis prior to the scroll event for comparison purposes.
        /// </remarks>
        public ChartAxisScrollbarSettingsRange PreviousAxisRange { get; set; } = null!;

        /// <summary>
        /// Gets or sets the previous visible range of the axis.
        /// </summary>
        /// <value>
        /// An instance of the <see cref="VisibleRangeModel"/> class representing the previous range.
        /// </value>
        /// <remarks>
        /// This property provides details about the previous visible range settings.
        /// </remarks>
        public VisibleRangeModel PreviousRange { get; set; }

        /// <summary>
        /// Gets or sets the previous zoom factor.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> value representing the previous zoom factor.
        /// </value>
        /// <remarks>
        /// This property allows tracking of zoom level changes during scrolling.
        /// </remarks>
        public double PreviousZoomFactor { get; set; }

        /// <summary>
        /// Gets or sets the previous Zoom Position.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> value representing the previous zoom position.
        /// </value>
        /// <remarks>
        /// This property is used to record the scroll axis's position before changes occurred.
        /// </remarks>
        public double PreviousZoomPosition { get; set; }

        /// <summary>
        /// Gets or sets the current visible range.
        /// </summary>
        /// <value>
        /// An instance of the <see cref="VisibleRangeModel"/> class representing the current visible range.
        /// </value>
        /// <remarks>
        /// This property reflects updates to the axis's visible range as it is being scrolled.
        /// </remarks>
        public VisibleRangeModel Range { get; set; }

        /// <summary>
        /// Gets or sets the current zoom factor.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> value representing the current zoom factor.
        /// </value>
        /// <remarks>
        /// This property informs the current zoom level applied to the axis during scrolling.
        /// </remarks>
        public double ZoomFactor { get; set; }

        /// <summary>
        /// Gets or sets the current Zoom Position.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> value representing the current zoom position.
        /// </value>
        /// <remarks>
        /// This property indicates the axis's position affected by the scroll operation.
        /// </remarks>
        public double ZoomPosition { get; set; }
    }

    /// <summary>
    /// Specifies the event arguments available for the zooming events in the chart component.
    /// </summary>
    public class ZoomingEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Defines the collection of axis.
        /// </summary>
        /// <value>
        /// A list of <see cref="AxisData"/> representing the collection of axes.
        /// </value>
        /// <remarks>
        /// This property provides details about each axis.
        /// </remarks>
        [JsonPropertyName("axisCollection")]
        public List<AxisData> AxisCollection { get; set; } = null!;
    }

    /// <summary>
    /// Specifies the axis information in the chart component.
    /// </summary>
    public class AxisData
    {
        /// <summary>
        /// Gets or sets the axis name.
        /// </summary>
        /// <value>
        /// A string representing the name of the axis.
        /// </value>
        /// <remarks>
        /// This property is used to uniquely identify the axis within the chart.
        /// </remarks>
        [JsonPropertyName("axisName")]
        public string AxisName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the axis range.
        /// </summary>
        /// <value>
        /// A <see cref="VisibleRangeModel"/> object representing the axis range.
        /// </value>
        /// <remarks>
        /// This property defines the minimum and maximum axis range values displayed on the axis.
        /// </remarks>
        [JsonPropertyName("axisRange")]
        public VisibleRangeModel AxisRange { get; set; } = null!;

        /// <summary>
        /// Gets or sets the value of the zoom factor.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> representing the zoom factor of the axis.
        /// </value>
        /// <remarks>
        /// This property indicates the level of zoom applied to the axis.
        /// </remarks>
        [JsonPropertyName("zoomFactor")]
        public double ZoomFactor { get; set; }

        /// <summary>
        /// Gets or sets the position value of the zoom.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> representing the zoom position of the axis.
        /// </value>
        /// <remarks>
        /// This property specifies the zoom position details where the zoom focus is centered along the axis.
        /// </remarks>
        [JsonPropertyName("zoomPosition")]
        public double ZoomPosition { get; set; }
    }

    /// <summary>
    /// Specifies the event arguments available for the shared tooltip render events in the chart component.
    /// </summary>
    public class SharedTooltipRenderEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SharedTooltipRenderEventArgs"/> class.
        /// </summary>
        /// <param name="name">The name of the event.</param>
        /// <param name="cancel">A value indicating whether the event operation can be canceled.</param>
        /// <param name="text">The tooltip text lines.</param>
        /// <param name="textStyle">The font style for the tooltip text.</param>
        /// <param name="headerText">The tooltip header text.</param>
        /// <param name="data">The data points associated with the tooltip.</param>
        internal SharedTooltipRenderEventArgs(string name, bool cancel, List<string> text, ChartDefaultFont textStyle, string headerText, List<PointInfo> data)
        {
            Name = name;
            Cancel = cancel;
            Text = text;
            TextStyle = textStyle;
            HeaderText = headerText;
            Data = data;
        }

        /// <summary>
        /// Gets or sets the text for the shared tooltip.
        /// </summary>
        /// <value>
        /// Accepts a list of string values representing the text content of the shared tooltip.
        /// </value>
        /// <remarks>
        /// Use this property to define or access the text content displayed in the shared tooltip.
        /// </remarks>
        public List<string> Text { get; set; }

        /// <summary>
        /// Gets the text style for the shared tooltip.
        /// </summary>
        /// <value>
        /// A <see cref="ChartDefaultFont"/> object representing the text style for the shared tooltip.
        /// </value>
        /// <remarks>
        /// This is a read-only property specifying the font style used in the shared tooltip.
        /// </remarks>
        public ChartDefaultFont TextStyle { get; private set; }

        /// <summary>
        /// Gets or sets the text of the header for the shared tooltip.
        /// </summary>
        /// <value>
        /// Accepts a string value representing the header text of the shared tooltip.
        /// </value>
        /// <remarks>
        /// The header text provides context or a title for the shared tooltip.
        /// </remarks>
        public string HeaderText { get; set; }

        /// <summary>
        /// Gets the information of the data points associated with the shared tooltip.
        /// </summary>
        /// <value>
        /// A list of <see cref="PointInfo"/> representing detailed information about the data points relevant to the shared tooltip.
        /// </value>
        /// <remarks>
        /// This read-only property provides comprehensive information about the data points that are currently highlighted by the shared tooltip.
        /// </remarks>
        public List<PointInfo> Data { get; private set; }
    }

    /// <summary>
    /// Specifies the event arguments available for the <c>OnCrosshairMove</c> event in the chart component.
    /// </summary>
    public class CrosshairMoveEventArgs
    {
        /// <summary>
        /// Gets or sets the axis information on crosshair move.
        /// </summary>
        /// <value>
        /// A list of <see cref="CrosshairAxisInfo"/> objects that define the axis information on crosshair move.
        /// </value>
        /// <remarks>
        /// Use this property to access or modify the axis information during the crosshair move event.
        /// </remarks>
        public List<CrosshairAxisInfo> AxisInfo { get; set; } = new List<CrosshairAxisInfo>();
    }

    /// <summary>
    /// Specifies the event arguments available for the selection changed events in the chart component.
    /// </summary>
    public class SelectionCompleteEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Gets or sets the currently selected data X, Y values.
        /// </summary>
        /// <value>
        /// A list of <see cref="PointXY"/> representing the selected data values.
        /// </value>
        /// <remarks>
        /// Use this property to access the details of data points selected by the user.
        /// </remarks>
        public List<PointXY> SelectedDataValues { get; set; } = null!;
    }

    /// <summary>
    /// Specifies the event arguments available for the multi-level label click events in the chart component.
    /// </summary>
    public class MultiLevelLabelClickEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiLevelLabelClickEventArgs"/> class.
        /// </summary>
        /// <param name="name">The name of the event.</param>
        /// <param name="cancel">A value indicating whether the event operation can be canceled.</param>
        /// <param name="text">The label text.</param>
        /// <param name="axis">The axis containing the clicked label.</param>
        /// <param name="customAttributes">Custom attributes associated with the label.</param>
        /// <param name="end">The end value of the label range.</param>
        /// <param name="level">The hierarchy level of the label.</param>
        /// <param name="start">The start value of the label range.</param>
        internal MultiLevelLabelClickEventArgs(string name, bool cancel, string text, ChartAxis axis, object customAttributes, object end, double level, object start)
        {
            Name = name;
            Cancel = cancel;
            Text = text;
            Axis = axis;
            CustomAttributes = customAttributes;
            End = end;
            Level = level;
            Start = start;
        }

        /// <summary>
        /// Gets the current axis associated with the multi-level label.
        /// </summary>
        /// <value>
        /// A <see cref="ChartAxis"/> object representing the current axis.
        /// </value>
        /// <remarks>
        /// This is a read-only property providing access to the axis where the label is clicked.
        /// </remarks>
        public ChartAxis Axis { get; private set; }

        /// <summary>
        /// Gets the custom attributes associated with the multi-level label.
        /// </summary>
        /// <value>
        /// An object representing the custom attributes for the multi-level labels.
        /// </value>
        /// <remarks>
        /// This is a read-only property providing additional attributes related to the labels.
        /// </remarks>
        public object CustomAttributes { get; private set; }

        /// <summary>
        /// Gets the end value of the multi-level labels.
        /// </summary>
        /// <value>
        /// An object representing the end value of the multi-level labels.
        /// </value>
        /// <remarks>
        /// This property is useful for determining where a range of multi-level labels ends on an axis. 
        /// </remarks>
        public object End { get; private set; }

        /// <summary>
        /// Gets the current level of the multi-level labels.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> representing the level of the multi-level labels.
        /// </value>
        /// <remarks>
        /// This property indicates the specific level of the multi-level labels currently being accessed. 
        /// </remarks>
        public double Level { get; private set; }

        /// <summary>
        /// Gets the start value of the multi-level labels.
        /// </summary>
        /// <value>
        /// An object representing the start value of the multi-level labels.
        /// </value>
        /// <remarks>
        /// This property provides the starting point or initial value used for the multi-level labels on an axis.
        /// </remarks>
        public object Start { get; private set; }

        /// <summary>
        /// Gets the text of the clicked multi-level label.
        /// </summary>
        /// <value>
        /// A string representing the text of the clicked multi-level label.
        /// </value>
        /// <remarks>
        /// This property retrieves the label text of a multi-level label that the user has interacted with.
        /// </remarks>
        public string Text { get; private set; }
    }

    /// <summary>
    /// Specifies the event arguments available for the editing events in the chart component.
    /// </summary>
    public class DataEditingEventArgs : BaseEventArgs
    {
        internal DataEditingEventArgs(string name, double newValue, double oldValue, Point point, double pointIndex, ChartSeries series, double seriesIndex)
        {
            Name = name;
            NewValue = newValue;
            OldValue = oldValue;
            Point = point;
            PointIndex = pointIndex;
            Series = series;
            SeriesIndex = seriesIndex;
        }

        /// <summary>
        /// Defines the current point new value.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> representing the new value.
        /// </value>
        /// <remarks>
        /// This is a read-only property that provides the updated value of the edited point.
        /// </remarks>
        public double NewValue { get; private set; }

        /// <summary>
        /// Defines the current point old value.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> representing the old value.
        /// </value>
        /// <remarks>
        /// This is a read-only property that keeps the original value before the editing occurred.
        /// </remarks>
        public double OldValue { get; private set; }

        /// <summary>
        /// Defines the current point.
        /// </summary>
        /// <value>
        /// A <see cref="Point"/> object representing the edited point.
        /// </value>
        /// <remarks>
        /// This is a read-only property providing information about the specific data point being edited.
        /// </remarks>
        public Point Point { get; private set; }

        /// <summary>
        /// Defines the current point index.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> representing the index of the point.
        /// </value>
        /// <remarks>
        /// This is a read-only property specifying the position of the point in its series.
        /// </remarks>
        public double PointIndex { get; private set; }

        /// <summary>
        /// Defines the current chart series.
        /// </summary>
        /// <value>
        /// A <see cref="ChartSeries"/> object that represents the current chart series.
        /// </value>
        /// <remarks>
        /// This is a read-only property indicating the series from which the edited point belongs.
        /// </remarks>
        public ChartSeries Series { get; private set; }

        /// <summary>
        /// Defines the current series index.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> representing the series index.
        /// </value>
        /// <remarks>
        /// This is a read-only property indicating the index of the series associated with the editing event.
        /// </remarks>
        public double SeriesIndex { get; private set; }
    }

    /// <summary>
    /// Specifies the event arguments available for on axis label click events in the chart component.
    /// </summary>
    public class AxisLabelClickEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AxisLabelClickEventArgs"/> class.
        /// </summary>
        /// <param name="name">The name of the event.</param>
        /// <param name="chart">The chart instance.</param>
        /// <param name="axis">The axis containing the clicked label.</param>
        /// <param name="text">The label text.</param>
        /// <param name="labelID">The HTML element ID of the label.</param>
        /// <param name="index">The index of the label.</param>
        /// <param name="location">The location of the associated annotation.</param>
        /// <param name="value">The numeric value represented by the label.</param>
        internal AxisLabelClickEventArgs(string name, SfChart chart, ChartAxis axis, string text, string labelID, int index, ChartEventLocation location, double value)
        {
            Name = name;
            Chart = chart;
            Axis = axis;
            Text = text;
            LabelID = labelID;
            Index = index;
            Location = location;
            Value = value;
        }

        /// <summary>
        /// Defines the chart instance when labelClick.
        /// </summary>
        /// <value>
        /// A <see cref="SfChart"/> object that represents the chart instance.
        /// </value>
        /// <remarks>
        /// This is a read-only property giving access to the chart related to the label click event.
        /// </remarks>
        public SfChart Chart { get; private set; }

        /// <summary>
        /// Defines the current axis.
        /// </summary>
        /// <value>
        /// A <see cref="ChartAxis"/> object that represents the current axis.
        /// </value>
        /// <remarks>
        /// This is a read-only property providing the context of the axis for the label click.
        /// </remarks>
        public ChartAxis Axis { get; private set; }

        /// <summary>
        /// Defines axis current label text.
        /// </summary>
        /// <value>
        /// Accepts the string value.
        /// </value>
        /// <remarks>
        /// This property retrieves the text of the axis label that was clicked.
        /// </remarks>
        public string Text { get; private set; }

        /// <summary>
        /// Defines axis current label element id.
        /// </summary>
        /// <value>
        /// Accepts the string value.
        /// </value>
        /// <remarks>
        /// This property provides the ID of the axis label element that was clicked.
        /// </remarks>
        public string LabelID { get; private set; }

        /// <summary>
        /// Defines axis current label index.
        /// </summary>
        /// <value>
        /// Accepts an integer value.
        /// </value>
        /// <remarks>
        /// This property provides the index of the label within the axis labels collection.
        /// </remarks>
        public int Index { get; private set; }

        /// <summary>
        /// Defines the current annotation location.
        /// </summary>
        /// <value>
        /// A <see cref="ChartEventLocation"/> object that defines the current annotation location.
        /// </value>
        /// <remarks>
        /// This property provides the location details where the annotation associated with the label click is placed.
        /// </remarks> 
        public ChartEventLocation Location { get; private set; }

        /// <summary>
        /// Defines axis current label value.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> value representing the axis's label value.
        /// </value>
        /// <remarks>
        /// This is a read-only property that gives the value depicted by the label on the clicked axis.
        /// </remarks>
        public double Value { get; private set; }
    }

    /// <summary>
    /// Defines the crosshair axis information.
    /// </summary>
    public class CrosshairAxisInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CrosshairAxisInfo"/> class.
        /// </summary>
        /// <param name="axisName">The name of the axis.</param>
        /// <param name="axisLabel">The label text of the axis at the intersection.</param>
        /// <param name="value">The value from JSON that may be a number, date, or string.</param>
        public CrosshairAxisInfo(string axisName, string axisLabel, object value)
        {
            AxisName = axisName;
            AxisLabel = axisLabel;

            if (value is not null)
            {
                JsonElement jsonElement = (JsonElement)value;

                if (jsonElement.ValueKind == JsonValueKind.Number)
                {
                    Value = jsonElement.GetDouble();
                }
                else if (jsonElement.ValueKind == JsonValueKind.String &&
                         DateTime.TryParse(jsonElement.GetString(), out DateTime dateTime))
                {
                    Value = dateTime;
                }
                else
                {
                    Value = jsonElement.GetString() ?? string.Empty;
                }
            }
        }

        /// <summary>
        /// Define the specific axis name.
        /// </summary>
        /// <value>
        /// A string representing the name of the axis.
        /// </value>
        /// <remarks>
        /// This is a read-only property providing the name of the axis crossed by the crosshair.
        /// </remarks>
        public string AxisName { get; private set; }

        /// <summary>
        /// Define the specific axis label.
        /// </summary>
        /// <value>
        /// A string representing the label of the axis.
        /// </value>
        /// <remarks>
        /// This is a read-only property providing the label information of the axis crossed by the crosshair.
        /// </remarks>
        public string AxisLabel { get; private set; }

        /// <summary>
        /// Gets the value of the axis crossed by the crosshair lines.
        /// </summary>
        /// <value>
        /// An object representing the value of the axis. The default value is <b>null</b>.
        /// </value>
        /// <remarks>
        /// It is a read-only value providing the exact point value on the axis where the crosshair lines intersect.
        /// </remarks>
        public object Value { get; internal set; } = null!;
    }

    /// <summary>
    /// Specifies the event arguments available for on loaded events in the chart component.
    /// </summary>
    public class LoadedEventArgs : BaseEventArgs
    {
    }

    /// <summary>
    /// Defines the axis label template point information.
    /// </summary>
    public class ChartAxisLabelInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChartAxisLabelInfo"/> class.
        /// </summary>
        /// <remarks>
        /// The parameterless constructor is used for JSON deserialization and default initialization.
        /// </remarks>
        public ChartAxisLabelInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartAxisLabelInfo"/> class with text and date.
        /// </summary>
        /// <param name="text">The label text.</param>
        /// <param name="date">The DateTime value for the label.</param>
        public ChartAxisLabelInfo(string text, DateTime date)
        {
            Text = text;
            DateTimeLabel = date;
        }

        /// <summary> 
        /// Gets the text for each label in the axis, which can be used to display in the axis label template. 
        /// </summary> 
        /// <value> 
        /// A <c>string</c> value representing text to be displayed in the axis labels. 
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
        /// This text can be used to display in the axis label templates for axis types such as category, numeric, and logarithmic. 
        /// </remarks> 
        public string Text { get; set; } = string.Empty;

        /// <summary> 
        /// Gets the <c>DateTime</c> object for the current axis label in the DateTime axis, which can be used for display in the axis label template. 
        /// </summary> 
        /// <value> 
        /// A <c>DateTime</c> value representing the text to be displayed in the DateTime axis labels. 
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
        ///             <div>@data.DateTimeValue.Month.ToString() </div> 
        ///         </LabelTemplate> 
        ///     </ChartPrimaryXAxis> 
        /// </SfChart> 
        /// ]]> 
        /// </code> 
        /// </example> 
        /// <remarks> 
        /// This property can be used to display date and time details in axis label templates for DateTime axes. 
        /// </remarks>  
        public DateTime DateTimeLabel { get; set; }
    }
}
