using System;
using RestSharp.Portable;

namespace Xmf2.Rest.OAuth2
{
	public class RestException : Exception
	{
		public IRestResponse Response { get; }

		public RestException(IRestResponse response) : base($"Status code: {response.StatusCode} : {response.ResponseUri.AbsoluteUri}")
		{
			Response = response;
		}
	}
}
