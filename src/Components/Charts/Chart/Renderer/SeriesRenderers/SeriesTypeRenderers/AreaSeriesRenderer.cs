
﻿using Microsoft.AspNetCore.Components.Rendering;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renders an area series inside the chart renderer pipeline.
    /// </summary>
    /// <remarks>
    /// This internal renderer prepares path directions, datapoint payload and animation options
    /// required by the chart SVG/Canvas layer. Behaviour must remain consistent with prior implementation.
    /// </remarks>
    internal class AreaSeriesRenderer : AreaBaseSeriesRenderer
    {
        #region Fields
        private string? _datapoints;
        private double _origin;
        #endregion

        #region Private Methods

        /// <summary>
        /// Calculates the path direction and data points for the area series.
        /// </summary>
        private void CalculateDirection()
        {
            List<Point> visiblePoints = EnableComplexProperty() ?? [];
            Direction = new System.Text.StringBuilder();
            ChartData = new System.Text.StringBuilder();
            _origin = Math.Max(YAxisRenderer?.VisibleRange.Start ?? 0, 0);
            ChartEventLocation? startPoint = null!;
            int count = visiblePoints.Count;

            for (int i = 0; i < count; i++)
            {
                Point point = visiblePoints[i];
                point.SymbolLocations = [];
                point.Regions = [];

                Point previousPoint = (i - 1 > -1 && i - 1 < count) ? visiblePoints[i - 1] : null!;
                Point nextPoint = (i + 1 < count) ? visiblePoints[i + 1] : null!;

                if (point.Visible && ChartHelper.WithInRange(previousPoint, point, nextPoint, XAxisRenderer))
                {
                    ProcessVisiblePoint(point, ref startPoint, visiblePoints, i);
                }

                IChartPoint chartPoint = ChartPoints?[point.Index] ?? null!;
                if (IsTooltipEnabled() && chartPoint.SymbolLocations.Count > 0)
                {
                    AppendChartData(chartPoint);
                }
            }

            if (Series?.Renderer.Points?.Count > 1 && Direction.Length != 0)
            {
                GetAreaPathDirection(Series.Renderer.Points[^1].XValue, _origin, Series, Owner?._requireInvertedAxis ?? false, null!, "L");
            }
        }

        /// <summary>
        /// Handles a single visible point: builds path fragments, handles empty-point breaks and stores symbol locations.
        /// </summary>
        /// <param name="point">Point being processed.</param>
        /// <param name="startPoint">Reference to current path start point; updated if path closes or opens.</param>
        /// <param name="visiblePoints">List of visible points.</param>
        /// <param name="index">Index of the current point in the visiblePoints list.</param>
        private void ProcessVisiblePoint(Point point, ref ChartEventLocation? startPoint, List<Point> visiblePoints, int index)
        {
            double pointX = point.XValue;
            double pointY = point.YValue;

            GetAreaPathDirection(pointX, _origin, Series ?? null!, Owner?._requireInvertedAxis ?? false, startPoint!, "M");
            startPoint = startPoint is not null ? startPoint : new ChartEventLocation(pointX, _origin);

            GetAreaPathDirection(pointX, pointY, Series ?? null!, Owner?._requireInvertedAxis ?? false, null!, "L");

            bool isNextBreak = (index + 1 < visiblePoints.Count)
                && (visiblePoints[index + 1] is not null)
                && (!visiblePoints[index + 1].Visible)
                && !(Series?.EmptyPointSettings.Mode == EmptyPointMode.Drop);

            if (isNextBreak)
            {
                GetAreaEmptyDirection(new ChartEventLocation(pointX, _origin), startPoint, Series ?? null!, Owner?._requireInvertedAxis ?? false);
                startPoint = null!;
            }

            StorePointLocation(point, Series ?? null!, Owner?._requireInvertedAxis ?? false);
            _datapoints = GetDataPoints(pointX, pointY);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Renders the series: prepares direction, path options and animation metadata.
        /// </summary>
        protected override void RenderSeries()
        {
            base.RenderSeries();
            CalculateDirection();

            string name = SeriesID();
            _options = new PathOptions()
            {
                Id = name,
                Fill = Interior ?? string.Empty,
                StrokeWidth = Series?.Border.Width ?? 0,
                Stroke = Series?.Border.Color ?? string.Empty,
                Opacity = Series?.Opacity ?? 1,
                StrokeDashArray = Series?.DashArray ?? string.Empty,
                Direction = Series?.Renderer.Points?.Count > 1 && Direction.Length != 0 ? Direction.ToString() : string.Empty,
                DataPoint = _datapoints ?? string.Empty,
            };

            bool isAnimationDefault = Series is not null && Series.Animation.Enable && SyncfusionService?._options.Animation == GlobalAnimationMode.Default;
            if (Owner is not null && Owner._shouldAnimateSeries && (isAnimationDefault || (SyncfusionService?._options.Animation == GlobalAnimationMode.Enable)))
            {
                AnimationOptions = new AnimationOptions(Series?.Renderer.ClipRectId() ?? null!, AnimationType.Linear);
            }
        }

        /// <summary>
        /// Builds the render tree for the area series element(s).
        /// </summary>
        /// <param name="builder">Render tree builder.</param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            if (ClipRect is null || builder is null || (Series is not null && !Series.Visible))
            {
                return;
            }

            CreateSeriesElements(builder);
            SetBorderOptions();
            RenderSeriesElement(builder, _options ?? null!);
            RenderSeriesElement(builder, _borderOptions ?? null!);
            builder.CloseElement();
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Updates the precomputed path direction and border options.
        /// </summary>
        internal override void UpdateDirection()
        {
            CalculateDirection();
            if (_options is { })
            {
                _options.Direction = Direction.ToString();
            }

            SetBorderOptions();

            if (_borderOptions is { })
            {
                _borderOptions.Direction = GetBorderDirection(_options?.Direction ?? null!);
            }

            base.UpdateDirection();
        }
        #endregion
    }
}
