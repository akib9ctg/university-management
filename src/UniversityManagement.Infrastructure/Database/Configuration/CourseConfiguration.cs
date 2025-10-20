using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityManagement.Domain.Entities;

namespace UniversityManagement.Infrastructure.Database.Configuration
{
    public class CourseConfiguration : BaseEntityTypeConfiguration<Course>
    {
        public override void Configure(EntityTypeBuilder<Course> builder)
        {
            base.Configure(builder);
            builder.ToTable("Courses", table =>
            {
                table.HasCheckConstraint(
                    "CK_Courses_Name_Alphanumeric",
                    "\"Name\" ~ '^[A-Za-z0-9]+$'");
            });

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(c => c.Description)
                .HasMaxLength(1000);
            builder.HasIndex(c => c.Name)
                .IsUnique();

            builder.HasMany(c => c.CourseClasses)
                .WithOne(cc => cc.Course)
                .HasForeignKey(cc => cc.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.UserCourses)
                .WithOne(uc => uc.Course)
                .HasForeignKey(uc => uc.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
