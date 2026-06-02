using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.JSInterop;
using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using System.Reflection;
using System.Collections;
using System.Security.Cryptography;
using System.Collections.Concurrent;

namespace Syncfusion.Blazor.Toolkit.Data
{
    /// <summary>
    /// The DataManager is a data management component used for performing data operations in applications.
    /// It acts as an abstraction for using local data source - IEnumerable and remote data source - web services returning JSON or oData.
    /// </summary>
    public class DataManager : SfBaseComponent
    {
        /// <summary>
        /// JavaScript runtime for invoking JS interop from this component.
        /// Use <see cref="JsRuntime"/> to call browser APIs or helper scripts required by the DataManager.
        /// </summary>
        /// <exclude />
        [Inject]
        protected IJSRuntime JsRuntime { get; set; }

        /// <summary>
        /// HTTP client for remote data requests.
        /// </summary>
        /// <exclude />
        [Inject]
        internal HttpClient HttpClient { get; set; }

        /// <exclude />
        [JsonIgnore]
        [Inject]
        public IServiceProvider ServiceProvider { get; set; }

        /// <exclude />
        [JsonIgnore]
        public BaseAdaptor BaseAdaptor { get; set; }

        /// <summary>
        /// Specifies the HttpClient instance to be used  by DataManager.
        /// </summary>
        /// <value>
        /// An <see cref="HttpClient"/> instance. When not specified, the injected HttpClient is used.
        /// </value>
        /// <remarks>Use HttpClientInstance property to inject named HttpClient into DataManager.</remarks>
        [Parameter]
        [JsonIgnore]
        public HttpClient HttpClientInstance { get; set; }

        /// <summary>
        /// Specifies the endpoint URL. DataManager requests this URL when data is needed.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the URL endpoint. Defaults to an empty string.
        /// </value>
        [Parameter]
        [DefaultValue("")]
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Specifies the custom adaptor class type. DataManager uses this type value to instantiate custom adaptor class.
        /// </summary>
        /// <value>
        /// A <see cref="Type"/> that derives from <see cref="DataAdaptor"/>. When not specified, the default <see cref="BlazorAdaptor"/> is used.
        /// </value>
        /// <remarks>
        /// DataManager uses the <c>Activator.CreateInstance</c> method to create the custom adaptor instance.
        /// Injecting services into a custom adaptor class is not supported when using this approach.
        /// To inject and use services, provide a custom adaptor as a Blazor component by extending the <see cref="DataAdaptor"/> class.
        /// </remarks>
        [Parameter]
        [JsonIgnore]
        [JsonPropertyName("adaptorInstance")]
        public Type AdaptorInstance { get; set; }

        /// <summary>
        /// Gets or Sets the properties to be specified for GraphQLAdaptor.
        /// </summary>
        [Parameter]
        public GraphQLAdaptorOptions GraphQLAdaptorOptions { get; set; }

        /// <summary>
        /// Gets or sets the active adaptor instance used by the DataManager to perform data operations.
        /// </summary>
        /// <value>
        /// An <see cref="IAdaptor"/> implementation that handles data operations such as reading, inserting, updating, and removing data.
        /// This may be a local or remote implementation. When set, DataManager delegates CRUD operations to this instance.
        /// </value>
        /// <remarks>
        /// The adaptor encapsulates the behavior for data operations. Set this property to use a custom adaptor,
        /// such as <see cref="BlazorAdaptor"/> for local data or a remote service adaptor for server-side operations.
        /// </remarks>
        [Parameter]
        [JsonIgnore]
        [JsonPropertyName("dataAdaptor")]
        public IAdaptor DataAdaptor { get; set; }

        /// <summary>
        /// Reference to a .NET object that can be passed to JavaScript for callback invocation.
        /// </summary>
        /// <value>
        /// A <see cref="DotNetObjectReference{T}"/> instance. This reference is created when DataManager exposes .NET methods to JavaScript
        /// and is disposed during component cleanup to avoid memory leaks. Defaults to <c>null</c>.
        /// </value>
        [JsonIgnore]
        public DotNetObjectReference<object> DotNetObjectRef { get; set; }

        /// <summary>
        /// Specifies the IEnumerable collection. This data could be queried and manipulated.
        /// </summary>
        /// <value>
        /// An <see cref="IEnumerable"/> collection that serves as the in-memory data source. Defaults to <c>null</c>.
        /// </value>
        [Parameter]
        [JsonPropertyName("json")]
        public IEnumerable<object> Json { get; set; }

        /// <summary>
        /// Specifies the key/value pair of headers.
        /// </summary>
        /// <value>
        /// An <see cref="IDictionary{TKey, TValue}"/> containing header key/value pairs. Defaults to <c>null</c>.
        /// </value>
        /// <remarks>
        /// Use Headers to add any custom headers to the request made by DataManager.
        /// Users can also send authentication bearer token using the Headers property.
        /// </remarks>
        [Parameter]
        [JsonPropertyName("headers")]
        public IDictionary<string, string> Headers { get; set; }

        /// <summary>
        /// Specifies the accept type.
        /// </summary>
        /// <value>
        /// <c>true</c> to indicate the client accepts JSON responses; otherwise, <c>false</c>. Defaults to <c>false</c>.
        /// </value>
        [Parameter]
        [JsonPropertyName("accept")]
        public bool Accept { get; set; }

        /// <summary>
        /// The in-memory data source used by the DataManager when operating in local mode.
        /// </summary>
        /// <value>
        /// An <see cref="object"/> that represents the data source. Accepts an <see cref="IEnumerable{T}"/> or any collection
        /// that the DataManager will query and manipulate on the client side. Defaults to <c>null</c>.
        /// </value>
        [Parameter]
        [JsonPropertyName("data")]
        public object Data { get; set; }

        /// <summary>
        /// Specifies the time limit to clear the cached data.
        /// </summary>
        /// <value>
        /// An <see cref="int"/> representing the time in milliseconds after which cached data expires. A value of <c>0</c> means no expiration. Defaults to <c>0</c>.
        /// </value>
        [Parameter]
        [JsonPropertyName("timeTillExpiration")]
        public int TimeTillExpiration { get; set; }

        /// <summary>
        /// Specifies the caching page size.
        /// </summary>
        /// <value>
        /// An <see cref="int"/> representing the number of records to cache per page. A value of <c>0</c> means no caching is performed. Defaults to <c>0</c>.
        /// </value>
        [Parameter]
        [JsonPropertyName("cachingPageSize")]
        public int CachingPageSize { get; set; }

        /// <summary>
        /// Enables data caching.
        /// </summary>
        /// <value>
        /// <c>true</c> to enable caching of data pages; otherwise, <c>false</c>. Defaults to <c>false</c>.
        /// When enabled, DataManager caches fetched data to reduce redundant network requests.
        /// </value>
        [Parameter]
        [JsonPropertyName("enableCaching")]
        public bool EnableCaching { get; set; }

        /// <summary>
        /// The HTTP request method type used when sending data to the server (for example, "GET" or "POST").
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the HTTP method. Remote adaptors use this value when constructing outgoing requests.
        /// When not specified, the adaptor determines the appropriate method based on the operation.
        /// </value>
        [Parameter]
        [JsonPropertyName("requestType")]
        public string RequestType { get; set; }

        /// <summary>
        /// Specifies the primary key value.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the field name that uniquely identifies each record in the data source.
        /// Used by remote adaptors to locate records during update and delete operations. Defaults to an empty string.
        /// </value>
        [Parameter]
        [JsonPropertyName("key")]
        public string Key { get; set; }

        /// <summary>
        /// When <c>true</c>, indicates that the request is a cross-domain request.
        /// </summary>
        /// <value>
        /// <c>true</c> if the request targets a different domain, subdomain, or protocol; otherwise, <c>false</c>.
        /// Defaults to <c>false</c>. When enabled, adaptors may use JSONP or cross-origin techniques.
        /// </value>
        [Parameter]
        [JsonPropertyName("crossDomain")]
        public bool CrossDomain { get; set; }

        /// <summary>
        /// When set, enables JSONP-style requests by specifying the callback parameter name.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the JSONP callback parameter name. Set this to enable JSONP requests
        /// for cross-domain scenarios that require JSONP responses. When not specified, standard HTTP requests are used.
        /// </value>
        /// <remarks>Use this only for cross-domain scenarios that require JSONP responses.</remarks>
        [Parameter]
        [JsonPropertyName("jsonp")]
        public string Jsonp { get; set; }

        /// <summary>
        /// Expected response data type for remote requests (for example "json" or "jsonp").
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the expected response format. Common values include <c>"json"</c> and <c>"jsonp"</c>.
        /// Remote adaptors may use this value to adjust request/response handling. Defaults to <c>null</c>.
        /// </value>
        [Parameter]
        [JsonPropertyName("dataType")]
        public string DataType { get; set; }

