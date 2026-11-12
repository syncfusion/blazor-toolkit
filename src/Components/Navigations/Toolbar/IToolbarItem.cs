using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit
{
    /// <exclude />
    /// <summary>
    /// Contract implemented by every component that takes part in toolbar registration, keyboard navigation and overflow.
    /// </summary>
    internal interface IToolbarItem
    {
        /// <exclude />
        /// <summary>
        /// Gets the component instance behind this item.
        /// </summary>
        ComponentBase Component { get; }

        /// <exclude />
        /// <summary>
        /// Gets the identifier of the item. Never null: an item without an explicit identifier receives a generated one.
        /// </summary>
        string ItemId { get; }

        /// <exclude />
        /// <summary>
        /// Gets whether the item is rendered.
        /// </summary>
        bool IsVisible { get; }

        /// <exclude />
        /// <summary>
        /// Gets whether the item is interactive.
        /// </summary>
        bool IsEnabled { get; }

        /// <exclude />
        /// <summary>
        /// Gets whether the item can hold keyboard focus (visible, enabled and a command).
        /// </summary>
        bool IsFocusable { get; }

        /// <exclude />
        /// <summary>
        /// Gets how strongly the item prefers to stay in the main bar.
        /// </summary>
        ToolbarOverflowOption OverflowOption { get; }

        /// <exclude />
        /// <summary>
        /// Gets the secondary overflow sort key.
        /// </summary>
        int OverflowPriority { get; }

        /// <exclude />
        /// <summary>
        /// Gets the tab index the item last rendered with, or <see cref="int.MinValue"/> before its first render.
        /// The host compares it with the wanted value to refresh only the items whose tab stop changed.
        /// </summary>
        int RenderedTabIndex { get; }

        /// <exclude />
        /// <summary>
        /// Gets whether the item takes part in overflow measurement: a visible command or template item, not a separator or spacer.
        /// </summary>
        bool IsMeasurable { get; }

        /// <exclude />
        /// <summary>
        /// Gets or sets whether the item currently sits in the overflow popup instead of the main bar.
        /// </summary>
        bool IsOverflowed { get; set; }

        /// <exclude />
        /// <summary>
        /// Gets the text shown for the item in the overflow popup.
        /// </summary>
        string? DisplayText { get; }

        /// <exclude />
        /// <summary>
        /// Gets the icon class shown for the item in the overflow popup.
        /// </summary>
        string? DisplayIconCss { get; }

        /// <exclude />
        /// <summary>
        /// Gets the selection group name, or <see langword="null"/> when the item is not grouped.
        /// </summary>
        string? SelectionGroup { get; }

        /// <exclude />
        /// <summary>
        /// Gets whether selecting this item clears other items in the same group.
        /// </summary>
        bool IsRadio { get; }

        /// <exclude />
        /// <summary>
        /// Gets whether the item keeps an independent selected state inside a check group.
        /// </summary>
        bool IsCheck { get; }

        /// <exclude />
        /// <summary>
        /// Gets whether the item is currently selected.
        /// </summary>
        bool IsSelected { get; }

        /// <exclude />
        /// <summary>
        /// Clears a radio selection without raising the item click.
        /// </summary>
        /// <returns>A task that completes when the change has been raised.</returns>
        Task ClearSelectionAsync();

        /// <exclude />
        /// <summary>
        /// Sets the tab index used while this item is an entry in an open nested menu.
        /// </summary>
        /// <param name="index">0 for the focused entry, otherwise -1. <see langword="null"/> returns control to the toolbar.</param>
        void SetNestedTabIndex(int? index);

        /// <exclude />
        /// <summary>
        /// Runs the same click flow as a click on the item in the bar: item handler, toolbar handler, cancel, toggle.
        /// </summary>
        /// <param name="args">The mouse event arguments of the activating click.</param>
        /// <returns>A task that completes when the flow has finished.</returns>
        Task ActivateAsync(Microsoft.AspNetCore.Components.Web.MouseEventArgs args);

        /// <exclude />
        /// <summary>
        /// Moves keyboard focus to the item.
        /// </summary>
        /// <returns>A task that completes when focus has moved.</returns>
        ValueTask FocusItemAsync();

        /// <exclude />
        /// <summary>
        /// Re-renders only this item.
        /// </summary>
        void Refresh();
    }
}
