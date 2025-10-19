using UniversityManagement.Application.Courses.Command.CreateCourse;
using UniversityManagement.Infrastructure.Database.Repository;

namespace UniversityManagement.UnitTests.Courses
{
    public class CreateCourseCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Creates_Course_And_Returns_Response()
        {
            using var dbContext = TestDbContextFactory.Create();
            var repository = new CourseRepository(dbContext);
            var handler = new CreateCourseCommandHandler(repository);
            var request = new CreateCourseRequest("CSE", "Computer science and engineering");

            var result = await handler.Handle(new CreateCourseCommand(request), CancellationToken.None);

            var storedCourse = dbContext.Courses.FirstOrDefault();

            Assert.NotNull(storedCourse);
            Assert.Equal(storedCourse.Id, result.Id);
            Assert.Equal(request.name, storedCourse.Name);
            Assert.Equal(storedCourse.Name, result.Name);
        }
    }
}

