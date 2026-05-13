using System.ComponentModel;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Syncfusion.Blazor.Toolkit.Popups
{
    /// <summary>
    /// A class used for configuring the button properties in the <see cref="SfDialog"/> component.
    /// </summary>
    /// <remarks>
    /// The <see cref="DialogButton"/> class provides comprehensive configuration options for buttons within the <see cref="SfDialog"/> component,
    /// including appearance customization, event handling, and button behavior settings. This class supports various button types such as primary buttons,
    /// toggle buttons, and flat buttons with configurable icons and content.
    /// </remarks>
    /// <example>
    /// In the following code example, a basic DialogButton has been rendered using the tag directive.
    /// <code><![CDATA[
    /// @using Syncfusion.Blazor.Toolkit.Popups
    /// <SfDialog Width="500px" @bind-Visible="Visibility">
    ///   <DialogTemplates>
    ///     <Content>
    ///         <p>
    ///            Dialog content
    ///         </p>
    ///     </Content>
    ///   </DialogTemplates>
    ///   <DialogButtons>
    ///       <DialogButton IsPrimary="true" Content="Ok" OnClick="@OnBtnClick" />
    ///       <DialogButton Content="Cancel" OnClick="@OnBtnClick" />
    ///   </DialogButtons>
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
    public partial class DialogButton : SfBaseComponent
    {
        private string? _content;
        private string? _cssClass;
        private int _tagIndex = -1;
        private bool _disabled;
        private ButtonType _type;
        private bool _isToggle;
        private bool _isPrimary;
        private IconPosition _iconPosition;
        private string? _iconCss;

        [CascadingParameter]
        internal DialogButtons? Parent { get; set; }

        /// <summary> 
        /// Gets or sets the child content for the button including HTML elements. If the child content is not specified, the button is rendered using the <see cref="Content"/> property.
        /// </summary> 
        /// <value> 
        /// A <see cref="RenderFragment"/> representing the template content. The default value is <c>null</c>. 
        /// </value> 
        /// <remarks> 
        /// The child content specified within the tag directive can be either a string or HTML element. 
        /// The string content can also be specified using the <see cref="Content"/> property. When both child content and the <see cref="Content"/> property are provided,
        /// the child content takes precedence over the <see cref="Content"/> property.
        /// </remarks>
        [Parameter]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public RenderFragment? ChildContent { get; set; }

        /// <summary> 
        /// Gets or sets the text content displayed on the button element. 
        /// </summary> 
        /// <value> 
        /// A <c>string</c> value representing the button's text content. The default value is <c>null</c>. 
        /// </value>
        /// <remarks>
        /// This property defines the textual content that appears on the button. If both <see cref="Content"/> and <see cref="ChildContent"/> are specified,
        /// the <see cref="ChildContent"/> takes precedence over this property.
        /// </remarks>
        [Parameter]
        public string Content { get; set; } = default!;

        /// <summary> 
        /// Gets or sets the CSS class string to customize the appearance of the button. 
        /// </summary> 
        /// <value> 
        /// A <c>string</c> containing CSS class names separated by spaces to customize the appearance of the button. The default value is <c>String.Empty</c>. 
        /// </value>
        /// <remarks>
        /// This property allows you to apply custom CSS classes to modify the button's visual appearance, including colors, fonts, borders, and other styling attributes.
        /// Multiple CSS classes can be specified by separating them with spaces.
        /// </remarks>
        [Parameter]
        public string CssClass { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets a value indicating whether the button is disabled. 
        /// </summary> 
        /// <value> 
        /// <c>true</c> if the button component is disabled; otherwise, <c>false</c>. The default value is <c>false</c>. 
        /// </value>
        /// <remarks>
        /// When this property is set to <c>true</c>, the button becomes non-interactive and appears in a disabled state.
        /// Users cannot click or interact with the button when it is disabled.
        /// </remarks>
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary> 
        /// Gets or sets the CSS class string to include an icon or image for the button.  
        /// </summary> 
        /// <value> 
        /// A <c>string</c> containing CSS class names separated by spaces to include an icon or image for the button. The default value is <c>String.Empty</c>. 
        /// </value>
        /// <remarks>
        /// This property allows you to specify CSS classes that define an icon or image to be displayed on the button.
        /// The icon's position relative to the button text is controlled by the <see cref="IconPosition"/> property.
        /// </remarks>
        [Parameter]
        public string IconCss { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets the position of the icon relative to the button content. 
        /// </summary> 
        /// <value> 
        /// One of the <see cref="IconPosition"/> enumeration values. The default value is <see cref="IconPosition.Left"/>. 
        /// </value> 
        /// <remarks> 
        /// This property determines where the icon appears in relation to the button text:
        /// <list type="bullet">
        /// <item><description><see cref="IconPosition.Left"/> - Icon appears to the left of the button text</description></item>
        /// <item><description><see cref="IconPosition.Right"/> - Icon appears to the right of the button text</description></item>
        /// <item><description><see cref="IconPosition.Top"/> - Icon appears above the button text</description></item>
        /// <item><description><see cref="IconPosition.Bottom"/> - Icon appears below the button text</description></item>
        /// </list>
        /// This property only takes effect when the <see cref="IconCss"/> property is specified.
        /// </remarks>
        [Parameter]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public IconPosition IconPosition { get; set; }

        /// <summary> 
        /// Gets or sets a value indicating whether the button uses the primary style appearance. 
        /// </summary> 
        /// <value> 
        /// <c>true</c> if the primary style is enabled for the button; otherwise, <c>false</c>. The default value is <c>false</c>. 
        /// </value>
        /// <remarks>
        /// When this property is set to <c>true</c>, the button is rendered with a primary style that typically makes it more prominent,
        /// such as using a different background color or emphasis styling. This is commonly used for the main action button in a dialog.
        /// </remarks>
        [Parameter]
        public bool IsPrimary { get; set; }

        /// <summary> 
        /// Gets or sets a value indicating whether the button behaves as a toggle button. 
        /// </summary> 
        /// <value> 
        /// <c>true</c> if the toggle option is enabled for the button; otherwise, <c>false</c>. The default value is <c>false</c>. 
        /// </value>
        /// <remarks>
        /// When this property is set to <c>true</c>, the button maintains a pressed or active state after being clicked,
        /// and clicking it again will deactivate the pressed state. This is useful for buttons that represent on/off states or selections.
        /// </remarks>
        [Parameter]
        public bool IsToggle { get; set; }


        /// <summary> 
        /// Gets or sets an event callback that is triggered when the button is clicked.
        /// </summary> 
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> that is invoked when the button is clicked. 
        /// The callback receives <see cref="MouseEventArgs"/> containing information about the mouse event.
        /// </value>
        /// <remarks> 
        /// The event is triggered for UI-based clicks only, including mouse clicks and keyboard activation (such as pressing Enter or Space).
        /// This event allows you to handle button click actions and perform custom logic when the user interacts with the button.
        /// </remarks> 
        /// <example> 
        /// <code><![CDATA[ 
        /// @using Syncfusion.Blazor.Toolkit.Popups
        /// <SfDialog Width="500px" @bind-Visible="Visibility">
        ///   <DialogTemplates>
        ///     <Content>
        ///       <p>
        ///         Dialog content
        ///       </p>
        ///     </Content>
        ///   </DialogTemplates>
        ///   <DialogButtons>
        ///     <DialogButton IsPrimary="true" Content="Ok" OnClick="@OnBtnClick" />
        ///   </DialogButtons>
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
        public EventCallback<MouseEventArgs> OnClick { get; set; }

        /// <summary>
        /// Gets or sets the type of button that determines its behavior within forms.
        /// </summary>
        /// <value> 
        /// One of the <see cref="ButtonType"/> enumeration values. The default value is <see cref="ButtonType.Button"/>. 
        /// </value>
        /// <remarks>
        /// This property specifies the button's behavior within HTML forms. The possible values are:
        /// <list type="bullet">
        /// <item>
        /// <description><see cref="ButtonType.Button"/> - Performs a standard button click action</description>
        /// </item>
        /// <item>
        /// <description><see cref="ButtonType.Submit"/> - Sends form data to a server for processing</description>
        /// </item>
        /// <item>
        /// <description><see cref="ButtonType.Reset"/> - Resets the filled values of a form to its initial values</description>
        /// </item>
        /// </list>
        /// </remarks>
        /// <example> 
        /// <code><![CDATA[ 
        /// @using Syncfusion.Blazor.Toolkit.Popups
        /// <SfDialog Width="500px" @bind-Visible="Visibility">
        ///   <DialogTemplates>
        ///     <Content>
        ///       <p>
        ///         Dialog content
        ///       </p>
        ///     </Content>
        ///   </DialogTemplates>
        ///   <DialogButtons>
        ///     <DialogButton Type="ButtonType.Submit" Content="Submit" OnClick="@OnBtnClick" />
        ///   </DialogButtons>
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
        public ButtonType Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the dialog button uses a flat appearance style.
        /// </summary>
        /// <value> 
        /// <c>true</c> if the flat appearance is enabled for the dialog button; otherwise, <c>false</c>. The default value is <c>true</c>. 
        /// </value>
        /// <remarks>
        /// When this property is set to <c>true</c>, the button is rendered with a flat appearance without raised borders or shadow effects.
        /// This provides a more modern, minimalistic look that is commonly used in dialog buttons. When set to <c>false</c>, 
        /// the button will have a more traditional raised appearance with borders and shadow effects.
        /// </remarks>
        [Parameter]
        public bool IsFlat { get; set; } = true;

        /// <exclude/>
        /// <summary> 
        /// Gets or sets a a value that indicates the collection of additional attributes that will applied to the button container element. 
        /// </summary> 
        [Parameter(CaptureUnmatchedValues = true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Dictionary<string, object> HtmlAttributes { get; set; } = [];

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method is called after the component has been initialized and is ready to start processing.
        /// It initializes the button properties and registers the button with its parent <see cref="DialogButtons"/> container.
        /// </remarks>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);
            if (Parent is null)
            {
                return;
            }
            _tagIndex = Parent.UpdateChildProperty(this);
            _content = Content;
            _cssClass = CssClass;
            _disabled = Disabled;
            _iconCss = IconCss;
            _isPrimary = IsPrimary;
            _iconPosition = IconPosition;
            _isToggle = IsToggle;
            _type = Type;
            Parent.Buttons[_tagIndex] = this;
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method is called whenever the component's parameters change. It compares the current parameter values
        /// with the previous values and triggers a refresh of the parent <see cref="DialogButtons"/> container when changes are detected.
        /// </remarks>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(false);
            if (!string.Equals(Content, _content, StringComparison.Ordinal) || !string.Equals(CssClass, _cssClass, StringComparison.Ordinal) || Disabled != _disabled
                || !string.Equals(IconCss, _iconCss, StringComparison.Ordinal) || IsPrimary != _isPrimary || IconPosition != _iconPosition
                || IsToggle != _isToggle || Type != _type)
            {
                _content = Content;
                _cssClass = CssClass;
                _disabled = Disabled;
                _iconCss = IconCss;
                _isPrimary = IsPrimary;
                _iconPosition = IconPosition;
                _isToggle = IsToggle;
                _type = Type;
                Parent?.Refresh();
            }
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="DialogButton"/> and optionally releases the managed resources.
        /// </summary>
        /// <remarks>
        /// This method is called by the <see cref="DisposeAsyncCore()"/> method and the finalizer.
        /// it releases all managed resources including removing the button from its parent container and updating button indices.
        /// </remarks>
        protected override ValueTask DisposeAsyncCore()
        {
            if (Parent is not null)
            {
                _ = Parent.Buttons.Remove(this);
                List<DialogButton> btns = Parent.Buttons;
                if (btns is not null)
                {
                    for (int i = 0; i < btns.Count; i++)
                    {
                        Parent.Buttons[i]._tagIndex = i;
                    }
                }

                ChildContent = null;
                Parent = null;
            }
            return base.DisposeAsyncCore();
        }
    }
}