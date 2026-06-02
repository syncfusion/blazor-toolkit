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
