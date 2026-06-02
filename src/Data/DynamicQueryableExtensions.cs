using System.Linq.Expressions;
using System.Reflection;
using System.Globalization;
using System.Dynamic;
using Syncfusion.Blazor.Toolkit.Internal;
using System.Text.RegularExpressions;

namespace Syncfusion.Blazor.Toolkit.Data
{
    /// <summary>
    /// Provides extension methods for building dynamic LINQ queries against <see cref="IQueryable{T}"/> sources,
    /// with support for dynamic objects and runtime expression building.
    /// </summary>
    /// <remarks>
    /// These extensions enable runtime expression building for operations such as filtering, sorting, searching,
    /// grouping, and aggregation. They support <see cref="DynamicObject"/> and <see cref="ExpandoObject"/> types,
    /// allowing dynamic query construction without compile-time type information.
    /// </remarks>
    public static class DynamicQueryableExtensions
    {
        /// <exclude />
        private static readonly string[] _stringSeparator = ["%3f"];
        /// <summary>
        /// Predicate is a Binary expression that needs to be built for a single or a series
        /// of values that needs to be passed on to the WHERE expression.
        /// <para></para>
        /// <para></para>
        /// <code lang="C#">var binaryExp = queryable.Predicate(parameter,
        /// &quot;EmployeeID&quot;, &quot;4&quot;, true);</code>
        /// </summary>
        /// <remarks>
        /// First create a ParameterExpression using the Parameter extension function, then
        /// use the same ParameterExpression to generate the predicates.
        /// </remarks>
        /// <param name="source">Data source.</param>
        /// <param name="paramExpression">Parameter expression to merge.</param>
        /// <param name="propertyName">Property name to be filtered.</param>
        /// <param name="constValue">Const value.</param>
        /// <param name="filterType">Filter operator type.</param>
        /// <param name="filterBehaviour">Specifies the filter behavior.</param>
        /// <param name="isCaseSensitive">Performs the case sensitive if true.</param>
        /// <param name="sourceType">Specifies the data source element type.</param>
        /// <param name="columnType">Specifies the current field type.</param>
        public static Expression Predicate(this IQueryable source, ParameterExpression paramExpression,
                                           string propertyName, object constValue, FilterType filterType,
                                           FilterBehavior filterBehaviour, bool isCaseSensitive, Type sourceType, Type? columnType = null) // Predicate1
        {
            ArgumentNullException.ThrowIfNull(sourceType);
            ArgumentNullException.ThrowIfNull(propertyName);
            return Predicate(source, constValue, filterType, filterBehaviour, isCaseSensitive, sourceType, null!, null!, paramExpression, propertyName, columnType!);
        }

