using Microsoft.AspNetCore.Components.Rendering;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renderer that supports multi-colored segments for chart series.
    /// </summary>
    /// <remarks>
    /// This class extends <see cref="AreaBaseSeriesRenderer"/> and provides logic to
    /// sort segments, compute clipping rectangles and apply per-segment colors.
    /// </remarks>
    internal class MultiColoredBaseSeriesRenderer : AreaBaseSeriesRenderer
    {
        #region Fields
        // Tracks the maximum numeric/time value encountered while computing segments.
        private double _maxSegmentValue = double.NegativeInfinity;
        #endregion

        #region Private Methods

        /// <summary>
        /// Creates a clip-path rectangle for a segment and renders it to the provided builder.
        /// </summary>
        /// <param name="builder">Render tree builder used to emit SVG elements.</param>
        /// <param name="startValue">Start axis value for clipping.</param>
        /// <param name="endValue">End axis value for clipping.</param>
        /// <param name="index">Segment index (used for clip id).</param>
        /// <param name="isX">Whether the segment axis is X.</param>
        /// <returns>CSS url() clip-path reference string or <c>null</c> when not applicable.</returns>
        private string CreateClipRect(RenderTreeBuilder builder, double startValue, double endValue, int index, bool isX, int seriesIndex)
        {
            bool isInverted = Owner?._requireInvertedAxis ?? false;
            string clipPathId = Owner?.ID + "_Series_" + seriesIndex + "_ChartSegmentClipRect_" + index;

            ChartEventLocation startPointLocation = ChartHelper.GetPoint
            (
                XAxisRenderer.GetPointValue(isX ? startValue : XAxisRenderer.VisibleRange.Start),
                YAxisRenderer.GetPointValue(isX ? YAxisRenderer.VisibleRange.End : endValue),
                XAxisRenderer,
                YAxisRenderer,
                XLength,
                YLength,
                isInverted
            );

            ChartEventLocation endPointLocation = ChartHelper.GetPoint
            (
                XAxisRenderer.GetPointValue(isX ? endValue : XAxisRenderer.VisibleRange.End),
                YAxisRenderer.GetPointValue(isX ? YAxisRenderer.VisibleRange.Start : startValue),
                XAxisRenderer,
                YAxisRenderer,
                XLength,
                YLength,
                isInverted
            );

            if ((endPointLocation.X - startPointLocation.X > 0) && (endPointLocation.Y - startPointLocation.Y > 0))
            {
                Owner?._svgRenderer?.OpenClipPath(builder, Owner._svgRenderer.Seq++, clipPathId);
                Owner?._svgRenderer?.RenderRect(builder, new RectOptions(
                    clipPathId + "_Rect",
                    startPointLocation.X,
                    startPointLocation.Y,
                    endPointLocation.X - startPointLocation.X,
                    endPointLocation.Y - startPointLocation.Y,
                    1,
                    "Gray",
                    Constants.Transparent,
                    0, 0, 1));

                builder.CloseElement();
                return "url(#" + clipPathId + ")";
            }

            return null!;
        }

        /// <summary>
        /// Computes an axis-position value for a segment value, honoring axis types.
        /// </summary>
        /// <param name="segmentValue">The raw segment value (may be null).</param>
        /// <param name="axis">Axis to use for conversions.</param>
        /// <returns>Numeric axis value suitable for comparisons and sorting.</returns>
        private double GetAxisValue(object segmentValue, ChartAxis axis)
        {
            if (segmentValue is null && axis.ValueType != ValueType.DateTime)
            {
                segmentValue = (axis.ValueType == ValueType.Double) ? Math.Max(axis.Renderer?.VisibleRange.End ?? 0, _maxSegmentValue) : axis.Renderer?.VisibleRange.End ?? 0;
            }

            if (axis.ValueType == ValueType.DateTime)
            {
                if (segmentValue is double val)
                {
                    return val;
                }
                return ChartHelper.GetTime(segmentValue is not null ? Convert.ToDateTime(segmentValue, Culture) : new DateTime(1970, 1, 1).AddMilliseconds(Math.Max(axis.Renderer?.VisibleRange.End ?? 0, _maxSegmentValue)));
            }
            else if (axis.AxisValueType is not null && axis.AxisValueType.Contains("Category", StringComparison.InvariantCulture))
            {
                return GetCategoryIndex(axis, segmentValue);
            }

            return Convert.ToDouble(segmentValue, null);
        }

        /// <summary>
        /// Resolves the index for a category axis label based on the segment value.
        /// </summary>
        /// <param name="axis">Axis with categories/labels.</param>
        /// <param name="segmentValue">Value to resolve.</param>
        /// <returns>Index within axis labels, or labels.Count when not found.</returns>
        private int GetCategoryIndex(ChartAxis axis, object? segmentValue)
        {
            string xValue = axis.ValueType == ValueType.DateTimeCategory
                ? DateTime.TryParse(segmentValue?.ToString(), out DateTime validDate) ? ChartHelper.GetTime(validDate).ToString(Culture) ?? string.Empty : segmentValue?.ToString() ?? string.Empty
                : segmentValue?.ToString() ?? string.Empty;
            return (axis.Renderer?.Labels.IndexOf(xValue) == -1) ? axis.Renderer.Labels.Count : axis.Renderer?.Labels.IndexOf(xValue) ?? 0;
        }

        /// <summary>
        /// Ensures there is a trailing segment to cover the axis visible range end.
        /// </summary>
        /// <param name="segments">Segment list to possibly extend.</param>
        /// <param name="axis">Axis used to determine visible range end.</param>
        /// <param name="length">Current length before modification.</param>
        private void IncludeSegment(List<ChartSegment> segments, ChartAxis axis, int length)
        {
            if (length <= 0)
            {
                ChartSegment chartSegment = new();
                chartSegment.SetSegmentValue(axis.Renderer?.VisibleRange.End ?? 0, Interior ?? null!);
                segments.Add(chartSegment);
                return;
            }

            if (GetAxisValue(segments[length - 1].Value, axis) < axis.Renderer?.VisibleRange.End)
            {
                ChartSegment chartSegment = new();
                chartSegment.SetSegmentValue(axis.Renderer.VisibleRange.End, Interior ?? null!);
                segments.Add(chartSegment);
            }
        }

        /// <summary>
        /// Builds a PathOptions instance for a single segment based on base path options and segment metadata.
        /// </summary>
        /// <param name="baseOption">Original path option.</param>
        /// <param name="series">Series that owns the path.</param>
        /// <param name="segment">Segment metadata (color, dash array).</param>
        /// <param name="index">Segment index.</param>
        /// <param name="clipPath">Clip path reference (url(#id)).</param>
        /// <returns>New <see cref="PathOptions"/> configured for the segment.</returns>
        private PathOptions BuildAttributeOptions(PathOptions baseOption, ChartSeries series, ChartSegment segment, int index, string clipPath)
        {
            return new PathOptions()
            {
                ClipPath = clipPath,
                StrokeDashArray = segment.DashArray,
                Opacity = baseOption.Opacity,
                Stroke = series?.SeriesType is not null && series.SeriesType.Contains("MultiColored", StringComparison.InvariantCulture) ? string.IsNullOrEmpty(segment.Color) ? Interior ?? string.Empty : segment.Color : series?.Border.Color ?? string.Empty,
                Fill = series?.SeriesType != null && series.SeriesType.Contains("Line", StringComparison.InvariantCulture) ? "none" : string.IsNullOrEmpty(segment.Color) ? Interior ?? string.Empty : segment.Color,
                Id = baseOption.Id + "_Segment_" + index,
                Direction = baseOption.Direction,
                StrokeWidth = baseOption.StrokeWidth,
                DataPoint = baseOption.DataPoint
            };
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Sorts segments for the provided series using the configured segment axis.
        /// </summary>
        /// <param name="series">The series containing segment axis configuration.</param>
        /// <param name="chartSegments">The list of chart segments to sort.</param>
        /// <returns>The sorted list of segments (same list instance).</returns>
        internal List<ChartSegment> SortSegments(ChartSeries series, IList<ChartSegment> chartSegments)
        {
            List<ChartSegment> segments = chartSegments?.ToList() ?? [];
            ChartAxis axis = series.SegmentAxis == Segment.X ? XAxisRenderer.Axis ?? null! : YAxisRenderer.Axis ?? null!;
            segments.Where((x) => axis.ValueType is not (ValueType.Category or ValueType.DateTimeCategory)).ToList().ForEach((a) =>
            {
                if (a.Value != null)
                {
                    double currentSegmentValue;
                    if (axis.ValueType == ValueType.DateTime)
                    {
                        currentSegmentValue = a.Value switch
                        {
                            DateTime dt => ChartHelper.GetTime(dt),
                            double d => d,
                            _ => ChartHelper.GetTime(Convert.ToDateTime(a.Value, Culture))
                        };
                    }
                    else
                    {
                        currentSegmentValue = Convert.ToDouble(a.Value, Culture);
                    }
                    _maxSegmentValue = Math.Max(_maxSegmentValue, currentSegmentValue);
                }
            });
            segments.Sort(delegate (ChartSegment a, ChartSegment b)
            {
                return GetAxisValue(a.Value, axis).CompareTo(GetAxisValue(b.Value, axis));
            });
            return segments;
        }

        /// <summary>
        /// Sets the fill/stroke color for the current point using the segment list or point color mapping.
        /// </summary>
        /// <param name="currentPoint">Point being colored.</param>
        /// <param name="previous">Previous point for comparison when using point color mapping.</param>
        /// <param name="series">Series metadata.</param>
        /// <param name="isXSegment">Whether segmentation occurs on the X axis.</param>
        /// <param name="segments">List of configured segments.</param>
        /// <returns>True when color changed compared to the previous point (used for boundary detection).</returns>
        /// <summary>
        /// Sets the fill/stroke color for the current point using the segment list or point color mapping.
        /// </summary>
        /// <param name="point">Point being colored.</param>
        /// <param name="color">Default series color.</param>
        /// <returns>The resolved color for the point.</returns>
        public override string SetPointColor(Point point, string color)
        {
            return point is not null && !string.IsNullOrEmpty(point.Interior) ? point.Interior : color;
        }


        /// </summary>
        internal bool SetPointColor(Point currentPoint, Point previous, ChartSeries series, bool isXSegment, List<ChartSegment> segments)
        {
            if (string.IsNullOrEmpty(series.PointColorMapping))
            {
                double compareValue = isXSegment ? currentPoint.XValue : currentPoint.YValue;
                for (int i = 0; i < segments.Count; i++)
                {
                    if (compareValue <= GetAxisValue(segments[i].Value, isXSegment ? XAxisRenderer.Axis ?? null! : YAxisRenderer.Axis ?? null!) || segments[i].Value is null)
                    {
                        if (series.Renderer.ChartPoints is not null)
                        {
                            currentPoint.Interior = series.Renderer.ChartPoints[currentPoint.Index].Interior = segments[i].Color;
                        }

                        break;
                    }
                }

                currentPoint.Interior ??= Interior ?? string.Empty;
                return previous is not null && SetPointColor(currentPoint, Interior ?? string.Empty) != SetPointColor(previous, Interior ?? string.Empty);
            }

            else
            {
                return previous is not null && SetPointColor(currentPoint, Interior ?? null!) != SetPointColor(previous, Interior ?? null!);
            }
        }

        /// <summary>
        /// Applies segment clipping and renders per-segment path options for a series.
        /// </summary>
        /// <param name="builder">RenderTreeBuilder to emit SVG elements.</param>
        /// <param name="series">Series metadata.</param>
        /// <param name="options">Precomputed path options to apply per segment.</param>
        /// <param name="segments">List of chart segments.</param>
        internal void ApplySegmentAxis(RenderTreeBuilder builder, ChartSeries series, List<PathOptions> options, List<ChartSegment> segments)
        {
            if (!string.IsNullOrEmpty(series.PointColorMapping))
            {
                foreach (PathOptions option in options)
                {
                    AppendLinePath(builder, option);
                }

                return;
            }

            ChartAxis axis = series.SegmentAxis == Segment.X ? XAxisRenderer.Axis ?? null! : YAxisRenderer.Axis ?? null!;
            IncludeSegment(segments, axis, segments.Count);
            PathOptions attributeOptions = null!;

            for (int index = 0; index < segments.Count; index++)
            {
                ChartSegment segment = segments[index];
                string clipPath = CreateClipRect
                (
                    builder,
                    index > 0 ? GetAxisValue(segments[index - 1].Value, axis) : axis.Renderer?.VisibleRange.Start ?? 0,
                    GetAxisValue(segment.Value, axis),
                    index,
                    series?.SegmentAxis == Segment.X,
                    series?.Renderer.Index ?? 0
                );

                if (!string.IsNullOrEmpty(clipPath))
                {
                    foreach (PathOptions option in options)
                    {
                        attributeOptions = BuildAttributeOptions(option, series!, segment, index, clipPath);
                        attributeOptions.Direction = ChartHelper.AppendPathElements(Owner ?? null!, attributeOptions.Direction, attributeOptions.Id);
                        _ = Owner?._svgRenderer?.RenderPath(builder, attributeOptions);
                    }
                }
            }

        }
        #endregion
    }
}