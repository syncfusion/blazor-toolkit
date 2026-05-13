using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Toolkit.Calendars.Internal
{
    /// <summary>
    /// Represents a base renderer for the calendar component in the Syncfusion Blazor library, responsible for rendering the Gregorian calendar view and enabling users to select dates interactively.
    /// </summary>
    /// <remarks>
    /// This class provides essential rendering constants and is intended to be used as a base for calendar UI components, such as DatePicker, and others that display selectable dates.
    /// It supplies constants used for calendar cells, months, years, navigation, and styling.
    /// </remarks>
    /// <example>
    /// The following example demonstrates the use of the calendar base renderer within a custom calendar component:
    /// <code><![CDATA[
    /// @inherits CalendarBaseRender<DateTime?>
    /// ]]> </code>
    /// </example>
    public partial class CalendarBaseRender<TValue> : CalendarBase<TValue>
    {
        private const string OTHER_MONTH = "e-other-month";
        private const string HEADER = "e-header";
        private const string CONTENT = "e-content";
        private const string CONTENT_TABLE = "e-calendar-content-table";
        private const string YEAR = "e-year";
        private const string MONTH = "e-month";
        private const string DECADE = "e-decade";
        private const string ICON = "e-toolkit-icons";
        private const string PREV_ICON = "e-prev";
        private const string NEXT_ICON = "e-next";
        private const string PREV_SPAN = "e-arrow-up";
        private const string NEXT_SPAN = "e-arrow-down";
        private const string ICON_CONTAINER = "e-icon-container";
        private const string DISABLED = "e-disabled";
        private const string OVERLAY = "e-overlay";
        private const string OTHER_MONTH_ROW = "e-month-hide";
        private const string TODAY = "e-today";
        private const string TITLE = "e-title";
        private const string LINK = "e-day";
        private const string FOOTER = "e-footer-container";
        private const string BTN = "e-btn";
        private const string FLAT = "e-flat";
        private const string CSS = "e-css";
        private const string PRIMARY = "e-primary";
        private const string MONTH_VIEW = "Month";
        private const string YEAR_VIEW = "Year";
        private const string DECADE_VIEW = "Decade";
        private const string VALUE = nameof(Value);
        private const string CALENDAR_BASE_VALUES = "Values";
        private const string NAVIGATED = "Navigated";
        private const string MONTHS = "months";
        private const string FORMAT_YEAR = "yyyy";
        private const string FORMAT_SHORT_DATE = "M/d/yy";
        private const string FORMAT_FULL_DATE = "dddd, MMMM dd, yyyy HH:mm";
        private const string FORMAT_MONTHS = "MMMM yyyy";
        private const string DAYS = "days";
        private const string TODAY_LOCALE_KEY = "Today";
        private const string FALSE = "false";
        private const string TRUE = "true";
        private const string MOVE_LEFT = "moveLeft";
        private const string MOVE_RIGHT = "moveRight";
        private const string MOVE_UP = "moveUp";
        private const string MOVE_DOWN = "moveDown";
        private const string SELECT = "select";
        private const string CONTROL_UP = "controlUp";
        private const string CONTROL_DOWN = "controlDown";
        private const string HOME = "home";
        private const string END = "end";
        private const string PAGE_UP = "pageUp";
        private const string PAGE_DOWN = "pageDown";
        private const string SHIFT_PAGE_UP = "shiftPageUp";
        private const string SHIFT_PAGE_DOWN = "shiftPageDown";
        private const string CONTROL_HOME = "controlHome";
        private const string CONTROL_END = "controlEnd";
        private const string PERSIAN = "fa";
        private const int CELLCOUNT = 42;
        private const int WEEK_NUMBER = 7;
        private const int YEAR_NUMBER = 12;
        private const int CELL_ROW = 4;
        private const string TITLE_SEPARATOR = " - ";
        private const int MONTH_VIEW_VAL = (int)CalendarView.Month;
        private const int YEAR_VIEW_VAL = (int)CalendarView.Year;
        private const int DECADE_VIEW_VAL = (int)CalendarView.Decade;

        // Navigation and offset constants
        private const int SINGLE_MONTH_OFFSET = 1;
        private const int SINGLE_YEAR_OFFSET = 1;
        private const int DECADE_YEAR_OFFSET = 10;
        private const int DECADE_CELL_COUNT = 35;
        private const int MIN_YEAR_VALUE = 10;
        private const int DECADE_MOD_VALUE = 10;
        private const int DECADE_START_OFFSET = 1;
        private const int YEAR_OFFSET_FOR_ISLAMIC = 1;
        private const int HIJRI_YEAR_RANGE_START = 1362;
        private const int HIJRI_YEAR_RANGE_END = 1492;

        // Date component indices
        private const int FIRST_DAY_OF_MONTH = 1;
        private const int FIRST_MONTH_OF_YEAR = 1;

        // Default Islamic calendar date ranges
        private const int ISLAMIC_MIN_YEAR = 1944;
        private const int ISLAMIC_MIN_MONTH = 2;
        private const int ISLAMIC_MIN_DAY = 18;
        private const int ISLAMIC_MAX_YEAR = 2069;
        private const int ISLAMIC_MAX_MONTH = 10;
        private const int ISLAMIC_MAX_DAY = 16;

        // Default Gregorian calendar date ranges
        private const int GREGORIAN_DEFAULT_MIN_YEAR = 1900;
        private const int GREGORIAN_DEFAULT_MAX_YEAR = 2099;
        private const int GREGORIAN_DEFAULT_MAX_MONTH = 12;
        private const int GREGORIAN_DEFAULT_MAX_DAY = 31;
    }


    /// <summary>
    /// Provides event arguments for the DatePicker popup, containing details such as the append target, cancellation status, original event, and prevention logic for popup behavior.
    /// </summary>
    /// <remarks>
    /// The <see cref="DatePickerPopupArgs"/> class is used in the DatePicker component to allow developers to customize and control how and where the DatePicker popup is appended and to handle cancellation or custom event logic as needed during the popup opening sequence.
    /// </remarks>
    /// <example>
    /// This example demonstrates how to use the event arguments to customize popup placement and cancel logic:
    /// <code><![CDATA[
    /// <SfDatePicker PopupAppendTo="body" PopupOpen="OnPopupOpen"></SfDatePicker>
    ///
    /// @code {
    ///     void OnPopupOpen(DatePickerPopupArgs args)
    ///     {
    ///         args.AppendTo = "#popup-root";
    ///         if (...) args.Cancel = true;
    ///     }
    /// }
    /// ]]></code>
    /// </example>
    public class DatePickerPopupArgs
    {
        /// <summary>
        /// Gets or sets the node selector to which the popup element should be appended when the popup is rendered.
        /// </summary>
        /// <value>
        /// A <c>string</c> indicating the CSS selector or DOM element reference where the popup will be attached. The default is <c>null</c>.
        /// </value>
        /// <remarks>
        /// Use this property to control DOM placement or context of the DatePicker popup for consistent UI alignment or containment.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// args.AppendTo = "body"; // Appends popup to the body element.
        /// ]]></code>
        /// </example>
        [JsonPropertyName("appendTo")]
        public string AppendTo { get; set; } = default!;

        /// <summary>
        /// Gets or sets a value that indicates whether the current action for opening the popup should be canceled.
        /// </summary>
        /// <value>
        /// A <c>bool</c> that indicates whether the operation is canceled. <c>true</c> cancels the popup operation, <c>false</c> allows it.
        /// </value>
        /// <remarks>
        /// Set <see cref="Cancel"/> to <c>true</c> during the event callback to prevent the DatePicker popup from opening.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// args.Cancel = true; // Cancels the popup open operation.
        /// ]]></code>
        /// </example>
        [JsonPropertyName("cancel")]
        public bool Cancel { get; set; }

        /// <summary>
        /// Gets or sets the original DOM event arguments associated with the popup open action.
        /// </summary>
        /// <value>
        /// An <c>object</c> representing the original browser event (such as a mouse click or keyboard event) that triggered the popup.
        /// </value>
        /// <remarks>
        /// This property provides access to the raw event arguments, enabling you to read custom event data or event-specific information during popup processing. Cast as needed to the specific event type.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// var mouseEvent = args.Event as MouseEventArgs;
        /// ]]></code>
        /// </example>
        [JsonPropertyName("event")]
        public object Event { get; set; } = new();

        /// <summary>
        /// Gets or sets an action or method that, when invoked, will prevent the default action of the browser event associated with opening the popup.
        /// </summary>
        /// <value>
        /// An <c>object</c> representing the prevention logic or function to be called. Usage is dependent on the JavaScript or Blazor event integration.
        /// </value>
        /// <remarks>
        /// Use this property to attach a function or flag to prevent the browser's default event action when working with advanced popup scenarios.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// args.PreventDefault = YourCustomPreventDefaultMethod;
        /// ]]></code>
        /// </example>
        [JsonPropertyName("preventDefault")]
        public object PreventDefault { get; set; } = new();
    }
}
