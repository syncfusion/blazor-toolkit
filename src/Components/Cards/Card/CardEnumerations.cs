namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>
    /// Specifies the visual style of an <see cref="SfCard"/>.
    /// </summary>
    public enum CardVariant
    {
        /// <summary>
        /// Solid surface using the theme card background and default elevation.
        /// </summary>
        Filled = 0,

        /// <summary>
        /// Emphasizes the border with a lighter or neutral fill and no shadow.
        /// </summary>
        Outlined = 1,

        /// <summary>
        /// Stronger drop shadow for emphasis; respects <see cref="SfCard.Elevation"/>.
        /// </summary>
        Elevated = 2,

        /// <summary>
        /// Minimal chrome with no border and no shadow.
        /// </summary>
        Flat = 3
    }

    /// <summary>
    /// Specifies how regions inside an <see cref="SfCard"/> are laid out.
    /// </summary>
    public enum CardOrientation
    {
        /// <summary>
        /// Stacks header, media, content, actions, and footer from top to bottom.
        /// </summary>
        Vertical = 0,

        /// <summary>
        /// Arranges primary regions in a horizontal flex row. Inline direction respects RTL.
        /// </summary>
        Horizontal = 1
    }

    /// <summary>
    /// Specifies how media content is fitted inside <see cref="SfCardMedia"/>.
    /// </summary>
    public enum CardMediaFit
    {
        /// <summary>
        /// Fills the media box and may crop overflow (CSS <c>object-fit: cover</c>).
        /// </summary>
        Cover = 0,

        /// <summary>
        /// Letterboxes media within the box (CSS <c>object-fit: contain</c>).
        /// </summary>
        Contain = 1,

        /// <summary>
        /// Stretches media to the box (CSS <c>object-fit: fill</c>).
        /// </summary>
        Fill = 2,

        /// <summary>
        /// Applies no <c>object-fit</c> override; intrinsic sizing applies.
        /// </summary>
        None = 3
    }
}
