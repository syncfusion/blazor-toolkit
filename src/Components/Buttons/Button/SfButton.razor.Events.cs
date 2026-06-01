using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Syncfusion.Blazor.Toolkit.Buttons
{
    public partial class SfButton
    {
        #region Events

        /// <summary>
        /// Gets or sets an event callback that is invoked when the button rendering is completed.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{Object}"/> that fires after the component renders for the first time.
        /// </value>
        /// <remarks>
        /// Use this event to perform custom logic or initialization when the button is first rendered in the DOM.
        /// The callback is invoked only once on the initial render.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfButton Created="@OnReady" Content="Loaded!" />
        /// @code {
        ///     private void OnReady(object args)
        ///     {
        ///         // Additional logic post render
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// Gets or sets an event callback that is invoked when the button is clicked through UI interaction.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{MouseEventArgs}"/> representing the click event handler.
        /// </value>
        /// <remarks>
        /// The <see cref="OnClick"/> event is triggered for user-initiated clicks on the button.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfButton OnClick="@Clicked" Content="Click Me" />
        /// @code {
        ///     private void Clicked(MouseEventArgs args)
        ///     {
        ///         // Handle click event
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }

        #endregion
    }
}
