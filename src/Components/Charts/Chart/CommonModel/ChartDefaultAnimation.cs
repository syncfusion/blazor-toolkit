using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides options for configuring animation behavior in chart series.
    /// </summary>
    public class ChartDefaultAnimation : ChartSubComponent
    {
        #region Properties

        /// <summary>
        /// Gets the parent container that owns this animation configuration.
        /// </summary>
        /// <value>
        /// The <see cref="SfChart"/> instance, or <c>null</c> if not initialized within a chart context.
        /// </value>
        [CascadingParameter]
        internal SfChart? Container { get; set; }

        /// <summary>
        /// Gets or sets the delay for the animation of the series.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> representing the delay in milliseconds before the animation starts. The default value is <b>0</b>.
        /// </value>
        /// <remarks>
        /// Use this property to specify how long the chart should wait before beginning the animation.
        /// This can help in synchronizing complex animations or staggering them for visual effects.
        /// </remarks>
        [Parameter]
        public double Delay { get; set; } = 0;

        /// <summary>
        /// Gets or sets the duration of the animation in milliseconds.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> representing the total time span of the animation in milliseconds. The default value is <b>1000</b>.
        /// </value>
        /// <remarks>
        /// Adjusting this property allows you to speed up or slow down the animation, making it more visually appealing based on the specific chart's context.
        /// </remarks>
        [Parameter]
        public double Duration { get; set; } = 1000;

        /// <summary>
        /// Gets or sets a value indicating whether the series is animated on initial loading.
        /// </summary>
        /// <value>
        /// <c>true</c> if the series animation is enabled on initial load; otherwise, <c>false</c>. The default value is <c>true</c>.
        /// </value>
        /// <remarks>
        /// Setting this property to <c>true</c> makes the series elements animate when the chart first loads,
        /// enhancing the visual engagement of the chart presentation. Disable it for static displays.
        /// </remarks>
        [Parameter]
        public bool Enable { get; set; } = true;

        #endregion

        #region Internal Methods

        /// <summary>
        /// Updates the animation settings for trendline animations.
        /// </summary>
        /// <param name="enable">Whether to enable trendline animation.</param>
        /// <param name="duration">The animation duration in milliseconds.</param>
        /// <param name="delay">The animation delay in milliseconds.</param>
        internal void SetTrendlineAnimation(bool enable, double duration, double delay)
        {
            Enable = enable;
            Duration = duration;
            Delay = delay;
        }
        #endregion
    }
}