using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    /// <summary>
    /// Represents a component that specifies the list of files that will be preloaded on rendering of the <see cref="SfUploader"/> component.
    /// </summary>
    /// <remarks>
    /// The <see cref="UploadedFiles"/> component allows you to define files that should be displayed as already uploaded when the <see cref="SfUploader"/> component is initially rendered.
    /// This is useful for scenarios where you want to show previously uploaded files or files that exist on the server.
    /// The component manages a collection of <see cref="UploadedFile"/> objects that represent the preloaded files.
    /// </remarks>
    /// <example>
    /// The following example shows how to use the <see cref="UploadedFiles"/> component to preload files in an uploader.
    /// <code><![CDATA[
    /// <SfUploader>
    ///     <UploadedFiles>
    ///         <UploadedFile Name="document.pdf" Size="1024000" Type="application/pdf"></UploadedFile>
    ///         <UploadedFile Name="image.jpg" Size="512000" Type="image/jpeg"></UploadedFile>
    ///     </UploadedFiles>
    /// </SfUploader>
    /// ]]></code>
    /// </example>
    public partial class UploadedFiles : SfBaseComponent
    {
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
        /// Gets or sets the collection of uploaded files that are preloaded in the <see cref="SfUploader"/> component.
        /// </summary>
        /// <value>
        /// A <see cref="List{T}"/> of <see cref="UploadedFile"/> objects representing the preloaded files. The default value is an empty list.
        /// </value>
        /// <remarks>
        /// This property contains the list of files that will be displayed as already uploaded when the <see cref="SfUploader"/> component is rendered.
        /// Each file in the collection is represented by an <see cref="UploadedFile"/> object that contains file metadata such as name, size, and type.
        /// The files in this collection are automatically added to the uploader's file list during component initialization.
        /// </remarks>
        /// <example>
        /// The following example shows how to access and work with the Files collection.
        /// <code><![CDATA[
        /// // Access the files collection
        /// var uploaderFiles = new UploadedFiles();
        /// uploaderFiles.Files.Add(new UploadedFile 
        /// { 
        ///     Name = "document.pdf", 
        ///     Size = 1024000, 
        ///     Type = "application/pdf" 
        /// });
        /// ]]></code>
        /// </example>
        public List<UploadedFile> Files { get; set; } = [];

        /// <summary>
        /// Updates the child property by adding a file to the Files collection.
        /// </summary>
        /// <param name="file">The <see cref="UploadedFile"/> object to be added to the Files collection.</param>
        /// <remarks>
        /// This internal method is used by the component infrastructure to add individual file instances to the Files collection.
        /// It is typically called during the component's initialization phase when child <see cref="UploadedFile"/> components are processed.
        /// The method provides a way for child components to register themselves with the parent <see cref="UploadedFiles"/> component.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/> is null.</exception>
        internal void UpdateChildProperty(UploadedFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file), "File cannot be null.");
            }
            Files.Add(file);
        }

        /// <summary>
        /// Executes initialization logic when the component is first rendered.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous initialization operation.</returns>
        /// <remarks>
        /// This method is called during the component's initialization phase and is responsible for notifying the parent <see cref="SfUploader"/> component
        /// about the Files collection. The method ensures that all preloaded files are properly registered with the parent uploader component
        /// so they can be displayed in the file list when the component is rendered.
        /// </remarks>
        /// <exception cref="InvalidOperationException">Thrown when the component is not properly configured or the parent uploader is not available.</exception>
        /// <exclude/>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(true);
            Parent?.UpdateChildProperties("Files", Files);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="UploadedFiles"/> component and optionally releases the managed resources.
        /// </summary>
        /// <remarks>
        /// This method is called when the component is being disposed to clean up resources and prevent memory leaks.
        /// </remarks>
        /// <exclude/>
        protected override ValueTask DisposeAsyncCore()
        {
            Parent = null;
            Files?.Clear();
            Files = null!;
            return base.DisposeAsyncCore();
        }
    }
}