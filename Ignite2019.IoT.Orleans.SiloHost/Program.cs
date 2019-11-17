using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Ignite2019.IoT.Orleans.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using WalkingTec.Mvvm.Core;

namespace Ignite2019.IoT.Orleans.SiloHost
{
    class Program
    {
        static Task Main(string[] args)
        {
            Console.Title = "Silo.Host";

            var hostBuilder = new HostBuilder();

            hostBuilder.ConfigureAppConfiguration(configurationBuilder =>
            {
                configurationBuilder.AddJsonFile("appsettings.json");
            });


            hostBuilder.UseOrleans((context, builder) =>
            {
                Configs con = context.Configuration.Get<Configs>() ?? new Configs();

                var azureTableConStr = con.ConnectionStrings.FirstOrDefault(cs => cs.Key == "orleans_azure_table")?.Value;

                builder.UseAzureStorageClustering(options =>
                    {
                        options.ConnectionString = azureTableConStr;
                        options.TableName = "Cluster";
                    }).ConfigureEndpoints(siloPort: 11111, gatewayPort: 30000)
                    .Configure<ClusterOptions>(options =>
                    {
                        options.ClusterId = "Ignite.IoT.Orleans";
                        options.ServiceId = "Ignite.IoT.Orleans";
                    });

                builder.AddAzureTableGrainStorageAsDefault(options =>
                {
                    options.ConnectionString = azureTableConStr;
                    options.TableName = "Storage";
                    options.UseJson = true;
                });

                builder.UseAzureTableReminderService(configure =>
                {
                    configure.ConnectionString = azureTableConStr;
                    configure.TableName = "Reminder";
                });

                builder.ConfigureLogging(loggingBuilder => loggingBuilder.AddConsole());


                builder.ConfigureApplicationParts(parts => parts.AddFromApplicationBaseDirectory().WithReferences());

                builder.UseDashboard(options =>
                {
                    options.Host = "*";
                    options.Port = 8080;
                    options.HostSelf = true;
                    options.CounterUpdateIntervalMs = 10000;
                });

                var sqlServerConnStr = con.ConnectionStrings.FirstOrDefault(cs => cs.Key == "default")?.Value;

                builder.ConfigureServices(services =>
                {
                    services.AddSingleton<Configs>(con);
                    services.AddDbContextPool<DbContext>(optionsBuilder =>
                             optionsBuilder.UseSqlServer(sqlServerConnStr)
                    );

                    //services.AddDbContext<DataContext>(
                    //    optionsBuilder =>
                    //    {
                    //        optionsBuilder.UseSqlServer(sqlServerConnStr);
                    //    });
                    GlobalServices.SetServiceProvider(services.BuildServiceProvider());
                });
            });


            return hostBuilder.Build().StartAsync();
        }
    }
}
