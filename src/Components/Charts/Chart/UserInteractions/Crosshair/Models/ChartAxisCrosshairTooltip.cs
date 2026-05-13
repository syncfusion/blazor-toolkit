using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides options to customize the axis crosshair tooltip.
    /// </summary>
    /// <remarks>
    /// Use this component inside an axis (e.g., <c>ChartPrimaryXAxis</c>) to enable and style the axis-specific crosshair tooltip.
    /// The tooltip is shown when crosshair is enabled on the chart.
    /// </remarks>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// // This example demonstrates enabling crosshair and displaying a tooltip for the X-axis in a Chart.
    /// <SfChart>
    ///     <ChartPrimaryXAxis>
    ///         <ChartAxisCrosshairTooltip Enable="true"></ChartAxisCrosshairTooltip>
    ///     </ChartPrimaryXAxis>
    ///     <ChartCrosshairSettings Enable="true" />
    ///     <ChartSeriesCollection>
    ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue">
    ///         </ChartSeries>
    ///     </ChartSeriesCollection>
    /// </SfChart>
    /// ]]>
    /// </code>
    /// </example>
    public class ChartAxisCrosshairTooltip : ChartSubComponent
    {

        #region Properties

        [CascadingParameter]
        private ChartAxis? Axis { get; set; }

        /// <summary> 
        /// Gets or sets a value indicating whether the axis crosshair tooltip is enabled. 
        /// </summary> 
        /// <value> 
        /// <b>true</b> if enabled; otherwise, <b>false</b>. Default is <b>false</b>. 
        /// </value> 
        /// <remarks>
        /// Set this to <c>true</c> to show an axis-aligned tooltip when the chart crosshair is active.
        /// </remarks>
        [Parameter]
        public bool Enable { get; set; }

        /// <summary> 
        /// Gets or sets the fill color of the tooltip background. 
        /// </summary> 
        /// <value> 
        /// A CSS color string (hex, rgba, or named). The default is theme-dependent.
        /// </value> 
        /// <remarks> 
        /// Accepts any valid CSS color (e.g., <c>#ff0000</c>, <c>rgba(255,0,0,0.5)</c>). 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartPrimaryXAxis>
        ///     <ChartAxisCrosshairTooltip Enable="true" Fill="red"></ChartAxisCrosshairTooltip>
        /// </ChartPrimaryXAxis>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string Fill { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets an instance of <see cref="ChartCrosshairTextStyle"/> that customizes the text style of the axis crosshair tooltip. 
        /// </summary> 
        /// <value> 
        /// An instance of <see cref="ChartCrosshairTextStyle"/>. 
        /// </value> 
        /// <remarks> 
        /// Configure text color, size, weight, and family for the axis crosshair tooltip.
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartPrimaryXAxis>
        ///     <ChartAxisCrosshairTooltip Enable="true">
        ///         <ChartCrosshairTextStyle Color="blue" Size="15px" />
        ///     </ChartAxisCrosshairTooltip>
        /// </ChartPrimaryXAxis>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ChartCrosshairTextStyle TextStyle { get; set; } = new();

        #endregion

        #region Lifecycle Methods

        /// <exclude />
        /// <summary>
        /// Initializes the component and registers it with the owning axis.
        /// </summary>
        /// <remarks>
        /// Resolves the cascading <see cref="ChartAxis"/> tracker and updates the axis properties to include this crosshair tooltip.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Tracker is ChartAxis chartAxis)
            {
                Axis = chartAxis;
            }

            Axis?.UpdateAxisProperties("CrosshairTooltip", this);
        }

        /// <summary>
        /// Disposes the component and releases references.
        /// </summary>
        /// <remarks>Clears references to avoid memory retention after component disposal.</remarks>
        protected override ValueTask DisposeAsyncCore()
        {
            Axis = null;
            ChildContent = null!;
            TextStyle = null!;
            return base.DisposeAsyncCore();
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Updates child properties pushed from nested content.
        /// </summary>
        /// <param name="key">The property key to update (e.g., <c>"TextStyle"</c>).</param>
        /// <param name="item">The value associated with the key.</param>
        internal void UpdateChildProperties(string key, object item)
        {
            if (key == nameof(TextStyle))
            {
                TextStyle = (ChartCrosshairTextStyle)item;
            }
        }

        #endregion
    }
}