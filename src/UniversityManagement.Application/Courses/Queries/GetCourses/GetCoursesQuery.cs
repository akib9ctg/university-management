using MediatR;
using UniversityManagement.Application.Common.Models;

namespace UniversityManagement.Application.Courses.Queries.GetCourses
{
    public sealed record GetCoursesQuery(GetCoursesRequest Request) : IRequest<PaginatedResult<CourseResponse>>;
}
