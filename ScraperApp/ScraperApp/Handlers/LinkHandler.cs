using ScraperApp.Files;
using ScraperApp.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ScraperApp.Handlers
{
    public class LinkHandler : ILinkHandler
    {
        private readonly IClient _client;
        public LinkHandler(IClient client)
        {
            _client = client;
        }

        public async Task<List<string>> HandleLink(string link, string rootFolder)
        {
            Console.WriteLine($"Handling link: {link}");

            var links = new List<string>();
            var document = _client.Get(link, "tretton");
            await document.ContinueWith(doc =>
            {
                var fileHandler = new FileHandler(rootFolder);
                links = Link.GetLinks(doc.Result);
                Console.WriteLine("Got linkes for link: {0}", link);

                var folderPath = fileHandler.CreateFolderPath(link);
                var fileName = fileHandler.CreateFileName(folderPath);
                Directory.CreateDirectory(folderPath);
                fileHandler.CreateAndWrite(doc.Result, folderPath, fileName);
            });

            await Task.WhenAll(document);
            return links;
        }
    }
}
