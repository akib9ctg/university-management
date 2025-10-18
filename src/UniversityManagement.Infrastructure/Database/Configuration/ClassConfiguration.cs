using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityManagement.Domain.Entities;

namespace UniversityManagement.Infrastructure.Database.Configuration
{
    public sealed class ClassConfiguration : BaseEntityTypeConfiguration<Class>
    {
        public override void Configure(EntityTypeBuilder<Class> builder)
        {
            base.Configure(builder);

            builder.ToTable("Classes");

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasMany(x => x.CourseClasses)
                .WithOne(cc => cc.Class)
                .HasForeignKey(cc => cc.ClassId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
