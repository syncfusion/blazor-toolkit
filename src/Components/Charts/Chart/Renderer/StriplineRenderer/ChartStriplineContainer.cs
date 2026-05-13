using Microsoft.AspNetCore.Components.Rendering;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Container responsible for managing and rendering stripline renderers.
    /// </summary>
    /// <remarks>
    /// This container coordinates initialization, sizing updates and rendering
    /// for stripline elements associated with chart axes.
    /// </remarks>
    public class ChartStriplineContainer : ChartRendererContainer
    {
        #region Properties
        /// <summary>
        /// Gets or sets the sequence number for rendering operations, used to ensure correct render tree ordering.
        /// </summary>
        protected int Sequence { get; set; }
        /// <summary>
        /// Gets or sets the currently computed clip rectangle for stripline rendering.
        /// </summary>
        protected Rect ClipRect { get; set; } = new Rect();
        #endregion

        #region Private Methods

        /// <summary>
        /// Updates the container clip rectangle from the chart's axis layout and requests a render.
        /// </summary>
        /// <remarks>
        /// Uses new empty Rect when no layout/series clip rect is available to avoid null propagation.
        /// </remarks>
        private void UpdateClipRect()
        {
            RendererShouldRender = true;
            if (Owner?._axisContainer?.AxisLayout is not null)
            {
                ClipRect = Owner._axisContainer.AxisLayout.SeriesClipRect ?? null!;
            }
            _ = InvokeAsync(StateHasChanged);
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Updates each stripline renderer's rendering options and processes its render queue.
        /// </summary>
        internal void UpdateStriplineCollection()
        {
            foreach (ChartStriplineRenderer renderer in Renderers.ToArray().Cast<ChartStriplineRenderer>())
            {
                renderer.CalculateRenderingOptions();
                renderer.ProcessRenderQueue();
            }
        }

        /// <summary>
        /// Initializes defaults for renderers when static server-side rendering is used.
        /// </summary>
        internal override void SetDefaultRendererValues()
        {
            if (Owner?._axisContainer?.AxisLayout is not null)
            {
                ClipRect = Owner._axisContainer.AxisLayout.SeriesClipRect ?? null!;
            }
            HandleChartSizeChange(Owner?.InitialRect ?? new Rect(0, 0, 0, 0));
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Handles chart size changes and initializes stripline renderers.
        /// </summary>
        /// <param name="rect">The new chart rectangle.</param>
        public override void HandleChartSizeChange(Rect rect)
        {
            IsRendererUpdate = Renderers.Count > 0;
            foreach (ChartStriplineRenderer renderer in Renderers.Cast<ChartStriplineRenderer>())
            {
                if (renderer.Stripline is not null)
                {
                    renderer.InitStripline();
                }
            }
        }

        /// <summary>
        /// Adds a renderer to the container and binds it to corresponding element metadata.
        /// </summary>
        /// <param name="renderer">Renderer to add.</param>
        public override void AddRenderer(IChartElementRenderer renderer)
        {
            ContainerPrerender = false;
            RendererShouldRender = true;
            Renderers.Add(renderer);
            int index = Renderers.IndexOf(renderer);
            ChartStriplineRenderer striplineRenderer = renderer as ChartStriplineRenderer ?? null!;

            if (striplineRenderer is not null && Elements.Count > index)
            {
                striplineRenderer.Stripline = Elements[index] as ChartStripline ?? null!;
                striplineRenderer.Index = index;
            }
        }

        /// <summary>
        /// Processes render queue for the container and child renderers.
        /// </summary>
        public override void ProcessRenderQueue()
        {
            UpdateClipRect();
            foreach (ChartStriplineRenderer renderer in Renderers.Cast<ChartStriplineRenderer>())
            {
                renderer.ProcessRenderQueue();
            }
        }
        #endregion
    }

    /// <summary>
    /// Container that renders striplines behind series visuals.
    /// </summary>
    public class ChartStriplineBehindContainer : ChartStriplineContainer
    {
        #region Lifecycle Methods

        /// <summary>
        /// Initializes the behind-stripline container and registers it with the owner chart.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            AddToRenderQueue(this);
            if (Owner is { })
            {
                Owner._striplineBehindContainer = this;
            }
        }
        #endregion

        #region Protected Methods

        /// <summary>
        /// Builds render tree for behind-stripline renderers with an SVG clip path.
        /// </summary>
        /// <param name="builder">RenderTreeBuilder instance.</param>
        protected override void BuildRenderers(RenderTreeBuilder builder)
        {
            if (builder is null)
            {
                return;
            }

            if (IsStaticSSR())
            {
                SetDefaultRendererValues();
            }

            Sequence = 0;
            string id = Owner?.ID + "_stripline_" + "Behind" + "_";
            double width = ClipRect.Width > 0 ? ClipRect.Width : 0,
            height = ClipRect.Height > 0 ? ClipRect.Height : 0;
            RectOptions rectOption = new(id + "ClipRect" + "_Rect", ClipRect.X, ClipRect.Y, width, height, 1, "transparent", "transparent", 0, 0, 1, "visible");

            builder.OpenElement(Sequence++, "g");
            builder.AddAttribute(Sequence++, "id", id + "collections");
            builder.AddAttribute(Sequence++, "clip-path", "url(#" + id + "ClipRect" + ")");
            builder.OpenElement(Sequence++, "defs");
            builder.OpenElement(Sequence++, "clipPath");
            builder.AddAttribute(Sequence++, "id", id + "ClipRect");
            builder.OpenComponent<SvgRect>(Sequence++);
            builder.AddMultipleAttributes(Sequence++, Owner?._svgRenderer?.GetOptions(rectOption));
            builder.CloseComponent();
            builder.CloseElement();
            builder.CloseElement();

            foreach (IChartElement element in Elements)
            {
                if (element.RendererType is not null)
                {
                    builder.OpenComponent(Sequence++, element.RendererType);
                    builder.SetKey(element);
                    builder.CloseComponent();
                }
            }
            builder.CloseElement();
        }
        #endregion
    }

    /// <summary>
    /// Container that renders striplines over series visuals (foreground).
    /// </summary>
    public class ChartStriplineOverContainer : ChartStriplineContainer
    {
        #region Lifecycle Methods

        /// <summary>
        /// Initializes the over-stripline container, registers it with the owner and ensures annotation container is created.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            AddToRenderQueue(this);
            if (Owner is not null)
            {
                Owner._striplineOverContainer = this;
                Owner._annotationContainer?.AddContainerRenderer();
            }
        }
        #endregion

        #region Protected Methods

        /// <summary>
        /// Builds render tree for over-stripline renderers with an SVG clip path.
        /// </summary>
        /// <param name="builder">RenderTreeBuilder instance.</param>
        protected override void BuildRenderers(RenderTreeBuilder builder)
        {
            if (builder is null)
            {
                return;
            }

            if (IsStaticSSR())
            {
                SetDefaultRendererValues();
            }

            Sequence = 0;
            string id = Owner?.ID + "_stripline_" + "Over" + "_";
            double width = ClipRect.Width > 0 ? ClipRect.Width : 0,
            height = ClipRect.Height > 0 ? ClipRect.Height : 0;
            RectOptions rectOption = new(id + "ClipRect" + "_Rect", ClipRect.X, ClipRect.Y, width, height, 1, "transparent", "transparent", 0, 0, 1, "visible");

            builder.OpenElement(Sequence++, "g");
            builder.AddAttribute(Sequence++, "id", id + "collections");
            builder.AddAttribute(Sequence++, "clip-path", "url(#" + id + "ClipRect" + ")");
            builder.OpenElement(Sequence++, "defs");
            builder.OpenElement(Sequence++, "clipPath");
            builder.AddAttribute(Sequence++, "id", id + "ClipRect");
            builder.OpenComponent<SvgRect>(Sequence++);
            builder.AddMultipleAttributes(Sequence++, Owner?._svgRenderer?.GetOptions(rectOption));
            builder.CloseComponent();
            builder.CloseElement();
            builder.CloseElement();

            foreach (IChartElement element in Elements)
            {
                if (element.RendererType is not null)
                {
                    builder.OpenComponent(Sequence++, element.RendererType);
                    builder.SetKey(element);
                    builder.CloseComponent();
                }
            }

            builder.CloseElement();
        }
        #endregion
    }
}
