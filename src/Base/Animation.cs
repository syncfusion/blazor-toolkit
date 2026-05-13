namespace Syncfusion.Blazor.Toolkit.Internal
{
    /// <summary>
    /// Animation properties for performing animation transition.
    /// </summary>
    internal class AnimationSettings
    {
        /// <summary>
        /// Gets or sets the animation duration.
        /// </summary>
        public int Duration { get; set; } = 400;

        /// <summary>
        /// Gets or sets the animation name.
        /// </summary>
        public string Name { get; set; } = "FadeIn";

        /// <summary>
        /// Gets or sets the animation timing function.
        /// </summary>
        public string TimingFunction { get; set; } = "ease";

        /// <summary>
        /// Gets or sets the animation delay.
        /// </summary>
        public int Delay { get; set; }
    }
}