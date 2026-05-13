using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

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
            if (Series?.Container is not null && Series.Renderer is not null && Series.Renderer.IsStaticSSR())
            {
                Series.Container._seriesContainer?.Prerender();
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