        /// <summary>
        /// Enables offline mode in DataManager.
        /// </summary>
        /// <value>
        /// <c>true</c> to enable offline mode; otherwise, <c>false</c>. Defaults to <c>false</c>.
        /// </value>
        /// <remarks>
        /// Applicable for remote data sources. If <c>true</c>, an initial request fetches data, and subsequent operations
        /// are handled in-memory with no additional server requests. Cached data is stored in the <see cref="Json"/> property.
        /// </remarks>
        [Parameter]
        [JsonPropertyName("offline")]
        public bool Offline { get; set; }

        /// <summary>
        /// Specifies whether data formatting is required.
        /// </summary>
        /// <value>
        /// <c>true</c> when formatting is required before sending data to the server; otherwise, <c>false</c>.
        /// Defaults to <c>false</c>. When enabled, adaptors may apply additional processing to format request data.
        /// </value>
        [Parameter]
        [JsonPropertyName("requiresFormat")]
        public bool RequiresFormat { get; set; }

        /// <summary>
        /// Indicates whether this instance is a DataManager.
        /// </summary>
        /// <value>
        /// <c>true</c> when this instance represents a fully initialized DataManager; otherwise, <c>false</c>.
        /// This property is set during the <see cref="OnInitializedAsync"/> lifecycle.
        /// </value>
        [DefaultValue(false)]
        [JsonPropertyName("isDataManager")]
        internal bool IsDataManager { get; set; }

        /// <summary>
        /// A lazily-initialized random integer identifier assigned on first access.
        /// </summary>
        /// <exclude/>
        private int Guid { get; set; }
        /// <summary>
        /// Unique identifier for this DataManager instance.
        /// </summary>
        /// <value>
        /// An <see cref="int"/> value that uniquely identifies this DataManager instance within a request.
        /// The value is randomly generated between 1 and 100000 on first access. Subsequent accesses return the same value.
        /// </value>
        /// <exclude />
        [JsonPropertyName("guid")]
        public int UniqueGuid
        {
            get
            {
                if (Guid == 0)
                {
                    Guid = RandomNumberGenerator.GetInt32(1, 100000);
                }
                return Guid;
            }
        }

        /// <summary>
        /// Cascading reference to the parent component that owns or provides context to this DataManager.
        /// Typical parents are data-bound components (for example grids) that cascade themselves so
        /// DataManager can access configuration and lifecycle information.
        /// </summary>
        [CascadingParameter]
        protected object Parent { get; set; }

        /// <summary>
        /// Strongly-typed cascading parent when the parent component derives from <see cref="BaseComponent"/>.
        /// Provides convenient access to base-component helper methods and shared lifecycle state.
        /// </summary>
        [CascadingParameter]
        protected BaseComponent BaseParent { get; set; }

        /// <summary>
        /// Gets or sets the child content to be rendered inside the DataManager component.
        /// </summary>
        /// <value>
        /// A <see cref="RenderFragment"/> that represents the content rendered inside the DataManager.
        /// Adaptors or child components may supply elements here to participate in data operations or provide UI for configuration.
        /// Defaults to <c>null</c>.
        /// </value>
        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Gets navigation manager to get base url.
        /// </summary>
        /// <exclude />
        [Inject]
        [JsonIgnore]
        private NavigationManager UriHelper { get; set; }

        /// <summary>
        /// Gets the Base URL.
        /// </summary>
        /// <remarks>BaseUri will be used to get absolute of Url, InsertUrl, UpdateUrl and RemoveUrl properties.</remarks>
        public string BaseUri { get; set; }

        /// <summary>
        /// Specifies the http client handler.
        /// </summary>
        /// <exclude />
        [JsonIgnore]
        internal HttpHandler _httpHandler;

        public DataManager()
        {
            InitDataManagerAdaptor();
            _httpHandler = new HttpHandler(HttpClientInstance ?? HttpClient!);

        }

        /// <summary>
        /// Component initialization lifecycle method.
        /// Runs after dependency injection is complete and is responsible for initializing
        /// DI-dependent resources (for example the internal HttpHandler), computing the
        /// <see cref="BaseUri"/>, and wiring this DataManager into its parent component if present.
        /// </summary>
        /// <exclude />
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);
            BaseUri = (HttpClientInstance ?? HttpClient)?.BaseAddress?.OriginalString ?? UriHelper.BaseUri;
            InitDataManagerAdaptor();
            _httpHandler = new HttpHandler(HttpClientInstance ?? HttpClient!);
            IsDataManager = true;
            if (Parent != null)
            {
                Type ParentType = Parent.GetType();
                ParentType.GetProperty("DataManager")?.SetValue(Parent, this);
                PropertyInfo? Field = Parent.GetType().GetProperty("jsProperty", BindingFlags.Instance | BindingFlags.NonPublic);
                string JSProperty = Field?.GetValue(Parent)?.ToString() ?? string.Empty;
                BaseComponent? MainParent = null;

                // components extended from SfBaseComponent will not have JsProperty
                if (string.IsNullOrEmpty(JSProperty))
                {
                    // Dynamic SfDataManager insertion
                    if (Parent is SfDataBoundComponent _comp)
                    {
                        if (_comp.IsRendered && !_comp.PropertyChanges.ContainsKey("DataSource"))
                        {
                            _comp.PropertyChanges.Add("DataSource", this);
                            await _comp.OnPropertyChangedAsync().ConfigureAwait(false);
                        }
                    }

                    return;
                }

                if (!JSProperty.Contains("sf.", StringComparison.Ordinal))
                {
                    PropertyInfo? MainParentProperty = Parent.GetType().GetProperty("BaseParent", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
                    MainParent = (BaseComponent)MainParentProperty?.GetValue(Parent)!;
                    MainParent.DataManagerContainer[JSProperty + ".dataSource"] = this;
                }
                object value = new DefaultAdaptor(JSProperty + ".dataSource", this);
                if (!JSProperty.Contains("sf.", StringComparison.Ordinal))
                {
                    BaseComponent.UpdateDictionary(JSProperty + ".dataSource", value, MainParent!.BindableProperties);
                }
                else
                {
                    BaseComponent.UpdateDictionary("dataSource", value, BaseParent.BindableProperties);
                }
            }
        }

        /// <summary>
        /// Executes given query and returns resultant data.
        /// </summary>
        /// <typeparam name="T">Type of the model class.</typeparam>
        /// <param name="query">Query class which will be executed against data source.</param>
        /// <returns>Task.</returns>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<object> ExecuteQuery<T>(Query query)
        {
            ArgumentNullException.ThrowIfNull(query);
            return await ExecuteQuery<T>(query.Queries).ConfigureAwait(false);
        }

        /// <summary>
        /// Executes given query and returns resultant data.
        /// </summary>
        /// <typeparam name="T">Type of the model class.</typeparam>
        /// <param name="query">Query class which will be executed against data source.</param>
        /// <returns>Task.</returns>
        public async Task<object> ExecuteQueryAsync<T>(Query query)
        {
            return await ExecuteQuery<T>(query).ConfigureAwait(false);
        }

        /// <summary>
        /// Executes given query and returns resultant data.
        /// </summary>
        /// <typeparam name="T">Type of the model class</typeparam>
        /// <param name="queries">Query class which will be executed against data source.</param>
        /// <returns>Task</returns>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<object> ExecuteQuery<T>(DataManagerRequest queries)
        {
            if (DataAdaptor != null && DataAdaptor.IsRemote())
            {
                if (Offline && queries != null)
                {
                    return await ProcessOfflineAsync<T>(queries).ConfigureAwait(false);
                }
                DataAdaptor.SetModelType(typeof(T));
                object request = DataAdaptor.ProcessQuery(queries!);
                using HttpRequestMessage queryRequest = HttpHandler.PrepareRequest((request as RequestOptions)!);
                BeforeSend(queryRequest);
                object dataResult = await (DataAdaptor?.PerformDataOperation<T>(queryRequest)).ConfigureAwait(false)!;
                object finalData = await (DataAdaptor?.ProcessResponse<T>(dataResult, queries!)).ConfigureAwait(false)!;
                return finalData;
            }
            else
            {
                DataManagerRequest request = (DataManagerRequest)DataAdaptor?.ProcessQuery(queries)!;
                object dataResult = await (DataAdaptor?.PerformDataOperation<T>(request)).ConfigureAwait(false)!;
                object finalData = await (DataAdaptor?.ProcessResponse<T>(dataResult, request)).ConfigureAwait(false)!;
                return finalData;
            }
        }

