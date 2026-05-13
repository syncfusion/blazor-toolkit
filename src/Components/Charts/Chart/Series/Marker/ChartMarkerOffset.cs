using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Represents the offset position of a chart marker, allowing customization of its location on the chart.
    /// </summary>
    /// <remarks>
    /// Use this inside <see cref="ChartMarker"/> to adjust horizontal (<c>X</c>) and vertical (<c>Y</c>) offsets for fine-tuned placement.
    /// </remarks>
    public class ChartMarkerOffset : ChartDefaultLocation
    {
        #region Lifecycle Methods

        /// <exclude />
        /// <summary>
        /// Initializes the subcomponent and pushes the offset instance to its marker owner.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Tracker is ChartMarker chartMarker)
            {
                chartMarker.UpdateMarkerProperties("Offset", this);
            }
        }

        /// <exclude />
        /// <summary>
        /// Applies parameter changes and rebinds the offset to the marker.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (Tracker is ChartMarker chartMarker)
            {
                chartMarker.UpdateMarkerProperties("Offset", this);
            }
        }

        /// <exclude />
        /// <summary>
        /// Disposes component resources and clears references.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override ValueTask DisposeAsyncCore()
        {
            ComponentDispose();
            return base.DisposeAsyncCore();
        }

        #endregion

        #region Internal Methods
        /// <summary>
        /// Disposes component resources and clears references.
        /// </summary>
        internal void ComponentDispose()
        {
            Chart = null;
            ChildContent = null!;
        }
        /// <summary>
        /// Sets the offset values at runtime.
        /// </summary>
        /// <param name="x">The horizontal offset.</param>
        /// <param name="y">The vertical offset.</param>
        internal void SetOffsetValues(double x, double y)
        {
            X = x;
            Y = y;
        }

        #endregion
    }
}
