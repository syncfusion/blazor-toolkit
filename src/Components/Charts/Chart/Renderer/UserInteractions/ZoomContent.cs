using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renders and manages the SVG rectangle used for interactive zoom selection on the chart.
    /// </summary>
    /// <remarks>
    /// This component is a light-weight renderer-only component. It expects a cascading <see cref="SfChart"/>
    /// instance and creates a <see cref="RectOptions"/> instance used by the chart's SVG renderer.
    /// </remarks>
    public class ZoomContent : ComponentBase
    {
        #region Properties
        /// <summary>
        /// Gets or sets the parent chart instance supplied via cascading parameter.
        /// </summary>
        [CascadingParameter]
        internal SfChart? Chart { get; set; }

        /// <summary>
        /// Gets or sets the rectangle options used to render the zoom selection area.
        /// </summary>
        /// <value>
        /// A <see cref="RectOptions"/> instance describing the rectangle's geometry, stroke and fill.
        /// </value>
        internal RectOptions? Options { get; set; }
        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Initializes component state and prepares the rectangle options.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetRectOption();
        }
        #endregion

        #region Protected Methods

        /// <summary>
        /// Builds the render tree to draw the zoom rectangle using the chart's SVG renderer.
        /// </summary>
        /// <param name="builder">The render tree builder provided by the framework.</param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (builder is null)
            {
                return;
            }

            base.BuildRenderTree(builder);
            _ = Chart?._svgRenderer?.RectElementList?.Remove(Chart._svgRenderer.RectElementList.Find(item => item.Id == Options?.Id) ?? null!);
            Chart?._svgRenderer?.RenderRect(builder, Options ?? null!);
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Creates or updates the <see cref="Options"/> instance for the zoom rectangle.
        /// </summary>
        internal void SetRectOption()
        {
            Options = new RectOptions(Chart?.ID + "_ZoomArea", 0, 0, 0, 0, 1, Chart?._chartThemeStyle?.SelectionRectStroke ?? string.Empty, Chart?._chartThemeStyle?.SelectionRectFill ?? string.Empty, 0, 0, 1)
            {
                Transform = string.Empty,
                DashArray = "3"
            };
        }
        #endregion
    }
}
