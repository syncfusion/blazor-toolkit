using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using Syncfusion.Blazor.Toolkit.Internal;
using System.ComponentModel;
using System.Dynamic;

namespace Syncfusion.Blazor.Toolkit.Data
{
    /// <summary>
    ///
    /// </summary>
    public static class EnumerableOperation
    {
        /// <summary>
        /// Executes the query against the given data source and returns the resultant records.
        /// </summary>
        /// <param name="dataSource">Input data source against which the query to be executed.</param>
        /// <param name="manager">Query to be executed.</param>
        /// <returns>IEnumerable - resultant records.</returns>
        public static IEnumerable Execute(IEnumerable dataSource, DataManagerRequest manager)
        {
            if (manager == null) { return dataSource; }
            if (manager.Where != null && manager.Where.Count > 0)
            {
                dataSource = PerformFiltering(dataSource, manager.Where, manager.Where[0].Operator);
            }

            if (manager.Search != null && manager.Search.Count > 0)
            {
                dataSource = PerformSearching(dataSource, manager.Search);
            }

            if (manager.Sorted != null && manager.Sorted.Count > 0)
            {
                dataSource = PerformSorting(dataSource, manager.Sorted);
            }

            if (manager.Skip != 0)
            {
                dataSource = PerformSkip(dataSource, manager.Skip);
            }

            if (manager.Take != 0)
            {
                dataSource = PerformTake(dataSource, manager.Take);
            }

            return dataSource;
        }

        /// <summary>
        /// Groups data source by the given list of column names.
        /// </summary>
        /// <param name="dataSource">Input data source to be grouped.</param>
        /// <param name="grouped">List of column names by which rows will be grouped.</param>
        /// <returns>IEnumerable.</returns>
        public static IEnumerable<GroupResult> PerformGrouping(IEnumerable dataSource, List<string> grouped)
        {
            if (dataSource == null || grouped == null)
            {
                throw new ArgumentNullException(nameof(dataSource), "Data source or group can't be null in PerformGrouping");
            }

            IQueryable dataSourceQuery = dataSource.AsQueryable();

            Func<string, Expression>? getvalufunc = null;
            Type objType = dataSource.GetElementType();
            bool isdyanmic = typeof(IDynamicMetaObjectProvider).IsAssignableFrom(objType);
            if (isdyanmic)
            {
                Expression<Func<string, object, object>> valufunc = (propertyName, obj) => ReflectionExtension.GetValue(obj, propertyName, true)!;
                getvalufunc = propertyName => valufunc;
            }

            return getvalufunc == null
                ? dataSourceQuery.GroupByMany(grouped).AsQueryable()
                : dataSource.GroupByMany(objType, getvalufunc, [.. grouped]).AsQueryable();
        }

