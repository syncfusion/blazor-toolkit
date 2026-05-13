using Bunit;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Toolkit.Inputs;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Inputs.TextBox
{
    public class PersistenceTests : BunitTestContext
    {
        [Fact(Timeout = 10000)]
        public async Task RestoresPersistedValueOnInit()
        {
            // Arrange
            var id = "tb-persist-id";
            // Mock getLocalStorageItem to return a persisted value when the component asks for the same id
            JSInterop.Setup<string>("getLocalStorageItem", args => args.Arguments.Count == 1 && args.Arguments[0]?.ToString() == id)
                    .SetResult("PersistedValue");

            // Act
            var textBox = RenderComponent<SfTextBox>(parameters => parameters.Add(p => p.EnablePersistence, true).Add(p => p.ID, id));
            await Task.Delay(100);

            var input = textBox.Find("input");

            // Assert
            Assert.Equal("PersistedValue", input.GetAttribute("value"));
            Assert.Contains(JSInterop.Invocations, i => i.Identifier == "getLocalStorageItem");
        }

        [Fact(Timeout = 10000)]
        public async Task SavesValueToLocalStorageOnChange()
        {
            // Arrange
            var id = "tb-persist-save-id";
            // Ensure getLocalStorageItem returns null for init
            JSInterop.Setup<string>("getLocalStorageItem", args => args.Arguments.Count == 1 && args.Arguments[0]?.ToString() == id)
                    .SetResult((string?)null!);

            // Capture setLocalStorageItem calls
            JSInterop.SetupVoid("setLocalStorageItem").SetVoidResult();

            var textBox = RenderComponent<SfTextBox>(parameters => parameters.Add(p => p.EnablePersistence, true).Add(p => p.ID, id));
            await Task.Delay(100);

            var input = textBox.Find("input");

            // Act: change the value
            input.Change("SavedValue");
            await Task.Delay(100);

            // Assert: setLocalStorageItem was invoked with the ID and the new value
            Assert.Contains(JSInterop.Invocations, i => i.Identifier == "setLocalStorageItem");
            var inv = JSInterop.Invocations.Last(i => i.Identifier == "setLocalStorageItem");
            Assert.True(inv.Arguments.Count >= 2);
            Assert.Equal(id, inv.Arguments[0]?.ToString());
            Assert.Equal("SavedValue", inv.Arguments[1]?.ToString());
        }
    }
}
