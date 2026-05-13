using Microsoft.AspNetCore.Components;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides options to customize the toolbar position for the <see cref="SfChart">Chart</see> zoom kit.
    /// </summary>
    /// <remarks>
    /// Use with <see cref="ChartZoomSettings"/> to control horizontal/vertical alignment and pixel offsets.
    /// </remarks>
    public class ChartZoomToolbarPosition : ChartSubComponent
    {
        #region Properties

        /// <summary>
        /// Gets or sets the owning <see cref="ChartZoomSettings"/> via cascading parameters.
        /// </summary>
        [CascadingParameter]
        private ChartZoomSettings? ZoomSettings { get; set; }

        /// <summary>
        /// Gets or sets the horizontal alignment of the toolbar.
        /// </summary>
        /// <value>
        /// Specifies the horizontal position of the toolbar. The available options are:
        /// <list type="bullet">
        ///   <item>
        ///     <term><b>Left</b></term>
        ///     <description>Aligns the toolbar to the left side of the chart.</description>
        ///   </item>
        ///   <item>
        ///     <term><b>Center</b></term>
        ///     <description>Centers the toolbar horizontally within the chart.</description>
        ///   </item>
        ///   <item>
        ///     <term><b>Right</b></term>
        ///     <description>Aligns the toolbar to the right side of the chart. This is the default value.</description>
        ///   </item>
        /// </list>
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <SfChart>
        ///     <ChartZoomSettings ToolbarDisplayMode="ToolbarMode.Always" Mode="ZoomMode.X">
        ///         <ChartZoomToolbarPosition HorizontalAlign="HorizontalAlign.Left">
        ///         </ChartZoomToolbarPosition>
        ///     </ChartZoomSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        /// <remarks>
        /// This property determines the horizontal placement of the toolbar relative to the chart's plotting area.
        /// </remarks>
        [Parameter]
        public HorizontalAlign HorizontalAlign
        {
            get; set;
        } = HorizontalAlign.Right;

        /// <summary>
        /// Gets or sets the vertical alignment of the toolbar.
        /// </summary>
        /// <value>
        /// Specifies the vertical position of the toolbar. The available options are:
        /// <list type="bullet">
        ///   <item>
        ///     <term><b>Top</b></term>
        ///     <description>Positions the toolbar at the top of the chart. This is the default value.</description>
        ///   </item>
        ///   <item>
        ///     <term><b>Middle</b></term>
        ///     <description>Vertically centers the toolbar within the chart.</description>
        ///   </item>
        ///   <item>
        ///     <term><b>Bottom</b></term>
        ///     <description>Positions the toolbar at the bottom of the chart.</description>
        ///   </item>
        /// </list>
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <SfChart>
        ///     <ChartZoomSettings ToolbarDisplayMode="ToolbarMode.Always" Mode="ZoomMode.X">
        ///         <ChartZoomToolbarPosition VerticalAlign="VerticalAlign.Top">
        ///         </ChartZoomToolbarPosition>
        ///     </ChartZoomSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        /// <remarks>
        /// This property determines the vertical placement of the toolbar relative to the chart's plotting area.
        /// </remarks>
        [Parameter]
        public VerticalAlign VerticalAlign { get; set; } = VerticalAlign.Top;

        /// <summary>
        /// Gets or sets the horizontal offset of the toolbar from its horizontal position.
        /// </summary>
        /// <value>
        /// A double value that specifies the horizontal distance, in pixels, from the toolbar's horizontal position.
        /// For example, if the horizontal alignment is set to <b>Center</b> and the offset is <b>30</b>, the toolbar will shift 30 pixels to the right of the center.
        /// Negative values move the toolbar in the opposite direction. The default value is <b>0</b>.
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <SfChart>
        ///     <ChartZoomSettings ToolbarDisplayMode="ToolbarMode.Always" Mode="ZoomMode.X">
        ///         <ChartZoomToolbarPosition X="30">
        ///         </ChartZoomToolbarPosition>
        ///     </ChartZoomSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        /// <remarks>
        /// Use this property to fine-tune the toolbar's horizontal placement beyond the predefined alignment options.
        /// </remarks>
        [Parameter]
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the vertical offset of the toolbar from its vertical position.
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <SfChart>
        ///     <ChartZoomSettings ToolbarDisplayMode="ToolbarMode.Always" Mode="ZoomMode.X">
        ///         <ChartZoomToolbarPosition Y="50">
        ///         </ChartZoomToolbarPosition>
        ///     </ChartZoomSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        /// <value>
        /// A double value that specifies the vertical distance, in pixels, from the toolbar's vertical position.
        /// For example, if the vertical alignment is set to <b>Middle</b> and the offset is <b>50</b>, the toolbar will shift 50 pixels downward from the center.
        /// Negative values move the toolbar upward. The default value is <b>0</b>.
        /// </value>
        /// <remarks>
        /// Use this property to fine-tune the toolbar's vertical placement beyond the predefined alignment options.
        /// </remarks>
        [Parameter]
        public double Y { get; set; }

        #endregion

        #region Lifecycle Methods

        /// <exclude />
        /// <summary>
        /// Called by the framework when the component is initialized.
        /// Registers this position object with the owning zoom settings.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Tracker is ChartZoomSettings chartZoomSettings)
            {
                ZoomSettings = chartZoomSettings;
            }

            ZoomSettings?.UpdateToolbarPosition(this);
        }

        /// <exclude />
        /// <summary>
        /// Called by the framework when component parameters are set.
        /// Notifies the owning zoom settings to update toolbar position.
        /// </summary>        
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            ZoomSettings?.UpdateToolbarPosition(this);
        }

        /// <summary>
        /// Releases references to allow garbage collection.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override ValueTask DisposeAsyncCore()
        {
            ZoomSettings = null;
            ChildContent = null!;
            return base.DisposeAsyncCore();
        }

        #endregion
    }
}