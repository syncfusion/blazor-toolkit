﻿using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides the options to customize the empty point of the chart.
    /// </summary>
    /// <remarks>
    /// Empty points are data points whose Y values are <c>null</c> or <c>NaN</c>. This component controls how such points
    /// are treated and displayed (mode, fill, and border). Property changes trigger fine-grained updates to avoid full re-rendering.
    /// </remarks>
    public class ChartEmptyPointSettings : ChartSubComponent
    {
        #region Fields

        private EmptyPointMode _mode = EmptyPointMode.Gap;
        private string _fill = string.Empty;
        private ChartEmptyPointBorder? _border = new();

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the series to which the empty point settings are applied.
        /// </summary>
        /// <value>The owning <see cref="ChartSeries"/> instance.</value>
        [CascadingParameter]
        internal ChartSeries? Series { get; set; }

        /// <summary>
        /// Gets or sets the mode for handling empty points in the <see cref="SfChart"/> series.
        /// </summary>
        /// <value>
        /// An <see cref="EmptyPointMode"/> enumeration value that indicates how empty points are treated in the chart. Options include:
        /// - <c>Gap</c>: Leaves a gap for empty points.
        /// - <c>Zero</c>: Renders empty points as zero.
        /// - <c>Average</c>: Uses the average of adjacent points for empty points.
        /// - <c>Drop</c>: Drops empty points from the chart.
        /// The default value is <see cref="EmptyPointMode.Gap"/>.
        /// </value>
        /// <remarks>
        /// This property determines how the chart visually represents data points that are missing or explicitly set to be empty.
        /// Use the <see cref="Mode"/> property to control how empty points are rendered. Data points with NaN or null values are treated as empty points.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // The following example demonstrates how to set the empty point mode to Average:
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@WeatherReports" XName="X" YName="Y" Type="ChartSeriesType.Column">
        ///             <ChartEmptyPointSettings Mode="EmptyPointMode.Average" />
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// @code {
        /// public class ChartData
        /// {
        ///     public double X { get; set; }
        ///     public double? Y { get; set; }
        /// }
        /// public List<ChartData> WeatherReports = new List<ChartData>
        /// {
        ///     new ChartData { X = 10, Y = 21 },
        ///     new ChartData { X = 20, Y = null },
        ///     new ChartData { X = 30, Y = 36 },
        ///     new ChartData { X = 40, Y = 38 }
        /// };
        /// }
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public EmptyPointMode Mode { get; set; } = EmptyPointMode.Gap;

        /// <summary>
        /// Gets or sets the fill color of the empty point.
        /// </summary>
        /// <value>
        /// A string representing the fill color of the empty point.
        /// </value>
        /// <remarks>
        /// This property is used to define the color used to fill empty points in the chart.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to handle empty points in a column chart by averaging them,
        /// // and setting a custom fill color for the empty points.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" Type="ChartSeriesType.Column">
        ///             <ChartEmptyPointSettings Mode="EmptyPointMode.Average" Fill="red"></ChartEmptyPointSettings>
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public string Fill { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the border configuration of the empty point.
        /// </summary>
        /// <value>
        /// An instance of <see cref="ChartEmptyPointBorder"/> that defines the border properties for the empty point.
        /// </value>
        /// <remarks>
        /// Use this property to specify border-related attributes such as color and width for the empty points,
        /// enhancing their appearance and distinction in the chart.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to apply a custom border color to the empty points.
        /// <SfChart>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" Type="ChartSeriesType.Column">
        ///             <ChartEmptyPointSettings Mode="EmptyPointMode.Average">
        ///                 <ChartEmptyPointBorder Width="2" Color="blue"></ChartEmptyPointBorder>
        ///             </ChartEmptyPointSettings>
        ///         </ChartSeries>
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ChartEmptyPointBorder? Border { get; set; } = new();

        #endregion

        #region Lifecycle Methods

        /// <exclude />
        /// <summary>
        /// Called by the framework to initialize the component. Binds to the parent series via tracker
        /// and registers these settings with the series.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Tracker is ChartSeries chartSeries)
            {
                Series = chartSeries;
            }

            Series?.UpdateSeriesProperties("EmptyPointSettings", this);
        }

        /// <exclude />
        /// <summary>
        /// Called by the framework when component parameters have been set.
        /// Propagates the current empty point settings to the owning series.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (IsRendered)
            {
                if (_mode != Mode)
                {
                    _mode = Mode;
                    if (Series is not null)
                    {
                        Series.Renderer?.UpdateEmptyPoint();
                    }
                }

                if (_fill != Fill)
                {
                    _fill = Fill;
                    if (Series is not null)
                    {
                        Series.Marker.Renderer?.UpdateDirection();
                        Series.Renderer?.UpdateEmptyPoint();
                    }
                }

                if (!Equals(_border, Border))
                {
                    _border = Border;
                    if (Series is not null && Series.Renderer?.Owner is not null && Series.Renderer.Owner._isChartFirstRender && Series.Renderer.IsSeriesRender)
                    {
                        Series.Renderer.UpdateEmptyPoint();
                    }
                }

                Series?.UpdateSeriesProperties("EmptyPointSettings", this);
            }
        }

        /// <summary>
        /// Disposes the component and its owned resources.
        /// </summary>
        protected override ValueTask DisposeAsyncCore()
        {
            ComponentDispose();
            return base.DisposeAsyncCore();
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Disposes the component and its owned resources.
        /// </summary>
        internal void ComponentDispose()
        {
            Series = null;
            Border?.ComponentDispose();
            _border = null;
        }
        /// <summary>
        /// Updates child property references for the empty point settings.
        /// </summary>
        /// <param name="key">The child property key to update. Expected value: <c>"Border"</c>.</param>
        /// <param name="keyValue">The new value for the specified key.</param>
        internal void UpdateEmptyPointProperties(string key, object keyValue)
        {
            if (key == "Border")
            {
                Border = (ChartEmptyPointBorder)keyValue;
            }
        }

        #endregion
    }
}
