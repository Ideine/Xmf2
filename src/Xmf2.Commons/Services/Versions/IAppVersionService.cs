using System;

namespace Xmf2.Commons.Services.Versions
{
	public interface IAppVersionService
	{
		string GetVersion();

        string GetBuildVersion();
    }
}
