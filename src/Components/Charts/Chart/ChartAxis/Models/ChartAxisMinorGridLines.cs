using Microsoft.AspNetCore.Components;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Represents the options to customize the minor grid lines of a chart axis in the Syncfusion Blazor chart.
    /// </summary>
    public class ChartAxisMinorGridLines : ChartSubComponent
    {
        #region Properties

        /// <summary>
        /// Gets or sets the parent <see cref="ChartAxis"/> component that owns this minor grid line configuration.
        /// </summary>
        /// <value>
        /// A reference to the parent <see cref="ChartAxis"/> component, or <see langword="null"/> if not set.
        /// </value>
        [CascadingParameter]
        private ChartAxis? Axis { get; set; }

        /// <summary> 
        /// Gets or sets the color of the minor grid line. 
        /// </summary> 
        /// <value> 
        /// A string representing the color of the minor grid line. The default minor grid line color is determined by the chart's theme. By default, the theme is set to Fluent with a minor grid line color of <b>#eaeaea</b>.
        /// </value> 
        /// <remarks> 
        /// Use valid hex or rgba CSS color strings for the color value. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a customized color for the axis minor grid lines.
        /// <SfChart>
        ///     <ChartPrimaryXAxis MinorTicksPerInterval="1">
        ///         <ChartAxisMinorGridLines Color="red" />
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis MinorTicksPerInterval="1">
        ///         <ChartAxisMinorGridLines Color="blue" />
        ///     </ChartPrimaryYAxis>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string Color { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets the dash array of the minor grid line. 
        /// </summary> 
        /// <value> 
        /// A string representing the dash array of the minor grid line. 
        /// The default value is an empty string. 
        /// </value>
        /// <remarks>
        /// The dash array specifies a pattern of dashes and gaps used for the grid line, using a series of numbers that define lengths of dashes and gaps.
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a customized dash array  for the axis minor grid lines.
        /// <SfChart>
        ///     <ChartPrimaryXAxis MinorTicksPerInterval="1">
        ///         <ChartAxisMinorGridLines DashArray="5,1" />
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis MinorTicksPerInterval="1">
        ///         <ChartAxisMinorGridLines DashArray="5,1" />
        ///     </ChartPrimaryYAxis>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string DashArray { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets the width of the minor grid line in pixels. 
        /// </summary> 
        /// <value> 
        /// The double value representing the width of the minor grid line in pixels. 
        /// The default value is <b>0.7</b>. 
        /// </value>
        /// <remarks>
        /// The width sets the thickness of the grid line on the chart.
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a customize width for the axis minor grid lines.
        /// <SfChart>
        ///     <ChartPrimaryXAxis MinorTicksPerInterval="1">
        ///         <ChartAxisMinorGridLines Width="3" />
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis MinorTicksPerInterval="1">
        ///         <ChartAxisMinorGridLines Width="3" />
        ///     </ChartPrimaryYAxis>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double Width { get; set; } = 0.7;

        #endregion

        #region Lifecycle methods

        /// <summary>
        /// Performs component initialization by registering with the parent <see cref="ChartAxis"/> instance.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Tracker is ChartAxis chartAxis)
            {
                Axis = chartAxis;
            }
            Axis?.UpdateAxisProperties("MinorGridLines", this);
        }
        #endregion
    }
}
