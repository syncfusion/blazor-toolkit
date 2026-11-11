namespace Syncfusion.Blazor.Toolkit.Internal
{
    /// <summary>
    /// Pure layout and gesture helpers used by <see cref="SfSidebar"/>.
    /// </summary>
    /// <exclude />
    internal static class SidebarLayout
    {
        /// <summary>
        /// Resolves the configured <see cref="SidebarPosition"/> to a physical edge.
        /// </summary>
        /// <param name="position">The configured position.</param>
        /// <param name="isRtl">Whether the global right-to-left option is enabled.</param>
        /// <returns><see langword="true"/> when the sidebar is docked at the right edge; otherwise, <see langword="false"/>.</returns>
        internal static bool ResolveToRight(SidebarPosition position, bool isRtl)
        {
            return position switch
            {
                SidebarPosition.Right => true,
                SidebarPosition.Left => false,
                SidebarPosition.End => !isRtl,
                SidebarPosition.Start => isRtl,
                _ => isRtl,
            };
        }

        /// <summary>
        /// Returns the viewport width in pixels below which <paramref name="breakpoint"/> selects mobile behavior.
        /// </summary>
        /// <param name="breakpoint">The configured breakpoint.</param>
        /// <returns>The exact threshold, or <c>0</c> when the breakpoint never selects mobile behavior (<see cref="SidebarBreakpoint.None"/>, <see cref="SidebarBreakpoint.Xs"/>, <see cref="SidebarBreakpoint.Always"/>).</returns>
        internal static int GetBreakpointThreshold(SidebarBreakpoint breakpoint)
        {
            return breakpoint switch
            {
                SidebarBreakpoint.Sm => 576,
                SidebarBreakpoint.Md => 768,
                SidebarBreakpoint.Lg => 992,
                SidebarBreakpoint.Xl => 1200,
                SidebarBreakpoint.Xxl => 1400,
                SidebarBreakpoint.None => 0,
                SidebarBreakpoint.Xs => 0,
                SidebarBreakpoint.Always => 0,
                _ => 0,
            };
        }

        /// <summary>
        /// Returns <paramref name="value"/> when it is a plain CSS length expression; otherwise <paramref name="fallback"/>.
        /// </summary>
        /// <param name="value">The user supplied CSS length such as <c>280px</c> or <c>calc(100% - 2rem)</c>.</param>
        /// <param name="fallback">The value returned when <paramref name="value"/> is empty or unsafe.</param>
        /// <returns>A value that is safe to write into a CSS custom property.</returns>
        /// <remarks>
        /// The value is rendered into an inline <c>style</c> attribute, so declaration separators, blocks, quotes,
        /// at-rules, comments and <c>url(</c> are rejected to prevent CSS injection.
        /// </remarks>
        internal static string SanitizeLength(string? value, string fallback)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return fallback;
            }

            string trimmed = value.Trim();
            foreach (char character in trimmed)
            {
                bool allowed = char.IsAsciiLetterOrDigit(character) || character is '.' or '%' or '-' or '+' or '*' or '/' or '(' or ')' or ',' or ' ';
                if (!allowed)
                {
                    return fallback;
                }
            }

            return trimmed.Contains("/*", StringComparison.Ordinal)
                || trimmed.Contains("url(", StringComparison.OrdinalIgnoreCase)
                || trimmed.Contains("expression(", StringComparison.OrdinalIgnoreCase)
                ? fallback
                : trimmed;
        }

        /// <summary>
        /// Interprets a horizontal swipe.
        /// </summary>
        /// <param name="toRight">Whether the sidebar is docked at the right edge.</param>
        /// <param name="isOpen">The current open state.</param>
        /// <param name="deltaX">Horizontal pointer travel in pixels. Positive values move right.</param>
        /// <param name="deltaY">Vertical pointer travel in pixels.</param>
        /// <param name="threshold">The minimum horizontal travel that counts as a swipe.</param>
        /// <returns><see langword="true"/> to open, <see langword="false"/> to close, or <see langword="null"/> when no state change is requested.</returns>
        internal static bool? GetSwipeResult(bool toRight, bool isOpen, double deltaX, double deltaY, double threshold)
        {
            if (Math.Abs(deltaX) < threshold || Math.Abs(deltaX) <= Math.Abs(deltaY))
            {
                return null;
            }

            bool towardsContent = toRight ? deltaX < 0 : deltaX > 0;
            return !isOpen && towardsContent ? true : isOpen && !towardsContent ? false : null;
        }
    }
}
