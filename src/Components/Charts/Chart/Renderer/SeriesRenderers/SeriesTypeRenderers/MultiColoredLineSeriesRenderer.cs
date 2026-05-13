using Microsoft.AspNetCore.Components.Rendering;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renderer for multi-colored line series. Produces segmented path options and supports per-segment customization.
    /// </summary>
    internal class MultiColoredLineSeriesRenderer : MultiColoredBaseSeriesRenderer
    {
        #region Fields
        private new readonly List<PathOptions> _options = [];
        private List<ChartSegment> _segments = [];
        #endregion

        #region Private Methods

        /// <summary>
        /// Sets animation options when the owner requests animated rendering.
        /// </summary>
        private void ConfigureAnimationIfRequired()
        {
            bool isDefaultAnimation = Series is not null && Series.Animation.Enable && SyncfusionService?._options.Animation == GlobalAnimationMode.Default;
            if (Owner is not null && Owner._shouldAnimateSeries && (isDefaultAnimation || (SyncfusionService?._options.Animation == GlobalAnimationMode.Enable)))
            {
                AnimationOptions = new AnimationOptions(ClipRectId(), AnimationType.Linear);
            }
        }

        /// <summary>
        /// Calculates path directions for the multi-colored line series, generating segmented path options based on point visibility and segment axis. This method also prepares tooltip data and handles empty points according to the series configuration.
        /// </summary>
        private List<ChartSegment> CalculateDirection()
        {
            _options.Clear();
            Direction = new System.Text.StringBuilder();
            ChartData = new System.Text.StringBuilder();
            bool isInverted = Owner is not null && Owner._requireInvertedAxis;
            List<Point> visiblePoints = EnableComplexProperty();
            _segments = SortSegments(Series ?? null!, Series?.Segments ?? null!);

            ProcessVisiblePoints(visiblePoints, isInverted);
            AddResidualPath(visiblePoints);
            return _segments;
        }

        /// <summary>
        /// Processes the list of visible points to construct path directions and segment options. Handles both visible and invisible points, applying appropriate logic for empty points and tooltip data preparation.
        /// </summary>
        private void ProcessVisiblePoints(List<Point> visiblePoints, bool isInverted)
        {
            string startPoint = "M";
            Point previous = null!;
            int count = visiblePoints.Count;

            for (int i = 0; i < count; i++)
            {
                Point point = visiblePoints[i];
                PreparePointCollections(point);

                Point previousPoint = i - 1 > -1 ? visiblePoints[i - 1] : null!;
                Point nextPoint = i + 1 < count ? visiblePoints[i + 1] : null!;

                if (point.Visible && ChartHelper.WithInRange(previousPoint, point, nextPoint, XAxisRenderer))
                {
                    startPoint = HandleVisiblePoint(point, previous, isInverted, startPoint);
                    previous = point;
                    StorePointLocation(point, Series ?? null!, isInverted);
                }
                else
                {
                    previous = HandleInvisiblePoint(point, previous, ref startPoint);
                }

                AppendTooltipData(point);
            }
        }

        /// <summary>
        /// Prepares the symbol locations and region collections for a given point. This is necessary to ensure that even points that are not visible (e.g., empty points) have their collections initialized, preventing potential null reference issues during rendering and interaction handling.
        /// </summary>
        private static void PreparePointCollections(Point point)
        {
            point.SymbolLocations = [];
            point.Regions = [];
        }

        /// <summary>
        /// Handles the logic for a visible point, including direction calculation, segment option creation, and color assignment based on the point's properties and its relation to the previous point. This method also determines whether to start a new path segment or continue the existing one based on the visibility and segment axis configuration.
        /// </summary>
        private string HandleVisiblePoint(Point point, Point previous, bool isInverted, string startPoint)
        {
            if (previous is not null)
            {
                GetLineDirection(previous.XValue, previous.YValue, point.XValue, point.YValue, isInverted, startPoint);

                if (SetPointColor(point, previous, Series ?? null!, Series?.SegmentAxis == Segment.X, _segments))
                {
                    _options.Add(new PathOptions(
                        Owner?.ID + "_Series_" + Series?.Renderer.Index + "_Point_" + previous.Index,
                        Direction.ToString(),
                        Series?.DashArray ?? string.Empty,
                        Series?.Width ?? 0,
                        SetPointColor(previous, Interior ?? string.Empty),
                        Series?.Opacity ?? 1,
                        "none", " ", " ", " ", " ", " ",
                        GetDataPoints()));

                    Direction = new System.Text.StringBuilder();
                    return "M";
                }

                return "L";
            }

            _ = SetPointColor(point, null!, Series ?? null!, Series?.SegmentAxis == Segment.X, _segments);
            return startPoint;
        }

        /// <summary>
        /// Handles the logic for an invisible point (e.g., an empty point), determining how to adjust the path direction and segment options based on the series configuration for empty points. Depending on whether the series is configured to drop empty points or not, this method may reset the previous point reference and adjust the starting point for the next segment accordingly.
        /// </summary>
        private Point HandleInvisiblePoint(Point point, Point previous, ref string startPoint)
        {
            bool dropMode = Series?.EmptyPointSettings.Mode == EmptyPointMode.Drop;
            Point resolvedPrevious = dropMode ? previous ?? null! : null!;
            startPoint = dropMode ? startPoint : "M";
            point.SymbolLocations = [];

            if (ChartPoints is { })
            {
                ChartPoints[point.Index].SymbolLocations = [];
            }

            return resolvedPrevious;
        }

        /// <summary>
        /// Appends tooltip data for a given point if tooltips are enabled and the point has symbol locations. This method retrieves the corresponding chart point from the ChartPoints collection and, if conditions are met, calls the method to append chart data for tooltip display. Note that this method assumes that the ChartPoints collection is properly initialized and contains entries corresponding to the points being processed, which is crucial for avoiding null reference issues during tooltip data preparation.
        /// </summary>
        private void AppendTooltipData(Point point)
        {
            IChartPoint chartPoint = ChartPoints?[point.Index] ?? null!;
            if (IsTooltipEnabled() && chartPoint.SymbolLocations.Count > 0)
            {
                AppendChartData(chartPoint);
            }
        }

        /// <summary>
        /// Adds a final path option for any remaining visible points after processing the main list. This is necessary to ensure that any segments that were being constructed are properly finalized and included in the rendering options, especially in cases where the last point(s) are visible and may not have been added to the options list during the main processing loop.
        /// </summary>
        private void AddResidualPath(List<Point> visiblePoints)
        {
            if (Direction.Length == 0)
            {
                return;
            }

            _options.Add(new PathOptions(
                Owner?.ID + "_Series_" + Series?.Renderer.Index,
                Direction.ToString(),
                Series?.DashArray ?? string.Empty,
                Series?.Width ?? 0,
                SetPointColor(visiblePoints[^1], Interior ?? null!),
                Series?.Opacity ?? 1,
                "none", " ", " ", " ", " ", " ",
                GetDataPoints()));
        }
        #endregion

        #region Protected Methods

        /// <summary>
        /// Renders the multi-colored line series and prepares animation options when required.
        /// </summary>
        protected override void RenderSeries()
        {
            base.RenderSeries();
            _ = CalculateDirection();
            ConfigureAnimationIfRequired();
        }

        /// <summary>
        /// Constructs render tree for the series paths and segments.
        /// </summary>
        /// <param name="builder">Render tree builder to receive rendered fragments.</param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            if (ClipRect is null || builder is null || (Series is not null && !Series.Visible))
            {
                return;
            }

            CreateSeriesElements(builder);
            if (_options.Count != 0)
            {
                ApplySegmentAxis(builder, Series ?? null!, _options, _segments);
            }

            builder.CloseElement();
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Updates internal direction/segments when external updates require recomputation.
        /// </summary>
        internal override void UpdateDirection()
        {
            _ = CalculateDirection();
            base.UpdateDirection();
        }

        /// <summary>
        /// Applies customization updates to existing path options.
        /// </summary>
        /// <param name="property">Name of the property that changed.</param>
        internal override void UpdateCustomization(string property)
        {
            RendererShouldRender = Series is not null && Series.Visible;
            if (property == "Fill")
            {
                Owner?._seriesContainer?.GetSeriesRendererInterior(this);
            }
            foreach (PathOptions option in _options)
            {
                switch (property)
                {
                    case "Fill":
                        option.Stroke = Interior ?? string.Empty;
                        Series?.Marker.Renderer?.MarkerColorChanged();
                        break;
                    case "DashArray":
                        option.StrokeDashArray = Series?.DashArray ?? string.Empty;
                        break;
                    case "Width":
                        option.StrokeWidth = Series?.Width ?? 0;
                        break;
                    case "Opacity":
                        option.Opacity = Series?.Opacity ?? 1;
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion
    }
}
