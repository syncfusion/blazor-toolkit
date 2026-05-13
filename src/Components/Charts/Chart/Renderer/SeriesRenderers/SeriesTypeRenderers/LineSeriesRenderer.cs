using Microsoft.AspNetCore.Components.Rendering;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renders a standard line series for the chart. Responsible for building the path
    /// directions, preparing animation options and invoking base render behavior.
    /// </summary>
    internal class LineSeriesRenderer : LineBaseSeriesRenderer
    {
        #region Private Methods

        /// <summary>
        /// Handles a visible point: computes direction segment and updates previous coordinates.
        /// </summary>
        /// <param name="prevPointX">Previous point X coordinate (updated).</param>
        /// <param name="prevPointY">Previous point Y coordinate (updated).</param>
        /// <param name="pointX">Current point X coordinate.</param>
        /// <param name="pointY">Current point Y coordinate.</param>
        /// <param name="startPoint">Current SVG path command (updated).</param>
        private void ProcessVisiblePoint(ref double prevPointX, ref double prevPointY, double pointX, double pointY, ref string startPoint)
        {
            GetLineDirection(prevPointX, prevPointY, pointX, pointY, Owner?._requireInvertedAxis ?? false, startPoint);
            startPoint = !double.IsNaN(prevPointX) ? "L" : startPoint;
            prevPointX = pointX;
            prevPointY = pointY;
        }

        /// <summary>
        /// Resets state for an empty or out-of-range point according to EmptyPointSettings.
        /// Also clears symbol locations for client-side keyboard/tooltip support.
        /// </summary>
        /// <param name="isDrop">Whether empty points should drop to previous value.</param>
        /// <param name="prevPointX">Previous point X coordinate (possibly reset).</param>
        /// <param name="prevPointY">Previous point Y coordinate (possibly reset).</param>
        /// <param name="startPoint">SVG path command to reset to (updated).</param>
        /// <param name="point">Point that is missing or not in range.</param>
        private void HandleMissingPoint(bool isDrop, ref double prevPointX, ref double prevPointY, ref string startPoint, Point point)
        {
            prevPointX = isDrop ? prevPointX : double.NaN;
            prevPointY = isDrop ? prevPointY : double.NaN;
            startPoint = "M";
            point.SymbolLocations = [];
            if (ChartPoints is { })
            {
                ChartPoints[point.Index].SymbolLocations = [];
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Prepares series rendering state and options then delegates to the base renderer.
        /// </summary>
        protected override void RenderSeries()
        {
            base.RenderSeries();
            CalculateDirection();
            _options = new PathOptions(SeriesID(), Direction.ToString(), Series?.DashArray ?? string.Empty, Series?.Width ?? 0, Interior ?? string.Empty, Series?.Opacity ?? 1, "None", " ", " ", " ", " ", " ", GetDataPoints());
            if (Owner is not null && Owner._shouldAnimateSeries && Series is not null && Series.Animation.Enable)
            {
                AnimationOptions = new AnimationOptions(ClipRectId(), AnimationType.Linear);
            }
        }

        /// <summary>
        /// Renders the series elements into the provided render tree builder.
        /// Guarded to avoid rendering when clip rectangle or series visibility is invalid.
        /// </summary>
        /// <param name="builder">Render tree builder to receive elements.</param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            if (ClipRect is null || builder is null || (Series is not null && !Series.Visible) || double.IsNaN(ClipRect.Height) || double.IsNaN(ClipRect.Y))
            {
                return;
            }

            CreateSeriesElements(builder);
            RenderSeriesElement(builder, _options ?? null!);
            builder.CloseElement();
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Computes the SVG path direction and chart data string for visible points.
        /// Ensures empty-point handling, tooltip chart-data population and point location storage.
        /// </summary>
        internal void CalculateDirection()
        {
            string startPoint = "M";
            bool isDrop = Series?.EmptyPointSettings.Mode == EmptyPointMode.Drop;
            List<Point> visiblePoints = EnableComplexProperty();
            Direction = new System.Text.StringBuilder();
            ChartData = new System.Text.StringBuilder();

            double prevPointX = double.NaN;
            double prevPointY = double.NaN;
            int count = visiblePoints.Count;

            for (int i = 0; i < count; i++)
            {
                Point point = visiblePoints[i];
                point.SymbolLocations = [];
                point.Regions = [];

                Point prevPoint = (i - 1 > -1) && (i - 1 < count) ? visiblePoints[i - 1] : null!;
                Point nextPoint = i + 1 < count ? visiblePoints[i + 1] : null!;

                if (point.Visible && ChartHelper.WithInRange(prevPoint, point, nextPoint, XAxisRenderer))
                {
                    double pointX = point.XValue;
                    double pointY = point.YValue;
                    ProcessVisiblePoint(ref prevPointX, ref prevPointY, pointX, pointY, ref startPoint);
                    StorePointLocation(point, Series ?? null!, Owner is not null && Owner._requireInvertedAxis);
                }
                else
                {
                    HandleMissingPoint(isDrop, ref prevPointX, ref prevPointY, ref startPoint, point);
                }
                //Series points are needed on the script side for keyboard navigation if markers and tooltips are not enabled.
                IChartPoint chartPoint = ChartPoints?[point.Index] ?? null!;
                if (IsTooltipEnabled() && chartPoint.SymbolLocations.Count > 0)
                {
                    AppendChartData(chartPoint);
                }
            }
        }

        /// <summary>
        /// Updates the direction path when series data has changed.
        /// </summary>
        internal override void UpdateDirection()
        {
            CalculateDirection();
            if (_options is { })
            {
                _options.Direction = Direction.ToString();
            }

            base.UpdateDirection();
        }
        #endregion
    }

    /// <summary>
    /// Specialized renderer used for Pareto line series. Inherits standard line rendering behavior.
    /// </summary>
    internal class ParetoLineSeriesRenderer : LineSeriesRenderer
    {
    }
}
