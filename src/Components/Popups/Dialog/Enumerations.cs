using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Popups
{
    /// <summary>
    /// Provides options to configure built-in dialogs shown using <see cref="SfDialogService.ConfirmAsync(string, string, DialogOptions)"/>,
    /// <see cref="SfDialogService.AlertAsync(string, string, DialogOptions)"/> and <see cref="SfDialogService.PromptAsync(string, string, DialogOptions)"/> methods.
    /// </summary>
    /// <remarks>
    /// This class contains properties that allow customization of dialog appearance, behavior, positioning, animation, and button configurations for built-in dialog types.
    /// </remarks>
    public class DialogOptions
    {
        /// <summary>
        /// Gets or sets whether the dialog can be dragged by the end-user.
        /// </summary>
        /// <value>
        /// <c>true</c> if the dialog can be dragged by the user; otherwise <c>false</c>. The default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// When <see cref="AllowDragging"/> is set to <c>true</c>, it allows a user to drag the dialog by selecting the header and dragging it for repositioning the dialog within the viewport.
        /// </remarks>
        public bool AllowDragging { get; set; }

        /// <summary>
        /// Gets or sets whether to show the close icon in the title of the dialog.
        /// </summary>
        /// <value>
        /// <c>true</c> if the close icon is displayed in the dialog title section; otherwise <c>false</c>. The default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// When enabled, a close button (X) will be displayed in the dialog header, allowing users to close the dialog by clicking on it.
        /// </remarks>
        /// <seealso cref="CloseOnEscape"/>
        public bool ShowCloseIcon { get; set; }

        /// <summary>
        /// Gets or sets whether the dialog can be closed by pressing the escape (ESC) key.
        /// </summary>
        /// <value>
        /// <c>true</c> if the dialog can be closed by pressing the escape (ESC) key; otherwise <c>false</c>. The default value is <c>true</c>.
        /// </value>
        /// <remarks>
        /// When enabled, users can dismiss the dialog by pressing the Escape key on their keyboard, providing a convenient way to close the dialog without using the mouse.
        /// </remarks>
        /// <seealso cref="ShowCloseIcon"/>
        public bool CloseOnEscape { get; set; } = true;

        /// <summary>
        /// Gets or sets the CSS class name that can be appended to the root element of the utility dialog.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the CSS class names. The default value is <c>String.Empty</c>.
        /// </value>
        /// <remarks>
        /// One or more custom CSS classes can be added to a utility dialog to customize its appearance. Multiple CSS classes should be separated by spaces.
        /// </remarks>
        public string CssClass { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the width of the dialog.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the width of the dialog. The default value is <c>auto</c>.
        /// </value>
        /// <remarks>
        /// The width can be specified in pixels (e.g., "300px"), percentage (e.g., "50%"), or as <c>auto</c> to let the dialog size itself based on content.
        /// </remarks>
        /// <seealso cref="Height"/>
        public string Width { get; set; } = "auto";

        /// <summary>
        /// Gets or sets the height of the dialog.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the height of the dialog. The default value is <c>auto</c>.
        /// </value>
        /// <remarks>
        /// The height can be specified in pixels (e.g., "400px"), percentage (e.g., "50%"), or as <c>auto</c> to let the dialog size itself based on content.
        /// </remarks>
        /// <seealso cref="Width"/>
        public string Height { get; set; } = "auto";

        /// <summary>
        /// Gets or sets the z-order that determines whether the utility dialog is displayed in front of or behind another component.
        /// </summary>
        /// <value>
        /// An <see cref="int"/> representing the z-index value. The default value is <c>1000</c>.
        /// </value>
        /// <remarks>
        /// The <see cref="ZIndex"/> value range can be changed depending on the application's needs. Higher values will display the dialog above elements with lower z-index values.
        /// </remarks>
        /// <seealso cref="CssClass"/>
        public int ZIndex { get; set; } = 1000;

        /// <summary>
        /// Gets or sets the position where the utility dialog can be positioned within the document or target.
        /// </summary>
        /// <value>
        /// A <see cref="PositionDataModel"/> representing the position configuration. The default value centers the dialog.
        /// </value>
        /// <remarks>
        /// The position can be represented with pre-configured positions or specific X and Y values.
        /// <c>PositionDataModel.X</c> can be configured with left, center, right, or offset value.
        /// <c>PositionDataModel.Y</c> can be configured with top, center, bottom, or offset value.
        /// </remarks>
        public PositionDataModel Position { get; set; } = default!;

        /// <summary>
        /// Gets or sets the animation settings of the dialog component.
        /// </summary>
        /// <value>
        /// A <see cref="DialogAnimationOptions"/> representing the animation configuration. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// The animation effect can be applied to open and close the dialog with configurable duration, delay, and effect type.
        /// </remarks>
        public DialogAnimationOptions AnimationSettings { get; set; } = default!;

        /// <summary>
        /// Gets or sets the primary (OK) action button settings.
        /// </summary>
        /// <value>
        /// A <see cref="DialogButtonOptions"/> representing the primary button configuration. The default value is a new instance of <see cref="DialogButtonOptions"/>.
        /// </value>
        /// <remarks>
        /// This property allows customization of the primary button's appearance and behavior, including its text content, icon, and enabled state.
        /// </remarks>
        /// <seealso cref="CancelButtonOptions"/>
        public DialogButtonOptions PrimaryButtonOptions { get; set; } = default!;

        /// <summary>
        /// Gets or sets the cancel action button settings.
        /// </summary>
        /// <value>
        /// A <see cref="DialogButtonOptions"/> representing the cancel button configuration. The default value is a new instance of <see cref="DialogButtonOptions"/>.
        /// </value>
        /// <remarks>
        /// This property allows customization of the cancel button's appearance and behavior, including its text content, icon, and enabled state.
        /// Note that this property is not applicable for alert dialogs.
        /// </remarks>
        /// <seealso cref="PrimaryButtonOptions"/>
        public DialogButtonOptions CancelButtonOptions { get; set; } = default!;

        /// <summary>
        /// Gets or sets the template content that is used as the dialog content.
        /// </summary>
        /// <value>
        /// A <see cref="RenderFragment"/> that represents the custom template content. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// When this property is set, the custom template will be rendered instead of the default content text. This allows for rich content including other Blazor components.
        /// </remarks>
        /// <example>
        /// In the code example below, the child content is configured for the prompt dialog box:
        /// <code><![CDATA[
        /// await DialogService.PromptAsync(null, "Enter Your Details", new DialogOptions()
        /// {
        ///     ChildContent = @<SfTextBox Placeholder="Enter Name"></SfTextBox>
        /// });
        /// ]]></code>
        /// </example>
        public RenderFragment ChildContent { get; set; } = default!;
    }

    /// <summary>
    /// Provides options to configure dialog buttons of built-in dialogs shown using <see cref="SfDialogService.ConfirmAsync(string, string, DialogOptions)"/>,
    /// <see cref="SfDialogService.AlertAsync(string, string, DialogOptions)"/> and <see cref="SfDialogService.PromptAsync(string, string, DialogOptions)"/> methods.
    /// </summary>
    /// <remarks>
    /// This class allows customization of button appearance and behavior, including text content, icons, and enabled state for both primary and cancel buttons in dialogs.
    /// </remarks>
    public class DialogButtonOptions
    {
        /// <summary>
        /// Gets or sets the text content of the button.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the button text. The default value is <c>String.Empty</c>.
        /// </value>
        /// <remarks>
        /// This property defines the text that will be displayed on the dialog button. If not specified, default button text will be used based on the dialog type.
        /// </remarks>
        public string Content { get; set; } = default!;

        /// <summary>
        /// Gets or sets one or more CSS classes to include an icon or image for the button.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing CSS classes separated by spaces to include an icon or image for the button. The default value is <c>String.Empty</c>.
        /// </value>
        /// <remarks>
        /// Buttons can include font icons and sprite images. Multiple CSS classes should be separated by spaces to apply multiple styling effects.
        /// </remarks>
        public string IconCss { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets whether the button is enabled or disabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if the button is disabled; otherwise <c>false</c>. The default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// When set to <c>true</c>, the button will be disabled and cannot be clicked by the user. The button will appear grayed out in the UI.
        /// </remarks>
        public bool Disabled { get; set; }
    }

    /// <summary>
    /// The animation options for the built-in dialogs shown using <see cref="SfDialogService.ConfirmAsync(string, string, DialogOptions)"/>,
    /// <see cref="SfDialogService.AlertAsync(string, string, DialogOptions)"/> and <see cref="SfDialogService.PromptAsync(string, string, DialogOptions)"/> methods.
    /// </summary>
    /// <remarks>
    /// This class provides properties to control the animation behavior when opening and closing dialogs, including delay, duration, and animation effects.
    /// </remarks>
    public class DialogAnimationOptions
    {
        /// <summary>
        /// Gets or sets the delay in milliseconds to start the animation.
        /// </summary>
        /// <value>
        /// An <see cref="int"/> representing the amount of time in milliseconds to delay animation before start. The default value is <c>0</c>.
        /// </value>
        /// <remarks>
        /// This property allows you to introduce a delay before the dialog animation begins, which can be useful for creating sequential animations or timing effects.
        /// </remarks>
        public int Delay { get; set; }

        /// <summary>
        /// Gets or sets the duration in milliseconds that the animation takes to open or close the dialog.
        /// </summary>
        /// <value>
        /// An <see cref="int"/> representing the amount of time in milliseconds to complete the animation. The default value is <c>400</c>.
        /// </value>
        /// <remarks>
        /// This property controls how long the dialog takes to animate in or out. Shorter durations create snappier animations, while longer durations create smoother, more gradual transitions.
        /// </remarks>
        public int Duration { get; set; } = 400;

        /// <summary>
        /// Gets or sets the animation effect to apply when opening and closing the dialog.
        /// </summary>
        /// <value>
        /// A <see cref="DialogEffect"/> value representing the animation effect. The default value varies based on the dialog type.
        /// </value>
        /// <remarks>
        /// If the user sets Fade animation, the dialog will open with the <c>FadeIn</c> effect and close with the <c>FadeOut</c> effect.
        /// The following are the list of animation effects available to configure:
        /// <c>Fade</c>, <c>FadeZoom</c>, <c>FlipLeftDown</c>, <c>FlipLeftUp</c>, <c>FlipRightDown</c>, <c>FlipRightUp</c>, 
        /// <c>FlipXDown</c>, <c>FlipXUp</c>, <c>FlipYLeft</c>, <c>FlipYRight</c>, <c>SlideBottom</c>, <c>SlideLeft</c>, 
        /// <c>SlideRight</c>, <c>SlideTop</c>, <c>Zoom</c>, and <c>None</c>.
        /// </remarks>
        public DialogEffect Effect { get; set; }
    }
}