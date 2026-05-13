using Xunit;
using Bunit;
using Syncfusion.Blazor.Toolkit.Calendars;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Tests.Calendars.Calendar
{
    public class ValuesCallbackTests : BunitTestContext
    {
        [Fact(Timeout = 10000, DisplayName = "ValuesChanged: invoked on programmatic update")]
        public async Task ValuesChanged_Invoked_OnProgrammaticUpdate()
        {
            DateTime[]? changedValues = null;
            var calendar = RenderComponent<SfCalendar<DateTime>>(parameters =>
                parameters.Add(p => p.IsMultiSelection, true)
                          .Add(p => p.ValuesChanged, EventCallback.Factory.Create<DateTime[]>(this, (v) => changedValues = v))
            );

            var newDates = new DateTime[] { new DateTime(2026, 3, 10), new DateTime(2026, 3, 11) };
            await calendar.Instance.AddDatesAsync(newDates);
            
            Assert.NotNull(changedValues);
            Assert.Equal(2, changedValues.Length);
            Assert.Contains(newDates[0], changedValues);
            Assert.Contains(newDates[1], changedValues);
        }

        [Fact(Timeout = 10000, DisplayName = "DayHeaderFormat: Wide format adds e-calendar-day-header-lg")]
        public void DayHeaderFormat_Wide_AddsClass()
        {
            var calendar = RenderComponent<SfCalendar<DateTime>>(parameters =>
                parameters.Add(p => p.DayHeaderFormat, DayHeaderFormats.Wide)
            );

            var table = calendar.Find("table");
            var root = table.ParentElement?.ParentElement;
            Assert.Contains("e-calendar-day-header-lg", root?.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "WeekNumber: true adds e-week-number class")]
        public void WeekNumber_True_AddsClass()
        {
            var calendar = RenderComponent<SfCalendar<DateTime>>(parameters =>
                parameters.Add(p => p.WeekNumber, true)
            );

            var table = calendar.Find("table");
            var root = table.ParentElement?.ParentElement;
            Assert.Contains("e-week-number", root?.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "FirstDayOfWeek: culture default respected when unset")]
        public void FirstDayOfWeek_CultureDefault_Respected()
        {
            // US culture defaults to Sunday (0)
            using (new CultureHelper(new CultureInfo("en-US")))
            {
                var calendar = RenderComponent<SfCalendar<DateTime>>();
                // In en-US, Sunday is FirstDayOfWeek
                // We check the first header cell in the table
                var firstHeader = calendar.Find("th");
                // Sunday abbreviation is typically 'Sun' or 'S'
                Assert.True(firstHeader.TextContent.Contains("S", StringComparison.OrdinalIgnoreCase));
            }

            // FR culture defaults to Monday (1)
            using (new CultureHelper(new CultureInfo("fr-FR")))
            {
                var calendar = RenderComponent<SfCalendar<DateTime>>();
                var firstHeader = calendar.Find("th");
                // Monday in French is Lundi, abbreviation starts with 'L'
                Assert.True(firstHeader.TextContent.Contains("L", StringComparison.OrdinalIgnoreCase));
            }
        }
    }

    /// <summary>
    /// Helper to temporarily switch culture
    /// </summary>
    public class CultureHelper : IDisposable
    {
        private readonly CultureInfo _originalCulture;
        private readonly CultureInfo _originalUICulture;

        public CultureHelper(CultureInfo culture)
        {
            _originalCulture = CultureInfo.CurrentCulture;
            _originalUICulture = CultureInfo.CurrentUICulture;
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
        }

        public void Dispose()
        {
            CultureInfo.CurrentCulture = _originalCulture;
            CultureInfo.CurrentUICulture = _originalUICulture;
        }
    }
}
