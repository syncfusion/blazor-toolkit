using Microsoft.JSInterop;
using Syncfusion.Blazor.Toolkit.Charts.Internal;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Partial class containing lifecycle-related implementations for the SfChart component.
    /// </summary>
    public partial class SfChart
    {
        #region LifeCycle Methods

        /// <summary>
        /// Initializes the chart component with default values and theme settings.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            _svgRenderer = new SvgRendering();
            _chartThemeStyle = ChartHelper.GetChartThemeStyle(Theme.ToString());
            _tabColor = _chartThemeStyle.TabColor;
            if (IsStaticServerRendering())
            {
                _svgWidth = "600";
                _svgHeight = "450";
            }
            base.OnInitialized();
        }

        /// <summary>
        /// Called by the framework when component parameters have been set.
        /// Updates the chart theme style and triggers a dimension update when required.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            ChartThemeStyle themeStyle = ChartHelper.GetChartThemeStyle(Theme.ToString());
            if (_chartThemeStyle != themeStyle)
            {
                _chartThemeStyle = themeStyle;
            }
            if (_updateLayout)
            {
                _updateLayout = false;
                _ = OnDimensionChangedAsync();
            }

            if (_isMultiSelect != AllowMultiSelection)
            {
                _isMultiSelect = AllowMultiSelection;
                _ = CallJSInteropForSelectionHighlightOptionAsync(_selectionModule is not null && _isScriptLoaded);
                if (_selectionModule is not null)
                {
                    _selectionModule.ClearDraggedRects();
                    _selectionModule.OnPropertyChanged();
                    _parentRect?.ClearElements();
                }
            }

            if (_highlightPattern != HighlightPattern)
            {
                _highlightPattern = HighlightPattern;
                _ = CallJSInteropForSelectionHighlightOptionAsync();
                if (_highlightModule is not null)
                {
                    _highlightModule.CallSeriesStyles(false);
                    _highlightModule.AppendSelectionPattern();
                }
            }

            if (_selectionPattern != SelectionPattern)
            {
                _selectionPattern = SelectionPattern;
                _ = CallJSInteropForSelectionHighlightOptionAsync();
                if (_selectionModule is not null)
                {
                    _selectionModule.CallSeriesStyles();
                    _selectionModule.AppendSelectionPattern();
                }
            }

            if (_highlightMode != HighlightMode)
            {
                _highlightMode = HighlightMode;
                _ = CallJSInteropForSelectionHighlightOptionAsync(_selectionModule is not null && _isScriptLoaded);
                if (_highlightModule is null && _isScriptLoaded)
                {
                    _highlightModule = new Highlight(this)
                    {
                        StyleRender = _highlightStyle ?? null!,
                        ReqPatterns = _highLightPatternCollection
                    };
                    _highlightModule.InvokeHighlight();
                    _selectionModule?.CallSeriesStyles();
                }

                _highlightModule?.PropertyChanged();
            }

            if (_selectionMode != SelectionMode)
            {
                _selectionMode = SelectionMode;
                _ = CallJSInteropForSelectionHighlightOptionAsync(_selectionModule is null && _isScriptLoaded);
                if (_selectionModule is null && _isScriptLoaded)
                {
                    _selectionModule = new Selection(this)
                    {
                        StyleRender = _selectionStyle ?? null!,
                        ReqPatterns = _selectionPatternCollection
                    };
                    _selectionModule.InvokeSelection();
                }

                _selectionModule?.ChartSelectionModeChanged();
            }

            if (_enableSideBySidePlacement != EnableSideBySidePlacement)
            {
                _enableSideBySidePlacement = EnableSideBySidePlacement;
                _updateLayout = true;
            }

            if (_theme != Theme)
            {
                _theme = Theme;
                if (IsRendered)
                {
                    _chartThemeStyle = ChartHelper.GetChartThemeStyle(_theme.ToString());
                    OnThemeChanged();
                }
            }

            if (_isTransposed != IsTransposed)
            {
                _isTransposed = IsTransposed;
                if (IsRendered)
                {
                    InitiAxis();
                    _updateLayout = IsRendered;
                }
            }

            if (_subTitle != SubTitle)
            {
                _subTitle = SubTitle;
                TitleChanged();
            }

            if (_title != Title)
            {
                _title = Title;
                TitleChanged();
            }

            if (_width != Width)
            {
                _width = Width;
                _updateLayout = IsRendered;
            }
            if (_height != Height)
            {
                _height = Height;
                _updateLayout = IsRendered;
            }
            if (_background != Background)
            {
                _background = Background;
                if (IsRendered && _chartBorderRenderer is not null)
                {
                    _chartBorderRenderer.RendererShouldRender = true;
                    _chartBorderRenderer.ProcessRenderQueue();
                }
            }

            if (_highlightColor != HighlightColor)
            {
                _highlightColor = HighlightColor;
                _ = CallJSInteropForSelectionHighlightOptionAsync();
                _highlightModule?.CallSeriesStyles(false);
            }

            if (_dataSource != DataSource)
            {
                _dataSource = DataSource;
                if (_dataSource is INotifyCollectionChanged notifyCollectionChanged)
                {
                    notifyCollectionChanged.CollectionChanged += DataCollectionChanged;

                    if (_dataSource.Any() && _dataSource.First() is INotifyPropertyChanged)
                    {
                        foreach (INotifyPropertyChanged item in _dataSource.Cast<INotifyPropertyChanged>())
                        {
                            if (item is INotifyPropertyChanged notifyPropertyChanged)
                            {
                                notifyPropertyChanged.PropertyChanged += PropertyChanged;
                            }
                        }
                    }
                }
            }

            if (!_palettes.SequenceEqual(Palettes))
            {
                if (Palettes is null)
                {
                    return;
                }

                _palettes = Palettes.Clone() as string[] ?? [];
            }
        }

        /// <summary>
        /// Handles the component rendering lifecycle after each render cycle.
        /// </summary>
        /// <param name="firstRender">Indicates whether this is the first time the component is being rendered.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            /*
             * In order to prevent background calculation processing even though component has been disposed of, the exceptions are treated by Try catch block for component disposal.
             * Ex: Quickly navigation between pages exception will throw, this has handled here.
             * This solution has suggested by MS blazor docs (https://docs.microsoft.com/en-us/aspnet/core/blazor/fundamentals/handle-errors?view=aspnetcore-3.1#component-disposal)
             */
            try
            {
                if (_skipRendering)
                {
                    _skipRendering = false;
                    return;
                }
                await ImportComponentModuleAsync().ConfigureAwait(true);
                if (!_isSizeSet)
                {
                    await HandleInitialRenderAsync(firstRender).ConfigureAwait(true);
                }
                await HandlePostRenderAsync(firstRender).ConfigureAwait(true);
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
        /// Handles operations that must occur after JavaScript interop scripts are loaded and rendered.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        internal override async Task OnAfterScriptRenderedAsync()
        {
            /*
             * In order to prevent background calculation processing even though component has been disposed of, the exceptions are treated by Try catch block for component disposal.
             * Ex: Quickly navigation between pages exception will throw, this has handled here.
             * This solution has suggested by MS blazor docs (https://docs.microsoft.com/en-us/aspnet/core/blazor/fundamentals/handle-errors?view=aspnetcore-3.1#component-disposal)
             */
            try
            {
                await HandleScriptRenderedAsync().ConfigureAwait(true);
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
        /// Disposes of component resources and cleans up event handlers.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override async ValueTask DisposeAsyncCore()
        {
            if (IsRendered)
            {
                _ = UnWireEventsAsync();
                _svgRenderer?.Dispose();
                _pathAnimationElements?.Clear();
                _textAnimationElements?.Clear();
                _rectAnimationElements?.Clear();
                _noDataTemplateContainer = null;

                await DisposeJsModuleAsync(_svgJsModule, _svgJsInProcessModule).ConfigureAwait(false);
                _svgJsModule = null;
                _svgJsInProcessModule = null;

                await DisposeJsModuleAsync(_chartJsModule, _chartJsInProcessModule).ConfigureAwait(false);
                _chartJsModule = null;
                _chartJsInProcessModule = null;
            }
            await base.DisposeAsyncCore().ConfigureAwait(true);
        }

        #endregion

        #region Lifecycle Helper Methods

        /// <summary>
        /// Handles the initial rendering setup for the chart component.
        /// </summary>
        /// <param name="firstRender">Indicates whether this is the first render of the component.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task HandleInitialRenderAsync(bool firstRender)
        {
            await SetCharSizeAsync().ConfigureAwait(true);
            await GetElementOffsetAsync(Constants.GetParentElementBoundsById).ConfigureAwait(true);
            CalculateAvailableSize();
            SetInitialRect();
            await SetSvgDimensionAsync(Constants.SetSvgDimensions).ConfigureAwait(true);
            await GetRemoteDataAsync().ConfigureAwait(true);
            InitModules();
            if (_axisContainer?.Renderers.Count == 0 || _seriesContainer?.Renderers.Count == 0 ||
                _columnContainer?.Renderers.Count == 0 || _rowContainer?.Renderers.Count == 0 ||
                _axisContainer?.Renderers.Count != _axisContainer?.Axes.Count)
            {
                RenderFrame();
            }
            await CalculateSecondaryElementPositionAsync().ConfigureAwait(true);
            await PerformLayoutAsync().ConfigureAwait(true);
            if (_hasLabelTemplate)
            {
                await UpdateAxisLabelTemplateAsync().ConfigureAwait(true);
            }
            InitPrivateModules();
            ApplyZoomkit();
            if (firstRender)
            {
                TriggerLoadedEvent();
            }
        }

        /// <summary>
        /// Handles post-render operations after the chart has been rendered to the DOM.
        /// </summary>
        /// <param name="firstRender">Indicates whether this is the first render of the component.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task HandlePostRenderAsync(bool firstRender)
        {
            //To set the default size to svg when the script wasn't referred. 
            if (_skipRendering)
            {
                _svgWidth = GetSvgWidth();
                _svgHeight = GetSvgHeight();
                await InvokeAsync(StateHasChanged).ConfigureAwait(true);
            }

            if (_tooltip is not null)
            {
                await Task.Delay(100).ConfigureAwait(true);
                GetChartPoints();
                if (_isScriptCalled && !_isLiveChart)
                {
                    if (_tooltip.Enable || _crosshair.Enable || _markerExplode is not null)
                    {
                        _template = _tooltip.Template;
                    }
                }
            }
            if (_legendRenderer is not null && !string.IsNullOrEmpty(_legendRenderer.KeyboardFocusTarget))
            {
                await InvokeVoidAsync(_chartJsModule!, _chartJsInProcessModule!, Constants.FocusTarget, [_legendRenderer.KeyboardFocusTarget]).ConfigureAwait(true);
            }
            await base.OnAfterRenderAsync(firstRender).ConfigureAwait(true);
            await ImportComponentModuleAsync().ConfigureAwait(true);
            _isLayoutChange = false;
            UpdateClientSideScrollbar();
            if (!firstRender && _isLegendRendered)
            {
                _ = UpdateLegendTemplateAsync();
            }
            if (_zoomingModule is not null && !string.IsNullOrEmpty(_zoomingKeyboardFocusTarget))
            {
                await InvokeVoidAsync(_chartJsModule, _chartJsInProcessModule, Constants.FocusTarget, [_zoomingKeyboardFocusTarget]).ConfigureAwait(true);
                _zoomingKeyboardFocusTarget = string.Empty;
            }
            if (!firstRender && _isSizeSet && _selectionModule is not null && SelectedDataIndexes.Count > 0)
            {
                await _selectionModule.RemoveSelectedElementsAsync().ConfigureAwait(true);
                _selectionModule.InvokeSelection();
            }
        }

        private new async Task ImportComponentModuleAsync()
        {
            await base.ImportComponentModuleAsync().ConfigureAwait(true);

            await LoadTouchScriptAsync().ConfigureAwait(true);

            JsModuleReference svgJsModuleReference = await ImportModuleAsync(
                "./_content/Syncfusion.Blazor.Toolkit/scripts/svgbase.js",
                _svgJsModule,
                _svgJsInProcessModule
            ).ConfigureAwait(true);
            _svgJsModule = svgJsModuleReference.AsyncRef;
            _svgJsInProcessModule = svgJsModuleReference.InProcessRef;
			
            await LoadAnimationScriptAsync().ConfigureAwait(true);
			
            JsModuleReference chartJsModuleReference = await ImportModuleAsync(
                "./_content/Syncfusion.Blazor.Toolkit/scripts/chart.js",
                _chartJsModule,
                _chartJsInProcessModule
            ).ConfigureAwait(true);
            _chartJsModule = chartJsModuleReference.AsyncRef;
            _chartJsInProcessModule = chartJsModuleReference.InProcessRef;
        }

        /// <summary>
        /// Handles operations that occur after JavaScript scripts are fully loaded and available.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task HandleScriptRenderedAsync()
        {
            _isScriptLoaded = true;
            if (!IsDimensionContainsPixel())
            {
                DomRect elementOffset = await InvokeAsync<DomRect>(_chartJsModule!, _chartJsInProcessModule!, Constants.GetParentElementBoundsById, [ID]).ConfigureAwait(true);
                if (elementOffset is not null)
                {
                    _elementOffset = elementOffset;
                    Size previousSize = new(AvailableSize.Width, AvailableSize.Height);
                    CalculateAvailableSize();
                    await SetSvgDimensionAsync(Constants.SetSvgDimensions).ConfigureAwait(true);
                    if (previousSize.Width != AvailableSize.Width || previousSize.Height != AvailableSize.Height)
                    {
                        OnLayoutChange();
                    }
                }
            }
            else
            {
                await SetSvgDimensionAsync(Constants.SetSvgDimensions).ConfigureAwait(true);
            }

            await CalculateSecondaryElementPositionAsync().ConfigureAwait(true);
            _chartDotNetReference?.Dispose();
            _chartDotNetReference = DotNetObjectReference.Create<object>(this);

            bool disableTouch = _zoomSettings.EnableSelectionZooming || _zoomSettings.EnablePinchZooming || SelectionMode != ChartSelectionMode.None || HighlightMode != HighlightMode.None;
            await InvokeVoidAsync(_chartJsModule!, _chartJsInProcessModule!, "initialize", [
                _dataId,
                _element,
                _chartDotNetReference,
                _zoomSettings.EnableMouseWheelZooming,
                IsScrollBarEnabled(),
                GetInstance(),
                GetSelectionHighlightOptions(),
                disableTouch,
                GetStripLineTooltip(),
                ChartMouseMove is not null,
                _dataLabelTemplateId
            ]).ConfigureAwait(true);

            if (_isLegendRendered)
            {
                _ = UpdateLegendTemplateAsync();
            }

            if (_tooltip.Enable || _crosshair.Enable || _markerExplode is not null)
            {
                _template = _tooltip.Template;
                _seriesContainer?.SetGlobalizationValues();
                await InvokeVoidAsync(_chartJsModule!, _chartJsInProcessModule!, Constants.SetTooltipOptions, [
                    _dataId,
                    _tooltip.GetTooltipForScript(),
                    GetTooltipOptions(),
                    _seriesClipRects.ToArray(),
                    _seriesMarkers.ToArray(),
                    _seriesBorders.ToArray(),
                    _axes.ToArray(),
                    _seriesContainer?._dateValuePairs as object ?? null!,
                    _seriesContainer?._numberValuePairs as object ?? null!
                ]).ConfigureAwait(true);
                _isScriptCalled = true;
            }

            await SetZoomOptionsAsync().ConfigureAwait(true);
            await GetBrowserDeviceInfoAsync().ConfigureAwait(true);
            await UpdateDatalabelTemplateAsync().ConfigureAwait(true);
            await PerformAnimationAsync().ConfigureAwait(true);
            PerformSelection();
        }


        /// <summary>
        /// Disposes JavaScript module references used for interop, handling both asynchronous and in-process module instances in a safe and unified manner.
        /// </summary>
        /// <param name="asyncModule">
        /// The asynchronous JavaScript module reference (<see cref="IJSObjectReference"/>) to dispose.
        /// </param>
        /// <param name="inProcessModule">
        /// The in-process JavaScript module reference (<see cref="IJSInProcessObjectReference"/>) to dispose.
        /// </param>
        private static async ValueTask DisposeJsModuleAsync(IJSObjectReference? asyncModule, IJSInProcessObjectReference? inProcessModule)
        {
            if (asyncModule is not null)
            {
                await asyncModule.DisposeAsync().ConfigureAwait(true);
            }
            inProcessModule?.Dispose();
        }

        #endregion
    }
}