        /// <summary>
        /// Executes given query and returns resultant data.
        /// </summary>
        /// <typeparam name="T">Type of the model class</typeparam>
        /// <param name="queries">Query class which will be executed against data source.</param>
        /// <returns>Task</returns>
        public async Task<object> ExecuteQueryAsync<T>(DataManagerRequest queries)
        {
            return await ExecuteQuery<T>(queries).ConfigureAwait(false);
        }

        /// <summary>
        /// Handles offline mode data processing: fetches remote data, caches it in <see cref="Json"/>,
        /// switches to a local <see cref="BlazorAdaptor"/>, then re-executes the query against the cached data.
        /// </summary>
        /// <exclude />
        internal async Task<object> ProcessOfflineAsync<T>(DataManagerRequest queries)
        {
            // Fetch remote data
            DataManagerRequest query = new()
            {
                Params = queries.Params
            };
            object request = DataAdaptor.ProcessQuery(query);
            using HttpRequestMessage queryRequest = HttpHandler.PrepareRequest((request as RequestOptions)!);
            BeforeSend(queryRequest);
            object dataResult = await DataAdaptor.PerformDataOperation<T>(queryRequest).ConfigureAwait(false);
            object finalData = await DataAdaptor.ProcessResponse<T>(dataResult, query).ConfigureAwait(false);


            //Assign to Json property
            Json = (IEnumerable<object>)finalData;
            DataAdaptor = new BlazorAdaptor(this);

            //Process with actual query and send data
            DataManagerRequest request1 = (DataManagerRequest)DataAdaptor.ProcessQuery(queries);
            object dataResult1 = await DataAdaptor.PerformDataOperation<T>(request1).ConfigureAwait(false);
            object finalData1 = await DataAdaptor.ProcessResponse<T>(dataResult1, request1).ConfigureAwait(false);
            return finalData1;
        }

        /// <summary>
        /// Invoked before sending http request.
        /// </summary>
        /// <param name="request">HttpRequestMessage instance.</param>
        public void BeforeSend(HttpRequestMessage request)
        {
            ArgumentNullException.ThrowIfNull(request);
            DataAdaptor.BeforeSend(request);
            if (Headers?.Count > 0)
            {
                IDictionary<string, string> headers = Headers;
                foreach (KeyValuePair<string, string> header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }
        }

        /// <summary>
        /// Initializes the default <see cref="BlazorAdaptor"/> for this DataManager.
        /// </summary>
        /// <exclude />
        internal void InitDataManagerAdaptor()
        {
            DataAdaptor = new BlazorAdaptor(this);
        }

        /// <summary>
        /// Dispose unmanaged resources in the Syncfusion Blazor component.
        /// </summary>
        public virtual void Dispose()
        {
            ComponentDispose();
        }

        /// <summary>
        /// Dispose unmanaged resources in the Syncfusion Blazor component.
        /// </summary>
        internal void ComponentDispose()
        {
            DotNetObjectRef?.Dispose();
            BaseParent?._childDotNetObjectRef.Clear();
        }
    }

    /// <summary>
    /// Defines the members of the query.
    /// </summary>
    /// <remarks>DataManagerRequest is used to model bind posted data at server side.</remarks>
    // [TypeConverter(typeof(DataManagerTypeConverter))]
    public class DataManagerRequest
    {
        /// <summary>
        /// Specifies the records to skip.
        /// </summary>
        /// <value>
        /// An <see cref="int"/> representing the number of records to skip from the beginning of the result set.
        /// Use with <see cref="Take"/> to implement pagination. Defaults to <c>0</c>.
        /// </value>
        [JsonPropertyName("skip")]
        public int Skip { get; set; }

        /// <summary>
        /// Specifies the records to take.
        /// </summary>
        /// <value>
        /// An <see cref="int"/> representing the maximum number of records to return. Use with <see cref="Skip"/> to implement pagination.
        /// A value of <c>0</c> means return all records. Defaults to <c>0</c>.
        /// </value>
        [JsonPropertyName("take")]
        public int Take { get; set; }

        /// <summary>
        /// Specifies the anti-forgery key.
        /// </summary>
        /// <exclude />
        [JsonPropertyName("antiForgery")]
        public string AntiForgery { get; set; }

        /// <summary>
        /// Specifies that the count of records is required in the response.
        /// </summary>
        /// <value>
        /// <c>true</c> when the response should include the total count of matching records; otherwise, <c>false</c>.
        /// Use this to enable pagination total display. Defaults to <c>false</c>.
        /// </value>
        [JsonPropertyName("requiresCounts")]
        public bool RequiresCounts { get; set; }

        /// <summary>
        /// Specifies the table name.
        /// </summary>
        /// <value>The name of the table to query. This is used when the data source is a relational database.</value>
        /// <remarks>
        /// When the <see cref="DataManager"/> connects to a remote data source, this property specifies
        /// the table or view name to query. For OData services, this corresponds to the entity set name.
        /// </remarks>
        [JsonPropertyName("table")]
        public string Table { get; set; }

        /// <summary>
        /// Specifies the parent id mapping value.
        /// </summary>
        /// <value>The name of the foreign key column that maps parent records to child records in hierarchical data.</value>
        /// <remarks>
        /// Use this property when working with self-referential data or tree structures where records
        /// have a parent-child relationship. The mapping enables operations like flatting hierarchical data
        /// or building parent-child relationships in the resulting dataset.
        /// </remarks>
        [JsonPropertyName("IdMapping")]
        public string IdMapping { get; set; }

        /// <summary>
        /// Specifies the grouped column details.
        /// </summary>
        /// <value>An array of field names by which records are grouped in the result set.</value>
        /// <remarks>
        /// When performing remote group operations, this property contains the column names
        /// that specify which fields to group by. Use this to organize records into hierarchical
        /// structures based on common field values.
        /// </remarks>
        [JsonPropertyName("group")]
        public List<string> Group { get; set; }

        /// <summary>
        /// Specifies the select column details.
        /// </summary>
        /// <value>An array of field names to include in the result, enabling projection of specific columns.</value>
        /// <remarks>
        /// Use this property to specify which columns to retrieve from the data source. This is useful
        /// for reducing bandwidth when you only need specific fields rather than entire records.
        /// </remarks>
        [JsonPropertyName("select")]
        public List<string> Select { get; set; }

        /// <summary>
        /// Specifies the relational table names to be eagerloaded.
        /// </summary>
        /// <value>An array of related table or navigation property names to include in the query.</value>
        /// <remarks>
        /// Use this property to perform eager loading of related entities. This reduces the number
        /// of database queries by including related data in a single query using joins or expansions.
        /// </remarks>
        [JsonPropertyName("expand")]
        public List<string> Expand { get; set; }

        /// <summary>
        /// Specifies the sort criteria.
        /// </summary>
        /// <value>A list of <see cref="Sort"/> objects defining the sort order for the result set.</value>
        /// <remarks>
        /// Use this property to define one or more sort operations to apply to the data.
        /// Each <see cref="Sort"/> specifies a field name and whether to sort in ascending or descending order.
        /// </remarks>
        [JsonPropertyName("sorted")]
        public List<Sort> Sorted { get; set; }

        /// <summary>
        /// Specifies the search criteria.
        /// </summary>
        /// <value>A list of <see cref="SearchFilter"/> objects defining search conditions to apply.</value>
        /// <remarks>
        /// Use this property to define search operations that filter records based on text matching.
        /// Each <see cref="SearchFilter"/> specifies the field name, operator, and value to search for.
        /// </remarks>
        [JsonPropertyName("search")]
        public List<SearchFilter> Search { get; set; }

        /// <summary>
        /// Specifies the filter criteria.
        /// </summary>
        /// <value>A list of <see cref="WhereFilter"/> objects defining the conditions for filtering records.</value>
        /// <remarks>
        /// Use this property to define complex filter expressions that combine multiple conditions using
        /// logical operators. Each <see cref="WhereFilter"/> specifies a field, operator, and value to filter by.
        /// </remarks>
        [JsonPropertyName("where")]
        public List<WhereFilter> Where { get; set; }

        /// <summary>
        /// Specifies the aggregate details.
        /// </summary>
        /// <value>A list of <see cref="Aggregate"/> objects defining the aggregation functions to compute.</value>
        /// <remarks>
        /// Use this property to specify aggregate operations like sum, average, count, min, or max
        /// to compute over the result set. Aggregates are computed after filtering and sorting.
        /// </remarks>
        [JsonPropertyName("aggregates")]
        public List<Aggregate> Aggregates { get; set; }

