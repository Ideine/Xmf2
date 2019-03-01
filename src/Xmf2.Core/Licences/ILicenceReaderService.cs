using System;
using System.Threading.Tasks;

namespace Xmf2.Core.Licences
{
	public interface ILicenceReaderService
	{
		Task<string> GetContent(string licencePathFile);
	}
}