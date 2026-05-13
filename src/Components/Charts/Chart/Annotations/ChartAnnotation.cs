using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using Syncfusion.Blazor.Toolkit.Charts.Internal;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Represents an annotation, a user-defined HTML element that can be added to a chart.
    /// This class provides options for customizing these annotations to enhance their visual appeal.
    /// </summary>
    public class ChartAnnotation : ChartSubComponent, IChartElement
    {
        #region Fields

        private string _horizontalAxisName = null!;
        private string _yCoordinate = "0";
        private string _verticalAxisName = null!;
        private bool _isPropertyChanged;
        private Units _coordinateUnits;
        private Regions _region;
        private object _xCoordinate = "0";
        private RenderFragment _contentTemplate = null!;
        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the associated chart annotation being rendered.
        /// </summary>
        internal ChartAnnotationRenderer? Renderer { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the parent annotations container.
        /// </summary>
        /// <value>
        /// The parent <see cref="ChartAnnotations"/> component that contains this annotation.
        /// </value>
        [CascadingParameter]
        public ChartAnnotations? Parent { get; set; }

        /// <summary> 
        /// Gets or sets the content template for the annotation. The content template can be any HTML element, allowing for a flexible and customizable way to display additional information or visuals on the chart.
        /// </summary> 
        /// <value>
        /// The template content is applied for the chart annotation based on the custom template provided by the user that is being rendered. The default value is <b>null</b>.
        /// </value> 
        /// <remarks> 
        /// The <see cref="ContentTemplate"/> is used to define a custom template for the chart annotation.
        /// </remarks> 
        /// <example> 
        /// <code> 
        /// <![CDATA[
        /// // The following code snippet shows how to set up chart with custom annotation templates:
        /// <SfChart Title="Olympic Medals"> 
        ///     <ChartAnnotations> 
        ///         <ChartAnnotation X="250" Y="100"> 
        ///             <ContentTemplate> 
        ///                 <div>Annotation in Pixel</div> 
        ///             </ContentTemplate> 
        ///         </ChartAnnotation> 
        ///     </ChartAnnotations> 
        ///     ... 
        /// </SfChart> 
        /// ]]> 
        /// </code> 
        /// </example>
        [Parameter]
        public RenderFragment ContentTemplate { get; set; } = null!;

        /// <summary> 
        /// Gets or sets the annotation’s coordinate units, either in Pixel or Point. 
        /// </summary> 
        /// <value> 
        /// One of the <see cref="Units"/> enumerations that specifies the coordinate units of an annotation. 
        /// The options include: 
        ///   - <c>Pixel</c>: Annotation renders based on x and y pixel values. 
        ///   - <c>Point</c>: Annotation renders based on x and y axis values. 
        /// <br/>
        /// The default value is <b>Units.Pixel</b>. 
        /// </value>
        /// <remarks>
        /// Adjust the <see cref="CoordinateUnits"/> property to control how the annotation is positioned on the chart.
        /// Choosing <c>Pixel</c> makes the annotation placement more absolute relative to the control's boundaries, 
        /// whereas <c>Point</c> allows it to respond dynamically to data changes and axis scaling.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to render the annotation based on pixel values:
        ///  <SfChart>
        ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"/>
        ///     <ChartAnnotations>
        ///         <ChartAnnotation X="@Country" Y="65" CoordinateUnits="Units.Point">
        ///             <ContentTemplate>
        ///                 <div>Highest Medal Count</div>
        ///             </ContentTemplate>
        ///         </ChartAnnotation>
        ///     </ChartAnnotations>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@MedalDetails" XName="Country" YName="Gold"/>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// 
        /// @code{
        ///     string Country = "China";
        ///     ...
        ///     public List<ChartData> MedalDetails = new List<ChartData>
        ///     {
        ///         new ChartData{ Country = "USA", Gold = 50 },
        ///         new ChartData{ Country = "China", Gold = 40 },
        ///         new ChartData{ Country = "Japan", Gold = 70 }
        ///     };
        /// }
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public Units CoordinateUnits { get; set; }

        /// <summary> 
        /// Gets or sets information about the annotation for assistive technology. 
        /// </summary> 
        /// <value> 
        /// A string providing additional details or context about the annotation that can be utilized by assistive technology. 
        /// </value>
        /// <remarks>
        /// Use the <see cref="Description"/> property to convey useful information to screen readers and other accessibility tools.
        /// </remarks>
        [Parameter]
        public string Description { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets the regions to insert annotations in relation to a series or a chart. 
        /// </summary> 
        /// <value> 
        /// One of the <see cref="Regions"/> enumerations that specifies the regions of an annotation. 
        /// The options include: 
        ///   - <c>Chart</c>: Annotation renders based on chart coordinates. 
        ///   - <c>Series</c>: Annotation renders based on series coordinates. 
        /// <br/>
        /// The default value is <b>Regions.Chart</b>. 
        /// </value> 
        /// <remarks> 
        /// Adjust the <see cref="Region"/> property to change where and how annotations are displayed on the chart.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to render a chart with an annotation placed within the chart region:
        /// <SfChart>
        ///     <ChartAnnotations>
        ///         <ChartAnnotation X="100" Y="65" Region="Regions.Series">
        ///             <ContentTemplate>
        ///                 <div>Highest Medal Count</div>
        ///             </ContentTemplate>
        ///         </ChartAnnotation>
        ///     </ChartAnnotations>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y"/>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public Regions Region { get; set; }

        /// <summary> 
        /// Gets or sets the X-coordinate value for the annotation. 
        /// </summary> 
        /// <value> 
        /// If the coordinate unit is set to <see cref="Units.Point"/>, this property specifies the axis value.
        /// Otherwise, it specifies the pixel or percentage of the coordinate. The default value is zero. 
        /// </value>
        /// <remarks>
        /// This property determines the horizontal position of the annotation based on the defined coordinate units.
        /// When the <see cref="CoordinateUnits"/> is set to <c>Units.Pixel</c>, the <c>X</c> value represents the
        /// number of pixels from the chart's left boundary. If set to <c>Units.Point</c>, it corresponds to a value
        /// along the horizontal axis.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to render a chart with an annotation using only the X coordinate:
        /// <SfChart>
        ///     <ChartAnnotations>
        ///         <ChartAnnotation X="100">
        ///             <ContentTemplate>
        ///                 <div>Highest Medal Count</div>
        ///             </ContentTemplate>
        ///         </ChartAnnotation>
        ///     </ChartAnnotations>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y"/>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public object? X { get; set; } = "0";

        /// <summary> 
        /// Gets or sets the name of the horizontal axis associated with the annotation. 
        /// </summary> 
        /// <value> 
        /// The name of the horizontal axis. 
        /// </value> 
        /// <remarks> 
        /// This property requires the presence of the 'Axes' of the chart. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to render a chart with an annotation aligned to a specific X axis:
        /// <SfChart>
        ///     <ChartAxes>
        ///         <ChartAxis Name="XAxis" OpposedPosition="true"/>
        ///     </ChartAxes>
        ///     <ChartAnnotations>
        ///         <ChartAnnotation X="100" Y="65" XAxisName="XAxis">
        ///             <ContentTemplate>
        ///                 <div>Highest Medal Count</div>
        ///             </ContentTemplate>
        ///         </ChartAnnotation>
        ///     </ChartAnnotations>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y"/>
        ///         <ChartSeries DataSource="@WeatherReports1" XName="X" YName="Y" XAxisName="XAxis"/>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string XAxisName { get; set; } = null!;

        /// <summary> 
        /// Gets or sets the Y-coordinate value for the annotation. 
        /// </summary> 
        /// <value> 
        /// If the coordinate unit is set to <see cref="Units.Point"/>, this property specifies the axis value.
        /// Otherwise, it specifies the pixel or percentage of the coordinate. The default value is zero.
        /// </value>
        /// <remarks>
        /// This property determines the vertical position of the annotation based on the defined coordinate units.
        /// When the <see cref="CoordinateUnits"/> is set to <c>Units.Pixel</c>, the <c>Y</c> value represents the
        /// number of pixels from the chart's top boundary. If set to <c>Units.Point</c>, it corresponds to a value
        /// along the vertical axis.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to render a chart with an annotation using only the y coordinate.
        /// <SfChart>
        ///     <ChartAnnotations>
        ///         <ChartAnnotation Y="100">
        ///             <ContentTemplate>
        ///                 <div>Highest Medal Count</div>
        ///             </ContentTemplate>
        ///         </ChartAnnotation>
        ///     </ChartAnnotations>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y"/>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string Y { get; set; } = "0";

        /// <summary> 
        /// Gets or sets the name of the vertical axis associated with the annotation. 
        /// </summary> 
        /// <value> 
        /// The name of the vertical axis. 
        /// </value> 
        /// <remarks> 
        /// This property requires the presence of the 'Axes' of the chart. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to render a chart with an annotation aligned to a specific Y axis:
        /// <SfChart>
        ///     <ChartAxes>
        ///         <ChartAxis Name="YAxis" OpposedPosition="true"/>
        ///     </ChartAxes>
        ///     <ChartAnnotations>
        ///         <ChartAnnotation X="100" Y="65" YAxisName="YAxis">
        ///             <ContentTemplate>
        ///                 <div>Highest Medal Count</div>
        ///             </ContentTemplate>
        ///         </ChartAnnotation>
        ///     </ChartAnnotations>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y"/>
        ///         <ChartSeries DataSource="@WeatherReports1" XName="X" YName="Y" YAxisName="YAxis"/>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string YAxisName { get; set; } = null!;

        /// <summary>
        /// Gets or sets a unique key for identifying the renderer associated with this annotation.
        /// </summary>
        /// <value>
        /// The renderer key used internally for tracking and rendering. Default value is <see cref="string.Empty"/>.
        /// </value>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public string RendererKey { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of the renderer used to render this annotation.
        /// </summary>
        /// <value>
        /// The <see cref="Type"/> of the annotation renderer. Default value is <b>null</b>.
        /// </value>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public Type RendererType { get; set; } = null!;

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Executes after the component is initialized.
        /// Registers the annotation with the parent chart and sets the renderer type.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Tracker is ChartAnnotations annotations)
            {
                Parent = annotations;
            }
            Parent?.Chart?.AddAnnotation(this);
            RendererType = typeof(ChartAnnotationRenderer);
        }

        /// <summary>
        /// Executes when component parameters are set or updated.
        /// Triggers rendering recalculation if properties have changed.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (!Equals(_contentTemplate, ContentTemplate) && ContentTemplate is not null)
            {
                _contentTemplate = ContentTemplate;
                _isPropertyChanged = Parent is not null;
            }

            if (_coordinateUnits != CoordinateUnits)
            {
                _coordinateUnits = CoordinateUnits;
                _isPropertyChanged = Parent is not null;
            }

            if (_region != Region)
            {
                _region = Region;
                _isPropertyChanged = Parent is not null;
            }

            if (!Equals(_xCoordinate, X))
            {
                _xCoordinate = X ?? "0";
                _isPropertyChanged = Parent is not null;
            }

            if (_horizontalAxisName != XAxisName)
            {
                _horizontalAxisName = XAxisName;
                _isPropertyChanged = Parent is not null;
            }

            if (_yCoordinate != Y)
            {
                _yCoordinate = Y;
                _isPropertyChanged = Parent is not null;
            }

            if (_verticalAxisName != YAxisName)
            {
                _verticalAxisName = YAxisName;
                _isPropertyChanged = Parent is not null;
            }
            if (_isPropertyChanged && Renderer is not null)
            {
                _isPropertyChanged = false;
                Renderer.CalculateRenderingOption();
                Renderer.ProcessRenderQueue();
            }
        }

        /// <summary>
        /// Disposes resources and unregisters the annotation from the parent chart.
        /// </summary>
        protected override ValueTask DisposeAsyncCore()
        {
            Parent?.Chart?.RemoveAnnotation(this);
            return base.DisposeAsyncCore();
        }

        #endregion
    }
}
