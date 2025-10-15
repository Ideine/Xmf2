using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using RestSharp.Portable;
using Xmf2.Rest.HttpClient.Impl.Http;

namespace Xmf2.Rest.HttpClient.Impl
{
	/// <summary>
	/// The default HTTP client factory
	/// </summary>
	/// <remarks>
	/// Any other implementation should derive from this class, because it contains several
	/// useful utility functions for the creation of a HTTP client and request message.
	/// </remarks>
	public class DefaultHttpClientFactory : IHttpClientFactory
	{
		/// <summary>
		/// Create the client
		/// </summary>
		/// <param name="client">The REST client that wants to create the HTTP client</param>
		/// <returns>A new HttpClient object</returns>
		/// <remarks>
		/// The DefaultHttpClientFactory contains some helpful protected methods that helps gathering
		/// the data required for a proper configuration of the HttpClient.
		/// </remarks>
		public virtual IHttpClient CreateClient(IRestClient client)
		{
			HttpMessageHandler handler = CreateMessageHandler(client);

			System.Net.Http.HttpClient httpClient;
			if (handler != null)
			{
				httpClient = new System.Net.Http.HttpClient(handler, true);
			}
			else
			{
				httpClient = new System.Net.Http.HttpClient();
			}

			httpClient.BaseAddress = GetBaseAddress(client);

			TimeSpan? timeout = client.Timeout;
			if (timeout.HasValue)
			{
				httpClient.Timeout = timeout.Value;
			}

			return new DefaultHttpClient(httpClient, client.CookieContainer);
		}

		/// <summary>
		/// Create the request message
		/// </summary>
		/// <param name="client">The REST client that wants to create the HTTP request message</param>
		/// <param name="request">The REST request for which the HTTP request message is created</param>
		/// <param name="parameters">The request parameters for the REST request except the content header parameters (read-only)</param>
		/// <returns>A new HttpRequestMessage object</returns>
		/// <remarks>
		/// The DefaultHttpClientFactory contains some helpful protected methods that helps gathering
		/// the data required for a proper configuration of the HttpClient.
		/// </remarks>
		public virtual IHttpRequestMessage CreateRequestMessage(IRestClient client, IRestRequest request, IList<Parameter> parameters)
		{
			Uri address = GetMessageAddress(client, request);
			var method = GetHttpMethod(client, request).ToHttpMethod();
			var message = new HttpRequestMessage(method, address);
			message = AddHttpHeaderParameters(message, request, parameters);
			return new DefaultHttpRequestMessage(message);
		}

		/// <summary>
		/// Get the REST requests base address (for HTTP client)
		/// </summary>
		/// <param name="client">REST client</param>
		/// <returns>The base URL</returns>
		protected virtual Uri GetBaseAddress(IRestClient client)
		{
			if (client.BaseUrl == null)
			{
				return null;
			}

			return client.BuildUri(null, false);
		}

		/// <summary>
		/// Get the REST requests relative address (for HTTP request message)
		/// </summary>
		/// <param name="client">REST client</param>
		/// <param name="request">REST request</param>
		/// <returns>The relative request message URL</returns>
		protected virtual Uri GetMessageAddress(IRestClient client, IRestRequest request)
		{
			Uri fullUrl = client.BuildUri(request);
			if (client.BaseUrl == null)
			{
				return fullUrl;
			}

			Uri url = client.BuildUri(null, false).MakeRelativeUri(fullUrl);
			return url;
		}

		/// <summary>
		/// Returns the HTTP method for the request message.
		/// </summary>
		/// <param name="client">The REST client that wants to create the HTTP client</param>
		/// <param name="request">REST request</param>
		/// <returns>HTTP method</returns>
		protected virtual Method GetHttpMethod(IRestClient client, IRestRequest request)
		{
			return client.GetEffectiveHttpMethod(request);
		}

		/// <summary>
		/// Returns a modified HTTP request message object with HTTP header parameters
		/// </summary>
		/// <param name="message">HTTP request message</param>
		/// <param name="request">REST request</param>
		/// <param name="parameters">The request parameters for the REST request except the content header parameters (read-only)</param>
		/// <returns>The modified HTTP request message</returns>
		protected virtual HttpRequestMessage AddHttpHeaderParameters(HttpRequestMessage message, IRestRequest request, IList<Parameter> parameters)
		{
			foreach (Parameter param in parameters.Where(x => x.Type == ParameterType.HttpHeader))
			{
				if (message.Headers.Contains(param.Name))
				{
					message.Headers.Remove(param.Name);
				}

				string paramValue = param.ToRequestString();
				if (param.ValidateOnAdd)
				{
					message.Headers.Add(param.Name, paramValue);
				}
				else
				{
					message.Headers.TryAddWithoutValidation(param.Name, paramValue);
				}
			}

			return message;
		}

		/// <summary>
		/// Create the message handler
		/// </summary>
		/// <param name="client">The REST client that wants to create the HTTP client</param>
		/// <returns>A new HttpMessageHandler object</returns>
		protected virtual HttpMessageHandler CreateMessageHandler(IRestClient client)
		{
			//vju 01/09/2021 : do not create HttpMessageHandler to use platform default
			//more detail on : http://jonathanpeppers.com/Blog/improving-http-performance-in-xamarin-applications
			return null;
		}
	}
}