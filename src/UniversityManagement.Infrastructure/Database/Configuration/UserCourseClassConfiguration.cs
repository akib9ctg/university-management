using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityManagement.Domain.Entities;

namespace UniversityManagement.Infrastructure.Database.Configuration
{
    public class UserCourseClassConfiguration : BaseEntityTypeConfiguration<UserCourseClass>
    {
        public override void Configure(EntityTypeBuilder<UserCourseClass> builder)
        {
            base.Configure(builder);

            builder.ToTable("UserCourseClass");
            builder.HasIndex(x => new { x.UserId, x.CourseId, x.ClassId })
                  .IsUnique();


            builder.HasOne(cc => cc.Course)
                .WithMany(c => c.UserCoursesClasses)
                .HasForeignKey(cc => cc.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cc => cc.Class)
                .WithMany(c => c.UserCourseClasses)
                .HasForeignKey(cc => cc.ClassId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasOne(cc => cc.User)
                .WithMany(c => c.UserCoursesClass)
                .HasForeignKey(cc => cc.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasOne(uc => uc.AssignedByUser)
                   .WithMany()
                   .HasForeignKey(uc => uc.AssignedByUserId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
