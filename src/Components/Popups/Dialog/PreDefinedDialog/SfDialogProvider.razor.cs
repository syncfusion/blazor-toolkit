using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;

namespace Syncfusion.Blazor.Toolkit.Popups
{
    /// <summary>
    /// The DialogProvider component serves as a target container where built-in dialogs are rendered using <see cref="SfDialogService.ConfirmAsync(string, string, DialogOptions)"/>,
    /// <see cref="SfDialogService.AlertAsync(string, string, DialogOptions)"/>, and <see cref="SfDialogService.PromptAsync(string, string, DialogOptions)"/> 
    /// methods from the <see cref="SfDialogService"/>. 
    /// </summary>
    /// <remarks>
    /// It is recommended to add this component in <c>MainLayout.razor</c> to enable utility dialogs to be displayed from anywhere in the application.
    /// If you add this component to a specific page, the utility dialogs will only be available for that particular page.
    /// The component automatically handles the lifecycle of dialogs created through the <see cref="SfDialogService"/>, including their opening, closing, and result handling.
    /// </remarks>
    /// <example>
    /// Adding the DialogProvider component to your application layout.
    /// <code><![CDATA[
    /// @* In MainLayout.razor *@
    /// <div class="page">
    ///     <div class="sidebar">
    ///         <NavMenu />
    ///     </div>
    ///     <main>
    ///         <div class="top-row px-4">
    ///             <a href="https://docs.microsoft.com/aspnet/" target="_blank">About</a>
    ///         </div>
    ///         <article class="content px-4">
    ///             @Body
    ///         </article>
    ///     </main>
    /// </div>
    /// <SfDialogProvider />
    /// ]]></code>
    /// </example>
    public partial class SfDialogProvider : ComponentBase, IDisposable
    {
        /// <summary>
        /// Gets or sets the logger instance for diagnostic and error logging purposes.
        /// </summary>
        /// <remarks>
        /// This logger is injected via dependency injection and used to log errors during dialog operations.
        /// </remarks>
        [Inject]
        internal ILogger<SfDialogProvider>? Logger { get; set; }

        /// <summary>
        /// Gets or sets the injected <see cref="SfDialogService"/> instance used for dialog operations.
        /// </summary>
        /// <remarks>
        /// This service is automatically injected by the Blazor dependency injection system and provides
        /// methods for displaying alert, confirm, and prompt dialogs.
        /// </remarks>
        [Inject]
        private SfDialogService? Service { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the dialog is currently visible.
        /// </summary>
        /// <remarks>
        /// This property controls the visibility state of the dialog in the UI.
        /// </remarks>
        private bool IsVisible { get; set; } = true;

        /// <summary>
        /// Gets or sets the input value entered by the user in prompt dialogs.
        /// </summary>
        /// <remarks>
        /// This property stores the text input from the user when using prompt dialogs.
        /// It is returned as the result when the dialog is confirmed.
        /// </remarks>
        private string? InputValue { get; set; }

        /// <summary>
        /// Gets or sets the current dialog configuration options.
        /// </summary>
        /// <remarks>
        /// This property holds the <see cref="DialogOptions"/> passed from the dialog service
        /// and is used to configure the dialog appearance and behavior.
        /// </remarks>
        private DialogOptions? DialogOptions { get; set; }

        /// <summary>
        /// Gets or sets the list of task completion sources for handling asynchronous dialog operations.
        /// </summary>
        /// <remarks>
        /// This collection manages the pending tasks that are waiting for dialog completion.
        /// Each task corresponds to a dialog service method call that returns a result asynchronously.
        /// </remarks>
        private List<TaskCompletionSource<dynamic>>? CompleteTask { get; set; }

        /// <summary>
        /// Gets or sets the type of the current dialog (Alert, Confirm, or Prompt).
        /// </summary>
        /// <remarks>
        /// This property determines the behavior and button configuration of the dialog.
        /// </remarks>
        private string? DialogType { get; set; }

        /// <summary>
        /// Gets or sets the content/message displayed in the dialog body.
        /// </summary>
        /// <remarks>
        /// This property contains the main message or content shown to the user in the dialog.
        /// </remarks>
        private string? DialogContent { get; set; }

        /// <summary>
        /// Gets or sets the title displayed in the dialog header.
        /// </summary>
        /// <remarks>
        /// This property contains the title text shown in the dialog's header area.
        /// </remarks>
        private string? DialogTitle { get; set; }

        private readonly string _id = $"dialog-{Guid.NewGuid()}";

        /// <summary>
        /// Opens a dialog with the specified options.
        /// </summary>
        /// <param name="options">The dialog configuration options.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>
        /// This method sets the dialog options, makes the dialog visible, and triggers a UI refresh.
        /// </remarks>
        private async Task OpenAsync(DialogOptions options)
        {
            DialogOptions = options;
            IsVisible = true;
            await InvokeAsync(StateHasChanged).ConfigureAwait(false);
        }

        /// <summary>
        /// Handles the dialog close event when the dialog is dismissed through built-in close mechanisms.
        /// </summary>
        /// <param name="args">The event arguments containing close-related information.</param>
        /// <remarks>
        /// For prompt dialogs, returns the input value or an empty string if no input was provided.
        /// For other dialog types (Alert, Confirm), returns false indicating cancellation.
        /// </remarks>
        private Task OnDialogClose(CloseEventArgs args)
        {
            ArgumentNullException.ThrowIfNull(args);
            InputValue ??= string.Empty;
            object result = DialogType == "Prompt" ? InputValue : false;
            return CloseAsync(result);
        }

        /// <summary>
        /// Closes the dialog and returns the specified result to the awaiting task.
        /// </summary>
        /// <param name="result">The result to return to the dialog service method caller. Default is null.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>
        /// This method clears the dialog state, completes the pending task with the provided result,
        /// and triggers a UI refresh to hide the dialog.
        /// </remarks>
        private Task CloseAsync(dynamic? result = null)
        {
            InputValue = string.Empty;
            DialogOptions = null;
            TaskCompletionSource<dynamic>? task = CompleteTask?.LastOrDefault();
            if (task is not null && task.Task is not null && !task.Task.IsCompleted)
            {
                CompleteTask?.Remove(task);
                task.SetResult(result);
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Releases all resources used by the <see cref="SfDialogProvider"/> component.
        /// </summary>
        /// <remarks>
        /// This method unsubscribes from the <see cref="SfDialogService.OnOpen"/> event to prevent memory leaks.
        /// It is automatically called when the component is disposed by the Blazor framework.
        /// </remarks>
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the component and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">If <c>true</c>, release both managed and unmanaged resources; otherwise release only unmanaged resources.</param>
        /// <exclude />
        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (Service is not null)
                    {
                        Service.OnOpen -= OnOpen;
                    }
                    if (DialogOptions is not null)
                    {
                        DialogOptions.ChildContent = default!;
                        DialogOptions.PrimaryButtonOptions = default!;
                        DialogOptions.CancelButtonOptions = default!;
                        DialogOptions.AnimationSettings = default!;
                        DialogOptions.Position = default!;
                    }
                    DialogOptions = null;
                    DialogType = null;
                    DialogContent = null;
                    DialogTitle = null;
                    InputValue = null;
                    CompleteTask = null;
                    Service = null;
                    IsVisible = false;
                }
            }
            catch (InvalidOperationException)
            {
                // Ensure Dispose never throws.
            }
        }

