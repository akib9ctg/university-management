using MediatR;

namespace UniversityManagement.Application.Courses.Command.AssignClassToCourse
{
    public sealed record AssignClassToCourseCommand(Guid CourseId, Guid ClassId) : IRequest<bool>;
}
