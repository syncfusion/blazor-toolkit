using Microsoft.AspNetCore.Components;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Represents a collection of strip lines used to shade different ranges in the chart's plot area.
    /// </summary>
    public class ChartStriplines : ChartSubComponent, ISubcomponentTracker
    {
        #region Fields
        private int _pendingParametersSetCount;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the cascading parent chart axis component.
        /// </summary>
        [CascadingParameter]
        internal ChartAxis? Axis { get; set; }

        #endregion

        #region Lifecycle methods

        /// <summary>
        /// Initializes the striplines collection and establishes the parent-axis relationship.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Tracker is ChartAxis chartAxis)
            {
                Axis = chartAxis;
            }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Increments the pending parameters set counter when a subcomponent begins parameter updates.
        /// </summary>
        void ISubcomponentTracker.PushSubcomponent()
        {
            _pendingParametersSetCount++;
        }

        /// <summary>
        /// Decrements the pending parameters set counter when a subcomponent finishes parameter updates.
        /// Triggers re-rendering of stripline containers when all updates are complete.
        /// </summary>
        void ISubcomponentTracker.PopSubcomponent()
        {
            _pendingParametersSetCount--;
            if (_pendingParametersSetCount == 0)
            {
                Axis?.Container?._striplineBehindContainer?.Prerender();
                Axis?.Container?._striplineOverContainer?.Prerender();
            }
        }
        #endregion
    }
}