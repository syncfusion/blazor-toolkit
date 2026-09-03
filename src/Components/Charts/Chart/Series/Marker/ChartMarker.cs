using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.Toolkit.Charts.Internal;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Represents a marker in a chart series, used to highlight data points.
    /// </summary>
    /// <remarks>
    /// This component is intended to be used inside a chart series and automatically propagates
    /// its values to the owning series for proper rendering and legend updates.
    /// </remarks>
    public class ChartMarker : ChartCommonMarker, ISubcomponentTracker
    {
        #region Fields
        private int _pendingParametersSetCount;
        #endregion

        #region Properties

        /// <summary>
        /// Gets the owning <see cref="ChartSeries"/> via cascading parameter.
        /// </summary>
        /// <value>The parent chart series instance.</value>
        [CascadingParameter]
        internal ChartSeries? Series { get; set; }

        #endregion

        #region Lifecycle Methods

        /// <exclude />
        /// <summary>
        /// Initializes the component and registers marker with the parent series.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Tracker is ChartSeries chartSeries)
            {
                Series = chartSeries;
            }

            Series?.UpdateSeriesProperties("Marker", this);
        }

        /// <exclude />
        /// <summary>
        /// Applies parameter changes and ensures legend and renderer are updated accordingly.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            Series?.UpdateSeriesProperties("Marker", this);
            UpdateLegend();
        }

        /// <exclude />
        /// <summary>
        /// Builds the render tree for this component.
        /// </summary>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/> instance.</param>
        /// <remarks>
        /// Invokes SSR prerender support when the series is static.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            SetDefaultRendererValues();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Updates legend when marker‑affecting parameters change.
        /// </summary>
        private void UpdateLegend()
        {
            if (Series?.Container?._legendRenderer is not null && Series.Renderer is not null)
            {
                Series.Container._legendRenderer.UpdateLegendShape(Series.Renderer);
                Series.Container._legendRenderer.ProcessRenderQueue();
            }
        }

        /// <summary>
        /// Ensures series prerender is triggered for SSR/static scenarios.
        /// </summary>
        private void SetDefaultRendererValues()
        {
            // Guard everything – any of these can still be null on the first SSR pass
            if (Series == null || Series.Container == null || Series.Renderer == null)
            {
                return;
            }

            if (!Series.Renderer.IsStaticSSR())
            {
                return;
            }

            // 1. Force the series container to run its layout / prerender
            Series.Container._seriesContainer?.Prerender();

            // 2. If the marker renderer component already exists, force it to calculate symbols
            var markerRenderer = Series.Marker?.Renderer;
            if (markerRenderer != null)
            {
                var rect = Series.Container.InitialRect ?? new Rect(0, 0, 0, 0);
                markerRenderer.HandleChartSizeChange(rect);
            }
            else
            {
                // Renderer component has not been created yet.
                // Just ask the series renderer to re-render; the OpenComponent will happen later.
                try
                {
                    Series.Renderer.RendererShouldRender = true;
                    Series.Renderer.ProcessRenderQueue();
                }
                catch
                {
                    // Ignore – under pure SSR ProcessRenderQueue may not be fully ready
                }
            }
        }

        /// <summary>
        /// Marks the start of a nested subcomponent parameter update.
        /// </summary>
        void ISubcomponentTracker.PushSubcomponent()
        {
            _pendingParametersSetCount++;
        }

        /// <summary>
        /// Marks the end of a nested subcomponent parameter update and prerenders when the batch completes.
        /// </summary>
        void ISubcomponentTracker.PopSubcomponent()
        {
            _pendingParametersSetCount--;
            if (_pendingParametersSetCount == 0)
            {
                Series?.Container?._seriesContainer?.Prerender();
            }
        }

        #endregion
    }
}
