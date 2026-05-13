using System.Collections;
using System.Reflection;
using System.Globalization;
using System.Dynamic;
using System.Text.RegularExpressions;
using Syncfusion.Blazor.Toolkit.Internal;
using System.Text.Json.Serialization;
using System.Text.Json;
using Syncfusion.Blazor.Toolkit.Charts.Internal;

namespace Syncfusion.Blazor.Toolkit.Data
{
    /// <summary>
    /// Provides utility method used by data manager.
    /// </summary>
    public static class DataUtil
    {
        private static List<string> IgnoredFields { get; set; } = [];

        internal static bool IsNullableComplexField { get; set; }

        /// <summary>
        /// Resolves the given base url and relative url to generate absolute url. And merge query string if any.
        /// </summary>
        /// <param name="baseUrl">Base address url.</param>
        /// <param name="relativeUrl">Relative url.</param>
        /// <param name="queryParams">Query string.</param>
        /// <returns>string - absolute url.</returns>
        public static string GetUrl(string baseUrl, string relativeUrl, string? queryParams = null)
        {
            bool bHasSlash = !string.IsNullOrEmpty(baseUrl) && baseUrl[^1] == '/';
            string url = baseUrl;
            string queryString = string.Empty;

            if (!string.IsNullOrEmpty(relativeUrl))
            {
                bool rHasSlash = !string.IsNullOrEmpty(relativeUrl) && relativeUrl[0] == '/';
                url = bHasSlash ^ rHasSlash
                    ? $"{baseUrl}{relativeUrl}"
                    : !bHasSlash && !rHasSlash
                        ? $"{baseUrl}/{relativeUrl}"
                        : bHasSlash && rHasSlash ? $"{baseUrl}{relativeUrl[1..]}" : $"{baseUrl}{relativeUrl}";
            }

            if (string.IsNullOrEmpty(queryParams))
            {
                return url;
            }

            // Query parameters process
            if (url[^1] != '?' && url.IndexOf('?', StringComparison.Ordinal) > -1)
            {
                queryString = $"&{queryParams}";
            }
            else if (url.IndexOf('?', StringComparison.Ordinal) < 0 && !string.IsNullOrEmpty(queryParams))
            {
                queryString = $"?{queryParams}";
            }

            return url + queryString;
        }

        /// <summary>
        /// Gets the property value with the given key.
        /// </summary>
        /// <param name="key">Property name.</param>
        /// <param name="value">Source object.</param>
        /// <returns>string.</returns>
        public static string GetKeyValue(string key, object value)
        {
            PropertyInfo? propInfo = value?.GetType().GetProperty(key);
            Type propType = propInfo!.PropertyType;

            // Check nullable types.
            if (propType.IsGenericType && propType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                propType = NullableHelperInternal.GetUnderlyingType(propType);
            }

            object? propVal = propInfo.GetValue(value);
            if (propType.Name == "DateTime")
            {
                propVal = ((DateTime)propVal!).ToString("s", CultureInfo.InvariantCulture);
            }

            return propVal?.ToString()!;
        }

        /// <summary>
        /// Converts dictionary of key/value pair to query string.
        /// </summary>
        /// <param name="Params">Input dictionary value.</param>
        /// <returns>string - Query string.</returns>
        public static string ToQueryParams(IDictionary<string, object> Params)
        {
            string[] sb = new string[Params != null ? Params.Count : 0];
            int i = 0;
            foreach (KeyValuePair<string, object> param in Params ?? new Dictionary<string, object>())
            {
                if (param.Value != null)
                {
                    sb[i++] = $"{param.Key}={param.Value}";
                }
            }

            return string.Join("&", sb);
        }