        /// <summary>
        /// Sorts the data source using the given sort descriptor and returns the sorted records.
        /// </summary>
        /// <param name="dataSource">Data source to be sorted.</param>
        /// <param name="sortedColumns">List of sort criteria.</param>
        /// <param name="sourceType">Specifies the source type.</param>
        /// <returns>IEnumerable - sorted records.</returns>
        public static IEnumerable PerformSorting(IEnumerable dataSource, List<SortedColumn> sortedColumns, Type? sourceType = null)
        {
            IQueryable data = dataSource.AsQueryable();
            sourceType ??= data.GetObjectType();
            bool isDynamic = typeof(IDynamicMetaObjectProvider).IsAssignableFrom(sourceType);
            Expression<Func<string, object, object>>? valueExpressionFunc = isDynamic ? (propertyName, obj) => ReflectionExtension.GetValue(obj, propertyName, true)! : null;

            bool firstTime = true;
            foreach (SortedColumn column in sortedColumns ?? [])
            {
                if (column.Direction == SortOrder.Ascending)
                {
                    if (firstTime)
                    {
                        data = !isDynamic
                            ? column.Comparer != null
                                ? data.OrderBy(column.Field, (column.Comparer as IComparer<object>)!, sourceType)
                                : data.OrderBy(column.Field, sourceType)
                            : column.Comparer != null
                                ? data.OrderBy(column.Field, (column.Comparer as IComparer<object>)!, valueExpressionFunc!)
                                : data.OrderBy(column.Field, valueExpressionFunc!);
                        firstTime = false;
                    }
                    else
                    {
                        data = !isDynamic
                            ? column.Comparer != null
                                ? data.ThenBy(column.Field, (column.Comparer as IComparer<object>)!, sourceType)
                                : data.ThenBy(column.Field, sourceType)
                            : column.Comparer != null
                                ? data.ThenBy(column.Field, (column.Comparer as IComparer<object>)!, valueExpressionFunc!)
                                : data.ThenBy(column.Field, valueExpressionFunc!);
                    }
                }
                else
                {
                    if (firstTime)
                    {
                        data = !isDynamic
                            ? column.Comparer != null
                                ? data.OrderByDescending(column.Field, (column.Comparer as IComparer<object>)!, sourceType)
                                : data.OrderByDescending(column.Field, sourceType)
                            : column.Comparer != null
                                ? data.OrderByDescending(column.Field, (column.Comparer as IComparer<object>)!, valueExpressionFunc!)
                                : data.OrderByDescending(column.Field, valueExpressionFunc!);
                        firstTime = false;
                    }
                    else
                    {
                        data = !isDynamic
                            ? column.Comparer != null
                                ? data.ThenByDescending(column.Field, (column.Comparer as IComparer<object>)!, sourceType)
                                : data.ThenByDescending(column.Field, sourceType)
                            : column.Comparer != null
                                ? data.ThenByDescending(column.Field, (column.Comparer as IComparer<object>)!, valueExpressionFunc!)
                                : data.ThenByDescending(column.Field, valueExpressionFunc!);
                    }
                }
            }

            return data;
        }

