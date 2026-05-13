using Xunit;
using Bunit;
using Syncfusion.Blazor.Toolkit.Inputs;

namespace Syncfusion.Blazor.Toolkit.Tests.Inputs.NumericTextBox
{
    public class NumericTextBoxEvents : BunitTestContext
    {
        [Fact(Timeout = 10000)]
        public async Task CreatedEventHandler()
        {
            var isCreated = false;
            var numeric = RenderComponent<SfNumericTextBox<int?>>(param => param.Add(p => p.Value, 123).AddChildContent<SfNumericTextBox<int?>>(param => param.Add(p => p.Created, (object args) =>
            {
                isCreated = true;
            })));
            await Task.Delay(10);
            Assert.True(isCreated);
        }

        [Fact(Timeout = 10000)]
        public async Task FocusEventHandler()
        {
            var numeric = RenderComponent<SfNumericTextBox<double>>(param => param.Add(p => p.Value, 123)
            .AddChildContent<SfNumericTextBox<double>>(param => param.Add(p => p.OnFocus, (NumericFocusEventArgs<double> args) =>
            {
                Assert.Equal("Microsoft.AspNetCore.Components.Web.FocusEventArgs", args.Event.ToString());
                Assert.Equal("Focus", args.Name);
                Assert.Equal(123, args.Value);
            })));
            var inputEle = numeric.Find("input");
            await numeric.Instance.FocusAsync();
            inputEle.Focus();
            await Task.Delay(10);
            await numeric.Instance.FocusOutAsync();
        }

        [Fact(Timeout = 10000)]
        public async Task BlurEventHandler()
        {
            IRenderedComponent<SfNumericTextBox<int?>> numeric = null;
            numeric = RenderComponent<SfNumericTextBox<int?>>(param => param.Add(p => p.ID, "Blur-ID").Add(p => p.Value, 123).AddChildContent<SfNumericTextBox<int?>>(param => param.Add(p => p.OnBlur, (NumericBlurEventArgs<int?> args) =>
            {
                Assert.Equal("Microsoft.AspNetCore.Components.Web.FocusEventArgs", args.Event.ToString());
                Assert.Equal("Blur", args.Name);
                Assert.Equal(22, args.Value);
            })));
            var inputEle = numeric.Find("input");
            await numeric.Instance.FocusAsync();
            inputEle.Focus();
        }

        [Fact(Timeout = 10000)]
        public async Task ChangeEventHandler()
        {
            var ChangeCount = 0;
            var eventType = string.Empty;
            var IsInteracted = true;
            var Name = string.Empty;
            var PreviousValue = 0;
            var value = 0;
            var numeric = RenderComponent<SfNumericTextBox<int?>>(param => param.Add(p => p.Value, 2024).Add(p => p.ShowClearButton, true).Add(p => p.ValueChange, (ChangeEventArgs<int?> args) =>
                  {
                      ChangeCount++;
                      eventType = args.Event?.ToString();
                      IsInteracted = args.IsInteracted;
                      Name = args.Name;
                      PreviousValue = Convert.ToInt32(args.PreviousValue);
                      value = args.Value != null ? Convert.ToInt32(args.Value) : 0;
                  }));
            var inputEle = numeric.Find("input");
            //Basic Change Event with Value
            inputEle.Change("122");
            await Task.Delay(100);
            Assert.Equal(1, ChangeCount);
            Assert.Equal("Microsoft.AspNetCore.Components.ChangeEventArgs", eventType);
            Assert.True(IsInteracted);
            Assert.Equal("ValueChange", Name);
            Assert.Equal(2024, PreviousValue);
            Assert.Equal(122, value);
            //IncrementAsync Method with Change Event
            await numeric.Instance.IncrementAsync(5);
            await Task.Delay(100);
            Assert.Equal(2, ChangeCount);
            Assert.False(IsInteracted);
            Assert.Equal("ValueChange", Name);
            Assert.Equal(122, PreviousValue);
            Assert.Equal(127, value);
            //DecrementAsync Method with Change Event
            await numeric.Instance.DecrementAsync(2);
            await Task.Delay(100);
            Assert.Equal(3, ChangeCount);
            Assert.False(IsInteracted);
            Assert.Equal("ValueChange", Name);
            Assert.Equal(127, PreviousValue);
            Assert.Equal(125, value);
            //Clear Button Click with Change Event
            var clearButton = numeric.Find(".e-toolkit-icons.e-close");
            clearButton.TouchStart();
            await Task.Delay(100);
            Assert.Equal(4, ChangeCount);
            Assert.Equal("Microsoft.AspNetCore.Components.Web.TouchEventArgs", eventType);
            Assert.True(IsInteracted);
            Assert.Equal("ValueChange", Name);
            Assert.Equal(125, PreviousValue);
            Assert.Equal(0, value);
            //EnablePersistence with Change Event
            numeric.SetParametersAndRender(param => param.Add(p => p.EnablePersistence, true));
            inputEle.Change("25");
            await Task.Delay(100);
            Assert.Equal(5, ChangeCount);
            Assert.Equal("Microsoft.AspNetCore.Components.ChangeEventArgs", eventType);
            Assert.True(IsInteracted);
            Assert.Equal("ValueChange", Name);
            Assert.Equal(0, PreviousValue);
            Assert.Equal(25, value);
        }

