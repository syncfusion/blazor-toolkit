using Microsoft.AspNetCore.Components;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides options to customize the border of the last data point label within a <see cref="ChartSeries"/>.
    /// </summary>
    /// <remarks>
    /// This subcomponent is used inside <see cref="ChartLastDataLabel"/> and allows setting border width and color
    /// for the label's background rect, improving separation and contrast.
    /// </remarks>
    public class ChartLastDataLabelBorder : ChartDefaultBorder
    {

        #region Fields
        ChartLastDataLabel? _lastDataLabel;
        double _prevWidth;
        string _prevColor = string.Empty;
        #endregion

        #region Properties

        /// <summary>
        /// Gets the owning <see cref="SfChart"/> via cascading parameters.
        /// </summary>
        /// <value>The parent <see cref="SfChart"/> if available; otherwise, <see langword="null"/>.</value>
        [CascadingParameter]
        SfChart? Chart { get; set; }

        /// <summary>
        /// Gets or sets the border width of the last value label.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> that defines the thickness of the border in pixels.
        /// The default value is <c>1</c>.
        /// </value>
        /// <remarks>
        /// Use this to visually distinguish the last value label over busy or colored chart backgrounds.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartLastDataLabel ShowLabel="true">
        ///   <ChartLastDataLabelBorder Width="1" />
        /// </ChartLastDataLabel>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override double Width { get; set; } = 1;

        /// <summary>
        /// Gets or sets the border color of the last value label.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the border color. Accepts CSS color formats such as hex (<c>#ff0000</c>), rgb, rgba, or named colors.
        /// The default value is <see cref="string.Empty"/>, which falls back to theme or inherited styles.
        /// </value>
        /// <remarks>
        /// Defines the visual boundary of the label so it remains legible against surrounding chart elements.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartLastDataLabel ShowLabel="true">
        ///   <ChartLastDataLabelBorder Color="red" />
        /// </ChartLastDataLabel>
        /// ]]>
        /// </code>
        /// </example>
        public override string Color { get; set; } = string.Empty;

        #endregion

        #region Lifecycle Methods

        /// <exclude />
        /// <summary>
        /// Initializes the border component and registers it with the owning <see cref="ChartLastDataLabel"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Tracker is ChartLastDataLabel lastLabel)
            {
                _lastDataLabel = lastLabel;
            }

            if (_lastDataLabel is not null)
            {
                _lastDataLabel.UpdateLastlabelProperties("Border", this);
            }
        
        }

        /// <exclude />
        /// <summary>
        /// Applies parameter updates and triggers a renderer refresh when values change after initial render.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            _lastDataLabel?.UpdateLastlabelProperties("Border", this);

            if (_prevWidth != Width || _prevColor != Color)
            {
                _prevWidth = Width;
                _prevColor = Color;

                if (Chart is not null && Chart.IsRendered)
                {
                    _lastDataLabel?.Renderer?.LastlabelValueChanged();
                }
            }
        }

        /// <summary>
        /// Releases references to parent components and clears content.
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
            _lastDataLabel = null;
            ChildContent = null!;
            Chart = null;
        }
        #endregion
    }
}
