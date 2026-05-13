using Microsoft.AspNetCore.Components.Rendering;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renders the chart area background and border for cartesian charts.
    /// </summary>
    /// <remarks>
    /// This renderer is attached to the owning chart via <see cref="ChartRenderer.Owner"/>.
    /// It draws the area rectangle and optional background image inside the series clip rectangle.
    /// </remarks>
    public class ChartAreaRenderer : ChartRenderer
    {
        #region Properties

        /// <summary>
        /// Gets the last computed chart area rectangle (series clip rect) used for rendering.
        /// </summary>
        /// <value>
        /// The rectangle defining the chart area in pixels; <c>null</c> when not computed.
        /// </value>
        internal Rect? ChartAreaRect { get; private set; }

        /// <summary>
        /// Gets or sets the chart area configuration used for rendering (background, border, image).
        /// </summary>
        /// <value>
        /// The <see cref="ChartArea"/> instance. May be <c>null</c> until default is initialized.
        /// </value>
        internal ChartArea? Area { get; set; }
        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Initializes the renderer and registers it with the owner's render queue.
        /// </summary>
        protected override void OnInitialized()
        {
            AddToRenderQueue(this);
            if (Owner is { })
            {
                Owner._chartAreaRenderer = this;
            }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Ensures <see cref="Area"/> has a default instance when not provided.
        /// </summary>
        private void SetDefaultArea()
        {
            if (Area is not null)
            {
                return;
            }

            Area = new ChartArea();
        }

        /// <summary>
        /// Renders the chart area border and optional background image for cartesian charts.
        /// </summary>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/> used to emit SVG elements.</param>
        private void RenderChartAreaBorder(RenderTreeBuilder builder)
        {
            if (builder is null || (Owner?._axisContainer?.AxisLayout as CartesianAxisLayout)?.SeriesClipRect is null)
            {
                return;
            }

            ChartAreaRect = (Owner?._axisContainer?.AxisLayout as CartesianAxisLayout ?? null!).SeriesClipRect ?? null!;
            Owner?._svgRenderer?.RenderRect
            (
                builder,
                new RectOptions
                {
                    Id = Owner.ID + "_ChartAreaBorder",
                    Fill = !string.IsNullOrEmpty(Area?.BackgroundImage) ? "transparent" : Area?.Background ?? string.Empty,
                    Stroke = !string.IsNullOrEmpty(Area?.Border.Color) ? Area.Border.Color : Owner._chartThemeStyle?.AreaBorder ?? string.Empty,
                    StrokeWidth = Area?.Border.Width ?? 0.5,
                    Opacity = Area?.Opacity ?? 1,
                    X = ChartAreaRect.X,
                    Y = ChartAreaRect.Y,
                    Width = ChartAreaRect.Width > 0 ? ChartAreaRect.Width : 0,
                    Height = ChartAreaRect.Height > 0 ? ChartAreaRect.Height : 0,
                    AriaHidden = "true"
                }
            );

            if (!string.IsNullOrEmpty(Area?.BackgroundImage))
            {
                UpdateImageOptions(builder);
            }
        }

        /// <summary>
        /// Updates the image options for the cartesian charts.
        /// </summary>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/> used to emit SVG elements.</param>
        private void UpdateImageOptions(RenderTreeBuilder builder)
        {
            if (ChartAreaRect is null || Area is null)
            {
                return;
            }

            ImageOptions image = new()
            {
                Id = Owner?.ID + "_ChartAreaBackground",
                Width = ChartAreaRect.Width > 0 ? ChartAreaRect.Width : 0,
                Height = ChartAreaRect.Height > 0 ? ChartAreaRect.Height : 0,
                Href = Area.BackgroundImage,
                X = ChartAreaRect.X,
                Y = ChartAreaRect.Y,
                Visibility = "visible",
                PreserveAspectRatio = "none"
            };
            Owner?._svgRenderer?.RenderImage(builder, image);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Builds the render tree for the chart area: ensures defaults and draws border/image.
        /// </summary>
        /// <param name="builder">The render tree builder.</param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            SetDefaultArea();
            RenderChartAreaBorder(builder);
            RendererShouldRender = false;
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Called when the chart theme changes; requests a re-render and processes the render queue.
        /// </summary>
        internal void OnThemeChanged()
        {
            RendererShouldRender = true;
            ProcessRenderQueue();
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Marks this renderer for re-render when the chart size changes.
        /// </summary>
        /// <param name="rect">The new chart rectangle.</param>
        public override void HandleChartSizeChange(Rect rect)
        {
            RendererShouldRender = true;
        }
        #endregion
    }
}