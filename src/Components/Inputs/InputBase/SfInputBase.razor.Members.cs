using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    public abstract partial class SfInputBase<TValue>
    {
        #region Public members

        /// <summary>
        /// Specifies the content that has to be passed.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Parameter]
        public RenderFragment ChildContent { get; set; } = default!;

        /// <summary>
        /// Gets or sets the ID of the component.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the identifier of the component. The default value is auto-generated.
        /// </value>
        /// <remarks>
        /// The ID property allows the component to be uniquely identified in the rendered HTML markup.
        /// If a custom ID is not set, the component's ID will be auto-generated, and it
        /// may not be as descriptive as required.
        /// </remarks>
        [Parameter]
        public string ID { get; set; } = default!;

        /// <summary>
        /// Gets or sets the current value of the input component.
        /// </summary>
        /// <value>
        /// A value of type <typeparamref name="TValue"/> representing the current input value. The default value depends on the type <typeparamref name="TValue"/>.
        /// </value>
        /// <remarks>
        /// This property supports two-way data binding and will automatically trigger the ValueChanged event when modified.
        /// The value type is determined by the generic type parameter <typeparamref name="TValue"/>.
        /// </remarks>
        [Parameter]
        public TValue? Value { get; set; }

        /// <summary>
        /// Gets or sets the expression that identifies the bound value for validation and form binding purposes.
        /// </summary>
        /// <value>
        /// An <see cref="Expression{TDelegate}"/> representing the property or field being bound to this component. The default value is <see langword="null"/>.
        /// </value>
        /// <remarks>
        /// This expression is used by the Blazor form validation system to identify which property is being validated.
        /// It is automatically set when using the @bind-Value directive in Razor components.
        /// </remarks>
        /// <exclude/>
        [Parameter]
        public Expression<Func<TValue>>? ValueExpression { get; set; }

        /// <summary>
        /// Gets or sets a CSS class string to customize the appearance of the component.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing CSS class names separated by spaces to customize the appearance. The default value is <see langword="null"/>.
        /// </value>
        [Parameter]
        public string CssClass { get; set; } = default!;

        /// <summary>
        /// Gets or sets whether to persist component's state between page reloads. When set to <see langword="true"/>, the <see cref="Value"/> property is persisted.
        /// </summary>
        /// <value>
        /// A <see cref="bool"/> indicating whether the component's state persistence is enabled. The default value is <see langword="false"/>.
        /// </value>
        /// <remarks>
        /// The <see cref="Value"/> property will be stored in browser local storage to persist component's state when page reloads.
        /// </remarks>
        [Parameter]
        public bool EnablePersistence { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether the component is disabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if the component is disabled; otherwise, <c>false</c>.
        /// The default value is <c>false</c>.
        /// </value>
        [Parameter]
        public bool Disabled { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether validation should be performed on each input change rather than only on blur.
        /// </summary>
        /// <value>
        /// A <see cref="bool"/> indicating whether validation should be performed during typing. The default value is <see langword="false"/>.
        /// </value>
        /// <remarks>
        /// <para>When set to <see langword="true"/>, this property enables real-time validation as the user types, providing immediate feedback.</para>
        /// <para>When <see langword="false"/> (default behavior), validation occurs only when the component loses focus, which is the standard form validation pattern.</para>
        /// <para>Note: The ValueChanged event will still be fired after the component loses focus regardless of this setting.</para>
        /// </remarks>
        [Parameter]
        public bool ValidateOnInput { get; set; }

        #endregion
    }
}
