using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    public abstract partial class SfInputBase<TValue>
    {
        #region Events

        /// <summary>
        /// Gets or sets the event callback that is triggered when the content of the input has changed and the element loses focus.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{ChangeEventArgs}"/> representing the change event handler. The callback receives a <see cref="ChangeEventArgs"/>.
        /// </value>
        /// <remarks>
        /// This event is fired when the input is completed and the element loses focus, allowing response to completed changes.
        /// </remarks>
        /// <exclude/>
        [Parameter]
        public EventCallback<ChangeEventArgs> OnChange { get; set; }

        /// <summary>
        /// Gets or sets the event callback that is triggered when content is pasted into the input element.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{ClipboardEventArgs}"/> representing the paste event handler. The callback receives a <see cref="ClipboardEventArgs"/>.
        /// </value>
        /// <remarks>
        /// This event allows handling of paste operations and performing custom logic such as data validation or formatting.
        /// </remarks>
        /// <exclude/>
        [Parameter]
        public EventCallback<ClipboardEventArgs> OnPaste { get; set; }

        /// <summary>
        /// Gets or sets the event callback that is triggered when the value of the component changes.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> representing the value change event handler. The callback receives a <typeparamref name="TValue"/>.
        /// </value>
        /// <remarks>
        /// This callback is essential for two-way data binding and is automatically invoked when the component's value is modified.
        /// Use this to respond to value changes and update the application state accordingly.
        /// </remarks>
        [Parameter]
        public EventCallback<TValue> ValueChanged { get; set; }

        #endregion
    }
}
