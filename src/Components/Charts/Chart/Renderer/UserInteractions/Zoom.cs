
namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Handles zooming and panning interactions for charts.
    /// </summary>
    /// <remarks>
    /// This class manages wheel, pinch, and touch zoom state, invokes zooming events, 
    /// and coordinates toolkit visibility based on user interactions.
    /// </remarks>
    public class Zoom
    {
        #region Constants
        private const int UPDATETHRESHOLD = 10;
        #endregion

        #region Fields
        private DateTime _previousRequestTime = DateTime.MinValue;
        private SfChart? _chart;
        private ChartZoomSettings? _zooming;
        internal ZoomingEventArgs? _wheelEndEventArgs;
        internal bool _isWheelStart = true;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether panning is currently active.
        /// </summary>
        /// <value><see langword="true"/> if panning is in progress; otherwise, <see langword="false"/>.</value>
        internal bool IsPanning { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the zooming operation has completed.
        /// </summary>
        /// <value><see langword="true"/> if zooming is complete; otherwise, <see langword="false"/>.</value>
        internal bool IsZoomingComplete { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether any axis is currently zoomed.
        /// </summary>
        /// <value><see langword="true"/> if at least one axis is zoomed; otherwise, <see langword="false"/>.</value>
        internal bool IsZoomed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether wheel zoom is active.
        /// </summary>
        /// <value><see langword="true"/> if wheel zoom is enabled; otherwise, <see langword="false"/>.</value>
        internal bool IsWheelZoom { get; set; }

        /// <summary>
        /// Gets or sets the browser information for device-specific behavior.
        /// </summary>
        /// <value>The browser instance associated with the chart.</value>
        internal Browser? Browser { get; set; }

        /// <summary>
        /// Gets or sets the target element identifier for pinch gestures.
        /// </summary>
        /// <value>The element ID targeted by pinch interactions.</value>
        internal string? PinchTarget { get; set; }

        /// <summary>
        /// Gets or sets the list of touch points at the start of a touch interaction.
        /// </summary>
        /// <value>A collection of touch coordinates when the gesture begins.</value>
        internal List<Touches>? TouchStartList { get; set; }

        /// <summary>
        /// Gets or sets the list of touch points during a touch move interaction.
        /// </summary>
        /// <value>A collection of touch coordinates as the gesture progresses.</value>
        internal List<Touches>? TouchMoveList { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the UI has been interacted with.
        /// </summary>
        /// <value><see langword="true"/> if UI interaction occurred; otherwise, <see langword="false"/>.</value>
        internal bool PerformedUI { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Zoom"/> class.
        /// </summary>
        /// <param name="sfChart">The chart instance to associate with this zoom controller.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="sfChart"/> is <see langword="null"/>.</exception>
        internal Zoom(SfChart sfChart)
        {
            _chart = sfChart;
            Browser = _chart._browser;
            ChartZoomSettings zooming = _chart._zoomSettings;
            _zooming = zooming;
            IsZoomed = PerformedUI = _zooming.EnablePan && _zooming.EnableSelectionZooming;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Collects zoom-related data from all chart axes.
        /// </summary>
        /// <returns>A list of <see cref="AxisData"/> objects representing the current state of each axis.</returns>
        private List<AxisData> CollectAxisData()
        {
            List<AxisData> zoomedAxisCollection = [];

            if (_chart?._axisContainer is not null)
            {
                foreach (ChartAxisRenderer axisRender in _chart._axisContainer.Renderers.Cast<ChartAxisRenderer>())
                {
                    ChartAxis axis = axisRender.Axis ?? null!;

                    zoomedAxisCollection.Add(new AxisData
                    {
                        ZoomFactor = axis.ZoomFactor,
                        ZoomPosition = axis.ZoomPosition,
                        AxisName = axis.GetName(),
                        AxisRange = ChartHelper.GetVisibleRangeModel(axisRender.VisibleRange, axisRender.VisibleInterval)
                    });
                }
            }
            return zoomedAxisCollection;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Determines whether the current environment is a mobile or tablet device.
        /// </summary>
        /// <returns><see langword="true"/> if running on a device; otherwise, <see langword="false"/>.</returns>
        internal bool IsDevice()
        {
            return _chart is not null && _chart.SyncfusionService is not null && _chart.SyncfusionService.IsDeviceMode;
        }

        /// <summary>
        /// Triggers a zooming event with the specified event handler and name.
        /// </summary>
        /// <param name="zoomingEvent">The event handler to invoke.</param>
        /// <param name="eventName">The name of the zooming event (e.g., "OnZoomStart", "OnZoomEnd").</param>
        /// <param name="preventEvent">If <see langword="true"/>, the event is not triggered. Default is <see langword="false"/>.</param>
        /// <returns>A <see cref="ZoomingEventArgs"/> instance containing event data, or <see langword="null"/> if prevented.</returns>
        /// <remarks>
        /// This method collects axis data (zoom factor, position, range) and invokes the provided callback 
        /// with a populated <see cref="ZoomingEventArgs"/> object.
        /// </remarks>
        internal ZoomingEventArgs TriggerZoomingEvent(Action<ZoomingEventArgs> zoomingEvent, string eventName, bool preventEvent = false)
        {
            if (preventEvent)
            {
                return null!;
            }

            List<AxisData> zoomedAxisCollection = CollectAxisData();

            ZoomingEventArgs zoomingEventArgs = new()
            {
                Cancel = false,
                AxisCollection = zoomedAxisCollection,
                Name = eventName
            };

            DataVizCommonHelper.InvokeEvent(zoomingEvent, zoomingEventArgs);


            return zoomingEventArgs;
        }

        /// <summary>
        /// Applies zoom toolkit visibility and state based on current zoom levels.
        /// </summary>
        /// <param name="chart">The chart instance to update.</param>
        /// <param name="axes">The collection of axis renderers to evaluate.</param>
        /// <remarks>
        /// This method determines whether the zoom toolkit should be shown or hidden based on 
        /// the <see cref="ToolbarMode"/> setting and whether any axes are zoomed.
        /// </remarks>
        internal void ApplyZoomToolkit(SfChart chart, List<IChartElementRenderer> axes)
        {
            if (chart._zoomingModule is not null && !chart._zoomingModule.IsZoomingComplete)
            {
                chart._zoomingModule.IsPanning = IsAxisZoomed(axes) && _zooming is not null && _zooming.EnablePan;
            }
            if (IsAxisZoomed(axes) && !(chart._zoomSettings.ToolbarDisplayMode == ToolbarMode.None))
            {
                chart._zoomingToolkitContent?.ShowZoomingKit();
            }
            else if (chart._zoomSettings.ToolbarDisplayMode == ToolbarMode.Always)
            {
                chart._zoomingToolkitContent?.ShowZoomingKit();
            }
            else
            {
                chart._zoomingToolkitContent?.RemoveTooltip();
                chart._zoomingToolkitContent?.HideZoomingKit();
                chart.SetSvgCursor("auto");
            }
        }

        /// <summary>
        /// Determines whether any axis in the provided collection is currently zoomed.
        /// </summary>
        /// <param name="axes">The collection of axis renderers to check.</param>
        /// <returns><see langword="true"/> if at least one axis has zoom factor ≠ 1 or zoom position ≠ 0; otherwise, <see langword="false"/>.</returns>
        internal bool IsAxisZoomed(List<IChartElementRenderer> axes)
        {
            bool showToolkit = false;
            foreach (ChartAxisRenderer axisRenderer in axes.Cast<ChartAxisRenderer>())
            {
                showToolkit = IsZoomed = showToolkit || axisRenderer.Axis?.ZoomFactor != 1 || axisRenderer.Axis.ZoomPosition != 0;
            }

            return showToolkit;
        }

        /// <summary>
        /// Invokes the zoom end event after a debounce delay for mouse wheel interactions.
        /// </summary>
        /// <remarks>
        /// This method uses a debounce mechanism to avoid firing the event too frequently during continuous wheel scrolling.
        /// </remarks>
        internal async Task InvokeMouseWheelZoomEndAsync()
        {
            if (!_isWheelStart && (_previousRequestTime == DateTime.MinValue || (DateTime.Now - _previousRequestTime).TotalMilliseconds > UPDATETHRESHOLD))
            {
                _previousRequestTime = DateTime.Now;
                await Task.Delay(UPDATETHRESHOLD).ConfigureAwait(true);
                if (_wheelEndEventArgs is { })
                {
                    _wheelEndEventArgs.Name = Constants.OnZoomEnd;
                }

                if (_chart?.OnZoomEnd is not null)
                {
                    DataVizCommonHelper.InvokeEvent(_chart.OnZoomEnd, _wheelEndEventArgs ?? null);
                }
                _isWheelStart = true;
            }
        }

        /// <summary>
        /// Releases all resources used by the <see cref="Zoom"/> instance.
        /// </summary>
        /// <remarks>
        /// This method sets all references to <see langword="null"/> to ensure proper garbage collection.
        /// </remarks>
        internal void Dispose()
        {
            _chart = null;
            Browser = null;
            _zooming = null;
            TouchMoveList = null;
            TouchStartList = null;
        }
        #endregion
    }
}