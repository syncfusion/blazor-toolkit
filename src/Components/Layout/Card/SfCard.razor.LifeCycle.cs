namespace Syncfusion.Blazor.Toolkit
{
    public partial class SfCard
    {
        #region LifeCycle Methods

        /// <exclude />
        /// <summary>
        /// Resolves the element id once so it stays stable across renders.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(true);
            _generatedId = IdPrefix + GetContextId();
        }

        /// <exclude />
        /// <summary>
        /// Recomputes the cached markup state whenever the parent changes a parameter.
        /// </summary>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(true);
            BuildRenderState();
        }

        #endregion
    }
}
