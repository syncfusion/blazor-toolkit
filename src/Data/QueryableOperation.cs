using System.Linq.Expressions;
using System.Reflection;
using Syncfusion.Blazor.Toolkit.Data;
using Syncfusion.Blazor.Toolkit.Internal;
using System.ComponentModel;
using System.Dynamic;
using System.Collections;

namespace Syncfusion.Blazor.Toolkit.Data
{
    /// <summary>
    /// DataOperation class that performs data operation in IQueryable type data sources.
    /// </summary>
    public static class QueryableOperation
    {
        private static Type DataSourceType<T>(IQueryable<T> dataSource)
        {
            Type? type = dataSource.GetElementType();
            if (type == null)
            {
                Type dataSourceType = dataSource.GetType();
                type = dataSourceType.GetElementType();
                if (type == null)
                {
                    dataSource = dataSource.OfType<T>();
                    type = dataSource.GetElementType();
                    if (type == null)
                    {
                        dataSourceType = dataSource.GetType();
                        type = dataSourceType.GetElementType();
                    }
                }
            }

            return type!;
        }

        /// <summary>
        /// Executes the query against the given data source and returns the resultant records.
        /// </summary>
        /// <param name="dataSource">Input data source against which the query to be executed.</param>
        /// <param name="manager">Query to be executed.</param>
        /// <returns>IQueryable - resultant records.</returns>
        public static IQueryable<T> Execute<T>(IQueryable<T> dataSource, DataManagerRequest manager)
        {
            if (manager == null) { return dataSource; }
            if (manager.Where != null && manager.Where.Count > 0)
            {
                dataSource = PerformFiltering(dataSource, manager.Where, string.Empty);
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
        /// <returns>IQueryable.</returns>
        public static IQueryable PerformGrouping<T>(IQueryable<T> dataSource, List<string> grouped)
        {
            return EnumerableOperation.PerformGrouping(dataSource, grouped).AsQueryable();
        }

        /// <summary>
        /// Sorts the data source using the given sort descriptor and returns the sorted records.
        /// </summary>
        /// <param name="dataSource">Data source to be sorted.</param>
        /// <param name="sortedColumns">List of sort criteria.</param>
        /// <returns>IQueryable - sorted records.</returns>
        public static IQueryable<T> PerformSorting<T>(IQueryable<T> dataSource, List<SortedColumn> sortedColumns)
        {
            return (IOrderedQueryable<T>)EnumerableOperation.PerformSorting(dataSource, sortedColumns, typeof(T));
        }

        /// <summary>
        /// Sorts the data source using the given sort descriptor and returns the sorted records.
        /// </summary>
        /// <param name="dataSource">Data source to be sorted.</param>
        /// <param name="sortColumns">List of sort criteria.</param>
        /// <returns>IQueryable - sorted records.</returns>
        public static IQueryable<T> PerformSorting<T>(IQueryable<T> dataSource, List<Sort> sortColumns)
        {
            sortColumns ??= [];
            if (sortColumns.Count != 0)
            {
                sortColumns.Reverse();
            }
            else
            {
                return dataSource;
            }

            List<SortedColumn> sortedColumn = [];
            foreach (Sort column in sortColumns)
            {
                SortOrder direction = Enum.Parse<SortOrder>(column.Direction, true);
                sortedColumn.Add(new SortedColumn { Direction = direction, Field = column.Name });
            }

            return PerformSorting(dataSource, sortedColumn);
        }

        /// <summary>
        /// Skip the given number of records from data source and returns the resultant records.
        /// </summary>
        /// <param name="dataSource">Input data source.</param>
        /// <param name="skip">Number of records to be skipped.</param>
        /// <returns>IQueryable.</returns>
        public static IQueryable<T> PerformSkip<T>(IQueryable<T> dataSource, int skip)
        {
            IQueryable<T> data = dataSource.AsQueryable();
            return data.Skip<T>(skip);
        }

        /// <summary>
        /// Take the given number of records from data source.
        /// </summary>
        /// <param name="dataSource">Input data source.</param>
        /// <param name="take">Number of records to be taken.</param>
        /// <returns>IQueryable.</returns>
        public static IQueryable<T> PerformTake<T>(IQueryable<T> dataSource, int take)
        {
            IQueryable<T> data = dataSource.AsQueryable();
            return data.Take<T>(take);
        }

        private static Type GetDataType<T>(IQueryable<T> dataSource, Type type, string field)
        {
            string[] complexData = field.Split('.');
            if (type.GetProperty(complexData[0]) == null)
            {
                type = dataSource.GetObjectType();
            }

            return type;
        }

        private static Type GetColumnType<T>(IQueryable<T> dataSource, string filterString, Type? type)
        {
            string[] complexData = filterString.Split('.');
            PropertyInfo? propInfo = null;
            for (int i = 0; i < complexData.Length; i++)
            {
                if (int.TryParse(complexData[i], out _))
                {
                    type = type?.GetElementType();
                }
                else if (string.Equals(type?.Name, "ExpandoObject", StringComparison.Ordinal))
                {
                    object value = DataUtil.GetObject(filterString, dataSource.AsQueryable().ElementAt(0));
                    type = value?.GetType();
                    return type!;
                }
                else if (type!.IsSubclassOf(typeof(DynamicObject)))
                {
                    object value = DataUtil.GetObject(filterString, dataSource.AsQueryable().ElementAt(0));
                    type = value.GetType();
                    return type;
                }
                else
                {
                    propInfo = type?.GetProperty(complexData[i]);
                    type = propInfo?.PropertyType;
                }
            }

            return propInfo?.PropertyType!;
        }

        /// <summary>
        /// Apply the given search criteria against the data source and returns the filtered records.
        /// </summary>
        /// <param name="dataSource">Data source to be filtered.</param>
        /// <param name="searchFilter">List of search criteria.</param>
        /// <returns>IQueryable - searched records.</returns>
        public static IQueryable<T> PerformSearching<T>(IQueryable<T> dataSource, List<SearchFilter> searchFilter)
        {
            Type? type = dataSource != null ? DataSourceType(dataSource) : null;
            foreach (SearchFilter? filter in searchFilter ?? [])
            {
                ParameterExpression paramExpression = type!.Parameter();
                bool initialLoop = true;
                Expression? predicate = null;
                string op = filter.Operator == "equal" ? "equals" : filter.Operator == "notequal" ? "notequals" : filter.Operator;
                FilterType FilterType = Enum.Parse<FilterType>(op.ToString(), true);
                foreach (string fields in filter.Fields)
                {
                    type = GetDataType(dataSource!, type!, fields);
                    Type t = GetColumnType(dataSource!, fields, type);
                    object? enumValue = new();
                    Type? underlyingType = Nullable.GetUnderlyingType(t);
                    if (underlyingType != null)
                    {
                        t = underlyingType;
                    }
                    if (t.IsEnum)
                    {
                        Type EnumPropType = DataUtil.GetEnumType(fields, type);
                        Type EnumUnderlyingPropType = Nullable.GetUnderlyingType(EnumPropType) ?? EnumPropType;

                        enumValue = filter?.Key == null ? null : EnumerationValue.GetValueFromEnumMember(filter.Key, EnumUnderlyingPropType);

                        if (enumValue == null) // if enumvalue and enummember value are different then use enum value.
                        {
                            _ = Enum.TryParse(EnumUnderlyingPropType, filter?.Key?.ToString(), out enumValue);
                        }
                    }

                    if (initialLoop && !t.IsEnum)
                    {
                        predicate = dataSource?.Predicate(paramExpression, fields, (t.IsEnum ? enumValue : filter?.Key)!, FilterType, FilterBehavior.StringTyped, !(filter?.IgnoreCase ?? false), type, filter?.IgnoreAccent ?? false);
                        initialLoop = false;
                    }
                    else if ((t.IsEnum && NullableHelperInternal.IsNullableType(t)) || (t.IsEnum && enumValue != null))
                    {
                        predicate = !initialLoop
                            ? (predicate?.OrElsePredicate(dataSource?.Predicate(paramExpression, fields, (t.IsEnum ? enumValue : filter?.Key)!, FilterType, FilterBehavior.StringTyped, !(filter?.IgnoreCase ?? false), type, filter?.IgnoreAccent ?? false)!))
                            : dataSource?.Predicate(paramExpression, fields, (t.IsEnum ? enumValue : filter?.Key)!, FilterType, FilterBehavior.StringTyped, !(filter?.IgnoreCase ?? false), type, filter?.IgnoreAccent ?? false)!;

                        initialLoop = false;
                    }
                    else if (!t.IsEnum)
                    {
                        predicate = predicate?.OrElsePredicate(dataSource?.Predicate(paramExpression, fields, (t.IsEnum ? enumValue : filter?.Key)!, FilterType, FilterBehavior.StringTyped, !(filter?.IgnoreCase ?? false), type, filter?.IgnoreAccent ?? false)!);
                    }
                }

                dataSource = dataSource?.Where(Expression.Lambda<Func<T, bool>>(predicate!, paramExpression))!;
            }

            return dataSource!;
        }

        private static Expression PredicateBuilder<T>(IQueryable<T> dataSource, List<WhereFilter> whereFilter, string condition, ParameterExpression paramExpression, Type type)
        {
            _ = typeof(object);
            Expression? predicate = null;
            foreach (WhereFilter? filter in whereFilter)
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
                    string op = filter.Operator == "equal" ? "equals" : filter.Operator == "notequal" ? "notequals" : filter.Operator;
                    FilterType filterType = Enum.Parse<FilterType>(op.ToString(), true);
                    type = GetDataType(dataSource, type, filter.Field);
                    Type t = GetColumnType(dataSource, filter.Field, type);
                    Type? underlyingType = Nullable.GetUnderlyingType(t);
                    object? enumValue = new();
                    if (t.IsEnum || (underlyingType != null && underlyingType.IsEnum))
                    {
                        Type EnumPropType = DataUtil.GetEnumType(filter.Field, type);
                        Type? EnumUnderlyingPropType = Nullable.GetUnderlyingType(EnumPropType);
                        if (EnumUnderlyingPropType != null)
                        {
                            EnumPropType = EnumUnderlyingPropType;
                        }
                        enumValue = filter.value == null ? null : EnumerationValue.GetValueFromEnumMember(filter.value.ToString()!, EnumUnderlyingPropType ?? EnumPropType);
                        if (enumValue == null) // if enumvalue and enummember value are different then use enum value.
                        {
                            _ = Enum.TryParse(EnumPropType, filter.value?.ToString(), out enumValue);
                        }
                    }

                    if (underlyingType != null)
                    {
                        t = underlyingType;
                    }

                    object? value = filter.value;
                    if (value != null)
                    {
                        if (t == typeof(Guid))
                        {
                            value = (Guid)TypeDescriptor.GetConverter(typeof(Guid)).ConvertFromInvariantString(filter?.value?.ToString()!)!;
                        }
                        else if (filter?.value?.GetType().Name == t.Name || filter?.value?.GetType().Name == "JsonElement")
                        {
                            value = t.IsEnum ? enumValue : SfBaseUtils.ChangeType(filter.value, t);
                        }
                    }

                    predicate = predicate == null
                        ? filter?.ColumnType == "DateTime"
                            ? dataSource.Predicate(paramExpression, filter.Field, value!, filterType, FilterBehavior.StringTyped, !filter.IgnoreCase, type, filter.IgnoreAccent, true)
                            : dataSource.Predicate(paramExpression, filter?.Field!, value!, filterType, FilterBehavior.StringTyped, !(filter?.IgnoreCase ?? false), type, filter?.IgnoreAccent ?? false)
                        : condition == "or"
                            ? filter?.ColumnType == "DateTime"
                                ? predicate.OrElsePredicate(dataSource.Predicate(paramExpression, filter.Field, value!, filterType, FilterBehavior.StringTyped, !filter.IgnoreCase, type, filter.IgnoreAccent, true))
                                : predicate.OrElsePredicate(dataSource.Predicate(paramExpression, filter?.Field!, value!, filterType, FilterBehavior.StringTyped, !(filter?.IgnoreCase ?? false), type, filter?.IgnoreAccent ?? false))
                            : filter?.ColumnType == "DateTime"
                                ? predicate.AndAlsoPredicate(dataSource.Predicate(paramExpression, filter.Field, value!, filterType, FilterBehavior.StringTyped, !filter.IgnoreCase, type, filter.IgnoreAccent, true))
                                : predicate.AndAlsoPredicate(dataSource.Predicate(paramExpression, filter?.Field!, value!, filterType, FilterBehavior.StringTyped, !(filter?.IgnoreCase ?? false), type, filter?.IgnoreAccent ?? false));
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
        /// <returns>IQueryable - filtered records.</returns>
        public static IQueryable<T> PerformFiltering<T>(IQueryable<T> dataSource, List<WhereFilter> whereFilter, string condition)
        {
            Type? type = dataSource != null ? DataSourceType(dataSource) : null;
            ParameterExpression paramExpression = type!.Parameter();
            dataSource = dataSource?.Where(Expression.Lambda<Func<T, bool>>(PredicateBuilder(dataSource, whereFilter ?? null!, condition, paramExpression, type!), paramExpression))!;
            return dataSource;
        }

        /// <summary>
        /// Selects the fields from data source.
        /// </summary>
        /// <param name="dataSource">Input data source.</param>
        /// <param name="select">Fields to select.</param>
        /// <returns></returns>
        public static IQueryable PerformSelect(IQueryable dataSource, List<string> select)
        {
            IEnumerable<string> sel = select.Where(item => item != null);
            Type type = dataSource.AsQueryable().GetObjectType();
            if (type == typeof(object))
            {
                IEnumerator? e = dataSource?.GetEnumerator();
                if (e != null && e.MoveNext())
                {
                    type = e.Current.GetType();
                }
            }

            IQueryable? data = dataSource;
            foreach (string item in sel)
            {
                data = data?.Select(item);
            }

            return data!;
        }

        /// <summary>
        /// Selects the fields from data source.
        /// </summary>
        /// <typeparam name="T">Type of the data source elements.</typeparam>
        /// <param name="dataSource">Input data source.</param>
        /// <param name="select">Fields to select.</param>
        /// <returns></returns>
        public static IQueryable PerformSelect<T>(IQueryable dataSource, List<string> select)
        {
            IEnumerable<string> sel = select.Where(item => item != null);
            Type type = dataSource.AsQueryable().GetObjectType();
            if (type == typeof(object))
            {
                IEnumerator? e = dataSource?.GetEnumerator();
                if (e != null && e.MoveNext())
                {
                    type = e.Current.GetType();
                }
            }

            string item = string.Join(",", select?.ToArray()!);
            return dataSource?.Select<T>(item)!;
        }
    }
}
