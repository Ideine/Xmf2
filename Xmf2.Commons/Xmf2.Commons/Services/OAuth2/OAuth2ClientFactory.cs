﻿using System;
using Ideine.Rest.OAuth2;
using RestSharp.Portable;
using Xmf2.Commons.Logs;

namespace Xmf2.Commons.Services.OAuth2
{
	public static class OAuth2ClientFactory
	{
		public static IOAuth2Client CreateClient(string baseUrl, OAuth2ConfigurationBase configuration, Action<Method, string, string> logMethod = null)
		{
			return new OAuth2RestClient(baseUrl)
			{
				Configuration = configuration,
				LogRequest = logMethod,
			};
		}

		public static Action<Method, string, string> CreateDefaultLog(ILogger logger)
		{
			return (method, uri, body) => logger.LogInfo(message: $"HTTP {method} {uri} with content {body}");
		}
	}
}