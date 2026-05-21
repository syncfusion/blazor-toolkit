using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components.Forms;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    /// <summary>
    /// Defines default configuration constants for the file uploader component.
    /// </summary>
    public static class UploaderDefaults
    {
        /// <summary>
        /// Default maximum file size in bytes (30 MB).
        /// </summary>
        public const long MaxFileSize = 30_000_000;

        /// <summary>
        /// Default minimum file size in bytes (0 bytes - no minimum restriction).
        /// </summary>
        public const long MinFileSize = 0;

        /// <summary>
        /// Default retry count for failed upload operations.
        /// </summary>
        public const int RetryCount = 3;

        /// <summary>
        /// Default delay in milliseconds before retrying a failed upload.
        /// </summary>
        public const int RetryAfterDelay = 500;

        /// <summary>
        /// Default chunk size in bytes (0 - chunking disabled by default).
        /// </summary>
        public const long ChunkSize = 0;
    }

    /// <summary>
    /// Provides information about the <see cref="SfUploader.OnActionComplete"/> event callback.
    /// </summary>
    /// <remarks>
    /// This event is triggered after all selected files have been processed (either successfully uploaded or failed).
    /// It provides comprehensive information about the completed upload operation, including details of all files
    /// that were part of the upload batch.
    /// </remarks>
    public class ActionCompleteEventArgs
    {
        /// <summary>
        /// Gets or sets the list of selected files' details from the <see cref="SfUploader"/> component.
        /// </summary>
        /// <value>
        /// A <see cref="List{FileInfo}"/> containing the details of all files that were processed, or <c>null</c> if no files were processed.
        /// </value>
        /// <remarks>
        /// This property contains comprehensive information about each file that was part of the upload operation,
        /// including file names, sizes, upload status, and any error information. It allows developers to review
        /// the results of the entire upload batch and take appropriate action based on the outcomes.
        /// </remarks>
        [JsonPropertyName("fileData")]
        public List<FileInfo> FileData { get; set; } = [];
    }

    /// <summary>
    /// Provides information about the <see cref="SfUploader.BeforeRemove"/> event callback.
    /// </summary>
    /// <remarks>
    /// This event is triggered before files are removed from the uploader component. It provides an opportunity
    /// to cancel the removal operation, modify the request parameters, or perform custom validation before
    /// the files are actually removed from the server.
    /// </remarks>
    public class BeforeRemoveEventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether the file removal action has been canceled.
        /// </summary>
        /// <value>
        /// <c>true</c> to cancel the file removal operation; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// Setting this property to <c>true</c> will prevent the file removal operation from proceeding.
        /// This allows for conditional removal based on custom logic or user confirmation.
        /// </remarks>
        [JsonPropertyName("cancel")]
        public bool Cancel { get; set; }

        /// <summary>
        /// Gets or sets the XMLHttpRequest instance associated with the file removal operation.
        /// </summary>
        /// <value>
        /// An object containing XMLHttpRequest details, or <c>null</c> if no request is associated.
        /// </value>
        /// <remarks>
        /// This property provides access to the underlying HTTP request object that will be used for the
        /// file removal operation. It allows for customization of request headers, authentication, and
        /// other HTTP-specific settings before the removal request is sent to the server.
        /// </remarks>
        [DefaultValue(null)]
        [JsonPropertyName("currentRequest")]
        public object? CurrentRequest { get; set; }

        /// <summary>
        /// Gets or sets the list of file details that will be removed from the server.
        /// </summary>
        /// <value>
        /// A <see cref="List{FileInfo}"/> containing the details of files to be removed, or <c>null</c> if no files are specified.
        /// </value>
        /// <remarks>
        /// This property contains information about all files that are scheduled for removal, including
        /// file names, sizes, and other metadata. This information can be used for logging, validation,
        /// or displaying confirmation messages to users before removal.
        /// </remarks>
        [JsonPropertyName("filesData")]
        public List<FileInfo> FilesData { get; set; } = [];

        /// <summary>
        /// Gets or sets additional custom data in key-value pair format to be submitted with the removal request.
        /// </summary>
        /// <value>
        /// An object containing key-value pairs of additional data, or <c>null</c> if no custom data is provided.
        /// </value>
        /// <remarks>
        /// This property allows developers to include additional parameters or metadata with the file removal
        /// request. Common uses include authentication tokens, user identifiers, or other application-specific
        /// data required by the server-side removal handler.
        /// </remarks>
        [DefaultValue(null)]
        [JsonPropertyName("customFormData")]
        public object? CustomFormData { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the selected raw file data should be sent to the server during removal.
        /// </summary>
        /// <value>
        /// <c>true</c> to send the raw file data to the server; otherwise, <c>false</c>. The default is <c>true</c>.
        /// </value>
        /// <remarks>
        /// When set to <c>true</c>, the complete file information including binary data is sent to the server.
        /// When set to <c>false</c>, only the file metadata (such as filename and size) is sent. Setting this
        /// to <c>false</c> can improve performance when only file identification is needed for removal operations.
        /// </remarks>
        [JsonPropertyName("postRawFile")]
        public bool PostRawFile { get; set; } = true;
    }

    /// <summary>
    /// Provides information about the <see cref="SfUploader.BeforeUpload"/> event callback.
    /// </summary>
    /// <remarks>
    /// This event is triggered before files are uploaded to the server. It provides an opportunity to cancel
    /// the upload operation, modify request parameters, add authentication headers, or perform validation
    /// before the actual upload process begins.
    /// </remarks>
    public class BeforeUploadEventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether the file upload action has been canceled.
        /// </summary>
        /// <value>
        /// <c>true</c> to cancel the file upload operation; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// Setting this property to <c>true</c> will prevent the file upload operation from proceeding.
        /// This allows for conditional uploads based on file validation, user permissions, or other custom logic.
        /// </remarks>
        [JsonPropertyName("cancel")]
        public bool Cancel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether requests should include credentials for cross-origin requests.
        /// </summary>
        /// <value>
        /// <c>true</c> to include credentials such as cookies and authorization headers in cross-site requests; otherwise, <c>false</c>. The default is <c>false</c>.
        /// </value>
        /// <remarks>
        /// When set to <c>true</c>, the upload request will include credentials such as cookies, authorization headers,
        /// and client certificates for cross-origin requests. This is essential for authenticated uploads when the
        /// upload endpoint is on a different domain than the client application.
        /// </remarks>
        [JsonPropertyName("withCredentials")]
        public bool WithCredentials { get; set; }

        /// <summary>
        /// Gets or sets the XMLHttpRequest instance associated with the file upload operation.
        /// </summary>
        /// <value>
        /// An object containing XMLHttpRequest details, or <c>null</c> if no request is associated.
        /// </value>
        /// <remarks>
        /// This property provides access to the underlying HTTP request object that will be used for the
        /// file upload operation. It allows for customization of request headers, progress monitoring, and
        /// other HTTP-specific settings before the upload request is sent to the server.
        /// </remarks>
        [DefaultValue(null)]
        [JsonPropertyName("currentRequest")]
        public object? CurrentRequest { get; set; }

        /// <summary>
        /// Gets or sets the list of selected file details that will be uploaded.
        /// </summary>
        /// <value>
        /// A <see cref="List{FileInfo}"/> containing the details of files to be uploaded, or <c>null</c> if no files are selected.
        /// </value>
        /// <remarks>
        /// This property contains information about all files that are scheduled for upload, including
        /// file names, sizes, types, and other metadata. This information can be used for validation,
        /// progress tracking, or preprocessing before the upload begins.
        /// </remarks>
        [JsonPropertyName("filesData")]
        public List<FileInfo> FilesData { get; set; } = [];

        /// <summary>
        /// Gets or sets additional custom data in key-value pair format to be sent with the file upload request.
        /// </summary>
        /// <value>
        /// An object containing key-value pairs of additional data, or <c>null</c> if no custom data is provided.
        /// </value>
        /// <remarks>
        /// This property allows developers to include additional parameters or metadata with the file upload
        /// request. Common uses include authentication tokens, user identifiers, or other application-specific
        /// data required by the server-side upload handler.
        /// </remarks>
        /// <example>
        /// The following example shows how to add authorization headers to the upload request:
        /// <code><![CDATA[
        /// <SfUploader BeforeUpload="@BeforeUploadHandler">
        /// </SfUploader>
        /// @code {
        /// public void BeforeUploadHandler(BeforeUploadEventArgs args) {
        ///    var accessToken = "Authorization_token";
        ///    args.CurrentRequest = new List<object> { new { Authorization = accessToken } };
        /// }
        /// }
        /// ]]></code>
        /// </example>
        [DefaultValue(null)]
        [JsonPropertyName("customFormData")]
        public object? CustomFormData { get; set; }
    }

    /// <summary>
    /// Provides information about the <see cref="SfUploader.OnCancel"/> event callback.
    /// </summary>
    /// <remarks>
    /// This event is triggered when a chunk file upload operation is canceled. It provides information
    /// about the cancellation status and the file that was being uploaded when the operation was canceled.
    /// This event allows developers to handle cancellation scenarios and take appropriate action.
    /// </remarks>
    public class CancelEventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether the chunk file upload cancel action has been canceled.
        /// </summary>
        /// <value>
        /// <c>true</c> if the cancel action has been canceled; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// This property allows you to prevent the cancellation of a file upload operation by setting
        /// it to <c>true</c>. This provides an opportunity to implement conditional cancellation logic
        /// based on application requirements.
        /// </remarks>
        [JsonPropertyName("cancel")]
        public bool Cancel { get; set; }

        /// <summary>
        /// Gets or sets the original event arguments associated with the cancel operation.
        /// </summary>
        /// <value>
        /// An object containing the original event arguments for the current event, or <c>null</c> if no event arguments are available.
        /// </value>
        /// <remarks>
        /// This property provides access to the underlying event arguments that triggered the cancel operation.
        /// <strong>Note:</strong> This property is obsolete and should no longer be used in new implementations.
        /// </remarks>
        [Obsolete("The Event property is obsolete and will be removed in a future version. Use the specific event argument properties (Cancel, FileData) instead to access cancellation information.", false)]
        [JsonPropertyName("event")]
        public object? Event { get; set; }

        /// <summary>
        /// Gets or sets the details of the file that was canceled during upload.
        /// </summary>
        /// <value>
        /// A <see cref="FileInfo"/> object containing the details of the canceled file, or <c>null</c> if no file data is available.
        /// </value>
        /// <remarks>
        /// This property contains comprehensive information about the file that was being uploaded when
        /// the cancel operation occurred, including file name, size, type, and upload status. This information
        /// can be used for logging, user feedback, or cleanup operations.
        /// </remarks>
        [JsonPropertyName("fileData")]
        public FileInfo FileData { get; set; } = new();
    }

    /// <summary>
    /// Provides information about the <see cref="SfUploader.OnClear"/> event callback.
    /// </summary>
    /// <remarks>
    /// This event is triggered before the file list is cleared when the user clicks the "clear" button.
    /// It provides an opportunity to cancel the clear operation or perform custom actions before
    /// the files are removed from the upload queue.
    /// </remarks>
    public class ClearingEventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether the file list clear action has been canceled.
        /// </summary>
        /// <value>
        /// <c>true</c> to cancel the file list clear operation; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// Setting this property to <c>true</c> will prevent the file list clear operation from proceeding.
        /// This allows for conditional clearing based on custom logic, user confirmation, or validation requirements.
        /// </remarks>
        [JsonPropertyName("cancel")]
        public bool Cancel { get; set; }

        /// <summary>
        /// Gets or sets the list of file details that will be cleared from the file list.
        /// </summary>
        /// <value>
        /// A <see cref="List{FileInfo}"/> containing the details of all files that will be removed from the file list, or <c>null</c> if no files are available.
        /// </value>
        /// <remarks>
        /// This property contains information about all files that are scheduled for removal from the upload queue.
        /// The file details include names, sizes, types, and current status information, which can be used
        /// for logging, user confirmation, or implementing custom clear logic.
        /// </remarks>
        [JsonPropertyName("filesData")]
        public List<FileInfo> FilesData { get; set; } = [];
    }

    /// <summary>
    /// Provides information about the <see cref="SfUploader.OnChunkFailure"/> and <see cref="SfUploader.OnFailure"/> events callback.
    /// </summary>
    /// <remarks>
    /// This event is triggered when file upload operations fail, either for individual chunks or entire files.
    /// It provides comprehensive information about the failure, including details about files that can be
    /// retried and the underlying cause of the failure. This event inherits all properties from
    /// <see cref="SuccessEventArgs"/> to provide complete context about the failed operation.
    /// </remarks>
    public class FailureEventArgs : SuccessEventArgs
    {
        /// <summary>
        /// Gets or sets the details about files that can be retried after upload failure.
        /// </summary>
        /// <value>
        /// An array of <see cref="FileInfo"/> objects representing files that are eligible for retry, or <c>null</c> if no files can be retried.
        /// </value>
        /// <remarks>
        /// This property contains information about files that failed to upload but can be retried based on
        /// the uploader's retry configuration. The retry mechanism helps handle temporary network issues
        /// or server unavailability by automatically attempting to upload failed files again.
        /// </remarks>
        [JsonPropertyName("retryFiles")]
        public FileInfo[]? RetryFiles { get; set; }
    }

    /// <summary>
    /// Provides comprehensive information about the selected files' details in the <see cref="SfUploader"/> component.
    /// </summary>
    /// <remarks>
    /// This class encapsulates all essential information about a file selected for upload, including metadata
    /// such as name, size, type, and status information. It serves as the primary data structure for tracking
    /// file information throughout the upload lifecycle, from selection through completion or failure.
    /// </remarks>
    public class FileInfo
    {
        /// <summary>
        /// Gets or sets the file path of the selected file as specified by the browser.
        /// </summary>
        /// <value>
        /// A string representing the file path, or <c>null</c> if the path is not available.
        /// </value>
        /// <remarks>
        /// This property contains the browser-provided file path information. Note that for security reasons,
        /// modern browsers may not provide the complete file system path, and this value should not be relied
        /// upon for file system operations.
        /// </remarks>
        [JsonPropertyName("fileSource")]
        public string FileSource { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the unique identifier of the file as generated by the uploader component.
        /// </summary>
        /// <value>
        /// A string containing the unique file identifier, or <c>null</c> if no ID has been assigned.
        /// </value>
        /// <remarks>
        /// This property provides a unique identifier for the file within the context of the uploader component.
        /// It is useful for tracking files throughout their lifecycle and for associating events with specific files.
        /// </remarks>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the file as specified by the browser.
        /// </summary>
        /// <value>
        /// A string containing the file name including extension, or <c>null</c> if the name is not available.
        /// </value>
        /// <remarks>
        /// This property contains the original filename as provided by the browser, including the file extension.
        /// It represents the name of the file as it exists on the user's file system.
        /// </remarks>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the raw file object containing the complete file data and metadata.
        /// </summary>
        /// <value>
        /// An object containing the raw file data and browser-provided metadata, or <c>null</c> if no raw file data is available.
        /// </value>
        /// <remarks>
        /// This property provides access to the underlying browser File object that contains the actual file
        /// content and additional metadata. It can be used for advanced file processing scenarios where direct
        /// access to the file content is required.
        /// </remarks>
        [DefaultValue(null)]
        [JsonPropertyName("rawFile")]
        public object? RawFile { get; set; }

        /// <summary>
        /// Gets or sets the size of the file in bytes as reported by the browser.
        /// </summary>
        /// <value>
        /// A double value representing the file size in bytes.
        /// </value>
        /// <remarks>
        /// This property contains the file size as determined by the browser. It is useful for validation,
        /// progress tracking, and displaying file information to users. The size is measured in bytes and
        /// can be used to enforce file size limits.
        /// </remarks>
        [JsonPropertyName("size")]
        public double Size { get; set; }

        /// <summary>
        /// Gets or sets the current status of the file in the upload process.
        /// </summary>
        /// <value>
        /// A string describing the current file status, or <c>null</c> if no status is available.
        /// </value>
        /// <remarks>
        /// This property indicates the current state of the file within the upload process. It provides
        /// human-readable status information that can be displayed to users to inform them of the
        /// file's current state in the upload workflow.
        /// </remarks>
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the current state code of the file in the upload lifecycle.
        /// </summary>
        /// <value>
        /// A string representing the status code such as "Failed", "Canceled", "Selected", "Uploaded", or "Uploading", or <c>null</c> if no status code is set.
        /// </value>
        /// <remarks>
        /// This property provides a standardized status code that represents the file's current state in the
        /// upload process. Common values include:
        /// <list type="bullet">
        /// <item><description><strong>Selected</strong> - File has been selected but not yet uploaded</description></item>
        /// <item><description><strong>Uploading</strong> - File upload is currently in progress</description></item>
        /// <item><description><strong>Uploaded</strong> - File has been successfully uploaded</description></item>
        /// <item><description><strong>Failed</strong> - File upload has failed</description></item>
        /// <item><description><strong>Canceled</strong> - File upload has been canceled</description></item>
        /// </list>
        /// </remarks>
        [JsonPropertyName("statusCode")]
        public string StatusCode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the MIME type of the file as determined by the browser.
        /// </summary>
        /// <value>
        /// A string representing the MIME type (e.g., "image/jpeg", "application/pdf"), or <c>null</c> if the type cannot be determined.
        /// </value>
        /// <remarks>
        /// This property contains the MIME type of the file as determined by the browser based on the file
        /// extension and content. If the browser cannot determine the file type, this property may be null
        /// or an empty string. This information is useful for file type validation and processing decisions.
        /// </remarks>
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the MIME content type of the file as specified by the browser.
        /// </summary>
        /// <value>
        /// A string representing the MIME content type, or <c>null</c> if the content type is not available.
        /// </value>
        /// <remarks>
        /// This property provides an alternative or additional MIME type specification for the file.
        /// It may contain more specific content type information or serve as a fallback when the
        /// primary Type property is not available.
        /// </remarks>
        [JsonPropertyName("mimeContentType")]
        public string MimeContentType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the last modified date of the file as reported by the browser.
        /// </summary>
        /// <value>
        /// A <see cref="DateTime"/> value representing when the file was last modified.
        /// </value>
        /// <remarks>
        /// This property contains the timestamp of when the file was last modified on the user's file system.
        /// This information can be useful for file versioning, synchronization scenarios, or providing
        /// additional context about the file to users.
        /// </remarks>
        [JsonPropertyName("lastModifiedDate")]
        public DateTime LastModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the validation error messages associated with this file.
        /// </summary>
        /// <value>
        /// A <see cref="ValidationMessages"/> object containing validation error messages, or <c>null</c> if no validation errors exist.
        /// </value>
        /// <remarks>
        /// This property contains validation error messages that are generated when the file fails to meet
        /// the specified validation criteria, such as file size limits or allowed file type restrictions.
        /// These messages can be displayed to users to help them understand why a file was rejected.
        /// </remarks>
        [JsonPropertyName("validationMessages")]
        public ValidationMessages ValidationMessages { get; set; } = new();
    }

    /// <summary>
    /// Provides information about the <see cref="SfUploader.OnFileListRender"/> event callback.
    /// </summary>
    /// <remarks>
    /// This event is triggered before rendering each file item in the file list. It provides access to
    /// the DOM element, file information, and rendering context, allowing developers to customize the
    /// appearance and behavior of individual file items in the uploader's file list.
    /// </remarks>
    public class FileListRenderingEventArgs
    {
        /// <summary>
        /// Gets or sets the details of the file currently being rendered in the file list.
        /// </summary>
        /// <value>
        /// A <see cref="FileInfo"/> object containing the file details, or <c>null</c> if no file information is available.
        /// </value>
        /// <remarks>
        /// This property contains comprehensive information about the file being rendered, including its name,
        /// size, type, status, and other metadata. This information can be used to customize the file item's
        /// appearance based on file properties or to implement custom rendering logic.
        /// </remarks>
        [JsonPropertyName("fileInfo")]
        public FileInfo FileInfo { get; set; } = new();

        /// <summary>
        /// Gets or sets the zero-based index of the file item in the file list.
        /// </summary>
        /// <value>
        /// A double value representing the index position of the file item in the file list.
        /// </value>
        /// <remarks>
        /// This property indicates the position of the current file item within the complete file list.
        /// It can be used for implementing alternating row styles, numbering file items, or applying
        /// position-specific customizations to the file list display.
        /// </remarks>
        [JsonPropertyName("index")]
        public double Index { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the file is preloaded in the uploader component.
        /// </summary>
        /// <value>
        /// <c>true</c> if the file is preloaded; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// Preloaded files are those that are configured to display in the file list when the uploader
        /// component is initialized, typically representing files that have already been uploaded to the
        /// server. This property helps distinguish between newly selected files and existing uploaded files.
        /// </remarks>
        [JsonPropertyName("isPreload")]
        public bool IsPreload { get; set; }
    }

    /// <summary>
    /// Provides information about the <see cref="SfUploader.Paused"/> and <see cref="SfUploader.OnResume"/> events callback.
    /// </summary>
    /// <remarks>
    /// This event is triggered when chunk-based file upload operations are paused or resumed. It provides
    /// detailed information about the chunk upload progress, including the current chunk being processed,
    /// total chunk count, and the file being uploaded. This enables developers to implement custom pause
    /// and resume functionality with accurate progress tracking.
    /// </remarks>
    public class PauseResumeEventArgs
    {
        /// <summary>
        /// Gets or sets the total number of chunks for the file being uploaded.
        /// </summary>
        /// <value>
        /// A double value representing the total count of chunks that the file is divided into for upload.
        /// </value>
        /// <remarks>
        /// This property indicates how many chunks the current file has been divided into for the upload
        /// process. The chunk count depends on the file size and the configured chunk size in the uploader
        /// settings. This information is useful for calculating upload progress and estimating completion time.
        /// </remarks>
        [JsonPropertyName("chunkCount")]
        public double ChunkCount { get; set; }

        /// <summary>
        /// Gets or sets the index of the file chunk that is currently paused or resumed.
        /// </summary>
        /// <value>
        /// A double value representing the zero-based index of the chunk that is paused or resumed.
        /// </value>
        /// <remarks>
        /// This property identifies the specific chunk within the file that is affected by the pause or
        /// resume operation. Combined with the total chunk count, it provides precise information about
        /// the upload progress and which portion of the file is currently being processed.
        /// </remarks>
        [JsonPropertyName("chunkIndex")]
        public double ChunkIndex { get; set; }

        /// <summary>
        /// Gets or sets the size of each chunk in bytes for the uploaded file.
        /// </summary>
        /// <value>
        /// A double value representing the chunk size in bytes.
        /// </value>
        /// <remarks>
        /// This property specifies the size of each chunk that the file is divided into for upload.
        /// The chunk size is configured in the uploader settings and determines how the file is split
        /// for sequential upload. Larger chunk sizes result in fewer chunks but larger individual transfers.
        /// </remarks>
        [JsonPropertyName("chunkSize")]
        public double ChunkSize { get; set; }

        /// <summary>
        /// Gets or sets the original event arguments associated with the pause or resume operation.
        /// </summary>
        /// <value>
        /// An <see cref="EventArgs"/> object containing the original event arguments, or <c>null</c> if no event arguments are available.
        /// </value>
        /// <remarks>
        /// This property provides access to the underlying event arguments that triggered the pause or resume
        /// operation. <strong>Note:</strong> This property is obsolete and should no longer be used in new
        /// implementations. Use other properties of this class for accessing pause/resume information.
        /// </remarks>
        [Obsolete("The Event property is obsolete and will be removed in a future version. Use the specific event argument properties (ChunkCount, ChunkIndex, ChunkSize, File) instead to access pause/resume information.", false)]
        [JsonPropertyName("event")]
        public EventArgs Event { get; set; } = new();

        /// <summary>
        /// Gets or sets the details of the file that is being paused or resumed.
        /// </summary>
        /// <value>
        /// A <see cref="FileInfo"/> object containing the file details, or <c>null</c> if no file information is available.
        /// </value>
        /// <remarks>
        /// This property contains comprehensive information about the file affected by the pause or resume
        /// operation, including file name, size, type, and current upload status. This information is
        /// essential for tracking upload progress and providing user feedback during pause/resume operations.
        /// </remarks>
        [JsonPropertyName("file")]
        public FileInfo File { get; set; } = new();
    }

    /// <summary>
    /// Provides information about the <see cref="SfUploader.Progressing"/> event callback.
    /// </summary>
    /// <remarks>
    /// This event is triggered during file upload operations to provide real-time progress information.
    /// It enables developers to display upload progress indicators, calculate completion percentages,
    /// and provide user feedback during file transfer operations. The event provides both the amount
    /// of data transferred and the total file size for accurate progress calculation.
    /// </remarks>
    public class ProgressEventArgs
    {
        /// <summary>
        /// Gets or sets the original event arguments associated with the progress event.
        /// </summary>
        /// <value>
        /// An object containing the original event arguments, or <c>null</c> if no event arguments are available.
        /// </value>
        /// <remarks>
        /// This property provides access to the underlying event arguments that triggered the progress event.
        /// <strong>Note:</strong> This property is obsolete and should no longer be used in new implementations.
        /// Use other properties of this class for accessing progress information.
        /// </remarks>
        [Obsolete("The E property is obsolete and will be removed in a future version. Use the specific event argument properties (LengthComputable, Loaded, Total, File) instead to access progress information.", false)]
        [JsonPropertyName("e")]
        public object E { get; set; } = new();

        /// <summary>
        /// Gets or sets a value indicating whether the file upload progress is computable.
        /// </summary>
        /// <value>
        /// <c>true</c> if the progress can be calculated; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// This property indicates whether the upload progress can be accurately calculated based on the
        /// available information. When <c>true</c>, the <see cref="Loaded"/> and <see cref="Total"/> properties
        /// provide reliable data for progress calculation. When <c>false</c>, progress cannot be determined,
        /// and progress indicators should show indeterminate progress.
        /// </remarks>
        [JsonPropertyName("lengthComputable")]
        public bool LengthComputable { get; set; }

        /// <summary>
        /// Gets or sets the amount of data that has been uploaded so far.
        /// </summary>
        /// <value>
        /// A decimal value representing the number of bytes that have been successfully uploaded.
        /// </value>
        /// <remarks>
        /// This property indicates the current progress of the file upload operation in bytes. It can be
        /// used in conjunction with the <see cref="Total"/> property to calculate the upload percentage
        /// and display progress indicators to users. The value increases as more data is transferred.
        /// </remarks>
        [JsonPropertyName("loaded")]
        public decimal Loaded { get; set; }

        /// <summary>
        /// Gets or sets the total size of the file being uploaded.
        /// </summary>
        /// <value>
        /// A decimal value representing the total file size in bytes.
        /// </value>
        /// <remarks>
        /// This property represents the complete size of the file being uploaded. Combined with the
        /// <see cref="Loaded"/> property, it enables calculation of upload progress percentages and
        /// estimated completion times. This value remains constant throughout the upload process.
        /// </remarks>
        [JsonPropertyName("total")]
        public decimal Total { get; set; }

        /// <summary>
        /// Gets or sets the details about the file being uploaded.
        /// </summary>
        /// <value>
        /// A <see cref="FileInfo"/> object containing comprehensive file details, or <c>null</c> if no file information is available.
        /// </value>
        /// <remarks>
        /// This property contains detailed information about the file currently being uploaded, including
        /// file name, size, type, and upload status. This information is useful for displaying file-specific
        /// progress information and managing multiple file uploads simultaneously.
        /// </remarks>
        [JsonPropertyName("file")]
        public FileInfo File { get; set; } = new();

        /// <summary>
        /// Gets the stream containing the uploaded file data.
        /// </summary>
        /// <value>
        /// A <see cref="Stream"/> object containing the file data, or <c>null</c> if no stream is available.
        /// </value>
        /// <remarks>
        /// <para>This property provides access to the file data stream during the upload process. It can be used
        /// for custom processing of file content, validation, or streaming operations. The stream represents
        /// the actual file data being transferred during the upload operation.</para>
        /// <para><strong>Important:</strong> The caller is responsible for properly disposing this stream when finished
        /// to prevent memory leaks. Use <c>using</c> statements or call <c>Dispose()</c> explicitly.</para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// private async Task OnProgressHandler(ProgressEventArgs args)
        /// {
        ///     if (args.Stream != null)
        ///     {
        ///         using (args.Stream)
        ///         {
        ///             // Process the stream
        ///             await ProcessStreamAsync(args.Stream);
        ///         }
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [JsonIgnore]
        public Stream? Stream { get; internal set; }

        /// <summary>
        /// Gets or sets the type of upload operation being performed.
        /// </summary>
        /// <value>
        /// A string describing the upload operation type, or <c>null</c> if no operation type is specified.
        /// </value>
        /// <remarks>
        /// This property indicates the specific type of upload operation that is currently in progress.
        /// It helps distinguish between different types of upload operations, such as initial upload,
        /// retry operations, or chunk-based uploads, allowing for operation-specific handling and reporting.
        /// </remarks>
        [JsonPropertyName("operation")]
        public string Operation { get; set; } = string.Empty;
    }

    /// <summary>
    /// Provides information about the <see cref="SfUploader.OnRemove"/> event callback.
    /// </summary>
    /// <remarks>
    /// This event is triggered when files are being removed from the uploader component. It provides
    /// comprehensive information about the removal operation, including the files being removed,
    /// request details, and cancellation options. This event allows developers to implement custom
    /// removal logic, validate removal requests, or cancel removal operations based on business rules.
    /// </remarks>
    public class RemovingEventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether the file remove action has been canceled.
        /// </summary>
        /// <value>
        /// <c>true</c> to cancel the file removal operation; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// Setting this property to <c>true</c> will prevent the file removal operation from proceeding.
        /// This allows for conditional removal based on custom validation, user confirmation, or
        /// business logic requirements.
        /// </remarks>
        [JsonPropertyName("cancel")]
        public bool Cancel { get; set; }

        /// <summary>
        /// Gets or sets the XMLHttpRequest instance associated with the file removal operation.
        /// </summary>
        /// <value>
        /// An object containing XMLHttpRequest details, or <c>null</c> if no request is associated.
        /// </value>
        /// <remarks>
        /// This property provides access to the underlying HTTP request object that will be used for the
        /// file removal operation. It allows for customization of request headers, authentication, and
        /// other HTTP-specific settings before the removal request is sent to the server.
        /// </remarks>
        [DefaultValue(null)]
        [JsonPropertyName("currentRequest")]
        public object? CurrentRequest { get; set; }

        /// <summary>
        /// Gets or sets additional custom data in key-value pair format to be submitted with the removal request.
        /// </summary>
        /// <value>
        /// An object containing key-value pairs of additional data, or <c>null</c> if no custom data is provided.
        /// </value>
        /// <remarks>
        /// This property allows developers to include additional parameters or metadata with the file removal
        /// request. Common uses include authentication tokens, user identifiers, or other application-specific
        /// data required by the server-side removal handler.
        /// </remarks>
        [DefaultValue(null)]
        [JsonPropertyName("customFormData")]
        public object? CustomFormData { get; set; }

        /// <summary>
        /// Gets or sets the original event arguments associated with the removal operation.
        /// </summary>
        /// <value>
        /// An object containing the original event arguments for the current event, or <c>null</c> if no event arguments are available.
        /// </value>
        /// <remarks>
        /// This property provides access to the underlying event arguments that triggered the removal operation.
        /// <strong>Note:</strong> This property is obsolete and should no longer be used in new implementations.
        /// Use other properties of this class for accessing removal information.
        /// </remarks>
        [Obsolete("The Event property is obsolete and will be removed in a future version. Use the specific event argument properties (Cancel, FilesData, CurrentRequest, CustomFormData) instead to access removal information.", false)]
        [JsonPropertyName("event")]
        public object? Event { get; set; }

        /// <summary>
        /// Gets or sets the list of file details that will be removed from the server.
        /// </summary>
        /// <value>
        /// A <see cref="List{FileInfo}"/> containing the details of files to be removed, or <c>null</c> if no files are specified.
        /// </value>
        /// <remarks>
        /// This property contains information about all files that are scheduled for removal, including
        /// file names, sizes, and other metadata. This information can be used for validation, logging,
        /// or displaying confirmation messages to users before removal.
        /// </remarks>
        [JsonPropertyName("filesData")]
        public List<FileInfo> FilesData { get; set; } = [];

        /// <summary>
        /// Gets or sets a value indicating whether the selected raw file data should be sent to the server during removal.
        /// </summary>
        /// <value>
        /// <c>true</c> to send the raw file data to the server; otherwise, <c>false</c> to send only the file name.
        /// </value>
        /// <remarks>
        /// When set to <c>true</c>, the complete file information including binary data is sent to the server.
        /// When set to <c>false</c>, only the file metadata (such as filename) is sent. Setting this to <c>false</c>
        /// can improve performance when only file identification is needed for removal operations.
        /// </remarks>
        [JsonPropertyName("postRawFile")]
        public bool PostRawFile { get; set; }
    }

    /// <summary>
    /// Provides information about HTTP response details for uploader operations.
    /// </summary>
    /// <remarks>
    /// This class encapsulates the HTTP response information received from server-side operations
    /// during file upload, removal, or other uploader-related requests. It provides comprehensive
    /// details about the server response, including headers, status codes, and response content,
    /// enabling developers to handle server responses appropriately.
    /// </remarks>
    public class ResponseEventArgs
    {
        /// <summary>
        /// Gets or sets the HTTP response headers received from the server.
        /// </summary>
        /// <value>
        /// A string containing the response headers, or <c>null</c> if no headers are available.
        /// </value>
        /// <remarks>
        /// This property contains the HTTP headers returned by the server in response to the upload
        /// or removal request. The headers provide additional metadata about the response and can
        /// be used for authentication, content type verification, or custom application logic.
        /// </remarks>
        [JsonPropertyName("headers")]
        public string Headers { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the current ready state of the HTTP response.
        /// </summary>
        /// <value>
        /// An object representing the ready state of the response, or <c>null</c> if the state is not available.
        /// </value>
        /// <remarks>
        /// This property indicates the current state of the HTTP request/response cycle, typically
        /// corresponding to XMLHttpRequest ready states (0-4). It helps track the progress of the
        /// request from initiation to completion.
        /// </remarks>
        [JsonPropertyName("readyState")]
        public object? ReadyState { get; set; }

        /// <summary>
        /// Gets or sets the HTTP status code returned by the server.
        /// </summary>
        /// <value>
        /// An object representing the HTTP status code, or <c>null</c> if the status code is not available.
        /// </value>
        /// <remarks>
        /// This property contains the HTTP status code (such as 200, 404, 500) returned by the server.
        /// The status code indicates the success or failure of the request and helps determine the
        /// appropriate handling of the server response.
        /// </remarks>
        [JsonPropertyName("statusCode")]
        public object? StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the HTTP status text associated with the status code.
        /// </summary>
        /// <value>
        /// A string containing the status text (e.g., "OK", "Not Found", "Internal Server Error"), or <c>null</c> if not available.
        /// </value>
        /// <remarks>
        /// This property provides a human-readable description of the HTTP status code returned by
        /// the server. It complements the status code with descriptive text that can be useful for
        /// logging, debugging, or user feedback purposes.
        /// </remarks>
        [JsonPropertyName("statusText")]
        public string StatusText { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the response content received from the server.
        /// </summary>
        /// <value>
        /// A string containing the response body content, or <c>null</c> if no response content is available.
        /// </value>
        /// <remarks>
        /// This property contains the actual response data sent by the server, which may include
        /// JSON data, HTML content, or plain text messages. This content is typically used to
        /// determine the success or failure details of the upload or removal operation.
        /// </remarks>
        [JsonPropertyName("responseText")]
        public string ResponseText { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether credentials are included in cross-origin requests.
        /// </summary>
        /// <value>
        /// <c>true</c> if credentials were included in the request; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// This property indicates whether the HTTP request included credentials (such as cookies,
        /// authorization headers, or client certificates) when making cross-origin requests. This
        /// information is important for understanding the authentication context of the response.
        /// </remarks>
        [JsonPropertyName("withCredentials")]
        public bool WithCredentials { get; set; }
    }

    /// <summary>
    /// Provides information about the <see cref="SfUploader.FileSelected"/> event callback.
    /// </summary>
    /// <remarks>
    /// This event is triggered after files are selected or dropped for upload. It provides comprehensive
    /// information about the selected files, allows cancellation of the selection, and enables modification
    /// of the file list before processing. This event is crucial for implementing file validation,
    /// preprocessing, and custom selection logic.
    /// </remarks>
    public class SelectedEventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether the file selection action has been canceled.
        /// </summary>
        /// <value>
        /// <c>true</c> to cancel the file selection operation; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// Setting this property to <c>true</c> will prevent the selected files from being added to the
        /// upload queue. This allows for conditional file selection based on validation rules, file type
        /// restrictions, or other business logic requirements.
        /// </remarks>
        [JsonPropertyName("cancel")]
        public bool Cancel { get; set; }

        /// <summary>
        /// Gets or sets the XMLHttpRequest instance associated with the file selection operation.
        /// </summary>
        /// <value>
        /// An object containing XMLHttpRequest details, or <c>null</c> if no request is associated.
        /// </value>
        /// <remarks>
        /// This property provides access to the current HTTP request headers that will be used for
        /// subsequent upload operations. It allows for customization of request headers, authentication,
        /// and other HTTP-specific settings before the upload process begins.
        /// </remarks>
        [DefaultValue(null)]
        [JsonPropertyName("currentRequest")]
        public object? CurrentRequest { get; set; }

        /// <summary>
        /// Gets or sets additional custom data in key-value pair format to be submitted with the upload action.
        /// </summary>
        /// <value>
        /// An object containing key-value pairs of additional data, or <c>null</c> if no custom data is provided.
        /// </value>
        /// <remarks>
        /// This property allows developers to include additional parameters or metadata with subsequent
        /// upload requests. Common uses include authentication tokens, user identifiers, or other
        /// application-specific data required by the server-side upload handler.
        /// </remarks>
        [JsonPropertyName("customFormData")]
        public object? CustomFormData { get; set; }

        /// <summary>
        /// Gets or sets the original event arguments associated with the file selection operation.
        /// </summary>
        /// <value>
        /// An object containing the original event arguments for the current event, or <c>null</c> if no event arguments are available.
        /// </value>
        /// <remarks>
        /// This property provides access to the underlying event arguments that triggered the file selection.
        /// <strong>Note:</strong> This property is obsolete and should no longer be used in new implementations.
        /// Use other properties of this class for accessing file selection information.
        /// </remarks>
        [Obsolete("The Event property is obsolete and will be removed in a future version. Use the specific event argument properties (Cancel, FilesData, ModifiedFilesData, IsCanceled, IsModified) instead to access file selection information.", false)]
        [JsonPropertyName("event")]
        public object? Event { get; set; }

        /// <summary>
        /// Gets or sets the list of files selected for uploading.
        /// </summary>
        /// <value>
        /// A <see cref="List{FileInfo}"/> containing the details of selected files, or <c>null</c> if no files are selected.
        /// </value>
        /// <remarks>
        /// This property contains information about all files that were selected by the user, including
        /// file names, sizes, types, and other metadata. This information can be used for validation,
        /// preprocessing, or displaying file information to users before upload begins.
        /// </remarks>
        [JsonPropertyName("filesData")]
        public List<FileInfo> FilesData { get; set; } = [];

        /// <summary>
        /// Gets or sets a value indicating whether the file selection operation has been canceled.
        /// </summary>
        /// <value>
        /// <c>true</c> if the file selection has been canceled; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// This property indicates whether the file selection process was canceled, either by user action
        /// or programmatic intervention. It provides additional context about the selection state and
        /// can be used to determine appropriate handling of the selection event.
        /// </remarks>
        [JsonPropertyName("isCanceled")]
        public bool IsCanceled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the selected files list is generated based on modified data.
        /// </summary>
        /// <value>
        /// <c>true</c> if the files list has been modified; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// This property indicates whether the original file selection has been modified through custom
        /// logic or preprocessing. When <c>true</c>, the <see cref="ModifiedFilesData"/> property should
        /// be used instead of the original <see cref="FilesData"/> for generating the file list.
        /// </remarks>
        [JsonPropertyName("isModified")]
        public bool IsModified { get; set; }

        /// <summary>
        /// Gets or sets the modified file data used to generate the file items when <see cref="IsModified"/> is <c>true</c>.
        /// </summary>
        /// <value>
        /// A <see cref="List{FileInfo}"/> containing the modified file details, or <c>null</c> if no modifications were made.
        /// </value>
        /// <remarks>
        /// This property contains the modified file information that should be used for generating the
        /// file list when the original selection has been altered. The availability and content of this
        /// property depends on the value of the <see cref="IsModified"/> property. This enables custom
        /// file processing and filtering during the selection process.
        /// </remarks>
        [JsonPropertyName("modifiedFilesData")]
        public List<FileInfo> ModifiedFilesData { get; set; } = [];

        /// <summary>
        /// Gets or sets the progress interval step value for the progress bar.
        /// </summary>
        /// <value>
        /// A string representing the step value for progress updates, or <c>null</c> if no interval is specified.
        /// </value>
        /// <remarks>
        /// This property defines the interval at which progress events are triggered during file upload
        /// operations. It helps control the frequency of progress updates, allowing developers to balance
        /// between progress granularity and performance considerations.
        /// </remarks>
        [JsonPropertyName("progressInterval")]
        public string ProgressInterval { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of the original event that triggered the file selection.
        /// </summary>
        /// <value>
        /// A string representing the event type, or <c>null</c> if no type is specified.
        /// </value>
        /// <remarks>
        /// This property indicates the type of user interaction that triggered the file selection, such as
        /// drag-and-drop, browse button click, or programmatic selection. <strong>Note:</strong> This property
        /// is obsolete and should no longer be used in new implementations.
        /// </remarks>
        [Obsolete("The Type property is obsolete and will be removed in a future version. The event type information is no longer needed as the selection context is available through other properties.", false)]
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
    }

    /// <summary>
    /// Provides information about the <see cref="SfUploader.OnChunkSuccess"/> and <see cref="SfUploader.Success"/> events callback.
    /// </summary>
    /// <remarks>
    /// This event is triggered when file upload operations complete successfully, either for individual
    /// chunks (in chunk-based uploads) or entire files. It provides comprehensive information about the
    /// successful operation, including server response details, file information, and upload context.
    /// This event enables developers to handle successful uploads and implement post-upload processing.
    /// </remarks>
    public class SuccessEventArgs
    {
        /// <summary>
        /// Gets or sets the index of the successfully uploaded file chunk.
        /// </summary>
        /// <value>
        /// A double value representing the zero-based index of the chunk that was successfully uploaded.
        /// </value>
        /// <remarks>
        /// This property identifies the specific chunk within a file that was successfully uploaded when
        /// chunk-based upload is enabled. For non-chunked uploads, this value may be zero or not applicable.
        /// It helps track progress in multi-chunk upload scenarios and can be used for implementing
        /// custom progress indicators or resumable upload functionality.
        /// </remarks>
        [JsonPropertyName("chunkIndex")]
        public double ChunkIndex { get; set; }

        /// <summary>
        /// Gets or sets the size of the uploaded file chunk in bytes.
        /// </summary>
        /// <value>
        /// A double value representing the chunk size in bytes.
        /// </value>
        /// <remarks>
        /// This property specifies the size of the chunk that was successfully uploaded. For chunk-based
        /// uploads, this represents the size of the individual chunk. For non-chunked uploads, this may
        /// represent the entire file size. This information is useful for calculating upload progress
        /// and bandwidth utilization.
        /// </remarks>
        [JsonPropertyName("chunkSize")]
        public double ChunkSize { get; set; }

        /// <summary>
        /// Gets or sets the original event arguments associated with the success event.
        /// </summary>
        /// <value>
        /// An object containing the original event arguments, or <c>null</c> if no event arguments are available.
        /// </value>
        /// <remarks>
        /// This property provides access to the underlying event arguments that triggered the success event.
        /// <strong>Note:</strong> This property is obsolete and should no longer be used in new implementations.
        /// Use other properties of this class for accessing success information.
        /// </remarks>
        [Obsolete("The E property is obsolete and will be removed in a future version. Use the specific event argument properties (File, Response, ChunkIndex, TotalChunk, Operation) instead to access success information.", false)]
        [JsonPropertyName("e")]
        public object? E { get; set; }

        /// <summary>
        /// Gets or sets the original event arguments for the upload success operation.
        /// </summary>
        /// <value>
        /// An object containing the original event arguments for the current event, or <c>null</c> if no event arguments are available.
        /// </value>
        /// <remarks>
        /// This property provides access to the underlying event arguments that triggered the upload success.
        /// <strong>Note:</strong> This property is obsolete and should no longer be used in new implementations.
        /// Use other properties of this class for accessing upload success information.
        /// </remarks>
        [Obsolete("The Event property is obsolete and will be removed in a future version. Use the specific event argument properties (File, Response, ChunkIndex, TotalChunk, Operation) instead to access upload success information.", false)]
        [JsonPropertyName("event")]
        public object? Event { get; set; }

        /// <summary>
        /// Gets or sets the details about the successfully uploaded file.
        /// </summary>
        /// <value>
        /// A <see cref="FileInfo"/> object containing comprehensive file details, or <c>null</c> if no file information is available.
        /// </value>
        /// <remarks>
        /// This property contains detailed information about the file that was successfully uploaded,
        /// including file name, size, type, and final upload status. This information is essential
        /// for post-upload processing, user feedback, and maintaining upload records.
        /// </remarks>
        [JsonPropertyName("file")]
        public FileInfo File { get; set; } = new();

        /// <summary>
        /// Gets or sets the type of upload operation that was successfully completed.
        /// </summary>
        /// <value>
        /// A string describing the upload operation type, or <c>null</c> if no operation type is specified.
        /// </value>
        /// <remarks>
        /// This property indicates the specific type of upload operation that completed successfully.
        /// It helps distinguish between different types of upload operations, such as initial upload,
        /// retry operations, or chunk-based uploads, allowing for operation-specific handling and reporting.
        /// </remarks>
        [JsonPropertyName("operation")]
        public string Operation { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the server response details for the successful upload operation.
        /// </summary>
        /// <value>
        /// A <see cref="ResponseEventArgs"/> object containing the server response information, or <c>null</c> if no response details are available.
        /// </value>
        /// <remarks>
        /// This property contains comprehensive information about the server's response to the upload
        /// request, including status codes, headers, and response content. This information can be
        /// used for validation, logging, or implementing server-specific post-upload processing.
        /// </remarks>
        [JsonPropertyName("response")]
        public ResponseEventArgs Response { get; set; } = new();

        /// <summary>
        /// Gets or sets the status text describing the upload operation result.
        /// </summary>
        /// <value>
        /// A string containing the status description, or <c>null</c> if no status text is available.
        /// </value>
        /// <remarks>
        /// This property provides a human-readable description of the upload operation status.
        /// It typically contains success messages or additional information about the completed
        /// upload that can be displayed to users or used for logging purposes.
        /// </remarks>
        [JsonPropertyName("statusText")]
        public string StatusText { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the total number of chunks for the uploaded file.
        /// </summary>
        /// <value>
        /// A double value representing the total count of chunks that the file was divided into for upload.
        /// </value>
        /// <remarks>
        /// This property indicates the total number of chunks that the file was divided into for the
        /// upload process. Combined with the <see cref="ChunkIndex"/> property, it provides complete
        /// information about chunk-based upload progress and helps determine when all chunks of a
        /// file have been successfully uploaded.
        /// </remarks>
        [JsonPropertyName("totalChunk")]
        public double TotalChunk { get; set; }
    }

    /// <summary>
    /// Represents the details of uploaded files in the <see cref="SfUploader"/> component.
    /// </summary>
    /// <remarks>
    /// This class encapsulates comprehensive information about files that have been uploaded through
    /// the uploader component. It provides access to both the file metadata and the actual file content,
    /// enabling developers to process uploaded files, save them to storage, or perform validation and
    /// manipulation operations on the uploaded data.
    /// </remarks>
    public class UploadFiles
    {
        /// <summary>
        /// Gets the memory stream containing the selected file data.
        /// </summary>
        /// <value>
        /// A <see cref="MemoryStream"/> containing the file data, or <c>null</c> if no stream is available.
        /// </value>
        /// <remarks>
        /// This property provides access to the file content as a memory stream. <strong>Note:</strong> This property
        /// is obsolete and will be removed in a future version. Use the <see cref="File"/> property instead to access
        /// the uploaded file's stream through the <c>OpenReadStream()</c> method.
        /// </remarks>
        [Obsolete("The Stream property is obsolete and will be removed in a future version. Use the File property instead to access the uploaded file's stream via File.OpenReadStream().", false)]
        public MemoryStream Stream { get; internal set; } = new();

        /// <summary>
        /// Gets the detailed information about the selected file.
        /// </summary>
        /// <value>
        /// A <see cref="FileInfo"/> object containing comprehensive file details, or <c>null</c> if no file information is available.
        /// </value>
        /// <remarks>
        /// This property provides access to comprehensive metadata about the uploaded file, including
        /// name, size, type, upload status, and other properties. This information is essential for
        /// file validation, processing decisions, and user interface updates.
        /// </remarks>
        public FileInfo FileInfo { get; internal set; } = new();

        /// <summary>
        /// Gets the browser file data representing the uploaded file from the <see cref="SfUploader"/> component.
        /// </summary>
        /// <value>
        /// An <see cref="IBrowserFile"/> object representing the uploaded file, or <c>null</c> if no file is available.
        /// </value>
        /// <remarks>
        /// The <see cref="File"/> property is used to handle file uploads in Blazor applications. 
        /// It represents the file selected by the user in the browser and provides access to the file's metadata, 
        /// such as the file name, size, and content type. To read the contents of the uploaded file, 
        /// call the <c>OpenReadStream()</c> method of the <see cref="IBrowserFile"/> interface, which returns a stream that you can use 
        /// to read the file data. This is the recommended approach for accessing file content in modern implementations.
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to handle file uploads and save them to disk:
        /// <code><![CDATA[
        /// <SfUploader AutoUpload="true" ValueChange="@OnChange">
        /// </SfUploader>
        /// @code{
        ///     private async Task OnChange(UploadChangeEventArgs args)
        ///     {
        ///         try
        ///         {
        ///             foreach (var file in args.Files)
        ///             {
        ///                 var path = @"D:\" + file.FileInfo.Name;
        ///                 FileStream filestream = new FileStream(path, FileMode.Create, FileAccess.Write);
        ///                 await file.File.OpenReadStream(long.MaxValue).CopyToAsync(filestream);
        ///                 filestream.Close();
        ///             }
        ///         }
        ///         catch (Exception ex)
        ///         {
        ///             Console.WriteLine(ex.Message);
        ///         }
        ///     }
        ///   }
        /// ]]></code>
        /// </example>
        public IBrowserFile? File { get; internal set; }
    }

    /// <summary>
    /// Provides information about the <see cref="SfUploader.OnValueChange"/> event callback.
    /// </summary>
    /// <remarks>
    /// This event is triggered when changes occur in the uploaded file list by selecting or dropping files.
    /// It provides access to the complete list of files that have been processed by the uploader component,
    /// enabling developers to implement custom handling logic, file validation, and post-upload processing.
    /// This event is essential for scenarios where immediate access to uploaded file data is required.
    /// </remarks>
    public class UploadChangeEventArgs
    {
        /// <summary>
        /// Gets or sets the list of uploaded files' details from the <see cref="SfUploader"/> component.
        /// </summary>
        /// <value>
        /// A <see cref="List{UploadFiles}"/> containing the details of all uploaded files, or <c>null</c> if no files have been uploaded.
        /// </value>
        /// <remarks>
        /// This property contains comprehensive information about all files that have been uploaded through
        /// the uploader component. Each file in the list includes both metadata (through the FileInfo property)
        /// and access to the actual file content (through the File property). This enables complete processing
        /// of uploaded files, including validation, storage operations, and content manipulation.
        /// </remarks>
        [JsonPropertyName("files")]
        public List<UploadFiles>? Files { get; set; }
    }

    /// <summary>
    /// Provides information about the <see cref="SfUploader.OnChunkUploadStart"/> and <see cref="SfUploader.OnUploadStart"/> events callback.
    /// </summary>
    /// <remarks>
    /// This event is triggered when file upload operations begin, either for individual chunks (in chunk-based uploads)
    /// or entire files. It provides comprehensive information about the upload process, including cancellation options,
    /// request customization, and chunk-specific details. This event enables developers to implement custom upload
    /// logic, authentication, and progress tracking before the actual upload begins.
    /// </remarks>
    public class UploadingEventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether the file upload action has been canceled.
        /// </summary>
        /// <value>
        /// <c>true</c> to cancel the file upload operation; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// Setting this property to <c>true</c> will prevent the file upload operation from proceeding.
        /// This allows for conditional uploads based on custom validation, authentication checks,
        /// or other business logic requirements that may be evaluated at upload start.
        /// </remarks>
        [JsonPropertyName("cancel")]
        public bool Cancel { get; set; }

        /// <summary>
        /// Gets or sets the size of the chunk being uploaded in bytes.
        /// </summary>
        /// <value>
        /// A double value representing the chunk size in bytes.
        /// </value>
        /// <remarks>
        /// This property specifies the size of the individual chunk that is being uploaded when chunk-based
        /// upload is enabled. For non-chunked uploads, this may represent the entire file size.
        /// The chunk size is configured in the uploader settings and determines how large files are split
        /// for sequential upload operations.
        /// </remarks>
        [JsonPropertyName("chunkSize")]
        public double ChunkSize { get; set; }

        /// <summary>
        /// Gets or sets the index of the current chunk being uploaded when chunk upload is enabled.
        /// </summary>
        /// <value>
        /// A double value representing the zero-based index of the current chunk being uploaded.
        /// </value>
        /// <remarks>
        /// This property identifies the specific chunk within a file that is currently being uploaded
        /// when chunk-based upload is enabled. It provides precise information about upload progress
        /// and helps track which portion of the file is being processed. For non-chunked uploads,
        /// this value is typically zero.
        /// </remarks>
        [JsonPropertyName("currentChunkIndex")]
        public double CurrentChunkIndex { get; set; }

        /// <summary>
        /// Gets or sets the XMLHttpRequest instance associated with the upload action.
        /// </summary>
        /// <value>
        /// An object containing XMLHttpRequest details, or <c>null</c> if no request is associated.
        /// </value>
        /// <remarks>
        /// This property provides access to the underlying HTTP request object that will be used for the
        /// file upload operation. It allows for customization of request headers, authentication tokens,
        /// and other HTTP-specific settings before the upload request is sent to the server.
        /// </remarks>
        [DefaultValue(null)]
        [JsonPropertyName("currentRequest")]
        public object? CurrentRequest { get; set; }

        /// <summary>
        /// Gets or sets the details of the file that will be uploaded.
        /// </summary>
        /// <value>
        /// A <see cref="FileInfo"/> object containing comprehensive file details, or <c>null</c> if no file information is available.
        /// </value>
        /// <remarks>
        /// This property contains detailed information about the file being uploaded, including file name,
        /// size, type, and current upload status. This information is essential for implementing custom
        /// upload logic, validation, and progress tracking during the upload operation.
        /// </remarks>
        [JsonPropertyName("fileData")]
        public FileInfo FileData { get; set; } = new();

        /// <summary>
        /// Gets or sets additional custom data in key-value pair format to be submitted with the upload action.
        /// </summary>
        /// <value>
        /// An object containing key-value pairs of additional data, or <c>null</c> if no custom data is provided.
        /// </value>
        /// <remarks>
        /// This property allows developers to include additional parameters or metadata with the upload
        /// request. Common uses include authentication tokens, user identifiers, file categories, or other
        /// application-specific data required by the server-side upload handler for proper file processing.
        /// </remarks>
        [DefaultValue(null)]
        [JsonPropertyName("customFormData")]
        public object? CustomFormData { get; set; }
    }

    /// <summary>
    /// Defines validation error messages for file upload operations in the <see cref="SfUploader"/> component.
    /// </summary>
    /// <remarks>
    /// This class contains validation error messages that are generated when uploaded files fail to meet
    /// the specified validation criteria. These messages provide user-friendly feedback about why files
    /// were rejected during the upload process, helping users understand and correct validation issues
    /// such as file size restrictions or format requirements.
    /// </remarks>
    public class ValidationMessages
    {
        /// <summary>
        /// Gets or sets the validation error message displayed when a file exceeds the maximum allowed file size.
        /// </summary>
        /// <value>
        /// A string containing the maximum file size validation error message, or <c>null</c> if no message is set.
        /// </value>
        /// <remarks>
        /// This property contains the error message that is displayed to users when they attempt to upload
        /// a file that exceeds the size limit specified by the <see cref="SfUploader.MaxFileSize"/> property.
        /// The message helps users understand the file size restriction and take appropriate action to
        /// either reduce the file size or select a smaller file.
        /// </remarks>
        [JsonPropertyName("maxSize")]
        public string MaxSize { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the validation error message displayed when a file is smaller than the minimum required file size.
        /// </summary>
        /// <value>
        /// A string containing the minimum file size validation error message, or <c>null</c> if no message is set.
        /// </value>
        /// <remarks>
        /// This property contains the error message that is displayed to users when they attempt to upload
        /// a file that is smaller than the minimum size specified by the <see cref="SfUploader.MinFileSize"/> property.
        /// This validation helps prevent the upload of empty files or files that are too small to be meaningful
        /// for the intended application use case.
        /// </remarks>
        [JsonPropertyName("minSize")]
        public string MinSize { get; set; } = string.Empty;
    }

    /// <summary>
    /// Defines the asynchronous settings configuration for the <see cref="SfUploader"/> component.
    /// </summary>
    /// <remarks>
    /// This class provides configuration options for server-side file operations including upload and removal
    /// endpoints, chunk-based upload settings, and retry mechanisms. These settings enable robust file upload
    /// functionality with support for large files, network resilience, and server-side processing integration.
    /// </remarks>
    public class AsyncSettingsModel
    {
        /// <summary>
        /// Gets or sets the chunk size used to split large files for sequential upload to the server.
        /// </summary>
        /// <value>
        /// A double value representing the chunk size in bytes. The default value is 0 (disabled).
        /// </value>
        /// <remarks>
        /// This property specifies the size in bytes for splitting large files into smaller chunks for upload.
        /// When a value greater than 0 is specified, the uploader automatically enables chunk-based upload,
        /// which improves reliability for large file transfers and allows for resumable uploads. The chunk size
        /// should be chosen based on network conditions, server capabilities, and file sizes. Typical values
        /// range from 1MB to 10MB for optimal performance.
        /// </remarks>
        [DefaultValue(0)]
        [JsonPropertyName("chunkSize")]
        public double ChunkSize { get; set; }

        /// <summary>
        /// Gets or sets the server endpoint URL for file removal operations.
        /// </summary>
        /// <value>
        /// A string containing the removal endpoint URL, or an empty string if not configured.
        /// </value>
        /// <remarks>
        /// This property specifies the server URL that handles file removal requests. The endpoint must accept
        /// POST requests and should define a "RemoveFileNames" attribute to receive information about files
        /// to be removed. This property is optional; if not specified, file removal functionality will be disabled.
        /// The server endpoint should implement proper validation and security measures for file removal operations.
        /// </remarks>
        [DefaultValue("")]
        [JsonPropertyName("removeUrl")]
        public string RemoveUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the delay time in milliseconds before automatic retry attempts after upload failure.
        /// </summary>
        /// <value>
        /// A double value representing the delay time in milliseconds. The default value is 500 milliseconds.
        /// </value>
        /// <remarks>
        /// This property specifies the waiting period before the uploader attempts to retry a failed upload.
        /// The delay helps handle temporary network issues or server unavailability by allowing time for
        /// conditions to improve before retrying. A reasonable delay prevents overwhelming the server with
        /// rapid retry attempts while ensuring timely recovery from temporary failures.
        /// </remarks>
        [DefaultValue(UploaderDefaults.RetryAfterDelay)]
        [JsonPropertyName("retryAfterDelay")]
        public double RetryAfterDelay { get; set; } = UploaderDefaults.RetryAfterDelay;

        /// <summary>
        /// Gets or sets the maximum number of retry attempts for failed file uploads.
        /// </summary>
        /// <value>
        /// A double value representing the maximum retry count. The default value is 3.
        /// </value>
        /// <remarks>
        /// This property specifies how many times the uploader will attempt to retry a failed upload before
        /// giving up. Setting an appropriate retry count prevents infinite retry loops while providing sufficient
        /// opportunities to recover from temporary failures. The default value of 3 provides a good balance
        /// between resilience and preventing excessive server load from repeated failed attempts.
        /// </remarks>
        [DefaultValue(UploaderDefaults.RetryCount)]
        [JsonPropertyName("retryCount")]
        public double RetryCount { get; set; } = UploaderDefaults.RetryCount;

        /// <summary>
        /// Gets or sets the server endpoint URL for file upload operations.
        /// </summary>
        /// <value>
        /// A string containing the upload endpoint URL, or an empty string if not configured.
        /// </value>
        /// <remarks>
        /// This property specifies the server URL that receives and processes uploaded files. The endpoint must
        /// accept POST requests and should define parameters with the same input name used by the component.
        /// This property is essential for upload functionality; without it, upload operations cannot be performed.
        /// The server endpoint should implement proper file handling, validation, and security measures.
        /// </remarks>
        [DefaultValue("")]
        [JsonPropertyName("saveUrl")]
        public string SaveUrl { get; set; } = string.Empty;
    }

    /// <summary>
    /// Defines the button content configuration for the <see cref="SfUploader"/> component.
    /// </summary>
    /// <remarks>
    /// This class allows customization of the text and HTML content displayed on the uploader component's
    /// buttons. It provides control over the appearance and localization of the browse, clear, and upload
    /// buttons, enabling developers to adapt the component's interface to different languages, branding
    /// requirements, or user experience preferences.
    /// </remarks>
    public class ButtonsPropsModel
    {
        /// <summary>
        /// Gets or sets the text or HTML content for the browse button.
        /// </summary>
        /// <value>
        /// An object containing the button content (text or HTML), or <c>null</c> to use the default content.
        /// </value>
        /// <remarks>
        /// This property allows customization of the browse button's display content. The content can be
        /// plain text for simple labels or HTML markup for more complex button designs including icons
        /// or styled text. When set to <c>null</c>, the component uses its default browse button text,
        /// which may be localized based on the component's locale settings.
        /// </remarks>
        [DefaultValue(null)]
        [JsonPropertyName("browse")]
        public object? Browse { get; set; }

        /// <summary>
        /// Gets or sets the text or HTML content for the clear button.
        /// </summary>
        /// <value>
        /// An object containing the button content (text or HTML), or <c>null</c> to use the default content.
        /// </value>
        /// <remarks>
        /// This property allows customization of the clear button's display content. The clear button is
        /// used to remove all selected files from the upload queue. The content can be plain text or HTML
        /// markup to include icons or styling. When set to <c>null</c>, the component uses its default
        /// clear button text, which may be localized based on the component's locale settings.
        /// </remarks>
        [DefaultValue(null)]
        [JsonPropertyName("clear")]
        public object? Clear { get; set; }

        /// <summary>
        /// Gets or sets the text or HTML content for the upload button.
        /// </summary>
        /// <value>
        /// An object containing the button content (text or HTML), or <c>null</c> to use the default content.
        /// </value>
        /// <remarks>
        /// This property allows customization of the upload button's display content. The upload button
        /// initiates the file upload process when AutoUpload is disabled. The content can be plain text
        /// or HTML markup for enhanced visual design. When set to <c>null</c>, the component uses its
        /// default upload button text, which may be localized based on the component's locale settings.
        /// </remarks>
        [DefaultValue(null)]
        [JsonPropertyName("upload")]
        public object? Upload { get; set; }
    }

    /// <summary>
    /// Defines the basic file properties for preloaded files in the <see cref="SfUploader"/> component.
    /// </summary>
    /// <remarks>
    /// This class represents the essential properties of files that are preloaded in the uploader component,
    /// typically representing files that have already been uploaded to the server and should be displayed
    /// in the file list when the component initializes. These properties are the minimum required information
    /// needed to display and manage preloaded files in the uploader interface.
    /// </remarks>
    public class FilesPropModel
    {
        /// <summary>
        /// Gets or sets the name of the file including its extension.
        /// </summary>
        /// <value>
        /// A string containing the complete file name, or an empty string if not specified.
        /// </value>
        /// <remarks>
        /// This property specifies the display name of the preloaded file, including the file extension.
        /// The file name is used for display purposes in the file list and should match the actual
        /// file name as stored on the server. This is a required property for preloaded files to
        /// ensure proper identification and display in the uploader interface.
        /// </remarks>
        [DefaultValue("")]
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the size of the file in bytes.
        /// </summary>
        /// <value>
        /// A double value representing the file size in bytes. The default value is 0.
        /// </value>
        /// <remarks>
        /// This property specifies the size of the preloaded file in bytes. The file size information
        /// is used for display purposes in the file list and for validation against size restrictions.
        /// This is a required property for preloaded files to provide complete file information to users
        /// and enable proper file management functionality.
        /// </remarks>
        [DefaultValue(default(double))]
        [JsonPropertyName("size")]
        public double Size { get; set; } = default;

        /// <summary>
        /// Gets or sets the MIME type of the file.
        /// </summary>
        /// <value>
        /// A string containing the file's MIME type (e.g., "image/jpeg", "application/pdf"), or an empty string if not specified.
        /// </value>
        /// <remarks>
        /// This property specifies the MIME type of the preloaded file, which indicates the file's content type
        /// and format. The type information is used for file validation, proper handling, and display of
        /// appropriate file icons in the uploader interface. This is a required property for preloaded files
        /// to ensure proper file type recognition and processing.
        /// </remarks>
        [DefaultValue("")]
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
    }

    /// <summary>
    /// Defines the comprehensive configuration model for the Syncfusion Blazor Uploader component.
    /// </summary>
    /// <remarks>
    /// This class serves as the primary configuration model for the uploader component, containing all
    /// properties and event handlers that control the behavior, appearance, and functionality of the file
    /// upload component. It provides comprehensive options for customizing upload behavior, validation,
    /// styling, and user interaction patterns, enabling developers to create robust file upload solutions
    /// tailored to their specific requirements.
    /// </remarks>
    /// <example>
    /// The following example demonstrates basic configuration of the uploader component:
    /// <code><![CDATA[
    /// <SfUploader AutoUpload="true" 
    ///             AllowedExtensions=".jpg,.png,.pdf"
    ///             MaxFileSize="10485760"
    ///             Multiple="true"
    ///             BeforeUpload="OnBeforeUpload" Success="OnUploadSuccess">
    ///     <UploaderAsyncSettings SaveUrl="/api/upload" RemoveUrl="/api/remove" />
    /// </SfUploader>
    /// ]]></code>
    /// </example>
    public class UploaderModel
    {
        /// <summary>
        /// Gets or sets the event callback that is triggered after all selected files have been processed for upload.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{T}"/> that is invoked when all upload operations are completed, regardless of success or failure.
        /// </value>
        /// <remarks>
        /// This event is triggered after all selected files have completed their upload process, whether they
        /// succeeded or failed. It provides a comprehensive overview of the batch upload operation and is useful
        /// for implementing post-upload cleanup, user notifications, or summary reporting functionality.
        /// </remarks>
        [JsonIgnore]
        [JsonPropertyName("actionComplete")]
        public EventCallback<object> ActionComplete { get; set; }

        /// <summary>
        /// Gets or sets the event callback that is triggered before files are removed from the server.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{T}"/> that is invoked before file removal operations to allow confirmation or cancellation.
        /// </value>
        /// <remarks>
        /// This event is triggered when users attempt to remove uploaded files from the server. It provides
        /// an opportunity to implement confirmation dialogs, validate removal permissions, or cancel the
        /// removal operation based on business logic requirements.
        /// </remarks>
        [JsonIgnore]
        [JsonPropertyName("beforeRemove")]
        public EventCallback<object> BeforeRemove { get; set; }

        /// <summary>
        /// Gets or sets the event callback that is triggered before the upload process begins.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{T}"/> that is invoked before upload operations start, allowing request customization.
        /// </value>
        /// <remarks>
        /// This event is triggered before the file upload process begins and is commonly used to add
        /// additional parameters to the upload request, such as authentication tokens, metadata, or
        /// custom headers required by the server-side upload handler.
        /// </remarks>
        [JsonIgnore]
        [JsonPropertyName("beforeUpload")]
        public EventCallback<object> BeforeUpload { get; set; }

        /// <summary>
        /// Gets or sets the event callback that is triggered when chunk file upload operations are canceled.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{T}"/> that is invoked when chunk-based upload operations are canceled.
        /// </value>
        /// <remarks>
        /// This event is triggered specifically when chunk-based file upload operations are canceled by
        /// the user or system. It provides information about the cancellation context and allows for
        /// cleanup operations or user feedback regarding the canceled upload.
        /// </remarks>
        [JsonIgnore]
        [JsonPropertyName("canceling")]
        public EventCallback<object> Canceling { get; set; }

        /// <summary>
        /// Gets or sets the upload change event arguments when files are selected or dropped.
        /// </summary>
        /// <value>
        /// An <see cref="UploadChangeEventArgs"/> object containing information about file changes, or <c>null</c> if no changes have occurred.
        /// </value>
        /// <remarks>
        /// This property contains the event arguments for file selection and drop operations. It provides
        /// access to the list of files that have been selected or dropped, enabling custom handling of
        /// file changes and immediate access to uploaded file data.
        /// </remarks>
        [DefaultValue(null)]
        [JsonPropertyName("change")]
        public UploadChangeEventArgs? Change { get; set; }

        /// <summary>
        /// Gets or sets the failure event arguments when chunk file uploads fail.
        /// </summary>
        /// <value>
        /// A <see cref="FailureEventArgs"/> object containing failure information, or <c>null</c> if no failures have occurred.
        /// </value>
        /// <remarks>
        /// This property contains the event arguments for chunk upload failures. It provides detailed
        /// information about failed chunk operations, including retry options and error context, enabling
        /// robust error handling in chunk-based upload scenarios.
        /// </remarks>
        [DefaultValue(null)]
        [JsonPropertyName("chunkFailure")]
        public FailureEventArgs? ChunkFailure { get; set; }

        /// <summary>
        /// Gets or sets the success event arguments when chunk files are uploaded successfully.
        /// </summary>
        /// <value>
        /// A <see cref="SuccessEventArgs"/> object containing success information, or <c>null</c> if no successful operations have occurred.
        /// </value>
        /// <remarks>
        /// This property contains the event arguments for successful chunk upload operations. It provides
        /// detailed information about completed chunk uploads, including server responses and progress
        /// information, enabling precise tracking of chunk-based upload progress.
        /// </remarks>
        [DefaultValue(null)]
        [JsonPropertyName("chunkSuccess")]
        public SuccessEventArgs? ChunkSuccess { get; set; }

        /// <summary>
        /// Gets or sets the event callback that is triggered when chunk upload processes start.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{T}"/> that is invoked when individual chunk upload operations begin.
        /// </value>
        /// <remarks>
        /// This event is triggered for each chunk upload operation when chunk-based uploading is enabled.
        /// It is commonly used to add additional parameters to chunk upload requests or implement
        /// chunk-specific customization logic for large file uploads.
        /// </remarks>
        [JsonIgnore]
        [JsonPropertyName("chunkUploading")]
        public EventCallback<object> ChunkUploading { get; set; }

        /// <summary>
        /// Gets or sets the event callback that is triggered before clearing the file list.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{T}"/> that is invoked before the file list is cleared.
        /// </value>
        /// <remarks>
        /// This event is triggered when users click the "clear" button to remove all files from the
        /// upload queue. It provides an opportunity to implement confirmation dialogs or cancel the
        /// clear operation based on user preferences or application requirements.
        /// </remarks>
        [JsonIgnore]
        [JsonPropertyName("clearing")]
        public EventCallback<object> Clearing { get; set; }

        /// <summary>
        /// Gets or sets the event callback that is triggered when the uploader component is created.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{T}"/> that is invoked when the uploader component initialization is complete.
        /// </value>
        /// <remarks>
        /// This event is triggered when the uploader component has finished its initialization process.
        /// It provides an opportunity to perform additional setup, apply custom configurations, or
        /// initialize related functionality that depends on the uploader being fully ready.
        /// </remarks>
        [JsonIgnore]
        [JsonPropertyName("created")]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// Gets or sets the failure event arguments when AJAX requests fail during upload or removal operations.
        /// </summary>
        /// <value>
        /// A <see cref="FailureEventArgs"/> object containing failure information, or <c>null</c> if no failures have occurred.
        /// </value>
        /// <remarks>
        /// This property contains the event arguments for general upload or removal failures. It provides
        /// comprehensive information about failed operations, including error details and retry options,
        /// enabling robust error handling and user feedback in upload scenarios.
        /// </remarks>
        [DefaultValue(null)]
        [JsonPropertyName("failure")]
        public FailureEventArgs? Failure { get; set; }

        /// <summary>
        /// Gets or sets the event callback that is triggered before rendering each file item in the file list.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{T}"/> that is invoked for each file item during file list rendering.
        /// </value>
        /// <remarks>
        /// This event is triggered before each file item is rendered in the file list, providing an
        /// opportunity to customize the structure and appearance of individual file items. It enables
        /// implementation of custom file item templates and dynamic styling based on file properties.
        /// </remarks>
        [JsonIgnore]
        [JsonPropertyName("fileListRendering")]
        public EventCallback<object> FileListRendering { get; set; }

        /// <summary>
        /// Gets or sets the event callback that is triggered when chunk file uploads are paused.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{T}"/> that is invoked when chunk upload operations are paused.
        /// </value>
        /// <remarks>
        /// This event is triggered when chunk-based file upload operations are paused by user action
        /// or system conditions. It provides context about the pause operation and enables implementation
        /// of custom pause handling or user feedback functionality.
        /// </remarks>
        [JsonIgnore]
        [JsonPropertyName("pausing")]
        public EventCallback<object> Pausing { get; set; }

        /// <summary>
        /// Gets or sets the progress event arguments during file upload operations.
        /// </summary>
        /// <value>
        /// A <see cref="ProgressEventArgs"/> object containing progress information, or <c>null</c> if no progress events have occurred.
        /// </value>
        /// <remarks>
        /// This property contains the event arguments for upload progress updates. It provides real-time
        /// information about upload progress, including bytes transferred and completion percentages,
        /// enabling implementation of progress indicators and user feedback during file transfers.
        /// </remarks>
        [DefaultValue(null)]
        [JsonPropertyName("progress")]
        public ProgressEventArgs? Progress { get; set; }

        /// <summary>
        /// Gets or sets the event callback that is triggered when files are being removed.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{T}"/> that is invoked during file removal operations.
        /// </value>
        /// <remarks>
        /// This event is triggered during the file removal process and provides an opportunity to
        /// implement confirmation dialogs, validate removal permissions, or perform cleanup operations
        /// before files are actually removed from the server.
        /// </remarks>
        [JsonIgnore]
        [JsonPropertyName("removing")]
        public EventCallback<object> Removing { get; set; }

        /// <summary>
        /// Gets or sets the event callback that is triggered when paused chunk file uploads are resumed.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{T}"/> that is invoked when paused chunk upload operations are resumed.
        /// </value>
        /// <remarks>
        /// This event is triggered when previously paused chunk-based upload operations are resumed.
        /// It provides context about the resume operation and enables implementation of custom resume
        /// handling or progress tracking functionality for interrupted uploads.
        /// </remarks>
        [JsonIgnore]
        [JsonPropertyName("resuming")]
        public EventCallback<object> Resuming { get; set; }

        /// <summary>
        /// Gets or sets the event callback that is triggered after files are selected or dropped.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{T}"/> that is invoked when files are selected or dropped for upload.
        /// </value>
        /// <remarks>
        /// This event is triggered after files are selected or dropped, providing an opportunity to
        /// validate file selections, modify the file list, or implement custom selection handling
        /// before files are added to the upload queue.
        /// </remarks>
        [JsonIgnore]
        [JsonPropertyName("selected")]
        public EventCallback<object> Selected { get; set; }

        /// <summary>
        /// Gets or sets the success event arguments when AJAX requests succeed during upload or removal operations.
        /// </summary>
        /// <value>
        /// A <see cref="SuccessEventArgs"/> object containing success information, or <c>null</c> if no successful operations have occurred.
        /// </value>
        /// <remarks>
        /// This property contains the event arguments for successful upload or removal operations. It provides
        /// comprehensive information about completed operations, including server responses and operation
        /// details, enabling implementation of success handling and user feedback functionality.
        /// </remarks>
        [DefaultValue(null)]
        [JsonPropertyName("success")]
        public SuccessEventArgs? Success { get; set; }

        /// <summary>
        /// Gets or sets the event callback that is triggered when upload processes start.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{T}"/> that is invoked when upload operations begin.
        /// </value>
        /// <remarks>
        /// This event is triggered when the file upload process starts and is commonly used to add
        /// additional parameters to upload requests, implement authentication, or perform pre-upload
        /// validation and setup operations.
        /// </remarks>
        [JsonIgnore]
        [JsonPropertyName("uploading")]
        public EventCallback<object> Uploading { get; set; }

        /// <summary>
        /// Gets or sets the file extensions allowed for upload in the uploader component.
        /// </summary>
        /// <value>
        /// A string containing comma-separated file extensions (e.g., ".jpg,.png,.pdf"), or an empty string to allow all file types.
        /// </value>
        /// <remarks>
        /// This property restricts the types of files that users can select and upload. Extensions should be specified
        /// with a leading dot and separated by commas. For example, to allow only image files, set this property to
        /// ".jpg,.jpeg,.png,.gif". When this property is set, the file selection dialog will filter available files
        /// to show only those with matching extensions, and validation will prevent upload of non-matching file types.
        /// </remarks>
        /// <example>
        /// To allow only image files: <c>AllowedExtensions = ".jpg,.png,.gif"</c>
        /// </example>
        [DefaultValue("")]
        [JsonPropertyName("allowedExtensions")]
        public string AllowedExtensions { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the asynchronous upload settings for server-side file operations.
        /// </summary>
        /// <value>
        /// An <see cref="UploaderAsyncSettings"/> object containing save and remove URLs and other async configurations, or <c>null</c> to disable asynchronous operations.
        /// </value>
        /// <remarks>
        /// This property configures the server endpoints for save and remove operations, enabling asynchronous
        /// file upload functionality. It specifies the URLs that handle file upload and removal on the server side,
        /// along with additional settings like chunk size, retry behavior, and delay configurations. When properly
        /// configured, this allows for automatic file upload with progress tracking and error handling.
        /// </remarks>
        [DefaultValue(null)]
        [JsonPropertyName("asyncSettings")]
        public UploaderAsyncSettings? AsyncSettings { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether files should be automatically uploaded when added to the upload queue.
        /// </summary>
        /// <value>
        /// <c>true</c> to automatically upload files when they are selected; otherwise, <c>false</c>. The default is <c>true</c>.
        /// </value>
        /// <remarks>
        /// When set to <c>true</c>, files are automatically uploaded as soon as they are selected or dropped.
        /// When set to <c>false</c>, files are added to the upload queue but require manual initiation of the upload
        /// process through the upload button. Setting this to <c>false</c> also displays upload and clear buttons
        /// in the file list, giving users complete control over when uploads occur.
        /// </remarks>
        [DefaultValue(true)]
        [JsonPropertyName("autoUpload")]
        public bool AutoUpload { get; set; } = true;

        /// <summary>
        /// Gets or sets the custom button text and content for the uploader interface.
        /// </summary>
        /// <value>
        /// An <see cref="UploaderButtons"/> object containing custom button configurations, or <c>null</c> to use default button text.
        /// </value>
        /// <remarks>
        /// This property allows comprehensive customization of the text displayed on the browse, upload, and clear buttons.
        /// The button text can be plain text or HTML content for enhanced visual design. If both this property and
        /// localization are configured, this property takes precedence over localized text, enabling fine-grained
        /// control over button appearance and content.
        /// </remarks>
        [DefaultValue(null)]
        [JsonPropertyName("buttons")]
        public UploaderButtons? Buttons { get; set; }

        /// <summary>
        /// Gets or sets the CSS class names to be applied to the uploader component.
        /// </summary>
        /// <value>
        /// A string containing one or more CSS class names separated by spaces, or an empty string for no custom classes.
        /// </value>
        /// <remarks>
        /// This property allows comprehensive custom styling of the uploader component by applying CSS classes to
        /// the root element. Multiple classes can be specified by separating them with spaces. These classes enable
        /// complete customization of the component's appearance, layout, and behavior through CSS styling, including
        /// themes, responsive designs, and custom branding requirements.
        /// </remarks>
        [DefaultValue("")]
        [JsonPropertyName("cssClass")]
        public string CssClass { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether folder upload functionality is enabled for the uploader component.
        /// </summary>
        /// <value>
        /// <c>true</c> to enable folder upload functionality; otherwise, <c>false</c>. The default is <c>false</c>.
        /// </value>
        /// <remarks>
        /// When enabled, users can select and upload entire folders rather than just individual files. This feature
        /// allows for bulk upload of folder contents while preserving the original folder structure. Note that folder
        /// upload support depends on browser capabilities and may not be available in all browsers or older browser versions.
        /// </remarks>
        [DefaultValue(false)]
        [JsonPropertyName("directoryUpload")]
        public bool DirectoryUpload { get; set; } = false;

        /// <summary>
        /// Gets or sets the target element or selector for drag-and-drop file upload operations.
        /// </summary>
        /// <value>
        /// An object specifying the drop target element or CSS selector, or <c>null</c> to use the default drop area.
        /// </value>
        /// <remarks>
        /// This property allows customization of the drag-and-drop target area beyond the default uploader interface.
        /// By default, the uploader creates a wrapper around the file input that serves as the drop target. Setting
        /// this property allows specification of a different element as the drop zone, enabling more flexible UI designs
        /// and integration with custom layouts or existing page elements.
        /// </remarks>
        [DefaultValue(null)]
        [JsonPropertyName("dropArea")]
        public object? DropArea { get; set; }

        /// <summary>
        /// Gets or sets the visual effect displayed during drag-and-drop operations.
        /// </summary>
        /// <value>
        /// A <see cref="DropEffect"/> enumeration value specifying the drag operation effect. The default is <see cref="DropEffect.Default"/>.
        /// </value>
        /// <remarks>
        /// This property controls the visual feedback shown to users during drag-and-drop operations, providing
        /// clear indication of the intended action. The available effects are:
        /// <list type="bullet">
        /// <item><description><strong>Copy</strong> - Indicates that the dragged files will be copied to the target location</description></item>
        /// <item><description><strong>Move</strong> - Indicates that the dragged files will be moved to the target location</description></item>
        /// <item><description><strong>Link</strong> - Indicates that a link or reference to the files will be created</description></item>
        /// <item><description><strong>None</strong> - No visual effect is displayed during drag operations</description></item>
        /// <item><description><strong>Default</strong> - Uses the browser's default drag effect based on the context</description></item>
        /// </list>
        /// </remarks>
        [DefaultValue(DropEffect.Default)]
        [JsonPropertyName("dropEffect")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DropEffect DropEffect { get; set; } = DropEffect.Default;

        /// <summary>
        /// Gets or sets a value indicating whether to persist the uploader state between page reloads.
        /// </summary>
        /// <value>
        /// <c>true</c> to enable state persistence; otherwise, <c>false</c>. The default is <c>false</c>.
        /// </value>
        /// <remarks>
        /// When enabled, the uploader component will persist its state (including the Files collection) between
        /// page reloads, providing continuity of the upload experience even if the page is refreshed or navigated
        /// away from and back. This feature is particularly useful in single-page applications or scenarios where
        /// users might accidentally reload the page during upload operations.
        /// </remarks>
        [DefaultValue(false)]
        [JsonPropertyName("enablePersistence")]
        public bool EnablePersistence { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the uploader component allows user interaction.
        /// </summary>
        /// <value>
        /// <c>false</c> if the uploader is enabled for user interaction; otherwise, <c>true</c>. The default is <c>false</c>.
        /// </value>
        /// <remarks>
        /// When set to <c>false</c>, the uploader component becomes disabled and users cannot interact with it.
        /// This includes file selection, drag-and-drop operations, and button clicks. Disabled uploader components
        /// typically appear grayed out and do not respond to user input, making them suitable for scenarios where
        /// upload functionality should be temporarily unavailable.
        /// </remarks>
        [DefaultValue(false)]
        [JsonPropertyName("disabled")]
        public bool Disabled { get; set; } = false;

        /// <summary>
        /// Gets or sets the list of files that will be preloaded when the uploader component is rendered.
        /// </summary>
        /// <value>
        /// A <see cref="List{UploadedFile}"/> containing the preloaded file configurations, or <c>null</c> if no files are preloaded.
        /// </value>
        /// <remarks>
        /// This property is used to display and manage files that have already been uploaded to the server.
        /// By default, preloaded files are configured with an "uploaded successfully" state. The following
        /// properties are mandatory for each preloaded file configuration:
        /// <list type="bullet">
        /// <item><description><strong>Name</strong> - The file name including extension</description></item>
        /// <item><description><strong>Size</strong> - The file size in bytes</description></item>
        /// <item><description><strong>Type</strong> - The MIME type of the file</description></item>
        /// </list>
        /// This feature enables seamless integration with existing file management systems where files
        /// may have been uploaded in previous sessions or through other means.
        /// </remarks>
        [DefaultValue(null)]
        [JsonPropertyName("files")]
        public List<UploadedFile>? Files { get; set; }

        /// <summary>
        /// Gets or sets additional HTML attributes to be applied to the root element of the uploader component.
        /// </summary>
        /// <value>
        /// An object containing HTML attributes as key-value pairs, or <c>null</c> if no additional attributes are specified.
        /// </value>
        /// <remarks>
        /// This property allows addition of custom HTML attributes such as styles, classes, data attributes,
        /// and more to the uploader's root element. If both this property and equivalent component-specific
        /// properties are configured (such as CssClass), the component-specific property values take precedence
        /// over the HTML attributes, ensuring predictable behavior and component integrity.
        /// </remarks>
        [DefaultValue(null)]
        [JsonPropertyName("htmlAttributes")]
        public object? HtmlAttributes { get; set; }

        /// <summary>
        /// Gets or sets the global culture and localization setting for the uploader component.
        /// </summary>
        /// <value>
        /// A string representing the locale identifier (e.g., "en-US", "fr-FR"), or an empty string to use the default locale.
        /// </value>
        /// <remarks>
        /// This property specifies the culture and localization context for the uploader component, affecting
        /// the display language of built-in text, date formats, number formats, and other culture-sensitive
        /// elements. The locale setting influences button text, error messages, and other user-facing content,
        /// enabling proper internationalization support for global applications.
        /// </remarks>
        [DefaultValue("")]
        [JsonPropertyName("locale")]
        public string Locale { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the maximum allowed file size for uploads in bytes.
        /// </summary>
        /// <value>
        /// A double value representing the maximum file size in bytes. The default value is 30,000,000 bytes (approximately 30 MB).
        /// </value>
        /// <remarks>
        /// This property enforces a maximum file size limit to prevent users from uploading excessively large files
        /// that could impact server performance, storage capacity, or network bandwidth. Files exceeding this size
        /// limit will be rejected with appropriate validation messages, helping maintain system stability and
        /// reasonable resource usage across upload operations.
        /// </remarks>
        [DefaultValue(UploaderDefaults.MaxFileSize)]
        [JsonPropertyName("maxFileSize")]
        public double MaxFileSize { get; set; } = UploaderDefaults.MaxFileSize;

        /// <summary>
        /// Gets or sets the minimum required file size for uploads in bytes.
        /// </summary>
        /// <value>
        /// A double value representing the minimum file size in bytes. The default value is 0.
        /// </value>
        /// <remarks>
        /// This property enforces a minimum file size requirement to prevent users from uploading empty files
        /// or files that are too small to contain meaningful content. This validation helps ensure data quality
        /// and prevents accidental upload of corrupted or incomplete files that could cause issues in downstream
        /// processing or storage systems.
        /// </remarks>
        [DefaultValue(UploaderDefaults.MinFileSize)]
        [JsonPropertyName("minFileSize")]
        public double MinFileSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether multiple files can be selected and uploaded simultaneously.
        /// </summary>
        /// <value>
        /// <c>true</c> to allow selection and upload of multiple files; otherwise, <c>false</c>. The default is <c>true</c>.
        /// </value>
        /// <remarks>
        /// When set to <c>true</c>, users can select multiple files at once through the file selection dialog
        /// or drag-and-drop multiple files simultaneously. When set to <c>false</c>, the uploader restricts
        /// selection to a single file at a time, which is suitable for scenarios where only one file should
        /// be processed or uploaded per operation.
        /// </remarks>
        [DefaultValue(true)]
        [JsonPropertyName("multiple")]
        public bool Multiple { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether files should be uploaded sequentially rather than simultaneously.
        /// </summary>
        /// <value>
        /// <c>true</c> to enable sequential upload processing; otherwise, <c>false</c> for simultaneous uploads. The default is <c>false</c>.
        /// </value>
        /// <remarks>
        /// By default, the uploader component processes multiple files simultaneously for optimal performance.
        /// When this property is enabled, the component performs uploads one after another in sequence, which
        /// can be beneficial for scenarios with limited bandwidth, server constraints, or when upload order
        /// is important for business logic or processing requirements.
        /// </remarks>
        [DefaultValue(false)]
        [JsonPropertyName("sequentialUpload")]
        public bool SequentialUpload { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the default file list should be rendered.
        /// </summary>
        /// <value>
        /// <c>true</c> to show the default file list; otherwise, <c>false</c> to hide it. The default is <c>true</c>.
        /// </value>
        /// <remarks>
        /// When set to <c>true</c>, the component displays the built-in file list showing selected files with
        /// their details, progress, and status information. When set to <c>false</c>, the default file list
        /// is hidden, allowing developers to create completely custom file list implementations using templates
        /// or external components while still leveraging the uploader's core functionality.
        /// </remarks>
        [DefaultValue(true)]
        [JsonPropertyName("showFileList")]
        public bool ShowFileList { get; set; } = true;

        /// <summary>
        /// Gets or sets the HTML template string used to customize the content of each file item in the list.
        /// </summary>
        /// <value>
        /// A string containing HTML template markup for file list items, or <c>null</c> to use the default template.
        /// </value>
        /// <remarks>
        /// This property allows complete customization of how individual files are displayed in the file list.
        /// The template can include HTML markup, CSS classes, and data binding expressions to create rich,
        /// interactive file list items that match application design requirements. When specified, this template
        /// overrides the default file list item rendering, providing maximum flexibility in file list presentation.
        /// </remarks>
        [DefaultValue(null)]
        [JsonPropertyName("template")]
        public string Template { get; set; } = string.Empty;
    }
}