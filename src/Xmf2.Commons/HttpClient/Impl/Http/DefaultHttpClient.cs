using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using RestSharp.Portable;

namespace Xmf2.Rest.HttpClient.Impl.Http
{
    /// <summary>
    /// Wraps a <see cref="HttpClient"/> instance as <see cref="IHttpClient"/>.
    /// </summary>
    public class DefaultHttpClient : IHttpClient
    {
        private readonly DefaultHttpHeaders _defaultHeaders;

        private readonly CookieContainer _cookies;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultHttpClient"/> class.
        /// </summary>
        /// <param name="client">The <see cref="HttpClient"/> to wrap.</param>
        /// <param name="cookies">A container of cookies</param>
        public DefaultHttpClient(System.Net.Http.HttpClient client, CookieContainer cookies)
        {
            _cookies = cookies;
            Client = client;
            _defaultHeaders = new DefaultHttpHeaders(client.DefaultRequestHeaders);
        }

        /// <summary>
        /// Gets the HTTP client to wrap.
        /// </summary>
        public System.Net.Http.HttpClient Client { get; }

        /// <summary>
        /// Gets or sets the base address of the HTTP client
        /// </summary>
        public Uri BaseAddress
        {
            get => Client.BaseAddress;
            set => Client.BaseAddress = value;
        }

        /// <summary>
        /// Gets the default request headers
        /// </summary>
        public IHttpHeaders DefaultRequestHeaders => _defaultHeaders;

        /// <summary>
        /// Gets or sets the timeout
        /// </summary>
        public TimeSpan Timeout
        {
            get => Client.Timeout;
            set => Client.Timeout = value;
        }

        /// <summary>
        /// Asynchronously send a request
        /// </summary>
        /// <param name="request">The request do send</param>
        /// <param name="cancellationToken">The cancellation token used to signal an abortion</param>
        /// <returns>The task to query the response</returns>
        public async Task<IHttpResponseMessage> SendAsync(IHttpRequestMessage request, CancellationToken cancellationToken)
        {
	        try
	        {
		        var requestMessage = request.AsHttpRequestMessage();
		        var response = await Client.SendAsync(requestMessage, cancellationToken);
		        return new DefaultHttpResponseMessage(requestMessage, response, _cookies);
	        }
	        catch (TaskCanceledException ex)
	        {
		        if (ex.CancellationToken == cancellationToken)
		        {
			        throw;
		        }
		        throw new TimeoutException("HTTP Timeout", ex);
	        }
        }

        /// <summary>
        /// Disposes the underlying HTTP client
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Disposes the underlying HTTP client when disposing is set to true
        /// </summary>
        /// <param name="disposing">true, when called from <see cref="Dispose()"/>.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            Client.Dispose();
        }
    }
}
