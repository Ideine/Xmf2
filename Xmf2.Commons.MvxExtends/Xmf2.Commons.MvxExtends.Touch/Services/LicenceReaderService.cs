using System;
using Xmf2.Commons.MvxExtends.Licences;

namespace Xmf2.Commons.MvxExtends.Touch.Services
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
