using Microsoft.AspNetCore.Components;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides options to customize the tooltip.
    /// </summary>
    /// <remarks>
    /// This component controls tooltip behavior (visibility, animations, shared/nearest tooltip modes)
    /// and appearance (fill, opacity, border, text style and templates).
    /// </remarks>
    public class ChartTooltipSettings : ChartSubComponent
    {
        #region Fields

        // Backing fields to avoid unnecessary re-renders and to apply side effects only when values change.
        private bool _enableHighlight;
        private bool _enable;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the parent chart to which this tooltip configuration applies.
        /// </summary>
        /// <value>
        /// The <see cref="SfChart"/> instance if available.
        /// </value>
        [CascadingParameter]
        private SfChart? Parent { get; set; }

        /// <summary>
        /// Gets or sets an instance of <see cref="ChartTooltipBorder"/> which controls the customization of the border of the tooltip.
        /// </summary>
        /// <value>
        /// An instance of <see cref="ChartTooltipBorder"/>.
        /// </value>
        /// <remarks>
        /// Use this to customize the color and width of the tooltip border.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartTooltipSettings Enable="true">
        ///     <ChartTooltipBorder Width="2" Color="tomato" />
        /// </ChartTooltipSettings>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ChartTooltipBorder Border { get; set; } = new ChartTooltipBorder();

        /// <summary>
        /// Gets or sets the duration for the tooltip animation in milliseconds.
        /// </summary>
        /// <value>
        /// The animation duration in milliseconds. Default is <b>300</b>.
        /// </value>
        /// <remarks>
        /// Effective only when <see cref="EnableAnimation"/> is <c>true</c>.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartTooltipSettings Enable="true" Duration="600" EnableAnimation="true" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double Duration { get; set; } = 300;

        /// <summary>
        /// Gets or sets a value indicating whether the tooltip is enabled.
        /// </summary>
        /// <value>
        /// <b>true</b> to enable tooltip; otherwise, <b>false</b>. Default is <b>false</b>.
        /// </value>
        /// <remarks>
        /// When enabled, the chart displays tooltip content on interaction.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartTooltipSettings Enable="true" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool Enable { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the tooltip animates while moving from one point to another.
        /// </summary>
        /// <value>
        /// <b>true</b> to enable animation; otherwise, <b>false</b>. Default is <b>true</b>.
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartTooltipSettings Enable="true" EnableAnimation="false" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool EnableAnimation { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to enable highlighting when hovered over the point element.
        /// </summary>
        /// <value>
        /// <b>true</b> to enable highlight; otherwise, <b>false</b>. Default is <b>false</b>.
        /// </value>
        /// <remarks>
        /// When enabled, the hovered point's series can be visually emphasized.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartTooltipSettings Enable="true" EnableHighlight="true" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool EnableHighlight { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to display a marker in the tooltip.
        /// </summary>
        /// <value>
        /// <b>true</b> to display marker; otherwise, <b>false</b>. Default is <b>true</b>.
        /// </value>
        /// <remarks>
        /// If the series marker is not enabled, a circle-shaped marker is rendered by default.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartTooltipSettings Enable="true" EnableMarker="false" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool EnableMarker { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to wrap the tooltip text based on the available space.
        /// </summary>
        /// <value>
        /// <b>true</b> to enable text wrap; otherwise, <b>false</b>. Default is <b>false</b>.
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartTooltipSettings Enable="true" EnableTextWrap="true" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool EnableTextWrap { get; set; }

        /// <summary>
        /// Gets or sets the duration for fading out the tooltip.
        /// </summary>
        /// <value>
        /// The fade-out duration in milliseconds. Default is <b>1000</b>.
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartTooltipSettings Enable="true" FadeOutDuration="0" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double FadeOutDuration { get; set; } = 1000;

        /// <summary>
        /// Gets or sets the fill color of the tooltip background.
        /// </summary>
        /// <value>
        /// CSS color string for the tooltip background fill. Default is theme-specific (e.g., <b>#000816</b> for Fluent).
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartTooltipSettings Enable="true" Fill="#0b1220" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string Fill { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the format for the tooltip content.
        /// </summary>
        /// <value>
        /// Format string for tooltip text. Default is empty string (auto).
        /// </value>
        /// <remarks>
        /// Use tokens like <c>${series.name}</c>, <c>${point.x}</c>, <c>${point.y}</c> to include series/point data.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartTooltipSettings Enable="true" Format="<b>${series.name}</b> : ${point.y}" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string Format { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the header text for the tooltip.
        /// </summary>
        /// <value>
        /// The tooltip header text. Default is empty.
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartTooltipSettings Enable="true" Header="Sales (USD)" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string Header { get; set; } = null!;

        /// <summary>
        /// Gets or sets the opacity of the tooltip.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> representing tooltip opacity. Default is <b>0.75</b>.
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartTooltipSettings Enable="true" Opacity="0.85" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double Opacity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a single tooltip should be displayed with shared values of multiple series.
        /// </summary>
        /// <value>
        /// <b>true</b> to show shared tooltip; otherwise, <b>false</b>. Default is <b>false</b>.
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartTooltipSettings Enable="true" Shared="true" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool Shared { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the values of the nearest points for all visible series should be shown in a shared tooltip.
        /// </summary>
        /// <value>
        /// <b>true</b> to show the nearest points; otherwise, <b>false</b>. Default is <b>true</b>.
        /// </value>
        /// <remarks>
        /// Applicable only when <see cref="Shared"/> is <c>true</c>.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartTooltipSettings Enable="true" Shared="true" ShowNearestPoint="true" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool ShowNearestPoint { get; set; } = true;

        /// <summary>
        /// Gets or sets a value that determines whether the header line is displayed in the tooltip.
        /// </summary>
        /// <value>
        /// <b>true</b> to show header line; otherwise, <b>false</b>. Default is <b>true</b>.
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartTooltipSettings Enable="true" ShowHeaderLine="false" Header="Gross Margin" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool ShowHeaderLine { get; set; } = true;

        /// <summary>
        /// Gets or sets a custom template to format the tooltip content.
        /// </summary>
        /// <value>
        /// A template (<see cref="RenderFragment{Object}"/>) used to render custom tooltip content, or <c>null</c> for default.
        /// </value>
        /// <remarks>
        /// Template context is a tooltip info object (single or list based on <see cref="Shared"/>).
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartTooltipSettings Enable="true" Shared="true">
        ///     <Template>
        ///         @{
        ///             var items = context as List<ChartTooltipInfo>;
        ///         }
        ///         <div style="padding:4px 8px">
        ///             <div><b>@items[0].X</b></div>
        ///             @foreach (var i in items)
        ///             {
        ///                 <div>@i.Series.Name : @i.Y</div>
        ///             }
        ///         </div>
        ///     </Template>
        /// </ChartTooltipSettings>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public RenderFragment<object> Template { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value that determines whether tooltips are displayed for only the nearest data point to the cursor.
        /// </summary>
        /// <value>
        /// <b>true</b> to show only the nearest tooltip; otherwise, <b>false</b>. Default is <b>false</b>.
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartTooltipSettings Enable="true" ShowNearestTooltip="true" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool ShowNearestTooltip { get; set; }

        /// <summary>
        /// Gets or sets an instance of <see cref="ChartTooltipTextStyle"/> which controls the customization of the text style of the tooltip.
        /// </summary>
        /// <value>
        /// The text style settings for the tooltip.
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartTooltipSettings Enable="true">
        ///     <ChartTooltipTextStyle Size="13px" FontFamily="Inter, Roboto, Arial" Color="#e5e7eb" FontWeight="600" />
        /// </ChartTooltipSettings>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ChartTooltipTextStyle TextStyle { get; set; } = new ChartTooltipTextStyle();

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Initializes the tooltip and registers it with the parent chart.
        /// </summary>
        /// <remarks>
        /// Called by the framework when the component is initialized.
        /// </remarks>
        /// <inheritdoc cref="ComponentBase.OnInitialized" />
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Parent is null)
            {
                return;
            }
            Parent._tooltip = this;
        }

        /// <summary>
        /// Releases references to avoid memory leaks.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override ValueTask DisposeAsyncCore()
        {
            Parent = null;
            ChildContent = null!;
            Border = null!;
            TextStyle = null!;
            return base.DisposeAsyncCore();
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

                // Scatter and Bubble require data update on first render toggle.
                if (Parent is { _isChartFirstRender: true })
                {
                    Parent.TriggerDataUpdateForScatterAndBubbleChart();
                }
            }

            if (_enableHighlight != EnableHighlight)
            {
                _enableHighlight = EnableHighlight;

                if (Parent is not null)
                {
                    _ = InvokeVoidAsync(Parent._chartJsModule!, Parent._chartJsInProcessModule!, "setEnableHighlight", [_enableHighlight, Parent._dataId]);
                }
            }

        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Updates nested tooltip sub-component properties (e.g., Border, TextStyle).
        /// </summary>
        /// <param name="key">Property key being updated (e.g., "Border" or "TextStyle").</param>
        /// <param name="keyValue">The new value to apply.</param>
        internal void UpdateTooltipProperties(string key, object keyValue)
        {
            switch (key)
            {
                case nameof(Border):
                    Border = (ChartTooltipBorder)keyValue;
                    break;
                case nameof(TextStyle):
                    TextStyle = (ChartTooltipTextStyle)keyValue;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Creates a serializable snapshot of the tooltip settings for interop.
        /// </summary>
        /// <returns>A shallow copy of <see cref="ChartTooltipSettings"/> suitable for JS interop.</returns>
        internal ChartTooltipSettings GetTooltipForScript()
        {
            return new ChartTooltipSettings
            {
                Enable = Enable,
                Shared = Shared,
                EnableHighlight = EnableHighlight,
                EnableAnimation = EnableAnimation,
                EnableMarker = EnableMarker,
                EnableTextWrap = EnableTextWrap,
                Duration = Duration,
                FadeOutDuration = FadeOutDuration,
                Fill = Fill,
                ShowHeaderLine = ShowHeaderLine,
                ShowNearestPoint = ShowNearestPoint,
                ShowNearestTooltip = ShowNearestTooltip,
                Header = Header,
                Format = Format,
                Opacity = Opacity,
                Border = Border,
                TextStyle = TextStyle,
            };
        }

        #endregion
    }
}
