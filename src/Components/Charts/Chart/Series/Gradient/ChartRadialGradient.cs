using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Represents options to configure a radial gradient for a chart <see cref="ChartSeries"/>,
    /// or <see cref="ChartTrendline"/> within the <see cref="SfChart"/> component.
    /// </summary>
    /// <remarks>
    /// <see cref="ChartRadialGradient"/> is a child component that inherits from <see cref="RadialGradient"/>. Set the center with
    /// <see cref="RadialGradient.Cx"/> and <see cref="RadialGradient.Cy"/>, the focal point with <see cref="RadialGradient.Fx"/> and
    /// <see cref="RadialGradient.Fy"/>, and the radius with <see cref="RadialGradient.R"/>. Define color transitions using
    /// <see cref="ChartGradientColorStops"/> containing one or more <see cref="ChartGradientColorStop"/> elements.
    ///
    /// Coordinates are typically normalized to 0..1 relative to the gradient box; <c>(0.5, 0.5)</c> centers the gradient, and <c>R="0.5"</c>
    /// covers roughly half the box.
    ///
    /// When placed under <see cref="ChartSeries"/>, or <see cref="ChartTrendline"/>, the gradient is applied
    /// automatically.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// @using Syncfusion.Blazor.Toolkit.Charts
    /// 
    /// <SfChart>
    ///     <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
    ///     <ChartSeries Name="Series1"
    ///                   Type="ChartSeriesType.Line"
    ///                   DataSource="@Data"
    ///                   XName="Month"
    ///                   YName="Value">
    ///         <ChartRadialGradient Cx="0.5" Cy="0.5" Fx="0.5" Fy="0.5" R="0.4">
    ///             <ChartGradientColorStops>
    ///                 <ChartGradientColorStop Offset="0"   Color="#EE4256" Opacity="1" />
    ///                 <ChartGradientColorStop Offset="100" Color="#6D83B8" Opacity="1" />
    ///             </ChartGradientColorStops>
    ///         </ChartRadialGradient>
    ///     </ChartSeries>
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
    public class ChartRadialGradient : RadialGradient, ISubcomponentTracker
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
                Series.UpdateSeriesProperties("RadialGradient", this);
            }
            else if (Tracker is ChartTrendline chartTrendline)
            {
                Trendline = chartTrendline;
                Trendline.UpdateTrendlineProperty("RadialGradient", this);
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
                Series.UpdateSeriesProperties("RadialGradient", this);
            }
            else
            {
                Trendline?.UpdateTrendlineProperty("RadialGradient", this);
            }
        }

        /// <summary>
        /// Releases references to allow garbage collection.
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
            Series = null;
            Trendline = null;
        }
        #endregion
    }
}
