using Microsoft.AspNetCore.Components;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides options to customize the border of the tooltip.
    /// </summary>
    /// <remarks>
    /// This component is used within <see cref="ChartTooltipSettings"/> to customize the tooltip border
    /// (e.g., color and thickness). It cascades its configuration to the parent tooltip settings.
    /// </remarks>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <SfChart>
    ///     <ChartTooltipSettings Enable="true">
    ///         <ChartTooltipBorder Width="2" Color="red"></ChartTooltipBorder>
    ///     </ChartTooltipSettings>
    /// </SfChart>
    /// ]]>
    /// </code>
    /// </example>
    public class ChartTooltipBorder : ChartDefaultBorder
    {
        #region Properties

        /// <summary>
        /// Gets or sets the parent tooltip settings to which this border belongs.
        /// </summary>
        /// <value>
        /// The parent <see cref="ChartTooltipSettings"/> instance, if any.
        /// </value>
        [CascadingParameter]
        private ChartTooltipSettings? Parent { get; set; }

        /// <summary>
        /// Gets or sets the border width for the tooltip.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> representing the width of the tooltip border. The default value is <b>0</b>.
        /// </value>
        /// <remarks>
        /// Use this property to control the thickness of the tooltip's border.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <SfChart>
        ///     <ChartTooltipSettings Enable="true">
        ///         <ChartTooltipBorder Width="2" Color="red"></ChartTooltipBorder>
        ///     </ChartTooltipSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override double Width { get; set; }

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Initializes the component and registers the border settings with the parent tooltip.
        /// </summary>
        /// <remarks>
        /// Called by the framework when the component is initialized.
        /// </remarks>
        /// <inheritdoc cref="ComponentBase.OnInitialized" />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Tracker is ChartTooltipSettings chartTooltipSettings)
            {
                Parent = chartTooltipSettings;
            }

            Parent?.UpdateTooltipProperties("Border", this);
        }

        /// <summary>
        /// Releases references and child content.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override ValueTask DisposeAsyncCore()
        {
            Parent = null;
            ChildContent = null!;
            return base.DisposeAsyncCore();
        }

        #endregion
    }
}
