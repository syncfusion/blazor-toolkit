using System.Runtime.InteropServices;
using Microsoft.JSInterop;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    internal partial class Selection : BaseSelection
    {
        #region Constants
        private const string SPACE = " ";
        private const string CIRCLE_SUFFIX = "circle_";
        private const string DRAG_CONTAINS = "Drag";
        private const string MOVE_CONTAINS = "move";
        #endregion

        #region Private Methods

        /// <summary>
        /// Processes drag selection operations during mouse movement.
        /// </summary>
        /// <param name="e">The chart mouse event arguments containing position and target information.</param>
        /// <remarks>
        /// This method handles different drag scenarios including rectangle grabbing, resizing, and lasso drawing
        /// based on the current selection mode and mouse position within the series clip rectangle.
        /// </remarks>
        private void DragSelectionProcess(ChartInternalMouseEventArgs e)
        {
            if (_chartInstance is null)
            {
                return;
            }
            if (ChartHelper.WithInBounds(_chartInstance._mouseDownX, _chartInstance._mouseDownY, SeriesClipRect))
            {
                if (_rectGrabbing && !_resizing)
                {
                    DraggedRectMoved(_dragRect ?? new Rect(0, 0, 0, 0), true);
                }
                else if (_dragging && !_resizing && _lassoDownCompleted)
                {
                    if (_chartInstance.SelectionMode == ChartSelectionMode.Lasso)
                    {
                        GetPath(_chartInstance._mouseDownX, _chartInstance._mouseDownY, _chartInstance._mouseX, _chartInstance._mouseY, (_chartInstance.AllowMultiSelection ? _count : 0).ToString(culture));
                        _ = DrawDraggingRectAsync(_dragRect ?? new Rect(0, 0, 0, 0));
                    }
                    else
                    {
                        _dragRect = GetDraggedRectLocation(_chartInstance._mouseDownX, _chartInstance._mouseDownY, _chartInstance._mouseX, _chartInstance._mouseY, SeriesClipRect);
                        _ = DrawDraggingRectAsync(_dragRect);
                    }
                }

                if (_rectPoints is not null && !_chartInstance.AllowMultiSelection)
                {
                    ResizingSelectionRect(e, new ChartEventLocation(_chartInstance._mouseX, _chartInstance._mouseY), null);
                }
                else if ((_chartInstance.AllowMultiSelection && !_dragging) || _resizing)
                {
                    ResizingSelectionRect(e, new ChartEventLocation(_chartInstance._mouseX, _chartInstance._mouseY), null);
                }
            }
            else
            {
                _ = CompleteSelectionAsync(e);
            }
        }

        /// <summary>
        /// Calculates and selects data points within the dragged selection rectangle or lasso path.
        /// </summary>
        /// <param name="dragRect">The selection rectangle or bounding area.</param>
        /// <param name="isClose">Indicates whether this is a close operation. Default is <see langword="false"/>.</param>
        /// <remarks>
        /// This method processes all visible series and their points to determine which fall within
        /// the selection area. It handles both rectangle and lasso selection modes.
        /// </remarks>
        private void CalculateDragSelectedElements(Rect dragRect, [Optional] bool isClose)
        {
            _ = RemoveSelectedElementsAsync();
            ValidateAndPrepareDragRect(dragRect, isClose);

            List<PointXY> selectedPointValues = ProcessSeriesSelection(dragRect);

            FinalizeSelection(dragRect, isClose, selectedPointValues);
        }

        /// <summary>
        /// Validates the drag rectangle and prepares data structures for selection.
        /// </summary>
        /// <param name="dragRect">The selection rectangle.</param>
        /// <param name="isClose">Indicates whether this is a close operation.</param>
        private void ValidateAndPrepareDragRect(Rect dragRect, bool isClose)
        {
            if (_chartInstance is null)
            {
                return;
            }
            Rect rect = new(dragRect.X, dragRect.Y, dragRect.Width, dragRect.Height);
            ChartEventLocation axisOffset = new(SeriesClipRect.X, SeriesClipRect.Y);
            RemoveOffset(rect, axisOffset);

            IsSeriesMode = false;
            bool isDragResize = _chartInstance.AllowMultiSelection && (_rectGrabbing || _resizing);
            _rectPoints = new Rect(dragRect.X, dragRect.Y, dragRect.Width, dragRect.Height);
            _dragRectArray[_draggedRect + (isDragResize ? _targetIndex : _count)] = _rectPoints;

            if (IsValidSelection(dragRect, isClose))
            {
                Rect filterRect = new(dragRect.X, dragRect.Y, dragRect.Width, dragRect.Height);
                RemoveOffset(filterRect, axisOffset);
                _filterArray[_draggedRect + (isDragResize ? _targetIndex : _count)] = filterRect;
            }
        }

        /// <summary>
        /// Determines if the selection dimensions are valid.
        /// </summary>
        /// <param name="dragRect">The selection rectangle.</param>
        /// <param name="isClose">Indicates whether this is a close operation.</param>
        /// <returns><see langword="true"/> if the selection is valid; otherwise, <see langword="false"/>.</returns>
        private static bool IsValidSelection(Rect dragRect, bool isClose)
        {
            return dragRect.Width > 0 && dragRect.Height > 0 && !isClose;
        }

        /// <summary>
        /// Processes all visible series to determine selected points.
        /// </summary>
        /// <param name="dragRect">The selection rectangle.</param>
        /// <returns>A list of selected point values with their coordinates and indices.</returns>
        private List<PointXY> ProcessSeriesSelection(Rect dragRect)
        {
            List<PointXY> selectedPointValues = [];
            ChartSeriesRendererContainer? seriesContainer = _chartInstance?._seriesContainer;

            if (seriesContainer is null)
            {
                return selectedPointValues;
            }

            ChartEventLocation axisOffset = new(SeriesClipRect.X, SeriesClipRect.Y);
            Rect selectionRect = new(dragRect.X, dragRect.Y, dragRect.Width, dragRect.Height);
            RemoveOffset(selectionRect, axisOffset);

            foreach (ChartSeriesRenderer seriesRenderer in seriesContainer.Renderers.Cast<ChartSeriesRenderer>())
            {
                ProcessSeriesPoints(seriesRenderer, selectionRect, axisOffset, selectedPointValues);
            }

            return selectedPointValues;
        }

        /// <summary>
        /// Processes points in a single series for selection.
        /// </summary>
        /// <param name="seriesRenderer">The series renderer.</param>
        /// <param name="selectionRect">The selection rectangle with offset removed.</param>
        /// <param name="axisOffset">The axis offset for coordinate adjustment.</param>
        /// <param name="selectedPointValues">The list to populate with selected point values.</param>
        private void ProcessSeriesPoints(ChartSeriesRenderer seriesRenderer, Rect selectionRect, ChartEventLocation axisOffset, List<PointXY> selectedPointValues)
        {
            ChartSeries? series = seriesRenderer.Series;
            if (series is null || !series.Visible || seriesRenderer.Points is null)
            {
                return;
            }

            (double xOffset, double yOffset) = CalculateAxisOffsets(seriesRenderer, series, axisOffset);

            foreach (Point currentPoint in seriesRenderer.Points)
            {
                ProcessSinglePoint(currentPoint, seriesRenderer, xOffset, yOffset, selectionRect, selectedPointValues);
            }
        }

        /// <summary>
        /// Calculates axis offsets for coordinate transformation.
        /// </summary>
        /// <param name="seriesRenderer">The series renderer.</param>
        /// <param name="series">The chart series.</param>
        /// <param name="axisOffset">The base axis offset.</param>
        /// <returns>A tuple containing X and Y axis offsets.</returns>
        private (double xOffset, double yOffset) CalculateAxisOffsets(ChartSeriesRenderer seriesRenderer, ChartSeries series, ChartEventLocation axisOffset)
        {
            if (_chartInstance is null)
            {
                return (0, 0);
            }
            bool isTransposedBar = (_chartInstance.IsTransposed ||
                                   (series.SeriesType is not null && series.SeriesType.Contains("Bar", StringComparison.InvariantCulture))) &&
                                   !(_chartInstance.IsTransposed && series.SeriesType is not null && series.SeriesType.Contains("Bar", StringComparison.InvariantCulture));

            if (isTransposedBar)
            {
                return (seriesRenderer.XAxisRenderer.Rect.Y - axisOffset.Y, seriesRenderer.YAxisRenderer.Rect.X - axisOffset.X);
            }

            return (seriesRenderer.XAxisRenderer.Rect.X - axisOffset.X, seriesRenderer.YAxisRenderer.Rect.Y - axisOffset.Y);
        }

        /// <summary>
        /// Processes a single data point for selection.
        /// </summary>
        /// <param name="currentPoint">The data point to process.</param>
        /// <param name="seriesRenderer">The series renderer containing the point.</param>
        /// <param name="xAxisOffset">The X axis offset.</param>
        /// <param name="yAxisOffset">The Y axis offset.</param>
        /// <param name="selectionRect">The selection rectangle.</param>
        /// <param name="selectedPointValues">The list to add selected points to.</param>
        private void ProcessSinglePoint(Point currentPoint, ChartSeriesRenderer seriesRenderer, double xAxisOffset, double yAxisOffset, Rect selectionRect, List<PointXY> selectedPointValues)
        {
            if (seriesRenderer.Category() == SeriesCategories.Indicator)
            {
                return;
            }

            bool isSelected = DeterminePointSelection(currentPoint, xAxisOffset, yAxisOffset, selectionRect);

            if (isSelected)
            {
                SelectPointAndAddToValues(currentPoint, seriesRenderer, selectedPointValues);
            }
        }

        /// <summary>
        /// Determines if a point should be selected based on selection mode and position.
        /// </summary>
        /// <param name="currentPoint">The point to check.</param>
        /// <param name="xAxisOffset">The X axis offset.</param>
        /// <param name="yAxisOffset">The Y axis offset.</param>
        /// <param name="selectionRect">The selection rectangle.</param>
        /// <returns><see langword="true"/> if the point is selected; otherwise, <see langword="false"/>.</returns>
        private bool DeterminePointSelection(Point currentPoint, double xAxisOffset, double yAxisOffset, Rect selectionRect)
        {

            return _chartInstance is not null && _chartInstance.SelectionMode == ChartSelectionMode.Lasso
                ? currentPoint.IsSelected
                : _chartInstance is not null && _chartInstance.AllowMultiSelection
                ? IsPointSelect(currentPoint, xAxisOffset, yAxisOffset, [.. _filterArray.Values])
                : currentPoint.SymbolLocations.Exists(location =>
                location is not null &&
                ChartHelper.WithInBounds(location.X + xAxisOffset, location.Y + yAxisOffset, selectionRect));
        }

        /// <summary>
        /// Selects a point and adds its values to the selected points collection.
        /// </summary>
        /// <param name="currentPoint">The point to select.</param>
        /// <param name="seriesRenderer">The series renderer.</param>
        /// <param name="selectedPointValues">The collection to add the point values to.</param>
        private void SelectPointAndAddToValues(Point currentPoint, ChartSeriesRenderer seriesRenderer, List<PointXY> selectedPointValues)
        {
            object selectedPointX = GetFormattedXValue(currentPoint);

            _ = SelectionChartAsync(ChartSelectedDataIndex.CreateSelectedData(currentPoint.Index, seriesRenderer.Index));
            selectedPointValues.Add(new PointXY
            {
                X = selectedPointX,
                Y = currentPoint.YValue,
                SeriesIndex = seriesRenderer.Index,
                PointIndex = currentPoint.Index
            });
        }

        /// <summary>
        /// Gets the formatted X value based on the axis value type.
        /// </summary>
        /// <param name="currentPoint">The data point.</param>
        /// <returns>The formatted X value as an object.</returns>
        private object GetFormattedXValue(Point currentPoint)
        {
            return _chartInstance?._axisContainer?.Axes["PrimaryXAxis"].ValueType == ValueType.Category
                ? currentPoint.X.ToString() ?? string.Empty
                : _chartInstance?._axisContainer?.Axes["PrimaryXAxis"].ValueType == ValueType.DateTime
                ? Convert.ToDateTime(currentPoint.X, null)
                : currentPoint.XValue;
        }

        /// <summary>
        /// Finalizes the selection by applying blur effects and invoking selection events.
        /// </summary>
        /// <param name="dragRect">The selection rectangle.</param>
        /// <param name="isClose">Indicates whether this is a close operation.</param>
        /// <param name="selectedPointValues">The list of selected point values.</param>
        private void FinalizeSelection(Rect dragRect, bool isClose, List<PointXY> selectedPointValues)
        {
            _ = BlurEffectAsync();
            if (_chartInstance is null)
            {
                return;
            }
            if (!isClose)
            {
                CreateCloseButton(
                    _chartInstance.SelectionMode == ChartSelectionMode.Lasso ? _chartInstance._mouseDownX : (dragRect.X + dragRect.Width),
                    _chartInstance.SelectionMode == ChartSelectionMode.Lasso ? _chartInstance._mouseDownY : dragRect.Y);
            }

            InvokeSelectionChangedEvent(selectedPointValues);
        }

        /// <summary>
        /// Invokes the selection changed event with the selected data values.
        /// </summary>
        /// <param name="selectedPointValues">The list of selected point values.</param>
        private void InvokeSelectionChangedEvent(List<PointXY> selectedPointValues)
        {
            _chartInstance?.OnSelectionChanged.Invoke(new SelectionCompleteEventArgs
            {
                SelectedDataValues = selectedPointValues,
                Name = Constants.OnSelectionChanged
            });

        }

        /// <summary>
        /// Creates a close button icon at the specified coordinates for dismissing the selection.
        /// </summary>
        /// <param name="x">The X coordinate for the close button.</param>
        /// <param name="y">The Y coordinate for the close button.</param>
        /// <remarks>
        /// The close button consists of a circle with a cross icon that allows users to
        /// remove the current selection area.
        /// </remarks>
        private void CreateCloseButton(double x, double y)
        {
            if (_chartInstance is null)
            {
                return;
            }
            bool isMultiDrag = _chartInstance.AllowMultiSelection, isDragResize = isMultiDrag && (_rectGrabbing || _resizing), isDrag = _rectGrabbing || _resizing;
            CircleOptions circle = CreateCloseCircle(isMultiDrag, isDrag, x, y);
            _closeCircleArray[_closeIconId + CIRCLE_SUFFIX + (isMultiDrag ? (isDrag ? _targetIndex : _count).ToString(culture) : string.Empty)] = circle;
            PathOptions path = UpdatePathOptions(isMultiDrag, isDrag, x, y);
            SvgSelectionRect element = _chartInstance._parentRect?.RectsReference.Values.FirstOrDefault(item => item.Id == _draggedRect + ((isDragResize || !_chartInstance.AllowMultiSelection) ? _targetIndex : _count)) ?? null!;
            SvgSelectionPath elementPath = _chartInstance._parentRect?.PathsReference.Values.FirstOrDefault(item => item.Id == _draggedRect + ((isDragResize || !_chartInstance.AllowMultiSelection) ? _targetIndex : _count)) ?? null!;

            if (!_chartInstance.AllowMultiSelection)
            {
                element = _chartInstance._parentRect?.RectsReference.Count > 0 ? _chartInstance._parentRect.RectsReference.Values.FirstOrDefault() ?? null! : null!;
                elementPath = _chartInstance._parentRect?.PathsReference.Count > 0 ? _chartInstance._parentRect.PathsReference.Values.FirstOrDefault() ?? null! : null!;
            }

            _ = _chartInstance.SelectionMode == ChartSelectionMode.Lasso
                ? (elementPath?.DrawCloseIconAsync(circle, path))
                : (element?.DrawCloseIconAsync(circle, path));
        }

        /// <summary>
        /// Creates a close button icon at the specified coordinates for dismissing the selection.
        /// </summary>
        /// <param name="isMultiDrag">Indicates if multi-selection is enabled.</param>
        /// <param name="isDrag">Indicates if current operation is drag or resize.</param>
        /// <param name="x">The X coordinate for the close button.</param>
        /// <param name="y">The Y coordinate for the close button.</param>
        /// <returns>A <see cref="CircleOptions"/> instance for the close circle.</returns>
        private CircleOptions CreateCloseCircle(bool isMultiDrag, bool isDrag, double x, double y)
        {
            return new CircleOptions(
                _closeIconId + CIRCLE_SUFFIX + (isMultiDrag ? (isDrag ? _targetIndex : _count).ToString(culture) : string.Empty),
                x.ToString(culture),
                y.ToString(culture),
                "10",
                string.Empty,
                2,
                _chartInstance?._chartThemeStyle?.SelectionCircleStroke ?? string.Empty,
                1,
                "#FFFFFF"
            );
        }

        /// <summary>
        /// Updates path options for the close icon cross symbol.
        /// </summary>
        /// <param name="isMultiDrag">Indicates if multi-selection is enabled.</param>
        /// <param name="isDrag">Indicates if current operation is drag or resize.</param>
        /// <param name="x">The X coordinate for the close icon.</param>
        /// <param name="y">The Y coordinate for the close icon.</param>
        /// <returns>A <see cref="PathOptions"/> instance for the close cross path.</returns>
        private PathOptions UpdatePathOptions(bool isMultiDrag, bool isDrag, double x, double y)
        {
            return new PathOptions
            (
                _closeIconId + "cross_" + (isMultiDrag ? (isDrag ? _targetIndex : _count).ToString(culture) : string.Empty),
                "M " + (x - 4).ToString(culture) + SPACE + (y - 4).ToString(culture) + " L " + (x + 4).ToString(culture) + SPACE + (y + 4).ToString(culture) +
                " M " + (x - 4).ToString(culture) + SPACE + (y + 4).ToString(culture) + " L " + (x + 4).ToString(culture) + SPACE + (y - 4).ToString(culture),
                string.Empty,
                2,
                _chartInstance?._chartThemeStyle?.SelectionCircleStroke ?? string.Empty
            );
        }

        /// <summary>
        /// Handles the movement of a grabbed selection rectangle.
        /// </summary>
        /// <param name="grabbedPoint">The original grabbed point position.</param>
        /// <param name="doDrawing">If <see langword="true"/>, draws the rectangle; otherwise calculates selection. Default is <see langword="false"/>.</param>
        /// <remarks>
        /// Calculates the new position based on mouse movement and ensures the rectangle
        /// stays within the series clip bounds.
        /// </remarks>
        private void DraggedRectMoved(Rect grabbedPoint, [Optional] bool doDrawing)
        {
            if (_chartInstance is null)
            {
                return;
            }
            Rect rect;
            if ((_resizing || _rectGrabbing) && _chartInstance.AllowMultiSelection)
            {
                Rect r = _dragRectArray[_draggedRect + _targetIndex];
                rect = new Rect(r.X, r.Y, r.Width, r.Height);
            }
            else
            {
                rect = new Rect(_rectPoints.X, _rectPoints.Y, _rectPoints.Width, _rectPoints.Height);
            }

            rect.X -= grabbedPoint.X - _chartInstance._mouseX;
            rect.Y -= grabbedPoint.Y - _chartInstance._mouseY;
            rect = GetDraggedRectLocation(rect.X, rect.Y, rect.X + rect.Width, rect.Height + rect.Y, SeriesClipRect);
            if (doDrawing)
            {
                _ = DrawDraggingRectAsync(rect);
            }
            else
            {
                CalculateDragSelectedElements(rect, false);
            }
        }

        /// <summary>
        /// Initializes drag operation state when drag begins.
        /// </summary>
        /// <param name="events">The chart mouse event arguments.</param>
        /// <remarks>
        /// Sets up initial drag rectangle and validates that the drag starts within valid bounds.
        /// </remarks>
        private void InitializeDragOperation(ChartInternalMouseEventArgs events)
        {
            if (_chartInstance is null)
            {
                return;
            }
            _count = _dragRectArray.ContainsKey(events.Target) ? _count : _count + 1;
            _dragRect = new Rect(_chartInstance._mouseDownX, _chartInstance._mouseDownY, 0, 0);
            if (_chartInstance._mouseDownX < SeriesClipRect.X || _chartInstance._mouseDownX > (SeriesClipRect.X + SeriesClipRect.Width) || _chartInstance._mouseDownY < SeriesClipRect.Y || _chartInstance._mouseDownY > (SeriesClipRect.Y + SeriesClipRect.Height))
            {
                _dragging = false;
            }
        }

        /// <summary>
        /// Handles rectangle mode initialization for resizing and grabbing operations.
        /// </summary>
        /// <param name="events">The chart mouse event arguments.</param>
        /// <param name="mouseDownX">The X coordinate of mouse down.</param>
        /// <param name="mouseDownY">The Y coordinate of mouse down.</param>
        /// <remarks>
        /// Determines if the user clicked on an existing rectangle for grabbing or
        /// on a resize handle for resizing operations.
        /// </remarks>
        private void HandleRectangleModeInitialization(ChartInternalMouseEventArgs events, double mouseDownX, double mouseDownY)
        {
            if (_chartInstance is null)
            {
                return;
            }
            if (_rectPoints is not null && !_chartInstance.AllowMultiSelection)
            {
                _dragRect = new Rect(_chartInstance._mouseDownX, _chartInstance._mouseDownY, 0, 0);
                ResizingSelectionRect(events, new ChartEventLocation(mouseDownX, mouseDownY), true);
                _rectGrabbing = ChartHelper.WithInBounds(mouseDownX, mouseDownY, _rectPoints);
            }

            if (_chartInstance.AllowMultiSelection)
            {
                int index = GetIndex(events.Target);
                _targetIndex = IsDragRect(events.Target) ? index : 0;
                if (_dragRectArray.Count != 0 && IsDragRect(events.Target))
                {
                    ResizingSelectionRect(events, new ChartEventLocation(mouseDownX, mouseDownY), true);
                    _rectGrabbing = _dragRectArray.ContainsKey(_draggedRect + index) ? ChartHelper.WithInBounds(mouseDownX, mouseDownY, _dragRectArray[_draggedRect + index]) : _rectGrabbing;
                }
            }
        }

        /// <summary>
        /// Handles selection rectangle resizing operations.
        /// </summary>
        /// <param name="e">The chart mouse event arguments.</param>
        /// <param name="location">The current mouse location.</param>
        /// <param name="tapped">Indicates if this is a tap/click event; <see langword="null"/> for mouse move.</param>
        /// <remarks>
        /// Determines the resize mode based on mouse position relative to rectangle edges
        /// and adjusts rectangle dimensions accordingly.
        /// </remarks>
        private void ResizingSelectionRect(ChartInternalMouseEventArgs e, ChartEventLocation location, bool? tapped)
        {
            Rect rect = GetResizingRect(e);
            if (rect.Width > 0 && rect.Height > 0)
            {
                if (_resizing)
                {
                    rect = GetDraggedRectLocation(rect.X, rect.Y, rect.X + rect.Width, rect.Y + rect.Height, SeriesClipRect);
                    _ = DrawDraggingRectAsync(rect);
                    _dragRect = rect;
                }

                if (tapped == true)
                {
                    _resizing = FindResizeMode(rect, location);
                }
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// Gets the rectangle to be resized based on selection mode.
        /// </summary>
        /// <param name="e">The chart mouse event arguments.</param>
        /// <returns>The rectangle to resize.</returns>
        private Rect GetResizingRect(ChartInternalMouseEventArgs e)
        {
            Rect rect = new();

            if ((_chartInstance is not null && _chartInstance.AllowMultiSelection && IsDragRect(e.Target)) || _dragRectArray.ContainsKey(_draggedRect + _targetIndex))
            {
                if (IsDragRect(e.Target))
                {
                    _targetIndex = GetIndex(e.Target);
                }

                if (_dragRectArray.ContainsKey(_draggedRect + _targetIndex))
                {
                    rect = _dragRectArray[_draggedRect + _targetIndex];
                }
            }

            if (_chartInstance is not null && !_chartInstance.AllowMultiSelection)
            {
                rect = new Rect(_rectPoints.X, _rectPoints.Y, _rectPoints.Width, _rectPoints.Height);
            }
            return rect;
        }

        /// <summary>
        /// Finds the resize mode based on mouse position relative to rectangle edges.
        /// </summary>
        /// <param name="rect">The rectangle being resized.</param>
        /// <param name="location">The current mouse location.</param>
        /// <returns><see langword="true"/> if a resize mode was found; otherwise, <see langword="false"/>.</returns>
        private bool FindResizeMode(Rect rect, ChartEventLocation location)
        {
            if (_chartInstance is null)
            {
                return false;
            }
            string cursorStyle = "se-resize";
            bool resize = false;
            if (!_resizing)
            {
                resize = DetectResizeEdge(rect, location, out cursorStyle);
            }
            else
            {
                ApplyResizeTransformation(rect, location);
            }

            if (CurrentMode != ChartSelectionMode.Lasso)
            {
                ChangeCursorStyle(resize, GetRectangleElement(_chartInstance.AllowMultiSelection ? _draggedRect + _targetIndex : _draggedRect + 0), cursorStyle);
            }

            return resize;
        }

        /// <summary>
        /// Detects which edge or corner of the rectangle is being resized.
        /// </summary>
        /// <param name="rect">The rectangle being resized.</param>
        /// <param name="location">The current mouse location.</param>
        /// <param name="cursorStyle">The cursor style to apply during resizing.</param>
        /// <returns><see langword="true"/> if a resize edge was detected; otherwise, <see langword="false"/>.</returns>
        private bool DetectResizeEdge(Rect rect, ChartEventLocation location, out string cursorStyle)
        {
            cursorStyle = "se-resize";
            bool resize = false;

            List<Rect> resizeEdges = [
                new Rect(rect.X, rect.Y, rect.Width - 5, 5),
                new Rect(rect.X, rect.Y, 5, rect.Height),
                new Rect(rect.X, rect.Y + rect.Height - 5, rect.Width - 5, 5),
                new Rect(rect.X + rect.Width - 5, rect.Y + 5, 5, rect.Height - 15),
                new Rect(rect.X + rect.Width - 10, rect.Y + rect.Height - 10, 10, 10)
            ];

            for (int i = 0; i < resizeEdges.Count; i++)
            {
                if (ChartHelper.WithInBounds(location.X, location.Y, resizeEdges[i]))
                {
                    cursorStyle = (i == 4) ? cursorStyle : (i % 2 == 0) ? "ns-resize" : "ew-resize";
                    resize = true;
                    _resizeMode = i;
                    break;
                }
            }
            return resize;
        }

        /// <summary>
        /// Applies the resize transformation based on the active resize mode.
        /// </summary>
        /// <param name="rect">The rectangle being resized.</param>
        /// <param name="location">The current mouse location.</param>
        private void ApplyResizeTransformation(Rect rect, ChartEventLocation location)
        {
            double x = rect.X, y = rect.Y;
            double width = location.X - x, height = location.Y - y;

            switch (_resizeMode)
            {
                case 0:
                    height = Math.Abs(rect.Height + rect.Y - location.Y);
                    rect.Y = Math.Min(rect.Height + rect.Y, location.Y);
                    rect.Height = height;
                    break;
                case 1:
                    width = Math.Abs(rect.Width + rect.X - location.X);
                    rect.X = Math.Min(rect.Width + rect.X, location.X);
                    rect.Width = width;
                    break;
                case 2:
                    rect.Height = Math.Abs(height);
                    rect.Y = Math.Min(location.Y, y);
                    break;
                case 3:
                    rect.Width = Math.Abs(width);
                    rect.X = Math.Min(location.X, x);
                    break;
                case 4:
                    rect.Width = Math.Abs(width);
                    rect.Height = Math.Abs(height);
                    rect.X = Math.Min(location.X, x);
                    rect.Y = Math.Min(location.Y, y);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Draws or updates the drag selection rectangle or lasso path.
        /// </summary>
        /// <param name="dragRect">The rectangle to draw.</param>
        /// <remarks>
        /// Handles both rectangle and lasso selection modes, adjusting dimensions based on
        /// the selection mode (DragX, DragY, or full drag).
        /// </remarks>
        private async Task DrawDraggingRectAsync(Rect dragRect)
        {
            Rect cartesianLayout = AdjustCartesianLayout();
            bool isLasso = _chartInstance?.SelectionMode == ChartSelectionMode.Lasso;

            dragRect = AdjustDragRectByMode(dragRect, cartesianLayout);

            if ((dragRect.Width < 5 || dragRect.Height < 5) && !isLasso)
            {
                return;
            }

            _ = SfBaseComponent.InvokeVoidAsync(_chartInstance?._chartJsModule, _chartInstance?._chartJsInProcessModule, "dragStart");

            if (_chartInstance is not null && _chartInstance.AllowMultiSelection &&
                (_chartInstance.SelectionMode.ToString().Contains(DRAG_CONTAINS, StringComparison.InvariantCulture) || isLasso))
            {
                await DrawMultiSelectionRectAsync(dragRect, isLasso).ConfigureAwait(false);
            }
            else
            {
                await DrawSingleSelectionRectAsync(dragRect, isLasso).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Adjusts the cartesian layout for drawing.
        /// </summary>
        /// <returns>The adjusted cartesian layout rectangle.</returns>
        private Rect AdjustCartesianLayout()
        {
            if (_chartInstance is null)
            {
                return new Rect(0, 0, 0, 0);
            }
            Rect cartesianLayout = SeriesClipRect;
            double border = _chartInstance._chartAreaRenderer?.Area?.Border.Width ?? 0;

            if (_isdrawRect)
            {
                cartesianLayout.X -= border / 2;
                cartesianLayout.Y -= border / 2;
                cartesianLayout.Width += border;
                cartesianLayout.Height += border;
                _isdrawRect = false;
            }
            return cartesianLayout;
        }

        /// <summary>
        /// Returns a drag rectangle adjusted based on selection mode.
        /// </summary>
        /// <param name="dragRect">The drag rectangle (struct copy).</param>
        /// <param name="cartesianLayout">The cartesian layout bounds.</param>
        /// <returns>The adjusted rectangle.</returns>
        private Rect AdjustDragRectByMode(Rect dragRect, Rect cartesianLayout)
        {
            if (_chartInstance is null)
            {
                return dragRect;
            }

            switch (_chartInstance.SelectionMode)
            {
                case ChartSelectionMode.DragX:
                    dragRect.Y = cartesianLayout.Y;
                    dragRect.Height = cartesianLayout.Height;
                    break;
                case ChartSelectionMode.DragY:
                    dragRect.X = cartesianLayout.X;
                    dragRect.Width = cartesianLayout.Width;
                    break;
                case ChartSelectionMode.None:
                case ChartSelectionMode.Series:
                case ChartSelectionMode.Point:
                case ChartSelectionMode.Cluster:
                case ChartSelectionMode.DragXY:
                case ChartSelectionMode.Lasso:
                    break;
                default:
                    break;
            }
            return dragRect;
        }

        /// <summary>
        /// Draws multi-selection rectangles or lasso paths.
        /// </summary>
        private async Task DrawMultiSelectionRectAsync(Rect dragRect, bool isLasso)
        {
            string rectFill = _chartInstance?._chartThemeStyle?.SelectionRectFill ?? string.Empty;
            string rectStroke = _chartInstance?._chartThemeStyle?.SelectionRectStroke ?? string.Empty;

            if (_rectGrabbing || _resizing)
            {
                await UpdateExistingRectangleAsync(dragRect).ConfigureAwait(false);
            }
            else
            {
                if (!isLasso)
                {
                    await CreateRectangleElementAsync(dragRect, rectFill, rectStroke).ConfigureAwait(false);
                }
                else
                {
                    await CreateLassoElementAsync(rectFill, rectStroke).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Updates an existing selection rectangle.
        /// </summary>
        private async Task UpdateExistingRectangleAsync(Rect dragRect)
        {
            SvgSelectionRect rectElement = GetRectangleElement(_draggedRect + _targetIndex);
            if (rectElement is not null)
            {
                await rectElement.ChangeRectangleAsync(dragRect).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Creates a rectangle selection element.
        /// </summary>
        private async Task CreateRectangleElementAsync(Rect dragRect, string rectFill, string rectStroke)
        {
            if (_chartInstance is null)
            {
                return;
            }
            if (_chartInstance._parentRect is not null && !_chartInstance._parentRect.RectsReference.ContainsKey(_draggedRect + _count))
            {
                _chartInstance._parentRect.DrawNewRectangle(new SelectionOptions()
                {
                    DragRect = dragRect,
                    Fill = rectFill,
                    Stroke = rectStroke,
                    StrokeWidth = "1",
                    Id = _draggedRect + _count
                });
            }
            else
            {
                if (_chartInstance._parentRect is not null && _chartInstance._parentRect.RectsReference.ContainsKey(_draggedRect + _count))
                {
                    await _chartInstance._parentRect.RectsReference[_draggedRect + _count].ChangeRectangleAsync(dragRect).ConfigureAwait(true);
                }
            }
        }

        /// <summary>
        /// Creates a lasso selection element.
        /// </summary>
        private async Task CreateLassoElementAsync(string rectFill, string rectStroke)
        {
            if (_chartInstance is null)
            {
                return;
            }
            if (_chartInstance._parentRect is not null && !_chartInstance._parentRect.PathsReference.ContainsKey(_draggedRect + _count))
            {
                _chartInstance._parentRect.DrawNewRectangle(new SelectionOptions()
                {
                    Fill = rectFill,
                    Stroke = rectStroke,
                    StrokeWidth = "3",
                    Id = _draggedRect + _count,
                    Path = _lassoPaths[_count.ToString(culture)],
                    IsLasso = true
                });
            }
            else
            {
                if (_chartInstance._parentRect is not null && _chartInstance._parentRect.PathsReference.ContainsKey(_draggedRect + _count))
                {
                    await _chartInstance._parentRect.PathsReference[_draggedRect + _count].ChangePathAsync(_lassoPaths[_count.ToString(culture)]).ConfigureAwait(true);
                }
            }
        }

        /// <summary>
        /// Draws single-selection rectangles or lasso paths.
        /// </summary>
        private async Task DrawSingleSelectionRectAsync(Rect dragRect, bool isLasso)
        {
            string rectFill = _chartInstance?._chartThemeStyle?.SelectionRectFill ?? string.Empty;
            string rectStroke = _chartInstance?._chartThemeStyle?.SelectionRectStroke ?? string.Empty;

            if (!isLasso)
            {
                await DrawSingleRectangleAsync(dragRect, rectFill, rectStroke).ConfigureAwait(false);
            }
            else
            {
                await DrawSingleLassoAsync(rectFill, rectStroke).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Draws a single rectangle selection.
        /// </summary>
        private async Task DrawSingleRectangleAsync(Rect dragRect, string rectFill, string rectStroke)
        {
            if (_chartInstance?._parentRect?.RectsReference.Count == 0)
            {
                _chartInstance._parentRect.DrawNewRectangle(new SelectionOptions()
                {
                    DragRect = dragRect,
                    Fill = rectFill,
                    Stroke = rectStroke,
                    StrokeWidth = "1",
                    Id = _draggedRect + _count
                });
            }
            else
            {
                SvgSelectionRect firstOrDefault = _chartInstance?._parentRect?.RectsReference.Values.FirstOrDefault() ?? null!;
                if (firstOrDefault is not null)
                {
                    await firstOrDefault.ChangeRectangleAsync(dragRect).ConfigureAwait(true);
                }
            }
        }

        /// <summary>
        /// Draws a single lasso selection.
        /// </summary>
        private async Task DrawSingleLassoAsync(string rectFill, string rectStroke)
        {
            if (_chartInstance is null)
            {
                return;
            }
            if (_chartInstance._parentRect?.PathsReference.Count == 0)
            {
                _lassoPaths["0"] = string.Empty;
                _chartInstance._parentRect.DrawNewRectangle(new SelectionOptions()
                {
                    Fill = rectFill,
                    Stroke = rectStroke,
                    StrokeWidth = "3",
                    Id = _draggedRect + _count,
                    IsLasso = true,
                    Path = string.Empty
                });
            }
            else
            {
                SvgSelectionPath firstOrDefault = _chartInstance._parentRect?.PathsReference.Values.FirstOrDefault() ?? null!;
                if (firstOrDefault is not null)
                {
                    await firstOrDefault.ChangePathAsync(_lassoPaths["0"]).ConfigureAwait(true);
                }
            }
        }

        /// <summary>
        /// Completes the drag selection operation when mouse up occurs.
        /// </summary>
        /// <param name="e">The chart mouse event arguments.</param>
        /// <remarks>
        /// Finalizes the selection by calculating selected elements, handling lasso closure,
        /// and cleaning up temporary drag elements.
        /// </remarks>
        private async Task CompleteSelectionAsync(ChartInternalMouseEventArgs e)
        {
            if (_chartInstance is null || _chartInstance.SelectionMode == ChartSelectionMode.None)
            {
                return;
            }

            CurrentMode = _chartInstance.SelectionMode;
            Rect previousRect = GetPreviousRect();

            ProcessDragCompletionAsync(previousRect);
            await ProcessLassoCompletionAsync(e).ConfigureAwait(false);

            _dragging = _rectGrabbing = _resizing = false;
            RemoveDraggedElements(e);
        }

        /// <summary>
        /// Gets the previous rectangle for completion operations.
        /// </summary>
        /// <returns>The previous rectangle or an empty rectangle.</returns>
        private Rect GetPreviousRect()
        {
            return _dragRectArray.ContainsKey(_draggedRect + _targetIndex) ? _dragRectArray[_draggedRect + _targetIndex] : new Rect(0, 0, 0, 0);
        }

        /// <summary>
        /// Processes drag completion operations.
        /// </summary>
        /// <param name="previousRect">The previous rectangle state.</param>
        private void ProcessDragCompletionAsync(Rect previousRect)
        {
            if ((_dragging || _resizing) && _dragRect?.Width > 5 && _dragRect.Height > 5)
            {
                CalculateDragSelectedElements(_dragRect);
            }
            else if (_chartInstance is not null && !_chartInstance.AllowMultiSelection && _rectGrabbing && _rectPoints.Width > 0 && _rectPoints.Height > 0)
            {
                DraggedRectMoved(_dragRect ?? new Rect(0, 0, 0, 0));
            }
            else if (_rectGrabbing && previousRect.Width > 0 && previousRect.Height > 0)
            {
                DraggedRectMoved(_dragRect ?? new Rect(0, 0, 0, 0));
            }
        }

        /// <summary>
        /// Processes lasso completion operations including path closure and checking.
        /// </summary>
        /// <param name="e">The chart mouse event arguments.</param>
        private async Task ProcessLassoCompletionAsync(ChartInternalMouseEventArgs e)
        {
            if (_chartInstance is not null && _chartInstance.SelectionMode == ChartSelectionMode.Lasso && _dragging && _lassoDownCompleted && _lassoPaths.Count > 0)
            {
                _lassoDownCompleted = false;
                if (!_chartInstance.AllowMultiSelection && _lassoPaths["0"].Contains('L', StringComparison.InvariantCulture) && !e.Target.Contains("close", StringComparison.InvariantCulture))
                {
                    SvgSelectionPath lassoEle = _chartInstance._parentRect?.PathsReference.Values.FirstOrDefault() ?? null!;
                    if (lassoEle is not null)
                    {
                        _lassoPaths["0"] += " Z";
                        await lassoEle.ChangePathAsync(_lassoPaths["0"]).ConfigureAwait(true);
                        await LassoCheckingAsync(lassoEle.Id).ConfigureAwait(true);
                    }
                }
                else
                {
                    SvgSelectionPath lassoEle = _chartInstance._parentRect?.PathsReference.Values.FirstOrDefault(item => item.Id == _draggedRect + _count) ?? null!;
                    if (lassoEle is not null && _lassoPaths[_count.ToString(culture)].Contains('L', StringComparison.InvariantCulture))
                    {
                        await lassoEle.ChangePathAsync(_lassoPaths[_count.ToString(culture)] + "Z").ConfigureAwait(true);
                        await LassoCheckingAsync(_draggedRect + GetIndex(lassoEle.Id)).ConfigureAwait(true);
                    }
                }

                if (_dragging || _resizing)
                {
                    CalculateDragSelectedElements(_dragRect ?? new Rect(0, 0, 0, 0));
                }
            }
        }

        /// <summary>
        /// Performs lasso selection checking using JavaScript interop to determine point containment.
        /// </summary>
        /// <param name="lassoPathId">The ID of the lasso path element.</param>
        /// <remarks>
        /// Uses browser APIs to check if data points fall within the lasso path polygon.
        /// Updates point selection state and maintains multi-selection tracking.
        /// </remarks>
        private async Task LassoCheckingAsync(string lassoPathId)
        {
            if (_chartInstance is null || _chartInstance.JSRuntime is null || _chartInstance._seriesContainer is null)
            {
                return;
            }
            DomRect elementOffset = await JSRuntimeExtensions.InvokeAsync<DomRect>(_chartInstance.JSRuntime, Constants.GetElementBoundsById, [.. new object[] { _chartInstance.ID }]).ConfigureAwait(true);
            double offsetX = SeriesClipRect.X + Math.Max(elementOffset.Left, 0);
            double offsetY = SeriesClipRect.Y + Math.Max(elementOffset.Top, 0);

            foreach (ChartSeriesRenderer series in _chartInstance._seriesContainer.Renderers.Cast<ChartSeriesRenderer>())
            {
                if (series.Points is not null)
                {
                    await ProcessLassoSeriesPointsAsync(series, lassoPathId, offsetX, offsetY).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Processes points in a series for lasso selection.
        /// </summary>
        /// <param name="series">The series renderer.</param>
        /// <param name="lassoPathId">The lasso path ID.</param>
        /// <param name="offsetX">The X offset.</param>
        /// <param name="offsetY">The Y offset.</param>
        private async Task ProcessLassoSeriesPointsAsync(ChartSeriesRenderer series, string lassoPathId, double offsetX, double offsetY)
        {
            if (series.Points is null)
            {
                return;
            }
            foreach (Point dataPoint in series.Points)
            {
                string selectedId = await SfBaseComponent.InvokeAsync<string>(_chartInstance?._chartJsModule!, _chartInstance?._chartJsInProcessModule!, "isLassoId", [.. new object[] { dataPoint.SymbolLocations[0].X + offsetX, dataPoint.SymbolLocations[0].Y + offsetY }]).ConfigureAwait(true);
                if (lassoPathId == selectedId)
                {
                    dataPoint.IsSelected = true;
                    UpdateLassoPointTracking(series, dataPoint);
                }
                else if (_chartInstance is not null && !_chartInstance.AllowMultiSelection)
                {
                    dataPoint.IsSelected = false;
                }
            }
        }

        /// <summary>
        /// Updates lasso point tracking for multi-selection.
        /// </summary>
        /// <param name="series">The series renderer.</param>
        /// <param name="dataPoint">The data point to track.</param>
        private void UpdateLassoPointTracking(ChartSeriesRenderer series, Point dataPoint)
        {
            if (_chartInstance is not null && _chartInstance.AllowMultiSelection && CurrentMode == ChartSelectionMode.Lasso)
            {
                if (_selectedLassoPoints.TryGetValue((_count, series.Index), out List<Point>? selectedPoints))
                {
                    selectedPoints.Add(dataPoint);
                }
                else
                {
                    selectedPoints = [];
                    _selectedLassoPoints.Add((_count, series.Index), selectedPoints);
                    selectedPoints.Add(dataPoint);
                }
            }
        }

        /// <summary>
        /// Gets a rectangle element by its identifier.
        /// </summary>
        /// <param name="id">The element identifier.</param>
        /// <returns>The <see cref="SvgSelectionRect"/> element or <see langword="null"/> if not found.</returns>
        private SvgSelectionRect GetRectangleElement(string id)
        {
            return _chartInstance is not null && _chartInstance._parentRect is not null && _chartInstance._parentRect.RectsReference.TryGetValue(id, out SvgSelectionRect? rect)
                ? rect
                : null!;
        }

        /// <summary>
        /// Calculates the bounded drag rectangle location within outer bounds.
        /// </summary>
        /// <param name="x1">The first X coordinate.</param>
        /// <param name="y1">The first Y coordinate.</param>
        /// <param name="x2">The second X coordinate.</param>
        /// <param name="y2">The second Y coordinate.</param>
        /// <param name="outerRect">The outer bounding rectangle.</param>
        /// <returns>A <see cref="Rect"/> representing the bounded drag location.</returns>
        /// <remarks>
        /// Ensures the drag rectangle stays within the specified outer bounds by adjusting position and dimensions.
        /// </remarks>
        private static Rect GetDraggedRectLocation(double x1, double y1, double x2, double y2, Rect outerRect)
        {
            double width = Math.Abs(x1 - x2);
            double height = Math.Abs(y1 - y2);

            double boundedX = Math.Max(CheckBounds(Math.Min(x1, x2), width, outerRect.X, outerRect.Width), outerRect.X);
            double boundedY = Math.Max(CheckBounds(Math.Min(y1, y2), height, outerRect.Y, outerRect.Height), outerRect.Y);
            double boundedWidth = Math.Min(width, outerRect.Width);
            double boundedHeight = Math.Min(height, outerRect.Height);

            return new Rect(boundedX, boundedY, boundedWidth, boundedHeight);
        }

        /// <summary>
        /// Removes offset from rectangle coordinates.
        /// </summary>
        /// <param name="rect">The rectangle to adjust.</param>
        /// <param name="clip">The offset to remove.</param>
        private static void RemoveOffset(Rect rect, ChartEventLocation clip)
        {
            rect.X -= clip.X;
            rect.Y -= clip.Y;
        }

        /// <summary>
        /// Determines if a point is within any of the provided selection rectangles.
        /// </summary>
        /// <param name="point">The point to check.</param>
        /// <param name="x_AxisOffset">The X axis offset to apply.</param>
        /// <param name="y_AxisOffset">The Y axis offset to apply.</param>
        /// <param name="rectCollection">The collection of selection rectangles.</param>
        /// <returns><see langword="true"/> if the point is within any rectangle; otherwise, <see langword="false"/>.</returns>
        private static bool IsPointSelect(Point point, double x_AxisOffset, double y_AxisOffset, List<Rect> rectCollection)
        {
            ChartEventLocation location = point.SymbolLocations.Count > 0 ? point.SymbolLocations[0] : null!;
            foreach (Rect rect in rectCollection)
            {
                if (rect is not null && location is not null && ChartHelper.WithInBounds(location.X + x_AxisOffset, location.Y + y_AxisOffset, rect))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Changes the cursor style for the selection rectangle element.
        /// </summary>
        /// <param name="isResize">Indicates whether the cursor is for resizing.</param>
        /// <param name="rectElement">The SVG selection rectangle element.</param>
        /// <param name="cursorStyle">The cursor style to apply if resizing.</param>
        /// <remarks>
        /// Sets the cursor to the specified style if <paramref name="isResize"/> is <see langword="true"/>;
        /// otherwise, sets it to "move".
        /// </remarks>
        private static void ChangeCursorStyle(bool isResize, SvgSelectionRect rectElement, string cursorStyle)
        {
            cursorStyle = isResize ? cursorStyle : MOVE_CONTAINS;
            if (rectElement is not null)
            {
                _ = rectElement.ChangeCursorAsync(cursorStyle);
            }
        }

        /// <summary>
        /// Checks and adjusts bounds to ensure a rectangle stays within specified limits.
        /// </summary>
        /// <param name="start">The starting coordinate.</param>
        /// <param name="size">The size dimension.</param>
        /// <param name="min">The minimum boundary.</param>
        /// <param name="max">The maximum boundary extent.</param>
        /// <returns>The adjusted starting coordinate.</returns>
        /// <remarks>
        /// Constrains the starting position to ensure the element (defined by start + size)
        /// remains within the boundaries defined by min and (min + max).
        /// </remarks>
        private static double CheckBounds(double start, double size, double min, double max)
        {
            if (start < min)
            {
                start = min;
            }
            else if ((start + size) > (max + min))
            {
                start = max + min - size;
            }

            return start;
        }

        /// <summary>
        /// Extracts the numeric index from a drag element identifier.
        /// </summary>
        /// <param name="id">The element identifier string.</param>
        /// <returns>The extracted index, or -1 if the identifier is invalid.</returns>
        /// <remarks>
        /// Parses element IDs in the format containing "_drag_" followed by index information.
        /// </remarks>
        private static int GetIndex(string id)
        {
            if (id is null)
            {
                return -1;
            }
            bool isValidID = id.Contains("_drag_", StringComparison.InvariantCulture);

            return isValidID ? int.Parse(id.Split("_drag_")[1].Split("_")[1], null) : -1;
        }

        /// <summary>
        /// Determines if the specified identifier represents a drag rectangle.
        /// </summary>
        /// <param name="id">The element identifier to check.</param>
        /// <returns><see langword="true"/> if the identifier represents a drag rectangle; otherwise, <see langword="false"/>.</returns>
        private static bool IsDragRect(string id)
        {
            return id is not null && id.Contains("_ej2_drag_rect", StringComparison.InvariantCulture);
        }

        /// <summary>
        /// Constructs or extends an SVG path for lasso selection.
        /// </summary>
        /// <param name="startX">The starting X coordinate.</param>
        /// <param name="startY">The starting Y coordinate.</param>
        /// <param name="endX">The ending X coordinate.</param>
        /// <param name="endY">The ending Y coordinate.</param>
        /// <param name="id">The path identifier.</param>
        /// <remarks>
        /// Creates a new path if one doesn't exist, or extends an existing path by adding line segments.
        /// Only operates when _dragging is active.
        /// </remarks>
        private void GetPath(double startX, double startY, double endX, double endY, string id)
        {
            if (_dragging)
            {
                bool isPath = _lassoPaths.Count > 0 && _lassoPaths.TryGetValue(id, out string? _) && !string.IsNullOrEmpty(_lassoPaths[id]) && !_lassoPaths[id].Contains('Z', StringComparison.InvariantCulture);

                _lassoPaths[id] = isPath ? (_lassoPaths[id] + " L" + endX.ToString(culture) + SPACE + endY.ToString(culture)) : ("M " + startX.ToString(culture) + SPACE + startY.ToString(culture));
            }
        }

        /// <summary>
        /// Removes dragged selection elements when the close icon is clicked.
        /// </summary>
        /// <param name="events">The mouse event arguments containing the target element information.</param>
        /// <remarks>
        /// Handles cleanup of drag rectangles, filters, and selected points when a user clicks
        /// the close icon. Supports both single and multi-selection modes.
        /// </remarks>
        private void RemoveDraggedElements(ChartInternalMouseEventArgs events)
        {
            if (!ShouldRemoveDraggedElement(events))
            {
                return;
            }
            if (_chartInstance is null)
            {
                return;
            }
            bool isSelectedvalues = true;

            if (_chartInstance.AllowMultiSelection)
            {
                isSelectedvalues = HandleMultiSelectionRemoval(events);
            }
            else
            {
                HandleSingleSelectionRemoval();
            }

            FinalizeRemoval(isSelectedvalues, events);
        }

        /// <summary>
        /// Determines whether the clicked target should trigger removal of dragged elements.
        /// </summary>
        /// <param name="events">Mouse event data to inspect.</param>
        /// <returns><see langword="true"/> when the event target corresponds to a close icon and is not a move event; otherwise <see langword="false"/>.</returns>
        private bool ShouldRemoveDraggedElement(ChartInternalMouseEventArgs events)
        {
            return !string.IsNullOrEmpty(_closeIconId)
                && events.Target is not null
                && events.Target.Contains(_closeIconId, StringComparison.InvariantCulture)
                && !events.Type.Contains(MOVE_CONTAINS, StringComparison.InvariantCulture);
        }

        /// <summary>
        /// Handles the multi-selection removal flow.
        /// </summary>
        /// <param name="events">Mouse event data for the close icon click.</param>
        /// <returns><see langword="true"/> if no other selections remain; otherwise <see langword="false"/>.</returns>
        private bool HandleMultiSelectionRemoval(ChartInternalMouseEventArgs events)
        {
            int index = GetIndex(events.Target);
            RemoveIndexedElements(index);

            if (_chartInstance?._parentRect?.RectsReference.Count == 0)
            {
                _dragRectArray.Clear();
                _filterArray.Clear();
            }

            return ProcessSelectionMode(index);
        }

        /// <summary>
        /// Removes the indexed entries from internal collections.
        /// </summary>
        /// <param name="index">Index of the dragged element.</param>
        private void RemoveIndexedElements(int index)
        {
            _ = _dragRectArray.Remove(_draggedRect + index);
            _ = _filterArray.Remove(_draggedRect + index);
            _ = _closeCircleArray.Remove(_closeIconId + CIRCLE_SUFFIX + index);
        }

        /// <summary>
        /// Processes removal depending on the current selection mode.
        /// </summary>
        /// <param name="index">Index of the removed element.</param>
        /// <returns>True when no selected values remain after processing; otherwise false.</returns>
        private bool ProcessSelectionMode(int index)
        {
            if (CurrentMode == ChartSelectionMode.Lasso)
            {
                return HandleLassoModeRemoval(index);
            }

            if (_filterArray.Count != 0)
            {
                return HandleFilterArrayRemoval();
            }

            CalculateDragSelectedElements(new Rect(0, 0, 0, 0), true);
            return true;
        }

        /// <summary>
        /// Handles removal logic specific to lasso selection.
        /// </summary>
        /// <param name="index">Index of the lasso path removed.</param>
        /// <returns><see langword="true"/> if no selected values remain; otherwise <see langword="false"/>.</returns>
        private bool HandleLassoModeRemoval(int index)
        {
            bool isSelectedvalues = true;

            for (int s = 0; s < _chartInstance?._seriesContainer?.Renderers.Count; s++)
            {
                if (_selectedLassoPoints.TryGetValue((index, s), out List<Point>? selectedPoints))
                {
                    selectedPoints.ForEach(x => x.IsSelected = false);
                    selectedPoints.Clear();
                }

                foreach (List<Point> data in _selectedLassoPoints.Values)
                {
                    if (data.Count != 0)
                    {
                        isSelectedvalues = false;
                        data.ForEach(x => x.IsSelected = true);
                    }
                }
            }

            CalculateDragSelectedElements(_dragRect ?? new Rect(0, 0, 0, 0), true);
            return isSelectedvalues;
        }

        /// <summary>
        /// Handles removal when filtered rectangles exist.
        /// </summary>
        /// <returns><see langword="true"/> if no selected values remain; otherwise <see langword="false"/>.</returns>
        private bool HandleFilterArrayRemoval()
        {
            bool isSelectedvalues = true;
            List<Rect> items = [.. _filterArray.Values];

            foreach (Rect item in items)
            {
                if (item is not null)
                {
                    isSelectedvalues = false;
                    CalculateDragSelectedElements(item, true);
                }
            }

            return isSelectedvalues;
        }

        /// <summary>
        /// Handles cleanup for single selection removal.
        /// </summary>
        private void HandleSingleSelectionRemoval()
        {
            if (_chartInstance is null)
            {
                return;
            }
            _ = SfBaseComponent.InvokeVoidAsync(_chartInstance._chartJsModule, _chartInstance._chartJsInProcessModule, Constants.DragRemove);
            CalculateDragSelectedElements(_dragRect ?? new Rect(0, 0, 0, 0), true);
            _ = RemoveSelectedElementsAsync();
        }

        /// <summary>
        /// Finalizes removal steps and updates references.
        /// </summary>
        /// <param name="isSelectedvalues">Whether selected values remain after removal.</param>
        /// <param name="events">Mouse event data used to determine element references.</param>
        private void FinalizeRemoval(bool isSelectedvalues, ChartInternalMouseEventArgs events)
        {
            _ = BlurEffectAsync();
            if (_chartInstance is null)
            {
                return;
            }
            if (!_chartInstance.AllowMultiSelection || isSelectedvalues)
            {
                _ = SfBaseComponent.InvokeVoidAsync(_chartInstance._chartJsModule!, _chartInstance._chartJsInProcessModule!, Constants.DragRemove);
                _rectPoints = null!;
            }

            RemoveElementReference(events);
        }

        /// <summary>
        /// Removes references to the SVG elements related to the clicked close icon.
        /// </summary>
        /// <param name="events">Mouse event data that contains the target id.</param>
        private void RemoveElementReference(ChartInternalMouseEventArgs events)
        {
            string refId = events.Target.Replace("closecircle", "rect", StringComparison.InvariantCulture);

            if (CurrentMode != ChartSelectionMode.Lasso)
            {
                RemoveRectReference(refId);
            }
            else
            {
                RemovePathReference(refId);
            }
        }

        /// <summary>
        /// Removes rectangle references from the parent rect container.
        /// </summary>
        /// <param name="refId">The reference id of the rectangle to remove.</param>
        private void RemoveRectReference(string refId)
        {
            if (_chartInstance is null)
            {
                return;
            }
            if (!_chartInstance.AllowMultiSelection)
            {
                if (_chartInstance._parentRect?.RectsReference.Count > 0)
                {
                    _chartInstance._parentRect.RemoveCurrentElement(_chartInstance._parentRect.RectsReference.First().Value);
                }
            }
            else if (_chartInstance._parentRect is not null
                     && _chartInstance._parentRect.RectsReference.TryGetValue(refId, out SvgSelectionRect? value))
            {
                _chartInstance._parentRect.RemoveCurrentElement(value);
            }
        }

        /// <summary>
        /// Removes path references from the parent rect container.
        /// </summary>
        /// <param name="refId">The reference id of the path to remove.</param>
        private void RemovePathReference(string refId)
        {
            if (_chartInstance is null)
            {
                return;
            }
            if (!_chartInstance.AllowMultiSelection)
            {
                if (_chartInstance._parentRect?.RectsReference.Count > 0)
                {
                    _chartInstance._parentRect.RemoveCurrentElement(_chartInstance._parentRect.PathsReference.First().Value);
                }
            }
            else if (_chartInstance._parentRect is not null
                     && _chartInstance._parentRect.PathsReference.TryGetValue(refId, out SvgSelectionPath? value))
            {
                _chartInstance._parentRect.RemoveCurrentElement(value);
            }
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Initiates drag selection operation when mouse down occurs.
        /// </summary>
        /// <param name="mouseDownX">The X coordinate of the mouse down position.</param>
        /// <param name="mouseDownY">The Y coordinate of the mouse down position.</param>
        /// <param name="events">The chart mouse event arguments.</param>
        /// <remarks>
        /// Sets up the initial state for drag operations based on the selection mode and mouse position.
        /// Handles both single and multi-selection scenarios.
        /// </remarks>
        internal void DragStart(double mouseDownX, double mouseDownY, ChartInternalMouseEventArgs events)
        {
            if (_chartInstance is null)
            {
                return;
            }
            string mode = _chartInstance.SelectionMode.ToString();
            CurrentMode = _chartInstance.SelectionMode;
            _dragging = _lassoDownCompleted = (mode.Contains(DRAG_CONTAINS, StringComparison.InvariantCulture) || mode == "Lasso") && (_chartInstance._isDoubleTap || !_chartInstance._isTouch);

            if (_dragging)
            {
                InitializeDragOperation(events);
            }

            if (CurrentMode == ChartSelectionMode.Lasso && _chartInstance._seriesContainer is not null)
            {
                foreach (ChartSeriesRenderer seriesRenderer in _chartInstance._seriesContainer.Renderers.Cast<ChartSeriesRenderer>())
                {
                    if (seriesRenderer.Series is not null && seriesRenderer.Series.Visible)
                    {
                        seriesRenderer.Points?.Where(x => !_chartInstance.AllowMultiSelection).ToList().ForEach(y => y.IsSelected = false);
                    }
                }
            }
            else
            {
                HandleRectangleModeInitialization(events, mouseDownX, mouseDownY);
            }
        }

        /// <summary>
        /// Determines whether a point is within the specified circle.
        /// </summary>
        /// <param name="x">The X coordinate of the point to test.</param>
        /// <param name="y">The Y coordinate of the point to test.</param>
        /// <param name="circle">The circle options to test against.</param>
        /// <returns><see langword="true"/> if the point is within the circle; otherwise <see langword="false"/>.</returns>
        internal static bool WithInCircle(double x, double y, CircleOptions circle)
        {
            double dx = 0;
            double dy = 0;
            if (circle is not null)
            {
                dx = x - Convert.ToDouble(circle.Cx, CultureInfo.InvariantCulture);
                dy = y - Convert.ToDouble(circle.Cy, CultureInfo.InvariantCulture);
            }

            return (dx * dx) + (dy * dy) <= Convert.ToDouble(circle?.R, null) * Convert.ToDouble(circle?.R, null);
        }
        #endregion
    }

    /// <summary>
    /// Options used to describe current selection visuals and state.
    /// </summary>
    public class SelectionOptions
    {
        /// <summary>
        /// Gets or sets the identifier for this selection instance.
        /// </summary>
        /// <value>The unique identifier. Default: "Default_Blazor_Selection".</value>
        public string Id { get; set; } = "Default_Blazor_Selection";

        /// <summary>
        /// Gets or sets the rectangle representing the drag selection.
        /// </summary>
        /// <value>The selection rectangle.</value>
        public Rect DragRect { get; set; } = new Rect();

        /// <summary>
        /// Gets or sets the stroke color for selection shapes.
        /// </summary>
        /// <value>The stroke color as a CSS string.</value>
        public string Stroke { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the stroke width for selection shapes.
        /// </summary>
        /// <value>The stroke width as a CSS string.</value>
        public string StrokeWidth { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the fill for selection shapes.
        /// </summary>
        /// <value>The fill as a CSS string. Default: "transparent".</value>
        public string Fill { get; set; } = "transparent";

        /// <summary>
        /// Gets or sets a value indicating whether the current selection is a lasso (freeform) selection.
        /// </summary>
        /// <value><see langword="true"/> if lasso; otherwise <see langword="false"/>.</value>
        public bool IsLasso { get; set; }

        /// <summary>
        /// Gets or sets the SVG path for lasso selection.
        /// </summary>
        /// <value>The path data string.</value>
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets closing options for shape completion, such as the closing circle.
        /// </summary>
        /// <value>A <see cref="CloseOptions"/> instance describing close visuals.</value>
        public CloseOptions Close { get; set; } = new CloseOptions();
    }

    /// <summary>
    /// Options that describe the visual elements used when closing a lasso/path selection.
    /// </summary>
    public class CloseOptions
    {
        /// <summary>
        /// Gets or sets the path options used for the close handle.
        /// </summary>
        /// <value>A <see cref="PathOptions"/> instance; may be <c>null</c>.</value>
        public PathOptions Path { get; set; } = null!;

        /// <summary>
        /// Gets or sets the circle options used for the close handle.
        /// </summary>
        /// <value>A <see cref="CircleOptions"/> instance; may be <c>null</c>.</value>
        public CircleOptions Circle { get; set; } = null!;
    }
}