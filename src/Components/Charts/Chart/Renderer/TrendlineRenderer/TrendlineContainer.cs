using Microsoft.AspNetCore.Components.Rendering;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Container responsible for managing trendline renderers inside a chart.
    /// </summary>
    /// <remarks>
    /// This class coordinates creation, removal and lifecycle operations for trendline series
    /// rendered as separate series renderers. It delegates size change, animation and render queue
    /// processing to contained renderers.
    /// </remarks>
    public class ChartTrendlineContainer : ChartRendererContainer
    {
        #region Lifecycle Methods

        /// <summary>
        /// Initializes the trendline container and registers it with its owner.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            AddToRenderQueue(this);
            if (Owner is { })
            {
                Owner._trendlineContainer = this;
            }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Renders a marker component for the trendline if markers are enabled on the target series.
        /// </summary>
        /// <param name="builder">The RenderTreeBuilder used to emit the marker component.</param>
        /// <param name="element">The trendline element containing marker configuration.</param>
        /// <param name="seq">The sequence number for the render tree builder, incremented as components are added.</param>
        private void RenderTrendlineMarkerIfNeeded(RenderTreeBuilder builder, ChartTrendline element, ref int seq)
        {
            if (element is not null && element.TargetSeries is not null && element.TargetSeries.Marker.Visible && Owner is not null && Owner._shouldRenderMarker)
            {
                builder.OpenComponent(seq++, typeof(ChartMarkerRenderer));
                builder.AddAttribute(seq++, "Series", element.TargetSeries);
                builder.SetKey(element.RendererKey + "-TrendlineMarker-" + element.TargetSeries.RendererKey);
                builder.CloseComponent();
            }
        }

        /// <summary>
        /// Renders a linear gradient component for the trendline if a linear gradient is defined.
        /// </summary>
        /// <param name="builder">The RenderTreeBuilder used to emit the gradient component.</param>
        /// <param name="element">The trendline element containing the linear gradient configuration.</param>
        /// <param name="seq">The sequence number for the render tree builder, incremented as components are added.</param>
        private static void RenderLinearGradientIfAny(RenderTreeBuilder builder, ChartTrendline element, ref int seq)
        {
            if (element is not null && element.LinearGradient is not null && element.LinearGradient.RendererType is not null)
            {
                builder.OpenComponent(seq++, typeof(ChartGradientRenderer));
                builder.AddAttribute(seq++, "Trendline", element);
                builder.SetKey("TrendlineLinearGradient-" + element.RendererKey);
                builder.CloseComponent();
            }
        }

        /// <summary>
        /// Renders a radial gradient component for the trendline if a radial gradient is defined.
        /// </summary>
        /// <param name="builder">The RenderTreeBuilder used to emit the gradient component.</param>
        /// <param name="element">The trendline element containing the radial gradient configuration.</param>
        /// <param name="seq">The sequence number for the render tree builder, incremented as components are added.</param>
        private static void RenderRadialGradientIfAny(RenderTreeBuilder builder, ChartTrendline element, ref int seq)
        {
            if (element is not null && element.RadialGradient is not null && element.RadialGradient.RendererType is not null)
            {
                builder.OpenComponent(seq++, typeof(ChartGradientRenderer));
                builder.AddAttribute(seq++, "Trendline", element);
                builder.SetKey("TrendlineRadialGradient-" + element.RendererKey);
                builder.CloseComponent();
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Called when a chart element is added to the container.
        /// </summary>
        /// <param name="element">The element that was added.</param>
        protected override void OnElementAdded(IChartElement element)
        {
            if (Owner?.InitialRect is not null)
            {
                StateHasChanged();
            }
        }

        /// <summary>
        /// Called when a chart element is removed from the container.
        /// </summary>
        /// <param name="element">The element that was removed.</param>
        protected override void OnElementRemoved(IChartElement element)
        {
            if (element is not null && !IsDisposed)
            {
                RemoveRenderer((element as ChartTrendline ?? null!).TargetSeries?.Renderer ?? null!);
                _ = InvokeAsync(StateHasChanged);
            }
        }

        /// <summary>
        /// Handles renderer additions and wires trendline-specific renderer state.
        /// </summary>
        /// <param name="renderer">Renderer instance added.</param>
        /// <param name="element">Associated chart element.</param>
        protected override void OnRendererAdded(IChartElementRenderer renderer, IChartElement element)
        {
            ChartSeriesRenderer trendLineSeries = renderer as ChartSeriesRenderer ?? null!;
            if (trendLineSeries is not null && element is not null)
            {
                IsTrendLine = true;
                ChartTrendline trendLine = element as ChartTrendline ?? null!;

                trendLine.Renderer = trendLineSeries;
                trendLineSeries.SourceIndex = trendLine.Parent?.Series?.Renderer is not null ? trendLine.Parent.Series.Renderer.Index : Renderers.IndexOf(renderer);
                trendLineSeries.Index = Renderers.IndexOf(renderer);
                trendLineSeries.Interior = !string.IsNullOrEmpty(trendLine.Fill) ? trendLine.Fill : "blue";
                trendLine.Renderer.TrendLineLegendVisibility = trendLine.Visible;

                Owner?._visibleSeriesRenderers.Add(trendLineSeries);
                if (Owner?.InitialRect is not null)
                {
                    AddToRenderQueue(renderer);
                    _ = Owner.ProcessOnLayoutChangeAsync();
                }
            }
        }

        /// <summary>
        /// Called when a renderer is removed from this container.
        /// </summary>
        /// <param name="renderer">Renderer that was removed.</param>
        protected override void OnRendererRemoved(IChartElementRenderer renderer)
        {
            if (!IsDisposed)
            {
                _ = Owner?._visibleSeriesRenderers.Remove(renderer as ChartSeriesRenderer ?? null!);
                _ = Owner?.ProcessOnLayoutChangeAsync();
            }
        }

        /// <summary>
        /// Renders child components for trendlines: trendline series, markers and gradients.
        /// </summary>
        /// <param name="builder">RenderTreeBuilder used to emit components.</param>
        protected override void BuildRenderers(RenderTreeBuilder builder)
        {
            if (builder is null)
            {
                return;
            }
            int seq = 0;
            foreach (ChartTrendline element in Elements.Cast<ChartTrendline>())
            {
                if (element.TargetSeries?.RendererType is not null)
                {
                    builder.OpenComponent(seq++, element.TargetSeries.RendererType);
                    builder.AddAttribute(seq++, "Trendlineseries", element.TargetSeries);
                    builder.SetKey(element.RendererKey + "-" + element.TargetSeries.RendererKey);
                    builder.CloseComponent();

                    RenderTrendlineMarkerIfNeeded(builder, element, ref seq);
                    RenderLinearGradientIfAny(builder, element, ref seq);
                    RenderRadialGradientIfAny(builder, element, ref seq);
                }

            }
            RendererShouldRender = false;
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Initializes data for each trendline element.
        /// </summary>
        internal void ProcessData()
        {
            foreach (ChartTrendline element in Elements.Cast<ChartTrendline>())
            {
                element.TrendlineInitiator?.InitDataSource();
            }
        }

        /// <summary>
        /// Assigns axes to each trendline element.
        /// </summary>
        internal void AssignAxisToTrendline()
        {
            foreach (ChartTrendline trendline in Elements.Cast<ChartTrendline>())
            {
                trendline.TrendlineInitiator?.InitiateAxis();
            }
        }

        /// <summary>
        /// Applies initial animations to contained renderers based on provided animation info.
        /// </summary>
        /// <param name="animationInfo">List of initial animation metadata.</param>
        internal void PerformAnimation(List<InitialAnimationInfo> animationInfo)
        {
            foreach (ChartSeriesRenderer renderer in Renderers.Cast<ChartSeriesRenderer>())
            {
                ChartSeries series = renderer.Series ?? null!;
                if (series.Visible && ((series.Animation.Enable && SyncfusionService?._options.Animation == GlobalAnimationMode.Default) || (SyncfusionService?._options.Animation == GlobalAnimationMode.Enable)))
                {
                    renderer.PerformInitialAnimation(animationInfo);
                    double endTime = series.Animation.Delay + series.Animation.Duration;
                    if (endTime >= Owner?._maxAnimationDuration)
                    {
                        Owner._maxAnimationDuration = endTime;
                        Owner._lastSeriesAnimationIndex = animationInfo.Count - 1;
                    }
                }
            }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Propagates chart size changes to contained series renderers.
        /// </summary>
        /// <param name="rect">The new chart rectangle.</param>
        public override void HandleChartSizeChange(Rect rect)
        {
            IsRendererUpdate = Renderers.Count > 0;
            foreach (ChartSeriesRenderer renderer in Renderers.Cast<ChartSeriesRenderer>())
            {
                renderer.HandleChartSizeChange(rect);
            }
        }

        /// <summary>
        /// Process pending render queues for contained renderers.
        /// </summary>
        public override void ProcessRenderQueue()
        {
            foreach (ChartRenderer renderer in Renderers.Cast<ChartRenderer>())
            {
                renderer.ProcessRenderQueue();
            }
        }
        #endregion
    }
}
