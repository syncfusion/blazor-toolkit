using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renders the outer container for chart axes and coordinates adding axis outside renderers.
    /// </summary>
    /// <remarks>
    /// This container registers itself with the owner chart on initialization and coordinates
    /// rendering of child axis outside renderers.
    /// </remarks>
    public class ChartAxisOutsideContainer : ChartRendererContainer
    {
        #region Lifecycle Methods

        /// <summary>
        /// Initializes the container and registers it with the owner chart.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            AddToRenderQueue(this);
            if (Owner is { })
            {
                Owner._axisOutSideContainer = this;
            }
        }
        #endregion

        #region Protected Methods

        /// <summary>
        /// Controls whether the container should re-render.
        /// </summary>
        /// <returns>Returns true when a render is required.</returns>
        protected override bool ShouldRender()
        {
            return RendererShouldRender || ContainerUpdate;
        }

        /// <summary>
        /// Builds child axis outside renderer components into the render tree.
        /// </summary>
        /// <param name="builder">Render tree builder instance.</param>
        protected override void BuildRenderers(RenderTreeBuilder builder)
        {
            if (builder is not null && Owner?._axisContainer is not null)
            {
                int seq = 0;
                foreach (ChartAxisRenderer axisRenderer in Owner._axisContainer.Renderers.Cast<ChartAxisRenderer>())
                {
                    builder.OpenComponent(seq++, typeof(ChartAxisOutsideRenderer));
                    builder.AddAttribute(seq++, "AxisRenderer", axisRenderer);
                    builder.CloseComponent();
                }
            }
        }
        #endregion
    }

    /// <summary>
    /// Renderer for a single axis outside layout. Implements lightweight rendering responsibilities.
    /// </summary>
    public class ChartAxisOutsideRenderer : ChartRenderer, IChartElementRenderer
    {
        #region Properties

        /// <summary>
        /// Gets or sets the axis renderer associated with this outside renderer.
        /// </summary>
        [Parameter]
        public ChartAxisRenderer AxisRenderer { get; set; } = null!;
        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Performs initialization tasks and links renderer references.
        /// </summary>
        protected override void OnInitialized()
        {
            Owner?._axisOutSideContainer?.AddRenderer(this);
            if (Owner is not null)
            {
                SvgRenderer = Owner._svgRenderer;
            }
            AxisRenderer.OutSideRenderer = this;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Renders polar/radar axis outside collection.
        /// </summary>
        /// <param name="builder">Render tree builder instance.</param>
        private void RenderPolarRadarAxisOutsideCollection(RenderTreeBuilder builder)
        {
            SvgRenderer?.OpenGroupElement(builder, Owner?.ID + "AxisCollection" + 0);

            foreach (KeyValuePair<string, List<PathOptions>> keyValue in AxisRenderer.AxisRenderInfo.AxisGridOptions)
            {
                if (keyValue.Key.Equals(Constants.MajorTickLine, StringComparison.Ordinal))
                {
                    AxisRenderer.DrawLine(builder, keyValue.Value);
                }

                if (keyValue.Key.Equals(Constants.MinorTickLine, StringComparison.Ordinal))
                {
                    AxisRenderer.DrawLine(builder, keyValue.Value);
                }
            }

            if (AxisRenderer.AxisRenderInfo.AxisLine is not null)
            {
                _ = SvgRenderer?.RenderPath(builder, AxisRenderer.AxisRenderInfo.AxisLine);
            }

            if (AxisRenderer.Axis?.LabelTemplate is not null)
            {
                Owner?._axisLabelTemplateContainer?.InvalidateRender();
            }
            else
            {
                SvgRenderer?.OpenGroupElement(builder, Owner?.ID + "AxisLabels" + 0);
                foreach (TextOptions option in AxisRenderer.AxisRenderInfo.AxisLabelOptions)
                {
                    ChartHelper.TextElement(builder, SvgRenderer ?? null!, option);
                }

                builder.CloseElement();
            }

            builder.CloseElement();
        }

        /// <summary>
        /// Renders cartesian axis outside collection with proper grouping and accessibility considerations.
        /// </summary>
        /// <param name="builder">Render tree builder instance.</param>
        private void RenderCartesianAxisOutsideCollection(RenderTreeBuilder builder)
        {
            SvgRenderer?.OpenGroupElement(builder, Owner?.ID + "AxisGroup" + AxisRenderer.Index + "Outside", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "true");

            if (AxisRenderer.AxisRenderInfo.AxisLine is not null && AxisRenderer.IsAxisInside)
            {
                AxisRenderer.DrawLine(builder, AxisRenderer.AxisRenderInfo.AxisLine);
            }

            if (AxisRenderer.IsTickInside)
            {
                foreach (KeyValuePair<string, List<PathOptions>> keyValue in AxisRenderer.AxisRenderInfo.AxisGridOptions)
                {
                    if (keyValue.Key.Equals(Constants.MajorTickLine, StringComparison.Ordinal))
                    {
                        AxisRenderer.DrawLine(builder, keyValue.Value);
                    }

                    if (keyValue.Key.Equals(Constants.MinorTickLine, StringComparison.Ordinal))
                    {
                        AxisRenderer.DrawLine(builder, keyValue.Value);
                    }
                }
            }

            if (AxisRenderer.IsAxisLabelInside)
            {
                if (AxisRenderer.Axis?.LabelTemplate is not null)
                {
                    Owner?._axisLabelTemplateContainer?.InvalidateRender();
                }
                else
                {
                    SvgRenderer?.OpenGroupElement(builder, Owner?.ID + "AxisLabels" + AxisRenderer.Index);
                    foreach (TextOptions option in AxisRenderer.AxisRenderInfo.AxisLabelOptions)
                    {
                        AxisRenderer.ChangeAxisLabelText(option);
                        ChartHelper.TextElement(builder, SvgRenderer ?? null!, option);
                    }

                    builder.CloseElement();
                }

                if (AxisRenderer.AxisRenderInfo.AxisBorder is not null)
                {
                    SvgRenderer?.RenderPath(builder, AxisRenderer.AxisRenderInfo.AxisBorder, "e-pointer-series");
                }

                AxisRenderer.MultiLevelLabelRenderer?.RenderMultilevelLabel(builder);
            }

            if (AxisRenderer.AxisRenderInfo.AxisTitleOption is not null && AxisRenderer.IsAxisInside)
            {
                ChartHelper.TextElement(builder, SvgRenderer ?? null!, AxisRenderer.AxisRenderInfo.AxisTitleOption);
            }

            builder.CloseElement();
        }
        #endregion

        #region Protected Methods

        /// <summary>
        /// Builds the render tree for the axis outside renderer.
        /// Chooses cartesian or polar/radar rendering path.
        /// </summary>
        /// <param name="builder">Render tree builder.</param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            if (builder is not null)
            {
                if (Owner?._axisContainer is not null && Owner._axisContainer.AxisLayout is CartesianAxisLayout)
                {
                    RenderCartesianAxisOutsideCollection(builder);
                }
                else
                {
                    RenderPolarRadarAxisOutsideCollection(builder);
                }

                RendererShouldRender = false;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Invalidates the renderer to trigger a re-render.
        /// </summary>
        public void InvalidateRender()
        {
        }

        /// <summary>
        /// Handles layout change notifications.
        /// </summary>
        public void HandleLayoutChange()
        {
        }

        /// <summary>
        /// Ensures the renderer processes its render queue when requested.
        /// </summary>
        public override void ProcessRenderQueue()
        {
            RendererShouldRender = true;
            base.ProcessRenderQueue();
        }
        #endregion
    }
}
