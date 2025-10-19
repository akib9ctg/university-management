using Serilog;
using UniversityManagement.API.Extensions;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureSerilog();
builder.ConfigureApplicationServices();

var app = builder.Build();

await app.MigrateDatabaseAsync();
app.ConfigureRequestPipeline();

await app.RunAsync();
