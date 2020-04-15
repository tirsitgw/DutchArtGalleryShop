using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DutchTreat
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(SetupConfiguration)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

    private static void SetupConfiguration(HostBuilderContext ctx, IConfigurationBuilder builder)
    {
      //throw new NotImplementedException();
      // Removing the default configuraiton options
      builder.Sources.Clear();

      builder.AddJsonFile("config.json", false, true)
              .AddEnvironmentVariables();


    }
  }
}
