using Microsoft.EntityFrameworkCore;
using UniversityManagement.Application.Common.Interfaces;
using UniversityManagement.Domain.Common;
using UniversityManagement.Domain.Entities;
using UniversityManagement.Infrastructure.Common.Interfaces;

namespace UniversityManagement.Infrastructure.Database.Persistence
{
    public sealed class ApplicationDbContext
        : DbContext, IApplicationDbContext
    {
        private readonly ICurrentUserService? _currentUserService;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService? currentUserService)
            : base(options)
        {
            _currentUserService = currentUserService;
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Class> Classes => Set<Class>();
        public DbSet<CourseClass> CourseClasses => Set<CourseClass>();
        public DbSet<UserCourse> UserCourses => Set<UserCourse>();
        public DbSet<UserCourseClass> UserCourseClasses => Set<UserCourseClass>();

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var currentUserId = _currentUserService?.GetUserId();
            var utcNow = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<BaseEntity>().Where(e => e.State is EntityState.Added or EntityState.Modified))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = utcNow;

                    entry.Entity.CreatedById = currentUserId;

                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.ModifiedAt = utcNow;
                    entry.Entity.ModifiedById = currentUserId;
                }
            }

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
