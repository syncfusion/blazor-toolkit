using System.Runtime.InteropServices;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Manages user selection interactions for chart elements, including drag selection, lasso selection, and data point highlighting.
    /// </summary>
    /// <remarks>
    /// This class handles mouse events, drag rectangles, lasso selection paths, and applies visual selection styles to chart series and data points.
    /// It supports multiple selection modes: Single, Multiple, DragXY, DragX, DragY, and Lasso.
    /// </remarks>
    internal partial class Selection : BaseSelection
    {
        #region Constants
        private const string ROOT_STYLE_ID = "_ej2_chart_selection";
        private const string DESELECT_STYLE_ID = "_ej2_deselected";
        private const string CLOSE_ICON_SUFFIX = "_ej2_drag_close";
        private const string DRAG_RECT_SUFFIX = "_ej2_drag_rect_";
        private const string HIGHLIGHT_STYLE_PREFIX = "_ej2_chart_highlight";
        private const string SERIES_PREFIX = "_Series_";
        private const string POINT_PREFIX = "_Point_";
        #endregion

        #region Fields

        private SfChart? _chartInstance;
        private StringComparison _stringCulture = StringComparison.InvariantCulture;
        private string _closeIconId = string.Empty;
        private string? _draggedRect;
        private List<SvgClass> _previousSelectedElements = [];
        private bool _dragging;
        private bool _lassoDownCompleted;
        private Rect _rectPoints = new();
        private Dictionary<string, Rect> _dragRectArray = [];
        private Dictionary<string, Rect> _filterArray = [];
        private Dictionary<string, string> _lassoPaths = [];
        private Dictionary<string, CircleOptions> _closeCircleArray = [];
        private int _targetIndex;
        private bool _rectGrabbing;
        private bool _isdrawRect = true;
        private bool _resizing;
        private Rect? _dragRect;
        private int _count = -1;
        private Dictionary<(int, int), List<Point>> _selectedLassoPoints = [];
        private int _resizeMode;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the collection of chart series renderers.
        /// </summary>
        /// <value>A list of <see cref="IChartElementRenderer"/> objects representing series in the chart.</value>
        protected List<IChartElementRenderer> Series { get; set; } = [];

        /// <summary>
        /// Gets or sets the list of data indexes that should be highlighted.
        /// </summary>
        /// <value>A collection of <see cref="ChartSelectedDataIndex"/> objects.</value>
        internal List<ChartSelectedDataIndex> HighlightDataIndexes { get; set; } = [];

        /// <summary>
        /// Gets or sets the current selection mode.
        /// </summary>
        /// <value>The <see cref="ChartSelectionMode"/> currently active for this selection manager.</value>
        internal ChartSelectionMode CurrentMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the selection is in series mode.
        /// </summary>
        /// <value><see langword="true"/> if series-level selection is active; otherwise <see langword="false"/>.</value>
        internal bool IsSeriesMode { get; set; }

        /// <summary>
        /// Gets or sets the list of currently selected data indexes.
        /// </summary>
        /// <value>A collection of <see cref="ChartSelectedDataIndex"/> objects representing selected data points.</value>
        internal List<ChartSelectedDataIndex> SelectedDataIndexes { get; set; } = [];

        /// <summary>
        /// Gets the series clip rectangle bounds.
        /// </summary>
        /// <value>A <see cref="Rect"/> representing the clipping area for chart series, or null if not available.</value>
        private Rect SeriesClipRect => _chartInstance?._axisContainer?.AxisLayout?.SeriesClipRect ?? null!;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Selection"/> class.
        /// </summary>
        /// <param name="chart">The parent chart instance. Required.</param>
        /// <param name="wireEvents">If <see langword="true"/>, subscribes to mouse events; otherwise, event subscription is deferred. Default: <see langword="true"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="chart"/> is <see langword="null"/>.</exception>
        internal Selection(SfChart chart, bool wireEvents = true)
        {
            _chartInstance = chart;
            InitPrivateVariables();
            if (wireEvents)
            {
                AddEventListener();
            }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Subscribes the selection manager to chart mouse events.
        /// </summary>
        private void AddEventListener()
        {
            if (_chartInstance is null)
            {
                return;
            }
            _chartInstance.MouseMove += MouseMoveHandler;
            _chartInstance.MouseDown += MouseDownHandler;
            _chartInstance.MouseUp += MouseUpHandler;
            _chartInstance.MouseCancel += MouseCancelHandler;
        }

        /// <summary>
        /// Sends the current selection to JavaScript for rendering.
        /// </summary>
        private async Task SelectDataIndexAsync()
        {
            try
            {
                if (_chartInstance is null)
                {
                    return;
                }
                await SfBaseComponent.InvokeVoidAsync(_chartInstance._chartJsModule, _chartInstance._chartJsInProcessModule, Constants.SelectDataIndex, [.. new object[] { _chartInstance._dataId, SelectedDataIndexes.Concat(_chartInstance.SelectedDataIndexes).ToList() }]).ConfigureAwait(true);
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine(exception);
            }
        }

        /// <summary>
        /// Handles mouse down events to initiate selection.
        /// </summary>
        /// <param name="source">The event source.</param>
        /// <param name="args">The mouse event arguments.</param>
        private void MouseDownHandler(object? source, ChartInternalMouseEventArgs args)
        {
            DragRectFromArray(args);
            if (_chartInstance is null || _chartInstance._isPointMouseDown || _chartInstance.SelectionMode == ChartSelectionMode.None || _chartInstance._isChartDrag)
            {
                return;
            }

            if (_chartInstance._isDoubleTap || !_chartInstance._isTouch || _rectPoints is not null)
            {
                DragStart(_chartInstance._mouseDownX, _chartInstance._mouseDownY, args);
            }
        }

        /// <summary>
        /// Handles mouse up events to complete selection.
        /// </summary>
        /// <param name="source">The event source.</param>
        /// <param name="args">The mouse event arguments.</param>
        private void MouseUpHandler(object? source, ChartInternalMouseEventArgs args)
        {
            CloseIconIdFromArray(args);
            _ = CompleteSelectionAsync(args);
            _ = SfBaseComponent.InvokeVoidAsync(_chartInstance?._chartJsModule, _chartInstance?._chartJsInProcessModule, "dragEnd");
        }

        /// <summary>
        /// Handles mouse cancel events to abort selection.
        /// </summary>
        /// <param name="source">The event source.</param>
        /// <param name="args">The mouse event arguments.</param>
        private void MouseCancelHandler(object? source, ChartInternalMouseEventArgs args)
        {
            _ = CompleteSelectionAsync(args);
        }

        /// <summary>
        /// Initializes private variables and style identifiers.
        /// </summary>
        private void InitPrivateVariables()
        {
            if (_chartInstance is null)
            {
                return;
            }
            int pointMax = 0;
            StyleId = _chartInstance.ID + ROOT_STYLE_ID;
            Unselected = _chartInstance.ID + DESELECT_STYLE_ID;
            _closeIconId = _chartInstance.ID + CLOSE_ICON_SUFFIX;
            _draggedRect = _chartInstance.ID + DRAG_RECT_SUFFIX;
            SelectedDataIndexes.Clear();
            _rectPoints = null!;
            IsSeriesMode = _chartInstance.SelectionMode == ChartSelectionMode.Series;
            _count = -1;
            _dragRectArray.Clear();
            _filterArray.Clear();
            _previousSelectedElements.Clear();
            _chartInstance._seriesContainer?.Renderers.ForEach(item => pointMax = Math.Max(pointMax, ((ChartSeriesRenderer)item).Points?.Count ?? 0));
            _selectedLassoPoints?.Clear();
            ReqPatterns?.Clear();
            Series = _chartInstance._seriesContainer?.Renderers ?? null!;
            CurrentMode = _chartInstance.SelectionMode;
        }
        #endregion

        #region Protected Methods

        /// <summary>
        /// Handles mouse move events for drag selection and lasso tracking.
        /// </summary>
        /// <param name="source">The event source.</param>
        /// <param name="args">The mouse event arguments.</param>
        protected void MouseMoveHandler(object? source, ChartInternalMouseEventArgs args)
        {
            DragSelectionProcess(args);
        }

        /// <summary>
        /// Applies series-level selection and highlight styles.
        /// </summary>
        protected void SeriesStyles()
        {
            if (_chartInstance is null)
            {
                return;
            }
            SelectionPattern selectionPattern = _chartInstance.SelectionPattern;
            SelectionPattern highlightPattern = _chartInstance.HighlightPattern;
            bool isHighlight = StyleId is not null && StyleId.Contains("highlight", StringComparison.Ordinal);
            bool hasHighlightColor = !string.IsNullOrEmpty(_chartInstance.HighlightColor);

            if ((isHighlight && hasHighlightColor) || selectionPattern != SelectionPattern.None || highlightPattern != SelectionPattern.None)
            {
                foreach (ChartSeriesRenderer seriesRenderer in Series.Cast<ChartSeriesRenderer>())
                {
                    string fill = FindPattern(isHighlight && hasHighlightColor ? _chartInstance.HighlightColor : seriesRenderer.Interior ?? null!, seriesRenderer.Index, StyleId is not null && StyleId.Contains("highlight", StringComparison.Ordinal) ? highlightPattern : selectionPattern, seriesRenderer.Series?.Opacity ?? 1);
                    string pattern = "{" + "fill" + ":" + fill + "}";
                    pattern = pattern.Contains("None", _stringCulture) ? "{}" : pattern;
                    InnerHTML += !string.IsNullOrEmpty(seriesRenderer.Series?.SelectionStyle) ? string.Empty : "." + (!string.IsNullOrEmpty(seriesRenderer.Series?.SelectionStyle) ? seriesRenderer.Series.SelectionStyle : StyleId + "_series" + seriesRenderer.Index + "," + "." + StyleId + "_series_" + seriesRenderer.Index + "> *") + pattern;
                }
            }

            double unSelectOpacity = 0.3;
            if (_chartInstance._selectionModule is null && _chartInstance.SelectionMode == ChartSelectionMode.None && !string.IsNullOrEmpty(_chartInstance.HighlightColor))
            {
                unSelectOpacity = 1;
            }
            InnerHTML += "." + Unselected + (_chartInstance._legendRenderer?.Legend is not null && _chartInstance._legendRenderer.Legend.EnableHighlight && _chartInstance._legendRenderer.Legend.ToggleVisibility ? ":not([id*=_chart_legend_shape_])" : "") + " { opacity:" + unSelectOpacity.ToString(culture) + ";} ";

            if (isHighlight || (_chartInstance._legendRenderer?.Legend is not null && _chartInstance._legendRenderer.Legend.EnableHighlight))
            {
                foreach (ChartSeriesRenderer seriesRenderer in Series.Cast<ChartSeriesRenderer>())
                {
                    if (seriesRenderer.IsPathSeries())
                    {
                        double width = Math.Max(seriesRenderer.Series?.Width ?? 0, seriesRenderer.Series?.Border.Width ?? 0) + 1;
                        InnerHTML += "." + _chartInstance.ID + "_ej2_chart_highlight_series" + seriesRenderer.Index + (_chartInstance._legendRenderer?.Legend is not null && _chartInstance._legendRenderer.Legend.EnableHighlight && _chartInstance._legendRenderer.Legend.ToggleVisibility ? ":not([id*=_chart_legend_shape_])" : "") + "{ stroke-width: " + width.ToString(culture) + "px;}";
                    }
                }
            }

            AppendStylesBasedOnContent();
        }

        /// <summary>
        /// Appends computed styles to appropriate style instances.
        /// </summary>
        private void AppendStylesBasedOnContent()
        {
            if (string.IsNullOrEmpty(InnerHTML) || _chartInstance is null)
            {
                return;
            }
            if (InnerHTML.Contains("selection", _stringCulture))
            {
                _chartInstance._selectionStyleInstance?.AppendStyleElement(InnerHTML);
                InnerHTML = string.Empty;
            }
            else if (InnerHTML.Contains("highlight", _stringCulture))
            {
                _chartInstance._highlightStyleInstance?.AppendStyleElement(InnerHTML);
                InnerHTML = string.Empty;
            }
            else
            {
                _chartInstance._selectionStyleInstance?.AppendStyleElement(InnerHTML);
                _chartInstance._highlightStyleInstance?.AppendStyleElement(InnerHTML);
                InnerHTML = string.Empty;
            }
        }

        /// <summary>
        /// Triggers a chart selection on the JavaScript side.
        /// </summary>
        /// <param name="index">The data index to select.</param>
        protected async Task SelectionChartAsync(ChartSelectedDataIndex index)
        {
            try
            {
                if (_chartInstance is null)
                {
                    return;
                }
                await SfBaseComponent.InvokeVoidAsync(_chartInstance._chartJsModule, _chartInstance._chartJsInProcessModule, "selectionChart", [.. new object[] { _chartInstance._dataId, index }]).ConfigureAwait(true);
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine(exception);
            }
        }

        /// <summary>
        /// Applies blur/opacity effect to non-selected chart elements.
        /// </summary>
        protected async Task BlurEffectAsync()
        {
            try
            {
                if (_chartInstance is null)
                {
                    return;
                }
                await SfBaseComponent.InvokeVoidAsync(_chartInstance._chartJsModule, _chartInstance._chartJsInProcessModule, "invokeBlurEffect", [.. new object[] { _chartInstance._dataId }]).ConfigureAwait(true);
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine(exception);
            }
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Invokes selection styling and data index updates.
        /// </summary>
        internal void InvokeSelection()
        {
            InitPrivateVariables();
            SeriesStyles();
            if (!CurrentMode.ToString().Contains("Drag", _stringCulture))
            {
                _ = SelectDataIndexAsync();
            }
        }

        /// <summary>
        /// Applies or refreshes selection/highlight styles.
        /// </summary>
        /// <param name="isSelection">If <see langword="true"/>, applies selection styles; otherwise applies highlight styles.</param>
        internal void CallSeriesStyles(bool isSelection = true)
        {
            StyleId = isSelection ? _chartInstance?.ID + ROOT_STYLE_ID : _chartInstance?.ID + HIGHLIGHT_STYLE_PREFIX;
            SeriesStyles();
        }

        /// <summary>
        /// Triggers a redraw of the current selection.
        /// </summary>
        internal async Task RedrawSelectionAsync()
        {
            try
            {
                if (_chartInstance is null)
                {
                    return;
                }
                await SfBaseComponent.InvokeVoidAsync(_chartInstance._chartJsModule, _chartInstance._chartJsInProcessModule, "redrawSelection", [.. new object[] { _chartInstance._dataId }]).ConfigureAwait(true);
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine(exception);
            }
        }

        /// <summary>
        /// Removes all selection styling from the chart.
        /// </summary>
        internal async Task RemoveSelectionStylesAsync()
        {
            try
            {
                if (_chartInstance is null)
                {
                    return;
                }
                await SfBaseComponent.InvokeVoidAsync(_chartInstance._chartJsModule, _chartInstance._chartJsInProcessModule, "removeSelectionStyles", [.. new object[] { _chartInstance._dataId }]).ConfigureAwait(true);
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine(exception);
            }
        }

        /// <summary>
        /// Removes visual selection indicators from chart elements.
        /// </summary>
        internal async Task RemoveSelectedElementsAsync()
        {
            try
            {
                if (_chartInstance is null)
                {
                    return;
                }
                await SfBaseComponent.InvokeVoidAsync(_chartInstance._chartJsModule, _chartInstance._chartJsInProcessModule, "invokeRemoveSelectedElements", [.. new object[] { _chartInstance._dataId }]).ConfigureAwait(true);
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine(exception);
            }
        }

        /// <summary>
        /// Triggers the selection complete event with selected point values.
        /// </summary>
        /// <param name="selectedPointValues">The collection of selected point coordinates.</param>
        internal void TriggerSelectionComplete(List<PointXY> selectedPointValues)
        {
            _chartInstance?.OnSelectionChanged?.Invoke(new SelectionCompleteEventArgs() { SelectedDataValues = selectedPointValues, Cancel = false, Name = Constants.OnSelectionChanged });
        }

        /// <summary>
        /// Handles changes to the selection mode.
        /// </summary>
        internal void ChartSelectionModeChanged()
        {
            if (_chartInstance is null)
            {
                return;
            }
            _chartInstance._shouldAnimateSeries = false;
            if (!_chartInstance.SelectionMode.ToString().Contains("Drag", _stringCulture))
            {
                OnPropertyChanged();
            }
            else
            {
                ClearDraggedRects();
                OnPropertyChanged();
                _chartInstance._parentRect?.ClearElements();
            }
        }

        /// <summary>
        /// Updates the selection state and redraws.
        /// </summary>
        internal void OnPropertyChanged()
        {
            if (_chartInstance is null)
            {
                return;
            }
            _chartInstance._shouldAnimateSeries = false;
            CurrentMode = _chartInstance.SelectionMode;
            StyleId = _chartInstance.ID + "_ej2_chart_selection";
            _ = RedrawSelectionAsync();
        }

        /// <summary>
        /// Clears all drag rectangles and associated state.
        /// </summary>
        /// <param name="isClearSelection">If <see langword="true"/>, clears selection state; otherwise preserves it.</param>
        internal void ClearDraggedRects([Optional] bool isClearSelection)
        {
            if (_dragRect is not null && _closeCircleArray.Count > 0)
            {
                _dragRect.Height = _dragRect.Width = 0;
                if (!isClearSelection)
                {
                    CalculateDragSelectedElements(_dragRect, false);
                }
            }
            _dragRectArray.Clear();
            _closeCircleArray.Clear();
            _filterArray.Clear();
        }

        /// <summary>
        /// Updates the drag rectangle reference from the filter array based on mouse coordinates.
        /// </summary>
        /// <param name="args">The mouse event arguments containing current mouse position.</param>
        internal void DragRectFromArray(ChartInternalMouseEventArgs args)
        {
            if (!string.IsNullOrEmpty(args.Target) && !args.Target.Contains(_chartInstance?.ID + SERIES_PREFIX, StringComparison.InvariantCulture) && !args.Target.Contains(_chartInstance?.ID + POINT_PREFIX, StringComparison.InvariantCulture) && args.Target.Contains(_chartInstance?.ID + "_", StringComparison.InvariantCulture) && _dragRectArray is not null)
            {
                foreach (KeyValuePair<string, Rect> rect in _filterArray)
                {
                    if (ChartHelper.WithInBounds(args.MouseX - SeriesClipRect.X, args.MouseY - SeriesClipRect.Y, rect.Value))
                    {
                        args.Target = rect.Key;
                        break;
                    }
                }
            }
            else
            {
                args.Target = _chartInstance?.ID + "_";
            }
        }

        /// <summary>
        /// Updates the close icon reference based on mouse proximity to close circles.
        /// </summary>
        /// <param name="args">The mouse event arguments containing current mouse position.</param>
        internal void CloseIconIdFromArray(ChartInternalMouseEventArgs args)
        {
            if (!string.IsNullOrEmpty(args.Target) && !args.Target.Contains(_chartInstance?.ID + "_Series_", StringComparison.InvariantCulture) && !args.Target.Contains(_chartInstance?.ID + POINT_PREFIX, StringComparison.InvariantCulture) && args.Target.Contains(_chartInstance?.ID + "_", StringComparison.InvariantCulture) && _closeCircleArray is not null)
            {
                foreach (KeyValuePair<string, CircleOptions> circle in _closeCircleArray)
                {
                    if (WithInCircle(args.MouseX, args.MouseY, circle.Value))
                    {
                        args.Target = circle.Key;
                        break;
                    }
                }
            }
            else
            {
                args.Target = _chartInstance?.ID + "_";
            }
        }

        /// <summary>
        /// Clears all selection state and visual indicators.
        /// </summary>
        internal void PerformClearSelection()
        {
            if (CurrentMode.ToString().Contains("Drag", _stringCulture) || CurrentMode == ChartSelectionMode.Lasso)
            {
                if (_chartInstance?._parentRect?.RectsReference.Count > 0 || _chartInstance?._parentRect?.PathsReference.Count > 0)
                {
                    _ = SfBaseComponent.InvokeVoidAsync(_chartInstance._chartJsModule!, _chartInstance._chartJsInProcessModule!, Constants.DragRemove);
                    _ = RemoveSelectedElementsAsync();
                    _ = BlurEffectAsync();
                    ClearDraggedRects(true);

                    if (CurrentMode == ChartSelectionMode.Lasso)
                    {
                        _selectedLassoPoints = [];
                        if (_chartInstance._seriesContainer is not null)
                        {
                            foreach (ChartSeriesRenderer series in _chartInstance._seriesContainer.Renderers.Cast<ChartSeriesRenderer>())
                            {
                                series?.Points?.ForEach(point => point.IsSelected = false);
                            }
                        }
                    }
                    else
                    {
                        _rectPoints = null!;
                    }
                    _chartInstance._parentRect.ClearElements();
                }
            }
            else
            {
                _ = RemoveSelectedElementsAsync();
                _ = BlurEffectAsync();
            }

            if (_chartInstance?.SelectedDataIndexes.Count > 0)
            {
                _chartInstance.SelectedDataIndexes = [];
            }
        }
        #endregion
    }
}