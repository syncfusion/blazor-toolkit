
namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Base renderer for column/bar style series. Contains shared geometry and layout helpers
    /// used by concrete column/bar renderers.
    /// </summary>
    public abstract class ColumnBaseRenderer : ChartSeriesRenderer
    {
        #region Constants
        /// <summary>
        /// Space character used for constructing path strings.
        /// </summary>
        protected const string SPACE = " ";
        #endregion

        #region Properties

        /// <summary>
        /// Collection of path rendering options produced by the renderer.
        /// </summary>
        /// <value>List of <see cref="PathOptions"/> used for drawing column shapes.</value>
        protected List<PathOptions> ColumnPathOptions { get; set; } = [];

        /// <summary>
        /// Accessibility text for the last-calculated point during path generation.
        /// </summary>
        /// <value>Localized or formatted description for assistive technologies.</value>
        protected string? AccessText { get; set; }
        #endregion

        #region Private Methods

        /// <summary>
        /// Computes and assigns side-by-side positions for a collection of series.
        /// </summary>
        /// <param name="seriesCollection">The series collection to evaluate.</param>
        private static void FindRectPosition(List<ChartSeries> seriesCollection)
        {
            Dictionary<string, double> groupingValues = [];
            RectPosition visibleSeries = new() { RectCount = 0, Position = double.NaN };

            AssignPositions(seriesCollection, groupingValues, visibleSeries);

            for (int i = 0; i < seriesCollection.Count; i++)
            {
                seriesCollection[i].Renderer.RectCount = visibleSeries.RectCount;
            }
        }

        /// <summary>
        /// Assigns position indices for each series in the collection.
        /// </summary>
        /// <param name="seriesCollection">Series list to process.</param>
        /// <param name="groupingValues">Dictionary mapping group names to assigned positions.</param>
        /// <param name="visibleSeries">Shared rect count and position state.</param>
        private static void AssignPositions(List<ChartSeries> seriesCollection, Dictionary<string, double> groupingValues, RectPosition visibleSeries)
        {
            for (int i = 0; i < seriesCollection.Count; i++)
            {
                string seriesType = seriesCollection[i].SeriesType ?? null!;
                if (seriesType.Contains("Stacking", StringComparison.InvariantCulture) || !string.IsNullOrEmpty(seriesCollection[i].GroupName))
                {
                    string groupName = seriesType.Contains("Stacking", StringComparison.InvariantCulture) ? seriesCollection[i].StackingGroup : seriesCollection[i].Type + seriesCollection[i].GroupName;
                    if (!string.IsNullOrEmpty(groupName))
                    {
                        if (!groupingValues.TryGetValue(groupName, out double _))
                        {
                            groupingValues.Add(groupName, double.NaN);
                        }

                        if (double.IsNaN(groupingValues[groupName]))
                        {
                            seriesCollection[i].Renderer.Position = visibleSeries.RectCount;
                            groupingValues[groupName] = visibleSeries.RectCount++;
                        }
                        else
                        {
                            seriesCollection[i].Renderer.Position = groupingValues[groupName];
                        }
                    }
                    else
                    {
                        if (double.IsNaN(visibleSeries.Position))
                        {
                            seriesCollection[i].Renderer.Position = visibleSeries.RectCount;
                            visibleSeries.Position = visibleSeries.RectCount++;
                        }
                        else
                        {
                            seriesCollection[i].Renderer.Position = visibleSeries.Position;
                        }
                    }
                }
                else
                {
                    seriesCollection[i].Renderer.Position = visibleSeries.RectCount++;
                }
            }
        }

        /// <summary>
        /// Adds the appropriate hit/test region to the point based on marker and rectangle size.
        /// </summary>
        /// <param name="point">Point to which region is added.</param>
        /// <param name="rect">Bounding rectangle used to derive the region.</param>
        private void GetRegion(Point point, Rect rect)
        {
            double markerHeight = (Series?.Marker.Height > 0) ? Series.Marker.Height : 0;
            if (point.Y is not null && Convert.ToDouble(point.Y, Culture) == 0)
            {
                double markerWidth = (Series?.Marker.Width > 0) ? Series.Marker.Width : 0;
                point.Regions.Add(new Rect(point.SymbolLocations[0].X - markerWidth, point.SymbolLocations[0].Y - markerHeight, 2 * markerWidth, 2 * markerHeight));
            }
            else if (rect.Height < 2 * markerHeight)
            {
                point.Regions.Add(new Rect(rect.X, point.SymbolLocations[0].Y - markerHeight, rect.Width, 2 * markerHeight));
            }
            else
            {
                point.Regions.Add(rect);
            }
        }

        /// <summary>
        /// Ensures side-by-side position information is computed for the series container.
        /// </summary>
        private void GetSideBySidePositions()
        {
            if (Series?.Container?._columnContainer is not null)
            {
                foreach (ChartColumnRenderer columnRenderer in Series.Container._columnContainer.Renderers.Cast<ChartColumnRenderer>())
                {
                    if (Series?.Container?._rowContainer is not null)
                    {
                        foreach (ChartRowRenderer rowRenderer in Series.Container._rowContainer.Renderers.Cast<ChartRowRenderer>())
                        {
                            FindRectPosition(FindSeriesCollection(columnRenderer, rowRenderer));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Creates a path for a rounded rectangle representing a column/bar.
        /// </summary>
        /// <param name="rect">Rectangle bounds.</param>
        /// <param name="topLeft">Top-left corner radius.</param>
        /// <param name="topRight">Top-right corner radius.</param>
        /// <param name="bottomLeft">Bottom-left corner radius.</param>
        /// <param name="bottomRight">Bottom-right corner radius.</param>
        /// <returns>SVG path string for the rounded rectangle.</returns>
        private string CalculateRoundedRectPath(Rect rect, double topLeft, double topRight, double bottomLeft, double bottomRight)
        {
            if (Owner?._seriesContainer is not null && Owner._seriesContainer._hasLargeData)
            {
                return BuildSimplePathForLargeData(rect);
            }

            if (Owner is not null && Owner._isAdaptiveRendering)
            {
                double maxCornerRadius = Math.Min(rect.Width / 2, rect.Height / 2);
                if (topLeft > rect.Width || topRight > rect.Width || bottomLeft > rect.Width || bottomRight > rect.Width)
                {
                    topLeft = topRight = bottomLeft = bottomRight = maxCornerRadius;
                }
            }
            AdjustCornerRadius(ref topLeft, ref topRight, ref bottomRight, ref bottomLeft, rect.Width, rect.Height);
            return BuildRoundedPathString(rect, topLeft, topRight, bottomLeft, bottomRight);
        }

        /// <summary>
        /// Fast path builder used when large-data optimizations are enabled.
        /// </summary>
        /// <param name="rect">Rectangle bounds.</param>
        /// <returns>Simplified path for large data sets.</returns>
        private static string BuildSimplePathForLargeData(Rect rect)
        {
            return "M" + SPACE + Convert.ToInt64(rect.X) + SPACE + Convert.ToInt64(rect.Y) + SPACE + "L " + Convert.ToInt64(rect.X) + SPACE + Convert.ToInt64(rect.Y + rect.Height);
        }

        /// <summary>
        /// Builds the full rounded rectangle path string using culture formatting.
        /// </summary>
        /// <param name="rect">Rectangle bounds.</param>
        /// <param name="topLeft">Top-left radius.</param>
        /// <param name="topRight">Top-right radius.</param>
        /// <param name="bottomLeft">Bottom-left radius.</param>
        /// <param name="bottomRight">Bottom-right radius.</param>
        /// <returns>SVG path string.</returns>
        private string BuildRoundedPathString(Rect rect, double topLeft, double topRight, double bottomLeft, double bottomRight)
        {
            return "M" + SPACE + rect.X.ToString(Culture) + SPACE + (topLeft + rect.Y).ToString(Culture) + " Q " + rect.X.ToString(Culture) + SPACE + rect.Y.ToString(Culture) + SPACE + (rect.X + topLeft).ToString(Culture) + SPACE + rect.Y.ToString(Culture)
                + SPACE + "L" + SPACE + (rect.X + rect.Width - topRight).ToString(Culture) + SPACE + rect.Y.ToString(Culture) + " Q " + (rect.X + rect.Width).ToString(Culture) + SPACE + rect.Y.ToString(Culture) + SPACE + (rect.X + rect.Width).ToString(Culture)
                + SPACE + (rect.Y + topRight).ToString(Culture) + SPACE + "L " + (rect.X + rect.Width).ToString(Culture) + SPACE + (rect.Y + rect.Height - bottomRight).ToString(Culture) + " Q " + (rect.X + rect.Width).ToString(Culture)
                + SPACE + (rect.Y + rect.Height).ToString(Culture) + SPACE + (rect.X + rect.Width - bottomRight).ToString(Culture) + SPACE + (rect.Y + rect.Height).ToString(Culture) + SPACE + "L " + (rect.X + bottomLeft).ToString(Culture)
                + SPACE + (rect.Y + rect.Height).ToString(Culture) + " Q " + rect.X.ToString(Culture) + SPACE + (rect.Y + rect.Height).ToString(Culture) + SPACE + rect.X.ToString(Culture) + SPACE + (rect.Y + rect.Height - bottomLeft).ToString(Culture)
                + SPACE + "L" + SPACE + rect.X.ToString(Culture) + SPACE + (topLeft + rect.Y).ToString(Culture) + SPACE + "Z";
        }

        /// <summary>
        /// Adjusts corner radii so they fit within the rectangle dimensions while preserving proportions.
        /// </summary>
        /// <param name="topLeft">Top-left radius (modified).</param>
        /// <param name="topRight">Top-right radius (modified).</param>
        /// <param name="bottomRight">Bottom-right radius (modified).</param>
        /// <param name="bottomLeft">Bottom-left radius (modified).</param>
        /// <param name="width">Rectangle width.</param>
        /// <param name="height">Rectangle height.</param>
        private static void AdjustCornerRadius(ref double topLeft, ref double topRight, ref double bottomRight, ref double bottomLeft, double width, double height)
        {
            double sumTop = topLeft + topRight;
            double sumBottom = bottomLeft + bottomRight;
            double scaleX = Math.Min(1.0, width / Math.Max(sumTop, sumBottom));

            if (scaleX < 1.0)
            {
                topLeft *= scaleX;
                topRight *= scaleX;
                bottomLeft *= scaleX;
                bottomRight *= scaleX;
            }

            double sumLeft = topLeft + bottomLeft;
            double sumRight = topRight + bottomRight;
            double scaleY = Math.Min(1.0, height / Math.Max(sumLeft, sumRight));

            if (scaleY < 1.0)
            {
                topLeft *= scaleY;
                topRight *= scaleY;
                bottomLeft *= scaleY;
                bottomRight *= scaleY;
            }
        }

        /// <summary>
        /// Maps corner radii for negative Y values so visual corners remain consistent with axis orientation.
        /// </summary>
        /// <param name="yValue">Y value used to determine sign.</param>
        /// <param name="cornerRadius">Original corner radius.</param>
        /// <returns>Adjusted corner radius for negative values.</returns>
        private CornerRadius GetCornerRadius(double yValue, CornerRadius cornerRadius)
        {
            return yValue < 0
                ? Owner is not null && !Owner._requireInvertedAxis
                    ? new CornerRadius
                    {
                        BottomLeft = cornerRadius.TopLeft,
                        BottomRight = cornerRadius.TopRight,
                        TopLeft = cornerRadius.BottomLeft,
                        TopRight = cornerRadius.BottomRight
                    }
                    : new CornerRadius
                    {
                        BottomLeft = cornerRadius.BottomRight,
                        BottomRight = cornerRadius.BottomLeft,
                        TopLeft = cornerRadius.TopRight,
                        TopRight = cornerRadius.TopLeft
                    }
                : cornerRadius;
        }

        /// <summary>
        /// Calculates and returns an adjusted double range for a series group based on the specified range, minimum
        /// point delta, and width.
        /// </summary>
        /// <param name="doubleRange">The original range to be adjusted. Represents the start and end values for the series group.</param>
        /// <param name="minimumPointDelta">The minimum distance between points in the series group. Used to scale the range according to column width.</param>
        /// <param name="width">The width to apply when calculating the adjustment for the range. Must be a positive value.</param>
        /// <returns>A DoubleRange representing the adjusted start and end values for the series group, scaled and offset
        /// according to the provided parameters.</returns>
        private DoubleRange UpdateSeriesGroupDoubleRange(DoubleRange doubleRange, double minimumPointDelta, double width)
        {
            double mainColumnWidth = 0.7;

            Series?.Container?._seriesContainer?.Renderers.ForEach(renderer =>
            {
                if ((renderer as ChartSeriesRenderer ?? null!).Series?.ColumnWidth > mainColumnWidth)
                {
                    mainColumnWidth = (renderer as ChartSeriesRenderer ?? null!).Series?.ColumnWidth ?? 0;
                }
            });
            double mainWidth = minimumPointDelta * mainColumnWidth;
            DoubleRange mainDoubleRange = new(doubleRange.Start * mainWidth, doubleRange.End * mainWidth);
            double difference = (mainDoubleRange.Delta - ((doubleRange.End * width) - (doubleRange.Start * width))) / 2;
            return new DoubleRange(mainDoubleRange.Start + difference, mainDoubleRange.End - difference);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Computes side-by-side width/location information for the current series.
        /// </summary>
        /// <returns>Calculated <see cref="DoubleRange"/> representing the start and end offsets.</returns>
        protected DoubleRange GetSideBySideInfo()
        {
            if (Series?.Container is not null && Series.Container.EnableSideBySidePlacement && (Series.Renderer.Position == 0 || double.IsNaN(Series.Renderer.Position)))
            {
                GetSideBySidePositions();
            }

            if (Series?.ColumnWidthInPixel > 0)
            {
                return new DoubleRange(0, 0);
            }

            double rectCount = Series?.Container is not null && !Series.Container.EnableSideBySidePlacement ? 1 : Series?.Renderer.RectCount ?? 0;

            if (Owner is not null && XAxisRenderer is null)
            {
                Owner.InitiAxis();
            }

            double minimumPointDelta = ChartHelper.GetMinPointsDelta(XAxisRenderer?.Axis ?? null!, Series?.Container?._seriesContainer?.Renderers.Cast<ChartSeriesRenderer>().ToList() ?? null!);
            double width = GetColumnWidth(minimumPointDelta);
            double location = ((Series?.Container is not null && !Series.Container.EnableSideBySidePlacement ? 0 : Series?.Renderer.Position ?? 0) / rectCount) - 0.5;
            DoubleRange doubleRange = new(location, location + (1 / rectCount));

            if (!(double.IsNaN(doubleRange.Start) || double.IsNaN(doubleRange.End)))
            {
                doubleRange = !string.IsNullOrEmpty(Series?.GroupName) && Series.SeriesType is not null && !Series.SeriesType.Contains("Stacking", StringComparison.InvariantCulture)
                    ? UpdateSeriesGroupDoubleRange(doubleRange, minimumPointDelta, width)
                    : new DoubleRange(doubleRange.Start * width, doubleRange.End * width);

                double radius = (Series?.Container is not null && Series.Container.EnableSideBySidePlacement ? Series.ColumnSpacing : 0) * doubleRange.Delta;
                doubleRange = new DoubleRange(doubleRange.Start + (radius / 2), doubleRange.End - (radius / 2));
            }
            return doubleRange;
        }

        /// <summary>
        /// Computes a rectangle in pixels from data coordinates (x1,y1)-(x2,y2).
        /// </summary>
        /// <param name="x1">First X data value.</param>
        /// <param name="y1">First Y data value.</param>
        /// <param name="x2">Second X data value.</param>
        /// <param name="y2">Second Y data value.</param>
        /// <returns>Pixel-space rectangle.</returns>
        protected Rect GetRectangle(double x1, double y1, double x2, double y2)
        {
            ChartEventLocation point1 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(x1), YAxisRenderer.GetPointValue(y1), XAxisRenderer, YAxisRenderer, Series?.Container?._requireInvertedAxis ?? false);
            ChartEventLocation point2 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(x2), YAxisRenderer.GetPointValue(y2), XAxisRenderer, YAxisRenderer, Series?.Container?._requireInvertedAxis ?? false);
            return new Rect(Math.Min(point1.X, point2.X), Math.Min(point1.Y, point2.Y), Math.Abs(point2.X - point1.X), Math.Abs(point2.Y - point1.Y));
        }

        /// <summary>
        /// Triggers the point render event and applies customizations from event args.
        /// </summary>
        /// <param name="point">Point being rendered.</param>
        /// <param name="fill">Fill color passed to the event.</param>
        /// <param name="border">Border model passed to the event.</param>
        /// <returns>Constructed <see cref="PointRenderEventArgs"/> or null when inputs are invalid.</returns>
        protected PointRenderEventArgs TriggerEvent(Point point, string fill, BorderModel border)
        {
            if (point is not null && border is not null)
            {
                PointRenderEventArgs argsData = new(Constants.PointRender, false, point, Series ?? null!, SetPointColor(point, fill), SetBorderColor(point, new ChartEventBorder() { Color = border.Color, Width = border.Width }))
                {
                    CornerRadius = new CornerRadius
                    {
                        TopLeft = Series?.CornerRadius.TopLeft ?? 0,
                        TopRight = Series?.CornerRadius.TopRight ?? 0,
                        BottomLeft = Series?.CornerRadius.BottomLeft ?? 0,
                        BottomRight = Series?.CornerRadius.BottomRight ?? 0
                    }
                };

                if (Series?.Container?.OnPointRender is not null)
                {
                    Series.Container.OnPointRender.Invoke(argsData);
                }
                if (ChartPoints is not null)
                {
                    point.Interior = ChartPoints[point.Index].Interior = argsData.Fill;
                }

                point.Visible = !argsData.Cancel;
                return argsData;
            }

            return null!;
        }

        /// <summary>
        /// Updates symbol location and region collections for a point based on computed rectangle.
        /// </summary>
        /// <param name="point">Point to update.</param>
        /// <param name="rect">Bounding rectangle used to compute symbol position and hit region.</param>
        protected void UpdateSymbolLocation(Point point, Rect rect)
        {
            if (point is not null)
            {
                if (ChartPoints is not null)
                {
                    ChartPoints[point.Index].SymbolLocations = [];
                    ChartPoints[point.Index].Regions = [];
                }
                if (Series?.Container is not null && !Series.Container._requireInvertedAxis)
                {
                    UpdateXRegion(point, rect);
                }
                else
                {
                    UpdateYRegion(point, rect);
                }

                point.SymbolLocations.ForEach(loc =>
                {
                    ChartPoints?[point.Index]?.SymbolLocations.Add(new IChartInternalLocation(Math.Round(loc.X, 2), Math.Round(loc.Y, 2)));
                });
                point.Regions.ForEach(rect =>
                {
                    ChartPoints?[point.Index]?.Regions.Add(new IRect(Math.Round(rect.X, 2), Math.Round(rect.Y, 2), rect.Width, rect.Height));
                });
            }
        }

        /// <summary>
        /// Update a point's symbol location and region when axis is not inverted.
        /// </summary>
        /// <param name="point">Point to update.</param>
        /// <param name="rect">Bounding rectangle.</param>
        protected void UpdateXRegion(Point point, Rect rect)
        {
            if (point is not null && rect is not null)
            {
                point.SymbolLocations.Add(new ChartEventLocation(rect.X + (rect.Width / 2), (SeriesType() == SeriesValueType.BoxPlot || SeriesType().ToString().Contains("HighLow", StringComparison.InvariantCulture) || ((point.YValue >= 0) == !YAxisRenderer.Axis?.IsAxisInverse)) ? rect.Y : (rect.Y + rect.Height)));
                GetRegion(point, rect);
            }
        }

        /// <summary>
        /// Update a point's symbol location and region when axis is inverted.
        /// </summary>
        /// <param name="point">Point to update.</param>
        /// <param name="rect">Bounding rectangle.</param>
        protected void UpdateYRegion(Point point, Rect rect)
        {
            if (point is not null && rect is not null)
            {
                point.SymbolLocations.Add(new ChartEventLocation((SeriesType() == SeriesValueType.BoxPlot || SeriesType().ToString().Contains("HighLow", StringComparison.InvariantCulture) || ((point.YValue >= 0) == !YAxisRenderer.Axis?.IsAxisInverse)) ? rect.X + rect.Width : rect.X, rect.Y + (rect.Height / 2)));
                GetRegion(point, rect);
            }
        }

        /// <summary>
        /// Calculates path for a rectangle (rounded if required), computes accessibility text and returns the SVG path element string.
        /// </summary>
        /// <param name="point">Point associated with the rectangle.</param>
        /// <param name="rect">Rectangle bounds.</param>
        /// <param name="id">Id used for path element.</param>
        /// <param name="seriesCornerRadius">Optional series-wide corner radius to apply.</param>
        /// <returns>SVG path element string or null when inputs are invalid.</returns>
        protected string CalculateRectangle(Point point, Rect rect, string id, CornerRadius seriesCornerRadius = null!)
        {
            if (point is not null && rect is not null)
            {
                if ((Series?.Container is not null && Series.Container._requireInvertedAxis ? rect.Height : rect.Width) <= 0)
                {
                    return null!;
                }

                string direction;
                if ((point.Y is not null && Convert.ToDouble(point.Y, Culture) == 0) || (Series is not null && !Series.Visible && Series._isLegendClicked))
                {
                    // For 0 values corner radius will not calculate
                    direction = CalculateRoundedRectPath(rect, 0, 0, 0, 0);
                }
                else
                {
                    CornerRadius cornerRadius = seriesCornerRadius is not null ? GetCornerRadius(Convert.ToDouble(point.Y, Culture), seriesCornerRadius) : new CornerRadius();
                    direction = CalculateRoundedRectPath(rect, cornerRadius.TopLeft, cornerRadius.TopRight, cornerRadius.BottomLeft, cornerRadius.BottomRight);
                }

                AccessText = string.Empty;
                if (SeriesType() == SeriesValueType.XY)
                {
                    AccessText = !string.IsNullOrEmpty(Series?.AccessibilityDescriptionFormat) ? GetPointDescriptionFormatText(point) : XAxisRenderer.GetFormatText(GetPointXValue(point.X, XAxisRenderer.DateFormat ?? string.Empty)) + ":" + YAxisRenderer.GetFormatText(GetPointXValue(point.YValue, YAxisRenderer.DateFormat ?? string.Empty)) + ", " + Series?.Name;
                }
                else if (SeriesType() == SeriesValueType.HighLow)
                {
                    FinancialPoint financialPoint = point as FinancialPoint ?? null!;
                    AccessText = !string.IsNullOrEmpty(Series?.AccessibilityDescriptionFormat) ? GetPointDescriptionFormatText(point) : XAxisRenderer.GetFormatText(GetPointXValue(financialPoint.X, XAxisRenderer.DateFormat ?? string.Empty)) + ":" + YAxisRenderer.GetFormatText(GetPointXValue(financialPoint.High, YAxisRenderer.DateFormat ?? string.Empty)) + ", " + YAxisRenderer.GetFormatText(GetPointXValue(financialPoint.Low, YAxisRenderer.DateFormat ?? string.Empty)) + ", " + Series?.Name;
                }
                else if (SeriesType() == SeriesValueType.BoxPlot)
                {
                    BoxPoint boxPoint = point as BoxPoint ?? null!;
                    AccessText = !string.IsNullOrEmpty(Series?.AccessibilityDescriptionFormat) ? GetPointDescriptionFormatText(point) : XAxisRenderer.GetFormatText(GetPointXValue(boxPoint.X, XAxisRenderer.DateFormat ?? string.Empty)) + ":" + YAxisRenderer.GetFormatText(GetPointXValue(boxPoint.Maximum, YAxisRenderer.DateFormat ?? string.Empty)) + ":" + YAxisRenderer.GetFormatText(GetPointXValue(boxPoint.Minimum, YAxisRenderer.DateFormat ?? string.Empty)) + ":" + YAxisRenderer.GetFormatText(GetPointXValue(boxPoint.LowerQuartile, YAxisRenderer.DateFormat ?? string.Empty)) + ":" + YAxisRenderer.GetFormatText(GetPointXValue(boxPoint.UpperQuartile, YAxisRenderer.DateFormat ?? string.Empty));
                }

                return ChartHelper.AppendPathElements(Series?.Container ?? null!, direction, id, SeriesElementId());
            }

            return null!;
        }

        /// <summary>
        /// Prepares animation options for the series if animations are enabled.
        /// </summary>
        protected virtual void Animate()
        {
            if (ShouldAnimate())
            {
                AnimationOptions = new AnimationOptions(SeriesElementId(), AnimationType.Rect);
            }
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Creates a pixel-space rectangle adjusted for series ColumnWidthInPixel when applicable.
        /// </summary>
        /// <param name="rectangle">Original rectangle computed from data points.</param>
        /// <returns>Adjusted rectangle accounting for explicit pixel width settings.</returns>
        internal Rect GetColumnWidthInPixelRect(Rect rectangle)
        {
            if (Series is not null && !double.IsNaN(Series.ColumnWidthInPixel) && Series.ColumnWidthInPixel > 0)
            {
                double columnWidth = Series.ColumnWidthInPixel;
                bool isTransposed = Owner?.IsTransposed ?? false;
                double halfColumnWidthAdjustment = columnWidth / 2 * (double.IsNaN(Series.Renderer.RectCount) ? 0 : Series.Renderer.RectCount);
                double positionAdjustment = columnWidth * (double.IsNaN(Series.Renderer.Position) ? 0 : Series.Renderer.Position);

                return Series.Type switch
                {
                    ChartSeriesType.Bar or ChartSeriesType.StackingBar or ChartSeriesType.StackingBar100 => isTransposed
                                                ? new Rect(rectangle.X - (halfColumnWidthAdjustment - positionAdjustment), rectangle.Y, columnWidth, rectangle.Height)
                                                : new Rect(rectangle.X, rectangle.Y - (halfColumnWidthAdjustment - (columnWidth * ((double.IsNaN(Series.Renderer.RectCount) ? 0 : Series.Renderer.RectCount) - (double.IsNaN(Series.Renderer.Position) ? 0 : Series.Renderer.Position) - 1))), rectangle.Width, columnWidth),
                    ChartSeriesType.Column or ChartSeriesType.StackingColumn or ChartSeriesType.StackingColumn100 => isTransposed
                                                ? new Rect(rectangle.X, rectangle.Y - (halfColumnWidthAdjustment - (columnWidth * ((double.IsNaN(Series.Renderer.RectCount) ? 0 : Series.Renderer.RectCount) - (double.IsNaN(Series.Renderer.Position) ? 0 : Series.Renderer.Position) - 1))), rectangle.Width, columnWidth)
                                                : new Rect(rectangle.X - (halfColumnWidthAdjustment - positionAdjustment), rectangle.Y, columnWidth, rectangle.Height),
                    ChartSeriesType.Line => new Rect(),
                    ChartSeriesType.Area => new Rect(),
                    ChartSeriesType.StackingArea => new Rect(),
                    ChartSeriesType.StackingLine => new Rect(),
                    ChartSeriesType.StackingStepArea => new Rect(),
                    ChartSeriesType.StepLine => new Rect(),
                    ChartSeriesType.StepArea => new Rect(),
                    ChartSeriesType.SplineArea => new Rect(),
                    ChartSeriesType.Scatter => new Rect(),
                    ChartSeriesType.Spline => new Rect(),
                    ChartSeriesType.StackingLine100 => new Rect(),
                    ChartSeriesType.StackingArea100 => new Rect(),
                    ChartSeriesType.Bubble => new Rect(),
                    ChartSeriesType.MultiColoredLine => new Rect(),
                    ChartSeriesType.MultiColoredArea => new Rect(),
                    _ => rectangle,
                };
            }
            else
            {
                return rectangle;
            }
        }

        /// <summary>
        /// Applies customization changes (styles/visibility) to all path options managed by the renderer.
        /// </summary>
        /// <param name="property">Property name that changed.</param>
        internal override void UpdateCustomization(string property)
        {
            base.UpdateCustomization(property);
            string visibility = ((Series is not null && Series.Animation.Enable && SyncfusionService?._options.Animation == GlobalAnimationMode.Default) || (SyncfusionService?._options.Animation == GlobalAnimationMode.Enable)) && Owner is not null && Owner._shouldAnimateSeries ? "hidden" : "visible";
            ColumnPathOptions.ForEach(option => option.Visibility = visibility);

            switch (property)
            {
                case "Fill":
                    ColumnPathOptions.ForEach(option => option.Fill = Interior ?? string.Empty);
                    break;
                case "DashArray":
                    ColumnPathOptions.ForEach(option => option.StrokeDashArray = Series?.DashArray ?? string.Empty);
                    break;
                case "Width":
                    ColumnPathOptions.ForEach(option => option.StrokeWidth = Series?.Border.Width ?? 0);
                    break;
                case "Color":
                    ColumnPathOptions.ForEach(option => option.Stroke = Series?.Border.Color ?? string.Empty);
                    break;
                case "Opacity":
                    ColumnPathOptions.ForEach(option => option.Opacity = Series?.Opacity ?? 1);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Identifies this renderer as a rectangular series renderer.
        /// </summary>
        /// <returns><see langword="true"/> for rect-based series.</returns>
        internal override bool IsRectSeries()
        {
            return true;
        }

        #endregion
    }
}
