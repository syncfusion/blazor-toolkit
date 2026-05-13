
﻿using Microsoft.AspNetCore.Components.Rendering;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renders bar (vertical) series by generating path options for each point and producing SVG path elements.
    /// </summary>
    /// <remarks>
    /// This renderer translates data points into rectangle paths, triggers point render events,
    /// and collects path options for rendering and animation. It preserves behavior from the original implementation
    /// while improving readability, splitting responsibilities into helpers, and adding XML documentation.
    /// </remarks>
    internal class BarSeriesRenderer : ColumnBaseRenderer
    {
        #region Private Methods

        /// <summary>
        /// Calculates path options for each column/bar point in the series.
        /// </summary>
        private void CalculateColumnPathOption()
        {
            DoubleRange sideBySideInfo = GetSideBySideInfo();
            ChartData = new System.Text.StringBuilder();
            double origin = Math.Max(YAxisRenderer?.VisibleRange.Start ?? 0, 0);
            string pointId = Series?.Container?.ID + "_Series_" + Index + "_Point_";
            string visibility = ShouldAnimate() ? "hidden" : "visible";

            for (int i = 0; i < Points?.Count; i++)
            {
                ProcessPoint(i, sideBySideInfo, origin, pointId, visibility);
            }
        }

        /// <summary>
        /// Processes a single data point: builds rectangle, triggers render event, updates symbol location,
        /// and appends a PathOptions instance when applicable.
        /// </summary>
        /// <param name="index">Index of the point in the series.</param>
        /// <param name="sideBySideInfo">Side-by-side offset information for multi-series layouts.</param>
        /// <param name="origin">Pixel origin for the baseline (usually zero or visible range start).</param>
        /// <param name="pointIdPrefix">Prefix used to compose the element id for the point.</param>
        /// <param name="visibility">Visibility setting for initial animation state.</param>
        private void ProcessPoint(int index, DoubleRange sideBySideInfo, double origin, string pointIdPrefix, string visibility)
        {
            if (Points is null)
            {
                return;
            }
            Point pointBar = Points[index];
            pointBar.SymbolLocations = [];
            pointBar.Regions = [];
            Point previousPoint = index - 1 > -1 ? Points[index - 1] : null!;
            Point nextPoint = index + 1 < Points.Count ? Points[index + 1] : null!;

            if (pointBar.Visible && ChartHelper.WithInRange(previousPoint, pointBar, nextPoint, XAxisRenderer))
            {
                Rect rect = GetRectangle(pointBar.XValue + sideBySideInfo.Start, pointBar.YValue, pointBar.XValue + sideBySideInfo.End, origin);
                rect = GetColumnWidthInPixelRect(rect);

                PointRenderEventArgs argsData = TriggerEvent(pointBar, Interior ?? null!, new BorderModel()
                {
                    Width = Series?.Border.Width ?? 0,
                    Color = (YAxisRenderer?.VisibleRange.Start >= pointBar.YValue) ? string.Empty : Series?.Border.Color ?? string.Empty
                });

                if (!argsData.Cancel)
                {
                    UpdateColumnPath(pointIdPrefix, pointBar, rect, argsData, visibility);
                }
            }

            //Series points are needed on the script side for keyboard navigation if markers and tooltips are not enabled.
            if (IsTooltipEnabled() && pointBar.SymbolLocations.Count > 0)
            {
                AppendChartData(ChartPoints?[pointBar.Index]);
            }
        }

        private void UpdateColumnPath(string pointIdPrefix, Point pointBar, Rect rect, PointRenderEventArgs argsData, string visibility)
        {
            UpdateSymbolLocation(pointBar, rect);

            string id = pointIdPrefix + pointBar.Index;
            string direction = CalculateRectangle(pointBar, rect, id, argsData.CornerRadius);
            if (direction is not null)
            {
                PathOptions option = new
                (
                    id, direction, Series?.DashArray ?? string.Empty,
                    argsData.Border.Width, argsData.Border.Color, Series?.Opacity ?? 1,
                    argsData.Fill, string.Empty, string.Empty, AccessText ?? string.Empty, "", "",
                    GetDataPoints(pointBar.XValue, pointBar.YValue)
                )
                {
                    Visibility = visibility
                };
                ColumnPathOptions.Add(option);
            }
        }
        #endregion

        #region Protected Methods

        /// <summary>
        /// Builds series path options and triggers animation.
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
        /// Renders the collected path options into the provided <see cref="RenderTreeBuilder"/>.
        /// </summary>
        /// <param name="builder">Render tree builder used to produce markup.</param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            if (ClipRect is null || builder is null || (Series is not null && !Series.Visible))
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
        /// Recomputes direction and path options when the series direction changes.
        /// </summary>
        internal override void UpdateDirection()
        {
            ColumnPathOptions.Clear();
            CalculateColumnPathOption();
            base.UpdateDirection();
        }

        /// <summary>
        /// Indicates this renderer produces rectangular shapes.
        /// </summary>
        /// <returns><see langword="true"/> since bar series are rect-based.</returns>
        internal override bool IsRectSeries()
        {
            return true;
        }
        #endregion
    }
}
