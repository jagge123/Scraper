using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using HtmlAgilityPack;
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
            CreateRootFolder("tretton37");


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
            var document = GetDocument(link);
            await document.ContinueWith(doc =>
            {
                links = GetLinks(doc.Result);
                Console.WriteLine("Got linkes for link: {0}", link);
                CreateDirAndSave(link, link, doc.Result);
            });
            await Task.WhenAll(document);
            return links;
        }

        public async Task<string> GetDocument(string path)
        {
            //Hämta HTML
            return await _client.Get(path, "tretton");          
        }

        public List<string> GetLinks(string document)
        {
            return Link.GetLinks(document);
        }

        public void CreateRootFolder(string folderName)
        {
            var workingDir = Environment.CurrentDirectory;
            var projectDirectory = Directory.GetParent(workingDir).Parent.Parent.FullName;
            var rootPath = Path.Combine(projectDirectory, folderName);
            Directory.CreateDirectory(rootPath);
            RootFolder = rootPath;
        }

        public void CreateDirAndSave(string path, string fileName, string document)
        {
            var newPath = path.Replace("/", "");
            var folderPath = Path.Combine(RootFolder, newPath);
            Directory.CreateDirectory(folderPath);
            //Save file

            var newName = fileName.Replace("/", "") + ".html";
            var existingName = Path.Combine(folderPath, newName);
            if (!File.Exists(existingName))
            {
                File.WriteAllText(existingName, document);
            }            
        }
    }
}
