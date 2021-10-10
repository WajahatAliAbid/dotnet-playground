using System;
using System.Text.Json;
using System.Threading.Tasks;
using Serilog;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;

namespace ESLogging
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var loggerConfig = new LoggerConfiguration();
            loggerConfig.WriteTo
                .Console(new ElasticsearchJsonFormatter());
            loggerConfig.WriteTo
                .Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200/"))
                {
                    IndexFormat = "serilog-{0:yyyy}",
                    NumberOfShards = 3,
                    NumberOfReplicas = 1,
                    AutoRegisterTemplate = true,
                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7, 
                    FailureCallback = e => Console.WriteLine("Unable to submit event " + e.MessageTemplate),
                    EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                                       EmitEventFailureHandling.WriteToFailureSink |
                                       EmitEventFailureHandling.RaiseCallback,
                });
            var logger = loggerConfig.CreateLogger();

            logger.Information("I am a person");
            logger.Information("I am {@Person}", new 
            {
                FirstName = "Wajahat",
                LastName = "Ali"
            });


            await Task.Delay(2300);
            Console.WriteLine("Hello World!");
        }
    }
}
