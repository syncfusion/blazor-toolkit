using System.Dynamic;

namespace Syncfusion.Blazor.Toolkit.Data
{
    /// <summary>
    /// Provides the members for data adaptor class.
    /// </summary>
    /// <exclude/>
    public interface IAdaptor
    {
        /// <summary>
        /// Returns the adaptor name.
        /// </summary>
        /// <returns>string.</returns>
        string GetName();

        /// <summary>
        /// Runs the data operation synchronously.
        /// </summary>
        /// <param name="runSync">Enables synchronous data operation.</param>
        void SetRunSyncOnce(bool runSync);

        /// <summary>
        /// Read query from <see cref="Query"/> and make it understandable by
        /// data source.
        /// </summary>
        /// <param name="queries">Specifies the query.</param>
        /// <returns>object.</returns>
        object ProcessQuery(DataManagerRequest queries);

        /// <summary>
        /// Process the data operation response/result from the data source and make it understandable by user end.
        /// </summary>
        /// <typeparam name="T">Specifies the data source model type.</typeparam>
        /// <param name="data">Specifies the data manager instance.</param>
        /// <param name="queries">Specifies the query.</param>
        /// <returns>Task.</returns>
        Task<object> ProcessResponse<T>(object data, DataManagerRequest queries);


        /// <summary>
        /// Performs data operation. If its a remote data source then make a server request.
        /// </summary>
        /// <typeparam name="T">Specifies the data source model type.</typeparam>
        /// <param name="queries">Specifies the query.</param>
        /// <returns>Task.</returns>
        Task<object> PerformDataOperation<T>(object queries);

        /// <summary>
        /// Returns true if data source is remote service.
        /// </summary>
        /// <returns>bool.</returns>
        bool IsRemote();

        /// <summary>
        /// To get model type.
        /// </summary>
        void SetModelType(Type type);

        /// <summary>
        /// To get model type.
        /// </summary>
        Type GetModelType();

        /// <summary>
        /// Adds additional paramerters from Query instance to server request.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="queries"></param>
        void AddParams(RequestOptions options, DataManagerRequest queries);

        /// <summary>
        /// Invoked before sending server request.
        /// </summary>
        /// <param name="request">Specifies the HttpRequestMessage instance.</param>
        void BeforeSend(HttpRequestMessage request);

        /// <summary>
        /// Process the data operation batch response/result from the data source and make it understandable by user end.
        /// </summary>
        /// <typeparam name="T">Specifies the data source model type.</typeparam>
        /// <param name="data">Specifies the data.</param>
        /// <param name="queries">Specifies the query.</param>
        /// <returns>Task.</returns>
        Task<object> ProcessBatchResponse<T>(object data, DataManagerRequest queries);
    }

    /// <summary>
    /// Base class for all data adaptors.
    /// </summary>
    public class AdaptorBase(DataManager dataManager) : IAdaptor
    {
        /// <summary>
        /// Specifies the data manager instance.
        /// </summary>
        public DataManager DataManager { get; set; } = dataManager;

        /// <summary>
        /// When true, runs data operation synchronously. Applicable only for BlazorAdaptor.
        /// </summary>
        public bool RunSyncOnce { get; set; }
        /// <summary>
        /// Runs the data operation synchronously.
        /// </summary>
        /// <param name="runSync">Enables synchronous data operation.</param>
        public virtual void SetRunSyncOnce(bool runSync)
        {
            RunSyncOnce = false;
        }

        /// <summary>
        /// Returns the adaptor name.
        /// </summary>
        /// <returns>string.</returns>
        public virtual string GetName()
        {
            return nameof(AdaptorBase);
        }

        /// <summary>
        /// Returns true if data source is remote service.
        /// </summary>
        /// <returns>bool.</returns>
        public virtual bool IsRemote()
        {
            return false;
        }

        /// <summary>
        /// To get model type.
        /// </summary>
        public virtual void SetModelType(Type type)
        {
        }

        /// <summary>
        /// To get model type.
        /// </summary>
        public virtual Type GetModelType()
        {
            return default!;
        }

        /// <summary>
        /// Read query from <see cref="Query"/> and make it understandable by
        /// data source.
        /// </summary>
        /// <param name="queries">Specifies the query.</param>
        /// <returns>object.</returns>
        public virtual object ProcessQuery(DataManagerRequest queries)
        {
            return default!;
        }

        /// <summary>
        /// Performs data operation. If its a remote data source then make a server request.
        /// </summary>
        /// <typeparam name="T">Specifies the data source model type.</typeparam>
        /// <param name="queries">Specifies the query.</param>
        /// <returns>Task.</returns>
        public virtual async Task<object> PerformDataOperation<T>(object queries)
        {
            return await Task.FromResult<object>(null!).ConfigureAwait(false);
        }

