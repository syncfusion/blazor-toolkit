using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Manages data point editing interactions in the chart, including dragging and repositioning.
    /// </summary>
    /// <remarks>
    /// Handles mouse events to enable users to drag chart data points and update their values.
    /// Supports min/max range constraints and axis-specific calculations.
    /// </remarks>
    public class DataEditing
    {
        #region Fields

        private double _dragY;
        private SfChart _chart = null!;
        private int? _seriesIndex;
        private int? _pointIndex;
        // Gets or sets a value indicating whether a point is currently being dragged.
        internal bool _isPointDragging;
        // Gets or sets a value indicating whether a point was dragged (completed action).
        internal bool _isPointDragged;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEditing"/> class.
        /// </summary>
        /// <param name="sfChart">The parent <see cref="SfChart"/> component.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="sfChart"/> is <see langword="null"/>.</exception>
        internal DataEditing(SfChart sfChart)
        {
            _chart = sfChart;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Updates the cursor style based on the current point and series configuration.
        /// </summary>
        /// <param name="pointData">The point data containing series and point information.</param>
        /// <param name="data">The chart data context.</param>
        /// <returns>A cursor style string (e.g., "ns-resize", "ew-resize", "null").</returns>
        private string UpdateCursorStyle(PointData pointData, ChartData data)
        {
            if (pointData.Series is not null && pointData.Series.ChartDataEditSettings?.Enable == true && pointData.Point is not null && (data.InsideRegion || !pointData.Series.Renderer.IsRectSeries()))
            {
                return GetCursorStyle(pointData);
            }

            return "null";
        }

        /// <summary>
        /// Invokes the <see cref="SfChart.OnDataEdit"/> event callback.
        /// </summary>
        /// <param name="seriesRenderer">The series renderer.</param>
        /// <param name="pointData">The point data being edited.</param>
        /// <returns><see langword="true"/> if the edit was canceled; otherwise <see langword="false"/>.</returns>
        private bool InvokeOnDataEdit(ChartSeriesRenderer seriesRenderer, PointData pointData)
        {
            if (_pointIndex is null)
            {
                return false;
            }
            DataEditingEventArgs editingEventArgs = new
            (
                "OnDataEdit",
                seriesRenderer.Points?[(int)_pointIndex].YValue ?? 0,
                seriesRenderer.YData.Count > 0 ? seriesRenderer.YData[(int)_pointIndex] : double.NaN,
                pointData.Point,
                (int)_pointIndex,
                pointData.Series ?? null!,
                _seriesIndex ?? 0
            );

            _chart.OnDataEdit?.Invoke(editingEventArgs);

            if (editingEventArgs.Cancel)
            {
                _chart.SetSvgCursor("null");
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines the appropriate cursor style based on the series type and chart orientation.
        /// </summary>
        /// <param name="pointData">The chart series.</param>
        /// <returns>A cursor style string (e.g., "ns-resize", "ew-resize").</returns>
        /// <remarks>
        /// Returns vertical resize for transposed bar charts; horizontal resize for standard charts.
        /// </remarks>
        private string GetCursorStyle(PointData pointData)
        {
            return pointData.Series.Type == ChartSeriesType.Bar && _chart.IsTransposed ? "ns-resize" : (_chart.IsTransposed || pointData.Series.Type == ChartSeriesType.Bar ? "ew-resize" : "ns-resize");
        }

        /// <summary>
        /// Updates the series render value with the current edited point data.
        /// </summary>
        /// <param name="seriesRenderer">The series renderer containing the points to update.</param>
        private void UpdateSeriesRenderValue(ChartSeriesRenderer seriesRenderer)
        {
            _chart.OnDataEditCompleted?.Invoke(new DataEditingEventArgs(
                    "OnDataEditCompleted",
                    seriesRenderer.Points?[_pointIndex ?? 0].YValue ?? 0,
                    seriesRenderer.YData.Count > 0 ? seriesRenderer.YData[_pointIndex ?? 0] : double.NaN,
                    seriesRenderer.Points?[_pointIndex ?? 0] ?? null!,
                    _pointIndex ?? 0,
                    seriesRenderer.Series!,
                    _seriesIndex ?? 0));
            if (seriesRenderer.Points is { })
            {
                seriesRenderer.Points[_pointIndex ?? 0].Y = seriesRenderer.Points[_pointIndex ?? 0].YValue;
            }
        }

        /// <summary>
        /// Performs the drag calculation and updates point values within constraints.
        /// </summary>
        /// <param name="seriesIndex">The index of the series being dragged.</param>
        /// <param name="pointIndex">The index of the point being dragged.</param>
        private void PointDragging(int seriesIndex, int pointIndex)
        {
            List<double> yValues = [];

            ChartSeriesRenderer seriesRenderer = _chart._seriesContainer is not null ? (ChartSeriesRenderer)_chart._seriesContainer.Renderers[seriesIndex] : null!;
            ChartDataEditSettings pointDrag = seriesRenderer?.Series?.ChartDataEditSettings ?? null!;
            ChartAxisRenderer yAxis = seriesRenderer?.YAxisRenderer ?? null!;
            int extra = (seriesRenderer is not null && seriesRenderer.IsRectSeries()) ? 1 : 0;
            Rect xRect = (seriesRenderer?.XAxisRenderer.Rect) ?? new Rect(0, 0, 0, 0);
            Rect axisRect = ChartHelper.GetTransform(xRect, yAxis.Rect, _chart._requireInvertedAxis);

            ChartSeriesType? seriesType = seriesRenderer?.Series?.Type;

            double yPosition = CalculateYPosition(seriesType, _chart.IsTransposed, axisRect, _chart._mouseX, _chart._mouseY);
            double ySize = CalculateYSize(seriesType, _chart.IsTransposed, axisRect);
            double rawInterpolated = InterpolateAxisValue(yAxis, yPosition, ySize);
            (double minRange, double maxRange) = CalculateMinMaxRange(yAxis, pointDrag, rawInterpolated, extra);

            if (maxRange >= rawInterpolated && minRange <= rawInterpolated)
            {
                _dragY = ComputeDragY(yAxis, rawInterpolated);

                if (seriesRenderer?.Points is not null)
                {
                    seriesRenderer.Points[pointIndex].YValue = _dragY;

                    if (seriesRenderer.ChartPoints is not null)
                    {
                        seriesRenderer.Points[pointIndex].Y =
                            seriesRenderer.ChartPoints[pointIndex].Y = _dragY;
                    }

                    seriesRenderer.Points[pointIndex].Interior = pointDrag.Fill;

                    for (int i = 0; i < seriesRenderer.Points.Count; i++)
                    {
                        yValues.Add(seriesRenderer.Points[i].YValue);
                    }
                }
            }

            if (seriesRenderer is not null)
            {
                seriesRenderer.YMin = yValues.Min();
                seriesRenderer.YMax = yValues.Max();
            }

            _isPointDragging = true;
            _chart.UpdateRenderers(true);
        }

        /// <summary>
        /// Calculates the Y position relative to axis rect depending on orientation and series type.
        /// </summary>
        private static double CalculateYPosition(ChartSeriesType? seriesType, bool isTransposed, Rect axisRect, double mouseX, double mouseY)
        {
            bool isBar = seriesType == ChartSeriesType.Bar;
            return isBar ? (isTransposed ? axisRect.Y + axisRect.Height - mouseY : mouseX - axisRect.X) : (isTransposed ? mouseX - axisRect.X : axisRect.Y + axisRect.Height - mouseY);
        }

        /// <summary>
        /// Calculates the axis length used for interpolation.
        /// </summary>
        private static double CalculateYSize(ChartSeriesType? seriesType, bool isTransposed, Rect axisRect)
        {
            bool isBar = seriesType == ChartSeriesType.Bar;

            return isBar ? isTransposed ? axisRect.Height : axisRect.Width : isTransposed ? axisRect.Width : axisRect.Height;
        }

        /// <summary>
        /// Interpolates a normalized axis value into the axis' visible range.
        /// </summary>
        private static double InterpolateAxisValue(ChartAxisRenderer yAxis, double yPosition, double ySize)
        {
            double normalized = (yAxis.Axis?.IsAxisInverse ?? false) ? 1 - (yPosition / ySize) : (yPosition / ySize);
            return (normalized * yAxis.VisibleRange.Delta) + yAxis.VisibleRange.Start;
        }

        /// <summary>
        /// Computes the numeric dragged Y value, preserving logarithmic behavior or rounding for linear axes.
        /// </summary>
        private static double ComputeDragY(ChartAxisRenderer yAxis, double rawInterpolated)
        {
            bool isLog = yAxis.Axis?.ValueType == ValueType.Logarithmic;
            if (isLog)
            {
                double baseVal = yAxis.Axis!.LogBase;
                return Math.Pow(baseVal, rawInterpolated);
            }

            string rounded = rawInterpolated.ToString("N2", CultureInfo.InvariantCulture);
            return Convert.ToDouble(rounded, null);
        }

        /// <summary>
        /// Computes min and max allowed ranges for the dragged value.
        /// </summary>
        private static (double minRange, double maxRange) CalculateMinMaxRange(ChartAxisRenderer yAxis, ChartDataEditSettings pointDrag, double rawInterpolated, int extra)
        {
            double minRange = yAxis.Axis?.Minimum is not null ? yAxis.VisibleRange.Start + extra : (double.IsNaN(pointDrag.MinY) ? rawInterpolated : pointDrag.MinY);
            double maxRange = yAxis.Axis?.Maximum is not null ? yAxis.VisibleRange.End + extra : (double.IsNaN(pointDrag.MaxY) ? rawInterpolated : pointDrag.MaxY);
            return (minRange, maxRange);
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Handles the mouse down event on a data point, initiating drag preparation.
        /// </summary>
        /// <remarks>
        /// Validates editing permissions, invokes the <c>OnDataEdit</c> callback, and sets up drag state.
        /// </remarks>
        internal void PointMouseDown()
        {
            ChartData data = new(_chart);
            PointData pointData = data.GetData();
            string cursor = UpdateCursorStyle(pointData, data);
            _chart.SetSvgCursor(cursor);

            if (pointData.Point is not null && (data.InsideRegion || (pointData.Series is not null && !pointData.Series.Renderer.IsRectSeries())))
            {
                _seriesIndex = pointData.Series?.Renderer.Index;
                _pointIndex = pointData.Point.Index;
                ChartSeriesRenderer seriesRenderer = (ChartSeriesRenderer)(_chart._seriesContainer?.Renderers[_seriesIndex ?? 0] ?? null!);

                if (seriesRenderer?.Series is not null && seriesRenderer.Series.ChartDataEditSettings?.Enable == true)
                {
                    bool canceled = InvokeOnDataEdit(seriesRenderer, pointData);
                    if (!canceled)
                    {
                        _chart._isPointMouseDown = true;
                        _ = _chart.GetBooleanValuesAsync();
                        _chart._zoomSettings?.UpdateProperties("EnableDeferredZooming", false);
                    }
                }
            }
        }

        /// <summary>
        /// Handles the mouse move event, updating the dragged point's value.
        /// </summary>
        /// <remarks>
        /// Recalculates point position based on mouse movement and axis range.
        /// Only processes when a point is actively being dragged.
        /// </remarks>
        internal void PointMouseMove()
        {
            if (_chart._isPointMouseDown)
            {
                if (_chart._seriesContainer is not null && _chart._isPointMouseDown)
                {
                    ((ChartSeriesRenderer)_chart._seriesContainer.Renderers[_seriesIndex ?? 0]).FindSplinePoint();
                    PointDragging(_seriesIndex ?? 0, _pointIndex ?? 0);
                }
            }
        }

        /// <summary>
        /// Handles the mouse up event, finalizing the drag operation.
        /// </summary>
        /// <remarks>
        /// Invokes the <c>OnDataEditCompleted</c> callback and refreshes the chart.
        /// </remarks>
        internal void PointMouseUp()
        {
            if (_chart._isPointMouseDown)
            {
                _chart.SetSvgCursor("null");
                ChartSeriesRenderer seriesRenderer = _chart._seriesContainer is not null ? (ChartSeriesRenderer)_chart._seriesContainer.Renderers[_seriesIndex ?? 0] : null!;

                if (seriesRenderer?.Series is not null && seriesRenderer.Series.ChartDataEditSettings?.Enable == true)
                {
                    UpdateSeriesRenderValue(seriesRenderer);

                    _chart._isPointMouseDown = false;
                    _isPointDragging = false;
                    _isPointDragged = !_isPointDragging;
                    _ = _chart.GetBooleanValuesAsync();
                    _seriesIndex = _pointIndex = null;

                    if (!_isPointDragging)
                    {
                        _chart.GetChartPoints();
                    }
                }
            }
        }

        /// <summary>
        /// Releases references held by this instance.
        /// </summary>
        internal void Dispose()
        {
            _chart = null!;
        }
        #endregion
    }
}