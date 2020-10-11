using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ScraperApp.Handlers
{
    public interface ILinkHandler
    {
        Task<List<string>> HandleLink(string link, string rootFolder);
    }
}
