using System.Dynamic;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components.Rendering;
using System.Drawing;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Internal;
using System.Collections.ObjectModel;
using System.Collections.Concurrent;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Provides helper methods for chart layout calculations, theming, and rendering utilities.
    /// </summary>
    /// <remarks>
    /// This class contains only static helpers. Methods validate critical inputs and try to avoid
    /// unsafe string concatenation where possible. XML documentation preserves and augments existing tags.
    /// </remarks>
    public class ChartHelper
    {
        #region Constants
        private const int RGB_HEX_CODE = 6;
        private const string DEFAULT_COLOR = "white";
        private const string SPACE = " ";
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a thread-safe cache of measured character sizes indexed by font characteristics.
        /// </summary>
        internal static ConcurrentDictionary<string, Size> SizePerCharacter { get; set; } = new ConcurrentDictionary<string, Size>();

        /// <summary>
        /// Gets or sets a list of cached font keys used for performance optimization.
        /// </summary>
        internal static List<string> ChartFontKeys { get; set; } = new List<string>();

        /// <summary>
        /// Gets a read-only lookup table mapping characters to their approximate pixel widths.
        /// </summary>
        /// <remarks>
        /// Values represent relative widths for standard font rendering at 12px size.
        /// </remarks>
        internal static readonly IReadOnlyDictionary<char, double> FontWidthLookup = new ReadOnlyDictionary<char, double>(new Dictionary<char, double>()
        {
            ['0'] = 8.0, ['1'] = 8.0, ['2'] = 8.0, ['3'] = 8.0, ['4'] = 8.0, ['5'] = 8.0, ['6'] = 8.0, ['7'] = 8.0, ['8'] = 8.0, ['9'] = 8.0,
            ['!'] = 5.0, ['"'] = 7.0, ['#'] = 8.0, ['$'] = 8.0, ['%'] = 14.0, ['&'] = 10.0, ['\''] = 4.0, ['('] = 5.0, [')'] = 5.0, ['*'] = 7.0, ['+'] = 8.0,
            [','] = 4.0, ['-'] = 5.0, ['.'] = 4.0, ['/'] = 6.0, [':'] = 4.0, [';'] = 4.0, ['<'] = 8.0, ['='] = 8.0, ['>'] = 8.0, ['?'] = 7.0, ['@'] = 14.0,
            ['A'] = 9.0, ['B'] = 10.0, ['C'] = 10.0, ['D'] = 10.0, ['E'] = 9.0, ['F'] = 8.0, ['G'] = 10.0, ['H'] = 11.0, ['I'] = 5.0, ['J'] = 8.0, ['K'] = 10.0, ['L'] = 8.0, ['M'] = 12.0,
            ['N'] = 11.0, ['O'] = 11.0, ['P'] = 10.0, ['Q'] = 11.0, ['R'] = 10.0, ['S'] = 9.0, ['T'] = 9.0, ['U'] = 11.0, ['V'] = 9.0, ['W'] = 13.0, ['X'] = 9.0, ['Y'] = 8.0, ['Z'] = 9.0,
            ['['] = 5.0, ['\\'] = 6.0, [']'] = 5.0, ['^'] = 8.0, ['_'] = 8.0, ['`'] = 9.0,
            ['a'] = 9.0, ['b'] = 9.0, ['c'] = 8.0, ['d'] = 9.0, ['e'] = 8.0, ['f'] = 5.0, ['g'] = 9.0, ['h'] = 9.0, ['i'] = 4.0, ['j'] = 4.0, ['k'] = 8.0, ['l'] = 5.0, ['m'] = 14.0,
            ['n'] = 9.0, ['o'] = 9.0, ['p'] = 9.0, ['q'] = 9.0, ['r'] = 6.0, ['s'] = 7.0, ['t'] = 6.0, ['u'] = 9.0, ['v'] = 8.0, ['w'] = 12.0, ['x'] = 8.0, ['y'] = 8.0, ['z'] = 7.0,
            ['{'] = 5.0, ['|'] = 4.0, ['}'] = 5.0, ['~'] = 8.0, [' '] = 5.0
        });
        #endregion

        #region Private Methods

        /// <summary>
        /// Updates the cached coordinates for a dynamic text animation element.
        /// </summary>
        /// <param name="element">The text animation options to update.</param>
        /// <param name="locationX">The new X coordinate.</param>
        /// <param name="locationY">The new Y coordinate.</param>
        private static void UpdateExistingElement(DynamicTextAnimationOptions element, double locationX, double locationY)
        {
            element.PreLocationX = element.CurLocationX != locationX ? element.CurLocationX : locationX;
            element.PreLocationY = element.CurLocationY != locationY ? element.CurLocationY : locationY;
            element.CurLocationX = locationX;
            element.CurLocationY = locationY;
        }

        /// <summary>
        /// Measures multi-line text (split by &lt;br&gt;) and returns the aggregate size.
        /// </summary>
        /// <param name="originalText">The text that may contain line breaks.</param>
        /// <param name="font">The font settings used during measurement.</param>
        /// <returns>The measured size encompassing all lines.</returns>
        private static Size MeasureBreakText(string originalText, ChartFontOptions font)
        {
            originalText = originalText.Replace("<br/>", "<br>", StringComparison.InvariantCulture);
            List<string> textCollection = originalText.Split("<br>").ToList();
            double width = 0;
            double height = 0;

            foreach (string text in textCollection)
            {
                Size size = MeasureText(text, font);
                if (size is not null)
                {
                    width = Math.Max(width, size.Width);
                    height += size.Height;
                }
            }

            return new Size(width, height);
        }

        /// <summary>
        /// Retrieves the cached character size for a font and caches the value if missing.
        /// </summary>
        /// <param name="character">The character to measure.</param>
        /// <param name="font">The font settings used during measurement.</param>
        /// <returns>The measured character size.</returns>
        private static Size GetCharSize(char character, ChartFontOptions font)
        {
            string key = character + Constants.Underscore + font.FontWeight + Constants.Underscore + font.FontStyle + Constants.Underscore + font.FontFamily;
            try
            {
                if (SizePerCharacter.TryGetValue(key, out Size? charSize))
                {
                    return charSize ?? null!;
                }
                double charWidth;
                if (FontWidthLookup.TryGetValue(character, out charWidth))
                {
                    // Create the new size for this character
                    Size newSize = new Size { Width = charWidth * 6.25, Height = 130 };
                    // Thread-safe add operation using GetOrAdd (available in all .NET versions)
                    Size result = SizePerCharacter.GetOrAdd(key, newSize);
                    return result ?? null!;
                }
                else
                {
                    // Default size for characters not in Font dictionary
                    Size defaultSize = new Size { Width = 50, Height = 130 };
                    Size result = SizePerCharacter.GetOrAdd(key, defaultSize);
                    return result ?? null!;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Converts a CSS size string (px, rem, em, pt, %) to a numeric pixel value.
        /// </summary>
        /// <param name="size">The CSS size string.</param>
        /// <returns>The pixel value represented by the string.</returns>
        private static double PixelToNumber(string size)
        {
            if (!string.IsNullOrEmpty(size))
            {
                string upperSize = size.ToUpper(CultureInfo.InvariantCulture);
                string unit = Constants.NumRegex().Match(upperSize).ToString();
                double numericValue = Convert.ToDouble(Constants.NumRegex().Replace(upperSize, string.Empty), null);

                switch (unit.ToString())
                {
                    case "PX":
                        return numericValue;
                    case "REM":
                    case "EM":
                        return numericValue * 16;
                    case "PT":
                        return numericValue * 1.3333333333333333;
                    case "%":
                        return numericValue * 0.13;
                    default:
                        return 0;
                }
            }

            return 0;
        }

        /// <summary>
        /// Builds a chart theme style instance using the supplied brush and font details.
        /// </summary>
        /// <param name="axisLabel">Axis label color.</param>
        /// <param name="axisTitle">Axis title color.</param>
        /// <param name="axisLine">Axis line color.</param>
        /// <param name="majorGridLine">Major gridline color.</param>
        /// <param name="minorGridLine">Minor gridline color.</param>
        /// <param name="majorTickLine">Major tick line color.</param>
        /// <param name="minorTickLine">Minor tick line color.</param>
        /// <param name="chartTitle">Chart title color.</param>
        /// <param name="legendLabel">Legend label color.</param>
        /// <param name="background">Chart background color.</param>
        /// <param name="areaBorder">Plot area border color.</param>
        /// <param name="errorBar">Error bar color.</param>
        /// <param name="crosshairLine">Crosshair line color.</param>
        /// <param name="crosshairBackground">Crosshair background color.</param>
        /// <param name="crosshairFill">Crosshair fill color.</param>
        /// <param name="crosshairLabel">Crosshair label color.</param>
        /// <param name="tooltipFill">Tooltip background color.</param>
        /// <param name="tooltipBoldLabel">Tooltip bold label color.</param>
        /// <param name="tooltipLightLabel">Tooltip light label color.</param>
        /// <param name="tooltipHeaderLine">Tooltip header line color.</param>
        /// <param name="markerShadow">Marker shadow color.</param>
        /// <param name="selectionRectFill">Selection rectangle fill color.</param>
        /// <param name="selectionRectStroke">Selection rectangle stroke color.</param>
        /// <param name="selectionCircleStroke">Selection circle stroke color.</param>
        /// <param name="tabColor">Dashboard tab color.</param>
        /// <param name="nDLineColor">ND line color.</param>
        /// <param name="chartTitleSize">Chart title font size.</param>
        /// <param name="chartTitleFontWeight">Chart title font weight.</param>
        /// <param name="chartTitleFontFamily">Chart title font family.</param>
        /// <param name="axisLabelFontSize">Axis label font size.</param>
        /// <param name="axisLabelFontWeight">Axis label font weight.</param>
        /// <param name="axisLabelFontFamily">Axis label font family.</param>
        /// <param name="legendTextSize">Legend text size.</param>
        /// <param name="legendFontFamily">Legend font family.</param>
        /// <param name="legendFontWeight">Legend font weight.</param>
        /// <param name="crosshairTextSize">Crosshair text size.</param>
        /// <param name="crosshairFontFamily">Crosshair font family.</param>
        /// <param name="crosshairFontWeight">Crosshair font weight.</param>
        /// <param name="dataLabelSize">Data label font size.</param>
        /// <param name="dataLabelFontFamily">Data label font family.</param>
        /// <param name="dataLabelFontWeight">Data label font weight.</param>
        /// <param name="axisTitleFontSize">Axis title font size.</param>
        /// <param name="axisTitleFontFamily">Axis title font family.</param>
        /// <param name="axisTitleFontWeight">Axis title font weight.</param>
        /// <param name="chartSubTitle">Subtitle color.</param>
        /// <param name="chartSubTitleSize">Subtitle font size.</param>
        /// <param name="chartSubTitleFontFamily">Subtitle font family.</param>
        /// <param name="chartSubTitleFontWeight">Subtitle font weight.</param>
        /// <param name="striplineTextColor">Stripline text color.</param>
        /// <param name="striplineFontFamily">Stripline font family.</param>
        /// <param name="striplineFontSize">Stripline font size.</param>
        /// <param name="StriplineFontWeight">Stripline font weight.</param>
        /// <param name="bearFillColor">Bear candle color.</param>
        /// <param name="bullFillColor">Bull candle color.</param>
        /// <param name="tooltipTextSize">Tooltip text size.</param>
        /// <param name="tooltipFontFamily">Tooltip font family.</param>
        /// <param name="tooltipFontWeight">Tooltip font weight.</param>
        /// <param name="centerLabelFontSize">Center label font size.</param>
        /// <param name="centerLabelFontFamily">Center label font family.</param>
        /// <param name="centerLabelFontWeight">Center label font weight.</param>
        /// <returns>The composed <see cref="ChartThemeStyle"/>.</returns>
        private static ChartThemeStyle GetThemeStyle(string axisLabel, string axisTitle, string axisLine, string majorGridLine, string minorGridLine, string majorTickLine, string minorTickLine, string chartTitle, string legendLabel, string background, string areaBorder, string errorBar, string crosshairLine,
                    string crosshairBackground, string crosshairFill, string crosshairLabel, string tooltipFill, string tooltipBoldLabel, string tooltipLightLabel, string tooltipHeaderLine, string markerShadow, string selectionRectFill, string selectionRectStroke, string selectionCircleStroke, string tabColor, string nDLineColor,
                    string chartTitleSize = "15px", string chartTitleFontWeight = "500", string chartTitleFontFamily = "Segoe UI", string axisLabelFontSize = "12px", string axisLabelFontWeight = "Normal", string axisLabelFontFamily = "Segoe UI", string legendTextSize = "13px", string legendFontFamily = "Segoe UI", string legendFontWeight = "Normal",
                    string crosshairTextSize = "13px", string crosshairFontFamily = "Segoe UI", string crosshairFontWeight = "Normal", string dataLabelSize = "11px", string dataLabelFontFamily = "Segoe UI", string dataLabelFontWeight = "Normal", string axisTitleFontSize = "14px", string axisTitleFontFamily = "Segoe UI", string axisTitleFontWeight = "Normal",
                    string chartSubTitle = "#424242", string chartSubTitleSize = "11px", string chartSubTitleFontFamily = "Segoe UI", string chartSubTitleFontWeight = "500", string striplineTextColor = "#353535", string striplineFontFamily = "Segoe UI", string striplineFontSize = "12px", string StriplineFontWeight = "400", string bearFillColor = "#2ecd71",
                    string bullFillColor = "#e74c3d", string tooltipTextSize = "13px", string tooltipFontFamily = "Segoe UI", string tooltipFontWeight = "Normal", string centerLabelFontSize = "16px", string centerLabelFontFamily = "Segoe UI", string centerLabelFontWeight = "600")
        {
            return new ChartThemeStyle()
            {
                AxisLabel = axisLabel,
                AxisTitle = axisTitle,
                AxisLine = axisLine,
                MajorGridLine = majorGridLine,
                MinorGridLine = minorGridLine,
                MajorTickLine = majorTickLine,
                MinorTickLine = minorTickLine,
                ChartTitle = chartTitle,
                LegendLabel = legendLabel,
                Background = background,
                AreaBorder = areaBorder,
                ErrorBar = errorBar,
                CrosshairLine = crosshairLine,
                CrosshairBackground = crosshairBackground,
                CrosshairFill = crosshairFill,
                CrosshairLabel = crosshairLabel,
                TooltipFill = tooltipFill,
                TooltipBoldLabel = tooltipBoldLabel,
                TooltipLightLabel = tooltipLightLabel,
                TooltipHeaderLine = tooltipHeaderLine,
                MarkerShadow = markerShadow,
                SelectionRectFill = selectionRectFill,
                SelectionRectStroke = selectionRectStroke,
                SelectionCircleStroke = selectionCircleStroke,
                TabColor = tabColor,
                NDLineColor = nDLineColor,
                BearFillColor = bearFillColor,
                BullFillColor = bullFillColor,
                ChartTitleSize = chartTitleSize,
                ChartTitleFontWeight = chartTitleFontWeight,
                ChartTitleFontFamily = chartTitleFontFamily,
                AxisLabelFontSize = axisLabelFontSize,
                AxisLabelFontWeight = axisLabelFontWeight,
                AxisLabelFontFamily = axisLabelFontFamily,
                TooltipTextSize = tooltipTextSize,
                TooltipFontFamily = tooltipFontFamily,
                ToolTipFontWeight = tooltipFontWeight,
                LegendTextSize = legendTextSize,
                LegendFontFamily = legendFontFamily,
                LegendFontWeight = legendFontWeight,
                CrosshairTextSize = crosshairTextSize,
                CrosshairFontFamily = crosshairFontFamily,
                CrosshairFontWeight = crosshairFontWeight,
                DataLabelSize = dataLabelSize,
                DataLabelFontFamily = dataLabelFontFamily,
                DataLabelFontWeight = dataLabelFontWeight,
                AxisTitleFontSize = axisTitleFontSize,
                AxisTitleFontFamily = axisTitleFontFamily,
                AxisTitleFontWeight = axisTitleFontWeight,
                ChartSubTitle = chartSubTitle,
                ChartSubTitleSize = chartSubTitleSize,
                ChartSubTitleFontFamily = chartSubTitleFontFamily,
                ChartSubTitleFontWeight = chartSubTitleFontWeight,
                StriplineTextColor = striplineTextColor,
                StriplineFontSize = striplineFontSize,
                StriplineFontFamily = striplineFontFamily,
                StriplineFontWeight = StriplineFontWeight,
                CenterLabelFontSize = centerLabelFontSize,
                CenterLabelFontFamily = centerLabelFontFamily,
                CenterLabelFontWeight = centerLabelFontWeight
            };
        }

        /// <summary>
        /// Wraps text anywhere when the measured width exceeds the provided maximum width.
        /// </summary>
        /// <param name="currentLabel">The label to wrap.</param>
        /// <param name="maximumWidth">The maximum width allowed for each line.</param>
        /// <param name="font">The font options used for measurement.</param>
        /// <returns>A collection of wrapped text segments.</returns>
        private static List<string> TextWrapAnyWhere(string currentLabel, double maximumWidth, ChartFontOptions font)
        {
            double size = MeasureText(currentLabel, font).Width;
            List<string> labelCollection = [];

            if (Math.Round(size) <= Math.Round(maximumWidth))
            {
                labelCollection.Add(currentLabel);
                return labelCollection;
            }
            WrapLabelSegments(currentLabel, maximumWidth, font, labelCollection);

            return labelCollection;
        }

        /// <summary>
        /// Recursively wraps label segments by measuring each character until maximum width is exceeded.
        /// </summary>
        /// <param name="label">The text to wrap.</param>
        /// <param name="maximumWidth">The maximum width allowed per line in pixels.</param>
        /// <param name="font">The font options used for measurement.</param>
        /// <param name="labelCollection">The output collection to append wrapped segments to.</param>
        private static void WrapLabelSegments(string label, double maximumWidth, ChartFontOptions font, ICollection<string> labelCollection)
        {
            string wrapLabel = string.Empty;
            int startIndex = 0;

            for (int index = 0; index < label.Length; index++)
            {
                string segment = label.Substring(startIndex, (index - startIndex) + 1);
                double segmentWidth = MeasureText(segment, font).Width;

                if (segmentWidth <= maximumWidth)
                {
                    wrapLabel = segment;

                    if (index == label.Length - 1)
                    {
                        labelCollection.Add(TextTrim(maximumWidth, segment, font));
                    }

                    continue;
                }

                if (!string.IsNullOrEmpty(wrapLabel))
                {
                    labelCollection.Add(TextTrim(maximumWidth, wrapLabel, font));
                }

                startIndex = index;
                index--;
            }
        }

        /// <summary>
        /// Calculates legend shape directions for the specified series marker shape.
        /// </summary>
        /// <param name="location">Legend location.</param>
        /// <param name="size">Legend size.</param>
        /// <param name="shape">Marker shape name.</param>
        /// <param name="options">Existing path options to update.</param>
        /// <returns>The path options with updated directives.</returns>
        private static PathOptions CalculateLegendShapes(ChartEventLocation location, Size size, string shape, PathOptions options)
        {
            if (string.IsNullOrWhiteSpace(shape))
            {
                return options;
            }

            CultureInfo culture = CultureInfo.InvariantCulture;
            double width = size.Width;
            double height = size.Height;
            double locationX = location.X;
            double locationY = location.Y;

            switch (shape)
            {
                case "MultiColoredLine":
                case "Line":
                case "StackingLine":
                case "StackingLine100":
                    SetLineLegend(culture, options, width, height, locationX, locationY);
                    break;
                case "StepLine":
                    SetStepLineLegend(culture, options, width, height, locationX, locationY);
                    break;
                case "RightArrow":
                    SetRightArrowLegend(culture, options, width, height, locationX, locationY);
                    break;
                case "LeftArrow":
                    SetLeftArrowLegend(culture, options, width, height, locationX, locationY);
                    break;
                case "Column":
                case "StackingColumn":
                case "StackingColumn100":
                    SetColumnLegend(culture, options, width, height, locationX, locationY);
                    break;
                case "Bar":
                case "StackingBar":
                case "StackingBar100":
                    SetBarLegend(culture, options, width, height, locationX, locationY);
                    break;
                case "Spline":
                    SetSplineLegend(culture, options, width, height, locationX, locationY);
                    break;
                case "Area":
                case "MultiColoredArea":
                case "StackingArea":
                case "StackingArea100":
                    SetAreaLegend(culture, options, width, height, locationX, locationY);
                    break;
                case "SplineArea":
                    SetSplineAreaLegend(culture, options, width, height, locationX, locationY);
                    break;
                case "Pie":
                case "Doughnut":
                    options.Stroke = "transparent";
                    options.Direction = GetAccumulationLegend(locationX, locationY, Math.Min(height, width) / 2, height, width);
                    break;
            }

            return options;
        }

        /// <summary>
        /// Generates SVG path directive for line legend shapes.
        /// </summary>
        /// <param name="culture">Culture info for number formatting.</param>
        /// <param name="options">Path options to update.</param>
        /// <param name="width">Shape width.</param>
        /// <param name="height">Shape height.</param>
        /// <param name="locationX">X coordinate.</param>
        /// <param name="locationY">Y coordinate.</param>
        /// <returns>Updated path options.</returns>
        private static PathOptions SetLineLegend(CultureInfo culture, PathOptions options, double width, double height, double locationX, double locationY)
        {
            double offset = width / 4;
            double startX = (locationX + (-width + (offset)));
            double endX = (locationX + (width - (offset)));

            options.Direction = "M" + SPACE + (startX.ToString(culture) + SPACE + locationY.ToString(culture) + SPACE + 'L' + SPACE + endX.ToString(culture) + SPACE + locationY.ToString(culture));
            return options;
        }

        /// <summary>
        /// Generates SVG path directive for step line legend shapes.
        /// </summary>
        /// <param name="culture">Culture info for number formatting.</param>
        /// <param name="options">Path options to update.</param>
        /// <param name="width">Shape width.</param>
        /// <param name="height">Shape height.</param>
        /// <param name="locationX">X coordinate.</param>
        /// <param name="locationY">Y coordinate.</param>
        /// <returns>Updated path options.</returns>
        private static PathOptions SetStepLineLegend(CultureInfo culture, PathOptions options, double width, double height, double locationX, double locationY)
        {
            options.Fill = "transparent";
            options.Direction = "M" + SPACE + (locationX + (-width / 2) - 2.5).ToString(culture) + SPACE + (locationY + (height / 2) - 1).ToString(culture)
                + SPACE + 'L' + SPACE + (locationX + (-width / 2) + (width / 10)).ToString(culture) + SPACE + (locationY + (height / 2) - 1).ToString(culture)
                + SPACE + 'L' + SPACE + (locationX + (-width / 2) + (width / 10)).ToString(culture) + SPACE + locationY.ToString(culture) + SPACE + 'L' + SPACE + (locationX + (-width / 10)).ToString(culture) + SPACE + locationY.ToString(culture)
                + SPACE + 'L' + SPACE + (locationX + (-width / 10)).ToString(culture) + SPACE + (locationY + (height / 2) - 1).ToString(culture) + SPACE + 'L' + SPACE + (locationX + (width / 5)).ToString(culture) + SPACE + (locationY + (height / 2) - 1).ToString(culture)
                + SPACE + 'L' + SPACE + (locationX + (width / 5)).ToString(culture) + SPACE + (locationY + (-height / 2)).ToString(culture) + SPACE + 'L' + SPACE + (locationX + (width / 2)).ToString(culture) + SPACE + (locationY + (-height / 2)).ToString(culture)
                + 'L' + SPACE + (locationX + (width / 2)).ToString(culture) + SPACE + (locationY + (height / 2) - 1).ToString(culture) + SPACE + 'L' + string.Empty + (locationX + (width / 2) + 2.5).ToString(culture) + SPACE + (locationY + (height / 2) - 1).ToString(culture);

            return options;
        }

        /// <summary>
        /// Generates SVG path directive for right arrow legend shapes.
        /// </summary>
        /// <param name="culture">Culture info for number formatting.</param>
        /// <param name="options">Path options to update.</param>
        /// <param name="width">Shape width.</param>
        /// <param name="height">Shape height.</param>
        /// <param name="locationX">X coordinate.</param>
        /// <param name="locationY">Y coordinate.</param>
        /// <returns>Updated path options.</returns>
        private static PathOptions SetRightArrowLegend(CultureInfo culture, PathOptions options, double width, double height, double locationX, double locationY)
        {
            options.Direction = "M" + SPACE + (locationX + (-width / 2)).ToString(culture) + SPACE + (locationY - (height / 2)).ToString(culture) + SPACE + 'L'
                + SPACE + (locationX + (width / 2)).ToString(culture) + SPACE + locationY.ToString(culture) + SPACE + 'L' + SPACE + (locationX + (-width / 2)).ToString(culture)
                + SPACE + (locationY + (height / 2)).ToString(culture) + " L" + SPACE + (locationX + (-width / 2)).ToString(culture) + SPACE + (locationY + (height / 2) - 2).ToString(culture)
                + SPACE + 'L' + SPACE + (locationX + (width / 2) - 4).ToString(culture) + SPACE + locationY.ToString(culture) + " L" + (locationX + (-width / 2)).ToString(culture) + SPACE + (locationY - (height / 2) + 2).ToString(culture) + " Z";

            return options;
        }

        /// <summary>
        /// Generates SVG path directive for left arrow legend shapes.
        /// </summary>
        /// <param name="culture">Culture info for number formatting.</param>
        /// <param name="options">Path options to update.</param>
        /// <param name="width">Shape width.</param>
        /// <param name="height">Shape height.</param>
        /// <param name="locationX">X coordinate.</param>
        /// <param name="locationY">Y coordinate.</param>
        /// <returns>Updated path options.</returns>
        private static PathOptions SetLeftArrowLegend(CultureInfo culture, PathOptions options, double width, double height, double locationX, double locationY)
        {
            options.Fill = options.Stroke;
            options.Stroke = "transparent";
            options.Direction = "M" + SPACE + (locationX + (width / 2)).ToString(culture) + SPACE + (locationY - (height / 2)).ToString(culture) + SPACE + 'L' + SPACE + (locationX + (-width / 2)).ToString(culture)
                + SPACE + locationY.ToString(culture) + SPACE + 'L' + SPACE + (locationX + (width / 2)).ToString(culture) + SPACE + (locationY + (height / 2)).ToString(culture) + SPACE + 'L'
                + SPACE + (locationX + (width / 2)).ToString(culture) + SPACE + (locationY + (height / 2) - 2).ToString(culture) + " L" + SPACE + (locationX + (-width / 2) + 4).ToString(culture)
                + SPACE + locationY.ToString(culture) + " L" + (locationX + (width / 2)).ToString(culture) + SPACE + (locationY - (height / 2) + 2).ToString(culture) + " Z";

            return options;
        }

        /// <summary>
        /// Generates SVG path directive for column legend shapes.
        /// </summary>
        /// <param name="culture">Culture info for number formatting.</param>
        /// <param name="options">Path options to update.</param>
        /// <param name="width">Shape width.</param>
        /// <param name="height">Shape height.</param>
        /// <param name="locationX">X coordinate.</param>
        /// <param name="locationY">Y coordinate.</param>
        /// <returns>Updated path options.</returns>
        private static PathOptions SetColumnLegend(CultureInfo culture, PathOptions options, double width, double height, double locationX, double locationY)
        {
            options.Direction = "M" + SPACE + (locationX - 3 * (width / 5)).ToString(culture) + SPACE + (locationY - (height / 5)).ToString(culture) + SPACE + 'L' + SPACE + (locationX + 3 * (-width / 10)).ToString(culture) + SPACE + (locationY - (height / 5)).ToString(culture) + SPACE + 'L'
                + SPACE + (locationX + 3 * (-width / 10)).ToString(culture) + SPACE + (locationY + (height / 2)).ToString(culture) + SPACE + 'L' + SPACE + (locationX - 3 * (width / 5)).ToString(culture) + SPACE + (locationY + (height / 2)).ToString(culture) + SPACE + 'Z' + SPACE + 'M'
                + SPACE + (locationX + (-width / 10) - (width / 20)).ToString(culture) + SPACE + (locationY - (height / 4) - 5).ToString(culture) + SPACE + 'L' + SPACE + (locationX + (width / 10) + (width / 20)).ToString(culture) + SPACE + (locationY - (height / 4) - 5).ToString(culture)
                + SPACE + 'L' + SPACE + (locationX + (width / 10) + (width / 20)).ToString(culture) + SPACE + (locationY + (height / 2)).ToString(culture) + SPACE + 'L' + SPACE + (locationX + (-width / 10) - (width / 20)).ToString(culture) + SPACE + (locationY + (height / 2)).ToString(culture)
                + SPACE + 'Z' + SPACE + 'M' + SPACE + (locationX + 3 * (width / 10)).ToString(culture) + SPACE + locationY.ToString(culture) + SPACE + 'L' + SPACE + (locationX + 3 * (width / 5)).ToString(culture) + SPACE + locationY.ToString(culture) + SPACE + 'L'
                + SPACE + (locationX + 3 * (width / 5)).ToString(culture) + SPACE + (locationY + (height / 2)).ToString(culture) + SPACE + 'L' + SPACE + (locationX + 3 * (width / 10)).ToString(culture) + SPACE + (locationY + (height / 2)).ToString(culture) + SPACE + 'Z';

            return options;
        }

        /// <summary>
        /// Generates SVG path directive for bar legend shapes.
        /// </summary>
        /// <param name="culture">Culture info for number formatting.</param>
        /// <param name="options">Path options to update.</param>
        /// <param name="width">Shape width.</param>
        /// <param name="height">Shape height.</param>
        /// <param name="locationX">X coordinate.</param>
        /// <param name="locationY">Y coordinate.</param>
        /// <returns>Updated path options.</returns>
        private static PathOptions SetBarLegend(CultureInfo culture, PathOptions options, double width, double height, double locationX, double locationY)
        {
            options.Direction = "M" + SPACE + (locationX + (-width / 2) + (-2.5)).ToString(culture) + SPACE + (locationY - 3 * (height / 5)).ToString(culture) + SPACE + 'L' + SPACE + (locationX + 3 * (width / 10)).ToString(culture) + SPACE + (locationY - 3 * (height / 5)).ToString(culture)
                + SPACE + 'L' + SPACE + (locationX + 3 * (width / 10)).ToString(culture) + SPACE + (locationY - 3 * (height / 10)).ToString(culture) + SPACE + 'L' + SPACE + (locationX - (width / 2) + (-2.5)).ToString(culture) + SPACE + (locationY - 3 * (height / 10)).ToString(culture)
                + SPACE + 'Z' + SPACE + 'M' + SPACE + (locationX + (-width / 2) + (-2.5)).ToString(culture) + SPACE + (locationY - (height / 5) + 0.5).ToString(culture) + SPACE + 'L' + SPACE + (locationX + (width / 2) + 2.5).ToString(culture) + SPACE + (locationY - (height / 5) + 0.5).ToString(culture)
                + SPACE + 'L' + SPACE + (locationX + (width / 2) + 2.5).ToString(culture) + SPACE + (locationY + (height / 10) + 0.5).ToString(culture) + SPACE + 'L' + SPACE + (locationX - (width / 2) + (-2.5)).ToString(culture) + SPACE + (locationY + (height / 10) + 0.5).ToString(culture)
                + SPACE + 'Z' + SPACE + 'M' + SPACE + (locationX - (width / 2) + (-2.5)).ToString(culture) + SPACE + (locationY + (height / 5) + 1).ToString(culture) + SPACE + 'L' + SPACE + (locationX + (-width / 4)).ToString(culture) + SPACE + (locationY + (height / 5) + 1).ToString(culture)
                + SPACE + 'L' + SPACE + (locationX + (-width / 4)).ToString(culture) + SPACE + (locationY + (height / 2) + 1).ToString(culture) + SPACE + 'L' + SPACE + (locationX - (width / 2) + (-2.5)).ToString(culture) + SPACE + (locationY + (height / 2) + 1).ToString(culture) + SPACE + 'Z';

            return options;
        }

        /// <summary>
        /// Generates SVG path directive for spline legend shapes.
        /// </summary>
        /// <param name="culture">Culture info for number formatting.</param>
        /// <param name="options">Path options to update.</param>
        /// <param name="width">Shape width.</param>
        /// <param name="height">Shape height.</param>
        /// <param name="locationX">X coordinate.</param>
        /// <param name="locationY">Y coordinate.</param>
        /// <returns>Updated path options.</returns>
        private static PathOptions SetSplineLegend(CultureInfo culture, PathOptions options, double width, double height, double locationX, double locationY)
        {
            options.Fill = "transparent";
            options.Direction = "M" + SPACE + (locationX - (width / 2)).ToString(culture) + SPACE + (locationY + (height / 5)).ToString(culture) + SPACE + 'Q' + SPACE + locationX.ToString(culture)
                + SPACE + (locationY - height).ToString(culture) + SPACE + locationX.ToString(culture) + SPACE + (locationY + (height / 5)).ToString(culture) + SPACE + 'M' + SPACE + locationX.ToString(culture)
                + SPACE + (locationY + (height / 5)).ToString(culture) + SPACE + 'Q' + SPACE + (locationX + (width / 2)).ToString(culture) + SPACE + (locationY + (height / 2)).ToString(culture)
                + SPACE + (locationX + (width / 2)).ToString(culture) + SPACE + (locationY - (height / 2)).ToString(culture);

            return options;
        }

        /// <summary>
        /// Generates SVG path directive for area legend shapes.
        /// </summary>
        /// <param name="culture">Culture info for number formatting.</param>
        /// <param name="options">Path options to update.</param>
        /// <param name="width">Shape width.</param>
        /// <param name="height">Shape height.</param>
        /// <param name="locationX">X coordinate.</param>
        /// <param name="locationY">Y coordinate.</param>
        /// <returns>Updated path options.</returns>
        private static PathOptions SetAreaLegend(CultureInfo culture, PathOptions options, double width, double height, double locationX, double locationY)
        {
            options.Direction = "M" + SPACE + (locationX - (width / 2) - 2.5).ToString(culture) + SPACE + (locationY + (height / 2) - 1).ToString(culture)
                + SPACE + 'L' + SPACE + (locationX + (-width / 4) + (-1.25)).ToString(culture) + SPACE + (locationY - (height / 2)).ToString(culture)
                + SPACE + 'L' + SPACE + locationX.ToString(culture) + SPACE + (locationY + (height / 4) - 1).ToString(culture) + SPACE + 'L' + SPACE + (locationX + (width / 4) + 1.25).ToString(culture)
                + SPACE + (locationY + (-height / 2) + (height / 4)).ToString(culture) + SPACE + 'L' + SPACE + (locationX + (height / 2) + 2.5).ToString(culture) + SPACE + (locationY + (height / 2) - 1).ToString(culture) + SPACE + 'Z';

            return options;
        }

        /// <summary>
        /// Generates SVG path directive for spline area legend shapes.
        /// </summary>
        /// <param name="culture">Culture info for number formatting.</param>
        /// <param name="options">Path options to update.</param>
        /// <param name="width">Shape width.</param>
        /// <param name="height">Shape height.</param>
        /// <param name="locationX">X coordinate.</param>
        /// <param name="locationY">Y coordinate.</param>
        /// <returns>Updated path options.</returns>
        private static PathOptions SetSplineAreaLegend(CultureInfo culture, PathOptions options, double width, double height, double locationX, double locationY)
        {
            options.Direction = "M" + SPACE + (locationX - (width / 2)).ToString(culture) + SPACE + (locationY + (height / 5)).ToString(culture) + SPACE + 'Q' + SPACE + locationX.ToString(culture) + SPACE + (locationY - height).ToString(culture) + SPACE + locationX.ToString(culture) + SPACE + (locationY + (height / 5)).ToString(culture) + SPACE + 'Z' + SPACE + 'M'
                        + SPACE + locationX.ToString(culture) + SPACE + (locationY + (height / 5)).ToString(culture) + SPACE + 'Q' + SPACE + (locationX + (width / 2)).ToString(culture) + SPACE + (locationY + (height / 2)).ToString(culture) + SPACE + (locationX + (width / 2)).ToString(culture) + SPACE + (locationY - (height / 2)).ToString(culture) + SPACE + " Z";

            return options;
        }

        /// <summary>
        /// Builds the accumulation legend path used for pie and doughnut charts.
        /// </summary>
        /// <param name="locX">Center X.</param>
        /// <param name="locY">Center Y.</param>
        /// <param name="radius">Radius for the legend arc.</param>
        /// <param name="height">Legend height.</param>
        /// <param name="width">Legend width.</param>
        /// <returns>The SVG path command.</returns>
        private static string GetAccumulationLegend(double locX, double locY, double radius, double height, double width)
        {
            CultureInfo culture = CultureInfo.InvariantCulture;
            ChartEventLocation cartesianlarge = DegreeToLocation(270, radius, new ChartEventLocation(locX, locY));
            ChartEventLocation cartesiansmall = DegreeToLocation(270, radius, new ChartEventLocation(locX + (width / 10), locY));

            return "M" + SPACE + locX.ToString(culture) + SPACE + locY.ToString(culture) + SPACE + 'L' + SPACE + (locX + radius).ToString(culture) + SPACE + locY.ToString(culture)
                + SPACE + 'A' + SPACE + radius.ToString(culture) + SPACE + radius.ToString(culture) + SPACE + 0 + SPACE + 1 + SPACE + 1 + SPACE + cartesianlarge.X.ToString(culture)
                + SPACE + cartesianlarge.Y.ToString(culture) + SPACE + 'Z' + SPACE + 'M' + SPACE + (locX + (width / 10)).ToString(culture) + SPACE + (locY - (height / 10)).ToString(culture)
                + SPACE + 'L' + (locX + radius).ToString(culture) + SPACE + (locY - height / 10).ToString(culture) + SPACE + 'A' + SPACE + radius.ToString(culture) + SPACE + radius.ToString(culture)
                + SPACE + 0 + SPACE + 0 + SPACE + 0 + SPACE + cartesiansmall.X.ToString(culture) + SPACE + cartesiansmall.Y.ToString(culture) + SPACE + 'Z';
        }

        /// <summary>
        /// Converts an RGBA color string to a six-digit hexadecimal value.
        /// </summary>
        /// <param name="cssColor">RGBA or hex color string.</param>
        /// <returns>A hexadecimal color string.</returns>
        private static string rgbaToHex (string cssColor)
        {
            cssColor = cssColor.Trim();
            int left = cssColor.IndexOf('(', StringComparison.InvariantCulture);
            int right = cssColor.IndexOf(')', StringComparison.InvariantCulture);

            if (left < 0 || right < 0)
            {
                return cssColor;
            }

            string[] parts = cssColor.Substring(left + 1, right - left - 1).Split(',');

            int r = int.Parse(parts[0], CultureInfo.InvariantCulture);
            int g = int.Parse(parts[1], CultureInfo.InvariantCulture);
            int b = int.Parse(parts[2], CultureInfo.InvariantCulture);

            if (parts.Length >= 4)
            {
                bool isParseAlpha = double.TryParse(parts[3].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out double alpha);
                if (isParseAlpha && alpha <= 0.0)
                {
                    return "#FFFFFF";
                }
            }

            return $"#{r:x2}{g:x2}{b:x2}";
        }

        /// <summary>
        /// Converts an RGB color to its HSL representation.
        /// </summary>
        /// <param name="color">Source RGB color.</param>
        /// <param name="h">Resulting hue.</param>
        /// <param name="s">Resulting saturation.</param>
        /// <param name="l">Resulting lightness.</param>
        private static void RgbToHsl(Color color, out double h, out double s, out double l)
        {
            double r = color.R / 255.0;
            double g = color.G / 255.0;
            double b = color.B / 255.0;

            double max = Math.Max(r, Math.Max(g, b));
            double min = Math.Min(r, Math.Min(g, b));
            h = 0;
            s = 0;
            l = (max + min) / 2.0;

            if (max != min)
            {
                double d = max - min;
                s = l > 0.5 ? d / (2.0 - max - min) : d / (max + min);
                if (max == r)
                {
                    h = (g - b) / d + (g < b ? 6 : 0);
                }
                else if (max == g)
                {
                    h = (b - r) / d + 2;
                }
                else if (max == b)
                {
                    h = (r - g) / d + 4;
                }
                h /= 6.0;
            }
        }

        /// <summary>
        /// Converts an HSL color to its RGB representation.
        /// </summary>
        /// <param name="h">Hue value.</param>
        /// <param name="s">Saturation value.</param>
        /// <param name="l">Lightness value.</param>
        /// <param name="r">Resulting red component.</param>
        /// <param name="g">Resulting green component.</param>
        /// <param name="b">Resulting blue component.</param>
        private static void HslToRgb(double h, double s, double l, out int r, out int g, out int b)
        {
            double rd, gd, bd;
            if (s == 0)
            {
                rd = gd = bd = l;
            }
            else
            {
                double q = l < 0.5 ? l * (1 + s) : l + s - l * s;
                double p = 2 * l - q;
                rd = HueToRgb(p, q, h + 1.0 / 3.0);
                gd = HueToRgb(p, q, h);
                bd = HueToRgb(p, q, h - 1.0 / 3.0);
            }

            r = (int)Math.Round(rd * 255);
            g = (int)Math.Round(gd * 255);
            b = (int)Math.Round(bd * 255);
        }

        /// <summary>
        /// Converts a hue component value to an RGB channel value during HSL-to-RGB conversion.
        /// </summary>
        /// <param name="p">The p component from HSL formula.</param>
        /// <param name="q">The q component from HSL formula.</param>
        /// <param name="t">The hue component (adjusted).</param>
        /// <returns>The RGB channel value (0–1 range).</returns>
        /// <remarks>
        /// This helper normalizes the hue value to 0–1 range and applies piecewise interpolation.
        /// </remarks>
        private static double HueToRgb(double p, double q, double t)
        {
            if (t < 0)
            {
                t += 1;
            }
            if (t > 1)
            {
                t -= 1;
            }
            if (t < 1.0 / 6.0)
            {
                return p + (q - p) * 6 * t;
            }
            if (t < 1.0 / 2.0)
            {
                return q;
            }
            if (t < 2.0 / 3.0)
            {
                return p + (q - p) * (2.0 / 3.0 - t) * 6;
            }
            return p;
        }

        /// <summary>
        /// Renders text span fragments for SVG label collections.
        /// </summary>
        /// <param name="id">The span element identifier.</param>
        /// <param name="locationX">The X coordinate for the tspan.</param>
        /// <param name="textLocationCollection">Text and Y coordinate collection.</param>
        /// <param name="svgRenderer">The SVG renderer for sequence numbers.</param>
        /// <returns>A render fragment containing the tspans.</returns>
        private static RenderFragment RenderTSpan(string id, string locationX, List<TextLocation> textLocationCollection, SvgRendering svgRenderer)
        {
            return builder =>
            {
                for (int i = 0; i < textLocationCollection.Count; i++)
                {
                    builder.OpenElement(svgRenderer.Seq++, "tspan");
                    Dictionary<string, object> svgattributes = new Dictionary<string, object>
                    {
                        { "id", id },
                        { "x", locationX },
                        { "y", textLocationCollection[i].Y.ToString(CultureInfo.InvariantCulture) }
                    };
                    builder.AddMultipleAttributes(svgRenderer.Seq++, svgattributes);
                    builder.AddContent(svgRenderer.Seq++, textLocationCollection[i].Text);
                    builder.CloseElement();
                }
            };
        }

        /// <summary>
        /// Builds a scrollbar theme style object.
        /// </summary>
        /// <param name="backRect">Scrollbar background color.</param>
        /// <param name="thumb">Scrollbar thumb color.</param>
        /// <param name="circle">Circle color.</param>
        /// <param name="circleHover">Circle hover color.</param>
        /// <param name="arrow">Arrow color.</param>
        /// <param name="grip">Grip color.</param>
        /// <param name="arrowHover">Arrow hover color.</param>
        /// <param name="backRectBorder">Background border color.</param>
        /// <returns>The scrollbar theme style.</returns>
        private static ScrollbarThemeStyle GetScrollbarStyle(string backRect, string thumb, string circle, string circleHover, string arrow, string grip, string arrowHover = null!, string backRectBorder = null!)
        {
            return new ScrollbarThemeStyle()
            {
                BackRect = backRect,
                Thumb = thumb,
                Circle = circle,
                CircleHover = circleHover,
                Arrow = arrow,
                Grip = grip,
                ArrowHover = arrowHover,
                BackRectBorder = backRectBorder
            };
        }

        /// <summary>
        /// Converts degrees to radians.
        /// </summary>
        /// <param name="degree">Degree value.</param>
        /// <returns>The radian equivalent.</returns>
        private static double DegreeToRadian(double degree)
        {
            return degree * (Math.PI / 180);
        }

        /// <summary>
        /// Builds plus direction string.
        /// </summary>
        /// <returns>SVG path for plus.</returns>
        private static string BuildPlusDirection(CultureInfo culture, double locationX, double locationY, double width, double height, double x, double y)
        {
            return "M" + SPACE + x.ToString(culture) + SPACE + locationY.ToString(culture) + SPACE + "L" + SPACE + (locationX + (width / 2)).ToString(culture) + SPACE + locationY.ToString(culture)
                + SPACE + 'M' + SPACE + locationX.ToString(culture) + SPACE + (locationY + (height / 2)).ToString(culture) + SPACE + 'L' + SPACE + locationX.ToString(culture) + SPACE + y.ToString(culture);
        }

        /// <summary>
        /// Builds cross direction string.
        /// </summary>
        /// <returns>SVG path for cross.</returns>
        private static string BuildCrossDirection(CultureInfo culture, double locationX, double locationY, double width, double height, double x, double y)
        {
            return "M" + SPACE + x.ToString(culture) + SPACE + y.ToString(culture) + SPACE + "L" + SPACE + (locationX + (width / 2)).ToString(culture)
                + SPACE + (locationY + (height / 2)).ToString(culture) + SPACE + 'M' + SPACE + (locationX - (width / 2)).ToString(culture) + SPACE + (locationY + (height / 2)).ToString(culture)
                + SPACE + 'L' + SPACE + (locationX + (width / 2)).ToString(culture) + SPACE + y.ToString(culture);
        }

        /// <summary>
        /// Builds location Y direction string.
        /// </summary>
        /// <returns>SVG path for location Y.</returns>
        private static string BuildMultiplocationYDirection(CultureInfo culture, double width, double height, double x, double y)
        {
            return "M " + x.ToString(culture) + SPACE + y.ToString(culture) + " L " + (x + width).ToString(culture) + SPACE + (y + height).ToString(culture)
                + " M " + (x + width).ToString(culture) + SPACE + y.ToString(culture) + " L " + x.ToString(culture) + SPACE + (y + height).ToString(culture);
        }

        /// <summary>
        /// Builds horizontal line direction string.
        /// </summary>
        /// <returns>SVG path for horizontal line.</returns>
        private static string BuildHorizontalLineDirection(CultureInfo culture, double locationX, double locationY, double width, double x)
        {
            return "M" + SPACE + x.ToString(culture) + SPACE + locationY.ToString(culture) + SPACE + 'L' + SPACE + (locationX + (width / 2)).ToString(culture) + SPACE + locationY.ToString(culture);
        }

        /// <summary>
        /// Builds vertical line direction string.
        /// </summary>
        /// <returns>SVG path for vertical line.</returns>
        private static string BuildVerticalLineDirection(CultureInfo culture, double locationX, double locationY, double height)
        {
            return "M" + SPACE + locationX.ToString(culture) + SPACE + (locationY + (height / 2)).ToString(culture) + SPACE + 'L'
                + SPACE + locationX.ToString(culture) + SPACE + (locationY + (-height / 2)).ToString(culture);
        }

        /// <summary>
        /// Builds diamond direction string.
        /// </summary>
        /// <returns>SVG path for diamond.</returns>
        private static string BuildDiamondDirection(CultureInfo culture, double locationX, double locationY, double width, double height, double x)
        {
            return "M" + SPACE + x.ToString(culture) + SPACE + locationY.ToString(culture) + SPACE + 'L' + SPACE + locationX.ToString(culture) + SPACE + (locationY + (-height / 2)).ToString(culture)
                + SPACE + 'L' + SPACE + (locationX + (width / 2)).ToString(culture) + SPACE + locationY.ToString(culture) + SPACE + 'L' + SPACE + locationX.ToString(culture) + SPACE + (locationY + (height / 2)).ToString(culture)
                + SPACE + 'L' + SPACE + x.ToString(culture) + SPACE + locationY.ToString(culture) + " z";
        }

        /// <summary>
        /// Builds actual rect direction string.
        /// </summary>
        /// <returns>SVG path for actual rect.</returns>
        private static string BuildActualRectDirection(CultureInfo culture, double locationX, double locationY, double height, double width, double x)
        {
            return "M" + SPACE + x.ToString(culture) + SPACE + (locationY + (-height / 8)).ToString(culture) + SPACE + 'L' + SPACE + (locationX).ToString(culture)
                + SPACE + (locationY + (-height / 8)).ToString(culture) + SPACE + 'L' + SPACE + (locationX).ToString(culture) + SPACE + (locationY + (height / 8)).ToString(culture)
                + SPACE + 'L' + SPACE + x.ToString(culture) + SPACE + (locationY + (height / 8)).ToString(culture) + SPACE + 'L' + SPACE + x.ToString(culture) + SPACE + (locationY + (-height / 8)).ToString(culture) + " z";
        }

        /// <summary>
        /// Builds target rect direction string.
        /// </summary>
        /// <returns>SVG path for target rect.</returns>
        private static string BuildTargetRectDirection(CultureInfo culture, double locationX, double locationY, double height, double x)
        {
            return "M" + SPACE + x.ToString(culture) + SPACE + (locationY + (-height / 2)).ToString(culture) + SPACE + 'L' + SPACE + locationX.ToString(culture)
                + SPACE + (locationY + (-height / 2)).ToString(culture) + SPACE + 'L' + SPACE + locationX.ToString(culture) + SPACE + (locationY + (height / 2)).ToString(culture)
                + SPACE + 'L' + SPACE + x.ToString(culture) + SPACE + (locationY + (height / 2)).ToString(culture) + SPACE + 'L' + SPACE + x.ToString(culture) + SPACE + (locationY + (-height / 2)).ToString(culture) + " z";
        }

        /// <summary>
        /// Builds rectangle direction string.
        /// </summary>
        /// <returns>SVG path for rectangle.</returns>
        private static string BuildRectangleDirection(CultureInfo culture, double locationX, double locationY, double height, double width, double x)
        {
            return "M" + SPACE + x.ToString(culture) + SPACE + (locationY + (-height / 2)).ToString(culture) + SPACE + 'L' + SPACE + (locationX + (width / 2)).ToString(culture)
                + SPACE + (locationY + (-height / 2)).ToString(culture) + SPACE + 'L' + SPACE + (locationX + (width / 2)).ToString(culture) + SPACE + (locationY + (height / 2)).ToString(culture)
                + SPACE + 'L' + SPACE + x.ToString(culture) + SPACE + (locationY + (height / 2)).ToString(culture) + SPACE + 'L' + SPACE + x.ToString(culture) + SPACE + (locationY + (-height / 2)).ToString(culture) + " z";
        }

        /// <summary>
        /// Builds triangle direction string.
        /// </summary>
        /// <returns>SVG path for triangle.</returns>
        private static string BuildTriangleDirection(CultureInfo culture, double locationX, double locationY, double height, double width, double x)
        {
            return "M" + SPACE + x.ToString(culture) + SPACE + (locationY + (height / 2)).ToString(culture) + SPACE + 'L' + SPACE + locationX.ToString(culture)
                + SPACE + (locationY + (-height / 2)).ToString(culture) + SPACE + 'L' + SPACE + (locationX + (width / 2)).ToString(culture) + SPACE + (locationY + (height / 2)).ToString(culture)
                + SPACE + 'L' + SPACE + x.ToString(culture) + SPACE + (locationY + (height / 2)).ToString(culture) + " z";
        }

        /// <summary>
        /// Builds funnel direction string.
        /// </summary>
        /// <returns>SVG path for funnel.</returns>
        private static string BuildFunnelDirection(CultureInfo culture, double locationX, double locationY, double height, double width)
        {
            return "M" + SPACE + (locationX + (width / 2)).ToString(culture) + SPACE + (locationY - (height / 2)).ToString(culture) + SPACE + 'L' + SPACE + locationX.ToString(culture)
                + SPACE + (locationY + (height / 2)).ToString(culture) + SPACE + 'L' + SPACE + (locationX - (width / 2)).ToString(culture) + SPACE + (locationY - (height / 2)).ToString(culture)
                + SPACE + 'L' + SPACE + (locationX + (width / 2)).ToString(culture) + SPACE + (locationY - (height / 2)).ToString(culture) + " z";
        }

        /// <summary>
        /// Builds pentagon direction string.
        /// </summary>
        /// <returns>SVG path for pentagon.</returns>
        private static string BuildPentagonDirection(CultureInfo culture, double locationX, double locationY, double height, double width)
        {
            string dir = string.Empty;
            for (int i = 0; i <= 5; i++)
            {
                double xVal = (width / 2) * Math.Cos((Math.PI / 180) * (i * 72)),
                yVal = (height / 2) * Math.Sin((Math.PI / 180) * (i * 72));
                if (i == 0)
                {
                    dir = "M" + SPACE + (locationX + xVal).ToString(culture) + SPACE + (locationY + yVal).ToString(culture) + SPACE;
                }
                else
                {
                    dir = dir + 'L' + SPACE + (locationX + xVal).ToString(culture) + SPACE + (locationY + yVal).ToString(culture) + SPACE;
                }
            }

            return dir = dir + 'Z';
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Determines if the current point is within the visible range of the X-axis.
        /// </summary>
        /// <param name="previousPoint">The previous data point in the series.</param>
        /// <param name="currentPoint">The current data point being evaluated.</param>
        /// <param name="nextPoint">The next data point in the series.</param>
        /// <param name="xAxis">The X-axis renderer containing visible range information.</param>
        /// <returns><c>true</c> if any of the points are within the visible range; otherwise, <c>false</c>.</returns>
        internal static bool WithInRange(Point previousPoint, Point currentPoint, Point nextPoint, ChartAxisRenderer xAxis)
        {
            double currentX = xAxis.GetPointValue(currentPoint.XValue),
                previousX = previousPoint is not null ? xAxis.GetPointValue(previousPoint.XValue) : currentX,
                nextX = nextPoint is not null ? xAxis.GetPointValue(nextPoint.XValue) : currentX,
                xStart = Math.Floor(xAxis.VisibleRange.Start),
                xEnd = Math.Ceiling(xAxis.VisibleRange.End);

            return (previousX >= xStart && previousX <= xEnd) || (currentX >= xStart && currentX <= xEnd)
                || (nextX >= xStart && nextX <= xEnd) || (xStart >= previousX && xStart <= nextX);
        }

        /// <summary>
        /// Appends path animation elements to the chart's animation collection.
        /// </summary>
        /// <param name="chart">The chart instance containing the animation elements.</param>
        /// <param name="direction">The SVG path direction string.</param>
        /// <param name="id">The unique identifier for the path element.</param>
        /// <param name="parentId">The parent element identifier (optional).</param>
        /// <returns>The path direction string to be used for rendering.</returns>
        internal static string AppendPathElements(SfChart chart, string direction, string id, string parentId = "")
        {
            bool redraw = chart._redraw;
            chart._pathAnimationElements.TryGetValue(id, out DynamicPathAnimationOptions? pathOption);
            DynamicPathAnimationOptions existElement = redraw ? pathOption ?? null! : null!;
            string previous = string.Empty;

            if (existElement is not null)
            {
                previous = existElement.CurrentDir != direction ? existElement.CurrentDir : string.Empty;
                existElement.PreviousDir = previous;
                existElement.CurrentDir = direction;
                direction = !string.IsNullOrEmpty(existElement.PreviousDir) && redraw ? existElement.PreviousDir : direction;
            }
            else
            {
                DynamicPathAnimationOptions dynamicPath = new DynamicPathAnimationOptions { ParentId = parentId, Id = id, CurrentDir = direction, PreviousDir = previous };
                if (!chart._pathAnimationElements.TryAdd(id, dynamicPath))
                {
                    chart._pathAnimationElements[id] = dynamicPath;
                }
            }
            return direction;
        }

        /// <summary>
        /// Appends text animation elements to the chart's animation collection.
        /// </summary>
        /// <param name="chart">The chart instance containing the animation elements.</param>
        /// <param name="id">The unique identifier for the text element.</param>
        /// <param name="locationX">The X-coordinate of the text location.</param>
        /// <param name="locationY">The Y-coordinate of the text location.</param>
        /// <param name="x">The X attribute name (default: "x").</param>
        /// <param name="y">The Y attribute name (default: "y").</param>
        /// <returns>An array containing the string representation of X and Y coordinates.</returns>
        internal static string[] AppendTextElements(SfChart chart, string id, double locationX, double locationY, string x = "x", string y = "y")
        {
            bool redraw = chart._redraw;

            if (chart._textAnimationElements.TryGetValue(id, out DynamicTextAnimationOptions? existElement))
            {
                UpdateExistingElement(existElement, locationX, locationY);
                locationX = redraw ? existElement.PreLocationX : locationX;
                locationY = redraw ? existElement.PreLocationY : locationY;
            }
            else
            {
                existElement = new DynamicTextAnimationOptions
                {
                    CurLocationX = locationX,
                    PreLocationX = locationX,
                    PreLocationY = locationY,
                    CurLocationY = locationY,
                    Id = id,
                    X = x,
                    Y = y
                };
                chart._textAnimationElements[id] = existElement;
            }

            return new string[] { locationX.ToString(CultureInfo.InvariantCulture), locationY.ToString(CultureInfo.InvariantCulture) };
        }

        /// <summary>
        /// Converts a point value to a coefficient based on the axis orientation and range.
        /// </summary>
        /// <param name="pointValue">The value to convert.</param>
        /// <param name="size">The size dimension (width or height).</param>
        /// <param name="orientation">The axis orientation (Horizontal or Vertical).</param>
        /// <param name="visibleRange">The visible range of the axis.</param>
        /// <param name="isInversed">Indicates whether the axis is inversed.</param>
        /// <returns>The calculated coefficient value.</returns>
        internal static double GetValueByPoint(double pointValue, double size, Orientation orientation, DoubleRange visibleRange, bool isInversed)
        {
            bool isHorizontalNotInversed = orientation == Orientation.Horizontal && !isInversed;
            bool isVerticalInversed = orientation != Orientation.Horizontal && isInversed;
            double coefficient = (isHorizontalNotInversed || isVerticalInversed) ? pointValue / size : (1 - (pointValue / size));

            return (coefficient * visibleRange.Delta) + visibleRange.Start;
        }

        /// <summary>
        /// Appends rectangle animation elements to the chart's animation collection.
        /// </summary>
        /// <param name="chart">The chart instance containing the animation elements.</param>
        /// <param name="id">The unique identifier for the rectangle element.</param>
        /// <param name="rect">The rectangle to be animated.</param>
        /// <returns>The rectangle to be used for rendering (previous if redrawing, current otherwise).</returns>
        internal static Rect AppendRectElements(SfChart chart, string id, Rect rect)
        {
            Rect previousRect = null!;
            DynamicRectAnimationOptions existElement = chart._rectAnimationElements.Find(item => item.Id == id) ?? null!;

            if (existElement is not null)
            {
                previousRect = !existElement.CurrentRect.Equals(rect) ? existElement.CurrentRect : rect;
                existElement.PreviousRect = previousRect;
                existElement.CurrentRect = rect;
            }
            else
            {
                chart._rectAnimationElements.Add(new DynamicRectAnimationOptions { Id = id, CurrentRect = rect });
            }

            return (chart._redraw && previousRect is not null) ? previousRect : rect;
        }

        /// <summary>
        /// Measures the size of text with the specified font options.
        /// </summary>
        /// <param name="text">The text to measure.</param>
        /// <param name="font">The font options for text measurement.</param>
        /// <returns>The calculated size of the text.</returns>
        internal static Size MeasureText(string text, ChartFontOptions font)
        {
            if (text.Contains("<br>", StringComparison.InvariantCulture) || text.Contains("<br/>", StringComparison.InvariantCulture))
            {
                return MeasureBreakText(text, font);
            }

            double width = 0, height = 0, fontSize = PixelToNumber(font.Size);
            Size charSize;

            if (IsRTLText(text))
            {
                string key = text + Constants.Underscore + font.FontWeight + Constants.Underscore + font.FontStyle + Constants.Underscore + font.FontFamily;
                if (SizePerCharacter.TryGetValue(key, out Size? value))
                {
                    charSize = value;
                    return new Size(charSize.Width * (fontSize / 100), charSize.Height * (fontSize / 100));
                }
            }

            for (int i = 0; i < text.Length; i++)
            {
                charSize = GetCharSize(text[i], font);
                if (charSize is not null)
                {
                    width += charSize.Width > 0 ? charSize.Width : 100;
                    height = Math.Max(charSize.Height, height);
                }
            }

            return new Size((width * fontSize) / 100, (height * fontSize) / 100);
        }

        /// <summary>
        /// Determines whether the given text contains Right-to-Left (RTL) characters.
        /// </summary>
        /// <param name="text">The text to evaluate.</param>
        /// <returns><c>true</c> if the text contains RTL characters; otherwise, <c>false</c>.</returns>
        internal static bool IsRTLText(string text)
        {
            return text.Any(c => c >= 0x600 && c <= 0x6ff);
        }

        /// <summary>
        /// Splits and joins a label format string by removing the specified split text.
        /// </summary>
        /// <param name="label">The label format string.</param>
        /// <param name="splitText">The text to split and remove.</param>
        /// <returns>The processed label string.</returns>
        internal static string SplitLabelFormat(string label, string splitText)
        {
            return string.Join(string.Empty, label.Split(splitText).Where(s => !string.IsNullOrEmpty(s)).ToArray());
        }

        /// <summary>
        /// Checks if a value is NaN or zero.
        /// </summary>
        /// <param name="point">The value to check.</param>
        /// <returns><c>true</c> if the value is NaN or zero; otherwise, <c>false</c>.</returns>
        internal static bool IsNaNOrZero(double point)
        {
            return double.IsNaN(point) || point == 0;
        }

        /// <summary>
        /// Subtracts thickness from a rectangle's dimensions.
        /// </summary>
        /// <param name="rect">The original rectangle.</param>
        /// <param name="thickness">The thickness to subtract.</param>
        /// <returns>A new rectangle with adjusted dimensions.</returns>
        internal static Rect SubtractThickness(Rect rect, Thickness thickness)
        {
            rect.X += thickness.Left;
            rect.Y += thickness.Top;
            rect.Width -= thickness.Left + thickness.Right;
            rect.Height -= thickness.Top + thickness.Bottom;

            return rect;
        }

        /// <summary>
        /// Determines the color to use based on actual and theme colors.
        /// </summary>
        /// <param name="actualColor">The actual color specified by the user.</param>
        /// <param name="themeColor">The theme's default color.</param>
        /// <returns>The actual color if specified; otherwise, the theme color.</returns>
        internal static string FindThemeColor(string actualColor, string themeColor)
        {
            return string.IsNullOrEmpty(actualColor) ? themeColor : actualColor;
        }

        /// <summary>
        /// Gets the chart theme style configuration based on the theme name.
        /// </summary>
        /// <param name="theme">The theme name.</param>
        /// <param name="isAccChart">Indicates whether it's an accessibility chart.</param>
        /// <returns>The theme style configuration.</returns>
        internal static ChartThemeStyle GetChartThemeStyle(string theme)
        {
            if(theme != "FluentDark")
            {
                return GetThemeStyle("#616161", "#242424", "#D2D0CE", "#EDEBE9", "#EDEBE9", "#D2D0CE", "#D2DOCE", "#242424", "#242424", "#FFFFFF", "#EDEBE9", "#A19F9D", "#A19F9D", "rgba(138, 136, 134, 0.1)", "#FFFFFF", "#242424", "#FFFFFF", "#242424", "#242424", "#D2D0CE", null!, "rgba(180, 214, 250, 0.1)", "#0F6CBD", "#0F6CBD", "#424242", "#A19F9D",
                "14px", "600", "Segoe UI", "12px", "400", "Segoe UI", "12px", "Segoe UI", "400", "12px", "Segoe UI", "700", "12px", "Segoe UI", "400", "12px", "Segoe UI", "600", "#616161", "12px", "Segoe UI", "600", "#616161", "Segoe UI", "12px", "400", "#E7910F", "#0076E5", "12px", "Segoe UI", "600");                
            }
            else
            {
                return GetThemeStyle("#ADADAD", "#FFFFFF", "#3B3A39", "#292827", "#292827", "#3B3A39", "#3B3A39", "#FFFFFF", "#FFFFFF", "#1c1b1f", "#292827", "#8A8886", "#8A8886", "rgba(138, 136, 134, 0.1)", "#292929", "#FFFFFF", "#292929", "#FFFFFF", "#FFFFFF", "#3B3A39", null!, "rgba(14, 71, 117, 0.1)", "#115EA3", "#115EA3", "#D6D6D6", "#8A8886",
                "14px", "600", "Segoe UI", "12px", "400", "Segoe UI", "12px", "Segoe UI", "400", "12px", "Segoe UI", "700", "12px", "Segoe UI", "400", "12px", "Segoe UI", "600", "#ADADAD", "12px", "Segoe UI", "600", "#ADADAD", "Segoe UI", "12px", "400", "#584EC6", "#43B786", "12px", "Segoe UI", "600");
            }
        }

        /// <summary>
        /// Gets the series color palette for the specified theme.
        /// </summary>
        /// <returns>An array of color strings for the series.</returns>
        internal static string[] GetSeriesColor(string theme)
        {
            if(theme != "FluentDark")
            {
                return new string[] { "#6200EE", "#09AF74", "#0076E5", "#CB3587", "#E7910F", "#0364DE", "#66CD15", "#F3A93C", "#107C10", "#C19C00" };
            }
            else
            {
                return new string[] { "#9BB449", "#2A72D5", "#43B786", "#3F579A", "#584EC6", "#E85F9C", "#6E7A89", "#EA6266", "#0B6A0B", "#C19C00" };
            }
        }

        /// <summary>
        /// Checks if both minimum and maximum values are set for the axis.
        /// </summary>
        /// <param name="axis">The chart axis to check.</param>
        /// <returns><c>true</c> if both minimum and maximum are set; otherwise, <c>false</c>.</returns>
        internal static bool SetRange(ChartAxis axis)
        {
            return axis.Minimum is not null && axis.Maximum is not null;
        }

        /// <summary>
        /// Calculates the logarithm of a value with a specified base.
        /// </summary>
        /// <param name="point">The value to calculate the logarithm for.</param>
        /// <param name="baseValue">The logarithm base.</param>
        /// <returns>The logarithmic value.</returns>
        internal static double LogBase(double point, double baseValue)
        {
            if (point <= 0 || double.IsNaN(point))
            {
                return double.NaN;
            }
            return (baseValue is <= 0 or 1 || double.IsNaN(baseValue)) ? double.NaN : Math.Log(point, baseValue);
        }

        /// <summary>
        /// Checks if an interval value is within a specified range (inclusive).
        /// </summary>
        /// <param name="interval">The interval value to check.</param>
        /// <param name="range">The range to check against.</param>
        /// <returns><c>true</c> if the interval is within the range (inclusive); otherwise, <c>false</c>.</returns>
        internal static bool WithIn(double interval, DoubleRange range)
        {
            return (interval <= range.End) && (interval >= range.Start);
        }

        /// <summary>
        /// Checks if an interval value is inside a specified range (exclusive).
        /// </summary>
        /// <param name="interval">The interval value to check.</param>
        /// <param name="range">The range to check against.</param>
        /// <returns><c>true</c> if the interval is inside the range (exclusive); otherwise, <c>false</c>.</returns>
        internal static bool Inside(double interval, DoubleRange range)
        {
            return (interval < range.End) && (interval > range.Start);
        }

        /// <summary>
        /// Converts a value to a coefficient based on the axis renderer's visible range.
        /// </summary>
        /// <param name="point">The value to convert.</param>
        /// <param name="axisRenderer">The axis renderer containing range information.</param>
        /// <returns>The calculated coefficient value.</returns>
        internal static double ValueToCoefficient(double point, ChartAxisRenderer axisRenderer)
        {
            double result = (point - axisRenderer.VisibleRange.Start) / axisRenderer.VisibleRange.Delta;
            return axisRenderer.Axis is not null && axisRenderer.Axis.IsAxisInverse ? (1 - result) : result;
        }

        /// <summary>
        /// Converts a value to a coefficient based on minimum, delta, and inversion flag.
        /// </summary>
        /// <param name="point">The value to convert.</param>
        /// <param name="min">The minimum value of the range.</param>
        /// <param name="delta">The delta (range) value.</param>
        /// <param name="isInversed">Indicates whether the axis is inversed.</param>
        /// <returns>The calculated coefficient value.</returns>
        internal static double ValueToCoefficient(double point, double min, double delta, bool isInversed)
        {
            double result = (point - min) / delta;
            return isInversed ? (1 - result) : result;
        }

        /// <summary>
        /// Checks if a label contains a break tag.
        /// </summary>
        /// <param name="label">The label text to check.</param>
        /// <returns><c>true</c> if the label contains a break tag; otherwise, <c>false</c>.</returns>
        internal static bool IsBreakLabel(string label)
        {
            return label.Contains("<br>", StringComparison.InvariantCulture);
        }

        /// <summary>
        /// Gets a point location in the chart coordinate system.
        /// </summary>
        /// <param name="x">The X-coordinate value.</param>
        /// <param name="y">The Y-coordinate value.</param>
        /// <param name="XAxisRenderer">The X-axis renderer.</param>
        /// <param name="YAxisRenderer">The Y-axis renderer.</param>
        /// <param name="isInverted">Indicates whether the axes are inverted.</param>
        /// <returns>The calculated point location.</returns>
        internal static ChartEventLocation GetPoint(double x, double y, ChartAxisRenderer XAxisRenderer, ChartAxisRenderer YAxisRenderer, bool isInverted = false)
        {
            x = ValueToCoefficient(x, XAxisRenderer.VisibleRange.Start, XAxisRenderer.VisibleRange.Delta, XAxisRenderer.Axis is not null && XAxisRenderer.Axis.IsAxisInverse);
            y = ValueToCoefficient(y, YAxisRenderer.VisibleRange.Start, YAxisRenderer.VisibleRange.Delta, YAxisRenderer.Axis is not null && YAxisRenderer.Axis.IsAxisInverse);
            double xLength = isInverted ? XAxisRenderer.Rect.Height : XAxisRenderer.Rect.Width;
            double yLength = isInverted ? YAxisRenderer.Rect.Width : YAxisRenderer.Rect.Height;
            return new ChartEventLocation(isInverted ? y * yLength : x * xLength, isInverted ? (1 - x) * xLength : (1 - y) * yLength);
        }

        /// <summary>
        /// Gets a point location with specified axis lengths.
        /// </summary>
        /// <param name="x">The X-coordinate value.</param>
        /// <param name="y">The Y-coordinate value.</param>
        /// <param name="XAxisRenderer">The X-axis renderer.</param>
        /// <param name="YAxisRenderer">The Y-axis renderer.</param>
        /// <param name="xLength">The X-axis length.</param>
        /// <param name="yLength">The Y-axis length.</param>
        /// <param name="isInverted">Indicates whether the axes are inverted.</param>
        /// <returns>The calculated point location.</returns>
        internal static ChartEventLocation GetPoint(double x, double y, ChartAxisRenderer XAxisRenderer, ChartAxisRenderer YAxisRenderer, double xLength, double yLength, bool isInverted = false)
        {
            x = ValueToCoefficient(x, XAxisRenderer.VisibleRange.Start, XAxisRenderer.VisibleRange.Delta, XAxisRenderer.Axis is not null && XAxisRenderer.Axis.IsAxisInverse);
            y = ValueToCoefficient(y, YAxisRenderer.VisibleRange.Start, YAxisRenderer.VisibleRange.Delta, YAxisRenderer.Axis is not null && YAxisRenderer.Axis.IsAxisInverse);
            return new ChartEventLocation(isInverted ? y * yLength : x * xLength, isInverted ? (1 - x) * xLength : (1 - y) * yLength);
        }

        /// <summary>
        /// Transforms coordinates to visible polar/radar chart coordinates.
        /// </summary>
        /// <param name="x">The X-coordinate value.</param>
        /// <param name="y">The Y-coordinate value.</param>
        /// <param name="xAxis">The X-axis.</param>
        /// <param name="yAxis">The Y-axis.</param>
        /// <param name="series">The chart series.</param>
        /// <param name="radius">The radius value (optional).</param>
        /// <returns>The transformed visible location.</returns>
        internal static ChartEventLocation TransformToVisible(double x, double y, ChartAxis xAxis, ChartAxis yAxis, ChartSeries series, double radius = 0)
        {
            x = xAxis.ValueType == ValueType.Logarithmic ? LogBase(x > 1 ? x : 1, xAxis.LogBase) : x;
            y = yAxis.ValueType == ValueType.Logarithmic ? LogBase(y > 1 ? y : 1, yAxis.LogBase) : y;
            x += xAxis.ValueType == ValueType.Category && xAxis.LabelPlacement == LabelPlacement.BetweenTicks ? 0.5 : 0;

            radius = (series.Renderer.Owner?._axisContainer?.AxisLayout.Radius ?? 0) * ValueToCoefficient(y, yAxis.Renderer ?? null!);
            ChartEventLocation point = CoefficientToVector(ValueToPolarCoefficient(x, xAxis.Renderer ?? null!), xAxis.StartAngle);
            return new ChartEventLocation((((series.Renderer.ClipRect?.Width ?? 0) / 2) + (series.Renderer.ClipRect?.X ?? 0)) + (radius * point.X),
                (((series.Renderer.ClipRect?.Height ?? 0) / 2) + (series.Renderer.ClipRect?.Y ?? 0)) + (radius * point.Y));
        }

        /// <summary>
        /// Converts a coefficient to a vector using angle calculations.
        /// </summary>
        /// <param name="coefficient">The coefficient value.</param>
        /// <param name="startAngle">The starting angle in degrees.</param>
        /// <returns>The calculated vector location.</returns>
        internal static ChartEventLocation CoefficientToVector(double coefficient, double startAngle)
        {
            startAngle = startAngle < 0 ? startAngle + 360 : startAngle;
            double angle = (Math.PI * (1.5 - (2 * coefficient))) + (startAngle * Math.PI) / 180;
            return new ChartEventLocation(Math.Cos(angle), Math.Sin(angle));
        }

        /// <summary>
        /// Gets title text lines with proper wrapping or trimming based on style.
        /// </summary>
        /// <param name="title">The title text.</param>
        /// <param name="style">The font style options.</param>
        /// <param name="width">The maximum width available.</param>
        /// <returns>A list of title text lines.</returns>
        internal static List<string> GetTitle(string title, ChartFontOptions style, double width)
        {
            List<string> titleCollection = new List<string>();
            switch (style.TextOverflow)
            {
                case TextOverflow.Wrap:
                    titleCollection = TextWrap(title, width, style);
                    break;
                case TextOverflow.Trim:
                    titleCollection.Add(TextTrim(width, title, style));
                    break;
                default:
                    titleCollection.Add(title);
                    break;
            }

            return titleCollection;
        }

        /// <summary>
        /// Wraps text to fit within a maximum width.
        /// </summary>
        /// <param name="currentLabel">The label text to wrap.</param>
        /// <param name="maximumWidth">The maximum width allowed.</param>
        /// <param name="font">The font options.</param>
        /// <param name="wrapAnyWhere">Indicates whether to wrap anywhere or at word boundaries.</param>
        /// <param name="isRtlEnable">Indicates whether RTL is enabled.</param>
        /// <returns>A list of wrapped text lines.</returns>
        internal static List<string> TextWrap(string currentLabel, double maximumWidth, ChartFontOptions font, bool wrapAnyWhere = false, bool isRtlEnable = false)
        {
            if (wrapAnyWhere)
            {
                return TextWrapAnyWhere(currentLabel, maximumWidth, font);
            }

            string label = string.Empty, text;
            List<string> labelCollection = new List<string>();
            string[] textCollection = currentLabel.Split(SPACE);

            for (int i = 0, len = textCollection.Length; i < len; i++)
            {
                text = textCollection[i];
                if (MeasureText(label + (string.IsNullOrEmpty(label) ? string.Empty : SPACE + text), font).Width < maximumWidth)
                {
                    label = label + (string.IsNullOrEmpty(label) ? string.Empty : SPACE) + text;
                }
                else
                {
                    if (!string.IsNullOrEmpty(label))
                    {
                        labelCollection.Add(TextTrim(maximumWidth, label, font, isRtlEnable));
                        label = text;
                    }
                    else
                    {
                        labelCollection.Add(TextTrim(maximumWidth, text, font, isRtlEnable));
                        text = string.Empty;
                    }
                }

                if (!string.IsNullOrEmpty(label) && i == len - 1)
                {
                    labelCollection.Add(TextTrim(maximumWidth, label, font, isRtlEnable));
                }
            }
            if (labelCollection.Count > 0 && labelCollection[0] == "...")
            {
                labelCollection.Clear();
                labelCollection.Add("...");
            }
            return labelCollection;
        }

        /// <summary>
        /// Calculates symbol shapes for chart markers and legends.
        /// </summary>
        /// <param name="location">The center location of the symbol.</param>
        /// <param name="size">The size of the symbol.</param>
        /// <param name="shape">The shape type.</param>
        /// <param name="url">The image URL (for Image shape).</param>
        /// <param name="option">The path options.</param>
        /// <param name="isBulletChart">Indicates whether it's a bullet chart.</param>
        /// <returns>The calculated symbol options.</returns>
        internal static SymbolOptions CalculateShapes(ChartEventLocation location, Size size, string shape, string url, PathOptions option, bool isBulletChart)
        {
            CultureInfo culture = CultureInfo.InvariantCulture;
            double width = isBulletChart && shape == "Circle" ? (size.Width - 2) : size.Width;
            double height = isBulletChart && shape == "Circle" ? (size.Height - 2) : size.Height;
            double locationX = location.X, locationY = location.Y;
            double y = location.Y + (-height / 2), x = location.X + (-width / 2);

            SymbolOptions symbolOption = new SymbolOptions { ShapeName = ShapeName.Path };

            switch (shape)
            {
                case "Bubble":
                case "Circle":
                    symbolOption.ShapeName = ShapeName.Ellipse;
                    symbolOption.EllipseOption = new EllipseOptions(option.Id, Convert.ToString(width / 2, culture), Convert.ToString(height / 2, culture), Convert.ToString(locationX, culture), Convert.ToString(locationY, culture), option.StrokeDashArray, option.StrokeWidth, option.Stroke, option.Opacity, option.Fill, option.DataPoint);
                    break;
                case "Plus":
                    option.Direction = BuildPlusDirection(culture, locationX, locationY, width, height, x, y);
                    break;
                case "Cross":
                    option.Direction = BuildCrossDirection(culture, locationX, locationY, width, height, x, y);
                    break;
                case "MultiplocationY":
                    option.Direction = BuildMultiplocationYDirection(culture, width, height, x, y);
                    option.Stroke = option.Fill;
                    break;
                case "HorizontalLine":
                    option.Direction = BuildHorizontalLineDirection(culture, locationX, locationY, width, x);
                    break;
                case "VerticalLine":
                    option.Direction = BuildVerticalLineDirection(culture, locationX, locationY, height);
                    break;
                case "Diamond":
                    option.Direction = BuildDiamondDirection(culture, locationX, locationY, width, height, x);
                    break;
                case "ActualRect":
                    option.Direction = BuildActualRectDirection(culture, locationX, locationY, height, width, x);
                    break;
                case "TargetRect":
                    option.Direction = BuildTargetRectDirection(culture, locationX, locationY, height, x);
                    break;
                case "Rectangle":
                case "StepArea":
                case "StackingStepArea":
                case "Square":
                case "Flag":
                case "RangeStepArea":
                    option.Direction = BuildRectangleDirection(culture, locationX, locationY, height, width, x);
                    break;
                case "Pyramid":
                case "Triangle":
                    option.Direction = BuildTriangleDirection(culture, locationX, locationY, height, width, x);
                    break;
                case "Funnel":
                case "InvertedTriangle":
                    option.Direction = BuildFunnelDirection(culture, locationX, locationY, height, width);
                    break;
                case "Pentagon":
                    option.Direction = BuildPentagonDirection(culture, locationX, locationY, height, width);
                    break;
                case "Image":
                    symbolOption.ShapeName = ShapeName.Image;
                    symbolOption.ImageOption = new ImageOptions(option.Id, x, y, width, height, url);
                    break;
            }
            option = CalculateLegendShapes(location, size, shape, option);
            symbolOption.PathOption = option;
            return symbolOption;
        }

        /// <summary>
        /// Converts a polar value to a coefficient.
        /// </summary>
        /// <param name="point">The point value.</param>
        /// <param name="axisRenderer">The axis renderer.</param>
        /// <returns>The calculated coefficient.</returns>
        internal static double ValueToPolarCoefficient(double point, ChartAxisRenderer axisRenderer)
        {
            DoubleRange range = axisRenderer.VisibleRange;
            List<VisibleLabels> visibleLables = axisRenderer.VisibleLabels;
            double delta, length;

            if (visibleLables.Count == 0)
            {
                delta = 1;
                length = 1;
            }
            else if (axisRenderer.Axis?.ValueType != ValueType.Category)
            {
                delta = range.End - ((axisRenderer.Axis?.ValueType == ValueType.DateTime) ? axisRenderer.DateTimeInterval : axisRenderer.VisibleInterval) - range.Start;
                length = visibleLables.Count - 1;
                delta = delta == 0 ? 1 : delta;
            }
            else
            {
                delta = visibleLables.Count == 1 ? 1 : (visibleLables[visibleLables.Count - 1].Value - visibleLables[0].Value);
                length = visibleLables.Count;
            }

            return axisRenderer.Axis is not null && axisRenderer.Axis.IsAxisInverse ? (point - range.Start) / delta * (1 - (1 / length)) : 1 - ((point - range.Start) / delta * (1 - (1 / length)));
        }

        /// <summary>
        /// Gets the minimum points delta for an axis across all series.
        /// </summary>
        /// <param name="axis">The chart axis.</param>
        /// <param name="seriesCollection">The collection of series renderers.</param>
        /// <returns>The minimum delta value.</returns>
        internal static double GetMinPointsDelta(ChartAxis axis, List<ChartSeriesRenderer> seriesCollection)
        {
            if (axis == null || seriesCollection == null || !seriesCollection.Any())
            {
                return 1; // Return a safe default
            }

            double minDelta = double.MaxValue, minVal;
            string axisName = axis.GetName();

            for (int index = 0; index < seriesCollection?.Count; index++)
            {
                ChartSeriesRenderer seriesRenderer = seriesCollection[index];
                ChartSeries series = seriesRenderer.Series ?? null!;
                List<double> xValues = new List<double>();

                if (series.Visible && (axisName == series.XAxisName || (axisName == Constants.PrimaryXAxis && series.XAxisName is null)))
                {
                    seriesRenderer.Points?.ForEach(x => xValues.Add(x.XValue));
                    xValues.Sort();
                    if (xValues.Count == 1)
                    {
                        double seriesMin = 0;
                        if (!double.IsNaN(axis.Renderer?.Min ?? 0) && !double.IsNaN(axis.Renderer?.Max ?? 0))
                        {
                            seriesMin = axis.Renderer?.Min ?? 0;
                        }
                        else
                        {
                            seriesMin = seriesRenderer.XMin;
                        }
                        minVal = xValues[0] - (!double.IsNaN(seriesMin) ? seriesMin : axis.Renderer?.VisibleRange.Start ?? 0);
                        if (minVal != 0)
                        {
                            minDelta = Math.Min(minDelta, minVal);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < xValues.Count; i++)
                        {
                            if (i > 0 & !double.IsNaN(xValues[i]))
                            {
                                minVal = xValues[i] - xValues[i - 1];
                                if (minVal != 0)
                                {
                                    minDelta = Math.Min(minDelta, minVal);
                                }
                            }
                        }
                    }
                }
            }

            if (minDelta == double.MaxValue)
            {
                minDelta = 1;
            }

            return minDelta;
        }

        /// <summary>
        /// Checks if a series needs a horizontal line gradient in the legend.
        /// </summary>
        /// <param name="series">The chart series.</param>
        /// <returns><c>true</c> if horizontal line gradient is needed; otherwise, <c>false</c>.</returns>
        internal static bool NeedsLegendHorizontalLineGradient(ChartSeries series)
        {
            return series is not null && (series.Type == ChartSeriesType.Line || series.Type == ChartSeriesType.StackingLine || series.Type == ChartSeriesType.StackingLine100);
        }

        /// <summary>
        /// Converts a DateTime to milliseconds since Unix epoch.
        /// </summary>
        /// <param name="current">The DateTime to convert.</param>
        /// <returns>The time in milliseconds.</returns>
        internal static double GetTime(DateTime current)
        {
            return (current - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }

        /// <summary>
        /// Converts milliseconds since Unix epoch to DateTime.
        /// </summary>
        /// <param name="date">The time in milliseconds.</param>
        /// <returns>The converted DateTime.</returns>
        internal static DateTime GetDate(double date)
        {
            return new DateTime(1970, 1, 1).AddMilliseconds(date);
        }

        /// <summary>
        /// Checks if a point is within specified bounds with optional padding.
        /// </summary>
        /// <param name="x">The X-coordinate.</param>
        /// <param name="y">The Y-coordinate.</param>
        /// <param name="bounds">The boundary rectangle.</param>
        /// <param name="width">Additional width padding (optional).</param>
        /// <param name="height">Additional height padding (optional).</param>
        /// <returns><c>true</c> if the point is within bounds; otherwise, <c>false</c>.</returns>
        internal static bool WithInBounds(double x, double y, Rect bounds, double width = 0, double height = 0)
        {
            return bounds is not null && x >= bounds.X - width && x <= bounds.X + bounds.Width + width && y >= bounds.Y - height && y <= bounds.Y + bounds.Height + height;
        }

        /// <summary>
        /// Checks if mouse coordinates are within an area's bounds.
        /// </summary>
        /// <param name="mouseX">The mouse X-coordinate.</param>
        /// <param name="mouseY">The mouse Y-coordinate.</param>
        /// <param name="axisRect">The axis rectangle.</param>
        /// <returns><c>true</c> if within area bounds; otherwise, <c>false</c>.</returns>
        internal static bool WithInAreaBounds(double mouseX, double mouseY, Rect axisRect)
        {
            return mouseX <= axisRect.X + axisRect.Width && axisRect.X <= mouseX && axisRect.Width != 0 || mouseY <= axisRect.Y + axisRect.Height && axisRect.Y <= mouseY && axisRect.Height != 0;
        }

        /// <summary>
        /// Checks if two rectangles overlap.
        /// </summary>
        /// <param name="currentRect">The first rectangle.</param>
        /// <param name="rect">The second rectangle.</param>
        /// <returns><c>true</c> if the rectangles overlap; otherwise, <c>false</c>.</returns>
        internal static bool IsOverlap(Rect currentRect, Rect rect)
        {
            return currentRect.X < rect.X + rect.Width && currentRect.X + currentRect.Width > rect.X && currentRect.Y < rect.Y + rect.Height && currentRect.Height + currentRect.Y > rect.Y;
        }

        /// <summary>
        /// Gets the coordinates of a rotated rectangle.
        /// </summary>
        /// <param name="actualPoints">The original corner points.</param>
        /// <param name="centerX">The X-coordinate of the rotation center.</param>
        /// <param name="centerY">The Y-coordinate of the rotation center.</param>
        /// <param name="angle">The rotation angle in degrees.</param>
        /// <returns>The list of rotated coordinates.</returns>
        internal static List<ChartEventLocation> GetRotatedRectangleCoordinates(List<ChartEventLocation> actualPoints, double centerX, double centerY, double angle)
        {
            List<ChartEventLocation> coordinatesAfterRotation = new List<ChartEventLocation>();
            for (int i = 0; i < 4; i++)
            {
                ChartEventLocation point = actualPoints[i];
                double tempX = point.X - centerX;
                double tempY = point.Y - centerY;
                point.X = (tempX * Math.Cos(DegreeToRadian(angle)) - tempY * Math.Sin(DegreeToRadian(angle))) + centerX;
                point.Y = (tempX * Math.Sin(DegreeToRadian(angle)) + tempY * Math.Cos(DegreeToRadian(angle))) + centerY;
                coordinatesAfterRotation.Add(new ChartEventLocation(point.X, point.Y));
            }

            return coordinatesAfterRotation;
        }

        /// <summary>
        /// Gets the RGB <see cref="Color"/> from a color string.
        /// </summary>
        /// <param name="color">The color string.</param>
        /// <returns>The resolved color.</returns>
        internal static Color GetRBGValue(string color)
        {
            Color rbgValue = new Color();
            char[] getChar;
            color = (string.IsNullOrEmpty(color) || color == Constants.Transparent) && string.IsNullOrEmpty(color) ? string.Empty : color;
            if (color.StartsWith("rgb", StringComparison.InvariantCulture) || color.StartsWith("rgba", StringComparison.InvariantCulture))
            {
               color = rgbaToHex(color);
            }

            if (color.Contains('#', StringComparison.InvariantCulture) && !color.Contains("url", StringComparison.InvariantCulture))
            {
                color = color.Replace("#", string.Empty, StringComparison.InvariantCulture);

                if (color.Length < RGB_HEX_CODE)
                {
                    getChar = color.ToCharArray();
                    color = string.Empty;
                    for (int i = 0; i < getChar.Length; i++)
                    {
                        color += getChar[i].ToString() + getChar[i].ToString();
                    }
                }

                rbgValue = Color.FromArgb(int.Parse(color.AsSpan(0, 2), NumberStyles.AllowHexSpecifier, null), int.Parse(color.AsSpan(2, 2), NumberStyles.AllowHexSpecifier, null), int.Parse(color.AsSpan(4, 2), NumberStyles.AllowHexSpecifier, null));           
            }
            else
            {
                color = (Color.FromName(color).A != 0) ? color : DEFAULT_COLOR;
                rbgValue = Color.FromName(color);
            }

            return rbgValue;
        }

        /// <summary>
        /// Lightens a color by a specified factor.
        /// </summary>
        /// <param name="color">The input color.</param>
        /// <param name="lightenFactor">The factor to lighten by.</param>
        /// <returns>The lightened color as hex.</returns>
        internal static string LightenColor(string color, double lightenFactor, bool isFluentDark = false)
        {
            if (string.IsNullOrEmpty(color))
            {
                return color;
            }

            Color initialColor = GetRBGValue(color);

            RgbToHsl(initialColor, out double h, out double s, out double l);

            l = Math.Max(0, Math.Min(1, l + lightenFactor));

            HslToRgb(h, s, l, out int newR, out int newG, out int newB);
            string finalRgbaString = $"rgba({newR},{newG},{newB},1)";
            return rgbaToHex(finalRgbaString);
        }

        /// <summary>
        /// Brightens a color by a specified factor.
        /// </summary>
        /// <param name="color">The input color.</param>
        /// <param name="brightenFactor">The factor to brighten by.</param>
        /// <returns>The brightened color as hex.</returns>
        internal static string BrightenColor(string color, double brightenFactor, bool isFluentDark = false)
        {
            if (string.IsNullOrEmpty(color))
            {
                return color;
            }

            brightenFactor = Math.Max(-1, Math.Min(1, brightenFactor));
            Color initialColor = GetRBGValue(color);

            int newR = (int)Math.Round(Math.Min(255, Math.Max(0, initialColor.R + brightenFactor * initialColor.R)));
            int newG = (int)Math.Round(Math.Min(255, Math.Max(0, initialColor.G + brightenFactor * initialColor.G)));
            int newB = (int)Math.Round(Math.Min(255, Math.Max(0, initialColor.B + brightenFactor * initialColor.B)));

            string finalRgbaString = $"rgba({newR},{newG},{newB},1)";
            return rgbaToHex(finalRgbaString);
        }

        /// <summary>
        /// Renders a text element with optional wrap and chart label behavior.
        /// </summary>
        /// <param name="builder">The render tree builder.</param>
        /// <param name="svgRenderer">The SVG renderer.</param>
        /// <param name="option">The text options.</param>
        /// <param name="textWidth">The text width.</param>
        /// <param name="isDatalabelWrap">Whether the label should wrap.</param>
        /// <param name="isChartDatalabel">Whether the text is a chart data label.</param>
        internal static void TextElement(RenderTreeBuilder builder, SvgRendering svgRenderer, TextOptions option, double textWidth = 0, bool isDatalabelWrap = false, bool isChartDatalabel = false)
        {
            int count = option.TextCollection.Count;
            double width = 0;
            option.TextLocationCollection = new List<TextLocation>();
            option.Text = count == 0 ? option.Text : option.IsMinus ? option.TextCollection[count - 1] : option.TextCollection[0];

            if (count > 0)
            {
                for (int i = 1; i < count; i++)
                {
                    double height = MeasureText(option.TextCollection[i], (ChartFontOptions)option.Font).Height;
                    width = Math.Max(width, MeasureText(option.TextCollection[i], (ChartFontOptions)option.Font).Width);
                    option.TextLocationCollection.Add(new TextLocation(option.IsMinus ? option.TextCollection[count - (i + 1)] : option.TextCollection[i], Convert.ToDouble(option.Y, CultureInfo.InvariantCulture) + (option.IsMinus ? -(i * height) : (i * height))));
                }
            }
            double X = (Convert.ToDouble(option.X, CultureInfo.InvariantCulture) + (textWidth / 2)) - (isChartDatalabel ? 0 : (width / 2));
            string optionX = isDatalabelWrap ? X.ToString(CultureInfo.InvariantCulture) : option.X;
            option.ChildContent = RenderTSpan(option.Id, optionX, option.TextLocationCollection, svgRenderer);

            if (!option.IsRotatedLabelIntersect && svgRenderer is not null)
            {
                svgRenderer.RenderText(builder, option);
            }
        }

        /// <summary>
        /// Calculates the size of rotated text.
        /// </summary>
        /// <param name="labelStyle">The label font options.</param>
        /// <param name="text">The text content.</param>
        /// <param name="angle">The rotation angle.</param>
        /// <returns>The rotated size.</returns>
        internal static Size RotateTextSize(ChartFontOptions labelStyle, string text, double angle)
        {
            Size size = MeasureText(text, labelStyle);
            double theta = angle * Math.PI / 180.0;
            while (theta < 0.0)
            {
                theta += 2 * Math.PI;
            }

            double adjacentTop, oppositeTop, adjacentBottom, oppositeBottom;

            if ((theta >= 0.0 && theta < Math.PI / 2.0) || (theta >= Math.PI && theta < (Math.PI + (Math.PI / 2.0))))
            {
                adjacentTop = Math.Abs(Math.Cos(theta)) * size.Width;
                oppositeTop = Math.Abs(Math.Sin(theta)) * size.Width;
                adjacentBottom = Math.Abs(Math.Cos(theta)) * size.Height;
                oppositeBottom = Math.Abs(Math.Sin(theta)) * size.Height;
            }
            else
            {
                adjacentTop = Math.Abs(Math.Sin(theta)) * size.Height;
                oppositeTop = Math.Abs(Math.Cos(theta)) * size.Height;
                adjacentBottom = Math.Abs(Math.Sin(theta)) * size.Width;
                oppositeBottom = Math.Abs(Math.Cos(theta)) * size.Width;
            }

            return new Size((int)Math.Ceiling(adjacentTop + oppositeBottom), (int)Math.Ceiling(adjacentBottom + oppositeTop));
        }

        /// <summary>
        /// Gets the transformed plotting rectangle for inverted or standard axes.
        /// </summary>
        /// <param name="xAxisRect">The X-axis rectangle.</param>
        /// <param name="yAxisRect">The Y-axis rectangle.</param>
        /// <param name="invertedAxis">Whether axes are inverted.</param>
        /// <returns>The transformed rectangle.</returns>
        internal static Rect GetTransform(Rect xAxisRect, Rect yAxisRect, bool invertedAxis)
        {
            return invertedAxis
                ? new Rect(yAxisRect.X, xAxisRect.Y, yAxisRect.Width, xAxisRect.Height)
                : new Rect(xAxisRect.X, yAxisRect.Y, xAxisRect.Width, yAxisRect.Height);
        }

        /// <summary>
        /// Gets scrollbar theme colors for a chart theme.
        /// </summary>
        /// <param name="theme">The chart theme.</param>
        /// <returns>The scrollbar theme style.</returns>
        internal static ScrollbarThemeStyle GetScrollbarThemeColor(string theme)
        {
            if(theme != "FluentDark")
            {
                return GetScrollbarStyle("#F5F5F5", "#F0F0F0", "#FAFAFA", "#FAFAFA", "#424242", "#424242");
            }
            else
            {
                return GetScrollbarStyle("#0A0A0A", "#141414", "#1F1F1F", "#1F1F1F", "#D6D6D6", "#D6D6D6");
            }
        }

        /// <summary>
        /// Converts a CSS size string into a numeric value.
        /// </summary>
        /// <param name="size">The size string.</param>
        /// <param name="containerSize">The container size.</param>
        /// <returns>The numeric value or <see cref="double.NaN"/>.</returns>
        internal static double StringToNumber(string size, double containerSize)
        {
            if (!string.IsNullOrEmpty(size) && size != "auto")
            {
               return size.Contains('%', StringComparison.InvariantCulture)
                    ? (containerSize / 100) * double.Parse(size.Replace("%", SPACE, StringComparison.InvariantCulture), null)
                    : double.Parse(size.ToLower(CultureInfo.CurrentCulture).Replace("px", string.Empty, StringComparison.InvariantCulture), provider: CultureInfo.InvariantCulture);
            }

            return double.NaN;
        }

        /// <summary>
        /// Trims text to fit the specified maximum width.
        /// </summary>
        /// <param name="maxWidth">The maximum width.</param>
        /// <param name="text">The text to trim.</param>
        /// <param name="font">The font options.</param>
        /// <param name="isRtlEnable">Whether RTL is enabled.</param>
        /// <returns>The trimmed text.</returns>
        internal static string TextTrim(double maxWidth, string text, ChartFontOptions font, bool isRtlEnable = false)
        {
            string label = text;
            double size = MeasureText(text, font).Width;

            if (Math.Round(size) > Math.Round(maxWidth))
            {
                for (int i = text.Length - 1; i >= 0; --i)
                {
                    label = isRtlEnable ? string.Concat("...", text.AsSpan(0, i)) : string.Concat(text.AsSpan(0, i), "...");
                    size = MeasureText(label, font).Width;
                    if (size <= maxWidth)
                    {
                        return label;
                    }
                }
            }

            return label;
        }

        /// <summary>
        /// Gets the X-position for a title based on alignment.
        /// </summary>
        /// <param name="rect">The bounding rectangle.</param>
        /// <param name="textAlignment">The alignment.</param>
        /// <returns>The X position.</returns>
        internal static double TitlePositionX(Rect rect, Alignment textAlignment)
        {
            if (textAlignment == Alignment.Near)
            {
                return rect.X;
            }
            else if (textAlignment == Alignment.Center)
            {
                return rect.X + (rect.Width / 2);
            }
            else
            {
                return rect.X + rect.Width;
            }
        }

        /// <summary>
        /// Converts a string to unicode subscript or superscript text.
        /// </summary>
        /// <param name="text">The input text.</param>
        /// <param name="pattern">The regex pattern.</param>
        /// <param name="regExp">The regex instance.</param>
        /// <returns>The converted unicode string.</returns>
        internal static string GetUniCode(string text, string pattern, Regex regExp)
        {
            string title = Regex.Replace(text, pattern, SPACE), convertedText = SPACE;
            MatchCollection digit = regExp.Matches(text);

            Dictionary<char, string> UnicodeSub = new Dictionary<char, string>()
            {
                { '0', "\u2080" }, { '1', "\u2081" }, { '2', "\u2082" }, { '3', "\u2083" }, { '4',"\u2084"},
                { '5',"\u2085"}, { '6',"\u2086"}, {'7',"\u2087"}, {'8',"\u2088"}, {'9',"\u2089"}
            };
            Dictionary<char, string> UnicodeSup = new Dictionary<char, string>()
            {
                { '0', "\u2070" }, { '1', "\u00B9" }, { '2', "\u00B2" }, { '3', "\u00B3" }, { '4',"\u2074"},
                { '5',"\u2075"}, {'6',"\u2076"}, {'7',"\u2077"}, {'8',"\u2078"}, {'9',"\u2079"}
            };
            int matchIndex = 0;

            for (int i = 0; i <= title.Length - 1; i++)
            {
                if (title[i].Equals(SPACE))
                {
                    string DigitSpecific = (regExp == Constants.SubRegex()) ? Convert.ToString(digit[matchIndex], null)?.Replace("~", string.Empty, StringComparison.Ordinal) ?? string.Empty : Convert.ToString(digit[matchIndex], null)?.Replace("^", string.Empty, StringComparison.Ordinal) ?? string.Empty;
                    for (int j = 0; j < DigitSpecific.Length; j++)
                    {
                        convertedText += (regExp == Constants.SubRegex()) ? UnicodeSub[DigitSpecific[j]] : UnicodeSup[DigitSpecific[j]];
                    }

                    matchIndex++;
                }
                else
                {
                    convertedText += title[i];
                }
            }

            return convertedText.Trim();
        }

        /// <summary>
        /// Gets the desired intervals count for axis labels.
        /// </summary>
        /// <param name="availableSize">The available size.</param>
        /// <param name="desiredIntervals">The desired intervals.</param>
        /// <param name="orientation">The axis orientation.</param>
        /// <param name="maximumLabels">The maximum label count.</param>
        /// <returns>The actual desired intervals count.</returns>
        internal static double GetActualDesiredIntervalsCount(Size availableSize, double desiredIntervals, Orientation orientation, double maximumLabels)
        {
            if (double.IsNaN(desiredIntervals))
            {
                return Math.Max((orientation == Orientation.Horizontal ? availableSize.Width : availableSize.Height) * (((orientation == Orientation.Horizontal ? 0.533 : 1) * maximumLabels) / 100), 1);
            }

            return desiredIntervals;
        }

        /// <summary>
        /// Converts a degree value to a chart location.
        /// </summary>
        /// <param name="degree">The degree value.</param>
        /// <param name="radius">The radius.</param>
        /// <param name="center">The center location.</param>
        /// <returns>The computed location.</returns>
        internal static ChartEventLocation DegreeToLocation(double degree, double radius, ChartEventLocation center)
        {
            double radian = (degree * Math.PI) / 180;
            return new ChartEventLocation(Math.Cos(radian) * radius + center.X, Math.Sin(radian) * radius + center.Y);
        }

        /// <summary>
        /// Gets visible points with assigned indices.
        /// </summary>
        /// <param name="points">The points list.</param>
        /// <returns>The visible points list.</returns>
        internal static List<Point> GetVisiblePoints(List<Point> points)
        {
            List<Point> tempPoints = new List<Point>();
            int pointIndex = 0;
            for (int i = 0; i < points.Count; i++)
            {
                Point tempPoint = points[i];
                tempPoint.Index = pointIndex++;
                tempPoints.Add(tempPoint);
            }

            return tempPoints;
        }

        /// <summary>
        /// Formats a value using the numeric formatter.
        /// </summary>
        /// <param name="formatValue">The value to format.</param>
        /// <param name="isCustom">Whether a custom format is used.</param>
        /// <param name="format">The format string.</param>
        /// <returns>The formatted string.</returns>
        internal static string FormatValue(object formatValue, bool isCustom, string format)
        {
            if (formatValue.GetType().Equals(typeof(double[])))
            {
                return "NAN";
            }
            return Intl.GetNumericFormat(formatValue, isCustom ? string.Empty : format);
        }

        /// <summary>
        /// Gets the label text for a data point.
        /// </summary>
        /// <param name="currentPoint">The current point.</param>
        /// <param name="seriesRenderer">The series renderer.</param>
        /// <returns>The label text collection.</returns>
        internal static List<string> GetLabelText(Point currentPoint, ChartSeriesRenderer seriesRenderer)
        {
            string labelFormat = (seriesRenderer.Series?.Marker.DataLabel.Format is not null) ? (seriesRenderer.Series.Marker.DataLabel.Format) : (seriesRenderer.YAxisRenderer.Axis?.LabelFormat ?? string.Empty);
            bool customLabelFormat = labelFormat.Contains("{value}", StringComparison.InvariantCulture);
            List<string> text = seriesRenderer.GetLabelText(currentPoint, seriesRenderer);

            if (string.IsNullOrEmpty(currentPoint.Text))
            {
                for (int i = 0; i < text.Count; i++)
                {
                    string labelFormatValue = seriesRenderer.YAxisRenderer.Type == ValueType.Double && seriesRenderer.YAxisRenderer.Chart is not null && seriesRenderer.YAxisRenderer.Chart.UseGroupingSeparator && (labelFormat.Contains("{value}", StringComparison.InvariantCulture) || string.IsNullOrEmpty(labelFormat)) ? "#,##0.###" : labelFormat;
                    text[i] = customLabelFormat ? labelFormat.Replace("{value}", FormatValue(Convert.ToDouble(text[i], CultureInfo.InvariantCulture), customLabelFormat && !labelFormatValue.Contains("#,##0.###", StringComparison.InvariantCulture), labelFormatValue), StringComparison.InvariantCulture) : FormatValue(Convert.ToDouble(text[i], CultureInfo.InvariantCulture), customLabelFormat, labelFormatValue);
                }
            }
            return text;
        }

        /// <summary>
        /// Calculates the rectangle for a label based on location and margin.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="textSize">The text size.</param>
        /// <param name="margin">The margin.</param>
        /// <returns>The calculated rectangle.</returns>
        internal static Rect CalculateRect(ChartEventLocation location, Size textSize, ChartEventMargin margin)
        {
            return new Rect(location.X - (textSize.Width / 2) - margin.Left, location.Y - (textSize.Height / 2) - margin.Top, textSize.Width + margin.Left + margin.Right, textSize.Height + margin.Top + margin.Bottom);
        }

        /// <summary>
        /// Determines whether the rectangle collides with others within the clip rectangle.
        /// </summary>
        /// <param name="rect">The current rectangle.</param>
        /// <param name="collections">The rectangle collection.</param>
        /// <param name="clipRect">The clip rectangle.</param>
        /// <param name="isCartesianAxes">Whether cartesian axes are used.</param>
        /// <returns><c>true</c> if colliding; otherwise <c>false</c>.</returns>
        internal static bool IsCollide(Rect rect, List<Rect> collections, Rect clipRect)
        {
            Rect currentRect = new Rect(rect.X + clipRect.X, rect.Y + clipRect.Y, rect.Width, rect.Height);
            return (collections.Count != 0 && collections.ToArray().Any(rect => currentRect.X < (rect.X + rect.Width) && (currentRect.X + currentRect.Width) > rect.X &&
                     currentRect.Y < (rect.Y + rect.Height) && (currentRect.Height + currentRect.Y) > rect.Y));
        }

        /// <summary>
        /// Determines whether two rotated rectangles intersect.
        /// </summary>
        /// <param name="a">The first polygon.</param>
        /// <param name="b">The second polygon.</param>
        /// <returns><c>true</c> if intersecting; otherwise <c>false</c>.</returns>
        internal static bool IsRotatedRectIntersect(List<ChartEventLocation> a, List<ChartEventLocation> b)
        {
            List<List<ChartEventLocation>> polygons = new List<List<ChartEventLocation>>() { a, b };
            double minA, maxA, projected, minB, maxB;
            for (int i = 0; i < polygons.Count; i++)
            {
                List<ChartEventLocation> polygon = polygons[i];
                for (int k = 0; k < polygon.Count; k++)
                {
                    int i2 = (k + 1) % polygon.Count;
                    ChartEventLocation normal = new ChartEventLocation(polygon[i2].Y - polygon[k].Y, polygon[k].X - polygon[i2].X);
                    minA = maxA = 0;
                    for (int j = 0; j < a.Count; j++)
                    {
                        projected = (normal.X * a[j].X) + (normal.Y * a[j].Y);
                        if (projected < minA)
                        {
                            minA = projected;
                        }

                        if (projected > maxA)
                        {
                            maxA = projected;
                        }
                    }

                    minB = maxB = 0;
                    for (int j = 0; j < b.Count; j++)
                    {
                        projected = (normal.X * b[j].X) + (normal.Y * b[j].Y);
                        if (projected < minB)
                        {
                            minB = projected;
                        }

                        if (projected > maxB)
                        {
                            maxB = projected;
                        }
                    }

                    if (maxA < minB || maxB < minA)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Builds a CSS font style string.
        /// </summary>
        /// <param name="font">The font options.</param>
        /// <returns>The CSS style string.</returns>
        internal static string GetFontStyle(ChartDefaultFont font)
        {
            return "font-size:" + font.Size + "; font-style:" + font.FontStyle + "; font-weight:" + font.FontWeight + "; font-family:" + font.FontFamily + ";opacity:" + font.Opacity + "; color:" + font.Color + ";";
        }

        /// <summary>
        /// Creates a visible range model from a range and interval.
        /// </summary>
        /// <param name="doubleRange">The double range.</param>
        /// <param name="interval">The interval.</param>
        /// <returns>The visible range model.</returns>
        internal static VisibleRangeModel GetVisibleRangeModel(DoubleRange doubleRange, double interval)
        {
            return new VisibleRangeModel()
            {
                Min = doubleRange.Start,
                Max = doubleRange.End,
                Delta = doubleRange.Delta,
                Interval = interval
            };
        }

        /// <summary>
        /// The method is used to get the textAnchor of title / subTitle.
        /// </summary>
        /// <param name="textAlignment">Specifies the text alignment of title.</param>
        /// <param name="enableRTL">Specifies the chart render in RTL mode.</param>
        /// <returns>The SVG text-anchor value.</returns>
        internal static string GetTextAnchor(Alignment textAlignment, bool enableRTL)
        {
            switch (textAlignment)
            {
                case Alignment.Near:
                    return enableRTL ? "end" : "start";
                case Alignment.Far:
                    return enableRTL ? "start" : "end";
                default:
                    return "middle";
            }
        }

        /// <summary>
        /// Gets a string value from a dynamic object.
        /// </summary>
        /// <param name="dynamicObject">The dynamic object.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>The value as a string.</returns>
        internal static string GetDynamicStringValue(DynamicObject dynamicObject, string propertyName)
        {
            return !string.IsNullOrEmpty(propertyName)
                ? Convert.ToString(ReflectionExtension.GetValueFromDynamicObject(dynamicObject, propertyName), CultureInfo.InvariantCulture) ?? string.Empty
                : string.Empty;
        }

        /// <summary>
        /// Gets the CLR value from a JSON element.
        /// </summary>
        /// <param name="jsonElement">The JSON element.</param>
        /// <returns>The value as a number or string.</returns>
        internal static object GetObjectValue(JsonElement jsonElement)
        {
            return jsonElement.ValueKind == JsonValueKind.Number
                ? jsonElement.GetDouble()
                : jsonElement.GetString() ?? null!;
        }

        /// <summary>
        /// Gets distinct values for list elements that are collections.
        /// </summary>
        /// <param name="xValuesList">The list to process.</param>
        /// <returns>The list with distinct sublists.</returns>
        internal static List<object> GetDistinctList(List<object> xValuesList)
        {
            for (int xValuesLength = 0; xValuesLength < xValuesList.Count; xValuesLength++)
            {
                if (xValuesList[xValuesLength] is List<object> sublist)
                {
                    var distinctSublist = sublist.Distinct().ToList();
                    xValuesList[xValuesLength] = distinctSublist;
                }
            }
            return xValuesList;
        }

        /// <summary>
        /// Collects distinct characters for font measurement caching.
        /// </summary>
        /// <param name="text">The text to analyze.</param>
        /// <param name="font">The font options.</param>
        /// <param name="distinctKeys">The output list of distinct keys.</param>
        internal static void GetDistinctCharacter(string text, ChartFontOptions font, List<string> distinctKeys)
        {
            if (!string.IsNullOrEmpty(text))
            {
                string key;
                if (IsRTLText(text))
                {
                    key = text + Constants.Underscore + font.FontWeight + Constants.Underscore + font.FontStyle + Constants.Underscore + font.FontFamily;
                    if (!SizePerCharacter.ContainsKey(key) && !ChartFontKeys.Contains(key))
                    {
                        distinctKeys.Add(key);
                        ChartFontKeys.Add(key);
                    }
                    else
                    {
                        foreach (char character in text)
                        {
                            key = character + Constants.Underscore + font.FontWeight + Constants.Underscore + font.FontStyle + Constants.Underscore + font.FontFamily;
                            if (!SizePerCharacter.ContainsKey(key) && !ChartFontKeys.Contains(key))
                            {
                                distinctKeys.Add(key);
                                ChartFontKeys.Add(key);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Clears static font measurement caches.
        /// </summary>
        internal static void ClearStaticStorage()
        {
            SizePerCharacter?.Clear();
            ChartFontKeys?.Clear();
        }

        #endregion
    }
}
