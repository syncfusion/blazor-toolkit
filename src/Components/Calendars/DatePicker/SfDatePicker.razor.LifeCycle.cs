using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Toolkit.Calendars.Interfaces;
using Syncfusion.Blazor.Toolkit.Internal;
using System.Globalization;
using System.Reflection;

namespace Syncfusion.Blazor.Toolkit.Calendars
{
    /// <summary>
    /// The DatePicker is a graphical user interface component that allows the user to select or enter a date value.
    /// </summary>
    public partial class SfDatePicker<TValue> : CalendarBase<TValue>, IMaskPlaceholder
    {
        /// <summary>
        /// Called when the component is initially rendered. Performs initialization and prepares popup/calendar logic.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method sets up device state, initializes property values, handles masked input requirements, and sets up persistence and events based on component parameters.
        /// <para>
        /// Override or use lifecycle events for additional custom logic.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// // Not called directly; overridden when customizing control behavior
        /// protected override async Task OnInitializedAsync()
        /// {
        ///     await base.OnInitializedAsync();
        /// }
        /// ]]></code>
        /// </example>
        /// <exclude/>
        protected override async Task OnInitializedAsync()
        {
            await InitializeBasePropertiesAsync().ConfigureAwait(false);
            InitializeComponentId();
            await InitializeValueAsync().ConfigureAwait(false);
            InitializeEventHandlers();
            InitializeParentReference();
        }

        private async Task InitializeBasePropertiesAsync()
        {
            PropertyInit();
            await base.OnInitializedAsync().ConfigureAwait(false);
            PropertyInitialized();
            IsValideValue = true;
            _ = SfBaseUtils.UpdateDictionary(ARIAEXPANDED, FALSE, InputHtmlAttributes);
        }

        private void InitializeComponentId()
        {
            if (string.IsNullOrEmpty(ID))
            {
                ID = "datepicker-" + Guid.NewGuid().ToString();
            }
            DataId = ID;
        }

        private async Task InitializeValueAsync()
        {
            if (Value is not null)
            {
                if (CalendarMode == CalendarType.Islamic)
                {
                    IslamicValueAsString = ConvertToHijri(Value, GetDefaultFormat());
                }
                await StrictModeUpdateAsync(true).ConfigureAwait(false);
                await UpdateInputAsync().ConfigureAwait(false);
            }
        }

        private void InitializeEventHandlers()
        {
            if (OnPaste.HasDelegate)
            {
                EventCallback<ClipboardEventArgs> createPasteEvent = EventCallback.Factory.Create<ClipboardEventArgs>(this, OnPasteHandlerAsync);
                InputHtmlAttributes = SfBaseUtils.UpdateDictionary("onpaste", createPasteEvent, InputHtmlAttributes);
            }
        }

        private void InitializeParentReference()
        {
            if (DatePickerParent is not null && Convert.ToString(DatePickerParent.Type, CultureInfo.CurrentCulture) == "Date")
            {
                PropertyInfo? componentRefProperty = DatePickerParent?.GetType().GetProperty("ComponentRef", BindingFlags.NonPublic | BindingFlags.Instance);
                componentRefProperty?.SetValue(DatePickerParent, this);
            }
        }

        /// <summary>
        /// Executed when any component parameter is changed dynamically. Applies updated rendering, mask/format and validation as relevant.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// Used internally by the framework when updating parameter values at runtime or via user code. Handles updating state for RTL, mask, localization, or events.
        /// <para>
        /// You should generally not invoke this manually, but may override in custom components.
        /// </para>
        /// </remarks>
        /// <exclude/>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(false);
            await PropertyParametersSetAsync().ConfigureAwait(false);
            SetRTL();
            CalendarClass = WeekNumber ? SfBaseUtils.AddClass(CalendarClass, WEEKNUMBER) : SfBaseUtils.RemoveClass(CalendarClass, WEEKNUMBER);
            SetDayHeaderFormat();
            SetAllowEdit();
            UpdateAriaAttributes();
            if (PropertyChanges is not null && PropertyChanges.Count > 0)
            {
                await OnPropertyChangeAsync(PropertyChanges).ConfigureAwait(false);
            }
            SetCssClass();
            UpdateValidateClass();
            if (PropertyChanges is not null)
            {
                _ = PropertyChanges.Remove(nameof(Value));
            }
        }

