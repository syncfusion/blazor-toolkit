using Microsoft.JSInterop;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Internal;
namespace Syncfusion.Blazor.Toolkit.Inputs
{
    /// <summary>
    /// The TextArea is an textarea element that allows to get input from the user. It allows the user to edit or display the text value.
    /// </summary>
    public partial class SfTextArea : SfInputBase<string>
    {
        #region Injected Services
        /// <summary>
        /// Gets the logger instance used for recording component errors.
        /// </summary>
        [Inject]
        private ILogger<SfTextArea>? Logger { get; set; }

        #endregion

        #region Logging
        /// <summary>
        /// LoggerMessage delegate for error processing lifecycle events.
        /// </summary>
        private static readonly Action<ILogger, string, Exception?> _logLifecycleError =
            LoggerMessage.Define<string>(
                LogLevel.Error,
                new EventId(1, "LifecycleError"),
                "Unexpected error in {Method}");

        /// <summary>
        /// LoggerMessage delegate for error processing dispose operations.
        /// </summary>
        private static readonly Action<ILogger, string, Exception> _logDisposeError =
            LoggerMessage.Define<string>(
                LogLevel.Error,
                new EventId(2, "DisposeError"),
                "Unexpected error in DisposeAsyncCore: {ExceptionMessage}");
        #endregion

        /// <summary>
        /// Initializes the TextArea component during its first rendering phase.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous initialization operation.</returns>
        /// <remarks>
        /// This method sets up essential component properties including script modules, resize modes, 
        /// width calculations, CSS classes, and HTML attributes. It also configures accessibility 
        /// attributes and handles initial property validation.
        /// </remarks>
        /// <exclude/>
        /// <inheritdoc/>
        protected override async Task OnInitializedAsync()
        {
            try
            {
                await base.OnInitializedAsync().ConfigureAwait(true);

                // Initialize internal properties and state tracking.
                InitializeProps();

                // Configure resize and width behavior.
                SetResizeMode();
                SetWidth();

                // Apply consumer-defined CSS classes.
                SetCssClass();

                // Update HTML attributes (rows, cols, maxlength, etc.).
                UpdateInputAttributes();
            }
            catch (Exception ex)
            {
                if (Logger is not null)
                {
                    _logLifecycleError(Logger, ex.Message, ex);
                }
                throw;
            }
        }

        /// <summary>
        /// Handles parameter updates and property changes when the component receives new parameters.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous parameter update operation.</returns>
        /// <remarks>
        /// This method is called whenever the component's parameters are updated. It processes property changes,
        /// updates the component state accordingly, and ensures validation classes are properly applied.
        /// </remarks>
        /// <inheritdoc/>
        protected override async Task OnParametersSetAsync()
        {
            try
            {
                PropertyUpdate();
                if (PropertyChanges?.Count > 0)
                {
                    await OnPropertyChangeAsync(PropertyChanges).ConfigureAwait(true);
                }
                await base.OnParametersSetAsync().ConfigureAwait(true);
                if (!InputHtmlAttributes.ContainsKey(ARIA_LABEL))
                {
                    InputHtmlAttributes = SfBaseUtils.UpdateDictionary(ARIA_LABEL, "textarea", InputHtmlAttributes);
                }
                UpdateValidationClass();
            }
            catch (Exception ex)
            {
                if (Logger is not null)
                {
                    _logLifecycleError(Logger, ex.Message, ex);
                }
                throw;
            }
        }

        /// <summary>
        /// Imports the JavaScript module required by this component and caches the reference
        /// for subsequent interop calls. When the hosting environment supports in‑process
        /// interop (typically Blazor Server with <see cref="IJSInProcessRuntime"/>),
        /// the method loads and stores an <see cref="IJSInProcessObjectReference"/>; otherwise,
        /// it loads and stores a standard asynchronous <see cref="IJSObjectReference"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> that represents the asynchronous import operation.
        /// The task completes when the underlying JS module has been imported and the
        /// appropriate reference field has been assigned.
        /// </returns>
        internal override async Task ImportComponentModuleAsync()
        {
            await base.ImportComponentModuleAsync().ConfigureAwait(true);

            JsModuleReference textAreaJsModuleReference = await ImportModuleAsync("./_content/Syncfusion.Blazor.Toolkit/scripts/textarea.js", _textAreaJsModule, _textAreaJsInProcessModule).ConfigureAwait(true);
            _textAreaJsModule = textAreaJsModuleReference.AsyncRef;
            _textAreaJsInProcessModule = textAreaJsModuleReference.InProcessRef;
        }

        /// <summary>
        /// Handles post-render operations and initializes component functionality after rendering.
        /// </summary>
        /// <param name="firstRender">A boolean value indicating whether this is the component's first render cycle.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous post-render operation.</returns>
        /// <remarks>
        /// On first render, this method handles persistence restoration, event initialization, and outline width calculations.
        /// It also triggers the Created event to notify when the component has been fully initialized.
        /// </remarks>
        /// <exclude/>
        /// <inheritdoc/>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                await base.OnAfterRenderAsync(firstRender).ConfigureAwait(true);
                if (firstRender)
                {
                    await RestorePersistedValueAsync().ConfigureAwait(true);
                    _previousValue = Value;
                    await InvokeVoidAsync(_textAreaJsModule, _textAreaJsInProcessModule, "initialize", [DataId ?? string.Empty, InputElement, ContainerElement]).ConfigureAwait(true);
                    if (Created.HasDelegate)
                    {
                        await Created.InvokeAsync(null).ConfigureAwait(true);
                    }
                }
                if (ContainerClass.Contains(OUTLINE, StringComparison.Ordinal))
                {
                    await InvokeVoidAsync(_textAreaJsModule, _textAreaJsInProcessModule, "calculateWidth", [DataId ?? string.Empty, InputElement]).ConfigureAwait(true);
                }
            }
            catch (Exception ex)
            {
                if (Logger is not null)
                {
                    _logLifecycleError(Logger, ex.Message, ex);
                }
                throw;
            }
        }

        /// <summary>
        /// Performs cleanup operations when the TextArea component is being disposed.
        /// </summary>
        /// <remarks>
        /// This method is called internally during component disposal to clean up resources and notify
        /// event handlers. It checks if the component has been properly rendered before invoking the
        /// Destroyed event, ensuring proper cleanup sequencing and preventing potential memory leaks.
        /// The method is part of the component lifecycle and should not be called directly.
        /// </remarks>
        protected override async ValueTask DisposeAsyncCore()
        {
            try
            {
                if (IsRendered)
                {
                    if (Destroyed.HasDelegate)
                    {
                        await Destroyed.InvokeAsync(null).ConfigureAwait(true);
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                if (Logger is not null)
                {
                    _logDisposeError(Logger, ex.Message, ex);
                }
            }
            catch (OperationCanceledException ex)
            {
                if (Logger is not null)
                {
                    _logDisposeError(Logger, ex.Message, ex);
                }
            }

            try
            {
                if (_textAreaJsModule is not null)
                {
                    await _textAreaJsModule.DisposeAsync().ConfigureAwait(true);
                }

                _textAreaJsInProcessModule?.Dispose();
            }
            catch (JSDisconnectedException)
            {
                // Ignore: The circuit disconnected (e.g., page reload) before JS disposal could complete.
            }
            await base.DisposeAsyncCore().ConfigureAwait(true);
        }
    }
}