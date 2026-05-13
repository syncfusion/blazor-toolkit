namespace Syncfusion.Blazor.Toolkit.Inputs
{
    /// <summary>
    /// Provides data for the value change event of the <see cref="SfTextArea"/> component.
    /// </summary>
    /// <remarks>
    /// This class contains information about the value change event, including the old and new values,
    /// and whether the change was triggered by user interaction or programmatically.
    /// </remarks>
    public class TextAreaValueChangeEventArgs
    {
        /// <summary> 
        /// Gets or sets a value indicating whether the event is triggered by user interaction. 
        /// </summary> 
        /// <value> 
        /// <c>true</c> if the component has been interacted with by the user; otherwise, <c>false</c>. 
        /// </value> 
        /// <remarks>
        /// This property helps distinguish between user-initiated changes (such as typing or pasting)
        /// and programmatic changes made through code. This is useful for implementing custom validation
        /// logic or preventing infinite loops in event handlers.
        /// </remarks>
        public bool IsInteracted { get; set; }

        /// <summary> 
        /// Gets or sets the previously entered value of the <see cref="SfTextArea"/>. 
        /// </summary> 
        /// <value> 
        /// A <c>string</c> representing the previous value of the TextArea. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property contains the value that was present in the TextArea before the current change occurred.
        /// It can be used to compare with the new value or to implement undo functionality.
        /// </remarks>
        public string? PreviousValue { get; set; }

        /// <summary> 
        /// Gets or sets the current entered value of the <see cref="SfTextArea"/>. 
        /// </summary> 
        /// <value> 
        /// A <c>string</c> representing the current value of the TextArea. The default value is <c>null</c>.
        /// </value> 
        /// <remarks>
        /// This property contains the new value after the change has occurred. This is the value
        /// that will be displayed in the TextArea component after the event completes.
        /// </remarks>
        public string? Value { get; set; }
    }

    /// <summary>
    /// Provides data for the input event of the <see cref="SfTextArea"/> component.
    /// </summary>
    /// <remarks>
    /// This class contains information about the input event that occurs while the user is typing
    /// or entering text in the TextArea. This event is fired for every character input or deletion.
    /// </remarks>
    public class TextAreaInputEventArgs
    {
        /// <summary>
        /// Gets or sets the event parameters from the TextArea component.
        /// </summary>
        /// <value>
        /// An <see cref="EventArgs"/> object that contains the event data associated with the TextArea input event.
        /// </value>
        /// <remarks>
        /// <para>This property provides details about the input event, such as event source and timestamp information.</para>
        /// </remarks>
        public EventArgs? Event { get; set; } = new();

        /// <summary> 
        /// Gets or sets the previously updated value of the <see cref="SfTextArea"/>. 
        /// </summary> 
        /// <value> 
        /// A <c>string</c> representing the previous value of the TextArea. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property contains the value that was present in the TextArea before the current input occurred.
        /// It provides context for tracking incremental changes during text input operations.
        /// </remarks>
        public string? PreviousValue { get; set; }

        /// <summary> 
        /// Gets or sets the current entered value of the <see cref="SfTextArea"/>. 
        /// </summary> 
        /// <value> 
        /// A <c>string</c> representing the current value being input in the TextArea. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property contains the current value as it's being typed. The input event is triggered
        /// for each character addition or deletion, making this useful for real-time validation or
        /// character counting functionality.
        /// </remarks>
        public string? Value { get; set; }
    }

    /// <summary>
    /// Provides data for the focus in event of the <see cref="SfTextArea"/> component.
    /// </summary>
    /// <remarks>
    /// This class contains information about the focus in event that occurs when the TextArea
    /// receives focus, typically when a user clicks on it or navigates to it using keyboard navigation.
    /// </remarks>
    public class TextAreaFocusInEventArgs
    {
        /// <summary>
        /// Gets or sets the event parameters from the TextArea component.
        /// </summary>
        /// <value>
        /// An <see cref="EventArgs"/> object that contains the event data associated with the TextArea focus in event.
        /// </value>
        /// <remarks>
        /// <para>This property contains details about the focus event, including the source and related timing information.</para>
        /// </remarks>
        public EventArgs? Event { get; set; }

        /// <summary> 
        /// Gets or sets the current value of the <see cref="SfTextArea"/> when it receives focus. 
        /// </summary> 
        /// <value> 
        /// A <c>string</c> representing the current value of the TextArea when focus is gained. The default value is <c>null</c>.
        /// </value> 
        /// <remarks>
        /// This property provides access to the TextArea's value at the moment it gains focus.
        /// This can be useful for tracking the initial value when implementing custom focus-based
        /// behaviors or validation logic.
        /// </remarks>
        public string? Value { get; set; }
    }

    /// <summary>
    /// Provides data for the focus out event of the <see cref="SfTextArea"/> component.
    /// </summary>
    /// <remarks>
    /// This class contains information about the focus out event that occurs when the TextArea
    /// loses focus, typically when a user clicks elsewhere or navigates away using keyboard navigation.
    /// This event is commonly used for validation or saving data.
    /// </remarks>
    public class TextAreaFocusOutEventArgs
    {
        /// <summary>
        /// Gets or sets the event parameters from the TextArea component.
        /// </summary>
        /// <value>
        /// An <see cref="EventArgs"/> object that contains the event data associated with the TextArea focus out event.
        /// </value>
        /// <remarks>
        /// <para>This property contains details about the blur event, including timing and focus-related information.</para>
        /// </remarks>
        public EventArgs Event { get; set; } = new();

        /// <summary> 
        /// Gets or sets the current value of the <see cref="SfTextArea"/> when it loses focus. 
        /// </summary>	 
        /// <value> 
        /// A <c>string</c> representing the current value of the TextArea when focus is lost. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property provides access to the TextArea's value at the moment it loses focus.
        /// This is particularly useful for performing validation, formatting, or saving operations
        /// after the user has finished entering text.
        /// </remarks>
        public string? Value { get; set; }
    }
}
