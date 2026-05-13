using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides options to customize the chart area.
    /// </summary>
    public class ChartArea : ChartSubComponent
    {
        #region Fields
        private ChartAreaBorder PrevBorder { get; set; } = new ChartAreaBorder();
        #endregion

        #region Properties

        /// <summary> 
        /// Gets or sets the background of the chart area. 
        /// </summary> 
        /// <value> 
        /// A string representing the background color of the chart area. The default value is <b>transparent</b>. 
        /// </value> 
        /// <remarks> 
        /// Accepts values in hex or rgba as valid CSS color string. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the background to the chart:
        /// <SfChart>
        ///     <ChartArea Background="red" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string Background { get; set; } = Constants.Transparent;

        /// <summary> 
        /// Gets or sets the background image location or URL of the chart area. 
        /// </summary> 
        /// <value> 
        /// A string representing the background image location or URL of the chart area. The default value is <b>null</b>. 
        /// </value> 
        /// <remarks> 
        /// This property accepts a value as a URL link or location of an image.
        /// In this example, the image (wheat.png) is stored in the wwwroot folder and referenced using a relative path. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the background image to the chart:
        /// <SfChart>
        ///     <ChartArea BackgroundImage="wheat.png" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string BackgroundImage { get; set; } = null!;

        /// <summary> 
        /// Gets or sets the width for the chart area. 
        /// </summary> 
        /// <value> 
        /// A string specifying the width for the chart area. The default value is <b>null</b>.  
        /// </value> 
        /// <remarks> 
        /// Accepts values in percentage or in pixels. 
        /// </remarks> 
        [Parameter]
        public string Width { get; set; } = null!;

        /// <summary> 
        /// Gets or sets the opacity for the chart area background. 
        /// </summary> 
        /// <value> 
        /// A double value representing the opacity for the chart area background. The default value is <b>1</b>. 
        /// </value> 
        /// <remarks> 
        /// Accepts values in numerical form from 0 to 1.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the background opacity for a chart area:
        /// <SfChart>
        ///     <ChartArea Background="red" Opacity="0.5" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double Opacity { get; set; } = 1;

        /// <summary> 
        /// Gets or sets an instance of <see cref="ChartAreaBorder"/> which controls the customization of the chart area border color and width. 
        /// </summary> 
        /// <value> 
        /// An instance of <see cref="ChartAreaBorder"/>. The default value is a new instance of <see cref="ChartAreaBorder"/>.
        /// </value> 
        /// <remarks> 
        /// This property can be used to customize the color and width of the chart area border. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the border for the chart area in the chart control:
        /// <SfChart>
        ///     <ChartArea>
        ///         <ChartAreaBorder Width="2" Color="blue"></ChartAreaBorder>
        ///     </ChartArea>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ChartAreaBorder Border { get; set; } = new ChartAreaBorder();

        /// <summary>
        /// Gets the owner chart component.
        /// </summary>
        /// <value>
        /// The <see cref="SfChart"/> instance that owns this chart area, or <see langword="null"/> if not yet cascaded.
        /// </value>
        [CascadingParameter]
        internal SfChart? Owner { get; set; }
        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Performs component initialization and registers this chart area with the owner chart.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Owner?._chartAreaRenderer is not null)
            {
                Owner._chartAreaRenderer.Area = this;
            }
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
            if (!Equals(PrevBorder, Border))
            {
                PrevBorder = Border;
            }
            if (Owner?._chartAreaRenderer is not null)
            {
                Owner._chartAreaRenderer.RendererShouldRender = true;
                Owner._chartAreaRenderer.ProcessRenderQueue();
            }
        }

        /// <summary>
        /// Disposes resources associated with this component.
        /// </summary>
        protected override ValueTask DisposeAsyncCore()
        {
            Owner = null;
            ChildContent = null!;
            return base.DisposeAsyncCore();
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Updates the border configuration for this chart area.
        /// </summary>
        /// <param name="chartBorder">The new border configuration to apply.</param>
        internal void UpdateBorder(ChartAreaBorder chartBorder)
        {
            PrevBorder = chartBorder;
        }
        #endregion
    }
}
