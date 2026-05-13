using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Charts.Internal;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides options to customize the appearance of the stack label, including font size, color, style, weight, and font family.
    /// </summary>
    /// <remarks>
    /// When values change, only an incremental render is requested to avoid unnecessary chart re-renders.
    /// </remarks>
    public class ChartStackLabelFont : ChartDefaultFont
    {
        #region Fields

        private ChartStackLabelSettings? _stackLabel;
        private string? _previousSize;
        private string? _previousColor;
        private string? _previousFontFamily;
        private string? _previousFontWeight;
        private string? _previousFontStyle;
        private Alignment _previousTextAlignment;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the font size of the stack label.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> that represents the font size of the stack label.
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <SfChart>
        ///     <ChartSeries XName="x" YName="y" Type="ChartSeriesType.StackingColumn">
        ///     </ChartSeries>
        ///     <ChartStackLabelSettings Visible="true">
        ///         <ChartStackLabelFont Size="12px">
        ///         </ChartStackLabelFont>
        ///     </ChartStackLabelSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        /// <remarks>
        /// Use this property to specify the font size of the stack label.
        /// </remarks>
        [Parameter]
        public override string Size { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the color of the stack label.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> that represents the font color of the stack label.
        /// Accepts values in hexadecimal or <c>rgba</c> format, as valid CSS color strings.
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <SfChart>
        ///     <ChartSeries XName="x" YName="y" Type="ChartSeriesType.StackingColumn">
        ///     </ChartSeries>
        ///     <ChartStackLabelSettings Visible="true">
        ///         <ChartStackLabelFont Color="green">
        ///         </ChartStackLabelFont>
        ///     </ChartStackLabelSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        /// <remarks>
        /// This property defines the font color used for the stack labels in the chart.
        /// </remarks>
        [Parameter]
        public override string Color { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the font family of the stack label.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> that represents the font family used for the stack label.
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <SfChart>
        ///     <ChartSeries XName="x" YName="y" Type="ChartSeriesType.StackingColumn">
        ///     </ChartSeries>
        ///     <ChartStackLabelSettings Visible="true">
        ///         <ChartStackLabelFont FontFamily="Arial">
        ///         </ChartStackLabelFont>
        ///     </ChartStackLabelSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        /// <remarks>
        /// Specifies the font family applied to the stack label. You can use standard or custom fonts supported by the browser or system.
        /// </remarks>
        [Parameter]
        public override string FontFamily { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the font weight of the stack label.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> that represents the weight of the font used for the stack label.
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <SfChart>
        ///     <ChartSeries XName="x" YName="y" Type="ChartSeriesType.StackingColumn">
        ///     </ChartSeries>
        ///     <ChartStackLabelSettings Visible="true">
        ///         <ChartStackLabelFont FontWeight="600">
        ///         </ChartStackLabelFont>
        ///     </ChartStackLabelSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        /// <remarks>
        /// Use this property to define the thickness of the text characters in the stack label, allowing you to control the level of boldness.
        /// </remarks>
        [Parameter]
        public override string FontWeight { get; set; } = "Bold";

        /// <summary>
        /// Gets or sets the alignment of the stack label relative to its position on the chart.
        /// The available options are:
        /// <list type="bullet">
        ///   <item>
        ///     <description><c>Near</c>: Aligns the label to the start of the stack.</description>
        ///   </item>
        ///   <item>
        ///     <description><c>Center</c>: Aligns the label to the center of the stack.</description>
        ///   </item>
        ///   <item>
        ///     <description><c>Far</c>: Aligns the label to the end of the stack.</description>
        ///   </item>
        /// </list>
        /// </summary>
        /// <value>
        /// An enum value representing the alignment of the stack label. The default value is <c>Center</c>.
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <SfChart>
        ///     <ChartSeries XName=”x” YName=”y” Type=”ChartSeriesType.StackingColumn”>
        ///     </ChartSeries>
        ///     <ChartStackLabelSettings Visible="true">
        ///         <ChartStackLabelFont TextAlignment=”Alignment.Near”>
        ///         </ChartStackLabelFont>
        ///     </ChartStackLabelSettings >
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        /// <remarks>
        /// This property sets the alignment for the stack label relative to its position on the chart.
        /// </remarks>
        [Parameter]
        public override Alignment TextAlignment { get; set; } = Alignment.Center;

        #endregion

        #region Lifecycle Methods

        /// <exclude />
        /// <summary>
        /// Initializes the component and registers this font instance with the parent <see cref="ChartStackLabelSettings"/>.
        /// </summary>
        /// <remarks>
        /// This ensures the parent stack-label settings can track and consume this font configuration when rendering.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Tracker is ChartStackLabelSettings stackLabelSettings)
            {
                _stackLabel = stackLabelSettings;
            }

            _stackLabel?.UpdateStackLabelProperties(nameof(ChartStackLabelSettings.Font), this);
        }

        /// <exclude />
        /// <summary>
        /// Applies parameter updates and notifies the renderer if any font-related values changed since the previous render.
        /// </summary>
        /// <remarks>
        /// Re-renders are minimized by diffing <see cref="Size"/>, <see cref="Color"/>, <see cref="FontFamily"/>,
        /// <see cref="FontWeight"/>, <see cref="ChartDefaultFont.FontStyle"/>, and <see cref="TextAlignment"/>.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            _stackLabel?.UpdateStackLabelProperties(nameof(ChartStackLabelSettings.Font), this);

            if (_previousSize != Size ||
                _previousColor != Color ||
                _previousFontFamily != FontFamily ||
                _previousFontWeight != FontWeight ||
                _previousFontStyle != FontStyle ||
                _previousTextAlignment != TextAlignment)
            {
                _previousSize = Size;
                _previousColor = Color;
                _previousFontFamily = FontFamily;
                _previousFontWeight = FontWeight;
                _previousFontStyle = FontStyle;
                _previousTextAlignment = TextAlignment;

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

        #endregion

        #region Internal Methods
        /// <summary>
        /// Releases references to allow the component to be collected and avoid memory leaks.
        /// </summary>
        /// <remarks>
        /// Clears references to the parent and child content; no unmanaged resources are held.
        /// </remarks>
        internal void ComponentDispose()
        {
            _stackLabel = null;
            ChildContent = null!;
        }
        /// <summary>
        /// Returns the effective font size, falling back to the theme style when unset.
        /// </summary>
        /// <param name="chartThemeStyle">The chart theme style.</param>
        /// <returns>The effective size string.</returns>
        internal string GetFontSize(ChartThemeStyle chartThemeStyle)
        {
            return string.IsNullOrEmpty(Size) ? chartThemeStyle.DataLabelSize ?? null! : Size;
        }

        /// <summary>
        /// Returns the effective font weight, falling back to the theme style when unset.
        /// </summary>
        /// <param name="chartThemeStyle">The chart theme style.</param>
        /// <returns>The effective font weight string.</returns>
        internal string GetFontWeight(ChartThemeStyle chartThemeStyle)
        {
            return string.IsNullOrEmpty(FontWeight) ? chartThemeStyle.DataLabelFontWeight ?? null! : FontWeight;
        }

        /// <summary>
        /// Returns the effective font family, falling back to the theme style when unset.
        /// </summary>
        /// <param name="chartThemeStyle">The chart theme style.</param>
        /// <returns>The effective font family string.</returns>
        internal string GetFontFamily(ChartThemeStyle chartThemeStyle)
        {
            return string.IsNullOrEmpty(FontFamily) ? chartThemeStyle.DataLabelFontFamily ?? null! : FontFamily;
        }

        /// <summary>
        /// Creates the font options used by the renderer by combining explicit values and theme fallbacks.
        /// </summary>
        /// <param name="chartThemeStyle">The chart theme style.</param>
        /// <returns>A populated <see cref="ChartFontOptions"/> instance.</returns>
        internal ChartFontOptions GetFontOptions(ChartThemeStyle chartThemeStyle)
        {
            return new ChartFontOptions
            {
                Color = Color,
                Size = GetFontSize(chartThemeStyle),
                FontFamily = GetFontFamily(chartThemeStyle),
                FontWeight = GetFontWeight(chartThemeStyle),
                FontStyle = FontStyle,
                TextAlignment = TextAlignment
            };
        }

        #endregion
    }
}
