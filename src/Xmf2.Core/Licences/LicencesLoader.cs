using System;
using System.Threading.Tasks;
using Xmf2.Core.Licences.Models;

namespace Xmf2.Core.Licences
{
	public static class LicencesLoader
	{
		public static Task<string> GetLicensesText(Notices notices, bool showFullLicenseText = true)
		{
			return NoticesHtmlBuilder
					.Create()
					.SetShowFullLicenseText(showFullLicenseText)
					.SetStyle(LicenceStyle.LicenceDefaultStyle)
					.SetNotices(notices)
					.SetClickableUrl(false)
					.Build();
		}
	}
}