        [Fact(Timeout = 10000)]
        public async Task ChangeEventHandlerWithDoubleValue()
        {
            var ChangeCount = 0;
            var eventType = string.Empty;
            var IsInteracted = true;
            var Name = string.Empty;
            var PreviousValue = 0.0;
            var value = 0.0;
            var numeric = RenderComponent<SfNumericTextBox<double?>>(param => param
                .Add(p => p.Value, 2024.5)
                .Add(p => p.ShowClearButton, true)
                .Add(p => p.ValueChange, (ChangeEventArgs<double?> args) =>
                    {
                        ChangeCount++;
                        eventType = args.Event?.ToString();
                        IsInteracted = args.IsInteracted;
                        Name = args.Name;
                        PreviousValue = args.PreviousValue ?? 0;
                        value = args.Value ?? 0;
                    }));
            var inputEle = numeric.Find("input");
            //Basic Change Event with Value
            inputEle.Change("122.75");
            await Task.Delay(100);
            Assert.Equal(1, ChangeCount);
            Assert.Equal("Microsoft.AspNetCore.Components.ChangeEventArgs", eventType);
            Assert.True(IsInteracted);
            Assert.Equal("ValueChange", Name);
            Assert.Equal(2024.5, PreviousValue);
            Assert.Equal(122.75, value);
            //IncrementAsync Method with Change Event
            await numeric.Instance.IncrementAsync(5);
            await Task.Delay(100);
            Assert.Equal(2, ChangeCount);
            Assert.False(IsInteracted);
            Assert.Equal("ValueChange", Name);
            Assert.Equal(122.75, PreviousValue);
            Assert.Equal(127.75, value);
            //DecrementAsync Method with Change Event
            await numeric.Instance.DecrementAsync(2.5);
            await Task.Delay(100);
            Assert.Equal(3, ChangeCount);
            Assert.False(IsInteracted);
            Assert.Equal("ValueChange", Name);
            Assert.Equal(127.75, PreviousValue);
            Assert.Equal(125.25, value);
            //Clear Button Click with Change Event
            var clearButton = numeric.Find(".e-toolkit-icons.e-close");
            clearButton.TouchStart();
            await Task.Delay(100);
            Assert.Equal(4, ChangeCount);
            Assert.Equal("Microsoft.AspNetCore.Components.Web.TouchEventArgs", eventType);
            Assert.True(IsInteracted);
            Assert.Equal("ValueChange", Name);
            Assert.Equal(125.25, PreviousValue);
            Assert.Equal(0, value);
            //EnablePersistence with Change Event
            numeric.SetParametersAndRender(param => param.Add(p => p.EnablePersistence, true));
            inputEle.Change("25.5");
            await Task.Delay(100);
            Assert.Equal(5, ChangeCount);
            Assert.Equal("Microsoft.AspNetCore.Components.ChangeEventArgs", eventType);
            Assert.True(IsInteracted);
            Assert.Equal("ValueChange", Name);
            Assert.Equal(0, PreviousValue);
            Assert.Equal(25.5, value);
        }

        [Fact(Timeout = 10000)]
        public async Task NativeChangeHandler()
        {
            var ChangeCount = 0;
            var numeric = RenderComponent<SfNumericTextBox<int?>>(param => param.Add(p => p.Value, 2024).Add(p => p.ShowClearButton, true).Add(p => p.OnChange, (Microsoft.AspNetCore.Components.ChangeEventArgs args) =>
                  {
                      ChangeCount++;
                  }));
            var inputEle = numeric.Find("input");
            //Spinner Button mehtod call
            inputEle.Focus();
            var spinUp = numeric.Find(".e-chevron-up");
            spinUp.MouseDown();
            spinUp.MouseUp();
            Assert.Equal(1, ChangeCount);
            var spinDown = numeric.Find(".e-chevron-down");
            spinDown.MouseDown();
            spinDown.MouseUp();
            Assert.Equal(2, ChangeCount);
            inputEle.Change(null);
            Assert.Equal(3, ChangeCount);
            inputEle.Change("122");
            Assert.Equal(4, ChangeCount);
            numeric.SetParametersAndRender(param => param.Add(p => p.Value, 20));
            inputEle.Change("20");
            await Task.Delay(100);
            Assert.Equal(5, ChangeCount);
            Assert.Equal(20, numeric.Instance.Value);
        }

        [Fact(Timeout = 10000)]
        public async Task ResetValue()
        {
            var changeCount = 0;
            IRenderedComponent<SfNumericTextBox<double?>> numeric = null;
            numeric = RenderComponent<SfNumericTextBox<double?>>(param => param.Add(p => p.Value, 10).Add(p => p.ShowClearButton, true).Add(p => p.ValueChange, (ChangeEventArgs<double?> args) =>
                  {
                      changeCount++;
                      numeric.SetParametersAndRender(param => param.Add(p => p.Value, null));
                  }));
            await numeric.Instance.IncrementAsync(1);
            await Task.Delay(300);
            var inputEle = numeric.Find("input");
            Assert.Null(inputEle.GetAttribute("value"));
            Assert.Null(numeric.Instance.Value);
        }

        [Fact(Timeout = 10000)]
        public async Task DestroyedHandler()
        {
            var count = 0;
            var numeric = RenderComponent<SfNumericTextBox<int?>>(param => param.Add(p => p.Value, 123).Add(p => p.Destroyed, (object args) =>
            {
                Assert.Null(args);
                count++;
            }));
            numeric.Instance.DisposeAsync();
            await Task.Delay(100);
            Assert.Equal(1, count);
        }
    }
}
