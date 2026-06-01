using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace Syncfusion.Blazor.Toolkit.Spinner
{

    /// <summary>
    /// Specifies the template options for the <see cref="SfSpinner"/> component.
    /// </summary>
    /// <remarks>
    /// The <see cref="SpinnerTemplates"/> component is used as a child tag within the <see cref="SfSpinner"/> component to define custom content 
    /// to be displayed within the spinner, replacing the default animation. This component must be a direct child of SfSpinner.
    /// </remarks>
    /// <example>
    /// In the following example, a custom template is defined for the <see cref="SfSpinner"/>.
    /// <code><![CDATA[
    /// <SfSpinner @bind-Visible="@SpinnerVisible">
    ///   <SpinnerTemplates>
    ///       <Template>
    ///           <div class="custom-spinner-template">
    ///               <div class="spinner-dot"></div>
    ///               <span>Loading...</span>
    ///           </div>
    ///       </Template>
    ///   </SpinnerTemplates>
    /// </SfSpinner>
    ///
    /// <style>
    /// .custom-spinner-template {
    ///     display: flex;
    ///     align-items: center;
    ///     justify-content: center;
    ///     flex-direction: column;
    /// }
    /// .spinner-dot {
    ///     width: 20px;
    ///     height: 20px;
    ///     border-radius: 50%;
    ///     background-color: #007bff;
    ///     animation: bounce 1.4s infinite ease-in-out both;
    /// }
    /// @keyframes bounce {
    ///     0%, 80%, 100% { transform: scale(0); }
    ///     40% { transform: scale(1.0); }
    /// }
    /// </style>
    ///
    /// @code {
    ///     private bool SpinnerVisible { get; set; } = true;
    /// }
    /// ]]></code>
    /// </example>
    public class SpinnerTemplates : SfBaseComponent
    {
        #region Properties

        /// <summary>
        /// Gets or sets the cascading parameter reference to the parent <see cref="SfSpinner"/> component.
        /// </summary>
        /// <value>
        /// A reference to the parent <see cref="SfSpinner"/> component. The default value is <see langword="null"/>.
        /// </value>
        /// <remarks>
        /// This cascading parameter is automatically provided by the parent SfSpinner component.
        /// It is used to communicate the custom template back to the parent.
        /// </remarks>
        [CascadingParameter]
        private SfSpinner? BaseParent { get; set; }

        /// <summary>
        /// Gets or sets the custom content to be displayed within the spinner.
        /// </summary>
        /// <value>
        /// A <see cref="RenderFragment"/> that defines the template for the spinner. The default value is <see langword="null"/>.
        /// </value>
        /// <remarks>
        /// When this property is set, the default spinner animation is replaced with the custom content provided in the template.
        /// This allows for complete customization of the spinner's visual appearance.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfSpinner @bind-Visible="@SpinnerVisible">
        ///   <SpinnerTemplates>
        ///       <Template>
        ///           <div class="custom-spinner-template">
        ///               <div class="spinner-dot"></div>
        ///               <span>Loading...</span>
        ///           </div>
        ///       </Template>
        ///   </SpinnerTemplates>
        /// </SfSpinner>
        /// ]]></code>
        /// </example>
        [Parameter]
        public RenderFragment? Template { get; set; }

        /// <summary>
        /// Optional logger for error reporting during template initialization.
        /// </summary>
        /// <exclude />
        [Inject]
        private ILogger<SpinnerTemplates>? Logger { get; set; }

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Performs component initialization and updates the parent component with the custom template if provided.
        /// </summary>
        /// <remarks>
        /// This method is invoked when the component is initialized. It checks if a <see cref="Template"/> has been provided 
        /// and updates the parent <see cref="SfSpinner"/> component accordingly.
        /// </remarks>
        protected override void OnInitialized()
        {
            try
            {
                if (Template is not null && BaseParent is not null)
                {
                    BaseParent.UpdateTemplate(Template);
                }
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, "Error during SpinnerTemplates initialization");
                throw;
            }
        }

        #endregion

        #region Disposal

        /// <summary>
        /// Releases all resources associated with this component.
        /// </summary>
        /// <remarks>
        /// This method is called when the component is being disposed. It performs cleanup operations
        /// and prevents memory leaks by clearing references.
        /// </remarks>
        protected override ValueTask DisposeAsyncCore()
        {
            try
            {
                // Clear parent reference to avoid memory leaks
                if (BaseParent is not null)
                {
                    BaseParent = null;
                }
            }
            catch (ObjectDisposedException ex)
            {
                Logger?.LogError(ex, "Error during SpinnerTemplates disposal");
            }

            return base.DisposeAsyncCore();
        }

        #endregion
    }
}
