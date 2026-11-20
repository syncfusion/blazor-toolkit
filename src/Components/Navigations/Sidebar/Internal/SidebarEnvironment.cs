using Microsoft.JSInterop;

namespace Syncfusion.Blazor.Toolkit.Internal
{
    /// <summary>
    /// The viewport, motion and direction state that <c>sidebar.js</c> reports for one <see cref="SfSidebar"/>.
    /// </summary>
    /// <exclude />
    internal sealed class SidebarEnvironment
    {
        /// <summary>
        /// Gets or sets a value indicating whether the viewport is below the active breakpoint.
        /// </summary>
        public bool IsMobile { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user prefers reduced motion.
        /// </summary>
        public bool ReducedMotion { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the layout direction of the host is right-to-left.
        /// </summary>
        public bool IsRtl { get; set; }
    }

    /// <summary>
    /// Receives the notifications of <c>sidebar.js</c> and forwards them to the owning <see cref="SfSidebar"/>.
    /// </summary>
    /// <remarks>
    /// A separate type keeps the JavaScript-invokable members off the public surface of <see cref="SfSidebar"/>.
    /// </remarks>
    /// <exclude />
    internal sealed class SidebarCallbacks
    {
        private readonly SfSidebar _owner;

        /// <summary>
        /// Initializes a new instance of the <see cref="SidebarCallbacks"/> class.
        /// </summary>
        /// <param name="owner">The sidebar that receives the notifications.</param>
        internal SidebarCallbacks(SfSidebar owner)
        {
            _owner = owner;
        }

        /// <summary>
        /// Reports that the viewport crossed the active breakpoint.
        /// </summary>
        /// <param name="isMobile"><see langword="true"/> when the viewport is now below the breakpoint.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [JSInvokable]
        public Task OnViewportChanged(bool isMobile)
        {
            return _owner.ApplyViewportAsync(isMobile);
        }

        /// <summary>
        /// Reports that the reduced-motion preference or the layout direction changed.
        /// </summary>
        /// <param name="reducedMotion"><see langword="true"/> when the user prefers reduced motion.</param>
        /// <param name="isRtl"><see langword="true"/> when the host direction is right-to-left.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [JSInvokable]
        public Task OnEnvironmentChanged(bool reducedMotion, bool isRtl)
        {
            return _owner.ApplyEnvironmentAsync(reducedMotion, isRtl);
        }
    }
}
