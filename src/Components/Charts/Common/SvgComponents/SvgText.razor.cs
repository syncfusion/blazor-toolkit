using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// A Blazor component that renders SVG text elements with full accessibility and styling support.
    /// </summary>
    /// <remarks>
    /// The SvgText component provides a wrapper around SVG text elements with support for text positioning, styling, transforms, and accessibility attributes.
    /// </remarks>
    public partial class SvgText
    {
        #region Fields
        double _opacity { get; set; } = 1;
        Dictionary<string, object>? _htmlAttributes { get; set; }
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the X coordinate of the text element.
        /// </summary>
        /// <value>The X coordinate as a string. Default: <see langword="null"/>.</value>
        [Parameter]
        public string X { get; set; } = null!;

        /// <summary>
        /// Gets or sets the Y coordinate of the text element.
        /// </summary>
        /// <value>The Y coordinate as a string. Default: <see langword="null"/>.</value>
        [Parameter]
        public string Y { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for the text element.
        /// </summary>
        /// <value>The element ID. Default: <see langword="null"/>.</value>
        [Parameter]
        public string Id { get; set; } = null!;

        /// <summary>
        /// Gets or sets the fill color of the text.
        /// </summary>
        /// <value>The fill color (e.g., "black", "#000000"). Default: <see langword="null"/>.</value>
        [Parameter]
        public string Fill { get; set; } = null!;

        /// <summary>
        /// Gets or sets the font size of the text.
        /// </summary>
        /// <value>The font size (e.g., "12px", "1em"). Default: <see langword="null"/>.</value>
        [Parameter]
        public string FontSize { get; set; } = null!;

        /// <summary>
        /// Gets or sets the font style of the text.
        /// </summary>
        /// <value>The font style (e.g., "normal", "italic"). Default: <see langword="null"/>.</value>
        [Parameter]
        public string FontStyle { get; set; } = null!;

        /// <summary>
        /// Gets or sets the font family of the text.
        /// </summary>
        /// <value>The font family name. Default: <see langword="null"/>.</value>
        [Parameter]
        public string FontFamily { get; set; } = null!;

        /// <summary>
        /// Gets or sets the font weight of the text.
        /// </summary>
        /// <value>The font weight (e.g., "normal", "bold", "600"). Default: <see langword="null"/>.</value>
        [Parameter]
        public string FontWeight { get; set; } = null!;

        /// <summary>
        /// Gets or sets the text anchor alignment.
        /// </summary>
        /// <value>The text anchor value (e.g., "start", "middle", "end"). Default: <see langword="null"/>.</value>
        [Parameter]
        public string TextAnchor { get; set; } = null!;

        /// <summary>
        /// Gets or sets the text content to be rendered.
        /// </summary>
        /// <value>The text string. Default: <see langword="null"/>.</value>
        [Parameter]
        public string Text { get; set; } = null!;

        /// <summary>
        /// Gets or sets the child content to be rendered inside the text element.
        /// </summary>
        /// <value>The child render fragment. Default: <see langword="null"/>.</value>
        [Parameter]
        public RenderFragment ChildContent { get; set; } = null!;

        /// <summary>
        /// Gets or sets the dominant baseline of the text.
        /// </summary>
        /// <value>The dominant baseline value (e.g., "auto", "middle"). Default: <see langword="null"/>.</value>
        [Parameter]
        public string DominantBaseline { get; set; } = null!;

        /// <summary>
        /// Gets or sets the SVG transform attribute.
        /// </summary>
        /// <value>The transform string (e.g., "translate(10, 20)"). Default: <see langword="null"/>.</value>
        [Parameter]
        public string Transform { get; set; } = null!;

        /// <summary>
        /// Gets or sets the accessibility text (aria-label) for screen readers.
        /// </summary>
        /// <value>The accessibility label. Default: <see cref="string.Empty"/>.</value>
        [Parameter]
        public string AccessibilityText { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the ARIA role of the text element.
        /// </summary>
        /// <value>The role value. Default: "text".</value>
        [Parameter]
        public string Role { get; set; } = "text";

        /// <summary>
        /// Gets or sets the tab index for keyboard navigation.
        /// </summary>
        /// <value>The tab index value. Default: <see cref="string.Empty"/>.</value>
        [Parameter]
        public string TabIndex { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets inline CSS styles for the text element.
        /// </summary>
        /// <value>The CSS style string. Default: <see langword="null"/>.</value>
        [Parameter]
        public string Style { get; set; } = null!;

        /// <summary>
        /// Gets or sets the clip-path attribute for clipping the text.
        /// </summary>
        /// <value>The clip-path value (e.g., "url(#clip)"). Default: <see cref="string.Empty"/>.</value>
        [Parameter]
        public string ClipPath { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the title attribute for tooltip text.
        /// </summary>
        /// <value>The title text. Default: "Text Element".</value>
        [Parameter]
        public string Title { get; set; } = "Text Element";

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> HtmlAttributes { get { return _htmlAttributes ?? null!; } set { _htmlAttributes = value; } }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Updates the text content and optionally changes its color.
        /// </summary>
        /// <param name="text">The new text content to display.</param>
        /// <param name="color">Optional color to apply to the text. If <see langword="null"/>, color is not changed.</param>
        internal void ChangeText(string text, string color = null!)
        {
            Text = text;
            if (color is not null)
            {
                Fill = color;
            }
            StateHasChanged();
        }

        /// <summary>
        /// Handles cleanup when the component is disposed.
        /// </summary>
        internal void Dispose()
        {
            Text = null!;
            ChildContent = null!;
        }
        #endregion
    }
}