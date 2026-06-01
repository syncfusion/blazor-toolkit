namespace Syncfusion.Blazor.Toolkit.Internal
{
    /// <summary>
    /// Configures animation timing, duration, and easing for component transitions.
    /// </summary>
    internal class AnimationSettings
    {
        /// <summary>
        /// Gets or sets the duration of the animation in milliseconds.
        /// </summary>
        /// <value>
        /// An integer representing milliseconds. The default is <c>400</c>.
        /// </value>
        public int Duration { get; set; } = 400;

        /// <summary>
        /// Gets or sets the name of the animation effect to apply.
        /// </summary>
        /// <value>
        /// A string matching one of the supported effect names (for example, <c>"FadeIn"</c> or <c>"SlideLeft"</c>). The default is <c>"FadeIn"</c>.
        /// </value>
        public string Name { get; set; } = "FadeIn";

        /// <summary>
        /// Gets or sets the CSS timing function that controls the acceleration curve of the animation.
        /// </summary>
        /// <value>
        /// A standard CSS easing keyword such as <c>"ease"</c>, <c>"linear"</c>, or <c>"ease-in-out"</c>. The default is <c>"ease"</c>.
        /// </value>
        public string TimingFunction { get; set; } = "ease";

        /// <summary>
        /// Gets or sets the delay before the animation starts, in milliseconds.
        /// </summary>
        /// <value>
        /// An integer representing milliseconds. The default is <c>0</c>.
        /// </value>
        public int Delay { get; set; }
    }
}