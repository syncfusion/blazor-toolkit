using Microsoft.JSInterop;
using Syncfusion.Blazor.Toolkit.Charts.Internal;
using System.ComponentModel;
using System.Text.Json;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Partial class that contains JavaScript interop handlers and related helpers for the SfChart component.
    /// </summary>
    public partial class SfChart
    {
        #region Private Fields

        private DateTime _previousScrollRequestTime = DateTime.MinValue;
        private DateTime _previousPanRequestTime = DateTime.MinValue;
        private static readonly TimeSpan _minPanInterval = TimeSpan.FromMilliseconds(33);
        private ZoomingEventArgs? _onZoomStartArgs;
        private ZoomingEventArgs? _onZoomingArgs;

        #endregion

        #region Internal Properties

        internal bool _isLayoutChange;
        internal bool _isPointMouseDown;
        internal bool _isResize;
        internal bool _isResizeTemplate;

        #endregion

        #region JSInvokable Methods

        /// <summary>
        /// Handles the chart scroll event triggered by JavaScript interop.
        /// </summary>
        /// <param name="axisName">The name of the axis being scrolled</param>
        /// <param name="zoomFactor">The zoom factor applied to the axis</param>
        /// <param name="zoomPosition">The position of the zoom window</param>
        /// <returns>A task representing the asynchronous scroll operation</returns>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task ChartScrolledAsync(string axisName, double zoomFactor, double zoomPosition)
        {
            DateTime currentTime = DateTime.UtcNow;
            if ((currentTime - _previousScrollRequestTime) < _minPanInterval)
            {
                return;
            }
            _previousScrollRequestTime = currentTime;
            string? axisNameWithoutUnderscore = axisName?.Replace("_", "", StringComparison.Ordinal);
            if (_axisContainer is not null)
            {
                foreach (IChartElementRenderer renderer in _axisContainer.Renderers)
                {
                    if (renderer is ChartAxisRenderer axisRenderer)
                    {
                        string currentAxisName = axisRenderer.Axis?.GetName() ?? string.Empty;
                        string currentAxisNameWithoutUnderscore = currentAxisName.Replace("_", "", StringComparison.Ordinal);

                        if (axisName == currentAxisName || axisNameWithoutUnderscore == currentAxisNameWithoutUnderscore)
                        {
                            axisRenderer.Axis?.UpdateZoomValues(zoomFactor, zoomPosition);
                            break;
                        }
                    }
                }
            }

            await UpdateNeededRenderersAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Updates the chart data and renders tooltip or crosshair information.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void UpdateChartData()
        {
            if (_zoomingModule is not null)
            {
                if (_zoomingModule.IsWheelZoom)
                {
                    _ = _zoomingModule.InvokeMouseWheelZoomEndAsync();
                }
                else
                {
                    UpdateRenderers(true);
                }
            }
            if (_tooltip.Enable || _crosshair.Enable || _markerExplode is not null)
            {
                _ = InvokeVoidAsync(_chartJsModule!, _chartJsInProcessModule!, Constants.SetTooltipOptions, [_dataId, _tooltip.GetTooltipForScript(), GetTooltipOptions(), _seriesClipRects.ToArray(), _seriesMarkers.ToArray(), _seriesBorders.ToArray(), _axes.ToArray(), _seriesContainer?._dateValuePairs as object ?? null!, _seriesContainer?._numberValuePairs as object ?? null!]);
                const int UPDATETHRESHOLD = 100;
                if (!IsDisposed && _seriesContainer is not null && (_seriesContainer._previousRequestTime == DateTime.MinValue || (DateTime.Now - _seriesContainer._previousRequestTime).TotalMilliseconds > UPDATETHRESHOLD))
                {
                    _seriesContainer._previousRequestTime = DateTime.Now;
                    _seriesContainer.SetGlobalizationValues();
                    _ = UpdateChartPointsAsync();
                }
            }
            UpdateClientSideScrollbar();
        }

        /// <summary>
        /// Handles chart panning triggered by JavaScript interop.
        /// </summary>
        /// <param name="axisNames">List of axis names being panned</param>
        /// <param name="zoomFactors">List of zoom factors for each axis</param>
        /// <param name="zoomPositions">List of zoom positions for each axis</param>
        /// <returns>A task representing the asynchronous pan operation</returns>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task ChartPanAsync(List<string> axisNames, List<double> zoomFactors, List<double> zoomPositions)
        {
            DateTime currentTime = DateTime.UtcNow;
            if ((currentTime - _previousPanRequestTime) < _minPanInterval)
            {
                return;
            }
            _previousPanRequestTime = currentTime;
            if (OnZooming is not null)
            {
                ZoomingEventArgs args = _zoomingModule?.TriggerZoomingEvent(OnZooming, Constants.OnZooming) ?? null!;

                if (args is not null && args.Cancel)
                {
                    return;
                }
            }
            if (_zoomingModule is not null)
            {
                _zoomingModule.IsPanning = true;
                _zoomingModule.IsZoomed = true;
                _zoomingModule.PerformedUI = true;
            }
            _isChartDrag = true;

            bool needsUpdate = false;
            int axisCount = Math.Min(axisNames?.Count ?? 0, Math.Min(zoomFactors?.Count ?? 0, zoomPositions?.Count ?? 0));

            for (int i = 0; i < axisCount; i++)
            {
                string currentAxisName = axisNames[i].Replace("_", "", StringComparison.OrdinalIgnoreCase);
                ChartAxisRenderer? axisRenderer = _axisContainer?.Renderers.FirstOrDefault(r =>
                    string.Equals((r as ChartAxisRenderer)?.Axis?.Name.Replace("_", "", StringComparison.OrdinalIgnoreCase), currentAxisName, StringComparison.OrdinalIgnoreCase)) as ChartAxisRenderer;

                if (axisRenderer is not null && zoomPositions is not null && zoomFactors is not null && zoomPositions[i] > 0.001)
                {
                    axisRenderer.Axis?.UpdateZoomValues(zoomFactors[i], zoomPositions[i]);
                    needsUpdate = true;
                }
            }

            if (needsUpdate)
            {
                await UpdateNeededRenderersAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Handles keyboard navigation events for the chart.
        /// </summary>
        /// <param name="actionKey">The keyboard action key (e.g., "Enter", "CtrlP", "Equal", "Minus")</param>
        /// <param name="targetId">The ID of the target element receiving focus</param>
        /// <returns>A task representing the asynchronous navigation operation</returns>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task OnChartKeyboardNavigationsAsync(string actionKey, string targetId)
        {
            if (!string.IsNullOrEmpty(targetId))
            {
                ChartInternalMouseEventArgs args = new() { Target = targetId };
                switch (actionKey)
                {
                    case "Enter":
                        MouseClick?.Invoke(this, args);
                        _legendRenderer?.ProcessNavigationLegendEnter(args.Target);
                        if (OnPointClick.HasDelegate)
                        {
                            TriggerPointEvent(Constants.PointerClick, OnPointClick, args);
                        }
                        if (_legendRenderer is not null)
                        {
                            _legendRenderer.KeyboardFocusTarget = targetId.Contains("_legend_", StringComparison.InvariantCulture) ? targetId : string.Empty;
                        }
                        if (_zoomingModule is not null)
                        {
                            _zoomingKeyboardFocusTarget = targetId.Contains("_Zooming_", StringComparison.InvariantCulture) ? targetId : string.Empty;
                        }
                        if (!string.IsNullOrEmpty(_zoomingKeyboardFocusTarget))
                        {
                            await InvokeVoidAsync(_chartJsModule!, _chartJsInProcessModule!, Constants.FocusTarget, [_zoomingKeyboardFocusTarget]).ConfigureAwait(false);
                        }
                        if (!string.IsNullOrEmpty(_legendRenderer?.KeyboardFocusTarget))
                        {
                            await InvokeVoidAsync(_chartJsModule!, _chartJsInProcessModule!, Constants.FocusTarget, [_legendRenderer.KeyboardFocusTarget]).ConfigureAwait(false);
                        }
                        break;
                    case "Equal":
                    case "Minus":
                        if (_legendRenderer is not null)
                        {
                            _legendRenderer.KeyboardFocusTarget = string.Empty;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Sets the accessibility label for a specific chart point.
        /// </summary>
        /// <param name="seriesIndex">The zero-based index of the series</param>
        /// <param name="pointIndex">The zero-based index of the point within the series</param>
        /// <param name="targetId">The ID of the target element to update</param>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void GetAccessibilityText(int seriesIndex, int pointIndex, string targetId)
        {
            IChartElementRenderer renderer = !string.IsNullOrEmpty(targetId) && targetId.Contains("_Zooming_", StringComparison.InvariantCulture) ? _trendlineContainer?.Renderers[seriesIndex] ?? null! : _seriesContainer?.Renderers[seriesIndex] ?? null!;
            ChartSeriesRenderer seriesRenderer = renderer as ChartSeriesRenderer ?? null!;
            _ = SetAttributeAsync(targetId, "aria-label", seriesRenderer.GetPointDescriptionFormatText(seriesRenderer.Points?[pointIndex] ?? null!), string.Empty);
        }

        /// <summary>
        /// Handles right-click (context menu) events on chart elements.
        /// </summary>
        /// <param name="args">The mouse event arguments containing click coordinates and target information</param>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void OnChartMouseRightClick(ChartInternalMouseEventArgs args)
        {
            if (args is null)
            {
                return;
            }

            IsTouchEnabled(args);
            if (OnPointClick.HasDelegate)
            {
                TriggerPointEvent(Constants.PointerClick, OnPointClick, args, true);
            }
        }

        /// <summary>
        /// Handles left-click events on chart elements.
        /// </summary>
        /// <param name="args">The mouse event arguments containing click coordinates and target information</param>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void OnChartMouseClick(ChartInternalMouseEventArgs args)
        {
            if (args is null)
            {
                return;
            }

            IsTouchEnabled(args);
            ChartMouseClickHandler(args);
        }

        /// <summary>
        /// Handles mouse leave events on the chart.
        /// </summary>
        /// <param name="args">The mouse event arguments</param>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void OnChartMouseLeave(ChartInternalMouseEventArgs args)
        {
            SetSvgCursor("null");
            if (args is null)
            {
                return;
            }
            IsTouchEnabled(args);
            _isChartDrag = _isPointMouseDown = false;
            if (HighlightMode == HighlightMode.None && _legendRenderer?.Legend is not null && _legendRenderer.Legend.EnableHighlight && _highlightModule?.HighlightDataIndexes.Count > 0)
            {
                _ = _highlightModule.RemoveSelectionStylesAsync();
            }
            MouseCancel?.Invoke(this, args);
            _ = (_striplineTooltipModule?.HideTooltipAsync());
        }

        /// <summary>
        /// Handles crosshair move events.
        /// </summary>
        /// <param name="args">The crosshair move event arguments</param>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void OnCrosshairMoveHandler(CrosshairMoveEventArgs args)
        {
            OnCrosshairMove?.Invoke(args);
        }

        /// <summary>
        /// Handles mouse wheel events on the chart.
        /// </summary>
        /// <param name="args">The mouse wheel event arguments</param>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void OnChartMouseWheel(ChartMouseWheelArgs args)
        {
            WheelEvent?.Invoke(this, args);
        }

        /// <summary>
        /// The method is invoke from js while resize.
        /// </summary>
        /// <param name="size">Specifies the format of the offset size of the chart.</param>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void OnChartResize(string size)
        {
            Size availabelSize = JsonSerializer.Deserialize<Size>(size) ?? null!;
            _isResize = true;
            _ = ResizeChartAsync(availabelSize);
        }

        /// <summary>
        /// Resizes the chart based on new available size.
        /// </summary>
        /// <param name="availabelSize">The new available size for the chart</param>
        /// <returns>A task representing the asynchronous resize operation</returns>
        private async Task ResizeChartAsync(Size availabelSize)
        {
            try
            {
                Size previousSize = new(AvailableSize.Width, AvailableSize.Height);
                _elementOffset.Width = availabelSize.Width;
                _elementOffset.Height = availabelSize.Height;
                CalculateAvailableSize();
                ResizeEventArgs argsData = new("SizeChanged", false, AvailableSize, previousSize);
                SizeChanged?.Invoke(argsData);

                if (!argsData.Cancel)
                {
                    AvailableSize = argsData.CurrentSize;
                    SetInitialRect();
                    await SetSvgDimensionAsync(Constants.SetSvgDimensions).ConfigureAwait(true);
                    _parentRect?.ClearElements();
                    await CalculateSecondaryElementPositionAsync().ConfigureAwait(false);
                    _isResizeTemplate = false;
                    OnLayoutChange();
                    if (_isLegendRendered)
                    {
                        _isResizeTemplate = true;
                        _ = UpdateLegendTemplateAsync();
                    }
                    if (_render.IsSizeSet && _selectionModule is not null)
                    {
                        _ = _selectionModule.RemoveSelectedElementsAsync();
                        _selectionModule.InvokeSelection();
                    }
                    await Task.Delay(500).ConfigureAwait(false);
                    _datalabelTemplateContainer?.InvalidateRender();
                    _axisLabelTemplateContainer?.InvalidateRender();
                    _legendItemTemplateContainer?.InvalidateRender();
                    if ((_tooltip.Enable || _crosshair.Enable || _markerExplode is not null) && _isScriptCalled)
                    {
                        _seriesContainer?.SetGlobalizationValues();
                        await InvokeVoidAsync(_chartJsModule, _chartJsInProcessModule, Constants.SetTooltipOptions, [_dataId, _tooltip.GetTooltipForScript(), GetTooltipOptions(), _seriesClipRects.ToArray(), _seriesMarkers.ToArray(), _seriesBorders.ToArray(), _axes.ToArray(), _seriesContainer?._dateValuePairs as object ?? null!, _seriesContainer?._numberValuePairs as object ?? null!]).ConfigureAwait(false);
                    }
                }
            }
            catch
            {
                if (!IsDisposed)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Disposes the .NET reference held by the JavaScript interop module.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void DisposeDotNetReference()
        {
            if (!IsDisposed && _chartDotNetReference?.Value is not null)
            {
                _chartDotNetReference?.Dispose();
            }
        }

        /// <exclude />
        /// <summary>
        /// Prevents the stock chart panning functionality.
        /// </summary>
        /// <remarks>
        /// Internal use only - Dependent component for stock chart functionality.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void OnChartLongPress()
        {
            _startMove = _startMove;
        }

        /// <summary>
        /// Triggers a point-related event with detailed point information.
        /// </summary>
        /// <param name="eventName">The name of the event to trigger</param>
        /// <param name="action">The event callback to invoke</param>
        /// <param name="evt">The mouse event arguments containing event details</param>
        /// <param name="isRightClick">Indicates whether this is a right-click event (default: <c>false</c>)</param>
        private void TriggerPointEvent(string eventName, EventCallback<PointEventArgs> action, ChartInternalMouseEventArgs evt, bool isRightClick = false)
        {
            PointData pointData = new ChartData(this).GetData();
            if (pointData.Series is not null && pointData.Point is not null)
            {
                PointEventArgs pointEvent = new(eventName, false, evt.ClientX, evt.ClientY, pointData.Point, pointData.Point.Index, pointData.Series, pointData.Series.Renderer.Index, evt.MouseX, evt.MouseY, isRightClick);
                _ = InvokeDelegateAsync(action, pointEvent);
            }
        }

        /// <summary>
        /// Triggers axis label click events when labels are clicked.
        /// </summary>
        /// <param name="mouseEventArgs">The mouse event arguments with target identification</param>
        private void TriggerAxisLabelClickEvent(ChartInternalMouseEventArgs mouseEventArgs)
        {
            if (mouseEventArgs.Target.Contains("_AxisLabel_", StringComparison.InvariantCulture))
            {
                string[] index = mouseEventArgs.Target.Split("_AxisLabel_");
                int axisIndex = Convert.ToInt32(index[0].Split(ID)[1], null);
                int labelIndex = Convert.ToInt32(index[1], null);
                ChartAxisRenderer currentAxisRenderer = _axisContainer?.Renderers[axisIndex] as ChartAxisRenderer ?? null!;
                ChartAxis currentAxis = currentAxisRenderer.Axis ?? null!;
                if (currentAxis.Visible && (axisIndex == 0 || axisIndex == 1))
                {
                    AxisLabelClickEventArgs argsData = new("OnAxisLabelClick",
                        this,
                        currentAxis,
                        currentAxisRenderer.VisibleLabels[labelIndex].Text,
                        mouseEventArgs.Target,
                        labelIndex,
                        new ChartEventLocation(mouseEventArgs.ClientX, mouseEventArgs.ClientY),
                        currentAxisRenderer.VisibleLabels[labelIndex].Value);
                    OnAxisLabelClick?.Invoke(argsData);
                }
            }
        }

        /// <summary>
        /// Triggers multilevel label click events.
        /// </summary>
        /// <param name="mouseEventArgs">The mouse event arguments containing target identification</param>
        private void TriggerMultilevelLabelClick(ChartInternalMouseEventArgs mouseEventArgs)
        {
            string multiLevelID = "_Axis_MultiLevelLabel_Level_";
            if (mouseEventArgs.Target.Contains(multiLevelID, StringComparison.InvariantCulture))
            {
                string elementId = mouseEventArgs.Target.Split(multiLevelID)[0];
                if (int.TryParse(elementId.AsSpan(elementId.Length - 1), out int length))
                {
                    MultiLevelLabelClick(mouseEventArgs.Target.Split(multiLevelID)[1], length);
                }
            }
        }

        /// <summary>
        /// Processes multilevel label click events.
        /// </summary>
        /// <param name="labelIndex">The multilevel label index string containing level and text element indices</param>
        /// <param name="axisIndex">The zero-based index of the axis</param>
        private void MultiLevelLabelClick(string labelIndex, int axisIndex)
        {
            int textElement = int.TryParse(labelIndex.AsSpan(7), out int element) ? element : 0;
            ChartAxis axis = (_axisContainer?.Renderers[axisIndex] as ChartAxisRenderer ?? null!).Axis ?? null!;
            List<ChartCategory> categories = axis.MultiLevelLabels[int.TryParse(labelIndex.AsSpan(0, 1), out int index) ? index : 0].Categories;
            MultiLevelLabelClickEventArgs multilevelclickArgs = new("OnMultiLevelLabelClick", false, categories[textElement].Text, axis, categories[textElement].CustomAttributes, categories[textElement].End, int.TryParse(labelIndex.AsSpan(0, 1), out int length) ? length : 0, categories[textElement].Start);
            OnMultiLevelLabelClick.Invoke(multilevelclickArgs);
        }

        /// <summary>
        /// Shows tooltip for trimmed title and axis labels.
        /// </summary>
        /// <param name="args">The mouse event arguments</param>
        private void TitleTooltip(ChartInternalMouseEventArgs args)
        {
            string targetId = args.Target, title;
            int index = 0;
            SvgText titleElement = _svgRenderer?.TextElementList?.Find(element => element.Id == targetId) ?? null!;
            if (targetId.Contains("_AxisTitle", StringComparison.InvariantCulture))
            {
                index = Convert.ToInt32(targetId.Replace(ID, string.Empty, StringComparison.InvariantCulture).Replace("AxisLabel_", string.Empty, StringComparison.InvariantCulture).Split("_")[2], 10);
            }

            if ((targetId == (ID + "_ChartTitle") || targetId == (ID + "_ChartSubTitle") || targetId.Contains("_AxisTitle", StringComparison.InvariantCulture) || targetId.Contains("_legend_title", StringComparison.InvariantCulture)) && (titleElement is not null ? titleElement.Text : string.Empty).Contains("...", StringComparison.InvariantCulture))
            {
                title = (_axisContainer?.Renderers[index] as ChartAxisRenderer ?? null!).Axis?.Title ?? string.Empty;
                ChartBorder border = _chartBorderRenderer?.ChartBorder ?? null!;
                _trimTooltip?.ShowTooltip((targetId == (ID + "_ChartTitle")) ? Title : targetId.Contains("_AxisTitle", StringComparison.InvariantCulture) ? title : targetId.Contains("_ChartSubTitle", StringComparison.InvariantCulture) ? SubTitle : string.Empty, args.MouseX, args.MouseY, AvailableSize.Width - border.Width, AvailableSize.Height - border.Width, ID + "_EJ2_Title_Tooltip");
            }
            else
            {
                _trimTooltip?.ChangeContent(ID + "_EJ2_Title_Tooltip");
            }
        }

        /// <summary>
        /// Shows tooltip for truncated axis labels.
        /// </summary>
        /// <param name="args">The mouse event arguments</param>
        private void AxisTooltip(ChartInternalMouseEventArgs args)
        {
            SvgText axisElement = _svgRenderer?.TextElementList?.Find(element => element.Id == args.Target) ?? null!;
            if ((args.Target.Contains("AxisLabel", StringComparison.InvariantCulture) || args.Target.Contains("Axis_MultiLevelLabel", StringComparison.InvariantCulture)) && ((axisElement is not null) ? axisElement.Text : string.Empty).Contains("...", StringComparison.InvariantCulture))
            {
                ChartBorder border = _chartBorderRenderer?.ChartBorder ?? null!;
                _trimTooltip?.ShowTooltip(FindAxisLabel(args.Target), args.MouseX, args.MouseY, AvailableSize.Width - border.Width, AvailableSize.Height - border.Width, ID + "_EJ2_AxisLabel_Tooltip");
            }
            else
            {
                _trimTooltip?.ChangeContent(ID + "_EJ2_AxisLabel_Tooltip");
            }
        }

        /// <summary>
        /// Finds the full text for a truncated axis label.
        /// </summary>
        /// <param name="text">The target element ID containing axis label information</param>
        /// <returns>The full original text of the axis label</returns>
        private string FindAxisLabel(string text)
        {
            string[] texts;
            string label;
            if (text.Contains("AxisLabel", StringComparison.InvariantCulture))
            {
                texts = text.Replace(ID, string.Empty, StringComparison.InvariantCulture).Replace("AxisLabel_", string.Empty, StringComparison.InvariantCulture).Split("_");
                label = (_axisContainer?.Renderers[Convert.ToInt32(texts[0], 10)] as ChartAxisRenderer ?? null!).VisibleLabels[Convert.ToInt32(texts[1], 10)].OriginalText;
                return label;
            }
            else
            {
                texts = text.Replace(ID, string.Empty, StringComparison.InvariantCulture).Replace("Axis_MultiLevelLabel_Level_", string.Empty, StringComparison.InvariantCulture).Replace("Text_", string.Empty, StringComparison.InvariantCulture).Split("_");
                label = (_axisContainer?.Renderers[Convert.ToInt32(texts[0], 10)] as ChartAxisRenderer ?? null!).Axis?.MultiLevelLabels[Convert.ToInt32(texts[1], 10)].Categories[Convert.ToInt32(texts[2], 10)].Text ?? string.Empty;
                return label;
            }
        }

        /// <summary>
        /// Calculates X and Y axis values at the clicked location.
        /// </summary>
        /// <param name="mouseClickArgs">The mouse event arguments that will be updated with axis data</param>
        private void FindXYPointValue(ChartMouseEventArgs mouseClickArgs)
        {
            double pointValue = 0; object axisValue;
            bool isAxisRect = true;
            mouseClickArgs.AxisData = [];
            if (_axisContainer is not null)
            {
                foreach (ChartAxisRenderer axisRenderer in _axisContainer.Renderers.Cast<ChartAxisRenderer>())
                {
                    if (!double.IsNaN(pointValue))
                    {
                        Rect axisRect = axisRenderer.Axis is not null && !axisRenderer.Axis.PlaceNextToAxisLine ? axisRenderer.Rect : axisRenderer.UpdatedRect;
                        double labelValue = axisRenderer.IsCategory() ? 0.5 : 0;
                        if (ChartHelper.WithInAreaBounds(mouseClickArgs.MouseX, mouseClickArgs.MouseY, axisRect))
                        {
                            pointValue = axisRenderer.Orientation == Orientation.Horizontal
                                ? ChartHelper.GetValueByPoint(mouseClickArgs.MouseX - axisRect.X, axisRect.Width, axisRenderer.Orientation, axisRenderer.VisibleRange, axisRenderer.Axis is not null && axisRenderer.Axis.IsAxisInverse) + labelValue
                                : ChartHelper.GetValueByPoint(mouseClickArgs.MouseY - axisRect.Y, axisRect.Height, axisRenderer.Orientation, axisRenderer.VisibleRange, axisRenderer.Axis is not null && axisRenderer.Axis.IsAxisInverse) + labelValue;
                        }
                        else
                        {
                            isAxisRect = false;
                        }
                        if (!double.IsNaN(pointValue) && isAxisRect)
                        {
                            bool isSeriesVisible = FindSeriesVisibility(axisRenderer);
                            if (axisRenderer.Axis is not null && axisRenderer.Axis.Visible && isSeriesVisible)
                            {
                                axisValue = axisRenderer.GetAxisData(pointValue);
                                mouseClickArgs.AxisData.Add(axisRenderer.Axis.Name, axisValue);
                            }
                            else
                            {
                                axisValue = null!;
                                mouseClickArgs.AxisData.Add(axisRenderer.Axis?.Name ?? string.Empty, axisValue);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Determines if any series using the specified axis is visible.
        /// </summary>
        /// <param name="axisRenderer">The axis renderer to check</param>
        /// <returns><c>true</c> if at least one series on this axis is visible; otherwise, <c>false</c></returns>
        private static bool FindSeriesVisibility(ChartAxisRenderer axisRenderer)
        {
            foreach (ChartSeriesRenderer seriesRenderer in axisRenderer.SeriesRenderer)
            {
                if (seriesRenderer.Series is not null && seriesRenderer.Series.Visible)
                {
                    return seriesRenderer.Series.Visible;
                }
            }
            return false;
        }

        /// <summary>
        /// Handles mouse down events for zooming operations.
        /// </summary>
        /// <param name="args">The mouse event arguments</param>
        /// <param name="isChartPanning">Indicates whether chart panning is active</param>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void OnZoomingMouseDown(ChartInternalMouseEventArgs args, bool isChartPanning)
        {
            if (args is null || _isLayoutChange)
            {
                return;
            }

            if (_zoomingModule is not null)
            {
                _zoomingModule.IsPanning = args.Target.Contains("_Zooming_Pan", StringComparison.InvariantCulture) || isChartPanning || IsPanning();
                _isChartDrag = (_zoomingModule.IsAxisZoomed(_axisContainer?.Renderers ?? null!) && isChartPanning) || _zoomSettings.EnableSelectionZooming;
            }
            _ = OnMouseDownAsync(args);
        }

        /// <summary>
        /// Handles mouse move events during zooming operations.
        /// </summary>
        /// <param name="args">The mouse event arguments</param>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void OnZoomingMouseMove(ChartInternalMouseEventArgs args)
        {
            if (args is null || _isLayoutChange)
            {
                return;
            }
            ProcessChartMouseMove(args);
        }

        /// <summary>
        /// Handles mouse up events after zooming operations.
        /// </summary>
        /// <param name="args">The mouse event arguments</param>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void OnZoomingMouseEnd(ChartInternalMouseEventArgs args)
        {
            if (args is null || _isLayoutChange)
            {
                return;
            }
            _isChartDrag = false;
            OnMouseUp(args);
        }

        /// <summary>
        /// Handles selection changes in the chart.
        /// </summary>
        /// <param name="args">List of selected point coordinates</param>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void OnSelectionChange(List<PointXY> args)
        {
            _selectionModule?.TriggerSelectionComplete(args);
        }

        /// <summary>
        /// Calculates the stacked total value for a specific point.
        /// </summary>
        /// <param name="seriesRenderer">The series renderer containing the point</param>
        /// <param name="pointIndex">The zero-based index of the point</param>
        /// <returns>The stacked total value for the point, or 0 if not applicable</returns>
        private static double GetStackedTotalValue(ChartSeriesRenderer seriesRenderer, int pointIndex)
        {
            if (seriesRenderer is null)
            {
                return 0;
            }
            int count = seriesRenderer.Points?.Count ?? 0;
            Point currentPoint = count > 0 && pointIndex < count ? seriesRenderer.Points?[pointIndex] ?? null! : null!;
            return currentPoint is not null && seriesRenderer.StackedPointValues.Count > 0 ? seriesRenderer.StackedPointValues[currentPoint.XValue] : 0;
        }

        /// <summary>
        /// Sets the tooltip template element size and position.
        /// </summary>
        /// <param name="templateLocationX">The X coordinate of the tooltip template location</param>
        /// <param name="templateLocationY">The Y coordinate of the tooltip template location</param>
        /// <param name="tooltipTemp">List of tooltip information for the displayed points</param>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void SetTooltipTemplateElementSizeAsync(double templateLocationX, double templateLocationY, List<IChartTooltipInfo> tooltipTemp)
        {
            if (_template is null)
            {
                return;
            }

            templateLocationX += _secondaryElementOffset.Left;
            templateLocationY += _secondaryElementOffset.Top;
            double totalYValue = 0;
            if (tooltipTemp is not null)
            {
                if (!_tooltip.Shared)
                {
                    ChartSeriesRenderer seriesRenderer = _visibleSeriesRenderers.Count > 0 ? _visibleSeriesRenderers[(int)tooltipTemp[0].SeriesIndex] ?? null! : null!;
                    if (seriesRenderer is not null && seriesRenderer.Series is not null && seriesRenderer.Series.SeriesType is not null && seriesRenderer.Series.SeriesType.Contains("Stacking", StringComparison.InvariantCulture))
                    {
                        totalYValue = GetStackedTotalValue(seriesRenderer, (int)tooltipTemp[0].PointIndex);
                    }
                    ChartTooltipInfo tooltipInfo = new()
                    {
                        PointX = tooltipTemp[0].PointX,
                        PointY = tooltipTemp[0].PointY,
                        PointIndex = tooltipTemp[0].PointIndex,
                        PointText = tooltipTemp[0].PointText,
                        SeriesIndex = tooltipTemp[0].SeriesIndex,
                        SeriesName = tooltipTemp[0].SeriesName,
                        High = tooltipTemp[0].High,
                        Low = tooltipTemp[0].Low,
                        Open = tooltipTemp[0].Open,
                        Close = tooltipTemp[0].Close,
                        Volume = tooltipTemp[0].Volume,
                        X = tooltipTemp[0].X,
                        Y = tooltipTemp[0].Y,
                        Text = tooltipTemp[0].Text,
                        StackedTotalValue = totalYValue
                    };
                    _templateTooltip?.ChangeContent(_template, new ChartEventLocation(templateLocationX, templateLocationY), tooltipInfo, true, this);
                }
                else
                {
                    List<ChartTooltipInfo> tooltipInfos = [];
                    tooltipTemp.ForEach(point =>
                    {
                        ChartSeriesRenderer seriesRenderer = _visibleSeriesRenderers.Count > 0 ? _visibleSeriesRenderers[(int)point.SeriesIndex] : null!;
                        if (seriesRenderer is not null && seriesRenderer.Series is not null && seriesRenderer.Series.SeriesType is not null && seriesRenderer.Series.SeriesType.Contains("Stacking", StringComparison.InvariantCulture))
                        {
                            totalYValue = GetStackedTotalValue(seriesRenderer, (int)point.PointIndex);
                        }
                        tooltipInfos.Add(new ChartTooltipInfo()
                        {
                            PointX = point.PointX,
                            PointY = point.PointY,
                            PointIndex = point.PointIndex,
                            PointText = point.PointText,
                            SeriesIndex = point.SeriesIndex,
                            SeriesName = point.SeriesName,
                            High = point.High,
                            Low = point.Low,
                            Open = point.Open,
                            Close = point.Close,
                            Volume = point.Volume,
                            X = point.X,
                            Y = point.Y,
                            Text = point.Text,
                            StackedTotalValue = totalYValue
                        });
                    });
                    _templateTooltip?.ChangeContent(_template, new ChartEventLocation(templateLocationX, templateLocationY), tooltipInfos);
                }
            }
        }

        /// <summary>
        /// Removes the current template tooltip from display.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void RemoveTemplateTooltip()
        {
            _templateTooltip?.TemplateFadeOut();
        }

        /// <summary>
        /// Processes tooltip render events triggered by JavaScript.
        /// </summary>
        /// <param name="args">The tooltip render event arguments</param>
        /// <returns>A task representing the asynchronous operation</returns>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task TooltipEventTriggeredAsync(ITooltipRenderEventArgs args)
        {
            if (args is not null && _visibleSeriesRenderers.Count > 0)
            {
                double totalYValue = 0;
                ChartSeries series = _visibleSeriesRenderers
                    .Where(seriesRenderer => seriesRenderer is not null && seriesRenderer.Index == args.Data.SeriesIndex)
                    .Select(seriesRenderer => seriesRenderer.Series)
                    .FirstOrDefault() ?? null!;
                int pointsCount = series.Renderer.Points?.Count ?? 0;
                if (series is null || series.Renderer is null || pointsCount == 0 || args.Data.PointIndex < 0 || (pointsCount > 0 && args.Data.PointIndex > pointsCount))
                {
                    return;
                }
                if (series.SeriesType is not null && series.SeriesType.Contains("Stacking", StringComparison.InvariantCulture))
                {
                    totalYValue = GetStackedTotalValue(series.Renderer, (int)args.Data.PointIndex);
                }
                TooltipRenderEventArgs argsData = new TooltipRenderEventArgs(
                "TooltipRender",
                false,
                new PointInfo()
                {
                    PointX = args.Data.PointX,
                    PointY = args.Data.PointY,
                    SeriesIndex = args.Data.SeriesIndex,
                    SeriesName = args.Data.SeriesName,
                    PointIndex = args.Data.PointIndex,
                    PointText = args.Data.PointText,
                    StackedTotalValue = totalYValue
                },
                args.HeaderText,
                series.Renderer.Points?[(int)args.Data.PointIndex] ?? null!,
                series,
                args.Text,
                _tooltip.TextStyle);
                if (TooltipRender is not null)
                {
                    TooltipRender.Invoke(argsData);
                    if (!argsData.Cancel)
                    {
                        await InvokeVoidAsync(_chartJsModule!, _chartJsInProcessModule!, "setTooltipArgsData", [_dataId, argsData.HeaderText, argsData.Text]).ConfigureAwait(false);
                    }
                }
            }
        }

        /// <summary>
        /// Processes shared tooltip render events triggered by JavaScript.
        /// </summary>
        /// <param name="args">The shared tooltip render event arguments containing multiple points</param>
        /// <returns>A task representing the asynchronous operation</returns>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task SharedTooltipEventTriggeredAsync(ISharedTooltipRenderEventArgs args)
        {

            List<PointInfo> argsData = [];
            if (args is not null)
            {
                args.Data.ForEach(point =>
                {
                    argsData.Add(new PointInfo()
                    {
                        PointX = point.PointX,
                        PointY = point.PointY,
                        SeriesIndex = point.SeriesIndex,
                        SeriesName = point.SeriesName,
                        PointIndex = point.PointIndex,
                        PointText = point.PointText,
                    });
                });
                SharedTooltipRenderEventArgs argument = new("SharedTooltipRender", false, args.Text, _tooltip.TextStyle, args.HeaderText, argsData);
                SharedTooltipRender?.Invoke(argument);
                await InvokeVoidAsync(_chartJsModule!, _chartJsInProcessModule!, "setSharedTooltipArgsData", [_dataId, argument.HeaderText, argument.Text, argument.Data.ToArray()]).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Removes tooltip data attributes from the chart.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void RemoveTooltipData()
        {
            _tooltipsData?.RemoveAttribute();
        }

        /// <summary>
        /// Completes the zooming operation and updates the chart display.
        /// </summary>
        /// <param name="zoomingEventArgs">The zooming event arguments containing axis collection and zoom state</param>
        /// <param name="zoomingStates">The current zooming states</param>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void ZoomingComplete(ZoomingEventArgs zoomingEventArgs, IZoomingStates zoomingStates)
        {
            _ = _axisContainer?.Renderers ?? null!;
            if (_zoomingModule is not null)
            {
                if (zoomingStates is not null)
                {
                    _zoomingModule.IsPanning = zoomingStates.IsPanning;
                    _zoomingModule.IsZoomed = zoomingStates.IsZoomed;
                    _zoomingModule.IsWheelZoom = zoomingStates.IsWheelZoom;
                    _zoomingModule.PerformedUI = zoomingStates.PerformedUI;
                    _delayRedraw = zoomingStates.DelayRedraw;
                    _isDoubleTap = zoomingStates.IsDoubleTap;
                    _isChartDrag = zoomingStates.IsChartDrag;
                    _zoomingModule.IsZoomingComplete = true;
                }
                if (_zoomingModule.IsWheelZoom)
                {
                    if (_zoomingModule._isWheelStart)
                    {
                        _zoomingModule._isWheelStart = false;
                        if (zoomingEventArgs is not null)
                        {
                            zoomingEventArgs.Name = Constants.OnZoomStart;
                        }
                    }
                    else
                    {
                        _zoomingModule._isWheelStart = false;
                        if (zoomingEventArgs is not null)
                        {
                            zoomingEventArgs.Name = Constants.OnZooming;
                        }
                    }
                    _zoomingModule._wheelEndEventArgs = zoomingEventArgs;
                }
            }
            InvokeZoomingEvents(zoomingEventArgs);

            if ((_onZoomStartArgs is not null && !_onZoomStartArgs.Cancel) || (_onZoomingArgs is not null && !_onZoomingArgs.Cancel))
            {
                UpdateAxisZoomValues(zoomingEventArgs.AxisCollection, zoomingStates?.IsChartPanning ?? false);
            }
            else if (_onZoomingArgs is null && _onZoomStartArgs is null)
            {
                UpdateAxisZoomValues(zoomingEventArgs.AxisCollection, zoomingStates?.IsChartPanning ?? false);
            }
            OnLayoutChange(_zoomingModule?.IsWheelZoom ?? false);
            if (_zoomingModule is not null)
            {
                _zoomingModule.IsZoomingComplete = false;
            }
        }

        /// <summary>
        /// Sets deferred zoom for the zooming toolkit.
        /// </summary>
        /// <param name="zoomingEventArgs">The zooming event arguments</param>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void ZoomToolkitSetDeferredZoom(ZoomingEventArgs zoomingEventArgs)
        {
            if (zoomingEventArgs is not null && zoomingEventArgs.AxisCollection is not null)
            {
                UpdateAxisZoomValues(zoomingEventArgs.AxisCollection);

                if (!zoomingEventArgs.Cancel)
                {
                    if (_zoomingToolkitContent is not null)
                    {
                        _zoomingToolkitContent._isReset = true;
                    }
                    InvokeZoomingEvents(zoomingEventArgs);
                    if (!zoomingEventArgs.Cancel)
                    {
                        _ = _zoomingToolkitContent?.SetDeferredZoomAsync(this);
                    }
                }
                else
                {
                    _ = _zoomingToolkitContent?.SetDeferredZoomAsync(this);
                }
            }
        }

        /// <summary>
        /// Triggers zooming events (zoom start or end).
        /// </summary>
        /// <param name="eventName">The name of the zooming event</param>
        /// <param name="isZoomStart">Indicates whether this is the zoom start event</param>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void TriggerZoomingEvents(string eventName, bool isZoomStart)
        {
            if (eventName == Constants.OnZoomStart && OnZoomStart is not null)
            {
                _onZoomStartArgs = _zoomingModule?.TriggerZoomingEvent(OnZoomStart, Constants.OnZoomStart);
            }
            else if (eventName == Constants.OnZoomEnd && OnZoomEnd is not null)
            {
                _ = (_zoomingModule?.TriggerZoomingEvent(OnZoomEnd, Constants.OnZoomEnd));
            }
        }

        /// <summary>
        /// Processes scroll events and triggers the <see cref="OnScrollChanged"/> callback.
        /// </summary>
        /// <param name="scrollEventsArgs">The scroll event arguments</param>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void TriggerScrollEvents(IScrollEventsArgs scrollEventsArgs)
        {
            if (OnScrollChanged is not null)
            {
                ChartAxis axis = new();
                ChartAxisScrollbarSettingsRange currentRange = null!;
                if (_axisContainer is not null)
                {
                    foreach (ChartAxisRenderer axisRenderer in _axisContainer.Renderers.Cast<ChartAxisRenderer>())
                    {
                        string currentAxisName = axisRenderer.Axis?.GetName().IndexOf('_', StringComparison.Ordinal) > -1 ? axisRenderer.Axis.GetName().Replace("_", "", StringComparison.Ordinal) : axisRenderer.Axis?.GetName() ?? null!;
                        if (scrollEventsArgs?.AxisName == currentAxisName)
                        {
                            axis = axisRenderer.Axis ?? null!;
                            break;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(scrollEventsArgs?.CurrentRangeMax) && !string.IsNullOrEmpty(scrollEventsArgs.CurrentRangeMin))
                {
                    currentRange = new();
                    if (axis.ValueType is ValueType.DateTime or ValueType.DateTimeCategory)
                    {
                        currentRange = GetStartEnd(Convert.ToDouble(scrollEventsArgs.CurrentRangeMin, CultureInfo.InvariantCulture), Convert.ToDouble(scrollEventsArgs.CurrentRangeMax, CultureInfo.InvariantCulture), axis.ValueType);
                    }
                    else
                    {
                        currentRange.SetMinMax(scrollEventsArgs.CurrentRangeMin, scrollEventsArgs.CurrentRangeMax);
                    }
                }
                if (axis.Renderer is not null && OnScrollChanged is not null)
                {
                    DataVizCommonHelper.InvokeEvent(OnScrollChanged, GetScrollArguments(scrollEventsArgs.Name, axis, axis.Renderer.VisibleRange, scrollEventsArgs.ZoomPosition, scrollEventsArgs.ZoomFactor, currentRange, axis.Renderer.VisibleInterval));
                }
            }
        }

        /// <summary>
        /// Sets the selected icon details for the zooming toolkit.
        /// </summary>
        /// <param name="id">The ID of the selected icon</param>
        /// <param name="isSelected">Whether the icon is selected</param>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable("SetSelectedIcon")]
        public static void SetSelectedIcon(string id, bool isSelected)
        {
            ZoomToolkit._selectedIconId = id;
            ZoomToolkit._isIconSelected = isSelected;
        }

        /// <summary>
        /// Invokes appropriate zooming event callbacks based on event type.
        /// </summary>
        /// <param name="zoomingEventArgs">The zooming event arguments</param>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void InvokeZoomingEvents(ZoomingEventArgs zoomingEventArgs)
        {
            if (zoomingEventArgs?.Name == Constants.OnZoomStart && OnZoomStart is not null)
            {
                _onZoomStartArgs = _zoomingModule?.TriggerZoomingEvent(OnZoomStart, Constants.OnZoomStart);
            }
            else if (zoomingEventArgs?.Name == Constants.OnZooming && OnZooming is not null)
            {
                DataVizCommonHelper.InvokeEvent(OnZooming, zoomingEventArgs);
                _onZoomingArgs = zoomingEventArgs;
            }
            else if (zoomingEventArgs?.Name == Constants.OnZoomEnd && OnZoomEnd is not null)
            {
                DataVizCommonHelper.InvokeEvent(OnZoomEnd, zoomingEventArgs);
            }
        }

        /// <summary>
        /// Converts scrollbar range values to appropriate types based on value type.
        /// </summary>
        /// <param name="start">The start value of the range</param>
        /// <param name="end">The end value of the range</param>
        /// <param name="valueType">The value type of the axis</param>
        /// <returns>A <see cref="ChartAxisScrollbarSettingsRange"/> with converted values</returns>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static ChartAxisScrollbarSettingsRange GetStartEnd(object start, object end, ValueType valueType)
        {
            switch (valueType)
            {
                case ValueType.Double:
                case ValueType.Category:
                case ValueType.Logarithmic:
                    start = Math.Round((double)start);
                    end = Math.Ceiling((double)end);
                    break;
                case ValueType.DateTime:
                case ValueType.DateTimeCategory:
                    start = new DateTime(1970, 1, 1).AddMilliseconds((double)start);
                    end = new DateTime(1970, 1, 1).AddMilliseconds((double)end);
                    break;
                default:
                    break;
            }

            ChartAxisScrollbarSettingsRange range = new();
            range.SetMinMax(Convert.ToString(start, null) ?? null!, Convert.ToString(end, null) ?? null!);
            return range;
        }

        /// <summary>
        /// Creates scroll event arguments from scrollbar event data.
        /// </summary>
        /// <param name="eventName">The name of the scroll event</param>
        /// <param name="axis">The axis being scrolled</param>
        /// <param name="range">The current visible range (default: empty range)</param>
        /// <param name="zoomPosition">The zoom position (default: <see cref="double.NaN"/>)</param>
        /// <param name="zoomFactor">The zoom factor (default: <see cref="double.NaN"/>)</param>
        /// <param name="currentRange">The current scrollbar range (default: <c>null</c>)</param>
        /// <param name="previousInterval">The previous axis interval (default: <see cref="double.NaN"/>)</param>
        /// <returns>A <see cref="ScrollEventArgs"/> instance with calculated range models</returns>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static ScrollEventArgs GetScrollArguments(string eventName, ChartAxis axis, DoubleRange range = new DoubleRange(), double zoomPosition = double.NaN, double zoomFactor = double.NaN, ChartAxisScrollbarSettingsRange currentRange = null!, double previousInterval = double.NaN)
        {
            currentRange = currentRange is not null ? currentRange : GetStartEnd(axis?.Renderer?.VisibleRange.Start ?? 0, axis.Renderer?.VisibleRange.End ?? 0, axis.ValueType);
            VisibleRangeModel rangeModel = new();
            if (axis?.Renderer is not null)
            {
                rangeModel = ChartHelper.GetVisibleRangeModel(axis.Renderer.VisibleRange, axis.Renderer.VisibleInterval);
            }
            VisibleRangeModel previousRange = ChartHelper.GetVisibleRangeModel(range, previousInterval);

            return new ScrollEventArgs(eventName, axis ?? new(), currentRange, previousRange, zoomFactor, zoomPosition, rangeModel, axis.ZoomFactor, axis.ZoomPosition);
        }
        #endregion
    }
}
