using Microsoft.EntityFrameworkCore;
using UniversityManagement.API;
using UniversityManagement.Application;
using UniversityManagement.Infrastructure;
using UniversityManagement.Infrastructure.Database.Persistence;
using UniversityManagement.Domain.Enums;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration;
builder.Services.AddSingleton<IConfiguration>(configuration);
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient()
                .AddCors()
                .AddControllers();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(PolicyNames.StaffOnly, policy => policy.RequireRole(UserRole.Staff.ToString()));
    options.AddPolicy(PolicyNames.StudentOnly, policy => policy.RequireRole(UserRole.Student.ToString()));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "University management V1");
        c.RoutePrefix = string.Empty;
    });
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
