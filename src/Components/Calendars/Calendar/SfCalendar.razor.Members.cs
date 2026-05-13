using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace Syncfusion.Blazor.Toolkit.Calendars
{
    /// <summary>
    /// Represents a graphical Calendar UI component that displays a Gregorian calendar and allows users to select one or more dates with ease and accessibility.
    /// </summary>
    /// <remarks>
    /// The <see cref="SfCalendar{TValue}"/> provides date picking functionality suitable for forms, standalone popups, or embedded calendar scenarios and can be customized for appearance and behavior.
    /// </remarks>
    /// <example>
    /// Example of using SfCalendar component with multi-date selection enabled:
    /// <code><![CDATA[
    /// <SfCalendar TValue="DateTime" IsMultiSelection="true" @bind-Values="SelectedDates"></SfCalendar>
    /// ]]></code>
    /// </example>
    public partial class SfCalendar<TValue> : CalendarBase<TValue>
    {
        /// <summary>
        /// Gets or sets the <see cref="Expression"/> used to bind multiple values to the calendar in two-way binding scenarios.
        /// </summary>
        /// <value>
        /// An expression of lambda type <c>Expression&lt;Func&lt;DateTime[]&gt;&gt;</c> for advanced data binding scenarios, especially in forms or custom validation.
        /// </value>
        /// <remarks>
        /// Typically used internally for validation and model binding when implementing complex forms with multiple selected dates.
        /// </remarks>
        /// <example>
        /// Example usage inside a form for complex model binding:
        /// <code><![CDATA[
        /// <EditForm Model="MyModel">
        ///     <SfCalendar TValue="DateTime"
        ///         ValuesExpression="@(() => MyModel.SelectedDates)"
        ///         @bind-Values="MyModel.SelectedDates">
        ///     </SfCalendar>
        /// </EditForm>
        /// ]]></code>
        /// </example>
        [Parameter]
        public Expression<Func<DateTime[]>> ValuesExpression { get; set; } = default!;

        /// <summary>
        /// Gets or sets a value that indicates whether the calendar allows selection of multiple dates concurrently.
        /// </summary>
        /// <value>
        /// <c>true</c> to enable multiple date selection; otherwise, <c>false</c>. The default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// When set to <c>true</c>, the user can select multiple non-contiguous dates within the calendar. When <c>false</c>, single date selection mode applies.
        /// </remarks>
        /// <example>
        /// Enable multiple date selection on the calendar:
        /// <code><![CDATA[
        /// <SfCalendar TValue="DateTime" IsMultiSelection="true" @bind-Values="SelectedDates"></SfCalendar>
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool IsMultiSelection { get; set; }

        /// <summary>
        /// Gets or sets the tab index which determines the tab order for the component in the page.
        /// </summary>
        /// <value>
        /// An <c>int</c> representing the tab index. The default is <c>0</c>.
        /// </value>
        /// <remarks>
        /// This value controls the order in which focus shifts when the user presses the Tab key.
        /// </remarks>
        /// <example>
        /// Set a custom tab index for accessibility:
        /// <code><![CDATA[
        /// <SfCalendar TValue="DateTime" TabIndex="2"></SfCalendar>
        /// ]]></code>
        /// </example>
        [Parameter]
        public int TabIndex { get; set; }

        /// <summary>
        /// Gets or sets an array of <see cref="DateTime"/> values representing the selected dates in the calendar.
        /// </summary>
        /// <value>
        /// An array of <see cref="DateTime"/> objects that indicate the currently selected dates.
        /// </value>
        /// <remarks>
        /// This can be used for binding multiple selected dates in multi-selection mode. Binding this property will update the calendar with the selected values.
        /// </remarks>
        /// <example>
        /// Bind selected date values from a model:
        /// <code><![CDATA[
        /// <SfCalendar TValue="DateTime" @bind-Values="MySelectedDates"></SfCalendar>
        /// ]]></code>
        /// </example>
        [Parameter]
        public DateTime[] Values { get; set; } = default!;

        /// <summary>
        /// Gets or sets the event callback that is triggered when the <see cref="Values"/> property changes.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{T}"/> of type <c>DateTime[]</c> invoked when the selected dates are updated by the user.
        /// </value>
        /// <remarks>
        /// This event can be used to perform additional actions or synchronize state when the collection of selected dates changes.
        /// </remarks>
        /// <example>
        /// Subscribe to value change events in code:
        /// <code><![CDATA[
        /// <SfCalendar TValue="DateTime" @bind-Values="SelectedDates" ValuesChanged="OnValuesChanged"></SfCalendar>
        /// @code {
        ///     private void OnValuesChanged(DateTime[] newValues) {
        ///         // Handle the changed dates
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<DateTime[]> ValuesChanged { get; set; }

        /// <summary>
        /// Gets or sets a collection of additional HTML attributes such as styles, CSS classes, and other custom settings to be applied to the calendar's root element.
        /// </summary>
        /// <value>A <see cref="Dictionary{TKey, TValue}"/> of <c>string</c> and <c>object</c> pairs. The default is <c>null</c>.</value>
        /// <remarks>
        /// Use this property to attach custom styles, attributes, or data-* attributes to the calendar component for advanced customization and integration scenarios.
        /// </remarks>
        /// <example>
        /// Add custom classes and styles to the calendar:
        /// <code><![CDATA[
        /// <SfCalendar TValue="DateTime" HtmlAttributes="@new Dictionary<string, object>{{ "class", "custom-calendar" }, { "data-type", "events" }}"></SfCalendar>
        /// ]]></code>
        /// </example>
        [Parameter]
        public Dictionary<string, object> HtmlAttributes { get; set; } = default!;

        private string Calendar_CssClass { get; set; } = string.Empty;
        private DateTime[]? Calendar_Values { get; set; }
        internal bool Calendar_Disabled { get; set; }
    }
}
