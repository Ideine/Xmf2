using System;
using System.IO;
using System.Threading.Tasks;
using Android.Content;
using Android.Content.Res;
using Xmf2.Core.Licences;

namespace Xmf2.Core.Droid.Services
{
	public class LicenceReaderService : ILicenceReaderService
	{
		private readonly AssetManager _assetManager;

		public LicenceReaderService(Context context)
		{
			_assetManager = context.Assets;
		}

		public async Task<string> GetContent(string licencePathFile)
		{
			string content;
			using (StreamReader sr = new StreamReader(_assetManager.Open(licencePathFile)))
			{
				content = await sr.ReadToEndAsync();
			}
			return content;
		}
	}
}