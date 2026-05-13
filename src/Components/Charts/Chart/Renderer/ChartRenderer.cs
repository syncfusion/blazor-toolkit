using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.Toolkit.Charts.Internal;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Defines a renderer contract for chart child elements.
    /// </summary>
    public interface IChartElementRenderer
    {
        /// <exclude />
        /// <summary>
        /// Requests the renderer to invalidate and re-render itself.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        void InvalidateRender();

        /// <exclude />
        /// <summary>
        /// Notifies the renderer that layout has changed and it should recompute layout related values.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        void HandleLayoutChange();
    }
}

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides a base renderer component for chart subcomponents.
    /// </summary>
    /// <remarks>
    /// This component manages render queue integration with the owning <see cref="SfChart"/> and
    /// exposes lifecycle hooks for size/layout changes.
    /// </remarks>
    public class ChartRenderer : SfBaseComponent
    {
        #region Properties

        /// <summary>
        /// Indicates whether the renderer should permit a Blazor render pass.
        /// </summary>
        /// <value><see langword="true"/> to allow rendering; otherwise <see langword="false"/>.</value>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal bool RendererShouldRender { get; set; }

        /// <summary>
        /// Indicates whether the renderer update is part of an incremental update flow.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal bool IsRendererUpdate { get; set; }

        /// <summary>
        /// Gets the injected Syncfusion Blazor service instance.
        /// </summary>
        [Inject]
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal new SyncfusionBlazorService? SyncfusionService { get; set; }

        /// <exclude />
        /// <summary>
        /// Gets or sets the SVG renderer helper used by chart renderers.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected SvgRendering? SvgRenderer { get; set; }

        /// <exclude />
        /// <summary>
        /// Gets or sets the owning chart component.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [CascadingParameter]
        public SfChart? Owner { get; set; }

        /// <exclude />
        /// <summary>
        /// Gets or sets child content for the renderer.
        /// </summary>
        /// <value>The render fragment rendered inside this renderer.</value>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Parameter]
        public RenderFragment ChildContent { get; set; } = null!;
        #endregion

        #region Protected Methods

        /// <exclude />
        /// <summary>
        /// Controls whether the component should render. This uses <see cref="RendererShouldRender"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override bool ShouldRender()
        {
            return RendererShouldRender;
        }

        /// <exclude />
        /// <summary>
        /// Builds the render tree for this renderer and provides cascading value to children.
        /// </summary>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/> used to construct the render tree.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (builder is null)
            {
                return;
            }

            if (IsStaticSSR())
            {
                SetDefaultRendererValues();
            }

            this.CreateCascadingValue(builder, 0, 1, this, 2,
               (builder2) =>
               {
                   if (ChildContent is not null)
                   {
                       ChildContent(builder2);
                   }
               });

            RendererShouldRender = false;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Called by parent when parameters are set; processes any queued render requests.
        /// </summary>
        internal virtual void OnParentParameterSet()
        {
            ProcessRenderQueue();
        }

        /// <summary>
        /// Adds the specified renderer to the owner's renderers collection.
        /// </summary>
        /// <param name="renderer">Renderer to add.</param>
        internal void AddToRenderQueue(ChartRenderer renderer)
        {
            Owner?._renderers.Add(renderer);
        }

        /// <summary>
        /// Initializes default renderer values. Safe to call during SSR.
        /// </summary>
        internal virtual void SetDefaultRendererValues()
        {
            try
            {
                HandleChartSizeChange(Owner?.InitialRect ?? null!);
            }
            catch
            {
                if (!IsDisposed)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Returns true when the owner indicates static server-side rendering mode.
        /// </summary>
        /// <returns><see langword="true"/> when static SSR is active; otherwise <see langword="false"/>.</returns>
        internal bool IsStaticSSR()
        {
            return Owner is not null && Owner.IsStaticServerRendering();
        }

        #endregion

        #region Public Methods

        /// <exclude />
        /// <summary>
        /// Override to handle size changes for the chart. Default implementation is a no-op.
        /// </summary>
        /// <param name="rect">The new rectangle representing the chart area.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void HandleChartSizeChange(Rect rect)
        {
        }

        /// <exclude />
        /// <summary>
        /// Enqueues a re-render by invoking StateHasChanged asynchronously.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void ProcessRenderQueue()
        {
            _ = InvokeAsync(StateHasChanged);
        }
        #endregion
    }
}