        /// <summary>
        /// Specifies additional parameters.
        /// </summary>
        /// <value>A dictionary of custom key-value pairs to include with the data request.</value>
        /// <remarks>
        /// Use this property to pass additional custom parameters that the data source may require.
        /// These parameters are sent along with the request and can be used for authentication,
        /// pagination, or any other custom server-side logic.
        /// </remarks>
        [JsonPropertyName("params")]
        public IDictionary<string, object> Params { get; set; }

        /// <summary>
        /// Specifies the field names to find distinct values.
        /// </summary>
        /// <value>An array of field names for which distinct values should be returned.</value>
        /// <remarks>
        /// Use this property to retrieve only unique values for specified fields from the data source.
        /// This is useful for populating dropdown lists or eliminating duplicate entries.
        /// </remarks>
        [JsonPropertyName("distinct")]
        public List<string> Distinct { get; set; }

        /// <summary>
        /// Holds field and format method to handle group by format.
        /// </summary>
        /// <value>A dictionary mapping field names to format method strings for custom group formatting.</value>
        /// <remarks>
        /// Use this property to define custom formatting logic for grouped data. The key is the field
        /// name and the value is the format method to apply when rendering group headers or values.
        /// </remarks>
        public IDictionary<string, string> GroupByFormatter { get; set; }

        /// <summary>
        /// Specifies that perform in-built grouping.
        /// </summary>
        /// <value>
        /// <c>true</c> to enable built-in grouping functionality; <c>false</c> to use custom grouping.
        /// When <c>true</c>, the <see cref="DataManager"/> handles group operations automatically. Defaults to <c>true</c>.
        /// </value>
        public bool ServerSideGroup { get; set; } = true;

        /// <summary>
        /// Specifies that the filtered records is required in response.
        /// </summary>
        /// <value>
        /// <c>true</c> when the response should include filtered records; otherwise, <c>false</c>.
        /// Enable this when you need both the filtered result set and the total count of matching records.
        /// </value>
        public bool RequiresFilteredRecords { get; set; }

        /// <summary>
        /// Specifies that perform lazy load grouping.
        /// </summary>
        /// <value>
        /// <c>true</c> to enable lazy load grouping; <c>false</c> to load all grouped data at once.
        /// When enabled, child groups are loaded on-demand as the parent group is expanded. Defaults to <c>false</c>.
        /// </value>
        public bool LazyLoad { get; set; }

        /// <summary>
        /// Specifies that to perform expand all for lazy load grouping.
        /// </summary>
        /// <value>
        /// <c>true</c> to expand all groups initially; <c>false</c> to collapse all groups.
        /// When <see cref="LazyLoad"/> is enabled, this property controls whether groups start expanded or collapsed. Defaults to <c>false</c>.
        /// </value>
        public bool LazyExpandAllGroup { get; set; }
    }

    /// <summary>
    /// Abstract class for Data adaptors.
    /// </summary>
    /// <remarks>
    /// Extend DataAdaptor component while creating custom adaptor component. DataAdaptor component is extended from
    /// <see cref="OwningComponentBase"></see> so that
    /// services can be accessed from <see cref="OwningComponentBase.ScopedServices"/> property.
    /// </remarks>
    /// <exclude />
    public abstract class DataAdaptor : OwningComponentBase, IDataAdaptor
    {
        /// <summary>
        /// JSON serializer options used for serializing adaptor data.
        /// </summary>
        /// <exclude />
        internal static JsonSerializerOptions _serializeOptions = new()
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.Preserve
        };

        /// <summary>
        /// Reference to the parent BaseComponent that owns this adaptor.
        /// </summary>
        /// <exclude />
        internal BaseComponent? _parent;

        /// <summary>
        /// Sets the parent component reference used by the data adaptor.
        /// </summary>
        /// <param name="parent">The parent component that owns or provides context to this adaptor.</param>
        public void SetParent(BaseComponent parent)
        {
            _parent = parent;
        }

        /// <summary>
        /// Cascading parameter providing access to the parent SfDataManager instance.
        /// </summary>
        /// <exclude />
        [CascadingParameter]
        internal SfDataManager? DataManager { get; set; }

        /// <summary>
        /// Performs component initialization for the data adaptor.
        /// </summary>
        /// <exclude />
        protected override async Task OnInitializedAsync()
        {
            if (DataManager!.AdaptorInstance == null)
            {
                DataManager.BaseAdaptor.Instance = this;
                DataManager.BaseAdaptor.Instance.SetParent((DataManager.BaseAdaptor.ParentComponent as BaseComponent)!);
            }

            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Returns the data collection after performing data operations based on request from <see cref="DataManagerRequest"/>
        /// </summary>
        /// <param name="dataManagerRequest">DataManagerRequest containes the information regarding paging, grouping, filtering, searching which is handled on the DataGrid component side</param>
        /// <param name="additionalParam">An optional parameter that can be used to perform additional data operations.</param>
        /// <returns>The data collection's type is determined by how this method has been implemented.</returns>
        public virtual Task<object> ReadAsync(DataManagerRequest dataManagerRequest, string? additionalParam = null)
        {
            return Task.FromResult<object>(null!);
        }        
    }

    /// <summary>
    /// Abstract class for Data adaptors.
    /// </summary>
    /// <remarks>
    /// Extend DataAdaptor{T} component while creating custom adaptor component. DataAdaptor{T} component is extended from
    /// <see cref="OwningComponentBase{TService}"></see> so that
    /// services can be accessed from <see cref="OwningComponentBase{TService}.Service"/> property.
    /// </remarks>
    /// <exclude />
    public abstract class DataAdaptor<T> : OwningComponentBase<T>, IDataAdaptor
    {
        /// <summary>
        /// JSON serializer options used for serializing adaptor data.
        /// </summary>
        /// <exclude />
        internal static JsonSerializerOptions _serializeOptions = new()
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.Preserve
        };

        /// <summary>
        /// Reference to the parent BaseComponent that owns this adaptor.
        /// </summary>
        /// <exclude />
        internal BaseComponent? Parent { get; set; }

        /// <summary>
        /// Cascading parameter providing access to the parent SfDataManager instance.
        /// </summary>
        /// <exclude />
        [CascadingParameter]
        internal SfDataManager DataManager { get; set; }

        /// <summary>
        /// Sets the parent component reference used by the data adaptor.
        /// </summary>
        /// <param name="parent">The parent component that owns or provides context to this adaptor.</param>
        public void SetParent(BaseComponent parent)
        {
            Parent = parent;
        }

