using Microsoft.AspNetCore.Components.Rendering;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renders stacked bar series visuals (stacked and stacked100 variants).
    /// </summary>
    /// <remarks>
    /// This renderer computes rectangular paths for stacked bars, triggers point events,
    /// and supplies path options for SVG rendering. It preserves existing behaviors while
    /// improving readability, null-safety and single-responsibility of methods.
    /// </remarks>
    public class StackingBarSeriesRenderer : ColumnBaseRenderer
    {
        #region Private Methods

        /// <summary>
        /// Ensures collections used for rendering on the point are initialized.
        /// </summary>
        /// <param name="point">The point to initialize collections for.</param>
        private static void EnsurePointCollections(Point point)
        {
            point.SymbolLocations = [];
            point.Regions = [];
        }

        /// <summary>
        /// Updates geometry, path and collection for a computed column rectangle.
        /// </summary>
        /// <param name="point">Point being rendered.</param>
        /// <param name="rect">Computed SVG rectangle in pixel coordinates.</param>
        /// <param name="pointIdPrefix">Prefix used for element id.</param>
        /// <param name="visibility">SVG visibility attribute for this path.</param>
        /// <param name="argsData">Point render event args providing fill/border info.</param>
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
        /// Renders the series by computing column path options then animating as required.
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
        /// Renders SVG path elements for computed column path options.
        /// </summary>
        /// <param name="builder">Render tree builder provided by Blazor.</param>
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
        /// Updates geometry direction after layout change.
        /// </summary>
        internal override void UpdateDirection()
        {
            ColumnPathOptions.Clear();
            CalculateColumnPathOption();
            base.UpdateDirection();
        }

        /// <summary>
        /// Calculates path options for each visible stacking bar point.
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

                if (pointColumn.Visible && ChartHelper.WithInRange(previousPoint, pointColumn, nextPoint, Series?.Renderer.XAxisRenderer ?? null!) && Series?.Renderer.StackedValues is not null)
                {
                    double startValue = GetStackingStartValue(pointColumn.Index, GetVisibleSeriesIndex());
                    Rect rect = GetRectangle(pointColumn.XValue + sideBySideInfo.Start, (!Series.Visible && Series._isLegendClicked) ? startValue : Series.Renderer.StackedValues.EndValues[pointColumn.Index], pointColumn.XValue + sideBySideInfo.End, (!Series.Visible && Series._isLegendClicked) ? startValue : Series.Renderer.StackedValues.StartValues[pointColumn.Index]);
                    rect = GetColumnWidthInPixelRect(rect);
                    PointRenderEventArgs argsData = TriggerEvent(pointColumn, Interior ?? null!, new BorderModel() { Width = Series.Border.Width, Color = Series.Border.Color });

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
        /// Returns true when a stacking column is non-negative and the label position is Outer.
        /// </summary>
        /// <param name="isMinus">Indicates negative value.</param>
        /// <param name="series">Series to inspect.</param>
        /// <returns>True if column is non-negative and label is Outer; otherwise false.</returns>
        internal override bool IsStackingColumnAndOuterPosition(bool isMinus, ChartSeries series)
        {
            return !isMinus && Series?.Marker.DataLabel.Position == ChartLabelPosition.Outer;
        }

        #endregion        
    }

    /// <summary>
    /// Renderer specialization for 100% stacked bar series; inherits stacking behavior.
    /// </summary>
    public class StackingBar100SeriesRenderer : StackingBarSeriesRenderer
    {
    }
}