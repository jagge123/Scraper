using HtmlAgilityPack;
using ScraperApp.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ScraperApp.Handlers
{
    public class LinkHandler : ILinkHandler
    {
        private readonly IClient _client;
        private readonly IFileHandler _fileHandler;

        public LinkHandler(IClient client, IFileHandler fileHandler)
        {
            _client = client;
            _fileHandler = fileHandler;
        }

        public async Task<List<string>> HandleLink(string link, string rootFolder)
        {
            Console.WriteLine($"Handling link: {link}");

            var links = new List<string>();
            var document = _client.Get(link, "tretton");
            await document.ContinueWith(doc =>
            {
                links = GetLinks(doc.Result);

                var folderPath = _fileHandler.CreateFolderPath(link, rootFolder);
                var fileName = _fileHandler.CreateFileName(folderPath);
                Directory.CreateDirectory(folderPath);
                _fileHandler.CreateAndWrite(doc.Result, folderPath, fileName);
            });

            await Task.WhenAll(document);
            return links;
        }

        public List<string> GetLinks(string document)
        {
            var hrefs = new List<string>();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(document);

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
