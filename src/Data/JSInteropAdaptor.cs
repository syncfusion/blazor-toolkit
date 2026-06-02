using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Toolkit.Data
{
    /// <summary>
    /// An interface for implementing JSInteropAdaptor.
    /// </summary>
    public interface IJSInteropAdaptor : IDisposable
    {
        /// <summary>
        /// Initializes the adaptor. Implementations should create any required
        /// DotNetObjectReference instances used for JavaScript interop.
        /// </summary>
        void Init();

        /// <summary>
        /// Creates and returns a new <see cref="DotNetObjectReference{T}"/> for the
        /// adaptor instance.
        /// </summary>
        DotNetObjectReference<object> Create();

        /// <summary>
        /// Gets the existing <see cref="DotNetObjectReference{T}"/> if available;
        /// otherwise creates and returns a new one.
        /// </summary>
        DotNetObjectReference<object> GetRef();
    }

    /// <summary>
    /// Custom handler of JSInterop to invoke the JavaScript methods with DotNetObjectReference.
    /// </summary>
    public class JSInteropAdaptor : ComponentBase, IJSInteropAdaptor
    {
        /// <inheritdoc/>
        public void Init()
        {
            DotnetRef = Create();
        }

        /// <summary>
        /// Holds the cached DotNetObjectReference for this adaptor instance.
        /// </summary>
        /// <exclude />
        private DotNetObjectReference<object>? DotnetRef { get; set; }

        /// <inheritdoc/>
        public virtual DotNetObjectReference<object> Create()
        {
            return DotNetObjectReference.Create<object>(this);
        }

        /// <inheritdoc/>
        public DotNetObjectReference<object> GetRef()
        {
            return DotnetRef ?? Create();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases the <see cref="DotNetObjectReference{T}"/> held by this adaptor.
        /// </summary>
        /// <param name="disposing">When <c>true</c>, disposes the underlying DotNetObjectReference.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                DotnetRef?.Dispose();
            }
        }
    }

    /// <summary>
    /// Internal utility class for Syncfusion JavaScript interop calls.
    /// </summary>
    /// <exclude />
    internal static class SyncfusionInterop
    {
        internal static async ValueTask<T> HandleInteropCallAsync<T>(IJSRuntime jsRuntime, Func<ValueTask<T>> jsInteropCall, string nameSpace, string elementId = "")
        {
            try
            {
                return await jsInteropCall().ConfigureAwait(true);
            }
            catch (JSException e)
            {
                string issue = $"{nameSpace} - #{elementId} - Interop call failed: {e.Message}\n";
                return await LogError<T>(jsRuntime, e, issue).ConfigureAwait(true);
            }
        }

        internal static async ValueTask<T> InitAsync<T>(
            IJSRuntime jsRuntime,
            string elementId,
            object model,
            string[] events,
            string nameSpace,
            DotNetObjectReference<object> helper,
            string bindableProps,
            Dictionary<string, object>? htmlAttributes = null,
            Dictionary<string, object>? templateRefs = null,
            DotNetObjectReference<object>? adaptor = null
        )
        {
            return await HandleInteropCallAsync(jsRuntime, () =>
                jsRuntime.InvokeAsync<T>("sfBlazor.initialize", elementId, model, events, nameSpace, helper, bindableProps, htmlAttributes, templateRefs, adaptor),
                nameSpace, elementId).ConfigureAwait(true);
        }


        internal static async ValueTask<T> UpdateAsync<T>(IJSRuntime jsRuntime, string elementId, string model, string nameSpace)
        {
            return await HandleInteropCallAsync(jsRuntime, () =>
                jsRuntime.InvokeAsync<T>("sfBlazor.setModel", elementId, model, nameSpace),
                nameSpace, elementId).ConfigureAwait(true);
        }

        internal static async ValueTask<T> InvokeMethodAsync<T>(IJSRuntime jsRuntime, string elementId, string methodName, string moduleName, object[] args, string nameSpace, ElementReference? element = null)
        {
            return await HandleInteropCallAsync(jsRuntime, () =>
                jsRuntime.InvokeAsync<T>("sfBlazor.invokeMethod", elementId, methodName, moduleName, JsonSerializer.Serialize(args), element),
                nameSpace, elementId).ConfigureAwait(true);
        }

        internal static ValueTask<T> LogError<T>(IJSRuntime jsRuntime, Exception e, string message = "")
        {
            try
            {
                ErrorMessage error = new()
                {
                    Message = message + e.Message,
                    Stack = e.StackTrace
                };
                if (e.InnerException != null)
                {
                    error.Message = message + e.InnerException.Message;
                    error.Stack = e.InnerException.StackTrace;
                }

                return jsRuntime.InvokeAsync<T>("sfBlazor.throwError", error);
            }
            catch (JSException)
            {
                return default;
            }
        }
    }

    /// <summary>
    /// Internal DTO for serializing JavaScript error payloads.
    /// </summary>
    /// <exclude />
    internal class ErrorMessage
    {
        [JsonPropertyName("message")]
        internal string? Message { get; set; }

        [JsonPropertyName("stack")]
        internal string? Stack { get; set; }
    }
}
