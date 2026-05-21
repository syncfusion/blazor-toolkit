using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Charts.Internal;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Represents the settings for a chart legend.
    /// </summary>
    public class ChartLegendSettings : ChartSubComponent, ILegendBase
    {
        #region Fields

        private bool _visible = true;
        private bool _toggleVisibility = true;
        private bool _enableHighlight;
        private bool _reverse;
        private bool _isInversed;
        private string _width = null!;
        private string _height = null!;
        private string _background = Constants.Transparent;
        private double _padding = 8;
        private double _itemPadding = double.NaN;
        private double _shapeWidth = 10;
        private double _shapeHeight = 10;
        private double _shapePadding = 8;
        private double _opacity = 1;
        private double _tabIndex = 3;
        private double _maximumLabelWidth;

        private LegendPosition _position = LegendPosition.Auto;
        private Alignment _alignment = Alignment.Center;
        private TextWrap _textWrap = TextWrap.Normal;
        private ChartLocation PrevLocation { get; set; } = new ChartLocation();
        private ChartLegendTextStyle PrevTextStyle { get; set; } = new ChartLegendTextStyle();
        private ChartLegendBorder PrevBorder { get; set; } = new ChartLegendBorder();
        private ChartLegendMargin PrevMargin { get; set; } = new ChartLegendMargin();
        private ChartLegendRenderer? _renderer;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the chart legend renderer instance.
        /// </summary>
        /// <value>
        /// An instance of <see cref="ChartLegendRenderer"/> that handles legend rendering. Default: <c>null</c>.
        /// </value>
        internal ChartLegendRenderer Renderer
        {
            get => _renderer ?? null!;
            set
            {
                if (_renderer != value)
                {
                    _renderer = value;
                    _renderer?.OnParentParameterSet();
                }
            }
        }

        /// <summary>
        /// Gets or sets the chart component that owns this legend.
        /// </summary>
        /// <value>
        /// The parent <see cref="SfChart"/> component. This is automatically set via cascading parameters.
        /// </value>
        [CascadingParameter]
        internal SfChart? Owner { get; set; }

        /// <summary> 
        /// Gets or sets a value indicating the visibility of the legend. 
        /// </summary> 
        /// <value> 
        /// <b>true</b> if the legend is visible; otherwise, <b>false</b>. The default value is <b>true</b>. 
        /// </value> 
        /// <remarks>
        /// This property controls the display of the chart legend.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code snippet demonstrates how to set the visibility of the legend in an chart:
        /// <SfChart>        
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" />
        ///     </ChartSeriesCollection>
        ///     <ChartLegendSettings Visible="true" />
        ///</SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool Visible
        {
            get => _visible;

            set
            {
                if (_visible != value)
                {
                    _visible = value;
                    if (_renderer is not null)
                    {
                        _renderer.RendererShouldRender = true;
                        Owner?.OnLayoutChange();
                    }
                }
            }
        }

        /// <summary> 
        /// Gets or sets a value indicating whether to enable highlighting when hovered over the legend element. 
        /// </summary> 
        /// <value> 
        /// <b>true</b> if highlight options are enabled; otherwise, <b>false</b>. The default value is <b>false</b>. 
        /// </value> 
        /// <remarks> 
        /// If set to <b>true</b>, when hovered over the legend element, the respective series will be highlighted. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code snippet demonstrates how hovering over a legend item highlights the corresponding series in the chart.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" />
        ///     </ChartSeriesCollection>
        ///     <ChartLegendSettings Visible="true" EnableHighlight="true" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool EnableHighlight
        {
            get => _enableHighlight;

            set
            {
                if (_enableHighlight != value)
                {
                    _enableHighlight = value;
                }
            }
        }

        /// <summary> 
        /// Gets or sets the width for the legend. 
        /// </summary> 
        /// <value> 
        /// A string representing the width of the legend. Based on the legend position, the width will be calculated. The default value is <b>null</b>.
        /// </value> 
        /// <remarks> 
        /// Accepts values in either pixels or percentage. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the width for a legend in a chart:
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" />
        ///         <ChartSeries DataSource="@MedalDetails" Name="Silver" XName="Country" YName="Silver" />
        ///         <ChartSeries DataSource="@MedalDetails" Name="Bronze" XName="Country" YName="Bronze" />
        ///     </ChartSeriesCollection>
        ///     <ChartLegendSettings Visible="true" Width="60px" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string Width
        {
            get => _width;

            set
            {
                if (_width != value)
                {
                    _width = value;
                    LegendPropertyChanged();
                }
            }
        }

        /// <summary> 
        /// Gets or sets the height for the legend element. 
        /// </summary> 
        /// <value> 
        /// A string representing the height of the legend element. The default value is <b>null</b>. Based on the legend position, the height of the legend will be calculated.
        /// </value> 
        /// <remarks> 
        /// Accepts values in either pixels or percentage. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the height for a legend in a chart:
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" />
        ///         <ChartSeries DataSource="@MedalDetails" Name="Silver" XName="Country" YName="Silver" />
        ///         <ChartSeries DataSource="@MedalDetails" Name="Bronze" XName="Country" YName="Bronze" />
        ///     </ChartSeriesCollection>
        ///     <ChartLegendSettings Visible="true" Height="60px" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string Height
        {
            get => _height;

            set
            {
                if (_height != value)
                {
                    _height = value;
                    LegendPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the format for the legend item accessibility description of the <see cref="ChartLegendSettings"/>.
        /// </summary>
        /// <value>
        /// Accepts a string that defines the format for the legend item accessibility description of the <see cref="ChartLegendSettings"/>. The default value is <b>null</b>.
        /// </value>
        /// <remarks>
        /// Use this property to specify a format for the legend item accessibility description of the <see cref="ChartLegendSettings"/>.
        /// The placeholder ${value} can be used to set the accessibility text for the legend item.
        /// For example, the format "Selected the ${value} legend" will read as "Selected the Product A legend" for the legend item with text "Product A".
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the accessibility description format for the legend items in a chart.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" />
        ///         <ChartSeries DataSource="@MedalDetails" Name="Silver" XName="Country" YName="Silver" />
        ///         <ChartSeries DataSource="@MedalDetails" Name="Bronze" XName="Country" YName="Bronze" />
        ///     </ChartSeriesCollection>
        ///     <ChartLegendSettings Visible="true" AccessibilityDescriptionFormat="Selected the ${value} legend" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string AccessibilityDescriptionFormat { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the accessibility role for the <see cref="ChartLegendSettings"/>.
        /// </summary>
        /// <value>
        /// Accepts a string that defines the accessibility role for the <see cref="ChartLegendSettings"/>. The default value is <b>null</b>.
        /// </value>
        /// <remarks>
        /// Use this property to provide an accessibility role for the <see cref="ChartLegendSettings"/>.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the accessibility rolefor the legend items in a chart.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" />
        ///         <ChartSeries DataSource="@MedalDetails" Name="Silver" XName="Country" YName="Silver" />
        ///         <ChartSeries DataSource="@MedalDetails" Name="Bronze" XName="Country" YName="Bronze" />
        ///     </ChartSeriesCollection>
        ///     <ChartLegendSettings Visible="true" AccessibilityRole="Legend" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string AccessibilityRole { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the accessibility keyboard navigation focus option for the <see cref="ChartLegendSettings"/>.
        /// </summary>
        /// <value>
        /// Accepts the boolean value to enable or disable the keyboard navigation for the <see cref="ChartLegendSettings"/>. The default value is <b>true</b>.
        /// </value>
        /// <remarks>
        /// Use this property to toggle the keyboard navigation focus for the <see cref="ChartLegendSettings"/>.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to enable keyboard navigation for focusable UI elements.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" />
        ///     </ChartSeriesCollection>
        ///     <ChartLegendSettings Visible="true" Focusable="true" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool Focusable { get; set; } = true;

        /// <summary> 
        /// Gets or sets an instance of <see cref="ChartLocation"/> that specifies the location of the legend relative to the chart. 
        /// If x is set to 20, the legend will move 20 pixels to the right of the chart. 
        /// </summary> 
        /// <value> 
        /// The default value is an instance of <see cref="ChartLocation"/>. 
        /// </value> 
        /// <remarks> 
        /// The <see cref="Position"/> must be set to <c>LegendPosition.Custom</c> to position the legend in the chart using the specified x and y offsets. 
        /// </remarks>
        /// <example>   
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to determine the x and y location of a legend based on its position in a chart.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" />
        ///     </ChartSeriesCollection>
        ///     <ChartLegendSettings Visible="true" Position="LegendPosition.Custom">
        ///         <ChartLocation X="30" Y="40" />
        ///     </ChartLegendSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ChartLocation Location
        {
            get => PrevLocation;
            set
            {
                if (PrevLocation != value)
                {
                    PrevLocation = value;
                }
            }
        }

        /// <summary> 
        /// Gets or sets the position of the legend in the chart. 
        /// </summary> 
        /// <value> 
        /// One of the <see cref="LegendPosition"/> enumeration that specifies the position of the legend. 
        /// The options include: 
        ///   - <c>Auto</c>: Places the legend based on based on width and height 
        ///   - <c>Top </c>: Displays the legend at the top of the chart. 
        ///   - <c>Left </c>: Displays the legend at the left of the chart. 
        ///   - <c>Bottom</c>: Displays the legend at the bottom of the chart. 
        ///   - <c>Right</c>: Displays the legend at the right of the chart. 
        ///   - <c>Custom</c>: Displays the legend based on the given x and y location values. 
        /// <br/>
        /// The default mode is <b>LegendPosition.Auto</b>. 
        /// </value>
        /// <remarks>
        /// Use this property to position the legend at the top, left, bottom, or right of the <see cref="SfChart">Accumulation Chart</see>.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the legend position in a chart.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" />
        ///     </ChartSeriesCollection>
        ///     <ChartLegendSettings Visible="true" Position="LegendPosition.Top" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public LegendPosition Position
        {
            get => _position;
            set
            {
                if (_position != value)
                {
                    _position = value;
                    if (_renderer is not null)
                    {
                        _renderer.RendererShouldRender = true;
                        Owner?.OnLayoutChange();
                    }
                }
            }
        }

        /// <summary> 
        /// Gets or sets the padding around the legend collection. 
        /// </summary> 
        /// <value> 
        /// A double representing the padding around the legend collection. The default value is <b>8</b>. 
        /// </value>
        /// <remarks>
        /// This property is used to adjust the space around the legend to enhance layout.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to adjust the space around the legend collection in a chart.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" />
        ///         <ChartSeries DataSource="@MedalDetails" Name="Silver" XName="Country" YName="Silver" />
        ///     </ChartSeriesCollection>
        ///     <ChartLegendSettings Visible="true" Padding="50" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double Padding
        {
            get => _padding;
            set
            {
                if (_padding != value)
                {
                    _padding = value;
                    LegendPropertyChanged();
                }
            }
        }

        /// <summary> 
        /// Gets or sets the padding between legend items. 
        /// </summary> 
        /// <value> 
        /// A double representing the padding between legend items. The default value is <b>NaN</b>. 
        /// </value> 
        /// <remarks>
        /// This property provides the spacing between items in the legend.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the spacing between items in the legend of a chart.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" />
        ///         <ChartSeries DataSource="@MedalDetails" Name="Silver" XName="Country" YName="Silver" />
        ///     </ChartSeriesCollection>
        ///     <ChartLegendSettings Visible="true" ItemPadding="50" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double ItemPadding
        {
            get => _itemPadding;

            set
            {
                if (_itemPadding != value)
                {
                    _itemPadding = value;
                    LegendPropertyChanged();
                }
            }
        }

        /// <summary> 
        /// Gets or sets the alignment of the legend. 
        /// </summary> 
        /// <value> 
        /// One of the <see cref="Alignment"/> enumerations that specifies the legend alignment. 
        /// Options include: 
        ///   - <c>Near</c>: Aligns the legend to the left or top based on the <see cref="Position"/>. 
        ///   - <c>Center</c>: Aligns the legend to the center of the chart. 
        ///   - <c>Far</c>: Aligns the legend to the right or bottom based on the <see cref="Position"/>. 
        /// <br/>
        /// The default value is <b>Alignment.Center</b>. 
        /// </value> 
        /// <remarks> 
        /// The alignment of the legend works as follows: 
        ///   - <c>Near</c>: Aligns the legend to the top-left of the chart if <see cref="Position"/> is <c>LegendPosition.Top</c> and to the right-top of the chart if <see cref="Position"/> is <c>LegendPosition.Right</c>. 
        ///   - <c>Center</c>: Aligns the legend to the top center of the chart if <see cref="Position"/> is <c>LegendPosition.Top</c> and to the right center of the chart if <see cref="Position"/> is <c>LegendPosition.Right</c>. 
        ///   - <c>Far</c>: Aligns the legend to the top-right of the chart if <see cref="Position"/> is <c>LegendPosition.Top</c> and to the right-bottom of the chart if <see cref="Position"/> is <c>LegendPosition.Right</c>. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the alignment of the legend of a chart.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" />
        ///     </ChartSeriesCollection>
        ///     <ChartLegendSettings Visible="true" Alignment="Alignment.Far" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public Alignment Alignment
        {
            get => _alignment;

            set
            {
                if (_alignment != value)
                {
                    _alignment = value;
                    if (_renderer is not null)
                    {
                        _renderer.RendererShouldRender = true;
                        if (_position is LegendPosition.Bottom or LegendPosition.Top)
                        {
                            _renderer.LegendBounds.X = LegendBase.AlignLegend(Owner?.InitialRect.X ?? 0, Owner?.AvailableSize.Width ?? 0, _renderer.LegendBounds.Width, _alignment);
                        }
                        else if (_position is LegendPosition.Left or LegendPosition.Right)
                        {
                            _renderer.LegendBounds.Y = LegendBase.AlignLegend(Owner?.InitialRect.Y ?? 0, Owner?.AvailableSize.Height ?? 0, _renderer.LegendBounds.Height, _alignment);
                        }
                        _renderer.CalculateRenderTreeBuilderOptions();
                        _renderer.RendererShouldRender = true;
                        Owner?.OnLayoutChange();
                    }
                }
            }
        }

        /// <summary> 
        /// Gets or sets an instance of <see cref="ChartLegendTextStyle"/> that specifies the text style of the legend. 
        /// </summary> 
        /// <value> 
        /// The default value is an instance of <see cref="ChartLegendTextStyle"/>. 
        /// </value> 
        /// <remarks> 
        /// The legend text font and alignment can be customized using this instance of <see cref="ChartLegendTextStyle"/>. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to customize the legend text in a chart.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" />
        ///     </ChartSeriesCollection>
        ///     <ChartLegendSettings Visible="true">
        ///         <ChartLegendTextStyle 
        ///             Size="14px" 
        ///             Color="blue" 
        ///             FontFamily="Roboto" 
        ///             FontWeight="400" 
        ///             FontStyle="Normal" 
        ///             Opacity="1" 
        ///             TextAlignment="Alignment.Center" 
        ///             TextOverflow="TextOverflow.Trim" />
        ///     </ChartLegendSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ChartLegendTextStyle TextStyle
        {
            get => PrevTextStyle;

            set
            {
                if (PrevTextStyle != value)
                {
                    PrevTextStyle = value;
                }
            }
        }

        /// <summary> 
        /// Gets or sets the width of the legend shape. 
        /// </summary> 
        /// <value> 
        /// A double representing the width of the legend shape. The default value is <b>10</b>. 
        /// </value> 
        /// <remarks>
        /// This property is used to modify the width of the legend shape.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to customize the legend shape width in a chart.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" />
        ///     </ChartSeriesCollection>
        ///     <ChartLegendSettings Visible="true" ShapeWidth="30" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double ShapeWidth
        {
            get => _shapeWidth;

            set
            {
                if (_shapeWidth != value)
                {
                    _shapeWidth = value;
                    LegendPropertyChanged();
                }
            }
        }

        /// <summary> 
        /// Gets or sets the height of the legend shape. 
        /// </summary> 
        /// <value> 
        /// A double representing the height of the legend shape. The default value is <b>10</b>. 
        /// </value> 
        /// <remarks>
        /// This property is used to modify the height of the legend shape.
        /// </remarks>

        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to customize the legend shape height in a chart.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" />
        ///     </ChartSeriesCollection>
        ///     <ChartLegendSettings Visible="true" ShapeHeight="30" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double ShapeHeight
        {
            get => _shapeHeight;

            set
            {
                if (_shapeHeight != value)
                {
                    _shapeHeight = value;
                    LegendPropertyChanged();
                }
            }
        }

        /// <summary> 
        /// Gets or sets an instance of <see cref="ChartLegendBorder"/> that specifies the border of the legend. 
        /// </summary> 
        /// <value> 
        /// The default value is an instance of <see cref="ChartLegendBorder"/>.
        /// </value>
        /// <remarks>
        /// Use this property to customize the border of the legend, allowing adjustments to properties such as color, width, and style to enhance visual appeal.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the legend's border in a chart.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" />
        ///     </ChartSeriesCollection>
        ///     <ChartLegendSettings Visible="true">
        ///         <ChartLegendBorder Color="blue" Width="2" />
        ///     </ChartLegendSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ChartLegendBorder Border
        {
            get => PrevBorder;

            set
            {
                if (PrevBorder != value)
                {
                    PrevBorder = value;
                }
            }
        }

        /// <summary> 
        /// Gets or sets an instance of <see cref="ChartLegendMargin"/> which specifies the margin for the legend. 
        /// </summary> 
        /// <value> 
        /// The default value is an instance of <see cref="ChartLegendMargin"/> allowing customization of left, right, top, and bottom margins. 
        /// </value> 
        /// <remarks> 
        /// The <see cref="ChartLegendMargin"/> class provides options to customize the left, right, top, and bottom margins of the chart legend. 
        /// Adjusting these margins controls the space between the legend and the surrounding elements. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to customize the chart's surrounding space of the legend.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" />
        ///     </ChartSeriesCollection>
        ///     <ChartLegendSettings Visible="true">
        ///         <ChartLegendBorder Color="blue" Width="2" />
        ///         <ChartLegendMargin Left="30" Bottom="30" Right="30" Top="30" />
        ///     </ChartLegendSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ChartLegendMargin Margin
        {
            get => PrevMargin;

            set
            {
                if (PrevMargin != value)
                {
                    PrevMargin = value;
                }
            }
        }

        /// <summary> 
        /// Gets or sets the padding between the legend shape and legend text. 
        /// </summary> 
        /// <value> 
        /// A double representing the padding between the legend shape and legend text. The default value is <b>8</b>. 
        /// </value>
        /// <remarks>
        /// This property allows for the adjustment of space between the legend shape and the text associated with it, enhancing the visual layout of the legend.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the padding between the legend shape and legend text in a chart.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" />
        ///     </ChartSeriesCollection>
        ///     <ChartLegendSettings Visible="true" ShapePadding="20"> />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double ShapePadding
        {
            get => _shapePadding;

            set
            {
                if (_shapePadding != value)
                {
                    _shapePadding = value;
                    LegendPropertyChanged();
                }
            }
        }

        /// <summary> 
        /// Gets or sets the background color of the chart legend area. 
        /// </summary> 
        /// <value>  
        /// A string value specifying the background color of the chart legend area. The default value is <b>"transparent"</b>. 
        /// </value> 
        /// <remarks> 
        /// The value can be specified in hex or rgba format, following valid CSS color string conventions. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the background color of the chart legend area.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" />
        ///     </ChartSeriesCollection>
        ///     <ChartLegendSettings Visible="true" Background="lightgray" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string Background
        {
            get => _background;

            set
            {
                if (_background != value)
                {
                    _background = value;
                }
            }
        }

        /// <summary> 
        /// Gets or sets the opacity of the legend background. 
        /// </summary> 
        /// <value> 
        /// A double representing the opacity of the legend background. The default value is <b>1</b>. 
        /// </value> 
        /// <remarks> 
        /// The provided background color will be rendered with the applied opacity value. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the opacity of the legend background in a chart.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" />
        ///     </ChartSeriesCollection>
        ///     <ChartLegendSettings Visible="true" Background="lightgray" Opacity="0.5" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>        
        [Parameter]
        public double Opacity
        {
            get => _opacity;

            set
            {
                if (_opacity != value)
                {
                    _opacity = value;
                }
            }
        }

        /// <summary> 
        /// Gets or sets a value indicating whether the series' visibility collapses on the legend click. 
        /// </summary> 
        /// <value> 
        /// <b>true</b> if the series' visibility collapses on the legend click; otherwise, <b>false</b>. The default value is <b>true</b>. 
        /// </value> 
        /// <remarks>
        /// This property allows users to toggle the visibility of chart series by clicking on the corresponding legend item, enhancing interactivity and user experience.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the visibility of chart series by clicking on the corresponding legend item.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" />
        ///     </ChartSeriesCollection>
        ///     <ChartLegendSettings Visible="true" ToggleVisibility="true" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool ToggleVisibility
        {
            get => _toggleVisibility;

            set
            {
                if (_toggleVisibility != value)
                {
                    _toggleVisibility = value;
                }
            }
        }

        /// <summary> 
        /// Gets or sets the tabindex value of the legend for accessibility purposes. 
        /// </summary> 
        /// <value> 
        /// A double representing the tabindex value for the legend. The default value is <b>3</b>. 
        /// </value> 
        /// <remarks>
        /// This property is intended to provide a textual description of the legend for assistive technologies, improving accessibility for users with disabilities.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the tabindex value of the legend for accessibility purposes.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" />
        ///     </ChartSeriesCollection>
        ///     <ChartLegendSettings Visible="true" TabIndex="0" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double TabIndex
        {
            get => _tabIndex;

            set
            {
                if (_tabIndex != value)
                {
                    _tabIndex = value;
                }
            }
        }

        /// <summary> 
        /// Gets or sets a value indicating whether to inverse legend item content (image and text). 
        /// </summary> 
        /// <value>  
        /// <b>true</b> if the legend content should be inversed; otherwise, <b>false</b>. The default value is <b>false</b>. 
        /// </value> 
        /// <remarks> 
        /// When set to <b>true</b>, the legend element will display the text before the associated shape. This is different from the default, where the shape is shown first, followed by the text. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the legend the text before the associated shape.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" />
        ///     </ChartSeriesCollection>
        ///     <ChartLegendSettings Visible="true" IsInversed="true" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool IsInversed
        {
            get => _isInversed;
            set
            {
                if (_isInversed != value)
                {
                    _isInversed = value;
                    LegendPropertyChanged();
                }
            }
        }

        /// <summary> 
        /// Gets or sets a value indicating whether to reverse the order of legend items. 
        /// </summary> 
        /// <value> 
        /// <b>true</b> if the legend items should be reversed; otherwise, <b>false</b>. The default value is <b>false</b>. 
        /// </value> 
        /// <remarks> 
        /// If set to <b>true</b>, the last series in the collection will be placed first. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the legend items should be reversed.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" />
        ///         <ChartSeries DataSource="@MedalDetails" Name="Silver" XName="Country" YName="Silver" />
        ///     </ChartSeriesCollection>
        ///     <ChartLegendSettings Visible="true" Reverse="true" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool Reverse
        {
            get => _reverse;
            set
            {
                if (_reverse != value)
                {
                    _reverse = value;
                    LegendPropertyChanged();
                }
            }
        }

        /// <summary> 
        /// Gets or sets the text overflow behavior to employ when the individual legend text overflows the legend bounds or 
        /// <see cref="MaximumLabelWidth"/>. 
        /// </summary> 
        /// <value> 
        /// One of the <see cref="LabelOverflow"/> enumeration values that specifies the text overflow option. 
        /// The options include: 
        /// - <c>LabelOverflow.Ellipse</c>: Trims the text if it exceeds the defined margins. 
        /// - <c>TextOverflow.Clip</c>: Shows the text as it is. 
        /// <br/>
        /// The default value is <see cref="LabelOverflow.Ellipse"/>. 
        /// </value>
        /// <remarks>
        /// This property manages how text should behave when it's too long for its allocated space, either by trimming, wrapping, or displaying fully.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the text overflow behavior to employ when the individual legend text overflows the legend bounds or MaximumLabelWidth.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" />
        ///         <ChartSeries DataSource="@MedalDetails" Name="Silver" XName="Country" YName="Silver" />
        ///     </ChartSeriesCollection>
        ///     <ChartLegendSettings Visible="true" TextOverflow="Syncfusion.Blazor.LabelOverflow.Ellipse" MaximumLabelWidth="20" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public LabelOverflow TextOverflow { get; set; }

        /// <summary> 
        /// Gets or sets the text wrap behavior when the individual legend text overflows the legend bounds or <see cref="MaximumLabelWidth"/>. 
        /// </summary> 
        /// <value> 
        /// One of the <see cref="TextWrap"/> enumeration values that specify the text wrap option. 
        /// The default value is <see cref="TextWrap.Normal"/>. 
        /// </value> 
        ///<remarks> 
        /// This property is applicable only when <see cref="TextOverflow"/> is set to <b>LabelOverflow.Ellipse</b>. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the text wrap behavior when the individual legend text overflows the legend bounds or MaximumLabelWidth.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" />
        ///     </ChartSeriesCollection>
        ///     <ChartLegendSettings Visible="true" TextWrap="Syncfusion.Blazor.TextWrap.Normal" MaximumLabelWidth="20" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public TextWrap TextWrap
        {
            get => _textWrap;
            set
            {
                if (_textWrap != value)
                {
                    _textWrap = value;
                    LegendPropertyChanged();
                }
            }
        }

        /// <summary> 
        /// Gets or sets the maximum width of the individual legend item after which they get trimmed, wrapped, or clipped. 
        /// </summary> 
        /// <value> 
        /// Specifies the maximum width for the legend text. The default value is <b>null</b>. 
        /// </value>
        /// <remarks>
        /// This property is used to control the maximum width of the labels in the legend items, ensuring that the text does not exceed the specified width, when <c>TextOverflow</c> or <c>TextWrap</c> properties are set.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the text wrap behavior when the individual legend text overflows the legend bounds or MaximumLabelWidth.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" />
        ///     </ChartSeriesCollection>
        ///     <ChartLegendSettings Visible="true" MaximumLabelWidth="20" />
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double MaximumLabelWidth
        {
            get => _maximumLabelWidth;
            set
            {
                if (_maximumLabelWidth != value)
                {
                    _maximumLabelWidth = value;
                    LegendPropertyChanged();
                }
            }
        }
        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Initializes the component during component instantiation.
        /// </summary>
        /// <remarks>
        /// Associates the legend settings with the parent chart's legend renderer and
        /// sets the initial visibility state for rendering.
        /// </remarks>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Owner?._legendRenderer is not null)
            {
                Owner._legendRenderer.LegendSettings = this;
                Renderer = Owner._legendRenderer;
                Owner._legendRenderer.Legend = this;
            }
            Renderer.RendererShouldRender = Visible;
        }

        /// <summary>
        /// Disposes resources associated with this component.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override ValueTask DisposeAsyncCore()
        {
            Owner = null;
            ChildContent = null!;
            return base.DisposeAsyncCore();
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Updates legend properties when nested components change.
        /// </summary>
        /// <param name="key">The property name to update (e.g., <c>"Location"</c>, <c>"Border"</c>).</param>
        /// <param name="keyValue">The new value for the property.</param>
        internal void UpdateLegendProperties(string key, object keyValue)
        {
            switch (key)
            {
                case nameof(Location):
                    Location = (ChartLocation)keyValue;
                    LegendPropertyChanged();
                    break;
                case nameof(Border):
                    Border = (ChartLegendBorder)keyValue;
                    break;
                case nameof(TextStyle):
                    TextStyle = (ChartLegendTextStyle)keyValue;
                    break;
                case nameof(Margin):
                    Margin = (ChartLegendMargin)keyValue;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Notifies the chart that a legend property has changed and triggers a layout update if necessary.
        /// </summary>
        /// <param name="isStyleUpdated">
        /// <see langword="true"/> if the change affects styling only; otherwise, <see langword="false"/>.
        /// </param>
        internal void LegendPropertyChanged(bool isStyleUpdated = false)
        {
            if (_renderer is not null && Owner is not null && Owner._isChartFirstRender)
            {
                _renderer.RendererShouldRender = true;
                if (isStyleUpdated)
                {
                    _ = Owner.ProcessOnLayoutChangeAsync();
                    return;
                }
                Owner.OnLayoutChange();
            }
        }
        #endregion
    }
}
