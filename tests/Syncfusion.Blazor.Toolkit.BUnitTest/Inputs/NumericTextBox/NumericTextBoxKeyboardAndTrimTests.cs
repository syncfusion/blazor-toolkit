using System.Text.Json;
using System.Text.Json.Serialization;
using Bunit;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Toolkit.Inputs;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Inputs.NumericTextBox
{
    /// <summary>
    /// Regression coverage for GitHub issues #82 and #83.
    /// </summary>
    public class NumericTextBoxKeyboardAndTrimTests : BunitTestContext
    {
        [Fact(Timeout = 10000, DisplayName = "Issue #82: clear button stays in the tab order")]
        public void ClearButton_IsKeyboardReachable_WhenShowClearButtonIsTrue()
        {
            var numeric = RenderComponent<SfNumericTextBox<decimal?>>(parameters => parameters
                .Add(component => component.Value, 12.5m)
                .Add(component => component.ShowClearButton, true));

            var clearButton = numeric.Find("button.e-close");

            Assert.Equal("button", clearButton.GetAttribute("type"));
            Assert.False(string.IsNullOrWhiteSpace(clearButton.GetAttribute("aria-label")));
            // Hidden until focus, but already a tab stop so Tab cannot race the reveal render.
            Assert.Contains("e-clear-icon-hide", clearButton.ClassList);
            Assert.NotEqual("-1", clearButton.GetAttribute("tabindex"));
            Assert.DoesNotContain("onmousedown:preventDefault", clearButton.OuterHtml, StringComparison.OrdinalIgnoreCase);
        }

        [Fact(Timeout = 10000, DisplayName = "Issue #82: spin buttons stay in the tab order")]
        public void SpinButtons_AreKeyboardReachable()
        {
            var numeric = RenderComponent<SfNumericTextBox<decimal?>>(parameters => parameters
                .Add(component => component.Value, 12.5m)
                .Add(component => component.ShowSpinButton, true));

            var spinButtons = numeric.FindAll("button.e-chevron-down, button.e-chevron-up");

            Assert.Equal(2, spinButtons.Count);
            Assert.All(spinButtons, button =>
            {
                Assert.Equal("button", button.GetAttribute("type"));
                Assert.False(string.IsNullOrWhiteSpace(button.GetAttribute("aria-label")));
                Assert.NotEqual("-1", button.GetAttribute("tabindex"));
                Assert.DoesNotContain("onmousedown:preventDefault", button.OuterHtml, StringComparison.OrdinalIgnoreCase);
            });
        }

        [Fact(Timeout = 10000, DisplayName = "Issue #82: keyboard activation clears a nullable value")]
        public async Task ClearButton_Click_ClearsNullableValue()
        {
            var numeric = RenderComponent<SfNumericTextBox<decimal?>>(parameters => parameters
                .Add(component => component.Value, 12.5m)
                .Add(component => component.ShowClearButton, true));

            await numeric.Find("button.e-close").ClickAsync(new MouseEventArgs());

            Assert.Null(numeric.Instance.Value);
        }

        [Fact(DisplayName = "Issue #82: blur handler does not cancel Tab focus")]
        public void NumericTextBoxScript_DoesNotCancelBlur()
        {
            string script = File.ReadAllText(NumericTextBoxScriptPath());
            int handlerStart = script.IndexOf("SfNumericTextBox.prototype.focusOutHandler", StringComparison.Ordinal);
            Assert.True(handlerStart >= 0, "focusOutHandler was not found in numerictextbox.js.");

            int handlerEnd = script.IndexOf("SfNumericTextBox.prototype.", handlerStart + 1, StringComparison.Ordinal);
            string handler = script[handlerStart..handlerEnd];

            Assert.DoesNotContain("event.preventDefault", handler, StringComparison.Ordinal);
            Assert.DoesNotContain(".preventDefault()", handler, StringComparison.Ordinal);
        }

        [Fact(DisplayName = "Issue #83: device detection uses a primitive bool interop result")]
        public void DeviceDetection_DoesNotDeserializeDeviceMode()
        {
            string component = File.ReadAllText(BaseComponentPath());
            int methodStart = component.IndexOf("internal async Task UpdateIsDeviceModeAsync()", StringComparison.Ordinal);
            Assert.True(methodStart >= 0, "UpdateIsDeviceModeAsync was not found.");

            int methodEnd = component.IndexOf("internal class JsModuleReference", methodStart, StringComparison.Ordinal);
            string method = component[methodStart..methodEnd];

            Assert.Contains("InvokeAsync<bool>", method, StringComparison.Ordinal);
            Assert.DoesNotContain("InvokeAsync<DeviceMode>", method, StringComparison.Ordinal);
            Assert.Contains("\"isDevice\"", method, StringComparison.Ordinal);
        }

        [Fact(DisplayName = "Issue #83: isDevice returns a boolean for Blazor interop and an object for JS callers")]
        public void IsDeviceScript_ReturnsBooleanForInteropWithoutBreakingJsCallers()
        {
            string script = File.ReadAllText(BaseScriptPath());

            Assert.Contains("function isDevice(asObject)", script, StringComparison.Ordinal);
            Assert.Contains("if (asObject)", script, StringComparison.Ordinal);
            Assert.Contains("return Browser.isDevice;", script, StringComparison.Ordinal);
            Assert.Contains("return { IsDevice: Browser.isDevice };", script, StringComparison.Ordinal);
        }

        [Fact(DisplayName = "Issue #83: DeviceMode survives reflection-based and source-generated deserialization")]
        public void DeviceMode_DeserializesWithoutAPublicConstructorRequirement()
        {
            const string payload = "{\"IsDevice\":true}";

            DeviceMode reflectionResult = JsonSerializer.Deserialize<DeviceMode>(payload, new JsonSerializerOptions(JsonSerializerDefaults.Web))!;
            DeviceMode generatedResult = JsonSerializer.Deserialize(payload, ToolkitJsonContext.Default.DeviceMode)!;

            Assert.True(reflectionResult.IsDevice);
            Assert.True(generatedResult.IsDevice);
        }

        private static string NumericTextBoxScriptPath() => RepoFile("src", "wwwroot", "scripts", "numerictextbox.js");

        private static string BaseScriptPath() => RepoFile("src", "wwwroot", "scripts", "base.js");

        private static string BaseComponentPath() => RepoFile("src", "Base", "SfBaseComponent.cs");

        private static string RepoFile(params string[] segments)
        {
            var directory = new DirectoryInfo(AppContext.BaseDirectory);
            while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "src", "Syncfusion.Blazor.Toolkit.csproj")))
            {
                directory = directory.Parent;
            }

            if (directory is null)
            {
                throw new DirectoryNotFoundException("Could not locate the toolkit repository root from the test output directory.");
            }

            return Path.Combine(new[] { directory.FullName }.Concat(segments).ToArray());
        }
    }

    /// <summary>
    /// Stand-in for the constructor metadata a trimmed publish keeps only when a type is
    /// registered with the source generator.
    /// </summary>
    [JsonSerializable(typeof(DeviceMode))]
    internal sealed partial class ToolkitJsonContext : JsonSerializerContext
    {
    }
}
