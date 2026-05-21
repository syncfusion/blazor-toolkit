using Microsoft.AspNetCore.Components;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Configures the border of data labels in <see cref="SfChart"/>.
    /// </summary>
    public class ChartDataLabelBorder : ChartDefaultBorder
    {
        #region Fields

        private ChartDataLabel? _dataLabel;
        private double _width;
        private string? _color;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the width of the data label border.
        /// </summary>
        /// <value>
        /// A double that represents the width of the border. The default value is <c>double.NaN</c>.
        /// </value>
        /// <remarks>
        /// The width of the data label border can be adjusted to enhance the visibility and aesthetics of the data labels in the chart.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following example demonstrates setting the width of the data label border to 2:
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y">
        ///             <ChartMarker>
        ///                 <ChartDataLabel Visible="true">
        ///                     <ChartDataLabelBorder Width="2" Color="#1b49cc"></ChartDataLabelBorder>
        ///                 </ChartDataLabel>
        ///             </ChartMarker>
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override double Width { get; set; } = double.NaN;

        #endregion

        #region Lifecycle Methods

        /// <exclude />
        /// <summary>
        /// Initializes the border and registers it with the parent <see cref="ChartDataLabel"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Tracker is ChartDataLabel chartDataLabel)
            {
                _dataLabel = chartDataLabel;
            }

            _dataLabel?.UpdateDatalabelProperties(nameof(ChartDataLabel.Border), this);
        }

        /// <exclude />
        /// <summary>
        /// Applies parameter updates and triggers label refresh when border values change.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            _dataLabel?.UpdateDatalabelProperties(nameof(ChartDataLabel.Border), this);

            if (_width != Width || _color != Color)
            {
                _width = Width;
                _color = Color;
                _dataLabel?.Renderer?.DatalabelValueChanged();
            }
        }

        /// <summary>
        /// Disposes this border and clears references.
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
            _dataLabel = null;
            ChildContent = null!;
        }
        #endregion
    }
}
