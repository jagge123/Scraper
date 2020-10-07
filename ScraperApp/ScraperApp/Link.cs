using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScraperApp
{
    public static class Link
    {
        //Store all links already fetched
        public static List<string> Links = new List<string>();
        public static List<string> GetLinks(string doc)
        {
            var hrefs = new List<string>();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(doc);

            foreach (HtmlNode link in htmlDoc.DocumentNode.SelectNodes("//a[@href]"))
            {
                string hrefValue = link.GetAttributeValue("href", string.Empty);
                //Simple way to remove javascript google maps etc...
                if (hrefValue.StartsWith("/") && !hrefValue.EndsWith("/") && !hrefValue.StartsWith("//"))
                    hrefs.Add(hrefValue);
            }
            return hrefs;
        }
    }
}
