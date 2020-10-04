using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ScraperApp.Http
{
    public interface IClient
    {
        Task<string> Get(string url, string namedClient);
    }
}
