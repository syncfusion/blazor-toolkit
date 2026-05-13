using Microsoft.AspNetCore.Components.Rendering;
using System.Drawing;
using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Represents options used when rendering a data label template for a chart point.
    /// </summary>
    /// <remarks>
    /// This class encapsulates the configuration required to render a templated data label,
    /// including its unique identifier, inline styles, and the render fragment to be displayed.
    /// </remarks>
    internal class DatalabelTemplateOptions
    {
        /// <summary>
        /// Gets or sets the element id for the template insertion point.
        /// </summary>
        /// <value>The id assigned to the templated element.</value>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the inline style applied to the template container.
        /// </summary>
        /// <value>CSS style string.</value>
        public string Style { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the render fragment that represents the template.
        /// </summary>
        /// <value>A RenderFragment that will be rendered for the data label template.</value>
        public RenderFragment Template { get; set; } = null!;

    }

    /// <summary>
    /// Handles data label measurement, layout and rendering for a ChartSeries.
    /// </summary>
    public class ChartDataLabelRenderer : ChartRenderer, IChartElementRenderer
    {
        #region Constants
        private const int MAX_LABEL_POSITION_ATTEMPTS = 4;
        private const int RANGE_SERIES_POSITION_LIMIT = 2;
        private const double LABEL_PADDING = 5.0;
        #endregion

        #region Fields
        private string? _commonId;
        private string? _chartBackground;
        private string? _fontBackground;
        private bool _inverted;
        private bool _yAxisInversed;
        private bool _isShape;
        private bool _isRotationEnabled;
        private double _borderWidth;
        private double _markerHeight;
        private double _errorHeight;
        private double _locationX;
        private double _locationY;
        private double _labelAngle;
        private List<Size> _prevPointSize = [];
        private ChartEventMargin _margin = new();
        private CultureInfo _culture = CultureInfo.InvariantCulture;
        private List<RectOptions> _rectOptions = [];
        private List<TextOptions> _textOptions = [];
        private List<Rect> _dataLabelActualRectOptions = [];
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the owning series for which data labels are rendered.
        /// </summary>
        [Parameter]
        public ChartSeries Series { get; set; } = null!;

        /// <summary>
        /// Reference to the series renderer responsible for the series layout.
        /// </summary>
        internal ChartSeriesRenderer? SeriesRenderer { get; set; }

        /// <summary>
        /// Collected options for templates that will be rendered outside SVG.
        /// </summary>
        internal List<DatalabelTemplateOptions> _templateOptions = [];
        #endregion

        #region Lifecycle methods

        /// <summary>
        /// Initializes the renderer and assigns itself to the series marker datalabel renderer.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            Series.Marker.DataLabel.Renderer = this;
        }
        #endregion

        #region Private methods

        /// <summary>
        /// Returns the series index or category string used in element identifiers.
        /// </summary>
        /// <returns>
        /// The series index as a string if available; otherwise, the category string;
        /// or <see cref="string.Empty"/> if neither is available.
        /// </returns>
        private string SeriesIndex()
        {
            return double.IsNaN(SeriesRenderer?.Index ?? 0)
                ? Convert.ToString(SeriesRenderer?.Category(), null) ?? string.Empty
                : Convert.ToString(SeriesRenderer?.Index, null) ?? string.Empty;
        }

        /// <summary>
        /// Initializes member variables based on series and marker configuration.
        /// </summary>
        /// <param name="series">The chart series being labeled. Must not be <see langword="null"/>.</param>
        /// <param name="marker">The marker configuration. Must not be <see langword="null"/>.</param>
        private void InitPrivateVariables(ChartSeries series, ChartMarker marker)
        {
            _markerHeight = series.Type == ChartSeriesType.Scatter || (marker.Visible && Owner is not null && Owner._shouldRenderMarker) ? (marker.Height / 2) : 0;
            _commonId = Owner?.ID + "_Series_" + SeriesIndex() + "_Point_";

            bool isTransparent = Owner?._chartAreaRenderer?.Area?.Background == Constants.Transparent;
            _chartBackground = isTransparent ? Owner?.Background ?? Owner?._chartThemeStyle?.Background ?? string.Empty : Owner?._chartAreaRenderer?.Area?.Background;
            _textOptions.Clear();
            _rectOptions.Clear();
            _templateOptions.Clear();
        }

        /// <summary>
        /// Calculates the text label rectangle position given input parameters.
        /// </summary>
        /// <param name="point">The data point being labeled.</param>
        /// <param name="series">The series containing the point.</param>
        /// <param name="textSize">The measured size of the label text.</param>
        /// <param name="dataLabel">The data label configuration.</param>
        /// <param name="labelIndex">The index of this label (for multi-label points).</param>
        /// <param name="angle">The rotation angle in degrees.</param>
        /// <param name="isRotationEnabled">Whether rotation is enabled.</param>
        /// <returns>A <see cref="Rect"/> representing the final label position.</returns>
        private Rect CalculateTextPosition(Point point, ChartSeries series, Size textSize, ChartDataLabel dataLabel, int labelIndex, double angle, bool isRotationEnabled)
        {
            Rect labelRegion = point.Regions.Count > 0 ? (labelIndex > 1 ? point.Regions[1] : point.Regions[0]) : null!;
            ChartEventLocation location = GetLabelLocation(point, labelIndex);
            double alignmentValue;

            if ((Owner is not null && !Owner._requireInvertedAxis) || !IsRectSeries())
            {
                _locationX = location.X;
                alignmentValue = textSize.Height + (_borderWidth * 2) + _markerHeight + _margin.Bottom + _margin.Top + LABEL_PADDING;
                location.X = CalculateAlignment(alignmentValue, location.X, dataLabel.Alignment, point);
                location.Y = (!IsRectSeries())
                    ? CalculatePathPosition(location.Y, labelRegion, (point.YValue < 0) != _yAxisInversed, dataLabel.Position, series, point, textSize, labelIndex)
                    : CalculateRectPosition(location.Y, labelRegion, ((dataLabel.Template is not null && point.YValue <= 0) || point.YValue < 0) != _yAxisInversed, dataLabel.Position, series, textSize, labelIndex, point);
            }
            else
            {
                _locationY = location.Y;
                bool isNegativeValue = (point.YValue < 0) != _yAxisInversed;
                alignmentValue = textSize.Width + _borderWidth + _margin.Left + _margin.Right - LABEL_PADDING;
                location.X = CalculateAlignment(alignmentValue, location.X, dataLabel.Alignment, point);
                location.X = CalculateRectPosition(location.X, labelRegion, isNegativeValue, dataLabel.Position, series, textSize, labelIndex, point);
            }

            Rect rect = ChartHelper.CalculateRect(location, textSize, _margin);

            return GetLabelRect(rect, angle, isRotationEnabled, true);
        }

        /// <summary>
        /// Adjusts a label rectangle to ensure it fits within the chart area, accounting for rotation.
        /// </summary>
        /// <param name="rect">The original label rectangle.</param>
        /// <param name="angle">The rotation angle in degrees.</param>
        /// <param name="isRotationEnabled">Whether rotation is applied.</param>
        /// <param name="isBackground">If <see langword="true"/>, updates font background color for overflow cases.</param>
        /// <returns>The adjusted rectangle, or the original if no adjustment needed.</returns>
        private Rect GetLabelRect(Rect rect, double angle, bool isRotationEnabled, bool isBackground = false)
        {
            Rect clipRect = SeriesRenderer?.ClipRect ?? null!;
            bool isOverflowingClipArea = (rect.Y > (clipRect.Y + clipRect.Height))
                || (rect.X > (clipRect.X + clipRect.Width))
                || (rect.X + rect.Width + (rect.X < 0 ? 5 : 0) < 0)
                || (rect.Y + rect.Height + (rect.Y < 0 ? 5 : 0) < 0);

            if (!(isRotationEnabled && angle != 0) && !isOverflowingClipArea)
            {
                rect.X = rect.X < 0 ? 5 : rect.X;
                rect.Y = rect.Y < 0 ? 5 : rect.Y;
                rect.X -= (rect.X + rect.Width) > (clipRect.X + clipRect.Width) ? rect.X + rect.Width - (clipRect.X + clipRect.Width) + 5 : 0;
                rect.Y -= (rect.Y + rect.Height) > (clipRect.Y + clipRect.Height) ? rect.Y + rect.Height - (clipRect.Y + clipRect.Height) + 5 : 0;
                if (isBackground)
                {
                    _fontBackground = _fontBackground == Constants.Transparent ? _chartBackground : _fontBackground;
                }
            }
            return rect;
        }

        /// <summary>
        /// Gets the initial location for a data label based on the point's symbol location or region.
        /// </summary>
        /// <param name="point">The data point. Must not be <see langword="null"/>.</param>
        /// <param name="labelIndex">The zero-based index of the label.</param>
        /// <returns>A <see cref="ChartEventLocation"/> representing the initial label position.</returns>
        private ChartEventLocation GetLabelLocation(Point point, int labelIndex)
        {
            ChartEventLocation location = new(0, 0);
            Rect labelRegion = point.Regions.Count > 0 ? (labelIndex > 1 ? point.Regions[1] : point.Regions[0]) : null!;
            bool isInverted = Owner is not null && Owner._requireInvertedAxis;

            if (labelIndex is 0 or 1)
            {
                location = new ChartEventLocation(point.SymbolLocations[0].X, point.SymbolLocations[0].Y);
            }
            else if (isInverted)
            {
                location.X = labelRegion.X + (labelRegion.Width / 2);
                location.Y = labelRegion.Y;
            }
            else
            {
                location.X = labelRegion.X + labelRegion.Width;
                location.Y = labelRegion.Y + (labelRegion.Height / 2);
            }

            return location;
        }

        /// <summary>
        /// Adjusts a label's position based on alignment preference, ensuring it stays within point boundaries.
        /// </summary>
        /// <param name="alignValue">The alignment distance to apply.</param>
        /// <param name="labelLocation">The current label location.</param>
        /// <param name="alignment">The alignment mode (Near, Center, Far).</param>
        /// <param name="point">The data point. Must not be <see langword="null"/>.</param>
        /// <returns>The adjusted label location.</returns>
        private double CalculateAlignment(double alignValue, double labelLocation, Alignment alignment, Point point)
        {
            switch (alignment)
            {
                case Alignment.Far:
                    labelLocation = !_inverted
                        ? labelLocation - alignValue < point.Regions[0].X ? point.Regions[0].X
                        : labelLocation - alignValue : labelLocation + alignValue; break;
                case Alignment.Near:
                    labelLocation = !_inverted
                        ? labelLocation + alignValue > point.Regions[0].X + point.Regions[0].Width
                        ? point.Regions[0].X + point.Regions[0].Width : labelLocation + alignValue
                        : labelLocation - alignValue; break;
                case Alignment.Center:
                    return labelLocation;
                default:
                    break;
            }

            return labelLocation;
        }

        /// <summary>
        /// Calculates label position for path series (line, area, etc.) to minimize overlaps.
        /// </summary>
        /// <param name="labelLocation">The initial label Y-location.</param>
        /// <param name="rect">The label region rectangle.</param>
        /// <param name="isNegative">Whether the point value is negative.</param>
        /// <param name="position">The desired label position.</param>
        /// <param name="series">The series.</param>
        /// <param name="point">The data point.</param>
        /// <param name="size">The label text size.</param>
        /// <param name="labelIndex">The label index.</param>
        /// <returns>The adjusted Y-location for the label.</returns>
        private double CalculatePathPosition(double labelLocation, Rect rect, bool isNegative, ChartLabelPosition position, ChartSeries series, Point point, Size size, int labelIndex)
        {
            bool isAreaSeries = series.SeriesType is not null && series.SeriesType.Contains("Area", StringComparison.InvariantCulture);
            if (isAreaSeries && _yAxisInversed && series.Marker.DataLabel.Position != ChartLabelPosition.Auto)
            {
                position = position == ChartLabelPosition.Top ? ChartLabelPosition.Bottom : position == ChartLabelPosition.Bottom ? ChartLabelPosition.Top : position;
            }

            switch (position)
            {
                case ChartLabelPosition.Top:
                case ChartLabelPosition.Outer:
                    labelLocation = labelLocation - _markerHeight - _borderWidth - (size.Height / 2) - _margin.Bottom - 5 - _errorHeight;
                    break;
                case ChartLabelPosition.Bottom:
                    labelLocation = labelLocation + _markerHeight + _borderWidth + (size.Height / 2) + _margin.Top + 5 + _errorHeight;
                    break;
                case ChartLabelPosition.Auto:
                    labelLocation = CalculatePathActualPosition(
                        labelLocation, rect, isNegative, series, point, size, labelIndex);
                    break;
                case ChartLabelPosition.Middle:
                    break;
                default:
                    break;
            }
            UpdateFontBackgroundForPathSeries(labelLocation, rect, isNegative, isAreaSeries, series, point);
            return labelLocation;
        }

        /// <summary>
        /// Calculates the optimal Y-location for a path series label in <see cref="ChartLabelPosition.Auto"/> mode,
        /// cycling through available positions until a non-overlapping placement is found.
        /// </summary>
        /// <param name="y">The initial Y-location of the label.</param>
        /// <param name="rect">The label region rectangle used for bounds checking.</param>
        /// <param name="isminus">Whether the data point value is negative.</param>
        /// <param name="series">The chart series containing the label.</param>
        /// <param name="point">The data point being labeled.</param>
        /// <param name="size">The measured size of the label text.</param>
        /// <param name="labelIndex">The zero-based index of the label for multi-label points.</param>
        /// <returns>The adjusted Y-location that minimizes overlap with other labels.</returns>
        private double CalculatePathActualPosition(double y, Rect rect, bool isminus, ChartSeries series, Point point, Size size, int labelIndex)
        {
            int index = point.Index;
            double yValue = SeriesRenderer?.Points?[index].YValue ?? 0;
            double y_Location = 0;
            bool isOverLap = true;
            List<Rect> collection = Owner?._seriesContainer?._dataLabelCollection ?? null!;

            ChartLabelPosition position = DetermineBestPathLabelPosition(series, index, yValue);

            bool isBottom = position == ChartLabelPosition.Bottom;
            ChartLabelPosition[] Test = [ChartLabelPosition.Outer, ChartLabelPosition.Top, ChartLabelPosition.Bottom, ChartLabelPosition.Middle, ChartLabelPosition.Auto];
            int positionIndex = Array.IndexOf(Test, position);

            while (isOverLap && positionIndex < 4)
            {
                y_Location = CalculatePathPosition(y, rect, isminus, GetPosition(positionIndex), series, point, size, labelIndex);
                Rect labelRect = ChartHelper.CalculateRect(new ChartEventLocation(_locationX, y_Location), size, _margin);

                isOverLap = labelRect.Y < 0 || ChartHelper.IsCollide(labelRect, collection, SeriesRenderer?.ClipRect ?? null!, Owner?._chartAreaType == ChartAreaType.CartesianAxes) || (labelRect.Y + labelRect.Height) > SeriesRenderer?.ClipRect?.Height;
                positionIndex = isBottom ? positionIndex - 1 : positionIndex + 1;
                isBottom = false;
            }

            return y_Location;
        }

        /// <summary>
        /// Determines the best initial <see cref="ChartLabelPosition"/> for a path series label
        /// based on the series type and the relationship between neighboring point values.
        /// </summary>
        /// <param name="series">The chart series used to determine series type and draw behavior.</param>
        /// <param name="index">The zero-based index of the current data point.</param>
        /// <param name="yValue">The Y value of the current data point.</param>
        /// <returns>
        /// A <see cref="ChartLabelPosition"/> of <c>Top</c> or <c>Bottom</c> based on neighboring
        /// point values, series type, and axis inversion state.
        /// </returns>
        private ChartLabelPosition DetermineBestPathLabelPosition(ChartSeries series, int index, double yValue)
        {
            ChartLabelPosition position;
            Point nextPoint = SeriesRenderer?.Points?.Count - 1 > index ? SeriesRenderer?.Points?[index + 1] ?? null! : null!;
            Point previousPoint = index > 0 ? SeriesRenderer?.Points?[index - 1] ?? null! : null!;

            if (series.Type == ChartSeriesType.Bubble)
            {
                position = ChartLabelPosition.Top;
            }
            else if (series.SeriesType is not null && series.SeriesType.Contains("Step", StringComparison.InvariantCulture))
            {
                position = ChartLabelPosition.Top;
                if (index != 0)
                {
                    position = (previousPoint is null || !previousPoint.Visible || ((yValue > previousPoint.YValue) != _yAxisInversed) || yValue == previousPoint.YValue) ? ChartLabelPosition.Top : ChartLabelPosition.Bottom;
                }
            }
            else
            {
                if (index == 0)
                {
                    position = (nextPoint is null || !nextPoint.Visible || yValue > nextPoint.YValue || (yValue < nextPoint.YValue && _yAxisInversed)) ? ChartLabelPosition.Top : ChartLabelPosition.Bottom;
                }
                else if (index == SeriesRenderer?.Points?.Count - 1)
                {
                    position = (previousPoint is null || !previousPoint.Visible || yValue > previousPoint.YValue || (yValue < previousPoint.YValue && _yAxisInversed)) ? ChartLabelPosition.Top : ChartLabelPosition.Bottom;
                }
                else
                {
                    if (!nextPoint.Visible && !(previousPoint is not null && previousPoint.Visible))
                    {
                        position = ChartLabelPosition.Top;
                    }
                    else if (!nextPoint.Visible || previousPoint is null)
                    {
                        position = nextPoint.YValue > yValue || (previousPoint is not null && previousPoint.YValue > yValue) ? ChartLabelPosition.Bottom : ChartLabelPosition.Top;
                    }
                    else
                    {
                        double slope = (nextPoint.YValue - previousPoint.YValue) / 2;
                        double intersectY = (slope * index) + (nextPoint.YValue - (slope * (index + 1)));
                        position = !_yAxisInversed ? intersectY < yValue ? ChartLabelPosition.Top : ChartLabelPosition.Bottom : intersectY < yValue ? ChartLabelPosition.Bottom : ChartLabelPosition.Top;
                    }
                }
            }
            return position;
        }

        /// <summary>
        /// Updates the <c>_fontBackground</c> field for a path or area series label based on
        /// whether the label location falls within the series region.
        /// </summary>
        /// <param name="labelLocation">The calculated label position coordinate.</param>
        /// <param name="rect">The region rectangle of the data point.</param>
        /// <param name="isNegative">Whether the data point value is negative.</param>
        /// <param name="isAreaSeries">Whether the series is an area type.</param>
        /// <param name="series">The chart series containing the data point.</param>
        /// <param name="point">The data point whose interior color may be used as the background.</param>
        private void UpdateFontBackgroundForPathSeries(double labelLocation, Rect rect, bool isNegative, bool isAreaSeries, ChartSeries series, Point point)
        {
            bool withInRegion = !_inverted
                ? isNegative ? (labelLocation < rect.Y) : (labelLocation > rect.Y)
                : isNegative ? (labelLocation > rect.X) : (labelLocation < rect.X);

            _fontBackground = withInRegion && (isAreaSeries || series.Type == ChartSeriesType.Bubble)
                ? _fontBackground == Constants.Transparent
                    ? (!string.IsNullOrEmpty(point?.Interior) ? point.Interior : SeriesRenderer?.Interior)
                    : _fontBackground
                : _fontBackground == Constants.Transparent ? _chartBackground : _fontBackground;
        }

        /// <summary>
        /// Calculates label position for rectangular series (column, bar, etc.).
        /// </summary>
        /// <param name="labelLocation">The initial label location.</param>
        /// <param name="rect">The label region rectangle.</param>
        /// <param name="isNegative">Whether the value is negative.</param>
        /// <param name="position">The desired label position.</param>
        /// <param name="series">The series.</param>
        /// <param name="textSize">The label text size.</param>
        /// <param name="labelIndex">The label index.</param>
        /// <param name="point">The data point.</param>
        /// <returns>The adjusted label location.</returns>
        private double CalculateRectPosition(double labelLocation, Rect rect, bool isNegative, ChartLabelPosition position, ChartSeries series, Size textSize, int labelIndex, Point point)
        {
            double extraSpace = _borderWidth + ((!_inverted ? textSize.Height : textSize.Width) / 2) + 5;
            position = AdjustPositionForSeriesType(position, series);

            switch (position)
            {
                case ChartLabelPosition.Bottom:
                    labelLocation = !_inverted ? isNegative ? (labelLocation - rect.Height + extraSpace + _margin.Top) : (labelLocation + rect.Height - extraSpace - _margin.Bottom) : isNegative ? (labelLocation + rect.Width - extraSpace - _margin.Left) : (labelLocation - rect.Width + extraSpace + _margin.Right);
                    break;
                case ChartLabelPosition.Middle:
                    labelLocation = !_inverted ? isNegative ? labelLocation - (rect.Height / 2) : labelLocation + (rect.Height / 2) : isNegative ? labelLocation + (rect.Width / 2) : labelLocation - (rect.Width / 2);
                    break;
                case ChartLabelPosition.Auto:
                    labelLocation = CalculateRectActualPosition(labelLocation, rect, isNegative, series, textSize, labelIndex, point, extraSpace);
                    break;
                default:
                    extraSpace += _errorHeight;
                    labelLocation = CalculateTopAndOuterPosition(labelLocation, position, series, extraSpace, isNegative);
                    break;
            }

            _fontBackground = (!_inverted ? (labelLocation < rect.Y || labelLocation > rect.Y + rect.Height) : (labelLocation < rect.X || labelLocation > rect.X + rect.Width)) ? (_fontBackground == Constants.Transparent ? _chartBackground : _fontBackground) :
                _fontBackground == Constants.Transparent ? (!string.IsNullOrEmpty(point.Interior) ? point.Interior : SeriesRenderer?.Interior) : _fontBackground;
            return labelLocation;
        }

        /// <summary>
        /// Adjusts the <see cref="ChartLabelPosition"/> for stacking and range series to ensure
        /// a valid position is applied before computing the final label location.
        /// </summary>
        /// <param name="position">The originally requested label position.</param>
        /// <param name="series">The chart series used to identify its draw type.</param>
        /// <returns>
        /// The corrected <see cref="ChartLabelPosition"/>; <c>Outer</c> becomes <c>Top</c> for stacking series,
        /// and unsupported positions fall back to <c>Auto</c> for range series.
        /// </returns>
        private static ChartLabelPosition AdjustPositionForSeriesType(ChartLabelPosition position, ChartSeries series)
        {
            if (series.SeriesType is not null && series.SeriesType.Contains("Stacking", StringComparison.InvariantCulture))
            {
                position = position == ChartLabelPosition.Outer ? ChartLabelPosition.Top : position;
            }
            else if (series.SeriesType is not null && series.SeriesType.Contains("Range", StringComparison.InvariantCulture))
            {
                position = (position is ChartLabelPosition.Outer or ChartLabelPosition.Top) ? position : ChartLabelPosition.Auto;
            }
            return position;
        }

        /// <summary>
        /// Calculates the optimal label location for a rectangular series in <see cref="ChartLabelPosition.Auto"/> mode,
        /// cycling through candidate positions until a non-overlapping placement is found.
        /// </summary>
        /// <param name="labelLocation">The initial label coordinate to adjust.</param>
        /// <param name="rect">The region rectangle of the data point.</param>
        /// <param name="isNegative">Whether the data point value is negative.</param>
        /// <param name="series">The chart series containing the label.</param>
        /// <param name="size">The measured size of the label text.</param>
        /// <param name="labelIndex">The zero-based label index for multi-label points.</param>
        /// <param name="point">The data point being labeled.</param>
        /// <param name="extraSpace">The extra spacing buffer used for overlap detection.</param>
        /// <returns>The adjusted label coordinate that minimizes overlap with existing labels.</returns>
        private double CalculateRectActualPosition(double labelLocation, Rect rect, bool isNegative, ChartSeries series, Size size, int labelIndex, Point point, double extraSpace)
        {
            double location = 0;
            bool isOverLap = true;
            int position = 0;
            List<Rect> collection = Owner?._seriesContainer?._dataLabelCollection ?? null!;
            int maxPosition = series.SeriesType is not null && series.SeriesType.Contains("Range", StringComparison.InvariantCulture) ? RANGE_SERIES_POSITION_LIMIT : MAX_LABEL_POSITION_ATTEMPTS;

            while (isOverLap && position < maxPosition)
            {
                ChartLabelPosition actualPosition = GetPosition(position);
                location = CalculateRectPosition(labelLocation, rect, isNegative, actualPosition, series, size, labelIndex, point);

                Rect labelRect = !_inverted
                    ? ChartHelper.CalculateRect(new ChartEventLocation(_locationX, location), size, _margin)
                    : ChartHelper.CalculateRect(new ChartEventLocation(location, _locationY), size, _margin);

                isOverLap = CheckRectLabelOverlap(labelRect, collection, extraSpace, point, series, actualPosition, size);

                position++;
            }

            return location;
        }

        /// <summary>
        /// Determines whether a rectangular series label rectangle overlaps with existing labels
        /// or falls outside the clip region boundaries.
        /// </summary>
        /// <param name="labelRect">The candidate label rectangle to evaluate.</param>
        /// <param name="dataLabelCollection">The collection of already-placed label rectangles.</param>
        /// <param name="extraSpace">The buffer space used for boundary checks.</param>
        /// <param name="point">The data point being labeled, used to access region dimensions.</param>
        /// <param name="series">The chart series used to access the clip rectangle.</param>
        /// <param name="position">The candidate label position being evaluated.</param>
        /// <param name="size">The measured size of the label text.</param>
        /// <returns>
        /// <c>true</c> if the label overlaps with existing labels or exceeds clip boundaries;
        /// otherwise, <c>false</c>.
        /// </returns>
        private bool CheckRectLabelOverlap(Rect labelRect, List<Rect> dataLabelCollection, double extraSpace, Point? point, ChartSeries series, ChartLabelPosition position, Size size)
        {
            bool baseOverlap;

            if (!_inverted)
            {
                baseOverlap = labelRect.Y < 0 || (!_isRotationEnabled && _labelAngle == 0 && ChartHelper.IsCollide(labelRect, dataLabelCollection, SeriesRenderer?.ClipRect ?? null!, Owner?._chartAreaType == ChartAreaType.CartesianAxes)) || labelRect.Y + labelRect.Height > SeriesRenderer?.ClipRect?.Height;
                if (Series.Marker.DataLabel.Template is null && !baseOverlap && position != ChartLabelPosition.Outer)
                {
                    baseOverlap = ((labelRect.Y / 2) + size.Height + point?.Regions[0].Height - (2 * extraSpace)) > series.Renderer.ClipRect?.Height;
                }
            }
            else
            {
                baseOverlap = labelRect.X < 0 || (!_isRotationEnabled && _labelAngle == 0 && ChartHelper.IsCollide(labelRect, dataLabelCollection, SeriesRenderer?.ClipRect ?? null!, Owner?._chartAreaType == ChartAreaType.CartesianAxes)) || labelRect.X + labelRect.Width > SeriesRenderer?.ClipRect?.Width;
            }
            return baseOverlap;
        }

        /// <summary>
        /// Computes the top or outer label coordinate for a rectangular series by applying spacing
        /// offsets in the appropriate direction based on inversion, negativity, and stacking state.
        /// </summary>
        /// <param name="location">The initial label coordinate before offset is applied.</param>
        /// <param name="position">The desired label position (<c>Top</c> or <c>Outer</c>).</param>
        /// <param name="series">The chart series used to identify stacking behavior.</param>
        /// <param name="extraSpace">The total spacing offset including border width and marker height.</param>
        /// <param name="isNegative">Whether the data point value is negative.</param>
        /// <returns>The adjusted label coordinate with the appropriate spacing offset applied.</returns>
        private double CalculateTopAndOuterPosition(double location, ChartLabelPosition position, ChartSeries series, double extraSpace, bool isNegative)
        {
            bool shouldApplyTopPosition = (isNegative && position == ChartLabelPosition.Top && SeriesRenderer is not null && !SeriesRenderer.IsStackingColumnAndOuterPosition(!isNegative, series))
                || (!isNegative && position == ChartLabelPosition.Outer) || (SeriesRenderer is not null && SeriesRenderer.IsStackingColumnAndOuterPosition(isNegative, series));

            location = shouldApplyTopPosition
                ? !_inverted ? location - extraSpace - _margin.Bottom - _markerHeight : location + extraSpace + _margin.Left + _markerHeight
                : !_inverted ? location + extraSpace + _margin.Top + _markerHeight : location - extraSpace - _margin.Right - _markerHeight;

            return location;
        }

        /// <summary>
        /// Gets the label position corresponding to the given index.
        /// </summary>
        /// <param name="index">The position index (0-4).</param>
        /// <returns>A <see cref="ChartLabelPosition"/> enum value.</returns>
        private static ChartLabelPosition GetPosition(int index)
        {
            ChartLabelPosition[] pos = [ChartLabelPosition.Outer, ChartLabelPosition.Top, ChartLabelPosition.Bottom, ChartLabelPosition.Middle, ChartLabelPosition.Auto];
            return pos[index];
        }

        /// <summary>
        /// Computes the corner points of a rectangle for collision detection.
        /// </summary>
        /// <param name="rect">The rectangle to decompose.</param>
        /// <returns>A list of four <see cref="ChartEventLocation"/> points representing the corners.</returns>
        private static List<ChartEventLocation> GetRectanglePoints(Rect rect)
        {
            return
            [
                new ChartEventLocation(rect.X, rect.Y),
                new ChartEventLocation(rect.X + rect.Width, rect.Y),
                new ChartEventLocation(rect.X + rect.Width, rect.Y + rect.Height),
                new ChartEventLocation(rect.X, rect.Y + rect.Height)
            ];
        }

        /// <summary>
        /// Determines whether the current series is a rectangular series type.
        /// </summary>
        /// <returns><see langword="true"/> if the series is rectangular (column, bar, etc.); otherwise <see langword="false"/>.</returns>
        private bool IsRectSeries()
        {
            return SeriesRenderer is not null && SeriesRenderer.IsRectSeries();
        }

        /// <summary>
        /// Determines whether the data label should be rendered as a shape (border/background).
        /// </summary>
        /// <param name="style">The text render event arguments containing style information.</param>
        private void IsDataLabelShape(TextRenderEventArgs style)
        {
            _isShape = (!string.IsNullOrEmpty(style.Color) && style.Color != Constants.Transparent) || style.Border.Width > 0;
            _borderWidth = !double.IsNaN(style.Border.Width) ? style.Border.Width : 0;
            if (!_isShape)
            {
                _margin = new ChartEventMargin() { Left = 0, Right = 0, Bottom = 0, Top = 0 };
            }
        }

        /// <summary>
        /// Creates a data label template option for rendering outside SVG.
        /// </summary>
        /// <param name="series">The series containing the label.</param>
        /// <param name="dataLabel">The data label configuration.</param>
        /// <param name="point">The data point.</param>
        /// <param name="data">The text render event arguments.</param>
        /// <param name="labelIndex">The label index.</param>
        private void CreateDataLabelTemplate(ChartSeries series, ChartDataLabel dataLabel, Point point, TextRenderEventArgs data, int labelIndex)
        {
            ChartAxisRenderer v_Axis = Owner is not null && Owner._requireInvertedAxis ? SeriesRenderer?.XAxisRenderer ?? null! : SeriesRenderer?.YAxisRenderer ?? null!;
            ChartAxisRenderer h_Axis = Owner is not null && Owner._requireInvertedAxis ? SeriesRenderer?.YAxisRenderer ?? null! : SeriesRenderer?.XAxisRenderer ?? null!;
            _margin = new ChartEventMargin() { Left = 0, Right = 0, Bottom = 0, Top = 0 };
            Rect clip = SeriesRenderer?.ClipRect ?? null!;
            List<Size> pointTemplateSize = (Owner?._zoomingModule is not null && Owner._zoomingModule.IsZoomed && !Owner._zoomSettings.EnableDeferredZooming) ? _prevPointSize is not null ? _prevPointSize : point.TemplateSize : point.TemplateSize;
            Size templateSize = pointTemplateSize.Count > 0 ? pointTemplateSize[labelIndex] : new Size(0, 0);
            Rect rect = CalculateTextPosition(point, series, templateSize, dataLabel, labelIndex, _labelAngle, _isRotationEnabled);
            double posX = (SeriesRenderer?.ClipRect?.X ?? 0) + rect.X,
            posY = (SeriesRenderer?.ClipRect?.Y ?? 0) + rect.Y;
            string left = Convert.ToString(posX, _culture) + "px";
            string top = Convert.ToString(posY, _culture) + "px";
            Color rgbValue = Color.FromName(_fontBackground ?? string.Empty);
            string color = string.IsNullOrEmpty(dataLabel.Font.Color) ? dataLabel.Font.Color : (Math.Round(Convert.ToDouble(((rgbValue.R * 299) + (rgbValue.G * 587) + (rgbValue.B * 114)) / 1000, _culture), 1) >= 128 ? "black" : "white");
            bool isAnimation = ((series.Animation.Enable && SyncfusionService?._options.Animation == GlobalAnimationMode.Default) || (SyncfusionService?._options.Animation == GlobalAnimationMode.Enable)) && Owner is not null && Owner._shouldAnimateSeries;
            string visibility = !(pointTemplateSize.Count > 0) ? "hidden" : isAnimation ? "hidden" : "visible";
            string id;
            ChartAxisRenderer xAxisRenderer = SeriesRenderer?.XAxisRenderer ?? null!;

            if ((!ChartHelper.IsCollide(rect, Owner?._seriesContainer?._dataLabelCollection ?? null!, clip, Owner?._chartAreaType == ChartAreaType.CartesianAxes) || dataLabel.LabelIntersectAction == "None") && (SeriesRenderer?.SeriesType() != SeriesValueType.XY || point.YValue.Equals(double.NaN) ||
                ChartHelper.WithIn(point.YValue, SeriesRenderer.YAxisRenderer.VisibleRange) || (series.SeriesType is not null && series.SeriesType.Contains("100", StringComparison.InvariantCulture) && ChartHelper.WithIn(SeriesRenderer.StackedValues?.EndValues[point.Index] ?? 0, SeriesRenderer.YAxisRenderer.VisibleRange))) &&
                ChartHelper.WithIn(point.XValue, xAxisRenderer.VisibleRange) && posY >= v_Axis.Rect.Y && posX >= h_Axis.Rect.X && posY <= v_Axis.Rect.Y + v_Axis.Rect.Height && posX <= h_Axis.Rect.X + h_Axis.Rect.Width)
            {
                id = Owner?.ID + "_Series_" + (string.IsNullOrEmpty(Convert.ToString(SeriesRenderer?.Index, null)) ? Convert.ToString(SeriesRenderer?.Category(), null) : Convert.ToString(SeriesRenderer?.Index, null)) + "_DataLabelPoint_" + point.Index + "_Label_" + labelIndex;
                point.TemplateID.Add(id);
                Owner?._seriesContainer?._dataLabelCollection.Add(new Rect(rect.X + clip.X, rect.Y + clip.Y, rect.Width, rect.Height));
                ChartDataPointInfo templatedata = new()
                {
                    X = point.X,
                    Y = point.Y,
                    PointX = point.X,
                    PointY = point.Y,
                    PointIndex = point.Index,
                    PointText = point.Text,
                    Text = point.Text,
                    SeriesIndex = SeriesRenderer?.Index ?? 0,
                    SeriesName = series.Name
                };
                _templateOptions.Add(new DatalabelTemplateOptions()
                {
                    Id = id,
                    Style = "position: absolute;background-color:" + data.Color + ";" + ChartHelper.GetFontStyle(dataLabel.Font) + "border:" + data.Border.Width.ToString(_culture) + "px solid " + data.Border.Color + ";left:" + left + ";top:" + top + ";visibility:" + visibility + ";",
                    Template = data.Template(templatedata)
                });
                if (isAnimation)
                {
                    SeriesRenderer?.SeriesTemplateID.Add(id);
                }
            }
        }

        /// <summary>
        /// Renders data label shapes and text elements into the provided render tree for the series.
        /// </summary>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/> used to build the SVG render tree.</param>
        /// <param name="marker">The marker configuration containing data label settings.</param>
        private void RenderDatalabels(RenderTreeBuilder builder, ChartMarker marker)
        {
            if (!(marker.DataLabel is not null && marker.DataLabel.Visible && marker.DataLabel.Template is null))
            {
                return;
            }

            string transform = string.Empty, clipPath = string.Empty;
            if (Owner?._chartAreaType == ChartAreaType.CartesianAxes)
            {
                transform = "translate(" + SeriesRenderer?.ClipRect?.X.ToString(_culture) + "," + SeriesRenderer?.ClipRect?.Y.ToString(_culture) + ")";
                clipPath = "url(#" + Owner.ID + "_ChartSeriesClipRect_" + SeriesIndex() + ')';
            }

            List<RectOptions> stackLabelRects = Owner?._stackLabelRenderer?._rectOptions ?? [];

            Owner?._svgRenderer?.OpenGroupElement(builder, SeriesShapeId(), transform, clipPath);
            RenderIfNotOverlapped(builder, _rectOptions.ToArray(), _dataLabelActualRectOptions, stackLabelRects, (b, option) => Owner?._svgRenderer?.RenderRect(b, option));
            builder.CloseElement();

            Owner?._svgRenderer?.OpenGroupElement(builder, SeriesTextId(), transform, clipPath);

            RenderIfNotOverlapped(builder, _textOptions.ToArray(), _dataLabelActualRectOptions, stackLabelRects, (b, option) => ChartHelper.TextElement(b, Owner?._svgRenderer ?? null!, option, 0, true, true));
            builder.CloseElement();
        }

        /// <summary>
        /// Renders a collection of elements into the render tree, skipping any that overlap with stacked label rectangles.
        /// </summary>
        /// <typeparam name="T">The type of element to render.</typeparam>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/> used to build the SVG render tree.</param>
        /// <param name="elements">The collection of elements to render.</param>
        /// <param name="dataLabelRects">The list of actual data label rectangles used for overlap detection.</param>
        /// <param name="stackLabelRects">The list of stacked label rectangles to check against for overlaps.</param>
        /// <param name="renderAction">The action invoked to render each non-overlapping element.</param>
        private void RenderIfNotOverlapped<T>(RenderTreeBuilder builder, IList<T> elements, List<Rect> dataLabelRects, List<RectOptions> stackLabelRects, Action<RenderTreeBuilder, T> renderAction)
        {
            int count = Math.Min(elements.Count, dataLabelRects.Count);

            for (int i = 0; i < count; i++)
            {
                bool isOverlapped;
                Rect dataLabelRect = dataLabelRects[i];
                double labelX, labelY;
                if (Series.Type == ChartSeriesType.StackingBar100 && Series.Marker.DataLabel.Position == ChartLabelPosition.Outer)
                {
                    labelX = dataLabelRect.X;
                    labelY = dataLabelRect.Y;
                }
                else
                {
                    labelX = (dataLabelRect.X + SeriesRenderer?.ClipRect?.X) ?? 0;
                    labelY = (dataLabelRect.Y + SeriesRenderer?.ClipRect?.Y) ?? 0;
                }
                isOverlapped = stackLabelRects.Any(stackedRect => stackedRect.X < labelX + dataLabelRect.Width && stackedRect.X + stackedRect.Width > labelX && stackedRect.Y < labelY + dataLabelRect.Height && stackedRect.Y + stackedRect.Height > labelY);

                if (!isOverlapped)
                {
                    renderAction(builder, elements[i]);
                }
            }
        }

        /// <summary>
        /// Clears all accumulated rendering _options including templates, text _options, and rectangle _options.
        /// </summary>
        private void ClearRenderingOptions()
        {
            _templateOptions.Clear();
            _textOptions.Clear();
            _rectOptions.Clear();
        }

        /// <summary>
        /// Initializes the rendering state for data label processing.
        /// </summary>
        /// <param name="series">The chart series containing data label configuration.</param>
        /// <param name="dataLabel">The data label settings to apply.</param>
        /// <returns>A <see cref="RenderingState"/> containing initialized rendering parameters.</returns>
        private RenderingState InitializeRenderingState(ChartSeries series, ChartDataLabel dataLabel)
        {
            InitPrivateVariables(series, series.Marker);

            return new RenderingState
            {
                Inverted = Owner is not null && Owner._requireInvertedAxis,
                YAxisInversed = series.Renderer.YAxisRenderer.Axis is not null && series.Renderer.YAxisRenderer.Axis.IsAxisInverse,
                TemplateId = Owner?.ID + "_Series_" + (SeriesRenderer?.Index != 0 ? SeriesRenderer?.Category().ToString() : SeriesRenderer.Index.ToString(CultureInfo.InvariantCulture)) + "_DataLabelCollections",
                LabelAngle = dataLabel.LabelIntersectAction == "Rotate90" ? 90 : dataLabel.Angle,
                IsRotationEnabled = dataLabel.LabelIntersectAction == "Rotate90" || dataLabel.EnableRotation
            };
        }

        /// <summary>
        /// Retrieves visible data points and prepares rotation settings for label rendering.
        /// </summary>
        /// <param name="series">The chart series to process.</param>
        /// <param name="dataLabel">The data label configuration.</param>
        /// <param name="state">The rendering state containing orientation and rotation settings.</param>
        /// <param name="font">When this method returns, contains the font options for the data labels.</param>
        /// <returns>A list of visible <see cref="Point"/> objects ready for label rendering.</returns>
        private List<Point>? GetVisiblePointsAndPrepareRotation(ChartSeries series, ChartDataLabel dataLabel, RenderingState state, out ChartFontOptions font)
        {
            List<Point> visiblePoints = ChartHelper.GetVisiblePoints(SeriesRenderer?.Points ?? null!);
            _inverted = state.Inverted;
            _yAxisInversed = state.YAxisInversed;
            _prevPointSize = visiblePoints is not null && visiblePoints.Count > 0
                ? visiblePoints.FirstOrDefault(point => point.TemplateSize is not null && point.TemplateSize.Count > 0)?.TemplateSize ?? null!
                : [];

            font = dataLabel.Font.GetFontOptions(Owner?._chartThemeStyle ?? null!);
            ApplyAdaptiveRotationIfNeeded(series, visiblePoints!, font, ref state);
            _isRotationEnabled = state.IsRotationEnabled;
            _labelAngle = state.LabelAngle;

            if (SeriesRenderer is { })
            {
                SeriesRenderer.SeriesTemplateID = [];
            }
            return visiblePoints is null ? default : visiblePoints;
        }

        /// <summary>
        /// Applies adaptive rotation to data labels when they would overflow their containing region.
        /// </summary>
        /// <param name="series">The chart series being rendered.</param>
        /// <param name="visiblePoints">The collection of visible data points.</param>
        /// <param name="font">The font options used for measuring label dimensions.</param>
        /// <param name="state">The rendering state to update with rotation settings.</param>
        private void ApplyAdaptiveRotationIfNeeded(ChartSeries series, List<Point> visiblePoints, ChartFontOptions font, ref RenderingState state)
        {
            if (Owner is not null && Owner.EnableAdaptiveRendering
                && !(state.IsRotationEnabled && (state.LabelAngle == 90 || state.LabelAngle == -90))
                && !series.Renderer.IsPathSeries()
                && series.Type != ChartSeriesType.Bubble
                && series.SeriesType is not null
                && !series.SeriesType.Contains("Bar", StringComparison.InvariantCulture)
                && visiblePoints is not null
                && visiblePoints.Any(point => point.Regions.Count > 0 && ChartHelper.MeasureText(ChartHelper.GetLabelText(point, SeriesRenderer ?? null!)[0], font).Width > point.Regions[0].Width))
            {
                state.IsRotationEnabled = true;
                state.LabelAngle = -90;
            }
        }

        /// <summary>
        /// Processes and renders data labels for all visible points in the series.
        /// </summary>
        /// <param name="visiblePoints">The collection of visible data points to label.</param>
        /// <param name="series">The chart series containing the points.</param>
        /// <param name="dataLabel">The data label configuration settings.</param>
        /// <param name="state">The current rendering state with orientation and rotation settings.</param>
        /// <param name="font">The font options for rendering label text.</param>
        private void ProcessAllVisiblePoints(List<Point> visiblePoints, ChartSeries series, ChartDataLabel dataLabel, RenderingState state, ChartFontOptions font)
        {
            double angle = state.LabelAngle;
            string anchor = (angle == -90 && state.IsRotationEnabled)
                ? (dataLabel.Position == ChartLabelPosition.Top ? "end" : (dataLabel.Position == ChartLabelPosition.Middle ? "middle" : "start"))
                : "middle";

            for (int i = 0; i < visiblePoints?.Count; i++)
            {
                Point point = visiblePoints[i];
                ProcessSinglePoint(point, i, series, dataLabel, state, font, angle, anchor);
            }
        }

        /// <summary>
        /// Processes and renders data labels for a single data point.
        /// </summary>
        /// <param name="point">The data point to label.</param>
        /// <param name="pointIndex">The zero-based index of the point in the series.</param>
        /// <param name="series">The chart series containing the point.</param>
        /// <param name="dataLabel">The data label configuration settings.</param>
        /// <param name="state">The current rendering state.</param>
        /// <param name="font">The font options for rendering label text.</param>
        /// <param name="angle">The rotation angle in degrees.</param>
        /// <param name="anchor">The text anchor alignment ('start', 'middle', or 'end').</param>
        private void ProcessSinglePoint(Point point, int pointIndex, ChartSeries series, ChartDataLabel dataLabel, RenderingState state, ChartFontOptions font, double angle, string anchor)
        {
            _margin = new ChartEventMargin
            {
                Left = dataLabel.Margin.Left,
                Right = dataLabel.Margin.Right,
                Bottom = dataLabel.Margin.Bottom,
                Top = dataLabel.Margin.Top
            };

            if (!ShouldProcessPoint(point, series))
            {
                return;
            }

            List<string> labelText = GetLabelTextForPoint(point, dataLabel);
            point.TemplateID = [];

            ProcessLabelTextCollection(point, pointIndex, labelText, series, dataLabel, state, font, angle, anchor);
        }

        /// <summary>
        /// Determines whether a data point should be processed for label rendering.
        /// </summary>
        /// <param name="point">The data point to evaluate.</param>
        /// <param name="series">The chart series containing the point.</param>
        /// <returns>
        /// <c>true</c> if the point has symbol locations and is within the visible range; otherwise, <c>false</c>.
        /// </returns>
        private static bool ShouldProcessPoint(Point point, ChartSeries series)
        {
            return point.SymbolLocations.Count != 0
                && point.SymbolLocations[0] is not null
                && series.Renderer.IsPointWithInRange(point);
        }

        /// <summary>
        /// Retrieves the label text to display for a data point.
        /// </summary>
        /// <param name="point">The data point whose label text is requested.</param>
        /// <param name="dataLabel">The data label configuration containing the label format.</param>
        /// <returns>
        /// A list of label text strings. Returns an empty list if no custom label name is set
        /// and the point has no text value.
        /// </returns>
        private List<string> GetLabelTextForPoint(Point point, ChartDataLabel dataLabel)
        {
            return !string.IsNullOrEmpty(dataLabel.Name) && point.Text is null ? [] : ChartHelper.GetLabelText(point, SeriesRenderer ?? null!);
        }

        /// <summary>
        /// Processes all label text entries for a single data point.
        /// </summary>
        /// <param name="point">The data point being labeled.</param>
        /// <param name="pointIndex">The zero-based index of the point.</param>
        /// <param name="labelText">The collection of label text strings to render.</param>
        /// <param name="series">The chart series containing the point.</param>
        /// <param name="dataLabel">The data label configuration.</param>
        /// <param name="state">The current rendering state.</param>
        /// <param name="font">The font options for label text.</param>
        /// <param name="angle">The rotation angle in degrees.</param>
        /// <param name="anchor">The text anchor alignment.</param>
        private void ProcessLabelTextCollection(Point point, int pointIndex, List<string> labelText, ChartSeries series, ChartDataLabel dataLabel, RenderingState state, ChartFontOptions font, double angle, string anchor)
        {
            for (int j = 0; j < labelText.Count; j++)
            {
                ProcessSingleLabel(point, pointIndex, j, labelText[j], series, dataLabel, state, font, angle, anchor);
            }
        }

        /// <summary>
        /// Processes and renders a single label text entry for a data point.
        /// </summary>
        /// <param name="point">The data point being labeled.</param>
        /// <param name="pointIndex">The zero-based index of the point in the series.</param>
        /// <param name="labelIndex">The zero-based index of this label (for multi-label points).</param>
        /// <param name="text">The label text to render.</param>
        /// <param name="series">The chart series containing the point.</param>
        /// <param name="dataLabel">The data label configuration settings.</param>
        /// <param name="state">The current rendering state.</param>
        /// <param name="font">The font options for label text.</param>
        /// <param name="angle">The rotation angle in degrees.</param>
        /// <param name="anchor">The text anchor alignment.</param>
        private void ProcessSingleLabel(Point point, int pointIndex, int labelIndex, string text, ChartSeries series, ChartDataLabel dataLabel, RenderingState state, ChartFontOptions font, double angle, string anchor)
        {
            LabelLocation labelLocation = new(0, 0);
            TextRenderEventArgs argsData = CreateDataLabelRenderArgs(point, labelLocation, text, series, dataLabel);

            InvokeDataLabelRenderEvent(argsData);

            if (!argsData.Cancel && point.Visible)
            {
                PrepareDataLabelRendering(argsData, series, point);
                if (argsData.Template is not null)
                {
                    CreateDataLabelTemplate(series, dataLabel, point, argsData, labelIndex);
                }
                else
                {
                    RenderTextDataLabel(point, pointIndex, labelIndex, argsData, series, dataLabel, state, font, angle, anchor, labelLocation);
                }
            }
        }

        /// <summary>
        /// Creates the event arguments for the data label render event.
        /// </summary>
        /// <param name="point">The data point being labeled.</param>
        /// <param name="labelLocation">The calculated label location coordinates.</param>
        /// <param name="text">The label text content.</param>
        /// <param name="series">The chart series containing the point.</param>
        /// <param name="dataLabel">The data label configuration.</param>
        /// <returns>
        /// A <see cref="TextRenderEventArgs"/> instance containing all label rendering properties.
        /// </returns>
        private TextRenderEventArgs CreateDataLabelRenderArgs(Point point, LabelLocation labelLocation, string text, ChartSeries series, ChartDataLabel dataLabel)
        {
            return new TextRenderEventArgs(
                "OnDataLabelRender",
                false,
                series,
                point,
                labelLocation,
                text,
                dataLabel.Fill,
                new BorderModel() { Width = dataLabel.Border.Width, Color = dataLabel.Border.Color! },
                dataLabel.Template,
                dataLabel.Font.GetChartDefaultFont(Owner?._chartThemeStyle ?? null!),
                SeriesRenderer?.Index ?? 0);
        }

        /// <summary>
        /// Invokes the data label render event if a handler is registered.
        /// </summary>
        /// <param name="argsData">The event arguments containing label rendering details.</param>
        private void InvokeDataLabelRenderEvent(TextRenderEventArgs argsData)
        {
            if (Owner?.OnDataLabelRender is not null)
            {
                Owner.OnDataLabelRender.Invoke(argsData);
            }
        }

        /// <summary>
        /// Prepares internal state for data label rendering based on event arguments.
        /// </summary>
        /// <param name="argsData">The event arguments containing label style and appearance settings.</param>
        /// <param name="series">The chart series being rendered.</param>
        /// <param name="point">The data point being labeled.</param>
        private void PrepareDataLabelRendering(TextRenderEventArgs argsData, ChartSeries series, Point point)
        {
            _fontBackground = argsData.Color;
            IsDataLabelShape(argsData);
            _markerHeight = (series.Type == ChartSeriesType.Bubble) ? (point.Regions[0].Height / 2) : _markerHeight;
        }

        /// <summary>
        /// Renders a text-based data label for a data point.
        /// </summary>
        /// <param name="point">The data point being labeled.</param>
        /// <param name="pointIndex">The zero-based index of the point.</param>
        /// <param name="labelIndex">The zero-based index of this label.</param>
        /// <param name="argsData">The event arguments containing label rendering properties.</param>
        /// <param name="series">The chart series containing the point.</param>
        /// <param name="dataLabel">The data label configuration.</param>
        /// <param name="state">The current rendering state.</param>
        /// <param name="font">The font options for label text.</param>
        /// <param name="angle">The rotation angle in degrees.</param>
        /// <param name="anchor">The text anchor alignment.</param>
        /// <param name="labelLocation">The label location coordinates to update.</param>
        private void RenderTextDataLabel(Point point, int pointIndex, int labelIndex, TextRenderEventArgs argsData, ChartSeries series, ChartDataLabel dataLabel, RenderingState state, ChartFontOptions font, double angle, string anchor, LabelLocation labelLocation)
        {
            List<string> labelCollection = [];
            Size textSize = ChartHelper.MeasureText(argsData.Text, font);

            ParseMultilineText(argsData.Text, labelCollection);

            if (ShouldSkipBubbleLabel(series, dataLabel, textSize, point))
            {
                return;
            }

            Rect rect = CalculateTextPosition(point, series, textSize, dataLabel, labelIndex, state.LabelAngle, state.IsRotationEnabled);
            ProcessTextLabelRendering(point, pointIndex, labelIndex, rect, textSize, argsData, series, dataLabel, state, angle, anchor, labelLocation, labelCollection);
        }

        /// <summary>
        /// Parses multi-line label text separated by HTML break tags.
        /// </summary>
        /// <param name="text">The label text that may contain break tags.</param>
        /// <param name="labelCollection">The collection to populate with parsed text lines.</param>
        private static void ParseMultilineText(string text, List<string> labelCollection)
        {
            if (text.Contains("<br>", StringComparison.InvariantCulture) || text.Contains("<br/>", StringComparison.InvariantCulture))
            {
                string separator = text.Contains("<br>", StringComparison.InvariantCulture) ? "<br>" : "<br/>";
                labelCollection.AddRange([.. text.Split(separator)]);
            }
        }

        /// <summary>
        /// Determines whether a bubble chart label should be skipped due to size constraints.
        /// </summary>
        /// <param name="series">The chart series being evaluated.</param>
        /// <param name="dataLabel">The data label configuration.</param>
        /// <param name="textSize">The measured size of the label text.</param>
        /// <param name="point">The data point containing region dimensions.</param>
        /// <returns>
        /// <c>true</c> if the label should be skipped because it exceeds the bubble width
        /// in adaptive rendering mode; otherwise, <c>false</c>.
        /// </returns>
        private bool ShouldSkipBubbleLabel(ChartSeries series, ChartDataLabel dataLabel, Size textSize, Point point)
        {
            return Owner is not null
                && Owner.EnableAdaptiveRendering
                && series.Type == ChartSeriesType.Bubble
                && dataLabel.Position == ChartLabelPosition.Middle
                && textSize.Width > point.Regions[0].Width;
        }

        /// <summary>
        /// Processes the final text label rendering including overlap detection and rendering.
        /// </summary>
        /// <param name="point">The data point being labeled.</param>
        /// <param name="pointIndex">The zero-based index of the point.</param>
        /// <param name="labelIndex">The zero-based index of this label.</param>
        /// <param name="rect">The calculated label rectangle.</param>
        /// <param name="textSize">The measured size of the label text.</param>
        /// <param name="argsData">The event arguments containing label properties.</param>
        /// <param name="series">The chart series containing the point.</param>
        /// <param name="dataLabel">The data label configuration.</param>
        /// <param name="state">The current rendering state.</param>
        /// <param name="angle">The rotation angle in degrees.</param>
        /// <param name="anchor">The text anchor alignment.</param>
        /// <param name="labelLocation">The label location coordinates.</param>
        /// <param name="labelCollection">The collection of parsed label text lines.</param>
        private void ProcessTextLabelRendering(Point point, int pointIndex, int labelIndex, Rect rect, Size textSize, TextRenderEventArgs argsData, ChartSeries series, ChartDataLabel dataLabel, RenderingState state, double angle, string anchor, LabelLocation labelLocation, List<string> labelCollection)
        {
            Rect clip = SeriesRenderer?.ClipRect ?? null!;
            Rect actualRect = new(rect.X + clip.X, rect.Y + clip.Y, rect.Width, rect.Height);

            OverlapCheckResult overlapResult = CheckLabelOverlap(actualRect, rect, labelIndex, state, angle, dataLabel);

            if (overlapResult.IsNotOverlapping)
            {
                Owner?._seriesContainer?._dataLabelCollection.Add(actualRect);
                RenderFinalTextLabel(point, pointIndex, labelIndex, rect, textSize, argsData, series, dataLabel, state, angle, anchor, labelLocation, labelCollection, clip);
            }
        }

        /// <summary>
        /// Checks whether a label overlaps with existing labels in the chart.
        /// </summary>
        /// <param name="actualRect">The absolute rectangle of the label in chart coordinates.</param>
        /// <param name="rect">The relative rectangle of the label.</param>
        /// <param name="labelIndex">The zero-based index of this label.</param>
        /// <param name="state">The current rendering state containing rotation settings.</param>
        /// <param name="angle">The rotation angle in degrees.</param>
        /// <param name="dataLabel">The data label configuration.</param>
        /// <returns>
        /// An <see cref="OverlapCheckResult"/> containing overlap status and rotated rectangle points
        /// for further collision detection.
        /// </returns>
        private OverlapCheckResult CheckLabelOverlap(Rect actualRect, Rect rect, int labelIndex, RenderingState state, double angle, ChartDataLabel dataLabel)
        {
            List<List<ChartEventLocation>> rectPoints = [];
            bool isNotOverlapping;

            if (state.IsRotationEnabled && angle != 0)
            {
                List<ChartEventLocation> rectCoordinates = GetRectanglePoints(actualRect);
                rectPoints.Add(ChartHelper.GetRotatedRectangleCoordinates(rectCoordinates, actualRect.X + (actualRect.Width * 0.5), actualRect.Y - (actualRect.Height / 2), angle));

                isNotOverlapping = CheckRotatedRectangleOverlap(rectPoints, labelIndex);
            }
            else
            {
                isNotOverlapping = dataLabel.LabelIntersectAction == "None" || !ChartHelper.IsCollide(rect, Owner?._seriesContainer?._dataLabelCollection ?? null!, SeriesRenderer?.ClipRect ?? null!, Owner?._chartAreaType == ChartAreaType.CartesianAxes);
            }

            return new OverlapCheckResult { IsNotOverlapping = isNotOverlapping, RectPoints = rectPoints };
        }

        /// <summary>
        /// Checks whether a rotated rectangle overlaps with previously rendered rotated rectangles.
        /// </summary>
        /// <param name="rectPoints">The collection of all rotated rectangle coordinate sets.</param>
        /// <param name="currentIndex">The zero-based index of the current rectangle to check.</param>
        /// <returns>
        /// <c>true</c> if the current rectangle does not overlap with any previous rectangles;
        /// otherwise, <c>false</c>.
        /// </returns>
        private static bool CheckRotatedRectangleOverlap(List<List<ChartEventLocation>> rectPoints, int currentIndex)
        {
            for (int index = currentIndex; index > 0; index--)
            {
                if (rectPoints.Count > currentIndex
                    && rectPoints[currentIndex] is not null
                    && rectPoints[index - 1] is not null
                    && ChartHelper.IsRotatedRectIntersect(rectPoints[currentIndex], rectPoints[index - 1]))
                {
                    rectPoints[currentIndex] = null!;
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Renders the final text label with all styling, positioning, and rotation applied.
        /// </summary>
        /// <param name="point">The data point being labeled.</param>
        /// <param name="pointIndex">The zero-based index of the point.</param>
        /// <param name="labelIndex">The zero-based index of this label.</param>
        /// <param name="rect">The label rectangle.</param>
        /// <param name="textSize">The measured size of the label text.</param>
        /// <param name="argsData">The event arguments containing label properties.</param>
        /// <param name="series">The chart series containing the point.</param>
        /// <param name="dataLabel">The data label configuration.</param>
        /// <param name="state">The current rendering state.</param>
        /// <param name="angle">The rotation angle in degrees.</param>
        /// <param name="anchor">The text anchor alignment.</param>
        /// <param name="labelLocation">The label location coordinates.</param>
        /// <param name="labelCollection">The collection of parsed label text lines.</param>
        /// <param name="clip">The clipping rectangle for the series.</param>
        private void RenderFinalTextLabel(Point point, int pointIndex, int labelIndex, Rect rect, Size textSize, TextRenderEventArgs argsData, ChartSeries series, ChartDataLabel dataLabel, RenderingState state, double angle, string anchor, LabelLocation labelLocation, List<string> labelCollection, Rect clip)
        {
            double contrast = CalculateLuminanceContrast(_fontBackground!);

            LabelPositionInfo posInfo = CalculateLabelPosition(rect, textSize, labelLocation, state, angle, dataLabel, clip, series);

            string rotation = BuildRotationTransform(posInfo.Degree, posInfo.XValue, posInfo.YValue, textSize.Width, dataLabel.Position, angle);

            AddShapeIfNeeded(point, pointIndex, rect, argsData, dataLabel, rotation);
            AddTextLabel(point, labelIndex, posInfo, argsData, dataLabel, contrast, anchor, rotation, labelCollection);
        }

        /// <summary>
        /// Calculates the luminance contrast ratio for a background color.
        /// </summary>
        /// <param name="backgroundColor">The background color as a string (name, hex, or rgb).</param>
        /// <returns>
        /// A contrast value between 0 and 255, where values >= 128 indicate light backgrounds
        /// and values &lt; 128 indicate dark backgrounds.
        /// </returns>
        private double CalculateLuminanceContrast(string backgroundColor)
        {
            Color rgbValue = ChartHelper.GetRBGValue(backgroundColor);
            return Math.Round(Convert.ToDouble(((rgbValue.R * 299) + (rgbValue.G * 587) + (rgbValue.B * 114)) / 1000, _culture), 1);
        }

        /// <summary>
        /// Calculates the final position and rotation parameters for a data label.
        /// </summary>
        /// <param name="rect">The label rectangle.</param>
        /// <param name="textSize">The measured size of the label text.</param>
        /// <param name="labelLocation">The label location coordinates to update.</param>
        /// <param name="state">The current rendering state with rotation settings.</param>
        /// <param name="angle">The rotation angle in degrees.</param>
        /// <param name="dataLabel">The data label configuration.</param>
        /// <param name="clip">The clipping rectangle for bounds checking.</param>
        /// <param name="series">The chart series containing the label.</param>
        /// <returns>
        /// A <see cref="LabelPositionInfo"/> containing final X/Y positions, rotation center,
        /// and rotation degree for the label.
        /// </returns>
        private LabelPositionInfo CalculateLabelPosition(Rect rect, Size textSize, LabelLocation labelLocation, RenderingState state, double angle, ChartDataLabel dataLabel, Rect clip, ChartSeries series)
        {
            double xPos = rect.X + _margin.Left + (textSize.Width / 2) + labelLocation.X;
            double yPos = rect.Y + _margin.Top + (textSize.Height * 3 / 4) + labelLocation.Y;
            labelLocation.X = 0;
            labelLocation.Y = 0;

            double xValue, yValue, degree;

            if (angle != 0 && state.IsRotationEnabled)
            {
                xValue = xPos - ((_isShape ? dataLabel.Margin.Left : 5) / 2) + ((_isShape ? dataLabel.Margin.Right : 5) / 2);
                yValue = yPos - ((_isShape ? dataLabel.Margin.Top : 5) / 2) - (textSize.Height / (_isShape ? dataLabel.Margin.Top : 5)) + ((_isShape ? dataLabel.Margin.Bottom : 5) / 2);
                degree = (angle > 360) ? angle - 360 : (angle < -360) ? angle + 360 : angle;
            }
            else
            {
                degree = 0;
                xValue = rect.X;
                yValue = rect.Y;

                AdjustPositionForClipping(ref xPos, ref yPos, textSize, clip, series);
            }

            return new LabelPositionInfo
            {
                XPos = xPos,
                YPos = yPos,
                XValue = xValue,
                YValue = yValue,
                Degree = degree
            };
        }

        /// <summary>
        /// Adjusts label position to prevent overflow beyond the clipping rectangle.
        /// </summary>
        /// <param name="xPos">The X position to adjust.</param>
        /// <param name="yPos">The Y position to adjust.</param>
        /// <param name="textSize">The measured size of the label text.</param>
        /// <param name="clip">The clipping rectangle defining bounds.</param>
        /// <param name="series">The chart series containing the label.</param>
        private void AdjustPositionForClipping(ref double xPos, ref double yPos, Size textSize, Rect clip, ChartSeries series)
        {
            xPos -= Owner?._chartAreaType == ChartAreaType.CartesianAxes && xPos + (textSize.Width / 2) > clip.Width
                ? (!Owner._requireInvertedAxis && xPos > clip.Width) ? 0 : xPos + (textSize.Width / 2) - clip.Width
                : 0;

            yPos -= (Owner?._chartAreaType == ChartAreaType.CartesianAxes
                && yPos + textSize.Height > clip.Y + clip.Height
                && !(series.SeriesType is not null && series.SeriesType.Contains("Bar", StringComparison.OrdinalIgnoreCase)))
                ? yPos + textSize.Height - (clip.Y + clip.Height)
                : 0;
        }

        /// <summary>
        /// Builds the SVG transform attribute for label rotation and translation.
        /// </summary>
        /// <param name="degree">The rotation angle in degrees.</param>
        /// <param name="xValue">The X coordinate of the rotation center.</param>
        /// <param name="yValue">The Y coordinate of the rotation center.</param>
        /// <param name="textWidth">The width of the label text.</param>
        /// <param name="position">The label position setting.</param>
        /// <param name="angle">The original rotation angle before normalization.</param>
        /// <returns>
        /// An SVG transform string combining translation and rotation, or an empty string
        /// if no transform is needed.
        /// </returns>
        private string BuildRotationTransform(double degree, double xValue, double yValue, double textWidth, ChartLabelPosition position, double angle)
        {
            double translateY = position == ChartLabelPosition.Outer && (angle == 270 || angle == 90) ? -(textWidth / 2) : 0;
            return "translate(0," + translateY.ToString(_culture) + ")"
                + "rotate(" + degree.ToString(_culture) + "," + xValue.ToString(_culture) + "," + yValue.ToString(_culture) + ")";
        }

        /// <summary>
        /// Adds a background shape rendering option for the data label if styling is configured.
        /// </summary>
        /// <param name="point">The data point being labeled.</param>
        /// <param name="pointIndex">The zero-based index of the point.</param>
        /// <param name="rect">The label rectangle defining shape bounds.</param>
        /// <param name="argsData">The event arguments containing shape styling.</param>
        /// <param name="dataLabel">The data label configuration with shape properties.</param>
        /// <param name="rotation">The SVG transform string for rotation.</param>
        private void AddShapeIfNeeded(Point point, int pointIndex, Rect rect, TextRenderEventArgs argsData, ChartDataLabel dataLabel, string rotation)
        {
            if (_isShape)
            {
                RectOptions rectOption = new(
                    _commonId + point.Index + "_TextShape_" + pointIndex,
                    rect.X,
                    rect.Y,
                    rect.Width,
                    rect.Height,
                    argsData.Border.Width,
                    argsData.Border.Color,
                    string.IsNullOrEmpty(argsData.Color) ? Constants.Transparent : argsData.Color,
                    dataLabel.Rx,
                    dataLabel.Ry,
                    dataLabel.Opacity)
                {
                    Transform = rotation
                };
                _rectOptions.Add(rectOption);
            }
            _dataLabelActualRectOptions.Add(rect);
        }

        /// <summary>
        /// Adds a text rendering option for the data label.
        /// </summary>
        /// <param name="point">The data point being labeled.</param>
        /// <param name="labelIndex">The zero-based index of this label.</param>
        /// <param name="posInfo">The calculated position information.</param>
        /// <param name="argsData">The event arguments containing text properties.</param>
        /// <param name="dataLabel">The data label configuration.</param>
        /// <param name="contrast">The background luminance contrast value.</param>
        /// <param name="anchor">The text anchor alignment.</param>
        /// <param name="rotation">The SVG transform string for rotation.</param>
        /// <param name="labelCollection">The collection of parsed multi-line text.</param>
        private void AddTextLabel(Point point, int labelIndex, LabelPositionInfo posInfo, TextRenderEventArgs argsData, ChartDataLabel dataLabel, double contrast, string anchor, string rotation, List<string> labelCollection)
        {
            string color = string.IsNullOrEmpty(argsData.Font.Color)
                ? (contrast >= 128 ? "black" : "white")
                : argsData.Font.Color;

            TextOptions option = new(
                posInfo.XPos.ToString(_culture),
                posInfo.YPos.ToString(_culture),
                color,
                dataLabel.Font.GetFontOptions(Owner?._chartThemeStyle ?? null!),
                argsData.Text,
                anchor,
                _commonId + point.Index + "_Text_" + labelIndex,
                rotation,
                posInfo.Degree.ToString(_culture),
                "auto")
            {
                TextCollection = labelCollection
            };

            string[] locations = ChartHelper.AppendTextElements(Owner ?? null!, option.Id, Convert.ToDouble(option.X, _culture), Convert.ToDouble(option.Y, _culture));
            option.X = locations[0];
            option.Y = locations[1];

            _textOptions.Add(option);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Builds the render tree for the data label renderer, rendering all data labels for visible series points.
        /// </summary>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/> used to construct the component output.</param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (builder is null)
            {
                return;
            }

            base.BuildRenderTree(builder);
            if (SeriesRenderer?.Series is not null && SeriesRenderer.Series.Visible && Owner is not null && Owner._shouldRenderDataLabel)
            {
                Owner._seriesContainer?._dataLabelCollection.Clear();
                Owner._svgRenderer?.OpenGroupElement(builder, Owner.ID + "_DataLabelCollection");
                RenderDatalabels(builder, Series.Marker);
                builder.CloseElement();
            }
        }

        /// <summary>
        /// Invoked when the axis layout changes. Derived renderers override this method
        /// to apply type-specific layout update logic.
        /// </summary>
        protected virtual void OnLayoutChange()
        {

        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Returns the SVG group element identifier for the data label text group of this series.
        /// </summary>
        /// <returns>A string representing the text group element id.</returns>
        internal string SeriesTextId()
        {
            return Owner?.ID + "TextGroup" + SeriesIndex();
        }

        /// <summary>
        /// Returns the SVG group element identifier for the data label shape group of this series.
        /// </summary>
        /// <returns>A string representing the shape group element id.</returns>
        internal string SeriesShapeId()
        {
            return Owner?.ID + "ShapeGroup" + SeriesIndex();
        }

        /// <summary>
        /// Calculates and accumulates all rendering options — including rectangle, text, and template options —
        /// for the data labels of the specified series and data label configuration.
        /// </summary>
        /// <param name="series">The <see cref="ChartSeries"/> whose data labels are being prepared.</param>
        /// <param name="dataLabel">The <see cref="ChartDataLabel"/> configuration containing style, position, and template settings.</param>
        internal void CalculateRenderTreeBuilderOptions(ChartSeries series, ChartDataLabel dataLabel)
        {
            if (SeriesRenderer is null)
            {
                return;
            }

            RenderingState state = InitializeRenderingState(series, dataLabel);
            List<Point>? visiblePoints = GetVisiblePointsAndPrepareRotation(series, dataLabel, state, out ChartFontOptions font);

            if (visiblePoints is not null)
            {
                ProcessAllVisiblePoints(visiblePoints, series, dataLabel, state, font);
            }
        }

        /// <summary>
        /// Toggles the visibility of data labels, triggering a re-render or clearing rendering options
        /// based on the current <see cref="ChartDataLabel.Visible"/> state.
        /// </summary>
        internal void ToggleVisibility()
        {
            if (Series.Marker.DataLabel.Visible)
            {
                DatalabelValueChanged();
            }
            else
            {
                RendererShouldRender = true;
                ClearRenderingOptions();
                InvalidateRender();
            }
        }

        /// <summary>
        /// Recalculates and re-renders data label options when data label properties change,
        /// clearing stale options before rebuilding render tree options.
        /// </summary>
        internal void DatalabelValueChanged()
        {
            RendererShouldRender = Series.Marker.DataLabel.Visible && Owner is not null && Owner._shouldRenderDataLabel;
            if (RendererShouldRender)
            {
                ClearRenderingOptions();
                CalculateRenderTreeBuilderOptions(Series, Series.Marker.DataLabel);
            }

            InvalidateRender();
        }

        /// <summary>
        /// Recalculates and updates the positions of data label templates,
        /// clearing existing options and refreshing the data label template container.
        /// </summary>
        internal void UpdateDatalabelTemplatePosition()
        {
            if (Series.Marker.DataLabel.Template is null || !Series.Marker.DataLabel.Visible || (Owner is not null && !Owner._shouldRenderDataLabel))
            {
                return;
            }

            ClearRenderingOptions();
            Owner?._seriesContainer?._dataLabelCollection.Clear();
            CalculateRenderTreeBuilderOptions(Series, Series.Marker.DataLabel);
            Owner?._datalabelTemplateContainer?.InvalidateRender();
        }

        /// <summary>
        /// Releases managed resources held by the data label renderer, including the margin reference.
        /// </summary>
        internal void Dispose()
        {
            _margin = null!;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Handles a chart size change by recalculating data label rendering options
        /// for the updated chart dimensions.
        /// </summary>
        /// <param name="rect">The new bounding <see cref="Rect"/> of the chart area.</param>
        public override void HandleChartSizeChange(Rect rect)
        {
            _templateOptions.Clear();
            _dataLabelActualRectOptions.Clear();
            RendererShouldRender = Series.Marker.DataLabel.Visible;
            SeriesRenderer = Series.Renderer;
            if (RendererShouldRender && SeriesRenderer.Series is not null && SeriesRenderer.Series.Visible && Owner is not null && Owner._shouldRenderDataLabel)
            {
                CalculateRenderTreeBuilderOptions(Series, Series.Marker.DataLabel);
            }
        }

        /// <summary>
        /// Propagates a layout change notification to the axis layout change handler.
        /// </summary>
        public void HandleLayoutChange()
        {
            OnLayoutChange();
        }

        /// <summary>
        /// Schedules an asynchronous re-render for this data label renderer.
        /// </summary>
        public void InvalidateRender()
        {
            _ = InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Schedules an asynchronous re-render for this data label renderer and propagates
        /// the render request to the associated data label template container when present.
        /// </summary>
        public override void ProcessRenderQueue()
        {
            _ = InvokeAsync(StateHasChanged);
            Owner?._datalabelTemplateContainer?.InvalidateRender();
        }
        #endregion
    }
    #region Helper Class

    /// <summary>
    /// Represents the mutable state used during data label rendering for a single series.
    /// </summary>
    /// <remarks>
    /// This type aggregates orientation, rotation and template identifier information
    /// that is computed once and passed through the label rendering pipeline.
    /// </remarks>
    internal class RenderingState
    {
        /// <summary>
        /// Gets or sets a value indicating whether the chart axes are inverted.
        /// </summary>
        public bool Inverted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Y axis is rendered in inverse order.
        /// </summary>
        public bool YAxisInversed { get; set; }

        /// <summary>
        /// Gets or sets the identifier used for grouping data label templates in the DOM.
        /// </summary>
        public string TemplateId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the rotation angle (in degrees) that should be applied to labels.
        /// </summary>
        public double LabelAngle { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether label rotation behavior is enabled.
        /// </summary>
        public bool IsRotationEnabled { get; set; }
    }

    /// <summary>
    /// Represents the result of an overlap check for a candidate label rectangle.
    /// </summary>
    /// <remarks>
    /// Contains both a boolean result indicating non-overlap and, when rotation is applied,
    /// the computed rotated rectangle point sets for further collision checks.
    /// </remarks>
    internal class OverlapCheckResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether the label does not overlap any existing labels.
        /// </summary>
        public bool IsNotOverlapping { get; set; }

        /// <summary>
        /// Gets or sets the collection of rotated rectangle coordinate lists used for rotated-collision checks.
        /// Each inner list contains corner coordinates for a single rotated rectangle.
        /// </summary>
        public List<List<ChartEventLocation>> RectPoints { get; set; } = [];
    }

    /// <summary>
    /// Carries final computed position and rotation values for a rendered label.
    /// </summary>
    /// <remarks>
    /// This helper stores the X/Y positions used for text placement, the rotation center (XValue/YValue),
    /// and the normalized rotation degree applied to the label.
    /// </remarks>
    internal class LabelPositionInfo
    {
        /// <summary>
        /// Gets or sets the computed X position for the text anchor (in relative coordinates).
        /// </summary>
        public double XPos { get; set; }

        /// <summary>
        /// Gets or sets the computed Y position for the text baseline (in relative coordinates).
        /// </summary>
        public double YPos { get; set; }

        /// <summary>
        /// Gets or sets the X coordinate of the rotation center used when rotating the label.
        /// </summary>
        public double XValue { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate of the rotation center used when rotating the label.
        /// </summary>
        public double YValue { get; set; }

        /// <summary>
        /// Gets or sets the normalized rotation degree applied to the label (0 when no rotation).
        /// </summary>
        public double Degree { get; set; }
    }
    #endregion
}