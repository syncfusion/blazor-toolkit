using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides options to customize the range of a scrollbar for chart axes.
    /// </summary>
    /// <remarks>
    /// This component is a child of <see cref="ChartAxisScrollbarSettings"/> and is used as a cascading parameter
    /// to define the minimum and maximum range for the axis scrollbar.
    /// </remarks>
    public class ChartAxisScrollbarSettingsRange : ChartCommonScrollbarSettingsRange
    {
        #region Fields
        [CascadingParameter]
        private ChartAxisScrollbarSettings? ScrollBar { get; set; }
        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Performs component initialization and registers this range configuration with the parent scrollbar settings.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Tracker is ChartAxisScrollbarSettings axisScrollbar)
            {
                ScrollBar = axisScrollbar;
            }
            ScrollBar?.UpdateRange(this);
        }

        /// <summary>
        /// Cleans up resources when the component is disposed.
        /// </summary>
        protected override ValueTask DisposeAsyncCore()
        {
            ComponentDispose();
            return base.DisposeAsyncCore();
        }

        internal void ComponentDispose()
        {
            ChildContent = null!;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Sets the minimum and maximum range values for the scrollbar.
        /// </summary>
        /// <param name="min">The minimum range value.</param>
        /// <param name="max">The maximum range value.</param>
        /// <remarks>
        /// This method is called internally to configure the scrollbar's range boundaries.
        /// </remarks>
        internal void SetMinMax(string min, string max)
        {
            Minimum = min;
            Maximum = max;
        }
        #endregion
    }
}