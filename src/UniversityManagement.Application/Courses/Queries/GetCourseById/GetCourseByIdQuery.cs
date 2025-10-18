using MediatR;

namespace UniversityManagement.Application.Courses.Queries.GetCourseById
{
    public sealed record GetCourseByIdQuery(Guid Id) : IRequest<CourseResponse>;
}
