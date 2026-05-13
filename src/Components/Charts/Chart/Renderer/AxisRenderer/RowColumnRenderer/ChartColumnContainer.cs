using Microsoft.AspNetCore.Components.Rendering;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Container responsible for managing chart column renderers.
    /// </summary>
    /// <remarks>
    /// Handles creation, sizing and axis assignment for <see cref="ChartColumnRenderer"/>.
    /// Keeps rendering lifecycle coordinated with the owning chart.
    /// </remarks>
    public class ChartColumnRendererContainer : ChartRendererContainer
    {
        #region Properties

        /// <summary>
        /// Gets or sets the list of default renderer types for this container.
        /// </summary>
        /// <value>A collection of <see cref="Type"/> representing renderer types. Default: <c>ChartColumnRenderer</c>.</value>
        internal override List<Type> DefaultRendererType { get; set; } = [typeof(ChartColumnRenderer)];
        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Initializes container and associates it with the owning chart.
        /// </summary>
        protected override void OnInitialized()
        {
            AddToRenderQueue(this);
            if (Owner is not null)
            {
                Owner._columnContainer = this;
                SvgRenderer = Owner._svgRenderer;
            }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Maps an axis's ColumnIndex to a valid renderer index.
        /// </summary>
        /// <param name="axis">Axis whose column index is used.</param>
        /// <returns>Clamped renderer index.</returns>
        private int GetActualColumn(ChartAxis axis)
        {
            int actualLength = Renderers.Count;
            int pos = Convert.ToInt32(axis.ColumnIndex);
            int result = pos >= actualLength ? actualLength - 1 : (pos < 0 ? 0 : pos);
            return result;
        }

        #endregion

        #region Protected Handlers

        /// <summary>
        /// Handles the addition of a new chart column element by triggering a component re-render.
        /// </summary>
        /// <param name="element">The chart column element that was added.</param>
        protected override void OnElementAdded(IChartElement element)
        {
            StateHasChanged();
        }

        /// <summary>
        /// Handles the removal of a chart column element by removing its renderer and triggering a re-render if appropriate.
        /// </summary>
        /// <param name="element">The chart column element that was removed.</param>
        protected override void OnElementRemoved(IChartElement element)
        {
            if (!IsDisposed)
            {
                _ = Renderers.Remove((element as ChartColumn)?.Renderer ?? null!);
                if (Owner is not null && !Owner.ChartDisposed())
                {
                    _ = InvokeAsync(StateHasChanged);
                }
            }
        }

        /// <summary>
        /// Called when a renderer is added; ensures the backing element exists and wires renderer to element.
        /// </summary>
        /// <param name="renderer">Renderer instance.</param>
        /// <param name="element">Optional backing element.</param>
        protected override void OnRendererAdded(IChartElementRenderer renderer, IChartElement element)
        {
            if (renderer is not null)
            {
                if (element is null)
                {
                    element = new ChartColumn { RendererKey = "chartcolumn_default" };
                    Elements.Add(element);
                }

                (renderer as ChartColumnRenderer ?? null!).ChartColumn = element as ChartColumn ?? null!;
                if (Owner?.InitialRect is not null)
                {
                    _ = Owner.ProcessOnLayoutChangeAsync();
                }
            }
        }

        /// <summary>
        /// Builds the render tree for default renderers when container update is requested.
        /// </summary>
        /// <param name="builder">Render tree builder.</param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (builder is null)
            {
                return;
            }

            int seq = 0;
            if (ContainerUpdate && Elements.Count > 0)
            {
                base.BuildRenderTree(builder);
            }
            else if (ContainerUpdate && Renderers.Count == 0)
            {
                foreach (Type defaultRenderer in DefaultRendererType)
                {
                    builder.OpenComponent(seq++, defaultRenderer);
                    builder.CloseComponent();
                }
            }

            RendererShouldRender = false;
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Assigns horizontal axes to corresponding column renderers.
        /// </summary>
        internal void AssignAxisToColumn()
        {
            int actualIndex;
            ChartColumnRenderer columnRenderer;
            ChartAxis axis;
            Renderers.ForEach(renderer => (renderer as ChartColumnRenderer ?? null!).Axes.Clear());
            if (Owner?._axisContainer is not null)
            {
                foreach (ChartAxisRenderer axisRenderer in Owner._axisContainer.Renderers.Cast<ChartAxisRenderer>())
                {
                    if (axisRenderer.Orientation == Orientation.Horizontal)
                    {
                        axis = axisRenderer.Axis ?? null!;
                        actualIndex = GetActualColumn(axis);
                        columnRenderer = Renderers[actualIndex] as ChartColumnRenderer ?? null!;
                        axisRenderer.SetInverseAndOpposedPosition();
                        columnRenderer.Axes.Add(axis);
                    }
                }
            }
        }

        /// <summary>
        /// Sets default renderer values when no explicit layout has been provided.
        /// </summary>
        internal override void SetDefaultRendererValues()
        {
            HandleChartSizeChange(Owner?.InitialRect ?? new Rect(0, 0, 0, 0));
        }

        /// <summary>
        /// Removes the default placeholder column element (if present) and disposes it properly.
        /// </summary>
        internal void RemoveDefaultColumnElement()
        {
            ChartColumn defaultcolumn = Elements.Find(item => { return item.RendererKey == "chartcolumn_default"; }) as ChartColumn ?? null!;
            if (defaultcolumn is not null)
            {
                defaultcolumn.Container = Owner;
                defaultcolumn.ComponentDispose();
            }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Computes layout for each column renderer based on the provided chart rectangle.
        /// </summary>
        /// <param name="rect">Available chart rectangle.</param>
        public override void HandleChartSizeChange(Rect rect)
        {
            if (rect is null)
            {
                return;
            }

            double remainingWidth = Math.Max(0, rect.Width);
            double columnLeft = rect.X;
            Rect initialRect = rect;

            for (int i = 0, len = Renderers.Count; i < len; i++)
            {
                ChartColumnRenderer renderer = Renderers[i] as ChartColumnRenderer ?? null!;
                renderer.HandleChartSizeChange(initialRect, remainingWidth, columnLeft, i != len - 1);
                remainingWidth -= renderer.ComputedWidth;
                columnLeft += renderer.ComputedWidth;
            }
        }
        #endregion
    }
}