        private static ValueTuple<Expression, Expression?> GetExpression(FilterType filterType,
            Type memberType, object value, Expression memExp,
            bool isCaseSensitive)
        {
            Type nullablememberType = NullableHelperInternal.GetNullableType(memberType);
            Expression? bExp = null;
            switch (filterType)
            {
                case FilterType.Equals:
                    if (isCaseSensitive || memberType != typeof(string))
                    {
                        if (value != null)
                        {
                            var exp = Expression.Constant(value, memberType);
                            if ((nullablememberType == memberType && memberType != typeof(object)) || memberType.GetTypeInfo().IsEnum)
                            {
                                memExp = Expression.Convert(memExp, nullablememberType);
                                bExp = Expression.Equal(memExp, Expression.Constant(value, nullablememberType));
                            }
                            else
                            {
                                bExp = Expression.Call(exp, exp?.Type?.GetMethod("Equals", new[] { memExp.Type })!, memExp);
                            }
                        }
                        else
                        {
                            memExp = Expression.Convert(memExp, nullablememberType);
                            bExp = Expression.Equal(memExp, Expression.Constant(value, nullablememberType));
                        }
                    }
                    else
                    {
                        memExp = Expression.Coalesce(memExp, Expression.Constant(value == null ? "blanks" : string.Empty));
                        var toLowerMethodCall = memExp.ToLowerMethodCallExpression();
                        bExp = Expression.Equal(toLowerMethodCall,
                                                Expression.Constant(
                                                    value == null ? "blanks" : value.ToString()?.ToLowerInvariant(),
                                                    typeof(string)));
                    }

                    break;
                case FilterType.NotEquals:
                    if (isCaseSensitive || memberType != typeof(string))
                    {
                        if (value != null)
                        {
                            memExp = Expression.Convert(memExp, nullablememberType);
                            bExp = Expression.NotEqual(memExp, Expression.Constant(value, nullablememberType));
                        }
                        else
                        {
                            memExp = Expression.Convert(memExp, nullablememberType);
                            bExp = Expression.NotEqual(memExp, Expression.Constant(value, nullablememberType));
                        }
                    }
                    else
                    {
                        memExp = Expression.Coalesce(memExp, Expression.Constant(value == null ? "blanks" : string.Empty));
                        var toLowerMethodCall = memExp.ToLowerMethodCallExpression();
                        bExp = Expression.NotEqual(toLowerMethodCall,
                                                   Expression.Constant(
                                                       value == null ? "blanks" : value.ToString()?.ToLowerInvariant(),
                                                       memberType));
                    }

                    break;
                case FilterType.LessThan:

                    memExp = Expression.Convert(memExp, nullablememberType);
                    bExp = Expression.LessThan(memExp, Expression.Constant(value, nullablememberType));

                    break;
                case FilterType.LessThanOrEqual:

                    memExp = Expression.Convert(memExp, nullablememberType);
                    bExp = Expression.LessThanOrEqual(memExp, Expression.Constant(value, nullablememberType));

                    break;
                case FilterType.GreaterThan:

                    memExp = Expression.Convert(memExp, nullablememberType);
                    bExp = Expression.GreaterThan(memExp, Expression.Constant(value, nullablememberType));

                    break;
                case FilterType.GreaterThanOrEqual:

                    memExp = Expression.Convert(memExp, nullablememberType);
                    bExp = Expression.GreaterThanOrEqual(memExp, Expression.Constant(value, nullablememberType));

                    break;
                case FilterType.IsNull:
                    memExp = Expression.Convert(memExp, nullablememberType);
                    bExp = Expression.Equal(memExp, Expression.Constant(value, nullablememberType));
                    break;
                case FilterType.IsNotNull:
                    memExp = Expression.Convert(memExp, nullablememberType);
                    bExp = Expression.NotEqual(memExp, Expression.Constant(value, nullablememberType));
                    break;
                case FilterType.WildCard:
                    break;
                case FilterType.IsEmpty:
                    break;
                case FilterType.IsNotEmpty:
                    break;
                case FilterType.StartsWith:
                    break;
                case FilterType.DoesNotStartWith:
                    break;
                case FilterType.EndsWith:
                    break;
                case FilterType.DoesNotEndWith:
                    break;
                case FilterType.Contains:
                    break;
                case FilterType.DoesNotContain:
                    break;
                case FilterType.Like:
                    break;
                case FilterType.Undefined:
                    break;
                case FilterType.Between:
                    break;
                default:
                    break;
            }
            return (memExp, bExp);
        }

        private static Expression Predicate(this IQueryable source, object constValue, FilterType filterType,
                                           FilterBehavior filterBehaviour, bool isCaseSensitive, Type sourceType, Type memberType, Expression memExp, ParameterExpression paramExpression, string propertyName, Type columnType = null!)
        {
            ArgumentNullException.ThrowIfNull(source);
            string[]? propertyNameList = null; _ = filterBehaviour;
            int propCount = 1;
            if (memExp == null)
            {
                if (sourceType.IsSubclassOf(typeof(DynamicObject)))
                {
                    Expression param = Expression.Convert(paramExpression, typeof(DynamicObject));
                    MethodInfo? methodName = typeof(DataUtil).GetMethod(nameof(DataUtil.GetDynamicValue));
                    memExp = Expression.Call(methodName!, param, Expression.Constant(propertyName));
                    memberType = columnType;
                    propertyNameList = propertyName.Split('.');
                    propCount = propertyNameList.Length;
                    if (propCount > 1)
                    {
                        memExp = null!;
                        for (int i = 0; i < propCount; i++)
                        {
                            if (memExp == null)
                            {
                                memExp = Expression.Call(methodName!, param, Expression.Constant(propertyNameList[i]));
                            }
                            else
                            {
                                param = Expression.Convert(memExp, typeof(DynamicObject));
                                memExp = Expression.Call(methodName!, param, Expression.Constant(propertyNameList[i]));
                            }
                        }
                    }
                }
                else
                {
                    Expression param = Expression.Convert(paramExpression, typeof(IDictionary<string, object>));
                    memExp = Expression.Property(param, "Item", [Expression.Constant(propertyName)]);
                    memberType = columnType ?? memExp.Type;
                    propertyNameList = propertyName.Split('.');
                    propCount = propertyNameList.Length;
                    if (propCount > 1)
                    {
                        memExp = null!;
                        for (int i = 0; i < propCount; i++)
                        {
                            if (memExp == null)
                            {
                                memExp = Expression.Property(param, "Item", [Expression.Constant(propertyNameList[i])]);
                            }
                            else
                            {
                                param = Expression.Convert(memExp, typeof(IDictionary<string, object>));
                                memExp = Expression.Property(param, "Item", [Expression.Constant(propertyNameList[i])]);
                            }
                        }
                    }
                }
            }

            object? value = constValue;
            Expression? bExp = null;
            if (filterType is FilterType.Equals or FilterType.NotEquals or
                 FilterType.LessThan or FilterType.LessThanOrEqual or
                 FilterType.GreaterThan or FilterType.GreaterThanOrEqual or FilterType.IsNull or FilterType.IsNotNull)
            {
                Type underlyingType = memberType;
                if (NullableHelperInternal.IsNullableType(memberType))
                {
                    underlyingType = NullableHelperInternal.GetUnderlyingType(memberType);
                }

                if (value != null)
                {
                    bool isJsonElement = value.GetType().Name == "JsonElement";
                    value = isJsonElement ? SfBaseUtils.ChangeType(value, underlyingType) : ValueConvert.ChangeType(value, underlyingType, CultureInfo.CurrentCulture);
                }

                ValueTuple<Expression, Expression?> v = GetExpression(filterType, memberType, value!, memExp, isCaseSensitive);
                memExp = v.Item1; bExp = v.Item2;
            }
            else
            {
                if (!isCaseSensitive && (filterType == FilterType.Equals || filterType == FilterType.NotEquals))
                {
                    value = NullableHelperInternal.FixDbNUllasNull(constValue, memberType);
                }

                ValueTuple<Expression, Expression?> v = GetPExpression(filterType, isCaseSensitive, value, memExp, memberType);
                memExp = v.Item1;
                bExp = v.Item2 ?? bExp;
            }

            return bExp!;
        }

