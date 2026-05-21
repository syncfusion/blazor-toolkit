using Syncfusion.Blazor.Toolkit.Internal;
using Microsoft.AspNetCore.Components.Web;

namespace Syncfusion.Blazor.Toolkit.Popups
{
    public partial class SfDialog : SfBaseComponent
    {
        /// <summary>
        /// Returns the specific <see cref="DialogButton"/> instance at the specified index in the <see cref="SfDialog"/> component.
        /// </summary>
        /// <param name="index">The zero-based index of the button to retrieve.</param>
        /// <returns>A <see cref="DialogButton"/> instance if found at the specified index; otherwise, <c>null</c>.</returns>
        /// <remarks>
        /// This method allows you to access individual dialog buttons by their index position. 
        /// The index is zero-based, meaning the first button has an index of 0.
        /// If the index is out of range or no buttons are configured, the method returns <c>null</c>.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the index is negative or exceeds the button collection bounds.</exception>
        /// <example>
        /// <code><![CDATA[
        /// // Get the first button from the dialog
        /// DialogButton firstButton = dialog.GetButton(0);
        /// if (firstButton is not null)
        /// {
        ///     // Use the button instance
        ///     Console.WriteLine($"Button text: {firstButton.Content}");
        /// }
        /// ]]></code>
        /// </example>
        public DialogButton? GetButton(int index)
        {
            return ButtonsValue?[index];
        }

        /// <summary>
        /// Returns the complete collection of <see cref="DialogButton"/> instances configured for the <see cref="SfDialog"/> component.
        /// </summary>
        /// <returns>A <see cref="List{T}"/> of <see cref="DialogButton"/> instances, or <c>null</c> if no buttons are configured.</returns>
        /// <remarks>
        /// This method provides access to all dialog buttons that have been configured for the dialog component.
        /// The returned collection includes all buttons in their original order as they appear in the dialog.
        /// If no buttons are configured, the method returns <c>null</c>.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// // Get all buttons from the dialog
        /// List<DialogButton> allButtons = dialog.GetButtonItems();
        /// if (allButtons is not null)
        /// {
        ///     foreach (var button in allButtons)
        ///     {
        ///         Console.WriteLine($"Button: {button.Content}");
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        public List<DialogButton>? GetButtonItems()
        {
            List<DialogButton>? button = ButtonsValue;
            return button;
        }

        /// <summary>
        /// Closes the <see cref="SfDialog"/> component if it is currently visible.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous hide operation.</returns>
        /// <remarks>
        /// This method programmatically closes the dialog if it is currently in a visible state.
        /// If the dialog is already hidden, calling this method has no effect.
        /// The method executes asynchronously and completes when the dialog is fully closed.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// // Hide the dialog programmatically
        /// await dialog.HideAsync();
        /// ]]></code>
        /// </example>
        public async Task HideAsync()
        {
            await HideDialogAsync(null).ConfigureAwait(false);
        }

        /// <summary>
        /// Closes the <see cref="SfDialog"/> component if it is currently visible, with optional interaction context.
        /// </summary>
        /// <param name="args">Optional string parameter that specifies the interaction type or context for closing the dialog.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous hide operation.</returns>
        /// <remarks>
        /// This overload allows you to specify additional context about how the dialog is being closed.
        /// The <paramref name="args"/> parameter can be used to pass information about the user interaction that triggered the close operation.
        /// If the dialog is already hidden, calling this method has no effect.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// // Hide the dialog with interaction context
        /// await dialog.HideAsync("cancel");
        /// ]]></code>
        /// </example>
        public async Task HideAsync(string args = "")
        {
            await HideDialogAsync(args, null).ConfigureAwait(false);
        }

        /// <summary>
        /// Closes the <see cref="SfDialog"/> component if it is currently visible, triggered by a mouse event.
        /// </summary>
        /// <param name="args">The <see cref="MouseEventArgs"/> containing details about the mouse interaction that triggered the close operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous hide operation.</returns>
        /// <remarks>
        /// This overload is typically used when the dialog needs to be closed in response to a mouse event, such as clicking outside the dialog or on a close button.
        /// The mouse event arguments provide context about the specific mouse interaction that triggered the close operation.
        /// If the dialog is already hidden, calling this method has no effect.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// // Hide the dialog in response to a mouse click event
        /// private async Task OnCloseButtonClick(MouseEventArgs e)
        /// {
        ///     await dialog.HideAsync(e);
        /// }
        /// ]]></code>
        /// </example>
        public async Task HideAsync(MouseEventArgs args)
        {
            await HideDialogAsync(null, new BeforeCloseEventArgs() { Event = args }).ConfigureAwait(false);
        }

