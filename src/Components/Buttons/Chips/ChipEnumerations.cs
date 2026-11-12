namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>Specifies the functional and semantic behavior of a chip.</summary>
    /// <remarks>Passive modes (<see cref="Informational"/>, <see cref="Status"/>) must not be exposed as buttons. Interactive modes use native elements appropriate to the mode.</remarks>
    public enum ChipType
    {
        /// <summary>Non-interactive informational content.</summary>
        Informational = 0,

        /// <summary>Non-interactive status content.</summary>
        Status = 1,

        /// <summary>Command activation using a native button.</summary>
        Action = 2,

        /// <summary>Assistive action using a native button.</summary>
        Assist = 3,

        /// <summary>Toggleable filter value; uses a pressed state.</summary>
        Filter = 4,

        /// <summary>Mutually exclusive choice within a chip set.</summary>
        Choice = 5,

        /// <summary>User token that is typically removable.</summary>
        Input = 6,

        /// <summary>Suggestion that notifies the host when activated.</summary>
        Suggestion = 7,

        /// <summary>Navigation chip rendered as a link when <c>Href</c> is set.</summary>
        Navigation = 8,

        /// <summary>Author-defined behavior via templates and attributes.</summary>
        Custom = 9,
    }

    /// <summary>Specifies selection behavior for a chip set.</summary>
    public enum ChipSelectionMode
    {
        /// <summary>Selection is not tracked.</summary>
        None = 0,

        /// <summary>At most one value may be selected.</summary>
        Single = 1,

        /// <summary>Zero or more values may be selected.</summary>
        Multiple = 2,
    }

    /// <summary>Visual surface style of a chip.</summary>
    public enum ChipVariant
    {
        /// <summary>Filled surface.</summary>
        Filled = 0,

        /// <summary>Outlined surface.</summary>
        Outlined = 1,
    }

    /// <summary>Chip size density.</summary>
    public enum ChipSize
    {
        /// <summary>Compact density.</summary>
        Small = 0,

        /// <summary>Default density.</summary>
        Medium = 1,

        /// <summary>Comfortable density.</summary>
        Large = 2,
    }

    /// <summary>Corner shape of a chip.</summary>
    public enum ChipShape
    {
        /// <summary>Standard rounded corners.</summary>
        Rounded = 0,

        /// <summary>Pill-shaped ends.</summary>
        Pill = 1,
    }

    /// <summary>How a chip set handles limited horizontal space.</summary>
    public enum ChipOverflowMode
    {
        /// <summary>Wrap onto additional lines.</summary>
        Wrap = 0,

        /// <summary>Single line with horizontal scrolling.</summary>
        Scroll = 1,

        /// <summary>Collapse overflowing chips behind an expand control.</summary>
        Collapse = 2,
    }

    /// <summary>Why a chip removal was requested.</summary>
    public enum ChipRemoveReason
    {
        /// <summary>Remove control activation.</summary>
        RemoveButton = 0,

        /// <summary>Delete or Backspace key.</summary>
        Keyboard = 1,

        /// <summary>Public API method.</summary>
        Programmatic = 2,
    }
}
