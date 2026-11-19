using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;

namespace Syncfusion.Blazor.Toolkit
{
    public partial class SfChipSet<TItem>
    {
        #region LifeCycle Methods

        /// <inheritdoc />
        protected override void OnParametersSet()
        {
            if (OverflowMode != ChipOverflowMode.Collapse)
            {
                _overflowExpanded = false;
            }

            Rebuild();
            if (OverflowMode == ChipOverflowMode.Collapse && _chipModule is null && JSRuntime is not null)
            {
                _ = ImportCollapseModuleAsync();
            }
        }

        /// <inheritdoc />
        [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "chip.js receives element ids only.")]
        internal override async Task ImportComponentModuleAsync()
        {
            await base.ImportComponentModuleAsync().ConfigureAwait(true);
            await ImportCollapseModuleAsync().ConfigureAwait(true);
        }

        [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "chip.js receives element ids only.")]
        private async Task ImportCollapseModuleAsync()
        {
            if (JSRuntime is null || OverflowMode != ChipOverflowMode.Collapse || _chipModule is not null)
            {
                return;
            }

            IJSObjectReference module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", ScriptPath).ConfigureAwait(true);
            _chipModule = module;
            await InvokeAsync(StateHasChanged).ConfigureAwait(true);
        }

        /// <inheritdoc />
        internal override async Task OnAfterScriptRenderedAsync()
        {
            if (_chipModule is null || OverflowMode != ChipOverflowMode.Collapse)
            {
                return;
            }

            await _chipModule.InvokeVoidAsync(ObserveMethod, RootId, DotnetObjectReference).ConfigureAwait(true);
            _jsReady = true;
        }

        /// <inheritdoc />
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender).ConfigureAwait(true);
            if (_jsReady || OverflowMode != ChipOverflowMode.Collapse || _overflowExpanded || _chipModule is null)
            {
                return;
            }

            await _chipModule.InvokeVoidAsync(ObserveMethod, RootId, DotnetObjectReference).ConfigureAwait(true);
            _jsReady = true;
        }

        /// <summary>
        /// Called by chip.js when collapse measurement changes. Layout pixels stay in Playwright.
        /// <see cref="SfChipSet{TItem}.CollapsedVisibleCount"/> is only the pre-script fallback.
        /// After the first measurement, chip.js may overwrite it so a resize can change the fold.
        /// Measurements are ignored while the set is expanded. An unchanged count does not render again.
        /// </summary>
        [JSInvokable]
        public Task OnOverflowMeasured(int count)
        {
            if (OverflowMode != ChipOverflowMode.Collapse || _overflowExpanded)
            {
                return Task.CompletedTask;
            }

            int visible = _projected.Count(chip => !chip.Removed);
            int upper = Math.Max(visible, 1);
            int next = Math.Clamp(count, visible == 0 ? 0 : 1, upper);
            if (CollapsedVisibleCount == next)
            {
                return Task.CompletedTask;
            }

            CollapsedVisibleCount = next;
            ApplyCollapse();
            StateHasChanged();
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        protected override async ValueTask DisposeAsyncCore()
        {
            try
            {
                if (_chipModule is not null)
                {
                    if (_jsReady)
                    {
                        await _chipModule.InvokeVoidAsync(DisposeMethod, RootId).ConfigureAwait(true);
                    }

                    await _chipModule.DisposeAsync().ConfigureAwait(true);
                }
            }
            catch (JSDisconnectedException)
            {
            }
            catch (JSException)
            {
            }

            await base.DisposeAsyncCore().ConfigureAwait(true);
        }

        #endregion
    }
}
