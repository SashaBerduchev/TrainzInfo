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
            Log.CreateFolder();
            Log.Init("Program", nameof(Main));
            Log.Start();
            Log.Wright("Try create host builder");
            CreateHostBuilder(args).Build().Run();
            Log.Wright("Host builder created");
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
