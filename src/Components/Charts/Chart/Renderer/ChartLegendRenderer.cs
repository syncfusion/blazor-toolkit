using Microsoft.AspNetCore.Components.Rendering;
using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Represents the legend renderer for chart components.
    /// Handles rendering, positioning, and interaction of chart legends.
    /// </summary>
    /// <remarks>
    /// This component is responsible for:
    /// - Calculating legend bounds and positions
    /// - Rendering legend items with proper styling
    /// - Managing legend paging and templates
    /// - Handling legend click and hover interactions
    /// </remarks>
    public class ChartLegendRenderer : LegendBase, ILegendMethods
    {
        #region Fields
        private int _seriesIndex;
        private string? _legendId;
        private List<string> _legendItemIds = [];
        private double _textHeight;
        private double _textPadding;
        private List<int>? _rowMaxWrapText;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the available rectangle for legend rendering.
        /// </summary>
        /// <value>The rectangular area available for the legend, or <see langword="null"/> if not set.</value>
        internal Rect? AvailableRect { get; set; }

        /// <summary>
        /// Gets or sets the legend settings configuration.
        /// </summary>
        /// <value>The legend settings, or <see langword="null"/> if not configured.</value>
        internal ChartLegendSettings? LegendSettings { get; set; }

        /// <summary>
        /// Gets or sets the keyboard focus target element ID.
        /// </summary>
        /// <value>The element ID that should receive keyboard focus.</value>
        internal string KeyboardFocusTarget { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets whether a legend item has been clicked.
        /// </summary>
        /// <value><see langword="true"/> if a legend item was clicked; otherwise <see langword="false"/>.</value>
        internal bool HasLegendClicked { get; set; }

        /// <summary>
        /// Gets or sets the current legend position.
        /// </summary>
        /// <value>The current position of the legend.</value>
        internal LegendPosition CurrentLegendPosition { get; set; }

        /// <summary>
        /// Gets or sets the temporary rectangle used during calculations.
        /// </summary>
        /// <value>A temporary rectangular area, or <see langword="null"/> if not set.</value>
        private Rect? TempRect { get; set; }
        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Initializes the legend renderer and sets up required dependencies.
        /// </summary>
        protected override void OnInitialized()
        {
            AddToRenderQueue(this);
            if (Owner is not null)
            {
                Owner._legendRenderer = this;
                ChartId = Owner.ID;
                _legendId = ChartId + "_chart_legend";
                _legendItemIds = [_legendId + "_text_", _legendId + "_shape_marker_", _legendId + "_shape_", _legendId + "_g_", _legendId + "_template_"];
                MaxWidth = RowCountPerPage = 0;
            }
        }

        /// <summary>
        /// Disposes of resources used by the legend renderer.
        /// </summary>
        protected override ValueTask DisposeAsyncCore()
        {
            LegendOptions?.Clear();
            PagingOptions?.Clear();
            TemplateOptions?.Clear();
            return base.DisposeAsyncCore();
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Sets default style for legend if not already set.
        /// </summary>
        private void SetDefaultStyle()
        {
            if (LegendSettings is not null)
            {
                return;
            }

            LegendSettings = new ChartLegendSettings();
            Legend = LegendSettings;
        }

        /// <summary>
        /// Clears rendering options to prepare for recalculation.
        /// </summary>
        private void ClearRenderingOptions()
        {
            LegendOptions.Clear();
            TemplateOptions.Clear();
            PagingOptions.Clear();
        }

        /// <summary>
        /// Initializes settings for legend collection.
        /// </summary>
        private void InitializeLegendCollectionSettings()
        {
            LegendCollection = [];
            BaseLegendRef = this;
            ThemeStyle = Owner?._chartThemeStyle ?? null;
            Position = Owner is not null && Owner._isAdaptiveRendering ? GetAdaptiveLegendPosition() : LegendSettings?.Position ?? LegendPosition.Auto;
            IsRTL = Owner?.EnableRtl ?? false;
            IsInverse = LegendSettings is not null && LegendSettings.IsInversed;
            BorderWidth = LegendSettings?.Border.Width ?? 1;
            Reverse = LegendSettings is not null && LegendSettings.Reverse;
        }

        /// <summary>
        /// Processes all visible series to create legend options.
        /// </summary>
        private void ProcessVisibleSeries()
        {
            int legendIndex = 0;
            if (Owner is not null)
            {
                foreach (ChartSeriesRenderer seriesRenderer in Owner._visibleSeriesRenderers)
                {
                    if (ShouldIncludeSeriesInLegend(seriesRenderer))
                    {
                        LegendOption option = CreateLegendOption(seriesRenderer, legendIndex);
                        LegendCollection.Add(option);
                    }
                    legendIndex++;
                }
            }
        }

        /// <summary>
        /// Determines whether a series should be included in the legend.
        /// </summary>
        /// <param name="seriesRenderer">The series renderer to check.</param>
        /// <returns><see langword="true"/> if series should be included; otherwise <see langword="false"/>.</returns>
        private static bool ShouldIncludeSeriesInLegend(ChartSeriesRenderer seriesRenderer)
        {
            ChartSeries? series = seriesRenderer.Series ?? null;
            return seriesRenderer.Category() != SeriesCategories.Indicator && (!string.IsNullOrEmpty(series?.Name) || series?.LegendItemTemplate is not null);
        }

        /// <summary>
        /// Creates a legend option for a series renderer.
        /// </summary>
        /// <param name="seriesRenderer">The series renderer.</param>
        /// <param name="legendIndex">The legend index.</param>
        /// <returns>A configured <see cref="LegendOption"/> instance.</returns>
        private LegendOption CreateLegendOption(ChartSeriesRenderer seriesRenderer, int legendIndex)
        {
            ChartSeries? series = seriesRenderer.Series ?? null!;
            bool visible = (seriesRenderer.Category() == SeriesCategories.TrendLine) ? seriesRenderer.TrendLineLegendVisibility : series.Visible;
            return Owner is null
                ? null!
                : new LegendOption(
                id: string.Empty,
                templateSize: series.LegendTemplateSize is not null ? series.LegendTemplateSize : new Size(0, 0),
                textSize: new Size(0, 0),
                text: series.Name,
                fill: GetLegendFill(seriesRenderer), seriesIndex: Owner._trendlineContainer is not null && Owner._trendlineContainer.IsTrendLine ? legendIndex : seriesRenderer.Index,
                shape: series.LegendShape,
                seriesWidth: series.Width,
                dashArray: series.DashArray,
                textStyle: LegendSettings?.TextStyle ?? null!,
                seriesBorderColor: series.Border.Color,
                seriesBorderWidth: series.Border.Width,
                visible: visible,
                type: series.SeriesType ?? null!,
                markerShape: series.Marker.Shape != ChartShape.Auto ? series.Marker.Shape : series.Marker.Visible ? (ChartShape)(seriesRenderer.Index % Constants.ChartMarkerCount) : ChartShape.Circle,
                markerVisibility: series.Marker.Visible && Owner._shouldRenderMarker,
                legendTemplate: series.LegendItemTemplate,
                locatedPageIndex: 0
            );
        }

        /// <summary>
        /// Gets the fill color/gradient for a legend item.
        /// </summary>
        /// <param name="seriesRenderer">The series renderer.</param>
        /// <returns>The fill value as a string (color or gradient URL).</returns>
        private string GetLegendFill(ChartSeriesRenderer seriesRenderer)
        {
            ChartSeries? series = seriesRenderer.Series;
            if (series is null)
            {
                return string.Empty;
            }
            if (seriesRenderer.Category() == SeriesCategories.TrendLine)
            {
                ChartSeries? parentSeries = (Owner?._seriesContainer?.Renderers.ElementAtOrDefault(seriesRenderer.SourceIndex) as ChartSeriesRenderer)?.Series;
                ChartTrendline? trendline = parentSeries?.Trendlines?.FirstOrDefault(t => t.TargetSeries == series);

                return GetTrendlineLegendFill(trendline!, series?.Fill ?? string.Empty);
            }

            if (ChartHelper.NeedsLegendHorizontalLineGradient(series))
            {
                bool hasLinear = series.LinearGradient?.GradientColorStops?.Count > 0 && !string.IsNullOrEmpty(series.LinearGradient?.ID);
                bool hasRadial = series.RadialGradient?.GradientColorStops?.Count > 0 && !string.IsNullOrEmpty(series.RadialGradient?.ID);
                return series.LinearGradient is null || series.RadialGradient is null
                    ? string.Empty
                    : hasLinear ? $"url(#{series.LinearGradient.ID}_{series.Type}Legend)" : hasRadial ? $"url(#{series.RadialGradient.ID}_{series.Type}Legend)" : seriesRenderer.Interior ?? string.Empty;
            }

            return seriesRenderer.Interior ?? string.Empty;
        }

        /// <summary>
        /// Gets the fill for a trendline legend item.
        /// </summary>
        /// <param name="trendline">The trendline configuration.</param>
        /// <param name="defaultStroke">The default stroke color.</param>
        /// <returns>The fill value for the trendline.</returns>
        private static string GetTrendlineLegendFill(ChartTrendline trendline, string defaultStroke)
        {
            if (trendline is null)
            {
                return defaultStroke ?? string.Empty;
            }

            string suffix = (trendline.Renderer?.Series?.LegendShape == LegendShape.HorizontalLine) ? $"_{trendline.Renderer.Series.LegendShape}Legend" : string.Empty;
            bool hasLinear = trendline.LinearGradient?.GradientColorStops?.Count > 0 && !string.IsNullOrEmpty(trendline.LinearGradient?.ID);
            bool hasRadial = trendline.RadialGradient?.GradientColorStops?.Count > 0 && !string.IsNullOrEmpty(trendline.RadialGradient?.ID);
            return hasLinear ? $"url(#{trendline.LinearGradient?.ID}{suffix})" : hasRadial ? $"url(#{trendline.RadialGradient?.ID}{suffix})" : defaultStroke ?? string.Empty;
        }

        /// <summary>
        /// The method is used to check whether current legend group within the legend bounds.
        /// </summary>
        /// <param name="previousBound">Specifies the pervious legend group total width.</param>
        /// <param name="textWidth">Specifies the current legend text width.</param>
        /// <param name="legendTemplate">Specifies the current legend template.</param>
        private bool IsWithinBounds(double previousBound, double textWidth, RenderFragment legendTemplate)
        {
            return !IsRTL
                ? (previousBound + textWidth) > (LegendBounds.X + LegendBounds.Width + (legendTemplate is null ? (Legend?.ShapeWidth ?? 0) / 2 : 0))
                : (previousBound - textWidth) < (LegendBounds.X - (legendTemplate is null ? (Legend?.ShapeWidth ?? 0) / 2 : 0));
        }

        /// <summary>
        /// Calculates the X position for the previous legend item.
        /// </summary>
        /// <param name="prevLegend">The previous legend option.</param>
        /// <param name="textWidth">The text width.</param>
        /// <returns>The calculated X position.</returns>
        private double CalculatePreviousBound(LegendOption prevLegend, double textWidth)
        {
            return (!IsRTL) ? (prevLegend?.Location.X ?? 0) + (textWidth - 0.5) : (prevLegend?.Location.X ?? 0) + 0.5 - textWidth;
        }

        /// <summary>
        /// Handles wrapped legend item placement.
        /// </summary>
        /// <param name="legendOption">The legend option to place.</param>
        /// <param name="start">The starting location.</param>
        /// <param name="prevLegend">The previous legend option.</param>
        /// <param name="count">The current item count.</param>
        /// <param name="firstLegend">The first legend index.</param>
        /// <param name="itemPadding">The item padding.</param>
        private void HandleWrappedPlacement(LegendOption legendOption, ChartEventLocation start, LegendOption prevLegend, int count, int firstLegend, double itemPadding)
        {
            if (legendOption is { })
            {
                legendOption.Location.X = start?.X ?? 0;
            }

            if (count != firstLegend)
            {
                ChartRowCount++;
            }

            double locationY = CalculateLocationY(prevLegend, count, firstLegend, itemPadding);
            double startY = Math.Max(PageStartY, LegendBounds.Y);
            double clipHeight = startY + ClipPathHeight + (MaxItemHeight / 2);

            if (legendOption is not null && count == firstLegend)
            {
                legendOption.Location.Y = prevLegend.Location.Y;
                legendOption.LocatedPageIndex = CurrentLegendPageLocationIndex;
            }
            else if (legendOption is not null && ExceedsClipHeight(legendOption, locationY, clipHeight))
            {
                legendOption.Location.Y = startY + ClipPathHeight + (Legend?.Padding ?? 0) + (MaxItemHeight / 2);
                PageStartY = startY + ClipPathHeight;
                CurrentLegendPageLocationIndex += 1;
                legendOption.LocatedPageIndex = CurrentLegendPageLocationIndex;
                if (legendOption.LegendTemplate is not null)
                {
                    TotalPageCount++;
                }
            }
            else
            {
                if (legendOption is not null)
                {
                    legendOption.Location.Y = locationY;
                    legendOption.LocatedPageIndex = CurrentLegendPageLocationIndex;
                }
            }

            UpdatePageCountForLegend(legendOption!);
        }

        /// <summary>
        /// Calculates the Y location for a legend item.
        /// </summary>
        /// <param name="prevLegend">The previous legend option.</param>
        /// <param name="count">The current item count.</param>
        /// <param name="firstLegend">The first legend index.</param>
        /// <param name="itemPadding">The item padding.</param>
        /// <returns>The calculated Y position.</returns>
        private double CalculateLocationY(LegendOption prevLegend, int count, int firstLegend, double itemPadding)
        {
            if (count == firstLegend)
            {
                return prevLegend.Location.Y;
            }

            double rowHeight = IsVertical ? prevLegend.TextSize.Height : (RowHeights?.Count > 0 ? RowHeights[ChartRowCount - 2] : 0);
            double padding = (IsVertical || (ChartRowCount > 1 && !double.IsNaN(Legend?.ItemPadding ?? 0))) ? ItemPadding : itemPadding;
            return prevLegend.Location.Y + rowHeight + padding;
        }

        /// <summary>
        /// Checks if content exceeds clip height.
        /// </summary>
        /// <param name="legendOption">The legend option.</param>
        /// <param name="locationY">The Y location.</param>
        /// <param name="clipHeight">The clip path height.</param>
        /// <returns><see langword="true"/> if exceeds; otherwise <see langword="false"/>.</returns>
        private bool ExceedsClipHeight(LegendOption legendOption, double locationY, double clipHeight)
        {
            double? wrapped = _rowMaxWrapText?[legendOption.RowIndex] * _textHeight;
            double? itemHeight = GetItemHeightNullable(legendOption);
            double? sum = locationY + wrapped + itemHeight;
            return sum > clipHeight;
        }

        /// <summary>
        /// Gets the item height, accounting for templates and shapes.
        /// </summary>
        /// <param name="legendOption">The legend option.</param>
        /// <returns>The calculated item height, or <see langword="null"/>.</returns>
        private double? GetItemHeightNullable(LegendOption legendOption)
        {
            if (legendOption.LegendTemplate is null)
            {
                return MaxItemHeight / 2d;
            }

            double? templateHeight = legendOption.TemplateSize?.Height;
            double? shapeHeight = Legend?.ShapeHeight;

            bool? templateLess = templateHeight < shapeHeight;
            return templateLess == true ? shapeHeight : templateHeight;
        }

        /// <summary>
        /// Updates page count for a legend item.
        /// </summary>
        /// <param name="legendOption">The legend option.</param>
        private void UpdatePageCountForLegend(LegendOption legendOption)
        {
            if (legendOption?.LegendTemplate is null)
            {
                double pageThreshold = LegendBounds.Y + (Legend?.Padding ?? 0) + (TotalPageCount * ClipPathHeight) + (IsPaging ? 0 : (MaxItemHeight + (Legend?.Padding ?? 0)));
                TotalPageCount += legendOption?.Location.Y > pageThreshold ? 1 : 0;
            }

            IsPaging = TotalPageCount > 1;
        }

        /// <summary>
        /// Handles inline legend item placement.
        /// </summary>
        /// <param name="legendOption">The legend option to place.</param>
        /// <param name="prevLegend">The previous legend option.</param>
        /// <param name="count">The current item count.</param>
        /// <param name="firstLegend">The first legend index.</param>
        /// <param name="previousBound">The previous bound.</param>
        private void HandleInlinePlacement(LegendOption legendOption, LegendOption prevLegend, int count, int firstLegend, double previousBound)
        {
            if (legendOption is not null)
            {
                legendOption.Location.X = (count == firstLegend) ? prevLegend.Location.X : previousBound;
                legendOption.Location.Y = prevLegend.Location.Y;
                legendOption.LocatedPageIndex = CurrentLegendPageLocationIndex;
            }
        }

        /// <summary>
        /// Applies text trimming to a legend item if configured.
        /// </summary>
        /// <param name="legendOption">The legend option to trim.</param>
        /// <param name="textPadding">The text padding.</param>
        private void ApplyTextTrimming(LegendOption legendOption, double textPadding)
        {
            if (LegendSettings?.TextOverflow == LabelOverflow.Ellipse && LegendSettings.TextWrap == TextWrap.Normal)
            {
                double availWidth = CalculateAvailableWidth(legendOption, textPadding);
                if (legendOption is { })
                {
                    legendOption.Text = ChartHelper.TextTrim(Convert.ToDouble(availWidth.ToString("F4", culture), culture), legendOption.Text, LegendTextStyle ?? null!);
                }
            }
        }

        /// <summary>
        /// Calculates the available width for a legend item's text.
        /// </summary>
        /// <param name="legendOption">The legend option.</param>
        /// <param name="textPadding">The text padding.</param>
        /// <returns>The available width for text.</returns>
        private double CalculateAvailableWidth(LegendOption legendOption, double textPadding)
        {
            double legendX = legendOption?.Location.X ?? 0;
            double shapeOffset = legendOption?.LegendTemplate is null ? (Legend?.ShapeWidth ?? 0) / 2 : 0;

            double availWidth = (!IsRTL) ? LegendBounds.X + LegendBounds.Width - (legendX + textPadding - ItemPadding - shapeOffset) : legendX - textPadding + ItemPadding + shapeOffset - LegendBounds.X;

            if (LegendSettings?.MaximumLabelWidth != 0)
            {
                availWidth = Math.Min(LegendSettings?.MaximumLabelWidth ?? 0, availWidth);
            }

            return availWidth;
        }

        /// <summary>
        /// Initializes legend variables for layout calculations.
        /// </summary>
        /// <param name="rowWidthList">The row width list to populate.</param>
        private void InitializeLegendVariables(out List<double> rowWidthList)
        {
            ChartRowCount = 1;
            RowHeights = [];
            ColumnHeights = [];
            PageHeights = [];
            _rowMaxWrapText = [];
            rowWidthList = [];
        }

        /// <summary>
        /// Sets up legend text measurements and styles.
        /// </summary>
        private void SetupLegendTextMeasurements()
        {
            LegendTextStyle = LegendSettings?.TextStyle.GetChartFontOptions(Owner?._chartThemeStyle ?? null!) ?? null!;
            _textHeight = ChartHelper.MeasureText("MeasureText", LegendTextStyle).Height;
            MaxItemHeight = Math.Max(_textHeight, Legend?.ShapeHeight ?? 0);
        }

        /// <summary>
        /// Applies extra space to legend bounds based on available size.
        /// </summary>
        /// <param name="availableSize">The available size for the legend.</param>
        private void CalculateAndApplyExtraSpace(Size availableSize)
        {
            double extraHeight = 0, extraWidth = 0;

            if (!IsVertical)
            {
                extraHeight = string.IsNullOrEmpty(Legend?.Height) ? ((availableSize?.Height ?? 0) / 100 * 5) : 0;
            }
            else
            {
                extraWidth = string.IsNullOrEmpty(Legend?.Width) ? ((availableSize?.Width ?? 0) / 100 * 5) : 0;
            }

            LegendBounds.Height += extraHeight;
            LegendBounds.Width += extraWidth;
        }

        /// <summary>
        /// Adjusts bounds for custom legend position.
        /// </summary>
        /// <param name="availableSize">The available size for the legend.</param>
        private void AdjustBoundsForCustomPosition(Size availableSize)
        {
            if (LegendSettings?.Position == LegendPosition.Custom)
            {
                LegendBounds.Height = Math.Min(LegendBounds.Height, (availableSize?.Height ?? 0) - LegendSettings.Location.Y - LegendSettings.Border.Width);
            }

            _textPadding = LegendCollection[0].LegendTemplate is null ? (Legend?.ShapeWidth ?? 0) + (Legend?.ShapePadding ?? 0) : 0;
        }

        /// <summary>
        /// Processes each legend option in the collection.
        /// </summary>
        /// <param name="rowWidthList">The row width list.</param>
        /// <param name="rowCount">The row count (ref).</param>
        /// <param name="maximumWidth">The maximum width (ref).</param>
        /// <param name="rowWidth">The row width (ref).</param>
        /// <param name="columnCount">The column count (ref).</param>
        /// <param name="maxTextHeight">The max text height (ref).</param>
        /// <param name="columnHeight">The column height (ref).</param>
        /// <param name="verticalArrowSpace">The vertical arrow space.</param>
        /// <returns><see langword="true"/> if any legend should be rendered.</returns>
        private bool ProcessEachLegendOption(List<double> rowWidthList, ref double rowCount, ref double maximumWidth, ref double rowWidth, ref double columnCount, ref double maxTextHeight, ref double columnHeight, double verticalArrowSpace)
        {
            double padding = Legend?.Padding ?? 0;
            bool render = false;

            foreach (LegendOption legendOption in LegendCollection)
            {
                bool firstLegend = LegendCollection.IndexOf(legendOption) == 0;

                RaiseLegendRenderEvent(legendOption);
                HandleTemplateResize(legendOption);
                legendOption.TextCollection = [];

                if (legendOption.Render && (!string.IsNullOrEmpty(legendOption.Text) || legendOption.LegendTemplate is not null))
                {
                    render = true;

                    int wrapTextCount = CalculateLegendItemDimensions(legendOption, rowWidth);
                    double legendWidth = CalculateLegendWidth(legendOption, firstLegend);
                    rowWidth += legendWidth;
                    maxTextHeight = Math.Max(maxTextHeight, legendOption?.TextSize.Height ?? 0);

                    bool wrapped = HandleLegendItemWrapping(legendWidth, ref rowWidth, padding, ref columnCount, ref columnHeight, ref maximumWidth, ref rowCount, rowWidthList, wrapTextCount, verticalArrowSpace);
                    if (!wrapped)
                    {
                        if (_rowMaxWrapText is not null)
                        {
                            if (_rowMaxWrapText.Count == 0 && rowCount == 0)
                            {
                                _rowMaxWrapText.Add(wrapTextCount);
                            }
                            else
                            {
                                _rowMaxWrapText[^1] = Math.Max(_rowMaxWrapText[^1], wrapTextCount);
                            }
                        }

                        firstLegend = false;
                        IsTopBottomVertical = false;
                    }

                    UpdateHeightCollections(legendOption!, rowCount, columnCount, padding, firstLegend);
                    columnCount++;
                }
            }

            return render;
        }

        /// <summary>
        /// Raises the legend render event for an item.
        /// </summary>
        /// <param name="legendOption">The legend option.</param>
        private void RaiseLegendRenderEvent(LegendOption legendOption)
        {
            LegendRenderEventArgs legendEvent = new()
            {
                Name = "OnLegendItemRender",
                Text = legendOption.Text,
                Fill = legendOption.Fill,
                Shape = legendOption.Shape,
                MarkerShape = legendOption.MarkerShape
            };

            if (Owner?.OnLegendItemRender is not null)
            {
                DataVizCommonHelper.InvokeEvent(Owner.OnLegendItemRender, legendEvent);
            }

            if (Constants.SubRegex().IsMatch(legendEvent.Text))
            {
                legendEvent.Text = ChartHelper.GetUniCode(legendEvent.Text, Constants.SubPattern, Constants.SubRegex());
            }

            if (Constants.SupRegex().IsMatch(legendEvent.Text))
            {
                legendEvent.Text = ChartHelper.GetUniCode(legendEvent.Text, Constants.SubPattern, Constants.SupRegex());
            }

            legendOption.Render = !legendEvent.Cancel;
            legendOption.Text = legendEvent.Text;
            legendOption.Fill = legendEvent.Fill;
            legendOption.Shape = legendEvent.Shape;
            legendOption.MarkerShape = legendEvent.MarkerShape;
        }

        /// <summary>
        /// Handles template resize if needed.
        /// </summary>
        /// <param name="legendOption">The legend option.</param>
        private void HandleTemplateResize(LegendOption legendOption)
        {
            if (Owner is not null && Owner._isResize &&
                legendOption.LegendTemplate is not null && !Owner._isResizeTemplate && IsVertical)
            {
                legendOption.TemplateSize = new Size(0, 0);
            }
        }

        /// <summary>
        /// Calculates the dimensions of a legend item.
        /// </summary>
        /// <param name="legendOption">The legend option.</param>
        /// <param name="rowWidth">The current row width.</param>
        /// <returns>The wrap text count.</returns>
        private int CalculateLegendItemDimensions(LegendOption legendOption, double rowWidth)
        {
            if (legendOption?.TemplateSize?.Width == 0 && legendOption.TemplateSize.Height == 0)
            {
                KeyValuePair<List<string>, Size> textAndSize = GetLegendTextAndSize(legendOption.Text, rowWidth);
                legendOption.TextCollection = textAndSize.Key;
                legendOption.TextSize = textAndSize.Value;
                return legendOption.TextCollection.Count - 1;
            }
            else
            {
                if (legendOption == null)
                {
                    return 0;
                }
                legendOption.TextSize = legendOption.TemplateSize ?? null!;
                return 0;
            }
        }

        /// <summary>
        /// Calculates the width of a legend item.
        /// </summary>
        /// <param name="legendOption">The legend option.</param>
        /// <param name="firstLegend">Indicates if this is the first legend item.</param>
        /// <returns>The calculated width.</returns>
        private double CalculateLegendWidth(LegendOption legendOption, bool firstLegend)
        {
            double padding = Legend?.Padding ?? 0;
            double itemPadding = (!IsVertical) ? (firstLegend || IsTopBottomVertical ? padding : ItemPadding) : padding;

            return _textPadding + (legendOption?.TextSize.Width ?? 0) + itemPadding;
        }

        /// <summary>
        /// Handles legend item wrapping logic.
        /// </summary>
        /// <param name="legendWidth">The legend width.</param>
        /// <param name="rowWidth">The current row width (ref).</param>
        /// <param name="padding">The padding value.</param>
        /// <param name="columnCount">The column count (ref).</param>
        /// <param name="columnHeight">The column height (ref).</param>
        /// <param name="maximumWidth">The maximum width (ref).</param>
        /// <param name="rowCount">The row count (ref).</param>
        /// <param name="rowWidthList">The row width list.</param>
        /// <param name="wrapTextCount">The wrap text count.</param>
        /// <param name="verticalArrowSpace">The vertical arrow space.</param>
        /// <returns><see langword="true"/> if wrapped; otherwise <see langword="false"/>.</returns>
        private bool HandleLegendItemWrapping(double legendWidth, ref double rowWidth, double padding, ref double columnCount, ref double columnHeight, ref double maximumWidth, ref double rowCount, List<double> rowWidthList, int wrapTextCount, double verticalArrowSpace)
        {
            if (LegendBounds.Width < ((2 * padding) + rowWidth) || IsVertical)
            {
                IsTopBottomVertical = true;
                maximumWidth = Math.Max(maximumWidth, rowWidth + padding - (IsVertical ? 0 : legendWidth));

                if (rowCount == 0 && (legendWidth != rowWidth))
                {
                    rowCount = 1;
                }

                rowWidthList.Add(maximumWidth);
                rowWidth = IsVertical ? 0 : legendWidth;
                rowCount++;
                columnCount = 0;
                columnHeight = verticalArrowSpace;
                _rowMaxWrapText?.Add(wrapTextCount);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Updates height collections for row and column tracking.
        /// </summary>
        /// <param name="legendOption">The legend option.</param>
        /// <param name="rowCount">The row count.</param>
        /// <param name="columnCount">The column count.</param>
        /// <param name="padding">The padding.</param>
        /// <param name="firstLegend">Indicates if first legend item.</param>
        private void UpdateHeightCollections(LegendOption legendOption, double rowCount, double columnCount, double padding, bool firstLegend)
        {
            int len = (int)(rowCount > 0 ? (rowCount - 1) : 0);

            RowHeights?.Insert(len, Math.Max(len >= 0 && len < RowHeights.Count ? RowHeights[len] : 0, MaxItemHeight));

            double columnHeightsValue = ComputeColumnHeightValue(legendOption!, columnCount, padding, firstLegend);
            ColumnHeights?.Insert((int)columnCount, columnHeightsValue);
        }

        /// <summary>
        /// Computes the column height value.
        /// </summary>
        /// <param name="legendOption">The legend option.</param>
        /// <param name="columnCount">The column count.</param>
        /// <param name="padding">The padding.</param>
        /// <param name="firstLegend">Indicates if first legend.</param>
        /// <returns>The computed column height.</returns>
        private double ComputeColumnHeightValue(LegendOption legendOption, double columnCount, double padding, bool firstLegend)
        {
            int colIndex = (int)columnCount;
            double baseHeight = (colIndex >= 0 && colIndex < ColumnHeights?.Count ? ColumnHeights[colIndex] : 0) + MaxItemHeight;
            double additionalPadding = IsVertical
                ? (firstLegend || LegendCollection.IndexOf(legendOption ?? null!) == 0) ? padding : ItemPadding
                : IsTopBottomVertical
                    ? !double.IsNaN(Legend?.ItemPadding ?? 0) ? ItemPadding : 8
                    : LegendCollection.IndexOf(legendOption ?? null!) == 0 ? padding : 0;
            return baseHeight + additionalPadding;
        }

        /// <summary>
        /// Updates row width statistics.
        /// </summary>
        /// <param name="rowWidthList">The row width list.</param>
        private void UpdateRowWidthStatistics(List<double> rowWidthList)
        {
            foreach (double item in rowWidthList)
            {
                MaxRowWidth = Math.Max(MaxRowWidth, item);
            }
        }

        /// <summary>
        /// Calculates paging and finalizes bounds.
        /// </summary>
        /// <param name="rowCount">The row count.</param>
        /// <param name="wrapTextCount">The wrap text count.</param>
        /// <param name="render">Indicates if should render.</param>
        /// <param name="rowWidth">The row width.</param>
        /// <param name="maximumWidth">The maximum width.</param>
        private void CalculatePagingAndFinalBounds(double rowCount, int wrapTextCount, bool render, double rowWidth, double maximumWidth)
        {
            double padding = Legend?.Padding ?? 0;
            double columnHeight = (ColumnHeights?.Count > 0 ? ColumnHeights.Max() : 0) + padding;

            columnHeight = Math.Max(columnHeight + (wrapTextCount > 1 ? padding : 0), (MaxItemHeight + padding) + padding);
            IsPaging = LegendBounds.Height < columnHeight && rowCount > 0;
            rowCount = IsPaging ? rowCount : 1;

            if (render)
            {
                SetBounds(Math.Max(rowWidth + padding, maximumWidth), columnHeight);

                TotalPageCount = Math.Ceiling((rowCount / Math.Max(1, RowCountPerPage - 1)) + (wrapTextCount / Math.Max(0, Math.Ceiling((LegendBounds.Height - (Legend?.Padding ?? 0)) / _textHeight) + 1)));
            }
            else
            {
                SetBounds(0, 0);
            }
        }

        /// <summary>
        /// Finalizes legend location and bounds.
        /// </summary>
        /// <param name="rect">The bounding rectangle.</param>
        /// <param name="availableSize">The available size.</param>
        private void FinalizeLegendLocation(Rect rect, Size availableSize)
        {
            if (LegendSettings is not null)
            {
                GetLocation(rect ?? null!, availableSize ?? null!, LegendSettings.Margin, Owner?._margin ?? null!, LegendSettings.Border, LegendSettings.Location);
            }

            LegendBounds.Width = LegendSettings?.Position == LegendPosition.Custom ? Math.Min(LegendBounds.Width, (availableSize?.Width ?? 0) - LegendSettings.Location.X - LegendSettings.Border.Width) : LegendBounds.Width;
        }

        /// <summary>
        /// Gets the maximum label width for text fitting.
        /// </summary>
        /// <param name="previousRowWidth">The previous row width.</param>
        /// <param name="currentRowWidth">The current row width.</param>
        /// <returns>The maximum label width.</returns>
        private double GetMaximumLabelWidth(double previousRowWidth, double currentRowWidth)
        {
            if (LegendSettings is not null && ChartHelper.IsNaNOrZero(LegendSettings.MaximumLabelWidth))
            {
                previousRowWidth = LegendBounds.Width < (Legend?.Padding + currentRowWidth + previousRowWidth) ? _textPadding : previousRowWidth;
                return LegendBounds.Width - LegendBounds.X - previousRowWidth;
            }
            return LegendSettings?.MaximumLabelWidth ?? 0;
        }

        /// <summary>
        /// Calculates the size of wrapped text lines.
        /// </summary>
        /// <param name="textCollection">The collection of text lines.</param>
        /// <returns>The total size of the wrapped text.</returns>
        private Size GetWrapTextSize(List<string> textCollection)
        {
            Size textSize;
            double textWidth = 0, textHeight = 0;
            foreach (string text in textCollection)
            {
                textSize = ChartHelper.MeasureText(text, LegendTextStyle ?? null!);
                textWidth = Math.Max(textWidth, textSize.Width);
                textHeight += textSize.Height;
            }
            return new Size(textWidth, textHeight);
        }

        /// <summary>
        /// Gets the text and size for a legend item with wrapping support.
        /// </summary>
        /// <param name="legendText">The legend text.</param>
        /// <param name="previousRowWidth">The previous row width.</param>
        /// <returns>A key-value pair of text collection and size.</returns>
        private KeyValuePair<List<string>, Size> GetLegendTextAndSize(string legendText, double previousRowWidth)
        {
            List<string> textCollection = [];
            Size legendTextSize = ChartHelper.MeasureText(legendText, LegendTextStyle ?? null!);
            double currentRowWidth = legendTextSize.Width + _textPadding;

            if (LegendSettings?.TextWrap == TextWrap.Normal && LegendSettings.TextOverflow == LabelOverflow.Ellipse)
            {
                string text = ChartHelper.TextTrim(GetMaximumLabelWidth(previousRowWidth, currentRowWidth), legendText, LegendTextStyle ?? null!);
                legendTextSize = ChartHelper.MeasureText(text, LegendTextStyle ?? null!);
                textCollection.Add(text);
                return new KeyValuePair<List<string>, Size>(textCollection, legendTextSize);
            }
            else if (LegendSettings?.TextOverflow != LabelOverflow.Clip)
            {
                textCollection = ChartHelper.TextWrap(legendText, GetMaximumLabelWidth(previousRowWidth, currentRowWidth), LegendTextStyle ?? null!, LegendSettings?.TextWrap == TextWrap.AnyWhere);
                legendTextSize = GetWrapTextSize(textCollection);
                return new KeyValuePair<List<string>, Size>(textCollection, legendTextSize);
            }
            else
            {
                textCollection.Add(legendText);
                return new KeyValuePair<List<string>, Size>(textCollection, new Size { Width = ChartHelper.IsNaNOrZero(LegendSettings.MaximumLabelWidth) ? legendTextSize.Width : LegendSettings.MaximumLabelWidth, Height = legendTextSize.Height });
            }
        }

        /// <summary>
        /// Processes click on legend items.
        /// </summary>
        /// <param name="eventArgs">The mouse event arguments.</param>
        /// <param name="isMouseMove">Indicates whether triggered by mouse movement.</param>
        /// <param name="targetId">The ID of the target element.</param>
        private void ProcessLegendItemClick(ChartInternalMouseEventArgs eventArgs, bool isMouseMove, string targetId)
        {
            if (!ChartHelper.WithInBounds(eventArgs.MouseX, eventArgs.MouseY, Owner?._legendRenderer?.PagingRect ?? null!) && ChartHelper.WithInBounds(eventArgs.MouseX, eventArgs.MouseY, Owner?._legendRenderer?.LegendBounds ?? null!) && LegendCollection.Count != 0)
            {
                foreach (string id in _legendItemIds)
                {
                    if (targetId.Contains(id, StringComparison.OrdinalIgnoreCase) && int.TryParse(targetId.AsSpan(id.Length), out _seriesIndex))
                    {
                        LegendClick(isMouseMove);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Handles legend click events.
        /// </summary>
        /// <param name="isMouseMove">Indicates whether triggered by mouse movement.</param>
        private void LegendClick(bool isMouseMove)
        {
            ChartSeriesRenderer seriesRenderer = Owner?._visibleSeriesRenderers[_seriesIndex] ?? null!;
            LegendOption legend = LegendCollection.Find(x => x.SeriesIndex == _seriesIndex) ?? null!;
            LegendClickEventArgs legendClickArgs = new
            (
                "OnLegendClick",
                false,
                Owner ?? null!,
                legend.Shape,
                seriesRenderer.Series ?? null!,
                legend.Text
            );

            if (!isMouseMove)
            {
                InvokeLegendClickEvent(legendClickArgs);
            }
            if (!legendClickArgs.Cancel)
            {
                seriesRenderer.Series?.SetLegendShape(legendClickArgs.LegendShape);
                UpdateSeriesInterior(seriesRenderer, isMouseMove);
            }
        }

        /// <summary>
        /// Invokes the legend click event.
        /// </summary>
        /// <param name="legendClickArgs">The legend click event arguments.</param>
        private void InvokeLegendClickEvent(LegendClickEventArgs legendClickArgs)
        {
            if (Owner?.OnLegendClick.HasDelegate == true)
            {
                _ = SfChart.InvokeDelegateAsync(Owner.OnLegendClick, legendClickArgs);
            }
        }

        /// <summary>
        /// Updates series interior color after legend interaction.
        /// </summary>
        /// <param name="seriesRenderer">The series renderer.</param>
        /// <param name="isMouseMove">Indicates whether triggered by mouse movement.</param>
        private void UpdateSeriesInterior(ChartSeriesRenderer seriesRenderer, bool isMouseMove)
        {
            if (!string.IsNullOrEmpty(seriesRenderer.Series?.Fill))
            {
                seriesRenderer.Interior = !string.IsNullOrEmpty(seriesRenderer.Series.Fill) ? seriesRenderer.Series.Fill : Owner?._seriesContainer?._palette?[seriesRenderer.Index % Owner._seriesContainer._palette.Length] ?? null;
            }

            List<ChartSelectedDataIndex> selectedDataIndexes = [];

            if (Owner?._selectionModule is not null)
            {
                selectedDataIndexes = Owner.SelectedDataIndexes;
            }

            if (Owner is not null && LegendSettings is not null && LegendSettings.ToggleVisibility && !isMouseMove)
            {
                Owner._redraw = Owner.EnableAnimation;
                if (seriesRenderer.Category() == SeriesCategories.TrendLine)
                {
                    ChartTrendline? trendLine = Owner._trendlineContainer?.Elements[seriesRenderer.Index] as ChartTrendline;
                    trendLine?.SetVisibility(!trendLine.Visible);
                }
                ChangeSeriesVisiblity(seriesRenderer, seriesRenderer.Series?.Visible ?? true);
                if (selectedDataIndexes.Count > 0 && Owner._selectionModule is not null)
                {
                    Owner._selectionModule.SelectedDataIndexes = selectedDataIndexes;
                    _ = Owner._selectionModule.RedrawSelectionAsync();
                }
            }
        }

        /// <summary>
        /// Processes click on paging controls.
        /// </summary>
        /// <param name="targetId">The target element ID.</param>
        private void ProcessPagingClick(string targetId)
        {
            if (IsPaging)
            {
                if (targetId.Contains(_legendId + "_pageup", StringComparison.OrdinalIgnoreCase))
                {
                    ChangePage(true);
                }
                else if (targetId.Contains(_legendId + "_pagedown", StringComparison.OrdinalIgnoreCase))
                {
                    ChangePage(false);
                }
            }
        }

        /// <summary>
        /// Changes the visibility state of a series.
        /// </summary>
        /// <param name="series">The series renderer.</param>
        /// <param name="visibility">The current visibility state.</param>
        private void ChangeSeriesVisiblity(ChartSeriesRenderer series, bool visibility)
        {
            HasLegendClicked = true;
            if (series?.Series is not null)
            {
                series.Series._isLegendClicked = true;
                series.Series.OnLegendClick(!visibility);
            }
        }

        /// <summary>
        /// Updates a template legend option for the new page.
        /// </summary>
        private void UpdateTemplateLegendOption(LegendOption legendOption, bool pageUp, int page, string pointerValue)
        {
            TextOptions? currentTextOption = LegendOptions.Find(l => l.Index == legendOption?.SeriesIndex)?.TextOption;
            double yValue = Convert.ToDouble(currentTextOption?.Y, null);
            yValue = pageUp ? yValue + ClipPathHeight : yValue - ClipPathHeight;

            if (currentTextOption is { })
            {
                currentTextOption.Y = yValue.ToString(culture);
            }

            string id = Owner?.ID + "_chart_legend_template_" + Convert.ToString(legendOption.SeriesIndex, null);
            legendOption.TemplateID = id;
            string visibility = "visible";

            if (legendOption.LocatedPageIndex != page)
            {
                visibility = "hidden";
            }
            string style = "position: absolute; visibility: " + visibility + ";" + " left: " + currentTextOption?.X + "px;" + " top: " + currentTextOption?.Y + "px;" + " pointer-events: bounding-box; cursor: " + pointerValue + "; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;";

            TemplateOptions.Add(new LegendItemTemplateOptions()
            {
                Id = id,
                style = style,
                LegendTemplate = legendOption.LegendTemplate
            });
        }

        /// <summary>
        /// Updates the legend options for a specific legend item.
        /// </summary>
        /// <param name="legendOption">The legend option to update.</param>
        /// <param name="index">The legend index.</param>
        private void UpdateLegendOptions(LegendOption legendOption, int index)
        {
            List<SymbolOptions> symbols = CalculateLegendOptions(legendOption, index);
            LegendSymbols? legendSymbols = LegendOptions.Find(x => x.Index == index);

            if (legendSymbols is not null)
            {
                legendSymbols.FirstSymbol = symbols[0];
                legendSymbols.SecondSymbol = symbols[1];
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Builds the render tree for the legend component.
        /// </summary>
        /// <param name="builder">The render tree builder instance.</param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            if (builder is null)
            {
                return;
            }

            RendererShouldRender = false;
        }

        /// <summary>
        /// Changes the current legend page.
        /// </summary>
        /// <param name="pageUp">Indicates whether to go to previous page.</param>
        protected void ChangePage(bool pageUp)
        {
            SvgText? pageNumberElement = Owner?._svgRenderer?.TextElementList?.Find(element => element.Id == PageNumberID);
            int page = int.Parse(pageNumberElement?.Text.Split('/')[!IsRTL ? 0 : 1] ?? string.Empty, null);
            if (pageUp && page > 1)
            {
                TemplateOptions.Clear();
                TranslateLegendTemplates(pageUp, page - 1);
                _ = TranslatePageAsync(page - 2, page - 1, pageNumberElement ?? null!);
                Owner?._legendItemTemplateContainer?.InvalidateRender();
            }
            else if (!pageUp && page < TotalPageCount)
            {
                TemplateOptions.Clear();
                TranslateLegendTemplates(pageUp, page + 1);
                _ = TranslatePageAsync(page, page + 1, pageNumberElement ?? null!);
                Owner?._legendItemTemplateContainer?.InvalidateRender();
            }
        }

        /// <summary>
        /// Translates the legend page display.
        /// </summary>
        /// <param name="page">The page index (0-based).</param>
        /// <param name="pageNumber">The page number (1-based).</param>
        /// <param name="pageNumberElement">The page number text element.</param>
        protected async Task TranslatePageAsync(double page, double pageNumber, SvgText pageNumberElement = null!)
        {
            Transform = "translate(0,-" + (ClipPathHeight * page).ToString(culture) + ")";
            CurrentPageNumber = pageNumber;

            if (Owner is not null)
            {
                await InvokeVoidAsync(Owner._chartJsModule!, Owner._chartJsInProcessModule!, Constants.SetAttribute, [.. new object[] { LegendTranslateID ?? null!, "transform", Transform }]).ConfigureAwait(true);
            }

            pageNumberElement?.ChangeText(!IsRTL ? CurrentPageNumber + "/" + TotalPageCount : TotalPageCount + "/" + CurrentPageNumber);
            PagingOptions[0].TextOption.Text = pageNumberElement?.Text ?? string.Empty;

            if (Owner?._customLegendRenderer is not null)
            {
                Owner._customLegendRenderer.RendererShouldRender = true;
                Owner._customLegendRenderer.ProcessRenderQueue();
            }
        }

        /// <summary>
        /// Translates legend templates for the specified page.
        /// </summary>
        /// <param name="pageUp">Indicates whether navigating to previous page.</param>
        /// <param name="page">The target page number.</param>
        protected void TranslateLegendTemplates(bool pageUp, int page)
        {
            string pointerValue = LegendSettings is not null && LegendSettings.ToggleVisibility ? "pointer" : "default";

            foreach (LegendOption legendOption in LegendCollection)
            {
                if (legendOption.LegendTemplate is not null)
                {
                    UpdateTemplateLegendOption(legendOption, pageUp, page, pointerValue);
                }
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Gets the font key for caching purposes.
        /// </summary>
        /// <returns>A unique font key string.</returns>
        internal string GetFontKey()
        {
            SetDefaultStyle();

            string? fontWeight = LegendSettings?.TextStyle.GetFontWeight(Owner?._chartThemeStyle ?? null!);
            return fontWeight + Constants.Underscore + LegendSettings?.TextStyle.FontStyle + Constants.Underscore + LegendSettings?.TextStyle.GetFontFamily(Owner?._chartThemeStyle ?? null!);
        }

        /// <summary>
        /// Updates legend template positions after recalculation.
        /// </summary>
        internal void UpdateLegendTemplatePosition()
        {
            ClearRenderingOptions();
            if (Owner is not null)
            {
                double marginTop = Owner._isAdaptiveRendering ? Owner.GetChartMargin(true) : !double.IsNaN(Owner._margin.Top) ? Owner._margin.Top : (SyncfusionService is not null && SyncfusionService.IsDeviceMode ? 5 : 10);

                CalculateLegendBounds(TempRect ?? null!, Owner.AvailableSize, marginTop, "Chart", null!);
                CalculateRenderTreeBuilderOptions();

                AvailableRect = TempRect;

                if (Owner._customLegendRenderer is not null)
                {
                    Owner._customLegendRenderer.RendererShouldRender = true;
                    Owner._customLegendRenderer.ProcessRenderQueue();
                }
                if (Owner._legendItemTemplateContainer is not null)
                {
                    Owner._legendItemTemplateContainer?.InvalidateRender();
                }
            }
        }

        /// <summary>
        /// Checks and updates the legend rendered state.
        /// </summary>
        internal void IsLegendRenderedCheck()
        {
            bool legendsWithTemplates = LegendCollection.Any(a => a.LegendTemplate is not null);
            if (legendsWithTemplates && Owner is not null)
            {
                Owner._isLegendRendered = true;
            }
        }

        /// <summary>
        /// Sets default renderer values and initializes the legend.
        /// </summary>
        internal override void SetDefaultRendererValues()
        {
            SetDefaultStyle();
            HandleChartSizeChange(Owner?.InitialRect ?? null!);
            if (Owner?._customLegendRenderer is not null)
            {
                Owner._customLegendRenderer.RendererShouldRender = RendererShouldRender;
                Owner._customLegendRenderer.ProcessRenderQueue();
            }
        }

        /// <summary>
        /// Retrieves and configures legend options for all visible series.
        /// </summary>
        internal void GetLegendOptions()
        {
            InitializeLegendCollectionSettings();
            ProcessVisibleSeries();
            if (Reverse)
            {
                LegendCollection.Reverse();
            }
        }

        /// <summary>
        /// Gets the adaptive legend position based on chart dimensions.
        /// </summary>
        /// <returns>The adaptive <see cref="LegendPosition"/>.</returns>
        internal LegendPosition GetAdaptiveLegendPosition()
        {
            LegendPosition position = LegendSettings?.Position ?? LegendPosition.Auto;

            if (Owner?._widthCategory == ChartWidthCategory.Medium)
            {
                if (LegendSettings?.Position is not (LegendPosition.Top or LegendPosition.Bottom))
                {
                    position = LegendPosition.Bottom;
                }
            }
            else if (Owner?._heightCategory == ChartHeightCategory.Medium)
            {
                if (LegendSettings?.Position is not (LegendPosition.Right or LegendPosition.Left))
                {
                    position = LegendPosition.Right;
                }
            }
            return position;
        }

        /// <summary>
        /// Processes navigation when entering a legend element.
        /// </summary>
        /// <param name="targetId">The target element ID.</param>
        /// <param name="isMouseMove">Indicates whether triggered by mouse movement.</param>
        internal void ProcessNavigationLegendEnter(string targetId, bool isMouseMove = false)
        {
            if (LegendSettings is not null && !LegendSettings.Visible)
            {
                return;
            }

            foreach (string id in _legendItemIds)
            {
                if (targetId.Contains(id, StringComparison.InvariantCulture))
                {
                    _seriesIndex = int.Parse(targetId.Split(id)[1], null);
                    LegendClick(isMouseMove);
                    break;
                }
            }
        }

        /// <summary>
        /// Handles click events on legend items or paging controls.
        /// </summary>
        /// <param name="eventArgs">The mouse event arguments.</param>
        /// <param name="isMouseMove">Indicates whether this is triggered by mouse movement.</param>
        internal void Click(ChartInternalMouseEventArgs eventArgs, bool isMouseMove = false)
        {
            if (LegendSettings is not null && !LegendSettings.Visible)
            {
                return;
            }

            ProcessLegendItemClick(eventArgs, isMouseMove, eventArgs.Target);
            ProcessPagingClick(eventArgs.Target);
        }

        /// <summary>
        /// Refreshes series positions after legend interaction.
        /// </summary>
        internal void RefreshSeriesPosition()
        {
            Owner?._seriesContainer?.Renderers.ForEach(renderer => (renderer as ChartSeriesRenderer ?? null!).Position = (((renderer as ChartSeriesRenderer ?? null!).Series?.Visible ?? false) && (!(renderer as ChartSeriesRenderer ?? null!).Series?.Renderer.IsStackingSeries() ?? false)) ? double.NaN : (renderer as ChartSeriesRenderer ?? null!).Position);
        }

        /// <summary>
        /// Updates the fill color for a series legend item.
        /// </summary>
        /// <param name="seriesRenderer">The series renderer.</param>
        /// <param name="seriesFill">The new fill color.</param>
        internal void UpdateLegendFill(ChartSeriesRenderer seriesRenderer, string seriesFill = null!)
        {
            if (seriesRenderer is null)
            {
                return;
            }

            ChartSeries series = seriesRenderer.Series ?? null!;
            LegendOption? legendOption = LegendCollection.Find(x => x.SeriesIndex == seriesRenderer.Index);

            if (seriesRenderer.Category() != SeriesCategories.Indicator && !string.IsNullOrEmpty(series.Name) && legendOption is not null)
            {
                bool visibility = (seriesRenderer.Category() == SeriesCategories.TrendLine) ? seriesRenderer.TrendLineLegendVisibility : seriesRenderer.Series is not null && seriesRenderer.Series.Visible;
                int index = (seriesRenderer.Category() == SeriesCategories.TrendLine) ? _seriesIndex : (int)legendOption.SeriesIndex;

                legendOption.Visible = visibility;
                legendOption.Fill = !string.IsNullOrEmpty(seriesFill) ? seriesFill : legendOption.Fill;

                UpdateLegendOptions(legendOption, (int)legendOption.SeriesIndex);
                LegendSymbols? legendSymbols = LegendOptions.Find(x => x.Index == index);

                if (legendSymbols is { })
                {
                    legendSymbols.TextOption.Fill = visibility ? ChartHelper.FindThemeColor(legendOption.TextStyle.Color, ThemeStyle?.LegendLabel ?? null!) : "#D3D3D3";
                }

                if (Owner?._customLegendRenderer is not null && Position == LegendPosition.Custom)
                {
                    Owner._customLegendRenderer.RendererShouldRender = true;
                    Owner._customLegendRenderer.ProcessRenderQueue();
                }
            }
        }

        /// <summary>
        /// Updates the legend shape for a series.
        /// </summary>
        /// <param name="seriesRenderer">The series renderer.</param>
        internal void UpdateLegendShape(ChartSeriesRenderer seriesRenderer)
        {
            if (seriesRenderer is null)
            {
                return;
            }

            ChartSeries series = seriesRenderer.Series ?? null!;
            if (seriesRenderer.Category() != SeriesCategories.Indicator && !string.IsNullOrEmpty(series.Name))
            {
                int legendIndex = !Reverse ? seriesRenderer.Index : LegendCollection.Count - 1 - seriesRenderer.Index;
                int index = (seriesRenderer.Category() == SeriesCategories.TrendLine) ? _seriesIndex : legendIndex;
                LegendOption? legendOption = LegendCollection.Find(x => x.SeriesIndex == seriesRenderer.Index);

                if (index < LegendCollection.Count && legendOption is not null)
                {
                    legendOption.Type = series.SeriesType ?? null!;
                    legendOption.MarkerShape = series.Marker.Shape != ChartShape.Auto ? series.Marker.Shape : series.Marker.Visible ? (ChartShape)(seriesRenderer.Index % Constants.ChartMarkerCount) : ChartShape.Circle;
                    legendOption.MarkerVisibility = series.Marker.Visible && (Owner?._shouldRenderMarker ?? false);
                    legendOption.Shape = series.LegendShape;
                    UpdateLegendOptions(legendOption, legendIndex);
                }

                if (Owner?._customLegendRenderer is not null && Position == LegendPosition.Custom)
                {
                    Owner._customLegendRenderer.RendererShouldRender = true;
                    Owner._customLegendRenderer.ProcessRenderQueue();
                }
            }
        }

        /// <summary>
        /// Updates the legend width for a series.
        /// </summary>
        /// <param name="seriesRenderer">The series renderer.</param>
        /// <param name="width">The new width value.</param>
        internal void UpdateLegendWidth(ChartSeriesRenderer seriesRenderer, double width)
        {
            if (seriesRenderer is null)
            {
                return;
            }

            LegendOption? legendOption = LegendCollection.Find(x => x.SeriesIndex == seriesRenderer.Index);
            if (seriesRenderer.Category() != SeriesCategories.Indicator && !string.IsNullOrEmpty(seriesRenderer.Series?.Name) && legendOption is not null)
            {
                bool visibility = (seriesRenderer.Category() == SeriesCategories.TrendLine) ? seriesRenderer.TrendLineLegendVisibility : seriesRenderer.Series.Visible;
                legendOption.Visible = visibility;
                legendOption.SeriesWidth = width;

                UpdateLegendOptions(legendOption, (int)legendOption.SeriesIndex);

                if (Owner?._customLegendRenderer is not null && Position == LegendPosition.Custom)
                {
                    Owner._customLegendRenderer.RendererShouldRender = true;
                    Owner._customLegendRenderer.ProcessRenderQueue();
                }
            }
        }

        /// <summary>
        /// Handles theme changes and updates legend styling.
        /// </summary>
        internal void OnThemeChanged()
        {
            RendererShouldRender = true;
            string fill = LegendSettings is not null && LegendSettings.Visible ? ChartHelper.FindThemeColor(LegendSettings.TextStyle.Color, Owner?._chartThemeStyle?.LegendLabel ?? null!) : "#D3D3D3";
            LegendOptions.ForEach(option => option.TextOption.Fill = fill);
            ProcessRenderQueue();
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Processes the render queue and updates child components.
        /// </summary>
        public override void ProcessRenderQueue()
        {
            if (Owner?._customLegendRenderer is not null)
            {
                Owner._customLegendRenderer.RendererShouldRender = true;
                Owner._customLegendRenderer.ProcessRenderQueue();
            }
            if (Owner?._legendItemTemplateContainer is not null)
            {
                Owner._legendItemTemplateContainer.InvalidateRender();
            }
        }

        /// <summary>
        /// Handles chart size changes and recalculates legend bounds.
        /// </summary>
        /// <param name="rect">The new available rectangle.</param>
        public override void HandleChartSizeChange(Rect rect)
        {
            if (AvailableRect != rect && rect is not null)
            {
                TemplateOptions.Clear();
                TempRect = new Rect(rect.X, rect.Y, rect.Width, rect.Height);
                RendererShouldRender = true;
                LegendOptions = [];
                PagingOptions = [];

                if (LegendSettings is not null && LegendSettings.Visible && !(Owner?._widthCategory == ChartWidthCategory.Small || Owner?._heightCategory == ChartHeightCategory.Small))
                {
                    GetLegendOptions();
                    if (LegendCollection.Count == 0)
                    {
                        return;
                    }

                    IsRendererUpdate = true;
                    double marginTop = Owner is not null && Owner._isAdaptiveRendering ? Owner.GetChartMargin(true) : !double.IsNaN(Owner?._margin.Top ?? 0) ? (Owner?._margin.Top ?? 0) : (SyncfusionService is not null && SyncfusionService.IsDeviceMode ? 5 : 10);
                    CalculateLegendBounds(rect, Owner?.AvailableSize ?? null!, marginTop, "Chart", null!);
                    CalculateRenderTreeBuilderOptions();
                    IsLegendRenderedCheck();
                }

                AvailableRect = rect;
            }
        }

        /// <summary>
        /// Calculates and assigns the rendering position for a legend item based on available space, wrapping rules, layout orientation, and the position of previously rendered legend items.
        /// </summary>
        /// <param name="legendOption"> The legend item whose render point needs to be calculated. </param>
        /// <param name="start"> The starting location from which legend item placement begins. </param>
        /// <param name="textPadding"> The combined padding applied before the legend text, including shape width and shape-to-text spacing. </param>
        /// <param name="prevLegend"> The previously rendered legend item. Used to determine horizontal or vertical continuation and spacing. </param>
        /// <param name="count"> The index of the current legend item in the render sequence. </param>
        /// <param name="firstLegend"> A flag indicating whether the current legend item is the first item in its row or column. Used for applying initial padding rules. </param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void GetRenderPoint(LegendOption legendOption, ChartEventLocation start, double textPadding, LegendOption prevLegend, int count, int firstLegend)
        {
            double itemPadding = 8;
            double textWidth = textPadding + (prevLegend?.TextSize.Width ?? 0);
            double previousBound = CalculatePreviousBound(prevLegend!, textWidth);
            bool shouldWrap = IsWithinBounds(previousBound, (legendOption?.TextSize.Width ?? 0) + textPadding - ItemPadding, legendOption?.LegendTemplate ?? null!) || IsVertical;

            if (shouldWrap && legendOption is not null)
            {
                HandleWrappedPlacement(legendOption, start, prevLegend!, count, firstLegend, itemPadding);
            }
            else
            {
                HandleInlinePlacement(legendOption!, prevLegend!, count, firstLegend, previousBound);
            }

            ApplyTextTrimming(legendOption!, textPadding);
        }

        /// <summary>
        /// Calculates and sets legend bounds based on available space and content.
        /// </summary>
        /// <param name="availableSize">The available size for the legend.</param>
        /// <param name="rect">The available rectangle.</param>
        /// <param name="maxLabelSize">The maximum label size.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void GetLegendBounds(Size availableSize, Rect rect, Size maxLabelSize)
        {
            InitializeLegendVariables(out List<double> rowWidthList);
            SetupLegendTextMeasurements();
            CalculateAndApplyExtraSpace(availableSize);
            AdjustBoundsForCustomPosition(availableSize);

            double rowCount = 0;
            double maximumWidth = 0, rowWidth = 0, columnCount = 0, maxTextHeight = 0;
            double columnHeight = 0, verticalArrowSpace = 0;

            bool render = ProcessEachLegendOption(rowWidthList, ref rowCount, ref maximumWidth, ref rowWidth, ref columnCount, ref maxTextHeight, ref columnHeight, verticalArrowSpace);

            rowWidthList.Add(rowWidth);
            UpdateRowWidthStatistics(rowWidthList);

            int wrapTextCount = _rowMaxWrapText!.Sum();

            CalculatePagingAndFinalBounds(rowCount, wrapTextCount, render, rowWidth, maximumWidth);
            FinalizeLegendLocation(rect, availableSize);
        }
        #endregion
    }
}
