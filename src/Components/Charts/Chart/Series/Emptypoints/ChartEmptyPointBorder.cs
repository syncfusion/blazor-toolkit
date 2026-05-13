using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Represents the border customization options for empty points in a chart series.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// // This example demonstrates how to apply a custom border color and width to the empty points.
    /// <SfChart>
    ///     <ChartSeriesCollection>
    ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" Type="ChartSeriesType.Column">
    ///             <ChartEmptyPointSettings Mode="EmptyPointMode.Average">
    ///                 <ChartEmptyPointBorder Width="2" Color="blue"></ChartEmptyPointBorder>
    ///             </ChartEmptyPointSettings>
    ///         </ChartSeries>
    ///     </ChartSeriesCollection>
    /// </SfChart>
    /// ]]>
    /// </code>
    /// </example>
    public class ChartEmptyPointBorder : ChartDefaultBorder
    {
        #region Fields

        private string _color = Constants.Transparent;
        private double _width;
        private bool _needsEmptyPointBorderUpdate;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the parent <see cref="ChartEmptyPointSettings"/> via cascading parameters.
        /// </summary>
        /// <value>The dynamic parent that owns this border configuration.</value>
        [CascadingParameter]
        private ChartEmptyPointSettings? DynamicParent { get; set; }

        /// <summary>
        /// Gets or sets the border color of the empty point.
        /// </summary>
        /// <value>
        /// A string representing the color of the empty point's border. The default value is <c>"transparent"</c>.
        /// </value>
        /// <remarks>
        /// Changing this property will trigger an update to the empty point's border color.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to apply a custom border color to the empty points.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" Type="ChartSeriesType.Column">
        ///             <ChartEmptyPointSettings Mode="EmptyPointMode.Average">
        ///                 <ChartEmptyPointBorder Width="2" Color="blue"></ChartEmptyPointBorder>
        ///             </ChartEmptyPointSettings>
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override string? Color { get; set; } = Constants.Transparent;

        /// <summary>
        /// Gets or sets the width of the empty point's border.
        /// </summary>
        /// <value>
        /// A double representing the width of the border. Default is <c>0</c>.
        /// </value>
        /// <remarks>
        /// Changing this property will trigger an update to the empty point's border width.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to apply a custom border width to the empty points.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" Type="ChartSeriesType.Column">
        ///             <ChartEmptyPointSettings Mode="EmptyPointMode.Average">
        ///                 <ChartEmptyPointBorder Width="2" Color="blue"></ChartEmptyPointBorder>
        ///             </ChartEmptyPointSettings>
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override double Width { get; set; }

        #endregion

        #region Lifecycle Methods

        /// <exclude />
        /// <summary>
        /// Called by the framework to initialize the component. Binds to the cascading parent and
        /// notifies it about the border instance to keep internal references in sync.
        /// </summary>        
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Tracker is ChartEmptyPointSettings emptyPointSettings)
            {
                DynamicParent = emptyPointSettings;
            }

            DynamicParent?.UpdateEmptyPointProperties("Border", this);
        }

        /// <exclude />
        /// <summary>
        /// Called by the framework when component parameters are set. If any border-related property
        /// changed, triggers an optimized marker/series update and re-registers this border with the parent.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (_needsEmptyPointBorderUpdate)
            {
                _needsEmptyPointBorderUpdate = false;
                DynamicParent?.Series?.Marker.Renderer?.UpdateDirection();
            }
            if (_color != Color)
            {
                _color = Color ?? Constants.Transparent;
                _needsEmptyPointBorderUpdate = true;
            }
            if (_width != Width)
            {
                _width = Width;
                _needsEmptyPointBorderUpdate = true;
            }

            DynamicParent?.UpdateEmptyPointProperties("Border", this);
        }

        /// <summary>
        /// Disposes the component and clears references to its parent.
        /// </summary>
        protected override ValueTask DisposeAsyncCore()
        {
            ComponentDispose();
            return base.DisposeAsyncCore();
        }
        internal void ComponentDispose()
        {
            DynamicParent = null;
        }
        #endregion
    }
}
