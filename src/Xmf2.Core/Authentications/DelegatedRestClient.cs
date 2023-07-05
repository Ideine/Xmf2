//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Threading;
//using System.Threading.Tasks;
//using RestSharp.Portable;

//namespace Xmf2.Core.Authentications
//{
//	public abstract class DelegatedRestClient : IAuthenticatedRestClient
//	{
//		private readonly IAuthenticatedRestClient _authenticatedRestClientImplementation;

//		protected DelegatedRestClient(IAuthenticatedRestClient authenticatedRestClientImplementation)
//		{
//			_authenticatedRestClientImplementation = authenticatedRestClientImplementation;
//		}

//		public void Dispose()
//		{
//			_authenticatedRestClientImplementation.Dispose();
//		}

//		public Task<IRestResponse> Execute(IRestRequest request) => _authenticatedRestClientImplementation.Execute(request);

//		public Task<IRestResponse<T>> Execute<T>(IRestRequest request) => _authenticatedRestClientImplementation.Execute<T>(request);

//		public Task<IRestResponse> Execute(IRestRequest request, CancellationToken ct) => _authenticatedRestClientImplementation.Execute(request, ct);

//		public Task<IRestResponse<T>> Execute<T>(IRestRequest request, CancellationToken ct) => _authenticatedRestClientImplementation.Execute<T>(request, ct);

//		public IDeserializer GetHandler(string contentType) => _authenticatedRestClientImplementation.GetHandler(contentType);

//		public IEncoding GetEncoding(IEnumerable<string> encodingIds) => _authenticatedRestClientImplementation.GetEncoding(encodingIds);

//		public IRestClient AddHandler(string contentType, IDeserializer deserializer) => _authenticatedRestClientImplementation.AddHandler(contentType, deserializer);

//		public IRestClient RemoveHandler(string contentType) => _authenticatedRestClientImplementation.RemoveHandler(contentType);

//		public IRestClient ClearHandlers() => _authenticatedRestClientImplementation.ClearHandlers();

//		public IRestClient AddEncoding(string encodingId, IEncoding encoding) => _authenticatedRestClientImplementation.AddEncoding(encodingId, encoding);

//		public IRestClient RemoveEncoding(string encodingId) => _authenticatedRestClientImplementation.RemoveEncoding(encodingId);

//		public IRestClient ClearEncodings() => _authenticatedRestClientImplementation.ClearEncodings();

//		public IAuthenticator Authenticator
//		{
//			get => _authenticatedRestClientImplementation.Authenticator;
//			set => _authenticatedRestClientImplementation.Authenticator = value;
//		}

//		public Uri BaseUrl
//		{
//			get => _authenticatedRestClientImplementation.BaseUrl;
//			set => _authenticatedRestClientImplementation.BaseUrl = value;
//		}

//		public IParameterCollection DefaultParameters => _authenticatedRestClientImplementation.DefaultParameters;

//		public CookieContainer CookieContainer
//		{
//			get => _authenticatedRestClientImplementation.CookieContainer;
//			set => _authenticatedRestClientImplementation.CookieContainer = value;
//		}

//		public IWebProxy Proxy
//		{
//			get => _authenticatedRestClientImplementation.Proxy;
//			set => _authenticatedRestClientImplementation.Proxy = value;
//		}

//		public ICredentials Credentials
//		{
//			get => _authenticatedRestClientImplementation.Credentials;
//			set => _authenticatedRestClientImplementation.Credentials = value;
//		}

//		public bool IgnoreResponseStatusCode
//		{
//			get => _authenticatedRestClientImplementation.IgnoreResponseStatusCode;
//			set => _authenticatedRestClientImplementation.IgnoreResponseStatusCode = value;
//		}

//		public TimeSpan? Timeout
//		{
//			get => _authenticatedRestClientImplementation.Timeout;
//			set => _authenticatedRestClientImplementation.Timeout = value;
//		}

//		public string UserAgent
//		{
//			get => _authenticatedRestClientImplementation.UserAgent;
//			set => _authenticatedRestClientImplementation.UserAgent = value;
//		}

//		public IDictionary<string, IDeserializer> ContentHandlers => _authenticatedRestClientImplementation.ContentHandlers;

//		public IDictionary<string, IEncoding> EncodingHandlers => _authenticatedRestClientImplementation.EncodingHandlers;

//		public IRestClient ParentClient { get; set; }
//		public virtual Task Logout() => _authenticatedRestClientImplementation.Logout();
//	}
//}