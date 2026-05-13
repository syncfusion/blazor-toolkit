using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Charts.Internal;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Represents a collection of columns within a chart component, managing their initialization and customization.
    /// </summary>
    /// <remarks>
    /// The component links to an <see cref="SfChart"/> via cascading parameter or a tracker and performs
    /// column/axis assignment during parameter updates.
    /// </remarks>
    public class ChartColumns : ChartSubComponent
    {
        #region Properties
        /// <summary>
        /// Gets or sets the chart owner cascading parameter.
        /// </summary>
        /// <value>
        /// The parent <see cref="SfChart"/> instance when available; otherwise <c>null</c>.
        /// </value>
        [CascadingParameter]
        public SfChart? Owner { get; set; }
        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Performs component initialization and establishes the chart reference if available via tracker.
        /// </summary>
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
        /// Handles parameter changes and applies column-axis assignments when applicable.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (Owner?._columnContainer is not null && Owner._columnContainer.Elements.Count > 1)
            {
                Owner._columnContainer.RemoveDefaultColumnElement();
                if (Owner._columnContainer.Renderers.Any(renderer => (renderer as ChartColumnRenderer)?.Axes?.Count == 0))
                {
                    Owner._columnContainer.AssignAxisToColumn();
                }
            }
        }
        #endregion
    }
}
