using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Toolkit.Internal;

namespace Syncfusion.Blazor.Toolkit.Spinner
{
    /// <summary>
    /// Contains the lifecycle method implementations for the <see cref="SfSpinner"/> component.
    /// </summary>
    /// <remarks>
    /// This partial class handles component initialization, parameter updates, rendering, and disposal.
    /// It ensures proper state management and resource cleanup to prevent memory leaks.
    /// </remarks>
    public partial class SfSpinner : SfBaseComponent
    {
        #region Lifecycle Methods

        /// <summary>
        /// Initializes the spinner component, configuring initial CSS classes, visibility state, and accessibility attributes.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method is invoked by the Blazor framework when the spinner is first created. It sets up the initial
        /// style with <see cref="ZIndex"/>, applies the <see cref="CssClass"/> if provided, and configures the
        /// <c>aria-label</c> attribute based on the <see cref="Label"/> property for accessibility compliance.
        /// </remarks>
        /// <exclude />
        protected override async Task OnInitializedAsync()
        {
            try
            {
                // Initialize style with z-index
                _style = $"{ZIndexStyle}: {ZIndex};";
                _attributes.Add(Style, _style);

                // Call base initialization
                await base.OnInitializedAsync().ConfigureAwait(false);

                // Cache the current property values for comparison
                _zIndex = ZIndex;
                _cssClass = CssClass;
                _previousCssClass = CssClass;

                // Add custom CSS class if provided
                if (!string.IsNullOrEmpty(CssClass))
                {
                    _spinnerClass = SfBaseUtils.AddClass(_spinnerClass, CssClass);
                }

                // Set initial visibility class
                _spinnerClass = SfBaseUtils.AddClass(_spinnerClass, Visible ? ClassShow : ClassHide);
                _previousVisible = Visible;

                // Enable rendering if initially visible
                if (Visible)
                {
                    _enableRender = true;
                }

                // Set aria-label for accessibility based on Label parameter
                if (!string.IsNullOrEmpty(Label))
                {
                    _attributes[AriaLabel] = Label;
                }
                else
                {
                    _attributes[AriaLabel] = Loading;
                }
            }
            catch (Exception ex)
            {
                // Log or handle initialization errors
                Logger?.LogError(ex, "Error during SfSpinner initialization");
                throw;
            }
        }

        /// <summary>
        /// Handles parameter updates from the parent component.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method is called when the spinner receives new or updated parameters from its parent.
        /// It tracks changes to properties like <see cref="CssClass"/>, <see cref="Visible"/>, and <see cref="ZIndex"/>
        /// and triggers a state update if any of these properties have changed.
        /// </remarks>
        /// <exclude />
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(false);

            // Sync aria-label to ensure it's always present for accessibility
            if (!string.IsNullOrEmpty(Label))
            {
                _attributes[AriaLabel] = Label;
            }
            else if (!_attributes.ContainsKey(AriaLabel))
            {
                _attributes[AriaLabel] = Loading;
            }

            // Notify and track property changes
            _cssClass = NotifyPropertyChanges(nameof(CssClass), CssClass, _cssClass);
            _visible = NotifyPropertyChanges(nameof(Visible), Visible, _visible);
            _zIndex = NotifyPropertyChanges(nameof(ZIndex), ZIndex, _zIndex);

            // Process any detected property changes
            if (PropertyChanges is not null && PropertyChanges.Count > 0)
            {
                await OnPropertyChangeAsync([.. PropertyChanges.Keys]).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Executes after the spinner has been rendered.
        /// </summary>
        /// <param name="firstRender">Indicates whether this is the first render cycle. When <see langword="true"/>, this is the initial render; when <see langword="false"/>, the spinner is being re-rendered.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// On the initial render, this method sets the internal load flag to <see langword="false"/> and invokes the <see cref="Created"/> event if it has been subscribed.
        /// </remarks>
        /// <exclude />
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender).ConfigureAwait(true);
            if (firstRender)
            {
                _isInitialLoad = false;

                // Invoke the Created event if subscribed
                if (Created.HasDelegate)
                {
                    await Created.InvokeAsync(new { Name = CreatedEvent }).ConfigureAwait(false);
                }
                if (Visible)
                {
                    await InvokeAsync(() =>
                    {
                        _enableRender = _previousVisible = true;
                        StateHasChanged();
                    }).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Releases all resources associated with the spinner upon disposal.
        /// </summary>
        /// <remarks>
        /// This method is called when the spinner is being removed from the UI. It clears child content, resets attributes,
        /// and invokes the <see cref="Destroyed"/> event if subscribed, ensuring proper resource management and preventing memory leaks.
        /// </remarks>
        /// <exclude />
        protected override async ValueTask DisposeAsyncCore()
        {
            // Skip disposal if component was never rendered
            if (!IsRendered)
            {
                return;
            }

            try
            {
                // Clear references to prevent memory leaks
                _attributes.Clear();
                _spinnerTemplate = null;

                // Invoke the Destroyed event if subscribed
                if (Destroyed.HasDelegate)
                {
                    await Destroyed.InvokeAsync(new { Name = DestroyedEvent }).ConfigureAwait(false);
                }
            }
            catch (ObjectDisposedException ex)
            {
                // Handle disposed object exception
                await InvokeDestroyedEventOnErrorAsync(ex).ConfigureAwait(false);
            }
            catch (InvalidOperationException ex)
            {
                // Handle invalid operation exception
                await InvokeDestroyedEventOnErrorAsync(ex).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                // Log unexpected exceptions but do not rethrow in async void to prevent unobserved exceptions
                Logger?.LogError(ex, "Unexpected error during SfSpinner disposal");
            }
            await base.DisposeAsyncCore().ConfigureAwait(true);
        }

        #endregion
    }

}
