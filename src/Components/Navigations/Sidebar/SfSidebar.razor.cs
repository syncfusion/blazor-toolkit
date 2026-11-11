using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Toolkit.Internal;

namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>
    /// The Blazor Sidebar is a collapsible panel that docks to the left or right edge of a container and shows navigation or contextual content
    /// over, beside, or instead of the main content.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The sidebar can be driven declaratively with <c>@bind-IsOpen</c> or imperatively through <see cref="OpenAsync"/>, <see cref="CloseAsync"/> and <see cref="ToggleAsync"/>.
    /// Every state change requested by the user, a method, a navigation or a breakpoint crossing applies the new state, raises <see cref="IsOpenChanged"/>, then raises
    /// <see cref="OnOpened"/> or <see cref="OnClosed"/> once the <see cref="Duration"/> has elapsed (immediately when animation is off), and is announced to assistive technology through a polite live region.
    /// A parent-driven <see cref="IsOpen"/> change raises neither event.
    /// </para>
    /// <para>
    /// While a modal overlay is open (<see cref="SidebarMode.Overlay"/> with <see cref="ShowBackdrop"/>), the sidebar is exposed as <c>role="dialog"</c> with <c>aria-modal="true"</c>,
    /// moves focus to its first focusable element, keeps Tab and Shift+Tab inside the panel, makes the target content inert, and returns focus to the opener when it closes.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// <SfSidebar @bind-IsOpen="_isOpen" Mode="SidebarMode.Push" Width="260px">
    ///     <HeaderTemplate>Menu</HeaderTemplate>
    ///     <ChildContent><nav>...</nav></ChildContent>
    ///     <TargetContent><main>...</main></TargetContent>
    /// </SfSidebar>
    /// ]]></code>
    /// </example>
    public partial class SfSidebar : SfBaseComponent, IDisposable
    {
        #region Internal variables
        private bool _isOpen;
        private bool _isOpenParameter;
        private bool _isMobile;
        private bool _reducedMotion;
        private bool _hostRtl;
        private bool _isHovering;
        private bool _modalActive;
        private bool _locationSubscribed;
        private int _observedThreshold = -1;
        private CancellationTokenSource? _transitionCts;
        private DotNetObjectReference<SidebarCallbacks>? _callbacksReference;
        private double? _pointerStartX;
        private double? _pointerStartY;
        private string _announcement = string.Empty;
        private string _containerClass = CONTAINER;
        private string _containerStyle = string.Empty;
        private string _sidebarClass = SIDEBAR;
        private string _role = ROLE_NAVIGATION;
        private string? _ariaModal;
        private string? _ariaHidden;
        private string? _ariaLabel = LABEL_FALLBACK;
        private string? _ariaLabelledBy;
        private string? _ariaDescribedBy;
        private string _notes = string.Empty;
        private string _notesId = string.Empty;
        private string _backdropClass = BACKDROP;
        private Dictionary<string, object>? _hostAttributes;
        private string _panelId = string.Empty;
        private ElementReference _sidebarElement;
        private string? _watchedMediaQuery;
        private IJSObjectReference? _sidebarJsModule;
        private IJSObjectReference? _mediaQuerySubscription;
        private Dictionary<string, object>? _htmlAttributes;
        #endregion

        /// <exclude />
        internal bool IsModal => ShowBackdrop && _isOpen && EffectiveMode == SidebarMode.Overlay;

        /// <exclude />
        internal SidebarMode EffectiveMode => Responsive && _isMobile ? SidebarMode.Overlay : Mode;

        private bool IsRtl => (SyncfusionService?._options.EnableRtl ?? false) || _hostRtl;

        private bool IsGloballyAnimated => EnableAnimation && (SyncfusionService?.IsAnimationEnabled() ?? true);

        private bool DelaysEvents => IsGloballyAnimated && !_reducedMotion && Duration > 0;

        private bool HasLegacyMediaQuery => !string.IsNullOrWhiteSpace(MediaQuery);

        private int Threshold => Responsive ? SidebarLayout.GetBreakpointThreshold(Breakpoint) : 0;

        private bool IsSidebarFocusable => IsRendered && (_isOpen || EnableMini);

        private string GetText(string key, string fallback)
        {
            LocalizedString text = Localizer[key];
            return text.ResourceNotFound || string.IsNullOrEmpty(text.Value) ? fallback : text.Value;
        }

        private void Announce()
        {
            _announcement = _isOpen ? GetText(OPENED_KEY, OPENED_FALLBACK) : GetText(CLOSED_KEY, CLOSED_FALLBACK);
        }

        private void UpdateDerivedState()
        {
            bool toRight = SidebarLayout.ResolveToRight(Position, IsRtl);
            string width = SidebarLayout.SanitizeLength(Width, DEFAULT_WIDTH);
            string miniWidth = SidebarLayout.SanitizeLength(MiniWidth, DEFAULT_MINI_WIDTH);
            bool isMini = EnableMini && !_isOpen;
            bool hoverApplicable = OpenMiniOnHover && isMini && IsEnabled && !_isMobile;
            if (!hoverApplicable)
            {
                _isHovering = false;
            }

            string container = SfBaseUtils.AddClass(CONTAINER, toRight ? CONTAINER_RIGHT : CONTAINER_LEFT);
            string sidebar = SfBaseUtils.AddClass(SIDEBAR, toRight ? RIGHT : LEFT);

            (string containerMode, string sidebarMode) = EffectiveMode switch
            {
                SidebarMode.Push => (CONTAINER_PUSH, PUSH),
                SidebarMode.Shrink => (CONTAINER_SHRINK, SHRINK),
                SidebarMode.Overlay => (CONTAINER_OVERLAY, OVERLAY),
                _ => (CONTAINER_OVERLAY, OVERLAY),
            };
            container = SfBaseUtils.AddClass(container, containerMode);
            sidebar = SfBaseUtils.AddClass(sidebar, sidebarMode);

            if (_isOpen)
            {
                container = SfBaseUtils.AddClass(container, CONTAINER_OPEN);
                sidebar = SfBaseUtils.AddClass(sidebar, OPEN);
            }

            if (isMini)
            {
                container = SfBaseUtils.AddClass(container, CONTAINER_MINI);
                sidebar = SfBaseUtils.AddClass(sidebar, MINI);
            }

            if (_isHovering)
            {
                sidebar = SfBaseUtils.AddClass(sidebar, HOVER);
            }

            if (Fixed)
            {
                container = SfBaseUtils.AddClass(container, CONTAINER_FIXED);
            }

            if (IsRtl)
            {
                container = SfBaseUtils.AddClass(container, RTL);
            }

            if (!IsGloballyAnimated)
            {
                container = SfBaseUtils.AddClass(container, NO_ANIMATION);
            }

            _containerClass = container;
            _sidebarClass = SfBaseUtils.AddClass(sidebar, CssClass);
            _backdropClass = SfBaseUtils.AddClass(BACKDROP, BackdropClass);
            _panelId = string.IsNullOrWhiteSpace(Id) ? _generatedId : Id;
            string occupied = _isOpen || _isHovering ? OCCUPIED_OPEN : isMini ? OCCUPIED_MINI : OCCUPIED_NONE;
            _containerStyle = $"--e-sidebar-width: {width}; --e-sidebar-mini-width: {miniWidth}; --e-sidebar-occupied-width: {occupied}; --e-sidebar-duration: {Math.Max(Duration, 0)}ms;";
            _role = string.IsNullOrWhiteSpace(Role) ? (IsModal ? ROLE_DIALOG : ROLE_NAVIGATION) : Role;
            _ariaModal = IsModal ? "true" : null;
            _ariaHidden = !_isOpen && !EnableMini ? "true" : null;
            _ariaLabelledBy = string.IsNullOrWhiteSpace(AriaLabelledBy) ? null : AriaLabelledBy.Trim();
            _ariaLabel = !string.IsNullOrWhiteSpace(AriaLabel) ? AriaLabel : _ariaLabelledBy is null ? GetText(LABEL_KEY, LABEL_FALLBACK) : null;
            _notesId = _panelId + "-notes";
            _notes = GetText(NOTES_KEY, NOTES_FALLBACK);
            _ariaDescribedBy = _notesId;
            Dictionary<string, object>? panelAttributes = BuildHtmlAttributes(HtmlAttributes, _ariaLabelledBy is not null);
            bool overlay = EffectiveMode == SidebarMode.Overlay;
            if (hoverApplicable)
            {
                panelAttributes ??= new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                panelAttributes["onpointerenter"] = EventCallback.Factory.Create<PointerEventArgs>(this, HandlePointerEnter);
            }

            if (_isHovering)
            {
                panelAttributes ??= new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                panelAttributes["onpointerleave"] = EventCallback.Factory.Create<PointerEventArgs>(this, HandlePointerLeave);
            }

            if (!overlay)
            {
                panelAttributes ??= new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                panelAttributes["onkeydown"] = EventCallback.Factory.Create<KeyboardEventArgs>(this, HandleKeyDownAsync);
            }

            _htmlAttributes = panelAttributes;
            _hostAttributes = overlay
                ? new Dictionary<string, object> { ["onkeydown"] = EventCallback.Factory.Create<KeyboardEventArgs>(this, HandleKeyDownAsync) }
                : null;
        }

        private static Dictionary<string, object>? BuildHtmlAttributes(Dictionary<string, object>? source, bool ownsLabelledBy)
        {
            if (source is null || source.Count == 0)
            {
                return null;
            }

            Dictionary<string, object> safe = new(source.Count, StringComparer.OrdinalIgnoreCase);
            foreach (KeyValuePair<string, object> attribute in source)
            {
                if (string.IsNullOrWhiteSpace(attribute.Key)
                    || _reservedAttributes.Contains(attribute.Key)
                    || (ownsLabelledBy && string.Equals(attribute.Key, ARIA_LABELLEDBY, StringComparison.OrdinalIgnoreCase))
                    || attribute.Key.StartsWith("on", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                safe[attribute.Key] = attribute.Value;
            }

            return safe.Count == 0 ? null : safe;
        }

        private async Task SetOpenStateAsync(bool isOpen, SidebarTrigger trigger)
        {
            if (IsDisposed || _isOpen == isOpen)
            {
                return;
            }

            CancelTransition();
            bool previous = _isOpen;
            _isOpen = isOpen;
            _isHovering = false;
            UpdateDerivedState();
            Announce();
            StateHasChanged();

            _ = await SfBaseUtils.UpdatePropertyAsync(isOpen, previous, IsOpenChanged).ConfigureAwait(true);

            // A handler may have awaited and moved the state again. Report only the change this call applied.
            if (_isOpen != isOpen || IsDisposed)
            {
                return;
            }

            EventCallback<SidebarEventArgs> changed = isOpen ? OnOpened : OnClosed;
            if (!changed.HasDelegate)
            {
                return;
            }

            SidebarEventArgs args = new() { IsOpen = isOpen, Mode = EffectiveMode, Trigger = trigger };
            if (!DelaysEvents)
            {
                await changed.InvokeAsync(args).ConfigureAwait(true);
                return;
            }

            CancellationTokenSource transition = new();
            _transitionCts = transition;
            _ = RaiseAfterTransitionAsync(changed, args, Duration, transition.Token);
        }

        private async Task RaiseAfterTransitionAsync(EventCallback<SidebarEventArgs> callback, SidebarEventArgs args, int duration, CancellationToken token)
        {
            try
            {
                await Task.Delay(duration, token).ConfigureAwait(true);
            }
            catch (OperationCanceledException)
            {
                return;
            }

            await InvokeAsync(() => token.IsCancellationRequested || IsDisposed ? Task.CompletedTask : callback.InvokeAsync(args)).ConfigureAwait(true);
        }

        private void CancelTransition()
        {
            CancellationTokenSource? pending = _transitionCts;
            _transitionCts = null;
            if (pending is null)
            {
                return;
            }

            pending.Cancel();
            pending.Dispose();
        }

        private Task HandleBackdropClickAsync()
        {
            return CloseOnOverlayClick && IsEnabled
                ? SetOpenStateAsync(false, SidebarTrigger.Backdrop)
                : Task.CompletedTask;
        }

        private Task HandleKeyDownAsync(KeyboardEventArgs args)
        {
            return CloseOnEscape && IsEnabled && _isOpen && string.Equals(args.Key, ESCAPE_KEY, StringComparison.Ordinal)
                ? SetOpenStateAsync(false, SidebarTrigger.Keyboard)
                : Task.CompletedTask;
        }

        private void HandlePointerEnter(PointerEventArgs args)
        {
            if (!OpenMiniOnHover || !EnableMini || _isOpen || !IsEnabled || _isMobile
                || string.Equals(args.PointerType, POINTER_TOUCH, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            _isHovering = true;
            UpdateDerivedState();
        }

        private void HandlePointerLeave(PointerEventArgs args)
        {
            if (!_isHovering)
            {
                return;
            }

            _isHovering = false;
            UpdateDerivedState();
        }

        /// <summary>
        /// Applies a viewport change reported by <c>sidebar.js</c>.
        /// </summary>
        /// <param name="isMobile"><see langword="true"/> when the viewport is below the breakpoint.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exclude />
        internal Task ApplyViewportAsync(bool isMobile)
        {
            return ApplyViewportAsync(isMobile, raiseBreakpointChanged: true);
        }

        private Task ApplyViewportAsync(bool isMobile, bool raiseBreakpointChanged)
        {
            return InvokeAsync(async () =>
            {
                if (IsDisposed || Threshold == 0 || _isMobile == isMobile)
                {
                    return;
                }

                _isMobile = isMobile;
                UpdateDerivedState();
                StateHasChanged();
                if (raiseBreakpointChanged && OnBreakpointChanged.HasDelegate)
                {
                    await OnBreakpointChanged.InvokeAsync(isMobile).ConfigureAwait(true);
                }

                if (isMobile && _isOpen && !HasLegacyMediaQuery)
                {
                    await SetOpenStateAsync(false, SidebarTrigger.Api).ConfigureAwait(true);
                }
            });
        }

        /// <summary>
        /// Applies a reduced-motion or direction change reported by <c>sidebar.js</c>.
        /// </summary>
        /// <param name="reducedMotion"><see langword="true"/> when the user prefers reduced motion.</param>
        /// <param name="isRtl"><see langword="true"/> when the host direction is right-to-left.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exclude />
        internal Task ApplyEnvironmentAsync(bool reducedMotion, bool isRtl)
        {
            return InvokeAsync(() =>
            {
                if (IsDisposed || (_reducedMotion == reducedMotion && _hostRtl == isRtl))
                {
                    return;
                }

                _reducedMotion = reducedMotion;
                _hostRtl = isRtl;
                UpdateDerivedState();
                StateHasChanged();
            });
        }

        private void HandlePointerDown(PointerEventArgs args)
        {
            if (!EnableGestures || !IsEnabled || string.Equals(args.PointerType, POINTER_MOUSE, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            _pointerStartX = args.ClientX;
            _pointerStartY = args.ClientY;
        }

        private void HandlePointerCancel()
        {
            _pointerStartX = null;
            _pointerStartY = null;
        }

        private Task HandlePointerUpAsync(PointerEventArgs args)
        {
            if (!EnableGestures || !IsEnabled || !_pointerStartX.HasValue || !_pointerStartY.HasValue)
            {
                return Task.CompletedTask;
            }

            double deltaX = args.ClientX - _pointerStartX.Value;
            double deltaY = args.ClientY - _pointerStartY.Value;
            HandlePointerCancel();

            bool? result = SidebarLayout.GetSwipeResult(SidebarLayout.ResolveToRight(Position, IsRtl), _isOpen, deltaX, deltaY, SWIPE_THRESHOLD);
            return result.HasValue ? SetOpenStateAsync(result.Value, SidebarTrigger.Gesture) : Task.CompletedTask;
        }
    }
}
