using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides the options to customize the border of a chart marker.
    /// </summary>
    /// <remarks>
    /// Use this subcomponent inside <see cref="ChartMarker"/> to adjust marker border color and width.
    /// </remarks>
    public class ChartMarkerBorder : ChartDefaultBorder
    {
        #region Properties

        /// <summary>
        /// Gets or sets the width of the marker border.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> representing the width of the border surrounding the chart marker. The default value is <b>2</b>.
        /// </value>
        /// <remarks>
        /// Controls the thickness of the marker border.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartMarker Visible="true">
        ///     <ChartMarkerBorder Color="red" Width="2"></ChartMarkerBorder>
        /// </ChartMarker>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override double Width { get; set; } = 2;

        #endregion

        #region Lifecycle Methods

        /// <exclude />
        /// <summary>
        /// Initializes the subcomponent and pushes the border instance to its marker owner.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Tracker is ChartMarker chartMarker)
            {
                chartMarker.UpdateMarkerProperties("Border", this);
            }
        }

        /// <exclude />
        /// <summary>
        /// Applies parameter changes and rebinds the border to the marker.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (Tracker is ChartMarker chartMarker)
            {
                chartMarker.UpdateMarkerProperties("Border", this);
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
            Owner = null;
            ChildContent = null!;
        }
        /// <summary>
        /// Sets the border values at runtime.
        /// </summary>
        /// <param name="color">The border color.</param>
        /// <param name="width">The border width.</param>
        internal void SetBorderValues(string? color, double width)
        {
            Color = color;
            Width = width;
        }

        #endregion
    }
}
