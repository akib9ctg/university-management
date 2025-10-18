using MediatR;
using UniversityManagement.Application.Courses;

namespace UniversityManagement.Application.Classes.Queries.GetClassCourses
{
    public sealed record GetClassCoursesQuery(Guid ClassId) : IRequest<List<CourseResponse>>;
}
