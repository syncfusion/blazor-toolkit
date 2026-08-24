using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Toolkit.Internal;

namespace Syncfusion.Blazor.Toolkit.Buttons
{
    /// <summary>
    /// Represents a single button within a <see cref="SfButtonGroup"/>. The button can display text, an icon, or both, and triggers an action when clicked.
    /// </summary>
    /// <remarks>
    /// The content for the <see cref="Button"/> can be defined using the <see cref="Content"/> property or by placing markup inside the component tag.
    /// </remarks>
    /// <example>
    /// This example demonstrates how to create a basic <see cref="SfButtonGroup"/> with several <see cref="Button"/> components.
    /// <code><![CDATA[
    /// <SfButtonGroup>
    ///   <Button Content="Left"></Button>
    ///   <Button Content="Center"></Button>
    ///   <Button Content="Right"></Button>
    /// </SfButtonGroup>
    /// ]]></code>
    /// </example>
    public partial class Button
    {

        #region Fields
        /// <exclude />
        /// <summary>
        /// Internal backing field for the selected state used during two-way binding updates.
        /// </summary>
        private bool _selected;

        /// <exclude />
        /// <summary>
        /// Tracks the target selected state during click handling for single-selection mode.
        /// </summary>
        private bool _buttonSelected;

        /// <exclude />
        /// <summary>
        /// Additional HTML attributes applied to the input element when the ButtonGroup uses selection mode.
        /// </summary>
        internal Dictionary<string, object> _inputAttributes = [];

        #endregion

        #region Internal properties

        /// <exclude />
        /// <summary>
        /// Reference to the parent <see cref="SfButtonGroup"/> that contains this button.
        /// </summary>
        [CascadingParameter]
        internal SfButtonGroup? ButtonGroup { get; set; }

        #endregion

        #region Event Handlers

        /// <exclude />
        /// <summary>
        /// Handles keyboard interactions on the input element, specifically Space key activation.
        /// </summary>
        /// <remarks>
        /// Space key activation is necessary because native radio/checkbox inputs respond to Enter,
        /// not Space. This handler ensures the button activates when Space is pressed while focused.
        /// </remarks>
        /// <param name="args">The keyboard event arguments.</param>
        private async Task HandleInputKeyDownAsync(KeyboardEventArgs args)
        {
            if (args is null || string.IsNullOrEmpty(args.Key))
            {
                return;
            }

            // Native radio/checkbox inputs respond to Space (toggle) and Enter (default click on a
            // wrapping label). Both should trigger the same selection flow.
            if (args.Code == "Space" || args.Key == " " || args.Key == "Enter")
            {
                await ClickHandlerAsync().ConfigureAwait(false);
            }
        }

        /// <exclude />
        /// <summary>
        /// Handles native <c>change</c> events from the underlying input element so the selection state
        /// is kept in sync with the DOM in scenarios where the browser changes the input state without
        /// going through <see cref="ClickHandlerAsync"/> (e.g., assistive technology interactions).
        /// </summary>
        /// <param name="args">The change event arguments from the input.</param>
        private async Task HandleInputChangeAsync(ChangeEventArgs args)
        {
            if (ButtonGroup is null || Disabled || ButtonGroup._buttonItems is null)
            {
                return;
            }

            if (args?.Value is null)
            {
                return;
            }

            bool newValue;
            try
            {
                newValue = Convert.ToBoolean(args.Value, System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (InvalidCastException)
            {
                return;
            }
            catch (FormatException)
            {
                return;
            }

            // Only update the state when the DOM-driven value diverges from the current value.
            // This avoids re-entering the click flow and keeps two-way binding stable.
            if (newValue != Selected)
            {
                ButtonGroup._isClicked = true;
                if (ButtonGroup.Mode == SelectionMode.Multiple)
                {
                    await UpdateButtonStateAsync(newValue).ConfigureAwait(false);
                }
                else if (ButtonGroup.Mode == SelectionMode.Single && newValue)
                {
                    await ButtonGroup.ClearSiblingsAsync(this).ConfigureAwait(false);
                    await UpdateButtonStateAsync(true).ConfigureAwait(false);
                }
            }
        }

        /// <exclude />
        /// <summary>
        /// Handles click interaction and coordinates selection state with the parent group.
        /// </summary>
        private async Task ClickHandlerAsync()
        {
            if (ButtonGroup is null || Disabled || ButtonGroup._buttonItems is null)
            {
                return;
            }

            ButtonGroup._isClicked = true;
            if (ButtonGroup.Mode == SelectionMode.Multiple)
            {
                await UpdateButtonStateAsync(!Selected).ConfigureAwait(false);
                return;
            }

            if (ButtonGroup.Mode == SelectionMode.Single)
            {
                await ButtonGroup.ClearSiblingsAsync(this).ConfigureAwait(false);
                await UpdateButtonStateAsync(true).ConfigureAwait(false);
            }
        }

        #endregion

        #region Helper Methods

        /// <exclude />
        /// <summary>
        /// Asynchronously updates the selection state of the button.
        /// </summary>
        /// <param name="state">The new selection state to apply.</param>
        internal async Task UpdateButtonStateAsync(bool state)
        {
            Selected = _selected = _buttonSelected = await SfBaseUtils.UpdatePropertyAsync(state, _selected, SelectedChanged).ConfigureAwait(false);
        }

        #endregion
    }
}
