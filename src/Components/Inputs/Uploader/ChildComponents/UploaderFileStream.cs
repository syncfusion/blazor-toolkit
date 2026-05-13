using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Inputs.Internal
{
    internal class UploaderStreamReader
    {
        /// <exclude/>
        internal class UploadData
        {
            public int FileByte { get; set; }

            public string Result { get; set; } = string.Empty;
        }
    }

    /// <summary>
    /// Represents the upload file details containing chunk size information for file upload operations.
    /// </summary>
    /// <remarks>
    /// This class extends <see cref="FileInfo"/> and provides additional properties to manage chunk-based file uploads.
    /// It contains information about the current chunk size and total chunk size for tracking upload progress.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// var uploadDetails = new UploadFileDetails
    /// {
    ///     ChunkSize = 1024,
    ///     TotalChunkSize = 10240
    /// };
    /// ]]></code>
    /// </example>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class UploadFileDetails : FileInfo
    {
        /// <summary>
        /// Gets or sets the size of the current chunk being uploaded.
        /// </summary>
        /// <value>
        /// A <see cref="long"/> value representing the size of the current chunk in bytes. The default value is 0.
        /// </value>
        /// <remarks>
        /// This property specifies the size of the individual chunk being processed during file upload operations.
        /// It is used to track the progress of chunked uploads and ensure proper handling of file segments.
        /// </remarks>
        public long ChunkSize { get; set; }

        /// <summary>
        /// Gets or sets the total size of all chunks for the file upload.
        /// </summary>
        /// <value>
        /// A <see cref="long"/> value representing the total size of all chunks combined in bytes. The default value is 0.
        /// </value>
        /// <remarks>
        /// This property represents the cumulative size of all chunks that make up the complete file being uploaded.
        /// It is used to calculate upload progress and verify the completion of the entire file transfer process.
        /// </remarks>
        public long TotalChunkSize { get; set; }
    }
}