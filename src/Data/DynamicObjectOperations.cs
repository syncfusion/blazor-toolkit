using System.Collections;
using System.Linq.Expressions;
using Syncfusion.Blazor.Toolkit.Data;
using System.Dynamic;
using System.Reflection;

namespace Syncfusion.Blazor.Toolkit.Data
{
    /// <summary>
    /// DataOperation class that performs data operation in DynamicObject type data sources.
    /// </summary>
    public static class DynamicObjectOperation
    {
        /// <summary>
        /// Executes the query against the given data source and returns the resultant records.
        /// </summary>
        /// <param name="DataSource">Input data source.</param>
        /// <param name="queries">Query to be executed against data source.</param>
        /// <returns>IEnumerable - resultant records.</returns>
        public static IEnumerable PerformDataOperations(IEnumerable DataSource, DataManagerRequest queries)
        {
            IDictionary<string, Type> columnTypes = DataUtil.GetColumnType(DataSource);
            Type sourceType = DataSource.GetElementType();
            if (sourceType == null)
            {
                Type type1 = sourceType!.GetType();
                sourceType = type1.GetElementType()!;
            }

            if (queries != null && queries.Where?.Count > 0) // perform Filtering
            {
                DataSource = PerformFiltering(DataSource, queries.Where, queries.Where[0].Operator, columnTypes);
            }

            if (queries != null && queries.Where?.Count > 0 && queries.Search?.Count > 0)
            {
                MethodInfo? methodInfo = typeof(Enumerable).GetMethod(nameof(Enumerable.Cast));
                MethodInfo? genericMethod = methodInfo?.MakeGenericMethod(sourceType);
                DataSource = (genericMethod?.Invoke(null, [DataSource]) as IEnumerable)!;

            }

            if (queries != null && queries.Search?.Count > 0) // perform Searching
            {
                DataSource = PerformSearching(DataSource, queries.Search, columnTypes);
            }

            if (queries != null && queries.Sorted?.Count > 0) // perform Sorting
            {
                DataSource = PerformSorting(DataSource.AsQueryable(), queries.Sorted);
            }

            return DataSource;
        }

        /// <summary>
        /// Sorts the given data source.
        /// </summary>
        /// <param name="dataSource">Input data source to be sorted.</param>
        /// <param name="sortedColumns">List of sort criteria.</param>
        /// <returns>IQuerable.</returns>
        public static IQueryable PerformSorting(IQueryable dataSource, List<Sort> sortedColumns)
        {
            bool firstTime = true;
            IQueryable<IDynamicMetaObjectProvider> dt = dataSource.Cast<IDynamicMetaObjectProvider>().AsQueryable();
            IOrderedQueryable<IDynamicMetaObjectProvider> data;
            Type sourceType = dt.GetObjectType();
            List<SortedColumn> sortedColumn = [];
            if (sortedColumns != null && sortedColumns.Count > 1)
            {
                sortedColumns.Reverse();
            }

            foreach (Sort column in sortedColumns ?? [])
            {
                SortOrder direction = Enum.Parse<SortOrder>(column.Direction.ToString(), true);
                sortedColumn.Add(new SortedColumn { Direction = direction, Field = column.Name, Comparer = column.Comparer });
            }

            List<object> dataSourceList = [.. dataSource.Cast<object>()];
            if (dataSourceList.Count == 0)
            {
                return dt;
            }

            bool isDynamicObjectType = dataSourceList[0].GetType().IsSubclassOf(typeof(DynamicObject));
            bool isExpandoObjectType = dataSourceList[0].GetType() == typeof(ExpandoObject);
            if (!isDynamicObjectType && !isExpandoObjectType)
            {
                return dt;
            }

            foreach (SortedColumn column in sortedColumn)
            {
                if (column.Direction == SortOrder.Ascending)
                {
                    if (firstTime)
                    {
                        dt = column.Comparer != null
                            ? dt.OrderBy(column.Field, (column.Comparer as IComparer<object>)!, sourceType).Cast<IDynamicMetaObjectProvider>().AsQueryable()
                            : dt.OrderBy(x => ReflectionExtension.GetValueFromIDynamicMetaObject(x, column.Field, true));
                        firstTime = false;
                    }
                    else
                    {
                        data = (IOrderedQueryable<IDynamicMetaObjectProvider>)dt;
                        dt = column.Comparer != null
                            ? data.ThenBy(column.Field, (column.Comparer as IComparer<object>)!, sourceType).Cast<IDynamicMetaObjectProvider>().AsQueryable()
                            : data.ThenBy(x => ReflectionExtension.GetValueFromIDynamicMetaObject(x, column.Field, true));
                    }
                }
                else
                {
                    if (firstTime)
                    {
                        dt = column.Comparer != null
                            ? dt.OrderByDescending(column.Field, (column.Comparer as IComparer<object>)!, sourceType).Cast<IDynamicMetaObjectProvider>().AsQueryable()
                            : dt.OrderByDescending(x => ReflectionExtension.GetValueFromIDynamicMetaObject(x, column.Field, true));
                        firstTime = false;
                    }
                    else
                    {
                        data = (IOrderedQueryable<IDynamicMetaObjectProvider>)dt;
                        dt = column.Comparer != null
                            ? data.ThenByDescending(column.Field, (column.Comparer as IComparer<object>)!, sourceType).Cast<IDynamicMetaObjectProvider>().AsQueryable()
                            : data.ThenByDescending(x => ReflectionExtension.GetValueFromIDynamicMetaObject(x, column.Field, true));
                    }
                }
            }

            return dt;
        }

