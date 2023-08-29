using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FFImageLoading;
using FFImageLoading.Cache;
using FFImageLoading.Config;
using FFImageLoading.Work;
using RestSharp.Portable;
using Xmf2.Core.Services;

namespace Xmf2.Core.ImgLoading.Cache
{
	/// <example>
	/// IRequestService requestService = ...;
	///	ImageService.Instance.Config.DownloadCache = new AuthenticatedDownloadCache(requestService);
	/// </example>
	public class AuthenticatedDownloadCache : DownloadCache
	{
		private readonly IRequestService _requestService;

		public AuthenticatedDownloadCache(IRequestService requestService) : this(requestService, ImageService.Instance.Config) { }

		public AuthenticatedDownloadCache(IRequestService requestService, Configuration configuration) : base(configuration)
		{
			_requestService = requestService;
		}

		protected override async Task<byte[]> DownloadAsync(string url, CancellationToken token, System.Net.Http.HttpClient client, TaskParameter parameters, DownloadInformation downloadInformation)
		{
			RestRequest request = new(url, Method.GET);
			IRestResponse response = await _requestService.Execute(request, token, withAuthentication: true);
			if (response.IsSuccess)
			{
				return response.RawBytes;
			}
			else
			{
				throw new HttpRequestException(response.StatusDescription);
			}
		}
	}
}