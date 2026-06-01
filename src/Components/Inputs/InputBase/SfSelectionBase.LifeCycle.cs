using Syncfusion.Blazor.Toolkit.Internal;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    /// <summary>
    /// Partial class containing lifecycle method implementations for <see cref="SfSelectionBase{TChecked}"/>.
    /// </summary>
    public partial class SfSelectionBase<TChecked>
    {
        #region LifeCycle Methods

        /// <summary>
        /// Initializes the component state when the component is first created.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(true);
            PreviousCheckedValue = Checked;

            // Check if a native onchange event handler is attached
            if (_inputAttributes != null && _inputAttributes.ContainsKey("onchange"))
            {
                HasOnChangeEvent = true;
            }
        }

        /// <summary>
        /// Handles parameter updates, detects checked state changes, and recomputes visual state.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(true);

            // Detect if the checked value changed dynamically
            bool isDynamicChange = !SfBaseUtils.Equals(Checked, PreviousCheckedValue);
            PreviousCheckedValue = NotifyPropertyChanges(CHECKED, Checked, PreviousCheckedValue);

            // Update the check state if component has already been rendered
            if (IsRendered && Checked != null)
            {
                await UpdateCheckStateAsync(Checked).ConfigureAwait(true);
            }

            InitRender(isDynamicChange);
            UpdateHTMLAttributes();
        }

        /// <summary>
        /// Executes after the first render to restore persisted state and raise the Created event.
        /// </summary>
        /// <param name="firstRender">Indicates whether this is the first render of the component.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender).ConfigureAwait(true);
            if (firstRender)
            {
                // Restore persisted state if enabled.
                if (EnablePersistence)
                {
                    string? localStorageValue = await InvokeAsync<string>(_baseJsModule, _baseJsInProcessModule, "getLocalStorageItem", [_idValue]).ConfigureAwait(true);
                    localStorageValue = string.IsNullOrEmpty(localStorageValue) ? null : localStorageValue;

                    if (!(localStorageValue == null && Checked != null))
                    {
                        if (localStorageValue == "null")
                        {
                            TChecked? persistValue = (TChecked?)(object?)null;
                            Checked = PreviousCheckedValue = await SfBaseUtils.UpdatePropertyAsync(persistValue, PreviousCheckedValue, CheckedChanged!, CascadedEditContext, CheckedExpression!).ConfigureAwait(true);
                        }
                        else if (string.IsNullOrEmpty(Name))
                        {
                            TChecked persistValue = (TChecked)SfBaseUtils.ChangeType(localStorageValue!, typeof(TChecked))!;
                            Checked = PreviousCheckedValue = await SfBaseUtils.UpdatePropertyAsync(persistValue, PreviousCheckedValue, CheckedChanged!, CascadedEditContext, CheckedExpression!).ConfigureAwait(true);
                        }
                    }
                }

                // Invoke the Created event callback.
                if (Created.HasDelegate)
                {
                    await Created.InvokeAsync(null).ConfigureAwait(true);
                }
            }
        }

        #endregion
    }
}
