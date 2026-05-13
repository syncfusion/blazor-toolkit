using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Toolkit.Calendars
{
    /// <summary>
    /// Provides event arguments for the blur event when a calendar component loses focus.
    /// </summary>
    /// <remarks>
    /// This class encapsulates the data associated with the blur event, which is triggered when a calendar component loses input focus.
    /// The event arguments include model information that can be used to determine the state of the component when focus is lost.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// <SfDatePicker TValue="DateTime?" OnBlur="HandleBlur"></SfDatePicker>
    /// 
    /// @code {
    ///     void HandleBlur(BlurEventArgs args)
    ///     {
    ///         Console.WriteLine($"Calendar lost focus. Model: {args.Model}");
    ///     }
    /// }
    /// ]]></code>
    /// </example>
    public class BlurEventArgs
    {
        /// <summary>
        /// Gets or sets the model associated with the blur event.
        /// </summary>
        /// <value>
        /// An <c>object</c> that represents the model data when the blur event occurs. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property contains model information that provides context about the component's state when the blur event is triggered.
        /// The model can be used to access component-specific data or configuration at the time focus is lost.
        /// </remarks>
        [JsonPropertyName("model")]
        public object Model { get; set; } = new();
    }

    /// <summary>
    /// Provides event arguments for the <see cref="SfCalendar{TValue}.Selected"/> event when a date is selected in a calendar component.
    /// </summary>
    /// <typeparam name="T">The type of the date value being selected (typically <see cref="DateTime"/> or nullable <see cref="DateTime"/>).</typeparam>
    /// <remarks>
    /// This class contains the selected date value when a user selects a date in the calendar component.
    /// The generic type parameter allows for flexibility in the date value type used by different calendar implementations.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// <SfCalendar TValue="DateTime?" Selected="OnDateSelected"></SfCalendar>
    /// 
    /// @code {
    ///     void OnDateSelected(SelectedEventArgs<DateTime?> args)
    ///     {
    ///         Console.WriteLine($"Selected date: {args.Value}");
    ///     }
    /// }
    /// ]]></code>
    /// </example>
    public class SelectedEventArgs<T>
    {
        /// <summary>
        /// Gets or sets the selected date value.
        /// </summary>
        /// <value>
        /// A value of type <c>T</c> representing the date that was selected. The default value is <c>default(T)</c>.
        /// </value>
        /// <remarks>
        /// This property contains the date value that was selected by the user in the calendar component.
        /// The type is determined by the generic parameter, allowing for different date value types.
        /// </remarks>
        public T Value { get; set; } = default!;
    }

    /// <summary>
    /// Provides event arguments for the <see cref="SfCalendar{TValue}.DeSelected"/> event when a date is deselected in a calendar component.
    /// </summary>
    /// <typeparam name="T">The type of the date value being deselected (typically <see cref="DateTime"/> or nullable <see cref="DateTime"/>).</typeparam>
    /// <remarks>
    /// This class contains the deselected date value when a user deselects a previously selected date in the calendar component.
    /// This event is typically used in multi-selection scenarios where users can remove dates from their selection.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// <SfCalendar TValue="DateTime?" IsMultiSelection="true" DeSelected="OnDateDeselected"></SfCalendar>
    /// 
    /// @code {
    ///     void OnDateDeselected(DeSelectedEventArgs<DateTime?> args)
    ///     {
    ///         Console.WriteLine($"Deselected date: {args.Value}");
    ///     }
    /// }
    /// ]]></code>
    /// </example>
    public class DeSelectedEventArgs<T>
    {
        /// <summary>
        /// Gets or sets the deselected date value.
        /// </summary>
        /// <value>
        /// A value of type <c>T</c> representing the date that was deselected. The default value is <c>default(T)</c>.
        /// </value>
        /// <remarks>
        /// This property contains the date value that was deselected by the user in the calendar component.
        /// The type is determined by the generic parameter, allowing for different date value types.
        /// </remarks>
        public T Value { get; set; } = default!;
    }

    /// <summary>
    /// Provides event arguments for the <see cref="SfCalendar{TValue}.ValueChange"/> event when the calendar value changes.
    /// </summary>
    /// <typeparam name="T">The type of the calendar value (typically <see cref="DateTime"/> or nullable <see cref="DateTime"/>).</typeparam>
    /// <remarks>
    /// This class contains comprehensive information about the calendar value change, including the new value, 
    /// whether the change was triggered by user interaction, and references to the DOM element and original event.
    /// This event provides detailed context for responding to calendar value changes in applications.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// <SfCalendar TValue="DateTime?" ValueChange="OnValueChanged"></SfCalendar>
    /// 
    /// @code {
    ///     void OnValueChanged(ChangedEventArgs<DateTime?> args)
    ///     {
    ///         if (args.IsInteracted)
    ///         {
    ///             Console.WriteLine($"User selected: {args.Value}");
    ///         }
    ///     }
    /// }
    /// ]]></code>
    /// </example>
    public class ChangedEventArgs<T>
    {
        /// <summary>
        /// Gets or sets the DOM element associated with the value change event.
        /// </summary>
        /// <value>
        /// An <c>object</c> that represents the DOM element where the value change occurred. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property provides access to the DOM element that was involved in the value change event,
        /// allowing for additional manipulation or inspection of the element if needed.
        /// </remarks>
        [JsonPropertyName("element")]
        public object Element { get; set; } = default!;

        /// <summary>
        /// Gets or sets the original browser event that triggered the value change.
        /// </summary>
        /// <value>
        /// An <c>object</c> containing the original event arguments from the browser. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property contains the raw browser event information, such as mouse click or keyboard events,
        /// that caused the calendar value to change. It can be used for advanced event handling scenarios.
        /// </remarks>
        [JsonPropertyName("event")]
        public object Event { get; set; } = default!;

        /// <summary>
        /// Gets or sets a value indicating whether the event was triggered by user interaction.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current event was triggered by user interaction; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// This property helps distinguish between programmatic value changes and user-initiated changes.
        /// When <c>true</c>, the value change was caused by user actions like clicking on a date.
        /// When <c>false</c>, the value change was likely caused by programmatic updates.
        /// </remarks>
        [JsonPropertyName("isInteracted")]
        public bool IsInteracted { get; set; }

        /// <summary>
        /// Gets or sets the name of the event.
        /// </summary>
        /// <value>
        /// A <c>string</c> value representing the event name. The default value is <c>Empty String</c>.
        /// </value>
        /// <remarks>
        /// This property specifies the name identifier for the event, which can be used to identify 
        /// the specific type of change event in scenarios where multiple event types are handled.
        /// </remarks>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the selected date value of the calendar.
        /// </summary>
        /// <value>
        /// A value of type <c>T</c> representing the currently selected date. The default value is <c>default(T)</c>.
        /// </value>
        /// <remarks>
        /// This property contains the primary selected date value when the calendar supports single selection.
        /// For multi-selection scenarios, use the <see cref="Values"/> property to access all selected dates.
        /// </remarks>
        [JsonPropertyName("value")]
        public T Value { get; set; } = default!;

        /// <summary>
        /// Gets or sets the multiple selected dates of the calendar.
        /// </summary>
        /// <value>
        /// An array of <see cref="DateTime"/> values representing all selected dates. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property is used when the calendar supports multiple date selection.
        /// It contains all the dates that are currently selected by the user.
        /// For single selection scenarios, this property may be <c>null</c> or contain only one date.
        /// </remarks>
        [JsonPropertyName("values")]
        public DateTime[] Values { get; set; } = default!;
    }

    /// <summary>
    /// Provides event arguments for the clear event when a calendar component's value is cleared.
    /// </summary>
    /// <remarks>
    /// This class contains event information when a user clears the calendar value using the clear button
    /// or through programmatic clearing. It provides access to the original browser event that triggered the clear action.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// <SfDatePicker TValue="DateTime?" ShowClearButton="true" Cleared="OnCleared"></SfDatePicker>
    /// 
    /// @code {
    ///     void OnCleared(ClearedEventArgs args)
    ///     {
    ///         Console.WriteLine("Calendar value was cleared");
    ///     }
    /// }
    /// ]]></code>
    /// </example>
    public class ClearedEventArgs
    {
        /// <summary>
        /// Gets or sets the original browser event that triggered the clear action.
        /// </summary>
        /// <value>
        /// An <c>object</c> containing the original event arguments from the browser. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property contains the raw browser event information, such as a click event on the clear button,
        /// that caused the calendar value to be cleared. It provides access to the underlying event details.
        /// </remarks>
        [JsonPropertyName("event")]
        public object Event { get; set; } = default!;
    }

    /// <summary>
    /// Provides event arguments for the focus event when a calendar component gains focus.
    /// </summary>
    /// <remarks>
    /// This class encapsulates the data associated with the focus event, which is triggered when a calendar component
    /// receives input focus. The event arguments include model information that provides context about the component's state.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// <SfDatePicker TValue="DateTime?" OnFocus="HandleFocus"></SfDatePicker>
    /// 
    /// @code {
    ///     void HandleFocus(FocusEventArgs args)
    ///     {
    ///         Console.WriteLine($"Calendar gained focus. Model: {args.Model}");
    ///     }
    /// }
    /// ]]></code>
    /// </example>
    public class FocusEventArgs
    {
        /// <summary>
        /// Gets or sets the model associated with the focus event.
        /// </summary>
        /// <value>
        /// An <c>object</c> that represents the model data when the focus event occurs. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property contains model information that provides context about the component's state when the focus event is triggered.
        /// The model can be used to access component-specific data or configuration at the time focus is received.
        /// </remarks>
        [JsonPropertyName("model")]
        public object Model { get; set; } = default!;
    }

    /// <summary>
    /// Provides event arguments for the <see cref="SfCalendar{TValue}.Navigated"/> event when calendar navigation occurs.
    /// </summary>
    /// <remarks>
    /// This class contains information about calendar navigation events, including the focused date, current view,
    /// and the original browser event that triggered the navigation. Navigation can occur when users change months,
    /// years, or decades in the calendar view.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// <SfCalendar TValue="DateTime?" Navigated="OnNavigated"></SfCalendar>
    /// 
    /// @code {
    ///     void OnNavigated(NavigatedEventArgs args)
    ///     {
    ///         Console.WriteLine($"Navigated to: {args.Date} in {args.View} view");
    ///     }
    /// }
    /// ]]></code>
    /// </example>
    public class NavigatedEventArgs
    {
        /// <summary>
        /// Gets or sets the focused date in the calendar view after navigation.
        /// </summary>
        /// <value>
        /// A <see cref="DateTime"/> value representing the focused date in the calendar view.
        /// </value>
        /// <remarks>
        /// This property indicates which date has focus after the navigation event occurs.
        /// The focused date determines which date is highlighted or has keyboard focus in the current view.
        /// </remarks>
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the original browser event that triggered the navigation.
        /// </summary>
        /// <value>
        /// An <c>object</c> containing the original event arguments from the browser. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property contains the raw browser event information, such as click or keyboard events,
        /// that caused the calendar navigation to occur. It provides access to the underlying event details.
        /// </remarks>
        [JsonPropertyName("event")]
        public object Event { get; set; } = default!;

        /// <summary>
        /// Gets or sets the name of the navigation event.
        /// </summary>
        /// <value>
        /// A <c>string</c> value representing the event name. The default value is <c>Empty String</c>.
        /// </value>
        /// <remarks>
        /// This property specifies the name identifier for the navigation event, which can be used to identify
        /// the specific type of navigation that occurred in event handling scenarios.
        /// </remarks>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the current view of the calendar after navigation.
        /// </summary>
        /// <value>
        /// A <c>string</c> value representing the current view name (e.g., "Month", "Year", "Decade"). The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property indicates the calendar view that is currently displayed after the navigation event.
        /// Common values include "Month" for month view, "Year" for year view, and "Decade" for decade view.
        /// </remarks>
        [JsonPropertyName("view")]
        public string View { get; set; } = default!;
    }

    /// <summary>
    /// Represents the details and state of a calendar cell, including unique identifier, style classes, and associated data.
    /// </summary>
    /// <remarks>
    /// The <see cref="CellDetails"/> class is used to provide detailed information about each cell in the calendar grid, including references to the cell element, mouse event, and the current date represented by the cell.
    /// </remarks>
    /// <example>
    /// Accessing the ID and current date of a cell in the calendar:
    /// <code><![CDATA[
    /// foreach (var cell in calendar.CellListData)
    /// {
    ///     var cellId = cell.CellID;
    ///     var date = cell.CurrentDate;
    /// }
    /// ]]></code>
    /// </example>
    public class CellDetails
    {
        /// <summary>
        /// Gets or sets the unique identifier for the calendar cell.
        /// </summary>
        /// <value>
        /// A <c>string</c> representing the unique ID of the cell.
        /// </value>
        /// <remarks>
        /// This ID can be used for referencing and updating the cell's state in the UI.
        /// </remarks>
        public string CellID { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the CSS class list applied to the cell element.
        /// </summary>
        /// <value>
        /// A <c>string</c> of CSS class names applied to the cell.
        /// </value>
        /// <remarks>
        /// Use this property to dynamically change the appearance of the cell (for highlighting, disabling, etc.).
        /// </remarks>
        public string ClassList { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the element reference for the calendar cell.
        /// </summary>
        /// <value>
        /// An <see cref="ElementReference"/> for direct DOM manipulation or focus control.
        /// </value>
        /// <remarks>
        /// This property is useful for advanced scenarios requiring JS interop or programmatic focus control.
        /// </remarks>
        public ElementReference? Element { get; set; }

        /// <summary>
        /// Gets or sets the mouse event arguments related to user interaction with the cell.
        /// </summary>
        /// <value>
        /// An instance of <see cref="MouseEventArgs"/> containing mouse event data.
        /// </value>
        /// <remarks>
        /// This property facilitates event handling for click, hover, and other pointer actions on the cell.
        /// </remarks>
        public MouseEventArgs EventArgs { get; set; } = new();

        /// <summary>
        /// Gets or sets the <see cref="DateTime"/> value for the date represented by this cell.
        /// </summary>
        /// <value>
        /// A <c>DateTime</c> indicating the specific calendar date associated with this cell.
        /// </value>
        /// <remarks>
        /// Use this property to access the actual date information rendered in the cell for processing selections, highlights, etc.
        /// </remarks>
        public DateTime CurrentDate { get; set; }
    }

    /// <summary>
    /// Provides event arguments for the <see cref="SfCalendar{TValue}.DayCellRendering"/> event when individual day cells are rendered.
    /// </summary>
    /// <remarks>
    /// This class contains information about individual day cells as they are rendered in the calendar view.
    /// It allows customization of day cell appearance, disabling specific dates, and accessing cell-specific data.
    /// This event is fired for each day cell that is rendered in the current calendar view.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// <SfCalendar TValue="DateTime?" DayCellRendering="DayCellRenderingHandler"></SfCalendar>
    ///
    /// @code {
    ///     void DayCellRenderingHandler(RenderDayCellEventArgs args)
    ///     {
    ///         // Disable weekends
    ///         if (args.Date.DayOfWeek == DayOfWeek.Saturday || args.Date.DayOfWeek == DayOfWeek.Sunday)
    ///         {
    ///             args.IsDisabled = true;
    ///         }
    ///     }
    /// }
    /// ]]></code>
    /// </example>
    public class RenderDayCellEventArgs
    {
        /// <summary>
        /// Gets or sets the date of the day cell being rendered.
        /// </summary>
        /// <value>
        /// A <see cref="DateTime"/> value representing the date of the current day cell being rendered.
        /// </value>
        /// <remarks>
        /// This property contains the specific date that corresponds to the day cell being rendered in the calendar view.
        /// It can be used to apply conditional logic based on the date, such as highlighting special dates or disabling certain dates.
        /// </remarks>
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current date should be disabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current date should be disabled and cannot be selected; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// This property allows you to programmatically disable specific dates during the cell rendering process.
        /// When set to <c>true</c>, the date will be displayed as disabled and cannot be selected by users.
        /// This is useful for implementing business rules like disabling weekends or holidays.
        /// </remarks>
        [JsonPropertyName("isDisabled")]
        public bool IsDisabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current date is outside the allowed range.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current date is outside the allowed range (less than Min or greater than Max); otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// This property indicates whether the current date falls outside the minimum and maximum date range
        /// configured for the calendar. Dates that are out of range are typically displayed as disabled.
        /// This property is automatically set by the calendar based on the Min and Max date configurations.
        /// </remarks>
        [JsonPropertyName("isOutOfRange")]
        public bool IsOutOfRange { get; set; }

        /// <summary>
        /// Gets or sets the detailed information about the calendar cell.
        /// </summary>
        /// <value>
        /// A <see cref="CellDetails"/> object containing detailed information about the calendar cell. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property provides comprehensive details about the calendar cell, including additional metadata
        /// that can be used for advanced customization scenarios. The cell data contains information about
        /// the cell's state, position, and other relevant properties.
        /// </remarks>
        public CellDetails CellData { get; set; } = default!;

        /// <summary>
        /// Gets or sets the name of the render day cell event.
        /// </summary>
        /// <value>
        /// A <c>string</c> value representing the event name. The default value is <c>Empty String</c>.
        /// </value>
        /// <remarks>
        /// This property specifies the name identifier for the render day cell event, which can be used
        /// to identify the specific event type in scenarios where multiple event handlers are used.
        /// </remarks>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the current view of the calendar when the day cell is being rendered.
        /// </summary>
        /// <value>
        /// A <c>string</c> value representing the calendar view such as <c>"Month"</c>, <c>"Year"</c>, or <c>"Decade"</c>.
        /// </value>
        /// <remarks>
        /// This property is useful for customizing the rendering of day cells based on the calendar's current view.
        /// A passed to the <see cref="SfCalendar{TValue}.DayCellRendering"/> event handlers.
        /// </remarks>
        /// <example>
        /// <code>
        /// void DayCellRenderingHandler(RenderDayCellEventArgs args)
        /// {
        ///     if (args.CurrentView == "Year")
        ///     {
        ///         args.CellData.ClassList += " highlight-year-view";
        ///     }
        /// }
        /// </code>
        /// </example>
        public string CurrentView { get; set; } = string.Empty;
    }

    /// <summary>
    /// Provides event arguments for the <see cref="SfDatePicker{TValue}.OnOpen"/> and <see cref="SfDatePicker{TValue}.OnClose"/> events.
    /// </summary>
    /// <remarks>
    /// This class contains information about popup open and close events for date picker components.
    /// It provides control over popup behavior, including the ability to cancel the action and specify
    /// where the popup should be appended in the DOM. The event arguments also include access to the original browser event.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// <SfDatePicker TValue="DateTime?" OnOpen="OnPopupOpen" OnClose="OnPopupClose"></SfDatePicker>
    /// 
    /// @code {
    ///     void OnPopupOpen(PopupObjectArgs args)
    ///     {
    ///         // Optionally cancel the popup opening
    ///         // args.Cancel = true;
    ///     }
    ///     
    ///     void OnPopupClose(PopupObjectArgs args)
    ///     {
    ///         Console.WriteLine("Popup is closing");
    ///     }
    /// }
    /// ]]></code>
    /// </example>
    public class PopupObjectArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether the popup action should be canceled.
        /// </summary>
        /// <value>
        /// <c>true</c> if the popup action should be canceled; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// This property allows you to prevent the popup from opening or closing by setting it to <c>true</c>.
        /// This is useful for implementing conditional popup behavior based on application state or user permissions.
        /// </remarks>
        [JsonPropertyName("cancel")]
        public bool Cancel { get; set; }

        /// <summary>
        /// Gets or sets the original browser event that triggered the popup action.
        /// </summary>
        /// <value>
        /// An <c>object</c> containing the original event arguments from the browser. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property contains the raw browser event information that caused the popup to open or close.
        /// It provides access to the underlying event details for advanced event handling scenarios.
        /// </remarks>
        [JsonPropertyName("event")]
        public object Event { get; set; } = default!;

        /// <summary>
        /// Gets or sets an object that can be used to prevent the default browser action.
        /// </summary>
        /// <value>
        /// An <c>object</c> that provides methods to prevent the default action. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property provides access to methods that can prevent the default browser behavior
        /// associated with the event that triggered the popup action. It allows for fine-grained control
        /// over event handling and browser behavior.
        /// </remarks>
        [JsonPropertyName("preventDefault")]
        public object PreventDefault { get; set; } = default!;
    }

    /// <summary>
    /// Provides strongly-typed event arguments for change events in calendar and time picker components.
    /// </summary>
    /// <typeparam name="T">The type of the component's value (typically <see cref="DateTime"/> or nullable <see cref="DateTime"/>).</typeparam>
    /// <remarks>
    /// This generic class provides comprehensive information about value changes in date/time picker components.
    /// It includes the new value, formatted text representation, interaction details, and references to the
    /// original browser event and DOM element. This allows for detailed handling of value change scenarios.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// <SfTimePicker TValue="DateTime?" ValueChange="OnTimeChanged"></SfTimePicker>
    /// 
    /// @code {
    ///     void OnTimeChanged(ChangeEventArgs<DateTime?> args)
    ///     {
    ///         if (args.IsInteracted && args.Value.HasValue)
    ///         {
    ///             Console.WriteLine($"User selected time: {args.Value}");
    ///             Console.WriteLine($"Formatted text: {args.Text}");
    ///         }
    ///     }
    /// }
    /// ]]></code>
    /// </example>
    public class ChangeEventArgs<T>
    {
        /// <summary>
        /// Gets or sets the original browser event that triggered the value change.
        /// </summary>
        /// <value>
        /// An <c>object</c> containing the original event arguments from the browser. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property contains the raw browser event information, such as input events, click events,
        /// or keyboard events, that caused the component's value to change. It provides access to the
        /// underlying event details for advanced event handling scenarios.
        /// </remarks>
        [JsonPropertyName("event")]
        public object Event { get; set; } = default!;

        /// <summary>
        /// Gets or sets a value indicating whether the change was triggered by user interaction.
        /// </summary>
        /// <value>
        /// <c>true</c> if the value change was triggered by user interaction; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// This property helps distinguish between programmatic value changes and user-initiated changes.
        /// When <c>true</c>, the value change was caused by user actions such as typing, clicking, or keyboard navigation.
        /// When <c>false</c>, the value change was likely caused by programmatic updates to the component's value.
        /// </remarks>
        [JsonPropertyName("isInteracted")]
        public bool IsInteracted { get; set; }

        /// <summary>
        /// Gets or sets the formatted text representation of the selected value.
        /// </summary>
        /// <value>
        /// A <c>string</c> containing the formatted value text. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property contains the human-readable string representation of the selected date/time value,
        /// formatted according to the component's format settings and culture-specific formatting rules.
        /// This is the text that appears in the component's input field.
        /// </remarks>
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the selected date/time value.
        /// </summary>
        /// <value>
        /// A value of type <c>T</c> representing the selected date/time value. The default value is <c>default(T)</c>.
        /// </value>
        /// <remarks>
        /// This property contains the strongly-typed date/time value that was selected or changed in the component.
        /// The type is determined by the generic parameter, allowing for flexibility in different date/time picker implementations.
        /// </remarks>
        [JsonPropertyName("value")]
        public T Value { get; set; } = default!;
    }

    /// <summary>
    /// Provides strongly-typed event arguments for item rendering events in list-based components.
    /// </summary>
    /// <typeparam name="T">The type of the item value being rendered (typically <see cref="DateTime"/> for time picker items).</typeparam>
    /// <remarks>
    /// This class contains information about individual list items as they are rendered in dropdown components
    /// such as time pickers. It allows customization of item appearance, disabling specific items, and accessing
    /// item-specific data during the rendering process. This event is fired for each item in the dropdown list.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// <SfTimePicker TValue="DateTime?" ItemRender="OnItemRender"></SfTimePicker>
    /// 
    /// @code {
    ///     void OnItemRender(ItemEventArgs<DateTime?> args)
    ///     {
    ///         // Disable lunch hour (12:00 PM)
    ///         if (args.Value?.Hour == 12 && args.Value?.Minute == 0)
    ///         {
    ///             args.IsDisabled = true;
    ///         }
    ///     }
    /// }
    /// ]]></code>
    /// </example>
    public class ItemEventArgs<T>
    {
        /// <summary>
        /// Gets or sets a value indicating whether the current item should be disabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current item should be disabled and cannot be selected; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// This property allows you to programmatically disable specific items during the rendering process.
        /// When set to <c>true</c>, the item will be displayed as disabled and cannot be selected by users.
        /// This is useful for implementing business rules such as disabling specific time slots or invalid options.
        /// </remarks>
        [JsonPropertyName("isDisabled")]
        public bool IsDisabled { get; set; }

        /// <summary>
        /// Gets or sets the name of the item rendering event.
        /// </summary>
        /// <value>
        /// A <c>string</c> value representing the event name. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property specifies the name identifier for the item rendering event, which can be used
        /// to identify the specific type of rendering event in scenarios where multiple event handlers are used.
        /// </remarks>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the display text for the list item.
        /// </summary>
        /// <value>
        /// A <c>string</c> containing the text to be displayed for the item. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property contains the formatted text that will be displayed to users in the dropdown list.
        /// You can modify this property to customize how items appear in the list, such as adding
        /// prefixes, suffixes, or applying different formatting rules.
        /// </remarks>
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the underlying value object for the list item.
        /// </summary>
        /// <value>
        /// A value of type <c>T</c> representing the actual value of the list item. The default value is <c>default(T)</c>.
        /// </value>
        /// <remarks>
        /// This property contains the strongly-typed value that corresponds to the list item being rendered.
        /// The type is determined by the generic parameter, allowing for flexibility in different component implementations.
        /// This value is what gets selected when the user chooses this item from the list.
        /// </remarks>
        [JsonPropertyName("value")]
        public T Value { get; set; } = default!;
    }

    /// <summary>
    /// Provides event arguments for popup open and close events in picker components.
    /// </summary>
    /// <remarks>
    /// This class contains information about popup state changes in various picker components such as time pickers.
    /// It provides control over popup behavior, including the ability to cancel the action and specify where
    /// the popup should be appended in the DOM. The event arguments also include access to the original browser event.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// <SfTimePicker TValue="DateTime?" OnOpen="OnPopupOpen" OnClose="OnPopupClose"></SfTimePicker>
    /// 
    /// @code {
    ///     void OnPopupOpen(PopupEventArgs args)
    ///     {
    ///         Console.WriteLine("Time picker popup is opening");
    ///         // Optionally cancel the opening
    ///         // args.Cancel = true;
    ///     }
    ///     
    ///     void OnPopupClose(PopupEventArgs args)
    ///     {
    ///         Console.WriteLine("Time picker popup is closing");
    ///     }
    /// }
    /// ]]></code>
    /// </example>
    public class PopupEventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether the popup action should be canceled.
        /// </summary>
        /// <value>
        /// <c>true</c> if the popup action should be canceled; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// This property allows you to prevent the popup from opening or closing by setting it to <c>true</c>.
        /// This is useful for implementing conditional popup behavior based on application state, validation results,
        /// or user permissions. When canceled, the popup action will be prevented and the popup state will remain unchanged.
        /// </remarks>
        [JsonPropertyName("cancel")]
        public bool Cancel { get; set; }

        /// <summary>
        /// Gets or sets the original browser event that triggered the popup action.
        /// </summary>
        /// <value>
        /// An <c>object</c> containing the original event arguments from the browser. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property contains the raw browser event information that caused the popup to open or close.
        /// It provides access to the underlying event details, such as click events, keyboard interactions,
        /// or focus events, for advanced event handling scenarios.
        /// </remarks>
        [JsonPropertyName("event")]
        public object Event { get; set; } = default!;

        /// <summary>
        /// Gets or sets the name of the popup event.
        /// </summary>
        /// <value>
        /// A <c>string</c> value representing the event name. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property specifies the name identifier for the popup event, which can be used to identify
        /// whether this is an open or close event in scenarios where the same event handler is used for both actions.
        /// </remarks>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents a comprehensive model for DateTimePicker component configuration and event handling.
    /// </summary>
    /// <typeparam name="T">The type of the date-time values (typically <see cref="DateTime"/> or nullable <see cref="DateTime"/>).</typeparam>
    /// <remarks>
    /// This class serves as a complete configuration model for DateTimePicker components, encapsulating all properties
    /// related to appearance, behavior, validation, time selection, calendar configuration, and event handling.
    /// It provides strongly-typed access to all DateTimePicker features including date selection, time interval settings,
    /// calendar views, formatting options, and accessibility features. The model supports both programmatic
    /// configuration and data binding scenarios for comprehensive date-time input functionality.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// <SfDateTimePicker TValue="DateTime?" @bind-Value="@Model.Value"
    ///                  Format="@Model.Format" Step="@Model.Step"
    ///                  ShowTodayButton="@Model.ShowTodayButton">
    /// </SfDateTimePicker>
    /// 
    /// @code {
    ///     DateTimePickerModel<DateTime?> Model = new DateTimePickerModel<DateTime?>
    ///     {
    ///         Format = "dd/MM/yyyy HH:mm",
    ///         Step = 15, // 15-minute intervals
    ///         ShowTodayButton = true,
    ///         AllowEdit = true
    ///     };
    /// }
    /// ]]></code>
    /// </example>
    public class DateTimePickerModel<T>
    {
        /// <summary>
        /// Triggers when the input loses the focus.
        /// </summary>
        [JsonIgnore]
        [JsonPropertyName("blur")]
        public EventCallback<object> Blur { get; set; }

        /// <summary>
        /// Triggers when the date or time value is changed.
        /// </summary>
        [JsonIgnore]
        [JsonPropertyName("change")]
        public EventCallback<object> Change { get; set; }

        /// <summary>
        /// Triggers when DateTimePicker value is cleared using clear button.
        /// </summary>
        [JsonIgnore]
        [JsonPropertyName("cleared")]
        public EventCallback<object> Cleared { get; set; }

        /// <summary>
        /// Triggers when the popup is closed.
        /// </summary>
        [DefaultValue(null)]
        [JsonPropertyName("close")]
        public PopupObjectArgs Close { get; set; } = default!;

        /// <summary>
        /// Triggers when the DateTimePicker is created.
        /// </summary>
        [JsonIgnore]
        [JsonPropertyName("created")]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// Triggers when the DateTimePicker is destroyed.
        /// </summary>
        [JsonIgnore]
        [JsonPropertyName("destroyed")]
        public EventCallback<object> Destroyed { get; set; }

        /// <summary>
        /// Triggers when the input gets focus.
        /// </summary>
        [JsonIgnore]
        [JsonPropertyName("focus")]
        public EventCallback<object> Focus { get; set; }

        /// <summary>
        /// Triggers when the Calendar is navigated to another view or within the same level of view.
        /// </summary>
        [JsonIgnore]
        [JsonPropertyName("navigated")]
        public EventCallback<object> Navigated { get; set; }

        /// <summary>
        /// Triggers when the popup is opened.
        /// </summary>
        [DefaultValue(null)]
        [JsonPropertyName("open")]
        public PopupObjectArgs Open { get; set; } = default!;

        /// <summary>
        /// Triggers when each day cell of the Calendar is rendered.
        /// </summary>
        [JsonIgnore]
        [JsonPropertyName("renderDayCell")]
        public EventCallback<object> RenderDayCell { get; set; }

        /// <summary>
        /// Specifies a boolean value whether the DateTimePicker allows user to change the value via typing. When set as false, the DateTimePicker allows user to change the value via picker only.
        /// </summary>
        [DefaultValue(true)]
        [JsonPropertyName("allowEdit")]
        public bool AllowEdit { get; set; } = true;

        /// <summary>
        /// Gets or sets the Calendar's Type like gregorian or islamic.
        /// </summary>
        [DefaultValue(CalendarType.Gregorian)]
        [JsonPropertyName("calendarMode")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CalendarType CalendarMode { get; set; } = CalendarType.Gregorian;

        /// <summary>
        /// Specifies the CSS class name that can be appended with the root element of the DateTimePicker. One or more custom CSS classes can be added to a DateTimePicker.
        /// </summary>
        [DefaultValue(null)]
        [JsonPropertyName("cssClass")]
        public string CssClass { get; set; } = default!;

        /// <summary>
        /// Specifies the format of the day that to be displayed in the header. By default, the format is Short.
        /// <para>Possible formats are:</para>
        /// <list type="bullet">
        /// <item>
        /// <term>Short</term>
        /// <description>Sets the short format of day name (like Su ) in day header.</description>
        /// </item>
        /// <item>
        /// <term>Narrow</term>
        /// <description>Sets the single character of day name (like S ) in day header.</description>
        /// </item>
        /// <item>
        /// <term>Abbreviated</term>
        /// <description>Sets the min format of day name (like Sun ) in day header.</description>
        /// </item>
        /// <item>
        /// <term>Wide</term>
        /// <description>Sets the long format of day name (like Sunday ) in day header.</description>
        /// </item>
        /// </list>.
        /// </summary>
        [DefaultValue(DayHeaderFormats.Short)]
        [JsonPropertyName("dayHeaderFormat")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DayHeaderFormats DayHeaderFormat { get; set; } = DayHeaderFormats.Short;

        /// <summary>
        /// Sets the maximum level of view (Month, Year, Decade) in the Calendar.
        /// <para>Depth view should be smaller than the Start view to restrict its view navigation.</para>.
        /// </summary>
        [DefaultValue(CalendarView.Month)]
        [JsonPropertyName("depth")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CalendarView Depth { get; set; } = CalendarView.Month;

        /// <summary>
        /// Enable or disable persisting DateTimePicker's state between page reloads. If enabled, the `Value` state will be persisted.
        /// </summary>
        [DefaultValue(false)]
        [JsonPropertyName("enablePersistence")]
        public bool EnablePersistence { get; set; } = false;

        /// <summary>
        /// Specifies a boolean value that indicates whether the DateTimePicker allows the user to interact with it.
        /// </summary>
        [DefaultValue(false)]
        [JsonPropertyName("disabled")]
        public bool Disabled { get; set; } = false;

        /// <summary>
        /// Sets the calendar's first day of the week. By default, the first day of the week will be based on the current culture.
        /// </summary>
        [DefaultValue(0)]
        [JsonPropertyName("firstDayOfWeek")]
        public int FirstDayOfWeek { get; set; }

        /// <summary>
        /// Specifies the floating label behavior of the DateTimePicker that the placeholder text floats above the DateTimePicker based on the following values.
        /// <para>Possible values are:</para>
        /// <list type="bullet">
        /// <item>
        /// <term>Never</term>
        /// <description>Never floats the label in the DateTimePicker when the placeholder is available.</description>
        /// </item>
        /// <item>
        /// <term>Always</term>
        /// <description>The floating label always floats above the DateTimePicker.</description>
        /// </item>
        /// <item>
        /// <term>Auto</term>
        /// <description>The floating label floats above the DateTimePicker after focusing it or when enters the value in it.</description>
        /// </item>
        /// </list>.
        /// </summary>
        [DefaultValue(FloatLabelType.Never)]
        [JsonPropertyName("floatLabelType")]
        public FloatLabelType FloatLabelType { get; set; } = FloatLabelType.Never;

        /// <summary>
        /// Specifies the format of the value that to be displayed in DateTimePicker.
        /// <para>By default, the format is based on the culture.</para>
        /// <para>You can set the format to "format:'dd/MM/yyyy hh:mm'".</para>.
        /// </summary>
        [DefaultValue(null)]
        [JsonPropertyName("format")]
        public string Format { get; set; } = default!;

        /// <summary>
        /// You can add the additional input attributes such as disabled, value, and more to the root element.
        /// <para>If you configured both the property and equivalent input attribute, then the DateTimePicker considers the property value.</para>.
        /// </summary>
        [DefaultValue(null)]
        [JsonPropertyName("htmlAttributes")]
        public object HtmlAttributes { get; set; } = default!;

        /// <summary>
        /// Customizes the key actions in DateTimePicker.
        /// For example, when using German keyboard, the key actions can be customized using these shortcuts.
        /// </summary>
        [DefaultValue(null)]
        [JsonPropertyName("keyConfigs")]
        public object KeyConfigs { get; set; } = default!;

        /// <summary>
        /// Specifies the global culture and localization of the DateTimePicker.
        /// </summary>
        [DefaultValue("")]
        [JsonPropertyName("locale")]
        public string Locale { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the maximum date that can be selected in the DateTimePicker.
        /// </summary>
        [JsonPropertyName("max")]
        public DateTime Max { get; set; } = new DateTime(2099, 12, 31);

        /// <summary>
        /// Gets or sets the minimum date that can be selected in the DateTimePicker.
        /// </summary>
        [JsonPropertyName("min")]
        public DateTime Min { get; set; } = new DateTime(1900, 01, 01);

        /// <summary>
        /// Specifies the text that is shown as a hint or placeholder until the user focuses or enter a value in DateTimePicker. The property is depending on the FloatLabelType property.
        /// </summary>
        [DefaultValue(null)]
        [JsonPropertyName("placeholder")]
        public string Placeholder { get; set; } = default!;

        /// <summary>
        /// Specifies a boolean value whether the DateTimePicker allows the user to change the text.
        /// </summary>
        [DefaultValue(false)]
        [JsonPropertyName("readonly")]
        public bool Readonly { get; set; } = false;

        /// <summary>
        /// Specifies the scroll bar position, if there is no value is selected in the timepicker popup list or
        /// the given value is not present in the timepicker popup list.
        /// </summary>
        [JsonPropertyName("scrollTo")]
        public DateTime ScrollTo { get; set; }

        /// <summary>
        /// By default, the date value will be processed based on system time zone.
        /// If you want to process the initial date value using server time zone
        /// then specify the time zone value to `ServerTimezoneOffset` property.
        /// </summary>
        [DefaultValue(default(double))]
        [JsonPropertyName("serverTimezoneOffset")]
        public double ServerTimezoneOffset { get; set; } = default;

        /// <summary>
        /// Specifies whether to show or hide the clear icon in textbox.
        /// </summary>
        [DefaultValue(true)]
        [JsonPropertyName("showClearButton")]
        public bool ShowClearButton { get; set; } = true;

        /// <summary>
        /// Specifies whether the today button is to be displayed or not.
        /// </summary>
        [DefaultValue(true)]
        [JsonPropertyName("showTodayButton")]
        public bool ShowTodayButton { get; set; } = true;

        /// <summary>
        /// Specifies the initial view of the Calendar when it is opened.
        /// With the help of this property, initial view can be changed to year or decade view.
        /// </summary>
        [DefaultValue(CalendarView.Month)]
        [JsonPropertyName("start")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CalendarView Start { get; set; } = CalendarView.Month;

        /// <summary>
        /// Specifies the time interval between the two adjacent time values in the time popup list .
        /// </summary>
        [DefaultValue(30)]
        [JsonPropertyName("step")]
        public int Step { get; set; } = 30;

        /// <summary>
        /// Specifies the DateTimePicker to act as strict. So that, it allows to enter only a valid date value within a specified range or else it will resets to previous value.
        /// <para> By default, StrictMode is in false. It allows invalid or out-of-range date value with highlighted error class.</para>
        /// </summary>
        [DefaultValue(false)]
        [JsonPropertyName("strictMode")]
        public bool StrictMode { get; set; } = false;

        /// <summary>
        /// Specifies the format of the time value that to be displayed in time popup list.
        /// </summary>
        [DefaultValue(null)]
        [JsonPropertyName("timeFormat")]
        public string TimeFormat { get; set; } = default!;

        /// <summary>
        /// Gets or sets the selected date of the Calendar.
        /// </summary>
        [JsonPropertyName("value")]
        public T Value { get; set; } = default!;

        /// <summary>
        /// Determines whether the week number of the year is to be displayed in the calendar or not.
        /// </summary>
        [DefaultValue(false)]
        [JsonPropertyName("weekNumber")]
        public bool WeekNumber { get; set; } = false;

        /// <summary>
        /// Specifies the width of the DateTimePicker component.
        /// </summary>
        [DefaultValue(null)]
        [JsonPropertyName("width")]
        public object Width { get; set; } = default!;

        /// <summary>
        /// Specifies the z-index value of the DateTimePicker popup element.
        /// </summary>
        [DefaultValue(1000)]
        [JsonPropertyName("zIndex")]
        public int ZIndex { get; set; } = 1000;
    }

    /// <summary>
    /// Represents a comprehensive model for DatePicker component configuration.
    /// </summary>
    /// <remarks>
    /// This class serves as a complete configuration model for DatePicker components, encapsulating all properties
    /// related to appearance, behavior, validation, localization, and accessibility. It provides strongly-typed
    /// access to all DatePicker features including date selection, formatting options, input validation,
    /// and user interface customization. The model supports both programmatic configuration and data binding
    /// scenarios for comprehensive date input functionality.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// <SfDatePicker TValue="DateTime?" @bind-Value="@SelectedDate"
    ///              AllowEdit="@Model.AllowEdit" Format="@Model.Format"
    ///              ShowClearButton="@Model.ShowClearButton" Placeholder="@Model.Placeholder">
    /// </SfDatePicker>
    /// 
    /// @code {
    ///     DateTime? SelectedDate = DateTime.Today;
    ///     
    ///     DatePickerModel Model = new DatePickerModel
    ///     {
    ///         AllowEdit = true,
    ///         Format = "dd/MM/yyyy",
    ///         ShowClearButton = true,
    ///         Placeholder = "Select a date...",
    ///         StrictMode = false
    ///     };
    /// }
    /// ]]></code>
    /// </example>
    public class DatePickerModel
    {
        /// <summary>
        /// Specifies a boolean value whether the DatePicker allows user to change the value via typing. When set as false, the DatePicker allows user to change the value via picker only.
        /// </summary>
        public bool AllowEdit { get; set; } = true;

        /// <summary>
        /// Specifies the CSS class name that can be appended with the root element of the DatePicker. One or more custom CSS classes can be added to a DatePicker.
        /// </summary>
        public string CssClass { get; set; } = string.Empty;

        /// <summary>
        /// Specifies a boolean value that indicates whether the DatePicker allows the user to interact with it.
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// Sets the calendar's first day of the week. By default, the first day of the week will be based on the current culture.
        /// </summary>
        public int FirstDayOfWeek { get; set; }

        /// <summary>
        /// Specifies the floating label behavior of the DatePicker that the placeholder text floats above the DatePicker based on the following values.
        /// <para>Possible values are:</para>
        /// <list type="bullet">
        /// <item>
        /// <term>Never</term>
        /// <description>Never floats the label in the DatePicker when the placeholder is available.</description>
        /// </item>
        /// <item>
        /// <term>Always</term>
        /// <description>The floating label always floats above the DatePicker.</description>
        /// </item>
        /// <item>
        /// <term>Auto</term>
        /// <description>The floating label floats above the DatePicker after focusing it or when enters the value in it.</description>
        /// </item>
        /// </list>.
        /// </summary>
        public FloatLabelType FloatLabelType { get; set; }

        /// <summary>
        /// Specifies the format of the value that to be displayed in component.
        /// <para>By default, the format is based on the culture.</para>
        /// <para>You can set the format to "format:'dd/MM/yyyy hh:mm'".</para>.
        /// </summary>
        public string Format { get; set; } = string.Empty;

        /// <summary>
        /// You can add the additional html attributes such as styles, class, and more to the root element.
        /// <para>If you configured both the property and equivalent html attribute, then the component considers the property value.</para>.
        /// </summary>
        public Dictionary<string, object> HtmlAttributes { get; set; } = default!;

        /// <summary>
        /// You can add the additional input attributes such as disabled, value, and more to the root element.
        /// <para>If you configured both the property and equivalent input attribute, then the component considers the property value.</para>.
        /// </summary>
        public Dictionary<string, object> InputAttributes { get; set; } = default!;

        /// <summary>
        /// Specifies the text that is shown as a hint or placeholder until the user focuses or enter a value in DatePicker. The property is depending on the FloatLabelType property.
        /// </summary>
        public string Placeholder { get; set; } = string.Empty;

        /// <summary>
        /// Specifies the boolean value whether the DatePicker allows the user to change the text.
        /// </summary>
        public bool Readonly { get; set; }

        /// <summary>
        /// Specifies a boolean value that indicates whether the clear button is displayed in DatePicker.
        /// </summary>
        public bool ShowClearButton { get; set; } = true;

        /// <summary>
        /// Specifies the component to act as strict. So that, it allows to enter only a valid date  value within a specified range or else it will resets to previous value.
        /// </summary>
        /// <para> By default, StrictMode is in false. It allows invalid or out-of-range date value with highlighted error class.</para>
        public bool StrictMode { get; set; }

        /// <summary>
        /// Specifies the width of the DatePicker component.
        /// </summary>
        public string Width { get; set; } = string.Empty;

        /// <summary>
        /// specifies the z-index value of the DatePicker popup element.
        /// </summary>
        public int ZIndex { get; set; } = 1000;

        /// <summary>
        /// Specifies the tab order of the DatePicker component.
        /// </summary>
        public int TabIndex { get; set; }
    }

    /// <summary>
    /// Represents a comprehensive model for TimePicker component configuration and event handling.
    /// </summary>
    /// <typeparam name="T">The type of the time values (typically <see cref="DateTime"/> or nullable <see cref="DateTime"/>).</typeparam>
    /// <remarks>
    /// This class serves as a complete configuration model for TimePicker components, encapsulating all properties
    /// related to appearance, behavior, validation, time selection, and event handling. It provides strongly-typed
    /// access to all TimePicker features including time interval settings, format customization, popup behavior,
    /// and accessibility features. The model supports both programmatic configuration and data binding scenarios
    /// for comprehensive time input functionality.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// <SfTimePicker TValue="DateTime?" @bind-Value="@Model.Value"
    ///              Format="@Model.Format" Step="@Model.Step"
    ///              ShowClearButton="@Model.ShowClearButton" 
    ///              ValueChange="@Model.Change"
    ///              ItemRender="@Model.ItemRender">
    /// </SfTimePicker>
    /// 
    /// @code {
    ///     TimePickerModel<DateTime?> Model = new TimePickerModel<DateTime?>
    ///     {
    ///         Format = "HH:mm",
    ///         Step = 30, // 30-minute intervals
    ///         ShowClearButton = true,
    ///         AllowEdit = false // Picker-only mode
    ///     };
    ///     
    ///     void OnItemRender(ItemEventArgs<DateTime?> args)
    ///     {
    ///         // Custom item rendering logic
    ///         if (args.Value?.Hour < 9 || args.Value?.Hour > 17)
    ///         {
    ///             args.IsDisabled = true; // Disable outside business hours
    ///         }
    ///     }
    /// }
    /// ]]></code>
    /// </example>
    public class TimePickerModel<T>
    {
        /// <summary>
        /// Triggers when the input loses the focus.
        /// </summary>
        [JsonIgnore]
        [JsonPropertyName("blur")]
        public EventCallback<object> Blur { get; set; }

        /// <summary>
        /// Triggers when the time value is changed.
        /// </summary>
        [JsonIgnore]
        [JsonPropertyName("change")]
        public EventCallback<object> Change { get; set; }

        /// <summary>
        /// Triggers when TimePicker value is cleared using clear button.
        /// </summary>
        [JsonIgnore]
        [JsonPropertyName("cleared")]
        public EventCallback<object> Cleared { get; set; }

        /// <summary>
        /// Triggers when the popup is closed.
        /// </summary>
        [JsonIgnore]
        [JsonPropertyName("close")]
        public EventCallback<object> Close { get; set; }

        /// <summary>
        /// Triggers when the TimePicker is created.
        /// </summary>
        [JsonIgnore]
        [JsonPropertyName("created")]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// Triggers when the TimePicker is destroyed.
        /// </summary>
        [JsonIgnore]
        [JsonPropertyName("destroyed")]
        public EventCallback<object> Destroyed { get; set; }

        /// <summary>
        /// Triggers when the input gets focus.
        /// </summary>
        [JsonIgnore]
        [JsonPropertyName("focus")]
        public EventCallback<object> Focus { get; set; }

        /// <summary>
        /// Triggers while rendering the each popup list item.
        /// </summary>
        [JsonIgnore]
        [JsonPropertyName("itemRender")]
        public EventCallback<object> ItemRender { get; set; }

        /// <summary>
        /// Triggers when the popup is opened.
        /// </summary>
        [JsonIgnore]
        [JsonPropertyName("open")]
        public EventCallback<object> Open { get; set; }

        /// <summary>
        /// Specifies a boolean value whether the TimePicker allows user to change the value via typing. When set as false, the TimePicker allows user to change the value via picker only.
        /// </summary>
        [DefaultValue(true)]
        [JsonPropertyName("allowEdit")]
        public bool AllowEdit { get; set; } = true;

        /// <summary>
        /// Specifies the CSS class name that can be appended with the root element of the TimePicker. One or more custom CSS classes can be added to a TimePicker.
        /// </summary>
        [DefaultValue(null)]
        [JsonPropertyName("cssClass")]
        public string CssClass { get; set; } = string.Empty;

        /// <summary>
        /// Enable or disable persisting TimePicker's state between page reloads. If enabled, the `Value` state will be persisted.
        /// </summary>
        [DefaultValue(false)]
        [JsonPropertyName("enablePersistence")]
        public bool EnablePersistence { get; set; } = false;

        /// <summary>
        /// Specifies a boolean value that indicates whether the TimePicker allows the user to interact with it.
        /// </summary>
        [DefaultValue(false)]
        [JsonPropertyName("disabled")]
        public bool Disabled { get; set; } = false;

        /// <summary>
        /// Specifies the floating label behavior of the TimePicker that the placeholder text floats above the TimePicker based on the following values.
        /// <para>Possible values are:</para>
        /// <list type="bullet">
        /// <item>
        /// <term>Never</term>
        /// <description>Never floats the label in the TimePicker when the placeholder is available.</description>
        /// </item>
        /// <item>
        /// <term>Always</term>
        /// <description>The floating label always floats above the TimePicker.</description>
        /// </item>
        /// <item>
        /// <term>Auto</term>
        /// <description>The floating label floats above the TimePicker after focusing it or when enters the value in it.</description>
        /// </item>
        /// </list>.
        /// </summary>
        [DefaultValue(FloatLabelType.Never)]
        [JsonPropertyName("floatLabelType")]
        public FloatLabelType FloatLabelType { get; set; } = FloatLabelType.Never;

        /// <summary>
        /// Specifies the format of the value that to be displayed in TimePicker.
        /// <para>By default, the format is based on the culture.</para>.
        /// </summary>
        [DefaultValue(null)]
        [JsonPropertyName("format")]
        public string Format { get; set; } = string.Empty;

        /// <summary>
        /// You can add the additional input attributes such as disabled, value, and more to the root element.
        /// <para>If you configured both the property and equivalent input attribute, then the TimePicker considers the property value.</para>.
        /// </summary>
        [DefaultValue(null)]
        [JsonPropertyName("htmlAttributes")]
        public object HtmlAttributes { get; set; } = default!;

        /// <summary>
        /// Customizes the key actions in TimePicker.
        /// For example, when using German keyboard, the key actions can be customized using these shortcuts.
        /// </summary>
        [DefaultValue(null)]
        [JsonPropertyName("keyConfigs")]
        public object KeyConfigs { get; set; } = default!;

        /// <summary>
        /// Specifies the global culture and localization of the TimePicker.
        /// </summary>
        [DefaultValue("")]
        [JsonPropertyName("locale")]
        public string Locale { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the maximum time value that can be allowed to select in TimePicker.
        /// </summary>
        [JsonPropertyName("max")]
        public DateTime Max { get; set; } = new DateTime(2099, 12, 31, 23, 59, 59);

        /// <summary>
        /// Gets or sets the minimum time value that can be allowed to select in TimePicker.
        /// </summary>
        [JsonPropertyName("min")]
        public DateTime Min { get; set; } = new DateTime(1900, 01, 01, 00, 00, 00);

        /// <summary>
        /// Specifies the text that is shown as a hint or placeholder until the user focuses or enter a value in TimePicker. The property is depending on the FloatLabelType property.
        /// </summary>
        [DefaultValue(null)]
        [JsonPropertyName("placeholder")]
        public string Placeholder { get; set; } = string.Empty;

        /// <summary>
        /// Specifies a boolean value whether the TimePicker allows the user to change the text.
        /// </summary>
        [DefaultValue(false)]
        [JsonPropertyName("readonly")]
        public bool Readonly { get; set; } = false;

        /// <summary>
        /// Specifies the scroll bar position, if there is no value is selected in the popup list or
        /// the given value is not present in the popup list.
        /// </summary>
        [JsonPropertyName("scrollTo")]
        public DateTime ScrollTo { get; set; }

        /// <summary>
        /// Specifies whether to show or hide the clear icon.
        /// </summary>
        [DefaultValue(true)]
        [JsonPropertyName("showClearButton")]
        public bool ShowClearButton { get; set; } = true;

        /// <summary>
        /// Specifies the time interval between the two adjacent time values in the popup list.
        /// </summary>
        [DefaultValue(30)]
        [JsonPropertyName("step")]
        public int Step { get; set; } = 30;

        /// <summary>
        /// Specifies the TimePicker to act as strict. So that, it allows to enter only a valid time value within a specified range or else it will resets to previous value.
        /// <para> By default, StrictMode is in false. It allows invalid or out-of-range time value with highlighted error class.</para>
        /// </summary>
        [DefaultValue(false)]
        [JsonPropertyName("strictMode")]
        public bool StrictMode { get; set; } = false;

        /// <summary>
        /// Gets or sets the value of the TimePicker. The value is parsed based on the culture specific time format.
        /// </summary>
        [JsonPropertyName("value")]
        public T Value { get; set; } = default!;

        /// <summary>
        /// Specifies the width of the TimePicker component.
        /// </summary>
        [DefaultValue(null)]
        [JsonPropertyName("width")]
        public object Width { get; set; } = default!;

        /// <summary>
        /// specifies the z-index value of the timePicker popup element.
        /// </summary>
        [DefaultValue(1000)]
        [JsonPropertyName("zIndex")]
        public int ZIndex { get; set; } = 1000;
    }
}