using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Toolkit.Internal;
using System.Globalization;
using System.Text;

namespace Syncfusion.Blazor.Toolkit.Buttons
{
    /// <summary> 
    /// Represents a graphical user interface <see cref="ComponentBase"/> that triggers events when clicked, supporting text, icons, SVG, or a combination as its content.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Button content can be set using the <see cref="Content"/> property for simple strings or by embedding content within the <see cref="SfButton"/> tag. 
    /// Both text and HTML elements (such as icons or SVG) are supported, allowing for a flexible UI experience.
    /// </para>
    /// <para>
    /// <strong>Security:</strong> This component is XSS-safe through Blazor's automatic HTML encoding. User-provided content in <see cref="Content"/> is automatically escaped.
    /// When using <see cref="ChildContent"/>, ensure you trust the source of any HTML to prevent XSS vulnerabilities.
    /// </para>
    /// <para>
    /// <strong>Performance:</strong> CSS class composition is optimized using StringBuilder to minimize memory allocations during parameter updates.
    /// </para>
    /// </remarks>
    /// <example>
    /// The following code example demonstrates a basic button initialized with the <see cref="Content"/> property.
    /// <code><![CDATA[
    /// <SfButton Content="Click" />
    /// ]]></code>
    /// </example>
    public partial class SfButton
    {
        #region Constants
        /// <exclude />
        /// <summary>
        /// Space character used for CSS class concatenation.
        /// </summary>
        private const char Space = ' ';

        /// <exclude />
        /// <summary>
        /// CSS class for right-to-left (RTL) direction support.
        /// </summary>
        private const string RtlClass = "e-rtl";

        /// <exclude />
        /// <summary>
        /// CSS class applied when the button is in an active/toggled state.
        /// </summary>
        private const string ActiveClass = "e-active";

        /// <exclude />
        /// <summary>
        /// CSS class for primary button styling.
        /// </summary>
        private const string PrimaryClass = "e-primary";

        /// <exclude />
        /// <summary>
        /// CSS class for button icon elements.
        /// </summary>
        private const string IconClass = "e-btn-icon";

        /// <exclude />
        /// <summary>
        /// Root CSS classes applied to all button instances.
        /// </summary>
        private const string RootCss = "e-control e-btn e-lib";
        #endregion

        #region Fields

        /// <summary>
        /// Gets or sets additional HTML attributes applied to the root button element.
        /// Captures unmatched attributes like <c>title</c>, <c>data-*</c>, and other standard or custom attributes.
        /// </summary>
        /// <value>
        /// A <see cref="Dictionary{String, Object}"/> containing attribute name-value pairs. The default is an empty dictionary.
        /// </value>
        /// <remarks>
        /// This property supports Blazor's component parameter binding mechanism. Use it to pass native HTML attributes not exposed as component parameters.
        /// <para>When the <c>class</c> attribute is specified, its value is merged with the component's computed CSS classes.</para>
        /// </remarks>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> HtmlAttributes { get => _htmlAttributes; set => _htmlAttributes = value; }

        /// <exclude />
        /// <summary>
        /// Computed CSS classes for icon positioning (for example, "e-btn-icon e-icon-left").
        /// </summary>
        internal string _iconCssModifier = string.Empty;

        /// <exclude />
        /// <summary>
        /// Fully computed CSS class string applied to the button element at render time.
        /// </summary>
        internal string _buttonCssClass = string.Empty;

        /// <exclude />
        /// <summary>
        /// Tracks the active/toggled state for toggle buttons to support ARIA attributes and visual feedback.
        /// </summary>
        private bool _isActive;

        /// <exclude />
        private Dictionary<string, object> _htmlAttributes = [];
        #endregion


        #region Private Methods
        /// <exclude />
        /// <summary>
        /// Initializes and composes the CSS classes and state used for rendering.
        /// </summary>
        /// <remarks>
        /// Uses <see cref="StringBuilder"/> for efficient string concatenation during CSS class composition,
        /// reducing memory allocations compared to standard string concatenation.
        /// </remarks>
        private void InitRender()
        {
            StringBuilder buttonCss = new(RootCss);

            ApplyToggleState(buttonCss);
            ApplyCustomClasses(buttonCss);
            ApplyPrimary(buttonCss);
            ApplyRtl(buttonCss);
            ApplyIconClasses(buttonCss);
            MergeUserDefinedClasses(buttonCss);
            _buttonCssClass = buttonCss.ToString();
        }

