using System;
using Xmf2.Commons.Services.Licences;

namespace Xmf2.Commons.iOS.Services
{
    public class LicenceReaderService : ILicenceReaderService
    {

        public LicenceReaderService()
        {

        }

        public string GetContent(string licencePathFile)
        {
            var content = System.IO.File.ReadAllText(licencePathFile);
            return content;
        }
    }
}