        private async Task OnPropertyChangeAsync(Dictionary<string, object> newProps)
        {
            List<KeyValuePair<string, object>> newProperties = [.. newProps];
            foreach (KeyValuePair<string, object> prop in newProperties)
            {
                switch (prop.Key)
                {
                    case nameof(Format):
                        await HandleFormatChangeAsync().ConfigureAwait(false);
                        break;
                    case nameof(Value):
                        await HandleValueChangeAsync().ConfigureAwait(false);
                        break;
                    case nameof(CssClass):
                        HandleCssClassChange();
                        break;
                    case nameof(FloatLabelType):
                        await HandleFloatLabelTypeChangeAsync().ConfigureAwait(false);
                        break;
                    case nameof(StrictMode):
                        await HandleStrictModeChangeAsync().ConfigureAwait(false);
                        break;
                    case nameof(Min):
                    case nameof(Max):
                        await HandleMinMaxChangeAsync().ConfigureAwait(false);
                        break;
                    case nameof(READONLYATTR):
                        await HandleReadOnlyChangeAsync().ConfigureAwait(false);
                        break;
                    default:
                        break;
                }
            }
        }

        private async Task HandleFormatChangeAsync()
        {
            if (EnableMask)
            {
                await CreateMaskAsync().ConfigureAwait(false);
            }
            if ((!string.IsNullOrEmpty(Value!.ToString()) || ((FloatLabelType == FloatLabelType.Always || string.IsNullOrEmpty(Placeholder)) && ClientMaskValue is not null)) && EnableMask)
            {
                CurrentValueAsString = ClientMaskValue?.InputElementValue;
            }
            if (Value is not null)
            {
                CurrentValueAsString = Intl.GetDateFormat(Value, GetDefaultFormat());
            }
        }

        private async Task HandleValueChangeAsync()
        {
            if (EnableMask && IsRendered)
            {
                await CreateMaskAsync().ConfigureAwait(false);
            }
            if (StrictMode)
            {
                await StrictModeUpdateAsync().ConfigureAwait(false);
                await UpdateInputAsync().ConfigureAwait(false);
            }
            else
            {
                UpdateErrorClass();
            }
            PreviousElementValue = CurrentValueAsString;
            PreviousDate = Value;
        }

        private void HandleCssClassChange()
        {
            ContainerClass = string.IsNullOrEmpty(ContainerClass) ? ContainerClass : SfBaseUtils.RemoveClass(ContainerClass, InternalCssClass);
            PopupContainer = string.IsNullOrEmpty(PopupContainer) ? PopupContainer : SfBaseUtils.RemoveClass(PopupContainer, InternalCssClass);
            InternalCssClass = CssClass;
        }

        private async Task HandleFloatLabelTypeChangeAsync()
        {
            if (!string.IsNullOrEmpty(Value!.ToString()) || ((FloatLabelType == FloatLabelType.Always || string.IsNullOrEmpty(Placeholder)) && ClientMaskValue is not null && EnableMask))
            {
                CurrentValueAsString = ClientMaskValue?.InputElementValue;
            }
            await OnAfterScriptRenderedAsync().ConfigureAwait(false);
        }

        private async Task HandleStrictModeChangeAsync()
        {
            if (Value is not null)
            {
                await StrictModeUpdateAsync().ConfigureAwait(false);
                await UpdateInputAsync().ConfigureAwait(false);
            }
        }

        private async Task HandleMinMaxChangeAsync()
        {
            if (Value is not null && StrictMode)
            {
                await StrictModeUpdateAsync().ConfigureAwait(false);
                await UpdateInputAsync().ConfigureAwait(false);
                await ChangeTriggerAsync().ConfigureAwait(false);
            }
            else
            {
                UpdateErrorClass();
            }
        }

