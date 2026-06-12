using System.Diagnostics;
namespace Syncfusion.Blazor.Toolkit.Buttons
{
    public partial class SfButton
    {
        #region LifeCycle Methods

        /// <exclude />
        /// <summary>
        /// Recomputes CSS classes and layout state when parent parameters change.
        /// </summary>
        /// <remarks>
        /// Calls <see cref="InitRender"/> to regenerate the computed <c>class</c> attribute after any parameter update.
        /// </remarks>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(false);
            InitRender();
            CheckIconOnlyAccessibleName();
        }

        /// <exclude />
        /// <summary>
        /// Checks if the button is icon-only without accessible name and emits a debug warning.
        /// </summary>
        /// <remarks>
        /// An icon-only button should have either <see cref="Content"/> or <see cref="ChildContent"/> set
        /// to provide an accessible name for screen readers.
        /// </remarks>
        [Conditional("DEBUG")]
        private void CheckIconOnlyAccessibleName()
        {
            if (!string.IsNullOrEmpty(IconCss) && string.IsNullOrEmpty(Content) && ChildContent is null)
            {
                Debug.WriteLine("Warning: Icon-only button lacks accessible name. Provide Content or ChildContent for screen readers.");
            }
        }

        /// <exclude />
        /// <summary>
        /// Invokes the <see cref="Created"/> event callback after the component is rendered for the first time.
        /// </summary>
        /// <param name="firstRender">
        /// <see langword="true"/> if this is the first render; otherwise, <see langword="false"/>.
        /// </param>
        /// <remarks>
        /// The <see cref="Created"/> event fires only once, immediately following the initial render.
        /// </remarks>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender).ConfigureAwait(false);
            if (firstRender)
            {
                if (Created.HasDelegate)
                {
                    await Created.InvokeAsync(null).ConfigureAwait(false);
                }
            }
        }

        #endregion
    }
}
