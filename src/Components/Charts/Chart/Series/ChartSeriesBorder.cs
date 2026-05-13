using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides options to customize the border of the chart series.
    /// </summary>
    /// <remarks>
    /// Use this component to configure color and width for series borders.
    /// Runtime changes queue a render to update visuals.
    /// </remarks>
    public class ChartSeriesBorder : ChartSubComponent
    {
        #region Fields

        private string _color = string.Empty;
        private double _width = 1;
        private bool _isPropertyChanged;

        #endregion

        #region Properties

        [CascadingParameter]
        private ChartSeries? Series { get; set; }

        /// <summary>
        /// Gets or sets the color of the border.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the color of the series border. Accepts valid CSS color strings such as hex and rgba.
        /// </value>
        /// <remarks>
        /// Changing this value will update the series border color on the next render pass.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartSeriesBorder Width="5" Color="blue" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string Color { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the width of the border in pixels.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> representing the width of the series border.
        /// </value>
        /// <remarks>
        /// Adjusting the width affects the visual thickness of the series border.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartSeriesBorder Width="10" Color="red" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double Width { get; set; } = 1;

        #endregion

        #region Lifecycle Methods

        /// <exclude />
        /// <summary>
        /// Initializes the border component and registers it with the parent <see cref="ChartSeries"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Tracker is ChartSeries chartSeries)
            {
                Series = chartSeries;
            }

            Series?.UpdateSeriesProperties("Border", this);
        }

        /// <exclude />
        /// <summary>
        /// Applies pending border changes by re-queuing the render on the series renderer.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (_color != Color)
            {
                _color = Color;
                Series?.Renderer?.UpdateCustomization(nameof(Color));
                if (Series is not null) { _isPropertyChanged = true; }
            }

            if (_width != Width)
            {
                _width = Width;
                Series?.Renderer?.UpdateCustomization(nameof(Width));
                if (Series is not null) { _isPropertyChanged = true; }
            }

            if (_isPropertyChanged)
            {
                _isPropertyChanged = false;
                Series?.Renderer?.ProcessRenderQueue();
            }
        }

        /// <summary>
        /// Disposes border component resources and releases references to the parent series.
        /// </summary>
        /// <inheritdoc />
        protected override ValueTask DisposeAsyncCore()
        {
            Series = null;
            ChildContent = null!;
            return base.DisposeAsyncCore();
        }

        #endregion
    }
}
