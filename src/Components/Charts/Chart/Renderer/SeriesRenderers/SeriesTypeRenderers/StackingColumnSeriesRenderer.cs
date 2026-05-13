using Microsoft.AspNetCore.Components.Rendering;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renders stacking column series for charts.
    /// </summary>
    /// <remarks>
    /// This renderer builds path options for stacking column series and invokes animation/rendering.
    /// </remarks>
    public class StackingColumnSeriesRenderer : ColumnBaseRenderer
    {
        #region Private Methods

        /// <summary>
        /// Ensures point collections for symbol locations and regions are initialized.
        /// </summary>
        /// <param name="point">The chart point to initialize.</param>
        private static void EnsurePointCollections(Point point)
        {
            point.SymbolLocations = [];
            point.Regions = [];
        }

        /// <summary>
        /// Updates column path option for a single point.
        /// </summary>
        /// <param name="point">The chart point.</param>
        /// <param name="rect">Rectangle for the column.</param>
        /// <param name="pointIdPrefix">Prefix used to build DOM id for the point.</param>
        /// <param name="visibility">Visibility value for the path element ("hidden"/"visible").</param>
        /// <param name="argsData">Rendering event arguments including styles and cancellation.</param>
        private void UpdateColumnPathOption(Point point, Rect rect, string pointIdPrefix, string visibility, PointRenderEventArgs argsData)
        {
            if (Series is null || Series.DashArray is null)
            {
                return;
            }
            UpdateSymbolLocation(point, rect);
            string id = pointIdPrefix + point.Index;
            string direction = CalculateRectangle(point, rect, id, argsData.CornerRadius);
            if (direction is not null)
            {
                PathOptions option = new(id, direction, Series.DashArray, argsData.Border.Width, argsData.Border.Color, Series.Opacity, argsData.Fill, string.Empty, string.Empty, AccessText ?? string.Empty, "", "", GetDataPoints(point.XValue, point.YValue))
                {
                    Visibility = visibility
                };
                ColumnPathOptions.Add(option);
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Renders series and triggers calculation and animation.
        /// </summary>
        protected override void RenderSeries()
        {
            base.RenderSeries();
            ColumnPathOptions = [];
            if (Series is not null && !Series.Visible && !Series._isLegendClicked)
            {
                return;
            }
            CalculateColumnPathOption();
            Animate();
        }

        /// <summary>
        /// Builds the render tree for the series paths.
        /// </summary>
        /// <param name="builder">The render tree builder.</param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            if (ClipRect is null || builder is null || (Series is not null && !Series.Visible && !Series._isLegendClicked))
            {
                return;
            }

            CreateSeriesElements(builder);
            foreach (PathOptions option in ColumnPathOptions)
            {
                _ = SvgRenderer?.RenderPath(builder, option);
            }

            builder.CloseElement();
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Clears and recalculates column path options and updates renderer direction.
        /// </summary>
        internal override void UpdateDirection()
        {
            ColumnPathOptions.Clear();
            CalculateColumnPathOption();
            base.UpdateDirection();
        }

        /// <summary>
        /// Calculates path options for all visible stacking column points.
        /// </summary>
        internal void CalculateColumnPathOption()
        {
            DoubleRange sideBySideInfo = GetSideBySideInfo();
            ChartData = new System.Text.StringBuilder();
            List<Point> visiblePoints = ChartHelper.GetVisiblePoints(Points ?? null!);
            string pointId = Series?.Container?.ID + "_Series_" + Index + "_Point_";
            string visibility = ShouldAnimate() ? "hidden" : "visible";

            foreach (Point pointColumn in visiblePoints)
            {
                EnsurePointCollections(pointColumn);

                Point previousPoint = pointColumn.Index - 1 > -1 ? Points?[pointColumn.Index - 1] ?? null! : null!;
                Point nextPoint = pointColumn.Index + 1 < Points?.Count ? Points[pointColumn.Index + 1] : null!;

                if (pointColumn.Visible && ChartHelper.WithInRange(previousPoint, pointColumn, nextPoint ?? null!, Series?.Renderer.XAxisRenderer ?? null!) && Series?.Renderer.StackedValues is not null)
                {
                    double startValue = GetStackingStartValue(pointColumn.Index, GetVisibleSeriesIndex());
                    Rect rect = GetRectangle(pointColumn.XValue + sideBySideInfo.Start, (!Series.Visible && Series._isLegendClicked) ? startValue : Series.Renderer.StackedValues.EndValues[pointColumn.Index], pointColumn.XValue + sideBySideInfo.End, (!Series.Visible && Series._isLegendClicked) ? startValue : Series.Renderer.StackedValues.StartValues[pointColumn.Index]);
                    rect = GetColumnWidthInPixelRect(rect);
                    PointRenderEventArgs argsData = TriggerEvent(pointColumn, Interior ?? null!, new BorderModel() { Width = Series.Visible ? Series.Border.Width : 0, Color = Series.Border.Color });

                    if (!argsData.Cancel)
                    {
                        UpdateColumnPathOption(pointColumn, rect, pointId, visibility, argsData);
                    }
                }
                if (IsTooltipEnabled() && pointColumn.SymbolLocations.Count > 0)
                {
                    AppendChartData(ChartPoints?[pointColumn.Index]);
                }
            }
        }

        /// <summary>
        /// Determines whether the stacking column is in outer position for positive stacks.
        /// </summary>
        /// <param name="isMinus">Indicates negative stacking.</param>
        /// <param name="series">Series to inspect.</param>
        /// <returns>True when not negative and label position is Outer.</returns>
        internal override bool IsStackingColumnAndOuterPosition(bool isMinus, ChartSeries series)
        {
            return !isMinus && Series?.Marker.DataLabel.Position == ChartLabelPosition.Outer;
        }

        #endregion
    }

    /// <summary>
    /// Renderer for 100% stacking column series.
    /// </summary>
    public class StackingColumn100SeriesRenderer : StackingColumnSeriesRenderer
    {
    }
}