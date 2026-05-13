using System.Collections;
using System.Dynamic;
using System.Reflection;
using Syncfusion.Blazor.Toolkit.Internal;

namespace Syncfusion.Blazor.Toolkit.Data
{
    /// <summary>
    /// Handles data operation in IEnumerable data source.
    /// </summary>
    public class BlazorAdaptor(DataManager dataManager) : AdaptorBase(dataManager)
    {
        /// <summary>
        /// Method to get the name of the adaptor.
        /// </summary>
        /// <returns>Returns the name of the adaptor.</returns>
        public override string GetName()
        {
            return nameof(BlazorAdaptor);
        }

        /// <summary>
        /// Sets a value indicating whether the operation should run synchronously one time.
        /// </summary>
        /// <param name="runSync">true to run the operation synchronously for a single execution; otherwise, false.</param>
        public override void SetRunSyncOnce(bool runSync)
        {
            RunSyncOnce = runSync;
        }

        /// <summary>
        /// Processes the specified data manager request and returns the result as an object.
        /// </summary>
        /// <param name="queries">The data manager request to process. Cannot be null.</param>
        /// <returns>An object representing the processed data manager request.</returns>
        public override object ProcessQuery(DataManagerRequest queries)
        {
            return SfBaseUtils.ChangeType(queries, typeof(DataManagerRequest))!;
        }

