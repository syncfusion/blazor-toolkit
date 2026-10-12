using Bunit;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Inputs;
using Syncfusion.Blazor.Toolkit.Tests;
using System.Net.Http;
using System.Text.Json;
using Xunit;

namespace TransportCharacterization;

// Opt-in current-behavior observations, intentionally outside the normal CI suite.
public sealed class UploaderTransportCharacterization : BunitTestContext
{
    private BunitJSModuleInterop SetupModules()
    {
        foreach (var module in new[] { "base", "ajax", "sanitize-html-helper", "animation" })
            JSInterop.SetupModule($"./_content/Syncfusion.Blazor.Toolkit/scripts/{module}.js").Mode = JSRuntimeMode.Loose;
        var uploader = JSInterop.SetupModule("./_content/Syncfusion.Blazor.Toolkit/scripts/uploader.js");
        uploader.Mode = JSRuntimeMode.Loose;
        return uploader;
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void Current_initialization_and_parent_update_export_defaults_without_sending(bool auto, bool persistence)
    {
        var module = SetupModules();
        var snapshots = new List<string>();
        module.SetupVoid("initialize", call => { snapshots.Add(JsonSerializer.Serialize(call.Arguments[3])); return true; }).SetVoidResult();
        module.SetupVoid("propertyChanges", call => { snapshots.Add(JsonSerializer.Serialize(call.Arguments[1])); return true; }).SetVoidResult();
        var handler = new CountingHandler();
        using var client = new HttpClient(handler);
        client.DefaultRequestHeaders.Add("X-R06-Backend", "DUMMY_INITIAL");
        var component = RenderComponent<SfUploader>(p => p.Add(x => x.HttpClientInstance, client)
            .Add(x => x.AutoUpload, auto).Add(x => x.EnablePersistence, persistence));
        component.WaitForAssertion(() => Assert.Contains(snapshots, x => x.Contains("DUMMY_INITIAL")));
        client.DefaultRequestHeaders.Remove("X-R06-Backend");
        client.DefaultRequestHeaders.Add("X-R06-Backend", "DUMMY_UPDATED");
        component.SetParametersAndRender(p => p.Add(x => x.CssClass, "r06-updated"));
        component.WaitForAssertion(() => Assert.Contains(snapshots, x => x.Contains("DUMMY_UPDATED")));
        Assert.Equal(0, handler.Sends);
        component.Dispose();
        Assert.False(handler.Disposed);
    }

    [Theory]
    [InlineData("before-upload")]
    [InlineData("before-remove")]
    [InlineData("upload-start")]
    [InlineData("remove-start")]
    public async Task Managed_bridges_await_handlers_even_when_browser_dispatch_does_not(string stage)
    {
        SetupModules();
        var entered = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        var release = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        async Task Wait() { entered.TrySetResult(true); await release.Task; }
        var headers = new object[] { new Dictionary<string, string> { ["X-R06-Explicit"] = "DUMMY_USER" } };
        var component = RenderComponent<SfUploader>(p => p
            .Add(x => x.BeforeUpload, EventCallback.Factory.Create<BeforeUploadEventArgs>(this,
                (Func<BeforeUploadEventArgs, Task>)(async e => { await Wait(); e.CurrentRequest = headers; e.Cancel = true; })))
            .Add(x => x.BeforeRemove, EventCallback.Factory.Create<BeforeRemoveEventArgs>(this,
                (Func<BeforeRemoveEventArgs, Task>)(async e => { await Wait(); e.CurrentRequest = headers; e.Cancel = true; })))
            .Add(x => x.OnUploadStart, EventCallback.Factory.Create<UploadingEventArgs>(this,
                (Func<UploadingEventArgs, Task>)(async e => { await Wait(); e.CurrentRequest = headers; e.Cancel = true; })))
            .Add(x => x.OnRemove, EventCallback.Factory.Create<RemovingEventArgs>(this,
                (Func<RemovingEventArgs, Task>)(async e => { await Wait(); e.CurrentRequest = headers; e.Cancel = true; }))));
        async Task<string> RunBridge() => stage switch
        {
            "before-upload" => JsonSerializer.Serialize(await component.Instance.BeforeUploadEventAsync(new())),
            "before-remove" => JsonSerializer.Serialize(await component.Instance.BeforeRemoveEventAsync(new())),
            "upload-start" => JsonSerializer.Serialize(await component.Instance.UploadingEventAsync(new())),
            _ => JsonSerializer.Serialize(await component.Instance.RemovingEventAsync(new()))
        };
        Task<string>? pending = null;
        await component.InvokeAsync(() => { pending = RunBridge(); });
        try
        {
            await entered.Task.WaitAsync(TimeSpan.FromSeconds(5));
            Assert.NotNull(pending);
            Assert.False(pending!.IsCompleted);
        }
        finally { release.TrySetResult(true); }
        var result = await pending.WaitAsync(TimeSpan.FromSeconds(5));
        Assert.Contains("DUMMY_USER", result);
        Assert.Contains("\"cancel\":true", result);
    }

    private sealed class CountingHandler : HttpMessageHandler
    {
        public int Sends { get; private set; }
        public bool Disposed { get; private set; }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Sends++;
            throw new InvalidOperationException("No HTTP operation is permitted in managed characterization.");
        }
        protected override void Dispose(bool disposing) { Disposed = true; base.Dispose(disposing); }
    }
}
