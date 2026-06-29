using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Toolkit.Charts.Internal;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Reflection;
using System.ComponentModel;
using Syncfusion.Blazor.Toolkit.Data;
using Microsoft.Extensions.Localization;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Syncfusion.Blazor.Toolkit.BUnitTest")]
namespace Syncfusion.Blazor.Toolkit.Charts
{
    internal interface ISubcomponentTracker
    {
        void PushSubcomponent();

        void PopSubcomponent();
    }

    public partial class SfChart : SfDataBoundComponent, ISubcomponentTracker
    {
        #region Constants

        private const int UpdateThresholdMs = 100;
        private const int ChartSmallThreshold = 200;
        private const int ChartMediumThreshold = 300;
        private const int ChartLargeThreshold = 400;
        private const int ChartDefaultHeight = 450;
        private const int ChartDefaultWidth = 600;
        private const string NullDimensionValue = "null";
        #endregion

        #region Private Fields

        internal class InteractionState
        {
            public string SvgCursor { get; set; } = "auto";
            public DateTime PreviousMouseMoveReqTime { get; set; } = DateTime.MinValue;
        }

        internal class RenderState
        {
            public bool ShouldChartRender { get; set; } = true;
            public bool IsSizeSet { get; set; }
            public Size AvailableSize { get; set; } = new(600, 450);
            public Rect? InitialRect { get; set; }
        }

        internal class DataState
        {
            public bool UpdateDataSource { get; set; } = true;
            public List<ChartSelectedDataIndex>? SelectedDataIndexes { get; set; }
        }

        internal class JsInteropState
        {
            public int PendingParametersSetCount { get; set; }
            public DateTime PreviousRequestTime { get; set; } = DateTime.MinValue;
            public IJSObjectReference? SvgJsModule { get; set; }
            public IJSInProcessObjectReference? SvgJsInProcessModule { get; set; }
        }

        private readonly InteractionState _interaction = new();
        private readonly RenderState _render = new();
        private readonly DataState _currentData = new();
        private readonly JsInteropState _interop = new();

        #endregion

        #region Internal Fields

        // Internal runtime flags and identifiers
        internal string _dataId = "sfChart-" + Guid.NewGuid().ToString();
        internal string _dataLabelTemplateId = string.Empty;
        internal bool _isScriptCalled;
        internal bool _isRefreshed;
        internal bool _isRefreshing;
        internal bool _isSeriesRendered;
        internal bool _seriesChanged;
        internal bool _isScrollBarExist;
        internal bool _isLiveChart;
        internal bool _isOnceRendered;

        internal IJSObjectReference? _chartJsModule;
        internal IJSInProcessObjectReference? _chartJsInProcessModule;

        // Internal SVG rendering and size management properties
        internal string? _svgWidth;
        internal string? _svgHeight;
        internal string? _tabColor;
        internal double _lastSeriesAnimationIndex = -1;
        internal double _maxAnimationDuration;
        internal double _markerHeight;

        // Internal mouse interaction coordinates and state
        internal double _mouseX;
        internal double _mouseY;
        internal double _mouseDownX;
        internal double _mouseDownY;
        internal double _previousMouseMoveX;
        internal double _previousMouseMoveY;

        // Internal focus, template, and rendering state flags
        internal bool _isFocused;
        internal bool _skipRendering;
        internal bool _hasLabelTemplate;
        internal bool _isAxisTemplateCalled;

        internal bool _isLegendTemplateCalled;
        internal bool _needAxisRendering;
        internal bool _isScriptLoaded;
        internal bool _redraw;
        internal bool _isChartFirstRender;
        internal bool _isLegendRendered;
        internal bool _enablePadding;
        internal bool _disableTrackTooltip;
        internal bool _isAdaptiveRendering;
        internal bool _isDoubleTap;
        internal bool _isTouch;
        internal bool _delayRedraw;
        internal bool _startMove;
        internal bool _isChartDrag;
        internal bool _requireInvertedAxis;

        // Internal keyboard navigation and conditional rendering toggles
        internal string _zoomingKeyboardFocusTarget = string.Empty;
        internal bool _shouldRenderMarker = true;
        internal bool _shouldRenderDataLabel = true;
        internal bool _shouldRenderStackLabel = true;

        // Internal adaptive layout size categories
        internal ChartWidthCategory _widthCategory = ChartWidthCategory.Normal;
        internal ChartHeightCategory _heightCategory = ChartHeightCategory.Normal;

        // Internal feature modules and chart renderers
        internal Crosshair? _crosshairModule;
        internal Browser? _browser;
        internal Zoom? _zoomingModule;
        internal Highlight? _highlightModule;
        internal SelectionStyleComponent? _highlightStyle;
        internal Selection? _selectionModule;
        internal SelectionStyleComponent? _selectionStyle;
        internal List<PatternOptions> _selectionPatternCollection = [];
        internal DataEditing? _dataEditingModule;
        internal SvgRendering? _svgRenderer;
        internal ChartThemeStyle? _chartThemeStyle;
        internal List<ChartRenderer> _renderers = [];
        internal List<ChartRenderer> _neededRenderers = [];
        internal Dictionary<string, DynamicPathAnimationOptions> _pathAnimationElements = [];
        internal Dictionary<string, DynamicTextAnimationOptions> _textAnimationElements = [];
        internal List<DynamicRectAnimationOptions> _rectAnimationElements = [];
        internal List<DynamicLastLabelOptions> _dynamicLastLabels = [];
        internal StyleElement? _selectionStyleInstance;
        internal StyleElement? _highlightStyleInstance;

        // Internal chart layout, area type, and core settings
        internal bool _shouldAnimateSeries = true;
        internal ChartMargin _margin = new();
        internal ChartZoomSettings _zoomSettings = new();
        internal List<ChartSelectedDataIndex> SelectedDataIndexes { get; set; } = [];
        internal DomRect _secondaryElementOffset = new();

        // Internal element references and DotNet object references
        internal ElementReference _element;
        internal DotNetObjectReference<object>? _chartDotNetReference;
        internal ElementReference _svgElement;

        // Internal series, axis, and data collections
        internal List<ChartSeriesRenderer> _visibleSeriesRenderers = [];
        internal ChartStackLabelSettings? _stackLabelSettings;
        internal ChartStackLabelRenderer? _stackLabelRenderer;
        internal List<IRect> _seriesClipRects = [];
        internal List<IMarkerSettingModel> _seriesMarkers = [];
        internal List<IChartEventBorder> _seriesBorders = [];
        internal List<IAxis> _axes = [];
        internal List<PatternOptions> _highLightPatternCollection = [];
        internal ChartAnnotations _annotations = new();
        internal DomRect _elementOffset = new();
        /*To store the SVGElement's dimensions value.*/
        internal DomRect _svgElementOffset = new();
        /*To store the stock chart SVGElement's dimensions value.*/
        internal DomRect _stockSvgElementOffset = new();

        // Internal template and UI component containers
        internal DataLabelTemplateContainer? _datalabelTemplateContainer;
        internal LegendItemTemplateContainer? _legendItemTemplateContainer;
        internal SvgSelectionRectCollection? _parentRect;
        internal ChartLegendRenderer? _legendRenderer;
        internal CustomLegendRenderer? _customLegendRenderer;
        internal ChartSeriesRendererContainer? _seriesContainer;
        internal ChartAxisRendererContainer? _axisContainer;
        internal ChartAxisOutsideContainer? _axisOutSideContainer;
        internal ChartColumnRendererContainer? _columnContainer;
        internal ChartRowRendererContainer? _rowContainer;
        internal AxisLabelTemplateContainer? _axisLabelTemplateContainer;
        internal ChartBorderRenderer? _chartBorderRenderer;
        internal ChartAreaRenderer? _chartAreaRenderer;
        internal ChartTitleRenderer? _chartTitleRenderer;
        internal ChartAnnotationRendererContainer? _annotationContainer;
        internal ChartStriplineBehindContainer? _striplineBehindContainer;
        internal ChartStriplineOverContainer? _striplineOverContainer;
        internal ChartTrendlineContainer? _trendlineContainer;
        internal ChartRenderer? _chartRender;

        // Internal interactive feature settings and modules
        internal ChartTooltipSettings _tooltip = new();
        internal ChartSorting _sorting = new();
        internal ChartCrosshairSettings _crosshair = new();
        internal ChartTooltip? _tooltipModule;
        internal MarkerExplode? _markerExplode;
        internal ChartTooltipComponent? _templateTooltip;
        internal ChartTooltipComponent? _striplineTooltip;
        internal TooltipData? _tooltipsData;
        internal SvgAxisGroup? _crossGroup;
        internal TrimTooltipBase? _trimTooltip;
        internal ChartStriplineTooltipSettings? _striplineTooltipModule;
        internal RenderFragment<object>? _template;
        internal ChartAnnotations _annotationsContainer = new();
        internal NoDataTemplateContainer? _noDataTemplateContainer;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the <see cref="IStringLocalizer"/> service used for localizing
        /// strings within this component.
        /// </summary>
        /// <remarks>
        /// This property is injected by the Blazor framework and provides access to
        /// localized resources. Use it to retrieve culture-specific text for UI elements.
        /// </remarks>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        [Inject]
        protected IStringLocalizer Localizer { get; set; } = default!;

        /// <summary>
        /// Gets or sets the child content to be rendered inside the <see cref="SfChart"/> component.
        /// </summary>
        /// <value>
        /// A <see cref="RenderFragment"/> representing the child content within the chart component.
        /// </value>
        /// <remarks>
        /// Use this property to define the content inside the <see cref="SfChart"/> component, such as additional components, HTML elements, or text.
        /// </remarks>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; } = null!;

