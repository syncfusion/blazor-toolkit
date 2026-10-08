using System.Reflection;
using System.Text.Json;
using Syncfusion.Blazor.Toolkit.Calendars;
using Syncfusion.Blazor.Toolkit.Calendars.Internal;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Calendars.Calendar
{
    public class NullableCalendarConversionTests
    {
        [Fact]
        public void DateConversions_PreserveDateTimeDateOnlyAndOffsetSemantics()
        {
            DateTime date = new(2026, 10, 3, 14, 15, 16, DateTimeKind.Unspecified);
            DateOnly day = DateOnly.FromDateTime(date);
            DateTimeOffset offset = new(date, TimeSpan.FromHours(5.5));

            AssertDate(date, date);
            AssertDate<DateTime?>(date, date);
            AssertDate(day, date.Date);
            AssertDate<DateOnly?>(day, date.Date);
            AssertDate(offset, date);
            AssertDate<DateTimeOffset?>(offset, date);
        }

        [Fact]
        public void NullNullableDates_StillThrowNullReferenceException()
        {
            Assert.Throws<NullReferenceException>(() => CalendarBase<DateTime?>.ConvertDate(null));
            Assert.Throws<NullReferenceException>(() => CalendarBase<DateOnly?>.ConvertDate(null));
            Assert.Throws<NullReferenceException>(() => CalendarBase<DateTimeOffset?>.ConvertDate(null));
            Assert.Throws<NullReferenceException>(() => new ConversionProbe<DateTime?>().ConvertRequired(null));
            Assert.Throws<NullReferenceException>(() => new ConversionProbe<DateOnly?>().ConvertRequired(null));
            Assert.Throws<NullReferenceException>(() => new ConversionProbe<DateTimeOffset?>().ConvertRequired(null));
        }

        [Fact]
        public void RequiredDateConversion_NullAndWrongBoxedTypesKeepTheirExceptions()
        {
            Assert.Throws<NullReferenceException>(() => new ConversionProbe<string?>().ConvertRequired(null));
            Assert.Throws<InvalidCastException>(() => new ConversionProbe<string>().ConvertRequired("not a date"));
            Assert.Throws<InvalidCastException>(() => new ConversionProbe<int>().ConvertRequired(123));
        }

        [Fact]
        public void GenericValue_PreservesNullableDateTypes()
        {
            DateTime date = new(2026, 10, 3, 14, 15, 16, DateTimeKind.Unspecified);
            Assert.Equal(date, CalendarBase<DateTime?>.GenericValue(date));
            Assert.Equal(DateOnly.FromDateTime(date), CalendarBase<DateOnly?>.GenericValue(date));
            Assert.Equal(new DateTimeOffset(date), CalendarBase<DateTimeOffset?>.GenericValue(date));
        }

        [Fact]
        public void MultiSelection_KeepsDateTimeValuesAndRejectsOtherBoxedDateTypes()
        {
            DateTime date = new(2026, 10, 3);
            var renderer = new CalendarBaseRender<DateTime?> { Parent = new CalendarValueProbe<DateTime?>(date) };
            var values = new List<DateTime>();
            InvokeMultiSelectionAdd(renderer, date.AddDays(1), values);
            Assert.Equal(new[] { date, date.AddDays(1) }, values);

            var dateOnlyRenderer = new CalendarBaseRender<DateOnly?> { Parent = new CalendarValueProbe<DateOnly?>(DateOnly.FromDateTime(date)) };
            var offsetRenderer = new CalendarBaseRender<DateTimeOffset?> { Parent = new CalendarValueProbe<DateTimeOffset?>(new DateTimeOffset(date)) };
            Assert.IsType<InvalidCastException>(Assert.Throws<TargetInvocationException>(() =>
                InvokeMultiSelectionAdd(dateOnlyRenderer, date, new List<DateTime>())).InnerException);
            Assert.IsType<InvalidCastException>(Assert.Throws<TargetInvocationException>(() =>
                InvokeMultiSelectionAdd(offsetRenderer, date, new List<DateTime>())).InnerException);
        }

        [Fact]
        public async Task Navigation_NullNullableDateStillFailsBeforeUpdatingState()
        {
            var renderer = new CalendarBaseRender<DateTime?> { Parent = new SfCalendar<DateTime?>() };
            await Assert.ThrowsAsync<NullReferenceException>(() => renderer.NavigateToAsync(CalendarView.Month, null));
        }

        [Fact]
        public void ClientWidths_SerializeNullWithoutSubstitutingEmptyString()
        {
            using JsonDocument datePicker = JsonDocument.Parse(JsonSerializer.Serialize(new DatePickerClientProps<DateTime?> { Width = null }));
            using JsonDocument timePicker = JsonDocument.Parse(JsonSerializer.Serialize(new TimePickerClientProps<DateTime?> { Width = null }));
            Assert.Equal(JsonValueKind.Null, datePicker.RootElement.GetProperty("Width").ValueKind);
            Assert.Equal(JsonValueKind.Null, timePicker.RootElement.GetProperty("Width").ValueKind);
        }

        private static void AssertDate<T>(T value, DateTime expected)
        {
            Assert.Equal(expected, CalendarBase<T>.ConvertDate(value));
            Assert.Equal(expected, new ConversionProbe<T>().ConvertRequired(value));
        }

        private static void InvokeMultiSelectionAdd<T>(CalendarBaseRender<T> renderer, DateTime date, List<DateTime> values)
        {
            MethodInfo method = typeof(CalendarBaseRender<T>).GetMethod("HandleMultiSelectionAdd", BindingFlags.Instance | BindingFlags.NonPublic)
                ?? throw new InvalidOperationException("Calendar selection method was not found.");
            method.Invoke(renderer, new object[] { date, values });
        }

        // Own the initial value without invoking the rendering lifecycle in conversion-only tests.
        private sealed class CalendarValueProbe<T> : SfCalendar<T>
        {
            public CalendarValueProbe(T value)
            {
                Value = value;
            }
        }

        private sealed class ConversionProbe<T> : CalendarBase<T>
        {
            public DateTime ConvertRequired(T value) => ConvertDateValue(value);
        }
    }
}
