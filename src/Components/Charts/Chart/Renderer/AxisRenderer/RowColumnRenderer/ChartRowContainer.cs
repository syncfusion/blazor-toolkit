using Microsoft.AspNetCore.Components.Rendering;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Manages and coordinates row renderers for chart layout and axis assignment.
    /// </summary>
    public class ChartRowRendererContainer : ChartRendererContainer
    {
        #region Properties

        /// <summary>
        /// Gets or sets the default renderer type used when no row renderers are present.
        /// </summary>
        /// <value>
        /// A <see cref="List{T}"/> containing <see cref="Type"/> objects representing default renderers.
        /// Default value: <see cref="ChartRowRenderer"/>.
        /// </value>
        internal override List<Type> DefaultRendererType { get; set; } = [typeof(ChartRowRenderer)];
        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Performs initialization of the row container and registers it with the owner chart.
        /// </summary>
        protected override void OnInitialized()
        {
            AddToRenderQueue(this);
            if (Owner is not null)
            {
                Owner._rowContainer = this;
                SvgRenderer = Owner._svgRenderer;
            }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Computes the actual row index for an axis, clamping it to valid bounds.
        /// </summary>
        /// <param name="axis">The axis for which to determine the row index.</param>
        /// <returns>
        /// A zero-based row index within valid bounds. If the requested index exceeds the renderer count,
        /// the last valid index is returned. If negative, zero is returned.
        /// </returns>
        private int GetActualRow(ChartAxis axis)
        {
            int actualLength = Renderers.Count;
            int pos = Convert.ToInt32(axis.RowIndex);
            return pos >= actualLength ? actualLength - 1 : (pos < 0 ? 0 : pos);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Triggered when an element is added to the container, requesting a UI update.
        /// </summary>
        /// <param name="element">The added chart element.</param>
        protected override void OnElementAdded(IChartElement element)
        {
            StateHasChanged();
        }

        /// <summary>
        /// Removes renderer when element removed and requests UI update if chart still active.
        /// </summary>
        /// <param name="element">Removed element.</param>
        protected override void OnElementRemoved(IChartElement element)
        {
            if (!IsDisposed)
            {
                _ = Renderers.Remove((element as ChartRow)?.Renderer ?? null!);
                if (Owner is not null && !Owner.ChartDisposed())
                {
                    _ = InvokeAsync(StateHasChanged);
                }
            }
        }

        /// <summary>
        /// Handles the addition of a new renderer, optionally creating a default row element if none exists.
        /// </summary>
        /// <param name="renderer">The renderer being added. Must not be <c>null</c>.</param>
        /// <param name="element">The associated chart element; if <c>null</c>, a new <see cref="ChartRow"/> is created.</param>
        protected override void OnRendererAdded(IChartElementRenderer renderer, IChartElement element)
        {
            if (renderer is not null)
            {
                if (element is null)
                {
                    element = new ChartRow
                    {
                        RendererKey = "chartrow_default"
                    };
                    Elements.Add(element);
                }

                (renderer as ChartRowRenderer ?? null!).ChartRow = element as ChartRow ?? null!;
                if (Owner?.InitialRect is not null)
                {
                    _ = Owner.ProcessOnLayoutChangeAsync();
                }
            }
        }

        /// <summary>
        /// Builds the render tree, including default renderer components when necessary.
        /// </summary>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/> used to construct the render tree.</param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            int seq = 0;

            if (ContainerUpdate && Elements.Count > 0)
            {
                base.BuildRenderTree(builder);
            }
            else if (ContainerUpdate && Renderers.Count == 0)
            {
                foreach (Type defaultRenderer in DefaultRendererType)
                {
                    builder?.OpenComponent(seq++, defaultRenderer);
                    builder?.CloseComponent();
                }
            }
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Assigns vertical axes to their corresponding row renderers and sets axis spans.
        /// </summary>
        internal void AssignAxisToRow()
        {
            int actualIndex, span;
            ChartRowRenderer rowRenderer;
            Renderers.ForEach(rowRenderer => (rowRenderer as ChartRowRenderer ?? null!).Axes.Clear());

            if (Owner?._axisContainer is not null)
            {
                foreach (ChartAxisRenderer axisRenderer in Owner._axisContainer.Renderers.Cast<ChartAxisRenderer>())
                {
                    if (axisRenderer.Orientation == Orientation.Vertical)
                    {
                        ChartAxis axis = axisRenderer.Axis ?? null!;
                        actualIndex = GetActualRow(axis);

                        if (actualIndex >= 0 && actualIndex < Renderers.Count)
                        {
                            rowRenderer = Renderers[actualIndex] as ChartRowRenderer ?? null!;
                            axisRenderer.SetInverseAndOpposedPosition();
                            rowRenderer.Axes.Add(axis);
                            span = (actualIndex + axisRenderer.Axis?.Span) > Renderers.Count ? Renderers.Count : actualIndex + axis.Span;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Applies default renderer values by computing sizes from initial rect.
        /// </summary>
        internal override void SetDefaultRendererValues()
        {
            HandleChartSizeChange(Owner?.InitialRect ?? new Rect(0, 0, 0, 0));
        }

        /// <summary>
        /// Removes default placeholder row element if present and disposes it.
        /// </summary>
        internal void RemoveDefaultRowElement()
        {
            ChartRow defaultrow = Elements.Find(item => { return item.RendererKey == "chartrow_default"; }) as ChartRow ?? null!;
            if (defaultrow is not null)
            {
                defaultrow.Container = Owner;
                defaultrow.ComponentDispose();
            }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Handles sizing for each row renderer.
        /// </summary>
        /// <param name="rect">Available rectangle.</param>
        public override void HandleChartSizeChange(Rect rect)
        {
            if (rect is not null)
            {
                double remainingHeight = Math.Max(0, rect.Height);
                double rowTop = rect.Y + rect.Height;

                for (int i = 0, len = Renderers.Count; i < len; i++)
                {
                    ChartRowRenderer renderer = Renderers[i] as ChartRowRenderer ?? null!;
                    renderer.HandleChartSizeChange(rect, remainingHeight, rowTop, i != len - 1);
                    remainingHeight -= renderer.ComputedHeight;
                    rowTop -= renderer.ComputedHeight;
                }
            }
        }

        #endregion
    }
}
