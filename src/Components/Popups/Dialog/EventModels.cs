using System.ComponentModel;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components.Web;

namespace Syncfusion.Blazor.Toolkit.Popups
{
    /// <summary>
    /// Provides the dimension data for the <see cref="SfDialog.GetDimensionAsync"/> method.
    /// </summary>
    /// <remarks>
    /// This class holds the width and height of the dialog, which can be retrieved programmatically.
    /// </remarks>
    public class DialogDimension
    {
        /// <summary>
        /// Gets or sets the current width of the dialog in pixels.
        /// </summary>
        /// <value>
        /// An <c>int</c> representing the width of the dialog.
        /// </value>
        /// <remarks>
        /// This value corresponds to the runtime width of the dialog component.
        /// </remarks>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the current height of the dialog in pixels.
        /// </summary>
        /// <value>
        /// An <c>int</c> representing the height of the dialog.
        /// </value>
        /// <remarks>
        /// This value corresponds to the runtime height of the dialog component.
        /// </remarks>
        public int Height { get; set; }
    }

    /// <summary>
    /// Provides arguments for the <see cref="SfDialog.OverlayClickHandlerAsync(MouseEventArgs)" /> event.
    /// </summary>
    /// <remarks>
    /// This event is triggered when the user clicks on the overlay of a modal dialog.
    /// </remarks>
    public class OverlayModalClickEventArgs
    {
        /// <summary>
        /// Gets the mouse event arguments associated with the overlay click.
        /// </summary>
        /// <value>
        /// A <see cref="MouseEventArgs"/> object containing details about the mouse event.
        /// </value>
        /// <remarks>
        /// Provides information such as the mouse cursor's position and which button was pressed.
        /// </remarks>
        public MouseEventArgs Event { get; set; } = new();

        /// <summary>
        /// Gets or sets a value indicating whether to prevent the dialog from automatically focusing on the first focusable element.
        /// </summary>
        /// <value>
        /// A <c>bool</c>. The default is <c>false</c>.
        /// </value>
        /// <remarks>
        /// When set to <c>true</c>, the default behavior of focusing on the first element is suppressed.
        /// </remarks>
        public bool PreventFocus { get; set; }
    }

    /// <summary>
    /// Provides arguments for the <see cref="SfDialog.OnClose"/> event, which is triggered before the dialog closes.
    /// </summary>
    /// <remarks>
    /// This event can be used to perform actions or validations before the dialog is closed. The closing process can be canceled by setting the <see cref="Cancel"/> property to <c>true</c>.
    /// </remarks>
    public class BeforeCloseEventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether the closing of the dialog should be canceled.
        /// </summary>
        /// <value>
        /// A <c>bool</c>. The default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// Set this property to <c>true</c> to prevent the dialog from closing.
        /// </remarks>
        public bool Cancel { get; set; }

        /// <summary>
        /// Gets a value indicating how the dialog was requested to be closed.
        /// </summary>
        /// <value>
        /// A <c>string</c> that specifies the source of the close action (e.g., "CloseIcon", "Escape", or a custom user action).
        /// </value>
        /// <remarks>
        /// This property helps determine whether the dialog was closed via the close icon, the Escape key, or another user-defined action.
        /// </remarks>
        public string ClosedBy { get; set; } = string.Empty;

        /// <summary>
        /// Gets the underlying event arguments that triggered the close action.
        /// </summary>
        /// <value>
        /// An <see cref="EventArgs"/> object.
        /// </value>
        /// <remarks>
        /// This can be a <see cref="MouseEventArgs"/> for a click or a <see cref="KeyboardEventArgs"/> for a key press.
        /// </remarks>
        public EventArgs Event { get; set; } = new();

