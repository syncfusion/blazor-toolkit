using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides option to customize the margin of the data label.
    /// </summary>
    public class ChartDataLabelMargin : ChartDefaultMargin
    {
        #region Fields

        private ChartDataLabel? _dataLabel;
        private double _bottom;
        private double _top;
        private double _right;
        private double _left;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the bottom margin of the data label in the <see cref="SfChart"/>.
        /// </summary>
        /// <value>
        /// A double representing the size of the bottom margin. The default value is <b>5</b>.
        /// </value>
        /// <remarks>
        /// This property allows customization of the space between the data label and the bottom edge of its boundary. It is particularly useful when the <see cref="ChartDataLabelBorder"/> is enabled.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // Example demonstrating how to set a custom bottom margin for a data label:
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y">
        ///             <ChartMarker>
        ///                 <ChartDataLabel Visible="true">
        ///                     <ChartDataLabelBorder Width="1" Color="black"></ChartDataLabelBorder>
        ///                     <ChartDataLabelMargin Bottom="20"></ChartDataLabelMargin>
        ///                 </ChartDataLabel>
        ///             </ChartMarker>
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override double Bottom { get; set; } = Constants.DefaultMargin;

        /// <summary>
        /// Gets or sets the left margin of the data label in the <see cref="SfChart"/>.
        /// </summary>
        /// <value>
        /// A double representing the size of the left margin. The default value is <b>5</b>.
        /// </value>
        /// <remarks>
        /// This property allows customization of the space between the data label and the left edge of its boundary. It is particularly useful when the <see cref="ChartDataLabelBorder"/> is enabled.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // Example demonstrating how to set a custom left margin for a data label:
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y">
        ///             <ChartMarker>
        ///                 <ChartDataLabel Visible="true">
        ///                     <ChartDataLabelBorder Width="1" Color="black"></ChartDataLabelBorder>
        ///                     <ChartDataLabelMargin Left="20"></ChartDataLabelMargin>
        ///                 </ChartDataLabel>
        ///             </ChartMarker>
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override double Left { get; set; } = Constants.DefaultMargin;

        /// <summary>
        /// Gets or sets the right margin of the data label in the <see cref="SfChart"/>.
        /// </summary>
        /// <value>
        /// A double representing the size of the right margin. The default value is <b>5</b>.
        /// </value>
        /// <remarks>
        /// This property allows customization of the space between the data label and the right edge of its boundary. It is particularly useful when the <see cref="ChartDataLabelBorder"/> is enabled.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // Example demonstrating how to set a custom right margin for a data label:
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y">
        ///             <ChartMarker>
        ///                 <ChartDataLabel Visible="true">
        ///                     <ChartDataLabelBorder Width="1" Color="black"></ChartDataLabelBorder>
        ///                     <ChartDataLabelMargin Right="20"></ChartDataLabelMargin>
        ///                 </ChartDataLabel>
        ///             </ChartMarker>
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override double Right { get; set; } = Constants.DefaultMargin;

        /// <summary>
        /// Gets or sets the top margin of the data label in the <see cref="SfChart"/>.
        /// </summary>
        /// <value>
        /// A double representing the size of the top margin. The default value is <b>5</b>.
        /// </value>
        /// <remarks>
        /// This property allows customization of the space between the data label and the top edge of its boundary. It is particularly useful when the <see cref="ChartDataLabelBorder"/> is enabled.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // Example demonstrating how to set a custom top margin for a data label:
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y">
        ///             <ChartMarker>
        ///                 <ChartDataLabel Visible="true">
        ///                     <ChartDataLabelBorder Width="1" Color="black"></ChartDataLabelBorder>
        ///                     <ChartDataLabelMargin Top="20"></ChartDataLabelMargin>
        ///                 </ChartDataLabel>
        ///             </ChartMarker>
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override double Top { get; set; } = Constants.DefaultMargin;

        #endregion

        #region Lifecycle Methods

        /// <exclude />
        /// <summary>
        /// Initializes the margin and registers it with the parent <see cref="ChartDataLabel"/>.
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

            _dataLabel?.UpdateDatalabelProperties(nameof(ChartDataLabel.Margin), this);
        }

        /// <exclude />
        /// <summary>
        /// Applies parameter updates and triggers label refresh when margins change.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            _dataLabel?.UpdateDatalabelProperties(nameof(ChartDataLabel.Margin), this);
            // The dynamic update in child class properties is not functioning correctly.
            // This logic ensures proper API updates by checking for changes and invoking necessary rendering methods.
            if (!Equals(_bottom, Bottom) || !Equals(_top, Top) ||
                !Equals(_right, Right) || !Equals(_left, Left))
            {
                _bottom = Bottom;
                _top = Top;
                _right = Right;
                _left = Left;

                _dataLabel?.Renderer?.DatalabelValueChanged();
            }
        }

        /// <summary>
        /// Disposes the margin component and clears references.
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
