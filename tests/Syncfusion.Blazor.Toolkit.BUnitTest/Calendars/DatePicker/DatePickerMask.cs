using Xunit;
using Bunit;
using Syncfusion.Blazor.Toolkit.Calendars;
using Syncfusion.Blazor.Toolkit.Inputs;
using Microsoft.AspNetCore.Components;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Syncfusion.Blazor.Toolkit.Tests.Calendars.DatePicker
{
    public class DatePickerMask : BunitTestContext
    {
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

        [Fact(Timeout = 10000, DisplayName = "ReadOnly - Readonly prevents popup when enabled for DatePicker with mask")]
        public async Task ReadOnly()
        {
            var dateInstance = RenderComponent<SfDatePicker<DateTime?>>(parameters => parameters.Add(p => p.Readonly, true).Add(p => p.EnableMask, true));
            var containerElement = dateInstance.Find("input").ParentElement;
            var iconElement = containerElement?.QuerySelector(".e-timeline-today");
            iconElement?.MouseDown();
            await Task.Delay(70);
            var readonlyPopupElements = dateInstance.FindAll(".e-popup");
            Assert.Equal(0, readonlyPopupElements.Count);
            dateInstance.SetParametersAndRender(("Readonly", false));
            containerElement = dateInstance.Find("input").ParentElement;
            iconElement = containerElement?.QuerySelector(".e-timeline-today");
            iconElement?.MouseDown();
            await Task.Delay(200);
            var popupElement = dateInstance.Find(".e-popup");
            Assert.NotNull(popupElement);
            await dateInstance.Instance.HidePopupAsync();
        }

        [Fact(Timeout = 10000, DisplayName = "Placeholder - Sets placeholder attribute on input for DatePicker with mask")]
        public void Placeholder()
        {
            var dateInstance = RenderComponent<SfDatePicker<DateTime?>>(parameters =>
                    parameters.Add(p => p.Placeholder, "Enter the date value").Add(p => p.EnableMask, true));
            var inputElement = dateInstance.Find("input");
            Assert.Contains("Enter the date value", inputElement.GetAttribute("placeholder"));
            dateInstance.SetParametersAndRender(("Placeholder", "Enter the date"));
            Assert.Contains("Enter the date", inputElement.GetAttribute("placeholder"));
        }

        [Fact(Timeout = 10000, DisplayName = "ClearButton - Shows and clears value using clear button for DatePicker with mask")]
        public async Task ClearButton()
        {
            var dateInstance = RenderComponent<SfDatePicker<DateTime?>>(param => param.Add(p => p.EnableMask, true));
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
            var prevButton = buttonList[0];
            var nextButton = buttonList[1];
            var todayButton = buttonList[2];
            Assert.Contains("e-prev", prevButton.ClassName);
            Assert.Contains("e-next", nextButton.ClassName);
            Assert.Contains("e-today", todayButton.ClassName);
            Assert.Equal("Today", todayButton.TextContent);
            var tableBody = tableElement?.QuerySelector("tbody");
            var tableRows = tableBody?.QuerySelectorAll("tr");
            Assert.Equal(6, tableRows?.Length);
            var tableCells = tableRows?[1].QuerySelectorAll("td");
            tableCells?[3].Click();
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
       
        [Fact(Timeout = 10000, DisplayName = "DefaultDateTimeValue - Renders calendar with provided DateTime and header")]
        public async Task DefaultDateTimeValue()
        {
            var dateInstance = RenderComponent<SfDatePicker<DateTime>>(parameters => parameters.
            Add(cal => cal.Value, new DateTime(1900, 1, 1)).Add(p => p.EnableMask, true));
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
        
        [Fact(Timeout = 10000, DisplayName = "MinValue - Enforces Min date and disables previous navigation for DateTime")]
        public async Task MinValue()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime>>(parameters =>
            parameters.Add(p => p.Value, new DateTime(2020, 1, 1)).Add(p => p.EnableMask, true));
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
       
        [Fact(Timeout = 10000, DisplayName = "MaxValue - Enforces Max date and disables next navigation for DateTime")]
        public async Task MaxValue()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime>>(parameters =>
            parameters.Add(p => p.Value, new DateTime(2020, 1, 1)).Add(p => p.EnableMask, true));
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

        [Fact(Timeout = 10000, DisplayName = "MaxValueWithStrictModeTrue - Enforces Max with StrictMode and prevents invalid assignment")]
        public void MaxValueWithStrictModeTrue()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime>>(parameters =>
            parameters.Add(p => p.Max, new DateTime(2020, 1, 1)).Add(p => p.EnableMask, true).Add(p => p.StrictMode, true));
            calendar.SetParametersAndRender(("Value", new DateTime(2020, 1, 1)));
            Assert.Equal(new DateTime(2020, 1, 1), calendar.Instance.Max);            
            var inputElement = calendar.Find("input");
            calendar.SetParametersAndRender(("Value", new DateTime(2021, 1, 1)));            
            Assert.Equal(new DateTime(2020, 1, 1), calendar.Instance.Value);
            var containerElement = calendar.Find("input").ParentElement;
            Assert.DoesNotContain("e-error", containerElement?.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "MinValueWithStrictModeTrue - Enforces Min with StrictMode and corrects value")]
        public void MinValueWithStrictModeTrue()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime>>(parameters =>
            parameters.Add(p => p.Value, new DateTime(2020, 1, 1)).Add(p => p.EnableMask, true).Add(p => p.StrictMode, true));
            calendar.SetParametersAndRender(("Min", new DateTime(2020, 1, 1)));
            Assert.Equal(new DateTime(2020, 1, 1), calendar.Instance.Min);
            var inputElement = calendar.Find("input");
            calendar.SetParametersAndRender(("Value", new DateTime(2019, 1, 1)));
            Assert.Equal("1/1/2020", inputElement.GetAttribute("value"));
            var containerElement = calendar.Find("input").ParentElement;
            Assert.DoesNotContain("e-error", containerElement?.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "MinValueWithStrictModeFalse - Shows error class when value is less than Min and StrictMode false")]
        public void MinValueWithStrictModeFalse()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime>>(parameters =>
            parameters.Add(p => p.Value, new DateTime(2020, 1, 1)).Add(p => p.EnableMask, true).Add(p => p.StrictMode, false));
            calendar.SetParametersAndRender(("Min", new DateTime(2020, 1, 1)));
            Assert.Equal(new DateTime(2020, 1, 1), calendar.Instance.Min);
            var inputElement = calendar.Find("input");
            calendar.SetParametersAndRender(("Value", new DateTime(2019, 1, 1)));
            Assert.Equal("1/1/2019", inputElement.GetAttribute("value"));
            var containerElement = calendar.Find("input").ParentElement;
            Assert.Contains("e-error", containerElement?.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "MaxValueWithStrictModeFalse - Shows error class when value is greater than Max and StrictMode false")]
        public void MaxValueWithStrictModeFalse()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime>>(parameters =>
            parameters.Add(p => p.Value, new DateTime(2020, 1, 1)).Add(p => p.EnableMask, true).Add(p => p.StrictMode, false));
            calendar.SetParametersAndRender(("Max", new DateTime(2020, 1, 1)));
            Assert.Equal(new DateTime(2020, 1, 1), calendar.Instance.Max);
            var inputElement = calendar.Find("input");
            calendar.SetParametersAndRender(("Value", new DateTime(2021, 1, 1)));
            Assert.Equal("1/1/2021", inputElement.GetAttribute("value"));
            var containerElement = calendar.Find("input").ParentElement;
            Assert.Contains("e-error", containerElement?.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "MaxValueWithNullable - Nullable Max enforces and disables navigation")]
        public async Task MaxValueWithNullable()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime?>>(parameters =>
            parameters.Add(p => p.Value, new DateTime(2020, 1, 1)).Add(p => p.EnableMask, true));
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
               
        [Fact(Timeout = 10000, DisplayName = "DynamicValueBinding - Updates UI when DateTime Value parameter changes at runtime")]
        public async Task DynamicValueBinding()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime?>>(param => param.Add(p => p.EnableMask, true));
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
            var selectedCells = tableElement?.QuerySelectorAll("td.e-selected");
            Assert.Equal(1, selectedCells?.Length);
            Assert.Equal("5", selectedCells?[0].FirstElementChild?.TextContent.Replace("\n", "").Trim());
            Assert.Equal(GetDateFormat(calendar.Instance.Value, "dddd, MMMM d, yyyy"), selectedCells?[0].FirstElementChild?.GetAttribute("title"));
        }

        [Fact(Timeout = 10000, DisplayName = "InputValueBinding - Binds input change to DatePicker value when user enters text")]
        public async Task InputValueBinding()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime?>>(param => param.Add(p => p.EnableMask, true));
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

        [Fact(Timeout = 10000, DisplayName = "AllowEdit - Toggles readonly attribute based on AllowEdit parameter")]
        public void AllowEdit()
        {
            var dateInstance = RenderComponent<SfDatePicker<DateTime?>>(param => param.Add(p => p.EnableMask, true));
            var inputElement = dateInstance.Find("input");
            Assert.Null(inputElement.GetAttribute("readonly"));
            Assert.True(dateInstance.Instance.AllowEdit);
            dateInstance.SetParametersAndRender(("AllowEdit", false));
            inputElement = dateInstance.Find("input");
            Assert.NotNull(inputElement.GetAttribute("readonly"));
            Assert.False(dateInstance.Instance.AllowEdit);
        }
        [Fact(Timeout = 10000, DisplayName = "FloatingLabel - Renders float label states for DatePicker FloatLabelType values")]
        public void FloatingLabel()
        {
            var dateInstance = RenderComponent<SfDatePicker<DateTime?>>(parameter => parameter.Add(p => p.Placeholder, "Enter the value").Add(p => p.EnableMask, true));
            var containerElement = dateInstance.Find("input").ParentElement;
            Assert.DoesNotContain("e-float-text", containerElement?.ClassName);
            dateInstance.SetParametersAndRender(("FloatLabelType", FloatLabelType.Auto));
            containerElement = dateInstance.Find("input").ParentElement;
            Assert.Contains("e-float-input", containerElement?.ClassName);
            var floatElement = containerElement?.QuerySelector(".e-float-text");
            Assert.NotNull(floatElement);
            Assert.Contains("e-label-bottom", floatElement.ClassName);
            dateInstance.SetParametersAndRender(("Value", DateTime.Now));
            var inputElement = dateInstance.Find("input");
            containerElement = dateInstance.Find("input").ParentElement;
            floatElement = containerElement?.QuerySelector(".e-float-text");
            Assert.Contains("e-label-top", floatElement?.ClassName);
            dateInstance.SetParametersAndRender(("FloatLabelType", FloatLabelType.Always));
            containerElement = dateInstance.Find("input").ParentElement;
            Assert.Contains("e-float-input", containerElement?.ClassName);
            floatElement = containerElement?.QuerySelector(".e-float-text");
            Assert.NotNull(floatElement);
            Assert.Contains("e-label-top", floatElement.ClassName);
            dateInstance = RenderComponent<SfDatePicker<DateTime?>>(parameter => parameter.Add(p => p.Placeholder, "Enter the value").Add(p => p.FloatLabelType, FloatLabelType.Always).Add(p => p.EnableMask, true));
            inputElement = dateInstance.Find("input");
            inputElement.Focus();
            containerElement = dateInstance.Find("input").ParentElement;
            floatElement = containerElement?.QuerySelector(".e-float-text");
            Assert.NotNull(floatElement);
            Assert.Contains("e-label-top", floatElement.ClassName);
            dateInstance = RenderComponent<SfDatePicker<DateTime?>>(parameter => parameter.Add(p => p.Placeholder, "Enter the value").Add(p => p.FloatLabelType, FloatLabelType.Never).Add(p => p.EnableMask, true));
            containerElement = dateInstance.Find("input").ParentElement;
            Assert.DoesNotContain("e-float-input", containerElement?.ClassName);
            floatElement = containerElement?.QuerySelector(".e-float-text");
            Assert.Null(floatElement);
            dateInstance.SetParametersAndRender(("FloatLabelType", FloatLabelType.Auto));
            containerElement = dateInstance.Find("input").ParentElement;
            Assert.Contains("e-float-input", containerElement?.ClassName);
            floatElement = containerElement?.QuerySelector(".e-float-text");
            Assert.NotNull(floatElement);
            Assert.Contains("e-label-bottom", floatElement.ClassName);
            inputElement = dateInstance.Find("input");
            inputElement.Focus();
            containerElement = dateInstance.Find("input").ParentElement;
            floatElement = containerElement?.QuerySelector(".e-float-text");
            Assert.NotNull(floatElement);
            Assert.Contains("e-label-top", floatElement.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "DefaultFormat - Uses default format and selects correct date")]
        public async Task DefaultFormat()
        {
            var dateInstance = RenderComponent<SfDatePicker<DateTime?>>(param => param.Add(p => p.Value, new DateTime(2021, 3, 5)).Add(p => p.EnableMask, true));
            await dateInstance.Instance.ShowPopupAsync();
            var popupElement = dateInstance.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            var tableRows = tableElement?.QuerySelectorAll("tr");
            tableRows?[2].QuerySelectorAll("td")[2].Click();
            Assert.Equal(new DateTime(2021, 3, 9), dateInstance.Instance.Value);
        }
        
        private string GetStandardFormatString(string format)
        {
            var ValidFormats = "^[F/U/u/O/o/d/D/f/g/G/m/M/R/r/s/t/T/y/Y]*$";
            if (format != null && format.Length < 2 && Regex.IsMatch(format, ValidFormats))
            {
                var cultureformat = CultureInfo.CurrentCulture.DateTimeFormat.GetAllDateTimePatterns(char.Parse(format));
                return cultureformat[0];
            }
            return format!;
        }

        [Fact(Timeout = 10000, DisplayName = "EnableMaskFormat - Verifies format mappings when EnableMask true")]
        public async Task EnableMaskFormat()
        {
            var dateInstance = RenderComponent<SfDatePicker<DateTime?>>(param => param.Add(p => p.StrictMode, false).Add(p => p.EnableMask, true).Add(p=>p.Format, "F"));            
            await Task.Delay(100);
            Assert.True(dateInstance.Instance.EnableMask);
            var containElement = dateInstance.Find("input");            
            Assert.Null(containElement.GetAttribute("value"));
            Assert.Equal("dddd, MMMM d, yyyy h:mm:ss tt", GetStandardFormatString(dateInstance.Instance.Format).Replace('\u202F', ' ').Trim());
            dateInstance.SetParametersAndRender(("Format", "D"));
            Assert.Equal("dddd, MMMM d, yyyy", GetStandardFormatString(dateInstance.Instance.Format).Replace('\u202F', ' ').Trim());
            dateInstance.SetParametersAndRender(("Format", "U"));
            Assert.Equal("dddd, MMMM d, yyyy h:mm:ss tt", GetStandardFormatString(dateInstance.Instance.Format).Replace('\u202F', ' ').Trim());
        }
    }
}
