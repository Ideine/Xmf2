using System.IO;
using System.Threading.Tasks;
using Xmf2.Core.Licences;

namespace Xmf2.Core.iOS.Services
{
	public class LicenceReaderService : ILicenceReaderService
	{
		public async Task<string> GetContent(string licencePathFile)
		{
			string content;
			using (var stream = File.OpenRead(licencePathFile))
			{
				using (var reader = new StreamReader(stream))
				{
					content = await reader.ReadToEndAsync();
				}
			}
			return content;
		}
	}
}