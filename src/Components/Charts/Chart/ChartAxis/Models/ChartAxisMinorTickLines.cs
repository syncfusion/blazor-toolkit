using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Represents the options to customize the minor tick lines of a chart axis in the Syncfusion Blazor chart.
    /// </summary>
    public class ChartAxisMinorTickLines : ChartSubComponent
    {
        #region Properties

        /// <summary>
        /// Gets or sets the parent <see cref="ChartAxis"/> component that owns this minor tick line configuration.
        /// </summary>
        /// <value>
        /// A reference to the parent <see cref="ChartAxis"/> component, or <see langword="null"/> if not set.
        /// </value>
        [CascadingParameter]
        private ChartAxis? Axis { get; set; }

        /// <summary> 
        /// Gets or sets the color of the minor tick line. 
        /// </summary> 
        /// <value> 
        /// A string representing the color of the minor tick line. The default minor tick line color is determined by the chart's theme. By default, the theme is set to Fluent with a minor tick line color of <b>#d6d6d6</b>. 
        /// </value> 
        /// <remarks> 
        /// Use valid hex or rgba CSS color strings for the color value. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a custom style for the axis minor tick lines.
        /// <SfChart>
        ///     <ChartPrimaryXAxis MinorTicksPerInterval="1">
        ///         <ChartAxisMinorTickLines Color="red" />
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis MinorTicksPerInterval="1">
        ///         <ChartAxisMinorTickLines Color="blue" />
        ///     </ChartPrimaryYAxis>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string Color { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets the height of the minor tick line in pixels. 
        /// </summary> 
        /// <value> 
        /// The double value representing the height of the minor tick line in pixels. 
        /// The default value is <b>5</b>. 
        /// </value>
        /// <remarks>
        /// The height specifies the length of the tick line extending from the axis line.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a customize height for the axis minor tick lines.
        /// <SfChart>
        ///     <ChartPrimaryXAxis MinorTicksPerInterval="1">
        ///         <ChartAxisMinorTickLines Height="10" />
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis MinorTicksPerInterval="1">
        ///         <ChartAxisMinorTickLines Height="10" />
        ///     </ChartPrimaryYAxis>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double Height { get; set; } = 5;

        /// <summary> 
        /// Gets or sets the width of the minor tick line in pixels. 
        /// </summary> 
        /// <value> 
        /// The double value representing the width of the minor tick line in pixels. 
        /// The default value is <b>0.7</b>. 
        /// </value>
        /// <remarks>
        /// The width determines the thickness of the tick line on the chart.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a customize width for the axis minor tick lines.
        /// <SfChart>
        ///     <ChartPrimaryXAxis MinorTicksPerInterval="1">
        ///         <ChartAxisMinorTickLines Width="3" />
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis MinorTicksPerInterval="1">
        ///         <ChartAxisMinorTickLines Width="3" />
        ///     </ChartPrimaryYAxis>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double Width { get; set; } = 0.7;

        #endregion

        #region Lifecycle Methods

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
            Axis?.UpdateAxisProperties("MinorTickLines", this);
        }
        #endregion
    }
}
