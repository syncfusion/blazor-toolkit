using AngleSharp.Css.Dom;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Toolkit.Buttons;
using Syncfusion.Blazor.Toolkit.Calendars;
using Syncfusion.Blazor.Toolkit.Inputs;
using System.Globalization;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Calendars.DateTimePicker
{
    public class DateTimePicker : BunitTestContext
    {
        const string FORMATFULLDATE = "dddd, MMMM d, yyyy";
        const string FORMATDATE = " d ";
        const string TITLE_SEPARATOR = " - ";
        const string FORMAT_YEAR = "yyyy";
        [Fact(Timeout = 10000)]
        public async Task DefaultInitialize()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>();
            var inputEle = dateInstance.Find("input");
            Assert.Contains("e-datetimepicker", inputEle.ClassName);
            Assert.Contains("e-control-container", inputEle.ParentElement.ClassName);
            Assert.True(inputEle.ParentElement.ChildElementCount == 3);
            Assert.True(inputEle.ParentElement.HasChildNodes);
            Assert.True(inputEle.ParentElement.NodeName == "SPAN");
            Assert.Equal("0", inputEle.GetAttribute("tabindex"));
            Assert.Null(inputEle.GetAttribute("placeholder"));
            Assert.Equal("false", inputEle.GetAttribute("aria-expanded"));
        }
        [Fact(Timeout = 10000)]
        public async Task DefaultValue()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>();
            Assert.Null(dateInstance.Instance.Value);
            Assert.Null(dateInstance.Instance.Placeholder);
            Assert.Null(dateInstance.Instance.Width);
            Assert.Equal(1000, dateInstance.Instance.ZIndex);
            Assert.Equal(new DateTime(1900, 01, 01, 00, 00, 00), dateInstance.Instance.Min);
            Assert.Equal(new DateTime(2099, 12, 31, 23, 59, 59), dateInstance.Instance.Max);
            Assert.True(dateInstance.Instance.AllowEdit);
        }
        [Fact(Timeout = 10000)]
        public async Task TabIndex()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(param => param.Add(p => p.TabIndex, 1));
            var inputEle = dateInstance.Find("input");
            Assert.Equal("1", inputEle.GetAttribute("tabindex"));
            dateInstance.SetParametersAndRender(("TabIndex", 3));
            inputEle = dateInstance.Find("input");
            Assert.Equal("3", inputEle.GetAttribute("tabindex"));
        }
        [Fact(Timeout = 10000)]
        public async Task CssClass()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(parameters => parameters.Add(p => p.CssClass, "sample-css"));
            await dateInstance.Instance.ShowPopupAsync();
            var containerEle = dateInstance.Find("input").ParentElement;
            var popupEle = dateInstance.Find(".e-popup");
            Assert.Contains("sample-css", containerEle.ClassName);
            Assert.Contains("sample-css", popupEle.ClassName);
            await dateInstance.Instance.HidePopupAsync();
            dateInstance.SetParametersAndRender(("CssClass", "test highlight"));
            await dateInstance.Instance.ShowPopupAsync();
            containerEle = dateInstance.Find("input").ParentElement;
            popupEle = dateInstance.Find(".e-popup");
            Assert.Contains("test highlight", containerEle.ClassName);
            Assert.Contains("test highlight", popupEle.ClassName);
            Assert.DoesNotContain("sample-css", containerEle.ClassName);
            Assert.DoesNotContain("sample-css", popupEle.ClassName);
            await dateInstance.Instance.HidePopupAsync();
        }
        [Fact(Timeout = 10000)]
        public async Task ReadOnly()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(parameters => parameters.Add(p => p.Readonly, true));
            var containerEle = dateInstance.Find("input").ParentElement;
            var iconEle = containerEle.QuerySelector(".e-timeline-today");
            iconEle.MouseDown();
            await Task.Delay(70);
            var readonlyPopupEle = dateInstance.FindAll(".e-popup");
            Assert.Equal(0, readonlyPopupEle.Count);
            dateInstance.SetParametersAndRender(("Readonly", false));
            containerEle = dateInstance.Find("input").ParentElement;
            iconEle = containerEle.QuerySelector(".e-timeline-today");
            iconEle.MouseDown();
            await Task.Delay(200);
            var popupElement = dateInstance.Find(".e-popup");
            Assert.NotNull(popupElement);
            await dateInstance.Instance.HidePopupAsync();
        }
        [Fact(Timeout = 10000)]
        public async Task WidthChange()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(parameters => parameters.Add(p => p.Width, "300px"));
            var containerEle = dateInstance.Find("input").ParentElement;
            Assert.Contains("width: 300px", containerEle.GetStyle().CssText.Trim());
            dateInstance.SetParametersAndRender(("Width", "600px"));
            containerEle = dateInstance.Find("input").ParentElement;
            Assert.Contains("width: 600px", containerEle.GetStyle().CssText.Trim());
        }
        [Fact(Timeout = 10000)]
        public async Task Placeholder()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(parameters =>
                    parameters.Add(p => p.Placeholder, "Enter the date value"));
            var inputEle = dateInstance.Find("input");
            Assert.Contains("Enter the date value", inputEle.GetAttribute("placeholder"));
            dateInstance.SetParametersAndRender(("Placeholder", "Enter the date"));
            Assert.Contains("Enter the date", inputEle.GetAttribute("placeholder"));
        }
        [Fact(Timeout = 10000)]
        public async Task Disabled()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>();
            dateInstance.SetParametersAndRender(("Disabled", true));
            var inputEle = dateInstance.Find("input");
            var containerEle = inputEle.ParentElement;
            Assert.Contains("e-disabled", containerEle.ClassName);
            Assert.Contains("e-disabled", inputEle.ClassName);
            Assert.Equal("true", inputEle.GetAttribute("aria-disabled"));
            Assert.True(inputEle.HasAttribute("disabled"));
            dateInstance.SetParametersAndRender(("Disabled", false));
            inputEle = dateInstance.Find("input");
            containerEle = inputEle.ParentElement;
            Assert.DoesNotContain("e-disabled", containerEle.ClassName);
            Assert.DoesNotContain("e-disabled", inputEle.ClassName);
            Assert.Equal("false", inputEle.GetAttribute("aria-disabled"));
            Assert.False(inputEle.HasAttribute("disabled"));
        }
        [Fact(Timeout = 10000)]
        public async Task HtmlAttributes()
        {
            var dropdownlist = RenderComponent<SfDateTimePicker<DateTime?>>();
            dropdownlist.SetParametersAndRender(("HtmlAttributes", new Dictionary<string, object>() { { "name", "datepicker" }, { "required", "true" }, { "class", "e-date-calendar" } }));
            var inputEle = dropdownlist.Find("input");
            var containerEle = inputEle.ParentElement;
            Assert.Contains("datepicker", inputEle.GetAttribute("name"));
            Assert.Contains("true", inputEle.GetAttribute("required"));
            Assert.Contains("e-date-calendar", containerEle.ClassName);
        }
        [Fact(Timeout = 10000)]
        public async Task DateCalendarElement()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>();
            var containerEle = dateInstance.Find("input").ParentElement;
            await dateInstance.Instance.ShowPopupAsync();
            var popupEle = dateInstance.Find(".e-popup");
            Assert.Contains("e-popup", popupEle.ClassName);
            var tableElement = popupEle.QuerySelector("table");
            var parentContainer = tableElement.ParentElement.ParentElement;
            Assert.DoesNotContain("e-rtl", parentContainer.ClassName);
            Assert.Contains("e-calendar", parentContainer.ClassName);
            Assert.Contains("e-header", parentContainer.Children[0].ClassName);
            Assert.Contains("e-month", parentContainer.Children[0].ClassName);
            Assert.Contains("e-content", parentContainer.Children[1].ClassName);
            Assert.Contains("e-month", parentContainer.Children[1].ClassName);
            Assert.Contains("e-footer-container", parentContainer.Children[2].ClassName);
            var buttonList = popupEle.QuerySelectorAll("button");
            var prevButton = buttonList[0];
            var nextButton = buttonList[1];
            var todayButton = buttonList[2];
            Assert.Contains("e-prev", prevButton.ClassName);
            Assert.Contains("e-next", nextButton.ClassName);
            Assert.Contains("e-today", todayButton.ClassName);
            Assert.Equal("Today", todayButton.TextContent);
            var tBody = tableElement.QuerySelector("tbody");
            var tRows = tBody.QuerySelectorAll("tr");
            Assert.Equal(6, tRows.Length);
            var tCell = tRows[0].QuerySelectorAll("td");
            Assert.Equal(7, tCell.Length);
        }
        [Fact(Timeout = 10000)]
        public async Task ClearButton()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>();
            var containerEle = dateInstance.Find("input").ParentElement;
            dateInstance.SetParametersAndRender(("ShowClearButton", true));
            containerEle = dateInstance.Find("input").ParentElement;
            var clearEle = containerEle.Children[1];
            Assert.Contains("e-clear-icon", clearEle.ClassName);
            Assert.Contains("e-clear-icon-hide", clearEle.ClassName);
            Assert.True(containerEle.Children[2]?.ClassName.Contains("e-timeline-today"));
            await dateInstance.Instance.ShowPopupAsync();
            var popupEle = dateInstance.Find(".e-popup");
            Assert.Contains("e-popup", popupEle.ClassName);
            var tableElement = popupEle.QuerySelector("table");
            var parentContainer = tableElement.ParentElement.ParentElement;
            Assert.DoesNotContain("e-rtl", parentContainer.ClassName);
            Assert.Contains("e-calendar", parentContainer.ClassName);
            Assert.Contains("e-header", parentContainer.Children[0].ClassName);
            Assert.Contains("e-month", parentContainer.Children[0].ClassName);
            Assert.Contains("e-content", parentContainer.Children[1].ClassName);
            Assert.Contains("e-month", parentContainer.Children[1].ClassName);
            Assert.Contains("e-footer-container", parentContainer.Children[2].ClassName);
            var buttonList = popupEle.QuerySelectorAll("button");
            var prevButton = buttonList[0];
            var nextButton = buttonList[1];
            var todayButton = buttonList[2];
            Assert.Contains("e-prev", prevButton.ClassName);
            Assert.Contains("e-next", nextButton.ClassName);
            Assert.Contains("e-today", todayButton.ClassName);
            Assert.Equal("Today", todayButton.TextContent);
            var tBody = tableElement.QuerySelector("tbody");
            var tRows = tBody.QuerySelectorAll("tr");
            Assert.Equal(6, tRows.Length);
            var tCell = tRows[1].QuerySelectorAll("td");
            tCell[3].Click();
            var inputEle = dateInstance.Find("input");
            await Task.Delay(100);
            Assert.NotNull(inputEle.GetAttribute("value"));
            Assert.NotNull(dateInstance.Instance.Value);
            containerEle = dateInstance.Find("input").ParentElement;
            clearEle = containerEle.Children[1];
            clearEle.MouseDown();
            await Task.Delay(200);
            inputEle = dateInstance.Find("input");
            Assert.Null(inputEle.GetAttribute("value"));
            Assert.Null(dateInstance.Instance.Value);
        }
        private string GetNativeDigits(string formatValue, string[] nativeDigits)
        {
            return formatValue.Replace("0", nativeDigits[0])
                .Replace("1", nativeDigits[1])
                .Replace("2", nativeDigits[2])
                .Replace("3", nativeDigits[3])
                .Replace("4", nativeDigits[4])
                .Replace("5", nativeDigits[5])
                .Replace("6", nativeDigits[6])
                .Replace("7", nativeDigits[7])
                .Replace("8", nativeDigits[8])
                .Replace("9", nativeDigits[9]);
        }
        private string GetDateFormat<T>(T date, string format = null, string culture = null)
        {
            try
            {
                var currentCulture = CultureInfo.CurrentCulture;
                IFormattable dateValue = date as IFormattable;
                var dateCulture = dateValue.ToString(format, currentCulture);
                dateCulture = GetNativeDigits(dateCulture, currentCulture.NumberFormat.NativeDigits);
                return dateCulture;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private string getDecadeTitle(DateTime localDate)
        {
            int localYr = localDate.Year;
            localYr = localYr < 10 ? 10 : localYr;
            int startYr = localYr - localYr % 10;
            int endYr = startYr + (10 - 1);
            string startHdrYr = GetDateFormat(new DateTime(startYr, 1, 1), FORMAT_YEAR);
            string endHdrYr = GetDateFormat(new DateTime(endYr, 1, 1), FORMAT_YEAR);
            return startHdrYr + TITLE_SEPARATOR + endHdrYr;
        }
        [Fact(Timeout = 10000)]
        public async Task DefaultDateTimeValue()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime>>();
            Assert.Equal(default(DateTime), dateInstance.Instance.Value);
            var containerEle = dateInstance.Find("input").ParentElement;
            var dateIcon = containerEle.QuerySelector(".e-timeline-today");
            dateIcon.MouseDown();
            await Task.Delay(200);
            var popupEle = dateInstance.Find(".e-popup");
            Assert.Contains("e-popup", popupEle.ClassName);
            var tableElement = popupEle.QuerySelector("table");
            var parentContainer = tableElement.ParentElement.ParentElement;
            Assert.Contains("e-calendar", parentContainer.ClassName);
            var fromtTitle = "MMMM yyyy";
            var title = GetDateFormat(DateTime.Now, fromtTitle);
            var headerEle = popupEle.QuerySelector(".e-day.e-title");
            Assert.Equal(title, headerEle.InnerHtml);
            var inputEle = dateInstance.Find("input");
            Assert.Equal("1/1/0001 12:00 AM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
        }
        [Fact(Timeout = 10000)]
        public async Task PredefinedValue()
        {
            var calendar = RenderComponent<SfDateTimePicker<DateTime>>(parameters =>
                parameters.Add(val => val.Value, new DateTime(2020, 1, 1)));
            Assert.Equal(new DateTime(2020, 1, 1), calendar.Instance.Value);
            await calendar.Instance.ShowPopupAsync();
            var popupEle = calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var selectedCell = tableElement.QuerySelectorAll(".e-selected");
            Assert.Equal(1, selectedCell.Length);
            Assert.Equal("1", selectedCell[0].FirstElementChild.TextContent);
            Assert.Equal(GetDateFormat(new DateTime(2020, 1, 1), "dddd, MMMM d, yyyy"), selectedCell[0].FirstElementChild.GetAttribute("title"));
            await calendar.Instance.HidePopupAsync();
            await calendar.Instance.ClosePopupAsync();
            calendar.SetParametersAndRender(("Value", new DateTime(2020, 1, 2)));
            await calendar.Instance.ShowPopupAsync();
            popupEle = calendar.Find(".e-popup");
            tableElement = popupEle.QuerySelector("table");
            selectedCell = tableElement.QuerySelectorAll(".e-selected");
            tableElement = calendar.Find("table");
            selectedCell = tableElement.QuerySelectorAll(".e-selected");
            Assert.Equal(1, selectedCell.Length);
            Assert.Equal("2", selectedCell[0].FirstElementChild.TextContent);
            Assert.Equal(GetDateFormat(new DateTime(2020, 1, 2), "dddd, MMMM d, yyyy"), selectedCell[0].FirstElementChild.GetAttribute("title"));
        }
        [Fact(Timeout = 10000)]
        public async Task PredefinedNullableValue()
        {
            var calendar = RenderComponent<SfDateTimePicker<DateTime?>>(parameters =>
                parameters.Add(val => val.Value, new DateTime(2020, 1, 1))
            );
            Assert.Equal(new DateTime(2020, 1, 1), calendar.Instance.Value);
            await calendar.Instance.ShowPopupAsync();
            var popupEle = calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var selectedCell = tableElement.QuerySelectorAll(".e-selected");
            Assert.Equal(1, selectedCell.Length);
            Assert.Equal("1", selectedCell[0].FirstElementChild.TextContent);
            Assert.Equal(GetDateFormat(new DateTime(2020, 1, 1), "dddd, MMMM d, yyyy"), selectedCell[0].FirstElementChild.GetAttribute("title"));
            await calendar.Instance.HidePopupAsync();
            await calendar.Instance.ClosePopupAsync();
            calendar.SetParametersAndRender(("Value", new DateTime(2020, 1, 2)));
            await calendar.Instance.ShowPopupAsync();
            popupEle = calendar.Find(".e-popup");
            tableElement = popupEle.QuerySelector("table");
            selectedCell = tableElement.QuerySelectorAll(".e-selected");
            tableElement = calendar.Find("table");
            selectedCell = tableElement.QuerySelectorAll(".e-selected");
            Assert.Equal(1, selectedCell.Length);
            Assert.Equal("2", selectedCell[0].FirstElementChild.TextContent);
            Assert.Equal(GetDateFormat(new DateTime(2020, 1, 2), "dddd, MMMM d, yyyy"), selectedCell[0].FirstElementChild.GetAttribute("title"));
        }
        [Fact(Timeout = 10000)]
        public async Task DayHeaderFormat()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime>>(parameters =>
            parameters.Add(calendar => calendar.DayHeaderFormat, DayHeaderFormats.Abbreviated));
            var containerEle = Calendar.Find("input").ParentElement;
            var iconEle = containerEle.QuerySelector(".e-timeline-today");
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var headerElement = tableElement.QuerySelector(".e-week-header");
            Assert.Equal("Sun", headerElement.QuerySelector("th").TextContent);
            Calendar.SetParametersAndRender(("DayHeaderFormat", DayHeaderFormats.Narrow));
            tableElement = Calendar.Find("table");
            headerElement = tableElement.QuerySelector(".e-week-header");
            Assert.Equal("S", headerElement.QuerySelector("th").TextContent);
            Calendar.SetParametersAndRender(("DayHeaderFormat", DayHeaderFormats.Short));
            tableElement = Calendar.Find("table");
            headerElement = tableElement.QuerySelector(".e-week-header");
            Assert.Equal("S", headerElement.QuerySelector("th").TextContent);
            Calendar.SetParametersAndRender(("DayHeaderFormat", DayHeaderFormats.Wide));
            tableElement = Calendar.Find("table");
            headerElement = tableElement.QuerySelector(".e-week-header");
            Assert.Equal("Sunday", headerElement.QuerySelector("th").TextContent);
        }
        [Fact(Timeout = 10000)]
        public async Task FirstDayOfAWeek()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime>>();
            var containerEle = Calendar.Find("input").ParentElement;
            var iconEle = containerEle.QuerySelector(".e-timeline-today");
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var headerElement = tableElement.QuerySelector(".e-week-header");
            Assert.Equal("S", headerElement.QuerySelector("th").TextContent);
            Calendar.SetParametersAndRender(("FirstDayOfWeek", 2));
            tableElement = Calendar.Find("table");
            headerElement = tableElement.QuerySelector(".e-week-header");
            Assert.Equal("T", headerElement.QuerySelector("th").TextContent);
            Calendar.SetParametersAndRender(("FirstDayOfWeek", 0));
            tableElement = Calendar.Find("table");
            headerElement = tableElement.QuerySelector(".e-week-header");
            Assert.Equal("S", headerElement.QuerySelector("th").TextContent);
        }
        [Fact(Timeout = 10000)]
        public async Task MinValue()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime>>(parameters =>
            parameters.Add(calendar => calendar.Value, new DateTime(2020, 1, 1)));
            Assert.Equal(new DateTime(1900, 1, 1, 0, 0, 0), Calendar.Instance.Min);
            Calendar.SetParametersAndRender(("Min", new DateTime(2020, 1, 1, 5, 30, 0)));
            Assert.Equal(new DateTime(2020, 1, 1, 5, 30, 0), Calendar.Instance.Min);
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var parentContainer = tableElement.ParentElement.ParentElement;
            var buttons = parentContainer.QuerySelectorAll("button");
            var totalCells = tableElement.QuerySelectorAll("td");
            var disabledCells = tableElement.QuerySelectorAll("td.e-disabled");
            var enabledCells = tableElement.QuerySelectorAll("td:not(.e-disabled)");
            Assert.Equal(3, disabledCells.Length);
            Assert.Equal(totalCells.Length, (disabledCells.Length + enabledCells.Length));
            Assert.Contains("e-prev", buttons[0].ClassName);
            Assert.Contains("e-disabled", buttons[0].ClassName);
            Assert.Contains("e-next", buttons[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttons[1].ClassName);
        }
        //[Fact(Timeout = 10000)]
        //public async Task NavigateCalendarToMonthviewWithMinValue()
        //{
        //    var Calendar = RenderComponent<SfDateTimePicker<DateTime?>>(parameters =>
        //    parameters.Add(Cal => Cal.Min, new DateTime(2020, 3, 1, 3, 30, 0)));
        //    await Calendar.Instance.ShowPopupAsync();
        //    var popupEle = Calendar.Find(".e-popup");
        //    var tableElement = popupEle.QuerySelector("table");
        //    Assert.Equal(new DateTime(2020, 3, 1, 3, 30, 0), Calendar.Instance.Min);
        //    Assert.Equal("Month", Calendar.Instance.CurrentView());
        //    await Calendar.Instance.NavigateAsync(CalendarView.Year, Calendar.Instance.Min);
        //    Assert.Equal("Year", Calendar.Instance.CurrentView());
        //    await Calendar.Instance.HidePopupAsync();
        //    await Calendar.Instance.ClosePopup();
        //    await Calendar.Instance.ShowPopupAsync();
        //    popupEle = Calendar.Find(".e-popup");
        //    tableElement = popupEle.QuerySelector("table");
        //    Assert.Equal(7, tableElement.QuerySelectorAll("tr").Length);
        //    Assert.Equal(42, tableElement.QuerySelectorAll("td").Length);
        //}
        [Fact(Timeout = 10000)]
        public async Task MaxValue()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime>>(parameters =>
            parameters.Add(calendar => calendar.Value, new DateTime(2020, 1, 1)));
            Calendar.SetParametersAndRender(("Max", new DateTime(2020, 1, 1)));
            Assert.Equal(new DateTime(2020, 1, 1), Calendar.Instance.Max);
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var parentContainer = tableElement.ParentElement.ParentElement;
            var buttons = parentContainer.QuerySelectorAll("button");
            var totalCells = tableElement.QuerySelectorAll("td");
            var disabledCells = tableElement.QuerySelectorAll("td.e-disabled");
            var enabledCells = tableElement.QuerySelectorAll("td:not(.e-disabled)");
            Assert.Equal(38, disabledCells.Length);
            Assert.Equal(totalCells.Length, (disabledCells.Length + enabledCells.Length));
            Assert.Contains("e-prev", buttons[0].ClassName);
            Assert.DoesNotContain("e-disabled", buttons[0].ClassName);
            Assert.Contains("e-next", buttons[1].ClassName);
            Assert.Contains("e-disabled", buttons[1].ClassName);
        }
        [Fact(Timeout = 10000)]
        public async Task MinMaxValue()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime>>(parameters =>
            parameters.Add(calendar => calendar.Value, new DateTime(2017, 4, 4, 10, 30, 0)).Add(calendar => calendar.Min, new DateTime(2016, 12, 12, 10, 0, 0)).Add(param => param.Max, new DateTime(2017, 3, 3, 11, 0, 0)));
            Assert.Equal(new DateTime(2017, 3, 3, 11, 0, 0), Calendar.Instance.Max);
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            Assert.NotNull(popupEle);
            var inputEle = Calendar.Find("input");
            var containerEle = inputEle.ParentElement;
            Assert.Contains("e-error", containerEle.ClassName);
        }
        [Fact(Timeout = 10000)]
        public async Task MaxValueWithNullable()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime?>>(parameters =>
            parameters.Add(calendar => calendar.Value, new DateTime(2020, 1, 1)));
            Assert.Equal(new DateTime(2099, 12, 31, 23, 59, 59), Calendar.Instance.Max);
            Calendar.SetParametersAndRender(("Max", new DateTime(2020, 1, 1, 23, 59, 59)));
            Assert.Equal(new DateTime(2020, 1, 1, 23, 59, 59), Calendar.Instance.Max);
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var parentContainer = tableElement.ParentElement.ParentElement;
            var buttons = parentContainer.QuerySelectorAll("button");
            var totalCells = tableElement.QuerySelectorAll("td");
            var disabledCells = tableElement.QuerySelectorAll("td.e-disabled");
            var enabledCells = tableElement.QuerySelectorAll("td:not(.e-disabled)");
            Assert.Equal(38, disabledCells.Length);
            Assert.Equal(totalCells.Length, (disabledCells.Length + enabledCells.Length));
            Assert.Contains("e-prev", buttons[0].ClassName);
            Assert.DoesNotContain("e-disabled", buttons[0].ClassName);
            Assert.Contains("e-next", buttons[1].ClassName);
            Assert.Contains("e-disabled", buttons[1].ClassName);
        }
        [Fact(Timeout = 10000)]
        public async Task WeekNumber()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime>>();
            Assert.False(Calendar.Instance.WeekNumber);
            Calendar.SetParametersAndRender(("WeekNumber", true));
            Assert.True(Calendar.Instance.WeekNumber);
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var weekCells = tableElement.QuerySelectorAll("td.e-week-number");
            Assert.InRange(weekCells.Length, 5, 6);
        }
        [Fact(Timeout = 10000)]
        public async Task WeekNumberWithNullable()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime>>();
            Assert.False(Calendar.Instance.WeekNumber);
            Calendar.SetParametersAndRender(("WeekNumber", true));
            Assert.True(Calendar.Instance.WeekNumber);
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var weekCells = tableElement.QuerySelectorAll("td.e-week-number");
            Assert.InRange(weekCells.Length, 5, 6);
        }
        [Fact(Timeout = 10000)]
        public async Task TodayButton()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime>>();
            Assert.True(Calendar.Instance.ShowTodayButton);
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var parentContainer = tableElement.ParentElement.ParentElement;
            var buttons = parentContainer.QuerySelectorAll("button");
            Assert.Equal(3, buttons.Length);
            Assert.Contains("e-today", buttons[2].ClassName);
            Assert.Contains("e-btn", buttons[2].ClassName);
            Assert.Equal("Today", buttons[2].TextContent);
            Calendar.SetParametersAndRender(("ShowTodayButton", false));
            Assert.False(Calendar.Instance.ShowTodayButton);
            tableElement = Calendar.Find("table");
            parentContainer = tableElement.ParentElement.ParentElement;
            buttons = parentContainer.QuerySelectorAll("button");
            Assert.Equal(2, buttons.Length);
        }
        [Fact(Timeout = 10000)]
        public async Task WeekRules()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime?>>(parameters =>
            parameters.Add(Cal => Cal.WeekNumber, true)
            .Add(Cal => Cal.WeekRule, System.Globalization.CalendarWeekRule.FirstDay)
            .Add(Cal => Cal.Value, new DateTime(2021, 1, 1)));
            var containerEle = Calendar.Find("input").ParentElement;
            var iconEle = containerEle.QuerySelector(".e-timeline-today");
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var tHead = tableElement.QuerySelector("thead.e-week-header");
            var headCell = tHead.QuerySelectorAll("th");
            Assert.Contains("e-week-number", headCell[0].ClassName);
            Assert.Empty(headCell[0].InnerHtml);
            var tBody = tableElement.QuerySelector("tbody");
            var tRows = tBody.QuerySelectorAll("tr");
            var tdCell = tRows[0].QuerySelectorAll("td");
            Assert.Contains("e-week-number", tdCell[0].ClassName);
            Assert.Contains("e-cell", tdCell[0].ClassName);
            Assert.Equal("1", tdCell[0].QuerySelector("span").InnerHtml.Replace("\n", "").Trim());
            var selectedCell = tBody.QuerySelector("td.e-selected");
            var title = GetDateFormat(Calendar.Instance.Value, FORMATFULLDATE);
            Assert.Equal(title, selectedCell.QuerySelector("span").GetAttribute("title"));
            Assert.Equal("e-day", selectedCell.QuerySelector("span").ClassName);
            var dayVal = GetDateFormat(Calendar.Instance.Value, FORMATDATE);
            Assert.Equal(new DateTime(2021, 1, 1).Day.ToString(), selectedCell.QuerySelector("span").InnerHtml.Replace("\n", "").Trim());
        }
        [Fact(Timeout = 10000)]
        public async Task StartAndDepthAsMonth()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime>>(parameters =>
            parameters.Add(Cal => Cal.Start, CalendarView.Month)
            .Add(Cal => Cal.Depth, CalendarView.Month).
            Add(p => p.Value, new DateTime(1900, 1, 1)));
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var parentContainer = tableElement.ParentElement.ParentElement;
            Assert.Equal(CalendarView.Month, Calendar.Instance.Start);
            Assert.Equal(CalendarView.Month, Calendar.Instance.Depth);
            Assert.Equal("Month", Calendar.Instance.CurrentView());
            Assert.Contains("January", parentContainer.QuerySelector(".e-title").TextContent);
            Assert.Equal(42, tableElement.QuerySelectorAll("td").Length);
            Assert.Equal(1, tableElement.QuerySelectorAll("td.e-selected").Length);
            Assert.Equal("1", tableElement.QuerySelectorAll("td.e-selected")[0].FirstElementChild.TextContent);
        }

        [Fact(Timeout = 10000)]
        public async Task StartAndDepthAsMonthWithNullable()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime?>>(parameters =>
            parameters.Add(Cal => Cal.Start, CalendarView.Month)
            .Add(Cal => Cal.Depth, CalendarView.Month));
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var parentContainer = tableElement.ParentElement.ParentElement;
            Assert.Equal(CalendarView.Month, Calendar.Instance.Start);
            Assert.Equal(CalendarView.Month, Calendar.Instance.Depth);
            Assert.Equal("Month", Calendar.Instance.CurrentView());
            Assert.Contains(DateTime.Now.ToString("MMMM"), parentContainer.QuerySelector(".e-title").TextContent);
            Assert.Equal(42, tableElement.QuerySelectorAll("td").Length);
            Assert.Equal(1, tableElement.QuerySelectorAll("td.e-focused-date").Length);
            Assert.Equal(DateTime.Now.Day.ToString(), tableElement.QuerySelectorAll("td.e-focused-date")[0].FirstElementChild.TextContent);
        }
        [Fact(Timeout = 10000)]
        public async Task StartAndDepthAsYear()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime>>(parameters =>
            parameters.Add(Cal => Cal.Start, CalendarView.Year)
            .Add(Cal => Cal.Depth, CalendarView.Year).
            Add(p => p.Value, new DateTime(1900, 1, 1)));
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var parentContainer = tableElement.ParentElement.ParentElement;
            Assert.Equal(CalendarView.Year, Calendar.Instance.Start);
            Assert.Equal(CalendarView.Year, Calendar.Instance.Depth);
            Assert.Equal("Year", Calendar.Instance.CurrentView());
            Assert.Contains("1900", parentContainer.QuerySelector(".e-title").TextContent);
            Assert.Equal(12, tableElement.QuerySelectorAll("td").Length);
            Assert.Equal(1, tableElement.QuerySelectorAll("td.e-selected").Length);
            Assert.Equal("Jan", tableElement.QuerySelectorAll("td.e-selected")[0].FirstElementChild.TextContent);
        }
        [Fact(Timeout = 10000)]
        public async Task StartAndDepthAsYearWithNullable()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime?>>(parameters =>
            parameters.Add(Cal => Cal.Start, CalendarView.Year)
            .Add(Cal => Cal.Depth, CalendarView.Year));
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var parentContainer = tableElement.ParentElement.ParentElement;
            Assert.Equal(CalendarView.Year, Calendar.Instance.Start);
            Assert.Equal(CalendarView.Year, Calendar.Instance.Depth);
            Assert.Equal("Year", Calendar.Instance.CurrentView());
            Assert.Contains(DateTime.Now.ToString("yyyy"), parentContainer.QuerySelector(".e-title").TextContent);
            Assert.Equal(12, tableElement.QuerySelectorAll("td").Length);
            Assert.Equal(1, tableElement.QuerySelectorAll("td.e-focused-date").Length);
            Assert.Equal(DateTime.Now.ToString("MMM"), tableElement.QuerySelectorAll("td.e-focused-date")[0].FirstElementChild.TextContent);
        }
        [Fact(Timeout = 10000)]
        public async Task StartAndDepthAsDecade()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime>>(parameters =>
            parameters.Add(Cal => Cal.Start, CalendarView.Decade)
            .Add(Cal => Cal.Depth, CalendarView.Decade).
            Add(p => p.Value, new DateTime(1900, 1, 1)));
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var parentContainer = tableElement.ParentElement.ParentElement;
            Assert.Equal(CalendarView.Decade, Calendar.Instance.Start);
            Assert.Equal(CalendarView.Decade, Calendar.Instance.Depth);
            Assert.Equal("Decade", Calendar.Instance.CurrentView());
            Assert.Contains("1900 - 1909", parentContainer.QuerySelector(".e-title").TextContent);
            Assert.Equal(12, tableElement.QuerySelectorAll("td").Length);
            Assert.Equal(1, tableElement.QuerySelectorAll("td.e-selected").Length);
            Assert.Equal("1900", tableElement.QuerySelectorAll("td.e-selected")[0].FirstElementChild.TextContent);
        }
        [Fact(Timeout = 10000)]
        public async Task StartAndDepthAsDecadeWithNullable()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime?>>(parameters =>
            parameters.Add(Cal => Cal.Start, CalendarView.Decade)
            .Add(Cal => Cal.Depth, CalendarView.Decade));
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var parentContainer = tableElement.ParentElement.ParentElement;
            Assert.Equal(CalendarView.Decade, Calendar.Instance.Start);
            Assert.Equal(CalendarView.Decade, Calendar.Instance.Depth);
            Assert.Equal("Decade", Calendar.Instance.CurrentView());
            Assert.Contains(this.getDecadeTitle(DateTime.Now), parentContainer.QuerySelector(".e-title").TextContent);
            Assert.Equal(12, tableElement.QuerySelectorAll("td").Length);
            Assert.Equal(1, tableElement.QuerySelectorAll("td.e-focused-date").Length);
            Assert.Equal(DateTime.Now.Year.ToString(), tableElement.QuerySelectorAll("td.e-focused-date")[0].FirstElementChild.TextContent);
        }
        [Fact(Timeout = 10000)]
        public async Task SelectDays()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime>>(parameters =>
            parameters.Add(calendar => calendar.Value, new DateTime(1900, 1, 1)));
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var tRows = tableElement.QuerySelectorAll("tr");
            tRows[2].QuerySelectorAll("td")[2].Click();
            Assert.Equal(1900, Calendar.Instance.Value.Year);
            var inputEle = Calendar.Find("input");
            Assert.Contains("1900", inputEle.GetAttribute("value"));
        }
        [Fact(Timeout = 10000)]
        public async Task SelectDaysWithNullable()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime?>>();
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var tRows = tableElement.QuerySelectorAll("tr");
            tRows[2].QuerySelectorAll("td")[2].Click();
            Assert.Equal(DateTime.Now.Year, Calendar.Instance.Value?.Year);
            var inputEle = Calendar.Find("input");
            Assert.Contains(DateTime.Now.Year.ToString(), inputEle.GetAttribute("value"));
        }
        [Fact(Timeout = 10000)]
        public async Task SelectDecade()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime>>(param => param.Add(p => p.Start, CalendarView.Decade).Add(p => p.Depth, CalendarView.Decade).
            Add(p => p.Value, new DateTime(1900, 1, 1)));
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var tContent = popupEle.QuerySelector(".e-content");
            Assert.Contains("e-decade", tContent.ClassName);
            var tRows = tableElement.QuerySelectorAll("tr");
            Assert.Equal(3, tRows.Length);
            var header = popupEle.QuerySelector(".e-header.e-decade");
            var title = getDecadeTitle(Calendar.Instance.Min);
            Assert.Equal(title, header.TextContent.Replace("\n", "").Trim());
            var tdCell = tRows[0].QuerySelectorAll("td");
            Assert.Equal(4, tdCell.Length);
            tRows[1].QuerySelectorAll("td")[2].Click();
            Assert.Equal(1905, Calendar.Instance.Value.Year);
            var inputEle = Calendar.Find("input");
            Assert.Contains("1905", inputEle.GetAttribute("value"));
        }
        [Fact(Timeout = 10000)]
        public async Task SelectDecadeWithNullable()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime?>>(param => param.Add(p => p.Start, CalendarView.Decade).Add(p => p.Depth, CalendarView.Decade).Add(p => p.Format, "yyyy"));
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var tContent = popupEle.QuerySelector(".e-content");
            Assert.Contains("e-decade", tContent.ClassName);
            var tRows = tableElement.QuerySelectorAll("tr");
            Assert.Equal(3, tRows.Length);
            var header = popupEle.QuerySelector(".e-header.e-decade");
            var title = getDecadeTitle(DateTime.Now.Date);
            Assert.Equal(title, header.TextContent.Replace("\n", "").Trim());
            var tdCell = tRows[0].QuerySelectorAll("td");
            Assert.Equal(4, tdCell.Length);
            tRows[1].QuerySelectorAll("td")[2].Click();
            Assert.Equal(tRows[1].QuerySelectorAll("td")[2].TextContent, Calendar.Instance.Value?.Date.ToString("yyyy"));
            var inputEle = Calendar.Find("input");
            Assert.Contains(tRows[1].QuerySelectorAll("td")[2].TextContent, inputEle.GetAttribute("value"));
        }
        [Fact(Timeout = 10000)]
        public async Task SelectYear()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime>>(param => param.Add(p => p.Start, CalendarView.Year).Add(p => p.Depth, CalendarView.Year).
            Add(p => p.Value, new DateTime(1900, 1, 1)));
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var tContent = popupEle.QuerySelector(".e-content");
            Assert.Contains("e-year", tContent.ClassName);
            var tRows = tableElement.QuerySelectorAll("tr");
            Assert.Equal(3, tRows.Length);
            var header = popupEle.QuerySelector(".e-header.e-year");
            Assert.Equal("1900", header.TextContent.Replace("\n", "").Trim());
            var tdCell = tRows[0].QuerySelectorAll("td");
            Assert.Equal(4, tdCell.Length);
            Assert.Equal("Jan", tRows[0].QuerySelector("td span").InnerHtml.Replace("\n", "").Trim());
            tRows[1].QuerySelectorAll("td")[2].Click();
            Assert.Equal(1900, Calendar.Instance.Value.Year);
            Assert.Equal(7, Calendar.Instance.Value.Month);
            var inputEle = Calendar.Find("input");
            Assert.Contains("7", inputEle.GetAttribute("value"));
        }
        [Fact(Timeout = 10000)]
        public async Task SelectYearWithNullable()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime?>>(param => param.Add(p => p.Start, CalendarView.Year).Add(p => p.Depth, CalendarView.Year));
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var tContent = popupEle.QuerySelector(".e-content");
            Assert.Contains("e-year", tContent.ClassName);
            var tRows = tableElement.QuerySelectorAll("tr");
            Assert.Equal(3, tRows.Length);
            var header = popupEle.QuerySelector(".e-header.e-year");
            Assert.Equal(DateTime.Now.Year.ToString(), header.TextContent.Replace("\n", "").Trim());
            var tdCell = tRows[0].QuerySelectorAll("td");
            Assert.Equal(4, tdCell.Length);
            Assert.Equal("Jan", tRows[0].QuerySelector("td span").InnerHtml.Replace("\n", "").Trim());
            tRows[1].QuerySelectorAll("td")[2].Click();
            Assert.Equal(DateTime.Now.Year, Calendar.Instance.Value?.Year);
            Assert.Equal(7, Calendar.Instance.Value?.Month);
            var inputEle = Calendar.Find("input");
            Assert.Contains("7", inputEle.GetAttribute("value"));
        }
        [Fact(Timeout = 10000)]
        public async Task DynamicValueBinding()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime?>>();
            Assert.Null(Calendar.Instance.Value);
            var inputEle = Calendar.Find("input");
            Assert.Null(inputEle.GetAttribute("value"));
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var fromtTitle = "MMMM yyyy";
            var title = GetDateFormat(DateTime.Now, fromtTitle);
            var headerEle = popupEle.QuerySelector(".e-day.e-title");
            Assert.Equal(title, headerEle.InnerHtml.Replace("\n", "").Trim());
            await Calendar.Instance.HidePopupAsync();
            await Calendar.Instance.ClosePopupAsync ();
            Calendar.SetParametersAndRender(("Value", new DateTime(2021, 3, 5)));
            inputEle = Calendar.Find("input");
            Assert.NotNull(inputEle.GetAttribute("value"));
            Assert.Contains("2021", inputEle.GetAttribute("value"));
            await Calendar.Instance.ShowPopupAsync();
            popupEle = Calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var selectedCell = tableElement.QuerySelectorAll("td.e-selected");
            Assert.Equal(1, selectedCell.Length);
            Assert.Equal("5", selectedCell[0].FirstElementChild.TextContent.Replace("\n", "").Trim());
            Assert.Equal(GetDateFormat(Calendar.Instance.Value, "dddd, MMMM d, yyyy"), selectedCell[0].FirstElementChild.GetAttribute("title"));
        }
        [Fact(Timeout = 10000)]
        public async Task InputValueBinding()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime?>>();
            Assert.Null(Calendar.Instance.Value);
            var inputEle = Calendar.Find("input");
            Assert.Null(inputEle.GetAttribute("value"));
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var fromtTitle = "MMMM yyyy";
            var title = GetDateFormat(DateTime.Now, fromtTitle);
            var headerEle = popupEle.QuerySelector(".e-day.e-title");
            Assert.Equal(title, headerEle.InnerHtml.Replace("\n", "").Trim());
            await Calendar.Instance.HidePopupAsync();
            await Calendar.Instance.ClosePopupAsync();
            inputEle = Calendar.Find("input");
            inputEle.SetAttribute("value", "05/03/2021");
            inputEle.Change(new ChangeEventArgs() { Value = "05/03/2021" });
            inputEle = Calendar.Find("input");
            Assert.NotNull(inputEle.GetAttribute("value"));
            Assert.Contains("2021", inputEle.GetAttribute("value"));
        }
        [Fact(Timeout = 10000)]
        public async Task TodayButtonClick()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime>>();
            Assert.Equal(default(DateTime), Calendar.Instance.Value);
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            Assert.Equal(0, tableElement.QuerySelectorAll("td.e-selected").Length);
            Assert.Equal(1, tableElement.QuerySelectorAll("td.e-focused-date").Length);
            var selectedDate = Calendar.Find("table").QuerySelector("td.e-selected");
            Assert.Null(selectedDate);
            var buttonList = Calendar.FindAll("button");
            buttonList[2].Click();
            await Calendar.Instance.ShowPopupAsync();
            tableElement = Calendar.Find("table");
            Assert.Equal(1, tableElement.QuerySelectorAll("td.e-selected").Length);
            Assert.Equal(DateTime.Now.Day.ToString(), tableElement.QuerySelector("td.e-selected").FirstElementChild.TextContent);
            Assert.Equal(1, tableElement.QuerySelectorAll("td.e-focused-date").Length);
            Assert.True(tableElement.QuerySelector("td.e-selected").ClassList.Contains("e-today"));
        }
        [Fact(Timeout = 10000)]
        public async Task TodayButtonClickWithNullable()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime?>>();
            Assert.Null(Calendar.Instance.Value);
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            Assert.Equal(0, tableElement.QuerySelectorAll("td.e-selected").Length);
            Assert.Equal(1, tableElement.QuerySelectorAll("td.e-focused-date").Length);
            var selectedDate = Calendar.Find("table").QuerySelector("td.e-selected");
            Assert.Null(selectedDate);
            var buttonList = Calendar.FindAll("button");
            buttonList[2].Click();
            await Calendar.Instance.ShowPopupAsync();
            popupEle = Calendar.Find(".e-popup");
            tableElement = popupEle.QuerySelector("table");
            Assert.Equal(1, tableElement.QuerySelectorAll("td.e-selected").Length);
            Assert.Equal(DateTime.Now.Day.ToString(), tableElement.QuerySelector("td.e-selected").FirstElementChild.TextContent);
            Assert.Equal(1, tableElement.QuerySelectorAll("td.e-focused-date").Length);
            Assert.True(tableElement.QuerySelector("td.e-selected").ClassList.Contains("e-today"));
        }
        [Fact(Timeout = 10000)]
        public async Task TodayButtonClickInOtherViews()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime>>();
            Assert.Equal(default(DateTime), Calendar.Instance.Value);
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var parentContainer = tableElement.ParentElement.ParentElement;
            Assert.Equal(0, tableElement.QuerySelectorAll("td.e-selected").Length);
            Assert.Equal(1, tableElement.QuerySelectorAll("td.e-focused-date").Length);
            var selectedDate = Calendar.Find("table").QuerySelector("td.e-selected");
            Assert.Null(selectedDate);
            Assert.Equal("Month", Calendar.Instance.CurrentView());
            // Today button click in Year view
            parentContainer.QuerySelector(".e-title").Click();
            var tContent = popupEle.QuerySelector(".e-content");
            Assert.Contains("e-year", tContent.ClassName);
            var buttonList = Calendar.FindAll("button");
            buttonList[2].Click();
            await Calendar.Instance.ShowPopupAsync();
            popupEle = Calendar.Find(".e-popup");
            tableElement = popupEle.QuerySelector("table");
            Assert.Equal(1, tableElement.QuerySelectorAll("td.e-selected").Length);
            Assert.Equal(DateTime.Now.Day.ToString(), tableElement.QuerySelector("td.e-selected").FirstElementChild.TextContent);
            Assert.Equal(1, tableElement.QuerySelectorAll("td.e-focused-date").Length);
            Assert.True(tableElement.QuerySelector("td.e-selected").ClassList.Contains("e-today"));
            Assert.Equal("Month", Calendar.Instance.CurrentView());
            // Today button click in Decade View
            parentContainer = tableElement.ParentElement.ParentElement;
            parentContainer.QuerySelector(".e-title").Click();
            tContent = popupEle.QuerySelector(".e-content");
            Assert.Contains("e-year", tContent.ClassName);
            parentContainer = tableElement.ParentElement.ParentElement;
            parentContainer.QuerySelector(".e-title").Click(); tContent = popupEle.QuerySelector(".e-content");
            Assert.Contains("e-decade", tContent.ClassName);
            buttonList[2].Click();
            tableElement = Calendar.Find("table");
            tContent = popupEle.QuerySelector(".e-content");
            Assert.Contains("e-month", tContent.ClassName);
            Assert.Equal(1, tableElement.QuerySelectorAll("td.e-selected").Length);
            Assert.Equal(DateTime.Now.Day.ToString(), tableElement.QuerySelector("td.e-selected").FirstElementChild.TextContent);
            Assert.Equal(1, tableElement.QuerySelectorAll("td.e-focused-date").Length);
            Assert.True(tableElement.QuerySelector("td.e-selected").ClassList.Contains("e-today"));
        }
        [Fact(Timeout = 10000)]
        public async Task TodayButtonClickWithStartAndDepthAsYear()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime>>(parameters => parameters.
            Add(Cal => Cal.Start, CalendarView.Year).
            Add(Cal => Cal.Depth, CalendarView.Year));
            Assert.Equal(default(DateTime), Calendar.Instance.Value);
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var parentContainer = tableElement.ParentElement.ParentElement;
            Assert.Equal(0, tableElement.QuerySelectorAll("td.e-selected").Length);
            Assert.Equal(1, tableElement.QuerySelectorAll("td.e-focused-date").Length);
            var selectedDate = Calendar.Find("table").QuerySelector("td.e-selected");
            Assert.Null(selectedDate);
            Assert.Equal("Year", Calendar.Instance.CurrentView());
            var buttonList = Calendar.FindAll("button");
            buttonList[2].Click();
            var tContent = popupEle.QuerySelector(".e-content");
            Assert.Contains("e-year", tContent.ClassName);
            tableElement = popupEle.QuerySelector("table");
            Assert.Equal(1, tableElement.QuerySelectorAll("td.e-selected").Length);
            Assert.Equal(DateTime.Now.ToString("MMM"), tableElement.QuerySelector("td.e-selected").FirstElementChild.TextContent);
            Assert.Equal(0, tableElement.QuerySelectorAll("td.e-focused-date").Length);
        }
        [Fact(Timeout = 10000)]
        public async Task TodayButtonClickWithStartAndDepthAsDecade()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime>>(parameters => parameters.
            Add(Cal => Cal.Start, CalendarView.Decade).
            Add(Cal => Cal.Depth, CalendarView.Decade));
            Assert.Equal(default(DateTime), Calendar.Instance.Value);
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var parentContainer = tableElement.ParentElement.ParentElement;
            Assert.Equal(0, tableElement.QuerySelectorAll("td.e-selected").Length);
            Assert.Equal(1, tableElement.QuerySelectorAll("td.e-focused-date").Length);
            var selectedDate = Calendar.Find("table").QuerySelector("td.e-selected");
            Assert.Null(selectedDate);
            Assert.Equal("Decade", Calendar.Instance.CurrentView());
            var buttonList = Calendar.FindAll("button");
            buttonList[2].Click();
            var tContent = popupEle.QuerySelector(".e-content");
            Assert.Contains("e-decade", tContent.ClassName);
            tableElement = popupEle.QuerySelector("table");
            Assert.Equal(1, tableElement.QuerySelectorAll("td.e-selected").Length);
            Assert.Equal(DateTime.Now.ToString("yyyy"), tableElement.QuerySelector("td.e-selected").FirstElementChild.TextContent);
            Assert.Equal(0, tableElement.QuerySelectorAll("td.e-focused-date").Length);
        }

        [Fact(Timeout = 10000)]
        public async Task AllowEdit()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>();
            var inputEle = dateInstance.Find("input");
            Assert.Null(inputEle.GetAttribute("readonly"));
            Assert.True(dateInstance.Instance.AllowEdit);
            dateInstance.SetParametersAndRender(("AllowEdit", false));
            inputEle = dateInstance.Find("input");
            Assert.NotNull(inputEle.GetAttribute("readonly"));
            Assert.False(dateInstance.Instance.AllowEdit);
            var containerEle = dateInstance.Find("input").ParentElement;
            var dateIcon = containerEle.QuerySelector(".e-timeline-today");
            dateIcon.MouseDown();
            await Task.Delay(300);
            var popupEle = dateInstance.Find(".e-popup");
            Assert.Contains("e-popup", popupEle.ClassName);
        }
        [Fact(Timeout = 10000)]
        public void FloatingLabel()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(parameter => parameter.Add(p => p.Placeholder, "Enter the value"));
            var containerEle = dateInstance.Find("input").ParentElement;
            Assert.DoesNotContain("e-float-text", containerEle.ClassName);
            dateInstance.SetParametersAndRender(("FloatLabelType", FloatLabelType.Auto));
            containerEle = dateInstance.Find("input").ParentElement;
            Assert.Contains("e-float-input", containerEle.ClassName);
            var floatEle = containerEle.QuerySelector(".e-float-text");
            Assert.NotNull(floatEle);
            Assert.Contains("e-label-bottom", floatEle.ClassName);
            dateInstance.SetParametersAndRender(("Value", DateTime.Now));
            var inputEle = dateInstance.Find("input");
            containerEle = dateInstance.Find("input").ParentElement;
            floatEle = containerEle.QuerySelector(".e-float-text");
            Assert.Contains("e-label-top", floatEle.ClassName);
            dateInstance.SetParametersAndRender(("FloatLabelType", FloatLabelType.Always));
            containerEle = dateInstance.Find("input").ParentElement;
            Assert.Contains("e-float-input", containerEle.ClassName);
            floatEle = containerEle.QuerySelector(".e-float-text");
            Assert.NotNull(floatEle);
            Assert.Contains("e-label-top", floatEle.ClassName);
            dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(parameter => parameter.Add(p => p.Placeholder, "Enter the value").Add(p => p.FloatLabelType, FloatLabelType.Always));
            inputEle = dateInstance.Find("input");
            inputEle.Focus();
            containerEle = dateInstance.Find("input").ParentElement;
            floatEle = containerEle.QuerySelector(".e-float-text");
            Assert.NotNull(floatEle);
            Assert.Contains("e-label-top", floatEle.ClassName);
            dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(parameter => parameter.Add(p => p.Placeholder, "Enter the value").Add(p => p.FloatLabelType, FloatLabelType.Never));
            containerEle = dateInstance.Find("input").ParentElement;
            Assert.DoesNotContain("e-float-input", containerEle.ClassName);
            floatEle = containerEle.QuerySelector(".e-float-text");
            Assert.Null(floatEle);
            dateInstance.SetParametersAndRender(("FloatLabelType", FloatLabelType.Auto));
            containerEle = dateInstance.Find("input").ParentElement;
            Assert.Contains("e-float-input", containerEle.ClassName);
            floatEle = containerEle.QuerySelector(".e-float-text");
            Assert.NotNull(floatEle);
            Assert.Contains("e-label-bottom", floatEle.ClassName);
            inputEle = dateInstance.Find("input");
            inputEle.Focus();
            containerEle = dateInstance.Find("input").ParentElement;
            floatEle = containerEle.QuerySelector(".e-float-text");
            Assert.NotNull(floatEle);
            Assert.Contains("e-label-top", floatEle.ClassName);
        }
        [Fact(Timeout = 10000)]
        public async Task DefaultFormat()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(param => param.Add(p => p.Value, new DateTime(2021, 3, 5)));
            await dateInstance.Instance.ShowPopupAsync();
            var popupEle = dateInstance.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var tRows = tableElement.QuerySelectorAll("tr");
            tRows[2].QuerySelectorAll("td")[2].Click();
            Assert.Equal(new DateTime(2021, 3, 9), dateInstance.Instance.Value);
            var inputEle = dateInstance.Find("input");
            var inputValue = GetDateFormat(dateInstance.Instance.Value, CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern + " " + CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern);
            Assert.Equal(inputValue, inputEle.GetAttribute("value"));
        }
        [Fact(Timeout = 10000)]
        public void InputAttribute()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(param => param.Add(p => p.InputAttributes, new Dictionary<string, object>() { { "required", "true" }, { "name", "datepicker" }, { "class", "e-date-calendar" } }));
            var inputEle = dateInstance.Find("input");
            var containerEle = inputEle.ParentElement;
            Assert.Contains("datepicker", inputEle.GetAttribute("name"));
            Assert.Contains("true", inputEle.GetAttribute("required"));
            Assert.Contains("e-date-calendar", inputEle.ClassName);
        }
        [Fact(Timeout = 10000)]
        public async Task CurrentViewMethod()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime>>();
            Assert.Equal("Month", dateInstance.Instance.CurrentView());
            await dateInstance.Instance.ShowPopupAsync();
            var popupEle = dateInstance.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var parentContainer = tableElement.ParentElement.ParentElement;
            parentContainer.QuerySelector(".e-title").Click();
            var tContent = popupEle.QuerySelector(".e-content");
            Assert.Contains("e-year", tContent.ClassName);
            popupEle = dateInstance.Find(".e-popup");
            tableElement = popupEle.QuerySelector("table");
            parentContainer = tableElement.ParentElement.ParentElement;
            parentContainer.QuerySelector(".e-title").Click();
            tContent = popupEle.QuerySelector(".e-content");
            Assert.Contains("e-decade", tContent.ClassName);
        }
        [Fact(Timeout = 10000)]
        public async Task NavigateToMethod()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime>>();
            await dateInstance.Instance.ShowPopupAsync();
            var popupEle = dateInstance.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            Assert.Equal("Month", dateInstance.Instance.CurrentView());
            // Navigating to Year view
            await dateInstance.Instance.NavigateAsync(CalendarView.Year, DateTime.Now);
            Assert.Equal("Year", dateInstance.Instance.CurrentView());
            // Navigating to Decade view
            await dateInstance.Instance.NavigateAsync(CalendarView.Decade, DateTime.Now);
            Assert.Equal("Decade", dateInstance.Instance.CurrentView());
            // Navigating to Month View
            await dateInstance.Instance.NavigateAsync(CalendarView.Month, DateTime.Now);
            Assert.Equal("Month", dateInstance.Instance.CurrentView());

        }
        [Fact(Timeout = 10000)]
        public async Task ShowHideDateTime()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>();
            await dateInstance.Instance.ShowPopupAsync();
            var popupEle = dateInstance.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            Assert.Equal("Month", dateInstance.Instance.CurrentView());
            Assert.NotNull(tableElement);
            await dateInstance.Instance.HidePopupAsync();
            await dateInstance.Instance.ClosePopupAsync();
            await dateInstance.Instance.ShowTimePopupAsync();
            popupEle = dateInstance.Find(".e-popup");
            var liCollec = popupEle.QuerySelectorAll("li");
            liCollec[1].Click();
            var inputEle = dateInstance.Find("input");
            Assert.Contains("12:30", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
            Assert.Equal(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 30, 0), dateInstance.Instance.Value);
        }
        [Fact(Timeout = 10000)]
        public async Task StrictModeRange()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime>>(param => param.Add(p => p.Value, new DateTime(2016, 12, 10, 10, 0, 0)).Add(p => p.Min, new DateTime(2016, 4, 4, 10, 0, 0)).Add(p => p.Max, new DateTime(2017, 3, 3, 11, 0, 0)).Add(p => p.StrictMode, true));
            await dateInstance.Instance.ShowTimePopupAsync();
            var popupEle = dateInstance.Find(".e-popup");
            var inputEle = dateInstance.Find("input");
            Assert.Contains("10:00 AM", inputEle.GetAttribute("value")?.Replace('\u202F', ' ').Trim());
            var selectedVaue = popupEle.QuerySelector("li.e-active").GetAttribute("data-value");
            Assert.Equal("10:00 AM", selectedVaue.Replace('\u202F', ' ').Trim());
            var liCollec = popupEle.QuerySelectorAll("li");
            liCollec[1].Click();
            inputEle = dateInstance.Find("input");
            Assert.Contains("12:30 AM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
            selectedVaue = popupEle.QuerySelector("li.e-active").GetAttribute("data-value");
            Assert.Equal("12:30 AM", selectedVaue?.Replace('\u202F', ' ').Trim());
        }
        [Fact(Timeout = 10000)]
        public async Task ValueChangeOnDynamically()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime>>(param => param.Add(p => p.Value, new DateTime(2014, 01, 01, 10, 0, 0)));
            var buttonClickCallback = EventCallback.Factory.Create<MouseEventArgs>(this, async (args) => {
                dateInstance.SetParametersAndRender(parameter => parameter.Add(p => p.Value, DateTime.Now));
            });
            var button = RenderComponent<SfButton>(parameters => parameters
                .Add(p => p.OnClick, buttonClickCallback)
            );
            var inputEle = dateInstance.Find("input");
            Assert.Contains("10:00 AM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
            inputEle.SetAttribute("value", "1/1/2020 10:00 AM");
            var buttonElem = button.Find("button");
            buttonElem.Click();
            await Task.Delay(100);
            await dateInstance.Instance.ShowTimePopupAsync();
            var popupEle = dateInstance.Find(".e-popup");
            var liCollec = popupEle.QuerySelectorAll("li");
            liCollec[1].Click();
            inputEle = dateInstance.Find("input");
            Assert.Contains("12:30 AM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
            var selectedVaue = popupEle.QuerySelector("li.e-active").GetAttribute("data-value");
            Assert.Equal("12:30 AM", selectedVaue.Replace('\u202F', ' ').Trim());
        }
        [Fact(Timeout = 10000, DisplayName = "show method with input focus related test case")]
        public async Task CheckInputFocus()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>();
            await dateInstance.Instance.ShowTimePopupAsync();
            var popupEle = dateInstance.Find(".e-popup");
            var inputEle = dateInstance.Find("input");
            Assert.DoesNotContain("e-input-focus", inputEle.ParentElement.ClassName);
            var timeIconEle = inputEle.ParentElement.QuerySelector(".e-input-group-icon.e-clock");
            var liCollec = popupEle.QuerySelectorAll("li");
            liCollec[1].Click();
            inputEle = dateInstance.Find("input");
            Assert.Contains("12:30 AM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
            var selectedVaue = popupEle.QuerySelector("li.e-active").GetAttribute("data-value");
            Assert.Equal("12:30 AM", selectedVaue.Replace('\u202F', ' ').Trim());
        }
        [Fact(Timeout = 10000)]
        public async Task TimeSteps()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(param => param.Add(p => p.Step, 30));
            await dateInstance.Instance.ShowTimePopupAsync();
            var popupEle = dateInstance.Find(".e-popup");
            var liCollec = popupEle.QuerySelectorAll("li");
            Assert.Equal("12:30 AM", liCollec[1].GetAttribute("data-value")?.Replace('\u202F', ' ').Trim());
            await dateInstance.Instance.HidePopupAsync();
            await dateInstance.Instance.ClosePopupAsync();
            dateInstance.SetParametersAndRender(param => param.Add(p => p.Step, 10));
            await dateInstance.Instance.ShowTimePopupAsync();
            popupEle = dateInstance.Find(".e-popup");
            liCollec = popupEle.QuerySelectorAll("li");
            Assert.Equal("12:10 AM", liCollec[1].GetAttribute("data-value")?.Replace('\u202F', ' ').Trim());
        }
        [Fact(Timeout = 10000)]
        public async Task TimeFormat()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(param => param.Add(p => p.TimeFormat, "h:mm tt"));
            await dateInstance.Instance.ShowTimePopupAsync();
            var popupEle = dateInstance.Find(".e-popup");
            var inputEle = dateInstance.Find("input");
            var liCollec = popupEle.QuerySelectorAll("li");
            Assert.Equal("12:30 AM", liCollec[1].GetAttribute("data-value").Replace('\u202F', ' ').Trim());
            liCollec[1].Click();
            Assert.Contains(DateTime.Now.Day.ToString(), inputEle.GetAttribute("value"));
            Assert.Contains("12:30 AM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
            await dateInstance.Instance.HidePopupAsync();
            await dateInstance.Instance.ClosePopupAsync();
            dateInstance.SetParametersAndRender(param => param.Add(p => p.Step, 10));
            await dateInstance.Instance.ShowTimePopupAsync();
            popupEle = dateInstance.Find(".e-popup");
            liCollec = popupEle.QuerySelectorAll("li");
            Assert.Equal("12:10 AM", liCollec[1].GetAttribute("data-value").Replace('\u202F', ' ').Trim());
        }
        [Fact(Timeout = 10000, DisplayName = "keyboard list selection and value updating")]
        public async Task KeyboardArrowNavigation()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>();
            var inputEle = dateInstance.Find("input");
            await dateInstance.Instance.ShowTimePopupAsync();
            var popupEle = dateInstance.Find(".e-popup");
            KeyboardEventArgs args = new KeyboardEventArgs() { Code = "ArrowDown", Key = "ArrowDown" };
            inputEle.KeyDown(args);
            await Task.Delay(300);
            popupEle = dateInstance.Find(".e-popup");
            var liCollection = popupEle.QuerySelectorAll("li.e-list-item");
            Assert.True(liCollection.Length > 0);
            inputEle.KeyDown(args);
            popupEle = dateInstance.Find(".e-popup");
            liCollection = popupEle.QuerySelectorAll("li.e-list-item");
            inputEle.KeyDown(args);
            popupEle = dateInstance.Find(".e-popup");
            liCollection = popupEle.QuerySelectorAll("li.e-list-item");
            await Task.Yield();
            Assert.Contains("e-list-item", liCollection[2].ClassName);
            args = new KeyboardEventArgs() { Code = "ArrowUp", Key = "ArrowUp" };
            inputEle.KeyDown(args);
            popupEle = dateInstance.Find(".e-popup");
            liCollection = popupEle.QuerySelectorAll("li.e-list-item");
            await Task.Yield();
            Assert.Contains("e-list-item", liCollection[1].ClassName);
            args = new KeyboardEventArgs() { Code = "Enter", Key = "Enter" };
            inputEle.KeyDown(args);
            await Task.Delay(200);
            inputEle = dateInstance.Find("input");
        }

        [Fact(Timeout = 10000)]
        public async Task CLockIconClick()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime>>();
            Assert.Equal(default(DateTime), dateInstance.Instance.Value);
            var containerEle = dateInstance.Find("input").ParentElement;
            var dateIcon = containerEle.QuerySelector(".e-clock");
            dateIcon.MouseDown();
            await Task.Delay(200);
            var popupEle = dateInstance.Find(".e-popup");
            Assert.Contains("e-popup", popupEle.ClassName);
        }

        //[Fact(Timeout = 10000)]
        //public async Task KeyboardTimePopupAction()
        //{
        //    var dateInstance = RenderComponent<SfDateTimePicker<DateTime>>();
        //    Assert.Equal(default(DateTime), dateInstance.Instance.Value);
        //    var containerEle = dateInstance.Find("input").ParentElement;
        //    await dateInstance.Instance.ShowDatePopupAsync();
        //    await Task.Delay(200);
        //    var args = new Syncfusion.Blazor.Calendars.Internal.KeyActions
        //    { 
        //        Action = "altDownArrow" ,

        //    };
        //    await dateInstance.Instance.InputKeyActionHandler(args , true);
        //    await Task.Delay(200);

        //}

        [Fact(Timeout = 10000)]
        public void DateTimePickerModel()
        {
            var model = new DateTimePickerModel<DateTime>
            {
                AllowEdit = false,
                CalendarMode = CalendarType.Gregorian,
                CssClass = "custom-css",
                DayHeaderFormat = DayHeaderFormats.Wide,
                Depth = CalendarView.Year,
                EnablePersistence = true,
                Disabled = true,
                FirstDayOfWeek = 1,
                FloatLabelType = FloatLabelType.Auto,
                Format = "dd/MM/yyyy HH:mm",
                HtmlAttributes = new { disabled = true },
                KeyConfigs = new { key1 = "value1" },
                Locale = "fr-FR",
                Max = new DateTime(2100, 12, 31),
                Min = new DateTime(1900, 1, 1),
                Placeholder = "Enter date",
                Readonly = true,
                ScrollTo = new DateTime(2024, 1, 1),
                ServerTimezoneOffset = -5.0,
                ShowClearButton = false,
                ShowTodayButton = false,
                Start = CalendarView.Decade,
                Step = 15,
                StrictMode = true,
                TimeFormat = "HH:mm:ss",
                Value = new DateTime(2024, 8, 11),
                WeekNumber = true,
                Width = "200px",
                ZIndex = 1500
            };

            Assert.False(model.AllowEdit);
            Assert.Equal(CalendarType.Gregorian, model.CalendarMode);
            Assert.Equal("custom-css", model.CssClass);
            Assert.Equal(DayHeaderFormats.Wide, model.DayHeaderFormat);
            Assert.Equal(CalendarView.Year, model.Depth);
            Assert.True(model.EnablePersistence);
            Assert.True(model.Disabled);
            Assert.Equal(1, model.FirstDayOfWeek);
            Assert.Equal(FloatLabelType.Auto, model.FloatLabelType);
            Assert.Equal("dd/MM/yyyy HH:mm", model.Format);
            Assert.NotNull(model.HtmlAttributes);
            Assert.NotNull(model.KeyConfigs);
            Assert.Equal("fr-FR", model.Locale);
            Assert.Equal(new DateTime(2100, 12, 31), model.Max);
            Assert.Equal(new DateTime(1900, 1, 1), model.Min);
            Assert.Equal("Enter date", model.Placeholder);
            Assert.True(model.Readonly);
            Assert.Equal(new DateTime(2024, 1, 1), model.ScrollTo);
            Assert.Equal(-5.0, model.ServerTimezoneOffset);
            Assert.False(model.ShowClearButton);
            Assert.False(model.ShowTodayButton);
            Assert.Equal(CalendarView.Decade, model.Start);
            Assert.Equal(15, model.Step);
            Assert.True(model.StrictMode);
            Assert.Equal("HH:mm:ss", model.TimeFormat);
            Assert.Equal(new DateTime(2024, 8, 11), model.Value);
            Assert.True(model.WeekNumber);
            Assert.Equal("200px", model.Width);
            Assert.Equal(1500, model.ZIndex);

        }

        [Fact(Timeout = 10000)]
        public void MaskPlaceholderClass()
        {
            var component = RenderComponent<MaskPlaceholder>(parameters => parameters
           .Add(p => p.Day, "1")
           .Add(p => p.Month, "1")
           .Add(p => p.Year, "2020")
           .Add(p => p.Hour, "00")
           .Add(p => p.Minute, "00")
           .Add(p => p.Second, "00")
           .Add(p => p.DayOfWeek, "1")
         );
            Assert.Equal("1", component.Instance.Day);
            Assert.Equal("1", component.Instance.Month);
            Assert.Equal("2020", component.Instance.Year);
            Assert.Equal("00", component.Instance.Hour);
            Assert.Equal("00", component.Instance.Minute);
            Assert.Equal("00", component.Instance.Second);
            Assert.Equal("1", component.Instance.DayOfWeek);
        }

        [Fact(Timeout = 10000, DisplayName = "BLAZ-21402 - DateTimeKind is set to Unspecified in DateTimePicker’s value")]
        public async Task DatTimeKindIssue()
        {
            var Val = new DateTime(2022, 3, 1, 10, 0, 0, DateTimeKind.Local);
            var Val1 = new DateTime(2022, 3, 9, 10, 0, 0);
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(param => param.Add(p => p.Value, Val));
            var inputEle = dateInstance.Find("input");
            Assert.Equal(dateInstance.Instance.Value, Val);
            Assert.Equal(dateInstance.Instance.Value.Value.Kind.ToString(), "Local");
            dateInstance.Instance.ShowPopupAsync();
            var popupEle = dateInstance.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var tBody = tableElement.QuerySelector("tbody");
            var tRows = tBody.QuerySelectorAll("tr");
            var tCell = tRows[1].QuerySelectorAll("td");
            tCell[3].Click();
            inputEle = dateInstance.Find("input");
            await Task.Delay(100);
            Assert.Equal(dateInstance.Instance.Value, Val1);
            Assert.Equal(inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim(), "3/9/2022 10:00 AM");
            //Assert.Equal(dateInstance.Instance.Value.Value.Kind.ToString(), "Local");
            await dateInstance.Instance.HidePopupAsync();
            await dateInstance.Instance.ClosePopupAsync();
            await dateInstance.Instance.ShowTimePopupAsync();
            await Task.Delay(200);
            var popupEle1 = dateInstance.Find(".e-popup");
            var liCollec = popupEle.QuerySelectorAll("li");
            liCollec[2].Click();
            var inputEle1 = dateInstance.Find("input");
            Assert.Contains("3/9/2022 1:00", inputEle1.GetAttribute("value"));
            Assert.Equal(dateInstance.Instance.Value.Value.Kind.ToString(), "Local");
        }

        [Fact(Timeout = 10000, DisplayName = "BLAZ-21535 - While click the other months dates in the current month isInteracted returns false")]
        public async Task DateTimeIsInteracted()
        {
            int createCount = 0;
            int focusCount = 0;
            int openCount = 0;
            int changeCount = 0;
            DateTime? dateVal = new DateTime(2021, 3, 5);
            IRenderedComponent<SfDateTimePicker<DateTime?>> dateInstance = null;
            dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(param => param
            .Add(p => p.Value, dateVal)
            .Add(p => p.ValueChanged, (DateTime? value) => { dateVal = value; })
            .Add(p => p.ValueChange, (ChangedEventArgs<DateTime?> args) =>
            {
                changeCount++;
                dateVal = null;
                Assert.True(args.IsInteracted);
                dateInstance.SetParametersAndRender(("Value", null));
            }));
            var inputEle = dateInstance.Find("input");
            await dateInstance.Instance.ShowPopupAsync();
            var popupEle = dateInstance.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var tRows = tableElement.QuerySelectorAll("tr");
            tRows[1].QuerySelectorAll("td")[0].Click();
            await Task.Delay(100);
            Assert.Null(dateInstance.Instance.Value);
            inputEle = dateInstance.Find("input");
            Assert.Null(inputEle.GetAttribute("value"));
        }

        [Fact(Timeout = 10000, DisplayName = "Two-way binding and ValueChange on time item click")]
        public async Task TwoWayBinding_ValueChange_OnTimeItemClick()
        {
            ChangedEventArgs<DateTime?>? received = null;
            var bound = new DateTime?(new DateTime(2024, 1, 1, 9, 0, 0));
            var comp = RenderComponent<SfDateTimePicker<DateTime?>>(ps => ps
                .Add(p => p.Value, bound)
                .Add(p => p.Step, 30)
                .Add(p => p.TimeFormat, "hh:mm tt")
                .Add(p => p.ValueChange, args => { received = args; })
            );
            await comp.Instance.ShowTimePopupAsync();
            var popup = comp.Find(".e-popup-holder");
            var li = popup.QuerySelectorAll("li").ElementAt(1);
            var clickValue = li.GetAttribute("data-value");
            li.Click();
            var input = comp.Find("input");
            Assert.Contains(clickValue, input.GetAttribute("value").Replace('\u202F', ' ').Trim());
            Assert.NotNull(received);
            Assert.Equal(comp.Instance.Value, received!.Value);
        }

        [Fact(Timeout = 10000, DisplayName = "OnKeyDown passthrough is invoked")]
        public async Task OnKeyDown_IsInvoked()
        {
            JSInterop.SetupVoid("renderPopup", _ => true).SetVoidResult();
            JSInterop.SetupVoid("updateScrollPosition", _ => true).SetVoidResult();
            KeyboardEventArgs? got = null;
            var onKeyDown = EventCallback.Factory.Create<KeyboardEventArgs>(
                this,
                (KeyboardEventArgs e) => { got = e; }
            );
            var comp = RenderComponent<SfDateTimePicker<DateTime?>>(ps => ps
                .Add(p => p.OnKeyDown, onKeyDown)
                .Add(p => p.Step, 30)
            );
            await comp.Instance.ShowTimePopupAsync();
            comp.WaitForAssertion(() =>
            {
                Assert.True(comp.FindAll("li").Count > 0);
            });
            var input = comp.Find("input");
            input.KeyDown(new KeyboardEventArgs { Code = "ArrowDown", Key = "ArrowDown" });
            comp.WaitForAssertion(() =>
            {
                Assert.NotNull(got);
                Assert.Equal("ArrowDown", got!.Code);
            });
        }

        [Fact(Timeout = 10000, DisplayName = "Time list renders with Step and TimeFormat")]
        public async Task TimeList_Renders_With_Step_And_TimeFormat()
        {
            var baseDate = new DateTime(2024, 6, 1, 0, 0, 0);
            var comp = RenderComponent<SfDateTimePicker<DateTime>>(ps => ps
                .Add(p => p.Value, baseDate)
                .Add(p => p.Step, 15)
                .Add(p => p.TimeFormat, "HH:mm")
            );
            await comp.Instance.ShowTimePopupAsync();
            var list = comp.FindAll("li");
            Assert.True(list.Count >= 4);
            Assert.Contains("00:00", list.First().TextContent);
            Assert.Contains("00:15", list.ElementAt(1).TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "MinTime and MaxTime clamp time list")]
        public async Task TimeList_Clamped_By_MinTime_MaxTime()
        {
            var value = new DateTime(2024, 6, 1, 12, 0, 0);
            var minTime = new DateTime(2024, 6, 1, 9, 0, 0);
            var maxTime = new DateTime(2024, 6, 1, 18, 0, 0);

            var comp = RenderComponent<SfDateTimePicker<DateTime>>(ps => ps
                .Add(p => p.Value, value)
                .Add(p => p.Step, 60)
                .Add(p => p.TimeFormat, "HH:mm")
                .Add(p => p.MinTime, minTime)
                .Add(p => p.MaxTime, maxTime)
            );

            await comp.Instance.ShowTimePopupAsync();
            var items = comp.FindAll("li").ToList();
            Assert.StartsWith("09:00", items.First().TextContent);
            Assert.StartsWith("18:00", items.Last().TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "OpenOnFocus opens date popup")]
        public void OpenOnFocus_Opens_DatePopup()
        {
            var comp = RenderComponent<SfDateTimePicker<DateTime?>>(ps => ps
                .Add(p => p.OpenOnFocus, true)
            );
            var input = comp.Find("input");
            input.Focus();
            comp.WaitForAssertion(() =>
            {
                var popup = comp.FindAll(".e-popup-holder");
                Assert.True(popup.Count > 0);
            });
        }

        [Fact(Timeout = 10000, DisplayName = "Disabled prevents popup open and change")]
        public async Task Disabled_Blocks_Open_And_Change()
        {
            int changeCount = 0;
            var comp = RenderComponent<SfDateTimePicker<DateTime>>(ps => ps
                .Add(p => p.Disabled, true)
                .Add(p => p.ValueChange, _ => { changeCount++; })
            );
            await comp.Instance.ShowTimePopupAsync();
            var holders = comp.FindAll(".e-popup-holder");
            Assert.True(holders.Count == 0 || holders.All(h => string.IsNullOrWhiteSpace(h.TextContent)));
            var container = comp.Find("input").ParentElement;
            var timeIcon = container.QuerySelector(".e-clock");
            timeIcon.MouseDown();
            Assert.Equal(0, comp.FindAll(".e-popup-holder").Count);
            Assert.Equal(0, changeCount);
        }

        [Fact(Timeout = 10000, DisplayName = "Date icon click opens date popup")]
        public void DateIconClick_Opens_DatePopup()
        {
            var comp = RenderComponent<SfDateTimePicker<DateTime>>();
            var container = comp.Find("input").ParentElement;
            var dateIcon = container.QuerySelector(".e-timeline-today");
            dateIcon.MouseDown();
            comp.WaitForAssertion(() =>
            {
                var popup = comp.FindAll(".e-popup-holder");
                Assert.True(popup.Count > 0);
            });
        }

        [Fact(Timeout = 10000, DisplayName = "ARIA expanded and active-descendant are updated on open and keyboard navigation")]
        public async Task Aria_Expanded_And_ActiveDescendant()
        {
            var comp = RenderComponent<SfDateTimePicker<DateTime?>>(ps => ps
                .Add(p => p.Step, 30)
            );
            await comp.Instance.ShowTimePopupAsync();
            var input = comp.Find("input");
            Assert.Equal("true", input.GetAttribute("aria-expanded"));
            input.KeyDown(new KeyboardEventArgs { Code = "ArrowDown" });
            comp.WaitForAssertion(() =>
            {
                var attr = input.GetAttribute("aria-activedescendant");
                Assert.False(string.IsNullOrEmpty(attr));
            });
        }

        [Fact(Timeout = 10000, DisplayName = "ScrollTo value is passed to JS renderPopup")]
        public async Task ScrollTo_Passed_To_JSInterop()
        {
            var scrollTo = new DateTime(2024, 1, 1, 14, 30, 0);
            JSInterop.SetupVoid("renderPopup", args => true).SetVoidResult();
            var comp = RenderComponent<SfDateTimePicker<DateTime?>>(ps => ps
                .Add(p => p.Step, 30)
                .Add(p => p.ScrollTo, scrollTo)
            );
            await comp.Instance.ShowTimePopupAsync();
            JSInterop.VerifyInvoke("renderPopup");
        }
    }
}
