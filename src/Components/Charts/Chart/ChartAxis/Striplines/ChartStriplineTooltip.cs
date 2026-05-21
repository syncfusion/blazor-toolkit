using Microsoft.AspNetCore.Components;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary> 
    /// Represents the tooltip configuration settings for an axis stripline rendered within the <see cref="SfChart"/> component. 
    /// </summary> 
    /// <remarks> 
    ///  
    /// Use <see cref="ChartStriplineTooltip"/> to enable and customize the tooltip shown for a <see cref="ChartStripline"/>. 
    /// When enabled, the tooltip is displayed when the user hovers, focuses, or taps the stripline region, providing contextual 
    /// information about the stripline range or purpose. 
    ///  
    ///  
    /// The tooltip follows the chart’s active theme and aligns with the standard tooltip behavior used by chart series. 
    /// You can configure visibility and appearance using the available properties on <see cref="ChartStriplineTooltip"/> (for example, enabling the tooltip and customizing its visuals as supported). 
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
    ///                 <ChartStriplineTooltip Enable="true" /> 
    ///             </ChartStripline> 
    ///         </ChartStriplines> 
    ///     </ChartPrimaryXAxis> 
    /// </SfChart> 
    /// ]]></code> 
    /// </example> 
    public partial class ChartStriplineTooltip : ChartSubComponent
    {
        #region Internal Properties

        /// <summary>
        /// Gets or sets the cascading parent stripline.
        /// </summary>
        [CascadingParameter]
        internal ChartStripline? Parent { get; set; }

        /// <summary>
        /// Gets or sets the border configuration for the stripline tooltip.
        /// </summary>
        internal ChartStriplineTooltipBorder Border { get; set; } = new ChartStriplineTooltipBorder();

        /// <summary>
        /// Gets or sets the text style configuration for the stripline tooltip.
        /// </summary>
        internal ChartStriplineTooltipTextStyle TextStyle { get; set; } = new ChartStriplineTooltipTextStyle();
        #endregion

        #region Properties

        /// <summary> 
        /// Gets or sets a value indicating whether the stripline tooltip is enabled. 
        /// </summary> 
        /// <value> 
        /// A <see cref="bool"/> indicating whether the stripline tooltip is enabled. The default value is <see langword="false"/>. 
        /// </value> 
        /// <remarks> 
        /// When enabled, a tooltip is shown for the associated <see cref="ChartStripline"/> on hover, focus, or tap within the stripline area. 
        /// Use this setting on <see cref="ChartStriplineTooltip"/> to turn stripline tooltips on or off per stripline. 
        /// </remarks> 
        /// <example> 
        /// <code><![CDATA[ 
        /// <SfChart> 
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"> 
        ///         <ChartStriplines> 
        ///             <ChartStripline Start="20" End="25" Color="red"> 
        ///                 <ChartStriplineTooltip Enable="true" /> 
        ///             </ChartStripline> 
        ///         </ChartStriplines> 
        ///     </ChartPrimaryXAxis> 
        /// </SfChart> 
        /// ]]></code> 
        /// </example> 
        [Parameter]
        public bool Enable { get; set; } = false;

        /// <summary> 
        /// Gets or sets the fill color of the stripline tooltip background. 
        /// </summary> 
        /// <value> 
        /// A <see cref="string"/> representing the tooltip background color. The default value is <c>string.Empty</c>. 
        /// </value> 
        /// <remarks> 
        /// Accepts any valid CSS color string (for example, hex, rgba, or named colors). 
        /// When the value is empty, the tooltip uses the active chart theme’s default background color. 
        /// </remarks> 
        /// <example> 
        /// <code><![CDATA[ 
        /// <SfChart> 
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"> 
        ///         <ChartStriplines> 
        ///             <ChartStripline Start="20" End="25" Color="red"> 
        ///                 <ChartStriplineTooltip Enable="true" Fill="rgba(0,0,0,0.85)" /> 
        ///             </ChartStripline> 
        ///         </ChartStriplines> 
        ///     </ChartPrimaryXAxis> 
        /// </SfChart> 
        /// ]]></code> 
        /// </example> 
        [Parameter]
        public string Fill { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets the opacity of the stripline tooltip. 
        /// </summary> 
        /// <value> 
        /// A <see cref="double"/> specifying the tooltip opacity in the range from <c>0</c> (transparent) to <c>1</c> (opaque). The default value is <c>0.75</c>. 
        /// </value> 
        /// <remarks> 
        /// Increase opacity to improve contrast over chart elements. Combine with <see cref="Fill"/> to achieve the desired visual clarity. 
        /// </remarks> 
        /// <example> 
        /// <code><![CDATA[ 
        /// <SfChart> 
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"> 
        ///         <ChartStriplines> 
        ///             <ChartStripline Start="20" End="25" Color="red"> 
        ///                 <ChartStriplineTooltip Enable="true" Opacity="0.85" /> 
        ///             </ChartStripline> 
        ///         </ChartStriplines> 
        ///     </ChartPrimaryXAxis> 
        /// </SfChart> 
        /// ]]></code> 
        /// </example> 
        [Parameter]
        public double Opacity { get; set; } = 0.75;

        /// <summary> 
        /// Gets or sets the header text for the stripline tooltip. 
        /// </summary> 
        /// <value> 
        /// A <see cref="string"/> representing the tooltip header text. The default value is <c>string.Empty</c>. 
        /// </value> 
        /// <remarks> 
        /// Use to provide a concise title or label for the stripline context shown in the tooltip. 
        /// If no header is provided, the header section and its separator may be suppressed based on <see cref="ShowHeaderLine"/>. 
        /// </remarks> 
        /// <example> 
        /// <code><![CDATA[ 
        /// <SfChart> 
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"> 
        ///         <ChartStriplines> 
        ///             <ChartStripline Start="20" End="25" Color="red"> 
        ///                 <ChartStriplineTooltip Enable="true" Header="Maintenance Window" /> 
        ///             </ChartStripline> 
        ///         </ChartStriplines> 
        ///     </ChartPrimaryXAxis> 
        /// </SfChart> 
        /// ]]></code> 
        /// </example> 
        [Parameter]
        public string Header { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets a value indicating whether the header separator line is displayed in the stripline tooltip. 
        /// </summary> 
        /// <value> 
        /// A <see cref="bool"/> value. <see langword="true"/> to display the header line; otherwise, <see langword="false"/>. The default value is <see langword="true"/>. 
        /// </value> 
        /// <remarks> 
        /// Shows a visual divider between the header and the tooltip content. 
        /// The separator line is automatically hidden when <see cref="Header"/> is empty. 
        /// </remarks> 
        /// <example> 
        /// <code><![CDATA[ 
        /// <SfChart> 
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"> 
        ///         <ChartStriplines> 
        ///             <ChartStripline Start="20" End="25" Color="red"> 
        ///                 <ChartStriplineTooltip Enable="true" Header="Promo" ShowHeaderLine="false" /> 
        ///             </ChartStripline> 
        ///         </ChartStriplines> 
        ///     </ChartPrimaryXAxis> 
        /// </SfChart> 
        /// ]]></code> 
        /// </example> 
        [Parameter]
        public bool ShowHeaderLine { get; set; } = true;

        /// <summary> 
        /// Gets or sets the format string used to generate the stripline tooltip content in the <see cref="SfChart"/> component. 
        /// </summary> 
        /// <value> 
        /// A <see cref="string"/> that defines the format for the tooltip content. The default value is <c>string.Empty</c>. 
        /// </value> 
        /// <remarks> 
        /// The format supports token placeholders that are replaced with corresponding values at runtime. 
        /// Supported tokens: 
        /// <list type="bullet"> 
        /// <item><description><c>${stripline.text}</c> – The stripline label.</description></item> 
        /// <item><description><c>${stripline.start}</c> – The stripline start value.</description></item> 
        /// <item><description><c>${stripline.end}</c> – The stripline end value.</description></item> 
        /// <item><description><c>${axis.name}</c> – The axis name.</description></item> 
        /// <item><description><c>${stripline.segmentStart}</c> – The stripline segment start value. (if applicable)</description></item>
        /// <item><description><c>${stripline.segmentEnd}</c> – The stripline segment end value. (if applicable)</description></item>
        /// <item><description><c>${stripline.segmentAxisName}</c> – The stripline segment axis name. (if applicable)</description></item>
        /// <item><description><c>${stripline.size}</c> – The stripline size. (if applicable)</description></item>
        /// </list> 
        /// </remarks> 
        /// <example> 
        /// <code><![CDATA[ 
        /// @using Syncfusion.Blazor.Toolkit.Charts 
        /// 
        /// <SfChart> 
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"> 
        ///         <ChartStriplines> 
        ///             <ChartStripline Start="20" End="25" Color="red" Text="Campaign"> 
        ///                 <ChartStriplineTooltip Enable="true" 
        ///                                        Header="Details" 
        ///                                        Content="${stripline.text}<br/>From: ${stripline.start}<br/>To: ${stripline.end}<br/>Axis: ${axis.name}" /> 
        ///             </ChartStripline> 
        ///         </ChartStriplines> 
        ///     </ChartPrimaryXAxis> 
        /// </SfChart> 
        /// ]]></code> 
        /// </example> 
        [Parameter]
        public string Content { get; set; } = string.Empty;
        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Performs component initialization and registers tooltip settings with the parent stripline.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Tracker is ChartStripline chartStripline)
            {
                Parent = chartStripline;
            }
            Parent?.SetTooltipSettings(this);
        }

        /// <summary>
        /// Handles parameter changes and updates tooltip settings on the parent stripline.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            Parent?.SetTooltipSettings(this);
        }

        /// <summary>
        /// Performs cleanup and resource disposal.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override ValueTask DisposeAsyncCore()
        {
            Border.ComponentDispose();
            TextStyle.ComponentDispose();
            Parent = null!;
            Border = null!;
            TextStyle = null!;
            ChildContent = null!;
            return base.DisposeAsyncCore();
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Updates tooltip property values based on the provided key and value.
        /// </summary>
        /// <param name="key">The name of the property to update.</param>
        /// <param name="keyValue">The new value for the property.</param>
        internal void UpdateTooltipProperties(string key, object keyValue)
        {
            if (key == nameof(Border) && keyValue is ChartStriplineTooltipBorder border)
            {
                Border = border;
            }
            else if (key == nameof(TextStyle) && keyValue is ChartStriplineTooltipTextStyle textStyle)
            {
                TextStyle = textStyle;
            }
        }
        #endregion
    }
}
