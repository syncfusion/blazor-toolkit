namespace Syncfusion.Blazor.Toolkit.Calendars
{
    /// <summary>
    /// Provides configuration for placeholder text to be displayed in a masked <see cref="SfDateTimePicker{TValue}"/> control, based on the <see cref="SfDatePicker{TValue}.Format"/>, until the user enters a value.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <c>DateTimePickerMaskPlaceholder</c> class enables customization of day, month, year, hour, minute, second, and day-of-week placeholder text in the <see cref="SfDateTimePicker{TValue}"/> input mask.
    /// Its properties are effective only when <see cref="SfDatePicker{TValue}.EnableMask"/> is set to <c>true</c>.
    /// </para>
    /// <para>
    /// This class inherits from <see cref="DatePickerMaskPlaceholder"/>, which in turn inherits from <see cref="MaskPlaceholder"/>, providing comprehensive placeholder customization for all date and time segments.
    /// Unlike <see cref="DatePickerMaskPlaceholder"/>, this class is specifically designed for datetime inputs that include time components (hour, minute, second) in addition to date components.
    /// </para>
    /// <para>
    /// The placeholder text helps users understand the expected input format for each segment of the datetime mask, improving user experience and reducing input errors.
    /// Common use cases include forms requiring precise datetime input, scheduling applications, and data entry systems with strict datetime formatting requirements.
    /// </para>
    /// </remarks>
    /// <example>
    /// <para>This example demonstrates basic usage of <c>DateTimePickerMaskPlaceholder</c> with custom placeholders for all datetime segments:</para>
    /// <code><![CDATA[
    /// <SfDateTimePicker TValue="DateTime" EnableMask="true" Format="MM/dd/yyyy hh:mm:ss tt">
    ///     <DateTimePickerMaskPlaceholder 
    ///         Day="dd" 
    ///         Month="MM" 
    ///         Year="yyyy" 
    ///         Hour="hh" 
    ///         Minute="mm" 
    ///         Second="ss" />
    /// </SfDateTimePicker>
    /// ]]></code>
    /// <para>This example shows usage with day-of-week placeholder in a comprehensive datetime format:</para>
    /// <code><![CDATA[
    /// <SfDateTimePicker TValue="DateTime?" EnableMask="true" Format="dddd, MMMM dd, yyyy h:mm tt">
    ///     <DateTimePickerMaskPlaceholder 
    ///         DayOfWeek="dddd" 
    ///         Month="MMMM" 
    ///         Day="dd" 
    ///         Year="yyyy" 
    ///         Hour="h" 
    ///         Minute="mm" />
    /// </SfDateTimePicker>
    /// ]]></code>
    /// </example>
    public class DateTimePickerMaskPlaceholder : DatePickerMaskPlaceholder
    {

    }
}
