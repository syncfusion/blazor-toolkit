
namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Provides hit-testing helpers for chart interaction (tooltip/selection/drag detection).
    /// </summary>
    /// <remarks>
    /// This class searches visible series and points to identify the item under the current mouse position.
    /// It considers series type, clipping region, marker presence, and editing states.
    /// </remarks>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ChartData"/> class.
    /// </remarks>
    /// <param name="sfchart">The chart instance.</param>
    public class ChartData(SfChart sfchart)
    {
        #region Constants

        // Small padding added to marker hit area to improve usability.
        private const double MarkerHitPadding = 5d;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the chart instance.
        /// </summary>
        protected SfChart? Chart { get; set; } = sfchart;

        /// <summary>
        /// Maintains the last matched region index (used for contextual operations).
        /// </summary>
        protected int LierIndex { get; set; }

        /// <summary>
        /// Holds the current hit points processed in the last interaction.
        /// </summary>
        protected List<PointData> CurrentPoints { get; set; } = [];

        /// <summary>
        /// Indicates whether the mouse is inside a particular region (used by editing/dragging).
        /// </summary>
        internal bool InsideRegion { get; set; }

        /// <summary>
        /// Holds previously hit points processed in the prior interaction.
        /// </summary>
        internal List<PointData> PreviousPoints { get; set; } = [];

        #endregion

        #region Private Methods

        /// <summary>
        /// Retrieves the point under the given mouse coordinates for the provided series renderer.
        /// </summary>
        /// <param name="seriesRenderer">Series renderer.</param>
        /// <param name="rect">Series clip rect.</param>
        /// <param name="x">Mouse X.</param>
        /// <param name="y">Mouse Y.</param>
        /// <returns>The matched <see cref="Point"/>; otherwise, <c>null</c>.</returns>
        private Point GetRectPoint(ChartSeriesRenderer seriesRenderer, Rect rect, double x, double y)
        {
            ChartSeries series = seriesRenderer.Series ?? null!;

            if (seriesRenderer.Points is not null)
            {
                foreach (Point point in seriesRenderer.Points.ToArray())
                {
                    if (point.RegionData is null && point.Regions.Count == 0)
                    {
                        continue;
                    }

                    if (IsPolarColumnHit(point, seriesRenderer, x, y))
                    {
                        return point;
                    }

                    if (series?.ChartDataEditSettings?.Enable == true &&
                        seriesRenderer.IsRectSeries() &&
                        RectRegion(x, y, point, rect, series))
                    {
                        InsideRegion = true;
                        return point;
                    }

                    if (CheckRegionContainsPoint(point.Regions, rect, x, y))
                    {
                        return point;
                    }
                }
            }

            return null!;
        }

        /// <summary>
        /// Determines whether the polar/column region contains the given location.
        /// </summary>
        /// <param name="point">Candidate point.</param>
        /// <param name="seriesRenderer">Series renderer.</param>
        /// <param name="x">Mouse X.</param>
        /// <param name="y">Mouse Y.</param>
        /// <returns><c>true</c> if hit; otherwise <c>false</c>.</returns>
        private bool IsPolarColumnHit(Point point, ChartSeriesRenderer seriesRenderer, double x, double y)
        {
            _ = seriesRenderer.Series ?? null!;
            if (point.RegionData is null)
            {
                return false;
            }

            double fromCenterX = x - (((seriesRenderer.ClipRect?.Width ?? 0) / 2) + (seriesRenderer.ClipRect?.X ?? 0));
            double fromCenterY = y - (((seriesRenderer.ClipRect?.Height ?? 0) / 2) + (seriesRenderer.ClipRect?.Y ?? 0));

            double arcAngle = 2 * Math.PI * (point.RegionData.CurrentXPosition < 0 ? 1 + point.RegionData.CurrentXPosition : point.RegionData.CurrentXPosition);
            double startAngle = point.RegionData.StartAngle;
            double endAngle = point.RegionData.EndAngle;
            double distanceFromCenter = Math.Sqrt(Math.Pow(Math.Abs(fromCenterX), 2) + Math.Pow(Math.Abs(fromCenterY), 2));
            double clickAngle = (Math.Atan2(fromCenterY, fromCenterX) + (0.5 * Math.PI) - arcAngle) % (2 * Math.PI);

            clickAngle = clickAngle < 0 ? (2 * Math.PI) + clickAngle : clickAngle;
            clickAngle += 2 * Math.PI * (seriesRenderer.XAxisRenderer.Axis?.StartAngle ?? 0);

            startAngle -= arcAngle;
            startAngle = startAngle < 0 ? (2 * Math.PI) + startAngle : startAngle;

            endAngle -= arcAngle;
            endAngle = endAngle < 0 ? (2 * Math.PI) + endAngle : endAngle;

            bool withinAngle = clickAngle >= startAngle && clickAngle <= endAngle;
            bool withinRadius = (distanceFromCenter >= point.RegionData.InnerRadius && distanceFromCenter <= point.RegionData.Radius) ||
                                (distanceFromCenter <= point.RegionData.InnerRadius && distanceFromCenter >= point.RegionData.Radius);
            bool withinChartRadius = distanceFromCenter <= Chart?._axisContainer?.AxisLayout.Radius;

            return withinAngle && withinRadius && withinChartRadius;
        }

        /// <summary>
        /// Checks whether any region rectangle contains the given point.
        /// </summary>
        /// <param name="regionRect">Regions to test.</param>
        /// <param name="rect">Clip rect.</param>
        /// <param name="x">Mouse X.</param>
        /// <param name="y">Mouse Y.</param>
        /// <returns><c>true</c> if the location is within any region; otherwise <c>false</c>.</returns>
        private bool CheckRegionContainsPoint(List<Rect> regionRect, Rect rect, double x, double y)
        {
            Rect result = regionRect.Find(region =>
            {
                double originX = rect.X + region.X;
                double originY = rect.Y + region.Y;
                return ChartHelper.WithInBounds(x, y, new Rect(originX, originY, region.Width, region.Height));
            }) ?? null!;

            if (result is not null)
            {
                LierIndex = regionRect.IndexOf(result);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether the mouse is within the editable region of a rect-series point.
        /// </summary>
        /// <param name="x">Mouse X.</param>
        /// <param name="y">Mouse Y.</param>
        /// <param name="point">Chart point.</param>
        /// <param name="rect">Clip rect.</param>
        /// <param name="series">Series.</param>
        /// <returns><c>true</c> if inside edit region; otherwise <c>false</c>.</returns>
        private bool RectRegion(double x, double y, Point point, Rect rect, ChartSeries series)
        {
            double xOffset = 0, yOffset = 0, width = 20, height = 20;
            bool isInversed = series.Renderer.YAxisRenderer.Axis is not null && series.Renderer.YAxisRenderer.Axis.IsAxisInverse;

            if (isInversed && Chart is not null && Chart.IsTransposed)
            {
                if (series.Type == ChartSeriesType.Bar)
                {
                    yOffset = point.Regions[0].Height - 10;
                    width = point.Regions[0].Width;
                }
                else
                {
                    xOffset = -10;
                    height = point.Regions[0].Height;
                }
            }
            else if (isInversed || point.YValue < 0)
            {
                if (series.Type == ChartSeriesType.Bar)
                {
                    xOffset = -10;
                    height = point.Regions[0].Height;
                }
                else
                {
                    yOffset = point.Regions[0].Height - 10;
                    width = point.Regions[0].Width;
                }
            }
            else if (Chart is not null && Chart.IsTransposed)
            {
                if (series.Type == ChartSeriesType.Bar)
                {
                    yOffset = -10;
                    width = point.Regions[0].Width;
                }
                else
                {
                    xOffset = point.Regions[0].Width - 10;
                    height = point.Regions[0].Height;
                }
            }
            else
            {
                if (series.Type == ChartSeriesType.Bar)
                {
                    xOffset = point.Regions[0].Width - 10;
                    height = point.Regions[0].Height;
                }
                else
                {
                    yOffset = -10;
                    width = point.Regions[0].Width;
                }
            }

            return point.Regions.Any(region =>
            {
                double originX = rect.X + region.X + xOffset;
                double originY = rect.Y + region.Y + yOffset;
                return ChartHelper.WithInBounds(x, y, new Rect(originX, originY, width, height));
            });
        }

        /// <summary>
        /// Computes marker hit padding half-sizes for the current series.
        /// </summary>
        /// <param name="series">Series to evaluate.</param>
        /// <returns>Tuple of (halfWidth, halfHeight) for hit testing.</returns>
        private static (double halfWidth, double halfHeight) GetMarkerHalfSize(ChartSeries series)
        {
            bool needsMarkerHit = series.Type == ChartSeriesType.Scatter || (!series.Renderer.IsRectSeries() && series.Marker.Visible);

            if (!needsMarkerHit)
            {
                return (0, 0);
            }

            // Add a small padding to improve mouse hit usability.
            double halfWidth = (series.Marker.Height + MarkerHitPadding) / 2;
            double halfHeight = (series.Marker.Width + MarkerHitPadding) / 2;
            return (halfWidth, halfHeight);
        }

        /// <summary>
        /// Adjusts mouse coordinates for edit handles when chart data edit is enabled for rect series.
        /// </summary>
        /// <param name="seriesRenderer">Series renderer.</param>
        /// <param name="series">Series.</param>
        /// <param name="mouseX">Mouse X (by ref).</param>
        /// <param name="mouseY">Mouse Y (by ref).</param>
        private void AdjustMouseForEditing(ChartSeriesRenderer seriesRenderer, ChartSeries series, ref double mouseX, ref double mouseY)
        {
            if (series.ChartDataEditSettings?.Enable != true || !seriesRenderer.IsRectSeries())
            {
                return;
            }

            if (!(series.Type == ChartSeriesType.Bar && Chart is not null && Chart.IsTransposed) &&
                ((Chart is not null && Chart.IsTransposed) || series.Type == ChartSeriesType.Bar))
            {
                double markerWidth = (series.Marker?.Width ?? 0) / 2;
                mouseX = seriesRenderer.YAxisRenderer?.Axis is not null && seriesRenderer.YAxisRenderer.Axis.IsAxisInverse
                    ? mouseX + markerWidth
                    : mouseX - markerWidth;
            }
            else
            {
                double markerHeight = (series.Marker?.Height ?? 0) / 2;
                mouseY = seriesRenderer.YAxisRenderer?.Axis is not null && seriesRenderer.YAxisRenderer.Axis.IsAxisInverse
                    ? mouseY - markerHeight
                    : mouseY + markerHeight;
            }
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Gets the point data under the current mouse location across visible series.
        /// </summary>
        /// <returns>The <see cref="PointData"/> if found; otherwise, a container with null point.</returns>
        internal PointData GetData()
        {
            Point point = null!;
            ChartSeries series = null!;
            double mouseX;
            double mouseY;
            InsideRegion = false;

            for (int len = Chart?._visibleSeriesRenderers.Count ?? 0, i = len - 1; i >= 0; i--)
            {
                ChartSeriesRenderer seriesRenderer = Chart?._visibleSeriesRenderers[i] ?? null!;
                series = seriesRenderer?.Series ?? null!;

                if (seriesRenderer is null || series is null)
                {
                    continue;
                }

                Rect clipRect = seriesRenderer.ClipRect ?? null!;

                (double halfWidth, double halfHeight) = GetMarkerHalfSize(series);
                mouseX = Chart?._mouseX ?? 0;
                mouseY = Chart?._mouseY ?? 0;

                AdjustMouseForEditing(seriesRenderer, series, ref mouseX, ref mouseY);

                if (series.Visible && ChartHelper.WithInBounds(mouseX, mouseY, clipRect, halfWidth, halfHeight))
                {
                    point = GetRectPoint(seriesRenderer, clipRect, mouseX, mouseY);
                }

                if (point is not null)
                {
                    return new PointData(point, series);
                }
            }

            return new PointData(null!, null!);
        }

        /// <summary>
        /// Clears references and internal collections.
        /// </summary>
        internal virtual void Dispose()
        {
            Chart = null;

            CurrentPoints?.Clear();
            PreviousPoints?.Clear();
        }

        #endregion
    }
}
