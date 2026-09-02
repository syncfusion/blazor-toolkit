using System.Collections.Specialized;
using System.ComponentModel;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Toolkit.Charts.Internal;

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
                EnsureStaticSsrRenderers();
            }
            base.OnInitialized();
        }

        /// <summary>
        /// Bootstraps axis and series renderers in Static SSR so the chart can produce
        /// a non-interactive server-rendered output without any JS interop.
        /// </summary>
        private void EnsureStaticSsrRenderers()
        {
            // 1) Make sure the renderer containers exist.
            _axisContainer ??= new ChartAxisRendererContainer { Owner = this };
            _seriesContainer ??= new ChartSeriesRendererContainer { Owner = this };

            // 2) ChartAxisContainer.BuildRenderers iterates Elements and opens
            //    <PrimaryXAxisRenderer> / <PrimaryYAxisRenderer> / <ChartAxisRenderer>
            //    children — but ONLY when ContainerUpdate is true.  By default
            //    ContainerUpdate is false; it flips to true only inside
            //    Prerender(), which runs from HandleInitialRenderAsync AFTER the
            //    first build pass has completed.  Under Static SSR that is too
            //    late: StateHasChanged is a no-op because the response is
            //    already buffered.  Force it to true here so the very first
            //    build pass emits the axis children.
            _axisContainer.ContainerUpdate = true;
            _seriesContainer.ContainerUpdate = true;
            if (_axisOutSideContainer is not null)
            {
                _axisOutSideContainer.ContainerUpdate = true;
            }

            // 3) Flag every renderer registered so far for re-render so the
            //    children emit their inner content (axis line, ticks, labels).
            foreach (IChartElementRenderer renderer in _axisContainer.Renderers)
            {
                if (renderer is ChartRenderer chartRenderer)
                {
                    chartRenderer.RendererShouldRender = true;
                }
            }

            // 5) Mark size as already known so we don't try to measure via JS.
            _render.AvailableSize = new Size(600, 450);
            _render.IsSizeSet = true;
        }

        /// <summary>
        /// Called by the framework when component parameters have been set.
        /// Updates the chart theme style and triggers a dimension update when required.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            ChartThemeStyle themeStyle = ChartHelper.GetChartThemeStyle(Theme.ToString());
            if (_chartThemeStyle != themeStyle)
            {
                _chartThemeStyle = themeStyle;
            }
            if (_layout.UpdateLayout)
            {
                _layout.UpdateLayout = false;
                await OnDimensionChangedAsync();
            }

            if (_selection.IsMultiSelect != AllowMultiSelection)
            {
                _selection.IsMultiSelect = AllowMultiSelection;
                await CallJSInteropForSelectionHighlightOptionAsync(_selectionModule is not null && _isScriptLoaded);
                if (_selectionModule is not null)
                {
                    _selectionModule.ClearDraggedRects();
                    _selectionModule.OnPropertyChanged();
                    _parentRect?.ClearElements();
                }
            }

            if (_selection.HighlightPattern != HighlightPattern)
            {
                _selection.HighlightPattern = HighlightPattern;
                await CallJSInteropForSelectionHighlightOptionAsync();
                if (_highlightModule is not null)
                {
                    _highlightModule.CallSeriesStyles(false);
                    _highlightModule.AppendSelectionPattern();
                }
            }

            if (_selection.SelectionPattern != SelectionPattern)
            {
                _selection.SelectionPattern = SelectionPattern;
                await CallJSInteropForSelectionHighlightOptionAsync();
                if (_selectionModule is not null)
                {
                    _selectionModule.CallSeriesStyles();
                    _selectionModule.AppendSelectionPattern();
                }
            }

            if (_selection.HighlightMode != HighlightMode)
            {
                _selection.HighlightMode = HighlightMode;
                await CallJSInteropForSelectionHighlightOptionAsync(_selectionModule is not null && _isScriptLoaded);
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

            if (_selection.SelectionMode != SelectionMode)
            {
                _selection.SelectionMode = SelectionMode;
                await CallJSInteropForSelectionHighlightOptionAsync(_selectionModule is null && _isScriptLoaded);
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

            if (_layout.EnableSideBySidePlacement != EnableSideBySidePlacement)
            {
                _layout.EnableSideBySidePlacement = EnableSideBySidePlacement;
                _layout.UpdateLayout = true;
            }

            if (_appearance.Theme != Theme)
            {
                _appearance.Theme = Theme;
                if (IsRendered)
                {
                    _chartThemeStyle = ChartHelper.GetChartThemeStyle(_appearance.Theme.ToString());
                    OnThemeChanged();
                }
            }

            if (_layout.IsTransposed != IsTransposed)
            {
                _layout.IsTransposed = IsTransposed;
                if (IsRendered)
                {
                    InitiAxis();
                    _layout.UpdateLayout = IsRendered;
                }
            }

            if (_appearance.SubTitle != SubTitle)
            {
                _appearance.SubTitle = SubTitle;
                TitleChanged();
            }

            if (_appearance.Title != Title)
            {
                _appearance.Title = Title;
                TitleChanged();
            }

            if (_appearance.Width != Width)
            {
                _appearance.Width = Width;
                _layout.UpdateLayout = IsRendered;
            }
            if (_appearance.Height != Height)
            {
                _appearance.Height = Height;
                _layout.UpdateLayout = IsRendered;
            }
            if (_appearance.Background != Background)
            {
                _appearance.Background = Background;
                if (IsRendered && _chartBorderRenderer is not null)
                {
                    _chartBorderRenderer.RendererShouldRender = true;
                    _chartBorderRenderer.ProcessRenderQueue();
                }
            }

            if (_appearance.HighlightColor != HighlightColor)
            {
                _appearance.HighlightColor = HighlightColor;
                await CallJSInteropForSelectionHighlightOptionAsync();
                _highlightModule?.CallSeriesStyles(false);
            }

            if (_data.DataSource != DataSource)
            {
                _data.DataSource = DataSource;
                if (_data.DataSource is INotifyCollectionChanged notifyCollectionChanged)
                {
                    notifyCollectionChanged.CollectionChanged += DataCollectionChanged;

                    if (_data.DataSource.Any() && _data.DataSource.First() is INotifyPropertyChanged)
                    {
                        foreach (INotifyPropertyChanged item in _data.DataSource.Cast<INotifyPropertyChanged>())
                        {
                            if (item is INotifyPropertyChanged notifyPropertyChanged)
                            {
                                notifyPropertyChanged.PropertyChanged += PropertyChanged;
                            }
                        }
                    }
                }
            }

            if (!_appearance.Palettes.SequenceEqual(Palettes))
            {
                if (Palettes is null)
                {
                    return;
                }

                _appearance.Palettes = Palettes.Clone() as string[] ?? [];
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
                if (IsStaticServerRendering())
                {
                    // No JS interop available under Static SSR.
                    // The base OnAfterRenderAsync sets IsRendered and calls JS interop;
                    // we can't go through that path, but we MUST still set IsRendered
                    // so that IsRendered-gated updates (e.g. TitleChanged) actually fire
                    // and the title (and other on-render elements) are emitted.
                    if (firstRender)
                    {
                        IsRendered = true;
                    }
                    if (!_render.IsSizeSet)
                    {
                        await HandleInitialRenderAsync(firstRender).ConfigureAwait(true);
                    }
                    // Mark the title renderer for re-render and trigger a second build
                    // so the <text> element for the title is included in the output.
                    if (firstRender)
                    {
                        TitleChanged();
                    }
                    return;
                }
                await ImportComponentModuleAsync().ConfigureAwait(true);
                if (!_render.IsSizeSet)
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
                await UnWireEventsAsync();
                _svgRenderer?.Dispose();
                _pathAnimationElements?.Clear();
                _textAnimationElements?.Clear();
                _rectAnimationElements?.Clear();
                _noDataTemplateContainer = null;

                await DisposeJsModuleAsync(_interop.SvgJsModule, _interop.SvgJsInProcessModule).ConfigureAwait(false);
                _interop.SvgJsModule = null;
                _interop.SvgJsInProcessModule = null;

                await DisposeJsModuleAsync(_chartJsModule, _chartJsInProcessModule).ConfigureAwait(false);
                _chartJsModule = null;
                _chartJsInProcessModule = null;
            }

            // Clear instance-level font measurement caches to prevent memory retention
            // after the chart is disposed. This ensures per-circuit/per-chart isolation.
            _fontSizeCache?.Clear();
            _requestedFontKeys?.Clear();

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
            if (IsStaticServerRendering())
            {
                // No JS interop / DOM measurement available under Static SSR.
                // Ensure renderers are wired and the size is known, then render the frame.
                EnsureStaticSsrRenderers();
                CalculateAvailableSize();
                SetInitialRect();
                InitModules();
                RenderFrame();

                // Force every series / axis / container renderer to flag itself
                // for re-render and flush its render queue so the visual elements
                // (paths, labels, ticks) actually appear in the output tree.
                foreach (ChartRenderer renderer in _renderers)
                {
                    if (renderer is null)
                    {
                        continue;
                    }
                    renderer.RendererShouldRender = true;
                    try
                    {
                        renderer.ProcessRenderQueue();
                    }
                    catch
                    {
                        // Defensive: a renderer that fails to process should not
                        // break the entire Static SSR render.
                    }
                }

                // A second RenderFrame ensures the freshly generated child
                // elements are emitted by the next build pass.
                RenderFrame();

                if (firstRender)
                {
                    TriggerLoadedEvent();
                }
                return;
            }

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
                await UpdateLegendTemplateAsync();
            }
            if (_zoomingModule is not null && !string.IsNullOrEmpty(_zoomingKeyboardFocusTarget))
            {
                await InvokeVoidAsync(_chartJsModule, _chartJsInProcessModule, Constants.FocusTarget, [_zoomingKeyboardFocusTarget]).ConfigureAwait(true);
                _zoomingKeyboardFocusTarget = string.Empty;
            }
            if (!firstRender && _render.IsSizeSet && _selectionModule is not null && SelectedDataIndexes.Count > 0)
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
                _interop.SvgJsModule,
                _interop.SvgJsInProcessModule
            ).ConfigureAwait(true);
            _interop.SvgJsModule = svgJsModuleReference.AsyncRef;
            _interop.SvgJsInProcessModule = svgJsModuleReference.InProcessRef;
			
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
                await UpdateLegendTemplateAsync();
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
