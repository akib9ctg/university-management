using MediatR;

namespace UniversityManagement.Application.Students.Queries.GetStudentCoursesAndClasses
{
    public sealed record GetStudentCoursesAndClassesQuery(Guid StudentId)
        : IRequest<List<StudentCourseClassResponse>>;
}