        /// <summary>
        /// Performs component initialization for the data adaptor.
        /// </summary>
        /// <exclude />
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);
            DataManager!.BaseAdaptor.Instance = this;
            DataManager.BaseAdaptor.Instance.SetParent((DataManager.BaseAdaptor.ParentComponent as BaseComponent)!);
        }

        /// <summary>
        /// Performs data Read operation asynchronously.
        /// </summary>
        public virtual Task<object> ReadAsync(DataManagerRequest dataManagerRequest, string? additionalParam = null)
        {
            return Task.FromResult<object>(null!);
        }       
    }
    internal class ForeignKeySortManager : IComparer<object>
    {
        /// <exclude />
        [JsonIgnore]
        internal string ForeignKeyField { get; }

        /// <exclude />
        [JsonIgnore]
        internal string ForeignKeyValue { get; }

        /// <exclude />
        [JsonIgnore]
        internal IEnumerable<object> ForeignKeyDataSource { get; }

        /// <exclude />
        private readonly Dictionary<string, ConcurrentDictionary<object?, object?>> _lookups = [];

        /// <exclude />
        private readonly Dictionary<string, Func<object, object?>> _fieldGetters = [];

        /// <exclude />
        private readonly Dictionary<string, Func<object, object?>> _valueGetters = [];

        /// <exclude />
        private Func<object, object?>? _displayGetter;

        /// <exclude />
        private static readonly ConcurrentDictionary<string, Func<object?, object?>> _getterCache = new();

        internal ForeignKeySortManager(string foreignKeyField, string foreignKeyValue, IEnumerable<object> foreignKeyDataSource)
        {
            ForeignKeyField = foreignKeyField;
            ForeignKeyValue = foreignKeyValue;
            ForeignKeyDataSource = foreignKeyDataSource;
            Func<object, object?> fieldGetter = GetOrCompileGetter(foreignKeyField);
            Func<object, object?> valueGetter = GetOrCompileGetter(foreignKeyValue);

            _fieldGetters[foreignKeyField] = fieldGetter;
            _valueGetters[foreignKeyField] = valueGetter;

            ConcurrentDictionary<object?, object?> lookup = new();

            _ = Parallel.ForEach(foreignKeyDataSource, item =>
            {
                object? key = fieldGetter(item);
                if (key != null)
                {
                    object? value = valueGetter(item);
                    _ = lookup.TryAdd(key, value);
                }
            });

            _lookups[foreignKeyField] = lookup;
        }
        internal void Initialize(List<Sort> sortColumns)
        {
            foreach (Sort sort in sortColumns)
            {
                if (sort.Comparer is ForeignKeySortManager foreignkey && foreignkey.ForeignKeyField == ForeignKeyField)
                {
                    _displayGetter = GetOrCompileGetter(sort.Name);
                }

            }
        }
        private static Func<object, object?> GetOrCompileGetter(string path)
        {
            return _getterCache.GetOrAdd(path, propertyPath =>
            {
                string[] parts = propertyPath.Split('.');
                return (target) =>
                {
                    if (target == null)
                    {
                        return null;
                    }

                    foreach (string part in parts)
                    {
                        if (target == null)
                        {
                            return null;
                        }

                        PropertyInfo? prop = target.GetType().GetProperty(part, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                        if (prop == null)
                        {
                            return null;
                        }
                        target = prop.GetValue(target);
                    }
                    return target;
                };
            });
        }

        private object? GetDisplayName(object record)
        {
            if (_displayGetter == null)
            {
                if (_fieldGetters.TryGetValue(ForeignKeyField, out Func<object, object?>? fkGetter))
                {
                    object? foreignkeyValue = fkGetter(record);
                    if (foreignkeyValue == null)
                    {
                        return null;
                    }

                    if (_lookups.TryGetValue(ForeignKeyField, out ConcurrentDictionary<object?, object?>? lookup) && lookup.TryGetValue(foreignkeyValue, out object? displayValue))
                    {
                        return displayValue;
                    }
                }
                return null;
            }
            object? fkValueFromDisplay = _displayGetter(record);
            if (fkValueFromDisplay == null)
            {
                return null;
            }

            foreach (ConcurrentDictionary<object?, object?> lookup in _lookups.Values)
            {
                if (lookup.TryGetValue(fkValueFromDisplay, out object? displayValue))
                {
                    return displayValue;
                }
            }

            return null;
        }
        public int Compare(object x, object y)
        {
            object? xDisplayValue = GetDisplayName(x);
            object? yDisplayValue = GetDisplayName(y);
            if (xDisplayValue == null && yDisplayValue == null)
            {
                return 0;
            }

            if (xDisplayValue == null)
            {
                return -1;
            }

            if (yDisplayValue == null)
            {
                return 1;
            }

            return xDisplayValue is string xs && yDisplayValue is string ys
                ? StringComparer.OrdinalIgnoreCase.Compare(xs, ys)
                : Comparer<object>.Default.Compare(xDisplayValue, yDisplayValue);
        }
    }
}
namespace Syncfusion.Blazor.Toolkit.Data
{
    /// <summary>
    /// Defines the sort descriptor.
    /// </summary>
    public class Sort
    {
        /// <summary>
        /// Gets the field name.
        /// </summary>
        /// <value>The name of the data field to sort by.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets the sort direction.
        /// </summary>
        /// <value>
        /// The sort direction: "ascending" for A-Z order, "descending" for Z-A order.
        /// Defaults to "ascending" when not specified.
        /// </value>
        public string Direction { get; set; }

        /// <summary>
        /// Gets the sort comparer.
        /// </summary>
        /// <exclude />
        public object Comparer { get; set; }
    }

    /// <summary>
    /// Defines members for creating search criteria.
    /// </summary>
    public class SearchFilter
    {
        /// <summary>
        /// Collection of fields to search.
        /// </summary>
        /// <value>An array of field names to search across. Multiple fields allow searching in multiple columns simultaneously.</value>
        public List<string> Fields { get; set; }

        /// <summary>
        /// Specifies the search key.
        /// </summary>
        /// <value>The search term to match against the specified fields.</value>
        public string Key { get; set; }

        /// <summary>
        /// Specifies the search operator. By default, contains operator will be used.
        /// </summary>
        /// <value>
        /// The operator used for the search comparison. Defaults to "contains" if not specified.
        /// Common operators include "startswith", "endswith", "equals", "contains".
        /// </value>
        public string Operator { get; set; }

        /// <summary>
        /// Specifies that incasesensitive search to be done.
        /// </summary>
        /// <value>
        /// <c>true</c> to perform case-insensitive search; <c>false</c> for case-sensitive.
        /// Defaults to <c>false</c>. Only applies when the field type is string.
        /// </value>
        public bool IgnoreCase { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to ignore accent marks and diacritic characters during search operations.
        /// </summary>
        /// <value>
        /// <c>true</c> to treat accented and unaccented characters as equivalent (e.g., "é" as "e"); otherwise, <c>false</c>. 
        /// The default is <c>false</c>.
        /// </value>
        /// <remarks>
        /// Enable this option to improve the accuracy and usability of search results in multilingual datasets 
        /// where users may omit accent marks. This is especially useful for user-entered queries in globalized applications.
        /// </remarks>
        public bool IgnoreAccent { get; set; }
    }

    /// <summary>
    /// Defines the members of the aggregate.
    /// </summary>
    public class Aggregate
    {
        /// <summary>
        /// Specifies the field name.
        /// </summary>
        [JsonPropertyName("field")]
        public string Field { get; set; }

        /// <summary>
        /// Specifies the aggregate type.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    /// <summary>
    /// Defines the members to build filter criteria.
    /// </summary>
    public class WhereFilter
    {
        /// <summary>
        /// Specifies the field name.
        /// </summary>
        /// <value>
        /// The name of the data field to filter on. Use dot notation for nested properties.
        /// </value>
        [JsonPropertyName("field")]
        public string Field { get; set; }

        /// <summary>
            /// Specifies that filter should be incasesensitive.
            /// </summary>
            /// <value>
            /// <c>true</c> to perform case-insensitive string comparison; <c>false</c> for case-sensitive.
            /// Defaults to <c>false</c>. Only applies when the field type is string.
            /// </value>
            /// <remarks>
            /// When filtering string fields, enable this property to ignore character casing during comparison.
            /// For example, filtering with "apple" would match "Apple", "APPLE", and "apple".
            /// </remarks>
            [JsonPropertyName("ignoreCase")]
            public bool IgnoreCase { get; set; }

        /// <summary>
        /// Specifies that ignore accent/diacritic letters while searching.
        /// </summary>
        /// <value>
        /// <c>true</c> to ignore accent marks and diacritic letters; <c>false</c> for exact matching.
        /// Defaults to <c>false</c>. Enable this for locale-aware text filtering.
        /// </value>
        /// <remarks>
        /// When enabled, characters like 'é' would match 'e', 'ñ' would match 'n', etc.
        /// Useful for search functionality in applications that support multiple languages.
        /// </remarks>
        [JsonPropertyName("ignoreAccent")]
        public bool IgnoreAccent { get; set; }

        /// <summary>
        /// Indicates whether this filter contains nested predicates.
        /// </summary>
        /// <value>
        /// <c>true</c> when the filter has child predicates; otherwise <c>false</c>.
        /// </value>
        [JsonPropertyName("isComplex")]
        public bool IsComplex { get; set; }

        /// <summary>
        /// Gets the filter operator.
        /// </summary>
        /// <value>The operator used to compare the field value. Common operators include "equals", "contains", "greaterthan", "lessthan".</value>
        /// <remarks>
        /// Supported operators vary by data source type. For remote data, OData operators like "eq", "contains", "startswith" are used.
        /// Local data operations support operators like "equals", "notequal", "contains", "startswith", "endswith".
        /// </remarks>
        [JsonPropertyName("operator")]
        public string Operator { get; set; }

        /// <summary>
        /// Provides the complex filter merge condition.
        /// </summary>
        /// <value>
        /// The logical operator to combine predicates: "and" or "or". Defaults to "and" when combining multiple conditions.
        /// </value>
        /// <remarks>
        /// This property is used when <see cref="IsComplex"/> is <c>true</c> to determine how multiple
        /// filter conditions in <see cref="Predicates"/> are combined.
        /// </remarks>
        [JsonPropertyName("condition")]
        public string Condition { get; set; }

        /// <summary>
        /// Specifies the filter value.
        /// </summary>
        /// <value>The value to compare against the field. The type should match the data type of the field.</value>
        /// <remarks>
        /// For numeric fields, this should be a number. For string fields, it should be a string.
        /// For date fields, use a DateTime value. Complex types can be used for advanced filtering scenarios.
        /// </remarks>
        [JsonPropertyName("value")]
        public object value { get; set; }

        /// <summary>
        /// Specifies the collection filter criteria.
        /// </summary>
        [JsonPropertyName("predicates")]
        public List<WhereFilter> Predicates { get; set; }

