﻿using System;
using Xmf2.Rest.OAuth2;
using RestSharp.Portable;
using Xmf2.Commons.Logs;
using Xmf2.Rest.HttpClient.Impl;

namespace Xmf2.Commons.Services.OAuth2
{
	public static class OAuth2ClientFactory
	{
		public static IOAuth2Client CreateClient(string baseUrl, OAuth2ConfigurationBase configuration, IHttpClientFactory factory = null, Action<Method, string, string> logMethod = null)
		{
			return new OAuth2RestClient(factory ?? new DefaultHttpClientFactory(), baseUrl)
			{
				Configuration = configuration,
				LogRequest = logMethod,
				Timeout = TimeSpan.FromSeconds(30)
			};
		}

		public static IOAuth2Client CreateClient(OAuth2ConfigurationBase configuration, IHttpClientFactory factory = null, Action<Method, string, string> logMethod = null)
		{
			return new OAuth2RestClient(factory ?? new DefaultHttpClientFactory())
			{
				Configuration = configuration,
				LogRequest = logMethod,
				Timeout = TimeSpan.FromSeconds(30)
			};
		}

		public static Action<Method, string, string> CreateDefaultLog(ILogger logger)
		{
			return (method, uri, body) => logger.LogInfo(message: $"HTTP {method} {uri} with content {body}");
		}
	}
}
