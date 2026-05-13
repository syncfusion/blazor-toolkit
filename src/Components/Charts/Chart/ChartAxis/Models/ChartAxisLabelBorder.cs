using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides options for customizing the border of axis labels, including color, type, and width.
    /// </summary>
    /// <remarks>
    /// This component is a cascading parameter that should be used within a <see cref="ChartAxis"/> component.
    /// It allows fine-grained control over the visual appearance of axis label borders.
    /// </remarks>
    public class ChartAxisLabelBorder : ChartSubComponent
    {
        #region Properties
        /// <summary>
        /// Gets or sets the parent <see cref="ChartAxis"/> component that owns this border configuration.
        /// </summary>
        /// <value>
        /// A reference to the parent <see cref="ChartAxis"/> component, or <see langword="null"/> if not set.
        /// </value>
        [CascadingParameter]
        private ChartAxis? Axis { get; set; }

        /// <summary> 
        /// Gets or sets the color of the axis label border. 
        /// </summary> 
        /// <value> 
        /// A string representing the color of the axis label border. The default axis border color is determined by the chart's theme. By default, the theme is set to Fluent with an axis border color of <b>#b5b5b5</b>.
        /// </value> 
        /// <remarks>
        /// Use valid hex or rgba CSS color strings for the color value. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a customized color for the axis label.
        /// <SfChart>
        ///     <ChartPrimaryXAxis>
        ///         <ChartAxisLabelBorder Color="blue" Width="2" />
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis>
        ///         <ChartAxisLabelBorder Color="blue" Width="2" />
        ///     </ChartPrimaryYAxis>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string Color { get; set; } = string.Empty;

        /// <summary>  
        /// Gets or sets the border type for axis labels.  
        /// </summary>  
        /// <value>  
        /// One of the <see cref="BorderType"/> enumerations that specifies the border type for axis labels.  
        /// The options include:  
        ///   - <c>Rectangle</c>   
        ///   - <c>WithoutTopBorder</c>  
        ///   - <c>WithoutTopandBottomBorder</c>  
        ///   - <c>WithoutBorder</c>  
        ///   - <c>Brace</c>  
        ///   - <c>CurlyBrace</c>  
        /// The default value is <b>BorderType.Rectangle</b>.  
        /// </value>  
        /// <remarks>  
        /// The <see cref="Type"/> property determines the style of the border around axis labels.  
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a customized border type for the axis label.
        /// <SfChart>
        ///     <ChartPrimaryXAxis>
        ///         <ChartAxisLabelBorder Color="blue" Width="2" Type="BorderType.WithoutTopBorder" />
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis>
        ///         <ChartAxisLabelBorder Color="blue" Width="2" Type="BorderType.WithoutTopandBottomBorder" />
        ///     </ChartPrimaryYAxis>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public BorderType Type { get; set; }

        /// <summary>  
        /// Gets or sets the width of the border around axis labels. 
        /// </summary> 
        /// <value>  
        /// The double value representing the width of axis labels border. The default value is <b>0</b>.  
        /// </value>
        /// <remarks> 
        /// Accepts the values in numerical forms to specify the border width.
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a customized border width for the axis label.
        /// <SfChart>
        ///     <ChartPrimaryXAxis>
        ///         <ChartAxisLabelBorder Color="blue" Width="2" />
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis>
        ///         <ChartAxisLabelBorder Color="blue" Width="2" />
        ///     </ChartPrimaryYAxis>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double Width { get; set; }

        #endregion

        #region Lifecycle methods

        /// <summary>
        /// Performs component initialization and registers this border configuration with the parent axis.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Tracker is ChartAxis axis)
            {
                Axis = axis;
            }
            Axis?.UpdateAxisProperties("Border", this);
        }
        #endregion
    }
}
