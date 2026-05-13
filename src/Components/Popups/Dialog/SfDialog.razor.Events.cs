using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Syncfusion.Blazor.Toolkit.Popups
{
    public partial class SfDialog
    {
        /// <summary> 
        /// Gets or sets the event callback that will be invoked when the <see cref="SfDialog"/> is closed.  
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> that is invoked after the dialog is closed. The callback receives a <see cref="CloseEventArgs"/> argument.
        /// </value>
        /// <remarks>
        /// This event is triggered after the <see cref="SfDialog"/> has been completely closed and all closing animations have finished.
        /// It provides information about how the dialog was closed and allows you to perform cleanup operations or update application state.
        /// Use this event when you need to execute logic after the dialog has been fully closed.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Toolkit.Popups
        /// <SfDialog Width="500px" @bind-Visible="Visibility" Closed="OnClosedHandler">
        ///   <DialogTemplates>
        ///     <Content>
        ///       <p>Dialog content</p>
        ///     </Content>
        ///   </DialogTemplates>
        ///   <DialogButtons>
        ///     <DialogButton Content="Ok" OnClick="@OnBtnClick" />
        ///   </DialogButtons>
        /// </SfDialog>
        /// @code {
        ///   private bool Visibility { get; set; } = true;
        ///   private void OnClosedHandler(CloseEventArgs args)
        ///   {
        ///     // Write your code here.
        ///   }
        ///   private void OnBtnClick()
        ///   {
        ///     this.Visibility = false;
        ///   }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<CloseEventArgs> Closed { get; set; }

        /// <summary> 
        /// Gets or sets the event callback that will be invoked when the <see cref="SfDialog"/> rendering is completed.  
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> that is invoked when the dialog component has been created and rendered. The callback receives an <c>object</c> argument.
        /// </value>
        /// <remarks>
        /// This event is triggered once the <see cref="SfDialog"/> component has been fully created and rendered in the DOM.
        /// It provides an opportunity to perform initialization tasks, set up additional event handlers, or configure the dialog after it has been created.
        /// This event occurs during the component lifecycle, typically after the first render.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Toolkit.Popups
        /// <SfDialog Width="500px" @bind-Visible="Visibility" Created="OnCreatedHandler">
        ///   <DialogTemplates>
        ///     <Content>
        ///       <p>Dialog content</p>
        ///     </Content>
        ///   </DialogTemplates>
        /// </SfDialog>
        /// @code {
        ///   private bool Visibility { get; set; } = true;
        ///   private void OnCreatedHandler(object args)
        ///   {
        ///     // Write your initialization code here.
        ///   }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<object> Created { get; set; }

        /// <summary> 
        /// Gets or sets the event callback that will be invoked when the <see cref="SfDialog"/> disposing is completed.  
        /// </summary> 
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> that is invoked when the dialog component is being destroyed. The callback receives an <c>object</c> argument.
        /// </value>
        /// <remarks>
        /// This event is triggered when the <see cref="SfDialog"/> component is being disposed and removed from the DOM.
        /// It provides an opportunity to perform cleanup operations, remove event handlers, or free resources before the component is destroyed.
        /// This event occurs during the component disposal process, typically when the component is being unmounted.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Toolkit.Popups
        /// <SfDialog Width="500px" @bind-Visible="Visibility" Destroyed="OnDestroyedHandler">
        ///   <DialogTemplates>
        ///     <Content>
        ///       <p>Dialog content</p>
        ///     </Content>
        ///   </DialogTemplates>
        /// </SfDialog>
        /// @code {
        ///   private bool Visibility { get; set; } = true;
        ///   private void OnDestroyedHandler(object args)
        ///   {
        ///     // Write your cleanup code here.
        ///   }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<object> Destroyed { get; set; }

        /// <summary>
        /// Gets or sets the event callback that will be invoked before the <see cref="SfDialog"/> is closed.  
        /// </summary> 
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> that is invoked before the dialog is closed. The callback receives a <see cref="BeforeCloseEventArgs"/> argument.
        /// </value>
        /// <remarks>
        /// This event is triggered before the <see cref="SfDialog"/> begins its closing process and animations.
        /// It provides an opportunity to prevent the dialog from closing by setting the <c>Cancel</c> property of the event arguments to <c>true</c>.
        /// You can use this event to validate user input, show confirmation messages, or perform other pre-closing operations.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Toolkit.Popups
        /// <SfDialog Width="500px" @bind-Visible="Visibility" OnClose="OnCloseHandler">
        ///   <DialogTemplates>
        ///     <Content>
        ///       <p>Dialog content</p>
        ///     </Content>
        ///   </DialogTemplates>
        ///   <DialogButtons>
        ///     <DialogButton Content="Ok" OnClick="@OnBtnClick" />
        ///   </DialogButtons>
        /// </SfDialog>
        /// @code {
        ///   private bool Visibility { get; set; } = true;
        ///   private void OnCloseHandler(BeforeCloseEventArgs args)
        ///   {
        ///     // Write your code here.
        ///   }
        ///   private void OnBtnClick()
        ///   {
        ///     this.Visibility = false;
        ///   }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<BeforeCloseEventArgs> OnClose { get; set; }

        /// <summary>
        /// Gets or sets the event callback that will be invoked when the <see cref="SfDialog"/> is being dragged.  
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> that is invoked during the drag operation of the dialog. The callback receives a <see cref="DragEventArgs"/> argument.
        /// </value>
        /// <remarks>
        /// This event is triggered continuously while the <see cref="SfDialog"/> is being dragged by the user.
        /// It provides real-time information about the drag operation, including the current position and movement details.
        /// You can use this event to implement custom drag behaviors, update UI elements, or track the dialog's position during dragging.
        /// Note that dragging must be enabled on the dialog for this event to be triggered.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Toolkit.Popups
        /// <SfDialog Width="500px" @bind-Visible="Visibility" AllowDragging="true" OnDrag="OnDragHandler">
        ///   <DialogTemplates>
        ///     <Content>
        ///       <p>Dialog content</p>
        ///     </Content>
        ///   </DialogTemplates>
        /// </SfDialog>
        /// @code {
        ///   private bool Visibility { get; set; } = true;
        ///   private void OnDragHandler(DragEventArgs args)
        ///   {
        ///     // Write your code here.
        ///   }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<DragEventArgs> OnDrag { get; set; }

        /// <summary>
        /// Gets or sets the event callback that will be invoked when the drag operation of the <see cref="SfDialog"/> is initiated.
        /// </summary> 
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> that is invoked when the drag operation begins. The callback receives a <see cref="DragStartEventArgs"/> argument.
        /// </value>
        /// <remarks>
        /// This event is triggered when the user begins dragging the <see cref="SfDialog"/> by clicking and holding the dialog header or designated drag area.
        /// It provides information about the initial drag state and allows you to perform setup operations before the drag begins.
        /// You can use this event to initialize drag-related variables, show visual indicators, or cancel the drag operation if needed.
        /// Note that dragging must be enabled on the dialog for this event to be triggered.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Toolkit.Popups
        /// <SfDialog Width="500px" @bind-Visible="Visibility" AllowDragging="true" OnDragStart="OnDragStartHandler">
        ///   <DialogTemplates>
        ///     <Content>
        ///       <p>Dialog content</p>
        ///     </Content>
        ///   </DialogTemplates>
        /// </SfDialog>
        /// @code {
        ///   private bool Visibility { get; set; } = true;
        ///   private void OnDragStartHandler(DragStartEventArgs args)
        ///   {
        ///     // Write your code here.
        ///   }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<DragStartEventArgs> OnDragStart { get; set; }

        /// <summary>
        /// Gets or sets the event callback that will be invoked when the drag operation of the <see cref="SfDialog"/> is stopped.
        /// </summary> 
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> that is invoked when the drag operation ends. The callback receives a <see cref="DragStopEventArgs"/> argument.
        /// </value>
        /// <remarks>
        /// This event is triggered when the user stops dragging the <see cref="SfDialog"/> by releasing the mouse button or touch.
        /// It provides information about the final position and state of the dialog after the drag operation is complete.
        /// You can use this event to perform cleanup operations, save the dialog position, or update related UI elements after dragging ends.
        /// Note that dragging must be enabled on the dialog for this event to be triggered.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Toolkit.Popups
        /// <SfDialog Width="500px" @bind-Visible="Visibility" AllowDragging="true" OnDragStop="OnDragStopHandler">
        ///   <DialogTemplates>
        ///     <Content>
        ///       <p>Dialog content</p>
        ///     </Content>
        ///   </DialogTemplates>
        /// </SfDialog>
        /// @code {
        ///   private bool Visibility { get; set; } = true;
        ///   private void OnDragStopHandler(DragStopEventArgs args)
        ///   {
        ///     // Write your code here.
        ///   }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<DragStopEventArgs> OnDragStop { get; set; }

        /// <summary>
        /// Gets or sets the event callback that will be invoked before the <see cref="SfDialog"/> is opened.  
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> that is invoked before the dialog is opened. The callback receives a <see cref="BeforeOpenEventArgs"/> argument.
        /// </value>
        /// <remarks>
        /// This event is triggered before the <see cref="SfDialog"/> begins its opening process and animations.
        /// It provides an opportunity to prevent the dialog from opening by setting the <c>Cancel</c> property of the event arguments to <c>true</c>.
        /// You can use this event to validate conditions, prepare data, or perform other pre-opening operations before the dialog becomes visible.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Toolkit.Popups
        /// <SfDialog Width="500px" @bind-Visible="Visibility" OnOpen="OnOpenHandler">
        ///   <DialogTemplates>
        ///     <Content>
        ///       <p>Dialog content</p>
        ///     </Content>
        ///   </DialogTemplates>
        /// </SfDialog>
        /// @code {
        ///   private bool Visibility { get; set; } = true;
        ///   private void OnOpenHandler(BeforeOpenEventArgs args)
        ///   {
        ///     // Write your code here.
        ///   }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<BeforeOpenEventArgs> OnOpen { get; set; }

        /// <summary>
        /// Gets or sets the event callback that will be invoked when the <see cref="SfDialog"/> modal overlay is clicked.  
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> that is invoked when the modal overlay is clicked. The callback receives an <see cref="OverlayModalClickEventArgs"/> argument.
        /// </value>
        /// <remarks>
        /// This event is triggered when the user clicks on the modal overlay (backdrop) of the <see cref="SfDialog"/> when it is displayed in modal mode.
        /// It provides an opportunity to respond to overlay clicks, which commonly involves closing the dialog or showing confirmation messages.
        /// This event is only applicable when the dialog is configured as a modal dialog using the <c>IsModal</c> property.
        /// You can prevent the default overlay click behavior by handling this event appropriately.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Toolkit.Popups
        /// <SfDialog Width="500px" @bind-Visible="Visibility" IsModal="true" OnOverlayModalClick="OnOverlayModalClickHandler">
        ///   <DialogTemplates>
        ///     <Content>
        ///       <p>Dialog content</p>
        ///     </Content>
        ///   </DialogTemplates>
        /// </SfDialog>
        /// @code {
        ///   private bool Visibility { get; set; } = true;
        ///   private void OnOverlayModalClickHandler(OverlayModalClickEventArgs args)
        ///   {
        ///     // Write your code here.
        ///   }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<OverlayModalClickEventArgs> OnOverlayModalClick { get; set; }

        /// <summary>
        /// Gets or sets the event callback that will be invoked when the resize operation of the <see cref="SfDialog"/> is initiated.  
        /// </summary> 
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> that is invoked when the resize operation begins. The callback receives a <see cref="MouseEventArgs"/> argument.
        /// </value>
        /// <remarks>
        /// This event is triggered when the user begins resizing the <see cref="SfDialog"/> by clicking and dragging the resize handles.
        /// It provides information about the mouse event that initiated the resize operation.
        /// You can use this event to perform setup operations, show visual indicators, or initialize resize-related variables before the resize begins.
        /// Note that resizing must be enabled on the dialog for this event to be triggered.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Toolkit.Popups
        /// <SfDialog Width="500px" @bind-Visible="Visibility" EnableResize="true" OnResizeStart="OnResizeStartHandler">
        ///   <DialogTemplates>
        ///     <Content>
        ///       <p>Dialog content</p>
        ///     </Content>
        ///   </DialogTemplates>
        /// </SfDialog>
        /// @code {
        ///   private bool Visibility { get; set; } = true;
        ///   private void OnResizeStartHandler(MouseEventArgs args)
        ///   {
        ///     // Write your code here.
        ///   }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<MouseEventArgs> OnResizeStart { get; set; }

        /// <summary>
        /// Gets or sets the event callback that will be invoked when the resize operation of the <see cref="SfDialog"/> is stopped.  
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> that is invoked when the resize operation ends. The callback receives a <see cref="MouseEventArgs"/> argument.
        /// </value>
        /// <remarks>
        /// This event is triggered when the user stops resizing the <see cref="SfDialog"/> by releasing the mouse button after dragging a resize handle.
        /// It provides information about the final mouse event and allows you to perform cleanup operations or save the dialog's new size.
        /// You can use this event to update related UI elements, persist size settings, or trigger other actions after the resize is complete.
        /// Note that resizing must be enabled on the dialog for this event to be triggered.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Toolkit.Popups
        /// <SfDialog Width="500px" @bind-Visible="Visibility" EnableResize="true" OnResizeStop="OnResizeStopHandler">
        ///   <DialogTemplates>
        ///     <Content>
        ///       <p>Dialog content</p>
        ///     </Content>
        ///   </DialogTemplates>
        /// </SfDialog>
        /// @code {
        ///   private bool Visibility { get; set; } = true;
        ///   private void OnResizeStopHandler(MouseEventArgs args)
        ///   {
        ///     // Write your code here.
        ///   }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<MouseEventArgs> OnResizeStop { get; set; }

        /// <summary>
        /// Gets or sets the event callback that will be invoked when the <see cref="SfDialog"/> is opened.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> that is invoked after the dialog is opened. The callback receives an <see cref="OpenEventArgs"/> argument.
        /// </value>
        /// <remarks>
        /// This event is triggered after the <see cref="SfDialog"/> has been completely opened and all opening animations have finished.
        /// It provides confirmation that the dialog is now fully visible and interactive to the user.
        /// You can use this event to perform post-opening operations such as setting focus, initializing content, or updating application state.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Toolkit.Popups
        /// <SfDialog Width="500px" @bind-Visible="Visibility" Opened="OnOpenedHandler">
        ///   <DialogTemplates>
        ///     <Content>
        ///       <p>Dialog content</p>
        ///     </Content>
        ///   </DialogTemplates>
        /// </SfDialog>
        /// @code {
        ///   private bool Visibility { get; set; } = true;
        ///   private void OnOpenedHandler(OpenEventArgs args)
        ///   {
        ///     // Write your code here.
        ///   }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<OpenEventArgs> Opened { get; set; }

        /// <summary>
        /// Gets or sets the event callback that will be invoked when the <see cref="SfDialog"/> is being resized.  
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> that is invoked during the resize operation of the dialog. The callback receives a <see cref="MouseEventArgs"/> argument.
        /// </value>
        /// <remarks>
        /// This event is triggered continuously while the <see cref="SfDialog"/> is being resized by the user dragging the resize handles.
        /// It provides real-time information about the resize operation, including mouse position and movement details.
        /// You can use this event to implement custom resize behaviors, update UI elements in real-time, or apply constraints during the resize process.
        /// Note that resizing must be enabled on the dialog for this event to be triggered.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @using Syncfusion.Blazor.Toolkit.Popups
        /// <SfDialog Width="500px" @bind-Visible="Visibility" EnableResize="true" Resizing="OnResizingHandler">
        ///   <DialogTemplates>
        ///     <Content>
        ///       <p>Dialog content</p>
        ///     </Content>
        ///   </DialogTemplates>
        /// </SfDialog>
        /// @code {
        ///   private bool Visibility { get; set; } = true;
        ///   private void OnResizingHandler(MouseEventArgs args)
        ///   {
        ///     // Write your code here.
        ///   }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<MouseEventArgs> Resizing { get; set; }
    }
}