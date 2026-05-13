namespace Syncfusion.Blazor.Toolkit.Calendars
{
    /// <summary>
    /// The DatePicker is a graphical user interface component that allows users to select or enter a date value. It offers interactive and accessible features for choosing dates via keyboard, mouse, or touch input in web applications.
    /// </summary>
    /// <remarks>
    /// Use the <see cref="SfDatePicker{TValue}"/> component to create a fully featured date selection UI with support for templates, formatting, validation, localization, and customization options.
    /// </remarks>
    /// <example>
    /// This example demonstrates how to use the DatePicker.
    /// <code><![CDATA[
    /// <SfDatePicker TValue="DateTime" Placeholder="Enter date"/>
    /// ]]></code>
    /// </example>
    public partial class SfDatePicker<TValue>
    {
        private const string INVALID = "invalid";
        private const string MODIFIED_INVALID = "modified invalid";
        private const string MODIFIED_VALID = "modified valid";
        private const string SUCCESS_CLASS = "e-success";
        private const string RTL = "e-rtl";
        private const string WEEKNUMBER = "e-week-number";
        private const string NOEDIT = "e-non-edit";
        private const string POPUP_CONTAINER = "e-popup-wrapper e-popup-container";
        private const string DAY_HEADER_WIDE = "e-calendar-day-header-lg";
        private const string CALENDAR_ROOT = "e-calendar e-lib";
        private const string DEPTH = "Depth";
        private const string MIN = nameof(Min);
        private const string MAX = nameof(Max);
        private const string ARABIC = "ar";
        private const string THAILAND = "th";
        private const string ARIA_LIVE = "aria-live";
        private const string ASSERTIVE = "assertive";
        private const string ARIA_AUTOMIC = "aria-atomic";
        private const string ARIA_HAS_POPUP = "aria-haspopup";
        private const string ARIA_OWN = "aria-owns";
        private const string POPUPS = "_popup";
        private const string ROLE = "role";
        private const string COMBOBOX = "combobox";
        private const string AUTO_CORRECT = "autocorrect";
        private const string ARIA_CONTROLS = "aria-controls";
        private const string OFF = "off";
        private const string SPELL_CHECK = "spellcheck";
        private const string GRID = "grid";
        private const string ALT_UP_ARROW = "altUpArrow";
        private const string ALT_DOWN_ARROW = "altDownArrow";
        private const string ESCAPE = "escape";
        private const string ENTER = "enter";
        private const string TAB = "tab";
        private const string SHIFT_TAB = "shiftTab";
        private const string HOME = "home";
        private const string END = "end";
        private const char ARABIC_START_DIGIT = (char)1632;
        private const char ARABIC_END_DIGIT = (char)1641;
        private const char THAILAND_START_DIGIT = (char)3664;
        private const char THAILAND_END_DIGIT = (char)3675;
        private const string VALUE = nameof(Value);
        private const string FORMAT = nameof(FORMAT);
        private const string INPUTFORMATS = nameof(InputFormats);
        private const string MOVEUP = "moveUp";
        private const string MOVEDOWN = "moveDown";
        private const string DEVICE = "e-device";
        private const string FORMAT_YEAR = "yyyy";
        private const string FORMAT_MONTH = "MMMM dd";
        private const string FORMAT_DAY = "ddd,";
        private const int INPUT_UPDATE_DELAY_MS = 10;
        private const int DEVICE_READONLY_DELAY_MS = 10;
        private const int POPUP_HIDE_DELAY_MS = 20;
        private const int FOCUS_TRANSITION_DELAY_MS = 5;
        private const int VALUE_SYNC_DELAY_MS = 50;

        /// <summary>
        /// Represents the string value used to indicate a boolean <c>false</c> in ARIA attribute updates and other attribute dictionaries.
        /// </summary>
        protected const string FALSE = "false";

        /// <summary>
        /// The <c>aria-activedescendant</c> attribute name used to indicate which element is currently active within a composite widget.
        /// </summary>
        protected const string ARIAACTIVEDESCENDANT = "aria-activedescendant";

        /// <summary>
        /// Specifies the error class name applied to the DatePicker input when an invalid date is entered.
        /// </summary>
        /// <value>
        /// The error class string value is <c>e-error</c>.
        /// </value>
        /// <remarks>
        /// Use this class to customize the appearance of the DatePicker in error state.
        /// </remarks>
        protected const string ERRORCLASS = "e-error";
        /// <summary>
        /// Specifies the expandable popup class for the DatePicker popup panel.
        /// </summary>
        /// <value>
        /// The CSS class string value for an expandable popup is <c>e-popup-expand</c>.
        /// </value>
        /// <remarks>
        /// Used internally to apply styling for popup animations or transitions.
        /// </remarks>
        /// <exclude/>
        protected const string POPUPEXPAND = "e-popup-expand";

        /// <summary>
        /// Specifies the popup class for the popup container.
        /// </summary>
        /// <value>
        /// The CSS class string value for the popup is <c>e-popup</c>.
        /// </value>
        /// <remarks>
        /// Used internally to style and identify popup containers in the DatePicker.
        /// </remarks>
        /// <exclude/>
        protected const string POPUP = "e-popup";
        /// <summary>
        /// Gets or sets the root element class for the DatePicker component.
        /// </summary>
        /// <value>
        /// The root CSS class string, default is <c>e-control e-datepicker e-lib</c>.
        /// </value>
        /// <remarks>
        /// This property is used to customize the root element styling of the DatePicker.
        /// </remarks>
        /// <exclude/>
        protected virtual string ROOT { get; set; } = "e-control e-datepicker e-lib";

        /// <summary>
        /// Gets or sets the container class for the DatePicker input wrapper.
        /// </summary>
        /// <value>
        /// The container CSS class string, default is <c>e-date-wrapper e-date-container</c>.
        /// </value>
        /// <remarks>
        /// This property is used to customize the container element styling of the DatePicker input field.
        /// </remarks>
        /// <exclude/>
        protected virtual string CONTAINERCLASS { get; set; } = "e-date-wrapper e-date-container";

        /// <summary>
        /// Specifies the component class name for the DatePicker input control.
        /// </summary>
        /// <value>
        /// The CSS class string for the DatePicker UI, <c>e-datepicker</c>.
        /// </value>
        /// <remarks>
        /// Used for styling and identifying the DatePicker instances in DOM.
        /// </remarks>
        /// <exclude/>
        protected const string DATEPICKER = "e-datepicker";
        /// <summary>
        /// Specifies the aria-expanded attribute value for the calendar popup button.
        /// </summary>
        /// <value>
        /// The attribute value used for accessibility, <c>aria-expanded</c>.
        /// </value>
        /// <remarks>
        /// Provides state information to assistive technologies about popup expansion.
        /// </remarks>
        /// <exclude/>
        protected const string ARIAEXPANDED = "aria-expanded";

        /// <summary>
        /// Specifies the aria-true constant value used for accessibility state attributes.
        /// </summary>
        /// <value>
        /// A string representing the <c>aria-true</c> value, <c>true</c>.
        /// </value>
        /// <remarks>
        /// Used internally in accessibility-related state changes.
        /// </remarks>
        /// <exclude/>
        protected const string TRUE = "true";
        /// <summary>
        /// Specifies the value used for the aria-invalid attribute for accessibility.
        /// </summary>
        /// <value>
        /// The attribute value string for <c>aria-invalid</c>.
        /// </value>
        /// <remarks>
        /// Used by assistive technologies to indicate that the DatePicker input is invalid.
        /// </remarks>
        protected const string ARIAINVALID = "aria-invalid";

        /// <summary>
        /// Specifies the active class applied to list items in the DatePicker and calendar UI.
        /// </summary>
        /// <value>
        /// The active class string value is <c>e-active</c>.
        /// </value>
        /// <remarks>
        /// Used to visually highlight the currently selected item in lists or grids.
        /// </remarks>
        /// <exclude/>
        protected const string ACTIVE = "e-active";

        /// <summary>
        /// Specifies the input readonly attribute value used in the DatePicker.
        /// </summary>
        /// <value>
        /// The attribute string value is <c>readonly</c>.
        /// </value>
        /// <remarks>
        /// Used to define whether the DatePicker input is read-only.
        /// </remarks>
        /// <exclude/>
        protected const string READONLYATTR = "readonly";

        /// <summary>
        /// Specifies the focus class applied when the DatePicker input is focused.
        /// </summary>
        /// <value>
        /// The focused input class string value is <c>e-input-focus</c>.
        /// </value>
        /// <remarks>
        /// Used to style the DatePicker control when it receives keyboard or mouse focus.
        /// </remarks>
        /// <exclude/>
        protected new const string INPUTFOCUS = "e-input-focus";
        /// <summary>
        /// Specifies the class name for the date icon.
        /// </summary>
        /// <value>
        /// The icon's CSS class string value is <c>e-timeline-today e-toolkit-icons</c>.
        /// </value>
        /// <remarks>
        /// Used to display a calendar icon within the DatePicker input.
        /// </remarks>
        protected const string DATEICONCLASS = "e-timeline-today e-toolkit-icons";

        /// <summary>
        /// Specifies the class name for the model wrapper in the calendar.
        /// </summary>
        /// <value>
        /// The model CSS class string value is <c>model</c>.
        /// </value>
        /// <remarks>
        /// Used for the calendar model's main wrapper for theme and style application.
        /// </remarks>
        /// <exclude/>
        protected const string MODEL = "model";

        /// <summary>
        /// Specifies the class name of the target element to which the DatePicker popup is appended.
        /// </summary>
        /// <value>
        /// The CSS selector string value is <c>body</c>.
        /// </value>
        /// <remarks>
        /// Used internally to determine the popup's parent element in the DOM.
        /// </remarks>
        /// <exclude/>
        protected const string BODY = "body";

        /// <summary>
        /// Specifies the header class name in the date model UI.
        /// </summary>
        /// <value>
        /// The header class string value is <c>e-model-header</c>.
        /// </value>
        /// <remarks>
        /// Used to style the model header region in the DatePicker popup.
        /// </remarks>
        /// <exclude/>
        protected const string MODELHEADER = "e-model-header";

        /// <summary>
        /// Specifies the year class name in the date model UI.
        /// </summary>
        /// <value>
        /// The year class string value is <c>e-model-year</c>.
        /// </value>
        /// <remarks>
        /// Used to visually indicate the year region in the date model popup.
        /// </remarks>
        /// <exclude/>
        protected const string MODELYEARCLASS = "e-model-year";

        /// <summary>
        /// Specifies the day class name in the date model UI.
        /// </summary>
        /// <value>
        /// The day class string value is <c>e-model-day</c>.
        /// </value>
        /// <remarks>
        /// Used to visually differentiate the day region in the date model popup.
        /// </remarks>
        /// <exclude/>
        protected const string MODELDAYCLASS = "e-model-day";

        /// <summary>
        /// Specifies the month class name in the date model UI.
        /// </summary>
        /// <value>
        /// The month class string value is <c>e-model-month</c>.
        /// </value>
        /// <remarks>
        /// Used to visually indicate the month region in the date model popup.
        /// </remarks>
        /// <exclude/>
        protected const string MODELMONTHCLASS = "e-model-month";

        /// <summary>
        /// Specifies the popup holder class for the DatePicker popup UI.
        /// </summary>
        /// <value>
        /// The popup holder's CSS class string value is <c>e-datepicker e-popup-holder</c>.
        /// </value>
        /// <remarks>
        /// Used internally for styling the DatePicker popup's outer container.
        /// </remarks>
        /// <exclude/>
        protected const string POPUPHOLDER = "e-datepicker e-popup-holder";

        /// <summary>
        /// Specifies the locale key constant string for day content.
        /// </summary>
        /// <value>
        /// The locale key string for day content is <c>Day</c>.
        /// </value>
        /// <remarks>
        /// Used for localization mapping when rendering day content in the DatePicker.
        /// </remarks>
        /// <exclude/>
        protected const string DAYLOCALEKEY = "Day";

        /// <summary>
        /// Specifies the default locale string value for day content.
        /// </summary>
        /// <value>
        /// The locale string value for day content is <c>Day</c>.
        /// </value>
        /// <remarks>
        /// Used as default day label in DatePicker localization scenarios.
        /// </remarks>
        /// <exclude/>
        protected const string DAYLOCALEVALUE = "Day";

        /// <summary>
        /// Specifies the locale key constant string for month content.
        /// </summary>
        /// <value>
        /// The locale key string for month content is <c>Month</c>.
        /// </value>
        /// <remarks>
        /// Used for localization mapping when rendering month content in the DatePicker.
        /// </remarks>
        /// <exclude/>
        protected const string MONTHLOCALEKEY = "Month";

        /// <summary>
        /// Specifies the default locale string value for month content.
        /// </summary>
        /// <value>
        /// The locale string value for month content is <c>Month</c>.
        /// </value>
        /// <remarks>
        /// Used as default month label in DatePicker localization scenarios.
        /// </remarks>
        /// <exclude/>
        protected const string MONTHLOCALEVALUE = "Month";

        /// <summary>
        /// Specifies the locale key constant string for year content.
        /// </summary>
        /// <value>
        /// The locale key string for year content is <c>Year</c>.
        /// </value>
        /// <remarks>
        /// Used for localization mapping when rendering year content in the DatePicker.
        /// </remarks>
        /// <exclude/>
        protected const string YEARLOCALEKEY = "Year";

        /// <summary>
        /// Specifies the default locale string value for year content.
        /// </summary>
        /// <value>
        /// The locale string value for the year content is <c>Year</c>.
        /// </value>
        /// <remarks>
        /// Used as default year label in DatePicker localization scenarios.
        /// </remarks>
        /// <exclude/>
        protected const string YEARLOCALEVALUE = "Year";

        /// <summary>
        /// Specifies the locale key constant string for hour content in the DatePicker time picker.
        /// </summary>
        /// <value>
        /// The locale key string for hour content is <c>Hour</c>.
        /// </value>
        /// <remarks>
        /// Used for localization mapping when rendering hour label in the DatePicker.
        /// </remarks>
        /// <exclude/>
        protected const string HOURLOCALEKEY = "Hour";

        /// <summary>
        /// Specifies the default locale string value for hour content in the DatePicker time picker.
        /// </summary>
        /// <value>
        /// The locale string value for hour content is <c>Hour</c>.
        /// </value>
        /// <remarks>
        /// Used as default hour label in DatePicker localization scenarios.
        /// </remarks>
        /// <exclude/>
        protected const string HOURLOCALEVALUE = "Hour";

        /// <summary>
        /// Specifies the locale key constant string for minute content in the DatePicker time picker.
        /// </summary>
        /// <value>
        /// The locale key string for minute content is <c>Minute</c>.
        /// </value>
        /// <remarks>
        /// Used for localization mapping when rendering minute label in the DatePicker.
        /// </remarks>
        /// <exclude/>
        protected const string MINUTELOCALEKEY = "Minute";

        /// <summary>
        /// Specifies the default locale string value for minute content in the DatePicker time picker.
        /// </summary>
        /// <value>
        /// The locale string value for minute content is <c>Minute</c>.
        /// </value>
        /// <remarks>
        /// Used as default minute label in DatePicker localization scenarios.
        /// </remarks>
        /// <exclude/>
        protected const string MINUTELOCALEVALUE = "Minute";

        /// <summary>
        /// Specifies the locale key constant string for second content in the DatePicker time picker.
        /// </summary>
        /// <value>
        /// The locale key string for second content is <c>Second</c>.
        /// </value>
        /// <remarks>
        /// Used for localization mapping when rendering second label in the DatePicker.
        /// </remarks>
        /// <exclude/>
        protected const string SECONDLOCALEKEY = "Second";

        /// <summary>
        /// Specifies the default locale string value for second content in the DatePicker time picker.
        /// </summary>
        /// <value>
        /// The locale string value for second content is <c>Second</c>.
        /// </value>
        /// <remarks>
        /// Used as default second label in DatePicker localization scenarios.
        /// </remarks>
        /// <exclude/>
        protected const string SECONDLOCALEVALUE = "Second";

        /// <summary>
        /// Specifies the locale key constant string for day of week content.
        /// </summary>
        /// <value>
        /// The locale key string for day of week content is <c>DayOfWeek</c>.
        /// </value>
        /// <remarks>
        /// Used for localization mapping when rendering day of week content in the DatePicker.
        /// </remarks>
        /// <exclude/>
        protected const string DAYOFWEEKLOCALEKEY = "DayOfWeek";

        /// <summary>
        /// Specifies the default locale string value for day of week content.
        /// </summary>
        /// <value>
        /// The locale string value for day of week is <c>DayOfWeek</c>.
        /// </value>
        /// <remarks>
        /// Used as default day of week label in DatePicker localization scenarios.
        /// </remarks>
        /// <exclude/>
        protected const string DAYOFWEEKLOCALEVALUE = "DayOfWeek";

        /// <summary>
        /// Specifies the class name for the calendar cell container in the DatePicker.
        /// </summary>
        /// <value>
        /// The CSS class string value is <c>e-calendar-cell-container</c>.
        /// </value>
        /// <remarks>
        /// Used internally for marking the container of calendar cells.
        /// </remarks>
        /// <exclude/>
        protected const string CALENDARCELLCONTAINER = "e-calendar-cell-container";

        /// <summary>
        /// Specifies the class name for the close icon in the full screen mobile popup.
        /// </summary>
        /// <value>
        /// The close icon's CSS class string value is <c>e-popup-close</c>.
        /// </value>
        /// <remarks>
        /// Used internally to render the close button for mobile full screen popups.
        /// </remarks>
        /// <exclude/>
        protected const string CLOSEICON = "e-popup-close";

        /// <summary>
        /// Specifies the class name for the date cell container in the DatePicker.
        /// </summary>
        /// <value>
        /// The CSS class string value is <c>e-date-container</c>.
        /// </value>
        /// <remarks>
        /// Used to group date-related UI elements in the component's structure.
        /// </remarks>
        /// <exclude/>
        protected const string DATECONTAINER = "e-date-container";
    }
}
