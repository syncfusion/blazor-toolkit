using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections;
using System.Globalization;
using Syncfusion.Blazor.Toolkit.Charts.Internal;

namespace Syncfusion.Blazor.Toolkit.Data
{
    /// <summary>
    /// Interface for http handler used by data manager.
    /// </summary>
    /// <exclude/>
    internal interface IHttpHandler
    {
        HttpClient GetClient();

        Task<HttpResponseMessage> SendRequest(HttpRequestMessage data);
    }

    /// <summary>
    /// Bas class for http handler used by data manager.
    /// </summary>
    /// <exclude/>
    /// <remarks>
    /// 
    /// </remarks>
    /// <param name="http"></param>
    internal class HttpHandlerBase(HttpClient http) : IHttpHandler
    {
        private HttpClient Client { get; set; } = http;
        /// <summary>
        /// Returns http client.
        /// </summary>
        /// <returns>HttpClient</returns>
        public virtual HttpClient GetClient()
        {
            return Client ??= new HttpClient();
        }

        public virtual async Task<HttpResponseMessage> SendRequest(HttpRequestMessage data)
        {
            return await Task.FromResult<HttpResponseMessage>(null!).ConfigureAwait(true);
        }
    }

    /// <summary>
    /// Handles HttpClient instance creation. Also build and sends HttpMessages request.
    /// </summary>
    /// <exclude/>
    internal class HttpHandler(HttpClient client) : HttpHandlerBase(client)
    {
        // Return a new mutable options instance each time
        private static JsonSerializerOptions SerializeOptions => new()
        {
            WriteIndented = true,
        };

        public HttpClient? Client { get; set; }

        public override async Task<HttpResponseMessage> SendRequest(HttpRequestMessage data)
        {
            Client = GetClient();
            HttpResponseMessage? returnData = null;
            try
            {
                returnData = await Client.SendAsync(data).ConfigureAwait(true);
                if (returnData.IsSuccessStatusCode)
                {
                    return returnData;
                }
                else
                {
                    _ = returnData.EnsureSuccessStatusCode();
                    return null!;
                }
            }
            catch (Exception e)
            {
                HttpContent? content = returnData?.Content;
                string responseContent = content == null ? "" : await content.ReadAsStringAsync().ConfigureAwait(true);

                HttpResponseMessage errorResponse = new(System.Net.HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"Error: {e}. Response content: {responseContent}"),
                    ReasonPhrase = "Internal Server Error"
                };
                return errorResponse;
                //This line throw has been added to avoid warning for the catch - CA1031
                throw new HttpRequestException(e.ToString(), new HttpRequestException(responseContent));
            }
        }

        public static HttpRequestMessage PrepareRequest(RequestOptions options)
        {
            if (!options.Url!.StartsWith("http", StringComparison.Ordinal))
            {
                options.Url = DataUtil.GetUrl(options.BaseUrl!, options.Url);
            }

            HttpRequestMessage req = new() { RequestUri = new Uri(options.Url), Method = options.RequestMethod! };
            JsonSerializerOptions settings = SerializeOptions;
            settings.Converters.Add(new JsonStringEnumConverter());
            settings.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            if (req.Method.Equals(HttpMethod.Patch))
            {
                List<IgnorePropertiesByType> propertiesToIgnore = DataUtil.GetPropertiesToRemove();
                if (propertiesToIgnore.Count > 0)
                {
                    settings.Converters.Add(new IgnorablePropertiesConverter<object>(propertiesToIgnore));
                }
                settings.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
            }
            string serializedData = options.Data == null ? string.Empty : (options.Data.GetType() == typeof(string) ? (string)options.Data : JsonSerializer.Serialize(options.Data, settings));
            if (req.Method != HttpMethod.Get && req.Method != HttpMethod.Head)
            {
                StringContent stringContent = new(serializedData, Encoding.UTF8, options.ContentType);
                req.Content = stringContent;
            }

            return req;
        }

