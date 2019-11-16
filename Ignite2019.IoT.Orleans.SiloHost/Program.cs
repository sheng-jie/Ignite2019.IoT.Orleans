using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Ignite2019.IoT.Orleans.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            builder.ConfigureServices((context, collection) =>
            {
                collection.AddDbContextPool<DataContext>(
                    optionsBuilder =>
                    {
                        var connStr = context.Configuration.GetConnectionString("Default");
                        optionsBuilder.UseSqlServer(connStr);
                    });


            });

                return builder.Build().StartAsync();
            }
    }
    }
