using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renders the "last data label" visual for a series.
    /// </summary>
    /// <remarks>
    /// This renderer is attached to ChartSeries.LastDataLabel.Renderer and produces
    /// background rect, connector path and centered text for the last visible point.
    /// </remarks>
    public class LastDataLabelRenderer : ChartRenderer, IChartElementRenderer
    {
        #region Constants
        private const double DEFAULT_PADDING = 6;
        private const int ANIMATION_DURATION_MS = 500;
        #endregion

        #region Fields
        private double _padding = DEFAULT_PADDING;
        private double _translateX;
        private double _translateY;
        private bool _shouldAnimate = true;
        private string _transformValue = string.Empty;
        private string _previousTransform = string.Empty;
        private string ElementId { get; set; } = string.Empty;
        private List<RectOptions> _rectOptions = [];
        private List<TextOptions> _textOptions = [];
        private List<PathOptions> _pathOptions = [];
        private CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the series owning this renderer.
        /// </summary>
        [Parameter]
        public ChartSeries? Series { get; set; }

        /// <summary>
        /// Gets or sets the associated series renderer.
        /// </summary>
        internal ChartSeriesRenderer? SeriesRenderer { get; set; }
        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Performs initialization and wires this renderer to the series.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Series is { })
            {
                Series.LastDataLabel.Renderer = this;
            }
        }

        /// <summary>
        /// Cleans up references held by the renderer.
        /// </summary>
        protected override ValueTask DisposeAsyncCore()
        {
            Series = null;
            SeriesRenderer = null;
            _rectOptions.Clear();
            _pathOptions.Clear();
            _textOptions.Clear();
            return base.DisposeAsyncCore();
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Computes and renders the group with rects, paths and text options.
        /// </summary>
        /// <param name="builder">Render tree builder.</param>
        private void RenderLastLabel(RenderTreeBuilder builder)
        {
            string clipPath = string.Empty;
            Owner?._svgRenderer?.OpenGroupElement(builder, SeriesShapeId(), _transformValue, clipPath);

            foreach (RectOptions rect in _rectOptions)
            {
                Owner?._svgRenderer?.RenderRect(builder, rect);
            }
            foreach (PathOptions path in _pathOptions)
            {
                path.Direction = ChartHelper.AppendPathElements(Owner ?? null!, path.Direction, path.Id, SeriesShapeId());
                _ = Owner?._svgRenderer?.RenderPath(builder, path);
            }
            foreach (TextOptions text in _textOptions)
            {
                Owner?._svgRenderer?.RenderText(builder, text);
            }
            builder.CloseElement();

            if (!string.IsNullOrEmpty(_previousTransform) && _previousTransform != _transformValue)
            {
                Owner?._dynamicLastLabels.Add(new DynamicLastLabelOptions
                {
                    Id = SeriesShapeId(),
                    PreviousTransform = _previousTransform,
                    CurrentX = _translateX,
                    CurrentY = _translateY,
                    Duration = ANIMATION_DURATION_MS
                });
            }
        }

        /// <summary>
        /// Resets internal collections and caches element id.
        /// </summary>
        private void InitPrivateVariables()
        {
            ElementId = Owner?.ID ?? null!;
            _rectOptions.Clear();
            _pathOptions.Clear();
            _textOptions.Clear();
        }

        /// <summary>
        /// Returns the textual series index used for element ids.
        /// </summary>
        /// <returns>Series index string.</returns>
        private string SeriesIndex()
        {
            return SeriesRenderer is not null && double.IsNaN(SeriesRenderer.Index) ? $"{SeriesRenderer.Category()}" : $"{SeriesRenderer?.Index}";
        }

        /// <summary>
        /// Clears render option collections.
        /// </summary>
        private void ClearRenderingOptions()
        {
            _rectOptions.Clear();
            _pathOptions.Clear();
            _textOptions.Clear();
        }

        /// <summary>
        /// Computes translation values for the label group.
        /// </summary>
        private void ComputeTranslations(bool isHighLowOpenClose, bool isHighLow, Point lastPoint, FinancialPoint? fPoint, ChartAxis yAxis)
        {
            _translateX = Owner is not null && Owner._requireInvertedAxis
                ? (SeriesRenderer?.ClipRect?.X ?? 0) + (isHighLowOpenClose ? (Convert.ToDouble(fPoint?.Open, Culture) <= Convert.ToDouble(fPoint?.Close, Culture))
                ? lastPoint.Regions[1].X : lastPoint.Regions[2].X : lastPoint.SymbolLocations[0].X) : (SeriesRenderer?.ClipRect?.X ?? 0);

            _translateY = Owner is not null && Owner._requireInvertedAxis
                ? (SeriesRenderer?.ClipRect?.Y ?? 0) : (SeriesRenderer?.ClipRect?.Y ?? 0) + (isHighLowOpenClose
                ? (Convert.ToDouble(fPoint?.Open, Culture) <= Convert.ToDouble(fPoint?.Close, Culture)) ? lastPoint.Regions[1].Y : lastPoint.Regions[2].Y : lastPoint.SymbolLocations[0].Y);

            _translateX = Owner is not null && Owner._requireInvertedAxis && isHighLow && !yAxis.IsInversed ? _translateX - lastPoint.Regions[isHighLow ? 0 : 1].Width : _translateX;
            _translateY = Owner is not null && !Owner._requireInvertedAxis && isHighLow && !yAxis.IsInversed ? _translateY + lastPoint.Regions[isHighLow ? 0 : 1].Height : _translateY;
        }

        /// <summary>
        /// Computes the label rectangle constrained to chart bounds.
        /// </summary>
        private Rect ComputeLabelRect(ChartLastDataLabel lastDataLabel, ChartAxis yAxis, Size textSize)
        {
            double labelWidth = textSize.Width + (_padding * 2);
            double labelHeight = textSize.Height + (_padding * 2);

            double baseValue = Owner is not null && Owner._requireInvertedAxis ? (yAxis.Renderer?.Rect.Y ?? 0) - _translateY : (yAxis.Renderer?.Rect.X ?? 0) - _translateX;
            double tickSize = yAxis.TickPosition == AxisPosition.Outside ? yAxis.MajorTickLines.Height : 0;
            double borderWidth = lastDataLabel.Border.Width * 2;
            double labelSize = Owner is not null && Owner._requireInvertedAxis ? (yAxis.Renderer?.MaxLabelSize.Height ?? 0) : yAxis.Renderer?.MaxLabelSize.Width ?? 0;
            bool isOutside = yAxis.LabelPosition == AxisPosition.Outside;
            bool isOpposed = Owner is not null && Owner.EnableRtl && !Owner._requireInvertedAxis ? !yAxis.OpposedPosition : yAxis.OpposedPosition;

            double labelX = Owner is not null && Owner._requireInvertedAxis ? -labelWidth / 2 : isOutside ? baseValue + (isOpposed ? yAxis.LabelPadding + tickSize - borderWidth : -(yAxis.LabelPadding + tickSize + borderWidth + labelSize)) : baseValue + (isOpposed ? -labelWidth : 0);
            double labelY = Owner is not null && Owner._requireInvertedAxis ? (isOutside ? baseValue + (isOpposed ? -(labelHeight + yAxis.LabelPadding + tickSize) : (yAxis.LabelPadding + tickSize + borderWidth)) : baseValue + (isOpposed ? 0 : -labelHeight)) : -labelHeight / 2;
            labelX = Math.Max(-_translateX + borderWidth, Math.Min(labelX, (Owner?.AvailableSize.Width ?? 0) - labelWidth - borderWidth - _translateX));
            labelY = Math.Max(-_translateY + borderWidth, Math.Min(labelY, (Owner?.AvailableSize.Height ?? 0) - labelHeight - borderWidth - _translateY));

            return new Rect(labelX, labelY, labelWidth, labelHeight);
        }

        /// <summary>
        /// Creates and adds the background rectangle render option.
        /// </summary>
        private void CreateBackgroundRect(ChartLastDataLabel lastDataLabel, Rect labelRect)
        {
            string background = !string.IsNullOrEmpty(lastDataLabel.Background) ? lastDataLabel.Background : Owner?._chartThemeStyle?.CrosshairFill ?? string.Empty;
            double borderWidthnew = lastDataLabel.Border?.Width ?? 1;
            string borderColor = lastDataLabel.Border?.Color ?? string.Empty;

            RectOptions rectOption = new($"{ElementId}_LastDataLabel_Background_{SeriesIndex()}", labelRect.X, labelRect.Y, labelRect.Width, labelRect.Height, borderWidthnew, borderColor, background, lastDataLabel.Rx, lastDataLabel.Ry);
            _rectOptions.Add(rectOption);
        }

        /// <summary>
        /// Creates and adds the connector path render option.
        /// </summary>
        private void CreateConnectorPath(ChartSeries series, ChartLastDataLabel lastDataLabel, Rect labelRect, ChartAxis yAxis, Point lastPoint)
        {
            bool isOpposed = Owner is not null && Owner.EnableRtl && !Owner._requireInvertedAxis ? !yAxis.OpposedPosition : yAxis.OpposedPosition;
            double lineStartY = Owner is not null && Owner._requireInvertedAxis ? (isOpposed ? SeriesRenderer?.ClipRect?.Height ?? 0 : 0) : 0;
            double lineStartX = Owner is not null && Owner._requireInvertedAxis ? 0 : (isOpposed ? 0 : SeriesRenderer?.ClipRect?.Width ?? 0);
            double lineEndX = Owner is not null && Owner._requireInvertedAxis ? 0 : (isOpposed ? labelRect.X : labelRect.X + labelRect.Width);
            double lineEndY = Owner is not null && Owner._requireInvertedAxis ? (isOpposed ? labelRect.Y + labelRect.Height : labelRect.Y) : 0;

            string direction = $"M {lineStartX.ToString(Culture)} {lineStartY.ToString(Culture)} L {lineEndX.ToString(Culture)} {lineEndY.ToString(Culture)}";
            string lineColor = !string.IsNullOrEmpty(lastDataLabel.LineColor) ? lastDataLabel.LineColor : Owner?._chartThemeStyle?.CrosshairLine ?? string.Empty;

            PathOptions pathOption = new($"{ElementId}_LastDataLabelLine_{SeriesIndex()}", direction, lastDataLabel.DashArray, lastDataLabel.LineWidth, lineColor, 1, "none", "", "", series.Renderer.GetPointDescriptionFormatText(lastPoint), "", "", "");
            _pathOptions.Add(pathOption);
        }

        /// <summary>
        /// Creates and adds the centered text render option.
        /// </summary>
        private void CreateTextOptions(Rect labelRect, Size textSize, ChartFontOptions font, string lastLabelText)
        {
            TextOptions options = new
            (
                Convert.ToString(labelRect.X + (labelRect.Width / 2), Culture),
                Convert.ToString(labelRect.Y + (labelRect.Height / 2) + (textSize.Height * 0.35), Culture),
                font.Color,
                font,
                lastLabelText,
                "middle",
                $"{ElementId}_LastDataLabel_{SeriesIndex()}",
                "",
                "0",
                "undefined",
                "",
                "text"
            );
            _textOptions.Add(options);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Builds the render tree for the last-data-label group when visible.
        /// </summary>
        /// <param name="builder">Render tree builder instance.</param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (builder is null)
            {
                return;
            }

            base.BuildRenderTree(builder);
            if (SeriesRenderer?.Series is not null && SeriesRenderer.Series.Visible && Series is not null && Series.LastDataLabel.ShowLabel)
            {
                Owner?._svgRenderer?.OpenGroupElement(builder, Owner.ID + "_LastDataLabelCollection", string.Empty, string.Empty, (_shouldAnimate ? "e-lastlabel-visible" : "e-lastlabel-hidden") + ";");
                RenderLastLabel(builder);
                builder.CloseElement();
            }
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Returns unique group id for this series last-data-label group.
        /// </summary>
        /// <returns>Series-specific shape id string.</returns>
        internal string SeriesShapeId()
        {
            return Owner?.ID + "LastDataLabelGroup_" + SeriesIndex();
        }

        /// <summary>
        /// Main entry that computes and prepares render options for the last data label.
        /// </summary>
        /// <param name="series">Series instance.</param>
        /// <param name="lastDataLabel">Associated last data label options.</param>
        internal void RenderLastValue(ChartSeries series, ChartLastDataLabel lastDataLabel)
        {
            InitPrivateVariables();

            bool isHighLowOpenClose = series.SeriesType is "HiloOpenClose" or "Candle";
            bool isHighLow = series.SeriesType is "Hilo" or "RangeArea" or "RangeStepArea" or "SplineRangeArea" or "RangeColumn";

            List<Point> visiblePoints = ChartHelper.GetVisiblePoints(SeriesRenderer?.Points ?? null!);
            if (visiblePoints.Count < 1)
            {
                return;
            }

            Point lastPoint = visiblePoints[^1];
            FinancialPoint fPoint = lastPoint as FinancialPoint ?? null!;
            ChartAxis yAxis = series.Renderer.YAxisRenderer.Axis ?? null!;

            double rawValue = isHighLowOpenClose ? Convert.ToDouble(fPoint.Close, Culture) : isHighLow ? Convert.ToDouble(fPoint.Low, Culture) : lastPoint.YValue;
            if (!(isHighLowOpenClose ? (lastPoint.Regions is not null && lastPoint.Regions.Count > 0) : (lastPoint.SymbolLocations is not null && lastPoint.SymbolLocations.Count > 0)) || rawValue > yAxis.Renderer?.VisibleRange.End || rawValue < yAxis.Renderer?.VisibleRange.Start)
            {
                return;
            }

            ComputeTranslations(isHighLowOpenClose, isHighLow, lastPoint, fPoint, yAxis);
            _previousTransform = _transformValue;
            _transformValue = "translate(" + _translateX.ToString(Culture) + "," + _translateY.ToString(Culture) + ")";

            string lastLabelText = yAxis.Renderer?.FormatValue(rawValue) ?? string.Empty;
            ChartFontOptions font = lastDataLabel.Font.GetFontOptions(Owner?._chartThemeStyle ?? null!);
            font.Size = Regex.IsMatch(font.Size, @"^\d+(\.\d+)?$") ? font.Size + "px" : font.Size;
            Size textSize = ChartHelper.MeasureText(lastLabelText, font);

            Rect labelRect = ComputeLabelRect(lastDataLabel, yAxis, textSize);
            CreateBackgroundRect(lastDataLabel, labelRect);
            CreateConnectorPath(series, lastDataLabel, labelRect, yAxis, lastPoint);

            bool isDefaultAnimation = SyncfusionService?._options.Animation == GlobalAnimationMode.Default;
            bool isGlobalAnimationEnabled = SyncfusionService?._options.Animation == GlobalAnimationMode.Enable;
            _shouldAnimate = Owner?._visibleSeriesRenderers.Count == 0 || (Owner is not null && !Owner._visibleSeriesRenderers.Any
                (s => ((s.Series is not null && s.Series.Animation.Enable && isDefaultAnimation) || isGlobalAnimationEnabled) && Owner._shouldAnimateSeries)
            );

            CreateTextOptions(labelRect, textSize, font, lastLabelText);
        }

        /// <summary>
        /// Toggles label visibility and triggers re-render when shown/hidden.
        /// </summary>
        internal void ToggleVisibility()
        {
            if (Series is not null && Series.LastDataLabel.ShowLabel)
            {
                LastlabelValueChanged();
            }
            else
            {
                RendererShouldRender = true;
                ClearRenderingOptions();
                InvalidateRender();
            }
        }

        /// <summary>
        /// Called when last label value or visibility changes.
        /// </summary>
        internal void LastlabelValueChanged()
        {
            RendererShouldRender = Series is not null && Series.LastDataLabel.ShowLabel;
            if (RendererShouldRender)
            {
                RenderLastValue(Series ?? null!, Series?.LastDataLabel ?? null!);
            }
            InvalidateRender();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Responds to chart size changes by recalculating label if required.
        /// </summary>
        /// <param name="rect">New chart rect.</param>
        public override void HandleChartSizeChange(Rect rect)
        {
            RendererShouldRender = Series is not null && Series.LastDataLabel.ShowLabel;
            SeriesRenderer = Series?.Renderer;
            if (RendererShouldRender && SeriesRenderer?.Series is not null && SeriesRenderer.Series.Visible)
            {
                RenderLastValue(Series ?? null!, Series?.LastDataLabel ?? null!);
            }
        }

        /// <summary>
        /// Requests a component re-render on the Blazor UI thread.
        /// </summary>
        public void InvalidateRender()
        {
            _ = InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Placeholder for future layout-change handling.
        /// </summary>
        public void HandleLayoutChange()
        {

        }
        #endregion
    }
}