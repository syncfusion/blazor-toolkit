using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Charts.Internal;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides font customization options for the last data label within a <see cref="ChartSeries"/>.
    /// </summary>
    /// <remarks>
    /// Place this subcomponent inside <see cref="ChartLastDataLabel"/> to control size, family, weight, style, and color of the label text.
    /// When a property is not provided, theme defaults (e.g., crosshair label text styles) are used to maintain visual consistency.
    /// </remarks>
    public class ChartLastDataLabelFont : ChartSubComponent
    {
        #region Fields
        ChartLastDataLabel? _lastDataLabel;

        // Cached values to minimize redundant renderer updates.
        string _prevSize = string.Empty;
        string _prevColor = string.Empty;
        string _prevFontFamily = string.Empty;
        string _prevFontWeight = string.Empty;
        string _prevFontStyle = string.Empty;
        #endregion

        #region Properties

        /// <summary>
        /// Gets the owning <see cref="SfChart"/> via cascading parameters.
        /// </summary>
        /// <value>The parent <see cref="SfChart"/> if available; otherwise, <see langword="null"/>.</value>
        [CascadingParameter]
        SfChart? Chart { get; set; }

        /// <summary>
        /// Gets or sets the font size of the last value label text.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> with any valid CSS unit (e.g., <c>14px</c>, <c>0.875rem</c>, <c>large</c>).
        /// If not set, a theme-specific default (e.g., crosshair text size) is used.
        /// </value>
        /// <remarks>
        /// Set this to tailor text legibility for your application’s typography scale.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartLastDataLabelFont Size="14px" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string Size { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the font color of the last value label text.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> specifying the text color. Accepts hex (e.g., <c>#00FF00</c>) and rgba (e.g., <c>rgba(0,255,0,1)</c>).
        /// If not set, the theme default is used (for example, <b>rgba(97,97,97,1)</b> in Material-based themes).
        /// </value>
        /// <remarks>
        /// Choose a color that contrasts with the background for optimal readability.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartLastDataLabelFont Color="Green" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string Color { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the font family of the last value label text.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> specifying the font family (e.g., <c>Arial</c>, <c>Roboto</c>).
        /// If not set, the chart theme’s default is used.
        /// </value>
        /// <remarks>
        /// Aligns text style with the broader application typography.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartLastDataLabelFont FontFamily="Arial" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string FontFamily { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the font weight of the last value label text.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> such as <c>Normal</c>, <c>Bold</c>, or numeric values like <c>600</c>.
        /// If not set, the theme default is used.
        /// </value>
        /// <remarks>
        /// Use heavier weights to emphasize the label in data-dense scenarios.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartLastDataLabelFont FontWeight="600" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string FontWeight { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the font style of the last value label text.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> such as <c>Normal</c>, <c>Italic</c>, or <c>Oblique</c>. The default is <c>Normal</c>.
        /// </value>
        /// <remarks>
        /// Italic styles may be used for subtle emphasis or to encode meaning.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartLastDataLabelFont FontStyle="Italic" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string FontStyle { get; set; } = "Normal";

        #endregion

        #region Lifecycle Methods

        /// <exclude />
        /// <summary>
        /// Initializes the font subcomponent and registers it with the parent <see cref="ChartLastDataLabel"/>.
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

            _lastDataLabel?.UpdateLastlabelProperties("Font", this);
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

            _lastDataLabel?.UpdateLastlabelProperties("Font", this);

            if (_prevSize != Size || _prevColor != Color || _prevFontFamily != FontFamily || _prevFontWeight != FontWeight || _prevFontStyle != FontStyle)
            {
                _prevSize = Size;
                _prevColor = Color;
                _prevFontFamily = FontFamily;
                _prevFontWeight = FontWeight;
                _prevFontStyle = FontStyle;

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

        #endregion

        #region Internal Methods
        /// <summary>
        /// Releases references to parent components and clears content.
        /// </summary>
        internal void ComponentDispose()
        {
            _lastDataLabel = null;
            ChildContent = null!;
            Chart = null;
        }
        /// <summary>
        /// Resolves the font size using component value or theme defaults.
        /// </summary>
        /// <param name="chartThemeStyle">The theme style object from the chart.</param>
        /// <returns>Resolved font size string.</returns>
        internal string GetFontSize(ChartThemeStyle chartThemeStyle)
            => !string.IsNullOrEmpty(Size) ? Size : chartThemeStyle?.CrosshairTextSize ?? string.Empty;

        /// <summary>
        /// Resolves the font weight using component value or theme defaults.
        /// </summary>
        /// <param name="chartThemeStyle">The theme style object from the chart.</param>
        /// <returns>Resolved font weight string.</returns>
        internal string GetFontWeight(ChartThemeStyle chartThemeStyle)
            => !string.IsNullOrEmpty(FontWeight) ? FontWeight : chartThemeStyle?.CrosshairFontWeight ?? string.Empty;

        /// <summary>
        /// Resolves the font family using component value or theme defaults.
        /// </summary>
        /// <param name="chartThemeStyle">The theme style object from the chart.</param>
        /// <returns>Resolved font family string.</returns>
        internal string GetFontFamily(ChartThemeStyle chartThemeStyle)
            => !string.IsNullOrEmpty(FontFamily) ? FontFamily : chartThemeStyle?.CrosshairFontFamily ?? string.Empty;

        /// <summary>
        /// Resolves the font color using component value or theme defaults.
        /// </summary>
        /// <param name="chartThemeStyle">The theme style object from the chart.</param>
        /// <returns>Resolved font color string.</returns>
        internal string GetFontColor(ChartThemeStyle chartThemeStyle)
            => !string.IsNullOrEmpty(Color) ? Color : chartThemeStyle?.CrosshairLabel ?? string.Empty;

        /// <summary>
        /// Builds a <see cref="ChartFontOptions"/> instance from current values and theme defaults.
        /// </summary>
        /// <param name="chartThemeStyle">The theme style object from the chart.</param>
        /// <returns>A fully resolved <see cref="ChartFontOptions"/> value object.</returns>
        internal ChartFontOptions GetFontOptions(ChartThemeStyle chartThemeStyle)
            => new ChartFontOptions
            {
                Color = GetFontColor(chartThemeStyle),
                Size = GetFontSize(chartThemeStyle),
                FontFamily = GetFontFamily(chartThemeStyle),
                FontWeight = GetFontWeight(chartThemeStyle),
                FontStyle = FontStyle
            };

        #endregion
    }
}