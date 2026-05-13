using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides options to configure a linear gradient for a chart <see cref="ChartSeries"/>,
    /// or <see cref="ChartTrendline"/> within the <see cref="SfChart"/> component.
    /// </summary>
    /// <remarks>
    /// <see cref="ChartLinearGradient"/> is a child component that inherits from <see cref="LinearGradient"/>. Use
    /// <see cref="LinearGradient.X1"/>, <see cref="LinearGradient.Y1"/>, <see cref="LinearGradient.X2"/>, and <see cref="LinearGradient.Y2"/>
    /// to set the gradient direction, and define color transitions using <see cref="ChartGradientColorStops"/> with one or more
    /// <see cref="ChartGradientColorStop"/> elements.
    ///
    /// Coordinate values are typically normalized to the range 0..1 relative to the paint box, where <c>(0,0)</c> is the top-left and
    /// <c>(1,1)</c> is the bottom-right.
    ///
    /// When nested under <see cref="ChartSeries"/>, or <see cref="ChartTrendline"/>, the gradient is applied
    /// automatically to the owning element.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// @using Syncfusion.Blazor.Toolkit.Charts
    /// 
    /// <SfChart>
    ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
    ///     <ChartSeriesCollection>
    ///         <ChartSeries Type="ChartSeriesType.Line"
    ///                      DataSource="@Data"
    ///                      XName="Month"
    ///                      YName="Value"
    ///                      Name="Sales">
    ///             <ChartLinearGradient X1="0" Y1="0" X2="0" Y2="1">
    ///                 <ChartGradientColorStops>
    ///                     <ChartGradientColorStop Offset="0"   Color="#4F46E5" Opacity="1" />
    ///                     <ChartGradientColorStop Offset="100" Color="#22D3EE" Opacity="1" />
    ///                 </ChartGradientColorStops>
    ///             </ChartLinearGradient>
    ///         </ChartSeries>
    ///     </ChartSeriesCollection>
    /// </SfChart>
    /// 
    /// @code {
    ///     public class Point { public string Month { get; set; } = ""; public double Value { get; set; } }
    ///     public List<Point> Data = new()
    ///     {
    ///         new() { Month = "Jan", Value = 35 },
    ///         new() { Month = "Feb", Value = 28 },
    ///         new() { Month = "Mar", Value = 34 },
    ///     };
    /// }
    /// ]]></code>
    /// </example>
    public class ChartLinearGradient : LinearGradient, ISubcomponentTracker
    {
        #region Properties

        /// <summary>
        /// Gets the parent <see cref="ChartSeries"/> when applied directly to a series.
        /// </summary>
        /// <value>The series owner, if any; otherwise, <c>null</c>.</value>
        [CascadingParameter]
        internal ChartSeries? Series { get; set; }

        /// <summary>
        /// Gets the parent <see cref="ChartTrendline"/> when applied to a trendline.
        /// </summary>
        /// <value>The trendline owner, if any; otherwise, <c>null</c>.</value>
        [CascadingParameter]
        internal ChartTrendline? Trendline { get; set; }

        #endregion

        #region Lifecycle Methods

        /// <exclude />
        /// <summary>
        /// Initializes the component and registers the gradient with its owning chart element.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Tracker is ChartSeries chartSeries)
            {
                Series = chartSeries;
                Series.UpdateSeriesProperties("LinearGradient", this);
            }
            else if (Tracker is ChartTrendline chartTrendline)
            {
                Trendline = chartTrendline;
                Trendline.UpdateTrendlineProperty("LinearGradient", this);
            }
        }

        /// <exclude />
        /// <summary>
        /// Applies parameter changes and re-registers the gradient with its owner.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (Series is not null)
            {
                Series.UpdateSeriesProperties("LinearGradient", this);
            }
            else
            {
                Trendline?.UpdateTrendlineProperty("LinearGradient", this);
            }
        }

        /// <summary>
        /// Releases references to allow garbage collection.
        /// </summary>
        protected override ValueTask DisposeAsyncCore()
        {
            ComponentDispose();
            return base.DisposeAsyncCore();
        }
        internal void ComponentDispose()
        {
            Series = null;
            Trendline = null;
        }
        #endregion
    }
}