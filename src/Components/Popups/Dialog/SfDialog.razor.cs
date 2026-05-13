using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Toolkit.Internal;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Popups
{
    /// <summary>
    /// The Blazor Dialog is a user interface (UI) component that displays critical information, errors, warnings, and questions to users, as well as confirms decisions and collects input from the users.
    /// Based on user interactions, the dialog is classified as modal or non-modal (modeless).
    /// </summary>
    /// <remarks>
    /// The <see cref="SfDialog"/> component provides a flexible way to display modal or modeless dialogs with customizable content, headers, footers, and buttons.
    /// It supports features like dragging, resizing, animations, and various positioning options to enhance user interaction.
    /// </remarks>
    /// <example>
    /// A basic dialog component with header, content, and buttons.
    /// <code><![CDATA[
    /// <SfDialog ID="defaultDialog" Width="400px" Height="300px" Header="Sample Dialog" Visible="true">
    ///     <div>
    ///         This is a sample dialog content.
    ///     </div>
    ///     <DialogButtons>
    ///         <DialogButton Content="OK" IsPrimary="true" />
    ///         <DialogButton Content="Cancel" />
    ///     </DialogButtons>
    /// </SfDialog>
    /// ]]></code>
    /// </example>
    public partial class SfDialog : SfBaseComponent
    {
        #region Internal variables
        private string? _styles;
        private string? _removedClass;
        private bool _previousVisible;
        private bool _preventVisibility;
        private string? _previousCssClass;
        private bool _allowMaxHeight = true;
        private bool DialogShown { get; set; }
        private Dictionary<string, object> ContainerAttribute { get; set; } = [];
        private Dictionary<string, object> OverlayAttribute { get; set; } = [];

        /// <summary>
        /// Gets a value indicating whether the target application is rendered with server-side rendering (SSR) none mode.
        /// </summary>
        /// <value>
        /// A <see cref="bool"/> value indicating whether static server rendering is enabled. Returns <c>true</c> if the application is using static rendering; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// This property is used internally to determine the rendering mode of the application and adjust dialog behavior accordingly.
        /// </remarks>
        /// <exclude />
        protected bool IsStaticRendering => IsStaticServerRendering();
        // Specifies whether the dialog component dynamically append to DOM element or not.
        private bool IsPreRender { get; set; }
        // AllowPrerender disabled state, ShowAsync public method with re-render and initialize a dialog required
        private bool _enableFullScreen;
        // AllowPrerender disabled state, ShowAsync public method with  single time re-render call on OnAfterRenderAsync handling required
        private bool _isShowCall;
        private BeforeCloseEventArgs _onClosedArgs = new();
        private string _dialogClass = "e-dialog e-lib e-blazor-hidden";
        private Dictionary<string, object> _changedProps = [];
        private Dictionary<string, object> _dialogAttribute = [];
        private Dictionary<string, object>? CloseIconAttributes { get; set; } = [];
        #endregion
        #region Internal variables

        internal string _dataId = Guid.NewGuid().ToString();
        internal RenderFragment? HeaderTemplate { get; set; }
        internal RenderFragment? ContentTemplate { get; set; }
        internal RenderFragment? FooterTemplates { get; set; }
        internal ElementReference DialogElement { get; set; }
        internal ElementReference ModalDialogElement { get; set; }

        internal IJSObjectReference? _dialogJsModule;
        internal IJSInProcessObjectReference? _dialogJsInProcessModule;

        internal IJSObjectReference? _draggableJsModule;
        internal IJSInProcessObjectReference? _draggableJsInModule;

        internal IJSObjectReference? _resizeJsModule;
        internal IJSInProcessObjectReference? _resizeJsInModule;

        [Inject]
        internal ILogger<SfDialog> Logger { get; set; } = default!;
        #endregion

        #region Internal methods
        internal void UpdateTemplate(string name, RenderFragment template)
        {
            switch (name)
            {
                case nameof(Header):
                    HeaderTemplate = template;
                    break;
                case nameof(Content):
                    ContentTemplate = template;
                    break;
                case nameof(FooterTemplate):
                    FooterTemplates = template;
                    break;
                default:
                    break;
            }
        }

        internal void UpdateButtons(List<DialogButton> buttons)
        {
            ButtonsValue = buttons;
            StateHasChanged();
        }

        internal void UpdateChildProperties(string key, object data)
        {
            if (key == POSITION)
            {
                PositionValue = (DialogPositionData)data;
            }
            else if (key == ANIMATION_SETTINGS)
            {
                AnimationSettingsValue = (DialogAnimationSettings)data;
            }
        }

        internal Dictionary<string, string> GetPosition()
        {
            return new Dictionary<string, string>
            {
                { X, PositionValue?.X ?? CENTER },
                { Y, PositionValue?.Y ?? CENTER }
            };
        }

        internal Dictionary<string, object> GetAnimationSettings()
        {
            return new Dictionary<string, object>
            {
                { DELAY, AnimationSettingsValue.Delay },
                { DURATION, AnimationSettingsValue.Duration },
                { ANIMATE_EFFECT, AnimationSettingsValue.Effect.ToString() }
            };
        }

        internal void Refresh()
        {
            StateHasChanged();
        }
        #endregion

        #region Private methods
        private void UpdateLocale()
        {
            CloseIconAttributes?.Clear();
            CloseIconAttributes?.Add(ARIA_LABEL, CLOSE);
            CloseIconAttributes?.Add(TITLE, Localizer[DIALOG_CLOSE] ?? CLOSE);
        }

        private void UpdateLocalProperties()
        {
            _dialogClass += " " + POPUP_CLOSE;
            if (!string.IsNullOrEmpty(CssClass))
            {
                _dialogClass = SfBaseUtils.AddClass(_dialogClass, CssClass);
            }
            if (EnableResize)
            {
                _dialogClass = SfBaseUtils.AddClass(_dialogClass, RESIZABLE);
            }
            if (SyncfusionService is not null && SyncfusionService._options.EnableRtl)
            {
                _dialogClass = SfBaseUtils.AddClass(_dialogClass, RTL);
            }
            if (IsModal)
            {
                _dialogClass += " " + DIALOG_MODAL;
                _styles = $"{Z_INDEX}: {ZIndex}";
                if (Visible && IsStaticRendering)
                {
                    _styles += ";display:flex;";
                }
                ContainerAttribute = new Dictionary<string, object>
                {
                    { STYLE, _styles }
                };
                /* To show a modal dialog element at top of overlay element, we reduce overlay zindex by -1 */
                OverlayAttribute = new Dictionary<string, object>
                {
                    { STYLE, $"{Z_INDEX}: {ZIndex - 1}" }
                };
            }
            if (!Visible && IsModal)
            {
                string styleValue = $"{Z_INDEX}: {ZIndex - 1};display:none;";
                OverlayAttribute = new Dictionary<string, object>
                {
                    { STYLE, styleValue }
                };
            }
            UpdateHtmlAttributes();
        }

        private ElementReference GetElementRef()
        {
            return IsModal ? ModalDialogElement : DialogElement;
        }

        /// <summary>
        /// Gets the resize direction string based on the configured resize handles.
        /// </summary>
        /// <returns>A space-separated string of resize directions.</returns>
        private string GetResizeDirection()
        {
            if (ResizeHandles is null)
            {
                return string.Empty;
            }

            string direction = BuildResizeDirectionString();
            return ApplyRtlDirectionAdjustment(direction);
        }

        /// <summary>
        /// Builds the resize direction string from the resize handles array.
        /// </summary>
        /// <returns>A space-separated string of resize directions.</returns>
        private string BuildResizeDirectionString()
        {
            string direction = string.Empty;
            for (int i = 0; i < ResizeHandles.Length; i++)
            {
                if (ResizeHandles[i] == ResizeDirection.All)
                {
                    return ALL_DIRECTIONS;
                }
                string directionValue = MapResizeDirectionToString(ResizeHandles[i]);
                direction += directionValue + SPACE;
            }
            return direction;
        }

        /// <summary>
        /// Maps a resize direction enum value to its string representation.
        /// </summary>
        /// <param name="resizeDirection">The resize direction to map.</param>
        /// <returns>The string representation of the resize direction.</returns>
        private string MapResizeDirectionToString(ResizeDirection resizeDirection)
        {
            return resizeDirection switch
            {
                ResizeDirection.SouthEast => SOUTH_EAST,
                ResizeDirection.SouthWest => SOUTH_WEST,
                ResizeDirection.NorthEast => NORTH_EAST,
                ResizeDirection.NorthWest => NORTH_WEST,
                ResizeDirection.South => "south",
                ResizeDirection.North => "north",
                ResizeDirection.East => "east",
                ResizeDirection.West => "west",
                ResizeDirection.All => ALL_DIRECTIONS,
                _ => resizeDirection.ToString()
            };
        }

        /// <summary>
        /// Adjusts resize direction for RTL mode by swapping east and west directions.
        /// </summary>
        /// <param name="direction">The direction string to adjust.</param>
        /// <returns>The adjusted direction string.</returns>
        private string ApplyRtlDirectionAdjustment(string direction)
        {
            if (SyncfusionService is not null && !SyncfusionService._options.EnableRtl)
            {
                return direction;
            }
            string trimmedDirection = direction.Trim();
            return trimmedDirection switch
            {
                SOUTH_EAST => SOUTH_WEST,
                SOUTH_WEST => SOUTH_EAST,
                _ => direction
            };
        }

        private bool IsHeaderContentExist()
        {
            bool hasHeader = (HeaderTemplate is not null && string.IsNullOrEmpty(Header)) || (HeaderTemplate is null && !string.IsNullOrEmpty(Header));
            _dialogAttribute = SfBaseUtils.UpdateDictionary("aria-labelledby", hasHeader ? $"{ID}_title" : $"{ID}_dialog-content", _dialogAttribute);
            return hasHeader;
        }
        private void UpdateHtmlAttributes()
        {
            Dictionary<string, object>? attributes = HtmlAttributes;
            if (attributes is not null && _dialogAttribute is not null)
            {
                foreach (KeyValuePair<string, object> item in attributes)
                {
                    if (item.Key == CLASS)
                    {
                        _dialogClass = SfBaseUtils.AddClass(_dialogClass, (string)item.Value);
                    }
                    else if (item.Key == STYLE)
                    {
                        if (_dialogAttribute.ContainsKey(STYLE))
                        {
                            _dialogAttribute[item.Key] += item.Value.ToString();
                        }
                        else
                        {
                            _dialogAttribute = SfBaseUtils.UpdateDictionary(item.Key, item.Value, _dialogAttribute);
                        }
                    }
                    else
                    {
                        _dialogAttribute = SfBaseUtils.UpdateDictionary(item.Key, item.Value, _dialogAttribute);
                    }
                }
            }
            if (_dialogAttribute is not null && !_dialogAttribute.ContainsKey("aria-label"))
            {
                _dialogAttribute = SfBaseUtils.UpdateDictionary("aria-label", "dialog", _dialogAttribute);
            }
            if (_dialogAttribute is not null && Visible && IsModal && IsStaticRendering)
            {
                string _zIndex = $"{Z_INDEX}: {ZIndex}";
                _dialogAttribute = SfBaseUtils.UpdateDictionary("style", _zIndex, _dialogAttribute);
            }
            if (_dialogAttribute is not null && Visible && IsStaticRendering && Width is not null)
            {
                string width = IsModal ? $"width: {Width}; z-index: {ZIndex};" : $"width: {Width}";
                _dialogAttribute = SfBaseUtils.UpdateDictionary("style", width, _dialogAttribute);
            }
        }

        /// <summary>
        /// Creates a dialog instance configuration based on the JavaScript runtime type.
        /// </summary>
        /// <param name="isInitial">Indicates whether this is the initial creation of the dialog instance.</param>
        /// <returns>A dictionary containing the dialog configuration properties, or null if an error occurs.</returns>
        private Dictionary<string, object> GetInstance(bool isInitial)
        {
            Dictionary<string, object> dlgObj = JSRuntime is IJSInProcessRuntime
                ? CreateInProcessInstance(isInitial)
                : CreateServerInstance(isInitial);

            AddOptionalDialogProperties(dlgObj);
            return dlgObj;
        }

        /// <summary>
        /// Creates the base instance configuration for in-process JavaScript runtime.
        /// </summary>
        /// <param name="isInitial">Indicates whether this is the initial creation.</param>
        /// <returns>A dictionary with base properties and conditionally added optional properties.</returns>
        private Dictionary<string, object> CreateInProcessInstance(bool isInitial)
        {
            Dictionary<string, object> dlgObj = new()
            {
                { DATA_ID, _dataId },
                { IS_INITIAL, isInitial },
                { ELEMENT, GetElementRef() },
                { DOT_NET_REF, DotnetObjectReference! }
            };
            AddInProcessOptionalProperties(dlgObj);
            AddDelegateProperties(dlgObj);
            return dlgObj;
        }

        /// <summary>
        /// Adds optional properties to the in-process instance configuration.
        /// </summary>
        /// <param name="dlgObj">The dialog object dictionary to populate.</param>
        private void AddInProcessOptionalProperties(Dictionary<string, object> dlgObj)
        {
            AddLayoutProperties(dlgObj);
            AddBehaviorProperties(dlgObj);
        }

        /// <summary>
        /// Adds layout-related properties to the dialog configuration.
        /// </summary>
        /// <param name="dlgObj">The dialog object dictionary to populate.</param>
        private void AddLayoutProperties(Dictionary<string, object> dlgObj)
        {
            if (!string.IsNullOrEmpty(CssClass))
            {
                dlgObj.Add(DICTIONARY_CSSCLASS, CssClass);
            }
            if (!string.IsNullOrEmpty(MinHeight))
            {
                dlgObj.Add(DICTIONARY_MIN_HEIGHT, MinHeight);
            }
            if (Height != "auto")
            {
                dlgObj.Add(DICTIONARY_HEIGHT, Height);
            }
            if (Width != "100%")
            {
                dlgObj.Add(DICTIONARY_WIDTH, Width);
            }
            if (ZIndex != 1000)
            {
                dlgObj.Add(DICTIONARY_ZINDEX, ZIndex);
            }
            if (!string.IsNullOrEmpty(Target))
            {
                dlgObj.Add(DICTIONARY_TARGET, Target);
            }
        }

        /// <summary>
        /// Adds behavior-related properties to the dialog configuration.
        /// </summary>
        /// <param name="dlgObj">The dialog object dictionary to populate.</param>
        private void AddBehaviorProperties(Dictionary<string, object> dlgObj)
        {
            if (!Visible)
            {
                dlgObj.Add(DICTIONARY_VISIBLE, false);
            }
            if (IsModal)
            {
                dlgObj.Add(DICTIONARY_IS_MODAL, true);
            }
            if (SyncfusionService is not null && SyncfusionService._options.EnableRtl)
            {
                dlgObj.Add(DICTIONARY_ENABLE_RTL, true);
            }
            if (EnableResize)
            {
                dlgObj.Add(DICTIONARY_ENABLE_RESIZE, true);
            }

            if (AllowDragging)
            {
                dlgObj.Add(DICTIONARY_ALLOW_DRAGGING, true);
            }
            if (!CloseOnEscape)
            {
                dlgObj.Add(DICTIONARY_CLOSE_ON_ESCAPE, false);
            }
            if (AllowPrerender)
            {
                dlgObj.Add(DICTIONARY_ALLOW_PRERENDER, true);
            }
            if (ResizeHandles.Length != 1 || ResizeHandles[0] != ResizeDirection.SouthEast)
            {
                dlgObj.Add(RESIZE_ICON_DIRECTION, GetResizeDirection());
            }
        }

        /// <summary>
        /// Adds event delegate properties to the dialog configuration.
        /// </summary>
        /// <param name="dlgObj">The dialog object dictionary to populate.</param>
        private void AddDelegateProperties(Dictionary<string, object> dlgObj)
        {
            if (Opened.HasDelegate)
            {
                dlgObj.Add(OPENED_ENABLED, true);
            }
            if (Closed.HasDelegate)
            {
                dlgObj.Add(CLOSED_ENABLED, true);
            }
            if (OnDrag.HasDelegate)
            {
                dlgObj.Add(ON_DRAG_ENABLED, true);
            }
            if (OnDragStart.HasDelegate)
            {
                dlgObj.Add(ON_DRAG_START_ENABLED, true);
            }
            if (OnDragStop.HasDelegate)
            {
                dlgObj.Add(ON_DRAG_STOP_ENABLED, true);
            }
            if (Resizing.HasDelegate)
            {
                dlgObj.Add(RESIZING_ENABLED, true);
            }
            if (OnResizeStart.HasDelegate)
            {
                dlgObj.Add(ON_RESIZE_START_ENABLED, true);
            }
            if (OnResizeStop.HasDelegate)
            {
                dlgObj.Add(ON_RESIZE_STOP_ENABLED, true);
            }
        }

        /// <summary>
        /// Creates the base instance configuration for server-side rendering.
        /// </summary>
        /// <param name="isInitial">Indicates whether this is the initial creation.</param>
        /// <returns>A dictionary with all server-side properties.</returns>
        private Dictionary<string, object> CreateServerInstance(bool isInitial)
        {
            Dictionary<string, object> dlgObj = GetServerInstanceBaseProperties(isInitial);
            AddServerInstanceEventProperties(dlgObj);
            return dlgObj;
        }

        /// <summary>
        /// Gets the base properties for server-side instance.
        /// </summary>
        /// <param name="isInitial">Indicates whether this is the initial creation.</param>
        /// <returns>A dictionary with base server-side properties.</returns>
        private Dictionary<string, object> GetServerInstanceBaseProperties(bool isInitial)
        {
            return new Dictionary<string, object>
            {
                { DATA_ID, _dataId },
                { IS_INITIAL, isInitial },
                { DOT_NET_REF, DotnetObjectReference! },
                { ELEMENT, GetElementRef() },
                { DICTIONARY_TARGET, Target ?? BODY },
                { DICTIONARY_WIDTH, SfBaseUtils.FormatUnit(Width) },
                { DICTIONARY_HEIGHT, SfBaseUtils.FormatUnit(Height) },
                { DICTIONARY_ZINDEX, ZIndex },
                { DICTIONARY_VISIBLE, Visible },
                { DICTIONARY_IS_MODAL, IsModal },
                { DICTIONARY_CSSCLASS, CssClass },
                { ALLOWMAXHEIGHT, _allowMaxHeight },
                { DICTIONARY_ENABLE_RTL, SyncfusionService is not null && SyncfusionService._options.EnableRtl },
                { DICTIONARY_MIN_HEIGHT, string.IsNullOrEmpty(MinHeight) ? MinHeight : SfBaseUtils.FormatUnit(MinHeight) },
                { PREVENT_VISIBILITY, _preventVisibility },
                { DICTIONARY_ENABLE_RESIZE, EnableResize },
                { DICTIONARY_ENABLE_PERSISTENCE, EnablePersistence },
                { DICTIONARY_ALLOW_DRAGGING, AllowDragging },
                { DICTIONARY_CLOSE_ON_ESCAPE, CloseOnEscape },
                { RESIZE_ICON_DIRECTION, GetResizeDirection() },
                { DICTIONARY_ALLOW_PRERENDER, AllowPrerender }
            };
        }

        /// <summary>
        /// Adds event delegate properties to the server-side instance.
        /// </summary>
        /// <param name="dlgObj">The dialog object dictionary to populate.</param>
        private void AddServerInstanceEventProperties(Dictionary<string, object> dlgObj)
        {
            dlgObj.Add(CLOSED_ENABLED, Closed.HasDelegate);
            dlgObj.Add(ON_DRAG_ENABLED, OnDrag.HasDelegate);
            dlgObj.Add(ON_DRAG_START_ENABLED, OnDragStart.HasDelegate);
            dlgObj.Add(ON_DRAG_STOP_ENABLED, OnDragStop.HasDelegate);
            dlgObj.Add(RESIZING_ENABLED, Resizing.HasDelegate);
            dlgObj.Add(ON_RESIZE_START_ENABLED, OnResizeStart.HasDelegate);
            dlgObj.Add(ON_RESIZE_STOP_ENABLED, OnResizeStop.HasDelegate);
            dlgObj.Add(OPENED_ENABLED, Opened.HasDelegate);
        }

        /// <summary>
        /// Adds optional animation and position properties to the dialog configuration.
        /// </summary>
        /// <param name="dlgObj">The dialog object dictionary to populate.</param>
        private void AddOptionalDialogProperties(Dictionary<string, object> dlgObj)
        {
            if (PositionValue is not null)
            {
                dlgObj.Add(POSITION, GetPosition());
            }
            if (IsAnimationEnabled())
            {
                if (AnimationSettingsValue is null)
                {
                    dlgObj.Add(ANIMATION_SETTINGS, new Dictionary<string, object>
                    {
                        { DELAY, 0 }, { DURATION, 400 }, { ANIMATE_EFFECT, "Fade" }
                    });
                }
                else
                {
                    dlgObj.Add(ANIMATION_SETTINGS, GetAnimationSettings());
                }
            }
        }

        /// <summary>
        /// Returns whether animations are enabled by global service settings.
        /// </summary>
        private bool IsAnimationEnabled()
        {
            return SyncfusionService is not null &&
                   (SyncfusionService._options.Animation == GlobalAnimationMode.Default ||
                    SyncfusionService._options.Animation == GlobalAnimationMode.Enable);
        }

        private async Task HideDialogAsync(string? action, BeforeCloseEventArgs? args = null)
        {
            DialogShown = false;
            if (_preventVisibility)
            {
                BeforeCloseEventArgs eventArgs = PrepareCloseEvent(action, args);
                _onClosedArgs = eventArgs;
                await SfBaseUtils.InvokeEventAsync(OnClose, eventArgs).ConfigureAwait(false);
                if (!eventArgs.Cancel)
                {
                    await ExecuteHideDialogAsync(eventArgs).ConfigureAwait(false);
                }
                else
                {
                    await CancelHideDialogAsync().ConfigureAwait(false);
                }
            }
            else
            {
                _preventVisibility = false;
            }
        }

        /// <summary>
        /// Prepares the event arguments for closing the dialog.
        /// </summary>
        /// <param name="action">The action that triggered the close operation.</param>
        /// <param name="args">The optional event arguments.</param>
        /// <returns>The prepared <see cref="BeforeCloseEventArgs"/>.</returns>
        private BeforeCloseEventArgs PrepareCloseEvent(string? action, BeforeCloseEventArgs? args)
        {
            return new BeforeCloseEventArgs()
            {
                Cancel = false,
                ClosedBy = action ?? USER_ACTION,
                Event = args is null ? new() : args.Event,
                IsInteracted = args is not null && args.Event is not null
            };
        }

        /// <summary>
        /// Executes the dialog hide operation after validation.
        /// </summary>
        /// <param name="eventArgs">The event arguments containing close dialog configuration.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task ExecuteHideDialogAsync(BeforeCloseEventArgs eventArgs)
        {
            string? dialogClass = await InvokeAsync<string>(_dialogJsModule!, _dialogJsInProcessModule!, JS_HIDE, _dataId, eventArgs.PreventFocus).ConfigureAwait(false);
            if (dialogClass is not null)
            {
                _dialogClass = dialogClass.Replace("e-popup-open", "e-popup-close", StringComparison.Ordinal);
            }
            if (!AllowPrerender)
            {
                await WaitForAnimationCompletionAsync().ConfigureAwait(false);
                IsPreRender = false;
            }

            // Ensure all component state changes and rendering are executed on the renderer/Dispatcher.
            await InvokeAsync(async () =>
            {
                _preventVisibility = false;
                _previousVisible = Visible;
                bool tempValue = _visible;
                _visible = false;
                Visible = _visible = await SfBaseUtils.UpdatePropertyAsync(false, tempValue, VisibleChanged).ConfigureAwait(false);
                StateHasChanged();
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Waits for the dialog animation to complete.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task WaitForAnimationCompletionAsync()
        {
            if (AnimationSettingsValue is not null && IsAnimationEnabled())
            {
                await Task.Delay(Convert.ToInt32(AnimationSettingsValue.Delay + AnimationSettingsValue.Duration)).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Cancels the dialog hide operation when the BeforeClose event is cancelled.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task CancelHideDialogAsync()
        {
            _previousVisible = Visible;
            Visible = _visible = await SfBaseUtils.UpdatePropertyAsync(true, _visible, VisibleChanged).ConfigureAwait(false);
        }

        private async Task ServerPropertyChangeHandlerAsync()
        {
            _previousVisible = Visible;
            if (Visible && !_preventVisibility)
            {
                if (!AllowPrerender)
                {
                    await InvokeVoidAsync(_dialogJsModule, _dialogJsInProcessModule, JS_INITIALIZE, GetInstance(false)).ConfigureAwait(true);
                }
                await ShowDialogAsync().ConfigureAwait(false);
            }
            else if (!Visible)
            {
                await HideDialogAsync(null).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Handles property changes on the client side by updating dialog classes and notifying JavaScript.
        /// </summary>
        /// <param name="changedKeys">List of property names that have changed.</param>
        internal async Task ClientPropertyChangeHandlerAsync(List<string> changedKeys)
        {
            await UpdateDialogClassForChangesAsync().ConfigureAwait(false);
            bool isDraggableDestroy = BuildChangedPropertiesDictionary(changedKeys);
            await ApplyClientPropertyChangesAsync(isDraggableDestroy).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates the dialog CSS class based on current state.
        /// </summary>
        private async Task UpdateDialogClassForChangesAsync()
        {
            if (AllowPrerender || (!AllowPrerender && IsPreRender && _preventVisibility))
            {
                _dialogClass = await InvokeAsync<string>(_dialogJsModule!, _dialogJsInProcessModule!, JS_GET_CLASS_LIST, GetElementRef()).ConfigureAwait(false);
            }

            if (string.IsNullOrEmpty(_dialogClass))
            {
                _dialogClass = SfBaseUtils.AddClass(_dialogClass, POPUP + " " + POPUP_CLOSE);
            }
        }

        /// <summary>
        /// Builds a dictionary of changed properties and updates dialog classes accordingly.
        /// </summary>
        /// <param name="changedKeys">List of property names that have changed.</param>
        /// <returns>True if draggable functionality should be destroyed; otherwise, false.</returns>
        private bool BuildChangedPropertiesDictionary(List<string> changedKeys)
        {
            bool isDraggableDestroy = false;
            _changedProps.Clear();

            UpdateCssClassChanges(changedKeys);
            UpdateDialogStateClasses(changedKeys);
            UpdatePropertyDictionary(changedKeys, ref isDraggableDestroy);

            return isDraggableDestroy;
        }

        /// <summary>
        /// Updates CSS class changes in the dialog.
        /// </summary>
        /// <param name="changedKeys">List of property names that have changed.</param>
        private void UpdateCssClassChanges(List<string> changedKeys)
        {
            if (!changedKeys.Contains(CSSCLASS))
            {
                return;
            }

            if (!string.IsNullOrEmpty(_previousCssClass))
            {
                _dialogClass = SfBaseUtils.RemoveClass(_dialogClass, _previousCssClass);
                _removedClass = _previousCssClass;
            }

            if (!string.IsNullOrEmpty(CssClass))
            {
                _dialogClass = SfBaseUtils.AddClass(_dialogClass, CssClass);
            }

            if (!string.IsNullOrEmpty(_removedClass) && _dialogClass.Contains(_removedClass, StringComparison.Ordinal))
            {
                _dialogClass = SfBaseUtils.RemoveClass(_dialogClass, _removedClass);
            }

            _previousCssClass = CssClass;
        }

        /// <summary>
        /// Updates dialog state-related CSS classes.
        /// </summary>
        /// <param name="changedKeys">List of property names that have changed.</param>
        private void UpdateDialogStateClasses(List<string> changedKeys)
        {
            if (changedKeys.Contains(ISMODAL))
            {
                _dialogClass = IsModal ? SfBaseUtils.AddClass(_dialogClass, DIALOG_MODAL) : SfBaseUtils.RemoveClass(_dialogClass, DIALOG_MODAL);
            }
            if (changedKeys.Contains(ENABLE_RTL))
            {
                _dialogClass = SyncfusionService is not null && SyncfusionService._options.EnableRtl ? SfBaseUtils.AddClass(_dialogClass, RTL) : SfBaseUtils.RemoveClass(_dialogClass, RTL);
                _changedProps.Add(DICTIONARY_ENABLE_RTL, SyncfusionService is not null && SyncfusionService._options.EnableRtl);
            }
            if (changedKeys.Contains(ENABLE_RESIZE))
            {
                _dialogClass = EnableResize ? SfBaseUtils.AddClass(_dialogClass, RESIZABLE) : SfBaseUtils.RemoveClass(_dialogClass, RESIZABLE);
                _changedProps.Add(DICTIONARY_ENABLE_RESIZE, EnableResize);
            }
        }

        /// <summary>
        /// Updates the changed properties dictionary with simple property changes.
        /// </summary>
        /// <param name="changedKeys">List of property names that have changed.</param>
        /// <param name="isDraggableDestroy">Output parameter indicating if draggable should be destroyed.</param>
        private void UpdatePropertyDictionary(List<string> changedKeys, ref bool isDraggableDestroy)
        {
            UpdateLayoutPropertyChanges(changedKeys);
            UpdateBehaviorPropertyChanges(changedKeys, ref isDraggableDestroy);
        }

        /// <summary>
        /// Updates layout-related property changes.
        /// </summary>
        /// <param name="changedKeys">List of property names that have changed.</param>
        private void UpdateLayoutPropertyChanges(List<string> changedKeys)
        {
            if (changedKeys.Contains(WIDTH))
            {
                _changedProps.Add(DICTIONARY_WIDTH, SfBaseUtils.FormatUnit(Width));
            }
            if (changedKeys.Contains(HEIGHT))
            {
                _changedProps.Add(DICTIONARY_HEIGHT, SfBaseUtils.FormatUnit(Height));
            }
            if (changedKeys.Contains(TARGET))
            {
                _changedProps.Add(DICTIONARY_TARGET, Target);
            }
            if (changedKeys.Contains(ZINDEX))
            {
                _changedProps.Add(DICTIONARY_ZINDEX, ZIndex);
            }
            if (changedKeys.Contains(MIN_HEIGHT))
            {
                _changedProps.Add(DICTIONARY_MIN_HEIGHT, string.IsNullOrEmpty(MinHeight) ? MinHeight : SfBaseUtils.FormatUnit(MinHeight));
            }
        }

        /// <summary>
        /// Updates behavior-related property changes.
        /// </summary>
        /// <param name="changedKeys">List of property names that have changed.</param>
        /// <param name="isDraggableDestroy">Output parameter indicating if draggable should be destroyed.</param>
        private void UpdateBehaviorPropertyChanges(List<string> changedKeys, ref bool isDraggableDestroy)
        {
            if (changedKeys.Contains(CLOSE_ON_ESCAPE))
            {
                _changedProps.Add(DICTIONARY_CLOSE_ON_ESCAPE, CloseOnEscape);
            }
            if (changedKeys.Contains(ALLOW_DRAGGING))
            {
                if (!AllowDragging)
                {
                    isDraggableDestroy = true;
                    _changedProps.Add(DICTIONARY_ALLOW_DRAGGING, AllowDragging);
                }
                else if (AllowDragging && (IsHeaderContentExist() || ShowCloseIcon))
                {
                    _changedProps.Add(DICTIONARY_ALLOW_DRAGGING, AllowDragging);
                }
            }
        }

        /// <summary>
        /// Applies the collected property changes to the client-side dialog instance.
        /// </summary>
        /// <param name="isDraggableDestroy">Indicates if draggable functionality should be destroyed.</param>
        private async Task ApplyClientPropertyChangesAsync(bool isDraggableDestroy)
        {
            if (_changedProps.Count == 0)
            {
                return;
            }
            _changedProps.Add(DATA_ID, _dataId);
            await InvokeVoidAsync(_dialogJsModule!, _dialogJsInProcessModule!, JS_PROPERTY_CHANGED, _changedProps).ConfigureAwait(true);
            if (isDraggableDestroy)
            {
                _dialogClass = await InvokeAsync<string>(_dialogJsModule!, _dialogJsInProcessModule!, JS_GET_CLASS_LIST, GetElementRef()).ConfigureAwait(false);
            }
        }
        #endregion

        #region Event handler methods
        private async Task OverlayClickHandlerAsync(MouseEventArgs args)
        {
            if (OnOverlayModalClick.HasDelegate)
            {
                OverlayModalClickEventArgs eventArgs = new() { Event = args, PreventFocus = false };
                await OnOverlayModalClick.InvokeAsync(eventArgs).ConfigureAwait(false);
                if (!eventArgs.PreventFocus)
                {
                    await InvokeVoidAsync(_dialogJsModule, _dialogJsInProcessModule, JS_FOCUS_CONTENT, _dataId).ConfigureAwait(true);
                }
            }
        }

        private async Task CloseIconClickHandlerAsync(MouseEventArgs args)
        {
            await HideDialogAsync(CLOSE_ICON, new BeforeCloseEventArgs() { Event = args }).ConfigureAwait(false);
        }
        #endregion

        #region JSInterop methods

        /// <summary>
        /// Method invoked after the dialog has been opened and displayed.
        /// </summary>
        /// <param name="classes">The CSS class names applied to the dialog element after opening.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method is called from JavaScript after the dialog opening animation is completed.
        /// It triggers the Opened event and handles focus management for the dialog content.
        /// </remarks>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task OpenEventAsync(string classes)
        {
            _dialogClass = classes;
            OpenEventArgs eventArgs = new()
            {
                Name = OPENED,
                Cancel = false,
                PreventFocus = false,
            };
            await SfBaseUtils.InvokeEventAsync(Opened, eventArgs).ConfigureAwait(false);
            if (!eventArgs.PreventFocus)
            {
                await Task.Yield(); // it ensure that the child component rendered.
                await InvokeVoidAsync(_dialogJsModule, _dialogJsInProcessModule, JS_FOCUS_CONTENT, _dataId).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Method invoked after the dialog has been closed and hidden.
        /// </summary>
        /// <param name="classes">The CSS class names applied to the dialog element after closing.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method is called from JavaScript after the dialog closing animation is completed.
        /// It triggers the Closed event and performs cleanup operations.
        /// </remarks>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task CloseEventAsync(string classes)
        {
            _dialogClass = classes;
            if (Closed.HasDelegate)
            {
                await Closed.InvokeAsync(new CloseEventArgs() { Name = CLOSED, Event = _onClosedArgs.Event, Cancel = _onClosedArgs.Cancel, ClosedBy = _onClosedArgs.ClosedBy, IsInteracted = _onClosedArgs.IsInteracted }).ConfigureAwait(false);
            }
            await InvokeVoidAsync(_dialogJsModule, _dialogJsInProcessModule, JS_POPUP_CLOSE, _dataId).ConfigureAwait(true);
        }

        /// <summary>
        /// Method invoked when the user starts to drag the dialog.
        /// </summary>
        /// <param name="args">The drag start event arguments containing information about the drag operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method is called from JavaScript when the user begins dragging the dialog.
        /// It triggers the OnDragStart event if dragging is enabled for the dialog.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="args"/> is <c>null</c>.</exception>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task DragStartEventAsync(DragStartEventArgs args)
        {
            ArgumentNullException.ThrowIfNull(args);
            if (OnDragStart.HasDelegate)
            {
                await OnDragStart.InvokeAsync(new DragStartEventArgs() { Name = DRAG_START, Event = args.Event }).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Method invoked while the user is dragging the dialog.
        /// </summary>
        /// <param name="args">The drag event arguments containing information about the current drag operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method is called from JavaScript during the drag operation as the user moves the dialog.
        /// It triggers the OnDrag event continuously while dragging is in progress.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="args"/> is <c>null</c>.</exception>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task DragEventAsync(DragEventArgs args)
        {
            ArgumentNullException.ThrowIfNull(args);
            if (OnDrag.HasDelegate)
            {
                await OnDrag.InvokeAsync(new DragEventArgs() { Name = DRAG, Event = args.Event }).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Method invoked when the user completes the drag action on the dialog.
        /// </summary>
        /// <param name="args">The drag stop event arguments containing information about the completed drag operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method is called from JavaScript when the user releases the dialog after dragging.
        /// It triggers the OnDragStop event to notify that the drag operation has been completed.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="args"/> is <c>null</c>.</exception>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task DragStopEventAsync(DragStopEventArgs args)
        {
            ArgumentNullException.ThrowIfNull(args);
            if (OnDragStop.HasDelegate)
            {
                await OnDragStop.InvokeAsync(new DragStopEventArgs() { Name = DRAG_STOP, Event = args.Event }).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Method invoked when the user starts to resize the dialog.
        /// </summary>
        /// <param name="args">The mouse event arguments containing information about the resize start operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method is called from JavaScript when the user begins resizing the dialog using the resize handles.
        /// It triggers the OnResizeStart event if resizing is enabled for the dialog.
        /// </remarks>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task ResizeStartEventAsync(MouseEventArgs args)
        {
            if (OnResizeStart.HasDelegate)
            {
                await OnResizeStart.InvokeAsync(args).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Method invoked while the user is resizing the dialog.
        /// </summary>
        /// <param name="args">The mouse event arguments containing information about the current resize operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method is called from JavaScript during the resize operation as the user changes the dialog size.
        /// It triggers the Resizing event continuously while resizing is in progress.
        /// </remarks>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task ResizingEventAsync(MouseEventArgs args)
        {
            if (Resizing.HasDelegate)
            {
                await Resizing.InvokeAsync(args).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Method invoked after the user completes resizing the dialog.
        /// </summary>
        /// <param name="args">The mouse event arguments containing information about the completed resize operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method is called from JavaScript when the user releases the resize handle after resizing the dialog.
        /// It triggers the OnResizeStop event to notify that the resize operation has been completed.
        /// </remarks>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task ResizeStopEventAsync(MouseEventArgs args)
        {
            if (OnResizeStop.HasDelegate)
            {
                await OnResizeStop.InvokeAsync(args).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Method invoked to show the dialog with proper event handling and validation.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method is called from JavaScript to display the dialog. It handles the BeforeOpen event,
        /// allows for cancellation, and manages the dialog's visibility state and focus behavior.
        /// </remarks>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task ShowDialogAsync()
        {
            if (!DialogShown)
            {
                DialogShown = true;
                BeforeOpenEventArgs eventArgs = await PrepareShowDialogEventAsync().ConfigureAwait(false);
                if (!eventArgs.Cancel)
                {
                    await ExecuteShowDialogAsync(eventArgs).ConfigureAwait(false);
                }
                else
                {
                    await CancelShowDialogAsync().ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Prepares the event arguments for showing the dialog.
        /// </summary>
        /// <returns>A <see cref="Task"/> containing the prepared <see cref="BeforeOpenEventArgs"/>.</returns>
        private async Task<BeforeOpenEventArgs> PrepareShowDialogEventAsync()
        {
            string maxHeight = await InvokeAsync<string>(_dialogJsModule!, _dialogJsInProcessModule!, JS_GET_MAX_HEIGHT, _dataId).ConfigureAwait(false);
            BeforeOpenEventArgs eventArgs = new()
            {
                Cancel = false,
                MaxHeight = maxHeight
            };

            // Ensure event callbacks that may trigger rendering run on the Blazor renderer/Dispatcher.
            await InvokeAsync(async () =>
            {
                await SfBaseUtils.InvokeEventAsync(OnOpen, eventArgs).ConfigureAwait(false);
            }).ConfigureAwait(false);

            return eventArgs;
        }

        /// <summary>
        /// Executes the dialog show operation after validation.
        /// </summary>
        /// <param name="eventArgs">The event arguments containing show dialog configuration.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task ExecuteShowDialogAsync(BeforeOpenEventArgs eventArgs)
        {
            string maxHeight = eventArgs.MaxHeight;
            if (await InvokeAsync<string>(_dialogJsModule!, _dialogJsInProcessModule!, JS_GET_MAX_HEIGHT, _dataId).ConfigureAwait(false) != maxHeight)
            {
                _allowMaxHeight = false;
            }
            await InvokeVoidAsync(_dialogJsModule!, _dialogJsInProcessModule!, JS_SHOW, new Dictionary<string, object>
            {
                { DATA_ID, _dataId },
                { FULL_SCREEN, _enableFullScreen },
                { MAX_HEIGHT, maxHeight },
            }).ConfigureAwait(true);
            _preventVisibility = true;
            if (AllowPrerender)
            {
                _previousVisible = Visible;
                _visible = await SfBaseUtils.UpdatePropertyAsync(true, _visible, VisibleChanged).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Cancels the dialog show operation when the BeforeOpen event is cancelled.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task CancelShowDialogAsync()
        {
            _previousVisible = Visible;
            Visible = _visible = await SfBaseUtils.UpdatePropertyAsync(false, _visible, VisibleChanged).ConfigureAwait(false);
            _preventVisibility = false;
            DialogShown = false;
        }

        /// <summary>
        /// Method invoked to close the dialog when triggered by keyboard events (such as Escape key).
        /// </summary>
        /// <param name="args">The keyboard event arguments containing information about the key press that triggered the close action.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method is called from JavaScript when a keyboard event (typically the Escape key) should close the dialog.
        /// It triggers the dialog closing process with the appropriate close action identifier.
        /// </remarks>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task CloseDialogAsync(KeyboardEventArgs args)
        {
            await HideDialogAsync(ESCAPE, new BeforeCloseEventArgs() { Event = args }).ConfigureAwait(false);
        }
        #endregion
    }
}
