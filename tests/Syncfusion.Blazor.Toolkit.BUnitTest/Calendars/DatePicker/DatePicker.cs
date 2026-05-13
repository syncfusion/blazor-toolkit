using Xunit;
using Bunit;
using Syncfusion.Blazor.Toolkit.Calendars;
using AngleSharp.Css.Dom;
using Syncfusion.Blazor.Toolkit.Inputs;
using Microsoft.AspNetCore.Components;
using System.Globalization;
using Syncfusion.Blazor.Toolkit.Calendars.Internal;

namespace Syncfusion.Blazor.Toolkit.Tests.Calendars.DatePicker
{
    public class DatePicker : BunitTestContext
    {
        const string FORMATFULLDATE = "dddd, MMMM d, yyyy";
        const string FORMATDATE = " d ";
        const string TITLE_SEPARATOR = " - ";
        const string FORMAT_YEAR = "yyyy";

        [Fact(Timeout = 10000, DisplayName = "DefaultInitialize - Renders input with default attributes and classes")]
        public async Task DefaultInitialize()
        {
            var dateInstance = RenderComponent<SfDatePicker<DateTime?>>();
            var inputElement = dateInstance.Find("input");
            Assert.Contains("e-datepicker", inputElement.ClassName);
            Assert.Contains("e-control-container", inputElement?.ParentElement?.ClassName);
            Assert.True(inputElement?.ParentElement?.ChildElementCount == 2);
            Assert.True(inputElement?.ParentElement?.HasChildNodes);
            Assert.True(inputElement?.ParentElement?.NodeName == "SPAN");
            Assert.Equal("0", inputElement.GetAttribute("tabindex"));
            Assert.Null(inputElement.GetAttribute("placeholder"));
            Assert.Equal("false", inputElement.GetAttribute("aria-expanded"));
        }

        [Fact(Timeout = 10000, DisplayName = "DefaultValue - Component default property values")]
        public async Task DefaultValue()
        {
            var dateInstance = RenderComponent<SfDatePicker<DateTime?>>();
            Assert.Null(dateInstance.Instance.Value);
            Assert.Null(dateInstance.Instance.Placeholder);
            Assert.Null(dateInstance.Instance.Width);
            Assert.Equal(1000, dateInstance.Instance.ZIndex);
            Assert.Equal(new DateTime(1900, 01, 01), dateInstance.Instance.Min);
            Assert.Equal(new DateTime(2099, 12, 31), dateInstance.Instance.Max);
            Assert.True(dateInstance.Instance.AllowEdit);
        }

        [Fact(Timeout = 10000, DisplayName = "TabIndex - Renders and updates tabindex parameter")]
        public async Task TabIndex()
        {
            var dateInstance = RenderComponent<SfDatePicker<DateTime?>>(param => param.Add(p => p.TabIndex, 1));
            var inputElement = dateInstance.Find("input");
            Assert.Equal("1", inputElement.GetAttribute("tabindex"));
            dateInstance.SetParametersAndRender(("TabIndex", 3));
            inputElement = dateInstance.Find("input");
            Assert.Equal("3", inputElement.GetAttribute("tabindex"));
        }

        [Fact(Timeout = 10000, DisplayName = "CssClass - Applies CssClass to container and popup")]
        public async Task CssClass()
        {
            var dateInstance = RenderComponent<SfDatePicker<DateTime?>>(parameters => parameters.Add(p => p.CssClass, "sample-css"));
            await dateInstance.Instance.ShowPopupAsync();
            var containerElement = dateInstance.Find("input").ParentElement;
            var popupElement = dateInstance.Find(".e-popup");
            Assert.Contains("sample-css", containerElement?.ClassName);
            Assert.Contains("sample-css", popupElement.ClassName);
            await dateInstance.Instance.HidePopupAsync();
            dateInstance.SetParametersAndRender(("CssClass", "test highlight"));
            await dateInstance.Instance.ShowPopupAsync();
            containerElement = dateInstance.Find("input").ParentElement;
            popupElement = dateInstance.Find(".e-popup");
            Assert.Contains("test highlight", containerElement?.ClassName);
            Assert.Contains("test highlight", popupElement.ClassName);
            Assert.DoesNotContain("sample-css", containerElement?.ClassName);
            Assert.DoesNotContain("sample-css", popupElement.ClassName);
            await dateInstance.Instance.HidePopupAsync();
        }

        [Fact(Timeout = 10000, DisplayName = "ReadOnly - Prevents popup when readonly and opens when not")]
        public async Task ReadOnly()
        {
            var dateInstance = RenderComponent<SfDatePicker<DateTime?>>(parameters => parameters.Add(p => p.Readonly, true));
            var containerElement = dateInstance.Find("input").ParentElement;
            var iconElement = containerElement?.QuerySelector(".e-timeline-today");
            iconElement?.MouseDown();
            await Task.Delay(70);
            var readonlyPopupEle = dateInstance.FindAll(".e-popup");
            Assert.Equal(0, readonlyPopupEle.Count);
            dateInstance.SetParametersAndRender(("Readonly", false));
            containerElement = dateInstance.Find("input").ParentElement;
            iconElement = containerElement?.QuerySelector(".e-timeline-today");
            iconElement?.MouseDown();
            await Task.Delay(200);
            var popupElement = dateInstance.Find(".e-popup");
            Assert.NotNull(popupElement);
            await dateInstance.Instance.HidePopupAsync();
        }

        [Fact(Timeout = 10000, DisplayName = "WidthChange - Applies Width style to container")]
        public async Task WidthChange()
        {
            var dateInstance = RenderComponent<SfDatePicker<DateTime?>>(parameters => parameters.Add(p => p.Width, "300px"));
            var containerElement = dateInstance.Find("input").ParentElement;
            Assert.Contains("width: 300px", containerElement.GetStyle().CssText.Trim());
            dateInstance.SetParametersAndRender(("Width", "600px"));
            containerElement = dateInstance.Find("input").ParentElement;
            Assert.Contains("width: 600px", containerElement.GetStyle().CssText.Trim());
        }

        [Fact(Timeout = 10000, DisplayName = "Placeholder - Sets placeholder attribute on input")]
        public async Task Placeholder()
        {
            var dateInstance = RenderComponent<SfDatePicker<DateTime?>>(parameters =>
                    parameters.Add(p => p.Placeholder, "Enter the date value"));
            var inputElement = dateInstance.Find("input");
            Assert.Contains("Enter the date value", inputElement.GetAttribute("placeholder"));
            dateInstance.SetParametersAndRender(("Placeholder", "Enter the date"));
            Assert.Contains("Enter the date", inputElement.GetAttribute("placeholder"));
        }

        [Fact(Timeout = 10000, DisplayName = "Enabled - Toggles disabled state and aria attributes")]
        public async Task Enabled()
        {
            var dateInstance = RenderComponent<SfDatePicker<DateTime?>>();
            dateInstance.SetParametersAndRender(("Disabled", true));
            var inputElement = dateInstance.Find("input");
            var containerElement = inputElement.ParentElement;
            Assert.Contains("e-disabled", containerElement?.ClassName);
            Assert.Contains("e-disabled", inputElement.ClassName);
            Assert.Equal("true", inputElement.GetAttribute("aria-disabled"));
            Assert.True(inputElement.HasAttribute("disabled"));
            dateInstance.SetParametersAndRender(("Disabled", false));
            inputElement = dateInstance.Find("input");
            containerElement = inputElement.ParentElement;
            Assert.DoesNotContain("e-disabled", containerElement?.ClassName);
            Assert.DoesNotContain("e-disabled", inputElement.ClassName);
            Assert.Equal("false", inputElement.GetAttribute("aria-disabled"));
            Assert.False(inputElement.HasAttribute("disabled"));
        }

