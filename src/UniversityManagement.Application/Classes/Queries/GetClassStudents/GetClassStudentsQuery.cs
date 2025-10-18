using MediatR;
using UniversityManagement.Application.Students;

namespace UniversityManagement.Application.Classes.Queries.GetClassStudents
{
    public sealed record GetClassStudentsQuery(Guid ClassId) : IRequest<List<StudentResponse>>;
}
