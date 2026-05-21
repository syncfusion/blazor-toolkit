using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Charts.Internal;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides options to customize the appearance of chart columns.
    /// </summary>
    /// <remarks>
    /// The <see cref="ChartColumn"/> component enables customization of chart column borders and dimensions,
    /// allowing for divided chart areas with independent axis configurations.
    /// </remarks>
    public class ChartColumn : ChartSubComponent, IChartElement
    {
        #region Fields

        private string _width = "100%";
        private string _borderColor = null!;
        private double _borderWidth = 1;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the owner chart component.
        /// </summary>
        /// <value>
        /// The <see cref="SfChart"/> instance that owns this chart area, or <see langword="null"/> if not yet cascaded.
        /// </value>
        [CascadingParameter]
        internal SfChart? Container { get; set; }

        /// <summary>
        /// Gets or sets the associated chart column being rendered.
        /// </summary>
        internal ChartColumnRenderer? Renderer { get; set; }

        /// <summary>
        /// Gets or sets the color of the chart column border as a string. 
        /// </summary> 
        /// <value>
        /// A string representing the border color of the chart column. The default value is <b>null</b>.
        /// </value> 
        /// <remarks> 
        /// This property allows customization of the border color for the left line of the column, which divides the chart area horizontally. 
        /// Accepts values in hex or rgba as valid CSS color strings. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a divided chart area with custom column border colors:
        /// <SfChart>
        ///     <ChartColumns>
        ///         <ChartColumn Width="50%" BorderColor="red" />
        ///         <ChartColumn Width="50%" BorderColor="blue" />
        ///     </ChartColumns>
        ///     <ChartAxes>
        ///         <ChartAxis ColumnIndex="1" Name="XAxis" OpposedPosition="true" />
        ///     </ChartAxes>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y" />
        ///         <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y1" XAxisName="XAxis" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string BorderColor { get; set; } = null!;

        /// <summary> 
        /// Gets or sets the width of the chart column border in pixels. 
        /// </summary> 
        /// <value> 
        /// The double value representing the border width of the chart column. The default value is <b>1</b> pixel.
        /// </value> 
        /// <remarks> 
        /// This property allows customization of the border width for the left line of the column, which divides the chart area horizontally. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a divided chart area with custom column border width:
        /// <SfChart>
        ///     <ChartColumns>
        ///         <ChartColumn Width="50%" BorderColor="red" BorderWidth="2" />
        ///         <ChartColumn Width="50%" BorderColor="blue" BorderWidth="2" />
        ///     </ChartColumns>
        ///     <ChartAxes>
        ///         <ChartAxis ColumnIndex="1" Name="XAxis" OpposedPosition="true" />
        ///     </ChartAxes>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y" />
        ///         <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y1" XAxisName="XAxis" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double BorderWidth { get; set; } = 1;

        /// <summary>
        /// Gets or sets the width of the column as a string, accepting input in the form of '100px' or '100%'.
        /// </summary>
        /// <value>
        /// A string representing the width of the column. The default value is <b>"100%"</b>.
        /// </value>
        /// <remarks>
        /// If specified as '100%', the column will render to the full width of its chart.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a divided chart area with custom column:
        /// <SfChart>
        ///     <ChartColumns>
        ///         <ChartColumn Width="50%" />
        ///         <ChartColumn Width="50%" />
        ///     </ChartColumns>
        ///     <ChartAxes>
        ///         <ChartAxis ColumnIndex="1" Name="XAxis" />
        ///     </ChartAxes>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y" />
        ///         <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y1" XAxisName="XAxis" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string Width { get; set; } = "100%";

        /// <summary>
        /// Gets or sets the renderer key for internal use.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public string RendererKey { get; set; } = null!;

        /// <summary>
        /// Gets or sets the renderer type for this component.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public Type RendererType { get; set; } = null!;

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Initializes the component and sets the renderer type.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            RendererType = typeof(ChartColumnRenderer);
        }

        /// <summary>
        /// Processes parameter changes and registers the column with the parent chart.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (Renderer is not null)
            {
                if (_borderColor != BorderColor)
                {
                    _borderColor = BorderColor;
                    _ = Container?.ProcessOnLayoutChangeAsync();
                }

                if (_borderWidth != BorderWidth)
                {
                    _borderWidth = BorderWidth;
                    _ = Container?.ProcessOnLayoutChangeAsync();
                }

                if (_width != Width)
                {
                    _width = Width;
                    _ = Container?.ProcessOnLayoutChangeAsync();
                }
            }

            if (Container is null)
            {
                return;
            }
            Container.AddColumn(this);
        }

        /// <summary>
        /// Cleans up resources when the component is disposed.
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
            Container?.RemoveColumn(this);
            Container = null;
            ChildContent = null!;
        }
        #endregion
    }
}
