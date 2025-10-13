using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UniversityManagement.Application.Classes.Interfaces;
using UniversityManagement.Application.Common.Interfaces;
using UniversityManagement.Application.Courses.Interfaces;
using UniversityManagement.Application.Users.Interfaces;
using UniversityManagement.Infrastructure.Common.Interfaces;
using UniversityManagement.Infrastructure.Database.Persistence;
using UniversityManagement.Infrastructure.Database.Repository;
using UniversityManagement.Infrastructure.Services.Identity;

namespace UniversityManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IApplicationDbContext>(provider =>
                provider.GetRequiredService<ApplicationDbContext>());


            services.AddScoped<IJwtService, JwtService>();

            // repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IClassRepository, ClassRepository>();

            return services;
        }
    }
}
