# Serilog with Amazon Elasticsearch

## Dependencies Required
- AWSSDK.Extensions.NetCore.Setup
- Elasticsearch.Net.Aws
- Serilog.Formatting.Elasticsearch
- Serilog.Sinks.Elasticsearch

## Setup
Create logger configuration as follows
```csharp
var loggerConfiguration = new LoggerConfiguration()
    .WriteTo
    .Elasticsearch(new ElasticsearchSinkOptions(new Uri(hostContext.Configuration.GetValue<string>("ES_URL")))
    {
        Connection  = new AwsHttpConnection(Configuration.GetAWSOptions()),
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
var logger = loggerConfiguration.CreateLogger();
```
Here, we are using AwsHttpConnection provided by [Brandon](https://github.com/bcuff/elasticsearch-net-aws) which wires up Elasticsearch.net with Amazon Version 4 Signing process. We get AWSOptions from the [IConfiguration](https://www.nuget.org/packages/Microsoft.Extensions.Configuration/) interface. We use official Serilog Sink for Elasticsearch and add additional formatter `Serilog.Formatting.Elasticsearch`. This formatter takes care of formatting things for Elasticsearch.

## Note
Be sure to call following function at the exit of the program. 
```csharp
Serilog.Log.CloseAndFlush();
```

This function ensures all logs are written to Elasticsearch before program exits. Otherwise, there will be logs that won't be able to make it to Elasticsearch.