using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Hosts data label templates for series so they can be rendered as Blazor content.
    /// </summary>
    public class DataLabelTemplateContainer : ComponentBase
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
        /// Registers the container instance with the owner chart.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Owner is { })
            {
                Owner._datalabelTemplateContainer = this;
            }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Gets parent id for the data label template.
        /// </summary>
        private string GetDatalabelTemplateParentId(int index)
        {
            if (Owner is null)
            {
                return string.Empty;
            }
            Owner._dataLabelTemplateId = Owner.ID + "_Series_" + index + "_DataLabelCollections";
            return Owner._dataLabelTemplateId;
        }
        #endregion

        #region Protected Methods

        /// <summary>
        /// Renders data label template containers for each visible series that has templates.
        /// </summary>
        /// <param name="builder">Render tree builder instance.</param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (builder is null)
            {
                return;
            }

            base.BuildRenderTree(builder);
            if (Owner?._seriesContainer?.Elements is not null)
            {
                foreach (ChartSeries series in Owner._seriesContainer.Elements.Cast<ChartSeries>())
                {
                    if (series?.Visible == true && series.Marker?.DataLabel?.Renderer is not null && series.Renderer is not null)
                    {
                        int seq = 0;
                        builder.OpenElement(seq++, "div");
                        builder.AddAttribute(seq++, "id", GetDatalabelTemplateParentId(series.Renderer.Index));
                        List<DatalabelTemplateOptions> options = series.Marker.DataLabel.Renderer?._templateOptions ?? null!;

                        if (options is not null)
                        {
                            foreach (DatalabelTemplateOptions templateOption in options)
                            {
                                builder.OpenElement(seq++, "div");
                                builder.AddAttribute(seq++, "id", templateOption.Id);
                                builder.AddAttribute(seq++, "style", templateOption.Style);
                                builder.AddContent(seq++, templateOption.Template);
                                builder.CloseElement();
                            }
                        }

                        builder.CloseElement();
                    }
                }
            }
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Requests a re-render of the data label template container.
        /// </summary>
        internal void InvalidateRender()
        {
            _ = InvokeAsync(StateHasChanged);
        }
        #endregion
    }
}
