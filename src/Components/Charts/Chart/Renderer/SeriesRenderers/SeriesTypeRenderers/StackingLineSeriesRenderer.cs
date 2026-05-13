using Microsoft.AspNetCore.Components.Rendering;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renders a stacked line series. Responsible for computing path directions,
    /// preparing render options and rendering the SVG path for a stacked line.
    /// </summary>
    internal class StackingLineSeriesRenderer : LineBaseSeriesRenderer
    {
        #region Private Methods

        /// <summary>
        /// Initializes StringBuilder instances used during rendering.
        /// </summary>
        private void InitializeDirectionBuilders()
        {
            Direction = new System.Text.StringBuilder();
            ChartData = new System.Text.StringBuilder();
        }

        /// <summary>
        /// Prepares point collections (Regions and SymbolLocations) for a point.
        /// </summary>
        /// <param name="point">The point to prepare collections for.</param>
        private void PreparePointCollections(Point point)
        {
            point.Regions = [];
            point.SymbolLocations = [];
            if (ChartPoints is not null)
            {
                ChartPoints[point.Index].SymbolLocations = [];
                ChartPoints[point.Index].Regions = [];
            }
        }

        /// <summary>
        /// Processes a single visible point: appends direction, computes symbol locations and regions.
        /// </summary>
        /// <param name="point">The currently processed point.</param>
        /// <param name="segmentIndex">Reference to the current segment index for path commands.</param>
        /// <param name="stackedValue">Stacked values for the series.</param>
        /// <param name="index">Index of the current point within the visiblePoints collection.</param>
        private void ProcessVisiblePoint(Point point, ref int segmentIndex, StackValues stackedValue, int index)
        {
            ChartEventLocation point1 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(point.XValue), YAxisRenderer.GetPointValue(stackedValue.EndValues[index]), XAxisRenderer, YAxisRenderer, XLength, YLength, Owner is not null && Owner._requireInvertedAxis);
            _ = Direction.Append((segmentIndex != 0 ? "L" : "M") + SPACE + point1.X.ToString(Culture) + SPACE + point1.Y.ToString(Culture) + SPACE);
            point.SymbolLocations.Add(point1);
            point.SymbolLocations.ForEach(loc =>
            {
                ChartPoints?[point.Index]?.SymbolLocations.Add(new IChartInternalLocation(Math.Round(loc.X, 2), Math.Round(loc.Y, 2)));
            });
            point.Regions.Add(new Rect(point.SymbolLocations[0].X - (Series?.Marker.Width ?? 0), point.SymbolLocations[0].Y - (Series?.Marker.Height ?? 0), 2 * (Series?.Marker.Width ?? 0), 2 * (Series?.Marker.Height ?? 0)));
            point.Regions.ForEach(rect =>
            {
                ChartPoints?[point.Index]?.Regions.Add(new IRect(Math.Round(rect.X, 2), Math.Round(rect.Y, 2), rect.Width, rect.Height));
            });
            segmentIndex++;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Prepares rendering options for the series and triggers animation setup when required.
        /// </summary>
        protected override void RenderSeries()
        {
            base.RenderSeries();
            CalculateDirection();
            string name = SeriesID();
            _options = new PathOptions()
            {
                Id = name,
                Fill = Constants.Transparent,
                StrokeWidth = Series?.Width ?? 0,
                Stroke = Interior ?? string.Empty,
                Opacity = Series?.Opacity ?? 1,
                StrokeDashArray = Series?.DashArray ?? string.Empty,
                Direction = Direction.ToString(),
                DataPoint = GetDataPoints()
            };

            if (Owner is not null && Owner._shouldAnimateSeries && ((Series is not null && Series.Animation.Enable && SyncfusionService?._options.Animation == GlobalAnimationMode.Default) || (SyncfusionService?._options.Animation == GlobalAnimationMode.Enable)))
            {
                AnimationOptions = new AnimationOptions(ClipRectId(), AnimationType.Linear);
            }
        }

        /// <summary>
        /// Builds DOM render tree for the series path element.
        /// </summary>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            if (ClipRect is null || builder is null || (Series is not null && !Series.Visible))
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
        /// Calculates the path direction string and prepares symbol locations/regions for points.
        /// </summary>
        internal void CalculateDirection()
        {
            List<Point> visiblePoints = EnableComplexProperty();
            InitializeDirectionBuilders();

            if (Series is not null && !Series.Visible)
            {
                return;
            }

            StackValues stackedValue = Series?.Renderer.StackedValues ?? null!;
            int segmentIndex = 0;
            for (int i = 0; i < visiblePoints.Count; i++)
            {
                Point point = visiblePoints[i];
                PreparePointCollections(point);
                Point previousPoint = i - 1 > -1 ? visiblePoints[i - 1] : null!;
                Point nextPoint = i + 1 < visiblePoints.Count ? visiblePoints[i + 1] : null!;

                if (point.Visible && ChartHelper.WithInRange(previousPoint, point, nextPoint, XAxisRenderer))
                {
                    ProcessVisiblePoint(point, ref segmentIndex, stackedValue, i);
                }
                else if (Series?.EmptyPointSettings.Mode != EmptyPointMode.Drop && nextPoint is not null && nextPoint.Visible)
                {
                    ChartEventLocation point1 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(nextPoint.XValue), YAxisRenderer.GetPointValue(stackedValue.EndValues[i + 1]), XAxisRenderer, YAxisRenderer, XLength, YLength, Owner is not null && Owner._requireInvertedAxis);
                    _ = Direction.Append("M" + SPACE + point1.X.ToString(Culture) + SPACE + point1.Y.ToString(Culture) + SPACE);
                }
                //Series points are needed on the script side for keyboard navigation if markers and tooltips are not enabled.
                if (IsTooltipEnabled() && point.SymbolLocations.Count > 0)
                {
                    AppendChartData(ChartPoints?[point.Index]);
                }
            }
        }

        /// <summary>
        /// Updates direction when data changes and notifies base renderer to refresh.
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
    /// Variant renderer for internal use to support specific versioning or naming.
    /// </summary>
    internal class StackingLine100SeriesRenderer : StackingLineSeriesRenderer
    {
    }
}