        /// <summary>
        /// Closes the <see cref="SfDialog"/> component if it is currently visible, triggered by a keyboard event.
        /// </summary>
        /// <param name="args">The <see cref="KeyboardEventArgs"/> containing details about the keyboard interaction that triggered the close operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous hide operation.</returns>
        /// <remarks>
        /// This overload is typically used when the dialog needs to be closed in response to a keyboard event, such as pressing the Escape key.
        /// The keyboard event arguments provide context about the specific key interaction that triggered the close operation.
        /// If the dialog is already hidden, calling this method has no effect.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// // Hide the dialog in response to a keyboard event
        /// private async Task OnKeyDown(KeyboardEventArgs e)
        /// {
        ///     if (e.Key == "Escape")
        ///     {
        ///         await dialog.HideAsync(e);
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        public async Task HideAsync(KeyboardEventArgs args)
        {
            await HideDialogAsync(null, new BeforeCloseEventArgs() { Event = args }).ConfigureAwait(false);
        }

        /// <summary>
        /// Refreshes and recalculates the position of the <see cref="SfDialog"/> component after dynamic size changes.
        /// </summary>        
        /// <returns>A <see cref="Task"/> representing the asynchronous position refresh operation.</returns>
        /// <remarks>
        /// This method is useful when the dialog's dimensions (height and width) are changed programmatically or dynamically at runtime.
        /// After calling this method, the dialog will recalculate its position to ensure it remains properly centered or positioned according to its configuration.
        /// This is particularly important for maintaining proper dialog positioning when content changes affect the dialog's size.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// // Refresh dialog position after dynamic content change
        /// dialog.Width = "800px";
        /// dialog.Height = "600px";
        /// await dialog.RefreshPositionAsync();
        /// ]]></code>
        /// </example>
        public async Task RefreshPositionAsync()
        {
            await InvokeVoidAsync(_dialogJsModule, _dialogJsInProcessModule, JS_REFRESH_POSITION, _dataId).ConfigureAwait(true);
        }

        /// <summary>
        /// Opens the <see cref="SfDialog"/> component if it is currently in a hidden state.
        /// </summary>
        /// <param name="isFullScreen">Optional boolean value that specifies whether the dialog should open in full screen mode. If <c>null</c>, uses the default behavior. Default is <c>false</c>.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous show operation.</returns>
        /// <remarks>
        /// This method programmatically opens the dialog if it is currently hidden.
        /// When <paramref name="isFullScreen"/> is set to <c>true</c>, the dialog will occupy the entire viewport.
        /// If the dialog is already visible, calling this method may update its full screen state based on the parameter.
        /// The method handles both pre-render and post-render scenarios appropriately.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// // Show the dialog in normal mode
        /// await dialog.ShowAsync();
        /// 
        /// // Show the dialog in full screen mode
        /// await dialog.ShowAsync(true);
        /// ]]></code>
        /// </example>
        public async Task ShowAsync(bool? isFullScreen = null)
        {
            if (!_preventVisibility)
            {
                _enableFullScreen = (bool)(isFullScreen is not null ? isFullScreen : false);
                if (!AllowPrerender && !Visible)
                {
                    IsPreRender = true;
                    _isShowCall = true;
                    if (Visible == _visible)
                    {
                        bool tempValue = _visible;
                        _visible = true;
                        _visible = await SfBaseUtils.UpdatePropertyAsync(true, tempValue, VisibleChanged).ConfigureAwait(false);
                    }
                }
                else
                {
                    await ShowDialogAsync().ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Gets the current dimensions (height and width) of the <see cref="SfDialog"/> component.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> that returns a <see cref="DialogDimension"/> object containing the current height and width of the dialog.</returns>
        /// <remarks>
        /// This method retrieves the actual rendered dimensions of the dialog component from the DOM.
        /// The returned <see cref="DialogDimension"/> object contains both the height and width values as they appear in the browser.
        /// This is useful for programmatically determining the dialog's current size, especially when using responsive or dynamic sizing.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// // Get current dialog dimensions
        /// DialogDimension dimensions = await dialog.GetDimensionAsync();
        /// Console.WriteLine($"Width: {dimensions.Width}, Height: {dimensions.Height}");
        /// ]]></code>
        /// </example>
        public async Task<DialogDimension> GetDimensionAsync()
        {
            return await InvokeAsync<DialogDimension>(_dialogJsModule!, _dialogJsInProcessModule!, JS_GET_DIMENSION, _dataId).ConfigureAwait(false);
        }
    }
}
