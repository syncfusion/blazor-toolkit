using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    /// <summary>
    /// Partial class containing event definitions for <see cref="SfSelectionBase{TChecked}"/>.
    /// </summary>
    public partial class SfSelectionBase<TChecked>
    {
        #region Events

        /// <summary>
        /// Gets or sets an event callback that is invoked when the component has been initialized and rendered.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{Object}"/> that is triggered after the component's first render.
        /// </value>
        /// <remarks>
        /// Use this event to perform custom initialization logic or to access the component
        /// after it has been fully created and rendered in the DOM.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfCheckBox Created="@OnCheckBoxCreated" Label="Accept Terms" />
        /// 
        /// @code {
        ///     private void OnCheckBoxCreated(object args)
        ///     {
        ///         // Perform post-initialization tasks
        ///         Console.WriteLine("CheckBox created successfully");
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<object> Created { get; set; }

        /// <exclude/>
        /// <summary>
        /// Gets or sets an event callback for two-way binding of the Checked property.
        /// </summary>
        /// <remarks>
        /// This event is automatically invoked by the Blazor framework to support @bind-Checked syntax.
        /// Do not call this directly; use the Checked property with two-way binding instead.
        /// </remarks>
        [Parameter]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public EventCallback<TChecked> CheckedChanged { get; set; }

        #endregion
    }

    #region Event Arguments

    /// <summary>
    /// Provides event data for the <c>ValueChange</c> event of checkbox and toggle components 
    /// in the Syncfusion Blazor Toolkit.
    /// </summary>
    /// <typeparam name="TChecked">The type of the checked value (typically <see cref="bool"/>).</typeparam>
    /// <remarks>
    /// This class contains information about the changed value and the mouse event that triggered the change.
    /// It is used by checkbox and switch components to provide context about value changes.
    /// </remarks>
    /// <example>
    /// The following example demonstrates handling a value change event:
    /// <code><![CDATA[
    /// <SfCheckBox @bind-Checked="isChecked" ValueChange="@OnValueChanged" />
    ///
    /// @code {
    ///     private bool isChecked;
    ///     
    ///     private void OnValueChanged(ChangeEventArgs<bool> args)
    ///     {
    ///         bool newValue = args.Checked;
    ///         MouseEventArgs mouseEvent = args.Event;
    ///         Console.WriteLine($"Checkbox changed to: {newValue}");
    ///     }
    /// }
    /// ]]></code>
    /// </example>
    public class CheckedChangeEventArgs<TChecked> : EventArgs
    {
        /// <summary>
        /// Gets or sets the new checked value after the change occurred.
        /// </summary>
        /// <value>
        /// The checked value of type <typeparamref name="TChecked"/> representing the current state.
        /// </value>
        /// <remarks>
        /// This property contains the updated value after the user interaction or programmatic change.
        /// For boolean checkboxes, this is typically <c>true</c> when checked or <c>false</c> when unchecked.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// private void OnValueChanged(ChangeEventArgs<bool> args)
        /// {
        ///     bool isNowChecked = args.Checked;
        /// }
        /// ]]></code>
        /// </example>
        public TChecked? Checked { get; set; }

        /// <summary>
        /// Gets or sets the mouse event that triggered the value change.
        /// </summary>
        /// <value>
        /// A <see cref="MouseEventArgs"/> object containing mouse event details, or <c>null</c> if the change was programmatic.
        /// </value>
        /// <remarks>
        /// This property provides additional context about the user interaction, including mouse position,
        /// button details, and modifier keys. It is <c>null</c> when the value is changed programmatically
        /// rather than through user interaction.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// private void OnValueChanged(ChangeEventArgs<bool> args)
        /// {
        ///     if (args.Event != null)
        ///     {
        ///         long clickX = args.Event.ClientX;
        ///         long clickY = args.Event.ClientY;
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        public MouseEventArgs? Event { get; set; }
    }

    /// <summary>
    /// Provides data for the value change event of radio button components.
    /// </summary>
    /// <typeparam name="TChecked">The type of the radio button value.</typeparam>
    /// <remarks>
    /// This class contains information about the changed value and the event that triggered the change.
    /// It is used by radio button components to provide context about selection changes.
    /// </remarks>
    /// <example>
    /// The following example demonstrates handling a radio button value change:
    /// <code><![CDATA[
    /// <SfRadioButton TValue="string" @bind-Value="selectedOption" 
    ///                Value="option1" ValueChange="@OnRadioChanged" />
    ///
    /// @code {
    ///     private string selectedOption;
    ///     
    ///     private void OnRadioChanged(ChangeArgs<string> args)
    ///     {
    ///         string newValue = args.Value;
    ///         Console.WriteLine($"Radio button changed to: {newValue}");
    ///     }
    /// }
    /// ]]></code>
    /// </example>
    public class ChangeArgs<TChecked>
    {
        /// <summary>
        /// Gets or sets the new value after the radio button selection changed.
        /// </summary>
        /// <value>
        /// The selected value of type <typeparamref name="TChecked"/>.
        /// </value>
        /// <remarks>
        /// This property contains the value of the newly selected radio button in the group.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// private void OnRadioChanged(ChangeArgs<string> args)
        /// {
        ///     string selectedValue = args.Value;
        /// }
        /// ]]></code>
        /// </example>
        public TChecked? Value { get; set; }

        /// <summary>
        /// Gets or sets the event arguments associated with the value change.
        /// </summary>
        /// <value>
        /// An <see cref="EventArgs"/> object containing basic event information.
        /// </value>
        /// <remarks>
        /// This property provides basic event context for the radio button value change.
        /// Unlike <see cref="ChangeEventArgs{TChecked}.Event"/>, this does not contain
        /// mouse-specific information.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// private void OnRadioChanged(ChangeArgs<string> args)
        /// {
        ///     var eventArgs = args.Event;
        ///     // Basic event information
        /// }
        /// ]]></code>
        /// </example>
        public EventArgs? Event { get; set; }
    }

    #endregion
}