        private static ValueTuple<Expression, Expression?> GetPExpression(FilterType filterType,
            bool isCaseSensitive, object value, Expression memExp, Type memberType)
        {
            Expression? bExp = null;
            MethodInfo? toString = memExp.Type.GetMethods().FirstOrDefault(d => d.Name == "ToString");
            string stringValue = string.Empty;
            if (NullableHelperInternal.IsNullableType(memberType) || memberType == typeof(string) || memberType == typeof(object))
            {
                memExp = Expression.Coalesce(memExp, Expression.Constant("Blanks"));
                stringValue = value == null ? "Blanks" : value.ToString()!;
            }
            else
            {
                memExp = Expression.Coalesce(memExp, Expression.Constant(""));
                stringValue = value == null ? "" : value.ToString()!;
            }

            memExp = Expression.Call(memExp, toString!);
            switch (filterType)
            {
                case FilterType.IsEmpty:
                    bExp = Expression.Equal(memExp, Expression.Constant(stringValue, typeof(string)));

                    break;
                case FilterType.IsNotEmpty:
                    bExp = Expression.NotEqual(memExp, Expression.Constant(stringValue, typeof(string)));

                    break;
                case FilterType.NotEquals:
                    if (!isCaseSensitive)
                    {
                        memExp = ToLowerMethodCallExpression(memExp);
                        bExp = Expression.NotEqual(memExp,
                                                    Expression.Constant(stringValue.ToLowerInvariant(), typeof(string)));
                    }
                    else
                    {
                        bExp = Expression.NotEqual(memExp, Expression.Constant(stringValue, typeof(string)));
                    }

                    break;
                case FilterType.Equals:
                case FilterType.StartsWith:
                case FilterType.Contains:
                case FilterType.EndsWith:
                    MethodInfo? stringMethod = typeof(string).GetMethod(filterType.ToString(), [memExp.Type]);
                    if (isCaseSensitive)
                    {
                        bExp = Expression.Call(memExp, stringMethod!, [Expression.Constant(stringValue, typeof(string))]);
                    }
                    else
                    {
                        MethodCallExpression toLowerMethod = ToLowerMethodCallExpression(memExp);
                        bExp = Expression.Call(toLowerMethod, stringMethod!,
                                    [
                                            Expression.Constant(stringValue.ToLowerInvariant(), typeof (string))
                                    ]);
                    }

                    break;
                case FilterType.DoesNotStartWith:
                    MethodInfo? doesNotStartWithMethod = typeof(string).GetMethod("StartsWith", [memExp.Type]);
                    if (isCaseSensitive)
                    {
                        bExp = Expression.Not(Expression.Call(memExp, doesNotStartWithMethod!, [Expression.Constant(stringValue, typeof(string))]));
                    }
                    else
                    {
                        MethodCallExpression toLowerMethod = ToLowerMethodCallExpression(memExp);
                        bExp = Expression.Not(Expression.Call(toLowerMethod, doesNotStartWithMethod!,
                                    [
                                            Expression.Constant(stringValue.ToLower(CultureInfo.CurrentCulture), typeof (string))
                                    ]));
                    }

                    break;
                case FilterType.DoesNotEndWith:
                    MethodInfo? doesNotEndWithMethod = typeof(string).GetMethod("EndsWith", [memExp.Type]);
                    if (isCaseSensitive)
                    {
                        bExp = Expression.Not(Expression.Call(memExp, doesNotEndWithMethod!, [Expression.Constant(stringValue, typeof(string))]));
                    }
                    else
                    {
                        MethodCallExpression toLowerMethod = ToLowerMethodCallExpression(memExp);
                        bExp = Expression.Not(Expression.Call(toLowerMethod, doesNotEndWithMethod!,
                                    [
                                            Expression.Constant(stringValue.ToLower(CultureInfo.CurrentCulture), typeof (string))
                                    ]));
                    }

                    break;
                case FilterType.DoesNotContain:
                    MethodInfo? doesNotContainMethod = typeof(string).GetMethod("Contains", [memExp.Type]);
                    if (isCaseSensitive)
                    {
                        bExp = Expression.Not(Expression.Call(memExp, doesNotContainMethod!, [Expression.Constant(stringValue, typeof(string))]));
                    }
                    else
                    {
                        MethodCallExpression toLowerMethod = ToLowerMethodCallExpression(memExp);
                        bExp = Expression.Not(Expression.Call(toLowerMethod, doesNotContainMethod!,
                                    [
                                            Expression.Constant(stringValue.ToLower(CultureInfo.CurrentCulture), typeof (string))
                                    ]));
                    }

                    break;
                case FilterType.Like:
                    MethodInfo? likeMethod = typeof(EnumerableExtensions).GetMethod("Like", [typeof(string), typeof(string)]);
                    if (isCaseSensitive)
                    {
                        bExp = Expression.Call(null, likeMethod!, memExp, Expression.Constant(stringValue, typeof(string)));
                    }
                    else
                    {
                        MethodCallExpression toLowerMethod = ToLowerMethodCallExpression(memExp);
                        bExp = Expression.Call(null, likeMethod!, toLowerMethod, Expression.Constant(stringValue.ToLower(CultureInfo.CurrentCulture), typeof(string)));
                    }
                    break;
                case FilterType.WildCard:
                    MethodInfo? regexMethod = typeof(Regex).GetMethod("IsMatch", [memExp.Type, typeof(string)]);
                    string regexPattern = WildcardToRegex(stringValue);

                    if (isCaseSensitive)
                    {
                        bExp = Expression.Call(Expression.Constant(regexPattern), regexMethod!, memExp);
                    }
                    else
                    {
                        MethodCallExpression toLowerMethod = ToLowerMethodCallExpression(memExp);
                        bExp = Expression.Call(null, regexMethod!, toLowerMethod, Expression.Constant(regexPattern.ToLower(CultureInfo.CurrentCulture), typeof(string)));

                    }
                    break;
            }

            return (memExp, bExp);
        }

