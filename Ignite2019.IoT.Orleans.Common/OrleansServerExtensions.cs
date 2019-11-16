using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Orleans.Configuration;
using Orleans.Hosting;

namespace Ignite2019.IoT.Orleans.Common
{
    public static class OrleansServerExtensions
    {
        public static IHostBuilder AddOrleansServer(this IHostBuilder hostBuilder)
        {
            hostBuilder.


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
                        options.ClusterId = typeof(Program).Namespace;
                        options.ServiceId = typeof(Program).Namespace;
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
        }
    }
}
