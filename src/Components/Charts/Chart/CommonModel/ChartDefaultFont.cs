using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Charts.Internal;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides comprehensive options for customizing font properties used throughout the chart component.
    /// </summary>
    public class ChartDefaultFont : ChartSubComponent, IChartDefaultFont
    {
        #region Properties

        /// <summary> 
        /// Gets or sets the color for the text. 
        /// </summary> 
        /// <value> 
        /// A string representing the color for the text. The default value is <b>null</b>. 
        /// </value> 
        /// <remarks> 
        /// Accepts values in hex and rgba as a valid CSS color string. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a custom font color for the text.
        /// <SfChart Title="Olympic Medals" SubTitle="In the year 2023">
        ///     <ChartTitleStyle Color="black"></ChartTitleStyle>
        ///     <ChartSubTitleStyleColor="blue"></ChartSubTitleStyle>
        ///     <ChartPrimaryXAxis Title="Country" ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category">
        ///         <ChartAxisCrosshairTooltip Enable="true">
        ///             <ChartCrosshairTextStyle Color="red"></ChartCrosshairTextStyle>
        ///         </ChartAxisCrosshairTooltip>
        ///         <ChartAxisLabelStyle Color="green"></ChartAxisLabelStyle>
        ///         <ChartAxisTitleStyle Color="red"></ChartAxisTitleStyle>
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis>
        ///         <ChartStriplines>
        ///             <ChartStripline Start="20" End="25" Color="red">
        ///                 <ChartStriplineTextStyle Color="red"/>
        ///             </ChartStripline>
        ///         </ChartStriplines>
        ///     </ChartPrimaryYAxis>
        ///     <ChartCrosshairSettings Enable="true"></ChartCrosshairSettings>
        ///     <ChartTooltipSettings Enable="true">
        ///         <ChartTooltipTextStyle Color="green"></ChartTooltipTextStyle>
        ///     </ChartTooltipSettings>
        ///     <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" Type="ChartSeriesType.Column">
        ///         <ChartMarker>
        ///             <ChartDataLabel Visible="true">
        ///                 <ChartDataLabelFont Color="yellow"></ChartDataLabelFont>
        ///             </ChartDataLabel>
        ///         </ChartMarker>
        ///     </ChartSeries>
        ///     <ChartLegendSettings Visible="true">
        ///         <ChartLegendTextStyle Color="blue"></ChartLegendTextStyle>
        ///      </ChartLegendSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public virtual string Color { get; set; } = null!;

        /// <summary> 
        /// Gets or sets the font family for the text. 
        /// </summary> 
        /// <value> 
        /// A string representing the font family for the text. The default value is <b>"Segoe UI"</b>. 
        /// </value> 
        /// <remarks>
        /// This property specifies the font face used in rendering the text, offering a wide variety of standard and custom fonts.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a custom font family for the text.
        /// <SfChart Title="Olympic Medals" SubTitle="In the year 2023">
        ///     <ChartTitleStyle FontFamily="Arial"></ChartTitleStyle>
        ///     <ChartSubTitleStyle FontFamily="Arial"></ChartSubTitleStyle>
        ///     <ChartPrimaryXAxis Title="Country" ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category">
        ///         <ChartAxisCrosshairTooltip Enable="true">
        ///             <ChartCrosshairTextStyle FontFamily="Arial"></ChartCrosshairTextStyle>
        ///         </ChartAxisCrosshairTooltip>
        ///         <ChartAxisLabelStyle FontFamily="Arial"></ChartAxisLabelStyle>
        ///         <ChartAxisTitleStyle FontFamily="Arial"></ChartAxisTitleStyle>
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis>
        ///         <ChartStriplines>
        ///             <ChartStripline Start="20" End="25" Color="red">
        ///                 <ChartStriplineTextStyle FontFamily="Arial" />
        ///             </ChartStripline>
        ///         </ChartStriplines>
        ///     </ChartPrimaryYAxis>
        ///     <ChartCrosshairSettings Enable="true"></ChartCrosshairSettings>
        ///     <ChartTooltipSettings Enable="true">
        ///         <ChartTooltipTextStyle FontFamily="Arial"></ChartTooltipTextStyle>
        ///     </ChartTooltipSettings>
        ///     <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" Type="ChartSeriesType.Column">
        ///         <ChartMarker>
        ///             <ChartDataLabel Visible="true">
        ///                 <ChartDataLabelFont FontFamily="Arial"></ChartDataLabelFont>
        ///             </ChartDataLabel>
        ///         </ChartMarker>
        ///     </ChartSeries>
        ///     <ChartLegendSettings Visible="true">
        ///         <ChartLegendTextStyle FontFamily="Arial"></ChartLegendTextStyle>
        ///      </ChartLegendSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public virtual string FontFamily { get; set; } = "Segoe UI";

        /// <summary> 
        /// Gets or sets the font style for the text. 
        /// </summary> 
        /// <value> 
        /// A string representing the font style for the text. The default value is <b>"Normal"</b>. 
        /// </value> 
        /// <remarks>
        /// The font style can be used to apply settings like Italic or Oblique to the text, affecting its emphasis and legibility.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a custom font style for the text.
        /// <SfChart Title="Olympic Medals" SubTitle="In the year 2023">
        ///     <ChartTitleStyle FontStyle="bold"></ChartTitleStyle>
        ///     <ChartSubTitleStyle FontStyle="bold"></ChartSubTitleStyle>
        ///     <ChartPrimaryXAxis Title="Country" ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category">
        ///         <ChartAxisCrosshairTooltip Enable="true">
        ///             <ChartCrosshairTextStyle FontStyle="bold"></ChartCrosshairTextStyle>
        ///         </ChartAxisCrosshairTooltip>
        ///         <ChartAxisLabelStyle FontStyle="bold"></ChartAxisLabelStyle>
        ///         <ChartAxisTitleStyle FontStyle="bold"></ChartAxisTitleStyle>
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis>
        ///         <ChartStriplines>
        ///             <ChartStripline Start="20" End="25" Color="red">
        ///                 <ChartStriplineTextStyle FontStyle="bold"/>
        ///             </ChartStripline>
        ///         </ChartStriplines>
        ///     </ChartPrimaryYAxis>
        ///     <ChartCrosshairSettings Enable="true"></ChartCrosshairSettings>
        ///     <ChartTooltipSettings Enable="true">
        ///         <ChartTooltipTextStyle FontStyle="bold"></ChartTooltipTextStyle>
        ///     </ChartTooltipSettings>
        ///     <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" Type="ChartSeriesType.Column">
        ///         <ChartMarker>
        ///             <ChartDataLabel Visible="true">
        ///                 <ChartDataLabelFont FontStyle="bold"></ChartDataLabelFont>
        ///             </ChartDataLabel>
        ///         </ChartMarker>
        ///     </ChartSeries>
        ///     <ChartLegendSettings Visible="true">
        ///         <ChartLegendTextStyle FontStyle="bold"></ChartLegendTextStyle>
        ///      </ChartLegendSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string FontStyle { get; set; } = "Normal";

        /// <summary> 
        /// Gets or sets the font weight for the text. 
        /// </summary> 
        /// <value> 
        /// A string representing the font weight for the text. The default value is <b>"Normal"</b>. 
        /// </value> 
        /// <remarks>
        /// Use this property to define the thickness of the text characters, providing levels of boldness.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a custom font weight for the text.
        /// <SfChart Title="Olympic Medals" SubTitle="In the year 2023">
        ///     <ChartTitleStyle FontWeight="600"></ChartTitleStyle>
        ///     <ChartSubTitleStyle FontWeight="600"></ChartSubTitleStyle>
        ///     <ChartPrimaryXAxis Title="Country" ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category">
        ///         <ChartAxisCrosshairTooltip Enable="true">
        ///             <ChartCrosshairTextStyle FontWeight="600"></ChartCrosshairTextStyle>
        ///         </ChartAxisCrosshairTooltip>
        ///         <ChartAxisLabelStyle FontWeight="600"></ChartAxisLabelStyle>
        ///         <ChartAxisTitleStyle FontWeight="600"></ChartAxisTitleStyle>
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis>
        ///         <ChartStriplines>
        ///             <ChartStripline Start="20" End="25" Color="red">
        ///                 <ChartStriplineTextStyle FontWeight="600" />
        ///             </ChartStripline>
        ///         </ChartStriplines>
        ///     </ChartPrimaryYAxis>
        ///     <ChartCrosshairSettings Enable="true"></ChartCrosshairSettings>
        ///     <ChartTooltipSettings Enable="true">
        ///         <ChartTooltipTextStyle FontWeight="600"></ChartTooltipTextStyle>
        ///     </ChartTooltipSettings>
        ///     <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" Type="ChartSeriesType.Column">
        ///         <ChartMarker>
        ///            <ChartDataLabel Visible="true">
        ///                 <ChartDataLabelFont FontWeight="600"></ChartDataLabelFont>
        ///             </ChartDataLabel>
        ///         </ChartMarker>
        ///     </ChartSeries>
        ///     <ChartLegendSettings Visible="true">
        ///         <ChartLegendTextStyle FontWeight="600"></ChartLegendTextStyle>
        ///      </ChartLegendSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public virtual string FontWeight { get; set; } = "Normal";

        /// <summary> 
        /// Gets or sets the opacity for the text. 
        /// </summary> 
        /// <value> 
        /// A double value representing the opacity for the text. The default value is <b>1</b>. 
        /// </value> 
        /// <remarks>
        /// The opacity determines the transparency level of the text, where 1 is completely opaque, and 0 is fully transparent.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a custom font opacity for the text.
        /// <SfChart Title="Olympic Medals" SubTitle="In the year 2023">
        ///     <ChartTitleStyle Opacity=0.5></ChartTitleStyle>
        ///     <ChartSubTitleStyle Opacity=0.5></ChartSubTitleStyle>
        ///     <ChartPrimaryXAxis Title="Country" ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category">
        ///         <ChartAxisCrosshairTooltip Enable="true">
        ///             <ChartCrosshairTextStyle Opacity=0.5></ChartCrosshairTextStyle>
        ///         </ChartAxisCrosshairTooltip>
        ///         <ChartAxisLabelStyle Opacity=0.5></ChartAxisLabelStyle>
        ///         <ChartAxisTitleStyle Opacity=0.5></ChartAxisTitleStyle>
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis>
        ///         <ChartStriplines>
        ///             <ChartStripline Start="20" End="25" Color="red">
        ///                 <ChartStriplineTextStyle Opacity=0.5 />
        ///             </ChartStripline>
        ///         </ChartStriplines>
        ///     </ChartPrimaryYAxis>
        ///     <ChartCrosshairSettings Enable="true"></ChartCrosshairSettings>
        ///     <ChartTooltipSettings Enable="true">
        ///         <ChartTooltipTextStyle Opacity=0.5></ChartTooltipTextStyle>
        ///     </ChartTooltipSettings>
        ///     <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" Type="ChartSeriesType.Column">
        ///         <ChartMarker>
        ///             <ChartDataLabel Visible="true">
        ///                 <ChartDataLabelFont Opacity=0.5></ChartDataLabelFont>
        ///             </ChartDataLabel>
        ///         </ChartMarker>
        ///     </ChartSeries>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double Opacity { get; set; } = 1;

        /// <summary> 
        /// Gets or sets the font size for the text. 
        /// </summary> 
        /// <value> 
        /// A string representing the font size for the text. The default value is <b>"16px"</b>. 
        /// </value> 
        /// <remarks>
        /// This property determines the size of the text, impacting its readability within the chart component context.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a custom font size for the text.
        /// <SfChart Title="Olympic Medals" SubTitle="In the year 2023">
        ///     <ChartTitleStyle Size="16px"></ChartTitleStyle>
        ///     <ChartSubTitleStyle Size="16px"></ChartSubTitleStyle>
        ///     <ChartPrimaryXAxis Title="Country" ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category">
        ///         <ChartAxisCrosshairTooltip Enable="true">
        ///             <ChartCrosshairTextStyle Size="16px"></ChartCrosshairTextStyle>
        ///         </ChartAxisCrosshairTooltip>
        ///         <ChartAxisLabelStyle Size="13px"></ChartAxisLabelStyle>
        ///         <ChartAxisTitleStyle Size="16px"></ChartAxisTitleStyle>
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis>
        ///         <ChartStriplines>
        ///             <ChartStripline Start="20" End="25" Color="red">
        ///                 <ChartStriplineTextStyle Size="20px"/>
        ///             </ChartStripline>
        ///         </ChartStriplines>
        ///     </ChartPrimaryYAxis>
        ///     <ChartCrosshairSettings Enable="true"></ChartCrosshairSettings>
        ///     <ChartTooltipSettings Enable="true">
        ///         <ChartTooltipTextStyle Size="16px"></ChartTooltipTextStyle>
        ///     </ChartTooltipSettings>
        ///     <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" Type="ChartSeriesType.Column">
        ///         <ChartMarker>
        ///             <ChartDataLabel Visible="true">
        ///                 <ChartDataLabelFont Size="16px"></ChartDataLabelFont>
        ///             </ChartDataLabel>
        ///         </ChartMarker>
        ///     </ChartSeries>
        ///     <ChartLegendSettings Visible="true">
        ///         <ChartLegendTextStyle Size="14px"></ChartLegendTextStyle>
        ///      </ChartLegendSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public virtual string Size { get; set; } = "16px";

        /// <summary> 
        /// Gets or sets the text alignment. 
        /// </summary> 
        /// <value> 
        /// One of the <see cref="Alignment"/> enumerations that specifies the alignment of the text. 
        /// The options include: 
        /// <list type="bullet">
        ///     <item><description><c>Near</c> - Aligns the text to the start of the container.</description></item>
        ///     <item><description><c>Far</c> - Aligns the text to the far end of the container.</description></item>
        ///     <item><description><c>Center</c> - Aligns the text to the center of the container.</description></item>
        /// </list>
        /// <br/>
        /// The default value is <b>Alignment.Center</b>. 
        /// </value> 
        /// <remarks>
        /// Text alignment adjusts the horizontal placement of text within its container, affecting its position relative to other content.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a text alignment for the subtitle text.
        /// <SfChart Title="Olympic Medals" SubTitle="In the year 2023">
        ///     <ChartTitleStyle TextAlignment="Alignment.Far"></ChartTitleStyle>
        ///     <ChartSubTitleStyle TextAlignment="Alignment.Far"></ChartSubTitleStyle>
        ///     <ChartPrimaryXAxis Title="Country" ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category">
        ///         <ChartAxisCrosshairTooltip Enable="true">
        ///             <ChartCrosshairTextStyle TextAlignment="Alignment.Far"></ChartCrosshairTextStyle>
        ///         </ChartAxisCrosshairTooltip>
        ///         <ChartAxisLabelStyle TextAlignment="Alignment.Far"></ChartAxisLabelStyle>
        ///         <ChartAxisTitleStyle TextAlignment="Alignment.Far"></ChartAxisTitleStyle>
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis>
        ///         <ChartStriplines>
        ///             <ChartStripline Start="20" End="25" Color="red">
        ///                 <ChartStriplineTextStyle TextAlignment="Alignment.Far"/>
        ///             </ChartStripline>
        ///         </ChartStriplines>
        ///     </ChartPrimaryYAxis>
        ///     <ChartCrosshairSettings Enable="true"></ChartCrosshairSettings>
        ///     <ChartTooltipSettings Enable="true">
        ///         <ChartTooltipTextStyle TextAlignment="Alignment.Far"></ChartTooltipTextStyle>
        ///     </ChartTooltipSettings>
        ///     <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" Type="ChartSeriesType.Column">
        ///         <ChartMarker>
        ///            <ChartDataLabel Visible="true">
        ///                 <ChartDataLabelFont TextAlignment="Alignment.Far"></ChartDataLabelFont>
        ///             </ChartDataLabel>
        ///         </ChartMarker>
        ///     </ChartSeries>
        ///     <ChartLegendSettings Visible="true">
        ///         <ChartLegendTextStyle TextAlignment="Alignment.Far"></ChartLegendTextStyle>
        ///      </ChartLegendSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public virtual Alignment TextAlignment { get; set; } = Alignment.Center;

        /// <summary> 
        /// Gets or sets the text overflow behavior to employ when the text exceeds the defined margins. 
        /// </summary> 
        /// <value> 
        /// One of the <see cref="TextOverflow"/> enumeration that specifies the text overflow options. 
        /// The options include: 
        /// - <c>TextOverflow.Trim</c>: Trims the text if it exceeds the defined margins. 
        /// - <c>TextOverflow.Wrap</c>: Wraps the text if it exceeds the defined margins. 
        /// - <c>TextOverflow.None</c>: Shows the text as it is. 
        /// <br/>
        /// The default value is <b>TextOverflow.Trim</b>. 
        /// </value> 
        /// <remarks>
        /// This property manages how text should behave when it's too long for its allocated space, either by trimming, wrapping, or displaying fully.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a text alignment for the subtitle text.
        /// <SfChart Title="Olympic Medals" SubTitle="In the year 2023">
        ///     <ChartTitleStyle TextOverflow="TextOverflow.None"></ChartTitleStyle>
        ///     <ChartSubTitleStyle TextOverflow="TextOverflow.None"></ChartSubTitleStyle>
        ///     <ChartPrimaryXAxis Title="Country" ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category">
        ///         <ChartAxisCrosshairTooltip Enable="true">
        ///             <ChartCrosshairTextStyle TextOverflow="TextOverflow.None"></ChartCrosshairTextStyle>
        ///         </ChartAxisCrosshairTooltip>
        ///         <ChartAxisLabelStyle TextOverflow="TextOverflow.None"></ChartAxisLabelStyle>
        ///         <ChartAxisTitleStyle TextOverflow="TextOverflow.None"></ChartAxisTitleStyle>
        ///     </ChartPrimaryXAxis>
        ///     <ChartPrimaryYAxis>
        ///         <ChartStriplines>
        ///             <ChartStripline Start="20" End="25" Color="red">
        ///                 <ChartStriplineTextStyle TextOverflow="TextOverflow.None"/>
        ///             </ChartStripline>
        ///         </ChartStriplines>
        ///     </ChartPrimaryYAxis>
        ///     <ChartCrosshairSettings Enable="true"></ChartCrosshairSettings>
        ///     <ChartTooltipSettings Enable="true">
        ///         <ChartTooltipTextStyle TextOverflow="TextOverflow.None"></ChartTooltipTextStyle>
        ///     </ChartTooltipSettings>
        ///     <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" Type="ChartSeriesType.Column">
        ///         <ChartMarker>
        ///             <ChartDataLabel Visible="true">
        ///                 <ChartDataLabelFont TextOverflow="TextOverflow.None"></ChartDataLabelFont>
        ///             </ChartDataLabel>
        ///         </ChartMarker>
        ///     </ChartSeries>
        ///     <ChartLegendSettings Visible="true">
        ///         <ChartLegendTextStyle TextOverflow="TextOverflow.None"></ChartLegendTextStyle>
        ///      </ChartLegendSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TextOverflow TextOverflow { get; set; } = TextOverflow.Trim;

        #endregion

        #region Internal Methods

        /// <summary>
        /// Generates a font key identifier based on font weight, style, and family.
        /// </summary>
        /// <param name="fontWeight">Default font weight to use if <see cref="FontWeight"/> is empty.</param>
        /// <param name="fontFamily">Default font family to use if <see cref="FontFamily"/> is empty.</param>
        /// <returns>A <see cref="string"/> combining font properties with underscore separators.</returns>
        internal string GetFontKey(string fontWeight, string fontFamily)
        {
            return (string.IsNullOrEmpty(FontWeight) ? fontWeight : FontWeight) + Constants.Underscore + FontStyle + Constants.Underscore + (string.IsNullOrEmpty(FontFamily) ? fontFamily : FontFamily);
        }

        /// <summary>
        /// Retrieves comprehensive chart font options including text overflow behavior.
        /// </summary>
        /// <returns>A <see cref="ChartFontOptions"/> object containing all font properties and text overflow settings.</returns>
        internal ChartFontOptions GetChartFontOptions()
        {
            return new ChartFontOptions
            {
                Color = Color,
                Size = Size,
                FontFamily = FontFamily,
                FontWeight = FontWeight,
                FontStyle = FontStyle,
                TextOverflow = TextOverflow
            };
        }

        #endregion
    }
}