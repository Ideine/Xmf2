﻿using System;
using System.Collections.Generic;

namespace Xmf2.Commons.Services.Licences.Models
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