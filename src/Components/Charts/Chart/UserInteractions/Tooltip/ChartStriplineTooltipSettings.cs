using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using Syncfusion.Blazor.Toolkit.Internal;
using System.Text.RegularExpressions;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Manages runtime behavior of stripline tooltips (show/hide, content building, and interop rendering).
    /// </summary>
    /// <remarks>
    /// This internal helper responds to mouse movement, resolves target striplines,
    /// builds content (with formatting and template parsing) and renders tooltips via JS interop.
    /// </remarks>
    internal class ChartStriplineTooltipSettings : TooltipBase
    {
        #region Fields

        private readonly SfChart? _chart;
        private string _striplineId = string.Empty;
        private ChartStriplineTooltip? _settings;
        private bool _isTooltipRendered;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartStriplineTooltipSettings"/> class.
        /// </summary>
        /// <param name="sfchart">The chart instance.</param>
        internal ChartStriplineTooltipSettings(SfChart sfchart) : base(sfchart)
        {
            Chart = sfchart;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Determines if any stripline tooltip is enabled across behind/over containers.
        /// </summary>
        /// <returns><c>true</c> if any stripline tooltip is enabled; otherwise <c>false</c>.</returns>
        private bool IsAnyStriplineTooltipEnabled()
        {
            bool behindStriplineEnabled = Chart?._striplineBehindContainer?.Elements?.OfType<ChartStripline>()
                .Any(s => s is not null && s.Visible && (s.StriplineTooltip?.Enable == true)) == true;

            bool overStriplineEnabled = Chart?._striplineOverContainer?.Elements?.OfType<ChartStripline>()
                .Any(s => s is not null && s.Visible && (s.StriplineTooltip?.Enable == true)) == true;

            return behindStriplineEnabled || overStriplineEnabled;
        }

        /// <summary>
        /// Performs a geometry hit test to resolve a stripline id from mouse coordinates.
        /// </summary>
        private bool TryFindStriplineIdByHitRect(double mouseX, double mouseY, out string id)
        {
            IEnumerable<ChartStriplineRenderer> over = Chart?._striplineOverContainer?.Renderers?.OfType<ChartStriplineRenderer>() ?? [];
            if (TryHitStriplineRects(over, mouseX, mouseY, out id))
            {
                return true;
            }

            IEnumerable<ChartStriplineRenderer> behind = Chart?._striplineBehindContainer?.Renderers?.OfType<ChartStriplineRenderer>() ?? [];
            return TryHitStriplineRects(behind, mouseX, mouseY, out id);
        }

        /// <summary>
        /// Tests renderer rectangles for a mouse hit.
        /// </summary>
        private static bool TryHitStriplineRects(IEnumerable<ChartStriplineRenderer> renderers, double x, double y, out string id)
        {
            foreach (ChartStriplineRenderer renderer in renderers)
            {
                foreach (RectOptions rectOption in renderer.StriplineRect)
                {
                    Rect rect = new(rectOption.X, rectOption.Y, rectOption.Width, rectOption.Height);
                    if (ChartHelper.WithInBounds(x, y, rect))
                    {
                        id = rectOption.Id;
                        return true;
                    }
                }
            }

            id = string.Empty;
            return false;
        }

        /// <summary>
        /// Gets an axis by name from the chart's axis container.
        /// </summary>
        private ChartAxis? GetAxisByName(string axisName)
        {
            if (string.IsNullOrEmpty(axisName) || Chart?._axisContainer?.Axes is null)
            {
                return null;
            }

            _ = Chart._axisContainer.Axes.TryGetValue(axisName, out ChartAxis? axis);
            return axis;
        }

        /// <summary>
        /// Renders or updates the stripline tooltip for the given axis/stripline.
        /// </summary>
        private async Task RenderOrUpdateTooltipAsync(ChartAxis axis, ChartStripline stripline)
        {
            _settings = stripline.StriplineTooltip;
            string header = string.IsNullOrEmpty(_settings?.Header) ? (stripline.Text ?? string.Empty) : _settings.Header;

            string start = GetFormattedValue(stripline.Start, axis);
            string end = GetFormattedValue(stripline.End, axis);
            string axisName = axis?.Name ?? string.Empty;
            string size = Convert.ToString(stripline.Size, CultureInfo.InvariantCulture) ?? string.Empty;

            bool isSegmented = stripline.IsSegmented;
            string segmentAxisName = string.Empty;
            string segmentStart = string.Empty;
            string segmentEnd = string.Empty;

            if (isSegmented)
            {
                segmentAxisName = stripline.SegmentAxisName ?? string.Empty;
                ChartAxis? segmentAxis = GetAxisByName(segmentAxisName) ?? axis;
                if (segmentAxis is not null)
                {
                    segmentStart = GetFormattedValue(stripline.SegmentStart, segmentAxis);
                    segmentEnd = GetFormattedValue(stripline.SegmentEnd, segmentAxis);
                }
            }

            string defaultText = BuildTooltipContent(start, end, axisName, size, isSegmented, segmentStart, segmentEnd, segmentAxisName, stripline, axis);

            HeaderText = header;
            FormattedText.Clear();
            FormattedText.Add(defaultText);
            Text = FormattedText;
            if (_settings is not null)
            {
                TextStyleModel textStyle = BuildTextStyle(_settings);
                SVGTooltip tooltipOptions = BuildSvgTooltip(_settings, header, stripline, textStyle);

                if (Chart is not null && Chart._isScriptLoaded)
                {
                    _isTooltipRendered = await SfBaseComponent.InvokeAsync<bool>(
                        Chart._chartJsModule!, Chart._chartJsInProcessModule!,
                        "renderStriplineTooltip",
                        [tooltipOptions, _settings.ShowHeaderLine, _striplineId, Chart._tooltip.FadeOutDuration]
                    ).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Builds the text style model using settings and theme fallbacks.
        /// </summary>
        private TextStyleModel BuildTextStyle(ChartStriplineTooltip settings)
        {
            return new TextStyleModel
            {
                Size = settings.TextStyle.GetFontSize(Chart?._chartThemeStyle ?? null!),
                FontFamily = settings.TextStyle.GetFontFamily(Chart?._chartThemeStyle ?? null!),
                FontWeight = settings.TextStyle.GetFontWeight(Chart?._chartThemeStyle ?? null!),
                FontStyle = settings.TextStyle?.FontStyle ?? string.Empty,
                Color = settings.TextStyle?.Color ?? string.Empty,
                Opacity = settings.TextStyle?.Opacity ?? 1
            };
        }

        /// <summary>
        /// Builds the SVGTooltip options payload for JS interop.
        /// </summary>
        private SVGTooltip BuildSvgTooltip(ChartStriplineTooltip settings, string header, ChartStripline stripline, TextStyleModel textStyle)
        {
            return new SVGTooltip
            {
                Opacity = settings.Opacity,
                Header = header,
                Content = (Text != null) ? [.. Text] : [],
                Fill = settings.Fill ?? string.Empty,
                Border = new TooltipBorderModel { Color = settings.Border?.Color ?? string.Empty, Width = settings.Border?.Width ?? 0 },
                EnableAnimation = Chart?._tooltip?.EnableAnimation ?? true,
                Location = new ToolLocationModel { X = Chart?._mouseX ?? 0, Y = Chart?._mouseY ?? 0 },
                Shared = true,
                CrosshairEnabled = false,
                Shapes = [TooltipShape.Circle],
                ClipBounds = new ToolLocationModel { X = Chart?.InitialRect.X ?? 0, Y = Chart?.InitialRect.Y ?? 0 },
                AreaBounds = new AreaBoundsModel
                {
                    X = (Chart?.InitialRect.X ?? 0) + (Chart?._secondaryElementOffset.Left ?? 0),
                    Y = (Chart?.InitialRect.Y ?? 0) + (Chart?._secondaryElementOffset.Top ?? 0),
                    Width = Chart?.InitialRect.Width ?? 0,
                    Height = Chart?.InitialRect.Height ?? 0
                },
                Palette = [stripline.Color ?? string.Empty],
                Template = null!,
                Data = null!,
                Theme = _chart?.Theme.ToString() ?? string.Empty,
                Offset = 0,
                TextStyle = textStyle,
                IsNegative = false,
                Inverted = Chart?._requireInvertedAxis ?? false,
                ArrowPadding = 0,
                AvailableSize = Chart?.AvailableSize ?? new Size(0, 0),
                Duration = Chart?._tooltip?.Duration ?? 300,
                IsCanvas = false,
                ControlName = "Chart",
                RX = 4,
                RY = 4,
                IsTextWrap = false,
                EnableRtl = Chart?.EnableRtl ?? false
            };
        }

        /// <summary>
        /// Formats axis values according to the axis value type and label format.
        /// </summary>
        private static string GetFormattedValue(object? value, ChartAxis axis)
        {
            if (axis is null || value is null)
            {
                return string.Empty;
            }

            string labelFormat = axis.LabelFormat;
            bool hasLabel = !string.IsNullOrEmpty(labelFormat);
            bool hasToken = hasLabel && labelFormat.Contains("{value}", StringComparison.OrdinalIgnoreCase);

            switch (axis.ValueType)
            {
                case ValueType.DateTime:
                case ValueType.DateTimeCategory:
                    {
                        DateTime dateTime = Convert.ToDateTime(value, CultureInfo.CurrentCulture);
                        if (!hasLabel)
                        {
                            return Intl.GetDateFormat(dateTime);
                        }

                        if (hasToken)
                        {
                            string text = dateTime.ToString(CultureInfo.CurrentCulture);
                            return labelFormat.Replace("{value}", text, StringComparison.OrdinalIgnoreCase);
                        }

                        return Intl.GetDateFormat(dateTime, labelFormat);
                    }

                case ValueType.Category:
                    {
                        string text = Convert.ToString(value, CultureInfo.CurrentCulture) ?? string.Empty;
                        return hasToken ? labelFormat.Replace("{value}", text, StringComparison.OrdinalIgnoreCase) : text;
                    }

                case ValueType.Logarithmic:
                case ValueType.Double:
                default:
                    {
                        double number = Convert.ToDouble(value, CultureInfo.InvariantCulture);
                        if (!hasLabel)
                        {
                            return Intl.GetNumericFormat(number);
                        }

                        if (hasToken)
                        {
                            string text = number.ToString(CultureInfo.CurrentCulture);
                            return labelFormat.Replace("{value}", text, StringComparison.OrdinalIgnoreCase);
                        }

                        return Intl.GetNumericFormat(number, labelFormat);
                    }
            }
        }

        /// <summary>
        /// Resolves the axis and stripline instances from a target id.
        /// </summary>
        private bool TryResolveStripline(string targetId, [NotNullWhen(true)] out ChartAxis? axis, [NotNullWhen(true)] out ChartStripline? stripline)
        {
            axis = null;
            stripline = null;

            if (string.IsNullOrEmpty(targetId) || !targetId.Contains("_stripline_", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            string[] tokens = targetId.Split("_stripline_", StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length != 2)
            {
                return false;
            }

            string[] segments = tokens[1].Split('_', StringSplitOptions.RemoveEmptyEntries);
            if (segments.Length < 3)
            {
                return false;
            }

            if (!int.TryParse(segments[^1], NumberStyles.Integer, CultureInfo.InvariantCulture, out int striplineIndex))
            {
                return false;
            }

            string axisId = string.Join('_', segments.Skip(2).Take(Math.Max(segments.Length - 3, 1)));
            bool isBehindStripline = segments[0].Contains("Behind", StringComparison.InvariantCulture);

            if (Chart?._axisContainer?.Axes is not null && Chart._axisContainer.Axes.TryGetValue(axisId, out ChartAxis? resolvedAxis))
            {
                axis = resolvedAxis;
                stripline = isBehindStripline
                    ? Chart?._striplineBehindContainer?.Elements.ElementAtOrDefault(striplineIndex) as ChartStripline
                    : Chart?._striplineOverContainer?.Elements.ElementAtOrDefault(striplineIndex) as ChartStripline;
            }

            return axis is not null && stripline is not null;
        }

        /// <summary>
        /// Constructs tooltip HTML content using default values or a user-defined template.
        /// </summary>
        private string BuildTooltipContent(
            string start,
            string end,
            string axisName,
            string size,
            bool isSegmented,
            string segmentStart,
            string segmentEnd,
            string segmentAxisName,
            ChartStripline? stripline,
            ChartAxis? axis)
        {
            if (!string.IsNullOrEmpty(_settings?.Content))
            {
                return ParseStriplineTemplate(stripline, axis, _settings.Content, start, end, segmentStart, segmentEnd, segmentAxisName, size);
            }

            if (string.IsNullOrEmpty(start) && string.IsNullOrEmpty(end) && (stripline?.IsRepeat ?? false))
            {
                return string.Format(CultureInfo.InvariantCulture, "Size: {0}<br>Axis Name: {1}", size, axisName);
            }

            StringBuilder template = new(128);
            _ = template.AppendFormat(CultureInfo.InvariantCulture, "Start: {0}<br>End: {1}<br>Axis Name: {2}", start, end, axisName);

            if (isSegmented)
            {
                _ = template.AppendFormat(CultureInfo.InvariantCulture, "<br>Segment Start: {0}<br>Segment End: {1}<br>Segment Axis Name: {2}", segmentStart, segmentEnd, segmentAxisName);
            }

            return template.ToString();
        }

        /// <summary>
        /// Parses user-defined template tokens and normalizes line breaks.
        /// </summary>
        private static string ParseStriplineTemplate(
            ChartStripline? stripline,
            ChartAxis? axis,
            string template,
            string start,
            string end,
            string segmentStart,
            string segmentEnd,
            string segmentAxisName,
            string size)
        {
            if (string.IsNullOrEmpty(template))
            {
                return string.Empty;
            }

            string format = template;
            format = format.Replace("${stripline.text}", stripline?.Text ?? string.Empty, StringComparison.OrdinalIgnoreCase)
                           .Replace("${stripline.start}", start ?? string.Empty, StringComparison.OrdinalIgnoreCase)
                           .Replace("${stripline.end}", end ?? string.Empty, StringComparison.OrdinalIgnoreCase)
                           .Replace("${axis.name}", axis?.Name ?? string.Empty, StringComparison.OrdinalIgnoreCase)
                           .Replace("${stripline.segmentStart}", segmentStart ?? string.Empty, StringComparison.OrdinalIgnoreCase)
                           .Replace("${stripline.segmentEnd}", segmentEnd ?? string.Empty, StringComparison.OrdinalIgnoreCase)
                           .Replace("${stripline.segmentAxisName}", segmentAxisName, StringComparison.OrdinalIgnoreCase)
                           .Replace("${stripline.size}", size ?? string.Empty, StringComparison.OrdinalIgnoreCase)
                           .Replace("\r\n", "\n", StringComparison.Ordinal)
                           .Replace("\r", "\n", StringComparison.Ordinal)
                           .Replace("\n", "<br/>", StringComparison.Ordinal)
                           .Replace("<br>", "<br/>", StringComparison.OrdinalIgnoreCase)
                           .Replace("<br />", "<br/>", StringComparison.OrdinalIgnoreCase)
                           .Replace("<br  />", "<br/>", StringComparison.OrdinalIgnoreCase);

            // Trim accidental leading break.
            format = Regex.Replace(format, "^(<br/>|<br>|</br>)", "", RegexOptions.IgnoreCase);

            return format;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Handles mouse move over the chart to render or hide stripline tooltips.
        /// </summary>
        /// <param name="targetID">The DOM id of the target element under the cursor.</param>
        internal async Task MouseMoveHandlerAsync(string targetID)
        {
            if (Chart is null ||
                !ChartHelper.WithInBounds(Chart._mouseX, Chart._mouseY, Chart.InitialRect) ||
                (!string.IsNullOrEmpty(targetID) && targetID.Contains("_Zooming_", StringComparison.OrdinalIgnoreCase)) ||
                (string.IsNullOrEmpty(targetID) && !IsAnyStriplineTooltipEnabled()))
            {
                await HideTooltipAsync().ConfigureAwait(false);
                return;
            }

            _striplineId = Chart.ID + "_stripline_tooltip";

            string elementId = targetID;
            if (!targetID.Contains("stripline", StringComparison.OrdinalIgnoreCase) &&
                !TryFindStriplineIdByHitRect(Chart._mouseX, Chart._mouseY, out elementId))
            {
                await HideTooltipAsync().ConfigureAwait(false);
                return;
            }

            if (TryResolveStripline(elementId, out ChartAxis? axis, out ChartStripline? stripline) &&
                (stripline.StriplineTooltip?.Enable ?? false))
            {
                await RenderOrUpdateTooltipAsync(axis, stripline).ConfigureAwait(false);
            }
            else
            {
                await HideTooltipAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Hides the stripline tooltip if rendered.
        /// </summary>
        internal async Task HideTooltipAsync()
        {
            if (Chart is not null && Chart._isScriptLoaded && _isTooltipRendered)
            {
                await SfBaseComponent.InvokeVoidAsync(
                    Chart._chartJsModule!, Chart._chartJsInProcessModule!,
                    "removeStriplineTooltip",
                    [_striplineId, Chart._tooltip.FadeOutDuration]
                ).ConfigureAwait(false);

                _isTooltipRendered = false;
                _settings = null;
            }
        }

        /// <summary>
        /// Releases references and state.
        /// </summary>
        internal override void Dispose()
        {
            base.Dispose();
            _settings = null;
        }

        #endregion
    }
}
