using System.ComponentModel;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Popups
{
    /// <summary>
    /// A class that represents the dialog utility service which is used to configure and display built-in dialog boxes such as alert, confirmation, and prompt dialogs.
    /// </summary>
    /// <remarks>
    /// <see cref="SfDialogService"/> can be injected using <c>@inject SfDialogService DialogService</c> in any page to show the built-in dialogs. 
    /// This service provides methods to display common dialog types with customizable options and handles user interactions asynchronously.
    /// </remarks>
    /// <example>
    /// Dialog service must be configured in the <c>Program.cs</c> file for Blazor WASM App, .NET 6 Blazor Server App and 
    /// <c>Startup.cs</c> file for .NET 5 and lower version Blazor Server App.
    /// In <c>Program.cs</c> dialog service can be configured like the below code:
    /// <code><![CDATA[
    /// using Syncfusion.Blazor.Toolkit.Popups;
    /// . . .
    /// builder.Services.AddScoped<SfDialogService>();
    /// ]]></code>
    /// In <c>Startup.cs</c> dialog service can be configured like the below code:
    /// <code><![CDATA[
    /// using Syncfusion.Blazor.Toolkit.Popups;
    /// . . .
    /// public void ConfigureServices(IServiceCollection services) 
    /// {
    ///     . . .
    ///     services.AddScoped<SfDialogService>();
    /// }
    /// ]]></code>
    /// </example>
    public class SfDialogService
    {
        /// <summary>
        /// Gets or sets the event callback that will be invoked before the dialog opens and closes.
        /// </summary>
        /// <remarks>
        /// Internal purpose.
        /// </remarks>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public event Action<string, DialogOptions, string, string?, List<TaskCompletionSource<dynamic>>>? OnOpen;

        internal List<TaskCompletionSource<dynamic>> _tasks = [];

        private Task<dynamic> OpenAsync(string type, DialogOptions? options, string content, string? title)
        {
            string? dialogTitle = title;
            string dialogContent = content ?? string.Empty;
            TaskCompletionSource<dynamic> task = new();
            _tasks.Add(task);
            OnOpen?.Invoke(type, GetDialogOptions(type, options), dialogContent, dialogTitle, _tasks);
            return task.Task;
        }

        private static DialogOptions GetDialogOptions(string type, DialogOptions? options)
        {
            DialogOptions dialogOption = new()
            {
                Height = options is not null && !string.IsNullOrEmpty(options.Height) ? options.Height : "auto",
                Width = options is not null && !string.IsNullOrEmpty(options.Width) ? options.Width : "auto",
                CssClass = options is not null && !string.IsNullOrEmpty(options.CssClass) ? options.CssClass : string.Empty,
                ZIndex = options is not null ? options.ZIndex : 1000,
                AllowDragging = options is not null && options.AllowDragging,
                ShowCloseIcon = options is not null && options.ShowCloseIcon,
                CloseOnEscape = options is null || options.CloseOnEscape,
                Position = options is not null && options.Position is not null ? options.Position : new PositionDataModel() { X = "center", Y = "center" },
                PrimaryButtonOptions = options is not null && options.PrimaryButtonOptions is not null ? options.PrimaryButtonOptions : new DialogButtonOptions(),
                AnimationSettings = options is not null && options.AnimationSettings is not null ? options.AnimationSettings : default!,
                ChildContent = options is not null && options.ChildContent is not null ? options.ChildContent : default!
            };
            if (type != "Alert")
            {
                dialogOption.CancelButtonOptions = options is not null && options.CancelButtonOptions is not null ? options.CancelButtonOptions : new DialogButtonOptions();
            }
            dialogOption.CssClass += !string.IsNullOrEmpty(dialogOption.CssClass) ? " " : "";
            dialogOption.CssClass += "e-" + type.ToLower(CultureInfo.CurrentCulture) + "-dialog";
            return dialogOption;
        }

        /// <summary>
        /// Opens a confirmation dialog box with two buttons as <c>OK</c> and <c>Cancel</c>.
        /// </summary>
        /// <param name="content">A string that specifies the text to be displayed in the confirmation dialog.</param>
        /// <param name="title">Optional. A string that specifies the title to be displayed in the confirmation dialog.</param>
        /// <param name="options">Optional. A <see cref="DialogOptions"/> that specifies the options to configure the confirmation dialog.</param>
        /// <returns>
        /// Returns a <see cref="bool"/> value which specifies which button is clicked by the user. <c>true</c> if the user clicked the <c>OK</c> button; otherwise <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This method displays a modal confirmation dialog that requires user interaction. The dialog contains two buttons by default: <c>OK</c> and <c>Cancel</c>.
        /// The method returns asynchronously and will wait for the user to make a selection before continuing execution.
        /// </remarks>
        /// <example>
        /// Code example to show confirmation dialog:
        /// <code><![CDATA[
        /// bool isConfirm = await DialogService.ConfirmAsync("Are you sure you want to permanently delete these items?", "Delete Multiple Items");
        /// string confirmMessage = isConfirm ? "confirmed" : "canceled";
        /// Console.WriteLine($"The user {confirmMessage} the dialog box.");
        /// ]]></code>
        /// </example>
        public async Task<bool> ConfirmAsync(string content, string? title = null, DialogOptions? options = null)
        {
            return await OpenAsync("Confirm", options, content, title).ConfigureAwait(false);
        }

        /// <summary>
        /// Opens an alert dialog box which is used to display warning or informational messages to the users.
        /// </summary>
        /// <param name="content">A string that specifies the text to be displayed in the alert dialog.</param>
        /// <param name="title">Optional. A string that specifies the title to be displayed in the alert dialog.</param>
        /// <param name="options">Optional. A <see cref="DialogOptions"/> that specifies the options to configure the alert dialog.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation that completes when the user closes the dialog.
        /// </returns>
        /// <remarks>
        /// This method displays a modal alert dialog that shows information to the user. The alert dialog typically contains a single <c>OK</c> button for dismissal.
        /// The method returns asynchronously and will wait for the user to acknowledge the message before continuing execution.
        /// </remarks>
        /// <example>
        /// Code example to show alert dialog:
        /// <code><![CDATA[
        /// await DialogService.AlertAsync("Alert Dialog Content");
        /// Console.WriteLine($"The user closed the alert dialog.");
        /// ]]></code>
        /// </example>
        public async Task AlertAsync(string content, string? title = null, DialogOptions? options = null)
        {
            await OpenAsync("Alert", options, content, title).ConfigureAwait(false);
        }

        /// <summary>
        /// Opens a prompt dialog that prompts the user to input text.
        /// </summary>
        /// <param name="content">A string that specifies the text to be displayed in the prompt dialog.</param>
        /// <param name="title">Optional. A string that specifies the title to be displayed in the prompt dialog.</param>
        /// <param name="options">Optional. A <see cref="DialogOptions"/> that specifies the options to configure the prompt dialog.</param>
        /// <returns>
        /// Returns a <see cref="string"/> entered by the user when the user clicks <c>OK</c>; otherwise <c>null</c> when the user clicks <c>Cancel</c>.
        /// </returns>
        /// <remarks>
        /// This method displays a modal prompt dialog that allows the user to enter text input. The dialog contains an input field and typically has <c>OK</c> and <c>Cancel</c> buttons.
        /// The method returns asynchronously and will wait for the user to either provide input or cancel the dialog before continuing execution.
        /// </remarks>
        /// <example>
        /// Code example to show prompt dialog:
        /// <code><![CDATA[
        /// string promptText = await DialogService.PromptAsync("Enter your name:", "Join Chat Group", new DialogOptions()
        /// {
        ///     PrimaryButtonOptions = new DialogButtonOptions { Content = "Okay" },
        ///     CancelButtonOptions = new DialogButtonOptions { Content = "Cancel" }, 
        /// });
        /// if (promptText is null)
        /// {
        ///     Console.WriteLine($"The user canceled the dialog box.");
        /// }
        /// else
        /// {
        ///     Console.WriteLine($"The User's input is returned as \"{promptText}\".");
        /// }
        /// ]]></code>
        /// </example>
        public async Task<string> PromptAsync(string content, string? title = null, DialogOptions? options = null)
        {
            return await OpenAsync("Prompt", options, content, title).ConfigureAwait(false);
        }
    }
}
