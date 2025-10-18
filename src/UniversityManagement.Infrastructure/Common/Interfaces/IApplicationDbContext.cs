using Microsoft.EntityFrameworkCore;
using UniversityManagement.Domain.Entities;

namespace UniversityManagement.Infrastructure.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; }
        DbSet<Course> Courses { get; }
        public DbSet<Class> Classes { get; }
        public DbSet<CourseClass> CourseClasses { get; }
        public DbSet<UserCourse> UserCourses { get; }
        public DbSet<UserCourseClass> UserCourseClasses { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
