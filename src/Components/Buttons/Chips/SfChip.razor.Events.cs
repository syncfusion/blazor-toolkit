using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Syncfusion.Blazor.Toolkit
{
    public partial class SfChip
    {
        #region Events

        /// <summary>Raised when <see cref="Selected"/> changes.</summary>
        [Parameter]
        public EventCallback<bool> SelectedChanged { get; set; }

        /// <summary>Raised for activation that is not a remove action.</summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }

        /// <summary>Raised before removal. Set <see cref="ChipRemovingEventArgs.Cancel"/> to keep the chip.</summary>
        [Parameter]
        public EventCallback<ChipRemovingEventArgs> Removing { get; set; }

        /// <summary>Raised after a successful removal request.</summary>
        [Parameter]
        public EventCallback<ChipRemovedEventArgs> Removed { get; set; }

        private async Task OnClickAsync(MouseEventArgs args)
        {
            if (ActivationBlocked || !Interactive)
            {
                return;
            }

            if (Owner is not null)
            {
                await Owner.ActivateAsync(Value ?? Text ?? RootId, args).ConfigureAwait(true);
                return;
            }

            if (ChipType is ChipType.Filter or ChipType.Choice)
            {
                bool next = !Selected;
                if (next != Selected)
                {
                    Selected = next;
                    await SelectedChanged.InvokeAsync(next).ConfigureAwait(true);
                }
            }

            await OnClick.InvokeAsync(args).ConfigureAwait(true);
        }

        private async Task OnRemoveClickAsync(MouseEventArgs _)
        {
            if (Owner is not null)
            {
                await Owner.RemoveChildAsync(Value ?? Text ?? RootId, ChipRemoveReason.RemoveButton).ConfigureAwait(true);
                return;
            }

            await RequestRemoveAsync(ChipRemoveReason.RemoveButton).ConfigureAwait(true);
        }

        private async Task OnKeyDownAsync(KeyboardEventArgs args)
        {
            if (args.Key is DeleteKey or BackspaceKey)
            {
                await RequestRemoveAsync(ChipRemoveReason.Keyboard).ConfigureAwait(true);
                return;
            }

            if (Owner is not null && args.Key is ArrowRightKey or ArrowLeftKey or ArrowDownKey or ArrowUpKey or HomeKey or EndKey)
            {
                return;
            }

            if (args.Key is SpaceKey or EnterKey && Interactive && !ActivationBlocked)
            {
                await OnClickAsync(new MouseEventArgs()).ConfigureAwait(true);
            }
        }

        private async Task RequestRemoveAsync(ChipRemoveReason reason)
        {
            if (!Removable || RemoveDisabled || _removed)
            {
                return;
            }

            ChipRemovingEventArgs removing = new()
            {
                Text = Text,
                Key = Value,
                Reason = reason,
            };
            await Removing.InvokeAsync(removing).ConfigureAwait(true);
            if (removing.Cancel)
            {
                return;
            }

            _removed = true;
            await Removed.InvokeAsync(new ChipRemovedEventArgs
            {
                Text = Text,
                Key = Value,
                Reason = reason,
            }).ConfigureAwait(true);
        }

        #endregion
    }
}
