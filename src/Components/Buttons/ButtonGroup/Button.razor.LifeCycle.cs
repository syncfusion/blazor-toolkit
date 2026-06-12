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
        #region Lifecycle Methods

        /// <exclude />
        /// <summary>
        /// Initializes the component and sets its initial state.
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents the asynchronous initialization operation.</returns>
        /// <remarks>
        /// Sets the initial selected state and registers the button with its parent <see cref="SfButtonGroup"/>.
        /// </remarks>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);
            _selected = Selected;
            _buttonSelected = Selected;
            InputId = SfBaseUtils.GenerateID("SfButtonGroup");
            ButtonGroup?.UpdateChildProperty(this);
            if (Selected)
            {
                _isFirstClick = false;
            }
        }

        /// <exclude />
        /// <summary>
        /// Executes when the component's parameters are set.
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents the asynchronous parameter setting operation.</returns>
        /// <remarks>
        /// This method is invoked when the component's parameters are updated. It manages changes to the <see cref="Selected"/> property based on the <see cref="SfButtonGroup.Mode"/> and re-renders the component.
        /// </remarks>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(false);

            if (ButtonGroup?.Mode == SelectionMode.Multiple && IsRendered && !ButtonGroup._isClicked)
            {
                _selected = NotifyPropertyChanges(nameof(Selected), Selected, _selected);
                if (PropertyChanges is not null && PropertyChanges.Count > 0)
                {
                    foreach (string key in PropertyChanges.Keys)
                    {
                        switch (key)
                        {
                            case nameof(Selected):
                                await UpdateButtonStateAsync(Selected).ConfigureAwait(false);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            if (ButtonGroup?.Mode == SelectionMode.Single && IsRendered)
            {
                if (ButtonGroup._isClicked)
                {
                    await UpdateButtonStateAsync(_buttonSelected).ConfigureAwait(false);
                }
                else
                {
                    await UpdateButtonStateAsync(Selected).ConfigureAwait(false);
                }
            }
        }

        /// <exclude />
        /// <summary>
        /// Invoked after the component has been rendered.
        /// </summary>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender).ConfigureAwait(false);
            if (ButtonGroup?._isClicked ?? false)
            {
                ButtonGroup._isClicked = false;
            }
        }

        /// <exclude />
        /// <summary>
        /// Disposes attributes and button group instance.
        /// </summary>
        protected override ValueTask DisposeAsyncCore()
        {
            // Clear local attribute reference to allow GC to collect attribute values.
            _inputAttributes.Clear();
            // If registered with a parent group, remove this instance from the parent's list.
            ButtonGroup?.RemoveChildProperty(this);
            ButtonGroup = null;
            return base.DisposeAsyncCore();
        }

        #endregion
    }
}
