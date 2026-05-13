using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    /// <summary>
    /// Represents the asynchronous settings configuration for the <see cref="SfUploader"/> component, enabling chunk upload, retry mechanisms, and server endpoint configurations.
    /// </summary>
    /// <remarks>
    /// The <see cref="UploaderAsyncSettings"/> class provides comprehensive configuration options for handling asynchronous file upload operations. 
    /// It supports chunk-based uploading for large files, automatic retry mechanisms for failed uploads, and configurable server endpoints for save and remove operations.
    /// This class is essential for implementing robust file upload functionality with enhanced reliability and performance optimization.
    /// </remarks>
    /// <example>
    /// The following example demonstrates how to configure asynchronous settings for the Uploader component:
    /// <code><![CDATA[
    /// <SfUploader>
    ///     <UploaderAsyncSettings 
    ///         SaveUrl="/api/upload" 
    ///         RemoveUrl="/api/remove"
    ///         ChunkSize="1048576"
    ///         RetryCount="3"
    ///         RetryAfterDelay="1000">
    ///     </UploaderAsyncSettings>
    /// </SfUploader>
    /// ]]></code>
    /// </example>
    public class UploaderAsyncSettings : SfBaseComponent
    {
        /// <summary>
        /// JavaScript interop method name for property changes.
        /// </summary>
        private const string JS_PROPERTY_CHANGES = "sfBlazorToolkit.Uploader.propertyChanges";

        /// <summary>
        /// Minimum allowed chunk size (1 KB).
        /// </summary>
        private const double MIN_CHUNK_SIZE = 1024;

        /// <summary>
        /// Maximum allowed chunk size (100 MB).
        /// </summary>
        private const double MAX_CHUNK_SIZE = 104857600;

        /// <summary>
        /// Minimum retry delay in milliseconds.
        /// </summary>
        private const double MIN_RETRY_DELAY = 100;

        /// <summary>
        /// Maximum retry delay in milliseconds (60 seconds).
        /// </summary>
        private const double MAX_RETRY_DELAY = 60000;

        /// <summary>
        /// Maximum allowed retry count.
        /// </summary>
        private const int MAX_RETRY_COUNT = 10;

        [CascadingParameter]
        private SfUploader? Parent { get; set; }

        /// <summary>
        /// Specifies the content that has to be passed.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// Gets or sets the chunk size in bytes for splitting large files into smaller chunks during upload.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> value representing the chunk size in bytes. The default value is 0, which disables chunk upload.
        /// </value>
        /// <remarks>
        /// When the <see cref="ChunkSize"/> property is set to a value greater than 0, the <see cref="SfUploader"/> automatically enables chunk upload functionality.
        /// Large files are divided into smaller chunks of the specified size and uploaded sequentially to the server.
        /// This approach improves upload reliability for large files and provides better progress tracking.
        /// The chunk size must be specified in bytes and should be chosen based on network conditions and server capabilities.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <UploaderAsyncSettings ChunkSize="1048576"> <!-- 1 MB chunks -->
        /// </UploaderAsyncSettings>
        /// ]]></code>
        /// </example>
        [Parameter]
        [Range(0, MAX_CHUNK_SIZE, ErrorMessage = "ChunkSize must be between 0 and 100 MB")]
        public double ChunkSize { get; set; }

        private double _chunkSize;

        /// <summary>
        /// Gets or sets the server endpoint URL for handling file removal operations.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the server URL that handles file removal requests. The default value is <see cref="string.Empty"/>.
        /// </value>
        /// <remarks>
        /// The <see cref="RemoveUrl"/> property specifies the server endpoint that will receive and process file removal requests.
        /// The server endpoint must accept POST requests and should define a "RemoveFileNames" attribute to receive the file information that needs to be removed.
        /// This property is optional; if not specified, the remove functionality will be disabled.
        /// The server implementation should handle the removal of files from the storage location and return appropriate responses.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <UploaderAsyncSettings RemoveUrl="/api/files/remove">
        /// </UploaderAsyncSettings>
        /// ]]></code>
        /// </example>
        [Parameter]
        public string RemoveUrl { get; set; } = string.Empty;

        private string? _removeUrl;

        /// <summary>
        /// Gets or sets the delay time in milliseconds before automatic retry attempts for failed uploads.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> value representing the delay in milliseconds before retry attempts. The default value is 500 milliseconds.
        /// </value>
        /// <remarks>
        /// The <see cref="RetryAfterDelay"/> property controls the waiting period between failed upload attempts and automatic retries.
        /// This delay helps prevent overwhelming the server with immediate retry requests and allows for temporary network issues to resolve.
        /// The delay is applied before each retry attempt, providing a progressive approach to handling upload failures.
        /// A reasonable delay value improves the success rate of retry operations while maintaining good user experience.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <UploaderAsyncSettings RetryAfterDelay="1000"> <!-- 1 second delay -->
        /// </UploaderAsyncSettings>
        /// ]]></code>
        /// </example>
        [Parameter]
        [Range(MIN_RETRY_DELAY, MAX_RETRY_DELAY, ErrorMessage = "RetryAfterDelay must be between 100ms and 60000ms")]
        public double RetryAfterDelay { get; set; } = 500;

        private double _retryAfterDelay;

        /// <summary>
        /// Gets or sets the maximum number of retry attempts for failed file uploads.
        /// </summary>
        /// <value>
        /// An <see cref="int"/> value representing the maximum number of retry attempts. The default value is 3.
        /// </value>
        /// <remarks>
        /// The <see cref="RetryCount"/> property defines the maximum number of automatic retry attempts the <see cref="SfUploader"/> will perform when a file upload fails.
        /// This property is essential to prevent infinite retry loops and control resource usage.
        /// After the specified number of retry attempts is exhausted, the upload will be marked as failed.
        /// Setting an appropriate retry count helps balance between upload reliability and system performance.
        /// The retry mechanism works in conjunction with <see cref="RetryAfterDelay"/> to provide controlled and intelligent retry behavior.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <UploaderAsyncSettings RetryCount="5"> <!-- Maximum 5 retry attempts -->
        /// </UploaderAsyncSettings>
        /// ]]></code>
        /// </example>
        [Parameter]
        [Range(0, MAX_RETRY_COUNT, ErrorMessage = "RetryCount must be between 0 and 10")]
        public int RetryCount { get; set; } = 3;

        private int _retryCount;

        /// <summary>
        /// Gets or sets the server endpoint URL for handling file upload and save operations.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the server URL that handles file upload requests. The default value is <see cref="string.Empty"/>.
        /// </value>
        /// <remarks>
        /// The <see cref="SaveUrl"/> property specifies the server endpoint that will receive uploaded files and handle the save operations.
        /// The server endpoint must accept POST requests and should define the request parameter with the same input name used to render the component.
        /// This URL is essential for asynchronous file upload functionality and must be properly configured to handle multipart form data.
        /// The server implementation should process the uploaded files, save them to the desired location, and return appropriate responses indicating success or failure.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <UploaderAsyncSettings SaveUrl="/api/files/upload">
        /// </UploaderAsyncSettings>
        /// ]]></code>
        /// </example>
        [Parameter]
        public string SaveUrl { get; set; } = string.Empty;

        private string? _saveUrl;

        /// <summary>
        /// Validates that the chunk size is either 0 (disabled) or within the acceptable range.
        /// </summary>
        /// <param name="chunkSize">The chunk size to validate.</param>
        /// <returns>True if valid; otherwise, false.</returns>
        private static bool IsValidChunkSize(double chunkSize)
        {
            return chunkSize is 0 or (>= MIN_CHUNK_SIZE and <= MAX_CHUNK_SIZE);
        }

        /// <summary>
        /// Validates that a URL is either empty or a valid relative/absolute URI.
        /// </summary>
        /// <param name="url">The URL to validate.</param>
        /// <returns>True if valid; otherwise, false.</returns>
        private static bool IsValidUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return true;
            }

            string character = "/";
            // Allow relative URLs (starting with /) and valid absolute URLs
            return url.StartsWith(character, StringComparison.Ordinal) || (Uri.TryCreate(url, UriKind.Absolute, out Uri? uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps));
        }

        /// <summary>
        /// Validates all component parameters and logs warnings for invalid values.
        /// </summary>
        private void ValidateParameters()
        {
            if (!IsValidChunkSize(ChunkSize))
            {
                // Log warning or throw exception based on requirements
                ChunkSize = 0; // Fallback to disabled
            }

            if (!IsValidUrl(SaveUrl))
            {
                SaveUrl = string.Empty; // Sanitize invalid URL
            }

            if (!IsValidUrl(RemoveUrl))
            {
                RemoveUrl = string.Empty; // Sanitize invalid URL
            }

            if (RetryAfterDelay < MIN_RETRY_DELAY)
            {
                RetryAfterDelay = MIN_RETRY_DELAY;
            }
            else if (RetryAfterDelay > MAX_RETRY_DELAY)
            {
                RetryAfterDelay = MAX_RETRY_DELAY;
            }

            if (RetryCount < 0)
            {
                RetryCount = 0;
            }
            else if (RetryCount > MAX_RETRY_COUNT)
            {
                RetryCount = MAX_RETRY_COUNT;
            }
        }

        /// <summary>
        /// Triggers while initial rendering of the component.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(true);
            ValidateParameters();
            Parent?.UpdateChildProperties("AsyncSettings", this);
            _chunkSize = ChunkSize;
            _removeUrl = RemoveUrl;
            _retryAfterDelay = RetryAfterDelay;
            _retryCount = RetryCount;
            _saveUrl = SaveUrl;
            if (Parent != null)
            {
                await Parent.CallStateHasChangedAsync().ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Triggers after the component parameters are set.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(true);
            ValidateParameters();
            _chunkSize = NotifyPropertyChanges(nameof(ChunkSize), ChunkSize, _chunkSize);
            _removeUrl = NotifyPropertyChanges(nameof(RemoveUrl), RemoveUrl, _removeUrl);
            _retryAfterDelay = NotifyPropertyChanges(nameof(RetryAfterDelay), RetryAfterDelay, _retryAfterDelay);
            _retryCount = NotifyPropertyChanges(nameof(RetryCount), RetryCount, _retryCount);
            _saveUrl = NotifyPropertyChanges(nameof(SaveUrl), SaveUrl, _saveUrl);
            if (PropertyChanges?.Count > 0 && IsRendered && Parent != null)
            {
                await UpdateParentPropertiesAsync().ConfigureAwait(true);
                PropertyChanges.Clear(); // Clear to prevent accumulation
            }
        }

        /// <summary>
        /// Updates parent component properties and invokes JavaScript interop for property changes.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task UpdateParentPropertiesAsync()
        {
            if (Parent == null)
            {
                return;
            }

            try
            {
                Parent.UpdateChildProperties("AsyncSettings", this);
                Dictionary<string, object> options = Parent.GetProperty();
                Dictionary<string, object> asyncProps = new() { ["AsyncSettings"] = this };
                await InvokeVoidAsync(Parent._uploaderJsModule, Parent._uploaderJsInProcessModule, JS_PROPERTY_CHANGES, [Parent.ID, options, asyncProps]).ConfigureAwait(true);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Unhandled exception occurred", e);
            }
        }

        /// <summary>
        /// Disposes resources used by the component.
        /// </summary>
        /// <exclude/>
        protected override ValueTask DisposeAsyncCore()
        {
            // Clear property changes to prevent memory leaks
            PropertyChanges?.Clear();
            // Null out parent reference
            Parent = null;
            return base.DisposeAsyncCore();
        }
    }
}