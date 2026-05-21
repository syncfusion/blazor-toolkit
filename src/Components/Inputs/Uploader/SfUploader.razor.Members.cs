using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Internal;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    /// <summary>
    /// Represents the Syncfusion Blazor File Upload component that provides an interface for uploading files with drag-and-drop support, progress tracking, and validation features.
    /// </summary>
    public partial class SfUploader
    {
        /// <summary>
        /// Gets the ID of the Uploader component.
        /// </summary>
        /// <value>
        /// Accepts the string value.
        /// </value>
        [Parameter]
        public string ID { get; set; } = string.Empty;

        /// <summary>
        /// Specifies the content that has to be passed.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// Gets or sets the extensions of the file types allowed in the Uploader component, passing the extensions with the comma separators.
        /// </summary>
        /// <value>
        /// Accepts the string value. The default value is empty.
        /// </value>
        /// <remarks>
        /// For example, if you want to upload specific image files, pass the property as ".jpg" and ".png."
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Inputs
        /// <SfUploader ID="UploadFiles" AllowedExtensions=".jpg, .png">
        ///     <UploaderAsyncSettings SaveUrl="api/SampleData/Save" RemoveUrl="api/SampleData/Remove"/>
        /// </SfUploader>
        /// ]]></code>
        /// </example>
        [Parameter]
        public string AllowedExtensions { get; set; } = string.Empty;

        private string? _allowedExtensions;

        private UploaderAsyncSettings? _asyncSettings;

        /// <summary>
        /// Gets or sets the internal uploader async settings for the component.
        /// </summary>
        internal UploaderAsyncSettings? UploadAsyncSettings { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to prevent cross-site scripting code in filenames.
        /// </summary>
        /// <value>
        /// <c>true</c> if the component prevents cross-site scripting code in filenames; otherwise, <c>false</c>. The default value is <c>true</c>.
        /// </value>
        /// <remarks>
        /// The EnableHtmlSanitizer property removes cross-site scripting code or functions from the filename and shows a validation error message to the user.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Inputs
        /// <SfUploader ID="UploadFiles" EnableHtmlSanitizer="false">
        ///     <UploaderAsyncSettings SaveUrl="api/SampleData/Save" RemoveUrl="api/SampleData/Remove" />
        /// </SfUploader>
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool EnableHtmlSanitizer { get; set; } = true;

        private bool _enableHtmlSanitizer;

        /// <summary>
        /// Gets or sets whether the <see cref="SfUploader"/> component initiates automatic upload after the files are selected.
        /// </summary>
        /// <value>
        /// <c>true</c>, if the automatic upload option can be enabled in component. Otherwise, <c>false</c>. The default value is <c>true</c>.
        /// </value>
        /// <remarks>
        /// If you want to manipulate the files before uploading to server, disable the <see cref="AutoUpload"/> property. 
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Inputs
        /// <SfUploader ID="UploadFiles" AutoUpload="false">
        ///     <UploaderAsyncSettings SaveUrl="api/SampleData/Save" RemoveUrl="api/SampleData/Remove" />
        /// </SfUploader>
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool AutoUpload { get; set; } = true;

        private bool _autoUpload;

        /// <summary>
        /// Gets or sets a value indicating whether to show the progress bar while uploading a file.
        /// </summary>
        /// <value>
        /// <c>true</c> if the progress bar should be shown; otherwise, <c>false</c>. The default value is <c>true</c>.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Inputs
        /// <SfUploader ID="UploadFiles" ShowProgressBar="false">
        ///     <UploaderAsyncSettings SaveUrl="api/SampleData/Save" RemoveUrl="api/SampleData/Remove" />
        /// </SfUploader>
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool ShowProgressBar { get; set; } = true;

        private UploaderButtons? _buttons;

        /// <summary>
        /// Gets or sets the internal uploader buttons configuration for the component.
        /// </summary>
        private UploaderButtons? UploadButtons { get; set; }

        /// <summary>
        /// Gets or sets one or more CSS classes that can be used to customize the appearance of a file upload component.
        /// </summary>
        /// <value>
        /// Accepts the CSS class string separated by space to customize the appearance of component.
        /// </value>
        /// <remarks>
        /// One or more custom CSS classes can be added to a file upload.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Inputs
        /// <SfUploader ID="UploadFiles" CssClass="custom-uploader my-class">
        ///     <UploaderAsyncSettings SaveUrl="api/SampleData/Save" RemoveUrl="api/SampleData/Remove" />
        /// </SfUploader>
        /// ]]></code>
        /// </example>
        [Parameter]
        public string CssClass { get; set; } = string.Empty;

        private string? _cssClass;

        /// <summary>
        /// Gets or sets a value indicating whether folders can be browsed and uploaded in the file upload component.
        /// </summary>
        /// <value>
        /// <c>true</c> if directory upload is enabled; otherwise, <c>false</c>. The default value is <c>false</c>.
        /// </value>
        [Parameter]
        public bool DirectoryUpload { get; set; }

        private bool _directoryUpload;

        /// <summary>
        /// Gets or sets the custom file drop target element selectors to handle file upload on drag-and-drop action.
        /// </summary>
        /// <value>
        /// Accepts the target element selector string.
        /// </value>
        /// <remarks>
        /// By default, the file upload component creates a container element around the file input that will act as a drop target.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Inputs
        /// <SfUploader DropArea="#CustomDropArea" >
        ///     <UploaderAsyncSettings SaveUrl="api/SampleData/Save" RemoveUrl="api/SampleData/Remove" />
        /// </SfUploader>
        /// <div id="CustomDropArea" style="width:200px; height:200px; border:dashed 1px" ></div>
        /// ]]></code>
        /// </example>
        [Parameter]
        public string? DropArea { get; set; }

        private string? _dropArea;

        /// <summary>
        /// Gets or sets the cursor displayed while dragging the file into the <see cref="SfUploader"/> component. It indicates which type of operation will occur.
        /// </summary>
        /// <value>
        /// One of the <see cref="Inputs.DropEffect" /> enumeration that specifies the drag operation for the component. The default value is <see cref="DropEffect.Default" />.
        /// </value>
        /// <remarks>
        ///  The <c>DropEffect</c> property can be set to one of the following values:
        /// <list type="bullet">
        /// <item>
        /// <term>Copy</term> The mouse cursor shows a copy symbol when dragging and dropping the files.
        /// </item>
        /// <item>
        /// <term>Move</term> The mouse cursor shows a move symbol when dragging and dropping the files.
        /// </item>
        /// <item>
        /// <term>Link</term> The mouse cursor shows a link symbol when dragging and dropping the files.
        /// </item>
        /// <item>
        /// <term>None</term> The files are not allowed to be dropped.
        /// </item>
        /// </list>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Inputs
        /// <SfUploader ID="UploadFiles" DropEffect="DropEffect.Copy">
        ///     <UploaderAsyncSettings SaveUrl="api/SampleData/Save" RemoveUrl="api/SampleData/Remove" />
        /// </SfUploader>
        /// ]]></code>
        /// </example>
        [Parameter]
        public DropEffect DropEffect { get; set; } = DropEffect.Default;

        private DropEffect _dropEffect;

        /// <summary>
        /// Gets or sets a value indicating whether the file upload state should be persisted on page reload. If enabled, the state of uploaded or selected files will be maintained.
        /// </summary>
        /// <value>
        /// <c>true</c> if persistence is enabled; otherwise, <c>false</c>. The default value is <c>false</c>.
        /// </value>
        [Parameter]
        public bool EnablePersistence { get; set; } = false;

        private bool _enablePersistence;

        /// <summary>
        /// Gets or sets a value that indicates whether the <see cref="SfUploader"/> component allows user interaction.
        /// </summary>
        /// <value>
        /// <c>true</c> if the user can interact with the component; otherwise, <c>false</c>. The default value is <c>true</c>.
        /// </value>
        [Parameter]
        public bool Disabled { get; set; } = false;

        private bool _disabled;

        private List<UploadedFile>? _files;

        /// <summary>
        /// Gets or sets the internal uploaded files collection for the component.
        /// </summary>
        private List<UploadedFile>? UploadedFiles { get; set; }

        /// <summary>
        /// Gets or sets an additional html attributes such as styles, class, and more to add the root element.
        /// </summary>
        /// <value>
        /// A dictionary of additional HTML attributes for the root element of the component.
        /// </value>
        /// <remarks>
        /// If you configured both property and equivalent html attributes, the component considers the property value.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Inputs
        /// <SfUploader ID="UploadFiles" HtmlAttributes="@HtmlAttribute">
        ///     <UploaderAsyncSettings SaveUrl="api/SampleData/Save" RemoveUrl="api/SampleData/Remove" />
        /// </SfUploader>
        /// @code {
        ///     Dictionary<string, object> HtmlAttribute = new Dictionary<string, object>() {
        ///     {"disabled","true" }
        ///     };
        ///  }
        /// ]]></code>
        /// </example>
        [Parameter]
        public Dictionary<string, object> HtmlAttributes { get; set; } = [];

        /// <summary>
        /// Gets or sets an additional input attributes such as disabled, value, and more to add the input file element.
        /// </summary>
        /// <value>
        /// A dictionary of additional input attributes for the root element of the component.
        /// </value>
        /// <remarks>
        /// If you configured both property and equivalent input attribute, the component considers the property value.
        /// </remarks>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> InputAttributes { get; set; } = [];

        /// <summary>
        /// Gets or sets the maximum allowed file size to be uploaded in bytes.
        /// </summary>
        /// <value>
        /// Accepts the double value representing that the maximum file size for the component. The default value is <c>30000000</c>.
        /// </value>
        /// <remarks>
        /// The property used to make sure that you cannot upload too large files.
        /// </remarks>
        [Parameter]
        public double MaxFileSize { get; set; } = 30000000;

        private double _maxFileSize;

        /// <summary>
        /// Gets or sets the minimum file size to be uploaded in bytes.
        /// </summary>
        /// <value>
        /// A double value representing the minimum file size for the component. The default value is <c>0</c>.
        /// </value>
        [Parameter]
        public double MinFileSize { get; set; }

        private double _minFileSize;

        /// <summary>
        /// Gets or sets a value that indicates whether multiple files can be browsed or dropped simultaneously in the <see cref="SfUploader"/> component.
        /// </summary>
        /// <value>
        /// <c>true</c> if multiple file selection is enabled; otherwise, <c>false</c>. The default value is <c>true</c>.
        /// </value>
        [Parameter]
        public bool AllowMultiple { get; set; } = true;

        private bool _allowMultiple;

        /// <summary>
        /// Gets or sets a value that indicates whether the <see cref="SfUploader"/> component processes multiple files sequentially rather than simultaneously.
        /// </summary>
        /// <value>
        /// <c>true</c> if sequential upload is enabled; otherwise, <c>false</c>. The default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// When the SequentialUpload property is enabled, the file upload component uploads files one after another instead of simultaneously.
        /// </remarks>
        [Parameter]
        public bool SequentialUpload { get; set; }

        private bool _sequentialUpload;

        /// <summary>
        /// Gets or sets a value that indicates whether the file list should be rendered.
        /// </summary>
        /// <value>
        /// <c>true</c> if the file list should be shown; otherwise, <c>false</c>. The default value is <c>true</c>.
        /// </value>
        /// <remarks>
        /// This property is used to hide the default file list and design your own template for the file list using the <see cref="Template"/> property.
        /// </remarks>
        [Parameter]
        public bool ShowFileList { get; set; } = true;

        private bool _showFileList;

        /// <summary>
        /// Gets or sets the tab order of the component.
        /// </summary>
        /// <value>
        /// An integer value representing the tab index of the Uploader component. The default value is <c>0</c>.
        /// </value>
        [Parameter]
        public int TabIndex { get; set; }

        /// <summary>
        /// Gets or sets a template that is used to customize the content of each file in the list.
        /// </summary>
        /// <value> 
        /// The template content. The default value is <c>null</c>. 
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Inputs
        /// @inject HttpClient Http
        /// <SfUploader ID="UploadFiles" DropEffect="DropEffect.Copy">
        ///     <UploaderAsyncSettings SaveUrl="api/SampleData/Save" RemoveUrl="api/SampleData/Remove"/>
        ///     <UploaderTemplates>
        ///         <Template Context="HttpContext">
        ///             <div style="padding: 7px;">
        ///                 <h5 title="@(HttpContext.Name)">@(HttpContext.Name)</h5>
        ///                 <i>@(HttpContext.Size) Bytes</i>
        ///             </div>
        ///         </Template>
        ///     </UploaderTemplates>
        /// </SfUploader>
        /// ]]></code>
        /// </example>
        [Parameter]
        public RenderFragment<FileInfo>? Template { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="HttpClient"/> instance used by the <see cref="SfUploader"/> component
        /// for performing upload and remove operations.
        /// </summary>
        /// <value>
        /// The <see cref="HttpClient"/> instance that handles HTTP requests for the <see cref="SfUploader"/> component.
        /// If not set, the component will use the globally injected <see cref="HttpClient"/> instance from the application.
        /// </value>
        /// <remarks>
        /// Use this property to bind a custom <see cref="HttpClient"/> instance to the <see cref="SfUploader"/> component.
        /// This allows for centralized management of HTTP configurations such as headers, base addresses, and timeouts,
        /// ensuring consistent HTTP settings across your application.
        /// <para>
        /// If <c>HttpClientInstance</c> is not specified, the component defaults to using the globally injected
        /// <see cref="HttpClient"/> instance provided in the application's <c>Program.cs</c> or <c>Startup.cs</c>.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @inject HttpClient httpClient
        /// <SfUploader ID="UploadFiles" HttpClientInstance="@httpClient">
        ///     <UploaderAsyncSettings SaveUrl="api/SampleData/Save"
        ///                            RemoveUrl="api/SampleData/Remove"/>
        /// </SfUploader>
        /// ]]></code>
        /// This example demonstrates setting the <c>HttpClientInstance</c> property to a globally injected
        /// <see cref="HttpClient"/> instance, enabling shared HTTP configurations within the <see cref="SfUploader"/> component.
        /// </example>
        [Parameter]
        public HttpClient HttpClientInstance { get; set; } = new();

        /// <summary>
        /// Initializes component properties to their default values and generates component ID if not provided.
        /// Called during component initialization phase to set up child properties and establish property change tracking.
        /// </summary>
        /// <exclude/>
        protected void PropertyInitialized()
        {
            UpdateChildProperties("Buttons", UploadButtons);
            UpdateChildProperties("Files", UploadedFiles);
            UpdateChildProperties("AsyncSettings", UploadAsyncSettings);
            UpdatePrivateProperty();
            if (string.IsNullOrEmpty(ID))
            {
                ID = SfBaseUtils.GenerateID("uploader");
            }
            DataId = ID;
        }

        /// <summary>
        /// Synchronizes child component properties (Buttons, Files, AsyncSettings) with their internal counterparts
        /// when component parameters are set. This method is called during the parameter update lifecycle.
        /// </summary>
        /// <exclude/>
        protected void PropertyParametersSet()
        {
            if (IsPropertyChanged())
            {
                _asyncSettings = UploadAsyncSettings;
                _buttons = UploadButtons;
            }
        }

        /// <summary>
        /// Updates internal property storage with current public property values for change detection.
        /// This enables tracking of property modifications between render cycles by maintaining
        /// a snapshot of previous values for comparison.
        /// </summary>
        private void UpdatePrivateProperty()
        {
            _allowedExtensions = AllowedExtensions;
            _enableHtmlSanitizer = EnableHtmlSanitizer;
            _autoUpload = AutoUpload;
            _cssClass = CssClass;
            _directoryUpload = DirectoryUpload;
            _dropArea = DropArea;
            _dropEffect = DropEffect;
            _enablePersistence = EnablePersistence;
            _disabled = Disabled;
            _files = UploadedFiles;
            _maxFileSize = MaxFileSize;
            _minFileSize = MinFileSize;
            _allowMultiple = AllowMultiple;
            _sequentialUpload = SequentialUpload;
            _showFileList = ShowFileList;
        }

        /// <summary>
        /// Determines whether any of the component properties have changed from their previous values.
        /// Uses helper methods to track individual property changes and update the PropertyChanges dictionary.
        /// </summary>
        /// <returns><c>true</c> if any property has changed; otherwise, <c>false</c>.</returns>
        private bool IsPropertyChanged()
        {
            bool isChanged = false;
            isChanged |= CheckStringPropertyChange(nameof(AllowedExtensions), AllowedExtensions, _allowedExtensions);
            isChanged |= CheckStringPropertyChange(nameof(CssClass), CssClass, _cssClass);
            isChanged |= CheckObjectPropertyChange(nameof(UploadAsyncSettings), UploadAsyncSettings, _asyncSettings);
            isChanged |= CheckBooleanPropertyChange(nameof(EnableHtmlSanitizer), EnableHtmlSanitizer, _enableHtmlSanitizer);
            isChanged |= CheckBooleanPropertyChange(nameof(DirectoryUpload), DirectoryUpload, _directoryUpload);
            isChanged |= CheckBooleanPropertyChange(nameof(EnablePersistence), EnablePersistence, _enablePersistence);
            isChanged |= CheckBooleanPropertyChange(nameof(Disabled), Disabled, _disabled);
            isChanged |= CheckBooleanPropertyChange(nameof(ShowFileList), ShowFileList, _showFileList);
            isChanged |= CheckBooleanPropertyChange(nameof(AllowMultiple), AllowMultiple, _allowMultiple);
            isChanged |= CheckObjectPropertyChange(nameof(DropEffect), DropEffect, _dropEffect);
            isChanged |= CheckObjectPropertyChange(nameof(UploadButtons), UploadButtons, _buttons);
            isChanged |= CheckObjectPropertyChange(nameof(DropArea), DropArea, _dropArea);
            isChanged |= CheckObjectPropertyChange(nameof(UploadedFiles), UploadedFiles, _files);
            isChanged |= CheckObjectPropertyChange(nameof(MaxFileSize), MaxFileSize, _maxFileSize);
            isChanged |= CheckObjectPropertyChange(nameof(MinFileSize), MinFileSize, _minFileSize);
            isChanged |= CheckBooleanPropertyChange(nameof(AutoUpload), AutoUpload, _autoUpload);
            isChanged |= CheckBooleanPropertyChange(nameof(SequentialUpload), SequentialUpload, _sequentialUpload);
            return isChanged;
        }

        /// <summary>
        /// Checks if a string property has changed and updates the PropertyChanges dictionary if it has.
        /// Uses ordinal string comparison for performance and culture-independent comparison.
        /// </summary>
        /// <param name="propertyName">The name of the property being checked.</param>
        /// <param name="currentValue">The current value of the property.</param>
        /// <param name="previousValue">The previous value of the property.</param>
        /// <returns><c>true</c> if the property has changed; otherwise, <c>false</c>.</returns>
        private bool CheckStringPropertyChange(string propertyName, string? currentValue, string? previousValue)
        {
            if (!string.Equals(currentValue, previousValue, StringComparison.Ordinal))
            {
                _ = SfBaseUtils.UpdateDictionary(propertyName, currentValue!, PropertyChanges);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if a boolean property has changed and updates the PropertyChanges dictionary if it has.
        /// </summary>
        /// <param name="propertyName">The name of the property being checked.</param>
        /// <param name="currentValue">The current value of the property.</param>
        /// <param name="previousValue">The previous value of the property.</param>
        /// <returns><c>true</c> if the property has changed; otherwise, <c>false</c>.</returns>
        private bool CheckBooleanPropertyChange(string propertyName, bool currentValue, bool previousValue)
        {
            if (currentValue != previousValue)
            {
                _ = SfBaseUtils.UpdateDictionary(propertyName, currentValue, PropertyChanges);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if an object property has changed and updates the PropertyChanges dictionary if it has.
        /// Uses SfBaseUtils.Equals for complex object comparison.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="propertyName">The name of the property being checked.</param>
        /// <param name="currentValue">The current value of the property.</param>
        /// <param name="previousValue">The previous value of the property.</param>
        /// <returns><c>true</c> if the property has changed; otherwise, <c>false</c>.</returns>
        private bool CheckObjectPropertyChange<T>(string propertyName, T? currentValue, T? previousValue)
        {
            if (!SfBaseUtils.Equals(currentValue, previousValue))
            {
                _ = SfBaseUtils.UpdateDictionary(propertyName, currentValue!, PropertyChanges);
                return true;
            }
            return false;
        }
    }
}