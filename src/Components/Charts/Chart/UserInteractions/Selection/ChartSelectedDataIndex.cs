using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Represents the index of a selected data point in a chart,
    /// making the selected data point distinguishable from others.
    /// </summary>
    /// <remarks>
    /// Use within <see cref="ChartSelectedDataIndexes"/> to declare initial selection by series and point index.
    /// </remarks>
    public class ChartSelectedDataIndex : ChartDefaultSelectedData
    {
        #region Properties

        /// <summary>
        /// Gets or sets the cascading collection that aggregates all selected data indexes for the owning chart.
        /// </summary>
        /// <value>
        /// A reference to the parent <see cref="ChartSelectedDataIndexes"/> that manages the list of selected indices.
        /// </value>
        [CascadingParameter]
        internal ChartSelectedDataIndexes? SelectedDataCollection { get; set; }

        #endregion

        #region Lifecycle Methods

        /// <exclude />
        /// <summary>
        /// Called by the framework when the component is initialized.
        /// Registers this item with the parent <see cref="ChartSelectedDataIndexes"/>.
        /// </summary>
        /// <remarks>
        /// This method is invoked once per component lifetime.
        /// </remarks>        
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Tracker is ChartSelectedDataIndexes chartSelectedDataIndexes)
            {
                SelectedDataCollection = chartSelectedDataIndexes;
            }

            SelectedDataCollection?.SelectedData?.Add(this);
        }

        /// <exclude />
        /// <summary>
        /// Releases references to allow garbage collection and detaches from parent.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override ValueTask DisposeAsyncCore()
        {
            ChildContent = null!;
            SelectedDataCollection?.RemoveDataIndex(this);
            SelectedDataCollection = null;
            return base.DisposeAsyncCore();
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Creates a <see cref="ChartSelectedDataIndex"/> for the specified point and series indices.
        /// </summary>
        /// <param name="point">Zero-based index of the data point.</param>
        /// <param name="series">Zero-based index of the series.</param>
        /// <returns>A new <see cref="ChartSelectedDataIndex"/> instance.</returns>
        internal static ChartSelectedDataIndex CreateSelectedData(int point, int series)
        {
            return new ChartSelectedDataIndex { Point = point, Series = series };
        }

        #endregion
    }
}