        /// <summary>
        /// Converts dictionary of key/value pair to query string.
        /// </summary>
        /// <param name="dataSource">Collection of Data source.</param>
        /// <param name="propertyName">property name which is need to distincts </param>.
        /// <returns>IEnumerable Distinct collections</returns>
        internal static IEnumerable<T> GetDistinct<T>(IEnumerable<T> dataSource, string propertyName)
        {
            List<T> DistinctCollections = [];
            Dictionary<string, object> DistinctData = [];
            string complexNameSpace = propertyName.Contains('.', StringComparison.InvariantCulture) ? propertyName.Split(".")[0] : propertyName;
            if (dataSource == null)
            {
                return DistinctCollections.AsEnumerable();
            }
            foreach (T? value in dataSource)
            {
                if (value is ExpandoObject)
                {
                    if (value is IDictionary<string, object> dictionaryValue && !dictionaryValue.ContainsKey(complexNameSpace))
                    {
                        continue;
                    }
                }

                object propertyValue = GetObject(propertyName, value!);
                string? key = propertyValue == null ? "null" : propertyValue.ToString();

                if (!DistinctData.ContainsKey(key!))
                {
                    DistinctData.Add(key!, value!);
                    DistinctCollections.Add(value);
                }
            }
            return DistinctCollections.AsEnumerable();
        }

        internal static IDictionary<string, string> _odUniOperator = new Dictionary<string, string>()
        {
            { "$=", "endswith" },
            { "^=", "startswith" },
            { "*=", "substringof" },
            { "isempty", " eq " },
            { "isnotempty", " ne " },
            { "endswith", "endswith" },
            { "startswith", "startswith" },
            { "contains", "substringof" },
            { "doesnotendwith", "notendswith" },
            { "doesnotstartwith", "notstartswith" },
            { "doesnotcontain", "notsubstringof" },
            { "like", "like" },
            { "wildcard", "wildcard" }
        };

        internal static IDictionary<string, string> _odBiOperator = new Dictionary<string, string>()
        {
            { "<", " lt " },
            { ">", " gt " },
            { "<=", " le " },
            { ">=", " ge " },
            { "==", " eq " },
            { "!=", " ne " },
            { "lessthan", " lt " },
            { "lessthanorequal", " le " },
            { "greaterthan", " gt " },
            { "greaterthanorequal", " ge " },
            { "equal", " eq " },
            { "notequal", " ne " },
            { "isnull", " eq " },
            { "isnotnull", " ne " },
            { "isempty", " eq " },
            { "isnotempty", " ne " },

        };

        internal static IDictionary<string, string> _odv4UniOperator = new Dictionary<string, string>()
        {
            { "$=", "endswith" },
            { "^=", "startswith" },
            { "*=", "contains" },
            { "isempty", " eq " },
            { "isnotempty", " ne " },
            { "endswith", "endswith" },
            { "startswith", "startswith" },
            { "contains", "contains" },
            { "doesnotendwith", "notendswith" },
            { "doesnotstartwith", "notstartswith" },
            { "doesnotcontain", "notcontains" },
            { "like", "like" },
            { "wildcard", "wildcard" }
        };

        internal static IDictionary<string, string> _consts = new Dictionary<string, string>()
        {
            { "GroupGuid", "{271bbba0-1ee7}" }
        };

