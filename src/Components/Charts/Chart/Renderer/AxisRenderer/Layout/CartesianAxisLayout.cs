
namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Computes layout information for Cartesian axes and prepares axis rendering primitives.
    /// </summary>
    /// <remarks>
    /// This internal helper is responsible for measuring axes, computing clip rectangles,
    /// grid lines, tick lines, labels, borders and titles for both X and Y axes in a cartesian chart.
    /// The implementation closely follows the original Syncfusion axis layout responsibilities.
    /// </remarks>
    internal partial class CartesianAxisLayout : AxisLayout
    {
        #region Constants
        private const double AXIS_LABEL_SPACE = 3.0;
        #endregion

        #region Fields
        private Rect? _initialClipRect;
        private double _leftSize;
        private double _rightSize;
        private double _topSize;
        private double _bottomSize;
        private double _columnLeftSize;
        private double _columnRightSize;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the collection of axes tracked by this layout.
        /// </summary>
        /// <value>
        /// A list of <see cref="ChartAxis"/> instances representing all axes in the chart.
        /// </value>
        internal List<ChartAxis> Axes { get; set; } = [];

        /// <summary>
        /// Gets the end position of the previously rendered axis, used for label overlap detection.
        /// </summary>
        internal static double _previousAxisEnd;

        /// <summary>
        /// Gets the starting X coordinate of the previously rendered axis.
        /// </summary>
        internal static double _previousStartX;

        /// <summary>
        /// Gets a reference to the previously rendered axis for inter-axis label collision handling.
        /// </summary>
        internal static ChartAxis? _previousAxis;

        #endregion

        #region Private Methods

        /// <summary>
        /// Resolves the cross-axis relationships based on axis configuration.
        /// </summary>
        private void CrossAt()
        {
            foreach (ChartAxis axis in Axes)
            {
                if (axis.CrossesAt is null || axis.Renderer is null)
                {
                    continue;
                }

                if (string.IsNullOrEmpty(axis.CrossesInAxis))
                {
                    SetDefaultCrossAxis(axis);
                }
                else
                {
                    FindNamedCrossAxis(axis);
                }
            }
        }

        /// <summary>
        /// Sets the default cross-axis based on chart orientation.
        /// </summary>
        /// <param name="axis">The axis to assign a default cross axis for.</param>
        private void SetDefaultCrossAxis(ChartAxis axis)
        {
            if (axis.Renderer is null)
            {
                return;
            }

            bool invertedChart = Chart?._requireInvertedAxis ?? false;
            bool isHorizontal = axis.Renderer.Orientation == Orientation.Horizontal;

            axis.Renderer.CrossInAxis = invertedChart ? (isHorizontal ? Axes[0] : Axes[1]) : (isHorizontal ? Axes[1] : Axes[0]);
        }

        /// <summary>
        /// Finds and assigns a named cross-axis by name matching.
        /// </summary>
        /// <param name="axis">The axis referencing another axis by name.</param>
        private void FindNamedCrossAxis(ChartAxis axis)
        {
            for (int i = 2; i < Axes.Count; i++)
            {
                if (axis.CrossesInAxis == Axes[i].Name)
                {
                    if (axis.Renderer is { })
                    {
                        axis.Renderer.CrossInAxis = Axes[i];
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// Calculates fixed chart area dimensions based on configuration.
        /// </summary>
        /// <param name="chartAreaWidth">The fixed width of the chart area.</param>
        private void CalculateFixedChartArea(double chartAreaWidth)
        {
            if (SeriesClipRect is null)
            {
                return;
            }

            double padding = 10;
            double titleHeight = ((Chart?._chartTitleRenderer?.TitleSize.Height ?? 0) * (Chart?._chartTitleRenderer?.TitleCollection.Count ?? 0)) + padding;
            double subTitleHeight = ((Chart?._chartTitleRenderer?.SubTitleSize.Height ?? 0) * (Chart?._chartTitleRenderer?.SubTitleCollection.Count ?? 0)) + padding;

            SeriesClipRect.Width = chartAreaWidth;
            double legendWidth = Chart?._legendRenderer?.CurrentLegendPosition == LegendPosition.Right ? Chart._legendRenderer.LegendBounds.Width : 0;
            double titleWidth = Chart?._chartTitleRenderer?.TitleStyle?.Position == ChartTitlePosition.Right ? (titleHeight + subTitleHeight) : 0;

            SeriesClipRect.X = (Chart?.AvailableSize.Width ?? 0) - (double.IsNaN(Chart?._margin.Right ?? 0) ? 0 : (Chart?._margin.Right ?? 0)) - chartAreaWidth - legendWidth - titleWidth;

            if (Chart?._rowContainer is not null)
            {
                for (int i = 0; i < Chart._rowContainer.Renderers.Count; i++)
                {
                    SeriesClipRect.X -= ((ChartRowRenderer)Chart._rowContainer.Renderers[i]).FarSizes?.Sum() ?? 0;
                }
            }
        }

        /// <summary>
        /// Calculates and positions all axes within their containers.
        /// </summary>
        /// <param name="rect">The rectangular bounds for sizing.</param>
        private void CalculateAxisSize(Rect rect)
        {
            Chart?._rowContainer?.HandleChartSizeChange(rect);
            CalculateRowAxisPositions(rect);

            Chart?._columnContainer?.HandleChartSizeChange(rect);
            CalculateColumnAxisPositions(rect);
        }

        /// <summary>
        /// NaN-safe maximum: if current is NaN, take candidate.
        /// </summary>
        private static double MaxOr(double current, double candidate)
        {
            return double.IsNaN(current) ? candidate : Math.Max(current, candidate);
        }

        /// <summary>
        /// NaN-safe minimum: if current is NaN, take candidate.
        /// </summary>
        private static double MinOr(double current, double candidate)
        {
            return double.IsNaN(current) ? candidate : Math.Min(current, candidate);
        }

        /// <summary>
        /// Positions all axes within row containers.
        /// </summary>
        private void CalculateRowAxisPositions(Rect rect)
        {
            if (Chart?._rowContainer is null)
            {
                return;
            }

            for (int i = 0; i < Chart._rowContainer.Renderers.Count; i++)
            {
                ChartRowRenderer renderer = (ChartRowRenderer)Chart._rowContainer.Renderers[i];
                double nearCount = 0;
                double farCount = 0;

                for (int j = 0; j < renderer.Axes.Count; j++)
                {
                    ChartAxis axis = renderer.Axes[j];
                    PositionRowAxis(axis, renderer, rect, ref nearCount, ref farCount, i);
                }
            }
        }

        /// <summary>
        /// Positions a single axis within a row container.
        /// </summary>
        private void PositionRowAxis(ChartAxis axis, ChartRowRenderer renderer, Rect rect, ref double nearCount, ref double farCount, int rowIndex)
        {
            if (double.IsNaN(axis.Renderer?.Rect.Height ?? 0))
            {
                InitializeRowAxisRect(axis, renderer, rowIndex);
            }

            double x;
            if (axis.IsAxisOpposedPosition)
            {
                x = rect.X + rect.Width + (renderer.FarSizes?.GetRange(0, Convert.ToInt32(farCount)).Sum() ?? 0);
                if (axis.Renderer is { })
                {
                    axis.Renderer.Rect.X = MaxOr(axis.Renderer.Rect.X, x);
                }
                farCount++;
            }
            else
            {
                x = rect.X - (renderer.NearSizes?.GetRange(0, Convert.ToInt32(nearCount)).Sum() ?? 0);
                if (axis.Renderer is { })
                {
                    axis.Renderer.Rect.X = MinOr(axis.Renderer.Rect.X, x);
                }
                nearCount++;
            }
        }

        /// <summary>
        /// Initializes the rectangular bounds for a row axis.
        /// </summary>
        private void InitializeRowAxisRect(ChartAxis axis, ChartRowRenderer renderer, int rowIndex)
        {
            if (axis.Renderer is null)
            {
                return;
            }

            axis.Renderer.Rect.Height = renderer.ComputedHeight;
            double spanHeight = 0;

            for (int k = rowIndex + 1; k < rowIndex + axis.Span; k++)
            {
                if (Chart?._rowContainer?.Renderers.Count > k)
                {
                    spanHeight += ((ChartRowRenderer)Chart._rowContainer.Renderers[k]).ComputedHeight;
                }
            }

            double axisOffset = axis.PlotOffset;
            axis.Renderer.Rect.Y = renderer.ComputedTop - spanHeight + (axis.PlotOffsetTop > 0 ? axis.PlotOffsetTop : axisOffset);
            axis.Renderer.Rect.Height = axis.Renderer.Rect.Height + spanHeight - GetAxisOffsetValue(axis.PlotOffsetTop, axis.PlotOffsetBottom, axis.PlotOffset);
            axis.Renderer.Rect.Width = 0;
        }

        /// <summary>
        /// Positions all axes within column containers.
        /// </summary>
        private void CalculateColumnAxisPositions(Rect rect)
        {
            if (Chart?._columnContainer is null)
            {
                return;
            }

            for (int i = 0; i < (Chart._columnContainer?.Renderers.Count ?? 0); i++)
            {
                if (Chart._columnContainer?.Renderers[i] is not ChartColumnRenderer renderer)
                {
                    continue;
                }

                double nearCount = 0;
                double farCount = 0;

                for (int j = 0; j < renderer.Axes.Count; j++)
                {
                    ChartAxis axis = renderer.Axes[j];
                    PositionColumnAxis(axis, renderer, rect, ref nearCount, ref farCount, i);
                }
            }
        }

        /// <summary>
        /// Positions a single axis within a column container.
        /// </summary>
        private void PositionColumnAxis(ChartAxis axis, ChartColumnRenderer renderer, Rect rect, ref double nearCount, ref double farCount, int columnIndex)
        {
            if (axis.Renderer is not null && double.IsNaN(axis.Renderer.Rect.Width))
            {
                InitializeColumnAxisRect(axis, renderer, columnIndex);
            }

            double y;
            if (axis.IsAxisOpposedPosition)
            {
                y = rect.Y - (renderer.FarSizes?.GetRange(0, Convert.ToInt32(farCount)).Sum() ?? 0);
                if (axis.Renderer is { })
                {
                    axis.Renderer.Rect.Y = MinOr(axis.Renderer.Rect.Y, y);
                }
                farCount++;
            }
            else
            {
                y = rect.Y + rect.Height + (renderer.NearSizes?.GetRange(0, Convert.ToInt32(nearCount)).Sum() ?? 0);
                if (axis.Renderer is { })
                {
                    axis.Renderer.Rect.Y = MaxOr(axis.Renderer.Rect.Y, y);
                }
                nearCount++;
            }
        }

        /// <summary>
        /// Initializes the rectangular bounds for a column axis.
        /// </summary>
        private void InitializeColumnAxisRect(ChartAxis axis, ChartColumnRenderer renderer, int columnIndex)
        {
            if (axis.Renderer is null)
            {
                return;
            }

            axis.Renderer.Rect.Width = 0;

            double spanWidth = 0;
            for (int k = columnIndex; k < columnIndex + axis.Span; k++)
            {
                if (Chart?._columnContainer?.Renderers.Count > k)
                {
                    spanWidth += ((ChartColumnRenderer)Chart._columnContainer.Renderers[k]).ComputedWidth;
                }
            }

            double axisOffset = axis.PlotOffset;
            axis.Renderer.Rect.X = renderer.ComputedLeft + (axis.PlotOffsetLeft > 0 ? axis.PlotOffsetLeft : axisOffset);
            axis.Renderer.Rect.Width = axis.Renderer.Rect.Width + spanWidth - GetAxisOffsetValue(axis.PlotOffsetLeft, axis.PlotOffsetRight, axis.PlotOffset);
            axis.Renderer.Rect.Height = 0;
        }

        /// <summary>
        /// Measures all row axes and updates internal size tracking.
        /// </summary>
        private void MeasureRowAxis()
        {
            if (Chart?._rowContainer is not null)
            {
                foreach (ChartRowRenderer renderer in Chart._rowContainer.Renderers.Cast<ChartRowRenderer>())
                {
                    renderer.NearSizes = [];
                    renderer.FarSizes = [];
                    MeasureRowDefinition(renderer, new Size(Chart.AvailableSize.Width, renderer.ComputedHeight));

                    _leftSize = Math.Max(_leftSize, renderer.NearSizes.Sum());
                    _rightSize = Math.Max(_rightSize, renderer.FarSizes.Sum());
                }
            }
        }

        /// <summary>
        /// Measures all column axes and updates internal size tracking.
        /// </summary>
        private void MeasureColumnAxis()
        {
            if (Chart?._columnContainer is not null)
            {
                foreach (ChartColumnRenderer renderer in Chart._columnContainer.Renderers.Cast<ChartColumnRenderer>())
                {
                    renderer.FarSizes = [];
                    renderer.NearSizes = [];
                    renderer.ColumnLeftSizes = [];
                    renderer.ColumnRightSizes = [];
                    MeasureColumnDefinition(renderer, new Size(renderer.ComputedWidth, Chart.AvailableSize.Height));

                    _bottomSize = Math.Max(_bottomSize, renderer.NearSizes.Sum());
                    _topSize = Math.Max(_topSize, renderer.FarSizes.Sum());
                    _columnLeftSize = Math.Max(_columnLeftSize, renderer.ColumnLeftSizes.Sum());
                    _columnRightSize = Math.Max(_columnRightSize, renderer.ColumnRightSizes.Sum());
                }
            }
        }

        /// <summary>
        /// Measures row axis definitions and computes sizes.
        /// </summary>
        /// <param name="renderer">The <see cref="ChartRowRenderer"/> to measure.</param>
        /// <param name="size">The available <see cref="Size"/>.</param>
        private static void MeasureRowDefinition(ChartRowRenderer renderer, Size size)
        {
            foreach (ChartAxis axis in renderer.Axes)
            {
                axis.Renderer?.ComputeSize(size);
                renderer.ComputeSize(axis, axis.ScrollBarHeight);
            }

            // Reduce bottom sizes to avoid overlap
            if (renderer.FarSizes?.Count > 0)
            {
                renderer.FarSizes[^1] -= 10;
            }

            if (renderer.NearSizes?.Count > 0)
            {
                renderer.NearSizes[^1] -= 10;
            }
        }

        /// <summary>
        /// Measures column axis definitions and computes sizes.
        /// </summary>
        /// <param name="renderer">The <see cref="ChartColumnRenderer"/> to measure.</param>
        /// <param name="size">The available <see cref="Size"/>.</param>
        private static void MeasureColumnDefinition(ChartColumnRenderer renderer, Size size)
        {
            foreach (ChartAxis axis in renderer.Axes)
            {
                axis.Renderer?.ComputeSize(size);
                renderer.ComputeSize(axis, axis.ScrollBarHeight);
            }

            // Reduce edge sizes to prevent overlaps
            if (renderer.FarSizes?.Count > 0)
            {
                renderer.FarSizes[^1] -= 10;
            }

            if (renderer.NearSizes?.Count > 0)
            {
                renderer.NearSizes[^1] -= 10;
            }

            if (renderer.ColumnLeftSizes?.Count > 0)
            {
                renderer.ColumnLeftSizes[^1] -= 10;
            }

            if (renderer.ColumnRightSizes?.Count > 0)
            {
                renderer.ColumnRightSizes[^1] -= 10;
            }
        }

        /// <summary>
        /// Resets all size accumulator fields to zero.
        /// </summary>
        private void ResetSizeAccumulators()
        {
            _leftSize = _rightSize = _topSize = _bottomSize = _columnLeftSize = _columnRightSize = 0;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Adds an axis to the layout's tracking list.
        /// </summary>
        /// <param name="axis">The <see cref="ChartAxis"/> to add.</param>
        internal override void AddAxis(ChartAxis axis)
        {
            Axes.Add(axis);
        }

        /// <summary>
        /// Removes an axis from the layout's tracking list.
        /// </summary>
        /// <param name="axis">The <see cref="ChartAxis"/> to remove.</param>
        internal override void RemoveAxis(ChartAxis axis)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Computes the plot area bounds and positions all axes accordingly.
        /// </summary>
        /// <param name="rect">The rectangular bounds for the plot area.</param>
        internal override void ComputePlotAreaBounds(Rect rect)
        {
            double chartAreaWidth = !string.IsNullOrEmpty(Chart?._chartAreaRenderer?.Area?.Width) ? DataVizCommonHelper.StringToNumber(Chart._chartAreaRenderer.Area.Width, Chart.AvailableSize.Width) : double.NaN;

            CrossAt();
            SeriesClipRect = new Rect() { X = rect.X, Y = rect.Y, Height = rect.Height, Width = rect.Width };
            _initialClipRect = rect;
            ResetSizeAccumulators();

            MeasureRowAxis();
            _initialClipRect = ChartHelper.SubtractThickness(_initialClipRect, new Thickness(_leftSize, _rightSize, 0, 0));

            MeasureColumnAxis();
            _initialClipRect = ChartHelper.SubtractThickness(_initialClipRect, new Thickness(_columnLeftSize, _columnRightSize, _topSize, _bottomSize));

            CalculateAxisSize(_initialClipRect);
            ResetSizeAccumulators();

            MeasureRowAxis();
            SeriesClipRect = ChartHelper.SubtractThickness(SeriesClipRect, new Thickness(_leftSize, _rightSize, 0, 0));

            MeasureColumnAxis();
            SeriesClipRect = ChartHelper.SubtractThickness(SeriesClipRect, new Thickness(_columnLeftSize, _columnRightSize, _topSize, _bottomSize));

            if (!double.IsNaN(chartAreaWidth))
            {
                CalculateFixedChartArea(chartAreaWidth);
            }

            RefreshAxis();
            CalculateAxisSize(SeriesClipRect);
        }

        /// <summary>
        /// Refreshes all axes by resetting their rectangles to NaN.
        /// </summary>
        internal void RefreshAxis()
        {
            foreach (ChartAxis axis in Axes)
            {
                if (axis.Renderer is { })
                {
                    axis.Renderer.Rect = new Rect() { X = double.NaN, Y = double.NaN, Height = double.NaN, Width = double.NaN };
                }
            }
        }

        /// <summary>
        /// Determines whether an axis is positioned inside its cross-axis range.
        /// </summary>
        /// <param name="axis">The <see cref="ChartAxis"/> to evaluate.</param>
        /// <returns>
        /// <c>true</c> if the axis should be rendered inside the plot area; otherwise <c>false</c>.
        /// </returns>
        internal static bool FindAxisPosition(ChartAxis axis)
        {
            return axis.Renderer is not null && !double.IsNaN(axis.Renderer.CrossAt) && axis.Renderer.CrossInAxis?.Renderer is not null && axis.Renderer.IsInside(axis.Renderer.CrossInAxis.Renderer.VisibleRange);
        }

        /// <summary>
        /// Performs rendering calculations for a specific axis renderer.
        /// </summary>
        /// <param name="renderer">The <see cref="ChartAxisRenderer"/> to process.</param>
        /// <remarks>
        /// This method orchestrates grid line, label, border, and title calculations
        /// for both X and Y axes based on their orientation.
        /// </remarks>
        internal override void AxisRenderingCalculation(ChartAxisRenderer renderer)
        {
            ChartAxis axis = renderer.Axis ?? null!;
            renderer.UpdateCrossValue();
            renderer.IsAxisInside = FindAxisPosition(renderer.Axis ?? null!);
            renderer.IsTickInside = renderer.IsAxisInside || axis.TickPosition == AxisPosition.Inside;
            renderer.IsAxisLabelInside = renderer.IsAxisInside || renderer.LabelPosition == AxisPosition.Inside;
            Rect rect = axis.PlaceNextToAxisLine ? renderer.UpdatedRect : renderer.Rect;

            CalculateAxisLine(axis, renderer);

            if (renderer.Orientation == Orientation.Horizontal)
            {
                if (axis.Renderer?.MajorGridLinesWidth > 0 || renderer.MajorTickLinesWidth > 0)
                {
                    CalculateXAxisGridLine(axis, renderer.Index, renderer.UpdatedRect);
                }

                if (axis.Visible)
                {
                    CalculateXAxisLabels(axis, renderer.Index, rect);
                    CalculateXAxisBorder(axis, renderer.Index, rect);
                    if (!string.IsNullOrEmpty(axis.Title))
                    {
                        CalculateXAxisTitle(axis, renderer.Index, rect);
                    }
                }
                CalculateColumnBorder(axis, rect);
            }
            else
            {
                if (axis.Renderer?.MajorGridLinesWidth > 0 || renderer.MajorTickLinesWidth > 0)
                {
                    CalculateYAxisGridLine(axis, renderer.Index, renderer.UpdatedRect);
                }

                if (axis.Visible)
                {
                    CalculateYAxisLabels(axis, renderer.Index, rect);
                    CalculateYAxisBorder(axis, renderer.Index, rect);
                    if (!string.IsNullOrEmpty(axis.Title))
                    {
                        CalculateYAxisTitle(axis, renderer.Index, rect);
                    }
                }
                CalculateRowBorder(axis, rect);
            }
        }

        /// <summary>
        /// Clears all tracked axes from the layout.
        /// </summary>
        internal override void ClearAxes()
        {
        }

        #endregion
    }
}
