using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Toolkit.Internal;

namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>
    /// A composable content surface for presenting a single subject with optional header, media, body, actions, and footer regions.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Place building blocks such as <see cref="SfCardHeader"/>, <see cref="SfCardMedia"/>, <see cref="SfCardContent"/>,
    /// <see cref="SfCardActions"/>, and <see cref="SfCardFooter"/> as child content, or supply arbitrary markup via
    /// <see cref="ChildContent"/>.
    /// </para>
    /// <para>
    /// Works in Static SSR for presentation; <see cref="OnClick"/> requires an interactive render mode.
    /// </para>
    /// <para>
    /// <strong>Accessibility:</strong> a static card exposes no role. A clickable card exposes <c>role="button"</c>, is focusable,
    /// activates with Enter and Space, and never traps Tab. Put interactive controls inside <see cref="SfCardActions"/>, which isolates them from the card activation.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// <SfCard Variant="CardVariant.Outlined" Width="280px">
    ///     <SfCardHeader>
    ///         <SfCardTitle>Trail Runner</SfCardTitle>
    ///         <SfCardSubTitle>Outdoor</SfCardSubTitle>
    ///     </SfCardHeader>
    ///     <SfCardContent>Lightweight daily trainer.</SfCardContent>
    /// </SfCard>
    /// ]]></code>
    /// </example>
    public partial class SfCard : SfBaseComponent
    {
        #region Fields

        /// <exclude />
        private string _generatedId = string.Empty;

        /// <exclude />
        private string _resolvedId = string.Empty;

        /// <exclude />
        private string _rootClass = RootClass;

        /// <exclude />
        private string? _rootStyle;

        /// <exclude />
        private string? _role;

        /// <exclude />
        private int? _tabIndex;

        /// <exclude />
        private string? _ariaBusy;

        /// <exclude />
        private string? _ariaDisabled;

        /// <exclude />
        private string _elevationLevel = "0";

        /// <exclude />
        private string _contentPadding = TrueValue;

        /// <exclude />
        private Dictionary<string, object> _attributes = [];

        /// <exclude />
        private ElementReference _element;

        /// <exclude />
        private IJSObjectReference? _cardJsModule;

        /// <exclude />
        private IJSInProcessObjectReference? _cardInProcessModule;

        /// <exclude />
        private bool _keyboardAttached;

        /// <exclude />
        /// <summary>
        /// Gets whether the loading state is active; read by <see cref="SfCardActions"/> to make its content inert.
        /// </summary>
        internal bool IsBusy => IsLoading;

        #endregion

        #region Private Methods

        /// <exclude />
        /// <summary>
        /// Computes every value the markup renders, once per parameter update.
        /// </summary>
        private void BuildRenderState()
        {
            _resolvedId = string.IsNullOrWhiteSpace(Id) ? _generatedId : Id;
            _rootClass = BuildRootClass();
            _rootStyle = CardAttributes.MergeStyle(BuildComputedStyle(), HtmlAttributes);
            _role = IsClickable ? RoleButton : null;
            _tabIndex = IsClickable ? 0 : null;
            _ariaBusy = IsLoading ? TrueValue : null;
            _ariaDisabled = IsClickable && IsLoading ? TrueValue : null;
            _elevationLevel = ResolveElevation().ToString(CultureInfo.InvariantCulture);
            _contentPadding = ContentPadding ? TrueValue : FalseValue;
            _attributes = CardAttributes.Filter(HtmlAttributes);

            // The handler is attached only on a clickable card, so a static card costs no SignalR round trip per event.
            // Enter and Space are handled by card.js, which forwards them here as a native click.
            if (IsClickable)
            {
                _attributes[ClickEvent] = EventCallback.Factory.Create<MouseEventArgs>(this, HandleClickAsync);
            }
        }

        /// <exclude />
        /// <summary>
        /// Gets whether the card may currently raise <see cref="OnClick"/>.
        /// </summary>
        private bool CanActivate => IsClickable && !IsLoading;

        /// <exclude />
        /// <summary>
        /// Maps <see cref="Elevation"/> and <see cref="Variant"/> to the 0-5 shadow level rendered as <c>data-elevation</c>.
        /// </summary>
        private int ResolveElevation()
        {
            if (Variant is CardVariant.Outlined or CardVariant.Flat)
            {
                return 0;
            }

            int level = Math.Clamp(Elevation, 0, MaxElevation);
            if (level > 0 && Variant == CardVariant.Elevated)
            {
                level = Math.Min(MaxElevation, level + ElevatedVariantBoost);
            }

            return level;
        }

        /// <exclude />
        private string BuildRootClass()
        {
            StringBuilder builder = new(RootClass);

            if (Orientation == CardOrientation.Horizontal)
            {
                AppendClass(builder, HorizontalClass);
            }

            switch (Variant)
            {
                case CardVariant.Outlined:
                    AppendClass(builder, OutlinedClass);
                    break;
                case CardVariant.Elevated:
                    AppendClass(builder, ElevatedClass);
                    break;
                case CardVariant.Flat:
                    AppendClass(builder, FlatClass);
                    break;
                case CardVariant.Filled:
                default:
                    break;
            }

            if (Square)
            {
                AppendClass(builder, SquareClass);
            }

            if (Hoverable)
            {
                AppendClass(builder, HoverableClass);
            }

            if (IsLoading)
            {
                // The loading overlay is the only dim. e-disabled would stack a second 0.72 opacity on top of it.
                AppendClass(builder, LoadingClass);
            }

            if (IsClickable)
            {
                AppendClass(builder, ClickableClass);
            }

            if (SyncfusionService?._options?.EnableRtl ?? false)
            {
                AppendClass(builder, RtlClass);
            }

            return CardAttributes.MergeClass(builder.ToString(), CssClass, HtmlAttributes);
        }

        /// <exclude />
        private string? BuildComputedStyle()
        {
            StringBuilder builder = new();
            AppendSize(builder, "width:", Width);
            AppendSize(builder, "height:", Height);
            return builder.Length == 0 ? null : builder.ToString();
        }

        /// <exclude />
        /// <summary>
        /// Appends a size declaration, skipping it when the value is blank or rejected by <see cref="SfBaseUtils.FormatUnit"/>.
        /// </summary>
        private static void AppendSize(StringBuilder builder, string property, string? value)
        {
            string size = string.IsNullOrWhiteSpace(value) ? string.Empty : SfBaseUtils.FormatUnit(value);
            if (size.Length > 0)
            {
                _ = builder.Append(property).Append(size).Append(';');
            }
        }

        /// <exclude />
        private static void AppendClass(StringBuilder builder, string className)
        {
            _ = builder.Append(' ').Append(className);
        }

        /// <exclude />
        private async Task HandleClickAsync(MouseEventArgs args)
        {
            if (!CanActivate)
            {
                return;
            }

            await OnClick.InvokeAsync(args).ConfigureAwait(true);
        }

        /// <exclude />
        /// <summary>
        /// Loads card.js and attaches keyboard activation the first time the card renders as clickable.
        /// The listeners stay attached afterwards and check the clickable class, so turning <see cref="IsClickable"/> off and on again needs no further interop.
        /// </summary>
        private async Task AttachKeyboardAsync()
        {
            if (_keyboardAttached || !IsClickable || IsDisposed)
            {
                return;
            }

            _keyboardAttached = true;
            JsModuleReference module = await ImportModuleAsync(ScriptPath, _cardJsModule, _cardInProcessModule).ConfigureAwait(true);
            _cardJsModule = module.AsyncRef;
            _cardInProcessModule = module.InProcessRef;
            await InvokeVoidAsync(_cardJsModule, _cardInProcessModule, "initialize", _element).ConfigureAwait(true);
        }

        #endregion
    }
}