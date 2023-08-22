using Serilog;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

namespace FundTransferWorker;

public static class LogConfiguration
{
    public static void ConfigureLogging()
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile(
                $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                optional: true)
            .Build();

        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .WriteTo.Debug()
            .WriteTo.Console()
            .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment))
            .Enrich.WithProperty("Environment", environment)
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
    }

    private static ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string environment)
    {
        var elasticUri = new Uri(configuration["ElasticConfiguration:Uri"]);
        var indexName = configuration["ElasticConfiguration:IndexName"];
        return new ElasticsearchSinkOptions(elasticUri)
        {
            AutoRegisterTemplate = true,
            IndexFormat = $"{indexName}-{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
        };
    }
}