        /// <summary>
        /// Initializes the component and subscribes to the dialog service events.
        /// </summary>
        /// <remarks>
        /// This method is called once when the component is first initialized.
        /// It subscribes to the <see cref="SfDialogService.OnOpen"/> event to handle dialog requests from the service.
        /// </remarks>
        /// <exclude />
        protected override void OnInitialized()
        {
            if (Service is not null)
            {
                Service.OnOpen += OnOpen;
            }
            else
            {
                Service = null;
            }
        }

        /// <summary>
        /// Handles the dialog open request from the <see cref="SfDialogService"/>.
        /// </summary>
        /// <param name="type">The type of dialog to open (Alert, Confirm, or Prompt).</param>
        /// <param name="options">The dialog configuration options.</param>
        /// <param name="content">The content/message to display in the dialog.</param>
        /// <param name="title">The title of the dialog.</param>
        /// <param name="tasks">The list of completion tasks associated with this dialog.</param>
        /// <remarks>
        /// This method is invoked when the <see cref="SfDialogService"/> triggers the OnOpen event.
        /// It stores the dialog parameters and initiates the dialog opening process.
        /// Note: This method uses fire-and-forget because it's subscribed to a .NET Action event
        /// which requires void return. Exceptions are logged via ILogger when available, otherwise
        /// they propagate to prevent silent failures that could mask runtime issues.
        /// </remarks>
        private async void OnOpen(string type, DialogOptions options, string content, string? title, List<TaskCompletionSource<dynamic>> tasks)
        {
            CompleteTask = tasks;
            DialogType = type;
            DialogContent = content;
            DialogTitle = title;
            try
            {
                await OpenAsync(options).ConfigureAwait(false);
            }
            catch (Exception ex) when (Logger is not null)
            {
                Logger.LogError(ex, "Error in OnOpen");
            }
            catch (Exception)
            {
                // If logger is unavailable, re-throw to prevent silent failure in fire-and-forget context
                throw;
            }
        }

        /// <summary>
        /// Handles the OK button click event for the dialog.
        /// </summary>
        /// <remarks>
        /// For prompt dialogs, returns the input value (or empty string if null).
        /// For confirm and alert dialogs, returns true indicating acceptance/acknowledgment.
        /// </remarks>
        private async Task OnOkButtonClickAsync()
        {
            InputValue ??= string.Empty;
            object result = DialogType == "Prompt" ? InputValue : true;
            await CloseAsync(result).ConfigureAwait(false);
        }

        /// <summary>
        /// Handles the Cancel button click event for the dialog.
        /// </summary>
        /// <param name="args">The mouse event arguments from the button click.</param>
        /// <remarks>
        /// For prompt dialogs, returns the default value (typically null).
        /// For confirm dialogs, returns false indicating cancellation/rejection.
        /// For alert dialogs, returns false (though alerts typically only have an OK button).
        /// </remarks>
        private Task OnCancelButtonClick(MouseEventArgs args)
        {
            object? result = DialogType == "Prompt" ? default : (object)false;
            return CloseAsync(result);
        }
    }
}
