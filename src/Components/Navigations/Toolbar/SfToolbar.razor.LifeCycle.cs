using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Syncfusion.Blazor.Toolkit
{
    public partial class SfToolbar
    {
        #region Lifecycle

        /// <exclude />
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            ComputeRenderState();
            _localizer = null;
            // Bind once. Replacing the callback on every parameter set gives SfButton a new OnClick during
            // the render that opens the menu, and the second click can miss the handler.
            if (!_overflowToggleBound)
            {
                _overflowToggleCallback = EventCallback.Factory.Create<MouseEventArgs>(this, ToggleAsync);
                _overflowToggleBound = true;
            }
            DetectLayoutParameterChange();
        }

        /// <exclude />
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender).ConfigureAwait(true);
            // Items nested in groups register after the first pass, so settle the single tab stop once everything has rendered.
            RefreshTabStops();
            await StartScrollObservingAsync().ConfigureAwait(true);
            await ApplyPopupPlacementAsync().ConfigureAwait(true);
            await ApplyPendingFocusAsync().ConfigureAwait(true);
        }

        #endregion
    }
}
