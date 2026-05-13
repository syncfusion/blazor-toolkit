using Microsoft.AspNetCore.Components;
using System.ComponentModel;

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
        #region Public Methods

        /// <summary>
        /// Adds icons to the <see cref="SfTextBox"/> component at the specified position.
        /// </summary>
        /// <param name="position">
        /// A <see cref="string"/> specifying the position where icons should be added. Valid values are <c>"prepend"</c> to add icons before the input field, or <c>"append"</c> to add icons after the input field.
        /// </param>
        /// <param name="icons">
        /// A <see cref="string"/> containing the CSS classes representing the icons to be added to the icon element. Multiple classes can be specified as space-separated values.
        /// </param>
        /// <param name="events">
        /// An optional <see cref="Dictionary{TKey, TValue}"/> of <see cref="string"/> keys and <see cref="object"/> values containing icon events to be added. The dictionary key represents the event type (e.g., "ontouchstart"), and the value represents the event handler method. The default value is <see langword="null"/>.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation that completes when the icons have been added.
        /// </returns>
        /// <remarks>
        /// <para>This method allows dynamic addition of icons to the TextBox component either before (prepend) or after (append) the input field.</para>
        /// <para>The <paramref name="icons"/> parameter accepts CSS class names that define the visual appearance of the icons. When providing the <paramref name="events"/> parameter, use the event type as the key and the event handler method as the value in the dictionary.</para>
        /// <para>If the <paramref name="position"/> parameter is <see langword="null"/> or empty, no icons will be added.</para>
        /// <para>Icons are typically rendered as span elements with the specified CSS classes, allowing customization through styling.</para>
        /// </remarks>
        /// <example>
        /// The following code demonstrates adding a prepended icon with a touch event handler:
        /// <code><![CDATA[
        /// <SfTextBox @ref="textBoxRef" Placeholder="Enter Date"></SfTextBox>
        /// 
        /// @code {
        ///     SfTextBox textBoxRef;
        ///     
        ///     private async Task AddDateIcon()
        ///     {
        ///         var iconEvents = new Dictionary<string, object>()
        ///         {
        ///             { "onclick", new Action(OnIconClick) },
        ///             { "ontouchstart", new Action(OnIconTouch) }
        ///         };
        ///         await textBoxRef.AddIconAsync("prepend", "e-date-icon", iconEvents);
        ///     }
        ///     
        ///     private void OnIconClick()
        ///     {
        ///         // Handle icon click
        ///     }
        ///     
        ///     private void OnIconTouch()
        ///     {
        ///         // Handle icon touch
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        public async Task AddIconAsync(string position, string icons, Dictionary<string, object>? events = null)
        {
            if (!string.IsNullOrEmpty(position))
            {
                await AddIconsAsync(position, icons, events).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Sets the focus to the <see cref="SfTextBox"/> component to enable user interaction.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation that completes when focus has been set.
        /// </returns>
        /// <remarks>
        /// <para>This method programmatically moves the keyboard focus to the TextBox input element, allowing users to immediately start typing.</para>
        /// <para>When called, the TextBox receives focus and displays the focus indicator (typically a border highlight or cursor). This is useful for improving user experience by automatically focusing important input fields or implementing custom navigation flows.</para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTextBox @ref="textBoxRef" Placeholder="Enter your text here"></SfTextBox>
        /// 
        /// @code {
        ///     SfTextBox textBoxRef;
        ///     
        ///     private async Task SetFocusToTextBox()
        ///     {
        ///         await textBoxRef.FocusAsync();
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        public async Task FocusAsync()
        {
            await InputElement.FocusAsync().ConfigureAwait(true);
        }

        /// <summary>
        /// Removes the focus from the <see cref="SfTextBox"/> component if it is currently in focus state.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation that completes when focus has been removed.
        /// </returns>
        /// <remarks>
        /// <para>This method programmatically removes keyboard focus from the TextBox input element, causing it to blur.</para>
        /// <para>When called, the TextBox loses focus and hides the focus indicator, and any validation or change events may be triggered. This is useful for implementing custom form validation, navigation flows, or when programmatically shifting focus to other elements.</para>
        /// <para>If the TextBox is not currently focused, this method has no effect.</para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTextBox @ref="textBoxRef" Placeholder="Enter your text here"></SfTextBox>
        /// 
        /// @code {
        ///     SfTextBox textBoxRef;
        ///     
        ///     private async Task RemoveFocusFromTextBox()
        ///     {
        ///         await textBoxRef.FocusOutAsync();
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        public async Task FocusOutAsync()
        {
            if (_textBoxJsModule is not null || _textBoxJsInProcessModule is not null)
            {
                await InvokeVoidAsync(_textBoxJsModule, _textBoxJsInProcessModule, "focusOut", InputElement).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Retrieves the properties to be maintained in the persisted state from local storage.
        /// </summary>
        /// <returns>
        /// A <see cref="Task{TResult}"/> of <see cref="string"/> containing the persisted component state as a JSON-formatted string, 
        /// or <see langword="null"/> if persistence is disabled or no previously saved state exists. The returned JSON includes properties 
        /// such as Value, Enabled state, ReadOnly state, and other user-modified settings.
        /// </returns>
        /// <remarks>
        /// <para>This method accesses the browser's local storage to retrieve previously saved component state using the component's ID as the key.</para>
        /// <para>The persisted data typically includes user-modified properties such as the current value, enabled state, and other configurable settings. 
        /// This functionality supports the component's state persistence feature, allowing the TextBox to restore its previous state across browser sessions.</para>
        /// <para>The returned string is in JSON format and can be deserialized to restore the component's state using the corresponding persistence methods.</para>
        /// <para>Persistence is only enabled if the <see cref="SfInputBase{TValue}.EnablePersistence"/> property is set to <see langword="true"/> and a unique component ID is assigned.</para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTextBox @ref="textBoxRef" ID="myTextBox" EnablePersistence="true" Placeholder="Enter your text here"></SfTextBox>
        /// 
        /// @code {
        ///     SfTextBox? textBoxRef;
        ///     
        ///     private async Task GetPersistedData()
        ///     {
        ///         if (textBoxRef != null)
        ///         {
        ///             string? persistedData = await textBoxRef.GetPersistDataAsync();
        ///             if (!string.IsNullOrEmpty(persistedData))
        ///             {
        ///                 // Parse and process the persisted data
        ///                 Console.WriteLine($"Persisted data: {persistedData}");
        ///                 // Example: deserialize JSON to extract specific properties
        ///             }
        ///         }
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        public async Task<string> GetPersistDataAsync()
        {
            return await InvokeAsync<string>(_baseJsModule!, _baseJsInProcessModule!, "getLocalStorageItem", ID).ConfigureAwait(true);
        }

        /// <summary>
        /// Updates the CSS classes for the root element and container element when the <see cref="SfTextBox"/> component is used within a parent container that requires specific styling or layout modifications.
        /// </summary>
        /// <param name="rootClass">
        /// A <see cref="string"/> containing the CSS class to be applied to the component's root element. This class typically controls the overall appearance and behavior of the component.
        /// </param>
        /// <param name="containerClass">
        /// A <see cref="string"/> containing the CSS class to be applied to the component's container element. This class typically controls the container styling and layout within parent components.
        /// </param>
        /// <remarks>
        /// <para>This method is primarily used by parent components (such as InputGroup or FormGroup) to apply appropriate styling and ensure proper visual integration.</para>
        /// <para>After updating the classes, it triggers validation class updates to maintain proper validation state visualization.</para>
        /// <para>This method is intended for internal framework use rather than direct consumer usage.</para>
        /// </remarks>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void UpdateParentClass(string rootClass, string containerClass)
        {
            RootClass = rootClass;
            ContainerClass = containerClass;
            UpdateValidateClass();
        }

        #endregion
    }
}
