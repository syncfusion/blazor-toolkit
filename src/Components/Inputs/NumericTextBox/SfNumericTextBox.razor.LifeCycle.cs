using Microsoft.JSInterop;
using Syncfusion.Blazor.Toolkit.Internal;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    public partial class SfNumericTextBox<TValue>
    {
        /// <summary>
        /// Triggers during initial rendering of the component.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task OnInitializedAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(ID))
                {
                    ID = "numeric-" + Guid.NewGuid().ToString();
                }
                DataId = ID;
                RootClass = ROOT_CLASS;
                ContainerClass = CONTAINER_CLASS;
                IsTriggerFocusHandler = true;
                await base.OnInitializedAsync().ConfigureAwait(true);
                IsNumberCulture = CultureInfo.CurrentCulture.Name.StartsWith(ARABIC, StringComparison.Ordinal) ||
                    CultureInfo.CurrentCulture.Name.StartsWith(THAILAND, StringComparison.Ordinal) ||
                    CultureInfo.CurrentCulture.Name.StartsWith(PERSIAN, StringComparison.Ordinal);
                PropertyInitialized();
                //ScriptModules = SfScriptModules.SfNumericTextBox;
                InvokeInputEvent();
                SetCssClass();
                UpdateDecimalType();
                ValidateMinMax();
                ValidateStep();
                await ChangeValueAsync((Value is null) ? default : StrictMode ? TrimValue(Value) : Value).ConfigureAwait(true);
                IsDoubleValue = Nullable.GetUnderlyingType(typeof(TValue)) == typeof(double) || typeof(TValue) == typeof(double);
            }
            catch (FormatException fe)
            {
                _logFormattingIssue(Logger, fe);
                throw;
            }
            catch (JSException jse)
            {
                _logJsError(Logger, jse);
                throw;
            }
        }

        /// <summary>
        /// Triggers when component properties are dynamically updated.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task OnParametersSetAsync()
        {
            try
            {
                await PropertyUpdateAsync().ConfigureAwait(true);
                if (PropertyChanges.Count > 0 && IsRendered)
                {
                    await OnPropertyChangedAsync(PropertyChanges).ConfigureAwait(true);
                }
                if (Min is null || Max is null)
                {
                    ValidateMinMax();
                }
                await base.OnParametersSetAsync().ConfigureAwait(true);
                UpdateValidateClass();
                InputHtmlAttributes = SfBaseUtils.UpdateDictionary(ROLE, SPIN_BUTTON, InputHtmlAttributes);
                InputHtmlAttributes = SfBaseUtils.UpdateDictionary(ARIA_LIVE, ASSERTIVE, InputHtmlAttributes);
                if (InputHtmlAttributes.TryGetValue("type", out object? value))
                {
                    if (value.Equals("number") && Format != "f")
                    {
                        Format = "f";
                        await PropertyUpdateAsync().ConfigureAwait(true);
                        if (PropertyChanges.Count > 0 && IsRendered)
                        {
                            await OnPropertyChangedAsync(PropertyChanges).ConfigureAwait(true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logUnexpectedErrorOnParametersSet(Logger, ex);
                throw;
            }
        }

        /// <summary>
        /// Triggers after the component has been rendered.
        /// </summary>
        /// <param name="firstRender">true if the component rendered for the first time.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                await base.OnAfterRenderAsync(firstRender).ConfigureAwait(true);
                if (IsSpinButtonChanged)
                {
                    IsSpinButtonChanged = false;
                    await InvokeVoidAsync(_numericTextBoxJsModule!, _numericTextBoxJsInProcessModule!, "spinButtonEvents", [DataId]).ConfigureAwait(true);

                }

                if (firstRender)
                {
                    if (EnablePersistence)
                    {
                        string? localStorageValue = await InvokeAsync<string>(_baseJsModule!, _baseJsInProcessModule!, "getLocalStorageItem", [ID]).ConfigureAwait(true);
                        localStorageValue = (string.IsNullOrEmpty(localStorageValue) || localStorageValue == "null") ? null : localStorageValue;
                        if (!(localStorageValue is null && Value is not null))
                        {
                            TValue? persistValue = (TValue)SfBaseUtils.ChangeType(localStorageValue, typeof(TValue), true);
                            InputTextValue = persistValue;
                        }

                        await ChangeValueAsync((Value is null) ? default : StrictMode ? TrimValue(Value) : Value).ConfigureAwait(true);
                    }

                    PrevValue = Value;
                    IsTriggerFocusHandler = false;
                    if (Created.HasDelegate)
                    {
                        await Created.InvokeAsync(null).ConfigureAwait(true);
                    }
                }
            }
            catch (Exception ex)
            {
                _logUnexpectedErrorOnAfterRender(Logger, ex);
                throw;
            }
        }

        internal override async Task ImportComponentModuleAsync()
        {
            await base.ImportComponentModuleAsync().ConfigureAwait(true);
            JsModuleReference numericTextBoxJsModuleReference = await ImportModuleAsync("./_content/Syncfusion.Blazor.Toolkit/scripts/numerictextbox.js", _numericTextBoxJsModule, _numericTextBoxJsInProcessModule).ConfigureAwait(true);
            _numericTextBoxJsModule = numericTextBoxJsModuleReference.AsyncRef;
            _numericTextBoxJsInProcessModule = numericTextBoxJsModuleReference.InProcessRef;
        }

        /// <summary>
        /// Performs cleanup operations when the component is being disposed.
        /// </summary>
        /// <remarks>
        /// This method handles the proper cleanup of the NumericTextBox component:
        /// <list type="bullet">
        /// <item><description>Calls client-side destroy methods to clean up JavaScript resources</description></item>
        /// <item><description>Triggers the Destroyed event if subscribers are present</description></item>
        /// <item><description>Disposes of window instance references to prevent memory leaks</description></item>
        /// <item><description>Only executes cleanup if the component has been rendered</description></item>
        /// </list>
        /// This ensures that all resources are properly released and no memory leaks occur when the component is removed from the DOM.
        /// </remarks>
        protected override async ValueTask DisposeAsyncCore()
        {
            if (IsRendered)
            {
                object[] destroyArgs = [DataId];
                await InvokeVoidAsync(_numericTextBoxJsModule, _numericTextBoxJsInProcessModule, "destroy", destroyArgs).ConfigureAwait(true);
                if (Destroyed.HasDelegate)
                {
                    await Destroyed.InvokeAsync(null).ConfigureAwait(true);
                }
                try
                {
                    if (_numericTextBoxJsModule is not null)
                    {
                        await _numericTextBoxJsModule.DisposeAsync().ConfigureAwait(true);
                    }
                    _numericTextBoxJsInProcessModule?.Dispose();
                    _selectRangeDotNetRef?.Dispose();
                    _selectRangeDotNetRef = null;
                    if (_delayCancellationTokenSource is not null)
                    {
                        await _delayCancellationTokenSource.CancelAsync().ConfigureAwait(true);
                        _delayCancellationTokenSource.Dispose();
                        _delayCancellationTokenSource = null;
                    }
                }
                catch (JSDisconnectedException)
                {
                    // Ignore: The circuit disconnected (e.g., page reload) before JS disposal could complete.
                }
            }
            await base.DisposeAsyncCore().ConfigureAwait(true);
        }
    }
}