        /// <summary>
        /// Sorts the data source using the given sort descriptor and returns the sorted records.
        /// </summary>
        /// <param name="dataSource">Data source to be sorted.</param>
        /// <param name="sortedColumns">List of sort criteria.</param>
        /// <returns>IEnumerable - sorted records.</returns>
        public static IEnumerable PerformSorting(IEnumerable dataSource, List<Sort> sortedColumns)
        {
            IEnumerable data = dataSource;
            if (dataSource != null && !dataSource.GetEnumerator().MoveNext())
            {
                return dataSource;
            }
            List<SortedColumn> sortColumns = [];
            if (sortedColumns != null && sortedColumns.Count > 1)
            {
                sortedColumns.Reverse();
            }

            foreach (Sort column in sortedColumns ?? [])
            {
                SortOrder direction = Enum.Parse<SortOrder>(column.Direction.ToString(), true);
                sortColumns.Add(new SortedColumn { Direction = direction, Field = column.Name, Comparer = column.Comparer });
            }
            if (sortColumns != null && sortColumns.Count > 0 && sortedColumns != null && sortColumns.Any(x => x.Comparer is not null and ForeignKeySortManager))
            {
                ForeignKeySortManager forignkeySortColumns = (ForeignKeySortManager)sortedColumns.Where(x => x.Comparer != null).ToList()[0].Comparer;
                forignkeySortColumns.Initialize(sortedColumns);
            }
            if (dataSource == null || sortedColumns == null)
            {
                return dataSource!;
            }

            List<object> list = [.. dataSource.Cast<object>()];
            if (list.Count < 1)
            {
                return list;
            }

            Type elementType = list.First().GetType();
            MethodInfo method = typeof(EnumerableOperation).GetMethod(nameof(SortInternal), BindingFlags.NonPublic | BindingFlags.Static)!.MakeGenericMethod(elementType);

            return (IEnumerable)method.Invoke(null, [list, sortColumns])!;
        }
        private static List<T> SortInternal<T>(List<object> source, List<SortedColumn> columns)
        {
            List<T> typedList = [.. source.Cast<T>()];
            if (typedList.Count <= 1)
            {
                return typedList;
            }

            Func<T, object>[] getters = new Func<T, object>[columns.Count];
            IComparer<object>[] comparers = new IComparer<object>[columns.Count];
            bool[] directions = new bool[columns.Count];
            for (int i = 0; i < columns.Count; i++)
            {
                SortedColumn column = columns[i];
                ParameterExpression param = Expression.Parameter(typeof(T), "x");
                Expression propExpr = BuildPropertyChain(param, column.Field ?? "");
                getters[i] = Expression.Lambda<Func<T, object>>(
                    Expression.Convert(propExpr, typeof(object)), param).Compile();

                comparers[i] = column.Comparer as IComparer<object> ?? Comparer<object>.Default;
                directions[i] = column.Direction == SortOrder.Descending;
            }

            object[][] keyArrays = new object[columns.Count][];
            _ = Parallel.For(0, columns.Count, colIdx =>
            {
                object[] keys = new object[typedList.Count];
                _ = Parallel.For(0, typedList.Count, idx =>
                {
                    keys[idx] = getters[colIdx](typedList[idx]);
                });
                keyArrays[colIdx] = keys;
            });

            int[] indices = [.. Enumerable.Range(0, typedList.Count)];

            Array.Sort(indices, (i1, i2) =>
            {
                for (int c = 0; c < columns.Count; c++)
                {
                    if (columns[c].Comparer != null)
                    {
                        SortDataComparer<T> fastSortComparer = new(columns);
                        int comparer = fastSortComparer.Compare(typedList[i1], typedList[i2]);
                        if (comparer != 0)
                        {
                            return comparer;
                        }
                    }
                    else
                    {
                        object v1 = keyArrays[c][i1];
                        object v2 = keyArrays[c][i2];
                        int comparer = comparers[c].Compare(v1, v2);
                        if (comparer != 0)
                        {
                            return directions[c] ? -comparer : comparer;
                        }
                    }
                }
                return i1.CompareTo(i2);
            });

            T[] sorted = new T[typedList.Count];
            _ = Parallel.For(0, indices.Length, i => sorted[i] = typedList[indices[i]]);
            return [.. sorted];
        }
        private static Expression BuildPropertyChain(Expression param, string propertyPath)
        {
            string[] properties = propertyPath.Split('.');
            Expression current = param;

            foreach (string prop in properties)
            {
                PropertyInfo? propertyInfo = current.Type.GetProperty(prop);
                if (propertyInfo != null)
                {
                    Expression propertyAccess = Expression.Property(current, propertyInfo);
                    current = !current.Type.IsValueType || Nullable.GetUnderlyingType(current.Type) != null
                        ? Expression.Condition(
                            Expression.Equal(current, Expression.Constant(null, current.Type)),
                            Expression.Default(propertyInfo.PropertyType),
                            propertyAccess
                        )
                        : propertyAccess;
                }
                else
                {
                    current = Expression.Call(
                        typeof(EnumerableOperation).GetMethod(nameof(GetDynamicValue))!,
                        Expression.Convert(current, typeof(object)),
                        Expression.Constant(prop)
                    );
                }
            }
            Expression nullSafe = Expression.Condition(
                Expression.Equal(param, Expression.Constant(null, param.Type)),
                Expression.Constant(null, typeof(object)),
                Expression.Convert(current, typeof(object))
            );

            return nullSafe;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetDynamicValue(object obj, string key)
        {
            if (obj is IDictionary<string, object> dict && dict.TryGetValue(key, out object? value))
            {
                return value;
            }
            else if (obj != null && obj.GetType().BaseType == typeof(DynamicObject))
            {
                return ReflectionExtension.GetValue(obj, key, true)!;
            }

            return null!;
        }
        private readonly struct SortDataComparer<T> : IComparer<T>
        {
            private readonly (Func<T, object> Getter, IComparer<object> Comparer, bool Descending, bool isForeignKeyComparer)[] _accessors;

            public SortDataComparer(List<SortedColumn> columns)
            {
                _accessors = new (Func<T, object>, IComparer<object>, bool, bool)[columns.Count];
                ParameterExpression param = Expression.Parameter(typeof(T), "x");

                for (int i = 0; i < columns.Count; i++)
                {
                    SortedColumn column = columns[i];
                    Expression propExpr = BuildPropertyChain(param, column.Field ?? "");
                    Func<T, object> getter = Expression.Lambda<Func<T, object>>(
                        Expression.Convert(propExpr, typeof(object)), param).Compile();

                    IComparer<object> comparer = column.Comparer as IComparer<object> ?? Comparer<object>.Default;
                    bool descending = column.Direction == SortOrder.Descending;
                    bool isForeignKeyComparer = column.Comparer is ForeignKeySortManager;
                    _accessors[i] = (getter, comparer, descending, isForeignKeyComparer);
                }
            }

            public int Compare(T? x, T? y)
            {
                foreach ((Func<T, object>? getter, IComparer<object>? comparer, bool descending, bool isForeignKeyComparer) in _accessors)
                {
                    int comparerObject;

                    if (isForeignKeyComparer || comparer is not Comparer<object>)
                    {
                        comparerObject = comparer.Compare(x!, y!);
                    }
                    else
                    {
                        object value1 = getter(x!);
                        object value2 = getter(y!);
                        comparerObject = SafeCompare(value1, value2);
                    }

                    if (comparerObject != 0)
                    {
                        return descending ? -comparerObject : comparerObject;
                    }
                }
                return 0;
            }
            private static int SafeCompare(object? x, object? y)
            {
                return x == y
                    ? 0
                    : x == null
                    ? -1
                    : y == null
                    ? 1
                    : x is IComparable comparerX && y is IComparable && x.GetType() == y.GetType()
                    ? comparerX.CompareTo(y)
                    : StringComparer.OrdinalIgnoreCase.Compare(x.ToString(), y.ToString());
            }


        }
        /// <summary>
        /// Generates predicate with the given filter criteria.
        /// </summary>
        /// <param name="dataSource">Input data source.</param>
        /// <param name="whereFilter">List of filter criteria.</param>
        /// <param name="condition">Value can be either AND or OR.</param>
        /// <param name="paramExpression">Parameter expression.</param>
        /// <param name="type">Specifies the source type.</param>
        /// <returns>Expression.</returns>
        public static Expression PredicateBuilder(IEnumerable dataSource, List<WhereFilter> whereFilter, string condition, ParameterExpression paramExpression, Type type)
        {
            Expression? predicate = null;
            foreach (WhereFilter? filter in whereFilter ?? [])
            {
                if (filter.IsComplex)
                {
                    predicate = predicate == null
                        ? PredicateBuilder(dataSource, filter.Predicates, filter.Condition, paramExpression, type)
                        : condition == "or"
                            ? predicate.OrElsePredicate(PredicateBuilder(dataSource, filter.Predicates, filter.Condition, paramExpression, type))
                            : predicate.AndAlsoPredicate(PredicateBuilder(dataSource, filter.Predicates, filter.Condition, paramExpression, type));
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

                    FilterType filterType = Enum.Parse<FilterType>(filterOperator?.ToString()!, true);
                    type = GetDataType(dataSource, type, filter.Field);
                    Type t = GetColumnType(dataSource, filter.Field, type);
                    if (t == null)
                    {
                        return null!;
                    }

                    object? enumValue = new();
                    Type? underlyingType = Nullable.GetUnderlyingType(t);
                    if (underlyingType != null)
                    {
                        t = underlyingType;
                    }
                    if (t.IsEnum)
                    {
                        Type EnumPropType = DataUtil.GetEnumType(filter.Field, type);
                        Type? underlyingEnumPropType = Nullable.GetUnderlyingType(EnumPropType);
                        if (underlyingEnumPropType != null)
                        {
                            EnumPropType = underlyingEnumPropType;
                        }
                        enumValue = filter.value != null ? EnumerationValue.GetValueFromEnumMember(filter.value.ToString()!, EnumPropType) : null;
                        if (enumValue == null) // if enumvalue and enummember value are different then use enum value.
                        {
                            _ = Enum.TryParse(EnumPropType!, filter.value?.ToString()!, out enumValue!);
                        }
                    }

                    object? value = filter.value;
                    if (value != null)
                    {
                        if (t == typeof(Guid))
                        {
                            value = Guid.TryParse(filter?.value?.ToString(), out Guid parsedGuid)
                                ? (Guid)TypeDescriptor.GetConverter(typeof(Guid)).ConvertFromInvariantString(filter?.value.ToString()!)!
                                : Guid.Empty;
                        }
                        else if (filter?.value?.GetType().Name == t.Name || filter?.value?.GetType().Name == "JsonElement")
                        {
                            value = t.IsEnum ? enumValue : SfBaseUtils.ChangeType(filter.value, t);
                        }
                    }

                    predicate = predicate == null
                        ? filter?.ColumnType == "DateTime"
                            ? dataSource.AsQueryable().Predicate(paramExpression, filter.Field, value!, filterType, FilterBehavior.StringTyped, !filter.IgnoreCase, type, filter.IgnoreAccent, true)
                            : dataSource.AsQueryable().Predicate(paramExpression, filter?.Field!, value!, filterType, FilterBehavior.StringTyped, !(filter?.IgnoreCase ?? false), type, filter?.IgnoreAccent ?? false)
                        : condition == "or"
                            ? filter?.ColumnType == "DateTime"
                                ? predicate.OrElsePredicate(dataSource.AsQueryable().Predicate(paramExpression, filter.Field, value!, filterType, FilterBehavior.StringTyped, !filter.IgnoreCase, type, filter.IgnoreAccent, true))
                                : predicate.OrElsePredicate(dataSource.AsQueryable().Predicate(paramExpression, filter?.Field!, value!, filterType, FilterBehavior.StringTyped, !(filter?.IgnoreCase ?? false), type, filter?.IgnoreAccent ?? false))
                            : filter?.ColumnType == "DateTime"
                                ? predicate.AndAlsoPredicate(dataSource.AsQueryable().Predicate(paramExpression, filter.Field, value!, filterType, FilterBehavior.StringTyped, !filter.IgnoreCase, type, filter.IgnoreAccent, true))
                                : predicate.AndAlsoPredicate(dataSource.AsQueryable().Predicate(paramExpression, filter?.Field!, value!, filterType, FilterBehavior.StringTyped, !(filter?.IgnoreCase ?? false), type, filter?.IgnoreAccent ?? false));
                }
            }

            return predicate!;
        }

        /// <summary>
        /// Apply the given filter criteria against the data source and returns the filtered records.
        /// </summary>
        /// <param name="dataSource">Data source to be filtered.</param>
        /// <param name="whereFilter">List of filter criteria.</param>
        /// <param name="condition">Filter merge condition. Value can be either AND or OR.</param>
        /// <returns>IEnumerable - filtered records.</returns>
        public static IEnumerable PerformFiltering(IEnumerable dataSource, List<WhereFilter> whereFilter, string condition)
        {
            Type? type = dataSource?.GetElementType();
            if (type == null)
            {
                Type? type1 = dataSource?.GetType();
                type = type1?.GetElementType();
            }

            ParameterExpression? paramExpression = type?.Parameter();
            dataSource = dataSource!.AsQueryable().Where(paramExpression!, PredicateBuilder(dataSource!, whereFilter, condition, paramExpression!, type!));
            return dataSource;
        }

        /// <summary>
        /// Apply the given search criteria against the data source and returns the filtered records.
        /// </summary>
        /// <param name="dataSource">Data source to be filtered.</param>
        /// <param name="searchFilter">List of search criteria.</param>
        /// <returns>IEnumerable - searched records.</returns>
        public static IEnumerable PerformSearching(IEnumerable dataSource, List<SearchFilter> searchFilter)
        {
            Type? type = dataSource.GetElementType();
            Type t = typeof(object);
            if (type == null)
            {
                Type? type1 = dataSource?.GetType();
                type = type1?.GetElementType();
            }

            foreach (SearchFilter filter in searchFilter ?? [])
            {
                ParameterExpression? paramExpression = type?.Parameter();
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
                    type = GetDataType(dataSource!, type!, fields);
                    t = GetColumnType(dataSource!, fields, type);
                    if (t == null)
                    {
                        continue;
                    }

                    object? enumValue = new();
                    if (t.IsEnum)
                    {
                        Type EnumPropType = DataUtil.GetEnumType(fields, type);
                        enumValue = EnumerationValue.GetValueFromEnumMember(filter.Key.ToString(), EnumPropType);
                        if (enumValue == null) // if enumvalue and enummember value are different then use enum value.
                        {
                            _ = Enum.TryParse(EnumPropType!, filter.Key.ToString()!, out enumValue);
                        }
                    }

                    if (initialLoop && !t.IsEnum)
                    {
                        predicate = dataSource?.AsQueryable().Predicate(paramExpression!, fields, (t.IsEnum ? enumValue : filter.Key)!, FilterType, FilterBehavior.StringTyped, !filter.IgnoreCase, type, filter.IgnoreAccent);
                        initialLoop = false;
                    }
                    else if ((t.IsEnum && NullableHelperInternal.IsNullableType(t)) || (t.IsEnum && enumValue != null))
                    {
                        predicate = !initialLoop
                            ? (predicate?.OrElsePredicate(dataSource?.AsQueryable()?.Predicate(paramExpression!, fields, (t.IsEnum ? enumValue : filter.Key)!, FilterType, FilterBehavior.StringTyped, !filter.IgnoreCase, type, filter.IgnoreAccent)!))
                            : (dataSource?.AsQueryable().Predicate(paramExpression!, fields, (t.IsEnum ? enumValue : filter.Key)!, FilterType, FilterBehavior.StringTyped, !filter.IgnoreCase, type, filter.IgnoreAccent));

                        initialLoop = false;
                    }
                    else if (!t.IsEnum)
                    {
                        predicate = predicate?.OrElsePredicate(dataSource?.AsQueryable().Predicate(paramExpression!, fields, (t.IsEnum ? enumValue : filter.Key)!, FilterType, FilterBehavior.StringTyped, !filter.IgnoreCase, type, filter.IgnoreAccent)!);
                    }
                }

                dataSource = dataSource!.AsQueryable().Where(paramExpression!, predicate!);
            }

            return dataSource!;
        }

