using System;
using System.Collections.Generic;
using Xmf2.Commons.MvxExtends.Licences.Models;

namespace Xmf2.Commons.MvxExtends.Models
{
    public class Notices
    {
        public List<Notice> AllNotices { get; set; }

        public Notices()
        {
            AllNotices = new List<Notice>();
        }

        public void AddNotice(Notice notice)
        {
            AllNotices.Add(notice);
        }
    }
}