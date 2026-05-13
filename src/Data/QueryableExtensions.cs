using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Globalization;
using System.Dynamic;
using Syncfusion.Blazor.Toolkit.Data;
using System.Text.RegularExpressions;

namespace Syncfusion.Blazor.Toolkit.Data
{
    /// <summary>
    /// Provides extension methods for Queryable source.
    /// <para></para>
    /// <para></para>
    /// <para>var fonts = FontFamily.Families.AsQueryable();. </para>
    /// <para></para>
    /// <para></para>
    /// <para>We would normally write Expressions as,. </para>
    /// <para></para>
    /// <code lang="C#">var names = new string[] {&quot;Tony&quot;, &quot;Al&quot;,
    /// &quot;Sean&quot;, &quot;Elia&quot;}.AsQueryable();
    /// names.OrderBy(n=&gt;n);</code>
    /// <para></para>
    /// <para></para>
    /// <para>This would sort the names based on alphabetical order. Like so, the
    /// Queryable extensions are a set of extension methods that define functions which
    /// will generate expressions based on the supplied values to the functions.</para>
    /// </summary>
    /// <exclude/>
    public static class QueryableExtensions
    {
        private static readonly Type[] _emptyTypes = Type.EmptyTypes;

        private static readonly string[] _stringSeparator = ["%3f"];

        /// <summary>
        /// Converts a non-generic <see cref="IEnumerable"/> into a queryable sequence that preserves the runtime element type.
        /// </summary>
        /// <param name="items">The source items.</param>
        /// <returns>A queryable sequence when a runtime element type can be determined; otherwise the original sequence.</returns>
        public static IEnumerable OfQueryable(this IEnumerable items)
        {
            IEnumerator? enumerator = items?.GetEnumerator();
            if (enumerator != null && enumerator.MoveNext())
            {
                if (enumerator.Current != null)
                {
                    Type type = enumerator.Current.GetType();
                    IQueryable? queryable = items?.AsQueryable();
                    return queryable?.OfType(type)!;
                }
            }

            return items!;
        }

        /// <summary>
        /// Converts a non-generic <see cref="IEnumerable"/> into a queryable sequence using the specified element type.
        /// </summary>
        /// <param name="items">The source items.</param>
        /// <param name="sourceType">The runtime source type.</param>
        /// <returns>A queryable sequence with the specified source type.</returns>
        public static IEnumerable OfQueryable(this IEnumerable items, Type sourceType)
        {
            IQueryable queryable = items.AsQueryable();
            return queryable.OfType(sourceType);
        }

        /// <summary>
        /// Generates an AND binary expression for the given Binary expressions.
        /// <para></para>
        /// </summary>
        /// <param name="expr1"></param>
        /// <param name="expr2"></param>
        public static BinaryExpression AndPredicate(this Expression expr1, Expression expr2)
        {
            return Expression.And(expr1, expr2);
        }

        /// <summary>
        /// Generates an <see cref="Expression.AndAlso(Expression, Expression)"/> binary expression.
        /// </summary>
        /// <param name="expr1">The left expression.</param>
        /// <param name="expr2">The right expression.</param>
        /// <returns>The combined binary expression.</returns>
        public static BinaryExpression AndAlsoPredicate(this Expression expr1, Expression expr2)
        {
            return Expression.AndAlso(expr1, expr2);
        }

        /// <summary>
        /// Generates an <see cref="Expression.OrElse(Expression, Expression)"/> binary expression.
        /// </summary>
        /// <param name="expr1">The left expression.</param>
        /// <param name="expr2">The right expression.</param>
        /// <returns>The combined binary expression.</returns>
        public static BinaryExpression OrElsePredicate(this Expression expr1, Expression expr2)
        {
            return Expression.OrElse(expr1, expr2);
        }

        /// <summary>
        /// Returns the number of elements in the queryable source.
        /// </summary>
        /// <param name="source">The queryable source.</param>
        /// <returns>The element count.</returns>
        public static int Count(this IQueryable source)
        {
            Type? sourceType = source?.ElementType;
            return (int)source?.Provider.Execute(
                Expression.Call(
                    typeof(Queryable),
                    "Count",
                    [sourceType!],
                    [source.Expression]))!;
        }

        /// <summary>
        /// Returns the element at the specified index from the queryable source.
        /// </summary>
        /// <param name="source">The queryable source.</param>
        /// <param name="index">Zero-based index of the element.</param>
        /// <param name="sourceType">The source element type.</param>
        /// <returns>The element at the specified index.</returns>
        public static object ElementAt(this IQueryable source, int index, Type sourceType)
        {
            return source?.Provider.Execute(
                Expression.Call(
                    typeof(Queryable),
                    "ElementAt",
                    [sourceType],
                    [source.Expression, Expression.Constant(index)]))!;
        }

        /// <summary>
        /// Returns the element at the specified index from the queryable source.
        /// </summary>
        /// <param name="source">The queryable source.</param>
        /// <param name="index">Zero-based index of the element.</param>
        /// <returns>The element at the specified index.</returns>
        public static object ElementAt(this IQueryable source, int index)
        {
            Type? sourceType = source?.ElementType;
            return source?.ElementAt(index, sourceType!)!;
        }

        /// <summary>
        /// Returns the element at the specified index or a default value when the index is out of range.
        /// </summary>
        /// <param name="source">The queryable source.</param>
        /// <param name="index">Zero-based index of the element.</param>
        /// <param name="sourceType">The source element type.</param>
        /// <returns>The element at the specified index or the default value.</returns>
        public static object ElementAtOrDefault(this IQueryable source, int index, Type sourceType)
        {
            return source?.Provider.Execute(
                Expression.Call(
                    typeof(Queryable),
                    "ElementAtOrDefault",
                    [sourceType],
                    [source.Expression, Expression.Constant(index)]))!;
        }

        /// <summary>
        /// Returns the element at the specified index or a default value when the index is out of range.
        /// </summary>
        /// <param name="source">The queryable source.</param>
        /// <param name="index">Zero-based index of the element.</param>
        /// <returns>The element at the specified index or the default value.</returns>
        public static object ElementAtOrDefault(this IQueryable source, int index)
        {
            Type? sourceType = source?.ElementType;
            return source?.ElementAtOrDefault(index, sourceType!)!;
        }

