using Microsoft.AspNetCore.Components;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides options to customize the margins of a chart legend.
    /// </summary>
    public class ChartLegendMargin : ChartDefaultMargin
    {
        #region Properties

        /// <summary>
        /// Gets or sets the chart component that owns this margin configuration.
        /// </summary>
        /// <value>
        /// The parent <see cref="SfChart"/> component. This is automatically set via cascading parameters.
        /// </value>
        [CascadingParameter]
        internal SfChart? Owner { get; set; }

        /// <summary>
        /// Gets or sets the legend settings that contain this margin configuration.
        /// </summary>
        /// <value>
        /// The <see cref="ChartLegendSettings"/> that manages this margin configuration.
        /// </value>
        internal ChartLegendSettings? ChartLegend { get; set; }

        /// <summary> 
        /// Gets or sets the left margin for the legend. 
        /// </summary> 
        /// <value> 
        /// The double value representing the left margin for the legend. The default value is <b>0</b>.
        /// </value>
        /// <remarks>
        /// Adjust this property to provide space on the left side of the chart legend.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to customize the chart's surrounding left side of the legend space.
        /// <SfChart>
        ///     <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" />
        ///     <ChartLegendSettings Visible="true">
        ///         <ChartLegendBorder Color="blue" Width="2" />
        ///         <ChartLegendMargin Left="30" />
        ///     </ChartLegendSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override double Left { get; set; }

        /// <summary> 
        /// Gets or sets the right margin for the legend. 
        /// </summary> 
        /// <value> 
        /// The double value representing the right margin for the legend. The default value is <b>0</b>.
        /// </value> 
        /// <remarks>
        /// Modify this property to provide space on the right side of the chart legend.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to customize the chart's surrounding right side of the legend space.
        /// <SfChart>
        ///     <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" />
        ///     <ChartLegendSettings Visible="true">
        ///         <ChartLegendBorder Color="blue" Width="2" />
        ///         <ChartLegendMargin Right="30" />
        ///     </ChartLegendSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override double Right { get; set; }

        /// <summary> 
        /// Gets or sets the top margin for the legend. 
        /// </summary> 
        /// <value> 
        /// The double value representing the top margin for the legend. The default value is <b>0</b>.
        /// </value> 
        /// <remarks>
        /// Use this property to define space above the chart legend.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to customize the chart's surrounding top side of the legend space.
        /// <SfChart>
        ///     <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" />
        ///     <ChartLegendSettings Visible="true">
        ///         <ChartLegendBorder Color="blue" Width="2" />
        ///         <ChartLegendMargin Top="30" />
        ///     </ChartLegendSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override double Top { get; set; }

        /// <summary> 
        /// Gets or sets the bottom margin for the legend. 
        /// </summary> 
        /// <value> 
        /// The double value representing the bottom margin for the legend. The default value is <b>0</b>.
        /// </value> 
        /// <remarks>
        /// Adjust this property to provide space below the chart legend.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to customize the chart's surrounding bottom side of the legend space.
        /// <SfChart>
        ///     <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" />
        ///     <ChartLegendSettings Visible="true">
        ///         <ChartLegendBorder Color="blue" Width="2" />
        ///         <ChartLegendMargin Bottom="30" />
        ///     </ChartLegendSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override double Bottom { get; set; }

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Executes during component initialization.
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
            ChartLegend?.UpdateLegendProperties("Margin", this);
        }

        /// <summary>
        /// Executes when parameters are set or updated.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            Owner?._legendRenderer?.ProcessRenderQueue();
        }
        #endregion
    }
}
