using System.Collections;
using System.Dynamic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Data;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Data
{
    public class NullableDataRegressionTests
    {
        [Theory]
        [InlineData(null, false, false)]
        [InlineData(null, true, false)]
        [InlineData("Missing", false, false)]
        [InlineData("Missing", true, false)]
        [InlineData(null, false, true)]
        [InlineData(null, true, true)]
        [InlineData("Missing", false, true)]
        [InlineData("Missing", true, true)]
        public void EnumerableSortedColumns_RecordComparerIgnoresField(string? field, bool descending, bool secondary)
        {
            var rows = new[] { new Record { Rank = 2 }, new Record { Rank = 1 } };
            var comparer = new RecordComparer();
            var columns = new List<SortedColumn>();
            if (secondary)
            {
                columns.Add(new SortedColumn { Field = nameof(Record.Group), Direction = SortOrder.Ascending });
            }
            columns.Add(new SortedColumn
            {
                Field = field,
                Direction = descending ? SortOrder.Descending : SortOrder.Ascending,
                Comparer = comparer
            });

            IEnumerable result = EnumerableOperation.PerformSorting(rows, columns);

            Assert.Equal(descending ? new[] { 2, 1 } : new[] { 1, 2 }, result.Cast<Record>().Select(row => row.Rank));
            Assert.True(comparer.Calls > 0);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("Missing")]
        public void EnumerableSortDescriptors_MissingKeysRetainInputOrder(string? field)
        {
            var rows = new[] { new Record { Rank = 2 }, new Record { Rank = 1 }, new Record { Rank = 3 } };
            var sort = new Sort { Direction = "ascending" };
            if (field != null)
            {
                sort.Name = field;
            }

            var result = EnumerableOperation.PerformSorting(rows, new List<Sort> { sort }).Cast<Record>().ToArray();

            Assert.Equal(rows, result);
        }

        [Theory]
        [InlineData(false, false, false)]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(true, true, false)]
        [InlineData(false, false, true)]
        [InlineData(true, false, true)]
        [InlineData(false, true, true)]
        [InlineData(true, true, true)]
        public void EnumerableDynamicSortedColumns_NullFieldFailsDuringExpressionConstruction(bool descending, bool customComparer, bool secondary)
        {
            var rows = new[] { Expando(2), Expando(1) };
            var columns = new List<SortedColumn>();
            if (secondary)
            {
                columns.Add(new SortedColumn { Field = "Group" });
            }
            var column = new SortedColumn { Direction = descending ? SortOrder.Descending : SortOrder.Ascending };
            if (customComparer)
            {
                column.Comparer = Comparer<object>.Default;
            }
            columns.Add(column);

            // This overload invokes an expression with an untyped null constant, not a null key selector.
            Assert.Throws<ArgumentException>(() => EnumerableOperation.PerformSorting(rows, columns));
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(true, true)]
        public void EnumerableDynamicSortedColumns_CustomComparerReceivesKeys(bool descending, bool secondary)
        {
            var rows = new[] { Expando(2), Expando(1) };
            var comparer = Comparer<object>.Create((left, right) => Assert.IsType<int>(left).CompareTo(Assert.IsType<int>(right)));
            var columns = new List<SortedColumn>();
            if (secondary)
            {
                columns.Add(new SortedColumn { Field = "Group" });
            }
            columns.Add(new SortedColumn
            {
                Field = "Rank", Direction = descending ? SortOrder.Descending : SortOrder.Ascending, Comparer = comparer
            });

            var result = EnumerableOperation.PerformSorting(rows, columns).Cast<ExpandoObject>().ToArray();

            Assert.Equal(descending ? rows : rows.Reverse(), result);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("Missing", false)]
        [InlineData(null, true)]
        [InlineData("Missing", true)]
        public void DynamicSorting_NullOrMissingFieldRetainsInputOrder(string? field, bool dynamicObject)
        {
            IDynamicMetaObjectProvider[] rows = dynamicObject
                ? new IDynamicMetaObjectProvider[] { new DynamicRecord(2), new DynamicRecord(1) }
                : new IDynamicMetaObjectProvider[] { Expando(2), Expando(1) };
            var sort = new Sort { Direction = "ascending" };
            if (field != null)
            {
                sort.Name = field;
            }

            var result = DynamicObjectOperation.PerformSorting(rows.AsQueryable(), new List<Sort> { sort });

            Assert.Equal(rows, result.Cast<IDynamicMetaObjectProvider>());
            var sortCall = Assert.IsAssignableFrom<MethodCallExpression>(result.Expression);
            Assert.Equal(typeof(Queryable), sortCall.Method.DeclaringType);
            Assert.Equal(nameof(Queryable.OrderBy), sortCall.Method.Name);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(true, true)]
        public void DynamicSorting_RecordComparerIgnoresNullField(bool descending, bool secondary)
        {
            var rows = new[] { Expando(2), Expando(1) };
            var comparer = Comparer<object>.Create((left, right) =>
                Rank(Assert.IsType<ExpandoObject>(left)).CompareTo(Rank(Assert.IsType<ExpandoObject>(right))));
            var sort = new Sort { Direction = descending ? "descending" : "ascending", Comparer = comparer };
            var columns = new List<Sort> { sort };
            if (secondary)
            {
                // The Sort overload reverses descriptors in place before building the query.
                columns.Add(new Sort { Name = "Group", Direction = "ascending" });
            }

            var result = DynamicObjectOperation.PerformSorting(rows.AsQueryable(), columns).Cast<ExpandoObject>().ToArray();

            Assert.Equal(descending ? rows : rows.Reverse(), result);
            Assert.Same(sort, columns[secondary ? 1 : 0]);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(true, true)]
        public void EnumerableDynamicSortedColumns_MissingFieldYieldsNullKeys(bool descending, bool secondary)
        {
            var rows = new[] { Expando(2), Expando(1) };
            var columns = new List<SortedColumn>();
            if (secondary)
            {
                columns.Add(new SortedColumn { Field = "Group" });
            }
            columns.Add(new SortedColumn
            {
                Field = "Missing", Direction = descending ? SortOrder.Descending : SortOrder.Ascending
            });

            var result = EnumerableOperation.PerformSorting(rows, columns);

            Assert.Equal(rows, result.Cast<ExpandoObject>());
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void EnumerableSortDescriptors_RecordComparerIgnoresNullField(bool descending)
        {
            var rows = new[] { new Record { Rank = 2 }, new Record { Rank = 1 } };
            var comparer = new RecordComparer();
            var columns = new List<Sort>
            {
                new() { Direction = descending ? "descending" : "ascending", Comparer = comparer }
            };

            var result = EnumerableOperation.PerformSorting(rows, columns).Cast<Record>().ToArray();

            Assert.Equal(descending ? rows : rows.Reverse(), result);
            Assert.True(comparer.Calls > 0);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Sorting_NullFieldValuesRemainNullKeys(bool dynamicSorting)
        {
            var rows = new[] { Expando(2), Expando(1), Expando(3) };
            ((IDictionary<string, object?>)rows[1])["Rank"] = null;
            var columns = new List<Sort> { new() { Name = "Rank", Direction = "ascending" } };

            IEnumerable result = dynamicSorting
                ? DynamicObjectOperation.PerformSorting(rows.AsQueryable(), columns)
                : EnumerableOperation.PerformSorting(rows, columns);

            Assert.Equal(new[] { rows[1], rows[0], rows[2] }, result.Cast<ExpandoObject>());
        }

        [Fact]
        public void DynamicFiltering_OptionalColumnTypesPreserveEmptyCriteria()
        {
            var rows = new[] { Expando(2), Expando(1) };
            var filters = new List<WhereFilter>();
            var parameter = Expression.Parameter(typeof(object));

            Assert.Null(DynamicObjectOperation.PredicateBuilder(rows, filters, "and", parameter, columnTypes: null));
            Assert.Equal(rows, DynamicObjectOperation.PerformFiltering(rows, filters, "and", columnTypes: null).Cast<ExpandoObject>());
        }

        [Fact]
        public async Task DataAdaptors_DefaultReadReturnsNullAndParentCanBeCleared()
        {
            using var adaptor = new EmptyAdaptor();
            using var genericAdaptor = new EmptyGenericAdaptor();
            IDataAdaptor[] adaptors = { adaptor, genericAdaptor };
            foreach (IDataAdaptor current in adaptors)
            {
                current.SetParent(null);
                Assert.Null(await current.ReadAsync(new DataManagerRequest()));
            }
            Assert.Null(adaptor._parent);
            Assert.Null(genericAdaptor.Parent);
        }

        [Fact]
        public async Task DataManager_RenderedParentRequiresPropertyChanges()
        {
            var parent = new DataBoundParent { IsRendered = true, PropertyChanges = null };
            var manager = new InitializableDataManager();
            using var client = new HttpClient { BaseAddress = new Uri("https://example.invalid/") };

            var error = await Assert.ThrowsAsync<InvalidOperationException>(() => manager.InitializeFor(parent, client));

            Assert.Contains("PropertyChanges", error.Message);
            Assert.Null(parent.PropertyChanges);
        }

        [Fact]
        public void DynamicSorting_ReadsKeysWhenEnumerated()
        {
            var rows = new[] { Expando(2), Expando(1) };
            var result = DynamicObjectOperation.PerformSorting(rows.AsQueryable(),
                new List<Sort> { new() { Name = "Rank", Direction = "ascending" } });

            ((IDictionary<string, object?>)rows[0])["Rank"] = 0;

            Assert.Equal(rows, result.Cast<ExpandoObject>());
        }

        [Fact]
        public void ForeignKeyComparer_SkipsNullKeysAndAcceptsNullRecordsAndDisplayValues()
        {
            var comparer = new ForeignKeySortManager(nameof(ForeignRecord.Id), nameof(ForeignRecord.Name), new object[]
            {
                new ForeignRecord { Id = null, Name = "Must not match" },
                new ForeignRecord { Id = 1, Name = null },
                new ForeignRecord { Id = 2, Name = "Beta" },
                new ForeignRecord { Id = 3, Name = "alpha" }
            });

            Assert.Equal(0, comparer.Compare(null, new ForeignRecord { Id = null }));
            Assert.Equal(0, comparer.Compare(new ForeignRecord { Id = 1 }, new ForeignRecord { Id = 99 }));
            Assert.Equal(0, comparer.Compare(new object(), null));
            Assert.True(comparer.Compare(null, new ForeignRecord { Id = 2 }) < 0);
            Assert.True(comparer.Compare(new ForeignRecord { Id = 2 }, null) > 0);
            Assert.True(comparer.Compare(new ForeignRecord { Id = 3 }, new ForeignRecord { Id = 2 }) < 0);

            comparer.Initialize(new List<Sort> { new() { Name = "Foreign.Id", Comparer = comparer } });
            Assert.Equal(0, comparer.Compare(new Record(), null));
            Assert.True(comparer.Compare(new Record { Foreign = new ForeignRecord { Id = 3 } },
                new Record { Foreign = new ForeignRecord { Id = 2 } }) < 0);
        }

        [Fact]
        public void GetDynamicValue_NullObjectAndMissingOrNullValuesReturnNull()
        {
            Assert.Null(EnumerableOperation.GetDynamicValue(null, "Rank"));
            Assert.Null(EnumerableOperation.GetDynamicValue(new Dictionary<string, object?>(), "Rank"));
            Assert.Null(EnumerableOperation.GetDynamicValue(new Dictionary<string, object?> { ["Rank"] = null }, "Rank"));
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task ExecuteQuery_RereadsAdaptorAfterAwait(bool remote)
        {
            var manager = new DataManager();
            var pending = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);
            var first = new LifecycleAdaptor(manager) { Remote = remote, Operation = _ => pending.Task };
            var second = new LifecycleAdaptor(manager) { Response = (_, _) => "replacement response" };
            SetParameters(manager, (nameof(DataManager.DataAdaptor), first));

            Task<object> query = manager.ExecuteQuery<Record>(new DataManagerRequest());
            Assert.False(query.IsCompleted);
            SetParameters(manager, (nameof(DataManager.DataAdaptor), second));
            pending.SetResult("operation result");

            Assert.Equal("replacement response", await query);
            Assert.Equal(0, first.ResponseCalls);
            Assert.Equal(1, second.ResponseCalls);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task ExecuteQuery_RereadsAdaptorAfterBeforeSendAndOfflineReplacement(bool offline)
        {
            var manager = SetParameters(new DataManager(), (nameof(DataManager.Offline), offline));
            var rows = new[] { new Record { Rank = 2 }, new Record { Rank = 1 } };
            var replacement = new LifecycleAdaptor(manager) { Response = (_, _) => rows };
            var remote = new LifecycleAdaptor(manager)
            {
                Remote = true,
                Sending = _ => SetParameters(manager, (nameof(DataManager.DataAdaptor), replacement))
            };
            SetParameters(manager, (nameof(DataManager.DataAdaptor), remote));
            Assert.Equal(offline, manager.Offline);

            object result = await manager.ExecuteQuery<Record>(new DataManagerRequest { Take = 1 });

            Assert.Equal(0, remote.OperationCalls);
            Assert.Equal(1, replacement.OperationCalls);
            Assert.Equal(1, replacement.ResponseCalls);
            if (offline)
            {
                Assert.IsType<BlazorAdaptor>(manager.DataAdaptor);
                Assert.Same(rows, manager.Json);
                Assert.Same(rows[0], Assert.Single(Assert.IsAssignableFrom<IEnumerable>(result).Cast<Record>()));
            }
            else
            {
                Assert.Same(rows, result);
                Assert.Same(replacement, manager.DataAdaptor);
            }
        }

        // Apply standalone manager parameters without running initialization or rendering.
        private static DataManager SetParameters(DataManager manager, params (string Name, object? Value)[] parameters)
        {
            ParameterView.FromDictionary(parameters.ToDictionary(parameter => parameter.Name, parameter => parameter.Value))
                .SetParameterProperties(manager);
            return manager;
        }

        private static ExpandoObject Expando(int rank)
        {
            var row = new ExpandoObject();
            var values = (IDictionary<string, object?>)row;
            values["Rank"] = rank;
            values["Group"] = 0;
            return row;
        }

        private static int Rank(ExpandoObject row) => Assert.IsType<int>(((IDictionary<string, object?>)row)["Rank"]);

        private sealed class Record
        {
            public int Rank { get; set; }
            public int Group { get; set; }
            public ForeignRecord? Foreign { get; set; }
        }

        private sealed class ForeignRecord
        {
            public int? Id { get; set; }
            public string? Name { get; set; }
        }

        private sealed class RecordComparer : IComparer<object>
        {
            public int Calls { get; private set; }

            public int Compare(object? left, object? right)
            {
                Calls++;
                return Assert.IsType<Record>(left).Rank.CompareTo(Assert.IsType<Record>(right).Rank);
            }
        }

        private sealed class DynamicRecord(int rank) : DynamicObject
        {
            public override bool TryGetMember(GetMemberBinder binder, out object? result)
            {
                result = binder.Name == "Rank" ? rank : null;
                return binder.Name == "Rank";
            }
        }

        private sealed class EmptyAdaptor : DataAdaptor { }

        private sealed class EmptyGenericAdaptor : DataAdaptor<object> { }

        private sealed class DataBoundParent : SfDataBoundComponent { }

        private sealed class InitializableDataManager : DataManager
        {
            public Task InitializeFor(SfDataBoundComponent parent, HttpClient client)
            {
                Parent = parent;
                HttpClientInstance = client;
                return OnInitializedAsync();
            }
        }

        private sealed class LifecycleAdaptor(DataManager manager) : AdaptorBase(manager)
        {
            public bool Remote { get; set; }
            public Action<HttpRequestMessage>? Sending { get; set; }
            public Func<object, Task<object>> Operation { get; set; } = _ => Task.FromResult<object>("operation result");
            public Func<object, DataManagerRequest, object> Response { get; set; } = (data, _) => data;
            public int OperationCalls { get; private set; }
            public int ResponseCalls { get; private set; }

            public override bool IsRemote() => Remote;

            public override object ProcessQuery(DataManagerRequest queries) => Remote
                ? new RequestOptions { Url = "https://example.invalid/data", RequestMethod = HttpMethod.Get }
                : queries;

            public override void BeforeSend(HttpRequestMessage request) => Sending?.Invoke(request);

            public override Task<object> PerformDataOperation<T>(object queries)
            {
                OperationCalls++;
                return Operation(queries);
            }

            public override Task<object> ProcessResponse<T>(object data, DataManagerRequest queries)
            {
                ResponseCalls++;
                return Task.FromResult(Response(data, queries));
            }
        }
    }
}
