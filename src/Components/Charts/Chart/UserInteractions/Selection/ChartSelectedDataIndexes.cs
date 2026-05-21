using Microsoft.AspNetCore.Components;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Represents a collection of selected data indexes in a chart that distinguishes them from other data points.
    /// </summary>
    /// <remarks>
    /// Declare one or more <see cref="ChartSelectedDataIndex"/> children inside this component to preselect points.
    /// </remarks>
    public class ChartSelectedDataIndexes : ChartSubComponent
    {
        #region Properties

        /// <summary>
        /// Gets or sets the owning chart supplied via cascading parameters.
        /// </summary>
        /// <value>
        /// The parent <c>SfChart</c> instance that consumes the selected data indexes.
        /// </value>
        [CascadingParameter]
        internal SfChart? Chart { get; set; }

        /// <summary>
        /// Gets or sets the collection of selected data indexes.
        /// </summary>
        /// <value>
        /// A mutable list containing instances of <see cref="ChartSelectedDataIndex"/> that represent selections.
        /// </value>
        internal List<ChartSelectedDataIndex> SelectedData { get; set; } = [];

        #endregion

        #region Lifecycle Methods

        /// <exclude />
        /// <summary>
        /// Called by the framework when the component is initialized.
        /// Passes the selected collection to the owning chart to apply initial selection.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Chart is null)
            {
                return;
            }
            Chart.SelectedDataIndexes = SelectedData;
        }

        /// <summary>
        /// Releases references to allow garbage collection.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override ValueTask DisposeAsyncCore()
        {
            Chart = null;
            ChildContent = null!;
            SelectedData = null!;
            return base.DisposeAsyncCore();
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Removes a previously registered selected index.
        /// </summary>
        /// <param name="dataIndex">The data index instance to remove.</param>
        internal void RemoveDataIndex(ChartSelectedDataIndex dataIndex)
        {
            if (SelectedData?.Count > 0 && SelectedData.Contains(dataIndex))
            {
                _ = SelectedData.Remove(dataIndex);
            }
        }

        #endregion
    }
}