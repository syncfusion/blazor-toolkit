using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renders series markers for a chart. Responsible for computing marker shapes,
    /// updating marker visual state and producing the render tree for markers.
    /// </summary>
    internal class ChartMarkerRenderer : ChartRenderer, IChartElementRenderer
    {
        #region Fields
        private string? _seriesIndex;
        private List<SymbolOptions> _symbolOptions = [];
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the series associated with this renderer.
        /// </summary>
        [Parameter]
        public ChartSeries Series { get; set; } = null!;

        /// <summary>
        /// Gets or sets the series renderer used to compute per-point values.
        /// </summary>
        internal ChartSeriesRenderer? SeriesRenderer { get; set; }
        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Initializes the renderer and wires back-reference on the marker model.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Series?.Marker is not null)
            {
                Series.Marker.Renderer = this;
            }
        }

        /// <summary>
        /// Releases resources held by this renderer.
        /// </summary>
        protected override ValueTask DisposeAsyncCore()
        {
            _symbolOptions?.Clear();
            return base.DisposeAsyncCore();
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Updates marker color for cached symbol options.
        /// </summary>
        private void UpdateMarkerColor()
        {
            if (Series?.Marker?.Border == null)
            {
                return;
            }
            string fill = !string.IsNullOrEmpty(Series.Marker.Border.Color) ? Series.Marker.Border.Color : SeriesRenderer?.Interior ?? string.Empty;
            foreach (SymbolOptions symbolOption in _symbolOptions.ToArray())
            {
                if (symbolOption.ShapeName == ShapeName.ellipse)
                {
                    symbolOption.EllipseOption.Stroke = fill;
                }
                else if (symbolOption.ShapeName == ShapeName.path)
                {
                    symbolOption.PathOption.Stroke = fill;
                }
            }
        }

        /// <summary>
        /// Updates fill for cached symbol options.
        /// </summary>
        private void UpdateMarkerFill()
        {
            if (Series?.Marker == null)
            {
                return;
            }
            string fill = !string.IsNullOrEmpty(Series.Marker.Fill) ? Series.Marker.Fill : "#ffffff";
            foreach (SymbolOptions symbolOption in _symbolOptions.ToArray())
            {
                if (symbolOption.ShapeName == ShapeName.ellipse)
                {
                    symbolOption.EllipseOption.Fill = fill;
                }
                else if (symbolOption.ShapeName == ShapeName.path)
                {
                    symbolOption.PathOption.Fill = fill;
                }
            }
        }

        /// <summary>
        /// Updates opacity for cached symbol options.
        /// </summary>
        private void UpdateMarkerOpacity()
        {
            if (Series?.Marker == null)
            {
                return;
            }
            double opacity = Series.Marker.Opacity;
            foreach (SymbolOptions symbolOption in _symbolOptions.ToArray())
            {
                if (symbolOption.ShapeName == ShapeName.ellipse)
                {
                    symbolOption.EllipseOption.Opacity = opacity;
                }
                else if (symbolOption.ShapeName == ShapeName.path)
                {
                    symbolOption.PathOption.Opacity = opacity;
                }
            }
        }

        /// <summary>
        /// Computes marker fill taking series, point and multi-color rules into account.
        /// Preserves original fallback of "#ffffff" when marker.IsFilled == false.
        /// </summary>
        private string ComputeMarkerFill(ChartSeries series, Point point, ChartMarker marker)
        {
            string defaultFill = marker.IsFilled ? (SeriesRenderer?.Interior ?? string.Empty) : "#ffffff";
            string fill = !string.IsNullOrEmpty(marker.Fill) ? marker.Fill : defaultFill;

            if ((series.Type == ChartSeriesType.MultiColoredLine || series.Type == ChartSeriesType.MultiColoredArea) && !string.IsNullOrEmpty(point.Interior))
            {
                fill = marker.IsFilled ? point.Interior : fill;
            }

            return fill;
        }

        /// <summary>
        /// Builds initial border settings for the marker given inputs.
        /// Preserves original SetPointColor fallback and multicolor override.
        /// </summary>
        private ChartEventBorder BuildInitialBorder(ChartMarker marker, string borderColor, ChartSeries series, Point point)
        {
            ChartEventBorder border = new() { Color = marker.Border?.Color ?? string.Empty, Width = marker.Border?.Width ?? 0 };
            border.Color = !string.IsNullOrEmpty(borderColor) ? borderColor : SeriesRenderer?.SetPointColor(point, SeriesRenderer.Interior ?? string.Empty) ?? null!;

            if ((series.Type == ChartSeriesType.MultiColoredLine || series.Type == ChartSeriesType.MultiColoredArea) && !string.IsNullOrEmpty(point.Interior))
            {
                border.Color = !string.IsNullOrEmpty(borderColor) ? borderColor : point.Interior;
            }

            return border;
        }

        /// <summary>
        /// Constructs the PointRenderEventArgs used by point customization events.
        /// Preserves empty-point fill override logic.
        /// </summary>
        private static PointRenderEventArgs BuildPointRenderArgs(ChartSeries series, Point point, ChartMarker marker, string fill, ChartEventBorder border, ChartShape shape)
        {
            PointRenderEventArgs argsData = new
            (
                Constants.PointRender,
                false,
                point,
                series,
                point.IsEmpty ? (!string.IsNullOrEmpty(series?.EmptyPointSettings?.Fill) ? series.EmptyPointSettings.Fill : fill) : fill,
                border,
                marker.Width,
                marker.Height,
                shape
            );
            return argsData;
        }

        /// <summary>
        /// Adds the computed symbol option to internal list and updates point/renderer models.
        /// Preserves symbolId construction (including culture and suffix emptiness), and uses original marker.ImageUrl.
        /// </summary>
        private void AddSymbolForPoint(Point point, ChartEventLocation location, int index, PointRenderEventArgs argsData, ChartSeries series, ChartMarker marker)
        {
            string symbolId = BuildSymbolId(point, index);

            PathOptions shapeOption = new(symbolId, string.Empty, string.Empty, argsData.Border.Width, argsData.Border.Color, argsData.Series?.Marker?.Opacity ?? 1, (argsData.Series?.Marker != null && !string.IsNullOrEmpty(argsData.Series.Marker.Fill)) ? argsData.Series.Marker.Fill : argsData.Fill, "", "", series.Renderer.GetPointDescriptionFormatText(point), "", "", SeriesRenderer?.GetDataPoints(point.XValue, point.YValue) ?? null!);
            _symbolOptions.Add(CalculateSymbol(location, argsData.Shape.ToString(), new Size(argsData.Width, argsData.Height), marker.ImageUrl?.ToString() ?? string.Empty, shapeOption, Owner ?? null!));
            point.Marker = new MarkerSettingModel() { Border = argsData.Border, Fill = argsData.Fill, Height = argsData.Height, Visible = true, Shape = argsData.Shape, Width = argsData.Width };
            if (Series?.Renderer?.ChartPoints is { })
            {
                Series.Renderer.ChartPoints[point.Index].Marker = new IMarkerSettingModel()
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
        /// Constructs the symbol id string for a point marker.
        /// </summary>
        private string BuildSymbolId(Point point, int index)
        {
            bool isTrendLine = SeriesRenderer?.Container is not null && SeriesRenderer.Container.IsTrendLine;
            string indexSuffix = !double.IsNaN(index) && index != 0 ? Convert.ToString(index, null) : string.Empty;
            return isTrendLine
                ? Owner?.ID + "_Series_" + _seriesIndex + "_TrendLine_" + _seriesIndex + "_Point_" + point.Index + "_Symbol" + indexSuffix
                : Owner?.ID + "_Series_" + _seriesIndex + "_Point_" + point.Index + "_Symbol" + indexSuffix;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Called for minor layout notifications. Kept minimal to avoid heavy work on layout events.
        /// </summary>
        protected virtual void OnLayoutChange()
        {

        }

        /// <summary>
        /// Populates internal symbol options list based on current series renderer and series points.
        /// </summary>
        protected virtual void CalculateRenderTreeBuilderOptions()
        {
            _symbolOptions = [];
            if (SeriesRenderer is null || !SeriesRenderer.ShouldRenderMarker() || (SeriesRenderer.Series is not null && !SeriesRenderer.Series.Visible))
            {
                return;
            }

            if (Owner is not null && Owner._shouldRenderMarker)
            {
                SeriesRenderer.ChartData = new System.Text.StringBuilder();
            }

            if (SeriesRenderer.Points is not null)
            {
                foreach (Point point in SeriesRenderer.Points.ToArray())
                {
                    if (point.Visible && point.SymbolLocations.Count >= 1 && Series?.Renderer?.IsPointWithInRange(point) == true)
                    {
                        foreach (ChartEventLocation location in point.SymbolLocations.ToArray())
                        {
                            CalculateSeriesRendererMarkerOptions(SeriesRenderer.Series ?? null!, point, location, point.SymbolLocations.IndexOf(location));
                        }
                    }
                    if (Owner?._tooltip is not null && Series?.Marker != null && Series.Marker.Visible && Owner._shouldRenderMarker)
                    {
                        Series.Renderer.GetChartData(point);
                    }
                }
            }
        }

        /// <summary>
        /// Calculates and adds a single symbol option for a point.
        /// </summary>
        /// <param name="series">Series metadata.</param>
        /// <param name="point">Point to render marker for.</param>
        /// <param name="location">Marker location.</param>
        /// <param name="index">Index for multi-symbol points.</param>
        protected virtual void CalculateSeriesRendererMarkerOptions(ChartSeries series, Point point, ChartEventLocation location, int index)
        {
            if (series == null || point == null || location == null)
            {
                return;
            }

            ChartMarker marker = series.Marker;
            if (marker == null)
            {
                return;
            }

            if (marker.Offset != null)
            {
                location.X += marker.Offset.X;
                location.Y -= marker.Offset.Y;
            }

            string fill = ComputeMarkerFill(series, point, marker);
          ;
            string borderColor = marker.Border?.Color ?? string.Empty;
            ChartEventBorder border = BuildInitialBorder(marker, borderColor, series, point);

            ChartEventBorder eventBorder = new() { Color = border.Color, Width = border.Width };
            int rendererIndex = SeriesRenderer is not null and ParetoLineSeriesRenderer ? SeriesRenderer.Index - 1 : SeriesRenderer?.Index ?? 0;
            ChartShape shape = marker.Shape != ChartShape.Auto ? marker.Shape : (marker.Visible ? (ChartShape)(rendererIndex % Constants.ChartMarkerCount) : ChartShape.Circle);
            PointRenderEventArgs argsData = BuildPointRenderArgs(series, point, marker, fill, eventBorder, shape);
            argsData.Border = SeriesRenderer?.SetBorderColor(point, new ChartEventBorder() { Width = argsData.Border.Width, Color = argsData.Border.Color }) ?? null!;

            if (Owner?.OnPointRender is not null)
            {
                Owner.OnPointRender.Invoke(argsData);
            }

            if (!argsData.Cancel)
            {
                AddSymbolForPoint(point, location, index, argsData, series, marker);
            }
            else
            {
                point.Marker = new MarkerSettingModel() { Visible = false };
                if (Series?.Renderer?.ChartPoints is { })
                {
                    Series.Renderer.ChartPoints[point.Index].Marker = new IMarkerSettingModel()
                    {
                        Visible = false
                    };
                }
            }
        }

        /// <summary>
        /// Renders computed symbols into the provided render tree builder.
        /// </summary>
        /// <param name="builder">Render tree builder.</param>
        protected void RenderMarkers(RenderTreeBuilder builder)
        {
            foreach (SymbolOptions symbolOption in _symbolOptions.ToArray())
            {
                if (symbolOption.ShapeName == ShapeName.ellipse)
                {
                    Owner?._svgRenderer?.RenderEllipse(builder, symbolOption.EllipseOption, symbolOption.EllipseOption.DataPoint);
                }
                else if (symbolOption.ShapeName == ShapeName.path)
                {
                    _ = Owner?._svgRenderer?.RenderPath(builder, symbolOption.PathOption);
                }
                else if (symbolOption.ShapeName == ShapeName.image)
                {
                    Owner?._svgRenderer?.RenderImage(builder, symbolOption.ImageOption);
                }
            }
        }

        /// <summary>
        /// Main Blazor render entry for markers.
        /// </summary>
        /// <param name="builder">Render tree builder.</param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            bool shouldHideMarker = Owner is not null && Owner.EnableAdaptiveRendering && SeriesRenderer?.Series?.Type != ChartSeriesType.Scatter && !Owner._shouldRenderMarker;
            if (SeriesRenderer is not null && Series?.Marker != null && Series.Marker.Visible && SeriesRenderer.Series is not null && SeriesRenderer.Series.Visible && !shouldHideMarker)
            {
                SeriesRenderer.RenderMarkerClipPath(builder);
                RenderMarkers(builder);
                builder.CloseElement();
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Calculates a symbol option from location and options; shared utility.
        /// </summary>
        /// <param name="location">Event location.</param>
        /// <param name="shape">Shape name.</param>
        /// <param name="size">Desired size.</param>
        /// <param name="url">Image URL if shape is image.</param>
        /// <param name="option">Path options.</param>
        /// <param name="chart">Owning chart.</param>
        /// <returns>Calculated SymbolOptions instance.</returns>
        internal static SymbolOptions CalculateSymbol(ChartEventLocation location, string shape, Size size, string url, PathOptions option, SfChart chart)
        {
            ChartEventLocation currentLocation = null!;
            if (chart is not null && shape == "Circle")
            {
                string[] locations = ChartHelper.AppendTextElements(chart, option.Id, location.X, location.Y, "cx", "cy");
                currentLocation = new ChartEventLocation(Convert.ToDouble(locations[0], CultureInfo.InvariantCulture), Convert.ToDouble(locations[1], CultureInfo.InvariantCulture));
            }

            SymbolOptions shapeoption = ChartHelper.CalculateShapes(currentLocation is not null ? currentLocation : location, size, shape, url, option, false);

            if (shapeoption.ShapeName == ShapeName.path)
            {
                shapeoption.PathOption.Direction = ChartHelper.AppendPathElements(chart ?? null!, shapeoption.PathOption.Direction, shapeoption.PathOption.Id);
                shapeoption.PathOption.Visibility = option.Visibility;
            }

            if (shapeoption.ShapeName == ShapeName.ellipse)
            {
                shapeoption.EllipseOption.Visibility = option.Visibility;
                shapeoption.EllipseOption.AccessibilityText = option.AccessibilityText;
            }

            if (shapeoption.ShapeName == ShapeName.image)
            {
                shapeoption.ImageOption.Visibility = option.Visibility;
            }

            return shapeoption;
        }

        /// <summary>
        /// Updates renderer state to reflect marker visibility / direction changes.
        /// </summary>
        internal void UpdateDirection()
        {
            RendererShouldRender = Series?.Marker != null && Series.Marker.Visible && ((Owner is not null && Owner._shouldRenderMarker) || Series.Type == ChartSeriesType.Scatter);
            if (RendererShouldRender)
            {
                CalculateRenderTreeBuilderOptions();
            }

            InvalidateRender();
        }

        /// <summary>
        /// Toggles visibility of markers and recalculates clip paths when necessary.
        /// </summary>
        internal void ToggleVisibility()
        {
            RendererShouldRender = true;
            if (Series?.Marker != null && Series.Marker.Visible && ((Owner is not null && Owner._shouldRenderMarker) || Series.Type == ChartSeriesType.Scatter))
            {
                SeriesRenderer?.CalculateMarkerClipPath();
                CalculateRenderTreeBuilderOptions();
            }
            else
            {
                _symbolOptions.Clear();
            }

            InvalidateRender();
        }

        /// <summary>
        /// Updates only marker colors inline to minimize recalculation.
        /// </summary>
        internal void MarkerColorChanged()
        {
            RendererShouldRender = Series?.Marker != null && Series.Marker.Visible && Owner is not null && Owner._shouldRenderMarker;
            if (RendererShouldRender)
            {
                UpdateMarkerColor();
                InvalidateRender();
            }
        }

        /// <summary>
        /// Updates customization based on the changed property.
        /// </summary>
        /// <param name="property">Name of the changed property: "Fill", "Color", "Width", "Opacity".</param>
        internal void UpdateCustomization(string property)
        {
            RendererShouldRender = Series?.Marker != null && Series.Marker.Visible && ((Owner is not null && Owner._shouldRenderMarker) || Series.Type == ChartSeriesType.Scatter);
            if (RendererShouldRender)
            {
                switch (property)
                {
                    case "Fill":
                        UpdateMarkerFill();
                        break;
                    case "Color":
                        UpdateMarkerColor();
                        break;
                    case "Width":
                        UpdateMarkerBorderWidth();
                        break;
                    case "Opacity":
                        UpdateMarkerOpacity();
                        break;
                    default:
                        break;
                }

                InvalidateRender();
            }
        }

        /// <summary>
        /// Updates stroke width for all cached symbol options.
        /// </summary>
        internal void UpdateMarkerBorderWidth()
        {
            if (Series?.Marker?.Border == null)
            {
                return;
            }
            double width = Series.Marker.Border.Width;
            foreach (SymbolOptions symbolOption in _symbolOptions.ToArray())
            {
                if (symbolOption.ShapeName == ShapeName.ellipse)
                {
                    symbolOption.EllipseOption.StrokeWidth = width;
                }
                else if (symbolOption.ShapeName == ShapeName.path)
                {
                    symbolOption.PathOption.StrokeWidth = width;
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Handles layout changes by invoking the protected OnLayoutChange method. Kept public for external layout notifications.
        /// </summary>
        public void HandleLayoutChange()
        {
            OnLayoutChange();
        }

        /// <summary>
        /// Requests a re-render of the marker visuals. Kept public for external triggers such as property changes.
        /// </summary>
        public void InvalidateRender()
        {
            _ = InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Handles chart size changes by updating renderer state and recalculating marker options if necessary.
        /// </summary>
        public override void HandleChartSizeChange(Rect rect)
        {
            if (Series == null)
            {
                return;
            }

            RendererShouldRender = Series.Marker?.Visible ?? false;
            SeriesRenderer = Series.Renderer;
            _seriesIndex = Convert.ToString(SeriesRenderer?.Index ?? 0, CultureInfo.InvariantCulture);
            if (RendererShouldRender && !(Series.Type != ChartSeriesType.Scatter && Owner is not null && !Owner._shouldRenderMarker))
            {
                CalculateRenderTreeBuilderOptions();
                SeriesRenderer.CalculateMarkerClipPath();
            }
        }
        #endregion
    }
}
