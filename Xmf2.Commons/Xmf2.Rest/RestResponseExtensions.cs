using System.Threading.Tasks;
using RestSharp.Portable;
using Xmf2.Rest.OAuth2;

namespace Xmf2.Rest
{
	public static class RestResponseExtensions
	{
		public static T Unwrap<T>(this IRestResponse<T> response)
		{
			if (response.IsSuccess)
			{
				return response.Data;
			}
			throw new RestException(response);
		}

		public static async Task<T> Unwrap<T>(this Task<IRestResponse<T>> responseTask)
		{
			return (await responseTask).Unwrap();
		}
	}
}