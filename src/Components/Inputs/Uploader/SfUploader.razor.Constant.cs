namespace Syncfusion.Blazor.Toolkit.Inputs
{
    /// <summary>
    /// The Syncfusion Blazor Uploader component provides a comprehensive file upload solution with advanced features for uploading images, documents, and other files to a server.
    /// This component extends the functionality of HTML5 file upload with multiple file selection, automatic upload, drag and drop support, progress tracking, file preloading, and validation capabilities.
    /// </summary>
    /// <remarks>
    /// The <see cref="SfUploader"/> component offers a rich set of features including:
    /// <list type="bullet">
    /// <item><description>Multiple file selection and upload</description></item>
    /// <item><description>Drag and drop functionality for intuitive file selection</description></item>
    /// <item><description>Automatic upload with configurable settings</description></item>
    /// <item><description>Progress bar visualization during upload operations</description></item>
    /// <item><description>File validation with size and type restrictions</description></item>
    /// <item><description>Template customization for enhanced user experience</description></item>
    /// <item><description>Chunk upload support for large files</description></item>
    /// <item><description>Resume and pause upload functionality</description></item>
    /// </list>
    /// The component integrates seamlessly with server-side upload handlers and provides comprehensive event handling for upload lifecycle management.
    /// </remarks>
    /// <example>
    /// Basic implementation of the SfUploader component:
    /// <code><![CDATA[
    /// <SfUploader ID="fileUpload" AutoUpload="false" OnUploadStart="@OnFileUploadStart" ValueChange="@OnChange">
    ///     <UploaderAsyncSettings SaveUrl="api/SampleData/Save" RemoveUrl="api/SampleData/Remove" />
    /// </SfUploader>
    /// 
    /// @code {
    ///     private void OnFileUploadStart(UploadingEventArgs args)
    ///     {
    ///         // Handle upload start logic
    ///     }
    ///     
    ///     private void OnChange(UploadChangeEventArgs args)
    ///     {
    ///         // Handle file selection changes
    ///     }
    /// }
    /// ]]></code>
    /// </example>
    public partial class SfUploader
    {
        #region CSS Class Constants
        private const string ROOT = "e-uploader";
        private const string CONTROL_CONTAINER = "e-upload e-control-wrapper e-control-container";
        private const string INPUT_CONTAINER = "e-file-select";
        private const string DROP_AREA = "e-file-drop";
        private const string DROP_CONTAINER = "e-file-select-wrap";
        private const string LIST_PARENT = "e-upload-files";
        private const string FILE_LIST_CLASS = "e-upload-file-list";
        private const string STATUS = "e-file-status";
        private const string ACTION_BUTTONS = "e-upload-actions";
        private const string HIDDEN = "e-hidden";
        private const string UPLOAD_BUTTONS = "e-file-upload-btn e-css e-btn e-flat e-primary";
        private const string CLEAR_BUTTONS = "e-file-clear-btn e-css e-btn e-flat";
        private const string UPLOAD_INPROGRESS = "e-upload-progress";
        private const string UPLOAD_SUCCESS = "e-upload-success";
        private const string VALIDATION_FAIL = "e-validation-fails";
        private const string RTL = "e-rtl";
        private const string DISABLED_CLASS = "e-disabled";
        private const string RTL_CONTAINER = "e-rtl-container";
        #endregion

        #region Common Constants
        private const string SPACE = " ";
        private const string TRUE = "true";
        #endregion

        #region Size and Buffer Constants
        /// <summary>
        /// Default buffer size for file upload chunks (20 KB).
        /// </summary>
        internal const int DEFAULT_BUFFER_SIZE_BYTES = 20480;

        /// <summary>
        /// Maximum allowed file size for uploads (100 GB).
        /// </summary>
        internal const long MAX_ALLOWED_FILE_SIZE_BYTES = 100000000000;

        /// <summary>
        /// Delay in milliseconds for UI state synchronization.
        /// </summary>
        internal const int UI_SYNC_DELAY_MS = 20;

        /// <summary>
        /// Delay in milliseconds for component rendering.
        /// </summary>
        internal const int RENDER_DELAY_MS = 100;
        #endregion

        #region Configuration Property Keys
        private const string ALLOW_EXTENSIONS = "allowedExtensions";
        private const string ENABLE_HTML_SANITIZER = "enableHtmlSanitizer";
        private const string ASYNC_SETTING = "asyncSettings";
        private const string AUTO_UPLOAD = "autoUpload";
        private const string BUTTON = "buttons";
        private const string CSSCLASS = "cssClass";
        private const string DROPAREA = "dropArea";
        private const string DIRECTORY_UPLOAD = "directoryUpload";
        private const string DROP_EFFECT = "dropEffect";
        private const string DISABLED = "disabled";
        private const string UPLOAD_FILES = "files";
        private const string MAX_FILE_SIZE = "maxFileSize";
        private const string MIN_FILE_SIZE = "minFileSize";
        private const string SHOW_FILE_LIST = "showFileList";
        private const string UPLOAD_MULTIPLE = "multiple";
        private const string SEQUENTIAL_UPLOAD = "sequentialUpload";
        private const string UPLOAD_TEMPLATE = "template";
        private const string PERSISTENCE = "enablePersistence";
        private const string HTTPCLIENTHEADERS = "httpClientHeaders";
        #endregion

        #region Event Configuration Keys
        private const string ACTION_COMPLETE_ENABLED = "actionCompleteEnabled";
        private const string BEFORE_REMOVE_ENABLED = "beforeRemoveEnabled";
        private const string BEFORE_UPLOAD_ENABLED = "beforeUploadEnabled";
        private const string CANCEL_ENABLED = "cancelEnabled";
        private const string CHANGE_ENABLED = "changeEnabled";
        private const string CHUNK_FAILURE_ENABLED = "chunkFailuredEnabled";
        private const string CHUNK_UPLOADING_ENABLED = "chunkUploadingEnabled";
        private const string UPLOADING_ENABLED = "uploadingEnabled";
        private const string CLEAR_ENABLED = "clearEnabled";
        private const string FAILURE_ENABLED = "failuredEnabled";
        private const string FILE_RENDER_ENABLED = "fileListRenderEnabled";
        private const string PAUSED_ENABLED = "pausedEnabled";
        private const string PROGRESSING_ENABLED = "progressingEnabled";
        private const string REMOVING_ENABLED = "removingEnabled";
        private const string RESUME_ENABLED = "resumeEnabled";
        private const string SELECTED_ENABLED = "selectedEnabled";
        private const string SUCCESS_ENABLED = "successEnabled";
        private const string CHUNK_SUCCESS_ENABLED = "chunkSuccessEnabled";
        #endregion

        #region Localization Keys
        private const string LOCALE_TEXT = "localeText";
        private const string BROWSE_KEY = "Browse";
        private const string ABORT_KEY = "Abort";
        private const string CANCEL_KEY = "Cancel";
        private const string CLEAR_KEY = "Clear";
        private const string DELETE_KEY = "Delete";
        private const string DROP_FILE_KEY = "DropFilesHint";
        private const string FILE_UPLOAD_CANCEL = "FileUploadCancel";
        private const string INPROGRESS_KEY = "InProgress";
        private const string INVALID_FILE_KEY = "InvalidFileType";
        private const string INVALID_FILE_NAME = "InvalidFileName";
        private const string INVALID_MAX_FILE_KEY = "InvalidMaxFileSize";
        private const string INVALID_MIN_FILE_KEY = "InvalidMinFileSize";
        private const string PAUSE_KEY = "Pause";
        private const string PAUSE_UPLOAD_KEY = "PauseUpload";
        private const string READY_UPLOAD_KEY = "ReadyToUploadMessage";
        private const string REMOVE_KEY = "Remove";
        private const string REMOVED_FAILED_KEY = "RemovedFailedMessage";
        private const string REMOVED_SUCCESS_KEY = "RemovedSuccessMessage";
        private const string RESUME_KEY = "Resume";
        private const string RETRY_KEY = "Retry";
        private const string UPLOAD_KEY = "Upload";
        private const string UPLOAD_FAILED_KEY = "UploadFailedMessage";
        private const string UPLOAD_SUCCESS_KEY = "UploadSuccessMessage";
        #endregion
    }
}