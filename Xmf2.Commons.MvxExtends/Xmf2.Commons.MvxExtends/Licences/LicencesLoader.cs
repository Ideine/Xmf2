using System;
using Xmf2.Commons.MvxExtends.Models;

namespace Xmf2.Commons.MvxExtends.Licences
{
    public static class LicencesLoader
    {
        public static String GetLicensesText(Notices notices, bool showFullLicenseText = true)
        {
            return NoticesHtmlBuilder
                    .Create()
                    .SetShowFullLicenseText(showFullLicenseText)
                    .SetStyle(LicenceStyle.LicenceDelfaultStyle)
                    .SetNotices(notices)
                    .SetClickableUrl(false)
                    .Build();
        }
    }
}
