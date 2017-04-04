using System;
using MvvmCross.Platform;

namespace Xmf2.Commons.MvxExtends.Licences.Models
{
    public class Licence
    {
        private ILicenceReaderService _licenceReaderService;         protected ILicenceReaderService LicenceReaderService => _licenceReaderService ?? (_licenceReaderService = Mvx.Resolve<ILicenceReaderService>());

        public virtual string LicencePathFile { get; set; } = "";

        public virtual String Name { get; set; } = "";

        public virtual String Version { get; set; } = "";

        public virtual String Url { get; set; } = "";

        private String CachedSummaryText = null;

        private String CachedFullText = null;


        public Licence()
        {
          
        }

        public String GetSummaryText(string licencePathFile)
        {
            if (CachedSummaryText == null)
            {
                CachedSummaryText = ReadSummaryTextFromPath(licencePathFile);
            }

            return CachedSummaryText;
        }

        public String GetFullText(string licencePathFile)
        {
            if (CachedFullText == null)
            {
                CachedFullText = ReadFullTextFromPath(licencePathFile);
            }

            return CachedFullText;
        }

        protected string GetContent(string licencePathFile)
        {
            return LicenceReaderService.GetContent(licencePathFile);
        }


        public string ReadFullTextFromPath(string licencePathFile)
        {
            return GetContent(licencePathFile);
        }

        public string ReadSummaryTextFromPath(string licencePathFile)
        {
            return GetContent(licencePathFile);
        }
    }
}

