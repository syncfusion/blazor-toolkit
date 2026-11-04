using Microsoft.JSInterop;

namespace Syncfusion.Blazor.Toolkit
{
    public partial class SfCard
    {
        #region LifeCycle Methods

        /// <exclude />
        /// <summary>
        /// Resolves the element id once so it stays stable across renders.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(true);
            _generatedId = IdPrefix + GetContextId();
        }

        /// <exclude />
        /// <summary>
        /// Recomputes the cached markup state whenever the parent changes a parameter.
        /// </summary>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(true);
            BuildRenderState();
        }

        /// <exclude />
        /// <summary>
        /// Attaches keyboard activation once the card is clickable. A static card never loads card.js.
        /// </summary>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender).ConfigureAwait(true);
            await AttachKeyboardAsync().ConfigureAwait(true);
        }

        /// <exclude />
        /// <summary>
        /// Detaches the keyboard listeners and releases the card.js module.
        /// </summary>
        protected override async ValueTask DisposeAsyncCore()
        {
            try
            {
                if (_cardJsModule is not null || _cardInProcessModule is not null)
                {
                    await InvokeVoidAsync(_cardJsModule, _cardInProcessModule, "destroy", _element).ConfigureAwait(true);
                }

                if (_cardJsModule is not null)
                {
                    await _cardJsModule.DisposeAsync().ConfigureAwait(true);
                }

                _cardInProcessModule?.Dispose();
            }
            catch (JSDisconnectedException)
            {
                // Ignore: the circuit disconnected (e.g., page reload) before JS disposal could complete.
            }

            await base.DisposeAsyncCore().ConfigureAwait(true);
        }

        #endregion
    }
}
