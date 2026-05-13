using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides options for customizing the connector line style.
    /// </summary>
    public class ChartDefaultConnector : ChartSubComponent
    {
        /// <summary> 
        /// Gets or sets the color of the connector line. 
        /// </summary> 
        /// <value> 
        /// A string representing the color of the connector line. The default value is <b>"black"</b>. 
        /// </value> 
        /// <remarks> 
        /// Accepts values in hex or rgba as a valid CSS color string.
        /// It allows customization of the connector line to match the chart's theme or styling preferences.
        /// </remarks> 
        [Parameter]
        public virtual string Color { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets the width of the connector line. 
        /// </summary> 
        /// <value> 
        /// A double value representing the width of the connector line. The default value is <b>1</b>. 
        /// </value> 
        /// <remarks>
        /// Adjust this property to change the thickness of the connector line for better visibility or styling.
        /// </remarks>
        [Parameter]
        public virtual double Width { get; set; } = 1;

        /// <summary> 
        /// Gets or sets the dashArray for the connector line. 
        /// </summary> 
        /// <value> 
        /// A string representing the dashArray for the connector line. The default value is <b>null</b>. 
        /// </value> 
        /// <remarks> 
        /// The <see cref="DashArray"/> property allows customization of the connector line by specifying a dash pattern. 
        /// This string value defines the pattern of dashes and gaps in the line. For example, "4,2" represents a dash of length 4 followed by a gap of length 2. 
        /// </remarks> 
        [Parameter]
        public string DashArray { get; set; } = string.Empty;
    }
}