        /// <summary>
        /// Groups the given data source with the field name.
        /// </summary>
        /// <typeparam name="T">Type of the data source elements.</typeparam>
        /// <param name="jsonArray">Input data source.</param>
        /// <param name="field">Specifies the group by field name.</param>
        /// <param name="aggregates">Aggregate details to aggregate grouped records.</param>
        /// <param name="level">Level of the group. For parent group it is 0.</param>
        /// <param name="format">Specifies the format and handler method to perform group by format.</param>
        /// <param name="isLazyLoad">Specifies the isLazyLoad property as true to handle lazy load grouping.</param>
        /// <param name="isLazyGroupExpandAll">Specifies the isLazyGroupExpandAll as true to perform expand all for lazy load grouping.</param>
        /// <returns>IEnumerable - Grouped record.</returns>
        public static IEnumerable Group<T>(IEnumerable jsonArray, string field, List<Aggregate> aggregates, int level, IDictionary<string, string> format, bool isLazyLoad = false, bool isLazyGroupExpandAll = false)
        {
            if (level == 0)
            {
                level = 1;
            }
            const string guid = "GroupGuid";
            if (jsonArray?.GetType().GetProperty(guid) != null &&
                (jsonArray as Group<T>)?.GroupGuid == _consts[guid])
            {
                Group<T> json = (Group<T>)jsonArray;
                for (int j = 0; j < json.Count; j++)
                {
                    json[j].Items = Group<T>(json[j].Items, field, aggregates, level + 1,
                        format, isLazyLoad, isLazyGroupExpandAll);
                    json[j].CountItems = json[j].Items.Cast<object>().Count();
                }
                json.ChildLevels++;
                return json;
            }
            Group<T> groupedArray = new()
            {
                GroupGuid = _consts[guid],
                Level = level,
                ChildLevels = 0,
                Records = jsonArray!
            };

            Dictionary<object, Group<T>> grouped = [];
            Dictionary<object, object> formatCache = [];

            foreach (object? itemObj in jsonArray!)
            {
                object? unformattedValue = GetGroupValue(field, itemObj);
                object val = unformattedValue ?? "null";
                if (format != null && format.TryGetValue(field, out string? fmt) && unformattedValue != null)
                {
                    if (!formatCache.TryGetValue(unformattedValue, out val!))
                    {
                        val = GetFormattedValue(unformattedValue, fmt);
                        formatCache[unformattedValue] = val;
                    }
                }

                if (!grouped.TryGetValue(val, out Group<T>? group))
                {
                    group = new Group<T>
                    {
                        Key = val,
                        UnformattedKey = unformattedValue,
                        CountItems = 0,
                        Level = level,
                        Items = new List<T>(isLazyLoad ? 0 : 16),
                        Aggregates = new object(),
                        Field = field,
                        GroupedData = new List<T>(isLazyLoad || isLazyGroupExpandAll ? 16 : 0)
                    };
                    grouped[val] = group;
                    groupedArray.Add(group);
                }

                group.CountItems++;
                if (!isLazyLoad || (isLazyLoad && aggregates != null))
                {
                    ((List<T>)group.Items).Add((T)itemObj);
                }

                if (isLazyLoad || isLazyGroupExpandAll)
                {
                    ((List<T>)group.GroupedData).Add((T)itemObj);
                }
            }
            if (aggregates?.Count > 0)
            {
                _ = Parallel.ForEach(groupedArray, group =>
                {
                    Dictionary<string, object> res = [];
                    Type? type = group.Items.Cast<object>().FirstOrDefault()?.GetType();
                    foreach (Aggregate aggregate in aggregates)
                    {
                        Func<IEnumerable, string, string, Type, object> fn = CalculateAggregateFunc();
                        if (fn != null)
                        {
                            res[$"{aggregate.Field} - {aggregate.Type}"] =
                                fn(group.Items, aggregate.Field, aggregate.Type, type!);
                        }
                    }
                    group.Aggregates = res;
                });
            }

            if (isLazyLoad && aggregates != null)
            {
                foreach (Group<T> group in groupedArray)
                {
                    group.Items = new List<T>();
                }
            }
            return groupedArray;
        }

        /// <summary>
        /// Sorts the grouped data source based on the specified sort descriptors and returns the sorted records,
        /// when the GroupSettings property is set to true.
        /// </summary>
        /// <param name="dataSource">Data source to be sorted.</param>
        /// <param name="sortedColumns">List of sort criteria.</param>
        /// <returns>IEnumerable - sorted records.</returns>
        public static IEnumerable GroupSorting<T>(IEnumerable dataSource, List<Sort> sortedColumns)
        {
            if (dataSource != null && dataSource is Group<T>)
            {
                string fieldName = (dataSource as Group<T>)!.ElementAt(0).Field;
                IEnumerable<Sort> sortedcolumn = sortedColumns.Where(x => x.Name == fieldName);
                dataSource = PerformGroupSorting<T>(dataSource, sortedcolumn);
            }
            return dataSource!;
        }

