using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Represents the major grid lines for a chart axis, enabling customization of color, width, and dash patterns.
    /// </summary>
    public class ChartAxisMajorGridLines : ChartSubComponent
    {
        #region Fields
        private string _color = null!;
        private double _width = 1;
        private string _dashArray = string.Empty;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the parent <see cref="ChartAxis"/> component that owns this grid line configuration.
        /// </summary>
        /// <value>
        /// A reference to the parent <see cref="ChartAxis"/> component, or <see langword="null"/> if not set.
        /// </value>
        [CascadingParameter]
        private ChartAxis? Axis { get; set; }

        /// <summary> 
        /// Gets or sets the color of the major grid line. 
        /// </summary> 
        /// <value> 
        /// A string representing the color of the major grid line. The default major grid line color is determined by the chart's theme. By default, the theme is set to Fluent with a major grid line color of <b>#dbdbdb</b>. 
        /// </value> 
        /// <remarks> 
        /// Use valid hex or rgba CSS color strings for the color value. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a custom color for the axis major gridlines.
        /// <SfChart>
        ///     <ChartPrimaryXAxis>
        ///         <ChartAxisMajorGridLines Color="red"  />
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis>
        ///         <ChartAxisMajorGridLines Color="blue"  />
        ///     </ChartPrimaryYAxis>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string Color { get; set; } = null!;

        /// <summary> 
        /// Gets or sets the dash array of the major grid line. 
        /// </summary> 
        /// <value> 
        /// A string representing the dash array of the major grid line. 
        /// The default value is an empty string. 
        /// </value>
        /// <remarks>
        /// The dash array is used to create dashed lines, specified by a series of numbers that define the lengths of the dashes and gaps.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a custom dash array for the axis major gridlines.
        /// <SfChart>
        ///     <ChartPrimaryXAxis>
        ///         <ChartAxisMajorGridLines DashArray="5,1" />
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis>
        ///         <ChartAxisMajorGridLines DashArray="5,1" />
        ///     </ChartPrimaryYAxis>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string DashArray { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets the width of the major grid line in pixels. 
        /// </summary> 
        /// <value> 
        /// The double value representing the width of the major grid line in pixels. 
        /// The default value is <b>1</b>. 
        /// </value>
        /// <remarks>
        /// The width determines the thickness of the grid line on the chart.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a width thickness for the axis major gridlines.
        /// <SfChart>
        ///     <ChartPrimaryXAxis>
        ///         <ChartAxisMajorGridLines Width="3" />
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis>
        ///         <ChartAxisMajorGridLines Width="3" />
        ///     </ChartPrimaryYAxis>
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
            Axis?.UpdateAxisProperties("MajorGridLines", this);
            _color = Color;
            _dashArray = DashArray;
            _width = Width;
        }

        /// <summary>
        /// Responds to parameter changes and notifies the parent axis when properties have been modified.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (_color != Color || _dashArray != DashArray || _width != Width)
            {
                _color = Color;
                _dashArray = DashArray;
                _width = Width;
                Axis?.UpdateAxisProperties("MajorGridLines", this);
            }
        }
        #endregion
    }
}
