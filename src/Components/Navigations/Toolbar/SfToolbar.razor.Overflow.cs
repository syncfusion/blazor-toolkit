using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Syncfusion.Blazor.Toolkit
{
    public partial class SfToolbar
    {
        #region Constants

        private const string ModulePath = "./_content/Syncfusion.Blazor.Toolkit/scripts/toolbar.js";
        private const string DefaultOverflowIcon = "e-icons e-more-horizontal-1";
        private const string ExtendedCollapseIcon = "e-icons e-chevron-up";
        private const string PopupRootClass = "e-toolbar-overflow-popup";
        private const string PopupTopClass = "e-toolbar-overflow-popup-top";
        private const string PopupIdPrefix = "sf-toolbar-popup-";
        private const int DebounceMilliseconds = 16;

        /// <summary>
        /// The distance one repeat step moves the toolbar, in CSS pixels.
        /// </summary>
        private const int ScrollStepDistance = 40;

        /// <summary>
        /// The delay before the held chevron starts repeating, in milliseconds.
        /// </summary>
        private const int ScrollRepeatDelay = 350;

        /// <summary>
        /// The interval between two repeat steps, in milliseconds.
        /// </summary>
        private const int ScrollRepeatInterval = 60;

        #endregion

        #region Fields

        private readonly string _popupId = PopupIdPrefix + Guid.NewGuid().ToString("N")[..12];
        private readonly List<OverflowEntry> _entries = [];
        private ElementReference _contentElement;
        private ElementReference _popupElement;
        private ElementReference _overflowContainer;
        private SfButton? _overflowButton;
        private EventCallback<MouseEventArgs> _overflowToggleCallback;
        private bool _overflowToggleBound;
        private IJSObjectReference? _toolbarModule;
        private IJSInProcessObjectReference? _toolbarInProcessModule;
        private DotNetObjectReference<SfToolbar>? _selfReference;
        private CancellationTokenSource? _debounce;
        private bool _observing;
        private bool _scrollObserving;
        private bool _hasOverflow;
        private bool _popupOpen;
        private bool _popupPositioned;

        /// <summary>
        /// Whether the overflow control currently holds the single tab stop of the toolbar.
        /// </summary>
        private bool OverflowButtonFocused { get; set; }

        /// <summary>
        /// The tab index the overflow control last rendered with, so the toolbar only renders again when it changes.
        /// </summary>
        private int OverflowButtonTabIndex { get; set; } = -1;
        private bool _extendedExpanded;
        private bool _focusPopupOnRender;
        private int _focusAttempts;
        private const int MaxFocusAttempts = 5;
        private bool _focusButtonOnRender;
        private int _popupFocusIndex;
        private bool _popupAbove;
        private bool _canScroll;
        private bool _atScrollStart = true;
        private bool _atScrollEnd = true;
        private Timer? _scrollRepeatTimer;
        private int _scrollRepeatDirection;
        private ToolbarOverflowMode _lastMode = ToolbarOverflowMode.Popup;
        private ToolbarOrientation _lastOrientation = ToolbarOrientation.Horizontal;

        #endregion

        #region Overflow state

        private bool IsPopupMode => OverflowMode == ToolbarOverflowMode.Popup;

        private bool IsExtendedMode => OverflowMode == ToolbarOverflowMode.Extended;

        private ToolbarOverflowSettings Settings => OverflowSettings ?? new ToolbarOverflowSettings();

        /// <summary>
        /// Gets whether the overflow control is rendered.
        /// </summary>
        /// <remarks>
        /// In Popup mode the control appears only when items have overflowed. In Extended mode there is no
        /// measurement, because the mode wraps instead of moving items out, so the control is the expand
        /// toggle and is always rendered (TB-OVF-140).
        /// </remarks>
        private bool ShowOverflowButton => Settings.ShowOverflowButton && (IsPopupMode ? _hasOverflow : IsExtendedMode);

        /// <summary>
        /// Gets whether the overflow menu is rendered on the fixed layer outside the toolbar root.
        /// </summary>
        private bool ShowOverflowPopup => ShowOverflowButton && _popupOpen && !IsExtendedMode && _entries.Count > 0;

        private string OverflowIconCss
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Settings.OverflowButtonIconCss))
                {
                    return Settings.OverflowButtonIconCss;
                }
                // In Extended mode the control expands the toolbar, so the collapsed state shows the
                // overflow glyph and the expanded state shows the glyph that collapses it again.
                return IsExtendedMode && _extendedExpanded ? ExtendedCollapseIcon : DefaultOverflowIcon;
            }
        }

        private string OverflowLabel => ResolveBuiltInString(Settings.OverflowButtonTooltip, "Toolbar_Overflow", "More");

        private string ExpandLabel => ResolveBuiltInString(null, _extendedExpanded ? "Toolbar_Collapse" : "Toolbar_Expand", _extendedExpanded ? "Collapse" : "Expand");

        private string ResolveBuiltInString(string? explicitValue, string key, string fallback)
        {
            if (!string.IsNullOrWhiteSpace(explicitValue) && explicitValue != "More")
            {
                return explicitValue;
            }
            IStringLocalizer? resolved = LocalizerFactory?.Create(typeof(SfToolbar));
            _localizer = resolved;
            if (_localizer is null)
            {
                return fallback;
            }
            LocalizedString localized = _localizer[key];
            return localized.ResourceNotFound || string.IsNullOrEmpty(localized.Value) ? fallback : localized.Value;
        }

        private string PopupClass
        {
            get
            {
                string classes = string.IsNullOrWhiteSpace(Settings.PopupCssClass) ? PopupRootClass : PopupRootClass + " " + Settings.PopupCssClass;
                // The popup opens above the trigger when there is no room below it, so the class tells
                // the stylesheet which side to anchor it to.
                return _popupAbove ? classes + " " + PopupTopClass : classes;
            }
        }

        /// <summary>
        /// Gets the attributes of the overflow control.
        /// </summary>
        /// <remarks>
        /// In Popup mode the control opens a menu, so it carries the menu semantics and the
        /// <c>toolbar-overflow-button</c> test id. In Extended mode the control expands the toolbar and
        /// opens no menu, so it carries the expand semantics and the <c>toolbar-expand-button</c> test id
        /// (TB-OVF-140).
        /// </remarks>
        private Dictionary<string, object> OverflowButtonAttributes
        {
            get
            {
                // The same dictionary is returned on every read. A new dictionary on each read gives SfButton a new
                // @attributes splat during the render that opens the menu, and the second click then misses OnClick.
                Dictionary<string, object> attributes = OverflowButtonAttributeStore;
                attributes["aria-expanded"] = IsExtendedMode ? (_extendedExpanded ? "true" : "false") : (_popupOpen ? "true" : "false");
                attributes["aria-label"] = IsExtendedMode ? ExpandLabel : OverflowLabel;
                attributes["title"] = IsExtendedMode ? ExpandLabel : OverflowLabel;
                attributes["tabindex"] = ((IToolbarHost)this).GetOverflowButtonTabIndex().ToString(System.Globalization.CultureInfo.InvariantCulture);
                attributes["data-testid"] = IsExtendedMode ? "toolbar-expand-button" : "toolbar-overflow-button";
                attributes["onkeyup"] = EventCallback.Factory.Create<KeyboardEventArgs>(this, HandleOverflowKeyUpAsync);
                attributes["onkeydown"] = EventCallback.Factory.Create<KeyboardEventArgs>(this, HandleOverflowKeyDownAsync);
                if (!IsExtendedMode)
                {
                    attributes["aria-haspopup"] = "menu";
                    attributes["aria-controls"] = _popupOpen ? _popupId : string.Empty;
                }
                else
                {
                    _ = attributes.Remove("aria-haspopup");
                    _ = attributes.Remove("aria-controls");
                }

                return attributes;
            }
        }

        private Dictionary<string, object> OverflowButtonAttributeStore { get; } = [];

        #endregion

        #region Module lifecycle

        /// <exclude />
        internal override async Task ImportComponentModuleAsync()
        {
            await base.ImportComponentModuleAsync().ConfigureAwait(true);
            if (!IsPopupMode && !IsScrollableMode)
            {
                return;
            }
            await EnsureModuleAsync().ConfigureAwait(true);
        }

        private async Task EnsureModuleAsync()
        {
            JsModuleReference reference = await ImportModuleAsync(ModulePath, _toolbarModule, _toolbarInProcessModule).ConfigureAwait(true);
            _toolbarModule = reference.AsyncRef;
            _toolbarInProcessModule = reference.InProcessRef;
        }

        /// <exclude />
        internal override async Task OnAfterScriptRenderedAsync()
        {
            await base.OnAfterScriptRenderedAsync().ConfigureAwait(true);
            await StartObservingAsync().ConfigureAwait(true);
            await RecalculateAsync().ConfigureAwait(true);
            await UpdateScrollStateAsync().ConfigureAwait(true);
        }

        private async Task StartObservingAsync()
        {
            if (_observing || IsDisposed || !IsPopupMode || (_toolbarModule is null && _toolbarInProcessModule is null))
            {
                return;
            }
            _selfReference ??= DotNetObjectReference.Create(this);
            _observing = true;
            await GuardedAsync(() => InvokeVoidAsync(_toolbarModule, _toolbarInProcessModule, "observe", _contentElement, _selfReference)).ConfigureAwait(true);
        }

        private async Task StopObservingAsync()
        {
            if (!_observing)
            {
                return;
            }
            _observing = false;
            await GuardedAsync(() => InvokeVoidAsync(_toolbarModule, _toolbarInProcessModule, "unobserve", _contentElement)).ConfigureAwait(true);
        }

        /// <summary>
        /// Starts listening for scroll events, so the navigation chevrons follow the position of a
        /// scrollable toolbar.
        /// </summary>
        private async Task StartScrollObservingAsync()
        {
            if (_scrollObserving || IsDisposed || !IsScrollableMode || (_toolbarModule is null && _toolbarInProcessModule is null))
            {
                return;
            }
            _selfReference ??= DotNetObjectReference.Create(this);
            _scrollObserving = true;
            await GuardedAsync(async () => await InvokeVoidAsync(_toolbarModule, _toolbarInProcessModule, "observeScroll", _contentElement, _selfReference).ConfigureAwait(true)).ConfigureAwait(true);
        }

        /// <summary>
        /// Called by the browser when a scrollable toolbar is scrolled, by the pointer, the wheel or a
        /// touch swipe. It updates the state of the two navigation chevrons.
        /// </summary>
        /// <returns>A task that completes when the state has been refreshed.</returns>
        [JSInvokable]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public Task OnScrolledAsync()
        {
            return IsDisposed ? Task.CompletedTask : InvokeAsync(UpdateScrollStateAsync);
        }

        /// <summary>
        /// Runs an interop call and swallows the failures that are expected when the circuit or the page is already gone.
        /// </summary>
        private static async Task GuardedAsync(Func<Task> call)
        {
            try
            {
                await call().ConfigureAwait(true);
            }
            catch (JSDisconnectedException)
            {
                // The circuit disconnected before the call completed. Nothing is left to update.
            }
            catch (ObjectDisposedException)
            {
                // The module reference was disposed while the call was in flight.
            }
            catch (JSException)
            {
                // A disposed or disconnected runtime can surface as a generic interop failure. The call is abandoned.
            }
            catch (InvalidOperationException)
            {
                // bUnit reports an unconfigured invocation this way. Placement is best-effort; a missing
                // script must not fail the render that opened or closed the menu.
            }
            catch (TaskCanceledException)
            {
                // The call was cancelled by a disconnect.
            }
        }

        /// <exclude />
        protected override async ValueTask DisposeAsyncCore()
        {
            if (_debounce is not null)
            {
                await _debounce.CancelAsync().ConfigureAwait(true);
                _debounce.Dispose();
                _debounce = null;
            }
            StopScrollRepeat();
            await StopObservingAsync().ConfigureAwait(true);
            await StopPopupTrackingAsync().ConfigureAwait(true);
            if (_toolbarModule is not null)
            {
                await GuardedAsync(async () => await _toolbarModule.DisposeAsync().ConfigureAwait(true)).ConfigureAwait(true);
                _toolbarModule = null;
            }
            _toolbarInProcessModule?.Dispose();
            _toolbarInProcessModule = null;
            _selfReference?.Dispose();
            _selfReference = null;
            _entries.Clear();
            _collection.Clear();
            _tabStop = null;
            _lastFocused = null;
            await base.DisposeAsyncCore().ConfigureAwait(true);
        }

        #endregion

        #region Recalculation

        /// <summary>
        /// Called by the browser when the container size changes. It is a no-op after the toolbar is disposed.
        /// </summary>
        /// <returns>A task that completes when the recalculation has been scheduled.</returns>
        /// <exclude />
        [JSInvokable]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public Task OnContainerResizedAsync()
        {
            ScheduleRecalculation();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Schedules one recalculation. Triggers that arrive inside the debounce window collapse into a single measurement.
        /// </summary>
        private void ScheduleRecalculation()
        {
            // Before the measurement module is ready there is nothing to measure. The first-render hook measures once
            // everything has registered, so scheduling here would only repeat that work.
            if (IsDisposed || !IsPopupMode || (_toolbarModule is null && _toolbarInProcessModule is null))
            {
                return;
            }
            CancellationTokenSource? previous = _debounce;
            if (previous is not null)
            {
                _ = previous.CancelAsync();
                previous.Dispose();
            }
            CancellationTokenSource source = new();
            _debounce = source;
            _ = RunDebouncedAsync(source.Token);
        }

        private async Task RunDebouncedAsync(CancellationToken token)
        {
            try
            {
                await Task.Delay(DebounceMilliseconds, token).ConfigureAwait(true);
            }
            catch (TaskCanceledException)
            {
                return;
            }
            if (token.IsCancellationRequested || IsDisposed)
            {
                return;
            }
            await InvokeAsync(RecalculateAsync).ConfigureAwait(true);
        }

        /// <summary>
        /// Measures the container and the items, runs the overflow algorithm, and applies the result.
        /// </summary>
        private async Task RecalculateAsync()
        {
            if (IsDisposed || !IsPopupMode || (_toolbarModule is null && _toolbarInProcessModule is null))
            {
                return;
            }

            ToolbarMeasurement? measurement = null;
            await GuardedAsync(async () =>
                measurement = await InvokeAsync<ToolbarMeasurement>(_toolbarModule!, _toolbarInProcessModule!, "measure", _contentElement, Orientation == ToolbarOrientation.Vertical).ConfigureAwait(true)).ConfigureAwait(true);

            if (IsDisposed || measurement is null)
            {
                return;
            }
            ApplyMeasurement(measurement);
        }

        private void ApplyMeasurement(ToolbarMeasurement measurement)
        {
            List<IToolbarItem> measurable = _collection.Flatten().FindAll(static i => i.IsMeasurable);
            Dictionary<string, double> sizes = [];
            foreach (ToolbarMeasuredItem measured in measurement.Items)
            {
                _ = sizes.TryAdd(measured.Id, measured.Size);
            }

            // Items that were not measured (not yet rendered) stay in the bar; only measured items can be overflowed.
            List<OverflowCandidate> candidates = [];
            Dictionary<string, IToolbarItem> byId = [];
            foreach (IToolbarItem item in measurable)
            {
                if (sizes.TryGetValue(item.ItemId, out double size) && byId.TryAdd(item.ItemId, item))
                {
                    candidates.Add(new OverflowCandidate(item.ItemId, size, item.OverflowOption, item.OverflowPriority));
                }
            }

            // Measured sizes include items already overflowed (hidden items measure 0 wide), so use the item natural size
            // from the last time it was in the bar. A hidden item reports 0 and would always fit, so keep the cached size.
            for (int i = 0; i < candidates.Count; i++)
            {
                OverflowCandidate candidate = candidates[i];
                if (candidate.Size <= 0 && _naturalSizes.TryGetValue(candidate.Id, out double known))
                {
                    candidates[i] = candidate with { Size = known };
                }
                else if (candidate.Size > 0)
                {
                    _naturalSizes[candidate.Id] = candidate.Size;
                }
            }

            OverflowResult result = OverflowCalculator.Calculate(candidates, measurement.Available, measurement.Gap, measurement.ButtonSize > 0 ? measurement.ButtonSize : DefaultButtonSize);
            HashSet<string> overflowedIds = [.. result.Overflowed.Select(static o => o.Id)];

            bool changed = false;
            foreach (KeyValuePair<string, IToolbarItem> pair in byId)
            {
                bool shouldOverflow = overflowedIds.Contains(pair.Key);
                if (pair.Value.IsOverflowed != shouldOverflow)
                {
                    pair.Value.IsOverflowed = shouldOverflow;
                    pair.Value.Refresh();
                    changed = true;
                }
            }

            bool hasOverflow = overflowedIds.Count > 0;
            if (hasOverflow != _hasOverflow)
            {
                _hasOverflow = hasOverflow;
                changed = true;
            }
            if (!hasOverflow && _popupOpen)
            {
                _popupOpen = false;
                changed = true;
                _ = InvokeAsync(RaiseOverflowCloseAsync);
            }

            RebuildEntries(result.Overflowed, byId);
            if (changed || hasOverflow)
            {
                RequestTabStopRefresh();
                StateHasChanged();
            }
        }

        private const double DefaultButtonSize = 32;

        private readonly Dictionary<string, double> _naturalSizes = [];

        #endregion

        #region Popup entries

        private sealed class OverflowEntry
        {
            internal string Id { get; init; } = string.Empty;

            internal int Index { get; init; }

            internal string? Text { get; init; }

            internal string? IconCss { get; init; }

            internal bool Enabled { get; init; }

            internal IToolbarItem Item { get; init; } = default!;

            internal EventCallback<MouseEventArgs> Click { get; init; }

            internal SfButton? Button { get; set; }

            internal Dictionary<string, object> Attributes { get; } = new()
            {
                ["role"] = "menuitem",
                ["tabindex"] = "-1",
                ["data-testid"] = "toolbar-overflow-item"
            };

            internal void SyncAttributes(int focusIndex)
            {
                Attributes["tabindex"] = Index == focusIndex ? "0" : "-1";
                Attributes["data-item-id"] = Id;
            }
        }

        private void RebuildEntries(IReadOnlyList<OverflowCandidate> overflowed, Dictionary<string, IToolbarItem> byId)
        {
            // The popup buttons are keyed by id, so a button outlives the entry that captured it. Carry it over, or a
            // recalculation that lands between a render and its focus request leaves the new entry without a button.
            Dictionary<string, SfButton> buttons = [];
            foreach (OverflowEntry previous in _entries)
            {
                if (previous.Button is not null)
                {
                    buttons[previous.Id] = previous.Button;
                }
            }
            _entries.Clear();
            int index = 0;
            foreach (OverflowCandidate candidate in overflowed)
            {
                if (!byId.TryGetValue(candidate.Id, out IToolbarItem? item))
                {
                    continue;
                }
                IToolbarItem captured = item;
                OverflowEntry created = new()
                {
                    Id = candidate.Id,
                    Index = index++,
                    Text = captured.DisplayText,
                    IconCss = captured.DisplayIconCss,
                    Enabled = captured.IsEnabled,
                    Item = captured,
                    Click = EventCallback.Factory.Create<MouseEventArgs>(this, async args =>
                    {
                        if (captured.Component is ToolbarItem templated && templated.Template is not null)
                        {
                            templated.MarkOverflowContext(true);
                            StateHasChanged();
                        }
                        await ActivateEntryAsync(captured, args).ConfigureAwait(true);
                    }),
                    Button = buttons.GetValueOrDefault(candidate.Id)
                };
                created.SyncAttributes(_popupFocusIndex);
                _entries.Add(created);
            }
            if (_popupFocusIndex >= _entries.Count)
            {
                _popupFocusIndex = 0;
            }
        }

        private async Task ActivateEntryAsync(IToolbarItem item, MouseEventArgs args)
        {
            if (IsDisposed || !item.IsEnabled)
            {
                return;
            }
            await item.ActivateAsync(args).ConfigureAwait(true);
            if (Settings.CloseOnSelect)
            {
                await ClosePopupAsync(restoreFocus: true).ConfigureAwait(true);
            }
        }

        #endregion

        #region Popup open and close

        private async Task ToggleAsync(MouseEventArgs args)
        {
            if (IsDisposed)
            {
                return;
            }
            // In Extended mode the overflow control is the expand toggle: it reveals the extra rows
            // instead of opening a menu, because the mode never moves items out of the bar (TB-OVF-140).
            if (IsExtendedMode)
            {
                await ToggleExtendedAsync().ConfigureAwait(true);
                return;
            }
            if (_popupOpen)
            {
                await ClosePopupAsync(restoreFocus: true).ConfigureAwait(true);
            }
            else if (_entries.Count == 0 && !_hasOverflow)
            {
                return;
            }
            else
            {
                _popupOpen = true;
                _popupPositioned = false;
                _popupFocusIndex = 0;
                _focusPopupOnRender = true;
                if (_entries.Count == 0)
                {
                    RebuildEntries(
                        [.. _collection.Flatten()
                            .Where(static item => item.IsOverflowed)
                            .Select(static item => new OverflowCandidate(item.ItemId, 1, item.OverflowOption, item.OverflowPriority))],
                        _collection.Flatten().ToDictionary(static item => item.ItemId));
                }
                StateHasChanged();
                await RaiseOverflowOpenAsync().ConfigureAwait(true);
            }
        }

        private Task ToggleExtendedAsync()
        {
            if (IsDisposed)
            {
                return Task.CompletedTask;
            }
            _extendedExpanded = !_extendedExpanded;
            ComputeRenderState();
            StateHasChanged();
            return Task.CompletedTask;
        }

        private async Task RaiseOverflowOpenAsync()
        {
            if (!IsDisposed && OnOverflowOpen.HasDelegate)
            {
                await OnOverflowOpen.InvokeAsync().ConfigureAwait(true);
            }
        }

        private async Task RaiseOverflowCloseAsync()
        {
            if (!IsDisposed && OnOverflowClose.HasDelegate)
            {
                await OnOverflowClose.InvokeAsync().ConfigureAwait(true);
            }
        }

        private Task ClosePopupFromBackdropAsync(MouseEventArgs _)
        {
            return ClosePopupAsync(restoreFocus: false);
        }

        private async Task ClosePopupAsync(bool restoreFocus)
        {
            if (!_popupOpen || IsDisposed)
            {
                return;
            }
            _popupOpen = false;
            _popupPositioned = false;
            _focusButtonOnRender = restoreFocus;
            await StopPopupTrackingAsync().ConfigureAwait(true);
            await RaiseOverflowCloseAsync().ConfigureAwait(true);
            StateHasChanged();
        }

        private Task HandleOverflowKeyDownAsync(KeyboardEventArgs args)
        {
            if (IsDisposed)
            {
                return Task.CompletedTask;
            }
            if (_popupOpen)
            {
                return HandlePopupKeyDownAsync(args);
            }
            // The control is the last stop of the roving sequence, so the arrow that moves back towards the
            // start returns the focus to the last item of the bar.
            return IsNavigationKey(args.Key) ? NavigateFromOverflowButtonAsync(args.Key) : Task.CompletedTask;
        }

        /// <summary>
        /// Records that the overflow control took the keyboard focus, so it becomes the single tab stop of
        /// the toolbar and the items give up theirs.
        /// </summary>
        /// <returns>A task that completes when the tab stop has moved.</returns>
        private Task HandleOverflowFocusInAsync()
        {
            if (IsDisposed || IsOverflowButtonFocused)
            {
                return Task.CompletedTask;
            }
            OverflowButtonFocused = true;
            RefreshTabStops();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Gets whether the key moves the focus inside the toolbar.
        /// </summary>
        /// <param name="key">The key name from the browser event.</param>
        /// <returns><see langword="true"/> when the key is a navigation key.</returns>
        private static bool IsNavigationKey(string key)
        {
            return key is "ArrowLeft" or "ArrowRight" or "ArrowUp" or "ArrowDown" or "Home" or "End";
        }

        /// <summary>
        /// Moves the focus from the overflow control back into the bar.
        /// </summary>
        /// <param name="key">The key name from the browser event.</param>
        /// <returns>A task that completes when the focus has moved.</returns>
        /// <remarks>
        /// The overflow control is the last stop of the sequence, so the arrow that points back towards the
        /// start of the bar moves the focus to the last item. The arrow that points further to the end
        /// leaves the focus where it is, because there is nothing after the control.
        /// </remarks>
        private async Task NavigateFromOverflowButtonAsync(string key)
        {
            List<IToolbarItem> focusable = _collection.GetFocusable();
            if (focusable.Count == 0)
            {
                return;
            }

            int index;
            if (key == "Home")
            {
                index = 0;
            }
            else if (key == "End")
            {
                index = focusable.Count - 1;
            }
            else
            {
                // A positive step moves the focus towards the end of the bar, a negative step towards its start.
                int step = Orientation == ToolbarOrientation.Vertical
                    ? (key == "ArrowDown" ? 1 : key == "ArrowUp" ? -1 : 0)
                    : (_rtl ? -1 : 1) * (key == "ArrowRight" ? 1 : key == "ArrowLeft" ? -1 : 0);

                if (step >= 0)
                {
                    // Nothing follows the overflow control, so the focus stays.
                    return;
                }
                index = focusable.Count - 1;
            }

            IToolbarItem next = focusable[index];
            _lastFocused = next;
            _tabStop = next;
            OverflowButtonFocused = false;
            RefreshTabStops();
            await next.FocusItemAsync().ConfigureAwait(true);
        }

        /// <summary>
        /// Moves the keyboard focus to the overflow control and makes it the single tab stop of the toolbar.
        /// </summary>
        /// <returns>A task that completes when the focus has moved.</returns>
        private async Task FocusOverflowButtonAsync()
        {
            if (_overflowButton is null)
            {
                return;
            }
            OverflowButtonFocused = true;
            RefreshTabStops();
            await FocusButtonAsync(_overflowButton).ConfigureAwait(true);
        }

        private async Task HandleOverflowKeyUpAsync(KeyboardEventArgs args)
        {
            if (IsDisposed || _popupOpen || args.Key is not ("Enter" or " "))
            {
                return;
            }
            // SfButton has no key handler. A keyup matches the browser click that Enter and Space already synthesize,
            // so the popup opens once instead of opening and immediately closing.
            await ToggleAsync(new MouseEventArgs()).ConfigureAwait(true);
        }

        private async Task HandlePopupKeyDownAsync(KeyboardEventArgs args)
        {
            if (IsDisposed || !_popupOpen)
            {
                return;
            }
            int count = _entries.Count;
            switch (args.Key)
            {
                case "Escape":
                    await ClosePopupAsync(restoreFocus: true).ConfigureAwait(true);
                    return;
                case "ArrowDown":
                    await MovePopupFocusAsync(count == 0 ? 0 : (_popupFocusIndex + 1) % count).ConfigureAwait(true);
                    break;
                case "ArrowUp":
                    await MovePopupFocusAsync(count == 0 ? 0 : (_popupFocusIndex - 1 + count) % count).ConfigureAwait(true);
                    break;
                case "Home":
                    await MovePopupFocusAsync(0).ConfigureAwait(true);
                    break;
                case "End":
                    await MovePopupFocusAsync(Math.Max(0, count - 1)).ConfigureAwait(true);
                    break;
                default:
                    break;
            }
        }

        private async Task MovePopupFocusAsync(int index)
        {
            // Update the tab stop on the existing entries. Rebuilding the attribute dictionary would re-render every
            // button and drop focus before the next arrow key arrives.
            _popupFocusIndex = index;
            foreach (OverflowEntry entry in _entries)
            {
                entry.SyncAttributes(index);
            }
            _ = await FocusEntryAsync(index).ConfigureAwait(true);
        }

        private async Task<bool> FocusEntryAsync(int index)
        {
            if (index >= 0 && index < _entries.Count && _entries[index].Button is { } button)
            {
                await FocusButtonAsync(button).ConfigureAwait(true);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Moves focus and ignores a runtime that cannot move it. bUnit has no focus handler unless a test sets one
        /// up, and an unhandled failure here aborts the render that opened or closed the menu.
        /// </summary>
        private static async Task FocusButtonAsync(SfButton button)
        {
            try
            {
                await button.FocusElementAsync().ConfigureAwait(true);
            }
            catch (JSException)
            {
                // The runtime cannot move focus. The menu state is unchanged.
            }
            catch (InvalidOperationException)
            {
                // The test runtime has no focus handler. The menu state is unchanged.
            }
        }

        private async Task ApplyPendingFocusAsync()
        {
            if (_focusPopupOnRender)
            {
                // The entry buttons are bound after the popup renders. If one is not bound yet, keep the request and try
                // again after the next render. The attempts are bounded so a popup that never renders cannot loop.
                bool focused = _popupOpen && await FocusEntryAsync(_popupFocusIndex).ConfigureAwait(true);
                if (focused || !_popupOpen || ++_focusAttempts >= MaxFocusAttempts)
                {
                    _focusPopupOnRender = false;
                    _focusAttempts = 0;
                }
                else
                {
                    StateHasChanged();
                }
            }
            if (_focusButtonOnRender)
            {
                _focusButtonOnRender = false;
                if (_overflowButton is not null)
                {
                    await FocusButtonAsync(_overflowButton).ConfigureAwait(true);
                }
            }
        }

        /// <summary>
        /// Positions the overflow menu against the overflow button in viewport coordinates.
        /// </summary>
        /// <remarks>
        /// The menu is a <c>position: fixed</c> layer inside the toolbar root (ADR-006), so an ancestor
        /// with <c>overflow: auto</c> or <c>overflow: hidden</c> does not clip it. The placement
        /// module reports and applies coordinates through CSSOM, avoiding a dynamic inline style
        /// attribute. This method tracks the reported side and reveals the layer after placement.
        /// The module is not available in Static SSR, so the menu remains unpositioned there (TB-LIF-020).
        /// An ancestor with <c>transform</c>, <c>filter</c> or <c>contain</c> still contains a fixed
        /// element. Escaping that requires moving the node, which this component does not do.
        /// </remarks>
        private async Task ApplyPopupPlacementAsync()
        {
            if (!_popupOpen || IsDisposed || !IsPopupMode || (_toolbarModule is null && _toolbarInProcessModule is null))
            {
                return;
            }

            ToolbarPopupPlacement? placement = null;
            await GuardedAsync(async () =>
            {
                // The first open measures a zero-height box if placement runs in the same turn as the
                // render that created the popup. Wait one frame so the entries have a real height.
                // Re-check after the yield: a click that closed the menu must not measure a removed node
                // and must not write a style that renders the menu again.
                await Task.Yield();
                if (!_popupOpen || IsDisposed)
                {
                    return;
                }
                placement = await InvokeAsync<ToolbarPopupPlacement>(_toolbarModule!, _toolbarInProcessModule!, "placeOverflowPopup", _popupElement, _overflowContainer).ConfigureAwait(true);
            }).ConfigureAwait(true);

            if (placement is null || IsDisposed || !_popupOpen)
            {
                return;
            }

            bool firstPlacement = !_popupPositioned;
            bool above = !placement.Below;
            bool stateChanged = firstPlacement || above != _popupAbove;
            if (firstPlacement)
            {
                _popupPositioned = true;
                await StartPopupTrackingAsync().ConfigureAwait(true);
            }

            // The flag records that the popup opens above, so it is the opposite of the reported placement.
            if (above != _popupAbove)
            {
                _popupAbove = above;
            }
            if (stateChanged)
            {
                StateHasChanged();
            }
        }

        /// <summary>
        /// Called by the browser when the page or an ancestor scrolls, or when the viewport resizes,
        /// while the overflow menu is open. The menu is repositioned against the overflow button.
        /// </summary>
        /// <returns>A task that completes when the reposition has been scheduled.</returns>
        /// <exclude />
        [JSInvokable]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public Task OnPopupAnchorMovedAsync()
        {
            return IsDisposed || !_popupOpen ? Task.CompletedTask : InvokeAsync(ApplyPopupPlacementAsync);
        }

        private async Task StartPopupTrackingAsync()
        {
            if (IsDisposed || (_toolbarModule is null && _toolbarInProcessModule is null))
            {
                return;
            }

            _selfReference ??= DotNetObjectReference.Create(this);
            await GuardedAsync(() => InvokeVoidAsync(_toolbarModule, _toolbarInProcessModule, "trackPopup", _overflowContainer, _selfReference)).ConfigureAwait(true);
        }

        private async Task StopPopupTrackingAsync()
        {
            if (_toolbarModule is null && _toolbarInProcessModule is null)
            {
                return;
            }

            // The anchor may already be gone when the toolbar disposes. The script treats a missing
            // anchor as a no-op, so disposal still clears the listener table.
            await GuardedAsync(() => InvokeVoidAsync(_toolbarModule, _toolbarInProcessModule, "untrackPopup", _overflowContainer)).ConfigureAwait(true);
        }

        /// <summary>
        /// Decides whether the nested menu of an item opens above or below the item.
        /// </summary>
        /// <param name="menu">The nested menu element.</param>
        /// <param name="anchor">The element the menu is anchored to.</param>
        /// <returns><see langword="true"/> when the menu opens above the item; otherwise <see langword="false"/>.</returns>
        /// <remarks>
        /// The nested menu is an inline element inside the item (ADR-006), so only the side it opens on
        /// changes. The measurement module is not available in Static SSR, so the menu keeps its default
        /// position there (TB-LIF-020).
        /// </remarks>
        async Task<bool> IToolbarHost.PlaceNestedMenuAsync(ElementReference menu, ElementReference anchor)
        {
            if (IsDisposed || (_toolbarModule is null && _toolbarInProcessModule is null))
            {
                return false;
            }

            ToolbarPopupPlacement? placement = null;
            await GuardedAsync(async () =>
            {
                await Task.Yield();
                placement = await InvokeAsync<ToolbarPopupPlacement>(_toolbarModule!, _toolbarInProcessModule!, "placePopup", menu, anchor).ConfigureAwait(true);
            }).ConfigureAwait(true);

            return placement is not null && !placement.Below;
        }

        #endregion

        #region Parameter-change triggers

        private void DetectLayoutParameterChange()
        {
            if (_lastMode != OverflowMode || _lastOrientation != Orientation)
            {
                bool leavingPopup = _lastMode == ToolbarOverflowMode.Popup && !IsPopupMode;
                bool leavingExtended = _lastMode == ToolbarOverflowMode.Extended && !IsExtendedMode;
                bool enteringScrollable = _lastMode != ToolbarOverflowMode.Scrollable && IsScrollableMode;
                _lastMode = OverflowMode;
                _lastOrientation = Orientation;
                if (leavingExtended)
                {
                    // The expanded rows are collapsed again when the toolbar leaves the Extended mode,
                    // so the next mode starts from its own default state.
                    _extendedExpanded = false;
                    ComputeRenderState();
                }
                if (leavingPopup)
                {
                    ClearOverflow();
                }
                else
                {
                    ScheduleRecalculation();
                }
                if (enteringScrollable)
                {
                    // The chevrons depend on the measured scroll range, so the state is read once the
                    // mode is active.
                    _canScroll = false;
                    _atScrollStart = true;
                    _atScrollEnd = true;
                    _ = InvokeAsync(UpdateScrollStateAsync);
                }
            }
        }

        private void ClearOverflow()
        {
            foreach (IToolbarItem item in _collection.Flatten())
            {
                if (item.IsOverflowed)
                {
                    item.IsOverflowed = false;
                    item.Refresh();
                }
            }
            _entries.Clear();
            _hasOverflow = false;
            if (_popupOpen)
            {
                _popupOpen = false;
                _ = InvokeAsync(RaiseOverflowCloseAsync);
            }
        }

        #endregion
    }
}