        internal static IEnumerable PerformGroupSorting<T>(IEnumerable dataSource, IEnumerable<Sort> sortedCol)
        {
            IQueryable data = dataSource.AsQueryable();
            Type sourceType = data.GetObjectType();

            foreach (Sort column in sortedCol)
            {
                data = column.Direction == "ascending"
                    ? column.Comparer != null
                        ? data.OrderBy("UnformattedKey", (column.Comparer as IComparer<object>)!, sourceType)
                        : data.OrderBy("UnformattedKey", sourceType)
                    : column.Comparer != null
                        ? data.OrderByDescending("UnformattedKey", (column.Comparer as IComparer<object>)!, sourceType)
                        : data.OrderByDescending("UnformattedKey", sourceType);
            }
            return data;
        }
        /// <summary>
        /// Performs aggregation on the given data source.
        /// </summary>
        /// <param name="jsonData">Input data source.</param>
        /// <param name="aggregates">List of aggregate to be calculated.</param>
        /// <returns>Dictionary of aggregate results.</returns>
        public static IDictionary<string, object> PerformAggregation(IEnumerable jsonData, List<Aggregate> aggregates)
        {
            Dictionary<string, object> res = [];
            Func<IEnumerable, string, string, Type, object> fn;

            IEnumerable<object>? jsoncol = jsonData?.Cast<object>();
            object? firstData = jsoncol?.FirstOrDefault();
            if (jsonData == null || firstData == null)
            {
                return res;
            }

            Type type = firstData.GetType();
            IEnumerable ConvData = CastList(type, jsoncol!);
            fn = CalculateAggregateFunc();
            for (int j = 0; j < (aggregates?.Count ?? 0); j++)
            {
                if (fn != null && aggregates?[j] != null)
                {
                    res[aggregates[j].Field + " - " + aggregates[j].Type.ToLowerInvariant()] = fn(ConvData, aggregates[j].Field, aggregates[j].Type, type);
                }
            }

            return res;
        }

        internal static IEnumerable CastList(Type type, IEnumerable<object> items)
        {
            Type enumerableType = typeof(Enumerable);
            MethodInfo? castMethod = enumerableType.GetMethod(nameof(Enumerable.Cast))?.MakeGenericMethod(type);
            object? castedItems = castMethod?.Invoke(null, [items]);
            return (castedItems as IEnumerable)!;
        }

        /// <summary>
        /// Gets the property value from list of object.
        /// </summary>
        /// <param name="jsonData">List of object.</param>
        /// <param name="index">Index of the item to be processed.</param>
        /// <param name="field">Property name to get value.</param>
        /// <returns>object.</returns>
        public static object GetVal(IEnumerable jsonData, int index, string field)
        {
            IQueryable<object> jsonDataCol = jsonData.AsQueryable().Cast<object>();
            return jsonDataCol.Any() ? field != null ? GetObject(field, jsonDataCol.ToArray()[index]) : jsonDataCol.ToArray()[index] : null!;
        }

        /// <summary>
        /// Gets the property value from object.
        /// </summary>
        /// <param name="nameSpace">Property name to be accessed.</param>
        /// <param name="from">Source object.</param>
        /// <returns>object - property value.</returns>
        public static object GetGroupValue(string nameSpace, object from)
        {
            return nameSpace != null ? GetObject(nameSpace, from) : from;
        }

        /// <summary>
        /// Gets the property value from object.
        /// </summary>
        /// <param name="nameSpace">Property name to be accessed.</param>
        /// <param name="from">Source object.</param>
        /// <returns>object - property value.</returns>
        /// <remarks>For accessing complex/nested property value, given the nameSpace with field names delimited by dot(.).</remarks>
        public static object GetObject(string nameSpace, object from)
        {
            return ReflectionExtension.GetValue(from, nameSpace);
        }

        /// <summary>
        /// Returns enum column type.
        /// </summary>
        /// <exclude/>
        internal static Type GetEnumType(string fieldName, Type type)
        {
            string[]? Fields = fieldName.Contains('.', StringComparison.InvariantCulture) ? fieldName.Split(".") : null;
            if (Fields != null)
            {
                Type? complexType = null;
                for (int v = 0; v < Fields.Length - 1; v++)
                {
                    complexType = complexType == null ? (type?.GetProperty(Fields[v])?.PropertyType) : (complexType.GetProperty(Fields[v])?.PropertyType);
                }

                return complexType?.GetProperty(Fields[^1])?.PropertyType!;
            }
            else
            {
                return type?.GetProperty(fieldName)?.PropertyType!;
            }
        }

