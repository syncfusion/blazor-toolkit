namespace Syncfusion.Blazor.Toolkit.Buttons
{
    /// <content>
    /// Lifecycle methods for <see cref="SfButtonGroup"/>.
    /// </content>
    public partial class SfButtonGroup
    {
        #region Fields

        private bool _previousIsVertical;

        #endregion

        #region Lifecycle Methods

        /// <exclude />
        /// <summary>
        /// Executes when component parameters are set.
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            // Trigger re-render if IsVertical property changes
            if (_previousIsVertical != IsVertical)
            {
                _previousIsVertical = IsVertical;
            }
        }

        /// <exclude />
        /// <summary>
        /// Executes after each render.
        /// </summary>
        /// <param name="firstRender">Indicates whether this is the first render.</param>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender).ConfigureAwait(false);
            if (firstRender && Created.HasDelegate)
            {
                await Created.InvokeAsync(null).ConfigureAwait(false);
            }
        }

        /// <exclude />
        /// <summary>
        /// Disposes resources and destroys interop handlers when the component is removed.
        /// </summary>
        protected override ValueTask DisposeAsyncCore()
        {
            _buttonItems?.Clear();
            _buttonItems = null;
            HtmlAttributes.Clear();
            _previousIsVertical = false;
            return base.DisposeAsyncCore();
        }

        #endregion
    }
}
