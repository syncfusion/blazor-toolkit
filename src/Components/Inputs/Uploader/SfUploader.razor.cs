using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Toolkit.Inputs.Internal;
using Syncfusion.Blazor.Toolkit.Internal;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    /// <summary>
    /// Represents the SfUploader component that provides functionality for uploading files to a server.
    /// This component is an extended version of HTML5 file upload with support for multiple file selection,
    /// automatic upload, drag and drop operations, progress indicators, preloaded files, and file validation.
    /// It supports both synchronous and asynchronous upload operations with comprehensive event handling
    /// and customizable UI templates.
    /// </summary>
    public partial class SfUploader : SfBaseComponent
    {
        internal IJSObjectReference? _uploaderJsModule;
        internal IJSInProcessObjectReference? _uploaderJsInProcessModule;
        private IJSObjectReference? _sanitizeJsModule;
        private IJSInProcessObjectReference? _sanitizeJsInProcessModule;
        private IJSObjectReference? _ajaxJsModule;
        private IJSInProcessObjectReference? _ajaxJsInProcessModule;

        private List<string> _containerAttributes = ["title", "style", "class"];

        private Dictionary<string, object> _containerAttr = [];

        private Dictionary<string, object> _browseBtnAttr = [];

        private Dictionary<string, object> _actionBtnAttr = [];

        private Dictionary<string, object> _inputAttr = [];

        private Dictionary<string, object> _progressBarAttr = [];

        private SemaphoreSlim FileSemaphore { get; } = new SemaphoreSlim(1);

        private List<InputFileChangeEventArgs> _inputFiles = [];

        private bool EnableUploadButton { get; set; }

        private bool _isActionButtonsHidden = true;

        /// <summary>
        /// Asynchronously invokes the StateHasChanged method to notify the component that its state has changed.
        /// This method is called internally to refresh the component's rendering when state changes occur.
        /// </summary>
        /// <returns>A task that represents the asynchronous state change notification operation.</returns>
        internal async Task CallStateHasChangedAsync()
        {
            await InvokeAsync(StateHasChanged).ConfigureAwait(true);
        }

        private string? ContainerClass { get; set; }

        private string RemoveIconDisable { get; set; } = string.Empty;

        private bool IsClearAll { get; set; }

        private bool IsShowRemoveIcon { get; set; }

        private string? BrowseBtnContent { get; set; }

        private string? InputContainer { get; set; }

        private string? DropAreaContainer { get; set; }

        private string? FileDropAreaContent { get; set; }

        /// <summary>
        /// Reference to the upload button element for programmatic focus and interaction.
        /// </summary>
        private ElementReference? ButtonElement { get; set; }

        private bool IsShowProgressBar { get; set; }

        [Inject]
        private IStringLocalizer Localizer { get; set; } = default!;

        private string BtnTabIndex { get; set; } = "0";

        internal List<UploadFileDetails> FileData { get; set; } = [];

        private List<UploadFileDetails> UploadedFileData { get; set; } = [];

        private List<FileInfo> UploadedFilesInfo { get; set; } = [];

        /// <summary>
        /// Reference to the InputFile element that represents the uploaded file.
        /// This is initialized during component rendering and should not be null after OnAfterRender.
        /// </summary>
        private InputFile? FileElement { get; set; }

        /// <summary>
        /// Event arguments received when the input file element changes.
        /// Contains information about the selected files.
        /// </summary>
        private InputFileChangeEventArgs? InputFileChangeEvent { get; set; }

        private string? FileListClass { get; set; }

        private string? FileListStatus { get; set; }

        private string? FileListStatusName { get; set; }

        private string? UploadStatus { get; set; }

        /// <summary>
        /// Reference to the action button container element for JavaScript interop.
        /// </summary>
        private ElementReference? ActionButtonRef { get; set; }

        /// <summary>
        /// Reference to the file list UL element for JavaScript interop and manipulation.
        /// </summary>
        private ElementReference? UlElementRef { get; set; }

        internal int BufferSize { get; set; } = DEFAULT_BUFFER_SIZE_BYTES;

        private int FileIndex { get; set; }

        private long ChunkIndex { get; set; }

        private long TotalChunk { get; set; }

        private FileInfo? FileInfo { get; set; }

        private long ProgressValue { get; set; }

        private bool IsForm { get; set; }

        private bool IsDevice { get; set; }

        private string DataId { get; set; } = string.Empty;

        /// <summary>
        /// Gets a value indicating whether the file list should be rendered based on component state.
        /// </summary>
        /// <remarks>
        /// The file list is rendered when all of the following conditions are met:
        /// - ShowFileList is enabled
        /// - FileData collection exists and contains items
        /// - IsClearAll is false
        /// - Either no SaveUrl is configured OR a custom Template is provided
        /// </remarks>
        private bool ShouldRenderFileList =>
            ShowFileList &&
            FileData?.Count > 0 &&
            !IsClearAll &&
            (string.IsNullOrEmpty(UploadAsyncSettings?.SaveUrl) || Template != null);

        /// <summary>
        /// Executes initialization logic after the JavaScript scripts have been rendered for the uploader component.
        /// This method initializes the client-side JavaScript component, configures device detection,
        /// and renders any preloaded files.
        /// </summary>
        /// <returns>A task that represents the asynchronous script rendering operation.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during JavaScript initialization or preload rendering.</exception>
        internal override async Task OnAfterScriptRenderedAsync()
        {
            try
            {
                Dictionary<string, object> options = GetProperty();
                await UpdateIsDeviceModeAsync().ConfigureAwait(true);
                await InvokeVoidAsync(_uploaderJsModule!, _uploaderJsInProcessModule!, "initialize", [DataId, FileElement!, DotnetObjectReference!, options]).ConfigureAwait(true);
                IsDevice = SyncfusionService != null && SyncfusionService.IsDeviceMode;
                await RenderPreloadFilesAsync().ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unhandled exception occurred while processing files.", ex);
            }
        }

        /// <summary>
        /// Retrieves custom HTTP headers from the HttpClient instance for use in upload requests.
        /// The headers are converted to dynamic objects that can be serialized and passed to JavaScript.
        /// </summary>
        /// <returns>An array of dynamic objects representing the custom HTTP headers, or an empty array if no headers are configured.</returns>
        private object[] GetCustomHeaders()
        {
            List<object> customHeaders = [];
            List<KeyValuePair<string, IEnumerable<string>>>? headers = HttpClientInstance?.DefaultRequestHeaders?.ToList();
            if (headers != null)
            {
                foreach (KeyValuePair<string, IEnumerable<string>> header in headers)
                {
                    dynamic customObject = new ExpandoObject();
                    ((IDictionary<string, object>)customObject)[header.Key] = header.Value;
                    customHeaders.Add(customObject);
                }
            }
            return [.. customHeaders];
        }

        /// <summary>
        /// Performs pre-rendering setup for the uploader component by configuring input attributes,
        /// container classes, and various component properties.
        /// This method is called during the component initialization phase.
        /// </summary>
        private void PreRender()
        {
            _inputAttr = SfBaseUtils.UpdateDictionary("class", "e-control" + SPACE + ROOT + SPACE + "e-lib", _inputAttr);
            _inputAttr = SfBaseUtils.UpdateDictionary("name", ID?.ToString()!, _inputAttr);
            ContainerClass = CONTROL_CONTAINER + SPACE + "e-lib e-keyboard";
            SetEnabled();
            SetCssClass();
            UpdateHTMLAttributes();
            UpdateInputAttributes();
            UpdateDirectoryAttr();
        }

        /// <summary>
        /// Updates the input element attributes with any custom attributes specified through the InputAttributes property.
        /// This method merges user-defined attributes with the component's default input attributes.
        /// </summary>
        private void UpdateInputAttributes()
        {
            if (InputAttributes != null && InputAttributes.Count > 0)
            {
                foreach (KeyValuePair<string, object> attr in InputAttributes)
                {
                    _ = SfBaseUtils.UpdateDictionary(attr.Key, attr.Value, _inputAttr);
                }
            }
        }

        /// <summary>
        /// Creates a configuration dictionary for JavaScript interop initialization.
        /// </summary>
        /// <returns>
        /// A dictionary containing all required configuration for the client-side component,
        /// including properties, event handler flags, localization, and HTTP headers.
        /// </returns>
        /// <remarks>
        /// This method consolidates approximately 40 configuration entries into a single object
        /// that is passed to the JavaScript initialization function. Pre-allocates dictionary capacity
        /// for optimal performance.
        /// </remarks>
        internal Dictionary<string, object> GetProperty()
        {
            // Pre-allocate capacity: ~15 basic props + ~18 event flags + ~2 locale/headers
            Dictionary<string, object> properties = new Dictionary<string, object>(capacity: 40);
            AddBasicProperties(properties);
            AddEventHandlerFlags(properties);
            AddLocalizationAndHeaders(properties);
            return properties;
        }

        /// <summary>
        /// Adds basic component properties to the configuration dictionary.
        /// </summary>
        /// <param name="properties">The dictionary to add properties to.</param>
        private void AddBasicProperties(Dictionary<string, object> properties)
        {
            string? template = (Template != null) ? "Blazor Template" : null;
            properties[ALLOW_EXTENSIONS] = AllowedExtensions;
            properties[ENABLE_HTML_SANITIZER] = EnableHtmlSanitizer;
            properties[ASYNC_SETTING] = UploadAsyncSettings!;
            properties[AUTO_UPLOAD] = AutoUpload;
            properties[BUTTON] = UploadButtons!;
            properties[CSSCLASS] = CssClass;
            properties[DROPAREA] = DropArea!;
            properties[DIRECTORY_UPLOAD] = DirectoryUpload;
            properties[DROP_EFFECT] = DropEffect.ToString();
            properties[DISABLED] = Disabled;
            properties[PERSISTENCE] = EnablePersistence;
            properties[UPLOAD_FILES] = UploadedFiles!;
            properties[MAX_FILE_SIZE] = MaxFileSize;
            properties[MIN_FILE_SIZE] = MinFileSize;
            properties[SHOW_FILE_LIST] = ShowFileList;
            properties[UPLOAD_MULTIPLE] = AllowMultiple;
            properties[SEQUENTIAL_UPLOAD] = SequentialUpload && !string.IsNullOrEmpty(_asyncSettings?.SaveUrl);
            properties[UPLOAD_TEMPLATE] = template!;
        }

        /// <summary>
        /// Adds event handler availability flags to the configuration dictionary.
        /// </summary>
        /// <param name="properties">The dictionary to add event flags to.</param>
        private void AddEventHandlerFlags(Dictionary<string, object> properties)
        {
            properties[ACTION_COMPLETE_ENABLED] = OnActionComplete.HasDelegate;
            properties[BEFORE_REMOVE_ENABLED] = BeforeRemove.HasDelegate;
            properties[BEFORE_UPLOAD_ENABLED] = BeforeUpload.HasDelegate;
            properties[CANCEL_ENABLED] = OnCancel.HasDelegate;
            properties[CHANGE_ENABLED] = OnValueChange.HasDelegate;
            properties[CHUNK_FAILURE_ENABLED] = OnChunkFailure.HasDelegate;
            properties[CHUNK_UPLOADING_ENABLED] = OnChunkUploadStart.HasDelegate;
            properties[UPLOADING_ENABLED] = OnUploadStart.HasDelegate;
            properties[CLEAR_ENABLED] = OnClear.HasDelegate;
            properties[FAILURE_ENABLED] = OnFailure.HasDelegate;
            properties[FILE_RENDER_ENABLED] = OnFileListRender.HasDelegate;
            properties[PAUSED_ENABLED] = Paused.HasDelegate;
            properties[PROGRESSING_ENABLED] = Progressing.HasDelegate;
            properties[REMOVING_ENABLED] = OnRemove.HasDelegate;
            properties[RESUME_ENABLED] = OnResume.HasDelegate;
            properties[SELECTED_ENABLED] = FileSelected.HasDelegate;
            properties[SUCCESS_ENABLED] = Success.HasDelegate;
            properties[CHUNK_SUCCESS_ENABLED] = OnChunkSuccess.HasDelegate;
        }

        /// <summary>
        /// Adds localization text and HTTP headers to the configuration dictionary.
        /// </summary>
        /// <param name="properties">The dictionary to add localization and headers to.</param>
        private void AddLocalizationAndHeaders(Dictionary<string, object> properties)
        {
            properties[LOCALE_TEXT] = GetLocaleText();
            properties[HTTPCLIENTHEADERS] = GetCustomHeaders();
        }

        /// <summary>
        /// Creates and returns a dictionary containing localized text values for the uploader component UI elements.
        /// This method retrieves localized strings from the localizer service and provides fallback values
        /// for all user-facing text elements in the component.
        /// </summary>
        /// <returns>A dictionary containing localized text values with keys corresponding to UI text elements and values as the localized strings.</returns>
        private Dictionary<string, string> GetLocaleText()
        {
            Dictionary<string, string> localeText = new()
            {
                { "browse", Localizer[BROWSE_KEY] },
                { "abort", Localizer[ABORT_KEY] },
                { "cancel", Localizer[CANCEL_KEY] },
                { "clear", Localizer[CLEAR_KEY] },
                { "delete", Localizer[DELETE_KEY] },
                { "dropFilesHint", Localizer[DROP_FILE_KEY] },
                { "fileUploadCancel", Localizer[FILE_UPLOAD_CANCEL] },
                { "inProgress", Localizer[INPROGRESS_KEY] },
                { "invalidFileType", Localizer[INVALID_FILE_KEY] },
                { "invalidFileName", Localizer[INVALID_FILE_NAME] },
                { "invalidMaxFileSize", Localizer[INVALID_MAX_FILE_KEY] },
                { "invalidMinFileSize", Localizer[INVALID_MIN_FILE_KEY] },
                { "pause", Localizer[PAUSE_KEY] },
                { "pauseUpload", Localizer[PAUSE_UPLOAD_KEY] },
                { "readyToUploadMessage", Localizer[READY_UPLOAD_KEY] },
                { "remove", Localizer[REMOVE_KEY] },
                { "removedFailedMessage", Localizer[REMOVED_FAILED_KEY] },
                { "removedSuccessMessage", Localizer[REMOVED_SUCCESS_KEY] },
                { "resume", Localizer[RESUME_KEY] },
                { "retry", Localizer[RETRY_KEY] },
                { "upload", Localizer[UPLOAD_KEY] },
                { "uploadFailedMessage", Localizer[UPLOAD_FAILED_KEY] },
                { "uploadSuccessMessage", Localizer[UPLOAD_SUCCESS_KEY] }
            };
            return localeText;
        }

        /// <summary>
        /// Handles the change event for the input file element when files are selected.
        /// This method stores the file change event arguments for processing and maintains
        /// a collection of all file inputs for later use in upload operations.
        /// </summary>
        /// <param name="args">The event arguments containing information about the selected files.</param>
        private void OnChange(InputFileChangeEventArgs args)
        {
            _inputFiles.Add(args);
            InputFileChangeEvent = args;
        }

        /// <summary>
        /// Applies the custom CSS class to the container element if specified.
        /// This method merges the user-provided CSS class with the component's default container classes.
        /// </summary>
        private void SetCssClass()
        {
            if (!string.IsNullOrEmpty(CssClass))
            {
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, CssClass);
            }
        }

        /// <summary>
        /// Configures the enabled/disabled state of the uploader component by updating CSS classes and input attributes.
        /// When disabled, adds the disabled class and aria attributes for accessibility.
        /// When enabled, removes these attributes and classes.
        /// </summary>
        private void SetEnabled()
        {
            if (Disabled)
            {
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, DISABLED_CLASS);
                _inputAttr = SfBaseUtils.UpdateDictionary("disabled", "disabled", _inputAttr);
                _inputAttr = SfBaseUtils.UpdateDictionary("aria-disabled", TRUE, _inputAttr);
            }
            else
            {
                ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, DISABLED_CLASS);
                _ = _inputAttr.Remove("disabled");
                _ = _inputAttr.Remove("aria-disabled");
            }
        }

        /// <summary>
        /// Updates the input element attributes to enable or disable directory upload functionality.
        /// When directory upload is enabled, adds the 'directory' and 'webkitdirectory' attributes
        /// to allow folder selection instead of individual files.
        /// </summary>
        private void UpdateDirectoryAttr()
        {
            if (DirectoryUpload)
            {
                _inputAttr = SfBaseUtils.UpdateDictionary("directory", TRUE, _inputAttr);
                _inputAttr = SfBaseUtils.UpdateDictionary("webkitdirectory", TRUE, _inputAttr);
            }
            else
            {
                _ = _inputAttr.Remove("directory");
                _ = _inputAttr.Remove("webkitdirectory");
            }
        }

        /// <summary>
        /// Performs the main rendering operations for the uploader component by configuring
        /// various UI elements and settings including browse button, upload initialization,
        /// multiple file selection, file extensions, and RTL support.
        /// </summary>
        private void Render()
        {
            UpdateBrowsBtn();
            InitializeUpload();
            SetMultipleSelection();
            SetExtensions();
            SetRTL();
        }

        /// <summary>
        /// Updates the browse button attributes including tab index, content text, title, and accessibility labels.
        /// The button content is set to either a localized value or custom text specified in UploadButtons.
        /// Accessibility attributes are also configured for screen readers.
        /// </summary>
        private void UpdateBrowsBtn()
        {
            _browseBtnAttr = SfBaseUtils.UpdateDictionary("tabindex", TabIndex, _browseBtnAttr);
            // Get localized browse text with fallback
            LocalizedString browseLocaleVal = Localizer[BROWSE_KEY];
            // Determine button content with defensive null checks
            BrowseBtnContent = UploadButtons == null || string.IsNullOrEmpty(UploadButtons.Browse) || UploadButtons.Browse == "Browse..."
                ? (string)browseLocaleVal
                : UploadButtons.Browse;

            // Set button attributes with sanitized content
            _browseBtnAttr = SfBaseUtils.UpdateDictionary("title", BrowseBtnContent, _browseBtnAttr);
            _browseBtnAttr = SfBaseUtils.UpdateDictionary("aria-label", "Browse for file to upload", _browseBtnAttr);
        }

        /// <summary>
        /// Initializes the upload functionality by configuring input attributes, container classes,
        /// and drop area content with appropriate localized text.
        /// The input element is set to tab index -1 to prevent direct focus.
        /// </summary>
        private void InitializeUpload()
        {
            _inputAttr = SfBaseUtils.UpdateDictionary("tabindex", "-1", _inputAttr);
            InputContainer = INPUT_CONTAINER;
            DropAreaContainer = DROP_CONTAINER;
            FileDropAreaContent = Localizer[DROP_FILE_KEY];
        }

        /// <summary>
        /// Configures the input element to allow multiple file selection based on the AllowMultiple property.
        /// When enabled, adds the 'multiple' attribute to the input element to allow selecting multiple files at once.
        /// When disabled, removes the 'multiple' attribute to restrict selection to single files.
        /// </summary>
        private void SetMultipleSelection()
        {
            if (AllowMultiple)
            {
                _inputAttr = SfBaseUtils.UpdateDictionary("multiple", "multiple", _inputAttr);
            }
            else
            {
                _ = _inputAttr.Remove("multiple");
            }
        }

        /// <summary>
        /// Configures the input element's 'accept' attribute based on the AllowedExtensions property.
        /// This restricts the file picker to only show files with the specified extensions.
        /// When no extensions are specified, removes the 'accept' attribute to allow all file types.
        /// </summary>
        private void SetExtensions()
        {
            if (!string.IsNullOrEmpty(AllowedExtensions))
            {
                _inputAttr = SfBaseUtils.UpdateDictionary("accept", AllowedExtensions, _inputAttr);
            }
            else
            {
                _ = _inputAttr.Remove("accept");
            }
        }

        /// <summary>
        /// Configures right-to-left (RTL) text direction support for the uploader component.
        /// Adds or removes the RTL CSS class based on the EnableRtl property or global RTL setting.
        /// This affects the layout and text direction of the entire component.
        /// </summary>
        private void SetRTL()
        {
            ContainerClass = SyncfusionService!._options.EnableRtl ? SfBaseUtils.AddClass(ContainerClass!, RTL) : SfBaseUtils.RemoveClass(ContainerClass!, RTL);
        }

        /// <summary>
        /// Processes HTML attributes from the HtmlAttributes property and applies them to appropriate elements.
        /// Container-specific attributes (title, style, class) are applied to the container element,
        /// while other attributes are applied to the input element.
        /// The 'class' attribute is specially handled by merging with existing container classes.
        /// </summary>
        private void UpdateHTMLAttributes()
        {
            if (HtmlAttributes != null)
            {
                foreach (KeyValuePair<string, object> item in HtmlAttributes)
                {
                    if (_containerAttributes.IndexOf(item.Key) < 0)
                    {
                        _inputAttr = SfBaseUtils.UpdateDictionary(item.Key, item.Value!, _inputAttr);
                    }
                    else
                    {
                        if (item.Key == "class")
                        {
                            ContainerClass = SfBaseUtils.AddClass(ContainerClass!, item.Value?.ToString()!);
                        }
                        else
                        {
                            _containerAttr = SfBaseUtils.UpdateDictionary(item.Key, item.Value!, _containerAttr);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles all property changes when the component is re-rendered.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task HandlePropertyChangesAsync()
        {
            UpdateDirectoryAttr();
            SetRTL();
            SetExtensions();
            await HandleCssClassChangeAsync().ConfigureAwait(true);
            await HandleUploadedFilesChangeAsync().ConfigureAwait(true);
            SetEnabled();
            UpdatePrivateProperty();
            await SyncPropertiesToJavaScriptAsync().ConfigureAwait(true);
        }

        /// <summary>
        /// Handles CSS class changes and updates the container styling.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task HandleCssClassChangeAsync()
        {
            if (IsPropertyChanged() && !string.Equals(CssClass, _cssClass, StringComparison.Ordinal))
            {
                ContainerClass = string.IsNullOrEmpty(ContainerClass)
                    ? ContainerClass
                    : SfBaseUtils.RemoveClass(ContainerClass, _cssClass);
                _cssClass = CssClass;
                SetCssClass();
            }
            await Task.CompletedTask.ConfigureAwait(true);
        }

        /// <summary>
        /// Handles changes to the uploaded files collection and re-renders preloaded files.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task HandleUploadedFilesChangeAsync()
        {
            if (!SfBaseUtils.Equals(UploadedFiles, _files))
            {
                await RenderPreloadFilesAsync().ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Synchronizes component properties to JavaScript for client-side functionality.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task SyncPropertiesToJavaScriptAsync()
        {
            Dictionary<string, object> options = GetProperty();
            await InvokeVoidAsync(_uploaderJsModule, _uploaderJsInProcessModule, "propertyChanges",
                [DataId, options, PropertyChanges!]).ConfigureAwait(true);
        }

        /// <summary>
        /// Updates HTML attributes and input attributes for the component.
        /// </summary>
        private void UpdateAttributesAndInputs()
        {
            UpdateHTMLAttributes();
            UpdateInputAttributes();
        }

        /// <summary>
        /// Initializes the upload session with default values.
        /// </summary>
        private void InitializeUploadSession()
        {
            FileIndex = 0;
            ProgressValue = 0;
            IsShowRemoveIcon = false;
            _progressBarAttr = new Dictionary<string, object>
            {
                {"style", "width: 0%;" }
            };
        }

        /// <summary>
        /// Validates if a file is ready for upload based on its status and validation messages.
        /// </summary>
        /// <param name="fileInfo">The file information to validate.</param>
        /// <returns>True if the file is valid for upload; otherwise, false.</returns>
        private bool IsValidFileForUpload(FileInfo fileInfo)
        {
            if (fileInfo == null)
            {
                return false;
            }

            LocalizedString readyMsgLocaleVal = Localizer[READY_UPLOAD_KEY];
            return (fileInfo.Status == readyMsgLocaleVal || fileInfo.Status == "Uploading")
                && string.IsNullOrEmpty(fileInfo.ValidationMessages?.MaxSize)
                && string.IsNullOrEmpty(fileInfo.ValidationMessages?.MinSize);
        }

        /// <summary>
        /// Retrieves the browser file corresponding to the file info from the input file change event.
        /// </summary>
        /// <param name="fileInfo">The file information to match.</param>
        /// <param name="index">The current file index.</param>
        /// <param name="emptyFileCount">The count of empty files encountered.</param>
        /// <returns>The matching IBrowserFile or null if not found.</returns>
        private IBrowserFile? GetBrowserFileForUpload(FileInfo fileInfo, int index, int emptyFileCount)
        {
            if (InputFileChangeEvent == null || fileInfo == null)
            {
                return null;
            }

            IReadOnlyList<IBrowserFile>? browserFiles = InputFileChangeEvent.GetMultipleFiles(InputFileChangeEvent.FileCount);
            IBrowserFile? currentFile = browserFiles?.FirstOrDefault(i => SfBaseUtils.Equals(i.Name, fileInfo.Name));

            if (fileInfo.FileSource == "paste")
            {
                string fileName = fileInfo.Name.Split('_')[0];
                currentFile = browserFiles?.FirstOrDefault(i => SfBaseUtils.Equals(i.Name, fileName + "." + fileInfo.Type));
            }

            if (DirectoryUpload && browserFiles != null && (index - emptyFileCount >= 0)
                && (index - emptyFileCount < browserFiles.Count) && browserFiles[index - emptyFileCount] != null)
            {
                currentFile = browserFiles[index - emptyFileCount];
            }

            currentFile ??= FindFileInInputCollection(fileInfo);

            return currentFile;
        }

        /// <summary>
        /// Searches for a file in the input files collection.
        /// </summary>
        /// <param name="fileInfo">The file information to search for.</param>
        /// <returns>The matching IBrowserFile or null if not found.</returns>
        private IBrowserFile? FindFileInInputCollection(FileInfo fileInfo)
        {
            if (fileInfo == null)
            {
                return null;
            }

            foreach (InputFileChangeEventArgs obj in _inputFiles)
            {
                IReadOnlyList<IBrowserFile> files = obj.GetMultipleFiles(obj.FileCount);
                IBrowserFile? currentFile = files?.FirstOrDefault(i =>
                    SfBaseUtils.Equals(i.Name, fileInfo.Name) ||
                    SfBaseUtils.Equals(i.Name, string.Concat(fileInfo.Id.AsSpan(0, fileInfo.Id.LastIndexOf('_')), ".", fileInfo.Type)));
                if (currentFile != null)
                {
                    return currentFile;
                }
            }
            return null;
        }

        /// <summary>
        /// Handles the file upload with progress tracking.
        /// </summary>
        /// <param name="currentFile">The browser file to upload.</param>
        /// <param name="index">The current file index.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task UploadFileWithProgressAsync(IBrowserFile currentFile, int index)
        {
            if (currentFile == null || !ShowProgressBar)
            {
                return;
            }

            IsShowProgressBar = true;
            await FileSemaphore.WaitAsync().ConfigureAwait(true);
            try
            {
                if (FileData[index].Status == "File uploaded successfully")
                {
                    return;
                }

                await ProcessFileStreamAsync(currentFile, index).ConfigureAwait(true);
            }
            finally
            {
                _ = FileSemaphore.Release();
            }
        }

        /// <summary>
        /// Processes the file stream in chunks and reports progress.
        /// </summary>
        /// <param name="currentFile">The browser file to process.</param>
        /// <param name="index">The current file index.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task ProcessFileStreamAsync(IBrowserFile currentFile, int index)
        {
            using Stream fileStream = currentFile.OpenReadStream(long.MaxValue);
            byte[] bufferSize = new byte[BufferSize];
            int totalLength;
            UploadFileDetails uploadingFile = FileData[index];

            while ((totalLength = await fileStream.ReadAsync(bufferSize).ConfigureAwait(true)) != 0)
            {
                if (!ShouldContinueUpload(uploadingFile))
                {
                    break;
                }

                await UpdateUploadProgressAsync(fileStream, index).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Determines if the upload should continue based on file presence and multiple file settings.
        /// </summary>
        /// <param name="uploadingFile">The file being uploaded.</param>
        /// <returns>True if upload should continue; otherwise, false.</returns>
        private bool ShouldContinueUpload(UploadFileDetails uploadingFile)
        {
            UploadFileDetails? isPresent = FileData.FirstOrDefault(i => SfBaseUtils.Equals(i.Name, uploadingFile.Name));
            return isPresent != null || AllowMultiple;
        }

        /// <summary>
        /// Updates the upload progress for the current file.
        /// </summary>
        /// <param name="fileStream">The file stream being uploaded.</param>
        /// <param name="index">The current file index.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task UpdateUploadProgressAsync(Stream fileStream, int index)
        {
            RemoveIconDisable = DISABLED_CLASS;
            ChunkIndex = fileStream.Position / Convert.ToInt64(BufferSize);
            TotalChunk = fileStream.Length / Convert.ToInt64(BufferSize);
            FileData[FileIndex - 1].ChunkSize = ChunkIndex;
            FileData[FileIndex - 1].TotalChunkSize = TotalChunk;
            FileData[FileIndex - 1].Status = "Uploading";
            UploadStatus = UPLOAD_INPROGRESS;
            ProgressValue = (long)((decimal)fileStream.Position * 100) / fileStream.Length;

            if (Progressing.HasDelegate)
            {
                ProgressEventArgs progressEventArgs = new()
                {
                    Total = fileStream.Length,
                    Loaded = fileStream.Position,
                    LengthComputable = true,
                    File = FileData[FileIndex - 1],
                    Stream = fileStream,
                    Operation = "Progressing"
                };
                await Progressing.InvokeAsync(progressEventArgs).ConfigureAwait(true);
            }

            if (ProgressValue != 100)
            {
                string progressStyle = $"width: {ProgressValue}%";
                _progressBarAttr = new Dictionary<string, object>
                {
                    {"style", progressStyle }
                };
                await CallStateHasChangedAsync().ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Finalizes the file upload after successful completion.
        /// </summary>
        /// <param name="fileInfo">The file information to finalize.</param>
        /// <param name="index">The current file index.</param>
        /// <param name="eventArgs">The upload change event arguments.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task FinalizeFileUploadAsync(FileInfo fileInfo, int index, UploadChangeEventArgs eventArgs)
        {
            IsShowProgressBar = false;
            UploadStatus = UPLOAD_SUCCESS;
            RemoveIconDisable = string.Empty;
            fileInfo.Status = Localizer[UPLOAD_SUCCESS_KEY];
            fileInfo.StatusCode = "2";
            FileData[FileIndex - 1].Status = fileInfo.Status;
            FileData[FileIndex - 1].StatusCode = fileInfo.StatusCode;
            FileListStatusName = fileInfo.Status;
            IsShowRemoveIcon = true;

            if (!AutoUpload && !EnableUploadButton)
            {
                _actionBtnAttr = SfBaseUtils.UpdateDictionary("disabled", "disabled", _actionBtnAttr);
            }

            await RaiseSuccessEventAsync(fileInfo, index).ConfigureAwait(true);
            AddToUploadedFilesCollection(fileInfo);

            if (OnValueChange.HasDelegate)
            {
                await OnValueChange.InvokeAsync(eventArgs).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Raises the success event for a successfully uploaded file.
        /// </summary>
        /// <param name="fileInfo">The file information.</param>
        /// <param name="index">The current file index.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task RaiseSuccessEventAsync(FileInfo fileInfo, int index)
        {
            SuccessEventArgs successEventArgs = new()
            {
                File = fileInfo,
                Operation = "upload",
                StatusText = fileInfo.Status
            };

            if (Success.HasDelegate)
            {
                await Success.InvokeAsync(successEventArgs).ConfigureAwait(true);
                FileData[index].Status = successEventArgs.StatusText;
            }
        }

        /// <summary>
        /// Adds the uploaded file details to the uploaded files collection.
        /// </summary>
        /// <param name="fileInfo">The file information to add.</param>
        private void AddToUploadedFilesCollection(FileInfo fileInfo)
        {
            UploadFileDetails uploadFileDetails = new()
            {
                FileSource = fileInfo.FileSource,
                Id = fileInfo.Id,
                Name = fileInfo.Name,
                RawFile = fileInfo.RawFile,
                Size = fileInfo.Size,
                Status = fileInfo.Status,
                StatusCode = fileInfo.StatusCode,
                Type = fileInfo.Type,
                MimeContentType = fileInfo.MimeContentType,
                LastModifiedDate = fileInfo.LastModifiedDate,
                ValidationMessages = fileInfo.ValidationMessages
            };

            UploadedFileData.Add(uploadFileDetails);
            UploadedFilesInfo.Add(fileInfo);
        }

        /// <summary>
        /// Processes a single file for upload including validation, progress tracking, and completion.
        /// </summary>
        /// <param name="fileInfo">The file information to process.</param>
        /// <param name="index">The current file index.</param>
        /// <param name="emptyFileCount">The count of empty files encountered.</param>
        /// <param name="eventArgs">The upload change event arguments.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task ProcessSingleFileAsync(FileInfo fileInfo, int index, int emptyFileCount, UploadChangeEventArgs eventArgs)
        {
            if (!InitializeFileUpload(fileInfo, index))
            {
                return;
            }

            IBrowserFile? currentFile = await AcquireBrowserFileAsync(fileInfo, index, emptyFileCount).ConfigureAwait(true);
            UpdateEventArgsWithFile(eventArgs, fileInfo, currentFile);

            if (ShouldShowProgress(currentFile))
            {
                await UploadFileWithProgressAsync(currentFile!, index).ConfigureAwait(true);
            }

            await FinalizeUploadIfCompleteAsync(fileInfo, index, eventArgs).ConfigureAwait(true);
        }

        /// <summary>
        /// Initializes the file upload process by setting up tracking variables and validating the file.
        /// </summary>
        /// <param name="fileInfo">The file information to initialize.</param>
        /// <param name="index">The current file index.</param>
        /// <returns>True if the file is valid and ready for upload; otherwise, false.</returns>
        private bool InitializeFileUpload(FileInfo fileInfo, int index)
        {
            FileIndex = index + 1;
            FileInfo = fileInfo;
            ProgressValue = 0;
            _progressBarAttr = new Dictionary<string, object>
            {
                {"style", "width: 0%;" }
            };
            return IsValidFileForUpload(fileInfo);
        }

        /// <summary>
        /// Acquires the browser file reference for the specified file information.
        /// </summary>
        /// <param name="fileInfo">The file information to match.</param>
        /// <param name="index">The current file index.</param>
        /// <param name="emptyFileCount">The count of empty files encountered.</param>
        /// <returns>The browser file reference if found; otherwise, null.</returns>
        private async Task<IBrowserFile?> AcquireBrowserFileAsync(FileInfo fileInfo, int index, int emptyFileCount)
        {
            return await Task.FromResult(GetBrowserFileForUpload(fileInfo, index, emptyFileCount)).ConfigureAwait(true);
        }

        /// <summary>
        /// Updates the event arguments with the current file information.
        /// </summary>
        /// <param name="eventArgs">The event arguments to update.</param>
        /// <param name="fileInfo">The file information.</param>
        /// <param name="currentFile">The browser file reference.</param>
        private void UpdateEventArgsWithFile(UploadChangeEventArgs eventArgs, FileInfo fileInfo, IBrowserFile? currentFile)
        {
            eventArgs.Files =
            [
                new() { FileInfo = fileInfo, File = currentFile }
            ];
        }

        /// <summary>
        /// Determines whether progress tracking should be displayed for the current file.
        /// </summary>
        /// <param name="browserFile">The browser file to check.</param>
        /// <returns>True if progress should be shown; otherwise, false.</returns>
        private bool ShouldShowProgress(IBrowserFile? browserFile)
        {
            return ShowProgressBar && browserFile != null;
        }

        /// <summary>
        /// Finalizes the upload if the operation is complete or progress tracking is disabled.
        /// </summary>
        /// <param name="fileInfo">The file information.</param>
        /// <param name="index">The current file index.</param>
        /// <param name="eventArgs">The upload change event arguments.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task FinalizeUploadIfCompleteAsync(FileInfo fileInfo, int index, UploadChangeEventArgs eventArgs)
        {
            if (ProgressValue == 100 || !ShowProgressBar)
            {
                await FinalizeFileUploadAsync(fileInfo, index, eventArgs).ConfigureAwait(true);
            }

            if (fileInfo.Size == 0 && OnValueChange.HasDelegate)
            {
                await OnValueChange.InvokeAsync(eventArgs).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Handles failures during file upload by logging and raising failure events.
        /// </summary>
        /// <param name="exception">The exception that occurred during upload.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task HandleFileUploadFailureAsync(Exception exception)
        {
            LocalizedString failureMessage = Localizer[UPLOAD_FAILED_KEY];

            if (FileData != null && FileIndex > 0 && FileIndex <= FileData.Count)
            {
                FileData[FileIndex - 1].Status = failureMessage;
                FileListStatusName = FileData[FileIndex - 1].Status;
            }

            RemoveIconDisable = string.Empty;

            if (OnFailure.HasDelegate && FileInfo != null)
            {
                FailureEventArgs failureEventArgs = new()
                {
                    ChunkIndex = ChunkIndex,
                    ChunkSize = BufferSize,
                    File = FileInfo,
                    StatusText = failureMessage,
                    TotalChunk = TotalChunk
                };
                await OnFailure.InvokeAsync(failureEventArgs).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Processes the provided files by initializing the upload session, handling file changes,
        /// processing each file, managing empty file lists, and finalizing the session.
        /// </summary>
        /// <param name="files">The list of files to process.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        internal async Task GetFilesDetailsAsync(List<FileInfo> files)
        {
            InitializeUploadSession();
            UploadChangeEventArgs eventArgs = CreateUploadChangeEventArgs();
            _ = await ProcessAllFilesAsync(files, eventArgs).ConfigureAwait(true);

            await HandleEmptyFileListAsync(files, eventArgs).ConfigureAwait(true);
            await FinalizeUploadSessionAsync().ConfigureAwait(true);
        }

        /// <summary>
        /// Creates a new UploadChangeEventArgs instance with an empty file list.
        /// </summary>
        /// <returns>A new UploadChangeEventArgs instance.</returns>
        private UploadChangeEventArgs CreateUploadChangeEventArgs()
        {
            return new UploadChangeEventArgs { Files = [] };
        }

        /// <summary>
        /// Processes all files in the collection and returns the count of empty files encountered.
        /// </summary>
        /// <param name="files">The list of files to process.</param>
        /// <param name="eventArgs">The event arguments to populate with file information.</param>
        /// <returns>The count of empty or invalid files encountered during processing.</returns>
        private async Task<int> ProcessAllFilesAsync(List<FileInfo> files, UploadChangeEventArgs eventArgs)
        {
            int emptyFileCount = 0;

            for (int index = 0; index < files.Count; index++)
            {
                try
                {
                    await ProcessSingleFileAsync(files[index], index, emptyFileCount, eventArgs).ConfigureAwait(true);
                    if (!IsValidFileForUpload(files[index]))
                    {
                        RemoveIconDisable = string.Empty;
                        emptyFileCount++;
                    }
                }
                catch (Exception e)
                {
                    await HandleFileUploadFailureAsync(e).ConfigureAwait(true);
                    throw new InvalidOperationException("Unhandled exception occurred while processing files.", e);
                }
            }

            return emptyFileCount;
        }

        /// <summary>
        /// Handles the case when no files are present in the upload list.
        /// </summary>
        /// <param name="files">The list of files being processed.</param>
        /// <param name="eventArgs">The event arguments to invoke with empty file information.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task HandleEmptyFileListAsync(List<FileInfo> files, UploadChangeEventArgs eventArgs)
        {
            if (files.Count == 0 && FileInfo != null)
            {
                eventArgs.Files?.Add(new UploadFiles() { FileInfo = FileInfo });
                if (OnValueChange.HasDelegate)
                {
                    await OnValueChange.InvokeAsync(eventArgs).ConfigureAwait(true);
                }
            }
        }

        /// <summary>
        /// Finalizes the upload session by raising completion events and resetting UI state.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task FinalizeUploadSessionAsync()
        {
            ActionCompleteEventArgs actionCompleteEventArgs = new()
            {
                FileData = UploadedFilesInfo
            };

            if (OnActionComplete.HasDelegate)
            {
                await OnActionComplete.InvokeAsync(actionCompleteEventArgs).ConfigureAwait(true);
            }

            RemoveIconDisable = string.Empty;
        }

        /// <summary>
        /// Retrieves the file name without its extension from the specified file name.
        /// </summary>
        /// <param name="fileName">The full name of the file including its extension.</param>
        /// <returns>The file name without the extension.</returns>
        private static string GetFileName(string fileName)
        {
            string type = GetFileType(fileName);
            string[] names = fileName.Split(["." + type], StringSplitOptions.None);
            return names[0];
        }

        /// <summary>
        /// Extracts and returns the file extension from the specified file name.
        /// </summary>
        /// <param name="name">The name of the file from which to retrieve the extension.</param>
        /// <returns>The file extension without the leading period, or an empty string if no extension is present.</returns>
        private static string GetFileType(string name)
        {
            string extension = string.Empty;
            int index = name.LastIndexOf('.');
            if (index > 0)
            {
                extension = name[(index + 1)..];
            }

            return (!string.IsNullOrEmpty(extension)) ? extension : string.Empty;
        }

        /// <summary>
        /// Converts a file size value into a readable string formatted in KB, MB, or GB.
        /// </summary>
        /// <param name="fileSize">The size of the file in bytes.</param>
        /// <returns>A formatted string representing the file size with one decimal precision.</returns>
        private static string GetFileSize(double fileSize)
        {
            int i = -1;
            if (fileSize == 0)
            {
                return "0.0 KB";
            }

            do
            {
                fileSize /= 1024;
                i++;
            }
            while (fileSize > 99);
            if (i >= 3)
            {
                fileSize *= 1024;
                i = 2;
            }
            else if (i == 2 && fileSize < 1.0)
            {
                fileSize *= 1024;
                i = 1;
            }
            string[] sizeType = ["KB", "MB", "GB"];
            return Convert.ToString(Math.Round(fileSize, 1), CultureInfo.InvariantCulture) + SPACE + sizeType[i];
        }

        /// <summary>
        /// Renders preloaded files by processing their data and metadata and creating the file list when a template is available.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task RenderPreloadFilesAsync()
        {
            if (UploadedFiles == null || UploadedFiles.Count == 0)
            {
                return;
            }

            ProcessPreloadFileData();
            ProcessPreloadFileInfo();

            if (Template != null)
            {
                await ServerCreateFileListAsync(FileData, IsForm, true).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Processes preloaded files and adds them to the FileData collection.
        /// </summary>
        private void ProcessPreloadFileData()
        {
            if (Template == null || UploadedFiles == null || UploadedFiles.Count == 0)
            {
                return;
            }

            // Pre-allocate capacity for better performance
            FileData ??= new List<UploadFileDetails>(UploadedFiles.Count);

            foreach (UploadedFile data in UploadedFiles)
            {
                UploadFileDetails preloadFileData = CreatePreloadFileDetails(data);
                FileData.Add(preloadFileData);
            }
        }

        /// <summary>
        /// Processes preloaded files and adds them to the UploadedFilesInfo collection.
        /// </summary>
        private void ProcessPreloadFileInfo()
        {
            if (UploadedFiles == null || UploadedFiles.Count == 0)
            {
                return;
            }

            // Pre-allocate capacity for better performance
            UploadedFilesInfo ??= new List<FileInfo>(UploadedFiles.Count);

            foreach (UploadedFile filedata in UploadedFiles)
            {
                FileInfo preloadFile = CreatePreloadFileInfo(filedata);
                UploadedFilesInfo.Add(preloadFile);
            }
        }

        /// <summary>
        /// Creates an UploadFileDetails object from uploaded file data.
        /// </summary>
        /// <param name="data">The uploaded file data.</param>
        /// <returns>A new UploadFileDetails instance.</returns>
        private UploadFileDetails CreatePreloadFileDetails(UploadedFile data)
        {
            return new UploadFileDetails
            {
                Name = data.Name + '.' + data.Type.Split('.')[^1],
                Size = data.Size,
                Status = Localizer[UPLOAD_SUCCESS_KEY],
                Type = data.Type,
                StatusCode = "2",
            };
        }

        /// <summary>
        /// Creates a FileInfo object from uploaded file data.
        /// </summary>
        /// <param name="data">The uploaded file data.</param>
        /// <returns>A new FileInfo instance.</returns>
        private FileInfo CreatePreloadFileInfo(UploadedFile data)
        {
            return new FileInfo
            {
                Name = data.Name + '.' + data.Type.Split('.')[^1],
                Size = data.Size,
                Status = Localizer[UPLOAD_SUCCESS_KEY],
                Type = data.Type,
                StatusCode = "2",
            };
        }

        /// <summary>
        /// Adds server-provided file data to the uploader and optionally initiates auto-upload
        /// based on the current configuration.
        /// </summary>
        /// <param name="fileData">The list of files received from the server.</param>
        /// <param name="isFormRender">Indicates whether the uploader is rendered within a form.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        internal async Task ServerFileDataAsync(List<UploadFileDetails> fileData, bool isFormRender)
        {
            if (fileData == null || fileData.Count == 0)
            {
                return;
            }

            IsForm = isFormRender;
            // Optimize: Use AddRange instead of loop and pre-allocate if needed
            FileData ??= new List<UploadFileDetails>(fileData.Count);

            FileData.AddRange(fileData);

            if (AutoUpload)
            {
                await Task.Delay(UI_SYNC_DELAY_MS).ConfigureAwait(true);
                await UploadHandlerAsync().ConfigureAwait(true);
            }
            else
            {
                _ = _actionBtnAttr.Remove("disabled");
                EnableUploadButton = true;
            }
        }

        /// <summary>
        /// Creates or merges the server file list, initializes uploader state, and triggers
        /// client-side or server-side upload processing as required.
        /// </summary>
        /// <param name="fileData">The list of files received from the server.</param>
        /// <param name="isFormRender">Indicates whether the uploader is rendered within a form.</param>
        /// <param name="isTemplateWithPreLoadFile">Indicates whether the files are preloaded via a template and should be merged with existing file data.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        internal async Task ServerCreateFileListAsync(List<UploadFileDetails> fileData, bool isFormRender, bool isTemplateWithPreLoadFile = false)
        {
            MergeExistingFileData(fileData, isTemplateWithPreLoadFile);
            InitializeFileListProperties(fileData, isFormRender);
            UpdateActionButtonState(fileData);

            if (string.IsNullOrEmpty(UploadAsyncSettings?.SaveUrl))
            {
                await HandleClientSideUploadAsync().ConfigureAwait(true);
            }
            else
            {
                await HandleServerSideUploadAsync().ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Merges existing file data with new files when multiple upload is enabled.
        /// </summary>
        /// <param name="fileData">The new file data to merge.</param>
        /// <param name="isTemplateWithPreLoadFile">Whether this is a template with preloaded files.</param>
        private void MergeExistingFileData(List<UploadFileDetails> fileData, bool isTemplateWithPreLoadFile)
        {
            if (string.IsNullOrEmpty(UploadAsyncSettings?.SaveUrl) && AllowMultiple)
            {
                if (FileData != null && FileData.Count > 0 && !isTemplateWithPreLoadFile)
                {
                    for (int i = FileData.Count - 1; i >= 0; i--)
                    {
                        fileData.Insert(0, FileData[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Initializes file list properties and resets UI state.
        /// </summary>
        /// <param name="fileData">The file data to set.</param>
        /// <param name="isFormRender">Whether this is a form render.</param>
        private void InitializeFileListProperties(List<UploadFileDetails> fileData, bool isFormRender)
        {
            IsForm = isFormRender;
            FileData = fileData;
            FileListClass = FILE_LIST_CLASS;
            FileListStatus = STATUS;
            IsClearAll = false;
            IsShowRemoveIcon = false;
            IsShowProgressBar = false;
        }

        /// <summary>
        /// Updates the action button state based on uploaded files.
        /// </summary>
        /// <param name="fileData">The file data to check.</param>
        private void UpdateActionButtonState(List<UploadFileDetails> fileData)
        {
            if (Template != null && UploadedFiles != null && UploadedFiles.Count != 0)
            {
                List<UploadFileDetails>? uploadedFiles = fileData?.Where(file => file.StatusCode == "2").ToList();
                if (fileData != null && uploadedFiles?.Count == fileData.Count)
                {
                    _actionBtnAttr = SfBaseUtils.UpdateDictionary("disabled", "disabled", _actionBtnAttr);
                }
                else
                {
                    _ = _actionBtnAttr.Remove("disabled");
                }
            }
        }

        /// <summary>
        /// Handles client-side upload processing and UI updates.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task HandleClientSideUploadAsync()
        {
            if (AutoUpload)
            {
                await Task.Delay(RENDER_DELAY_MS).ConfigureAwait(true);
                await UploadHandlerAsync().ConfigureAwait(true);
            }
            else
            {
                _ = _actionBtnAttr.Remove("disabled");
                EnableUploadButton = true;
            }

            await CallStateHasChangedAsync().ConfigureAwait(true);

            if (!AutoUpload && ButtonElement.HasValue)
            {
                await ButtonElement.Value.FocusAsync().ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Handles server-side upload processing and JavaScript initialization.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task HandleServerSideUploadAsync()
        {
            await CallStateHasChangedAsync().ConfigureAwait(true);
            await InvokeVoidAsync(_uploaderJsModule!, _uploaderJsInProcessModule!, "serverFileListElement",
                [DataId, UlElementRef!, ActionButtonRef!, AutoUpload]).ConfigureAwait(true);
        }

        /// <summary>
        /// Triggered when the file upload process starts.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task UploadHandlerAsync()
        {
            if (string.IsNullOrEmpty(UploadAsyncSettings?.SaveUrl))
            {
                EnableUploadButton = false;
                List<FileInfo> filesData = ConvertFileDataToFileInfo();
                BeforeUploadEventArgs beforeUploadEvent = await CreateAndInvokeBeforeUploadEventAsync(filesData).ConfigureAwait(true);

                if (!beforeUploadEvent.Cancel)
                {
                    await GetFilesDetailsAsync(beforeUploadEvent.FilesData).ConfigureAwait(true);
                }
            }
        }

        /// <summary>
        /// Converts FileData collection to FileInfo collection.
        /// </summary>
        /// <returns>A list of FileInfo objects.</returns>
        private List<FileInfo> ConvertFileDataToFileInfo()
        {
            List<FileInfo> filesData = [];
            if (FileData != null && FileData.Count > 0)
            {
                for (int i = 0; i < FileData.Count; i++)
                {
                    FileInfo fileInfo = new()
                    {
                        FileSource = FileData[i].FileSource,
                        Id = FileData[i].Id,
                        Name = FileData[i].Name,
                        RawFile = FileData[i].RawFile,
                        Size = FileData[i].Size,
                        Status = FileData[i].Status,
                        StatusCode = FileData[i].StatusCode,
                        Type = FileData[i].Type,
                        MimeContentType = FileData[i].MimeContentType,
                        LastModifiedDate = FileData[i].LastModifiedDate,
                        ValidationMessages = FileData[i].ValidationMessages
                    };
                    filesData.Add(fileInfo);
                }
            }

            return filesData;
        }

        /// <summary>
        /// Creates and invokes the BeforeUpload event.
        /// </summary>
        /// <param name="filesData">The list of files to upload.</param>
        /// <returns>The BeforeUploadEventArgs after event invocation.</returns>
        private async Task<BeforeUploadEventArgs> CreateAndInvokeBeforeUploadEventAsync(List<FileInfo> filesData)
        {
            BeforeUploadEventArgs beforeUploadEventArgs = new()
            {
                Cancel = false,
                WithCredentials = false,
                FilesData = filesData
            };

            return await BeforeUploadEventAsync(beforeUploadEventArgs).ConfigureAwait(true);
        }

        /// <summary>
        /// Updates the uploader state and UI when a file fails minimum or maximum validation checks.
        /// </summary>
        /// <param name="statusText">The validation status message to display for the file.</param>
        private void UpdateMinMaxValid(string statusText)
        {
            if (FileData.Count == 1 && !AutoUpload)
            {
                _actionBtnAttr = SfBaseUtils.UpdateDictionary("disabled", "disabled", _actionBtnAttr);
            }

            UploadStatus = VALIDATION_FAIL;
            FileListStatusName = statusText;
        }

        /// <summary>
        /// Validates the size of the specified file and updates its validation status,
        /// messages, and uploader state accordingly.
        /// </summary>
        /// <param name="file">The file details to validate against the minimum and maximum size limits.</param>
        internal void ValidatedFileSize(UploadFileDetails file)
        {
            file.ValidationMessages ??= new ValidationMessages();

            if (file.Size < MinFileSize)
            {
                UpdateMinMaxValid(Localizer[INVALID_MIN_FILE_KEY]);
                file.ValidationMessages.MinSize = FileListStatusName ?? string.Empty;
            }
            else if (file.Size > MaxFileSize)
            {
                UpdateMinMaxValid(Localizer[INVALID_MAX_FILE_KEY]);
                file.ValidationMessages.MaxSize = FileListStatusName ?? string.Empty;
            }
            else if (file.Status == Localizer[READY_UPLOAD_KEY])
            {
                if (_actionBtnAttr.ContainsKey(DISABLED_CLASS) && !AutoUpload)
                {
                    _ = _actionBtnAttr.Remove("disabled");
                }

                UploadStatus = string.Empty;
                FileListStatusName = Localizer[READY_UPLOAD_KEY];
                file.Status = FileListStatusName;
            }
            else
            {
                UpdateMinMaxValid(Localizer[INVALID_FILE_KEY]);
                file.Status = FileListStatusName ?? string.Empty;
            }
        }

        /// <summary>
        /// Handles keyboard events for uploader actions.
        /// </summary>
        /// <param name="eventArgs">The keyboard event arguments.</param>
        /// <param name="action">The action to perform (remove, clear, upload).</param>
        /// <param name="file">The file to operate on (optional).</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task KeyHandlerAsync(KeyboardEventArgs eventArgs, string action, UploadFileDetails? file = null)
        {
            if (eventArgs.Code == "Enter")
            {
                switch (action)
                {
                    case "remove":
                        await RemoveHandlerAsync(file).ConfigureAwait(true);
                        break;
                    case "clear":
                        await ClearAllHandlerAsync().ConfigureAwait(true);
                        break;
                    case "upload":
                        await UploadHandlerAsync().ConfigureAwait(true);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Clears all files from the uploader.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        internal async Task ClearAllHandlerAsync()
        {
            ClearingEventArgs clearingEvent = new();

            if (string.IsNullOrEmpty(_asyncSettings?.SaveUrl))
            {
                ClearingEventArgs clearingEventArgs = new()
                {
                    Cancel = false,
                    FilesData = UploadedFilesInfo
                };
                clearingEvent = await ClearingEventAsync(clearingEventArgs).ConfigureAwait(true);
            }

            if (!clearingEvent.Cancel || !string.IsNullOrEmpty(_asyncSettings?.SaveUrl))
            {
                ClearFileCollections();
            }
        }

        /// <summary>
        /// Clears all file collections and resets UI state.
        /// </summary>
        private void ClearFileCollections()
        {
            IsClearAll = true;
            FileData = [];
            UploadedFileData = [];
            UploadedFilesInfo = [];
        }

        /// <summary>
        /// Removes a specific file from the uploader by locating it in the current file collection
        /// and delegating to <see cref="RemoveFileListAsync(int, UploadFileDetails?, MouseEventArgs?)"/>.
        /// </summary>
        /// <param name="file">The file to remove. If <c>null</c>, the method returns without action.</param>
        /// <param name="eventArgs">Optional mouse event arguments associated with the removal action.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task RemoveHandlerAsync(UploadFileDetails? file, MouseEventArgs? eventArgs = null)
        {
            if (file == null || FileData == null)
            {
                return;
            }

            UploadFileDetails? fileToRemove = FileData.FirstOrDefault(e => SfBaseUtils.Equals(e.Name, file.Name));
            if (fileToRemove == null)
            {
                return;
            }

            int index = FileData.IndexOf(fileToRemove);
            if (index > -1)
            {
                await RemoveFileListAsync(index, file, eventArgs).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Removes a file from internal collections.
        /// </summary>
        /// <param name="index">The zero-based index of the file to remove.</param>
        /// <param name="file">Optional file reference to remove.</param>
        /// <param name="eventArgs">Optional mouse event arguments associated with the removal action.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        internal async Task RemoveFileListAsync(int index, UploadFileDetails? file = null, MouseEventArgs? eventArgs = null)
        {
            try
            {
                if (FileData == null)
                {
                    return;
                }

                (BeforeRemoveEventArgs? beforeRemoveEvent, RemovingEventArgs? removingEvent) = await InvokeRemovalEventsAsync(file, eventArgs).ConfigureAwait(true);

                if (ShouldRemoveFile(beforeRemoveEvent, removingEvent))
                {
                    RemoveFileFromCollections(index, file);
                    await HandleServerSideRemovalAsync().ConfigureAwait(true);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unhandled exception occurred while processing files.", ex);
            }
        }

        /// <summary>
        /// Invokes removal-related events for client-side upload.
        /// </summary>
        /// <param name="file">The file to remove.</param>
        /// <param name="eventArgs">The mouse event arguments.</param>
        /// <returns>A tuple containing BeforeRemoveEventArgs and RemovingEventArgs.</returns>
        private async Task<(BeforeRemoveEventArgs, RemovingEventArgs)> InvokeRemovalEventsAsync(
            UploadFileDetails? file,
            MouseEventArgs? eventArgs)
        {
            BeforeRemoveEventArgs beforeRemoveEvent = new();
            RemovingEventArgs removingEvent = new();

            if (string.IsNullOrEmpty(_asyncSettings?.SaveUrl))
            {
                UploadFileDetails? removeFile = FileData.FirstOrDefault(e => SfBaseUtils.Equals(e.Name, file?.Name));
                if (removeFile != null)
                {
                    List<FileInfo> removeFiles = [removeFile];

                    beforeRemoveEvent = await CreateAndInvokeBeforeRemoveEventAsync(removeFiles).ConfigureAwait(true);
                    removingEvent = await CreateAndInvokeRemovingEventAsync(removeFiles, eventArgs).ConfigureAwait(true);

                    if (!removingEvent.Cancel)
                    {
                        await InvokeSuccessEventForRemovalAsync(removeFile).ConfigureAwait(true);
                    }
                }
            }

            return (beforeRemoveEvent, removingEvent);
        }

        /// <summary>
        /// Creates and invokes the BeforeRemove event.
        /// </summary>
        /// <param name="removeFiles">The files to remove.</param>
        /// <returns>The BeforeRemoveEventArgs after event invocation.</returns>
        private async Task<BeforeRemoveEventArgs> CreateAndInvokeBeforeRemoveEventAsync(List<FileInfo> removeFiles)
        {
            BeforeRemoveEventArgs beforeRemoveEventArgs = new()
            {
                Cancel = false,
                FilesData = removeFiles,
                PostRawFile = true
            };
            return await BeforeRemoveEventAsync(beforeRemoveEventArgs).ConfigureAwait(true);
        }

        /// <summary>
        /// Creates and invokes the Removing event.
        /// </summary>
        /// <param name="removeFiles">The files to remove.</param>
        /// <param name="eventArgs">The mouse event arguments.</param>
        /// <returns>The RemovingEventArgs after event invocation.</returns>
        private async Task<RemovingEventArgs> CreateAndInvokeRemovingEventAsync(
            List<FileInfo> removeFiles,
            MouseEventArgs? eventArgs)
        {
            RemovingEventArgs removingEventArgs = new()
            {
                Cancel = false,
                FilesData = removeFiles,
                PostRawFile = true
            };
            return await RemovingEventAsync(removingEventArgs).ConfigureAwait(true);
        }

        /// <summary>
        /// Invokes the Success event after successful file removal.
        /// </summary>
        /// <param name="removeFile">The removed file.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task InvokeSuccessEventForRemovalAsync(FileInfo removeFile)
        {
            removeFile.Status = "File removed successfully";
            SuccessEventArgs successEventArgs = new()
            {
                File = removeFile,
                Operation = "upload",
            };

            if (Success.HasDelegate)
            {
                await Success.InvokeAsync(successEventArgs).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Determines if the file should be removed based on event cancellations.
        /// </summary>
        /// <param name="beforeRemoveEvent">The BeforeRemove event arguments.</param>
        /// <param name="removingEvent">The Removing event arguments.</param>
        /// <returns>True if the file should be removed; otherwise, false.</returns>
        private bool ShouldRemoveFile(BeforeRemoveEventArgs beforeRemoveEvent, RemovingEventArgs removingEvent)
        {
            return (!(beforeRemoveEvent.Cancel || removingEvent.Cancel)) ||
                   (!string.IsNullOrEmpty(_asyncSettings?.SaveUrl));
        }

        /// <summary>
        /// Removes the file from all internal collections.
        /// </summary>
        /// <param name="index">The index of the file in FileData.</param>
        /// <param name="file">The file details to remove.</param>
        private void RemoveFileFromCollections(int index, UploadFileDetails? file)
        {
            FileData.RemoveRange(index, 1);

            if (file != null)
            {
                RemoveFromUploadedFileData(file);
                RemoveFromUploadedFilesInfo(file);
            }

            if (FileData.Count == 0)
            {
                IsClearAll = true;
            }
        }

        /// <summary>
        /// Removes a file from the UploadedFileData collection.
        /// </summary>
        /// <param name="file">The file to remove.</param>
        private void RemoveFromUploadedFileData(UploadFileDetails file)
        {
            RemoveFileByName(UploadedFileData, file.Name, (UploadFileDetails f) => f.Name);
        }

        /// <summary>
        /// Removes a file from the UploadedFilesInfo collection.
        /// </summary>
        /// <param name="file">The file to remove.</param>
        private void RemoveFromUploadedFilesInfo(UploadFileDetails file)
        {
            RemoveFileByName(UploadedFilesInfo, file.Name, (FileInfo f) => f.Name);
        }

        /// <summary>
        /// Removes the first item from the provided list whose name matches the provided name using the selector.
        /// </summary>
        private static void RemoveFileByName<T>(List<T>? list, string name, Func<T, string?> getName)
        {
            if (list == null || string.IsNullOrEmpty(name))
            {
                return;
            }

            int index = list.FindIndex(item => SfBaseUtils.Equals(getName(item), name));
            if (index >= 0)
            {
                list.RemoveAt(index);
            }
        }

        /// <summary>
        /// Handles server-side removal JavaScript invocation.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task HandleServerSideRemovalAsync()
        {
            if (Template == null && !string.IsNullOrEmpty(_asyncSettings?.SaveUrl))
            {
                await InvokeVoidAsync(_uploaderJsModule, _uploaderJsInProcessModule, "serverRemoveIconBindEvent", [DataId]).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Method to update the template.
        /// </summary>
        /// <param name="template"></param>
        internal void UpdateTemplate(RenderFragment<FileInfo> template)
        {
            Template = template;
        }

        /// <summary>
        /// Update the dropdownlist fileds.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal void UpdateChildProperties(string key, object? fieldValue)
        {
            switch (key)
            {
                case "Files":
                    UploadedFiles = _files = (List<UploadedFile>)fieldValue!;
                    break;
                case "AsyncSettings":
                    UploaderAsyncSettings asyncSetting = fieldValue == null ? new UploaderAsyncSettings() : (UploaderAsyncSettings)fieldValue;
                    UploadAsyncSettings = _asyncSettings = asyncSetting;
                    break;
                case "Buttons":
                    UploaderButtons button = fieldValue == null ? new UploaderButtons() : (UploaderButtons)fieldValue;
                    UploadButtons = _buttons = button;
                    UpdateBrowsBtn();
                    break;
                default:
                    break;
            }
        }
    }
}
