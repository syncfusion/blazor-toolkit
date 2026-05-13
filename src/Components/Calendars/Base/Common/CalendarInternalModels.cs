using Microsoft.AspNetCore.Components.Web;

namespace Syncfusion.Blazor.Toolkit.Calendars.Internal
{
    /// <summary>
    /// Represents the key actions performed within components of the calendar, such as navigation and selection of dates.
    /// </summary>
    /// <remarks>
    /// This class facilitates tracking of user interaction events, key information, and provides supporting properties for date selection workflows in Syncfusion calendar components.
    /// </remarks>
    /// <example>
    /// The following example demonstrates usage of <see cref="KeyActions"/> for customizing calendar behavior during keyboard interaction:
    /// <code><![CDATA[
    /// var keyActions = new KeyActions {
    ///     Action = "select",
    ///     Key = "Enter",
    ///     SelectDate = "2024-06-18"
    /// };
    /// ]]></code>
    /// </example>
    public class KeyActions
    {
        /// <summary>
        /// Gets or sets the action performed during keyboard interaction in the calendar.
        /// </summary>
        /// <value>The action type. For example, "select", "navigate", or "open". The default value is <c>null</c>.</value>
        /// <remarks>
        /// This property identifies the specific action triggered by the user, providing context to custom keyboard navigation and selection.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// keyActions.Action = "navigate";
        /// ]]></code>
        /// </example>
        public string Action { get; set; } = default!;

        /// <summary>
        /// Gets or sets the key value that was pressed for this action.
        /// </summary>
        /// <value>A <c>string</c> representing the keyboard key, such as "Enter" or "ArrowLeft". Default is <c>null</c>.</value>
        /// <remarks>
        /// Useful for intercepting or logging particular keyboard presses in calendar navigation or selection.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// keyActions.Key = "Enter";
        /// ]]></code>
        /// </example>
        public string Key { get; set; } = default!;

        /// <summary>
        /// Gets or sets the selected date string associated with the current key action.
        /// </summary>
        /// <value>A string representing the selected date, typically in ISO 8601 format.</value>
        /// <remarks>
        /// Use this property to access or modify the selected date during keyboard interactions with the calendar.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// keyActions.SelectDate = "2024-06-18";
        /// ]]></code>
        /// </example>
        public string SelectDate { get; set; } = default!;

        /// <summary>
        /// Gets or sets the currently focused date string during keyboard navigation.
        /// </summary>
        /// <value>A string value indicating the focused date, usually in ISO format.</value>
        /// <remarks>
        /// This property assists in tracking the date cell currently highlighted by keyboard navigation.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// keyActions.FocusedDate = "2024-06-20";
        /// ]]></code>
        /// </example>
        public string FocusedDate { get; set; } = default!;

