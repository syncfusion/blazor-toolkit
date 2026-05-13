using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Toolkit.Internal;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    /// <summary>
    /// Represents a Switch that toggles between checked (ON) and unchecked (OFF) states.
    /// Supports keyboard, mouse, and touch interactions, and integrates with form binding.
    /// </summary>
    /// <typeparam name="TChecked">The type of the checked state value, typically bool.</typeparam>
    public partial class SfSwitch<TChecked> : SfSelectionBase<TChecked>
    {
        #region Logging

        private static readonly Action<ILogger, string, Exception> _logDebugGetAriaPressed =
            LoggerMessage.Define<string>(
                LogLevel.Debug,
                new EventId(1, nameof(GetAriaPressed)),
                "GetAriaPressed: unable to convert Checked to boolean: {ExceptionMessage}");

        private static readonly Action<ILogger, string, Exception> _logErrorInitializeSwitch =
            LoggerMessage.Define<string>(
                LogLevel.Error,
                new EventId(2, nameof(OnAfterScriptRenderedAsync)),
                "Error initializing Switch interop in OnAfterScriptRenderedAsync: {ExceptionMessage}");

        private static readonly Action<ILogger, string, Exception> _logErrorDestroySwitch =
            LoggerMessage.Define<string>(
                LogLevel.Error,
                new EventId(3, nameof(DisposeAsyncCore)),
                "Error destroying Switch interop: {ExceptionMessage}");

        #endregion

        #region Constants

        /// <summary>
        /// Single whitespace used to concatenate CSS classes.
        /// </summary>
        private const string Space = " ";

        /// <summary>
        /// CSS class enabling right-to-left layout support.
        /// </summary>
        private const string Rtl = "e-rtl";

        /// <summary>
        /// Keyword representing the checked state token in internal logic.
        /// </summary>
        private const string Check = "check";

        /// <summary>
        /// Keyword representing the unchecked state token in internal logic.
        /// </summary>
        private const string UnCheck = "uncheck";

        /// <summary>
        /// CSS class for the switch track/inner element.
        /// </summary>
        private const string Inner = "e-switch-inner";

        /// <summary>
        /// CSS class for the switch handle element.
        /// </summary>
        private const string Handle = "e-switch-handle";

        /// <summary>
        /// CSS class appended to indicate active/checked state.
        /// </summary>
        private const string Active = " e-switch-active";

        /// <summary>
        /// CSS class applied when the component is disabled.
        /// </summary>
        private const string DisabledClass = "e-switch-disabled";

        /// <summary>
        /// Root CSS classes for the switch wrapper.
        /// </summary>
        private const string Switch = "e-switch-wrapper e-wrapper";

        #endregion

        #region Private Fields

        /// <summary>
        /// Tracks whether a drag or touch gesture is currently in progress.
        /// Used to distinguish between click and swipe interactions.
        /// </summary>
        private bool _isDrag;

        /// <summary>
        /// Computed CSS class string for the root wrapper element.
        /// Combines base classes with state-specific classes (RTL, disabled, etc.).
        /// </summary>
        internal string _rootClass = string.Empty;

        /// <summary>
        /// Computed CSS class string for the switch inner track element.
        /// Includes active state class when checked.
        /// </summary>
        internal string _innerClass = string.Empty;

        /// <summary>
        /// Computed CSS class string for the switch handle (thumb) element.
        /// Includes active state class when checked.
        /// </summary>
        internal string _handleClass = string.Empty;

        /// <summary>
        /// Indicates whether to call preventDefault on touch events.
        /// Used to prevent default browser behavior during toggle gestures.
        /// </summary>
        internal bool _preventDefault;

        /// <summary>
        /// Tracks whether this is the first touch interaction in a gesture sequence.
        /// Used to handle platform-specific quirks in touch event handling.
        /// </summary>
        private bool _isFirstTouch = true;

        /// <summary>
        /// Baseline Y-coordinate captured at touchstart event.
        /// Used to calculate vertical movement distance during swipe gestures.
        /// </summary>
        private double _baseTouchY;

        /// <summary>
        /// Baseline X-coordinate captured at touchstart event.
        /// Used to calculate horizontal movement distance during swipe gestures.
        /// </summary>
        private double _baseTouchX;

        /// <summary>
        /// Indicates whether the current gesture is a vertical scroll rather than a horizontal toggle swipe.
        /// When true, the toggle action is suppressed to allow normal scrolling behavior.
        /// </summary>
        private bool _isScrollSwipe;

        /// <summary>
        /// Gets or sets the optional visible text label rendered next to the switch.
        /// </summary>
        internal string? Label { get; set; }

        /// <summary>
        /// Gets or sets the optional accessible name for the switch when no visible label is supplied.
        /// </summary>
        internal string? AriaLabel { get; set; }

        /// <summary>
        /// Logger instance for diagnostic and error logging.
        /// </summary>
        [Inject]
        protected ILogger<SfSwitch<TChecked>> Logger { get; set; } = null!;

        // JS module references for JS isolation
        private IJSObjectReference? _componentJsModule;
        private IJSInProcessObjectReference? _componentsInProcessModule;

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes CSS classes and input attributes before each render cycle.
        /// Sets up the root element classes based on current state (checked, disabled, RTL).
        /// Configures change event handler for disabled state to prevent unintended mutations.
        /// </summary>
        /// <param name="isDynamic">Reserved for framework use; indicates dynamic rendering scenario.</param>
        protected override void InitRender(bool isDynamic = false)
        {
            // Start with base wrapper classes
            _rootClass = Switch;

            // Apply checked/unchecked state classes
            bool isChecked = Convert.ToBoolean(Checked, CultureInfo.InvariantCulture);
            ChangeState(isChecked ? Check : UnCheck);

            // Append custom CSS classes if provided
            if (!string.IsNullOrEmpty(CssClass))
            {
                _rootClass += Space + CssClass;
            }

            // Handle disabled state
            if (Disabled)
            {
                _rootClass += Space + DisabledClass;

                // Add change handler to prevent state mutations when disabled
                if (_inputAttributes is null || !_inputAttributes.ContainsKey("onchange"))
                {
                    EventCallback<ChangeEventArgs> changeEvent = EventCallback.Factory.Create<ChangeEventArgs>(this, HandleChange);
                    _inputAttributes = SfBaseUtils.UpdateDictionary("onchange", changeEvent, _inputAttributes);
                }
            }
            else
            {
                // Remove disabled class and change handler when not disabled
                _rootClass = _rootClass.Replace(Space + DisabledClass, string.Empty, StringComparison.Ordinal);
                if (_inputAttributes is not null && _inputAttributes.ContainsKey("onchange"))
                {
                    _ = _inputAttributes.Remove("onchange");
                }
            }

            // Apply RTL (right-to-left) layout if enabled
            bool isRtl = SyncfusionService?._options?.EnableRtl == true;
            if (isRtl)
            {
                _rootClass += Space + Rtl;
            }
            else
            {
                _rootClass = _rootClass.Replace(Space + Rtl, string.Empty, StringComparison.Ordinal);
            }
        }
        /// <summary>
        /// Returns the string value for the `aria-checked` state expected by assistive technologies.
        /// Returns "true" or "false" when the checked value can be converted to boolean,
        /// and returns "mixed" when the checked state is null or cannot be determined.
        /// </summary>
        private string? GetAriaPressed()
        {
            try
            {
                if (Checked is null)
                {
                    return "mixed";
                }

                bool b = Convert.ToBoolean(Checked, CultureInfo.InvariantCulture);
                return b ? "true" : "false";
            }
            catch (InvalidOperationException ex)
            {
                _logDebugGetAriaPressed(Logger, ex.Message, ex);
                return "mixed";
            }
        }

        /// <summary>
        /// Updates the CSS classes for the switch's visual state (inner track and handle).
        /// Applies active state classes when checked, removes them when unchecked.
        /// </summary>
        /// <param name="state">State token: use <c>CHECK</c> for checked state, <c>UNCHECK</c> for unchecked.</param>
        private void ChangeState(string state)
        {
            _innerClass = Inner;
            _handleClass = Handle;

            if (state == Check)
            {
                _innerClass += Active;
                _handleClass += Active;
            }
        }
        /// <summary>
        /// Handles native change events from the underlying checkbox input.
        /// Prevents state mutations when the component is disabled by reverting any changes.
        /// This ensures the switch cannot be toggled via native input interaction when disabled.
        /// </summary>
        /// <param name="args">Change event arguments containing the attempted new value.</param>
        private void HandleChange(ChangeEventArgs args)
        {
            if (Disabled)
            {
                // Revert the change by toggling back to the opposite state
                bool attemptedValue = Convert.ToBoolean(args.Value, CultureInfo.InvariantCulture);
                try
                {
                    Checked = (TChecked)(object)!attemptedValue;
                }
                catch (InvalidCastException)
                {
                    throw new InvalidOperationException(
                        $"Cannot revert Switch state: TChecked type '{typeof(TChecked).Name}' is incompatible with boolean value. " +
                        $"The Switch component supports boolean or boolean-compatible types only.",
                        null);
                }
            }
        }


        #endregion

        #region Event Handlers


        /// <summary>
        /// Handles click events on the switch to toggle its checked state.
        /// </summary>
        /// <param name="args">Mouse event arguments from the click interaction.</param>
        private async Task OnClickHandlerAsync(MouseEventArgs args)
        {
            await ToggleAsync(args).ConfigureAwait(false);
        }

        /// <summary>
        /// Toggles the switch between checked and unchecked states.
        /// Updates visual state, persists value if enabled, and invokes the ValueChange callback.
        /// </summary>
        /// <param name="args">Optional mouse event arguments; null for programmatic or touch-based toggles.</param>
        private async Task ToggleAsync(MouseEventArgs? args = null)
        {
            if (Disabled)
            {
                return;
            }

            // Determine new state (opposite of current)
            bool isCurrentlyChecked = Convert.ToBoolean(Checked, CultureInfo.InvariantCulture);
            TChecked newState;
            try
            {
                newState = (TChecked)(object)!isCurrentlyChecked;
            }
            catch (InvalidCastException)
            {
                throw new InvalidOperationException(
                    $"Cannot toggle Switch: TChecked type '{typeof(TChecked).Name}' is incompatible with boolean value. " +
                    $"The Switch component supports boolean or boolean-compatible types only.",
                    null);
            }

            // Update visual classes
            ChangeState(isCurrentlyChecked ? UnCheck : Check);

            // Update the bound checked value
            await UpdateCheckStateAsync(newState).ConfigureAwait(false);

            // Persist to local storage if enabled
            if (EnablePersistence && Checked is not null)
            {
                await SetLocalStorageAsync(_idValue, Checked).ConfigureAwait(false);
            }

            // Notify subscribers of the value change
            if (ValueChange.HasDelegate)
            {
                CheckedChangeEventArgs<TChecked> changeArgs = new()
                {
                    Checked = Checked,
                    Event = args
                };
                await ValueChange.InvokeAsync(changeArgs).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Handles mousedown events to initiate a potential drag gesture.
        /// Sets the drag flag used by subsequent mouse/touch event handlers.
        /// </summary>
        /// <param name="args">Mouse event arguments.</param>
        private void OnMouseDownHandler(MouseEventArgs args)
        {
            if (args.Type == "mousedown")
            {
                _isDrag = true;
            }
        }

        /// <summary>
        /// Handles touch events (touchstart, touchmove, touchend) to support swipe-to-toggle gestures.
        /// Distinguishes between vertical scrolling and horizontal toggle swipes.
        /// Only triggers toggle when horizontal movement dominates, allowing normal scrolling otherwise.
        /// </summary>
        /// <param name="args">Touch event arguments containing touch point data.</param>
        private async Task OnTouchHandlerAsync(TouchEventArgs args)
        {
            switch (args.Type)
            {
                case "touchstart":
                    // Capture initial touch position
                    _isDrag = true;
                    _baseTouchX = args.ChangedTouches[0].ClientX;
                    _baseTouchY = args.ChangedTouches[0].ClientY;
                    _isScrollSwipe = false;
                    break;

                case "touchmove":
                    // Calculate movement delta to distinguish scroll from swipe
                    _preventDefault = false;
                    double currentTouchX = args.ChangedTouches[0].ClientX;
                    double currentTouchY = args.ChangedTouches[0].ClientY;
                    double xDiff = Math.Abs(_baseTouchX - currentTouchX);
                    double yDiff = Math.Abs(_baseTouchY - currentTouchY);

                    // If vertical movement dominates, treat as scroll; otherwise as swipe
                    if (yDiff > xDiff)
                    {
                        _isScrollSwipe = true;
                        _isFirstTouch = true;
                    }
                    else
                    {
                        _isFirstTouch = false;
                    }
                    break;

                case "mouseup":
                case "touchend":
                    // Complete the toggle if this was a valid horizontal swipe
                    if (_isDrag && !_isScrollSwipe)
                    {
                        bool isWebAssembly = OperatingSystem.IsBrowser();

                        // Handle platform-specific first-touch quirks
                        if (_isFirstTouch && !isWebAssembly)
                        {
                            _isFirstTouch = false;
                        }
                        else
                        {
                            await ToggleAsync(null).ConfigureAwait(false);
                        }

                        _preventDefault = true;
                    }
                    break;

                default:
                    // No action needed for other touch event types
                    break;
            }
        }

        #endregion
    }
}