        /// <summary>
        /// Performs a data operation, such as filtering, sorting, or paging, on the provided data source using the
        /// specified query parameters.
        /// </summary>
        /// <remarks>If the query requires count information, the returned object is a DataResult
        /// containing both the result set and the total count. Otherwise, only the result set is returned. The method
        /// executes synchronously or asynchronously based on internal state, but callers should always await the
        /// returned task.</remarks>
        /// <typeparam name="T">The type of the data items in the data source.</typeparam>
        /// <param name="queries">An object representing the query parameters to apply to the data operation. Must be convertible to a
        /// DataManagerRequest.</param>
        /// <returns>A task that represents the asynchronous operation. The result contains either a DataResult object with the
        /// operation results and count information, or the operation result collection, depending on the query
        /// parameters.</returns>
        public override async Task<object> PerformDataOperation<T>(object queries)
        {
            IEnumerable DataSource = DataManager.Json; //Component data source should be propagated here.            
            DataManagerRequest query = (DataManagerRequest)SfBaseUtils.ChangeType(queries, typeof(DataManagerRequest))!;
            DataResult DataObject;
            /*
             * Using Task.Run will start a new thread and UI flashing will happens.
             * https://github.com/dotnet/aspnetcore/issues/15266
             */
            if (!RunSyncOnce)
            {
                DataObject = DataOperationInvoke<T>(DataSource, query);
                return query.RequiresCounts ? DataObject : DataObject.Result;
            }
            else
            {
                RunSyncOnce = false;
                DataObject = DataOperationInvoke<T>(DataSource, query);
                return await Task.FromResult(query.RequiresCounts ? DataObject : (object)DataObject.Result).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Performs data operation.
        /// </summary>
        /// <typeparam name="T">Type of the data source item.</typeparam>
        /// <param name="dataSource">Data source value.</param>
        /// <param name="queries">Query to be processed.</param>
        /// <returns>DataResult.</returns>
        public static DataResult DataOperationInvoke<T>(
            IEnumerable dataSource, DataManagerRequest queries)
        {
            DataResult dataResult = new();
            ArgumentNullException.ThrowIfNull(queries);

            bool isSortComparer = queries.Sorted?.Any(e => e.Comparer != null) ?? false;
            bool isGroupAggregate = queries.Aggregates?.Count > 0 &&
                                   queries.Group?.Count > 0 &&
                                   queries.LazyLoad;

            IEnumerable<object>? dataEnumerable = dataSource as IEnumerable<object>;
            IEnumerable? sortedDataSource = dataEnumerable;
            bool isSortingApplied = false;
            if (dataEnumerable == null || !dataEnumerable.Any())
            {
                dataResult.Result = dataEnumerable!;
                return dataResult;
            }

            // Handle dynamic object operations
            Type? elementType = dataSource?.GetElementType();
            if (dataSource?.GetType() == typeof(List<ExpandoObject>) ||
                typeof(T) == typeof(ExpandoObject) ||
                elementType == typeof(ExpandoObject) ||
                typeof(IDynamicMetaObjectProvider).IsAssignableFrom(elementType?.BaseType))
            {
                dataSource = DynamicObjectOperation.PerformDataOperations(dataSource!, queries);
                dataEnumerable = dataSource as IEnumerable<object>;
            }
            else
            {
                if (queries.Where?.Count > 0)
                {
                    dataSource = DataOperations.PerformFiltering(dataSource!, queries.Where, queries.Where[0].Operator);
                }

                if (queries.Search?.Count > 0)
                {
                    dataSource = DataOperations.PerformSearching(dataSource!, queries.Search);
                }

                if ((queries.Sorted?.Count > 0 && (!queries.LazyLoad || isSortComparer || isGroupAggregate)) ||
                    (queries.LazyLoad && (queries.Group == null || queries.Group?.Count == 0) && (!isSortComparer || !isGroupAggregate)))
                {
                    isSortingApplied = true;
                    sortedDataSource = DataOperations.PerformSorting(dataSource!, queries?.Sorted!);
                }

                dataEnumerable = dataSource as IEnumerable<object>;
            }

            if (queries!.RequiresFilteredRecords)
            {
                dataResult.FilteredRecords = dataEnumerable!;
            }

            dataResult.Count = dataEnumerable?.Count() ?? 0;
            if (isSortingApplied)
            {
                isSortingApplied = !isSortingApplied;
                dataSource = sortedDataSource!;
            }
            IEnumerable<object> aggregateData = dataEnumerable!;

            if (queries.Skip != 0)
            {
                dataSource = DataOperations.PerformSkip(dataSource!, queries.Skip);
            }

            if (queries.Take != 0)
            {
                dataSource = DataOperations.PerformTake(dataSource!, queries.Take);
            }

            if (queries.IdMapping != null && queries.Where != null)
            {
                dataSource = CollectChildRecords(dataSource!, queries);
            }

            if (queries.Aggregates?.Count > 0)
            {
                dataResult.Aggregates = DataUtil.PerformAggregation(aggregateData, queries.Aggregates);
            }

            if (queries.Group?.Count > 0 && queries.ServerSideGroup)
            {
                dataResult = GroupResult<T>(queries, dataSource!, dataResult);
                if (!queries.LazyLoad || isSortComparer || isGroupAggregate)
                {
                    return dataResult;
                }
            }

            if ((!isSortComparer || !isGroupAggregate) &&
                queries.Sorted?.Count > 0 &&
                queries.LazyLoad &&
                queries.Group?.Count > 0)
            {
                dataSource = dataResult.Result ?? dataSource!;
                dataSource = DataUtil.GroupSorting<T>(dataSource!, queries.Sorted);
                dataResult.Result = (dataSource as IEnumerable<object>)?.ToList()!;
            }
            else
            {
                dataResult.Result = (dataSource as IEnumerable<object>)?.ToList()!;
            }

            return dataResult;
        }

        /// <summary>
        /// Groups the provided data source according to the specified grouping criteria and aggregates, and returns the
        /// result in the given DataResult object.
        /// </summary>
        /// <remarks>If lazy loading is enabled in the queries parameter, only the first group is
        /// processed and the count is set accordingly. Otherwise, all groups are processed sequentially.</remarks>
        /// <typeparam name="T">The type of elements in the data source to be grouped.</typeparam>
        /// <param name="queries">The grouping and aggregation options to apply to the data source. Cannot be null.</param>
        /// <param name="DataSource">The collection of data to be grouped. Must implement IEnumerable.</param>
        /// <param name="DataObject">The DataResult object that will hold the grouped result and count. Cannot be null.</param>
        /// <returns>A DataResult object containing the grouped data and the total count after grouping.</returns>
        public static DataResult GroupResult<T>(DataManagerRequest queries, IEnumerable DataSource, DataResult DataObject)
        {
            ArgumentNullException.ThrowIfNull(queries);
            ArgumentNullException.ThrowIfNull(DataObject);
            if (!queries.LazyLoad)
            {
                foreach (string group in queries.Group)
                {
                    DataSource = DataUtil.Group<T>(DataSource, group, queries.Aggregates, 0, queries.GroupByFormatter, queries.LazyLoad, queries.LazyExpandAllGroup);
                }
            }
            else if (queries.LazyLoad)
            {
                DataSource = DataUtil.Group<T>(DataSource, queries.Group[0], queries.Aggregates, 0, queries.GroupByFormatter, queries.LazyLoad, queries.LazyExpandAllGroup);
                DataObject.Count = DataSource.Cast<object>().Count();
            }
            DataObject.Result = DataSource;
            return DataObject;
        }

        /// <summary>
        /// Performs data operation on child records.
        /// </summary>
        /// <param name="datasource">Data source value.</param>
        /// <param name="dm">Query to be processed.</param>
        /// <returns>IEnumerable.</returns>
        public static IEnumerable CollectChildRecords(IEnumerable datasource, DataManagerRequest dm)
        {
            if (datasource == null || dm == null) { return null!; }
            object? data = SfBaseUtils.ChangeType(datasource, datasource.GetType());
            IEnumerable DataSource = (IEnumerable)data!;
            string IdMapping = dm.IdMapping;
            object[] TaskIds = [];
            if (DataSource?.GetType() == typeof(List<ExpandoObject>) || DataSource?.GetElementType() == typeof(ExpandoObject))
            {
                foreach (ExpandoObject? rec in datasource.Cast<ExpandoObject>().ToList())
                {
                    IDictionary<string, object> propertyValues = rec!;
                    object taskid = propertyValues[IdMapping];
                    TaskIds = [.. TaskIds, taskid];
                }
            }
            else
            {
                foreach (object? rec in datasource)
                {
                    object taskid = rec?.GetType()?.GetProperty(IdMapping)?.GetValue(rec)!;
                    TaskIds = [.. TaskIds, taskid!];
                }
            }

            IEnumerable? ChildRecords = null;
            foreach (object id in TaskIds)
            {
                IEnumerable? records = null;
                dm.Where[0].value = id;
                records = DataSource?.GetType() == typeof(List<ExpandoObject>) || DataSource?.GetElementType() == typeof(ExpandoObject)
                    ? DynamicObjectOperation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator)
                    : DataOperations.PerformFiltering(DataSource!, dm.Where, dm.Where[0].Operator);

                ChildRecords = ChildRecords == null || (ChildRecords.AsQueryable().Count() == 0) ? records : ((IEnumerable<object>)ChildRecords).Concat((IEnumerable<object>)records);
            }

            if (ChildRecords != null)
            {
                ChildRecords = CollectChildRecords(ChildRecords, dm);
                if (dm.Sorted != null && dm.Sorted.Count > 0) // perform Sorting
                {
                    ChildRecords = DataSource?.GetType() == typeof(List<ExpandoObject>) || DataSource?.GetElementType() == typeof(ExpandoObject)
                        ? DynamicObjectOperation.PerformSorting(ChildRecords.AsQueryable(), dm.Sorted)
                        : DataOperations.PerformSorting(ChildRecords, dm.Sorted);
                }

                datasource = ((IEnumerable<object>)datasource).Concat((IEnumerable<object>)ChildRecords);
            }

            return datasource;
        }

        /// <inheritdoc/>
        public override async Task<object> ProcessResponse<T>(object data, DataManagerRequest queries)
        {
            if (queries != null && queries.RequiresCounts)
            {
                return queries.Group?.Count > 0
                    ? await Task.FromResult((data as DataResult<object>)!).ConfigureAwait(false)
                    : await Task.FromResult((data as DataResult<object>)!).ConfigureAwait(false);
            }
            else
            {
                if (queries?.Group?.Count > 0)
                {
                    bool isGroupAggregate = queries.Aggregates?.Count > 0;
                    return queries.LazyLoad && isGroupAggregate
                        ? await Task.FromResult((Group<T>)((IEnumerable)data).Cast<Group<T>>()).ConfigureAwait(false)
                        : (object)await Task.FromResult(((IEnumerable)data).Cast<Group<T>>().ToList()).ConfigureAwait(false);
                }

                return await Task.FromResult(((IEnumerable)data).Cast<T>().ToList()).ConfigureAwait(false);
            }
        }        
    }
}
