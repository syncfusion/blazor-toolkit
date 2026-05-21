using Microsoft.AspNetCore.Components.Rendering;
using System.Text.Json;
using System.Dynamic;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renders bubble series in chart components, calculating bubble positions, sizes, and visual properties.
    /// </summary>
    /// <remarks>
    /// The <see cref="BubbleSeriesRenderer"/> handles bubble point calculations, symbol rendering, and dynamic data processing
    /// for multiple data source types (ExpandoObject, DynamicObject, JSON, and typed objects).
    /// </remarks>
    internal class BubbleSeriesRenderer : ChartSeriesRenderer
    {
        #region Fields
        private List<SymbolOptions> _symbolOptions = [];
        #endregion

        #region Private Methods

        /// <summary>
        /// Calculates bubble positions, sizes, and visual properties for all visible points.
        /// </summary>
        private void CalculateDirection()
        {
            _symbolOptions.Clear();
            ChartData = new System.Text.StringBuilder();

            (double minRadius, double radius, double maximumSize) = CalculateRadiusAndSize();

            List<Point> points = Series?.Renderer.Points ?? null!;
            foreach (BubblePoint bubblePoint in points.Cast<BubblePoint>())
            {
                ProcessBubblePoint(bubblePoint, minRadius, radius, maximumSize);
            }
        }

        /// <summary>
        /// Calculates the maximum radius, minimum radius, effective radius, and maximum size for bubbles.
        /// </summary>
        /// <returns>A tuple containing (maxRadius, minRadius, radius, maximumSize).</returns>
        private (double minRadius, double radius, double maximumSize) CalculateRadiusAndSize()
        {
            double clipValue = Math.Max(Owner?.InitialRect.Height ?? 0, Owner?.InitialRect.Width ?? 0);
            double percentChange = clipValue / 100;
            double maxRadius = (Series?.MaxRadius ?? 1) * percentChange;
            double minRadius = (Series?.MinRadius ?? 1) * percentChange;
            double maximumSize = 0;
            double radius;

            if (Owner?._seriesContainer is not null && Series is not null && (double.IsNaN(Series.MaxRadius) || double.IsNaN(Series.MinRadius)))
            {
                foreach (ChartSeries series in Owner._seriesContainer.Elements.Cast<ChartSeries>())
                {
                    if (series.Type == ChartSeriesType.Bubble && series.Visible && (double.IsNaN(series.MaxRadius) || double.IsNaN(series.MinRadius)))
                    {
                        maximumSize = series.Renderer.MaxSize > maximumSize ? series.Renderer.MaxSize : maximumSize;
                    }
                }

                minRadius = maxRadius = 1;
                radius = clipValue / 5 / 2 * maxRadius;
            }
            else
            {
                maximumSize = Series?.Renderer.MaxSize ?? 0;
                radius = maxRadius - minRadius;
            }
            return (minRadius, radius, maximumSize);
        }

        /// <summary>
        /// Processes a single bubble point: calculates its size, visibility, and renders the symbol.
        /// </summary>
        /// <param name="bubblePoint">The bubble point to process.</param>
        /// <param name="minRadius">The minimum bubble radius.</param>
        /// <param name="radius">The effective radius range.</param>
        /// <param name="maximumSize">The maximum bubble size in the dataset.</param>
        private void ProcessBubblePoint(BubblePoint bubblePoint, double minRadius, double radius, double maximumSize)
        {
            bubblePoint.SymbolLocations = [];
            bubblePoint.Regions = [];

            if (ChartPoints is not null)
            {
                ChartPoints[bubblePoint.Index].SymbolLocations = [];
                ChartPoints[bubblePoint.Index].Regions = [];
            }

            bool isPointInRange = ChartHelper.WithInRange(bubblePoint.Index - 1 > -1 ? Series?.Renderer.Points?[bubblePoint.Index - 1] ?? null! : null!, bubblePoint, bubblePoint.Index + 1 < Series?.Renderer.Points?.Count ? Series.Renderer.Points[bubblePoint.Index + 1] : null!, XAxisRenderer);

            if (bubblePoint.Visible && isPointInRange)
            {
                double segmentRadius = CalculateSegmentRadius(bubblePoint, minRadius, radius, maximumSize);
                RenderBubblePoint(bubblePoint, segmentRadius);
            }
            if (IsTooltipEnabled() && bubblePoint.SymbolLocations.Count > 0)
            {
                AppendChartData(ChartPoints?[bubblePoint.Index] as IBubblePoint);
            }
        }

        /// <summary>
        /// Calculates the radius for a single bubble segment based on its size relative to the maximum.
        /// </summary>
        /// <param name="bubblePoint">The bubble point.</param>
        /// <param name="minRadius">The minimum radius.</param>
        /// <param name="radius">The effective radius range.</param>
        /// <param name="maximumSize">The maximum bubble size in the dataset.</param>
        /// <returns>The calculated segment radius.</returns>
        private double CalculateSegmentRadius(BubblePoint bubblePoint, double minRadius, double radius, double maximumSize)
        {
            double segmentRadius = double.IsNaN(Series?.MaxRadius ?? 0) || double.IsNaN(Series?.MinRadius ?? 0)
                ? (bubblePoint.Size is not null) ? radius * Math.Abs((double)bubblePoint.Size / maximumSize) : 0
                : (bubblePoint.Size is not null) ? minRadius + (radius * Math.Abs((double)bubblePoint.Size / maximumSize)) : 0;
            segmentRadius = segmentRadius != 0 && !double.IsNaN(segmentRadius) ? segmentRadius : minRadius;
            return segmentRadius;
        }

        /// <summary>
        /// Renders a bubble point with the specified radius, handling events and visual properties.
        /// </summary>
        /// <param name="bubblePoint">The bubble point to render.</param>
        /// <param name="segmentRadius">The calculated segment radius.</param>
        private void RenderBubblePoint(BubblePoint bubblePoint, double segmentRadius)
        {
            PointRenderEventArgs argsData = CreatePointRenderArgs(bubblePoint, segmentRadius);

            if (Owner?.OnPointRender is not null)
            {
                Owner.OnPointRender.Invoke(argsData);
                segmentRadius = argsData.Height / 2;
            }

            if (!argsData.Cancel)
            {
                RenderBubbleSymbol(bubblePoint, segmentRadius, argsData);
            }
            else
            {
                bubblePoint.Marker = new MarkerSettingModel { Visible = false };
                if (ChartPoints is { })
                {
                    ChartPoints[bubblePoint.Index].Marker = new IMarkerSettingModel() { Visible = false };
                }
            }
        }

        /// <summary>
        /// Creates event arguments for a point render event.
        /// </summary>
        /// <param name="bubblePoint">The bubble point.</param>
        /// <param name="segmentRadius">The segment radius.</param>
        /// <returns>The point render event arguments.</returns>
        private PointRenderEventArgs CreatePointRenderArgs(BubblePoint bubblePoint, double segmentRadius)
        {
            return new PointRenderEventArgs
            (
                Constants.PointRender,
                false,
                bubblePoint,
                Series ?? null!,
                SetPointColor(bubblePoint, Interior ?? null!),
                SetBorderColor(bubblePoint, new ChartEventBorder { Width = Series?.Border.Width ?? 0, Color = Series?.Border.Color ?? string.Empty }),
                2 * segmentRadius,
                2 * segmentRadius,
                ChartShape.Circle
            );
        }

        /// <summary>
        /// Renders the bubble symbol and updates its regions and marker settings.
        /// </summary>
        /// <param name="bubblePoint">The bubble point.</param>
        /// <param name="segmentRadius">The segment radius.</param>
        /// <param name="argsData">The point render event arguments containing visual properties.</param>
        private void RenderBubbleSymbol(BubblePoint bubblePoint, double segmentRadius, PointRenderEventArgs argsData)
        {
            bubblePoint.SymbolLocations.Add(ChartHelper.GetPoint(XAxisRenderer.GetPointValue(bubblePoint.XValue), YAxisRenderer.GetPointValue(bubblePoint.YValue), XAxisRenderer, YAxisRenderer, XLength, YLength, Owner is not null && Owner._requireInvertedAxis));
            bubblePoint.SymbolLocations.ForEach(loc =>
            {
                ChartPoints?[bubblePoint.Index]?.SymbolLocations.Add(new IChartInternalLocation(Math.Round(loc.X, 2), Math.Round(loc.Y, 2)));
            });
            bubblePoint.Interior = argsData.Fill;

            PathOptions shapeOption = new()
            {
                Id = Owner?.ID + "_Series_" + Series?.Renderer.Index + "_Point_" + bubblePoint.Index,
                Fill = argsData.Fill,
                StrokeWidth = argsData.Border.Width,
                Stroke = argsData.Border.Color,
                Opacity = Series?.Opacity ?? 1,
                DataPoint = GetDataPoints(bubblePoint.XValue, bubblePoint.YValue),
                AccessibilityText = GetPointDescriptionFormatText(bubblePoint),
                Visibility = string.Empty
            };

            SymbolOptions symbol = ChartMarkerRenderer.CalculateSymbol
            (
                bubblePoint.SymbolLocations[0],
                ChartShape.Circle.ToString(),
                new Size(argsData.Width, argsData.Height),
                Series?.Marker.ImageUrl?.ToString() ?? string.Empty,
                shapeOption,
                Owner ?? null!
            );

            bubblePoint.Regions.Add
            (
                new Rect
                (
                    bubblePoint.SymbolLocations[0].X - segmentRadius,
                    bubblePoint.SymbolLocations[0].Y - segmentRadius,
                    2 * segmentRadius,
                    2 * segmentRadius
                )
            );

            bubblePoint.Regions.ForEach(rect =>
            {
                ChartPoints?[bubblePoint.Index]?.Regions.Add(new IRect(Math.Round(rect.X, 2), Math.Round(rect.Y, 2), rect.Width, rect.Height));
            });

            UpdateMarkerSettings(bubblePoint, argsData);
            _symbolOptions.Add(symbol);
        }

        /// <summary>
        /// Updates marker settings for the bubble point in both public and internal representations.
        /// </summary>
        /// <param name="bubblePoint">The bubble point.</param>
        /// <param name="argsData">The point render event arguments.</param>
        private void UpdateMarkerSettings(BubblePoint bubblePoint, PointRenderEventArgs argsData)
        {
            bubblePoint.Marker = new MarkerSettingModel
            {
                Border = argsData.Border,
                Fill = argsData.Fill,
                Height = argsData.Height,
                Visible = true,
                Shape = ChartShape.Circle,
                Width = argsData.Width
            };

            if (ChartPoints is { })
            {
                ChartPoints[bubblePoint.Index].Marker = new IMarkerSettingModel()
                {
                    Border = new IChartEventBorder() { Color = argsData.Border.Color, Width = argsData.Border.Width },
                    Fill = argsData.Fill,
                    Height = argsData.Height,
                    Width = argsData.Width,
                    Shape = argsData.Shape.ToString(),
                    Visible = true
                };
            }
        }

        /// <summary>
        /// Updates the stroke color for all rendered symbols.
        /// </summary>
        private void UpdateShapeColor()
        {
            string borderColor = Series?.Border.Color ?? string.Empty;
            foreach (SymbolOptions symbolOption in _symbolOptions)
            {
                if (symbolOption.ShapeName == ShapeName.Ellipse)
                {
                    symbolOption.EllipseOption.Stroke = borderColor;
                }
                else if (symbolOption.ShapeName == ShapeName.Path)
                {
                    symbolOption.PathOption.Stroke = borderColor;
                }
            }
        }

        /// <summary>
        /// Updates the fill color for all rendered symbols.
        /// </summary>
        private void UpdateShapeFill()
        {
            string fill = Interior ?? string.Empty;
            foreach (SymbolOptions symbolOption in _symbolOptions)
            {
                if (symbolOption.ShapeName == ShapeName.Ellipse)
                {
                    symbolOption.EllipseOption.Fill = fill;
                }
                else if (symbolOption.ShapeName == ShapeName.Path)
                {
                    symbolOption.PathOption.Fill = fill;
                }
            }
        }

        /// <summary>
        /// Updates the opacity for all rendered symbols.
        /// </summary>
        private void UpdateShapeOpacity()
        {
            double opacity = Series?.Opacity ?? 1;
            foreach (SymbolOptions symbolOption in _symbolOptions)
            {
                if (symbolOption.ShapeName == ShapeName.Ellipse)
                {
                    symbolOption.EllipseOption.Opacity = opacity;
                }
                else if (symbolOption.ShapeName == ShapeName.Path)
                {
                    symbolOption.PathOption.Opacity = opacity;
                }
            }
        }

        /// <summary>
        /// Determines whether data sorting is enabled for non-X axis properties.
        /// </summary>
        /// <returns><see langword="true"/> if sorting is enabled and not by X axis; otherwise <see langword="false"/>.</returns>
        private bool IsDataSortingEnabled()
        {
            return !string.IsNullOrEmpty(Owner?._sorting.PropertyName) && !Owner._sorting.PropertyName.Equals("X", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Extracts and processes bubble point data from an ExpandoObject.
        /// </summary>
        private void ExtractAndProcessExpandoPointData(IDictionary<string, object> expandoData, Type firstDataType, string xName, string yName, string tempSize, string pointColor, string tooltipText, int index, bool isSortingEnabled)
        {
            _ = expandoData.TryGetValue(xName, out object? x);
            _ = expandoData.TryGetValue(yName, out object? y);
            _ = expandoData.TryGetValue(tempSize, out object? size);
            _ = expandoData.TryGetValue(pointColor, out object? color);
            _ = expandoData.TryGetValue(tooltipText, out object? tooltip);

            IBubblePoint chartPoint = new();
            BubblePoint point = new()
            {
                X = chartPoint.X = x ?? null!,
                Y = chartPoint.Y = y ?? null!,
                Size = chartPoint.Size = size ?? null!,
                Interior = chartPoint.Interior = Convert.ToString(color, Culture) ?? null!,
                Text = chartPoint.Text = Convert.ToString(GetTextMapping(), Culture) ?? null!,
                Tooltip = chartPoint.Tooltip = Convert.ToString(tooltip, Culture) ?? null!
            };

            GetSetXValue(point, chartPoint, index);
            SetEmptyPoint(point, chartPoint, index, firstDataType);

            if (isSortingEnabled)
            {
                FindExpandoObjectDataSortingValue(Owner?._sorting.PropertyName ?? string.Empty, expandoData, point.X.ToString() ?? string.Empty, point);
            }
        }

        /// <summary>
        /// Extracts and processes bubble point data from a DynamicObject.
        /// </summary>
        private void ExtractAndProcessDynamicPointData(DynamicObject dynamicObject, Type firstDataType, string xName, string yName, string size, string pointColor, int index, bool isSortingEnabled)
        {
            IBubblePoint chartPoint = new();
            BubblePoint point = new()
            {
                X = chartPoint.X = ReflectionExtension.GetValueFromDynamicObject(dynamicObject, xName) ?? 0,
                Y = chartPoint.Y = ReflectionExtension.GetValueFromDynamicObject(dynamicObject, yName) ?? 0,
                Size = chartPoint.Size = !string.IsNullOrEmpty(size) ? ReflectionExtension.GetValueFromDynamicObject(dynamicObject, size) : null!,
                Interior = chartPoint.Interior = ChartHelper.GetDynamicStringValue(dynamicObject ?? null!, pointColor),
                Text = chartPoint.Text = ChartHelper.GetDynamicStringValue(dynamicObject ?? null!, GetTextMapping()),
                Tooltip = chartPoint.Tooltip = ChartHelper.GetDynamicStringValue(dynamicObject ?? null!, Series?.TooltipMappingName ?? null!)
            };

            GetSetXValue(point, chartPoint, index);
            SetEmptyPoint(point, chartPoint, index, firstDataType);

            if (isSortingEnabled)
            {
                FindDynamicObjectDataSortingValue(Owner?._sorting.PropertyName ?? string.Empty, dynamicObject ?? null!, point.X?.ToString() ?? null!, point);
            }
        }

        /// <summary>
        /// Extracts and processes bubble point data from a JsonElement.
        /// </summary>
        private void ExtractAndProcessJsonPointData(JsonElement jsonObject, Type firstDataType, string xName, string yName, string size, string pointColor, int index, bool isSortingEnabled)
        {
            IBubblePoint chartPoint = new();
            BubblePoint point = new()
            {
                X = chartPoint.X = ChartHelper.GetObjectValue(jsonObject.GetProperty(xName)),
                Y = chartPoint.Y = jsonObject.GetProperty(yName).GetDouble(),
                Size = chartPoint.Size = jsonObject.GetProperty(size).GetDouble(),
                Interior = chartPoint.Interior = Convert.ToString(jsonObject.GetProperty(pointColor).GetString(), Culture) ?? string.Empty,
                Text = chartPoint.Text = Convert.ToString(jsonObject.GetProperty(GetTextMapping()).GetString(), Culture) ?? string.Empty,
                Tooltip = chartPoint.Tooltip = Convert.ToString(jsonObject.GetProperty(Series?.TooltipMappingName ?? null!).GetString(), Culture) ?? string.Empty
            };

            GetSetXValue(point, chartPoint, index);
            SetEmptyPoint(point, chartPoint, index, firstDataType);

            if (isSortingEnabled)
            {
                FindJObjectDataSortingValue(Owner?._sorting.PropertyName ?? string.Empty, jsonObject, point.X.ToString() ?? string.Empty, point);
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Renders series elements to the provided render tree builder.
        /// </summary>
        protected override void RenderSeries()
        {
            base.RenderSeries();
            CalculateDirection();

            if (Owner is not null && Owner._shouldAnimateSeries && ((Series is not null && Series.Animation.Enable && SyncfusionService?._options.Animation == GlobalAnimationMode.Default) || (SyncfusionService?._options.Animation == GlobalAnimationMode.Enable)))
            {
                AnimationOptions = new AnimationOptions(SeriesElementId(), AnimationType.Marker);
            }
        }

        /// <summary>
        /// Processes ExpandoObject data source and extracts bubble point information.
        /// </summary>
        /// <param name="firstDataType">The first data type in the collection for type inference.</param>
        /// <param name="xName">The property name for X-axis values.</param>
        /// <param name="yName">The property name for Y-axis values.</param>
        /// <param name="currentViewData">The enumerable collection of data objects to process.</param>
        protected override void ProcessExpandoObjectData(Type firstDataType, string xName, string yName, IEnumerable<object> currentViewData)
        {
            if (CurrentViewData is null)
            {
                return;
            }

            string tempSize = Series?.Size ?? null!;
            string pointColor = Series?.PointColorMapping ?? null!;
            string tooltipText = Series?.TooltipMappingName ?? null!;
            bool isSortingEnabled = IsDataSortingEnabled();
            int index = 0;

            foreach (object data in CurrentViewData)
            {
                IDictionary<string, object> expandoData = (IDictionary<string, object>)data;
                ExtractAndProcessExpandoPointData(expandoData, firstDataType, xName, yName, tempSize, pointColor, tooltipText, index, isSortingEnabled);
                index++;
            }
        }

        /// <summary>
        /// Processes DynamicObject data source and extracts bubble point information.
        /// </summary>
        /// <param name="firstDataType">The first data type in the collection for type inference.</param>
        /// <param name="xName">The property name for X-axis values.</param>
        /// <param name="yName">The property name for Y-axis values.</param>
        /// <param name="currentViewData">The enumerable collection of data objects to process.</param>
        protected override void ProcessDynamicObjectData(Type firstDataType, string xName, string yName, IEnumerable<object> currentViewData)
        {
            if (CurrentViewData is null)
            {
                return;
            }

            string size = Series?.Size ?? string.Empty;
            string pointColor = Series?.PointColorMapping ?? string.Empty;
            bool isSortingEnabled = IsDataSortingEnabled();
            int index = 0;

            foreach (object data in CurrentViewData)
            {
                DynamicObject? dynamicObject = data as DynamicObject;
                ExtractAndProcessDynamicPointData(dynamicObject!, firstDataType, xName, yName, size, pointColor, index, isSortingEnabled);
                index++;
            }
        }

        /// <summary>
        /// Processes JSON (JsonElement) data source and extracts bubble point information.
        /// </summary>
        /// <param name="firstDataType">The first data type in the collection for type inference.</param>
        /// <param name="xName">The property name for X-axis values.</param>
        /// <param name="yName">The property name for Y-axis values.</param>
        /// <param name="currentViewData">The enumerable collection of data objects to process.</param>
        protected override void ProcessJObjectData(Type firstDataType, string xName, string yName, IEnumerable<object> currentViewData)
        {
            if (CurrentViewData is null)
            {
                return;
            }

            string size = Series?.Size ?? null!;
            string pointColor = Series?.PointColorMapping ?? null!;
            bool isSortingEnabled = IsDataSortingEnabled();
            int index = 0;

            foreach (object data in CurrentViewData)
            {
                JsonElement jsonObject = (JsonElement)data;
                ExtractAndProcessJsonPointData(jsonObject, firstDataType, xName, yName, size, pointColor, index, isSortingEnabled);
                index++;
            }
        }

        /// <summary>
        /// Processes strongly-typed object data source and extracts bubble point information.
        /// </summary>
        /// <param name="firstDataType">The first data type in the collection for type inference.</param>
        /// <param name="xName">The property name for X-axis values.</param>
        /// <param name="yName">The property name for Y-axis values.</param>
        /// <param name="currentViewData">The enumerable collection of data objects to process.</param>
        protected override void ProcessObjectData(Type firstDataType, string xName, string yName, IEnumerable<object> currentViewData)
        {
            if (CurrentViewData is null || Series is null || Owner is null)
            {
                return;
            }
            using IPropertyAccessor x = FastReflectionExtension.CreateAccessor(firstDataType, xName);
            using IPropertyAccessor y = FastReflectionExtension.CreateAccessor(firstDataType, yName);
            using IPropertyAccessor size = FastReflectionExtension.CreateAccessor(firstDataType, Series.Size);
            using IPropertyAccessor pointColor = FastReflectionExtension.CreateAccessor(firstDataType, Series.PointColorMapping);
            using IPropertyAccessor textMapping = FastReflectionExtension.CreateAccessor(firstDataType, GetTextMapping());
            using IPropertyAccessor tooltipMapping = FastReflectionExtension.CreateAccessor(firstDataType, Series.TooltipMappingName);
            using IPropertyAccessor sortingInfo = FastReflectionExtension.CreateAccessor(firstDataType, Owner._sorting.PropertyName);

            bool isSortingEnabled = IsDataSortingEnabled();
            int index = 0;

            foreach (object data in CurrentViewData)
            {
                IBubblePoint chartPoint = new();
                BubblePoint point = new()
                {
                    X = chartPoint.X = x.GetValue(data),
                    Y = chartPoint.Y = y.GetValue(data),
                    Size = chartPoint.Size = size.GetValue(data),
                    Interior = chartPoint.Interior = Convert.ToString(pointColor.GetValue(data), Culture) ?? string.Empty,
                    Text = chartPoint.Text = Convert.ToString(textMapping.GetValue(data), Culture) ?? string.Empty,
                    Tooltip = chartPoint.Tooltip = Convert.ToString(tooltipMapping.GetValue(data), Culture) ?? string.Empty
                };

                GetSetXValue(point, chartPoint, index);
                SetEmptyPoint(point, chartPoint, index, firstDataType);

                if (isSortingEnabled)
                {
                    FindObjectDataSortingValue(sortingInfo, data, point.X.ToString() ?? string.Empty, point);
                }
                index++;
            }
        }

        /// <summary>
        /// Renders series elements to the provided render tree builder.
        /// </summary>
        /// <param name="builder">The render tree builder for SVG/HTML rendering.</param>
        protected void RenderSeriesElements(RenderTreeBuilder builder)
        {
            foreach (SymbolOptions symbolOption in _symbolOptions)
            {
                if (symbolOption.ShapeName == ShapeName.Ellipse)
                {
                    Owner?._svgRenderer?.RenderEllipse(builder, symbolOption.EllipseOption, symbolOption.EllipseOption.DataPoint);
                }
                else if (symbolOption.ShapeName == ShapeName.Path)
                {
                    _ = Owner?._svgRenderer?.RenderPath(builder, symbolOption.PathOption);
                }
                else if (symbolOption.ShapeName == ShapeName.Image)
                {
                    Owner?._svgRenderer?.RenderImage(builder, symbolOption.ImageOption);
                }
            }
        }

        /// <summary>
        /// Determines the sort value for bubble points based on the configured sort key.
        /// </summary>
        /// <typeparam name="T">The point type (typically BubblePoint).</typeparam>
        /// <param name="point">The point to extract the sort value from.</param>
        /// <param name="sortValue">The extracted sort value for sorting operations.</param>
        /// <returns><see langword="true"/> if a custom sort value was mapped; otherwise <see langword="false"/>.</returns>
        protected override bool IsPointValueMapped<T>(T point, out double sortValue)
        {
            string sortKey = Owner?._sorting.PropertyName.ToUpperInvariant() ?? string.Empty;
            bool isPointValueMapped = base.IsPointValueMapped(point, out sortValue);

            if (!isPointValueMapped)
            {
                BubblePoint bubblePoint = point as BubblePoint ?? null!;
                switch (sortKey)
                {
                    case "Y":
                        isPointValueMapped = true;
                        sortValue = Convert.ToDouble(bubblePoint.Y, Culture);
                        break;
                    case "SIZE":
                        isPointValueMapped = true;
                        sortValue = Convert.ToDouble(bubblePoint.Size, Culture);
                        break;
                    default:
                        break;
                }
            }
            return isPointValueMapped;
        }

        /// <summary>
        /// Determines whether markers should be rendered (always false for bubble series).
        /// </summary>
        /// <returns>Always returns <see langword="false"/> for bubble series.</returns>
        protected override bool IsMarker()
        {
            return false;
        }

        /// <summary>
        /// Builds the render tree for the series component.
        /// </summary>
        /// <param name="builder">The render tree builder.</param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            if (ClipRect is null || builder is null || (Series is not null && !Series.Visible))
            {
                return;
            }

            CreateSeriesElements(builder);
            RenderSeriesElements(builder);
            builder.CloseElement();
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Indicates whether marker rendering should occur (always false for bubble series).
        /// </summary>
        /// <returns>Always returns <see langword="false"/> for bubble series.</returns>
        internal override bool ShouldRenderMarker()
        {
            return false;
        }

        /// <summary>
        /// Determines the visibility of a bubble point based on axis constraints.
        /// </summary>
        /// <param name="point">The point to evaluate for visibility.</param>
        /// <returns><see langword="true"/> if the point should be hidden; otherwise <see langword="false"/>.</returns>
        internal override bool FindVisibility(Point point)
        {
            BubblePoint bubblePoint = point as BubblePoint ?? null!;
            SetXYMinMax(bubblePoint.XValue, bubblePoint.YValue);
            YData.Add(point.YValue);
            MaxSize = Math.Max(MaxSize, bubblePoint.Size is null || double.IsNaN((double)bubblePoint.Size) ? MaxSize : (double)bubblePoint.Size);

            return bubblePoint.X.Equals(null) || bubblePoint.Y is null || double.IsNaN(Convert.ToDouble(bubblePoint.Y, Culture));
        }

        /// <summary>
        /// Updates the series direction and recalculates bubble positions.
        /// </summary>
        internal override void UpdateDirection()
        {
            CalculateDirection();
            base.UpdateDirection();
        }

        /// <summary>
        /// Retrieves and appends chart data for a specific point to the data collection.
        /// </summary>
        /// <param name="point">The point to serialize and append.</param>
        internal override void GetChartData(Point point)
        {
            _ = ChartData?.Append(JsonSerializer.Serialize(Series?.Renderer.ChartPoints?[point.Index] as IBubblePoint, _jsonOptions));
            _ = ChartData?.Append(',');
        }

        /// <summary>
        /// Updates visual customizations (fill, color, border width, opacity) for rendered symbols.
        /// </summary>
        /// <param name="property">The property name being customized (e.g., "Fill", "Color", "Width", "Opacity").</param>
        internal override void UpdateCustomization(string property)
        {
            base.UpdateCustomization(property);
            if (RendererShouldRender)
            {
                switch (property)
                {
                    case "Fill":
                        UpdateShapeFill();
                        break;
                    case "Color":
                        UpdateShapeColor();
                        break;
                    case "Width":
                        UpdateShapeBorderWidth();
                        break;
                    case "Opacity":
                        UpdateShapeOpacity();
                        break;
                    default:
                        break;
                }

                InvalidateRender();
            }
        }

        /// <summary>
        /// Updates the border width of all rendered symbols.
        /// </summary>
        internal void UpdateShapeBorderWidth()
        {
            double width = Series?.Border.Width ?? 0;
            foreach (SymbolOptions symbolOption in _symbolOptions)
            {
                if (symbolOption.ShapeName == ShapeName.Ellipse)
                {
                    symbolOption.EllipseOption.StrokeWidth = width;
                }
                else if (symbolOption.ShapeName == ShapeName.Path)
                {
                    symbolOption.PathOption.StrokeWidth = width;
                }
            }
        }
        #endregion
    }
}
