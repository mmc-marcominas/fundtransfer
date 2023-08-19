using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using FundTransfer;
using FundTransfer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<ITransactionsService, TransactionsService>();
builder.Services.AddSingleton<TransactionsDatabaseService>();
builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("TransactionDatabase"));

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