        public static HttpRequestMessage PrepareBatchRequest(RequestOptions options, Type? ModelType = null)
        {
            if (!options.Url!.StartsWith("http", StringComparison.Ordinal))
            {
                options.Url = DataUtil.GetUrl(options.BaseUrl!, options.Url);
            }

            HttpRequestMessage req = new() { RequestUri = new Uri(options.Url), Method = options.RequestMethod! };
            MultipartContent batchContent = new("mixed", options.ContentType);
            CRUDModel<object>? batchRecords = options.Data as CRUDModel<object>;
            int count = 0;
            JsonSerializerOptions settings = new();
            settings.Converters.Add(new JsonStringEnumConverter());
            settings.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            if (batchRecords?.Added?.Count > 0)
            {
                foreach (object data in batchRecords.Added)
                {
                    using MultipartContent changeSet = new("mixed", options.CSet!);
                    using HttpRequestMessage postRequest = new(HttpMethod.Post, options.BaseUrl);
                    postRequest.Content = new StringContent(JsonSerializer.Serialize(data, settings), Encoding.UTF8, "application/json");
                    postRequest.Headers.Add("Accept", options.Accept);
                    postRequest.Headers.Add("Content-Id", count.ToString(CultureInfo.InvariantCulture));
                    count += 1;

                    using HttpMessageContent postRequestContent = new(postRequest);
                    _ = postRequestContent.Headers.Remove("Content-Type");
                    postRequestContent.Headers.Add("Content-Type", "application/http");
                    postRequestContent.Headers.Add("Content-Transfer-Encoding", "binary");

                    changeSet.Add(postRequestContent);
                    batchContent.Add(changeSet);
                }
            }

            if (batchRecords?.Changed?.Count > 0)
            {
                for (int i = 0; i < batchRecords.Changed.Count; i++)
                {
                    using MultipartContent changeSet = new("mixed", options.CSet!);
                    object value = DataUtil.GetVal(batchRecords.Changed, i, options.KeyField!);
                    string urlKey = DataUtil.GetODataUrlKey(null!, options.KeyField!, value, ModelType);
                    string param = DataUtil.GetAdditionalParams(options);
                    using HttpRequestMessage putRequest = new(options.UpdateType!, $"{options.BaseUrl}{urlKey}{param}");
                    List<object>? orgData = (options.Original as IEnumerable)?.Cast<object>().ToList().Where((e) =>
                    {
                        return DataUtil.GetVal(batchRecords.Changed, i, options.KeyField!)?.ToString() == e?.GetType()?.GetProperty(options.KeyField!)?.GetValue(e)?.ToString();
                    }).ToList();
                    object changedData = DataUtil.CompareAndRemove(batchRecords.Changed[i], orgData?[0]!, options.KeyField!, IsFromBatch: true);
                    putRequest.Content = new StringContent(JsonSerializer.Serialize(changedData, settings), Encoding.UTF8, "application/json");
                    putRequest.Headers.Add("Accept", options.Accept);
                    putRequest.Headers.Add("Content-Id", count.ToString(CultureInfo.InvariantCulture));
                    count += 1;

                    using HttpMessageContent postRequestContent = new(putRequest);
                    _ = postRequestContent.Headers.Remove("Content-Type");
                    postRequestContent.Headers.Add("Content-Type", "application/http");
                    postRequestContent.Headers.Add("Content-Transfer-Encoding", "binary");

                    changeSet.Add(postRequestContent);
                    batchContent.Add(changeSet);
                }
            }

            if (batchRecords?.Deleted?.Count > 0)
            {
                foreach (object data in batchRecords.Deleted)
                {
                    using MultipartContent changeSet = new("mixed", options.CSet!);
                    string urlKey = DataUtil.GetODataUrlKey(data, options.KeyField!, ModelType: ModelType);
                    string param = DataUtil.GetAdditionalParams(options);
                    using HttpRequestMessage deleteRequest = new(HttpMethod.Delete, $"{options.BaseUrl}{urlKey}{param}");
                    deleteRequest.Content = new StringContent(JsonSerializer.Serialize(data, settings), Encoding.UTF8, "application/json");
                    deleteRequest.Headers.Add("Accept", "application/json;odata=light;q=1,application/json;odata=verbose;q=0.5");
                    deleteRequest.Headers.Add("Content-Id", count.ToString(CultureInfo.InvariantCulture));
                    count += 1;

                    using HttpMessageContent postRequestContent = new(deleteRequest);
                    _ = postRequestContent.Headers.Remove("Content-Type");
                    postRequestContent.Headers.Add("Content-Type", "application/http");
                    postRequestContent.Headers.Add("Content-Transfer-Encoding", "binary");

                    changeSet.Add(postRequestContent);
                    batchContent.Add(changeSet);
                }
            }

            req.Content = batchContent;
            return req;
        }
    }
}
