using Microsoft.AspNetCore.Components.Rendering;
using System.Globalization;
using Syncfusion.Blazor.Toolkit.Data;
using System.Text.RegularExpressions;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Container responsible for series renderers lifecycle, data processing and stacking calculations.
    /// </summary>
    public class ChartSeriesRendererContainer : ChartRendererContainer
    {
        #region Constants
        /// <summary>
        /// Separator characters used to split date/time format strings.
        /// </summary>
        private static readonly char[] _separator = [',', '.', '-', '\\', '/', '_', ' ', ':'];
        #endregion

        #region Fields
        private int _paretoLineSeriesRendererCount;
        private int _paretoLineSeriesRendererIndex;

        private List<string> _seriesType = [];
        private List<string> _drawTypes = [];
        private ChartSeries ParetoSeries { get; set; } = null!;
        private ChartSeries DefaultSeries { get; set; } = null!;

        // To identify series collection having large data
        internal bool _hasLargeData;
        internal bool _isBackSymbol;
        internal string[]? _palette;
        internal DateTime _previousRequestTime = DateTime.MinValue;
        internal CultureInfo _culture = CultureInfo.InvariantCulture;
        internal Dictionary<string, string> _dateValuePairs = [];
        internal Dictionary<string, string> _numberValuePairs = [];
        internal List<Rect> _dataLabelCollection = [];
        internal Dictionary<string, double> _total = [];
        internal Dictionary<string, int> _seriesIndexes = [];
        // Collects renderers that require axis information.
        internal List<IRequireAxis> _elementsRequiredAxis = [];
        internal IEnumerable<object> _data = [];

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Initializes component and registers the container with the owner/Chart.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            AddToRenderQueue(this);
            if (Owner is { })
            {
                Owner._seriesContainer = this;
            }
            _palette = Owner?.Palettes.Length > 0 ? Owner.Palettes : ChartHelper.GetSeriesColor(Owner?.Theme.ToString() ?? string.Empty);
        }

        /// <summary>
        /// Ensures owner reference is kept in sync after parameters change.
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (Owner is { })
            {
                Owner._seriesContainer = this;
            }
        }

        /// <summary>
        /// Disposes unmanaged or referenced resources.
        /// </summary>
        protected override ValueTask DisposeAsyncCore()
        {
            ParetoSeries = null!;
            DefaultSeries = null!;
            return base.DisposeAsyncCore();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Adds a series renderer to the owner's visible series collection based on explicit index mapping or default ordering.
        /// </summary>
        private void AddVisibleSeriesRenderers(ChartSeriesRenderer chartSeriesRenderer)
        {
            string name = chartSeriesRenderer.Series?.Name ?? string.Empty;
            if (Owner is not null && !string.IsNullOrEmpty(name) && _seriesIndexes.TryGetValue(chartSeriesRenderer.Series?.GenerateSeriesKey() ?? null!, out int index))
            {
                if (Owner._visibleSeriesRenderers.Count > index)
                {
                    Owner._visibleSeriesRenderers.Insert(index, chartSeriesRenderer);
                }
                else
                {
                    Owner._visibleSeriesRenderers.Add(chartSeriesRenderer);
                }
                Owner._visibleSeriesRenderers = [.. Owner._visibleSeriesRenderers.OrderBy(x => Elements.IndexOf(x.Series ?? null!))];
            }
            else
            {
                Owner?._visibleSeriesRenderers.Add(chartSeriesRenderer);
            }
        }

        /// <summary>
        /// Sorts the points of a series renderer based on the owner's sorting settings and the X-axis value type, ensuring that both the Points and ChartPoints collections are ordered consistently for rendering and data processing.
        /// </summary>
        private List<Point> SortedPoints(ChartSeriesRenderer seriesRenderer)
        {
            List<Point>? sortedPoints = [];

            if (seriesRenderer.XAxisRenderer.Axis?.ValueType == ValueType.DateTimeCategory)
            {
                if (Owner?._sorting.Direction == ListSortDirection.Descending)
                {
                    sortedPoints = (!Owner._sorting.PropertyName.Equals("X", StringComparison.OrdinalIgnoreCase)) ? seriesRenderer.Points?.OrderByDescending(y => y.SumOfSameIndex).ToList() : seriesRenderer.Points?.OrderByDescending(y => ChartHelper.GetTime(Convert.ToDateTime(y.X, CultureInfo.InvariantCulture))).ToList();
                    seriesRenderer.ChartPoints = (!Owner._sorting.PropertyName.Equals("X", StringComparison.OrdinalIgnoreCase)) ? seriesRenderer.ChartPoints?.OrderByDescending(y => y.SumOfSameIndex).ToList() : seriesRenderer.ChartPoints?.OrderByDescending(y => ChartHelper.GetTime(Convert.ToDateTime(y.X, CultureInfo.InvariantCulture))).ToList();
                }
                else
                {
                    sortedPoints = (Owner is not null && !Owner._sorting.PropertyName.Equals("X", StringComparison.OrdinalIgnoreCase)) ? seriesRenderer.Points?.OrderBy(y => y.SumOfSameIndex).ToList() : seriesRenderer.Points?.OrderBy(y => ChartHelper.GetTime(Convert.ToDateTime(y.X, CultureInfo.InvariantCulture))).ToList();
                    seriesRenderer.ChartPoints = (Owner is not null && !Owner._sorting.PropertyName.Equals("X", StringComparison.OrdinalIgnoreCase)) ? seriesRenderer.ChartPoints?.OrderBy(y => y.SumOfSameIndex).ToList() : seriesRenderer.ChartPoints?.OrderBy(y => ChartHelper.GetTime(Convert.ToDateTime(y.X, CultureInfo.InvariantCulture))).ToList();
                }
            }
            else
            {
                if (Owner?._sorting.Direction == ListSortDirection.Descending)
                {
                    sortedPoints = (!Owner._sorting.PropertyName.Equals("X", StringComparison.OrdinalIgnoreCase)) ? seriesRenderer.Points?.OrderByDescending(y => y.SumOfSameIndex).ToList() : seriesRenderer.Points?.OrderByDescending(y => y.X.ToString()).ToList();
                    seriesRenderer.ChartPoints = (!Owner._sorting.PropertyName.Equals("X", StringComparison.OrdinalIgnoreCase)) ? seriesRenderer.ChartPoints?.OrderByDescending(y => y.SumOfSameIndex).ToList() : seriesRenderer.ChartPoints?.OrderByDescending(y => y.X.ToString()).ToList();
                }
                else
                {
                    sortedPoints = (Owner is not null && !Owner._sorting.PropertyName.Equals("X", StringComparison.OrdinalIgnoreCase)) ? seriesRenderer.Points?.OrderBy(y => y.SumOfSameIndex).ToList() : seriesRenderer.Points?.OrderBy(y => y.X.ToString()).ToList();
                    seriesRenderer.ChartPoints = (Owner is not null && !Owner._sorting.PropertyName.Equals("X", StringComparison.OrdinalIgnoreCase)) ? seriesRenderer.ChartPoints?.OrderBy(y => y.SumOfSameIndex).ToList() : seriesRenderer.ChartPoints?.OrderBy(y => y.X.ToString()).ToList();
                }
            }

            return sortedPoints ?? null!;
        }

        /// <summary>
        /// For each series in the stacking collection, calculates the percentage contribution of each point to the total stack value at that X position, and updates the Point's Percentage property accordingly. If isStacking100 is true, it assumes values are already normalized to 100% and skips recalculation.
        /// </summary>
        private static void FindPercentageOfStacking(List<ChartSeries> stackingSeries, List<double> values, bool isStacking100)
        {
            foreach (ChartSeries item in stackingSeries)
            {
                if (isStacking100)
                {
                    return;
                }

                if (item.Renderer.Points is not null)
                {
                    foreach (Point point in ChartHelper.GetVisiblePoints(item.Renderer.Points))
                    {
                        if (item.Renderer.ChartPoints is not null)
                        {
                            point.Percentage = item.Renderer.ChartPoints[point.Index].Percentage = Convert.ToDouble(Math.Abs(Convert.ToDouble(point.Y, null) / values[point.Index] * 100).ToString("N2", null), null);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Calculates the stacking values for each series in the collection, updating their Renderer.StackedValues and Renderer.StackedPointValues properties based on the cumulative sums of positive and negative values at each X position. If isStacking100 is true, it normalizes values to percentages based on total frequencies. It also updates the YMin and YMax properties of each series renderer to ensure proper axis scaling for stacked charts.
        /// </summary>
        private static void CalculateStackingValues(List<ChartSeries> seriesCollection, bool isStacking100)
        {
            Dictionary<string, Dictionary<double, double>> lastPositive = [];
            Dictionary<string, Dictionary<double, double>> lastNegative = [];
            Dictionary<string, Dictionary<double, double>> frequencies = isStacking100 ? FindFrequencies(seriesCollection) : [];

            List<ChartSeries> stackingSeries = [];
            List<double> stackedValues = [];

            foreach (ChartSeries series in seriesCollection)
            {
                if (series.SeriesType is not null && series.SeriesType.Contains("Stacking", StringComparison.InvariantCulture))
                {
                    series.Renderer.StackedPointValues = [];
                    string stackingGroup = GetStackingGroup(series);

                    EnsureStackingDictionaries(lastPositive, lastNegative, stackingGroup);

                    stackingSeries.Add(series);
                    List<Point> visiblePoints = ChartHelper.GetVisiblePoints(series.Renderer.Points ?? null!);

                    ProcessSeriesStacking(series, isStacking100, lastPositive, lastNegative, frequencies, stackedValues, visiblePoints);
                }
            }

            FindPercentageOfStacking(stackingSeries, stackedValues, isStacking100);
        }

        /// <summary>
        /// Ensures that the lastPositive and lastNegative dictionaries have entries for the given stacking group, initializing them if they do not exist. This method checks if the stacking group key is present in each dictionary, and if not, it adds a new entry with a null value. It then initializes the nested dictionary for that stacking group if it was previously null, ensuring that both lastPositive and lastNegative have valid dictionaries to store cumulative sums for the stacking calculations.
        /// </summary>
        private static void EnsureStackingDictionaries(Dictionary<string, Dictionary<double, double>> lastPositive, Dictionary<string, Dictionary<double, double>> lastNegative, string stackingGroup)
        {
            if (!lastPositive.TryGetValue(stackingGroup, out Dictionary<double, double>? _))
            {
                lastPositive.Add(stackingGroup, null!);
            }

            if (!lastNegative.ContainsKey(stackingGroup))
            {
                lastNegative.Add(stackingGroup, null!);
            }

            if (lastPositive[stackingGroup] is null)
            {
                lastPositive[stackingGroup] = [];
                lastNegative[stackingGroup] = [];
            }
        }

        /// <summary>
        /// Processes the stacking calculations for a single series, updating its stacked values based on the cumulative sums of positive and negative values at each X position. It iterates through the visible points of the series, calculating the new start and end values for stacking based on the last positive and negative sums for that stacking group and X position. If isStacking100 is true, it normalizes the Y value to a percentage based on the total frequency for that stacking group and X position. It also updates the series renderer's YMin and YMax properties to ensure proper axis scaling for stacked charts.
        /// </summary>
        private static void ProcessSeriesStacking(ChartSeries series, bool isStacking100, Dictionary<string, Dictionary<double, double>> lastPositive, Dictionary<string, Dictionary<double, double>> lastNegative, Dictionary<string, Dictionary<double, double>> frequencies, List<double> stackedValues, List<Point> visiblePoints)
        {
            string stackingGroup = GetStackingGroup(series);
            List<double> startValues = [];
            List<double> endValues = [];

            for (int j = 0, pointsLength = visiblePoints.Count; j < pointsLength; j++)
            {
                double lastValue,
                y_Value = (series.Container?._sorting.PropertyName is not null) ? series.Renderer.Points?[j].YValue ?? 0 : double.IsNaN(series.Renderer.YData[j]) ? 0 : series.Renderer.YData[j],
                pos = visiblePoints[j].XValue;
                if (!lastPositive[stackingGroup].TryGetValue(pos, out double _))
                {
                    lastPositive[stackingGroup].Add(pos, 0);
                }

                if (!lastNegative[stackingGroup].TryGetValue(pos, out double _))
                {
                    lastNegative[stackingGroup].Add(pos, 0);
                }

                if (isStacking100)
                {
                    y_Value = y_Value / frequencies[stackingGroup][pos] * 100;
                    y_Value = !double.IsNaN(y_Value) ? y_Value : 0;
                    if (series.Renderer.ChartPoints is not null)
                    {
                        visiblePoints[j].Percentage = series.Renderer.ChartPoints[j].Percentage = Convert.ToDouble(y_Value.ToString("N2", null), null);
                    }
                }
                else
                {
                    if (stackedValues.Count == j)
                    {
                        stackedValues.Add(Math.Abs(y_Value));
                    }
                    else
                    {
                        stackedValues[j] = stackedValues[j] + Math.Abs(y_Value);
                    }
                }

                if (y_Value >= 0)
                {
                    lastValue = lastPositive[stackingGroup][pos];
                    lastPositive[stackingGroup][pos] += y_Value;
                }
                else
                {
                    lastValue = lastNegative[stackingGroup][pos];
                    lastNegative[stackingGroup][pos] += y_Value;
                }

                startValues.Add(lastValue);
                endValues.Add(y_Value + lastValue);
                if (isStacking100 && (endValues[j] > 100))
                {
                    endValues[j] = 100;
                }
            }

            series.Renderer.StackedValues = new StackValues(startValues, endValues);
            series.Renderer.StackedPointValues = lastPositive[stackingGroup];

            double startMin = startValues.Count > 0 ? startValues.Min() : 0;
            double startMax = startValues.Count > 0 ? startValues.Max() : 0;
            double endMin = endValues.Count > 0 ? endValues.Min() : 0;
            double endMax = endValues.Count > 0 ? endValues.Max() : 0;

            series.Renderer.YMin = startMin;
            series.Renderer.YMax = endMax;
            if (series.Renderer.YMin > endMin)
            {
                series.Renderer.YMin = isStacking100 ? -100 : endMin;
            }

            if (series.Renderer.YMax < startMax)
            {
                series.Renderer.YMax = 0;
            }
        }

        /// <summary>
        /// For each stacking group, calculates the total frequency (cumulative sum of values) at each X position across all series in the group, and returns a nested dictionary mapping stacking group names to their respective X value frequencies. This is used for calculating percentages in 100% stacked charts.
        /// </summary>
        private static Dictionary<string, Dictionary<double, double>> FindFrequencies(List<ChartSeries> seriesCollection)
        {
            Dictionary<string, Dictionary<double, double>> frequencies = [];
            foreach (ChartSeries series in seriesCollection)
            {
                series.Renderer.YAxisRenderer.IsStack100 = series.SeriesType is not null && series.SeriesType.Contains("100", StringComparison.InvariantCulture);
                List<Point> visiblePoints = ChartHelper.GetVisiblePoints(series.Renderer.Points ?? null!);
                if (series.SeriesType is not null && series.SeriesType.Contains("Stacking", StringComparison.InvariantCulture))
                {
                    string stackingGroup = GetStackingGroup(series);
                    if (!frequencies.TryGetValue(stackingGroup, out Dictionary<double, double>? _))
                    {
                        frequencies.Add(stackingGroup, null!);
                    }

                    if (frequencies[stackingGroup] is null)
                    {
                        frequencies[stackingGroup] = [];
                    }

                    for (int j = 0; j < visiblePoints.Count; j++)
                    {
                        double xVal = visiblePoints[j].XValue;
                        if (!frequencies[stackingGroup].ContainsKey(xVal))
                        {
                            frequencies[stackingGroup].Add(xVal, 0);
                        }

                        if (!string.IsNullOrEmpty(series.Container?._sorting.PropertyName))
                        {
                            frequencies[stackingGroup][xVal] += (Convert.ToDouble(series.Renderer.Points?[j].Y, CultureInfo.InvariantCulture) > 0 ? 1 : -1) * Convert.ToDouble(series.Renderer.Points?[j].Y, CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            frequencies[stackingGroup][xVal] += (series.Renderer.YData[j] > 0 ? 1 : -1) * series.Renderer.YData[j];
                        }
                    }
                }
            }

            return frequencies;
        }

        /// <summary>
        /// Creates the main series renderer component for a given chart element, assigning it a unique key and renderer index based on its position in the Elements collection and any special handling for Pareto line series. This method is responsible for instantiating the appropriate renderer type for each series element and ensuring it is properly keyed for Blazor's rendering system to track component instances across updates.
        /// </summary>
        private void CreateSeriesElements(RenderTreeBuilder builder, IChartElement element)
        {
            int seq = 0;
            builder.OpenComponent(seq++, element.RendererType);
            builder.SetKey(element.RendererKey + "_Renderer");
            int seriesIndex = (_paretoLineSeriesRendererIndex > 0 ? _paretoLineSeriesRendererIndex : 0) + Elements.IndexOf(element);
            builder.AddAttribute(seq++, "RendererIndex", seriesIndex);
            builder.CloseComponent();
        }

        /// <summary>
        /// Creates nested renderer components for a given series element, such as marker renderer, data label renderer, error bar renderer, and gradient renderers, based on the presence of their respective RendererType properties. Each nested renderer is also assigned a unique key for Blazor's rendering system. This method ensures that all auxiliary renderers associated with a series are instantiated and linked to the series for proper rendering of markers, labels, error bars, and gradients as needed.
        /// </summary>
        private static void CreateSeriesNestedElements(RenderTreeBuilder builder, ChartSeries element)
        {
            int seq = 0;
            ChartSeries series = element;
            if (series.Marker?.RendererType is not null)
            {
                builder.OpenComponent(seq++, series.Marker.RendererType);
                builder.AddAttribute(seq++, "Series", series);
                builder.SetKey(element.RendererKey + "_MarkerRenderer");
                builder.CloseComponent();

                if (series.Marker.DataLabel?.RendererType is not null)
                {
                    builder.OpenComponent(seq++, series.Marker.DataLabel.RendererType);
                    builder.AddAttribute(seq++, "Series", series);
                    builder.SetKey(element.RendererKey + "_LabelRenderer");
                    builder.CloseComponent();
                }
            }

            if (series.LastDataLabel?.RendererType is not null)
            {
                builder.OpenComponent(seq++, series.LastDataLabel.RendererType);
                builder.AddAttribute(seq++, "Series", series);
                builder.SetKey(element.RendererKey + "_LastDataLabelRenderer");
                builder.CloseComponent();
            }

            if (series.LinearGradient?.RendererType is not null)
            {
                builder.OpenComponent(seq++, series.LinearGradient.RendererType);
                builder.AddAttribute(seq++, "Series", series);
                builder.SetKey(element.RendererKey + "_LinearGradientRenderer");
                builder.CloseComponent();
            }

            if (series.RadialGradient?.RendererType is not null)
            {
                builder.OpenComponent(seq++, series.RadialGradient.RendererType);
                builder.AddAttribute(seq++, "Series", series);
                builder.SetKey(element.RendererKey + "_RadialGradientRenderer");
                builder.CloseComponent();
            }
        }

        /// <summary>
        /// Updates the SumOfSameIndex property for each point across all series, based on the total values calculated for each X value.
        /// </summary>
        private void UpdateSumOfSameIndex()
        {
            foreach (IChartElementRenderer renderer in Renderers)
            {
                ChartSeriesRenderer? seriesRenderer = renderer as ChartSeriesRenderer;
                if (seriesRenderer?.Points is not null)
                {
                    foreach (Point point in seriesRenderer.Points.ToArray())
                    {
                        _ = _total.TryGetValue(seriesRenderer.FindSeriesAxisKey() + point.X, out double total);
                        if (seriesRenderer.ChartPoints is not null)
                        {
                            point.SumOfSameIndex = seriesRenderer.ChartPoints[point.Index].SumOfSameIndex = total;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Determines the appropriate date/time format string for a date axis based on the axis's Format property, the associated series renderer's X-axis date format, and the current culture's date/time patterns. It checks for specific format strings ("d" for short date, "T" for long time) and falls back to defaults as needed to ensure that date values are formatted correctly on the axis labels.
        /// </summary>
        private static string DetermineDateAxisFormat(ChartAxis axis, ChartSeriesRenderer seriesRenderer, CultureInfo currentCulture)
        {
            string axisFormat = axis.Format;
            if (axis.ValueType == ValueType.DateTime)
            {
                axisFormat = !string.IsNullOrEmpty(axisFormat) ? axisFormat : seriesRenderer.XAxisRenderer.DateFormat?.ToString() == "d" ? currentCulture.DateTimeFormat.ShortDatePattern : seriesRenderer.XAxisRenderer.DateFormat?.ToString() == "T" ? "hh:mm:ss tt" : seriesRenderer.XAxisRenderer.DateFormat?.ToString() ?? string.Empty;
                axisFormat = string.IsNullOrEmpty(axisFormat) ? currentCulture.DateTimeFormat.ShortDatePattern : axisFormat;
            }
            else
            {
                axisFormat = (seriesRenderer.XAxisRenderer as DateTimeCategoryAxisRenderer ?? null!).CustomFormat();
            }
            return axisFormat;
        }

        /// <summary>
        /// Generates a default set of sample DateTime values for testing or demonstration purposes, covering a range of dates and times across different months. This method can be used to populate a date axis with representative data points when actual data is not available, ensuring that the axis formatting and rendering can be properly validated with typical date/time values.
        /// </summary>
        private static List<DateTime> GetDefaultSampleDates()
        {
            return
            [
                new DateTime(2022, 1, 2, 13, 30, 45),
                new DateTime(2022, 2, 7, 10, 30, 45),
                new DateTime(2022, 3, 1),
                new DateTime(2022, 4, 6),
                new DateTime(2022, 5, 5),
                new DateTime(2022, 6, 3),
                new DateTime(2022, 7, 2),
                new DateTime(2022, 8, 1),
                new DateTime(2022, 9, 1),
                new DateTime(2022, 10, 1),
                new DateTime(2022, 11, 1),
                new DateTime(2022, 12, 1)
            ];
        }

        /// <summary>
        /// Updates the Points collection of a series renderer with a new list of sorted points, ensuring that the XValue and Index properties of each point are updated to reflect their new positions in the sorted order. It also updates the ChartPoints collection if it exists, and handles category and date/time axes by maintaining a list of unique sorting labels and updating XValue accordingly. This method ensures that both the data points and their corresponding chart points are consistently ordered for accurate rendering and interaction.
        /// </summary>
        private static void UpdateSortedPoints(ChartSeriesRenderer seriesRenderer, List<Point> sortedPoints)
        {
            List<string> sortingLabels = [];
            for (int i = 0; i < seriesRenderer?.Points?.Count; i++)
            {
                Point sortedPoint = sortedPoints[i];
                if (seriesRenderer.ChartPoints is not null)
                {
                    sortedPoint.XValue = seriesRenderer.ChartPoints[i].XValue = sortedPoint.Index = seriesRenderer.ChartPoints[i].Index = i;
                }

                if (seriesRenderer.XAxisRenderer.Axis?.ValueType == ValueType.Category)
                {
                    string labelText = sortedPoint.X.ToString() ?? string.Empty;
                    if (sortingLabels.IndexOf(labelText) == -1)
                    {
                        sortingLabels.Add(labelText);
                    }
                    if (seriesRenderer.ChartPoints is not null)
                    {
                        sortedPoints[i].XValue = seriesRenderer.ChartPoints[i].XValue = sortingLabels.IndexOf(labelText);
                    }
                }

                if (seriesRenderer.XAxisRenderer.Axis?.ValueType == ValueType.DateTimeCategory)
                {
                    if (sortingLabels.IndexOf(sortedPoint.X.ToString() ?? string.Empty) == -1)
                    {
                        sortingLabels.Insert(i, ChartHelper.GetTime(Convert.ToDateTime(sortedPoint.X, CultureInfo.CurrentCulture)).ToString(CultureInfo.InvariantCulture));
                    }
                }
            }
            if (seriesRenderer is not null)
            {
                seriesRenderer.XAxisRenderer.Labels = sortingLabels;
                seriesRenderer.Points = sortedPoints;
            }
        }

        /// <summary>
        /// Builds mappings between default date format parts and their corresponding formatted values in the current culture, based on a set of sample dates and the specified axis format. It splits the axis format into its component parts, formats each part using both the default culture and the current culture, and stores the mappings in the DateValuePairs dictionary. This allows for consistent formatting of date values on the axis labels according to the current culture's conventions, while still supporting a wide range of date formats.
        /// </summary>
        private void BuildDateMappings(CultureInfo defaultCulture, CultureInfo currentCulture, string axisFormat)
        {
            List<DateTime> defaultDates = GetDefaultSampleDates();
            string[] formatParts = axisFormat.Split(_separator);
            _ = _dateValuePairs.TryAdd(defaultCulture.DateTimeFormat.DateSeparator, currentCulture.DateTimeFormat.DateSeparator);
            foreach (DateTime date in defaultDates)
            {
                foreach (string formatPart in formatParts)
                {
                    string datePart = formatPart.Length == 1 ? date.ToString("%" + formatPart, defaultCulture) : date.ToString(formatPart, defaultCulture);
                    if (!double.TryParse(datePart, out double result) || double.IsNaN(result))
                    {
                        string defaultText = Regex.Replace(date.ToString(formatPart, defaultCulture), @"[-.:,]", string.Empty);
                        string cultureText = Regex.Replace(date.ToString(formatPart, currentCulture), @"[-.:,]", string.Empty);
                        _ = _dateValuePairs.TryAdd(defaultText, cultureText);
                    }
                }
            }
        }

        /// <summary>
        /// Builds mappings between default number format symbols and their corresponding symbols in the current culture, including decimal separators, group separators, percent symbols, and currency symbols. It uses the NumberFormat properties of both the default culture and the current culture to populate the NumberValuePairs dictionary with the appropriate mappings. This ensures that numeric values are formatted correctly according to the current culture's conventions when displayed on the chart axes and labels.
        /// </summary>
        private void BuildNumberMappings(CultureInfo defaultCulture, CultureInfo currentCulture)
        {
            string currencySymbol = _isBackSymbol ? "back" : "front";
            _ = _numberValuePairs.TryAdd(defaultCulture.NumberFormat.CurrencyDecimalSeparator, currentCulture.NumberFormat.CurrencyDecimalSeparator);
            _ = _numberValuePairs.TryAdd(defaultCulture.NumberFormat.CurrencyGroupSeparator, currentCulture.NumberFormat.CurrencyGroupSeparator);
            _ = _numberValuePairs.TryAdd(defaultCulture.NumberFormat.PercentSymbol, currentCulture.NumberFormat.PercentSymbol);
            _ = _numberValuePairs.TryAdd(defaultCulture.NumberFormat.PercentDecimalSeparator, currentCulture.NumberFormat.PercentDecimalSeparator);
            _ = _numberValuePairs.TryAdd(defaultCulture.NumberFormat.PercentGroupSeparator, currentCulture.NumberFormat.PercentGroupSeparator);
            _ = _numberValuePairs.TryAdd(defaultCulture.NumberFormat.NumberDecimalSeparator, currentCulture.NumberFormat.NumberDecimalSeparator);
            _ = _numberValuePairs.TryAdd(defaultCulture.NumberFormat.NumberGroupSeparator, currentCulture.NumberFormat.NumberGroupSeparator);
            _ = _numberValuePairs.TryAdd(currencySymbol, currencySymbol);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Adds a custom element (series) to the container, respecting explicit index mapping.
        /// </summary>
        /// <param name="element">The chart element to add.</param>
        protected override void AddCustomElement(IChartElement element)
        {
            string name = (element as ChartSeries ?? null!).Name;
            if (!string.IsNullOrEmpty(name) && _seriesIndexes.TryGetValue((element as ChartSeries ?? null!).GenerateSeriesKey(), out int index))
            {
                if (Elements.Count > index)
                {
                    Elements.Insert(index, element);
                }
                else
                {
                    Elements.Add(element);
                }
                OrderTheElements([.. Elements.OrderBy(x => _seriesIndexes[(x as ChartSeries ?? null!).GenerateSeriesKey()])]);
            }
            else
            {
                Elements.Add(element);
            }
        }

        /// <summary>
        /// Called when a new element is added; triggers UI update when initial rect is available.
        /// </summary>
        /// <param name="element">Added element.</param>
        protected override void OnElementAdded(IChartElement element)
        {
            if (Owner?.InitialRect is not null)
            {
                _ = InvokeAsync(StateHasChanged);
            }
        }

        /// <summary>
        /// Called when an element is removed; ensures renderer and state are cleaned.
        /// </summary>
        /// <param name="element">Removed element.</param>
        protected override void OnElementRemoved(IChartElement element)
        {
            if (element is not null)
            {
                RemoveRenderer((element as ChartSeries ?? null!).Renderer);
                if (Owner is not null && !Owner.ChartDisposed())
                {
                    _ = InvokeAsync(StateHasChanged);
                }
            }
        }

        /// <summary>
        /// Called when a renderer is added; wires renderer-to-series relationships and registers axis requirements.
        /// </summary>
        /// <param name="renderer">Renderer instance.</param>
        /// <param name="element">Corresponding element.</param>
        protected override void OnRendererAdded(IChartElementRenderer renderer, IChartElement element)
        {
            if (renderer is IRequireAxis)
            {
                _elementsRequiredAxis.Add(renderer as IRequireAxis ?? null!);
            }

            ChartSeriesRenderer seriesRenderer = renderer as ChartSeriesRenderer ?? null!;

            if (seriesRenderer is not null)
            {
                seriesRenderer.Index = seriesRenderer.RendererIndex;
                seriesRenderer.Series = element as ChartSeries;
                seriesRenderer.XAxisName = seriesRenderer.Series?.XAxisName ?? string.Empty;
                seriesRenderer.YAxisName = seriesRenderer.Series?.YAxisName ?? string.Empty;
                seriesRenderer.Interior = !string.IsNullOrEmpty(seriesRenderer.Series?.Fill) ? seriesRenderer.Series.Fill : _palette?[seriesRenderer.Index % _palette.Length];
                if (seriesRenderer.Series is not null && seriesRenderer.Series.Container is null)
                {
                    seriesRenderer.Series.Container = Owner;
                }
            }

            if (renderer is not null && !renderer.GetType().Equals(typeof(DefaultSeriesRenderer)))
            {
                AddVisibleSeriesRenderers(seriesRenderer ?? null!);
            }
        }

        /// <summary>
        /// Called when a renderer is removed; updates owner collections and axis requirements.
        /// </summary>
        /// <param name="renderer">Renderer to remove.</param>
        protected override void OnRendererRemoved(IChartElementRenderer renderer)
        {
            _ = Owner?._visibleSeriesRenderers.Remove(renderer as ChartSeriesRenderer ?? null!);
            if (renderer is IRequireAxis)
            {
                _ = _elementsRequiredAxis.Remove(renderer as IRequireAxis ?? null!);

            }
        }

        /// <summary>
        /// Resolves the stacking group identifier for the specified series, returning a normalized group name for 100% stacked area or line series, or the series' explicit <see cref="ChartSeries.StackingGroup"/> value for all other types.
        /// </summary>
        /// <param name="series"> The <see cref="ChartSeries"/> whose stacking group is being resolved.</param>
        /// <returns>
        /// <c>"StackingArea100"</c> when the series type contains <c>"StackingArea"</c>;
        /// <c>"StackingLine100"</c> when the series type contains <c>"StackingLine"</c>;
        /// otherwise, the value of <see cref="ChartSeries.StackingGroup"/>.
        /// </returns>
        protected static string GetStackingGroup(ChartSeries series)
        {
            return series?.SeriesType is not null && series.SeriesType.Contains("StackingArea", StringComparison.InvariantCulture) ? "StackingArea100" : series?.SeriesType is not null && series.SeriesType.Contains("StackingLine", StringComparison.InvariantCulture) ? "StackingLine100" : series?.StackingGroup ?? null!;
        }

        /// <summary>
        /// Orders elements by Z-order so that renderers are created in correct stacking order.
        /// </summary>
        protected override void BuildRenderers(RenderTreeBuilder builder)
        {
            if (builder is null)
            {
                return;
            }

            int seq = 0;
            _paretoLineSeriesRendererCount = _paretoLineSeriesRendererIndex = 0;

            if (Elements.Any(element => (element as ChartSeries ?? null!).NeedRendererRemove))
            {
                foreach (ChartSeries element in Elements.Cast<ChartSeries>())
                {
                    if (element.NeedRendererRemove)
                    {
                        element.NeedRendererRemove = false;
                        element.UpdateDataSource = false;
                        RemoveRenderer(element.Renderer);
                    }

                    CreateSeriesElements(builder, element);
                }

                foreach (ChartSeries element in Elements.Cast<ChartSeries>())
                {
                    CreateSeriesNestedElements(builder, element);
                }
            }
            else
            {
                if (ContainerUpdate && Elements.Count == 0)
                {
                    builder.OpenComponent(seq++, typeof(DefaultSeriesRenderer));
                    builder.CloseComponent();
                }
                else
                {
                    SortSeriesByZOrder();
                    foreach (IChartElement element in Elements)
                    {
                        if (element.RendererType is null)
                        {
                            continue;
                        }

                        CreateSeriesElements(builder, element);
                    }

                    foreach (ChartSeries element in Elements.Cast<ChartSeries>())
                    {
                        if (element.RendererType is null)
                        {
                            continue;
                        }

                        CreateSeriesNestedElements(builder, element);
                    }
                }
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Finds series that belong to both the specified column and row renderers (grid intersection).
        /// </summary>
        /// <param name="columnRenderer">Column renderer.</param>
        /// <param name="rowRenderer">Row renderer.</param>
        /// <returns>List of series that match both axis sets.</returns>
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
                                    if (columnseriesRenderer == rowSeriesRenderer && columnseriesRenderer.Series is not null && columnseriesRenderer.Series.Visible)

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
        /// Finds series renderers that share both provided axis renderers.
        /// </summary>
        /// <param name="x_axisRenderer">X axis renderer.</param>
        /// <param name="y_axisRenderer">Y axis renderer.</param>
        /// <returns>Collection of matching series renderers.</returns>
        internal static List<ChartSeriesRenderer> FindAxisToSeriesCollection(ChartAxisRenderer x_axisRenderer, ChartAxisRenderer y_axisRenderer)
        {
            List<ChartSeriesRenderer> seriesCollection = [];
            foreach (ChartSeriesRenderer x_SeriesRenderer in x_axisRenderer.SeriesRenderer)
            {
                foreach (ChartSeriesRenderer y_SeriesRenderer in y_axisRenderer.SeriesRenderer)
                {
                    if (x_SeriesRenderer == y_SeriesRenderer)
                    {
                        seriesCollection.Add(x_SeriesRenderer);
                    }
                }
            }

            return seriesCollection;
        }

        /// <summary>
        /// Resets and initializes fields on contained series renderers.
        /// </summary>
        internal void InitSeriesRendererFields()
        {
            foreach (ChartSeriesRenderer renderer in Renderers.Cast<ChartSeriesRenderer>())
            {
                renderer.XAxisRenderer.Labels.Clear();
                renderer.InitSeriesRendererFields();
            }
            if (Owner is not null)
            {
                foreach (ChartSeriesRenderer renderer in Owner._visibleSeriesRenderers)
                {
                    renderer.InitSeriesRendererFields();
                }
            }
        }

        /// <summary>
        /// Processes raw data for each renderer, updates totals and triggers stacking/sorting.
        /// </summary>
        internal void ProcessData()
        {
            _seriesType.Clear();
            _drawTypes.Clear();
            Owner?._seriesContainer?._total.Clear();

            foreach (IChartElementRenderer renderer in Renderers)
            {
                ChartSeriesRenderer? seriesRenderer = renderer as ChartSeriesRenderer;
                if (seriesRenderer is not null && (seriesRenderer.Points?.Count != seriesRenderer.Series?.CurrentViewData.Count() || (seriesRenderer.Points?.Count == 0 && _data.Any())))
                {
                    seriesRenderer.SetCurrentViewData(_data);

                    if (!string.IsNullOrEmpty(Owner?._sorting.PropertyName) && seriesRenderer.XAxisRenderer.Axis?.ValueType == ValueType.Double)
                    {
                        seriesRenderer.XAxisRenderer.Axis.SetIsInversed(Owner._sorting.Direction == ListSortDirection.Descending);
                    }

                    seriesRenderer.ProcessData();

                    if (seriesRenderer.IsRectSeries())
                    {
                        _hasLargeData = _hasLargeData || seriesRenderer.Points?.Count > 10000;
                    }
                    _seriesType.Add(seriesRenderer.Series?.SeriesType ?? null!);
                }
            }

            UpdateSumOfSameIndex();
            Sorting();
            CalculateStacking();
        }

        /// <summary>
        /// Performs sorting for category/date-category axes when global sorting is enabled.
        /// </summary>
        internal void Sorting()
        {
            if (string.IsNullOrEmpty(Owner?._sorting.PropertyName))
            {
                return;
            }

            foreach (IChartElementRenderer renderer in Renderers)
            {
                ChartSeriesRenderer? seriesRenderer = renderer as ChartSeriesRenderer;
                if (seriesRenderer?.XAxisRenderer.Axis?.ValueType != ValueType.Double)
                {
                    List<Point> sortedPoints = SortedPoints(seriesRenderer ?? null!);
                    UpdateSortedPoints(seriesRenderer!, sortedPoints);
                }
            }
        }

        /// <summary>
        /// Determines whether stacking calculations are required and triggers them.
        /// </summary>
        internal void CalculateStacking()
        {
            bool isCalculateStacking = false;
            foreach (string type in _seriesType)
            {
                if (type.Contains("Stacking", StringComparison.InvariantCulture) && !isCalculateStacking)
                {
                    CalculateStackedValue(type.Contains("100", StringComparison.InvariantCulture));
                    isCalculateStacking = true;
                    break;
                }
            }
        }

        /// <summary>
        /// Updates stacking values for individual renderers when series collection changes.
        /// </summary>
        internal void UpdateStackingValues()
        {
            string type;
            foreach (ChartSeriesRenderer renderer in Renderers.Cast<ChartSeriesRenderer>())
            {
                if (renderer.Series is not null && renderer.Series.Visible)
                {
                    renderer.Series.Renderer.RectCount = renderer.Series.Renderer.Position = 0;
                }
                type = renderer.Series?.SeriesType ?? string.Empty;
                if (type.Contains("Stacking", StringComparison.InvariantCulture))
                {
                    CalculateStackedValue(type.Contains("100", StringComparison.InvariantCulture));
                }
            }
        }

        /// <summary>
        /// Calculates stacked values across grid cells of column/row renderers.
        /// </summary>
        /// <param name="isStacking">True when 100% stacking is required.</param>
        internal void CalculateStackedValue(bool isStacking)
        {
            if (Owner?._columnContainer is not null)
            {
                foreach (ChartColumnRenderer columnRenderer in Owner._columnContainer.Renderers.Cast<ChartColumnRenderer>())
                {
                    if (Owner._rowContainer is not null)
                    {
                        foreach (ChartRowRenderer rowRenderer in Owner._rowContainer.Renderers.Cast<ChartRowRenderer>())
                        {
                            CalculateStackingValues(FindSeriesCollection(columnRenderer, rowRenderer), isStacking);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Performs initial animations for visible series according to global options.
        /// </summary>
        /// <param name="animationInfo">List of animation descriptors.</param>
        internal void PerformAnimation(List<InitialAnimationInfo> animationInfo)
        {
            foreach (ChartSeriesRenderer renderer in Renderers.Cast<ChartSeriesRenderer>())
            {
                ChartSeries series = renderer.Series ?? null!;
                if (series.Visible && ((series.Animation.Enable && SyncfusionService?._options.Animation == GlobalAnimationMode.Default) || (SyncfusionService?._options.Animation == GlobalAnimationMode.Enable)))
                {
                    renderer.PerformInitialAnimation(animationInfo);
                    double endTime = series.Animation.Delay + series.Animation.Duration;
                    if (endTime >= Owner?._maxAnimationDuration)
                    {
                        Owner._maxAnimationDuration = endTime;
                        Owner._lastSeriesAnimationIndex = animationInfo.Count - 1;
                    }
                }
            }
        }

        /// <summary>
        /// Sets default renderer values and triggers axis / legend defaults in a safe manner.
        /// </summary>
        internal override void SetDefaultRendererValues()
        {
            try
            {
                Owner?.InitiAxis();
                ProcessData();
                Owner?._legendRenderer?.SetDefaultRendererValues();
                Owner?._axisContainer?.SetDefaultRendererContainerValues();
            }
            catch
            {
                if (!IsDisposed)
                {
                    throw;
                }
            }
        }

        internal void OnThemeChanged()
        {
            _palette = Owner?.Palettes.Length > 0 ? Owner.Palettes : ChartHelper.GetSeriesColor(Owner?.Theme.ToString() ?? string.Empty);
            foreach (ChartSeriesRenderer renderer in Renderers.Cast<ChartSeriesRenderer>())
            {
                ChartMarker seriesMarker = renderer.Series is not null ? renderer.Series.Marker : null!;
                renderer.Interior = !string.IsNullOrEmpty(renderer.Series?.Fill) ? renderer.Series.Fill : _palette[renderer.Index % _palette.Length];
                renderer.UpdateCustomization("Fill");
                renderer.ProcessRenderQueue();
                seriesMarker?.Renderer?.UpdateDirection();
                seriesMarker?.DataLabel?.Renderer?.DatalabelValueChanged();
            }
        }

        /// <summary>
        /// Ensures renderer has a valid interior fill derived from palette.
        /// </summary>
        /// <param name="renderer">Series renderer.</param>
        internal void GetSeriesRendererInterior(ChartSeriesRenderer renderer)
        {
            renderer.Interior ??= _palette?[renderer.Index % _palette.Length];
        }

        /// <summary>
        /// Gathers globalization mappings for axis and label formatting.
        /// </summary>
        /// <param name="seriesRenderer">Renderer to evaluate.</param>
        /// <param name="defaultCulture">Default culture used for pattern sources.</param>
        internal void GetGlobalizationOptions(ChartSeriesRenderer seriesRenderer, CultureInfo defaultCulture)
        {
            if (seriesRenderer.XAxisRenderer is null || seriesRenderer.YAxisRenderer is null)
            {
                return;
            }

            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            ChartAxis axis = seriesRenderer.XAxisRenderer.Axis ?? null!;
            ChartAxis y_axis = seriesRenderer.YAxisRenderer.Axis ?? null!;
            string axisLabelFormat = axis.LabelFormat;
            string axisFormat;

            if (axis.ValueType is ValueType.DateTime or ValueType.DateTimeCategory)
            {
                axisFormat = DetermineDateAxisFormat(axis, seriesRenderer, currentCulture);
                BuildDateMappings(defaultCulture, currentCulture, axisFormat);
            }
            if ((axis.ValueType != ValueType.DateTime || axis.ValueType != ValueType.DateTimeCategory) && ((axis.ValueType != ValueType.Category && !string.IsNullOrEmpty(axisLabelFormat)) || !string.IsNullOrEmpty(y_axis.LabelFormat)))
            {
                if (!axisLabelFormat.Contains("{value}", StringComparison.InvariantCulture) || !y_axis.LabelFormat.Contains("{value}", StringComparison.InvariantCulture))
                {
                    _ = _numberValuePairs.TryAdd(defaultCulture.NumberFormat.CurrencySymbol, currentCulture.NumberFormat.CurrencySymbol);
                }
            }
            BuildNumberMappings(defaultCulture, currentCulture);
        }

        /// <summary>
        /// Builds globalization mapping dictionaries for all renderers.
        /// </summary>
        internal void SetGlobalizationValues()
        {
            bool invariantGlobalizationEnabled = CultureInfo.InvariantCulture.Equals(CultureInfo.CurrentCulture);
            CultureInfo defaultCulture = invariantGlobalizationEnabled ? CultureInfo.InvariantCulture : CultureInfo.CreateSpecificCulture("en-US");
            _dateValuePairs.Clear();
            _numberValuePairs.Clear();
            foreach (IChartElementRenderer renderer in Renderers)
            {
                GetGlobalizationOptions(renderer as ChartSeriesRenderer ?? null!, defaultCulture);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds renderer to the container keeping ordering and special renderer handling (default and pareto).
        /// </summary>
        /// <param name="renderer">Renderer to add.</param>
        public override void AddRenderer(IChartElementRenderer renderer)
        {
            if (renderer is not null && !Renderers.Contains(renderer))
            {
                ContainerPrerender = false;
                RendererShouldRender = true;
                Renderers.Insert((renderer as ChartSeriesRenderer ?? null!).RendererIndex, renderer);
                int index = Renderers.IndexOf(renderer);

                if (renderer.GetType().Equals(typeof(DefaultSeriesRenderer)))
                {
                    DefaultSeries = new ChartSeries();
                    OnRendererAdded(renderer, DefaultSeries);
                }
                else if (renderer.GetType().Equals(typeof(ParetoLineSeriesRenderer)))
                {
                    OnRendererAdded(renderer, ParetoSeries ?? null!);
                    _paretoLineSeriesRendererCount++;
                }
                else
                {
                    OnRendererAdded(renderer, Elements[index - _paretoLineSeriesRendererCount]);
                }
            }
        }

        /// <summary>
        /// Signals size changes to child renderers and clears label cache.
        /// </summary>
        /// <param name="rect">New chart rectangle.</param>
        public override void HandleChartSizeChange(Rect rect)
        {
            IsRendererUpdate = Renderers.Count > 0;
            _dataLabelCollection.Clear();
            foreach (ChartRenderer renderer in Renderers.Cast<ChartRenderer>())
            {
                renderer.HandleChartSizeChange(rect);
            }
        }

        /// <summary>
        /// Processes each renderer's render queue.
        /// </summary>
        public override void ProcessRenderQueue()
        {
            foreach (ChartRenderer renderer in Renderers.Cast<ChartRenderer>())
            {
                renderer.ProcessRenderQueue();
            }
        }

        /// <summary>
        /// Returns the area type for the first renderer. Defaults to CartesianAxes.
        /// </summary>
        /// <returns>Chart area type.</returns>
        public ChartAreaType GetAreaType()
        {
            ChartSeriesRenderer renderer = Renderers.Count > 0 ? Renderers.First() as ChartSeriesRenderer ?? null! : null!;
            return renderer is not null ? ChartAreaType.CartesianAxes : ChartAreaType.CartesianAxes;
        }
        #endregion
    }
}
