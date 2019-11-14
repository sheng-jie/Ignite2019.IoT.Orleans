using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace Ignite2019.IoT.Orleans.SiloHost
{
    class Program
    {
        static Task Main(string[] args)
        {
            var builder = new SiloHostBuilder();
            builder.ConfigureAppConfiguration((configurationBuilder =>
            {
                configurationBuilder.AddJsonFile("appsettings.json");
            }));


            builder.UseAzureStorageClustering(options => { options.ConnectionString = ""; })
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = typeof(Program).Namespace;
                    options.ServiceId = typeof(Program).Namespace;
                })
                .AddAzureTableGrainStorageAsDefault(options =>
                {
                    options.ConnectionString = "";
                    options.UseJson = true;
                })
                .UseAzureTableReminderService(configure => configure.ConnectionString = "");
            builder.ConfigureApplicationParts(parts => parts.AddFromApplicationBaseDirectory().WithReferences());

            builder.UseDashboard(options =>
            {
                options.Host = "*";
                options.Port = 8080;
                options.HostSelf = true;
                options.CounterUpdateIntervalMs = 10000;
            });

            return builder.Build().StartAsync();
        }
    }
}
