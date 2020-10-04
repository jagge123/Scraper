using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ScraperApp.Http
{
    public class Client : IClient
    {
        private readonly IHttpClientFactory _httpClient;

        public Client(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> Get(string url, string namedClient)
        {
            var client = _httpClient.CreateClient(namedClient);
            var response = await client.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