        /// <summary>
        /// Specifies the column type to denoting the type of data it displays. 
        /// </summary>
        internal string? ColumnType { get; set; }

        /// <summary>
        /// Merge the give collection of predicates using And condition.
        /// </summary>
        /// <param name="predicates">List of predicates.</param>
        /// <returns>WhereFilter.</returns>
        public static WhereFilter And(List<WhereFilter> predicates)
        {
            return new WhereFilter() { Condition = "and", IsComplex = true, Predicates = predicates };
        }

        /// <summary>
        /// Merge the give collection of predicates using Or condition.
        /// </summary>
        /// <param name="predicates">List of predicates.</param>
        /// <returns>WhereFilter.</returns>
        public static WhereFilter Or(List<WhereFilter> predicates)
        {
            return new WhereFilter() { Condition = "or", IsComplex = true, Predicates = predicates };
        }

        /// <summary>
        /// Merge the give predicate using And condition.
        /// </summary>
        /// <param name="fieldName">Specifies the field name.</param>
        /// <param name="operator">Specifies the filter operator.</param>
        /// <param name="value">Specifies the filter value.</param>
        /// <param name="ignoreCase">Performs incasesensitive filtering.</param>
        /// <param name="ignoreAccent">Ignores accent/diacritic letters while filtering.</param>
        /// <returns></returns>
        public WhereFilter And(string fieldName, string? @operator = null, object? value = null, bool ignoreCase = false, bool ignoreAccent = false)
        {
            WhereFilter predicate = new()
            {
                Field = fieldName,
                Operator = @operator!,
                value = value!,
                IgnoreCase = ignoreCase,
                IgnoreAccent = ignoreAccent
            };
            WhereFilter combined = new()
            {
                Condition = "and",
                IsComplex = true,
                Predicates =
                [
                    this,
                    predicate
                ]
            };
            return combined;
        }

        internal WhereFilter And(string fieldName, string? @operator = null, object? value = null, bool ignoreCase = false, bool ignoreAccent = false, string? columnType = null)
        {
            WhereFilter predicate = new()
            {
                Field = fieldName,
                Operator = @operator!,
                value = value!,
                IgnoreCase = ignoreCase,
                IgnoreAccent = ignoreAccent,
                ColumnType = columnType
            };
            WhereFilter combined = new()
            {
                Condition = "and",
                IsComplex = true,
                Predicates =
                [
                    this,
                    predicate
                ]
            };
            return combined;
        }

        /// <summary>
        /// Merge the give predicate using And condition.
        /// </summary>
        /// <param name="predicate">Predicate to be merged.</param>
        /// <returns>WhereFilter.</returns>
        public WhereFilter And(WhereFilter predicate)
        {
            WhereFilter combined = new()
            {
                Condition = "and",
                IsComplex = true,
                Predicates =
                [
                    this,
                    predicate
                ]
            };
            return combined;
        }

        /// <summary>
        /// Merge the give predicate using Or condition.
        /// </summary>
        /// <param name="fieldName">Specifies the field name.</param>
        /// <param name="operator">Specifies the filter operator.</param>
        /// <param name="value">Specifies the filter value.</param>
        /// <param name="ignoreCase">Performs incasesensitive filtering.</param>
        /// <param name="ignoreAccent">Ignores accent/diacritic letters while filtering.</param>
        /// <returns></returns>
        public WhereFilter Or(string fieldName, string? @operator = null, object? value = null, bool ignoreCase = false, bool ignoreAccent = false)
        {
            WhereFilter predicate = new()
            {
                Field = fieldName,
                Operator = @operator!,
                value = value!,
                IgnoreCase = ignoreCase,
                IgnoreAccent = ignoreAccent
            };
            WhereFilter combined = new()
            {
                Condition = "or",
                IsComplex = true,
                Predicates =
                [
                    this,
                    predicate
                ]
            };
            return combined;
        }

        internal WhereFilter Or(string fieldName, string? @operator = null, object? value = null, bool ignoreCase = false, bool ignoreAccent = false, string? columnType = null)
        {
            WhereFilter predicate = new()
            {
                Field = fieldName,
                Operator = @operator!,
                value = value!,
                IgnoreCase = ignoreCase,
                IgnoreAccent = ignoreAccent,
                ColumnType = columnType
            };
            WhereFilter combined = new()
            {
                Condition = "or",
                IsComplex = true,
                Predicates =
                [
                    this,
                    predicate
                ]
            };
            return combined;
        }

        /// <summary>
        /// Merge the give predicate using Or condition.
        /// </summary>
        /// <param name="predicate">Predicate to be merged.</param>
        /// <returns>WhereFilter.</returns>
        public WhereFilter Or(WhereFilter predicate)
        {
            WhereFilter combined = new()
            {
                Condition = "or",
                IsComplex = true,
                Predicates =
                [
                    this,
                    predicate
                ]
            };
            return combined;
        }
    }

    /// <summary>
    /// Gets or Sets the properties to be specified for GraphQLAdaptor.
    /// </summary>
    public class GraphQLAdaptorOptions
    {
        /// <summary>
        /// Defines the GraphQL query used to fetch data from the GraphQL service.
        /// </summary>
        /// <value>
        /// This property holds the GraphQL query string that is used to request data from the GraphQL service.
        /// The query string should be formatted according to the GraphQL syntax.
        /// </value>
        /// <remarks>
        /// The following parameter is passed in the Query string,
        /// <c>dataManager</c> - Contains the <see cref="DataManagerRequest"/>class properties.
        /// The <see cref="DataManagerRequest"/>class properties are used for performing data operations.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <SfDataManager Url=https://localhost:7140/graphql GraphQLAdaptorOptions=@AdaptorOptions Adaptor="Adaptors.GraphQLAdaptor">
        /// </SfDataManager>
        /// @code{
        ///     private GraphQLAdaptorOptions AdaptorOptions { get; set; } = new GraphQLAdaptorOptions
        ///     {
        ///         Query = @"
        ///             query orderDatas($dataManager: DataManagerRequestInput!) {
        ///                 orderDatas(dataManager: $dataManager) {
        ///                     count, result { OrderID, EmployeeID, Freight, OrderDate } , aggregates
        ///             }
        ///         }"
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        public string Query { get; set; }

        /// <summary>
        /// Defines the mutations used to perform CRUD operations in the GraphQL service.
        /// </summary>
        /// <value>
        /// This property holds an instance of the <see cref="GraphQLMutation"/> class, which provides a way to define and manage mutations
        /// for performing CRUD operations in the GraphQL service.
        /// </value>         
        /// <remarks>
        /// The <c>Mutation</c> property provides a way to define and manage mutations for performing CRUD operations in the GraphQL service.        
        /// These mutations facilitate creating, updating, and deleting data, allowing comprehensive data manipulation through GraphQL service.
        /// </remarks>
        public GraphQLMutation Mutation { get; set; }

        /// <summary>
        /// Defines the resolver function name used in the GraphQL service.
        /// </summary>
        /// <value>
        /// This property holds the name of the resolver function, which is used to retrieve data from the GraphQL service
        /// and bind it to the corresponding component.  
        /// </value>        
        /// <remarks>
        /// The <c>ResolverName</c> property should be specified in PascalCase, and the same name will be used as the resolver function's name
        /// in the GraphQL service. This property plays a pivotal role in connecting the component to the GraphQL service.
        /// By specifying the resolver function name, the component can fetch the necessary data to populate itself with information
        /// In GraphQL service, the resolver function should return <c>DataResult</c> class object containing Result, Count and optional Aggregates information.
        /// When working with the GraphQL service, it's important to include Aggregates details in the returned <c>DataResult</c> object when rendering footer summaries.
        /// However, if footer summaries are not being rendered, there's no need to include Aggregates information in the returned <c>DataResult</c> object.   
        /// </remarks>
        /// <example>
        /// <code>
        /// Code example in GraphQL sample
        /// <![CDATA[
        /// <SfDataManager Url=https://localhost:7140/graphql GraphQLAdaptorOptions=@AdaptorOptions Adaptor="Adaptors.GraphQLAdaptor">
        /// </SfDataManager>
        /// @code {
        ///     private GraphQLAdaptorOptions AdaptorOptions { get; set; } = new GraphQLAdaptorOptions
        ///     {
        ///         ResolverName = “OrderDatas”;
        ///     }
        /// }
        /// ]]>
        /// Resolver function code in GraphQL service
        /// <![CDATA[
        /// public DataResult<T> OrderDatas(DataManagerRequest dataManager)
        /// {
        ///     var result = GetOrders();
        ///     int count = result.Count();
        ///     ...
        ///     ...
        ///     if (dataManagerRequest.Aggregates != null) {
        ///         ...
        ///         return new DataResult<T>() { Count = count, Result = result, Aggregates = aggregates };
        ///     }
        ///     return new DataResult<T>() { Count = count, Result = result };
        ///  }
        ///  public class DataResult<T> {
        ///     public int Count { get; set; }
        ///     public IEnumerable<T> Result { get; set; }
        ///     [GraphQLType(typeof(AnyType))]
        ///     public IDictionary<string, object> Aggregates { get; set; }
        ///  }
        /// ]]>
        /// </code>
        /// </example>
        public string ResolverName { get; set; }
    }

