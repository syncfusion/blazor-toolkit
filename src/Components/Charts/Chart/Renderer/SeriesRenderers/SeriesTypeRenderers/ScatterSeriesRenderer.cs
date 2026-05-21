using Microsoft.AspNetCore.Components.Rendering;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renders scatter series markers and symbols for charting components.
    /// </summary>
    /// <remarks>
    /// This renderer calculates marker positions, creates symbol options,
    /// and instructs the SVG renderer to draw shapes/images/paths.
    /// </remarks>
    internal class ScatterSeriesRenderer : LineBaseSeriesRenderer
    {
        #region Fields
        private ChartMarker? _seriesMarker;
        private PathOptions? _shapeOption;
        private SymbolOptions? _symbolOption;
        private Size _markerSize = new();
        private Dictionary<string, SymbolOptions> _symbolOptions = [];
        private bool IsDataPointNeeded { get; set; }
        private List<Point>? VisiblePoints { get; set; }
        #endregion

        #region Private Methods

        /// <summary>
        /// Calculates symbol directions/options for visible scatter points in batches.
        /// </summary>
        private void CalculateDirection()
        {
            _symbolOptions?.Clear();
            ChartData = new System.Text.StringBuilder();
            IsDataPointNeeded = Owner is not null && (Owner.SelectionMode != ChartSelectionMode.None || Owner.HighlightMode != HighlightMode.None || (Owner._legendRenderer?.LegendSettings is not null && Owner._legendRenderer.LegendSettings.EnableHighlight) || Owner._tooltip.Enable);
            VisiblePoints = EnableComplexProperty();
            int count = VisiblePoints.Count;

            //batching the loop for better performance.
            const int batchSize = 1000;
            for (int batchStart = 0; batchStart < count; batchStart += batchSize)
            {
                int batchEnd = Math.Min(batchStart + batchSize, count);
                RenderScatterPoints(batchStart, batchEnd);
            }
        }

        /// <summary>
        /// Iterates a batch of points and processes each point.
        /// </summary>
        /// <param name="start">Start index inclusive.</param>
        /// <param name="end">End index exclusive.</param>
        private void RenderScatterPoints(int start, int end)
        {
            for (int i = start; i < end; i++)
            {
                Point point = VisiblePoints?[i] ?? null!;
                if (!point.Visible || !ChartHelper.WithInRange(i > 0 ? VisiblePoints?[i - 1] ?? null! : null!, point, i + 1 < VisiblePoints?.Count ? VisiblePoints[i + 1] : null!, XAxisRenderer))
                {
                    continue;
                }
                ProcessPoint(point);
            }
        }

        /// <summary>
        /// Processes a single point: invokes point render handlers and finalizes rendering data.
        /// </summary>
        /// <param name="point">The point instance.</param>
        private void ProcessPoint(Point point)
        {
            ChartEventLocation startLocation = point.SymbolLocations.Count > 0 ? point.SymbolLocations[0] : null!;
            point.SymbolLocations = [];
            point.Regions = [];
            if (ChartPoints is not null)
            {
                ChartPoints[point.Index].SymbolLocations = [];
                ChartPoints[point.Index].Regions = [];
            }

            PointRenderEventArgs argsData = new(Constants.PointRender, false, point, Series ?? null!, SetPointColor(point, Interior ?? null!), SetBorderColor(point, new ChartEventBorder { Width = Series?.Border.Width ?? 0, Color = Series?.Border.Color ?? string.Empty }), Series?.Marker.Width ?? 0, Series?.Marker.Height ?? 0, Series?.Marker.Shape ?? 0);
            if (Owner?.OnPointRender is not null)
            {
                Owner.OnPointRender.Invoke(argsData);
            }

            if (!argsData.Cancel)
            {
                AddPointSymbolLocation(point);
                if (ChartPoints is not null)
                {
                    point.Interior = ChartPoints[point.Index].Interior = argsData.Fill;
                }

                CalculateSeriesElements(point, argsData);
            }
            else
            {
                point.Marker = new MarkerSettingModel { Visible = true };
                if (ChartPoints is { })
                {
                    ChartPoints[point.Index].Marker = new IMarkerSettingModel()
                    {
                        Visible = false
                    };
                }
            }
            if (IsTooltipEnabled() && point.SymbolLocations.Count > 0)
            {
                AppendChartData(ChartPoints?[point.Index]);
            }
        }

        /// <summary>
        /// Adds computed symbol location for the provided point.
        /// </summary>
        /// <param name="point">Point whose symbol location will be appended.</param>
        private void AddPointSymbolLocation(Point point)
        {
            point.SymbolLocations.Add(ChartHelper.GetPoint(XAxisRenderer.GetPointValue(point.XValue), YAxisRenderer.GetPointValue(point.YValue), XAxisRenderer, YAxisRenderer, XLength, YLength, Owner is not null && Owner._requireInvertedAxis));
            point.SymbolLocations.ForEach(loc =>
            {
                ChartPoints?[point.Index]?.SymbolLocations.Add(new IChartInternalLocation(Math.Round(loc.X, 2), Math.Round(loc.Y, 2)));
            });
        }

        /// <summary>
        /// Calculates and updates all series-specific rendering elements for a point.
        /// </summary>
        /// <param name="point">Point being processed.</param>
        /// <param name="argsData">Render arguments for styling.</param>
        private void CalculateSeriesElements(Point point, PointRenderEventArgs argsData)
        {
            _seriesMarker = Series?.Marker;
            bool isCustomStroke = _seriesMarker is not null &&
                (_seriesMarker.Shape == ChartShape.HorizontalLine || _seriesMarker.Shape == ChartShape.VerticalLine ||
                 _seriesMarker.Shape == ChartShape.Cross || _seriesMarker.Shape == ChartShape.Plus);

            string stroke = isCustomStroke
                ? (!string.IsNullOrEmpty(argsData.Border.Color) ? argsData.Border.Color : argsData.Fill)
                : argsData.Border.Color;

            ChartShape seriesShape = argsData.Shape != ChartShape.Auto
                ? argsData.Shape
                : (_seriesMarker is not null && _seriesMarker.Visible ? (ChartShape)(Index % Constants.ChartMarkerCount) : ChartShape.Circle);

            string shape = seriesShape.ToString();
            if (shape is "VerticalLine" or "HorizontalLine" or "Cross" or "Plus")
            {
                stroke = !string.IsNullOrEmpty(argsData.Border.Color) ? argsData.Border.Color : argsData.Fill;
            }

            _shapeOption = CreateShapeOption(point, argsData, stroke);
            _markerSize.Width = argsData.Width;
            _markerSize.Height = argsData.Height;
            _symbolOption = CalculateScatterSymbol(point.SymbolLocations[0], shape, _markerSize, _seriesMarker?.ImageUrl?.ToString() ?? string.Empty, _shapeOption);
            UpdatePointRegions(point, _seriesMarker ?? null!);
            UpdatePointMarker(point, argsData, seriesShape, shape);

            if (_symbolOptions is not null && !_symbolOptions.TryAdd(_shapeOption.Id, _symbolOption))
            {
                _symbolOptions[_shapeOption.Id] = _symbolOption;
            }
        }

        /// <summary>
        /// Creates path option for symbol creation.
        /// </summary>
        /// <param name="point">Point being rendered.</param>
        /// <param name="argsData">Render args used for style values.</param>
        /// <param name="stroke">Calculated stroke color.</param>
        /// <returns>A fully populated <see cref="PathOptions"/> instance.</returns>
        private PathOptions CreateShapeOption(Point point, PointRenderEventArgs argsData, string stroke)
        {
            return new PathOptions
            {
                Id = $"{Owner?.ID}_Series_{Index}_Point_{point.Index}",
                Fill = argsData.Fill,
                StrokeWidth = argsData.Border.Width,
                Stroke = stroke,
                Opacity = Series?.Opacity ?? 1,
                DataPoint = IsDataPointNeeded ? GetDataPoints(point.XValue, point.YValue) : string.Empty,
                AccessibilityText = GetPointDescriptionFormatText(point),
                Visibility = string.Empty
            };
        }

        /// <summary>
        /// Adds region and internal region information for the given point.
        /// </summary>
        /// <param name="point">Point whose region is updated.</param>
        /// <param name="marker">Marker settings used for region size.</param>
        private void UpdatePointRegions(Point point, ChartMarker marker)
        {
            Rect newRect = new(
                point.SymbolLocations[0].X - marker.Width,
                point.SymbolLocations[0].Y - marker.Height,
                2 * marker.Width,
                2 * marker.Height
            );
            point.Regions.Add(newRect);

            if (ChartPoints?[point.Index] is not null)
            {
                ChartPoints[point.Index].Regions.Add(new IRect(
                    Math.Round(newRect.X, 2),
                    Math.Round(newRect.Y, 2),
                    newRect.Width,
                    newRect.Height
                ));
            }
        }

        /// <summary>
        /// Updates marker model on point and internal ChartPoints representation.
        /// </summary>
        /// <param name="point">Target point.</param>
        /// <param name="argsData">Rendering arguments containing style values.</param>
        /// <param name="seriesShape">Enum shape chosen.</param>
        /// <param name="shape">String form of shape.</param>
        private void UpdatePointMarker(Point point, PointRenderEventArgs argsData, ChartShape seriesShape, string shape)
        {
            point.Marker = new MarkerSettingModel
            {
                Border = argsData.Border,
                Fill = argsData.Fill,
                Height = argsData.Height,
                Visible = true,
                Width = argsData.Width,
                Shape = seriesShape
            };

            if (ChartPoints?[point.Index] is not null)
            {
                ChartPoints[point.Index].Marker = new IMarkerSettingModel()
                {
                    Border = new IChartEventBorder() { Color = argsData.Border.Color, Width = argsData.Border.Width },
                    Fill = argsData.Fill,
                    Height = argsData.Height,
                    Width = argsData.Width,
                    Shape = shape,
                    Visible = true
                };
            }
        }

        /// <summary>
        /// Calculates a symbol option for a scatter point and adjusts SVG path/ellipse/image options for accessibility and visibility.
        /// </summary>
        /// <param name="location">Location of symbol.</param>
        /// <param name="shape">Shape name as string.</param>
        /// <param name="size">Marker size.</param>
        /// <param name="url">Optional image URL for image shapes.</param>
        /// <param name="option">Source path options.</param>
        /// <returns>Computed <see cref="SymbolOptions"/> structure.</returns>
        private SymbolOptions CalculateScatterSymbol(ChartEventLocation location, string shape, Size size, string url, PathOptions option)
        {
            ChartEventLocation currentLocation = location;
            if (Owner is not null && shape == "Circle")
            {
                string[] locations = ChartHelper.AppendTextElements(Owner, option.Id, location.X, location.Y, "cx", "cy");
                currentLocation.X = Math.Round(double.Parse(locations[0], Culture), 2);
                currentLocation.Y = Math.Round(double.Parse(locations[1], Culture), 2);
            }

            SymbolOptions shapeoption = ChartHelper.CalculateShapes(currentLocation, size, shape, url, option, false);

            switch (shapeoption.ShapeName)
            {
                case ShapeName.Path:
                    shapeoption.PathOption.Direction = ChartHelper.AppendPathElements(Owner ?? null!, shapeoption.PathOption.Direction, shapeoption.PathOption.Id);
                    shapeoption.PathOption.Visibility = option.Visibility;
                    break;
                case ShapeName.Ellipse:
                    shapeoption.EllipseOption.Visibility = option.Visibility;
                    shapeoption.EllipseOption.AccessibilityText = option.AccessibilityText;
                    break;
                case ShapeName.Image:
                    shapeoption.ImageOption.Visibility = option.Visibility;
                    break;
                default:
                    break;
            }

            return shapeoption;
        }

        /// <summary>
        /// Updates stroke color for already computed shape options.
        /// </summary>
        private void UpdateShapeColor()
        {
            string borderColor = Series?.Border.Color ?? string.Empty;
            if (_symbolOptions is not null)
            {
                foreach (KeyValuePair<string, SymbolOptions> symbolOption in _symbolOptions)
                {
                    if (symbolOption.Value.ShapeName == ShapeName.Ellipse)
                    {
                        symbolOption.Value.EllipseOption.Stroke = borderColor;
                    }
                    else if (symbolOption.Value.ShapeName == ShapeName.Path)
                    {
                        symbolOption.Value.PathOption.Stroke = borderColor;
                    }
                }
            }
        }

        /// <summary>
        /// Updates fill color for already computed shape options.
        /// </summary>
        private void UpdateShapeFill()
        {
            string fill = Interior ?? string.Empty;
            if (_symbolOptions is not null)
            {
                foreach (KeyValuePair<string, SymbolOptions> _symbolOption in _symbolOptions)
                {
                    if (_symbolOption.Value.ShapeName == ShapeName.Ellipse)
                    {
                        _symbolOption.Value.EllipseOption.Fill = fill;
                    }
                    else if (_symbolOption.Value.ShapeName == ShapeName.Path)
                    {
                        _symbolOption.Value.PathOption.Fill = fill;
                    }
                }
            }
        }

        /// <summary>
        /// Updates opacity for already computed shape options.
        /// </summary>
        private void UpdateShapeOpacity()
        {
            double opacity = Series?.Opacity ?? 1;
            if (_symbolOptions is not null)
            {
                foreach (KeyValuePair<string, SymbolOptions> _symbolOption in _symbolOptions)
                {
                    if (_symbolOption.Value.ShapeName == ShapeName.Ellipse)
                    {
                        _symbolOption.Value.EllipseOption.Opacity = opacity;
                    }
                    else if (_symbolOption.Value.ShapeName == ShapeName.Path)
                    {
                        _symbolOption.Value.PathOption.Opacity = opacity;
                    }
                }
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Renders series and prepares animation options when necessary.
        /// </summary>
        protected override void RenderSeries()
        {
            base.RenderSeries();
            CalculateDirection();
            if (ShouldAnimate())
            {
                AnimationOptions = new AnimationOptions(SeriesElementId(), AnimationType.Marker);
            }
        }

        /// <summary>
        /// Renders collected symbol options into SVG elements using the owner's SVG renderer.
        /// </summary>
        /// <param name="builder">RenderTreeBuilder to append elements to.</param>
        protected void RenderSeriesElements(RenderTreeBuilder builder)
        {
            if (_symbolOptions is not null)
            {
                foreach (KeyValuePair<string, SymbolOptions> symbol in _symbolOptions)
                {
                    SymbolOptions options = symbol.Value;
                    switch (options.ShapeName)
                    {
                        case ShapeName.Ellipse:
                            Owner?._svgRenderer?.RenderEllipse(builder, options.EllipseOption, options.EllipseOption.DataPoint);
                            break;
                        case ShapeName.Path:
                            _ = Owner?._svgRenderer?.RenderPath(builder, options.PathOption);
                            break;
                        case ShapeName.Image:
                            Owner?._svgRenderer?.RenderImage(builder, options.ImageOption);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Builds the render tree for the scatter series.
        /// </summary>
        /// <param name="builder">Builder provided by Blazor to create UI output.</param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (builder is null || (Series is not null && !Series.Visible))
            {
                return;
            }

            base.BuildRenderTree(builder);

            if (ClipRect is not null)
            {
                CreateSeriesElements(builder);
                RenderSeriesElements(builder);
                builder.CloseElement();
            }
        }

        /// <summary>
        /// Marker check override for series-specific logic.
        /// </summary>
        /// <returns><see langword="false"/>; markers rendered via custom pipeline.</returns>
        protected override bool IsMarker()
        {
            return false;
        }
        #endregion

        #region Internal Methods
        /// <summary>
        /// Disposes renderer-managed resources and clears collections.
        /// </summary>
        protected override ValueTask DisposeAsyncCore()
        {
            _markerSize = null!;
            _seriesMarker = null;
            _shapeOption = null;
            _symbolOption = null;
            _symbolOptions?.Clear();
            _symbolOptions = null!;
            VisiblePoints?.Clear();
            VisiblePoints = null;
            IsDataPointNeeded = false;
            return base.DisposeAsyncCore();
        }

        /// <summary>
        /// Indicates whether markers should be rendered by the base marker pipeline.
        /// </summary>
        /// <returns><see langword="false"/> for scatter -- custom rendering is used.</returns>
        internal override bool ShouldRenderMarker()
        {
            return false;
        }

        /// <summary>
        /// Updates direction calculation and forwards to base.
        /// </summary>
        internal override void UpdateDirection()
        {
            CalculateDirection();
            base.UpdateDirection();
        }

        /// <summary>
        /// Updates rendering customization based on a changed property.
        /// </summary>
        /// <param name="property">Property name that changed.</param>
        internal override void UpdateCustomization(string property)
        {
            RendererShouldRender = Series is not null && Series.Visible;
            if (RendererShouldRender)
            {
                switch (property)
                {
                    case "Fill":
                        Owner?._seriesContainer?.GetSeriesRendererInterior(this);
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
        /// Updates stroke width for rendered shapes.
        /// </summary>
        internal void UpdateShapeBorderWidth()
        {
            double width = Series?.Border.Width ?? 0;
            if (_symbolOptions is not null)
            {
                foreach (KeyValuePair<string, SymbolOptions> _symbolOption in _symbolOptions)
                {
                    if (_symbolOption.Value.ShapeName == ShapeName.Ellipse)
                    {
                        _symbolOption.Value.EllipseOption.StrokeWidth = width;
                    }
                    else if (_symbolOption.Value.ShapeName == ShapeName.Path)
                    {
                        _symbolOption.Value.PathOption.StrokeWidth = width;
                    }
                }
            }
        }
        #endregion
    }
}