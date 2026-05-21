using Microsoft.AspNetCore.Components;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Represents the collection of color stops for the gradient fill of a chart <see cref="ChartSeries"/>.
    /// These stops determine how the colors blend across the gradient.
    /// </summary>
    /// <remarks>
    /// Use <see cref="ChartGradientColorStops"/> as a child of <see cref="ChartLinearGradient"/> or <see cref="ChartRadialGradient"/> to
    /// define the ordered set of <see cref="ChartGradientColorStop"/> entries that form the gradient. At least two stops (for example,
    /// offsets 0 and 100) are recommended to produce a visible transition.
    ///
    /// Stops are applied in ascending order of <see cref="ChartGradientColorStop.Offset"/>. For consistent results, ensure offsets are within
    /// the 0–100 range and colors/opacity are set as required by your design.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// @using Syncfusion.Blazor.Toolkit.Charts
    ///
    /// <SfChart>
    ///     <ChartSeriesCollection>
    ///         <ChartSeries Type="ChartSeriesType.Column"
    ///                      XName="Category"
    ///                      YName="Value">
    ///             <ChartLinearGradient X1="0" Y1="0" X2="1" Y2="0">
    ///                 <ChartGradientColorStops>
    ///                     <ChartGradientColorStop Offset="0"   Color="#42A5F5" Opacity="1" />
    ///                     <ChartGradientColorStop Offset="100" Color="#1E88E5" Opacity="1" />
    ///                 </ChartGradientColorStops>
    ///             </ChartLinearGradient>
    ///         </ChartSeries>
    ///     </ChartSeriesCollection>
    /// </SfChart>
    /// ]]></code>
    /// </example>
    public class ChartGradientColorStops : ChartSubComponent
    {
        #region Properties

        /// <summary>
        /// Gets the owning <see cref="ChartLinearGradient"/> when this collection is used under a linear gradient.
        /// </summary>
        /// <value>The linear gradient owner, if any; otherwise, <c>null</c>.</value>
        [CascadingParameter]
        internal ChartLinearGradient? LinearGradient { get; set; }

        /// <summary>
        /// Gets the owning <see cref="ChartRadialGradient"/> when this collection is used under a radial gradient.
        /// </summary>
        /// <value>The radial gradient owner, if any; otherwise, <c>null</c>.</value>
        [CascadingParameter]
        internal ChartRadialGradient? RadialGradient { get; set; }

        /// <summary>
        /// Gets the list of <see cref="ChartGradientColorStop"/> entries that define the gradient.
        /// </summary>
        /// <value>An ordered <see cref="List{T}"/> of color stops.</value>
        internal List<ChartGradientColorStop> GradientColorStops { get; set; } = [];

        #endregion

        #region Lifecycle Methods

        /// <exclude />
        /// <summary>
        /// Initializes the component and resolves its owner via the subcomponent tracker.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Tracker is ChartLinearGradient chartLinearGradient)
            {
                LinearGradient = chartLinearGradient;
            }
            else if (Tracker is ChartRadialGradient chartRadialGradient)
            {
                RadialGradient = chartRadialGradient;
            }
        }

        /// <exclude />
        /// <summary>
        /// Applies parameter changes and pushes the current stops collection to the owner gradient.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (LinearGradient is not null)
            {
                LinearGradient.UpdateGradientProperties(nameof(LinearGradient.GradientColorStops), GradientColorStops);
            }
            else
            {
                RadialGradient?.UpdateGradientProperties(nameof(RadialGradient.GradientColorStops), GradientColorStops);
            }
        }

        /// <summary>
        /// Releases references to allow garbage collection.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override ValueTask DisposeAsyncCore()
        {
            LinearGradient = null;
            RadialGradient = null;
            // Maintain non-null list; clear to free references.
            GradientColorStops.Clear();
            return base.DisposeAsyncCore();
        }

        #endregion
    }
}