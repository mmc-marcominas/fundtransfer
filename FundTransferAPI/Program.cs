using Microsoft.Extensions.DependencyInjection;
using Serilog;

using FundTransfer;
using FundTransfer.Services;

var builder = WebApplication.CreateBuilder(args);

LogConfiguration.ConfigureLogging();
builder.Host.UseSerilog();

// Add services to the container. Singleton order matters, pay attention on it.
builder.Services.AddSingleton<ITransactionsQueueService, TransactionsQueueService>();
builder.Services.AddSingleton<ITransactionsService, TransactionsService>();
builder.Services.AddSingleton<ITransactionsDatabaseService, TransactionsDatabaseService>();
builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("TransactionDatabase"));
builder.Services.Configure<QueueSettings>(
    builder.Configuration.GetSection("TransactionQueue"));

// Configure o logging
builder.Services.AddLogging();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRequestIdMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