        /// <summary>
        /// Gets or sets the available size used for layout calculations.
        /// </summary>
        internal Size AvailableSize
        {
            get => _render.AvailableSize;
            set
            {
                if (_render.AvailableSize != value)
                {
                    _render.AvailableSize = value;
                    _render.IsSizeSet = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the initial chart rectangle used for layout.
        /// </summary>
        internal Rect InitialRect
        {
            get => _render.InitialRect ?? null!;
            set
            {
                if (_render.InitialRect != value)
                {
                    _render.InitialRect = value;
                    _render.IsSizeSet = true;
                }
            }
        }
        #endregion

        #region Events
        /// Mouse wheel event for internal chart usage.
        internal event EventHandler<ChartMouseWheelArgs>? WheelEvent;
        /// Mouse click event for internal chart usage.
        internal event EventHandler<ChartInternalMouseEventArgs>? MouseClick;
        /// Mouse move event for internal chart usage.
        internal event EventHandler<ChartInternalMouseEventArgs>? MouseMove;
        /// Mouse down event for internal chart usage.
        internal event EventHandler<ChartInternalMouseEventArgs>? MouseDown;
        /// Mouse up event for internal chart usage.
        internal event EventHandler<ChartInternalMouseEventArgs>? MouseUp;
        /// Mouse cancel event for internal chart usage.
        internal event EventHandler<ChartInternalMouseEventArgs>? MouseCancel;
        #endregion

        #region Private Methods

        /// <summary>
        /// Retrieves browser and device information asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task GetBrowserDeviceInfoAsync()
        {
            _browser = await InvokeAsync<Browser>(_chartJsModule!, _chartJsInProcessModule!, "getBrowserDeviceInfo", []).ConfigureAwait(false);
            if (_zoomingModule is null)
            {
                return;
            }
            _zoomingModule.Browser = _browser;
        }

        /// <summary>
        /// Determines whether any axis in the chart has a strip line tooltip enabled.
        /// </summary>
        /// <returns>
        /// <c>true</c> if at least one axis has strip line tooltip enabled; otherwise, <c>false</c>.
        /// </returns>
        private bool GetStripLineTooltip()
        {
            bool isStripLineTooltip = false;
            for (int i = 0; i < _axes.Count; i++)
            {
                if (_axes[i].IsStripLineTooltip)
                {
                    isStripLineTooltip = true;
                    break;
                }
            }
            return isStripLineTooltip;
        }

        /// <summary>
        /// Gets the selection and highlight options for the chart.
        /// </summary>
        /// <returns>A <see cref="SelectionHighlightOptions"/> object containing configuration settings.</returns>
        private SelectionHighlightOptions GetSelectionHighlightOptions()
        {
            List<string> seriestypes = [];
            _visibleSeriesRenderers.ForEach(item =>
            {
                if (item.Series?.SeriesType is not null)
                {
                    seriestypes.Add(item.Series.SeriesType);
                }
            });
            return new SelectionHighlightOptions()
            {
                PinchZoomingEnable = _zoomSettings is not null && _zoomSettings.EnablePinchZooming,
                ToggleVisibility = _legendRenderer?.Legend is null || _legendRenderer.Legend.ToggleVisibility,
                EnableHighlight = _legendRenderer?.Legend is not null && _legendRenderer.Legend.EnableHighlight,
                HighlightMode = Enum.GetName(HighlightMode) ?? null!,
                SelectionMode = Enum.GetName(SelectionMode) ?? null!,
                SeriesTypes = [.. seriestypes],
                SelectedDataIndexes = [.. SelectedDataIndexes],
                HighlightColor = HighlightColor,
                HighlightPattern = Enum.GetName(HighlightPattern) ?? null!,
                SelectionPattern = Enum.GetName(SelectionPattern) ?? null!,
                AllowMultiSelection = AllowMultiSelection
            };
        }

        /// <summary>
        /// Gets the zoom settings configuration for the chart.
        /// </summary>
        /// <returns>An <see cref="IChartZoomSettings"/> object containing zoom configuration.</returns>
        private IChartZoomSettings GetChartZoomSettings()
        {
            return new IChartZoomSettings()
            {
                EnableDeferredZooming = _zoomSettings.EnableDeferredZooming,
                EnableMouseWheelZooming = _zoomSettings.EnableMouseWheelZooming,
                EnablePan = _zoomSettings.EnablePan,
                EnablePinchZooming = _zoomSettings.EnablePinchZooming,
                EnableSelectionZooming = _zoomSettings.EnableSelectionZooming,
                Mode = Enum.GetName(_zoomSettings.Mode) ?? null!,
                ToolbarDisplayMode = Enum.GetName(_zoomSettings.ToolbarDisplayMode) ?? null!,
                ThemeStyleSelectionRectFill = _chartThemeStyle?.SelectionRectFill ?? null!,
                ThemeStyleSelectionRectStroke = _chartThemeStyle?.SelectionRectStroke ?? null!,
                IsOnZoomStartCalled = OnZoomStart is not null,
                IsOnZoomEndCalled = OnZoomEnd is not null,
                IsPanning = _zoomingModule is not null && _zoomingModule.IsPanning
            };
        }

        /// <summary>
        /// Gets the scrollbar options for the chart axes.
        /// </summary>
        /// <param name="adaptiveAxes">The list of axes to use for adaptive rendering, or null to use all axes.</param>
        /// <returns>A <see cref="ScrollbarOptions"/> object containing scrollbar configuration.</returns>
        private ScrollbarOptions GetScrollbarOptions(List<IAxis> adaptiveAxes)
        {
            ScrollbarOptions scrollbarOptions = new()
            {
                IsResize = _isResize,
                IsLazyLoad = false,
                Axes = adaptiveAxes ?? _axes,
                ScrollbarThemeStyle = ChartHelper.GetScrollbarThemeColor(Theme.ToString()),
                ChartAreaType = "CartesianAxes",
                Width = _axisContainer?.AxisLayout.SeriesClipRect?.Width ?? 0,
                Height = _axisContainer?.AxisLayout.SeriesClipRect?.Height ?? 0,
                IsScrollExist = true,
                isScrollEventCalled = OnScrollChanged is not null,
                ChartTitleHeight = (_chartTitleRenderer?.TitleSize.Height ?? 0) * (_chartTitleRenderer?.TitleCollection.Count ?? 0),
                ChartTitlePosition = Enum.GetName(_chartTitleRenderer?.TitleStyle?.Position ?? ChartTitlePosition.Top) ?? null!,
                ChartSubTitleHeight = (_chartTitleRenderer?.SubTitleSize.Height ?? 0) * (_chartTitleRenderer?.SubTitleCollection.Count ?? 0),
                Margin = new ChartMargin() { Left = double.IsNaN(_margin.Left) ? 0 : _margin.Left, Top = double.IsNaN(_margin.Top) ? 0 : _margin.Top, Right = double.IsNaN(_margin.Right) ? 0 : _margin.Right, Bottom = double.IsNaN(_margin.Bottom) ? 0 : _margin.Bottom },
                IsLegendVisible = _legendRenderer?.LegendSettings?.Visible ?? true,
                MarkerHeight = _markerHeight,
                EnablePadding = _enablePadding
            };

            return scrollbarOptions;
        }

        /// <summary>
        /// Gets the tooltip options for the chart.
        /// </summary>
        /// <returns>A <see cref="TooltipOptions"/> object containing tooltip configuration.</returns>
        private TooltipOptions GetTooltipOptions()
        {
            return new TooltipOptions()
            {
                AvailableSize = AvailableSize,
                BorderWidth = _chartBorderRenderer?.ChartBorder?.Width ?? 0,
                DisableTrackTooltip = _disableTrackTooltip,
                AxisClipRect = _axisContainer?.AxisLayout?.SeriesClipRect ?? new Rect(0, 0, 0, 0),
                IsPointDragging = _dataEditingModule is not null && _dataEditingModule._isPointDragging,
                IsPointMouseDown = _isPointMouseDown,
                IsInverted = _requireInvertedAxis,
                ChartAreaType = "CartesianAxes",
                TooltipFormat = _tooltip.Format,
                MarkerEnable = _tooltip.EnableMarker,
                EnableRTL = EnableRtl,
                EnableHighlight = _tooltip.EnableHighlight,
                Crosshair = _crosshair is not null && !(_widthCategory == ChartWidthCategory.Small || _heightCategory == ChartHeightCategory.Small) ? _crosshair : null!,
                MarkerExplode = _markerExplode is not null,
                ChartRadius = _axisContainer?.AxisLayout?.Radius ?? 0,
                Theme = Theme.ToString() ?? string.Empty,
                ThemeStyleCrosshairLine = _chartThemeStyle?.CrosshairLine ?? string.Empty,
                ThemeStyleCrosshairBackground = _chartThemeStyle?.CrosshairBackground ?? string.Empty,
                ThemeStyleCrosshairFill = _chartThemeStyle?.CrosshairFill ?? string.Empty,
                ThemeStyleCrosshairLabel = _chartThemeStyle?.CrosshairLabel ?? string.Empty,
                ThemeStyleCrosshairTextSize = _chartThemeStyle?.CrosshairTextSize ?? string.Empty,
                ThemeStyleCrosshairFontFamily = _chartThemeStyle?.CrosshairFontFamily ?? string.Empty,
                ThemeStyleCrosshairFontWeight = _chartThemeStyle?.CrosshairFontWeight ?? string.Empty,
                TemplateString = _template is not null ? "tooltip_template" : string.Empty,
                InitialRect = InitialRect,
                SecondaryElementOffset = _secondaryElementOffset,
                TooltipEventCalled = TooltipRender is not null,
                SharedTooltipEventCalled = SharedTooltipRender is not null,
                CrosshairMouseMoveEventCalled = OnCrosshairMove is not null,
                SeriesTooltipTop = 0,
                UseGrouping = UseGroupingSeparator,
                Focusable = Focusable,
            };
        }

        /// <summary>
        /// Determines whether scrollbar is enabled for any axis in the chart.
        /// </summary>
        /// <returns>
        /// <c>true</c> if scrollbar is enabled on at least one axis; otherwise, <c>false</c>.
        /// </returns>
        private bool IsScrollBarEnabled()
        {
            List<ChartAxisRenderer> axisRenderers = _axisContainer?.Renderers.Cast<ChartAxisRenderer>().ToList() ?? null!;
            _isScrollBarExist = false;
            if (axisRenderers is not null)
            {
                foreach (ChartAxisRenderer axisRenderer in axisRenderers)
                {
                    if (((_zoomSettings.EnableScrollbar && axisRenderer.Axis is not null && axisRenderer.Axis.EnableScrollbarOnZooming) || (axisRenderer.Axis is not null && axisRenderer.Axis.ScrollbarSettings.Enable)) && axisRenderer.ShouldRenderScrollbar)
                    {
                        _isScrollBarExist = true;
                    }
                }
            }

            return _isScrollBarExist;
        }

        /// <summary>
        /// Gets the chart instance configuration as a dictionary.
        /// </summary>
        /// <returns>A dictionary containing chart feature flags.</returns>
        private Dictionary<string, object> GetInstance()
        {
            Dictionary<string, object> charts = new()
            {
                { "enableZoom", _zoomSettings.EnableMouseWheelZooming || _zoomSettings.EnablePinchZooming || _zoomSettings.EnableSelectionZooming },
                { "enableSelection", SelectionMode != ChartSelectionMode.None },
                { "enableHighlight", HighlightMode != HighlightMode.None },
                { "showTooltip", _tooltip.Enable }
            };

            return charts;
        }

        /// <summary>
        /// Sets the container size by calculating available size and initial rectangle.
        /// </summary>
        private void SetContainerSize()
        {
            CalculateAvailableSize();
            SetInitialRect();
        }

        /// <summary>
        /// Sets the initial rectangle for the chart area.
        /// </summary>
        private void SetInitialRect()
        {
            double borderWidth = _chartBorderRenderer is not null ? _chartBorderRenderer.ChartBorder?.Width ?? 0 : 0;
            double horizontalMargin = GetChartMargin();
            double verticalMargin = GetChartMargin(true);
            double width = CalculateDimension(AvailableSize.Width, horizontalMargin, _margin.Left, _margin.Right, borderWidth, _widthCategory != ChartWidthCategory.Normal);
            double height = CalculateDimension(AvailableSize.Height, verticalMargin, _margin.Top, _margin.Bottom, borderWidth + 0.25, _heightCategory != ChartHeightCategory.Normal);
            double x = GetInitialCoordinate(horizontalMargin, _margin.Left, _widthCategory != ChartWidthCategory.Normal);
            double y = GetInitialCoordinate(verticalMargin, _margin.Top, _heightCategory != ChartHeightCategory.Normal);
            AdjustRectForScrollbars(ref x, ref y, ref width, ref height);

            InitialRect = new Rect()
            {
                X = x,
                Y = y,
                Width = width,
                Height = height
            };
        }

        /// <summary>
        /// Calculates a dimension (width or height) for the chart.
        /// </summary>
        /// <param name="_availableSize">The total available size.</param>
        /// <param name="adaptiveMargin">The margin to use for adaptive rendering.</param>
        /// <param name="staticStartMargin">The static start margin value.</param>
        /// <param name="staticEndMargin">The static end margin value.</param>
        /// <param name="additionalOffset">Additional offset to subtract (e.g., border width).</param>
        /// <param name="isAdaptive">Whether adaptive rendering is active.</param>
        /// <returns>The calculated dimension value.</returns>
        private double CalculateDimension(double _availableSize, double adaptiveMargin, double staticStartMargin, double staticEndMargin, double additionalOffset, bool isAdaptive)
        {
            double startMargin = isAdaptive
                ? adaptiveMargin
                : !double.IsNaN(staticStartMargin)
                    ? staticStartMargin
                    : (SyncfusionService?.IsDeviceMode == true ? 5 : 10);
            double endMargin = isAdaptive
                ? adaptiveMargin
                : !double.IsNaN(staticEndMargin)
                    ? staticEndMargin
                    : (SyncfusionService?.IsDeviceMode == true ? 5 : 10);
            return _availableSize - (startMargin + endMargin + additionalOffset);
        }

        /// <summary>
        /// Gets the initial coordinate (X or Y) for the chart rectangle.
        /// </summary>
        /// <param name="adaptiveMargin">The margin to use for adaptive rendering.</param>
        /// <param name="staticMargin">The static margin value.</param>
        /// <param name="isAdaptive">Whether adaptive rendering is active.</param>
        /// <returns>The initial coordinate value.</returns>
        private double GetInitialCoordinate(double adaptiveMargin, double staticMargin, bool isAdaptive)
        {
            return isAdaptive
                ? adaptiveMargin
                : !double.IsNaN(staticMargin)
                    ? staticMargin
                    : (SyncfusionService?.IsDeviceMode == true ? 5 : 10);
        }

        /// <summary>
        /// Calculates the available size for the chart based on width and height properties.
        /// </summary>
        private void CalculateAvailableSize()
        {
            double height = Height != NullDimensionValue ? ChartHelper.StringToNumber(Height, _elementOffset.Height) : ChartDefaultHeight;
            double width = Width != NullDimensionValue ? ChartHelper.StringToNumber(Width, _elementOffset.Width) : ChartDefaultHeight;

            AvailableSize = new Size(width > 0 ? width : AvailableSize.Width, height > 0 ? height : AvailableSize.Height);
            if (EnableAdaptiveRendering)
            {
                SetAdaptiveWidthCategory();
                SetAdaptiveHeightCategory();
                _isAdaptiveRendering = _widthCategory != ChartWidthCategory.Normal || _heightCategory != ChartHeightCategory.Normal;
            }
        }

        /// <summary>
        /// Sets the adaptive width category based on the chart's width.
        /// </summary>
        private void SetAdaptiveWidthCategory()
        {
            double chartWidth = AvailableSize.Width;
            _widthCategory = ChartWidthCategory.Normal;
            if (chartWidth <= ChartSmallThreshold)
            {
                _widthCategory = ChartWidthCategory.Small;
                _shouldRenderMarker = _shouldRenderDataLabel = false;
                _shouldRenderStackLabel = false;
            }
            else if (chartWidth <= ChartMediumThreshold)
            {
                _widthCategory = ChartWidthCategory.Medium;
            }
            else if (chartWidth <= ChartLargeThreshold)
            {
                _widthCategory = ChartWidthCategory.Large;
            }

            if (_widthCategory != ChartWidthCategory.Small)
            {
                _shouldRenderMarker = _shouldRenderDataLabel = true;
                _shouldRenderStackLabel = true;
            }
        }

        /// <summary>
        /// Sets the adaptive height category based on the chart's height.
        /// </summary>
        private void SetAdaptiveHeightCategory()
        {
            double chartHeight = AvailableSize.Height;
            _heightCategory = ChartHeightCategory.Normal;
            if (chartHeight <= ChartSmallThreshold)
            {
                _heightCategory = ChartHeightCategory.Small;
                _shouldRenderMarker = _shouldRenderDataLabel = false;
            }
            else if (chartHeight <= ChartMediumThreshold)
            {
                _heightCategory = ChartHeightCategory.Medium;
            }
            else if (chartHeight <= ChartLargeThreshold)
            {
                _heightCategory = ChartHeightCategory.Large;
            }
        }

        /// <summary>
        /// Performs the initial animation for series, and trendlines.
        /// </summary>
        /// <returns>A task that represents the asynchronous animation operation.</returns>
        private async Task PerformAnimationAsync()
        {
            List<InitialAnimationInfo> animationInfo = [];
            _seriesContainer?.PerformAnimation(animationInfo);
            _trendlineContainer?.PerformAnimation(animationInfo);
            _shouldAnimateSeries = false;
            if (animationInfo.Count > 0)
            {
                await InvokeVoidAsync(_chartJsModule!, _chartJsInProcessModule!, Constants.DoInitialAnimation, animationInfo, _lastSeriesAnimationIndex).ConfigureAwait(false);
                if (_annotationContainer is not null && _annotationContainer.Elements.Count > 0)
                {
                    _annotationContainer.Elements.ForEach(item =>
                    {
                        if (item is ChartAnnotation annotation && annotation.Renderer is not null)
                        {
                            annotation.Renderer.IsAnnotationRendered = true;
                        }
                    });
                }
            }
        }

        /// <summary>
        /// Performs delayed animation after a throttle period.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task PerformDelayAnimationAsync()
        {
            if (_shouldAnimateSeries && !IsDisposed)
            {
                _interop.PreviousRequestTime = DateTime.Now;
                await Task.Delay(UpdateThresholdMs).ConfigureAwait(true);
                List<InitialAnimationInfo> animationInfo = [];
                _seriesContainer?.PerformAnimation(animationInfo);
                _trendlineContainer?.PerformAnimation(animationInfo);
                await InvokeVoidAsync(_chartJsModule!, _chartJsInProcessModule!, Constants.DoInitialAnimation, animationInfo).ConfigureAwait(true);
                _shouldAnimateSeries = false;
            }
        }

        /// <summary>
        /// Performs selection and highlight operations on the chart.
        /// </summary>
        private void PerformSelection()
        {
            if (_selectionModule is not null)
            {
                _currentData.SelectedDataIndexes = [];
                _currentData.SelectedDataIndexes = _selectionModule.SelectedDataIndexes.GetRange(0, _selectionModule.SelectedDataIndexes.Count);

                _selectionModule.InvokeSelection();
                _selectionModule.AppendSelectionPattern();
            }

            if (_highlightModule is not null)
            {
                _highlightModule.InvokeHighlight();
                _highlightModule.AppendSelectionPattern();
            }

            if (_selectionModule is not null && _currentData.SelectedDataIndexes?.Count > 0)
            {
                _selectionModule.SelectedDataIndexes = _currentData.SelectedDataIndexes.GetRange(0, _currentData.SelectedDataIndexes.Count);
                _ = _selectionModule.RedrawSelectionAsync();
            }
        }

        /// <summary>
        /// Sets character sizes for various fonts used in the chart asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task SetCharSizeAsync()
        {
            await GetCharSizeListAsync(SetFontKeys()).ConfigureAwait(true);
        }

        /// <summary>
        /// Sets font keys for title, subtitle, axes, and legend.
        /// </summary>
        /// <returns>A list of distinct font keys used in the chart.</returns>
        private List<string> SetFontKeys()
        {
            List<string> keys = [_chartTitleRenderer?.GetTitleFontKey() ?? null!, _chartTitleRenderer?.GetSubTitleFontKey() ?? null!];
            if (_axisContainer is not null)
            {
                foreach (KeyValuePair<string, ChartAxis> keyValue in _axisContainer.Axes)
                {
                    ChartAxis axis = keyValue.Value;
                    keys.Add(axis.TitleStyle.GetFontKey(_chartThemeStyle?.AxisTitleFontWeight ?? string.Empty, _chartThemeStyle?.AxisTitleFontFamily ?? string.Empty));
                    keys.Add(axis.CrosshairTooltip.TextStyle.GetFontKey(_chartThemeStyle?.CrosshairFontWeight ?? string.Empty, _chartThemeStyle?.CrosshairFontFamily ?? string.Empty));
                    keys.Add(axis.LabelStyle.GetFontKey(_chartThemeStyle?.AxisLabelFontWeight ?? string.Empty, _chartThemeStyle?.AxisLabelFontFamily ?? string.Empty));
                    axis.StripLines.ForEach(x => keys.Add(x.TextStyle.GetFontKey(_chartThemeStyle?.StriplineFontWeight ?? string.Empty, _chartThemeStyle?.StriplineFontFamily ?? string.Empty)));
                }
            }

            if (_legendRenderer is not null)
            {
                keys.Add(_legendRenderer.GetFontKey());
            }

            return [.. keys.Distinct()];
        }

        /// <summary>
        /// Gets element offset dimensions from the DOM asynchronously.
        /// </summary>
        /// <param name="getElementBounds">The JavaScript method name to invoke for getting bounds.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task GetElementOffsetAsync(string getElementBounds)
        {
            if (!IsDimensionContainsPixel())
            {
                try
                {
                    DomRect elementRect = await InvokeAsync<DomRect>(_chartJsModule!, _chartJsInProcessModule!, getElementBounds, [ID]).ConfigureAwait(true);
                    if (elementRect is not null)
                    {
                        _elementOffset = elementRect;
                    }
                    else
                    {
                        _skipRendering = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error getting element offset: {ex.Message}");
                    _skipRendering = true;
                }
            }
            else
            {
                _skipRendering = true;
            }
        }

        /// <summary>
        /// Determines whether the height and width properties contain pixel units.
        /// </summary>
        /// <returns>
        /// <c>true</c> if both height and width contain "px"; otherwise, <c>false</c>.
        /// </returns>
        private bool IsDimensionContainsPixel()
        {
            return !string.IsNullOrEmpty(Height) && Height.Contains("px", StringComparison.InvariantCulture) && !string.IsNullOrEmpty(Width) && Width.Contains("px", StringComparison.InvariantCulture);
        }

        /// <summary>
        /// Renders the chart frame by calling the prerender process.
        /// </summary>
        private void RenderFrame()
        {
            Prerender();
        }

        /// <summary>
        /// Prerenders all chart containers including series, axes, columns, rows, and annotations.
        /// </summary>
        private void Prerender()
        {
            _seriesContainer?.Prerender();
            _axisContainer?.Prerender();
            _axisOutSideContainer?.Prerender();
            _columnContainer?.Prerender();
            _rowContainer?.Prerender();

            NonDefaultContainer(_annotationContainer ?? null!);
        }

        /// <summary>
        /// Processes the render queue for non-default containers such as annotations.
        /// </summary>
        /// <param name="container">The chart renderer container to process.</param>
        private static void NonDefaultContainer(ChartRendererContainer container)
        {
            if (container.Elements.Count != 0)
            {
                container.Prerender();
            }
        }

        /// <summary>
        /// Processes data for series, and trendlines.
        /// </summary>
        private void ProcessData()
        {
            _seriesContainer?.ProcessData();
            _trendlineContainer?.ProcessData();
        }

        /// <summary>
        /// Processes the render queue for all renderers in the chart.
        /// </summary>
        private void ProcessRenderQueue()
        {
            foreach (ChartRenderer renderer in _renderers)
            {
                renderer.ProcessRenderQueue();
            }
        }

        /// <summary>
        /// Gets the SVG height as a pixel string.
        /// </summary>
        /// <returns>The SVG height with "px" suffix.</returns>
        private string GetSvgHeight()
        {
            double height = !string.IsNullOrEmpty(Height) ? IsRerendering && Height.Contains('%', StringComparison.InvariantCulture) ? AvailableSize.Height :  ChartHelper.StringToNumber(Height, _elementOffset.Height) : ChartDefaultHeight;
            return AvailableSize is not null ? Convert.ToString(height > 0 ? height : AvailableSize.Height, CultureInfo.InvariantCulture) + "px" : "0px";
        }

        /// <summary>
        /// Gets the text direction for the chart container based on RTL settings.
        /// </summary>
        /// <returns>"rtl" if <see cref="EnableRtl"/> is true; otherwise, an empty string.</returns>
        private string GetDirection()
        {
            return EnableRtl ? "rtl" : string.Empty;
        }

        /// <summary>
        /// Renders data label templates by updating their positions.
        /// </summary>
        private void RenderDatalabelTemplate()
        {
            if (_seriesContainer?.Elements is not null)
            {
                foreach (ChartSeries series in _seriesContainer.Elements.Cast<ChartSeries>())
                {
                    series.Marker.DataLabel.Renderer?.UpdateDatalabelTemplatePosition();
                }
            }
        }

        /// <summary>
        /// Gets or sets legend template information including template IDs and sizes.
        /// </summary>
        /// <param name="legendTemplateIdCollection">Collection to populate with template IDs, or null to skip.</param>
        /// <param name="templateSizeList">List of template sizes to assign, or null to skip.</param>
        private void GetSetLegendTemplateInfo(List<string> legendTemplateIdCollection, List<SymbolLocation> templateSizeList)
        {
            if (templateSizeList is null && legendTemplateIdCollection is null)
            {
                return;
            }
            int j = 0;
            if (_legendRenderer is not null)
            {
                foreach (LegendOption legend in _legendRenderer.LegendCollection)
                {
                    legendTemplateIdCollection?.Add(legend.TemplateID ?? string.Empty);
                    if (templateSizeList is not null && j < templateSizeList.Count)
                    {
                        int index = (int)legend.SeriesIndex;
                        if (legend.SeriesIndex >= 0 && _seriesContainer?.Elements is not null && index < _seriesContainer.Elements.Count)
                        {
                            ChartSeries? series = _seriesContainer.Elements[index] as ChartSeries;
                            if (series is not null)
                            {
                                legend.TemplateSize = series.LegendTemplateSize = new Size(templateSizeList[j].X, templateSizeList[j].Y);
                            }
                        }
                        j++;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets data label template information including template IDs and sizes.
        /// </summary>
        /// <param name="templateIdCollection">Collection to populate with template IDs, or null to skip.</param>
        /// <param name="templateSizeList">List of template sizes to assign, or null to skip.</param>
        private void GetSetDataLabelTemplateInfo(List<string> templateIdCollection, List<SymbolLocation> templateSizeList)
        {
            int j = 0;
            if (_seriesContainer?.Renderers is not null)
            {
                foreach (ChartSeriesRenderer series in _seriesContainer.Renderers.Cast<ChartSeriesRenderer>())
                {
                    if (series.Series is not null && series.Series.Visible && series.Points is not null)
                    {
                        foreach (Point point in series.Points.ToArray())
                        {
                            for (int i = 0; i < point.TemplateID.Count; i++)
                            {
                                if (templateIdCollection is not null)
                                {
                                    templateIdCollection.Add(point.TemplateID[i]);
                                }
                                else if (j < templateSizeList.Count)
                                {
                                    point.TemplateSize.Add(new Size(templateSizeList[j].X, templateSizeList[j].Y));
                                    j++;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Calculates the position of secondary elements (tooltips, legends) relative to the chart asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task CalculateSecondaryElementPositionAsync()
        {
            if (_chartJsModule is not null || _chartJsInProcessModule is not null)
            {
                DomRect svgOffset = await InvokeAsync<DomRect>(_chartJsModule!, _chartJsInProcessModule!, Constants.GetElementBoundsById, [SvgId(), false]).ConfigureAwait(false);
                DomRect elementOffset = await InvokeAsync<DomRect>(_chartJsModule!, _chartJsInProcessModule!, Constants.GetElementBoundsById, [ID]).ConfigureAwait(false);
                if (svgOffset is null || elementOffset is null)
                {
                    return;
                }
                _elementOffset = elementOffset;
                _svgElementOffset = svgOffset;
                _secondaryElementOffset.Left = Math.Max(svgOffset.Left - elementOffset.Left, 0);
                _secondaryElementOffset.Top = Math.Max(svgOffset.Top - elementOffset.Top, 0);
                if (_legendRenderer is not null && !string.IsNullOrEmpty(_legendRenderer.KeyboardFocusTarget))
                {
                    await InvokeVoidAsync(_chartJsModule!, _chartJsInProcessModule!, Constants.FocusTarget, [_legendRenderer.KeyboardFocusTarget]).ConfigureAwait(true);
                    _legendRenderer.KeyboardFocusTarget = string.Empty;
                }
            }
        }

        /// <summary>
        /// Gets the SVG width as a pixel string.
        /// </summary>
        /// <returns>The SVG width with "px" suffix.</returns>
        private string GetSvgWidth()
        {
            double width = !string.IsNullOrEmpty(Width) ? ChartHelper.StringToNumber(Width, _elementOffset.Width) : ChartDefaultWidth;
            return AvailableSize is not null ? Convert.ToString(width > 0 ? width : AvailableSize.Width, CultureInfo.InvariantCulture) + "px" : "0px";
        }

        /// <summary>
        /// Increments the pending parameters set counter to track subcomponent initialization.
        /// </summary>
        void ISubcomponentTracker.PushSubcomponent()
        {
            _interop.PendingParametersSetCount++;
        }

        /// <summary>
        /// Decrements the pending parameters set counter and triggers rendering when all subcomponents are initialized.
        /// </summary>
        void ISubcomponentTracker.PopSubcomponent()
        {
            _interop.PendingParametersSetCount--;
            if (_interop.PendingParametersSetCount == 0)
            {
                RenderFrame();
            }
        }

        /// <summary>
        /// Retrieves remote data for series from the data manager asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task GetRemoteDataAsync()
        {
            if (_seriesContainer is null)
            {
                return;
            }

            if (DataManager is null)
            {
                foreach (ChartSeriesRenderer seriesRenderer in _seriesContainer.Renderers.Cast<ChartSeriesRenderer>())
                {
                    if (seriesRenderer.Series?.DataManager is not null && !string.IsNullOrEmpty(seriesRenderer.Series.DataManager.Url))
                    {
                        _ = seriesRenderer.UpdateSeriesDataAsync();

                    }
                }
                return;
            }

            Query query = new();

            object data = await GenerateAndExecuteQueryAsync(query).ConfigureAwait(true);
            if (data is not null)
            {
                _seriesContainer._data = (IEnumerable<object>)data;
            }

        }

        /// <summary>
        /// Generates and executes a data query asynchronously.
        /// </summary>
        /// <param name="query">The query to execute.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the query result as an object.
        /// </returns>
        private async Task<object> GenerateAndExecuteQueryAsync(Query query)
        {
            return await DataManager.ExecuteQuery<object>(query).ConfigureAwait(true);
        }

        /// <summary>
        /// Determines whether panning is currently active in the chart.
        /// </summary>
        /// <returns>
        /// <c>true</c> if panning is enabled and axes are zoomed; otherwise, <c>false</c>.
        /// </returns>
        private bool IsPanning()
        {
            return _zoomingModule is not null && _axisContainer is not null && _zoomingModule.IsAxisZoomed(_axisContainer.Renderers) && (_zoomingModule.IsPanning || (_zoomSettings is not null && _zoomSettings.EnablePan));
        }

        /// <summary>
        /// Initializes private modules including marker explode, tooltip, and crosshair.
        /// </summary>
        private void InitPrivateModules()
        {
            _markerExplode = new MarkerExplode(this);
            if (_tooltip.Enable)
            {
                _tooltipModule = new ChartTooltip(this);
            }

            InitCrossHair();
        }

        /// <summary>
        /// Initializes the crosshair module based on chart settings.
        /// </summary>
        private void InitCrossHair()
        {
            if (_crosshair.Enable)
            {
                _crosshairModule = new Crosshair(this);
            }
            else if (_crosshair.Enable && _crosshairModule is not null)
            {
                _crosshairModule = null;
            }
        }

        /// <summary>
        /// Triggers the <see cref="Loaded"/> event after the chart has finished loading.
        /// </summary>
        private void TriggerLoadedEvent()
        {
            if (Loaded is not null)
            {
                DataVizCommonHelper.InvokeEvent(Loaded, new LoadedEventArgs { Name = "Loaded" });
            }
        }

        /// <summary>
        /// Determines whether the component's renderer has been disposed.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the renderer is disposed; otherwise, <c>false</c>.
        /// </returns>
        private bool IsRendererDisposed()
        {
            const string RENDERHANDLE = "_renderHandle";
            const string RENDERER = "_renderer";
            const string DISPOSED = "_disposed";

            try
            {
                FieldInfo field = GetType().BaseType?.BaseType?.BaseType?.BaseType?.BaseType?.GetField(RENDERHANDLE, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                if (field == null) return false;

                object renderHandlerObj = field.GetValue(this);
                if (renderHandlerObj == null) return false;

                RenderHandle renderHandler = (RenderHandle)renderHandlerObj;
                FieldInfo rendererInfo = renderHandler.GetType().GetField(RENDERER, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                if (rendererInfo == null) return false;

                object renderer = rendererInfo.GetValue(renderHandler);
                if (renderer == null) return false;

                FieldInfo disposedInfo = renderer.GetType().BaseType?.GetField(DISPOSED, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                return disposedInfo is not null && (bool)(disposedInfo.GetValue(renderer) ?? false);
            }
            catch
            {
                // In test environments (like BUnit), reflection might not work as expected
                return false;
            }
        }

        /// <summary>
        /// Updates data for all series in the chart asynchronously.
        /// </summary>
        private async Task UpdateDataAsync()
        {
            if (_seriesContainer is not null)
            {
               
                foreach (ChartSeriesRenderer seriesRenderer in _seriesContainer.Renderers.Cast<ChartSeriesRenderer>())
                {
                    if (seriesRenderer.Series is not null)
                      
                    {
                        _ = await seriesRenderer.Series.UpdateSeriesDataAsync().ConfigureAwait(true);
                    }
                }
            }
        }

        /// <summary>
        /// Gets character sizes for non-Latin characters used in the chart asynchronously.
        /// </summary>
        /// <param name="_updateDataSource">Whether to update the data source.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task GetOtherLanguageCharSizeAsync(bool _updateDataSource)
        {
            if (_updateDataSource)
            {
                List<string> distinctKeys = [];
                GetDistinctCharacter(Title, _chartTitleRenderer?.TitleStyle?.GetFontOptions(_chartThemeStyle ?? null!) ?? null!, distinctKeys);
                GetDistinctCharacter(SubTitle, _chartTitleRenderer?.SubTitleStyle?.GetFontOptions(_chartThemeStyle ?? null!) ?? null!, distinctKeys);
                if (_axisContainer is not null)
                {
                    foreach (KeyValuePair<string, ChartAxis> keyValue in _axisContainer.Axes)
                    {
                        ChartAxis axis = keyValue.Value;
                        GetDistinctCharacter(axis.Title, axis.TitleStyle.GetChartFontOptions(_chartThemeStyle ?? null!), distinctKeys);
                        axis.StripLines.ForEach(x => GetDistinctCharacter(x.Text, x.TextStyle.GetFontOptions(_chartThemeStyle ?? null!), distinctKeys));
                    }
                }

                ChartAxis xAxis, yAxis;
                ChartDataLabel dataLabel;
                bool isCustomLableFormat;
                if (_seriesContainer is not null)
                {
                    foreach (ChartSeriesRenderer seriesRenderer in _seriesContainer.Renderers.Cast<ChartSeriesRenderer>())
                    {
                        xAxis = seriesRenderer.XAxisRenderer?.Axis ?? null!;
                        yAxis = seriesRenderer.YAxisRenderer?.Axis ?? null!;
                        dataLabel = seriesRenderer.Series?.Marker?.DataLabel ?? null!;

                        if (xAxis is null || yAxis is null)
                        {
                            continue;
                        }

                        isCustomLableFormat = yAxis.LabelFormat?.Contains("{value}", StringComparison.InvariantCulture) ?? false;
                        if (xAxis.ValueType == ValueType.Category)
                        {
                             seriesRenderer.XAxisRenderer?.Labels?.ForEach(label => GetDistinctCharacter(label, xAxis.LabelStyle.GetChartFontOptions(_chartThemeStyle ?? null!), distinctKeys));
                        }

                        if (_legendRenderer is not null)
                        {
                            GetDistinctCharacter(seriesRenderer.Series?.Name ?? null!, _legendRenderer.LegendSettings?.TextStyle.GetChartFontOptions(_chartThemeStyle ?? null!) ?? null!, distinctKeys);
                        }

                        if (xAxis.ValueType == ValueType.Double && (xAxis.LabelFormat?.Contains("{value}", StringComparison.InvariantCulture) ?? false))
                        {
                            GetDistinctCharacter(ChartHelper.SplitLabelFormat(xAxis.LabelFormat, "{value}"), xAxis.LabelStyle.GetChartFontOptions(_chartThemeStyle ?? null!), distinctKeys);
                        }

                        if (isCustomLableFormat)
                        {
                            GetDistinctCharacter(ChartHelper.SplitLabelFormat(yAxis.LabelFormat ?? string.Empty, "{value}"), yAxis.LabelStyle.GetChartFontOptions(_chartThemeStyle ?? null!), distinctKeys);
                        }

                        if (dataLabel is not null && dataLabel.Visible && _shouldRenderDataLabel && (isCustomLableFormat || !string.IsNullOrEmpty(dataLabel.Name)))
                        {
                            string label;
                            if (seriesRenderer.Points is not null)
                            {
                                foreach (Point point in seriesRenderer.Points.ToArray())
                                {
                                    label = !string.IsNullOrEmpty(dataLabel.Name) && point.Text is not null ? point.Text : yAxis.LabelFormat?.Replace("{value}", Convert.ToString(point.YValue, CultureInfo.InvariantCulture), StringComparison.InvariantCulture) ?? string.Empty;
                                    GetDistinctCharacter(label, dataLabel.Font?.GetFontOptions(_chartThemeStyle ?? null!) ?? null!, distinctKeys);
                                }
                            }
                        }
                    }
                }
                await LoadCharacterDictionaryAsync(distinctKeys).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Gets distinct characters from text and adds them to the distinct keys collection.
        /// </summary>
        /// <param name="text">The text to analyze.</param>
        /// <param name="font">The font options for the text.</param>
        /// <param name="distinctKeys">The collection to populate with distinct character keys.</param>
        private static void GetDistinctCharacter(string text, ChartFontOptions font, List<string> distinctKeys)
        {
            ChartHelper.GetDistinctCharacter(text, font, distinctKeys);
        }

        /// <summary>
        /// Loads character size dictionary from JavaScript for specified font keys asynchronously.
        /// </summary>
        /// <param name="distinctKeys">The list of font keys to load character sizes for.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task LoadCharacterDictionaryAsync(List<string> distinctKeys)
        {
            if (distinctKeys.Count == 0)
            {
                return;
            }

            string methodName = "getCharSizeByFontKeys";
            string result = await InvokeAsync<string>(_chartJsModule!, _chartJsInProcessModule!, methodName, [distinctKeys]).ConfigureAwait(true);
            if (result is null)
            {
                return;
            }

            Dictionary<string, SymbolLocation> charSizeList = JsonSerializer.Deserialize<Dictionary<string, SymbolLocation>>(result) ?? null!;
            foreach (KeyValuePair<string, SymbolLocation> charSize in charSizeList)
            {
                _ = ChartHelper.SizePerCharacter.TryAdd(charSize.Key, new Size { Width = charSize.Value.X, Height = charSize.Value.Y });
            }
        }

        /// <summary>
        /// Sets the SVG element dimensions asynchronously via JavaScript interop.
        /// </summary>
        /// <param name="methodName">The JavaScript method name to invoke.</param>
        private async Task SetSvgDimensionAsync(string methodName)
        {
            if (_svgElement.Id is not null)
            {
                await InvokeVoidAsync(_chartJsModule!, _chartJsInProcessModule!, methodName, [_svgElement, AvailableSize.Width.ToString(CultureInfo.InvariantCulture) + "px", AvailableSize.Height.ToString(CultureInfo.InvariantCulture) + "px", Focusable, _isFocused]).ConfigureAwait(true);
                _isFocused = true;
            }
        }

        /// <summary>
        /// Unwires event listeners and cleans up resources asynchronously.
        /// </summary>
        private async Task UnWireEventsAsync()
        {
            if (IsRendered && !IsDisposed)
            {
                ChartHelper.ClearStaticStorage();
                await InvokeVoidAsync(_chartJsModule!, _chartJsInProcessModule!, "destroy", [_dataId]).ConfigureAwait(true);
                await WindowInstanceDisposeAsync(_dataId).ConfigureAwait(true);
            }
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Gets a localized label for the specified text key.
        /// </summary>
        /// <param name="text">The text key to localize.</param>
        /// <returns>The localized text, or an empty string if localization provider is not available.</returns>
        internal string GetLocalizedLabel(string text)
        {
            return Localizer[text] ?? string.Empty;
        }

        /// <summary>
        /// Performs layout calculations for the chart asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        internal async Task PerformLayoutAsync()
        {
            _neededRenderers.Clear();
            InitiAxis();
            if (_render.IsSizeSet && _interop.PendingParametersSetCount == 0)
            {
                ProcessData();
                await GetOtherLanguageCharSizeAsync(_currentData.UpdateDataSource).ConfigureAwait(true);
                SetContainerSize();
                Rect initialClipRect = InitialRect;

                foreach (ChartRenderer renderer in _renderers)
                {
                    if (renderer.Equals(_noDataTemplateContainer))
                    {
                        continue;
                    }
                    if (_hasLabelTemplate || _isLegendRendered)
                    {
                        break;
                    }
                    renderer.HandleChartSizeChange(initialClipRect);
                    if (renderer.IsRendererUpdate)
                    {
                        _neededRenderers.Add(renderer);
                    }
                }
                if (NoDataTemplate is not null)
                {
                    _noDataTemplateContainer?.HandleChartSizeChange(initialClipRect);
                }
                _isChartFirstRender = true;
                ProcessRenderQueue();
            }
        }

        /// <summary>
        /// Initializes axes and assigns them to series, trendlines, rows, and columns.
        /// </summary>
        internal void InitiAxis()
        {
            IsInvertedAxis();
            _axisContainer?.AssignAxisToSeries(_seriesContainer?._elementsRequiredAxis ?? null!);
            _trendlineContainer?.AssignAxisToTrendline();
            _rowContainer?.AssignAxisToRow();
            _columnContainer?.AssignAxisToColumn();
        }

        /// <summary>
        /// Determines whether the chart requires inverted axes based on series types.
        /// </summary>
        internal void IsInvertedAxis()
        {
            _requireInvertedAxis = false;
            if (_seriesContainer?.Elements is null || _seriesContainer.Elements.Count == 0)
            {
                return;
            }
            foreach (IChartElement element in _seriesContainer.Elements)
            {
                ChartSeries series = element as ChartSeries ?? null!;
                // Use the strongly-typed enum for detection to avoid timing issues
                // where SeriesType (string) may not yet be populated during initialization.
                bool isBarTypeSeries = series.Type is ChartSeriesType.Bar or ChartSeriesType.StackingBar or ChartSeriesType.StackingBar100;

                if ((isBarTypeSeries && !IsTransposed) || (!isBarTypeSeries && IsTransposed))
                {
                    _requireInvertedAxis = true;
                    return;
                }
            }
        }

        /// <summary>
        /// Triggers data update for scatter and bubble chart series.
        /// </summary>
        internal void TriggerDataUpdateForScatterAndBubbleChart()
        {
            if (_visibleSeriesRenderers.Count > 0)
            {
                List<ChartSeriesRenderer> availableSeriesRenderer = [.. _visibleSeriesRenderers.Where(renderer => renderer is not null && renderer.Series is not null && (renderer.Series.Type == ChartSeriesType.Scatter ||
                renderer.Series.Type == ChartSeriesType.Bubble) && renderer.Series.EnableTooltip)];

                if (availableSeriesRenderer.Count > 0)
                {
                    foreach (ChartSeriesRenderer seriesRenderer in availableSeriesRenderer)
                    {
                        seriesRenderer.UpdateDirection();
                    }
                }
            }
        }

        /// <summary>
        /// Gets chart point data and prepares it for tooltip and crosshair interactions.
        /// </summary>
        internal void GetChartPoints()
        {
            _seriesClipRects.Clear();
            _seriesMarkers.Clear();
            _seriesBorders.Clear();
            GetAxisValues();
            List<string> chartData = [];
            List<string> indicatorData = [];
            List<string> trendlineData = [];
            List<IRect> indicatorClipRects = [];
            List<IRect> trendlineClipRects = [];
            string pointsData = string.Empty;
            if (_visibleSeriesRenderers is not null)
            {
                List<ChartSeriesRenderer> availableSeriesRenderers = [.. _visibleSeriesRenderers.OrderBy(series => series.Index)];
                bool isSeriesAvailable = availableSeriesRenderers.Where(series => series.Series is not null && series.Series.Visible).ToList().Count > 0;
                for (int i = 0; i < availableSeriesRenderers.Count; i++)
                {
                    ChartSeriesRenderer seriesRenderer = availableSeriesRenderers[i];
                    if (seriesRenderer is not null && seriesRenderer.Series is not null && seriesRenderer.Series.Visible)
                    {
                        pointsData = seriesRenderer.ChartData?.ToString() ?? null!;
                        Rect sRect = seriesRenderer.ClipRect ?? null!;
                        if (sRect is not null)
                        {
                            if (seriesRenderer.Container is ChartTrendlineContainer)
                            {
                                trendlineClipRects.Add(new IRect(sRect.X, sRect.Y, sRect.Width, sRect.Height));
                            }
                            else
                            {
                                _seriesClipRects.Add(new IRect(sRect.X, sRect.Y, sRect.Width, sRect.Height));
                            }
                        }
                        if (seriesRenderer.Series.Border is not null)
                        {
                            _seriesBorders.Add(new IChartEventBorder() { Color = seriesRenderer.Series.Border.Color, Width = seriesRenderer.Series.Border.Width });
                        }
                        if (seriesRenderer.Series.Marker is not null)
                        {
                            ChartMarker marker = seriesRenderer.Series.Marker;
                            _seriesMarkers.Add(new IMarkerSettingModel
                            {
                                Visible = marker.Visible && _shouldRenderMarker,
                                Border = new IChartEventBorder() { Color = marker.Border.Color, Width = marker.Border.Width },
                                Fill = marker.Fill,
                                Height = marker.Height,
                                Width = marker.Width,
                                Shape = Enum.GetName(seriesRenderer.GetMarkerShape(marker)) ?? null!,
                                ImageUrl = marker.ImageUrl?.ToString() ?? string.Empty,
                                Opacity = marker.Opacity,
                                AllowHighlight = marker.AllowHighlight,
                            });
                        }
                        if (seriesRenderer.Series._isSeriesChanged)
                        {
                            _seriesChanged = seriesRenderer.Series._isSeriesChanged;
                            seriesRenderer.Series._isSeriesChanged = !seriesRenderer.Series._isSeriesChanged;
                        }
                    }
                    try
                    {
                        if (!string.IsNullOrEmpty(pointsData) && seriesRenderer?.Series is not null && seriesRenderer.Series.Visible)
                        {
                            if (seriesRenderer.Container is ChartSeriesRendererContainer && !seriesRenderer.Container.IsTrendLine)
                            {
                                chartData.Add("[" + pointsData.Remove((seriesRenderer.ChartData?.Length ?? 0) - 1, 1) + "]");
                            }
                            else if (seriesRenderer.Container is ChartTrendlineContainer && seriesRenderer.Container.IsTrendLine)
                            {
                                trendlineData.Add("[" + pointsData.Remove((seriesRenderer.ChartData?.Length ?? 0) - 1, 1) + "]");
                            }
                        }
                        else if (seriesRenderer?.Series is not null && seriesRenderer.Series.Visible)
                        {
                            chartData.Add("");
                        }
                    }
                    catch
                    {
                        if (!IsDisposed && ((seriesRenderer?.Series is not null && !seriesRenderer.Series.UpdateDataSource) || !_isRefreshed))
                        {
                            throw;
                        }
                    }
                }
                if (indicatorData.Count > 0)
                {
                    chartData.AddRange(indicatorData);
                    _seriesClipRects.AddRange(indicatorClipRects);
                }
                if (trendlineData.Count > 0)
                {
                    chartData.AddRange(trendlineData);
                    _seriesClipRects.AddRange(trendlineClipRects);
                }
                if (chartData.Count > 0 || !isSeriesAvailable)
                {
                    _tooltipsData?.UpdateAttribute(this, chartData);
                }
            }
        }

        /// <summary>
        /// Gets axis values and populates the axes collection.
        /// </summary>
        internal void GetAxisValues()
        {
            _axes?.Clear();
            List<ChartAxisRenderer> axisRenderers = _axisContainer?.Renderers.Cast<ChartAxisRenderer>().ToList() ?? null!;
            for (int k = 0, length = axisRenderers.Count; k < length; k++)
            {
                ChartAxisRenderer axisRenderer = axisRenderers[k];
                ChartAxis axis = axisRenderer.Axis ?? null!;
                _axes?.Add(new IAxis
                {
                    Name = axis.Name,
                    PlaceNextToAxisLine = axis.PlaceNextToAxisLine,
                    Rect = new IRect(axisRenderer.Rect.X, axisRenderer.Rect.Y, axisRenderer.Rect.Width, axisRenderer.Rect.Height),
                    UpdatedRect = new IRect(axisRenderer.UpdatedRect.X, axisRenderer.UpdatedRect.Y, axisRenderer.UpdatedRect.Width, axisRenderer.UpdatedRect.Height),
                    ValueType = axis.AxisValueType ?? null!,
                    LabelPlacement = axis.LabelPlacement == LabelPlacement.BetweenTicks ? "BetweenTicks" : "OnTicks",
                    IsAxisOppositePosition = axis.IsAxisOpposedPosition,
                    Orientation = axisRenderer.Orientation == Orientation.Horizontal ? "Horizontal" : axisRenderer.Orientation == Orientation.Vertical ? "Vertical" : "Null",
                    PlotOffset = axis.PlotOffset,
                    VisibleRange = axisRenderer.VisibleRange,
                    ActualRange = axisRenderer.ActualRange,
                    IsAxisInverse = axis.IsAxisInverse,
                    DateFormat = GetFormat(axisRenderer.DateFormat ?? string.Empty),
                    LabelFormat = GetFormat(axis.LabelFormat),
                    LabelPosition = axis.Renderer?.LabelPosition == AxisPosition.Outside ? "Outside" : "Inside",
                    Labels = axisRenderer.Labels,
                    ScrollbarSettingsEnable = axis.ScrollbarSettings.Enable,
                    ScrollBarHeight = axis.ScrollBarHeight,
                    IsStack100 = axisRenderer.IsStack100,
                    ActualIntervalType = axis.AxisActualIntervalType,
                    RangeIntervalType = Enum.Parse<RangeIntervalType>(axis.AxisActualIntervalType).ToString(),
                    Format = GetFormat(axis.Format),
                    LogBase = axis.LogBase,
                    CrosshairTooltip = axis.CrosshairTooltip,
                    StartAngle = axis.StartAngle,
                    DataTimeInterval = axisRenderer.DateTimeInterval,
                    VisibleInterval = axisRenderer.VisibleInterval,
                    VisibleLabelCount = axisRenderer.VisibleLabels.Count,
                    ZoomFactor = axis.ZoomFactor,
                    ZoomPosition = axis.ZoomPosition,
                    ScrollbarSettings = axis.ScrollbarSettings,
                    Visible = axis.Visible,
                    MaxPointLength = axisRenderer.MaxPointLength,
                    AxisLineStyleWidth = axis.LineStyle.Width,
                    IsUniversalDateTime = IsUniversalDateTime(axisRenderer),
                    IsStripLineTooltip = axisRenderer.IsStripLineTooltip
                });
            }
        }

        /// <summary>
        /// Determines whether DateTime values are in universal (UTC) format for the specified axis.
        /// </summary>
        /// <param name="axisRenderer">The axis renderer to check.</param>
        /// <returns>
        /// <c>true</c> if the axis uses universal DateTime; otherwise, <c>false</c>.
        /// </returns>
        internal static bool IsUniversalDateTime(ChartAxisRenderer axisRenderer)
        {
            bool isUniversal = false;
            if ((axisRenderer.Axis?.ValueType == ValueType.DateTime || axisRenderer.Axis?.ValueType == ValueType.DateTimeCategory) && axisRenderer.SeriesRenderer is not null)
            {
                foreach (ChartSeriesRenderer chartSeries in axisRenderer.SeriesRenderer)
                {
                    if (chartSeries is not null && chartSeries.Points?.Count > 0 && DateTime.TryParse(chartSeries.Points[0].X.ToString(), out DateTime temp))
                    {
                        isUniversal = chartSeries.XAxisRenderer.IsDateOnly || chartSeries.XAxisRenderer.IsTimeOnly || chartSeries.IsDateTimeOffset
                            ? Convert.ToDateTime(Convert.ToString(chartSeries.Points[0].X, CultureInfo.InvariantCulture), CultureInfo.InvariantCulture).Kind == DateTimeKind.Utc
                            : Convert.ToDateTime(chartSeries.Points[0].X, CultureInfo.InvariantCulture).Kind == DateTimeKind.Utc;
                    }
                }
            }

            return isUniversal;
        }

        /// <summary>
        /// Gets the formatted label format string, converting culture-specific patterns.
        /// </summary>
        /// <param name="labelFormat">The label format to process.</param>
        /// <returns>The formatted label format string.</returns>
        internal static string GetFormat(string labelFormat)
        {
            return labelFormat == "d" ? CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern : labelFormat == "T" ? CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern :
                labelFormat == "t" ? CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern : labelFormat;
        }

        /// <summary>
        /// Gets boolean values for UI interactions asynchronously.
        /// </summary>
        internal async Task GetBooleanValuesAsync()
        {
            await Task.Delay(200).ConfigureAwait(true);
            if (_tooltip.Enable || _crosshair.Enable || _markerExplode is not null)
            {
                await InvokeVoidAsync(_chartJsModule!, _chartJsInProcessModule!, "setUIBooleanValues", [_dataId, _isPointMouseDown, _disableTrackTooltip, (_dataEditingModule is not null && _dataEditingModule._isPointDragging)]).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Sets tooltip data for the chart asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        internal async Task SetTooltipDataAsync()
        {
            RemoveTemplateTooltip();
            await InvokeVoidAsync(_chartJsModule!, _chartJsInProcessModule!, "setTooltipData", [_dataId, _seriesClipRects.ToArray(), _seriesMarkers.ToArray(), _seriesBorders.ToArray(), _axes.ToArray(), _tooltip.Template is not null ? null! : _tooltip ?? null!, _seriesContainer?._dateValuePairs as object ?? null!, _seriesContainer?._numberValuePairs as object ?? null!, _axisContainer?.AxisLayout.SeriesClipRect ?? null!, _template is not null ? "tooltip_template" : null!, Theme.ToString()]).ConfigureAwait(true);
        }

        internal async Task SetTooltipStyleAsync(string tooltipDataId)
        {
            await InvokeVoidAsync(_chartJsModule!, _chartJsInProcessModule!, "setTooltipStyle", [tooltipDataId]).ConfigureAwait(true);
        }

        /// <summary>
        /// Sets zoom options for the chart asynchronously via JavaScript interop.
        /// </summary>
        internal async Task SetZoomOptionsAsync()
        {
            if (_zoomingModule is not null)
            {
                await InvokeVoidAsync(_chartJsModule!, _chartJsInProcessModule!, "setZoomOptions", [_dataId, GetChartZoomSettings()]).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Sets tooltip and crosshair options for the chart asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        internal async Task SetTooltipCrosshairOptionsAsync()
        {
            _template = _tooltip.Template;
            await InvokeVoidAsync(_chartJsModule!, _chartJsInProcessModule!, "setTooltipCrosshairOptions", [_dataId, _tooltip.GetTooltipForScript(), GetTooltipOptions()]).ConfigureAwait(true);
        }

        /// <summary>
        /// Updates axis zoom values based on provided axis data.
        /// </summary>
        /// <param name="axisCollections">Collection of axis data containing zoom factor and position.</param>
        /// <param name="isChartPanning">Whether the update is triggered by chart panning.</param>
        internal void UpdateAxisZoomValues(List<AxisData> axisCollections, bool isChartPanning = false)
        {
            foreach (AxisData axisData in axisCollections)
            {
                ChartAxisRenderer? axisRenderer = (ChartAxisRenderer)(_axisContainer?.Renderers.Find(item => ((ChartAxisRenderer)item).Axis?.GetName() == axisData.AxisName) ?? null!);
                axisRenderer?.Axis?.UpdateZoomValues(axisData.ZoomFactor, axisData.ZoomPosition);
            }
        }

        /// <summary>
        /// Initializes chart modules including zoom, selection, highlight, data editing, and stripline tooltip.
        /// </summary>
        internal void InitModules()
        {
            if (_axisContainer is not null && !_axisContainer._isScrollSettingEnabled && (_zoomSettings.EnableSelectionZooming || _zoomSettings.EnableMouseWheelZooming || _zoomSettings.EnablePinchZooming || _zoomSettings.EnablePan || _zoomSettings.EnableScrollbar || _zoomSettings.ToolbarDisplayMode == ToolbarMode.Always))
            {
                _zoomingModule = new Zoom(this);
            }
            else if (_zoomSettings.EnableSelectionZooming)
            {
                _zoomingModule = new Zoom(this);
            }

            if (_selectionModule is null && SelectionMode != ChartSelectionMode.None)
            {
                _selectionModule = new(this)
                {
                    StyleRender = _selectionStyle ?? null!,
                    ReqPatterns = _selectionPatternCollection
                };
            }

            if (_highlightModule is null && (HighlightMode != HighlightMode.None || (_legendRenderer is not null && _legendRenderer.Legend is not null && _legendRenderer.Legend.EnableHighlight)))
            {
                _highlightModule = new(this)
                {
                    StyleRender = _highlightStyle ?? null!,
                    ReqPatterns = _highLightPatternCollection
                };
            }

            _dataEditingModule ??= new DataEditing(this);

            _striplineTooltipModule ??= new ChartStriplineTooltipSettings(this);
        }

        /// <summary>
        /// Initializes the static chart by calculating size and setting the initial rectangle.
        /// </summary>
        internal void InitializeStaticChart()
        {
            try
            {
                if (!_render.IsSizeSet)
                {
                    CalculateAvailableSize();
                    SetInitialRect();
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
        /// Invokes an event delegate asynchronously with the specified event arguments.
        /// </summary>
        /// <typeparam name="T">The type of event arguments.</typeparam>
        /// <param name="eventFn">The event callback delegate.</param>
        /// <param name="eventArgs">The event arguments to pass.</param>
        internal static async Task InvokeDelegateAsync<T>(object eventFn, T eventArgs)
        {
            if (eventFn is not null)
            {
                if (eventFn is EventCallback<T> asyncDelegate)
                {
                    await asyncDelegate.InvokeAsync(eventArgs).ConfigureAwait(true);
                }
            }
        }

        /// <summary>
        /// Adjusts the chart rectangle for scrollbar presence by modifying dimensions and position.
        /// </summary>
        /// <param name="x">The X coordinate to adjust.</param>
        /// <param name="y">The Y coordinate to adjust.</param>
        /// <param name="width">The width to adjust.</param>
        /// <param name="height">The height to adjust.</param>
        internal void AdjustRectForScrollbars(ref double x, ref double y, ref double width, ref double height)
        {
            if (_axisContainer is not null)
            {
                foreach (KeyValuePair<string, ChartAxis> keyValue in _axisContainer.Axes)
                {
                    ChartAxis axis = keyValue.Value;
                    if (axis is null || axis.Renderer is null)
                    {
                        continue;
                    }
                    double totalScrollbarHeight = axis.ScrollbarSettings.Height + Constants.ScrollbarPadding;
                    Orientation orientation = axis.Renderer.Orientation;
                    ScrollbarPosition scrollbarPosition = axis.ScrollbarSettings.Position;
                    bool shouldRenderScrollbar = axis.Renderer.ShouldRenderScrollbar && (!(_zoomingModule is null || !_zoomSettings.EnableScrollbar || !axis.EnableScrollbarOnZooming
                        || (axis.ZoomFactor >= 1 && axis.ZoomPosition <= 0)) || axis.ScrollbarSettings.Enable);
                    if (!shouldRenderScrollbar || scrollbarPosition == ScrollbarPosition.PlaceNextToAxisLine)
                    {
                        continue;
                    }

                    height -= (orientation == Orientation.Horizontal && (scrollbarPosition == ScrollbarPosition.Bottom || scrollbarPosition == ScrollbarPosition.Top)) ? totalScrollbarHeight : 0;
                    y += (orientation == Orientation.Horizontal && scrollbarPosition == ScrollbarPosition.Top) ? totalScrollbarHeight : 0;
                    width -= (orientation == Orientation.Vertical && (scrollbarPosition == ScrollbarPosition.Right || scrollbarPosition == ScrollbarPosition.Left)) ? totalScrollbarHeight : 0;
                    x += (orientation == Orientation.Vertical && scrollbarPosition == ScrollbarPosition.Left) ? totalScrollbarHeight : 0;
                }
            }
        }

        /// <summary>
        /// Gets the appropriate margin value for the chart based on adaptive rendering categories.
        /// </summary>
        /// <param name="isTopOrBottom">Whether to get top/bottom margin (true) or left/right margin (false).</param>
        /// <returns>The margin value in pixels.</returns>
        internal double GetChartMargin(bool isTopOrBottom = false)
        {
            return !isTopOrBottom
                ? _widthCategory == ChartWidthCategory.Small ? 1 : _widthCategory == ChartWidthCategory.Medium ? 5 : _widthCategory == ChartWidthCategory.Large ? 8 : 10
                : _heightCategory == ChartHeightCategory.Small ? 1 : _heightCategory == ChartHeightCategory.Medium ? 5 : _heightCategory == ChartHeightCategory.Large ? 8 : 10;
        }

        /// <summary>
        /// Performs redraw animation asynchronously for dynamic chart updates.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        internal async Task PerformRedrawAnimationAsync()
        {
            if (_redraw && !IsDisposed && (_interop.PreviousRequestTime == DateTime.MinValue || (DateTime.Now - _interop.PreviousRequestTime).TotalMilliseconds > UpdateThresholdMs))
            {
                _interop.PreviousRequestTime = DateTime.Now;
                await Task.Delay(UpdateThresholdMs).ConfigureAwait(true);
                _redraw = false;
                await InvokeVoidAsync(_chartJsModule!, _chartJsInProcessModule!, "doDynamicAnimation", [_pathAnimationElements.Values.ToArray(), _rectAnimationElements, _textAnimationElements.Values.ToArray(), _dynamicLastLabels]).ConfigureAwait(true);
                await UpdateDatalabelTemplateAsync().ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Refreshes and clears redraw element collections if not currently redrawing.
        /// </summary>
        internal void RefreshRedrawElements()
        {
            if (!_redraw)
            {
                _pathAnimationElements.Clear();
                _textAnimationElements.Clear();
                _rectAnimationElements.Clear();
                _dynamicLastLabels.Clear();
            }
        }

        /// <summary>
        /// Gets character size list for specified font keys asynchronously.
        /// </summary>
        /// <param name="fontKeys">The list of font keys to retrieve character sizes for.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        internal async Task GetCharSizeListAsync(List<string> fontKeys)
        {
            List<string> uniqueKeys = [];
            foreach (string fontKey in fontKeys)
            {
                if (!ChartHelper.ChartFontKeys.Contains(fontKey))
                {
                    uniqueKeys.Add(fontKey);
                    ChartHelper.ChartFontKeys.Add(fontKey);
                }
            }

            if (uniqueKeys.Count == 0)
            {
                return;
            }

            string methodName = "getCharCollectionSize";
            string result = await InvokeAsync<string>(_chartJsModule!, _chartJsInProcessModule!, methodName, false, [uniqueKeys]).ConfigureAwait(true);
            if (result is null)
            {
                return;
            }

            string[] result_2 = JsonSerializer.Deserialize<string[]>(result) ?? null!;
            int i = 33, j = 0;
            foreach (string width in result_2)
            {
                _ = ChartHelper.SizePerCharacter.TryAdd(Convert.ToChar(i) + Constants.Underscore + uniqueKeys[j], new Size { Width = Convert.ToInt16(width, null), Height = 133 });
                i++;
                if (i > 590)
                {
                    i = 33;
                    j++;
                }
            }
        }

        /// <summary>
        /// Adds an axis to the chart's axis container.
        /// </summary>
        /// <param name="axis">The axis to add.</param>
        internal void AddAxis(ChartAxis axis)
        {
            _axisContainer?.AddElement(axis);
        }

        /// <summary>
        /// Adds a column definition to the chart's column container.
        /// </summary>
        /// <param name="column">The column to add.</param>
        internal void AddColumn(ChartColumn column)
        {
            _columnContainer?.AddElement(column);
        }

        /// <summary>
        /// Removes a column definition from the chart's column container.
        /// </summary>
        /// <param name="column">The column to remove.</param>
        internal void RemoveColumn(ChartColumn column)
        {
            _columnContainer?.RemoveElement(column);
        }

        /// <summary>
        /// Adds a row definition to the chart's row container.
        /// </summary>
        /// <param name="row">The row to add.</param>
        internal void AddRow(ChartRow row)
        {
            _rowContainer?.AddElement(row);
        }

        /// <summary>
        /// Removes a row definition from the chart's row container.
        /// </summary>
        /// <param name="row">The row to remove.</param>
        internal void RemoveRow(ChartRow row)
        {
            _rowContainer?.RemoveElement(row);
        }

        /// <summary>
        /// Removes an axis from the chart's axis container.
        /// </summary>
        /// <param name="axis">The axis to remove.</param>
        internal void RemoveAxis(ChartAxis axis)
        {
            _axisContainer?.RemoveElement(axis);
        }

        /// <summary>
        /// Adds a series to the chart's series container.
        /// </summary>
        /// <param name="series">The series to add.</param>
        internal void AddSeries(ChartSeries series)
        {
            ChartSeriesRenderer defaultRenderer = _seriesContainer?.Renderers.Find(renderer => renderer.GetType().Equals(typeof(DefaultSeriesRenderer))) as ChartSeriesRenderer ?? null!;
            if (defaultRenderer is not null)
            {
                _seriesContainer?.RemoveRenderer(defaultRenderer);
            }

            if (_seriesContainer?.Elements.Count > 0 && !_seriesContainer.Elements.Contains(series))
            {
                bool isBar = series.Type is ChartSeriesType.Bar or ChartSeriesType.StackingBar or ChartSeriesType.StackingBar100;
                switch (series.Type)
                {
                    case ChartSeriesType.Bar:
                    case ChartSeriesType.StackingBar:
                    case ChartSeriesType.StackingBar100:
                        if (isBar)
                        {
                            _seriesContainer.AddElement(series);
                        }

                        break;
                    default:
                        if (!isBar)
                        {
                            _seriesContainer.AddElement(series);
                        }

                        break;
                }
            }
            else if (_seriesContainer is not null && !_seriesContainer.Elements.Contains(series))
            {
                _seriesContainer.AddElement(series);
            }
        }

        internal void RemoveSeries(ChartSeries series)
        {
            _seriesContainer?.RemoveElement(series);
        }

        /// <summary>
        /// Adds an annotation to the chart.
        /// </summary>
        /// <param name="annotation">The annotation to add.</param>
        internal void AddAnnotation(ChartAnnotation annotation)
        {
            _annotationContainer?.AddElement(annotation);
        }

        /// <summary>
        /// Removes an annotation from the chart.
        /// </summary>
        /// <param name="annotation">The annotation to remove.</param>
        internal void RemoveAnnotation(ChartAnnotation annotation)
        {
            if (IsDisposed)
            {
                return;
            }

            if (_annotationContainer is not null)
            {
                _annotationContainer.RemoveRenderer(annotation.Renderer ?? null!);
                _annotationContainer.RemoveElement(annotation);
                _annotationContainer.InvalidateRenderer();
            }
        }

        /// <summary>
        /// Adds a stripline to the chart at the specified Z-index position.
        /// </summary>
        /// <param name="stripline">The stripline to add.</param>
        /// <param name="position">The Z-index position (behind or over series).</param>
        internal void AddStripline(ChartStripline stripline, ZIndexPosition position)
        {
            if (position == ZIndexPosition.Behind)
            {
                _striplineBehindContainer?.AddElement(stripline);
            }
            else
            {
                _striplineOverContainer?.AddElement(stripline);
            }
            if (_isScriptLoaded)
            {
                _ = ProcessOnLayoutChangeAsync();
            }
        }

        /// <summary>
        /// Removes a stripline from the chart.
        /// </summary>
        /// <param name="stripline">The stripline to remove.</param>
        /// <param name="position">The Z-index position of the stripline.</param>
        internal void RemoveStripline(ChartStripline stripline, ZIndexPosition position)
        {
            if (position == ZIndexPosition.Behind)
            {
                _striplineBehindContainer?.RemoveRenderer(stripline.Renderer ?? null!);
                _striplineBehindContainer?.RemoveElement(stripline);
            }
            else
            {
                _striplineOverContainer?.RemoveRenderer(stripline.Renderer ?? null!);
                _striplineOverContainer?.RemoveElement(stripline);
            }
            _ = ProcessOnLayoutChangeAsync();
        }

        /// <summary>
        /// Adds a trendline to the chart's trendline container.
        /// </summary>
        /// <param name="trendline">The trendline to add.</param>
        internal void AddTrendline(ChartTrendline trendline)
        {
            _trendlineContainer?.AddElement(trendline);
        }

        /// <summary>
        /// Delays layout change processing with throttling to avoid excessive updates.
        /// </summary>
        /// <param name="isZoom">Whether the layout change is triggered by zoom operations.</param>
        internal async Task DelayLayoutChangeAsync(bool isZoom = false)
        {
            _isLayoutChange = true;
            if (!IsDisposed && (_interop.PreviousRequestTime == DateTime.MinValue || (DateTime.Now - _interop.PreviousRequestTime).TotalMilliseconds > UpdateThresholdMs))
            {
                _interop.PreviousRequestTime = DateTime.Now;
                await Task.Delay(UpdateThresholdMs).ConfigureAwait(true);
                if (isZoom && _zoomingModule is not null)
                {
                    _zoomingModule.PerformedUI = true;
                    _zoomingModule.IsPanning = _zoomingModule.IsAxisZoomed(_axisContainer?.Renderers ?? null!);
                    _zoomingModule.IsZoomed = _zoomingModule.IsPanning;
                    SetSvgCursor(_zoomingModule.IsPanning ? "cursor" : "auto");
                    await InvokeVoidAsync(_chartJsModule!, _chartJsInProcessModule!, Constants.UpdateZoomingOptions, [_dataId, _zoomingModule.IsPanning]).ConfigureAwait(true);
                }
                if (!_isLiveChart)
                {
                    OnLayoutChange(_zoomingModule is not null && _zoomingModule.IsWheelZoom);
                    _isLayoutChange = false;
                }
            }
        }

        /// <summary>
        /// Processes layout changes by updating renderers and applying zoom/scrollbar.
        /// </summary>
        /// <param name="skip">Whether to skip secondary element position calculation and tooltip updates.</param>
        internal void OnLayoutChange(bool skip = false)
        {
            if (_render.IsSizeSet && _isChartFirstRender)
            {
                UpdateRenderers(skip);
                ApplyZoomkit();
                UpdateClientSideScrollbar();
            }
        }

        /// <summary>
        /// Updates needed renderers asynchronously by recalculating layout.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        internal async Task UpdateNeededRenderersAsync()
        {
            _svgRenderer?.RefreshElementList();
            RefreshRedrawElements();
            SetInitialRect();
            foreach (ChartRenderer renderer in _neededRenderers)
            {
                renderer.HandleChartSizeChange(InitialRect);
            }

            foreach (ChartRenderer renderer in _neededRenderers)
            {
                renderer.ProcessRenderQueue();
            }
        }

        /// <summary>
        /// Updates all renderers in the chart.
        /// </summary>
        /// <param name="skip">Whether to skip secondary calculations.</param>
        internal void UpdateRenderers(bool skip = false)
        {
            _neededRenderers.Clear();
            _svgRenderer?.RefreshElementList();
            RefreshRedrawElements();
            SetInitialRect();
            foreach (ChartRenderer renderer in _renderers)
            {
                if (_hasLabelTemplate)
                {
                    break;
                }
                renderer.HandleChartSizeChange(InitialRect);
                if (renderer.IsRendererUpdate)
                {
                    _neededRenderers.Add(renderer);
                }
            }

            foreach (ChartRenderer renderer in _renderers)
            {
                renderer.ProcessRenderQueue();
            }

            if (_hasLabelTemplate)
            {
                _ = UpdateAxisLabelTemplateAsync();
                return;
            }

            if (skip)
            {
                return;
            }

            _ = CalculateSecondaryElementPositionAsync();
            if (_tooltip.Enable || _crosshair.Enable || _markerExplode is not null)
            {
                _ = InvokeVoidAsync(_chartJsModule!, _chartJsInProcessModule!, Constants.SetTooltipOptions, [_dataId, _tooltip.GetTooltipForScript(), GetTooltipOptions(), _seriesClipRects.ToArray(), _seriesMarkers.ToArray(), _seriesBorders.ToArray(), _axes.ToArray(), _seriesContainer?._dateValuePairs as object ?? null!, _seriesContainer?._numberValuePairs as object ?? null!]);
                if (_seriesContainer is not null && !IsDisposed && (_seriesContainer._previousRequestTime == DateTime.MinValue || (DateTime.Now - _seriesContainer._previousRequestTime).TotalMilliseconds > UpdateThresholdMs))
                {
                    _seriesContainer._previousRequestTime = DateTime.Now;
                    _seriesContainer.SetGlobalizationValues();
                    _ = UpdateChartPointsAsync();
                }
            }
        }

        /// <summary>
        /// Updates chart points and prepares data for tooltips asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        internal async Task UpdateChartPointsAsync()
        {
            GetChartPoints();
            if (_isScriptLoaded && _isScriptCalled)
            {
                if (_legendRenderer is not null && (_legendRenderer.Legend as ChartLegendSettings ?? null!).Visible && (_legendRenderer.Legend as ChartLegendSettings ?? null!).EnableHighlight)
                {
                    await Task.Delay(UpdateThresholdMs).ConfigureAwait(true);
                    _ = InvokeVoidAsync(_chartJsModule!, _chartJsInProcessModule!, "performLegendClickHighlight", [_dataId]);
                }
            }
        }

        /// <summary>
        /// Gets the SVG element ID for the chart.
        /// </summary>
        /// <returns>The SVG element ID constructed from <see cref="ID"/> with "_svg" suffix.</returns>
        internal string GetSvgId()
        {
            return ID + Constants.Svg;
        }

        /// <summary>
        /// Updates legend template by retrieving template sizes and recalculating layout asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        internal async Task UpdateLegendTemplateAsync()
        {
            List<string> legendTemplateIdCollection = [];
            GetSetLegendTemplateInfo(legendTemplateIdCollection, null!);
            if (legendTemplateIdCollection.Count > 0)
            {
                string result = await InvokeAsync<string>(_chartJsModule!, _chartJsInProcessModule!, Constants.GetDatalabelTemplateBoundsById, [legendTemplateIdCollection]).ConfigureAwait(true);
                if (result is not null)
                {
                    List<SymbolLocation>? templateSizeList = JsonSerializer.Deserialize<List<SymbolLocation>>(result);
                    GetSetLegendTemplateInfo(null!, templateSizeList ?? null!);
                    _isLegendTemplateCalled = true;
                    _legendRenderer?.UpdateLegendTemplatePosition();
                    UpdateRenderers();
                    _isLegendRendered = false;
                    _isLegendTemplateCalled = false;
                }
            }
        }

        /// <summary>
        /// Updates data label template positions asynchronously after retrieving template sizes from DOM.
        /// </summary>
        internal async Task UpdateDatalabelTemplateAsync()
        {
            List<string> templateIdCollection = [];
            GetSetDataLabelTemplateInfo(templateIdCollection, null!);
            if (templateIdCollection.Count > 0)
            {
                string result = await InvokeAsync<string>(_chartJsModule!, _chartJsInProcessModule!, Constants.GetDatalabelTemplateBoundsById, [templateIdCollection]).ConfigureAwait(true);
                List<SymbolLocation> templateSizeList = JsonSerializer.Deserialize<List<SymbolLocation>>(result) ?? null!;
                GetSetDataLabelTemplateInfo(null!, templateSizeList);
                RenderDatalabelTemplate();
            }
        }

        /// <summary>
        /// Updates axis label templates by retrieving template sizes and recalculating layout asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        internal async Task UpdateAxisLabelTemplateAsync()
        {
            if (_axisContainer is not null)
            {
                foreach (ChartAxisRenderer renderer in _axisContainer.Renderers.Cast<ChartAxisRenderer>())
                {
                    renderer.AxisRenderInfo.AxisLabelTemplateSizeList.Clear();
                    if (renderer.Axis?.LabelTemplate is not null)
                    {
                        List<string> axisTemplateIdCollection = [];
                        foreach (VisibleLabels label in renderer.VisibleLabels)
                        {
                            axisTemplateIdCollection.Add(label.TemplateID ?? null!);
                        }
                        string result = await InvokeAsync<string>(_chartJsModule!, _chartJsInProcessModule!, "getAxisLabelTemplatesSize", [axisTemplateIdCollection]).ConfigureAwait(true);
                        List<Size> templateSizeList = JsonSerializer.Deserialize<List<Size>>(result) ?? null!;
                        if (templateSizeList.Count > 0)
                        {
                            int i = 0;
                            foreach (VisibleLabels label in renderer.VisibleLabels)
                            {
                                renderer.AxisRenderInfo.AxisLabelTemplateSizeList.Add(new KeyValuePair<string, Size>(label.TemplateID ?? null!, templateSizeList[i]));
                                i++;
                            }
                        }
                    }
                }
            }
            _isAxisTemplateCalled = true;
            _hasLabelTemplate = false;
            UpdateRenderers();
            _isAxisTemplateCalled = false;
            if (_needAxisRendering)
            {
                _axisContainer?.UpdateAxisRendering();
                _needAxisRendering = false;
            }
        }

        /// <summary>
        /// Sets the Svg cursor style.
        /// </summary>
        /// <param name="cursor">The cursor style to apply.</param>
        /// <param name="isKeyboardFocused">Whether the chart has keyboard focus.</param>
        internal void SetSvgCursor(string cursor, bool isKeyboardFocused = false)
        {
            if (cursor == "null" && IsPanning())
            {
                return;
            }
            _interaction.SvgCursor = cursor;
            _ = SetAttributeAsync(SvgId(), "cursor", cursor, isKeyboardFocused ? ID : string.Empty);
        }

        /// <summary>
        /// Gets the SVG element ID.
        /// </summary>
        /// <returns>The SVG element ID constructed from <see cref="ID"/>.</returns>
        internal string SvgId()
        {
            return ID + Constants.Svg;
        }

        /// <summary>
        /// Sets an attribute on a DOM element asynchronously via JavaScript interop.
        /// </summary>
        /// <param name="id">The element ID.</param>
        /// <param name="key">The attribute name.</param>
        /// <param name="data">The attribute value.</param>
        /// <param name="focusId">The element ID to focus after setting the attribute.</param>
        internal async Task SetAttributeAsync(string id, string key, string data, string focusId)
        {
            await InvokeVoidAsync(_chartJsModule!, _chartJsInProcessModule!, Constants.SetAttribute, [id, key, data, focusId]).ConfigureAwait(true);
        }

        /// <summary>
        /// Applies the zoom toolkit to the chart if zoom module is initialized.
        /// </summary>
        internal void ApplyZoomkit()
        {
            if (!_redraw && _zoomingModule is not null && (!_zoomSettings.EnablePan || _zoomingModule.PerformedUI || _zoomSettings.ToolbarDisplayMode == ToolbarMode.Always))
            {
                _zoomingModule.ApplyZoomToolkit(this, _axisContainer?.Renderers ?? null!);
            }
        }

        /// <summary>
        /// Determines whether the chart has been disposed or its renderer is disposed.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the chart is disposed; otherwise, <c>false</c>.
        /// </returns>
        internal bool ChartDisposed()
        {
            return IsDisposed || IsRendererDisposed();
        }

        /// <summary>
        /// Processes layout change by triggering prerender and refresh operations asynchronously.
        /// </summary>
        internal async Task ProcessOnLayoutChangeAsync()
        {
            const int UPDATETHRESHOLD = 10;
            _isLayoutChange = true;
            if (!IsDisposed && IsRendered && (_interop.PreviousRequestTime == DateTime.MinValue || (DateTime.Now - _interop.PreviousRequestTime).TotalMilliseconds > UPDATETHRESHOLD))
            {
                _interop.PreviousRequestTime = DateTime.Now;
                if (JSRuntime is not JSInProcessRuntime || !_isLiveChart)
                {
                    await Task.Delay(UPDATETHRESHOLD).ConfigureAwait(true);
                }
                Prerender();
                if (!_isLiveChart)
                {
                    await RefreshChartAsync().ConfigureAwait(true);
                }
                ApplyZoomkit();
                UpdateClientSideScrollbar();
                await UpdateDatalabelTemplateAsync().ConfigureAwait(true);
                _isLayoutChange = false;
            }
        }

        /// <summary>
        /// Refreshes the chart by reinitializing axes, processing data, and updating renderers.
        /// </summary>
        internal async Task RefreshChartAsync()
        {
            try
            {
                InitiAxis();
                _seriesContainer?.InitSeriesRendererFields();
                ProcessData();
                await GetOtherLanguageCharSizeAsync(_currentData.UpdateDataSource).ConfigureAwait(true);
                _isRefreshed = true;
                UpdateRenderers();
                PerformSelection();
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
        /// Updates the client-side scrollbar for the chart asynchronously via JavaScript interop.
        /// </summary>
        internal void UpdateClientSideScrollbar()
        {
            if (_axisContainer?.AxisLayout is null)
            {
                return;
            }

            _ = IsScrollBarEnabled();
            List<IAxis> adaptiveAxes = null!;
            if (EnableAdaptiveRendering && _isScrollBarExist)
            {
                foreach (ChartAxisRenderer axisRenderer in _axisContainer.Renderers.Cast<ChartAxisRenderer>().ToList())
                {
                    if (!axisRenderer.ShouldRenderScrollbar)
                    {
                        _ = InvokeVoidAsync(_chartJsModule!, _chartJsInProcessModule!, Constants.RemoveScrollbarSvg, [_dataId, _axes.ToArray()]);
                        adaptiveAxes = axisRenderer.Orientation == Orientation.Horizontal ? [.. _axes.Where(x => x.Orientation == "Vertical")] : [.. _axes.Where(x => x.Orientation == "Horizontal")];
                    }
                }
            }

            if (_isScrollBarExist)
            {
                _ = InvokeVoidAsync(_chartJsModule!, _chartJsInProcessModule!, "setScrollbarOptions", [_dataId, GetScrollbarOptions(adaptiveAxes)]);
                _ = InvokeVoidAsync(_chartJsModule!, _chartJsInProcessModule!, "renderScrollbar", [_dataId, adaptiveAxes is not null ? [.. adaptiveAxes] : _axes.ToArray()]);
            }
            else
            {
                _ = InvokeVoidAsync(_chartJsModule!, _chartJsInProcessModule!, Constants.RemoveScrollbarSvg, [_dataId, _axes.ToArray()]);
            }
        }
        #endregion

    }

    /// <summary>
    /// Represents the base component for data-bound chart components in Syncfusion Blazor Charts.
    /// </summary>
    public class ChartDataBoundComponent : SfDataBoundComponent, ISubcomponentTracker
    {
        #region Fields
        private bool _isPush = true;
        private bool _isPop = true;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the cascading parameter that tracks subcomponent initialization state.
        /// </summary>
        [CascadingParameter]
        internal ISubcomponentTracker? Tracker { get; set; }

        /// <summary>
        /// Gets or sets the content to render inside this <see cref="SfChart"/> component.
        /// </summary>
        /// <value>
        /// A <see cref="RenderFragment"/> representing the child content to be rendered inside the <see cref="SfChart"/> component.
        /// </value>
        /// <remarks>
        /// This property is used to include additional components or HTML elements as children within the <see cref="SfChart"/> component, allowing for customization and enrichment of the chart's visual representation.
        /// </remarks>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; } = null!;
        #endregion

        #region Lifecyle Methods

        /// <summary>
        /// Invoked when the component's parameters are set.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            PushSubcomponent();
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Builds the render tree for the component.
        /// </summary>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/> used to construct the component's render tree.</param>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (builder is null)
            {
                return;
            }

            this.CreateCascadingValue(
                builder,
                0,
                1,
                this,
                2,
                (builder2) =>
                {
                    if (ChildContent is not null)
                    {
                        ChildContent(builder2);
                    }
                });

            this.CreateCascadingValue(
                builder,
                3,
                4,
                this,
                5,
                (builder2) =>
                {
                    PopSubcomponent();
                });
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Signals that a subcomponent has completed its parameter initialization.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public void PopSubcomponent()
        {
            if (_isPop)
            {
                _isPop = false;
                Tracker?.PopSubcomponent();
            }
        }

        /// <summary>
        /// Signals that a subcomponent is beginning its parameter initialization.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public void PushSubcomponent()
        {
            if (_isPush)
            {
                _isPush = false;
                Tracker?.PushSubcomponent();
            }
        }
        #endregion
    }

    /// <summary>
    /// Abstract base class for chart subcomponents in Syncfusion Blazor Charts.
    /// </summary>
    public abstract class ChartSubComponent : SfBaseComponent, ISubcomponentTracker
    {
        #region Fields

        private bool _isPush = true;
        private bool _isPop = true;
        internal bool _isPropertyChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the cascading parameter that tracks subcomponent initialization state.
        /// </summary>
        [CascadingParameter]
        internal ISubcomponentTracker? Tracker { get; set; }

        /// <summary>
        /// Gets or sets the content to be rendered inside this subcomponent.
        /// </summary>
        /// <value>
        /// A <see cref="RenderFragment"/> representing the child content to be rendered inside the subcomponent.
        /// </value>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; } = null!;
        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Invoked when the component's parameters are set.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            PushSubcomponent();
        }

        /// <summary>
        /// Performs cleanup operations when the component is being disposed.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override ValueTask DisposeAsyncCore()
        {
            return base.DisposeAsyncCore();
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Builds the render tree for the component.
        /// </summary>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/> used to construct the component's render tree.</param>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (builder is null)
            {
                return;
            }

            this.CreateCascadingValue(
                builder,
                0,
                1,
                this,
                2,
                (builder2) =>
                {
                    if (ChildContent is not null)
                    {
                        ChildContent(builder2);
                    }
                });

            this.CreateCascadingValue(
                builder,
                3,
                4,
                this,
                5,
                (builder2) =>
                {
                    PopSubcomponent();
                });
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Signals that a subcomponent has completed its parameter initialization.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public void PopSubcomponent()
        {
            if (_isPop)
            {
                _isPop = false;
                Tracker?.PopSubcomponent();
            }
        }

        /// <summary>
        /// Signals that a subcomponent is beginning its parameter initialization.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public void PushSubcomponent()
        {
            if (_isPush)
            {
                _isPush = false;
                Tracker?.PushSubcomponent();
            }
        }
        #endregion
    }
}
