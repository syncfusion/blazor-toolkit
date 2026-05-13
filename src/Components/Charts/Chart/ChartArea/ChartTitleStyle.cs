using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Charts.Internal;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides options to customize the title style of the chart.
    /// </summary>
    public class ChartTitleStyle : ChartDefaultFont
    {
        #region Fields
        private ChartTitlePosition _position = ChartTitlePosition.Top;
        #endregion

        #region Properties

        /// <summary>
        /// Gets the parent chart component that owns this title style.
        /// </summary>
        /// <value>The parent <see cref="SfChart"/> component, or <see langword="null"/> if not cascaded.</value>
        [CascadingParameter]
        internal SfChart? Owner { get; set; }

        /// <summary> 
        /// Gets or sets the font size of the chart title. 
        /// </summary> 
        /// <value>
        /// A string representing the font size of the chart title. The default size is determined by the <see cref="SfChart">Chart's</see> theme. By default, the theme is set to Fluent with a title text size of <b>16px</b>.
        /// </value>
        /// <remarks>
        /// Use the <see cref="Size"/> property to change the font size of title text in the <see cref="SfChart">Chart</see>.
        /// The size value accepts standard CSS font size specifications such as 'px', 'em', etc.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a custom font size for the title text:
        /// <SfChart Title="Chart">
        ///     <ChartTitleStyle Size="20px" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override string Size { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets the font weight of the chart title. 
        /// </summary> 
        /// <value> 
        /// A string representing the font weight of the title text. The default font weight is determined by the <see cref="SfChart">Chart's</see> theme. By default, the theme is set to Fluent with title font weight of <b>600</b>.
        /// </value> 
        /// <remarks>
        /// The font weight can be a number (100 to 900), or a keyword such as 'normal', 'bold', 'bolder', or 'lighter'.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a custom font weight for the title text:
        /// <SfChart Title="Chart">
        ///     <ChartTitleStyle FontWeight="400" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override string FontWeight { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets the font family of the chart title. 
        /// </summary> 
        /// <value> 
        /// A string representing the font family of the chart title. The default font family is determined by the chart's theme. By default, the theme is set to Fluent with the font family <b>Roboto</b>.
        /// </value> 
        /// <remarks>
        /// This property allows customization of the font family for the chart title, providing flexibility in text appearance and style.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a custom font family for the title text:
        /// <SfChart Title="Chart">
        ///     <ChartTitleStyle FontFamily="italic" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override string FontFamily { get; set; } = string.Empty;

        /// <summary>
        /// Defines the position of the chart title and subtitle.
        /// </summary>
        /// <value>
        /// An enum value representing the position of the title. The default value is 'Top'.
        /// </value>
        /// <remarks>
        /// Available options:
        /// <list type="bullet">
        ///   <item>
        ///     <term>Top</term>
        ///     <description>Displays the title and subtitle at the top of the chart.</description>
        ///   </item>
        ///   <item>
        ///     <term>Left</term>
        ///     <description>Displays the title and subtitle on the left side of the chart.</description>
        ///   </item>
        ///   <item>
        ///     <term>Bottom</term>
        ///     <description>Displays the title and subtitle at the bottom of the chart.</description>
        ///   </item>
        ///   <item>
        ///     <term>Right</term>
        ///     <description>Displays the title and subtitle on the right side of the chart.</description>
        ///   </item>
        ///   <item>
        ///     <term>Custom</term>
        ///     <description>Displays the title and subtitle based on the specified X and Y coordinates.</description>
        ///   </item>
        /// </list>
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting the position of the title.
        /// <SfChart Title="Sales Data" SubTitle="2023 Overview">
        ///     <ChartTitleStyle Position="ChartTitlePosition.Right" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ChartTitlePosition Position { get; set; } = ChartTitlePosition.Top;

        /// <summary>
        /// Defines the X coordinate for the chart title and subtitle.
        /// </summary>
        /// <value>
        /// A double value representing the horizontal position. The default value is <b>0</b>.
        /// </value>
        /// <remarks>
        /// This property is applicable only when the position is set to <b>Custom</b>.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting the X coordinate for the chart title.
        /// <SfChart Title="Sales Data" SubTitle="2023 Overview">
        ///     <ChartTitleStyle Position="ChartTitlePosition.Custom" X="40" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double X { get; set; }

        /// <summary>
        /// Defines the Y coordinate for the chart title and subtitle.
        /// </summary>
        /// <value>
        /// A double value representing the vertical position. The default value is <b>0</b>.
        /// </value>
        /// <remarks>
        /// This property is applicable only when the position is set to <b>Custom</b>.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting the Y coordinate for the chart title.
        /// <SfChart Title="Sales Data" SubTitle="2023 Overview">
        ///     <ChartTitleStyle Position="ChartTitlePosition.Custom" Y="40" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double Y { get; set; }

        /// <summary>
        /// Gets or sets the accessibility description for the chart title.
        /// </summary>
        /// <value>
        /// Accepts a string that defines the accessibility description for the chart title. The default value is an empty string.
        /// </value>
        /// <remarks>
        /// Use this property to provide an accessibility description for screen readers and other assistive technologies.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting an accessibility description for the chart title:
        /// <SfChart Title="Chart">
        ///     <ChartTitleStyle AccessibilityDescription="This is the chart title" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string AccessibilityDescription { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the accessibility role for the chart title.
        /// </summary>
        /// <value>
        /// Accepts a string that defines the accessibility role for the chart title. The default value is <b>null</b>.
        /// </value>
        /// <remarks>
        /// Use this property to provide an accessibility role for the chart title.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting an accessibility role for the chart title:
        /// <SfChart Title="Chart">
        ///     <ChartTitleStyle AccessibilityRole="heading" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string AccessibilityRole { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the accessibility keyboard navigation focus option for the chart title.
        /// </summary>
        /// <value>
        /// Accepts the boolean value to enable or disable the keyboard navigation for the chart title. The default value is <b>true</b>.
        /// </value>
        /// <remarks>
        /// Use this property to toggle the keyboard navigation focus for the chart title.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting the keyboard navigation focus for the chart title:
        /// <SfChart Title="Chart">
        ///     <ChartTitleStyle Focusable="false" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool Focusable { get; set; } = true;

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Initializes the component and sets up the title style on the parent chart renderer.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Owner?._chartTitleRenderer is not null)
            {
                Owner._chartTitleRenderer.TitleStyle = this;
            }
        }

        /// <summary>
        /// Handles parameter changes and notifies the renderer if changes require re-rendering.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (_position != Position)
            {
                _position = Position;
                if (Owner is not null && Owner._chartTitleRenderer is not null)
                {
                    Owner._chartTitleRenderer.RendererShouldRender = true;
                }
            }
            if (Owner?._chartTitleRenderer is not null)
            {
                Owner._chartTitleRenderer.RendererShouldRender = true;
                Owner._chartTitleRenderer.ProcessRenderQueue();
            }
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Retrieves the effective font size for the title, falling back to the theme default if not set.
        /// </summary>
        /// <param name="chartThemeStyle">The chart's theme style settings.</param>
        /// <returns>The computed font size string.</returns>
        internal string GetFontSize(ChartThemeStyle chartThemeStyle)
        {
            return !string.IsNullOrEmpty(Size) ? Size : chartThemeStyle?.ChartTitleSize ?? string.Empty;
        }

        /// <summary>
        /// Retrieves the effective font weight for the title, falling back to the theme default if not set.
        /// </summary>
        /// <param name="chartThemeStyle">The chart's theme style settings.</param>
        /// <returns>The computed font weight string.</returns>
        internal string GetFontWeight(ChartThemeStyle chartThemeStyle)
        {
            return !string.IsNullOrEmpty(FontWeight) ? FontWeight : chartThemeStyle?.ChartTitleFontWeight ?? string.Empty;
        }

        /// <summary>
        /// Retrieves the effective font family for the title, falling back to the theme default if not set.
        /// </summary>
        /// <param name="chartThemeStyle">The chart's theme style settings.</param>
        /// <returns>The computed font family string.</returns>
        internal string GetFontFamily(ChartThemeStyle chartThemeStyle)
        {
            return !string.IsNullOrEmpty(FontFamily) ? FontFamily : chartThemeStyle?.ChartTitleFontFamily ?? string.Empty;
        }

        /// <summary>
        /// Compiles all font styling properties into a single <see cref="ChartFontOptions"/> object.
        /// </summary>
        /// <param name="chartThemeStyle">The chart's theme style settings.</param>
        /// <returns>A <see cref="ChartFontOptions"/> instance with computed values.</returns>
        internal ChartFontOptions GetFontOptions(ChartThemeStyle chartThemeStyle)
        {
            return new ChartFontOptions
            {
                Color = Color,
                Size = GetFontSize(chartThemeStyle),
                FontFamily = GetFontFamily(chartThemeStyle),
                FontWeight = GetFontWeight(chartThemeStyle),
                FontStyle = FontStyle,
                TextOverflow = TextOverflow
            };
        }
        #endregion
    }
}
