namespace Syncfusion.Blazor.Toolkit.Internal
{
    /// <summary>
    /// Metadata for the Toolkit embedded icon font. Each entry is a class that the
    /// theme root already maps and the code point that mapping names. This type
    /// does not emit CSS, embed font bytes, or feed the theme payload.
    /// </summary>
    internal static class ToolkitIconCatalog
    {
        /// <summary>One verified icon: its CSS class and its mapped code point.</summary>
        internal readonly record struct Entry(string ClassName, string CodePoint);

        /// <summary>Icons whose CSS mapping and code point both come from the theme payload.</summary>
        internal static readonly Entry[] _entries =
        [
            new("e-chevron-up", "\uE700"),
            new("e-chevron-down", "\uE701"),
            new("e-chevron-left", "\uE702"),
            new("e-chevron-right", "\uE703"),
            new("e-arrow-up", "\uE704"),
            new("e-arrow-down", "\uE705"),
            new("e-arrow-right", "\uE706"),
            new("e-check", "\uE707"),
            new("e-intermediate-state-2", "\uE708"),
            new("e-close", "\uE709"),
            new("e-timeline-today", "\uE70a"),
            new("e-timeline-day", "\uE70b"),
            new("e-clock", "\uE70c"),
            new("e-pause", "\uE70d"),
            new("e-play", "\uE70e"),
            new("e-refresh", "\uE70f"),
            new("e-trash", "\uE710"),
            new("e-circle-remove", "\uE711"),
            new("e-resizer-right", "\uE712"),
            new("e-more-horizontal-1", "\uE713"),
            new("e-chevron-left-small", "\uE714"),
            new("e-chevron-right-small", "\uE715"),
            new("e-chevron-up-fill", "\uE716"),
            new("e-chevron-down-fill", "\uE717"),
            new("e-chevron-left-fill", "\uE718"),
            new("e-chevron-right-fill", "\uE719"),
            new("e-reset", "\uE71a"),
            new("e-star-filled", "\uE71b"),
            new("e-chevron-down-small", "\uE71c"),
            new("e-chevron-up-small", "\uE71d"),
            new("e-resizer-rtl", "\uE71e"),
            new("e-input", "\uE71f"),
            new("e-chart", "\uE720"),
            new("e-spinner", "\uE721"),
            new("e-popups", "\uE722"),
            new("e-button", "\uE723"),
            new("e-explore", "\uE724"),
            new("e-system-requirement", "\uE725"),
            new("e-browser-support", "\uE726"),
            new("e-right-left", "\uE727"),
            new("e-state-persistence", "\uE728"),
            new("e-globalization", "\uE729"),
            new("e-text-matrix", "\uE72a"),
            new("e-release-notes", "\uE72b"),
            new("e-api-reference", "\uE72c"),
            new("e-getting-started", "\uE72d"),
            new("e-calendars", "\uE72e"),
        ];
    }
}
