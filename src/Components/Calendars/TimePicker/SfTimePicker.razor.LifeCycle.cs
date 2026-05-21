using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Toolkit.Inputs;
using Syncfusion.Blazor.Toolkit.Internal;
using System.Globalization;
using System.Reflection;

namespace Syncfusion.Blazor.Toolkit.Calendars
{
    public partial class SfTimePicker<TValue> : SfInputBase<TValue>
    {
        /// <summary>
        /// Triggers during the initial rendering of the component and performs essential initialization.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous initialization operation.</returns>
        /// <remarks>
        /// This method initializes the component's core properties, sets up CSS classes, configures culture settings,
        /// generates component ID if not provided, and establishes parent-child relationships.
        /// It's called once when the component is first created.
        /// </remarks>
        /// <example>
        /// This method is automatically called by the Blazor framework during component lifecycle.
        /// No manual invocation is required.
        /// </example>
        /// <exclude/>
        protected override async Task OnInitializedAsync()
        {
            RootClass = ROOT;
            ContainerClass = CONTAINER_CLASS;
            PopupContainer = POPUP_CONTAINER;

            // Unique class added for dynamically rendered Inplace-editor components
            if (TimePickerParent is not null)
            {
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, "e-editable-elements");
                PopupContainer = SfBaseUtils.AddClass(PopupContainer, "e-editable-elements");
            }

            TimeIcon = TIME_ICON;
            await base.OnInitializedAsync().ConfigureAwait(false);
            PropertyInitialized();
            CurrentCulture = GetDefaultCulture();
            PreviousDateTime = Value;
            IsValideValue = true;
            _ = SfBaseUtils.UpdateDictionary(ARIA_EXPANDED, FALSE, InputHtmlAttributes);
            if (string.IsNullOrEmpty(ID))
            {
                ID = SfBaseUtils.GenerateID(TIME_PICKER);
            }
            DataId = ID;
            if (OnPaste.HasDelegate)
            {
                EventCallback<ClipboardEventArgs> createPasteEvent = EventCallback.Factory.Create<ClipboardEventArgs>(this, OnPasteHandlerAsync);
                InputHtmlAttributes = SfBaseUtils.UpdateDictionary("onpaste", createPasteEvent, InputHtmlAttributes);
            }
            if (TimePickerParent is not null && Convert.ToString(TimePickerParent.Type, CultureInfo.CurrentCulture) == "Time")
            {
                PropertyInfo? componentRefProperty = TimePickerParent?.GetType().GetProperty("ComponentRef", BindingFlags.NonPublic | BindingFlags.Instance);
                componentRefProperty?.SetValue(TimePickerParent, this);
            }
        }

