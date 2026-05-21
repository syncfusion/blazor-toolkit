using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Charts.Internal;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Represents a row within a chart, providing customization options for its appearance and layout.
    /// </summary>
    public class ChartRow : ChartSubComponent, IChartElement
    {
        #region Fields
        private string _height = "100%";
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
        /// Gets or sets the associated chart axes and computed dimension.
        /// </summary>
        internal ChartRowRenderer? Renderer { get; set; }

        /// <summary> 
        /// Gets or sets the color of the chart row border as a string. 
        /// </summary> 
        /// <value> 
        /// A string representing the border color of the chart row. The default value is <b>null</b>.
        /// </value> 
        /// <remarks> 
        /// This property allows customization of the border color for the bottom line of the row, which divides the chart area vertically. 
        /// Accepts values in hex or rgba as valid CSS color strings. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a divided chart area with custom row borders color:
        /// <SfChart>
        ///     <ChartRows>
        ///         <ChartRow Height="50%" BorderColor="red"/>
        ///         <ChartRow Height="50%" BorderColor="blue" />
        ///     </ChartRows>
        ///     <ChartAxes>
        ///         <ChartAxis RowIndex="1" Name="YAxis" OpposedPosition="true" />
        ///     </ChartAxes>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y" />
        ///         <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y1" YAxisName="YAxis" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string BorderColor { get; set; } = null!;

        /// <summary> 
        /// Gets or sets the width of the chart row border in pixels. 
        /// </summary> 
        /// <value>
        /// The double value representing the border width of the chart row. The default value is <b>1</b> pixel.
        /// </value>
        /// <remarks> 
        /// This property allows customization of the border width for the bottom line of the row, which divides the chart area vertically. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a divided chart area with custom row borders width:
        /// <SfChart>
        ///     <ChartRows>
        ///         <ChartRow Height="50%" BorderColor="red" BorderWidth="2" />
        ///         <ChartRow Height="50%" BorderColor="blue" BorderWidth="2" />
        ///     </ChartRows>
        ///     <ChartAxes>
        ///         <ChartAxis RowIndex="1" Name="YAxis" OpposedPosition="true"/>
        ///     </ChartAxes>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y" />
        ///         <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y1" YAxisName="YAxis" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public double BorderWidth { get; set; } = 1;

        /// <summary>
        /// Gets or sets the height of the row as a string, accepting input in the form of '100px' or '100%'.
        /// </summary>
        /// <value>
        /// A string representing the height of the row. The default value is <b>"100%"</b>.
        /// </value>
        /// <remarks>
        /// If specified as '100%', the row will render to the full height of its chart.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following code demonstrates setting a custom row in a chart:
        /// <SfChart>
        ///     <ChartRows>
        ///         <ChartRow Height="50%" />
        ///         <ChartRow Height="50%" />
        ///     </ChartRows>
        ///     <ChartAxes>
        ///         <ChartAxis RowIndex="1" Name="YAxis" />
        ///     </ChartAxes>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y" />
        ///         <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y1" YAxisName="YAxis" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string Height { get; set; } = "100%";

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
        /// Performs component initialization and sets up the renderer type.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            RendererType = typeof(ChartRowRenderer);
        }

        /// <summary>
        /// Handles parameter changes and registers the row with its parent chart container.
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

                if (_height != Height)
                {
                    _height = Height;
                    _ = Container?.ProcessOnLayoutChangeAsync();
                }
            }

            if (Container is null)
            {
                return;
            }
            Container.AddRow(this);
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
            Container?.RemoveRow(this);
            Container = null;
            ChildContent = null!;
        }
        #endregion
    }
}
