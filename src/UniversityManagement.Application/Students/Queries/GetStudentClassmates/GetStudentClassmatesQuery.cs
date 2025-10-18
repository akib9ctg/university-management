using MediatR;

namespace UniversityManagement.Application.Students.Queries.GetStudentClassmates
{
    public sealed record GetStudentClassmatesQuery(Guid StudentId)
        : IRequest<List<StudentClassmatesResponse>>;
}
