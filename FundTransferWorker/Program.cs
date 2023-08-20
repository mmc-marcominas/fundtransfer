using FundTransferWorker.Services;
using Polly;
using Polly.Extensions.Http;

namespace FundTransferWorker;

class Program
{
    static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            .WaitAndRetryAsync(6, retryAttempt =>
                                  TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.Configure<AccountApiSettings>(hostContext.Configuration.GetSection("AccountApi"));
                services.Configure<QueueSettings>(hostContext.Configuration.GetSection("TransactionQueue"));
                services.Configure<DatabaseSettings>(hostContext.Configuration.GetSection("TransactionDatabase"));

                services.AddHttpClient<FundTransferService>()
                        .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                        .AddPolicyHandler(GetRetryPolicy());
                services.AddScoped<FundTransferService>();
                services.AddSingleton<TransactionsDatabaseService>();
                services.AddHostedService<Worker>();
            });
}