        private static string WildcardToRegex(string pattern)
        {
            string[] asteriskSplit;
            string[] optionalSplit;

            if (pattern.Contains('*', StringComparison.Ordinal))
            {
                if (pattern[0] != '*')
                {
                    pattern = '^' + pattern;
                }

                if (pattern[^1] != '*')
                {
                    pattern += '$';
                }

                asteriskSplit = pattern.Split('*');

                for (int i = 0; i < asteriskSplit.Length; i++)
                {
                    asteriskSplit[i] = !asteriskSplit[i].Contains('.', StringComparison.Ordinal) ? asteriskSplit[i] + ".*" : asteriskSplit[i] + '*';
                }

                pattern = string.Join("", asteriskSplit);
            }

            if (pattern.Contains("%3f", StringComparison.Ordinal) || pattern.Contains('?', StringComparison.Ordinal))
            {
                optionalSplit = pattern.Contains("%3f", StringComparison.Ordinal) ? pattern.Split(_stringSeparator, StringSplitOptions.None) : pattern.Split('?');
                pattern = string.Join(".", optionalSplit);
            }

            return pattern;

        }

        private static MethodCallExpression ToLowerMethodCallExpression(this Expression memExp)
        {
            MethodInfo? tolowerMethod = typeof(string).GetMethods().FirstOrDefault(m => m.Name == "ToLower");
            if (memExp.Type == typeof(object))
            {
                memExp = Expression.Convert(memExp, typeof(string));
            }

            MethodCallExpression toLowerMethodCall = Expression.Call(memExp, tolowerMethod!, []);
            return toLowerMethodCall;
        }
    }
}