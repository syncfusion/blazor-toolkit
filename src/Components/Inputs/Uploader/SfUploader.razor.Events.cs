using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    /// <summary>
    /// Represents a class that contains all the event callbacks for the <see cref="SfUploader"/> component.
    /// This class provides event handling capabilities for various upload operations including file selection,
    /// upload progress, success, failure, and file management operations.
    /// </summary>
    /// <remarks>
    /// This component is designed to work in both Blazor WebAssembly and Blazor Server hosting models.
    /// All event callbacks are asynchronous and support both synchronous and asynchronous event handlers.
    /// The component automatically cleans up resources when disposed to prevent memory leaks.
    /// </remarks>
    public partial class SfUploader
    {
        #region Event Callbacks

        /// <summary>
        /// Gets or sets the event callback that will be invoked after all selected files have been processed for upload, 
        /// whether they completed successfully or failed during the upload operation to the server.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{ActionCompleteEventArgs}"/> that handles the action complete event.
        /// </value>
        [Parameter]
        public EventCallback<ActionCompleteEventArgs> OnActionComplete { get; set; }

        /// <summary>
        /// Gets or sets the event callback that will be invoked before removing a file from the server.
        /// This event allows you to cancel the removal operation or perform custom actions before file deletion.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{BeforeRemoveEventArgs}"/> that handles the before remove event.
        /// </value>
        [Parameter]
        public EventCallback<BeforeRemoveEventArgs> BeforeRemove { get; set; }

        /// <summary>
        /// Gets or sets the event callback that will be invoked before the uploading process starts.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{BeforeUploadEventArgs}"/> that handles the before upload event.
        /// </value>
        /// <remarks>
        /// You can pass additional data with the file uploading request in the <see cref="BeforeUploadEventArgs.CustomFormData"/> argument.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Inputs
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
        [Parameter]
        public EventCallback<BeforeUploadEventArgs> BeforeUpload { get; set; }

        /// <summary>
        /// Gets or sets the event callback that will be invoked when a chunk file upload operation is canceled by the user.
        /// This event is only applicable when chunk uploading is enabled.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{CancelEventArgs}"/> that handles the upload cancellation event.
        /// </value>
        [Parameter]
        public EventCallback<CancelEventArgs> OnCancel { get; set; }

        /// <summary>
        /// Gets or sets the event callback that will be invoked when the collection of the selected files is uploaded for each file.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{UploadChangeEventArgs}"/> that handles the file upload change event.
        /// </value>
        /// <remarks>
        /// This event is triggered when the user selects a new file in the input file element.
        /// To read the contents of the uploaded file, call the `OpenReadStream()` method of the `IBrowserFile` interface, 
        /// which returns a stream that you can use to read the file data.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Inputs
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
        [Parameter]
        public EventCallback<UploadChangeEventArgs> OnValueChange { get; set; }

        /// <summary>
        /// Gets or sets the event callback that will be invoked when a chunk file fails to upload to the server.
        /// This event provides error information for debugging upload failures in chunk upload scenarios.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{FailureEventArgs}"/> that handles chunk upload failure events.
        /// </value>
        [Parameter]
        public EventCallback<FailureEventArgs> OnChunkFailure { get; set; }

        /// <summary>
        /// Gets or sets the event callback that will be invoked when each chunk file is uploaded successfully to the server.
        /// This event is triggered for each successful chunk upload operation.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{SuccessEventArgs}"/> that handles chunk upload success events.
        /// </value>
        [Parameter]
        public EventCallback<SuccessEventArgs> OnChunkSuccess { get; set; }

        /// <summary>
        /// Gets or sets the event callback that will be invoked when every chunk upload process gets started.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{UploadingEventArgs}"/> that handles the chunk upload start event.
        /// </value>
        /// <remarks>
        /// Pass the additional data with the file uploading request in the <see cref="UploadingEventArgs.CustomFormData"/> argument.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Inputs
        /// <SfUploader ID="UploadFiles" OnChunkUploadStart="@ChunkUploadStartHandler">
        ///     <UploaderAsyncSettings SaveUrl="api/SampleData/Save" RemoveUrl="api/SampleData/Remove" ChunkSize="50000" />
        /// </SfUploader>
        /// @code {
        /// public void ChunkUploadStartHandler(UploadingEventArgs args) {
        ///    var accessToken = "Authorization_token";
        ///    args.CurrentRequest = new List<object> { new { Authorization = accessToken } };
        /// }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<UploadingEventArgs> OnChunkUploadStart { get; set; }

        /// <summary>
        /// Gets or sets the event callback that will be invoked before clearing all items in the file list using the Clear button.
        /// This event allows you to cancel the clear operation or perform custom actions before clearing the file list.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{ClearingEventArgs}"/> that handles the file list clearing event.
        /// </value>
        [Parameter]
        public EventCallback<ClearingEventArgs> OnClear { get; set; }

        /// <summary>
        /// Gets or sets the event callback that will be invoked when the <see cref="SfUploader"/> component has been created and initialized.
        /// This event is useful for performing initialization tasks after the component is ready.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{Object}"/> that handles the component creation event.
        /// </value>
        [Parameter]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// Gets or sets the event callback that will be invoked when a file upload or file removal request fails.
        /// This event provides error information to help diagnose and handle upload or removal failures.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{FailureEventArgs}"/> that handles upload or removal failure events.
        /// </value>
        [Parameter]
        public EventCallback<FailureEventArgs> OnFailure { get; set; }

        /// <summary>
        /// Gets or sets the event callback that will be invoked before rendering each file item in the file list.
        /// This event allows you to customize the structure and appearance of individual file items.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{FileListRenderingEventArgs}"/> that handles file list rendering events.
        /// </value>
        [Parameter]
        public EventCallback<FileListRenderingEventArgs> OnFileListRender { get; set; }

        /// <summary>
        /// Gets or sets the event callback that will be invoked when a chunk file upload operation is paused by the user.
        /// This event is only applicable when chunk uploading is enabled and provides pause/resume functionality.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{PauseResumeEventArgs}"/> that handles upload pause events.
        /// </value>
        [Parameter]
        public EventCallback<PauseResumeEventArgs> Paused { get; set; }

        /// <summary>
        /// Gets or sets the event callback that will be invoked during the file upload process to track upload progress.
        /// This event provides real-time information about the upload progress for each file.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{ProgressEventArgs}"/> that handles upload progress events.
        /// </value>
        [Parameter]
        public EventCallback<ProgressEventArgs> Progressing { get; set; }

        /// <summary>
        /// Gets or sets the event callback that will be invoked when removing an uploaded file from the server.
        /// This event allows you to perform custom actions or confirmations before file removal.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{RemovingEventArgs}"/> that handles file removal events.
        /// </value>
        /// <remarks>
        /// This event can be used to confirm the file removal operation before it proceeds.
        /// </remarks>
        [Parameter]
        public EventCallback<RemovingEventArgs> OnRemove { get; set; }

        /// <summary>
        /// Gets or sets the event callback that will be invoked when a paused chunk file upload is resumed.
        /// This event is only applicable when chunk uploading is enabled and provides pause/resume functionality.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{PauseResumeEventArgs}"/> that handles upload resume events.
        /// </value>
        [Parameter]
        public EventCallback<PauseResumeEventArgs> OnResume { get; set; }

        /// <summary>
        /// Gets or sets the event callback that will be invoked after selecting or dropping files in the <see cref="SfUploader"/> component.
        /// This event provides information about the newly selected files and allows validation before upload.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{SelectedEventArgs}"/> that handles file selection events.
        /// </value>
        [Parameter]
        public EventCallback<SelectedEventArgs> FileSelected { get; set; }

        /// <summary>
        /// Gets or sets the event callback that will be invoked when file upload or file removal operations complete successfully.
        /// This event provides confirmation and result information for successful operations.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{SuccessEventArgs}"/> that handles successful upload or removal events.
        /// </value>
        [Parameter]
        public EventCallback<SuccessEventArgs> Success { get; set; }

        /// <summary>
        /// Gets or sets the event callback that will be invoked when the file upload process starts.
        /// This event allows you to add custom parameters to the upload request or perform pre-upload operations.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{UploadingEventArgs}"/> that handles upload start events.
        /// </value>
        [Parameter]
        public EventCallback<UploadingEventArgs> OnUploadStart { get; set; }

        #endregion
    }
}
