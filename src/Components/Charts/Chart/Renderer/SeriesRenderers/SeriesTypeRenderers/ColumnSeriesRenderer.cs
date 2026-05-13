using Microsoft.AspNetCore.Components.Rendering;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renders column series on the chart area.
    /// </summary>
    /// <remarks>
    /// This renderer computes column geometry, prepares path options and renders SVG paths.
    /// It preserves animation visibility behaviour and triggers point render events.
    /// </remarks>
    public class ColumnSeriesRenderer : ColumnBaseRenderer
    {
        #region Private Methods

        /// <summary>
        /// Calculates path options for each visible point in the column series.
        /// </summary>
        private void CalculateColumnPathOption()
        {
            DoubleRange sideBySideInfo = GetSideBySideInfo();
            string pointId = Series?.Container?.ID + "_Series_" + Index + "_Point_", id;
            double origin = Math.Max(YAxisRenderer?.VisibleRange.Start ?? 0, 0);
           
            string visibility = ShouldAnimate() ? "hidden" : "visible";
            int pointsCount = Points?.Count ?? 0;

            ChartData = new System.Text.StringBuilder();

            for (int i = 0; i < pointsCount; i++)
            {
                Point point = Points?[i] ?? null!;
                PreparePointContainers(point);

                Point previousPoint = i - 1 > -1 ? Points?[i - 1] ?? null! : null!;
                Point nextPoint = i + 1 < Points?.Count ? Points[i + 1] : null!;

                if (point.Visible && ChartHelper.WithInRange(previousPoint, point, nextPoint, XAxisRenderer))
                {
                    Rect rect = GetRectangle(point.XValue + sideBySideInfo.Start, point.YValue, point.XValue + sideBySideInfo.End, origin);
                    if (rect is not null && rect.Width < 1)
                    {
                        rect.Width = 1;
                    }
                    rect = GetColumnWidthInPixelRect(rect ?? null!);
                    PointRenderEventArgs argsData = CreatePointRenderArgs(point);

                    if (!argsData.Cancel)
                    {
                        ProcessRenderablePoint(point, rect, pointId, argsData, visibility);
                    }
                }
                //Series points are needed on the script side for keyboard navigation if markers and tooltips are not enabled.
                if (IsTooltipEnabled() && point.SymbolLocations.Count > 0)
                {
                    AppendChartData(ChartPoints?[point.Index]);
                }
            }
        }

        /// <summary>
        /// Prepares the symbol locations and regions containers for a point.
        /// </summary>
        /// <param name="point">The chart point to prepare containers for.</param>
        private void PreparePointContainers(Point point)
        {
            point.SymbolLocations = [];
            if (ChartPoints is not null)
            {
                ChartPoints[point.Index].SymbolLocations = [];
                ChartPoints[point.Index].Regions = [];
            }
            point.Regions = [];
        }

        /// <summary>
        /// Processes a renderable point by updating its location, calculating geometry, and creating path options.
        /// </summary>
        /// <param name="point">The chart point to process.</param>
        /// <param name="rect">The calculated rectangle bounds for the point.</param>
        /// <param name="pointId">The base identifier for the point element.</param>
        /// <param name="argsData">The point render event arguments containing styling information.</param>
        /// <param name="visibility">The visibility state of the point ("visible" or "hidden").</param>
        private void ProcessRenderablePoint(Point point, Rect rect, string pointId, PointRenderEventArgs argsData, string visibility)
        {
            UpdateSymbolLocation(point, rect);
            string id = pointId + point.Index;
            string direction = CalculateRectangle(point, rect, id, argsData.CornerRadius);
            if (direction is not null)
            {
                PathOptions option = new(id, direction, Series?.DashArray ?? string.Empty, argsData.Border.Width, argsData.Border.Color, Series?.Opacity ?? 1, argsData.Fill, string.Empty, string.Empty, AccessText ?? string.Empty, "", "", Owner?._seriesContainer is not null && Owner._seriesContainer._hasLargeData ? "" : GetDataPoints(point.XValue, point.YValue))
                {
                    Visibility = visibility
                };
                ColumnPathOptions.Add(option);
            }
        }

        /// <summary>
        /// Creates point render event arguments with styling information for the specified point.
        /// </summary>
        /// <param name="point">The chart point for which to create render event arguments.</param>
        /// <returns>
        /// A <see cref="PointRenderEventArgs"/> object containing the point's fill color, 
        /// border settings, and other render options.
        /// </returns>
        private PointRenderEventArgs CreatePointRenderArgs(Point point)
        {
            return TriggerEvent
            (
                point,
                Interior ?? null!,
                new BorderModel()
                {
                    Width = Series?.Border.Width ?? 0,
                    Color = (YAxisRenderer?.VisibleRange.Start >= point.YValue) ? string.Empty : Series?.Border.Color ?? string.Empty
                }
            );
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Renders the column series by calculating path options and initiating animation.
        /// </summary>
        protected override void RenderSeries()
        {
            base.RenderSeries();
            ColumnPathOptions = [];
            if (Series is not null && Series.Visible)
            {
                CalculateColumnPathOption();
            }

            Animate();
        }

        /// <summary>
        /// Builds the component render tree by creating series elements and rendering SVG paths.
        /// </summary>
        /// <param name="builder">The render tree builder provided by Blazor.</param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            if (ClipRect is null || builder is null || (Series is not null && !Series.Visible))
            {
                return;
            }

            CreateSeriesElements(builder);
            foreach (PathOptions option in ColumnPathOptions.ToArray())
            {
                SvgRenderer?.RenderPath(builder, option, "e-pointer-series", option.DataPoint);
            }

            builder.CloseElement();
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Updates rendering direction and recalculates path options when axis direction changes.
        /// </summary>
        internal override void UpdateDirection()
        {
            ColumnPathOptions.Clear();
            CalculateColumnPathOption();
            base.UpdateDirection();
        }

        #endregion
    }
}