using System.IO;
using System.Threading.Tasks;
using Android.Content;
using Android.Content.Res;
using Xmf2.Commons.Services.Resources;

namespace Xmf2.Commons.Droid.Services
{
	//TODO: revoir si le dossier est bien nommée pour ça
	public class AssetService : IAssetService
	{
		private readonly AssetManager _assetManager;

		public AssetService(Context context)
		{
			_assetManager = context.Assets;
		}
		
		public async Task<Stream> ReadAsset(string path)
		{
			using (Stream inputStream = _assetManager.Open(path))
			{
				MemoryStream result = new MemoryStream();
				await inputStream.CopyToAsync(result);

				result.Seek(0, SeekOrigin.Begin);
				return result;
			}
		}
	}
}