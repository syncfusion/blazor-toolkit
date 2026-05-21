using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides options to customize the scrollbar of a chart axis.
    /// </summary>
    /// <remarks>
    /// This component allows you to configure scrollbar behavior and appearance for horizontal or vertical chart axes.
    /// It cascades configuration to child <see cref="ChartAxisScrollbarSettingsRange"/> components.
    /// </remarks>
    public class ChartAxisScrollbarSettings : ChartCommonScrollbarSettings
    {
        #region Fields
        [CascadingParameter]
        private ChartAxis? PrevChartAxis { get; set; }
        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Performs component initialization and registers this scrollbar configuration with the parent chart axis.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Tracker is ChartAxis axis)
            {
                PrevChartAxis = axis;
            }
            PrevChartAxis?.UpdateAxisProperties("ScrollbarSettings", this);
        }

        /// <summary>
        /// Handles parameter changes and updates the axis properties if the renderer is ready.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (PrevChartAxis is not null && PrevChartAxis.Renderer is not null)
            {
                PrevChartAxis.UpdateAxisProperties("ScrollbarSettings", this);
            }
        }

        /// <summary>
        /// Cleans up resources when the component is disposed.
        /// </summary>
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
            PrevChartAxis = null;
            ChildContent = null!;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Updates the range configuration for this scrollbar.
        /// </summary>
        /// <param name="range">The scrollbar range settings object.</param>
        /// <remarks>
        /// This method is called internally when a child <see cref="ChartAxisScrollbarSettingsRange"/> component initializes.
        /// </remarks>
        internal void UpdateRange(object range)
        {
            Range = (ChartAxisScrollbarSettingsRange)range;
        }
        #endregion
    }
}