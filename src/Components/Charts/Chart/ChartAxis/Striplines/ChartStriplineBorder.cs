using Microsoft.AspNetCore.Components;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Specifies the configuration of border styling for striplines.
    /// </summary>
    /// <remarks>
    /// This component allows customization of stripline borders, including color, width, and style.
    /// Border properties cascade from the parent <see cref="ChartStripline"/> component.
    /// </remarks>
    public class ChartStriplineBorder : ChartDefaultBorder
    {
        #region Fields
        [CascadingParameter]
        private ChartStripline? Parent { get; set; }
        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Initializes the stripline border component and registers it with the parent stripline.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Tracker is ChartStripline chartStripline)
            {
                Parent = chartStripline;
            }
            Parent?.SetBorderValues(this);
        }
        #endregion
    }
}