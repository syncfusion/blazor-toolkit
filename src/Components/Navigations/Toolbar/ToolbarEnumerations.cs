namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>
    /// Specifies the layout direction of a <see cref="SfToolbar"/>.
    /// </summary>
    public enum ToolbarOrientation
    {
        /// <summary>
        /// Items are arranged along the inline axis, left to right in a left-to-right layout. This is the default.
        /// </summary>
        Horizontal,

        /// <summary>
        /// Items are arranged top to bottom.
        /// </summary>
        Vertical
    }

    /// <summary>
    /// Specifies how items are aligned inside a <see cref="SfToolbar"/>.
    /// </summary>
    public enum ToolbarAlignment
    {
        /// <summary>
        /// Items are packed toward the start edge. This is the default.
        /// </summary>
        Start,

        /// <summary>
        /// Items are centered.
        /// </summary>
        Center,

        /// <summary>
        /// Items are packed toward the end edge.
        /// </summary>
        End,

        /// <summary>
        /// Items are evenly distributed across the available space.
        /// </summary>
        Justify
    }

    /// <summary>
    /// Specifies the strategy a <see cref="SfToolbar"/> uses when its items do not fit in the available space.
    /// </summary>
    public enum ToolbarOverflowMode
    {
        /// <summary>
        /// Items stay on one line and the bar scrolls.
        /// </summary>
        Scrollable,

        /// <summary>
        /// Items that do not fit move into a "More" menu. This is the default.
        /// </summary>
        Popup,

        /// <summary>
        /// Items that do not fit wrap onto additional rows.
        /// </summary>
        MultiRow,

        /// <summary>
        /// Extra rows are collapsed behind an expand button.
        /// </summary>
        Extended
    }

    /// <summary>
    /// Specifies how strongly an item prefers to stay in the main bar when space is short.
    /// </summary>
    public enum ToolbarOverflowOption
    {
        /// <summary>
        /// The item stays in the main bar in preference to other items.
        /// </summary>
        Show,

        /// <summary>
        /// The item moves to the overflow menu before other items.
        /// </summary>
        Hide,

        /// <summary>
        /// The item follows normal document order. This is the default.
        /// </summary>
        None
    }

    /// <summary>
    /// Specifies the behavior and appearance of a <see cref="ToolbarItem"/>.
    /// </summary>
    public enum ToolbarItemType
    {
        /// <summary>
        /// A standard push button. This is the default.
        /// </summary>
        Button,

        /// <summary>
        /// A button that keeps a selected or unselected state. With a non-empty group name it joins a radio group.
        /// </summary>
        Toggle,

        /// <summary>
        /// A button that keeps a selected state and can stay selected with others that share its group name.
        /// </summary>
        Check,

        /// <summary>
        /// A primary action with a secondary drop-down.
        /// </summary>
        Split,

        /// <summary>
        /// A button that opens a drop-down list.
        /// </summary>
        DropDown,

        /// <summary>
        /// A button that opens a menu.
        /// </summary>
        Menu,

        /// <summary>
        /// A visual separator.
        /// </summary>
        Separator,

        /// <summary>
        /// A flexible space.
        /// </summary>
        Spacer,

        /// <summary>
        /// Custom content supplied by a template.
        /// </summary>
        Template
    }

    /// <summary>
    /// Specifies whether a <see cref="ToolbarItem"/> shows its icon, its text, or both.
    /// </summary>
    public enum ToolbarItemDisplayMode
    {
        /// <summary>
        /// Only the icon is shown. The text stays as the accessible name.
        /// </summary>
        IconOnly,

        /// <summary>
        /// Only the text is shown.
        /// </summary>
        TextOnly,

        /// <summary>
        /// Both the icon and the text are shown. This is the default.
        /// </summary>
        IconAndText
    }
}
