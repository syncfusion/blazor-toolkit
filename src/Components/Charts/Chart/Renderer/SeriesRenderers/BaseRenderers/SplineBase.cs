using System.Runtime.InteropServices;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Base renderer for spline series implementations.
    /// </summary>
    /// <remarks>
    /// Provides spline coefficient calculation and control-point generation for various spline types.
    /// </remarks>
    internal abstract class SplineBaseSeriesRenderer : LineBaseSeriesRenderer
    {
        #region Constants
        private const double ONE_THIRD = 1 / 3.0;
        #endregion

        #region Fields
        private double[]? _splinePoints;
        #endregion

        #region Internal Properties
        /// <summary>
        /// Gets or sets the control point for spline curve calculation.
        /// </summary>
        internal List<ControlPoints> DrawPoints { get; set; } = [];
        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Called when parent parameters are set. Propagates spline type from associated series.
        /// </summary>
        internal override void OnParentParameterSet()
        {
            base.OnParentParameterSet();
        }

        /// <summary>
        /// Cleans up component state for garbage collection.
        /// </summary>
        protected override ValueTask DisposeAsyncCore()
        {
            _splinePoints = null;
            return base.DisposeAsyncCore();
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Performs division with guard clauses for zero denominators and non-finite results.
        /// Returns the fallback value when the denominator is zero, NaN, or would produce Infinity/NaN.
        /// </summary>
        private static double SafeDivide(double n, double d, double fallback)
        {
            return (d == 0 || double.IsNaN(d) || double.IsInfinity(n / d)) ? fallback : n / d;
        }

        /// <summary>
        /// Creates control points for each segment of the spline based on the filtered points and their corresponding spline coefficients.
        /// </summary>
        private void CreateDrawPoints(List<Point> points, ref bool isNegativePoint)
        {
            if (_splinePoints is null)
            {
                return;
            }
            foreach (Point data in points.ToArray())
            {
                isNegativePoint = isNegativePoint ? isNegativePoint : data.YValue < 0;
                if (data.Index != 0)
                {
                    int previous = GetPreviousIndex(points, data.Index - 1, Series ?? null!);
                    ControlPoints pointValue = GetControlPoints(points[previous], data, _splinePoints[previous], _splinePoints[data.Index], Series ?? null!);
                    if (pointValue is not null)
                    {
                        DrawPoints.Add(pointValue);
                    }
                    if (data.YValue != 0 && !double.IsNaN(data.YValue) && pointValue?.ControlPoint1.Y != 0 && !double.IsNaN(pointValue.ControlPoint1.Y) && pointValue.ControlPoint2.Y != 0 && !double.IsNaN(pointValue.ControlPoint2.Y) && Series is not null && (Series.Renderer.YMax - Series.Renderer.YMin) > 1)
                    {
                        bool maxValue = Math.Abs(Series.Renderer.YMin - Math.Floor(Series.Renderer.YMin)) > double.Epsilon;
                        Series.Renderer.YMin = maxValue ? Series.Renderer.YMin : Math.Floor(Math.Min(Math.Min(Series.Renderer.YMin, data.YValue), Math.Min(pointValue.ControlPoint1.Y, pointValue.ControlPoint2.Y)));
                        Series.Renderer.YMax = Math.Ceiling(Math.Max(Math.Max(Series.Renderer.YMax, data.YValue), Math.Max(pointValue.ControlPoint1.Y, pointValue.ControlPoint2.Y)));
                    }
                }
            }
        }

        /// <summary>
        /// Computes monotonic spline coefficients for a point list, ensuring that the resulting spline does not introduce new extrema between points, thus preserving the monotonicity of the data.
        /// </summary>
        private static void ComputeMonotonicCoefficients(List<Point> points, ChartSeries series, double[] y_Spline, bool isLow)
        {
            int count = points.Count;
            int slopeSize = Math.Max(0, count - 1);
            double[] dx = new double[slopeSize];
            double[] dy = new double[slopeSize];
            double[] slope = new double[slopeSize];

            for (int i = 0; i < count - 1; i++)
            {
                series.Renderer.GetSplineTypePoints(points, i, SplineType.Monotonic, isLow);
                dx[i] = !double.IsNaN(points[i + 1].XValue - points[i].XValue) ? points[i + 1].XValue - points[i].XValue : 0;
                dy[i] = !double.IsNaN(points[i + 1].YValue - points[i].YValue) ? points[i + 1].YValue - points[i].YValue : 0;
                slope[i] = dy[i] / dx[i];
            }

            y_Spline[0] = slope[0];
            y_Spline[count - 1] = slope[^1];

            for (int j = 0; j < dx.Length; j++)
            {
                if (slope.Length > j + 1)
                {
                    if (slope[j] * slope[j + 1] <= 0)
                    {
                        y_Spline[j + 1] = 0;
                    }
                    else
                    {
                        double interPoint = dx[j] + dx[j + 1];
                        y_Spline[j + 1] = 3 * interPoint / (((interPoint + dx[j + 1]) / slope[j]) + ((interPoint + dx[j]) / slope[j + 1]));
                    }
                }
            }
        }

        /// <summary>
        /// Computes cardinal spline coefficients for a point list, applying the specified tension to control the curvature of the resulting spline. The tension parameter allows for adjusting the "tightness" of the curve, with values closer to 0 producing a looser curve and values closer to 1 producing a tighter curve that more closely follows the control points.
        /// </summary>
        private static void ComputeCardinalCoefficients(List<Point> points, ChartSeries series, double[] y_Spline)
        {
            int count = points.Count;
            double cardinalSplineTension = series.CardinalSplineTension > 0 ? series.CardinalSplineTension : 0.5;
            cardinalSplineTension = cardinalSplineTension < 0 ? 0 : cardinalSplineTension > 1 ? 1 : cardinalSplineTension;

            for (int i = 0; i < count; i++)
            {
                y_Spline[i] = i == 0
                    ? (count > 2) ? (cardinalSplineTension * (points[i + 2].XValue - points[i].XValue)) : 0
                    : i == (count - 1)
                        ? (count > 2) ? (cardinalSplineTension * (points[count - 1].XValue - points[count - 3].XValue)) : 0
                        : cardinalSplineTension * (points[i + 1].XValue - points[i - 1].XValue);
            }
        }

        /// <summary>
        /// Computes natural or clamped spline coefficients for a point list, depending on the specified spline type. For natural splines, the second derivatives at the endpoints are set to zero, resulting in a "natural" curvature. For clamped splines, the first derivatives at the endpoints are specified based on the slope of the first and last segments, allowing for more control over the shape of the spline at the boundaries.
        /// </summary>
        private static void ComputeNaturalOrClampedCoefficients(List<Point> points, ChartSeries series, bool isLow, double[] y_Spline)
        {
            int count = points.Count;
            double[] y_SplineDuplicate = new double[count];

            if (series.SplineType == SplineType.Clamped)
            {
                for (int i = 1; i < count - 1; i++)
                {
                    series.Renderer.GetSplineTypePoints(points, i, SplineType.Clamped, isLow);
                }

                double firstSegmentX = points[1].XValue - points[0].XValue;
                double lastSegmentX = points[^1].XValue - points[^2].XValue;

                y_Spline[0] = SafeDivide(3 * (points[1].YValue - points[0].YValue), firstSegmentX, 0) - 3;
                y_SplineDuplicate[0] = 0.5;

                if (y_Spline.Length > points.Count - 1)
                {
                    y_Spline[points.Count - 1] = SafeDivide(3 * (points[^1].YValue - points[^2].YValue), lastSegmentX, 0);
                }

                if (double.IsInfinity(Math.Abs(y_Spline[0])))
                {
                    y_Spline[0] = y_SplineDuplicate[0] = 0;
                }

                if (y_Spline.Length > points.Count - 1 && double.IsInfinity(Math.Abs(y_Spline[points.Count - 1])))
                {
                    y_Spline[points.Count - 1] = y_SplineDuplicate[points.Count - 1] = 0;
                }
            }
            else
            {
                y_Spline[0] = y_SplineDuplicate[0] = 0;

                if (y_Spline.Length > points.Count - 1)
                {
                    y_Spline[points.Count - 1] = 0;
                }
            }

            for (int i = 1; i < count - 1; i++)
            {
                y_Spline[0] = y_SplineDuplicate[0] = 0;

                if (y_Spline.Length > points.Count - 1)
                {
                    y_Spline[count - 1] = 0;
                }

                series.Renderer.GetDefaultSplineTypePoints(points, i, isLow);
                double coefficient1 = points[i].XValue - points[i - 1].XValue;
                double coefficient2 = points[i + 1].XValue - points[i - 1].XValue;
                double coefficient3 = points[i + 1].XValue - points[i].XValue;
                double dy1 = double.IsNaN(points[i + 1].YValue - points[i].YValue) ? 0 : points[i + 1].YValue - points[i].YValue;
                double dy2 = double.IsNaN(points[i].YValue - points[i - 1].YValue) ? 0 : points[i].YValue - points[i - 1].YValue;

                if (coefficient1 == 0 || coefficient2 == 0 || coefficient3 == 0)
                {
                    y_Spline[i] = 0;
                    y_SplineDuplicate[i] = 0;
                }
                else
                {
                    double p = SafeDivide(1, (coefficient1 * y_Spline[i - 1]) + (2 * coefficient2), 0);
                    y_Spline[i] = -p * coefficient3;
                    y_SplineDuplicate[i] = p * ((6 * (SafeDivide(dy1, coefficient3, 0) - SafeDivide(dy2, coefficient1, 0))) - (coefficient1 * y_SplineDuplicate[i - 1]));
                }
            }

            for (int k = count - 2; k >= 0; k--)
            {
                y_Spline[k] = (y_Spline[k] * y_Spline[k + 1]) + y_SplineDuplicate[k];
            }
        }

        /// <summary>
        /// Computes spline coefficients for a point list and spline type.
        /// </summary>
        /// <param name="points">Input points.</param>
        /// <param name="series">Series metadata.</param>
        /// <param name="isLow">Optional flag for low-spline computations.</param>
        /// <returns>Array of spline coefficients per point.</returns>
        private static double[] FindSplineCoefficients(List<Point> points, ChartSeries series, bool isLow = false)
        {
            int count = points.Count;
            double[] y_Spline = new double[count];

            if (count == 0)
            {
                return y_Spline;
            }

            switch (series.SplineType)
            {
                case SplineType.Monotonic:
                    ComputeMonotonicCoefficients(points, series, y_Spline, isLow);
                    break;
                case SplineType.Cardinal:
                    ComputeCardinalCoefficients(points, series, y_Spline);
                    break;
                default:
                    ComputeNaturalOrClampedCoefficients(points, series, isLow, y_Spline);
                    break;
            }

            return y_Spline;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Returns previous non-null index for empty-point handling.
        /// </summary>
        /// <param name="points">The point list.</param>
        /// <param name="i">Starting index.</param>
        /// <param name="series">Series instance.</param>
        /// <returns>Index of previous valid point.</returns>
        protected static int GetPreviousIndex(List<Point> points, int i, ChartSeries series)
        {
            if (series.EmptyPointSettings.Mode != EmptyPointMode.Drop)
            {
                return i;
            }

            bool isNull = i <= -1 || points[i] is null;
            while (isNull && i > -1)
            {
                i -= 1;
            }

            return i;
        }

        /// <summary>
        /// Resolves a millisecond-equivalent for the current X-axis interval type.
        /// </summary>
        /// <returns>Milliseconds for one unit of the ActualIntervalType.</returns>
        protected double DateTimeInterval()
        {
            IntervalType interval = XAxisRenderer.ActualIntervalType;
            double intervalInMilliseconds = 86400000;

            switch (interval)
            {
                case IntervalType.Years:
                    intervalInMilliseconds = 365 * intervalInMilliseconds;
                    break;

                case IntervalType.Months:
                    intervalInMilliseconds = 30 * intervalInMilliseconds;
                    break;

                case IntervalType.Days:
                    return intervalInMilliseconds;

                case IntervalType.Hours:
                    intervalInMilliseconds = 60 * 60 * 1000;
                    break;

                case IntervalType.Minutes:
                    intervalInMilliseconds = 60 * 1000;
                    break;

                case IntervalType.Seconds:
                    intervalInMilliseconds = 1000;
                    break;
                case IntervalType.Auto:
                    break;
                default:
                    intervalInMilliseconds = 30 * intervalInMilliseconds;
                    break;
            }

            return intervalInMilliseconds;

        }

        /// <summary>
        /// Generates control points for a segment between two points.
        /// </summary>
        /// <param name="prevdata">Previous data point.</param>
        /// <param name="nextData">Next data point.</param>
        /// <param name="y_Spline1">Spline coefficient for previous point.</param>
        /// <param name="y_Spline2">Spline coefficient for next point.</param>
        /// <param name="series">Series metadata.</param>
        /// <returns>Pair of control points.</returns>
        protected ControlPoints? GetControlPoints(Point prevdata, Point nextData, double y_Spline1, double y_Spline2, ChartSeries series)
        {
            ControlPoints point;
            double y_SplineDuplicate1 = y_Spline1;
            double y_SplineDuplicate2 = y_Spline2;

            switch (series.SplineType)
            {
                case SplineType.Cardinal:
                    if (XAxisRenderer.Axis?.ValueType == ValueType.DateTime)
                    {
                        y_SplineDuplicate1 = y_Spline1 / DateTimeInterval();
                        y_SplineDuplicate2 = y_Spline2 / DateTimeInterval();
                    }

                    point = new ControlPoints(new ChartEventLocation(prevdata.XValue + (y_Spline1 / 3), prevdata.YValue + (y_SplineDuplicate1 / 3)), new ChartEventLocation(nextData.XValue - (y_Spline2 / 3), nextData.YValue - (y_SplineDuplicate2 / 3)));
                    break;
                case SplineType.Monotonic:
                    double pointValue = (nextData.XValue - prevdata.XValue) / 3;
                    point = new ControlPoints(new ChartEventLocation(prevdata.XValue + pointValue, prevdata.YValue + (y_Spline1 * pointValue)), new ChartEventLocation(nextData.XValue - pointValue, nextData.YValue - (y_Spline2 * pointValue)));
                    break;
                default:
                    double deltaX2 = nextData.XValue - prevdata.XValue;
                    deltaX2 *= deltaX2;

                    bool shouldUseAbs = Series is not null &&
                        Math.Abs(Series.Renderer.YMin - Math.Floor(Series.Renderer.YMin)) > double.Epsilon &&
                        double.IsNegative(ONE_THIRD * ((2 * prevdata.YValue) + nextData.YValue - (ONE_THIRD * deltaX2 * (y_Spline1 + (0.5 * y_Spline2))))) &&
                        double.IsNegative(ONE_THIRD * (prevdata.YValue + (2 * nextData.YValue) - (ONE_THIRD * deltaX2 * ((0.5 * y_Spline1) + y_Spline2))));

                    point = shouldUseAbs
                        ? new ControlPoints(
                        new ChartEventLocation(Math.Abs(((2 * prevdata.XValue) + nextData.XValue) * ONE_THIRD), Math.Abs(ONE_THIRD * ((2 * prevdata.YValue) + nextData.YValue - (ONE_THIRD * deltaX2 * (y_Spline1 + (0.5 * y_Spline2)))))),
                        new ChartEventLocation(Math.Abs((prevdata.XValue + (2 * nextData.XValue)) * ONE_THIRD), Math.Abs(ONE_THIRD * (prevdata.YValue + (2 * nextData.YValue) - (ONE_THIRD * deltaX2 * ((0.5 * y_Spline1) + y_Spline2))))))
                        : new ControlPoints(
                        new ChartEventLocation(((2 * prevdata.XValue) + nextData.XValue) * ONE_THIRD, ONE_THIRD * ((2 * prevdata.YValue) + nextData.YValue - (ONE_THIRD * deltaX2 * (y_Spline1 + (0.5 * y_Spline2))))),
                        new ChartEventLocation((prevdata.XValue + (2 * nextData.XValue)) * ONE_THIRD, ONE_THIRD * (prevdata.YValue + (2 * nextData.YValue) - (ONE_THIRD * deltaX2 * ((0.5 * y_Spline1) + y_Spline2)))));

                    break;
            }

            return point;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Filters empty points according to series empty-point mode.
        /// </summary>
        /// <param name="series">Series instance.</param>
        /// <param name="seriesPoints">Optional list of points to filter; if null, will use series.Renderer.Points.</param>
        /// <returns>Filtered list of points.</returns>
        internal static List<Point> FilterEmptyPoints(ChartSeries series, [Optional] List<Point> seriesPoints)
        {
            List<Point> points = seriesPoints is not null ? seriesPoints : series.Renderer.Points ?? null!;

            if (series.EmptyPointSettings.Mode != EmptyPointMode.Drop)
            {
                return points;
            }

            for (int i = 0; i < points?.Count; i++)
            {
                points[i].Index = i;
                if (points[i].IsEmpty)
                {
                    if (series.Renderer.ChartPoints is { })
                    {
                        series.Renderer.ChartPoints[i].SymbolLocations = [];
                    }
                    points[i].SymbolLocations = [];
                    if (series.Renderer.ChartPoints is { })
                    {
                        series.Renderer.ChartPoints[i].Regions = [];
                    }
                    points[i].Regions = [];
                    points.RemoveRange(i, 1);
                    series.Renderer.ChartPoints?.RemoveRange(i, 1);
                    i--;
                }
            }

            return points ?? null!;
        }

        /// <summary>
        /// Populates spline control points for the current series.
        /// </summary>
        internal override void FindSplinePoint()
        {
            List<Point> points = FilterEmptyPoints(Series ?? null!);
            bool isNegativePoint = false;

            _splinePoints = FindSplineCoefficients(points, Series ?? null!);
            if (points.Count > 1)
            {
                DrawPoints = [];
                CreateDrawPoints(points, ref isNegativePoint);

                if (!isNegativePoint && Series?.Renderer.YMin < 0)
                {
                    Series.Renderer.YMin = 0;
                }
            }
        }

        /// <summary>
        /// Builds the SVG path direction for a spline segment (cartesian).
        /// </summary>
        internal virtual string GetSplineDirection(ControlPoints data, Point firstPoint, Point point, bool isInverted, ChartSeries series, string startPoint)
        {
            ChartEventLocation pt1 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(firstPoint.XValue), YAxisRenderer.GetPointValue(firstPoint.YValue), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
            ChartEventLocation pt2 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(point.XValue), YAxisRenderer.GetPointValue(point.YValue), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
            ChartEventLocation bpt1 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(data.ControlPoint1.X), YAxisRenderer.GetPointValue(data.ControlPoint1.Y), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
            ChartEventLocation bpt2 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(data.ControlPoint2.X), YAxisRenderer.GetPointValue(data.ControlPoint2.Y), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);

            if (Series is not null && (Series.Renderer.YAxisRenderer is LogarithmicAxisRenderer) && Math.Abs(Series.Renderer.YMin - Math.Floor(Series.Renderer.YMin)) > double.Epsilon)
            {
                bpt1.Y = Math.Abs(bpt1.Y) > YLength ? 1 : bpt1.Y;
                bpt2.Y = Math.Abs(bpt2.Y) > YLength ? 1 : bpt2.Y;
            }

            return startPoint + SPACE + pt1.X.ToString(Culture) + SPACE + pt1.Y.ToString(Culture)
                + SPACE + "C" + SPACE + bpt1.X.ToString(Culture) + SPACE + bpt1.Y.ToString(Culture)
                + SPACE + bpt2.X.ToString(Culture) + SPACE + bpt2.Y.ToString(Culture)
                + SPACE + pt2.X.ToString(Culture) + SPACE + pt2.Y.ToString(Culture) + SPACE;
        }

        /// <summary>
        /// Builds the polar spline path direction.
        /// </summary>
        internal virtual string GetPolarSplineDirection(ControlPoints data, Point firstPoint, Point point, bool isInverted, ChartSeries series, string startPoint)
        {
            ChartEventLocation pt1 = ChartHelper.TransformToVisible(firstPoint.XValue, firstPoint.YValue, XAxisRenderer.Axis ?? null!, YAxisRenderer.Axis ?? null!, series);
            ChartEventLocation pt2 = ChartHelper.TransformToVisible(point.XValue, point.YValue, XAxisRenderer.Axis ?? null!, YAxisRenderer.Axis ?? null!, series);
            ChartEventLocation bpt1 = ChartHelper.TransformToVisible(data.ControlPoint1.X, data.ControlPoint1.Y, XAxisRenderer.Axis ?? null!, YAxisRenderer.Axis ?? null!, series);
            ChartEventLocation bpt2 = ChartHelper.TransformToVisible(data.ControlPoint2.X, data.ControlPoint2.Y, XAxisRenderer.Axis ?? null!, YAxisRenderer.Axis ?? null!, series);

            return startPoint + SPACE + pt1.X.ToString(Culture) + SPACE + pt1.Y.ToString(Culture) + SPACE + "C"
                + SPACE + bpt1.X.ToString(Culture) + SPACE + bpt1.Y.ToString(Culture) + SPACE + bpt2.X.ToString(Culture)
                + SPACE + bpt2.Y.ToString(Culture) + SPACE + pt2.X.ToString(Culture) + SPACE + pt2.Y.ToString(Culture) + SPACE;
        }

        /// <summary>
        /// Builds area path direction for a Cartesian spline segment.
        /// </summary>
        internal virtual string GetSplineAreaDirection(ControlPoints data, ChartEventLocation pt1, bool isInverted, ChartSeries series)
        {
            ChartEventLocation bpt1 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(data.ControlPoint1.X), YAxisRenderer.GetPointValue(data.ControlPoint1.Y), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
            ChartEventLocation bpt2 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(data.ControlPoint2.X), YAxisRenderer.GetPointValue(data.ControlPoint2.Y), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
            return "C " + bpt1.X.ToString(Culture) + SPACE + bpt1.Y.ToString(Culture) + SPACE + bpt2.X.ToString(Culture) + SPACE + bpt2.Y.ToString(Culture) + SPACE + pt1.X.ToString(Culture) + SPACE + pt1.Y.ToString(Culture) + SPACE;
        }

        /// <summary>
        /// Builds area path direction for a polar spline segment.
        /// </summary>
        internal virtual string GetPolarSplineAreaDirection(ControlPoints data, ChartEventLocation pt1, bool isInverted, ChartSeries series)
        {
            ChartEventLocation bpt1 = ChartHelper.TransformToVisible(data.ControlPoint1.X, data.ControlPoint1.Y, XAxisRenderer.Axis ?? null!, YAxisRenderer.Axis ?? null!, series);
            ChartEventLocation bpt2 = ChartHelper.TransformToVisible(data.ControlPoint2.X, data.ControlPoint2.Y, XAxisRenderer.Axis ?? null!, YAxisRenderer.Axis ?? null!, series);
            return "C " + bpt1.X.ToString(Culture) + SPACE + bpt1.Y.ToString(Culture) + SPACE + bpt2.X.ToString(Culture) + SPACE + bpt2.Y.ToString(Culture) + SPACE + pt1.X.ToString(Culture) + SPACE + pt1.Y.ToString(Culture) + SPACE;
        }

        /// <summary>
        /// Returns next non-null index for empty-point handling.
        /// </summary>
        /// <param name="points">The point list.</param>
        /// <param name="i">Starting index.</param>
        /// <param name="series">Series instance.</param>
        /// <returns>Index of next valid point.</returns>
        internal static int GetNextIndex(List<Point> points, int i, ChartSeries series)
        {
            if (series.EmptyPointSettings.Mode != EmptyPointMode.Drop)
            {
                return i;
            }

            bool isNull = i <= -1 || i < points.Count || points[i] is null;
            while (isNull && i < points.Count)
            {
                i += 1;
            }

            return i;
        }
        #endregion
    }
}
