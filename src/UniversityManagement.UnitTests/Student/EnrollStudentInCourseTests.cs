using UniversityManagement.Application.Students.Commands.EnrollStudentInCourse;
using UniversityManagement.Domain.Entities;
using UniversityManagement.Domain.Enums;
using UniversityManagement.Infrastructure.Database.Repository;

namespace UniversityManagement.UnitTests.Student
{
    public class EnrollStudentInCourseTests
    {
        [Fact]
        public async Task Enrolling_In_Course_Enrolls_In_All_Its_Classes()
        {
            using var dbContext = TestDbContextFactory.Create();
            var userRepository = new UserRepository(dbContext);
            var courseRepository = new CourseRepository(dbContext);

            var course = new Course { Id = Guid.NewGuid(), Name = "CSE" };
            var classA = new Class { Id = Guid.NewGuid(), Name = "Math-101" };
            var classB = new Class { Id = Guid.NewGuid(), Name = "Econ-101" };
            var courseClassA = new CourseClass { CourseId = course.Id, Course = course, ClassId = classA.Id, Class = classA };
            var courseClassB = new CourseClass { CourseId = course.Id, Course = course, ClassId = classB.Id, Class = classB };
            var student = new User
            {
                Id = Guid.NewGuid(),
                Email = "akib9ctg@gmail.com",
                FirstName = "Ahasanul Kalam",
                LastName = "Akib",
                Role = UserRole.Student
            };

            dbContext.Courses.Add(course);
            dbContext.Classes.AddRange(classA, classB);
            dbContext.CourseClasses.AddRange(courseClassA, courseClassB);
            dbContext.Users.Add(student);
            await dbContext.SaveChangesAsync();

            var handler = new EnrollStudentInCourseCommandHandler(userRepository, courseRepository);
            var command = new EnrollStudentInCourseCommand(new EnrollStudentInCourseRequest(student.Id, course.Id, null));

            await handler.Handle(command, default);

            var enrollments = dbContext.UserCourseClasses.Where(x => x.UserId == student.Id).ToList();

            Assert.Equal(2, enrollments.Count);
            Assert.Contains(enrollments, enrollment => enrollment.ClassId == classA.Id);
            Assert.Contains(enrollments, enrollment => enrollment.ClassId == classB.Id);
        }
    }
}

