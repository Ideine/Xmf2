using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Xmf2.Commons.Services.Resources;

namespace Xmf2.Commons.UWP.Services
{
    public class AssetService : IAssetService
    {
        public async Task<Stream> ReadAsset(string fileName)
        {
            StorageFolder installationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;

            string resourcePath = $"{installationFolder.Path }\\Assets\\{fileName}";

            using (Stream inputStream = File.OpenRead(resourcePath))
            {
               var result = new MemoryStream();

                await inputStream.CopyToAsync(result);

                result.Seek(0, SeekOrigin.Begin);
                return result;
            }

        }
    }
}
