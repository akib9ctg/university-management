using Microsoft.EntityFrameworkCore;
using Serilog;
using UniversityManagement.API.Middleware;
using UniversityManagement.Infrastructure.Database;
using UniversityManagement.Infrastructure.Database.Persistence;

namespace UniversityManagement.API.Extensions;

public static class WebApplicationExtensions
{
    public static async Task MigrateDatabaseAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.MigrateAsync();
        await ApplicationDbContextSeeder.SeedAsync(dbContext);
    }

    public static WebApplication ConfigureRequestPipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.RoutePrefix = "swagger";
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "University management V1");
        });

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseSerilogRequestLogging();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapGet("/", () => Results.Ok("University Management API is running."));

        app.MapControllers();

        return app;
    }
}
