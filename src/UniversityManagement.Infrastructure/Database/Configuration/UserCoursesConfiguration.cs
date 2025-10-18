using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityManagement.Domain.Entities;

namespace UniversityManagement.Infrastructure.Database.Configuration
{
    internal class UserCoursesConfiguration : BaseEntityTypeConfiguration<UserCourse>
    {
        public override void Configure(EntityTypeBuilder<UserCourse> builder)
        {
            base.Configure(builder);
            builder.ToTable("UserCourses");
            builder.HasIndex(uc => new { uc.UserId, uc.CourseId }).IsUnique();

            builder.HasOne(uc => uc.User)
                   .WithMany(u => u.UserCourses)
                   .HasForeignKey(uc => uc.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(uc => uc.Course)
                   .WithMany(c => c.UserCourses)
                   .HasForeignKey(uc => uc.CourseId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(uc => uc.AssignedByUser)
                   .WithMany()
                   .HasForeignKey(uc => uc.AssignedByUserId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
