using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp.Portable;
using Xmf2.Rest.HttpClient.Impl;
using RestClientExtensions = Xmf2.Rest.HttpClient.RestClientExtensions;

namespace Xmf2.Rest.OAuth2
{
	public class OAuth2RestClient : RestClientBase, IOAuth2Client
    {
		protected OAuth2Authenticator OAuth2Authenticator { get; set; }

		public OAuth2ConfigurationBase Configuration { get; set; }

		public string RefreshToken { get; private set; }

		public string AccessToken { get; private set; }

		/// <summary>
		/// Extension point to log requests (HttpMethod, url, body)
		/// </summary>
		public Action<Method, string, string> LogRequest { get; set; }

		public event EventHandler<OAuth2AuthResult> OnAuthSuccess;

	    public event EventHandler<OAuth2AuthResult> OnAuthError;
		
	    public OAuth2RestClient() : base(new DefaultHttpClientFactory())
	    {
		    IgnoreResponseStatusCode = true;
		    Timeout = TimeSpan.FromSeconds(5);
	    }

	    public OAuth2RestClient(string baseUrl) : this(new Uri(baseUrl))
	    {
		    
	    }

	    public OAuth2RestClient(Uri baseUrl) : this()
	    {
		    BaseUrl = baseUrl;
	    }

		protected override IHttpContent GetContent(IRestRequest request, RequestParameters parameters)
		{
			Action<Method, string, string> logger = LogRequest;
			if (logger != null)
			{
				string content;
				Parameter body = parameters.OtherParameters.FirstOrDefault(x => x.Type == ParameterType.RequestBody);
				if (body != null)
				{
					byte[] data = body.Value as byte[];
					if (data != null)
					{
						if (body.ContentType.Contains("json") || body.ContentType.Contains("xml"))
						{
							content = Encoding.UTF8.GetString(data, 0, data.Length);
						}
						else
						{
							content = $"<binary content> contentType: {body.ContentType}";
						}
					}
					else if (body.Value is string)
					{
						content = (string) body.Value;
					}
					else
					{
						content = JsonConvert.SerializeObject(body.Value);
					}
				}
				else
				{
					List<Parameter> getOrPostParameters = parameters.OtherParameters.GetGetOrPostParameters().ToList();
					content = getOrPostParameters.Count != 0 ? string.Join("\n", getOrPostParameters.Select(x => $"{x.Name}={x.Value}")) : null;
				}

				logger(request.Method, request.Resource, content);

			}

			return RestClientExtensions.GetContent(this, request, parameters);
		}

	    public Task<OAuth2AuthResult> Login(string login, string password)
	    {
		    return Login(login, password, CancellationToken.None);
	    }

		public Task<OAuth2AuthResult> Login(string login, string password, CancellationToken ct)
		{
			if (Configuration == null)
			{
				throw new InvalidOperationException("Configuration has not been set before calling login method");
			}

			IRestRequest request = new RestRequest(Configuration.LoginUrl, Configuration.LoginMethod);
			Configuration.PopulateLoginRequest(request, login, password);

			return ExecuteAuthRequest(request, ct);
		}

		public Task<OAuth2AuthResult> Refresh()
	    {
		    return Refresh(CancellationToken.None);
	    }

		public Task<OAuth2AuthResult> Refresh(CancellationToken ct)
		{
			if (Configuration == null)
			{
				throw new InvalidOperationException("Configuration has not been set before calling login method");
			}

			if (RefreshToken == null)
			{
				throw new InvalidOperationException("Can not refresh without a Refresh token");
			}

			IRestRequest request = new RestRequest(Configuration.RefreshUrl, Configuration.RefreshMethod);
			Configuration.PopulateRefreshRequest(request, RefreshToken);
			return ExecuteAuthRequest(request, ct);
		}

		public Task<OAuth2AuthResult> Refresh(string refreshToken) 
		{
			RefreshToken = refreshToken;
			return Refresh(CancellationToken.None);
		}

	    public Task<OAuth2AuthResult> Refresh(string refreshToken, CancellationToken ct)
	    {
		    RefreshToken = refreshToken;
		    return Refresh(ct);
	    }

		protected async Task<OAuth2AuthResult> ExecuteAuthRequest(IRestRequest request, CancellationToken ct)
	    {
			request.AddHeader(OAuth2Authenticator.NO_AUTH_HEADER, true);
			IRestResponse response = await Execute(request, ct);
			OAuth2AuthResult result = Configuration.HandleAuthResult(response);
			
			if (result.IsSuccess)
			{
				AccessToken = result.AccessToken;
				RefreshToken = result.RefreshToken;

				OAuth2Authenticator authenticator = OAuth2Authenticator ?? CreateAuthenticator();
				authenticator.Access = result;
				Authenticator = OAuth2Authenticator = authenticator;
			}

			RaiseOnAuthEvents(result);

		    return result;
	    }

	    public void Logout()
	    {
		    Authenticator = null;
		    OAuth2Authenticator = null;
	    }
		
		public override async Task<IRestResponse<T>> Execute<T>(IRestRequest request, CancellationToken ct)
		{
			using (IHttpResponseMessage response = await ExecuteRequest(request, ct).ConfigureAwait(false))
			{
				if (response.IsSuccessStatusCode)
				{
					return await RestResponse.CreateResponse<T>(this, request, response, ct).ConfigureAwait(false);
				}
				IRestResponse restResponse = await RestResponse.CreateResponse(this, request, response, ct).ConfigureAwait(false);
				throw new RestException(restResponse);
			}
		}

		protected virtual OAuth2Authenticator CreateAuthenticator() => new OAuth2Authenticator
	    {
		    Configuration = Configuration
	    };
		
	    protected void RaiseOnAuthEvents(OAuth2AuthResult authResult)
	    {
		    if(authResult.IsSuccess)
		    {
			    RaiseOnAuthSuccess(authResult);
		    }
		    else
		    {
				RaiseOnAuthError(authResult);
			}
	    }

	    protected void RaiseOnAuthSuccess(OAuth2AuthResult authResult)
	    {
		    OnAuthSuccess?.Invoke(this, authResult);
	    }

	    protected void RaiseOnAuthError(OAuth2AuthResult authResult)
	    {
		    OnAuthError?.Invoke(this, authResult);
	    }
    }
}
