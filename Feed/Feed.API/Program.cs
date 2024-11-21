using Feed.API;
using Feed.Core;
using Feed.Persistence;
using Oakton;
using Wolverine;
using Wolverine.FluentValidation;
using Wolverine.Http;
using Wolverine.Http.FluentValidation;

var builder = WebApplication.CreateBuilder(args);

//Setup to use multiple apsettings based om env variable
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddWolverineHttp();

builder.Services.UsePersistence(builder.Configuration.GetConnectionString("DefaultConnection")!);

builder.Services.AddAuthorization();
builder.Services.AddAuthentication();

builder.Host.UseWolverine(opts =>
{
    opts.Discovery.IncludeAssembly(typeof(AssemblyInfo).Assembly);

    opts.UseFluentValidation();

    //Wolverine is built and optimized for asynchronous messaging
    //since I use it for mediator pattern only I can turn down some overhead
    opts.Durability.Mode = DurabilityMode.MediatorOnly;
});

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() 
    || app.Environment.EnvironmentName == "docker")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapWolverineEndpoints(opts =>
{
    //Directs Wolverine.HTTP to use Fluent Validation middleware
    //to validate any request bodies where there's a known
    //validator (or many validators)
    opts.UseFluentValidationProblemDetailMiddleware();

    //here you have the possiniliy to add further middleware
    //Example:
    //opts.AddMiddleware(typeof(MyMiddleware));
});

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment()
    || app.Environment.EnvironmentName == "docker")
{
    app.SeedDatabase();
}

return await app.RunOaktonCommands(args);

//Necessary for IntegrationTest project
public partial class Program
{

}