        /// <summary>
        /// Apply the given filter criteria against the data source and returns the filtered records.
        /// </summary>
        /// <param name="dataSource">Input data source.</param>
        /// <param name="whereFilter">List of filter criteria.</param>
        /// <param name="condition">Condition to merge two filter criteria.</param>
        /// <param name="columnTypes">Type collection of each property in data source.</param>
        /// <returns></returns>
        public static IQueryable PerformFiltering(IEnumerable dataSource, List<WhereFilter> whereFilter, string condition, IDictionary<string, Type> columnTypes = null)
        {
            IQueryable<IDynamicMetaObjectProvider> data = dataSource.Cast<IDynamicMetaObjectProvider>().AsQueryable();
            ParameterExpression paramExpression = Expression.Parameter(typeof(object));
            Expression predicate = PredicateBuilder(dataSource, whereFilter, condition, paramExpression, columnTypes);
            if (predicate == null)
            {
                return data;
            }

            Expression<Func<IDynamicMetaObjectProvider, bool>> bExp = Expression.Lambda<Func<IDynamicMetaObjectProvider, bool>>(predicate, paramExpression);
            data = data.Where(bExp);
            return data;
        }

        private static Type GetColumnType(IEnumerable dataSource, string filterString, bool nullable = true)
        {
            _ = nullable;
            List<IDynamicMetaObjectProvider>? dataSourceList = [.. dataSource.Cast<IDynamicMetaObjectProvider>()];
            if (dataSourceList?.Count == 0)
            {
                return null!;
            }
            Type? rowType = dataSourceList?[0].GetType();

            IDynamicMetaObjectProvider? rowData = dataSourceList?[0];
            object? propertyValue = ReflectionExtension.GetValueFromIDynamicMetaObject(rowData!, filterString, true);
            if (propertyValue == null)
            {
                rowData = dataSourceList?.Where(x => ReflectionExtension.GetValueFromIDynamicMetaObject(x, filterString, true) != null).FirstOrDefault();
            }
            return propertyValue?.GetType()!;
        }

        private static Type ColumnType(IDictionary<string, Type> columns, string field, object data = null!)
        {
            Type? type = null;
            string[] Fields = field.Split('.');
            IDictionary<string, Type>? dynamicType;
            int Complex = Fields.Length;

            if (Complex > 1)
            {
                for (int i = 0; i < Complex; i++)
                {
                    if (data is not null and ExpandoObject)
                    {
                        if (i != 0)
                        {
                            object? customData = data.GetType().GetProperty(Fields[i - 1])?.GetValue(data);
                            customData ??= DataUtil.GetExpandoValue((IDictionary<string, object>)data, Fields[i - 1]);
                            dynamicType = customData != null ? DataUtil.GetColumnType(new List<object>() { customData }, true) : null;
                            if (dynamicType != null && dynamicType.TryGetValue(Fields[i], out Type? value))
                            {
                                type = value;
                            }
                            data = customData!;
                        }
                        else
                        {
                            IDictionary<string, object> expandoData = (IDictionary<string, object>)data;

                            dynamicType = DataUtil.GetColumnType(new List<object>() { expandoData[Fields[i]] }, true);
                            type = dynamicType[Fields[i + 1]];
                        }
                    }
                }
            }
            else
            {
                type = columns.TryGetValue(field, out Type? value) ? value : null;
            }

            return type!;
        }

