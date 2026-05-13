using Xunit;
using Bunit;
using Syncfusion.Blazor.Toolkit.Calendars;
using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Tests.Calendars.DateTimePicker
{
    public class DateTimePickerMask : BunitTestContext
    {
        const string FORMATFULLDATE = "dddd, MMMM dd, yyyy";
        const string FORMATDATE = " d ";
        const string TITLE_SEPARATOR = " - ";
        const string FORMAT_YEAR = "yyyy";

        //[Fact(Timeout = 10000)]
        //public async Task ReadOnly()
        //{
        //    var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(parameters => parameters.Add(p => p.Readonly, true).Add(p => p.EnableMask, true));
        //    var containerEle = dateInstance.Find("input").ParentElement;
        //    var iconEle = containerEle.QuerySelector(".e-timeline-today");
        //    iconEle.MouseDown();
        //    await Task.Delay(70);
        //    var readonlyPopupEle = dateInstance.FindAll(".e-popup");
        //    Assert.Equal(0, readonlyPopupEle.Count);
        //    dateInstance.SetParametersAndRender(("Readonly", false));
        //    containerEle = dateInstance.Find("input").ParentElement;
        //    iconEle = containerEle.QuerySelector(".e-timeline-today");
        //    iconEle.MouseDown();
        //    await Task.Delay(200);
        //    var popupElement = dateInstance.Find(".e-popup");
        //    Assert.NotNull(popupElement);
        //    await dateInstance.Instance.HidePopupAsync();
        //}

        [Fact(Timeout = 10000)]
        public void Placeholder()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(parameters =>
                    parameters.Add(p => p.Placeholder, "Enter the date value").Add(p => p.EnableMask, true));
            var inputEle = dateInstance.Find("input");
            Assert.Contains("Enter the date value", inputEle.GetAttribute("placeholder"));
            dateInstance.SetParametersAndRender(("Placeholder", "Enter the date"));
            Assert.Contains("Enter the date", inputEle.GetAttribute("placeholder"));
        }

        [Fact(Timeout = 10000)]
        public async Task ClearButton()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(param => param.Add(p => p.EnableMask, true));
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

        [Fact(Timeout = 10000)]
        public async Task MinValue()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime>>(parameters =>
            parameters.Add(calendar => calendar.Value, new DateTime(2020, 1, 1)).Add(p => p.EnableMask, true));
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

        [Fact(Timeout = 10000)]

        public void MinvalueStrictModeTrue()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime>>(parameters =>
            parameters.Add(calendar => calendar.Value, new DateTime(2020, 1, 1)).Add(p => p.EnableMask, true).Add(p => p.StrictMode, true));
            Calendar.SetParametersAndRender(("Min", new DateTime(2020, 1, 1, 3, 35, 00)));
            Assert.Equal(new DateTime(2020, 1, 1, 3, 35, 00), Calendar.Instance.Min);
            Calendar.SetParametersAndRender(("Value", new DateTime(2019, 12, 31, 18, 30, 00)));
            Assert.Equal(new DateTime(2020, 1, 1, 3, 35, 00), Calendar.Instance.Value);
            var containerEle = Calendar.Find("input").ParentElement;
            Assert.DoesNotContain("e-error", containerEle.ClassName);
        }

        [Fact(Timeout = 10000)]

        public void MaxvalueStrictModeTrue()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime>>(parameters =>
            parameters.Add(calendar => calendar.Value, new DateTime(2020, 1, 1)).Add(p => p.EnableMask, true).Add(p => p.StrictMode, true));
            Calendar.SetParametersAndRender(("Max", new DateTime(2020, 1, 1, 3, 35, 00)));
            Assert.Equal(new DateTime(2020, 1, 1, 3, 35, 00), Calendar.Instance.Max);
            Calendar.SetParametersAndRender(("Value", new DateTime(2020, 12, 31, 18, 30, 00)));
            Assert.Equal(new DateTime(2020, 1, 1, 3, 35, 00), Calendar.Instance.Value);
            var containerEle = Calendar.Find("input").ParentElement;
            Assert.DoesNotContain("e-error", containerEle.ClassName);
        }

        [Fact(Timeout = 10000)]

        public void MinvalueStrictModeFalse()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime>>(parameters =>
            parameters.Add(calendar => calendar.Value, new DateTime(2020, 1, 1)).Add(p => p.EnableMask, true).Add(p => p.StrictMode, false));
            Calendar.SetParametersAndRender(("Min", new DateTime(2020, 1, 1, 3, 35, 00)));
            Assert.Equal(new DateTime(2020, 1, 1, 3, 35, 00), Calendar.Instance.Min);
            Calendar.SetParametersAndRender(("Value", new DateTime(2019, 12, 31, 18, 30, 00)));
            Assert.Equal(new DateTime(2019, 12, 31, 18, 30, 00), Calendar.Instance.Value);
            var containerEle = Calendar.Find("input").ParentElement;
            Assert.Contains("e-error", containerEle.ClassName);
        }

        [Fact(Timeout = 10000)]

        public void MaxvalueStrictModeFalse()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime>>(parameters =>
            parameters.Add(calendar => calendar.Value, new DateTime(2020, 1, 1)).Add(p => p.EnableMask, true).Add(p => p.StrictMode, false));
            Calendar.SetParametersAndRender(("Max", new DateTime(2020, 1, 1, 3, 35, 00)));
            Assert.Equal(new DateTime(2020, 1, 1, 3, 35, 00), Calendar.Instance.Max);
            Calendar.SetParametersAndRender(("Value", new DateTime(2020, 12, 31, 18, 30, 00)));
            Assert.Equal(new DateTime(2020, 12, 31, 18, 30, 00), Calendar.Instance.Value);
            var containerEle = Calendar.Find("input").ParentElement;
            Assert.Contains("e-error", containerEle.ClassName);
        }

        [Fact(Timeout = 10000)]
        public async Task MaxValue()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime>>(parameters =>
            parameters.Add(calendar => calendar.Value, new DateTime(2020, 1, 1)).Add(p => p.EnableMask, true));
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
            parameters.Add(calendar => calendar.Value, new DateTime(2017, 4, 4, 10, 30, 0)).Add(calendar => calendar.Min, new DateTime(2016, 12, 12, 10, 0, 0)).Add(p => p.EnableMask, true).Add(param => param.Max, new DateTime(2017, 3, 3, 11, 0, 0)));
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
        public async Task InputValueBinding()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime?>>(param => param.Add(p => p.EnableMask, true));
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
        public async Task AllowEdit()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(param => param.Add(p => p.EnableMask, true));
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
        public async Task TimeSteps()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(param => param.Add(p => p.Step, 30).Add(p => p.EnableMask, true));
            await dateInstance.Instance.ShowTimePopupAsync();
            var popupEle = dateInstance.Find(".e-popup");
            var liCollec = popupEle.QuerySelectorAll("li");
            Assert.Equal("12:30 AM", liCollec[1].GetAttribute("data-value").Replace('\u202F', ' ').Trim());
            await dateInstance.Instance.HidePopupAsync();
            await dateInstance.Instance.ClosePopupAsync();
            dateInstance.SetParametersAndRender(param => param.Add(p => p.Step, 10));
            await dateInstance.Instance.ShowTimePopupAsync();
            popupEle = dateInstance.Find(".e-popup");
            liCollec = popupEle.QuerySelectorAll("li");
            Assert.Equal("12:10 AM", liCollec[1].GetAttribute("data-value").Replace('\u202F', ' ').Trim());
        }
        [Fact(Timeout = 10000)]
        public async Task TimeFormat()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(param => param.Add(p => p.TimeFormat, "h:mm tt").Add(p => p.EnableMask, true));
            await dateInstance.Instance.ShowTimePopupAsync();
            var popupEle = dateInstance.Find(".e-popup");
            var inputEle = dateInstance.Find("input");
            var liCollec = popupEle.QuerySelectorAll("li");
            Assert.Equal("12:30 AM", liCollec[1].GetAttribute("data-value").Replace('\u202F', ' ').Trim());
            liCollec[1].Click();
            Assert.Contains(DateTime.Now.Day.ToString(), inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
            Assert.Contains("12:30 AM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
            await dateInstance.Instance.HidePopupAsync();
            await dateInstance.Instance.ClosePopupAsync();
            dateInstance.SetParametersAndRender(param => param.Add(p => p.Step, 10));
            await dateInstance.Instance.ShowTimePopupAsync();
            popupEle = dateInstance.Find(".e-popup");
            liCollec = popupEle.QuerySelectorAll("li");
            Assert.Equal("12:10 AM", liCollec[1].GetAttribute("data-value").Replace('\u202F', ' ').Trim());
        }

        [Fact(Timeout = 10000, DisplayName = "Masked input: valid typing commits via change")]
        public void Mask_Typing_Valid_Commits_Via_Change()
        {
            var comp = RenderComponent<SfDateTimePicker<DateTime?>>(ps => ps
                .Add(p => p.EnableMask, true)
                .Add(p => p.Format, "M/d/yyyy h:mm tt")
                .Add(p => p.ShowClearButton, true)
            );
            var input = comp.Find("input");
            input.Change(new ChangeEventArgs { Value = "4/5/2024 1:30 PM" });
            comp.WaitForAssertion(() =>
            {
                var val = input.GetAttribute("value").Replace('\u202F', ' ').Trim();
                Assert.Equal("4/5/2024 1:30 PM", val);
                Assert.DoesNotContain("e-error", input.ParentElement.ClassName);
            });
        }

        [Fact(Timeout = 10000, DisplayName = "Clear button clears value and updates input")]
        public async Task Mask_ClearButton_Clears_Value()
        {
            var comp = RenderComponent<SfDateTimePicker<DateTime?>>(ps => ps
                .Add(p => p.EnableMask, true)
                .Add(p => p.ShowClearButton, true)
                .Add(p => p.Value, new DateTime(2024, 5, 5, 10, 0, 0))
            );
            var input = comp.Find("input");
            var clear = input.ParentElement.QuerySelector(".e-close");
            Assert.NotNull(clear);
            clear.MouseDown();
            clear.Click();
            comp.WaitForAssertion(() =>
            {
                Assert.True(string.IsNullOrEmpty(input.GetAttribute("value")));
            });
        }
    }
}