        [Fact(Timeout = 10000, DisplayName = "HtmlAttributes - Applies html attributes to rendered elements")]
        public async Task HtmlAttributes()
        {
            var dropdownlist = RenderComponent<SfDatePicker<DateTime?>>();
            dropdownlist.SetParametersAndRender(("HtmlAttributes", new Dictionary<string, object>() { { "name", "datepicker" }, { "required", "true" }, { "class", "e-date-calendar" } }));
            var inputElement = dropdownlist.Find("input");
            var containerElement = inputElement.ParentElement;
            Assert.Contains("datepicker", inputElement.GetAttribute("name"));
            Assert.Contains("true", inputElement.GetAttribute("required"));
            Assert.Contains("e-date-calendar", containerElement?.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "DateCalendarElement - Calendar popup structure and buttons")]
        public async Task DateCalendarElement()
        {
            var dateInstance = RenderComponent<SfDatePicker<DateTime?>>();
            var containerElement = dateInstance.Find("input").ParentElement;
            await dateInstance.Instance.ShowPopupAsync();
            var popupElement = dateInstance.Find(".e-popup");
            Assert.Contains("e-popup", popupElement.ClassName);
            var tableElement = popupElement.QuerySelector("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Contains("e-calendar", parentContainer?.ClassName);
            Assert.Contains("e-header", parentContainer?.Children[0].ClassName);
            Assert.Contains("e-month", parentContainer?.Children[0].ClassName);
            Assert.Contains("e-content", parentContainer?.Children[1].ClassName);
            Assert.Contains("e-month", parentContainer?.Children[1].ClassName);
            Assert.Contains("e-footer-container", parentContainer?.Children[2].ClassName);
            var buttonList = popupElement.QuerySelectorAll("button");
            var previousButton = buttonList[0];
            var nextButton = buttonList[1];
            var todayButton = buttonList[2];
            Assert.Contains("e-prev", previousButton.ClassName);
            Assert.Contains("e-next", nextButton.ClassName);
            Assert.Contains("e-today", todayButton.ClassName);
            Assert.Equal("Today", todayButton.TextContent);
            var tableBody = tableElement?.QuerySelector("tbody");
            var tableRows = tableBody?.QuerySelectorAll("tr");
            Assert.Equal(6, tableRows?.Length);
            var tCell = tableRows?[0].QuerySelectorAll("td");
            Assert.Equal(7, tCell?.Length);
        }

        [Fact(Timeout = 10000, DisplayName = "ClearButton - Shows and handles clear button behavior")]
        public async Task ClearButton()
        {
            var dateInstance = RenderComponent<SfDatePicker<DateTime?>>();
            var containerElement = dateInstance.Find("input").ParentElement;
            dateInstance.SetParametersAndRender(("ShowClearButton", true));
            containerElement = dateInstance.Find("input").ParentElement;
            var clearElement = containerElement?.Children[1];
            Assert.Contains("e-clear-icon", clearElement?.ClassName);
            Assert.Contains("e-clear-icon-hide", clearElement?.ClassName);
            Assert.True(containerElement?.Children[2]?.ClassName?.Contains("e-timeline-today"));
            await dateInstance.Instance.ShowPopupAsync();
            var popupElement = dateInstance.Find(".e-popup");
            Assert.Contains("e-popup", popupElement.ClassName);
            var tableElement = popupElement.QuerySelector("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Contains("e-calendar", parentContainer?.ClassName);
            Assert.Contains("e-header", parentContainer?.Children[0].ClassName);
            Assert.Contains("e-month", parentContainer?.Children[0].ClassName);
            Assert.Contains("e-content", parentContainer?.Children[1].ClassName);
            Assert.Contains("e-month", parentContainer?.Children[1].ClassName);
            Assert.Contains("e-footer-container", parentContainer?.Children[2].ClassName);
            var buttonList = popupElement.QuerySelectorAll("button");
            var previousButton = buttonList[0];
            var nextButton = buttonList[1];
            var todayButton = buttonList[2];
            Assert.Contains("e-prev", previousButton.ClassName);
            Assert.Contains("e-next", nextButton.ClassName);
            Assert.Contains("e-today", todayButton.ClassName);
            Assert.Equal("Today", todayButton.TextContent);
            var tableBody = tableElement?.QuerySelector("tbody");
            var tableRows = tableBody?.QuerySelectorAll("tr");
            Assert.Equal(6, tableRows?.Length);
            var tCell = tableRows?[1].QuerySelectorAll("td");
            tCell?[3].Click();
            var inputElement = dateInstance.Find("input");
            await Task.Delay(100);
            Assert.NotNull(dateInstance.Instance.Value);
            containerElement = dateInstance.Find("input").ParentElement;
            clearElement = containerElement?.Children[1];
            clearElement?.MouseDown();
            await Task.Delay(200);
            inputElement = dateInstance.Find("input");
            Assert.Null(inputElement.GetAttribute("value"));
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

        private string GetDateFormat<T>(T date, string? format = null, string? culture = null)
        {
            try
            {
                var currentCulture = CultureInfo.CurrentCulture;
                IFormattable? dateValue = date as IFormattable;
                var dateCulture = dateValue?.ToString(format, currentCulture);
                dateCulture = GetNativeDigits(dateCulture!, currentCulture.NumberFormat.NativeDigits);
                return dateCulture;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private string GetDecadeTitle(DateTime localDate)
        {
            int localYear = localDate.Year;
            localYear = localYear < 10 ? 10 : localYear;
            int startYear = localYear - localYear % 10;
            int endYear = startYear + (10 - 1);
            string startHeaderYear = GetDateFormat(new DateTime(startYear, 1, 1), FORMAT_YEAR);
            string endHeaderYear = GetDateFormat(new DateTime(endYear, 1, 1), FORMAT_YEAR);
            return startHeaderYear + TITLE_SEPARATOR + endHeaderYear;
        }

        [Fact(Timeout = 10000, DisplayName = "DefaultDateTimeValue - Renders calendar with provided DateTime and header")]
        public async Task DefaultDateTimeValue()
        {
            var dateInstance = RenderComponent<SfDatePicker<DateTime>>(parameters => parameters.
            Add(Cal => Cal.Value, new DateTime(1900, 1, 1)));
            Assert.Equal(new DateTime(1900, 1, 1), dateInstance.Instance.Value);
            var containerElement = dateInstance.Find("input").ParentElement;
            var dateIcon = containerElement?.QuerySelector(".e-timeline-today");
            dateIcon?.MouseDown();
            await Task.Delay(200);
            var popupElement = dateInstance.Find(".e-popup");
            Assert.Contains("e-popup", popupElement.ClassName);
            var tableElement = popupElement.QuerySelector("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Contains("e-calendar", parentContainer?.ClassName);
            var fromtTitle = "MMMM yyyy";
            var title = GetDateFormat(dateInstance.Instance.Min, fromtTitle);
            var headerEle = popupElement.QuerySelector(".e-day.e-title");
            Assert.Equal(title, headerEle?.InnerHtml);
            var inputElement = dateInstance.Find("input");
            Assert.Equal("1/1/1900", inputElement.GetAttribute("value"));
        }

        [Fact(Timeout = 10000, DisplayName = "PredefinedValue - Selects and updates predefined non-nullable value")]
        public async Task PredefinedValue()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime>>(parameters =>
                parameters.Add(val => val.Value, new DateTime(2020, 1, 1)));
            Assert.Equal(new DateTime(2020, 1, 1), calendar.Instance.Value);
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            var selectedCell = tableElement?.QuerySelectorAll(".e-selected");
            Assert.Equal(1, selectedCell?.Length);
            Assert.Equal("1", selectedCell?[0].FirstElementChild?.TextContent);
            Assert.Equal(GetDateFormat(new DateTime(2020, 1, 1), "dddd, MMMM d, yyyy"), selectedCell?[0].FirstElementChild?.GetAttribute("title"));
            await calendar.Instance.HidePopupAsync();
            await calendar.Instance.ClosePopupAsync();
            calendar.SetParametersAndRender(parameters => parameters.Add(p => p.Value, new DateTime(2020, 1, 2)));
            await calendar.Instance.ShowPopupAsync();
            popupElement = calendar.Find(".e-popup");
            tableElement = popupElement.QuerySelector("table");
            selectedCell = tableElement?.QuerySelectorAll(".e-selected");
            tableElement = calendar.Find("table");
            selectedCell = tableElement.QuerySelectorAll(".e-selected");
            Assert.Equal(1, selectedCell?.Length);
            Assert.Equal("2", selectedCell?[0].FirstElementChild?.TextContent);
            Assert.Equal(GetDateFormat(new DateTime(2020, 1, 2), "dddd, MMMM d, yyyy"), selectedCell?[0].FirstElementChild?.GetAttribute("title"));
        }

        [Fact(Timeout = 10000, DisplayName = "PredefinedNullableValue - Selects and updates predefined nullable value")]
        public async Task PredefinedNullableValue()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime?>>(parameters =>
                parameters.Add(val => val.Value, new DateTime(2020, 1, 1))
            );
            Assert.Equal(new DateTime(2020, 1, 1), calendar.Instance.Value);
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            var selectedCell = tableElement?.QuerySelectorAll(".e-selected");
            Assert.Equal(1, selectedCell?.Length);
            Assert.Equal("1", selectedCell?[0].FirstElementChild?.TextContent);
            Assert.Equal(GetDateFormat(new DateTime(2020, 1, 1), "dddd, MMMM d, yyyy"), selectedCell?[0].FirstElementChild?.GetAttribute("title"));
            await calendar.Instance.HidePopupAsync();
            await calendar.Instance.ClosePopupAsync();
            calendar.SetParametersAndRender(parameters => parameters.Add(p => p.Value, new DateTime(2020, 1, 2)));
            await calendar.Instance.ShowPopupAsync();
            popupElement = calendar.Find(".e-popup");
            tableElement = popupElement.QuerySelector("table");
            selectedCell = tableElement?.QuerySelectorAll(".e-selected");
            tableElement = calendar.Find("table");
            selectedCell = tableElement.QuerySelectorAll(".e-selected");
            Assert.Equal(1, selectedCell?.Length);
            Assert.Equal("2", selectedCell?[0].FirstElementChild?.TextContent);
            Assert.Equal(GetDateFormat(new DateTime(2020, 1, 2), "dddd, MMMM d, yyyy"), selectedCell?[0].FirstElementChild?.GetAttribute("title"));
        }

        [Fact(Timeout = 10000, DisplayName = "DayHeaderFormat - Renders day headers using configured DayHeaderFormat")]
        public async Task DayHeaderFormat()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime>>(parameters =>
            parameters.Add(calendar => calendar.DayHeaderFormat, DayHeaderFormats.Abbreviated));
            var containerElement = calendar.Find("input").ParentElement;
            var iconElement = containerElement?.QuerySelector(".e-timeline-today");
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            var headerElement = tableElement?.QuerySelector(".e-week-header");
            Assert.Equal("Sun", headerElement?.QuerySelector("th")?.TextContent);
            calendar.SetParametersAndRender(("DayHeaderFormat", DayHeaderFormats.Narrow));
            tableElement = calendar.Find("table");
            headerElement = tableElement.QuerySelector(".e-week-header");
            Assert.Equal("S", headerElement?.QuerySelector("th")?.TextContent);
            calendar.SetParametersAndRender(("DayHeaderFormat", DayHeaderFormats.Short));
            await calendar.Instance.ShowPopupAsync();
            tableElement = calendar.Find("table");
            headerElement = tableElement.QuerySelector(".e-week-header");
            Assert.Equal("S", headerElement?.QuerySelector("th")?.TextContent);
            calendar.SetParametersAndRender(("DayHeaderFormat", DayHeaderFormats.Wide));
            await calendar.Instance.ShowPopupAsync();
            tableElement = calendar.Find("table");
            headerElement = tableElement.QuerySelector(".e-week-header");
            Assert.Equal("Sunday", headerElement?.QuerySelector("th")?.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "FirstDayOfAWeek - Updates header when FirstDayOfWeek parameter changes")]
        public async Task FirstDayOfAWeek()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime>>();
            var containerElement = calendar.Find("input").ParentElement;
            var iconElement = containerElement?.QuerySelector(".e-timeline-today");
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            var headerElement = tableElement?.QuerySelector(".e-week-header");
            Assert.Equal("S", headerElement?.QuerySelector("th")?.TextContent);
            calendar.SetParametersAndRender(("FirstDayOfWeek", 2));
            tableElement = calendar.Find("table");
            headerElement = tableElement.QuerySelector(".e-week-header");
            Assert.Equal("T", headerElement?.QuerySelector("th")?.TextContent);
            calendar.SetParametersAndRender(("FirstDayOfWeek", 0));
            tableElement = calendar.Find("table");
            headerElement = tableElement.QuerySelector(".e-week-header");
            Assert.Equal("S", headerElement?.QuerySelector("th")?.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "MinValue - Enforces Min date and disables previous navigation")]
        public async Task MinValue()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime>>(parameters =>
            parameters.Add(calendar => calendar.Value, new DateTime(2020, 1, 1)));
            Assert.Equal(new DateTime(1900, 1, 1), calendar.Instance.Min);
            calendar.SetParametersAndRender(("Min", new DateTime(2020, 1, 1)));
            Assert.Equal(new DateTime(2020, 1, 1), calendar.Instance.Min);
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            var buttons = parentContainer?.QuerySelectorAll("button");
            var totalCells = tableElement?.QuerySelectorAll("td");
            var disabledCells = tableElement?.QuerySelectorAll("td.e-disabled");
            var enabledCells = tableElement?.QuerySelectorAll("td:not(.e-disabled)");
            Assert.Equal(3, disabledCells?.Length);
            Assert.Equal(totalCells?.Length, (disabledCells?.Length + enabledCells?.Length));
            Assert.Contains("e-prev", buttons?[0].ClassName);
            Assert.Contains("e-disabled", buttons?[0].ClassName);
            Assert.Contains("e-next", buttons?[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttons?[1].ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "NavigateCalendarToMonthviewWithMinValue - Navigation respects Min and view counts")]
        public async Task NavigateCalendarToMonthviewWithMinValue()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime?>>(parameters =>
            parameters.Add(Cal => Cal.Min, new DateTime(2020, 3, 1)));
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            Assert.Equal(new DateTime(2020, 3, 1), calendar.Instance.Min);
            Assert.Equal("Month", calendar.Instance.CurrentView());
            await calendar.Instance.NavigateAsync(CalendarView.Year, calendar.Instance.Min);
            Assert.Equal("Year", calendar.Instance.CurrentView());
            await calendar.Instance.HidePopupAsync();
            await calendar.Instance.ClosePopupAsync();
            await calendar.Instance.ShowPopupAsync();
            popupElement = calendar.Find(".e-popup");
            tableElement = popupElement.QuerySelector("table");
            Assert.Equal(3, tableElement?.QuerySelectorAll("tr").Length);
            Assert.Equal(12, tableElement?.QuerySelectorAll("td").Length);
        }

        [Fact(Timeout = 10000, DisplayName = "MaxValue - Enforces Max date and disables next navigation")]
        public async Task MaxValue()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime>>(parameters =>
            parameters.Add(calendar => calendar.Value, new DateTime(2020, 1, 1)));
            calendar.SetParametersAndRender(("Max", new DateTime(2020, 1, 1)));
            Assert.Equal(new DateTime(2020, 1, 1), calendar.Instance.Max);
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            var buttons = parentContainer?.QuerySelectorAll("button");
            var totalCells = tableElement?.QuerySelectorAll("td");
            var disabledCells = tableElement?.QuerySelectorAll("td.e-disabled");
            var enabledCells = tableElement?.QuerySelectorAll("td:not(.e-disabled)");
            Assert.Equal(38, disabledCells?.Length);
            Assert.Equal(totalCells?.Length, (disabledCells?.Length + enabledCells?.Length));
            Assert.Contains("e-prev", buttons?[0].ClassName);
            Assert.DoesNotContain("e-disabled", buttons?[0].ClassName);
            Assert.Contains("e-next", buttons?[1].ClassName);
            Assert.Contains("e-disabled", buttons?[1].ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "MaxValueWithNullable - Nullable Max disables dates and next navigation")]
        public async Task MaxValueWithNullable()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime?>>(parameters =>
            parameters.Add(calendar => calendar.Value, new DateTime(2020, 1, 1)));
            Assert.Equal(new DateTime(2099, 12, 31), calendar.Instance.Max);
            calendar.SetParametersAndRender(("Max", new DateTime(2020, 1, 1)));
            Assert.Equal(new DateTime(2020, 1, 1), calendar.Instance.Max);
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            var buttons = parentContainer?.QuerySelectorAll("button");
            var totalCells = tableElement?.QuerySelectorAll("td");
            var disabledCells = tableElement?.QuerySelectorAll("td.e-disabled");
            var enabledCells = tableElement?.QuerySelectorAll("td:not(.e-disabled)");
            Assert.Equal(38, disabledCells?.Length);
            Assert.Equal(totalCells?.Length, (disabledCells?.Length + enabledCells?.Length));
            Assert.Contains("e-prev", buttons?[0].ClassName);
            Assert.DoesNotContain("e-disabled", buttons?[0].ClassName);
            Assert.Contains("e-next", buttons?[1].ClassName);
            Assert.Contains("e-disabled", buttons?[1].ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "WeekNumber - Toggles week number column and renders week cells")]
        public async Task WeekNumber()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime>>();
            Assert.False(calendar.Instance.WeekNumber);
            calendar.SetParametersAndRender(("WeekNumber", true));
            Assert.True(calendar.Instance.WeekNumber);
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            var weekCells = tableElement?.QuerySelectorAll("td.e-week-number");
            Assert.InRange(weekCells != null ? weekCells.Length : 5, 5, 6);
        }

        [Fact(Timeout = 10000, DisplayName = "WeekNumberWithNullable - Week numbers render when enabled")]
        public async Task WeekNumberWithNullable()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime>>();
            Assert.False(calendar.Instance.WeekNumber);
            calendar.SetParametersAndRender(("WeekNumber", true));
            Assert.True(calendar.Instance.WeekNumber);
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            var weekCells = tableElement?.QuerySelectorAll("td.e-week-number");
            Assert.InRange(weekCells != null ? weekCells.Length : 5, 5, 6);
        }

        [Fact(Timeout = 10000, DisplayName = "TodayButton - Shows Today button and toggles its visibility")]
        public async Task TodayButton()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime>>();
            Assert.True(calendar.Instance.ShowTodayButton);
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            var buttons = parentContainer?.QuerySelectorAll("button");
            Assert.Equal(3, buttons?.Length);
            Assert.Contains("e-today", buttons?[2].ClassName);
            Assert.Contains("e-btn", buttons?[2].ClassName);
            Assert.Equal("Today", buttons?[2].TextContent);
            calendar.SetParametersAndRender(("ShowTodayButton", false));
            Assert.False(calendar.Instance.ShowTodayButton);
            tableElement = calendar.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            buttons = parentContainer?.QuerySelectorAll("button");
            Assert.Equal(2, buttons?.Length);
        }

        [Fact(Timeout = 10000, DisplayName = "WeekRules - Renders week numbers according to WeekRule and validates selected cell title")]
        public async Task WeekRules()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime?>>(parameters =>
            parameters.Add(Cal => Cal.WeekNumber, true)
            .Add(Cal => Cal.WeekRule, System.Globalization.CalendarWeekRule.FirstDay)
            .Add(Cal => Cal.Value, new DateTime(2021, 1, 1)));
            var containerElement = calendar.Find("input").ParentElement;
            var iconElement = containerElement?.QuerySelector(".e-timeline-today");
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            var tableHead = tableElement?.QuerySelector("thead.e-week-header");
            var headCell = tableHead?.QuerySelectorAll("th");
            Assert.Contains("e-week-number", headCell?[0].ClassName);
            Assert.Empty(headCell?[0].InnerHtml!);
            var tableBody = tableElement?.QuerySelector("tbody");
            var tableRows = tableBody?.QuerySelectorAll("tr");
            var tdCell = tableRows?[0].QuerySelectorAll("td");
            Assert.Contains("e-week-number", tdCell?[0].ClassName);
            Assert.Contains("e-cell", tdCell?[0].ClassName);
            Assert.Equal("1", tdCell?[0].QuerySelector("span")?.InnerHtml.Replace("\n", "").Trim());
            var selectedCell = tableBody?.QuerySelector("td.e-selected");
            var title = GetDateFormat(calendar.Instance.Value, FORMATFULLDATE);
            Assert.Equal(title, selectedCell?.QuerySelector("span")?.GetAttribute("title"));
            Assert.Equal("e-day", selectedCell?.QuerySelector("span")?.ClassName);
            var dayVal = GetDateFormat(calendar.Instance.Value, FORMATDATE);
            Assert.Equal(new DateTime(2021, 1, 1).Day.ToString(), selectedCell?.QuerySelector("span")?.InnerHtml.Replace("\n", "").Trim());
        }

        [Fact(Timeout = 10000, DisplayName = "StartAndDepthAsMonth - Start and Depth set to Month render month grid and focus")]
        public async Task StartAndDepthAsMonth()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime>>(parameters =>
            parameters.Add(Cal => Cal.Start, CalendarView.Month)
            .Add(Cal => Cal.Depth, CalendarView.Month).Add(Cal => Cal.Value, new DateTime(1900, 1, 1)));
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(CalendarView.Month, calendar.Instance.Start);
            Assert.Equal(CalendarView.Month, calendar.Instance.Depth);
            Assert.Equal("Month", calendar.Instance.CurrentView());
            Assert.Contains("January", parentContainer?.QuerySelector(".e-title")?.TextContent);
            Assert.Equal(42, tableElement?.QuerySelectorAll("td").Length);
            Assert.Equal(1, tableElement?.QuerySelectorAll("td.e-focused-date").Length);
            Assert.Equal("1", tableElement?.QuerySelectorAll("td.e-focused-date")[0]?.FirstElementChild?.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "StartAndDepthAsMonthWithNullable - Month view with nullable value focuses today")]
        public async Task StartAndDepthAsMonthWithNullable()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime?>>(parameters =>
                parameters.Add(Cal => Cal.Start, CalendarView.Month).Add(Cal => Cal.Depth, CalendarView.Month));
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(CalendarView.Month, calendar.Instance.Start);
            Assert.Equal(CalendarView.Month, calendar.Instance.Depth);
            Assert.Equal("Month", calendar.Instance.CurrentView());
            Assert.Contains(DateTime.Now.ToString("MMMM"), parentContainer?.QuerySelector(".e-title")?.TextContent);
            Assert.Equal(42, tableElement?.QuerySelectorAll("td").Length);
            Assert.Equal(1, tableElement?.QuerySelectorAll("td.e-focused-date").Length);
            Assert.Equal(DateTime.Now.Day.ToString(), tableElement?.QuerySelectorAll("td.e-focused-date")?[0].FirstElementChild?.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "StartAndDepthAsYear - Start and Depth Year render months and selection")]
        public async Task StartAndDepthAsYear()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime>>(parameters =>
                parameters.Add(Cal => Cal.Start, CalendarView.Year).Add(Cal => Cal.Depth, CalendarView.Year).Add(Cal => Cal.Value, new DateTime(1900, 1, 1)));
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(CalendarView.Year, calendar.Instance.Start);
            Assert.Equal(CalendarView.Year, calendar.Instance.Depth);
            Assert.Equal("Year", calendar.Instance.CurrentView());
            Assert.Contains("1900", parentContainer?.QuerySelector(".e-title")?.TextContent);
            Assert.Equal(12, tableElement?.QuerySelectorAll("td").Length);
            Assert.Equal(1, tableElement?.QuerySelectorAll("td.e-selected").Length);
            Assert.Equal("Jan", tableElement?.QuerySelectorAll("td.e-selected")?[0].FirstElementChild?.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "StartAndDepthAsYearWithNullable - Year view with nullable focuses current month")]
        public async Task StartAndDepthAsYearWithNullable()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime?>>(parameters =>
                parameters.Add(Cal => Cal.Start, CalendarView.Year).Add(Cal => Cal.Depth, CalendarView.Year));
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(CalendarView.Year, calendar.Instance.Start);
            Assert.Equal(CalendarView.Year, calendar.Instance.Depth);
            Assert.Equal("Year", calendar.Instance.CurrentView());
            Assert.Contains(DateTime.Now.ToString("yyyy"), parentContainer?.QuerySelector(".e-title")?.TextContent);
            Assert.Equal(12, tableElement?.QuerySelectorAll("td").Length);
            Assert.Equal(1, tableElement?.QuerySelectorAll("td.e-focused-date").Length);
            Assert.Equal(DateTime.Now.ToString("MMM"), tableElement?.QuerySelectorAll("td.e-focused-date")[0]?.FirstElementChild?.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "StartAndDepthAsDecade - Decade view renders decade ranges and selection")]
        public async Task StartAndDepthAsDecade()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime>>(parameters =>
                parameters.Add(Cal => Cal.Start, CalendarView.Decade).Add(Cal => Cal.Depth, CalendarView.Decade)
                    .Add(p => p.Value, new DateTime(1900, 1, 1)));
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(CalendarView.Decade, calendar.Instance.Start);
            Assert.Equal(CalendarView.Decade, calendar.Instance.Depth);
            Assert.Equal("Decade", calendar.Instance.CurrentView());
            Assert.Contains("1900 - 1909", parentContainer?.QuerySelector(".e-title")?.TextContent);
            Assert.Equal(12, tableElement?.QuerySelectorAll("td").Length);
            Assert.Equal(1, tableElement?.QuerySelectorAll("td.e-selected").Length);
            Assert.Equal("1900", tableElement?.QuerySelectorAll("td.e-selected")?[0].FirstElementChild?.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "StartAndDepthAsDecadeWithNullable - Decade view with nullable focuses current year")]
        public async Task StartAndDepthAsDecadeWithNullable()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime?>>(parameters =>
                parameters.Add(Cal => Cal.Start, CalendarView.Decade).Add(Cal => Cal.Depth, CalendarView.Decade));
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(CalendarView.Decade, calendar.Instance.Start);
            Assert.Equal(CalendarView.Decade, calendar.Instance.Depth);
            Assert.Equal("Decade", calendar.Instance.CurrentView());
            Assert.Contains(this.GetDecadeTitle(DateTime.Now), parentContainer?.QuerySelector(".e-title")?.TextContent);
            Assert.Equal(12, tableElement?.QuerySelectorAll("td").Length);
            Assert.Equal(1, tableElement?.QuerySelectorAll("td.e-focused-date").Length);
            Assert.Equal(DateTime.Now.Year.ToString(), tableElement?.QuerySelectorAll("td.e-focused-date")[0]?.FirstElementChild?.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "SelectDays - Selects a day in month view and binds value")]
        public async Task SelectDays()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime>>(parameters =>
                parameters.Add(calendar => calendar.Value, new DateTime(1900, 1, 1)));
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            var tableRows = tableElement?.QuerySelectorAll("tr");
            tableRows?[2].QuerySelectorAll("td")[2].Click();
            Assert.Equal(1900, calendar.Instance.Value.Year);
            var inputElement = calendar.Find("input");
            Assert.Contains("1900", inputElement.GetAttribute("value"));
        }

        [Fact(Timeout = 10000, DisplayName = "SelectDaysWithNullable - Selects a day when value is nullable")]
        public async Task SelectDaysWithNullable()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime?>>();
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            var tableRows = tableElement?.QuerySelectorAll("tr");
            tableRows?[2].QuerySelectorAll("td")[2].Click();
            Assert.Equal(DateTime.Now.Year, calendar.Instance.Value?.Year);
        }

        [Fact(Timeout = 10000, DisplayName = "SelectDecade - Selects a decade cell and updates value")]
        public async Task SelectDecade()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime>>(param => param.Add(p => p.Start, CalendarView.Decade).Add(p => p.Depth, CalendarView.Decade)
                .Add(p => p.Value, new DateTime(1900, 1, 1)));
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            var tableContent = popupElement.QuerySelector(".e-content");
            Assert.Contains("e-decade", tableContent?.ClassName);
            var tableRows = tableElement?.QuerySelectorAll("tr");
            Assert.Equal(3, tableRows?.Length);
            var header = popupElement.QuerySelector(".e-header.e-decade");
            var title = GetDecadeTitle(calendar.Instance.Min);
            Assert.Equal(title, header?.TextContent.Replace("\n", "").Trim());
            var tdCell = tableRows?[0].QuerySelectorAll("td");
            Assert.Equal(4, tdCell?.Length);
            tableRows?[1].QuerySelectorAll("td")[2].Click();
            Assert.Equal(1905, calendar.Instance.Value.Year);
        }

        [Fact(Timeout = 10000, DisplayName = "SelectDecadeWithNullable - Selects decade when value is nullable")]
        public async Task SelectDecadeWithNullable()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime?>>(param => param.Add(p => p.Start, CalendarView.Decade).Add(p => p.Depth, CalendarView.Decade));
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            var tableContent = popupElement.QuerySelector(".e-content");
            Assert.Contains("e-decade", tableContent?.ClassName);
            var tableRows = tableElement?.QuerySelectorAll("tr");
            Assert.Equal(3, tableRows?.Length);
            var header = popupElement.QuerySelector(".e-header.e-decade");
            var title = GetDecadeTitle(DateTime.Now.Date);
            Assert.Equal(title, header?.TextContent.Replace("\n", "").Trim());
            var tdCell = tableRows?[0].QuerySelectorAll("td");
            Assert.Equal(4, tdCell?.Length);
            tableRows?[1].QuerySelectorAll("td")[2].Click();
            Assert.Equal(tableRows?[1].QuerySelectorAll("td")[2].TextContent, calendar.Instance.Value?.Date.ToString("yyyy"));
        }

        [Fact(Timeout = 10000, DisplayName = "SelectYear - Selects month in year view and updates value")]
        public async Task SelectYear()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime>>(param => param.Add(p => p.Start, CalendarView.Year).Add(p => p.Depth, CalendarView.Year)
                .Add(p => p.Value, new DateTime(1900, 1, 1)));
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            var tableContent = popupElement.QuerySelector(".e-content");
            Assert.Contains("e-year", tableContent?.ClassName);
            var tableRows = tableElement?.QuerySelectorAll("tr");
            Assert.Equal(3, tableRows?.Length);
            var header = popupElement.QuerySelector(".e-header.e-year");
            Assert.Equal("1900", header?.TextContent.Replace("\n", "").Trim());
            var tdCell = tableRows?[0].QuerySelectorAll("td");
            Assert.Equal(4, tdCell?.Length);
            Assert.Equal("Jan", tableRows?[0].QuerySelector("td span")?.InnerHtml.Replace("\n", "").Trim());
            tableRows?[1].QuerySelectorAll("td")[2].Click();
            Assert.Equal(1900, calendar.Instance.Value.Year);
            Assert.Equal(7, calendar.Instance.Value.Month);
        }

        [Fact(Timeout = 10000, DisplayName = "SelectYearWithNullable - Selects month in year view with nullable value")]
        public async Task SelectYearWithNullable()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime?>>(param => param.Add(p => p.Start, CalendarView.Year).Add(p => p.Depth, CalendarView.Year));
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            var tableContent = popupElement.QuerySelector(".e-content");
            Assert.Contains("e-year", tableContent?.ClassName);
            var tableRows = tableElement?.QuerySelectorAll("tr");
            Assert.Equal(3, tableRows?.Length);
            var header = popupElement.QuerySelector(".e-header.e-year");
            Assert.Equal(DateTime.Now.Year.ToString(), header?.TextContent.Replace("\n", "").Trim());
            var tdCell = tableRows?[0].QuerySelectorAll("td");
            Assert.Equal(4, tdCell?.Length);
            Assert.Equal("Jan", tableRows?[0].QuerySelector("td span")?.InnerHtml.Replace("\n", "").Trim());
            tableRows?[1].QuerySelectorAll("td")[2].Click();
            Assert.Equal(DateTime.Now.Year, calendar.Instance.Value?.Year);
            Assert.Equal(7, calendar.Instance.Value?.Month);
        }

        [Fact(Timeout = 10000, DisplayName = "DynamicValueBinding - Updates UI when Value parameter changes at runtime")]
        public async Task DynamicValueBinding()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime?>>();
            Assert.Null(calendar.Instance.Value);
            var inputElement = calendar.Find("input");
            Assert.Null(inputElement.GetAttribute("value"));
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var fromtTitle = "MMMM yyyy";
            var title = GetDateFormat(DateTime.Now, fromtTitle);
            var headerEle = popupElement.QuerySelector(".e-day.e-title");
            Assert.Equal(title, headerEle?.InnerHtml.Replace("\n", "").Trim());
            await calendar.Instance.HidePopupAsync();
            await calendar.Instance.ClosePopupAsync();
            calendar.SetParametersAndRender(("Value", new DateTime(2021, 3, 5)));
            inputElement = calendar.Find("input");
            Assert.NotNull(inputElement.GetAttribute("value"));
            Assert.Contains("2021", inputElement.GetAttribute("value"));
            await calendar.Instance.ShowPopupAsync();
            popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            var selectedCell = tableElement?.QuerySelectorAll("td.e-selected");
            Assert.Equal(1, selectedCell?.Length);
            Assert.Equal("5", selectedCell?[0].FirstElementChild?.TextContent.Replace("\n", "").Trim());
            Assert.Equal(GetDateFormat(calendar.Instance.Value, "dddd, MMMM d, yyyy"), selectedCell?[0].FirstElementChild?.GetAttribute("title"));
        }

        [Fact(Timeout = 10000, DisplayName = "InputValueBinding - Binds input change to component value when user enters text")]
        public async Task InputValueBinding()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime?>>();
            Assert.Null(calendar.Instance.Value);
            var inputElement = calendar.Find("input");
            Assert.Null(inputElement.GetAttribute("value"));
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var fromtTitle = "MMMM yyyy";
            var title = GetDateFormat(DateTime.Now, fromtTitle);
            var headerEle = popupElement.QuerySelector(".e-day.e-title");
            Assert.Equal(title, headerEle?.InnerHtml.Replace("\n", "").Trim());
            await calendar.Instance.HidePopupAsync();
            await calendar.Instance.ClosePopupAsync();
            inputElement = calendar.Find("input");
            inputElement.SetAttribute("value", "05/03/2021");
            inputElement.Change(new ChangeEventArgs() { Value = "05/03/2021" });
            inputElement = calendar.Find("input");
            Assert.NotNull(inputElement.GetAttribute("value"));
            Assert.Contains("2021", inputElement.GetAttribute("value"));
        }

        [Fact(Timeout = 10000, DisplayName = "TodayButtonClick - Clicking Today selects today's date in Month view")]
        public async Task TodayButtonClick()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime>>();
            Assert.Equal(default(DateTime), calendar.Instance.Value);
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            Assert.Equal(0, tableElement?.QuerySelectorAll("td.e-selected").Length);
            Assert.Equal(1, tableElement?.QuerySelectorAll("td.e-focused-date").Length);
            var selectedDate = calendar.Find("table").QuerySelector("td.e-selected");
            Assert.Null(selectedDate);
            var buttonList = calendar.FindAll("button");
            buttonList[2].Click();
            tableElement = calendar.Find("table");
            Assert.Equal(1, tableElement.QuerySelectorAll("td.e-selected").Length);
            Assert.Equal(DateTime.Now.Day.ToString(), tableElement?.QuerySelector("td.e-selected")?.FirstElementChild?.TextContent);
            Assert.Equal(1, tableElement?.QuerySelectorAll("td.e-focused-date").Length);
            Assert.True(tableElement?.QuerySelector("td.e-selected")?.ClassList.Contains("e-today"));
        }

        [Fact(Timeout = 10000, DisplayName = "TodayButtonClickWithNullable - Today button selects date when Value is nullable")]
        public async Task TodayButtonClickWithNullable()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime?>>();
            Assert.Null(calendar.Instance.Value);
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            Assert.Equal(0, tableElement?.QuerySelectorAll("td.e-selected").Length);
            Assert.Equal(1, tableElement?.QuerySelectorAll("td.e-focused-date").Length);
            var selectedDate = calendar.Find("table").QuerySelector("td.e-selected");
            Assert.Null(selectedDate);
            var buttonList = calendar.FindAll("button");
            buttonList[2].Click();
            await calendar.Instance.ShowPopupAsync();
            popupElement = calendar.Find(".e-popup");
            tableElement = popupElement.QuerySelector("table");
            Assert.Equal(1, tableElement?.QuerySelectorAll("td.e-selected").Length);
            Assert.Equal(DateTime.Now.Day.ToString(), tableElement?.QuerySelector("td.e-selected")?.FirstElementChild?.TextContent);
            Assert.Equal(1, tableElement?.QuerySelectorAll("td.e-focused-date").Length);
            Assert.True(tableElement?.QuerySelector("td.e-selected")?.ClassList.Contains("e-today"));
        }

        [Fact(Timeout = 10000, DisplayName = "TodayButtonClickInOtherViews - Today button works in Year and Decade views and returns to Month view")]
        public async Task TodayButtonClickInOtherViews()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime>>();
            Assert.Equal(default(DateTime), calendar.Instance.Value);
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(0, tableElement?.QuerySelectorAll("td.e-selected").Length);
            Assert.Equal(1, tableElement?.QuerySelectorAll("td.e-focused-date").Length);
            var selectedDate = calendar.Find("table").QuerySelector("td.e-selected");
            Assert.Null(selectedDate);
            Assert.Equal("Month", calendar.Instance.CurrentView());
            // Today button click in Year view
            parentContainer?.QuerySelector(".e-title")?.Click();
            var tableContent = popupElement.QuerySelector(".e-content");
            Assert.Contains("e-year", tableContent?.ClassName);
            var buttonList = calendar.FindAll("button");
            buttonList[2].Click();
            await calendar.Instance.ShowPopupAsync();
            popupElement = calendar.Find(".e-popup");
            tableElement = popupElement.QuerySelector("table");
            Assert.Equal(1, tableElement?.QuerySelectorAll("td.e-selected").Length);
            Assert.Equal(DateTime.Now.Day.ToString(), tableElement?.QuerySelector("td.e-selected")?.FirstElementChild?.TextContent);
            Assert.Equal(1, tableElement?.QuerySelectorAll("td.e-focused-date").Length);
            Assert.True(tableElement?.QuerySelector("td.e-selected")?.ClassList.Contains("e-today"));
            Assert.Equal("Month", calendar.Instance.CurrentView());
            // Today button click in Decade View
            parentContainer = tableElement?.ParentElement?.ParentElement;
            parentContainer?.QuerySelector(".e-title")?.Click();
            tableContent = popupElement.QuerySelector(".e-content");
            Assert.Contains("e-year", tableContent?.ClassName);
            parentContainer = tableElement?.ParentElement?.ParentElement;
            parentContainer?.QuerySelector(".e-title")?.Click();
            tableContent = popupElement.QuerySelector(".e-content");
            Assert.Contains("e-decade", tableContent?.ClassName);
            buttonList[2].Click();
            tableElement = calendar.Find("table");
            tableContent = popupElement.QuerySelector(".e-content");
            Assert.Contains("e-month", tableContent?.ClassName);
            Assert.Equal(1, tableElement.QuerySelectorAll("td.e-selected").Length);
            Assert.Equal(DateTime.Now.Day.ToString(), tableElement?.QuerySelector("td.e-selected")?.FirstElementChild?.TextContent);
            Assert.Equal(1, tableElement?.QuerySelectorAll("td.e-focused-date").Length);
            Assert.True(tableElement?.QuerySelector("td.e-selected")?.ClassList.Contains("e-today"));
        }

        [Fact(Timeout = 10000, DisplayName = "TodayButtonClickWithStartAndDepthAsYear - Today selection in Year start/depth")]
        public async Task TodayButtonClickWithStartAndDepthAsYear()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime>>(parameters => parameters.
            Add(Cal => Cal.Start, CalendarView.Year).
            Add(Cal => Cal.Depth, CalendarView.Year));
            Assert.Equal(default(DateTime), calendar.Instance.Value);
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(0, tableElement?.QuerySelectorAll("td.e-selected").Length);
            var selectedDate = calendar.Find("table").QuerySelector("td.e-selected");
            Assert.Null(selectedDate);
            Assert.Equal("Year", calendar.Instance.CurrentView());
            var buttonList = calendar.FindAll("button");
            buttonList[2].Click();
            var tableContent = popupElement.QuerySelector(".e-content");
            Assert.Contains("e-year", tableContent?.ClassName);
            tableElement = popupElement.QuerySelector("table");
            Assert.Equal(1, tableElement?.QuerySelectorAll("td.e-selected").Length);
            Assert.Equal(DateTime.Now.ToString("MMM"), tableElement?.QuerySelector("td.e-selected")?.FirstElementChild?.TextContent);
            Assert.Equal(0, tableElement?.QuerySelectorAll("td.e-focused-date").Length);
        }

        [Fact(Timeout = 10000, DisplayName = "TodayButtonClickWithStartAndDepthAsDecade - Today selection in Decade start/depth")]
        public async Task TodayButtonClickWithStartAndDepthAsDecade()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime>>(parameters => parameters.
            Add(Cal => Cal.Start, CalendarView.Decade).
            Add(Cal => Cal.Depth, CalendarView.Decade));
            Assert.Equal(default(DateTime), calendar.Instance.Value);
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(0, tableElement?.QuerySelectorAll("td.e-selected").Length);
            var selectedDate = calendar.Find("table").QuerySelector("td.e-selected");
            Assert.Null(selectedDate);
            Assert.Equal("Decade", calendar.Instance.CurrentView());
            var buttonList = calendar.FindAll("button");
            buttonList[2].Click();
            var tableContent = popupElement.QuerySelector(".e-content");
            Assert.Contains("e-decade", tableContent?.ClassName);
            tableElement = popupElement.QuerySelector("table");
            Assert.Equal(1, tableElement?.QuerySelectorAll("td.e-selected").Length);
            Assert.Equal(DateTime.Now.ToString("yyyy"), tableElement?.QuerySelector("td.e-selected")?.FirstElementChild?.TextContent);
            Assert.Equal(0, tableElement?.QuerySelectorAll("td.e-focused-date").Length);
        }

        [Fact(Timeout = 10000, DisplayName = "AllowEdit - Toggles readonly attribute based on AllowEdit parameter")]
        public void AllowEdit()
        {
            var dateInstance = RenderComponent<SfDatePicker<DateTime?>>();
            var inputElement = dateInstance.Find("input");
            Assert.Null(inputElement.GetAttribute("readonly"));
            Assert.True(dateInstance.Instance.AllowEdit);
            dateInstance.SetParametersAndRender(("AllowEdit", false));
            inputElement = dateInstance.Find("input");
            Assert.NotNull(inputElement.GetAttribute("readonly"));
            Assert.False(dateInstance.Instance.AllowEdit);
        }

        [Fact(Timeout = 10000, DisplayName = "FloatingLabel - Renders float label states for different FloatLabelType values")]
        public void FloatingLabel()
        {
            var dateInstance = RenderComponent<SfDatePicker<DateTime?>>(parameter => parameter.Add(p => p.Placeholder, "Enter the value"));
            var containerElement = dateInstance.Find("input").ParentElement;
            Assert.DoesNotContain("e-float-text", containerElement?.ClassName);
            dateInstance.SetParametersAndRender(("FloatLabelType", FloatLabelType.Auto));
            containerElement = dateInstance.Find("input").ParentElement;
            Assert.Contains("e-float-input", containerElement?.ClassName);
            var floatEle = containerElement?.QuerySelector(".e-float-text");
            Assert.NotNull(floatEle);
            Assert.Contains("e-label-bottom", floatEle.ClassName);
            dateInstance.SetParametersAndRender(("Value", DateTime.Now));
            var inputElement = dateInstance.Find("input");
            containerElement = dateInstance.Find("input").ParentElement;
            floatEle = containerElement?.QuerySelector(".e-float-text");
            Assert.Contains("e-label-top", floatEle?.ClassName);
            dateInstance.SetParametersAndRender(("FloatLabelType", FloatLabelType.Always));
            containerElement = dateInstance.Find("input").ParentElement;
            Assert.Contains("e-float-input", containerElement?.ClassName);
            floatEle = containerElement?.QuerySelector(".e-float-text");
            Assert.NotNull(floatEle);
            Assert.Contains("e-label-top", floatEle.ClassName);
            dateInstance = RenderComponent<SfDatePicker<DateTime?>>(parameter => parameter.Add(p => p.Placeholder, "Enter the value").Add(p => p.FloatLabelType, FloatLabelType.Always));
            inputElement = dateInstance.Find("input");
            inputElement.Focus();
            containerElement = dateInstance.Find("input").ParentElement;
            floatEle = containerElement?.QuerySelector(".e-float-text");
            Assert.NotNull(floatEle);
            Assert.Contains("e-label-top", floatEle.ClassName);
            dateInstance = RenderComponent<SfDatePicker<DateTime?>>(parameter => parameter.Add(p => p.Placeholder, "Enter the value").Add(p => p.FloatLabelType, FloatLabelType.Never));
            containerElement = dateInstance.Find("input").ParentElement;
            Assert.DoesNotContain("e-float-input", containerElement?.ClassName);
            floatEle = containerElement?.QuerySelector(".e-float-text");
            Assert.Null(floatEle);
            dateInstance.SetParametersAndRender(("FloatLabelType", FloatLabelType.Auto));
            containerElement = dateInstance.Find("input").ParentElement;
            Assert.Contains("e-float-input", containerElement?.ClassName);
            floatEle = containerElement?.QuerySelector(".e-float-text");
            Assert.NotNull(floatEle);
            Assert.Contains("e-label-bottom", floatEle.ClassName);
            inputElement = dateInstance.Find("input");
            inputElement.Focus();
            containerElement = dateInstance.Find("input").ParentElement;
            floatEle = containerElement?.QuerySelector(".e-float-text");
            Assert.NotNull(floatEle);
            Assert.Contains("e-label-top", floatEle.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "DefaultFormat - Uses default format when none provided and selects correct date")]
        public async Task DefaultFormat()
        {
            var dateInstance = RenderComponent<SfDatePicker<DateTime?>>(param => param.Add(p => p.Value, new DateTime(2021, 3, 5)));
            await dateInstance.Instance.ShowPopupAsync();
            var popupElement = dateInstance.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            var tableRows = tableElement?.QuerySelectorAll("tr");
            tableRows?[2].QuerySelectorAll("td")[2].Click();
            Assert.Equal(new DateTime(2021, 3, 9), dateInstance.Instance.Value);
        }

        [Fact(Timeout = 10000, DisplayName = "InputAttribute - Applies InputAttributes to the input element")]
        public void InputAttribute()
        {
            var dateInstance = RenderComponent<SfDatePicker<DateTime?>>(param=>param.Add(p=>p.InputAttributes, new Dictionary<string, object>() { { "required", "true" }, { "name", "datepicker" }, { "class", "e-date-calendar"  } }));
            var inputElement = dateInstance.Find("input");
            var containerElement = inputElement.ParentElement;
            Assert.Contains("datepicker", inputElement.GetAttribute("name"));
            Assert.Contains("true", inputElement.GetAttribute("required"));
            Assert.Contains("e-date-calendar", inputElement.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "CurrentViewMethod - Navigates views by clicking title")]
        public async Task CurrentViewMethod()
        {
            var dateInstance = RenderComponent<SfDatePicker<DateTime>>();
            Assert.Equal("Month", dateInstance.Instance.CurrentView());
            await dateInstance.Instance.ShowPopupAsync();
            var popupElement = dateInstance.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            parentContainer?.QuerySelector(".e-title")?.Click();
            var tableContent = popupElement.QuerySelector(".e-content");
            Assert.Contains("e-year", tableContent?.ClassName);
            popupElement = dateInstance.Find(".e-popup");
            tableElement = popupElement.QuerySelector("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            parentContainer?.QuerySelector(".e-title")?.Click();
            tableContent = popupElement.QuerySelector(".e-content");
            Assert.Contains("e-decade", tableContent?.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "NavigateToMethod - Programmatically navigates between calendar views")]
        public async Task NavigateToMethod()
        {
            var dateInstance = RenderComponent<SfDatePicker<DateTime>>();
            await dateInstance.Instance.ShowPopupAsync();
            var popupElement = dateInstance.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
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

        [Fact(Timeout = 10000, DisplayName = "InputKeyActionHandler - Handles keyboard actions on input (alt/tab/select/escape)")]
        public async Task InputKeyActionHandler()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime>>(parameters =>
            parameters.Add(calendar => calendar.Value, new DateTime(2020, 1, 1)));
            var keyActionArgs = new Syncfusion.Blazor.Toolkit.Calendars.Internal.KeyActions
            {
                Action = "altUpArrow",
            };
            await calendar.Instance.InputKeyActionHandleAsync(keyActionArgs, null, true);
            await Task.Delay(200);
            keyActionArgs = new Syncfusion.Blazor.Toolkit.Calendars.Internal.KeyActions
            {
                Action = "altDownArrow",
            };
            await calendar.Instance.InputKeyActionHandleAsync(keyActionArgs, null, true);
            await Task.Delay(200);
            keyActionArgs = new Syncfusion.Blazor.Toolkit.Calendars.Internal.KeyActions
            {
                Action = "escape",
            };
            await calendar.Instance.InputKeyActionHandleAsync(keyActionArgs, null, true);
            await Task.Delay(200);
            keyActionArgs = new Syncfusion.Blazor.Toolkit.Calendars.Internal.KeyActions
            {
                Action = "tab",
            };
            await calendar.Instance.InputKeyActionHandleAsync(keyActionArgs, null, true);
            await Task.Delay(200);
            keyActionArgs = new Syncfusion.Blazor.Toolkit.Calendars.Internal.KeyActions
            {
                Action = "select",
            };
            await calendar.Instance.InputKeyActionHandleAsync(keyActionArgs, null, true);
            await Task.Delay(200);
            await calendar.Instance.HidePopupAsync();
            Assert.Equal(new DateTime(2020, 1, 1), calendar.Instance.Value);
        }

        [Fact(Timeout = 10000, DisplayName = "ChangeValue - Handles user change and select action to update value")]
        public async Task ChangeValue()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime>>(parameters =>
                parameters.Add(calendar => calendar.Value, new DateTime(2020, 1, 1)));
            var inputElement = calendar.Find("input");
            inputElement.Change(new DateTime(2020, 1, 2));
            var keyActionArgs = new Syncfusion.Blazor.Toolkit.Calendars.Internal.KeyActions
            {
                Action = "select",
            };
            await calendar.Instance.InputKeyActionHandleAsync(keyActionArgs, null, true);
            await Task.Delay(200);
            var containerElement = inputElement.ParentElement;
            Assert.Equal(new DateTime(0001, 1, 1), calendar.Instance.Value);
        }

        [Fact(Timeout = 10000, DisplayName = "ScrollTONext - Scrolls calendar to next section programmatically")]
        public async Task ScrollTONext()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime>>(parameters =>
            parameters.Add(calendar => calendar.Value, new DateTime(2020, 1, 1)));
            await calendar.Instance.ShowPopupAsync();
            calendar.Instance.ScrollToNextSection();
            calendar.Instance.ScrollToNextSection(true);
            Assert.Equal(new DateTime(2020, 1, 1), calendar.Instance.Value);
        }

        [Fact(Timeout = 10000, DisplayName = "UpdateFieldSetStatus - Updates fieldset status without changing value")]
        public async Task UpdateFieldSetStatus()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime>>(parameters =>
                parameters.Add(calendar => calendar.Value, new DateTime(2015, 3, 3)));
            await calendar.InvokeAsync(() => calendar.Instance.UpdateFieldSetStatus(false));
            Assert.Equal(new DateTime(2015, 3, 3), calendar.Instance.Value);
        }

        [Fact(Timeout = 10000, DisplayName = "InputKeyActionHandle - Handles Enter select action with provided input string")]
        public async Task InputKeyActionHandle()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime>>(parameters =>
                parameters.Add(calendar => calendar.Value, new DateTime(2012, 3, 3)));
            var keyActionArgs = new Syncfusion.Blazor.Toolkit.Calendars.Internal.KeyActions
            {
                Action = "select",
                Key = "Enter"
            };
            await calendar.Instance.InputKeyActionHandleAsync(keyActionArgs , "8/10/2024" , true);
            Assert.Equal(new DateTime(2012, 3, 3), calendar.Instance.Value);
        }

        [Fact(Timeout = 10000, DisplayName = "FocusOutHandler - Focus out behavior opens popup when icon clicked")]
        public async Task FocusOutHandler()
        {
            var dateInstance = RenderComponent<SfDatePicker<DateTime>>(parameters => parameters.
                Add(Cal => Cal.Value, new DateTime(1900, 1, 1)));
            Assert.Equal(new DateTime(1900, 1, 1), dateInstance.Instance.Value);
            var containerElement = dateInstance.Find("input").ParentElement;
            var dateIcon = containerElement?.QuerySelector(".e-timeline-today");
            dateIcon?.MouseDown();
            await Task.Delay(200);
            var popupElement = dateInstance.Find(".e-popup");
            Assert.Contains("e-popup", popupElement.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "InteParameters - Input key actions with EnableMask and custom format")]
        public async Task InteParameters()
        {
            var dateInstance = RenderComponent<SfDatePicker<DateTime>>(parameters => parameters
                .Add(Cal => Cal.Value, new DateTime(1900, 1, 1)).Add(cal => cal.EnableMask , true).Add(cal => cal.Format , "dddd, MMMM dd, yyyy"));
            var keyActionArgs = new Syncfusion.Blazor.Toolkit.Calendars.Internal.KeyActions{ Action = "altUpArrow", };
            await dateInstance.Instance.InputKeyActionHandleAsync(keyActionArgs, "8/10/2024", true);
        }

        [Fact(Timeout = 10000, DisplayName = "FocusOutAsync - Calls FocusOutAsync without throwing")]
        public async Task FocusOutAsync()
        {
            var dateInstance = RenderComponent<SfDatePicker<DateTime>>(parameters =>
                parameters.Add(calendar => calendar.Value, new DateTime(2012, 3, 3)));
            var containerElement = dateInstance.Find("input");
            await dateInstance.Instance.FocusOutAsync();
        }

        [Fact(Timeout = 10000, DisplayName = "GetPersistData - Gets persisted data when enabled")]
        public async Task GetPersistData()
        {
            var dateInstance = RenderComponent<SfDatePicker<DateTime>>(parameters => parameters.
                Add(Cal => Cal.Value, new DateTime(1900, 1, 1)).Add(Cal => Cal.FullScreen, false));
            Assert.Equal(new DateTime(1900, 1, 1), dateInstance.Instance.Value);
            var containerElement = dateInstance.Find("input").ParentElement;
            var dateIcon = containerElement?.QuerySelector(".e-timeline-today");
            dateIcon?.MouseDown();
            await Task.Delay(200);
            var popupElement = dateInstance.Find(".e-popup");
            Assert.False(dateInstance.Instance.FullScreen);
            await dateInstance.Instance.GetPersistDataAsync();
        }

        [Fact(Timeout = 10000, DisplayName = "InputHandler - Handles raw input and validation")]
        public async Task InputHandler()
        {
            var dateInstance = RenderComponent<SfDatePicker<DateTime>>(parameters => parameters.
                Add(Cal => Cal.Value, new DateTime(1900, 1, 1)));
            Assert.Equal(new DateTime(1900, 1, 1), dateInstance.Instance.Value);
            var containerElement = dateInstance.Find("input").ParentElement;
            var inputElement = dateInstance.Find("input");
            var changeArgs = new ChangeEventArgs { Value = "8/10/2024s" };
            inputElement.Input(changeArgs);
            await Task.Delay(200);
        }

        [Fact(Timeout = 10000, DisplayName = "KeyboardTimePopupAction - Keyboard actions while popup displayed")]
        public async Task KeyboardTimePopupAction()
        {

            var dateInstance = RenderComponent<SfDatePicker<DateTime>>(parameters => parameters.
                Add(Cal => Cal.Value, new DateTime(1900, 1, 1)));
            Assert.Equal(new DateTime(1900, 1, 1), dateInstance.Instance.Value);
            await dateInstance.Instance.ShowPopupAsync();
            var keyActionArgs = new Syncfusion.Blazor.Toolkit.Calendars.Internal.KeyActions
            {
                Action = "altDownArrow",
            };
            await dateInstance.Instance.InputKeyActionHandleAsync(keyActionArgs, null, true);
            await Task.Delay(200);
        }

        [Fact(Timeout = 10000, DisplayName = "MoveFocusToPopup - Moves focus into popup via keyboard action")]
        public async Task MoveFocusToPopup()
        {

            var dateInstance = RenderComponent<SfDatePicker<DateTime>>(parameters => parameters.
                Add(Cal => Cal.Value, new DateTime(1900, 1, 1)));
            Assert.Equal(new DateTime(1900, 1, 1), dateInstance.Instance.Value);
            await dateInstance.Instance.ShowPopupAsync();
            var keyActionArgs = new Syncfusion.Blazor.Toolkit.Calendars.Internal.KeyActions
            {
                Action = "tab",
                TargetClassList = "tab-event"
            };
            await dateInstance.Instance.InputKeyActionHandleAsync(keyActionArgs, null, true);
            await Task.Delay(200);
        }

        [Fact(Timeout = 10000, DisplayName = "FocusOut - Focus and then FocusOutAsync to ensure blur handling")]
        public async Task FocusOut()
        {
            var dateInstance = RenderComponent<SfDatePicker<DateTime>>(parameters =>
                parameters.Add(calendar => calendar.Value, new DateTime(2012, 3, 3)));
            var containerElement = dateInstance.Find("input");
            containerElement.Focus();
            await Task.Delay(200);
            await dateInstance.Instance.FocusOutAsync();
        }

        [Fact(Timeout = 10000, DisplayName = "DatePickerModelClass - Validates DatePickerModel property defaults and assignment")]
        public void DatePickerModelClass()
        {
            var model = new DatePickerModel
            {
                AllowEdit = false,
                CssClass = "custom-class",
                Disabled = true,
                FloatLabelType = FloatLabelType.Always,
                Format = "dd/MM/yyyy",
                HtmlAttributes = new Dictionary<string, object> { { "style", "color:red;" } },
                InputAttributes = new Dictionary<string, object> { { "disabled", "true" } },
                Placeholder = "Select date",
                Readonly = true,
                ShowClearButton = false,
                StrictMode = true,
                Width = "250px",
                ZIndex = 2000,
                TabIndex = 1
            };

            Assert.False(model.AllowEdit);
            Assert.Equal("custom-class", model.CssClass);
            Assert.True(model.Disabled);
            Assert.Equal(FloatLabelType.Always, model.FloatLabelType);
            Assert.Equal("dd/MM/yyyy", model.Format);
            Assert.NotNull(model.HtmlAttributes);
            Assert.Contains(model.HtmlAttributes, kvp => kvp.Key == "style" && kvp.Value.ToString() == "color:red;");
            Assert.NotNull(model.InputAttributes);
            Assert.Contains(model.InputAttributes, kvp => kvp.Key == "disabled" && kvp.Value.ToString() == "true");
            Assert.Equal("Select date", model.Placeholder);
            Assert.True(model.Readonly);
            Assert.False(model.ShowClearButton);
            Assert.True(model.StrictMode);
            Assert.Equal("250px", model.Width);
            Assert.Equal(2000, model.ZIndex);
            Assert.Equal(1, model.TabIndex);
        }

        [Fact(Timeout = 10000, DisplayName = "DatePickerMaskPlaceholder - Renders mask placeholder child content and properties")]
        public void DatePickerMaskPlaceholder()
        {
            var dateInstance = RenderComponent<SfDatePicker<DateTime>>(parameters =>parameters
            .Add(calendar => calendar.Value, new DateTime(2012, 3, 3))
            .Add(p => p.EnableMask , true)
            .AddChildContent<DatePickerMaskPlaceholder>(
                param => param
             .Add(p => p.Hour, "hour")
             .Add(p => p.Minute, "minute")
             .Add(p => p.Second, "second")
             ));
            Assert.Equal(1000, dateInstance.Instance.ZIndex);
        }

        [Fact(Timeout = 10000, DisplayName = "EnablePersistence_WritesPersistedValue_OnValueChange - Stores value to localStorage when persistence enabled")]
        public async void EnablePersistence_WritesPersistedValue_OnValueChange()
        {
            JSInterop.SetupVoid("window.localStorage.setItem").SetVoidResult();
            var datePicker = RenderComponent<SfDatePicker<DateTime>>(parameters => parameters.Add(p => p.EnablePersistence, true));
            await datePicker.Instance.ShowPopupAsync();
            var popup = datePicker.Find(".e-popup");
            var buttons = popup.QuerySelectorAll("button");
            Assert.True(buttons.Length >= 3, "expected today button in popup");
            buttons[2].Click();
            Assert.True(JSInterop.Invocations.Any(i => i.Identifier == "setLocalStorageItem"), "Expected localStorage.setItem to be invoked");
        }

        [Fact(Timeout = 10000, DisplayName = "ValueChange_And_ValueChanged_Behavior - ValueChange event invoked on UI selection and ValueChanged parameter updates")]
        public void ValueChange_And_ValueChanged_Behavior()
        {
            int valueChangeCount = 0;
            DateTime? lastValueFromEvent = null;
            var datePicker = RenderComponent<SfDatePicker<DateTime?>>(parameters => parameters
                .Add(p => p.ValueChange, (ChangedEventArgs<DateTime?> args) => { valueChangeCount++; lastValueFromEvent = args.Value; })
            );
            datePicker.SetParametersAndRender(p => p.Add(pp => pp.Value, new DateTime(2021, 1, 1)));
            Assert.Equal(0, valueChangeCount);
            datePicker.InvokeAsync(async () => await datePicker.Instance.ShowPopupAsync()).GetAwaiter().GetResult();
            var popup = datePicker.Find(".e-popup");
            var cells = popup.QuerySelectorAll("td").Where(td => !td.ClassList.Contains("e-disabled")).ToArray();
            var target = cells.FirstOrDefault();
            Assert.NotNull(target);
            target.Click();
            datePicker.WaitForAssertion(() => Assert.True(valueChangeCount >= 1 || lastValueFromEvent != null), TimeSpan.FromMilliseconds(500));
        }

        [Fact(Timeout = 10000, DisplayName = "BindingMaxValue - Handles DateTime.MaxValue binding for Value and Max without errors")]
        public async Task BindingMaxValue()
        {
            var component = RenderComponent<SfDatePicker<DateTime?>>(param => param.Add(p => p.Value, DateTime.MaxValue).Add(p=>p.Max, DateTime.MaxValue));
            var inputElement = component.Find("input");
            Assert.Equal("12/31/9999", inputElement.GetAttribute("value"));
            await component.Instance.ShowPopupAsync();
            var popupElement = component.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            var tableRows = tableElement?.QuerySelectorAll("tr");
            var selectedCell = tableElement?.QuerySelectorAll("td.e-selected");
            Assert.Equal(1, selectedCell?.Length);
            Assert.Equal("31", selectedCell?[0].FirstElementChild?.TextContent.Replace("\n", "").Trim());
        }

        [Fact(Timeout = 10000, DisplayName = "DateTimeIsInteracted - Ensures IsInteracted flag when selecting other month dates")]
        public async Task DateTimeIsInteracted()
        {
            int changeCount = 0;
            DateTime? dateValue = new DateTime(2021, 3, 5);
            IRenderedComponent<SfDatePicker<DateTime?>>? component = null;
            component = RenderComponent<SfDatePicker<DateTime?>>(param => param.Add(p => p.Value, dateValue).Add(p => p.ValueChanged, (DateTime? value) => { dateValue = value; })
            .Add(p => p.ValueChange, (ChangedEventArgs<DateTime?> args) => {
                changeCount++;
                dateValue = null;
                Assert.True(args.IsInteracted);
                component?.SetParametersAndRender(("Value", null));
            }));
            var inputElement = component.Find("input");
            await component.Instance.ShowPopupAsync();
            var popupElement = component.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            var tableRows = tableElement?.QuerySelectorAll("tr");
            tableRows?[1].QuerySelectorAll("td")[0].Click();
            await Task.Delay(100);
            Assert.Null(component.Instance.Value);
            inputElement = component.Find("input");
            Assert.Null(inputElement.GetAttribute("value"));
        }

        [Fact(Timeout = 10000, DisplayName = "RenderDayCellCustomization - DayCellRendering can add custom class to specific day cell")]
        public async Task RenderDayCellCustomization()
        {
            var targetDate = new DateTime(2020, 1, 1);
            var comp = RenderComponent<SfDatePicker<DateTime>>(parameters => parameters
                .Add(p => p.Value, targetDate)
                .Add(p => p.DayCellRendering, (RenderDayCellEventArgs args) =>
                {
                    if (args.Date.Date == targetDate.Date)
                    {
                        args.CellData.ClassList += " my-custom-day";
                    }
                }));

            await comp.Instance.ShowPopupAsync();
            var popup = comp.Find(".e-popup");
            var customCell = popup.QuerySelector(".my-custom-day");
            Assert.NotNull(customCell);
        }
    }
}
