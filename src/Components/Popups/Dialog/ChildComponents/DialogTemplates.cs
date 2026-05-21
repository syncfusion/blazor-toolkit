using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Popups
{
    /// <summary>
    /// A component class used within the <see cref="SfDialog"/> to configure custom templates for the header, content, and footer sections of the dialog.
    /// </summary>
    /// <remarks>
    /// The <see cref="DialogTemplates"/> class provides template properties to customize the appearance and content of different areas within a dialog component. 
    /// This allows developers to define custom UI elements and layouts for the dialog's header, content, and footer sections instead of using the default dialog structure.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// @using Syncfusion.Blazor.Toolkit.Popups
    /// <SfDialog Width="500px" @bind-Visible="Visibility">
    ///   <DialogTemplates>
    ///     <Header><h1>Dialog Header</h1></Header>
    ///     <Content>
    ///       <p>Dialog content</p>
    ///     </Content>
    ///     <FooterTemplate>
    ///       <button class="e-btn" style="background-color:#8A2BE2;" onclick="@OnBtnClick">OK</button>
    ///     </FooterTemplate>
    ///   </DialogTemplates>
    /// </SfDialog>
    /// @code {
    ///   private bool Visibility { get; set; } = true;
    ///   private void OnBtnClick()
    ///   {
    ///     this.Visibility = false;
    ///   }
    /// }
    /// ]]></code>
    /// </example>
    public class DialogTemplates : SfBaseComponent

    {
        [CascadingParameter]
        internal SfDialog? Parent { get; set; }

        /// <summary>
        /// Gets or sets the template as <see cref="RenderFragment"/> that defines the custom appearance of the dialog's header area.
        /// </summary>
        /// <value>
        /// A <see cref="RenderFragment"/> that specifies the custom template for the dialog header area. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// The <see cref="Header"/> property is used to customize the appearance of the dialog's title area. 
        /// When this property is set, it overrides the default header content and allows you to define custom HTML elements, Blazor components, or any other content for the header section.
        /// Specify the <see cref="Header"/> template within the <see cref="DialogTemplates"/> tag directive.
        /// </remarks>        
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Toolkit.Popups
        /// <SfDialog Width="500px" @bind-Visible="Visibility">
        ///   <DialogTemplates>
        ///     <Header><h1>Dialog Header</h1></Header>
        ///     <Content>
        ///       <p>Dialog content</p>
        ///     </Content>
        ///   </DialogTemplates>
        /// </SfDialog>
        /// @code {
        ///   private bool Visibility { get; set; } = true;
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public RenderFragment? Header { get; set; }

        /// <summary>
        /// Gets or sets the template as <see cref="RenderFragment"/> that defines the custom appearance of the dialog's content area.
        /// </summary>
        /// <value>
        /// A <see cref="RenderFragment"/> that specifies the custom template for the dialog content area. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// The <see cref="Content"/> property is used to customize the main body or content area of the dialog. 
        /// This template allows you to define any custom HTML elements, Blazor components, forms, or other content that should be displayed in the dialog's body section.
        /// Specify the <see cref="Content"/> template within the <see cref="DialogTemplates"/> tag directive.
        /// </remarks>        
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Toolkit.Popups
        /// <SfDialog Width="500px" @bind-Visible="Visibility">
        ///   <DialogTemplates>
        ///     <Content>
        ///       <p>Dialog content</p>
        ///     </Content>
        ///   </DialogTemplates>
        /// </SfDialog>
        /// @code {
        ///   private bool Visibility { get; set; } = true;
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public RenderFragment? Content { get; set; }

        /// <summary>
        /// Gets or sets the template as <see cref="RenderFragment"/> that defines the custom appearance of the dialog's footer area.
        /// </summary>
        /// <value>
        /// A <see cref="RenderFragment"/> that specifies the custom template for the dialog footer area. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// The <see cref="FooterTemplate"/> property is used to customize the footer area of the dialog, typically used for action buttons like OK, Cancel, Save, etc.
        /// When this property is set, it overrides the default footer buttons and allows you to define custom buttons, controls, or other content for the footer section.
        /// If not specified, the action buttons are enabled by default in the footer. 
        /// Specify the <see cref="FooterTemplate"/> within the <see cref="DialogTemplates"/> tag directive.
        /// </remarks>        
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Toolkit.Popups
        /// <SfDialog Width="500px" @bind-Visible="Visibility">
        ///   <DialogTemplates>
        ///     <Content>
        ///       <p>Dialog content</p>
        ///     </Content>
        ///     <FooterTemplate>
        ///       <button class="e-btn" style="background-color:#8A2BE2;" onclick="@OnBtnClick">OK</button>
        ///     </FooterTemplate>
        ///   </DialogTemplates>
        /// </SfDialog>
        /// @code {
        ///   private bool Visibility { get; set; } = true;
        ///   private void OnBtnClick()
        ///   {
        ///     this.Visibility = false;
        ///   }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public RenderFragment? FooterTemplate { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start, used to register templates with the parent dialog.
        /// </summary>
        /// <remarks>
        /// This method is automatically called by the Blazor framework during component initialization. 
        /// It registers the Header, Content, and FooterTemplate with the parent <see cref="SfDialog"/> component if they are defined,
        /// and triggers a refresh of the parent dialog to apply the template changes.
        /// </remarks>
        /// <exclude />
        protected override void OnInitialized()
        {
            if (Header is not null)
            {
                Parent?.UpdateTemplate(nameof(Header), Header);
            }

            if (Content is not null)
            {
                Parent?.UpdateTemplate(nameof(Content), Content);
            }

            if (FooterTemplate is not null)
            {
                Parent?.UpdateTemplate(nameof(FooterTemplate), FooterTemplate);
            }
            Parent?.Refresh();
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="DialogTemplates"/> and optionally releases the managed resources.
        /// </summary>
        /// <remarks>
        /// This protected method follows the standard .NET dispose pattern.
        /// it cleans up managed resources including setting the Parent reference to <c>null</c> to avoid memory leaks.
        /// The method calls the base class implementation to ensure proper cleanup of inherited resources.
        /// </remarks>
        /// <exclude />
        protected override ValueTask DisposeAsyncCore()
        {
            Parent = null;
            return base.DisposeAsyncCore();
        }
    }
}