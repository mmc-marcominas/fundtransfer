using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace FundTransferWorker;

public static class LogConfiguration
{
    public static void ConfigureLogging()
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Undefined";
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .Build();
        var settings = configuration.GetSection("ElasticSearchConfiguration").Get<ElasticSearchSettings>();

        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .WriteTo.Debug()
            .WriteTo.Console()
            .WriteTo.Elasticsearch(ConfigureElasticSink(settings, environment))
            .Enrich.WithProperty("Environment", environment)
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
    }

    private static ElasticsearchSinkOptions ConfigureElasticSink(ElasticSearchSettings? settings, string environment)
    {
        if (settings == null)
            throw new ArgumentNullException(nameof(settings));

        var elasticUri = new Uri(settings.Endpoint);
        return new ElasticsearchSinkOptions(elasticUri)
        {
            AutoRegisterTemplate = true,
            IndexFormat = $"{settings.IndexName}-{settings.ApplicationName}-{environment.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
        };
    }
}
