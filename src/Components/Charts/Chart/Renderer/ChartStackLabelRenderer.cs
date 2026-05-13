using Microsoft.AspNetCore.Components.Rendering;
using System.Drawing;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renders stack labels for stacked chart series.
    /// </summary>
    /// <remarks>
    /// This renderer handles positioning, formatting, and rendering of stack labels
    /// above/below stacked chart data points. It manages both positive and negative
    /// stack values and applies theme-specific styling. Uses internal caching and
    /// collection-based rendering for performance.
    /// </remarks>
    public class ChartStackLabelRenderer : ChartRenderer, IChartElementRenderer
    {
        #region Fields
        private List<TextOptions> _chartStackLabels = [];
        private CultureInfo _cultureInfo = CultureInfo.InvariantCulture;
        private bool _shouldAnimate = true;
        internal List<RectOptions> _rectOptions = [];
        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Initializes the renderer and registers itself with the render queue.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            AddToRenderQueue(this);
            if (Owner is { })
            {
                Owner._stackLabelRenderer = this;
            }
        }

        /// <summary>
        /// Disposes of resources associated with this renderer.
        /// </summary>
        protected override ValueTask DisposeAsyncCore()
        {
            _rectOptions.Clear();
            _chartStackLabels.Clear();
            _cultureInfo = null!;
            return base.DisposeAsyncCore();
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Clears all accumulated rendering options.
        /// </summary>
        private void ClearRenderingOptions()
        {
            _chartStackLabels.Clear();
            _rectOptions.Clear();
        }

        /// <summary>
        /// Formats a numeric value according to the specified format string.
        /// </summary>
        /// <param name="value">The numeric value to format.</param>
        /// <param name="format">The format string.</param>
        /// <returns>The formatted label text.</returns>
        private string FormatLabel(double value, string format)
        {
            string labelFormat = string.IsNullOrEmpty(format) ? string.IsNullOrEmpty(Owner?._stackLabelSettings?.Format) ? null! : Owner._stackLabelSettings.Format : format;
            bool useCustomFormat = !string.IsNullOrEmpty(labelFormat) && labelFormat.Contains("{value}", StringComparison.InvariantCulture);
            string formattedValue = value % 1 == 0 ? value.ToString("0", CultureInfo.CurrentCulture) : Math.Round(value, 1).ToString("0.0", CultureInfo.CurrentCulture);

            return useCustomFormat
                ? labelFormat.Replace("{value}", formattedValue, StringComparison.InvariantCulture)
                : !string.IsNullOrEmpty(labelFormat) ? ChartHelper.FormatValue(value, false, labelFormat) : formattedValue;
        }

        /// <summary>
        /// Determines the appropriate fill color for stack labels based on theme and background.
        /// </summary>
        /// <returns>The fill color as a hex string.</returns>
        private string GetStackLabelFill()
        {
            ChartFontOptions stackLabelFont = Owner?._stackLabelSettings?.Font.GetFontOptions(Owner._chartThemeStyle ?? null!) ?? null!;
            if (!string.IsNullOrEmpty(stackLabelFont.Color) && stackLabelFont.Color != Constants.Transparent)
            {
                return stackLabelFont.Color;
            }

            string areaBackground = Owner?._chartAreaRenderer?.Area?.Background ?? string.Empty;
            string fallbackBackground = Owner?.Background ?? Owner?._chartThemeStyle?.Background ?? string.Empty;

            string fontBackground = string.IsNullOrEmpty(Owner?._stackLabelSettings?.Fill) ||
                                    Owner._stackLabelSettings.Fill == Constants.Transparent
                                    ? (areaBackground == Constants.Transparent ? fallbackBackground : areaBackground)
                                    : Owner._stackLabelSettings.Fill;

            Color rgbValue = ChartHelper.GetRBGValue(fontBackground);
            double contrast = Math.Round(((rgbValue.R * 299) + (rgbValue.G * 587) + (rgbValue.B * 114)) / 1000.0, 1, MidpointRounding.AwayFromZero);

            return contrast >= 128 ? "black" : "white";
        }

        /// <summary>
        /// Checks if there are visible stacking series to render.
        /// </summary>
        /// <returns><see langword="true"/> if visible stacking series exist; otherwise <see langword="false"/>.</returns>
        private bool HasVisibleStackingSeries()
        {
            return Owner is not null &&
                   Owner._visibleSeriesRenderers.Any(
                       s => s.Series?.SeriesType is not null &&
                            s.Series.Visible &&
                            s.Series.SeriesType.Contains("Stacking", StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Retrieves font options for stack labels with theme application.
        /// </summary>
        /// <returns>The <see cref="ChartFontOptions"/> or null if not configured.</returns>
        private ChartFontOptions GetStackLabelFont()
        {
            return Owner?._stackLabelSettings?.Font.GetFontOptions(Owner._chartThemeStyle ?? null!) ?? null!;
        }

        /// <summary>
        /// Collects stack points and calculates total Y values for rendering.
        /// </summary>
        /// <returns>A tuple containing positive points, negative points, and total Y values indexed by (xKey, group).</returns>
        private (Dictionary<(object xKey, string group), Point> positivePoints, Dictionary<(object xKey, string group), Point> negativePoints, Dictionary<(object xKey, string group), double> totalYValues) CollectStackPointsAndTotals()
        {
            Dictionary<(object xKey, string group), Point> positivePoints = [];
            Dictionary<(object xKey, string group), Point> negativePoints = [];
            Dictionary<(object xKey, string group), double> totalYValues = [];

            for (int i = (Owner?._visibleSeriesRenderers.Count ?? 0) - 1; i >= 0; i--)
            {
                ChartSeriesRenderer series = Owner?._visibleSeriesRenderers[i] ?? null!;
                if (series?.Points is null || (series.Series is not null && !series.Series.Visible))
                {
                    continue;
                }

                string group = series.Series?.StackingGroup ?? string.Empty;

                for (int j = 0; j < series.Points.Count; j++)
                {
                    ProcessPointForStacking(series, j, group, positivePoints, negativePoints, totalYValues);
                }
            }

            return (positivePoints, negativePoints, totalYValues);
        }

        /// <summary>
        /// Processes a single point for stacking calculations.
        /// </summary>
        /// <param name="series">The chart series renderer containing the point.</param>
        /// <param name="pointIndex">The index of the point within the series.</param>
        /// <param name="group">The stacking group identifier.</param>
        /// <param name="positivePoints">Dictionary to accumulate positive stack points.</param>
        /// <param name="negativePoints">Dictionary to accumulate negative stack points.</param>
        /// <param name="totalYValues">Dictionary to accumulate total Y values per group.</param>
        private static void ProcessPointForStacking(ChartSeriesRenderer series, int pointIndex, string group, Dictionary<(object xKey, string group), Point> positivePoints, Dictionary<(object xKey, string group), Point> negativePoints, Dictionary<(object xKey, string group), double> totalYValues)
        {
            if (series.Points is null)
            {
                return;
            }
            Point? point = series?.Points[pointIndex];
            if (point is null || !point.Visible || double.IsNaN(point.YValue) || point.SymbolLocations.Count == 0)
            {
                return;
            }

            object xKey = point.XValue;
            double endValue = series?.StackedValues?.EndValues?[pointIndex] ?? point.YValue;
            (object xKey, string group) key = (xKey, group);

            if (endValue >= 0 && !positivePoints.ContainsKey(key))
            {
                positivePoints[key] = point;
            }
            else if (endValue < 0 && !negativePoints.ContainsKey(key))
            {
                negativePoints[key] = point;
            }

            if (!double.IsNaN(point.YValue))
            {
                if (!totalYValues.ContainsKey(key))
                {
                    totalYValues[key] = 0;
                }
                totalYValues[key] += point.YValue;
            }
        }

        /// <summary>
        /// Renders stack labels for all collected stack groups.
        /// </summary>
        /// <param name="positivePoints">Dictionary of positive stack points.</param>
        /// <param name="negativePoints">Dictionary of negative stack points.</param>
        /// <param name="totalYValues">Dictionary of total Y values per group.</param>
        /// <param name="stackLabelFont">Font options for stack labels.</param>
        /// <param name="stackLabelFill">Fill color for stack labels.</param>
        private void RenderLabelsForAllKeys(Dictionary<(object xKey, string group), Point> positivePoints, Dictionary<(object xKey, string group), Point> negativePoints, Dictionary<(object xKey, string group), double> totalYValues, ChartFontOptions stackLabelFont, string stackLabelFill)
        {
            HashSet<(object xKey, string group)> allKeys = [.. positivePoints.Keys.Concat(negativePoints.Keys)];

            foreach ((object xKey, string group) in allKeys)
            {
                RenderStackLabelForKey((xKey, group), positivePoints, negativePoints, totalYValues, stackLabelFont, stackLabelFill);
            }
        }

        /// <summary>
        /// Renders a stack label for a specific stacking group key.
        /// </summary>
        /// <param name="key">The composite key (xKey, group) identifying the stack group.</param>
        /// <param name="positivePoints">Dictionary of positive stack points.</param>
        /// <param name="negativePoints">Dictionary of negative stack points.</param>
        /// <param name="totalYValues">Dictionary of total Y values per group.</param>
        /// <param name="stackLabelFont">Font options for stack labels.</param>
        /// <param name="stackLabelFill">Fill color for stack labels.</param>
        private void RenderStackLabelForKey((object xKey, string group) key, Dictionary<(object xKey, string group), Point> positivePoints, Dictionary<(object xKey, string group), Point> negativePoints, Dictionary<(object xKey, string group), double> totalYValues, ChartFontOptions stackLabelFont, string stackLabelFill)
        {
            _ = positivePoints.TryGetValue(key, out Point? positivePoint);
            _ = negativePoints.TryGetValue(key, out Point? negativePoint);

            (Point? topPoint, ChartSeriesRenderer? topSeries, bool hasPositive) = DetermineTopPointAndSeries(positivePoint, negativePoint);

            if (topPoint?.SymbolLocations is null || topPoint.SymbolLocations.Count == 0 || topSeries is null)
            {
                return;
            }

            int pointIndex = topSeries.Points?.IndexOf(topPoint) ?? 0;
            if (pointIndex < 0)
            {
                return;
            }

            double totalValue = totalYValues.TryGetValue(key, out double val) ? val : 0;
            string formattedLabel = FormatLabel(totalValue, Owner?._stackLabelSettings?.Format ?? string.Empty);
            Size textSize = ChartHelper.MeasureText(formattedLabel, stackLabelFont);

            ChartEventLocation location = topPoint.SymbolLocations[0];

            (double xPosition, double yPosition) = CalculateLabelPosition(topSeries, location, textSize, hasPositive);
            Rect rect = CalculateLabelRect(xPosition, yPosition, textSize);
            string rotation = CalculateRotationTransform(xPosition, yPosition);

            AddRectOptionToCollection(pointIndex, rect, rotation);
            AddTextOptionToCollection(pointIndex, xPosition, yPosition, stackLabelFill, stackLabelFont, formattedLabel, rotation);
        }

        /// <summary>
        /// Determines the top-most point and series for label rendering.
        /// </summary>
        /// <param name="positivePoint">The top positive stack point, if any.</param>
        /// <param name="negativePoint">The top negative stack point, if any.</param>
        /// <returns>A tuple containing the top point, its series, and whether it represents a positive value.</returns>
        private (Point? topPoint, ChartSeriesRenderer? topSeries, bool hasPositive) DetermineTopPointAndSeries(Point? positivePoint, Point? negativePoint)
        {
            ChartSeriesRenderer? pSeries = positivePoint is not null && Owner is not null
                ? Owner._visibleSeriesRenderers.FirstOrDefault(s => s.Series is not null && s.Series.Visible && s.Points is not null && s.Points.Contains(positivePoint))
                : null;

            int pIndex = positivePoint is not null ? pSeries?.Points?.IndexOf(positivePoint) ?? -1 : -1;
            double positiveValue = pIndex >= 0 ? pSeries?.StackedValues?.EndValues?[pIndex] ?? 0 : 0;
            bool hasPositive = positiveValue > 0;

            Point? topPoint = hasPositive ? positivePoint : negativePoint;
            ChartSeriesRenderer? topSeries =
                topPoint is not null && Owner is not null
                    ? Owner._visibleSeriesRenderers.FirstOrDefault(
                        s => s.Series is not null && s.Series.Visible && s.Points is not null && s.Points.Contains(topPoint))
                    : null;

            return (topPoint, topSeries, hasPositive);
        }

        /// <summary>
        /// Calculates the position of a stack label based on point location and text size.
        /// </summary>
        /// <param name="topSeries">The series containing the label point.</param>
        /// <param name="location">The symbol location of the point.</param>
        /// <param name="textSize">The measured size of the label text.</param>
        /// <param name="hasPositive">Indicates if rendering for a positive stack value.</param>
        /// <returns>A tuple containing the calculated X and Y positions.</returns>
        private (double xPosition, double yPosition) CalculateLabelPosition(ChartSeriesRenderer topSeries, ChartEventLocation location, Size textSize, bool hasPositive)
        {
            double padding = 10;

            double xOffset = CalculateXOffsetForLabel(textSize, hasPositive, topSeries, padding);
            double yOffset = CalculateYOffsetForLabel(textSize, hasPositive, topSeries, padding);

            double xPosition = Math.Max((topSeries.ClipRect?.X ?? 0) + textSize.Width, Math.Min(xOffset + (topSeries.ClipRect?.X ?? 0) + location.X, (topSeries.ClipRect?.X ?? 0) + (topSeries.ClipRect?.Width ?? 0) - textSize.Width));
            double labelRotationOffset = (Owner?._stackLabelSettings?.Angle > 0 && !Owner._requireInvertedAxis) ? textSize.Width / 2 : 0;
            double yPosition = Math.Max((topSeries.ClipRect?.Y ?? 0) + textSize.Height, Math.Min(yOffset + (topSeries.ClipRect?.Y ?? 0) + location.Y - labelRotationOffset, (topSeries.ClipRect?.Y ?? 0) + (topSeries.ClipRect?.Height ?? 0) - textSize.Height));

            return (xPosition, yPosition);
        }

        /// <summary>
        /// Calculates the horizontal offset for label positioning based on alignment and axis orientation.
        /// </summary>
        /// <param name="textSize">The measured size of the label text.</param>
        /// <param name="hasPositive">Indicates if rendering for a positive stack value.</param>
        /// <param name="topSeries">The series containing the label point.</param>
        /// <param name="padding">The padding value for label positioning.</param>
        /// <returns>The calculated X offset value.</returns>
        private double CalculateXOffsetForLabel(Size textSize, bool hasPositive, ChartSeriesRenderer topSeries, double padding)
        {
            Alignment alignment = Owner?._stackLabelSettings?.Font.TextAlignment ?? Alignment.Center;
            double xOffset;

            double alignmentValue = textSize.Width + (Owner is not null && !double.IsNaN(Owner._stackLabelSettings?.Border.Width ?? 0)
                ? Owner._stackLabelSettings?.Border.Width ?? 0 : 0) + (Owner?._stackLabelSettings?.Margin.Left ?? 0)
                + (Owner?._stackLabelSettings?.Margin.Right ?? 0) - (padding / 2);

            if (Owner is not null && Owner._requireInvertedAxis)
            {
                double halfLabelWidth = padding + (textSize.Width / 2);
                xOffset = topSeries.YAxisRenderer.Axis is not null && topSeries.YAxisRenderer.Axis.IsAxisInverse
                    ? (hasPositive ? -halfLabelWidth : halfLabelWidth) : (hasPositive ? halfLabelWidth : -halfLabelWidth);
            }
            else
            {
                xOffset = 0;
            }

            xOffset += alignment == Alignment.Far ? alignmentValue : alignment == Alignment.Near ? -alignmentValue : 0;

            return xOffset;
        }

        /// <summary>
        /// Calculates the vertical offset for label positioning based on axis orientation.
        /// </summary>
        /// <param name="textSize">The measured size of the label text.</param>
        /// <param name="hasPositive">Indicates if rendering for a positive stack value.</param>
        /// <param name="topSeries">The series containing the label point.</param>
        /// <param name="padding">The padding value for label positioning.</param>
        /// <returns>The calculated Y offset value.</returns>
        private double CalculateYOffsetForLabel(Size textSize, bool hasPositive, ChartSeriesRenderer topSeries, double padding)
        {
            double yOffset = Owner is not null && Owner._requireInvertedAxis ? padding / 2 : (topSeries.YAxisRenderer.Axis is not null && topSeries.YAxisRenderer.Axis.IsAxisInverse
                ? (hasPositive ? (textSize.Height + (padding / 2)) : -padding) : (hasPositive ? -padding : (textSize.Height + (padding / 2))));

            return yOffset;
        }

        /// <summary>
        /// Calculates the bounding rectangle for a stack label including margins.
        /// </summary>
        /// <param name="xPosition">The X coordinate of the label center.</param>
        /// <param name="yPosition">The Y coordinate of the label center.</param>
        /// <param name="textSize">The measured size of the label text.</param>
        /// <returns>A <see cref="Rect"/> representing the label bounding box.</returns>
        private Rect CalculateLabelRect(double xPosition, double yPosition, Size textSize)
        {
            double padding = 10;

            double rectX = xPosition - (textSize.Width / 2) - (Owner?._stackLabelSettings?.Margin.Left ?? 0);
            double rectY = yPosition - textSize.Height - (Owner?._stackLabelSettings?.Margin.Top ?? 0);
            double rectWidth = textSize.Width + ((Owner?._stackLabelSettings?.Margin.Left ?? 0) + (Owner?._stackLabelSettings?.Margin.Right ?? 0));
            double rectHeight = textSize.Height + (padding / 2) + ((Owner?._stackLabelSettings?.Margin.Top ?? 0) + (Owner?._stackLabelSettings?.Margin.Bottom ?? 0));

            return new Rect(rectX, rectY, rectWidth, rectHeight);
        }

        /// <summary>
        /// Calculates the SVG rotation transform string for a label.
        /// </summary>
        /// <param name="centerX">The X coordinate of the rotation center.</param>
        /// <param name="centerY">The Y coordinate of the rotation center.</param>
        /// <returns>The rotation transform string, or empty if no rotation is configured.</returns>
        private string CalculateRotationTransform(double centerX, double centerY)
        {
            return Owner?._stackLabelSettings?.Angle != 0 ? $"rotate({Owner?._stackLabelSettings?.Angle}, {centerX:F2}, {centerY:F2})" : string.Empty;
        }

        /// <summary>
        /// Adds a rectangle rendering option to the collection.
        /// </summary>
        /// <param name="pointIndex">The index of the point for ID generation.</param>
        /// <param name="rect">The rectangle dimensions and position.</param>
        /// <param name="rotation">The rotation transform string.</param>
        private void AddRectOptionToCollection(int pointIndex, Rect rect, string rotation)
        {
            RectOptions rectOption = new(
                $"{Owner?.ID}_StackLabel_Text_{pointIndex}",
                rect.X, rect.Y, rect.Width, rect.Height,
                Owner?._stackLabelSettings?.Border.Width ?? 0,
                Owner?._stackLabelSettings?.Border.Color ?? string.Empty,
                string.IsNullOrEmpty(Owner?._stackLabelSettings?.Fill) ? Constants.Transparent : Owner._stackLabelSettings.Fill,
                Owner?._stackLabelSettings?.Rx ?? 0,
                Owner?._stackLabelSettings?.Ry ?? 0)
            {
                Transform = rotation

            };
            _rectOptions.Add(rectOption);

        }

        /// <summary>
        /// Adds a text rendering option to the collection.
        /// </summary>
        /// <param name="pointIndex">The index of the point for ID generation.</param>
        /// <param name="xPosition">The X coordinate for text placement.</param>
        /// <param name="yPosition">The Y coordinate for text placement.</param>
        /// <param name="stackLabelFill">The fill color for the text.</param>
        /// <param name="stackLabelFont">The font options for the text.</param>
        /// <param name="formattedLabel">The label text to render.</param>
        /// <param name="rotation">The rotation transform string.</param>
        private void AddTextOptionToCollection(int pointIndex, double xPosition, double yPosition, string stackLabelFill, ChartFontOptions stackLabelFont, string formattedLabel, string rotation)
        {
            _shouldAnimate =
                Owner?._visibleSeriesRenderers.Count == 0 || (Owner is not null && !Owner._visibleSeriesRenderers.Any(series =>
                ((series.Series is not null && series.Series.Animation.Enable && SyncfusionService?._options.Animation == GlobalAnimationMode.Default)
                || SyncfusionService?._options.Animation == GlobalAnimationMode.Enable) && Owner._shouldAnimateSeries));

            TextOptions textOption = new(
                xPosition.ToString("F2", _cultureInfo),
                yPosition.ToString("F2", _cultureInfo),
                stackLabelFill,
                stackLabelFont,
                formattedLabel,
                "Middle",
                $"{Owner?.ID}_StackLabel_Text_{pointIndex}",
                rotation,
                "auto");

            _chartStackLabels.Add(textOption);
        }
        #endregion

        #region Protected Methods

        /// <summary>
        /// Builds the render tree for stack labels, rendering rectangles and text elements.
        /// </summary>
        /// <param name="builder">The render tree builder.</param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (builder is null || _chartStackLabels is null || _chartStackLabels.Count == 0)
            {
                return;
            }
            base.BuildRenderTree(builder);
            Owner?._svgRenderer?.OpenGroupElement(builder, Owner.ID + "_StackLabelCollection", string.Empty, string.Empty, _shouldAnimate ? "e-stacklabel-visible" : "e-stacklabel-hidden");
            foreach (RectOptions rectOption in _rectOptions.ToArray())
            {
                Owner?._svgRenderer?.RenderRect(builder, rectOption);
            }
            foreach (TextOptions textOption in _chartStackLabels.ToArray())
            {
                Owner?._svgRenderer?.RenderText(builder, textOption);
            }
            builder.CloseElement();
        }

        /// <summary>
        /// Called when the layout changes to reprocess the render queue.
        /// </summary>
        protected virtual void OnLayoutChange()
        {
            ProcessRenderQueue();
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Handles stack label value changes and triggers re-rendering if needed.
        /// </summary>
        internal void StackLabelValueChanged()
        {
            RendererShouldRender = Owner?._shouldRenderStackLabel ?? false;
            if (RendererShouldRender)
            {
                ClearRenderingOptions();
                RenderStackLabels();
            }
            InvalidateRender();
        }

        /// <summary>
        /// Renders all visible stack labels for stacked series.
        /// </summary>
        /// <remarks>
        /// Collects positive and negative stack points, calculates total values,
        /// computes label positions and sizes, then generates render options
        /// for rectangles and text.
        /// </remarks>
        internal void RenderStackLabels()
        {
            _chartStackLabels.Clear();
            _rectOptions.Clear();

            if (Owner is not null && !HasVisibleStackingSeries())
            {
                return;
            }

            ChartFontOptions stackLabelFont = GetStackLabelFont();
            string stackLabelFill = GetStackLabelFill();

            (Dictionary<(object xKey, string group), Point>? positivePoints, Dictionary<(object xKey, string group), Point>? negativePoints, Dictionary<(object xKey, string group), double>? totalYValues) = CollectStackPointsAndTotals();

            RenderLabelsForAllKeys(positivePoints, negativePoints, totalYValues, stackLabelFont, stackLabelFill);
        }

        /// <summary>
        /// Toggles stack label visibility on or off.
        /// </summary>
        internal void ToggleVisibility()
        {
            if (Owner?._stackLabelSettings is not null && Owner._stackLabelSettings.Visible)
            {
                StackLabelValueChanged();
            }
            else
            {
                RendererShouldRender = true;
                ClearRenderingOptions();
                InvalidateRender();
            }
        }

        /// <summary>
        /// Handles theme changes and updates label styling.
        /// </summary>
        internal void OnThemeChanged()
        {
            if (Owner?._stackLabelRenderer is null || Owner._stackLabelSettings is null)
            {
                return;
            }
            RendererShouldRender = _shouldAnimate = true;
            string fill = GetStackLabelFill();
            _chartStackLabels.ForEach(option => option.Fill = fill);
            ProcessRenderQueue();
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Handles chart size changes and updates label positioning accordingly.
        /// </summary>
        /// <param name="rect">The new chart rectangle dimensions.</param>
        public override void HandleChartSizeChange(Rect rect)
        {
            if (Owner?._stackLabelRenderer is not null && Owner._stackLabelSettings is not null)
            {
                RendererShouldRender = Owner._stackLabelSettings.Visible;
                if (RendererShouldRender)
                {
                    RenderStackLabels();
                }
            }
        }

        /// <summary>
        /// Invalidates the component state and triggers a re-render.
        /// </summary>
        public void InvalidateRender()
        {
            _ = InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Handles layout changes and reprocesses the render queue.
        /// </summary>
        public void HandleLayoutChange()
        {
            OnLayoutChange();
        }

        /// <summary>
        /// Reprocesses the render queue and invalidates the renderer.
        /// </summary>
        public override void ProcessRenderQueue()
        {
            if (Owner?._stackLabelRenderer is not null)
            {
                Owner._stackLabelRenderer.RendererShouldRender = true;
                Owner._stackLabelRenderer?.InvalidateRender();
            }
        }
        #endregion
    }
}