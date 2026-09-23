using System.Text.Json.Serialization;
using Syncfusion.Blazor.Toolkit.Data;

namespace Syncfusion.Blazor.Toolkit.Internal
{
    /// <summary>
    /// JSON serializer context for Syncfusion Blazor Toolkit types to enable source generation
    /// and improve AOT/trimming compatibility.
    /// </summary>
    [JsonSerializable(typeof(object))]
    [JsonSerializable(typeof(string))]
    [JsonSerializable(typeof(int))]
    [JsonSerializable(typeof(double))]
    [JsonSerializable(typeof(bool))]
    [JsonSerializable(typeof(DateTime))]
    [JsonSerializable(typeof(Dictionary<string, object>))]
    [JsonSerializable(typeof(Dictionary<string, string>))]
    [JsonSerializable(typeof(List<string>))]
    [JsonSerializable(typeof(DataManagerRequest))]
    [JsonSerializable(typeof(List<WhereFilter>))]
    [JsonSerializable(typeof(List<Sort>))]
    [JsonSerializable(typeof(List<Aggregate>))]
    [JsonSerializable(typeof(List<SearchFilter>))]
    [JsonSourceGenerationOptions(WriteIndented = true, PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
    internal partial class SyncfusionJsonContext : JsonSerializerContext
    {
    }
}