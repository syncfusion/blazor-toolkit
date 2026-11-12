using Microsoft.AspNetCore.Components.Web;

namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>
    /// Data model for one command when items are supplied through <c>SfToolbar.Items</c>.
    /// </summary>
    public class ToolbarItemModel
    {
        /// <summary>
        /// Gets or sets the unique identifier of the item.
        /// </summary>
        /// <value>An identifier string. The default is <see langword="null"/>.</value>
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the display text and accessible name.
        /// </summary>
        /// <value>A text string. The default is <see langword="null"/>.</value>
        public string? Text { get; set; }

        /// <summary>
        /// Gets or sets the CSS class of the icon.
        /// </summary>
        /// <value>A space-separated list of CSS class names. The default is <see langword="null"/>.</value>
        public string? IconCss { get; set; }

        /// <summary>
        /// Gets or sets the item type.
        /// </summary>
        /// <value>A <see cref="ToolbarItemType"/> value. The default is <see cref="ToolbarItemType.Button"/>.</value>
        public ToolbarItemType Type { get; set; } = ToolbarItemType.Button;

        /// <summary>
        /// Gets or sets whether the item is interactive.
        /// </summary>
        /// <value><see langword="true"/> to make the item interactive; otherwise, <see langword="false"/>. The default is <see langword="true"/>.</value>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Gets or sets whether the item is rendered.
        /// </summary>
        /// <value><see langword="true"/> to render the item; otherwise, <see langword="false"/>. The default is <see langword="true"/>.</value>
        public bool Visible { get; set; } = true;

        /// <summary>
        /// Gets or sets the selected state of a toggle item.
        /// </summary>
        /// <value><see langword="true"/> when selected. The default is <see langword="false"/>.</value>
        public bool Selected { get; set; }

        /// <summary>
        /// Gets or sets how strongly the item prefers to stay in the main bar.
        /// </summary>
        /// <value>A <see cref="ToolbarOverflowOption"/> value. The default is <see cref="ToolbarOverflowOption.None"/>.</value>
        public ToolbarOverflowOption Overflow { get; set; } = ToolbarOverflowOption.None;

        /// <summary>
        /// Gets or sets the secondary overflow sort key. Lower values stay visible longer.
        /// </summary>
        /// <value>An integer. The default is 0.</value>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the supplemental tooltip text.
        /// </summary>
        /// <value>A text string. The default is <see langword="null"/>.</value>
        public string? Tooltip { get; set; }

        /// <summary>
        /// Gets or sets the badge content.
        /// </summary>
        /// <value>A text string. The default is <see langword="null"/>.</value>
        public string? Badge { get; set; }

        /// <summary>
        /// Gets or sets the name of the mutually exclusive group the item belongs to.
        /// </summary>
        /// <value>A group name. The default is <see langword="null"/>.</value>
        public string? GroupName { get; set; }

        /// <summary>
        /// Gets or sets whether the icon, the text, or both are shown.
        /// </summary>
        /// <value>A <see cref="ToolbarItemDisplayMode"/> value. The default is <see cref="ToolbarItemDisplayMode.IconAndText"/>.</value>
        public ToolbarItemDisplayMode DisplayMode { get; set; } = ToolbarItemDisplayMode.IconAndText;
    }

    /// <summary>
    /// Advanced overflow configuration for a <see cref="SfToolbar"/>.
    /// </summary>
    public class ToolbarOverflowSettings
    {
        /// <summary>
        /// Gets or sets whether the "More" button is shown when items overflow.
        /// </summary>
        /// <value><see langword="true"/> to show the button. The default is <see langword="true"/>.</value>
        public bool ShowOverflowButton { get; set; } = true;

        /// <summary>
        /// Gets or sets the CSS class of the overflow button icon.
        /// </summary>
        /// <value>A space-separated list of CSS class names. The default is <see langword="null"/>.</value>
        public string? OverflowButtonIconCss { get; set; }

        /// <summary>
        /// Gets or sets the tooltip of the overflow button.
        /// </summary>
        /// <value>A text string. The default is <c>"More"</c>.</value>
        public string OverflowButtonTooltip { get; set; } = "More";

        /// <summary>
        /// Gets or sets additional CSS classes for the overflow popup.
        /// </summary>
        /// <value>A space-separated list of CSS class names. The default is <see langword="null"/>.</value>
        public string? PopupCssClass { get; set; }

        /// <summary>
        /// Gets or sets whether the popup closes after an item is selected.
        /// </summary>
        /// <value><see langword="true"/> to close on selection. The default is <see langword="true"/>.</value>
        public bool CloseOnSelect { get; set; } = true;
    }

    /// <summary>
    /// Context supplied to item templates.
    /// </summary>
    public class ToolbarTemplateContext
    {
        /// <summary>
        /// Gets or sets the data item being rendered.
        /// </summary>
        /// <value>A <see cref="ToolbarItemModel"/>.</value>
        public ToolbarItemModel Item { get; set; } = new();

        /// <summary>
        /// Gets or sets whether the item is rendered inside the overflow menu.
        /// </summary>
        /// <value><see langword="true"/> inside the overflow menu; otherwise, <see langword="false"/>.</value>
        public bool IsOverflow { get; set; }
    }

    /// <summary>
    /// Event arguments for <c>SfToolbar.OnItemClick</c>.
    /// </summary>
    /// <remarks>
    /// The item-level <c>OnClick</c> runs first. <c>OnItemClick</c> then always fires, even when <see cref="Cancel"/> is set.
    /// <see cref="Cancel"/> stops only the default action of the item.
    /// </remarks>
    public class ToolbarItemClickEventArgs
    {
        /// <summary>
        /// Gets or sets the identifier of the clicked item.
        /// </summary>
        /// <value>An identifier string.</value>
        public string ItemId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the data model of the clicked item when the item is data-bound.
        /// </summary>
        /// <value>A <see cref="ToolbarItemModel"/>, or <see langword="null"/> for a declarative item.</value>
        public ToolbarItemModel? Item { get; set; }

        /// <summary>
        /// Gets or sets the original browser mouse event arguments.
        /// </summary>
        /// <value>A <see cref="MouseEventArgs"/>.</value>
        public MouseEventArgs? OriginalArgs { get; set; }

        /// <summary>
        /// Gets or sets whether the default action of the item is stopped.
        /// </summary>
        /// <value>
        /// <see langword="true"/> to keep a toggle's <c>Selected</c> value and to stop a split, drop-down or menu item from opening.
        /// The default is <see langword="false"/>. <c>OnItemClick</c> still fires.
        /// </value>
        public bool Cancel { get; set; }
    }
}
