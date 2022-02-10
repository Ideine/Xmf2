using System.IO;
using Android.Content;
using Android.Content.Res;
using Xmf2.Commons.MvxExtends.Licences;

namespace Xmf2.Commons.MvxExtends.Droid.Services
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
