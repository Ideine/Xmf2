using System;
using System.Threading.Tasks;

namespace Xmf2.Core.Licences.Models
{
	public class Licence
	{
		private readonly ILicenceReaderService _licenceReaderService;

		public virtual string LicencePathFile { get; set; }

		public virtual string Name { get; set; }

		public virtual string Version { get; set; }

		public virtual string Url { get; set; }

		private string CachedSummaryText = null;

		private string CachedFullText = null;

		public Licence(ILicenceReaderService readerService)
		{
			_licenceReaderService = readerService;
		}

		public async Task<string> GetSummaryText(string licencePathFile)
		{
			return CachedSummaryText ?? (CachedSummaryText = await ReadSummaryTextFromPath(licencePathFile));
		}

		public async Task<string> GetFullText(string licencePathFile)
		{
			return CachedFullText ?? (CachedFullText = await ReadFullTextFromPath(licencePathFile));
		}

		protected Task<string> GetContent(string licencePathFile)
		{
			return _licenceReaderService.GetContent(licencePathFile);
		}

		public Task<string> ReadFullTextFromPath(string licencePathFile)
		{
			return GetContent(licencePathFile);
		}

		public Task<string> ReadSummaryTextFromPath(string licencePathFile)
		{
			return GetContent(licencePathFile);
		}
	}
}