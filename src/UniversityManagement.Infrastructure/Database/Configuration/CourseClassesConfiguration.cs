using Microsoft.EntityFrameworkCore;
using UniversityManagement.Domain.Entities;

namespace UniversityManagement.Infrastructure.Database.Configuration
{
    public class CourseClassesConfiguration : BaseEntityTypeConfiguration<CourseClass>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<CourseClass> builder)
        {
            base.Configure(builder);
            builder.ToTable("CourseClasses");

            builder.HasIndex(cc => new { cc.CourseId, cc.ClassId }).IsUnique();

            builder.HasOne(cc => cc.Course)
                .WithMany(c => c.CourseClasses)
                .HasForeignKey(cc => cc.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cc => cc.Class)
                .WithMany(c => c.CourseClasses)
                .HasForeignKey(cc => cc.ClassId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
