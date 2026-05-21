using System.Collections;

namespace Syncfusion.Blazor.Toolkit.Data
{
    /// <summary>
    /// DataOperation class that performs data operation in IEnumerable and IQueryable type data sources.
    /// </summary>
    public static class DataOperations
    {
        /// <summary>
        /// Executes the query against the given data source and returns the resultant records.
        /// </summary>
        /// <param name="dataSource">Input data source against which the query to be executed.</param>
        /// <param name="query">Query to be executed.</param>
        /// <returns>IEnumerable - resultant records.</returns>
        public static IEnumerable Execute(IEnumerable dataSource, DataManagerRequest query)
        {
            return EnumerableOperation.Execute(dataSource, query);
        }

        /// <summary>
        /// Sorts the data source using the given sort descriptor and returns the sorted records.
        /// </summary>
        /// <param name="dataSource">Data source to be sorted.</param>
        /// <param name="sortedColumns">List of sort criteria.</param>
        /// <returns>IEnumerable - sorted records.</returns>
        public static IEnumerable PerformSorting(IEnumerable dataSource, List<SortedColumn> sortedColumns)
        {
            return EnumerableOperation.PerformSorting(dataSource, sortedColumns);
        }

        /// <summary>
        /// Sorts the data source using the given sort descriptor and returns the sorted records.
        /// </summary>
        /// <param name="dataSource">Data source to be sorted.</param>
        /// <param name="sortedColumns">List of sort criteria.</param>
        /// <returns>IEnumerable - sorted records.</returns>
        public static IEnumerable PerformSorting(IEnumerable dataSource, List<Sort> sortedColumns)
        {
            return EnumerableOperation.PerformSorting(dataSource, sortedColumns);
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
            return EnumerableOperation.PerformFiltering(dataSource, whereFilter, condition);
        }

        /// <summary>
        /// Apply the given search criteria against the data source and returns the filtered records.
        /// </summary>
        /// <param name="dataSource">Data source to be filtered.</param>
        /// <param name="searchFilter">List of search criteria.</param>
        /// <returns>IEnumerable - searched records.</returns>
        public static IEnumerable PerformSearching(IEnumerable dataSource, List<SearchFilter> searchFilter)
        {
            return EnumerableOperation.PerformSearching(dataSource, searchFilter);
        }

        /// <summary>
        /// Skip the given number of records from data source and returns the resultant records.
        /// </summary>
        /// <param name="dataSource">Input data source.</param>
        /// <param name="skip">Number of records to be skipped.</param>
        /// <returns>IEnumerable.</returns>
        public static IEnumerable PerformSkip(IEnumerable dataSource, int skip)
        {
            return EnumerableOperation.PerformSkip(dataSource, skip);
        }

        /// <summary>
        /// Take the given number of records from data source.
        /// </summary>
        /// <param name="dataSource">Input data source.</param>
        /// <param name="take">Number of records to be taken.</param>
        /// <returns>IEnumerable.</returns>
        public static IEnumerable PerformTake(IEnumerable dataSource, int take)
        {
            return EnumerableOperation.PerformTake(dataSource, take);
        }

        /// <summary>
        /// Groups data source by the given list of column names.
        /// </summary>
        /// <param name="dataSource">Input data source to be grouped.</param>
        /// <param name="grouped">List of column names by which rows will be grouped.</param>
        /// <returns>IEnumerable.</returns>
        public static IEnumerable PerformGrouping(IEnumerable dataSource, List<string> grouped)
        {
            return EnumerableOperation.PerformGrouping(dataSource, grouped);
        }

        /// <summary>
        /// Executes the query against the given data source and returns the resultant records.
        /// </summary>
        /// <param name="dataSource">Input data source against which the query to be executed.</param>
        /// <param name="query">Query to be executed.</param>
        /// <returns>IEnumerable - resultant records.</returns>
        /// <typeparam name="T">Type of the data source elements.</typeparam>
        public static IEnumerable<T> Execute<T>(IEnumerable<T> dataSource, DataManagerRequest query)
        {
            return QueryableOperation.Execute(dataSource.AsQueryable(), query);
        }

        /// <summary>
        /// Skip the given number of records from data source and returns the resultant records.
        /// </summary>
        /// <param name="dataSource">Input data source.</param>
        /// <param name="skip">Number of records to be skipped.</param>
        /// <typeparam name="T">Type of the data source elements.</typeparam>
        /// <returns>IEnumerable.</returns>
        public static IEnumerable<T> PerformSkip<T>(IEnumerable<T> dataSource, int skip)
        {
            return QueryableOperation.PerformSkip(dataSource.AsQueryable(), skip);
        }

