using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides options to customize the categories for the chart labels.
    /// </summary>
    /// <remarks>
    /// This component manages the collection of categories for chart labels, allowing for structured labeling within chart axes.
    /// </remarks>
    public class ChartCategories : ChartSubComponent
    {
        #region Properties

        /// <summary>
        /// Gets or sets the cascading parent multi-level label component.
        /// </summary>
        [CascadingParameter]
        internal ChartMultiLevelLabel? MultilevelLabel { get; set; }

        /// <summary>
        /// Gets or sets the parent chart instance.
        /// </summary>
        internal SfChart? Chart { get; set; }

        /// <summary>
        /// Gets or sets the associated axis for the categories.
        /// </summary>
        internal ChartAxis? Axis { get; set; }

        /// <summary>
        /// Gets or sets the collection of chart categories.
        /// </summary>
        internal List<ChartCategory> Categories { get; set; } = [];
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

            if (Tracker is ChartMultiLevelLabel multiLevelLabel)
            {
                MultilevelLabel = multiLevelLabel;
            }
            if (MultilevelLabel?.MultilevelLabelCollection is not null)
            {
                Axis = MultilevelLabel.MultilevelLabelCollection.Axis;
            }
            if (Axis is not null)
            {
                Chart = Axis.Container;
            }

            MultilevelLabel?.UpdateMultiLevelLabelProperties(nameof(Categories), Categories);
        }

        /// <summary>
        /// Disposes resources used by this component.
        /// </summary>
        protected override ValueTask DisposeAsyncCore()
        {
            MultilevelLabel = null;
            ChildContent = null!;
            Categories?.Clear();
            return base.DisposeAsyncCore();
        }
        #endregion
    }
}