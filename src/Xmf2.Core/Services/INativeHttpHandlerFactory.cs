using System.Net.Http;
using RestSharp.Portable;

namespace Xmf2.Core.Services
{
	public interface INativeHttpHandlerFactory
	{
		HttpClientHandler NewHandler();
	}
}