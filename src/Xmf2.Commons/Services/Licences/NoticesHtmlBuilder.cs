using System;
using System.Collections.Generic;
using System.Text;
using Xmf2.Commons.Services.Licences.Models;

namespace Xmf2.Commons.Services.Licences
{
    public class NoticesHtmlBuilder
    {
        private Dictionary<Licence, String> mLicenseTextCache = new Dictionary<Licence, String>();
        private Notices _notices;
        private Notice _notice;
        private String _style;
        private bool _showFullLicenseText;

        private bool _isUrlClickable;

        public static NoticesHtmlBuilder Create()
        {
            return new NoticesHtmlBuilder();
        }

        public static NoticesHtmlBuilder Create(string style)
        {
            return new NoticesHtmlBuilder(style);
        }

        private NoticesHtmlBuilder()
        {
            _showFullLicenseText = false;
        }

        private NoticesHtmlBuilder(string style)
        {
            _style = style;
            _showFullLicenseText = false;
            _isUrlClickable = true;
        }

        public NoticesHtmlBuilder SetNotices(Notices notices)
        {
            _notices = notices;
            _notice = null;
            return this;
        }

        public NoticesHtmlBuilder SetNotice(Notice notice)
        {
            _notice = notice;
            _notices = null;
            return this;
        }

        public NoticesHtmlBuilder SetStyle(String style)
        {
            _style = style;
            return this;
        }

        public NoticesHtmlBuilder SetShowFullLicenseText(bool showFullLicenseText)
        {
            _showFullLicenseText = showFullLicenseText;
            return this;
        }

        public NoticesHtmlBuilder SetClickableUrl(bool isUrlClickable)
        {
            _isUrlClickable = isUrlClickable;
            return this;
        }

        public String Build()
        {
            StringBuilder noticesHtmlBuilder = new StringBuilder(500);
            AppendNoticesContainerStart(noticesHtmlBuilder);
            if (_notice != null)
            {
                AppendNoticeBlock(noticesHtmlBuilder, _notice);
            }
            else if (_notices != null)
            {

                foreach (var notice in _notices.AllNotices)
                {
                    AppendNoticeBlock(noticesHtmlBuilder, notice);
                }
            }
            else
            {
                throw new Exception("no notice(s) set");
            }
            AppendNoticesContainerEnd(noticesHtmlBuilder);
            return noticesHtmlBuilder.ToString();
        }

        //

        private void AppendNoticesContainerStart(StringBuilder noticesHtmlBuilder)
        {
            noticesHtmlBuilder.Append("<!DOCTYPE html><html><head>")
                .Append("<style type=\"text/css\">").Append(_style).Append("</style>")
                .Append("</head><body>");
        }

        private void AppendNoticeBlock(StringBuilder noticesHtmlBuilder, Notice notice)
        {
            noticesHtmlBuilder.Append("<ul><li>").Append(notice.Name);
            String currentNoticeUrl = notice.Url;
            if (currentNoticeUrl != null && currentNoticeUrl.Length > 0)
            {
                if (_isUrlClickable)
                {
                    noticesHtmlBuilder.Append(" (<a href=\"")
                        .Append(currentNoticeUrl)
                        .Append("\" target=\"_blank\">")
                        .Append(currentNoticeUrl)
                        .Append("</a>)");
                }
                else
                {
                    noticesHtmlBuilder.Append(" (<span>").Append(currentNoticeUrl).Append("</span>)");
                }
            }
            noticesHtmlBuilder.Append("</li></ul>");
            noticesHtmlBuilder.Append("<pre>");
            String copyright = notice.Copyright;
            if (!string.IsNullOrEmpty(copyright))
            {
                noticesHtmlBuilder.Append(copyright).Append("<br/><br/>");
            }
            noticesHtmlBuilder.Append(GetLicenseText(notice.License)).Append("</pre>");
        }

        private void AppendNoticesContainerEnd(StringBuilder noticesHtmlBuilder)
        {
            noticesHtmlBuilder.Append("</body></html>");
        }

        private String GetLicenseText(Licence license)
        {
            if (license != null)
            {
                if (!mLicenseTextCache.ContainsKey(license))
                {
                    mLicenseTextCache[license] = _showFullLicenseText ? license.GetFullText(license.LicencePathFile) : license.GetSummaryText(license.LicencePathFile);
                }
                return mLicenseTextCache[license];
            }
            return "";
        }
    }
}