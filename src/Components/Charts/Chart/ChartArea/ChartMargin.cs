using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides options for customizing the margin of a chart.
    /// </summary>
    public class ChartMargin : ChartDefaultMargin
    {
        #region Properties
        /// <summary>
        /// Gets or sets the parent <see cref="SfChart"/> component that owns this margin configuration.
        /// </summary>
        /// <value>The parent chart component, or <see langword="null"/> if not yet initialized.</value>
        [CascadingParameter]
        internal SfChart? Owner { get; set; }
        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Performs component initialization and registers this margin instance with the parent chart.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Owner is not null)
            {
                Owner._margin = this;
            }
        }

        /// <summary>
        /// Handles parameter changes and triggers layout recalculation if margins were modified.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (_isPropertyChanged)
            {
                _isPropertyChanged = false;
                Owner?.OnLayoutChange();
            }
        }

        /// <summary>
        /// Disposes resources and clears the parent chart reference.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override ValueTask DisposeAsyncCore()
        {
            Owner = null;
            return base.DisposeAsyncCore();
        }
        #endregion
    }
}