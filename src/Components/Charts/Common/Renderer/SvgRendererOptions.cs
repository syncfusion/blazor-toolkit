using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    public class FontOptions
    {
        /// <summary>
        /// Gets or sets the font color.
        /// </summary>
        public string Color { get; set; } = null!;

        /// <summary>
        /// Gets or sets the font size.
        /// </summary>
        public string Size { get; set; } = null!;

        /// <summary>
        /// Gets or sets the font family.
        /// </summary>
        public string FontFamily { get; set; } = null!;

        /// <summary>
        /// Gets or sets the font weight.
        /// </summary>
        public string FontWeight { get; set; } = null!;

        /// <summary>
        /// Gets or sets the font style.
        /// </summary>
        public string FontStyle { get; set; } = null!;
    }

    /// <summary>
    /// Specifies SVG pattern fill options.
    /// </summary>
    public class PatternOptions
    {
        /// <summary>
        /// Gets or sets the pattern width.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets the pattern height.
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the pattern.
        /// </summary>
        public string Id { get; set; } = null!;

        /// <summary>
        /// Gets or sets the pattern units coordinate system.
        /// </summary>
        public string PatternUnits { get; set; } = null!;

        /// <summary>
        /// Gets or sets the list of shape options within the pattern.
        /// </summary>
        public List<object> ShapeOptions { get; set; } = null!;
    }

    /// <summary>
    /// Specifies SVG text element rendering options.
    /// </summary>
    public class TextOptions
    {
        #region Properties

        /// <summary>
        /// Gets or sets the collection of text content strings.
        /// </summary>
        public List<string> TextCollection { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the collection of text locations for multi-line text.
        /// </summary>
        public List<TextLocation> TextLocationCollection { get; set; } = new List<TextLocation>();

        /// <summary>
        /// Gets or sets the X coordinate.
        /// </summary>
        public string X { get; set; } = null!;

        /// <summary>
        /// Gets or sets the Y coordinate.
        /// </summary>
        public string Y { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for the text element.
        /// </summary>
        public string Id { get; set; } = null!;

        /// <summary>
        /// Gets or sets the fill color.
        /// </summary>
        public string Fill { get; set; } = null!;

        /// <summary>
        /// Gets or sets the font size.
        /// </summary>
        public string FontSize { get; set; } = null!;

        /// <summary>
        /// Gets or sets the font style.
        /// </summary>
        public string FontStyle { get; set; } = null!;

        /// <summary>
        /// Gets or sets the font family.
        /// </summary>
        public string FontFamily { get; set; } = null!;

        /// <summary>
        /// Gets or sets the font weight.
        /// </summary>
        public string FontWeight { get; set; } = null!;

        /// <summary>
        /// Gets or sets the text anchor alignment.
        /// </summary>
        public string TextAnchor { get; set; } = null!;

        /// <summary>
        /// Gets or sets the font options object.
        /// </summary>
        public object Font { get; set; } = null!;

        /// <summary>
        /// Gets or sets the text content.
        /// </summary>
        public string Text { get; set; } = null!;

        /// <summary>
        /// Gets or sets the SVG transform attribute.
        /// </summary>
        public string Transform { get; set; } = null!;

        /// <summary>
        /// Gets or sets the label rotation angle.
        /// </summary>
        public string LabelRotation { get; set; } = null!;

        /// <summary>
        /// Gets or sets the dominant baseline for vertical text alignment.
        /// </summary>
        public string DominantBaseline { get; set; } = null!;

        /// <summary>
        /// Gets or sets the Razor child content.
        /// </summary>
        public RenderFragment ChildContent { get; set; } = null!;

        /// <summary>
        /// Gets or sets the accessibility text for screen readers.
        /// </summary>
        public string AccessibilityText { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the WAI-ARIA role.
        /// </summary>
        public string Role { get; set; } = "text";

        /// <summary>
        /// Gets or sets the tab index for keyboard navigation.
        /// </summary>
        public string TabIndex { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets inline CSS styles.
        /// </summary>
        public string Style { get; set; } = null!;

        /// <summary>
        /// Indicates whether the text represents a negative value (internal use).
        /// </summary>
        internal bool IsMinus { get; set; }

        /// <summary>
        /// Indicates whether a rotated label intersects with other elements (internal use).
        /// </summary>
        internal bool IsRotatedLabelIntersect { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TextOptions"/> class with specified parameters.
        /// </summary>
        /// <param name="x">The X coordinate position.</param>
        /// <param name="y">The Y coordinate position.</param>
        /// <param name="fill">The fill color; if empty, uses font color.</param>
        /// <param name="font">Font styling options.</param>
        /// <param name="text">The text content.</param>
        /// <param name="anchor">The text-anchor alignment value.</param>
        /// <param name="id">The unique element identifier.</param>
        /// <param name="transform">Optional SVG transform attribute. Default: <c>""</c>.</param>
        /// <param name="labelRotation">Optional rotation angle in degrees. Default: <c>"0"</c>.</param>
        /// <param name="dominantBaseline">Optional dominant-baseline value. Default: <c>"undefined"</c>.</param>
        /// <param name="accessibilityText">Optional accessibility description. Default: <c>""</c>.</param>
        /// <param name="role">Optional ARIA role. Default: <c>"text"</c>.</param>
        /// <param name="tabIndex">Optional tab index value. Default: <c>""</c>.</param>
        /// <param name="style">Optional inline CSS styles. Default: <c>""</c>.</param>
        public TextOptions(string x, string y, string fill, FontOptions font, string text, string anchor, string id, string transform = "", string labelRotation = "0", string dominantBaseline = "undefined", string accessibilityText = "", string role = "text", string tabIndex = "", string style = "")
        {
            X = x;
            Y = y;
            Fill = !string.IsNullOrEmpty(fill) ? fill : font?.Color ?? string.Empty;
            FontSize = font?.Size ?? string.Empty;
            FontFamily = font?.FontFamily ?? string.Empty;
            FontWeight = font?.FontWeight ?? string.Empty;
            FontStyle = font?.FontStyle ?? string.Empty;
            TextAnchor = anchor;
            Text = text;
            Id = id;
            Transform = transform;
            LabelRotation = labelRotation;
            DominantBaseline = dominantBaseline;
            AccessibilityText = accessibilityText;
            Role = role;
            TabIndex = tabIndex;
            Style = style;
            Font = font ?? null!;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextOptions"/> class with default values.
        /// </summary>
        public TextOptions()
        {
        }
        #endregion
    }

    /// <summary>
    /// Specifies SVG rectangle element rendering options.
    /// </summary>
    public class RectOptions
    {
        #region Properties

        /// <summary>
        /// Gets or sets the X coordinate of the rectangle.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate of the rectangle.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Gets or sets the width of the rectangle.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the rectangle.
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Gets or sets the stroke width.
        /// </summary>
        public double StrokeWidth { get; set; }

        /// <summary>
        /// Gets or sets the fill color.
        /// </summary>
        public string Fill { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for the rectangle element.
        /// </summary>
        public string Id { get; set; } = null!;

        /// <summary>
        /// Gets or sets the stroke dash array pattern.
        /// </summary>
        public string DashArray { get; set; } = null!;

        /// <summary>
        /// Gets or sets the SVG transform attribute.
        /// </summary>
        public string Transform { get; set; } = null!;

        /// <summary>
        /// Gets or sets the stroke color.
        /// </summary>
        public string Stroke { get; set; } = null!;

        /// <summary>
        /// Gets or sets the X-axis corner radius.
        /// </summary>
        public double Rx { get; set; }

        /// <summary>
        /// Gets or sets the Y-axis corner radius.
        /// </summary>
        public double Ry { get; set; }

        /// <summary>
        /// Gets or sets the opacity level.
        /// </summary>
        public double Opacity { get; set; }

        /// <summary>
        /// Gets or sets inline CSS styles.
        /// </summary>
        public string Style { get; set; } = null!;

        /// <summary>
        /// Gets or sets the SVG filter reference.
        /// </summary>
        public string Filter { get; set; } = null!;

        /// <summary>
        /// Gets or sets the visibility state.
        /// </summary>
        public string Visibility { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the tab index for keyboard navigation.
        /// </summary>
        public string TabIndex { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the WAI-ARIA hidden state.
        /// </summary>
        public string AriaHidden { get; set; } = "true";

        /// <summary>
        /// Gets or sets the tooltip title text.
        /// </summary>
        public string Title { get; set; } = "Rect Element";

        /// <summary>
        /// Gets or sets the accessibility text for screen readers.
        /// </summary>
        public string AccessibilityText { get; set; } = string.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RectOptions"/> class with specified parameters.
        /// </summary>
        /// <param name="id">The unique element identifier.</param>
        /// <param name="x">The X coordinate position.</param>
        /// <param name="y">The Y coordinate position.</param>
        /// <param name="width">The rectangle width.</param>
        /// <param name="height">The rectangle height.</param>
        /// <param name="strokeWidth">The stroke width.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="fill">The fill color.</param>
        /// <param name="rx">Optional X-axis corner radius. Default: <c>0</c>.</param>
        /// <param name="ry">Optional Y-axis corner radius. Default: <c>0</c>.</param>
        /// <param name="opacity">Optional opacity level. Default: <c>1</c>.</param>
        /// <param name="visibility">Optional visibility state. Default: <c>""</c>.</param>
        /// <param name="style">Optional inline CSS styles. Default: <c>""</c>.</param>
        /// <param name="filter">Optional filter reference. Default: <c>""</c>.</param>
        /// <param name="tabIndex">Optional tab index value. Default: <c>""</c>.</param>
        /// <param name="ariaHidden">Optional ARIA hidden state. Default: <c>"true"</c>.</param>
        /// <param name="title">Optional title text. Default: <c>"Rect Element"</c>.</param>
        public RectOptions(string id, double x, double y, double width, double height, double strokeWidth, string stroke, string fill, double rx = 0, double ry = 0, double opacity = 1, string visibility = "", string style = "", string filter = "", string tabIndex = "", string ariaHidden = "true", string title = "Rect Element")
        {
            Id = id;
            X = x;
            Y = y;
            Width = width;
            Height = height;
            StrokeWidth = strokeWidth;
            Stroke = stroke;
            Fill = fill;
            Rx = rx;
            Ry = ry;
            Opacity = opacity;
            Visibility = visibility;
            Style = style;
            Filter = filter;
            TabIndex = tabIndex;
            AriaHidden = ariaHidden;
            Title = title;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="RectOptions"/> class with default values.
        /// </summary>
        public RectOptions()
        {
        }
        #endregion
    }

    /// <summary>
    /// Specifies SVG path element rendering options.
    /// </summary>
    public class PathOptions
    {
        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the path element.
        /// </summary>
        public string Id { get; set; } = null!;

        /// <summary>
        /// Gets or sets the SVG path direction (d attribute).
        /// </summary>
        public string Direction { get; set; } = null!;

        /// <summary>
        /// Gets or sets the stroke dash array pattern.
        /// </summary>
        public string StrokeDashArray { get; set; } = null!;

        /// <summary>
        /// Gets or sets the stroke color.
        /// </summary>
        public string Stroke { get; set; } = null!;

        /// <summary>
        /// Gets or sets the stroke width.
        /// </summary>
        public double StrokeWidth { get; set; } = 1;

        /// <summary>
        /// Gets or sets the opacity level.
        /// </summary>
        public double Opacity { get; set; }

        /// <summary>
        /// Gets or sets the fill color.
        /// </summary>
        public string Fill { get; set; } = null!;

        /// <summary>
        /// Gets or sets the stroke miter limit.
        /// </summary>
        public string StrokeMiterLimit { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the clip path reference.
        /// </summary>
        public string ClipPath { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the accessibility text for screen readers.
        /// </summary>
        public string AccessibilityText { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the visibility state.
        /// </summary>
        public string Visibility { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets inline CSS styles.
        /// </summary>
        public string Style { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the associated data point identifier.
        /// </summary>
        public string DataPoint { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the tab index for keyboard navigation.
        /// </summary>
        public string TabIndex { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the tooltip title text.
        /// </summary>
        public string Title { get; set; } = "Path Element";

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PathOptions"/> class with specified parameters.
        /// </summary>
        /// <param name="id">The unique element identifier.</param>
        /// <param name="direction">The SVG path direction (d attribute).</param>
        /// <param name="strokeDasharray">The stroke dash array pattern.</param>
        /// <param name="strokeWidth">The stroke width.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="opacity">Optional opacity level. Default: <c>1</c>.</param>
        /// <param name="fill">Optional fill color. Default: <c>"none"</c>.</param>
        /// <param name="strokeMiterLimit">Optional stroke miter limit. Default: <c>""</c>.</param>
        /// <param name="clipPath">Optional clip path reference. Default: <c>""</c>.</param>
        /// <param name="accessText">Optional accessibility text. Default: <c>""</c>.</param>
        /// <param name="style">Optional inline CSS styles. Default: <c>""</c>.</param>
        /// <param name="tabIndex">Optional tab index value. Default: <c>""</c>.</param>
        /// <param name="datapoint">Optional data point identifier. Default: <c>""</c>.</param>
        /// <param name="title">Optional title text. Default: <c>"Path Element"</c>.</param>
        public PathOptions(string id, string direction, string strokeDasharray, double strokeWidth, string stroke, double opacity = 1, string fill = "none", string strokeMiterLimit = "", string clipPath = "", string accessText = "", string style = "", string tabIndex = "", string datapoint = "", string title = "Path Element")
        {
            Id = id;
            Direction = direction;
            StrokeDashArray = strokeDasharray;
            StrokeWidth = strokeWidth;
            Stroke = stroke;
            Opacity = opacity;
            Fill = fill;
            StrokeMiterLimit = strokeMiterLimit;
            ClipPath = clipPath;
            AccessibilityText = accessText;
            Style = style;
            TabIndex = tabIndex;
            DataPoint = datapoint;
            Title = title;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PathOptions"/> class with default values.
        /// </summary>
        public PathOptions()
        {
        }
        #endregion
    }

    /// <summary>
    /// Specifies SVG ellipse element rendering options.
    /// </summary>
    public class EllipseOptions
    {
        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the ellipse element.
        /// </summary>
        public string Id { get; set; } = null!;

        /// <summary>
        /// Gets or sets the X-axis radius.
        /// </summary>
        public string Rx { get; set; } = null!;

        /// <summary>
        /// Gets or sets the Y-axis radius.
        /// </summary>
        public string Ry { get; set; } = null!;

        /// <summary>
        /// Gets or sets the center X coordinate.
        /// </summary>
        public string Cx { get; set; } = null!;

        /// <summary>
        /// Gets or sets the center Y coordinate.
        /// </summary>
        public string Cy { get; set; } = null!;

        /// <summary>
        /// Gets or sets the stroke dash array pattern.
        /// </summary>
        public string StrokeDashArray { get; set; } = null!;

        /// <summary>
        /// Gets or sets the stroke color.
        /// </summary>
        public string Stroke { get; set; } = null!;

        /// <summary>
        /// Gets or sets the stroke width.
        /// </summary>
        public double StrokeWidth { get; set; } = 1;

        /// <summary>
        /// Gets or sets the opacity level.
        /// </summary>
        public double Opacity { get; set; }

        /// <summary>
        /// Gets or sets the fill color.
        /// </summary>
        public string Fill { get; set; } = null!;

        /// <summary>
        /// Gets or sets the visibility state.
        /// </summary>
        public string Visibility { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the associated data point identifier.
        /// </summary>
        public string DataPoint { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the accessibility text for screen readers.
        /// </summary>
        public string AccessibilityText { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the tooltip title text.
        /// </summary>
        public string Title { get; set; } = "Ellipse Element";

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EllipseOptions"/> class with specified parameters.
        /// </summary>
        /// <param name="id">The unique element identifier.</param>
        /// <param name="rx">The X-axis radius.</param>
        /// <param name="ry">The Y-axis radius.</param>
        /// <param name="cx">The center X coordinate.</param>
        /// <param name="cy">The center Y coordinate.</param>
        /// <param name="strokeDasharray">The stroke dash array pattern.</param>
        /// <param name="strokeWidth">The stroke width.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="opacity">Optional opacity level. Default: <c>1</c>.</param>
        /// <param name="fill">Optional fill color. Default: <c>"none"</c>.</param>
        /// <param name="datapoint">Optional data point identifier. Default: <c>""</c>.</param>
        /// <param name="accessText">Optional accessibility text. Default: <c>""</c>.</param>
        /// <param name="title">Optional title text. Default: <c>"Ellipse Element"</c>.</param>
        public EllipseOptions(string id, string rx, string ry, string cx, string cy, string strokeDasharray, double strokeWidth, string stroke, double opacity = 1, string fill = "none", string datapoint = "", string accessText = "", string title = "Ellipse Element")
        {
            Id = id;
            Rx = rx;
            Ry = ry;
            Cx = cx;
            Cy = cy;
            StrokeDashArray = strokeDasharray;
            StrokeWidth = strokeWidth;
            Stroke = stroke;
            Opacity = opacity;
            Fill = fill;
            DataPoint = datapoint;
            AccessibilityText = accessText;
            Title = title;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EllipseOptions"/> class with default values.
        /// </summary>
        public EllipseOptions()
        {
        }
        #endregion
    }

    /// <summary>
    /// Specifies SVG circle element rendering options.
    /// </summary>
    public class CircleOptions
    {
        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the circle element.
        /// </summary>
        public string Id { get; set; } = null!;

        /// <summary>
        /// Gets or sets the center X coordinate.
        /// </summary>
        public string Cx { get; set; } = null!;

        /// <summary>
        /// Gets or sets the center Y coordinate.
        /// </summary>
        public string Cy { get; set; } = null!;

        /// <summary>
        /// Gets or sets the circle radius.
        /// </summary>
        public string R { get; set; } = null!;

        /// <summary>
        /// Gets or sets the stroke dash array pattern.
        /// </summary>
        public string StrokeDashArray { get; set; } = null!;

        /// <summary>
        /// Gets or sets the stroke color.
        /// </summary>
        public string Stroke { get; set; } = null!;

        /// <summary>
        /// Gets or sets the stroke width.
        /// </summary>
        public double StrokeWidth { get; set; } = 1;

        /// <summary>
        /// Gets or sets the opacity level.
        /// </summary>
        public double Opacity { get; set; } = 1;

        /// <summary>
        /// Gets or sets the fill color.
        /// </summary>
        public string Fill { get; set; } = "none";

        /// <summary>
        /// Gets or sets the visibility state.
        /// </summary>
        public string Visibility { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the accessibility text for screen readers.
        /// </summary>
        public string AccessibilityText { get; set; } = string.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CircleOptions"/> class with specified parameters.
        /// </summary>
        /// <param name="id">The unique element identifier.</param>
        /// <param name="cx">The center X coordinate.</param>
        /// <param name="cy">The center Y coordinate.</param>
        /// <param name="r">The circle radius.</param>
        /// <param name="strokeDasharray">The stroke dash array pattern.</param>
        /// <param name="strokeWidth">The stroke width.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="opacity">Optional opacity level. Default: <c>1</c>.</param>
        /// <param name="fill">Optional fill color. Default: <c>"none"</c>.</param>
        /// <param name="visibility">Optional visibility state. Default: <c>""</c>.</param>
        /// <param name="accessText">Optional accessibility text. Default: <c>""</c>.</param>
        public CircleOptions(string id, string cx, string cy, string r, string strokeDasharray, double strokeWidth, string stroke, double opacity = 1, string fill = "none", string visibility = "", string accessText = "")
        {
            Id = id;
            Cx = cx;
            Cy = cy;
            R = r;
            StrokeDashArray = strokeDasharray;
            StrokeWidth = strokeWidth;
            Stroke = stroke;
            Opacity = opacity;
            Fill = fill;
            Visibility = visibility;
            AccessibilityText = accessText;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CircleOptions"/> class with default values.
        /// </summary>
        public CircleOptions()
        {
        }
        #endregion
    }

    /// <summary>
    /// Specifies SVG image element rendering options.
    /// </summary>
    public class ImageOptions
    {
        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the image element.
        /// </summary>
        public string Id { get; set; } = null!;

        /// <summary>
        /// Gets or sets the X coordinate of the image.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate of the image.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Gets or sets the width of the image.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the image.
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Gets or sets the image URL or data URI.
        /// </summary>
        public string Href { get; set; } = null!;

        /// <summary>
        /// Gets or sets the visibility state.
        /// </summary>
        public string Visibility { get; set; } = null!;

        /// <summary>
        /// Gets or sets the preserve aspect ratio mode.
        /// </summary>
        public string PreserveAspectRatio { get; set; } = "none";

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageOptions"/> class with specified parameters.
        /// </summary>
        /// <param name="id">The unique element identifier.</param>
        /// <param name="x">The X coordinate position.</param>
        /// <param name="y">The Y coordinate position.</param>
        /// <param name="width">The image width.</param>
        /// <param name="height">The image height.</param>
        /// <param name="url">The image source URL.</param>
        /// <param name="visibility">Optional visibility state. Default: <c>""</c>.</param>
        /// <param name="preserveAspectRatio">Optional preserve aspect ratio mode. Default: <c>"none"</c>.</param>
        public ImageOptions(string id, double x, double y, double width, double height, string url, string visibility = "", string preserveAspectRatio = "none")
        {
            Id = id;
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Href = url;
            Visibility = visibility;
            PreserveAspectRatio = preserveAspectRatio;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageOptions"/> class with default values.
        /// </summary>
        public ImageOptions()
        {
        }
        #endregion
    }

    /// <summary>
    /// Specifies combined symbol rendering options for different shape types.
    /// </summary>
    public class SymbolOptions
    {
        /// <summary>
        /// Gets or sets the path rendering options.
        /// </summary>
        public PathOptions PathOption { get; set; } = new PathOptions();

        /// <summary>
        /// Gets or sets the ellipse rendering options.
        /// </summary>
        public EllipseOptions EllipseOption { get; set; } = new EllipseOptions();

        /// <summary>
        /// Gets or sets the image rendering options.
        /// </summary>
        public ImageOptions ImageOption { get; set; } = new ImageOptions();

        /// <summary>
        /// Gets or sets the shape type to render.
        /// </summary>
        public ShapeName ShapeName { get; set; }
    }

    /// <summary>
    /// Represents the rectangular dimensions of a region in two-dimensional space.
    /// </summary>
    public class Rect 
    {
        #region Properties

        /// <summary>
        /// Gets or sets the X coordinate of the region.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate of the region.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Gets or sets the width of the region.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the region.
        /// </summary>
        public double Height { get; set; }

        #endregion

        #region  Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Rect"/> class with specified dimensions.
        /// </summary>
        /// <param name="x">The X coordinate of the region.</param>
        /// <param name="y">The Y coordinate of the region.</param>
        /// <param name="width">The width of the region.</param>
        /// <param name="height">The height of the region.</param>
        public Rect(double x, double y, double width, double height )
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rect"/> class with default values.
        /// </summary>
        public Rect()
        {
        }
        #endregion
    }

    /// <summary>
    /// Represents a single text location with Y-coordinate for multi-line text rendering.
    /// </summary>
    public class TextLocation
    {
        /// <summary>
        /// Gets or sets the text content at this location.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the Y-coordinate offset for this text line.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextLocation"/> class with specified text and Y coordinate.
        /// </summary>
        /// <param name="text">The text content.</param>
        /// <param name="y">The Y-coordinate position.</param>
        public TextLocation(string text, double y)
        {
            Text = text;
            Y = y;
        }
    }
}