using Syncfusion.Blazor.Toolkit.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace Syncfusion.Blazor.Toolkit.Spinner
{
    /// <summary>
    /// The Spinner is a component that provides a visual indication of an ongoing operation, such as loading or processing, to keep the user informed and engaged.
    /// </summary>
    /// <remarks>
    /// The <see cref="SfSpinner"/> can be customized with different sizes, colors, and templates to match the application's design. 
    /// It supports embedding within other components and can be shown or hidden programmatically.
    /// This component ensures accessibility compliance with proper ARIA attributes and supports content security policies.
    /// </remarks>
    /// <example>
    /// A simple Spinner component.
    /// <code><![CDATA[ 
    /// <SfSpinner @bind-Visible="@SpinnerVisible" />
    ///
    /// @code {
    ///     private bool SpinnerVisible { get; set; } = true;
    /// }
    /// ]]></code>
    /// </example>
    public partial class SfSpinner : SfBaseComponent
    {
        #region Constants
        /// <summary>
        /// CSS class constants for spinner styling and visibility states.
        /// </summary>
        private const string SpinnerClass = "e-spinner-pane";
        private const string SpinnerInnerClass = "e-spinner-inner";
        private const string ClassShow = "e-spin-show";
        private const string ClassHide = "e-spin-hide";
        private const string ClassLabel = "e-spin-label";

        /// <summary>
        /// HTML attribute constants.
        /// </summary>
        private const string Style = "style";
        private const string AriaLabel = "aria-label";
        private const string Loading = "Loading";
        private const string Auto = "auto";
        private const string ZIndexStyle = "z-index";

        /// <summary>
        /// Property name constants for property change tracking.
        /// </summary>
        private const string CssClassProp = "CssClass";
        private const string ZIndexProp = "ZIndex";
        private const string VisibleProp = "Visible";

        /// <summary>
        /// Event name constants.
        /// </summary>
        private const string CreatedEvent = "Created";
        private const string DestroyedEvent = "Destroyed";
        #endregion

        #region Fields
        /// <summary>
        /// Custom template for the spinner content.
        /// </summary>
        private RenderFragment? _spinnerTemplate;

        /// <summary>
        /// Flag indicating whether the spinner should be rendered.
        /// </summary>
        private bool _enableRender;

        /// <summary>
        /// Tracks the previous CSS class for proper removal.
        /// </summary>
        private string? _removedClass;

        /// <summary>
        /// Tracks the previous visibility state.
        /// </summary>
        private bool _previousVisible;

        /// <summary>
        /// Tracks the previous CSS class value for comparison.
        /// </summary>
        private string? _previousCssClass;

        /// <summary>
        /// Flag to track the initial load state.
        /// </summary>
        private bool _isInitialLoad = true;

        /// <summary>
        /// Inline style string for the spinner element.
        /// </summary>
        private string _style = string.Empty;

        /// <summary>
        /// Complete CSS class string for the spinner element.
        /// </summary>
        private string _spinnerClass = SpinnerClass;

        /// <summary>
        /// Dictionary of HTML attributes to apply to the spinner element.
        /// </summary>
        private readonly Dictionary<string, object> _attributes = [];

        /// <summary>
        /// Cached z-index value.
        /// </summary>
        private string? _zIndex;

        /// <summary>
        /// Cached CSS class value.
        /// </summary>
        private string? _cssClass;

        /// <summary>
        /// Cached visibility state.
        /// </summary>
        private bool _visible;

        /// <summary>
        /// Optional logger for error reporting. Allows errors during component lifecycle to be logged instead of silently discarded.
        /// </summary>
        [Inject]
        private ILogger<SfSpinner>? Logger { get; set; }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Shows the spinner by updating visibility state and invoking the before open event.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method triggers the OnBeforeOpen event, allowing subscribers to cancel the show operation.
        /// If not cancelled, it updates the CSS classes and triggers a component re-render.
        /// </remarks>
        private async Task ShowInternalAsync()
        {
            // Check if OnBeforeOpen event is subscribed and invoke it
            if (OnOpen.HasDelegate)
            {
                SpinnerEventArgs openEventArgs = new() { Cancel = false };
                await OnOpen.InvokeAsync(openEventArgs).ConfigureAwait(false);

                // If the event handler cancelled the operation, exit early
                if (openEventArgs.Cancel)
                {
                    return;
                }
            }

            // Update CSS classes for visibility
            _spinnerClass = SfBaseUtils.RemoveClass(_spinnerClass, ClassHide);
            _spinnerClass = SfBaseUtils.AddClass(_spinnerClass, ClassShow);
            // ConfigureAwait(true) required: UpdateVisibleAsync calls InvokeAsync(StateHasChanged) which must run on UI context
            await UpdateVisibleAsync(true).ConfigureAwait(true);
        }

        /// <summary>
        /// Updates the visible state of the spinner.
        /// </summary>
        /// <param name="visibility">The visibility state to set.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method synchronizes the internal visibility state with the component parameter
        /// and triggers a re-render to reflect the changes.
        /// </remarks>
        private async Task UpdateVisibleAsync(bool visibility)
        {
            _previousVisible = Visible;
            Visible = _visible = await SfBaseUtils.UpdatePropertyAsync(visibility, _visible, VisibleChanged).ConfigureAwait(false);
            _enableRender = visibility;
            await InvokeAsync(StateHasChanged).ConfigureAwait(true);
        }

        /// <summary>
        /// Hides the spinner by updating visibility state and invoking the before close event.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method triggers the OnBeforeClose event, allowing subscribers to cancel the hide operation.
        /// If not cancelled, it updates the CSS classes and triggers a component re-render.
        /// </remarks>
        private async Task HideInternalAsync()
        {
            // Check if OnBeforeClose event is subscribed and invoke it
            if (OnClose.HasDelegate)
            {
                SpinnerEventArgs closeEventArgs = new() { Cancel = false };
                await OnClose.InvokeAsync(closeEventArgs).ConfigureAwait(false);

                // If the event handler cancelled the operation, exit early
                if (closeEventArgs.Cancel)
                {
                    return;
                }
            }

            // Update CSS classes for visibility
            _spinnerClass = SfBaseUtils.RemoveClass(_spinnerClass, ClassShow);
            _spinnerClass = SfBaseUtils.AddClass(_spinnerClass, ClassHide);
            await UpdateVisibleAsync(false).ConfigureAwait(false);
        }

        /// <summary>
        /// Invoked when one or more component properties have changed.
        /// </summary>
        /// <param name="changedKeys">A <see cref="List{T}"/> of string values that specifies the names of the properties that have changed.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        /// <remarks>
        /// This method is part of the component's lifecycle and is used to respond to changes in properties like <c>Visible</c>, <c>ZIndex</c>, and <c>CssClass</c>.
        /// It dynamically updates the component's state and appearance based on the new property values.
        /// </remarks>
        private async Task OnPropertyChangeAsync(List<string>? changedKeys)
        {
            // Guard clause: return if no keys are provided
            if (changedKeys is null || changedKeys.Count == 0)
            {
                return;
            }

            foreach (string key in changedKeys)
            {
                switch (key)
                {
                    case VisibleProp:
                        await HandleVisibleChangeAsync().ConfigureAwait(false);
                        break;
                    case ZIndexProp:
                        HandleZIndexChange();
                        break;
                    case CssClassProp:
                        HandleCssClassChange();
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Handles changes to the Visible property.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method determines whether to show or hide the spinner based on the visibility state change.
        /// It skips processing during the initial load to avoid unnecessary updates.
        /// </remarks>
        private async Task HandleVisibleChangeAsync()
        {
            if (!_isInitialLoad && _previousVisible != Visible && Visible)
            {
                await ShowInternalAsync().ConfigureAwait(false);
            }

            if (!Visible && _previousVisible != Visible)
            {
                await HideInternalAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Handles changes to the ZIndex property.
        /// </summary>
        /// <remarks>
        /// This method updates the inline style with the new z-index value,
        /// controlling the stacking order of the spinner element.
        /// </remarks>
        private void HandleZIndexChange()
        {
            // Build style string with z-index
            _style = $"{ZIndexStyle}: {ZIndex};";
            _attributes[Style] = _style;
        }

        /// <summary>
        /// Handles changes to the CssClass property.
        /// </summary>
        /// <remarks>
        /// This method manages the addition and removal of CSS classes from the spinner element.
        /// It properly cleans up old classes before adding new ones.
        /// </remarks>
        private void HandleCssClassChange()
        {
            // Remove the previous CSS class if it exists
            if (!string.IsNullOrEmpty(_previousCssClass))
            {
                _spinnerClass = SfBaseUtils.RemoveClass(_spinnerClass, _previousCssClass);
                _removedClass = _previousCssClass;
            }

            // Add the new CSS class if it's provided
            if (!string.IsNullOrEmpty(CssClass))
            {
                _spinnerClass = SfBaseUtils.AddClass(_spinnerClass, CssClass);
            }

            // Ensure the removed class doesn't exist in the final class string
            if (!string.IsNullOrEmpty(_removedClass) && _spinnerClass.Contains(_removedClass, StringComparison.Ordinal))
            {
                _spinnerClass = SfBaseUtils.RemoveClass(_spinnerClass, _removedClass);
            }

            // Update the previous CSS class reference
            _previousCssClass = CssClass;
        }

        /// <summary>
        /// Updates the spinner template with custom content.
        /// </summary>
        /// <param name="template">The custom template to display.</param>
        /// <remarks>
        /// This method is called by the SpinnerTemplates child component to provide
        /// a custom rendering template that replaces the default spinner animation.
        /// </remarks>
        internal void UpdateTemplate(RenderFragment template)
        {
            _spinnerTemplate = template;
        }

        /// <summary>
        /// Invokes the Destroyed event when an error occurs during disposal.
        /// </summary>
        /// <param name="exception">The exception that occurred.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method safely invokes the Destroyed event even when an exception occurs,
        /// allowing subscribers to perform cleanup operations.
        /// </remarks>
        private async Task InvokeDestroyedEventOnErrorAsync(Exception exception)
        {
            try
            {
                if (Destroyed.HasDelegate)
                {
                    await Destroyed.InvokeAsync(exception).ConfigureAwait(false);
                }
            }
            catch (ObjectDisposedException ex)
            {
                Logger?.LogError(ex, "Error invoking Destroyed event");
            }
        }
        #endregion
    }
}
