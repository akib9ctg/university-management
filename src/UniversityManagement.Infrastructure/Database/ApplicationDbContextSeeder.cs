using System.Linq;
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
                FirstName = "Staff",
                LastName = "User",
                Email = "admin@university.com",
                PasswordHash = PasswordHasher.Hash("Admin123!"),
                Role = UserRole.Staff
            };

            var studentAlice = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Alice",
                LastName = "Johnson",
                Email = "alice@student.com",
                PasswordHash = PasswordHasher.Hash("Student123!"),
                Role = UserRole.Student
            };

            var studentBob = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Bob",
                LastName = "Martinez",
                Email = "bob@student.com",
                PasswordHash = PasswordHasher.Hash("Student123!"),
                Role = UserRole.Student
            };

            var programmingCourse = new Course
            {
                Id = Guid.NewGuid(),
                Name = "CSE",
                Description = "Computer science and engineering"
            };

            var businessCourse = new Course
            {
                Id = Guid.NewGuid(),
                Name = "BBA",
                Description = "Foundations of business administration."
            };

            var mathCourse = new Course
            {
                Id = Guid.NewGuid(),
                Name = "MATH",
                Description = "Foundations of Math."
            };
            var programmingClass = new Class
            {
                Id = Guid.NewGuid(),
                Name = "CSE1101",
                Description = "Programming language"
            };

            var foundationBBAClass = new Class
            {
                Id = Guid.NewGuid(),
                Name = "BBA1101",
                Description = "Fundamental of BBA"
            };

            var mathClass = new Class
            {
                Id = Guid.NewGuid(),
                Name = "Math1101",
                Description = "Morning session for Math 101."
            };

            var courseClasses = new List<CourseClass>
            {
                new()
                {
                    CourseId = programmingCourse.Id,
                    Course = programmingCourse,
                    ClassId = programmingClass.Id,
                    Class = programmingClass
                },
                new()
                {
                    CourseId = businessCourse.Id,
                    Course = businessCourse,
                    ClassId = foundationBBAClass.Id,
                    Class = foundationBBAClass
                },
                new()
                {
                    CourseId = mathCourse.Id,
                    Course = mathCourse,
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
                    UserId = studentBob.Id,
                    User = studentBob,
                    CourseId = businessCourse.Id,
                    Course = businessCourse,
                    AssignedByUserId = staffUser.Id
                }
            };

            var now = DateTime.UtcNow;

            var userCourseClasses = new List<UserCourseClass>();

            foreach (var userCourse in userCourses)
            {
                var classesForCourse = courseClasses
                    .Where(cc => cc.CourseId == userCourse.CourseId)
                    .Select(cc => cc.Class);

                foreach (var @class in classesForCourse)
                {
                    userCourseClasses.Add(new UserCourseClass
                    {
                        UserId = userCourse.UserId,
                        User = userCourse.User,
                        CourseId = userCourse.CourseId,
                        Course = userCourse.Course,
                        ClassId = @class.Id,
                        Class = @class,
                        AssignedByUserId = staffUser.Id,
                        AssignedAt = now
                    });
                }
            }

            foreach (var student in new[] { studentAlice, studentBob })
            {
                var userCourse = userCourses.First(course => course.UserId == student.Id);
                var mathCourseClass = courseClasses.First(c => c.Class.Name == "Math1101");
                userCourseClasses.Add(new UserCourseClass
                {
                    UserId = student.Id,
                    User = student,
                    CourseId = mathCourseClass.CourseId,
                    Course = mathCourseClass.Course,
                    ClassId = mathCourseClass.ClassId,
                    Class = mathCourseClass.Class,
                    AssignedByUserId = staffUser.Id,
                    AssignedAt = now
                });
            }

            await context.Users.AddRangeAsync(new[] { staffUser, studentAlice, studentBob }, cancellationToken);
            await context.Courses.AddRangeAsync(new[] { programmingCourse, businessCourse }, cancellationToken);
            await context.Classes.AddRangeAsync(new[] { programmingClass, foundationBBAClass, mathClass }, cancellationToken);
            await context.CourseClasses.AddRangeAsync(courseClasses, cancellationToken);
            await context.UserCourses.AddRangeAsync(userCourses, cancellationToken);
            await context.UserCourseClasses.AddRangeAsync(userCourseClasses, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
