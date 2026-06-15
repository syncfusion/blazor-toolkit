using Microsoft.JSInterop;
using Syncfusion.Blazor.Toolkit.Internal;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    /// <summary>
    /// Represents a TextBox component that provides an input element for accepting text input from users.
    /// </summary>
    /// <remarks>
    /// <para>The <see cref="SfTextBox"/> component allows users to edit or display text values with support for various input types, validation, floating labels, and customization options.</para>
    /// <para>The component provides a rich set of features including real-time input events, focus management, and accessibility support.</para>
    /// </remarks>
    public partial class SfTextBox : SfInputBase<string>
    {
        #region Lifecycle methods

        /// <summary>
        /// Asynchronously initializes the <see cref="SfTextBox"/> component during its initial render lifecycle.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous initialization operation.
        /// </returns>
        /// <remarks>
        /// <para>This method sets up essential component properties, configures input attributes, applies CSS classes, and establishes the component's initial state including script module registration and event binding.</para>
        /// <para>The method performs the following initialization tasks:</para>
        /// <list type="bullet">
        /// <item><description>Registers the SfTextBox script module for client-side functionality.</description></item>
        /// <item><description>Configures autocomplete and input type attributes.</description></item>
        /// <item><description>Sets up input event handlers for clear button and float label functionality.</description></item>
        /// <item><description>Initializes component properties and generates unique IDs.</description></item>
        /// <item><description>Establishes parent-child relationships for nested components.</description></item>
        /// <item><description>Applies initial CSS classes and styling.</description></item>
        /// </list>
        /// </remarks>
        /// <exclude/>
        [RequiresUnreferencedCode("Reflection on TextBoxParent type and ComponentRef property which may be trimmed")]
        [DynamicDependency(DynamicallyAccessedMemberTypes.NonPublicProperties, typeof(SfTextBox))]
        protected override async Task OnInitializedAsync()
        {
            try
            {
                string? autoCompleteEnumValue = SfBaseUtils.GetEnumValue(Autocomplete);
                if (autoCompleteEnumValue is not null)
                {
                    BaseAutocomplete = autoCompleteEnumValue;
                }
                await base.OnInitializedAsync().ConfigureAwait(true);
                InvokeInputEvent();
                _cssClass = CssClass;
                InputTextValue = Value;
                InternalValue = Value;
                _autocomplete = Autocomplete;
                _type = Type;
                InitializeProps();
                if (autoCompleteEnumValue is not null)
                {
                    InputHtmlAttributes = SfBaseUtils.UpdateDictionary(AUTOCOMPLETE, autoCompleteEnumValue, InputHtmlAttributes);
                }
                if (!Multiline)
                {
                    string? typeEnumValue = SfBaseUtils.GetEnumValue(Type);
                    if (typeEnumValue is not null)
                    {
                        InputHtmlAttributes = SfBaseUtils.UpdateDictionary(TYPE, typeEnumValue, InputHtmlAttributes);
                    }
                }
                SetCssClass();
                if (TextBoxParent is not null && Convert.ToString(TextBoxParent.Type, CultureInfo.CurrentCulture) == "Text")
                {
                    PropertyInfo? componentRefProperty = TextBoxParent?.GetType().GetProperty("ComponentRef", BindingFlags.NonPublic | BindingFlags.Instance);
                    componentRefProperty?.SetValue(TextBoxParent, this);

                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unhandled exception occurred. ", ex);
            }
        }

        /// <summary>
        /// Asynchronously processes parameter changes when component properties are updated dynamically.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous parameter processing operation.
        /// </returns>
        /// <remarks>
        /// <para>This method handles property validation, updates input attributes, and triggers necessary re-rendering operations to reflect the changes in the component's state and appearance.</para>
        /// <para>The method performs the following parameter update tasks:</para>
        /// <list type="bullet">
        /// <item><description>Validates and updates input type attributes for non-multiline text boxes.</description></item>
        /// <item><description>Ensures proper ARIA labeling for accessibility compliance.</description></item>
        /// <item><description>Processes property changes through the PropertyUpdate mechanism.</description></item>
        /// <item><description>Applies property-specific updates via OnPropertyChange method.</description></item>
        /// <item><description>Updates validation CSS classes based on current validation state.</description></item>
        /// <item><description>Handles form validation integration and visual feedback.</description></item>
        /// </list>
        /// </remarks>
        /// <exclude/>
        protected override async Task OnParametersSetAsync()
        {
            try
            {
                if (!Multiline && !InputHtmlAttributes.ContainsKey(TYPE))
                {
                    string? typeEnumValue = SfBaseUtils.GetEnumValue(Type);
                    if (typeEnumValue is not null)
                    {
                        InputHtmlAttributes = SfBaseUtils.UpdateDictionary(TYPE, typeEnumValue, InputHtmlAttributes);
                    }
                }
                await base.OnParametersSetAsync().ConfigureAwait(true);
                if (!InputHtmlAttributes.ContainsKey(ARIA_LABEL))
                {
                    InputHtmlAttributes = SfBaseUtils.UpdateDictionary(ARIA_LABEL, "textbox", InputHtmlAttributes);
                }
                await PropertyUpdateAsync().ConfigureAwait(true);
                if (PropertyChanges?.Count > 0)
                {
                    await OnPropertyChangeAsync(PropertyChanges).ConfigureAwait(true);
                }
                UpdateValidateClass();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Asynchronously executes post-render operations after the <see cref="SfTextBox"/> component has been rendered to the DOM.
        /// </summary>
        /// <param name="firstRender">
        /// A <see cref="bool"/> value indicating whether this is the component's initial render cycle. When <see langword="true"/>, performs one-time initialization operations; when <see langword="false"/>, handles subsequent re-renders.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous post-render operation.
        /// </returns>
        /// <remarks>
        /// <para>This method handles first-time initialization tasks, persistence restoration, event invocations, and client-side script interactions for advanced component functionality.</para>
        /// <para>During the first render cycle, this method performs the following operations:</para>
        /// <list type="bullet">
        /// <item><description>Restores persisted values from browser local storage if <see cref="SfInputBase{TValue}.EnablePersistence"/> is enabled.</description></item>
        /// <item><description>Initializes the PreviousValue tracking for change detection.</description></item>
        /// <item><description>Invokes the <see cref="Created"/> event callback to notify component initialization completion.</description></item>
        /// </list>
        /// <para>For components with outline styling, it also:</para>
        /// <list type="bullet">
        /// <item><description>Calculates and adjusts component width using client-side JavaScript interop.</description></item>
        /// <item><description>Ensures proper visual alignment and spacing for outlined text boxes.</description></item>
        /// </list>
        /// </remarks>
        /// <exclude/>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                await base.OnAfterRenderAsync(firstRender).ConfigureAwait(true);
                if (firstRender)
                {
                    if (EnablePersistence)
                    {
                        string? localStorageValue = await InvokeAsync<string>(_baseJsModule!, _baseJsInProcessModule!, "getLocalStorageItem", ID).ConfigureAwait(true);
                        localStorageValue = (string.IsNullOrEmpty(localStorageValue) || localStorageValue == "null") ? null : localStorageValue;
                        if (!(localStorageValue == null && Value is not null))
                        {
                            await SetValueAsync(localStorageValue, FloatLabelType, ShowClearButton).ConfigureAwait(true);
                        }
                    }
                    _previousValue = Value;
                    if (Created.HasDelegate)
                    {
                        await Created.InvokeAsync(null).ConfigureAwait(true);
                    }
                }
                if (ContainerClass.Contains(OUTLINE, StringComparison.Ordinal) || FloatLabelType != FloatLabelType.Never)
                {
                    await InvokeVoidAsync(_textBoxJsModule!, _textBoxJsInProcessModule!, "calculateWidth", InputElement, DotnetObjectReference!, ContainerElement).ConfigureAwait(true);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Asynchronously releases <see cref="SfTextBox"/>-specific resources when the component is disposed.
        /// </summary>
        /// <returns>
        /// A <see cref="ValueTask"/> representing the asynchronous disposal operation that completes when the <see cref="Destroyed"/> callback has been invoked and the TextBox JavaScript module has been released.
        /// </returns>
        /// <remarks>
        /// <para>This override performs the following cleanup steps before delegating to the base class implementation:</para>
        /// <list type="bullet">
        /// <item><description>Invokes the <see cref="Destroyed"/> event callback (when registered and the component has rendered at least once) so consumers can release external resources tied to this instance.</description></item>
        /// <item><description>Disposes the TextBox JavaScript interop modules (<see cref="IJSObjectReference"/> and <see cref="IJSInProcessObjectReference"/>) used for client-side functionality.</description></item>
        /// <item><description>Swallows <see cref="JSDisconnectedException"/> when the circuit has already disconnected (for example, after a page reload), preventing noisy errors during teardown.</description></item>
        /// </list>
        /// </remarks>
        /// <exclude/>
        protected override async ValueTask DisposeAsyncCore()
        {
            if (IsRendered)
            {
                if (Destroyed.HasDelegate)
                {
                    await Destroyed.InvokeAsync(null).ConfigureAwait(true);
                }
            }
            try
            {
                if (_textBoxJsModule is not null)
                {
                    await _textBoxJsModule.DisposeAsync().ConfigureAwait(true);
                }
                _textBoxJsInProcessModule?.Dispose();
            }
            catch (JSDisconnectedException)
            {
                // Ignore: The circuit disconnected (e.g., page reload) before JS disposal could complete.
            }
            await base.DisposeAsyncCore().ConfigureAwait(true);
        }
        #endregion
    }
}
