using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides options to customize the zoom settings of the chart.
    /// </summary>
    /// <remarks>
    /// Supports selection zoom, mouse wheel zoom, pinch zoom, pan, scrollbar, and a configurable toolbar with accessibility options.
    /// </remarks>
    public class ChartZoomSettings : ChartSubComponent
    {
        #region Fields

        private bool _isNeedUpdate;
        private bool _enableDeferredZooming = true;
        private bool _enableMouseWheelZooming;
        private bool _enablePan;
        private bool _enablePinchZooming;
        private bool _enableScrollbar;
        private bool _enableSelectionZooming;
        private ZoomMode _mode = ZoomMode.XY;
        private ToolbarMode _toolbarDisplayMode = ToolbarMode.OnDemand;
        private List<ToolbarItems> _toolbarItems = new List<ToolbarItems>
        {
            Toolkit.ToolbarItems.Zoom,
            Toolkit.ToolbarItems.ZoomIn,
            Toolkit.ToolbarItems.ZoomOut,
            Toolkit.ToolbarItems.Pan,
            Toolkit.ToolbarItems.Reset
        };
        private ChartZoomToolbarPosition? _toolbarPosition;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the cascading chart instance that owns these zoom settings.
        /// </summary>
        [CascadingParameter]
        private SfChart? Chart { get; set; }

        /// <summary> 
        /// Gets or sets a value indicating whether to update the chart with a delay while panning. 
        /// </summary> 
        /// <value> 
        /// <b>true</b> if updating the chart with a delay while panning is enabled; otherwise, <b>false</b>. 
        /// The default value is <b>true</b>. 
        /// </value> 
        /// <remarks> 
        /// Enabling this option can provide a smoother user experience during panning by introducing a delay before the chart is updated. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to enable selection zooming with deferred zooming in a chart.
        /// <SfChart>
        ///     <ChartZoomSettings EnableDeferredZooming="true" EnableSelectionZooming="true" />
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" Name="England" XName="XValue" YName="YValue">
        ///             <ChartMarker Visible="true" Width="10" Height="10" />
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool EnableDeferredZooming { get; set; } = true;

        /// <summary> 
        /// Gets or sets a value indicating whether the chart can be zoomed using the mouse wheel. 
        /// </summary> 
        /// <value> 
        /// <b>true</b> if the chart can be zoomed using the mouse wheel; otherwise, <b>false</b>. 
        /// The default value is <b>false</b>. 
        /// </value> 
        /// <remarks> 
        /// Enabling this option allows users to zoom in or out on the chart by using the mouse wheel. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to enable mouse wheel zooming in a chart.
        /// <SfChart>
        ///     <ChartZoomSettings EnableMouseWheelZooming="true" />
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" Name="England" XName="XValue" YName="YValue">
        ///             <ChartMarker Visible="true" Width="10" Height="10" />
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool EnableMouseWheelZooming { get; set; }

        /// <summary> 
        /// Gets or sets a value indicating whether the chart can be panned when zoomed. 
        /// </summary> 
        /// <value> 
        /// <b>true</b> if the chart can be panned when zoomed; otherwise, <b>false</b>. 
        /// The default value is <b>false</b>. 
        /// </value> 
        /// <remarks> 
        /// Enabling this option allows users to pan across the zoomed-in chart area for better exploration of specific regions. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to enable pan functionality in a chart.
        /// <SfChart>
        ///     <ChartZoomSettings EnablePan="true" />
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" Name="England" XName="XValue" YName="YValue">
        ///             <ChartMarker Visible="true" Width="10" Height="10" />
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool EnablePan { get; set; }

        /// <summary> 
        /// Gets or sets a value indicating whether the chart can be zoomed through pinch gestures on touch-enabled devices. 
        /// </summary> 
        /// <value> 
        /// <b>true</b> if the chart can be zoomed through pinch gestures; otherwise, <b>false</b>. 
        /// The default value is <b>false</b>. 
        /// </value> 
        /// <remarks> 
        /// Enabling this option allows users to zoom in or zoom out on the chart by performing pinch gestures on touch-enabled devices. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to enable pinch zooming in a chart.
        /// <SfChart>
        ///     <ChartZoomSettings EnablePinchZooming="true" />
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" Name="England" XName="XValue" YName="YValue">
        ///             <ChartMarker Visible="true" Width="10" Height="10" />
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool EnablePinchZooming { get; set; }

        /// <summary> 
        /// Gets or sets a value indicating whether the axis should have a scrollbar when zoomed. 
        /// </summary> 
        /// <value> 
        /// <b>true</b> if the axis should have a scrollbar when zoomed; otherwise, <b>false</b>. 
        /// The default value is <b>false</b>. 
        /// </value> 
        /// <remarks> 
        /// Enabling this option allows users to navigate and visualize specific regions of the chart using scrollbar when zoomed. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to enable the scrollbar in a chart.
        /// <SfChart>
        ///     <ChartZoomSettings EnableScrollbar="true" ToolbarDisplayMode="ToolbarMode.Always" />
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" Name="England" XName="XValue" YName="YValue">
        ///             <ChartMarker Visible="true" Width="10" Height="10" />
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool EnableScrollbar { get; set; }

        /// <summary> 
        /// Gets or sets a value indicating whether the chart can be zoomed by selecting a rectangular region on the plot area. 
        /// </summary> 
        /// <value> 
        /// <b>true</b> if the chart can be zoomed by selecting a rectangular region on the plot area; otherwise, <b>false</b>. 
        /// The default value is <b>false</b>. 
        /// </value> 
        /// <remarks> 
        /// Enabling this option provides an interactive way for users to focus on specific areas of interest within the chart. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to enable selection zooming in a chart.
        /// <SfChart>
        ///     <ChartZoomSettings EnableSelectionZooming="true" ToolbarDisplayMode="ToolbarMode.Always" />
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" Name="England" XName="XValue" YName="YValue">
        ///             <ChartMarker Visible="true" Width="10" Height="10" />
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool EnableSelectionZooming { get; set; }

        /// <summary> 
        /// Gets or sets the mode of zooming. 
        /// </summary> 
        /// <value> 
        /// One of the <see cref="ZoomMode"/> enumeration that specifies the mode of zooming. 
        /// The options include: 
        /// - <c>XY</c>: Chart can be zoomed both vertically and horizontally. 
        /// - <c>X</c>: Chart can be zoomed horizontally. 
        /// - <c>Y</c>: Chart can be zoomed vertically. 
        /// <br/>
        /// The default mode is <b>ZoomMode.XY</b>. 
        /// </value> 
        /// <remarks> 
        /// This property can be set to allow zooming both vertically and horizontally, only horizontally, or only vertically. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to enable zooming only along the Y-axis in a chart.
        /// <SfChart>
        ///     <ChartZoomSettings Mode="ZoomMode.Y" ToolbarDisplayMode="ToolbarMode.Always" />
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" Name="England" XName="XValue" YName="YValue">
        ///             <ChartMarker Visible="true" Width="10" Height="10" />
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ZoomMode Mode { get; set; } = ZoomMode.XY;

        /// <summary> 
        /// Gets or sets the visibility mode for the zooming toolbar in the chart. 
        /// </summary> 
        /// <value> 
        /// One of the <see cref="ToolbarMode"/> enumeration that specifies the visibility mode for the zooming toolbar. 
        /// The options include: 
        /// - <c>OnDemand</c>: The zooming toolbar is only visible while the chart has zoomed. 
        /// - <c>Always</c>: The zooming toolbar is always visible even though the chart has not zoomed. 
        /// - <c>None</c>: The zooming toolbar will not be visible even though the chart is zoomed. 
        /// <br/>
        /// The default value is <b>ToolbarMode.OnDemand</b>. 
        /// </value> 
        /// <remarks> 
        /// It determines whether the toolbar should be visible only when the chart is zoomed, always visible, or never visible. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to display the zoom toolbar always in a chart.
        /// <SfChart>
        ///     <ChartZoomSettings ToolbarDisplayMode="ToolbarMode.Always" />
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" Name="England" XName="XValue" YName="YValue">
        ///             <ChartMarker Visible="true" Width="10" Height="10" />
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example> 
        [Parameter]
        public ToolbarMode ToolbarDisplayMode { get; set; } = ToolbarMode.OnDemand;

        /// <summary> 
        /// Gets or sets the list of items to be added in the toolbar for the chart. 
        /// </summary> 
        /// <value> 
        /// The list of <see cref="ToolbarItems"/> to be displayed in the chart toolbar. 
        /// The available items include: 
        /// - <b>Zoom</b> 
        /// - <b>ZoomIn</b> 
        /// - <b>ZoomOut</b> 
        /// - <b>Pan</b> 
        /// - <b>Reset</b> 
        /// </value> 
        /// <remarks> 
        /// The <see cref="ToolbarItems"/> property allows customization of the items displayed in the chart toolbar. 
        /// Users can choose from a set of predefined items, such as Zoom, ZoomIn, ZoomOut, Pan, and Reset, to be included in the toolbar. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to configure specific toolbar items in a chart's zoom settings.
        /// <SfChart>
        ///     <ChartZoomSettings ToolbarDisplayMode="ToolbarMode.Always"
        ///                        ToolbarItems="new List<ToolbarItems>() { ToolbarItems.Zoom, ToolbarItems.Reset, ToolbarItems.Pan }" />
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" Name="England" XName="XValue" YName="YValue">
        ///             <ChartMarker Visible="true" Width="10" Height="10" />
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public List<ToolbarItems> ToolbarItems { get; set; } =
        [
            Toolkit.ToolbarItems.Zoom,
            Toolkit.ToolbarItems.ZoomIn,
            Toolkit.ToolbarItems.ZoomOut,
            Toolkit.ToolbarItems.Pan,
            Toolkit.ToolbarItems.Reset
        ];

        /// <summary>
        /// Gets or sets the format for the toolkit accessibility description of the <see cref="ToolbarItems">chart zoom toolkit</see>.
        /// </summary>
        /// <value>
        /// Accepts a string that defines the format for the toolkit accessibility description of the <see cref="ToolbarItems">chart zoom toolkit</see>. The default value is <b>null</b>.
        /// </value>
        /// <remarks>
        /// Use this property to specify a format for the toolkit accessibility description of the <see cref="ToolbarItems">chart zoom toolkit</see>.
        /// The placeholder ${value} can be used to set the accessibility text for the zoom toolkit toolbar item name.
        /// For example, the format "Selected the ${value} option" will read as "Selected the ZoomIn option" for the <see cref="ToolbarItems.ZoomIn"/> button.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to configure the accessibility description format for the toolbar items in a chart's zoom settings.
        /// <SfChart>
        ///     <ChartZoomSettings ToolbarDisplayMode="ToolbarMode.Always" AccessibilityDescriptionFormat="Selected the ${value} option" />
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" Name="England" XName="XValue" YName="YValue">
        ///             <ChartMarker Visible="true" Width="10" Height="10" />
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string AccessibilityDescriptionFormat { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the accessibility role for the <see cref="ChartZoomSettings"></see> chart zoom toolkit.
        /// </summary>
        /// <value>
        /// Accepts a string that defines the accessibility role for the <see cref="ChartZoomSettings"></see> chart zoom toolkit. The default value is <b>null</b>.
        /// </value>
        /// <remarks>
        /// Use this property to provide an accessibility role for the <see cref="ChartZoomSettings"></see> chart zoom toolkit.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to configure the accessibility role for the toolbar items in a chart's zoom settings.
        /// <SfChart>
        ///     <ChartZoomSettings ToolbarDisplayMode="ToolbarMode.Always" AccessibilityRole="button" />
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" Name="England" XName="XValue" YName="YValue">
        ///             <ChartMarker Visible="true" Width="10" Height="10" />
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string AccessibilityRole { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the accessibility keyboard navigation options for the <see cref="ChartZoomSettings"></see> chart zoom toolkit.
        /// </summary>
        /// <value>
        /// Accepts the boolean value to enable or disable the keyboard navigation for the <see cref="ChartZoomSettings"></see> chart zoom toolkit. The default value is <b>true</b>.
        /// </value>
        /// <remarks>
        /// Use this property to toggle the keyboard navigation focus for the <see cref="ChartZoomSettings"></see> chart zoom toolkit.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to configure zoom settings to be focusable in the chart.
        /// <SfChart>
        ///     <ChartZoomSettings ToolbarDisplayMode="ToolbarMode.Always" Focusable="true" />
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" Name="England" XName="XValue" YName="YValue">
        ///             <ChartMarker Visible="true" Width="10" Height="10" />
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool Focusable { get; set; } = true;

        /// <summary>
        /// Gets or sets the position of the zoom toolbar.
        /// </summary>
        /// <value>
        /// The position of the zoom toolbar, represented by a <see cref="ChartZoomToolbarPosition"/> value.
        /// The default value is determined by the chart's default settings.
        /// </value>
        /// <remarks>
        /// This property allows you to customize the position of the zoom toolbar in the chart.
        /// Changing this value will update the toolbar's position when the chart is rendered or updated.
        /// </remarks>
        internal ChartZoomToolbarPosition ToolbarPosition { get; set; } = new();

        #endregion

        #region Lifecycle Methods

        /// <exclude />
        /// <summary>
        /// Called by the framework when the component is initialized.
        /// Registers these zoom settings on the owning chart.
        /// </summary>        
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Chart is null)
            {
                return;
            }
            Chart._zoomSettings = this;
        }

        /// <exclude />
        /// <summary>
        /// Called by the framework when component parameters are set.
        /// Applies pending zoom configuration updates to the chart and its interop layer.
        /// </summary>
        /// <returns>A task representing the asynchronous update operation.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override async Task OnParametersSetAsync()
        {
            // Detect parameter changes and apply the same side-effects previously performed in property setters.
            if (_enableDeferredZooming != EnableDeferredZooming)
            {
                _enableDeferredZooming = EnableDeferredZooming;
                MarkForUpdateIfFirstRender();
            }

            if (_enableMouseWheelZooming != EnableMouseWheelZooming)
            {
                _enableMouseWheelZooming = EnableMouseWheelZooming;
                MarkForUpdateIfFirstRender();
            }

            if (_enablePan != EnablePan)
            {
                _enablePan = EnablePan;
                MarkForUpdateIfFirstRender();
            }

            if (_enablePinchZooming != EnablePinchZooming)
            {
                _enablePinchZooming = EnablePinchZooming;
                MarkForUpdateIfFirstRender();
            }

            if (_enableSelectionZooming != EnableSelectionZooming)
            {
                _enableSelectionZooming = EnableSelectionZooming;
                MarkForUpdateIfFirstRender();
            }

            if (_mode != Mode)
            {
                _mode = Mode;
                MarkForUpdateIfFirstRender();
            }

            if (_toolbarDisplayMode != ToolbarDisplayMode)
            {
                _toolbarDisplayMode = ToolbarDisplayMode;
                MarkForUpdateIfFirstRender();
            }

            if (!ReferenceEquals(_toolbarItems, ToolbarItems))
            {
                _toolbarItems = ToolbarItems;
                MarkForUpdateIfFirstRender();
            }

            if (_enableScrollbar != EnableScrollbar)
            {
                _enableScrollbar = EnableScrollbar;
                if (Chart is not null && Chart._isChartFirstRender)
                {
                    _isNeedUpdate = true;
                    Chart.UpdateRenderers(true);
                }
            }
            if (_toolbarPosition != ToolbarPosition)
            {
                _toolbarPosition = ToolbarPosition;
                MarkForUpdateIfFirstRender();
            }

            if (Chart is not null && _isNeedUpdate)
            {
                Chart.InitModules();
                await Chart.SetZoomOptionsAsync().ConfigureAwait(true);
                if (Chart._zoomingModule is not null)
                {
                    await InvokeVoidAsync(Chart._chartJsModule, Chart._chartJsInProcessModule, Constants.UpdateZoomingOptions, [Chart._dataId, Chart._zoomSettings.EnablePan])
                        .ConfigureAwait(false);
                }
            }

            await base.OnParametersSetAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Releases references to allow garbage collection.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override ValueTask DisposeAsyncCore()
        {
            Chart = null;
            ChildContent = null!;
            ToolbarItems = null!;
            return base.DisposeAsyncCore();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Notifies the chart to apply toolbar position updates.
        /// </summary>
        private void NotifyToolbarPositionChanged()
        {
            _ = Chart?.SetZoomOptionsAsync();
            Chart?.ApplyZoomkit();
        }

        /// <summary>
        /// Marks the component for update if the chart has already performed first render.
        /// </summary>
        private void MarkForUpdateIfFirstRender()
        {
            if (Chart is not null && Chart._isChartFirstRender)
            {
                _isNeedUpdate = true;
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Updates the toolbar position and applies the change if the chart is already rendered.
        /// </summary>
        /// <param name="value">The new <see cref="ChartZoomToolbarPosition"/> value.</param>
        internal void UpdateToolbarPosition(ChartZoomToolbarPosition value)
        {
            ToolbarPosition = value;
            if (Chart is not null && Chart._isChartFirstRender)
            {
                NotifyToolbarPositionChanged();
            }
        }

        /// <summary>
        /// Updates a property dynamically by its name.
        /// </summary>
        /// <param name="propertyName">The property name (e.g., <c>EnablePan</c>).</param>
        /// <param name="value">The value to apply.</param>
        internal void UpdateProperties(string propertyName, object value)
        {
            switch (propertyName)
            {
                case "Color":
                    EnableDeferredZooming = (bool)value;
                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}