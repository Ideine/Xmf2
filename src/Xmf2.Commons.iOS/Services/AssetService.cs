using System.IO;
using System.Threading.Tasks;
using Foundation;
using Xmf2.Commons.Services.Resources;

namespace Xmf2.Commons.iOS.Services
{
	public class AssetService : IAssetService
	{
		public async Task<Stream> ReadAsset(string path)
		{
			string resourcePath = NSBundle.MainBundle.PathForResource(Path.GetFileNameWithoutExtension(path), Path.GetExtension(path)?.TrimStart('.'));
			
			using (Stream inputStream = File.OpenRead(resourcePath))
			{
				MemoryStream result = new MemoryStream();
				await inputStream.CopyToAsync(result);

				result.Seek(0, SeekOrigin.Begin);
				return result;
			}
		}
	}
}