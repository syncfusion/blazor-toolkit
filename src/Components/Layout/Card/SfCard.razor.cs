using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
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
        private bool _spaceArmed;

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

            // Handlers are attached only on a clickable card, so a static card costs no SignalR round trip per event.
            if (IsClickable)
            {
                _attributes[ClickEvent] = EventCallback.Factory.Create<MouseEventArgs>(this, HandleClickAsync);
                _attributes[KeyDownEvent] = EventCallback.Factory.Create<KeyboardEventArgs>(this, HandleKeyDownAsync);
                _attributes[KeyUpEvent] = EventCallback.Factory.Create<KeyboardEventArgs>(this, HandleKeyUpAsync);
                _attributes[KeyDownPreventDefault] = (Func<KeyboardEventArgs, bool>)IsSpaceOrEnter;
                _attributes[KeyUpPreventDefault] = (Func<KeyboardEventArgs, bool>)IsSpaceKey;
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
                AppendClass(builder, LoadingClass);
                AppendClass(builder, DisabledClass);
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

            if (!string.IsNullOrWhiteSpace(Width))
            {
                _ = builder.Append("width:").Append(SfBaseUtils.FormatUnit(Width.Trim())).Append(';');
            }

            if (!string.IsNullOrWhiteSpace(Height))
            {
                _ = builder.Append("height:").Append(SfBaseUtils.FormatUnit(Height.Trim())).Append(';');
            }

            return builder.Length == 0 ? null : builder.ToString();
        }

        /// <exclude />
        private static void AppendClass(StringBuilder builder, string className)
        {
            _ = builder.Append(' ').Append(className);
        }

        /// <exclude />
        private static bool IsSpaceOrEnter(KeyboardEventArgs args)
        {
            return string.Equals(args.Key, EnterKey, StringComparison.Ordinal) || IsSpace(args);
        }

        /// <exclude />
        private static bool IsSpaceKey(KeyboardEventArgs args)
        {
            return IsSpace(args);
        }

        /// <exclude />
        private static bool IsSpace(KeyboardEventArgs args)
        {
            return string.Equals(args.Key, SpaceKey, StringComparison.Ordinal) ||
                   string.Equals(args.Code, SpaceCode, StringComparison.Ordinal);
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
        /// Enter activates on key down; Space is armed here and activates on key up, as a native button does.
        /// Tab and every other key are left untouched so focus is never trapped.
        /// </summary>
        private async Task HandleKeyDownAsync(KeyboardEventArgs args)
        {
            if (!CanActivate || args.Repeat)
            {
                return;
            }

            if (string.Equals(args.Key, EnterKey, StringComparison.Ordinal))
            {
                await OnClick.InvokeAsync(new MouseEventArgs { Detail = 0, Type = args.Type }).ConfigureAwait(true);
            }
            else if (IsSpace(args))
            {
                _spaceArmed = true;
            }
        }

        /// <exclude />
        private async Task HandleKeyUpAsync(KeyboardEventArgs args)
        {
            if (!IsSpace(args) || !_spaceArmed)
            {
                return;
            }

            _spaceArmed = false;
            if (CanActivate)
            {
                await OnClick.InvokeAsync(new MouseEventArgs { Detail = 0, Type = args.Type }).ConfigureAwait(true);
            }
        }

        #endregion
    }
}
