using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renderer for category axes. Handles labels that map to an explicit collection of string categories.
    /// </summary>
    /// <remarks>
    /// This renderer is used for discrete category data where each data point corresponds to a specific
    /// category label. It supports label placement between ticks for better visual alignment.
    /// </remarks>
    internal class CategoryAxisRenderer : DateTimeBase
    {
        #region Private Methods

        /// <summary>
        /// Determines if a label should be rendered at the given position.
        /// </summary>
        /// <param name="position">The position to check.</param>
        /// <returns><see langword="true"/> if label should be rendered; otherwise, <see langword="false"/>.</returns>
        private bool ShouldRenderLabel(double position)
        {
            return ChartHelper.WithIn(position, VisibleRange) && Labels.Count > 0 && Labels.Count > (int)position && (int)position >= 0;
        }

        /// <summary>
        /// Gets the label text for a given position.
        /// </summary>
        /// <param name="position">The position index.</param>
        /// <returns>The label text at the position.</returns>
        private string GetLabelText(int position)
        {
            return !string.IsNullOrEmpty(Labels[position])
                ? Labels[position].ToString(CultureInfo.InvariantCulture)
                : string.Empty;
        }

        /// <summary>
        /// Calculates the ticks padding based on label placement and chart type.
        /// </summary>
        /// <returns>The calculated ticks padding value.</returns>
        private double CalculateTicksPadding()
        {
            bool isBetweenTicks = Axis?.LabelPlacement == LabelPlacement.BetweenTicks;
            return isBetweenTicks ? 0.5 : 0;
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Calculates the actual interval for the category axis.
        /// </summary>
        /// <param name="range">The range to calculate interval for.</param>
        /// <returns>The calculated interval value.</returns>
        internal override double CalculateActualInterval(DoubleRange range)
        {
            return (Axis is not null && double.IsNaN(Axis.Interval))
                ? Math.Max(1, Math.Floor(range.Delta / GetActualDesiredIntervalsCount()))
                : Math.Ceiling(Axis?.Interval ?? 0);
        }

        /// <summary>
        /// Generates visible labels for the category axis.
        /// </summary>
        internal override void GenerateVisibleLabels()
        {
            VisibleLabels = [];
            double tempInterval = Math.Ceiling(VisibleRange.Start);

            if (Axis?.ZoomFactor < 1 || Axis?.ZoomPosition > 0)
            {
                tempInterval = VisibleRange.Start - (VisibleRange.Start % VisibleInterval);
            }

            while (tempInterval <= Math.Floor(VisibleRange.End))
            {
                if (ShouldRenderLabel(tempInterval))
                {
                    int position = (int)Math.Round(tempInterval);
                    string labelText = GetLabelText(position);
                    TriggerLabelRender(position, labelText);
                }

                tempInterval += VisibleInterval;
            }

            GetMaxLabelWidth();
            base.GenerateVisibleLabels();
        }

        /// <summary>
        /// Gets the axis data for a given point value.
        /// </summary>
        /// <param name="pointValue">The point value to get data for.</param>
        /// <returns>The category label at the given point value.</returns>
        internal override object GetAxisData(double pointValue)
        {
            return double.IsNegativeInfinity(pointValue) || double.IsPositiveInfinity(pointValue) ? null! : (pointValue < Labels.Count ? Labels[(int)Math.Floor(pointValue)] : null!);
        }

        /// <summary>
        /// Determines if this axis is a category axis.
        /// </summary>
        /// <returns><see langword="true"/> if labels are placed between ticks; otherwise, <see langword="false"/>.</returns>
        internal override bool IsCategory()
        {
            return Axis?.LabelPlacement == LabelPlacement.BetweenTicks;
        }
        #endregion

        #region Protected Methods

        /// <summary>
        /// Applies range padding for categorical axes (accounting for between-ticks).
        /// </summary>
        /// <param name="range">The range to apply padding to.</param>
        /// <param name="interval">The interval value (not used for category axis).</param>
        /// <returns>The range with appropriate padding applied.</returns>
        protected override DoubleRange ApplyRangePadding(DoubleRange range, double interval)
        {
            double ticks = CalculateTicksPadding();
            double minimum = range.Start;
            double maximum = range.End;

            if (ticks > 0)
            {
                minimum -= ticks;
                maximum += ticks;
            }
            else
            {
                maximum += !double.IsNaN(maximum) ? 0 : 0.5;
            }

            return new DoubleRange(minimum, maximum);
        }
        #endregion
    }
}