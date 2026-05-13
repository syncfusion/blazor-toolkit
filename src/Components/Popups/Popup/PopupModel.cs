using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Toolkit.Popups
{

    /// <summary>
    /// Represents the position data model for defining popup element positioning coordinates and alignment.
    /// </summary>
    /// <remarks>
    /// The <see cref="PositionDataModel"/> class provides string-based position values that can specify both
    /// alignment keywords (such as "left", "center", "right" for X-axis and "top", "center", "bottom" for Y-axis)
    /// and numeric offset values. This flexible approach allows for both relative positioning and precise pixel-based positioning.
    /// </remarks>
    /// <example>
    /// Configuring position data with alignment keywords.
    /// <code><![CDATA[
    /// var positionData = new PositionDataModel
    /// {
    ///     X = "center",
    ///     Y = "top"
    /// };
    /// 
    /// // Or with numeric values
    /// var positionDataNumeric = new PositionDataModel
    /// {
    ///     X = "100",
    ///     Y = "50"
    /// };
    /// ]]></code>
    /// </example>
    public class PositionDataModel
    {
        /// <summary>
        /// Gets or sets the horizontal position value for the popup element.
        /// </summary>
        /// <value>
        /// A <c>string</c> value that specifies the horizontal positioning. This can be alignment keywords like "left", "center", "right" or numeric pixel values. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property defines the horizontal alignment or offset for the popup element.
        /// Supported alignment values include "left", "center", and "right".
        /// Numeric string values are treated as pixel offsets from the reference point.
        /// If <c>null</c>, the default horizontal positioning behavior will be applied.
        /// </remarks>
        [JsonPropertyName("X")]
        public string X { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the vertical position value for the popup element.
        /// </summary>
        /// <value>
        /// A <c>string</c> value that specifies the vertical positioning. This can be alignment keywords like "top", "center", "bottom" or numeric pixel values. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property defines the vertical alignment or offset for the popup element.
        /// Supported alignment values include "top", "center", and "bottom".
        /// Numeric string values are treated as pixel offsets from the reference point.
        /// If <c>null</c>, the default vertical positioning behavior will be applied.
        /// </remarks>
        [JsonPropertyName("Y")]
        public string Y { get; set; } = string.Empty;
    }
}