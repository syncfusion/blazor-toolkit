using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Internal;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Tooltip helper component backing logic for trimmed tooltips used by chart components.
    /// </summary>
    /// <remarks>
    /// This partial contains the core logic for showing, updating and fading out a lightweight tooltip.
    /// Content is HTML-encoded to mitigate XSS risks when rendering untrusted text.
    /// </remarks>
    public partial class TrimTooltipBase
    {
        #region Fields
        CultureInfo _culture { get; set; } = CultureInfo.InvariantCulture;
        Dictionary<string, object> _trimTooltipAttribute = [];
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the tooltip element identifier.
        /// </summary>
        /// <value>The identifier associated with the active tooltip.</value>
        [Parameter]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the inline style for positioning and appearance of the tooltip.
        /// </summary>
        /// <value>A CSS style string suitable for the tooltip container.</value>
        [Parameter]
        public string Style { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the content text rendered inside the tooltip. Content is HTML-encoded.
        /// </summary>
        /// <value>The tooltip text; encoding is applied to avoid XSS.</value>
        [Parameter]
        public string Content { get; set; } = string.Empty;
        #endregion

        protected override async Task OnInitializedAsync()
        {
            updateStyleAttributes(Style);
        }

        #region Internal Methods

        /// <summary>
        /// Change or remove tooltip content based on id or explicit removal flag.
        /// </summary>
        /// <param name="id">Tooltip identifier to match. If empty, ignored.</param>
        /// <param name="remove">If <c>true</c>, forcibly clears tooltip.</param>
        internal void ChangeContent(string id = "", bool remove = false)
        {
            if (remove)
            {
                Id = Content = string.Empty;
                Style = null!;
                updateStyleAttributes(Style);
                InvokeAsync(StateHasChanged);
            }
            else if (id == Id)
            {
                Style = null!;
                Content = string.Empty;
                updateStyleAttributes(Style);
                InvokeAsync(StateHasChanged);
            }
        }

        /// <summary>
        /// Shows a tooltip at the requested location with safety encoding and bounds checking.
        /// </summary>
        /// <param name="text">Raw text to render inside the tooltip.</param>
        /// <param name="x">X coordinate (pixels) relative to the chart area.</param>
        /// <param name="y">Y coordinate (pixels) relative to the chart area.</param>
        /// <param name="areaWidth">Width of the chart area used for boundary checks.</param>
        /// <param name="areaHeight">Height of the chart area used for boundary checks.</param>
        /// <param name="id">Identifier for the tooltip instance.</param>
        internal void ShowTooltip(string text, double x, double y, double areaWidth, double areaHeight, string id)
        {
            Size textSize = ChartHelper.MeasureText(text, new ChartFontOptions()
            {
                FontFamily = "Segoe UI",
                Size = "12px",
                FontStyle = "Normal",
                FontWeight = "Regular"
            });
            
            double width = textSize.Width + 5;
            x = (x + width > areaWidth) ? x - (width + 15) : x + 15;
            y = (y + textSize.Height > areaHeight) ? y - (textSize.Height / 2) : y;
            
            Id = id;
            Content = text;
            x = x < 0 ? 0 : x;
            y = y < 0 ? 0 : y;

            Style = "top:" + (y - 10).ToString(_culture) + "px;left:" + x.ToString(_culture) + "px;background-color: rgb(255, 255, 255) !important; color:black !important; " +
                    "position:absolute;border:1px solid rgb(112, 112, 112); padding-left : 3px; padding-right : 2px;" + "padding-bottom : 2px; padding-top : 2px; font-size:12px; font-family: 'Segoe UI';pointer-events: none;";
            updateStyleAttributes(Style);
            InvokeAsync(StateHasChanged);
        }
        #endregion

        #region Private Methods

        void updateStyleAttributes(string style)
        {
            _trimTooltipAttribute.Clear();
            _trimTooltipAttribute = [];
            _trimTooltipAttribute = SfBaseUtils.UpdateDictionary("style", style, _trimTooltipAttribute);
        }

        #endregion
    }
}