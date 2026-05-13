using Microsoft.AspNetCore.Components;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides options to customize the scrollbar range of the axis.
    /// </summary>
    public class ChartCommonScrollbarSettingsRange : ChartSubComponent
    {
        #region Fields
        private string? _maximum;
        private string? _minimum;
        #endregion

        #region Properties

        /// <summary> 
        /// Gets or sets the maximum range of a scrollbar.
        /// </summary> 
        /// <value> 
        /// A string representing the maximum range of the scrollbar. The default value is <c>null</c>. 
        /// </value>
        /// <remarks>
        /// This property defines the upper limit of the scrollbar's range.
        /// </remarks>
        [Parameter]
        public string? Maximum { get; set; }

        /// <summary> 
        /// Gets or sets the minimum range of a scrollbar. 
        /// </summary> 
        /// <value> 
        /// A string representing the minimum range of the scrollbar. The default value is <c>null</c>. 
        /// </value> 
        /// <remarks>
        /// This property specifies the lower limit of the scrollbar's range.
        /// </remarks>
        [Parameter]
        public string? Minimum { get; set; }
        #endregion

        #region Lifecycle Methods
        /// <summary>
        /// Invoked when component parameters are set and used to track changes in scrollbar range values.
        /// </summary>
        /// <remarks>
        /// Compares current parameter values with previously stored values and flags the component
        /// when a change is detected.
        /// </remarks>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (_maximum != Maximum || _minimum != Minimum)
            {
                _maximum = Maximum;
                _minimum = Minimum;

                _isPropertyChanged = true;
            }
        }
        #endregion
    }
}
