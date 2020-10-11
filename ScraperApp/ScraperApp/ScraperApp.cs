using ScraperApp.Handlers;
using ScraperApp.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScraperApp
{
    public class ScraperApp
    {
        public HashSet<string> LinksToHandle = new HashSet<string>() { "" };
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
            var rootFolder = FileHandler.CreateRootFolder("tretton37");
            while(LinksToHandle.Count > 0)
            {
                var tasks = new List<Task<List<string>>>();
                foreach (var link in LinksToHandle)
                {
                    var linkCopy = link;
                    tasks.Add(Task.Run(() => _linkHandler.HandleLink(linkCopy, rootFolder)));
                }
                //Jag har hanterat alla dessa länkar
                HandledLinks.AddRange(LinksToHandle);
                LinksToHandle.Clear();

                await Task.WhenAll(tasks).ContinueWith(task =>
                {
                    foreach (var links in task.Result)
                    {
                        foreach (var link in links)
                        {
                            link.RemoveHtmlEnding();
                        }
                        var linksToHandle = links.Except(HandledLinks).ToList();
                        LinksToHandle.UnionWith(linksToHandle);
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
            return HandledLinks;
        }
    }
}