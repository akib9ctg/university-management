using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityManagement.Application.Common.Interfaces;
using UniversityManagement.Application.Users.Interfaces;
using UniversityManagement.Infrastructure.Common.Interfaces;
using UniversityManagement.Infrastructure.Persistence;
using UniversityManagement.Infrastructure.Repository;
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


            return services;
        }
    }
}