        /// <summary>
        /// Take the given number of records from data source.
        /// </summary>
        /// <param name="dataSource">Input data source.</param>
        /// <param name="take">Number of records to be taken.</param>
        /// <typeparam name="T">Type of the data source elements.</typeparam>
        /// <returns>IEnumerable.</returns>
        public static IEnumerable<T> PerformTake<T>(IEnumerable<T> dataSource, int take)
        {
            return QueryableOperation.PerformTake(dataSource.AsQueryable(), take);
        }

        /// <summary>
        /// Groups data source by the given list of column names.
        /// </summary>
        /// <param name="dataSource">Input data source to be grouped.</param>
        /// <param name="grouped">List of column names by which rows will be grouped.</param>
        /// <typeparam name="T">Type of the data source elements.</typeparam>
        /// <returns>IEnumerable.</returns>
        public static IEnumerable PerformGrouping<T>(IEnumerable<T> dataSource, List<string> grouped)
        {
            return QueryableOperation.PerformGrouping(dataSource.AsQueryable(), grouped);
        }

        /// <summary>
        /// Sorts the data source using the given sort descriptor and returns the sorted records.
        /// </summary>
        /// <param name="dataSource">Data source to be sorted.</param>
        /// <param name="sortedColumns">List of sort criteria.</param>
        /// <typeparam name="T">Type of the data source elements.</typeparam>
        /// <returns>IEnumerable - sorted records.</returns>
        public static IEnumerable<T> PerformSorting<T>(IEnumerable<T> dataSource, List<SortedColumn> sortedColumns)
        {
            return QueryableOperation.PerformSorting(dataSource.AsQueryable(), sortedColumns);
        }

        /// <summary>
        /// Sorts the data source using the given sort descriptor and returns the sorted records.
        /// </summary>
        /// <param name="dataSource">Data source to be sorted.</param>
        /// <param name="sortedColumns">List of sort criteria.</param>
        /// <typeparam name="T">Type of the data source elements.</typeparam>
        /// <returns>IEnumerable - sorted records.</returns>
        public static IEnumerable<T> PerformSorting<T>(IEnumerable<T> dataSource, List<Sort> sortedColumns)
        {
            return QueryableOperation.PerformSorting(dataSource.AsQueryable(), sortedColumns);
        }

        /// <summary>
        /// Selected the given field names alone from the data source.
        /// </summary>
        /// <param name="dataSource">Input data source.</param>
        /// <param name="select">List of fields to select.</param>
        /// <returns>IEnumerale.</returns>
        public static IEnumerable PerformSelect(IEnumerable dataSource, List<string> select)
        {
            return QueryableOperation.PerformSelect(dataSource.AsQueryable(), select);
        }

        /// <summary>
        /// Apply the given search criteria against the data source and returns the filtered records.
        /// </summary>
        /// <param name="dataSource">Data source to be filtered.</param>
        /// <param name="searchFilter">List of search criteria.</param>
        /// <returns>IEnumerable - searched records.</returns>
        /// <typeparam name="T">Type of the data source elements.</typeparam>
        public static IEnumerable<T> PerformSearching<T>(IEnumerable<T> dataSource, List<SearchFilter> searchFilter)
        {
            return QueryableOperation.PerformSearching(dataSource.AsQueryable(), searchFilter);
        }

        /// <summary>
        /// Apply the given filter criteria against the data source and returns the filtered records.
        /// </summary>
        /// <param name="dataSource">Data source to be filtered.</param>
        /// <param name="whereFilter">List of filter criteria.</param>
        /// <param name="condition">Filter merge condition. Value can be either AND or OR.</param>
        /// <returns>IEnumerable - filtered records.</returns>
        /// <typeparam name="T">Type of the data source elements.</typeparam>
        public static IEnumerable<T> PerformFiltering<T>(IEnumerable<T> dataSource, List<WhereFilter> whereFilter, string condition)
        {
            return QueryableOperation.PerformFiltering(dataSource.AsQueryable(), whereFilter, condition);
        }

        /// <summary>
        /// Executes the query against the given data source and returns the resultant records.
        /// </summary>
        /// <param name="dataSource">Input data source against which the query to be executed.</param>
        /// <param name="query">Query to be executed.</param>
        /// <returns>IQueryable - resultant records.</returns>
        /// <typeparam name="T">Type of the data source elements.</typeparam>
        public static IQueryable<T> Execute<T>(IQueryable<T> dataSource, DataManagerRequest query)
        {
            return QueryableOperation.Execute(dataSource, query);
        }

