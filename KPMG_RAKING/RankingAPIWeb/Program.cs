using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RankingAPIWeb
{
    public class Program
    {


        public static void Main(string[] args)
        {
            Task t = Task.Run(Startup);
            if (t.IsCompleted.Equals(true))
            {
                Task.Run(Startup);
            }
            CreateHostBuilder(args).Build().Run();
        }

        private static void Startup()
        {
            new BLL.AutoSaveCached();
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
