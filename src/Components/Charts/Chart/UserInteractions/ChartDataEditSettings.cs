using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides options to customize drag-edit behavior for chart series points.
    /// </summary>
    /// <remarks>
    /// When enabled, users can drag supported series points to edit Y-values within optional bounds (<see cref="MinY"/>, <see cref="MaxY"/>).
    /// </remarks>
    public class ChartDataEditSettings : ChartSubComponent
    {
        #region Fields

        private bool _enable;
        private string _fill = string.Empty;
        private double _maxY = double.NaN;
        private double _minY = double.NaN;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the owning <see cref="ChartSeries"/> via cascading parameters.
        /// </summary>
        /// <value>
        /// The parent <see cref="ChartSeries"/> that consumes the data edit settings.
        /// </value>
        [CascadingParameter]
        private ChartSeries? Series { get; set; }

        /// <summary> 
        /// Gets or sets a value indicating whether to enable data editing by dragging the point. 
        /// </summary> 
        /// <value> 
        /// <b>true</b> if data editing is enabled; otherwise, <b>false</b>. 
        /// The default value is <b>false</b>. 
        /// </value> 
        /// <remarks> 
        /// When set to <b>true</b>, users can interactively edit data points by dragging them on the chart. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example shows how to enable data editing in a chart by setting the ChartDataEditSettings.
        /// <SfChart>
        ///     <ChartSeries DataSource="@Data" Name="England" XName="XValue" YName="YValue">
        ///         <ChartDataEditSettings Enable="true"></ChartDataEditSettings>
        ///         <ChartMarker Visible="true" Width="10" Height="10" />
        ///     </ChartSeries>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example> 
        [Parameter]
        public bool Enable { get; set; }

        /// <summary> 
        /// Gets or sets the color for the edited point. 
        /// </summary> 
        /// <value> 
        /// A string representing the color of the edited point. 
        /// </value> 
        /// <remarks> 
        /// The <see cref="Fill"/> property specifies the color of the point interior which will be assigned while dragging and will remain for the edited point. 
        /// This property accepts values in hex or rgba as a valid CSS color string. 
        /// <br/>
        /// Note: It is not applicable to line-based series types such as Line, Step Line, and Spline. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates enabling data editing in a column chart and setting the fill color of edited points.
        /// <SfChart>
        ///     <ChartSeries DataSource="@Data" Name="England" XName="XValue" YName="YValue" Type="ChartSeriesType.Column">
        ///         <ChartDataEditSettings Enable="true" Fill="red"></ChartDataEditSettings>
        ///         <ChartMarker Visible="true" Width="10" Height="10" />
        ///     </ChartSeries>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string Fill { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets the maximum Y value to which the point can be dragged for data editing. 
        /// </summary>
        /// <value>
        /// Accepts double value. The default value is <see cref="double.NaN"/>.
        /// </value>
        /// <remarks> 
        /// This property helps control the upper limit for the Y-axis when interactively editing data points on the chart. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to enable data editing in a column chart
        /// // and limit the maximum editable Y-value to 20 using MaxY property.
        /// <SfChart>
        ///     <ChartSeries DataSource="@Data" Name="England" XName="XValue" YName="YValue" Type="ChartSeriesType.Column">
        ///         <ChartDataEditSettings Enable="true" MaxY="20"></ChartDataEditSettings>
        ///         <ChartMarker Visible="true" Width="10" Height="10" />
        ///     </ChartSeries>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double MaxY { get; set; } = double.NaN;

        /// <summary> 
        /// Gets or sets the minimum Y value to which the point can be dragged for data editing. 
        /// </summary>
        /// <value>
        /// Accepts double value. The default value is <see cref="double.NaN"/>.
        /// </value>
        /// <remarks> 
        /// This property helps control the lower limit for the Y-axis when interactively editing data points on the chart. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to enable data editing in a column chart
        /// // and restrict the minimum editable Y-value to 30 using the MinY property.
        /// <SfChart>
        ///     <ChartSeries DataSource="@Data" Name="England" XName="XValue" YName="YValue" Type="ChartSeriesType.Column">
        ///         <ChartDataEditSettings Enable="true" MinY="30"></ChartDataEditSettings>
        ///         <ChartMarker Visible="true" Width="10" Height="10" />
        ///     </ChartSeries>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double MinY { get; set; } = double.NaN;

        #endregion

        #region Lifecycle Methods

        /// <exclude />
        /// <summary>
        /// Called by the framework when the component is initialized.
        /// Binds this settings instance to the owning <see cref="ChartSeries"/>.
        /// </summary>
        /// <remarks>This method runs once during component initialization.</remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Tracker is ChartSeries chartSeries)
            {
                Series = chartSeries;
            }

            Series?.UpdateSeriesProperties("ChartDataEditSettings", this);
        }

        /// <summary>
        /// Called by the framework when component parameters have been set.
        /// Updates the chart theme style and triggers a dimension update when required.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (_enable != Enable)
            {
                _enable = Enable;
                _isPropertyChanged = true;
            }

            if (_fill != Fill)
            {
                _fill = Fill;
                _isPropertyChanged = true;
            }

            if (_maxY != MaxY)
            {
                _maxY = MaxY;
                _isPropertyChanged = true;
            }
            if (_minY != MinY)
            {
                _minY = MinY;
                _isPropertyChanged = true;
            }
        }

        /// <summary>
        /// Releases references to allow garbage collection.
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
            Series = null;
            ChildContent = null!;
        }
        #endregion
    }
}