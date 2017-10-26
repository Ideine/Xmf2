using System;

namespace Xmf2.Commons.Services.Licences.Models
{
    public class Licence
    {
        private readonly ILicenceReaderService _licenceReaderService;

        public virtual string LicencePathFile { get; set; } = "";

        public virtual string Name { get; set; } = "";

        public virtual string Version { get; set; } = "";

        public virtual string Url { get; set; } = "";

        private string CachedSummaryText = null;

        private string CachedFullText = null;

        public Licence(ILicenceReaderService readerService)
        {
            _licenceReaderService = readerService;
        }

        public string GetSummaryText(string licencePathFile)
        {
            return CachedSummaryText ?? (CachedSummaryText = ReadSummaryTextFromPath(licencePathFile));
        }

        public string GetFullText(string licencePathFile)
        {
            return CachedFullText ?? (CachedFullText = ReadFullTextFromPath(licencePathFile));
        }

        protected string GetContent(string licencePathFile)
        {
            return _licenceReaderService.GetContent(licencePathFile);
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

