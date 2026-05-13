using Microsoft.AspNetCore.Components.Rendering;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renders striplines for chart axes. Responsible for measuring, creating SVG path/rect/text options and rendering them.
    /// </summary>
    /// <remarks>
    /// This renderer handles both pixel-based and value-based striplines, supports repeated striplines,
    /// segmented striplines, and text rendering with customizable positioning and rotation.
    /// </remarks>
    public class ChartStriplineRenderer : ChartRenderer, IChartElementRenderer
    {
        #region Constants
        private const string SPACE = " ";
        #endregion

        #region Fields
        private Rect? _rect;
        private Rect? _seriesClipRect;
        private CultureInfo _culture = CultureInfo.InvariantCulture;
        private ChartAxisRenderer? _axisRenderer;
        private ChartAxis? _axis;
        private PathOptions? _striplinePath;
        private List<TextOptions> _striplineText = [];
        #endregion

        #region Properties

        /// <summary>
        /// Collection of rectangle options used for rendering non-pixel striplines.
        /// </summary>
        /// <value>A list of <see cref="RectOptions"/> objects representing stripline rectangles.</value>
        internal List<RectOptions> StriplineRect { get; set; } = [];

        /// <summary>
        /// Associated stripline model.
        /// </summary>
        /// <value>The <see cref="ChartStripline"/> model; <see langword="null"/> if not associated.</value>
        internal ChartStripline? Stripline { get; set; }

        /// <summary>
        /// Index of this renderer (used to compose IDs).
        /// </summary>
        /// <value>A zero-based index for uniquely identifying this renderer instance.</value>
        internal int Index { get; set; }
        #endregion

        #region Private Methods

        /// <summary>
        /// Clamps a range value between min and max bounds.
        /// </summary>
        /// <param name="range">The value to clamp.</param>
        /// <param name="min">The minimum allowed value.</param>
        /// <param name="max">The maximum allowed value.</param>
        /// <returns>The clamped value within [min, max].</returns>
        private static double FindValue(double range, double min, double max)
        {
            return range < min ? min : (range > max ? max : range);
        }

        /// <summary>
        /// Calculates text start position based on alignment and size.
        /// </summary>
        /// <param name="xy">The base coordinate.</param>
        /// <param name="size">The size of the text container.</param>
        /// <param name="textAlignment">The alignment anchor.</param>
        /// <returns>Adjusted coordinate for text positioning.</returns>
        private static double GetTextStart(double xy, double size, Anchor textAlignment)
        {
            switch (textAlignment)
            {
                case Anchor.Start:
                    xy = xy - (size * 0.5) + 5;
                    break;
                case Anchor.End:
                    xy = xy + (size * 0.5) - 5;
                    break;
                case Anchor.Middle:
                    break;
                default:
                    break;
            }
            return xy;
        }

        /// <summary>
        /// Computes alignment factor for positioning calculations.
        /// </summary>
        /// <param name="anchor">The anchor alignment.</param>
        /// <returns>A factor of -1, 0, or 1 depending on alignment.</returns>
        private static double Factor(Anchor anchor)
        {
            return anchor == Anchor.Middle ? 0 : (anchor == Anchor.Start ? 1 : -1);
        }

        /// <summary>
        /// Inverts text alignment (Start ↔ End, Middle unchanged).
        /// </summary>
        /// <param name="anchor">The original anchor.</param>
        /// <returns>The inverted anchor.</returns>
        private static Anchor InvertAlignment(Anchor anchor)
        {
            if (anchor == Anchor.Start)
            {
                anchor = Anchor.End;
            }
            else if (anchor == Anchor.End)
            {
                anchor = Anchor.Start;
            }

            return anchor;
        }

        /// <summary>
        /// Clears previously computed rendering options.
        /// </summary>
        private void ClearPerviousPathOptions()
        {
            StriplineRect.Clear();
            _striplineText.Clear();
        }

        /// <summary>
        /// Main entry point for calculating stripline rendering options.
        /// </summary>
        /// <param name="isUpdateDirection">Indicates whether this is a direction-only update.</param>
        private void GetSetStriplineOptions(bool isUpdateDirection = false)
        {
            if (Stripline is not null && Stripline.Visible && _axis is not null)
            {
                ChartAxisRenderer segmentAxisRenderer = null!;
                if (Stripline.IsSegmented && Stripline.SegmentStart is not null && Stripline.SegmentEnd is not null && Stripline.SizeType != SizeType.Pixel)
                {
                    segmentAxisRenderer = GetSegmentAxis();
                }

                if (Stripline.IsRepeat && Stripline.RepeatEvery is not null && Stripline.Size != 0 && Stripline.SizeType != SizeType.Pixel)
                {
                    ProcessRepeatedStriplines(segmentAxisRenderer, isUpdateDirection);
                }
                else
                {
                    CalculateStriplineDrawOptions(segmentAxisRenderer, double.NaN, isUpdateDirection);
                }

                if (!string.IsNullOrEmpty(Stripline.Text) && !(Owner is not null && Owner.EnableAdaptiveRendering && (Owner._widthCategory == ChartWidthCategory.Small || Owner._heightCategory == ChartHeightCategory.Small)))
                {
                    CalculateStriplineTextOption(isUpdateDirection);
                }
            }
        }

        /// <summary>
        /// Processes and renders repeated striplines within the visible range.
        /// </summary>
        /// <param name="segmentAxisRenderer">Optional renderer for segmented striplines.</param>
        /// <param name="isUpdateDirection">Indicates whether this is a direction-only update.</param>
        private void ProcessRepeatedStriplines(ChartAxisRenderer? segmentAxisRenderer, bool isUpdateDirection)
        {
            if (Stripline is null)
            {
                return;
            }
            double limit = (Stripline.RepeatUntil is not null) ? ((_axis?.ValueType == ValueType.DateTime) ? ChartHelper.GetTime((DateTime)Stripline.RepeatUntil) : Convert.ToDouble(Stripline.RepeatUntil, null)) : (_axisRenderer?.ActualRange.End ?? 0);
            double startValue = CalculateStartValue();

            while (startValue < limit)
            {
                if ((startValue >= _axisRenderer?.VisibleRange.Start && startValue < _axisRenderer.VisibleRange.End) || (_axisRenderer is not null && ChartHelper.WithIn(startValue + (_axis?.ValueType == ValueType.DateTime ? _axisRenderer.DateTimeInterval * +Stripline.Size : Stripline.Size), _axisRenderer.VisibleRange)))
                {
                    CalculateStriplineDrawOptions(segmentAxisRenderer!, startValue, isUpdateDirection);
                }
                startValue = GetStartValue(startValue);
            }
        }

        /// <summary>
        /// Calculates the initial start value for repeated stripline generation.
        /// </summary>
        /// <returns>The computed start value.</returns>
        private double CalculateStartValue()
        {
            if (Stripline is null)
            {
                return 0;
            }
            double startValue = (Stripline.Start is not null && _axis?.ValueType == ValueType.DateTime) ? Convert.ToDouble(ChartHelper.GetTime((DateTime)Stripline.Start)) : Convert.ToDouble(Stripline.Start, null);

            if ((Stripline.StartFromAxis && _axis?.ValueType == ValueType.DateTime && Stripline.SizeType == SizeType.Auto) || (startValue < _axisRenderer?.VisibleRange.Start))
            {
                startValue = _axisRenderer?.VisibleLabels[0].Value == _axisRenderer?.VisibleRange.Start ? _axisRenderer?.VisibleRange.Start ?? 0 : (_axisRenderer?.VisibleLabels[0].Value ?? 0) - (_axis?.ValueType == ValueType.DateTime ? (_axisRenderer?.DateTimeInterval ?? 0) : (_axisRenderer?.VisibleInterval ?? 0));
            }

            return Stripline.StartFromAxis && _axis?.ValueType != ValueType.DateTime ? _axisRenderer?.VisibleRange.Start ?? 0 : startValue;
        }

        /// <summary>
        /// Updates color for path or rect options based on stripline type.
        /// </summary>
        /// <param name="isLineOption">Indicates if this is a pixel-based (line) stripline.</param>
        private void UpdateColorCustomization(bool isLineOption)
        {
            if (isLineOption && _striplinePath is not null)
            {
                _striplinePath.Stroke = Stripline?.Color ?? string.Empty;
            }
            else
            {
                StriplineRect.ForEach(rect => rect.Fill = Stripline?.Color ?? string.Empty);
            }
        }

        /// <summary>
        /// Updates opacity for path or rect options based on stripline type.
        /// </summary>
        /// <param name="isLineOption">Indicates if this is a pixel-based (line) stripline.</param>
        private void UpdateOpacityCustomization(bool isLineOption)
        {
            if (isLineOption && _striplinePath is not null)
            {
                _striplinePath.Opacity = Stripline?.Opacity ?? 1;
            }
            else
            {
                StriplineRect.ForEach(rect => rect.Opacity = Stripline?.Opacity ?? 1);
            }
        }

        /// <summary>
        /// Calculates SVG path or rect options for a single stripline instance.
        /// </summary>
        /// <param name="segmentAxisRenderer">Optional segment axis for segmented striplines.</param>
        /// <param name="startValue">The start value for repeated striplines (NaN if not repeated).</param>
        /// <param name="isUpdateDirection">Indicates whether this is a direction-only update.</param>
        private void CalculateStriplineDrawOptions(ChartAxisRenderer segmentAxisRenderer, double startValue, bool isUpdateDirection = false)
        {
            _rect = MeasureStripline(segmentAxisRenderer, startValue);
            string id = Owner?.ID + "_stripline_" + Stripline?.ZIndex + "_";

            if (Stripline?.SizeType == SizeType.Pixel)
            {
                CreatePixelStriplinePath(isUpdateDirection, id);
            }
            else if (_rect.Height != 0 && _rect.Width != 0)
            {
                id += "rect_" + _axis?.Name + "_" + Index;
                if (!isUpdateDirection)
                {
                    Rect currentRect = ChartHelper.AppendRectElements(Owner ?? null!, id, _rect);
                    StriplineRect.Add(new RectOptions(id, currentRect.X, currentRect.Y, currentRect.Width, currentRect.Height, Stripline?.Border.Width ?? 1, Stripline?.Border.Color ?? string.Empty, Stripline?.Color ?? string.Empty, 0, 0, Stripline?.Opacity ?? 1));
                }
                else if (StriplineRect.Count > 0)
                {
                    RectOptions rectOption = StriplineRect.Find(element => element.Id == id) ?? null!;
                    rectOption.X = _rect.X;
                    rectOption.Y = _rect.Y;
                    rectOption.Width = _rect.Width;
                    rectOption.Height = _rect.Height;
                }
            }
        }

        /// <summary>
        /// Creates a pixel-based (line) stripline as a path element.
        /// </summary>
        /// <param name="isUpdateDirection">Indicates whether this is a direction-only update.</param>
        /// <param name="id">The unique identifier for the stripline path element.</param>
        private void CreatePixelStriplinePath(bool isUpdateDirection, string id)
        {
            id += "path_" + _axis?.Name + '_' + Index;
            string direction = CalculatePixelPathDirection();
            direction = ChartHelper.AppendPathElements(Owner ?? null!, direction, id);

            if (!isUpdateDirection && Stripline?.DashArray is not null)
            {
                _striplinePath = new PathOptions
                {
                    Id = id,
                    Direction = direction,
                    StrokeDashArray = Stripline.DashArray,
                    StrokeWidth = Stripline.Size != 0 ? Stripline.Size : 1,
                    Stroke = Stripline.Color,
                    Opacity = Stripline.Opacity
                };
            }
            else if (_striplinePath is { })
            {
                _striplinePath.Direction = direction;
            }
        }

        /// <summary>
        /// Calculates the SVG path direction string for a pixel stripline.
        /// </summary>
        /// <returns>An SVG path command string.</returns>
        private string CalculatePixelPathDirection()
        {
            return _rect is null
                ? string.Empty
                : _axisRenderer?.Orientation == Orientation.Vertical
                ? ("M" + _rect.X.ToString(_culture) + SPACE + _rect.Y.ToString(_culture) + SPACE + "L" + (_rect.X + _rect.Width).ToString(_culture) + SPACE + _rect.Y.ToString(_culture))
                : ("M" + _rect.X.ToString(_culture) + SPACE + _rect.Y.ToString(_culture) + SPACE + "L" + _rect.X.ToString(_culture) + SPACE + (_rect.Y + _rect.Height).ToString(_culture));
        }

        /// <summary>
        /// Calculates text options for stripline labels.
        /// </summary>
        /// <param name="isUpdateDirection">Indicates whether this is a direction-only update.</param>
        private void CalculateStriplineTextOption(bool isUpdateDirection = false)
        {
            if (_axisRenderer is null)
            {
                return;
            }
            string id = Owner?.ID + "_stripline_" + Stripline?.ZIndex + "_" + "text_" + _axis?.Name + "_" + Index;
            Size textSize = ChartHelper.MeasureText(Stripline?.Text ?? null!, Stripline?.TextStyle.GetFontOptions(Owner?._chartThemeStyle ?? null!) ?? null!);
            _axisRenderer.IsStripLineTooltip = !_axisRenderer.IsStripLineTooltip && Stripline?.StriplineTooltip is not null ? Stripline.StriplineTooltip.Enable : _axisRenderer.IsStripLineTooltip;
            (double tx, double ty, Anchor anchor) = CalculateTextPosition(textSize);

            TextOptions textOption;
            if (isUpdateDirection)
            {
                textOption = _striplineText.Find(element => element.Id == id) ?? null!;
                textOption.X = tx.ToString(_culture);
                textOption.Y = ty.ToString(_culture);
            }
            else
            {
                textOption = new TextOptions(tx.ToString(_culture), ty.ToString(_culture), !string.IsNullOrEmpty(Stripline?.TextStyle.Color) ? Stripline.TextStyle.Color : Owner?._chartThemeStyle?.StriplineTextColor ?? string.Empty, Stripline?.TextStyle.GetFontOptions(Owner?._chartThemeStyle ?? null!) ?? null!, Stripline?.Text ?? string.Empty, anchor.ToString(), id, "rotate(" + (double.IsNaN(Stripline?.Rotation ?? 0) ? _axisRenderer?.Orientation == Orientation.Vertical ? 0 : -90 : Stripline?.Rotation) + "," + tx.ToString(_culture) + "," + ty.ToString(_culture) + ")");
                string[] locations = ChartHelper.AppendTextElements(Owner ?? null!, id, tx, ty);
                textOption.X = locations[0];
                textOption.Y = locations[1];
                _striplineText.Add(textOption);
            }
        }

        /// <summary>
        /// Calculates text position and anchor based on axis orientation and alignment.
        /// </summary>
        /// <param name="textSize">The measured text size.</param>
        /// <returns>A tuple of (tx, ty, anchor).</returns>
        private (double tx, double ty, Anchor anchor) CalculateTextPosition(Size textSize)
        {
            double textMid = 3 * (textSize.Height / 8);
            double ty = (_rect?.Y ?? 0) + ((_rect?.Height ?? 0) / 2) + textMid;
            double tx = (_rect?.X ?? 0) + ((_rect?.Width ?? 0) / 2);
            Anchor anchor;

            if (_axisRenderer?.Orientation == Orientation.Horizontal)
            {
                tx = GetTextStart(tx + (textMid + Factor(Stripline?.HorizontalAlignment ?? Anchor.Middle)), _rect?.Width ?? 0, Stripline?.HorizontalAlignment ?? Anchor.Middle);
                ty = GetTextStart(ty - textMid, _rect?.Height ?? 0, Stripline?.VerticalAlignment ?? Anchor.Middle);
                anchor = InvertAlignment(Stripline?.VerticalAlignment ?? Anchor.Middle);
            }
            else
            {
                tx = GetTextStart(tx, _rect?.Width ?? 0, Stripline?.HorizontalAlignment ?? Anchor.Middle);
                ty = GetTextStart(ty + (textMid * Factor(Stripline?.VerticalAlignment ?? Anchor.Middle)) - 5, _rect?.Height ?? 0, Stripline?.VerticalAlignment ?? Anchor.Middle);
                anchor = Stripline?.HorizontalAlignment ?? Anchor.Middle;
            }
            return (tx, ty, anchor);
        }

        /// <summary>
        /// Retrieves the segment axis renderer if a specific segment axis is named.
        /// </summary>
        /// <returns>The <see cref="ChartAxisRenderer"/> for the segment axis.</returns>
        /// <exception cref="InvalidOperationException">Thrown if axes container is unavailable or segment axis not found.</exception>
        private ChartAxisRenderer GetSegmentAxis()
        {
            List<ChartAxisRenderer> axes = Owner?._axisContainer?.Renderers.Cast<ChartAxisRenderer>().ToList() ?? null!;
            return string.IsNullOrEmpty(Stripline?.SegmentAxisName)
                ? _axis?.Renderer?.Orientation == Orientation.Horizontal ? axes.ElementAt(1) : axes.First()
                : axes.First(axis => Stripline.SegmentAxisName == axis.Axis?.Name);
        }

        /// <summary>
        /// Measures stripline bounds in pixel coordinates.
        /// </summary>
        /// <param name="segmentAxis">Optional renderer for segmented striplines.</param>
        /// <param name="startValue">The start value for repeated striplines (NaN if not repeated).</param>
        /// <returns>A <see cref="Rect"/> representing the stripline bounds.</returns>
        private Rect MeasureStripline(ChartAxisRenderer segmentAxis, double startValue = double.NaN)
        {
            (double actualStart, double actualEnd) = CalculateActualValues(startValue);

            FromTo rect = GetFromToValue(actualStart, actualEnd, _axisRenderer ?? null!, Stripline?.Size ?? 0, Stripline is not null && Stripline.StartFromAxis);
            double height = _axisRenderer?.Orientation == Orientation.Vertical ? (rect.To - rect.From) * _axisRenderer.Rect.Height : _seriesClipRect?.Height ?? 0;
            double width = _axisRenderer?.Orientation == Orientation.Horizontal ? (rect.To - rect.From) * _axisRenderer.Rect.Width : _seriesClipRect?.Width ?? 0;
            double x = _axisRenderer?.Orientation == Orientation.Vertical ? _seriesClipRect?.X ?? 0 : ((rect.From * (_axisRenderer?.Rect.Width ?? 0)) + _axisRenderer?.Rect.X) ?? 0;
            double y = _axisRenderer?.Orientation == Orientation.Horizontal ? _seriesClipRect?.Y ?? 0 : ((_axisRenderer?.Rect.Y ?? 0) + (_axisRenderer?.Rect.Height ?? 0) - ((Stripline?.SizeType == SizeType.Pixel ? rect.From : rect.To) * (_axisRenderer?.Rect.Height ?? 0)));

            if (Stripline is not null && Stripline.IsSegmented && Stripline.SegmentStart is not null && Stripline.SegmentEnd is not null && Stripline.SizeType != SizeType.Pixel)
            {
                FromTo segRect = GetFromToValue(segmentAxis.Axis?.ValueType == ValueType.DateTime ? ChartHelper.GetTime((DateTime)Stripline.SegmentStart) : Convert.ToDouble(Stripline.SegmentStart, null), segmentAxis.Axis?.ValueType == ValueType.DateTime ? ChartHelper.GetTime((DateTime)Stripline.SegmentEnd) : Convert.ToDouble(Stripline.SegmentEnd, null), segmentAxis);
                if (segmentAxis.Orientation == Orientation.Vertical)
                {
                    y = segmentAxis.Rect.Y + segmentAxis.Rect.Height - (segRect.To * segmentAxis.Rect.Height);
                    height = (segRect.To - segRect.From) * segmentAxis.Rect.Height;
                }
                else
                {
                    x = (segRect.From * segmentAxis.Rect.Width) + segmentAxis.Rect.X;
                    width = (segRect.To - segRect.From) * segmentAxis.Rect.Width;
                }
            }

            return (height != 0 && width != 0) || (Stripline?.SizeType == SizeType.Pixel && (Stripline.Start is not null || Stripline.StartFromAxis))
                ? new Rect(x, y, width, height)
                : new Rect(0, 0, 0, 0);
        }

        /// <summary>
        /// Calculates actual start and end values based on axis type and stripline configuration.
        /// </summary>
        /// <param name="startValue">The start value for repeated striplines.</param>
        /// <returns>A tuple of (actualStart, actualEnd).</returns>
        private (double actualStart, double actualEnd) CalculateActualValues(double startValue)
        {
            double actualStart, actualEnd;
            if (Stripline is not null && Stripline.IsRepeat && Stripline.Size != 0)
            {
                actualStart = startValue;
                actualEnd = double.NaN;
            }
            else
            {
                if (_axis?.ValueType == ValueType.DateTimeCategory)
                {
                    actualStart = Stripline?.Start is null ? double.NaN : Stripline.Start.GetType().Equals(typeof(int)) ? (int)Stripline.Start : _axisRenderer?.Labels.IndexOf(ChartHelper.GetTime((DateTime)Stripline.Start).ToString(_culture)) ?? 0;
                    actualEnd = Stripline?.End is null ? double.NaN : Stripline.End.GetType().Equals(typeof(int)) ? (int)Stripline.End : _axisRenderer?.Labels.IndexOf(ChartHelper.GetTime((DateTime)Stripline.End).ToString(_culture)) ?? 0;
                }
                else if (_axis?.ValueType == ValueType.DateTime)
                {
                    actualStart = Stripline?.Start is null ? double.NaN : ChartHelper.GetTime((DateTime)Stripline.Start);
                    actualEnd = Stripline?.End is null ? double.NaN : ChartHelper.GetTime((DateTime)Stripline.End);
                }
                else if (_axis?.ValueType == ValueType.Logarithmic)
                {
                    actualStart = Stripline?.Start is null ? double.NaN : ChartHelper.LogBase(Convert.ToDouble(Stripline.Start, null), _axis.LogBase);
                    actualEnd = Stripline?.End is null ? double.NaN : ChartHelper.LogBase(Convert.ToDouble(Stripline.End, null), _axis.LogBase);
                }
                else
                {
                    actualStart = Stripline?.Start is null ? double.NaN : Convert.ToDouble(Stripline.Start, null);
                    actualEnd = Stripline?.End is null ? double.NaN : Convert.ToDouble(Stripline.End, null);
                }
            }
            return (actualStart, actualEnd);
        }

        /// <summary>
        /// Converts axis values to normalized coefficient ranges.
        /// </summary>
        /// <param name="start">Start value.</param>
        /// <param name="end">End value.</param>
        /// <param name="axis">The chart axis renderer.</param>
        /// <param name="size">Size for calculating end value (if auto-calculated).</param>
        /// <param name="startFromAxis">Whether to start from axis minimum.</param>
        /// <returns>A <see cref="FromTo"/> representing normalized positions.</returns>
        private FromTo GetFromToValue(double start, double end, ChartAxisRenderer axis, double size = double.NaN, bool startFromAxis = false)
        {
            FromTo result = new();
            double from = (startFromAxis && Stripline is not null && !Stripline.IsRepeat) ? axis.VisibleRange.Start : start;
            double to = GetToValue(Math.Max(!double.IsNaN(start) ? start : double.NegativeInfinity, double.IsNaN(end) ? start : end), from, size, end);
            from = FindValue(from, axis.VisibleRange.Start, axis.VisibleRange.End);
            to = FindValue(to, axis.VisibleRange.Start, axis.VisibleRange.End);
            result.From = ChartHelper.ValueToCoefficient(axis.Axis is not null && axis.Axis.IsAxisInverse ? to : from, axis);
            result.To = ChartHelper.ValueToCoefficient(axis.Axis is not null && axis.Axis.IsAxisInverse ? from : to, axis);
            return result;
        }

        /// <summary>
        /// Calculates the end coordinate for a stripline based on size and type.
        /// </summary>
        /// <param name="to">The nominal end value.</param>
        /// <param name="from">The start value.</param>
        /// <param name="size">The stripline size.</param>
        /// <param name="end">Explicit end value if specified.</param>
        /// <returns>The calculated end value.</returns>
        private double GetToValue(double to, double from, double size, double end)
        {
            SizeType sizeType = Stripline?.SizeType ?? SizeType.Auto;
            if (_axis?.ValueType == ValueType.DateTime)
            {
                DateTime fromValue = new DateTime(1970, 1, 1).AddMilliseconds(from);
                if (sizeType == SizeType.Auto)
                {
                    sizeType = Enum.Parse<SizeType>(_axisRenderer?.Axis?.AxisActualIntervalType ?? string.Empty);
                    size *= _axisRenderer?.VisibleInterval ?? 0;
                }

                return sizeType switch
                {
                    SizeType.Years => double.IsNaN(end) ? ChartHelper.GetTime(fromValue.AddYears((int)size)) : to,
                    SizeType.Months => double.IsNaN(end) ? ChartHelper.GetTime(fromValue.AddMonths((int)size)) : to,
                    SizeType.Days => double.IsNaN(end) ? ChartHelper.GetTime(fromValue.AddDays((int)size)) : to,
                    SizeType.Hours => double.IsNaN(end) ? ChartHelper.GetTime(fromValue.AddHours((int)size)) : to,
                    SizeType.Minutes => double.IsNaN(end) ? ChartHelper.GetTime(fromValue.AddMinutes((int)size)) : to,
                    SizeType.Seconds => double.IsNaN(end) ? ChartHelper.GetTime(fromValue.AddSeconds((int)size)) : to,
                    SizeType.Auto => 0,
                    SizeType.Pixel => 0,
                    _ => from,
                };
            }
            else
            {
                return Stripline?.SizeType == SizeType.Pixel ? from : (double.IsNaN(end) ? from + size : to);
            }
        }

        /// <summary>
        /// Calculates the next start value for repeated striplines.
        /// </summary>
        /// <param name="startValue">The current repeat iteration start value.</param>
        /// <returns>The next start value.</returns>
        private double GetStartValue(double startValue)
        {
            return _axis?.ValueType == ValueType.DateTime
                ? GetToValue(double.NaN, startValue, Convert.ToDouble(Stripline?.RepeatEvery, null), double.NaN)
                : startValue + Convert.ToDouble(Stripline?.RepeatEvery, null);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Renders the stripline SVG elements into the render tree.
        /// </summary>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/> to append elements to.</param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            if (_rect is not null && builder is not null)
            {
                if (_striplinePath is not null)
                {
                    Owner?._svgRenderer?.RenderPath(builder, _striplinePath.Id, _striplinePath.Direction, _striplinePath.StrokeDashArray, _striplinePath.StrokeWidth, _striplinePath.Stroke, _striplinePath.Opacity);
                }
                else
                {
                    foreach (RectOptions rect in StriplineRect)
                    {
                        if (!(double.IsNaN(rect.X) || double.IsNaN(rect.Width)))
                        {
                            Owner?._svgRenderer?.RenderRect(builder, rect);
                        }
                    }
                }

                foreach (TextOptions text in _striplineText)
                {
                    Owner?._svgRenderer?.RenderText(builder, text);
                }

                RendererShouldRender = false;
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Initializes internal values required before measuring and rendering the stripline.
        /// </summary>
        /// <remarks>
        /// Called during component initialization to set up axis references and clip rectangles.
        /// Must be called before <see cref="CalculateRenderingOptions"/>.
        /// </remarks>
        internal void InitStripline()
        {
            _seriesClipRect = Owner?._axisContainer?.AxisLayout.SeriesClipRect;
            _axis = Stripline?.Parent?.Axis ?? null!;
            _axisRenderer = _axis.Renderer;
            CalculateRenderingOptions();
        }

        /// <summary>
        /// Computes rendering state and prepares path/rect/text options.
        /// </summary>
        /// <remarks>
        /// Clears previous options and recalculates all stripline rendering geometry.
        /// Called during initialization and when properties change.
        /// </remarks>
        internal void CalculateRenderingOptions()
        {
            RendererShouldRender = true;
            ClearPerviousPathOptions();
            GetSetStriplineOptions();
        }

        /// <summary>
        /// Updates only path/rect direction values (used when axis orientation changes).
        /// </summary>
        /// <remarks>
        /// Recalculates geometry while preserving other properties like color and opacity.
        /// Triggered when axis orientation (horizontal/vertical) changes.
        /// </remarks>
        internal void UpdateDirection()
        {
            RendererShouldRender = true;
            GetSetStriplineOptions(true);
        }

        /// <summary>
        /// Apply customization changes (color, dash array, text, opacity) to already computed _options.
        /// </summary>
        /// <param name="property">Name of the property that changed (e.g., "Color", "Text").</param>
        /// <remarks>
        /// Allows efficient updates to specific style properties without full recalculation.
        /// Supported properties: Color, DashArray, Text, Opacity.
        /// </remarks>
        internal void UpdateCustomization(string property)
        {
            RendererShouldRender = true;
            bool isLineOption = Stripline?.SizeType == SizeType.Pixel;

            switch (property)
            {
                case "Color":
                    UpdateColorCustomization(isLineOption);
                    break;
                case "DashArray":
                    if (_striplinePath is { })
                    {
                        _striplinePath.StrokeDashArray = Stripline?.DashArray ?? string.Empty;
                    }
                    break;
                case "Text":
                    _striplineText.ForEach(element => element.Text = Stripline?.Text ?? string.Empty);
                    break;
                case "Opacity":
                    UpdateOpacityCustomization(isLineOption);
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Stub that invalidates this renderer's state to force a re-render.
        /// </summary>
        /// <remarks>Kept as explicit hook for future use in invalidation scenarios.</remarks>
        public void InvalidateRender()
        {
        }

        /// <summary>
        /// Handle layout changes from parent/chart. Kept as explicit hook for future use.
        /// </summary>
        /// <remarks>Called when parent chart layout changes; allows this renderer to respond accordingly.</remarks>
        public void HandleLayoutChange()
        {
        }
        #endregion
    }

    /// <summary>
    /// Specialized renderer for striplines rendered behind chart series.
    /// </summary>
    public class ChartStriplineBehindRenderer : ChartStriplineRenderer
    {
        #region Lifecycle Methods

        /// <summary>
        /// Initializes the behind-stripline renderer and registers it with the container.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            Owner?._striplineBehindContainer?.AddRenderer(this);
            if (Stripline is { })
            {
                Stripline.Renderer = this;
            }
        }
        #endregion

        /// <summary>
        /// Sets default renderer values from the behind-stripline container.
        /// </summary>
        internal override void SetDefaultRendererValues()
        {
            Owner?._striplineBehindContainer?.SetDefaultRendererContainerValues();
        }
    }

    /// <summary>
    /// Specialized renderer for striplines rendered over chart series.
    /// </summary>
    public class ChartStriplineOverRenderer : ChartStriplineRenderer
    {
        #region Lifecycle Methods

        /// <summary>
        /// Initializes the over-stripline renderer and registers it with the container.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            Owner?._striplineOverContainer?.AddRenderer(this);
            if (Stripline is { })
            {
                Stripline.Renderer = this;
            }
        }
        #endregion

        /// <summary>
        /// Sets default renderer values from the over-stripline container.
        /// </summary>
        internal override void SetDefaultRendererValues()
        {
            Owner?._striplineOverContainer?.SetDefaultRendererContainerValues();
        }
    }
}
