using AngleSharp;
using AngleSharp.Dom;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ScraperApp
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var builder = new HostBuilder()
                          .ConfigureServices((hostContext, services) =>
                          {
                              services.AddHttpClient();
                              services.AddTransient<ScraperApp>();
                          }).UseConsoleLifetime();

            var host = builder.Build();

            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                try
                {
                    var myService = services.GetRequiredService<ScraperApp>();
                    var result = await myService.Run();

                    foreach (var res in result)
                    {
                        Console.WriteLine(res);
                    }
                    
                    Console.Read();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return 0;
        }
    }
}
