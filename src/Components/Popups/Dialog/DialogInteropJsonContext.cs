using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Toolkit.Popups
{
    /// <summary>
    /// System.Text.Json source-generation context for the small, well-known data-transfer
    /// types exchanged between the dialog component and its JavaScript interop layer
    /// (for example the persisted position/size state stored in local storage).
    /// </summary>
    /// <remarks>
    /// Using a compile-time generated context keeps the (de)serialization of these types
    /// trim- and Native AOT-safe: it avoids the reflection-based serializer paths that raise
    /// IL2026 / IL3050 warnings, without hiding the requirement behind a suppression.
    /// </remarks>
    [JsonSourceGenerationOptions]
    [JsonSerializable(typeof(Dictionary<string, object>))]
    internal sealed partial class DialogInteropJsonContext : JsonSerializerContext
    {
    }
}
