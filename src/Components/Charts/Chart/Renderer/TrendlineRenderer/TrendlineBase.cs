using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Encapsulates logic to compute and render trendline series for chart series.
    /// </summary>
    /// <remarks>
    /// This class handles the creation, configuration, and data computation for various trendline types
    /// including Linear, Exponential, Moving Average, Polynomial, Power, and Logarithmic trendlines.
    /// </remarks>
    internal class TrendlineBase : IDisposable
    {
        #region Fields
        private ChartSeries? _series;
        private SfChart? _chart;
        private List<Point>? _points;
        private ChartSeries? _trendLineSeries;
        private double?[]? _polynomialSlopes;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the trendline configuration for the current parent series.
        /// </summary>
        /// <value>A <see cref="ChartTrendline"/> instance containing trendline settings, or <see langword="null"/> if not configured.</value>
        internal ChartTrendline? Trendline { get; set; }

        /// <summary>
        /// Gets or sets the Syncfusion service for accessing global animation settings.
        /// </summary>
        /// <value>The injected <see cref="SyncfusionBlazorService"/>, or <see langword="null"/> if not available.</value>
        [Inject]
        internal SyncfusionBlazorService? SyncfusionService { get; set; }
        #endregion

        #region Private Methods

        /// <summary>
        /// Computes the Y value for a polynomial at a given X coordinate using precomputed coefficients.
        /// </summary>
        /// <param name="slopes">Array of polynomial coefficients (a0 + a1*x + a2*x² + ...).</param>
        /// <param name="x">The X coordinate at which to evaluate the polynomial.</param>
        /// <returns>The computed Y value.</returns>
        /// <remarks>Iterates through coefficients and applies Horner's method conceptually by computing power terms.</remarks>
        private static double GetPolynomialYValue(double?[] slopes, double x)
        {
            double sum = 0;
            for (int index = 0; index < slopes.Length; index++)
            {
                sum += (double)(slopes[index] ?? 0) * Math.Pow(x, index);
            }

            return sum;
        }

        /// <summary>
        /// Performs Gauss-Jordan elimination to solve a system of linear equations.
        /// </summary>
        /// <param name="matrix">The augmented matrix to reduce.</param>
        /// <param name="polynomialSlopes">The solution vector (right-hand side of equations).</param>
        /// <returns><see langword="true"/> if elimination succeeded; <see langword="false"/> if the matrix is singular.</returns>
        /// <remarks>
        /// This method modifies the <paramref name="matrix"/> and <paramref name="polynomialSlopes"/> in-place
        /// to compute polynomial coefficients for trendline fitting.
        /// </remarks>
        private static bool GaussJordanElimination(double?[][] matrix, double?[] polynomialSlopes)
        {
            int length = matrix.Length;
            double?[] rowPivotRecord = new double?[length];
            double?[] columnPivotRecord = new double?[length];
            double?[] pivotUsage = new double?[length];

            ResetPivotUsage(pivotUsage);

            for (int iteration = 0; iteration < length; iteration++)
            {
                (int pivotRow, int pivotColumn) = SelectPivot(matrix, pivotUsage);

                pivotUsage[pivotColumn] = (pivotUsage[pivotColumn] ?? 0) + 1;

                SwapPivotRows(matrix, polynomialSlopes, pivotRow, pivotColumn);

                rowPivotRecord[iteration] = pivotRow;
                columnPivotRecord[iteration] = pivotColumn;

                if (matrix[pivotColumn][pivotColumn] == 0.0)
                {
                    return false;
                }

                if (!NormalizePivotRow(matrix, polynomialSlopes, pivotColumn))
                {
                    return false;
                }

                EliminateOtherRows(matrix, polynomialSlopes, pivotColumn);
            }

            RestorePermutation(matrix, rowPivotRecord, columnPivotRecord);
            return true;
        }

        /// <summary>
        /// Resets the pivot usage tracking array to zero.
        /// </summary>
        /// <param name="pivotUsage">Array to reset.</param>
        private static void ResetPivotUsage(double?[] pivotUsage)
        {
            for (int i = 0; i < pivotUsage.Length; i++)
            {
                pivotUsage[i] = 0;
            }
        }

        /// <summary>
        /// Selects the pivot element with the largest absolute value (partial pivoting).
        /// </summary>
        /// <param name="matrix">The matrix being reduced.</param>
        /// <param name="pivotUsage">Tracks which rows/columns have been used as pivots.</param>
        /// <returns>A tuple of (pivotRow, pivotColumn) for the selected pivot.</returns>
        private static (int pivotRow, int pivotColumn) SelectPivot(double?[][] matrix, double?[] pivotUsage)
        {
            int length = matrix.Length;
            double maxAbs = 0.0;
            int selectedRow = 0;
            int selectedColumn = 0;

            for (int row = 0; row < length; row++)
            {
                if (pivotUsage[row] == 1)
                {
                    continue;
                }

                for (int col = 0; col < length; col++)
                {
                    if (pivotUsage[col] == 0)
                    {
                        double value = Math.Abs((double)(matrix[row][col] ?? 0));
                        if (value >= maxAbs)
                        {
                            maxAbs = value;
                            selectedRow = row;
                            selectedColumn = col;
                        }
                    }
                }
            }

            return (selectedRow, selectedColumn);
        }

        /// <summary>
        /// Swaps the pivot row and column during Gauss-Jordan elimination.
        /// </summary>
        /// <param name="matrix">The matrix being reduced.</param>
        /// <param name="polynomialSlopes">The solution vector.</param>
        /// <param name="pivotRow">The row to swap.</param>
        /// <param name="pivotColumn">The column to swap.</param>
        private static void SwapPivotRows(double?[][] matrix, double?[] polynomialSlopes, int pivotRow, int pivotColumn)
        {
            if (pivotRow == pivotColumn)
            {
                return;
            }

            (matrix[pivotColumn], matrix[pivotRow]) = (matrix[pivotRow], matrix[pivotColumn]);
            (polynomialSlopes[pivotColumn], polynomialSlopes[pivotRow]) = (polynomialSlopes[pivotRow], polynomialSlopes[pivotColumn]);
        }

        /// <summary>
        /// Normalizes the pivot row by scaling it such that the pivot element becomes 1.
        /// </summary>
        /// <param name="matrix">The matrix being reduced.</param>
        /// <param name="polynomialSlopes">The solution vector.</param>
        /// <param name="pivotColumn">The column to normalize.</param>
        /// <returns><see langword="true"/> if normalization succeeded; <see langword="false"/> otherwise.</returns>
        private static bool NormalizePivotRow(double?[][] matrix, double?[] polynomialSlopes, int pivotColumn)
        {
            double scale = Convert.ToDouble(1.0 / matrix[pivotColumn][pivotColumn], provider: null);

            matrix[pivotColumn][pivotColumn] = 1.0;

            int length = matrix.Length;
            for (int col = 0; col < length; col++)
            {
                matrix[pivotColumn][col] *= scale;
            }

            polynomialSlopes[pivotColumn] *= scale;
            return true;
        }

        /// <summary>
        /// Eliminates all non-pivot elements in the pivot column.
        /// </summary>
        /// <param name="matrix">The matrix being reduced.</param>
        /// <param name="polynomialSlopes">The solution vector.</param>
        /// <param name="pivotColumn">The column to eliminate.</param>
        private static void EliminateOtherRows(double?[][] matrix, double?[] polynomialSlopes, int pivotColumn)
        {
            int length = matrix.Length;

            for (int row = 0; row < length; row++)
            {
                if (row == pivotColumn)
                {
                    continue;
                }

                double factor = (double)(matrix[row][pivotColumn] ?? 0);
                matrix[row][pivotColumn] = 0.0;

                for (int col = 0; col < length; col++)
                {
                    matrix[row][col] -= matrix[pivotColumn][col] * factor;
                }

                polynomialSlopes[row] -= polynomialSlopes[pivotColumn] * factor;
            }
        }

        /// <summary>
        /// Restores row and column permutations after Gauss-Jordan elimination.
        /// </summary>
        /// <param name="matrix">The matrix to restore (modified in-place).</param>
        /// <param name="rowPivotRecord">Record of row swaps.</param>
        /// <param name="columnPivotRecord">Record of column swaps.</param>
        private static void RestorePermutation(double?[][] matrix, double?[] rowPivotRecord, double?[] columnPivotRecord)
        {
            for (int i = rowPivotRecord.Length - 1; i >= 0; i--)
            {
                int rowIndex = (int)(rowPivotRecord[i] ?? 0);
                int columnIndex = (int)(columnPivotRecord[i] ?? 0);

                if (rowIndex == columnIndex)
                {
                    continue;
                }

                for (int r = 0; r < matrix.Length; r++)
                {
                    (matrix[r][columnIndex], matrix[r][rowIndex]) = (matrix[r][rowIndex], matrix[r][columnIndex]);
                }
            }
        }

        /// <summary>
        /// Initializes instance references from Trendline parent hierarchy.
        /// </summary>
        /// <remarks>
        /// Extracts <see cref="ChartSeries"/> and <see cref="SfChart"/> references from the trendline's parent configuration.
        /// </remarks>
        private void InitPriavteInstances()
        {
            _series = Trendline?.Parent?.Series;
            _chart = _series?.Container;
        }

        /// <summary>
        /// Applies visual and accessibility properties from Trendline configuration to the internal trend series.
        /// </summary>
        private void SetSeriesProperties()
        {
            if (_trendLineSeries is null)
            {
                return;
            }

            string fill = !string.IsNullOrEmpty(Trendline?.Fill) ? Trendline.Fill : "blue";
            LegendShape legendShape = LegendShape.HorizontalLine;
            ChartSeriesBorder border = new() { };

            _trendLineSeries.SetTrendlineValues
            (
                Trendline?.Name ?? string.Empty,
                "x",
                "y",
                Trendline?.DashArray ?? string.Empty,
                Trendline?.Width ?? 0, fill,
                legendShape,
                Trendline is not null && Trendline.EnableTooltip,
                border,
                Trendline?.AccessibilityDescription ?? string.Empty,
                Trendline?.AccessibilityDescriptionFormat ?? string.Empty,
                Trendline?.AccessibilityRole ?? string.Empty,
                Trendline is not null && Trendline.Focusable
            );

            _trendLineSeries.Container = _chart;
            _trendLineSeries.RendererKey += Trendline?.RendererKey;
            if (Trendline is { })
            {
                Trendline.TargetSeries = _trendLineSeries;
            }
        }

        /// <summary>
        /// Computes and sets linear trendline points including forecast values.
        /// </summary>
        private void SetLinearRange()
        {
            List<double> xValues = [];
            List<double> yValues = [];

            for (int index = 0; index < _points?.Count; index++)
            {
                xValues.Add(_points[index].XValue);
                yValues.Add(_points[index].YValue);
            }
            if (_trendLineSeries is { })
            {
                _trendLineSeries.Renderer.Points = GetLinearPoints(xValues, FindSlopeIntercept(xValues, yValues));
            }
        }

        /// <summary>
        /// Computes linear trendline points with forecast extensions.
        /// </summary>
        /// <param name="xValues">The X coordinates of the data points.</param>
        /// <param name="slopeInterceptLinear">The computed slope and intercept values.</param>
        /// <returns>A list of computed trendline points.</returns>
        private List<Point> GetLinearPoints(List<double> xValues, SlopeIntercept slopeInterceptLinear)
        {
            List<Point> pts = [];
            int max = xValues.IndexOf(xValues.Max());
            int min = xValues.IndexOf(xValues.Min());
            double x1Linear = (xValues[min] - Trendline?.BackwardForecast) ?? 0;
            double x2Linear = (xValues[max] + Trendline?.ForwardForecast) ?? 0;

            pts.Add(GetDataPoint(x1Linear, (slopeInterceptLinear.Slope * x1Linear) + slopeInterceptLinear.Intercept, pts.Count));
            pts.Add(GetDataPoint(x2Linear, (slopeInterceptLinear.Slope * x2Linear) + slopeInterceptLinear.Intercept, pts.Count));
            return pts;
        }

        /// <summary>
        /// Computes and sets exponential trendline points including forecast values.
        /// </summary>
        private void SetExponentialRange()
        {
            List<double> x_Value = [];
            List<double> y_Value = [];
            for (int index = 0; index < _points?.Count; index++)
            {
                x_Value.Add(_points[index].XValue);
                y_Value.Add(_points[index].YValue != 0 ? Math.Log(_points[index].YValue) : 0);
            }
            if (_trendLineSeries is { })
            {
                _trendLineSeries.Renderer.Points = GetExponentialPoints(x_Value, FindSlopeIntercept(x_Value, y_Value));
            }
        }

        /// <summary>
        /// Computes exponential trendline points with forecast extensions.
        /// </summary>
        /// <param name="xValues">The X coordinates of the data points.</param>
        /// <param name="slopeIntercept">The computed slope and intercept values.</param>
        /// <returns>A list of computed exponential trendline points.</returns>
        private List<Point> GetExponentialPoints(List<double> xValues, SlopeIntercept slopeIntercept)
        {
            int midPoint = Convert.ToInt32(Math.Round((double)(_points?.Count ?? 0) / 2));
            List<Point> pts = [];

            double x1 = (xValues[0] - Trendline?.BackwardForecast) ?? 0;
            double x2 = xValues[midPoint - 1];
            double x3 = (xValues[^1] + Trendline?.ForwardForecast) ?? 0;

            pts.Add(GetDataPoint(x1, slopeIntercept.Intercept * Math.Exp(slopeIntercept.Slope * x1), pts.Count));
            pts.Add(GetDataPoint(x2, slopeIntercept.Intercept * Math.Exp(slopeIntercept.Slope * x2), pts.Count));
            pts.Add(GetDataPoint(x3, slopeIntercept.Intercept * Math.Exp(slopeIntercept.Slope * x3), pts.Count));
            return pts;
        }

        /// <summary>
        /// Computes and sets moving average trendline points.
        /// </summary>
        private void SetMovingAverageRange()
        {
            List<double> xValues = [];
            List<double> yValues = [];
            List<double> xAvgValues = [];

            for (int index = 0; index < _points?.Count; index++)
            {
                xAvgValues.Add(_points[index].XValue);
                xValues.Add(index + 1);
                yValues.Add(_points[index].YValue);
            }

            if (_trendLineSeries is { })
            {
                _trendLineSeries.Renderer.Points = GetMovingAveragePoints(xAvgValues, yValues);
            }
        }

        /// <summary>
        /// Computes moving average trendline points.
        /// </summary>
        /// <param name="xValues">The X coordinates of the data points.</param>
        /// <param name="yValues">The Y values of the data points.</param>
        /// <returns>A list of moving average trendline points.</returns>
        private List<Point> GetMovingAveragePoints(List<double> xValues, List<double> yValues)
        {
            List<Point> pts = [];
            double period = Math.Max(2, Trendline?.Period >= _points?.Count ? _points.Count - 1 : Trendline?.Period ?? 0);
            int index = 0;

            while ((period + index) <= _points?.Count)
            {
                double average = 0, count = 0, nullCount = 0;
                for (int j = index; count < period; j++)
                {
                    count++;
                    if (yValues[j] == 0 || double.IsNaN(yValues[j]))
                    {
                        nullCount++;
                    }

                    average += yValues[j];
                }

                average = period - nullCount <= 0 ? double.NaN : average / (period - nullCount);
                if (average != 0 && !double.IsNaN(average))
                {
                    pts.Add(GetDataPoint(xValues[Convert.ToInt32(period - 1 + index)], average, pts.Count));
                }

                index++;
            }

            return pts;
        }

        /// <summary>
        /// Computes and sets polynomial trendline points.
        /// </summary>
        private void SetPolynomialRange()
        {
            List<double> xPolyValues = [];
            List<double> yPolyValues = [];
            for (int index = 0; index < _points?.Count; index++)
            {
                xPolyValues.Add(_points[index].XValue);
                yPolyValues.Add(_points[index].YValue);
            }

            if (_trendLineSeries is { })
            {
                _trendLineSeries.Renderer.Points = GetPolynomialPoints(xPolyValues, yPolyValues);
            }
        }

        /// <summary>
        /// Creates a data point for the trendline.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="index">The index of the point in the series.</param>
        /// <returns>A configured <see cref="Point"/> instance.</returns>
        private Point GetDataPoint(double x, double y, int index)
        {
            IChartPoint chartPoint = new();
            Point trendPoint = new()
            {
                X = chartPoint.X = x,
                Y = chartPoint.Y = y,
                XValue = chartPoint.XValue = x,
                Interior = chartPoint.Interior = _trendLineSeries?.Fill ?? string.Empty,
                Index = chartPoint.Index = index,
                YValue = chartPoint.YValue = y,
                Visible = true
            };

            if (_trendLineSeries is not null)
            {
                _trendLineSeries.Renderer.XMin = Math.Min(_trendLineSeries.Renderer.XMin, trendPoint.XValue);
                _trendLineSeries.Renderer.YMin = Math.Min(_trendLineSeries.Renderer.YMin, trendPoint.YValue);
                _trendLineSeries.Renderer.XMax = Math.Max(_trendLineSeries.Renderer.XMax, trendPoint.XValue);
                _trendLineSeries.Renderer.YMax = Math.Max(_trendLineSeries.Renderer.YMax, trendPoint.YValue);
                _trendLineSeries.Renderer.XData.Add(trendPoint.XValue);
                _trendLineSeries.Renderer.ChartPoints?.Add(chartPoint);
            }
            return trendPoint;
        }

        /// <summary>
        /// Computes slope and intercept for linear and exponential trendlines.
        /// </summary>
        /// <param name="xValues">The X coordinates of the data points.</param>
        /// <param name="yValues">The Y values of the data points.</param>
        /// <returns>A <see cref="SlopeIntercept"/> containing computed slope and intercept.</returns>
        private SlopeIntercept FindSlopeIntercept(List<double> xValues, List<double> yValues)
        {
            double xAvg = 0, yAvg = 0, xyAvg = 0, xxAvg = 0, slope, intercept;
            for (int index = 0; index < _points?.Count; index++)
            {
                if (double.IsNaN(yValues[index]))
                {
                    yValues[index] = (yValues[index - 1] + yValues[index + 1]) / 2;
                }

                xAvg += xValues[index];
                yAvg += yValues[index];
                xyAvg += xValues[index] * yValues[index];
                xxAvg += xValues[index] * xValues[index];
            }

            if (Trendline?.Intercept != 0 && !double.IsNaN(Trendline?.Intercept ?? 0) && (Trendline?.Type == TrendlineTypes.Linear || Trendline?.Type == TrendlineTypes.Exponential))
            {
                intercept = Trendline.Intercept;
                slope = ComputeSlopeWithIntercept(xAvg, xyAvg, xxAvg);
            }
            else
            {
                slope = (((_points?.Count ?? 0) * xyAvg) - (xAvg * yAvg)) / (((_points?.Count ?? 0) * xxAvg) - (xAvg * xAvg));
                slope = Trendline?.Type == TrendlineTypes.Linear ? slope : Math.Abs(slope);
                intercept = ComputeIntercept(xAvg, yAvg, slope);
            }

            return new SlopeIntercept { Slope = slope, Intercept = intercept };
        }

        /// <summary>
        /// Computes slope when intercept is manually specified.
        /// </summary>
        /// <param name="xAvg">Average of X values.</param>
        /// <param name="xyAvg">Average of X*Y products.</param>
        /// <param name="xxAvg">Average of X² values.</param>
        /// <returns>The computed slope.</returns>
        private double ComputeSlopeWithIntercept(double xAvg, double xyAvg, double xxAvg)
        {
            double slope = 0;
            switch (Trendline?.Type)
            {
                case TrendlineTypes.Linear:
                    slope = (xyAvg - (Trendline.Intercept * xAvg)) / xxAvg;
                    break;
                case TrendlineTypes.Exponential:
                    slope = (xyAvg - (Math.Log(Math.Abs(Trendline.Intercept)) * xAvg)) / xxAvg;
                    break;
                case TrendlineTypes.Polynomial:
                case TrendlineTypes.Power:
                case TrendlineTypes.Logarithmic:
                case TrendlineTypes.MovingAverage:
                case null:
                    break;
                default:
                    break;
            }
            return slope;
        }

        /// <summary>
        /// Computes intercept based on slope and data averages.
        /// </summary>
        /// <param name="xAvg">Average of X values.</param>
        /// <param name="yAvg">Average of Y values.</param>
        /// <param name="slope">The computed slope.</param>
        /// <returns>The computed intercept.</returns>
        private double ComputeIntercept(double xAvg, double yAvg, double slope)
        {
            return (Trendline?.Type is TrendlineTypes.Exponential or TrendlineTypes.Power)
                ? Math.Exp((yAvg - (slope * xAvg)) / (_points?.Count ?? 0)) : (yAvg - (slope * xAvg)) / (_points?.Count ?? 0);
        }

        /// <summary>
        /// Computes polynomial trendline points.
        /// </summary>
        /// <param name="xValues">The X coordinates of the data points.</param>
        /// <param name="yValues">The Y values of the data points.</param>
        /// <returns>A list of polynomial trendline points.</returns>
        private List<Point> GetPolynomialPoints(List<double> xValues, List<double> yValues)
        {
            double? tlOrderNullable = Trendline?.PolynomialOrder;
            double tlOrderRaw = tlOrderNullable ?? 0d;
            double orderForTrendline = DetermineOrderForTrendlineSetter(_points?.Count, tlOrderNullable);

            ChartTrendline chartTrendline = new();
            chartTrendline.PolynomialOrderValue(orderForTrendline);
            int slopesLen = Convert.ToInt32(tlOrderRaw + 1);
            _polynomialSlopes = new double?[slopesLen];
            int orderFloor = tlOrderNullable.HasValue ? (int)Math.Floor(tlOrderNullable.Value) : -1;

            if (tlOrderNullable.HasValue)
            {
                AccumulatePolynomialSlopes(xValues, yValues, orderFloor, _polynomialSlopes);
            }

            double?[][] coefficientMatrix = BuildCoefficientMatrix(xValues, tlOrderNullable, orderFloor);

            if (!GaussJordanElimination(coefficientMatrix, _polynomialSlopes))
            {
                _polynomialSlopes = null;
            }

            return GetPoints(xValues);
        }

        /// <summary>
        /// Determines the polynomial order for fitting, clamped between 2 and 6.
        /// </summary>
        /// <param name="pointsCount">The number of data points.</param>
        /// <param name="tlOrderNullable">The requested polynomial order, or <see langword="null"/>.</param>
        /// <returns>The effective polynomial order.</returns>
        private static double DetermineOrderForTrendlineSetter(int? pointsCount, double? tlOrderNullable)
        {
            double selectedOrder = pointsCount.HasValue && tlOrderNullable.HasValue && pointsCount.Value <= tlOrderNullable.Value
                ? pointsCount.Value
                : tlOrderNullable ?? 0d;
            if (selectedOrder < 2)
            {
                selectedOrder = 2;
            }

            if (selectedOrder > 6)
            {
                selectedOrder = 6;
            }

            return selectedOrder;
        }

        /// <summary>
        /// Accumulates polynomial slope coefficients from data points.
        /// </summary>
        /// <param name="xValues">The X coordinates of the data points.</param>
        /// <param name="yValues">The Y values of the data points.</param>
        /// <param name="orderFloor">The polynomial order (floor value).</param>
        /// <param name="polynomialSlopes">Array to accumulate the coefficients (modified in-place).</param>
        private static void AccumulatePolynomialSlopes(List<double> xValues, List<double> yValues, int orderFloor, double?[] polynomialSlopes)
        {
            for (int i = 0; i < xValues.Count; i++)
            {
                double x = xValues[i];
                double y = yValues[i];

                double xPow = 1d;
                for (int j = 0; j <= orderFloor; j++)
                {
                    if (!polynomialSlopes[j].HasValue)
                    {
                        polynomialSlopes[j] = 0d;
                    }

                    polynomialSlopes[j] += xPow * y;
                    xPow *= x;
                }
            }
        }

        /// <summary>
        /// Builds the coefficient matrix for polynomial least-squares fitting.
        /// </summary>
        /// <param name="xValues">The X coordinates of the data points.</param>
        /// <param name="tlOrderNullable">The polynomial order, or <see langword="null"/>.</param>
        /// <param name="orderFloor">The polynomial order (floor value).</param>
        /// <returns>A coefficient matrix for use in Gauss-Jordan elimination.</returns>
        private static double?[][] BuildCoefficientMatrix(List<double> xValues, double? tlOrderNullable, int orderFloor)
        {
            double tlOrderRaw = tlOrderNullable ?? 0d;
            int len = Convert.ToInt32(1 + (2 * tlOrderRaw));
            double?[] powerSums = CalculatePowerSums(xValues, len);
            int size = Convert.ToInt32(tlOrderRaw + 1);
            double?[][] matrix = new double?[size][];
            for (int i = 0; i < size; i++)
            {
                matrix[i] = new double?[size];
            }

            if (tlOrderNullable.HasValue)
            {
                for (int i = 0; i <= orderFloor; i++)
                {
                    for (int j = 0; j <= orderFloor; j++)
                    {
                        matrix[i][j] = powerSums[i + j];
                    }
                }
            }

            return matrix;
        }

        /// <summary>
        /// Calculates cumulative power sums for X values (used in polynomial fitting).
        /// </summary>
        /// <param name="xValues">The X coordinates of the data points.</param>
        /// <param name="len">The maximum power to compute.</param>
        /// <returns>An array of power sums [Σ1, Σx, Σx², ..., Σx^(len-1)].</returns>
        private static double?[] CalculatePowerSums(List<double> xValues, int len)
        {
            double?[] sums = new double?[len];

            for (int i = 0; i < xValues.Count; i++)
            {
                double x = xValues[i];
                double power = 1.0;

                for (int k = 0; k < len; k++)
                {
                    if (!sums[k].HasValue)
                    {
                        sums[k] = 0d;
                    }

                    sums[k] += power;
                    power *= x;
                }
            }

            return sums;
        }

        /// <summary>
        /// Computes polynomial trendline points with forecast extensions.
        /// </summary>
        /// <param name="xValues">The X coordinates of the data points.</param>
        /// <returns>A list of computed polynomial trendline points.</returns>
        private List<Point> GetPoints(List<double> xValues)
        {
            List<Point> pts = [];
            double xInterpolated = 1;
            for (int i = 1; i <= _polynomialSlopes?.Length; i++)
            {
                double xValue, yValue;

                if (i == 1)
                {
                    xValue = (xValues[0] - Trendline?.BackwardForecast) ?? 0;
                    yValue = GetPolynomialYValue(_polynomialSlopes, xValue);
                }
                else if (i == _polynomialSlopes.Length)
                {
                    xValue = (xValues[(_points?.Count ?? 0) - 1] + Trendline?.ForwardForecast) ?? 0;
                    yValue = GetPolynomialYValue(_polynomialSlopes, xValue);
                }
                else
                {
                    double forecast = _trendLineSeries?.Renderer.XAxisRenderer is DateTimeAxisRenderer ? 0 : Trendline?.ForwardForecast ?? 0;
                    xInterpolated += ((_points?.Count ?? 0) + forecast) / _polynomialSlopes.Length;
                    xValue = xValues[Convert.ToInt32(Math.Round(xInterpolated)) - 1];
                    yValue = GetPolynomialYValue(_polynomialSlopes, xValue);
                    pts.Add(GetDataPoint(xValue, yValue, pts.Count));
                }
                pts.Add(GetDataPoint(xValue, yValue, pts.Count));
            }

            return pts;
        }

        /// <summary>
        /// Computes and sets power trendline points.
        /// </summary>
        private void SetPowerRange()
        {
            List<double> xValues = [];
            List<double> yValues = [];
            List<double> powerPoints = [];

            for (int index = 0; index < _points?.Count; index++)
            {
                powerPoints.Add(_points[index].XValue);
                xValues.Add(_points[index].XValue != 0 && !double.IsNaN(_points[index].XValue) ? Math.Log(_points[index].XValue) : 0);
                yValues.Add(_points[index].YValue != 0 && !double.IsNaN(_points[index].YValue) ? Math.Log(_points[index].YValue) : 0);
            }
            if (_trendLineSeries is { })
            {
                _trendLineSeries.Renderer.Points = GetPowerPoints(powerPoints, FindSlopeIntercept(xValues, yValues));
            }
        }

        /// <summary>
        /// Computes power trendline points with forecast extensions.
        /// </summary>
        /// <param name="xValues">The X coordinates of the data points.</param>
        /// <param name="slopeIntercept">The computed slope and intercept values.</param>
        /// <returns>A list of computed power trendline points.</returns>
        private List<Point> GetPowerPoints(List<double> xValues, SlopeIntercept slopeIntercept)
        {
            int midPoint = Convert.ToInt32(Math.Round((double)((_points?.Count ?? 0) / 2)));
            List<Point> pts = [];

            double x1 = (xValues[0] - Trendline?.BackwardForecast) ?? 0;
            x1 = x1 > -1 ? x1 : 0;

            double x2 = xValues[midPoint - 1];
            double x3 = (xValues[^1] + Trendline?.ForwardForecast) ?? 0;

            pts.Add(GetDataPoint(x1, slopeIntercept.Intercept * Math.Pow(x1, slopeIntercept.Slope), pts.Count));
            pts.Add(GetDataPoint(x2, slopeIntercept.Intercept * Math.Pow(x2, slopeIntercept.Slope), pts.Count));
            pts.Add(GetDataPoint(x3, slopeIntercept.Intercept * Math.Pow(x3, slopeIntercept.Slope), pts.Count));
            return pts;
        }

        /// <summary>
        /// Computes and sets logarithmic trendline points.
        /// </summary>
        private void SetLogarithmicRange()
        {
            List<double> x_LogValue = [];
            List<double> y_LogValue = [];
            List<double> x_PointsLgr = [];

            for (int index = 0; index < _points?.Count; index++)
            {
                x_PointsLgr.Add(_points[index].XValue);
                x_LogValue.Add(_points[index].XValue != 0 && !double.IsNaN(_points[index].XValue) ? Math.Log(_points[index].XValue) : 0);
                y_LogValue.Add(_points[index].YValue);
            }

            if (_trendLineSeries is { })
            {
                _trendLineSeries.Renderer.Points = GetLogarithmicPoints(x_PointsLgr, FindSlopeIntercept(x_LogValue, y_LogValue));
            }
        }

        /// <summary>
        /// Computes logarithmic trendline points with forecast extensions.
        /// </summary>
        /// <param name="xValues">The X coordinates of the data points.</param>
        /// <param name="slopeIntercept">The computed slope and intercept values.</param>
        /// <returns>A list of computed logarithmic trendline points.</returns>
        private List<Point> GetLogarithmicPoints(List<double> xValues, SlopeIntercept slopeIntercept)
        {
            int midPoint = Convert.ToInt32(Math.Round((double)((_points?.Count ?? 0) / 2)));
            List<Point> pts = [];

            double x1 = (xValues[0] - Trendline?.BackwardForecast) ?? 0;
            double y1 = slopeIntercept.Intercept + (slopeIntercept.Slope * (x1 != 0 && !double.IsNaN(x1) ? Math.Log(x1) : 0));

            double x2 = xValues[midPoint - 1];
            double y2 = slopeIntercept.Intercept + (slopeIntercept.Slope * (x2 != 0 && !double.IsNaN(x2) ? Math.Log(x2) : 0));

            double x3 = (xValues[^1] + Trendline?.ForwardForecast) ?? 0;
            double y3 = slopeIntercept.Intercept + (slopeIntercept.Slope * (x3 != 0 && !double.IsNaN(x3) ? Math.Log(x3) : 0));

            pts.Add(GetDataPoint(x1, y1, pts.Count));
            pts.Add(GetDataPoint(x2, y2, pts.Count));
            pts.Add(GetDataPoint(x3, y3, pts.Count));
            return pts;
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Creates and configures the internal series used to render the trendline.
        /// </summary>
        /// <remarks>
        /// Initializes a <see cref="ChartSeries"/> and assigns the appropriate renderer type
        /// (Line for Linear/MovingAverage, Spline for others).
        /// </remarks>
        internal void InitSeriesCollection()
        {
            InitPriavteInstances();
            _trendLineSeries ??= new ChartSeries();

            if (Trendline?.Type is TrendlineTypes.Linear or TrendlineTypes.MovingAverage)
            {
                _trendLineSeries.SetTrendlineType(ChartSeriesType.Line);
                _trendLineSeries.RendererType = typeof(Trendline_LineSeriesRenderer);
            }
            else
            {
                _trendLineSeries.SetTrendlineType(ChartSeriesType.Spline);
                _trendLineSeries.RendererType = typeof(Trendline_SplineSeriesRenderer);
            }

            SetSeriesProperties();
        }

        /// <summary>
        /// Updates marker settings on the internal trendline series.
        /// </summary>
        internal void UpdateTrendlineMarker()
        {
            ChartMarker marker = new();
            marker.SetMarkerValues(Trendline?.Marker ?? null!);
            _trendLineSeries?.UpdateSeriesProperties("Marker", marker);
        }

        /// <summary>
        /// Configures animation on the trendline series respecting global service settings.
        /// </summary>
        internal void UpdateTrendlineAnimation()
        {
            ChartDefaultAnimation animation = new();
            bool enableAnimation = SyncfusionService is not null ? (SyncfusionService._options.Animation == GlobalAnimationMode.Enable || (SyncfusionService._options.Animation == GlobalAnimationMode.Default && Trendline is not null && Trendline.Animation.Enable)) : Trendline is not null && Trendline.Animation.Enable;
            animation.SetTrendlineAnimation(enableAnimation, Trendline?.Animation.Duration ?? 1000, Trendline?.Animation.Delay ?? 0);
        }

        /// <summary>
        /// Prepares the trendline data points based on the configured trendline type.
        /// </summary>
        internal void InitDataSource()
        {
            _trendLineSeries?.Renderer.InitSeriesRendererFields();
            _points = _series?.Renderer.Points;

            if (_points is not null && _points.Count > 0)
            {
                switch (Trendline?.Type)
                {
                    case TrendlineTypes.Linear:
                        SetLinearRange();
                        break;
                    case TrendlineTypes.Exponential:
                        SetExponentialRange();
                        break;
                    case TrendlineTypes.MovingAverage:
                        SetMovingAverageRange();
                        break;
                    case TrendlineTypes.Polynomial:
                        SetPolynomialRange();
                        break;
                    case TrendlineTypes.Power:
                        SetPowerRange();
                        break;
                    case TrendlineTypes.Logarithmic:
                        SetLogarithmicRange();
                        break;
                    default:
                        break;
                }
            }

            if (Trendline?.Type is not TrendlineTypes.Linear and not TrendlineTypes.MovingAverage)
            {
                _trendLineSeries?.Renderer.FindSplinePoint();
            }
        }

        /// <summary>
        /// Hooks the trendline series to the same axes as the parent series.
        /// </summary>
        internal void InitiateAxis()
        {
            ChartAxisRenderer x_AxisRender = _series?.Renderer.XAxisRenderer ?? null!;
            ChartAxisRenderer y_AxisRender = _series?.Renderer.YAxisRenderer ?? null!;
            ChartSeriesRenderer trendlineSeriesRenderer = _trendLineSeries?.Renderer ?? null!;

            trendlineSeriesRenderer.XAxisRenderer = x_AxisRender;
            trendlineSeriesRenderer.YAxisRenderer = y_AxisRender;
            x_AxisRender.SeriesRenderer.Add(trendlineSeriesRenderer);
            y_AxisRender.SeriesRenderer.Add(trendlineSeriesRenderer);
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Releases native resources associated with trendline helper.
        /// </summary>
        public void Dispose()
        {
            _series?.ComponentDispose();
            _trendLineSeries?.ComponentDispose();
            _chart = null;
            _points = null;
        }
        #endregion
    }
}
