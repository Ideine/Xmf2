using System;
using System.Net.Http;

namespace Xmf2.Core.Services
{
	public interface INativeHttpHandlerFactory
	{
		HttpClientHandler NewHandler();
	}
}