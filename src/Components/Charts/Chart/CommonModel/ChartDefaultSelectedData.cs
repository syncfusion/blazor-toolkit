using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides options to customize the selected data of the chart component.
    /// </summary>
    public class ChartDefaultSelectedData : ChartSubComponent
    {
        #region Properties

        /// <summary>
        /// Gets or sets the series index for the selected data.
        /// </summary>
        /// <value>
        /// An integer representing the index of the series in the chart data that is selected.
        /// </value>
        /// <remarks>
        /// Use this property to specify or retrieve the series index of the data point that has been selected in the chart.
        /// This can be useful for interacting with specific data within the series.
        /// </remarks>
        [Parameter]
        public int Series { get; set; }

        /// <summary>
        /// Gets or sets the point index for the selected data.
        /// </summary>
        /// <value>
        /// An integer representing the index of the point within the series that is selected.
        /// </value>
        /// <remarks>
        /// This property allows for the identification and manipulation of a selected data point within its series,
        /// enabling specific point-level handling and customization in the chart.
        /// </remarks>
        [Parameter]
        public int Point { get; set; }
        #endregion
    }
}