        internal static Func<IEnumerable, string, string, Type, object> CalculateAggregateFunc()
        {
            return (items, property, pd, dataType) =>
            {
                string aggregateType = pd;
                bool isDynamicObjectType = dataType.BaseType == typeof(DynamicObject);
                bool isExpandoObjectType = dataType == typeof(ExpandoObject);

                if (isDynamicObjectType || isExpandoObjectType)
                {
                    IQueryable<IDynamicMetaObjectProvider> dt = items.Cast<IDynamicMetaObjectProvider>().AsQueryable();
                    switch (aggregateType)
                    {
                        case "Count":
                            return dt.Count();
                        case "Max":
                            return dt.Max(item => ReflectionExtension.GetValueFromIDynamicMetaObject(item, property, true))!;
                        case "Min":
                            return dt.Min(item => ReflectionExtension.GetValueFromIDynamicMetaObject(item, property, true))!;
                        case "Average":
                            return dt.Select(item => ReflectionExtension.GetValueFromIDynamicMetaObject(item, property, true)).ToList().Average(value => Convert.ToDouble(value, CultureInfo.InvariantCulture));
                        case "Sum":
                            return dt.Select(item => ReflectionExtension.GetValueFromIDynamicMetaObject(item, property, true)).ToList().Sum(value => Convert.ToDouble(value, CultureInfo.InvariantCulture));
                        case "TrueCount":
                            List<WhereFilter> trueWhereFilter = [new WhereFilter { Field = property, Operator = "equal", value = true }];
                            return DynamicObjectOperation.PerformFiltering(items, trueWhereFilter, null!, null!).Count();
                        case "FalseCount":
                            List<WhereFilter> falseWhereFilter = [new WhereFilter { Field = property, Operator = "equal", value = false }];
                            return DynamicObjectOperation.PerformFiltering(items, falseWhereFilter, null!, null!).Count();
                        default:
                            return null!;
                    }
                }
                else
                {
                    IQueryable queryable = items.AsQueryable();
                    return aggregateType switch
                    {
                        "Count" => queryable.Count(),
                        "Max" => queryable.Max(property),
                        "Min" => queryable.Min(property),
                        "Average" => queryable.Average(property),
                        "Sum" => queryable.Sum(property),
                        "TrueCount" => queryable.Where(property, true, FilterType.Equals, false).Count(),
                        "FalseCount" => queryable.Where(property, false, FilterType.Equals, false).Count(),
                        _ => null!,
                    };
                }
            };
        }

        /// <summary>
        /// Returns the field name to be ignored in Patch request.
        /// </summary>
        /// <exclude/>
        internal static List<IgnorePropertiesByType> _ignoredPropertiesPerType = [];

        internal static List<IgnorePropertiesByType> GetPropertiesToRemove()
        {
            return _ignoredPropertiesPerType;
        }

        private static bool IsDefaultInitialized(PropertyInfo property, object value)
        {
            if (property == null)
            {
                return true;
            }

            Type type = property.PropertyType;

            object? defaultValue = type == typeof(string)
                ? string.Empty
                : type.IsValueType ? Activator.CreateInstance(type) : null;

            return Equals(defaultValue, value);
        }

        internal static object CompareAndRemove(object data, object original, string key = "", bool? IsComplex = null, bool? IsFromBatch = null)
        {
            _ignoredPropertiesPerType.Clear();
            if (original == null && (!IsNullableComplexField || (IsFromBatch == true)))
            {
                return data;
            }

            Type type = data.GetType();
            HashSet<string> propertiesToIgnore = [];

            if (IsComplex.HasValue && IsComplex.Value && IsFromBatch == null)
            {
                propertiesToIgnore = [];
            }

            List<PropertyInfo> props = [.. type.GetProperties()];

            foreach (PropertyInfo prop in props)
            {
                object? propertyValue = prop.GetValue(data);
                PropertyInfo? propInfo = data?.GetType().GetProperty(prop.Name);
                if (IsNullableComplexField && original == null && IsSimpleType(propertyValue!))
                {
                    if (IsDefaultInitialized(propInfo!, propertyValue!))
                    {
                        _ = propertiesToIgnore.Add(prop.Name);
                    }
                }

                PropertyInfo? orgProp = original?.GetType().GetProperty(prop.Name);
                object? originalValue = orgProp?.GetValue(original);
                bool areEqual = Equals(propertyValue, originalValue);
                if (!IsSimpleType(propertyValue!))
                {
                    _ = CompareAndRemove(propertyValue!, originalValue!, IsComplex: true);
                    if (IsFromBatch == null && (propertyValue == null || JsonSerializer.Serialize(propertyValue) == "{}"))
                    {
                        _ = propertiesToIgnore.Add(prop.Name);
                    }
                }
                else if (prop.Name != key && areEqual)
                {
                    _ = propertiesToIgnore.Add(prop.Name);
                }
            }

