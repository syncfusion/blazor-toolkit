using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Toolkit
{
    /// <exclude />
    /// <summary>
    /// The container and item sizes that the browser measurement module reports to .NET for one overflow calculation.
    /// </summary>
    internal sealed class ToolbarMeasurement
    {
        /// <exclude />
        /// <summary>
        /// Gets or sets the room available along the main axis, in CSS pixels, with the container padding removed.
        /// </summary>
        [JsonPropertyName("available")]
        public double Available { get; set; }

        /// <exclude />
        /// <summary>
        /// Gets or sets the gap between items, in CSS pixels.
        /// </summary>
        [JsonPropertyName("gap")]
        public double Gap { get; set; }

        /// <exclude />
        /// <summary>
        /// Gets or sets the size of the overflow button, or 0 when it is not rendered.
        /// </summary>
        [JsonPropertyName("buttonSize")]
        public double ButtonSize { get; set; }

        /// <exclude />
        /// <summary>
        /// Gets or sets the measured items, in document order.
        /// </summary>
        [JsonPropertyName("items")]
        public List<ToolbarMeasuredItem> Items { get; set; } = [];
    }

    /// <exclude />
    /// <summary>
    /// One measured item.
    /// </summary>
    internal sealed class ToolbarMeasuredItem
    {
        /// <exclude />
        /// <summary>
        /// Gets or sets the identifier of the item.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        /// <exclude />
        /// <summary>
        /// Gets or sets the size of the item along the main axis, in CSS pixels.
        /// </summary>
        [JsonPropertyName("size")]
        public double Size { get; set; }
    }

    /// <exclude />
    /// <summary>
    /// Viewport coordinates for the overflow menu, measured against the overflow button.
    /// </summary>
    /// <remarks>
    /// The menu is <c>position: fixed</c> (ADR-006). These values are CSS pixels from the viewport
    /// edges, not offsets inside the toolbar. JavaScript measures and applies them through CSSOM.
    /// </remarks>
    internal sealed class ToolbarPopupPlacement
    {
        /// <exclude />
        /// <summary>
        /// Gets or sets the side of the anchor the popup opens on: <c>bottom</c> or <c>top</c>.
        /// </summary>
        [JsonPropertyName("placement")]
        public string Placement { get; set; } = "bottom";

        /// <exclude />
        /// <summary>
        /// Gets or sets whether the popup opens below the anchor.
        /// </summary>
        [JsonPropertyName("below")]
        public bool Below { get; set; } = true;

        /// <exclude />
        /// <summary>
        /// Gets or sets the room the chosen side offers, in CSS pixels. The popup is capped to it when
        /// no side fits the whole popup, so the entries stay reachable by scrolling inside the popup.
        /// </summary>
        [JsonPropertyName("available")]
        public double Available { get; set; }

        /// <exclude />
        /// <summary>
        /// Gets or sets whether the anchor had a real box. An unpositioned result must not be written,
        /// or the menu flashes at the viewport origin before the first layout.
        /// </summary>
        [JsonPropertyName("positioned")]
        public bool Positioned { get; set; }

        /// <exclude />
        /// <summary>Gets or sets the distance from the viewport top, in CSS pixels, when the menu opens below.</summary>
        [JsonPropertyName("top")]
        public double Top { get; set; }

        /// <exclude />
        /// <summary>Gets or sets the distance from the viewport bottom, in CSS pixels, when the menu opens above.</summary>
        [JsonPropertyName("bottom")]
        public double Bottom { get; set; }

        /// <exclude />
        /// <summary>Gets or sets the distance from the viewport left, in CSS pixels, in RTL.</summary>
        [JsonPropertyName("left")]
        public double Left { get; set; }

        /// <exclude />
        /// <summary>Gets or sets the distance from the viewport right, in CSS pixels, in LTR.</summary>
        [JsonPropertyName("right")]
        public double Right { get; set; }

        /// <exclude />
        /// <summary>Gets or sets the block-size cap, in CSS pixels. Zero means the stylesheet cap applies.</summary>
        [JsonPropertyName("maxBlockSize")]
        public double MaxBlockSize { get; set; }

        /// <exclude />
        /// <summary>Gets or sets whether the anchor is in a right-to-left toolbar.</summary>
        [JsonPropertyName("rtl")]
        public bool Rtl { get; set; }
    }

    /// <exclude />
    /// <summary>
    /// The scroll position of a scrollable toolbar, used to drive the navigation chevrons.
    /// </summary>
    internal sealed class ToolbarScrollState
    {
        /// <exclude />
        /// <summary>
        /// Gets or sets whether the content is larger than the visible area, so the toolbar can scroll.
        /// </summary>
        [JsonPropertyName("canScroll")]
        public bool CanScroll { get; set; }

        /// <exclude />
        /// <summary>
        /// Gets or sets whether the toolbar is at the start of the scroll range.
        /// </summary>
        [JsonPropertyName("atStart")]
        public bool AtStart { get; set; } = true;

        /// <exclude />
        /// <summary>
        /// Gets or sets whether the toolbar is at the end of the scroll range.
        /// </summary>
        [JsonPropertyName("atEnd")]
        public bool AtEnd { get; set; } = true;

        /// <exclude />
        /// <summary>
        /// Gets or sets the size of the scrollable content.
        /// </summary>
        [JsonPropertyName("scrollSize")]
        public double ScrollSize { get; set; }

        /// <exclude />
        /// <summary>
        /// Gets or sets the size of the visible area.
        /// </summary>
        [JsonPropertyName("clientSize")]
        public double ClientSize { get; set; }
    }
}
