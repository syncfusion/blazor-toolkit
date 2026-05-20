using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Internal;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    /// <summary>
    /// Represents a list of files that are preloaded and displayed in the <see cref="SfUploader"/> component during initial rendering.
    /// </summary>
    /// <remarks>
    /// The <see cref="UploaderUploadedFile"/> class is used to configure files that should appear as already uploaded when the <see cref="SfUploader"/> component is first rendered.
    /// This is useful for scenarios where you need to show existing files that were previously uploaded, allowing users to see and manage them alongside new uploads.
    /// Each instance represents a single uploaded file with properties like name, size, and type that define the file's metadata.
    /// </remarks>
    /// <example>
    /// The following example demonstrates how to configure preloaded files in the Uploader component:
    /// <code><![CDATA[
    /// <SfUploader>
    ///     <UploaderFiles>
    ///         <UploaderUploadedFile Name="Document1.pdf" Size="2048576" Type="application/pdf" />
    ///         <UploaderUploadedFile Name="Image1.jpg" Size="1024000" Type="image/jpeg" />
    ///     </UploaderFiles>
    /// </SfUploader>
    /// ]]></code>
    /// </example>
    public class UploaderUploadedFile : SfBaseComponent
    {
        [CascadingParameter]
        private UploaderFiles? Parent { get; set; }

        /// <summary>
        /// Specifies the content that has to be passed.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// Gets or sets the name of the uploaded file.
        /// </summary>
        /// <value>
        /// A <c>string</c> representing the name of the uploaded file. The default value is <c>String.Empty</c>.
        /// </value>
        /// <remarks>
        /// This property specifies the display name of the file that will be shown in the <see cref="SfUploader"/> component.
        /// The name should include the file extension to properly identify the file type and provide a meaningful display name to users.
        /// </remarks>
        [Parameter]
        public string Name { get; set; } = string.Empty;

        private string? _name;

        /// <summary>
        /// Gets or sets the size of the uploaded file in bytes.
        /// </summary>
        /// <value>
        /// A <c>double</c> representing the file size in bytes. The default value is 0.
        /// </value>
        /// <remarks>
        /// This property defines the size of the uploaded file in bytes, which is used by the <see cref="SfUploader"/> component
        /// to display file size information to users. The size is typically displayed in a human-readable format (KB, MB, etc.)
        /// in the uploader's file list interface.
        /// </remarks>
        [Parameter]
        public double Size { get; set; }

        private double _size;

        /// <summary>
        /// Gets or sets the MIME type of the uploaded file.
        /// </summary>
        /// <value>
        /// A <c>string</c> representing the MIME type of the uploaded file. The default value is <c>String.Empty</c>.
        /// </value>
        /// <remarks>
        /// This property specifies the MIME type (media type) of the file, which helps identify the file format and content type.
        /// Common examples include "image/jpeg" for JPEG images, "application/pdf" for PDF documents, or "text/plain" for text files.
        /// The MIME type is used by the <see cref="SfUploader"/> component for file type validation and display purposes.
        /// </remarks>
        [Parameter]
        public string Type { get; set; } = string.Empty;

        private string? _type;

        /// <summary>
        /// Triggers while initial rendering of the component.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(true);
            Parent?.UpdateChildProperty(this);
            UpdatePrivateProperty();
        }

        /// <summary>
        /// Triggers while dynamically updating the component properties.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(true);
            if (IsPropertyChanged())
            {
                UpdatePrivateProperty();
            }
        }

        /// <summary>
        /// Determines whether any of the component's parameters have changed.
        /// </summary>
        /// <returns><c>true</c> if any property has changed; otherwise, <c>false</c>.</returns>
        private bool IsPropertyChanged()
        {
            bool isChanged = !string.Equals(Name, _name, StringComparison.Ordinal) ||
                             !SfBaseUtils.Equals(Size, _size) ||
                             !string.Equals(Type, _type, StringComparison.Ordinal);

            return isChanged;
        }

        /// <summary>
        /// Updates the private backing fields with the current parameter values.
        /// </summary>
        private void UpdatePrivateProperty()
        {
            _name = Name;
            _size = Size;
            _type = Type;
        }

        /// <summary>
        /// Releases the resources used by the component.
        /// </summary>
        protected override ValueTask DisposeAsyncCore()
        {
            Parent = null;
            return base.DisposeAsyncCore();
        }
    }
}