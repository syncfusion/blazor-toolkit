using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Charts.Internal;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary> 
    /// Provides options to customize the text style of the stripline tooltip in the <see cref="SfChart"/> component. 
    /// </summary> 
    /// <remarks> 
    ///  
    /// The <see cref="ChartStriplineTooltipTextStyle"/> class configures the font appearance for text displayed inside a 
    /// <see cref="ChartStripline"/> tooltip. It inherits common font properties from <see cref="ChartDefaultFont"/>, 
    /// such as size, family, weight, and color. 
    ///  
    ///  
    /// When not explicitly set, tooltip text style values fall back to the active chart theme defaults. Use this class within 
    /// <see cref="ChartStriplineTooltip"/> to ensure tooltip text remains readable and visually consistent with your application’s design. 
    ///  
    /// </remarks> 
    /// <example> 
    /// <code><![CDATA[ 
    /// @using Syncfusion.Blazor.Toolkit.Charts 
    /// 
    /// <SfChart> 
    ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"> 
    ///         <ChartStriplines> 
    ///             <ChartStripline Start="20" End="25" Color="red"> 
    ///                 <ChartStriplineTooltip Enable="true"> 
    ///                     <ChartStriplineTooltipTextStyle Size="14px" 
    ///                                                     FontFamily="Roboto" 
    ///                                                     FontWeight="600" 
    ///                                                     Color="#FFFFFF" /> 
    ///                 </ChartStriplineTooltip> 
    ///             </ChartStripline> 
    ///         </ChartStriplines> 
    ///     </ChartPrimaryXAxis> 
    /// </SfChart> 
    /// ]]></code> 
    /// </example> 
    public class ChartStriplineTooltipTextStyle : ChartDefaultFont
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
        /// Gets or sets the font size of the stripline tooltip text in the <see cref="SfChart"/> component. 
        /// </summary> 
        /// <value> 
        /// A <see cref="string"/> representing the font size (for example, <c>"12px"</c>, <c>"0.875rem"</c>). The default value is <c>string.Empty</c>. 
        /// </value> 
        /// <remarks> 
        /// Accepts valid CSS length units such as <c>px</c>, <c>em</c>, or <c>rem</c>. 
        /// When empty, the active chart theme’s default size is applied. 
        /// </remarks> 
        /// <example> 
        /// <code><![CDATA[ 
        /// <SfChart> 
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"> 
        ///         <ChartStriplines> 
        ///             <ChartStripline Start="20" End="25" Color="red"> 
        ///                 <ChartStriplineTooltip Enable="true"> 
        ///                     <ChartStriplineTooltipTextStyle Size="15px" /> 
        ///                 </ChartStriplineTooltip> 
        ///             </ChartStripline> 
        ///         </ChartStriplines> 
        ///     </ChartPrimaryXAxis> 
        /// </SfChart> 
        /// ]]></code> 
        /// </example> 
        [Parameter]
        public override string Size { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets the font family of the stripline tooltip text in the <see cref="SfChart"/> component. 
        /// </summary> 
        /// <value> 
        /// A <see cref="string"/> representing the font family. The default value is <c>string.Empty</c>. 
        /// </value> 
        /// <remarks> 
        /// Accepts any valid CSS <c>font-family</c> value, including comma-separated fallback lists. 
        /// When empty, the active chart theme’s default font family is applied. 
        /// </remarks> 
        /// <example> 
        /// <code><![CDATA[ 
        /// <SfChart> 
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"> 
        ///         <ChartStriplines> 
        ///             <ChartStripline Start="20" End="25" Color="red"> 
        ///                 <ChartStriplineTooltip Enable="true"> 
        ///                     <ChartStriplineTooltipTextStyle FontFamily="Segoe UI" /> 
        ///                 </ChartStriplineTooltip> 
        ///             </ChartStripline> 
        ///         </ChartStriplines> 
        ///     </ChartPrimaryXAxis> 
        /// </SfChart> 
        /// ]]></code> 
        /// </example> 
        [Parameter]
        public override string FontFamily { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets the font weight of the stripline tooltip text in the <see cref="SfChart"/> component. 
        /// </summary> 
        /// <value> 
        /// A <see cref="string"/> representing the font weight (for example, <c>"400"</c>, <c>"600"</c>, or <c>"bold"</c>). The default value is <c>string.Empty</c>. 
        /// </value> 
        /// <remarks> 
        /// Accepts numeric weights and standard CSS keywords such as <c>normal</c>, <c>bold</c>, or <c>bolder</c>. 
        /// When empty, the active chart theme’s default font weight is applied. 
        /// </remarks> 
        /// <example> 
        /// <code><![CDATA[ 
        /// <SfChart> 
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"> 
        ///         <ChartStriplines> 
        ///             <ChartStripline Start="20" End="25" Color="red"> 
        ///                 <ChartStriplineTooltip Enable="true"> 
        ///                     <ChartStriplineTooltipTextStyle FontWeight="bold" /> 
        ///                 </ChartStriplineTooltip> 
        ///             </ChartStripline> 
        ///         </ChartStriplines> 
        ///     </ChartPrimaryXAxis> 
        /// </SfChart> 
        /// ]]></code> 
        /// </example> 
        [Parameter]
        public override string FontWeight { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets the font color of the stripline tooltip text in the <see cref="SfChart"/> component. 
        /// </summary> 
        /// <value> 
        /// A <see cref="string"/> specifying the text color. The default value is <c>string.Empty</c>. 
        /// </value> 
        /// <remarks> 
        /// Accepts valid CSS color values in hex, rgb/rgba, hsl/hsla, or named color formats. 
        /// When empty, the active chart theme’s default text color is applied. 
        /// </remarks> 
        /// <example> 
        /// <code><![CDATA[ 
        /// <SfChart> 
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"> 
        ///         <ChartStriplines> 
        ///             <ChartStripline Start="20" End="25" Color="red"> 
        ///                 <ChartStriplineTooltip Enable="true"> 
        ///                     <ChartStriplineTooltipTextStyle Color="#EEEEEE" /> 
        ///                 </ChartStriplineTooltip> 
        ///             </ChartStripline> 
        ///         </ChartStriplines> 
        ///     </ChartPrimaryXAxis> 
        /// </SfChart> 
        /// ]]></code> 
        /// </example> 
        [Parameter]
        public override string Color { get; set; } = string.Empty;

        #endregion

        #region Lifecycle methods

        /// <summary>
        /// Performs component initialization and registers tooltip properties with the parent.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Tracker is ChartStriplineTooltip chartStriplineTooltip)
            {
                Parent = chartStriplineTooltip;
            }
            Parent?.UpdateTooltipProperties("TextStyle", this);
        }

        /// <summary>
        /// Handles parameter changes and updates tooltip properties.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            Parent?.UpdateTooltipProperties("TextStyle", this);
        }

        /// <summary>
        /// Releases resources associated with this component.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
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

        #region Internal Methods

        /// <summary>
        /// Retrieves the effective font size, falling back to the chart theme default if not set.
        /// </summary>
        /// <param name="chartThemeStyle">The active chart theme style.</param>
        /// <returns>The font size value or an empty string.</returns>
        internal string GetFontSize(ChartThemeStyle chartThemeStyle)
        {
            return !string.IsNullOrEmpty(Size) ? Size : chartThemeStyle.TooltipTextSize ?? string.Empty;
        }

        /// <summary>
        /// Retrieves the effective font family, falling back to the chart theme default if not set.
        /// </summary>
        /// <param name="chartThemeStyle">The active chart theme style.</param>
        /// <returns>The font family value or an empty string.</returns>
        internal string GetFontFamily(ChartThemeStyle chartThemeStyle)
        {
            return !string.IsNullOrEmpty(FontFamily) ? FontFamily : chartThemeStyle.TooltipFontFamily ?? string.Empty;
        }

        /// <summary>
        /// Retrieves the effective font weight, falling back to the chart theme default if not set.
        /// </summary>
        /// <param name="chartThemeStyle">The active chart theme style.</param>
        /// <returns>The font weight value or an empty string.</returns>
        internal string GetFontWeight(ChartThemeStyle chartThemeStyle)
        {
            return !string.IsNullOrEmpty(FontWeight) ? FontWeight : chartThemeStyle.ToolTipFontWeight ?? string.Empty;
        }
        #endregion
    }
}
