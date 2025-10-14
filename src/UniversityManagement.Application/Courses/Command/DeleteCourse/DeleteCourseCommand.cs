using MediatR;

namespace UniversityManagement.Application.Courses.Command.DeleteCourse
{
    public sealed record DeleteCourseCommand(Guid Id) : IRequest<bool>;
}
