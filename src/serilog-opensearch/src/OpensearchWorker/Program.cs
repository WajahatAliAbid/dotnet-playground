using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elasticsearch.Net.Aws;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;

namespace OpensearchWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(opt => opt.AddUserSecrets<Program>())
                .ConfigureLogging((hostContext, logging) => 
                {
                    var loggerConfiguration = new LoggerConfiguration()
                        .WriteTo
                        .Elasticsearch(new ElasticsearchSinkOptions(new Uri(hostContext.Configuration.GetValue<string>("ES_URL")))
                        {
                            Connection  = new AwsHttpConnection(hostContext.Configuration.GetAWSOptions()),
                            IndexFormat = "serilog-{0:yyyy}",
                            NumberOfShards = 1,
                            NumberOfReplicas = 1,
                            AutoRegisterTemplate = true,
                            AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7, 
                            FailureCallback = e => Console.WriteLine("Unable to submit event " + e.MessageTemplate),
                            EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                                            EmitEventFailureHandling.WriteToFailureSink |
                                            EmitEventFailureHandling.RaiseCallback,
                            CustomFormatter = new ExceptionAsObjectJsonFormatter(renderMessage:true)
                        });
                    logging.AddSerilog(loggerConfiguration.CreateLogger(), dispose: true);
                    
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                });
    }
}
