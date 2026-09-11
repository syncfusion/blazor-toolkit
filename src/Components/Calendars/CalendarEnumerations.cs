using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Toolkit
{
    /// <summary> 
    /// Specifies the calendar system to be used in calendar components. 
    /// </summary> 
    /// <remarks> 
    /// The <see cref="CalendarType"/> enum allows selecting between different calendar systems such as Gregorian and Islamic (Hijri).  
    /// It determines how dates are calculated, displayed, and handled in components such as <c>SfCalendar</c>, <c>SfDatePicker</c>, and <c>SfDateTimePicker</c>.
    /// The calendar type affects date calculations, month names, and cultural formatting.
    /// </remarks>
    /// <example>
    /// Setting the calendar type to Islamic:
    /// <code><![CDATA[
    /// <SfCalendar TValue="DateTime" CalendarMode="CalendarType.Islamic"></SfCalendar>
    /// ]]></code>
    /// </example>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CalendarType
    {
        /// <summary>
        /// Represents the Gregorian calendar system, which is the internationally accepted civil calendar.
        /// </summary>
        /// <remarks>
        /// This is the default calendar type used worldwide. The Gregorian calendar has 12 months with varying day counts,
        /// and includes leap years every four years (with some exceptions). It starts from January 1st as the new year.
        /// </remarks>
        [EnumMember(Value = "Gregorian")]
        Gregorian,

        /// <summary>
        /// Represents the Islamic (Hijri) calendar system used in Islamic cultures.
        /// </summary>
        /// <remarks>
        /// The Islamic calendar is a lunar calendar consisting of 12 months with approximately 354 or 355 days in a year.
        /// It starts from the year of Prophet Muhammad's migration to Medina (622 CE in the Gregorian calendar).
        /// Each month begins with the sighting of the new moon.
        /// </remarks>
        [EnumMember(Value = "Islamic")]
        Islamic,
    }

    /// <summary>
    /// Specifies the display format for day names in the calendar header.
    /// </summary>
    /// <remarks>
    /// The <see cref="DayHeaderFormats"/> enum controls how day names are displayed in the header row of calendar components.
    /// This affects the visual appearance and space utilization of the calendar, allowing customization based on available space and user preferences.
    /// Different formats provide varying levels of detail for day identification.
    /// </remarks>
    /// <example>
    /// Setting the day header format:
    /// <code><![CDATA[
    /// <SfCalendar TValue="DateTime" DayHeaderFormat="DayHeaderFormats.Abbreviated"></SfCalendar>
    /// ]]></code>
    /// </example>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DayHeaderFormats
    {
        /// <summary>
        /// Displays day names in short format, typically showing two characters (e.g., "Su", "Mo", "Tu").
        /// </summary>
        /// <remarks>
        /// This format provides a compact representation while still being easily recognizable.
        /// It's suitable for calendars where space is moderately constrained but readability remains important.
        /// </remarks>
        [EnumMember(Value = "Short")]
        Short,

        /// <summary>
        /// Displays day names as single characters (e.g., "S", "M", "T").
        /// </summary>
        /// <remarks>
        /// This is the most compact format, using only the first letter of each day name.
        /// It's ideal for mobile interfaces or very small calendar displays where space is at a premium.
        /// Note that some days may share the same initial letter in certain locales.
        /// </remarks>
        [EnumMember(Value = "Narrow")]
        Narrow,

        /// <summary>
        /// Displays day names in abbreviated format, typically showing three characters (e.g., "Sun", "Mon", "Tue").
        /// </summary>
        /// <remarks>
        /// This format provides a good balance between space efficiency and clarity.
        /// It's commonly used in many calendar applications as it's easily readable while not taking up excessive space.
        /// </remarks>
        [EnumMember(Value = "Abbreviated")]
        Abbreviated,

        /// <summary>
        /// Displays day names in full format, showing complete day names (e.g., "Sunday", "Monday", "Tuesday").
        /// </summary>
        /// <remarks>
        /// This format provides the clearest representation of day names but requires the most space.
        /// It's suitable for large calendar displays where readability is prioritized over space conservation.
        /// </remarks>
        [EnumMember(Value = "Wide")]
        Wide,
    }

    /// <summary>
    /// Specifies the different view levels available for calendar components.
    /// </summary>
    /// <remarks>
    /// The <see cref="CalendarView"/> enum defines the hierarchical view levels that can be displayed in calendar components such as <c>SfCalendar</c>, <c>SfDatePicker</c>, and <c>SfDateTimePicker</c>.
    /// Users can navigate between these views to select dates at different levels of granularity.
    /// </remarks>
    /// <example>
    /// Setting the calendar view:
    /// <code><![CDATA[
    /// <SfCalendar TValue="DateTime" View="CalendarView.Year"></SfCalendar>
    /// ]]></code>
    /// </example>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CalendarView
    {
        /// <summary>
        /// Displays the calendar in month view, showing individual days of a specific month.
        /// </summary>
        /// <remarks>
        /// This is the default and most detailed view level, allowing users to select specific dates within a month.
        /// The month view displays all days in a grid format with day names as headers.
        /// </remarks>
        [EnumMember(Value = "Month")]
        Month,

        /// <summary>
        /// Displays the calendar in year view, showing all months of a specific year.
        /// </summary>
        /// <remarks>
        /// In year view, users can select entire months rather than individual dates.
        /// This view is useful when month-level selection is required or when navigating to different months quickly.
        /// </remarks>
        [EnumMember(Value = "Year")]
        Year,

        /// <summary>
        /// Displays the calendar in decade view, showing a range of years within a decade.
        /// </summary>
        /// <remarks>
        /// The decade view provides the highest level of navigation, allowing users to select entire years.
        /// This view is typically used for quick navigation across multiple years or when year-level selection is needed.
        /// </remarks>
        [EnumMember(Value = "Decade")]
        Decade,
    }
}