            // Add properties to the global list
            IgnorePropertiesByType? existingTypeProperties = _ignoredPropertiesPerType.FirstOrDefault(x => x.Type == type);
            if (existingTypeProperties != null)
            {
                existingTypeProperties.Properties.UnionWith(propertiesToIgnore);
            }
            else
            {
                _ignoredPropertiesPerType.Add(new IgnorePropertiesByType(type, propertiesToIgnore));
            }

            return data!;
        }

        private static bool IsSimpleType(object value)
        {
            return value == null || value is string || value.GetType().GetTypeInfo().IsPrimitive || value is TimeSpan ||
                        value is decimal || value is DateTime || value is TimeOnly || value is DateOnly || value is IEnumerable || value is DateTimeOffset ||
                        value is ICollection || value is Guid || value.GetType().GetTypeInfo().IsEnum;
        }


        /// <summary>
        /// Formats the given value.
        /// </summary>
        /// <param name="value">Value to be formatted.</param>
        /// <param name="format">Format string.</param>
        /// <returns>string.</returns>
        public static string GetFormattedValue(object value, string format)
        {
            List<string> Type = ["Double", "Int64", "Int32", "Int16", "Decimal", "Single"];
            string TypeName = value?.GetType()?.Name!;
            return TypeName is "DateTime" or "DateTimeOffset" or "DateOnly" or "TimeOnly"
                ? Intl.GetDateFormat(value!, format)
                : value != null && Type.Any(t => TypeName.Contains(t, StringComparison.Ordinal))
                    ? Intl.GetNumericFormat(value, format)
                    : value?.ToString()!;
        }

        internal static IDictionary<string, Type> GetColumnType(IEnumerable dataSource, bool nullable = true, string? columnName = null)
        {
            _ = columnName;
            Dictionary<string, Type> columnTypes = [];
            List<IDynamicMetaObjectProvider> dynamics = [.. dataSource.AsQueryable().Cast<IDynamicMetaObjectProvider>()];
            Type? rowType = null;
            if (dynamics.Count > 0)
            {
                rowType = dynamics[0].GetType();
            }
            if (rowType == null || rowType.IsSubclassOf(typeof(DynamicObject)))
            {
                return null!;
            }
            List<ExpandoObject> totalRecords = [.. dataSource.AsQueryable().OfType<ExpandoObject>()];
            int count = totalRecords.Count;
            int iteration = 0;
            foreach (ExpandoObject item in totalRecords)
            {
                iteration++;
                IDictionary<string, object?> propertyValues = item;
                foreach (string fields in propertyValues.Keys)
                {
                    object? value = propertyValues[fields];
                    if (value != null && !columnTypes.ContainsKey(fields))
                    {

                        Type type = value.GetType();
                        if (type.IsValueType && nullable)
                        {
                            type = typeof(Nullable<>).MakeGenericType(type);
                        }
                        columnTypes.Add(fields, type);

                    }
                    else if (iteration == count && !columnTypes.ContainsKey(fields))
                    {
                        columnTypes.Add(fields, typeof(object));
                    }
                }
                if (columnTypes.Count == propertyValues.Keys.Count)
                {
                    break;
                }
            }
            return columnTypes;
        }

        internal static string GetAdditionalParams(RequestOptions options)
        {
            string param = string.Empty;

            if (options.Queries != null && options.Queries.Queries.Params != null)
            {
                IDictionary<string, object> additionalParams = options.Queries.Queries.Params;
                if (additionalParams.Count > 0) { param = "?"; }
                foreach (KeyValuePair<string, object> item in additionalParams)
                {
                    param += item.Key + "=" + item.Value;
                    if (additionalParams.Count > 1) { param += "&"; }
                }
            }
            return param;
        }

