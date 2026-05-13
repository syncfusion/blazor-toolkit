using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Calendars
{
    /// <summary>
    /// The DateTimePicker is a graphical user interface component that allows the user to select or enter a date and time value.
    /// </summary>
    /// <remarks>
    /// The <see cref="SfDateTimePicker{TValue}"/> component extends the <see cref="SfDatePicker{TValue}"/> to provide both date and time selection capabilities.
    /// It includes a time popup list with configurable time intervals, time format options, and time range restrictions.
    /// The component supports various data types through the generic <c>TValue</c> parameter and provides comprehensive validation for both date and time values.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// <SfDateTimePicker TValue="DateTime?" @bind-Value="@SelectedDateTime" 
    ///                   Min="@MinDateTime" Max="@MaxDateTime" 
    ///                   MinTime="@MinTime" MaxTime="@MaxTime"
    ///                   Step="30" TimeFormat="HH:mm">
    /// </SfDateTimePicker>
    /// 
    /// @code {
    ///     DateTime? SelectedDateTime = DateTime.Now;
    ///     DateTime MinDateTime = new DateTime(2024, 1, 1, 9, 0, 0);
    ///     DateTime MaxDateTime = new DateTime(2024, 12, 31, 18, 0, 0);
    ///     DateTime MinTime = new DateTime(2024, 1, 1, 9, 0, 0);
    ///     DateTime MaxTime = new DateTime(2024, 1, 1, 18, 0, 0);
    /// }
    /// ]]></code>
    /// </example>
    public partial class SfDateTimePicker<TValue> : SfDatePicker<TValue>
    {
        /// <summary>
        /// Gets or sets the scroll bar position in the time popup list when no value is selected or the given value is not present in the DateTimePicker popup list.
        /// </summary>
        /// <value>
        /// A <see cref="Nullable{DateTime}"/> value that represents the scroll position in the time popup list. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// The <see cref="ScrollTo"/> property determines the initial scroll position of the time popup list.
        /// When set, the time popup will scroll to the specified time value, making it visible in the dropdown.
        /// If the specified time is not available in the popup list or if no value is provided, the component will use this property to set the scroll position.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfDateTimePicker TValue="DateTime?" ScrollTo="@ScrollPosition">
        /// </SfDateTimePicker>
        /// 
        /// @code {
        ///     DateTime? ScrollPosition = new DateTime(2024, 1, 1, 14, 30, 0);
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public DateTime? ScrollTo { get; set; }

        /// <summary>
        /// Gets or sets the maximum date and time value that can be selected in the <see cref="SfDateTimePicker{TValue}"/> component.
        /// </summary>
        /// <value>
        /// A <see cref="DateTime"/> value that represents the maximum date and time that can be selected. The default value is <c>December 31, 2099 at 23:59:59</c>.
        /// </value>
        /// <remarks>
        /// The <see cref="Max"/> property defines the latest date and time that can be selected in the DateTimePicker component.
        /// When combined with the <see cref="MaxTime"/> property, the following behaviors apply:
        /// <list type="bullet">
        /// <item><description>If <see cref="MaxTime"/> is greater than the current <see cref="Max"/> property's time, the component will prioritize the <see cref="Max"/> property.</description></item>
        /// <item><description>If <see cref="MaxTime"/> is less than the current <see cref="Max"/> property's time, the component will prioritize the <see cref="MaxTime"/> property.</description></item>
        /// </list>
        /// Users will not be able to select any date and time beyond this maximum value.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfDateTimePicker TValue="DateTime?" Max="@MaxDateTime">
        /// </SfDateTimePicker>
        /// 
        /// @code {
        ///     DateTime MaxDateTime = new DateTime(2024, 12, 31, 23, 59, 59);
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public override DateTime Max { get; set; } = new DateTime(2099, 12, 31, 23, 59, 59);

        /// <summary>
        /// Gets or sets the minimum date and time value that can be selected in the <see cref="SfDateTimePicker{TValue}"/> component.
        /// </summary>
        /// <value>
        /// A <see cref="DateTime"/> value that represents the minimum date and time that can be selected. The default value is <c>January 1, 1900 at 00:00:00</c>.
        /// </value>
        /// <remarks>
        /// The <see cref="Min"/> property defines the earliest date and time that can be selected in the DateTimePicker component.
        /// When combined with the <see cref="MinTime"/> property, the following behaviors apply:
        /// <list type="bullet">
        /// <item><description>If <see cref="MinTime"/> is less than the current <see cref="Min"/> property's time, the component will prioritize the <see cref="Min"/> property.</description></item>
        /// <item><description>If <see cref="MinTime"/> is greater than the current <see cref="Min"/> property's time, the component will prioritize the <see cref="MinTime"/> property.</description></item>
        /// </list>
        /// Users will not be able to select any date and time before this minimum value.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfDateTimePicker TValue="DateTime?" Min="@MinDateTime">
        /// </SfDateTimePicker>
        /// 
        /// @code {
        ///     DateTime MinDateTime = new DateTime(2024, 1, 1, 0, 0, 0);
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public override DateTime Min { get; set; } = new DateTime(1900, 01, 01, 00, 00, 00);

        /// <summary>
        /// Gets or sets the maximum time that can be selected in the time popup of the <see cref="SfDateTimePicker{TValue}"/> component.
        /// </summary>
        /// <value>
        /// A <see cref="DateTime"/> value representing the maximum selectable time. The default value is <c>December 31, 2099 at 23:59:59</c>.
        /// </value>
        /// <remarks>
        /// The <see cref="MaxTime"/> property restricts the time selection for all dates except the specific date defined by the <see cref="Max"/> property.
        /// The following behaviors apply when both <see cref="MaxTime"/> and <see cref="Max"/> are configured:
        /// <list type="bullet">
        /// <item><description>If <see cref="MaxTime"/> is greater than the current <see cref="Max"/> property's time component, the component will prioritize the <see cref="Max"/> property.</description></item>
        /// <item><description>If <see cref="MaxTime"/> is less than the current <see cref="Max"/> property's time component, the component will prioritize the <see cref="MaxTime"/> property.</description></item>
        /// </list>
        /// This property is particularly useful for setting business hours or operational time limits across all selectable dates.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfDateTimePicker TValue="DateTime?" MinTime="@MinTime" MaxTime="@MaxTime">
        /// </SfDateTimePicker>
        /// 
        /// @code {
        ///     DateTime MinTime = new DateTime(2024, 8, 6, 9, 0, 0);
        ///     DateTime MaxTime = new DateTime(2024, 8, 6, 18, 0, 0);
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public DateTime MaxTime { get; set; } = new DateTime(2099, 12, 31, 23, 59, 59);

        /// <summary>
        /// Gets or sets the minimum time that can be selected in the time popup of the <see cref="SfDateTimePicker{TValue}"/> component.
        /// </summary>
        /// <value>
        /// A <see cref="DateTime"/> value representing the minimum selectable time. The default value is <c>January 1, 1900 at 00:00:00</c>.
        /// </value>
        /// <remarks>
        /// The <see cref="MinTime"/> property restricts the time selection for all dates except the specific date defined by the <see cref="Min"/> property.
        /// The following behaviors apply when both <see cref="MinTime"/> and <see cref="Min"/> are configured:
        /// <list type="bullet">
        /// <item><description>If <see cref="MinTime"/> is less than the current <see cref="Min"/> property's time component, the component will prioritize the <see cref="Min"/> property.</description></item>
        /// <item><description>If <see cref="MinTime"/> is greater than the current <see cref="Min"/> property's time component, the component will prioritize the <see cref="MinTime"/> property.</description></item>
        /// </list>
        /// This property is particularly useful for setting business hours or operational time limits across all selectable dates.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfDateTimePicker TValue="DateTime?" MinTime="@MinTime" MaxTime="@MaxTime">
        /// </SfDateTimePicker>
        /// 
        /// @code {
        ///     DateTime MinTime = new DateTime(2024, 8, 6, 9, 0, 0);
        ///     DateTime MaxTime = new DateTime(2024, 8, 6, 18, 0, 0);
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public DateTime MinTime { get; set; } = new DateTime(1900, 01, 01, 00, 00, 00);

        /// <summary>
        /// Gets or sets the time interval between adjacent time values in the time popup list of the <see cref="SfDateTimePicker{TValue}"/> component.
        /// </summary>
        /// <value>
        /// An <c>int</c> value that specifies the time interval in minutes between two adjacent time values in the time popup list. The default value is <c>30</c> minutes.
        /// </value>
        /// <remarks>
        /// The <see cref="Step"/> property controls the granularity of time selection in the dropdown list.
        /// For example, if set to 30 minutes, the time options will appear as 00:00, 00:30, 01:00, 01:30, and so on.
        /// Smaller values provide finer time selection granularity, while larger values reduce the number of available options in the dropdown.
        /// The step value must be a positive integer representing minutes.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfDateTimePicker TValue="DateTime?" Step="15">
        /// </SfDateTimePicker>
        /// 
        /// <!-- This creates time intervals of 15 minutes: 00:00, 00:15, 00:30, 00:45, etc. -->
        /// ]]></code>
        /// </example>
        [Parameter]
        public int Step { get; set; } = 30;

        /// <summary>
        /// Gets or sets the format of the time value to be displayed in the time popup list of the <see cref="SfDateTimePicker{TValue}"/> component.
        /// </summary>
        /// <value>
        /// A <c>string</c> value that specifies the time format pattern for the time popup list. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// The <see cref="TimeFormat"/> property allows you to customize how time values appear in the dropdown list.
        /// You can use standard .NET DateTime format strings such as "HH:mm", "hh:mm tt", "H:mm", etc.
        /// If not specified, the component will use the default time format based on the current culture.
        /// This property only affects the display format in the time popup list and does not change the actual value format.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfDateTimePicker TValue="DateTime?" TimeFormat="HH:mm">
        /// </SfDateTimePicker>
        /// 
        /// <!-- Displays time in 24-hour format: 09:30, 14:45, etc. -->
        /// 
        /// <SfDateTimePicker TValue="DateTime?" TimeFormat="hh:mm tt">
        /// </SfDateTimePicker>
        /// 
        /// <!-- Displays time in 12-hour format with AM/PM: 09:30 AM, 02:45 PM, etc. -->
        /// ]]></code>
        /// </example>
        [Parameter]
        public string TimeFormat { get; set; } = default!;

        /// <summary>
        /// Parent component of DateTimePicker.
        /// </summary>
        /// <exclude />
        [CascadingParameter(Name = "InPlaceEditor")]
        protected dynamic? DateTimePickerParent { get; set; }
    }
}
