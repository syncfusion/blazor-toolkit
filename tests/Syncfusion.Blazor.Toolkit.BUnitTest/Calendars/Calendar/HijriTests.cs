using Xunit;
using Bunit;
using Syncfusion.Blazor.Toolkit.Calendars;
using Syncfusion.Blazor.Toolkit.Internal;
using System;
using System.Threading.Tasks;
using System.Globalization;
using Syncfusion.Blazor.Toolkit;
using Microsoft.AspNetCore.Components.Web;

namespace Syncfusion.Blazor.Toolkit.Tests.Calendars.Calendar
{
    public class HijriTests : BunitTestContext
    {
        [Fact(Timeout = 10000, DisplayName = "Hijri: CalendarMode=Islamic displays Hijri month names")]
        public void Hijri_IslamicMode_DisplaysIslamicMonths()
        {
            // Ramadan 1, 1447 AH is approx Feb 18, 2026 AD
            var date = new DateTime(2026, 2, 18);
            var calendar = RenderComponent<SfCalendar<DateTime>>(parameters =>
                parameters.Add(p => p.Value, date)
                          .Add(p => p.CalendarMode, CalendarType.Islamic)
            );

            // The header title should contain the Islamic month name
            // ar-SA culture is often used for Islamic month names in the toolkit as seen in source
            var monthName = new CultureInfo("ar-SA").DateTimeFormat.AbbreviatedMonthNames[8]; // Ramadan is 9th month (index 8)
            var header = calendar.Find(".e-title");
            Assert.Contains("Ramadan", header.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "Hijri: ConvertToHijri and ConvertToGregorian round-trip")]
        public void Hijri_Conversion_RoundTrip()
        {
            var calendar = RenderComponent<SfCalendar<DateTime>>();
            var originalDate = new DateTime(2026, 3, 10);
            
            // Use public API ConvertToHijri and ConvertToGregorian
            var hijriString = calendar.Instance.ConvertToHijri(originalDate, "dd/MM/yyyy");
            var convertedBack = calendar.Instance.ConvertToGregorian(hijriString, "dd/MM/yyyy");
            
            Assert.Equal(originalDate.Date, convertedBack.Date);
        }

        [Fact(Timeout = 10000, DisplayName = "Hijri: Navigate in Islamic mode")]
        public async Task Hijri_Navigation_Islamic()
        {
            var date = new DateTime(2026, 2, 18); // Ramadan 1, 1447
            var calendar = RenderComponent<SfCalendar<DateTime>>(parameters =>
                parameters.Add(p => p.Value, date)
                          .Add(p => p.CalendarMode, CalendarType.Islamic)
            );

            var nextButton = calendar.Find(".e-next");
            await nextButton.ClickAsync(new MouseEventArgs());
            
            // Should move to Shawwal 1447
            var nextMonthName = new CultureInfo("ar-SA").DateTimeFormat.AbbreviatedMonthNames[9]; // Shawwal (index 9)
            var header = calendar.Find(".e-title");
            Assert.Contains("Shawwal1447", header.TextContent);
        }
    }
}
