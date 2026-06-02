using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    /// <summary>
    /// Represents the Toolkit TextArea component for Blazor applications, which provides a multiline text input element
    /// that allows users to enter and edit multi-line text content.
    /// </summary>
    /// <remarks>
    /// This file contains the public methods for the <see cref="SfTextArea"/> component.
    /// </remarks>
    public partial class SfTextArea : SfInputBase<string>
    {
        /// <summary> 
        /// Sets the focus to the <see cref="SfTextArea"/> component for interaction. 
        /// </summary> 
        /// <returns>A <see cref="Task"/> that represents the asynchronous focus operation.</returns>
        /// <remarks>
        /// This method asynchronously sets the focus to the <see cref="SfTextArea"/> component, enabling user interaction.
        /// When called, the textarea element will receive focus and become the active input field on the page.
        /// This is useful for programmatically directing user attention to the textarea or improving accessibility.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTextArea @ref="textAreaRef" Placeholder="Enter your text here"></SfTextArea>
        /// 
        /// @code {
        ///     SfTextArea textAreaRef;
        ///     
        ///     private async Task SetFocusToTextArea()
        ///     {
        ///         await textAreaRef.FocusAsync();
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        public async Task FocusAsync()
        {
            await InputElement.FocusAsync().ConfigureAwait(true);
        }

        /// <summary> 
        /// Removes the focus from the <see cref="SfTextArea"/> component, if the component is in focus state. 
        /// </summary> 
        /// <returns>A <see cref="Task"/> that represents the asynchronous focus removal operation.</returns>
        /// <remarks>
        /// This method asynchronously removes the focus from the <see cref="SfTextArea"/> component if it currently has focus.
        /// When called, the textarea element will lose focus and will no longer be the active input field.
        /// This method is useful for programmatically controlling focus flow or implementing custom navigation patterns.
        /// If the component is not currently focused, this method will have no effect.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTextArea @ref="textAreaRef" Placeholder="Enter your text here"></SfTextArea>
        /// 
        /// @code {
        ///     SfTextArea textAreaRef;
        ///     
        ///     private async Task RemoveFocusFromTextArea()
        ///     {
        ///         await textAreaRef.FocusOutAsync();
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        public async Task FocusOutAsync()
        {
            if (_textAreaJsModule is not null || _textAreaJsInProcessModule is not null)
            {
                await InvokeVoidAsync(_textAreaJsModule, _textAreaJsInProcessModule, "focusOut", [InputElement]).ConfigureAwait(true);
            }
        }

        /// <summary> 
        /// Gets the properties to be maintained in the persisted state. 
        /// </summary> 
        /// <returns>A <see cref="Task{TResult}"/> of type <see cref="string"/> that represents the asynchronous operation. The result contains the persisted data as a JSON string, or <c>null</c> if no data is persisted.</returns>
        /// <remarks>
        /// This method asynchronously retrieves the persisted state data for the <see cref="SfTextArea"/> component from the browser's local storage.
        /// The returned string contains the serialized properties that were previously saved using the component's persistence functionality.
        /// This method is typically used internally by the component to restore its state when the <c>EnablePersistence</c> property is enabled.
        /// The persisted data is stored using the component's ID as the key in the browser's local storage.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTextArea @ref="textAreaRef" ID="myTextArea" EnablePersistence="true" Placeholder="Enter your text here"></SfTextArea>
        /// 
        /// @code {
        ///     SfTextArea textAreaRef;
        ///     
        ///     private async Task GetPersistedData()
        ///     {
        ///         string persistedData = await textAreaRef.GetPersistDataAsync();
        ///         if (!string.IsNullOrEmpty(persistedData))
        ///         {
        ///             // Process the persisted data
        ///             Console.WriteLine($"Persisted data: {persistedData}");
        ///         }
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        public async Task<string> GetPersistDataAsync()
        {
            return await InvokeAsync<string>(_baseJsModule!, _baseJsInProcessModule!, "getLocalStorageItem", [ID]).ConfigureAwait(true);
        }
    }
}