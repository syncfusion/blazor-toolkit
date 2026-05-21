using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Component backing for chart tooltip rendering and templating.
    /// </summary>
    /// <remarks>
    /// Holds the template context and positioning state for a tooltip instance.
    /// </remarks>
    public partial class ChartTooltipComponent
    {
        #region Fields
        private object? _templateContext;
        private readonly CultureInfo _culture = CultureInfo.InvariantCulture;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the template fragment used to render tooltip content.
        /// </summary>
        [Parameter]
        public RenderFragment<object> GivenContent { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier for the tooltip element.
        /// </summary>
        /// <value>The DOM id string. Default: <c>string.Empty</c>.</value>
        [Parameter]
        public string ID { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets CSS class names applied to the tooltip wrapper.
        /// </summary>
        /// <value>Space-separated class list. Default: <c>string.Empty</c>.</value>
        [Parameter]
        public string Class { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets inline styles for the tooltip wrapper.
        /// </summary>
        /// <value>Inline style string. Default: <c>string.Empty</c>.</value>
        [Parameter]
        public string Style { get; set; } = string.Empty;
        #endregion

        #region Private Methods

        /// <summary>
        /// Produces a render fragment for the template content.
        /// </summary>
        /// <returns>A <see cref="RenderFragment"/> that renders the GivenContent with the current context.</returns>
        private RenderFragment TemplateElements()
        {
            return builder =>
            {
                int seq = 0;
                if (GivenContent is not null)
                {
                    builder.OpenElement(seq++, "div");
                    builder.AddContent(3, GivenContent(_templateContext ?? null!));
                    builder.CloseElement();
                }
            };
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Updates the tooltip content, position and internal state, then requests a re-render.
        /// </summary>
        /// <param name="data">Render fragment used to render tooltip content.</param>
        /// <param name="location">Target location for the tooltip (client coordinates).</param>
        /// <param name="chartData">Context object passed to the template.</param>
        /// <param name="visible">Whether the tooltip should be visible.</param>
        /// <param name="chart">Reference to the owning chart instance.</param>
        internal void ChangeContent(RenderFragment<object> data, ChartEventLocation location, object chartData, bool visible = true, SfChart chart = null!)
        {
            _templateContext = chartData;

            if (chart is not null)
            {
                Style = "top:" + location.Y.ToString(_culture) + "px;left:" + location.X.ToString(_culture) + "px;pointer-events:none; position:absolute;z-index: 1;visibility:" + (visible ? "visible" : "hidden") + ";";
            }

            GivenContent = data;
            _ = InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Clears the template content and requests a re-render.
        /// </summary>
        internal void TemplateFadeOut()
        {
            GivenContent = null!;
            _ = InvokeAsync(StateHasChanged);
        }
        #endregion
    }
}