        /// <summary>
        /// Gets a value indicating whether the dialog was closed through user interaction.
        /// </summary>
        /// <value>
        /// <c>true</c> if the closing was initiated by the user; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// This allows distinguishing between programmatic closing and user-initiated actions.
        /// </remarks>
        public bool IsInteracted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to prevent restoring focus to the previously active element after the <see cref="SfDialog"/> closes.
        /// </summary>
        /// <value>
        /// <c>true</c> to prevent automatic focus restoration; <c>false</c> to restore focus to the previously active element.
        /// The default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// <para>
        /// When set to <c>true</c>, the <see cref="SfDialog"/> will not restore focus to the element that was active before the dialog opened,
        /// allowing for custom focus management. This is useful in multi-dialog workflows, form continuity scenarios, or when integrating
        /// with assistive technologies that handle focus independently.
        /// </para>
        /// <para>
        /// When set to <c>false</c> (default), the dialog automatically restores focus to the previously active element.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfDialog @bind-Visible="@IsVisible" OnClose="OnDialogClose">
        ///     <DialogTemplates>
        ///         <Content>Dialog content</Content>
        ///     </DialogTemplates>
        ///     <DialogButtons>
        ///         <DialogButton Content="Close" OnClick="@OnButtonClick" />
        ///     </DialogButtons>
        /// </SfDialog>
        ///
        /// @code {
        ///     private bool IsVisible { get; set; } = true;
        ///    
        ///     private void OnDialogClose(BeforeCloseEventArgs args)
        ///     {     
        ///         args.PreventFocus = true; // Prevent focus restoration
        ///     }
        ///    
        ///     private void OnButtonClick()
        ///     {
        ///         IsVisible = false;
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        public bool PreventFocus { get; set; }
    }

    /// <summary>
    /// Provides arguments for the <see cref="SfDialog.OnOpen"/> event, which is triggered before the dialog opens.
    /// </summary>
    /// <remarks>
    /// This event allows for customization or cancellation of the dialog before it is displayed.
    /// </remarks>
    public class BeforeOpenEventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether the opening of the dialog should be canceled.
        /// </summary>
        /// <value>
        /// A <c>bool</c>. The default is <c>false</c>.
        /// </value>
        /// <remarks>
        /// Setting this to <c>true</c> prevents the dialog from being shown.
        /// </remarks>
        public bool Cancel { get; set; }

        /// <summary>
        /// Gets or sets a value that overrides the dialog's maximum height.
        /// </summary>
        /// <value>
        /// A <c>string</c> representing the maximum height (e.g., "500px", "80%").
        /// </value>
        /// <remarks>
        /// This allows for dynamically adjusting the maximum height before the dialog is rendered.
        /// </remarks>
        public string MaxHeight { get; set; } = string.Empty;
    }

    /// <summary>
    /// Provides arguments for the <see cref="SfDialog.Closed"/> event, which is triggered after the dialog has been closed.
    /// </summary>
    /// <remarks>
    /// This event can be used to execute code after the dialog is no longer visible.
    /// </remarks>
    public class CloseEventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether a subsequent action should be canceled. This property is not typically used in the <c>Closed</c> event.
        /// </summary>
        /// <value>
        /// A <c>bool</c>. The default is <c>false</c>.
        /// </value>
        /// <remarks>
        /// While this property is available, it generally does not affect behavior as the dialog has already closed.
        /// </remarks>
        public bool Cancel { get; set; }

        /// <summary>
        /// Gets a value indicating how the dialog was closed.
        /// </summary>
        /// <value>
        /// A <c>string</c> that specifies the source of the close action (e.g., "CloseIcon", "Escape").
        /// </value>
        /// <remarks>
        /// This is useful for logging or analytics to track how users interact with the dialog.
        /// </remarks>
        public string ClosedBy { get; set; } = string.Empty;

        /// <summary>
        /// Gets the event arguments that triggered the close action.
        /// </summary>
        /// <value>
        /// An <see cref="EventArgs"/> object.
        /// </value>
        /// <remarks>
        /// This provides context on the original user or programmatic action that led to the dialog closing.
        /// </remarks>
        public EventArgs Event { get; set; } = new();

        /// <summary>
        /// Gets a value indicating whether the dialog was closed as a result of user interaction.
        /// </summary>
        /// <value>
        /// <c>true</c> if the closing was initiated by the user; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// This helps differentiate between a user closing the dialog versus a programmatic close.
        /// </remarks>
        public bool IsInteracted { get; set; }

        /// <summary>
        /// Gets the name of the event.
        /// </summary>
        /// <value>
        /// The event name as a <c>string</c>. For this event, the value is typically "Closed".
        /// </value>
        /// <remarks>
        /// This property identifies the event that was fired.
        /// </remarks>
        public string Name { get; set; } = string.Empty;
    }

    /// <summary>
    /// Provides arguments for the <see cref="SfDialog.OnDrag"/> event, which is triggered while the dialog is being dragged.
    /// </summary>
    /// <remarks>
    /// This event fires continuously as the user moves the dialog across the screen.
    /// </remarks>
    public class DragEventArgs
    {
        /// <summary>
        /// Gets the browser's mouse event arguments for the drag action.
        /// </summary>
        /// <value>
        /// A <see cref="MouseEventArgs"/> object.
        /// </value>
        /// <remarks>
        /// This provides detailed information about the mouse state, such as its current position.
        /// </remarks>
        [DefaultValue(null)]
        [JsonPropertyName("event")]
        public MouseEventArgs Event { get; set; } = new();

        /// <summary>
        /// Gets the name of the event.
        /// </summary>
        /// <value>
        /// The event name as a <c>string</c>, which is typically "OnDrag".
        /// </value>
        /// <remarks>
        /// Identifies the fired event.
        /// </remarks>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    /// <summary>
    /// Provides arguments for the <see cref="SfDialog.OnDragStart"/> event, which is triggered when the user begins dragging the dialog.
    /// </summary>
    /// <remarks>
    /// This event can be used to prepare for the drag operation or to prevent it by canceling the event if applicable.
    /// </remarks>
    public class DragStartEventArgs
    {
        /// <summary>
        /// Gets the browser's mouse event arguments for the drag start action.
        /// </summary>
        /// <value>
        /// A <see cref="MouseEventArgs"/> object.
        /// </value>
        /// <remarks>
        /// This provides detailed information about the mouse state at the start of the drag.
        /// </remarks>
        [DefaultValue(null)]
        [JsonPropertyName("event")]
        public MouseEventArgs Event { get; set; } = new();

        /// <summary>
        /// Gets the name of the event.
        /// </summary>
        /// <value>
        /// The event name as a <c>string</c>, typically "OnDragStart".
        /// </value>
        /// <remarks>
        /// Identifies the fired event.
        /// </remarks>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    /// <summary>
    /// Provides arguments for the <see cref="SfDialog.OnDragStop"/> event, which is triggered when the user stops dragging the dialog.
    /// </summary>
    /// <remarks>
    /// This event is useful for performing actions after the dialog has been moved to a new position.
    /// </remarks>
    public class DragStopEventArgs
    {
        /// <summary>
        /// Gets the browser's mouse event arguments for the drag stop action.
        /// </summary>
        /// <value>
        /// A <see cref="MouseEventArgs"/> object.
        /// </value>
        /// <remarks>
        /// This provides details such as the final position of the mouse cursor.
        /// </remarks>
        [DefaultValue(null)]
        [JsonPropertyName("event")]
        public MouseEventArgs Event { get; set; } = new();

        /// <summary>
        /// Gets the name of the event.
        /// </summary>
        /// <value>
        /// The event name as a <c>string</c>, typically "OnDragStop".
        /// </value>
        /// <remarks>
        /// Identifies the fired event.
        /// </remarks>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    /// <summary>
    /// Provides arguments for the <see cref="SfDialog.Opened"/> event, which is triggered after the dialog has opened.
    /// </summary>
    /// <remarks>
    /// This event is useful for executing code after the dialog is visible and fully rendered.
    /// </remarks>
    public class OpenEventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether a subsequent action should be canceled. This is not typically used in the <c>Opened</c> event.
        /// </summary>
        /// <value>
        /// A <c>bool</c>. The default is <c>false</c>.
        /// </value>
        /// <remarks>
        /// Since the dialog is already open, canceling has no effect on its visibility.
        /// </remarks>
        public bool Cancel { get; set; }

        /// <summary>
        /// Gets the name of the event.
        /// </summary>
        /// <value>
        /// The event name as a <c>string</c>, typically "Opened".
        /// </value>
        /// <remarks>
        /// Identifies the fired event.
        /// </remarks>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether to prevent the default behavior of focusing on the first focusable element.
        /// </summary>
        /// <value>
        /// A <c>bool</c>. The default is <c>false</c>.
        /// </value>
        /// <remarks>
        /// Set to <c>true</c> to handle focus manually after the dialog opens.
        /// </remarks>
        public bool PreventFocus { get; set; }
    }
}