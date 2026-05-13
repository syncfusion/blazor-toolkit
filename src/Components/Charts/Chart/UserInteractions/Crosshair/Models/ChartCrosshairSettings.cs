using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides options to customize the crosshair for charts.
    /// </summary>
    /// <remarks>
    /// Configure general crosshair behavior and appearance. For axis-specific tooltip customization, use
    /// <see cref="ChartAxisCrosshairTooltip"/> within the respective axis.
    /// </remarks>
    public class ChartCrosshairSettings : ChartSubComponent
    {
        #region Properties

        [CascadingParameter]
        internal SfChart? Chart { get; set; }

        /// <summary> 
        /// Gets or sets the dash pattern for the crosshair line. 
        /// </summary> 
        /// <value> 
        /// A string defining the dash array (e.g., <c>"5,3"</c>). The default value is an empty string. 
        /// </value> 
        /// <remarks> 
        /// Customize the visual style of the crosshair line using SVG dash patterns. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartCrosshairSettings Enable="true" DashArray="5,3" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string DashArray { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets a value indicating whether the crosshair is enabled. 
        /// </summary> 
        /// <value> 
        /// <b>true</b> to enable the crosshair; otherwise, <b>false</b>. The default value is <b>false</b>. 
        /// </value> 
        /// <remarks>
        /// When enabled, crosshair lines appear for precise inspection of data values.
        /// </remarks>
        [Parameter]
        public bool Enable { get; set; }

        /// <summary> 
        /// Gets or sets the crosshair line customization. 
        /// </summary> 
        /// <value> 
        /// An instance of <see cref="ChartCrosshairLine"/> that configures color and width. 
        /// </value> 
        /// <remarks> 
        /// Use this to override the default line styling.
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartCrosshairSettings Enable="true">
        ///     <ChartCrosshairLine Width="2" Color="blue"></ChartCrosshairLine>
        /// </ChartCrosshairSettings>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ChartCrosshairLine Line { get; set; } = new ChartCrosshairLine();

        /// <summary>
        /// Gets or sets a value indicating whether the horizontal crosshair snaps to the nearest data point.
        /// </summary>
        /// <value>
        /// <c>true</c> to snap to the nearest data point; otherwise, <c>false</c>. Default is <c>false</c>.
        /// </value>
        /// <remarks>
        /// Improves precision when hovering near data points on horizontal charts.
        /// </remarks>
        [Parameter]
        public bool SnapToData { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entire category range is highlighted on hover.
        /// </summary>
        /// <value>
        /// <c>true</c> to highlight the full category span; otherwise, <c>false</c>. Default is <c>false</c>.
        /// </value>
        /// <remarks>
        /// Applicable only to category axes.
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartCrosshairSettings Enable="true" HighlightCategory="true" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool HighlightCategory { get; set; }

        /// <summary> 
        /// Gets or sets the type of crosshair line to display. 
        /// </summary> 
        /// <value> 
        /// A <see cref="LineType"/> value:
        /// <list type="bullet">
        /// <item><description><c>None</c>: Hides both vertical and horizontal lines.</description></item>
        /// <item><description><c>Both</c>: Shows both vertical and horizontal lines.</description></item>
        /// <item><description><c>Vertical</c>: Shows only the vertical line.</description></item>
        /// <item><description><c>Horizontal</c>: Shows only the horizontal line.</description></item>
        /// </list>
        /// Default is <b>LineType.Both</b>. 
        /// </value> 
        /// <remarks>
        /// Determines which crosshair lines are rendered.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartCrosshairSettings Enable="true" LineType="LineType.Vertical" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LineType LineType { get; set; } = LineType.Both;

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Initializes the component and attaches it to the owning chart.
        /// </summary>
        /// <remarks>Registers this instance on the chart so that crosshair updates are applied centrally.</remarks>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Chart is null)
            {
                return;
            }
            Chart._crosshair = this;
        }

        /// <summary>
        /// Applies parameter updates and pushes crosshair options to the chart when ready.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(false);
            await ApplyCrosshairUpdateAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Disposes the component and releases references.
        /// </summary>
        /// <remarks>Clears references to avoid memory retention after component disposal.</remarks>
        protected override ValueTask DisposeAsyncCore()
        {
            Chart = null;
            ChildContent = null!;
            return base.DisposeAsyncCore();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Applies updates to the chart's crosshair/tooltip options when the chart is ready.
        /// </summary>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
        private ValueTask ApplyCrosshairUpdateAsync()
        {
            if (Chart is not null && Chart._isChartFirstRender)
            {
                Task task = Chart.SetTooltipCrosshairOptionsAsync();
                return task.IsCompletedSuccessfully ? ValueTask.CompletedTask : new ValueTask(task);
            }

            return ValueTask.CompletedTask;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Updates crosshair sub-properties pushed from child components.
        /// </summary>
        /// <param name="key">The property key to update (e.g., <c>"Line"</c>).</param>
        /// <param name="keyValue">The value associated with the key.</param>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
        internal ValueTask UpdateCrosshairPropertiesAsync(string key, object keyValue)
        {
            if (key == nameof(Line))
            {
                Line = (ChartCrosshairLine)keyValue;
                return ApplyCrosshairUpdateAsync();
            }

            return ValueTask.CompletedTask;
        }

        #endregion
    }
}
