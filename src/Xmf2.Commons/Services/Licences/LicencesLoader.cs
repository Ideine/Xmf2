using System;
using Xmf2.Commons.Services.Licences.Models;

namespace Xmf2.Commons.Services.Licences
{
    public static class LicencesLoader
    {
        public static String GetLicensesText(Notices notices, bool showFullLicenseText = true)
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
