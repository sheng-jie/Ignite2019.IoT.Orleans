using System;
using System.Threading;
using System.Threading.Tasks;
using Ignite2019.IoT.Orleans.Grains;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Runtime;

namespace Ignite2019.IoT.Orleans
{
    public class ClusterClientHostedService : IHostedService
    {
        private readonly ILogger<ClusterClientHostedService> _logger;

        public ClusterClientHostedService(IConfiguration configuration,
            ILogger<ClusterClientHostedService> logger,
            ILoggerProvider loggerProvider)
        {
            var clusterConnStr = configuration.GetValue<string>("AppSettings:orleans_sql_server");
            _logger = logger;

            var clientBuilder = UseSqlServerOrleansClient(clusterConnStr);
            //var clientBuilder = UseAzureOrleansClient(clusterConnStr);

            this.Client = clientBuilder.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(IUniqueIdGenerator).Assembly))
                .ConfigureLogging(builder => builder.AddProvider(loggerProvider))
                .Build();
        }



        private IClientBuilder UseSqlServerOrleansClient(string clusterConnStr)
        {
            return new ClientBuilder()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "Ignite.IoT.Orleans";
                    options.ServiceId = "Ignite.IoT.Orleans";
                })
                .UseAdoNetClustering(option =>
                {
                    option.Invariant = "System.Data.SqlClient";
                    option.ConnectionString = clusterConnStr;
                });
        }

        private IClientBuilder UseAzureOrleansClient(string clusterConnStr)
        {
            return new ClientBuilder()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "Ignite.IoT.Orleans";
                    options.ServiceId = "Ignite.IoT.Orleans";
                })
                .UseAzureStorageClustering(option =>
                {
                    option.TableName = "Cluster";
                    option.ConnectionString = clusterConnStr;
                });
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var attempt = 0;
            var maxAttempts = 100;
            var delay = TimeSpan.FromSeconds(1);
            return Client.Connect(async error =>
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return false;
                }

                if (++attempt < maxAttempts)
                {
                    _logger.LogWarning(error,
                        "Failed to connect to Orleans cluster on attempt {@Attempt} of {@MaxAttempts}.",
                        attempt, maxAttempts);

                    try
                    {
                        await Task.Delay(delay, cancellationToken);
                    }
                    catch (OperationCanceledException)
                    {
                        return false;
                    }

                    return true;
                }
                else
                {
                    _logger.LogError(error,
                        "Failed to connect to Orleans cluster on attempt {@Attempt} of {@MaxAttempts}.",
                        attempt, maxAttempts);

                    return false;
                }
            });
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                await Client.Close();
            }
            catch (OrleansException error)
            {
                _logger.LogWarning(error, "Error while gracefully disconnecting from Orleans cluster. Will ignore and continue to shutdown.");
            }
        }

        public IClusterClient Client { get; }
    }
}