        /// <summary>
        /// Returns data type.
        /// </summary>
        /// <exclude/>
        public static Type GetDataType(IEnumerable dataSource, Type type, string field)
        {
            string[] complexData = field != null ? field.Split('.') : [];
            if (type != null && type.GetProperty(complexData[0]) == null)
            {
                type = dataSource.AsQueryable().GetObjectType();
            }

            return type!;
        }

        /// <summary>
        /// Returns column type.
        /// </summary>
        /// <exclude/>
        public static Type GetColumnType(IEnumerable dataSource, string filterString, Type type)
        {
            string[] complexData = filterString != null ? filterString.Split('.') : []; ;
            PropertyInfo? propInfo = null;
            for (int i = 0; i < complexData.Length; i++)
            {
                if (int.TryParse(complexData[i], out _))
                {
                    type = type?.GetProperties()[2].PropertyType!;
                }
                else if (string.Equals(type?.Name, "ExpandoObject", StringComparison.Ordinal))
                {
                    object? value = DataUtil.GetObject(filterString!, dataSource.AsQueryable().ElementAt(0));
                    type = value == null && dataSource != null ? UpdateType(dataSource, filterString!, value!, type!) : value?.GetType()!;
                    return type;
                }
                else if (type!.IsSubclassOf(typeof(DynamicObject)))
                {
                    object? value = DataUtil.GetObject(filterString!, dataSource.AsQueryable().ElementAt(0));
                    type = value == null && dataSource != null ? UpdateType(dataSource, filterString!, value!, type) : value?.GetType()!;
                    return type;
                }
                else
                {
                    propInfo = type.GetProperty(complexData[i], BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    type = propInfo?.PropertyType!;
                }
            }

            return propInfo?.PropertyType!;
        }

        private static Type UpdateType(IEnumerable dataSource, string filterString, object value, Type type)
        {
            bool isValue = false;
            foreach (object? item in dataSource)
            {
                value = DataUtil.GetObject(filterString, item);
                if (value != null)
                {
                    isValue = true;
                    break;
                }
            }
            if (isValue)
            {
                type = value!.GetType();
            }
            return type;
        }

        /// <summary>
        /// Skip the given number of records from data source and returns the resultant records.
        /// </summary>
        /// <param name="dataSource">Input data source.</param>
        /// <param name="skip">Number of records to be skipped.</param>
        /// <returns>IEnumerable.</returns>
        public static IEnumerable PerformSkip(IEnumerable dataSource, int skip)
        {
            IEnumerable data = dataSource;
            return data.AsQueryable().Skip(skip);
        }

        /// <summary>
        /// Take the given number of records from data source.
        /// </summary>
        /// <param name="dataSource">Input data source.</param>
        /// <param name="take">Number of records to be taken.</param>
        /// <returns>IEnumerable.</returns>
        public static IEnumerable PerformTake(IEnumerable dataSource, int take)
        {
            IEnumerable data = dataSource;
            return data.AsQueryable().Take(take);
        }
    }
}
