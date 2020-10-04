using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using HtmlAgilityPack;
using ScraperApp.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ScraperApp
{
    public class ScraperApp
    {
        private readonly IClient _client;
        public ScraperApp(IClient client)
        {
            _client = client;
        }

        public async Task<List<string>> Run()
        {
            var document = await _client.Get("/", "tretton");
            Console.WriteLine("Document GET");

            var rootPath = CreateDirectory("tretton37");
            Console.WriteLine("Directory Created!");
            await SaveToDisk(Path.Combine(rootPath,"tretton.html"), document);
            Console.WriteLine("File Saved to disk!");

            var links = Link.GetLinks(document);
            Console.WriteLine("Links!");
            return links;
        }

        public string CreateDirectory(string path)
        {
            var workingDir = Environment.CurrentDirectory;
            var projectDirectory = Directory.GetParent(workingDir).Parent.Parent.FullName;
            var rootPath = Path.Combine(projectDirectory, path);
            Directory.CreateDirectory(rootPath);
            return rootPath;
        }

        public Task SaveToDisk(string fileName, string document)
        {
            if (!File.Exists(fileName))
            {
                File.WriteAllText(fileName, document);
            }
            return Task.CompletedTask;
        }
    }
}
