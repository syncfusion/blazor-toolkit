using System.Reflection;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Toolkit.Popups;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Popups.Dialog
{
    public class NullableDialogResultTests
    {
        [Fact]
        public async Task PromptCancel_ReturnsNullAndRemovesPendingTask()
        {
            var service = new SfDialogService();
            using var provider = new SfDialogProvider();
            Connect(service, provider);

            Task<string?> pending = service.PromptAsync("Enter a value");
            Assert.False(pending.IsCompleted);
            await Invoke(provider, "OnCancelButtonClick", new MouseEventArgs());

            Assert.Null(await pending);
            Assert.Empty(service._tasks);
        }

        [Theory]
        [InlineData(null, "")]
        [InlineData("", "")]
        [InlineData("entered value", "entered value")]
        public async Task PromptConfirmation_PreservesEmptyAndEnteredText(string? input, string expected)
        {
            var service = new SfDialogService();
            using var provider = new SfDialogProvider();
            Connect(service, provider);

            Task<string?> pending = service.PromptAsync("Enter a value");
            SetProperty(provider, "InputValue", input);
            await Invoke(provider, "OnOkButtonClickAsync");

            Assert.Equal(expected, await pending);
            Assert.Empty(service._tasks);
        }

        [Fact]
        public async Task PromptBuiltInClose_WithNullInputStillReturnsEmptyString()
        {
            var service = new SfDialogService();
            using var provider = new SfDialogProvider();
            Connect(service, provider);

            Task<string?> pending = service.PromptAsync("Enter a value");
            await Invoke(provider, "OnDialogClose", new CloseEventArgs());

            Assert.Equal(string.Empty, await pending);
            Assert.Empty(service._tasks);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Confirmation_ReturnsBoolean(bool accept)
        {
            var service = new SfDialogService();
            using var provider = new SfDialogProvider();
            Connect(service, provider);

            Task<bool> pending = service.ConfirmAsync("Continue?");
            if (accept)
            {
                await Invoke(provider, "OnOkButtonClickAsync");
            }
            else
            {
                await Invoke(provider, "OnCancelButtonClick", new MouseEventArgs());
            }

            Assert.Equal(accept, await pending);
            Assert.Empty(service._tasks);
        }

        [Fact]
        public async Task PublicOnOpenEvent_CanCompletePromptWithNull()
        {
            var service = new SfDialogService();
            service.OnOpen += (_, _, _, _, tasks) => tasks[^1].SetResult(null);
            Assert.Null(await service.PromptAsync("Enter a value"));
        }

        private static void Connect(SfDialogService service, SfDialogProvider provider)
        {
            // Exercise the actual service task list and provider completion handlers without
            // opening browser UI or invoking the provider's async-void render callback.
            service.OnOpen += (type, _, _, _, tasks) =>
            {
                SetProperty(provider, "DialogType", type);
                SetProperty(provider, "CompleteTask", tasks);
            };
        }

        private static void SetProperty(SfDialogProvider provider, string name, object? value)
        {
            PropertyInfo property = typeof(SfDialogProvider).GetProperty(name, BindingFlags.Instance | BindingFlags.NonPublic)
                ?? throw new InvalidOperationException($"Dialog property {name} was not found.");
            property.SetValue(provider, value);
        }

        private static Task Invoke(SfDialogProvider provider, string name, params object?[] args)
        {
            MethodInfo method = typeof(SfDialogProvider).GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic)
                ?? throw new InvalidOperationException($"Dialog method {name} was not found.");
            return Assert.IsAssignableFrom<Task>(method.Invoke(provider, args));
        }
    }
}
