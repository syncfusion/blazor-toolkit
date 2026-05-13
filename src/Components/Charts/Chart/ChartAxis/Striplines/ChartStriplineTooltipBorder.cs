using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary> 
    /// Provides options to customize the border of the stripline tooltip in the <see cref="SfChart"/> component. 
    /// </summary> 
    /// <remarks> 
    /// Use <see cref="ChartStriplineTooltipBorder"/> to configure the tooltip border appearance for a <see cref="ChartStripline"/>. 
    ///  When the width is <c>0</c> or the color is not specified, the tooltip may render without a visible border based on the active theme. 
    /// </remarks> 
    /// <example> 
    /// The following example demonstrates how to apply a custom border to a stripline tooltip. 
    /// <code><![CDATA[ 
    /// @using Syncfusion.Blazor.Toolkit.Charts 
    /// 
    /// <SfChart> 
    ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"> 
    ///         <ChartStriplines> 
    ///             <ChartStripline Start="20" End="25" Color="red"> 
    ///                 <ChartStriplineTooltip Enable="true"> 
    ///                     <ChartStriplineTooltipBorder Width="2" Color="#D32F2F" /> 
    ///                 </ChartStriplineTooltip> 
    ///             </ChartStripline> 
    ///         </ChartStriplines> 
    ///     </ChartPrimaryXAxis> 
    /// </SfChart> 
    /// ]]></code> 
    /// </example> 
    public class ChartStriplineTooltipBorder : ChartDefaultBorder
    {
        #region Properties

        /// <summary>
        /// Gets or sets the parent <see cref="ChartStriplineTooltip"/> component.
        /// </summary>
        /// <value>
        /// The cascading parent tooltip component, or <see langword="null"/> if not set.
        /// </value>
        [CascadingParameter]
        internal ChartStriplineTooltip? Parent { get; set; }

        /// <summary> 
        /// Gets or sets the border width for the stripline tooltip. 
        /// </summary> 
        /// <value> 
        /// A <see cref="double"/> representing the width of the tooltip border, in pixels. The default value is <c>0</c>. 
        /// </value> 
        /// <remarks> 
        /// Increase the value to emphasize the tooltip boundary. A value of <c>0</c> results in no visible border. 
        /// </remarks> 
        /// <example> 
        /// <code><![CDATA[ 
        /// <SfChart> 
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"> 
        ///         <ChartStriplines> 
        ///             <ChartStripline Start="20" End="25" Color="red"> 
        ///                 <ChartStriplineTooltip Enable="true"> 
        ///                     <ChartStriplineTooltipBorder Width="1" /> 
        ///                 </ChartStriplineTooltip> 
        ///             </ChartStripline> 
        ///         </ChartStriplines> 
        ///     </ChartPrimaryXAxis> 
        /// </SfChart> 
        /// ]]></code> 
        /// </example> 
        [Parameter]
        public override double Width { get; set; }

        /// <summary> 
        /// Gets or sets the border color for the stripline tooltip. 
        /// </summary> 
        /// <value> 
        /// A <see cref="string"/> representing the tooltip border color. The default value is <c>string.Empty</c>, which applies the active chart theme’s default border color. 
        /// </value> 
        /// <remarks> 
        /// Accepts valid CSS color values (hex, rgb/rgba, hsl/hsla, or named colors). 
        /// Set this property along with a non-zero <see cref="ChartDefaultBorder.Width"/> to render a visible border. 
        /// </remarks> 
        /// <example> 
        /// <code><![CDATA[ 
        /// <SfChart> 
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"> 
        ///         <ChartStriplines> 
        ///             <ChartStripline Start="20" End="25" Color="red"> 
        ///                 <ChartStriplineTooltip Enable="true"> 
        ///                     <ChartStriplineTooltipBorder Width="1" Color="#D32F2F" /> 
        ///                 </ChartStriplineTooltip> 
        ///             </ChartStripline> 
        ///         </ChartStriplines> 
        ///     </ChartPrimaryXAxis> 
        /// </SfChart> 
        /// ]]></code> 
        /// </example> 
        [Parameter]
        public override string? Color { get; set; } = string.Empty;

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Executes when the component is initialized. Registers this border configuration with the parent tooltip.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Tracker is ChartStriplineTooltip chartStriplineTooltip)
            {
                Parent = chartStriplineTooltip;
            }
            Parent?.UpdateTooltipProperties("Border", this);
        }

        /// <summary>
        /// Executes when component parameters are set or change. Updates the parent tooltip with the latest border properties.
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            Parent?.UpdateTooltipProperties("Border", this);
        }

        /// <summary>
        /// Disposes resources associated with this component, clearing references to the parent tooltip.
        /// </summary>
        protected override ValueTask DisposeAsyncCore()
        {
            ComponentDispose();
            return base.DisposeAsyncCore();
        }

        internal void ComponentDispose()
        {
            Parent = null!;
            ChildContent = null!;
        }
        #endregion
    }
}
