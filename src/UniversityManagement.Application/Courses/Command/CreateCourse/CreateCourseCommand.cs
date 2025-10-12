using MediatR;


namespace UniversityManagement.Application.Courses.Command.CreateCourse
{
    public record CreateCourseCommand(CreateCourseRequest createCourseRequest) : IRequest<CourseResponse>;

}
