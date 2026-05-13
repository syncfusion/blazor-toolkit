using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Calendars
{
    /// <summary>
    /// Provides customizable text placeholders for different input segments (such as day, month, year, hour, minute, second, and day of week) in masked date and time components. These placeholders offer hints or instructions until the user provides input, improving usability and accessibility.
    /// </summary>
    /// <remarks>
    /// The <c>MaskPlaceholder</c> class properties are effective when the <see cref="SfDatePicker{TValue}.EnableMask"/> property is enabled. This allows you to guide users about the expected input format for each segment in masked calendar editors.
    /// </remarks>
    /// <example>
    /// This example shows how to apply mask placeholders to the <see cref="SfDatePicker{DateTime}"/>:
    /// <code><![CDATA[
    /// <SfDatePicker EnableMask="true">
    ///     <MaskPlaceholder Day="dd" Month="mm" Year="yyyy" Hour="hh" Minute="mm" Second="ss" />
    /// </SfDatePicker>
    /// ]]></code>
    /// </example>
    public partial class MaskPlaceholder : SfBaseComponent
    {
        /// <summary>
        /// Gets or sets the text displayed as a hint or placeholder for the day segment of the date input until the user enters a value.
        /// </summary>
        /// <value>
        /// A <c>string</c> value representing the placeholder text for the day segment. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// Typically used with input fields utilizing a date mask constrained by <see cref="SfDatePicker{TValue}.Format"/>; for instance, <c>mm/dd/yyyy</c> where <c>dd</c> is for the day. Set <see cref="Day"/> to a value like <c>dd</c> to indicate the required format for users.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfDatePicker EnableMask="true">
        ///     <MaskPlaceholder Day="dd" />
        /// </SfDatePicker>
        /// ]]></code>
        /// </example>
        [Parameter]
        public string Day { get; set; } = default!;

        /// <summary>
        /// Gets or sets the text displayed as a hint or placeholder for the month segment of the date input until the user enters a value.
        /// </summary>
        /// <value>
        /// A <c>string</c> that represents the placeholder text for the month segment. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// Used with input fields with masked formatting such as <c>mm/dd/yyyy</c>, where <c>mm</c> is the month segment. Set the <see cref="Month"/> property to display the desired placeholder for users.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfDatePicker EnableMask="true">
        ///     <MaskPlaceholder Month="mm" />
        /// </SfDatePicker>
        /// ]]></code>
        /// </example>
        [Parameter]
        public string Month { get; set; } = default!;

        /// <summary>
        /// Gets or sets the text displayed as a hint or placeholder for the year segment of the date input until the user enters a value.
        /// </summary>
        /// <value>
        /// A <c>string</c> that represents the placeholder text for the year segment. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// Used when the input is masked for an expected year format such as <c>mm/dd/yyyy</c> where <c>yyyy</c> is the year. Assign <see cref="Year"/> as <c>yyyy</c> for clear formatting guidance.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfDatePicker EnableMask="true">
        ///     <MaskPlaceholder Year="yyyy" />
        /// </SfDatePicker>
        /// ]]></code>
        /// </example>
        [Parameter]
        public string Year { get; set; } = default!;

        /// <summary>
        /// Gets or sets the text displayed as a hint or placeholder for the hour segment of a datetime input until the user enters a value.
        /// </summary>
        /// <value>
        /// A <c>string</c> representing the placeholder for the hour segment. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// Used for input masks like <c>mm/dd/yyyy hh:mm</c>, where <c>hh</c> is the hour. Set <see cref="Hour"/> to <c>hh</c> for user guidance in the time segment.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfDatePicker EnableMask="true">
        ///     <MaskPlaceholder Hour="hh" />
        /// </SfDatePicker>
        /// ]]></code>
        /// </example>
        [Parameter]
        public string Hour { get; set; } = default!;

        /// <summary>
        /// Gets or sets the text displayed as a hint or placeholder for the minute segment of a datetime input until the user enters a value.
        /// </summary>
        /// <value>
        /// A <c>string</c> indicating the placeholder text for the minute segment. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// Used for input masks such as <c>mm/dd/yyyy hh:mm</c>, where <c>mm</c> refers to minutes. Setting <see cref="Minute"/> to <c>mm</c> clarifies the input for the minute portion of the field.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfDatePicker EnableMask="true">
        ///     <MaskPlaceholder Minute="mm" />
        /// </SfDatePicker>
        /// ]]></code>
        /// </example>
        [Parameter]
        public string Minute { get; set; } = default!;

        /// <summary>
        /// Gets or sets the text displayed as a hint or placeholder for the second segment of a datetime input until the user enters a value.
        /// </summary>
        /// <value>
        /// A <c>string</c> specifying the placeholder text for the second segment. The default is <c>null</c>.
        /// </value>
        /// <remarks>
        /// Used in scenarios where the mask includes seconds, as in <c>mm/dd/yyyy hh:mm:ss</c> with <c>ss</c> for the seconds segment. Set <see cref="Second"/> to <c>ss</c> for user clarity.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfDatePicker EnableMask="true">
        ///     <MaskPlaceholder Second="ss" />
        /// </SfDatePicker>
        /// ]]></code>
        /// </example>
        [Parameter]
        public string Second { get; set; } = default!;

        /// <summary>
        /// Gets or sets the text displayed as a hint or placeholder for the day of week segment of a datetime input until the user enters a value.
        /// </summary>
        /// <value>
        /// A <c>string</c> value for the day of week segment. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// Useful for formats like <c>dddd,dd/mm/yyyy</c>, where <c>dddd</c> is the day of the week. Assign <see cref="DayOfWeek"/> to <c>dddd</c> for weekday input clarity.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfDatePicker EnableMask="true">
        ///     <MaskPlaceholder DayOfWeek="dddd" />
        /// </SfDatePicker>
        /// ]]></code>
        /// </example>
        [Parameter]
        public string DayOfWeek { get; set; } = default!;

    }
}
