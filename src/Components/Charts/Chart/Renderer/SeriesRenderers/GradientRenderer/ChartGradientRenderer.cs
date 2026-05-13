using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// <c>ChartGradientRenderer</c> is responsible for rendering SVG gradient definitions for Series, Trendlines, and Indicators.
    /// </summary>
    /// <remarks>
    /// The renderer creates &lt;linearGradient&gt; and &lt;radialGradient&gt; definitions inside a &lt;defs&gt; element.
    /// It listens for layout/size changes and re-renders only when gradients exist and the associated series are visible.
    /// </remarks>
    internal class ChartGradientRenderer : ChartRenderer, IChartElementRenderer
    {
        #region Constants
        private const string LINEAR_GRADIENT_ELEMENT = "linearGradient";
        private const string RADIAL_GRADIENT_ELEMENT = "radialGradient";
        private const string STOP_ELEMENT = "stop";
        private const string GRADIENT_UNITS_USER_SPACE = "userSpaceOnUse";
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the associated series for which gradients will be rendered.
        /// </summary>
        /// <value>The <see cref="ChartSeries"/> instance. Default: <c>null</c>.</value>
        [Parameter]
        public ChartSeries? Series { get; set; } = null;

        /// <summary>
        /// Gets or sets the trendline whose gradient definitions should be rendered.
        /// </summary>
        /// <value>The <see cref="ChartTrendline"/> instance. Default: <c>null</c>.</value>
        [Parameter]
        public ChartTrendline? Trendline { get; set; } = null;

        /// <summary>
        /// Gets or sets the currently referenced series renderer used to determine visibility.
        /// </summary>
        /// <value>Internal <see cref="ChartSeriesRenderer"/> instance or <c>null</c>.</value>
        internal ChartSeriesRenderer? SeriesRenderer { get; set; }

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Initializes gradient renderer associations for Series, Trendline and Indicator.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            SvgRenderer = Owner?._svgRenderer;

            string ownerId = Owner?.ID ?? string.Empty;

            if (Series is not null)
            {
                InitializeSeriesGradients(ownerId);
                SeriesRenderer ??= Series.Renderer;
            }

            if (Trendline is not null)
            {
                InitializeTrendlineGradients(ownerId);
                SeriesRenderer ??= Trendline.Renderer;
            }
        }

        /// <summary>
        /// Cleans up references held by the renderer.
        /// </summary>
        protected override ValueTask DisposeAsyncCore()
        {
            SeriesRenderer = null;
            Series = null;
            Trendline = null;
            return base.DisposeAsyncCore();
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes gradient renderers for the Series, setting unique IDs and renderer references for any defined gradients.
        /// </summary>
        private void InitializeSeriesGradients(string ownerId)
        {
            ChartLinearGradient? seriesLinear = Series?.LinearGradient;
            if (seriesLinear?.RendererType is not null)
            {
                seriesLinear.Renderer = this;
                seriesLinear.ID = $"{ownerId}_Series_{Series?.Renderer?.Index}_LinearGradient";
            }

            ChartRadialGradient? seriesRadial = Series?.RadialGradient;
            if (seriesRadial?.RendererType is not null)
            {
                seriesRadial.Renderer = this;
                seriesRadial.ID = $"{ownerId}_Series_{Series?.Renderer?.Index}_RadialGradient";
            }
        }

        /// <summary>
        /// Initializes gradient renderers for the Trendline, setting unique IDs and renderer references for any defined gradients.
        /// </summary>
        private void InitializeTrendlineGradients(string ownerId)
        {
            ChartLinearGradient? trendLinear = Trendline?.LinearGradient;
            if (trendLinear?.RendererType is not null)
            {
                trendLinear.Renderer = this;
                trendLinear.ID = $"{ownerId}_Trendline_{Trendline?.Renderer?.Index}_LinearGradient";
            }

            ChartRadialGradient? trendRadial = Trendline?.RadialGradient;
            if (trendRadial?.RendererType is not null)
            {
                trendRadial.Renderer = this;
                trendRadial.ID = $"{ownerId}_Trendline_{Trendline?.Renderer?.Index}_RadialGradient";
            }
        }

        /// <summary>
        /// Determines whether any gradient stops are present across Series, Trendline, or Indicator.
        /// </summary>
        /// <returns><see langword="true"/> if any gradient color stops are defined; otherwise <see langword="false"/>.</returns>
        private bool HasAnyGradientStops()
        {
            bool seriesHas = Series?.LinearGradient?.GradientColorStops?.Count > 0 || Series?.RadialGradient?.GradientColorStops?.Count > 0;
            bool trendHas = Trendline?.LinearGradient?.GradientColorStops?.Count > 0 || Trendline?.RadialGradient?.GradientColorStops?.Count > 0;
            return seriesHas || trendHas;
        }

        /// <summary>
        /// Renders all defined gradients for the Series, Trendline, and Indicator into the provided <see cref="RenderTreeBuilder"/>.
        /// </summary>
        private void RenderAllGradients(RenderTreeBuilder builder)
        {
            if (SvgRenderer is null || Series is null)
            {
                return;
            }
            ChartLinearGradient? seriesLinear = Series.LinearGradient;
            if (seriesLinear?.RendererType is not null)
            {
                RenderLinearGradient(builder, seriesLinear, SvgRenderer);
            }

            ChartRadialGradient? seriesRadial = Series.RadialGradient;
            if (seriesRadial?.RendererType is not null)
            {
                RenderRadialGradient(builder, seriesRadial, SvgRenderer);
            }

            ChartLinearGradient? trendlineLinear = Trendline?.LinearGradient;
            if (trendlineLinear?.RendererType is not null)
            {
                RenderLinearGradient(builder, trendlineLinear, SvgRenderer);
            }

            ChartRadialGradient? trendlineRadial = Trendline?.RadialGradient;
            if (trendlineRadial?.RendererType is not null)
            {
                RenderRadialGradient(builder, trendlineRadial, SvgRenderer, null);
            }

            if (ChartHelper.NeedsLegendHorizontalLineGradient(Series))
            {
                if (seriesLinear?.RendererType is not null)
                {
                    RenderLinearGradient(builder, seriesLinear, SvgRenderer, $"{Series.Type}Legend");
                }
                if (seriesRadial?.RendererType is not null)
                {
                    RenderRadialGradient(builder, seriesRadial, SvgRenderer, $"{Series.Type}Legend");
                }
            }

            if (Trendline?.TargetSeries is not null)
            {
                string? suffix = Trendline.TargetSeries.LegendShape == LegendShape.HorizontalLine ? $"{Trendline.TargetSeries.LegendShape}Legend" : null;

                if (trendlineLinear?.RendererType is not null)
                {
                    RenderLinearGradient(builder, trendlineLinear, SvgRenderer, suffix);
                }

                if (trendlineRadial?.RendererType is not null)
                {
                    RenderRadialGradient(builder, trendlineRadial, SvgRenderer, suffix);
                }
            }
        }

        /// <summary>
        /// Renders a single linear gradient definition into the provided <see cref="RenderTreeBuilder"/> with an optional ID suffix for uniqueness.
        /// </summary>
        private static void RenderLinearGradient(RenderTreeBuilder builder, ChartLinearGradient gradient, SvgRendering svgRenderer, string? idSuffix = null)
        {
            string? gradientId = idSuffix is null ? gradient.ID : $"{gradient.ID}_{idSuffix}";

            builder.OpenElement(svgRenderer.Seq++, LINEAR_GRADIENT_ELEMENT);
            builder.AddAttribute(svgRenderer.Seq++, "id", gradientId);
            builder.AddAttribute(svgRenderer.Seq++, "x1", gradient.X1.ToString(CultureInfo.InvariantCulture));
            builder.AddAttribute(svgRenderer.Seq++, "y1", gradient.Y1.ToString(CultureInfo.InvariantCulture));
            builder.AddAttribute(svgRenderer.Seq++, "x2", gradient.X2.ToString(CultureInfo.InvariantCulture));
            builder.AddAttribute(svgRenderer.Seq++, "y2", gradient.Y2.ToString(CultureInfo.InvariantCulture));

            if (idSuffix is not null)
            {
                builder.AddAttribute(svgRenderer.Seq++, "gradientUnits", GRADIENT_UNITS_USER_SPACE);
            }

            foreach (ChartGradientColorStop stop in gradient.GradientColorStops)
            {
                builder.OpenElement(svgRenderer.Seq++, STOP_ELEMENT);
                builder.AddAttribute(svgRenderer.Seq++, "offset", $"{stop.Offset}%");
                builder.AddAttribute(svgRenderer.Seq++, "stop-color", stop.GetProcessedColor());
                builder.AddAttribute(svgRenderer.Seq++, "stop-opacity", stop.Opacity.ToString(CultureInfo.InvariantCulture));
                builder.CloseElement();
            }

            builder.CloseElement();
        }

        /// <summary>
        /// Renders a single radial gradient definition into the provided <see cref="RenderTreeBuilder"/> with an optional ID suffix for uniqueness.
        /// </summary>
        private static void RenderRadialGradient(RenderTreeBuilder builder, ChartRadialGradient gradient, SvgRendering svgRenderer, string? idSuffix = null)
        {
            string? gradientId = idSuffix is null ? gradient.ID : $"{gradient.ID}_{idSuffix}";

            builder.OpenElement(svgRenderer.Seq++, RADIAL_GRADIENT_ELEMENT);
            builder.AddAttribute(svgRenderer.Seq++, "id", gradientId);
            builder.AddAttribute(svgRenderer.Seq++, "cx", gradient.Cx.ToString(CultureInfo.InvariantCulture));
            builder.AddAttribute(svgRenderer.Seq++, "cy", gradient.Cy.ToString(CultureInfo.InvariantCulture));
            builder.AddAttribute(svgRenderer.Seq++, "r", gradient.R.ToString(CultureInfo.InvariantCulture));
            builder.AddAttribute(svgRenderer.Seq++, "fx", gradient.Fx.ToString(CultureInfo.InvariantCulture));
            builder.AddAttribute(svgRenderer.Seq++, "fy", gradient.Fy.ToString(CultureInfo.InvariantCulture));

            if (idSuffix is not null)
            {
                builder.AddAttribute(svgRenderer.Seq++, "gradientUnits", GRADIENT_UNITS_USER_SPACE);
            }

            foreach (ChartGradientColorStop stop in gradient.GradientColorStops)
            {
                builder.OpenElement(svgRenderer.Seq++, STOP_ELEMENT);
                builder.AddAttribute(svgRenderer.Seq++, "offset", $"{stop.Offset}%");
                builder.AddAttribute(svgRenderer.Seq++, "stop-color", stop.GetProcessedColor());
                builder.AddAttribute(svgRenderer.Seq++, "stop-opacity", stop.Opacity.ToString(CultureInfo.InvariantCulture));
                builder.CloseElement();
            }

            builder.CloseElement();
        }
        #endregion

        #region Protected Methods

        /// <summary>
        /// Returns a boolean indicating whether the component should render, based on the presence of any gradient stops and visibility of associated series.
        /// </summary>
        protected override bool ShouldRender()
        {
            return HasAnyGradientStops();
        }

        /// <summary>
        /// Builds the render tree for the component, rendering a &lt;defs&gt; element containing all defined gradients if any exist and the associated series are visible.
        /// </summary>
        /// <param name="builder"></param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            SvgRenderer ??= Owner?._svgRenderer;
            SeriesRenderer ??= Series?.Renderer;

            if (SeriesRenderer is not null && SeriesRenderer.Series is not null && !SeriesRenderer.Series.Visible)
            {
                return;
            }

            builder.OpenElement(0, "defs");
            RenderAllGradients(builder);
            builder.CloseElement();
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Invoked when a gradient value changed; updates render flags and requests rerender.
        /// </summary>
        internal void GradientValueChanged()
        {
            RendererShouldRender = HasAnyGradientStops() && (Series?.Renderer?.Series?.Visible ?? Trendline?.Renderer?.Series?.Visible ?? false);
            InvalidateRender();
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Handles layout change notifications from the chart.
        /// </summary>
        public void HandleLayoutChange()
        {

        }

        /// <summary>
        /// Requests a component re-render asynchronously.
        /// </summary>
        public void InvalidateRender()
        {
            _ = InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Handles chart size changes, updating render flags based on the presence of gradients and visibility of associated series.
        /// </summary>
        public override void HandleChartSizeChange(Rect rect)
        {
            RendererShouldRender = HasAnyGradientStops() && (Series?.Renderer?.Series?.Visible ?? Trendline?.Renderer?.Series?.Visible ?? false);

            if (RendererShouldRender)
            {
                SeriesRenderer = Series?.Renderer;
            }
        }
        #endregion
    }
}
