using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides options to customize the scrollbar of the axis.
    /// </summary>
    public class ChartCommonScrollbarSettings : ChartSubComponent
    {
        #region Fields
        private bool _enable;
        private double _pointsLength;
        private double _height = 16;
        private string _gripColor = string.Empty;
        private string _trackColor = string.Empty;
        private string _scrollbarColor = string.Empty;
        private ChartAxisScrollbarSettingsRange ScrollRange { get; set; } = new ChartAxisScrollbarSettingsRange();
        #endregion

        #region Properties

        /// <summary>  
        /// Gets or sets a value indicating whether the scrollbar is enabled.  
        /// </summary>  
        /// <value>  
        /// <c>true</c> to enable the scrollbar; otherwise, <c>false</c>.  
        /// The default value is <c>false</c>.  
        /// </value>  
        /// <remarks>  
        /// If set to <c>true</c>, the axis will be rendered with a scrollbar.  
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to enable and configure the scrollbar on the primary X-axis of a chart.
        /// <SfChart>
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.DateTime">
        ///         <ChartAxisScrollbarSettings Enable="true" PointsLength="30" />
        ///     </ChartPrimaryXAxis>
        ///     <ChartSeries DataSource="@dataSource" XName="x" YName="y" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool Enable { get; set; }

        /// <summary>  
        /// Gets or sets the length of the points for numeric and logarithmic values.  
        /// </summary>  
        /// <value>  
        /// A double value representing the length of the points.  
        /// </value>  
        /// <remarks>  
        /// At a time either this <see cref="PointsLength"/> or <see cref="ChartAxisScrollbarSettingsRange"/> is applicable.  
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the points length for the scrollbar on the primary X-axis of a chart.
        /// <SfChart>
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.DateTime">
        ///         <ChartAxisScrollbarSettings Enable="true" PointsLength="30" />
        ///     </ChartPrimaryXAxis>
        ///     <ChartSeries DataSource="@dataSource" XName="x" YName="y" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double PointsLength { get; set; }

        /// <summary> 
        /// Gets or sets the height of the scrollbar. 
        /// </summary> 
        /// <value> 
        /// A <c>double</c> value that defines the height of the scrollbar. The default value is <c>16</c>. 
        /// </value> 
        /// <example> 
        /// <code> 
        /// <![CDATA[ 
        /// <SfChart> 
        ///     <ChartPrimaryXAxis Title="X-Axis"> 
        ///         <ChartAxisScrollbarSettings Height="16" /> 
        ///     </ChartPrimaryXAxis> 
        /// </SfChart> 
        /// ]]> 
        /// </code> 
        /// </example> 
        /// <remarks> 
        /// Use this property to customize the height of the scrollbar on the axis. 
        /// </remarks> 
        [Parameter]
        public double Height { get; set; } = 16;
        /// <summary> 
        /// Gets or sets the corner radius of the scrollbar track. 
        /// </summary> 
        /// <value> 
        /// A <c>double</c> value that specifies the corner radius of the scrollbar track. The default value is <c>0</c>. 
        /// </value> 
        /// <example> 
        /// <code> 
        /// <![CDATA[ 
        /// <SfChart> 
        ///     <ChartPrimaryXAxis Title="X-Axis"> 
        ///         <ChartAxisScrollbarSettings TrackRadius="14" /> 
        ///     </ChartPrimaryXAxis> 
        /// </SfChart> 
        /// ]]> 
        /// </code> 
        /// </example> 
        /// <remarks> 
        /// Use this property to customize the corner radius of the scrollbar track on the axis. 
        /// </remarks> 
        [Parameter]
        public double TrackRadius { get; set; }

        /// <summary> 
        /// Gets or sets the corner radius of the scrollbar border. 
        /// </summary> 
        /// <value> 
        /// A <c>double</c> value that specifies the corner radius of the scrollbar border. The default value is <c>0</c>. 
        /// </value> 
        /// <example> 
        /// <code> 
        /// <![CDATA[ 
        /// <SfChart> 
        ///     <ChartPrimaryXAxis Title="X-Axis"> 
        ///         <ChartAxisScrollbarSettings ScrollbarRadius="14" /> 
        ///     </ChartPrimaryXAxis> 
        /// </SfChart> 
        /// ]]> 
        /// </code> 
        /// </example> 
        /// <remarks> 
        /// Use this property to customize the corner radius of the scrollbar border on the axis. 
        /// </remarks> 
        [Parameter]
        public double ScrollbarRadius { get; set; }

        /// <summary>
        /// Gets or sets the position of the scrollbar in the chart.
        /// 
        /// Available options include:
        /// <list type="bullet">
        /// <item>
        /// <description><c>Top</c>: Positions the scrollbar at the top of the chart. Applicable only to horizontal scrollbars.</description>
        /// </item>
        /// <item>
        /// <description><c>Bottom</c>: Positions the scrollbar at the bottom of the chart. Applicable only to horizontal scrollbars.</description>
        /// </item>
        /// <item>
        /// <description><c>Left</c>: Positions the scrollbar on the left side of the chart. Applicable only to vertical scrollbars.</description>
        /// </item>
        /// <item>
        /// <description><c>Right</c>: Positions the scrollbar on the right side of the chart. Applicable only to vertical scrollbars.</description>
        /// </item>
        /// <item>
        /// <description><c>PlaceNextToAxisLine</c>: Positions the scrollbar next to the axis line.</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <value>
        /// The default value is <c>PlaceNextToAxisLine</c>.
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <SfChart>
        ///     <ChartPrimaryXAxis>
        ///         <ChartAxisScrollbarSettings Enable="true" Position="ScrollbarPosition.Bottom">
        ///         </ChartAxisScrollbarSettings>
        ///     </ChartPrimaryXAxis>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        /// <remarks>
        /// Use this property to change the position of the scrollbar in the chart to suit your desired layout.
        /// </remarks>
        [Parameter]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ScrollbarPosition Position { get; set; }

        /// <summary>
        /// Gets or sets the visibility of the arrow at the end of the scrollbar, which allows zooming using the scrollbar. 
        /// </summary>
        /// <value> 
        /// <c>true</c> to display the arrows in the scrollbar for zooming; otherwise, <c>false</c>.
        /// The default value is <c>true</c>.
        /// </value> 
        /// <example> 
        /// <code> 
        /// <![CDATA[ 
        /// <SfChart> 
        ///     <ChartPrimaryXAxis Title="X-Axis"> 
        ///         <ChartAxisScrollbarSettings EnableZoom="false" /> 
        ///     </ChartPrimaryXAxis> 
        /// </SfChart> 
        /// ]]> 
        /// </code> 
        /// </example> 
        /// <remarks> 
        /// Use this property to show or hide the arrows at the end of the scrollbar, which enable zooming functionality.
        /// </remarks> 
        [Parameter]
        public bool EnableZoom { get; set; } = true;

        /// <summary> 
        /// Gets or sets an instance of <see cref="ChartAxisScrollbarSettingsRange"/> which controls the customization of the scrollbar range. 
        /// </summary> 
        /// <value> 
        /// An instance of <see cref="ChartAxisScrollbarSettingsRange"/>. 
        /// </value> 
        /// <remarks>  
        /// At a time either this <see cref="Range"/> or <see cref="PointsLength"/> is applicable.  
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the range for the scrollbar on the primary X-axis of a chart.
        /// <SfChart>
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.DateTime">
        ///         <ChartAxisScrollbarSettings Enable="true" Range="@settingsRange" />
        ///     </ChartPrimaryXAxis>
        ///     <ChartSeries DataSource="@dataSource" XName="x" YName="y" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ChartAxisScrollbarSettingsRange Range { get; set; } = new ChartAxisScrollbarSettingsRange();


        /// <summary> 
        /// Gets or sets the fill color of the scrollbar rifles. 
        /// </summary> 
        /// <value> 
        /// A <c>string</c> that specifies the color of the scrollbar rifles. The default value is an <b>empty string</b>. 
        /// </value> 
        /// <example> 
        /// <code> 
        /// <![CDATA[ 
        /// <SfChart> 
        ///     <ChartPrimaryXAxis Title="X-Axis"> 
        ///         <ChartAxisScrollbarSettings GripColor="green" /> 
        ///     </ChartPrimaryXAxis> 
        /// </SfChart> 
        /// ]]> 
        /// </code> 
        /// </example> 
        /// <remarks> 
        /// Use this property to customize the fill color of the rifles in the scrollbar on the axis. 
        /// </remarks> 
        [Parameter]
        public string GripColor { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets the color of the scrollbar track. 
        /// </summary> 
        /// <value> 
        /// A <c>string</c> that specifies the color of the scrollbar track. The default value is an <b>empty string</b>. 
        /// </value> 
        /// <example> 
        /// <code> 
        /// <![CDATA[ 
        /// <SfChart> 
        ///     <ChartPrimaryXAxis Title="X-Axis"> 
        ///         <ChartAxisScrollbarSettings TrackColor="red" /> 
        ///     </ChartPrimaryXAxis> 
        /// </SfChart> 
        /// ]]> 
        /// </code> 
        /// </example> 
        /// <remarks> 
        /// Use this property to customize the color of the scrollbar track on the axis. 
        /// </remarks> 
        [Parameter]
        public string TrackColor { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets the fill color of the scrollbar. 
        /// </summary> 
        /// <value> 
        /// A <c>string</c> that specifies the fill color of the scrollbar. The default value is an <b>empty string</b>. 
        /// </value> 
        /// <example> 
        /// <code> 
        /// <![CDATA[ 
        /// <SfChart> 
        ///     <ChartPrimaryXAxis Title="X-Axis"> 
        ///         <ChartAxisScrollbarSettings ScrollbarColor="yellow" /> 
        ///     </ChartPrimaryXAxis> 
        /// </SfChart> 
        /// ]]> 
        /// </code> 
        /// </example> 
        /// <remarks> 
        /// Use this property to customize the fill color of the scrollbar on the axis. 
        /// </remarks> 
        [Parameter]
        public string ScrollbarColor { get; set; } = string.Empty;

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Invoked when component parameters are set and used to detect property changes.
        /// </summary>
        /// <remarks>
        /// Compares current parameter values with previous values and updates the component state
        /// if any changes are detected.
        /// </remarks>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (_enable != Enable ||
                _pointsLength != PointsLength ||
                _height != Height ||
                _gripColor != GripColor ||
                _trackColor != TrackColor ||
                _scrollbarColor != ScrollbarColor ||
                !Equals(ScrollRange, Range))
            {
                _enable = Enable;
                _pointsLength = PointsLength;
                _height = Height;
                _gripColor = GripColor;
                _trackColor = TrackColor;
                _scrollbarColor = ScrollbarColor;
                ScrollRange = Range;

                _isPropertyChanged = true;
            }
        }

        /// <summary>
        /// Disposes resources associated with this component and its child components.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override ValueTask DisposeAsyncCore()
        {
            ScrollRange?.ComponentDispose();
            return base.DisposeAsyncCore();
        }
        #endregion
    }
}
