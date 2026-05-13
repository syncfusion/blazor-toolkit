using Microsoft.AspNetCore.Components.Rendering;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Represents a chart element descriptor used by renderers.
    /// </summary>
    public interface IChartElement
    {
        /// <summary>
        /// Gets or sets the renderer key used to identify the renderer instance.
        /// </summary>
        /// <value>The renderer key string.</value>
        string RendererKey { get; set; }

        /// <summary>
        /// Gets or sets the renderer component type for this element.
        /// </summary>
        /// <value>The <see cref="Type"/> of the renderer component.</value>
        Type RendererType { get; set; }
    }

    /// <summary>
    /// Container renderer that manages a collection of chart element renderers and their lifecycle.
    /// </summary>
    public class ChartRendererContainer : ChartRenderer
    {
        #region Fields
        private Queue<IChartElementRenderer> _rendererQueue = new();
        private bool _firstRender = true;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the container should prerender its children.
        /// </summary>
        /// <value><c>true</c> when prerender is required; otherwise <c>false</c>.</value>
        internal bool ContainerPrerender { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the container is rendering a trend line.
        /// </summary>
        internal bool IsTrendLine { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the container is updating.
        /// </summary>
        internal bool ContainerUpdate { get; set; }

        /// <summary>
        /// Gets or sets the default renderer types used by the container.
        /// </summary>
        internal virtual List<Type> DefaultRendererType { get; set; } = [];

        /// <summary>
        /// Gets the ordered list of chart elements managed by the container.
        /// </summary>
        internal List<IChartElement> Elements { get; private set; } = [];

        /// <summary>
        /// Gets the active renderer instances associated with the container.
        /// </summary>
        internal List<IChartElementRenderer> Renderers { get; private set; } = [];
        #endregion

        #region Protected Methods

        /// <summary>
        /// Orders the elements list with the provided ordering.
        /// </summary>
        /// <param name="chartElements">The ordered list of elements to apply.</param>
        protected void OrderTheElements(List<IChartElement> chartElements)
        {
            Elements = chartElements;
        }

        /// <summary>
        /// Allows derived classes to customize how elements are stored when added.
        /// </summary>
        /// <param name="element">The element to add to internal collection.</param>
        protected virtual void AddCustomElement(IChartElement element)
        {
            Elements.Add(element);
        }

        /// <summary>
        /// Allows derived classes to add custom element behavior when an element is added.
        /// </summary>
        /// <param name="element">The element that was added.</param>
        protected virtual void OnElementAdded(IChartElement element)
        {
        }

        /// <summary>
        /// Invoked when an element is removed; override to customize behavior.
        /// </summary>
        /// <param name="element">The removed element.</param>
        protected virtual void OnElementRemoved(IChartElement element)
        {
        }

        /// <summary>
        /// Invoked when a renderer is added; provides the associated element if available.
        /// </summary>
        /// <param name="renderer">The renderer that was added.</param>
        /// <param name="element">The associated element or <c>null</c>.</param>
        protected virtual void OnRendererAdded(IChartElementRenderer renderer, IChartElement element)
        {
        }

        /// <summary>
        /// Invoked when a renderer is removed; override to customize behavior.
        /// </summary>
        /// <param name="renderer">The renderer that was removed.</param>
        protected virtual void OnRendererRemoved(IChartElementRenderer renderer)
        {
        }

        /// <summary>
        /// Builds the component render tree and provides this container as a cascading value.
        /// </summary>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/> used to build the UI.</param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (builder is null)
            {
                return;
            }

            this.CreateCascadingValue(builder, 0, 1, this, 2,
               BuildRenderers);
            RendererShouldRender = false;
        }

        /// <summary>
        /// Builds renderer components for each element using their RendererType.
        /// </summary>
        /// <param name="builder">The render tree builder to use.</param>
        protected virtual void BuildRenderers(RenderTreeBuilder builder)
        {
            if (builder is null)
            {
                return;
            }

            int seq = 0;
            foreach (IChartElement element in Elements)
            {
                if (element.RendererType is not null)
                {
                    builder.OpenComponent(seq++, element.RendererType);
                    builder.CloseComponent();
                }
            }
        }

        /// <summary>
        /// Determines whether the component should render.
        /// </summary>
        /// <returns><c>true</c> when rendering is required; otherwise <c>false</c>.</returns>
        protected override bool ShouldRender()
        {
            return RendererShouldRender || ContainerPrerender;
        }

        /// <summary>
        /// Sorts series elements by their Z-order. Safe to call when elements are not series.
        /// </summary>
        protected void SortSeriesByZOrder()
        {
            Elements = [.. Elements.OrderBy(element => (element as ChartSeries ?? null!).ZOrder)];
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Enqueues a renderer for later processing.
        /// </summary>
        /// <param name="renderer">The renderer to enqueue.</param>
        internal void AddToRenderQueue(IChartElementRenderer renderer)
        {
            _rendererQueue.Enqueue(renderer);
        }

        /// <summary>
        /// Ensures default renderer container values are initialized on first render.
        /// </summary>
        internal void SetDefaultRendererContainerValues()
        {
            if (_firstRender)
            {
                SetDefaultRendererValues();
                _firstRender = false;
            }
        }

        /// <summary>
        /// Triggers a prerender of the container on the renderer thread.
        /// </summary>
        internal void Prerender()
        {
            ContainerUpdate = true;
            _ = InvokeAsync(StateHasChanged);
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a chart element to the container if it does not already exist.
        /// </summary>
        /// <param name="element">The chart element to add.</param>
        public void AddElement(IChartElement element)
        {
            if (!Elements.Contains(element))
            {
                ContainerPrerender = true;
                AddCustomElement(element);
                OnElementAdded(element);
            }
        }

        /// <summary>
        /// Removes the specified element from the container.
        /// </summary>
        /// <param name="element">The element to remove.</param>
        public void RemoveElement(IChartElement element)
        {
            if (Elements.Contains(element))
            {
                ContainerPrerender = true;
                _ = Elements.Remove(element);
                OnElementRemoved(element);
            }
        }

        /// <summary>
        /// Adds a renderer instance to the container and invokes hook for element association.
        /// </summary>
        /// <param name="renderer">The renderer instance to add.</param>
        public virtual void AddRenderer(IChartElementRenderer renderer)
        {
            if (!Renderers.Contains(renderer))
            {
                ContainerPrerender = false;
                RendererShouldRender = true;
                Renderers.Add(renderer);

                int index = Renderers.IndexOf(renderer);
                IChartElement? element = (index <= Elements.Count - 1 && index != -1) ? Elements[index] : null!;
                OnRendererAdded(renderer, element);
            }
        }

        /// <summary>
        /// Removes a renderer instance from the container.
        /// </summary>
        /// <param name="renderer">The renderer instance to remove.</param>
        public void RemoveRenderer(IChartElementRenderer renderer)
        {
            if (Renderers.Contains(renderer))
            {
                ContainerPrerender = false;
                RendererShouldRender = true;
                _ = Renderers.Remove(renderer);
                OnRendererRemoved(renderer);
            }
        }

        /// <summary>
        /// Processes the queued renderers by invalidating each renderer's render state.
        /// </summary>
        public override void ProcessRenderQueue()
        {
            while (_rendererQueue.Count != 0)
            {
                IChartElementRenderer renderer = _rendererQueue.Dequeue();
                renderer.InvalidateRender();
            }
        }
        #endregion
    }
}
