using Microsoft.AspNetCore.Components.Rendering;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Defines the contract for chart elements that require associated series data.
    /// </summary>
    public interface IRequireSeries
    {
        /// <summary>
        /// Gets or sets the chart series renderer associated with this element.
        /// </summary>
        ChartSeriesRenderer Series { get; set; }

        /// <summary>
        /// Invoked when the associated series data has changed.
        /// </summary>
        void OnSeriesChanged();
    }

    /// <summary>
    /// <see cref="ChartAxisRendererContainer"/> manages axis renderers, axis elements and coordinates layout
    /// and rendering passes for chart axes.
    /// </summary>
    public class ChartAxisRendererContainer : ChartRendererContainer
    {
        #region Fields
        private int _axisIndex;
        private Rect? _availableRect;
        private AxisLayout _axisLayout = null!;
        private bool _needsLayout = true;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the collection of all axes, keyed by axis name.
        /// </summary>
        internal Dictionary<string, ChartAxis> Axes { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of horizontal axes.
        /// </summary>
        internal Dictionary<string, ChartAxis> _horizontalAxes = [];

        /// <summary>
        /// Gets or sets the collection of vertical axes.
        /// </summary>
        internal Dictionary<string, ChartAxis> _verticalAxes = [];

        /// <summary>
        /// Gets or sets a value indicating whether any axis has enabled scrollbar settings.
        /// </summary>
        internal bool _isScrollSettingEnabled;

        /// <summary>
        /// Gets or sets the current axis layout implementation.
        /// </summary>
        /// <value>The <see cref="AxisLayout"/> used to compute axis layout and bounds. Updates trigger layout recalculation.</value>
        internal AxisLayout AxisLayout
        {
            get => _axisLayout;

            set
            {
                if (_axisLayout != value)
                {
                    _needsLayout = true;
                    _axisLayout?.ClearAxes();

                    _axisLayout = value;

                    if (_axisLayout is not null)
                    {
                        foreach (ChartAxis axis in Axes.Values)
                        {
                            _axisLayout.AddAxis(axis);
                        }
                    }
                    if (_axisLayout is { })
                    {
                        _axisLayout.Chart = Owner;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the collection of elements requiring series data.
        /// </summary>
        internal List<IRequireSeries> _elementsRequiredSeries = [];
        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Initializes the container and registers it with the owner chart.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            AddToRenderQueue(this);
            if (Owner is not null)
            {
                Owner._axisContainer = this;
                SvgRenderer = Owner._svgRenderer;
            }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Marks axes with enabled scrollbar settings and clears cached label template sizes if needed.
        /// </summary>
        private void MarkScrollAndClearLabelSizes()
        {
            foreach (ChartAxisRenderer axisRender in Renderers.Cast<ChartAxisRenderer>())
            {
                if (axisRender.Axis is not null && axisRender.Axis.ScrollbarSettings.Enable)
                {
                    _isScrollSettingEnabled = true;
                }
                if (axisRender.Axis is not null)
                {
                    axisRender.Axis.AxisIntervalType = string.IsNullOrEmpty(axisRender.Axis.AxisIntervalType) ? axisRender.Axis.IntervalType.ToString() : axisRender.Axis.AxisIntervalType;
                    axisRender.Axis.AxisValueType = string.IsNullOrEmpty(axisRender.Axis.AxisValueType) ? axisRender.Axis.ValueType.ToString() : axisRender.Axis.AxisValueType;
                }
                if (Owner is not null && !Owner._isAxisTemplateCalled)
                {
                    axisRender.AxisRenderInfo.AxisLabelTemplateSizeList.Clear();
                }
            }
        }

        /// <summary>
        /// Computes plot area bounds and prepares axes for rendering.
        /// </summary>
        /// <param name="newRect">New available rectangle.</param>
        private void ComputePlotAreaBounds(Rect newRect)
        {
            CheckAreaType();
            PrepareAxesVisibilityAndAdaptiveValues();

            if (Owner?._legendRenderer is not null)
            {
                Owner._legendRenderer.HasLegendClicked = false;
            }

            AxisLayout.ComputePlotAreaBounds(newRect);

            CartesianAxisLayout._previousAxisEnd = 0;
            CartesianAxisLayout._previousStartX = 0;
            CartesianAxisLayout._previousAxis = null;

            PerformAxisRenderingCalculations();

            CartesianAxisLayout._previousAxisEnd = 0;
            CartesianAxisLayout._previousAxis = null;
        }

        /// <summary>
        /// Prepares axis visibility based on associated series and sets adaptive axis values.
        /// </summary>
        private void PrepareAxesVisibilityAndAdaptiveValues()
        {
            foreach (ChartAxisRenderer axisRenderer in Renderers.Cast<ChartAxisRenderer>())
            {
                foreach (ChartSeriesRenderer series in axisRenderer.SeriesRenderer)
                {
                    axisRenderer.Axis?.SetAxisVisibility(GetAxisVisible(series, axisRenderer.Axis));
                    if (axisRenderer.Axis is not null && axisRenderer.Axis.Visible)
                    {
                        break;
                    }
                }
                axisRenderer.SetAdaptiveAxisValues(axisRenderer.Axis ?? null!, axisRenderer.Orientation);
            }
        }

        /// <summary>
        /// Performs per-axis rendering calculations.
        /// </summary>
        private void PerformAxisRenderingCalculations()
        {
            foreach (ChartAxisRenderer renderer in Renderers.Cast<ChartAxisRenderer>())
            {
                renderer.ClearAxisInfo();
                if (renderer.Orientation != Orientation.Null)
                {
                    AxisLayout.AxisRenderingCalculation(renderer);
                }
                if (Owner is not null && !Owner._isAxisTemplateCalled && renderer.Axis?.LabelTemplate is not null)
                {
                    Owner._hasLabelTemplate = true;
                }
            }
        }

        /// <summary>
        /// Determines if an axis should be visible based on series visibility and axis settings.
        /// </summary>
        /// <param name="seriesRenderer">The series renderer to check.</param>
        /// <param name="axis">The axis to evaluate.</param>
        /// <returns><see langword="true"/> if the axis should be visible; otherwise <see langword="false"/>.</returns>
        private static bool GetAxisVisible(ChartSeriesRenderer seriesRenderer, ChartAxis axis)
        {
            bool axisVisible = axis.InternalVisiblity;
            bool seriesVisible = seriesRenderer.Series is not null && seriesRenderer.Series.Visible;
            return !axis.Name.Contains("Primary", StringComparison.OrdinalIgnoreCase) ? axisVisible && seriesVisible : axisVisible;
        }

        /// <summary>
        /// Adds default PrimaryXAxis and PrimaryYAxis renderers if not already present.
        /// </summary>
        /// <param name="builder">The render tree builder.</param>
        /// <param name="seq">Starting sequence number for render tree commands.</param>
        private void AddDefaultRenderer(RenderTreeBuilder builder, int seq)
        {
            if (Elements.Count == 0)
            {
                builder.OpenComponent(seq++, typeof(PrimaryXAxisRenderer));
                builder.AddAttribute(seq++, "AxisName", "PrimaryXAxis");
                builder.CloseComponent();

                builder.OpenComponent(seq++, typeof(PrimaryYAxisRenderer));
                builder.AddAttribute(seq++, "AxisName", "PrimaryYAxis");
                builder.CloseComponent();
            }
            else
            {
                if (!Elements.Any(element => (element as ChartAxis ?? null!).GetName() == "PrimaryXAxis"))
                {
                    builder.OpenComponent(seq++, typeof(PrimaryXAxisRenderer));
                    builder.AddAttribute(seq++, "AxisName", "PrimaryXAxis");
                    builder.CloseComponent();
                }

                if (!Elements.Any(element => (element as ChartAxis ?? null!).GetName() == "PrimaryYAxis"))
                {
                    builder.OpenComponent(seq++, typeof(PrimaryYAxisRenderer));
                    builder.AddAttribute(seq++, "AxisName", "PrimaryYAxis");
                    builder.CloseComponent();
                }
            }
        }

        /// <summary>
        /// Determines the area type and sets the appropriate axis layout.
        /// </summary>
        private void CheckAreaType()
        {
            AxisLayout = AxisLayout is CartesianAxisLayout ? AxisLayout : new CartesianAxisLayout();
        }

        /// <summary>
        /// Assigns the X-axis renderer to a series.
        /// </summary>
        /// <param name="series">The series requiring axis assignment.</param>
        private void AssignXAxis(IRequireAxis series)
        {
            string axisName = series.XAxisName;
            if (Axes.TryGetValue(axisName, out ChartAxis? xAxis) && xAxis.Renderer is not null)
            {
                series.XAxisRenderer = xAxis.Renderer;
                xAxis.Renderer.Orientation = (Owner is not null && !Owner._requireInvertedAxis) ? Orientation.Horizontal : Orientation.Vertical;
                xAxis.Renderer.SeriesRenderer.Add(series as ChartSeriesRenderer ?? null!);
            }
        }

        /// <summary>
        /// Assigns the Y-axis renderer to a series.
        /// </summary>
        /// <param name="series">The series requiring axis assignment.</param>
        private void AssignYAxis(IRequireAxis series)
        {
            string axisName = series.YAxisName;
            if (Axes.TryGetValue(axisName, out ChartAxis? yAxis) && yAxis.Renderer is not null)
            {
                series.YAxisRenderer = yAxis.Renderer;
                yAxis.Renderer.Orientation = (Owner is not null && !Owner._requireInvertedAxis) ? Orientation.Vertical : Orientation.Horizontal;
                yAxis.Renderer.SeriesRenderer.Add(series as ChartSeriesRenderer ?? null!);
            }
        }

        /// <summary>
        /// Clears series renderer collections from axes.
        /// </summary>
        /// <param name="seriesList">Collection of series to filter on.</param>
        /// <param name="refreshSeries">If <see langword="true"/>, clears all axes; otherwise only specific axes.</param>
        private void ClearSeriesRenderer(IEnumerable<IRequireAxis> seriesList, bool refreshSeries)
        {
            if (!refreshSeries)
            {
                foreach (KeyValuePair<string, ChartAxis> axis in Axes)
                {
                    if (axis.Value.Renderer is not null)
                    {
                        axis.Value.Renderer.SeriesRenderer.Clear();
                        axis.Value.Renderer.Orientation = Orientation.Null;
                        axis.Value.Renderer.IsStack100 = false;
                    }
                }
            }
            else
            {
                foreach (IRequireAxis series in seriesList)
                {
                    ChartAxisRenderer axisRenderer = Axes[series.XAxisName].Renderer ?? null!;
                    axisRenderer.SeriesRenderer.Clear();
                    axisRenderer.Orientation = Orientation.Null;
                    axisRenderer.IsStack100 = false;

                    axisRenderer = Axes[series.YAxisName].Renderer ?? null!;
                    axisRenderer.SeriesRenderer.Clear();
                    axisRenderer.Orientation = Orientation.Null;
                    axisRenderer.IsStack100 = false;
                }
            }
        }

        /// <summary>
        /// Creates and initializes a secondary axis with default settings.
        /// </summary>
        /// <returns>A new <see cref="ChartAxis"/> configured for secondary use.</returns>
        private static ChartAxis InitAxis()
        {
            ChartAxis newAxis = new()
            {
                Name = "SecondaryAxis",
                MajorGridLines = new ChartAxisMajorGridLines { Width = 0 },
                MajorTickLines = new ChartAxisMajorTickLines { Width = 0 },
                LineStyle = new ChartAxisLineStyle { Width = 0 },
                Minimum = 0,
                Maximum = 100,
                RowIndex = 0,
                OpposedPosition = true,
                RendererType = typeof(NumericAxisRenderer),
                LabelFormat = "{value}%"
            };

            return newAxis;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Determines whether the component should render.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if rendering is required; otherwise <see langword="false"/>.
        /// </returns>
        protected override bool ShouldRender()
        {
            return RendererShouldRender || ContainerUpdate;
        }

        /// <summary>
        /// Handles new axis element addition.
        /// </summary>
        /// <param name="element">The element added to the container.</param>
        protected override void OnElementAdded(IChartElement element)
        {
            if (element is not null)
            {
                ChartAxis axis = element as ChartAxis ?? null!;
                if (Axes.TryAdd(axis.GetName(), axis))
                {
                    AxisLayout?.AddAxis(axis);
                }

                if (Owner?.InitialRect is not null)
                {
                    StateHasChanged();
                }
            }
        }

        /// <summary>
        /// Called when a renderer is registered; wires axis/series relationships.
        /// </summary>
        /// <param name="renderer">The renderer added.</param>
        /// <param name="element">Associated chart element.</param>
        protected override void OnRendererAdded(IChartElementRenderer renderer, IChartElement element)
        {
            if (renderer is IRequireSeries)
            {
                _elementsRequiredSeries.Add(renderer as IRequireSeries ?? null!);
            }

            ChartAxisRenderer axisRenderer = renderer as ChartAxisRenderer ?? null!;

            if (axisRenderer is not null)
            {
                axisRenderer.Index = _axisIndex++;
                axisRenderer.Axis = element as ChartAxis;

                if (axisRenderer.Axis is not null)
                {
                    axisRenderer.Axis.Renderer = axisRenderer;
                    if (axisRenderer.Axis.Container is null)
                    {
                        axisRenderer.Axis.Container = Owner ?? null!;
                    }
                }

                if (Owner?.InitialRect is not null)
                {
                    _needsLayout = true;
                    _ = Owner.ProcessOnLayoutChangeAsync();
                }
            }
        }

        /// <summary>
        /// Called when a renderer is removed from the container.
        /// </summary>
        /// <param name="renderer">The renderer removed.</param>
        protected override void OnRendererRemoved(IChartElementRenderer renderer)
        {
            _axisIndex = Renderers.IndexOf(renderer);
            if (!IsDisposed)
            {
                _ = Owner?.ProcessOnLayoutChangeAsync();
            }
        }

        /// <summary>
        /// Called when an axis element is removed.
        /// </summary>
        /// <param name="element">Removed element.</param>
        protected override void OnElementRemoved(IChartElement element)
        {
            if (element is not null)
            {
                ChartAxis axis = element as ChartAxis ?? null!;
                RemoveRenderer(axis.Renderer ?? null!);
                Owner?._axisOutSideContainer?.RemoveRenderer(axis.Renderer?.OutSideRenderer ?? null!);

                if (Axes.ContainsValue(axis))
                {
                    _ = Axes.Remove(axis.GetName());
                }

                if (Owner is not null && !Owner.ChartDisposed())
                {
                    _ = InvokeAsync(StateHasChanged);
                }
            }
        }

        /// <summary>
        /// Renders child renderer components inside the render tree.
        /// </summary>
        /// <param name="builder">RenderTreeBuilder instance used to build content.</param>
        protected override void BuildRenderers(RenderTreeBuilder builder)
        {
            if (builder is null)
            {
                return;
            }

            int seq = 0;
            if (ContainerUpdate)
            {
                AddDefaultRenderer(builder, seq);
                foreach (IChartElement element in Elements)
                {
                    if (element.RendererType is null)
                    {
                        continue;
                    }

                    builder.OpenComponent(seq++, element.RendererType);
                    builder.SetKey(element.RendererKey);
                    builder.AddAttribute(seq++, "AxisName", (element as ChartAxis ?? null!).Name);
                    builder.CloseComponent();
                }

                RendererShouldRender = false;
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Forces update of axis rendering information and triggers per-renderer render queues.
        /// </summary>
        internal void UpdateAxisRendering()
        {
            foreach (ChartAxisRenderer renderer in Renderers.Cast<ChartAxisRenderer>())
            {
                renderer.ClearAxisInfo();
                if (renderer.Orientation != Orientation.Null)
                {
                    renderer.RendererShouldRender = true;
                    if (renderer.Axis is not null)
                    {
                        renderer.SetAdaptiveAxisValues(renderer.Axis, renderer.Orientation);
                    }
                    AxisLayout.AxisRenderingCalculation(renderer);
                    renderer.ProcessRenderQueue();
                }
            }
        }

        /// <summary>
        /// Invoked when theme changes to update each axis renderer.
        /// </summary>
        internal void OnThemeChanged()
        {
            foreach (ChartAxisRenderer renderer in Renderers.Cast<ChartAxisRenderer>())
            {
                renderer.OnThemeChange();
                renderer.ProcessRenderQueue();
            }
        }

        #endregion

        #region Public Methods        

        /// <summary>
        /// Adds renderer and associated default axis elements when needed.
        /// </summary>
        /// <param name="renderer">Renderer to add.</param>
        public override void AddRenderer(IChartElementRenderer renderer)
        {
            if (renderer is not null && !Renderers.Contains(renderer))
            {
                ContainerPrerender = false;
                RendererShouldRender = true;
                Renderers.Add(renderer);

                if (Renderers.Count <= 2)
                {
                    Renderers.Sort((renderer1, renderer2) => (renderer1 as ChartAxisRenderer ?? null!).Index.CompareTo((renderer2 as ChartAxisRenderer ?? null!).Index));
                }

                if (renderer.GetType().Equals(typeof(ParetoAxisRenderer)))
                {
                    ChartAxis axis = InitAxis();
                    OnElementAdded(axis);
                    OnRendererAdded(renderer, axis);
                }
                else if (renderer.GetType().Equals(typeof(PrimaryXAxisRenderer)))
                {
                    ChartAxis axis = new ChartPrimaryXAxis();
                    OnElementAdded(axis);
                    OnRendererAdded(renderer, axis);
                }
                else if (renderer.GetType().Equals(typeof(PrimaryYAxisRenderer)))
                {
                    ChartAxis axis = new ChartPrimaryYAxis();
                    OnElementAdded(axis);
                    OnRendererAdded(renderer, axis);
                }
                else
                {
                    OnRendererAdded(renderer, Elements.Find(axis => (axis as ChartAxis ?? null!).GetName() == (renderer as ChartAxisRenderer ?? null!).AxisName) ?? null!);
                }
            }
        }

        /// <summary>
        /// Handles chart size changes and triggers layout if necessary.
        /// </summary>
        /// <param name="rect">Available rectangle for layout.</param>
        public override void HandleChartSizeChange(Rect rect)
        {
            if (_availableRect != rect || _needsLayout)
            {
                IsRendererUpdate = Renderers.Count > 0;
                MarkScrollAndClearLabelSizes();
                ComputePlotAreaBounds(rect);
                _availableRect = rect;
                RendererShouldRender = true;
            }
        }

        /// <summary>
        /// Processes render queue for all axis renderers using last-known available rect.
        /// </summary>
        public override void ProcessRenderQueue()
        {
            foreach (ChartAxisRenderer renderer in Renderers.Cast<ChartAxisRenderer>())
            {
                renderer.HandleChartSizeChange(_availableRect ?? new Rect(0, 0, 0, 0));
                renderer.ProcessRenderQueue();
            }
        }

        /// <summary>
        /// Assigns axis renderers to series renderers based on axis names.
        /// </summary>
        /// <param name="seriesList">Collection of series requiring axis assignment.</param>
        /// <param name="refreshSeries">If true, clears and repopulates per-series renderer lists.</param>
        public void AssignAxisToSeries(IEnumerable<IRequireAxis> seriesList, bool refreshSeries = false)
        {
            if (seriesList is null)
            {
                return;
            }

            ClearSeriesRenderer(seriesList, refreshSeries);
            foreach (IRequireAxis series in seriesList)
            {
                AssignXAxis(series);
                AssignYAxis(series);
            }
        }
        #endregion
    }
}