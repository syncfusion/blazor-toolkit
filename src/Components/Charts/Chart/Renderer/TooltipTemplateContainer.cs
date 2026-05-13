using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Container component that hosts tooltip templates for the chart.
    /// </summary>
    /// <remarks>
    /// This component is rendered into the chart DOM and exposes the owner's
    /// tooltip template region. It ensures the owner reference is set and cleared
    /// during initialization and disposal to avoid memory leaks.
    /// </remarks>
    public class TooltipContainer : ComponentBase
    {
        #region Protected Methods

        /// <summary>
        /// Renders the tooltip template container when an owner-provided template exists.
        /// </summary>
        /// <param name="builder">Render tree builder instance.</param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
        }
        #endregion
    }
}
