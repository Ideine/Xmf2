using System;
namespace Xmf2.Commons.Services.Licences.Models
{
    public class Notice
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Copyright { get; set; }
        public Licence License { get; set; }
       
        public Notice(String name, String url, String copyright, Licence license)
        {
            Name = name;
            Url = url;
            Copyright = copyright;
            License = license;
        }
    }
}