        /// <exclude />
        /// <summary>
        /// Applies the toggle state based on current settings.
        /// </summary>
        private void ApplyToggleState(StringBuilder buttonCss)
        {
            if (IsToggle && _isActive)
            {
                _ = buttonCss.Append(Space);
                _ = buttonCss.Append(ActiveClass);
            }
            else if (!IsToggle)
            {
                _isActive = false;
            }
        }

        /// <exclude />
        /// <summary>
        /// Applies any custom CSS classes passed via <see cref="CssClass"/>.
        /// </summary>
        private void ApplyCustomClasses(StringBuilder buttonClass)
        {
            if (!string.IsNullOrWhiteSpace(CssClass))
            {
                _ = buttonClass.Append(Space);
                _ = buttonClass.Append(CssClass);
            }
        }

        /// <exclude />
        /// <summary>
        /// Applies the primary style when <see cref="IsPrimary"/> is enabled.
        /// </summary>
        private void ApplyPrimary(StringBuilder buttonCss)
        {
            if (IsPrimary)
            {
                _ = buttonCss.Append(Space);
                _ = buttonCss.Append(PrimaryClass);
            }
        }

        /// <exclude />
        /// <summary>
        /// Applies RTL classes based on component or global options.
        /// </summary>
        private void ApplyRtl(StringBuilder buttonCss)
        {
            if (SyncfusionService?._options?.EnableRtl ?? false)
            {
                _ = buttonCss.Append(Space);
                _ = buttonCss.Append(RtlClass);
            }
        }

        /// <exclude />
        /// <summary>
        /// Applies icon-related classes and layout classes when icon and position are specified.
        /// </summary>
        private void ApplyIconClasses(StringBuilder buttonCss)
        {
            if (!string.IsNullOrEmpty(IconCss))
            {
                _iconCssModifier = IconClass;
                if (string.IsNullOrEmpty(Content) && ChildContent is null)
                {
                    _ = buttonCss.Append(Space);
                    _ = buttonCss.Append(IconClass);
                }
                else
                {
                    string iconPosition = IconPosition.ToString().ToLower(CultureInfo.InvariantCulture);
                    _iconCssModifier += " e-icon-" + iconPosition;
                    if (IconPosition is IconPosition.Top or IconPosition.Bottom)
                    {
                        _ = buttonCss.Append(Space);
                        _ = buttonCss.Append("e-");
                        _ = buttonCss.Append(iconPosition);
                        _ = buttonCss.Append("-icon-btn");
                    }
                }
            }
        }

        /// <exclude />
        /// <summary>
        /// Merges user-defined CSS classes from <see cref="HtmlAttributes"/>.
        /// </summary>
        private void MergeUserDefinedClasses(StringBuilder buttonCss)
        {
            if (HtmlAttributes.TryGetValue("class", out object? valueClass) && valueClass is not null)
            {
                _ = buttonCss.Append(Space);
                _ = buttonCss.Append(valueClass.ToString());
            }
        }

        /// <exclude />
        /// <summary>
        /// Toggles the active state for toggle buttons.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The active state is the single source of truth. CSS classes are a projection
        /// computed during the next render cycle and should not be mutated here.
        /// </para>
        /// </remarks>
        private void ToggleActiveState()
        {
            _isActive = !_isActive;
        }

        /// <exclude />
        /// <summary>
        /// Returns the ARIA disabled attribute value based on the button's disabled state.
        /// </summary>
        /// <returns><c>true</c> if the button is disabled; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// This method ensures the <c>aria-disabled</c> attribute always has a valid string value rather than <see langword="null"/>.
        /// </remarks>
        private string GetAriaDisabled()
        {
            return Disabled ? "true" : "false";
        }

        /// <exclude />
        /// <summary>
        /// Returns the ARIA pressed attribute value for toggle buttons.
        /// </summary>
        /// <returns><c>true</c> or <c>false</c> when <see cref="IsToggle"/> is enabled; otherwise, <see langword="null"/> to omit the attribute.</returns>
        private string? GetAriaPressed()
        {
            if (!IsToggle)
            {
                return null;
            }
            return _isActive ? "true" : "false";
        }


        #endregion

        #region Event Handlers
        /// <exclude />
        /// <summary>
        /// Handles click events, toggles state when applicable, and raises <see cref="OnClick"/>.
        /// </summary>
        /// <param name="args">The mouse event arguments from the UI interaction.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task HandleClickAsync(MouseEventArgs args)
        {
            // Prevent any action when disabled (bUnit simulates clicks anyway).
            if (Disabled)
            {
                return;
            }
            if (IsToggle)
            {
                ToggleActiveState();
            }
            await SfBaseUtils.InvokeEventAsync(OnClick, args).ConfigureAwait(false);
        }
        #endregion
    }
}