        /// <summary>
        /// Triggers when the component parameters are set or changed dynamically during the component lifecycle.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous parameter processing operation.</returns>
        /// <remarks>
        /// This method is called whenever component parameters change. It updates internal state, processes property changes,
        /// updates CSS classes, handles validation, and ensures the component reflects the new parameter values.
        /// It's part of the Blazor component lifecycle and is called after OnInitializedAsync.
        /// </remarks>
        /// <example>
        /// This method is automatically invoked by Blazor when parameters change:
        /// <code><![CDATA[
        /// // When parent component changes TimePicker parameters
        /// <SfTimePicker @bind-Value="timeValue" Format="@currentFormat" Disabled="@isDisabled" />
        /// ]]></code>
        /// </example>
        /// <exclude/>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(false);
            await PropertyParametersSetAsync().ConfigureAwait(false);
            SetRTL();
            SetTimeAllowEdit();
            UpdateAriaAttributes();
            await SetHtmlAttributesAsync().ConfigureAwait(false);
            if (PropertyChanges is not null && PropertyChanges.Count > 0)
            {
                await OnPropertyChangeAsync(PropertyChanges).ConfigureAwait(false);
            }
            UpdateErrorClass();
            SetCssClass();
            UpdateValidateClass();
        }

        /// <summary>
        /// Executes after JavaScript interop scripts have been rendered and initializes client-side functionality.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous script initialization operation.</returns>
        /// <remarks>
        /// This method initializes the TimePicker's JavaScript functionality, sets up mask placeholders,
        /// configures client properties, and handles device-specific popup configurations.
        /// It's called after the component's scripts are loaded in the browser.
        /// </remarks>
        internal override async Task OnAfterScriptRenderedAsync()
        {
            TimePickerClientProps<TValue> options = GetClientProperties();
            MaskPlaceholderContent();
            await base.OnAfterScriptRenderedAsync().ConfigureAwait(false);
            await UpdateIsDeviceModeAsync().ConfigureAwait(false);
            ClientMaskValue = await InvokeAsync<ClientMaskValues>(_timePickerJsModule!, _timePickerJsInProcessModule!, "initialize", [DataId, ContainerElement, InputElement, DotnetObjectReference!, options]).ConfigureAwait(false);
            if (EnableMask && ClientMaskValue is not null && !((FloatLabelType == FloatLabelType.Auto || FloatLabelType == FloatLabelType.Never) && !string.IsNullOrEmpty(Placeholder) && Value is null))
            {
                CurrentValueAsString = ClientMaskValue.InputElementValue;
                await InvokeVoidAsync(_timePickerJsModule!, _timePickerJsInProcessModule!, "updateCurrentValue", [DataId, CurrentValueAsString]).ConfigureAwait(true);
            }
            if (EnableMask && ClientMaskValue is not null)
            {
                CurrentMaskFormat = ClientMaskValue.CurrentMaskFormat;
            }
            IsDevice = SyncfusionService is not null && SyncfusionService.IsDeviceMode;
            if (IsDevice && FullScreen)
            {
                ContainerClass = (!string.IsNullOrEmpty(ContainerClass)) ? SfBaseUtils.AddClass(ContainerClass, POPUPEXPAND) : ContainerClass;
                PopupContainer = (!string.IsNullOrEmpty(PopupContainer)) ? SfBaseUtils.AddClass(PopupContainer, POPUPEXPAND) : PopupContainer;
            }
        }

        /// <summary>
        /// Triggers after the component has been rendered in the DOM.
        /// </summary>
        /// <param name="firstRender">true if this is the first time the component is being rendered; otherwise, false.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous post-render operations.</returns>
        /// <remarks>
        /// This method handles post-render operations such as popup rendering, persistence data loading,
        /// and firing the Created event. On first render, it also handles local storage persistence
        /// and initializes component state. This is part of the Blazor component lifecycle.
        /// </remarks>
        /// <example>
        /// This method is automatically called by Blazor after each render cycle:
        /// <code><![CDATA[
        /// // First render: firstRender = true
        /// // Subsequent renders: firstRender = false
        /// ]]></code>
        /// </example>
        /// <exclude/>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender).ConfigureAwait(false);
            await ClientPopupRenderAsync().ConfigureAwait(false);
            if (firstRender)
            {
                if (EnablePersistence)
                {
                    string? localStorageValue = await InvokeAsync<string>(_baseJsModule!, _baseJsInProcessModule!, "getLocalStorageItem", [ID]).ConfigureAwait(true);
                    localStorageValue = (string.IsNullOrEmpty(localStorageValue) || localStorageValue == "null") ? null : localStorageValue;
                    if (!(localStorageValue is null && Value is not null))
                    {
                        TValue? persistValue = (TValue)SfBaseUtils.ChangeType(localStorageValue!, typeof(TValue));
                        InputTextValue = persistValue;
                    }

                    await UpdateInputAsync().ConfigureAwait(false);
                }

                PreviousElementValue = CurrentValueAsString;
                if (Created.HasDelegate)
                {
                    await InvokeAsync(() => Created.InvokeAsync(null)).ConfigureAwait(false);
                }

                IsValideValue = true;
            }
        }

        internal override async Task ImportComponentModuleAsync()
        {
            await base.ImportComponentModuleAsync().ConfigureAwait(true);
            // Load animation-related script before timepicker scripts
            await LoadAnimationScriptAsync().ConfigureAwait(true);
            await LoadPopupScriptAsync().ConfigureAwait(true);

            JsModuleReference timePickerJsModuleReference = await ImportModuleAsync("./_content/Syncfusion.Blazor.Toolkit/scripts/timepicker.js", _timePickerJsModule, _timePickerJsInProcessModule).ConfigureAwait(true);
            _timePickerJsModule = timePickerJsModuleReference.AsyncRef;
            _timePickerJsInProcessModule = timePickerJsModuleReference.InProcessRef;

            JsModuleReference textBoxJsModuleReference = await ImportModuleAsync("./_content/Syncfusion.Blazor.Toolkit/scripts/textbox.js", _textBoxJsModule, _textBoxJsInProcessModule).ConfigureAwait(true);
            _textBoxJsModule = textBoxJsModuleReference.AsyncRef;
            _textBoxJsInProcessModule = textBoxJsModuleReference.InProcessRef;

            if (EnableMask)
            {
                JsModuleReference maskJsModuleReference = await ImportModuleAsync("./_content/Syncfusion.Blazor.Toolkit/scripts/masked-datetime.js", _maskJsModule, _maskJsInProcessModule).ConfigureAwait(true);
                _maskJsModule = maskJsModuleReference.AsyncRef;
                _maskJsInProcessModule = maskJsModuleReference.InProcessRef;
            }
        }

        /// <summary>
        /// Disposes of component resources and cleans up JavaScript interop references.
        /// </summary>
        /// <remarks>
        /// This override handles cleanup of JavaScript resources, triggers the Destroyed event,
        /// nullifies references to prevent memory leaks, and disposes of window instance resources.
        /// It's automatically called when the component is being destroyed.
        /// </remarks>
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
                    TimePickerClientProps<TValue> options = new()
                    {
                        EnableRtl = SyncfusionService!._options.EnableRtl,
                        ZIndex = ZIndex,
                        KeyConfigs = KeyConfigs,
                        Value = Value!,
                        Width = Width,
                        Step = Step,
                        ScrollTo = ScrollTo
                    };
                    object[] destroyArgs = [DataId, PopupElement, PopupHolderEle, new PopupEventArgs() { Cancel = false }, options];
                    await InvokeVoidAsync(_timePickerJsModule!, _timePickerJsInProcessModule!, "destroy", destroyArgs, TaskScheduler.Current).ConfigureAwait(false);
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
                if (_timePickerJsModule is not null)
                {
                    await _timePickerJsModule.DisposeAsync().ConfigureAwait(false);
                    _timePickerJsModule = null;
                }
                if (_maskJsModule is not null)
                {
                    await _maskJsModule.DisposeAsync().ConfigureAwait(false);
                    _maskJsModule = null;
                }
                _timePickerJsInProcessModule?.Dispose();
                _timePickerJsInProcessModule = null;
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
            TimeIcon = string.Empty;
            PopupEventArgs = default!;
            ClientMaskValue = null;
            MaskPlaceholder = null;
            MaskPlaceholderDictionary = default!;
            ListData = null;
        }
    }
}