        /// <summary>
        /// Filters the source sequence to only the elements that are assignable to the specified type.
        /// </summary>
        /// <param name="source">The queryable source.</param>
        /// <param name="sourceType">The type to filter by.</param>
        /// <returns>A queryable sequence containing only the matching elements.</returns>
        public static IQueryable OfType(this IQueryable source, Type sourceType)
        {
            return source?.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable), "OfType",
                    [sourceType], [source.Expression]))!;
        }

        /// <summary>
        /// Generates a OrderBy query for the Queryable source.
        /// <para></para>
        /// <code lang="C#">            DataClasses1DataContext db = new
        /// DataClasses1DataContext();
        ///             var orders = db.Orders.Skip(0).Take(10).ToList();
        ///             var queryable = orders.AsQueryable();
        ///             var sortedOrders =
        /// queryable.OrderBy(&quot;ShipCountry&quot;);</code>
        /// <para></para>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="sourceType"></param>
        public static IQueryable OrderBy(this IQueryable source, string propertyName, Type sourceType)
        {
            ArgumentNullException.ThrowIfNull(source);
            ParameterExpression paramExpression = Expression.Parameter(source.ElementType, sourceType?.Name);
            LambdaExpression lambda = GetLambdaWithComplexPropertyNullCheck(source, propertyName ?? string.Empty, paramExpression, sourceType!);

            return source.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable), "OrderBy",
                    [source.ElementType, lambda.Body.Type],
                    [source.Expression, lambda]));
        }

        /// <summary>
        /// Generates lambda expression for the complex properties.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="paramExpression"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        private static LambdaExpression GetLambdaWithComplexPropertyNullCheck(IEnumerable source, string propertyName,
                                                                              ParameterExpression paramExpression, Type sourceType)
        {
            LambdaExpression? lambda; _ = source;
            string[] properties = propertyName.Split(_separator);
            if (properties.GetLength(0) > 1)
            {
                // has complex properties... need to check each level for null & return null if any level is null...
                Expression memExp = paramExpression.GetValueExpression(propertyName, sourceType);

                // make memExp type object so it can be compared with null below
                if (memExp.Type != typeof(object))
                {
                    memExp = Expression.Convert(memExp, typeof(object));
                }

                Expression? memExp2 = null;
                string name = string.Empty;
                int count = properties.GetLength(0);
                for (int i = 0; i < count; i++)
                {
                    if (i == 0) // the first one
                    {
                        memExp2 = Expression.Equal(
                            paramExpression.GetValueExpression(properties[i], sourceType), Expression.Constant(null));
                        name = properties[i];
                    }
                    else if (i < count - 1) // don't add the last inner property check as it will be added after this loop
                    {
                        name += '.' + properties[i];
                        memExp2 = Expression.OrElse(
                            memExp2!,
                            Expression.Equal(paramExpression.GetValueExpression(name, sourceType), Expression.Constant(null)));
                    }
                }
                memExp2 = Expression.Condition(memExp2!, Expression.Constant(null), memExp);
                lambda = Expression.Lambda(memExp2, paramExpression);
            }
            else
            {
                Expression memExp = paramExpression.GetValueExpression(propertyName, sourceType);
                lambda = Expression.Lambda(memExp, paramExpression);
            }

            return lambda;
        }

        /// <summary>
        /// Generates a lambda expression for complex properties and protects nested member access with null checks.
        /// </summary>
        /// <param name="source">The source sequence.</param>
        /// <param name="propertyName">The property path.</param>
        /// <param name="paramExpression">The lambda parameter.</param>
        /// <param name="sourceType">The source type.</param>
        /// <returns>A lambda expression for ordering or selection.</returns>
        private static LambdaExpression GetLambdaWithComplexPropertyNullCheck(IQueryable source, string propertyName,
                                                                              ParameterExpression paramExpression, Type sourceType)
        {
            LambdaExpression? lambda; _ = source;
            string[] properties = propertyName.Split(_separator);

            if (properties.GetLength(0) > 1)
            {
                // has complex properties... need to check each level for null & return null if any level is null...
                Expression memExp = paramExpression.GetValueExpression(propertyName, sourceType);

                // make memExp type object so it can be compared with null below
                if (memExp.Type != typeof(object))
                {
                    memExp = Expression.Convert(memExp, typeof(object));
                }

                Expression? memExp2 = null;
                string name = string.Empty;
                int count = properties.GetLength(0);
                for (int i = 0; i < count; i++)
                {
                    if (i == 0) // the first one
                    {
                        memExp2 = Expression.Equal(
                            paramExpression.GetValueExpression(properties[i], sourceType), Expression.Constant(null));
                        name = properties[i];
                    }
                    else if (i < count - 1) // don't add the last inner property check as it will be added after this loop
                    {
                        name += '.' + properties[i];
                        memExp2 = Expression.OrElse(
                            memExp2!,
                            Expression.Equal(paramExpression.GetValueExpression(name, sourceType), Expression.Constant(null)));
                    }
                }
                memExp2 = Expression.Condition(memExp2!, Expression.Constant(null), memExp);
                lambda = Expression.Lambda(memExp2, paramExpression);
            }
            else
            {
                Expression memExp = paramExpression.GetValueExpression(propertyName, sourceType);
                lambda = Expression.Lambda(memExp, paramExpression);
            }

            return lambda;
        }

        /// <summary>
        /// Generates an order-by query using a custom expression factory.
        /// </summary>
        /// <param name="source">The queryable source.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>The ordered queryable.</returns>
        public static IQueryable OrderBy(this IQueryable source, string propertyName)
        {
            ArgumentNullException.ThrowIfNull(source);
            Type sourceType = source.ElementType;
            return source.OrderBy(propertyName, sourceType);
        }

        /// <summary>
        /// Generates an order-by query using a custom expression factory.
        /// </summary>
        /// <param name="source">The queryable source.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="expressionFunc">The expression factory.</param>
        /// <returns>The ordered queryable.</returns>
        public static IQueryable OrderBy(this IQueryable source, string propertyName,
                                         Expression<Func<string, object, object>> expressionFunc)
        {
            ArgumentNullException.ThrowIfNull(source);
            Type sourceType = source.ElementType;
            ParameterExpression paramExpression = Expression.Parameter(sourceType, sourceType?.Name);
            ConstantExpression cExp = Expression.Constant(propertyName);
            InvocationExpression iExp = Expression.Invoke(expressionFunc, [cExp, paramExpression]);
            return OrderBy(source, paramExpression, iExp);
        }

        /// <summary>
        /// Generates an order-by query using a comparer and a custom expression factory.
        /// </summary>
        /// <param name="source">The queryable source.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="comparer">The comparer to use.</param>
        /// <param name="expressionFunc">The expression factory.</param>
        /// <returns>The ordered queryable.</returns>
        public static IQueryable OrderBy(this IQueryable source, string propertyName, IComparer<object> comparer,
                                         Expression<Func<string, object, object>> expressionFunc)
        {
            ArgumentNullException.ThrowIfNull(source);
            Type sourceType = source.ElementType;
            ParameterExpression paramExpression = Expression.Parameter(sourceType, sourceType?.Name);
            ConstantExpression cExp = Expression.Constant(propertyName);
            InvocationExpression iExp = Expression.Invoke(expressionFunc, [cExp, paramExpression]);
            LambdaExpression lambda = Expression.Lambda(iExp, paramExpression);
            MethodInfo method = typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name == "OrderBy" && m.GetParameters().Length == 3);
            ConstantExpression conExp = Expression.Constant(comparer, typeof(IComparer<object>));
            Expression methodExp = Expression.Call(null,
                                                             method!.MakeGenericMethod(
                                                                 [
                                                                    source.ElementType, lambda.Body.Type
                                                                 ]),
                [source.Expression, lambda, conExp]);
            return source.Provider.CreateQuery(methodExp);
        }

        /// <summary>
        /// Generates an order-by query from a parameter expression and member expression.
        /// </summary>
        /// <param name="source">The queryable source.</param>
        /// <param name="paramExpression">The parameter expression.</param>
        /// <param name="mExp">The member expression.</param>
        /// <returns>The ordered queryable.</returns>
        public static IQueryable OrderBy(this IQueryable source, ParameterExpression paramExpression, Expression mExp)
        {
            ArgumentNullException.ThrowIfNull(source);
            LambdaExpression lambda = Expression.Lambda(mExp, paramExpression);
            IQueryable orderedSource = source.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable),
                    "OrderBy",
                    [source.ElementType, lambda.Body.Type],
                    source.Expression,
                    lambda));
            return orderedSource;
        }

        /// <summary>
        /// Generates an OrderBy query for the IComparer defined.
        /// <code lang="C#">   public class OrdersComparer :
        /// IComparer&lt;Order&gt;
        ///     {
        ///         public int Compare(Order x, Order y)
        ///         {
        ///             return string.Compare(x.ShipCountry, y.ShipCountry);
        ///         }
        ///     }</code>
        /// <para></para>
        /// <para><code lang="C#">var sortedOrders =
        /// db.Orders.Skip(0).Take(5).ToList().OrderBy(o =&gt; o, new
        /// OrdersComparer());</code></para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="comparer"></param>
        /// <param name="sourceType"></param>
        public static IQueryable OrderBy<T>(this IQueryable source, IComparer<T> comparer, Type sourceType)
        {
            ArgumentNullException.ThrowIfNull(source);
            ParameterExpression paramExpression = Expression.Parameter(source.ElementType, sourceType?.Name);
            LambdaExpression lambda = Expression.Lambda(paramExpression, paramExpression);
            MethodInfo? method = typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name == "OrderBy" && m.GetParameters().Length == 3);
            ConstantExpression conExp = Expression.Constant(comparer, typeof(IComparer<T>));
            MethodCallExpression methodExp = Expression.Call(null,
                                                             method!.MakeGenericMethod(
                                                                 [
                                                                    source.ElementType, lambda.Body.Type
                                                                 ]),
                [source.Expression, lambda, conExp]);
            return source.Provider.CreateQuery(methodExp);
        }

        /// <summary>
        /// Generates an OrderBy query for the IComparer defined.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="comparer"></param>
        public static IQueryable OrderBy<T>(this IQueryable source, IComparer<T> comparer)
        {
            ArgumentNullException.ThrowIfNull(source);
            Type sourceType = source.ElementType;
            return source.OrderBy(comparer, sourceType);
        }

        /// <summary>
        /// Generates an OrderBy query for the IComparer defined.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="comparer"></param>
        /// <param name="sourceType"></param>
        public static IQueryable OrderBy(this IQueryable source, string propertyName, IComparer<object> comparer,
                                         Type sourceType)
        {
            ArgumentNullException.ThrowIfNull(source); _ = propertyName;
            ParameterExpression paramExpression = Expression.Parameter(source.ElementType, sourceType?.Name);
            // var memExp = Expression.PropertyOrField(paramExpression, propertyName);
            LambdaExpression lambda = Expression.Lambda(paramExpression, paramExpression);
            MethodInfo? method = typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name == "OrderBy" && m.GetParameters().Length == 3);
            ConstantExpression conExp = Expression.Constant(comparer, typeof(IComparer<object>));
            MethodCallExpression methodExp = Expression.Call(null,
                                                             method!.MakeGenericMethod(
                                                                 [
                                                                    source.ElementType, lambda.Body.Type
                                                                 ]),
                [source.Expression, lambda, conExp]);
            return source.Provider.CreateQuery(methodExp);
        }

        /// <summary>
        /// Generates an OrderByDescending query for the IComparer defined.
        /// <para></para>
        /// <para> </para>
        /// <code lang="C#">   public class OrdersComparer :
        /// IComparer&lt;Order&gt;
        ///     {
        ///         public int Compare(Order x, Order y)
        ///         {
        ///             return string.Compare(x.ShipCountry, y.ShipCountry);
        ///         }
        ///     }</code>
        /// <para></para>
        /// <para><code lang="C#">var sortedOrders =
        /// db.Orders.Skip(0).Take(5).ToList().OrderByDescending(o =&gt; o, new
        /// OrdersComparer());</code></para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="comparer"></param>
        /// <param name="sourceType"></param>
        public static IQueryable OrderByDescending<T>(this IQueryable source, IComparer<T> comparer, Type sourceType)
        {
            ArgumentNullException.ThrowIfNull(source);
            ParameterExpression paramExpression = Expression.Parameter(source.ElementType, sourceType?.Name);
            LambdaExpression lambda = Expression.Lambda(paramExpression, paramExpression);
            MethodInfo? method =
                typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name == "OrderByDescending" && m.GetParameters().Length == 3);
            ConstantExpression conExp = Expression.Constant(comparer, typeof(IComparer<T>));
            MethodCallExpression methodExp = Expression.Call(null,
                                                             method!.MakeGenericMethod(
                                                                 [
                                                                    source.ElementType, lambda.Body.Type
                                                                 ]),
                [source.Expression, lambda, conExp]);
            return source.Provider.CreateQuery(methodExp);
        }

        /// <summary>
        /// Generates an OrderByDescending query for the IComparer defined.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="comparer"></param>
        /// <param name="sourceType"></param>
        public static IQueryable OrderByDescending(this IQueryable source, string propertyName,
                                                   IComparer<object> comparer, Type sourceType)
        {
            ArgumentNullException.ThrowIfNull(source); _ = propertyName;
            ParameterExpression paramExpression = Expression.Parameter(source.ElementType, sourceType?.Name);
            // var memExp = Expression.PropertyOrField(paramExpression, propertyName);
            LambdaExpression lambda = Expression.Lambda(paramExpression, paramExpression);
            MethodInfo? method =
                typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name == "OrderByDescending" && m.GetParameters().Length == 3);
            ConstantExpression conExp = Expression.Constant(comparer, typeof(IComparer<object>));
            MethodCallExpression methodExp = Expression.Call(null,
                                                             method!.MakeGenericMethod(
                                                                 [
                                                                    source.ElementType, lambda.Body.Type
                                                                 ]),
                [source.Expression, lambda, conExp]);
            return source.Provider.CreateQuery(methodExp);
        }

        /// <summary>
        /// Generates an OrderByDescending query for the IComparer defined.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="comparer"></param>
        public static IQueryable OrderByDescending<T>(this IQueryable source, IComparer<T> comparer)
        {
            ArgumentNullException.ThrowIfNull(source);
            Type sourceType = source.ElementType;
            return source.OrderByDescending(comparer, sourceType);
        }

        /// <summary>
        /// Generates a OrderByDescending query for the Queryable source.
        /// <para></para>
        /// <code lang="C#">            DataClasses1DataContext db = new
        /// DataClasses1DataContext();
        ///             var orders = db.Orders.Skip(0).Take(10).ToList();
        ///             var queryable = orders.AsQueryable();
        ///             var sortedOrders =
        /// queryable.OrderByDescending(&quot;ShipCountry&quot;);</code>
        /// <para></para>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="sourceType"></param>
        public static IQueryable OrderByDescending(this IQueryable source, string propertyName, Type sourceType)
        {
            ArgumentNullException.ThrowIfNull(source);
            ParameterExpression paramExpression = Expression.Parameter(source.ElementType, sourceType?.Name);
            // Expression memExp = paramExpression.GetValueExpression(propertyName, source.ElementType);
            // LambdaExpression lambda = Expression.Lambda(memExp, paramExpression);
            LambdaExpression lambda = GetLambdaWithComplexPropertyNullCheck(source, propertyName ?? string.Empty, paramExpression, sourceType!);

            // Previously  source.ElementType passed as parameter. This will leads to conflict when we use different classes derived from one interface. Now passing sourType as parameter to resolve this issue.
            return source.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable),
                    "OrderByDescending",
                    [source.ElementType, lambda.Body.Type],
                    source.Expression,
                    lambda));
        }

        /// <summary>
        /// Returns a member-access expression for the specified property name.
        /// </summary>
        /// <param name="paramExpression">The lambda parameter.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>The member-access expression.</returns>
        public static Expression GetExpression(this ParameterExpression paramExpression, string propertyName)
        {
            return paramExpression.GetValueExpression(propertyName, paramExpression?.Type!);
        }

        /// <summary>
        /// Generate expression from simple and complex property.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="sourceType"></param>
        /// <param name="paramExpression"></param>
        /// <returns></returns>
        public static Expression GetValueExpression(this ParameterExpression paramExpression, string propertyName,
                                                    Type sourceType)
        {
            Expression? exp = null;
            bool isExpando = false;
            bool isDynamic = false;
            // Split the complex property to simple property and generate member expression
            string[] propertyNameList = propertyName?.Split('.') ?? [];
            foreach (string property in propertyNameList)
            {
                if (exp != null)
                {
                    if (string.Equals(nameof(ExpandoObject), exp.Type.Name, StringComparison.Ordinal) || isExpando)
                    {
                        // handle Expando object
                        Expression param = Expression.Convert(exp, typeof(IDictionary<string, object>));
                        exp = Expression.Property(param, "Item", [Expression.Constant(property)]);
                        isExpando = true;
                    }
                    else if (exp.Type.IsSubclassOf(typeof(DynamicObject)) || isDynamic)
                    {
                        // handle Dynamic object
                        Expression param = Expression.Convert(exp, typeof(DynamicObject));
                        MethodInfo methodName = typeof(DataUtil).GetMethod(nameof(DataUtil.GetDynamicValue))!;
                        exp = Expression.Call(methodName, param, Expression.Constant(property));
                        isDynamic = true;
                    }
                    else if (!isDynamic && !isExpando)
                    {
                        exp = Expression.PropertyOrField(exp, property);
                    }
                }
                else
                {
                    exp = paramExpression;
                    if (paramExpression?.Type != sourceType)
                    {
                        exp = Expression.Convert(paramExpression!, sourceType);
                    }

                    exp = Expression.PropertyOrField(exp!, property);
                }
            }
            return exp!;
        }

        /// <summary>
        /// Generates a OrderByDescending query for the Queryable source.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        public static IQueryable OrderByDescending(this IQueryable source, string propertyName)
        {
            ArgumentNullException.ThrowIfNull(source);
            Type sourceType = source.ElementType;
            return source.OrderByDescending(propertyName, sourceType);
        }

        /// <summary>
        /// Generates a OrderByDescending query for the Queryable source.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="comparer"></param>
        /// <param name="expressionFunc"></param>
        public static IQueryable OrderByDescending(this IQueryable source, string propertyName,
                                                   IComparer<object> comparer,
                                                   Expression<Func<string, object, object>> expressionFunc)
        {
            ArgumentNullException.ThrowIfNull(source);
            Type sourceType = source.ElementType;
            ParameterExpression paramExpression = Expression.Parameter(sourceType, sourceType?.Name);
            ConstantExpression cExp = Expression.Constant(propertyName);
            InvocationExpression iExp = Expression.Invoke(expressionFunc, [cExp, paramExpression]);
            LambdaExpression lambda = Expression.Lambda(iExp, paramExpression);
            MethodInfo? method = typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name == "OrderByDescending" && m.GetParameters().Length == 3);
            ConstantExpression conExp = Expression.Constant(comparer, typeof(IComparer<object>));
            MethodCallExpression methodExp = Expression.Call(null,
                                                             method!.MakeGenericMethod(
                                                                 [
                                                                    source.ElementType, lambda.Body.Type
                                                                 ]),
                [source.Expression, lambda, conExp]);
            return source.Provider.CreateQuery(methodExp);
        }

        /// <summary>
        /// Generates a descending order-by query using a custom expression factory.
        /// </summary>
        /// <param name="source">The queryable source.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="expressionFunc">The expression factory.</param>
        /// <returns>The ordered queryable.</returns>
        public static IQueryable OrderByDescending(this IQueryable source, string propertyName,
                                                   Expression<Func<string, object, object>> expressionFunc)
        {
            ArgumentNullException.ThrowIfNull(source);
            Type sourceType = source.ElementType;
            ParameterExpression paramExpression = Expression.Parameter(sourceType, sourceType?.Name);
            ConstantExpression cExp = Expression.Constant(propertyName);
            InvocationExpression iExp = Expression.Invoke(expressionFunc, [cExp, paramExpression]);
            return OrderByDescending(source, paramExpression, iExp);
        }

        /// <summary>
        /// Generates a descending order-by query from a parameter expression and member expression.
        /// </summary>
        /// <param name="source">The queryable source.</param>
        /// <param name="paramExpression">The parameter expression.</param>
        /// <param name="mExp">The member expression.</param>
        /// <returns>The ordered queryable.</returns>
        public static IQueryable OrderByDescending(this IQueryable source, ParameterExpression paramExpression,
                                                   Expression mExp)
        {
            ArgumentNullException.ThrowIfNull(source);
            LambdaExpression lambda = Expression.Lambda(mExp, paramExpression);
            _ = source.ElementType;
            IQueryable orderedSource = source.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable),
                    "OrderByDescending",
                    [source.ElementType, lambda.Body.Type],
                    source.Expression,
                    lambda));
            return orderedSource;
        }

        /// <summary>
        /// Generates an OR binary expression for the given Binary expressions.
        /// <para></para>
        /// </summary>
        /// <param name="expr1"></param>
        /// <param name="expr2"></param>
        public static BinaryExpression OrPredicate(this Expression expr1, Expression expr2)
        {
            return Expression.Or(expr1, expr2);
        }

        /// <summary>
        /// Creates a ParameterExpression that is required when building a series of
        /// predicates for the WHERE filter.
        /// <para></para>
        /// <code lang="C#">        DataClasses1DataContext db = new
        /// DataClasses1DataContext();
        ///         var orders = db.Orders.Skip(0).Take(100).ToList();
        ///         var queryable = orders.AsQueryable();
        ///         var parameter =
        /// queryable.Parameter();</code>
        /// <para></para>
        /// <para></para>Use this same parameter passed to generate different predicates and
        /// finally to generate the Lambda.
        /// </summary>
        /// <remarks>
        /// If we specify a parameter for every predicate, then the Lambda expression scope
        /// will be out of the WHERE query that gets generated.
        /// </remarks>
        /// <param name="source"></param>
        public static ParameterExpression Parameter(this IQueryable source)
        {
            ArgumentNullException.ThrowIfNull(source);
            Type sourceType = source.ElementType;
            ParameterExpression paramExpression = Expression.Parameter(sourceType, sourceType?.Name);
            return paramExpression;
        }

        /// <summary>
        /// Creates a parameter expression for the specified source type.
        /// </summary>
        /// <param name="sourceType">The source type.</param>
        /// <returns>A parameter expression.</returns>
        public static ParameterExpression Parameter(this Type sourceType)
        {
            return Expression.Parameter(sourceType, sourceType?.Name);
        }

        /// <summary>
        /// Builds an equality expression for the specified property and value.
        /// </summary>
        /// <param name="paramExpression">The lambda parameter.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="value">The comparison value.</param>
        /// <returns>An equality expression.</returns>
        public static Expression Equal(this ParameterExpression paramExpression, string propertyName, object value)
        {
            Expression memExp = paramExpression.GetValueExpression(propertyName, paramExpression?.Type!);
            object result = NullableHelperInternal.FixDbNUllasNull(value, memExp.Type);
            result = NullableHelperInternal.ChangeType(result, memExp.Type, CultureInfo.InvariantCulture);
            BinaryExpression bExp = Expression.Equal(memExp, Expression.Constant(result, memExp.Type));
            return bExp;
        }

        /// <summary>
        /// Builds an equality expression for the specified property using a custom expression factory.
        /// </summary>
        /// <param name="paramExpression">The lambda parameter.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="propertyName2">The expression factory.</param>
        /// <returns>An equality expression.</returns>
        public static BinaryExpression Equal(this ParameterExpression paramExpression, string propertyName,
                                             string propertyName2)
        {
            Expression memExp = paramExpression.GetExpression(propertyName); _ = propertyName2;
            Expression memExp2 = paramExpression.GetExpression(propertyName);
            BinaryExpression bExp = Expression.Equal(memExp, memExp2);
            return bExp;
        }

        /// <summary>
        /// Builds an equality expression for the specified property using a custom expression factory.
        /// </summary>
        /// <param name="paramExpression">The lambda parameter.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="value">The comparison value.</param>
        /// <param name="elementType">The target element type.</param>
        /// <param name="expressionFunc">The expression factory.</param>
        /// <returns>An equality expression.</returns>
        public static Expression Equal(this ParameterExpression paramExpression, string propertyName, object value,
                                       Type elementType, Expression<Func<string, object, object>> expressionFunc)
        {
            // constructing a wrapper Func that would return typed value
            MethodInfo[] methods = [.. typeof(QueryableExtensions).GetMethods(BindingFlags.Static | BindingFlags.NonPublic)];
            MethodInfo? wrapperFuncMethod =
                methods.FirstOrDefault(
                    m => m.Name == "GetInvokeExpressionAggregateFunc" && m.IsStatic && m.IsPrivate && m.IsGenericMethod);
            MethodInfo? genericWrapper = wrapperFuncMethod?.MakeGenericMethod([elementType]);
            Expression invokeExp =
                (Expression)genericWrapper?.Invoke(null, [paramExpression, propertyName, expressionFunc])!;
            value = NullableHelperInternal.ChangeType(value, elementType, CultureInfo.InvariantCulture);
            Expression rightOperandExpression = Expression.Constant(value);
            if (rightOperandExpression.Type != elementType)
            {
                rightOperandExpression = Expression.Convert(rightOperandExpression, invokeExp.Type);
            }

            BinaryExpression bExp = Expression.Equal(invokeExp, rightOperandExpression);
            return bExp;
        }

        /// <summary>
        /// Builds a not-equal expression for two properties.
        /// </summary>
        /// <param name="paramExpression">The lambda parameter.</param>
        /// <param name="propertyName">The first property name.</param>
        /// <param name="value">The second property name.</param>
        /// <param name="elementType">The second property name.</param>
        /// <param name="expressionFunc">The second property name.</param>
        /// <returns>A not-equal expression.</returns>
        public static Expression NotEqual(this ParameterExpression paramExpression, string propertyName, object value,
                                          Type elementType, Expression<Func<string, object, object>> expressionFunc)
        {
            // constructing a wrapper Func that would return typed value
            MethodInfo[] methods = [.. typeof(QueryableExtensions).GetMethods(BindingFlags.Static | BindingFlags.NonPublic)];
            MethodInfo? wrapperFuncMethod =
                methods.FirstOrDefault(
                    m => m.Name == "GetInvokeExpressionAggregateFunc" && m.IsStatic && m.IsPrivate && m.IsGenericMethod);
            MethodInfo? genericWrapper = wrapperFuncMethod?.MakeGenericMethod([elementType]);
            Expression invokeExp =
                (Expression)genericWrapper?.Invoke(null, [paramExpression, propertyName, expressionFunc])!;
            value = NullableHelperInternal.ChangeType(value, elementType, CultureInfo.InvariantCulture);
            Expression rightOperandExpression = Expression.Constant(value);
            if (rightOperandExpression.Type != elementType)
            {
                rightOperandExpression = Expression.Convert(rightOperandExpression, invokeExp.Type);
            }

            BinaryExpression bExp = Expression.NotEqual(invokeExp, rightOperandExpression);
            return bExp;
        }

        /// <summary>
        /// Builds a not-equal expression for two properties.
        /// </summary>
        /// <param name="paramExpression">The lambda parameter.</param>
        /// <param name="propertyName">The first property name.</param>
        /// <param name="value">The second property name.</param>
        /// <returns>A not-equal expression.</returns>
        public static BinaryExpression NotEqual(this ParameterExpression paramExpression, string propertyName, object value)
        {
            Expression memExp = paramExpression.GetExpression(propertyName);
            object result = NullableHelperInternal.FixDbNUllasNull(value, memExp.Type);
            result = NullableHelperInternal.ChangeType(result, memExp.Type, CultureInfo.InvariantCulture);
            BinaryExpression bExp = Expression.NotEqual(memExp, Expression.Constant(result, memExp.Type));
            return bExp;
        }

        /// <summary>
        /// Builds a not-equal expression for two properties.
        /// </summary>
        /// <param name="paramExpression">The lambda parameter.</param>
        /// <param name="propertyName">The first property name.</param>
        /// <param name="propertyName2">The second property name.</param>
        /// <returns>A not-equal expression.</returns>
        public static BinaryExpression NotEqual(this ParameterExpression paramExpression, string propertyName, string propertyName2)
        {
            Expression memExp = paramExpression.GetExpression(propertyName);
            Expression memExp2 = paramExpression.GetExpression(propertyName2);
            BinaryExpression bExp = Expression.NotEqual(memExp, memExp2);
            return bExp;
        }

        /// <summary>
        /// Builds a greater-than-or-equal expression using two properties.
        /// </summary>
        /// <param name="paramExpression">The lambda parameter.</param>
        /// <param name="propertyName">The first property name.</param>
        /// <param name="value">The value property name.</param>
        /// <returns>A greater-than-or-equal expression.</returns>
        public static BinaryExpression GreaterThanOrEqual(this ParameterExpression paramExpression, string propertyName, object value)
        {
            Expression memExp = paramExpression.GetValueExpression(propertyName, paramExpression?.Type!);
            object result = NullableHelperInternal.FixDbNUllasNull(value, memExp.Type);
            result = NullableHelperInternal.ChangeType(result, memExp.Type, CultureInfo.InvariantCulture);
            BinaryExpression bExp = Expression.GreaterThanOrEqual(memExp, Expression.Constant(result, memExp.Type));
            return bExp;
        }

        /// <summary>
        /// Builds a greater-than-or-equal expression using two properties.
        /// </summary>
        /// <param name="paramExpression">The lambda parameter.</param>
        /// <param name="propertyName">The first property name.</param>
        /// <param name="propertyName2">The second property name.</param>
        /// <returns>A greater-than-or-equal expression.</returns>
        public static BinaryExpression GreaterThanOrEqual(this ParameterExpression paramExpression, string propertyName, string propertyName2)
        {
            Expression memExp = paramExpression.GetExpression(propertyName);
            Expression memExp2 = paramExpression.GetExpression(propertyName2);
            BinaryExpression bExp = Expression.GreaterThanOrEqual(memExp, memExp2);
            return bExp;
        }

        /// <summary>
        /// Builds a greater-than-or-equal expression using a custom expression factory.
        /// </summary>
        /// <param name="paramExpression">The lambda parameter.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="value">The comparison value.</param>
        /// <param name="elementType">The target element type.</param>
        /// <param name="expressionFunc">The expression factory.</param>
        /// <returns>A greater-than-or-equal expression.</returns>
        public static Expression GreaterThanOrEqual(this ParameterExpression paramExpression, string propertyName,
                                                    object value, Type elementType, Expression<Func<string, object, object>> expressionFunc)
        {
            // constructing a wrapper Func that would return Int32 value
            MethodInfo[] methods = [.. typeof(QueryableExtensions).GetMethods(BindingFlags.Static | BindingFlags.NonPublic)];
            MethodInfo? wrapperFuncMethod =
                methods.FirstOrDefault(
                    m => m.Name == "GetInvokeExpressionAggregateFunc" && m.IsStatic && m.IsPrivate && m.IsGenericMethod);
            MethodInfo? genericWrapper = wrapperFuncMethod?.MakeGenericMethod([elementType]);
            Expression invokeExp =
                (Expression)genericWrapper?.Invoke(null, [paramExpression, propertyName, expressionFunc])!;
            value = NullableHelperInternal.ChangeType(value, elementType, CultureInfo.InvariantCulture);
            Expression rightOperandExpression = Expression.Constant(value);
            if (rightOperandExpression.Type != elementType)
            {
                rightOperandExpression = Expression.Convert(rightOperandExpression, invokeExp.Type);
            }

            BinaryExpression bExp = Expression.GreaterThanOrEqual(invokeExp, rightOperandExpression);
            return bExp;
        }

        /// <summary>
        /// Builds a greater-than expression using a single property and a constant value.
        /// </summary>
        /// <param name="paramExpression">The lambda parameter.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="value">The comparison value.</param>
        /// <returns>A greater-than expression.</returns>
        public static BinaryExpression GreaterThan(this ParameterExpression paramExpression, string propertyName, object value)
        {
            Expression memExp = paramExpression.GetValueExpression(propertyName, paramExpression?.Type!);
            object result = NullableHelperInternal.FixDbNUllasNull(value, memExp.Type);
            result = NullableHelperInternal.ChangeType(result, memExp.Type, CultureInfo.InvariantCulture);
            BinaryExpression bExp = Expression.GreaterThan(memExp, Expression.Constant(result, memExp.Type));
            return bExp;
        }

        /// <summary>
        /// Builds a greater-than expression using two properties.
        /// </summary>
        /// <param name="paramExpression">The lambda parameter.</param>
        /// <param name="propertyName">The first property name.</param>
        /// <param name="propertyName2">The second property name.</param>
        /// <returns>A greater-than expression.</returns>
        public static BinaryExpression GreaterThan(this ParameterExpression paramExpression, string propertyName, string propertyName2)
        {
            Expression memExp = paramExpression.GetExpression(propertyName);
            Expression memExp2 = paramExpression.GetExpression(propertyName2);
            BinaryExpression bExp = Expression.GreaterThan(memExp, memExp2);
            return bExp;
        }

        /// <summary>
        /// Builds a greater-than expression using a custom expression factory.
        /// </summary>
        /// <param name="paramExpression">The lambda parameter.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="value">The comparison value.</param>
        /// <param name="elementType">The target element type.</param>
        /// <param name="expressionFunc">The expression factory.</param>
        /// <returns>A greater-than expression.</returns>
        public static Expression GreaterThan(this ParameterExpression paramExpression, string propertyName, object value,
                                             Type elementType, Expression<Func<string, object, object>> expressionFunc)
        {
            // constructing a wrapper Func that would return typed value
            MethodInfo[] methods = [.. typeof(QueryableExtensions).GetMethods(BindingFlags.Static | BindingFlags.NonPublic)];
            MethodInfo? wrapperFuncMethod =
                methods.FirstOrDefault(
                    m => m.Name == "GetInvokeExpressionAggregateFunc" && m.IsStatic && m.IsPrivate && m.IsGenericMethod);
            MethodInfo? genericWrapper = wrapperFuncMethod?.MakeGenericMethod([elementType]);
            Expression invokeExp =
                (Expression)genericWrapper?.Invoke(null, [paramExpression, propertyName, expressionFunc])!;
            value = NullableHelperInternal.ChangeType(value, elementType, CultureInfo.InvariantCulture);
            Expression rightOperandExpression = Expression.Constant(value);
            if (rightOperandExpression.Type != elementType)
            {
                rightOperandExpression = Expression.Convert(rightOperandExpression, invokeExp.Type);
            }

            BinaryExpression bExp = Expression.GreaterThan(invokeExp, rightOperandExpression);
            return bExp;
        }

        /// <summary>
        /// Builds a less-than expression using a single property and a constant value.
        /// </summary>
        /// <param name="paramExpression">The lambda parameter.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="value">The comparison value.</param>
        /// <returns>A less-than expression.</returns>
        public static BinaryExpression LessThan(this ParameterExpression paramExpression, string propertyName, object value)
        {
            Expression memExp = paramExpression.GetValueExpression(propertyName, paramExpression?.Type!);
            object result = NullableHelperInternal.FixDbNUllasNull(value, memExp.Type);
            result = NullableHelperInternal.ChangeType(result, memExp.Type, CultureInfo.InvariantCulture);
            BinaryExpression bExp = Expression.LessThan(memExp, Expression.Constant(result, memExp.Type));
            return bExp;
        }

        /// <summary>
        /// Builds a less-than expression using two properties.
        /// </summary>
        /// <param name="paramExpression">The lambda parameter.</param>
        /// <param name="propertyName">The first property name.</param>
        /// <param name="propertyName2">The second property name.</param>
        /// <returns>A less-than expression.</returns>
        public static BinaryExpression LessThan(this ParameterExpression paramExpression, string propertyName, string propertyName2)
        {
            Expression memExp = paramExpression.GetExpression(propertyName);
            Expression memExp2 = paramExpression.GetExpression(propertyName2);
            BinaryExpression bExp = Expression.LessThan(memExp, memExp2);
            return bExp;
        }

        /// <summary>
        /// Builds a less-than expression using a custom expression factory.
        /// </summary>
        /// <param name="paramExpression">The lambda parameter.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="value">The comparison value.</param>
        /// <param name="elementType">The target element type.</param>
        /// <param name="expressionFunc">The expression factory.</param>
        /// <returns>A less-than expression.</returns>
        public static Expression LessThan(this ParameterExpression paramExpression, string propertyName, object value,
                                          Type elementType, Expression<Func<string, object, object>> expressionFunc)
        {
            // constructing a wrapper Func that would return Int32 value
            MethodInfo[] methods = [.. typeof(QueryableExtensions).GetMethods(BindingFlags.Static | BindingFlags.NonPublic)];
            MethodInfo? wrapperFuncMethod =
                methods.FirstOrDefault(
                    m => m.Name == "GetInvokeExpressionAggregateFunc" && m.IsStatic && m.IsPrivate && m.IsGenericMethod);
            MethodInfo? genericWrapper = wrapperFuncMethod?.MakeGenericMethod([elementType]);
            Expression invokeExp =
                (Expression)genericWrapper?.Invoke(null, [paramExpression, propertyName, expressionFunc])!;
            value = NullableHelperInternal.ChangeType(value, elementType, CultureInfo.InvariantCulture);
            Expression rightOperandExpression = Expression.Constant(value);
            if (rightOperandExpression.Type != elementType)
            {
                rightOperandExpression = Expression.Convert(rightOperandExpression, invokeExp.Type);
            }

            BinaryExpression bExp = Expression.LessThan(invokeExp, rightOperandExpression);
            return bExp;
        }

        /// <summary>
        /// Builds a less-than-or-equal expression using a single property and a constant value.
        /// </summary>
        /// <param name="paramExpression">The lambda parameter.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="value">The comparison value.</param>
        /// <returns>A less-than-or-equal expression.</returns>
        public static BinaryExpression LessThanOrEqual(this ParameterExpression paramExpression, string propertyName, object value)
        {
            Expression memExp = paramExpression.GetValueExpression(propertyName, paramExpression?.Type!);
            object result = NullableHelperInternal.FixDbNUllasNull(value, memExp.Type);
            result = NullableHelperInternal.ChangeType(result, memExp.Type, CultureInfo.InvariantCulture);
            BinaryExpression bExp = Expression.LessThanOrEqual(memExp, Expression.Constant(result, memExp.Type));
            return bExp;
        }

        /// <summary>
        /// Builds a less-than-or-equal expression using two properties.
        /// </summary>
        /// <param name="paramExpression">The lambda parameter.</param>
        /// <param name="propertyName">The first property name.</param>
        /// <param name="propertyName2">The second property name.</param>
        /// <returns>A less-than-or-equal expression.</returns>
        public static BinaryExpression LessThanOrEqual(this ParameterExpression paramExpression, string propertyName, string propertyName2)
        {
            Expression memExp = paramExpression.GetValueExpression(propertyName, paramExpression?.Type!);
            Expression memExp2 = paramExpression!.GetExpression(propertyName2);
            BinaryExpression bExp = Expression.LessThanOrEqual(memExp, memExp2);
            return bExp;
        }

        /// <summary>
        /// Builds a less-than-or-equal expression using a custom expression factory.
        /// </summary>
        /// <param name="paramExpression">The lambda parameter.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="value">The comparison value.</param>
        /// <param name="elementType">The target element type.</param>
        /// <param name="expressionFunc">The expression factory.</param>
        /// <returns>A less-than-or-equal expression.</returns>
        public static Expression LessThanOrEqual(this ParameterExpression paramExpression, string propertyName,
                                                 object value, Type elementType,
                                                 Expression<Func<string, object, object>> expressionFunc)
        {
            // constructing a wrapper Func that would return Int32 value
            MethodInfo[] methods = [.. typeof(QueryableExtensions).GetMethods(BindingFlags.Static | BindingFlags.NonPublic)];
            MethodInfo? wrapperFuncMethod =
                methods.FirstOrDefault(
                    m => m.Name == "GetInvokeExpressionAggregateFunc" && m.IsStatic && m.IsPrivate && m.IsGenericMethod);
            MethodInfo? genericWrapper = wrapperFuncMethod?.MakeGenericMethod([elementType]);
            Expression invokeExp =
                (Expression)genericWrapper?.Invoke(null, [paramExpression, propertyName, expressionFunc])!;
            value = NullableHelperInternal.ChangeType(value, elementType, CultureInfo.InvariantCulture);
            Expression rightOperandExpression = Expression.Constant(value);
            if (rightOperandExpression.Type != elementType)
            {
                rightOperandExpression = Expression.Convert(rightOperandExpression, invokeExp.Type);
            }

            BinaryExpression bExp = Expression.LessThanOrEqual(invokeExp, rightOperandExpression);
            return bExp;
        }

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
        /// <param name="source"></param>
        /// <param name="paramExpression"></param>
        /// <param name="propertyName"></param>
        /// <param name="constValue"></param>
        /// <param name="filterType"></param>
        /// <param name="filterBehaviour"></param>
        /// <param name="isCaseSensitive"></param>
        /// <param name="sourceType"></param>
        /// <param name="ignoreAccent"></param>
        public static Expression Predicate(this IQueryable source, ParameterExpression paramExpression,
                                           string propertyName, object constValue, FilterType filterType,
                                           FilterBehavior filterBehaviour, bool isCaseSensitive, Type sourceType, bool ignoreAccent = false) // Predicate1
        {
            return Predicate(source, constValue, filterType, filterBehaviour, isCaseSensitive, sourceType, null!, null!, paramExpression ?? null!, propertyName ?? string.Empty, ignoreAccent);
        }

        /// <summary>
        /// Predicate is a Binary expression that needs to be built for a single or a series
        /// of values that needs to be passed on to the WHERE expression.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="paramExpression"></param>
        /// <param name="propertyName"></param>
        /// <param name="constValue"></param>
        /// <param name="filterType"></param>
        /// <param name="filterBehaviour"></param>
        /// <param name="isCaseSensitive"></param>
        /// <param name="sourceType"></param>
        /// <param name="ignoreAccent"></param>
        /// <param name="isDateTimeColumn"></param>
        internal static Expression Predicate(this IQueryable source, ParameterExpression paramExpression,
                                          string propertyName, object constValue, FilterType filterType,
                                          FilterBehavior filterBehaviour, bool isCaseSensitive, Type sourceType, bool ignoreAccent = false, bool isDateTimeColumn = false) // internal predicate to handle datetime filter
        {
            return Predicate(source, constValue, filterType, filterBehaviour, isCaseSensitive, sourceType, null!, null!, paramExpression ?? null!, propertyName ?? string.Empty, ignoreAccent, isDateTimeColumn);
        }

        /// <summary>
        /// Predicate is a Binary expression that needs to be built for a single or a series
        /// of values that needs to be passed on to the WHERE expression.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="paramExpression"></param>
        /// <param name="propertyName"></param>
        /// <param name="constValue"></param>
        /// <param name="filterType"></param>
        /// <param name="filterBehaviour"></param>
        /// <param name="isCaseSensitive"></param>
        /// <param name="sourceType"></param>
        /// <param name="expressionFunc"></param>
        /// <param name="memberType"></param>
        public static Expression Predicate(this IQueryable source, ParameterExpression paramExpression,
                                           string propertyName, object constValue, FilterType filterType,
                                           FilterBehavior filterBehaviour, bool isCaseSensitive, Type sourceType,
            /*Expression<Func<string, object, object>>*/ Delegate expressionFunc, Type? memberType = null)

        // Predicate2
        {
            ArgumentNullException.ThrowIfNull(source);
            IEnumerator enumerator = source.GetEnumerator();
            if (!enumerator.MoveNext())
            {
                return null!;
            }

            if (filterBehaviour == FilterBehavior.StringTyped)
            {
                memberType = typeof(string);
            }
            else
            {
                if (memberType == null)
                {
                    while (enumerator.MoveNext())
                    {
                        object? returnValue = expressionFunc?.DynamicInvoke([propertyName, enumerator.Current]);
                        if (returnValue != null)
                        {
                            memberType = returnValue.GetType();
                            break;
                        }
                    }
                }

                if (memberType == null && constValue != null)
                {
                    memberType = constValue.GetType();
                }
                else if (constValue == null) // if value is null, we need to set the filterbehaviour as stringtyped.
                {
                    filterBehaviour = FilterBehavior.StringTyped;
                    memberType = typeof(string);
                }
            }

            return Predicate(source, paramExpression, propertyName, constValue!, memberType!, filterType, filterBehaviour,
                             isCaseSensitive, sourceType, expressionFunc!);
        }

        /// <summary>
        /// Builds a predicate expression using a cached delegate for complex expressions.
        /// </summary>
        /// <param name="source">The query source.</param>
        /// <param name="paramExpression">The lambda parameter.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="constValue">The comparison value.</param>
        /// <param name="memberType">The member type.</param>
        /// <param name="filterType">The filter operation.</param>
        /// <param name="filterBehaviour">The filter behavior.</param>
        /// <param name="isCaseSensitive">Whether comparison is case sensitive.</param>
        /// <param name="sourceType">The source element type.</param>
        /// <param name="expressionFunc">The cached expression delegate.</param>
        /// <returns>A predicate expression.</returns>
        public static Expression Predicate(this IQueryable source, ParameterExpression paramExpression,
                           string propertyName, object constValue, Type memberType,
                           FilterType filterType, FilterBehavior filterBehaviour, bool isCaseSensitive,
                           Type sourceType, /*Expression<Func<string, object, object>>*/
                           Delegate expressionFunc) ////Predicate2
        {
            if (filterBehaviour == FilterBehavior.StronglyTyped)
            {
                MethodInfo[] methods = [.. typeof(QueryableExtensions).GetMethods(BindingFlags.Static | BindingFlags.NonPublic)];
                MethodInfo? wrapperFuncMethod = methods.FirstOrDefault(
                        m => m.Name == "GetDelegateInvokeExpressionAggregateFunc"
                        && m.IsStatic && m.IsPrivate && m.IsGenericMethod);

                MethodInfo? genericWrapper = wrapperFuncMethod?.MakeGenericMethod([memberType]);
                Expression memExp = (Expression)genericWrapper?.Invoke(null,
                        [
                            paramExpression, propertyName, expressionFunc
                        ])!;
                return Predicate(source, constValue, filterType, filterBehaviour, isCaseSensitive,
                    sourceType, memberType ?? null!, memExp, paramExpression ?? null!, propertyName ?? string.Empty);
            }
            else
            {
                Func<Delegate, string, object, string> fun = (lambda, prop, rec) =>
                {
                    object? val = lambda.DynamicInvoke([prop, rec]);
                    return val != null ? val.ToString()! : null!;
                };
                InvocationExpression invokeExp = Expression.Invoke(
                    Expression.Constant(fun),
                    [
                                                      Expression.Constant(expressionFunc),
                                                      Expression.Constant(propertyName), paramExpression
                                                  ]);

                return Predicate(source, constValue, filterType, filterBehaviour, isCaseSensitive,
                    sourceType, memberType ?? null!, invokeExp, paramExpression ?? null!, propertyName ?? string.Empty);
            }
        }

        /// <summary>
        /// Builds the expression tree for comparison filters that operate on typed values.
        /// </summary>
        /// <param name="filterType">The filter operation.</param>
        /// <param name="memberType">The member type.</param>
        /// <param name="value">The comparison value.</param>
        /// <param name="isCaseSensitive">Whether comparison is case sensitive.</param>
        /// <param name="memExp">The member expression.</param>
        /// <param name="bExp">The current binary expression.</param>
        /// <param name="isDynamicDataObject">Whether the source is dynamic.</param>
        /// <returns>A tuple containing the transformed member expression, binary expression, and value.</returns>
        private static ValueTuple<Expression, Expression, object?> GetPxExpression(
            FilterType filterType, Type memberType, object value,
            bool isCaseSensitive, Expression memExp, Expression bExp, bool isDynamicDataObject = false
            )
        {
            Type underlyingType = memberType;
            if (NullableHelperInternal.IsNullableType(memberType))
            {
                underlyingType = NullableHelperInternal.GetUnderlyingType(memberType);
            }

            if (value != null)
            {
                try
                {
                    if (underlyingType == typeof(DateTimeOffset))
                    {
                        value = value switch
                        {
                            DateTime dateTimeValue => new DateTimeOffset(dateTimeValue),
                            TimeOnly timeOnlyValue => new DateTimeOffset(DateTime.Today.Add(timeOnlyValue.ToTimeSpan())),
                            DateOnly dateOnlyValue => new DateTimeOffset(dateOnlyValue.ToDateTime(TimeOnly.MinValue)),
                            _ => value
                        };
                    }
                    else
                    {
                        value = ValueConvert.ChangeType(underlyingType.Name.Equals("DateTimeOffset", StringComparison.Ordinal) ? ((DateTimeOffset)value).ToString("o", CultureInfo.InvariantCulture) : value, underlyingType, CultureInfo.CurrentCulture);
                    }
                }
                catch (InvalidCastException e)
                {
                    Console.WriteLine(e);
                }
            }
            Type nullablememberType = NullableHelperInternal.GetNullableType(memberType);

            switch (filterType)
            {
                case FilterType.Equals:
                    if (isCaseSensitive || memberType != typeof(string))
                    {
                        if (value != null)
                        {
                            ConstantExpression exp = Expression.Constant(value, memberType);

                            bExp = (nullablememberType == memberType && memberType != typeof(object)) || memberType.GetTypeInfo().IsEnum
                                ? Expression.Equal(memExp, Expression.Constant(value, memberType))
                                : Expression.Call(exp, exp.Type.GetMethod("Equals", new[] { memExp.Type })!, memExp);
                        }
                        else
                        {
                            memExp = Expression.Convert(memExp, nullablememberType);
                            bExp = Expression.Equal(memExp, Expression.Constant(value, nullablememberType));
                        }
                    }
                    else
                    {
                        if (isDynamicDataObject && memExp.Type != memberType)
                        {
                            memExp = Expression.Convert(memExp, nullablememberType);
                        }

                        memExp = Expression.Coalesce(memExp, Expression.Constant(value == null ? "blanks" : string.Empty));
                        MethodCallExpression toLowerMethodCall = memExp.ToLowerMethodCallExpression();
                        bExp = Expression.Equal(toLowerMethodCall,
                                                Expression.Constant(
                                                    value == null ? "blanks" : value.ToString()?.ToLower(CultureInfo.CurrentCulture),
                                                    typeof(string)));
                    }

                    break;
                case FilterType.NotEquals:
                    if (isCaseSensitive || memberType != typeof(string))
                    {
                        if (value != null && !isDynamicDataObject)
                        {
                            bExp = Expression.NotEqual(memExp, Expression.Constant(value, memberType));
                        }
                        else
                        {
                            memExp = Expression.Convert(memExp, nullablememberType);
                            bExp = Expression.NotEqual(memExp, Expression.Constant(value, nullablememberType));
                        }
                    }
                    else
                    {
                        if (isDynamicDataObject && memExp.Type != memberType)
                        {
                            memExp = Expression.Convert(memExp, nullablememberType);
                        }

                        memExp = Expression.Coalesce(memExp, Expression.Constant(value == null ? "blanks" : string.Empty));
                        MethodCallExpression toLowerMethodCall = memExp.ToLowerMethodCallExpression();
                        bExp = Expression.NotEqual(toLowerMethodCall,
                                                   Expression.Constant(
                                                       value == null ? "blanks" : value.ToString()!.ToLowerInvariant(),
                                                       memberType));
                    }

                    break;
                case FilterType.LessThan:
                    if (value != null && !isDynamicDataObject)
                    {
                        bExp = Expression.LessThan(memExp, Expression.Constant(value, memberType));
                    }
                    else
                    {
                        memExp = Expression.Convert(memExp, nullablememberType);
                        bExp = Expression.LessThan(memExp, Expression.Constant(value, nullablememberType));
                    }
                    break;
                case FilterType.LessThanOrEqual:
                    if (value != null && !isDynamicDataObject)
                    {
                        bExp = Expression.LessThanOrEqual(memExp, Expression.Constant(value, memberType));
                    }
                    else
                    {
                        memExp = Expression.Convert(memExp, nullablememberType);
                        bExp = Expression.LessThanOrEqual(memExp, Expression.Constant(value, nullablememberType));
                    }
                    break;
                case FilterType.GreaterThan:
                    if (value != null && !isDynamicDataObject)
                    {
                        bExp = Expression.GreaterThan(memExp, Expression.Constant(value, memberType));
                    }
                    else
                    {
                        memExp = Expression.Convert(memExp, nullablememberType);
                        bExp = Expression.GreaterThan(memExp, Expression.Constant(value, nullablememberType));
                    }
                    break;
                case FilterType.GreaterThanOrEqual:
                    if (value != null && !isDynamicDataObject)
                    {
                        bExp = Expression.GreaterThanOrEqual(memExp, Expression.Constant(value, memberType));
                    }
                    else
                    {
                        memExp = Expression.Convert(memExp, nullablememberType);
                        bExp = Expression.GreaterThanOrEqual(memExp, Expression.Constant(value, nullablememberType));
                    }
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
            return (memExp, bExp, value);
        }

        /// <summary>
        /// Builds the expression tree for string-based filter operations.
        /// </summary>
        /// <param name="filterType">The filter operation.</param>
        /// <param name="memExp">The member expression.</param>
        /// <param name="bExp">The current binary expression.</param>
        /// <param name="value">The comparison value.</param>
        /// <param name="isCaseSensitive">Whether comparison is case sensitive.</param>
        /// <param name="memberType">The member type.</param>
        /// <param name="constValue">The original constant value.</param>
        /// <param name="isDynamicDataObject">Whether the source is dynamic.</param>
        /// <returns>A tuple containing the transformed member expression, binary expression, and value.</returns>
        private static ValueTuple<Expression, Expression, object?> GetPxxExpression(FilterType filterType,
            Expression memExp, Expression bExp, object value, bool isCaseSensitive, Type memberType, object constValue, bool isDynamicDataObject = false)
        {
            if (!isCaseSensitive && (filterType == FilterType.Equals || filterType == FilterType.NotEquals))
            {
                value = NullableHelperInternal.FixDbNUllasNull(constValue, memberType);
            }
            if (isDynamicDataObject && memExp.Type != memberType)
            {
                Type nullablememberType = NullableHelperInternal.GetNullableType(memberType);
                memExp = Expression.Convert(memExp, nullablememberType);
            }
            MethodInfo? toString = memExp.Type.GetMethods().FirstOrDefault(d => d.Name == "ToString");
            string? stringValue = string.Empty;
            if (memberType == typeof(string))
            {
                memExp = value == null || string.Equals("null", value.ToString(), StringComparison.Ordinal) ? Expression.Coalesce(memExp, Expression.Constant("Blanks")) : Expression.Coalesce(memExp, Expression.Constant(string.Empty));
                stringValue = value == null || string.Equals("null", value.ToString(), StringComparison.Ordinal) ? "Blanks" : value.ToString();
            }
            else
            {
                stringValue = value == null! ? "" : value.ToString() ?? string.Empty;
            }

            if (memberType.Name != "String")
            {
                memExp = Expression.Call(memExp, toString!);
            }
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
                                                    Expression.Constant(stringValue?.ToLower(CultureInfo.CurrentCulture), typeof(string)));
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
                                            Expression.Constant(stringValue?.ToLower(CultureInfo.CurrentCulture), typeof (string))
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
                                            Expression.Constant(stringValue?.ToLower(CultureInfo.CurrentCulture), typeof (string))
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
                                            Expression.Constant(stringValue?.ToLower(CultureInfo.CurrentCulture), typeof (string))
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
                                            Expression.Constant(stringValue?.ToLower(CultureInfo.CurrentCulture), typeof (string))
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
                        bExp = Expression.Call(null, likeMethod!, toLowerMethod, Expression.Constant(stringValue?.ToLower(CultureInfo.CurrentCulture), typeof(string)));
                    }
                    break;
                case FilterType.WildCard:
                    MethodInfo? regexMethod = typeof(Regex).GetMethod("IsMatch", [memExp.Type, typeof(string)]);
                    string regexPattern = WildcardToRegex(stringValue!);
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
                case FilterType.IsNull:
                    break;
                case FilterType.IsNotNull:
                    break;
                case FilterType.LessThan:
                    break;
                case FilterType.LessThanOrEqual:
                    break;
                case FilterType.GreaterThanOrEqual:
                    break;
                case FilterType.GreaterThan:
                    break;
                case FilterType.Undefined:
                    break;
                case FilterType.Between:
                    break;
                default:
                    break;
            }

            return (memExp, bExp, value);
        }

        /// <summary>
        /// Converts a wildcard pattern to a regular expression pattern.
        /// </summary>
        /// <param name="pattern">The wildcard pattern.</param>
        /// <returns>The regex pattern.</returns>
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

        /// <summary>
        /// Builds a predicate expression for the specified filter.
        /// </summary>
        /// <param name="source">The query source.</param>
        /// <param name="constValue">The comparison value.</param>
        /// <param name="filterType">The filter operation.</param>
        /// <param name="filterBehaviour">The filter behavior.</param>
        /// <param name="isCaseSensitive">Whether comparison is case sensitive.</param>
        /// <param name="sourceType">The source element type.</param>
        /// <param name="memberType">The member type.</param>
        /// <param name="memExp">The member expression.</param>
        /// <param name="paramExpression">The lambda parameter.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="ignoreAccent">Whether to ignore accents for string values.</param>
        /// <param name="isDateTimeColumn">Whether the property is a date-time column.</param>
        /// <returns>A predicate expression.</returns>
        private static Expression Predicate(this IQueryable source, object constValue, FilterType filterType,
                                           FilterBehavior filterBehaviour, bool isCaseSensitive, Type sourceType, Type memberType, Expression memExp, ParameterExpression paramExpression, string propertyName, bool ignoreAccent = false, bool isDateTimeColumn = false)
        {
            bool hasExpressionFunc = false; _ = filterBehaviour;
            string[]? propertyNameList = null;
            bool isDynamicDataObject = false;
            Type? columnType = null;
            int propCount = 1;
            if (memExp == null)
            {
                bool isComplex = propertyName.Contains('.', StringComparison.InvariantCulture);
                //This condition is specifically included for the TreeGrid component.
                if (isComplex && propertyName.Contains("DataItem", StringComparison.Ordinal))
                {
                    object item = source.ElementAtOrDefault(0);
                    object complexItem = item?.GetType().GetProperty(propertyName.Split('.')[0])?.GetValue(item)!;
                    if ((sourceType?.FullName?.Contains("ExpandoObject", StringComparison.Ordinal) == true) || complexItem?.GetType().BaseType == typeof(DynamicObject))
                    {
                        isDynamicDataObject = true;
                    }
                }
                // update the memberType as columnType if complex column as dynamic model class
                if (isDynamicDataObject)
                {
                    columnType = EnumerableOperation.GetColumnType(source, propertyName, sourceType!);
                }
                memExp = paramExpression.GetValueExpression(propertyName, sourceType!);
                memberType = columnType ?? memExp.Type;
                propertyNameList = propertyName.Split('.');
                propCount = propertyNameList.Length;
            }
            else
            {
                hasExpressionFunc = true;
            }
            if (ignoreAccent && memberType == typeof(string))
            {
                if (constValue != null)
                {
                    constValue = constValue switch
                    {
                        string[] stringArray => Array.ConvertAll(stringArray, RemoveDiacritics),
                        IEnumerable<string> stringEnumerable => stringEnumerable.Select(RemoveDiacritics).ToArray(),
                        IEnumerable enumerable when constValue is not string => enumerable.Cast<object>()
                                                                           .Select(item => RemoveDiacritics(item?.ToString() ?? string.Empty))
                                                                           .ToArray(),
                        _ => RemoveDiacritics(constValue.ToString() ?? string.Empty),
                    };
                }
                string? providerFullName = source?.Provider?.GetType().FullName;
                bool isEfCore = providerFullName != null && providerFullName.Contains("EntityFrameworkCore", StringComparison.OrdinalIgnoreCase);
                if (!isEfCore)
                {
                    MethodInfo RemoveDiacriticsMethod = typeof(QueryableExtensions).GetMethod("RemoveDiacritics", BindingFlags.Static | BindingFlags.NonPublic)!;
                    if (RemoveDiacriticsMethod != null)
                    {
                        memExp = Expression.Call(null, RemoveDiacriticsMethod, memExp);
                    }
                }
            }

            object? value = constValue;
            Expression? bExp = null;
            if (memberType == typeof(DateTime?) && value != null && DateTime.TryParse(value.ToString(), out DateTime newdatetime))
            {
                double dateAndTime = newdatetime.TimeOfDay.TotalSeconds;
                MemberExpression hasVal = Expression.Property(memExp, nameof(Nullable<DateTime>.HasValue));
                MemberExpression dateVal = Expression.Property(memExp, nameof(Nullable<DateTime>.Value));
                MemberExpression propertyDate = (dateAndTime == 0) ? Expression.Property(dateVal, nameof(DateTime.Date)) : dateVal;
                if (isDateTimeColumn)
                {
                    propertyDate = dateVal;
                }
                memExp = Expression.Condition(Expression.Not(hasVal), Expression.Constant(null, typeof(DateTime?)), Expression.Convert(propertyDate, typeof(DateTime?)));
            }
            if (!isDynamicDataObject && memberType == typeof(DateTime) && value != null && DateTime.TryParse(value.ToString(), out DateTime dateTime) && dateTime.TimeOfDay == TimeSpan.Zero && !isDateTimeColumn)
            {
                memExp = Expression.Property(memExp, nameof(DateTime.Date));
            }
            if (memberType == typeof(DateTimeOffset?) && value != null && DateTimeOffset.TryParse(value.ToString(), out DateTimeOffset newdatetimeoffset))
            {
                double dateAndTime = newdatetimeoffset.TimeOfDay.TotalSeconds;
                MemberExpression hasVal = Expression.Property(memExp, nameof(Nullable<DateTimeOffset>.HasValue));
                MemberExpression dateVal = Expression.Property(memExp, nameof(Nullable<DateTimeOffset>.Value));
                MemberExpression propertyDate = (dateAndTime == 0) ? Expression.Property(dateVal, nameof(DateTimeOffset.Date)) : dateVal;
                if (isDateTimeColumn)
                {
                    propertyDate = dateVal;
                }
                memExp = Expression.Condition(Expression.Not(hasVal), Expression.Constant(null, typeof(DateTimeOffset?)), Expression.Convert(propertyDate, typeof(DateTimeOffset?)));
            }

            if (filterType is FilterType.Equals or FilterType.NotEquals or
                 FilterType.LessThan or FilterType.LessThanOrEqual or
                 FilterType.GreaterThan or FilterType.GreaterThanOrEqual or FilterType.IsNull or FilterType.IsNotNull)
            {
                ValueTuple<Expression, Expression, object?> v = GetPxExpression(filterType, memberType, value!, isCaseSensitive, memExp, bExp!, isDynamicDataObject);
                memExp = v.Item1;
                bExp = v.Item2;
                value = v.Item3;
            }
            else
            {

                ValueTuple<Expression, Expression, object?> v = GetPxxExpression(filterType, memExp, bExp!, value!,
                    isCaseSensitive, memberType, constValue!, isDynamicDataObject);
                memExp = v.Item1;
                bExp = v.Item2;
                value = v.Item3;
            }

            // Coding for complex property
            if (!hasExpressionFunc && propCount > 1)
            {
                Expression? basenullexp = null;
                Expression? basenotnullexp = null;
                Expression propExp = paramExpression;
                Expression? complexnullexp = null;
                ConstantExpression valueExp = Expression.Constant(value);
                ConstantExpression nullExp = Expression.Constant(null);
                UnaryExpression Exp = Expression.Convert(valueExp, typeof(object));
                BinaryExpression nullvalExp = Expression.Equal(Exp, nullExp);
                BinaryExpression notnullvalExp = Expression.NotEqual(Exp, nullExp);

                // Get the query provider full name for the entity framwork
                string? queryProviderFullName = source?.Provider?.GetType().FullName;

                Type currentType = sourceType!;
                for (int prop = 0; prop < propCount - 1; prop++)
                {
                    if (!string.Equals(propExp.Type.Name, "ExpandoObject", StringComparison.Ordinal) && !propExp.Type.IsSubclassOf(typeof(DynamicObject)))
                    {
                        if (propExp?.Type != sourceType)
                        {
                            propExp = Expression.Convert(propExp!, currentType);
                        }
                        propExp = Expression.PropertyOrField(propExp!, propertyNameList![prop]);
                        currentType = propExp!.Type;
                        BinaryExpression tempnullexp = Expression.Equal(propExp, nullExp);
                        basenullexp = basenullexp == null ? tempnullexp : Expression.OrElse(basenullexp, tempnullexp);
                        BinaryExpression tempnotnullexp = Expression.NotEqual(propExp, nullExp);
                        basenotnullexp = basenotnullexp == null ? tempnotnullexp : Expression.AndAlso(basenotnullexp, tempnotnullexp);
                    }
                }

                if (basenullexp != null && queryProviderFullName != null && !queryProviderFullName.Contains("EntityFrameworkCore", StringComparison.OrdinalIgnoreCase))
                {
                    if (filterType == FilterType.Equals)
                    {
                        basenullexp = basenullexp.AndAlsoPredicate(nullvalExp);
                    }
                    else if (filterType == FilterType.NotEquals)
                    {
                        complexnullexp = basenullexp;
                        basenullexp = basenullexp.AndAlsoPredicate(notnullvalExp);
                    }
                    else if (filterType is not FilterType.StartsWith and not FilterType.EndsWith)
                    {
                        complexnullexp = basenullexp;
                        bExp = basenullexp.OrElsePredicate(bExp);
                    }

                    bExp = basenotnullexp?.AndAlsoPredicate(bExp);
                }
                if ((value == null || string.IsNullOrEmpty(value.ToString())) && complexnullexp == null)
                {
                    bExp = bExp?.OrElsePredicate(basenullexp!);
                }
                else if (complexnullexp != null && value != null && !string.IsNullOrEmpty(value.ToString()))
                {
                    bExp = bExp?.OrElsePredicate(complexnullexp);
                }
            }

            return bExp!;
        }

        /// <summary>
        /// Builds a predicate expression for formatted comparisons.
        /// </summary>
        /// <param name="source">The query source.</param>
        /// <param name="paramExpression">The lambda parameter.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="constValue">The comparison value.</param>
        /// <param name="filterType">The filter operation.</param>
        /// <param name="filteBehaviour">The filter behavior.</param>
        /// <param name="isCaseSensitive">Whether comparison is case sensitive.</param>
        /// <param name="sourceType">The source element type.</param>
        /// <param name="format">The format string.</param>
        /// <returns>A predicate expression.</returns>
        public static Expression Predicate(this IQueryable source, ParameterExpression paramExpression,
                                           string propertyName, object constValue, FilterType filterType,
                                           FilterBehavior filteBehaviour, bool isCaseSensitive, Type sourceType,
                                           string format) // Predicate3
        {
            Expression memExp = paramExpression.GetValueExpression(propertyName, sourceType);
            Type? memberType = memExp?.Type; _ = filteBehaviour;
            Type? underlyingType = memberType;
            bool isSource = source != null;
            if (NullableHelperInternal.IsNullableType(memberType!))
            {
                underlyingType = NullableHelperInternal.GetUnderlyingType(memberType!);
            }

            object? value = NullableHelperInternal.FixDbNUllasNull(constValue, memberType!);
            if (value != null)
            {
                if (memberType?.Name == "Boolean")
                {
                    if ("true".Contains(value.ToString()?.ToLowerInvariant()!, StringComparison.Ordinal))
                    {
                        value = "1";
                    }
                    else if ("false".Contains(value.ToString()?.ToLowerInvariant()!, StringComparison.Ordinal))
                    {
                        value = "0";
                    }
                }
            }

            if (filterType is FilterType.Equals or FilterType.NotEquals or
                FilterType.LessThan or FilterType.LessThanOrEqual or
                FilterType.GreaterThan or FilterType.GreaterThanOrEqual)
            {
                ValueTuple<Expression?, object?> v = GetPExpression(filterType, underlyingType!,
                    format, memExp!, value!, memberType!, isCaseSensitive);
                Expression bExp = v.Item1!; value = v.Item2;
                return bExp;
            }
            else
            {
                MethodInfo? toString = memExp?.Type?.GetMethods()?.FirstOrDefault(d => d.Name == "ToString");
                memExp = Expression.Call(memExp, toString!);
                Expression coalesceExp = Expression.Coalesce(memExp!, Expression.Constant(string.Empty));
                MethodInfo? stringMethod = typeof(string).GetMethods().FirstOrDefault(m => m.Name == filterType.ToString());

                if (isCaseSensitive)
                {
                    MethodCallExpression methodCallExp = Expression.Call(coalesceExp, stringMethod!,
                        [Expression.Constant(value, typeof(string))]);
                    return methodCallExp;
                }
                else
                {
                    MethodCallExpression toLowerMethodCall = ToLowerMethodCallExpression(coalesceExp);
                    MethodCallExpression methodCallExp = Expression.Call(
                        toLowerMethodCall,
                        stringMethod!,
                        [Expression.Constant(value == null ? null : value.ToString()!.ToLowerInvariant(), typeof(string))]);
                    return methodCallExp;
                }

            }
        }

        /// <summary>
        /// Builds the predicate expression for formatted comparisons.
        /// </summary>
        /// <param name="filterType">The filter operation.</param>
        /// <param name="underlyingType">The underlying member type.</param>
        /// <param name="format">The format string.</param>
        /// <param name="memExp">The member expression.</param>
        /// <param name="value">The comparison value.</param>
        /// <param name="memberType">The member type.</param>
        /// <param name="isCaseSensitive">Whether comparison is case sensitive.</param>
        /// <returns>A tuple containing the binary expression and converted value.</returns>
        private static ValueTuple<Expression?, object?> GetPExpression(FilterType filterType,
            Type underlyingType, string format, Expression memExp, object value, Type memberType, bool isCaseSensitive)
        {
            Expression? bExp = null;
            switch (filterType)
            {
                case FilterType.Equals:
                    if (underlyingType != typeof(string))
                    {
                        if (!string.IsNullOrEmpty(format))
                        {
                            MethodCallExpression formatMethodCall = GetFormatMethodCallExpression(memExp, format);
                            bExp = Expression.Equal(formatMethodCall, Expression.Constant(value, typeof(string)));
                        }
                        else
                        {
                            ConstantExpression exp = Expression.Constant(value, memberType);
                            // Expression.Equal can't compare DBNull. So Expression.Call used to compare DBNull value.
                            bExp = value != null && !memberType.GetTypeInfo().IsEnum
                                ? Expression.Call(exp, exp.Type.GetMethod("Equals", new[] { memExp.Type })!, memExp)
                                : Expression.Equal(memExp, exp);
                        }
                    }
                    else
                    {
                        if (isCaseSensitive)
                        {
                            ConstantExpression exp = Expression.Constant(value, memberType);
                            // Expression.Equal can't compare DBNull. So Expression.Call used to compare DBNull value.
                            bExp = value != null
                                ? Expression.Call(exp, exp.Type.GetMethod("Equals", [memExp.Type])!, memExp)
                                : Expression.Equal(memExp, Expression.Constant(value, memberType));
                        }
                        else
                        {
                            MethodCallExpression toLowerMethodCall =
                                ToLowerMethodCallExpression(Expression.Coalesce(memExp, Expression.Constant(string.Empty)));
                            bExp = Expression.Equal(toLowerMethodCall,
                                                    Expression.Constant(value?.ToString()?.ToLowerInvariant(), memExp.Type));
                        }
                    }

                    break;
                case FilterType.NotEquals:
                    if (underlyingType != typeof(string))
                    {
                        if (!string.IsNullOrEmpty(format))
                        {
                            MethodCallExpression formatMethodCall = GetFormatMethodCallExpression(memExp, format);
                            bExp = Expression.NotEqual(formatMethodCall, Expression.Constant(value, typeof(string)));
                        }
                        else
                        {
                            bExp = Expression.NotEqual(memExp, Expression.Constant(value, memberType));
                        }
                    }
                    else
                    {
                        if (isCaseSensitive)
                        {
                            bExp = Expression.NotEqual(memExp, Expression.Constant(value, memberType));
                        }
                        else
                        {
                            MethodCallExpression toLowerMethodCall = ToLowerMethodCallExpression(Expression.Coalesce(memExp, Expression.Constant(string.Empty)));
                            bExp = Expression.NotEqual(toLowerMethodCall, Expression.Constant(value?.ToString()?.ToLowerInvariant(), memberType));
                        }
                    }

                    break;
                case FilterType.LessThan:
                    if (!string.IsNullOrEmpty(format))
                    {
                        value = ValueConvert.ChangeType(value, underlyingType, CultureInfo.CurrentCulture, format, true);
                        bExp = Expression.LessThan(memExp, Expression.Constant(value, memberType));
                    }
                    else
                    {
                        bExp = Expression.LessThan(memExp, Expression.Constant(value, memberType));
                    }

                    break;
                case FilterType.LessThanOrEqual:
                    if (!string.IsNullOrEmpty(format))
                    {
                        MethodCallExpression formatMethodCall = GetFormatMethodCallExpression(memExp, format);
                        Expression eqbExp = Expression.Equal(formatMethodCall,
                                                             Expression.Constant(value, typeof(string)));
                        value = ValueConvert.ChangeType(value, underlyingType, CultureInfo.CurrentCulture, format,
                                                        true);
                        bExp = Expression.Or(Expression.LessThan(memExp, Expression.Constant(value, memberType)),
                                             eqbExp);
                    }
                    else
                    {
                        bExp = Expression.LessThanOrEqual(memExp, Expression.Constant(value, memberType));
                    }

                    break;
                case FilterType.GreaterThan:
                    if (!string.IsNullOrEmpty(format))
                    {
                        MethodCallExpression formatMethodCall = GetFormatMethodCallExpression(memExp, format);
                        Expression notbExp = Expression.NotEqual(formatMethodCall,
                                                                 Expression.Constant(value, typeof(string)));
                        value = ValueConvert.ChangeType(value, underlyingType, CultureInfo.CurrentCulture, format,
                                                        true);
                        bExp = Expression.And(
                            Expression.GreaterThan(memExp, Expression.Constant(value, memberType)), notbExp);
                    }
                    else
                    {
                        bExp = Expression.GreaterThan(memExp, Expression.Constant(value, memberType));
                    }

                    break;
                case FilterType.GreaterThanOrEqual:
                    value = ValueConvert.ChangeType(value, underlyingType, CultureInfo.CurrentCulture, format, true);
                    bExp = Expression.GreaterThanOrEqual(memExp, Expression.Constant(value, memberType));
                    break;
                case FilterType.WildCard:
                    break;
                case FilterType.IsNull:
                    break;
                case FilterType.IsNotNull:
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

            return (bExp, value);
        }
        /// <summary>
        /// Removes diacritic characters from the specified text.
        /// </summary>
        /// <param name="text">The input text.</param>
        /// <returns>The normalized text without diacritics.</returns>
        private static string RemoveDiacritics(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            string normalized = text.Normalize(NormalizationForm.FormD);
            int length = normalized.Length;
            StringBuilder sb = new(capacity: length);

            for (int i = 0; i < length; i++)
            {
                char c = normalized[i];
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    _ = sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
        /// <summary>
        /// Builds a <c>ToLower()</c> call for the specified expression.
        /// </summary>
        /// <param name="memExp">The expression to convert.</param>
        /// <returns>A method call expression.</returns>
        private static MethodCallExpression ToLowerMethodCallExpression(this Expression memExp)
        {
            MethodInfo? tolowerMethod = typeof(string).GetMethods().FirstOrDefault(m => m.Name == "ToLower");
            MethodCallExpression toLowerMethodCall = Expression.Call(memExp, tolowerMethod!, []);
            return toLowerMethodCall;
        }

        /// <summary>
        /// Builds a <c>ToString()</c> call for the specified expression.
        /// </summary>
        /// <param name="memExp">The expression to convert.</param>
        /// <returns>A method call expression.</returns>
        private static MethodCallExpression ToStringMethodCallExpression(this Expression memExp)
        {
            MethodInfo? toString = memExp.Type.GetMethods().FirstOrDefault(d => d.Name == "ToString");
            BinaryExpression coalesceExp = Expression.Coalesce(memExp, Expression.Constant(string.Empty));
            return Expression.Call(coalesceExp, toString!);
        }

        /// <summary>
        /// Builds a formatted string call for the specified expression.
        /// </summary>
        /// <param name="memExp">The member expression.</param>
        /// <param name="format">The format string.</param>
        /// <returns>A method call expression.</returns>
        private static MethodCallExpression GetFormatMethodCallExpression(Expression memExp, string format)
        {
            if (memExp.Type.GetTypeInfo().IsGenericType && memExp.Type.GetTypeInfo().GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                memExp = Expression.Call(memExp, "GetValueOrDefault", _emptyTypes);
            }

            MethodInfo? formatMethod = typeof(DateTime).GetMethod(
                "ToString",
                [typeof(string), typeof(IFormatProvider)]);
            if (memExp.Type == typeof(decimal))
            {
                formatMethod = typeof(decimal).GetMethod(
                    "ToString",
                    [typeof(string), typeof(IFormatProvider)]);
            }
            else if (memExp.Type == typeof(double))
            {
                formatMethod = typeof(double).GetMethod(
                    "ToString",
                    [typeof(string), typeof(IFormatProvider)]);
            }
            else if (memExp.Type == typeof(float))
            {
                formatMethod = typeof(float).GetMethod(
                    "ToString",
                    [typeof(string), typeof(IFormatProvider)]);
            }
            else if (memExp.Type == typeof(short))
            {
                formatMethod = typeof(short).GetMethod(
                    "ToString",
                    [typeof(string), typeof(IFormatProvider)]);
            }
            else if (memExp.Type == typeof(int))
            {
                formatMethod = typeof(int).GetMethod(
                    "ToString",
                    [typeof(string), typeof(IFormatProvider)]);
            }
            else if (memExp.Type == typeof(long))
            {
                formatMethod = typeof(long).GetMethod(
                    "ToString",
                    [typeof(string), typeof(IFormatProvider)]);
            }

            MethodCallExpression formatMethodCall = Expression.Call(
                memExp,
                formatMethod!, Expression.Constant(format), Expression.Constant(CultureInfo.CurrentCulture));
            return formatMethodCall;
        }

        /// <summary>
        /// Generates a Select query for a single property value.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="sourceType">Type.</param>
        public static IQueryable Select(this IQueryable source, string propertyName, Type sourceType)
        {
            ArgumentNullException.ThrowIfNull(source);
            ParameterExpression paramExpression = Expression.Parameter(source.ElementType, sourceType?.Name);
            Expression memExp = paramExpression.GetValueExpression(propertyName, sourceType!);
            LambdaExpression lambda = Expression.Lambda(memExp, paramExpression);
            return source.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable),
                    "Select",
                    [source.ElementType, lambda.Body.Type],
                    source.Expression,
                    lambda));
        }

        /// <summary>
        /// Generates a Select query for a single property value.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        public static IQueryable Select(this IQueryable source, string propertyName)
        {
            ArgumentNullException.ThrowIfNull(source);
            Type sourceType = source.ElementType;
            return source.Select(propertyName, sourceType);
        }

        /// <summary>
        /// Generates a Select query for a single and multiple property value.
        /// </summary>
        /// <typeparam name="T">Type of the data source elements.</typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="sourceType">Type.</param>
        public static IQueryable Select<T>(this IQueryable source, string propertyName, Type sourceType)
        {
            ArgumentNullException.ThrowIfNull(source);
            ParameterExpression xParameter = Expression.Parameter(typeof(T), "o"); _ = sourceType;
            NewExpression xNew = Expression.New(typeof(T));
            IEnumerable<MemberAssignment>? bindings = propertyName?.Split(',').Select(o => o.Trim()).Select(o =>
            {
                string propertyPath = o.Split('.')[0];
                PropertyInfo? mi = typeof(T).GetProperty(propertyPath);
                MemberExpression xOriginal = Expression.Property(xParameter, mi!);
                return Expression.Bind(mi!, xOriginal);
            });
            MemberInitExpression xInit = Expression.MemberInit(xNew, bindings!);
            LambdaExpression lambda = Expression.Lambda<Func<T, T>>(xInit, xParameter);
            return source.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable),
                    "Select",
                    [source.ElementType, lambda.Body.Type],
                    source.Expression,
                    lambda));
        }

        /// <summary>
        /// Generates a Select query for a single property value.
        /// </summary>
        /// <typeparam name="T">Type of the data source elements.</typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        public static IQueryable Select<T>(this IQueryable source, string propertyName)
        {
            ArgumentNullException.ThrowIfNull(source);
            Type sourceType = source.ElementType;
            return source.Select<T>(propertyName, sourceType);
        }

        /// <summary>
        /// Generates a SKIP expression in the IQueryable source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="constValue">The const value.</param>
        /// <param name="sourceType">Type.</param>
        /// <returns></returns>
        public static IQueryable Skip(this IQueryable source, int constValue, Type sourceType)
        {
            ArgumentNullException.ThrowIfNull(source);
            _ = sourceType;
            return source.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable),
                    "Skip",
                    [source.ElementType],
                    [source.Expression, Expression.Constant(constValue)]));
        }

        /// <summary>
        /// Generates a SKIP expression in the IQueryable source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="constValue">The const value.</param>
        /// <returns></returns>
        public static IQueryable Skip(this IQueryable source, int constValue)
        {
            ArgumentNullException.ThrowIfNull(source);
            Type sourceType = source.ElementType;
            return source.Skip(constValue, sourceType);
        }

        #region Aggregate extensions

        // Func to calculate the summary aggregates when UseBindingValue is true.
        private static InvocationExpression GetInvokeExpressionAggregateFuncSummaryCalculation<TResult>(ParameterExpression paramExp, string propertyName,
                                                                                                                Expression<Func<string, object, object>> expressionFunc)
        {
            Func<Expression<Func<string, object, object>>, string, object, TResult> fun = (func, prop, rec) =>
            {
                Func<string, object, object> lambda = func.Compile();
                object? tempValue = lambda.DynamicInvoke([prop, rec]);

                // if we use UseBindingValue as true in column with nullable values, zero will be consider for null values.
                tempValue ??= NullableHelperInternal.ChangeType(0, typeof(TResult), CultureInfo.InvariantCulture);
                TResult val = (TResult)tempValue;
                return val;
            };

            InvocationExpression invokeExp = Expression.Invoke(
                Expression.Constant(fun),
                [
                                                      Expression.Constant(expressionFunc),
                                                      Expression.Constant(propertyName), paramExp
                                                  ]);
            return invokeExp;
        }

        private static InvocationExpression GetInvokeExpressionAggregateFunc<TResult>(
            ParameterExpression paramExp,
            string propertyName,
            Expression<Func<string, object, object>>
                                                                                expressionFunc)
        {
            // constructing a wrapper Func that would return a generic value
            Func<Expression<Func<string, object, object>>, string, object, TResult> fun = (func, prop, rec) =>
                {
                    Func<string, object, object> lambda = func.Compile();
                    TResult? val = (TResult?)lambda.DynamicInvoke([prop, rec]);
                    return val!;
                };

            InvocationExpression invokeExp = Expression.Invoke(
                Expression.Constant(fun),
                [
                                                      Expression.Constant(expressionFunc),
                                                      Expression.Constant(propertyName), paramExp
                                                  ]);
            return invokeExp;
        }

        /// <summary>
        /// Use this method with a cached delegate, this improves performance when using complex Expressions.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="paramExp"></param>
        /// <param name="propertyName"></param>
        /// <param name="expressionFunc"></param>
        /// <returns></returns>
        private static InvocationExpression GetDelegateInvokeExpressionAggregateFunc<TResult>(ParameterExpression paramExp, string propertyName, Delegate expressionFunc)
        {
            // constructing a wrapper Func that would return a generic value
            Func<Delegate, string, object, TResult> fun = (lambda, prop, rec) =>
                {
                    // var lambda = func.Compile();
                    object? val = lambda.DynamicInvoke([prop, rec]);
                    return val != null ? (TResult)val : default!;
                };

            // Expression<Func<Delegate, string, object, TResult>> eIFunc = (func, prop, rec) => fun(func, prop, rec);
            InvocationExpression invokeExp = Expression.Invoke(
                Expression.Constant(fun),
                [
                                                      Expression.Constant(expressionFunc),
                                                      Expression.Constant(propertyName), paramExp
                                                  ]);
            return invokeExp;
        }

        // MethodInfo[] collection is calculated frequently whenever the summary value changes.
        // To prevent, this collection is calculated and it it used whenever the summary value changes.
        private static MethodInfo[]? _queryableSumMethod;

        private static MethodInfo[] QueryableSummethod => _queryableSumMethod ??= [.. typeof(Queryable).GetMethods().Where(m => m.Name == "Sum" && m.GetParameters().Length == 2)];

        // MethodInfo[] collection for Queryable extensions and hold the average methods other than Int32.
        private static MethodInfo[]? _queryableaverageMethod;

        private static MethodInfo[] QueryableAverageMethod => _queryableaverageMethod ??= [.. typeof(Queryable).GetMethods().Where(m => m.Name == "Average" && m.GetParameters().Length == 2)];

        // MethodInfo[] collection is calculated frequently whenever the summary value changes.
        // To prevent, this collection is calculated and it is used whenever the summary value changes.
        private static MethodInfo[]? _enumerablesummethods;

        private static MethodInfo[] EnumerableSumMethods => _enumerablesummethods ??=
                        [.. typeof(EnumerableExtensions).GetMethods().Where(static m => m.Name == "Sum" && m.GetParameters().Length == 2)];

        // MethodInfo[] collection for enumerable extensions and hold the average methods for Int32.
        private static MethodInfo[]? _enumerableaverageMethods;

        private static MethodInfo[] EnumerableAverageMethods => _enumerableaverageMethods ??= [.. typeof(EnumerableExtensions).GetMethods().Where(m => m.Name == "Average" && m.GetParameters().Length == 2)];

        private static readonly char[] _separator = ['.'];

        #region Sum

        /// <summary>
        /// Calculates the sum of the specified property for the queryable source.
        /// </summary>
        /// <param name="source">The queryable source.</param>
        /// <param name="propertyName">The property to aggregate.</param>
        /// <param name="sourceType">The source element type.</param>
        /// <returns>The calculated sum value, or <see langword="null"/> when the query is empty or no sum method is available.</returns>
        public static object Sum(this IQueryable source, string propertyName, Type sourceType)
        {
            ArgumentNullException.ThrowIfNull(source);
            ParameterExpression paramExpression = Expression.Parameter(source.ElementType, sourceType?.Name);
            // var memExp = Expression.PropertyOrField(paramExpression, propertyName);
            Expression memExp = paramExpression.GetValueExpression(propertyName, sourceType!);
            LambdaExpression lambda = Expression.Lambda(memExp, paramExpression);

            // Commented the below lines since it was declared as an property and used whenever needed.
            // var tmethod = typeof(Queryable).GetMethods()
            //    .Where(m => m.Name == "Sum" && m.GetParameters().Length == 2).ToArray();
            Type bodyType = lambda.Body.Type;

            // Get the exact method based on the bodyType.
            MethodInfo method = GetQueryableSumMethod(bodyType);

            // if we use property type as short, Queryable not having the own method to deal Int16 type methods. so here, get the methods from EnumerableExtensions.
            if (method == null &&
                (bodyType.Name == "Int16" || NullableHelperInternal.GetUnderlyingType(bodyType)?.Name == "Int16"))
            {
                // Commented the below lines since it was declared as an property and used whenever needed.
                // var eMethods = typeof(EnumerableExtensions).GetMethods().Where(m => m.Name == "Sum" && m.GetParameters().Length == 2).ToArray();
                method = NullableHelperInternal.IsNullableType(bodyType) ? EnumerableSumMethods[2] : EnumerableSumMethods[0];
            }

            return method != null
                ? source.Provider.Execute(Expression.Call(null, method.MakeGenericMethod([sourceType!]),
                    [source.Expression, Expression.Quote(lambda)]))!
                : null!;
        }

        /// <summary>
        /// Calculates the sum of the specified property for the queryable source using the source element type.
        /// </summary>
        /// <param name="source">The queryable source.</param>
        /// <param name="propertyName">The property to aggregate.</param>
        /// <returns>The calculated sum value, or <see langword="null"/> when the query is empty or no sum method is available.</returns>
        public static object Sum(this IQueryable source, string propertyName)
        {
            ArgumentNullException.ThrowIfNull(source);
            return source.Sum(propertyName, source.ElementType);
        }

        /// <summary>
        /// Calculates the sum of the specified property using the provided expression factories to determine the value type.
        /// </summary>
        /// <param name="source">The queryable source.</param>
        /// <param name="propertyName">The property to aggregate.</param>
        /// <param name="expressionFunc">The value selector expression factory.</param>
        /// <param name="typeFunc">The type selector expression factory.</param>
        /// <returns>The calculated sum value, or <see langword="null"/> when the query is empty or no sum method is available.</returns>
        public static object Sum(this IQueryable source, string propertyName, Expression<Func<string, object, object>> expressionFunc, Expression<Func<string, object, object>> typeFunc)
        {
            ArgumentNullException.ThrowIfNull(source);
            Type? bodyType;

            // determine the return type, we expect the sequence to be of same type from the expression and hence get the first value and simply take its type
            IEnumerator enumerator = source.GetEnumerator();
            if (!enumerator.MoveNext())
            {
                return null!;
            }

            if (typeFunc != null)
            {
                Func<string, object, object> checkDelg = typeFunc.Compile();
                bodyType = (Type?)checkDelg.DynamicInvoke([propertyName, enumerator.Current]);
            }
            else
            {
                Func<string, object, object>? checkDelg = expressionFunc?.Compile();
                object? returnValue = checkDelg?.DynamicInvoke([propertyName, enumerator.Current]);
                bodyType = returnValue?.GetType();
            }

            MethodInfo method = GetQueryableAverageMethod(bodyType!);
            if (method == null && bodyType?.Name == "Int16")
            {
                MethodInfo[] eMethods = [.. typeof(EnumerableExtensions).GetMethods().Where(m => m.Name == "Sum" && m.GetParameters().Length == 2)];
                method = NullableHelperInternal.IsNullableType(bodyType) ? eMethods[1] : eMethods[0];
            }

            if (method != null)
            {
                Type sourceType = source.ElementType;
                ParameterExpression paramExp = Expression.Parameter(sourceType, sourceType?.Name);
                ConstantExpression cExp = Expression.Constant(propertyName);
                InvocationExpression iExp = Expression.Invoke(expressionFunc!, [cExp, paramExp]);

                // constructing a wrapper Func that would return Int32 value
                MethodInfo[] methods = [.. typeof(QueryableExtensions).GetMethods(BindingFlags.Static | BindingFlags.NonPublic)];
                MethodInfo? wrapperFuncMethod = methods.FirstOrDefault(m => m.Name == "GetInvokeExpressionAggregateFunc" && m.IsStatic && m.IsPrivate && m.IsGenericMethod);
                MethodInfo? genericWrapper = wrapperFuncMethod?.MakeGenericMethod([bodyType!]);
                Expression? invokeExp = (Expression?)genericWrapper?.Invoke(null, [paramExp, propertyName, expressionFunc!]);
                LambdaExpression lExp = Expression.Lambda(invokeExp!, paramExp);
                MethodCallExpression sumMethodCallExp = Expression.Call(null, method.MakeGenericMethod([sourceType!]),
                    [
                        source.Expression, Expression.Quote(lExp)
                    ]);
                object? result = source.Provider.Execute(sumMethodCallExp);
                return result!;
            }

            return 0;
        }

        #endregion

        #region GetMethodInfo method

        /// <summary>
        /// Get the exact Sum method from Queryable based on body type.
        /// </summary>
        /// <param name="bodyType"></param>
        /// <returns>exact method info.</returns>
        private static MethodInfo GetQueryableSumMethod(Type bodyType)
        {
            MethodInfo? method = null;
            if (NullableHelperInternal.IsNullableType(bodyType))
            {
                Type originalType = NullableHelperInternal.GetUnderlyingType(bodyType);
                switch (originalType.Name)
                {
                    case "Int32":
                        method = QueryableSummethod[1];
                        break;
                    case "Int64":
                        method = QueryableSummethod[3];
                        break;
                    case "Single":
                        method = QueryableSummethod[5];
                        break;
                    case "Double":
                        method = QueryableSummethod[7];
                        break;
                    case "Decimal":
                        method = QueryableSummethod[9];
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (bodyType.Name)
                {
                    case "Int32":
                        method = QueryableSummethod[0];
                        break;
                    case "Int64":
                        method = QueryableSummethod[2];
                        break;
                    case "Single":
                        method = QueryableSummethod[4];
                        break;
                    case "Double":
                        method = QueryableSummethod[6];
                        break;
                    case "Decimal":
                        method = QueryableSummethod[8];
                        break;
                    default:
                        break;
                }
            }

            return method!;
        }

        /// <summary>
        /// Get the exact Average method from Queryable based on body type.
        /// </summary>
        /// <param name="bodyType"></param>
        /// <returns>exact method info.</returns>
        private static MethodInfo GetQueryableAverageMethod(Type bodyType)
        {
            MethodInfo? method = null;
            if (NullableHelperInternal.IsNullableType(bodyType))
            {
                Type originalType = NullableHelperInternal.GetUnderlyingType(bodyType);
                MethodInfo[] nullableAvgMethods = [.. QueryableAverageMethod.Where(m => NullableHelperInternal.IsNullableType(m.ReturnType))];
                switch (originalType.Name)
                {
                    case "Int32":
                        method = nullableAvgMethods[0];
                        break;
                    case "Single":
                        method = nullableAvgMethods[1];
                        break;
                    case "Int64":
                        method = nullableAvgMethods[2];
                        break;
                    case "Double":
                        method = nullableAvgMethods[3];
                        break;
                    case "Decimal":
                        method = nullableAvgMethods[4];
                        break;
                    default:
                        break;
                }
            }
            else
            {
                //OrderBy is used to fix ordering issue between .NET 5 and .NET 6.
                MethodInfo[] avgMethods = [.. QueryableAverageMethod.Where(m => !NullableHelperInternal.IsNullableType(m.ReturnType)).OrderBy(minfo => minfo.ToString())];
                switch (bodyType.Name)
                {
                    case "Int32":
                        method = avgMethods[1];
                        break;
                    case "Single":
                        method = avgMethods[3];
                        break;
                    case "Int64":
                        method = avgMethods[2];
                        break;
                    case "Double":
                        method = avgMethods[0];
                        break;
                    case "Decimal":
                        method = avgMethods[4];
                        break;
                    default:
                        break;
                }
            }

            return method!;
        }

        #endregion

        #region Average

        /// <summary>
        /// Calculates the average value for the specified property.
        /// </summary>
        /// <param name="source">The queryable source.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>The calculated average value.</returns>
        public static object Average(this IQueryable source, string propertyName)
        {
            ArgumentNullException.ThrowIfNull(source);
            Type sourceType = source.ElementType;
            return source.Average(propertyName, sourceType);
        }

        /// <summary>
        /// Calculates the average value for the specified property using the provided source type.
        /// </summary>
        /// <param name="source">The queryable source.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="sourceType">The source element type.</param>
        /// <returns>The calculated average value.</returns>
        public static object Average(this IQueryable source, string propertyName, Type sourceType)
        {
            ArgumentNullException.ThrowIfNull(source);
            ParameterExpression paramExpression = Expression.Parameter(source.ElementType, sourceType?.Name);
            // var memExp = Expression.PropertyOrField(paramExpression, propertyName);
            Expression memExp = paramExpression.GetValueExpression(propertyName, sourceType!);
            LambdaExpression lambda = Expression.Lambda(memExp, paramExpression);

            // Commented the below lines since it was declared as an property and used whenever needed.
            Type bodyType = lambda.Body.Type;

            // Get the exact method based on the bodyType.
            MethodInfo method = GetQueryableAverageMethod(bodyType);

            // if we use property type as short, Queryable not having the own method to deal Int16 type methods. so here, we are getting the methods from EnumerableExtensions.
            if (method == null &&
                (bodyType.Name == "Int16" || NullableHelperInternal.GetUnderlyingType(bodyType)?.Name == "Int16"))
            {
                // Commented the below lines since it was declared as an property and used whenever needed.
                method = NullableHelperInternal.IsNullableType(bodyType) ? EnumerableAverageMethods[2] : EnumerableAverageMethods[0];
            }

            if (method != null)
            {
                return source.Provider.Execute(Expression.Call(null, method.MakeGenericMethod([sourceType!]),
                            [source.Expression, Expression.Quote(lambda)]))!;
            }

            return null!;
        }

        /// <summary>
        /// Calculates the average value for the specified property using a custom expression factory.
        /// </summary>
        /// <param name="source">The queryable source.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="expressionFunc">The expression factory.</param>
        /// <param name="typeFunc">The type resolver factory.</param>
        /// <returns>The calculated average value.</returns>
        public static object Average(this IQueryable source, string propertyName, Expression<Func<string, object, object>> expressionFunc, Expression<Func<string, object, object>> typeFunc)
        {
            ArgumentNullException.ThrowIfNull(source);
            Type? bodyType;
            MethodInfo[] tmethod = [.. typeof(Queryable).GetMethods().Where(m => m.Name == "Average" && m.GetParameters().Length == 2)];
            // determine the return type, we expect the sequence to be of same type from the expression and hence get the first value and simply take its type
            IEnumerator enumerator = source.GetEnumerator();
            if (!enumerator.MoveNext())
            {
                return null!;
            }

            if (typeFunc != null)
            {
                Func<string, object, object> checkDelg = typeFunc.Compile();
                bodyType = (Type?)checkDelg.DynamicInvoke([propertyName, enumerator.Current]);
            }
            else
            {
                Func<string, object, object>? checkDelg = expressionFunc?.Compile();
                object? returnValue = checkDelg?.DynamicInvoke([propertyName, enumerator.Current]);
                bodyType = returnValue?.GetType();
            }

            MethodInfo method = GetQueryableAverageMethod(bodyType!);
            if (method == null && bodyType?.Name == "Int16")
            {
                MethodInfo[] eMethods = [.. typeof(EnumerableExtensions).GetMethods().Where(m => m.Name == "Average" && m.GetParameters().Length == 2)];
                method = NullableHelperInternal.IsNullableType(bodyType) ? eMethods[1] : eMethods[0];
            }

            if (method != null)
            {
                Type sourceType = source.ElementType;
                ParameterExpression paramExp = Expression.Parameter(sourceType, sourceType?.Name);
                ConstantExpression cExp = Expression.Constant(propertyName);
                InvocationExpression iExp = Expression.Invoke(expressionFunc!, [cExp, paramExp]);

                // constructing a wrapper Func that would return Int32 value
                MethodInfo[] methods = [.. typeof(QueryableExtensions).GetMethods(BindingFlags.Static | BindingFlags.NonPublic)];
                MethodInfo? wrapperFuncMethod = methods.FirstOrDefault(m => m.Name == "GetInvokeExpressionAggregateFunc" && m.IsStatic && m.IsPrivate && m.IsGenericMethod);
                MethodInfo? genericWrapper = wrapperFuncMethod?.MakeGenericMethod([bodyType!]);
                Expression? invokeExp = (Expression?)genericWrapper?.Invoke(null, [paramExp, propertyName, expressionFunc!]);
                LambdaExpression lExp = Expression.Lambda(invokeExp!, paramExp);
                MethodCallExpression avgMethodCallExp = Expression.Call(null, method.MakeGenericMethod([sourceType!]),
        [
            source.Expression, Expression.Quote(lExp)
        ]);
                object? result = source.Provider.Execute(avgMethodCallExp);
                return result!;
            }

            return 0;
        }

        #endregion

        #region Max

        /// <summary>
        /// Returns the maximum value for the specified property.
        /// </summary>
        /// <param name="source">The queryable source.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>The maximum value.</returns>
        public static object Max(this IQueryable source, string propertyName)
        {
            ArgumentNullException.ThrowIfNull(source);
            Type sourceType = source.ElementType;
            return source.Max(propertyName, sourceType);
        }

        /// <summary>
        /// Returns the maximum value for the specified property using the provided source type.
        /// </summary>
        /// <param name="source">The queryable source.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="sourceType">The source element type.</param>
        /// <returns>The maximum value.</returns>
        public static object Max(this IQueryable source, string propertyName, Type sourceType)
        {
            ArgumentNullException.ThrowIfNull(source);
            ParameterExpression paramExpression = Expression.Parameter(source.ElementType, sourceType?.Name);
            Expression memExp = paramExpression.GetValueExpression(propertyName, sourceType!);
            LambdaExpression lambda = Expression.Lambda(memExp, paramExpression);
            MethodInfo? method = typeof(Queryable).GetMethods().LastOrDefault(m => m.Name == "Max");
            MethodCallExpression methodExp = Expression.Call(null, method!.MakeGenericMethod([sourceType!, lambda.Body.Type]),
        [
            source.Expression, Expression.Quote(lambda)
        ]);
            return source?.Provider?.Execute(methodExp)!;
        }

        /// <summary>
        /// Returns the maximum value for the specified property using a custom expression factory.
        /// </summary>
        /// <param name="source">The queryable source.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="expressionFunc">The expression factory.</param>
        /// <returns>The maximum value.</returns>
        public static object Max(this IQueryable source, string propertyName,
                                 Expression<Func<string, object, object>> expressionFunc)
        {
            ArgumentNullException.ThrowIfNull(source);
            IEnumerator enumerator = source.GetEnumerator();
            if (enumerator == null)
            {
                return null!;
            }

            Func<string, object, object>? checkDelg = expressionFunc?.Compile();
            Type? bodyType = null;

            // invoking this delegate will return a value, that determines the type of method to be called in the Queryable class.
            while (enumerator.MoveNext())
            {
                object? returnValue = checkDelg?.DynamicInvoke([propertyName, enumerator.Current]);
                if (returnValue != null)
                {
                    bodyType = returnValue.GetType();
                    break;
                }
            }

            if (bodyType == null)
            {
                return null!;
            }

            Type sourceType = source.ElementType;
            ParameterExpression paramExp = Expression.Parameter(sourceType, sourceType?.Name);
            ConstantExpression cExp = Expression.Constant(propertyName);
            InvocationExpression iExp = Expression.Invoke(expressionFunc!, [cExp, paramExp]);

            // constructing a wrapper Func that would return Int32 value
            MethodInfo[] methods = [.. typeof(QueryableExtensions).GetMethods(BindingFlags.Static | BindingFlags.NonPublic)];
            MethodInfo? wrapperFuncMethod =
                methods.FirstOrDefault(
                    m => m.Name == "GetInvokeExpressionAggregateFuncSummaryCalculation" && m.IsStatic && m.IsPrivate && m.IsGenericMethod);
            MethodInfo? genericWrapper = wrapperFuncMethod?.MakeGenericMethod([bodyType]);
            Expression? invokeExp =
                (Expression?)genericWrapper?.Invoke(null, [paramExp, propertyName, expressionFunc!]);
            LambdaExpression lExp = Expression.Lambda(invokeExp!, paramExp);
            MethodInfo? method = typeof(Queryable).GetMethods().LastOrDefault(m => m.Name == "Max");
            MethodCallExpression maxMethodCallExp = Expression.Call(null, method!.MakeGenericMethod([sourceType!, bodyType]),
                                                   [
                                                            source.Expression, Expression.Quote(lExp)
                                                        ]);
            object? result = source.Provider.Execute(maxMethodCallExp);
            return result!;
        }

        #endregion

        #region Min

        /// <summary>
        /// Returns the minimum value for the specified property.
        /// </summary>
        /// <param name="source">The queryable source.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>The minimum value.</returns>
        public static object Min(this IQueryable source, string propertyName)
        {
            ArgumentNullException.ThrowIfNull(source);
            Type sourceType = source.ElementType;
            return source.Min(propertyName, sourceType);
        }

        /// <summary>
        /// Returns the minimum value for the specified property using the provided source type.
        /// </summary>
        /// <param name="source">The queryable source.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="sourceType">The source element type.</param>
        /// <returns>The minimum value.</returns>
        public static object Min(this IQueryable source, string propertyName, Type sourceType)
        {
            ArgumentNullException.ThrowIfNull(source);
            ParameterExpression paramExpression = Expression.Parameter(source.ElementType, sourceType?.Name);
            Expression memExp = paramExpression.GetValueExpression(propertyName, sourceType!);
            LambdaExpression lambda = Expression.Lambda(memExp, paramExpression);
            MethodInfo? method = typeof(Queryable).GetMethods().LastOrDefault(m => m.Name == "Min");
            MethodCallExpression methodExp = Expression.Call(null, method!.MakeGenericMethod([sourceType!, lambda.Body.Type]),
                                            [source.Expression, lambda]);
            return source.Provider.Execute(methodExp)!;
        }

        /// <summary>
        /// Returns the minimum value for the specified property using a custom expression factory.
        /// </summary>
        /// <param name="source">The queryable source.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="expressionFunc">The expression factory.</param>
        /// <returns>The minimum value.</returns>
        public static object Min(this IQueryable source, string propertyName, Expression<Func<string, object, object>> expressionFunc)
        {
            ArgumentNullException.ThrowIfNull(source);
            IEnumerator enumerator = source.GetEnumerator();
            if (enumerator == null)
            {
                return null!;
            }

            enumerator = source.GetEnumerator();
            Func<string, object, object>? checkDelg = expressionFunc?.Compile();
            // invoking this delegate will return a value, that determines the type of method to be called in the Queryable class.
            Type? bodyType = null;

            // invoking this delegate will return a value, that determines the type of method to be called in the Queryable class.
            while (enumerator.MoveNext())
            {
                object? returnValue = checkDelg?.DynamicInvoke([propertyName, enumerator.Current]);
                if (returnValue != null)
                {
                    bodyType = returnValue.GetType();
                    break;
                }
            }

            if (bodyType == null)
            {
                return null!;
            }

            Type sourceType = source.ElementType;
            ParameterExpression paramExp = Expression.Parameter(sourceType, sourceType?.Name);
            ConstantExpression cExp = Expression.Constant(propertyName);
            InvocationExpression iExp = Expression.Invoke(expressionFunc!, [cExp, paramExp]);

            // constructing a wrapper Func that would return Int32 value
            MethodInfo[] methods = [.. typeof(QueryableExtensions).GetMethods(BindingFlags.Static | BindingFlags.NonPublic)];
            MethodInfo? wrapperFuncMethod = methods.FirstOrDefault(m => m.Name == "GetInvokeExpressionAggregateFuncSummaryCalculation" && m.IsStatic && m.IsPrivate && m.IsGenericMethod);
            MethodInfo? genericWrapper = wrapperFuncMethod?.MakeGenericMethod([bodyType]);
            Expression? invokeExp =
                (Expression?)genericWrapper?.Invoke(null, [paramExp, propertyName, expressionFunc!]);
            LambdaExpression lExp = Expression.Lambda(invokeExp!, paramExp);
            MethodInfo? method = typeof(Queryable).GetMethods().LastOrDefault(m => m.Name == "Min");
            MethodCallExpression minMethodCallExp = Expression.Call(null, method!.MakeGenericMethod([sourceType!, bodyType]), [source.Expression, Expression.Quote(lExp)]);
            object? result = source.Provider.Execute(minMethodCallExp);
            return result!;
        }

        #endregion

        #endregion

        /// <summary>
        /// Generates a TAKE expression in the IQueryable source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="constValue">The const value.</param>
        /// <param name="sourceType">Type.</param>
        /// <returns></returns>
        public static IQueryable Take(this IQueryable source, int constValue, Type sourceType)
        {
            ArgumentNullException.ThrowIfNull(source);
            return source.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable),
                    "Take",
                    [sourceType],
                    [source.Expression, Expression.Constant(constValue)]));
        }

        /// <summary>
        /// Generates a TAKE expression in the IQueryable source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="constValue">The const value.</param>
        /// <returns></returns>
        public static IQueryable Take(this IQueryable source, int constValue)
        {
            ArgumentNullException.ThrowIfNull(source);
            Type sourceType = source.ElementType;
            return source.Take(constValue, sourceType);
        }

        /// <summary>
        /// Generates a ThenBy query for the Queryable source.
        /// <para></para>
        /// <code lang="C#">            DataClasses1DataContext db = new
        /// DataClasses1DataContext();
        ///             var orders = db.Orders.Skip(0).Take(10).ToList();
        ///             var queryable = orders.AsQueryable();
        ///             var sortedOrders = queryable.OrderBy(&quot;ShipCountry&quot;);
        ///             sortedOrders = sortedOrders.ThenBy(&quot;ShipCity&quot;);</code>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="sourceType"></param>
        public static IQueryable ThenBy(this IQueryable source, string propertyName, Type sourceType)
        {
            ArgumentNullException.ThrowIfNull(source);
            ParameterExpression paramExpression = Expression.Parameter(source.ElementType, sourceType?.Name);
            LambdaExpression lambda = GetLambdaWithComplexPropertyNullCheck(source, propertyName ?? string.Empty, paramExpression, sourceType!);
            return source.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable),
                    "ThenBy",
                    [source.ElementType, lambda.Body.Type],
                    [source.Expression, lambda]));
        }

        /// <summary>
        /// Generates a ThenBy query for the Queryable source.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        public static IQueryable ThenBy(this IQueryable source, string propertyName)
        {
            ArgumentNullException.ThrowIfNull(source);
            Type sourceType = source.ElementType;
            return source.ThenBy(propertyName, sourceType);
        }

        /// <summary>
        /// Generates a ThenBy query for the Queryable source.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="comparer"></param>
        /// <param name="expressionFunc"></param>
        public static IQueryable ThenBy(this IQueryable source, string propertyName, IComparer<object> comparer,
                                        Expression<Func<string, object, object>> expressionFunc)
        {
            ArgumentNullException.ThrowIfNull(source);
            Type sourceType = source.ElementType;
            ParameterExpression paramExpression = Expression.Parameter(sourceType, sourceType?.Name);
            ConstantExpression cExp = Expression.Constant(propertyName);
            InvocationExpression iExp = Expression.Invoke(expressionFunc, [cExp, paramExpression]);
            LambdaExpression lambda = Expression.Lambda(iExp, paramExpression);
            MethodInfo? method =
                typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name == "ThenBy" && m.GetParameters().Length == 3);
            ConstantExpression conExp = Expression.Constant(comparer, typeof(IComparer<object>));
            MethodCallExpression methodExp = Expression.Call(
                null,
                method?.MakeGenericMethod([source.ElementType, lambda.Body.Type])!,
                [source.Expression, lambda, conExp]);
            return source.Provider.CreateQuery(methodExp);
        }

        /// <summary>
        /// Generates a ThenBy query for the Queryable source.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="expressionFunc"></param>
        public static IQueryable ThenBy(this IQueryable source, string propertyName,
                                        Expression<Func<string, object, object>> expressionFunc)
        {
            ArgumentNullException.ThrowIfNull(source);
            Type sourceType = source.ElementType;
            ParameterExpression paramExpression = Expression.Parameter(sourceType, sourceType?.Name);
            ConstantExpression cExp = Expression.Constant(propertyName);
            InvocationExpression iExp = Expression.Invoke(expressionFunc, [cExp, paramExpression]);
            return ThenBy(source, paramExpression, iExp);
        }

        /// <summary>
        /// Generates a ThenBy query for the Queryable source.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="paramExpression"></param>
        /// <param name="mExp"></param>
        public static IQueryable ThenBy(this IQueryable source, ParameterExpression paramExpression, Expression mExp)
        {
            ArgumentNullException.ThrowIfNull(source);
            LambdaExpression lambda = Expression.Lambda(mExp, paramExpression);
            IQueryable orderedSource = source.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable),
                    "ThenBy",
                    [source.ElementType, lambda.Body.Type],
                    source.Expression,
                    lambda));
            return orderedSource;
        }

        /// <summary>
        /// Generates an ThenBy query for the IComparer defined.
        /// <para></para>
        /// <para> </para>
        /// <code lang="C#">   public class OrdersComparer :
        /// IComparer&lt;Order&gt;
        ///     {
        ///         public int Compare(Order x, Order y)
        ///         {
        ///             return string.Compare(x.ShipCountry, y.ShipCountry);
        ///         }
        ///     }</code>
        /// <para></para>
        /// <para><code lang="C#">var sortedOrders =
        /// db.Orders.Skip(0).Take(5).ToList().ThenBy(o =&gt; o, new
        /// OrdersComparer());</code></para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="comparer"></param>
        /// <param name="sourceType"></param>
        public static IQueryable ThenBy<T>(this IQueryable source, IComparer<T> comparer, Type sourceType)
        {
            ArgumentNullException.ThrowIfNull(source);
            ParameterExpression paramExpression = Expression.Parameter(source.ElementType, sourceType?.Name);
            LambdaExpression lambda = Expression.Lambda(paramExpression, paramExpression);
            MethodInfo? method =
                typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name == "ThenBy" && m.GetParameters().Length == 3);
            ConstantExpression conExp = Expression.Constant(comparer, typeof(IComparer<T>));
            MethodCallExpression methodExp = Expression.Call(
                null,
                method?.MakeGenericMethod([source.ElementType, lambda.Body.Type])!,
                [source.Expression, lambda, conExp]);
            return source.Provider.CreateQuery(methodExp);
        }

        /// <summary>
        /// Generates an ThenBy query for the IComparer defined.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="comparer"></param>
        /// <param name="sourceType"></param>
        public static IQueryable ThenBy(this IQueryable source, string propertyName, IComparer<object> comparer,
                                        Type sourceType)
        {
            ArgumentNullException.ThrowIfNull(source);
            _ = propertyName;
            ParameterExpression paramExpression = Expression.Parameter(source.ElementType, sourceType?.Name);
            LambdaExpression lambda = Expression.Lambda(paramExpression, paramExpression);
            MethodInfo? method =
                typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name == "ThenBy" && m.GetParameters().Length == 3);
            ConstantExpression conExp = Expression.Constant(comparer, typeof(IComparer<object>));
            MethodCallExpression methodExp = Expression.Call(
                null,
                method?.MakeGenericMethod([source.ElementType, lambda.Body.Type])!,
                [source.Expression, lambda, conExp]);
            return source.Provider.CreateQuery(methodExp);
        }

        /// <summary>
        /// Generates an ThenBy query for the IComparer defined.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="comparer"></param>
        public static IQueryable ThenBy<T>(this IQueryable source, IComparer<T> comparer)
        {
            ArgumentNullException.ThrowIfNull(source);
            Type sourceType = source.ElementType;
            return source.ThenBy(comparer, sourceType);
        }

        /// <summary>
        /// Generates an ThenByDescending query for the IComparer defined.
        /// <para></para>
        /// <para> </para>
        /// <code lang="C#">   public class OrdersComparer :
        /// IComparer&lt;Order&gt;
        ///     {
        ///         public int Compare(Order x, Order y)
        ///         {
        ///             return string.Compare(x.ShipCountry, y.ShipCountry);
        ///         }
        ///     }</code>
        /// <para></para>
        /// <para><code lang="C#">var sortedOrders =
        /// db.Orders.Skip(0).Take(5).ToList().ThenByDescending(o =&gt; o, new
        /// OrdersComparer());</code></para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="comparer"></param>
        /// <param name="sourceType"></param>
        public static IQueryable ThenByDescending<T>(this IQueryable source, IComparer<T> comparer, Type sourceType)
        {
            ArgumentNullException.ThrowIfNull(source);
            ParameterExpression paramExpression = Expression.Parameter(source.ElementType, sourceType?.Name);
            LambdaExpression lambda = Expression.Lambda(paramExpression, paramExpression);
            MethodInfo? method =
                typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name == "ThenByDescending" && m.GetParameters().Length == 3);
            ConstantExpression conExp = Expression.Constant(comparer, typeof(IComparer<T>));
            MethodCallExpression methodExp = Expression.Call(
                null,
                method?.MakeGenericMethod([source.ElementType, lambda.Body.Type])!,
                [source.Expression, lambda, conExp]);
            return source.Provider.CreateQuery(methodExp);
        }

        /// <summary>
        /// Generates an ThenByDescending query for the IComparer defined.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="comparer"></param>
        /// <param name="sourceType"></param>
        public static IQueryable ThenByDescending(this IQueryable source, string propertyName,
                                                  IComparer<object> comparer, Type sourceType)
        {
            ArgumentNullException.ThrowIfNull(source);
            _ = propertyName;
            ParameterExpression paramExpression = Expression.Parameter(source.ElementType, sourceType?.Name);
            LambdaExpression lambda = Expression.Lambda(paramExpression, paramExpression);
            MethodInfo? method =
                typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name == "ThenByDescending" && m.GetParameters().Length == 3);
            ConstantExpression conExp = Expression.Constant(comparer, typeof(IComparer<object>));
            MethodCallExpression methodExp = Expression.Call(
                null,
                method?.MakeGenericMethod([source.ElementType, lambda.Body.Type])!,
                [source.Expression, lambda, conExp]);
            return source.Provider.CreateQuery(methodExp);
        }

        /// <summary>
        /// Generates an ThenByDescending query for the IComparer defined.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="comparer"></param>
        public static IQueryable ThenByDescending<T>(this IQueryable source, IComparer<T> comparer)
        {
            ArgumentNullException.ThrowIfNull(source);
            Type sourceType = source.ElementType;
            return source.ThenByDescending(comparer, sourceType);
        }

        /// <summary>
        /// Generates an ThenByDescending query for the IComparer defined.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="comparer"></param>
        /// <param name="expressionFunc"></param>
        public static IQueryable ThenByDescending(this IQueryable source, string propertyName,
                                                  IComparer<object> comparer,
                                                  Expression<Func<string, object, object>> expressionFunc)
        {
            ArgumentNullException.ThrowIfNull(source);
            Type sourceType = source.ElementType;
            ParameterExpression paramExpression = Expression.Parameter(sourceType, sourceType?.Name);
            ConstantExpression cExp = Expression.Constant(propertyName);
            InvocationExpression iExp = Expression.Invoke(expressionFunc, [cExp, paramExpression]);
            LambdaExpression lambda = Expression.Lambda(iExp, paramExpression);
            MethodInfo? method =
                typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name == "ThenByDescending" && m.GetParameters().Length == 3);
            ConstantExpression conExp = Expression.Constant(comparer, typeof(IComparer<object>));
            MethodCallExpression methodExp = Expression.Call(
                null,
                method?.MakeGenericMethod([source.ElementType, lambda.Body.Type])!,
                [source.Expression, lambda, conExp]);
            return source.Provider.CreateQuery(methodExp);
        }

        /// <summary>
        /// Generates an ThenByDescending query for the IComparer defined.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="expressionFunc"></param>
        public static IQueryable ThenByDescending(this IQueryable source, string propertyName,
                                                  Expression<Func<string, object, object>> expressionFunc)
        {
            ArgumentNullException.ThrowIfNull(source);
            Type sourceType = source.ElementType;
            ParameterExpression paramExpression = Expression.Parameter(sourceType, sourceType?.Name);
            ConstantExpression cExp = Expression.Constant(propertyName);
            InvocationExpression iExp = Expression.Invoke(expressionFunc, [cExp, paramExpression]);
            return ThenByDescending(source, paramExpression, iExp);
        }

        /// <summary>
        /// Generates an ThenByDescending query for the IComparer defined.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="paramExpression"></param>
        /// <param name="mExp"></param>
        public static IQueryable ThenByDescending(this IQueryable source, ParameterExpression paramExpression,
                                                  Expression mExp)
        {
            ArgumentNullException.ThrowIfNull(source);
            LambdaExpression lambda = Expression.Lambda(mExp, paramExpression);
            IQueryable orderedSource = source.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable),
                    "ThenByDescending",
                    [source.ElementType, lambda.Body.Type],
                    source.Expression,
                    lambda));
            return orderedSource;
        }

        /// <summary>
        /// Generates a ThenByDescending query for the Queryable source.
        /// <para></para>
        /// <code lang="C#">            DataClasses1DataContext db = new
        /// DataClasses1DataContext();
        ///             var orders = db.Orders.Skip(0).Take(10).ToList();
        ///             var queryable = orders.AsQueryable();
        ///             var sortedOrders = queryable.OrderBy(&quot;ShipCountry&quot;);
        ///             sortedOrders = sortedOrders.ThenByDescending(&quot;ShipCity&quot;);</code>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="sourceType"></param>
        public static IQueryable ThenByDescending(this IQueryable source, string propertyName, Type sourceType)
        {
            ArgumentNullException.ThrowIfNull(source);
            ParameterExpression paramExpression = Expression.Parameter(source.ElementType, sourceType?.Name);
            LambdaExpression lambda = GetLambdaWithComplexPropertyNullCheck(source, propertyName ?? string.Empty, paramExpression, sourceType!);
            return source.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable),
                    "ThenByDescending",
                    [source.ElementType, lambda.Body.Type],
                    source.Expression,
                    lambda));
        }

        /// <summary>
        /// Generates a ThenByDescending query for the Queryable source.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        public static IQueryable ThenByDescending(this IQueryable source, string propertyName)
        {
            ArgumentNullException.ThrowIfNull(source);
            Type sourceType = source.ElementType;
            return source.ThenByDescending(propertyName, sourceType);
        }

        /// <summary>
        /// Generates the where expression.
        /// <para></para>
        /// <code lang="C#">            var nw = new Northwind(@&quot;Data Source =
        /// Northwind.sdf&quot;);
        ///             IQueryable queryable = nw.Orders.AsQueryable();
        ///             var filters = queryable.Where(&quot;ShipCountry&quot;,
        /// &quot;z&quot;, FilterType.Contains);
        ///             foreach (Orders item in filters)
        ///             {
        ///                 Console.WriteLine(&quot;{0}/{1}&quot;, item.OrderID,
        /// item.ShipCountry);
        ///             }</code>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value"></param>
        /// <param name="filterType"></param>
        /// <param name="isCaseSensitive"></param>
        /// <param name="sourceType"></param>
        public static IQueryable Where(this IQueryable source, string propertyName, object value, FilterType filterType,
                                       bool isCaseSensitive, Type sourceType)
        {
            ArgumentNullException.ThrowIfNull(source);
            ParameterExpression paramExpression = Expression.Parameter(source.ElementType, sourceType?.Name);
            // Code for convert complex property to simple property
            Expression memExp = paramExpression.GetValueExpression(propertyName, sourceType!);
            Type underlyingType = memExp.Type;
            if (NullableHelperInternal.IsNullableType(memExp.Type))
            {
                underlyingType = NullableHelperInternal.GetUnderlyingType(memExp.Type);
            }

            if (filterType is FilterType.Equals or FilterType.NotEquals or
                FilterType.LessThan or FilterType.LessThanOrEqual or
                FilterType.GreaterThan or FilterType.GreaterThanOrEqual)
            {
                BinaryExpression? bExp = null;
                switch (filterType)
                {
                    case FilterType.Equals:
                        if (underlyingType != typeof(string))
                        {
                            bExp = Expression.Equal(memExp, Expression.Constant(value, memExp.Type));
                        }
                        else
                        {
                            if (isCaseSensitive)
                            {
                                bExp = Expression.Equal(memExp, Expression.Constant(value, memExp.Type));
                            }
                            else
                            {
                                MethodCallExpression toLowerMethodCall = ToLowerMethodCallExpression(memExp);
                                bExp = Expression.Equal(toLowerMethodCall,
                                                        Expression.Constant(
                                                            value == null ? null : value.ToString()!.ToLowerInvariant(),
                                                            memExp.Type));
                            }
                        }

                        break;
                    case FilterType.NotEquals:
                        if (underlyingType != typeof(string))
                        {
                            bExp = Expression.NotEqual(memExp, Expression.Constant(value, memExp.Type));
                        }
                        else
                        {
                            if (isCaseSensitive)
                            {
                                bExp = Expression.NotEqual(memExp, Expression.Constant(value, memExp.Type));
                            }
                            else
                            {
                                MethodCallExpression toLowerMethodCall = ToLowerMethodCallExpression(memExp);
                                bExp = Expression.NotEqual(toLowerMethodCall,
                                                           Expression.Constant(
                                                               value?.ToString()?.ToLowerInvariant(),
                                                               memExp.Type));
                            }
                        }

                        break;
                    case FilterType.LessThan:
                        bExp = Expression.LessThan(memExp, Expression.Constant(value, memExp.Type));
                        break;
                    case FilterType.LessThanOrEqual:
                        bExp = Expression.LessThanOrEqual(memExp, Expression.Constant(value, memExp.Type));
                        break;
                    case FilterType.GreaterThan:
                        bExp = Expression.GreaterThan(memExp, Expression.Constant(value, memExp.Type));
                        break;
                    case FilterType.GreaterThanOrEqual:
                        bExp = Expression.GreaterThanOrEqual(memExp, Expression.Constant(value, memExp.Type));
                        break;
                    case FilterType.WildCard:
                        break;
                    case FilterType.IsNull:
                        break;
                    case FilterType.IsNotNull:
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

                LambdaExpression lambda = Expression.Lambda(bExp!, paramExpression);
                return source.Provider.CreateQuery(
                    Expression.Call(
                        typeof(Queryable),
                        "Where",
                        [source.ElementType],
                        source.Expression,
                        lambda));
            }
            else
            {
                MethodInfo? stringMethod = typeof(string).GetMethods().FirstOrDefault(m => m.Name == filterType.ToString());
                MethodCallExpression? methodCallExp = null;
                if (isCaseSensitive)
                {
                    methodCallExp = Expression.Call(
                        memExp,
                        stringMethod!,
                        [Expression.Constant(value, typeof(string))]);
                }
                else
                {
                    MethodCallExpression toLowerMethodCall = ToLowerMethodCallExpression(memExp);
                    methodCallExp = Expression.Call(
                        toLowerMethodCall,
                        stringMethod!,
                        [Expression.Constant(value == null ? null : value.ToString()!.ToLowerInvariant(), typeof(string))]);
                }

                LambdaExpression lambda = Expression.Lambda(methodCallExp, paramExpression);
                return source.Provider.CreateQuery(
                    Expression.Call(
                        typeof(Queryable),
                        "Where",
                        [source.ElementType],
                        source.Expression,
                        lambda));
            }
        }

        /// <summary>
        /// Generates the where expression.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value"></param>
        /// <param name="filterType"></param>
        /// <param name="isCaseSensitive"></param>
        public static IQueryable Where(this IQueryable source, string propertyName, object value, FilterType filterType,
                                       bool isCaseSensitive)
        {
            ArgumentNullException.ThrowIfNull(source);
            Type sourceType = source.ElementType;
            return source.Where(propertyName, value, filterType, isCaseSensitive, sourceType);
        }

        /// <summary>
        /// Applies paging to the queryable source.
        /// </summary>
        /// <param name="source">The queryable source.</param>
        /// <param name="pageIndex">The zero-based page index.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>The paged queryable source.</returns>
        public static IQueryable Page(this IQueryable source, int pageIndex, int pageSize)
        {
            ArgumentNullException.ThrowIfNull(source);
            IQueryable tempSource = source;
            if (pageIndex > 0)
            {
                tempSource = tempSource.Skip(pageIndex * pageSize);
            }

            if (pageSize > 0)
            {
                tempSource = tempSource.Take(pageSize);
            }

            return tempSource;
        }

        /// <summary>
        /// Use this function to generate WHERE expression based on Predicates. The
        /// AndPredicate and OrPredicate should be used in combination to build the
        /// predicate expression which is finally passed on to this function for creating a
        /// Lambda.
        /// <para></para>
        /// <para></para>
        /// <para></para>DataClasses1DataContext db = new DataClasses1DataContext();.
        /// <para></para>            var orders = db.Orders.Skip(0).Take(100).ToList();.
        /// <para></para>            var queryable = orders.AsQueryable();.
        /// <para></para>            var parameter =
        /// queryable.Parameter(&quot;ShipCountry&quot;);.
        /// <para></para>            var binaryExp = queryable.Predicate(parameter,.
        /// <para></para>&quot;ShipCountry&quot;, &quot;USA&quot;, true);.
        /// <para></para>            var filteredOrders = queryable.Where(parameter,
        /// binaryExp);.
        /// <para></para>            foreach (var order in filteredOrders).
        /// <para></para>            {.
        /// <para></para>                Console.WriteLine(order);.
        /// <para></para>            }.
        /// <para></para>
        /// <para></para>
        /// <para></para>Build Predicates for Contains / StartsWith / EndsWith,.
        /// <para></para>
        /// <para></para>            IQueryable queryable = nw.Orders.AsQueryable();.
        /// <para></para>            var parameter = queryable.Parameter();.
        /// <para></para>            var exp1 = queryable.Predicate(parameter,
        /// &quot;ShipCountry&quot;, &quot;h&quot;, FilterType.Contains);.
        /// <para></para>            var exp2 = queryable.Predicate(parameter,
        /// &quot;ShipCountry&quot;, &quot;a&quot;, FilterType.StartsWith);.
        /// <para></para>            var andExp = exp2.OrPredicate(exp1);.
        /// <para></para>            var filters = queryable.Where(parameter, andExp);.
        /// <para></para>            foreach (Orders item in filters).
        /// <para></para>            {.
        /// <para></para>                Console.WriteLine(&quot;{0}/{1}&quot;,
        /// item.OrderID, item.ShipCountry);.
        /// <para></para>            }.
        /// <para></para>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="paramExpression"></param>
        /// <param name="predicateExpression"></param>
        public static IQueryable Where(this IQueryable source, ParameterExpression paramExpression,
                                       Expression predicateExpression)
        {
            ArgumentNullException.ThrowIfNull(source);
            if (predicateExpression == null)
            {
                return Enumerable.Empty<object>().AsQueryable();
            }
            LambdaExpression lambda = Expression.Lambda(predicateExpression, paramExpression);
            return source.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable),
                    "Where",
                    [source.ElementType],
                    source.Expression,
                    lambda));
        }

        #region GroupByMany extensions

        /// <summary>
        /// Groups the specified sequence using one or more selectors and applies sort information to each level.
        /// </summary>
        /// <typeparam name="TElement">The element type.</typeparam>
        /// <param name="elements">The source elements.</param>
        /// <param name="sortFields">The sort fields to apply to grouped results.</param>
        /// <param name="groupSelectors">The selectors used to build each grouping level.</param>
        /// <returns>A sequence of grouped results.</returns>
        public static IEnumerable<GroupResult> GroupByMany<TElement>(
            this IEnumerable<TElement> elements,
            List<SortDescription> sortFields,
            IEnumerable<Func<TElement, object>> groupSelectors)
        {
            return GroupByMany(elements, sortFields, [.. groupSelectors]);
        }

        /// <summary>
        /// Groups the specified sequence using one or more selectors and applies custom sort comparers.
        /// </summary>
        /// <typeparam name="TElement">The element type.</typeparam>
        /// <param name="elements">The source elements.</param>
        /// <param name="sortFields">The sort fields to apply to grouped results.</param>
        /// <param name="sortComparers">The custom comparers keyed by property name.</param>
        /// <param name="properties">The grouping property names.</param>
        /// <param name="groupSelectors">The selectors used to build each grouping level.</param>
        /// <returns>A sequence of grouped results.</returns>
        public static IEnumerable<GroupResult> GroupByMany<TElement>(
            this IEnumerable<TElement> elements,
            List<SortDescription> sortFields,
            Dictionary<string, IComparer<object>> sortComparers,
            string[] properties,
            IEnumerable<Func<TElement, object>> groupSelectors)
        {
            return GroupByMany(elements, sortFields, sortComparers, [.. properties], [.. groupSelectors]);
        }

        /// <summary>
        /// Groups the specified sequence using one or more selectors and applies custom sort comparers.
        /// </summary>
        /// <typeparam name="TElement">The element type.</typeparam>
        /// <param name="elements">The source elements.</param>
        /// <param name="sortFields">The sort fields to apply to grouped results.</param>
        /// <param name="sortComparers">The custom comparers keyed by property name.</param>
        /// <param name="properties">The grouping property names.</param>
        /// <param name="groupSelectors">The selectors used to build each grouping level.</param>
        /// <returns>A sequence of grouped results.</returns>
        public static IEnumerable<GroupResult> GroupByMany<TElement>(
            this IEnumerable<TElement> elements,
            List<SortDescription> sortFields,
            Dictionary<string, IComparer<object>> sortComparers,
            List<string> properties,
            params Func<TElement, object>[] groupSelectors)
        {
            if (groupSelectors != null && groupSelectors.Length > 0)
            {
                Func<TElement, object> selector = groupSelectors.First();
                Func<TElement, object>[] nextSelectors = [.. groupSelectors.Skip(1)];

                IEnumerable<GroupResult> groupBy =
                    elements.GroupBy(selector).Select(
                        g => new GroupResult
                        {
                            Key = g.Key,
                            Count = g.Count(),
                            Items = g,
                            SubGroups =
                                g.GroupByMany(sortFields.Count > 0 ? [.. sortFields.Skip(1)] : sortFields,
                                                sortComparers,
                                                properties.Count > 0 ? [.. properties.Skip(0)] : properties,
                                                nextSelectors)
                        });

                if (sortFields != null && sortFields.Count > 0)
                {
                    SortDescription sortKey = sortFields.FirstOrDefault(d => d.PropertyName == properties[0]);
                    if (sortKey.PropertyName != null && sortKey != default) // && sortKey.Index == 0)
                    {
                        IComparer<object>? customComparer = null;
                        _ = (sortComparers?.TryGetValue(sortKey.PropertyName, out customComparer));

                        groupBy = sortKey.Direction == ListSortDirection.Ascending
                            ? customComparer == null ? groupBy.OrderBy(g => g.Key) : groupBy.OrderBy(g => g.Key, customComparer)
                            : customComparer == null ? groupBy.OrderByDescending(g => g.Key) : groupBy.OrderByDescending(g => g.Key, customComparer);
                    }
                }

                return groupBy;
            }
            else
            {
                return null!;
            }
        }

        // var orders = Orders.GroupBy(o => o.ShipCountry).Select(g => new { Key = g.Key, Items = g.OrderBy(o1 => o1.ShipCountry) });
        /// <summary>
        /// Groups the specified sequence by a list of sort fields and selectors.
        /// </summary>
        /// <typeparam name="TElement">The element type.</typeparam>
        /// <param name="elements">The source elements.</param>
        /// <param name="sortFields">The sort fields to apply to grouped results.</param>
        /// <param name="groupSelectors">The selectors used to build each grouping level.</param>
        /// <returns>A sequence of grouped results.</returns>
        public static IEnumerable<GroupResult> GroupByMany<TElement>(
            this IEnumerable<TElement> elements,
            List<SortDescription> sortFields,
            params Func<TElement, object>[] groupSelectors)
        {
            if (groupSelectors != null && groupSelectors.Length > 0)
            {
                Func<TElement, object> selector = groupSelectors.First();
                Func<TElement, object>[] nextSelectors = [.. groupSelectors.Skip(1)];

                IEnumerable<GroupResult> groupBy =
                    elements.GroupBy(selector).Select(
                        g => new GroupResult
                        {
                            Key = g.Key,
                            Count = g.Count(),
                            Items = g,
                            SubGroups =
                                    g.GroupByMany(
                                        sortFields.Count > 0 ? [.. sortFields.Skip(1)] : sortFields,
                                        nextSelectors)
                        });

                if (sortFields != null && sortFields.Count > 0)
                {
                    SortDescription sortKey = sortFields.First();
                    if (sortKey.PropertyName != null) // && sortKey.Index == 0)
                    {
                        groupBy = sortKey.Direction == ListSortDirection.Ascending ? groupBy.OrderBy(g => g.Key) : groupBy.OrderByDescending(g => g.Key);
                    }
                }

                return groupBy;
            }
            else
            {
                return null!;
            }
        }

        /// <summary>
        /// Groups the specified sequence by one or more selectors.
        /// </summary>
        /// <typeparam name="TElement">The element type.</typeparam>
        /// <param name="elements">The source elements.</param>
        /// <param name="groupSelectors">The selectors used to build each grouping level.</param>
        /// <returns>A sequence of grouped results.</returns>
        public static IEnumerable<GroupResult> GroupByMany<TElement>(
            this IEnumerable<TElement> elements,
            params Func<TElement, object>[] groupSelectors)
        {
            if (groupSelectors != null && groupSelectors.Length > 0)
            {
                Func<TElement, object> selector = groupSelectors.First();
                Func<TElement, object>[] nextSelectors = [.. groupSelectors.Skip(1)];

                return
                   elements.GroupBy(selector).Select(
                       g => new GroupResult
                       {
                           Key = g.Key,
                           Count = g.Count(),
                           Items = g,
                           SubGroups = g.GroupByMany(nextSelectors)
                       });
            }
            else
            {
                return null!;
            }
        }

        /// <summary>
        /// Groups the specified sequence by one or more selectors.
        /// </summary>
        /// <typeparam name="TElement">The element type.</typeparam>
        /// <param name="elements">The source elements.</param>
        /// <param name="groupSelectors">The selectors used to build each grouping level.</param>
        /// <returns>A sequence of grouped results.</returns>
        public static IEnumerable<GroupResult> GroupByMany<TElement>(
            this IEnumerable<TElement> elements,
            IEnumerable<Func<TElement, object>> groupSelectors)
        {
            return GroupByMany(elements, groupSelectors.ToArray());
        }

        /// <summary>
        /// Groups the queryable source by the specified property names.
        /// </summary>
        /// <param name="source">The queryable source.</param>
        /// <param name="properties">The property names to group by.</param>
        /// <returns>A sequence of grouped results.</returns>
        public static IEnumerable<GroupResult> GroupByMany(this IQueryable source, IEnumerable<string> properties)
        {
            return GroupByMany(source, properties.ToArray());
        }

        /// <summary>
        /// Groups the queryable source by the specified property names.
        /// </summary>
        /// <param name="source">The queryable source.</param>
        /// <param name="sourceType">The source element type.</param>
        /// <param name="properties">The property names to group by.</param>
        /// <returns>A sequence of grouped results.</returns>
        public static IEnumerable<GroupResult> GroupByMany(this IQueryable source, Type sourceType,
                                                           params string[] properties)
        {
            return source.GroupByMany(null!, sourceType, properties);
        }

        /// <summary>
        /// Groups the queryable source by the specified property names and formatting rules.
        /// </summary>
        /// <param name="source">The queryable source.</param>
        /// <param name="formatColl">The format strings keyed by property name.</param>
        /// <param name="sourceType">The source element type.</param>
        /// <param name="properties">The property names to group by.</param>
        /// <returns>A sequence of grouped results.</returns>
        public static IEnumerable<GroupResult> GroupByMany(this IQueryable source, Dictionary<string, string> formatColl,
                                                           Type sourceType, params string[] properties)
        {
            ArgumentNullException.ThrowIfNull(source);
            if (properties != null && properties.Length == 0)
            {
                return null!;
            }

            string format = string.Empty;
            List<LambdaExpression> lambdas = [];
            foreach (string property in properties ?? [])
            {
                format = string.Empty;
                ParameterExpression param = Expression.Parameter(source.ElementType, sourceType?.Name);
                // Code to convert complex property to simple property
                Expression propertyExp = param.GetValueExpression(property, sourceType!);
                UnaryExpression conv = Expression.Convert(propertyExp, typeof(object));
                if (formatColl != null)
                {
                    if (formatColl.ContainsKey(property))
                    {
                        format = formatColl.Where(key => key.Key == property).ToList().FirstOrDefault().Value;
                        if (format.Contains(property, StringComparison.Ordinal))
                        {
                            format = format.Replace(property, "0", StringComparison.Ordinal);
                        }

                        if (!string.IsNullOrEmpty(format))
                        {
                            format = Regex.Match(format, @"{0:(.*?)}").Groups[1].Value;
                        }
                    }

                    if (!string.IsNullOrEmpty(format) && propertyExp.Type != typeof(string))
                    {
                        MethodCallExpression formatMethodCall = GetFormatMethodCallExpression(propertyExp, format);
                        conv = Expression.Convert(formatMethodCall, typeof(object));
                    }
                }

                LambdaExpression lambdaExp = Expression.Lambda(conv, [param]);
                lambdas.Add(lambdaExp);
            }

            IList values = CreateGeneric(typeof(List<>), lambdas[0].Type);
            foreach (LambdaExpression lambdaExp in lambdas)
            {
                _ = values.Add(lambdaExp.Compile());
            }

            // ElementAt(1) is the GroupByMany method with IEnumerable<T> properties
            MethodInfo methodInfo = GetGroupByManyMethod();

            Type[] genericArgs = [source.ElementType];
            MethodInfo genericMethodInfo = methodInfo.MakeGenericMethod(genericArgs);
            IEnumerable<GroupResult>? result = genericMethodInfo.Invoke(null, [source, values]) as IEnumerable<GroupResult>;
            return result!;
        }

        /// <summary>
        /// Groups the queryable source by the specified property names and applies sort information.
        /// </summary>
        /// <param name="source">The queryable source.</param>
        /// <param name="sourceType">The source element type.</param>
        /// <param name="sortFields">The sort fields to apply to grouped results.</param>
        /// <param name="properties">The property names to group by.</param>
        /// <returns>A sequence of grouped results.</returns>
        public static IEnumerable<GroupResult> GroupByMany(this IQueryable source, Type sourceType,
                                   List<SortDescription> sortFields, params string[] properties)
        {
            ArgumentNullException.ThrowIfNull(source);
            if (properties != null && properties.Length == 0)
            {
                return null!;
            }

            List<LambdaExpression> lambdas = [];
            foreach (string property in properties ?? [])
            {
                ParameterExpression param = Expression.Parameter(source.ElementType, sourceType?.Name);
                // Code to convert complex property to simple property
                Expression propertyExp = param.GetValueExpression(property, sourceType!);
                UnaryExpression conv = Expression.Convert(propertyExp, typeof(object));
                LambdaExpression lambdaExp = Expression.Lambda(conv, [param]);
                lambdas.Add(lambdaExp);
            }

            IList values = CreateGeneric(typeof(List<>), lambdas[0].Type);
            foreach (LambdaExpression lambdaExp in lambdas)
            {
                _ = values.Add(lambdaExp.Compile());
            }

            // ElementAt(1) is the GroupByMany method with IEnumerable<T> properties
            MethodInfo methodInfo = GetGroupByManyMethod2();

            Type[] genericArgs = [source.ElementType];
            MethodInfo genericMethodInfo = methodInfo.MakeGenericMethod(genericArgs);
            IEnumerable<GroupResult>? result = genericMethodInfo.Invoke(null, [source, sortFields, values]) as IEnumerable<GroupResult>;
            return result!;
        }

        /// <summary>
        /// Groups the enumerable source by the specified property names using a selector factory.
        /// </summary>
        /// <param name="source">The enumerable source.</param>
        /// <param name="sourceType">The source element type.</param>
        /// <param name="GetExpressionFunc">A factory that creates a grouping expression for each property.</param>
        /// <param name="properties">The property names to group by.</param>
        /// <returns>A sequence of grouped results.</returns>
        public static IEnumerable<GroupResult> GroupByMany(this IEnumerable source, Type sourceType,
                                   Func<string, Expression> GetExpressionFunc,
                                   params string[] properties)
        {
            ArgumentNullException.ThrowIfNull(source);
            if (properties != null && properties.Length == 0)
            {
                return null!;
            }

            Type elementType = source.GetElementType();
            List<LambdaExpression> lambdas = [];
            foreach (string property in properties ?? [])
            {
                ParameterExpression param = Expression.Parameter(elementType, sourceType?.Name);
                // Code to convert complex property to simple property
                Expression? expressionFunc = GetExpressionFunc != null ? GetExpressionFunc(property) : null;

                if (expressionFunc == null)
                {
                    if (property.Contains('.', StringComparison.Ordinal))
                    {
                        LambdaExpression lambdaExp = GetLambdaWithComplexPropertyNullCheck(source, property, param, sourceType!);
                        lambdas.Add(lambdaExp);
                    }
                    else
                    {
                        Expression propertyExp = param.GetValueExpression(property, sourceType!);
                        UnaryExpression conv = Expression.Convert(propertyExp, typeof(object));
                        LambdaExpression lambdaExp = Expression.Lambda(conv, [param]);
                        lambdas.Add(lambdaExp);
                    }
                }
                else
                {
                    ConstantExpression cExp = Expression.Constant(property);
                    InvocationExpression iExp = Expression.Invoke(expressionFunc, [cExp, param]);
                    LambdaExpression lambdaExp = Expression.Lambda(iExp, param);
                    lambdas.Add(lambdaExp);
                }
            }

            IList values = CreateGeneric(typeof(List<>), lambdas[0].Type);
            foreach (LambdaExpression lambdaExp in lambdas)
            {
                _ = values.Add(lambdaExp.Compile());
            }

            // ElementAt(1) is the GroupByMany method with IEnumerable<T> properties
            MethodInfo methodInfo = GetGroupByManyMethod();

            Type[] genericArgs = [elementType];
            MethodInfo genericMethodInfo = methodInfo.MakeGenericMethod(genericArgs);
            IEnumerable<GroupResult>? result = genericMethodInfo.Invoke(null, [source, values]) as IEnumerable<GroupResult>;
            return result!;
        }

        /// <summary>
        /// Groups the enumerable source by the specified property names using custom comparers.
        /// </summary>
        /// <param name="source">The enumerable source.</param>
        /// <param name="sourceType">The source element type.</param>
        /// <param name="sortFields">The sort fields to apply to grouped results.</param>
        /// <param name="sortComparers">The custom comparers keyed by property name.</param>
        /// <param name="GetExpressionFunc">A factory that creates a grouping expression for each property.</param>
        /// <param name="properties">The property names to group by.</param>
        /// <returns>A sequence of grouped results.</returns>
        public static IEnumerable<GroupResult> GroupByMany(this IEnumerable source, Type sourceType,
                                   List<SortDescription> sortFields,
                                   Dictionary<string, IComparer<object>> sortComparers,
                                   Func<string, Expression> GetExpressionFunc,
                                   params string[] properties)
        {
            ArgumentNullException.ThrowIfNull(source);
            if (properties != null && properties.Length == 0)
            {
                return null!;
            }

            Type elementType = source.GetElementType();
            List<LambdaExpression> lambdas = [];
            foreach (string property in properties ?? [])
            {
                ParameterExpression param = Expression.Parameter(elementType, sourceType?.Name);
                // Code to convert complex property to simple property
                Expression? expressionFunc = GetExpressionFunc != null ? GetExpressionFunc(property) : null;
                if (expressionFunc == null)
                {
                    Expression propertyExp = param.GetValueExpression(property, sourceType!);
                    UnaryExpression conv = Expression.Convert(propertyExp, typeof(object));
                    LambdaExpression lambdaExp = Expression.Lambda(conv, [param]);
                    lambdas.Add(lambdaExp);
                }
                else
                {
                    ConstantExpression cExp = Expression.Constant(property);
                    InvocationExpression iExp = Expression.Invoke(expressionFunc, [cExp, param]);
                    LambdaExpression lambdaExp = Expression.Lambda(iExp, param);
                    lambdas.Add(lambdaExp);
                }
            }

            IList values = CreateGeneric(typeof(List<>), lambdas[0].Type);
            foreach (LambdaExpression lambdaExp in lambdas)
            {
                _ = values.Add(lambdaExp.Compile());
            }

            // ElementAt(1) is the GroupByMany method with IEnumerable<T> properties
            MethodInfo methodInfo = GetGroupByManyMethod3();

            Type[] genericArgs = [elementType];
            MethodInfo genericMethodInfo = methodInfo.MakeGenericMethod(genericArgs);
            IEnumerable<GroupResult>? result =
                genericMethodInfo.Invoke(null, [source, sortFields, sortComparers, properties!, values]) as
                IEnumerable<GroupResult>;
            return result!;
        }

        /// <summary>
        /// Groups the enumerable source by the specified property names and applies sort information.
        /// </summary>
        /// <param name="source">The enumerable source.</param>
        /// <param name="sourceType">The source element type.</param>
        /// <param name="sortFields">The sort fields to apply to grouped results.</param>
        /// <param name="GetExpressionFunc">A factory that creates a grouping expression for each property.</param>
        /// <param name="properties">The property names to group by.</param>
        /// <returns>A sequence of grouped results.</returns>
        public static IEnumerable<GroupResult> GroupByMany(this IEnumerable source, Type sourceType,
                                   List<SortDescription> sortFields,
                                   Func<string, Expression> GetExpressionFunc,
                                   params string[] properties)
        {
            ArgumentNullException.ThrowIfNull(source);
            if (properties != null && properties.Length == 0)
            {
                return null!;
            }

            Type elementType = source.GetElementType();
            List<LambdaExpression> lambdas = [];
            foreach (string property in properties ?? [])
            {
                ParameterExpression param = Expression.Parameter(elementType, sourceType?.Name);
                // Code to convert complex property to simple property
                Expression? expressionFunc = GetExpressionFunc != null ? GetExpressionFunc(property) : null;
                if (expressionFunc == null)
                {
                    Expression propertyExp = param.GetValueExpression(property, sourceType!);
                    UnaryExpression conv = Expression.Convert(propertyExp, typeof(object));
                    LambdaExpression lambdaExp = Expression.Lambda(conv, [param]);
                    lambdas.Add(lambdaExp);
                }
                else
                {
                    ConstantExpression cExp = Expression.Constant(property);
                    InvocationExpression iExp = Expression.Invoke(expressionFunc, [cExp, param]);
                    LambdaExpression lambdaExp = Expression.Lambda(iExp, param);
                    lambdas.Add(lambdaExp);
                }
            }

            IList values = CreateGeneric(typeof(List<>), lambdas[0].Type);
            foreach (LambdaExpression lambdaExp in lambdas)
            {
                _ = values.Add(lambdaExp.Compile());
            }

            // ElementAt(1) is the GroupByMany method with IEnumerable<T> properties
            MethodInfo methodInfo = GetGroupByManyMethod2();

            Type[] genericArgs = [elementType];
            MethodInfo genericMethodInfo = methodInfo.MakeGenericMethod(genericArgs);
            IEnumerable<GroupResult>? result =
                genericMethodInfo.Invoke(null, [source, sortFields, values]) as IEnumerable<GroupResult>;
            return result!;
        }

        /// <summary>
        /// Gets the <see cref="MethodInfo"/> for the <c>GroupByMany</c> overload that accepts enumerable selectors.
        /// </summary>
        /// <returns>The matching <see cref="MethodInfo"/>.</returns>
        private static MethodInfo GetGroupByManyMethod()
        {
            MethodInfo? method = null;
            IEnumerable<MethodInfo> methods = typeof(QueryableExtensions).GetMethods().Where(m => m.Name == "GroupByMany" && m.IsStatic && m.IsPublic && m.IsGenericMethod);
            foreach (MethodInfo? m in methods)
            {
                ParameterInfo[] pInfo = m.GetParameters();
                if (pInfo.Length > 1)
                {
                    ParameterInfo p = pInfo[1];
                    if (typeof(IEnumerable<>).Name == p.ParameterType.Name)
                    {
                        method = m;
                        break;
                    }
                }

            }

            return method!;
        }

        /// <summary>
        /// Gets the <see cref="MethodInfo"/> for the <c>GroupByMany</c> overload that accepts sort fields and enumerable selectors.
        /// </summary>
        /// <returns>The matching <see cref="MethodInfo"/>.</returns>
        private static MethodInfo GetGroupByManyMethod2()
        {
            MethodInfo? method = null;
            IEnumerable<MethodInfo> methods =
                typeof(QueryableExtensions).GetMethods()
                                            .Where(
                                                m =>
                                                m.Name == "GroupByMany" && m.IsStatic && m.IsPublic && m.IsGenericMethod);
            foreach (MethodInfo? m in methods)
            {
                ParameterInfo[] pInfo = m.GetParameters();
                if (pInfo.Length > 2)
                {
                    ParameterInfo p = pInfo[2];
                    if (typeof(IEnumerable<>).Name == p.ParameterType.Name)
                    {
                        method = m;
                        break;
                    }
                }
            }

            return method!;
        }

        /// <summary>
        /// Gets the <see cref="MethodInfo"/> for the <c>GroupByMany</c> overload that accepts custom comparers.
        /// </summary>
        /// <returns>The matching <see cref="MethodInfo"/>.</returns>
        private static MethodInfo GetGroupByManyMethod3()
        {
            MethodInfo? method = null;
            IEnumerable<MethodInfo> methods =
                typeof(QueryableExtensions).GetMethods()
                                            .Where(
                                                m =>
                                                m.Name == "GroupByMany" && m.IsStatic && m.IsPublic && m.IsGenericMethod);
            foreach (MethodInfo m in methods)
            {
                ParameterInfo[] pInfo = m.GetParameters();
                if (pInfo.Length > 3)
                {
                    ParameterInfo p = pInfo[2];
                    if (typeof(Dictionary<string, IComparer<object>>).Name == p.ParameterType.Name)
                    {
                        method = m;
                        break;
                    }
                }
            }

            return method!;
        }

        /// <summary>
        /// Groups the queryable source by the specified property names.
        /// </summary>
        /// <param name="source">The queryable source.</param>
        /// <param name="properties">The property names to group by.</param>
        /// <returns>A sequence of grouped results.</returns>
        public static IEnumerable<GroupResult> GroupByMany(this IQueryable source, params string[] properties)
        {
            ArgumentNullException.ThrowIfNull(source);
            Type sourceType = source.ElementType;
            return source.GroupByMany(sourceType, properties);
        }

        /// <summary>
        /// Groups the queryable source by the specified property names using formatting rules.
        /// </summary>
        /// <param name="source">The queryable source.</param>
        /// <param name="formatcoll">The format strings keyed by property name.</param>
        /// <param name="properties">The property names to group by.</param>
        /// <returns>A sequence of grouped results.</returns>
        public static IEnumerable<GroupResult> GroupByMany(this IQueryable source, Dictionary<string, string> formatcoll,
                                   params string[] properties)
        {
            ArgumentNullException.ThrowIfNull(source);
            Type sourceType = source.ElementType;
            return source.GroupByMany(formatcoll, sourceType, properties);
        }

        #endregion

        private static IList CreateGeneric(Type generic, Type innerType, params object[] args)
        {
            Type specificType = generic.MakeGenericType([innerType]);
            return (IList)Activator.CreateInstance(specificType, args)!;
        }

        /// <summary>
        /// Gets the runtime element type for the specified queryable source.
        /// </summary>
        /// <param name="source">The queryable source.</param>
        /// <returns>The element type represented by the queryable source.</returns>
        public static Type GetObjectType(this IQueryable source)
        {
            ArgumentNullException.ThrowIfNull(source);
            Type enumerable = source.ElementType;
            if (enumerable == typeof(object))
            {
                IEnumerator e = source.GetEnumerator();
                if (e.MoveNext())
                {
                    enumerable = e.Current.GetType();
                }
            }

            return enumerable;
        }
    }

    /// <summary>
    /// Represents the result of a grouped query, including the grouping key, item count, items, and child groups.
    /// </summary>
    public class GroupResult
    {
        /// <summary>
        /// Gets or sets the grouping key.
        /// </summary>
        public object Key { get; set; }

        /// <summary>
        /// Gets or sets the number of items in the group.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets the items contained in the group.
        /// </summary>
        public IEnumerable Items { get; set; }

        /// <summary>
        /// Gets or sets the nested subgroups.
        /// </summary>
        public IEnumerable<GroupResult> SubGroups { get; set; }

        /// <summary>
        /// Returns a string representation of the group result.
        /// </summary>
        /// <returns>A string containing the key and item count.</returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} ({1})", Key, Count);
        }
    }

    /// <summary>
    /// Associates a sort index with a sort description.
    /// </summary>
    public class SortDescriptionIndex
    {
        /// <summary>
        /// Gets or sets the sort index.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the sort description.
        /// </summary>
        public SortDescription SortDescription { get; set; }
    }
}
