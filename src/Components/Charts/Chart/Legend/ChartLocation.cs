using Microsoft.AspNetCore.Components;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Gets or sets the option for customizing the legend location in the <see cref="SfChart">Chart</see>.
    /// </summary>
    /// <remarks>
    /// This is only applicable when the <see cref="ChartLegendSettings.Position"/> is set to <c>LegendPosition.Custom</c>.
    /// </remarks>
    public class ChartLocation : ChartDefaultLocation
    {
        #region Fields
        private double _xCoordinate;
        private double _yCoordinate;
        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the associated chart legend settings.
        /// </summary>
        internal ChartLegendSettings? ChartLegend { get; set; }
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the x-coordinate of the legend's location. This is only applicable when the <see cref="ChartLegendSettings.Position"/> is set to <c>LegendPosition.Custom</c>.
        /// </summary>
        /// <value> 
        /// A double value representing the x-coordinate of the legend's location, measured in pixels. The default value is <b>0</b> pixel. 
        /// </value> 
        /// <remarks>
        /// This property accepts numerical values to precisely control the x-coordinate of the legend's location.
        /// </remarks>
        /// <example>   
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to determine the x location of a legend based on its position in a chart.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" />
        ///     </ChartSeriesCollection>
        ///     <ChartLegendSettings Visible="true" Position="LegendPosition.Custom">
        ///         <ChartLocation X="30" />
        ///     </ChartLegendSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override double X { get; set; }

        /// <summary>
        /// Gets or sets the y-coordinate of the legend's location. This is only applicable when the <see cref="ChartLegendSettings.Position"/> is set to <c>LegendPosition.Custom</c>.
        /// </summary>
        /// <value> 
        /// A double value representing the y-coordinate of the legend's location, measured in pixels. The default value is <b>0</b> pixel. 
        /// </value> 
        /// <remarks>
        /// This property accepts numerical values to precisely control the y-coordinate of the legend's location.
        /// </remarks>
        /// <example>   
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to determine the y location of a legend based on its position in a chart.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" />
        ///     </ChartSeriesCollection>
        ///     <ChartLegendSettings Visible="true" Position="LegendPosition.Custom">
        ///         <ChartLocation Y="40" />
        ///     </ChartLegendSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override double Y { get; set; }
		
        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Performs initialization when the component is initialized.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Tracker is ChartLegendSettings legendSettings)
            {
                ChartLegend = legendSettings;
            }

            _xCoordinate = X;
            _yCoordinate = Y;
            ChartLegend?.UpdateLegendProperties("Location", this);
        }

        /// <summary>
        /// Handles parameter changes and ensures the chart legend render queue is processed.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (_xCoordinate != X || _yCoordinate != Y)
            {
                _isPropertyChanged = true;
                ChartLegend?.UpdateLegendProperties("Location", this);
                _xCoordinate = X;
                _yCoordinate = Y;
            }

            Chart?._legendRenderer?.ProcessRenderQueue();
        }
        #endregion
    }
}
