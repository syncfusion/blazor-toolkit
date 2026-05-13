using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Popups
{
    /// <summary>
    /// Represents a collection of <see cref="DialogButton"/> components that are rendered within the <see cref="SfDialog"/> component.
    /// </summary>
    /// <remarks>
    /// The <see cref="DialogButtons"/> component serves as a container for multiple <see cref="DialogButton"/> instances within a dialog.
    /// To generate dynamic <see cref="DialogButton"/> instances based on a collection, use <c>@foreach</c> within the <see cref="DialogButtons"/> tag directive.
    /// Each button in the collection can be configured with different properties such as content, click handlers, and styling options.
    /// </remarks>
    /// <example>
    /// In the following code example, a basic dialog with buttons has been rendered using the tag directive.
    /// <code><![CDATA[
    /// @using Syncfusion.Blazor.Toolkit.Popups
    /// <SfDialog Width="500px" @bind-Visible="Visibility">
    ///   <DialogTemplates>
    ///     <Content>
    ///       <p>
    ///         Dialog content
    ///       </p>
    ///     </Content>
    ///   </DialogTemplates>
    ///   <DialogButtons>
    ///     <DialogButton IsPrimary="true" Content="Ok" OnClick="@OnBtnClick" />
    ///     <DialogButton Content="Cancel" OnClick="@OnBtnClick" />
    ///   </DialogButtons>
    /// </SfDialog>
    /// @code {
    ///   private bool Visibility { get; set; } = true;
    ///   private void OnBtnClick()
    ///   {
    ///     this.Visibility = false;
    ///   }
    /// }
    /// ]]></code>
    /// </example>
    public partial class DialogButtons : SfBaseComponent
    {
        [CascadingParameter]
        internal SfDialog? Parent { get; set; }

        /// <summary>
        /// Gets or sets the content of the <see cref="DialogButtons"/> container, which typically contains one or more <see cref="DialogButton"/> components.
        /// </summary>
        /// <value>
        /// A <see cref="RenderFragment"/> that represents the child content of the dialog buttons container.
        /// </value>
        /// <remarks>
        /// This property serves as the render fragment container for dialog buttons and is used internally by the Blazor component framework.
        /// The content typically consists of <see cref="DialogButton"/> components defined within the <see cref="DialogButtons"/> tag.
        /// </remarks>
        /// <exclude/>
        [Parameter]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public RenderFragment? ChildContent { get; set; }

        internal List<DialogButton> Buttons { get; set; } = [];

        internal int UpdateChildProperty(DialogButton button)
        {
            if (button is not null)
            {
                Buttons.Add(button);
            }
            Parent?.UpdateButtons(Buttons);
            return Buttons.Count - 1;
        }

        internal void Refresh()
        {
            Parent?.Refresh();
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="DialogButtons"/> component and optionally releases the managed resources.
        /// </summary>
        /// <remarks>
        /// When DisposeAsyncCore is called, this method clears the button collection, nullifies the child content,
        /// and removes the reference to the parent dialog component to prevent memory leaks.
        /// </remarks>
        protected override ValueTask DisposeAsyncCore()
        {
            Buttons?.Clear();
            ChildContent = null;
            Parent = null;
            return base.DisposeAsyncCore();
        }
    }
}