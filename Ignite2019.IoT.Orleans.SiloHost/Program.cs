using System;
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
                var azureTableConStr = context.Configuration.GetConnectionString("orleans_azure_table");

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

                var sqlServerConnStr = context.Configuration.GetConnectionString("Default");
                builder.ConfigureServices(services =>
                {
                    services.AddDbContextPool<DataContext>(
                        optionsBuilder =>
                        {
                            optionsBuilder.UseSqlServer(sqlServerConnStr);
                        });


                });
            });


            return hostBuilder.Build().StartAsync();
        }
    }
}
