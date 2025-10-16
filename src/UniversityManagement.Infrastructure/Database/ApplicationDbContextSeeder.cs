using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UniversityManagement.Application.Common.Utilities;
using UniversityManagement.Domain.Entities;
using UniversityManagement.Domain.Enums;
using UniversityManagement.Infrastructure.Database.Persistence;

namespace UniversityManagement.Infrastructure.Database
{
    public static class ApplicationDbContextSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken = default)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var hasUsers = await context.Users.AnyAsync(cancellationToken);
            var hasCourses = await context.Courses.AnyAsync(cancellationToken);
            var hasClasses = await context.Classes.AnyAsync(cancellationToken);

            if (hasUsers || hasCourses || hasClasses)
            {
                return;
            }


            var staffUser = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Admin",
                LastName = "User",
                Email = "admin@university.local",
                PasswordHash = PasswordHasher.Hash("Admin123!"),
                Role = UserRole.Staff
            };

            var studentAlice = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Alice",
                LastName = "Johnson",
                Email = "alice@student.local",
                PasswordHash = PasswordHasher.Hash("Student123!"),
                Role = UserRole.Student
            };

            var studentBob = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Bob",
                LastName = "Martinez",
                Email = "bob@student.local",
                PasswordHash = PasswordHasher.Hash("Student123!"),
                Role = UserRole.Student
            };

            var programmingCourse = new Course
            {
                Id = Guid.NewGuid(),
                Name = "Programming 101",
                Description = "Introduction to programming fundamentals."
            };

            var businessCourse = new Course
            {
                Id = Guid.NewGuid(),
                Name = "Business Basics",
                Description = "Foundations of business administration."
            };

            var mathClass = new Class
            {
                Id = Guid.NewGuid(),
                Name = "Math 101 - Morning",
                Description = "Morning session for Math 101."
            };

            var programmingLabClass = new Class
            {
                Id = Guid.NewGuid(),
                Name = "Programming Lab - Afternoon",
                Description = "Hands-on programming lab session."
            };

            var courseClasses = new List<CourseClass>
            {
                new()
                {
                    CourseId = programmingCourse.Id,
                    Course = programmingCourse,
                    ClassId = programmingLabClass.Id,
                    Class = programmingLabClass
                },
                new()
                {
                    CourseId = businessCourse.Id,
                    Course = businessCourse,
                    ClassId = mathClass.Id,
                    Class = mathClass
                }
            };

            var userCourses = new List<UserCourse>
            {
                new()
                {
                    UserId = studentAlice.Id,
                    User = studentAlice,
                    CourseId = programmingCourse.Id,
                    Course = programmingCourse,
                    AssignedByUserId = staffUser.Id
                },
                new()
                {
                    UserId = studentAlice.Id,
                    User = studentAlice,
                    CourseId = businessCourse.Id,
                    Course = businessCourse,
                    AssignedByUserId = staffUser.Id
                },
                new()
                {
                    UserId = studentBob.Id,
                    User = studentBob,
                    CourseId = programmingCourse.Id,
                    Course = programmingCourse,
                    AssignedByUserId = staffUser.Id
                }
            };

            var now = DateTime.UtcNow;

            var userCourseClasses = new List<UserCourseClass>
            {
                new()
                {
                    UserId = studentAlice.Id,
                    User = studentAlice,
                    CourseId = programmingCourse.Id,
                    Course = programmingCourse,
                    ClassId = programmingLabClass.Id,
                    Class = programmingLabClass,
                    AssignedByUserId = staffUser.Id,
                    AssignedAt = now
                },
                new()
                {
                    UserId = studentAlice.Id,
                    User = studentAlice,
                    CourseId = businessCourse.Id,
                    Course = businessCourse,
                    ClassId = mathClass.Id,
                    Class = mathClass,
                    AssignedByUserId = staffUser.Id,
                    AssignedAt = now
                },
                new()
                {
                    UserId = studentBob.Id,
                    User = studentBob,
                    CourseId = programmingCourse.Id,
                    Course = programmingCourse,
                    ClassId = programmingLabClass.Id,
                    Class = programmingLabClass,
                    AssignedByUserId = staffUser.Id,
                    AssignedAt = now
                }
            };

            await context.Users.AddRangeAsync(new[] { staffUser, studentAlice, studentBob }, cancellationToken);
            await context.Courses.AddRangeAsync(new[] { programmingCourse, businessCourse }, cancellationToken);
            await context.Classes.AddRangeAsync(new[] { mathClass, programmingLabClass }, cancellationToken);
            await context.CourseClasses.AddRangeAsync(courseClasses, cancellationToken);
            await context.UserCourses.AddRangeAsync(userCourses, cancellationToken);
            await context.UserCourseClasses.AddRangeAsync(userCourseClasses, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
