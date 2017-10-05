using System.IO;
using System.Threading.Tasks;

namespace Xmf2.Rx.Services.Resources
{
	public interface IAssetService
	{
		Task<Stream> ReadAsset(string path);
	}
}