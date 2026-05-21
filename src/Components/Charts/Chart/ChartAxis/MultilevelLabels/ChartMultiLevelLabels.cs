using Microsoft.AspNetCore.Components;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Represents a collection of <see cref="ChartMultiLevelLabel"/> objects.
    /// </summary>
    /// <remarks>
    /// This component manages and provides options to customize the multi-level labels of a chart axis.
    /// It serves as a container for <see cref="ChartMultiLevelLabel"/> child components and coordinates
    /// with the parent <see cref="ChartAxis"/> to apply label configuration settings.
    /// </remarks>
    public class ChartMultiLevelLabels : ChartSubComponent
    {
        #region Properties

        /// <summary>
        /// Gets or sets the cascading parent chart axis component.
        /// </summary>
        [CascadingParameter]
        internal ChartAxis? Axis { get; set; }

        /// <summary>
        /// Gets or sets the cascading parent multi-level label component.
        /// </summary>
        internal List<ChartMultiLevelLabel> MultiLevelLabels { get; set; } = [];

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Invoked when the component initializes.
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
            Axis?.UpdateAxisProperties("MultiLevelLabels", MultiLevelLabels);
        }

        /// <summary>
        /// Disposes resources used by this component.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override ValueTask DisposeAsyncCore()
        {
            Axis = null;
            ChildContent = null!;
            MultiLevelLabels?.Clear();
            return base.DisposeAsyncCore();
        }
        #endregion
    }
}