using MediatR;
using UniversityManagement.Application.Classes;

namespace UniversityManagement.Application.Courses.Queries.GetCourseClasses
{
    public sealed record GetCourseClassesQuery(Guid CourseId) : IRequest<List<ClassResponse>>;
}
