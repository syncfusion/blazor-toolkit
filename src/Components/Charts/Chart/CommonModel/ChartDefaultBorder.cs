using Microsoft.AspNetCore.Components;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides options for customizing the color and width of chart borders.
    /// </summary>
    public class ChartDefaultBorder : ChartSubComponent
    {
        #region Fields
        private string? _color;
        private double _width = 1;
        #endregion

        #region Properties

        /// <summary>
        /// Gets the parent chart component that owns this border configuration.
        /// </summary>
        /// <value>
        /// The <see cref="SfChart"/> instance, or <c>null</c> if not initialized within a chart context.
        /// </value>
        [CascadingParameter]
        internal SfChart? Owner { get; set; }

        /// <summary>
        /// Gets or sets the color of the border, accepting values in hexadecimal or RGBA formats as valid CSS color strings.
        /// </summary>
        /// <value>
        /// A string that specifies the color of the chart border. The value must be a valid CSS color string, formatted as either a hexadecimal or RGBA value.
        /// </value>
        /// <remarks>
        /// Changing this property will update the visual boundary of the chart's border to match the specified color, enhancing the stylistic appearance or aligning with specific design standards.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set all the borders used in the chart control.
        /// <SfChart>
        ///     <ChartBorder Color="blue" Width="1"></ChartBorder>
        ///     <ChartArea>
        ///         <ChartAreaBorder Width="2" Color="green"></ChartAreaBorder>
        ///     </ChartArea>
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category">
        ///         <ChartAxisCrosshairTooltip Enable="true"></ChartAxisCrosshairTooltip>
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis>
        ///         <ChartStriplines>
        ///             <ChartStripline Start="20" End="25" Color="red">
        ///                 <ChartStriplineBorder Color="#4b107e" Width="2"></ChartStriplineBorder>
        ///             </ChartStripline>
        ///             <ChartStripline Start="32" End="35" Color="blue">
        ///                 <ChartStriplineBorder Color="#d41986" Width="2"></ChartStriplineBorder>
        ///             </ChartStripline>
        ///         </ChartStriplines>
        ///     </ChartPrimaryYAxis>
        ///     <ChartCrosshairSettings Enable="true">
        ///         <ChartCrosshairLine Width="2" Color="green"></ChartCrosshairLine>
        ///     </ChartCrosshairSettings>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" Type="ChartSeriesType.Column">
        ///             <ChartEmptyPointSettings Mode="EmptyPointMode.Average">
        ///                 <ChartEmptyPointBorder Width="1" Color="brown"></ChartEmptyPointBorder>
        ///             </ChartEmptyPointSettings>
        ///             <ChartSeriesBorder Width="1" Color="black"></ChartSeriesBorder>
        ///             <ChartMarker Visible="true">
        ///                 <ChartMarkerBorder Width="2" Color="red"></ChartMarkerBorder>
        ///                 <ChartDataLabel Visible="true">
        ///                     <ChartDataLabelBorder Width="2" Color="red"></ChartDataLabelBorder>
        ///                 </ChartDataLabel>
        ///             </ChartMarker>
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        ///     <ChartLegendSettings Visible="true">
        ///         <ChartLegendBorder Color="red" Width="2" />
        ///      </ChartLegendSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public virtual string? Color { get; set; }

        /// <summary>
        /// Gets or sets the width of the border in pixels.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> specifying the border's width. The default value is <b>1</b>, representing 1 pixel.
        /// </value>
        /// <remarks>
        /// Adjust this property to increase or decrease the thickness of the chart border, which can help draw focus or emphasize the chart's outline.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set all the borders used in the chart control.
        /// <SfChart>
        ///     <ChartBorder Color="blue" Width="1"></ChartBorder>
        ///     <ChartArea>
        ///         <ChartAreaBorder Width="2" Color="green"></ChartAreaBorder>
        ///     </ChartArea>
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category">
        ///         <ChartAxisCrosshairTooltip Enable="true"></ChartAxisCrosshairTooltip>
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis>
        ///         <ChartStriplines>
        ///             <ChartStripline Start="20" End="25" Color="red">
        ///                 <ChartStriplineBorder Color="#4b107e" Width="2"></ChartStriplineBorder>
        ///             </ChartStripline>
        ///             <ChartStripline Start="32" End="35" Color="blue">
        ///                 <ChartStriplineBorder Color="#d41986" Width="2"></ChartStriplineBorder>
        ///             </ChartStripline>
        ///         </ChartStriplines>
        ///     </ChartPrimaryYAxis>
        ///     <ChartCrosshairSettings Enable="true">
        ///         <ChartCrosshairLine Width="2" Color="green"></ChartCrosshairLine>
        ///     </ChartCrosshairSettings>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" Type="ChartSeriesType.Column">
        ///             <ChartEmptyPointSettings Mode="EmptyPointMode.Average">
        ///                 <ChartEmptyPointBorder Width="1" Color="brown"></ChartEmptyPointBorder>
        ///             </ChartEmptyPointSettings>
        ///             <ChartSeriesBorder Width="1" Color="black"></ChartSeriesBorder>
        ///             <ChartMarker Visible="true">
        ///                 <ChartMarkerBorder Width="2" Color="red"></ChartMarkerBorder>
        ///                 <ChartDataLabel Visible="true">
        ///                     <ChartDataLabelBorder Width="2" Color="red"></ChartDataLabelBorder>
        ///                 </ChartDataLabel>
        ///             </ChartMarker>
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        ///     <ChartLegendSettings Visible="true">
        ///         <ChartLegendBorder Color="red" Width="2" />
        ///      </ChartLegendSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public virtual double Width { get; set; } = 1;
        #endregion


        #region Lifecycle methods
        /// <summary>
        /// Handles parameter changes and processes property updates.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (_color != Color || _width != Width)
            {
                _color = Color;
                _width = Width;
                _isPropertyChanged = true;
            }
        }
        #endregion
    }
}