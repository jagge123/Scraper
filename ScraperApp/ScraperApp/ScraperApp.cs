using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using HtmlAgilityPack;
using ScraperApp.Files;
using ScraperApp.Handlers;
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
        private readonly ILinkHandler _linkHandler;
        private string RootFolder;

        public ScraperApp(ILinkHandler linkHandler)
        {
            _linkHandler = linkHandler;
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
                    tasks.Add(Task.Run(() => _linkHandler.HandleLink(linkCopy, RootFolder)));
                }
                //Jag har hanterat alla dessa länkar
                HandledLinks.AddRange(LinksToHandle);
                LinksToHandle.Clear();

                await Task.WhenAll(tasks).ContinueWith(task =>
                {
                    foreach (var link in task.Result)
                    {
                        var linksToHandle = link.Except(HandledLinks).ToList();
                        LinksToHandle.AddRange(linksToHandle);
                    }

                });
                try
                {
                    Task.WaitAll(tasks.ToArray());
                }
                catch(AggregateException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return Link.Links;
        }
    }
}