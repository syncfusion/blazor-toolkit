using Microsoft.AspNetCore.Components.Rendering;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renderer that draws the chart legend into the SVG layer when a legend exists.
    /// </summary>
    public class CustomLegendRenderer : ChartRenderer
    {
        #region Lifecycle Methods

        /// <summary>
        /// Registers renderer with the render queue and owner chart.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            AddToRenderQueue(this);
            if (Owner is { })
            {
                Owner._customLegendRenderer = this;
            }
        }
        #endregion

        #region Protected Methods

        /// <summary>
        /// Determines whether this renderer should render.
        /// </summary>
        protected override bool ShouldRender()
        {
            return RendererShouldRender;
        }

        /// <summary>
        /// Builds the SVG legend group when legend data is available.
        /// </summary>
        /// <param name="builder">Render tree builder instance.</param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (builder is null)
            {
                return;
            }

            if (Owner?._legendRenderer is not null && Owner._legendRenderer.AvailableRect is not null && Owner._legendRenderer.LegendOptions.Count != 0)
            {
                base.BuildRenderTree(builder);
                Owner._svgRenderer?.OpenGroupElement(builder, Owner._legendRenderer.LegendID + "_g", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "false");
                Owner._legendRenderer.RenderLegend(builder, Owner._svgRenderer ?? null!, Owner._legendRenderer.LegendSettings?.Border ?? null!);
                builder.CloseElement();
            }

            RendererShouldRender = false;
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Default renderer values placeholder.
        /// </summary>
        internal override void SetDefaultRendererValues()
        {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Handles chart size changes and triggers re-render when legend rect exists.
        /// </summary>
        /// <param name="rect">Chart bounds.</param>
        public override void HandleChartSizeChange(Rect rect)
        {
            if (Owner?._legendRenderer?.AvailableRect is not null)
            {
                RendererShouldRender = true;
            }
        }
        #endregion
    }
}
