using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using Syncfusion.Blazor.Toolkit.Data;

namespace Syncfusion.Blazor.Toolkit.Data
{
    /// <summary>
    /// Provides extension methods for enumerable collections to support parallel invocation and data operations.
    /// </summary>
    public static class EnumerableExtensions
    {
        internal static IEnumerable InvokeParallel(this IEnumerable source, Predicate<object> func, Type sourceType)
        {
            if (sourceType == null)
            {
                sourceType = source.GetElementType();
            }

            MethodInfo? genericWrapper = typeof(EnumerableExtensions).GetMethods().FirstOrDefault(m => m.Name == "InvokeParallelExecution" && m.IsStatic && m.IsGenericMethod);
            genericWrapper = genericWrapper?.MakeGenericMethod(sourceType);
            return (genericWrapper?.Invoke(null, [source, func]) as ParallelQuery)!;
        }

        internal static IEnumerable InvokeParallel(this IEnumerable source, Predicate<object> func)
        {
            Type sourceType = source.GetElementType();
            return source.InvokeParallel(func, sourceType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static ParallelQuery InvokeParallelExecution<T>(IEnumerable source, Predicate<object> func)
        {
            IEnumerable<T>? enumerable = source as IEnumerable<T>;
            return enumerable!.AsParallel().AsOrdered().Where(rec =>
            {
                return func(rec!);
            });
        }

        #region Average

        /// <summary>
        /// Computes the average of the sequence of <see cref="short"/> values obtained by invoking
        /// the specified selector expression on each element of the input <see cref="IQueryable{TSource}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the input sequence.</typeparam>
        /// <param name="source">An <see cref="IQueryable{TSource}"/> whose elements to apply the selector to.</param>
        /// <param name="selector">An expression that projects each element to a <see cref="short"/> value to average.</param>
        /// <returns>The average of the projected values as a <see cref="double"/>.</returns>
        public static double Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, short>> selector)
        {
            ArgumentNullException.ThrowIfNull(source);

            ArgumentNullException.ThrowIfNull(selector);
            return source.Provider.Execute<short>(Expression.Call(null, ((MethodInfo)MethodBase.GetMethodFromHandle(new RuntimeMethodHandle())!).MakeGenericMethod(new Type[] { typeof(TSource) }), new Expression[] { source.Expression, Expression.Quote(selector) }));
        }

        /// <summary>
        /// Computes the average of a sequence of values that are obtained by invoking a transform function on each
        /// element of the input sequence.
        /// </summary>
        /// <remarks>This method throws an InvalidOperationException if the source sequence contains no
        /// elements. Use this method to compute the average of values derived from each element in the sequence using
        /// the specified selector function.</remarks>
        /// <typeparam name="TSource">The type of the elements of the input sequence.</typeparam>
        /// <param name="source">The sequence of values to calculate the average of.</param>
        /// <param name="selector">A transform function to apply to each element of the source sequence to obtain the value to average.</param>
        /// <returns>The average of the projected values.</returns>
        public static double Average<TSource>(this IEnumerable<TSource> source, Func<TSource, short> selector)
        {
            return source.Select(selector).Average();
        }

        /// <summary>
        /// Computes the average of a sequence of 16-bit signed integers.
        /// </summary>
        /// <param name="source">A sequence of 16-bit signed integers to calculate the average of.</param>
        /// <returns>The average value of the sequence of values.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the source sequence contains no elements.</exception>
        public static double Average(this IEnumerable<short> source)
        {
            ArgumentNullException.ThrowIfNull(source);

            long num = 0L;
            long num2 = 0L;
            foreach (int num3 in source)
            {
                num += num3;
                num2 += 1L;
            }

            return num2 <= 0L ? throw new InvalidOperationException("Not enough elements") : (double)num / num2;
        }

        /// <summary>
        /// Computes the average of the sequence of nullable <see cref="short"/> values obtained by
        /// invoking the specified selector expression on each element of the input <see cref="IQueryable{TSource}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the input sequence.</typeparam>
        /// <param name="source">An <see cref="IQueryable{TSource}"/> whose elements to apply the selector to.</param>
        /// <param name="selector">An expression that projects each element to a nullable <see cref="short"/> value to average.</param>
        /// <returns>The average of the non-null projected values as a nullable <see cref="double"/>, or <c>null</c> if there are no non-null values.</returns>
        public static double? Average<TSource>(
            this IQueryable<TSource> source,
            Expression<Func<TSource, short?>> selector)
        {
            ArgumentNullException.ThrowIfNull(source);

            ArgumentNullException.ThrowIfNull(selector);
            return source.Provider.Execute<short?>(Expression.Call(null, ((MethodInfo)MethodBase.GetMethodFromHandle(new RuntimeMethodHandle())!).MakeGenericMethod(new Type[] { typeof(TSource) }), new Expression[] { source.Expression, Expression.Quote(selector) }));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static double? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, short?> selector)
        {
            return source.Select(selector).Average();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static double? Average(this IEnumerable<short?> source)
        {
            ArgumentNullException.ThrowIfNull(source);

            long num = 0L;
            long num2 = 0L;
            foreach (short? nullable in source)
            {
                if (nullable.HasValue)
                {
                    num += nullable.GetValueOrDefault();
                    num2 += 1L;
                }
            }

            return num2 > 0L ? new double?(num / ((double)num2)) : null;
        }

        #endregion

        #region Sum

        /// <summary>
        /// Computes the sum of the sequence of <see cref="short"/> values obtained by invoking
        /// the specified selector expression on each element of the input <see cref="IQueryable{TSource}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the input sequence.</typeparam>
        /// <param name="source">An <see cref="IQueryable{TSource}"/> whose elements to apply the selector to.</param>
        /// <param name="selector">An expression that projects each element to a <see cref="short"/> value to sum.</param>
        /// <returns>The sum of the projected values as a <see cref="short"/>.</returns>
        public static short Sum<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, short>> selector)
        {
            ArgumentNullException.ThrowIfNull(source);

            ArgumentNullException.ThrowIfNull(selector);
            return source.Provider.Execute<short>(Expression.Call(null, ((MethodInfo)MethodBase.GetMethodFromHandle(new RuntimeMethodHandle())!).MakeGenericMethod([typeof(TSource)]), [source.Expression, Expression.Quote(selector)]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static short Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, short> selector)
        {
            return source.Select(selector).Sum();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static short Sum(this IEnumerable<short> source)
        {
            ArgumentNullException.ThrowIfNull(source);

            short num = 0;
            foreach (short num2 in source)
            {
                num += num2;
            }

            return num;
        }

        /// <summary>
        /// Computes the sum of the sequence of nullable <see cref="short"/> values obtained by
        /// invoking the specified selector expression on each element of the input <see cref="IQueryable{TSource}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the input sequence.</typeparam>
        /// <param name="source">An <see cref="IQueryable{TSource}"/> whose elements to apply the selector to.</param>
        /// <param name="selector">An expression that projects each element to a nullable <see cref="short"/> value to sum.</param>
        /// <returns>The sum of the non-null projected values as a nullable <see cref="short"/>, or <c>null</c> if there are no non-null values.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static short? Sum<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, short?>> selector)
        {
            ArgumentNullException.ThrowIfNull(source);

            ArgumentNullException.ThrowIfNull(selector);
            return source.Provider.Execute<short?>(Expression.Call(null, ((MethodInfo)MethodBase.GetMethodFromHandle(new RuntimeMethodHandle())!).MakeGenericMethod([typeof(TSource)]), [source.Expression, Expression.Quote(selector)]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static short? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, short?> selector)
        {
            return source.Select(selector).Sum();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static short? Sum(this IEnumerable<short?> source)
        {
            ArgumentNullException.ThrowIfNull(source);

            short num = 0;
            foreach (short? nullable in source)
            {
                if (nullable.HasValue)
                {
                    num += nullable.GetValueOrDefault();
                }
            }

            return new short?(num);
        }

        #endregion

        #region Max

        /// <summary>
        /// Computes the maximum <see cref="short"/> value obtained by invoking the specified
        /// selector expression on each element of the input <see cref="IQueryable{TSource}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the input sequence.</typeparam>
        /// <param name="source">An <see cref="IQueryable{TSource}"/> whose elements to apply the selector to.</param>
        /// <param name="selector">An expression that projects each element to a <see cref="short"/> value.</param>
        /// <returns>The maximum projected value as a <see cref="short"/>.</returns>
        public static short Max<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, short>> selector)
        {
            ArgumentNullException.ThrowIfNull(source);

            ArgumentNullException.ThrowIfNull(selector);

            return source.Provider.Execute<short>(Expression.Call(null, ((MethodInfo)MethodBase.GetMethodFromHandle(new RuntimeMethodHandle())!).MakeGenericMethod([typeof(TSource)]), [source.Expression, Expression.Quote(selector)]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static short Max<TSource>(this IEnumerable<TSource> source, Func<TSource, short> selector)
        {
            return source.Select(selector).Max();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static short Max(this IEnumerable<short> source)
        {
            ArgumentNullException.ThrowIfNull(source);

            short num = 0;
            bool flag = false;
            foreach (short num2 in source)
            {
                if (flag)
                {
                    if (num2 > num)
                    {
                        num = num2;
                    }
                }
                else
                {
                    num = num2;
                    flag = true;
                }
            }

            return !flag ? throw new InvalidOperationException("Not enough elements") : num;
        }

        /// <summary>
        /// Computes the maximum nullable <see cref="short"/> value obtained by invoking the
        /// specified selector expression on each element of the input <see cref="IQueryable{TSource}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the input sequence.</typeparam>
        /// <param name="source">An <see cref="IQueryable{TSource}"/> whose elements to apply the selector to.</param>
        /// <param name="selector">An expression that projects each element to a nullable <see cref="short"/> value.</param>
        /// <returns>The maximum of the non-null projected values as a nullable <see cref="short"/>, or <c>null</c> if there are no non-null values.</returns>
        public static short? Max<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, short?>> selector)
        {
            ArgumentNullException.ThrowIfNull(source);

            ArgumentNullException.ThrowIfNull(selector);

            return source.Provider.Execute<short?>(Expression.Call(null, ((MethodInfo)MethodBase.GetMethodFromHandle(new RuntimeMethodHandle())!).MakeGenericMethod([typeof(TSource)]), [source.Expression, Expression.Quote(selector)]));

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static short? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, short?> selector)
        {
            return source.Select(selector).Max();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static short? Max(this IEnumerable<short?> source)
        {
            ArgumentNullException.ThrowIfNull(source);

            short? nullable = null;
            foreach (short? nullable2 in source)
            {
                if (nullable.HasValue)
                {
                    short? nullable3 = nullable2;
                    short? nullable4 = nullable;
                    if ((nullable3.GetValueOrDefault() <= nullable4.GetValueOrDefault()) ||
                        !(nullable3.HasValue & nullable4.HasValue))
                    {
                        continue;
                    }
                }

                nullable = nullable2;
            }

            return nullable;
        }

        #endregion

        #region Min

        /// <summary>
        /// Computes the minimum <see cref="short"/> value obtained by invoking the specified
        /// selector expression on each element of the input <see cref="IQueryable{TSource}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the input sequence.</typeparam>
        /// <param name="source">An <see cref="IQueryable{TSource}"/> whose elements to apply the selector to.</param>
        /// <param name="selector">An expression that projects each element to a <see cref="short"/> value.</param>
        /// <returns>The minimum projected value as a <see cref="short"/>.</returns>
        public static short Min<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, short>> selector)
        {
            ArgumentNullException.ThrowIfNull(source);

            ArgumentNullException.ThrowIfNull(selector);

            return source.Provider.Execute<short>(Expression.Call(null, ((MethodInfo)MethodBase.GetMethodFromHandle(new RuntimeMethodHandle())!).MakeGenericMethod([typeof(TSource)]), [source.Expression, Expression.Quote(selector)]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static short Min<TSource>(this IEnumerable<TSource> source, Func<TSource, short> selector)
        {
            return source.Select(selector).Min();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static short Min(this IEnumerable<short> source)
        {
            ArgumentNullException.ThrowIfNull(source);

            short num = 0;
            bool flag = false;
            foreach (short num2 in source)
            {
                if (flag)
                {
                    if (num2 < num)
                    {
                        num = num2;
                    }
                }
                else
                {
                    num = num2;
                    flag = true;
                }
            }

            return !flag ? throw new InvalidOperationException("Not enough elements") : num;
        }

        /// <summary>
        /// Computes the minimum nullable <see cref="short"/> value obtained by invoking the
        /// specified selector expression on each element of the input <see cref="IQueryable{TSource}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the input sequence.</typeparam>
        /// <param name="source">An <see cref="IQueryable{TSource}"/> whose elements to apply the selector to.</param>
        /// <param name="selector">An expression that projects each element to a nullable <see cref="short"/> value.</param>
        /// <returns>The minimum of the non-null projected values as a nullable <see cref="short"/>, or <c>null</c> if there are no non-null values.</returns>
        public static short? Min<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, short?>> selector)
        {
            ArgumentNullException.ThrowIfNull(source);

            ArgumentNullException.ThrowIfNull(selector);

            return source.Provider.Execute<short?>(Expression.Call(null, ((MethodInfo)MethodBase.GetMethodFromHandle(new RuntimeMethodHandle())!).MakeGenericMethod([typeof(TSource)]), [source.Expression, Expression.Quote(selector)]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static short? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, short?> selector)
        {
            return source.Select(selector).Min();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static short? Min(this IEnumerable<short?> source)
        {
            ArgumentNullException.ThrowIfNull(source);

            short? nullable = null;
            foreach (short? nullable2 in source)
            {
                if (nullable.HasValue)
                {
                    short? nullable3 = nullable2;
                    short? nullable4 = nullable;
                    if ((nullable3.GetValueOrDefault() >= nullable4.GetValueOrDefault()) ||
                        !(nullable3.HasValue & nullable4.HasValue))
                    {
                        continue;
                    }
                }

                nullable = nullable2;
            }

            return nullable;
        }

        #endregion

        #region Sorting

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <param name="propertyName"></param>
        /// <param name="GetFunc"></param>
        /// <returns></returns>
        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> entities, string propertyName, Func<string, object, object> GetFunc)
        {
            if ((entities != null && !entities.AsQueryable().Any()) || string.IsNullOrEmpty(propertyName))
            {
                return entities!;
            }

            if (GetFunc != null)
            {
                return entities!.OrderBy(e => GetFunc(propertyName, e!));
            }
            else
            {
                PropertyInfo? propertyInfo = entities?.First()?.GetType().GetProperty(propertyName);
                return entities!.OrderBy(e => propertyInfo?.GetValue(e, null));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <param name="propertyName"></param>
        /// <param name="GetFunc"></param>
        /// <returns></returns>
        public static IEnumerable<T> OrderByDescending<T>(this IEnumerable<T> entities, string propertyName, Func<string, object, object> GetFunc)
        {
            if ((entities != null && !entities.AsQueryable().Any()) || string.IsNullOrEmpty(propertyName))
            {
                return entities!;
            }

            if (GetFunc != null)
            {
                return entities!.OrderByDescending(e => GetFunc(propertyName, e!));
            }
            else
            {

                PropertyInfo? propertyInfo = entities?.First()?.GetType().GetProperty(propertyName);
                return entities!.OrderByDescending(e => propertyInfo?.GetValue(e, null));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <param name="propertyName"></param>
        /// <param name="GetFunc"></param>
        /// <returns></returns>
        public static IEnumerable<T> ThenBy<T>(this IOrderedEnumerable<T> entities, string propertyName, Func<string, object, object> GetFunc)
        {
            if ((entities != null && !entities.AsQueryable().Any()) || string.IsNullOrEmpty(propertyName))

            {
                return entities!;
            }

            if (GetFunc != null)
            {
                return entities!.ThenBy(e => GetFunc(propertyName, e!));
            }
            else
            {

                PropertyInfo? propertyInfo = entities?.First()?.GetType().GetProperty(propertyName);
                return entities!.ThenBy(e => propertyInfo?.GetValue(e, null));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <param name="propertyName"></param>
        /// <param name="GetFunc"></param>
        /// <returns></returns>
        public static IEnumerable<T> ThenByDescending<T>(this IOrderedEnumerable<T> entities, string propertyName, Func<string, object, object> GetFunc)
        {

            if ((entities != null && !entities.AsQueryable().Any()) || string.IsNullOrEmpty(propertyName))
            {
                return entities!;
            }

            if (GetFunc != null)
            {
                return entities!.ThenByDescending(e => GetFunc(propertyName, e!));
            }
            else
            {
                PropertyInfo? propertyInfo = entities?.First()?.GetType().GetProperty(propertyName);
                return entities!.ThenByDescending(e => propertyInfo?.GetValue(e, null));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <param name="propertyName"></param>
        /// <param name="GetFunc"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> entities, string propertyName, Func<string, object, object> GetFunc, IComparer<object> comparer)
        {

            if ((entities != null && !entities.AsQueryable().Any()) || string.IsNullOrEmpty(propertyName))
            {
                return entities!;
            }

            if (GetFunc != null)
            {
                return entities!.OrderBy(e => GetFunc(propertyName, e!), comparer);
            }
            else
            {
                PropertyInfo? propertyInfo = entities?.First()?.GetType().GetProperty(propertyName);
                return entities!.OrderBy(e => propertyInfo?.GetValue(e, null), comparer!);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <param name="propertyName"></param>
        /// <param name="GetFunc"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static IEnumerable<T> OrderByDescending<T>(this IEnumerable<T> entities, string propertyName, Func<string, object, object> GetFunc, IComparer<object> comparer)
        {

            if ((entities != null && !entities.AsQueryable().Any()) || string.IsNullOrEmpty(propertyName))
            {
                return entities!;
            }

            if (GetFunc != null)
            {
                return entities!.OrderByDescending(e => GetFunc(propertyName, e!), comparer);
            }
            else
            {
                PropertyInfo? propertyInfo = entities?.First()?.GetType().GetProperty(propertyName);
                return entities!.OrderByDescending(e => propertyInfo?.GetValue(e, null), comparer!);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <param name="propertyName"></param>
        /// <param name="GetFunc"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static IEnumerable<T> ThenBy<T>(this IOrderedEnumerable<T> entities, string propertyName, Func<string, object, object> GetFunc, IComparer<object> comparer)
        {

            if ((entities != null && !entities.AsQueryable().Any()) || string.IsNullOrEmpty(propertyName))
            {
                return entities!;
            }

            if (GetFunc != null)
            {
                return entities!.ThenBy(e => GetFunc(propertyName, e!), comparer);
            }
            else
            {
                PropertyInfo? propertyInfo = entities?.First()?.GetType().GetProperty(propertyName);
                return entities!.ThenBy(e => propertyInfo?.GetValue(e, null), comparer!);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <param name="propertyName"></param>
        /// <param name="GetFunc"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static IEnumerable<T> ThenByDescending<T>(this IOrderedEnumerable<T> entities, string propertyName, Func<string, object, object> GetFunc, IComparer<object> comparer)
        {
            if ((entities != null && !entities.AsQueryable().Any()) || string.IsNullOrEmpty(propertyName))
            {
                return entities!;
            }

            if (GetFunc != null)
            {
                return entities!.ThenByDescending(e => GetFunc(propertyName, e!), comparer);
            }
            else
            {
                PropertyInfo? propertyInfo = entities?.First()?.GetType().GetProperty(propertyName);
                return entities!.ThenByDescending(e => propertyInfo?.GetValue(e, null), comparer!);
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static ParallelQuery<T> GetParallelQueryFor<T>(IEnumerable source)
        {
            IEnumerable<T>? enumerable = source as IEnumerable<T>;
            return enumerable!.AsParallel();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public static ParallelQuery GetParallelQuery(this IEnumerable source, Type? sourceType = null)
        {
            if (sourceType == null)
            {
                sourceType = source.GetElementType();
            }

            MethodInfo? genericWrapper = typeof(EnumerableExtensions).GetMethods().FirstOrDefault(m => m.Name == "GetParallelQueryFor" && m.IsStatic && m.IsGenericMethod);
            genericWrapper = genericWrapper?.MakeGenericMethod(sourceType);
            ParallelQuery parallelQuery = (ParallelQuery)genericWrapper?.Invoke(null, [source])!;
            return parallelQuery;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Type GetElementType(this IEnumerable source)
        {
            return GetElementTypeByRepresentativeItem(source, false);
        }

        internal static Type GetElementTypeByRepresentativeItem(this IEnumerable source, bool useRepresentativeItem)
        {
            IEnumerable list = source;

            // var prop = list.GetType().GetProperty("Item");
            PropertyInfo prop = list.GetItemPropertyInfo();
            return prop != null ? prop.PropertyType : GetItemType(source, useRepresentativeItem);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static PropertyInfo GetItemPropertyInfo(this IEnumerable list)
        {
            IEnumerable<PropertyInfo>? prop = list?.GetType().GetProperties().Where(p => p.Name.Equals("Item", StringComparison.Ordinal));
            return prop?.Count() > 1
                ? prop?.FirstOrDefault(p =>
                {
                    ParameterInfo[] para = p?.GetGetMethod()?.GetParameters()!;
                    return para.Length != 0 && para[0].ParameterType == typeof(int);
                })!
                : list?.GetType().GetProperty("Item")!;
        }

        internal static Type GetElementType(this IEnumerable source, ref bool isEmpty, object? firstItem = null)
        {
            firstItem ??= GetRepresentativeItem(source);

            isEmpty = firstItem == null;

            if (isEmpty)
            {
                return GetElementTypeByRepresentativeItem(source, true);
            }

            Type castType = GetGenericSourceType(source);
            if (castType != null)
            {
                return castType;
            }

            Type? firstItemType = firstItem?.GetType();
            return firstItemType != null && !string.IsNullOrEmpty(firstItemType.AssemblyQualifiedName) &&
                firstItemType.AssemblyQualifiedName.Contains("System.Data.Entity.DynamicProxies", StringComparison.Ordinal)
                ? GetElementType(source)
                : firstItemType!;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Type GetGenericSourceType(IEnumerable source)
        {
            Type? type = source?.GetType();
            return GetBaseGenericInterfaceType(type!, false);
        }

        private static Type GetBaseGenericInterfaceType(Type type, bool canreturn)
        {

            if (type.GetTypeInfo().IsGenericType)
            {
                Type[] genericArguments = type.GetGenericArguments();
                if (genericArguments.Length == 1)
                {
                    if (genericArguments[0].GetTypeInfo().IsInterface || genericArguments[0].GetTypeInfo().IsAbstract)
                    {
                        return genericArguments[0];
                    }

                    if (canreturn)
                    {
                        return genericArguments[0];
                    }
                }
            }
            else if (type.GetTypeInfo().BaseType != null)
            {
                return GetBaseGenericInterfaceType(type.GetTypeInfo().BaseType!, canreturn);
            }

            return null!;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="useRepresentativeItem"></param>
        /// <returns></returns>
        public static Type GetItemType(this IEnumerable source, bool useRepresentativeItem)
        {
            Type? type = source?.GetType();
            if (type != null && type.GetTypeInfo().IsGenericType)
            {
                Type generictype = GetBaseGenericInterfaceType(type, true);
                if (generictype == null || generictype.GetTypeInfo().IsInterface || generictype.GetTypeInfo().IsAbstract)
                {
                    if (useRepresentativeItem)
                    {
                        object representativeItem = GetRepresentativeItem(source!);
                        if (representativeItem != null)
                        {
                            return representativeItem.GetType();
                        }
                    }
                }

                return generictype!;
            }
            else if (useRepresentativeItem)
            {
                object representativeItem = GetRepresentativeItem(source!);
                if (representativeItem != null)
                {
                    return representativeItem.GetType();
                }
                else if (type?.GetTypeInfo().BaseType != null && type.GetTypeInfo().BaseType!.GetTypeInfo().IsGenericType)
                {
                    return type?.GetTypeInfo()?.BaseType?.GetGenericArguments()[0]!;
                }
            }

            return null!;
        }

        private static object GetRepresentativeItem(IEnumerable source)
        {
            IEnumerator enumerator = source.GetEnumerator();
            return enumerator.MoveNext() ? enumerator.Current : null!;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int IndexOf<T>(this IEnumerable<T> source, T value)
        {
            ArgumentNullException.ThrowIfNull(source);
            int index = 0;
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            foreach (T item in source)
            {
                if (comparer.Equals(item, value))
                {
                    return index;
                }

                index++;
            }

            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Like(this string source, string value)
        {
            if (!string.IsNullOrEmpty(source))
            {
                if (!string.IsNullOrEmpty(value) && value.Contains('%', StringComparison.Ordinal))
                {
                    if (value.StartsWith('%') && value.LastIndexOf('%') < 2)
                    {
                        value = value[1..];
                        return source.StartsWith(value, StringComparison.CurrentCulture);
                    }

                    else if (value.EndsWith('%') && value.IndexOf('%', StringComparison.Ordinal) > value.Length - 3)
                    {
                        value = value[..^1];
                        return source.EndsWith(value, StringComparison.CurrentCulture);
                    }

                    else if (value.LastIndexOf('%') != value.IndexOf('%', StringComparison.Ordinal) && value.LastIndexOf('%') > value.IndexOf('%', StringComparison.Ordinal) + 1)
                    {
                        value = value[(value.IndexOf('%', StringComparison.Ordinal) + 1)..value.LastIndexOf('%')];
                        return source.Contains(value, StringComparison.CurrentCulture);
                    }
                    else
                    {
                        return source.Contains(value, StringComparison.CurrentCulture);
                    }
                }
            }

            return (bool)source?.Contains(value, StringComparison.CurrentCulture)!;
        }
    }
}
