using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Internal;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Calendars.Internal
{
    /// <summary>
    /// Represents the header section for the calendar table, displaying the days of the week, based on the <see cref="CalendarView"/> and provided culture information.
    /// </summary>
    /// <remarks>
    /// The <see cref="CalendarTableHeader{TCalendarHeader}"/> component generates and formats the day names in the calendar's header using the specified first day of the week and header format. Inherits from <see cref="CalendarBase{TCalendarHeader}"/>.
    /// </remarks>
    /// <typeparam name="TCalendarHeader">Specifies the type parameter for the calendar header, allowing generic customization and reuse.</typeparam>
    /// <example>
    /// Example usage:
    /// <code><![CDATA[
    /// <CalendarTableHeader CalendarRenderView="CalendarView.Month" />
    /// ]]></code>
    /// </example>
    public partial class CalendarTableHeader<TCalendarHeader> : CalendarBase<TCalendarHeader>
    {
        internal const string WEEK_NUMBER = "e-week-number";
        internal const string WEEK_HEADER = "e-week-header";
        internal const int DAYCOUNT = 7;
        private int DaysCount { get; set; }
        private string[] ShortNames { get; set; } = default!;

        [CascadingParameter]
        internal CalendarBase<TCalendarHeader>? Parent { get; set; }

        /// <summary>
        /// Gets or sets the calendar view used for rendering the header.
        /// </summary>
        /// <value>
        /// A <see cref="CalendarView"/> value that determines the calendar view (such as Month, Year, or Decade) associated with the current header instance.
        /// </value>
        /// <remarks>
        /// Use this property to configure the <see cref="CalendarTableHeader{TCalendarHeader}"/> layout and visual representation based on the selected calendar view.
        /// </remarks>
        /// <example>
        /// This example configures the calendar header for a month view:
        /// <code><![CDATA[
        /// <CalendarTableHeader CalendarRenderView="CalendarView.Month" />
        /// ]]></code>
        /// </example>
        /// <exclude/>
        [Parameter]
        public CalendarView CalendarRenderView { get; set; }

        /// <summary>
        /// Called when the component is initialized. Used to set up the content header of the calendar.
        /// </summary>
        /// <remarks>
        /// This method invokes <see cref="CreateContentHeader"/> to initialize the day header based on component configuration.
        /// </remarks>
        /// <exclude />
        protected override void OnInitialized()
        {
            CreateContentHeader();
        }

        /// <summary>
        /// Called when the component's parameters are set or updated. Responsible for regenerating the content header as needed.
        /// </summary>
        /// <remarks>
        /// This method ensures that the calendar header reflects any parameter and configuration changes dynamically by calling <see cref="CreateContentHeader"/>.
        /// </remarks>
        /// <exclude />
        protected override void OnParametersSet()
        {
            CreateContentHeader();
        }

        /// <summary>
        /// Generates the content for the calendar header by assigning the day names according to current settings.
        /// </summary>
        /// <remarks>
        /// This method updates the <c>DaysCount</c> and invokes internal logic to construct the day headers.
        /// </remarks>
        /// <exclude />
        protected void CreateContentHeader()
        {
            DaysCount = DAYCOUNT;
            UpdateHeaderName();
        }

        private void UpdateHeaderName()
        {
            CultureInfo currentCulture = Intl.GetCulture();
            bool isFirstDayOfWeek = Parent is not null && Parent.FirstDayOfWeek != 0;
            switch (Parent?.DayHeaderFormat)
            {
                case DayHeaderFormats.Short:
                    if (isFirstDayOfWeek)
                    {
                        string[] totalDays = currentCulture.DateTimeFormat.ShortestDayNames;
                        GetFirstDayOfHeader(totalDays);
                    }
                    else
                    {
                        ShortNames = currentCulture.DateTimeFormat.ShortestDayNames;
                    }

                    break;
                case DayHeaderFormats.Abbreviated:
                    if (isFirstDayOfWeek)
                    {
                        string[] totalDays = currentCulture.DateTimeFormat.AbbreviatedDayNames;
                        GetFirstDayOfHeader(totalDays);
                    }
                    else
                    {
                        ShortNames = currentCulture.DateTimeFormat.AbbreviatedDayNames;
                    }

                    break;
                case DayHeaderFormats.Narrow:
                    if (isFirstDayOfWeek)
                    {
                        string[] totalDays = [.. Intl.GetNarrowDayNames()];
                        GetFirstDayOfHeader(totalDays);
                    }
                    else
                    {
                        ShortNames = [.. Intl.GetNarrowDayNames()];
                    }

                    break;
                case DayHeaderFormats.Wide:
                    if (isFirstDayOfWeek)
                    {
                        string[] totalDays = currentCulture.DateTimeFormat.DayNames;
                        GetFirstDayOfHeader(totalDays);
                    }
                    else
                    {
                        ShortNames = currentCulture.DateTimeFormat.DayNames;
                    }

                    break;
                default:
                    break;
            }
        }

        private void GetFirstDayOfHeader(string[] totalDays)
        {
            if (Parent is null)
            {
                return;
            }
            int firstDay = Parent.FirstDayOfWeek;
            ArraySegment<string> val = new(totalDays);
            string[] sliceItems = [.. val.Slice(firstDay)];
            string[] remainItem = [.. val.Slice(0, firstDay)];
            ShortNames = [.. sliceItems, .. remainItem];
        }
    }
}
