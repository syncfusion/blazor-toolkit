using Xunit;
using Bunit;
using Syncfusion.Blazor.Toolkit.Calendars;

namespace Syncfusion.Blazor.Toolkit.Tests.Calendars.DatePicker
{
    public class StrictMode : BunitTestContext
    {
        [Fact(Timeout = 10000, DisplayName = "StrictMode: default behavior")]
        public void DefaultStrictMode()
        {
            var component = RenderComponent<SfDatePicker<DateTime>>();
            Assert.False(component.Instance.StrictMode);
            var containerElement = component.Find("input").ParentElement;
            Assert.Contains("e-error", containerElement?.ClassName);
            component.SetParametersAndRender(("StrictMode", true));
            Assert.True(component.Instance.StrictMode);
            containerElement = component.Find("input").ParentElement;
            Assert.DoesNotContain("e-error", containerElement?.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "StrictMode: Min and Max same value")]
        public void StrictModeMinMax()
        {
            var component = RenderComponent<SfDatePicker<DateTime>>(param=>param.Add(p=>p.Value, DateTime.Now).Add(p=>p.StrictMode, true).Add(p=>p.Min, new DateTime(2017, 3, 3)).Add(p=>p.Max, new DateTime(2017, 3, 3)));
            Assert.Equal(new DateTime(2017, 3, 3), component.Instance.Value);
            var inputElement = component.Find("input");
            Assert.Equal("3/3/2017", inputElement.GetAttribute("value"));
        }

        [Fact(Timeout = 10000, DisplayName = "StrictMode: Min greater than Max")]
        public void StrictModeMinGreaterthanMax()
        {
            var component = RenderComponent<SfDatePicker<DateTime>>(param => param.Add(p => p.Value, new DateTime(2017, 3, 3)).Add(p => p.StrictMode, true).Add(p => p.Min, new DateTime(2017, 3, 4)).Add(p => p.Max, new DateTime(2017, 3, 3)));
            Assert.Equal(new DateTime(2017, 3, 3), component.Instance.Value);
            Assert.Equal(new DateTime(2017, 3, 4), component.Instance.Min);
            Assert.Equal(new DateTime(2017, 3, 3), component.Instance.Max);
            var inputElement = component.Find("input");
            Assert.Equal("3/3/2017", inputElement.GetAttribute("value"));
            component.SetParametersAndRender(("Min", new DateTime(2017, 3, 3)));
            Assert.Equal(new DateTime(2017, 3, 3), component.Instance.Min);
            inputElement = component.Find("input");
            Assert.Equal("3/3/2017", inputElement.GetAttribute("value"));
        }

        [Fact(Timeout = 10000, DisplayName = "StrictMode: value higher than Max")]
        public void ValueHigherThanMax()
        {
            var component = RenderComponent<SfDatePicker<DateTime>>(param => param.Add(p => p.Value, new DateTime(9999, 4, 4)).Add(p => p.StrictMode, true));
            Assert.Equal(new DateTime(2099, 12, 31), component.Instance.Value);
            var inputElement = component.Find("input");
            Assert.Equal("12/31/2099", inputElement.GetAttribute("value"));
        }

        [Fact(Timeout = 10000, DisplayName = "StrictMode: value lower than Min")]
        public void ValueLowerThanMax()
        {
            var component = RenderComponent<SfDatePicker<DateTime>>(param => param.Add(p => p.Value, new DateTime(1111, 1, 1)).Add(p => p.StrictMode, true).Add(p=>p.Format, "M/d/yyyy"));
            Assert.Equal(new DateTime(1900, 01, 01), component.Instance.Value);
            var inputElement = component.Find("input");
            Assert.Equal("1/1/1900", inputElement.GetAttribute("value"));
        }

        [Fact(Timeout = 10000, DisplayName = "StrictMode: value out of Min and Max range")]
        public void MinMaxValueBind()
        {
            var component = RenderComponent<SfDatePicker<DateTime>>(param => param.Add(p => p.Value, new DateTime(2017, 3, 3)).Add(p => p.StrictMode, true).Add(p => p.Format, "M/d/yyyy").
            Add(p=>p.Min, new DateTime(2017, 4, 4)).Add(p=>p.Max, new DateTime(2017, 6, 6)));
            Assert.Equal(new DateTime(2017, 4, 4), component.Instance.Value);
            var inputElement = component.Find("input");
            Assert.Equal("4/4/2017", inputElement.GetAttribute("value"));
            Assert.Equal(new DateTime(2017, 4, 4), component.Instance.Min);
            Assert.Equal(new DateTime(2017, 6, 6), component.Instance.Max);
        }

        [Fact(Timeout = 10000, DisplayName = "StrictMode: false with value higher than Max")]
        public void HigherThanMax()
        {
            var component = RenderComponent<SfDatePicker<DateTime>>(param => param.Add(p => p.Value, new DateTime(9999, 4, 4)).Add(p => p.StrictMode, false));
            Assert.Equal(new DateTime(9999, 4, 4), component.Instance.Value);
            var inputElement = component.Find("input");
            var containerElement = inputElement.ParentElement;
            Assert.Equal("4/4/9999", inputElement.GetAttribute("value"));
            Assert.Contains("e-error", containerElement?.ClassName);
        }
        
        [Fact(Timeout = 10000, DisplayName = "StrictMode: false with value lower than Min")]
        public void LowerThanMin()
        {
            var component = RenderComponent<SfDatePicker<DateTime>>(param => param.Add(p => p.Value, new DateTime(1111, 1, 1)).Add(p => p.StrictMode, false));
            Assert.Equal(new DateTime(1111, 1, 1), component.Instance.Value);
            var inputElement = component.Find("input");
            var containerElement = inputElement.ParentElement;
            Assert.Equal("1/1/1111", inputElement.GetAttribute("value"));
            Assert.Contains("e-error", containerElement?.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "StrictMode: false with Min and Max range")]
        public void MaxMinStrictMode()
        {
            var component = RenderComponent<SfDatePicker<DateTime>>(param => param.Add(p => p.Value, new DateTime(2017, 3, 3)).Add(p => p.Min, new DateTime(2017, 4, 4)).Add(p => p.Max, new DateTime(2017, 6, 6)).Add(p => p.StrictMode, false));
            Assert.Equal(new DateTime(2017, 3, 3), component.Instance.Value);
            var inputElement = component.Find("input");
            var containerElement = inputElement.ParentElement;
            Assert.Equal("3/3/2017", inputElement.GetAttribute("value"));
            Assert.Contains("e-error", containerElement?.ClassName);
            Assert.Equal(new DateTime(2017, 4, 4), component.Instance.Min);
            Assert.Equal(new DateTime(2017, 6, 6), component.Instance.Max);
        }
    }
}
