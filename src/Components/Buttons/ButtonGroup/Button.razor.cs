using Microsoft.AspNetCore.Components;
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
        /// Tracks whether the button has been clicked at least once.
        /// </summary>
        private bool _isFirstClick = true;

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

            _buttonSelected = (ButtonGroup.Mode == SelectionMode.Single && !_isFirstClick) || !Selected;

            for (int i = 0; i < ButtonGroup._buttonItems.Count; i++)
            {
                if (!SfBaseUtils.Equals(this, ButtonGroup._buttonItems[i]))
                {
                    ButtonGroup._buttonItems[i]._buttonSelected = false;
                    ButtonGroup._buttonItems[i].Selected = false;
                }
            }

            await UpdateButtonStateAsync(_buttonSelected).ConfigureAwait(false);
            _isFirstClick = false;
        }

        #endregion

        #region Helper Methods

        /// <exclude />
        /// <summary>
        /// Asynchronously updates the selection state of the button.
        /// </summary>
        /// <param name="state">The new selection state to apply.</param>
        protected async Task UpdateButtonStateAsync(bool state)
        {
            Selected = _selected = await SfBaseUtils.UpdatePropertyAsync(state, _selected, SelectedChanged).ConfigureAwait(false);
        }

        #endregion
    }
}
