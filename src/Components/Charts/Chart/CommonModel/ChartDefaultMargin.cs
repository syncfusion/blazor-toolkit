using Microsoft.AspNetCore.Components;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides the options for customizing the bottom, left, right, and top margins of the chart component.
    /// </summary>
    public class ChartDefaultMargin : ChartSubComponent
    {
        #region Fields
        private double _bottom = double.NaN;
        private double _top = double.NaN;
        private double _right = double.NaN;
        private double _left = double.NaN;

        internal bool _isPropertyChanged;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the bottom margin for the chart component.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> representing the space in pixels added below the chart component.
        /// The default value is <c>double.NaN</c>.
        /// </value>
        /// <remarks>
        /// Adjusting the bottom margin allows for additional space at the bottom of the chart,
        /// which is useful for accommodating labels or other elements that extend beyond the chart area.
        /// </remarks>
        [Parameter]
        public virtual double Bottom { get; set; } = double.NaN;

        /// <summary>
        /// Gets or sets the left margin for the chart component.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> representing the space in pixels added to the left of the chart component.
        /// The default value is <c>double.NaN</c>.
        /// </value>
        /// <remarks>
        /// The left margin is primarily used to create buffer space on the left side, ideal for accommodating additional labels or annotations.
        /// </remarks>
        [Parameter]
        public virtual double Left { get; set; } = double.NaN;

        /// <summary>
        /// Gets or sets the right margin for the chart component.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> representing the space in pixels added to the right of the chart component.
        /// The default value is <c>double.NaN</c>.
        /// </value>
        /// <remarks>
        /// Adjusting the right margin is often necessary to provide extra space for labels or other chart features that extend beyond the chart's immediate area.
        /// </remarks>
        [Parameter]
        public virtual double Right { get; set; } = double.NaN;

        /// <summary>
        /// Gets or sets the top margin for the chart component.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> representing the space in pixels added above the chart component.
        /// The default value is <c>double.NaN</c>.
        /// </value>
        /// <remarks>
        /// The top margin is useful for creating space at the top of the chart to accommodate titles or legends that might otherwise overlap with chart data.
        /// </remarks>
        [Parameter]
        public virtual double Top { get; set; } = double.NaN;
        #endregion

        #region Lifecycle methods
        /// <summary>
        /// Handles parameter changes and processes property updates.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (_bottom != Bottom ||
                _left != Left ||
                _right != Right ||
                _top != Top)
            {
                _bottom = Bottom;
                _left = Left;
                _right = Right;
                _top = Top;

                _isPropertyChanged = true;
            }
        }
        #endregion
    }
}