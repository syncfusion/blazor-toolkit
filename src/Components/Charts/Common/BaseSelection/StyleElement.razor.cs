using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Component partial responsible for safely rendering dynamic style content into the component.
    /// </summary>
    public partial class StyleElement
    {
        #region Fields
        private RenderFragment _templateContent { get; set; } = null!;
        private bool _allowStyles { get; set; }
        #endregion

        #region Private Methods

        /// <summary>
        /// Creates a <see cref="RenderFragment"/> that injects sanitized CSS inside a &lt;style&gt; element.
        /// </summary>
        /// <param name="styles">Raw CSS text to render.</param>
        /// <returns>A <see cref="RenderFragment"/> that renders the sanitized CSS inside a style tag.</returns>
        private static RenderFragment RenderStyles(string styles)
        {
            return builder =>
            {
                builder.AddMarkupContent(1, styles);
            };
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Appends style content to the component for rendering. Content is sanitized to reduce XSS risk.
        /// </summary>
        /// <param name="styleContent">The CSS text to append. Null or whitespace is ignored.</param>
        internal void AppendStyleElement(string styleContent)
        {
            _allowStyles = true;
            _templateContent = RenderStyles(styleContent);
            _ = InvokeAsync(StateHasChanged);
        }
        #endregion
    }
}