        /// <summary>
        /// Groups data source by the given list of column names.
        /// </summary>
        /// <param name="dataSource">Input data source to be grouped.</param>
        /// <param name="grouped">List of column names by which rows will be grouped.</param>
        /// <typeparam name="T">Type of the data source elements.</typeparam>
        /// <returns>IQueryable.</returns>
        public static IQueryable PerformGrouping<T>(IQueryable<T> dataSource, List<string> grouped)
        {
            return QueryableOperation.PerformGrouping(dataSource, grouped);
        }

        /// <summary>
        /// Sorts the data source using the given sort descriptor and returns the sorted records.
        /// </summary>
        /// <param name="dataSource">Data source to be sorted.</param>
        /// <param name="sortedColumns">List of sort criteria.</param>
        /// <typeparam name="T">Type of the data source elements.</typeparam>
        /// <returns>IQueryable - sorted records.</returns>
        public static IQueryable<T> PerformSorting<T>(IQueryable<T> dataSource, List<SortedColumn> sortedColumns)
        {
            return QueryableOperation.PerformSorting(dataSource, sortedColumns);
        }

        /// <summary>
        /// Sorts the data source using the given sort descriptor and returns the sorted records.
        /// </summary>
        /// <param name="dataSource">Data source to be sorted.</param>
        /// <param name="sortedColumns">List of sort criteria.</param>
        /// <typeparam name="T">Type of the data source elements.</typeparam>
        /// <returns>IQueryable - sorted records.</returns>
        public static IQueryable<T> PerformSorting<T>(IQueryable<T> dataSource, List<Sort> sortedColumns)
        {
            return QueryableOperation.PerformSorting(dataSource, sortedColumns);
        }

        /// <summary>
        /// Skip the given number of records from data source and returns the resultant records.
        /// </summary>
        /// <param name="dataSource">Input data source.</param>
        /// <param name="skip">Number of records to be skipped.</param>
        /// <typeparam name="T">Type of the data source elements.</typeparam>
        /// <returns>IQueryable.</returns>
        public static IQueryable<T> PerformSkip<T>(IQueryable<T> dataSource, int skip)
        {
            return QueryableOperation.PerformSkip(dataSource, skip);
        }

        /// <summary>
        /// Take the given number of records from data source.
        /// </summary>
        /// <param name="dataSource">Input data source.</param>
        /// <param name="take">Number of records to be taken.</param>
        /// <typeparam name="T">Type of the data source elements.</typeparam>
        /// <returns>IEnumerable.</returns>
        public static IQueryable<T> PerformTake<T>(IQueryable<T> dataSource, int take)
        {
            return QueryableOperation.PerformTake(dataSource, take);
        }

        /// <summary>
        /// Apply the given search criteria against the data source and returns the filtered records.
        /// </summary>
        /// <param name="dataSource">Data source to be filtered.</param>
        /// <param name="searchFilter">List of search criteria.</param>
        /// <returns>IQueryable - searched records.</returns>
        /// <typeparam name="T">Type of the data source elements.</typeparam>
        public static IQueryable<T> PerformSearching<T>(IQueryable<T> dataSource, List<SearchFilter> searchFilter)
        {
            return QueryableOperation.PerformSearching(dataSource, searchFilter);
        }

        /// <summary>
        /// Apply the given filter criteria against the data source and returns the filtered records.
        /// </summary>
        /// <param name="dataSource">Data source to be filtered.</param>
        /// <param name="whereFilter">List of filter criteria.</param>
        /// <param name="condition">Filter merge condition. Value can be either AND or OR.</param>
        /// <returns>IQueryable - filtered records.</returns>
        /// <typeparam name="T">Type of the data source elements.</typeparam>
        public static IQueryable<T> PerformFiltering<T>(IQueryable<T> dataSource, List<WhereFilter> whereFilter, string condition)
        {
            return QueryableOperation.PerformFiltering(dataSource, whereFilter, condition);
        }

        /// <summary>
        /// Selected the given field names alone from the data source.
        /// </summary>
        /// <param name="dataSource">Input data source.</param>
        /// <param name="select">List of fields to select.</param>
        /// <returns>IQueryable.</returns>
        /// <typeparam name="T">Type of the data source elements.</typeparam>
        public static IQueryable PerformSelect<T>(IQueryable<T> dataSource, List<string> select)
        {
            return QueryableOperation.PerformSelect<T>(dataSource, select);
        }
    }
}
