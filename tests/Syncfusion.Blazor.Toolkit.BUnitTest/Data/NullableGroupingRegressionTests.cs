using System.Collections;
using System.Dynamic;
using System.Linq.Expressions;
using Syncfusion.Blazor.Toolkit.Data;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Data
{
    public class NullableGroupingRegressionTests
    {
        [Fact]
        public void ValueHelpers_PreserveNullMissingAndIdentityResults()
        {
            var row = new Record();

            Assert.Null(DataUtil.GetVal(Array.Empty<Record>(), 0, nameof(Record.Key)));
            Assert.Null(DataUtil.GetVal(new[] { row }, 0, nameof(Record.Key)));
            Assert.Null(DataUtil.GetVal(new[] { row }, 0, "Missing"));
            Assert.Same(row, DataUtil.GetVal(new[] { row }, 0, null));
            Assert.Throws<IndexOutOfRangeException>(() => DataUtil.GetVal(new[] { row }, 1, null));
            Assert.Null(DataUtil.GetGroupValue(nameof(Record.Key), row));
            Assert.Null(DataUtil.GetGroupValue("Missing", row));
            Assert.Same(row, DataUtil.GetGroupValue(null, row));
            Assert.Null(DataUtil.GetGroupValue(null, null));
            Assert.Null(DataUtil.GetGroupValue(nameof(Record.Key), null));
            Assert.Null(DataUtil.GetObject(nameof(Record.Key), null));
        }

        [Fact]
        public void Distinct_PreservesNullSentinelAndMissingExpandoKeyBehavior()
        {
            var missing = new ExpandoObject();
            var firstNull = Expando(null);
            var secondNull = Expando(null);
            var literalNull = Expando("null");
            var value = Expando("value");
            var rows = new[] { missing, firstNull, secondNull, literalNull, value };

            var result = DataUtil.GetDistinct(rows, nameof(Record.Key)).ToArray();

            // Missing expando keys are excluded; actual null and the existing "null" sentinel share a key.
            Assert.Equal(new[] { firstNull, value }, result);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void GroupByMany_CustomComparerReceivesNullKeysAndRemainsDeferred(bool descending)
        {
            var rows = new[] { new Record { Key = "b" }, new Record(), new Record { Key = "a" }, new Record() };
            var comparer = new NullLastComparer();
            var comparers = new Dictionary<string, IComparer<object?>> { [nameof(Record.Key)] = comparer };
            var sortFields = new List<SortDescription>
            {
                new(nameof(Record.Key), descending ? ListSortDirection.Descending : ListSortDirection.Ascending)
            };
            int reads = 0;
            IEnumerable<Func<Record, object?>> selectors = new Func<Record, object?>[]
            {
                row => { reads++; return row.Key; }
            };

            var result = rows.GroupByMany(sortFields, comparers, new[] { nameof(Record.Key) }, selectors);

            Assert.Equal(0, reads);
            Assert.Equal(0, comparer.Calls);
            rows[0].Key = "0";
            var groups = result.ToArray();

            Assert.Equal(rows.Length, reads);
            Assert.Equal(descending ? new object?[] { null, "a", "0" } : new object?[] { "0", "a", null },
                groups.Select(group => group.Key));
            Assert.True(comparer.SawNull);
            var nullGroup = Assert.Single(groups.Where(group => group.Key == null));
            Assert.Equal(2, nullGroup.Count);
            Assert.Equal(new[] { rows[1], rows[3] }, Assert.IsAssignableFrom<IEnumerable>(nullGroup.Items).Cast<Record>());
            Assert.All(groups, group => Assert.Null(group.SubGroups));
            Assert.Null(rows.GroupByMany(sortFields, comparers, new List<string>(), Array.Empty<Func<Record, object?>>()));
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void GroupByMany_ReflectedComparerOverloadPreservesNullKeys(bool descending)
        {
            var rows = new List<Record> { new() { Key = "a" }, new() };
            var comparer = new NullLastComparer();
            var sortFields = new List<SortDescription>
            {
                new(nameof(Record.Key), descending ? ListSortDirection.Descending : ListSortDirection.Ascending)
            };
            var comparers = new Dictionary<string, IComparer<object?>> { [nameof(Record.Key)] = comparer };
            Expression<Func<string, Record, object?>> selector = (field, row) => row.Key;

            var result = ((IEnumerable)rows).GroupByMany(typeof(Record), sortFields, comparers, _ => selector, nameof(Record.Key));

            Assert.Equal(0, comparer.Calls);
            Assert.Equal(descending ? new object?[] { null, "a" } : new object?[] { "a", null },
                result.Select(group => group.Key));
            Assert.True(comparer.SawNull);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void QueryableTypeInference_NullOrMissingFirstValueFailsWithoutSkipping(bool expando, bool missing)
        {
            IQueryable rows = expando
                ? new[] { missing ? new ExpandoObject() : Expando(null), Expando("later") }.AsQueryable()
                : new[] { new DynamicRecord(null, missing), new DynamicRecord("later") }.AsQueryable();
            var filters = new List<WhereFilter>
            {
                new() { Field = nameof(Record.Key), Operator = "equal", value = "later" }
            };

            if (expando)
            {
                Assert.Throws<ArgumentNullException>(() => QueryableOperation.PerformFiltering(rows.Cast<ExpandoObject>(), filters, "and"));
            }
            else
            {
                var error = Assert.Throws<InvalidOperationException>(() =>
                    QueryableOperation.PerformFiltering(rows.Cast<DynamicRecord>(), filters, "and"));
                Assert.Contains(nameof(Record.Key), error.Message);
            }
        }

        private static ExpandoObject Expando(object? key)
        {
            var row = new ExpandoObject();
            ((IDictionary<string, object?>)row)[nameof(Record.Key)] = key;
            return row;
        }

        public sealed class Record
        {
            public object? Key { get; set; }
        }

        private sealed class NullLastComparer : IComparer<object?>
        {
            public int Calls { get; private set; }
            public bool SawNull { get; private set; }

            public int Compare(object? left, object? right)
            {
                Calls++;
                SawNull |= left == null || right == null;
                return left == null ? right == null ? 0 : 1
                    : right == null ? -1 : StringComparer.Ordinal.Compare(Assert.IsType<string>(left), Assert.IsType<string>(right));
            }
        }

        private sealed class DynamicRecord(object? key, bool missing = false) : DynamicObject
        {
            public override bool TryGetMember(GetMemberBinder binder, out object? result)
            {
                result = binder.Name == nameof(Record.Key) && !missing ? key : null;
                return binder.Name == nameof(Record.Key) && !missing;
            }
        }
    }
}