    /// <summary>
    /// Represents a collection of mutations used to perform CRUD operations in a GraphQL service.
    /// </summary>
    public class GraphQLMutation
    {
        /// <summary>
        /// Defines the mutation used to perform an insert operation in a GraphQL service.
        /// </summary>
        /// <value>
        /// This property holds the GraphQL mutation query for data insertion in the GraphQL service.
        /// The mutation query should be formatted according to the GraphQL syntax and enable inserting data into the service.
        /// </value>
        /// <returns>Returns the newly inserted record details.</returns>
        /// <remarks>
        /// The <c>Insert</c> property allows you to specify the GraphQL mutation insert query to perform data insertion in the GraphQL service.
        /// The following parameters are passed in the Insert query string,
        /// <c>record</c> - The new record which is need to be inserted.
        /// <c>index</c> - Specifies the index at which the newly added record will be inserted.
        /// <c>action</c> - Indicates the type of operation being performed.
        /// <c>additionalParameters</c> - An optional parameter that can be used to perform additional operations.
        /// </remarks>
        /// <example>
        /// <code>      
        /// <![CDATA[
        /// <SfDataManager Url=https://localhost:7140/graphql GraphQLAdaptorOptions=@AdaptorOptions Adaptor="Adaptors.GraphQLAdaptor">
        /// </SfDataManager>
        /// @code{
        ///     private GraphQLAdaptorOptions AdaptorOptions { get; set; } = new GraphQLAdaptorOptions
        ///     {
        ///         Mutation = new GraphQLMutation
        ///         {
        ///             Insert = @"
        ///                mutation insert($record: OrderInput!, $index: Int!, $action: String!, $additionalParameters: Any) {
        ///                   insertOrder(order: $record, position: $index, action: $action, additionalParameters: $additionalParameters) {
        ///                    OrderID, EmployeeID, Freight, OrderDate
        ///                  }
        ///             }"
        ///         }
        ///     }
        /// ]]>
        /// </code>
        /// </example>
        public string Insert { get; set; }

        /// <summary>
        /// Defines the mutation used to perform an update operation in a GraphQL service.
        /// </summary>
        /// <value>
        /// This property holds the GraphQL mutation query for updating data in the GraphQL service.
        /// The mutation query should be formatted according to the GraphQL syntax and enable updating existing data in the service.
        /// </value>
        /// <returns>Returns the updated record details.</returns>        
        /// <remarks>
        /// The <c>Update</c> property allows you to specify the GraphQL mutation update query to perform data updation in the GraphQL service.
        /// The following parameters are passed in the Update query string,
        /// <c>record</c> - The new record which is need to be updated.
        /// <c>action</c> - Indicates the type of operation being performed.
        /// <c>primaryColumnName</c> - The primaryColumnName specifies the field name of the primary column.
        /// <c>primaryColumnValue</c> - The primaryColumnValue specifies the primary column value which is needs to be updated in the collection.
        /// <c>additionalParameters</c> - An optional parameter that can be used to perform additional operations.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <SfDataManager Url=https://localhost:7140/graphql GraphQLAdaptorOptions=@AdaptorOptions Adaptor="Adaptors.GraphQLAdaptor">
        /// </SfDataManager>
        /// @code{
        ///     private GraphQLAdaptorOptions AdaptorOptions { get; set; } = new GraphQLAdaptorOptions
        ///     {
        ///         Mutation = new GraphQLMutation
        ///         {
        ///             Update = @"
        ///                mutation update($record: OrderInput!, $action: String!, $primaryColumnName: String!, $primaryColumnValue: Int!, $additionalParameters: Any) {
        ///                  updateOrder(order: $record, action: $action, keyField: $primaryColumnName, keyValue: $primaryColumnValue, additionalParameters: $additionalParameters) {
        ///                    OrderID, EmployeeID, Freight, OrderDate
        ///                  }
        ///             }"
        ///         }
        ///     }
        /// ]]>
        /// </code>
        /// </example>
        public string Update { get; set; }

        /// <summary>
        /// Defines the mutation used to perform delete operation in GraphQL service.
        /// </summary>      
        /// <value>
        /// This property holds the GraphQL mutation query for deleting the data in the GraphQL service.
        /// The mutation query should be formatted according to the GraphQL syntax and enable deleting the existing data in the service.
        /// </value> 
        /// <returns>Returns the deleted record details.</returns>        
        /// <remarks>
        /// The <c>Delete</c> property allows you to specify the GraphQL mutation delete query to perform delete operation in the GraphQL service.
        /// The following parameters are passed in the Delete query string,
        /// <c>primaryColumnValue</c> - The primaryColumnValue specifies the primary column value which is needs to be removed from the collection.
        /// <c>action</c> Indicates the type of operation being performed.
        /// <c>primaryColumnName</c> - The primaryColumnName specifies the field name of the primary column.  
        /// <c>additionalParameters</c> - An optional parameter that can be used to perform additional operations.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <SfDataManager Url=https://localhost:7140/graphql GraphQLAdaptorOptions=@AdaptorOptions Adaptor="Adaptors.GraphQLAdaptor">
        /// </SfDataManager>
        /// @code{
        ///     private GraphQLAdaptorOptions AdaptorOptions { get; set; } = new GraphQLAdaptorOptions
        ///     {
        ///         Mutation = new GraphQLMutation
        ///         {
        ///             Delete = @"
        ///                mutation delete($primaryColumnValue: Int!, $action: String!, $primaryColumnName: String!, $additionalParameters: Any) {
        ///                  deleteOrder(id: $ primaryColumnValue, action: $action, keyField: $primaryColumnName, additionalParameters: $additionalParameters) {
        ///                    OrderID, EmployeeID, Freight, OrderDate
        ///                  }
        ///             }"
        ///         }
        ///     }
        /// ]]>
        /// </code>
        /// </example>
        public string Delete { get; set; }

        /// <summary>
        /// Defines the mutation used to perform CRUD operation Synchronously in GraphQL service.
        /// </summary>
        /// <value>
        /// This property holds the GraphQL mutation query for bulk changes in the GraphQL service.
        /// The mutation query should be formatted according to the GraphQL syntax and enable CRUD operations Synchronously.
        /// </value>
        /// <returns>Returns the updated collection. </returns>          
        /// <remarks>
        /// The <c>Batch</c> property allows you to specify the GraphQL mutation bacth query to perform CRUD operation in the GraphQL service.
        /// The following parameters are passed in the Batch query string,
        /// <c>changed</c> - Specifies the collection of record to be updated.
        /// <c>added</c> - Specifies the collection of record to be inserted.
        /// <c>deleted</c> - Specifies the collection of record to be removed.
        /// <c>action</c> - Indicates the type of operation being performed.
        /// <c>primaryColumnName</c> - The primaryColumnName specifies the field name of the primary column.
        /// <c>additionalParameters</c> - An optional parameter that can be used to perform additional operations.
        /// <c>dropIndex</c> - Specifies the record position, from which new records will be added while performing row drag and drop.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <SfDataManager Url=https://localhost:7140/graphql GraphQLAdaptorOptions=@AdaptorOptions Adaptor="Adaptors.GraphQLAdaptor">
        /// </SfDataManager>
        /// @code{
        ///     private GraphQLAdaptorOptions AdaptorOptions { get; set; } = new GraphQLAdaptorOptions
        ///     {
        ///         Mutation = new GraphQLMutation
        ///         {
        ///             Batch = @"
        ///                mutation batch($changed: [OrderInput!], $added: [OrderInput!], $deleted: [OrderInput!], $action: String!, $primaryColumnName: String!, $additionalParameters: Any, , $dropIndex: Int!) {
        ///                  batchUpdate(changed: $changed, added: $added, deleted: $deleted, action: $action, keyField :$primaryColumnName, additionalParameters: $additionalParameters,  dropIndex: $dropIndex) {
        ///                    OrderID, EmployeeID, Freight, OrderDate
        ///                  }
        ///             }"
        ///         }
        ///     }
        /// ]]>
        /// </code>
        /// </example>
        public string Batch { get; set; }
    }


