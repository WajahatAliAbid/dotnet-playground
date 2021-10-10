using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;

namespace ESLogging
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var logger = LoggerFactory.Create(options => 
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
                        CustomFormatter = new ExceptionAsObjectJsonFormatter(renderMessage:true)
                    });
                options.AddSerilog(loggerConfig.CreateLogger());
            }).CreateLogger<Program>();


            logger.LogInformation("I am a person");
            logger.LogInformation("I am {@Person}", new 
            {
                FirstName = "Wajahat",
                LastName = "Ali"
            });
            try
            {
                throw new ArgumentException("Testing argument not found");
            }
            catch (Exception ex)
            {
                Console.WriteLine("exception happened");
                logger.LogCritical(ex,"Argument exception happened");
            }
            
            await Task.Delay(4000);
            Console.WriteLine("Hello World!");
        }
    }
}
