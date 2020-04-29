using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DutchTreat.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore;

namespace DutchTreat
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();
            var host = BuildWebHost(args);
            RunSeeding(host);

            host.Run();

        }

    private static void RunSeeding(IWebHost host)
    {
      var scopedFactory = host.Services.GetService<IServiceScopeFactory>();

      using (var scope = scopedFactory.CreateScope())
      {
        var seeder = scope.ServiceProvider.GetService<DutchSeeder>();
        seeder.SeedAsync().Wait();
      }
      
    }

    public static IWebHost BuildWebHost(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
              .ConfigureAppConfiguration(SetupConfiguration)
              .UseStartup<Startup>()
              .Build();

    private static void SetupConfiguration(WebHostBuilderContext ctx, IConfigurationBuilder builder)
    {
      // Removing the default configuration options
      builder.Sources.Clear();

      builder.AddJsonFile("config.json", false, true)
              .AddEnvironmentVariables();
    }
  }
}
