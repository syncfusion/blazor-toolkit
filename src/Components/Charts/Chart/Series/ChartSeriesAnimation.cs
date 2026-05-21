using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Specifies the configuration of the series animation.
    /// </summary>
    /// <remarks>
    /// Apply this component within a <see cref="ChartSeries"/> to control entry and update animations
    /// such as duration and delay. Changes are propagated to the series rendering.
    /// </remarks>
    public class ChartSeriesAnimation : ChartDefaultAnimation
    {
        #region Properties

        [CascadingParameter]
        private ChartSeries? Series { get; set; }

        #endregion

        #region Lifecycle Methods

        /// <exclude />
        /// <summary>
        /// Initializes the animation component and registers it with the parent <see cref="ChartSeries"/>.
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

            Series?.UpdateSeriesProperties("Animation", this);
        }

        /// <exclude />
        /// <summary>
        /// Propagates parameter changes to the parent <see cref="ChartSeries"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            Series?.UpdateSeriesProperties("Animation", this);
        }

        /// <summary>
        /// Disposes animation component resources and releases references to the parent series.
        /// </summary>
        /// <inheritdoc />
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override ValueTask DisposeAsyncCore()
        {
            ComponentDispose();
            return base.DisposeAsyncCore();
        }
        internal void ComponentDispose()
        {
            Container = null;
            Series = null;
        }
        #endregion
    }
}
