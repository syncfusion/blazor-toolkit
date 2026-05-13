namespace Syncfusion.Blazor.Toolkit.Inputs
{
    /// <summary>
    /// Provides data for the <see cref="SfTextBox"/> changed event that occurs when the value of the component is modified.
    /// </summary>
    /// <remarks>
    /// <para>This event is triggered after the value has been changed and the component has lost focus.</para>
    /// </remarks>
    public class ChangedEventArgs
    {
        /// <summary>
        /// Gets or sets the event parameters from the TextBox component.
        /// </summary>
        /// <value>
        /// An <see cref="EventArgs"/> object that contains the event data associated with the TextBox changed event.
        /// </value>
        public EventArgs? Event { get; set; } = new();

        /// <summary>
        /// Gets or sets a value indicating whether the event is triggered by user interaction.
        /// </summary>
        /// <value>
        /// A <see cref="bool"/> value that is <see langword="true"/> if the component has been interacted with by the user through typing, pasting, or other input methods; otherwise, <see langword="false"/> if the change was programmatic. The default value is <see langword="false"/>.
        /// </value>
        public bool IsInteracted { get; set; }

        /// <summary>
        /// Gets or sets the previously entered value of the TextBox before the current change.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the previous value, or <see langword="null"/> if no previous value exists.
        /// </value>
        public string? PreviousValue { get; set; }

        /// <summary>
        /// Gets or sets the current entered value of the TextBox.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the current value. This property is may be an empty string or <see langword="null"/> if no text is entered.
        /// </value>
        public string? Value { get; set; }
    }

    /// <summary>
    /// Provides data for the <see cref="SfTextBox"/> focus in event that occurs when the component receives focus.
    /// </summary>
    /// <remarks>
    /// <para>This event is triggered when the user clicks on the TextBox or navigates to it using keyboard navigation (Tab key).</para>
    /// </remarks>
    public class FocusInEventArgs
    {
        /// <summary>
        /// Gets or sets the event parameters from the TextBox component.
        /// </summary>
        /// <value>
        /// An <see cref="EventArgs"/> object that contains the event data associated with the TextBox focus in event.
        /// </value>
        /// <remarks>
        /// <para>This property contains details about the focus event, including the source and related timing information.</para>
        /// </remarks>
        public EventArgs? Event { get; set; }

        /// <summary>
        /// Gets or sets the current entered value of the TextBox when focus is received.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the current value at the time focus was received, or <see langword="null"/> if no value is entered.
        /// </value>
        public string? Value { get; set; }
    }

    /// <summary>
    /// Provides data for the <see cref="SfTextBox"/> focus out event that occurs when the component loses focus.
    /// </summary>
    /// <remarks>
    /// <para>This event is triggered when the user clicks outside the TextBox or navigates away from it using keyboard navigation.</para>
    /// </remarks>
    public class FocusOutEventArgs
    {
        /// <summary>
        /// Gets or sets the event parameters from the TextBox component.
        /// </summary>
        /// <value>
        /// An <see cref="EventArgs"/> object that contains the event data associated with the TextBox focus out event.
        /// </value>
        /// <remarks>
        /// <para>This property contains details about the blur event, including timing and focus-related information.</para>
        /// </remarks>
        public EventArgs Event { get; set; } = new();

        /// <summary>
        /// Gets or sets the current entered value of the TextBox when focus is lost.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the current value at the time focus was lost. This property is may be an empty string or <see langword="null"/> if no text is entered.
        /// </value>
        public string? Value { get; set; }
    }

    /// <summary>
    /// Provides data for the <see cref="SfTextBox"/> input event that occurs during real-time text input.
    /// </summary>
    /// <remarks>
    /// <para>This event is triggered while the user is typing, providing immediate feedback for each character entered or deleted.</para>
    /// </remarks>
    public class InputEventArgs
    {
        /// <summary>
        /// Gets or sets the event parameters from the TextBox component.
        /// </summary>
        /// <value>
        /// An <see cref="EventArgs"/> object that contains the event data associated with the TextBox input event.
        /// </value>
        /// <remarks>
        /// <para>This property provides details about the input event, such as event source and timestamp information.</para>
        /// </remarks>
        public EventArgs? Event { get; set; } = new();

        /// <summary>
        /// Gets or sets the previously updated value of the TextBox before the current input change.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the previous value before the current keystroke or input action, or <see langword="null"/> if no previous value exists.
        /// </value>
        /// <remarks>
        /// <para>This property enables detection of incremental changes by comparing the previous and current values.</para>
        /// </remarks>
        public string? PreviousValue { get; set; }

        /// <summary>
        /// Gets or sets the current entered value of the TextBox during input.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the current value being entered after the latest input action, or <see langword="null"/> if no value is entered.
        /// </value>
        /// <remarks>
        /// <para>This property reflects the value in real-time as the user types. It may be empty or <see langword="null"/> if the input field is cleared.</para>
        /// </remarks>
        public string? Value { get; set; }
    }
}