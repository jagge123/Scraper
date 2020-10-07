using AngleSharp;
using AngleSharp.Dom;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ScraperApp.Http;
using System;
using System.Configuration;
using System.Diagnostics;
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
                          services.AddHttpClient("tretton", t =>
                          {
                              t.BaseAddress = new Uri("https://tretton37.com");
                          });
                              services.AddTransient<ScraperApp>();
                              services.AddTransient<IClient, Client>();
                          }).UseConsoleLifetime();

            var host = builder.Build();

            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                try
                {
                    var myService = services.GetRequiredService<ScraperApp>();
                    var stopWatch = new Stopwatch();
                    stopWatch.Start();
                    var result = await myService.Run();
                    stopWatch.Stop();
                    foreach (var res in result)
                    {
                        Console.WriteLine(res);
                    }
                    Console.WriteLine($"Time elapsed: {stopWatch.ElapsedMilliseconds} ms");
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
