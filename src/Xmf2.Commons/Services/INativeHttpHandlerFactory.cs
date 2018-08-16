using System.Net.Http;

namespace Xmf2.Commons.Services
{
	public interface INativeHttpHandlerFactory
	{
		HttpClientHandler NewHandler();
	}
}
