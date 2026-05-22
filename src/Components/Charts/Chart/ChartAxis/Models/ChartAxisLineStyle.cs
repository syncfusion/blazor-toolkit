using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Represents the style settings for a chart axis line, enabling customization of color, width, and dash patterns.
    /// </summary>
    public class ChartAxisLineStyle : ChartSubComponent
    {
        #region Properties

        /// <summary>
        /// Gets or sets the parent <see cref="ChartAxis"/> component that owns this axis line style configuration.
        /// </summary>
        /// <value>
        /// A reference to the parent <see cref="ChartAxis"/> component, or <see langword="null"/> if not set.
        /// </value>
        [CascadingParameter]
        private ChartAxis? Axis { get; set; }

        /// <summary> 
        /// Gets or sets the color of the axis line. 
        /// </summary> 
        /// <value> 
        /// A string representing the color of the axis line. The default value is <b>#b5b5b5</b>. 
        /// </value> 
        /// <remarks> 
        /// Use valid hex or rgba CSS color strings for the color value to ensure the correct presentation of the axis line in charts. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a customize color for the axis line.
        /// <SfChart>
        ///     <ChartPrimaryXAxis>
        ///         <ChartAxisLineStyle Width="3" Color="red" />
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis>
        ///         <ChartAxisLineStyle Width="3" Color="red" />
        ///     </ChartPrimaryYAxis>
        ///     <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string Color { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets the dash array of the axis line. 
        /// </summary> 
        /// <value> 
        /// A string representing the dash array of the axis line. The default value is an empty string. 
        /// </value>
        /// <remarks>
        /// This property allows customization of the axis line's stroke pattern.
        /// Use a series of numbers to define the dash and gap lengths (e.g., "5,2,1" for a pattern of 5px dash, 2px gap, 1px dash).
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a customize color for the axis line.
        /// <SfChart>
        ///     <ChartPrimaryXAxis>
        ///         <ChartAxisLineStyle Width="3" DashArray="5,1" />
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis>
        ///         <ChartAxisLineStyle Width="3" DashArray="5,1" />
        ///     </ChartPrimaryYAxis>
        ///     <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string DashArray { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets the width of the axis line in pixels. 
        /// </summary> 
        /// <value> 
        /// The double value representing the width of the axis line in pixels. The default value is <b>1</b>. 
        /// </value>
        /// <remarks>
        /// This property specifies the thickness of the axis line, allowing for visual emphasis as necessary.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a customize width for the axis line.
        /// <SfChart>
        ///     <ChartPrimaryXAxis>
        ///         <ChartAxisLineStyle Width="3" />
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis>
        ///         <ChartAxisLineStyle Width="3" />
        ///     </ChartPrimaryYAxis>
        ///     <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double Width { get; set; } = 1;

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
            Axis?.UpdateAxisProperties("LineStyle", this);
        }
        #endregion
    }
}
