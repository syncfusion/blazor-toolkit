using Microsoft.JSInterop;
using Syncfusion.Blazor.Toolkit.Inputs.Internal;
using Syncfusion.Blazor.Toolkit.Internal;
using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    /// <summary>
    /// Represents the Syncfusion Blazor File Uploader component that provides asynchronous file upload functionality with advanced features.
    /// This partial class contains the public methods for file operations including upload, pause, resume, cancel, and remove operations.
    /// </summary>
    public partial class SfUploader
    {
        /// <summary>
        /// Helper method to invoke event callbacks if they have delegates, reducing code duplication.
        /// </summary>
        /// <typeparam name="T">The type of event arguments.</typeparam>
        /// <param name="eventCallback">The event callback to invoke.</param>
        /// <param name="args">The event arguments to pass.</param>
        /// <returns>A task representing the asynchronous operation, returning the modified arguments.</returns>
        /// <remarks>
        /// This method does NOT use ConfigureAwait(false) because event callbacks require the synchronization context.
        /// </remarks>
        private async Task<T> InvokeEventIfHasDelegateAsync<T>(EventCallback<T> eventCallback, T args)
        {
            if (eventCallback.HasDelegate)
            {
                await eventCallback.InvokeAsync(args).ConfigureAwait(true);
            }
            return args;
        }
        /// <summary>
        /// Converts bytes value into kilobytes or megabytes depending on the size based on binary prefix.
        /// </summary>
        /// <param name="bytes">The file size in bytes to be converted.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous operation. The task result contains a <see cref="string"/> representing the formatted file size with appropriate units (B, KB, MB, GB, etc.).</returns>
        /// <remarks>
        /// This method converts numeric byte values to human-readable file size representations using binary prefixes (1024-based calculation).
        /// For example, 1024 bytes will be converted to "1 KB", and 1048576 bytes will be converted to "1 MB".
        /// The conversion follows the binary prefix standard where each unit is 1024 times larger than the previous unit.
        /// </remarks>
        /// <example>
        /// Convert file size from bytes to readable format:
        /// <code><![CDATA[
        /// string fileSize = await uploaderInstance.BytesToSizeAsync(2048);
        /// // Returns "2 KB"
        /// ]]></code>
        /// </example>
        public async Task<string> BytesToSizeAsync(double bytes)
        {
            return await InvokeAsync<string>(_uploaderJsModule!, _uploaderJsInProcessModule!, "bytesToSize", [DataId, bytes]).ConfigureAwait(true);
        }
        /// <summary>
        /// Stops the in-progress chunked upload based on the specified file data.
        /// </summary>
        /// <param name="fileData">An array of <see cref="FileInfo"/> objects representing the files to cancel. If <c>null</c>, all in-progress uploads will be canceled.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous cancel operation.</returns>
        /// <remarks>
        /// When the file upload is canceled using this method, the partially uploaded file chunks are automatically removed from the server.
        /// This method is particularly useful for chunked uploads where large files are divided into smaller parts for transmission.
        /// If no specific file data is provided, all currently uploading files will be canceled.
        /// </remarks>
        /// <example>
        /// Cancel specific files or all uploads:
        /// <code><![CDATA[
        /// // Cancel specific files
        /// await uploaderInstance.CancelAsync(selectedFiles);
        /// 
        /// // Cancel all uploads
        /// await uploaderInstance.CancelAsync();
        /// ]]></code>
        /// </example>
        public async Task CancelAsync(FileInfo[]? fileData = null)
        {
            await InvokeVoidAsync(_uploaderJsModule!, _uploaderJsInProcessModule!, "cancel", [DataId, fileData!]).ConfigureAwait(true);
        }
        /// <summary>
        /// Clears all file entries from the upload list, including both uploaded files and files in the upload queue.
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents the asynchronous clear operation.</returns>
        /// <remarks>
        /// This method removes all files from the uploader component, whether they are already uploaded, currently uploading, or waiting in the queue.
        /// The method behavior varies based on whether the uploader is configured with server-side processing (SaveUrl) or client-side only processing.
        /// For server-side uploads, it communicates with the server to clear files, while for client-side only uploads, it clears the local file list.
        /// </remarks>
        /// <example>
        /// Clear all files from the uploader:
        /// <code><![CDATA[
        /// await uploaderInstance.ClearAllAsync();
        /// ]]></code>
        /// </example>
        public async Task ClearAllAsync()
        {
            if (!string.IsNullOrEmpty(UploadAsyncSettings?.SaveUrl))
            {
                await InvokeVoidAsync(_uploaderJsModule, _uploaderJsInProcessModule, "clearAll", [DataId]).ConfigureAwait(true);
            }
            else
            {
                await ClearAllHandlerAsync().ConfigureAwait(true);
            }
        }
        /// <summary>
        /// Creates and renders the file list UI for the specified file data in the uploader component.
        /// </summary>
        /// <param name="fileData">An array of <see cref="FileInfo"/> objects containing the file information to be displayed in the list.</param>
        /// <param name="isSelectedFile">A <see cref="bool"/> value indicating whether the files are selected. If <c>true</c>, the files are marked as selected in the UI.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous file list creation operation.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task CreateFileListAsync(FileInfo[] fileData, bool? isSelectedFile = null)
        {
            await InvokeVoidAsync(_uploaderJsModule!, _uploaderJsInProcessModule!, "createFileList", [DataId, fileData, isSelectedFile!]).ConfigureAwait(true);
        }
        /// <summary>
        /// Retrieves the data of files that are displayed in the file list.
        /// </summary>
        /// <param name="index">The optional index of a specific file to retrieve. If <c>null</c>, all files in the list are returned.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous operation. The task result contains a <see cref="List{T}"/> of <see cref="FileInfo"/> objects representing the file data.</returns>
        /// <remarks>
        /// This method returns information about files currently displayed in the uploader's file list.
        /// If an index is specified, only the file at that position is returned (wrapped in a list).
        /// If no index is provided, all files in the current file list are returned.
        /// The behavior differs between server-side and client-side configurations, with server-side calls communicating with JavaScript interop.
        /// </remarks>
        /// <example>
        /// Get all files or a specific file by index:
        /// <code><![CDATA[
        /// // Get all files
        /// List<FileInfo> allFiles = await uploaderInstance.GetFilesDataAsync();
        /// 
        /// // Get specific file by index
        /// List<FileInfo> specificFile = await uploaderInstance.GetFilesDataAsync(0);
        /// ]]></code>
        /// </example>
        public async Task<List<FileInfo>> GetFilesDataAsync(double? index = null)
        {
            return !string.IsNullOrEmpty(UploadAsyncSettings?.SaveUrl)
                ? await GetFilesDataFromServerAsync(index).ConfigureAwait(false)
                : GetFilesDataClientSide(index);
        }

        /// <summary>
        /// Retrieves files data from the server via JavaScript interop.
        /// </summary>
        private async Task<List<FileInfo>> GetFilesDataFromServerAsync(double? index)
        {
            return await InvokeAsync<List<FileInfo>>(_uploaderJsModule!, _uploaderJsInProcessModule!, "getFilesData", [DataId, index!]).ConfigureAwait(true);
        }

        /// <summary>
        /// Retrieves files data from the client-side collection.
        /// </summary>
        private List<FileInfo> GetFilesDataClientSide(double? index)
        {
            if (index.HasValue)
            {
                int fileIndex = Convert.ToInt32(index.Value);
                if (fileIndex >= 0 && fileIndex < UploadedFilesInfo.Count)
                {
                    return [UploadedFilesInfo[fileIndex]];
                }
            }

            return UploadedFilesInfo;
        }
        /// <summary>
        /// Pauses the in-progress chunked upload based on the specified file data.
        /// </summary>
        /// <param name="fileData">A <see cref="List{T}"/> of <see cref="FileInfo"/> objects representing the files to pause. If <c>null</c>, all in-progress uploads will be paused.</param>
        /// <param name="custom">A <see cref="bool"/> value indicating whether custom UI is being used. Set to <c>true</c> if using custom UI templates.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous pause operation.</returns>
        /// <remarks>
        /// This method temporarily halts the upload process for chunked file uploads, allowing them to be resumed later using <see cref="ResumeAsync(FileInfo[], bool?)"/>.
        /// Pausing is particularly useful for large file uploads where users might want to temporarily stop the upload process due to network conditions or other priorities.
        /// The paused uploads maintain their current progress and can be resumed from the same point without losing uploaded chunks.
        /// </remarks>
        /// <example>
        /// Pause specific files or all uploads:
        /// <code><![CDATA[
        /// // Pause specific files
        /// await uploaderInstance.PauseAsync(selectedFiles);
        /// 
        /// // Pause all uploads
        /// await uploaderInstance.PauseAsync();
        /// ]]></code>
        /// </example>
        public async Task PauseAsync(List<FileInfo>? fileData = null, bool? custom = null)
        {
            await InvokeVoidAsync(_uploaderJsModule!, _uploaderJsInProcessModule!, "pause", [DataId, fileData!, custom!]).ConfigureAwait(true);
        }
        /// <summary>
        /// Removes the uploaded files from the server manually by calling the remove URL action.
        /// </summary>
        /// <param name="fileData">An array of <see cref="FileInfo"/> objects representing the files to remove from the file list and server. If <c>null</c> or empty, the complete file list will be cleared.</param>
        /// <param name="customTemplate">A <see cref="bool"/> value indicating whether the component is rendering with a custom template. Set to <c>true</c> if using custom templates.</param>
        /// <param name="removeDirectly">A <see cref="bool"/> value indicating whether to remove files directly without triggering removing events. Set to <c>true</c> to bypass event handling.</param>
        /// <param name="postRawFile">A <see cref="bool"/> value indicating the data format to post to the remove action. Set to <c>false</c> to post only the file name instead of the complete file data.</param>
        /// <param name="args">Additional arguments to pass to the remove action.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous remove operation.</returns>
        /// <remarks>
        /// This method removes files both from the client-side file list and from the server storage by calling the configured remove URL.
        /// If no specific file data is provided, all files in the current list will be removed.
        /// The method behavior varies based on whether server-side processing is configured through the SaveUrl property.
        /// For client-side only configurations, files are removed from the local file collection without server communication.
        /// </remarks>
        /// <example>
        /// Remove specific files or clear all files:
        /// <code><![CDATA[
        /// // Remove specific files
        /// await uploaderInstance.RemoveAsync(selectedFiles);
        /// 
        /// // Clear all files
        /// await uploaderInstance.RemoveAsync();
        /// 
        /// // Remove files with custom options
        /// await uploaderInstance.RemoveAsync(selectedFiles, customTemplate: true, removeDirectly: false);
        /// ]]></code>
        /// </example>
        public async Task RemoveAsync(FileInfo[]? fileData = null, bool? customTemplate = null, bool? removeDirectly = null, bool? postRawFile = null, object? args = null)
        {
            if (!string.IsNullOrEmpty(UploadAsyncSettings?.SaveUrl))
            {
                await RemoveViaServerAsync(fileData!, customTemplate, removeDirectly, postRawFile, args!).ConfigureAwait(true);
            }
            else
            {
                await RemoveClientSideAsync(fileData!).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Removes files via server-side JavaScript interop.
        /// </summary>
        private async Task RemoveViaServerAsync(FileInfo[] fileData, bool? customTemplate,
            bool? removeDirectly, bool? postRawFile, object args)
        {
            await InvokeVoidAsync(_uploaderJsModule!, _uploaderJsInProcessModule!, "remove",
                [DataId, fileData, customTemplate!, removeDirectly!, postRawFile!, args]).ConfigureAwait(true);
        }

        /// <summary>
        /// Removes files from the client-side collection.
        /// </summary>
        private async Task RemoveClientSideAsync(FileInfo[] fileData)
        {
            if (fileData == null || fileData.Length == 0)
            {
                await ClearAllHandlerAsync().ConfigureAwait(false);
                return;
            }

            await RemoveSpecificFilesAsync(fileData).ConfigureAwait(false);
        }

        /// <summary>
        /// Removes specific files from the client-side collection.
        /// </summary>
        private async Task RemoveSpecificFilesAsync(FileInfo[] fileData)
        {
            foreach (FileInfo fileInfo in fileData)
            {
                UploadFileDetails? file = FileData.FirstOrDefault(j => SfBaseUtils.Equals(j.Name, fileInfo.Name));
                if (file != null)
                {
                    await RemoveHandlerAsync(file, null).ConfigureAwait(false);
                }
            }
        }
        /// <summary>
        /// Resumes the chunked upload that was previously paused based on the specified file data.
        /// </summary>
        /// <param name="fileData">An array of <see cref="FileInfo"/> objects representing the files to resume. If <c>null</c>, all paused uploads will be resumed.</param>
        /// <param name="custom">A <see cref="bool"/> value indicating whether custom UI is being used. Set to <c>true</c> if using custom UI templates.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous resume operation.</returns>
        /// <remarks>
        /// This method continues the upload process for files that were previously paused using <see cref="PauseAsync(List{FileInfo}?, bool?)"/>.
        /// The upload resumes from the exact point where it was paused, utilizing the already uploaded chunks without re-uploading them.
        /// This feature is particularly useful for handling large file uploads where network interruptions or user preferences require temporary pausing.
        /// </remarks>
        /// <example>
        /// Resume specific paused files or all paused uploads:
        /// <code><![CDATA[
        /// // Resume specific files
        /// await uploaderInstance.ResumeAsync(pausedFiles);
        /// 
        /// // Resume all paused uploads
        /// await uploaderInstance.ResumeAsync();
        /// ]]></code>
        /// </example>
        public async Task ResumeAsync(FileInfo[]? fileData = null, bool? custom = null)
        {
            await InvokeVoidAsync(_uploaderJsModule!, _uploaderJsInProcessModule!, "resume", [DataId, fileData!, custom!]).ConfigureAwait(true);
        }
        /// <summary>
        /// Retries the canceled or failed file upload based on the specified file data.
        /// </summary>
        /// <param name="fileData">An array of <see cref="FileInfo"/> objects representing the files to retry. If <c>null</c>, all canceled or failed uploads will be retried.</param>
        /// <param name="fromcanceledStage">A <see cref="bool"/> value indicating the retry starting point. Set to <c>true</c> to retry from the canceled stage, or <c>false</c> to retry from the initial stage.</param>
        /// <param name="custom">A <see cref="bool"/> value indicating whether custom UI is being used. Set to <c>true</c> if using custom UI templates.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous retry operation.</returns>
        /// <remarks>
        /// This method allows users to restart uploads for files that have failed or been canceled during the upload process.
        /// When retrying from the canceled stage, the upload continues from where it was interrupted, preserving any successfully uploaded chunks.
        /// When retrying from the initial stage, the entire upload process starts over from the beginning.
        /// This functionality is essential for handling network issues, server timeouts, or user-initiated cancellations.
        /// </remarks>
        /// <example>
        /// Retry failed uploads with different options:
        /// <code><![CDATA[
        /// // Retry specific files from canceled stage
        /// await uploaderInstance.RetryAsync(failedFiles, fromcanceledStage: true);
        /// 
        /// // Retry all failed files from initial stage
        /// await uploaderInstance.RetryAsync(fromcanceledStage: false);
        /// ]]></code>
        /// </example>
        public async Task RetryAsync(FileInfo[]? fileData = null, bool? fromcanceledStage = null, bool? custom = null)
        {
            await InvokeVoidAsync(_uploaderJsModule!, _uploaderJsInProcessModule!, "retry", [DataId, fileData!, fromcanceledStage!, custom!]).ConfigureAwait(true);
        }
        /// <summary>
        /// Sorts the file data alphabetically based on the file names.
        /// </summary>
        /// <param name="filesData">An array of <see cref="FileInfo"/> objects representing the files to sort. If <c>null</c>, all files in the current list will be sorted.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous operation. The task result contains a <see cref="List{T}"/> of <see cref="FileInfo"/> objects sorted alphabetically by file name.</returns>
        /// <remarks>
        /// This method organizes the file list in alphabetical order based on the file names, making it easier for users to locate specific files.
        /// The sorting is case-insensitive and follows standard alphabetical ordering rules.
        /// If no specific file data is provided, the method will sort all files currently present in the uploader's file list.
        /// </remarks>
        /// <example>
        /// Sort files alphabetically:
        /// <code><![CDATA[
        /// // Sort specific files
        /// List<FileInfo> sortedFiles = await uploaderInstance.SortFileListAsync(selectedFiles);
        /// 
        /// // Sort all files in the list
        /// List<FileInfo> allSortedFiles = await uploaderInstance.SortFileListAsync();
        /// ]]></code>
        /// </example>
        public async Task<List<FileInfo>> SortFileListAsync(FileInfo[]? filesData = null)
        {
            return await InvokeAsync<List<FileInfo>>(_uploaderJsModule!, _uploaderJsInProcessModule!, "sortFileList", [DataId, filesData!]).ConfigureAwait(true);
        }
        /// <summary>
        /// Initiates the upload process manually by calling the save URL action.
        /// </summary>
        /// <param name="files">An array of <see cref="FileInfo"/> objects representing the specific files to upload. If <c>null</c>, all files in the upload queue will be processed.</param>
        /// <param name="custom">A <see cref="bool"/> value indicating whether to use custom file handling. Set to <c>true</c> for custom file processing.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous upload operation.</returns>
        /// <remarks>
        /// This method manually triggers the upload process for files in the uploader component.
        /// If no specific files are provided, all files currently in the upload queue will be uploaded.
        /// The method behavior varies based on the uploader configuration: for server-side uploads, it communicates with the configured SaveUrl,
        /// while for client-side only scenarios, it processes files locally through the upload handler.
        /// This method is useful when you need programmatic control over when uploads should begin.
        /// </remarks>
        /// <example>
        /// Upload files manually:
        /// <code><![CDATA[
        /// // Upload specific files
        /// await uploaderInstance.UploadAsync(selectedFiles);
        /// 
        /// // Upload all queued files
        /// await uploaderInstance.UploadAsync();
        /// 
        /// // Upload with custom handling
        /// await uploaderInstance.UploadAsync(selectedFiles, custom: true);
        /// ]]></code>
        /// </example>
        public async Task UploadAsync(FileInfo[]? files = null, bool? custom = null)
        {
            if (!string.IsNullOrEmpty(UploadAsyncSettings?.SaveUrl))
            {
                await UploadViaServerAsync(files!, custom).ConfigureAwait(false);
            }
            else
            {
                await UploadClientSideAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Uploads files via server-side JavaScript interop.
        /// </summary>
        private async Task UploadViaServerAsync(FileInfo[] files, bool? custom)
        {
            bool? forceCustom = (!ShowFileList && custom == null) ? true : custom;
            await InvokeVoidAsync(_uploaderJsModule!, _uploaderJsInProcessModule!, "upload", [DataId, files, forceCustom!]).ConfigureAwait(true);
        }

        /// <summary>
        /// Uploads files using client-side handler.
        /// </summary>
        private async Task UploadClientSideAsync()
        {
            await UploadHandlerAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves and processes detailed information for the specified files. This method is invoked from JavaScript interop.
        /// </summary>
        /// <param name="file">A <see cref="List{T}"/> of <see cref="FileInfo"/> objects containing the files for which details need to be retrieved.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous file details retrieval operation.</returns>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task GetFileDetailsAsync(List<FileInfo> file)
        {
            if (file != null)
            {
                if (!ShowFileList)
                {
                    await Task.Delay(UI_SYNC_DELAY_MS).ConfigureAwait(false);
                }
                await GetFilesDetailsAsync(file).ConfigureAwait(true);
            }
        }
        /// <summary>
        /// Creates the file list UI based on the provided upload file details. This method is invoked from JavaScript interop to handle server-side file list creation.
        /// </summary>
        /// <param name="fileData">A <see cref="List{T}"/> of <see cref="UploadFileDetails"/> objects containing the detailed information of files to be added to the list.</param>
        /// <param name="isForm">A <see cref="bool"/> value indicating whether the uploader component is rendered inside a form element. Set to <c>true</c> if the component is within a form.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous file list creation operation.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task CreateFileListAsync(List<UploadFileDetails> fileData, bool isForm)
        {
            if (fileData != null)
            {
                await ServerCreateFileListAsync(fileData, isForm).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Clears all files from the uploader component's file list. This method is invoked from JavaScript interop to handle clearing operations.
        /// </summary>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task ClearAllFileAsync()
        {
            await ClearAllHandlerAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Removes file data from the uploader component based on the specified index. This method is invoked from JavaScript interop for file removal operations.
        /// </summary>
        /// <param name="index">An <see cref="int"/> value representing the zero-based index of the file to be removed from the file list.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous file removal operation.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task RemoveFileDataAsync(int index)
        {
            await RemoveFileListAsync(index).ConfigureAwait(true);
        }

        /// <summary>
        /// Updates the file data on the server side with the provided file details. This method is invoked from JavaScript interop to synchronize client-side file changes with the server.
        /// </summary>
        /// <param name="fileData">A <see cref="List{T}"/> of <see cref="UploadFileDetails"/> objects containing the updated file information to be processed on the server.</param>
        /// <param name="isForm">A <see cref="bool"/> value indicating whether the uploader component is rendered inside a form component. Set to <c>true</c> if the component is within a form.</param>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task UpdateServerFileDataAsync(List<UploadFileDetails> fileData, bool isForm)
        {
            if (fileData != null)
            {
                await ServerFileDataAsync(fileData, isForm).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Handles the file selection event when files are selected in the uploader component. This method is invoked from JavaScript interop when the FileSelected event is triggered.
        /// </summary>
        /// <param name="args">A <see cref="SelectedEventArgs"/> object containing information about the selected files and event details.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous event handling operation. The task result contains the processed <see cref="SelectedEventArgs"/> object.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<SelectedEventArgs> SelectedEventAsync(SelectedEventArgs args)
        {
            return await InvokeEventIfHasDelegateAsync(FileSelected, args).ConfigureAwait(true);
        }

        /// <summary>
        /// Handles the file removing event when files are being removed from the uploader component. This method is invoked from JavaScript interop when the OnRemove event is triggered.
        /// </summary>
        /// <param name="args">A <see cref="RemovingEventArgs"/> object containing information about the files being removed and event details.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous event handling operation. The task result contains the processed <see cref="RemovingEventArgs"/> object.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<RemovingEventArgs> RemovingEventAsync(RemovingEventArgs args)
        {
            return await InvokeEventIfHasDelegateAsync(OnRemove, args).ConfigureAwait(true);
        }

        /// <summary>
        /// Handles the action complete event when an upload action is completed in the uploader component. This method is invoked from JavaScript interop when the OnActionComplete event is triggered.
        /// </summary>
        /// <param name="args">A <see cref="ActionCompleteEventArgs"/> object containing information about the completed action and event details.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous event handling operation. The task result contains the processed <see cref="ActionCompleteEventArgs"/> object.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<ActionCompleteEventArgs> ActionCompleteEventAsync(ActionCompleteEventArgs args)
        {
            return await InvokeEventIfHasDelegateAsync(OnActionComplete, args).ConfigureAwait(true);
        }

        /// <summary>
        /// Handles the upload success event when files are successfully uploaded in the uploader component. This method is invoked from JavaScript interop when the Success event is triggered.
        /// </summary>
        /// <param name="args">A <see cref="SuccessEventArgs"/> object containing information about the successfully uploaded files and event details.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous event handling operation. The task result contains the processed <see cref="SuccessEventArgs"/> object.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<SuccessEventArgs> SuccessEventAsync(SuccessEventArgs args)
        {
            return await InvokeEventIfHasDelegateAsync(Success, args).ConfigureAwait(true);
        }

        /// <summary>
        /// Handles the value change event when the uploader component's state changes. This method is invoked from JavaScript interop when the ValueChange event is triggered.
        /// </summary>
        /// <param name="args">A <see cref="UploadChangeEventArgs"/> object containing information about the changed values and event details.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous event handling operation.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task ChangeEventAsync(UploadChangeEventArgs args)
        {
            if (!string.IsNullOrEmpty(UploadAsyncSettings?.SaveUrl) && args != null)
            {
                args.Files = null;
            }

            if (OnValueChange.HasDelegate)
            {
                await OnValueChange.InvokeAsync(args).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Handles the upload failure event when files fail to upload in the uploader component. This method is invoked from JavaScript interop when the OnFailure event is triggered.
        /// </summary>
        /// <param name="args">A <see cref="FailureEventArgs"/> object containing information about the failed upload and error details.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous event handling operation. The task result contains the processed <see cref="FailureEventArgs"/> object.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<FailureEventArgs> FailureEventAsync(FailureEventArgs args)
        {
            if (OnFailure.HasDelegate)
            {
                await OnFailure.InvokeAsync(args).ConfigureAwait(true);
            }

            return args;
        }

        /// <summary>
        /// Handles the chunk upload failure event when individual file chunks fail to upload in the uploader component. This method is invoked from JavaScript interop when the OnChunkFailure event is triggered.
        /// </summary>
        /// <param name="args">A <see cref="FailureEventArgs"/> object containing information about the failed chunk upload and error details.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous event handling operation. The task result contains the processed <see cref="FailureEventArgs"/> object.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<FailureEventArgs> ChunkFailureEventAsync(FailureEventArgs args)
        {
            if (OnChunkFailure.HasDelegate)
            {
                await OnChunkFailure.InvokeAsync(args).ConfigureAwait(true);
            }

            return args;
        }

        /// <summary>
        /// Handles the file list rendering event when the file list UI is being rendered in the uploader component. This method is invoked from JavaScript interop when the OnFileListRender event is triggered.
        /// </summary>
        /// <param name="args">A <see cref="FileListRenderingEventArgs"/> object containing information about the file list rendering process and customization options.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous event handling operation.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task FileListRenderingEventAsync(FileListRenderingEventArgs args)
        {
            if (OnFileListRender.HasDelegate)
            {
                await OnFileListRender.InvokeAsync(args).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Handles the upload progress event when files are being uploaded in the uploader component. This method is invoked from JavaScript interop when the Progressing event is triggered.
        /// </summary>
        /// <param name="args">A <see cref="ProgressEventArgs"/> object containing information about the upload progress, including percentage completion and file details.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous event handling operation. The task result contains the processed <see cref="ProgressEventArgs"/> object.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<ProgressEventArgs> ProgressEventAsync(ProgressEventArgs args)
        {
            if (Progressing.HasDelegate)
            {
                await Progressing.InvokeAsync(args).ConfigureAwait(true);
            }

            return args;
        }

        /// <summary>
        /// Handles the upload canceling event when file uploads are being canceled in the uploader component. This method is invoked from JavaScript interop when the OnCancel event is triggered.
        /// </summary>
        /// <param name="args">A <see cref="CancelEventArgs"/> object containing information about the files being canceled and event details.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous event handling operation. The task result contains the processed <see cref="CancelEventArgs"/> object.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<CancelEventArgs> CancelingEventAsync(CancelEventArgs args)
        {
            if (OnCancel.HasDelegate)
            {
                await OnCancel.InvokeAsync(args).ConfigureAwait(true);
            }

            return args;
        }

        /// <summary>
        /// Handles the upload starting event when files begin uploading in the uploader component. This method is invoked from JavaScript interop when the OnUploadStart event is triggered.
        /// </summary>
        /// <param name="args">A <see cref="UploadingEventArgs"/> object containing information about the files starting to upload and event details.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous event handling operation. The task result contains the processed <see cref="UploadingEventArgs"/> object.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<UploadingEventArgs> UploadingEventAsync(UploadingEventArgs args)
        {
            if (OnUploadStart.HasDelegate)
            {
                await OnUploadStart.InvokeAsync(args).ConfigureAwait(true);
            }

            return args;
        }

        /// <summary>
        /// Handles the chunk upload starting event when individual file chunks begin uploading in the uploader component. This method is invoked from JavaScript interop when the OnChunkUploadStart event is triggered.
        /// </summary>
        /// <param name="args">A <see cref="UploadingEventArgs"/> object containing information about the file chunks starting to upload and event details.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous event handling operation. The task result contains the processed <see cref="UploadingEventArgs"/> object.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<UploadingEventArgs> ChunkUploadingEventAsync(UploadingEventArgs args)
        {
            if (OnChunkUploadStart.HasDelegate)
            {
                await OnChunkUploadStart.InvokeAsync(args).ConfigureAwait(true);
            }

            return args;
        }

        /// <summary>
        /// Handles the chunk upload success event when individual file chunks are successfully uploaded in the uploader component. This method is invoked from JavaScript interop when the OnChunkSuccess event is triggered.
        /// </summary>
        /// <param name="args">A <see cref="SuccessEventArgs"/> object containing information about the successfully uploaded file chunk and event details.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous event handling operation. The task result contains the processed <see cref="SuccessEventArgs"/> object.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<SuccessEventArgs> ChunkSuccessEventAsync(SuccessEventArgs args)
        {
            if (OnChunkSuccess.HasDelegate)
            {
                await OnChunkSuccess.InvokeAsync(args).ConfigureAwait(true);
            }

            return args;
        }

        /// <summary>
        /// Handles the upload pausing event when file uploads are being paused in the uploader component. This method is invoked from JavaScript interop when the Paused event is triggered.
        /// </summary>
        /// <param name="args">A <see cref="PauseResumeEventArgs"/> object containing information about the files being paused and event details.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous event handling operation. The task result contains the processed <see cref="PauseResumeEventArgs"/> object.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<PauseResumeEventArgs> PausingEventAsync(PauseResumeEventArgs args)
        {
            if (Paused.HasDelegate)
            {
                await Paused.InvokeAsync(args).ConfigureAwait(true);
            }

            return args;
        }

        /// <summary>
        /// Handles the upload resuming event when paused file uploads are being resumed in the uploader component. This method is invoked from JavaScript interop when the OnResume event is triggered.
        /// </summary>
        /// <param name="args">A <see cref="PauseResumeEventArgs"/> object containing information about the files being resumed and event details.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous event handling operation. The task result contains the processed <see cref="PauseResumeEventArgs"/> object.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<PauseResumeEventArgs> ResumingEventAsync(PauseResumeEventArgs args)
        {
            if (OnResume.HasDelegate)
            {
                await OnResume.InvokeAsync(args).ConfigureAwait(true);
            }

            return args;
        }

        /// <summary>
        /// Handles the before upload event that occurs just before files begin uploading in the uploader component. This method is invoked from JavaScript interop when the BeforeUpload event is triggered.
        /// </summary>
        /// <param name="args">A <see cref="BeforeUploadEventArgs"/> object containing information about the files that are about to be uploaded and event details that can be modified or canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous event handling operation. The task result contains the processed <see cref="BeforeUploadEventArgs"/> object.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<BeforeUploadEventArgs> BeforeUploadEventAsync(BeforeUploadEventArgs args)
        {
            if (BeforeUpload.HasDelegate)
            {
                await BeforeUpload.InvokeAsync(args).ConfigureAwait(true);
            }

            return args;
        }

        /// <summary>
        /// Handles the before remove event that occurs just before files are removed from the uploader component. This method is invoked from JavaScript interop when the BeforeRemove event is triggered.
        /// </summary>
        /// <param name="args">A <see cref="BeforeRemoveEventArgs"/> object containing information about the files that are about to be removed and event details that can be modified or canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous event handling operation. The task result contains the processed <see cref="BeforeRemoveEventArgs"/> object.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<BeforeRemoveEventArgs> BeforeRemoveEventAsync(BeforeRemoveEventArgs args)
        {
            if (BeforeRemove.HasDelegate)
            {
                await BeforeRemove.InvokeAsync(args).ConfigureAwait(true);
            }

            return args;
        }

        /// <summary>
        /// Handles the clearing event that occurs when all files are being cleared from the uploader component. This method is invoked from JavaScript interop when the OnClear event is triggered.
        /// </summary>
        /// <param name="args">A <see cref="ClearingEventArgs"/> object containing information about the clearing operation and event details that can be modified or canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous event handling operation. The task result contains the processed <see cref="ClearingEventArgs"/> object.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<ClearingEventArgs> ClearingEventAsync(ClearingEventArgs args)
        {
            if (OnClear.HasDelegate)
            {
                await OnClear.InvokeAsync(args).ConfigureAwait(true);
            }

            return args;
        }
    }
}
