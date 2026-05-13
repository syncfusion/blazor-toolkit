using System.Diagnostics.Contracts;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Syncfusion.Blazor.Toolkit.Data
{
    /// <summary>
    /// Derived HttpMessageContent class to prepare or modify the multipart type requests.
    /// Reference from the https://github.com/aspnet/AspNetWebStack/blob/master/src/System.Net.Http.Formatting/HttpMessageContent.cs to prepare a HttpContent extension.
    /// </summary>
    /// <exclude/>
    /// <summary>
    /// Represents an <see cref="HttpContent"/> wrapper for serializing HTTP request and response messages.
    /// </summary>
    internal class HttpMessageContent : HttpContent
    {
        private const string SP = " ";
        private const string ColonSP = ": ";
        private const string CRLF = "\r\n";
        private const string CommaSeparator = ", ";
        private const int DefaultHeaderAllocation = 2 * 1024;
        private const string DefaultMediaType = "application/http";
        private const string MsgTypeParameter = "msgtype";
        private const string DefaultRequestMsgType = "request";
        private const string DefaultResponseMsgType = "response";

        private static readonly HashSet<string> _singleValueHeaderFields = new(StringComparer.OrdinalIgnoreCase)
        {
            "Cookie",
            "Set-Cookie",
            "X-Powered-By",
        };

        private static readonly HashSet<string> _spaceSeparatedValueHeaderFields = new(StringComparer.OrdinalIgnoreCase)
        {
            "User-Agent",
        };

        private bool _contentConsumed;
        private Lazy<Task<Stream>> _streamTask;
        private long? _cachedLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpMessageContent"/> class for an HTTP request message.
        /// </summary>
        /// <param name="httpRequest">The HTTP request message to wrap.</param>
        public HttpMessageContent(HttpRequestMessage httpRequest)
        {
            HttpRequestMessage = httpRequest;
            Headers.ContentType = new MediaTypeHeaderValue(DefaultMediaType);
            Headers.ContentType.Parameters.Add(new NameValueHeaderValue(MsgTypeParameter, DefaultRequestMsgType));

            InitializeStreamTask();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpMessageContent"/> class for an HTTP response message.
        /// </summary>
        /// <param name="httpResponse">The HTTP response message to wrap.</param>
        public HttpMessageContent(HttpResponseMessage httpResponse)
        {
            HttpResponseMessage = httpResponse;
            Headers.ContentType = new MediaTypeHeaderValue(DefaultMediaType);
            Headers.ContentType.Parameters.Add(new NameValueHeaderValue(MsgTypeParameter, DefaultResponseMsgType));

            InitializeStreamTask();
        }

        private HttpContent? Content => HttpRequestMessage != null ? HttpRequestMessage.Content : HttpResponseMessage.Content;

        /// <summary>
        /// Gets the wrapped HTTP request message, if this instance was created from a request.
        /// </summary>
        public HttpRequestMessage HttpRequestMessage { get; private set; }

        /// <summary>
        /// Gets the wrapped HTTP response message, if this instance was created from a response.
        /// </summary>
        public HttpResponseMessage HttpResponseMessage { get; private set; }

        private void InitializeStreamTask()
        {
            _streamTask = new Lazy<Task<Stream>>(() => Content == null ? null! : Content.ReadAsStreamAsync());
        }

        /// <summary>
        /// Serializes the wrapped HTTP message into the target stream.
        /// </summary>
        /// <param name="stream">The destination stream.</param>
        /// <param name="context">The transport context.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            byte[] header = SerializeHeader();
            if (stream != null)
            {
                await stream.WriteAsync(header.AsMemory(), CancellationToken.None).ConfigureAwait(false);
                if (Content != null)
                {
                    Stream readStream = await _streamTask.Value.ConfigureAwait(false);
                    ValidateStreamForReading(readStream);
                    if (readStream.CanSeek)
                    {
                        _cachedLength = header.Length + readStream.Length;
                    }
                    else if (Content.Headers.ContentLength.HasValue)
                    {
                        _cachedLength = header.Length + Content.Headers.ContentLength.Value;
                    }

                    await Content.CopyToAsync(stream).ConfigureAwait(false);
                }
                else
                {
                    _cachedLength = header.Length;
                }
            }
        }

        /// <summary>
        /// Determines the length of the serialized HTTP content, if it can be computed.
        /// </summary>
        /// <param name="length">When this method returns, contains the computed length in bytes.</param>
        /// <returns><see langword="true"/> if the length can be computed; otherwise, <see langword="false"/>.</returns>
        protected override bool TryComputeLength(out long length)
        {
            byte[] header = SerializeHeader();
            if (_cachedLength.HasValue)
            {
                length = _cachedLength.Value;
                return true;
            }

            length = header.Length;
            if (Content == null)
            {
                _cachedLength = length;
                return true;
            }

            if (Content.Headers.ContentLength.HasValue)
            {
                length += Content.Headers.ContentLength.Value;
                _cachedLength = length;
                return true;
            }

            length = -1;
            return false;
        }

        /// <summary>
        /// Releases the unmanaged resources used by this instance and optionally disposes managed resources.
        /// </summary>
        /// <param name="disposing">true to dispose managed resources; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (HttpRequestMessage != null)
                {
                    HttpRequestMessage.Dispose();
                    HttpRequestMessage = null!;
                }

                if (HttpResponseMessage != null)
                {
                    HttpResponseMessage.Dispose();
                    HttpResponseMessage = null!;
                }
            }

            base.Dispose(disposing);
        }

        private static void SerializeRequestLine(StringBuilder message, HttpRequestMessage httpRequest)
        {
            Contract.Assert(message != null, "message cannot be null");
            _ = message.Append(httpRequest.Method + SP);
            _ = message.Append(httpRequest?.RequestUri?.PathAndQuery + SP);
            _ = message.Append("HTTP/1.1" + CRLF);
        }

        private static void SerializeHeaderFields(StringBuilder message, HttpHeaders headers)
        {
            Contract.Assert(message != null, "message cannot be null");
            if (headers != null)
            {
                foreach (KeyValuePair<string, IEnumerable<string>> header in headers)
                {
                    if (_singleValueHeaderFields.Contains(header.Key))
                    {
                        foreach (string value in header.Value)
                        {
                            _ = message.Append(header.Key + ColonSP + value + CRLF);
                        }
                    }
                    else
                    {
                        _ = _spaceSeparatedValueHeaderFields.Contains(header.Key)
                            ? message.Append(header.Key + ColonSP + string.Join(SP, header.Value) + CRLF)
                            : message.Append(header.Key + ColonSP + string.Join(CommaSeparator, header.Value) + CRLF);
                    }
                }
            }
        }

        private byte[] SerializeHeader()
        {
            StringBuilder message = new(DefaultHeaderAllocation);
            SerializeRequestLine(message, HttpRequestMessage);
            HttpHeaders? headers = HttpRequestMessage.Headers;
            HttpContent? content = HttpRequestMessage.Content;
            SerializeHeaderFields(message, headers);
            if (content != null)
            {
                SerializeHeaderFields(message, content.Headers);
            }

            _ = message.Append(CRLF);
            return Encoding.UTF8.GetBytes(message.ToString());
        }

        private void ValidateStreamForReading(Stream stream)
        {
            if (_contentConsumed)
            {
                stream.Position = 0;
            }

            _contentConsumed = true;
        }
    }
}