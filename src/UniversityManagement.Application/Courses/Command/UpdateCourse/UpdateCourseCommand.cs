using MediatR;

namespace UniversityManagement.Application.Courses.Command.UpdateCourse
{
    public sealed record UpdateCourseCommand(UpdateCourseRequest updateCourseRequest) : IRequest<CourseResponse>;
}