        private async Task HandleReadOnlyChangeAsync()
        {
            if (EnableMask)
            {
                await CreateMaskAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Called after every render of the component. Used to update DOM interop and invoke lifecycle events post-render.
        /// </summary>
        /// <param name="firstRender">Indicates whether this render is the first render; if <c>true</c>, executes initial data setup and persistence restore.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous render operation.</returns>
        /// <remarks>
        /// Do not call directly; this is invoked by the Blazor framework on each render or property update.
        /// Initializes popup, restores local state/persistence, and triggers Created lifecycle events.
        /// </remarks>
        /// <exclude/>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender).ConfigureAwait(false);
            await ClientPopupRenderAsync().ConfigureAwait(false);
            if (firstRender)
            {
                await HandleFirstRenderAsync().ConfigureAwait(false);
            }
        }

        private async Task HandleFirstRenderAsync()
        {
            if (EnablePersistence)
            {
                await RestorePersistedValueAsync().ConfigureAwait(false);
            }
            InitializePreviousValues();
            if (Created.HasDelegate)
            {
                await InvokeAsync(() => Created.InvokeAsync(null)).ConfigureAwait(false);
            }
            IsValideValue = false;
        }

        private async Task RestorePersistedValueAsync()
        {
            string? localStorageValue = await InvokeAsync<string>(_baseJsModule!, _baseJsInProcessModule!, "getLocalStorageItem", [ID]).ConfigureAwait(true);
            localStorageValue = (string.IsNullOrEmpty(localStorageValue) || localStorageValue == "null") ? null : localStorageValue;
            if (!(localStorageValue is null && Value is not null))
            {
                TValue? persistValue = (TValue)SfBaseUtils.ChangeType(localStorageValue!, typeof(TValue));
                InputTextValue = persistValue;
            }
            await UpdateInputAsync().ConfigureAwait(false);
            await InvokeAsync(StateHasChanged).ConfigureAwait(false);
        }

        private void InitializePreviousValues()
        {
            PreviousDate = Value;
            PreviousElementValue = CurrentValueAsString;
            if (CalendarMode == CalendarType.Islamic)
            {
                PreviousElementValue = IslamicValueAsString;
            }
        }

        internal override async Task OnAfterScriptRenderedAsync()
        {
            DatePickerClientProps<TValue> options = GetClientProperties();
            MaskPlaceholderContent();
            await UpdateIsDeviceModeAsync().ConfigureAwait(false);
            ClientMaskValue = await InvokeAsync<ClientMaskValues>(_datePickerJsModule!, _datePickerJsInProcessModule!, "initialize", [DataId, ContainerElement, InputElement, DotnetObjectReference!, options]).ConfigureAwait(false);
            if (EnableMask && ClientMaskValue is not null && !((FloatLabelType == FloatLabelType.Auto || FloatLabelType == FloatLabelType.Never) && !string.IsNullOrEmpty(Placeholder) && Value is null))
            {
                CurrentValueAsString = ClientMaskValue.InputElementValue;
                await InvokeVoidAsync(_datePickerJsModule!, _datePickerJsInProcessModule!, "updateCurrentValue", [DataId, CurrentValueAsString]).ConfigureAwait(true);
            }
            if (EnableMask && ClientMaskValue is not null)
            {
                CurrentMaskFormat = ClientMaskValue.CurrentMaskFormat;
            }
            IsDevice = SyncfusionService is not null && SyncfusionService.IsDeviceMode;
            if (IsDevice)
            {
                await LoadTouchScriptAsync().ConfigureAwait(true);
            }
            if (IsDevice && FullScreen)
            {
                ContainerClass = (!string.IsNullOrEmpty(ContainerClass)) ? SfBaseUtils.AddClass(ContainerClass, POPUPEXPAND) : ContainerClass;
                PopupContainer = (!string.IsNullOrEmpty(PopupContainer)) ? SfBaseUtils.AddClass(PopupContainer, POPUPEXPAND) : PopupContainer;
            }
        }

        internal override async Task ImportComponentModuleAsync()
        {
            await base.ImportComponentModuleAsync().ConfigureAwait(true);
            // Load animation-related script before datepicker scripts
            await LoadAnimationScriptAsync().ConfigureAwait(true);
            // Load popup-related script before datepicker scripts
            await LoadPopupScriptAsync().ConfigureAwait(true);

            JsModuleReference datePickerJsModuleReference = await ImportModuleAsync("./_content/Syncfusion.Blazor.Toolkit/scripts/datepicker.js", _datePickerJsModule, _datePickerJsInProcessModule).ConfigureAwait(true);
            _datePickerJsModule = datePickerJsModuleReference.AsyncRef;
            _datePickerJsInProcessModule = datePickerJsModuleReference.InProcessRef;

            if (EnableMask)
            {
                JsModuleReference maskJsModuleReference = await ImportModuleAsync("./_content/Syncfusion.Blazor.Toolkit/scripts/masked-datetime.js", _maskJsProcessModule, _maskJsInProcessModule).ConfigureAwait(true);
                _maskJsProcessModule = maskJsModuleReference.AsyncRef;
                _maskJsInProcessModule = maskJsModuleReference.InProcessRef;
            }
        }

        /// <summary>
        /// Triggers while disposing the component.
        /// </summary>
        /// <exclude/>
        protected override async ValueTask DisposeAsyncCore()
        {
            try
            {
                await ComponentDisposeAsync().ConfigureAwait(false);
            }
            catch (JSDisconnectedException)
            {
                // Ignore: The circuit disconnected (e.g., page reload) before JS disposal could complete.
            }
            catch (ObjectDisposedException)
            {
                // Ignore: Already disposed
            }
            await base.DisposeAsyncCore().ConfigureAwait(true);
        }

        private async Task ComponentDisposeAsync()
        {
            if (!IsRendered)
            {
                return;
            }
            IsDisposed = true;
            if (IsRendered)
            {
                try
                {
                    object[] destroyArgs = [DataId, PopupElement, PopupHolderEle, new PopupEventArgs() { Cancel = false }, GetClientProperties()];
                    await InvokeVoidAsync(_datePickerJsModule!, _datePickerJsInProcessModule!, "destroy", destroyArgs).ConfigureAwait(false);
                }
                catch (JSDisconnectedException)
                {
                    // Ignore: The circuit disconnected before JS disposal could complete.
                }
                catch (ObjectDisposedException)
                {
                    // Ignore: Module already disposed
                }
                try
                {
                    if (Destroyed.HasDelegate)
                    {
                        await InvokeAsync(() => Destroyed.InvokeAsync(null)).ConfigureAwait(false);
                    }
                }
                catch (JSDisconnectedException)
                {
                    // Ignore: The circuit disconnected before event could fire.
                }
            }
            try
            {
                if (_datePickerJsModule is not null)
                {
                    await _datePickerJsModule.DisposeAsync().ConfigureAwait(false);
                    _datePickerJsModule = null;
                }
                if (_maskJsProcessModule is not null)
                {
                    await _maskJsProcessModule.DisposeAsync().ConfigureAwait(false);
                    _maskJsProcessModule = null;
                }
                _datePickerJsInProcessModule?.Dispose();
                _datePickerJsInProcessModule = null;
                _maskJsInProcessModule?.Dispose();
                _maskJsInProcessModule = null;
            }
            catch (JSDisconnectedException)
            {
                // Ignore: The circuit disconnected (e.g., page reload) before JS disposal could complete.
            }
            catch (ObjectDisposedException)
            {
                // Ignore: Already disposed
            }
            DateIcon = string.Empty;
            PopupEventArgs = default!;
            ClientMaskValue = null;
            ChangedEventArgs = null;
            MaskPlaceholder = null;
            MaskPlaceholderDictionary = default!;
        }
    }
}
