using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Syncfusion.Blazor.Toolkit.Internal;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    /// <summary>
    /// Provides a base class for form _input components in Syncfusion Blazor, encapsulating common functionality and API contracts for _input controls with checked/value states.
    /// </summary>
    /// <remarks>
    /// This abstract class defines core implementation for form-oriented _input elements—such as checkboxes and radio buttons—offering consistent property contracts for Checked/Value/CssClass/Name, support for form integration, RTL rendering, persistence, and extensible rendering. Component developers should inherit this base to ensure seamless interop, accessibility, and persistence with Syncfusion’s form controls.
    /// </remarks>
    /// <typeparam name="TChecked">The type of the checked value (typically <see cref="bool"/> for checkboxes or <see cref="string"/> for radio buttons).</typeparam>
    /// <example>
    /// See <c>SfCheckBox</c> or <c>SfRadioButton</c> for real-world usage and parameter pattern derived from <see cref="SfSelectionBase{TChecked}"/>.
    /// </example>
    public abstract partial class SfSelectionBase<TChecked> : SfBaseComponent
    {
        #region Constants

        /// <summary>
        /// Property name constant for the Checked property, used in change notifications.
        /// </summary>
        private const string CHECKED = "Checked";

        #endregion

        #region Fields

        /// <summary>
        /// Cascading parameter providing the edit context for form integration and validation.
        /// </summary>
        [CascadingParameter]
        private EditContext? CascadedEditContext { get; set; }

        /// <summary>
        /// Stores the previous checked value for change detection and comparison.
        /// </summary>
        private TChecked? PreviousCheckedValue { get; set; }

        #endregion

        #region Protected Fields

        /// <summary>
        /// Indicates whether the component has a native onchange event handler attached via HTML attributes.
        /// </summary>
        protected bool HasOnChangeEvent { get; set; }

        #endregion

        #region Internal Fields

        /// <summary>
        /// Stores the component's ID value, extracted from HTML attributes or generated.
        /// </summary>
        internal string _idValue = string.Empty;

        /// <summary>
        /// Reference to the underlying _input HTML element.
        /// </summary>
        internal ElementReference _input;

        /// <summary>
        /// Reference to the container HTML element.
        /// </summary>
        internal ElementReference _container;

        /// <summary>
        /// Dictionary containing additional HTML attributes to be applied to the component.
        /// </summary>
        internal Dictionary<string, object> _inputAttributes = [];

        #endregion

        #region Non Browsable Members

        /// <exclude/>
        /// <summary>
        /// Gets or sets the child content to be rendered inside this _input component, typically containing <c>HTML</c> elements.
        /// </summary>
        [Parameter]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public RenderFragment? ChildContent { get; set; }

        /// <exclude/>
        /// <summary>
        /// Gets or sets the expression that identifies the Checked property for two-way binding.
        /// </summary>
        [Parameter]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Expression<Func<TChecked>>? CheckedExpression { get; set; }

        #endregion

        #region Private Methods

        /// <summary>
        /// Updates internal HTML attributes by extracting the ID value from the HtmlAttributes dictionary.
        /// </summary>
        private void UpdateHTMLAttributes()
        {
            if (HtmlAttributes != null && HtmlAttributes.TryGetValue("id", out object? idAttribute))
            {
                _idValue = idAttribute?.ToString() ?? string.Empty;
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Updates the Checked state through EditContext and notifies two-way binding.
        /// </summary>
        /// <param name="state">The new checked value.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        protected async Task UpdateCheckStateAsync(TChecked state)
        {
            Checked = PreviousCheckedValue = await SfBaseUtils.UpdatePropertyAsync(
                state,
                PreviousCheckedValue,
                CheckedChanged!,
                CascadedEditContext,
                CheckedExpression!).ConfigureAwait(true);
        }

        /// <summary>
        /// Called by derived classes to initialize the visual state. Override to compute classes.
        /// </summary>
        /// <param name="isDynamic">True when the checked value changed dynamically.</param>
        protected virtual void InitRender(bool isDynamic = false)
        {
            // Base implementation intentionally empty - derived classes override for custom behavior.
        }

        /// <summary>
        /// Persists the checked state to browser's localStorage.
        /// </summary>
        /// <param name="persistId">The storage key identifier.</param>
        /// <param name="checkedValue">The checked value to persist.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected async Task SetLocalStorageAsync(string persistId, TChecked checkedValue)
        {
            await InvokeVoidAsync(_baseJsModule, _baseJsInProcessModule, "setLocalStorageItem", [persistId, checkedValue!]).ConfigureAwait(true);
        }

        #endregion
    }
}
