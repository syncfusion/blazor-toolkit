using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.Toolkit.Internal;
using System.Dynamic;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renderer base for chart series. Handles data processing, axis assignment, clip paths,
    /// animations, markers and accessibility descriptions for series types.
    /// </summary>
    /// <remarks>
    /// This abstract class serves as the foundation for all series renderers in the chart component.
    /// It manages data binding, point calculations, animation orchestration, and SVG rendering coordination.
    /// Derived classes override virtual members to implement specific series visualization logic.
    /// </remarks>
    public abstract class ChartSeriesRenderer : ChartRenderer, IChartElementRenderer, IRequireAxis
    {
        #region Constants
        private const StringComparison INVARIANT_COMPARISON = StringComparison.InvariantCulture;
        #endregion

        #region Fields
        private Rect? _markerClipRect;

        // Cached JSON serializer options with floating-point support.
        internal JsonSerializerOptions _jsonOptions = new()
        {
            NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
        };
        #endregion

        #region Properties
        /// <summary>
        /// Gets the container managing this renderer element.
        /// </summary>
        [CascadingParameter]
        internal ChartRendererContainer? Container { get; set; }

        /// <summary>
        /// Gets or sets the index of this renderer within the series collection.
        /// </summary>
        [Parameter]
        public int RendererIndex { get; set; }

        /// <summary>
        /// Gets or sets the width of the X-axis in the rendering context.
        /// </summary>
        protected double XLength { get; set; }

        /// <summary>
        /// Gets or sets the height of the Y-axis in the rendering context.
        /// </summary>
        protected double YLength { get; set; }

        /// <summary>
        /// Gets or sets the current view data for this series.
        /// </summary>
        internal IEnumerable<object>? CurrentViewData { get; set; }

        /// <summary>
        /// Gets or sets the animation _options applied to this series.
        /// </summary>
        protected AnimationOptions? AnimationOptions { get; set; }

        /// <summary>
        /// Gets or sets the culture used for numeric and date formatting.
        /// </summary>
        protected CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;

        /// <summary>
        /// Gets the string comparison mode for culture-invariant operations.
        /// </summary>
        protected StringComparison StringComparison { get; set; } = StringComparison.InvariantCulture;

        /// <summary>
        /// Gets or sets the clipping rectangle for this series.
        /// </summary>
        internal Rect? ClipRect { get; set; }

        /// <summary>
        /// Gets or sets the associated series model.
        /// </summary>
        internal ChartSeries? Series { get; set; }

        /// <summary>
        /// Gets or sets the collection of rendered points for this series.
        /// </summary>
        internal List<Point>? Points { get; set; }

        /// <summary>
        /// Gets or sets the collection of chart-specific point data.
        /// </summary>
        internal List<IChartPoint>? ChartPoints { get; set; }

        /// <summary>
        /// Gets or sets the StringBuilder for accumulating chart data in serialization.
        /// </summary>
        internal StringBuilder? ChartData { get; set; }

        /// <summary>
        /// Gets the list of template IDs associated with this series.
        /// </summary>
        internal List<string> SeriesTemplateID { get; set; } = [];

        /// <summary>
        /// Gets or sets the ID for the error bar element.
        /// </summary>
        internal string? ErrorBarId { get; set; }

        /// <summary>
        /// Gets or sets the ID for the error bar clip rectangle.
        /// </summary>
        internal string? ErrorBarClipRectId { get; set; }

        /// <summary>
        /// Gets or sets the renderer index (alias for RendererIndex).
        /// </summary>
        internal int Index
        {
            get => RendererIndex; set => RendererIndex = value;
        }

        /// <summary>
        /// Gets or sets the fill color/style for this series.
        /// </summary>
        internal string? Interior { get; set; }

        /// <summary>
        /// Gets or sets stacking-related computed values for this series.
        /// </summary>
        internal StackValues? StackedValues { get; set; }

        /// <summary>
        /// Gets or sets the position parameter for series layout.
        /// </summary>
        internal double Position { get; set; } = double.NaN;

        /// <summary>
        /// Gets or sets the count of rectangles in stacking calculations.
        /// </summary>
        internal double RectCount { get; set; } = double.NaN;

        /// <summary>
        /// Gets or sets the source data index for this renderer.
        /// </summary>
        internal int SourceIndex { get; set; }

        /// <summary>
        /// Gets or sets whether trendlines in this series should be visible in the legend.
        /// </summary>
        internal bool TrendLineLegendVisibility { get; set; } = true;

        /// <summary>
        /// Gets or sets whether the series is currently being rendered.
        /// </summary>
        internal bool IsSeriesRender { get; set; }

        /// <summary>
        /// Gets or sets whether the X-axis data represents DateTimeOffset values.
        /// </summary>
        internal bool IsDateTimeOffset { get; set; }

        /// <summary>
        /// Gets or sets the focus index for the first series (static across instances).
        /// </summary>
        internal static double FirstFocusSeriesIndex { get; set; } = double.NaN;

        /// <summary>
        /// Gets or sets the focus index for the first trendline (static across instances).
        /// </summary>
        internal static double FirstFocusTrendlineSeriesIndex { get; set; } = double.NaN;

        /// <summary>
        /// Gets the dictionary of stacked point values keyed by point index.
        /// </summary>
        internal Dictionary<double, double> StackedPointValues { get; set; } = [];

        /// <summary>
        /// Gets or sets the renderer for the X-axis associated with this series.
        /// </summary>
        /// <value>The X-axis renderer. Default: <c>null</c>.</value>
        public ChartAxisRenderer XAxisRenderer { get; set; } = null!;

        /// <summary>
        /// Gets or sets the renderer for the Y-axis associated with this series.
        /// </summary>
        /// <value>The Y-axis renderer. Default: <c>null</c>.</value>
        public ChartAxisRenderer YAxisRenderer { get; set; } = null!;

        /// <summary>
        /// Gets or sets the name of the X-axis this series binds to.
        /// </summary>
        public string XAxisName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the name of the Y-axis this series binds to.
        /// </summary>
        public string YAxisName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the data range for X-axis values.
        /// </summary>
        public DoubleRange XRange { get; set; }

        /// <summary>
        /// Gets or sets the data range for Y-axis values.
        /// </summary>
        public DoubleRange YRange { get; set; }

        /// <summary>
        /// Gets or sets whether this series is visible.
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets the collection of X-axis data values for this series.
        /// </summary>
        public List<double> XData { get; set; } = null!;

        /// <summary>
        /// Gets or sets the collection of Y-axis data values for this series.
        /// </summary>
        public List<double> YData { get; set; } = null!;

        /// <summary>
        /// Gets or sets the minimum X-axis value in the series data.
        /// </summary>
        public double XMin { get; set; }

        /// <summary>
        /// Gets or sets the maximum X-axis value in the series data.
        /// </summary>
        public double XMax { get; set; }

        /// <summary>
        /// Gets or sets the minimum Y-axis value in the series data.
        /// </summary>
        public double YMin { get; set; }

        /// <summary>
        /// Gets or sets the maximum Y-axis value in the series data.
        /// </summary>
        public double YMax { get; set; }

        /// <summary>
        /// Gets or sets the maximum size parameter for marker/bubble series.
        /// </summary>
        public double MaxSize { get; set; }

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Initializes the series renderer and assigns renderer to series and container.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            InitSeriesRendererFields();
            Owner?._seriesContainer?.AddRenderer(this);
            UpdateCurrentViewData(Series?.CurrentViewData ?? null!);

            if (Series is not null)
            {
                Series.Renderer = this;
                Series.SeriesType = string.IsNullOrEmpty(Series.SeriesType) ? Series.Type.ToString() : Series.SeriesType;
            }

            SvgRenderer = Owner?._svgRenderer;
            _jsonOptions.Converters.Add(new DateOnlyConverter());
            _jsonOptions.Converters.Add(new TimeOnlyConverter());

            if (Owner?.InitialRect is not null && Series is not null && Series.NeedRendererUpdate)
            {
                InitializeSeriesAxis();
            }
            else if (Owner?.InitialRect is not null)
            {
                _ = Owner.ProcessOnLayoutChangeAsync();
            }
        }

        /// <summary>
        /// Handles parameter updates and triggers renderer initialization if necessary.
        /// </summary>
        internal override void OnParentParameterSet()
        {
            base.OnParentParameterSet();
        }

        /// <summary>
        /// Performs post-render work and animation orchestration.
        /// </summary>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            IsSeriesRender = !firstRender;
            if (!firstRender && Series?.Container is not null && Series.Container._redraw)
            {
                
                await Series.Container.PerformRedrawAnimationAsync().ConfigureAwait(true);
            }
            if (!firstRender)
            {
                Series?.Container?._striplineBehindContainer?.UpdateStriplineCollection();
                Series?.Container?._striplineOverContainer?.UpdateStriplineCollection();
            }
        }

        /// <summary>
        /// Releases resources and resets the component's state to prepare for disposal.
        /// </summary>
        protected override ValueTask DisposeAsyncCore()
        {
            Points?.Clear();
            ChartPoints?.Clear();
            ClipRect = null;
            _ = (ChartData?.Clear());
            AnimationOptions = null;
            _ = Owner?.ProcessOnLayoutChangeAsync();
            return base.DisposeAsyncCore();
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes axis renderers and processes series data on first render.
        /// </summary>
        private void InitializeSeriesAxis()
        {
            if (Series is null || Owner is null)
            {
                return;
            }
            Series.NeedRendererUpdate = false;
            Owner._isLiveChart = false;
            Owner._axisContainer?.AssignAxisToSeries(Owner._seriesContainer?._elementsRequiredAxis ?? null!, true);

            XAxisRenderer = Owner._axisContainer?.Axes[Series.XAxisName].Renderer ?? null!;
            YAxisRenderer = Owner._axisContainer?.Axes[Series.YAxisName].Renderer ?? null!;
            YAxisRenderer.IsStack100 = Series.SeriesType is not null && Series.SeriesType.Contains("100", INVARIANT_COMPARISON);

            if ((XAxisRenderer.Axis?.ValueType == ValueType.Category || XAxisRenderer.Axis?.ValueType == ValueType.DateTimeCategory) && XAxisRenderer.Axis.IsIndexed)
            {
                UpdateCategoryData();
            }
            else
            {
                ProcessData();
            }

            bool isStackingSeries = Series.SeriesType is not null && Series.SeriesType.Contains("Stacking", INVARIANT_COMPARISON);

            if (isStackingSeries)
            {
                Owner._seriesContainer?.CalculateStackedValue(Series.SeriesType is not null && Series.SeriesType.Contains("100", INVARIANT_COMPARISON));
            }

            _ = Owner.DelayLayoutChangeAsync();
        }

        /// <summary>
        /// Routes data processing based on detected data type (JSON, ExpandoObject, DynamicObject, or standard CLR).
        /// </summary>
        private void ProcessDataByType(string dataType, Type firstDataType, string xName, string yName, IEnumerable<object> currentViewData)
        {
            switch (dataType)
            {
                case "JsonElement":
                    ProcessJObjectData(firstDataType, xName, yName, currentViewData);
                    break;
                case "ExpandoObject":
                    ProcessExpandoObjectData(firstDataType, xName, yName, currentViewData);
                    break;
                case "DynamicObject":
                    ProcessDynamicObjectData(firstDataType, xName, yName, currentViewData);
                    break;
                default:
                    ProcessObjectData(firstDataType, xName, yName, currentViewData);
                    break;
            }
        }

        /// <summary>
        /// Processes DateTime X-axis values with format conversion.
        /// </summary>
        private void ProcessDateTimeValue(Point point, int index)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(point.X, Culture)))
            {
                if (XAxisRenderer?.Axis?.ValueType == ValueType.DateTime)
                {
                    point.XValue = XAxisRenderer.IsDateOnly || XAxisRenderer.IsTimeOnly || IsDateTimeOffset
                        ? ChartHelper.GetTime(Convert.ToDateTime(Convert.ToString(point.X, Culture), Culture))
                        : ChartHelper.GetTime(Convert.ToDateTime(point.X, Culture));
                }
                else
                {
                    PushCategoryData(point, index, ChartHelper.GetTime(Convert.ToDateTime(point.X, Culture)).ToString(Culture));
                }
            }
            else
            {
                point.Visible = false;
                point.X = null!;
                point.XValue = double.NaN;
            }
        }

        /// <summary>
        /// Finds and accumulates sort values for stacking calculations.
        /// </summary>
        private void FindSumOfSameIndex(string pointAxisKey, double sortValue, bool addPreviousValue = true)
        {
            if (Owner?._seriesContainer is not null && !Owner._seriesContainer._total.TryAdd(pointAxisKey, sortValue))
            {
                if (Owner._seriesContainer._total.TryGetValue(pointAxisKey, out double previousSortValue))
                {
                    Owner._seriesContainer._total[pointAxisKey] = sortValue + (addPreviousValue ? previousSortValue : 0);
                }
            }
        }

        /// <summary>
        /// Determines whether legend text mapping has changed.
        /// </summary>
        private bool IsLegendChanged()
        {
            for (int count = 0; count < Owner?._visibleSeriesRenderers.Count; count++)
            {
                if (Owner._legendRenderer?.LegendOptions.Count > 0 && Owner._visibleSeriesRenderers[count].Series?.Name != Owner._legendRenderer?.LegendOptions[count].TextOption.Text)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Updates trendline data sources for all trendlines in the series.
        /// </summary>
        private void UpdateTrendlineDataSource()
        {
            if (Series is not null)
            {
                foreach (ChartTrendline trendline in Series.Trendlines)
                {
                    trendline.TrendlineInitiator?.InitDataSource();
                }
            }
        }

        /// <summary>
        /// Updates trendline directions.
        /// </summary>
        private void UpdateTrendlineDirection()
        {
            if (Series is not null)
            {
                foreach (ChartTrendline trendline in Series.Trendlines)
                {
                    trendline.Renderer?.UpdateDirection();
                }
            }
        }

        /// <summary>
        /// Validates and triggers direction updates or rerenders as required.
        /// </summary>
        private void ValidateUpdateDirection()
        {
            if (Series is not null && Series.Marker.DataLabel.Visible && Owner is not null && Owner._shouldRenderDataLabel && IsCategoryAxis())
            {
                Series.UpdateSeriesCollection();
            }
            else
            {
                try
                {
                    if (Series is not null && Series._isSeriesChanged)
                    {
                        RenderSeries();
                    }
                    else
                    {
                        UpdateDirection();
                    }
                }
                catch
                {
                    if (Series is not null && !Series.UpdateDataSource)
                    {
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Handles layout changes by invoking the series renderer's layout change logic.
        /// </summary>
        void IChartElementRenderer.HandleLayoutChange()
        {
            HandleLayoutChange();
        }

        /// <summary>
        /// Triggers a re-render of the series element.
        /// </summary>
        void IChartElementRenderer.InvalidateRender()
        {
            InvalidateRender();
        }

        /// <summary>
        /// Animates rectangular series types by calculating center points and dimensions for animation info.
        /// </summary>
        private void AnimateRect(int index, Point point, List<InitialAnimationInfo> animationInfo, int animationInfoIndex)
        {
            bool isPlot = point.YValue < 0;
            double x, y, centerX, centerY;
            double elementHeight = point.Regions[0].Height;
            double elementWidth = point.Regions[0].Width;

            if (Owner is not null && !Owner._requireInvertedAxis)
            {
                x = point.Regions[0].X;
                if (Series?.SeriesType is not null && Series.SeriesType.Contains("Stacking", INVARIANT_COMPARISON))
                {
                    y = (1 - ChartHelper.ValueToCoefficient(0, YAxisRenderer)) * YAxisRenderer.Rect.Height;
                    centerX = x;
                    centerY = y;
                }
                else
                {
                    y = +point.Regions[0].Y;
                    centerY = (SeriesType().ToString().Contains("HighLow", INVARIANT_COMPARISON) || (Series?.SeriesType is not null && Series.SeriesType.Contains("Waterfall", INVARIANT_COMPARISON))) ? y + (elementHeight / 2) : (isPlot != YAxisRenderer.Axis?.IsAxisInverse) ? y : y + elementHeight;
                    centerX = isPlot ? x : x + elementWidth;
                }
            }
            else
            {
                y = +point.Regions[0].Y;
                if (Series?.SeriesType is not null && Series.SeriesType.Contains("Stacking", INVARIANT_COMPARISON))
                {
                    x = ChartHelper.ValueToCoefficient(0, YAxisRenderer) * YAxisRenderer.Rect.Width;
                    centerX = x;
                    centerY = y;
                }
                else
                {
                    x = +point.Regions[0].X;
                    centerY = isPlot ? y : y + elementHeight;
                    centerX = (SeriesType().ToString().Contains("HighLow", INVARIANT_COMPARISON) || (Series?.SeriesType is not null && Series.SeriesType.Contains("Waterfall", INVARIANT_COMPARISON))) ? x + (elementWidth / 2) : (isPlot != YAxisRenderer.Axis?.IsAxisInverse) ? x + elementWidth : x;
                }
            }

            animationInfo[animationInfoIndex].PointIndex.Add(index);
            animationInfo[animationInfoIndex].PointX.Add(centerX);
            animationInfo[animationInfoIndex].PointY.Add(centerY);
            animationInfo[animationInfoIndex].PointWidth.Add(elementWidth);
            animationInfo[animationInfoIndex].PointHeight.Add(elementHeight);
        }
        #endregion

        #region Protected Methods

        /// <summary>
        /// Retrieves the axis lengths and invokes series rendering.
        /// </summary>
        protected void SeriesRenderer()
        {
            if (XAxisRenderer is null || YAxisRenderer is null || XAxisRenderer.IsAxisRendererRect() || YAxisRenderer.IsAxisRendererRect())
            {
                return;
            }
            GetAxisLength();
            RenderSeries();
        }

        /// <summary>
        /// Computes X and Y axis lengths based on orientation.
        /// </summary>
        protected void GetAxisLength()
        {
            bool isInverted = Series?.Container?._requireInvertedAxis ?? false;
            if (XAxisRenderer is not null && YAxisRenderer is not null)
            {
                XLength = isInverted ? XAxisRenderer.Rect.Height : XAxisRenderer.Rect.Width;
                YLength = isInverted ? YAxisRenderer.Rect.Width : YAxisRenderer.Rect.Height;
            }
        }

        /// <summary>
        /// Performs the base series rendering logic and focus index tracking.
        /// </summary>
        protected virtual void RenderSeries()
        {
            RendererShouldRender = true;
            FindClipRect();

            if (Series?.Renderer.Container is not null && !Series.Renderer.Container.IsTrendLine)
            {
                GetSeriesFocusIndex();
            }
            else
            {
                GetTrendlineFocusIndex();
            }
        }

        /// <summary>
        /// Processes ExpandoObject data source, extracting X/Y values and applying any configured mappings.
        /// </summary>
        /// <param name="firstDataType">The type of the first data element.</param>
        /// <param name="xName">The property name for X-axis values.</param>
        /// <param name="yName">The property name for Y-axis values.</param>
        /// <param name="currentViewData">The enumerable collection of data objects.</param>
        protected virtual void ProcessExpandoObjectData(Type firstDataType, string xName, string yName, IEnumerable<object> currentViewData)
        {
            string pointColor = Series?.PointColorMapping ?? string.Empty;
            string tooltipText = Series?.TooltipMappingName ?? string.Empty;
            int index = 0;

            if (currentViewData is null)
            {
                return;
            }

            bool isSortingEnabled = !string.IsNullOrEmpty(Owner?._sorting.PropertyName) && !Owner._sorting.PropertyName.Equals("X", StringComparison.OrdinalIgnoreCase);

            foreach (object data in currentViewData)
            {
                IDictionary<string, object> expandoData = (IDictionary<string, object>)data;
                _ = expandoData.TryGetValue(xName, out object? x);
                _ = expandoData.TryGetValue(yName, out object? y);
                _ = expandoData.TryGetValue(pointColor, out object? color);
                _ = expandoData.TryGetValue(tooltipText, out object? tooltip);

                IChartPoint chartPoint = new();
                Point point = new()
                {
                    X = chartPoint.X = x ?? null!,
                    Y = chartPoint.Y = y ?? null!,
                    Interior = chartPoint.Interior = Convert.ToString(color, CultureInfo.InvariantCulture) ?? string.Empty,
                    Text = chartPoint.Text = Convert.ToString(GetTextMapping(), CultureInfo.InvariantCulture) ?? string.Empty,
                    Tooltip = chartPoint.Tooltip = Convert.ToString(tooltip, CultureInfo.InvariantCulture) ?? string.Empty
                };
                GetSetXValue(point, chartPoint, index);
                SetEmptyPoint(point, chartPoint, index, firstDataType);

                if (isSortingEnabled)
                {
                    FindExpandoObjectDataSortingValue(Owner?._sorting.PropertyName ?? null!, expandoData, point.X?.ToString() ?? string.Empty, point);
                }
                index++;
            }
        }

        /// <summary>
        /// Processes DynamicObject data source, extracting X/Y values using dynamic member access.
        /// </summary>
        protected virtual void ProcessDynamicObjectData(Type firstDataType, string xName, string yName, IEnumerable<object> currentViewData)
        {
            string pointColor = Series?.PointColorMapping ?? string.Empty;
            string tooltip = Series?.TooltipMappingName ?? string.Empty;
            int index = 0;

            if (currentViewData is null)
            {
                return;
            }

            bool isSortingEnabled = !string.IsNullOrEmpty(Owner?._sorting.PropertyName) && !Owner._sorting.PropertyName.Equals("X", StringComparison.OrdinalIgnoreCase);

            foreach (object data in currentViewData)
            {
                DynamicObject? dynamicObject = data as DynamicObject;
                IChartPoint chartPoint = new();
                Point point = new()
                {
                    X = chartPoint.X = ReflectionExtension.GetValueFromDynamicObject(dynamicObject!, xName) ?? 0,
                    Y = chartPoint.Y = ReflectionExtension.GetValueFromDynamicObject(dynamicObject!, yName) ?? 0,
                    Interior = chartPoint.Interior = ChartHelper.GetDynamicStringValue(dynamicObject ?? null!, pointColor),
                    Text = chartPoint.Text = ChartHelper.GetDynamicStringValue(dynamicObject ?? null!, GetTextMapping()),
                    Tooltip = chartPoint.Tooltip = ChartHelper.GetDynamicStringValue(dynamicObject ?? null!, tooltip)
                };

                GetSetXValue(point, chartPoint, index);
                SetEmptyPoint(point, chartPoint, index, firstDataType);

                if (isSortingEnabled)
                {
                    FindDynamicObjectDataSortingValue(Owner?._sorting.PropertyName ?? string.Empty, dynamicObject ?? null!, point.X?.ToString() ?? string.Empty, point);
                }
                index++;
            }
        }

        /// <summary>
        /// Processes JsonElement data source, extracting X/Y values with null-safety checks.
        /// </summary>
        protected virtual void ProcessJObjectData(Type firstDataType, string xName, string yName, IEnumerable<object> currentViewData)
        {
            string pointColor = Series?.PointColorMapping ?? null!;
            string tooltip = Series?.TooltipMappingName ?? string.Empty;
            int index = 0;

            if (currentViewData is null)
            {
                return;
            }

            bool isSortingEnabled = !string.IsNullOrEmpty(Owner?._sorting.PropertyName) && !Owner._sorting.PropertyName.Equals("X", StringComparison.OrdinalIgnoreCase);

            foreach (object data in currentViewData.ToArray())
            {
                JsonElement jsonObject = (JsonElement)data;
                IChartPoint chartPoint = new();
                Point point = new()
                {
                    X = chartPoint.X = ChartHelper.GetObjectValue(jsonObject.GetProperty(xName)),
                    Y = chartPoint.Y = jsonObject.GetProperty(yName).ValueKind == JsonValueKind.Null ? double.NaN : ChartHelper.GetObjectValue(jsonObject.GetProperty(yName)),
                    Interior = chartPoint.Interior = jsonObject.TryGetProperty(pointColor, out JsonElement pointelement) ? Convert.ToString(jsonObject.GetProperty(pointColor).GetString(), CultureInfo.InvariantCulture) ?? string.Empty : string.Empty,
                    Text = chartPoint.Text = jsonObject.TryGetProperty(GetTextMapping(), out JsonElement mappingelement) ? Convert.ToString(jsonObject.GetProperty(GetTextMapping()).GetString(), CultureInfo.InvariantCulture) ?? string.Empty : string.Empty,
                    Tooltip = chartPoint.Tooltip = jsonObject.TryGetProperty(tooltip, out JsonElement tootipelement) ? Convert.ToString(jsonObject.GetProperty(tooltip).GetString(), CultureInfo.InvariantCulture) ?? string.Empty : string.Empty
                };

                if (point.X is not null)
                {
                    GetSetXValue(point, chartPoint, index);
                    SetEmptyPoint(point, chartPoint, index, firstDataType);
                    if (isSortingEnabled)
                    {
                        FindJObjectDataSortingValue(Owner?._sorting.PropertyName ?? string.Empty, jsonObject, point.X.ToString() ?? string.Empty, point);
                    }
                    index++;
                }
            }
        }

        /// <summary>
        /// Processes standard CLR object data using reflection for optimal performance.
        /// </summary>
        protected virtual void ProcessObjectData(Type firstDataType, string xName, string yName, IEnumerable<object> currentViewData)
        {
            if (firstDataType is null || currentViewData is null || Series is null || Owner is null)
            {
                return;
            }

            using IPropertyAccessor x = FastReflectionExtension.CreateAccessor(firstDataType, xName);
            using IPropertyAccessor y = FastReflectionExtension.CreateAccessor(firstDataType, yName);
            using IPropertyAccessor pointColor = FastReflectionExtension.CreateAccessor(firstDataType, Series.PointColorMapping);
            using IPropertyAccessor textMapping = FastReflectionExtension.CreateAccessor(firstDataType, GetTextMapping());
            using IPropertyAccessor tooltipMapping = FastReflectionExtension.CreateAccessor(firstDataType, Series.TooltipMappingName);
            using IPropertyAccessor sortingInfo = FastReflectionExtension.CreateAccessor(firstDataType, Owner._sorting.PropertyName);

            int index = 0;
            XAxisRenderer.IsDateOnly = x.PropertyInfo.PropertyType.Name == "DateOnly";
            XAxisRenderer.IsTimeOnly = x.PropertyInfo.PropertyType.Name == "TimeOnly";
            IsDateTimeOffset = x.PropertyInfo.PropertyType.Name == "DateTimeOffset";

            bool isSortingEnabled = !string.IsNullOrEmpty(Owner?._sorting.PropertyName) && !Owner._sorting.PropertyName.Equals("X", StringComparison.OrdinalIgnoreCase);
            object[] tempArray = [.. currentViewData];
            XData = new double[tempArray.Length].ToList();

            foreach (object data in tempArray)
            {
                IChartPoint chartPoint = new();
                object text = textMapping.PropertyInfo is not null ? textMapping.GetValue(data) : string.Empty;
                Point point = new()
                {
                    X = chartPoint.X = x.GetValue(data),
                    Y = chartPoint.Y = y.PropertyInfo is not null ? y.GetValue(data) : GetPropertyValue(data, yName),
                    Interior = chartPoint.Interior = Convert.ToString(pointColor.GetValue(data), CultureInfo.InvariantCulture) ?? string.Empty,
                    Text = chartPoint.Text = text is not null ? Convert.ToString(text, CultureInfo.InvariantCulture) ?? string.Empty : null!,
                    Tooltip = chartPoint.Tooltip = Convert.ToString(tooltipMapping.GetValue(data), CultureInfo.InvariantCulture) ?? string.Empty
                };

                if (point.X is not null)
                {
                    GetSetXValue(point, chartPoint, index);
                    SetEmptyPoint(point, chartPoint, index, firstDataType);
                    if (isSortingEnabled)
                    {
                        FindObjectDataSortingValue(sortingInfo, data, point.X.ToString() ?? string.Empty, point);
                    }
                    index++;
                }
            }
        }

        /// <summary>
        /// Pushes a point's data into the YData and internal collections.
        /// </summary>
        /// <param name="point">The point containing data to push.</param>
        /// <param name="i">The point's collection index.</param>
        protected virtual void PushData(Point point, int i)
        {
            if (XData.Count < i || point is null)
            {
                return;
            }

            point.Index = i;
            point.YValue = (point.Y is not null) ? Convert.ToDouble(point.Y, Culture) : double.NaN;

            XData[i] = point.XValue;
        }

        /// <summary>
        /// Updates X/Y min/max bounds based on point values.
        /// </summary>
        /// <param name="xvalue">The X-axis value.</param>
        /// <param name="yvalue">The Y-axis value.</param>
        protected virtual void SetXYMinMax(double xvalue, double yvalue)
        {
            bool isLogAxis = YAxisRenderer.Axis?.ValueType == ValueType.Logarithmic || XAxisRenderer.Axis?.ValueType == ValueType.Logarithmic;
            bool isRectSeries = (Series?.SeriesType is not null && Series.SeriesType.Contains("Column", INVARIANT_COMPARISON)) || (Series?.SeriesType is not null && Series.SeriesType.Contains("Bar", INVARIANT_COMPARISON));
            double ymin = (isLogAxis && isRectSeries && !ChartHelper.SetRange(YAxisRenderer.Axis ?? null!)) ? 1 : yvalue;

            XMin = double.IsNaN(xvalue) ? XMin : Math.Min(XMin, xvalue);
            XMax = double.IsNaN(xvalue) ? XMax : Math.Max(XMax, xvalue);
            YMin = isLogAxis ? Math.Min(YMin, (double.IsNaN(ymin) || (ymin == 0)) ? YMin : ymin) :
                Math.Min(YMin, double.IsNaN(ymin) ? YMin : ymin);
            YMax = Math.Max(YMax, double.IsNaN(yvalue) ? YMax : yvalue);
        }

        /// <summary>
        /// Determines if a point's Y value is mapped for sorting purposes.
        /// </summary>
        protected virtual bool IsPointValueMapped<T>(T point, out double sortValue)
        {
            string sortKey = Owner?._sorting.PropertyName.ToUpperInvariant() ?? string.Empty;
            sortValue = 0.0;
            if (sortKey == "Y")
            {
                sortValue = (point as Point ?? null!).YValue;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Extracts sorting value from a standard CLR object.
        /// </summary>
        protected void FindObjectDataSortingValue<T>(IPropertyAccessor sortingInfo, object data, string x, T point)
        {
            if (IsPointValueMapped(point, out double pointSortValue))
            {
                FindSumOfSameIndex(FindSeriesAxisKey() + x, pointSortValue);
            }
            else
            {
                object sortObject = sortingInfo?.PropertyInfo is not null ? sortingInfo.GetValue(data) : GetPropertyValue(data, Owner?._sorting.PropertyName ?? string.Empty);
                double sortValue = (sortObject is not null) ? Convert.ToDouble(sortObject, null) : 0;
                FindSumOfSameIndex(FindSeriesAxisKey() + x, sortValue, false);
            }
        }

        /// <summary>
        /// Extracts sorting value from an ExpandoObject.
        /// </summary>
        protected void FindExpandoObjectDataSortingValue<T>(string sortkey, IDictionary<string, object> expandoData, string x, T point)
        {
            if (IsPointValueMapped(point, out double pointSortValue))
            {
                FindSumOfSameIndex(FindSeriesAxisKey() + x, pointSortValue);
            }
            else
            {
                if (expandoData is not null && expandoData.TryGetValue(sortkey, out object? sortValue) && sortValue is not null)
                {
                    FindSumOfSameIndex(FindSeriesAxisKey() + x, Convert.ToDouble(sortValue, Culture), false);
                }
            }
        }

        /// <summary>
        /// Extracts sorting value from a DynamicObject.
        /// </summary>
        protected void FindDynamicObjectDataSortingValue<T>(string sortkey, DynamicObject data, string x, T point)
        {
            if (IsPointValueMapped(point, out double pointSortValue))
            {
                FindSumOfSameIndex(FindSeriesAxisKey() + x, pointSortValue);
            }
            else
            {
                object? sortValue = ReflectionExtension.GetValueFromDynamicObject(data, sortkey);
                FindSumOfSameIndex(FindSeriesAxisKey() + x, Convert.ToDouble(sortValue, Culture), false);
            }
        }

        /// <summary>
        /// Extracts sorting value from a JsonElement.
        /// </summary>
        protected void FindJObjectDataSortingValue<T>(string sortkey, JsonElement jsonObject, string x, T point)
        {
            if (IsPointValueMapped(point, out double pointSortValue))
            {
                FindSumOfSameIndex(FindSeriesAxisKey() + x, pointSortValue);
            }
            else
            {
                object sortValue = jsonObject.GetProperty(sortkey).ValueKind == JsonValueKind.Null ? 0.0 : ChartHelper.GetObjectValue(jsonObject.GetProperty(sortkey));
                FindSumOfSameIndex(FindSeriesAxisKey() + x, Convert.ToDouble(sortValue, Culture), false);
            }
        }

        /// <summary>
        /// Computes the average of previous and next non-empty point values.
        /// </summary>
        /// <param name="type">The CLR type of the data object.</param>
        /// <param name="name">The property name for Y-axis values.</param>
        /// <param name="i">The index of the empty point.</param>
        /// <returns>The calculated average value.</returns>
        protected double GetAverage(Type type, string name, int i)
        {
            PropertyInfo prop = type?.GetProperty(name) ?? null!;
            IEnumerable<object> data = CurrentViewData ?? null!;

            object previousPoint = i - 1 > -1 ? data.ElementAt(i - 1) : null!;
            object nextPoint = i + 1 < data.Count() ? data.ElementAt(i + 1) : null!;

            return (((previousPoint is not null && prop.GetValue(previousPoint) is not null) ? (!double.IsNaN((double)(prop.GetValue(previousPoint) ?? null!)) ? (double)(prop.GetValue(previousPoint) ?? null!) : 0) : 0) +
                ((nextPoint is not null && prop.GetValue(nextPoint) is not null) ? !double.IsNaN((double)(prop.GetValue(nextPoint) ?? null!)) ? (double)(prop.GetValue(nextPoint) ?? null!) : 0 : 0)) / 2;
        }

        /// <summary>
        /// Gets the ID for the marker clip path.
        /// </summary>
        protected string MarkerClipPathID()
        {
            return Owner?.ID + "_ChartMarkerClipRect_" + Index;
        }

        /// <summary>
        /// Gets the ID for the error bar clip path.
        /// </summary>
        protected string ErrorBarClipPathID()
        {
            return Owner?.ID + "_ChartErrorBarClipRect_" + Index;
        }

        /// <summary>
        /// Handles layout changes by invoking the OnLayoutChange method.
        /// </summary>
        protected void HandleLayoutChange()
        {
            OnLayoutChange();
        }

        /// <summary>
        /// Retrieves the focus index for trendline series if applicable.
        /// </summary>
        protected void GetTrendlineFocusIndex()
        {
            if (Index == 0)
            {
                for (int value = 0; value < Owner?._visibleSeriesRenderers.Count; value++)
                {
                    ChartRendererContainer container = Owner._visibleSeriesRenderers[value].Container ?? null!;
                    ChartSeries series = Owner._visibleSeriesRenderers[value].Series ?? null!;
                    if (container.IsTrendLine && series.Focusable)
                    {
                        FirstFocusTrendlineSeriesIndex = Owner._visibleSeriesRenderers[value].Index;
                        value = Owner._visibleSeriesRenderers.Count;
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves the focus index for non-trendline series if applicable.
        /// </summary>
        protected void GetSeriesFocusIndex()
        {
            if (Index == 0)
            {
                for (int value = 0; value < Owner?._visibleSeriesRenderers.Count; value++)
                {
                    ChartRendererContainer container = Owner._visibleSeriesRenderers[value].Container ?? null!;
                    ChartSeries series = Owner._visibleSeriesRenderers[value].Series ?? null!;
                    if (!container.IsTrendLine && series.Focusable)
                    {
                        FirstFocusSeriesIndex = Owner._visibleSeriesRenderers[value].Index;
                        value = Owner._visibleSeriesRenderers.Count;
                    }
                }
            }
        }

        /// <summary>
        /// Determines if tooltip should be enabled based on series and chart settings.
        /// </summary>
        protected virtual bool IsTooltipEnabled()
        {
            return (Owner?._tooltip is not null && Owner._tooltip.Enable && Series is not null && Series.EnableTooltip &&
                (Series.Type == ChartSeriesType.Scatter || Series.Type == ChartSeriesType.Bubble || (Series.Marker is not null && !(Series.Marker.Visible && Owner._shouldRenderMarker)) || Series.Marker is null)) || (Owner?._crosshair is not null && Owner._crosshair.SnapToData);
        }

        /// <summary>
        /// Invalidates the current render, triggering a re-render of the series.
        /// </summary>
        protected void InvalidateRender()
        {
            StateHasChanged();
        }

        /// <summary>
        /// Handles layout changes by invoking the OnLayoutChange method.
        /// </summary>
        protected virtual void OnLayoutChange()
        {
            ProcessRenderQueue();
        }

        /// <summary>
        /// Creates the SVG elements for the series, applying necessary transformations and accessibility attributes.
        /// </summary>
        protected virtual void CreateSeriesElements(RenderTreeBuilder builder)
        {
            if (builder is null)
            {
                return;
            }
            CultureInfo culture = CultureInfo.InvariantCulture;
            SvgRenderer?.OpenGroupElement(builder, SeriesElementId(), "translate(" + ClipRect?.X.ToString(culture) + ',' + ClipRect?.Y.ToString(culture) + ')', SeriesClipPath(), "e-series-outline", Owner is not null && Owner.Focusable && Series is not null && Series.Focusable && !double.IsNaN(FirstFocusSeriesIndex) && Index == FirstFocusSeriesIndex ? "0" : "-1", GetSeriesDescriptionFormatText(Points ?? null!), "false", GetDataPoints(), !string.IsNullOrEmpty(Series?.AccessibilityRole) ? Series.AccessibilityRole : "group");
            SvgRenderer?.RenderClipPath(builder, ClipPathId(), GetSeriesClipRect(), ShouldAnimate() ? "hidden" : "visible");
        }

        /// <summary>
        /// Determines if animation should be applied based on series and chart settings.
        /// </summary>
        protected virtual bool ShouldAnimate()
        {
            return !IsStaticSSR() && ((Series is not null && Series.Animation.Enable && SyncfusionService?._options.Animation == GlobalAnimationMode.Default) || (SyncfusionService?._options.Animation == GlobalAnimationMode.Enable)) && Owner is not null && Owner._shouldAnimateSeries;
        }

        /// <summary>
        /// Retrieves the description text for accessibility purposes based on the series points.
        /// </summary>
        protected virtual string SeriesID()
        {
            return Owner?.ID + "_Series_" + Index;
        }

        /// <summary>
        /// Retrieves the description text for accessibility purposes based on the series points.
        /// </summary>
        protected virtual bool IsMarker()
        {
            return true;
        }

        /// <summary>
        /// Retrieves the description text for accessibility purposes based on the series points.
        /// </summary>
        protected virtual int IncFactor()
        {
            return 1;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Calculates marker height and updates padding if necessary.
        /// </summary>
        internal void CalculateMarkerHeightAndPadding()
        {
            if (Series is null || !Series.Visible || Owner is null)
            {
                return;
            }

            bool hasMinYValue = YData?.Any(y => y.ToString(CultureInfo.InvariantCulture) == YMin.ToString(CultureInfo.InvariantCulture)) == true;

            Owner._markerHeight = (Series?.Marker?.Visible == true && Series.Marker.Height > Owner._markerHeight) ? Series.Marker.Height : Owner._markerHeight;
            Owner._enablePadding = (!Owner._enablePadding && hasMinYValue) || Owner._enablePadding;
        }

        /// <summary>
        /// Finds and caches the clipping rectangle for this series rendering area.
        /// </summary>
        internal void FindClipRect()
        {
            Rect rect = ClipRect = new Rect();
            if (XAxisRenderer is not null && YAxisRenderer is not null)
            {
                if (Owner is not null && Owner._requireInvertedAxis)
                {
                    rect.X = YAxisRenderer.Rect.X;
                    rect.Y = XAxisRenderer.Rect.Y;
                    rect.Width = YAxisRenderer.Rect.Width + (YAxisRenderer.Axis?.PlotOffset ?? 0);
                    rect.Height = XAxisRenderer.Rect.Height + (XAxisRenderer.Axis?.PlotOffset ?? 0);
                }
                else
                {
                    rect.X = XAxisRenderer.Rect.X;
                    rect.Y = YAxisRenderer.Rect.Y;
                    rect.Width = XAxisRenderer.Rect.Width + (XAxisRenderer.Axis?.PlotOffset ?? 0);
                    rect.Height = YAxisRenderer.Rect.Height + (YAxisRenderer.Axis?.PlotOffset ?? 0);
                }
            }
        }

        /// <summary>
        /// Processes the current view data and extracts X/Y values based on data type.
        /// </summary>
        internal virtual void ProcessData()
        {
            SeriesRenderEventArgs eventArgs = new
            (
                "OnSeriesRender",
                false,
                Interior ?? string.Empty,
                Series?.CurrentViewData ?? null!,
                Series ?? null!
            );

            if ((Owner is not null && !Owner._isSeriesRendered) || (Owner?._legendRenderer is not null && Owner._legendRenderer.HasLegendClicked))
            {
                if (Owner.OnSeriesRender is not null)
                {
                    DataVizCommonHelper.InvokeEvent(Owner.OnSeriesRender, eventArgs);
                }
            }

            CurrentViewData = eventArgs.Data;
            Interior = GetSeriesFill(Series!, eventArgs.Fill);

            int dataCount = CurrentViewData.Count();
            if (dataCount == 0)
            {
                return;
            }

            XData = new double[dataCount].ToList();
            Type firstDataType = CurrentViewData.First().GetType();
            string xName = Series?.XName ?? string.Empty;
            string yName = Series?.YName ?? string.Empty;
            string dataType = DataVizCommonHelper.FindDataType(firstDataType);

            ProcessDataByType(dataType, firstDataType, xName, yName, CurrentViewData);

            if (Owner?._seriesContainer is not null)
            {
                foreach (IChartElementRenderer seriesRenderer in Owner._seriesContainer.Renderers)
                {
                    ChartSeriesRenderer series = seriesRenderer as ChartSeriesRenderer ?? null!;
                    series.FindSplinePoint();
                }
            }
        }

        /// <summary>
        /// Sets X-axis value and processes it based on axis type.
        /// </summary>
        /// <param name="point">The point to process.</param>
        /// <param name="chartPoint">The chart-specific point data.</param>
        /// <param name="index">The zero-based index of the point.</param>
        internal virtual void GetSetXValue(Point point, IChartPoint chartPoint, int index)
        {
            if (XAxisRenderer.Axis?.ValueType == ValueType.Category)
            {
                PushCategoryData(point, index, point.X.ToString() ?? string.Empty);
            }
            else if (XAxisRenderer.Axis?.ValueType is ValueType.DateTime or ValueType.DateTimeCategory)
            {
                ProcessDateTimeValue(point, index);
            }
            else
            {
                point.XValue = Convert.ToDouble(point.X, null);
            }

            PushData(point, index);
            chartPoint.XValue = point.XValue;
            chartPoint.YValue = point.YValue;
            chartPoint.Index = point.Index;
            ChartPoints?.Add(chartPoint);
            Points?.Add(point);
        }

        /// <summary>
        /// Finds visibility and updates min/max values for the point.
        /// </summary>
        /// <param name="point">The point to evaluate.</param>
        /// <returns><see langword="true"/> if the point is empty; otherwise <see langword="false"/>.</returns>
        internal virtual bool FindVisibility(Point point)
        {
            if (point is null)
            {
                return false;
            }

            SetXYMinMax(point.XValue, point.YValue);
            YData.Add(point.YValue);
            return point.X is null || (point.Y is null) || double.IsNaN(Convert.ToDouble(point.Y, Culture));
        }

        /// <summary>
        /// Recursively retrieves a property value from an object, supporting nested properties via dot notation.
        /// </summary>
        /// <param name="src">The source object to inspect.</param>
        /// <param name="propName">The property name, potentially containing dots for nested access (e.g., "Address.City").</param>
        /// <returns>The property value, or <c>null</c> if not found.</returns>
        /// <remarks>
        /// Uses reflection to locate properties. Searches nested objects if the direct property is not found.
        /// </remarks>
        internal static object GetPropertyValue(object src, string propName)
        {
            if (src is null || propName is null)
            {
                return null!;
            }
            if (propName.Contains('.', StringComparison.OrdinalIgnoreCase))
            {
                string[] prop = propName.Split(".", 2);
                return GetPropertyValue(GetPropertyValue(src, prop[0]), prop[1]);
            }
            else
            {
                PropertyInfo prop = src.GetType().GetProperty(propName) ?? null!;
                if (prop is not null)
                {
                    return prop.GetValue(src, null) ?? null!;
                }
                else
                {
                    PropertyInfo[] properties = src.GetType().GetProperties();
                    foreach (PropertyInfo property in properties)
                    {
                        if (property.PropertyType != typeof(string) && property.PropertyType.IsClass)
                        {
                            object value = src.GetType().GetProperty(property.Name)?.GetValue(src, null) ?? null!;
                            if (value is not null)
                            {
                                return GetPropertyValue(value, propName);
                            }
                        }
                    }
                }

                return null!;
            }
        }

        /// <summary>
        /// Determines whether the X-axis is a category type.
        /// </summary>
        internal bool IsCategoryAxis()
        {
            return XAxisRenderer.Axis?.ValueType is ValueType.Category or ValueType.DateTimeCategory;
        }

        /// <summary>
        /// Pushes category label to the axis and sets the point's X value.
        /// </summary>
        internal void PushCategoryData(Point point, int index, string pointX)
        {
            if ((Series is not null && Series.Visible) || (Series is not null && Series._isLegendClicked && IsStackingSeries()))
            {
                if (XAxisRenderer.Axis is not null && !XAxisRenderer.Axis.IsIndexed)
                {
                    if (XAxisRenderer.Labels.IndexOf(pointX) < 0 || string.IsNullOrEmpty(pointX))
                    {
                        XAxisRenderer.Labels.Add(pointX);
                    }

                    point.XValue = string.IsNullOrEmpty(pointX) ? index : XAxisRenderer.Labels.IndexOf(pointX);
                }
                else
                {
                    if (XAxisRenderer.Labels.Count - 1 >= index && !string.IsNullOrEmpty(XAxisRenderer.Labels[index]))
                    {
                        XAxisRenderer.Labels[index] += ", " + pointX;
                    }
                    else
                    {
                        XAxisRenderer.Labels.Add(pointX);
                    }

                    point.XValue = index;
                }
            }
        }

        /// <summary>
        /// Sets empty point handling and updates visibility based on the configured mode.
        /// </summary>
        /// <param name="point">The point to evaluate.</param>
        /// <param name="chartPoint">The chart-specific point data.</param>
        /// <param name="i">The point's index in the data collection.</param>
        /// <param name="type">The CLR type of the data object.</param>
        internal virtual void SetEmptyPoint(Point point, IChartPoint chartPoint, int i, Type type)
        {
            if (!FindVisibility(point))
            {
                point.Visible = true;
                return;
            }

            point.IsEmpty = true;
            if (Series?.EmptyPointSettings is not null)
            {
                switch (GetEmptyPointMode(Series.EmptyPointSettings.Mode))
                {
                    case EmptyPointMode.Zero:
                        point.Visible = true;
                        point.Y = point.YValue = YData[i] = (double)(chartPoint.Y = chartPoint.YValue = 0);
                        break;
                    case EmptyPointMode.Average:
                        CalculateAverageValue(point, chartPoint, i, type);
                        break;
                    case EmptyPointMode.Drop:
                    case EmptyPointMode.Gap:
                        YData[i] = 0;
                        point.Visible = false;
                        chartPoint.Y = point.Y = 0;
                        chartPoint.YValue = point.YValue = 0;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Determines the empty point mode to apply.
        /// </summary>
        internal virtual EmptyPointMode GetEmptyPointMode(EmptyPointMode mode)
        {
            return mode;
        }

        /// <summary>
        /// Calculates and assigns the average of neighboring values for an empty point.
        /// </summary>
        internal virtual void CalculateAverageValue(Point point, IChartPoint chartPoint, int i, Type type)
        {
            point.Y = point.YValue = YData[i] = (double)(chartPoint.Y = chartPoint.YValue = GetAverage(type, Series?.YName ?? string.Empty, i));
            point.Visible = true;
        }

        /// <summary>
        /// Gets the clip path ID for this series.
        /// </summary>
        internal virtual string ClipPathId()
        {
            return Owner?.ID + "_ChartSeriesClipRect_" + Index;
        }

        /// <summary>
        /// Gets the clip rectangle ID for this series.
        /// </summary>
        internal virtual string ClipRectId()
        {
            return ClipPathId() + "_Rect";
        }

        /// <summary>
        /// Gets the SVG clip path URL reference.
        /// </summary>
        internal string SeriesClipPath()
        {
            return "url(#" + ClipPathId() + ")";
        }

        /// <summary>
        /// Gets the ID for the series SVG group element.
        /// </summary>
        internal virtual string SeriesElementId()
        {
            return Owner?.ID + "SeriesGroup" + Index;
        }

        /// <summary>
        /// Gets the ID for the series symbol/marker SVG group element.
        /// </summary>
        internal string SeriesSymbolId()
        {
            return Container is not null && Container.IsTrendLine ? Owner?.ID + "TrendLineSymbolGroup" + Index : Owner?.ID + "SymbolGroup" + Index;
        }

        /// <summary>
        /// Gets the ID for the marker clip rectangle.
        /// </summary>
        internal virtual string MarkerClipRectId()
        {
            return MarkerClipPathID() + "_Rect";
        }

        /// <summary>
        /// Gets the SVG marker clip path URL reference.
        /// </summary>
        internal string MarkerClipPath()
        {
            return "url(#" + MarkerClipPathID() + ")";
        }

        /// <summary>
        /// Gets the column width for rectangular series types.
        /// </summary>
        /// <param name="minimumPointDelta">The minimum point spacing delta.</param>
        /// <returns>The computed column width.</returns>
        internal virtual double GetColumnWidth(double minimumPointDelta)
        {
            double columnWidth = ChartHelper.IsNaNOrZero(Series?.ColumnWidth ?? 0) ? 0.7 : Series?.ColumnWidth ?? 0;
            return minimumPointDelta * columnWidth;
        }

        /// <summary>
        /// Gets the visible series index, accounting for hidden/legend-clicked series.
        /// </summary>
        internal virtual int GetVisibleSeriesIndex()
        {
            if (Series is not null && !Series.Visible && Series._isLegendClicked)
            {
                for (int value = Index; value >= 0; value--)
                {
                    ChartSeries series = Owner?._visibleSeriesRenderers[value].Series ?? null!;
                    if (Owner is not null && series is not null && series.Visible)
                    {
                        return Owner._visibleSeriesRenderers[value].Index;
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// Gets the stacking start value for a specific point index and series index.
        /// </summary>
        internal virtual double GetStackingStartValue(int pointIndex, int seriesIndex)
        {
            return Series is not null && !Series.Visible && Series._isLegendClicked && seriesIndex >= 0 && seriesIndex < Owner?._visibleSeriesRenderers.Count && Owner._visibleSeriesRenderers[seriesIndex].StackedValues is not null &&
                pointIndex < Owner._visibleSeriesRenderers[seriesIndex].StackedValues?.StartValues?.Count && pointIndex < Owner._visibleSeriesRenderers[seriesIndex].StackedValues?.EndValues?.Count
                ? Index > 0 ? Owner._visibleSeriesRenderers[seriesIndex].StackedValues?.EndValues[pointIndex] ?? 0 : Series.Renderer.StackedValues?.StartValues[pointIndex] ?? 0
                : 0;
        }

        /// <summary>
        /// Determines whether this is a stacking series.
        /// </summary>
        internal virtual bool IsStackingSeries()
        {
            string? seriesType = Series?.Type.ToString();
            return (seriesType?.IndexOf("StackingArea", StringComparison.Ordinal) > -1) ||
                (seriesType?.IndexOf("StackingColumn", StringComparison.Ordinal) > -1) ||
                (seriesType?.IndexOf("StackingBar", StringComparison.Ordinal) > -1);
        }

        /// <summary>
        /// Determines whether the specified series represents a stacking column at an outer position in the chart.
        /// </summary>
        /// <param name="isMinus">A value indicating whether the series is in the negative direction. Set to <see langword="true"/> for
        /// negative stacking; otherwise, <see langword="false"/>.</param>
        /// <param name="series">The chart series to evaluate for stacking column and outer position status.</param>
        /// <returns>A value indicating whether the series is a stacking column at an outer position. <see langword="true"/> if
        /// the series meets the criteria; otherwise, <see langword="false"/>.</returns>
        internal virtual bool IsStackingColumnAndOuterPosition(bool isMinus, ChartSeries series)
        {
            return false;
        }

        /// <summary>
        /// Updates the current view data for the instance and, if applicable, for the associated series.
        /// </summary>
        /// <remarks>If the associated series exists and its current view data is empty, this method also
        /// updates the series with the provided data.</remarks>
        /// <param name="currentViewData">The collection of data objects to set as the current view data. Cannot be null.</param>
        internal void UpdateCurrentViewData(IEnumerable<object> currentViewData)
        {
            CurrentViewData = currentViewData;
            if (Series is not null && !Series.CurrentViewData.Any())
            {
                Series.CurrentViewData = currentViewData;
            }
        }

        /// <summary>
        /// Sets the current view data to the specified collection if the existing view data is empty.
        /// </summary>
        /// <param name="currentViewData">The collection of objects to assign as the current view data. Cannot be null.</param>
        internal void SetCurrentViewData(IEnumerable<object> currentViewData)
        {
            if (CurrentViewData is not null && !CurrentViewData.Any())
            {
                UpdateCurrentViewData(currentViewData);
            }
        }

        /// <summary>
        /// Updates the series key mapping for the specified chart series renderer within the series container.
        /// </summary>
        /// <param name="seriesRenderer">The chart series renderer whose series key and index are to be updated. Must not be null and must reference
        /// a valid series.</param>
        internal void UpdateSeriesKey(ChartSeriesRenderer seriesRenderer)
        {
            if (Owner is not null && seriesRenderer.Series is not null && Owner._seriesContainer is not null && !Owner._seriesContainer._seriesIndexes.ContainsKey(seriesRenderer.Series.GenerateSeriesKey()))
            {
                Owner._seriesContainer._seriesIndexes[seriesRenderer.Series.GenerateSeriesKey()] = seriesRenderer.Index;
            }
        }

        /// <summary>
        /// Updates the series data and related chart elements, ensuring that axis ranges, stacking values, trendlines,
        /// and indicators are recalculated as needed.
        /// </summary>
        /// <param name="isRemoteData">Indicates whether the series data update is triggered by remote data. If <see langword="true"/>, the update
        /// process will not refresh the current view data from the series.</param>
        /// <returns>A task that represents the asynchronous operation of updating the series data and chart state.</returns>
        internal async Task UpdateSeriesDataAsync(bool isRemoteData = false)
        {
            bool isLastRenderer = Owner?._seriesContainer?.Renderers.Last() == Series?.Renderer;
            if (!isRemoteData && Series is not null)
            {
                UpdateCurrentViewData(await Series.UpdateSeriesDataAsync().ConfigureAwait(false));
            }
            Owner?._seriesContainer?._seriesIndexes.Clear();
            if (Owner?._seriesContainer is not null)
            {
                foreach (ChartSeriesRenderer renderer in Owner._seriesContainer.Renderers.Cast<ChartSeriesRenderer>())
                {
                    UpdateSeriesKey(renderer);
                }
            }

            if (XAxisRenderer is null || YAxisRenderer is null)
            {
                Owner?.InitiAxis();
            }
            if (IsCategoryAxis())
            {
                UpdateCategoryData();
                Owner?._seriesContainer?.UpdateStackingValues();
            }
            else
            {
                XAxisRenderer?.Labels.Clear();
                InitSeriesRendererFields();
                ProcessData();
            }

            if (Series?.SeriesType is not null && Series.SeriesType.Contains("Stacking", INVARIANT_COMPARISON))
            {
                Owner?._seriesContainer?.CalculateStackedValue(Series?.SeriesType is not null && Series.SeriesType.Contains("100", INVARIANT_COMPARISON));
            }

            UpdateTrendlineDataSource();

            if (XAxisRenderer is not null && XAxisRenderer.IsFixedRange() && YAxisRenderer is not null && YAxisRenderer.IsFixedRange())
            {
                ValidateUpdateDirection();
                UpdateTrendlineDirection();
            }
            else
            {
                if (YAxisRenderer is not null && !YAxisRenderer.IsFixedRange() && YAxisRenderer.NeedAxisLayoutChange(YMin, YMax))
                {
                    if (Owner is not null && !Owner._isLiveChart && isLastRenderer)
                    {
                        await Owner.ProcessOnLayoutChangeAsync().ConfigureAwait(true);
                    }
                }
                else
                {
                    bool isRangeChanged = false;
                    bool legendChanged = IsLegendChanged();

                    if (XAxisRenderer is DateTimeCategoryAxisRenderer && !XAxisRenderer.IsFixedRange() && XAxisRenderer.NeedAxisLayoutChange(XMin, XMax) && isLastRenderer)
                    {
                        Owner?.OnLayoutChange();
                    }
                    else if ((XAxisRenderer is not null && !XAxisRenderer.IsFixedRange() && XMin < XAxisRenderer.ActualRange.Start) || XMax > XAxisRenderer?.ActualRange.End)
                    {
                        XAxisRenderer.ChangeAxisRange();
                        ValidateUpdateDirection();
                        isRangeChanged = true;
                    }

                    if ((YAxisRenderer is not null && !YAxisRenderer.IsFixedRange() && YMin < YAxisRenderer.ActualRange.Start) || YMax > YAxisRenderer?.ActualRange.End)
                    {
                        YAxisRenderer.ChangeAxisRange();
                        ValidateUpdateDirection();
                        isRangeChanged = true;
                    }

                    if (!isRangeChanged || legendChanged)
                    {
                        if (legendChanged && isLastRenderer && Owner is not null)
                        {
                            await Owner.ProcessOnLayoutChangeAsync().ConfigureAwait(false);
                        }
                        else
                        {
                            ValidateUpdateDirection();
                        }
                    }

                    UpdateTrendlineDirection();
                }
            }
        }

        /// <summary>
        /// Finds and returns a collection of visible rectangular chart series that are associated with both the
        /// specified column and row renderers.
        /// </summary>
        /// <param name="columnRenderer">The column renderer containing axes and series renderers to search for matching chart series.</param>
        /// <param name="rowRenderer">The row renderer containing axes and series renderers to search for matching chart series.</param>
        /// <returns>A list of chart series that are visible, rectangular, and present in both the column and row renderers. The
        /// list will be empty if no matching series are found.</returns>
        internal static List<ChartSeries> FindSeriesCollection(ChartColumnRenderer columnRenderer, ChartRowRenderer rowRenderer)
        {
            List<ChartSeries> seriesCollection = [];
            foreach (ChartAxis rowAxis in rowRenderer.Axes)
            {
                if (rowAxis.Renderer is not null)
                {
                    foreach (ChartSeriesRenderer rowSeriesRenderer in rowAxis.Renderer.SeriesRenderer)
                    {
                        foreach (ChartAxis axis in columnRenderer.Axes)
                        {
                            if (axis.Renderer is not null)
                            {
                                foreach (ChartSeriesRenderer columnseriesRenderer in axis.Renderer.SeriesRenderer)
                                {
                                    if (columnseriesRenderer == rowSeriesRenderer && columnseriesRenderer.Series is not null && columnseriesRenderer.Series.Visible && columnseriesRenderer.IsRectSeries())
                                    {
                                        seriesCollection.Add(columnseriesRenderer.Series);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return seriesCollection;
        }

        /// <summary>
        /// Sets the border color for a point, applying empty point border settings if needed.
        /// </summary>
        /// <param name="point">The data point.</param>
        /// <param name="border">The default border specification.</param>
        /// <returns>The computed border specification.</returns>
        internal ChartEventBorder SetBorderColor(Point point, ChartEventBorder border)
        {
            border.Width = point.IsEmpty && Series?.EmptyPointSettings?.Border?.Width is double emptyWidth && emptyWidth != 0 ? emptyWidth : border.Width;
            border.Color = point.IsEmpty ? Series?.EmptyPointSettings?.Border?.Color ?? border.Color : border.Color;
            return border;
        }

        /// <summary>
        /// Updates the marker direction and related rendering state for the chart series.
        /// </summary>
        internal virtual void UpdateDirection()
        {
            RendererShouldRender = true;
            _ = InvokeAsync(StateHasChanged);

            if (Series?.Type is not ChartSeriesType.Scatter and not ChartSeriesType.Bubble)
            {
                Series?.Marker?.Renderer?.UpdateDirection();
            }
            CalculateMarkerHeightAndPadding();
        }

        /// <summary>
        /// Updates the customization state of the series renderer based on the specified property.
        /// </summary>
        /// <param name="property">The name of the property that triggers the customization update. For example, use "Fill" to update the
        /// renderer's interior appearance.</param>
        internal virtual void UpdateCustomization(string property)
        {
            RendererShouldRender = Series?.Visible ?? true;
            if (property == "Fill")
            {
                Owner?._seriesContainer?.GetSeriesRendererInterior(this);
            }
        }

        /// <summary>
        /// Determines the renderer type associated with the specified chart series and draw type.
        /// </summary>
        /// <param name="type">The chart series type for which to retrieve the renderer. Must be a supported value of <see
        /// cref="ChartSeriesType"/>.</param>
        /// <returns>A <see cref="Type"/> representing the renderer class for the specified chart series and draw type.</returns>
        /// <exception cref="NotImplementedException">Thrown if the specified chart series type is not supported.</exception>
        internal static Type GetRendererType(ChartSeriesType type)
        {
            switch (type)
            {
                case ChartSeriesType.Line:
                    return typeof(LineSeriesRenderer);

                case ChartSeriesType.Spline:
                    return typeof(SplineSeriesRenderer);

                case ChartSeriesType.Area:
                    return typeof(AreaSeriesRenderer);

                case ChartSeriesType.SplineArea:
                    return typeof(SplineAreaSeriesRenderer);

                case ChartSeriesType.StepLine:
                    return typeof(StepLineSeriesRenderer);

                case ChartSeriesType.StepArea:
                    return typeof(StepAreaSeriesRenderer);

                case ChartSeriesType.Column:
                    return typeof(ColumnSeriesRenderer);

                case ChartSeriesType.Bar:
                    return typeof(BarSeriesRenderer);

                case ChartSeriesType.StackingColumn:
                    return typeof(StackingColumnSeriesRenderer);

                case ChartSeriesType.StackingColumn100:
                    return typeof(StackingColumn100SeriesRenderer);

                case ChartSeriesType.StackingBar:
                    return typeof(StackingBarSeriesRenderer);

                case ChartSeriesType.StackingBar100:
                    return typeof(StackingBar100SeriesRenderer);

                case ChartSeriesType.StackingLine:
                    return typeof(StackingLineSeriesRenderer);

                case ChartSeriesType.StackingLine100:
                    return typeof(StackingLine100SeriesRenderer);

                case ChartSeriesType.StackingArea:
                    return typeof(StackingAreaSeriesRenderer);

                case ChartSeriesType.StackingArea100:
                    return typeof(StackingArea100SeriesRenderer);

                case ChartSeriesType.StackingStepArea:
                    return typeof(StackingStepAreaSeriesRenderer);

                case ChartSeriesType.Bubble:
                    return typeof(BubbleSeriesRenderer);

                case ChartSeriesType.MultiColoredLine:
                    return typeof(MultiColoredLineSeriesRenderer);

                case ChartSeriesType.MultiColoredArea:
                    return typeof(MultiColoredAreaSeriesRenderer);

                case ChartSeriesType.Scatter:
                    return typeof(ScatterSeriesRenderer);
                default:
                    break;
            }

            throw new NotImplementedException("Specified chart type is not implemented");
        }

        /// <summary>
        /// Initializes fields related to series rendering to their default values.
        /// </summary>
        internal virtual void InitSeriesRendererFields()
        {
            Points = [];
            ChartPoints = [];
            ChartData = new StringBuilder();
            XData = [];
            YData = [];
            XMin = double.PositiveInfinity;
            XMax = double.NegativeInfinity;
            YMin = double.PositiveInfinity;
            YMax = double.NegativeInfinity;
        }

        /// <summary>
        /// Constructs a composite key for series-axis identification.
        /// </summary>
        /// <returns>A string combining X and Y axis names.</returns>
        internal string FindSeriesAxisKey()
        {
            return Series?.XAxisName + "_" + Series?.YAxisName + "_";
        }

        /// <summary>
        /// Finds a point on a spline curve. Intended for use by derived classes to implement specific spline point
        /// calculations.
        /// </summary>
        internal virtual void FindSplinePoint()
        {
        }

        /// <summary>
        /// Calculates and adds control points for a specified spline type to the provided collection.
        /// </summary>
        internal virtual void GetSplineTypePoints(List<Point> points, int i, SplineType splineType, bool isLow)
        {
        }

        /// <summary>
        /// Initializes or modifies the collection of points for a default spline type at the specified index.
        /// </summary>
        internal virtual void GetDefaultSplineTypePoints(List<Point> points, int i, bool isLow)
        {
        }

        /// <summary>
        /// Gets the property name used for data label text mapping.
        /// </summary>
        internal virtual string GetTextMapping()
        {
            return Series?.Marker?.DataLabel?.Name ?? string.Empty;
        }

        /// <summary>
        /// Updates the rendering state and axis ranges for the series, ensuring that empty data points and stacking
        /// values are correctly processed.
        /// </summary>
        internal void UpdateEmptyPoint()
        {
            RendererShouldRender = Series?.Visible ?? true;
            if (RendererShouldRender)
            {
                InitSeriesRendererFields();
                ProcessData();
                if (Series?.SeriesType is not null && Series.SeriesType.Contains("Stacking", INVARIANT_COMPARISON))
                {
                    Owner?._seriesContainer?.CalculateStackedValue(Series.SeriesType is not null && Series.SeriesType.Contains("100", INVARIANT_COMPARISON));
                }

                if ((!XAxisRenderer.IsFixedRange() && XMin < XAxisRenderer.ActualRange.Start) || XMax > XAxisRenderer.ActualRange.End)
                {
                    XAxisRenderer.ChangeAxisRange();
                }

                if ((!YAxisRenderer.IsFixedRange() && YMin < YAxisRenderer.ActualRange.Start) || YMax > YAxisRenderer.ActualRange.End)
                {
                    YAxisRenderer.ChangeAxisRange();
                }

                Series?.UpdateSeriesCollection();
            }
        }

        /// <summary>
        /// Calculates the clipping rectangle for the series area, adjusted for axis inversion and plot offset.
        /// </summary>
        /// <returns>A <see cref="Rect"/> representing the clipping region for the series. The rectangle's position and size are
        /// determined by the current axis configuration and plot offset.</returns>
        internal Rect GetSeriesClipRect()
        {
            Rect rect = new()
            {
                Width = ClipRect?.Width ?? 0,
                Height = ClipRect?.Height ?? 0
            };

            double plotOffset = Series?.Renderer.XAxisRenderer.Axis is not null ? Series.Renderer.XAxisRenderer.Axis.PlotOffset : 0;
            double halfPlotOffset = plotOffset != 0 ? -(plotOffset / 2) : 0;

            if (Owner is not null && Owner._requireInvertedAxis)
            {
                rect.X = 0;
                rect.Y = halfPlotOffset;
            }
            else
            {
                rect.X = halfPlotOffset;
                rect.Y = 0;
            }

            return rect;
        }

        /// <summary>
        /// Populates the specified collection with initial animation information for chart elements based on the
        /// current animation options and series configuration.
        /// </summary>
        /// <param name="animationInfo">A list to which initial animation details for chart elements will be added. Must not be null.</param>
        internal void PerformInitialAnimation(List<InitialAnimationInfo> animationInfo)
        {
            AnimationOptions options = AnimationOptions ?? null!;
            if (options is null)
            {
                return;
            }

            string animationType = options.Type.ToString();
            double duration = Series?.Animation?.Duration ?? 1000;
            double delay = Series?.Animation?.Delay ?? 0;

            if (options.Type == AnimationType.Progressive)
            {
                animationInfo.Add(new InitialAnimationInfo { Type = animationType, ElementId = options.Id, ClipPathId = ClipRectId(), Duration = duration, Delay = delay, StrokeDashArray = Series?.DashArray ?? string.Empty });
            }
            else if (options.Type == AnimationType.Linear)
            {
                animationInfo.Add(new InitialAnimationInfo { Type = animationType, ElementId = options.Id, ClipPathId = ClipRectId(), Duration = duration, Delay = delay, IsInvertedAxis = Series?.Container?._requireInvertedAxis ?? false });
            }
            else if (options.Type == AnimationType.Rect)
            {
                animationInfo.Add(new InitialAnimationInfo { Type = animationType, ElementId = options.Id, ClipPathId = ClipRectId(), Duration = duration, Delay = delay, IsInvertedAxis = Owner?._requireInvertedAxis ?? false });
                int animationInfoIndex = animationInfo.Count - 1;
                int count = Category() == SeriesCategories.Indicator ? 0 : 1;
                List<Point> visiblePoints = ChartHelper.GetVisiblePoints(Series?.Renderer.Points ?? null!);
                foreach (Point point in visiblePoints)
                {
                    if (point.SymbolLocations.Count == 0)
                    {
                        continue;
                    }

                    AnimateRect(count, point, animationInfo, animationInfoIndex);
                    count++;
                }
            }
            else if (options.Type == AnimationType.Marker)
            {
                animationInfo.Add(new InitialAnimationInfo { Type = animationType, ElementId = options.Id, ClipPathId = ClipRectId(), Duration = duration, Delay = delay });
                int animationInfoIndex = animationInfo.Count - 1;
                int count = 1;
                if (Points is not null)
                {
                    foreach (Point point in Points)
                    {
                        if (point.SymbolLocations.Count == 0)
                        {
                            continue;
                        }

                        animationInfo[animationInfoIndex].PointIndex.Add(count);
                        animationInfo[animationInfoIndex].PointX.Add(point.SymbolLocations[0].X);
                        animationInfo[animationInfoIndex].PointY.Add(point.SymbolLocations[0].Y);
                        count++;
                    }
                }
            }

            InitialAnimationInfo currentAnimationInfo = animationInfo[^1];
            if (Series?.Marker?.Visible == true && IsMarker() && Owner?._shouldRenderMarker == true)
            {
                int j = 1;
                int incFactor = IncFactor();
                currentAnimationInfo.MarkerInfo = new MarkerAnimationInfo() { MarkerElementId = SeriesSymbolId(), MarkerClipPathId = Series?.Renderer?.MarkerClipRectId() ?? string.Empty };
                for (int i = 0; i < Points?.Count; i++)
                {
                    if (Points[i].SymbolLocations is not null)
                    {
                        if (Points[i].SymbolLocations.Count == 0)
                        {
                            continue;
                        }

                        currentAnimationInfo.MarkerInfo.PointIndex.Add(j);
                        currentAnimationInfo.MarkerInfo.PointX.Add(Points[i].SymbolLocations[0].X);
                        currentAnimationInfo.MarkerInfo.PointY.Add(Points[i].SymbolLocations[0].Y);

                        j += incFactor;
                    }
                }
            }

            if (Owner is not null && Owner._shouldRenderDataLabel)
            {
                if (Series?.Marker?.DataLabel?.Visible == true && Series.Marker.DataLabel.Template is null)
                {
                    currentAnimationInfo.DataLabelInfo = new DataLabelAnimatioInfo { ShapeGroupId = Series.Marker.DataLabel.Renderer?.SeriesShapeId() ?? null!, TextGroupId = Series.Marker.DataLabel.Renderer?.SeriesTextId() ?? null! };
                }
                else if (Series?.Marker?.DataLabel?.Visible == true && Series.Marker.DataLabel.Template is not null)
                {
                    currentAnimationInfo.DataLabelInfo = new DataLabelAnimatioInfo { TemplateId = SeriesTemplateID };
                }
            }
        }

        /// <summary>
        /// Gets the label text for a data point, typically used for data label rendering.
        /// </summary>
        /// <param name="currentPoint">The data point.</param>
        /// <param name="seriesRenderer">The series renderer context (optional).</param>
        /// <returns>A list of formatted label strings.</returns>
        internal virtual List<string> GetLabelText(Point currentPoint, ChartSeriesRenderer seriesRenderer)
        {
            return
            [
                !string.IsNullOrEmpty(currentPoint.Text) ? currentPoint.Text : currentPoint.YValue.ToString(CultureInfo.InvariantCulture)
            ];
        }

        /// <summary>
        /// Determines whether this is a rectangular series (Column, Bar, etc.).
        /// </summary>
        internal virtual bool IsRectSeries()
        {
            return false;
        }

        /// <summary>
        /// Determines whether this is a path-based series (Line, Area, etc.).
        /// </summary>
        internal virtual bool IsPathSeries()
        {
            return false;
        }

        /// <summary>
        /// Determines whether this is a stacking series.
        /// </summary>
        internal virtual SeriesValueType SeriesType()
        {
            return SeriesValueType.XY;
        }

        /// <summary>
        /// Determines whether markers should be rendered for this series.
        /// </summary>
        internal virtual bool ShouldRenderMarker()
        {
            return true;
        }

        /// <summary>
        /// Gets the Y value for a marker at a given point (e.g., High/Low for range series).
        /// </summary>
        /// <param name="point">The data point.</param>
        /// <returns>The Y value to use for marker rendering.</returns>
        internal virtual object GetMarkerY(Point point)
        {
            return point.Y;
        }

        /// <summary>
        /// Calculates the clipping region for marker rendering based on the current series and marker settings.
        /// </summary>
        internal virtual void CalculateMarkerClipPath()
        {
            double explodeValue = (Series?.Marker?.Border?.Width ?? 0) + 13;
            bool isZoomed = false;

            if (Series?.Marker?.Visible == true && ClipRect is not null)
            {
                double markerHeight = isZoomed ? 0 : ((Series.Marker.Height + explodeValue) / 2);
                double markerWidth = isZoomed ? 0 : ((Series.Marker.Width + explodeValue) / 2);
                _markerClipRect = new Rect(-markerWidth, -markerHeight, ClipRect.Width + (markerWidth * 2), ClipRect.Height + (markerHeight * 2));
            }
        }

        /// <summary>
        /// Generates a formatted accessibility text string for the specified data point, combining X and Y axis values
        /// with the series name.
        /// </summary>
        /// <param name="point">The data point for which to generate the accessibility text. The X and Y coordinates represent axis values
        /// to be formatted.</param>
        /// <returns>A string containing the formatted X and Y axis values and the series name, suitable for use in accessibility
        /// scenarios.</returns>
        internal virtual string CalCulateAccessibilityText(Point point)
        {
            return XAxisRenderer.GetFormatText(GetPointXValue(point.X, XAxisRenderer.DateFormat ?? string.Empty)) + ": " + YAxisRenderer.GetFormatText(GetPointXValue(point.Y, YAxisRenderer.DateFormat ?? string.Empty)) + ", " + Series?.Name;
        }

        /// <summary>
        /// Returns the X value for a chart point, formatted as a date if a trend line is present and a date format is
        /// specified.
        /// </summary>
        /// <param name="pointX">The original X value of the chart point. If a trend line is present and a date format is specified, this
        /// value is converted to a date; otherwise, it is returned as-is.</param>
        /// <param name="dateFormat">The date format string to use when formatting the X value as a date. If null or empty, the X value is not
        /// formatted as a date.</param>
        /// <returns>An object representing the X value of the chart point. If a trend line is present and a date format is
        /// specified, returns a DateTime object; otherwise, returns the original value.</returns>
        internal virtual object GetPointXValue(object pointX, string dateFormat)
        {
            return Series?.Renderer.Container is not null && Series.Renderer.Container.IsTrendLine && !string.IsNullOrEmpty(dateFormat) ? DateTime.Parse(Intl.GetDateFormat(ChartHelper.GetDate(Convert.ToDouble(pointX, Culture)), string.Empty), CultureInfo.CurrentCulture) : pointX;
        }

        /// <summary>
        /// Generates a formatted description string for the series, including its name, type, and the number of data
        /// points.
        /// </summary>
        /// <param name="points">A list of data points associated with the series. The count of points is included in the description.</param>
        /// <returns>A string containing the accessibility description if available; otherwise, a formatted description of the
        /// series and its data points.</returns>
        internal string GetSeriesDescriptionFormatText(List<Point> points)
        {
            return Series is not null && Series.AccessibilityDescription is not null ? Series.AccessibilityDescription : Series?.Name + (Series?.Renderer.Container is not null && Series.Renderer.Container.IsTrendLine ? ",Trendline " : ",") + Series?.Type + " series with " + points.Count + " data points";
        }

        /// <summary>
        /// Generates the formatted accessibility description text for the specified data point.
        /// </summary>
        /// <param name="point">The data point for which to generate the accessibility description. Cannot be null.</param>
        /// <returns>A string containing the accessibility description for the given data point. Returns a formatted string based
        /// on the series template if available; otherwise, returns a default description.</returns>
        internal string GetPointDescriptionFormatText(Point point)
        {
            return Series is not null && Series.AccessibilityDescriptionFormat is not null ? ParseTemplate(point, Series.AccessibilityDescriptionFormat) : CalCulateAccessibilityText(point);
        }

        /// <summary>
        /// Parses the specified template string and replaces placeholders with property values from the provided point
        /// and its associated series.
        /// </summary>
        /// <param name="point">The point whose property values are used to substitute corresponding placeholders in the template.</param>
        /// <param name="format">The template string containing placeholders to be replaced with values from the point and series properties.</param>
        /// <returns>A string with all recognized placeholders replaced by their corresponding values from the point and series.</returns>
        internal string ParseTemplate(Point point, string format)
        {
            PropertyInfo[] pointInfo = point.GetType().GetProperties();
            PropertyInfo[] seriesInfo = Series?.GetType().GetProperties() ?? null!;
            foreach (PropertyInfo dataValue in pointInfo)
            {
                Regex val = new("${point" + '.' + dataValue.Name + '}', RegexOptions.Multiline);
                string textValue = Convert.ToString(dataValue.GetValue(point), CultureInfo.InvariantCulture) ?? null!;
                textValue = GetTextValue(val.ToString(), point, textValue);
                format = format.Replace(val.ToString(), textValue, StringComparison.OrdinalIgnoreCase);
                if (format.Contains("separatorY", StringComparison.OrdinalIgnoreCase))
                {
                    format = format.Replace("${point.separatorY}", string.Format(CultureInfo.InvariantCulture, "{0:N0}", point.Y), StringComparison.OrdinalIgnoreCase);
                }
            }

            foreach (PropertyInfo dataValue in seriesInfo)
            {
                Regex val = new("${series." + dataValue.Name + '}', RegexOptions.Multiline);
                string textValue = Convert.ToString(dataValue.GetValue(Series), null) ?? null!;
                format = format.Replace(val.ToString(), textValue, StringComparison.OrdinalIgnoreCase);
            }

            return format;
        }

        /// <summary>
        /// Retrieves a formatted text value based on the specified placeholder and point data.
        /// </summary>
        /// <param name="regexValue">The placeholder string that determines which value to extract and format from the point. Supported values
        /// include "${point.x}", "${point.y}", "${point.high}", "${point.low}", "${point.open}", "${point.close}", and
        /// "${point.volume}".</param>
        /// <param name="point">The point object containing the data to be formatted. Must be a valid instance of Point or FinancialPoint,
        /// depending on the placeholder used.</param>
        /// <param name="text">The default text to return if the placeholder does not match any supported value.</param>
        /// <returns>A formatted string representing the requested value from the point, or the default text if the placeholder
        /// is not recognized.</returns>
        internal virtual string GetTextValue(string regexValue, Point point, string text)
        {
            FinancialPoint financialPoint = point as FinancialPoint ?? null!;
            switch (regexValue)
            {
                case "${point.x}":
                    return Convert.ToString(XAxisRenderer.GetFormatText(GetPointXValue(point.X, XAxisRenderer.DateFormat ?? string.Empty)), Culture) ?? string.Empty;
                case "${point.y}":
                    return Convert.ToString(YAxisRenderer.GetFormatText(GetPointXValue(point.Y, YAxisRenderer.DateFormat ?? string.Empty)), Culture) ?? string.Empty;
                case "${point.high}":
                    return Convert.ToString(YAxisRenderer.GetFormatText(GetPointXValue(financialPoint.High, YAxisRenderer.DateFormat ?? string.Empty)), Culture) ?? string.Empty;
                case "${point.low}":
                    return Convert.ToString(YAxisRenderer.GetFormatText(GetPointXValue(financialPoint.Low, YAxisRenderer.DateFormat ?? string.Empty)), Culture) ?? string.Empty;
                case "${point.open}":
                    return Convert.ToString(YAxisRenderer.GetFormatText(GetPointXValue(financialPoint.Open, YAxisRenderer.DateFormat ?? string.Empty)), Culture) ?? string.Empty;
                case "${point.close}":
                    return Convert.ToString(YAxisRenderer.GetFormatText(GetPointXValue(financialPoint.Close, YAxisRenderer.DateFormat ?? string.Empty)), Culture) ?? string.Empty;
                case "${point.volume}":
                    return Convert.ToString(YAxisRenderer.GetFormatText(GetPointXValue(financialPoint.Volume, YAxisRenderer.DateFormat ?? string.Empty)), Culture) ?? string.Empty;
                default:
                    break;
            }
            return text;
        }

        /// <summary>
        /// Renders the SVG clip path for the marker using the specified render tree builder.
        /// </summary>
        /// <param name="builder">The render tree builder used to construct the SVG elements for the marker clip path.</param>
        internal virtual void RenderMarkerClipPath(RenderTreeBuilder builder)
        {
            // Need to implemented rect animation
            string visibility = ShouldAnimate() ? "hidden" : "visible";
            if (Owner?._svgRenderer is not null)
            {
                Owner._svgRenderer.OpenGroupElement(builder, SeriesSymbolId(), "translate(" + ClipRect?.X.ToString(Culture) + "," + ClipRect?.Y.ToString(Culture) + ")", MarkerClipPath(), string.Empty, string.Empty, string.Empty, "true");
                Owner._svgRenderer.RenderClipPath(builder, MarkerClipPathID(), _markerClipRect ?? new Rect(0, 0, 0, 0), visibility);
            }
        }

        /// <summary>
        /// Returns the default category for the series.
        /// </summary>
        /// <returns>A <see cref="SeriesCategories"/> value representing the default category, typically <see
        /// cref="SeriesCategories.Series"/>.</returns>
        internal virtual SeriesCategories Category()
        {
            return SeriesCategories.Series;
        }

        /// <summary>
        /// Updates the category data for the chart by processing series renderers and recalculating point totals.
        /// </summary>
        internal void UpdateCategoryData()
        {
            XAxisRenderer.Labels.Clear();
            if (Owner?._seriesContainer is not null)
            {
                foreach (ChartSeriesRenderer renderer in Owner._seriesContainer.Renderers.Cast<ChartSeriesRenderer>())
                {
                    if (renderer.Series is not null && renderer.Series.Visible)
                    {
                        renderer.InitSeriesRendererFields();
                        renderer.ProcessData();
                    }
                }

                foreach (IChartElementRenderer renderer in Owner._seriesContainer.Renderers)
                {
                    ChartSeriesRenderer seriesRenderer = renderer as ChartSeriesRenderer ?? null!;
                    if (seriesRenderer.Points is not null)
                    {
                        foreach (Point point in seriesRenderer.Points.ToArray())
                        {
                            _ = Owner._seriesContainer._total.TryGetValue(seriesRenderer.FindSeriesAxisKey() + point.X, out double total);
                            if (seriesRenderer.ChartPoints is not null)
                            {
                                point.SumOfSameIndex = seriesRenderer.ChartPoints[point.Index].SumOfSameIndex = total;
                            }
                        }
                    }
                }
                Owner._seriesContainer.Sorting();
                Owner._isSeriesRendered = Owner._seriesContainer.Renderers.Count > 0;
            }
        }

        /// <summary>
        /// Processes the chart data for the specified point and appends the serialized result to the chart data
        /// collection.
        /// </summary>
        /// <param name="point">The point for which chart data is to be processed. The <paramref name="point"/> parameter must specify a
        /// valid index within the chart points collection.</param>
        /// <exception cref="InvalidOperationException">Thrown if an error occurs while processing or serializing the chart data for the specified point.</exception>
        internal virtual void GetChartData(Point point)
        {
            try
            {
                _ = ChartData?.Append(JsonSerializer.Serialize(Series?.Renderer.ChartPoints?[point.Index], _jsonOptions));
                _ = ChartData?.Append(',');
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("An error occurred while processing the chart data.", exception);
            }
        }

        /// <summary>
        /// Appends a serialized chart data point to the internal chart data buffer.
        /// </summary>
        /// <typeparam name="T">The type of the chart data point to serialize and append. Must be compatible with JSON serialization.</typeparam>
        /// <param name="ChartPoint">The chart data point to serialize and append to the chart data buffer. Cannot be null.</param>
        /// <exception cref="InvalidOperationException">Thrown if an error occurs during serialization or appending of the chart data point.</exception>
        internal void AppendChartData<T>(T ChartPoint)
        {
            try
            {
                _ = ChartData?.Append(JsonSerializer.Serialize(ChartPoint, _jsonOptions));
                _ = ChartData?.Append(',');
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("An error occurred while processing the chart data.", exception);
            }
        }

        /// <summary>
        /// Sets the default values for the renderer, initializing container settings and chart size as required.
        /// </summary>
        internal override void SetDefaultRendererValues()
        {
            Container?.SetDefaultRendererContainerValues();
            HandleChartSizeChange(Owner?.InitialRect ?? null!);
        }

        /// <summary>
        /// Retrieves the data point value for the specified coordinates and optional rectangle list.
        /// </summary>
        /// <param name="x">The X-coordinate used to determine the data point value. Defaults to 0 if not specified.</param>
        /// <param name="y">The Y-coordinate used to determine the data point value. Defaults to 0 if not specified.</param>
        /// <param name="rectY">A list of rectangles whose Y values may influence the data point calculation. If null, an empty list is
        /// used.</param>
        /// <returns>A string representing the calculated data point value based on the provided coordinates and rectangle list.</returns>
        internal string GetDataPoints(double x = 0, double y = 0, List<Rect> rectY = null!)
        {
            double rectYvalue = 0;
            rectY ??= [];
            if (rectY.Count > 0)
            {
                for (int i = 0; i < rectY.Count; i++)
                {
                    rectYvalue = rectY[i].Y;
                }

                return GetDataPointValue(x, y, rectYvalue);
            }
            else
            {
                return GetDataPointValue(x, y);
            }
        }

        /// <summary>
        /// Generates a comma-separated string representing the value and metadata of a data point based on the
        /// specified coordinates and related chart properties.
        /// </summary>
        /// <param name="x">The X-coordinate of the data point. Defaults to 0 if not specified.</param>
        /// <param name="y">The Y-coordinate of the data point. Defaults to 0 if not specified.</param>
        /// <param name="rectYvalue">The Y-value associated with the rectangle representation of the data point. Defaults to 0 if not specified.</param>
        /// <returns>A comma-separated string containing the formatted values and metadata for the data point, including
        /// coordinates, styles, series information, and other chart-related properties.</returns>
        internal string GetDataPointValue(double x = 0, double y = 0, double rectYvalue = 0)
        {
            return string.Join(",", x.ToString(Culture), y.ToString(Culture), Interior, Series?.Type, Index, Series?.NonHighlightStyle, Series?.SelectionStyle, Series?.UnSelectedStyle, Series?.Visible, rectYvalue.ToString(Culture), Category(), SeriesType(), GetMarkerShape(Series?.Marker ?? null!),
                   !string.IsNullOrEmpty(Series?.TooltipFormat) ? Series.TooltipFormat : string.Empty, "/" + Series?.Name + "/", Series?.EnableTooltip, Series?.ChartDataEditSettings?.Enable, Owner?.IsTransposed, Series?.Opacity.ToString(Culture), Series?.XAxisName, Series?.YAxisName, Series?.Volume, Series?.Marker?.DataLabel?.Format, XMin.ToString(Culture), XMax.ToString(Culture), Series?.EmptyPointSettings?.Mode, Series?.Focusable, Series?.ShowNearestTooltip, Series?.Width);
        }

        /// <summary>
        /// Determines the shape to use for the specified chart marker based on its visibility and configuration.
        /// </summary>
        /// <param name="marker">The chart marker for which to determine the shape. If the marker is not visible, a default shape is
        /// returned.</param>
        /// <returns>The shape to use for the marker. Returns the marker's configured shape if specified; otherwise, returns a
        /// shape based on the series index. If the marker is not visible, returns a default shape.</returns>
        internal ChartShape GetMarkerShape(ChartMarker marker)
        {
            if (marker == null)
            {
                return ChartShape.Circle;
            }

            int rendererIndex = Series?.Renderer is not null && Series.Renderer is ParetoLineSeriesRenderer ? Index - 1 : Index;
            return marker.Visible ? (marker.Shape != ChartShape.Auto ? marker.Shape : (ChartShape)(rendererIndex % Constants.ChartMarkerCount)) : ChartShape.Circle;
        }

        /// <summary>
        /// Determines the fill style for a chart series based on its gradient settings or returns a default fill value.
        /// </summary>
        /// <param name="series">The chart series for which to determine the fill style. May contain linear or radial gradient information.</param>
        /// <param name="defaultFill">The default fill value to use if the series does not specify a valid gradient. If null, an empty string is
        /// returned.</param>
        /// <returns>A string representing the fill style for the series. Returns a URL reference to the gradient if defined;
        /// otherwise, returns the specified default fill or an empty string.</returns>
        internal static string GetSeriesFill(ChartSeries series, string defaultFill)
        {
            return series?.LinearGradient?.GradientColorStops?.Count > 0 && !string.IsNullOrEmpty(series.LinearGradient.ID)
                ? $"url(#{series.LinearGradient.ID})"
                : series?.RadialGradient?.GradientColorStops?.Count > 0 && !string.IsNullOrEmpty(series.RadialGradient.ID)
                ? $"url(#{series.RadialGradient.ID})"
                : defaultFill ?? string.Empty;
        }

        /// <summary>
        /// Determines the fill style for a trendline based on its gradient settings or returns a default stroke value.
        /// </summary>
        /// <param name="trendline">The trendline whose fill style is to be determined. May contain linear or radial gradient information.</param>
        /// <param name="defaultStroke">The default stroke value to use if the trendline does not specify a valid gradient fill. If null, an empty
        /// string is returned.</param>
        /// <returns>A string representing the fill style for the trendline. Returns a URL reference to the gradient if defined;
        /// otherwise, returns the default stroke value or an empty string.</returns>
        internal static string GetTrendlineFill(ChartTrendline trendline, string defaultStroke)
        {
            return trendline?.LinearGradient?.GradientColorStops?.Count > 0 && !string.IsNullOrEmpty(trendline.LinearGradient.ID)
                ? $"url(#{trendline.LinearGradient.ID})"
                : trendline?.RadialGradient?.GradientColorStops?.Count > 0 && !string.IsNullOrEmpty(trendline.RadialGradient.ID)
                ? $"url(#{trendline.RadialGradient.ID})"
                : defaultStroke ?? string.Empty;
        }

        /// <summary>
        /// Determines whether the specified point lies within the currently visible range of both the X and Y axes.
        /// </summary>
        /// <param name="currentPoint">The point to evaluate, containing X and Y values to be checked against the visible axis ranges.</param>
        /// <returns>true if the point is within the visible range of both axes; otherwise, false.</returns>
        internal bool IsPointWithInRange(Point currentPoint)
        {
            if (XAxisRenderer.Axis?.ZoomFactor == 1 && YAxisRenderer.Axis?.ZoomFactor == 1 &&
                XAxisRenderer.Axis.ZoomPosition == 0 && YAxisRenderer.Axis.ZoomPosition == 0)
            {
                return true;
            }

            double pointXValue = XAxisRenderer is LogarithmicAxisRenderer ? XAxisRenderer.GetPointValue(currentPoint.XValue) : currentPoint.XValue;
            double pointYValue = YAxisRenderer is LogarithmicAxisRenderer ? YAxisRenderer.GetPointValue(currentPoint.YValue) : currentPoint.YValue;

            return pointXValue >= XAxisRenderer.VisibleRange.Start && pointXValue <= XAxisRenderer.VisibleRange.End &&
                   pointYValue >= YAxisRenderer.VisibleRange.Start && pointYValue <= YAxisRenderer.VisibleRange.End;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes the series with axis assignments.
        /// </summary>
        public void OnAxisChanged()
        {
        }

        /// <summary>
        /// Handles chart size changes and triggers re-rendering.
        /// </summary>
        /// <param name="rect">The new chart rectangle dimensions.</param>
        public override void HandleChartSizeChange(Rect rect)
        {
            RendererShouldRender = true;
            SeriesRenderer();
            Series?.Marker?.Renderer?.HandleChartSizeChange(rect);
            Series?.Marker?.DataLabel?.Renderer?.HandleChartSizeChange(rect);
            Series?.LastDataLabel?.Renderer?.HandleChartSizeChange(rect);
            Series?.LinearGradient?.Renderer?.HandleChartSizeChange(rect);
            Series?.RadialGradient?.Renderer?.HandleChartSizeChange(rect);
            CalculateMarkerHeightAndPadding();
        }

        /// <summary>
        /// Gets the ID for the error bar group element.
        /// </summary>
        public string ErrorBarGroupID()
        {
            return Owner?.ID + "ErrorBarGroup" + Index;
        }

        /// <summary>
        /// Sets the fill color for a point, applying point color mapping or empty point settings as needed.
        /// </summary>
        /// <param name="point">The data point.</param>
        /// <param name="color">The default fill color.</param>
        /// <returns>The computed fill color.</returns>
        public virtual string SetPointColor(Point point, string color)
        {
            color = !string.IsNullOrEmpty(point?.Interior) && !string.IsNullOrEmpty(Series?.PointColorMapping) ? point.Interior : color;
            return point is not null && point.IsEmpty ? (!string.IsNullOrEmpty(Series?.EmptyPointSettings.Fill) ? Series.EmptyPointSettings.Fill : color) : color;
        }

        /// <summary>
        /// Processes the render queue for the series and its associated visual elements, ensuring that all components
        /// are prepared for rendering.
        /// </summary>
        public override void ProcessRenderQueue()
        {
            base.ProcessRenderQueue();
            Series?.Marker?.Renderer?.ProcessRenderQueue();
            Series?.Marker?.DataLabel?.Renderer?.ProcessRenderQueue();
            Series?.LastDataLabel?.Renderer?.ProcessRenderQueue();
            Series?.LinearGradient?.Renderer?.ProcessRenderQueue();
            Series?.RadialGradient?.Renderer?.ProcessRenderQueue();
        }
        #endregion
    }

    /// <summary>
    /// Provides the default rendering implementation for chart series within the charting framework.
    /// </summary>
    internal class DefaultSeriesRenderer : ChartSeriesRenderer
    {
    }

    /// <summary>
    /// custom JsonConverter type to serialize the DateOnly data type.
    /// </summary>
    internal class DateOnlyConverter : JsonConverter<DateOnly>
    {
        private const string DateFormat = "yyyy-MM-dd";

        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateOnly.ParseExact(reader.GetString()!, DateFormat, CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(DateFormat, CultureInfo.InvariantCulture));
        }
    }

    /// <summary>
    /// custom JsonConverter type to serialize the TimeOnly data type.
    /// </summary>
    internal class TimeOnlyConverter : JsonConverter<TimeOnly>
    {
        private const string TimeFormat = "HH:mm:ss.FFFFFFF";

        public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return TimeOnly.ParseExact(reader.GetString()!, TimeFormat, CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(TimeFormat, CultureInfo.InvariantCulture));
        }
    }
}
