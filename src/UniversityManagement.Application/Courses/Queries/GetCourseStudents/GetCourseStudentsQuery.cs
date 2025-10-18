using MediatR;
using UniversityManagement.Application.Students;

namespace UniversityManagement.Application.Courses.Queries.GetCourseStudents
{
    public sealed record GetCourseStudentsQuery(Guid CourseId) : IRequest<List<StudentResponse>>;
}
