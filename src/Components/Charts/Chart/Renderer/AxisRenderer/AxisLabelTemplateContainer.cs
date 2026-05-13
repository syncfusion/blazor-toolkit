using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renders axis label templates for a chart. This component hosts rendered label templates
    /// and updates when the owning chart requires re-rendering of templates.
    /// </summary>
    /// <remarks>
    /// The component is intended to be internally cascaded from <see cref="SfChart"/>
    /// and should not be used directly by consumers.
    /// </remarks>
    public class AxisLabelTemplateContainer : SfBaseComponent
    {
        #region Fields
        private bool _disposed;
        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the cascading chart parent.
        /// </summary>
        [CascadingParameter]
        internal SfChart? Owner { get; set; }

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Perform initialization and register this container with the owning chart.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();


            if (Owner is { })
            {
                Owner._axisLabelTemplateContainer = this;
            }


        }

        /// <summary>
        /// Dispose pattern for the component; unregisters from the owner and marks as disposed.
        /// </summary>
        protected override ValueTask DisposeAsyncCore()
        {
            _disposed = true;
            if (Owner is not null && Owner._axisLabelTemplateContainer == this)
            {
                Owner._axisLabelTemplateContainer = null;
                Owner = null;
            }
            return base.DisposeAsyncCore();
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Compute the inline style string for a label template element.
        /// </summary>
        /// <param name="label">Options describing position and size.</param>
        /// <returns>Inline style string with trailing semicolon.</returns>
        private static string GetLabelStyle(LabelTemplateOptions label)
        {
            List<string> styleParts =
                [
                "position: absolute",
                $"left: {label.X}px",
                $"top: {label.Y}px",
                "cursor: default",
                "white-space: nowrap"
            ];

            if (label.TemplateSize.Height > 0 && label.TemplateSize.Width > 0)
            {
                styleParts.Add($"width: {label.Width}px");
                styleParts.Add($"height: {label.Height}px");
            }
            else
            {
                styleParts.Add("visibility: hidden");
            }

            if (!string.IsNullOrEmpty(label.Transform))
            {
                styleParts.Add(label.Transform);
            }
            else
            {
                styleParts.Add("overflow: hidden");
            }

            return string.Join("; ", styleParts) + ";";
        }
        #endregion

        #region Protected Methods

        /// <summary>
        /// Build the render tree containing all axis label templates.
        /// </summary>
        /// <param name="builder">Render tree builder instance.</param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (_disposed || Owner?._axisContainer?.Renderers is null || builder is null)
            {
                return;
            }

            int seq = 0;
            base.BuildRenderTree(builder);

            builder.OpenElement(seq++, "div");
            builder.AddAttribute(seq++, "id", Owner.ID + "_AxisLabelTemplate_Collections");

            foreach (ChartAxisRenderer renderer in Owner._axisContainer.Renderers.Cast<ChartAxisRenderer>())
            {
                ChartAxis axis = renderer.Axis ?? null!;
                if (axis.LabelTemplate is null)
                {
                    continue;
                }

                foreach (LabelTemplateOptions label in renderer.AxisRenderInfo.LabelTemplateOptions)
                {
                    ChartAxisLabelInfo context = label.AxisInfo;
                    if (context is null)
                    {
                        continue;
                    }

                    builder.OpenElement(seq++, "div");
                    builder.AddAttribute(seq++, "id", label.Id);
                    builder.AddAttribute(seq++, "style", GetLabelStyle(label));
                    builder.AddContent(seq++, axis.LabelTemplate(context));
                    builder.CloseElement();
                }
            }
            builder.CloseElement();
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Request a re-render of this component if it has not been disposed.
        /// </summary>
        internal void InvalidateRender()
        {
            if (!_disposed)
            {
                _ = InvokeAsync(StateHasChanged);
            }
        }
        #endregion        
    }
}
