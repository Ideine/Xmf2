using System;
using System.Threading.Tasks;
using Xmf2.Commons.Services.Licences;

namespace Xmf2.Commons.iOS.Services
{
    public class LicenceReaderService : ILicenceReaderService
    {

        public LicenceReaderService()
        {

        }

        //TODO: revoir pour faire du async
        public string GetContent(string licencePathFile)
        {
            var content = System.IO.File.ReadAllText(licencePathFile);
            return content;
        }
    }
}
