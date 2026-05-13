using Microsoft.AspNetCore.Components;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Configures the border appearance for the stack labels, including options for border width and color.
    /// </summary>
    /// <remarks>
    /// Use this component within <see cref="ChartStackLabelSettings"/> to customize the border around the stack labels.
    /// Changes trigger a lightweight renderer refresh only when values actually change.
    /// </remarks>
    public class ChartStackLabelBorder : ChartDefaultBorder
    {
        #region Fields

        private ChartStackLabelSettings? _stackLabel;
        private double _previousWidth;
        private string? _previousColor;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the width of the stack label border.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> that represents the width of the border, in pixels.
        /// The default value is <c>double.NaN</c>.
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <SfChart>
        ///     <ChartSeries XName="x" YName="y" Type="ChartSeriesType.StackingColumn">
        ///     </ChartSeries>
        ///     <ChartStackLabelSettings Visible="true">
        ///         <ChartStackLabelBorder Width="2">
        ///         </ChartStackLabelBorder>
        ///     </ChartStackLabelSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        /// <remarks>
        /// Adjust this property to increase or decrease the thickness of the stack label border, enhancing the visibility
        /// and appearance of stack labels in the chart.
        /// </remarks>
        [Parameter]
        public override double Width { get; set; } = double.NaN;

        #endregion

        #region Lifecycle Methods

        /// <exclude />
        /// <summary>
        /// Initializes the component and registers this border instance with the parent <see cref="ChartStackLabelSettings"/>.
        /// </summary>
        /// <remarks>
        /// This ensures the parent stack-label settings can track and consume this border configuration when rendering.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Tracker is ChartStackLabelSettings chartStackLabelSettings)
            {
                _stackLabel = chartStackLabelSettings;
            }

            _stackLabel?.UpdateStackLabelProperties(nameof(ChartStackLabelSettings.Border), this);
        }

        /// <exclude />
        /// <summary>
        /// Applies parameter updates and notifies the renderer if border values changed since the previous render.
        /// </summary>
        /// <remarks>
        /// Re-renders are minimized by comparing the current and previous values for <see cref="Width"/> and <see cref="ChartDefaultBorder.Color"/>.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            _stackLabel?.UpdateStackLabelProperties(nameof(ChartStackLabelSettings.Border), this);

            if (_previousWidth != Width || _previousColor != Color)
            {
                _previousWidth = Width;
                _previousColor = Color;
                _stackLabel?.Renderer?.StackLabelValueChanged();
            }
        }

        /// <summary>
        /// Releases references to allow the component to be collected and avoid memory leaks.
        /// </summary>
        /// <remarks>
        /// Clears references to the parent and child content; no unmanaged resources are held.
        /// </remarks>
        protected override ValueTask DisposeAsyncCore()
        {
            ComponentDispose();
            return base.DisposeAsyncCore();
        }
        internal void ComponentDispose()
        {
            _stackLabel = null;
            ChildContent = null!;
        }
        #endregion
    }
}