        /// <summary>
        /// Gets or sets the CSS class(es) associated with the calendar cell or target.
        /// </summary>
        /// <value>A <c>string</c> containing one or more CSS class names.</value>
        /// <remarks>
        /// Useful for conditional styling or identifying elements related to the triggered action.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// keyActions.ClassList = "e-selected e-active";
        /// ]]></code>
        /// </example>
        public string ClassList { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the unique identifier (ID) for the element associated with the key action.
        /// </summary>
        /// <value>A <c>string</c> identifier value. Default is <c>null</c>.</value>
        /// <remarks>
        /// Assign this property for referencing a particular calendar cell or button.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// keyActions.ID = "calendarCell-2024-06-01";
        /// ]]></code>
        /// </example>
        public string ID { get; set; } = default!;

        /// <summary>
        /// Gets or sets the mouse event arguments relating to the key action.
        /// </summary>
        /// <value>A <see cref="MouseEventArgs"/> instance. The default is <c>null</c>.</value>
        /// <remarks>
        /// Used for scenarios where mouse and keyboard actions are combined or need to be distinguished.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// keyActions.Events = args;
        /// ]]></code>
        /// </example>
        public MouseEventArgs Events { get; set; } = new();

        /// <summary>
        /// Gets or sets the CSS class of the target element involved in the action.
        /// </summary>
        /// <value>A <c>string</c> specifying the class list of the HTML target.</value>
        /// <remarks>
        /// Enables advanced customization and behavioral logic based on the visual state of an element.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// keyActions.TargetClassList = "e-calendar-cell";
        /// ]]></code>
        /// </example>
        public string TargetClassList { get; set; } = default!;

        /// <summary>
        /// Gets or sets the numeric key code of the keyboard key associated with the action.
        /// </summary>
        /// <value>An <c>int</c> value representing the keyboard code. The default value is 0.</value>
        /// <remarks>
        /// This property helps in advanced keyboard event handling in scenarios requiring low-level key code access.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// keyActions.KeyCode = 13;
        /// ]]></code>
        /// </example>
        public int KeyCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the key event relates to the left calendar (in multi-calendar scenarios).
        /// </summary>
        /// <value><c>true</c> if the action targets the left calendar; otherwise, <c>false</c>. Default is <c>false</c>.</value>
        /// <remarks>
        /// Used in dual-calendar or range picker configurations to identify the calendar pane being manipulated.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// keyActions.IsLeftCalendar = true;
        /// ]]></code>
        /// </example>
        public bool IsLeftCalendar { get; set; }

        /// <summary>
        /// Gets or sets the focused cell's class list during key navigation.
        /// </summary>
        /// <value>A <c>string</c> of class names indicating the focused cell's visual states.</value>
        /// <remarks>
        /// Helps apply specific styles or logic to the cell currently receiving keyboard focus.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// keyActions.FocusedDateClassList = "e-focused-cell";
        /// ]]></code>
        /// </example>
        public string FocusedDateClassList { get; set; } = default!;

        /// <summary>
        /// Gets or sets the current date value as it relates to the key action.
        /// </summary>
        /// <value>A <c>string</c> of the relevant date. Default value is <c>null</c>.</value>
        /// <remarks>
        /// Used for referencing the raw date value in custom key event handlers.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// keyActions.DateValue = "2024-07-01";
        /// ]]></code>
        /// </example>
        public string DateValue { get; set; } = default!;
    }

    /// <summary>
    /// Holds the configuration for a list item in calendar input elements such as dropdowns and lists.
    /// </summary>
    /// <typeparam name="T">Represents the type of the date-time value, often <c>DateTime</c> or <c>DateTimeOffset</c>.</typeparam>
    /// <remarks>
    /// This class is typically used for managing calendar input lists, including their visual style and associated date values.
    /// </remarks>
    /// <example>
    /// The following demonstrates assignment of a date list item with styling:
    /// <code><![CDATA[
    /// var item = new ListOptions<DateTime>{
    ///     DateTimeValue = DateTime.Today,
    ///     ItemData = "2024-06-18",
    ///     ListClass = "e-list-item"
    /// };
    /// ]]></code>
    /// </example>
    public class ListOptions<T>
    {
        /// <summary>
        /// Gets or sets the date and time value for the list item.
        /// </summary>
        /// <value>The date-time value of type <c>T</c>. The default value is <c>default</c>.</value>
        /// <remarks>
        /// This property ties a calendar list entry to its relevant <c>T</c> value, generally representing a calendar date.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// item.DateTimeValue = DateTime.Now;
        /// ]]></code>
        /// </example>
        public T DateTimeValue { get; set; } = default!;

        /// <summary>
        /// Gets or sets the data associated with this list item (for example, the ISO date string for display).
        /// </summary>
        /// <value>A <c>string</c> value that represents the item's display text. Default is <c>null</c>.</value>
        /// <remarks>
        /// Used to connect formatting logic with the UI element.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// item.ItemData = "2024-06-18";
        /// ]]></code>
        /// </example>
        public string ItemData { get; set; } = default!;

        /// <summary>
        /// Gets or sets the CSS class for the list item.
        /// </summary>
        /// <value>A <c>string</c> containing the CSS class or classes for styling. Default is <c>null</c>.</value>
        /// <remarks>
        /// Useful for identifying or styling the list item in a calendar dropdown.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// item.ListClass = "e-list-item";
        /// ]]></code>
        /// </example>
        public string ListClass { get; set; } = default!;
    }
}