        /// <summary>
        /// Apply the given search criteria against the data source and returns the filtered records.
        /// </summary>
        /// <param name="dataSource">Data source to be filtered.</param>
        /// <param name="searchFilter">List of search criteria.</param>
        /// <returns>IEnumerable - searched records.</returns>
        /// <param name="columnTypes">Type collection of each property in data source.</param>
        public static IQueryable PerformSearching(IEnumerable dataSource, List<SearchFilter> searchFilter, IDictionary<string, Type>? columnTypes = null)
        {
            IQueryable<IDynamicMetaObjectProvider>? data = null;
            Type? type = dataSource.GetElementType();
            Type t = typeof(object);
            if (type == null)
            {
                Type? type1 = dataSource?.GetType();
                type = type1?.GetElementType();
            }

            foreach (SearchFilter filter in searchFilter ?? [])
            {
                ParameterExpression paramExpression = Expression.Parameter(typeof(object));
                bool initialLoop = true;
                Expression? predicate = null;
                string op = filter.Operator;
                if (op == "equal")
                {
                    op = "equals";
                }
                else if (op == "notequal")
                {
                    op = "notequals";
                }

                FilterType FilterType = Enum.Parse<FilterType>(op.ToString(), true);
                foreach (string fields in filter.Fields)
                {
                    Type? columnType = columnTypes != null
                        ? ColumnType(columnTypes, fields, (dataSource?.Cast<ExpandoObject>()?.ToList()?.ElementAt(0))!)
                        : GetColumnType(dataSource!, fields);
                    if (initialLoop)
                    {
                        predicate = dataSource?.AsQueryable().Predicate(paramExpression, fields, filter.Key, FilterType, FilterBehavior.StringTyped, false, type!, columnType);
                        initialLoop = false;
                    }
                    else if (columnType != null)
                    {
                        predicate = predicate?.OrPredicate(dataSource?.AsQueryable()?.Predicate(paramExpression, fields, filter.Key, FilterType, FilterBehavior.StringTyped, false, type!, columnType)!);
                    }
                }

                Expression<Func<IDynamicMetaObjectProvider, bool>> query = Expression.Lambda<Func<IDynamicMetaObjectProvider, bool>>(predicate!, paramExpression);

                IQueryable<IDynamicMetaObjectProvider>? dt = dataSource?.AsQueryable()?.Cast<IDynamicMetaObjectProvider>();
                data = dt!.AsQueryable().Where(query);
            }

            return data!;
        }

        /// <summary>
        /// Generates predicate from the filter criteria.
        /// </summary>
        /// <param name="dataSource">Data source to be filtered.</param>
        /// <param name="whereFilter">List of filter criteria.</param>
        /// <param name="condition">Condition to merge two filter criteria.</param>
        /// <param name="paramExpression">Parameter expression.</param>
        /// <param name="columnTypes">Type collection of each property in data source.</param>
        /// <returns>Expression.</returns>
        public static Expression PredicateBuilder(IEnumerable dataSource, List<WhereFilter> whereFilter, string condition, ParameterExpression paramExpression, IDictionary<string, Type> columnTypes = null)
        {
            Type? type = dataSource.GetElementType();
            if (type == null)
            {
                Type? type1 = dataSource?.GetType();
                type = type1?.GetElementType();
            }

            Expression? predicate = null;
            foreach (WhereFilter filter in whereFilter ?? [])
            {
                if (filter.IsComplex)
                {
                    predicate = predicate == null
                        ? PredicateBuilder(dataSource!, filter.Predicates, filter.Condition, paramExpression, columnTypes!)
                        : condition == "or"
                            ? predicate.OrElsePredicate(PredicateBuilder(dataSource!, filter.Predicates, filter.Condition, paramExpression, columnTypes!))
                            : predicate.AndAlsoPredicate(PredicateBuilder(dataSource!, filter.Predicates, filter.Condition, paramExpression, columnTypes!));
                }
                else
                {
                    string filterOperator = filter.Operator;
                    if (filterOperator == "equal")
                    {
                        filterOperator = "equals";
                    }
                    else if (filterOperator == "notequal")
                    {
                        filterOperator = "notequals";
                    }
                    else if (filterOperator == "none")
                    {
                        continue;
                    }

                    FilterType filterType = Enum.Parse<FilterType>(filterOperator.ToString(), true);
                    object value = filter.value;
                    Type? columnType;
                    if (columnTypes != null)
                    {
                        // TODOComplex: check the datasource while parsing
                        columnType = ColumnType(columnTypes, filter.Field, dataSource?.Cast<IDynamicMetaObjectProvider>()?.ToList()?.ElementAt(0)!);
                    }
                    else
                    {
                        columnType = GetColumnType(dataSource!, filter.Field);
                    }

                    if (columnType == null)
                    {
                        return predicate!;
                    }

                    predicate = predicate == null
                        ? (dataSource?.AsQueryable().Predicate(paramExpression, filter.Field, value, filterType, FilterBehavior.StringTyped, !filter.IgnoreCase, type!, columnType))
                        : condition == "or"
                            ? predicate.OrElsePredicate(dataSource?.AsQueryable()?.Predicate(paramExpression, filter.Field, value, filterType, FilterBehavior.StringTyped, !filter.IgnoreCase, type!, columnType)!)
                            : predicate.AndPredicate(dataSource?.AsQueryable()?.Predicate(paramExpression, filter.Field, value, filterType, FilterBehavior.StringTyped, !filter.IgnoreCase, type!, columnType)!);
                }
            }

            return predicate!;
        }
    }
}
