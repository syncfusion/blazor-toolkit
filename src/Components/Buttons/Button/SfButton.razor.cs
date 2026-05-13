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
        /// <summary>
        /// Space character used for CSS class concatenation.
        /// </summary>
        private const char Space = ' ';

        /// <summary>
        /// CSS class for right-to-left (RTL) direction support.
        /// </summary>
        private const string RtlClass = "e-rtl";

        /// <summary>
        /// CSS class applied when the button is in an active/toggled state.
        /// </summary>
        private const string ActiveClass = "e-active";

        /// <summary>
        /// CSS class for primary button styling.
        /// </summary>
        private const string PrimaryClass = "e-primary";

        /// <summary>
        /// CSS class for button icon elements.
        /// </summary>
        private const string IconClass = "e-btn-icon";

        /// <summary>
        /// Root CSS classes applied to all button instances.
        /// </summary>
        private const string RootCss = "e-control e-btn e-lib";
        #endregion

        #region Fields

        /// <summary>
        /// Additional HTML attributes applied to the root button element.
        /// Captures unmatched attributes like `title`, `data-*`, etc.
        /// </summary>
        /// <remarks>
        /// This property has a public setter to support Blazor's component parameter binding mechanism,
        /// which requires writable properties for parameter updates. This is an architectural requirement
        /// of the Blazor framework and is not considered a code smell in this context.
        /// </remarks>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> HtmlAttributes { get => _htmlAttributes; set => _htmlAttributes = value; }

        /// <summary>
        /// Computed CSS classes for icon positioning (e.g., "e-btn-icon e-icon-left").
        /// </summary>
        internal string _iconCssModifier = string.Empty;

        /// <summary>
        /// Fully computed CSS class string applied to the button element at render time.
        /// </summary>
        internal string _buttonCssClass = string.Empty;

        /// <summary>
        /// Tracks the active/toggled state for toggle buttons to support ARIA attributes and visual feedback.
        /// </summary>
        private bool _isActive;

        private Dictionary<string, object> _htmlAttributes = [];
        #endregion


        #region Private Methods
        /// <summary>
        /// Initializes and composes the CSS classes and state used for rendering.
        /// </summary>
        /// <remarks>
        /// Performance: Uses StringBuilder for efficient string concatenation during CSS class composition,
        /// reducing memory allocations compared to string concatenation.
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

        /// <summary>
        /// Toggles the active state for toggle buttons.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The active state (`_isActive`) is the single source of truth. CSS classes are a projection
        /// computed during the next render cycle by `InitRender()` and should not be mutated here.
        /// </para>
        /// </remarks>
        private void ToggleActiveState()
        {
            _isActive = !_isActive;
        }

        /// <summary>
        /// Returns the ARIA disabled attribute value based on the button's disabled state.
        /// </summary>
        /// <returns>"true" if the button is disabled; otherwise, "false".</returns>
        /// <remarks>
        /// This method ensures the aria-disabled attribute always has a valid string value rather than null.
        /// </remarks>
        private string GetAriaDisabled()
        {
            return Disabled ? "true" : "false";
        }

        // Returns "true"/"false" only for toggle buttons; otherwise omits the attribute.
        private string? GetAriaPressed()
        {
            if (!IsToggle)
            {
                return null; // Blazor omits the attribute when null
            }
            return _isActive ? "true" : "false";
        }


        #endregion

        #region Event Handlers
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
