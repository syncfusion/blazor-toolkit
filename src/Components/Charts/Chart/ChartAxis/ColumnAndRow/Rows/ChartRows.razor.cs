using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Charts.Internal;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Represents a collection of rows within a chart, managing their initialization and customization.
    /// </summary>
    /// <remarks>
    /// This internal component handles the lifecycle and configuration of chart row containers,
    /// including default row element removal and axis assignment to rows. It integrates with the
    /// parent <see cref="SfChart"/> component through cascading parameters.
    /// </remarks>
    public class ChartRows : ChartSubComponent
    {
        #region Properties
        /// <summary>
        /// Gets or sets the parent chart component that owns this row collection.
        /// </summary>
        [CascadingParameter]
        private SfChart? Owner { get; set; }
        #endregion

        #region Lifecycle Methods

        /// <summary>Initializes the chart rows component and establishes the parent chart reference.</summary>
        /// <remarks>
        /// Called during component initialization. If available, captures the parent <see cref="SfChart"/>
        /// component from the <see cref="Owner"/> to ensure proper integration with the chart hierarchy.
        /// </remarks>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Tracker is SfChart chart)
            {
                Owner = chart;
            }
        }

        /// <summary>
        /// Handles parameter changes and reconfigures row containers.
        /// </summary>
        /// <remarks>
        /// Invoked after any parameter changes. Performs cleanup of default row elements and ensures axes are properly assigned to rows within the container.
        /// </remarks>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (Owner?._rowContainer is not null && Owner._rowContainer.Elements.Count > 1)
            {
                Owner._rowContainer.RemoveDefaultRowElement();
                if (Owner._rowContainer.Renderers.Any(renderer => (renderer as ChartRowRenderer)?.Axes?.Count == 0))
                {
                    Owner._rowContainer.AssignAxisToRow();
                }
            }
        }
        #endregion
    }
}
