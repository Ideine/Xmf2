using System.IO;
using Windows.Storage;
using Xmf2.Commons.Services.Licences;

namespace Xmf2.Commons.UWP.Services
{
    public class LicenceReaderService : ILicenceReaderService
    {
        public string GetContent(string licencePathFile)
        {
            StorageFolder installationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;

            string resourcePath = $"{installationFolder.Path }\\Assets\\{licencePathFile}";

            using (Stream inputStream = File.OpenRead(resourcePath))
            {
                string content;

                using (StreamReader sr = new StreamReader(inputStream))
                {
                    content = sr.ReadToEnd();
                }
                return content;
            }

        }
    }
}
