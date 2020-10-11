using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScraperApp.Handlers
{
    public interface ILinkHandler
    {
        Task<List<string>> HandleLink(string link, string rootFolder);
        List<string> GetLinks(string document);
    }
}