        internal static string GetODataUrlKey(object rowData, string keyField, object? value = null, Type? ModelType = null)
        {
            object? keyVal = value ?? GetObject(keyField, rowData);
            if (keyVal?.GetType() == typeof(string))
            {
                if ((ModelType != typeof(string)) && (Guid.TryParse((string)keyVal, out _) || int.TryParse((string)keyVal, out _) || decimal.TryParse((string)keyVal, out _) || (keyVal?.GetType() == null)
                 || double.TryParse((string)keyVal, out _)))
                {
                    return $"({keyVal})";
                }
                else if (ModelType != typeof(string) && DateTime.TryParse((string)keyVal, out DateTime _))
                {
                    if (keyVal is string keyStr && Regex.IsMatch(keyStr, @"(Z|[+-]\d{2}:\d{2})$"))
                    {
                        keyVal = DateTimeOffset.Parse(keyStr, CultureInfo.InvariantCulture);
                        return $"({((DateTimeOffset)keyVal).ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture)})";
                    }
                    else
                    {
                        keyVal = Convert.ToDateTime(keyVal, CultureInfo.InvariantCulture);
                        return $"({((DateTime)keyVal).ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture)})";
                    }
                }
                else
                {
                    return $"('{keyVal}')";
                }
            }
            else
            {
                return keyVal?.GetType() == typeof(DateTime)
                    ? $"({((DateTime)keyVal).ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture)})"
                    : keyVal?.GetType() == typeof(DateTimeOffset)
                                    ? $"({((DateTimeOffset)keyVal).ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture)})"
                                    : $"({keyVal})";
            }
        }

        /// <summary>
        /// Gets the property value from the DynamicObject.
        /// </summary>
        /// <param name="obj">Input dynamic object.</param>
        /// <param name="name">Property name to get.</param>
        /// <returns>object.</returns>
        public static object GetDynamicValue(DynamicObject obj, string name)
        {
            object? value = null;
            _ = (obj?.TryGetMember(new DataMemberBinder(name, false), out value));
            return value!;
        }

        /// <summary>
        /// Gets the property value from the ExpandoObject.
        /// </summary>
        /// <param name="obj">Input Expando object.</param>
        /// <param name="name">Property name to get.</param>
        /// <returns>object.</returns>
        public static object GetExpandoValue(IDictionary<string, object> obj, string name)
        {
            object? value = null;
            _ = (obj?.TryGetValue(name, out value));
            return value!;
        }

        internal static object UpdateDictionary(IEnumerable<object> ExpandData, string[] columns)
        {
            List<IDictionary<string, object>> DicData = [];
            IDictionary<string, object> DicValue;
            if (ExpandData != null && !ExpandData.AsQueryable().Any())
            {
                return null!;
            }

            PropertyInfo[]? props = ExpandData?.First()?.GetType().GetProperties();

            if (ExpandData == null)
            {
                return null!;
            }

            foreach (object obj in ExpandData)
            {
                string guid = Guid.NewGuid().ToString();
                if (obj is DynamicObject dynamicObj)
                {
                    _ = dynamicObj.TrySetMember(new DataSetMemberBinder("BlazId", false), "BlazTempId_" + guid);
                    Dictionary<string, object> rowDataHolder = [];
                    foreach (string col in columns)
                    {
                        _ = dynamicObj.TryGetMember(new DataMemberBinder(col, false), out object? value);
                        rowDataHolder.Add(col, value!);
                    }

                    DicData.Add(rowDataHolder);
                }
                else if (obj is ExpandoObject)
                {
                    DicValue = (IDictionary<string, object>)obj;
                    DicValue.AddOrUpdateDataItem("BlazId", "BlazTempId_" + guid);
                    DicData.Add(DicValue);
                }
                else
                {
                    DicValue = ObjectToDictionary(obj, props!);
                    DicValue.AddOrUpdateDataItem("BlazId", "BlazTempId_" + guid);
                    DicData.Add(DicValue);
                }
            }
            return DicData.Count > 0 ? DicData : ExpandData;
        }

        internal static IDictionary<string, object> ObjectToDictionary(object o, PropertyInfo[] props)
        {
            IDictionary<string, object> res = new Dictionary<string, object>();
            for (int i = 0; i < props.Length; i++)
            {
                if (props[i].CanRead &&
                    !Attribute.IsDefined(props[i], typeof(JsonIgnoreAttribute)) && !Attribute.IsDefined(props[i], typeof(JsonIgnoreAttribute)))
                {
                    res.AddOrUpdateDataItem(props[i].Name, props[i].GetValue(o, null)!);
                }
            }

            return res;
        }
    }

    internal static class DataUtilExtension
    {
        internal static void AddOrUpdateDataItem(this IDictionary<string, object> dict, string key, object value)
        {
            if (dict.ContainsKey(key))
            {
                dict[key] = value;
            }
            else
            {
                dict.Add(key, value);
            }
        }
    }
}
