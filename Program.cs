using Datacap.Models;
using Datacap.Middleware;
using Datacap.Models.DTO_Models;
using Datacap.Services;
using Datacap.Repositories;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// For endpoint analysis and debugging
builder.Services.AddSwaggerGen();

// Sends the filepaths down via dependency injection, no more debugging file path typos!
builder.Services.Configure<FilePaths>(builder.Configuration.GetSection("FilePaths"));

// Registering transaction repository and service
builder.Services.AddScoped<ITransactionRespository, InMemoryTransactionRepository>();
builder.Services.AddScoped<TransactionsService>();

// I decided to offer the example processors as default processors but a controller to create new processors with new details
var defaultFeeRules = new List<FeeRuleDTO>
{
    new FeeRuleDTO
    {
        ProcessorName = "TSYS",
        SmallTransactionRate = new RateDTO { FlatRate = 0.10m, PercentageRate = 0.01m },
        SmallTransactionFlatFee = 0.10m,
        LargeTransactionRate = new RateDTO { FlatRate = 0.10m, PercentageRate = 0.02m },
        LargeTransactionFlatFee = 0.10m
    },
    new FeeRuleDTO
    {
        ProcessorName = "First Data",
        SmallTransactionRate = new RateDTO { FlatRate = 0.08m, PercentageRate = 0.0125m },
        SmallTransactionFlatFee = 0.08m,
        LargeTransactionRate = new RateDTO { FlatRate = 0.90m, PercentageRate = 0.01m },
        LargeTransactionFlatFee = 0.90m
    },
    new FeeRuleDTO
    {
        ProcessorName = "EVO",
        SmallTransactionRate = new RateDTO { FlatRate = 0.09m, PercentageRate = 0.011m },
        SmallTransactionFlatFee = 0.09m,
        LargeTransactionRate = new RateDTO { FlatRate = 0.20m, PercentageRate = 0.015m },
        LargeTransactionFlatFee = 0.20m
    }
};

builder.Services.AddSingleton<FileService>();


builder.Services.AddSingleton(new ProcessorService(defaultFeeRules));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseMiddleware<ExceptionMiddleware>();
    // Log the fact that we're in production mode
    app.Logger.LogInformation("Application is in production mode");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
