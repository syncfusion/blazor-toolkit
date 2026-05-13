using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides options to customize the chart area border.
    /// </summary>
    public class ChartAreaBorder : ChartDefaultBorder
    {
        #region Properties

        /// <summary> 
        /// Gets or sets the width of the border surrounding the chart area.
        /// </summary> 
        /// <value> 
        /// A double value representing the width of the chart area border, measured in pixels. The default value is <b>0.5</b> pixel, providing a subtle border appearance. 
        /// </value> 
        /// <remarks> 
        /// This property accepts numerical values to precisely control the border width.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the border width for the chart area in the chart control:
        /// <SfChart>
        ///     <ChartArea>
        ///         <ChartAreaBorder Color="blue" Width="2" />
        ///     </ChartArea>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override double Width { get; set; } = 0.5;

        /// <summary>
        /// Gets or sets the associated chart area component.
        /// </summary>
        /// <value>
        /// A <see cref="ChartArea"/> instance, or <see langword="null"/> if not set.
        /// </value>
        internal ChartArea? ChartArea { get; set; }
        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Performs component initialization and associates this border with its parent chart area.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Tracker is ChartArea chartArea)
            {
                ChartArea = chartArea;
            }
            ChartArea?.UpdateBorder(this);
        }

        /// <summary>
        /// Handles parameter changes and triggers a render update for the chart area.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (Owner?._chartAreaRenderer is not null)
            {
                Owner._chartAreaRenderer.RendererShouldRender = true;
                Owner._chartAreaRenderer.ProcessRenderQueue();
            }
        }
        #endregion
    }
}