        /// <summary>
        /// Process the data operation response/result from the data source and make it understandable by user end.
        /// </summary>
        /// <typeparam name="T">Specifies the data source model type.</typeparam>
        /// <param name="data">Specifies the data manager instance.</param>
        /// <param name="queries">Specifies the query.</param>
        /// <returns>Task.</returns>
        public virtual async Task<object> ProcessResponse<T>(object data, DataManagerRequest queries)
        {
            return await Task.FromResult(data).ConfigureAwait(false);
        }
                
        /// <summary>
        /// Adds additional paramerters from Query instance to server request.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="queries"></param>
        public virtual void AddParams(RequestOptions options, DataManagerRequest queries)
        {
        }

        /// <summary>
        /// Invoked before sending server request.
        /// </summary>
        /// <param name="request">Specifies the HttpRequestMessage instance.</param>
        public virtual void BeforeSend(HttpRequestMessage request)
        {
        }

        /// <summary>
        /// Process the data operation batch response/result from the data source and make it understandable by user end.
        /// </summary>
        /// <typeparam name="T">Specifies the data source model type.</typeparam>
        /// <param name="data">Specifies the data.</param>
        /// <param name="queries">Specifies the query.</param>
        /// <returns>Task.</returns>
        public virtual async Task<object> ProcessBatchResponse<T>(object data, DataManagerRequest queries)
        {
            return await Task.FromResult(data).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Defines internal adaptor options.
    /// </summary>
    /// <exclude/>
    public struct RemoteOptions : IEquatable<RemoteOptions>
    {
        /// <summary>
        /// Compares the specified instance and the current instance of RemoteOptions
        ///     for value equality.
        /// </summary>
        /// <param name="obj">The instance to compare.</param>
        /// <returns>true.</returns>
        public override readonly bool Equals(object obj)
        {
            return true;
        }
        /// <summary>
        /// Compares the specified instance and the current instance of RemoteOptions
        ///     for value equality.
        /// </summary>
        /// <param name="other">The instance to compare.</param>
        /// <returns>true.</returns>
        public readonly bool Equals(RemoteOptions other)
        {
            return true;
        }

        /// <summary>
        /// Returns the hash code.
        /// </summary>
        /// <returns>int.</returns>
        public override readonly int GetHashCode()
        {
            return base.GetHashCode();
        }

    }

    /// <summary>
    /// Defines the members of the CRUD arguments send during server request. Use this class to model
    /// bind request parameters while using UrlAdaptor.
    /// </summary>
    /// <typeparam name="T">Type of the data.</typeparam>
    public class CRUDModel<T>
    {
        /// <summary>
        /// Initializes a new instance of the CRUDModel class.
        /// </summary>
        public CRUDModel()
        {
        }

        /// <summary>
        /// Specifies the list of added records while batch editing.
        /// </summary>
        /// <remarks>The Added property will holds values on batch editing only.</remarks>
        public List<T> Added { get; set; }

        /// <summary>
        /// Specifies the list of updated records while batch editing.
        /// </summary>
        /// <remarks>The Changed property will holds values on batch editing only.</remarks>
        public List<T> Changed { get; set; }

        /// <summary>
        /// Specifies the list of deleted records while batch editing.
        /// </summary>
        /// <remarks>The Deleted property will holds values on batch editing only.</remarks>
        public List<T> Deleted { get; set; }
    }

    /// <summary>
    /// Class holds URL and Key for batch operation.
    /// </summary>
    public class Utils
    {
        /// <summary>
        /// Specifies the batch url.
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// Specifies the batch primary key value.
        /// </summary>
        public string? Key { get; set; }
    }

    /// <summary>
    /// Defines members of the request option for remote data handling.
    /// </summary>
    public class RequestOptions
    {
        /// <summary>
        /// Specifies the service url.
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// Specifies the application base url.
        /// </summary>
        public string? BaseUrl { get; set; }

        /// <summary>
        /// Specifies the Http request method.
        /// </summary>
        public HttpMethod? RequestMethod { get; set; }

        /// <summary>
        /// Specifies the data to be posted.
        /// </summary>
        public object? Data { get; set; }

        /// <summary>
        /// Specifies the source query value.
        /// </summary>
        public Query? Queries { get; set; }

        /// <summary>
        /// Specifies the content type. By default, application/json is used.
        /// </summary>
        public string ContentType { get; set; } = "application/json";

        internal object? Original { get; set; }

        internal string? CSet { get; set; }

        internal string? KeyField { get; set; }

        internal string? Accept { get; set; }

        internal HttpMethod? UpdateType { get; set; }
    }
}
