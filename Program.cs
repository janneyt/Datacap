using Datacap.Models;
using Datacap.Middleware; // add this

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Sends the filepaths down via dependency injection, no more debugging file path typos!
builder.Services.Configure<FilePaths>(builder.Configuration.GetSection("FilePaths"));


var app = builder.Build();

// Get the logger
var logger = app.Logger;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage(); // Use this in development for detailed exception information
}
else
{
    app.UseMiddleware<ExceptionMiddleware>(); // Use this instead
    // Log the fact that we're in production mode
    logger.LogInformation("Application is in production mode");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