    /// <summary>
    /// Provide adaptor information which sends to client side.
    /// </summary>
    /// <exclude />
    public class DefaultAdaptor
    {
        /// <summary>
        /// Gets or sets the adaptor identifier used by the client side data manager.
        /// </summary>
        [Parameter]
        [JsonPropertyName("adaptor")]
        public string Adaptor { get; set; } = "BlazorAdaptor";

        /// <summary>
        /// Gets or sets the name of the adaptor implementation.
        /// </summary>
        [JsonPropertyName("adaptorName")]
        public string AdaptorName { get; set; } = "BlazorAdaptor";

        /// <summary>
        /// Gets or sets the key field used by the adaptor.
        /// </summary>
        [JsonPropertyName("key")]
        internal string Key { get; set; }

        /// <summary>
        /// Gets or sets the service URL used by the adaptor.
        /// </summary>
        [JsonPropertyName("url")]
        [DefaultValue("")]
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the adaptor operates in offline mode.
        /// </summary>
        [JsonPropertyName("offline")]
        public bool Offline { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultAdaptor"/> class.
        /// </summary>
        public DefaultAdaptor()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultAdaptor"/> class with the specified key and adaptor name.
        /// </summary>
        /// <param name="key">The primary key field name.</param>
        /// <param name="adaptorName">The adaptor name to use.</param>
        public DefaultAdaptor(string key, string adaptorName = "BlazorAdaptor")
        {
            Key = key;
            AdaptorName = adaptorName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultAdaptor"/> class with the specified key, data manager, and adaptor name.
        /// </summary>
        /// <param name="key">The primary key field name.</param>
        /// <param name="manager">The data manager instance that provides URL and offline settings.</param>
        /// <param name="adaptorName">The adaptor name to use.</param>
        public DefaultAdaptor(string key, DataManager manager, string adaptorName = "BlazorAdaptor")
        {
            Key = key;
            AdaptorName = adaptorName;
            Url = manager?.Url!;
            Offline = manager?.Offline ?? false;
        }
    }

    /// <summary>
    /// Defines the members of the data manager operation result.
    /// </summary>
    public class DataResult : DataResult<object>
    {
    }

    /// <summary>
    /// Defines the members of the data manager operation result.
    /// </summary>
    /// <typeparam name="T">Type of the data source element.</typeparam>
    public class DataResult<T>
    {
        /// <summary>
        /// Gets the result of the data operation.
        /// </summary>
        [JsonPropertyName("result")]
        public IEnumerable Result { get; set; }

        /// <summary>
        /// Gets the total count of the records in data source.
        /// </summary>
        [JsonPropertyName("count")]
        public int Count { get; set; }

        /// <summary>
        /// Gets the aggregate result based on the aggregate query.
        /// </summary>
        [JsonPropertyName("aggregates")]
        public IDictionary<string, object> Aggregates { get; set; }

        /// <summary>
        /// Gets the filtered records.
        /// </summary>
        public IEnumerable FilteredRecords { get; set; }
    }

    /// <summary>
    /// Handles custom adaptor logic.
    /// </summary>
    /// <exclude />
    public class BaseAdaptor
    {
        /// <summary>
        /// Gets or sets the active data adaptor instance.
        /// </summary>
        public IDataAdaptor Instance { get; set; }

        /// <summary>
        /// Gets or sets the generic type used to deserialize records for the adaptor.
        /// </summary>
        public Type GenericType { get; set; }

        /// <summary>
        /// Gets or sets the parent component associated with this adaptor.
        /// </summary>
        public object ParentComponent { get; set; }

        /// <summary>
        /// Gets or sets the owning data manager instance.
        /// </summary>
        public DataManager DataManagerInstance { get; set; }

        /// <summary>
        /// Gets or sets the JSON serializer options used when deserializing data.
        /// </summary>
        public static JsonSerializerOptions DeserializeOptions { get; set; } = new JsonSerializerOptions
        {
            Converters = {
                                new DateTimeOffsetCustomConverter(),
                                new DateTimeZoneHandlingConverter()
                            },
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        /// <summary>
        /// Gets or sets the JSON serializer options used when serializing data.
        /// </summary>
        public static JsonSerializerOptions SerializeOptions { get; set; } = new JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.Preserve
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseAdaptor"/> class.
        /// </summary>
        /// <param name="type">The adaptor type to create.</param>
        /// <param name="parentComponent">The parent component associated with the adaptor.</param>
        /// <param name="dataManagerInstance">The owning data manager instance.</param>
        public BaseAdaptor(Type type, object parentComponent, DataManager dataManagerInstance)
        {
            ParentComponent = parentComponent;
            DataManagerInstance = dataManagerInstance;
            if (type != null && DataManagerInstance != null)
            {
                Instance = (DataAdaptor)DataManagerInstance.ServiceProvider.GetService(type)!;
                Instance ??= (DataAdaptor)Activator.CreateInstance(type)!;
                Instance.SetParent((parentComponent as BaseComponent)!);
            }

            GenericType = ParentComponent.GetType();
            if (GenericType.IsGenericType && GenericType.GetGenericArguments().Length > 0)
            {
                GenericType = GenericType.GetGenericArguments()[0];
            }
            else
            {
                GenericType = null!;
            }
        }
    }

    /// <summary>
    /// Defines the members of the grouped record.
    /// </summary>
    /// <typeparam name="T">Type of the data source elements.</typeparam>
    public class Group<T> : List<Group<T>>
    {
        /// <summary>
        /// Unique identifier.
        /// </summary>
        [JsonPropertyName("groupGuid")]
        public string GroupGuid { get; set; }

        /// <summary>
        /// Specifies the level of this group.
        /// </summary>
        [JsonPropertyName("level")]
        public int Level { get; set; }

        /// <summary>
        /// Specifies the count of child if any.
        /// </summary>
        [JsonPropertyName("childLevels")]
        public int ChildLevels { get; set; }

        /// <summary>
        /// Specifies the ungrouped records.
        /// </summary>
        [JsonPropertyName("records")]
        public IEnumerable Records { get; set; }

        /// <summary>
        /// Specifies the group key value.
        /// </summary>
        [JsonPropertyName("key")]
        public object Key { get; set; }

        /// <summary>
        /// Specifies the count of items in this group.
        /// </summary>
        [JsonPropertyName("count")]
        public int CountItems { get; set; }

        /// <summary>
        /// Specifies the items of the group.
        /// </summary>
        [JsonPropertyName("items")]
        public IEnumerable Items { get; set; }

        /// <summary>
        /// Specifies the aggregates of this group.
        /// </summary>
        [JsonPropertyName("aggregates")]
        public object Aggregates { get; set; }

        /// <summary>
        /// Specifies the field value.
        /// </summary>
        [JsonPropertyName("field")]
        public string Field { get; set; }

        /// <summary>
        /// Specifies the header text of the field.
        /// </summary>
        [JsonPropertyName("headerText")]
        public string HeaderText { get; set; }

        /// <summary>
        /// Specifies the foreign key.
        /// </summary>
        [JsonPropertyName("foreignKey")]
        public string ForeignKey { get; set; }

        /// <summary>
        /// Specifies the result.
        /// </summary>
        [JsonPropertyName("result")]
        public object Result { get; set; }

        /// <summary>
        /// Specifies the grouped data.
        /// </summary>
        public IEnumerable GroupedData { get; set; }

        /// <summary>
        /// This pertains to the group key value in its unformatted state, which is used for sorting purposes to maintain consistency.
        /// </summary>
        /// <remarks>
        /// The formatted date values were in string format, leading to incorrect ordering in the date column. To address this, we utilize this property to sort date values as Date objects, rather than formatted date strings, preventing such issues.
        /// </remarks>
        internal object? UnformattedKey { get; set; }
    }

    /// <summary>
    /// Interface for Data adaptors.
    /// </summary>
    public interface IDataAdaptor
    {
        /// <summary>
        /// Sets the parent component reference used by the adaptor.
        /// </summary>
        /// <param name="parent">The parent component that owns or provides context to the adaptor.</param>
        void SetParent(BaseComponent parent);

        /// <summary>
        /// Returns the data collection after performing data operations based on request from <see cref="DataManagerRequest"/>
        /// </summary>
        /// <param name="dataManagerRequest">DataManagerRequest containes the information regarding paging, grouping, filtering, searching which is handled on the DataGrid component side</param>
        /// <param name="additionalParam">An optional parameter that can be used to perform additional data operations.</param>
        /// <returns>The data collection's type is determined by how this method has been implemented.</returns>
        Task<object> ReadAsync(DataManagerRequest dataManagerRequest, string? additionalParam = null);      
    }
}
