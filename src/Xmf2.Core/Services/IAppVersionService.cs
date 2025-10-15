namespace Xmf2.Core.Services
{
	public interface IAppVersionService
	{
		/// <summary>
		/// Should return short version (eg: 1.2)
		/// For android: versionName
		/// For iOS: short version with two digits
		/// </summary>
		/// <returns></returns>
		string GetVersion();

		/// <summary>
		/// Should return full version (eg: 1.2.3)
		/// For android: versionName.versionCode
		/// For iOS: full version with three digits
		/// </summary>
		/// <returns></returns>
		string GetFullVersion();
	}
}