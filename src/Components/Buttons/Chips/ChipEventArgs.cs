using Microsoft.AspNetCore.Components.Web;

namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>Cancelable removal data for <see cref="SfChip"/>.</summary>
    public class ChipRemovingEventArgs
    {
        /// <summary>Chip text used when no data item exists.</summary>
        public string? Text { get; init; }

        /// <summary>Resolved key, if any.</summary>
        public object? Key { get; init; }

        /// <summary>Removal reason.</summary>
        public ChipRemoveReason Reason { get; init; }

        /// <summary>Set to <c>true</c> to cancel removal.</summary>
        public bool Cancel { get; set; }
    }

    /// <summary>Cancelable removal data for <see cref="SfChipSet{TItem}"/>.</summary>
    /// <typeparam name="TItem">Data item type.</typeparam>
    public class ChipRemovingEventArgs<TItem> : ChipRemovingEventArgs
    {
        /// <summary>Item being removed.</summary>
        public TItem Item { get; init; } = default!;
    }

    /// <summary>Data after a chip has been removed from <see cref="SfChip"/>.</summary>
    public class ChipRemovedEventArgs
    {
        /// <summary>Chip text when no data item exists.</summary>
        public string? Text { get; init; }

        /// <summary>Resolved key, if any.</summary>
        public object? Key { get; init; }

        /// <summary>Removal reason.</summary>
        public ChipRemoveReason Reason { get; init; }
    }

    /// <summary>Data after a chip has been removed from <see cref="SfChipSet{TItem}"/>.</summary>
    /// <typeparam name="TItem">Data item type.</typeparam>
    public class ChipRemovedEventArgs<TItem> : ChipRemovedEventArgs
    {
        /// <summary>Removed item.</summary>
        public TItem Item { get; init; } = default!;
    }

    /// <summary>Cancelable selection change data.</summary>
    /// <typeparam name="TItem">Data item type.</typeparam>
    public class ChipSelectionChangingEventArgs<TItem>
    {
        /// <summary>Item associated with the change, if any.</summary>
        public TItem? Item { get; init; }

        /// <summary>Value associated with the change.</summary>
        public object? Value { get; init; }

        /// <summary>Selected values before the change.</summary>
        public IReadOnlyList<object?> CurrentSelectedValues { get; init; } = [];

        /// <summary>Selected values that will apply if the change is not canceled.</summary>
        public IReadOnlyList<object?> ProposedSelectedValues { get; init; } = [];

        /// <summary>Set to <c>true</c> to cancel the selection change.</summary>
        public bool Cancel { get; set; }
    }

    /// <summary>Selection change data after a successful update.</summary>
    /// <typeparam name="TItem">Data item type.</typeparam>
    public class ChipSelectionChangedEventArgs<TItem>
    {
        /// <summary>Item associated with the change, if any.</summary>
        public TItem? Item { get; init; }

        /// <summary>Value associated with the change.</summary>
        public object? Value { get; init; }

        /// <summary>Selected values after the change.</summary>
        public IReadOnlyList<object?> SelectedValues { get; init; } = [];
    }

    /// <summary>Activation data that is not a remove action.</summary>
    /// <typeparam name="TItem">Data item type.</typeparam>
    public class ChipClickEventArgs<TItem>
    {
        /// <summary>Activated item.</summary>
        public TItem Item { get; init; } = default!;

        /// <summary>Resolved key, if any.</summary>
        public object? Key { get; init; }

        /// <summary>Pointer arguments when activation came from a pointer device.</summary>
        public MouseEventArgs? OriginalArgs { get; init; }
    }
}
