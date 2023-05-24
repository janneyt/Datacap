using Datacap.Models;
using Datacap.Middleware;
using Datacap.Models.DTO_Models;
using Datacap.Services;
using Datacap.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;

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

// Registering FluentValidation
builder.Services.AddControllersWithViews().AddFluentValidation(fv =>
{
    fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
});

// Initialize default processors with default fee rules
var defaultProcessors = new List<ProcessorDTO>
{
    new ProcessorDTO
    {
        ProcessorName = "TSYS",
        FeeRule = new FeeRuleDTO
        {
            ProcessorName = "TSYS",
            SmallTransactionRate = new RateDTO { FlatRate = 0.10m, PercentageRate = 0.01m },
            SmallTransactionFlatFee = 0.10m,
            LargeTransactionRate = new RateDTO { FlatRate = 0.10m, PercentageRate = 0.02m },
            LargeTransactionFlatFee = 0.10m
        },
        Transactions = new List<TransactionDTO>(), // Initialize transaction list
    },
    new ProcessorDTO
    {
        ProcessorName = "First Data",
        FeeRule = new FeeRuleDTO
        {
            ProcessorName = "First Data",
            SmallTransactionRate = new RateDTO { FlatRate = 0.08m, PercentageRate = 0.0125m },
            SmallTransactionFlatFee = 0.08m,
            LargeTransactionRate = new RateDTO { FlatRate = 0.90m, PercentageRate = 0.01m },
            LargeTransactionFlatFee = 0.90m
        },
        Transactions = new List<TransactionDTO>(), // Initialize transaction list
    },
    new ProcessorDTO
    {
        ProcessorName = "EVO",
        FeeRule = new FeeRuleDTO
        {
            ProcessorName = "EVO",
            SmallTransactionRate = new RateDTO { FlatRate = 0.09m, PercentageRate = 0.011m },
            SmallTransactionFlatFee = 0.09m,
            LargeTransactionRate = new RateDTO { FlatRate = 0.20m, PercentageRate = 0.015m },
            LargeTransactionFlatFee = 0.20m
        },
        Transactions = new List<TransactionDTO>(), // Initialize transaction list
    }
};

// Registering processor service with the default processors
builder.Services.AddSingleton(new ProcessorService(defaultProcessors));

// Registering file service
builder.Services.AddSingleton<FileService>();

var app = builder.Build();

// Configure the HTTP request pipeline and the exception handling
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

