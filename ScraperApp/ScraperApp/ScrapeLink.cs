using System;
using System.Collections.Generic;
using System.Text;

namespace ScraperApp
{
    public class ScrapeLink
    {
        public string Link { get; set; }
        public string FolderPath { get; set; }

        public ScrapeLink(string link)
        {
            Link = link;
        }
    }
}
