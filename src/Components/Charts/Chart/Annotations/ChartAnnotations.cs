using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Represents the collection of annotations added to a chart.
    /// </summary>
    /// <remarks>
    /// An annotation is a user-defined HTML element that can be placed on a chart.
    /// Use annotations to enhance visual appeal and provide additional context or information.
    /// </remarks>
    public class ChartAnnotations : ChartSubComponent, ISubcomponentTracker
    {
        #region Fields
        private int _pendingParametersSetCount;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the accessibility description for the <see cref="ChartAnnotations"/>.
        /// </summary>
        /// <value>
        /// Accepts a string that defines the accessibility description for the <see cref="ChartAnnotations"/>. The default value is an empty string.
        /// </value>
        /// <remarks>
        /// Use this property to provide an accessibility description for the <see cref="ChartAnnotations"/>.
        /// </remarks>
        [Parameter]
        public string AccessibilityDescription { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the accessibility role for the <see cref="ChartAnnotations"/>.
        /// </summary>
        /// <value>
        /// Accepts a string that defines the accessibility role for the <see cref="ChartAnnotations"/>. The default value is <b>null</b>.
        /// </value>
        /// <remarks>
        /// Use this property to provide an accessibility role for the <see cref="ChartAnnotations"/>.
        /// </remarks>
        [Parameter]
        public string AccessibilityRole { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the accessibility keyboard navigation focus option for the <see cref="ChartAnnotations"/>.
        /// </summary>
        /// <value>
        /// Accepts the boolean value to enable or disable the keyboard navigation for the <see cref="ChartAnnotations"/>. The default value is <b>true</b>.
        /// </value>
        /// <remarks>
        /// Use this property to toggle the keyboard navigation focus for the <see cref="ChartAnnotations"/>.
        /// </remarks>
        [Parameter]
        public bool Focusable { get; set; } = true;

        /// <summary>
        /// Gets the parent chart component that owns this annotations collection.
        /// </summary>
        /// <value>
        /// An instance of <see cref="SfChart"/>, or <see langword="null"/> if not cascaded.
        /// </value>
        [CascadingParameter]
        internal SfChart? Chart { get; set; }

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Performs component initialization.
        /// </summary>
        /// <remarks>Registers this annotations container with the parent chart component.</remarks>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Chart is null)
            {
                return;
            }
            Chart._annotations = this;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Increments the pending parameters set counter when a subcomponent is pushed.
        /// </summary>
        void ISubcomponentTracker.PushSubcomponent()
        {
            _pendingParametersSetCount++;
        }

        /// <summary>
        /// Decrements the pending parameters set counter and triggers annotation prerender when all subcomponents are processed.
        /// </summary>
        void ISubcomponentTracker.PopSubcomponent()
        {
            _pendingParametersSetCount--;
            if (_pendingParametersSetCount == 0)
            {
                Chart?._annotationContainer?.Prerender();
            }
        }
        #endregion
    }
}

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Manages the rendering container for chart annotations.
    /// </summary>
    /// <remarks>
    /// Initializes the annotation renderer container and sets up initial configurations.
    /// Handles annotation lifecycle, rendering updates, and size change notifications.
    /// </remarks>
    public class ChartAnnotationRendererContainer : ChartRendererContainer
    {
        #region Lifecycle Methods

        /// <summary>
        /// Performs container initialization.
        /// </summary>
        /// <remarks>Registers this container with the parent chart and initializes stock chart annotations if present.</remarks>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Owner is null)
            {
                return;
            }
            Owner._annotationContainer = this;
        }
        #endregion

        #region Protected Methods

        /// <summary>
        /// Handles the addition of an annotation element to the container.
        /// </summary>
        /// <param name="element">The chart element being added.</param>
        /// <remarks>Marks the container for re-render if the chart has been initialized.</remarks>
        protected override void OnElementAdded(IChartElement element)
        {
            if (Owner?.InitialRect is not null)
            {
                RendererShouldRender = true;
                StateHasChanged();
            }
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Adds this container to the render queue.
        /// </summary>
        internal void AddContainerRenderer()
        {
            AddToRenderQueue(this);
        }

        /// <summary>
        /// Updates all annotation renderers with current rendering options.
        /// </summary>
        /// <remarks>Recalculates rendering options and processes the render queue for all annotation renderers.</remarks>
        internal void UpdateRenderers()
        {
            foreach (ChartAnnotationRenderer renderer in Renderers.Cast<ChartAnnotationRenderer>())
            {
                renderer.CalculateRenderingOption();
                renderer.ProcessRenderQueue();
            }
        }

        /// <summary>
        /// Marks the renderer as invalid and triggers a state change if not disposed.
        /// </summary>
        internal void InvalidateRenderer()
        {
            RendererShouldRender = true;
            if (!IsDisposed)
            {
                _ = InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Handles changes in chart size and adjusts annotation rendering as necessary.
        /// </summary>
        /// <param name="rect">A <see cref="Rect"/> object representing the new dimensions of the chart area.</param>
        /// <remarks>
        /// This method recalculates rendering options for all annotation renderers whenever the chart size changes,
        /// ensuring that annotations are rendered correctly within the updated chart boundaries.
        /// </remarks>
        public override void HandleChartSizeChange(Rect rect)
        {
            IsRendererUpdate = Renderers.Count > 0;
            foreach (ChartAnnotationRenderer renderer in Renderers.Cast<ChartAnnotationRenderer>())
            {
                renderer.CalculateRenderingOption();
            }
        }

        /// <summary>
        /// Processes the render queue and renders all pending chart annotations.
        /// </summary>
        /// <remarks>
        /// This method iterates through all renderers in the queue and triggers the rendering process,
        /// ensuring that all annotations are rendered according to their configurations and current chart state.
        /// </remarks>
        public override void ProcessRenderQueue()
        {
            foreach (ChartRenderer renderer in Renderers.Cast<ChartRenderer>())
            {
                renderer.ProcessRenderQueue();
            }
        }

        /// <summary>
        /// Adds a renderer for an annotation to the container and initiates its rendering process.
        /// </summary>
        /// <param name="renderer">An instance of <see cref="IChartElementRenderer"/> representing the renderer for a chart annotation.</param>
        /// <remarks>
        /// This method is responsible for managing the renderer lifecycle by adding it to the render queue, and if initialized, immediately starting the rendering process for the provided annotation renderer.
        /// </remarks>
        public override void AddRenderer(IChartElementRenderer renderer)
        {
            ContainerPrerender = false;
            RendererShouldRender = true;
            Renderers.Add(renderer);
            int index = Renderers.IndexOf(renderer);

            if (renderer is ChartAnnotationRenderer annotationRenderer)
            {
                annotationRenderer.Annotation = Elements[index] as ChartAnnotation;

                if (Owner?.InitialRect is not null)
                {
                    annotationRenderer.IsAnnotationRendered = true;
                    annotationRenderer.CalculateRenderingOption();
                    annotationRenderer.ProcessRenderQueue();
                }
            }
        }
        #endregion
    }
}