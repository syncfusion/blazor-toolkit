using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Popups
{
    public partial class SfDialog : SfBaseComponent
    {
        #region Private variables
        private bool _allowDragging;
        private bool _closeOnEscape;
        private string? _cssClass;
        private bool _enableResize;
        private double _zIndex;
        private string? _width;
        private bool _visible;
        private string? _target;
        private string? _minHeight;
        private bool _isModal;
        private string? _height;
        #endregion

        /// <summary>
        /// Gets or sets the `id` attribute for the root element of the <see cref="SfDialog"/> component.
        /// </summary>
        /// <value>
        /// A string that serves as a unique identifier for the dialog. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// Providing a unique ID can be useful for targeting the dialog element with CSS or JavaScript.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Toolkit.Popups
        /// <SfDialog @bind-Visible="Visibility" ID="dialog_default">
        /// </SfDialog>
        /// @code {
        ///     private bool Visibility { get; set; } = true;
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public string ID { get; set; } = default!;

        /// <summary>
        /// Gets or sets a value indicating whether to persist the component's state between page reloads. When set to <c>true</c>, the dialog's position, <see cref="Width"/>, and <see cref="Height"/> are persisted.
        /// </summary>
        /// <value>
        /// <c>true</c> if the component's state persistence is enabled; otherwise, <c>false</c>. The default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// The component's position, <see cref="Width"/>, and <see cref="Height"/> properties will be stored in the browser's local storage to persist its state when the page reloads.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Toolkit.Popups
        /// <SfDialog @bind-Visible="Visibility" EnablePersistence="true" ID="dialog_persist">
        /// </SfDialog>
        /// @code {
        ///     private bool Visibility { get; set; } = true;
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool EnablePersistence { get; set; }

        /// <summary>
        /// Gets or sets the child content of the <see cref="SfDialog"/> component.
        /// </summary>
        /// <value>
        /// A <see cref="RenderFragment"/> representing the content to be displayed within the dialog.
        /// </value>
        /// <exclude />
        [Parameter]
        public RenderFragment ChildContent { get; set; } = default!;

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="SfDialog"/> can be dragged by the user.
        /// </summary>
        /// <value>
        /// <c>true</c> if dragging is enabled; otherwise, <c>false</c>. The default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// When enabled, you can reposition the dialog by clicking and dragging its header. The dialog can only be dragged within its container element.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Toolkit.Popups
        /// <SfDialog Width="500px" @bind-Visible="Visibility" AllowDragging="true">
        ///   <DialogTemplates>
        ///     <Header>
        ///         Dialog Header
        ///       </Header>
        ///     <Content>
        ///         <p>
        ///            Dialog content
        ///           </p>
        ///       </Content>
        ///   </DialogTemplates>
        /// </SfDialog>
        ///  @code {
        ///   private bool Visibility { get; set; } = true;
        ///  }
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool AllowDragging { get; set; }

        internal DialogAnimationSettings AnimationSettingsValue { get; set; } = default!;

        internal List<DialogButton>? ButtonsValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="SfDialog"/> can be closed by pressing the Escape key.
        /// </summary>
        /// <value>
        /// <c>true</c> to allow closing the dialog with the Escape key; otherwise, <c>false</c>. The default value is <c>true</c>.
        /// </value>
        /// <remarks>
        /// If this property is set to <c>true</c>, pressing the Escape key will close the dialog.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Toolkit.Popups
        /// <SfDialog Width="500px" @bind-Visible="Visibility" CloseOnEscape="false">
        /// </SfDialog>
        ///  @code {
        ///   private bool Visibility { get; set; } = true;
        ///  }
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool CloseOnEscape { get; set; } = true;

        /// <summary>
        /// Gets or sets the content to be displayed in the body of the <see cref="SfDialog"/> component.
        /// </summary>
        /// <value>
        /// A string that represents the content of the dialog. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property is an alternative to using <see cref="ChildContent"/> or <c>DialogTemplates</c> for defining the dialog's content. If both are set, templates take precedence.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Toolkit.Popups
        /// <SfDialog Width="500px" @bind-Visible="Visibility" Content="@DialogContent">
        /// </SfDialog>
        ///  @code {
        ///   private bool Visibility { get; set; } = true;
        ///   private string DialogContent { get; set; } = "<p> Dialog content </p>";
        ///  }
        /// ]]></code>
        /// </example>
        [Parameter]
        public string Content { get; set; } = default!;

        /// <summary> 
        /// Gets or sets one or more custom CSS classes to be applied to the root element of the <see cref="SfDialog"/> component.
        /// </summary> 
        /// <value> 
        /// A string representing the CSS classes. Multiple classes can be separated by a space. The default value is <c>String.Empty</c>.
        /// </value>
        /// <remarks>
        /// This property allows for custom styling of the dialog component.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Toolkit.Popups
        /// <SfDialog Width="500px" @bind-Visible="Visibility" CssClass="custom-class">
        ///   <DialogTemplates>
        ///     <Content>
        ///         <p>
        ///            Dialog content
        ///           </p>
        ///       </Content>
        ///   </DialogTemplates>
        /// </SfDialog>
        /// <style>
        /// .custom-class .e-dlg-content{
        ///    background-color: #e0f6ff;
        ///  }
        /// </style>
        ///  @code {
        ///   private bool Visibility { get; set; } = true;
        ///  }
        /// ]]></code>
        /// </example>
        [Parameter]
        public string CssClass { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="SfDialog"/> can be resized by the user.
        /// </summary>
        /// <value>
        /// <c>true</c> if the dialog can be resized; otherwise, <c>false</c>. The default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// When enabled, a resize grip is created that allows the user to resize the dialog by dragging.
        /// During resizing, the dialog's dimensions cannot be smaller than the value specified by the <see cref="MinHeight"/> property.
        /// The directions in which the dialog can be resized are specified by the <see cref="ResizeHandles"/> property.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Toolkit.Popups
        /// <SfDialog Width="500px" @bind-Visible="Visibility" EnableResize="true">
        ///   <DialogTemplates>
        ///     <Content>
        ///         <p>
        ///            Dialog content
        ///           </p>
        ///       </Content>
        ///   </DialogTemplates>
        /// </SfDialog>
        ///  @code {
        ///   private bool Visibility { get; set; } = true;
        ///  }
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool EnableResize { get; set; }

        /// <summary>
        /// Gets or sets the directions in which the <see cref="SfDialog"/> can be resized.
        /// </summary>
        /// <value>
        /// An array of <see cref="ResizeDirection"/> enum values that specify the available resize handles. The default value is an array containing only <see cref="ResizeDirection.SouthEast"/>.
        /// </value>
        /// <remarks>
        /// This property only has an effect if <see cref="EnableResize"/> is set to <c>true</c>.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Toolkit.Popups
        /// <SfDialog Width="500px" @bind-Visible="Visibility" ResizeHandles="ResizeHandles" EnableResize="true">
        ///   <DialogTemplates>
        ///     <Content>
        ///         <p>
        ///            Dialog content
        ///           </p>
        ///       </Content>
        ///   </DialogTemplates>
        /// </SfDialog>
        ///  @code {
        ///   private bool Visibility { get; set; } = true;
        ///   private ResizeDirection[] ResizeHandles { get; set; }
        ///   protected override void OnInitialized()
        ///   {
        ///     ResizeHandles = new ResizeDirection[] { ResizeDirection.NorthEast };
        ///   }
        ///  }
        /// ]]></code>
        /// </example>
        [Parameter]
        public ResizeDirection[] ResizeHandles { get; set; } = [ResizeDirection.SouthEast];

        /// <summary>
        /// Gets or sets the template for rendering the footer of the <see cref="SfDialog"/> component.
        /// </summary>
        /// <value> 
        /// A string that defines the HTML content for the dialog's footer. The default value is <c>null</c>. 
        /// </value>
        /// <remarks>
        /// This property is optional and is typically used to add custom buttons or other content to the footer.
        /// If a <see cref="FooterTemplate"/> is specified, the default action buttons defined via the <see cref="Buttons"/> property will be ignored.
        /// For more complex content, consider using the <c>DialogTemplates.Footer</c> render fragment.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Toolkit.Popups
        /// <SfDialog Width="500px" @bind-Visible="Visibility" FooterTemplate="@FoooterTemplate">
        ///   <DialogTemplates>
        ///     <Content>
        ///         <p>
        ///            Dialog content
        ///           </p>
        ///       </Content>
        ///   </DialogTemplates>
        /// </SfDialog>
        ///  @code {
        ///   private bool Visibility { get; set; } = true;
        ///   private string FoooterTemplate { get; set; } = "<p>Footer content</p>";
        ///  }
        /// ]]></code>
        /// </example>
        [Parameter]
        public string FooterTemplate { get; set; } = default!;

        /// <summary>
        /// Gets or sets the content for the header/title of the <see cref="SfDialog"/> component.
        /// </summary>
        /// <value> 
        /// A string that specifies the title of the dialog. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property defines the text or HTML content that will be displayed in the dialog's header area.
        /// If not set, the header will not be displayed. For more complex header content, consider using the <c>DialogTemplates.Header</c> render fragment.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Toolkit.Popups
        /// <SfDialog Width="500px" @bind-Visible="Visibility" Header="@HeaderTemplate">
        ///   <DialogTemplates>
        ///     <Content>
        ///         <p>
        ///            Dialog content
        ///           </p>
        ///       </Content>
        ///   </DialogTemplates>
        /// </SfDialog>
        ///  @code {
        ///   private bool Visibility { get; set; } = true;
        ///   private string HeaderTemplate { get; set; } = "<p>Header content</p>";
        ///  }
        /// ]]></code>
        /// </example>
        [Parameter]
        public string Header { get; set; } = default!;


        /// <summary> 
        /// Gets or sets the height of the <see cref="SfDialog"/> component.
        /// </summary> 
        /// <value> 
        /// A string representing the height in pixels, percentage, or other CSS units. A numeric value is treated as pixels. The default value is <c>"auto"</c>.
        /// </value>
        /// <remarks>
        /// This property controls the vertical size of the dialog. When set to <c>"auto"</c>, the height is automatically determined by the dialog's content.
        /// You can specify values in various CSS units such as pixels (px), percentage (%), or other supported units.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Toolkit.Popups
        /// <SfDialog Width="500px" @bind-Visible="Visibility"  Height="150px">
        ///   <DialogTemplates>
        ///     <Content>
        ///         <p>
        ///            Dialog content
        ///           </p>
        ///       </Content>
        ///   </DialogTemplates>
        /// </SfDialog>
        ///  @code {
        ///   private bool Visibility { get; set; } = true;
        ///  }
        /// ]]></code>
        /// </example>
        /// <seealso cref="MinHeight"/>
        [Parameter]
        public string Height { get; set; } = "auto";

        /// <summary>
        /// Gets or sets a collection of additional HTML attributes to be applied to the root element of the <see cref="SfDialog"/>.
        /// </summary>
        /// <value>
        /// A <see cref="Dictionary{TKey, TValue}"/> where the key is the attribute name and the value is the attribute value. The default is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property allows you to add custom attributes like `id`, `title`, or `aria-` attributes to the dialog element.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Toolkit.Popups
        /// <SfDialog @bind-Visible="Visibility" title="Dialog">
        /// </SfDialog>
        /// @code {
        ///     private bool Visibility { get; set; } = true;
        /// }
        /// ]]></code>
        /// </example>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> HtmlAttributes
        {
            get => HtmlAttributesValue;
            set => HtmlAttributesValue = value;
        }

        internal Dictionary<string, object> HtmlAttributesValue { get; set; } = default!;

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="SfDialog"/> is displayed as a modal dialog.
        /// </summary>
        /// <value> 
        /// <c>true</c> to display as a modal dialog; otherwise, <c>false</c>. The default value is <c>false</c>. 
        /// </value> 
        /// <remarks>
        /// A modal dialog creates an overlay that blocks interaction with the rest of the application until the dialog is closed.
        /// A modeless dialog (when <see cref="IsModal"/> is <c>false</c>) allows the user to continue interacting with the parent application while the dialog remains open.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Toolkit.Popups
        /// <SfDialog @bind-Visible="Visibility" IsModal="true">
        /// </SfDialog>
        /// @code {
        ///     private bool Visibility { get; set; } = true;
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool IsModal { get; set; }

        /// <summary> 
        /// Gets or sets the minimum height of the <see cref="SfDialog"/> component.
        /// </summary> 
        /// <value> 
        /// A string representing the minimum height in pixels, percentage, or other CSS units. A numeric value is treated as pixels. The default value is <c>String.Empty</c>.
        /// </value>
        /// <remarks>
        /// This property ensures that the dialog's height, whether set directly or through user resizing, does not fall below the specified minimum value.
        /// When the user attempts to resize the dialog below this threshold, the dialog size will be constrained to the <see cref="MinHeight"/> setting.
        /// This is particularly useful when <see cref="EnableResize"/> is set to <c>true</c>.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Toolkit.Popups
        /// <SfDialog Width="500px" @bind-Visible="Visibility" MinHeight="150px">
        ///   <DialogTemplates>
        ///     <Content>
        ///         <p>
        ///            Dialog content
        ///           </p>
        ///       </Content>
        ///   </DialogTemplates>
        /// </SfDialog>
        ///  @code {
        ///   private bool Visibility { get; set; } = true;
        ///  }
        /// ]]></code>
        /// </example>
        /// <seealso cref="Height"/>
        [Parameter]
        public string MinHeight { get; set; } = string.Empty;

        internal DialogPositionData? PositionValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to display a close icon in the header of the <see cref="SfDialog"/>.
        /// </summary>
        /// <value>
        /// <c>true</c> to display the close icon; otherwise, <c>false</c>. The default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// When this property is set to <c>true</c>, a close button (typically an 'X' icon) is displayed in the dialog's header, allowing the user to close it.
        /// A header must be visible for the close icon to be displayed.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Toolkit.Popups
        /// <SfDialog @bind-Visible="Visibility" ShowCloseIcon="true">
        ///   <DialogTemplates>
        ///     <Header>
        ///         Dialog Header
        ///       </Header>
        ///     <Content>
        ///         <p>
        ///            Dialog content
        ///           </p>
        ///       </Content>
        ///   </DialogTemplates>
        /// </SfDialog>
        /// @code {
        ///     private bool Visibility { get; set; } = true;
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool ShowCloseIcon { get; set; }

        /// <summary>
        /// Gets or sets the target element in which the <see cref="SfDialog"/> should be displayed.
        /// </summary>
        /// <value>
        /// The default value is <c>null</c>, which refers to the <c>Document.body</c> element.
        /// </value>
        /// <remarks>
        /// You can use this property to specify the CSS selector.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Toolkit.Popups
        /// <div id="target" style="width:100%; height:300px;"></div>
        /// <SfDialog Width="500px" @bind-Visible="Visibility" Target="#target">
        ///   <DialogTemplates>
        ///     <Content>
        ///         <p>
        ///            Dialog content
        ///           </p>
        ///       </Content>
        ///   </DialogTemplates>
        /// </SfDialog>
        ///  @code {
        ///   private bool Visibility { get; set; } = true;
        ///  }
        /// ]]></code>
        /// </example>
        [Parameter]
        public string Target { get; set; } = default!;

        /// <summary>
        /// Gets or sets a value that represents whether the <see cref="SfDialog"/> component is visible.
        /// </summary>
        /// <value>
        /// <c>true</c> if the dialog is visible; otherwise, <c>false</c>. The default value is <c>true</c>.
        /// </value>
        /// <remarks>
        /// This property can be used to programmatically show or hide the dialog.
        /// When set to <c>true</c>, the dialog will be displayed; when set to <c>false</c>, the dialog will be hidden.
        /// This property supports two-way data binding using the <c>@bind-Visible</c> syntax.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Toolkit.Popups
        /// <SfDialog @bind-Visible="Visibility">
        /// </SfDialog>
        /// @code {
        ///     private bool Visibility { get; set; } = true;
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool Visible { get; set; } = true;

        /// <summary>
        /// Gets or sets an event callback that is raised when the <see cref="Visible"/> property changes.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{T}"/> of type <see cref="bool"/> that receives the new visibility state.
        /// </value>
        /// <remarks>
        /// This event callback is automatically invoked whenever the <see cref="Visible"/> property value changes.
        /// It is typically used for two-way data binding with the <c>@bind-Visible</c> syntax, allowing the parent component to be notified when the dialog's visibility state changes.
        /// </remarks>
        [Parameter]
        public EventCallback<bool> VisibleChanged { get; set; }

        /// <summary> 
        /// Gets or sets the width of the <see cref="SfDialog"/> component.
        /// </summary> 
        /// <value> 
        /// A string that specifies the width in various units (e.g., "500px", "50%"). Numeric values are treated as pixels. The default value is <c>"100%"</c>.
        /// </value>
        /// <remarks>
        /// This property determines the horizontal size of the dialog.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Toolkit.Popups
        /// <SfDialog Width="500px" @bind-Visible="Visibility"  Width="150px">
        ///   <DialogTemplates>
        ///     <Content>
        ///         <p>
        ///            Dialog content
        ///           </p>
        ///       </Content>
        ///   </DialogTemplates>
        /// </SfDialog>
        ///  @code {
        ///   private bool Visibility { get; set; } = true;
        ///  }
        /// ]]></code>
        /// </example>
        [Parameter]
        public string Width { get; set; } = "100%";

        /// <summary>
        /// Gets or sets the z-index of the <see cref="SfDialog"/>, which determines its stacking order relative to other elements.
        /// </summary>
        /// <value>
        /// A numeric value representing the z-index. The default value is <c>1000</c>.
        /// </value>
        /// <remarks>
        /// A higher z-index value will cause the dialog to be displayed in front of elements with a lower z-index.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Toolkit.Popups
        /// <SfDialog @bind-Visible="Visibility" ZIndex="1500">
        /// </SfDialog>
        /// @code {
        ///     private bool Visibility { get; set; } = true;
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public double ZIndex { get; set; } = 1000;

        /// <summary>
        /// Gets or sets a value indicating whether to keep the <see cref="SfDialog"/> component's DOM structure in the page when it is closed.
        /// </summary>
        /// <value>
        /// <c>true</c> to maintain the dialog's DOM elements when it's closed; otherwise, <c>false</c>. The default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// When set to <c>false</c>, the dialog's DOM elements are completely removed from the page when it is closed and recreated when it is opened again.
        /// When set to <c>true</c>, the dialog is hidden using CSS but its DOM structure remains in the page, which can improve performance when the dialog is frequently opened and closed.
        /// This property is particularly useful for optimizing rendering performance in scenarios where the dialog contains complex content or is opened and closed repeatedly.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Toolkit.Popups
        /// <SfDialog @bind-Visible="Visibility" AllowPrerender="true">
        /// </SfDialog>
        /// @code {
        ///     private bool Visibility { get; set; } = true;
        /// }
        /// ]]></code>
        /// </example>
        /// <seealso cref="Visible"/>
        [Parameter]
        public bool AllowPrerender { get; set; }
    }
}