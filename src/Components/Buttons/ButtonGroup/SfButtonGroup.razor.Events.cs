using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Buttons
{
    /// <content>
    /// Members (constants, fields, and properties) for <see cref="SfButtonGroup"/>.
    /// </content>
    public partial class SfButtonGroup
    {
        #region Events

        /// <summary>
        /// Gets or sets an event callback that is raised after the <see cref="SfButtonGroup"/> component has been rendered for the first time.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> of type <see cref="object"/>.
        /// </value>
        /// <remarks>
        /// This event is triggered when the component is first created, allowing for custom logic to be executed.
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to handle the <see cref="Created"/> event.
        /// <code><![CDATA[
        /// <SfButtonGroup Created="@OnButtonGroupCreated">
        ///   <Button>Left</Button>
        ///   <Button>Center</Button>
        ///   <Button>Right</Button>
        /// </SfButtonGroup>
        /// @code {
        ///     private void OnButtonGroupCreated()
        ///     {
        ///         // Custom logic to execute after the ButtonGroup is created.
        ///         Console.WriteLine("ButtonGroup has been created.");
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<object> Created { get; set; }

        #endregion
    }
}
