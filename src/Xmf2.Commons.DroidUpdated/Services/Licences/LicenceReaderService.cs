using System;
using System.IO;
using Android.Content;
using Android.Content.Res;
using Xmf2.Commons.Services.Licences;

namespace Xmf2.Commons.Droid.Services.Licences
{
	public class LicenceReaderService : ILicenceReaderService
	{
		private readonly Context _context;

		private AssetManager _assetManager;

		public LicenceReaderService(Context context)
		{
			_context = context;
			_assetManager = _context.Assets;
		}

		public string GetContent(string licencePathFile)
		{
			string content;
			using (StreamReader sr = new StreamReader(_assetManager.Open(licencePathFile)))
			{
				content = sr.ReadToEnd();
			}
			return content;
		}
	}
}
