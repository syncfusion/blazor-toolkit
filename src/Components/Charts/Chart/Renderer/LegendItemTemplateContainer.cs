using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Hosts legend item templates rendered outside the SVG layer to allow arbitrary Blazor content.
    /// </summary>
    public class LegendItemTemplateContainer : SfBaseComponent
    {
        #region Properties

        /// <summary>
        /// Cascading chart owner reference.
        /// </summary>
        [CascadingParameter]
        internal SfChart? Owner { get; set; }
        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Registers the container with the owning chart.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Owner is { })
            {
                Owner._legendItemTemplateContainer = this;
            }
        }

        /// <summary>
        /// Performs cleanup for the component instance.
        /// </summary>
        protected override ValueTask DisposeAsyncCore()
        {
            Owner = null;
            return base.DisposeAsyncCore();
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the parent id for the legend template.
        /// </summary>
        private string GetLegendTemplateParentId()
        {
            return Owner?.ID + "_ChartSeries_LegendTemplateCollection";
        }
        #endregion

        #region Protected Methods

        /// <summary>
        /// Renders the legend item templates container and each template element.
        /// </summary>
        /// <param name="builder">Render tree builder instance.</param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (builder is null)
            {
                return;
            }

            int seq = 0;
            base.BuildRenderTree(builder);
            builder.OpenElement(seq++, "div");
            builder.AddAttribute(seq++, "id", GetLegendTemplateParentId());
            builder.AddAttribute(seq++, "class", "e-control");

            if (Owner is not null && Owner._legendRenderer is not null)
            {
                foreach (LegendBase.LegendItemTemplateOptions option in Owner._legendRenderer.TemplateOptions)
                {
                    builder.OpenElement(seq++, "div");
                    builder.AddAttribute(seq++, "id", option.Id);
                    builder.AddAttribute(seq++, "style", option.style);
                    builder.AddContent(seq++, option.LegendTemplate);
                    builder.CloseElement();
                }
            }
            builder.CloseElement();
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Requests a re-render of this container.
        /// </summary>
        internal void InvalidateRender()
        {
            _ = InvokeAsync(StateHasChanged);
        }
        #endregion
    }
}
