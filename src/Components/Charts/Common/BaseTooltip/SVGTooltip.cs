using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Represents font and text related styling used by tooltip rendering.
    /// </summary>
    public class TextStyleModel
    {
        /// <summary>
        /// Gets or sets the font style (e.g., "Normal", "Italic").
        /// </summary>
        /// <value>Font style string. Default: empty string.</value>
        [JsonPropertyName("fontStyle")]
        public string FontStyle { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the opacity value for the text.
        /// </summary>
        /// <value>Opacity as a double between 0 and 1.</value>
        [JsonPropertyName("opacity")]
        public double Opacity { get; set; }

        /// <summary>
        /// Gets or sets the color used to render the text.
        /// </summary>
        /// <value>CSS color string. Default: empty string.</value>
        [JsonPropertyName("color")]
        public string Color { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the font size (e.g., "12px", "0.9rem").
        /// </summary>
        /// <value>Font size string. Default: empty string.</value>
        [JsonPropertyName("size")]
        public string Size { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the font family used for the text.
        /// </summary>
        /// <value>Font family string. Default: empty string.</value>
        [JsonPropertyName("fontFamily")]
        public string FontFamily { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the font weight (e.g., "Normal", "Bold", "400").
        /// </summary>
        /// <value>Font weight string. Default: empty string.</value>
        [JsonPropertyName("fontWeight")]
        public string FontWeight { get; set; } = string.Empty;
    }

    /// <summary>
    /// Describes border characteristics for tooltip shapes.
    /// </summary>
    public class TooltipBorderModel
    {
        /// <summary>
        /// Gets or sets the border color.
        /// </summary>
        /// <value>CSS color string. Default: empty string.</value>
        [JsonPropertyName("color")]
        public string Color { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the border width in pixels.
        /// </summary>
        /// <value>Border width as double.</value>
        [JsonPropertyName("width")]
        public double Width { get; set; }
    }

    /// <summary>
    /// Represents a 2D point location used for tooltip placement calculations.
    /// </summary>
    public class ToolLocationModel
    {
        /// <summary>
        /// Gets or sets the X coordinate.
        /// </summary>
        /// <value>X coordinate as double.</value>
        [JsonPropertyName("x")]
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate.
        /// </summary>
        /// <value>Y coordinate as double.</value>
        [JsonPropertyName("y")]
        public double Y { get; set; }
    }

    /// <summary>
    /// Represents rectangular bounds (location + size) used for area calculations.
    /// </summary>
    public class AreaBoundsModel : ToolLocationModel
    {
        /// <summary>
        /// Gets or sets the width of the area.
        /// </summary>
        /// <value>Width as double.</value>
        [JsonPropertyName("width")]
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the area.
        /// </summary>
        /// <value>Height as double.</value>
        [JsonPropertyName("height")]
        public double Height { get; set; }
    }

    /// <summary>
    /// Serializable model that contains all properties required to render SVG tooltips.
    /// </summary>
    public class SVGTooltip
    {
        /// <summary>
        /// Gets or sets a value indicating whether the tooltip is shared across series.
        /// </summary>
        /// <value><see langword="true"/> if shared; otherwise <see langword="false"/>.</value>
        [JsonPropertyName("shared")]
        public bool Shared { get; set; }

        /// <summary>
        /// Gets or sets whether crosshair is enabled.
        /// </summary>
        /// <value><see langword="true"/> to enable crosshair; otherwise <see langword="false"/>.</value>
        [JsonPropertyName("crosshair")]
        public bool CrosshairEnabled { get; set; }

        /// <summary>
        /// Gets or sets the fill color of the tooltip.
        /// </summary>
        /// <value>CSS color string. Default: empty string.</value>
        [JsonPropertyName("fill")]
        public string Fill { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the header text of the tooltip.
        /// </summary>
        /// <value>Header text string. Default: empty string.</value>
        [JsonPropertyName("header")]
        public string Header { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the tooltip opacity.
        /// </summary>
        /// <value>Opacity as double between 0 and 1.</value>
        [JsonPropertyName("opacity")]
        public double Opacity { get; set; }

        /// <summary>
        /// Gets or sets the text style details used inside tooltip.
        /// </summary>
        /// <value>Instance of <see cref="TextStyleModel"/>.</value>
        [JsonPropertyName("textStyle")]
        public TextStyleModel TextStyle { get; set; } = null!;

        /// <summary>
        /// Gets or sets the template string (html/svg fragment) for the tooltip.
        /// </summary>
        /// <value>Template string. Default: empty string.</value>
        [JsonPropertyName("template")]
        public string Template { get; set; } = null!;

        /// <summary>
        /// Gets or sets whether animation is enabled for tooltip show/hide.
        /// </summary>
        /// <value><see langword="true"/> to enable animation; otherwise <see langword="false"/>.</value>
        [JsonPropertyName("enableAnimation")]
        public bool EnableAnimation { get; set; }

        /// <summary>
        /// Gets or sets the animation duration in milliseconds.
        /// </summary>
        /// <value>Duration as double.</value>
        [JsonPropertyName("duration")]
        public double Duration { get; set; }

        /// <summary>
        /// Gets or sets whether the tooltip is rendered in inverted mode.
        /// </summary>
        /// <value><see langword="true"/> if inverted; otherwise <see langword="false"/>.</value>
        [JsonPropertyName("inverted")]
        public bool Inverted { get; set; }

        /// <summary>
        /// Gets or sets whether the values are negative (affects styling).
        /// </summary>
        /// <value><see langword="true"/> if negative; otherwise <see langword="false"/>.</value>
        [JsonPropertyName("isNegative")]
        public bool IsNegative { get; set; }

        /// <summary>
        /// Gets or sets the tooltip border configuration.
        /// </summary>
        /// <value>Instance of <see cref="TooltipBorderModel"/>.</value>
        [JsonPropertyName("border")]
        public TooltipBorderModel Border { get; set; } = null!;

        /// <summary>
        /// Gets or sets the textual content lines for the tooltip.
        /// </summary>
        /// <value>Array of content strings.</value>
        [JsonPropertyName("content")]
        public string[] Content { get; set; } = null!;

        /// <summary>
        /// Gets or sets the clip bounds location used when rendering within constrained area.
        /// </summary>
        /// <value>Instance of <see cref="ToolLocationModel"/>.</value>
        [JsonPropertyName("clipBounds")]
        public ToolLocationModel ClipBounds { get; set; } = null!;

        /// <summary>
        /// Gets or sets the palette of colors available to the tooltip.
        /// </summary>
        /// <value>Array of CSS color strings.</value>
        [JsonPropertyName("palette")]
        public string[] Palette { get; set; } = null!;

        /// <summary>
        /// Gets or sets the shapes rendered in the tooltip (legend markers).
        /// </summary>
        /// <value>Array of <see cref="TooltipShape"/> values.</value>
        [JsonPropertyName("shapes")]
        public TooltipShape[] Shapes { get; set; } = null!;

        /// <summary>
        /// Gets or sets the tooltip location.
        /// </summary>
        /// <value>Instance of <see cref="ToolLocationModel"/>.</value>
        [JsonPropertyName("location")]
        public ToolLocationModel Location { get; set; } = null!;

        /// <summary>
        /// Gets or sets the offset (pixels) used to position the tooltip relative to the marker.
        /// </summary>
        /// <value>Offset as double.</value>
        [JsonPropertyName("offset")]
        public double Offset { get; set; }

        /// <summary>
        /// Gets or sets the horizontal corner radius (rx) for tooltip background.
        /// </summary>
        /// <value>RX radius as double.</value>
        [JsonPropertyName("rx")]
        public double RX { get; set; }

        /// <summary>
        /// Gets or sets the vertical corner radius (ry) for tooltip background.
        /// </summary>
        /// <value>RY radius as double.</value>
        [JsonPropertyName("ry")]
        public double RY { get; set; }

        /// <summary>
        /// Gets or sets the inner padding used for tooltip arrow placement.
        /// </summary>
        /// <value>Arrow padding as double.</value>
        [JsonPropertyName("arrowPadding")]
        public double ArrowPadding { get; set; }

        /// <summary>
        /// Gets or sets additional template-specific data.
        /// </summary>
        /// <value>Template data model instance.</value>
        [JsonPropertyName("data")]
        public TemplateData Data { get; set; } = null!;

        /// <summary>
        /// Gets or sets the theme identifier for tooltip styling.
        /// </summary>
        /// <value>Theme name string.</value>
        [JsonPropertyName("theme")]
        public string Theme { get; set; } = null!;

        /// <summary>
        /// Gets or sets the area bounds used for available drawing region.
        /// </summary>
        /// <value>Instance of <see cref="AreaBoundsModel"/>.</value>
        [JsonPropertyName("areaBounds")]
        public AreaBoundsModel AreaBounds { get; set; } = null!;

        /// <summary>
        /// Gets or sets the available size for rendering.
        /// </summary>
        /// <value>Instance of <see cref="Size"/> representing width/height.</value>
        [JsonPropertyName("availableSize")]
        public Size AvailableSize { get; set; } = null!;

        /// <summary>
        /// Gets or sets whether rendering target is canvas.
        /// </summary>
        /// <value><see langword="true"/> if canvas; otherwise <see langword="false"/>.</value>
        [JsonPropertyName("isCanvas")]
        public bool IsCanvas { get; set; }

        /// <summary>
        /// Gets or sets the control name associated with this tooltip model.
        /// </summary>
        /// <value>Control name string.</value>
        [JsonPropertyName("controlName")]
        public string ControlName { get; set; } = null!;

        /// <summary>
        /// Gets or sets whether text wrapping is enabled for tooltip content.
        /// </summary>
        /// <value><see langword="true"/> to enable text wrap; otherwise <see langword="false"/>.</value>
        [JsonPropertyName("isTextWrap")]
        public bool IsTextWrap { get; set; }

        /// <summary>
        /// Gets or sets whether right-to-left layout is enabled.
        /// </summary>
        /// <value><see langword="true"/> for RTL; otherwise <see langword="false"/>.</value>
        [JsonPropertyName("enableRTL")]
        public bool EnableRtl { get; set; }
    }
}