using Microsoft.AspNetCore.Components.Rendering;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renders the outer chart border (background rectangle and optional background image).
    /// </summary>
    /// <remarks>
    /// This renderer is responsible for drawing the chart background rectangle and
    /// an optional background image. It minimizes re-renders by tracking available rect changes.
    /// </remarks>
    public class ChartBorderRenderer : ChartRenderer
    {
        #region Fields
        private Rect? _availableRect;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the chart border settings used for rendering.
        /// </summary>
        /// <value>The <see cref="ChartBorder"/> instance; may be <c>null</c>.</value>
        internal ChartBorder? ChartBorder { get; set; }
        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Initializes the renderer and registers it with the owner chart.
        /// </summary>
        protected override void OnInitialized()
        {
            AddToRenderQueue(this);
            if (Owner is { })
            {
                Owner._chartBorderRenderer = this;
            }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Ensures a default <see cref="ChartBorder"/> exists.
        /// </summary>
        private void SetDefaultBorder()
        {
            if (ChartBorder is not null)
            {
                return;
            }

            ChartBorder = new ChartBorder();
        }

        /// <summary>
        /// Renders the chart border rectangle and optional background image.
        /// </summary>
        /// <param name="builder">Render tree builder instance used to write markup.</param>
        private void RenderChartBorder(RenderTreeBuilder builder)
        {
            double width = (Owner?.AvailableSize.Width ?? 0) - (ChartBorder?.Width ?? 0);
            double height = (Owner?.AvailableSize.Height ?? 0) - (ChartBorder?.Width ?? 0);

            Owner?._svgRenderer?.RenderRect(builder, new RectOptions
            {
                Id = Owner.ID + "_ChartBorder",
                Fill = !string.IsNullOrEmpty(Owner.BackgroundImage) ? "transparent" : (!string.IsNullOrEmpty(Owner.Background) ? Owner.Background : Owner._chartThemeStyle?.Background) ?? string.Empty,
                Stroke = ChartBorder?.Color ?? string.Empty,
                StrokeWidth = ChartBorder?.Width ?? 0,
                Opacity = 1,
                X = (ChartBorder?.Width ?? 0) * 0.5,
                Y = (ChartBorder?.Width ?? 0) * 0.5,
                Width = width,
                Height = height,
                AriaHidden = "true"
            });

            if (!string.IsNullOrEmpty(Owner?.BackgroundImage))
            {
                UpdateImageOptions(builder, width, height);
            }
        }

        /// <summary>
        /// Renders the background image with predictable attributes.
        /// </summary>
        /// <param name="builder">Render tree builder instance.</param>
        /// <param name="width">Computed width for the image in pixels.</param>
        /// <param name="height">Computed height for the image in pixels.</param>
        private void UpdateImageOptions(RenderTreeBuilder builder, double width, double height)
        {
            if (Owner == null)
            {
                return;
            }
            ImageOptions image = new()
            {
                Id = Owner.ID + "_ChartBackground",
                Width = width,
                Height = height,
                Href = Owner.BackgroundImage,
                X = 0,
                Y = 0,
                Visibility = "visible",
                PreserveAspectRatio = "none"
            };
            Owner._svgRenderer?.RenderImage(builder, image);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Builds the render tree for the chart border when required.
        /// </summary>
        /// <param name="builder">The render tree builder.</param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            SetDefaultBorder();
            base.BuildRenderTree(builder);
            if (_availableRect is not null)
            {
                RenderChartBorder(builder);
            }

            RendererShouldRender = false;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Called when theme changes; schedules a render pass.
        /// </summary>
        internal void OnThemeChanged()
        {
            RendererShouldRender = true;
            ProcessRenderQueue();
        }

        /// <summary>
        /// Initializes default values for this renderer and aligns with the owner's initial size.
        /// </summary>
        internal override void SetDefaultRendererValues()
        {
            Owner?.InitializeStaticChart();
            HandleChartSizeChange(Owner?.InitialRect ?? null!);
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Handles chart size changes; triggers a re-render only when the rectangle changes.
        /// </summary>
        /// <param name="rect">The new available rectangle for the chart.</param>
        public override void HandleChartSizeChange(Rect rect)
        {
            if (_availableRect != rect)
            {
                _availableRect = rect;
                RendererShouldRender = true;
            }
        }

        #endregion
    }
}