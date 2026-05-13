using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides options to customize the border of the chart.
    /// </summary>
    public class ChartBorder : ChartDefaultBorder
    {
        #region Properties

        /// <summary> 
        /// Gets or sets the width of the chart border in pixels. 
        /// </summary> 
        /// <value> 
        /// A double value representing the width of the chart border. The default value is <b>0</b> pixel. 
        /// </value> 
        /// <remarks> 
        /// Accepts numerical values to define the thickness of the chart's border. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the border thickness for the chart:
        /// <SfChart>
        ///     <ChartBorder Width="2" Color="green" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override double Width { get; set; }
        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Performs component initialization and registers this border instance with the chart renderer.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Owner?._chartBorderRenderer is not null)
            {
                Owner._chartBorderRenderer.ChartBorder = this;
            }
        }

        /// <summary>
        /// Handles parameter changes and updates the renderer when border properties are modified.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (Owner?._chartBorderRenderer is not null)
            {
                Owner._chartBorderRenderer.RendererShouldRender = true;
                Owner._chartBorderRenderer.ProcessRenderQueue();
            }
            if (_isPropertyChanged)
            {
                _isPropertyChanged = false;
                Owner?.OnLayoutChange();
            }
        }

        /// <summary>
        /// Disposes resources and clears component references.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override ValueTask DisposeAsyncCore()
        {
            Owner = null!;
            ChildContent = null!;
            return base.DisposeAsyncCore();
        }
        #endregion
    }
}