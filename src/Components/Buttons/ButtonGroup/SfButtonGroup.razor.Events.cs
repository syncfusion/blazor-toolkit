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
        /// An <see cref="EventCallback{Object}"/> that fires after the initial render.
        /// </value>
        /// <remarks>
        /// Use this event to perform custom logic when the ButtonGroup is first rendered in the DOM.
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
        ///     private void OnButtonGroupCreated(object args)
        ///     {
        ///         // Custom logic to execute after the ButtonGroup is created.
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<object> Created { get; set; }

        #endregion
    }
}
