using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityManagement.Domain.Entities;
using UniversityManagement.Infrastructure.Common.Interfaces;

namespace UniversityManagement.Infrastructure.Database.Persistence
{
    public sealed class ApplicationDbContext
    : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Class> Classes => Set<Class>();
        public DbSet<CourseClass> CourseClasses => Set<CourseClass>();
        public DbSet<UserCourse> UserCourses => Set<UserCourse>();
        public DbSet<UserCourseClass> UserCourseClasses => Set<UserCourseClass>();

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            modelBuilder.Entity<User>()
                .Property(user => user.Role)
                .HasConversion<string>();

        }
    }
}
