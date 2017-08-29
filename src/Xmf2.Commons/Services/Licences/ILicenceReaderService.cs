using System;
namespace Xmf2.Commons.Services.Licences
{
    public interface ILicenceReaderService
    {
        string GetContent(string licencePathFile);
    }
}
