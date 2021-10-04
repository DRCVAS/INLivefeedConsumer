using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using INLiveFeecd.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace INLivefeedConsumer
{
    public class Program
    {



        public static void Main(string[] args)
        {
            var TopicName = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["TopicName"];
            var GroupID = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["GroupID"];
            var BootstrapServers = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["BootstrapServers"];

            Consumer.StartConsumer(GroupID,TopicName,  BootstrapServers);
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
