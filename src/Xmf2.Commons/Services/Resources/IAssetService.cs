using System.IO;
using System.Threading.Tasks;

namespace Xmf2.Commons.Services.Resources
{
	public interface IAssetService
	{
		Task<Stream> ReadAsset(string path);
	}
}