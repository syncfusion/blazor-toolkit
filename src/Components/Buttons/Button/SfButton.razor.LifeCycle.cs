namespace Syncfusion.Blazor.Toolkit.Buttons
{
    public partial class SfButton
    {
        #region LifeCycle Methods

        /// <exclude />
        /// <summary>
        /// Handles parameter updates and recomputes render-time state.
        /// </summary>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(false);
            InitRender();
        }

        /// <exclude />
        /// <summary>
        /// Executes after component render.
        /// </summary>
        /// <param name="firstRender">Indicates whether this is the first render.</param>
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
