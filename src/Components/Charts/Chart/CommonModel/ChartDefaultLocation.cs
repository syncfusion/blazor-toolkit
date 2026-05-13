using Microsoft.AspNetCore.Components;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides options for customizing the location of chart elements, such as legends and titles.
    /// </summary>
    public class ChartDefaultLocation : ChartSubComponent
    {
        #region Fields
        private double _xCoordinate;
        private double _yCoordinate;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the cascading chart parent component reference.
        /// </summary>
        /// <value>
        /// The parent <see cref="SfChart"/> component, or <see langword="null"/> if not within a chart context.
        /// </value>
        [CascadingParameter]
        internal SfChart? Chart { get; set; }

        /// <summary>
        /// Gets or sets the x-coordinate of the legend in pixels.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> representing the horizontal position of the legend. The value is measured in pixels.
        /// </value>
        /// <remarks>
        /// Adjusting this property changes the x-position of the chart legend, allowing it to be precisely aligned within the chart area.
        /// </remarks>
        [Parameter]
        public virtual double X { get; set; }

        /// <summary>
        /// Gets or sets the y-coordinate of the legend in pixels.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> representing the vertical position of the legend. The value is measured in pixels.
        /// </value>
        /// <remarks>
        /// Changing this property affects the y-position of the chart legend, enabling precise vertical placement within the chart layout.
        /// </remarks>
        [Parameter]
        public virtual double Y { get; set; }
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

            if (_xCoordinate != X || _yCoordinate != Y)
            {
                _xCoordinate = X;
                _yCoordinate = Y;
                _isPropertyChanged = true;
            }
        }
        #endregion
    }
}