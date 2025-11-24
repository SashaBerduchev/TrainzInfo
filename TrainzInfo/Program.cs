using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TrainzInfo.Tools;

namespace TrainzInfo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LoggingExceptions.CreateFolder();
            LoggingExceptions.Init("Program", nameof(Main));
            LoggingExceptions.Start();
            LoggingExceptions.Wright("Try create host builder");
            CreateHostBuilder(args).Build().Run();
            LoggingExceptions.Wright("Host builder created");
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
