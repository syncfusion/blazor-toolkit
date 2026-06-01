using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Toolkit.Internal;
using System.ComponentModel;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using System.Collections.ObjectModel;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    /// <summary>
    /// Represents the base class for text-based input components in the <see cref="Toolkit"/> library.
    /// </summary>
    /// <typeparam name="TValue">The type of the value representing the input data.</typeparam>
    /// <remarks>
    /// This abstract class provides the foundational functionality for text-based input components in the Blazor Toolkit suite.
    /// It handles common input operations such as validation, floating labels, clear button functionality, and accessibility features.
    /// </remarks>
    public abstract partial class SfInputBase<TValue> : SfBaseComponent
    {
        #region Protected members

        /// <summary>
        /// Gets or sets the internal backing field for the component's value.
        /// </summary>
        /// <value>
        /// A nullable value of type <typeparamref name="TValue"/> used for internal state management.
        /// </value>
        /// <remarks>
        /// This field is used internally to track value changes and should not be accessed directly from external code.
        /// </remarks>
        /// <exclude/>
        protected TValue? InternalValue { get; set; }

        /// <summary>
        /// Gets or sets the edit context used for form validation and field identification.
        /// </summary>
        /// <value>
        /// An <see cref="EditContext"/> instance that provides validation context for the input component.
        /// </value>
        /// <remarks>
        /// This cascading parameter is automatically provided by parent form components such as EditForm.
        /// It enables integration with Blazor's built-in validation system.
        /// </remarks>
        /// <exclude/>
        [CascadingParameter]
        protected EditContext? InputEditContext { get; set; }

        /// <summary>
        /// Gets or sets the floating label behavior of the input component that determines how the placeholder text is displayed.
        /// </summary>
        /// <value>
        /// A <see cref="FloatLabelType"/> enumeration value that specifies the floating label behavior. The default value depends on the specific input component implementation.
        /// </value>
        /// <remarks>
        /// <para>The floating label behavior controls how the placeholder text is presented to the user:</para>
        /// <list type="bullet">
        /// <item>
        /// <term><see cref="FloatLabelType.Never"/></term>
        /// <description>The label never floats and remains as a standard placeholder text within the input field.</description>
        /// </item>
        /// <item>
        /// <term><see cref="FloatLabelType.Always"/></term>
        /// <description>The label is always positioned above the input field, providing a consistent visual layout.</description>
        /// </item>
        /// <item>
        /// <term><see cref="FloatLabelType.Auto"/></term>
        /// <description>The label dynamically floats above the input field when the component receives focus or contains a value, creating an animated transition effect.</description>
        /// </item>
        /// </list>
        /// </remarks>
        /// <exclude/>
        protected virtual FloatLabelType BaseFloatLabelType { get; set; }

        /// <summary>
        /// Gets or sets additional HTML attributes to be applied to the input component's container element.
        /// </summary>
        /// <value>
        /// A <see cref="Dictionary{String, Object}"/> containing key-value pairs of HTML attributes, or <c>null</c> if no additional attributes are specified.
        /// </value>
        /// <remarks>
        /// Use this property to add custom HTML attributes such as data attributes, accessibility attributes, or styling classes to the component's container.
        /// Common attributes like 'class', 'style', and 'title' are handled specially and may be applied to different elements within the component structure.
        /// </remarks>
        /// <exclude/>
        protected virtual Dictionary<string, object>? BaseHtmlAttributes { get; set; }

        /// <summary>
        /// Gets or sets additional HTML attributes to be applied specifically to the input element itself.
        /// </summary>
        /// <value>
        /// A <see cref="Dictionary{String, Object}"/> containing key-value pairs of HTML attributes. The default value is an empty dictionary.
        /// </value>
        /// <remarks>
        /// This property allows direct customization of the underlying input element with additional HTML attributes.
        /// Attributes set here will be applied directly to the input tag, enabling fine-grained control over input behavior and appearance.
        /// </remarks>
        /// <exclude/>
        protected virtual Dictionary<string, object>? BaseInputAttributes { get; set; } = [];

        /// <summary>
        /// Gets or sets a value indicating whether the input component supports multiple lines of text.
        /// </summary>
        /// <value>
        /// A <see cref="bool"/> indicating whether the input supports multiple lines. The default value is <see langword="false"/> but may be overridden in derived implementations.
        /// </value>
        /// <remarks>
        /// This property determines whether the component renders as a single-line input or a multi-line textarea element.
        /// When <see langword="true"/>, additional styling and behavior for multi-line text handling will be applied.
        /// </remarks>
        /// <exclude/>
        protected virtual bool MultilineInput { get; set; }

        /// <summary>
        /// Gets or sets the placeholder text displayed in the input when no value is present.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the placeholder text. The default value is <see langword="null"/> but may be overridden in derived implementations.
        /// </value>
        /// <remarks>
        /// The placeholder provides a hint about the expected input format or content.
        /// This text is automatically hidden when the input receives focus or contains a value.
        /// </remarks>
        /// <exclude/>
        protected virtual string BasePlaceholder { get; set; } = default!;

        /// <summary>
        /// Gets or sets a value indicating whether the input component is in read-only mode.
        /// </summary>
        /// <value>
        /// A <see cref="bool"/> indicating whether the input is read-only. The default value is <see langword="false"/> but may be overridden in derived implementations.
        /// </value>
        /// <remarks>
        /// When in read-only mode, content can be viewed and selected but cannot be modified.
        /// The input will still receive focus and trigger events, but input will be prevented.
        /// </remarks>
        /// <exclude/>
        protected virtual bool BaseReadOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the input element itself should be marked as read-only.
        /// </summary>
        /// <value>
        /// A <see cref="bool"/> indicating whether the input element is read-only. The default value is <see langword="false"/> but may be overridden in derived implementations.
        /// </value>
        /// <remarks>
        /// This property specifically controls the readonly attribute on the underlying HTML input element,
        /// which may behave differently from the component-level <see cref="BaseReadOnly"/> property.
        /// </remarks>
        /// <exclude/>
        protected virtual bool BaseIsReadOnlyInput { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a clear button should be displayed for quickly clearing the input value.
        /// </summary>
        /// <value>
        /// A <see cref="bool"/> indicating whether the clear button is shown. The default value is <see langword="false"/> but may be overridden in derived implementations.
        /// </value>
        /// <remarks>
        /// When enabled, a clear button appears within the input field when it contains a value, enabling quick content clearing.
        /// The button is automatically hidden when the input is empty, disabled, or readonly.
        /// </remarks>
        /// <exclude/>
        protected virtual bool BaseShowClearButton { get; set; }

        /// <summary>
        /// Gets or sets the width of the input component.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the width in CSS units (e.g., "100px", "50%"). The default value is <see langword="null"/> but may be overridden in derived implementations.
        /// </value>
        /// <remarks>
        /// This property accepts any valid CSS width value including pixels, percentages, em units, etc.
        /// The width is applied to the component's container element to control the overall size.
        /// </remarks>
        /// <exclude/>
        protected virtual string? BaseWidth { get; set; }

        /// <summary>
        /// Gets or sets the tab index of the input component for keyboard navigation.
        /// </summary>
        /// <value>
        /// An <see cref="int"/> representing the tab order. The default value is <c>0</c> but may be overridden in derived implementations. Positive values indicate the explicit tab order, 0 means the element is focusable in document order, and negative values remove the element from tab navigation.
        /// </value>
        /// <remarks>
        /// This property controls the order in which the component receives focus during keyboard navigation.
        /// When the component is disabled, the tab index is automatically set to -1 to remove it from the tab sequence.
        /// </remarks>
        /// <exclude/>
        protected virtual int BaseTabIndex { get; set; }

        /// <summary>
        /// Gets or sets the autocomplete behavior for the input component.
        /// </summary>
        /// <value>
        /// A string representing the autocomplete attribute value. The default value is "off".
        /// </value>
        /// <remarks>
        /// This property controls how browsers handle autocomplete functionality for the input field.
        /// Common values include "off" (disable autocomplete), "on" (enable autocomplete), or specific autocomplete tokens like "email", "name", etc.
        /// </remarks>
        /// <exclude/>
        protected virtual string BaseAutocomplete { get; set; } = "off";

        /// <summary>
        /// Gets or sets the HTML attributes to be applied to the input component's container element.
        /// </summary>
        /// <value>
        /// A <see cref="Dictionary{String, Object}"/> containing key-value pairs of HTML attributes. The default value is an empty dictionary.
        /// </value>
        /// <remarks>
        /// This dictionary contains attributes that will be applied to the main container element of the input component.
        /// It is used internally to manage container-level styling, accessibility attributes, and other HTML properties.
        /// </remarks>
        /// <exclude/>
        protected Dictionary<string, object> ContainerHtmlAttributes { get; set; } = [];

        /// <summary>
        /// Gets or sets a value indicating whether spin buttons (increment/decrement) are enabled for numeric input components.
        /// </summary>
        /// <value>
        /// A <see cref="bool"/> indicating whether spin buttons are enabled. The default value is <see langword="true"/>.
        /// </value>
        /// <remarks>
        /// This property is primarily used by numeric input components to control the visibility and functionality of spin buttons.
        /// When disabled, values can still be modified through direct input but not through the spin button interface.
        /// </remarks>
        /// <exclude/>
        protected bool SpinButton { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether a value template is currently being displayed in the input component.
        /// </summary>
        /// <value>
        /// A <see cref="bool"/> indicating whether a value template is being displayed. The default value is <see langword="false"/>.
        /// </value>
        /// <remarks>
        /// This flag is used internally to track the template display state and coordinate between template rendering and standard text display modes.
        /// </remarks>
        /// <exclude/> 
        protected bool IsValueTemplate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether click event propagation should be prevented for certain elements.
        /// </summary>
        /// <value>
        /// A <see cref="bool"/> indicating whether click propagation should be stopped. The default value is <see langword="false"/>.
        /// </value>
        /// <remarks>
        /// This property is used internally to control event bubbling behavior, particularly for preventing unwanted click events
        /// from propagating to parent elements during specific interactions.
        /// </remarks>
        /// <exclude/> 
        protected bool ClickStopPropagation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether mouse down events on spinner elements should be prevented.
        /// </summary>
        /// <value>
        /// A <see cref="bool"/> indicating whether mouse down events on spinners should be prevented. The default value is <see langword="false"/>.
        /// </value>
        /// <remarks>
        /// This property is used internally to control mouse event handling on spinner buttons,
        /// preventing unwanted interactions during specific component states or operations.
        /// </remarks>
        /// <exclude/> 
        protected bool MouseDownSpinnerPrevent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether icon click handlers should be prevented from executing.
        /// </summary>
        /// <value>
        /// A <see cref="bool"/> indicating whether icon handlers should be prevented. The default value is <see langword="false"/>.
        /// </value>
        /// <remarks>
        /// This property is used internally to temporarily disable icon interactions during specific component operations
        /// or state transitions to prevent conflicting actions.
        /// </remarks>
        /// <exclude/> 
        protected bool PreventIconHandler { get; set; }

        /// <summary>
        /// Gets or sets the collection of button groups associated with the input component.
        /// </summary>
        /// <value>
        /// A <see cref="List{ButtonGroups}"/> containing button configuration objects, or <c>null</c> if no buttons are configured.
        /// </value>
        /// <remarks>
        /// This property maintains a list of button configurations that can be added to the input component,
        /// such as clear buttons, dropdown buttons, or custom action buttons with their respective icons and event handlers.
        /// </remarks>
        /// <exclude/>
        protected Collection<ButtonGroups>? ListOfButtons { get; set; }

        /// <summary>
        /// Gets or sets the data identifier used for internal component operations and event handling.
        /// </summary>
        /// <value>
        /// A string representing the data identifier, or <c>null</c> if no identifier is specified.
        /// </value>
        /// <remarks>
        /// This property is used internally for component identification in JavaScript interop operations
        /// and for associating DOM elements with their corresponding component instances.
        /// </remarks>
        /// <exclude/>
        protected string DataId { get; set; } = default!;

        /// <summary>
        /// Gets or sets a value indicating whether the input event binding is enabled.
        /// </summary>
        /// <value>
        /// A <see cref="bool"/> indicating whether input event binding is enabled. The default value is <see langword="false"/>.
        /// </value>
        /// <remarks>
        /// This property controls whether the component should bind to input events for real-time value updates.
        /// </remarks>
        /// <exclude/>
        protected bool IsBindInputEvent { get; set; }

        /// <summary>
        /// Gets or sets the collection of HTML attributes to be applied to the input element.
        /// </summary>
        /// <value>
        /// A <see cref="Dictionary{String, Object}"/> containing HTML attribute key-value pairs. The default value is an empty dictionary.
        /// </value>
        /// <remarks>
        /// This dictionary is used internally to manage all HTML attributes that will be rendered on the input element,
        /// including dynamically calculated attributes for styling, accessibility, and behavior.
        /// </remarks>
        /// <exclude/>
        protected Dictionary<string, object> InputHtmlAttributes { get; set; } = [];

        /// <summary>
        /// Gets or sets the list of HTML attribute names that should be applied to the container element rather than the input element.
        /// </summary>
        /// <value>
        /// A <see cref="List{String}"/> containing attribute names. The default value includes "title", "style", and "class".
        /// </value>
        /// <remarks>
        /// This list is used during attribute processing to determine which custom attributes should be applied to the container
        /// versus the input element, ensuring proper HTML structure and styling inheritance.
        /// </remarks>
        /// <exclude/>
        protected Collection<string>? ContainerAttributes { get; set; } = ["title", "style", "class"];

        /// <summary>
        /// Gets or sets the CSS class string applied to the floating label element.
        /// </summary>
        /// <value>
        /// A string containing CSS classes that control the floating label's appearance and position.
        /// </value>
        /// <remarks>
        /// This property manages the CSS classes that control floating label animations and positioning,
        /// including classes for top/bottom positioning and transition effects.
        /// </remarks>
        /// <exclude/>
        protected string FloatLabel { get; set; } = default!;

        /// <summary>
        /// Gets or sets a value indicating whether the input component currently has focus.
        /// </summary>
        /// <value>
        /// A <see cref="bool"/> indicating whether the component has focus. The default value is <see langword="false"/>.
        /// </value>
        /// <remarks>
        /// This property tracks the focus state of the component and is used to manage focus-dependent behaviors
        /// such as floating label animations, clear button visibility, and styling updates.
        /// </remarks>
        /// <exclude/>
        protected bool IsFocused { get; set; }

        /// <summary>
        /// Gets or sets the tooltip text for the increment button in numeric input components.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the localized tooltip text for the increment button. The default value is <see langword="null"/>.
        /// </value>
        /// <remarks>
        /// This property holds the localized tooltip text displayed when hovering over the increment spinner button,
        /// enhancing accessibility and experience.
        /// </remarks>
        /// <exclude/>
        protected string? IncrementTitle { get; set; }

        /// <summary>
        /// Gets or sets the tooltip text for the decrement button in numeric input components.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the localized tooltip text for the decrement button. The default value is <see langword="null"/>.
        /// </value>
        /// <remarks>
        /// This property holds the localized tooltip text displayed when hovering over the decrement spinner button,
        /// enhancing accessibility and experience.
        /// </remarks>
        /// <exclude/>
        protected string? DecrementTitle { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the clear button was recently clicked.
        /// </summary>
        /// <value>
        /// A <see cref="bool"/> indicating whether the clear button was clicked. The default value is <see langword="false"/>.
        /// </value>
        /// <remarks>
        /// This flag is used internally to track clear button interactions and coordinate the clearing operation
        /// with other component behaviors and event handling.
        /// </remarks>
        /// <exclude/>
        protected bool IsClearButtonClicked { get; set; }

        /// <summary>
        /// Gets or sets the CSS class string applied to the input component's container element.
        /// </summary>
        /// <value>
        /// A string containing space-separated CSS class names. The default value is an empty string.
        /// </value>
        /// <remarks>
        /// This property manages the CSS classes applied to the container element, including base styling classes,
        /// state-dependent classes (focused, disabled, etc.), and theme-specific classes.
        /// </remarks>
        /// <exclude/>
        protected virtual string ContainerClass { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the base CSS classes applied to the input component's root element.
        /// </summary>
        /// <value>
        /// A string containing the fundamental CSS classes for the component. The default value is "e-control e-textbox e-lib".
        /// </value>
        /// <remarks>
        /// This property defines the core CSS classes that establish the component's base appearance and behavior.
        /// These classes are typically combined with additional state and configuration classes during rendering.
        /// </remarks>
        /// <exclude/>
        protected virtual string RootClass { get; set; } = "e-control e-textbox e-lib";

        /// <summary>
        /// Gets or sets a reference identifier used to distinguish this component instance from others.
        /// </summary>
        /// <value>
        /// A string that uniquely identifies this component instance, or <c>null</c> if no specific reference is needed.
        /// </value>
        /// <remarks>
        /// This property is used internally to manage component lifecycle and distinguish between different instances
        /// of the same component type, particularly during initialization and script module loading.
        /// </remarks>
        /// <exclude/>
        protected virtual string? ComponentReference { get; set; }

        #endregion

        #region Constants variables

        /// <summary>
        /// The CSS class name applied to the main container element of the input component.
        /// </summary>
        /// <exclude/>
        protected const string CONTROLCONTAINER = "e-control-container";

        /// <summary>
        /// The CSS class name applied to the wrapper element of the input component, used for styling and layout purposes.
        /// </summary>
        /// <exclude/>
        protected const string CONTROLOLDCONTAINER = "e-control-wrapper";

        /// <summary>
        /// The CSS class name applied to the input group container when the component is configured with additional buttons or icons.
        /// </summary>
        /// <exclude/>
        protected const string INPUTGROUP = "e-input-group";

        /// <summary>
        /// The CSS class name applied to the clear button icon when it is hidden, used to control visibility through CSS.
        /// </summary>
        /// <exclude/>
        protected const string CLEARICONHIDE = "e-clear-icon-hide";

        /// <summary>
        /// The CSS class name applied to the input element when it is configured to support multiple lines of text, typically rendering as a textarea.
        /// </summary>
        /// <exclude/>
        protected const string MULTILINE = "e-multi-line-input";

        /// <summary>
        /// The CSS class name applied to the input element when it is in a disabled state, used to visually indicate that the input is not interactive.
        /// </summary>
        /// <exclude/>
        protected const string DISABLE = "e-disabled";

        /// <summary>
        /// The CSS class name applied to the input element when it is in a read-only state, used to visually indicate that the input cannot be modified.
        /// </summary>
        /// <exclude/>
        protected const string INPUT = "e-input";

        /// <summary>
        /// The CSS class name applied to the input element when it is focused, used to visually indicate that the input is active and ready for user interaction.
        /// </summary>
        /// <exclude/>
        protected const string INPUTFOCUS = "e-input-focus";

        /// <summary>
        /// The CSS class name applied to the input element when it is in a valid state, used to visually indicate that the current input value meets validation criteria.
        /// </summary>
        /// <exclude/>
        protected const string FLOATINPUT = "e-float-input";

        /// <summary>
        /// The CSS class name applied to the floating label element, used to control the appearance and positioning of the label when it is in a floating state.
        /// </summary>
        /// <exclude/>
        protected const string FLOATTEXT = "e-float-text";

        /// <summary>
        /// The CSS class name applied to the floating label element when it is positioned at the bottom of the input field, used to control the label's appearance in this state.
        /// </summary>
        /// <exclude/>
        protected const string FLOATLABELBOTTOM = "e-label-bottom";

        /// <summary>
        /// The CSS class name applied to the floating label element when it is positioned at the top of the input field, used to control the label's appearance in this state.
        /// </summary>
        /// <exclude/>
        protected const string FLOATLABELTOP = "e-label-top";

        /// <summary>
        /// The CSS class name applied to icons that are appended to the input field, used to control styling and layout of appended icons.
        /// </summary>
        /// <exclude/>
        protected const string APPENDICON = "e-append";

        /// <summary>
        /// The CSS class name applied to icons that are prepended to the input field, used to control styling and layout of prepended icons.
        /// </summary>
        /// <exclude/>
        protected const string PREPENDICON = "e-prepend";

        /// <summary>
        /// The CSS class name applied to the clear button icon when it is hidden, used to control visibility through CSS.
        /// </summary>
        /// <exclude/>
        protected const string CLEARICON = "e-toolkit-icons e-close e-clear-icon-hide";

        /// <summary>
        /// The CSS class name applied to the input group icon elements, used to control styling and layout of icons within the input group.
        /// </summary>
        /// <exclude/>
        protected const string GROUPICON = "e-input-group-icon";

        /// <summary>
        /// The CSS class name applied to the spin down button in numeric input components, used to visually indicate the decrement action.
        /// </summary>
        /// <exclude/>
        protected const string SPINDOWN = "e-toolkit-icons e-chevron-down";

        /// <summary>
        /// The CSS class name applied to the spin up button in numeric input components, used to visually indicate the increment action.
        /// </summary>
        /// <exclude/>
        protected const string SPINUP = "e-toolkit-icons e-chevron-up";

        /// <summary>
        /// The CSS class name applied to the icon when it is in a disabled state.
        /// </summary>
        /// <exclude/>
        protected const string DISABLEICON = "e-ddl-disable-icon";

        /// <summary>
        /// The CSS class name applied to the input wrapper element, used as the base class for styling and layout of the input component.
        /// </summary>
        /// <exclude/>
        protected const string INPUTBASECLASS = "e-input-base-wrapper";

        /// <summary>
        /// The string constant representing the "title" attribute for localization of the increment button in numeric input components.
        /// </summary>
        /// <exclude/>
        protected const string INCREMENTHEADING = "IncrementTitle";

        /// <summary>
        /// The string constant representing the "title" attribute for localization of the decrement button in numeric input components.
        /// </summary>
        /// <exclude/>
        protected const string DECREMENTHEADING = "DecrementTitle";

        /// <summary>
        /// The string constant representing the localized tooltip text for the increment button in numeric input components, used to enhance accessibility and user experience.
        /// </summary>
        /// <exclude/>
        protected const string INCREMENT = "Increment value";

        /// <summary>
        /// The string constant representing the localized tooltip text for the decrement button in numeric input components, used to enhance accessibility and user experience.
        /// </summary>
        /// <exclude/>
        protected const string DECREMENT = "Decrement value";

        /// <summary>
        /// The string constant representing the "disabled" attribute for HTML input elements, used to indicate that the input is not interactive and cannot be modified by the user.
        /// </summary>
        /// <exclude/>
        protected const string DISABLEDATTRIBUTE = "disabled";

        /// <summary>
        /// The string constant representing the "aria-disabled" attribute for accessibility, used to indicate that the input is disabled to assistive technologies.
        /// </summary>
        /// <exclude/>
        protected const string ARIADISABLED = "aria-disabled";

        /// <summary>
        /// The string to represent the append position for icons or buttons in the input component.
        /// </summary>
        /// <exclude/>
        protected const string APPEND = "append";

        /// <summary>
        /// The string to represent the prepend position for icons or buttons in the input component.
        /// </summary>
        /// <exclude/>
        protected const string PREPEND = "prepend";

        /// <summary>
        /// The string constant representing a single space character.
        /// </summary>
        /// <exclude/>
        protected const string SPACE = " ";

        private const string FILLED = "e-filled";
        private const string OUTLINE = "e-outline";
        private const string RTL = "e-rtl";
        private const string VALIDINPUT = "e-valid-input";
        private const string CLASS = "class";
        private const string ROLE = "role";
        private const string NAME = "name";
        private const string TEXTBOX = "textbox";
        private const string TAB_INDEX = "tabindex";
        private const string STYLE = "style";
        private const string PLACE_HOLDER = "placeholder";
        private const string READONLY = "readonly";
        private const string ARIA_READONLY = "aria-readonly";
        private const string ARIA_LABEL_BY = "aria-labelledby";
        private const string TEXT_AREA = "e-textarea";

        #endregion

        #region Internal variables

        /// <summary>
        /// Gets or sets the CSS class name applied to the clear button icon.
        /// </summary>
        /// <value>
        /// A string representing the CSS class for the clear button icon.
        /// </value>
        /// <remarks>
        /// This property controls the visibility and styling of the clear button icon through CSS class manipulation.
        /// It typically includes classes for hiding/showing the icon and applying appropriate visual styling.
        /// </remarks>
        internal string ClearIconClass { get; set; } = default!;

        /// <summary>
        /// Gets or sets the internal representation of the input's text value with change tracking and validation integration.
        /// </summary>
        /// <value>
        /// A value of type <typeparamref name="TValue"/> that represents the current input state.
        /// </value>
        /// <remarks>
        /// <para>This property serves as the internal interface for managing the component's value.</para>
        /// </remarks>
        internal TValue? InputTextValue
        {
            get => Value;
            set => _ = SetInputTextValueAsync(value);
        }

        /// <summary>
        /// Gets or sets the string representation of the current input value for display and parsing purposes.
        /// </summary>
        /// <value>
        /// A string representation of the current value, formatted according to the component's formatting rules.
        /// </value>
        /// <remarks>
        /// This property provides a two-way conversion between the strongly-typed <see cref="InputTextValue"/> 
        /// and its string representation for display in the HTML input element. The getter formats the current value
        /// as a string, while the setter parses the string back to the appropriate type.
        /// </remarks>
        internal string? CurrentValueAsString
        {
            get => FormatValueAsString(InputTextValue); set => InputTextValue = FormatValue(value);
        }

        /// <summary>
        /// Gets or sets the element reference to the main input element.
        /// </summary>
        /// <value>
        /// An <see cref="ElementReference"/> pointing to the input DOM element.
        /// </value>
        /// <remarks>
        /// This reference is essential for JavaScript interop operations, focus management, and direct DOM manipulation of the input field.
        /// </remarks>
        internal ElementReference InputElement { get; set; }

        /// <summary>
        /// Gets or sets the element reference to the component's container element.
        /// </summary>
        /// <value>
        /// An <see cref="ElementReference"/> pointing to the container DOM element.
        /// </value>
        /// <remarks>
        /// This reference provides access to the outer container for layout operations, event handling, and styling coordination.
        /// </remarks>
        internal ElementReference ContainerElement { get; set; }

        /// <summary>
        /// Represents the JavaScript module reference for the Input components functionality.
        /// </summary>
        internal IJSObjectReference? _textBoxJsModule;

        /// <summary>
        /// Represents the JavaScript in-process module reference for the Input components functionality.
        /// </summary>
        internal IJSInProcessObjectReference? _textBoxJsInProcessModule;

        #endregion

        #region Injected services

        /// <summary>
        /// Gets or sets the localization service for retrieving localized strings.
        /// </summary>
        /// <value>
        /// An <see cref="IStringLocalizer"/> instance used for string localization, or <c>null</c> if localization is not available.
        /// </value>
        /// <remarks>
        /// This injected service provides access to localized strings for UI elements such as button tooltips,
        /// accessibility labels, and user-facing messages based on the current culture.
        /// </remarks>
        [Inject]
        internal IStringLocalizer Localizer { get; set; } = default!;

        #endregion

        #region Protected methods

        /// <summary>
        /// Converts a strongly-typed value to its string representation for display purposes.
        /// </summary>
        /// <param name="formatValue">The value to be formatted as a string.</param>
        /// <returns>A string representation of the value, or <c>null</c> if the value is <c>null</c>.</returns>
        /// <remarks>
        /// This virtual method provides the default string formatting behavior and can be overridden in derived classes
        /// to implement custom formatting logic, such as number formatting, date formatting, or cultural considerations.
        /// </remarks>
        /// <exclude/>
        protected virtual string? FormatValueAsString(TValue? formatValue)
        {
            return formatValue?.ToString();
        }

        /// <summary>
        /// Converts a string value to the strongly-typed value expected by the component.
        /// </summary>
        /// <param name="genericValue">The string value to be converted.</param>
        /// <returns>A value of type <typeparamref name="TValue"/> parsed from the string, or the default value if the string is null or empty.</returns>
        /// <remarks>
        /// This virtual method provides the default parsing behavior and can be overridden in derived classes
        /// to implement custom parsing logic, validation, or type-specific conversion rules.
        /// </remarks>
        /// <exclude/>
        protected virtual TValue? FormatValue(string? genericValue)
        {
            return string.IsNullOrEmpty(genericValue) ? default : (TValue)SfBaseUtils.ChangeType(genericValue, typeof(TValue))!;
        }

        /// <summary>
        /// Provides a virtual method for derived classes to implement custom input handling logic.
        /// </summary>
        /// <param name="args">The change event arguments containing the input data, or <c>null</c> if no data is available.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous input handling operation.</returns>
        /// <remarks>
        /// This virtual method serves as an extension point for derived classes to implement component-specific
        /// input processing logic while maintaining the base class's standard input handling behavior.
        /// </remarks>
        /// <exclude/>
        protected virtual async Task InputHandlerAsync(ChangeEventArgs? args)
        {
            await Task.CompletedTask.ConfigureAwait(true);
        }

        /// <summary>
        /// Provides a virtual method for derived classes to implement custom focus handling logic.
        /// </summary>
        /// <param name="args">The focus event arguments containing information about the focus operation.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous focus handling operation.</returns>
        /// <remarks>
        /// This virtual method serves as an extension point for derived classes to implement component-specific
        /// focus processing logic while maintaining the base class's standard focus handling behavior.
        /// </remarks>
        /// <exclude/>
        protected virtual async Task FocusHandlerAsync(FocusEventArgs args)
        {
            await Task.CompletedTask.ConfigureAwait(true);
        }

        /// <summary>
        /// Provides a virtual method for derived classes to implement custom focus-out (blur) handling logic.
        /// </summary>
        /// <param name="args">The focus event arguments containing information about the blur operation.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous focus-out handling operation.</returns>
        /// <remarks>
        /// This virtual method serves as an extension point for derived classes to implement component-specific
        /// blur processing logic while maintaining the base class's standard focus-out handling behavior.
        /// </remarks>
        /// <exclude/>
        protected virtual async Task FocusOutHandlerAsync(FocusEventArgs args)
        {
            await Task.CompletedTask.ConfigureAwait(true);
        }

        /// <summary>
        /// Provides a virtual method for derived classes to implement custom change event handling logic.
        /// </summary>
        /// <param name="args">The change event arguments containing the updated value, or <c>null</c> if no data is available.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous change handling operation.</returns>
        /// <remarks>
        /// This virtual method serves as an extension point for derived classes to implement component-specific
        /// change processing logic while maintaining the base class's standard change event handling behavior.
        /// </remarks>
        /// <exclude/>
        protected virtual async Task ChangeHandlerAsync(ChangeEventArgs? args)
        {
            await Task.CompletedTask.ConfigureAwait(true);
        }

        /// <summary>
        /// Handles input events from the HTML input element, managing value updates, validation, and floating label behavior.
        /// </summary>
        /// <param name="args">The change event arguments containing the current input value.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous input handling operation.</returns>
        /// <remarks>
        /// <para>This method coordinates input event processing with the following operations:</para>
        /// <list type="bullet">
        /// <item><description>Updates floating label positioning based on input content</description></item>
        /// <item><description>Manages clear button visibility</description></item>
        /// <item><description>Performs real-time validation when <see cref="ValidateOnInput"/> is enabled</description></item>
        /// <item><description>Calls virtual <see cref="InputHandlerAsync"/> for component-specific logic</description></item>
        /// <item><description>Triggers the <c>OnInput</c> event callback</description></item>
        /// </list>
        /// </remarks>
        /// <exclude/>
        protected async Task OnInputHandlerAsync(ChangeEventArgs args)
        {
            string? inputVal = args is not null ? args.Value as string : null;
            CheckInputValue(BaseFloatLabelType, inputVal);
            UpdateIconState(inputVal);
            if (ValidateOnInput && InputEditContext is not null)
            {
                InputTextValue = FormatValue(inputVal);
            }
            if (!(Disabled || BaseReadOnly || BaseIsReadOnlyInput))
            {
                await InputHandlerAsync(args).ConfigureAwait(true);
            }
            IsClearButtonClicked = false;
        }

        /// <summary>
        /// Handles paste events from the clipboard, allowing components to respond to paste operations.
        /// </summary>
        /// <param name="args">The <see cref="ClipboardEventArgs"/> containing information about the paste operation.</param>
        /// <returns>A <see cref="Task"/> that completes when the paste handling operation finishes.</returns>
        /// <remarks>
        /// This method serves as a bridge between the DOM paste event and the component's <see cref="OnPaste"/> event callback,
        /// enabling custom paste handling logic such as data validation, formatting, or content filtering.
        /// </remarks>
        /// <exclude/>
        protected async Task OnPasteHandlerAsync(ClipboardEventArgs args)
        {
            if (OnPaste.HasDelegate)
            {
                await OnPaste.InvokeAsync(args).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Handles change events when the input value is modified and the element loses focus.
        /// </summary>
        /// <param name="args">The <see cref="ChangeEventArgs"/> containing the updated input value.</param>
        /// <returns>A <see cref="Task"/> that completes when the change handling operation finishes.</returns>
        /// <remarks>
        /// <para>This method processes change events with the following operations:</para>
        /// <list type="bullet">
        /// <item><description>Updates floating label positioning based on Auto mode settings</description></item>
        /// <item><description>Validates input content and updates visual state</description></item>
        /// <item><description>Calls virtual <see cref="ChangeHandlerAsync"/> for component-specific logic</description></item>
        /// <item><description>Triggers the <see cref="OnChange"/> event callback if configured</description></item>
        /// </list>
        /// </remarks>
        /// <exclude/>
        protected async Task OnChangeHandlerAsync(ChangeEventArgs args)
        {
            string? changeVal = args is not null ? args.Value as string : null;
            if (BaseFloatLabelType == FloatLabelType.Auto)
            {
                UpdateLabelState(changeVal);
            }
            CheckInputValue(BaseFloatLabelType, changeVal);
            await ChangeHandlerAsync(args).ConfigureAwait(true);
            if (OnChange.HasDelegate)
            {
                await OnChange.InvokeAsync(args).ConfigureAwait(true);
            }

        }

        /// <summary>
        /// Handles clear button click events to reset the input value.
        /// </summary>
        /// <returns>A <see cref="Task"/> that completes when the clear operation finishes.</returns>
        /// <remarks>
        /// <para>This method performs the following operations when the clear button is clicked:</para>
        /// <list type="bullet">
        /// <item><description>Validates that the input is not disabled or readonly</description></item>
        /// <item><description>Resets the input value to its default state</description></item>
        /// <item><description>Updates the clear button visibility by adding the hidden CSS class</description></item>
        /// <item><description>Sets the <see cref="IsClearButtonClicked"/> flag to track the clear action</description></item>
        /// </list>
        /// </remarks>
        /// <exclude/>
        protected async Task WireClearBtnEventsAsync()
        {
            if (!(Disabled || BaseReadOnly || BaseIsReadOnlyInput))
            {
                IsClearButtonClicked = true;
                InputTextValue = default;
                ClearIconClass = SfBaseUtils.AddClass(ClearIconClass, CLEARICONHIDE);
            }

            await Task.CompletedTask.ConfigureAwait(true);
        }

        /// <summary>
        /// Configures the enabled/disabled state of the component by applying appropriate CSS classes and HTML attributes.
        /// </summary>
        /// <remarks>
        /// <para>This method manages the component's enabled state by:</para>
        /// <list type="bullet">
        /// <item><description>When disabled: adds disabled CSS classes, sets the "disabled" attribute, and configures ARIA accessibility attributes</description></item>
        /// <item><description>When enabled: removes disabled styling and attributes, restores interactive functionality</description></item>
        /// <item><description>Updates both container and input element styling to reflect the current state</description></item>
        /// </list>
        /// </remarks>
        /// <exclude/>
        protected void SetEnabled()
        {
            if (Disabled)
            {
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, DISABLE);
                InputHtmlAttributes = SfBaseUtils.UpdateDictionary(CLASS, InputHtmlAttributes[CLASS] + " " + DISABLE, InputHtmlAttributes);
                InputHtmlAttributes = SfBaseUtils.UpdateDictionary(DISABLEDATTRIBUTE, DISABLEDATTRIBUTE, InputHtmlAttributes);
                InputHtmlAttributes = SfBaseUtils.UpdateDictionary(ARIADISABLED, "true", InputHtmlAttributes);
            }
            else
            {
                ContainerClass = ContainerClass.Replace(DISABLE, string.Empty, StringComparison.Ordinal);
                InputHtmlAttributes[CLASS] = InputHtmlAttributes[CLASS].ToString()!.Replace(DISABLE, string.Empty, StringComparison.Ordinal);
                _ = InputHtmlAttributes.Remove(DISABLEDATTRIBUTE);
                InputHtmlAttributes = SfBaseUtils.UpdateDictionary(ARIADISABLED, "false", InputHtmlAttributes);
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Handles the blur event when the component loses focus, with special handling for iPad devices.
        /// </summary>
        /// <param name="ElementValue">The current value of the element when it loses focus, primarily used for iPad compatibility.</param>
        /// <param name="isIPad">A value indicating whether the blur event is occurring on an iPad device, which may require special value synchronization.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous blur handling operation.</returns>
        /// <remarks>
        /// <para>This JavaScript-invokable method handles focus loss events with the following behavior:</para>
        /// <list type="bullet">
        /// <item><description>For iPad devices: synchronizes the element value to ensure consistency between DOM and component state</description></item>
        /// <item><description>Calls the internal blur handler to perform standard blur processing</description></item>
        /// <item><description>Updates component state and triggers relevant events</description></item>
        /// </list>
        /// </remarks>
        /// <exclude />
        [JSInvokable]
        public async Task BlurHandlerAsync(string? ElementValue = null, bool isIPad = false)
        {
            if (isIPad)
            {
                InputTextValue = FormatValue(ElementValue);
            }
            await OnBlurHandlerAsync().ConfigureAwait(true);
        }

        #endregion

        #region Internal methods

        /// <summary>
        /// Handles focus events when the input component receives focus, updating visual state and triggering related behaviors.
        /// </summary>
        /// <param name="args">The <see cref="FocusEventArgs"/> containing information about the focus operation.</param>
        /// <returns>A <see cref="Task"/> that completes when the focus handling operation finishes.</returns>
        /// <remarks>
        /// <para>This method manages focus-related behaviors when the component becomes active:</para>
        /// <list type="bullet">
        /// <item><description>Only processes the event if the component is enabled</description></item>
        /// <item><description>Adds focus-specific CSS classes to the container for visual feedback</description></item>
        /// <item><description>Updates the internal focus state and floating label positioning</description></item>
        /// <item><description>Controls clear button visibility for non-readonly inputs</description></item>
        /// <item><description>Calls virtual <see cref="FocusHandlerAsync"/> and triggers <c>OnFocus</c> event</description></item>
        /// </list>
        /// </remarks>
        internal async Task OnFocusHandlerAsync(FocusEventArgs args)
        {
            if (!Disabled)
            {
                if (ContainerClass.Contains(INPUTGROUP, StringComparison.Ordinal) || ContainerClass.Contains(OUTLINE, StringComparison.Ordinal) || ContainerClass.Contains(FILLED, StringComparison.Ordinal))
                {
                    ContainerClass = SfBaseUtils.AddClass(ContainerClass, INPUTFOCUS);
                }

                IsFocused = true;
                UpdateFloatLabelOnFocus();
                if (!BaseReadOnly)
                {
                    UpdateIconState(FormatValueAsString(InputTextValue));
                }
                await FocusHandlerAsync(args).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Handles blur events when the input component loses focus, updating visual state and triggering related behaviors.
        /// </summary>
        /// <param name="args">The <see cref="FocusEventArgs"/> containing information about the blur operation, or <see langword="null"/> to create default arguments.</param>
        /// <returns>A <see cref="Task"/> that completes when the blur handling operation finishes.</returns>
        /// <remarks>
        /// <para>This method manages blur-related behaviors when the component loses focus:</para>
        /// <list type="bullet">
        /// <item><description>Removes focus-specific CSS classes from the container</description></item>
        /// <item><description>Updates floating label positioning based on Auto mode and current value</description></item>
        /// <item><description>Hides the clear button if configured</description></item>
        /// <item><description>Calls virtual <see cref="FocusOutHandlerAsync"/> and triggers <c>OnBlur</c> event</description></item>
        /// <item><description>Invokes state change if no blur event delegate is configured</description></item>
        /// </list>
        /// </remarks>
        internal async Task OnBlurHandlerAsync(FocusEventArgs? args = null)
        {
            UpdateFloatLabelOnBlur();
            if (ContainerClass.Contains(INPUTGROUP, StringComparison.Ordinal) || ContainerClass.Contains(OUTLINE, StringComparison.Ordinal) || ContainerClass.Contains(FILLED, StringComparison.Ordinal))
            {
                ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, INPUTFOCUS);
            }

            if (BaseFloatLabelType == FloatLabelType.Auto)
            {
                UpdateLabelState(FormatValueAsString(InputTextValue));
            }

            IsFocused = false;
            args ??= new FocusEventArgs();
            ClearIconClass = BaseShowClearButton ? SfBaseUtils.AddClass(ClearIconClass, CLEARICONHIDE) : ClearIconClass;
            if (!Disabled)
            {
                await FocusOutHandlerAsync(args).ConfigureAwait(true);
            }
            StateHasChanged();
        }

        /// <summary>
        /// Sets the input value and updates related UI elements such as floating labels and clear button visibility.
        /// </summary>
        /// <param name="value">The <see cref="string"/> value to set, or <see langword="null"/> to clear the input.</param>
        /// <param name="floatLabelType">The <see cref="FloatLabelType"/> determining floating label behavior.</param>
        /// <param name="clearButton">A <see cref="bool"/> indicating whether to update clear button visibility. The default value is <see langword="false"/>.</param>
        /// <param name="isValueTemp">A <see cref="bool"/> indicating whether a value template is being displayed. The default value is <see langword="false"/>.</param>
        /// <returns>A <see cref="Task"/> that completes when the value update and UI refresh finish.</returns>
        /// <remarks>
        /// This method updates the input value, manages floating label positioning for Auto mode,
        /// controls clear button visibility, and triggers a component state refresh.
        /// </remarks>
        internal async Task SetValueAsync(string? value, FloatLabelType floatLabelType, bool clearButton = false, bool isValueTemp = false)
        {
            InputTextValue = FormatValue(value);
            IsValueTemplate = isValueTemp;
            if (floatLabelType == FloatLabelType.Auto)
            {
                ValidateLabel(value);
            }

            if (clearButton)
            {
                ClearIconClass = !string.IsNullOrEmpty(value) && ContainerClass.Contains(INPUTFOCUS, StringComparison.Ordinal)
                    ? ClearIconClass.Replace(CLEARICONHIDE, string.Empty, StringComparison.Ordinal)
                    : SfBaseUtils.AddClass(ClearIconClass, CLEARICONHIDE);
            }

            CheckInputValue(floatLabelType, value);
            await InvokeAsync(StateHasChanged).ConfigureAwait(true);
        }

        /// <summary>
        /// Adds icon buttons to the input component at the specified position with optional event handlers.
        /// </summary>
        /// <param name="position">A <see cref="string"/> specifying the icon position ("append" or "prepend"). The default value is <c>"append"</c>.</param>
        /// <param name="icons">A <see cref="string"/> containing the CSS class or identifier for the icon. The default value is <see langword="null"/>.</param>
        /// <param name="events">A <see cref="Dictionary{String, Object}"/> containing event name and handler pairs. The default value is <see langword="null"/>.</param>
        /// <returns>A <see cref="Task"/> that completes when the icon addition operation finishes.</returns>
        /// <remarks>
        /// This method creates a button group configuration with the specified icon, position, and events,
        /// then updates the container CSS classes to reflect the icon positioning.
        /// </remarks>
        internal async Task AddIconsAsync(string position = APPEND, string? icons = null, Dictionary<string, object>? events = null)
        {
            ListOfButtons ??= [];
            ListOfButtons.Add(new ButtonGroups()
            {
                Icon = icons,
                Position = position,
                _events = events
            });
            ContainerClass = position == APPEND ? SfBaseUtils.AddClass(ContainerClass, APPENDICON) : SfBaseUtils.AddClass(ContainerClass, PREPENDICON);
#if NET10_0
            StateHasChanged();
#endif            
            await Task.CompletedTask.ConfigureAwait(true);
        }

        /// <summary>
        /// Retrieves default numeric values for properties based on the specified type and property name.
        /// </summary>
        /// <typeparam name="T">The numeric type for which to retrieve the default value.</typeparam>
        /// <param name="property">The name of the property ("Step", "MinValue", or "MaxValue") to retrieve the default value for.</param>
        /// <returns>The default value for the specified property and type.</returns>
        /// <remarks>
        /// <para>This method provides type-specific default values for numeric input components:</para>
        /// <list type="bullet">
        /// <item><description>For "Step" property: returns 1 (decimal.One) for all types</description></item>
        /// <item><description>For "MinValue" property: returns the minimum value for the specified numeric type</description></item>
        /// <item><description>For "MaxValue" property: returns the maximum value for the specified numeric type</description></item>
        /// </list>
        /// <para>Supported types include all standard .NET numeric types (Int32, Int64, Single, Double, Decimal, etc.) including their nullable versions.</para>
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal static T GetNumericValue<T>(string property)
        {
            Type? propertyType = typeof(T);
            bool isNullable = propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
            propertyType = isNullable ? Nullable.GetUnderlyingType(propertyType) : propertyType;
            return property == "Step"
                ? (T)SfBaseUtils.ChangeType(decimal.One, propertyType!)!
                : (propertyType?.Name) switch
                {
                    nameof(Int32) => (property == "MinValue") ? (T)SfBaseUtils.ChangeType(int.MinValue, propertyType)! : (T)SfBaseUtils.ChangeType(int.MaxValue, propertyType)!,
                    nameof(Int64) => (property == "MinValue") ? (T)SfBaseUtils.ChangeType(long.MinValue, propertyType)! : (T)SfBaseUtils.ChangeType(long.MaxValue, propertyType)!,
                    nameof(Int16) => (property == "MinValue") ? (T)SfBaseUtils.ChangeType(short.MinValue, propertyType)! : (T)SfBaseUtils.ChangeType(short.MaxValue, propertyType)!,
                    nameof(Single) => (property == "MinValue") ? (T)SfBaseUtils.ChangeType(float.MinValue, propertyType)! : (T)SfBaseUtils.ChangeType(float.MaxValue, propertyType)!,
                    nameof(Double) => (property == "MinValue") ? (T)SfBaseUtils.ChangeType(double.MinValue, propertyType)! : (T)SfBaseUtils.ChangeType(double.MaxValue, propertyType)!,
                    nameof(Decimal) => (property == "MinValue") ? (T)SfBaseUtils.ChangeType(decimal.MinValue, propertyType)! : (T)SfBaseUtils.ChangeType(decimal.MaxValue, propertyType)!,
                    nameof(UInt16) => (property == "MinValue") ? (T)SfBaseUtils.ChangeType(ushort.MinValue, propertyType)! : (T)SfBaseUtils.ChangeType(ushort.MaxValue, propertyType)!,
                    nameof(UInt32) => (property == "MinValue") ? (T)SfBaseUtils.ChangeType(uint.MinValue, propertyType)! : (T)SfBaseUtils.ChangeType(uint.MaxValue, propertyType)!,
                    nameof(UInt64) => (property == "MinValue") ? (T)SfBaseUtils.ChangeType(ulong.MinValue, propertyType)! : (T)SfBaseUtils.ChangeType(ulong.MaxValue, propertyType)!,
                    _ => (property == "MinValue") ? (T)SfBaseUtils.ChangeType(propertyType?.GetField("MinValue")?.GetValue(null)!, propertyType!)! : (T)SfBaseUtils.ChangeType(propertyType?.GetField("MaxValue")?.GetValue(null)!, propertyType!)!,
                };
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Sets the input value asynchronously to ensure validation state consistency.
        /// </summary>
        /// <param name="value">The value to set.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        /// <remarks>
        /// <para>This method serves as the internal interface for managing the component's value with the following behaviors:</para>
        /// <list type="bullet">
        /// <item><description>Gets the current <see cref="Value"/> property</description></item>
        /// <item><description>When set, compares the new value with the current value for changes</description></item>
        /// <item><description>If changed and the component is enabled and not read-only, updates the value and triggers <see cref="ValueChanged"/></description></item>
        /// <item><description>Notifies the form validation system when integrated with <see cref="EditContext"/></description></item>
        /// </list>
        /// </remarks>
        private async Task SetInputTextValueAsync(TValue? value)
        {
            bool hasChanged = !EqualityComparer<TValue>.Default.Equals(value, Value);
            if (hasChanged && !(Disabled || BaseReadOnly || BaseIsReadOnlyInput))
            {
                Value = InternalValue = value;
                await ValueChanged.InvokeAsync(Value).ConfigureAwait(true);
                if (InputEditContext != null && ValueExpression != null)
                {
                    InputEditContext.NotifyFieldChanged(FieldIdentifier.Create(ValueExpression));
                }
            }
        }

        /// <summary>
        /// Performs pre-rendering setup by initializing input attributes, container classes, and component state before the component is rendered.
        /// </summary>
        private void PreRender()
        {
            InputHtmlAttributes = SfBaseUtils.UpdateDictionary(CLASS, RootClass, InputHtmlAttributes);
            InputHtmlAttributes = SfBaseUtils.UpdateDictionary(NAME, ID, InputHtmlAttributes);
            if (InputHtmlAttributes.Count > 0 && InputHtmlAttributes.TryGetValue("type", out object? value) && value.ToString() == "text")
            {
                InputHtmlAttributes = SfBaseUtils.UpdateDictionary(ROLE, TEXTBOX, InputHtmlAttributes);
            }
            int tabIndex = !Disabled ? BaseTabIndex : -1;
            InputHtmlAttributes = SfBaseUtils.UpdateDictionary(TAB_INDEX, tabIndex, InputHtmlAttributes);
            if (BaseFloatLabelType == FloatLabelType.Never)
            {
                InputHtmlAttributes = SfBaseUtils.UpdateDictionary(CLASS, RootClass + " " + INPUT, InputHtmlAttributes);
                ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, FLOATINPUT);
            }

            ContainerHtmlAttributes = SfBaseUtils.UpdateDictionary(STYLE, "width:" + BaseWidth + ";", ContainerHtmlAttributes);
            if (!MultilineInput)
            {
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, INPUTGROUP);
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, CONTROLCONTAINER);
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, CONTROLOLDCONTAINER);
            }
            else
            {
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, CONTROLCONTAINER);
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, CONTROLOLDCONTAINER);
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, MULTILINE);
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, INPUTGROUP);
            }
            if (RootClass is not null && !RootClass.Contains(TEXT_AREA, StringComparison.Ordinal))
            {
                InputHtmlAttributes = SfBaseUtils.UpdateDictionary("autocomplete", BaseAutocomplete, InputHtmlAttributes);
            }
            InputHtmlAttributes = SfBaseUtils.UpdateDictionary(PLACE_HOLDER, BasePlaceholder, InputHtmlAttributes);
            SetReadOnly();
            SetEnabled();
            SetRtl();
            CreateFloatingLabel();
            CheckInputValue(BaseFloatLabelType, FormatValueAsString(InputTextValue));
            UpdateHtmlAttr();
            UpdateInputAttr();
            if (SpinButton)
            {
                LocalizedString incrementLocale = Localizer[INCREMENTHEADING];
                LocalizedString decrementLocale = Localizer[DECREMENTHEADING];
                IncrementTitle = incrementLocale ?? INCREMENT;
                DecrementTitle = decrementLocale ?? DECREMENT;
            }
        }

        /// <summary>
        /// Configures the read-only state of the input element by setting appropriate HTML attributes.
        /// </summary>
        /// <remarks>
        /// <para>This method updates the input element's attributes to reflect the read-only state:</para>
        /// <list type="bullet">
        /// <item><description>When read-only: adds the "readonly" attribute and sets "aria-readonly" to "true" for accessibility</description></item>
        /// <item><description>When not read-only: removes the readonly attributes to allow user input</description></item>
        /// </list>
        /// </remarks>
        private void SetReadOnly()
        {
            if (BaseReadOnly || BaseIsReadOnlyInput)
            {
                InputHtmlAttributes = SfBaseUtils.UpdateDictionary(READONLY, true, InputHtmlAttributes);
                InputHtmlAttributes = SfBaseUtils.UpdateDictionary(ARIA_READONLY, "true", InputHtmlAttributes);
            }
            else
            {
                _ = InputHtmlAttributes.Remove(READONLY);
                _ = InputHtmlAttributes.Remove(ARIA_READONLY);
            }
        }

        /// <summary>
        /// Configures the floating label behavior and appearance based on the component's floating label type setting.
        /// </summary>
        /// <remarks>
        /// <para>This method handles the floating label functionality by:</para>
        /// <list type="bullet">
        /// <item><description>Removing placeholder attributes when floating labels are enabled (Auto or Always modes)</description></item>
        /// <item><description>For Auto mode: positioning the label based on focus state and input content</description></item>
        /// <item><description>For Always mode: permanently positioning the label above the input</description></item>
        /// <item><description>Adding appropriate CSS classes and ARIA attributes for accessibility</description></item>
        /// </list>
        /// </remarks>
        private void CreateFloatingLabel()
        {
            if (BaseFloatLabelType is FloatLabelType.Auto or FloatLabelType.Always)
            {
                _ = InputHtmlAttributes.Remove(PLACE_HOLDER);
            }

            if (BaseFloatLabelType == FloatLabelType.Auto && !ContainerClass.Contains(INPUTFOCUS, StringComparison.Ordinal) && !IsFocused)
            {
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, FLOATINPUT);
                InputHtmlAttributes = SfBaseUtils.UpdateDictionary(ARIA_LABEL_BY, "label_" + ID, InputHtmlAttributes);
                FloatLabel = string.IsNullOrEmpty(FormatValueAsString(InputTextValue)) ? FLOATTEXT + " " + FLOATLABELBOTTOM : FLOATTEXT + " " + FLOATLABELTOP;
            }
            else if (BaseFloatLabelType == FloatLabelType.Always)
            {
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, FLOATINPUT);
                ContainerClass = ContainerClass.Contains(OUTLINE, StringComparison.Ordinal) ? SfBaseUtils.AddClass(ContainerClass, VALIDINPUT) : ContainerClass;
                InputHtmlAttributes = SfBaseUtils.UpdateDictionary(ARIA_LABEL_BY, "label_" + ID, InputHtmlAttributes);
                FloatLabel = FLOATTEXT + " " + FLOATLABELTOP;
            }
        }

        /// <summary>
        /// Updates the input element's HTML attributes by merging base input attributes into the input attribute dictionary.
        /// </summary>
        private void UpdateInputAttr()
        {
            if (BaseInputAttributes?.Count > 0)
            {
                foreach (KeyValuePair<string, object> attr in BaseInputAttributes)
                {
                    _ = SfBaseUtils.UpdateDictionary(attr.Key, attr.Value, InputHtmlAttributes);
                }
            }
        }

        /// <summary>
        /// Configures the right-to-left (RTL) text direction by applying or removing the RTL CSS class based on the component's RTL settings.
        /// </summary>
        private void SetRtl()
        {
            ContainerClass = SyncfusionService?._options?.EnableRtl == true
                ? SfBaseUtils.AddClass(ContainerClass, RTL)
                : ContainerClass.Replace(RTL, string.Empty, StringComparison.Ordinal);
        }

        /// <summary>
        /// Distributes custom HTML attributes between the container and input elements based on predefined attribute categories.
        /// </summary>
        private void UpdateHtmlAttr()
        {
            if (BaseHtmlAttributes is null)
            {
                return;
            }

            foreach (KeyValuePair<string, object> item in BaseHtmlAttributes)
            {
                if (ContainerAttributes?.IndexOf(item.Key) < 0)
                {
                    InputHtmlAttributes = SfBaseUtils.UpdateDictionary(item.Key, item.Value, InputHtmlAttributes);
                }
                else
                {
                    if (item.Key == CLASS)
                    {
                        ContainerClass = SfBaseUtils.AddClass(ContainerClass, (string)item.Value);
                    }
                    else if (item.Key == STYLE)
                    {
                        if (ContainerHtmlAttributes.ContainsKey(STYLE))
                        {
                            ContainerHtmlAttributes[item.Key] += item.Value.ToString();
                        }
                        else
                        {
                            ContainerHtmlAttributes = SfBaseUtils.UpdateDictionary(item.Key, item.Value, ContainerHtmlAttributes);
                        }
                    }
                    else
                    {
                        ContainerHtmlAttributes = SfBaseUtils.UpdateDictionary(item.Key, item.Value, ContainerHtmlAttributes);
                    }
                }
            }
        }

        /// <summary>
        /// Updates the floating label CSS classes to position the label at the top when the input receives focus.
        /// </summary>
        private void UpdateFloatLabelOnFocus()
        {
            if (BaseFloatLabelType == FloatLabelType.Auto)
            {
                FloatLabel = FloatLabel.Contains(FLOATLABELTOP, StringComparison.Ordinal) ? FloatLabel : FloatLabel + " " + FLOATLABELTOP;
                FloatLabel = FloatLabel.Contains(FLOATLABELBOTTOM, StringComparison.Ordinal) ? FloatLabel.Replace(FLOATLABELBOTTOM, string.Empty, StringComparison.Ordinal) : FloatLabel;
            }
        }

        /// <summary>
        /// Updates the floating label CSS classes to position the label at the bottom when the input loses focus and has no value.
        /// </summary>
        private void UpdateFloatLabelOnBlur()
        {
            if (BaseFloatLabelType == FloatLabelType.Auto)
            {
                FloatLabel = FloatLabel.Contains(FLOATLABELTOP, StringComparison.Ordinal) ? FloatLabel.Replace(FLOATLABELTOP, string.Empty, StringComparison.Ordinal) : FloatLabel;
                FloatLabel = FloatLabel.Contains(FLOATLABELBOTTOM, StringComparison.Ordinal) ? FloatLabel : FloatLabel + " " + FLOATLABELBOTTOM;
            }
        }

        /// <summary>
        /// Updates the floating label position based on whether the input has a value.
        /// </summary>
        /// <param name="inputValue">The current input value to evaluate for label positioning.</param>
        private void UpdateLabelState(string? inputValue)
        {
            if (!string.IsNullOrEmpty(inputValue))
            {
                FloatLabel = FloatLabel.Contains(FLOATLABELTOP, StringComparison.Ordinal) ? FloatLabel : FloatLabel + " " + FLOATLABELTOP;
                FloatLabel = FloatLabel.Contains(FLOATLABELBOTTOM, StringComparison.Ordinal) ? FloatLabel.Replace(FLOATLABELBOTTOM, string.Empty, StringComparison.Ordinal) : FloatLabel;
            }
            else
            {
                FloatLabel = FloatLabel.Contains(FLOATLABELTOP, StringComparison.Ordinal) ? FloatLabel.Replace(FLOATLABELTOP, string.Empty, StringComparison.Ordinal) : FloatLabel;
                FloatLabel = FloatLabel.Contains(FLOATLABELBOTTOM, StringComparison.Ordinal) ? FloatLabel : FloatLabel + " " + FLOATLABELBOTTOM;
            }
        }

        /// <summary>
        /// Validates the input value and applies or removes the valid input CSS class based on content presence and float label type.
        /// </summary>
        /// <param name="floatLabelType">The floating label type determining validation behavior.</param>
        /// <param name="inputValue">The input value to validate for content presence.</param>
        private void CheckInputValue(FloatLabelType floatLabelType, string? inputValue)
        {
            if (!string.IsNullOrEmpty(inputValue))
            {
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, VALIDINPUT);
            }
            else if (floatLabelType != FloatLabelType.Always)
            {
                ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, VALIDINPUT);
            }
        }

        /// <summary>
        /// Validates and updates the floating label state for Auto mode based on the input value.
        /// </summary>
        /// <param name="value">The input value to evaluate for label state updates.</param>
        private void ValidateLabel(string? value)
        {
            if (ContainerClass.Contains(FLOATINPUT, StringComparison.Ordinal) && BaseFloatLabelType == FloatLabelType.Auto)
            {
                UpdateLabelState(value);
            }
        }

        /// <summary>
        /// Updates the visibility of the clear button icon based on whether the input has a value.
        /// </summary>
        /// <param name="value">The input value to evaluate for icon visibility.</param>
        private void UpdateIconState(string? value)
        {
            ClearIconClass = !string.IsNullOrEmpty(value)
                ? ClearIconClass.Replace(CLEARICONHIDE, string.Empty, StringComparison.Ordinal)
                : SfBaseUtils.AddClass(ClearIconClass, CLEARICONHIDE);
        }

        #endregion

        #region Nested Classes

        /// <summary>
        /// Represents a configuration class for button groups that can be associated with input components.
        /// </summary>
        /// <remarks>
        /// This class encapsulates the configuration for buttons that can be added to input components,
        /// such as clear buttons, dropdown buttons, or custom action buttons with their respective icons,
        /// positioning, and event handlers.
        /// </remarks>
        /// <exclude/>
        protected class ButtonGroups
        {
            #region Internal Variables

            /// <summary>
            /// Gets or sets the CSS class or icon identifier for the button's visual representation.
            /// </summary>
            /// <value>
            /// A string representing the icon CSS class or identifier. The default value is an empty string.
            /// </value>
            /// <remarks>
            /// This property typically contains CSS class names for icon fonts or image identifiers
            /// that will be applied to the button to provide visual representation.
            /// </remarks>
            internal string? Icon { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the position of the button relative to the input field.
            /// </summary>
            /// <value>
            /// A string indicating the button position, typically "append" or "prepend". The default value is an empty string.
            /// </value>
            /// <remarks>
            /// This property determines whether the button appears before (prepend) or after (append) the input field text,
            /// affecting the overall layout and user interaction flow.
            /// </remarks>
            internal string Position { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the collection of event handlers associated with the button.
            /// </summary>
            /// <value>
            /// A <see cref="Dictionary{String, Object}"/> containing event name and handler pairs. The default value is an empty dictionary.
            /// </value>
            /// <remarks>
            /// This dictionary maps event names (such as "click", "mousedown", etc.) to their corresponding
            /// event handler functions, enabling custom behavior when users interact with the button.
            /// </remarks>
            internal Dictionary<string, object>? _events = [];

            #endregion
        }

        #endregion
    }
}
