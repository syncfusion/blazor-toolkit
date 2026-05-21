using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    /// <summary>
    /// Customize the default text of browse, clear, and upload buttons with plain text.
    /// </summary>
    /// <remarks>
    /// The <see cref="UploaderButtons"/> component allows you to customize the text displayed on the browse, clear, and upload buttons of the <see cref="SfUploader"/> component.
    /// This provides flexibility to localize button text or customize the user interface to match application requirements.
    /// </remarks>
    /// <example>
    /// The following example demonstrates how to customize uploader button text.
    /// <code><![CDATA[
    /// <SfUploader ID="UploadFiles">
    ///     <UploaderButtons Browse="Select Files" Clear="Remove All" Upload="Start Upload"></UploaderButtons>
    /// </SfUploader>
    /// ]]></code>
    /// </example>
    public partial class UploaderButtons : SfBaseComponent
    {
        /// <summary>
        /// Property name constant for the Buttons child component.
        /// </summary>
        private const string BUTTONS_PROPERTY_NAME = "Buttons";

        /// <summary>
        /// JavaScript interop method name for property changes.
        /// </summary>
        private const string JS_PROPERTY_CHANGES_METHOD = "sfBlazorToolkit.Uploader.propertyChanges";

        /// <summary>
        /// Default text for the browse button.
        /// </summary>
        private const string DEFAULT_BROWSE_TEXT = "Browse...";

        /// <summary>
        /// Default text for the clear button.
        /// </summary>
        private const string DEFAULT_CLEAR_TEXT = "Clear";

        /// <summary>
        /// Default text for the upload button.
        /// </summary>
        private const string DEFAULT_UPLOAD_TEXT = "Upload";

        [CascadingParameter]
        private SfUploader? Parent { get; set; }

        /// <summary>
        /// Specifies the content that has to be passed.
        /// </summary>
        /// <value>
        /// A <see cref="RenderFragment"/> that represents the child content to be rendered within the component.
        /// </value>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// Specifies the text or HTML content to be displayed on the browse button.
        /// </summary>
        /// <value>
        /// A string representing the text or HTML content for the browse button. The default value is "Browse...".
        /// </value>
        /// <remarks>
        /// The browse button is used to open the file selection dialog. You can customize this text to match your application's localization requirements or user interface theme.
        /// HTML content is supported, allowing you to include icons or formatted text within the button.
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to customize the browse button text.
        /// <code><![CDATA[
        /// <UploaderButtons Browse="Select Files"></UploaderButtons>
        /// ]]></code>
        /// </example>
        [Parameter]
        public string Browse { get; set; } = DEFAULT_BROWSE_TEXT;

        private string? _browse;

        /// <summary>
        /// Specifies the text or HTML content to be displayed on the clear button.
        /// </summary>
        /// <value>
        /// A string representing the text or HTML content for the clear button. The default value is "Clear".
        /// </value>
        /// <remarks>
        /// The clear button is used to remove all selected files from the uploader before upload. This button appears when files are selected and provides users with the ability to reset their file selection.
        /// HTML content is supported, allowing you to include icons or formatted text within the button.
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to customize the clear button text.
        /// <code><![CDATA[
        /// <UploaderButtons Clear="Remove All"></UploaderButtons>
        /// ]]></code>
        /// </example>
        [Parameter]
        public string Clear { get; set; } = DEFAULT_CLEAR_TEXT;

        private string? _clear;

        /// <summary>
        /// Specifies the text or HTML content to be displayed on the upload button.
        /// </summary>
        /// <value>
        /// A string representing the text or HTML content for the upload button. The default value is "Upload".
        /// </value>
        /// <remarks>
        /// The upload button is used to initiate the file upload process for all selected files. This button becomes active when files are selected and the uploader is configured for manual upload mode.
        /// HTML content is supported, allowing you to include icons or formatted text within the button.
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to customize the upload button text.
        /// <code><![CDATA[
        /// <UploaderButtons Upload="Start Upload"></UploaderButtons>
        /// ]]></code>
        /// </example>
        [Parameter]
        public string Upload { get; set; } = DEFAULT_UPLOAD_TEXT;

        private string? _upload;

        /// <summary>
        /// Triggers during the initial rendering of the component.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        /// <remarks>
        /// This method is called when the component is first initialized. It updates the parent uploader with the current button properties and triggers a state change to ensure the UI reflects the initial button configuration.
        /// </remarks>
        /// <exclude/>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(true);
            InitializeButtonProperties();
        }

        /// <summary>
        /// Initializes button properties and notifies the parent component.
        /// </summary>
        private void InitializeButtonProperties()
        {
            Parent?.UpdateChildProperties(BUTTONS_PROPERTY_NAME, this);
            _browse = Browse;
            _clear = Clear;
            _upload = Upload;
        }

        /// <summary>
        /// Triggers when component parameters are dynamically updated.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        /// <remarks>
        /// This method is called whenever component parameters change after initial rendering. It handles property change notifications and updates the parent uploader component with the new button configurations.
        /// The method also invokes JavaScript interop to apply property changes on the client side when the component is rendered.
        /// </remarks>
        /// <exclude/>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(true);
            UpdateButtonProperties();
            if (ShouldNotifyParent())
            {
                await NotifyParentOfChangesAsync().ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Updates internal button property values and tracks changes.
        /// </summary>
        private void UpdateButtonProperties()
        {
            _browse = NotifyPropertyChanges(nameof(Browse), Browse, _browse);
            _clear = NotifyPropertyChanges(nameof(Clear), Clear, _clear);
            _upload = NotifyPropertyChanges(nameof(Upload), Upload, _upload);
        }

        /// <summary>
        /// Determines whether the parent component should be notified of property changes.
        /// </summary>
        /// <returns>True if changes should be propagated to the parent; otherwise, false.</returns>
        private bool ShouldNotifyParent()
        {
            return PropertyChanges?.Count > 0 && IsRendered && Parent != null;
        }

        /// <summary>
        /// Notifies the parent uploader component of button property changes via JavaScript interop.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task NotifyParentOfChangesAsync()
        {
            // Null-forgiving operator used here as parent is already verified in ShouldNotifyParent()
            Parent!.UpdateChildProperties(BUTTONS_PROPERTY_NAME, this);
            Dictionary<string, object> options = Parent.GetProperty();
            Dictionary<string, object> buttonProps = CreateButtonPropertyDictionary();
            await InvokeVoidAsync(Parent._uploaderJsModule, Parent._uploaderJsInProcessModule, JS_PROPERTY_CHANGES_METHOD, [Parent.ID, options, buttonProps]).ConfigureAwait(true);
        }

        /// <summary>
        /// Creates a dictionary containing the buttons property for JavaScript interop.
        /// </summary>
        /// <returns>A dictionary with the buttons property.</returns>
        private Dictionary<string, object> CreateButtonPropertyDictionary()
        {
            return new Dictionary<string, object> { { BUTTONS_PROPERTY_NAME, this } };
        }

        /// <summary>
        /// Performs cleanup operations when the component is disposed.
        /// </summary>
        /// <remarks>
        /// This method is called when the component is being disposed. It clears the parent reference to prevent memory leaks and ensure proper garbage collection.
        /// </remarks>
        /// <exclude/>
        protected override ValueTask DisposeAsyncCore()
        {
            Parent = null;
            return base.DisposeAsyncCore();
        }
    }
}