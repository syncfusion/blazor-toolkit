using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Represents the zoom toolkit component for chart interactions.
    /// Provides zoom, pan, and reset functionality through a visual toolbar.
    /// </summary>
    /// <remarks>
    /// This component renders zoom control buttons (Zoom In, Zoom Out, Pan, Zoom, Reset) 
    /// based on the chart's zoom settings and current state. The toolbar is dynamically 
    /// positioned and themed according to chart configuration.
    /// </remarks>
    public class ZoomToolkit : ComponentBase
    {
        #region Constants
        private const double SPACING = 10;
        private const double TOOLKIT_SHADOW_PADDING = 2;
        #endregion

        #region Fields

        private bool _shouldRender;
        private string? _selectionColor;
        private string? _fillColor;
        private string? _elementOpacity;
        private string? _elementId;
        private Rect? _iconRect;
        private string? _hoveredID;
        private string _iconRectOverFill = Constants.Transparent;
        private string _iconRectSelectionFill = Constants.Transparent;
        private CultureInfo _culture = CultureInfo.InvariantCulture;
        private string? _zoomingKitCollection;
        private double _zoomkitOpacity;

        internal bool _visible;
        internal bool _isReset;
        internal static bool _isIconSelected;
        internal static string? _selectedIconId;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the cascading chart instance.
        /// </summary>
        [CascadingParameter]
        private SfChart? Chart { get; set; }
        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Initializes the component and configures theme-specific styling.
        /// </summary>
        protected override void OnInitialized()
        {
            _elementId = Chart?.ID;
            _selectionColor = Chart?.Theme != Theme.FluentDark ? "#424242" : "#D6D6D6";
            _fillColor = Chart?.Theme != Theme.FluentDark ? "#424242" : "#D6D6D6";
            _iconRectOverFill = Chart?.Theme != Theme.FluentDark ? "#EBEBEB" : "#383838";
            _iconRectSelectionFill = Chart?.Theme != Theme.FluentDark ? "#EBEBEB" : "#383838";
            _iconRect = Chart?.Theme != Theme.FluentDark ? new Rect(-7, -8, 32, 32) : new Rect(0, 0, 16, 16);
            _zoomkitOpacity = 1;
        }

        /// <summary>
        /// Executes after the component has rendered.
        /// Handles keyboard focus for accessibility when required.
        /// </summary>
        /// <param name="firstRender">Indicates whether this is the first render of the component.</param>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender && !string.IsNullOrEmpty(Chart?._zoomingKeyboardFocusTarget))
            {
                _ = await SfBaseComponent.InvokeAsync<bool>(Chart._chartJsModule!, Chart._chartJsInProcessModule!, Constants.FocusTarget, [.. new object[] { Chart._zoomingKeyboardFocusTarget }]).ConfigureAwait(true);
                Chart._zoomingKeyboardFocusTarget = string.Empty;
            }
        }

        /// <summary>
        /// Determines whether the component should render based on state changes.
        /// </summary>
        /// <returns><see langword="true"/> if the component should render; otherwise <see langword="false"/>.</returns>
        protected override bool ShouldRender()
        {
            return _shouldRender;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Determines whether the current device is mobile/touch-enabled.
        /// </summary>
        /// <returns><see langword="true"/> if device is mobile; otherwise <see langword="false"/>.</returns>
        private bool IsDevice()
        {
            return Chart?._zoomingModule is not null && Chart._zoomingModule.IsDevice();
        }

        /// <summary>
        /// Displays the zooming toolbar with all configured buttons.
        /// </summary>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/> used to render the component.</param>
        private void ShowZoomingToolkit(RenderTreeBuilder builder)
        {
            List<ToolbarItems> toolboxItems = Chart?._zoomSettings.ToolbarItems ?? null!;
            Rect areaBounds = Chart?._axisContainer?.AxisLayout.SeriesClipRect ?? null!;
            Size size = MeasureResetText();
            int length = IsDevice() ? 1 : toolboxItems.Count;
            double iconSize = IsDevice() ? size.Width : 16;

            (double transX, double transY, double width, double height) = CalculateToolkitPosition(areaBounds, size, iconSize, length);

            SvgRendering renderer = Chart?._svgRenderer ?? null!;
            _zoomingKitCollection = _elementId + "_Zooming_KitCollection";

            if (length == 0 || (renderer.GroupCollection is not null && !renderer.GroupCollection.Find(item => item.Id == _zoomingKitCollection).Equals(new ElementReference())))
            {
                return;
            }

            toolboxItems = IsDevice() ? [ToolbarItems.Reset] : toolboxItems;
            RectOptions rectOptions = new(_elementId + "_Zooming_Rect", 0, 0, width, height + (SPACING * 2), 1, Constants.Transparent, Chart?.Theme != Theme.FluentDark ? "#fafafa" : "#1C1B1F", 4, 4, 1);
            RenderZoomKit(builder, transX, transY, rectOptions, length, toolboxItems, iconSize);
        }

        /// <summary>
        /// Calculates the position and dimensions of the zoom toolkit.
        /// </summary>
        /// <param name="areaBounds">The bounds of the chart area.</param>
        /// <param name="size">The size of the text or icons.</param>
        /// <param name="iconSize">The size of the icons.</param>
        /// <param name="length">The number of toolbar items.</param>
        /// <returns>A tuple containing transX, transY, width, and height.</returns>
        private (double, double, double, double) CalculateToolkitPosition(Rect areaBounds, Size size, double iconSize, int length)
        {
            double height = IsDevice() ? size.Height : 18;
            double width = (length * iconSize) + ((length + 1) * SPACING) + ((length - 1) * SPACING);

            double transX = CalculateAlignment(Chart?._zoomSettings.ToolbarPosition is not null ? Chart._zoomSettings.ToolbarPosition.HorizontalAlign : HorizontalAlign.Right, areaBounds.X, areaBounds.Width, width);
            transX += Chart?._zoomSettings.ToolbarPosition is not null && !double.IsNaN(Chart._zoomSettings.ToolbarPosition.X) ? Chart._zoomSettings.ToolbarPosition.X : 0;

            double transY = CalculateAlignment(Chart?._zoomSettings.ToolbarPosition is not null ? Chart._zoomSettings.ToolbarPosition.VerticalAlign : VerticalAlign.Top, areaBounds.Y, areaBounds.Height, height + (SPACING * 2));
            transY += Chart?._zoomSettings.ToolbarPosition is not null && !double.IsNaN(Chart._zoomSettings.ToolbarPosition.Y) ? Chart._zoomSettings.ToolbarPosition.Y : 0;

            transX = Math.Max((Chart?._chartBorderRenderer?.ChartBorder?.Width ?? 0) + TOOLKIT_SHADOW_PADDING,
                Math.Min(transX, (Chart?.AvailableSize.Width ?? 0) - width - (Chart?._chartBorderRenderer?.ChartBorder?.Width ?? 0) - TOOLKIT_SHADOW_PADDING)
            );
            transY = Math.Max((Chart?._chartBorderRenderer?.ChartBorder?.Width ?? 0) + TOOLKIT_SHADOW_PADDING,
                Math.Min(transY, (Chart?.AvailableSize.Height ?? 0) - height - (SPACING * 2) - (Chart?._chartBorderRenderer?.ChartBorder?.Width ?? 0) - TOOLKIT_SHADOW_PADDING)
            );

            return (transX, transY, width, height);
        }

        /// <summary>
        /// Measures the size of the "Reset Zoom" text for device rendering.
        /// </summary>
        /// <returns>A <see cref="Size"/> containing the measured text dimensions.</returns>
        private Size MeasureResetText()
        {
            ChartFontOptions fontOptions = new()
            {
                Size = "12px",
                FontFamily = Chart?._chartThemeStyle?.AxisLabelFontFamily ?? string.Empty,
                FontWeight = "400"
            };

            return ChartHelper.MeasureText("Reset Zoom", fontOptions);
        }

        /// <summary>
        /// Calculates the alignment offset for a component within a bounds area.
        /// </summary>
        /// <param name="alignment">The alignment value (HorizontalAlign or VerticalAlign).</param>
        /// <param name="areaBoundsStart">The start position of the bounds area.</param>
        /// <param name="areaBoundsSize">The size of the bounds area.</param>
        /// <param name="elementSize">The size of the element to align.</param>
        /// <returns>The calculated alignment offset.</returns>
        private static double CalculateAlignment(object alignment, double areaBoundsStart, double areaBoundsSize, double elementSize)
        {
            return alignment switch
            {
                HorizontalAlign.Left or VerticalAlign.Top => areaBoundsStart + SPACING,
                HorizontalAlign.Center or VerticalAlign.Middle => (areaBoundsSize / 2) - (elementSize / 2) + areaBoundsStart,
                HorizontalAlign.Right or VerticalAlign.Bottom => areaBoundsStart + areaBoundsSize - elementSize - SPACING,
                _ => areaBoundsStart + SPACING
            };
        }

        /// <summary>
        /// Renders the complete zoom kit toolbar.
        /// </summary>
        private void RenderZoomKit(RenderTreeBuilder builder, double transX, double transY, RectOptions rectOptions, int length, List<ToolbarItems> toolboxItems, double iconSize)
        {
            SvgRendering renderer = Chart?._svgRenderer ?? null!;

            builder.OpenElement(renderer.Seq++, "g");
            AddGroupAttributes(builder, transX, transY);
            builder.AddElementReferenceCapture(renderer.Seq++, ins => { renderer.GroupCollection?.Add(ins); });

            RenderDefinitions(builder);
            RenderBackgroundRectangles(builder, renderer, rectOptions);
            RenderToolbarItems(builder, toolboxItems, length, iconSize);

            builder.CloseElement();
        }

        /// <summary>
        /// Adds attributes and event handlers to the root group element.
        /// </summary>
        /// <param name="builder">Render tree builder.</param>
        /// <param name="transX">X translation.</param>
        /// <param name="transY">Y translation.</param>
        private void AddGroupAttributes(RenderTreeBuilder builder, double transX, double transY)
        {
            SvgRendering renderer = Chart?._svgRenderer ?? null!;
            builder.AddAttribute(renderer.Seq++, "id", _zoomingKitCollection);
            builder.AddAttribute(renderer.Seq++, "transform", $"translate({transX.ToString(_culture)},{transY.ToString(_culture)})");
            builder.AddAttribute(renderer.Seq++, "opacity", IsDevice() ? 1 : _zoomkitOpacity);
            builder.AddAttribute(renderer.Seq++, "cursor", "auto");

            if (!IsDevice())
            {
                AddEventHandlers(builder);
            }
        }

        /// <summary>
        /// Adds mouse and touch event handlers for non-device scenarios.
        /// </summary>
        /// <param name="builder">Render tree builder.</param>
        private void AddEventHandlers(RenderTreeBuilder builder)
        {
            SvgRendering renderer = Chart?._svgRenderer ?? null!;
            builder.AddAttribute(renderer.Seq++, "onmousemove", EventCallback.Factory.Create<MouseEventArgs>(this, ZoomToolkitMove));
            builder.AddAttribute(renderer.Seq++, "ontouchstart", EventCallback.Factory.Create<TouchEventArgs>(this, ZoomToolkitMove));
            builder.AddAttribute(renderer.Seq++, "onpointermove", EventCallback.Factory.Create<PointerEventArgs>(this, ZoomToolkitMove));
            builder.AddAttribute(renderer.Seq++, "onmouseout", EventCallback.Factory.Create<MouseEventArgs>(this, ZoomToolkitLeave));
            builder.AddAttribute(renderer.Seq++, "ontouchend", EventCallback.Factory.Create<TouchEventArgs>(this, ZoomToolkitLeave));
            builder.AddAttribute(renderer.Seq++, "onpointerout", EventCallback.Factory.Create<PointerEventArgs>(this, ZoomToolkitLeave));
        }

        /// <summary>
        /// Renders SVG definitions used for toolkit visuals (filters).
        /// </summary>
        /// <param name="builder">Render tree builder.</param>
        private void RenderDefinitions(RenderTreeBuilder builder)
        {
            SvgRendering renderer = Chart?._svgRenderer ?? null!;
            builder.OpenElement(renderer.Seq++, "defs");
            builder.AddMarkupContent(renderer.Seq++, GetShadowFilter());
            builder.CloseElement();
        }

        /// <summary>
        /// Returns an SVG filter definition used to render a subtle shadow.
        /// </summary>
        /// <returns>SVG filter markup string.</returns>
        private static string GetShadowFilter()
        {
            return "<filter id='chart_shadow' height='130%'><feGaussianBlur in='SourceAlpha' stdDeviation='5'/>" +
                   "<feOffset dx='-3' dy='4' result='offsetblur'/><feComponentTransfer><feFuncA type='linear' slope='1'/>" +
                   "</feComponentTransfer><feMerge><feMergeNode/><feMergeNode in='SourceGraphic'/></feMerge></filter>";
        }

        /// <summary>
        /// Renders two layered background rectangles: base and shadow.
        /// </summary>
        /// <param name="builder">Render tree builder.</param>
        /// <param name="renderer">SVG rendering helper.</param>
        /// <param name="rectOptions">Rectangle options.</param>
        private static void RenderBackgroundRectangles(RenderTreeBuilder builder, SvgRendering renderer, RectOptions rectOptions)
        {
            renderer.RenderRect(builder, rectOptions);
            rectOptions.Filter = "drop-shadow(0px 1px 3px rgba(0, 0, 0, 0.15)) drop-shadow(0px 1px 2px rgba(0, 0, 0, 0.3))";
            renderer.RenderRect(builder, rectOptions);
        }

        /// <summary>
        /// Renders toolbar items in sequence.
        /// </summary>
        /// <param name="builder">Render tree builder.</param>
        /// <param name="toolboxItems">Items to render.</param>
        /// <param name="length">Count of items.</param>
        /// <param name="iconSize">Icon size for spacing.</param>
        private void RenderToolbarItems(RenderTreeBuilder builder, List<ToolbarItems> toolboxItems, int length, double iconSize)
        {
            SvgRendering renderer = Chart?._svgRenderer ?? null!;
            double xPosition = SPACING;

            for (int i = 1; i <= length; i++)
            {
                builder.OpenElement(renderer.Seq++, "g");
                double yPosition = IsDevice() ? SPACING : SPACING + 1;
                builder.AddAttribute(renderer.Seq++, "transform", $"translate({xPosition.ToString(_culture)},{yPosition.ToString(_culture)})");

                RenderToolbarButton(builder, toolboxItems[i - 1]);
                xPosition += iconSize + (SPACING * 2);
                builder.CloseElement();
            }
        }

        /// <summary>
        /// Renders a single toolbar button based on the <see cref="ToolbarItems"/> value.
        /// </summary>
        /// <param name="builder">Render tree builder.</param>
        /// <param name="item">Toolbar item to render.</param>
        private void RenderToolbarButton(RenderTreeBuilder builder, ToolbarItems item)
        {
            switch (item)
            {
                case ToolbarItems.Pan: CreatePanButton(builder); break;
                case ToolbarItems.Zoom: CreateZoomButton(builder, Chart ?? null!); break;
                case ToolbarItems.ZoomIn: CreateZoomInButton(builder, Chart ?? null!); break;
                case ToolbarItems.ZoomOut: CreateZoomOutButton(builder, Chart ?? null!); break;
                case ToolbarItems.Reset: CreateResetButton(builder); break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Handles the mouse/touch leave event for the zoom toolkit to hide tooltip.
        /// </summary>
        private void ZoomToolkitLeave()
        {
            SetAttribute(_zoomingKitCollection ?? null!, "opacity", _zoomkitOpacity.ToString(_culture));
        }

        /// <summary>
        /// Handles the mouse/touch move event for the zoom toolkit to show tooltip and highlight.
        /// </summary>
        private void ZoomToolkitMove()
        {
            _zoomkitOpacity = 1;
            SetAttribute(_zoomingKitCollection ?? null!, "opacity", _zoomkitOpacity.ToString(_culture));
        }

        /// <summary>
        /// Renders the reset button as text (mobile/device view).
        /// </summary>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/> used to render.</param>
        /// <param name="resetId">The ID of the reset button element.</param>
        /// <param name="render">The SVG rendering helper.</param>
        private void RenderResetButtonAsText(SvgRendering render, RenderTreeBuilder builder, string resetId)
        {
            Size size = ChartHelper.MeasureText(Chart?.GetLocalizedLabel("Chart_ResetZoom") ?? string.Empty, new ChartFontOptions { Size = "12px", FontFamily = Chart?._chartThemeStyle?.AxisLabelFontFamily ?? string.Empty, FontWeight = "400" });
            render.RenderRect(builder, new RectOptions(resetId + "_1", 0, 0, size.Width, size.Height, 0, Constants.Transparent, Constants.Transparent, 0, 0, 1));
            render.RenderText(builder, new TextOptions()
            {
                Id = resetId + "_2",
                X = (size.Width / 2).ToString(_culture),
                Y = (size.Height / 2).ToString(_culture),
                TextAnchor = "middle",
                Text = Chart?.GetLocalizedLabel("Chart_ResetZoom") ?? string.Empty,
                Transform = "rotate(0," + 0 + ',' + 0 + ')',
                DominantBaseline = "middle",
                FontSize = "12px",
                Fill = Chart?.Theme != Theme.FluentDark ? "black" : "white"
            });
        }

        /// <summary>
        /// Renders the reset button as text (desktop/icon view).
        /// </summary>
        /// <param name="render">SVG rendering helper.</param>
        /// <param name="builder">Render tree builder.</param>
        /// <param name="resetId">The ID of the reset element.</param>
        private void RenderResetButtonAsIcon(SvgRendering render, RenderTreeBuilder builder, string resetId)
        {
            string direction = "M12.364,8h-2.182l2.909,3.25L16,8h-2.182c0-3.575-2.618-6.5-5.818-6.5c-1.128,0-2.218,0.366-3.091,1.016l1.055,1.178C6.581,3.328,7.272,3.125,8,3.125C10.4,3.125,12.363,5.319,12.364,8L12.364,8z M11.091,13.484l-1.055-1.178C9.419,12.672,8.728,12.875,8,12.875c-2.4,0-4.364-2.194-4.364-4.875h2.182L2.909,4.75L0,8h2.182c0,3.575,2.618,6.5,5.818,6.5C9.128,14.5,10.219,14.134,11.091,13.484L11.091,13.484z";

            render.RenderRect(builder, new RectOptions(resetId + "_1", _iconRect?.X ?? 0, _iconRect?.Y ?? 0, _iconRect?.Width ?? 0, _iconRect?.Height ?? 0, 0, Constants.Transparent, Constants.Transparent, 4, 4, 1));
            _ = render.RenderPath(builder, new PathOptions(resetId + "_2", direction, null!, 0, Constants.Transparent, 1, _fillColor ?? string.Empty));
        }

        /// <summary>
        /// Removes an element from the DOM via JavaScript interop.
        /// </summary>
        /// <param name="id">The ID of the element to remove.</param>
        private async Task RemoveElementAsync(string id)
        {
            if (Chart is not null)
            {
                await SfBaseComponent.InvokeVoidAsync(Chart._chartJsModule, Chart._chartJsInProcessModule, "removeElement", [.. new object[] { id }]).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Sets an SVG element attribute via the chart's method.
        /// </summary>
        /// <param name="id">The ID of the element.</param>
        /// <param name="key">The name of the attribute to set.</param>
        /// <param name="fill">The value to set.</param>
        private void SetAttribute(string id, string key, string fill)
        {
            _ = Chart?.SetAttributeAsync(id, key, fill, string.Empty);
        }

        /// <summary>
        /// Resets the zooming module state after a zoom operation.
        /// </summary>
        /// <param name="chart">The chart instance to reset.</param>
        private static void ResetZoomingModule(SfChart chart)
        {
            if (chart._zoomingModule is not null)
            {
                chart._zoomingModule.IsZoomed = chart._zoomingModule.IsPanning = chart._isChartDrag = chart._delayRedraw = chart._isLiveChart = false;
                chart._zoomingModule.TouchMoveList = chart._zoomingModule.TouchStartList = [];
                chart._zoomingModule.PinchTarget = null;
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Rebuilds the render tree for the component.
        /// </summary>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/> used to render the component.</param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            _shouldRender = false;
            if (_visible)
            {
                ShowZoomingToolkit(builder);
            }
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Invalidates the renderer and triggers a component re-render.
        /// </summary>
        internal void InvalidateRenderer()
        {
            _ = InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Shows the zooming toolkit if conditions are met.
        /// </summary>
        internal void ShowZoomingKit()
        {
            if (Chart is not null && Chart.EnableAdaptiveRendering && (Chart._widthCategory == ChartWidthCategory.Small || Chart._heightCategory == ChartHeightCategory.Small))
            {
                if (_visible)
                {
                    HideZoomingKit();
                }
                return;
            }
            _visible = _shouldRender = true;
            InvalidateRenderer();
        }

        /// <summary>
        /// Hides the zooming toolkit.
        /// </summary>
        internal void HideZoomingKit()
        {
            _visible = false;
            _shouldRender = true;
            InvalidateRenderer();
        }

        /// <summary>
        /// Creates the Pan button SVG markup.
        /// </summary>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/> used to render.</param>
        internal void CreatePanButton(RenderTreeBuilder builder)
        {
            if (Chart?._svgRenderer is null)
            {
                return;
            }

            bool isPanning = Chart._zoomingModule is not null && Chart._zoomingModule.IsPanning;
            string? fillColor = isPanning ? _selectionColor : _fillColor;
            const string panPath = "M5,3h2.3L7.275,5.875h1.4L8.65,3H11L8,0L5,3z M3,11V8.7l2.875,0.025v-1.4L3,7.35V5L0,8L3,11z "
                + "M11,13H8.7l0.025-2.875h-1.4L7.35,13H5l3,3L11,13z M13,5v2.3l-2.875-0.025v1.4L13,8.65V11l3-3L13,5z";

            SvgRendering renderer = Chart._svgRenderer ?? null!;

            builder.AddAttribute(renderer.Seq++, "id", _elementId + "_Zooming_Pan");
            builder.AddAttribute(renderer.Seq++, "role", "button");

            string iconText = Chart.GetLocalizedLabel("Chart_Pan") ?? string.Empty;
            string? format = Chart._zoomSettings?.AccessibilityDescriptionFormat;
            string ariaLabel = !string.IsNullOrEmpty(format) ? (format.Contains("${value}", StringComparison.Ordinal)
                    ? format.Replace("${value}", "Pan", StringComparison.Ordinal)
                    : format) : iconText;

            builder.AddAttribute(renderer.Seq++, "aria-label", ariaLabel);
            builder.AddAttribute(renderer.Seq++, "data-text", iconText);
            _elementOpacity = ShouldDisablePanButton() ? "0.2" : "1";
            builder.AddAttribute(renderer.Seq++, "opacity", _elementOpacity);

            Chart?._svgRenderer?.RenderRect(builder, new RectOptions(
                _elementId + "_Zooming_Pan_1",
                _iconRect?.X ?? 0, _iconRect?.Y ?? 0,
                _iconRect?.Width ?? 0, _iconRect?.Height ?? 0,
                0, Constants.Transparent, Constants.Transparent, 4, 4, 1
            ));

            _ = Chart?._svgRenderer?.RenderPath(builder, new PathOptions(
                _elementId + "_Zooming_Pan_2", panPath, null!,
                0, Constants.Transparent, 1, fillColor!
            ));
        }

        /// <summary>
        /// Determines whether the Pan button should be disabled.
        /// </summary>
        internal bool ShouldDisablePanButton()
        {
            return Chart?._zoomSettings?.ToolbarDisplayMode == ToolbarMode.Always
                   && Chart._zoomingModule is not null
                   && !Chart._zoomingModule.IsZoomed;
        }

        /// <summary>
        /// Creates the Zoom button SVG markup.
        /// </summary>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/> used to render.</param>
        /// <param name="chart">The chart instance.</param>
        internal void CreateZoomButton(RenderTreeBuilder builder, SfChart chart)
        {
            SvgRendering render = chart._svgRenderer ?? null!;
            string _fillColor = (chart._zoomingModule is not null && chart._zoomingModule.IsPanning) || (chart._zoomSettings.ToolbarDisplayMode == ToolbarMode.Always && chart._zoomingModule is not null && !chart._zoomingModule.IsZoomed) ? this._fillColor ?? null! : _selectionColor ?? null!;
            string rectColor = chart._zoomingModule is not null && chart._zoomingModule.IsPanning ? Constants.Transparent : chart._zoomSettings.ToolbarDisplayMode == ToolbarMode.Always && chart._zoomingModule is not null && !chart._zoomingModule.IsZoomed ? "transparent" : _iconRectSelectionFill; ;
            string direction = "M0.001,14.629L1.372,16l4.571-4.571v-0.685l0.228-0.274c1.051,0.868,2.423,1.417,3.885,1.417c3.291,0,5.943-2.651,5.943-5.943S13.395,0,10.103,0S4.16,2.651,4.16,5.943c0,1.508,0.503,2.834,1.417,3.885l-0.274,0.228H4.571L0.001,14.629L0.001,14.629z M5.943,5.943c0-2.285,1.828-4.114,4.114-4.114s4.114,1.828,4.114,";

            builder.AddAttribute(render.Seq++, "id", _elementId + "_Zooming_Zoom");
            string iconText = chart.GetLocalizedLabel("Chart_Zoom");
            builder.AddAttribute(render.Seq++, "aria-label", !string.IsNullOrEmpty(chart._zoomSettings.AccessibilityDescriptionFormat) ? (chart._zoomSettings.AccessibilityDescriptionFormat.Contains("${value}", StringComparison.Ordinal) ? chart._zoomSettings.AccessibilityDescriptionFormat.Replace("${value}", "Zoom", StringComparison.Ordinal) : chart._zoomSettings.AccessibilityDescriptionFormat) : iconText);
            builder.AddAttribute(render.Seq++, "role", "button");
            builder.AddAttribute(render.Seq++, "data-text", iconText);
            _elementOpacity = chart._zoomSettings.ToolbarDisplayMode == ToolbarMode.Always && chart._zoomingModule is not null && !chart._zoomingModule.IsZoomed ? "0.2" : "1";
            builder.AddAttribute(render.Seq++, "opacity", _elementOpacity);

            render.RenderRect(builder, new RectOptions(_elementId + "_Zooming_Zoom_1", _iconRect?.X ?? 0, _iconRect?.Y ?? 0, _iconRect?.Width ?? 0, _iconRect?.Height ?? 0, 0, Constants.Transparent, rectColor, 4, 4, 1));
            _ = render.RenderPath(builder, new PathOptions(_elementId + "_Zooming_Zoom_3", direction + "4.114s-1.828,4.114-4.114,4.114S5.943,8.229,5.943,5.943z", null!, 0, Constants.Transparent, 1, _fillColor));
        }

        /// <summary>
        /// Creates the Zoom In button SVG markup.
        /// </summary>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/> used to render.</param>
        /// <param name="chart">The chart instance.</param>
        internal void CreateZoomInButton(RenderTreeBuilder builder, SfChart chart)
        {
            SvgRendering render = chart._svgRenderer ?? null!;
            string direction = "M10.103,0C6.812,0,4.16,2.651,4.16,5.943c0,1.509,0.503,2.834,1.417,3.885l-0.274,0.229H4.571L0,14.628l0,0L1.372,16l4.571-4.572v-0.685l0.228-0.275c1.052,0.868,2.423,1.417,3.885,1.417c3.291,0,5.943-2.651,5.943-5.943C16,2.651,13.395,0,10.103,0z M10.058,10.058c-2.286,0-4.114-1.828-4.114-4.114c0-2.286,1.828-4.114,4.114-4.114c2.286,0,4.114,1.828,4.114,4.114C14.172,8.229,12.344,10.058,10.058,10.058z";
            string zoomingZoomIn = _elementId + "_Zooming_ZoomIn";
            string polygonDirection = "12.749,5.466 10.749,5.466 10.749,3.466 9.749,3.466 9.749,5.466 7.749,5.466 7.749,6.466 9.749,6.466 9.749,8.466 10.749,8.466 10.749,6.466 12.749,6.466";

            builder.AddAttribute(render.Seq++, "id", zoomingZoomIn);
            string iconText = chart.GetLocalizedLabel("Chart_ZoomIn");
            builder.AddAttribute(render.Seq++, "aria-label", !string.IsNullOrEmpty(chart._zoomSettings.AccessibilityDescriptionFormat) ? (chart._zoomSettings.AccessibilityDescriptionFormat.Contains("${value}", StringComparison.Ordinal) ? chart._zoomSettings.AccessibilityDescriptionFormat.Replace("${value}", "ZoomIn", StringComparison.Ordinal) : chart._zoomSettings.AccessibilityDescriptionFormat) : iconText);
            builder.AddAttribute(render.Seq++, "tabindex", chart.Focusable && chart._zoomSettings.Focusable ? "0" : "-1");
            builder.AddAttribute(render.Seq++, "role", !string.IsNullOrEmpty(chart._zoomSettings.AccessibilityRole) ? chart._zoomSettings.AccessibilityRole : "button");
            builder.AddAttribute(render.Seq++, "data-text", iconText);
            _elementOpacity = chart._zoomingModule is not null && chart._zoomingModule.IsPanning ? "0.2" : "1";
            builder.AddAttribute(render.Seq++, "opacity", _elementOpacity);

            render.RenderRect(builder, new RectOptions(zoomingZoomIn + "_1", _iconRect?.X ?? 0, _iconRect?.Y ?? 0, _iconRect?.Width ?? 0, _iconRect?.Height ?? 0, 0, Constants.Transparent, _selectedIconId == zoomingZoomIn && _isIconSelected ? _iconRectOverFill : Constants.Transparent, 4, 4, 1));
            _ = render.RenderPath(builder, new PathOptions(zoomingZoomIn + "_2", direction, null!, 0, Constants.Transparent, 1, _selectedIconId == zoomingZoomIn && _isIconSelected ? _selectionColor ?? string.Empty : _fillColor ?? string.Empty));
            render.RenderPolygon(builder, render.Seq++, zoomingZoomIn + "_3", _selectedIconId == zoomingZoomIn && _isIconSelected ? _selectionColor ?? string.Empty : _fillColor ?? string.Empty, polygonDirection);
        }

        /// <summary>
        /// Creates the Zoom Out button SVG markup.
        /// </summary>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/> used to render.</param>
        /// <param name="chart">The chart instance.</param>
        internal void CreateZoomOutButton(RenderTreeBuilder builder, SfChart chart)
        {
            SvgRendering render = chart._svgRenderer ?? null!;
            string _fillColor = this._fillColor ?? null!;
            string direction = "M0,14.622L1.378,16l4.533-4.533v-0.711l0.266-0.266c1.022,0.889,2.4,1.422,3.866,1.422c3.289,0,5.955-2.666,5.955-5.955S13.333,0,10.044,0S4.089,2.667,4.134,5.911c0,1.466,0.533,2.844,1.422,3.866l-0.266,0.266H4.578L0,14.622L0,14.622z M5.911,5.911c0-2.311,1.822-4.133,4.133-4.133s4.133,1.822,4.133,4.133s-1.866,4.133-4.133,4.133S5.911,8.222,5.911,5.911z M12.567,6.466h-5v-1h5V6.466z";
            string zooming_ZoomOut = _elementId + "_Zooming_ZoomOut";

            builder.AddAttribute(render.Seq++, "id", zooming_ZoomOut);
            string iconText = chart.GetLocalizedLabel("Chart_ZoomOut");
            builder.AddAttribute(render.Seq++, "aria-label", !string.IsNullOrEmpty(chart._zoomSettings.AccessibilityDescriptionFormat) ? (chart._zoomSettings.AccessibilityDescriptionFormat.Contains("${value}", StringComparison.Ordinal) ? chart._zoomSettings.AccessibilityDescriptionFormat.Replace("${value}", "ZoomOut", StringComparison.Ordinal) : chart._zoomSettings.AccessibilityDescriptionFormat) : iconText);
            builder.AddAttribute(render.Seq++, "tabindex", chart.Focusable && chart._zoomSettings.Focusable ? "0" : "-1");
            builder.AddAttribute(render.Seq++, "role", !string.IsNullOrEmpty(chart._zoomSettings.AccessibilityRole) ? chart._zoomSettings.AccessibilityRole : "button");
            builder.AddAttribute(render.Seq++, "data-text", iconText);
            _elementOpacity = (chart._zoomingModule is not null && chart._zoomingModule.IsPanning) || (chart._zoomSettings.ToolbarDisplayMode == ToolbarMode.Always && chart._zoomingModule is not null && !chart._zoomingModule.IsZoomed) ? "0.2" : "1";
            builder.AddAttribute(render.Seq++, "opacity", _elementOpacity);

            render.RenderRect(builder, new RectOptions(zooming_ZoomOut + "_1", _iconRect?.X ?? 0, _iconRect?.Y ?? 0, _iconRect?.Width ?? 0, _iconRect?.Height ?? 0, 0, Constants.Transparent, _selectedIconId == zooming_ZoomOut && _isIconSelected ? _iconRectOverFill : Constants.Transparent, 4, 4, 1));
            _ = render.RenderPath(builder, new PathOptions(zooming_ZoomOut + "_2", direction, null!, 0, Constants.Transparent, 1, _selectedIconId == zooming_ZoomOut && _isIconSelected ? _selectionColor ?? string.Empty : _fillColor));
        }

        /// <summary>
        /// Creates the Reset button SVG markup (icon or text based on device type).
        /// </summary>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/> used to render.</param>
        internal void CreateResetButton(RenderTreeBuilder builder)
        {
            SvgRendering render = Chart?._svgRenderer ?? null!;
            string zooming_Reset = _elementId + "_Zooming_Reset";

            builder.AddAttribute(render.Seq++, "id", zooming_Reset);
            string iconText = Chart?.GetLocalizedLabel("Chart_Reset") ?? null!;
            builder.AddAttribute(render.Seq++, "aria-label", !string.IsNullOrEmpty(Chart?._zoomSettings.AccessibilityDescriptionFormat) ? (Chart._zoomSettings.AccessibilityDescriptionFormat.Contains("${value}", StringComparison.Ordinal) ? Chart._zoomSettings.AccessibilityDescriptionFormat.Replace("${value}", "Reset", StringComparison.Ordinal) : Chart._zoomSettings.AccessibilityDescriptionFormat) : iconText);
            builder.AddAttribute(render.Seq++, "tabindex", Chart is not null && Chart.Focusable && Chart._zoomSettings.Focusable ? "0" : "-1");
            builder.AddAttribute(render.Seq++, "role", !string.IsNullOrEmpty(Chart?._zoomSettings.AccessibilityRole) ? Chart._zoomSettings.AccessibilityRole : "button");
            builder.AddAttribute(render.Seq++, "data-text", iconText);
            _elementOpacity = Chart?._zoomSettings.ToolbarDisplayMode == ToolbarMode.Always && Chart._zoomingModule is not null && !Chart._zoomingModule.IsZoomed ? "0.2" : "1";
            builder.AddAttribute(render.Seq++, "opacity", _elementOpacity);

            if (!IsDevice())
            {
                RenderResetButtonAsIcon(render, builder, zooming_Reset);
            }
            else
            {
                RenderResetButtonAsText(render, builder, zooming_Reset);
            }
        }

        /// <summary>
        /// Removes the zoom tooltip from the DOM.
        /// </summary>
        internal void RemoveTooltip()
        {
            if (!string.IsNullOrEmpty(_hoveredID))
            {
                bool isPanning = Chart?._zoomingModule is not null && Chart._zoomingModule.IsPanning;
                bool isZoom = _hoveredID.Contains("_Zoom_", StringComparison.InvariantCulture);

                string rectFill = isPanning ? _hoveredID.Contains("_Pan_", StringComparison.InvariantCulture) ? _iconRectSelectionFill : Constants.Transparent : isZoom ? _iconRectSelectionFill : Constants.Transparent;
                string pathFill = isPanning ? _hoveredID.Contains("_Pan_", StringComparison.InvariantCulture) ? _selectionColor ?? string.Empty : _fillColor ?? string.Empty : isZoom ? _selectionColor ?? string.Empty : _fillColor ?? string.Empty;

                SetAttribute(_hoveredID, "fill", rectFill);
                SetAttribute(_hoveredID.Replace("_1", "_2", StringComparison.InvariantCulture), "fill", pathFill);
                SetAttribute(_hoveredID.Replace("_1", "_3", StringComparison.InvariantCulture), "fill", isPanning ? _fillColor ?? string.Empty : isZoom ? _selectionColor ?? string.Empty : _fillColor ?? string.Empty);
            }

            _ = RemoveElementAsync("EJ2_Chart_ZoomTip");
        }

        /// <summary>
        /// Resets the zoom state and refreshes the chart layout.
        /// </summary>
        internal async Task SetDeferredZoomAsync(SfChart chart)
        {
            chart._disableTrackTooltip = false;
            await chart.GetBooleanValuesAsync().ConfigureAwait(true);
            ResetZoomingModule(chart);

            if (chart.EnableAutoIntervalOnBothAxis)
            {
                chart._seriesContainer?.ProcessData();
            }

            chart.OnLayoutChange();
            await Task.Delay(200).ConfigureAwait(true);
            await chart.UpdateDatalabelTemplateAsync().ConfigureAwait(true);
            _elementOpacity = "1";
        }
        #endregion
    }
}
