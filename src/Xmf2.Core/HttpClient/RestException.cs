using System;
using RestSharp;

namespace Xmf2.Core.HttpClient
{
	public class RestException(RestResponse response) : Exception($"Status code: {response.StatusCode} : {response.ResponseUri?.AbsoluteUri}")
	{
		public RestResponse Response { get; } = response;
	}
}