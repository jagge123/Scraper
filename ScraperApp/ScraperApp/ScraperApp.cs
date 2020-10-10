using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using HtmlAgilityPack;
using ScraperApp.Files;
using ScraperApp.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public List<string> LinksToHandle = new List<string>() { "" };
        public List<string> HandledLinks = new List<string>();
        private readonly IClient _client;
        private string RootFolder;
        public ScraperApp(IClient client)
        {
            _client = client;
        }

        public async Task<List<string>> Run()
        {
            //Create root
            RootFolder = FileHandler.CreateRootFolder("tretton37");
            while(LinksToHandle.Count > 0)
            {
                var tasks = new List<Task<List<string>>>();
                foreach (var link in LinksToHandle)
                {
                    var linkCopy = link;
                    tasks.Add(Task.Run(() => HandleLink(linkCopy)));
                }
                var taskResults = Task.WhenAll(tasks);
                try
                {
                    taskResults.Wait();
                }
                catch (AggregateException e)
                {
                    Console.WriteLine(e.Message);
                }
                //Jag har hanterat alla dessa länkar
                HandledLinks.AddRange(LinksToHandle);
                LinksToHandle.Clear();
                //Kan jag hantera dessa parallelt?
                foreach (var taskResult in taskResults.Result)
                {
                    //Jag behöver hantera dessa länkarna
                    var linksToHandle = taskResult.Except(HandledLinks).ToList();
                    LinksToHandle.AddRange(linksToHandle);
                }
            }
            return Link.Links;
        }

        public async Task<List<string>> HandleLink(string link)
        {
            Console.WriteLine($"Handling link: {link}");

            var links = new List<string>();
            var document = _client.Get(link, "tretton");
            await document.ContinueWith(doc =>
            {
                var fileHandler = new FileHandler(RootFolder);
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