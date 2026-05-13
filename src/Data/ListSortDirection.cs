using System.Text.Json.Serialization;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Syncfusion.Blazor.Toolkit.Data
{
    /// <summary>
    ///  Specifies the direction of a sort operation.
    /// </summary>
    public enum ListSortDirection
    {
        /// <summary>
        /// Sorts in ascending order.
        /// </summary>
        Ascending = 0,

        /// <summary>
        /// Sorts in descending order.
        /// </summary>
        Descending = 1,
    }

    /// <summary>
    /// Sepcifies the sort order.
    /// </summary>
    public enum SortOrder
    {
        /// <summary>
        /// No sort order.
        /// </summary>
        [EnumMember(Value = "None")]
        None,

        /// <summary>
        /// Sorts in ascending order.
        /// </summary>
        [EnumMember(Value = "Ascending")]
        Ascending,

        /// <summary>
        /// Sorts in descending order.
        /// </summary>
        [EnumMember(Value = "Descending")]
        Descending,
    }

    /// <summary>
    /// Defines the sort column.
    /// </summary>
    public class SortedColumn
    {

        /// <summary>
        /// Specifies the field to sort.
        /// </summary>
        [JsonPropertyName("field")]
        [DefaultValue(null)]
        public string? Field { get; set; }

        /// <summary>
        /// Specifies the sort order.
        /// </summary>
        [JsonPropertyName("direction")]
        [DefaultValue(SortOrder.Ascending)]
        [JsonConverter(typeof(JsonStringEnumConverter))]

        public SortOrder Direction { get; set; } = SortOrder.Ascending;

        /// <summary>
        /// Gets the sort comparer
        /// </summary>
        public object? Comparer { get; set; }
    }
}
