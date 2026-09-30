using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// System.Text.Json source-generation context for the small, well-known data-transfer
    /// types exchanged between the chart component and its JavaScript interop layer.
    /// </summary>
    /// <remarks>
    /// Using a compile-time generated context keeps the (de)serialization of these types
    /// trim- and Native AOT-safe: it avoids the reflection-based serializer paths that raise
    /// IL2026 / IL3050 warnings, without hiding the requirement behind a suppression.
    /// </remarks>
    [JsonSourceGenerationOptions]
    [JsonSerializable(typeof(Dictionary<string, SymbolLocation>))]
    [JsonSerializable(typeof(List<SymbolLocation>))]
    [JsonSerializable(typeof(List<Size>))]
    [JsonSerializable(typeof(Size))]
    [JsonSerializable(typeof(string[]))]
    internal sealed partial class ChartInteropJsonContext : JsonSerializerContext
    {
    }
}
