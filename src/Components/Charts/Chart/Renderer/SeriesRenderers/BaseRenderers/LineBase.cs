using Microsoft.AspNetCore.Components.Rendering;
using System.Runtime.InteropServices;
using System.Text;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Base renderer providing shared line-series rendering helpers.
    /// </summary>
    /// <remarks>
    /// Refactored for clarity, security, and maintainability. Preserves original behavior while
    /// improving naming, regions, and method sizes.
    /// </remarks>
    internal abstract class LineBaseSeriesRenderer : ChartSeriesRenderer
    {
        #region Constants
        protected const string SPACE = " ";
        #endregion

        #region Fields
        protected PathOptions? _options;
        protected PathOptions? _borderOptions;
        #endregion

        #region Properties

        /// <summary>
        /// Reference to the parent chart instance.
        /// </summary>
        internal SfChart Chart { get; set; } = null!;

        /// <summary>
        /// Mutable builder used for constructing SVG path directions.
        /// </summary>
        protected StringBuilder Direction { get; set; } = new StringBuilder();

        internal LineBaseSeriesRenderer([Optional] SfChart chart)
        {
            Chart = chart;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Computes X and Y tolerances used to collapse near-duplicate points.
        /// </summary>
        /// <param name="areaBounds">Clip rectangle for the chart area.</param>
        /// <returns>Tuple with X and Y tolerances.</returns>
        private (double xTolerance, double yTolerance) ComputeTolerances(Rect areaBounds)
        {
            double x_Tolerance = Math.Abs(XAxisRenderer.VisibleRange.Delta / areaBounds.Width);
            double y_Tolerance = Math.Abs(YAxisRenderer.VisibleRange.Delta / areaBounds.Height);
            return (x_Tolerance, y_Tolerance);
        }

        /// <summary>
        /// Validates that an object converts to a numeric double.
        /// </summary>
        /// <param name="data">Object to validate.</param>
        /// <returns>True when convertible to double; otherwise false.</returns>
        private static bool IsValid(object data)
        {
            return double.TryParse(Convert.ToString(data, null), out double _);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Produces a filtered list of points when the series has EnableComplexProperty true.
        /// </summary>
        /// <returns>A reduced list of points for rendering.</returns>
        protected List<Point> EnableComplexProperty()
        {
            List<Point> tempPoints = [];
            List<Point> tempPoints2 = [];
            Rect areaBounds = ClipRect ?? null!;

            if (Series is not null && Series.EnableComplexProperty && Points?.Count > 0)
            {
                (double xTolerance, double yTolerance) = ComputeTolerances(areaBounds);
                double prevXValue = (Points[0] is not null && IsValid(Points[0].X) && (Convert.ToDouble(Points[0].X, null) > xTolerance)) ? 0 : xTolerance;
                double prevYValue = (Points[0] is not null && IsValid(Points[0].Y) && (Convert.ToDouble(Points[0].Y, null) > xTolerance)) ? 0 : yTolerance;

                foreach (Point currentPoint in Points.ToArray())
                {
                    currentPoint.SymbolLocations = [];
                    double x_Val = currentPoint.XValue != 0 ? currentPoint.XValue : XAxisRenderer.VisibleRange.Start;
                    double y_Val = currentPoint.YValue != 0 ? currentPoint.YValue : YAxisRenderer.VisibleRange.Start;
                    if (Math.Abs(prevXValue - x_Val) >= xTolerance || Math.Abs(prevYValue - y_Val) >= yTolerance)
                    {
                        tempPoints.Add(currentPoint);
                        prevXValue = x_Val;
                        prevYValue = y_Val;
                    }
                }

                for (int i = 0; i < tempPoints.Count; i++)
                {
                    if (tempPoints[i].X is null || string.IsNullOrEmpty(Convert.ToString(tempPoints[i].X, null)))
                    {
                        continue;
                    }
                    else
                    {
                        tempPoints2.Add(tempPoints[i]);
                    }
                }
            }

            return tempPoints2.Count > 0 ? tempPoints2 : Points ?? null!;
        }

        /// <summary>
        /// Builds line direction between two points and appends to the direction buffer.
        /// </summary>
        /// <param name="firstPointX">First point X value.</param>
        /// <param name="firstPointY">First point Y value.</param>
        /// <param name="secondPointX">Second point X value.</param>
        /// <param name="secondPointY">Second point Y value.</param>
        /// <param name="isInverted">Indicates if axes are inverted.</param>
        /// <param name="startPoint">Starting path command (e.g., 'M').</param>
        protected virtual void GetLineDirection(double firstPointX, double firstPointY, double secondPointX, double secondPointY, bool isInverted, string startPoint)
        {
            if (!double.IsNaN(firstPointX) && !double.IsNaN(firstPointY) && !double.IsNaN(YLength))
            {
                ChartEventLocation point1 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(firstPointX), YAxisRenderer.GetPointValue(firstPointY), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                ChartEventLocation point2 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(secondPointX), YAxisRenderer.GetPointValue(secondPointY), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                _ = Direction.Append(string.Join(string.Empty, startPoint, SPACE, Convert.ToInt64(point1.X), SPACE, Convert.ToInt64(point1.Y), SPACE, 'L', SPACE, Convert.ToInt64(point2.X), SPACE, Convert.ToInt64(point2.Y), SPACE));
            }
        }

        /// <summary>
        /// Builds polar line direction between two points and appends to the direction buffer.
        /// </summary>
        /// <param name="firstPointX">First point X value.</param>
        /// <param name="firstPointY">First point Y value.</param>
        /// <param name="secondPointX">Second point X value.</param>
        /// <param name="secondPointY">Second point Y value.</param>
        /// <param name="isInverted">Indicates if axes are inverted.</param>
        /// <param name="startPoint">Starting path command (e.g., 'M').</param>
        protected virtual void GetPolarLineDirection(double firstPointX, double firstPointY, double secondPointX, double secondPointY, bool isInverted, string startPoint)
        {
            if (!double.IsNaN(firstPointX))
            {
                ChartEventLocation point1 = ChartHelper.TransformToVisible(firstPointX, firstPointY, XAxisRenderer.Axis ?? null!, YAxisRenderer.Axis ?? null!, Series ?? null!);
                ChartEventLocation point2 = ChartHelper.TransformToVisible(secondPointX, secondPointY, XAxisRenderer.Axis ?? null!, YAxisRenderer.Axis ?? null!, Series ?? null!);
                _ = Direction.Append(string.Join(string.Empty, startPoint, SPACE, Convert.ToInt64(point1.X), SPACE, Convert.ToInt64(point1.Y), SPACE, 'L', SPACE, Convert.ToInt64(point2.X), SPACE, Convert.ToInt64(point2.Y), SPACE));
            }
        }

        /// <summary>
        /// Returns the SVG path fragment for a step-style line between two visible points.
        /// </summary>
        /// <param name="point1">First visible point.</param>
        /// <param name="point2">Second visible point.</param>
        /// <param name="stepPosition">Step position style.</param>
        /// <returns>Path string fragment.</returns>
        protected string GetStepLineDirection(ChartEventLocation point1, ChartEventLocation point2, StepPosition stepPosition)
        {
            if (stepPosition == StepPosition.Right)
            {
                return "L" + SPACE + point1.X.ToString(Culture) + SPACE + point2.Y.ToString(Culture) + SPACE + " L" + SPACE + point2.X.ToString(Culture) + SPACE + point2.Y.ToString(Culture) + SPACE;
            }
            else if (stepPosition == StepPosition.Center)
            {
                double midX = point1.X + ((point2.X - point1.X) / 2);
                return "L" + SPACE + midX.ToString(Culture) + SPACE + point1.Y.ToString(Culture) + SPACE + "L" + SPACE + midX.ToString(Culture) + SPACE + point2.Y.ToString(Culture) + SPACE + " L" + SPACE + point2.X.ToString(Culture) + SPACE + point2.Y.ToString(Culture) + SPACE;
            }
            else
            {
                return "L" + SPACE + point2.X.ToString(Culture) + SPACE + point1.Y.ToString(Culture) + SPACE + " L" + SPACE + point2.X.ToString(Culture) + SPACE + point2.Y.ToString(Culture) + SPACE;
            }
        }

        /// <summary>
        /// Stores symbol location and region information for a point.
        /// </summary>
        /// <param name="point">Point to store metadata for.</param>
        /// <param name="series">Series owning the point.</param>
        /// <param name="isInverted">Whether chart axes are inverted.</param>
        protected virtual void StorePointLocation(Point point, ChartSeries series, bool isInverted)
        {
            if (ChartPoints is not null)
            {
                ChartPoints[point.Index].SymbolLocations = [];
                ChartPoints[point.Index].Regions = [];
            }

            double markerWidth = Series is not null && !double.IsNaN(Series.Marker.Width) ? Series.Marker.Width : 0;
            double markerHeight = Series is not null && !double.IsNaN(Series.Marker.Height) ? Series.Marker.Height : 0;
            ChartEventLocation location = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(point.XValue), YAxisRenderer.GetPointValue(point.YValue), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);

            if ((location.X + ClipRect?.X + markerWidth >= ClipRect?.X && location.Y + ClipRect.Y + markerHeight >= ClipRect.Y) ||
                (Owner is not null && Owner._zoomingModule is not null && Owner._zoomingModule.IsZoomed))
            {
                point?.SymbolLocations.Add(location);
                series.Renderer.ChartPoints?[point?.Index ?? 0]?.SymbolLocations.Add(new IChartInternalLocation(Math.Round(location.X, 2), Math.Round(location.Y, 2)));
                point?.Regions.Add(new Rect(point.SymbolLocations[0].X - markerWidth, point.SymbolLocations[0].Y - markerHeight, 2 * markerWidth, 2 * markerHeight));
                series.Renderer.ChartPoints?[point?.Index ?? 0]?.Regions.Add(new IRect(Math.Round(point?.SymbolLocations[0].X ?? 0, 2) - markerWidth, Math.Round(point?.SymbolLocations[0].Y ?? 0, 2) - markerHeight, 2 * markerWidth, 2 * markerHeight));
            }
        }

        /// <summary>
        /// Returns border path direction derived from the main series direction for area types.
        /// </summary>
        /// <param name="seriesDirection">Main series direction string.</param>
        /// <returns>Border direction string.</returns>
        protected virtual string GetBorderDirection(string seriesDirection)
        {
            List<string> coordinates = [.. seriesDirection.Split(" ")];
            RemoveEmptyPointsBorder(coordinates);
            if (coordinates.Count > 3)
            {
                coordinates.RemoveRange(coordinates.Count - 4, 3);
            }
            return string.Join(" ", coordinates);
        }

        /// <summary>
        /// Removes segments corresponding to empty points from an area's border coordinates.
        /// </summary>
        /// <param name="coordinates">Mutable list of coordinate tokens.</param>
        protected virtual void RemoveEmptyPointsBorder(List<string> coordinates)
        {
            int startIndex = 0;
            int currentIndex;
            do
            {
                currentIndex = coordinates.FindIndex(startIndex, x => x.Contains('M', StringComparison));
                if (currentIndex > -1)
                {
                    if (coordinates.Count > currentIndex + 1)
                    {
                        int countToRemove = Math.Min(3, coordinates.Count - (currentIndex + 1));
                        coordinates.RemoveRange(currentIndex + 1, countToRemove);
                    }
                    startIndex = currentIndex + 1;
                    if (currentIndex - 6 > 0)
                    {
                        coordinates.RemoveRange(currentIndex - 6, 6);
                        startIndex -= 6;
                    }
                }
            } while (currentIndex != -1);
        }

        /// <summary>
        /// Creates and assigns border path <see cref="PathOptions"/> for area series.
        /// </summary>
        protected void SetBorderOptions()
        {
            _borderOptions = new PathOptions
            (
                _options?.Id + "_Border", GetBorderDirection(_options?.Direction ?? null!),
                _options?.StrokeDashArray ?? string.Empty, Series?.Border.Width ?? 0,
                !string.IsNullOrEmpty(Series?.Border.Color) ? Series.Border.Color : _options?.Fill ?? string.Empty,
                1.0,
                Constants.Transparent,
                _options?.StrokeMiterLimit ?? string.Empty,
                _options?.ClipPath ?? string.Empty,
                _options?.AccessibilityText ?? string.Empty,
                _options?.Style ?? string.Empty
            );
            if (_options is not null)
            {
                _options.StrokeWidth = 0;
                _options.Stroke = Constants.Transparent;
                _borderOptions.DataPoint = _options.DataPoint;
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Returns the first and last visible points in the provided list.
        /// </summary>
        /// <param name="points">List of points to inspect.</param>
        /// <returns>A <see cref="ConnectPoints"/> with First and Last visible points.</returns>
        internal static ConnectPoints GetFirstLastVisiblePoint(List<Point> points)
        {
            Point first = null!;
            Point last = null!;
            foreach (Point point in points)
            {
                if (first is null && point.Visible)
                {
                    first = last = point;
                }

                last = point.Visible ? point : last;
            }

            return new ConnectPoints() { First = first, Last = last };
        }

        /// <summary>
        /// Updates renderer state for a customization property.
        /// </summary>
        /// <param name="property">Name of the property changed (e.g., "Fill").</param>
        internal override void UpdateCustomization(string property)
        {
            base.UpdateCustomization(property);
            switch (property)
            {
                case "Fill":
                    if (_options is { })
                    {
                        _options.Stroke = Interior ?? string.Empty;
                    }

                    Series?.Marker.Renderer?.MarkerColorChanged();
                    break;
                case "DashArray":
                    if (_options is { })
                    {
                        _options.StrokeDashArray = Series?.DashArray ?? string.Empty;
                    }

                    break;
                case "Width":
                    if (_options is { })
                    {
                        _options.StrokeWidth = Series?.Width ?? 1;
                    }

                    break;
                case "Opacity":
                    if (_options is { })
                    {
                        _options.Opacity = Series?.Opacity ?? 1;
                    }

                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Determines if the point's Y value lies within the axis visible range.
        /// </summary>
        /// <param name="point">Point to test.</param>
        /// <param name="y_Axis">YAxis renderer to evaluate.</param>
        /// <returns>True if within visible range; otherwise false.</returns>
        internal static bool WithinYRange(Point point, ChartAxisRenderer y_Axis)
        {
            return point.YValue >= y_Axis.VisibleRange.Start && point.YValue <= y_Axis.VisibleRange.End;
        }

        /// <summary>
        /// Renders the series path element using the provided path options.
        /// </summary>
        /// <param name="builder">Render tree builder.</param>
        /// <param name="options">Path options to render.</param>
        internal void RenderSeriesElement(RenderTreeBuilder builder, PathOptions options)
        {
            if (options is not null)
            {
                options.Direction = ChartHelper.AppendPathElements(Owner ?? null!, options.Direction, options.Id, SeriesElementId());
                _ = SvgRenderer?.RenderPath(builder, options);
            }
        }

        /// <summary>
        /// Appends the line path to the owner SVG and renders it.
        /// </summary>
        /// <param name="builder">Render tree builder.</param>
        /// <param name="options">Path options instance.</param>
        internal void AppendLinePath(RenderTreeBuilder builder, PathOptions options)
        {
            if (options is not null)
            {
                options.Direction = ChartHelper.AppendPathElements(Owner ?? null!, options.Direction, options.Id);
                _ = Owner?._svgRenderer?.RenderPath(builder, options);
            }
        }

        /// <summary>
        /// Indicates this renderer renders a path-based series.
        /// </summary>
        /// <returns>True (path series).</returns>
        internal override bool IsPathSeries()
        {
            return true;
        }

        /// <summary>
        /// Disposes of renderer resources and clears references to allow garbage collection.
        /// </summary>
        protected override ValueTask DisposeAsyncCore()
        {
            Chart = null!;
            _ = Direction?.Clear();
            return base.DisposeAsyncCore();
        }
        #endregion
    }
}