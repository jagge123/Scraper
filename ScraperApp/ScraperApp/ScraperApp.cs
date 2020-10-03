using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ScraperApp
{
    public class ScraperApp
    {
        private readonly IHttpClientFactory _httpClient;
        public ScraperApp(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<string>> Run()
        {
            var scraper = new PathParser();
            //Get page
            var client = _httpClient.CreateClient();
            var response = await client.GetAsync("http://tretton37.com/");
            var result = await response.Content.ReadAsStringAsync();
            //Get a link paths in page
            var links = scraper.GetPaths(result);
            return links;
        }
    }

    public class PathParser
    {
        public List<string> GetPaths(string doc)
        {
            var hrefs = new List<string>();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(doc);

            foreach (HtmlNode link in htmlDoc.DocumentNode.SelectNodes("//a[@href]"))
            {
                string hrefValue = link.GetAttributeValue("href", string.Empty);
                //Simple way to remove javascript etc...
                if(hrefValue.StartsWith("/"))
                    hrefs.Add(hrefValue);
            }
            return hrefs;
        }
    }
}
