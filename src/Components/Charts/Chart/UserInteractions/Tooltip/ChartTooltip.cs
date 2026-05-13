namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Chart tooltip runtime binder that leverages <see cref="TooltipBase"/> infrastructure.
    /// </summary>
    /// <remarks>
    /// This class serves as a thin runtime orchestrator for chart tooltips; behavior is implemented in the base.
    /// </remarks>
    internal class ChartTooltip : TooltipBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartTooltip"/> class.
        /// </summary>
        /// <param name="sfchart">The chart instance.</param>
        internal ChartTooltip(SfChart sfchart) : base(sfchart)
        {
            Chart = sfchart;
        }

        #endregion
    }
}
