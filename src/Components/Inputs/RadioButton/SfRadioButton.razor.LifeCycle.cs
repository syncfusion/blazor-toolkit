namespace Syncfusion.Blazor.Toolkit.Inputs
{
    public partial class SfRadioButton<TChecked>
    {
        #region LifeCycle Methods

        /// <summary>
        /// Initializes the component and generates a unique ID if not explicitly set.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(true);
            if (string.IsNullOrEmpty(_idValue) || (_inputAttributes is not null && _inputAttributes.ContainsKey("id")))
            {
                _idValue = "radiobutton-" + Guid.NewGuid().ToString();
            }
        }

        /// <summary>
        /// Handles parameter changes and loads persisted radio button state if enabled.
        /// </summary>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(true);
            if (EnablePersistence && Checked is not null && _isFirstLoad)
            {
                if (_baseJsModule is not null && _baseJsInProcessModule is not null)
                {
                    await InvokeVoidAsync(_baseJsModule, _baseJsInProcessModule, "setLocalStorageItem", [_idValue, Checked!]).ConfigureAwait(true);
                }
            }
        }

        /// <summary>
        /// Performs post-render initialization and restores persisted state after initial render.
        /// </summary>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender).ConfigureAwait(true);
            if (firstRender && EnablePersistence)
            {
                string? localStorageValue = null;
                if (_baseJsModule is not null && _baseJsInProcessModule is not null)
                {
                    localStorageValue = await InvokeAsync<string>(_baseJsModule, _baseJsInProcessModule, "getLocalStorageItem", [_idValue]).ConfigureAwait(true);
                }
                localStorageValue = string.IsNullOrEmpty(localStorageValue) ? null : localStorageValue;
                _isChecked = (localStorageValue is null && Checked is not null)
                    ? Value!.Equals(Checked)
                    : Value is not null and not NullLocalStorageValue && Value.Equals(TryParseValueFromString(localStorageValue!));
                _isFirstLoad = true;
                StateHasChanged();
            }
        }

        #endregion
    }
}
